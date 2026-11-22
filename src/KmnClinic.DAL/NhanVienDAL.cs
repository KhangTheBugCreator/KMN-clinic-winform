using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>Truy cập bảng Nhan_vien phục vụ đăng nhập và đổi mật khẩu (W0). Thêm/sửa/xóa nhân viên làm ở W2.</summary>
public class NhanVienDAL
{
    private const string CotNhanVien = @"
SELECT nv.ma_nv, nv.ho_ten_nv, nv.ngay_sinh_nv, nv.sdt_nv, nv.chuc_vu, nv.ma_khoa, k.ten_khoa, nv.duong_dan_anh, nv.mat_khau_nv
FROM Nhan_vien nv LEFT JOIN Khoa k ON k.ma_khoa = nv.ma_khoa";

    /// <summary>Tìm nhân viên theo số điện thoại (sdt_nv là UNIQUE). Trả về cả mật khẩu đã lưu để BUS kiểm tra.</summary>
    public (NhanVienDTO NhanVien, string MatKhauDaLuu)? TimTheoSdt(string sdt)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(CotNhanVien + " WHERE nv.sdt_nv = @sdt").P("@sdt", SqlDbType.VarChar, 10, sdt);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (DocNhanVien(r), r.GetString(r.GetOrdinal("mat_khau_nv")));
    }

    /// <summary>Lấy mật khẩu đã lưu của một nhân viên (null nếu không có nhân viên đó).</summary>
    public string? LayMatKhau(string maNv)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh("SELECT mat_khau_nv FROM Nhan_vien WHERE ma_nv = @ma").P("@ma", SqlDbType.VarChar, 10, maNv);
        return cmd.ExecuteScalar() as string;
    }

    /// <summary>Ghi mật khẩu mới (đã băm). Trả về true nếu cập nhật được một dòng.</summary>
    public bool CapNhatMatKhau(string maNv, string matKhauDaBam)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh("UPDATE Nhan_vien SET mat_khau_nv = @mk WHERE ma_nv = @ma")
            .P("@mk", SqlDbType.VarChar, 255, matKhauDaBam)
            .P("@ma", SqlDbType.VarChar, 10, maNv);
        return cmd.ExecuteNonQuery() == 1;
    }

    internal static NhanVienDTO DocNhanVien(SqlDataReader r) => new()
    {
        MaNv = r.GetString(r.GetOrdinal("ma_nv")),
        HoTenNv = r.GetString(r.GetOrdinal("ho_ten_nv")),
        NgaySinhNv = r.NgayHoacNull("ngay_sinh_nv"),
        SdtNv = r.GetString(r.GetOrdinal("sdt_nv")),
        ChucVu = r.GetString(r.GetOrdinal("chuc_vu")),
        MaKhoa = r.ChuoiHoacNull("ma_khoa"),
        TenKhoa = r.ChuoiHoacNull("ten_khoa"),
        DuongDanAnh = r.ChuoiHoacNull("duong_dan_anh"),
    };
}
