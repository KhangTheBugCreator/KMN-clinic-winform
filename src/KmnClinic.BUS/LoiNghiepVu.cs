namespace KmnClinic.BUS;

/// <summary>Lỗi do vi phạm quy tắc nghiệp vụ. GUI hiện nguyên câu thông báo cho người dùng.</summary>
public class LoiNghiepVu(string thongBao) : Exception(thongBao);
