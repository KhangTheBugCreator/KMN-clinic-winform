using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>
/// Lịch hẹn. W4: lễ tân đặt lịch hộ khách gọi điện hoặc đến quầy (quyết định C2: khách không dùng app);
/// lịch đặt hộ được xác nhận luôn, ghi tên lễ tân (truy vết). W5: xem, xác nhận lịch khách tự đặt trên web, hủy.
/// </summary>
public class LichHenBUS
{
    public const int TrieuChungToiDa = 255;
    private readonly LichHenDAL _dal = new();

    /// <summary>Giờ hiện tại; test có thể thay để thử RB16.</summary>
    public Func<DateTime> BayGio { get; set; } = () => DateTime.Now;

    public static readonly string[] CacTrangThai = [TrangThai.DaDat, TrangThai.DaXacNhan, TrangThai.DaKham, TrangThai.DaHuy];

    /// <summary>Danh sách lịch hẹn; ngay null = mọi ngày, trangThai null = mọi trạng thái.</summary>
    public List<LichHenDTO> DanhSach(DateTime? ngay, string? trangThai, string? tuKhoa)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        if (!string.IsNullOrWhiteSpace(trangThai) && !CacTrangThai.Contains(trangThai)) throw new LoiNghiepVu("Trạng thái lịch hẹn không hợp lệ.");
        return _dal.DanhSach(ngay, trangThai, tuKhoa);
    }

    /// <summary>Xác nhận lịch "Đã đặt" (khách tự đặt trên web), ghi lễ tân đang đăng nhập.</summary>
    public void XacNhan(string maLichHen)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        var kq = KiemTra.Ghi(() => _dal.XacNhan(maLichHen, PhienDangNhap.NhanVien!.MaNv, BayGio()));
        BaoLoi(kq, "xác nhận được lịch Đã đặt", "Ca khám của lịch hẹn này đã kết thúc, không xác nhận được nữa.");
    }

    public void Huy(string maLichHen)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        var kq = KiemTra.Ghi(() => _dal.Huy(maLichHen));
        if (kq.Loi == LoiLichHen.SaiTrangThai)
            throw new LoiNghiepVu(kq.TrangThai == TrangThai.DaHuy ? "Lịch hẹn này đã được hủy trước đó." : "Lịch hẹn đã được tiếp nhận khám, không hủy được.");
        BaoLoi(kq, "", "");
    }

    internal static void BaoLoi(KetQuaLichHen kq, string chiDuoc, string caKetThuc)
    {
        switch (kq.Loi)
        {
            case LoiLichHen.Khong: return;
            case LoiLichHen.KhongTimThay: throw new LoiNghiepVu("Không tìm thấy lịch hẹn.");
            case LoiLichHen.SaiTrangThai: throw new LoiNghiepVu($"Lịch hẹn đang ở trạng thái {kq.TrangThai}, chỉ {chiDuoc}.");
            case LoiLichHen.CaDaKetThuc: throw new LoiNghiepVu(caKetThuc);
            case LoiLichHen.KhacNgay:
                throw new LoiNghiepVu($"Chỉ tiếp nhận đúng ngày hẹn ({kq.Ngay:dd/MM/yyyy}). Khách đến khác ngày thì tiếp nhận như khách vãng lai.");
        }
    }

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
