using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>
/// W5: lễ tân xác nhận, hủy, tiếp nhận có hẹn và vãng lai. Dùng phòng, bác sĩ, khách thử; lịch làm việc hôm nay ca sáng
/// (07:00–11:30) chèn thẳng bằng SQL vì BUS chỉ xếp từ ngày mai. Giờ hiện tại được giả lập qua BayGio.
/// </summary>
public class LeTanTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly LichHenBUS _lichHen = new();
    private readonly TiepNhanBUS _tiepNhan = new();
    private readonly string _bs, _lichHomNay, _lichSau, _khach;
    private readonly DateTime _homNay = DateTime.Today;

    public LeTanTests()
    {
        _db.DangNhap(ChucVu.QuanTriVien);
        var duoi = Random.Shared.Next(100000, 999999);
        var phong = _db.Xoa("Phong_lam_viec", "ma_phong", new PhongBUS().Them($"ZTest Phòng {duoi}", "K0001"));
        _bs = _db.ThemNhanVien("x", ChucVu.BacSi).Ma;
        _lichHomNay = _db.Xoa("Lich_lam_viec", "ma_lich_lam_viec", "LVZ" + Random.Shared.Next(1000000, 9999999));
        _db.GiaTri($@"INSERT INTO Lich_lam_viec (ma_lich_lam_viec, ngay_lam_viec, ma_nv, ma_phong, ma_ca)
                      VALUES ('{_lichHomNay}', CAST(GETDATE() AS date), '{_bs}', '{phong}', 'CA0001')");
        _lichSau = _db.Xoa("Lich_lam_viec", "ma_lich_lam_viec", new LichLamViecBUS().Them(_homNay.AddDays(200), _bs, phong, "CA0001"));
        _khach = _db.Xoa("Khach_hang", "ma_kh", new KhachHangBUS().Them(new KhachHangDTO
        {
            HoTenKh = "ZTest Khách lễ tân", SdtKh = "09999" + Random.Shared.Next(0, 100000).ToString("D5"), GioiTinhKh = "Nữ",
            NgaySinhKh = new DateTime(1990, 1, 1),
        }));
        _db.DangNhap(ChucVu.LeTan);
    }

    public void Dispose() => _db.Dispose();

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    /// <summary>Lịch hẹn hôm nay "Đã đặt" như khách tự đặt trên web (đặt lúc 06:00, trước giờ ca).</summary>
    private string HenHomNayDaDat()
    {
        var ma = _db.Xoa("Lich_hen", "ma_lich_hen", "LHZ" + Random.Shared.Next(1000000, 9999999));
        _db.GiaTri($@"INSERT INTO Lich_hen (ma_lich_hen, ngay_gio_dat, trieu_chung_ban_dau, trang_thai, ma_kh, ma_lich_lam_viec)
                      VALUES ('{ma}', DATEADD(HOUR, 6, CAST(CAST(GETDATE() AS date) AS datetime)), N'Ho', N'Đã đặt', '{_khach}', '{_lichHomNay}')");
        return ma;
    }

    private string HoSoCuaHen(string maHen)
        => _db.Xoa("Ho_so_kham", "ma_ho_so", (string)_db.GiaTri($"SELECT ma_ho_so FROM Ho_so_kham WHERE ma_lich_hen = '{maHen}'")!);

    private object? Cot(string cot, string maHen) => _db.GiaTri($"SELECT {cot} FROM Lich_hen WHERE ma_lich_hen = '{maHen}'");

    [Fact]
    public void XacNhan_GhiLeTan_VaChiTuDaDat()
    {
        var ma = HenHomNayDaDat();
        _lichHen.BayGio = () => _homNay.AddHours(6.5);
        _lichHen.XacNhan(ma);
        Assert.Equal(TrangThai.DaXacNhan, Cot("trang_thai", ma));
        Assert.Equal(PhienDangNhap.NhanVien!.MaNv, Cot("ma_nv", ma));
        Loi("Lịch hẹn đang ở trạng thái Đã xác nhận, chỉ xác nhận được lịch Đã đặt.", () => _lichHen.XacNhan(ma));
        var l = _lichHen.DanhSach(_homNay, null, _khach).Single(x => x.MaLichHen == ma);
        Assert.Equal(PhienDangNhap.NhanVien!.HoTenNv, l.LeTanXacNhan);
    }

    [Fact]
    public void XacNhan_CaDaKetThuc()
    {
        var ma = HenHomNayDaDat();
        _lichHen.BayGio = () => _homNay.AddHours(12);
        Loi("Ca khám của lịch hẹn này đã kết thúc, không xác nhận được nữa.", () => _lichHen.XacNhan(ma));
    }

    [Fact]
    public void Huy_TuDaDatHoacDaXacNhan()
    {
        var ma = HenHomNayDaDat();
        _lichHen.Huy(ma);
        Assert.Equal(TrangThai.DaHuy, Cot("trang_thai", ma));
        Loi("Lịch hẹn này đã được hủy trước đó.", () => _lichHen.Huy(ma));
        Loi("Không tìm thấy lịch hẹn.", () => _lichHen.Huy("LH9999999"));
    }

    [Fact]
    public void TiepNhanCoHen_DaDat_XacNhanVaTaoHoSo()
    {
        var ma = HenHomNayDaDat();
        _tiepNhan.BayGio = () => _homNay.AddHours(7).AddMinutes(10);
        var hs = _db.Xoa("Ho_so_kham", "ma_ho_so", _tiepNhan.TiepNhanCoHen(ma, "  Ho, sốt nhẹ "));
        Assert.Equal(TrangThai.DaKham, Cot("trang_thai", ma)); // trigger tự chuyển
        Assert.Equal(PhienDangNhap.NhanVien!.MaNv, Cot("ma_nv", ma)); // Q3: xác nhận cùng lúc
        Assert.Equal("Ho, sốt nhẹ", _db.GiaTri($"SELECT trieu_chung FROM Ho_so_kham WHERE ma_ho_so = '{hs}'"));
        Assert.Equal(PhienDangNhap.NhanVien!.MaNv, _db.GiaTri($"SELECT ma_nv_tiep_nhan FROM Ho_so_kham WHERE ma_ho_so = '{hs}'"));
        var h = _tiepNhan.HoSoTheoNgay(_homNay).Single(x => x.MaHoSo == hs);
        Assert.Equal(TrangThai.ChoKham, h.TrangThai);
        Assert.Equal("Có hẹn", h.Nguon);
        Loi("Lịch hẹn đã được tiếp nhận khám, không hủy được.", () => _lichHen.Huy(ma));
        Loi("Lịch hẹn đang ở trạng thái Đã khám, chỉ tiếp nhận được lịch Đã đặt hoặc Đã xác nhận.", () => _tiepNhan.TiepNhanCoHen(ma, "x"));
    }

    [Fact]
    public void TiepNhanCoHen_KhacNgay_VaThieuTrieuChung()
    {
        _db.DangNhap(ChucVu.LeTan);
        var (ma, _) = _lichHen.DatHo(_khach, null, _lichSau, null);
        _db.Xoa("Lich_hen", "ma_lich_hen", ma);
        Loi($"Chỉ tiếp nhận đúng ngày hẹn ({_homNay.AddDays(200):dd/MM/yyyy}). Khách đến khác ngày thì tiếp nhận như khách vãng lai.",
            () => _tiepNhan.TiepNhanCoHen(ma, "Ho"));
        Loi("Vui lòng nhập triệu chứng.", () => _tiepNhan.TiepNhanCoHen(ma, "  "));
    }

    [Fact]
    public void VangLai_RB41_Tu30PhutTruocCa()
    {
        _tiepNhan.BayGio = () => _homNay.AddHours(6).AddMinutes(20);
        Assert.DoesNotContain(_tiepNhan.BacSiDangTruc(), b => b.MaNv == _bs);
        Loi("Bác sĩ được chọn không trực ca này (RB41). Chỉ nhận khách vãng lai từ 30 phút trước giờ bắt đầu ca đến hết ca.",
            () => _tiepNhan.TiepNhanVangLai(_khach, _bs, "Đau bụng"));

        _tiepNhan.BayGio = () => _homNay.AddHours(6).AddMinutes(30);
        var hs = _db.Xoa("Ho_so_kham", "ma_ho_so", _tiepNhan.TiepNhanVangLai(_khach, _bs, "Đau bụng"));
        Assert.Equal(DBNull.Value, _db.GiaTri($"SELECT ma_lich_hen FROM Ho_so_kham WHERE ma_ho_so = '{hs}'"));
        Assert.Equal(1, _tiepNhan.BacSiDangTruc().Single(b => b.MaNv == _bs).SoDangCho);
        Assert.Equal("Vãng lai", _tiepNhan.HoSoTheoNgay(_homNay).Single(x => x.MaHoSo == hs).Nguon);
    }

    [Fact]
    public void VangLai_KiemTraDauVao()
    {
        _tiepNhan.BayGio = () => _homNay.AddHours(8);
        Loi("Vui lòng tìm khách theo số điện thoại hoặc thêm khách mới.", () => _tiepNhan.TiepNhanVangLai(null, _bs, "x"));
        Loi("Vui lòng chọn bác sĩ đang trực.", () => _tiepNhan.TiepNhanVangLai(_khach, null, "x"));
        Loi("Vui lòng nhập triệu chứng.", () => _tiepNhan.TiepNhanVangLai(_khach, _bs, ""));
        Loi("Không tìm thấy khách hàng đã chọn.", () => _tiepNhan.TiepNhanVangLai("KH99999", _bs, "x"));
    }

    [Fact]
    public void TongQuan_DemTheoNgay()
    {
        _tiepNhan.BayGio = () => _homNay.AddHours(6);
        var truoc = _tiepNhan.TongQuan();
        HenHomNayDaDat();
        var sau = _tiepNhan.TongQuan();
        Assert.Equal(truoc.ChoXacNhan + 1, sau.ChoXacNhan);
        Assert.Equal(truoc.HenHomNay + 1, sau.HenHomNay);
    }

    [Fact]
    public void ChiLeTan()
    {
        var ma = HenHomNayDaDat();
        _db.DangNhap(ChucVu.BacSi);
        Assert.Throws<LoiNghiepVu>(() => _lichHen.XacNhan(ma));
        Assert.Throws<LoiNghiepVu>(() => _tiepNhan.TiepNhanCoHen(ma, "x"));
        _db.DangNhap(ChucVu.ThuNgan);
        Assert.Throws<LoiNghiepVu>(() => _lichHen.DanhSach(null, null, null));
        Assert.Throws<LoiNghiepVu>(() => _tiepNhan.TongQuan());
    }
}
