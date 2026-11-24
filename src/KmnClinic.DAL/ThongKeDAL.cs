using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>
/// Thống kê và báo cáo (W8), chỉ đọc. Doanh thu = tổng tong_tien của hóa đơn Đã thanh toán, tính theo ngày thanh toán.
/// Báo cáo trả DataTable (SqlDataAdapter) với tên cột tiếng Việt để hiện thẳng lên lưới và xuất file.
/// </summary>
public class ThongKeDAL
{
    public TongQuanQuanTriDTO TongQuan(DateTime homNay)
    {
        homNay = homNay.Date;
        using var conn = DbHelper.MoKetNoi();
        var kq = new TongQuanQuanTriDTO();
        using (var cmd = conn.Lenh(@"
SELECT
  (SELECT ISNULL(SUM(tong_tien), 0) FROM Hoa_don WHERE ngay_gio_thanh_toan >= @hn AND ngay_gio_thanh_toan < DATEADD(day, 1, @hn)),
  (SELECT ISNULL(SUM(tong_tien), 0) FROM Hoa_don WHERE ngay_gio_thanh_toan >= DATEADD(day, -1, @hn) AND ngay_gio_thanh_toan < @hn),
  (SELECT COUNT(*) FROM Hoa_don WHERE ngay_gio_thanh_toan >= @hn AND ngay_gio_thanh_toan < DATEADD(day, 1, @hn)),
  (SELECT COUNT(*) FROM Ho_so_kham WHERE ngay_gio_kham >= @hn AND ngay_gio_kham < DATEADD(day, 1, @hn)),
  (SELECT COUNT(*) FROM Ho_so_kham WHERE ngay_gio_kham >= @hn AND ngay_gio_kham < DATEADD(day, 1, @hn) AND ma_lich_hen IS NULL),
  (SELECT COUNT(*) FROM Lich_hen WHERE trang_thai = N'Đã đặt'),
  (SELECT COUNT(*) FROM Hoa_don WHERE trang_thai_thanh_toan = N'Chưa thanh toán')").P("@hn", SqlDbType.DateTime, 0, homNay))
        using (var r = cmd.ExecuteReader())
        {
            r.Read();
            kq.DoanhThuHomNay = r.GetDecimal(0); kq.DoanhThuHomQua = r.GetDecimal(1); kq.HoaDonHomNay = r.GetInt32(2);
            kq.LuotKhamHomNay = r.GetInt32(3); kq.VangLaiHomNay = r.GetInt32(4); kq.LichHenChoXacNhan = r.GetInt32(5); kq.HoaDonChoThu = r.GetInt32(6);
        }
        var tu = homNay.AddDays(-13);
        var theoNgay = new Dictionary<DateTime, decimal>();
        using (var cmd = conn.Lenh(@"
SELECT CAST(ngay_gio_thanh_toan AS date), SUM(tong_tien) FROM Hoa_don
WHERE ngay_gio_thanh_toan >= @tu AND ngay_gio_thanh_toan < DATEADD(day, 1, @hn)
GROUP BY CAST(ngay_gio_thanh_toan AS date)").P("@tu", SqlDbType.DateTime, 0, tu).P("@hn", SqlDbType.DateTime, 0, homNay))
        using (var r = cmd.ExecuteReader())
            while (r.Read()) theoNgay[r.GetDateTime(0)] = r.GetDecimal(1);
        for (var d = tu; d <= homNay; d = d.AddDays(1))
            kq.DoanhThu14Ngay.Add((d, theoNgay.GetValueOrDefault(d)));
        return kq;
    }

    private static SqlParameter[] Ky(DateTime tu, DateTime den) =>
    [
        new("@tu", SqlDbType.DateTime) { Value = tu.Date },
        new("@denSau", SqlDbType.DateTime) { Value = den.Date.AddDays(1) },
    ];

    /// <summary>Từng hóa đơn đã thu trong kỳ.</summary>
    public DataTable BaoCaoDoanhThu(DateTime tu, DateTime den) => DbHelper.LayBang(@"
SELECT d.ma_hoa_don AS [Mã hóa đơn], d.ngay_gio_thanh_toan AS [Thu lúc], k.ho_ten_kh AS [Khách hàng], bs.ho_ten_nv AS [Bác sĩ],
       kh.ten_khoa AS [Khoa], hs.tien_cong_kham AS [Tiền khám],
       ISNULL((SELECT SUM(ct.so_luong_thuoc * ct.don_gia_thuoc) FROM Chi_tiet_don_thuoc ct WHERE ct.ma_ho_so = hs.ma_ho_so), 0) AS [Tiền thuốc],
       d.tong_tien AS [Tổng tiền], tn.ho_ten_nv AS [Thu ngân]
FROM Hoa_don d
JOIN Ho_so_kham hs ON hs.ma_ho_so = d.ma_ho_so
JOIN Khach_hang k ON k.ma_kh = hs.ma_kh
JOIN Nhan_vien bs ON bs.ma_nv = hs.ma_nv_kham
JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
JOIN Nhan_vien tn ON tn.ma_nv = d.ma_nv
WHERE d.trang_thai_thanh_toan = N'Đã thanh toán' AND d.ngay_gio_thanh_toan >= @tu AND d.ngay_gio_thanh_toan < @denSau
ORDER BY d.ngay_gio_thanh_toan, d.ma_hoa_don", Ky(tu, den));

    /// <summary>Lượt khám và doanh thu theo bác sĩ trong kỳ (lượt khám theo ngày khám, doanh thu theo ngày thu).</summary>
    public DataTable BaoCaoBacSi(DateTime tu, DateTime den) => DbHelper.LayBang(@"
SELECT kh.ten_khoa AS [Khoa], bs.ma_nv AS [Mã BS], bs.ho_ten_nv AS [Bác sĩ],
       (SELECT COUNT(*) FROM Ho_so_kham hs WHERE hs.ma_nv_kham = bs.ma_nv AND hs.ngay_gio_kham >= @tu AND hs.ngay_gio_kham < @denSau) AS [Lượt khám],
       (SELECT COUNT(*) FROM Ho_so_kham hs WHERE hs.ma_nv_kham = bs.ma_nv AND hs.ngay_gio_kham >= @tu AND hs.ngay_gio_kham < @denSau
           AND hs.ma_lich_hen IS NOT NULL) AS [Có hẹn],
       (SELECT ISNULL(SUM(d.tong_tien), 0) FROM Hoa_don d JOIN Ho_so_kham hs ON hs.ma_ho_so = d.ma_ho_so
        WHERE hs.ma_nv_kham = bs.ma_nv AND d.ngay_gio_thanh_toan >= @tu AND d.ngay_gio_thanh_toan < @denSau) AS [Doanh thu]
FROM Nhan_vien bs JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
WHERE bs.chuc_vu = N'Bác sĩ'
ORDER BY kh.ten_khoa, bs.ho_ten_nv", Ky(tu, den));

    /// <summary>Thuốc đã xuất (đơn của hóa đơn đã thu trong kỳ), nhiều nhất trước.</summary>
    public DataTable BaoCaoThuoc(DateTime tu, DateTime den) => DbHelper.LayBang(@"
SELECT t.ma_thuoc AS [Mã thuốc], t.ten_thuoc AS [Thuốc], t.ham_luong AS [Hàm lượng], t.don_vi_tinh AS [ĐVT],
       SUM(ct.so_luong_thuoc) AS [Số lượng xuất], COUNT(*) AS [Số đơn], SUM(ct.so_luong_thuoc * ct.don_gia_thuoc) AS [Thành tiền],
       t.so_luong_ton AS [Tồn hiện tại]
FROM Chi_tiet_don_thuoc ct
JOIN Thuoc t ON t.ma_thuoc = ct.ma_thuoc
JOIN Hoa_don d ON d.ma_ho_so = ct.ma_ho_so
WHERE d.trang_thai_thanh_toan = N'Đã thanh toán' AND d.ngay_gio_thanh_toan >= @tu AND d.ngay_gio_thanh_toan < @denSau
GROUP BY t.ma_thuoc, t.ten_thuoc, t.ham_luong, t.don_vi_tinh, t.so_luong_ton
ORDER BY SUM(ct.so_luong_thuoc) DESC, t.ten_thuoc", Ky(tu, den));

    /// <summary>Tồn kho hiện tại, ít nhất trước (không phụ thuộc kỳ).</summary>
    public DataTable BaoCaoTonKho() => DbHelper.LayBang($@"
SELECT ma_thuoc AS [Mã thuốc], ten_thuoc AS [Thuốc], nsx AS [Nhà sản xuất], ham_luong AS [Hàm lượng], don_vi_tinh AS [ĐVT],
       don_gia_thuoc AS [Đơn giá], so_luong_ton AS [Tồn],
       CASE WHEN so_luong_ton = 0 THEN N'Hết hàng' WHEN so_luong_ton <= {ThuocDTO.NguongSapHet} THEN N'Sắp hết' ELSE N'' END AS [Ghi chú]
FROM Thuoc ORDER BY so_luong_ton, ten_thuoc");
}
