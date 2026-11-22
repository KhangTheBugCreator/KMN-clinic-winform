using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>
/// Lễ tân tiếp nhận: tạo Ho_so_kham. Trigger TG_Ho_so_kham_KiemTra (RB19, RB20, RB34, RB41) vẫn chặn ở CSDL và tự chuyển
/// lịch hẹn sang "Đã khám"; DAL kiểm tra trước để báo lỗi rõ ràng. Giờ hiện tại do BUS truyền vào.
/// </summary>
public class TiepNhanDAL
{
    /// <summary>Trạng thái hồ sơ suy ra (không có cột trạng thái), giống bản web.</summary>
    public const string BieuThucTrangThai = @"CASE
  WHEN d.trang_thai_thanh_toan = N'Đã thanh toán' THEN N'Đã thanh toán'
  WHEN d.ma_hoa_don IS NOT NULL THEN N'Hoàn tất'
  WHEN hs.chan_doan_benh IS NOT NULL OR EXISTS (SELECT 1 FROM Chi_tiet_don_thuoc x WHERE x.ma_ho_so = hs.ma_ho_so) THEN N'Đang khám'
  ELSE N'Chờ khám' END";

    private const string DieuKienDangTruc = @"l.ngay_lam_viec = @homNay
  AND @gioNay BETWEEN DATEADD(MINUTE, -30, c.gio_bat_dau) AND c.gio_ket_thuc";

    /// <summary>
    /// Tiếp nhận khách có hẹn: chỉ đúng ngày hẹn (Q10). Lịch còn "Đã đặt" thì xác nhận rồi tạo hồ sơ trong cùng transaction (Q3).
    /// Tiền công ghi 0, bác sĩ nhập khi khám.
    /// </summary>
    public KetQuaLichHen TiepNhanCoHen(string maLichHen, string maLeTan, string trieuChung, DateTime bayGio) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var h = LichHenDAL.KhoaLichHen(conn, tx, maLichHen);
        if (h is null) return new KetQuaLichHen { Loi = LoiLichHen.KhongTimThay };
        var (tt, ngay, _, _, maKh, maBs) = h.Value;
        if (tt is not (TrangThai.DaDat or TrangThai.DaXacNhan)) return new KetQuaLichHen { Loi = LoiLichHen.SaiTrangThai, TrangThai = tt };
        if (ngay.Date != bayGio.Date) return new KetQuaLichHen { Loi = LoiLichHen.KhacNgay, Ngay = ngay };
        if (tt == TrangThai.DaDat) LichHenDAL.XacNhanTrongGiaoDich(conn, tx, maLichHen, maLeTan);
        return new KetQuaLichHen { MaHoSo = ThemHoSo(conn, tx, maKh, maBs, maLeTan, maLichHen, trieuChung, bayGio) };
    });

    /// <summary>Khách vãng lai: bác sĩ phải đang trực, tính từ 30 phút trước giờ bắt đầu ca đến hết ca (RB41).</summary>
    public KetQuaVangLai TiepNhanVangLai(string maKh, string maBs, string maLeTan, string trieuChung, DateTime bayGio) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        using (var c = conn.Lenh("SELECT COUNT(*) FROM Khach_hang WHERE ma_kh = @kh", tx).P("@kh", SqlDbType.VarChar, 10, maKh))
            if ((int)c.ExecuteScalar()! == 0) return new KetQuaVangLai { KhongTimThayKhach = true };
        if (!BacSiDangTruc(conn, tx, bayGio).Any(b => b.MaNv == maBs)) return new KetQuaVangLai { BacSiKhongTruc = true };
        return new KetQuaVangLai { MaHoSo = ThemHoSo(conn, tx, maKh, maBs, maLeTan, null, trieuChung, bayGio) };
    });

    private static string ThemHoSo(SqlConnection conn, SqlTransaction tx, string maKh, string maBs, string maLeTan,
        string? maLichHen, string trieuChung, DateTime bayGio)
    {
        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.HoSoKham);
        using var c = conn.Lenh(@"
INSERT INTO Ho_so_kham (ma_ho_so, ngay_gio_kham, trieu_chung, chan_doan_benh, loi_dan, tien_cong_kham, ma_lich_hen, ma_kh, ma_nv_kham, ma_nv_tiep_nhan)
VALUES (@ma, @gio, @tc, NULL, NULL, 0, @lh, @kh, @bs, @lt)", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@gio", SqlDbType.DateTime, 0, bayGio).P("@tc", SqlDbType.NVarChar, 255, trieuChung)
            .P("@lh", SqlDbType.VarChar, 10, maLichHen).P("@kh", SqlDbType.VarChar, 10, maKh)
            .P("@bs", SqlDbType.VarChar, 10, maBs).P("@lt", SqlDbType.VarChar, 10, maLeTan);
        c.ExecuteNonQuery();
        return ma;
    }

    public List<BacSiDangTrucDTO> BacSiDangTruc(DateTime bayGio)
    {
        using var conn = DbHelper.MoKetNoi();
        return BacSiDangTruc(conn, null, bayGio);
    }

    private static List<BacSiDangTrucDTO> BacSiDangTruc(SqlConnection conn, SqlTransaction? tx, DateTime bayGio)
    {
        using var cmd = conn.Lenh($@"
SELECT n.ma_nv, n.ho_ten_nv, k.ten_khoa, p.ten_phong, c.ten_ca, c.gio_bat_dau, c.gio_ket_thuc,
       (SELECT COUNT(*) FROM Ho_so_kham hs WHERE hs.ma_nv_kham = n.ma_nv AND CAST(hs.ngay_gio_kham AS date) = @homNay
          AND NOT EXISTS (SELECT 1 FROM Hoa_don d WHERE d.ma_ho_so = hs.ma_ho_so))
FROM Lich_lam_viec l
JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca
JOIN Nhan_vien n ON n.ma_nv = l.ma_nv
JOIN Khoa k ON k.ma_khoa = n.ma_khoa
JOIN Phong_lam_viec p ON p.ma_phong = l.ma_phong
WHERE {DieuKienDangTruc}
ORDER BY k.ten_khoa, n.ho_ten_nv", tx)
            .P("@homNay", SqlDbType.Date, 0, bayGio.Date).P("@gioNay", SqlDbType.Time, 0, bayGio.TimeOfDay);
        var ds = new List<BacSiDangTrucDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new BacSiDangTrucDTO
            {
                MaNv = r.GetString(0), HoTenNv = r.GetString(1), TenKhoa = r.GetString(2), TenPhong = r.GetString(3),
                TenCa = r.GetString(4), GioBatDau = r.GetTimeSpan(5), GioKetThuc = r.GetTimeSpan(6), SoDangCho = r.GetInt32(7),
            });
        return ds;
    }

    /// <summary>Hồ sơ tiếp nhận trong một ngày, mới nhất trước.</summary>
    public List<HoSoKhamDTO> HoSoTheoNgay(DateTime ngay)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh($@"
SELECT hs.ma_ho_so, hs.ngay_gio_kham, hs.trieu_chung, hs.ma_lich_hen, k.ma_kh, k.ho_ten_kh, k.sdt_kh,
       bs.ma_nv, bs.ho_ten_nv, kh.ten_khoa, lt.ho_ten_nv, {BieuThucTrangThai}
FROM Ho_so_kham hs
JOIN Khach_hang k ON k.ma_kh = hs.ma_kh
JOIN Nhan_vien bs ON bs.ma_nv = hs.ma_nv_kham
JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
JOIN Nhan_vien lt ON lt.ma_nv = hs.ma_nv_tiep_nhan
LEFT JOIN Hoa_don d ON d.ma_ho_so = hs.ma_ho_so
WHERE hs.ngay_gio_kham >= @tu AND hs.ngay_gio_kham < DATEADD(day, 1, @tu)
ORDER BY hs.ngay_gio_kham DESC, hs.ma_ho_so DESC").P("@tu", SqlDbType.DateTime, 0, ngay.Date);
        var ds = new List<HoSoKhamDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new HoSoKhamDTO
            {
                MaHoSo = r.GetString(0), NgayGioKham = r.GetDateTime(1), TrieuChung = r.GetString(2), MaLichHen = DbHelper.ChuoiHoacNull(r, 3),
                MaKh = r.GetString(4), TenKhachHang = r.GetString(5), SdtKhachHang = r.GetString(6), MaBacSi = r.GetString(7),
                TenBacSi = r.GetString(8), TenKhoa = r.GetString(9), TenLeTan = r.GetString(10), TrangThai = r.GetString(11),
            });
        return ds;
    }

    public TongQuanLeTanDTO TongQuan(DateTime bayGio)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh($@"
SELECT
  (SELECT COUNT(*) FROM Lich_hen h JOIN Lich_lam_viec l ON l.ma_lich_lam_viec = h.ma_lich_lam_viec
     JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca
     WHERE h.trang_thai = N'Đã đặt' AND (l.ngay_lam_viec > @homNay OR (l.ngay_lam_viec = @homNay AND c.gio_ket_thuc > @gioNay))),
  (SELECT COUNT(*) FROM Lich_hen h JOIN Lich_lam_viec l ON l.ma_lich_lam_viec = h.ma_lich_lam_viec
     WHERE l.ngay_lam_viec = @homNay AND h.trang_thai <> N'Đã hủy'),
  (SELECT COUNT(*) FROM Lich_hen h JOIN Lich_lam_viec l ON l.ma_lich_lam_viec = h.ma_lich_lam_viec
     WHERE l.ngay_lam_viec = @homNay AND h.trang_thai = N'Đã xác nhận'),
  (SELECT COUNT(*) FROM Ho_so_kham WHERE CAST(ngay_gio_kham AS date) = @homNay),
  (SELECT COUNT(*) FROM Ho_so_kham WHERE CAST(ngay_gio_kham AS date) = @homNay AND ma_lich_hen IS NULL),
  (SELECT COUNT(DISTINCT l.ma_nv) FROM Lich_lam_viec l JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca WHERE {DieuKienDangTruc})")
            .P("@homNay", SqlDbType.Date, 0, bayGio.Date).P("@gioNay", SqlDbType.Time, 0, bayGio.TimeOfDay);
        using var r = cmd.ExecuteReader();
        r.Read();
        return new TongQuanLeTanDTO
        {
            ChoXacNhan = r.GetInt32(0), HenHomNay = r.GetInt32(1), ChoTiepNhan = r.GetInt32(2),
            HoSoHomNay = r.GetInt32(3), VangLaiHomNay = r.GetInt32(4), BacSiDangTruc = r.GetInt32(5),
        };
    }
}

public sealed class KetQuaVangLai
{
    public bool KhongTimThayKhach { get; init; }
    public bool BacSiKhongTruc { get; init; }
    public string? MaHoSo { get; init; }
}
