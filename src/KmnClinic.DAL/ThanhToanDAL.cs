using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

public enum LoiThanhToan { Khong, KhongTimThay, DaThanhToan, ThieuKho }

public sealed class KetQuaThanhToan
{
    public LoiThanhToan Loi { get; init; }
    /// <summary>Khi đã thanh toán trước đó: lúc thu và người thu.</summary>
    public DateTime? Luc { get; init; }
    public string? NguoiThu { get; init; }
    public List<ChiTietDonThuocDTO> ThuocThieu { get; init; } = [];
}

/// <summary>
/// Thu ngân xem hóa đơn và xác nhận đã thu. TG_Hoa_don_KiemTra vẫn chặn ở CSDL: RB32 người thu là thu ngân,
/// không chuyển ngược về Chưa thanh toán, RB33 trừ kho khi chuyển sang Đã thanh toán; CK_Hoa_don_thanh_toan (RB31)
/// bắt buộc có giờ và người thu. Không có hàm sửa, xóa, hủy hóa đơn để giữ lịch sử.
/// </summary>
public class ThanhToanDAL
{
    private const string SqlHoaDon = @"
SELECT d.ma_hoa_don, d.tong_tien, d.trang_thai_thanh_toan, d.ngay_gio_thanh_toan, tn.ma_nv, tn.ho_ten_nv,
       hs.ma_ho_so, hs.ngay_gio_kham, hs.tien_cong_kham, k.ma_kh, k.ho_ten_kh, k.sdt_kh, bs.ho_ten_nv, kh.ten_khoa,
       (SELECT COUNT(*) FROM Chi_tiet_don_thuoc ct WHERE ct.ma_ho_so = hs.ma_ho_so),
       hs.trieu_chung, hs.chan_doan_benh, hs.loi_dan, k.gioi_tinh_kh, k.ngay_sinh_kh, k.dia_chi_kh
FROM Hoa_don d
JOIN Ho_so_kham hs ON hs.ma_ho_so = d.ma_ho_so
JOIN Khach_hang k ON k.ma_kh = hs.ma_kh
JOIN Nhan_vien bs ON bs.ma_nv = hs.ma_nv_kham
JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
LEFT JOIN Nhan_vien tn ON tn.ma_nv = d.ma_nv";

    /// <summary>
    /// Danh sách hóa đơn (null = không lọc). Lọc ngày theo giờ thanh toán. Chờ thu: khám sớm nhất lên đầu (khách đợi lâu nhất);
    /// đã thu: mới thu nhất lên đầu.
    /// </summary>
    public List<HoaDonDTO> DanhSach(string? trangThai, DateTime? tu, DateTime? den, string? maThuNgan, string? tuKhoa)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlHoaDon + @"
WHERE (@tt IS NULL OR d.trang_thai_thanh_toan = @tt)
  AND (@tu IS NULL OR d.ngay_gio_thanh_toan >= @tu) AND (@den IS NULL OR d.ngay_gio_thanh_toan < DATEADD(day, 1, @den))
  AND (@tn IS NULL OR d.ma_nv = @tn)
  AND (@q IS NULL OR d.ma_hoa_don LIKE @q OR hs.ma_ho_so LIKE @q OR k.ho_ten_kh LIKE @q OR k.sdt_kh LIKE @q OR k.ma_kh LIKE @q)
ORDER BY CASE WHEN d.trang_thai_thanh_toan = N'Chưa thanh toán' THEN 0 ELSE 1 END,
         CASE WHEN d.trang_thai_thanh_toan = N'Chưa thanh toán' THEN hs.ngay_gio_kham END,
         d.ngay_gio_thanh_toan DESC, d.ma_hoa_don DESC")
            .P("@tt", SqlDbType.NVarChar, 20, trangThai)
            .P("@tu", SqlDbType.DateTime, 0, tu?.Date).P("@den", SqlDbType.DateTime, 0, den?.Date)
            .P("@tn", SqlDbType.VarChar, 10, maThuNgan).P("@q", SqlDbType.NVarChar, 60, TimKiem.MauLike(tuKhoa));
        return Doc(cmd);
    }

    /// <summary>Một hóa đơn kèm đơn thuốc; null nếu không có.</summary>
    public HoaDonDTO? ChiTiet(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        return ChiTiet(conn, null, ma);
    }

    private static HoaDonDTO? ChiTiet(SqlConnection conn, SqlTransaction? tx, string ma)
    {
        HoaDonDTO? hd;
        using (var cmd = conn.Lenh(SqlHoaDon + " WHERE d.ma_hoa_don = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma))
            hd = Doc(cmd).FirstOrDefault();
        if (hd is null) return null;
        using var c = conn.Lenh(@"
SELECT ct.ma_thuoc, t.ten_thuoc, t.ham_luong, t.don_vi_tinh, ct.so_luong_thuoc, ct.lieu_dung, ct.don_gia_thuoc, t.so_luong_ton
FROM Chi_tiet_don_thuoc ct JOIN Thuoc t ON t.ma_thuoc = ct.ma_thuoc
WHERE ct.ma_ho_so = @hs ORDER BY t.ten_thuoc", tx).P("@hs", SqlDbType.VarChar, 10, hd.MaHoSo);
        using var r = c.ExecuteReader();
        while (r.Read())
            hd.Thuoc.Add(new ChiTietDonThuocDTO
            {
                MaThuoc = r.GetString(0), TenThuoc = r.GetString(1), HamLuong = r.IsDBNull(2) ? null : r.GetDecimal(2),
                DonViTinh = r.GetString(3), SoLuongThuoc = r.GetInt32(4), LieuDung = DbHelper.ChuoiHoacNull(r, 5),
                DonGiaThuoc = r.GetDecimal(6), SoLuongTon = r.GetInt32(7),
            });
        return hd;
    }

    private static List<HoaDonDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<HoaDonDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new HoaDonDTO
            {
                MaHoaDon = r.GetString(0), TongTien = r.GetDecimal(1), TrangThaiThanhToan = r.GetString(2),
                NgayGioThanhToan = r.IsDBNull(3) ? null : r.GetDateTime(3), MaThuNgan = DbHelper.ChuoiHoacNull(r, 4),
                ThuNgan = DbHelper.ChuoiHoacNull(r, 5), MaHoSo = r.GetString(6), NgayKham = r.GetDateTime(7),
                TienCongKham = r.GetDecimal(8), MaKh = r.GetString(9), TenKhachHang = r.GetString(10), SdtKhachHang = r.GetString(11),
                TenBacSi = r.GetString(12), TenKhoa = r.GetString(13), SoThuoc = r.GetInt32(14),
                TrieuChung = r.GetString(15), ChanDoanBenh = DbHelper.ChuoiHoacNull(r, 16), LoiDan = DbHelper.ChuoiHoacNull(r, 17),
                GioiTinh = DbHelper.ChuoiHoacNull(r, 18), NgaySinh = r.GetDateTime(19), DiaChi = DbHelper.ChuoiHoacNull(r, 20),
            });
        return ds;
    }

    /// <summary>
    /// Xác nhận đã thu: khóa dòng hóa đơn rồi mới kiểm tra trạng thái (bấm hai lần hoặc hai thu ngân cùng bấm thì chỉ một lần
    /// thành công). Khóa các dòng thuốc trong đơn rồi kiểm tra tồn, để hai hóa đơn dùng chung thuốc không cùng qua bước này.
    /// Trigger trừ kho (RB33) trong cùng giao dịch.
    /// </summary>
    public KetQuaThanhToan ThanhToan(string ma, string maThuNgan, DateTime bayGio) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        string trangThai, maHoSo; DateTime? luc; string? nguoiThu;
        using (var c = conn.Lenh(@"
SELECT d.trang_thai_thanh_toan, d.ma_ho_so, d.ngay_gio_thanh_toan, tn.ho_ten_nv
FROM Hoa_don d WITH (UPDLOCK, ROWLOCK) LEFT JOIN Nhan_vien tn ON tn.ma_nv = d.ma_nv
WHERE d.ma_hoa_don = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma))
        using (var r = c.ExecuteReader())
        {
            if (!r.Read()) return new KetQuaThanhToan { Loi = LoiThanhToan.KhongTimThay };
            trangThai = r.GetString(0); maHoSo = r.GetString(1);
            luc = r.IsDBNull(2) ? null : r.GetDateTime(2); nguoiThu = DbHelper.ChuoiHoacNull(r, 3);
        }
        if (trangThai == TrangThai.DaThanhToan) return new KetQuaThanhToan { Loi = LoiThanhToan.DaThanhToan, Luc = luc, NguoiThu = nguoiThu };

        var thieu = new List<ChiTietDonThuocDTO>();
        using (var c = conn.Lenh(@"
SELECT t.ten_thuoc, ct.so_luong_thuoc, t.so_luong_ton
FROM Chi_tiet_don_thuoc ct JOIN Thuoc t WITH (UPDLOCK, ROWLOCK) ON t.ma_thuoc = ct.ma_thuoc
WHERE ct.ma_ho_so = @hs", tx).P("@hs", SqlDbType.VarChar, 10, maHoSo))
        using (var r = c.ExecuteReader())
            while (r.Read())
                if (r.GetInt32(1) > r.GetInt32(2))
                    thieu.Add(new ChiTietDonThuocDTO { TenThuoc = r.GetString(0), SoLuongThuoc = r.GetInt32(1), SoLuongTon = r.GetInt32(2) });
        if (thieu.Count > 0) return new KetQuaThanhToan { Loi = LoiThanhToan.ThieuKho, ThuocThieu = thieu };

        using (var c = conn.Lenh(@"
UPDATE Hoa_don SET trang_thai_thanh_toan = N'Đã thanh toán', ngay_gio_thanh_toan = @luc, ma_nv = @tn
WHERE ma_hoa_don = @ma AND trang_thai_thanh_toan = N'Chưa thanh toán'", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@luc", SqlDbType.DateTime, 0, bayGio).P("@tn", SqlDbType.VarChar, 10, maThuNgan))
            c.ExecuteNonQuery();
        return new KetQuaThanhToan();
    });

    public TongQuanThuNganDTO TongQuan(string maThuNgan, DateTime bayGio)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(@"
SELECT
  ISNULL(SUM(CASE WHEN trang_thai_thanh_toan = N'Chưa thanh toán' THEN 1 ELSE 0 END), 0),
  ISNULL(SUM(CASE WHEN trang_thai_thanh_toan = N'Chưa thanh toán' THEN tong_tien ELSE 0 END), 0),
  ISNULL(SUM(CASE WHEN ngay_gio_thanh_toan >= @homNay THEN 1 ELSE 0 END), 0),
  ISNULL(SUM(CASE WHEN ngay_gio_thanh_toan >= @homNay THEN tong_tien ELSE 0 END), 0),
  ISNULL(SUM(CASE WHEN ngay_gio_thanh_toan >= @homNay AND ma_nv = @tn THEN 1 ELSE 0 END), 0)
FROM Hoa_don
WHERE trang_thai_thanh_toan = N'Chưa thanh toán' OR (ngay_gio_thanh_toan >= @homNay AND ngay_gio_thanh_toan < DATEADD(day, 1, @homNay))")
            .P("@homNay", SqlDbType.DateTime, 0, bayGio.Date).P("@tn", SqlDbType.VarChar, 10, maThuNgan);
        using var r = cmd.ExecuteReader();
        r.Read();
        return new TongQuanThuNganDTO
        {
            ChoThu = r.GetInt32(0), TienChoThu = r.GetDecimal(1), DaThuHomNay = r.GetInt32(2),
            DoanhThuHomNay = r.GetDecimal(3), CuaToiHomNay = r.GetInt32(4),
        };
    }
}
