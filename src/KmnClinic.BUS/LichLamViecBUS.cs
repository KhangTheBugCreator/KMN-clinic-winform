using System.Globalization;
using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>
/// Lịch làm việc: Quản trị viên xếp, sửa, xóa; Lễ tân, Thu ngân xem toàn bộ; Bác sĩ chỉ xem lịch của mình.
/// Chỉ xếp lịch từ ngày mai trở đi (chặt hơn RB9), trong vòng một năm, tối đa 62 ngày một lần.
/// </summary>
public class LichLamViecBUS
{
    public const int SoNgayToiDa = 62;
    private readonly LichLamViecDAL _dal = new();

    /// <summary>Thứ 2 của tuần chứa ngày đã cho.</summary>
    public static DateTime DauTuan(DateTime ngay) => ngay.Date.AddDays(-(((int)ngay.DayOfWeek + 6) % 7));

    /// <summary>Xem lịch trong khoảng ngày. Bác sĩ luôn chỉ thấy lịch của chính mình.</summary>
    public List<LichLamViecDTO> DanhSach(DateTime tu, DateTime den, string? maKhoa = null)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan, ChucVu.BacSi, ChucVu.ThuNgan);
        if (den.Date < tu.Date) throw new LoiNghiepVu("Ngày kết thúc phải sau ngày bắt đầu.");
        if (SoNgay(tu, den) > SoNgayToiDa) throw new LoiNghiepVu($"Chỉ xem tối đa {SoNgayToiDa} ngày một lần.");
        var maNv = PhienDangNhap.VaiTro == ChucVu.BacSi ? PhienDangNhap.NhanVien!.MaNv : null;
        return _dal.DanhSach(tu, den, maNv, maKhoa);
    }

    public List<NhanVienDTO> DanhSachBacSi(string? maKhoa = null)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.LeTan, ChucVu.BacSi, ChucVu.ThuNgan);
        return _dal.DanhSachBacSi(maKhoa);
    }

    /// <summary>Xếp một ngày. Trả về mã lịch mới.</summary>
    public string Them(DateTime ngay, string? maNv, string? maPhong, string? maCa)
        => ThemCacNgay([ngay.Date], maNv, maPhong, maCa)[0];

    /// <summary>
    /// Xếp hàng loạt: cùng bác sĩ, phòng, ca cho các ngày trong [tu, den] rơi vào các thứ đã chọn
    /// (DayOfWeek: 1 = Thứ 2 ... 6 = Thứ 7, 0 = Chủ nhật). Tất cả hoặc không ngày nào. Trả về các mã mới.
    /// </summary>
    public List<string> ThemHangLoat(DateTime tu, DateTime den, IEnumerable<DayOfWeek> cacThu, string? maNv, string? maPhong, string? maCa)
    {
        if (den.Date < tu.Date) throw new LoiNghiepVu("Ngày kết thúc phải sau ngày bắt đầu.");
        if (SoNgay(tu, den) > SoNgayToiDa) throw new LoiNghiepVu($"Chỉ xếp tối đa {SoNgayToiDa} ngày một lần.");
        var thu = cacThu.ToHashSet();
        if (thu.Count == 0) throw new LoiNghiepVu("Vui lòng chọn ít nhất một thứ trong tuần.");
        var cacNgay = Enumerable.Range(0, SoNgay(tu, den)).Select(i => tu.Date.AddDays(i)).Where(d => thu.Contains(d.DayOfWeek)).ToList();
        if (cacNgay.Count == 0) throw new LoiNghiepVu("Không có ngày nào khớp các thứ đã chọn trong khoảng ngày này.");
        return ThemCacNgay(cacNgay, maNv, maPhong, maCa);
    }

    public void Sua(string maLich, DateTime ngay, string? maNv, string? maPhong, string? maCa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var (nv, phong, ca) = BatBuocChon(maNv, maPhong, maCa);
        KiemTraNgay(ngay);
        BaoLoi(KiemTra.Ghi(() => _dal.Sua(maLich, ngay.Date, nv, phong, ca)), "sửa");
    }

    public void Xoa(string maLich)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        BaoLoi(KiemTra.Ghi(() => _dal.Xoa(maLich)), "xóa");
    }

    private List<string> ThemCacNgay(List<DateTime> cacNgay, string? maNv, string? maPhong, string? maCa)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var (nv, phong, ca) = BatBuocChon(maNv, maPhong, maCa);
        foreach (var d in cacNgay) KiemTraNgay(d);
        var kq = KiemTra.Ghi(() => _dal.Them(cacNgay, nv, phong, ca));
        BaoLoi(kq, "xếp");
        return kq.CacMaMoi;
    }

    /// <summary>
    /// Chủ phòng khám quyết định (12/11/2026, bản web): chỉ xếp lịch từ ngày mai trở đi, chặt hơn RB9.
    /// Trigger RB9 chỉ chặn khi thêm; BUS áp dụng cả khi sửa.
    /// </summary>
    private static void KiemTraNgay(DateTime ngay)
    {
        var d = ngay.Date;
        if (d < DateTime.Today) throw new LoiNghiepVu("Không được xếp lịch làm việc cho ngày trong quá khứ (RB9).");
        if (d == DateTime.Today) throw new LoiNghiepVu("Chỉ xếp lịch làm việc từ ngày mai trở đi.");
        if (d > DateTime.Today.AddYears(1)) throw new LoiNghiepVu("Chỉ xếp lịch trong vòng một năm tới.");
    }

    private static (string, string, string) BatBuocChon(string? maNv, string? maPhong, string? maCa)
    {
        if (string.IsNullOrWhiteSpace(maNv)) throw new LoiNghiepVu("Vui lòng chọn bác sĩ.");
        if (string.IsNullOrWhiteSpace(maPhong)) throw new LoiNghiepVu("Vui lòng chọn phòng.");
        if (string.IsNullOrWhiteSpace(maCa)) throw new LoiNghiepVu("Vui lòng chọn ca.");
        return (maNv.Trim(), maPhong.Trim(), maCa.Trim());
    }

    private static int SoNgay(DateTime tu, DateTime den) => (den.Date - tu.Date).Days + 1;

    private static string Ngay(DateTime d) => d.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

    private static void BaoLoi(KetQuaXepLich kq, string thaoTac)
    {
        switch (kq.Loi)
        {
            case LoiXepLich.Khong: return;
            case LoiXepLich.KhongTimThayBacSi: throw new LoiNghiepVu("Không tìm thấy bác sĩ đã chọn.");
            case LoiXepLich.KhongPhaiBacSi: throw new LoiNghiepVu("Nhân viên được xếp lịch phải là bác sĩ (RB10).");
            case LoiXepLich.KhongTimThayPhong: throw new LoiNghiepVu("Không tìm thấy phòng đã chọn.");
            case LoiXepLich.KhongTimThayCa: throw new LoiNghiepVu("Không tìm thấy ca đã chọn.");
            case LoiXepLich.PhongKhacKhoa: throw new LoiNghiepVu("Phòng phải thuộc khoa của bác sĩ (RB11).");
            case LoiXepLich.KhongTimThayLich: throw new LoiNghiepVu("Không tìm thấy lịch làm việc.");
            case LoiXepLich.DaQua: throw new LoiNghiepVu($"Lịch ngày {Ngay(kq.Ngay!.Value)} đã qua, không {thaoTac} được để giữ lịch sử.");
            case LoiXepLich.DaCoLichHen: throw new LoiNghiepVu($"Không thể {thaoTac}: lịch này đã có {kq.SoLuong} lịch hẹn của khách.");
            case LoiXepLich.DaCoKhachVangLai: throw new LoiNghiepVu($"Không thể {thaoTac}: bác sĩ đã tiếp nhận {kq.SoLuong} khách vãng lai theo lịch này.");
            case LoiXepLich.TrungLich:
                var dong = kq.DanhSachTrung.Select(t => t.TrungBacSi
                    ? $"ngày {Ngay(t.Ngay)} bác sĩ đã có lịch ở {t.TenPhong} (RB13)"
                    : $"ngày {Ngay(t.Ngay)} {t.TenPhong} đã được xếp cho {t.TenBacSi} (RB12)").ToList();
                var hien = dong.Take(5).ToList();
                if (dong.Count > 5) hien.Add($"và {dong.Count - 5} ngày khác");
                throw new LoiNghiepVu("Trùng lịch trong ca này: " + string.Join("; ", hien) + ".");
        }
    }
}
