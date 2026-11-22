using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>
/// W1: Khoa, Phòng, Ca, Thuốc. Test tự thêm dòng có tên bắt đầu "ZTest" rồi xóa khi xong.
/// Dữ liệu mẫu chỉ được đọc, hoặc dùng cho thao tác bị chặn (không làm đổi dữ liệu).
/// </summary>
public class DanhMucTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly KhoaBUS _khoa = new();
    private readonly PhongBUS _phong = new();
    private readonly CaBUS _ca = new();
    private readonly ThuocBUS _thuoc = new();
    private readonly string _duoi = Random.Shared.Next(100000, 999999).ToString();

    public DanhMucTests() => _db.DangNhap(ChucVu.QuanTriVien);

    public void Dispose() => _db.Dispose();

    private string ThemKhoa(string ten = "ZTest Khoa") => _db.Xoa("Khoa", "ma_khoa", _khoa.Them($"{ten} {_duoi}", "Khoa kiểm thử"));

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    // ---------------- Khoa ----------------

    [Fact]
    public void Khoa_ThemSuaXoa()
    {
        var ma = _khoa.Them($"ZTest Khoa {_duoi}", "  ");
        Assert.Matches("^K[0-9]{4,}$", ma);
        var k = _khoa.DanhSach($"ZTest Khoa {_duoi}").Single();
        Assert.Null(k.MoTa); // mô tả toàn khoảng trắng lưu NULL
        Assert.Equal(0, k.SoPhong);

        _khoa.Sua(ma, $"ZTest Khoa sửa {_duoi}", "Mô tả mới");
        Assert.Equal("Mô tả mới", _khoa.DanhSach($"ZTest Khoa sửa {_duoi}").Single().MoTa);

        _khoa.Xoa(ma);
        Assert.Empty(_khoa.DanhSach($"ZTest Khoa sửa {_duoi}"));
    }

    [Fact]
    public void Khoa_TrungTen_BaoLoiTiengViet()
    {
        var ten = _khoa.DanhSach().First().TenKhoa;
        Loi("Tên khoa đã tồn tại.", () => _khoa.Them(ten, null));
    }

    [Fact]
    public void Khoa_ConPhongHoacBacSi_KhongXoaDuoc()
    {
        var k = _khoa.DanhSach().First(x => x.SoBacSi > 0);
        var loi = Assert.Throws<LoiNghiepVu>(() => _khoa.Xoa(k.MaKhoa));
        Assert.StartsWith("Không thể xóa: khoa còn", loi.Message);
        Assert.NotNull(_khoa.DanhSach(k.MaKhoa).SingleOrDefault());
    }

    [Theory]
    [InlineData("", "Vui lòng nhập tên khoa.")]
    [InlineData("   ", "Vui lòng nhập tên khoa.")]
    [InlineData("123456789012345678901234567890123456789012345678901", "Tên khoa tối đa 50 ký tự.")]
    public void Khoa_TenKhongHopLe(string ten, string thongBao) => Loi(thongBao, () => _khoa.Them(ten, null));

    [Fact]
    public void Khoa_TimKiem_KyTuDacBietKhongLoi()
        => Assert.Empty(_khoa.DanhSach("%_[ZZZ"));

    // ---------------- Phòng ----------------

    [Fact]
    public void Phong_ThemSuaXoa_VaKhoaKhongXoaDuocKhiConPhong()
    {
        var maKhoa = ThemKhoa();
        var maKhoa2 = ThemKhoa("ZTest Khoa 2");
        var ma = _db.Xoa("Phong_lam_viec", "ma_phong", _phong.Them($"ZTest Phòng {_duoi}", maKhoa));

        var p = _phong.DanhSach(maKhoa: maKhoa).Single();
        Assert.Equal(ma, p.MaPhong);
        Assert.Equal(0, p.SoLichLamViec);
        Assert.StartsWith("Không thể xóa: khoa còn 1 phòng", Assert.Throws<LoiNghiepVu>(() => _khoa.Xoa(maKhoa)).Message);

        _phong.Sua(ma, $"ZTest Phòng sửa {_duoi}", maKhoa2); // chưa có lịch nên đổi khoa được
        Assert.Equal(maKhoa2, _phong.DanhSach($"ZTest Phòng sửa {_duoi}").Single().MaKhoa);

        _phong.Xoa(ma);
        Assert.Empty(_phong.DanhSach(maKhoa: maKhoa2));
    }

    [Fact]
    public void Phong_DaCoLich_KhongDoiKhoa_KhongXoa_NhungDoiTenDuoc()
    {
        var p = _phong.DanhSach().First(x => x.SoLichLamViec > 0);
        var khoaKhac = _khoa.DanhSach().First(k => k.MaKhoa != p.MaKhoa).MaKhoa;

        Assert.StartsWith("Không thể đổi khoa", Assert.Throws<LoiNghiepVu>(() => _phong.Sua(p.MaPhong, p.TenPhong, khoaKhac)).Message);
        Assert.StartsWith("Không thể xóa: phòng đã có", Assert.Throws<LoiNghiepVu>(() => _phong.Xoa(p.MaPhong)).Message);

        // Đổi tên rồi trả lại tên cũ, giữ khoa: được phép
        _phong.Sua(p.MaPhong, p.TenPhong + " Z", p.MaKhoa);
        _phong.Sua(p.MaPhong, p.TenPhong, p.MaKhoa);
        Assert.Equal(p.MaKhoa, _phong.DanhSach(p.MaPhong).Single().MaKhoa);
    }

    [Fact]
    public void Phong_ChuaChonKhoa_HoacKhoaKhongTonTai()
    {
        Loi("Vui lòng chọn khoa.", () => _phong.Them("ZTest Phòng", null));
        Loi("Dữ liệu tham chiếu không tồn tại hoặc không hợp lệ.", () => _phong.Them($"ZTest Phòng {_duoi}", "KXXXX"));
    }

    // ---------------- Ca ----------------

    [Fact]
    public void Ca_ThemSuaXoa()
    {
        var ma = _db.Xoa("Ca_lam_viec", "ma_ca", _ca.Them($"ZT{_duoi}", new(18, 0, 30), new(21, 0, 0)));
        var c = _ca.DanhSach().Single(x => x.MaCa == ma);
        Assert.Equal(new TimeSpan(18, 0, 0), c.GioBatDau); // bỏ phần giây

        _ca.Sua(ma, $"ZT{_duoi}b", new(18, 30, 0), new(21, 30, 0)); // chưa có lịch nên đổi giờ được
        c = _ca.DanhSach().Single(x => x.MaCa == ma);
        Assert.Equal(new TimeSpan(21, 30, 0), c.GioKetThuc);

        _ca.Xoa(ma);
        Assert.DoesNotContain(_ca.DanhSach(), x => x.MaCa == ma);
    }

    [Fact]
    public void Ca_GioBatDauPhaiNhoHonKetThuc()
    {
        Loi("Giờ bắt đầu ca phải nhỏ hơn giờ kết thúc.", () => _ca.Them("ZTest", new(13, 0, 0), new(13, 0, 0)));
        Loi("Giờ bắt đầu ca phải nhỏ hơn giờ kết thúc.", () => _ca.Them("ZTest", new(17, 0, 0), new(13, 0, 0)));
    }

    [Fact]
    public void Ca_DaCoLich_ChiDoiTen()
    {
        var c = _ca.DanhSach().First(x => x.SoLichLamViec > 0);
        Assert.StartsWith("Không thể đổi giờ", Assert.Throws<LoiNghiepVu>(() =>
            _ca.Sua(c.MaCa, c.TenCa, c.GioBatDau, c.GioKetThuc + TimeSpan.FromMinutes(30))).Message);
        Assert.StartsWith("Không thể xóa: ca đã được dùng", Assert.Throws<LoiNghiepVu>(() => _ca.Xoa(c.MaCa)).Message);
        _ca.Sua(c.MaCa, c.TenCa, c.GioBatDau, c.GioKetThuc); // giữ nguyên giờ: được
    }

    // ---------------- Thuốc ----------------

    private ThuocDTO ThuocMau(int ton = 50) => new()
    {
        TenThuoc = $"ZTest Thuốc {_duoi}", Nsx = "Công ty kiểm thử", HamLuong = 250.555m, DonViTinh = "Viên",
        DonGiaThuoc = 1500, SoLuongTon = ton, MoTa = "",
    };

    [Fact]
    public void Thuoc_Them_VaSuaKhongXacNhanThiGiuTon()
    {
        var ma = _db.Xoa("Thuoc", "ma_thuoc", _thuoc.Them(ThuocMau(50)));
        var t = _thuoc.DanhSach(ma).Single();
        Assert.Equal(250.56m, t.HamLuong); // làm tròn 2 chữ số như cột decimal(10,2)
        Assert.Null(t.MoTa);

        t.DonGiaThuoc = 2000;
        t.SoLuongTon = 999;
        _thuoc.Sua(t, suaTon: false);
        t = _thuoc.DanhSach(ma).Single();
        Assert.Equal(2000, t.DonGiaThuoc);
        Assert.Equal(50, t.SoLuongTon); // không tick xác nhận: tồn giữ nguyên

        t.SoLuongTon = 10;
        _thuoc.Sua(t, suaTon: true);
        t = _thuoc.DanhSach(ma).Single();
        Assert.Equal(10, t.SoLuongTon);
        Assert.True(t.SapHet);
        Assert.Contains(_thuoc.DanhSach(chiSapHet: true), x => x.MaThuoc == ma);
    }

    [Fact]
    public void Thuoc_TrungTenNsxHamLuong()
    {
        _db.Xoa("Thuoc", "ma_thuoc", _thuoc.Them(ThuocMau()));
        Loi("Thuốc cùng tên, nhà sản xuất và hàm lượng đã tồn tại.", () => _thuoc.Them(ThuocMau()));
    }

    [Theory]
    [InlineData(0, 1000, 1, "Hàm lượng phải lớn hơn 0.")]
    [InlineData(5, 0, 1, "Đơn giá phải lớn hơn 0.")]
    [InlineData(5, 1000.5, 1, "Đơn giá tính theo đồng, không có phần lẻ.")]
    [InlineData(5, 1000, -1, "Số lượng tồn không được âm.")]
    public void Thuoc_SoKhongHopLe(double hamLuong, double gia, int ton, string thongBao)
    {
        var t = ThuocMau(ton);
        t.HamLuong = (decimal)hamLuong;
        t.DonGiaThuoc = (decimal)gia;
        Loi(thongBao, () => _thuoc.Them(t));
    }

    [Fact]
    public void Thuoc_ThieuNhaSanXuat()
    {
        var t = ThuocMau();
        t.Nsx = " ";
        Loi("Vui lòng nhập nhà sản xuất.", () => _thuoc.Them(t));
    }

    // ---------------- Phân quyền ----------------

    [Fact]
    public void PhanQuyen_LeTanXemDanhMucNhungKhongSua_KhongXemThuoc()
    {
        PhienDangNhap.DangXuat();
        _db.DangNhap(ChucVu.LeTan);
        Assert.NotEmpty(_khoa.DanhSach());
        Assert.NotEmpty(_ca.DanhSach());
        Assert.NotEmpty(_phong.DanhSach());
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _khoa.Them("ZTest", null));
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _ca.Xoa("CA0001"));
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _thuoc.DanhSach());
    }

    [Fact]
    public void PhanQuyen_BacSiXemThuocNhungKhongSua()
    {
        PhienDangNhap.DangXuat();
        _db.DangNhap(ChucVu.BacSi);
        var t = _thuoc.DanhSach().First();
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _thuoc.Sua(t, false));
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _phong.Xoa("P0001"));
    }
}
