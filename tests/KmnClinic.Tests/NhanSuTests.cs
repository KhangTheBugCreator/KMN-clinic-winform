using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>W2: nhân viên, khách hàng. Test tự thêm người có SĐT 09999xxxxx rồi xóa khi xong.</summary>
public class NhanSuTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly NhanVienBUS _nv = new();
    private readonly KhachHangBUS _kh = new();

    public NhanSuTests() => _db.DangNhap(ChucVu.QuanTriVien);

    public void Dispose() => _db.Dispose();

    private static string SdtThu() => "09999" + Random.Shared.Next(0, 100000).ToString("D5");

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    private NhanVienDTO NvMau(string chucVu = ChucVu.LeTan, string? khoa = null) => new()
    {
        HoTenNv = " ZTest Nhân viên ", NgaySinhNv = new DateTime(1995, 5, 20, 14, 30, 0), SdtNv = SdtThu(),
        ChucVu = chucVu, MaKhoa = khoa, DuongDanAnh = "",
    };

    private string ThemNv(NhanVienDTO nv, string mk = "matkhau123") => _db.Xoa("Nhan_vien", "ma_nv", _nv.Them(nv, mk));

    // ---------------- Nhân viên ----------------

    [Fact]
    public void NhanVien_Them_MatKhauBam_DangNhapDuoc()
    {
        var nv = NvMau();
        var ma = ThemNv(nv, "matkhau-moi1");
        var luu = _nv.DanhSach(ma).Single();
        Assert.Equal("ZTest Nhân viên", luu.HoTenNv);
        Assert.Equal(new DateTime(1995, 5, 20), luu.NgaySinhNv);
        Assert.Null(luu.DuongDanAnh);
        Assert.StartsWith("PBKDF2-SHA256$", _db.LayMatKhauLuu(ma));

        PhienDangNhap.DangXuat();
        Assert.Equal(ma, new NhanVienBUS().DangNhap(nv.SdtNv, "matkhau-moi1").MaNv);
    }

    [Fact]
    public void NhanVien_KhacBacSi_KhongCoKhoa_BacSiPhaiCoKhoa()
    {
        var ma = ThemNv(NvMau(ChucVu.ThuNgan, "K0001")); // khoa bị bỏ vì không phải bác sĩ (RB6)
        Assert.Null(_nv.DanhSach(ma).Single().MaKhoa);

        Loi("Bác sĩ phải thuộc một khoa (RB6).", () => _nv.Them(NvMau(ChucVu.BacSi), "matkhau123"));
        var bs = ThemNv(NvMau(ChucVu.BacSi, "K0001"));
        Assert.Equal("K0001", _nv.DanhSach(bs).Single().MaKhoa);
    }

    [Theory]
    [InlineData("090711223", "Số điện thoại gồm 10 chữ số, bắt đầu bằng 0.")]
    [InlineData("1907112233", "Số điện thoại gồm 10 chữ số, bắt đầu bằng 0.")]
    [InlineData("09071122ab", "Số điện thoại gồm 10 chữ số, bắt đầu bằng 0.")]
    [InlineData("", "Vui lòng nhập số điện thoại.")]
    public void NhanVien_SdtSai(string sdt, string thongBao)
    {
        var nv = NvMau();
        nv.SdtNv = sdt;
        Loi(thongBao, () => _nv.Them(nv, "matkhau123"));
    }

    [Fact]
    public void NhanVien_TrungSdt_MatKhauNgan_NgaySinhTuongLai_AnhKhongHttps()
    {
        var nv = NvMau();
        nv.SdtNv = "0907112233"; // SĐT của quản trị viên mẫu
        Loi("Số điện thoại nhân viên đã được sử dụng.", () => _nv.Them(nv, "matkhau123"));
        Loi("Mật khẩu tối thiểu 8 ký tự.", () => _nv.Them(NvMau(), "1234567"));

        var tl = NvMau();
        tl.NgaySinhNv = DateTime.Today.AddDays(1);
        Loi("Ngày sinh không được ở tương lai.", () => _nv.Them(tl, "matkhau123"));

        var anh = NvMau();
        anh.DuongDanAnh = "http://anh.vn/a.jpg";
        Loi("Đường dẫn ảnh phải bắt đầu bằng https://", () => _nv.Them(anh, "matkhau123"));
    }

    [Fact]
    public void NhanVien_Sua_DoiChucVuKhiChuaCoDuLieu_VaDatLaiMatKhau()
    {
        var nv = NvMau();
        var ma = ThemNv(nv);
        nv.MaNv = ma;
        nv.ChucVu = ChucVu.BacSi;
        nv.MaKhoa = "K0002";
        nv.DuongDanAnh = "https://kmnclinic.click/anh/bs.jpg";
        _nv.Sua(nv, "");
        var luu = _nv.DanhSach(ma).Single();
        Assert.Equal(ChucVu.BacSi, luu.ChucVu);
        Assert.Equal("K0002", luu.MaKhoa);
        Assert.True(MatKhau.KiemTra("matkhau123", _db.LayMatKhauLuu(ma))); // để trống: giữ mật khẩu

        _nv.Sua(nv, "matkhau-dat-lai");
        Assert.True(MatKhau.KiemTra("matkhau-dat-lai", _db.LayMatKhauLuu(ma)));
    }

    [Fact]
    public void NhanVien_DaCoDuLieu_KhongDoiChucVu_KhongXoa()
    {
        var bs = _nv.DanhSach(chucVu: ChucVu.BacSi).First(n => n.SoLienKet > 0);
        var sua = new NhanVienDTO
        {
            MaNv = bs.MaNv, HoTenNv = bs.HoTenNv, NgaySinhNv = bs.NgaySinhNv, SdtNv = bs.SdtNv,
            ChucVu = bs.ChucVu, MaKhoa = bs.MaKhoa == "K0001" ? "K0002" : "K0001", DuongDanAnh = bs.DuongDanAnh,
        };
        Assert.StartsWith("Không thể đổi chức vụ hoặc khoa", Assert.Throws<LoiNghiepVu>(() => _nv.Sua(sua, null)).Message);
        Assert.StartsWith("Không thể xóa: nhân viên đã có", Assert.Throws<LoiNghiepVu>(() => _nv.Xoa(bs.MaNv)).Message);
        Assert.Equal(bs.MaKhoa, _nv.DanhSach(bs.MaNv).Single().MaKhoa);
    }

    [Fact]
    public void NhanVien_QuanTriKhongTuXoa_KhongTuBoChucVu()
    {
        var toi = PhienDangNhap.NhanVien!;
        Loi("Bạn không thể tự xóa chính mình.", () => _nv.Xoa(toi.MaNv));
        var sua = new NhanVienDTO { MaNv = toi.MaNv, HoTenNv = toi.HoTenNv, SdtNv = toi.SdtNv, ChucVu = ChucVu.LeTan };
        Loi("Bạn không thể tự đổi chức vụ của mình khi đang ở vai trò quản trị viên.", () => _nv.Sua(sua, null));
    }

    [Fact]
    public void NhanVien_XoaNhanVienMoi()
    {
        var ma = _nv.Them(NvMau(), "matkhau123");
        _nv.Xoa(ma);
        Assert.Empty(_nv.DanhSach(ma));
    }

    [Fact]
    public void NhanVien_LocTheoChucVu()
    {
        var ds = _nv.DanhSach(chucVu: ChucVu.BacSi);
        Assert.NotEmpty(ds);
        Assert.All(ds, n => { Assert.Equal(ChucVu.BacSi, n.ChucVu); Assert.NotNull(n.MaKhoa); });
    }

    // ---------------- Khách hàng ----------------

    private KhachHangDTO KhMau() => new()
    {
        HoTenKh = "ZTest Khách", NgaySinhKh = new DateTime(1990, 1, 2), GioiTinhKh = "Nữ", SdtKh = SdtThu(), DiaChiKh = " ",
    };

    [Fact]
    public void KhachHang_ThemSuaXoa_TimTheoSdt()
    {
        var kh = KhMau();
        var ma = _kh.Them(kh);
        Assert.Matches("^KH[0-9]{4,}$", ma);
        var luu = _kh.TimTheoSdt(kh.SdtKh)!;
        Assert.Equal(ma, luu.MaKh);
        Assert.Null(luu.DiaChiKh);
        Assert.Null(_db.GiaTri($"SELECT mat_khau_kh FROM Khach_hang WHERE ma_kh = '{ma}'") as string); // khách không có tài khoản

        kh.MaKh = ma;
        kh.GioiTinhKh = "Nam";
        kh.DiaChiKh = "12 Nguyễn Huệ, Vĩnh Long";
        _kh.Sua(kh);
        Assert.Equal("Nam", _kh.DanhSach(ma).Single().GioiTinhKh);

        _kh.Xoa(ma);
        Assert.Null(_kh.TimTheoSdt(kh.SdtKh));
    }

    [Fact]
    public void KhachHang_DuLieuSai()
    {
        var a = KhMau(); a.GioiTinhKh = null;
        Loi("Vui lòng chọn giới tính.", () => _kh.Them(a));
        var b = KhMau(); b.NgaySinhKh = null;
        Loi("Vui lòng nhập ngày sinh.", () => _kh.Them(b));
        var c = KhMau(); c.SdtKh = _kh.DanhSach().First().SdtKh;
        Loi("Số điện thoại khách hàng đã được sử dụng.", () => _kh.Them(c));
    }

    [Fact]
    public void KhachHang_DaCoLichHen_KhongXoa()
    {
        var kh = _kh.DanhSach().First(k => k.SoLichHen > 0);
        Assert.StartsWith("Không thể xóa: khách hàng đã có", Assert.Throws<LoiNghiepVu>(() => _kh.Xoa(kh.MaKh)).Message);
    }

    // ---------------- Phân quyền ----------------

    [Fact]
    public void PhanQuyen_LeTanThemSuaKhachNhungKhongXoa_KhongXemNhanVien()
    {
        PhienDangNhap.DangXuat();
        _db.DangNhap(ChucVu.LeTan);
        var kh = KhMau();
        var ma = _db.Xoa("Khach_hang", "ma_kh", _kh.Them(kh));
        kh.MaKh = ma;
        kh.HoTenKh = "ZTest Khách sửa";
        _kh.Sua(kh);
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _kh.Xoa(ma));
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _nv.DanhSach());
    }

    [Fact]
    public void PhanQuyen_BacSiKhongXemKhachHang()
    {
        PhienDangNhap.DangXuat();
        _db.DangNhap(ChucVu.BacSi);
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _kh.DanhSach());
    }
}
