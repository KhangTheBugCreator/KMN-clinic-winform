using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>
/// W7: thu ngân thu tiền. Hóa đơn thử: lễ tân tiếp nhận vãng lai 07:10 hôm nay, bác sĩ kê 4 viên thuốc thử (tồn 10, giá 1.500),
/// tiền công 100.000, hoàn tất → hóa đơn 106.000 "Chưa thanh toán".
/// </summary>
public class ThanhToanTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly ThanhToanBUS _tt = new();
    private readonly string _thuoc, _hoaDon, _sdtThuNgan;
    private readonly DateTime _homNay = DateTime.Today;

    public ThanhToanTests()
    {
        _db.DangNhap(ChucVu.QuanTriVien);
        var duoi = Random.Shared.Next(100000, 999999);
        var phong = _db.Xoa("Phong_lam_viec", "ma_phong", new PhongBUS().Them($"ZTest Phòng {duoi}", "K0001"));
        var (bs, sdtBs) = _db.ThemNhanVien(MatKhau.Bam("matkhau123"), ChucVu.BacSi);
        (_, _sdtThuNgan) = _db.ThemNhanVien(MatKhau.Bam("matkhau123"), ChucVu.ThuNgan);
        var lich = _db.Xoa("Lich_lam_viec", "ma_lich_lam_viec", "LVZ" + Random.Shared.Next(1000000, 9999999));
        _db.GiaTri($@"INSERT INTO Lich_lam_viec (ma_lich_lam_viec, ngay_lam_viec, ma_nv, ma_phong, ma_ca)
                      VALUES ('{lich}', CAST(GETDATE() AS date), '{bs}', '{phong}', 'CA0001')");
        var khach = _db.Xoa("Khach_hang", "ma_kh", new KhachHangBUS().Them(new KhachHangDTO
        {
            HoTenKh = "ZTest Khách thu tiền", SdtKh = "09999" + Random.Shared.Next(0, 100000).ToString("D5"), GioiTinhKh = "Nữ",
            NgaySinhKh = new DateTime(1995, 5, 5), DiaChiKh = "Vĩnh Long",
        }));
        _thuoc = _db.Xoa("Thuoc", "ma_thuoc", new ThuocBUS().Them(new ThuocDTO
            { TenThuoc = $"ZTest Thuốc thu {duoi}", Nsx = "Kiểm thử", HamLuong = 500, DonViTinh = "Viên", DonGiaThuoc = 1500, SoLuongTon = 10 }));

        _db.DangNhap(ChucVu.LeTan);
        var hs = new TiepNhanBUS { BayGio = () => _homNay.AddHours(7).AddMinutes(10) }.TiepNhanVangLai(khach, bs, "Ho");
        _db.Xoa("Ho_so_kham", "ma_ho_so", hs);
        _db.Xoa("Chi_tiet_don_thuoc", "ma_ho_so", hs);
        _db.Xoa("Hoa_don", "ma_ho_so", hs);

        new NhanVienBUS().DangNhap(sdtBs, "matkhau123");
        var kham = new KhamBenhBUS();
        kham.KeThuoc(hs, _thuoc, 4, "Ngày 2 lần");
        _hoaDon = kham.HoanTat(hs, "Ho", "Viêm họng", "Uống nhiều nước", 100000);

        new NhanVienBUS().DangNhap(_sdtThuNgan, "matkhau123");
    }

    public void Dispose() => _db.Dispose();

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);
    private object? Cot(string cot) => _db.GiaTri($"SELECT {cot} FROM Hoa_don WHERE ma_hoa_don = '{_hoaDon}'");
    private int Ton() => (int)_db.GiaTri($"SELECT so_luong_ton FROM Thuoc WHERE ma_thuoc = '{_thuoc}'")!;

    [Fact]
    public void ChoThu_ChiTiet_DuThongTin()
    {
        var h = Assert.Single(_tt.ChoThu("ZTest Khách thu tiền"));
        Assert.Equal((_hoaDon, 106000m, 1, TrangThai.ChuaThanhToan), (h.MaHoaDon, h.TongTien, h.SoThuoc, h.TrangThaiThanhToan));
        var ct = _tt.ChiTiet(_hoaDon);
        Assert.Equal(("Viêm họng", 100000m, 6000m, "Vĩnh Long"), (ct.ChanDoanBenh, ct.TienCongKham, ct.TienThuoc, ct.DiaChi));
        var t = Assert.Single(ct.Thuoc);
        Assert.Equal((4, "Ngày 2 lần", 1500m), (t.SoLuongThuoc, t.LieuDung, t.DonGiaThuoc));
        Loi("Không tìm thấy hóa đơn.", () => _tt.ChiTiet("HD9999999"));
    }

    [Fact]
    public void ThanhToan_GhiNguoiThu_TruKho_KhongThuLai()
    {
        var luc = _homNay.AddHours(9).AddMinutes(30);
        _tt.BayGio = () => luc;
        var tq0 = _tt.TongQuan();
        var hd = _tt.ThanhToan(_hoaDon);
        Assert.True(hd.DaThanhToan);
        Assert.Equal(luc, hd.NgayGioThanhToan);
        Assert.Equal(PhienDangNhap.NhanVien!.MaNv, Cot("ma_nv"));
        Assert.Equal(6, Ton()); // RB33: 10 - 4
        Assert.Equal(106000m, Cot("tong_tien"));

        Loi($"Hóa đơn {_hoaDon} đã được thanh toán lúc 09:30 {_homNay:dd/MM/yyyy} bởi Nhân viên kiểm thử.", () => _tt.ThanhToan(_hoaDon));
        Assert.Equal(6, Ton());
        Assert.Empty(_tt.ChoThu("ZTest Khách thu tiền"));
        Assert.Contains(_tt.LichSu(_homNay, _homNay, true, null), x => x.MaHoaDon == _hoaDon);
        Assert.Empty(_tt.LichSu(_homNay.AddDays(-3), _homNay.AddDays(-1), false, _hoaDon));
        var tq = _tt.TongQuan();
        Assert.Equal((tq0.ChoThu - 1, tq0.DaThuHomNay + 1, tq0.CuaToiHomNay + 1, tq0.DoanhThuHomNay + 106000m),
            (tq.ChoThu, tq.DaThuHomNay, tq.CuaToiHomNay, tq.DoanhThuHomNay));

        // thu ngân khác: không thấy trong "của tôi"
        _db.DangNhap(ChucVu.ThuNgan);
        Assert.DoesNotContain(_tt.LichSu(null, null, true, null), x => x.MaHoaDon == _hoaDon);
        Assert.Contains(_tt.LichSu(null, null, false, _hoaDon), x => x.MaHoaDon == _hoaDon);
        Loi("Từ ngày phải trước hoặc bằng đến ngày.", () => _tt.LichSu(_homNay, _homNay.AddDays(-1), false, null));
    }

    [Fact]
    public void ThanhToan_KhoKhongDu_KhongThu()
    {
        _db.GiaTri($"UPDATE Thuoc SET so_luong_ton = 3 WHERE ma_thuoc = '{_thuoc}'");
        Loi($"Kho không đủ thuốc để thanh toán: {_db.GiaTri($"SELECT ten_thuoc FROM Thuoc WHERE ma_thuoc = '{_thuoc}'")} cần 4, còn 3. " +
            "Vui lòng báo quản trị viên nhập thêm thuốc rồi thanh toán lại.", () => _tt.ThanhToan(_hoaDon));
        Assert.Equal(TrangThai.ChuaThanhToan, Cot("trang_thai_thanh_toan"));
        Assert.Equal(3, Ton());
    }

    [Fact]
    public void ChiThuNganDuocThu()
    {
        Loi("Không tìm thấy hóa đơn.", () => _tt.ThanhToan("HD9999999"));
        _db.DangNhap(ChucVu.LeTan);
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _tt.ThanhToan(_hoaDon));
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _tt.ChoThu(null));
        Assert.Equal(TrangThai.ChuaThanhToan, Cot("trang_thai_thanh_toan"));
    }
}
