using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Khách hàng: Quản trị viên và Lễ tân xem, thêm, sửa; chỉ Quản trị viên xóa.</summary>
public class KhachHangBUS
{
    public const string GioiTinhKhac = "Khác";

    /// <summary>Lựa chọn giới tính trên giao diện. "Khác" được lưu NULL trong CSDL.</summary>
    public static readonly string[] GioiTinh = ["Nam", "Nữ", GioiTinhKhac];

    private readonly KhachHangDAL _dal = new();

    public List<KhachHangDTO> DanhSach(string? tuKhoa = null, string? gioiTinh = null)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan);
        return _dal.DanhSach(tuKhoa, gioiTinh);
    }

    /// <summary>Tìm khách theo đúng số điện thoại (khách gọi điện đặt lịch). Không có thì trả về null.</summary>
    public KhachHangDTO? TimTheoSdt(string sdt)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan);
        return _dal.TimTheoSdt(KiemTra.SoDienThoai(sdt));
    }

    public string Them(KhachHangDTO kh)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan);
        var hopLe = ChuanHoa(kh);
        return KiemTra.Ghi(() => _dal.Them(hopLe));
    }

    public void Sua(KhachHangDTO kh)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan);
        var hopLe = ChuanHoa(kh);
        hopLe.MaKh = kh.MaKh;
        if (!KiemTra.Ghi(() => _dal.Sua(hopLe))) throw new LoiNghiepVu("Không tìm thấy khách hàng.");
    }

    /// <summary>Chỉ xóa được khách chưa có lịch hẹn và hồ sơ khám (giữ lịch sử khám).</summary>
    public void Xoa(string maKh)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var kq = KiemTra.Ghi(() => _dal.Xoa(maKh));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy khách hàng.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể xóa: khách hàng đã có {kq.SoLuongDangDung} lịch hẹn và {kq.SoLuongDangDung2} hồ sơ khám cần giữ lại.");
        }
    }

    private static KhachHangDTO ChuanHoa(KhachHangDTO kh)
    {
        // Nam / Nữ lưu nguyên; "Khác" hoặc bỏ trống lưu NULL
        var gt = (kh.GioiTinhKh ?? "").Trim();
        if (gt.Length > 0 && !GioiTinh.Contains(gt)) throw new LoiNghiepVu("Giới tính chỉ nhận Nam, Nữ hoặc Khác.");
        string? gioiTinhLuu = gt is "Nam" or "Nữ" ? gt : null;
        if (kh.NgaySinhKh is null) throw new LoiNghiepVu("Vui lòng nhập ngày sinh.");
        return new KhachHangDTO
        {
            HoTenKh = KiemTra.BatBuoc(kh.HoTenKh, "họ tên", 50),
            NgaySinhKh = KiemTra.NgaySinh(kh.NgaySinhKh),
            GioiTinhKh = gioiTinhLuu,
            SdtKh = KiemTra.SoDienThoai(kh.SdtKh),
            DiaChiKh = KiemTra.TuyChon(kh.DiaChiKh, "địa chỉ", 100),
        };
    }
}
