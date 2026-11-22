using KmnClinic.DAL;
using Microsoft.Data.SqlClient;

// Các test dùng chung PhienDangNhap (static) và cùng CSDL nên chạy tuần tự.
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace KmnClinic.Tests;

/// <summary>
/// Kết nối CSDL PHONGKHAM để kiểm thử. Chuỗi kết nối lấy từ biến môi trường KMN_TEST_DB,
/// không có thì dùng SQL Server trên máy (đăng nhập Windows).
/// Mỗi test tự thêm nhân viên thử (SĐT 09999xxxxx) và xóa khi xong, không đụng dữ liệu mẫu.
/// </summary>
public sealed class CsdlThu : IDisposable
{
    private readonly List<string> _maDaThem = [];

    public CsdlThu()
    {
        DbHelper.ChuoiKetNoi = Environment.GetEnvironmentVariable("KMN_TEST_DB")
            ?? "Server=.;Database=PHONGKHAM;Trusted_Connection=True;TrustServerCertificate=True";
    }

    /// <summary>Thêm một nhân viên thử với mật khẩu lưu đúng như tham số (bản gốc hoặc đã băm).</summary>
    public (string Ma, string Sdt) ThemNhanVien(string matKhauLuu, string chucVu = "Lễ tân")
    {
        var sdt = "09999" + Random.Shared.Next(0, 100000).ToString("D5");
        var ma = DbHelper.TrongGiaoDich((c, t) =>
        {
            var ma = MaSoGenerator.TaoMa(c, t, LoaiMa.NhanVien);
            using var cmd = c.Lenh(@"INSERT INTO Nhan_vien (ma_nv, ho_ten_nv, sdt_nv, mat_khau_nv, chuc_vu, ma_khoa)
                                     VALUES (@ma, N'Nhân viên kiểm thử', @sdt, @mk, @cv, NULL)", t)
                .P("@ma", ma).P("@sdt", sdt).P("@mk", matKhauLuu).P("@cv", chucVu);
            cmd.ExecuteNonQuery();
            return ma;
        });
        _maDaThem.Add(ma);
        return (ma, sdt);
    }

    /// <summary>Thêm nhân viên với SĐT cho trước (để thử vi phạm UNIQUE). Không ghi nhận để xóa vì dự kiến lỗi.</summary>
    public void ThemNhanVienVoiSdt(string sdt) => DbHelper.TrongGiaoDich((c, t) =>
    {
        var ma = MaSoGenerator.TaoMa(c, t, LoaiMa.NhanVien);
        c.Lenh(@"INSERT INTO Nhan_vien (ma_nv, ho_ten_nv, sdt_nv, mat_khau_nv, chuc_vu, ma_khoa)
                 VALUES (@ma, N'Trùng SĐT', @sdt, 'x', N'Lễ tân', NULL)", t).P("@ma", ma).P("@sdt", sdt).ExecuteNonQuery();
    });

    public string LayMatKhauLuu(string ma) => new NhanVienDAL().LayMatKhau(ma)!;

    public void Dispose()
    {
        KmnClinic.BUS.PhienDangNhap.DangXuat();
        using var conn = DbHelper.MoKetNoi();
        foreach (var ma in _maDaThem)
        {
            using var cmd = conn.Lenh("DELETE FROM Nhan_vien WHERE ma_nv = @ma").P("@ma", ma);
            cmd.ExecuteNonQuery();
        }
    }
}
