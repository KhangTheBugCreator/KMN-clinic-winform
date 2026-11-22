namespace KmnClinic.GUI.Theme;

/// <summary>Bảng màu KMN Clinic, lấy đúng theo theme.css của bản web để hai bản trông thống nhất.</summary>
public static class Mau
{
    public static readonly Color Navy = ColorTranslator.FromHtml("#17366f");
    public static readonly Color Blue = ColorTranslator.FromHtml("#1f6fd1");
    public static readonly Color Blue600 = ColorTranslator.FromHtml("#1a5fb4");
    public static readonly Color Cyan = ColorTranslator.FromHtml("#22b3e6");
    public static readonly Color Green = ColorTranslator.FromHtml("#4caf6a");
    public static readonly Color GreenSoft = ColorTranslator.FromHtml("#e3f4e8");
    public static readonly Color Sky = ColorTranslator.FromHtml("#eaf4fd");
    public static readonly Color Sky2 = ColorTranslator.FromHtml("#f5f9fe");
    public static readonly Color Text = ColorTranslator.FromHtml("#2b3445");
    public static readonly Color Muted = ColorTranslator.FromHtml("#6b7686");
    public static readonly Color Border = ColorTranslator.FromHtml("#e3e9f1");
    public static readonly Color Red = ColorTranslator.FromHtml("#e5534b");
    public static readonly Color Orange = ColorTranslator.FromHtml("#f2a33a");
    public static readonly Color Gray = ColorTranslator.FromHtml("#9aa3af");

    /// <summary>Nền thanh menu bên trái và màu mục đang chọn.</summary>
    public static readonly Color SidebarNen = Navy;
    public static readonly Color SidebarChon = ColorTranslator.FromHtml("#24498f");
    public static readonly Color SidebarChu = ColorTranslator.FromHtml("#c9d6ee");

    /// <summary>Màu badge theo trạng thái nghiệp vụ (giống --st-* của web).</summary>
    public static Color TheoTrangThai(string? trangThai) => trangThai switch
    {
        "Đã đặt" => Orange,
        "Đã xác nhận" => Blue,
        "Đã khám" or "Đã thanh toán" or "Hoàn tất" => Green,
        "Đã hủy" => Gray,
        "Chưa thanh toán" => Red,
        "Chờ khám" => Orange,
        "Đang khám" => Blue,
        _ => Muted
    };
}
