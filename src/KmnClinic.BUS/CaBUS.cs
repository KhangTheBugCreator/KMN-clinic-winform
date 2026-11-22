using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Danh mục ca làm việc: mọi nhân viên xem, chỉ Quản trị viên thêm/sửa/xóa.</summary>
public class CaBUS
{
    private readonly CaDAL _dal = new();

    public List<CaDTO> DanhSach()
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan, ChucVu.BacSi, ChucVu.ThuNgan);
        return _dal.DanhSach();
    }

    public string Them(string tenCa, TimeSpan gioBatDau, TimeSpan gioKetThuc)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var ten = KiemTra.BatBuoc(tenCa, "tên ca", 20);
        var (bd, kt) = KiemTraGio(gioBatDau, gioKetThuc);
        return KiemTra.Ghi(() => _dal.Them(ten, bd, kt));
    }

    /// <summary>Ca đã được xếp lịch thì chỉ được đổi tên; muốn đổi giờ thì tạo ca mới.</summary>
    public void Sua(string maCa, string tenCa, TimeSpan gioBatDau, TimeSpan gioKetThuc)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var ten = KiemTra.BatBuoc(tenCa, "tên ca", 20);
        var (bd, kt) = KiemTraGio(gioBatDau, gioKetThuc);
        var kq = KiemTra.Ghi(() => _dal.Sua(maCa, ten, bd, kt));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy ca.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể đổi giờ: ca đã được dùng trong {kq.SoLuongDangDung} lịch làm việc. Chỉ được đổi tên, hoặc tạo ca mới với giờ khác.");
        }
    }

    public void Xoa(string maCa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var kq = KiemTra.Ghi(() => _dal.Xoa(maCa));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy ca.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể xóa: ca đã được dùng trong {kq.SoLuongDangDung} lịch làm việc.");
        }
    }

    /// <summary>RB3: giờ bắt đầu nhỏ hơn giờ kết thúc (CSDL cũng chặn bằng CK_Ca_lam_viec_gio). Bỏ phần giây.</summary>
    private static (TimeSpan, TimeSpan) KiemTraGio(TimeSpan bd, TimeSpan kt)
    {
        bd = new TimeSpan(bd.Hours, bd.Minutes, 0);
        kt = new TimeSpan(kt.Hours, kt.Minutes, 0);
        if (bd >= kt) throw new LoiNghiepVu("Giờ bắt đầu ca phải nhỏ hơn giờ kết thúc.");
        return (bd, kt);
    }
}
