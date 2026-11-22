using System.Text.Json;

namespace KmnClinic.GUI;

/// <summary>
/// Đọc chuỗi kết nối từ appsettings.json đặt cạnh KmnClinic.exe:
/// { "ConnectionStrings": { "PhongKham": "Server=.;Database=PHONGKHAM;..." } }
/// Không có file thì dùng chuỗi mặc định (SQL Server cài sẵn trên máy, đăng nhập Windows).
/// </summary>
public static class CauHinh
{
    public const string ChuoiMacDinh = "Server=.;Database=PHONGKHAM;Trusted_Connection=True;TrustServerCertificate=True";

    public static string DocChuoiKetNoi()
    {
        var duongDan = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(duongDan)) return ChuoiMacDinh;

        using var doc = JsonDocument.Parse(File.ReadAllText(duongDan),
            new JsonDocumentOptions { CommentHandling = JsonCommentHandling.Skip, AllowTrailingCommas = true });
        if (doc.RootElement.TryGetProperty("ConnectionStrings", out var cs) &&
            cs.TryGetProperty("PhongKham", out var pk) &&
            pk.GetString() is { Length: > 0 } chuoi)
            return chuoi;
        return ChuoiMacDinh;
    }
}
