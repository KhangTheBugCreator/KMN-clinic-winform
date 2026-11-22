namespace KmnClinic.BUS;

/// <summary>Lỗi do vi phạm quy tắc nghiệp vụ. GUI hiện nguyên câu thông báo cho người dùng.</summary>
public class LoiNghiepVu : Exception
{
    public LoiNghiepVu(string thongBao) : base(thongBao) { }
    public LoiNghiepVu(string thongBao, Exception goc) : base(thongBao, goc) { }
}
