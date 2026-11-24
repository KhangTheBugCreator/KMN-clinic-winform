using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>
/// W6: bác sĩ khám, kê đơn, hoàn tất. Hồ sơ thử được lễ tân tiếp nhận vãng lai lúc 07:10 hôm nay (lịch làm việc ca sáng
/// chèn thẳng bằng SQL). Thuốc thử tồn 10, giá 1.500.
/// </summary>
public class KhamBenhTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly KhamBenhBUS _kham = new();
    private readonly string _bs, _sdtBs, _khach, _thuoc, _thuoc2, _hoSo;
    private readonly DateTime _homNay = DateTime.Today;

    public KhamBenhTests()
    {
        _db.DangNhap(ChucVu.QuanTriVien);
        var duoi = Random.Shared.Next(100000, 999999);
        var phong = _db.Xoa("Phong_lam_viec", "ma_phong", new PhongBUS().Them($"ZTest Phòng {duoi}", "K0001"));
        (_bs, _sdtBs) = _db.ThemNhanVien(MatKhau.Bam("matkhau123"), ChucVu.BacSi);
        var lich = _db.Xoa("Lich_lam_viec", "ma_lich_lam_viec", "LVZ" + Random.Shared.Next(1000000, 9999999));
        _db.GiaTri($@"INSERT INTO Lich_lam_viec (ma_lich_lam_viec, ngay_lam_viec, ma_nv, ma_phong, ma_ca)
                      VALUES ('{lich}', CAST(GETDATE() AS date), '{_bs}', '{phong}', 'CA0001')");
        _khach = _db.Xoa("Khach_hang", "ma_kh", new KhachHangBUS().Them(new KhachHangDTO
        {
            HoTenKh = "ZTest Khách khám", SdtKh = "09999" + Random.Shared.Next(0, 100000).ToString("D5"), GioiTinhKh = "Nam",
            NgaySinhKh = new DateTime(1980, 1, 1),
        }));
        var thuocBus = new ThuocBUS();
        _thuoc = _db.Xoa("Thuoc", "ma_thuoc", thuocBus.Them(new ThuocDTO
            { TenThuoc = $"ZTest Thuốc A {duoi}", Nsx = "Kiểm thử", HamLuong = 500, DonViTinh = "Viên", DonGiaThuoc = 1500, SoLuongTon = 10 }));
        _thuoc2 = _db.Xoa("Thuoc", "ma_thuoc", thuocBus.Them(new ThuocDTO
            { TenThuoc = $"ZTest Thuốc B {duoi}", Nsx = "Kiểm thử", HamLuong = 10, DonViTinh = "Gói", DonGiaThuoc = 3000, SoLuongTon = 5 }));
        _db.DangNhap(ChucVu.LeTan);
        _hoSo = TiepNhan();
        new NhanVienBUS().DangNhap(_sdtBs, "matkhau123");
    }

    private string TiepNhan(int phut = 10)
    {
        var tn = new TiepNhanBUS { BayGio = () => _homNay.AddHours(7).AddMinutes(phut) };
        var ma = tn.TiepNhanVangLai(_khach, _bs, "Đau đầu");
        // xóa trước hồ sơ: chi tiết đơn, hóa đơn
        _db.Xoa("Ho_so_kham", "ma_ho_so", ma);
        _db.Xoa("Chi_tiet_don_thuoc", "ma_ho_so", ma);
        _db.Xoa("Hoa_don", "ma_ho_so", ma);
        return ma;
    }

    public void Dispose() => _db.Dispose();

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    [Fact]
    public void HangCho_ChoKham_RoiDangKhamLenTruoc()
    {
        var hs2 = TiepNhanBangLeTan(20);
        var ds = _kham.HangCho();
        Assert.Equal([_hoSo, hs2], ds.Select(h => h.MaHoSo).ToArray());
        Assert.All(ds, h => Assert.Equal(TrangThai.ChoKham, h.TrangThai));
        Assert.Equal("Nam", ds[0].GioiTinh);

        _kham.KeThuoc(hs2, _thuoc, 2, "Ngày 2 lần");
        ds = _kham.HangCho();
        Assert.Equal([hs2, _hoSo], ds.Select(h => h.MaHoSo).ToArray());
        Assert.Equal(TrangThai.DangKham, ds[0].TrangThai);
        var tq = _kham.TongQuan();
        Assert.Equal((1, 1, 0), (tq.ChoKham, tq.DangKham, tq.HoanTatHomNay));
    }

    private string TiepNhanBangLeTan(int phut)
    {
        _db.DangNhap(ChucVu.LeTan);
        var ma = TiepNhan(phut);
        new NhanVienBUS().DangNhap(_sdtBs, "matkhau123");
        return ma;
    }

    [Fact]
    public void BacSiKhac_KhongThayKhongSua()
    {
        _db.DangNhap(ChucVu.BacSi);
        Assert.DoesNotContain(_kham.HangCho(), h => h.MaHoSo == _hoSo);
        Loi("Không tìm thấy hồ sơ khám của bạn.", () => _kham.ChiTiet(_hoSo));
        Loi("Không tìm thấy hồ sơ khám của bạn.", () => _kham.Luu(_hoSo, "Ho", "Cảm", null, 0));
        Loi("Không tìm thấy hồ sơ khám của bạn.", () => _kham.KeThuoc(_hoSo, _thuoc, 1, "x"));
        _db.DangNhap(ChucVu.LeTan);
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _kham.HangCho());
    }

    [Fact]
    public void LuuNhap_KiemTraDuLieu()
    {
        _kham.Luu(_hoSo, " Đau đầu, chóng mặt ", "  ", " Nghỉ ngơi ", 150000);
        var h = _kham.ChiTiet(_hoSo);
        Assert.Equal("Đau đầu, chóng mặt", h.TrieuChung);
        Assert.Null(h.ChanDoanBenh);
        Assert.Equal("Nghỉ ngơi", h.LoiDan);
        Assert.Equal(150000m, h.TienCongKham);
        Assert.Equal(TrangThai.ChoKham, h.TrangThai);
        Loi("Vui lòng nhập triệu chứng.", () => _kham.Luu(_hoSo, " ", null, null, 0));
        Loi("Chẩn đoán tối đa 255 ký tự.", () => _kham.Luu(_hoSo, "Ho", new string('a', 256), null, 0));
        Loi("Tiền công khám không hợp lệ.", () => _kham.Luu(_hoSo, "Ho", null, null, -1));
        Loi("Tiền công khám tính theo đồng, không có số lẻ.", () => _kham.Luu(_hoSo, "Ho", null, null, 1000.5m));
        _kham.Luu(_hoSo, "Ho", "Viêm họng", null, 0);
        Assert.Equal(TrangThai.DangKham, _kham.ChiTiet(_hoSo).TrangThai);
    }

    [Fact]
    public void KeThuoc_RB27_RB28_TrungVaSuaXoa()
    {
        Loi($"ZTest Thuốc A {TenDuoi()} chỉ còn 10 trong kho, không kê được 11 (RB27).", () => _kham.KeThuoc(_hoSo, _thuoc, 11, "x"));
        Loi("Vui lòng nhập liều dùng.", () => _kham.KeThuoc(_hoSo, _thuoc, 1, " "));
        Loi("Số lượng thuốc từ 1 đến 100.000.", () => _kham.KeThuoc(_hoSo, _thuoc, 0, "x"));
        _kham.KeThuoc(_hoSo, _thuoc, 10, "Sáng 1 viên");
        Loi($"ZTest Thuốc A {TenDuoi()} đã có trong đơn. Hãy sửa số lượng ở dòng đó.", () => _kham.KeThuoc(_hoSo, _thuoc, 1, "x"));

        // RB28: đổi giá danh mục sau khi kê, đơn giữ giá cũ
        _db.GiaTri($"UPDATE Thuoc SET don_gia_thuoc = 2000 WHERE ma_thuoc = '{_thuoc}'");
        _kham.SuaThuoc(_hoSo, _thuoc, 4, "Tối 1 viên");
        var ct = _kham.DonThuoc(_hoSo).Single();
        Assert.Equal((4, "Tối 1 viên", 1500m, 6000m), (ct.SoLuongThuoc, ct.LieuDung, ct.DonGiaThuoc, ct.ThanhTien));
        Loi($"ZTest Thuốc A {TenDuoi()} chỉ còn 10 trong kho, không kê được 12 (RB27).", () => _kham.SuaThuoc(_hoSo, _thuoc, 12, "x"));

        Loi("Thuốc này không có trong đơn.", () => _kham.SuaThuoc(_hoSo, _thuoc2, 1, "x"));
        _kham.XoaThuoc(_hoSo, _thuoc);
        Assert.Empty(_kham.DonThuoc(_hoSo));
        Loi("Thuốc này không có trong đơn.", () => _kham.XoaThuoc(_hoSo, _thuoc));
    }

    private string TenDuoi() => ((string)_db.GiaTri($"SELECT ten_thuoc FROM Thuoc WHERE ma_thuoc = '{_thuoc}'")!)["ZTest Thuốc A ".Length..];

    [Fact]
    public void HoanTat_RB22_LapHoaDon_RoiKhoa()
    {
        _kham.KeThuoc(_hoSo, _thuoc, 2, "Ngày 2 lần");
        _kham.KeThuoc(_hoSo, _thuoc2, 1, "Tối 1 gói");
        Loi("Vui lòng nhập chẩn đoán trước khi hoàn tất khám (RB22).", () => _kham.HoanTat(_hoSo, "Đau đầu", " ", null, 100000));

        var hd = _kham.HoanTat(_hoSo, "Đau đầu", "Căng thẳng", "Ngủ đủ giấc", 100000);
        Assert.Equal(_hoSo, _db.GiaTri($"SELECT ma_ho_so FROM Hoa_don WHERE ma_hoa_don = '{hd}'"));
        Assert.Equal("Chưa thanh toán", _db.GiaTri($"SELECT trang_thai_thanh_toan FROM Hoa_don WHERE ma_hoa_don = '{hd}'"));
        Assert.Equal(DBNull.Value, _db.GiaTri($"SELECT ma_nv FROM Hoa_don WHERE ma_hoa_don = '{hd}'"));
        var h = _kham.ChiTiet(_hoSo);
        Assert.Equal((TrangThai.HoanTat, 106000m, "Căng thẳng"), (h.TrangThai, h.TongTien, h.ChanDoanBenh)); // 100.000 + 2×1.500 + 3.000
        Assert.True(h.DaHoanTat);
        Assert.DoesNotContain(_kham.HangCho(), x => x.MaHoSo == _hoSo);
        Assert.Equal(1, _kham.TongQuan().HoanTatHomNay);
        // tồn kho chưa trừ: chỉ trừ khi thu ngân thu tiền (RB33)
        Assert.Equal(10, _db.GiaTri($"SELECT so_luong_ton FROM Thuoc WHERE ma_thuoc = '{_thuoc}'"));

        const string khoa = "Hồ sơ đã hoàn tất khám và đã lập hóa đơn, không sửa được nữa.";
        Loi(khoa, () => _kham.Luu(_hoSo, "Ho", "Cảm", null, 0));
        Loi(khoa, () => _kham.KeThuoc(_hoSo, _thuoc, 1, "x"));
        Loi(khoa, () => _kham.SuaThuoc(_hoSo, _thuoc, 1, "x"));
        Loi(khoa, () => _kham.XoaThuoc(_hoSo, _thuoc));
        Loi(khoa, () => _kham.HoanTat(_hoSo, "Ho", "Cảm", null, 0));

        Assert.Contains(_kham.DanhSach(_homNay, _homNay, TrangThai.HoanTat, null), x => x.MaHoSo == _hoSo);
        Assert.DoesNotContain(_kham.DanhSach(_homNay, _homNay, TrangThai.ChoKham, null), x => x.MaHoSo == _hoSo);
        Assert.Contains(_kham.DanhSach(null, null, null, "Căng thẳng"), x => x.MaHoSo == _hoSo);
        Loi("Từ ngày phải trước hoặc bằng đến ngày.", () => _kham.DanhSach(_homNay, _homNay.AddDays(-1), null, null));
    }

    [Fact]
    public void HoanTat_TonGiamSauKhiKe_BaoThieuVaKhongLuu()
    {
        _kham.KeThuoc(_hoSo, _thuoc, 8, "x");
        _db.GiaTri($"UPDATE Thuoc SET so_luong_ton = 5 WHERE ma_thuoc = '{_thuoc}'");
        Loi($"Tồn kho không đủ, vui lòng sửa đơn: ZTest Thuốc A {TenDuoi()} kê 8, còn 5.",
            () => _kham.HoanTat(_hoSo, "Đau đầu đổi", "Cảm", null, 0));
        Assert.Equal(0, _db.GiaTri($"SELECT COUNT(*) FROM Hoa_don WHERE ma_ho_so = '{_hoSo}'"));
        Assert.Equal("Đau đầu", _kham.ChiTiet(_hoSo).TrieuChung); // không lưu nửa vời
    }

    [Fact]
    public void LanKhamTruoc_ChiLayLanDaHoanTatCuaCungKhach()
    {
        _kham.KeThuoc(_hoSo, _thuoc, 2, "Ngày 2 lần");
        _kham.HoanTat(_hoSo, "Đau đầu", "Căng thẳng", null, 0);
        var hs2 = TiepNhanBangLeTan(30);
        var hs3 = TiepNhanBangLeTan(40); // chưa hoàn tất: không hiện
        var ds = _kham.LanKhamTruoc(hs3);
        var lan = Assert.Single(ds);
        Assert.Equal(_hoSo, lan.MaHoSo);
        Assert.Equal("Căng thẳng", lan.ChanDoanBenh);
        Assert.Equal("Nhân viên kiểm thử", lan.TenBacSi);
        Assert.Contains("× 2 viên (Ngày 2 lần)", Assert.Single(lan.Thuoc));
        Assert.Empty(_kham.LanKhamTruoc(_hoSo));
        Assert.Single(_kham.LanKhamTruoc(hs2));
    }

    [Fact]
    public void LichHenVaLichLamViecCuaToi()
    {
        var lich = Assert.Single(_kham.LichLamViecSapToi(), l => l.NgayLamViec == _homNay);
        Assert.Empty(_kham.LichHenCuaToi(null, null));
        var hen = _db.Xoa("Lich_hen", "ma_lich_hen", "LHZ" + Random.Shared.Next(1000000, 9999999));
        _db.GiaTri($@"INSERT INTO Lich_hen (ma_lich_hen, ngay_gio_dat, trieu_chung_ban_dau, trang_thai, ma_kh, ma_lich_lam_viec)
                      VALUES ('{hen}', DATEADD(HOUR, 6, CAST(CAST(GETDATE() AS date) AS datetime)), N'Ho', N'Đã đặt', '{_khach}', '{lich.MaLichLamViec}')");
        var lh = Assert.Single(_kham.LichHenCuaToi(_homNay, TrangThai.DaDat));
        Assert.Equal((hen, "ZTest Khách khám"), (lh.MaLichHen, lh.TenKhachHang));
        Assert.Empty(_kham.LichHenCuaToi(_homNay, TrangThai.DaHuy));
        Loi("Trạng thái lịch hẹn không hợp lệ.", () => _kham.LichHenCuaToi(null, "Lạ"));
    }
}
