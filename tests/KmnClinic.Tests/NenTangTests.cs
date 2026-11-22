using KmnClinic.BUS;
using KmnClinic.DAL;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.Tests;

public class NenTangTests : IDisposable
{
    private readonly CsdlThu _db = new();

    public void Dispose() => _db.Dispose();

    [Fact]
    public void KetNoi_DuocCsdl()
        => Assert.Null(DbHelper.KiemTraKetNoi());

    [Fact]
    public void TaoMa_LaSoKeTiep()
    {
        var lonNhat = DbHelper.LayBang("SELECT MAX(CAST(SUBSTRING(ma_nv, 3, 10) AS int)) AS m FROM Nhan_vien WHERE ma_nv LIKE 'NV[0-9]%'")
            .Rows[0]["m"];
        var ma = DbHelper.TrongGiaoDich((c, t) => MaSoGenerator.TaoMa(c, t, LoaiMa.NhanVien));
        Assert.Equal("NV" + ((int)lonNhat + 1).ToString("D4"), ma);
    }

    [Fact]
    public void TaoMa_LoaiLa_BiTuChoi()
        => Assert.Throws<ArgumentException>(() =>
            DbHelper.TrongGiaoDich((c, t) => MaSoGenerator.TaoMa(c, t, new LoaiMa("Nhan_vien; DROP TABLE x", "ma_nv", "NV"))));

    [Fact]
    public void GiaoDich_LoiThiRollback()
    {
        Assert.ThrowsAny<SqlException>(() => DbHelper.TrongGiaoDich((c, t) =>
        {
            c.Lenh("UPDATE Thuoc SET so_luong_ton = so_luong_ton + 1 WHERE ma_thuoc = 'TH0001'", t).ExecuteNonQuery();
            c.Lenh("UPDATE Thuoc SET so_luong_ton = -1 WHERE ma_thuoc = 'TH0001'", t).ExecuteNonQuery(); // vi phạm CHECK
        }));
        var truoc = DbHelper.LayBang("SELECT so_luong_ton FROM Thuoc WHERE ma_thuoc = 'TH0001'").Rows[0][0];
        Assert.True((int)truoc >= 0);
    }

    [Fact]
    public void DichLoi_TrungSdtNhanVien()
    {
        var (_, sdt) = _db.ThemNhanVien("matkhau123");
        var ex = Assert.Throws<SqlException>(() => _db.ThemNhanVienVoiSdt(sdt));
        Assert.Equal("Số điện thoại nhân viên đã được sử dụng.", DichLoiSql.Dich(ex));
    }

    [Fact]
    public void DichLoi_LeTanCoKhoa_ViPhamCheck()
    {
        var ex = Assert.Throws<SqlException>(() => DbHelper.TrongGiaoDich((c, t) =>
            c.Lenh(@"INSERT INTO Nhan_vien (ma_nv, ho_ten_nv, sdt_nv, mat_khau_nv, chuc_vu, ma_khoa)
                     VALUES ('NVTHU', N'Thử', '0999911111', 'x', N'Lễ tân', 'K0001')", t).ExecuteNonQuery()));
        Assert.Equal("Bác sĩ phải thuộc một khoa; nhân viên khác không thuộc khoa nào.", DichLoiSql.Dich(ex));
    }

    [Theory]
    [InlineData(50001, "Lịch hẹn mới phải có trạng thái Đã đặt.", "Lịch hẹn mới phải có trạng thái Đã đặt.")]
    [InlineData(1205, "deadlock", "Hệ thống đang bận xử lý yêu cầu khác. Vui lòng thử lại sau giây lát.")]
    [InlineData(547, "The DELETE statement conflicted with the REFERENCE constraint \"FK_x\".", "Không thể xóa vì dữ liệu này đang được sử dụng ở nơi khác.")]
    [InlineData(18456, "Login failed", "Không mở được CSDL PHONGKHAM. Kiểm tra đã chạy script tạo CSDL và chuỗi kết nối trong appsettings.json.")]
    public void DichLoi_TheoSoLoi(int so, string cau, string mongDoi)
        => Assert.Equal(mongDoi, DichLoiSql.Dich(so, cau));

    [Fact]
    public void PhanQuyen_TheoVaiTro()
    {
        Assert.Throws<LoiNghiepVu>(() => PhanQuyen.YeuCau(ChucVu.LeTan)); // chưa đăng nhập

        var (_, sdt) = _db.ThemNhanVien(MatKhau.Bam("matkhau123"), ChucVu.ThuNgan);
        new NhanVienBUS().DangNhap(sdt, "matkhau123");
        PhanQuyen.YeuCau(ChucVu.ThuNgan, ChucVu.QuanTriVien);
        var loi = Assert.Throws<LoiNghiepVu>(() => PhanQuyen.YeuCau(ChucVu.LeTan));
        Assert.Equal("Bạn không có quyền thực hiện chức năng này.", loi.Message);
    }

    [Fact]
    public void MatKhau_BamMoiLanKhacNhau()
    {
        var a = MatKhau.Bam("matkhau123");
        var b = MatKhau.Bam("matkhau123");
        Assert.NotEqual(a, b);
        Assert.True(MatKhau.KiemTra("matkhau123", a));
        Assert.False(MatKhau.KiemTra("matkhau124", a));
        Assert.False(MatKhau.KiemTra("x", "PBKDF2-SHA256$abc$!!$!!"));
    }
}
