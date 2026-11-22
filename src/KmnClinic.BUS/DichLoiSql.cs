using Microsoft.Data.SqlClient;

namespace KmnClinic.BUS;

/// <summary>
/// Đổi lỗi SQL Server thành câu tiếng Việt cho người dùng (cùng quy tắc với bản web):
/// - THROW 50000–50999 trong trigger: câu thông báo đã là tiếng Việt, trả nguyên văn.
/// - Vi phạm UNIQUE (2627, 2601) và CHECK / khóa ngoại (547): câu theo tên ràng buộc.
/// - Không kết nối được: hướng dẫn kiểm tra SQL Server và appsettings.json.
/// Lỗi khác trả null (GUI hiện câu lỗi gốc).
/// </summary>
public static class DichLoiSql
{
    // Tên ràng buộc lấy đúng từ database/cau-truc-phongkham.sql.
    private static readonly Dictionary<string, string> ThongBaoRangBuoc = new(StringComparer.OrdinalIgnoreCase)
    {
        ["UQ_Khoa_ten_khoa"] = "Tên khoa đã tồn tại.",
        ["UQ_PhongLamViec_ten_phong"] = "Tên phòng đã tồn tại.",
        ["UQ_Nhan_vien_sdt_nv"] = "Số điện thoại nhân viên đã được sử dụng.",
        ["UQ_Khach_hang_sdt_kh"] = "Số điện thoại khách hàng đã được sử dụng.",
        ["UQ_LICH_LAM_VIEC_nv_ca_ngay"] = "Nhân viên này đã có lịch làm việc ở ca đó trong ngày.",
        ["UQ_LICH_LAM_VIEC_phong_ca_ngay"] = "Phòng này đã được xếp lịch ở ca đó trong ngày.",
        ["UQ_Thuoc_ten_nsx_ham_luong"] = "Thuốc cùng tên, nhà sản xuất và hàm lượng đã tồn tại.",
        ["UQ_Hoa_don_ma_ho_so"] = "Hồ sơ khám này đã có hóa đơn.",
        ["PK_Chi_tiet_don_thuoc"] = "Thuốc này đã có trong đơn.",
        ["UX_Ho_so_kham_ma_lich_hen"] = "Lịch hẹn này đã được tiếp nhận thành hồ sơ khám.",
        ["CK_Ca_lam_viec_gio"] = "Giờ bắt đầu ca phải nhỏ hơn giờ kết thúc.",
        ["CK_CTDT_so_luong"] = "Số lượng thuốc phải lớn hơn 0.",
        ["CK_CTDT_don_gia"] = "Đơn giá thuốc phải lớn hơn 0.",
        ["CK_Ho_so_kham_tien_cong"] = "Tiền công khám không được âm.",
        ["CK_Hoa_don_tong_tien"] = "Tổng tiền không được âm.",
        ["CK_Hoa_don_trang_thai"] = "Trạng thái thanh toán không hợp lệ.",
        ["CK_Hoa_don_thanh_toan"] = "Thông tin thanh toán không khớp với trạng thái hóa đơn.",
        ["CK_Khach_hang_gioi_tinh"] = "Giới tính chỉ nhận Nam hoặc Nữ.",
        ["CK_LICH_HEN_trang_thai"] = "Trạng thái lịch hẹn không hợp lệ.",
        ["CK_LICH_HEN_le_tan"] = "Lịch hẹn đã xác nhận hoặc đã khám phải có lễ tân phụ trách.",
        ["CK_Nhan_vien_chuc_vu"] = "Chức vụ không hợp lệ.",
        ["CK_Nhan_vien_khoa"] = "Bác sĩ phải thuộc một khoa; nhân viên khác không thuộc khoa nào.",
        ["CK_Thuoc_don_gia"] = "Đơn giá thuốc phải lớn hơn 0.",
        ["CK_Thuoc_so_luong_ton"] = "Số lượng tồn không được âm.",
    };

    public static string? Dich(SqlException ex)
    {
        // Lỗi đầu tiên là lỗi gốc; các lỗi sau thường là "The statement has been terminated."
        var loi = ex.Errors.Count > 0 ? ex.Errors[0] : null;
        var so = loi?.Number ?? ex.Number;
        var thongDiep = loi?.Message ?? ex.Message;
        return Dich(so, thongDiep);
    }

    /// <summary>Tách riêng theo (số lỗi, câu lỗi) để kiểm thử được mà không cần tạo SqlException.</summary>
    public static string? Dich(int so, string thongDiep)
    {
        if (so is >= 50000 and <= 50999) return thongDiep;

        if (so == 1205) return "Hệ thống đang bận xử lý yêu cầu khác. Vui lòng thử lại sau giây lát.";

        if (so is 2627 or 2601) return TimThongBao(thongDiep) ?? "Dữ liệu bị trùng với bản ghi đã có.";

        if (so == 547)
        {
            if (thongDiep.Contains("CHECK constraint", StringComparison.OrdinalIgnoreCase))
                return TimThongBao(thongDiep) ?? "Dữ liệu không thỏa ràng buộc của hệ thống.";
            if (thongDiep.Contains("DELETE statement", StringComparison.OrdinalIgnoreCase))
                return "Không thể xóa vì dữ liệu này đang được sử dụng ở nơi khác.";
            return "Dữ liệu tham chiếu không tồn tại hoặc không hợp lệ.";
        }

        // Không kết nối được máy chủ (sai tên server, SQL Server chưa chạy) hoặc sai tên CSDL / đăng nhập
        if (so is 2 or 53 or -1 or 40 or 26 or 10061)
            return "Không kết nối được SQL Server. Kiểm tra SQL Server đã chạy và chuỗi kết nối trong appsettings.json.";
        if (so is 4060 or 18456)
            return "Không mở được CSDL PHONGKHAM. Kiểm tra đã chạy script tạo CSDL và chuỗi kết nối trong appsettings.json.";

        return null;
    }

    private static string? TimThongBao(string thongDiep)
    {
        foreach (var (ten, thongBao) in ThongBaoRangBuoc)
            if (thongDiep.Contains($"'{ten}'", StringComparison.OrdinalIgnoreCase) ||
                thongDiep.Contains($"\"{ten}\"", StringComparison.OrdinalIgnoreCase))
                return thongBao;
        return null;
    }
}
