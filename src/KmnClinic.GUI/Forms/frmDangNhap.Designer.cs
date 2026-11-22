namespace KmnClinic.GUI.Forms;

partial class frmDangNhap
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTieuDe = new Label();
        lblSdt = new Label();
        txtSdt = new TextBox();
        lblMatKhau = new Label();
        txtMatKhau = new TextBox();
        chkHienMatKhau = new CheckBox();
        lblLoi = new Label();
        btnDangNhap = new Button();
        btnThoat = new Button();
        lblGoiY = new Label();
        SuspendLayout();
        //
        // lblTieuDe
        //
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
        lblTieuDe.Location = new Point(20, 16);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Text = "ĐĂNG NHẬP HỆ THỐNG";
        //
        // lblSdt
        //
        lblSdt.AutoSize = true;
        lblSdt.Location = new Point(22, 60);
        lblSdt.Name = "lblSdt";
        lblSdt.Text = "Số điện thoại:";
        //
        // txtSdt
        //
        txtSdt.Location = new Point(130, 57);
        txtSdt.MaxLength = 10;
        txtSdt.Name = "txtSdt";
        txtSdt.Size = new Size(220, 23);
        txtSdt.TabIndex = 0;
        //
        // lblMatKhau
        //
        lblMatKhau.AutoSize = true;
        lblMatKhau.Location = new Point(22, 92);
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.Text = "Mật khẩu:";
        //
        // txtMatKhau
        //
        txtMatKhau.Location = new Point(130, 89);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.Size = new Size(220, 23);
        txtMatKhau.TabIndex = 1;
        txtMatKhau.UseSystemPasswordChar = true;
        //
        // chkHienMatKhau
        //
        chkHienMatKhau.AutoSize = true;
        chkHienMatKhau.Location = new Point(130, 118);
        chkHienMatKhau.Name = "chkHienMatKhau";
        chkHienMatKhau.TabIndex = 2;
        chkHienMatKhau.Text = "Hiện mật khẩu";
        chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
        //
        // lblLoi
        //
        lblLoi.ForeColor = Color.Red;
        lblLoi.Location = new Point(22, 144);
        lblLoi.Name = "lblLoi";
        lblLoi.Size = new Size(330, 20);
        //
        // btnDangNhap
        //
        btnDangNhap.Location = new Point(164, 170);
        btnDangNhap.Name = "btnDangNhap";
        btnDangNhap.Size = new Size(90, 28);
        btnDangNhap.TabIndex = 3;
        btnDangNhap.Text = "Đăng nhập";
        btnDangNhap.Click += btnDangNhap_Click;
        //
        // btnThoat
        //
        btnThoat.DialogResult = DialogResult.Cancel;
        btnThoat.Location = new Point(260, 170);
        btnThoat.Name = "btnThoat";
        btnThoat.Size = new Size(90, 28);
        btnThoat.TabIndex = 4;
        btnThoat.Text = "Thoát";
        //
        // lblGoiY
        //
        lblGoiY.ForeColor = SystemColors.GrayText;
        lblGoiY.Location = new Point(22, 212);
        lblGoiY.Name = "lblGoiY";
        lblGoiY.Size = new Size(330, 52);
        lblGoiY.Text = "Bản dữ liệu giả, mật khẩu matkhau123. QTV 0907112233, Lễ tân 0932145678, Bác sĩ 0903126789, Thu ngân 0708671234";
        //
        // frmDangNhap
        //
        AcceptButton = btnDangNhap;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnThoat;
        ClientSize = new Size(374, 274);
        Controls.AddRange(new Control[] { lblTieuDe, lblSdt, txtSdt, lblMatKhau, txtMatKhau, chkHienMatKhau, lblLoi, btnDangNhap, btnThoat, lblGoiY });
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmDangNhap";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "KMN Clinic - Đăng nhập";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTieuDe;
    private Label lblSdt;
    private TextBox txtSdt;
    private Label lblMatKhau;
    private TextBox txtMatKhau;
    private CheckBox chkHienMatKhau;
    private Label lblLoi;
    private Button btnDangNhap;
    private Button btnThoat;
    private Label lblGoiY;
}
