using KmnClinic.DTO;

namespace KmnClinic.GUI;

/// <summary>Nhân viên đang đăng nhập. Mọi thao tác ghi "người thực hiện" lấy mã từ đây (3 câu hỏi truy vết).</summary>
public static class PhienDangNhap
{
    public static NhanVienDTO? NhanVien { get; private set; }

    public static string VaiTro => NhanVien?.ChucVu ?? "";

    public static void DangNhap(NhanVienDTO nv) => NhanVien = nv;

    public static void DangXuat() => NhanVien = null;
}
