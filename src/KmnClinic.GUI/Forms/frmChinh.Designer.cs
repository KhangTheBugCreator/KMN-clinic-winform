namespace KmnClinic.GUI.Forms;

partial class frmChinh
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlMenu = new Panel();
        flpMenu = new FlowLayoutPanel();
        pnlLogo = new Panel();
        picLogo = new PictureBox();
        pnlDau = new Panel();
        btnDangXuat = new Button();
        lblVaiTro = new Label();
        lblNguoiDung = new Label();
        lblTieuDe = new Label();
        pnlNoiDung = new Panel();
        pnlMenu.SuspendLayout();
        pnlLogo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picLogo).BeginInit();
        pnlDau.SuspendLayout();
        SuspendLayout();
        //
        // pnlMenu
        //
        pnlMenu.BackColor = Color.FromArgb(23, 54, 111);
        pnlMenu.Controls.Add(flpMenu);
        pnlMenu.Controls.Add(pnlLogo);
        pnlMenu.Dock = DockStyle.Left;
        pnlMenu.Name = "pnlMenu";
        pnlMenu.Size = new Size(250, 760);
        //
        // pnlLogo
        //
        pnlLogo.BackColor = Color.White;
        pnlLogo.Controls.Add(picLogo);
        pnlLogo.Dock = DockStyle.Top;
        pnlLogo.Name = "pnlLogo";
        pnlLogo.Padding = new Padding(24, 10, 24, 10);
        pnlLogo.Size = new Size(250, 72);
        //
        // picLogo
        //
        picLogo.Dock = DockStyle.Fill;
        picLogo.Name = "picLogo";
        picLogo.SizeMode = PictureBoxSizeMode.Zoom;
        picLogo.TabStop = false;
        //
        // flpMenu
        //
        flpMenu.AutoScroll = true;
        flpMenu.Dock = DockStyle.Fill;
        flpMenu.FlowDirection = FlowDirection.TopDown;
        flpMenu.Name = "flpMenu";
        flpMenu.Padding = new Padding(12, 12, 0, 12);
        flpMenu.WrapContents = false;
        //
        // pnlDau
        //
        pnlDau.BackColor = Color.White;
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(lblNguoiDung);
        pnlDau.Controls.Add(lblVaiTro);
        pnlDau.Controls.Add(btnDangXuat);
        pnlDau.Dock = DockStyle.Top;
        pnlDau.Name = "pnlDau";
        pnlDau.Size = new Size(1030, 72);
        //
        // lblTieuDe
        //
        lblTieuDe.AutoSize = true;
        lblTieuDe.Font = new Font("Segoe UI Semibold", 15F);
        lblTieuDe.ForeColor = Color.FromArgb(23, 54, 111);
        lblTieuDe.Location = new Point(28, 20);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Text = "Tổng quan";
        //
        // lblNguoiDung
        //
        lblNguoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblNguoiDung.Font = new Font("Segoe UI Semibold", 10F);
        lblNguoiDung.ForeColor = Color.FromArgb(43, 52, 69);
        lblNguoiDung.Location = new Point(600, 14);
        lblNguoiDung.Name = "lblNguoiDung";
        lblNguoiDung.Size = new Size(280, 22);
        lblNguoiDung.Text = "Họ tên nhân viên";
        lblNguoiDung.TextAlign = ContentAlignment.MiddleRight;
        //
        // lblVaiTro
        //
        lblVaiTro.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblVaiTro.Font = new Font("Segoe UI", 9F);
        lblVaiTro.ForeColor = Color.FromArgb(107, 118, 134);
        lblVaiTro.Location = new Point(600, 36);
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.Size = new Size(280, 20);
        lblVaiTro.Text = "Vai trò";
        lblVaiTro.TextAlign = ContentAlignment.MiddleRight;
        //
        // btnDangXuat
        //
        btnDangXuat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnDangXuat.Location = new Point(896, 18);
        btnDangXuat.Name = "btnDangXuat";
        btnDangXuat.Size = new Size(110, 36);
        btnDangXuat.Text = "Đăng xuất";
        btnDangXuat.Click += btnDangXuat_Click;
        //
        // pnlNoiDung
        //
        pnlNoiDung.BackColor = Color.FromArgb(245, 249, 254);
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(24);
        //
        // frmChinh
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1280, 760);
        Controls.Add(pnlNoiDung);
        Controls.Add(pnlDau);
        Controls.Add(pnlMenu);
        MinimumSize = new Size(1100, 680);
        Name = "frmChinh";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "KMN Clinic";
        WindowState = FormWindowState.Maximized;
        pnlMenu.ResumeLayout(false);
        pnlLogo.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picLogo).EndInit();
        pnlDau.ResumeLayout(false);
        pnlDau.PerformLayout();
        ResumeLayout(false);
    }

    private Panel pnlMenu;
    private Panel pnlLogo;
    private PictureBox picLogo;
    private FlowLayoutPanel flpMenu;
    private Panel pnlDau;
    private Label lblTieuDe;
    private Label lblNguoiDung;
    private Label lblVaiTro;
    private Button btnDangXuat;
    private Panel pnlNoiDung;
}
