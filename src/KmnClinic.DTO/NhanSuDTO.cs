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
    /// <summary>Số dòng lịch làm việc, lịch hẹn, hồ sơ, hóa đơn trỏ tới nhân viên (có thì không đổi chức vụ/khoa, không xóa).</summary>
    public int SoLienKet { get; set; }
    public override string ToString() => HoTenNv;
}

/// <summary>Bảng Khach_hang.</summary>
public class KhachHangDTO
{
    public string MaKh { get; set; } = "";
    public string HoTenKh { get; set; } = "";
    public DateTime? NgaySinhKh { get; set; }
    /// <summary>"Nam", "Nữ" hoặc null (= giới tính khác).</summary>
    public string? GioiTinhKh { get; set; }
    /// <summary>Giới tính để hiển thị: null hiện là "Khác".</summary>
    public string GioiTinhHienThi => GioiTinhKh ?? "Khác";
    public string SdtKh { get; set; } = "";
    public string? DiaChiKh { get; set; }
    /// <summary>Số lịch hẹn và hồ sơ khám của khách (có thì không xóa được).</summary>
    public int SoLichHen { get; set; }
    public int SoHoSo { get; set; }
    public override string ToString() => $"{HoTenKh} – {SdtKh}";
}

/// <summary>Các giá trị chuc_vu hợp lệ (RB5).</summary>
public static class ChucVu
{
    public const string QuanTriVien = "Quản trị viên";
    public const string LeTan = "Lễ tân";
    public const string BacSi = "Bác sĩ";
    public const string ThuNgan = "Thu ngân";

    public static readonly string[] TatCa = [QuanTriVien, LeTan, BacSi, ThuNgan];
}
