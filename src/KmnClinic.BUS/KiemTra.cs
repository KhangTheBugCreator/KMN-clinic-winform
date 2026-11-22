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
