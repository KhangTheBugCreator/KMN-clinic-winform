namespace KmnClinic.DTO;

/// <summary>Bảng Lich_lam_viec, kèm tên để hiển thị.</summary>
public class LichLamViecDTO
{
    public string MaLichLamViec { get; set; } = "";
    public DateTime NgayLamViec { get; set; }
    public string MaNv { get; set; } = "";
    public string TenBacSi { get; set; } = "";
    public string MaPhong { get; set; } = "";
    public string TenPhong { get; set; } = "";
    public string MaCa { get; set; } = "";
    public string TenCa { get; set; } = "";
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }
    public string MaKhoa { get; set; } = "";
    public string TenKhoa { get; set; } = "";
    /// <summary>Số lịch hẹn gắn với lịch này (mọi trạng thái) và số lịch hẹn còn hiệu lực (Đã đặt, Đã xác nhận).</summary>
    public int SoLichHen { get; set; }
    public int SoLichHenConHieuLuc { get; set; }
    /// <summary>Ngày làm việc đã qua (không sửa/xóa được).</summary>
    public bool DaQua => NgayLamViec.Date < DateTime.Today;
}

/// <summary>Bảng Lich_hen, kèm tên để hiển thị.</summary>
public class LichHenDTO
{
    public string MaLichHen { get; set; } = "";
    public DateTime NgayGioDat { get; set; }
    public DateTime NgayKham { get; set; }
    public string TenCa { get; set; } = "";
    public string TenKhachHang { get; set; } = "";
    public string SdtKhachHang { get; set; } = "";
    public string TenBacSi { get; set; } = "";
    public string TenKhoa { get; set; } = "";
    public string? TrieuChungBanDau { get; set; }
    public string TrangThai { get; set; } = "";
    public string? LeTanXacNhan { get; set; }
    public string MaKh { get; set; } = "";
    public string MaLichLamViec { get; set; } = "";
    public string MaBacSi { get; set; } = "";
    public string TenPhong { get; set; } = "";
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }
    /// <summary>Hồ sơ khám tạo từ lịch hẹn này (khi đã tiếp nhận).</summary>
    public string? MaHoSo { get; set; }
    public string CaKham => $"{TenCa} {GioBatDau:hh\\:mm}–{GioKetThuc:hh\\:mm}";
}

/// <summary>Bác sĩ đang trực lúc này (cho nhận khách vãng lai, RB41), kèm số hồ sơ chưa xong trong ngày.</summary>
public class BacSiDangTrucDTO
{
    public string MaNv { get; set; } = "";
    public string HoTenNv { get; set; } = "";
    public string TenKhoa { get; set; } = "";
    public string TenPhong { get; set; } = "";
    public string TenCa { get; set; } = "";
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }
    public int SoDangCho { get; set; }
    public string CaTruc => $"{TenCa} {GioBatDau:hh\\:mm}–{GioKetThuc:hh\\:mm}";
    public override string ToString() => $"{HoTenNv} · {TenKhoa} · {TenPhong} · {SoDangCho} đang chờ";
}

/// <summary>Số liệu trong ngày cho lễ tân.</summary>
public class TongQuanLeTanDTO
{
    /// <summary>Lịch "Đã đặt" (khách tự đặt trên web) chưa hết ca, cần gọi xác nhận.</summary>
    public int ChoXacNhan { get; set; }
    public int HenHomNay { get; set; }
    /// <summary>Hẹn hôm nay đã xác nhận, chưa tiếp nhận.</summary>
    public int ChoTiepNhan { get; set; }
    public int HoSoHomNay { get; set; }
    public int VangLaiHomNay { get; set; }
    public int BacSiDangTruc { get; set; }
}

/// <summary>Bảng Ho_so_kham, kèm tên để hiển thị.</summary>
public class HoSoKhamDTO
{
    public string MaHoSo { get; set; } = "";
    public DateTime NgayGioKham { get; set; }
    public string MaKh { get; set; } = "";
    public string TenKhachHang { get; set; } = "";
    public string GioiTinh { get; set; } = "";
    public int Tuoi { get; set; }
    public string TenBacSi { get; set; } = "";
    public string? TrieuChung { get; set; }
    public string? ChanDoanBenh { get; set; }
    public string? LoiDan { get; set; }
    public decimal TienCongKham { get; set; }
    public string? MaLichHen { get; set; }
    public string Nguon => MaLichHen is null ? "Vãng lai" : "Có hẹn";
    public string TrangThai { get; set; } = "";
    public string MaBacSi { get; set; } = "";
    public string TenKhoa { get; set; } = "";
    public string SdtKhachHang { get; set; } = "";
    public string? TenLeTan { get; set; }
    public string? MaHoaDon { get; set; }
    public decimal? TongTien { get; set; }
    /// <summary>Đã có hóa đơn: hồ sơ bị khóa, chỉ xem (Q4).</summary>
    public bool DaHoanTat => MaHoaDon is not null;
}

/// <summary>Một lần khám đã hoàn tất trước đó của bệnh nhân (mọi bác sĩ), chỉ đọc (Q13).</summary>
public class LanKhamTruocDTO
{
    public string MaHoSo { get; set; } = "";
    public DateTime NgayGioKham { get; set; }
    public string TenBacSi { get; set; } = "";
    public string TenKhoa { get; set; } = "";
    public string TrieuChung { get; set; } = "";
    public string? ChanDoanBenh { get; set; }
    public string? LoiDan { get; set; }
    public List<string> Thuoc { get; set; } = [];
}

/// <summary>Số liệu của bác sĩ đang đăng nhập.</summary>
public class TongQuanBacSiDTO
{
    public int ChoKham { get; set; }
    public int DangKham { get; set; }
    public int HoanTatHomNay { get; set; }
}

/// <summary>Bảng Chi_tiet_don_thuoc, kèm tên thuốc.</summary>
public class ChiTietDonThuocDTO
{
    public string MaThuoc { get; set; } = "";
    public string TenThuoc { get; set; } = "";
    public string DonViTinh { get; set; } = "";
    public int SoLuongThuoc { get; set; }
    public string? LieuDung { get; set; }
    public decimal DonGiaThuoc { get; set; }
    public decimal ThanhTien => SoLuongThuoc * DonGiaThuoc;
    public decimal? HamLuong { get; set; }
    /// <summary>Tồn kho hiện tại (để báo thiếu khi hoàn tất).</summary>
    public int SoLuongTon { get; set; }
}

/// <summary>Bảng Hoa_don, kèm thông tin hồ sơ.</summary>
public class HoaDonDTO
{
    public string MaHoaDon { get; set; } = "";
    public string MaHoSo { get; set; } = "";
    public DateTime NgayKham { get; set; }
    public string TenKhachHang { get; set; } = "";
    public string SdtKhachHang { get; set; } = "";
    public string TenBacSi { get; set; } = "";
    public decimal TienCongKham { get; set; }
    public decimal TongTien { get; set; }
    public string TrangThaiThanhToan { get; set; } = "";
    public DateTime? NgayGioThanhToan { get; set; }
    public string? ThuNgan { get; set; }
}

/// <summary>Các giá trị trạng thái dùng trong CSDL (RB14, RB15, CHECK của Hoa_don).</summary>
public static class TrangThai
{
    public const string DaDat = "Đã đặt";
    public const string DaXacNhan = "Đã xác nhận";
    public const string DaKham = "Đã khám";
    public const string DaHuy = "Đã hủy";
    public const string ChuaThanhToan = "Chưa thanh toán";
    public const string DaThanhToan = "Đã thanh toán";

    // Trạng thái hồ sơ suy ra (không phải cột trong CSDL), giống bản web.
    public const string ChoKham = "Chờ khám";
    public const string DangKham = "Đang khám";
    public const string HoanTat = "Hoàn tất";
}
