using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

public enum LoiDatLich { Khong, KhongTimThayLich, CaDaBatDau, DaCoLichHen, KhongTimThayKhach }

public sealed class KetQuaDatLich
{
    public LoiDatLich Loi { get; init; }
    public string MaLichHen { get; init; } = "";
    public string MaKh { get; init; } = "";
    /// <summary>Mã lịch hẹn còn hiệu lực đã có của khách trong ca này (Q6).</summary>
    public string? MaTrung { get; init; }
}

/// <summary>
/// Bảng Lich_hen. Giờ hiện tại do BUS truyền vào (giờ máy chạy ứng dụng), không dùng GETDATE của SQL Server.
/// </summary>
public class LichHenDAL
{
    /// <summary>
    /// Các lịch làm việc còn đặt được trong một ngày (ca chưa bắt đầu, RB16), lọc theo khoa và bác sĩ.
    /// SoLichHenConHieuLuc cho lễ tân biết ca đã đông hay vắng.
    /// </summary>
    public List<LichLamViecDTO> CaConDat(DateTime ngay, string? maKhoa, string? maNv, DateTime bayGio)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(@"
SELECT l.ma_lich_lam_viec, l.ngay_lam_viec, c.ma_ca, c.ten_ca, c.gio_bat_dau, c.gio_ket_thuc,
       n.ma_nv, n.ho_ten_nv, k.ma_khoa, k.ten_khoa, p.ma_phong, p.ten_phong,
       (SELECT COUNT(*) FROM Lich_hen h WHERE h.ma_lich_lam_viec = l.ma_lich_lam_viec
            AND h.trang_thai IN (N'Đã đặt', N'Đã xác nhận')) AS so_hen_con
FROM Lich_lam_viec l
JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca
JOIN Nhan_vien n ON n.ma_nv = l.ma_nv
JOIN Phong_lam_viec p ON p.ma_phong = l.ma_phong
JOIN Khoa k ON k.ma_khoa = n.ma_khoa
WHERE l.ngay_lam_viec = @ngay
  AND (l.ngay_lam_viec > @homNay OR (l.ngay_lam_viec = @homNay AND c.gio_bat_dau > @gioNay))
  AND (@khoa IS NULL OR n.ma_khoa = @khoa) AND (@nv IS NULL OR l.ma_nv = @nv)
ORDER BY c.gio_bat_dau, n.ho_ten_nv")
            .P("@ngay", SqlDbType.Date, 0, ngay.Date)
            .P("@homNay", SqlDbType.Date, 0, bayGio.Date).P("@gioNay", SqlDbType.Time, 0, bayGio.TimeOfDay)
            .P("@khoa", SqlDbType.VarChar, 10, Rong(maKhoa)).P("@nv", SqlDbType.VarChar, 10, Rong(maNv));
        var ds = new List<LichLamViecDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new LichLamViecDTO
            {
                MaLichLamViec = r.GetString(0), NgayLamViec = r.GetDateTime(1), MaCa = r.GetString(2), TenCa = r.GetString(3),
                GioBatDau = r.GetTimeSpan(4), GioKetThuc = r.GetTimeSpan(5), MaNv = r.GetString(6), TenBacSi = r.GetString(7),
                MaKhoa = r.GetString(8), TenKhoa = r.GetString(9), MaPhong = r.GetString(10), TenPhong = r.GetString(11),
                SoLichHenConHieuLuc = r.GetInt32(12),
            });
        return ds;
    }

    /// <summary>
    /// Lễ tân đặt lịch hộ trong một transaction: (thêm khách mới nếu có) → khóa lịch làm việc (UPDLOCK) → kiểm tra
    /// ca chưa bắt đầu (RB16) và khách chưa có lịch còn hiệu lực ở ca này (Q6) → INSERT "Đã đặt" (trigger RB14 bắt buộc)
    /// → UPDATE "Đã xác nhận" ghi lễ tân (RB15 cho phép Đã đặt → Đã xác nhận).
    /// </summary>
    public KetQuaDatLich DatHo(string? maKh, KhachHangDTO? khachMoi, string maLichLamViec, string? trieuChung,
        DateTime bayGio, string maLeTan) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        DateTime ngay; TimeSpan gioBd;
        using (var c = conn.Lenh(@"
SELECT l.ngay_lam_viec, c.gio_bat_dau FROM Lich_lam_viec l WITH (UPDLOCK, ROWLOCK)
JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca WHERE l.ma_lich_lam_viec = @l", tx).P("@l", SqlDbType.VarChar, 10, maLichLamViec))
        using (var r = c.ExecuteReader())
        {
            if (!r.Read()) return new KetQuaDatLich { Loi = LoiDatLich.KhongTimThayLich };
            ngay = r.GetDateTime(0);
            gioBd = r.GetTimeSpan(1);
        }
        if (bayGio >= ngay.Date + gioBd) return new KetQuaDatLich { Loi = LoiDatLich.CaDaBatDau };

        if (khachMoi is not null) maKh = KhachHangDAL.Chen(conn, tx, khachMoi);
        else
        {
            using var c = conn.Lenh("SELECT COUNT(*) FROM Khach_hang WHERE ma_kh = @kh", tx).P("@kh", SqlDbType.VarChar, 10, maKh);
            if ((int)c.ExecuteScalar()! == 0) return new KetQuaDatLich { Loi = LoiDatLich.KhongTimThayKhach };

            // Dòng lịch làm việc đã bị khóa ở trên nên hai lần đặt cùng ca phải chờ nhau.
            using var t = conn.Lenh(@"
SELECT TOP 1 ma_lich_hen FROM Lich_hen
WHERE ma_kh = @kh AND ma_lich_lam_viec = @l AND trang_thai IN (N'Đã đặt', N'Đã xác nhận')", tx)
                .P("@kh", SqlDbType.VarChar, 10, maKh).P("@l", SqlDbType.VarChar, 10, maLichLamViec);
            if (t.ExecuteScalar() is string trung) return new KetQuaDatLich { Loi = LoiDatLich.DaCoLichHen, MaTrung = trung, MaKh = maKh! };
        }

        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.LichHen);
        using (var c = conn.Lenh(@"
INSERT INTO Lich_hen (ma_lich_hen, ngay_gio_dat, trieu_chung_ban_dau, trang_thai, ma_kh, ma_lich_lam_viec, ma_nv)
VALUES (@ma, @dat, @tc, N'Đã đặt', @kh, @l, NULL)", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@dat", SqlDbType.DateTime, 0, bayGio)
            .P("@tc", SqlDbType.NVarChar, 255, trieuChung)
            .P("@kh", SqlDbType.VarChar, 10, maKh).P("@l", SqlDbType.VarChar, 10, maLichLamViec))
            c.ExecuteNonQuery();
        using (var c = conn.Lenh("UPDATE Lich_hen SET trang_thai = N'Đã xác nhận', ma_nv = @lt WHERE ma_lich_hen = @ma", tx)
            .P("@lt", SqlDbType.VarChar, 10, maLeTan).P("@ma", SqlDbType.VarChar, 10, ma))
            c.ExecuteNonQuery();
        return new KetQuaDatLich { MaLichHen = ma, MaKh = maKh! };
    });

    private static string? Rong(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
