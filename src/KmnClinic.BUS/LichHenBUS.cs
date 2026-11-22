using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>
/// Lịch hẹn. W4: lễ tân đặt lịch hộ khách gọi điện hoặc đến quầy (quyết định C2: khách không dùng app).
/// Lịch lễ tân đặt hộ được xác nhận luôn, ghi tên lễ tân (truy vết).
/// </summary>
public class LichHenBUS
{
    public const int TrieuChungToiDa = 255;
    private readonly LichHenDAL _dal = new();

    /// <summary>Giờ hiện tại; test có thể thay để thử RB16.</summary>
    public Func<DateTime> BayGio { get; set; } = () => DateTime.Now;

    /// <summary>Các ca còn đặt được trong ngày (ca chưa bắt đầu), lọc theo khoa và bác sĩ (null = tất cả).</summary>
    public List<LichLamViecDTO> CaConDat(DateTime ngay, string? maKhoa, string? maNv)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan, ChucVu.QuanTriVien);
        return _dal.CaConDat(ngay, maKhoa, maNv, BayGio());
    }

    /// <summary>
    /// Đặt lịch hộ. Khách đã có: truyền maKh. Khách mới: truyền khachMoi (thêm khách và lịch hẹn trong cùng transaction,
    /// lỗi ở bước nào thì không lưu gì). Trả về (mã lịch hẹn, mã khách).
    /// </summary>
    public (string MaLichHen, string MaKh) DatHo(string? maKh, KhachHangDTO? khachMoi, string? maLichLamViec, string? trieuChung)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        if (string.IsNullOrWhiteSpace(maKh) && khachMoi is null) throw new LoiNghiepVu("Vui lòng tìm khách theo số điện thoại hoặc nhập khách mới.");
        if (string.IsNullOrWhiteSpace(maLichLamViec)) throw new LoiNghiepVu("Vui lòng chọn một ca còn đặt được.");
        var khach = khachMoi is null ? null : KhachHangBUS.ChuanHoa(khachMoi);
        var tc = KiemTra.TuyChon(trieuChung, "triệu chứng ban đầu", TrieuChungToiDa);

        var kq = KiemTra.Ghi(() => _dal.DatHo(maKh?.Trim(), khach, maLichLamViec.Trim(), tc, BayGio(), PhienDangNhap.NhanVien!.MaNv));
        return kq.Loi switch
        {
            LoiDatLich.Khong => (kq.MaLichHen, kq.MaKh),
            LoiDatLich.KhongTimThayLich => throw new LoiNghiepVu("Không tìm thấy lịch làm việc đã chọn. Vui lòng chọn lại."),
            LoiDatLich.CaDaBatDau => throw new LoiNghiepVu("Ca khám này đã bắt đầu hoặc đã qua, không đặt được nữa (RB16). Vui lòng chọn ca khác."),
            LoiDatLich.KhongTimThayKhach => throw new LoiNghiepVu("Không tìm thấy khách hàng đã chọn."),
            LoiDatLich.DaCoLichHen => throw new LoiNghiepVu($"Khách đã có lịch hẹn {kq.MaTrung} với bác sĩ ở ca này, không cần đặt lại."),
            _ => throw new InvalidOperationException(),
        };
    }
}
