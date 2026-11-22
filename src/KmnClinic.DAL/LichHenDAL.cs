using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

public enum LoiDatLich { Khong, KhongTimThayLich, CaDaBatDau, DaCoLichHen, KhongTimThayKhach }

/// <summary>Lý do không xác nhận, hủy, tiếp nhận được lịch hẹn. BUS đổi thành câu thông báo.</summary>
public enum LoiLichHen { Khong, KhongTimThay, SaiTrangThai, CaDaKetThuc, KhacNgay }

public sealed class KetQuaLichHen
{
    public LoiLichHen Loi { get; init; }
    /// <summary>Trạng thái hiện tại (khi SaiTrangThai), ngày hẹn (khi KhacNgay), mã hồ sơ mới (khi tiếp nhận thành công).</summary>
    public string? TrangThai { get; init; }
    public DateTime? Ngay { get; init; }
    public string? MaHoSo { get; init; }
}

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

    private const string SqlLichHen = @"
SELECT h.ma_lich_hen, h.ngay_gio_dat, h.trieu_chung_ban_dau, h.trang_thai,
       k.ma_kh, k.ho_ten_kh, k.sdt_kh,
       l.ma_lich_lam_viec, l.ngay_lam_viec, c.ten_ca, c.gio_bat_dau, c.gio_ket_thuc,
       n.ma_nv, n.ho_ten_nv, kh.ten_khoa, p.ten_phong, lt.ho_ten_nv, hs.ma_ho_so
FROM Lich_hen h
JOIN Khach_hang k ON k.ma_kh = h.ma_kh
JOIN Lich_lam_viec l ON l.ma_lich_lam_viec = h.ma_lich_lam_viec
JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca
JOIN Nhan_vien n ON n.ma_nv = l.ma_nv
JOIN Khoa kh ON kh.ma_khoa = n.ma_khoa
JOIN Phong_lam_viec p ON p.ma_phong = l.ma_phong
LEFT JOIN Nhan_vien lt ON lt.ma_nv = h.ma_nv
LEFT JOIN Ho_so_kham hs ON hs.ma_lich_hen = h.ma_lich_hen";

    /// <summary>
    /// Danh sách lịch hẹn cho lễ tân. ngay null = mọi ngày. "Đã đặt" (cần gọi xác nhận) lên đầu, rồi Đã xác nhận, theo giờ ca.
    /// </summary>
    public List<LichHenDTO> DanhSach(DateTime? ngay, string? trangThai, string? tuKhoa, string? maBacSi = null)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlLichHen + @"
WHERE (@ngay IS NULL OR l.ngay_lam_viec = @ngay) AND (@tt IS NULL OR h.trang_thai = @tt) AND (@bs IS NULL OR l.ma_nv = @bs)
  AND (@q IS NULL OR h.ma_lich_hen LIKE @q OR k.ho_ten_kh LIKE @q OR k.sdt_kh LIKE @q OR k.ma_kh LIKE @q)
ORDER BY l.ngay_lam_viec, CASE h.trang_thai WHEN N'Đã đặt' THEN 0 WHEN N'Đã xác nhận' THEN 1 WHEN N'Đã khám' THEN 2 ELSE 3 END,
         c.gio_bat_dau, h.ngay_gio_dat")
            .P("@ngay", SqlDbType.Date, 0, ngay?.Date).P("@tt", SqlDbType.NVarChar, 20, Rong(trangThai))
            .P("@bs", SqlDbType.VarChar, 10, Rong(maBacSi)).P("@q", SqlDbType.NVarChar, 60, TimKiem.MauLike(tuKhoa));
        return DocLichHen(cmd);
    }

    public LichHenDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlLichHen + " WHERE h.ma_lich_hen = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        return DocLichHen(cmd).FirstOrDefault();
    }

    private static List<LichHenDTO> DocLichHen(SqlCommand cmd)
    {
        var ds = new List<LichHenDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new LichHenDTO
            {
                MaLichHen = r.GetString(0), NgayGioDat = r.GetDateTime(1), TrieuChungBanDau = DbHelper.ChuoiHoacNull(r, 2),
                TrangThai = r.GetString(3), MaKh = r.GetString(4), TenKhachHang = r.GetString(5), SdtKhachHang = r.GetString(6),
                MaLichLamViec = r.GetString(7), NgayKham = r.GetDateTime(8), TenCa = r.GetString(9),
                GioBatDau = r.GetTimeSpan(10), GioKetThuc = r.GetTimeSpan(11), MaBacSi = r.GetString(12), TenBacSi = r.GetString(13),
                TenKhoa = r.GetString(14), TenPhong = r.GetString(15), LeTanXacNhan = DbHelper.ChuoiHoacNull(r, 16),
                MaHoSo = DbHelper.ChuoiHoacNull(r, 17),
            });
        return ds;
    }

    /// <summary>Khóa (UPDLOCK) và đọc lịch hẹn kèm ngày, giờ ca. Null nếu không có.</summary>
    internal static (string TrangThai, DateTime Ngay, TimeSpan BatDau, TimeSpan KetThuc, string MaKh, string MaBs)?
        KhoaLichHen(SqlConnection conn, SqlTransaction tx, string ma)
    {
        using var c = conn.Lenh(@"
SELECT h.trang_thai, l.ngay_lam_viec, c.gio_bat_dau, c.gio_ket_thuc, h.ma_kh, l.ma_nv
FROM Lich_hen h WITH (UPDLOCK, ROWLOCK)
JOIN Lich_lam_viec l ON l.ma_lich_lam_viec = h.ma_lich_lam_viec
JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca
WHERE h.ma_lich_hen = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        using var r = c.ExecuteReader();
        if (!r.Read()) return null;
        return (r.GetString(0), r.GetDateTime(1), r.GetTimeSpan(2), r.GetTimeSpan(3), r.GetString(4), r.GetString(5));
    }

    internal static void XacNhanTrongGiaoDich(SqlConnection conn, SqlTransaction tx, string ma, string maLeTan)
    {
        using var c = conn.Lenh("UPDATE Lich_hen SET trang_thai = N'Đã xác nhận', ma_nv = @lt WHERE ma_lich_hen = @ma", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@lt", SqlDbType.VarChar, 10, maLeTan);
        c.ExecuteNonQuery();
    }

    /// <summary>Đã đặt → Đã xác nhận, ghi lễ tân xác nhận (RB15, RB17). Không xác nhận lịch mà ca đã kết thúc.</summary>
    public KetQuaLichHen XacNhan(string ma, string maLeTan, DateTime bayGio) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var h = KhoaLichHen(conn, tx, ma);
        if (h is null) return new KetQuaLichHen { Loi = LoiLichHen.KhongTimThay };
        if (h.Value.TrangThai != TrangThai.DaDat) return new KetQuaLichHen { Loi = LoiLichHen.SaiTrangThai, TrangThai = h.Value.TrangThai };
        if (bayGio >= h.Value.Ngay.Date + h.Value.KetThuc) return new KetQuaLichHen { Loi = LoiLichHen.CaDaKetThuc };
        XacNhanTrongGiaoDich(conn, tx, ma, maLeTan);
        return new KetQuaLichHen();
    });

    /// <summary>Hủy lịch Đã đặt hoặc Đã xác nhận (RB15). Giữ lễ tân đã xác nhận (nếu có) để truy vết.</summary>
    public KetQuaLichHen Huy(string ma) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var h = KhoaLichHen(conn, tx, ma);
        if (h is null) return new KetQuaLichHen { Loi = LoiLichHen.KhongTimThay };
        if (h.Value.TrangThai is not (TrangThai.DaDat or TrangThai.DaXacNhan))
            return new KetQuaLichHen { Loi = LoiLichHen.SaiTrangThai, TrangThai = h.Value.TrangThai };
        using var c = conn.Lenh("UPDATE Lich_hen SET trang_thai = N'Đã hủy' WHERE ma_lich_hen = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        c.ExecuteNonQuery();
        return new KetQuaLichHen();
    });

    private static string? Rong(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
