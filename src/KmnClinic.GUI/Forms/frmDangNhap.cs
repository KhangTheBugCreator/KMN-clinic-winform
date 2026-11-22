using KmnClinic.GUI.DuLieuGia;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Đăng nhập bằng số điện thoại + mật khẩu (RB35–RB39).
/// Giai đoạn 2: kiểm tra trên danh sách tài khoản giả. Module W0 sẽ thay bằng NhanVienBUS.DangNhap (băm PBKDF2).
/// </summary>
public partial class frmDangNhap : Form
{
    public frmDangNhap()
    {
        InitializeComponent();
    }

    private void chkHienMatKhau_CheckedChanged(object? sender, EventArgs e)
        => txtMatKhau.UseSystemPasswordChar = !chkHienMatKhau.Checked;

    private void btnDangNhap_Click(object? sender, EventArgs e)
    {
        lblLoi.Text = "";
        var sdt = txtSdt.Text.Trim();
        if (sdt.Length == 0) { lblLoi.Text = "Vui lòng nhập số điện thoại."; txtSdt.Focus(); return; }
        if (txtMatKhau.Text.Length < 8) { lblLoi.Text = "Mật khẩu tối thiểu 8 ký tự."; txtMatKhau.Focus(); return; }

        var nv = Gia.NhanVien.FirstOrDefault(x => x.SdtNv == sdt);
        if (nv is null || txtMatKhau.Text != "matkhau123")
        {
            lblLoi.Text = "Số điện thoại hoặc mật khẩu không đúng.";
            return;
        }
        PhienDangNhap.DangNhap(nv);
        DialogResult = DialogResult.OK;
    }
}
