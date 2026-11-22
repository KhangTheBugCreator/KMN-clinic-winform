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
        menuChinh = new MenuStrip();
        mnuHeThong = new ToolStripMenuItem();
        mnuDoiMatKhau = new ToolStripMenuItem();
        mnuDangXuat = new ToolStripMenuItem();
        mnuThoat = new ToolStripMenuItem();
        thanhTrangThai = new StatusStrip();
        lblNguoiDung = new ToolStripStatusLabel();
        lblVaiTro = new ToolStripStatusLabel();
        lblNgay = new ToolStripStatusLabel();
        lblTieuDe = new Label();
        pnlNoiDung = new Panel();
        menuChinh.SuspendLayout();
        thanhTrangThai.SuspendLayout();
        SuspendLayout();
        //
        // menuChinh
        //
        menuChinh.Items.AddRange(new ToolStripItem[] { mnuHeThong });
        menuChinh.Location = new Point(0, 0);
        menuChinh.Name = "menuChinh";
        menuChinh.Size = new Size(1100, 24);
        //
        // mnuHeThong
        //
        mnuHeThong.DropDownItems.AddRange(new ToolStripItem[] { mnuDoiMatKhau, mnuDangXuat, mnuThoat });
        mnuHeThong.Name = "mnuHeThong";
        mnuHeThong.Text = "&Hệ thống";
        mnuDoiMatKhau.Name = "mnuDoiMatKhau";
        mnuDoiMatKhau.Text = "Đổi mật khẩu";
        mnuDoiMatKhau.Click += mnuDoiMatKhau_Click;
        mnuDangXuat.Name = "mnuDangXuat";
        mnuDangXuat.Text = "Đăng xuất";
        mnuDangXuat.Click += mnuDangXuat_Click;
        mnuThoat.Name = "mnuThoat";
        mnuThoat.Text = "Thoát";
        mnuThoat.Click += mnuThoat_Click;
        //
        // thanhTrangThai
        //
        thanhTrangThai.Items.AddRange(new ToolStripItem[] { lblNguoiDung, lblVaiTro, lblNgay });
        thanhTrangThai.Location = new Point(0, 678);
        thanhTrangThai.Name = "thanhTrangThai";
        thanhTrangThai.Size = new Size(1100, 22);
        lblNguoiDung.Name = "lblNguoiDung";
        lblNguoiDung.BorderSides = ToolStripStatusLabelBorderSides.Right;
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.BorderSides = ToolStripStatusLabelBorderSides.Right;
        lblNgay.Name = "lblNgay";
        //
        // lblTieuDe
        //
        lblTieuDe.Dock = DockStyle.Top;
        lblTieuDe.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Padding = new Padding(6, 0, 0, 0);
        lblTieuDe.Size = new Size(1100, 32);
        lblTieuDe.TextAlign = ContentAlignment.MiddleLeft;
        //
        // pnlNoiDung
        //
        pnlNoiDung.Dock = DockStyle.Fill;
        pnlNoiDung.Name = "pnlNoiDung";
        pnlNoiDung.Padding = new Padding(6, 0, 6, 6);
        //
        // frmChinh
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1100, 700);
        Controls.Add(pnlNoiDung);
        Controls.Add(lblTieuDe);
        Controls.Add(thanhTrangThai);
        Controls.Add(menuChinh);
        MainMenuStrip = menuChinh;
        MinimumSize = new Size(1000, 640);
        Name = "frmChinh";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "KMN Clinic";
        WindowState = FormWindowState.Maximized;
        menuChinh.ResumeLayout(false);
        menuChinh.PerformLayout();
        thanhTrangThai.ResumeLayout(false);
        thanhTrangThai.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private MenuStrip menuChinh;
    private ToolStripMenuItem mnuHeThong;
    private ToolStripMenuItem mnuDoiMatKhau;
    private ToolStripMenuItem mnuDangXuat;
    private ToolStripMenuItem mnuThoat;
    private StatusStrip thanhTrangThai;
    private ToolStripStatusLabel lblNguoiDung;
    private ToolStripStatusLabel lblVaiTro;
    private ToolStripStatusLabel lblNgay;
    private Label lblTieuDe;
    private Panel pnlNoiDung;
}
