using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Nhân viên đang đăng nhập. Mọi thao tác ghi "người thực hiện" lấy mã từ đây (3 câu hỏi truy vết).</summary>
public static class PhienDangNhap
{
    public static NhanVienDTO? NhanVien { get; private set; }

    public static string VaiTro => NhanVien?.ChucVu ?? "";

    public static bool DaDangNhap => NhanVien is not null;

    internal static void DangNhap(NhanVienDTO nv) => NhanVien = nv;

    public static void DangXuat() => NhanVien = null;
}
