using KmnClinic.BUS;

namespace KmnClinic.GUI.Forms;

/// <summary>Đăng nhập bằng số điện thoại + mật khẩu (RB35–RB39). Kiểm tra trong CSDL qua NhanVienBUS.</summary>
public partial class frmDangNhap : Form
{
    private readonly NhanVienBUS _bus = new();

    public frmDangNhap()
    {
        InitializeComponent();
    }

    private void chkHienMatKhau_CheckedChanged(object? sender, EventArgs e)
        => txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;

    private void btnDangNhap_Click(object? sender, EventArgs e)
    {
        lblLoi.Text = "";
        Cursor = Cursors.WaitCursor;
        try
        {
            _bus.DangNhap(txtSdt.Text, txtMatKhau.Text);
            DialogResult = DialogResult.OK;
        }
        catch (LoiNghiepVu ex)
        {
            lblLoi.Text = ex.Message;
            txtMatKhau.SelectAll();
            txtMatKhau.Focus();
        }
        catch (Exception ex)
        {
            // Thường là không kết nối được SQL Server: báo rõ để kiểm tra appsettings.json
            lblLoi.Text = "Không đăng nhập được, xem thông báo lỗi.";
            ThongBaoLoi.Hien(ex, this);
        }
        finally
        {
            Cursor = Cursors.Default;
        }
    }
}
