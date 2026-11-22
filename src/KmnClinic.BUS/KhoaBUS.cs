using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Danh mục khoa: mọi nhân viên xem, chỉ Quản trị viên thêm/sửa/xóa.</summary>
public class KhoaBUS
{
    private readonly KhoaDAL _dal = new();

    public List<KhoaDTO> DanhSach(string? tuKhoa = null)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan, ChucVu.BacSi, ChucVu.ThuNgan);
        return _dal.DanhSach(tuKhoa);
    }

    /// <summary>Thêm khoa, trả về mã mới.</summary>
    public string Them(string tenKhoa, string? moTa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var ten = KiemTra.BatBuoc(tenKhoa, "tên khoa", 50);
        var mt = KiemTra.TuyChon(moTa, "mô tả", 255);
        return KiemTra.Ghi(() => _dal.Them(ten, mt));
    }

    public void Sua(string maKhoa, string tenKhoa, string? moTa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var ten = KiemTra.BatBuoc(tenKhoa, "tên khoa", 50);
        var mt = KiemTra.TuyChon(moTa, "mô tả", 255);
        if (!KiemTra.Ghi(() => _dal.Sua(maKhoa, ten, mt))) throw new LoiNghiepVu("Không tìm thấy khoa.");
    }

    /// <summary>Chỉ xóa được khoa không còn phòng và bác sĩ nào.</summary>
    public void Xoa(string maKhoa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var kq = KiemTra.Ghi(() => _dal.Xoa(maKhoa));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy khoa.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể xóa: khoa còn {kq.SoLuongDangDung} phòng và {kq.SoLuongDangDung2} bác sĩ. Hãy chuyển hoặc xóa chúng trước.");
        }
    }
}
