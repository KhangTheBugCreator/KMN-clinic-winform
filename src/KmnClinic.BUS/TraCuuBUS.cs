using System.Data;
using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

public enum LoaiTraCuu { LichHen, HoSo, HoaDon }

/// <summary>Quản trị viên tra cứu lịch hẹn, hồ sơ khám, hóa đơn (chỉ xem, tối đa 1.000 dòng mới nhất).</summary>
public class TraCuuBUS
{
    private readonly TraCuuDAL _dal = new();
    private readonly KhamBenhDAL _kham = new();

    public static string[] CacTrangThai(LoaiTraCuu loai) => loai switch
    {
        LoaiTraCuu.LichHen => LichHenBUS.CacTrangThai,
        LoaiTraCuu.HoSo => KhamBenhBUS.CacTrangThai,
        _ => ThanhToanBUS.CacTrangThai,
    };

    public DataTable TimKiem(LoaiTraCuu loai, DateTime? tu, DateTime? den, string? trangThai, string? tuKhoa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        if (tu is not null && den is not null && tu.Value.Date > den.Value.Date) throw new LoiNghiepVu("Từ ngày phải trước hoặc bằng đến ngày.");
        if (!string.IsNullOrWhiteSpace(trangThai) && !CacTrangThai(loai).Contains(trangThai)) throw new LoiNghiepVu("Trạng thái không hợp lệ.");
        return loai switch
        {
            LoaiTraCuu.LichHen => _dal.LichHen(tu, den, trangThai, tuKhoa),
            LoaiTraCuu.HoSo => _dal.HoSo(tu, den, trangThai, tuKhoa),
            _ => _dal.HoaDon(tu, den, trangThai, tuKhoa),
        };
    }

    /// <summary>Hồ sơ khám kèm đơn thuốc (của mọi bác sĩ).</summary>
    public (HoSoKhamDTO HoSo, List<ChiTietDonThuocDTO> Thuoc) ChiTietHoSo(string maHoSo)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var hs = _kham.Tim(maHoSo, null) ?? throw new LoiNghiepVu("Không tìm thấy hồ sơ khám.");
        return (hs, _kham.DonThuoc(maHoSo));
    }
}
