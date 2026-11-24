using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>
/// Bác sĩ khám bệnh (W6). Bác sĩ chỉ thấy và sửa hồ sơ của mình (mã bác sĩ lấy từ phiên đăng nhập, không nhận từ GUI).
/// Mỗi thao tác ghi thẳng vào CSDL; khi đã hoàn tất (có hóa đơn) hồ sơ bị khóa (Q4).
/// </summary>
public class KhamBenhBUS
{
    public const int DoDaiToiDa = 255;
    public const decimal TienCongToiDa = 999_999_999_999m; // decimal(12,0)
    public const int SoLuongToiDa = 100_000;
    public static readonly string[] CacTrangThai = [TrangThai.ChoKham, TrangThai.DangKham, TrangThai.HoanTat, TrangThai.DaThanhToan];

    private readonly KhamBenhDAL _dal = new();
    private readonly LichHenDAL _lichHen = new();
    private readonly LichLamViecDAL _lich = new();

    /// <summary>Giờ hiện tại; test có thể thay.</summary>
    public Func<DateTime> BayGio { get; set; } = () => DateTime.Now;

    private static string MaBs()
    {
        PhanQuyen.YeuCau(ChucVu.BacSi);
        return PhienDangNhap.NhanVien!.MaNv;
    }

    public List<HoSoKhamDTO> HangCho() => _dal.HangCho(MaBs());

    public List<HoSoKhamDTO> DanhSach(DateTime? tu, DateTime? den, string? trangThai, string? tuKhoa)
    {
        var bs = MaBs();
        if (tu is not null && den is not null && tu.Value.Date > den.Value.Date) throw new LoiNghiepVu("Từ ngày phải trước hoặc bằng đến ngày.");
        if (!string.IsNullOrWhiteSpace(trangThai) && !CacTrangThai.Contains(trangThai)) throw new LoiNghiepVu("Trạng thái hồ sơ không hợp lệ.");
        return _dal.DanhSach(bs, tu, den, trangThai, tuKhoa);
    }

    public HoSoKhamDTO ChiTiet(string maHoSo)
        => _dal.Tim(maHoSo, MaBs()) ?? throw new LoiNghiepVu("Không tìm thấy hồ sơ khám của bạn.");

    public List<ChiTietDonThuocDTO> DonThuoc(string maHoSo)
    {
        ChiTiet(maHoSo);
        return _dal.DonThuoc(maHoSo);
    }

    public List<LanKhamTruocDTO> LanKhamTruoc(string maHoSo)
        => _dal.LanKhamTruoc(maHoSo, MaBs()) ?? throw new LoiNghiepVu("Không tìm thấy hồ sơ khám của bạn.");

    public TongQuanBacSiDTO TongQuan() => _dal.TongQuan(MaBs(), BayGio());

    /// <summary>Lịch làm việc của bác sĩ từ hôm nay đến 6 ngày sau.</summary>
    public List<LichLamViecDTO> LichLamViecSapToi()
    {
        var bs = MaBs();
        var homNay = BayGio().Date;
        return _lich.DanhSach(homNay, homNay.AddDays(6), maNv: bs);
    }

    /// <summary>Lịch hẹn khách đặt vào ca của bác sĩ (ngay null = mọi ngày).</summary>
    public List<LichHenDTO> LichHenCuaToi(DateTime? ngay, string? trangThai)
    {
        var bs = MaBs();
        if (!string.IsNullOrWhiteSpace(trangThai) && !LichHenBUS.CacTrangThai.Contains(trangThai)) throw new LoiNghiepVu("Trạng thái lịch hẹn không hợp lệ.");
        return _lichHen.DanhSach(ngay, trangThai, null, bs);
    }

    /* ---------- Ghi ---------- */

    private sealed record ThongTinKham(string TrieuChung, string? ChanDoan, string? LoiDan, decimal TienCong);

    private static ThongTinKham KiemTraThongTin(string? trieuChung, string? chanDoan, string? loiDan, decimal tienCong)
    {
        var tc = KiemTra.BatBuoc(trieuChung, "triệu chứng", DoDaiToiDa);
        var cd = KiemTra.TuyChon(chanDoan, "chẩn đoán", DoDaiToiDa);
        var ld = KiemTra.TuyChon(loiDan, "lời dặn", DoDaiToiDa);
        if (tienCong < 0 || tienCong > TienCongToiDa) throw new LoiNghiepVu("Tiền công khám không hợp lệ.");
        if (tienCong != decimal.Truncate(tienCong)) throw new LoiNghiepVu("Tiền công khám tính theo đồng, không có số lẻ.");
        return new(tc, cd, ld, tienCong);
    }

    /// <summary>Lưu nháp: chưa cần chẩn đoán.</summary>
    public void Luu(string maHoSo, string? trieuChung, string? chanDoan, string? loiDan, decimal tienCong)
    {
        var bs = MaBs();
        var t = KiemTraThongTin(trieuChung, chanDoan, loiDan, tienCong);
        BaoLoi(KiemTra.Ghi(() => _dal.Luu(maHoSo, bs, t.TrieuChung, t.ChanDoan, t.LoiDan, t.TienCong)));
    }

    private static (int SoLuong, string LieuDung) KiemTraThuoc(string? maThuoc, int soLuong, string? lieuDung)
    {
        if (string.IsNullOrWhiteSpace(maThuoc)) throw new LoiNghiepVu("Vui lòng chọn thuốc.");
        if (soLuong < 1 || soLuong > SoLuongToiDa) throw new LoiNghiepVu("Số lượng thuốc từ 1 đến 100.000.");
        return (soLuong, KiemTra.BatBuoc(lieuDung, "liều dùng", DoDaiToiDa));
    }

    public void KeThuoc(string maHoSo, string? maThuoc, int soLuong, string? lieuDung)
    {
        var bs = MaBs();
        var (sl, ld) = KiemTraThuoc(maThuoc, soLuong, lieuDung);
        BaoLoi(KiemTra.Ghi(() => _dal.KeThuoc(maHoSo, bs, maThuoc!.Trim(), sl, ld)));
    }

    public void SuaThuoc(string maHoSo, string? maThuoc, int soLuong, string? lieuDung)
    {
        var bs = MaBs();
        var (sl, ld) = KiemTraThuoc(maThuoc, soLuong, lieuDung);
        BaoLoi(KiemTra.Ghi(() => _dal.SuaThuoc(maHoSo, bs, maThuoc!.Trim(), sl, ld)));
    }

    public void XoaThuoc(string maHoSo, string? maThuoc)
    {
        var bs = MaBs();
        if (string.IsNullOrWhiteSpace(maThuoc)) throw new LoiNghiepVu("Vui lòng chọn thuốc.");
        BaoLoi(KiemTra.Ghi(() => _dal.XoaThuoc(maHoSo, bs, maThuoc.Trim())));
    }

    /// <summary>Hoàn tất khám: bắt buộc chẩn đoán (RB22), lập hóa đơn "Chưa thanh toán". Trả mã hóa đơn.</summary>
    public string HoanTat(string maHoSo, string? trieuChung, string? chanDoan, string? loiDan, decimal tienCong)
    {
        var bs = MaBs();
        var t = KiemTraThongTin(trieuChung, chanDoan, loiDan, tienCong);
        var kq = KiemTra.Ghi(() => _dal.HoanTat(maHoSo, bs, t.TrieuChung, t.ChanDoan, t.LoiDan, t.TienCong));
        BaoLoi(kq);
        return kq.MaHoaDon!;
    }

    private static void BaoLoi(KetQuaKhamBenh kq)
    {
        switch (kq.Loi)
        {
            case LoiKhamBenh.Khong: return;
            case LoiKhamBenh.KhongTimThayHoSo: throw new LoiNghiepVu("Không tìm thấy hồ sơ khám của bạn.");
            case LoiKhamBenh.DaHoanTat: throw new LoiNghiepVu("Hồ sơ đã hoàn tất khám và đã lập hóa đơn, không sửa được nữa.");
            case LoiKhamBenh.KhongTimThayThuoc: throw new LoiNghiepVu("Không tìm thấy thuốc đã chọn.");
            case LoiKhamBenh.DaCoTrongDon: throw new LoiNghiepVu($"{kq.TenThuoc} đã có trong đơn. Hãy sửa số lượng ở dòng đó.");
            case LoiKhamBenh.KhongCoTrongDon: throw new LoiNghiepVu("Thuốc này không có trong đơn.");
            case LoiKhamBenh.ThieuTon: throw new LoiNghiepVu($"{kq.TenThuoc} chỉ còn {kq.Ton} trong kho, không kê được {kq.SoLuong} (RB27).");
            case LoiKhamBenh.ChuaChanDoan: throw new LoiNghiepVu("Vui lòng nhập chẩn đoán trước khi hoàn tất khám (RB22).");
            case LoiKhamBenh.TonKhongDu:
                throw new LoiNghiepVu("Tồn kho không đủ, vui lòng sửa đơn: " +
                    string.Join("; ", kq.ThuocThieu.Select(t => $"{t.TenThuoc} kê {t.SoLuongThuoc}, còn {t.SoLuongTon}")) + ".");
            default: throw new LoiNghiepVu("Không lưu được hồ sơ khám.");
        }
    }
}
