using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace KmnClinic.BUS;

/// <summary>Hàm dùng chung của các lớp BUS: kiểm tra dữ liệu nhập và đổi lỗi SQL thành LoiNghiepVu.</summary>
internal static class KiemTra
{
    /// <summary>Bắt buộc nhập, cắt khoảng trắng, không vượt độ dài cột.</summary>
    public static string BatBuoc(string? giaTri, string tenTruong, int doDaiToiDa)
    {
        var s = (giaTri ?? "").Trim();
        if (s.Length == 0) throw new LoiNghiepVu($"Vui lòng nhập {tenTruong}.");
        if (s.Length > doDaiToiDa) throw new LoiNghiepVu($"{VietHoa(tenTruong)} tối đa {doDaiToiDa} ký tự.");
        return s;
    }

    /// <summary>Không bắt buộc: chuỗi rỗng thành null.</summary>
    public static string? TuyChon(string? giaTri, string tenTruong, int doDaiToiDa)
    {
        var s = (giaTri ?? "").Trim();
        if (s.Length == 0) return null;
        if (s.Length > doDaiToiDa) throw new LoiNghiepVu($"{VietHoa(tenTruong)} tối đa {doDaiToiDa} ký tự.");
        return s;
    }

    /// <summary>Số điện thoại Việt Nam: 10 chữ số, bắt đầu bằng 0 (cột varchar(10)).</summary>
    public static string SoDienThoai(string? sdt)
    {
        var s = (sdt ?? "").Trim().Replace(" ", "").Replace(".", "");
        if (s.Length == 0) throw new LoiNghiepVu("Vui lòng nhập số điện thoại.");
        if (!Regex.IsMatch(s, @"^0\d{9}$")) throw new LoiNghiepVu("Số điện thoại gồm 10 chữ số, bắt đầu bằng 0.");
        return s;
    }

    /// <summary>Ngày sinh không ở tương lai, không quá 130 năm trước. Bỏ phần giờ.</summary>
    public static DateTime? NgaySinh(DateTime? ngay)
    {
        if (ngay is null) return null;
        var d = ngay.Value.Date;
        if (d > DateTime.Today) throw new LoiNghiepVu("Ngày sinh không được ở tương lai.");
        if (d < DateTime.Today.AddYears(-130)) throw new LoiNghiepVu("Ngày sinh không hợp lệ.");
        return d;
    }

    /// <summary>
    /// Đường dẫn ảnh nhân viên: bỏ trống được; nếu có phải là URL https đầy đủ, không khoảng trắng, tối đa 1000 ký tự
    /// (CSDL chỉ lưu đường dẫn, ảnh nằm trên mạng; phần mềm tải về để hiện bằng PictureBox).
    /// </summary>
    public static string? DuongDanAnh(string? url)
    {
        if (string.IsNullOrWhiteSpace(url)) return null;
        url = url.Trim();
        if (url.Length > 1000) throw new LoiNghiepVu("Đường dẫn ảnh tối đa 1000 ký tự.");
        if (url.Any(c => char.IsControl(c) || char.IsWhiteSpace(c))) throw new LoiNghiepVu("Đường dẫn ảnh không được chứa khoảng trắng.");
        if (!Uri.TryCreate(url, UriKind.Absolute, out var uri)) throw new LoiNghiepVu("Đường dẫn ảnh không đúng định dạng.");
        if (uri.Scheme != Uri.UriSchemeHttps) throw new LoiNghiepVu("Đường dẫn ảnh phải bắt đầu bằng https://");
        if (!string.IsNullOrEmpty(uri.UserInfo)) throw new LoiNghiepVu("Đường dẫn ảnh không được chứa tên đăng nhập hoặc mật khẩu.");
        return url;
    }

    /// <summary>
    /// Chạy lệnh ghi CSDL. Lỗi SQL có câu tiếng Việt (trùng tên, vi phạm CHECK, trigger) thành LoiNghiepVu
    /// để GUI và kiểm thử chỉ cần bắt một loại lỗi.
    /// </summary>
    public static T Ghi<T>(Func<T> lenh)
    {
        try
        {
            return lenh();
        }
        catch (SqlException ex) when (DichLoiSql.Dich(ex) is { } thongBao)
        {
            throw new LoiNghiepVu(thongBao, ex);
        }
    }

    private static string VietHoa(string s) => s.Length == 0 ? s : char.ToUpper(s[0]) + s[1..];
}
