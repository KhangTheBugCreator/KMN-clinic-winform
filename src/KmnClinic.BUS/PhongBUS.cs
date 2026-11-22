using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Danh mục phòng làm việc: mọi nhân viên xem, chỉ Quản trị viên thêm/sửa/xóa. Mỗi phòng thuộc một khoa (RB11).</summary>
public class PhongBUS
{
    private readonly PhongDAL _dal = new();

    public List<PhongDTO> DanhSach(string? tuKhoa = null, string? maKhoa = null)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan, ChucVu.BacSi, ChucVu.ThuNgan);
        return _dal.DanhSach(tuKhoa, maKhoa);
    }

    public string Them(string tenPhong, string? maKhoa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var ten = KiemTra.BatBuoc(tenPhong, "tên phòng", 50);
        var khoa = ChonKhoa(maKhoa);
        return KiemTra.Ghi(() => _dal.Them(ten, khoa));
    }

    /// <summary>Phòng đã có lịch làm việc thì chỉ được đổi tên, không đổi khoa (RB11).</summary>
    public void Sua(string maPhong, string tenPhong, string? maKhoa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var ten = KiemTra.BatBuoc(tenPhong, "tên phòng", 50);
        var khoa = ChonKhoa(maKhoa);
        var kq = KiemTra.Ghi(() => _dal.Sua(maPhong, ten, khoa));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy phòng.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể đổi khoa: phòng đã có {kq.SoLuongDangDung} lịch làm việc theo khoa hiện tại (RB11).");
        }
    }

    /// <summary>Phòng đã từng được xếp lịch thì không xóa, để giữ lịch sử sử dụng phòng.</summary>
    public void Xoa(string maPhong)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var kq = KiemTra.Ghi(() => _dal.Xoa(maPhong));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy phòng.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể xóa: phòng đã có {kq.SoLuongDangDung} lịch làm việc. Lịch sử sử dụng phòng cần được giữ lại.");
        }
    }

    private static string ChonKhoa(string? maKhoa)
        => string.IsNullOrWhiteSpace(maKhoa) ? throw new LoiNghiepVu("Vui lòng chọn khoa.") : maKhoa.Trim();
}
