using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>
/// W4: lễ tân đặt lịch hộ. Dùng phòng, bác sĩ, lịch làm việc thử ở ngày cách hôm nay khoảng 200 ngày;
/// khách thử có SĐT 09999xxxxx. Mọi dòng thêm vào được xóa khi xong.
/// </summary>
public class LichHenTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly LichHenBUS _bus = new();
    private readonly string _bs, _lichSang, _lichChieu, _khachCo;
    private readonly DateTime _ngay = LichLamViecBUS.DauTuan(DateTime.Today.AddDays(200));

    public LichHenTests()
    {
        _db.DangNhap(ChucVu.QuanTriVien);
        var duoi = Random.Shared.Next(100000, 999999);
        var phong = _db.Xoa("Phong_lam_viec", "ma_phong", new PhongBUS().Them($"ZTest Phòng {duoi}", "K0001"));
        _bs = _db.ThemNhanVien("x", ChucVu.BacSi).Ma;
        var lich = new LichLamViecBUS();
        _lichSang = _db.Xoa("Lich_lam_viec", "ma_lich_lam_viec", lich.Them(_ngay, _bs, phong, "CA0001"));
        _lichChieu = _db.Xoa("Lich_lam_viec", "ma_lich_lam_viec", lich.Them(_ngay, _bs, phong, "CA0002"));
        _khachCo = _db.Xoa("Khach_hang", "ma_kh", new KhachHangBUS().Them(KhachMoi("ZTest Khách có")));
        _db.DangNhap(ChucVu.LeTan);
    }

    public void Dispose() => _db.Dispose();

    private static KhachHangDTO KhachMoi(string ten) => new()
    {
        HoTenKh = ten, SdtKh = "09999" + Random.Shared.Next(0, 100000).ToString("D5"), GioiTinhKh = "Khác", NgaySinhKh = new DateTime(1995, 5, 5),
    };

    private string GhiNhan(string ma) => _db.Xoa("Lich_hen", "ma_lich_hen", ma);

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    [Fact]
    public void DatHo_KhachCo_XacNhanLuon_GhiLeTan()
    {
        var (ma, maKh) = _bus.DatHo(_khachCo, null, _lichSang, "  Đau đầu  ");
        GhiNhan(ma);
        Assert.Matches("^LH[0-9]{4,}$", ma);
        Assert.Equal(_khachCo, maKh);
        Assert.Equal(TrangThai.DaXacNhan, _db.GiaTri($"SELECT trang_thai FROM Lich_hen WHERE ma_lich_hen = '{ma}'"));
        Assert.Equal(PhienDangNhap.NhanVien!.MaNv, _db.GiaTri($"SELECT ma_nv FROM Lich_hen WHERE ma_lich_hen = '{ma}'"));
        Assert.Equal("Đau đầu", _db.GiaTri($"SELECT trieu_chung_ban_dau FROM Lich_hen WHERE ma_lich_hen = '{ma}'"));
    }

    [Fact]
    public void DatHo_KhachMoi_ThemCungLuc()
    {
        var k = KhachMoi("ZTest Khách mới");
        var (ma, maKh) = _bus.DatHo(null, k, _lichChieu, null);
        _db.Xoa("Khach_hang", "ma_kh", maKh);
        GhiNhan(ma);
        Assert.Equal(k.SdtKh, _db.GiaTri($"SELECT sdt_kh FROM Khach_hang WHERE ma_kh = '{maKh}'"));
        Assert.Equal(DBNull.Value, _db.GiaTri($"SELECT gioi_tinh_kh FROM Khach_hang WHERE ma_kh = '{maKh}'")); // Khác = NULL
    }

    [Fact]
    public void DatHo_KhachMoi_LoiThiKhongLuuKhach()
    {
        var k = KhachMoi("ZTest Khách lỗi");
        _bus.BayGio = () => _ngay.AddHours(8); // ca sáng 07:00 đã bắt đầu
        Assert.Throws<LoiNghiepVu>(() => _bus.DatHo(null, k, _lichSang, null));
        Assert.Equal(0, _db.GiaTri($"SELECT COUNT(*) FROM Khach_hang WHERE sdt_kh = '{k.SdtKh}'"));
    }

    [Fact]
    public void Q6_KhongDatTrungCa()
    {
        var (ma, _) = _bus.DatHo(_khachCo, null, _lichSang, null);
        GhiNhan(ma);
        Loi($"Khách đã có lịch hẹn {ma} với bác sĩ ở ca này, không cần đặt lại.", () => _bus.DatHo(_khachCo, null, _lichSang, null));
        GhiNhan(_bus.DatHo(_khachCo, null, _lichChieu, null).MaLichHen); // ca khác thì được
    }

    [Fact]
    public void RB16_CaDaBatDau()
    {
        _bus.BayGio = () => _ngay.AddHours(7); // đúng giờ bắt đầu ca sáng
        Loi("Ca khám này đã bắt đầu hoặc đã qua, không đặt được nữa (RB16). Vui lòng chọn ca khác.",
            () => _bus.DatHo(_khachCo, null, _lichSang, null));
        // Ca chiều vẫn đặt được, và danh sách ca còn đặt chỉ còn ca chiều
        Assert.Equal([_lichChieu], _bus.CaConDat(_ngay, null, _bs).Select(l => l.MaLichLamViec));
        GhiNhan(_bus.DatHo(_khachCo, null, _lichChieu, null).MaLichHen);
    }

    [Fact]
    public void CaConDat_DemSoHen()
    {
        GhiNhan(_bus.DatHo(_khachCo, null, _lichSang, null).MaLichHen);
        var ds = _bus.CaConDat(_ngay, "K0001", _bs);
        Assert.Equal([_lichSang, _lichChieu], ds.Select(l => l.MaLichLamViec));
        Assert.Equal(1, ds[0].SoLichHenConHieuLuc);
        Assert.Equal(0, ds[1].SoLichHenConHieuLuc);
    }

    [Fact]
    public void KiemTraDauVao()
    {
        Loi("Vui lòng tìm khách theo số điện thoại hoặc nhập khách mới.", () => _bus.DatHo(null, null, _lichSang, null));
        Loi("Vui lòng chọn một ca còn đặt được.", () => _bus.DatHo(_khachCo, null, null, null));
        Loi("Không tìm thấy lịch làm việc đã chọn. Vui lòng chọn lại.", () => _bus.DatHo(_khachCo, null, "LV9999999", null));
        Loi("Không tìm thấy khách hàng đã chọn.", () => _bus.DatHo("KH99999", null, _lichSang, null));
        var k = KhachMoi("ZTest");
        k.NgaySinhKh = null;
        Loi("Vui lòng nhập ngày sinh.", () => _bus.DatHo(null, k, _lichSang, null));
    }

    [Fact]
    public void ChiLeTanDatHo()
    {
        _db.DangNhap(ChucVu.BacSi);
        Assert.Throws<LoiNghiepVu>(() => _bus.DatHo(_khachCo, null, _lichSang, null));
        _db.DangNhap(ChucVu.ThuNgan);
        Assert.Throws<LoiNghiepVu>(() => _bus.CaConDat(_ngay, null, null));
    }
}
