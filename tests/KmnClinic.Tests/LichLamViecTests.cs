using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>
/// W3: lịch làm việc. Test dùng phòng thử "ZTest Phòng" (khoa K0001), bác sĩ thử (K0001) và ngày cách hôm nay
/// khoảng 200 ngày để không đụng lịch mẫu; mọi lịch thêm vào được xóa khi xong.
/// </summary>
public class LichLamViecTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly LichLamViecBUS _bus = new();
    private readonly string _phong, _phong2, _bs, _bs2, _bsKhoaKhac, _leTan;
    private const string Ca = "CA0001";
    private readonly DateTime _ngay = LichLamViecBUS.DauTuan(DateTime.Today.AddDays(200)); // một Thứ 2

    public LichLamViecTests()
    {
        _db.DangNhap(ChucVu.QuanTriVien);
        var duoi = Random.Shared.Next(100000, 999999);
        _phong = _db.Xoa("Phong_lam_viec", "ma_phong", new PhongBUS().Them($"ZTest Phòng {duoi}", "K0001"));
        _phong2 = _db.Xoa("Phong_lam_viec", "ma_phong", new PhongBUS().Them($"ZTest Phòng B {duoi}", "K0001"));
        _bs = _db.ThemNhanVien("x", ChucVu.BacSi).Ma;
        _bs2 = _db.ThemNhanVien("x", ChucVu.BacSi).Ma;
        _leTan = _db.ThemNhanVien("x", ChucVu.LeTan).Ma;
        _bsKhoaKhac = (string)_db.GiaTri("SELECT TOP 1 ma_nv FROM Nhan_vien WHERE chuc_vu = N'Bác sĩ' AND ma_khoa <> 'K0001'")!;
    }

    public void Dispose() => _db.Dispose();

    private string Them(DateTime ngay, string bs, string phong) => GhiNhan(_bus.Them(ngay, bs, phong, Ca));

    private string GhiNhan(string ma) => _db.Xoa("Lich_lam_viec", "ma_lich_lam_viec", ma);

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    [Fact]
    public void Them_Sua_Xoa_MotNgay()
    {
        var ma = Them(_ngay, _bs, _phong);
        Assert.Matches("^LV[0-9]{4,}$", ma);
        var l = _bus.DanhSach(_ngay, _ngay.AddDays(6)).Single(x => x.MaLichLamViec == ma);
        Assert.Equal(_bs, l.MaNv);
        Assert.Equal("K0001", l.MaKhoa);
        Assert.Equal(0, l.SoLichHen);

        _bus.Sua(ma, _ngay.AddDays(1), _bs2, _phong2, "CA0002");
        l = _bus.DanhSach(_ngay, _ngay.AddDays(6)).Single(x => x.MaLichLamViec == ma);
        Assert.Equal((_ngay.AddDays(1), _bs2, _phong2, "CA0002"), (l.NgayLamViec, l.MaNv, l.MaPhong, l.MaCa));

        _bus.Xoa(ma);
        Assert.DoesNotContain(_bus.DanhSach(_ngay, _ngay.AddDays(6)), x => x.MaLichLamViec == ma);
    }

    [Fact]
    public void ChiXepTuNgayMai()
    {
        Loi("Không được xếp lịch làm việc cho ngày trong quá khứ (RB9).", () => _bus.Them(DateTime.Today.AddDays(-1), _bs, _phong, Ca));
        Loi("Chỉ xếp lịch làm việc từ ngày mai trở đi.", () => _bus.Them(DateTime.Today, _bs, _phong, Ca));
        Loi("Chỉ xếp lịch trong vòng một năm tới.", () => _bus.Them(DateTime.Today.AddYears(1).AddDays(1), _bs, _phong, Ca));
        var ma = Them(_ngay, _bs, _phong);
        Loi("Chỉ xếp lịch làm việc từ ngày mai trở đi.", () => _bus.Sua(ma, DateTime.Today, _bs, _phong, Ca));
    }

    [Fact]
    public void RB10_RB11_NguonLuc()
    {
        Loi("Nhân viên được xếp lịch phải là bác sĩ (RB10).", () => _bus.Them(_ngay, _leTan, _phong, Ca));
        Loi("Phòng phải thuộc khoa của bác sĩ (RB11).", () => _bus.Them(_ngay, _bsKhoaKhac, _phong, Ca));
        Loi("Không tìm thấy bác sĩ đã chọn.", () => _bus.Them(_ngay, "NV9999", _phong, Ca));
        Loi("Không tìm thấy phòng đã chọn.", () => _bus.Them(_ngay, _bs, "P9999", Ca));
        Loi("Không tìm thấy ca đã chọn.", () => _bus.Them(_ngay, _bs, _phong, "CA9999"));
        Loi("Vui lòng chọn bác sĩ.", () => _bus.Them(_ngay, "", _phong, Ca));
    }

    [Fact]
    public void RB12_RB13_TrungLich()
    {
        Them(_ngay, _bs, _phong);
        var ngay = _ngay.ToString("dd/MM/yyyy");
        var tenPhong = (string)_db.GiaTri($"SELECT ten_phong FROM Phong_lam_viec WHERE ma_phong = '{_phong}'")!;
        // RB13: cùng bác sĩ, cùng ca, phòng khác
        Loi($"Trùng lịch trong ca này: ngày {ngay} bác sĩ đã có lịch ở {tenPhong} (RB13).", () => _bus.Them(_ngay, _bs, _phong2, Ca));
        // RB12: cùng phòng, cùng ca, bác sĩ khác
        Loi($"Trùng lịch trong ca này: ngày {ngay} {tenPhong} đã được xếp cho Nhân viên kiểm thử (RB12).", () => _bus.Them(_ngay, _bs2, _phong, Ca));
        // Ca khác thì được
        GhiNhan(_bus.Them(_ngay, _bs, _phong, "CA0002"));
    }

    [Fact]
    public void HangLoat_TheoThu_TatCaHoacKhong()
    {
        // 2 tuần, Thứ 2 + Thứ 4 → 4 ngày
        var ma = _bus.ThemHangLoat(_ngay, _ngay.AddDays(13), [DayOfWeek.Monday, DayOfWeek.Wednesday], _bs, _phong, Ca);
        ma.ForEach(m => GhiNhan(m));
        Assert.Equal(4, ma.Count);
        var ds = _bus.DanhSach(_ngay, _ngay.AddDays(13)).Where(l => l.MaNv == _bs).Select(l => l.NgayLamViec).ToList();
        Assert.Equal([_ngay, _ngay.AddDays(2), _ngay.AddDays(7), _ngay.AddDays(9)], ds);

        // Bác sĩ 2 xếp cả tuần vào cùng phòng: trùng 2 ngày → không ngày nào được lưu
        var loi = Assert.Throws<LoiNghiepVu>(() => _bus.ThemHangLoat(_ngay, _ngay.AddDays(6), Enum.GetValues<DayOfWeek>(), _bs2, _phong, Ca));
        Assert.StartsWith("Trùng lịch trong ca này:", loi.Message);
        Assert.Contains("(RB12)", loi.Message);
        Assert.DoesNotContain(_bus.DanhSach(_ngay, _ngay.AddDays(6)), l => l.MaNv == _bs2);
    }

    [Fact]
    public void HangLoat_KiemTraKhoangNgay()
    {
        Loi("Ngày kết thúc phải sau ngày bắt đầu.", () => _bus.ThemHangLoat(_ngay, _ngay.AddDays(-1), [DayOfWeek.Monday], _bs, _phong, Ca));
        Loi("Chỉ xếp tối đa 62 ngày một lần.", () => _bus.ThemHangLoat(_ngay, _ngay.AddDays(62), [DayOfWeek.Monday], _bs, _phong, Ca));
        Loi("Vui lòng chọn ít nhất một thứ trong tuần.", () => _bus.ThemHangLoat(_ngay, _ngay.AddDays(6), [], _bs, _phong, Ca));
        Loi("Không có ngày nào khớp các thứ đã chọn trong khoảng ngày này.",
            () => _bus.ThemHangLoat(_ngay, _ngay.AddDays(1), [DayOfWeek.Friday], _bs, _phong, Ca));
    }

    [Fact]
    public void HangLoat_QuaNhieuNgayTrung_ChiHienNamNgay()
    {
        _bus.ThemHangLoat(_ngay, _ngay.AddDays(6), Enum.GetValues<DayOfWeek>(), _bs, _phong, Ca).ForEach(m => GhiNhan(m));
        var loi = Assert.Throws<LoiNghiepVu>(() => _bus.ThemHangLoat(_ngay, _ngay.AddDays(6), Enum.GetValues<DayOfWeek>(), _bs, _phong2, Ca));
        Assert.EndsWith("; và 2 ngày khác.", loi.Message);
    }

    [Fact]
    public void CoLichHen_KhongSuaXoaDuoc()
    {
        var ma = Them(_ngay, _bs, _phong);
        var kh = (string)_db.GiaTri("SELECT TOP 1 ma_kh FROM Khach_hang")!;
        var lh = "LHZ" + Random.Shared.Next(1000000, 9999999);
        _db.GiaTri($"INSERT INTO Lich_hen (ma_lich_hen, ngay_gio_dat, trang_thai, ma_kh, ma_lich_lam_viec) VALUES ('{lh}', GETDATE(), N'Đã đặt', '{kh}', '{ma}')");
        _db.Xoa("Lich_hen", "ma_lich_hen", lh);

        Loi("Không thể xóa: lịch này đã có 1 lịch hẹn của khách.", () => _bus.Xoa(ma));
        Loi("Không thể sửa: lịch này đã có 1 lịch hẹn của khách.", () => _bus.Sua(ma, _ngay, _bs2, _phong, Ca));
        Assert.Equal(1, _bus.DanhSach(_ngay, _ngay).Single(l => l.MaLichLamViec == ma).SoLichHenConHieuLuc);
    }

    [Fact]
    public void LichDaQua_KhongXoaDuoc()
    {
        var ma = (string?)_db.GiaTri(@"SELECT TOP 1 l.ma_lich_lam_viec FROM Lich_lam_viec l
            WHERE l.ngay_lam_viec < CAST(GETDATE() AS date)");
        if (ma is null) return; // dữ liệu mẫu không có lịch cũ
        var ngay = ((DateTime)_db.GiaTri($"SELECT ngay_lam_viec FROM Lich_lam_viec WHERE ma_lich_lam_viec = '{ma}'")!).ToString("dd/MM/yyyy");
        Loi($"Lịch ngày {ngay} đã qua, không xóa được để giữ lịch sử.", () => _bus.Xoa(ma));
    }

    [Fact]
    public void BacSi_ChiThayLichCuaMinh_VaKhongXepDuoc()
    {
        Them(_ngay, _bs2, _phong);
        _db.DangNhap(ChucVu.BacSi);
        var toi = PhienDangNhap.NhanVien!.MaNv;
        Assert.All(_bus.DanhSach(_ngay, _ngay.AddDays(6)), l => Assert.Equal(toi, l.MaNv));
        Assert.Empty(_bus.DanhSach(_ngay, _ngay.AddDays(6)));
        Assert.Throws<LoiNghiepVu>(() => _bus.Them(_ngay.AddDays(1), toi, _phong, Ca));
    }

    [Fact]
    public void LeTan_XemTatCa_KhongXepDuoc()
    {
        var ma = Them(_ngay, _bs, _phong);
        _db.DangNhap(ChucVu.LeTan);
        Assert.Contains(_bus.DanhSach(_ngay, _ngay.AddDays(6)), l => l.MaLichLamViec == ma);
        Assert.Throws<LoiNghiepVu>(() => _bus.Xoa(ma));
    }

    [Fact]
    public void DauTuan_LaThuHai()
    {
        Assert.Equal(new DateTime(2026, 11, 16), LichLamViecBUS.DauTuan(new DateTime(2026, 11, 22))); // Chủ nhật
        Assert.Equal(new DateTime(2026, 11, 23), LichLamViecBUS.DauTuan(new DateTime(2026, 11, 23)));
    }
}
