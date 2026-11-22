namespace KmnClinic.BUS;

/// <summary>
/// Kiểm tra quyền ở tầng BUS (RB38–RB39). GUI đã ẩn menu theo vai trò,
/// nhưng mỗi hàm BUS ghi dữ liệu vẫn gọi YeuCau để không phụ thuộc vào giao diện.
/// </summary>
public static class PhanQuyen
{
    public static bool Co(params string[] vaiTro) => PhienDangNhap.DaDangNhap && vaiTro.Contains(PhienDangNhap.VaiTro);

    /// <summary>Ném LoiNghiepVu nếu chưa đăng nhập hoặc vai trò hiện tại không nằm trong danh sách.</summary>
    public static void YeuCau(params string[] vaiTro)
    {
        if (!PhienDangNhap.DaDangNhap)
            throw new LoiNghiepVu("Phiên đăng nhập đã kết thúc. Vui lòng đăng nhập lại.");
        if (!vaiTro.Contains(PhienDangNhap.VaiTro))
            throw new LoiNghiepVu("Bạn không có quyền thực hiện chức năng này.");
    }
}
