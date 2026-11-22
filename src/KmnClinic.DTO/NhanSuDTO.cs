namespace KmnClinic.DTO;

/// <summary>Bảng Nhan_vien (không mang mật khẩu ra giao diện).</summary>
public class NhanVienDTO
{
    public string MaNv { get; set; } = "";
    public string HoTenNv { get; set; } = "";
    public DateTime? NgaySinhNv { get; set; }
    public string SdtNv { get; set; } = "";
    public string ChucVu { get; set; } = "";
    public string? MaKhoa { get; set; }
    public string? TenKhoa { get; set; }
    public string? DuongDanAnh { get; set; }
    public override string ToString() => HoTenNv;
}

/// <summary>Bảng Khach_hang.</summary>
public class KhachHangDTO
{
    public string MaKh { get; set; } = "";
    public string HoTenKh { get; set; } = "";
    public DateTime? NgaySinhKh { get; set; }
    public string? GioiTinhKh { get; set; }
    public string SdtKh { get; set; } = "";
    public string? DiaChiKh { get; set; }
    public override string ToString() => $"{HoTenKh} – {SdtKh}";
}

/// <summary>Các giá trị chuc_vu hợp lệ (RB5).</summary>
public static class ChucVu
{
    public const string QuanTriVien = "Quản trị viên";
    public const string LeTan = "Lễ tân";
    public const string BacSi = "Bác sĩ";
    public const string ThuNgan = "Thu ngân";
}
