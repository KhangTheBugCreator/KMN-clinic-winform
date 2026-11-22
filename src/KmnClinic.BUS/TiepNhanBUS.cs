using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>
/// Lễ tân tiếp nhận khách: có hẹn đúng ngày (Q10; "Đã đặt" thì xác nhận và tiếp nhận một lần, Q3) và khách vãng lai
/// với bác sĩ đang trực (RB41). Tiếp nhận tạo hồ sơ khám, ghi lễ tân đang đăng nhập (RB34).
/// </summary>
public class TiepNhanBUS
{
    public const int TrieuChungToiDa = 255;
    private readonly TiepNhanDAL _dal = new();

    /// <summary>Giờ hiện tại; test có thể thay để thử theo giờ.</summary>
    public Func<DateTime> BayGio { get; set; } = () => DateTime.Now;

    public string TiepNhanCoHen(string? maLichHen, string? trieuChung)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        if (string.IsNullOrWhiteSpace(maLichHen)) throw new LoiNghiepVu("Vui lòng chọn lịch hẹn.");
        var tc = KiemTra.BatBuoc(trieuChung, "triệu chứng", TrieuChungToiDa);
        var kq = KiemTra.Ghi(() => _dal.TiepNhanCoHen(maLichHen.Trim(), PhienDangNhap.NhanVien!.MaNv, tc, BayGio()));
        LichHenBUS.BaoLoi(kq, "tiếp nhận được lịch Đã đặt hoặc Đã xác nhận", "");
        return kq.MaHoSo!;
    }

    public string TiepNhanVangLai(string? maKh, string? maBacSi, string? trieuChung)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        if (string.IsNullOrWhiteSpace(maKh)) throw new LoiNghiepVu("Vui lòng tìm khách theo số điện thoại hoặc thêm khách mới.");
        if (string.IsNullOrWhiteSpace(maBacSi)) throw new LoiNghiepVu("Vui lòng chọn bác sĩ đang trực.");
        var tc = KiemTra.BatBuoc(trieuChung, "triệu chứng", TrieuChungToiDa);
        var kq = KiemTra.Ghi(() => _dal.TiepNhanVangLai(maKh.Trim(), maBacSi.Trim(), PhienDangNhap.NhanVien!.MaNv, tc, BayGio()));
        if (kq.KhongTimThayKhach) throw new LoiNghiepVu("Không tìm thấy khách hàng đã chọn.");
        if (kq.BacSiKhongTruc)
            throw new LoiNghiepVu("Bác sĩ được chọn không trực ca này (RB41). Chỉ nhận khách vãng lai từ 30 phút trước giờ bắt đầu ca đến hết ca.");
        return kq.MaHoSo!;
    }

    public List<BacSiDangTrucDTO> BacSiDangTruc()
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        return _dal.BacSiDangTruc(BayGio());
    }

    public List<HoSoKhamDTO> HoSoTheoNgay(DateTime ngay)
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        return _dal.HoSoTheoNgay(ngay);
    }

    public TongQuanLeTanDTO TongQuan()
    {
        PhanQuyen.YeuCau(ChucVu.LeTan);
        return _dal.TongQuan(BayGio());
    }
}
