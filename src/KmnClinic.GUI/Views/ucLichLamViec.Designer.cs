namespace KmnClinic.GUI.Views;

partial class ucLichLamViec
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        pnlCongCu = new Panel();
        flpLoc = new FlowLayoutPanel();
        btnTruoc = new Button();
        lblTuan = new Label();
        btnSau = new Button();
        btnHomNay = new Button();
        cboKhoa = new ComboBox();
        btnXepLich = new Button();
        btnSua = new Button();
        btnXoa = new Button();
        khung = new Panel();
        dgvLich = new DataGridView();
        lblGhiChu = new Label();
        pnlCongCu.SuspendLayout();
        flpLoc.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLich).BeginInit();
        SuspendLayout();
        //
        // pnlCongCu
        //
        pnlCongCu.Controls.Add(flpLoc);
        pnlCongCu.Dock = DockStyle.Top;
        pnlCongCu.Name = "pnlCongCu";
        pnlCongCu.Padding = new Padding(0, 0, 0, 12);
        pnlCongCu.Size = new Size(1000, 56);
        //
        // flpLoc
        //
        flpLoc.Controls.Add(btnTruoc);
        flpLoc.Controls.Add(lblTuan);
        flpLoc.Controls.Add(btnSau);
        flpLoc.Controls.Add(btnHomNay);
        flpLoc.Controls.Add(cboKhoa);
        flpLoc.Controls.Add(btnXepLich);
        flpLoc.Controls.Add(btnSua);
        flpLoc.Controls.Add(btnXoa);
        flpLoc.Dock = DockStyle.Fill;
        flpLoc.Name = "flpLoc";
        flpLoc.WrapContents = false;
        btnTruoc.Margin = new Padding(0); btnTruoc.Name = "btnTruoc"; btnTruoc.Size = new Size(44, 30); btnTruoc.Text = "‹"; btnTruoc.Click += btnTruoc_Click;
        lblTuan.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblTuan.Margin = new Padding(8, 0, 8, 0);
        lblTuan.Name = "lblTuan"; lblTuan.Size = new Size(230, 38); lblTuan.TextAlign = ContentAlignment.MiddleCenter;
        btnSau.Margin = new Padding(0); btnSau.Name = "btnSau"; btnSau.Size = new Size(44, 30); btnSau.Text = "›"; btnSau.Click += btnSau_Click;
        btnHomNay.Margin = new Padding(8, 0, 16, 0); btnHomNay.Name = "btnHomNay"; btnHomNay.Size = new Size(100, 30); btnHomNay.Text = "Tuần này"; btnHomNay.Click += btnHomNay_Click;
        cboKhoa.DropDownStyle = ComboBoxStyle.DropDownList; cboKhoa.Margin = new Padding(0, 4, 0, 0); cboKhoa.Name = "cboKhoa"; cboKhoa.Size = new Size(200, 30);
        cboKhoa.SelectedIndexChanged += cboKhoa_SelectedIndexChanged;
        btnXepLich.Margin = new Padding(8, 2, 0, 0); btnXepLich.Name = "btnXepLich"; btnXepLich.Size = new Size(110, 30); btnXepLich.Text = "Xếp lịch...";
        btnXepLich.Click += btnXepLich_Click;
        btnSua.Margin = new Padding(4, 2, 0, 0); btnSua.Name = "btnSua"; btnSua.Size = new Size(80, 30); btnSua.Text = "Sửa"; btnSua.Click += btnSua_Click;
        btnXoa.Margin = new Padding(4, 2, 0, 0); btnXoa.Name = "btnXoa"; btnXoa.Size = new Size(80, 30); btnXoa.Text = "Xóa"; btnXoa.Click += btnXoa_Click;
        //
        // khung
        //
        khung.Controls.Add(dgvLich);
        khung.Controls.Add(lblGhiChu);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        dgvLich.Dock = DockStyle.Fill;
        dgvLich.Name = "dgvLich";
        dgvLich.CellDoubleClick += dgvLich_CellDoubleClick;
        dgvLich.KeyDown += dgvLich_KeyDown;
        lblGhiChu.Dock = DockStyle.Bottom; lblGhiChu.Name = "lblGhiChu";
        lblGhiChu.Size = new Size(900, 30); lblGhiChu.TextAlign = ContentAlignment.BottomLeft;
        //
        // ucLichLamViec
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(khung);
        Controls.Add(pnlCongCu);
        Name = "ucLichLamViec";
        Size = new Size(1000, 600);
        pnlCongCu.ResumeLayout(false);
        flpLoc.ResumeLayout(false);
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLich).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlCongCu;
    private FlowLayoutPanel flpLoc;
    private Button btnTruoc;
    private Label lblTuan;
    private Button btnSau;
    private Button btnHomNay;
    private ComboBox cboKhoa;
    private Button btnXepLich;
    private Button btnSua;
    private Button btnXoa;
    private Panel khung;
    private DataGridView dgvLich;
    private Label lblGhiChu;
}
