using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>W9: quản trị viên tra cứu (chỉ đọc trên dữ liệu mẫu).</summary>
public class TraCuuTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly TraCuuBUS _tc = new();

    public TraCuuTests() => _db.DangNhap(ChucVu.QuanTriVien);
    public void Dispose() => _db.Dispose();

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    [Fact]
    public void TraCuu_DuBaLoai_LocTrangThai()
    {
        Assert.Equal(_db.GiaTri("SELECT COUNT(*) FROM Lich_hen"), _tc.TimKiem(LoaiTraCuu.LichHen, null, null, null, null).Rows.Count);
        Assert.Equal(_db.GiaTri("SELECT COUNT(*) FROM Ho_so_kham"), _tc.TimKiem(LoaiTraCuu.HoSo, null, null, null, null).Rows.Count);
        var daTT = _tc.TimKiem(LoaiTraCuu.HoaDon, null, null, TrangThai.DaThanhToan, null);
        Assert.Equal(_db.GiaTri("SELECT COUNT(*) FROM Hoa_don WHERE trang_thai_thanh_toan = N'Đã thanh toán'"), daTT.Rows.Count);
        var hsDaTT = _tc.TimKiem(LoaiTraCuu.HoSo, null, null, TrangThai.DaThanhToan, null);
        Assert.Equal(daTT.Rows.Count, hsDaTT.Rows.Count);
        Assert.Empty(_tc.TimKiem(LoaiTraCuu.HoSo, DateTime.Today.AddYears(5), DateTime.Today.AddYears(5), null, null).Rows.Cast<object>());
        Loi("Trạng thái không hợp lệ.", () => _tc.TimKiem(LoaiTraCuu.HoaDon, null, null, TrangThai.DaDat, null));
        Loi("Từ ngày phải trước hoặc bằng đến ngày.", () => _tc.TimKiem(LoaiTraCuu.LichHen, DateTime.Today, DateTime.Today.AddDays(-1), null, null));
    }

    [Fact]
    public void ChiTietHoSo_MoiBacSi_VaPhanQuyen()
    {
        var ma = (string)_db.GiaTri("SELECT TOP 1 ma_ho_so FROM Ho_so_kham ORDER BY ma_ho_so")!;
        var (hs, thuoc) = _tc.ChiTietHoSo(ma);
        Assert.Equal(ma, hs.MaHoSo);
        Assert.Equal(_db.GiaTri($"SELECT COUNT(*) FROM Chi_tiet_don_thuoc WHERE ma_ho_so = '{ma}'"), thuoc.Count);
        Loi("Không tìm thấy hồ sơ khám.", () => _tc.ChiTietHoSo("HS9999999"));
        _db.DangNhap(ChucVu.LeTan);
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _tc.TimKiem(LoaiTraCuu.HoSo, null, null, null, null));
    }
}
