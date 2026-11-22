using KmnClinic.BUS;
using Microsoft.Data.SqlClient;

namespace KmnClinic.GUI;

/// <summary>Hiện lỗi cho người dùng: lỗi nghiệp vụ và lỗi CSDL hiện câu tiếng Việt, lỗi khác hiện câu gốc.</summary>
public static class ThongBaoLoi
{
    public static string CauThongBao(Exception ex) => ex switch
    {
        LoiNghiepVu l => l.Message,
        SqlException s => DichLoiSql.Dich(s) ?? "Lỗi CSDL: " + s.Message,
        _ => ex.Message,
    };

    public static void Hien(Exception ex, IWin32Window? chu = null)
    {
        var nghiepVu = ex is LoiNghiepVu;
        MessageBox.Show(chu, CauThongBao(ex), nghiepVu ? "Thông báo" : "Có lỗi xảy ra",
            MessageBoxButtons.OK, nghiepVu ? MessageBoxIcon.Warning : MessageBoxIcon.Error);
    }
}
