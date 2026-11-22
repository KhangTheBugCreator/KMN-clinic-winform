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
    private readonly List<(string Bang, string Cot, string Ma)> _canXoa = [];

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
            // CK_Nhan_vien_khoa: chỉ Bác sĩ có khoa
            using var cmd = c.Lenh(@"INSERT INTO Nhan_vien (ma_nv, ho_ten_nv, sdt_nv, mat_khau_nv, chuc_vu, ma_khoa)
                                     VALUES (@ma, N'Nhân viên kiểm thử', @sdt, @mk, @cv, @khoa)", t)
                .P("@ma", ma).P("@sdt", sdt).P("@mk", matKhauLuu).P("@cv", chucVu)
                .P("@khoa", chucVu == "Bác sĩ" ? "K0001" : null);
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

    /// <summary>Thêm nhân viên thử có vai trò cho trước rồi đăng nhập bằng tài khoản đó.</summary>
    public void DangNhap(string chucVu)
    {
        var (_, sdt) = ThemNhanVien(KmnClinic.BUS.MatKhau.Bam("matkhau123"), chucVu);
        new KmnClinic.BUS.NhanVienBUS().DangNhap(sdt, "matkhau123");
    }

    /// <summary>Ghi nhận một dòng do test thêm để xóa khi xong (xóa theo thứ tự ngược lại).</summary>
    public string Xoa(string bang, string cot, string ma)
    {
        _canXoa.Add((bang, cot, ma));
        return ma;
    }

    /// <summary>Chạy một câu SELECT trả về một giá trị.</summary>
    public object? GiaTri(string sql)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(sql);
        return cmd.ExecuteScalar();
    }

    public string LayMatKhauLuu(string ma) => new NhanVienDAL().LayMatKhau(ma)!;

    public void Dispose()
    {
        KmnClinic.BUS.PhienDangNhap.DangXuat();
        using var conn = DbHelper.MoKetNoi();
        for (var i = _canXoa.Count - 1; i >= 0; i--)
        {
            var (bang, cot, ma) = _canXoa[i];
            using var cmd = conn.Lenh($"DELETE FROM {bang} WHERE {cot} = @ma").P("@ma", ma);
            cmd.ExecuteNonQuery();
        }
        foreach (var ma in _maDaThem)
        {
            using var cmd = conn.Lenh("DELETE FROM Nhan_vien WHERE ma_nv = @ma").P("@ma", ma);
            cmd.ExecuteNonQuery();
        }
    }
}
