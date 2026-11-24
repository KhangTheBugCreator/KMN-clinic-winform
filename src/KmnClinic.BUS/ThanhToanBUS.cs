using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>
/// Thu ngân (W7): xem hóa đơn, xác nhận đã thu (Chưa thanh toán → Đã thanh toán, ghi giờ thu và thu ngân đang đăng nhập,
/// RB31, RB32; trigger trừ kho RB33). Không sửa, xóa, hủy hóa đơn.
/// </summary>
public class ThanhToanBUS
{
    public static readonly string[] CacTrangThai = [TrangThai.ChuaThanhToan, TrangThai.DaThanhToan];
    private readonly ThanhToanDAL _dal = new();

    /// <summary>Giờ hiện tại; test có thể thay.</summary>
    public Func<DateTime> BayGio { get; set; } = () => DateTime.Now;

    private static string MaThuNgan()
    {
        PhanQuyen.YeuCau(ChucVu.ThuNgan);
        return PhienDangNhap.NhanVien!.MaNv;
    }

    public List<HoaDonDTO> ChoThu(string? tuKhoa)
    {
        MaThuNgan();
        return _dal.DanhSach(TrangThai.ChuaThanhToan, null, null, null, tuKhoa);
    }

    /// <summary>Lịch sử đã thu, lọc theo ngày thu (null = mọi ngày); chiCuaToi = chỉ hóa đơn mình thu.</summary>
    public List<HoaDonDTO> LichSu(DateTime? tu, DateTime? den, bool chiCuaToi, string? tuKhoa)
    {
        var tn = MaThuNgan();
        if (tu is not null && den is not null && tu.Value.Date > den.Value.Date) throw new LoiNghiepVu("Từ ngày phải trước hoặc bằng đến ngày.");
        return _dal.DanhSach(TrangThai.DaThanhToan, tu, den, chiCuaToi ? tn : null, tuKhoa);
    }

    public HoaDonDTO ChiTiet(string maHoaDon)
    {
        MaThuNgan();
        return _dal.ChiTiet(maHoaDon) ?? throw new LoiNghiepVu("Không tìm thấy hóa đơn.");
    }

    public TongQuanThuNganDTO TongQuan() => _dal.TongQuan(MaThuNgan(), BayGio());

    /// <summary>Xác nhận đã thu tiền. Trả hóa đơn sau khi thu (để in).</summary>
    public HoaDonDTO ThanhToan(string maHoaDon)
    {
        var tn = MaThuNgan();
        if (string.IsNullOrWhiteSpace(maHoaDon)) throw new LoiNghiepVu("Vui lòng chọn hóa đơn.");
        var kq = KiemTra.Ghi(() => _dal.ThanhToan(maHoaDon.Trim(), tn, BayGio()));
        switch (kq.Loi)
        {
            case LoiThanhToan.KhongTimThay: throw new LoiNghiepVu("Không tìm thấy hóa đơn.");
            case LoiThanhToan.DaThanhToan:
                throw new LoiNghiepVu($"Hóa đơn {maHoaDon.Trim()} đã được thanh toán lúc {kq.Luc:HH:mm dd/MM/yyyy}" +
                                      (kq.NguoiThu is null ? "." : $" bởi {kq.NguoiThu}."));
            case LoiThanhToan.ThieuKho:
                throw new LoiNghiepVu("Kho không đủ thuốc để thanh toán: " +
                    string.Join("; ", kq.ThuocThieu.Select(t => $"{t.TenThuoc} cần {t.SoLuongThuoc}, còn {t.SoLuongTon}")) +
                    ". Vui lòng báo quản trị viên nhập thêm thuốc rồi thanh toán lại.");
        }
        return _dal.ChiTiet(maHoaDon.Trim())!;
    }
}
