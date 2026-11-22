using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

public class DangNhapTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly NhanVienBUS _bus = new();

    public void Dispose() => _db.Dispose();

    [Fact]
    public void DangNhap_Dung_LuuVaoPhien()
    {
        var (ma, sdt) = _db.ThemNhanVien(MatKhau.Bam("matkhau-dung"));
        var nv = _bus.DangNhap(sdt, "matkhau-dung");
        Assert.Equal(ma, nv.MaNv);
        Assert.Equal(ChucVu.LeTan, PhienDangNhap.VaiTro);
        Assert.Null(nv.MaKhoa);
    }

    [Fact]
    public void DangNhap_SaiMatKhau_BaoLoiChung()
    {
        var (_, sdt) = _db.ThemNhanVien(MatKhau.Bam("matkhau-dung"));
        var loi = Assert.Throws<LoiNghiepVu>(() => _bus.DangNhap(sdt, "matkhau-sai"));
        Assert.Equal("Số điện thoại hoặc mật khẩu không đúng.", loi.Message);
        Assert.False(PhienDangNhap.DaDangNhap);
    }

    [Fact]
    public void DangNhap_SdtKhongTonTai_BaoCungMotCau()
    {
        var loi = Assert.Throws<LoiNghiepVu>(() => _bus.DangNhap("0999900000", "matkhau123"));
        Assert.Equal("Số điện thoại hoặc mật khẩu không đúng.", loi.Message);
    }

    [Theory]
    [InlineData("", "matkhau123", "Vui lòng nhập số điện thoại.")]
    [InlineData("0907112233", "1234567", "Mật khẩu tối thiểu 8 ký tự.")]
    public void DangNhap_DuLieuNhapSai(string sdt, string mk, string thongBao)
        => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(() => _bus.DangNhap(sdt, mk)).Message);

    [Fact]
    public void DangNhap_MatKhauBanGoc_DuocBamLai()
    {
        var (ma, sdt) = _db.ThemNhanVien("matkhau123");
        _bus.DangNhap(sdt, "matkhau123");

        var luu = _db.LayMatKhauLuu(ma);
        Assert.StartsWith("PBKDF2-SHA256$", luu);
        Assert.True(MatKhau.KiemTra("matkhau123", luu));

        // Lần sau vẫn đăng nhập được bằng mật khẩu cũ (đã băm)
        PhienDangNhap.DangXuat();
        Assert.Equal(ma, _bus.DangNhap(sdt, "matkhau123").MaNv);
    }

    [Fact]
    public void DoiMatKhau_ThanhCong_DangNhapBangMatKhauMoi()
    {
        var (ma, sdt) = _db.ThemNhanVien(MatKhau.Bam("matkhau-cu1"));
        _bus.DangNhap(sdt, "matkhau-cu1");
        _bus.DoiMatKhau("matkhau-cu1", "matkhau-moi1", "matkhau-moi1");

        PhienDangNhap.DangXuat();
        Assert.Throws<LoiNghiepVu>(() => _bus.DangNhap(sdt, "matkhau-cu1"));
        Assert.Equal(ma, _bus.DangNhap(sdt, "matkhau-moi1").MaNv);
    }

    [Theory]
    [InlineData("sai-mat-khau", "matkhau-moi1", "matkhau-moi1", "Mật khẩu hiện tại không đúng.")]
    [InlineData("matkhau-cu1", "ngan", "ngan", "Mật khẩu mới tối thiểu 8 ký tự.")]
    [InlineData("matkhau-cu1", "matkhau-moi1", "matkhau-moi2", "Nhập lại mật khẩu mới không khớp.")]
    [InlineData("matkhau-cu1", "matkhau-cu1", "matkhau-cu1", "Mật khẩu mới phải khác mật khẩu hiện tại.")]
    public void DoiMatKhau_DuLieuSai(string cu, string moi, string nhapLai, string thongBao)
    {
        var (_, sdt) = _db.ThemNhanVien(MatKhau.Bam("matkhau-cu1"));
        _bus.DangNhap(sdt, "matkhau-cu1");
        Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(() => _bus.DoiMatKhau(cu, moi, nhapLai)).Message);
    }

    [Fact]
    public void DoiMatKhau_ChuaDangNhap_BaoLoi()
        => Assert.Throws<LoiNghiepVu>(() => _bus.DoiMatKhau("a", "matkhau-moi1", "matkhau-moi1"));
}
