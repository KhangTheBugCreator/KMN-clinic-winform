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
        tlpNen = new TableLayoutPanel();
        pnlTrai = new Panel();
        lblGioiThieu = new Label();
        lblKhauHieu = new Label();
        picLogo = new PictureBox();
        pnlPhai = new Panel();
        lblGoiY = new Label();
        lblLoi = new Label();
        btnDangNhap = new Button();
        chkHienMatKhau = new CheckBox();
        txtMatKhau = new TextBox();
        lblMatKhau = new Label();
        txtSdt = new TextBox();
        lblSdt = new Label();
        lblMoTa = new Label();
        lblTieuDe = new Label();
        tlpNen.SuspendLayout();
        pnlTrai.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
        pnlPhai.SuspendLayout();
        SuspendLayout();
        //
        // tlpNen
        //
        tlpNen.ColumnCount = 2;
        tlpNen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 44F));
        tlpNen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 56F));
        tlpNen.Controls.Add(pnlTrai, 0, 0);
        tlpNen.Controls.Add(pnlPhai, 1, 0);
        tlpNen.Dock = DockStyle.Fill;
        tlpNen.Margin = new Padding(0);
        tlpNen.Name = "tlpNen";
        tlpNen.RowCount = 1;
        tlpNen.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpNen.Size = new Size(920, 560);
        //
        // pnlTrai
        //
        pnlTrai.BackColor = Color.FromArgb(234, 244, 253);
        pnlTrai.Controls.Add(lblGioiThieu);
        pnlTrai.Controls.Add(lblKhauHieu);
        pnlTrai.Controls.Add(picLogo);
        pnlTrai.Dock = DockStyle.Fill;
        pnlTrai.Margin = new Padding(0);
        pnlTrai.Name = "pnlTrai";
        pnlTrai.Padding = new Padding(40);
        //
        // picLogo
        //
        picLogo.Location = new Point(40, 150);
        picLogo.Name = "picLogo";
        picLogo.Size = new Size(320, 106);
        picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picLogo.TabStop = false;
        //
        // lblKhauHieu
        //
        lblKhauHieu.Font = new Font("Segoe UI Semibold", 14F);
        lblKhauHieu.ForeColor = Color.FromArgb(23, 54, 111);
        lblKhauHieu.Location = new Point(40, 280);
        lblKhauHieu.Name = "lblKhauHieu";
        lblKhauHieu.Size = new Size(330, 60);
        lblKhauHieu.Text = "Phần mềm quản lý phòng khám";
        //
        // lblGioiThieu
        //
        lblGioiThieu.ForeColor = Color.FromArgb(107, 118, 134);
        lblGioiThieu.Location = new Point(40, 340);
        lblGioiThieu.Name = "lblGioiThieu";
        lblGioiThieu.Size = new Size(330, 80);
        lblGioiThieu.Text = "Lịch trực, lịch hẹn, tiếp nhận, khám bệnh, kê đơn và thanh toán trên cùng một cơ sở dữ liệu.";
        //
        // pnlPhai
        //
        pnlPhai.BackColor = Color.White;
        pnlPhai.Controls.Add(lblGoiY);
        pnlPhai.Controls.Add(lblLoi);
        pnlPhai.Controls.Add(btnDangNhap);
        pnlPhai.Controls.Add(chkHienMatKhau);
        pnlPhai.Controls.Add(txtMatKhau);
        pnlPhai.Controls.Add(lblMatKhau);
        pnlPhai.Controls.Add(txtSdt);
        pnlPhai.Controls.Add(lblSdt);
        pnlPhai.Controls.Add(lblMoTa);
        pnlPhai.Controls.Add(lblTieuDe);
        pnlPhai.Dock = DockStyle.Fill;
        pnlPhai.Margin = new Padding(0);
        pnlPhai.Name = "pnlPhai";
        //
        // lblTieuDe
        //
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = new Font("Segoe UI Semibold", 20F);
        lblTieuDe.ForeColor = Color.FromArgb(23, 54, 111);
        lblTieuDe.Location = new Point(70, 70);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Text = "Đăng nhập";
        //
        // lblMoTa
        //
        lblMoTa.AutoSize = true;
        lblMoTa.ForeColor = Color.FromArgb(107, 118, 134);
        lblMoTa.Location = new Point(72, 120);
        lblMoTa.Name = "lblMoTa";
        lblMoTa.Text = "Dành cho nhân viên phòng khám KMN Clinic";
        //
        // lblSdt
        //
        lblSdt.AutoSize = true;
        lblSdt.Font = new Font("Segoe UI Semibold", 10F);
        lblSdt.ForeColor = Color.FromArgb(43, 52, 69);
        lblSdt.Location = new Point(72, 170);
        lblSdt.Name = "lblSdt";
        lblSdt.Text = "Số điện thoại";
        //
        // txtSdt
        //
        txtSdt.Font = new Font("Segoe UI", 11F);
        txtSdt.Location = new Point(72, 196);
        txtSdt.MaxLength = 10;
        txtSdt.Name = "txtSdt";
        txtSdt.PlaceholderText = "VD: 0932145678";
        txtSdt.Size = new Size(370, 32);
        txtSdt.TabIndex = 0;
        //
        // lblMatKhau
        //
        lblMatKhau.AutoSize = true;
        lblMatKhau.Font = new Font("Segoe UI Semibold", 10F);
        lblMatKhau.ForeColor = Color.FromArgb(43, 52, 69);
        lblMatKhau.Location = new Point(72, 244);
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.Text = "Mật khẩu";
        //
        // txtMatKhau
        //
        txtMatKhau.Font = new Font("Segoe UI", 11F);
        txtMatKhau.Location = new Point(72, 270);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.PlaceholderText = "Tối thiểu 8 ký tự";
        txtMatKhau.Size = new Size(370, 32);
        txtMatKhau.TabIndex = 1;
        txtMatKhau.UseSystemPasswordChar = true;
        //
        // chkHienMatKhau
        //
        chkHienMatKhau.AutoSize = true;
        chkHienMatKhau.ForeColor = Color.FromArgb(107, 118, 134);
        chkHienMatKhau.Location = new Point(72, 312);
        chkHienMatKhau.Name = "chkHienMatKhau";
        chkHienMatKhau.TabIndex = 2;
        chkHienMatKhau.Text = "Hiện mật khẩu";
        chkHienMatKhau.CheckedChanged += chkHienMatKhau_CheckedChanged;
        //
        // btnDangNhap
        //
        btnDangNhap.Location = new Point(72, 352);
        btnDangNhap.Name = "btnDangNhap";
        btnDangNhap.Size = new Size(370, 44);
        btnDangNhap.TabIndex = 3;
        btnDangNhap.Text = "Đăng nhập";
        btnDangNhap.Click += btnDangNhap_Click;
        //
        // lblLoi
        //
        lblLoi.ForeColor = Color.FromArgb(229, 83, 75);
        lblLoi.Location = new Point(72, 404);
        lblLoi.Name = "lblLoi";
        lblLoi.Size = new Size(370, 24);
        //
        // lblGoiY
        //
        lblGoiY.BackColor = Color.FromArgb(245, 249, 254);
        lblGoiY.Font = new Font("Segoe UI", 9F);
        lblGoiY.ForeColor = Color.FromArgb(107, 118, 134);
        lblGoiY.Location = new Point(72, 440);
        lblGoiY.Name = "lblGoiY";
        lblGoiY.Padding = new Padding(8);
        lblGoiY.Size = new Size(370, 88);
        lblGoiY.Text = "Bản khung giao diện (dữ liệu giả), mật khẩu matkhau123:\r\nQuản trị 0907112233 · Lễ tân 0932145678\r\nBác sĩ 0903126789 · Thu ngân 0708671234";
        //
        // frmDangNhap
        //
        AcceptButton = btnDangNhap;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        ClientSize = new Size(920, 560);
        Controls.Add(tlpNen);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "frmDangNhap";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "KMN Clinic – Đăng nhập";
        tlpNen.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
        pnlPhai.ResumeLayout(false);
        pnlPhai.PerformLayout();
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpNen;
    private Panel pnlTrai;
    private PictureBox picLogo;
    private Label lblKhauHieu;
    private Label lblGioiThieu;
    private Panel pnlPhai;
    private Label lblTieuDe;
    private Label lblMoTa;
    private Label lblSdt;
    private TextBox txtSdt;
    private Label lblMatKhau;
    private TextBox txtMatKhau;
    private CheckBox chkHienMatKhau;
    private Button btnDangNhap;
    private Label lblLoi;
    private Label lblGoiY;
}
