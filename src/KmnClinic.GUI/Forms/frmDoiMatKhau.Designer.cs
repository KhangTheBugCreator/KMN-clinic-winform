namespace KmnClinic.GUI.Forms;

partial class frmDoiMatKhau
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTk = new Label();
        lblTaiKhoan = new Label();
        lblCu = new Label();
        txtCu = new TextBox();
        lblMoi = new Label();
        txtMoi = new TextBox();
        lblNhapLai = new Label();
        txtNhapLai = new TextBox();
        chkHien = new CheckBox();
        lblLoi = new Label();
        btnLuu = new Button();
        btnHuy = new Button();
        SuspendLayout();
        //
        // lblTk
        //
        lblTk.AutoSize = true;
        lblTk.Location = new Point(20, 20);
        lblTk.Name = "lblTk";
        lblTk.Text = "Tài khoản:";
        //
        // lblTaiKhoan
        //
        lblTaiKhoan.AutoSize = true;
        lblTaiKhoan.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblTaiKhoan.Location = new Point(150, 20);
        lblTaiKhoan.Name = "lblTaiKhoan";
        //
        // lblCu
        //
        lblCu.AutoSize = true;
        lblCu.Location = new Point(20, 52);
        lblCu.Name = "lblCu";
        lblCu.Text = "Mật khẩu hiện tại:";
        //
        // txtCu
        //
        txtCu.Location = new Point(150, 49);
        txtCu.Name = "txtCu";
        txtCu.Size = new Size(210, 23);
        txtCu.TabIndex = 0;
        txtCu.UseSystemPasswordChar = true;
        //
        // lblMoi
        //
        lblMoi.AutoSize = true;
        lblMoi.Location = new Point(20, 84);
        lblMoi.Name = "lblMoi";
        lblMoi.Text = "Mật khẩu mới:";
        //
        // txtMoi
        //
        txtMoi.Location = new Point(150, 81);
        txtMoi.Name = "txtMoi";
        txtMoi.Size = new Size(210, 23);
        txtMoi.TabIndex = 1;
        txtMoi.UseSystemPasswordChar = true;
        //
        // lblNhapLai
        //
        lblNhapLai.AutoSize = true;
        lblNhapLai.Location = new Point(20, 116);
        lblNhapLai.Name = "lblNhapLai";
        lblNhapLai.Text = "Nhập lại mật khẩu mới:";
        //
        // txtNhapLai
        //
        txtNhapLai.Location = new Point(150, 113);
        txtNhapLai.Name = "txtNhapLai";
        txtNhapLai.Size = new Size(210, 23);
        txtNhapLai.TabIndex = 2;
        txtNhapLai.UseSystemPasswordChar = true;
        //
        // chkHien
        //
        chkHien.AutoSize = true;
        chkHien.Location = new Point(150, 142);
        chkHien.Name = "chkHien";
        chkHien.TabIndex = 3;
        chkHien.Text = "Hiện mật khẩu";
        chkHien.CheckedChanged += chkHien_CheckedChanged;
        //
        // lblLoi
        //
        lblLoi.ForeColor = Color.Red;
        lblLoi.Location = new Point(20, 168);
        lblLoi.Name = "lblLoi";
        lblLoi.Size = new Size(340, 20);
        //
        // btnLuu
        //
        btnLuu.Location = new Point(174, 196);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new Size(90, 28);
        btnLuu.TabIndex = 4;
        btnLuu.Text = "Lưu";
        btnLuu.Click += btnLuu_Click;
        //
        // btnHuy
        //
        btnHuy.DialogResult = DialogResult.Cancel;
        btnHuy.Location = new Point(270, 196);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new Size(90, 28);
        btnHuy.TabIndex = 5;
        btnHuy.Text = "Hủy";
        //
        // frmDoiMatKhau
        //
        AcceptButton = btnLuu;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnHuy;
        ClientSize = new Size(384, 242);
        Controls.AddRange(new Control[] { lblTk, lblTaiKhoan, lblCu, txtCu, lblMoi, txtMoi, lblNhapLai, txtNhapLai, chkHien, lblLoi, btnLuu, btnHuy });
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmDoiMatKhau";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Đổi mật khẩu";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTk;
    private Label lblTaiKhoan;
    private Label lblCu;
    private TextBox txtCu;
    private Label lblMoi;
    private TextBox txtMoi;
    private Label lblNhapLai;
    private TextBox txtNhapLai;
    private CheckBox chkHien;
    private Label lblLoi;
    private Button btnLuu;
    private Button btnHuy;
}
