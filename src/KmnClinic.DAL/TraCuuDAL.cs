using System.Data;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>
/// Tra cứu của quản trị viên (W9), chỉ đọc. Trả DataTable (SqlDataAdapter) có tên cột tiếng Việt.
/// Lọc ngày: lịch hẹn theo ngày làm việc, hồ sơ và hóa đơn theo ngày khám. null = không lọc.
/// </summary>
public class TraCuuDAL
{
    private const int ToiDa = 1000;

    private static SqlParameter[] ThamSo(DateTime? tu, DateTime? den, string? trangThai, string? tuKhoa) =>
    [
        new("@tu", SqlDbType.DateTime) { Value = (object?)tu?.Date ?? DBNull.Value },
        new("@denSau", SqlDbType.DateTime) { Value = (object?)den?.Date.AddDays(1) ?? DBNull.Value },
        new("@tt", SqlDbType.NVarChar, 20) { Value = string.IsNullOrWhiteSpace(trangThai) ? DBNull.Value : trangThai },
        new("@q", SqlDbType.NVarChar, 60) { Value = (object?)TimKiem.MauLike(tuKhoa) ?? DBNull.Value },
    ];

    public DataTable LichHen(DateTime? tu, DateTime? den, string? trangThai, string? tuKhoa) => DbHelper.LayBang($@"
SELECT TOP {ToiDa} h.ma_lich_hen AS [Mã], l.ngay_lam_viec AS [Ngày khám], c.ten_ca AS [Ca], k.ho_ten_kh AS [Khách hàng], k.sdt_kh AS [SĐT],
       bs.ho_ten_nv AS [Bác sĩ], kh.ten_khoa AS [Khoa], h.trieu_chung_ban_dau AS [Triệu chứng ban đầu], h.trang_thai AS [Trạng thái],
       h.ngay_gio_dat AS [Đặt lúc], lt.ho_ten_nv AS [Lễ tân xác nhận]
FROM Lich_hen h
JOIN Khach_hang k ON k.ma_kh = h.ma_kh
JOIN Lich_lam_viec l ON l.ma_lich_lam_viec = h.ma_lich_lam_viec
JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca
JOIN Nhan_vien bs ON bs.ma_nv = l.ma_nv
JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
LEFT JOIN Nhan_vien lt ON lt.ma_nv = h.ma_nv
WHERE (@tu IS NULL OR l.ngay_lam_viec >= @tu) AND (@denSau IS NULL OR l.ngay_lam_viec < @denSau)
  AND (@tt IS NULL OR h.trang_thai = @tt)
  AND (@q IS NULL OR h.ma_lich_hen LIKE @q OR k.ho_ten_kh LIKE @q OR k.sdt_kh LIKE @q OR bs.ho_ten_nv LIKE @q)
ORDER BY l.ngay_lam_viec DESC, c.gio_bat_dau, h.ngay_gio_dat", ThamSo(tu, den, trangThai, tuKhoa));

    public DataTable HoSo(DateTime? tu, DateTime? den, string? trangThai, string? tuKhoa) => DbHelper.LayBang($@"
SELECT TOP {ToiDa} * FROM (
  SELECT hs.ma_ho_so AS [Mã hồ sơ], hs.ngay_gio_kham AS [Ngày khám], k.ho_ten_kh AS [Khách hàng], k.sdt_kh AS [SĐT],
         bs.ho_ten_nv AS [Bác sĩ], kh.ten_khoa AS [Khoa], CASE WHEN hs.ma_lich_hen IS NULL THEN N'Vãng lai' ELSE N'Có hẹn' END AS [Nguồn],
         hs.chan_doan_benh AS [Chẩn đoán], {TiepNhanDAL.BieuThucTrangThai} AS [Trạng thái]
  FROM Ho_so_kham hs
  JOIN Khach_hang k ON k.ma_kh = hs.ma_kh
  JOIN Nhan_vien bs ON bs.ma_nv = hs.ma_nv_kham
  JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
  LEFT JOIN Hoa_don d ON d.ma_ho_so = hs.ma_ho_so
  WHERE (@tu IS NULL OR hs.ngay_gio_kham >= @tu) AND (@denSau IS NULL OR hs.ngay_gio_kham < @denSau)
    AND (@q IS NULL OR hs.ma_ho_so LIKE @q OR k.ho_ten_kh LIKE @q OR k.sdt_kh LIKE @q OR bs.ho_ten_nv LIKE @q OR hs.chan_doan_benh LIKE @q)
) x
WHERE (@tt IS NULL OR x.[Trạng thái] = @tt)
ORDER BY x.[Ngày khám] DESC, x.[Mã hồ sơ] DESC", ThamSo(tu, den, trangThai, tuKhoa));

    public DataTable HoaDon(DateTime? tu, DateTime? den, string? trangThai, string? tuKhoa) => DbHelper.LayBang($@"
SELECT TOP {ToiDa} d.ma_hoa_don AS [Mã hóa đơn], hs.ma_ho_so AS [Mã hồ sơ], hs.ngay_gio_kham AS [Ngày khám], k.ho_ten_kh AS [Khách hàng],
       k.sdt_kh AS [SĐT], bs.ho_ten_nv AS [Bác sĩ], d.tong_tien AS [Tổng tiền], d.trang_thai_thanh_toan AS [Trạng thái],
       d.ngay_gio_thanh_toan AS [Thu lúc], tn.ho_ten_nv AS [Thu ngân]
FROM Hoa_don d
JOIN Ho_so_kham hs ON hs.ma_ho_so = d.ma_ho_so
JOIN Khach_hang k ON k.ma_kh = hs.ma_kh
JOIN Nhan_vien bs ON bs.ma_nv = hs.ma_nv_kham
LEFT JOIN Nhan_vien tn ON tn.ma_nv = d.ma_nv
WHERE (@tu IS NULL OR hs.ngay_gio_kham >= @tu) AND (@denSau IS NULL OR hs.ngay_gio_kham < @denSau)
  AND (@tt IS NULL OR d.trang_thai_thanh_toan = @tt)
  AND (@q IS NULL OR d.ma_hoa_don LIKE @q OR hs.ma_ho_so LIKE @q OR k.ho_ten_kh LIKE @q OR k.sdt_kh LIKE @q)
ORDER BY hs.ngay_gio_kham DESC, d.ma_hoa_don DESC", ThamSo(tu, den, trangThai, tuKhoa));
}
