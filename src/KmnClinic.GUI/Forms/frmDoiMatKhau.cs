using KmnClinic.BUS;

namespace KmnClinic.GUI.Forms;

/// <summary>Nhân viên đang đăng nhập tự đổi mật khẩu (RB36). Mật khẩu mới được băm PBKDF2 trước khi lưu.</summary>
public partial class frmDoiMatKhau : Form
{
    private readonly NhanVienBUS _bus = new();

    public frmDoiMatKhau()
    {
        InitializeComponent();
        lblTaiKhoan.Text = $"{PhienDangNhap.NhanVien?.HoTenNv} ({PhienDangNhap.NhanVien?.SdtNv})";
    }

    private void chkHien_CheckedChanged(object? sender, EventArgs e)
    {
        var an = !chkHien.Checked;
        txtCu.UseSystemPasswordChar = an;
        txtMoi.UseSystemPasswordChar = an;
        txtNhapLai.UseSystemPasswordChar = an;
    }

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        lblLoi.Text = "";
        try
        {
            _bus.DoiMatKhau(txtCu.Text, txtMoi.Text, txtNhapLai.Text);
            MessageBox.Show(this, "Đã đổi mật khẩu. Lần đăng nhập sau dùng mật khẩu mới.", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (LoiNghiepVu ex)
        {
            lblLoi.Text = ex.Message;
        }
    }
}
