using KmnClinic.GUI.Forms;

namespace KmnClinic.GUI;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        // Font mặc định, DPI, visual styles lấy từ KmnClinic.GUI.csproj
        ApplicationConfiguration.Initialize();
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        Application.ThreadException += (_, e) =>
            MessageBox.Show(e.Exception.Message, "Có lỗi xảy ra", MessageBoxButtons.OK, MessageBoxIcon.Error);

        // Vòng lặp: đăng nhập → form chính → đăng xuất thì quay lại đăng nhập, đóng form chính thì thoát.
        while (true)
        {
            using var dangNhap = new frmDangNhap();
            if (dangNhap.ShowDialog() != DialogResult.OK) return;

            using var chinh = new frmChinh();
            Application.Run(chinh);
            if (!chinh.DangXuat) return;
            PhienDangNhap.DangXuat();
        }
    }
}
