namespace KmnClinic.DAL;

/// <summary>
/// Kết quả của lệnh sửa/xóa có điều kiện. DAL kiểm tra điều kiện và ghi trong cùng một transaction
/// (khóa UPDLOCK + HOLDLOCK) rồi trả về kết quả; BUS đổi kết quả thành câu thông báo.
/// </summary>
public enum TrangThaiGhi
{
    ThanhCong,
    KhongTimThay,
    /// <summary>Dữ liệu đang được dùng ở bảng khác nên không làm được (SoLuongDangDung cho biết bao nhiêu).</summary>
    DangDuocDung,
    /// <summary>Quản trị viên tự xóa mình hoặc tự bỏ chức vụ quản trị của mình.</summary>
    TuThaoTacChinhMinh,
    /// <summary>Thao tác làm phòng khám không còn quản trị viên nào.</summary>
    HetQuanTriVien,
}

public readonly record struct KetQuaGhi(TrangThaiGhi TrangThai, int SoLuongDangDung = 0, int SoLuongDangDung2 = 0)
{
    public static readonly KetQuaGhi ThanhCong = new(TrangThaiGhi.ThanhCong);
    public static readonly KetQuaGhi KhongTimThay = new(TrangThaiGhi.KhongTimThay);
}
