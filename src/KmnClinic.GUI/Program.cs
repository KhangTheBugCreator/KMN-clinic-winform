using KmnClinic.BUS;
using KmnClinic.DAL;
using KmnClinic.GUI.Forms;

namespace KmnClinic.GUI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // DPI, visual styles lấy từ KmnClinic.GUI.csproj
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        // Lỗi chưa bắt ở form nào: lỗi nghiệp vụ / lỗi SQL hiện câu tiếng Việt (ThongBaoLoi)
        Application.ThreadException += (_, e) => ThongBaoLoi.Hien(e.Exception);

        try
        {
            DbHelper.ChuoiKetNoi = CauHinh.DocChuoiKetNoi();
        }
        catch (Exception ex)
        {
            MessageBox.Show("File appsettings.json không đúng định dạng JSON.\n\n" + ex.Message, "Lỗi cấu hình",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Vòng lặp: đăng nhập → form chính → đăng xuất thì quay lại đăng nhập, đóng form chính thì thoát.
        while (true)
        {
            using var dangNhap = new frmDangNhap();
            if (dangNhap.ShowDialog() != DialogResult.OK) return;

            using var chinh = new frmChinh();
            Application.Run(chinh);
            PhienDangNhap.DangXuat();
            if (!chinh.DangXuat) return;
        }
    }
}
