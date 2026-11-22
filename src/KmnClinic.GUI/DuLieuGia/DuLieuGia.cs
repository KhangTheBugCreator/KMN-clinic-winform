using KmnClinic.DTO;

namespace KmnClinic.GUI.DuLieuGia;

/// <summary>
/// DỮ LIỆU GIẢ chỉ dùng ở Giai đoạn 2 (khung giao diện), để xem bố cục khi chưa nối CSDL.
/// Tên, số điện thoại, thuốc lấy theo database/du-lieu-mau-day-du.sql cho giống thật.
/// Từ module W0 trở đi, các màn hình sẽ lấy dữ liệu qua BUS → DAL thay cho lớp này.
/// </summary>
public static class Gia
{
    public static DateTime HomNay => DateTime.Today;

    public static readonly List<KhoaDTO> Khoa =
    [
        new() { MaKhoa = "K0001", TenKhoa = "Nội tổng quát" },
        new() { MaKhoa = "K0002", TenKhoa = "Nhi" },
        new() { MaKhoa = "K0003", TenKhoa = "Tai Mũi Họng" },
        new() { MaKhoa = "K0004", TenKhoa = "Da liễu" },
    ];

    public static readonly List<CaDTO> Ca =
    [
        new() { MaCa = "CA0001", TenCa = "Ca sáng", GioBatDau = new(7, 0, 0), GioKetThuc = new(11, 30, 0) },
        new() { MaCa = "CA0002", TenCa = "Ca chiều", GioBatDau = new(13, 0, 0), GioKetThuc = new(17, 0, 0) },
    ];

    public static readonly List<string> BacSi =
    [
        "Nguyễn Văn Hùng", "Trần Thị Mai Phương", "Phạm Thị Hồng Nhung", "Đỗ Văn Thắng",
        "Huỳnh Công Danh", "Lâm Thị Kiều Oanh", "Quách Thị Minh Thư", "Đinh Gia Bảo",
    ];

    public static readonly List<KhachHangDTO> KhachHang =
    [
        new() { MaKh = "KH0001", HoTenKh = "Nguyễn Hoàng Long", SdtKh = "0901234567", GioiTinhKh = "Nam", NgaySinhKh = new(1990, 3, 15), DiaChiKh = "45 Phạm Thái Bường, Phường 4, Vĩnh Long" },
        new() { MaKh = "KH0002", HoTenKh = "Trần Thị Bích Ngọc", SdtKh = "0912345678", GioiTinhKh = "Nữ", NgaySinhKh = new(1985, 7, 22), DiaChiKh = "12 Nguyễn Huệ, Phường 2, Vĩnh Long" },
        new() { MaKh = "KH0003", HoTenKh = "Lê Văn Tài", SdtKh = "0923456789", GioiTinhKh = "Nam", NgaySinhKh = new(1972, 11, 5), DiaChiKh = "88 Trần Phú, Phường 4, Vĩnh Long" },
        new() { MaKh = "KH0004", HoTenKh = "Phạm Thị Hoa", SdtKh = "0934567890", GioiTinhKh = "Nữ", NgaySinhKh = new(1995, 1, 30), DiaChiKh = "210 Lê Thái Tổ, Phường 2, Vĩnh Long" },
        new() { MaKh = "KH0005", HoTenKh = "Võ Minh Tuấn", SdtKh = "0945678901", GioiTinhKh = "Nam", NgaySinhKh = new(2001, 9, 12), DiaChiKh = "7 Đinh Tiên Hoàng, Phường 8, Vĩnh Long" },
        new() { MaKh = "KH0006", HoTenKh = "Huỳnh Thị Cẩm Tú", SdtKh = "0356789012", GioiTinhKh = "Nữ", NgaySinhKh = new(1998, 4, 8), DiaChiKh = "33 Hưng Đạo Vương, Phường 1, Vĩnh Long" },
        new() { MaKh = "KH0007", HoTenKh = "Đặng Quốc Bảo", SdtKh = "0367890123", GioiTinhKh = "Nam", NgaySinhKh = new(2018, 6, 19), DiaChiKh = "15 Nguyễn Văn Thiệt, Phường 3, Vĩnh Long" },
        new() { MaKh = "KH0009", HoTenKh = "Ngô Văn Phúc", SdtKh = "0789012345", GioiTinhKh = "Nam", NgaySinhKh = new(1955, 2, 14), DiaChiKh = "102 Mậu Thân, Phường 3, Vĩnh Long" },
    ];

    public static readonly List<ThuocDTO> Thuoc =
    [
        new() { MaThuoc = "TH0001", TenThuoc = "Paracetamol", Nsx = "Dược Hậu Giang", HamLuong = 500, DonViTinh = "Viên", DonGiaThuoc = 1000, SoLuongTon = 1000, MoTa = "Hạ sốt, giảm đau." },
        new() { MaThuoc = "TH0002", TenThuoc = "Paracetamol (siro trẻ em)", Nsx = "Dược Hậu Giang", HamLuong = 120, DonViTinh = "Chai", DonGiaThuoc = 28000, SoLuongTon = 60, MoTa = "Hạ sốt, giảm đau cho trẻ em." },
        new() { MaThuoc = "TH0003", TenThuoc = "Ibuprofen", Nsx = "Stada Việt Nam", HamLuong = 400, DonViTinh = "Viên", DonGiaThuoc = 1500, SoLuongTon = 500, MoTa = "Giảm đau, kháng viêm, hạ sốt." },
        new() { MaThuoc = "TH0004", TenThuoc = "Amoxicillin", Nsx = "Imexpharm", HamLuong = 500, DonViTinh = "Viên", DonGiaThuoc = 1800, SoLuongTon = 800, MoTa = "Kháng sinh nhóm penicillin." },
        new() { MaThuoc = "TH0006", TenThuoc = "Cefuroxim", Nsx = "Pymepharco", HamLuong = 500, DonViTinh = "Viên", DonGiaThuoc = 7000, SoLuongTon = 300, MoTa = "Kháng sinh nhóm cephalosporin." },
        new() { MaThuoc = "TH0008", TenThuoc = "Cetirizin", Nsx = "Traphaco", HamLuong = 10, DonViTinh = "Viên", DonGiaThuoc = 800, SoLuongTon = 600, MoTa = "Kháng histamin, chống dị ứng." },
        new() { MaThuoc = "TH0010", TenThuoc = "Desloratadin (siro)", Nsx = "Domesco", HamLuong = 2.5m, DonViTinh = "Chai", DonGiaThuoc = 45000, SoLuongTon = 8, MoTa = "Chống dị ứng cho trẻ em." },
        new() { MaThuoc = "TH0011", TenThuoc = "Omeprazol", Nsx = "Dược Hậu Giang", HamLuong = 20, DonViTinh = "Viên", DonGiaThuoc = 1200, SoLuongTon = 700, MoTa = "Giảm tiết acid dạ dày." },
        new() { MaThuoc = "TH0013", TenThuoc = "Oresol", Nsx = "Bidiphar", HamLuong = 4.1m, DonViTinh = "Gói", DonGiaThuoc = 2500, SoLuongTon = 400, MoTa = "Bù nước và điện giải." },
        new() { MaThuoc = "TH0016", TenThuoc = "Amlodipin", Nsx = "Domesco", HamLuong = 5, DonViTinh = "Viên", DonGiaThuoc = 600, SoLuongTon = 800, MoTa = "Điều trị tăng huyết áp." },
        new() { MaThuoc = "TH0019", TenThuoc = "Ambroxol", Nsx = "Mekophar", HamLuong = 30, DonViTinh = "Viên", DonGiaThuoc = 700, SoLuongTon = 15, MoTa = "Long đờm." },
        new() { MaThuoc = "TH0023", TenThuoc = "Xylometazolin (xịt mũi)", Nsx = "Traphaco", HamLuong = 0.1m, DonViTinh = "Chai", DonGiaThuoc = 25000, SoLuongTon = 100, MoTa = "Giảm nghẹt mũi." },
        new() { MaThuoc = "TH0027", TenThuoc = "Clotrimazol (kem bôi)", Nsx = "Medipharco", HamLuong = 1, DonViTinh = "Tuýp", DonGiaThuoc = 15000, SoLuongTon = 150, MoTa = "Điều trị nấm da." },
    ];

    public static List<LichHenDTO> LichHen()
    {
        var d = HomNay;
        return
        [
            new() { MaLichHen = "LH0012", NgayGioDat = d.AddDays(-2).AddHours(9.2), NgayKham = d, TenCa = "Ca sáng", TenKhachHang = "Nguyễn Hoàng Long", SdtKhachHang = "0901234567", TenBacSi = "Nguyễn Văn Hùng", TenKhoa = "Nội tổng quát", TrieuChungBanDau = "Ho, sốt nhẹ 2 ngày", TrangThai = TrangThai.DaXacNhan, LeTanXacNhan = "Nguyễn Thị Thu Trang" },
            new() { MaLichHen = "LH0013", NgayGioDat = d.AddDays(-1).AddHours(14.5), NgayKham = d, TenCa = "Ca sáng", TenKhachHang = "Trần Thị Bích Ngọc", SdtKhachHang = "0912345678", TenBacSi = "Lâm Thị Kiều Oanh", TenKhoa = "Tai Mũi Họng", TrieuChungBanDau = "Đau họng, khàn tiếng", TrangThai = TrangThai.DaDat },
            new() { MaLichHen = "LH0014", NgayGioDat = d.AddDays(-1).AddHours(16), NgayKham = d, TenCa = "Ca chiều", TenKhachHang = "Phạm Thị Hoa", SdtKhachHang = "0934567890", TenBacSi = "Quách Thị Minh Thư", TenKhoa = "Da liễu", TrieuChungBanDau = "Nổi mẩn ngứa ở tay", TrangThai = TrangThai.DaXacNhan, LeTanXacNhan = "Nguyễn Thị Thu Trang" },
            new() { MaLichHen = "LH0011", NgayGioDat = d.AddDays(-3).AddHours(10), NgayKham = d, TenCa = "Ca sáng", TenKhachHang = "Lê Văn Tài", SdtKhachHang = "0923456789", TenBacSi = "Nguyễn Văn Hùng", TenKhoa = "Nội tổng quát", TrieuChungBanDau = "Tái khám huyết áp", TrangThai = TrangThai.DaKham, LeTanXacNhan = "Phạm Ngọc Bích" },
            new() { MaLichHen = "LH0015", NgayGioDat = d.AddHours(-2), NgayKham = d.AddDays(1), TenCa = "Ca sáng", TenKhachHang = "Đặng Quốc Bảo", SdtKhachHang = "0367890123", TenBacSi = "Phạm Thị Hồng Nhung", TenKhoa = "Nhi", TrieuChungBanDau = "Sốt, biếng ăn", TrangThai = TrangThai.DaDat },
            new() { MaLichHen = "LH0016", NgayGioDat = d.AddHours(-1), NgayKham = d.AddDays(2), TenCa = "Ca chiều", TenKhachHang = "Võ Minh Tuấn", SdtKhachHang = "0945678901", TenBacSi = "Huỳnh Công Danh", TenKhoa = "Tai Mũi Họng", TrieuChungBanDau = "Nghẹt mũi kéo dài", TrangThai = TrangThai.DaDat },
            new() { MaLichHen = "LH0010", NgayGioDat = d.AddDays(-4), NgayKham = d.AddDays(-1), TenCa = "Ca chiều", TenKhachHang = "Huỳnh Thị Cẩm Tú", SdtKhachHang = "0356789012", TenBacSi = "Đinh Gia Bảo", TenKhoa = "Da liễu", TrieuChungBanDau = "Mụn trứng cá", TrangThai = TrangThai.DaHuy },
        ];
    }

    public static List<HoSoKhamDTO> HoSoHomNay()
    {
        var d = HomNay;
        return
        [
            new() { MaHoSo = "HS0021", NgayGioKham = d.AddHours(7.3), MaKh = "KH0003", TenKhachHang = "Lê Văn Tài", GioiTinh = "Nam", Tuoi = d.Year - 1972, TenBacSi = "Nguyễn Văn Hùng", TrieuChung = "Tái khám huyết áp", MaLichHen = "LH0011", TrangThai = TrangThai.HoanTat, ChanDoanBenh = "Tăng huyết áp độ 1", TienCongKham = 150000 },
            new() { MaHoSo = "HS0022", NgayGioKham = d.AddHours(7.8), MaKh = "KH0009", TenKhachHang = "Ngô Văn Phúc", GioiTinh = "Nam", Tuoi = d.Year - 1955, TenBacSi = "Nguyễn Văn Hùng", TrieuChung = "Đau thượng vị, ợ chua", TrangThai = TrangThai.DangKham },
            new() { MaHoSo = "HS0023", NgayGioKham = d.AddHours(8.1), MaKh = "KH0001", TenKhachHang = "Nguyễn Hoàng Long", GioiTinh = "Nam", Tuoi = d.Year - 1990, TenBacSi = "Nguyễn Văn Hùng", TrieuChung = "Ho, sốt nhẹ 2 ngày", MaLichHen = "LH0012", TrangThai = TrangThai.ChoKham },
            new() { MaHoSo = "HS0024", NgayGioKham = d.AddHours(8.4), MaKh = "KH0005", TenKhachHang = "Võ Minh Tuấn", GioiTinh = "Nam", Tuoi = d.Year - 2001, TenBacSi = "Nguyễn Văn Hùng", TrieuChung = "Mệt mỏi, chóng mặt", TrangThai = TrangThai.ChoKham },
        ];
    }

    public static List<ChiTietDonThuocDTO> DonThuocMau() =>
    [
        new() { MaThuoc = "TH0001", TenThuoc = "Paracetamol", DonViTinh = "Viên", SoLuongThuoc = 10, LieuDung = "Uống 1 viên khi sốt trên 38,5°C, cách 6 giờ", DonGiaThuoc = 1000 },
        new() { MaThuoc = "TH0004", TenThuoc = "Amoxicillin", DonViTinh = "Viên", SoLuongThuoc = 14, LieuDung = "Uống 1 viên x 2 lần/ngày sau ăn, 7 ngày", DonGiaThuoc = 1800 },
        new() { MaThuoc = "TH0019", TenThuoc = "Ambroxol", DonViTinh = "Viên", SoLuongThuoc = 10, LieuDung = "Uống 1 viên x 2 lần/ngày", DonGiaThuoc = 700 },
    ];

    public static List<HoaDonDTO> HoaDon()
    {
        var d = HomNay;
        return
        [
            new() { MaHoaDon = "HD0018", MaHoSo = "HS0021", NgayKham = d.AddHours(7.3), TenKhachHang = "Lê Văn Tài", SdtKhachHang = "0923456789", TenBacSi = "Nguyễn Văn Hùng", TienCongKham = 150000, TongTien = 172000, TrangThaiThanhToan = TrangThai.ChuaThanhToan },
            new() { MaHoaDon = "HD0017", MaHoSo = "HS0019", NgayKham = d.AddHours(7.1), TenKhachHang = "Phạm Thị Hoa", SdtKhachHang = "0934567890", TenBacSi = "Quách Thị Minh Thư", TienCongKham = 120000, TongTien = 165000, TrangThaiThanhToan = TrangThai.ChuaThanhToan },
            new() { MaHoaDon = "HD0016", MaHoSo = "HS0018", NgayKham = d.AddDays(-1).AddHours(15), TenKhachHang = "Trần Thị Bích Ngọc", SdtKhachHang = "0912345678", TenBacSi = "Lâm Thị Kiều Oanh", TienCongKham = 150000, TongTien = 215000, TrangThaiThanhToan = TrangThai.DaThanhToan, NgayGioThanhToan = d.AddDays(-1).AddHours(15.6), ThuNgan = "Bùi Văn Lộc" },
            new() { MaHoaDon = "HD0015", MaHoSo = "HS0017", NgayKham = d.AddDays(-1).AddHours(9), TenKhachHang = "Đặng Quốc Bảo", SdtKhachHang = "0367890123", TenBacSi = "Phạm Thị Hồng Nhung", TienCongKham = 100000, TongTien = 156000, TrangThaiThanhToan = TrangThai.DaThanhToan, NgayGioThanhToan = d.AddDays(-1).AddHours(9.7), ThuNgan = "Bùi Văn Lộc" },
        ];
    }

    /// <summary>Lịch trực trong tuần chứa ngày đã cho, theo cùng quy luật xoay vòng của dữ liệu mẫu.</summary>
    public static List<LichLamViecDTO> LichTuan(DateTime ngayTrongTuan)
    {
        var thu2 = ngayTrongTuan.Date.AddDays(-(((int)ngayTrongTuan.DayOfWeek + 6) % 7));
        var phong = new[] { ("P0001", "Phòng 101", "Nội tổng quát"), ("P0003", "Phòng 201", "Nhi"), ("P0005", "Phòng 301", "Tai Mũi Họng"), ("P0007", "Phòng 401", "Da liễu") };
        var bs = new[] { new[] { "Nguyễn Văn Hùng", "Trần Thị Mai Phương" }, new[] { "Phạm Thị Hồng Nhung", "Đỗ Văn Thắng" }, new[] { "Huỳnh Công Danh", "Lâm Thị Kiều Oanh" }, new[] { "Quách Thị Minh Thư", "Đinh Gia Bảo" } };
        var kq = new List<LichLamViecDTO>();
        int so = 1;
        for (int n = 0; n < 7; n++)
            for (int p = 0; p < phong.Length; p++)
                for (int c = 0; c < 2; c++)
                    kq.Add(new LichLamViecDTO
                    {
                        MaLichLamViec = $"LV{so++:0000}", NgayLamViec = thu2.AddDays(n),
                        MaPhong = phong[p].Item1, TenPhong = phong[p].Item2, TenKhoa = phong[p].Item3,
                        MaCa = Ca[c].MaCa, TenCa = Ca[c].TenCa, TenBacSi = bs[p][(n + c) % 2]
                    });
        return kq;
    }

    /// <summary>Doanh thu 14 ngày gần nhất (triệu đồng) cho biểu đồ tổng quan.</summary>
    public static (DateTime ngay, double trieu)[] DoanhThu14Ngay()
    {
        double[] v = [3.2, 4.1, 2.8, 5.0, 4.6, 3.9, 1.8, 3.5, 4.4, 5.2, 4.8, 3.7, 2.1, 4.0];
        return v.Select((x, i) => (HomNay.AddDays(i - 13), x)).ToArray();
    }
}
