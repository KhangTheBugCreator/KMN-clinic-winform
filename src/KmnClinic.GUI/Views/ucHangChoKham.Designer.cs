namespace KmnClinic.GUI.Views;

partial class ucHangChoKham
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        flpThe = new FlowLayoutPanel();
        theCho = new KmnClinic.GUI.Controls.TheThongKe();
        theDang = new KmnClinic.GUI.Controls.TheThongKe();
        theXong = new KmnClinic.GUI.Controls.TheThongKe();
        khung = new KmnClinic.GUI.Controls.KhungTrang();
        dgvHangCho = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colGio = new DataGridViewTextBoxColumn();
        colKhach = new DataGridViewTextBoxColumn();
        colTuoi = new DataGridViewTextBoxColumn();
        colNguon = new DataGridViewTextBoxColumn();
        colTrieuChung = new DataGridViewTextBoxColumn();
        colTrangThai = new DataGridViewTextBoxColumn();
        pnlDau = new Panel();
        btnKham = new Button();
        lblTieuDe = new Label();
        flpThe.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHangCho).BeginInit();
        pnlDau.SuspendLayout();
        SuspendLayout();
        //
        // flpThe
        //
        flpThe.Controls.Add(theCho);
        flpThe.Controls.Add(theDang);
        flpThe.Controls.Add(theXong);
        flpThe.Dock = DockStyle.Top;
        flpThe.Name = "flpThe";
        flpThe.Size = new Size(1000, 128);
        flpThe.WrapContents = false;
        theCho.Name = "theCho"; theCho.TieuDe = "Đang chờ khám"; theCho.MauNhan = Color.FromArgb(242, 163, 58);
        theDang.Name = "theDang"; theDang.TieuDe = "Đang khám"; theDang.MauNhan = Color.FromArgb(31, 111, 209);
        theXong.Name = "theXong"; theXong.TieuDe = "Đã hoàn tất hôm nay"; theXong.MauNhan = Color.FromArgb(76, 175, 106);
        //
        // khung
        //
        khung.Controls.Add(dgvHangCho);
        khung.Controls.Add(pnlDau);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        pnlDau.Controls.Add(lblTieuDe);
        pnlDau.Controls.Add(btnKham);
        pnlDau.Dock = DockStyle.Top; pnlDau.Name = "pnlDau"; pnlDau.Size = new Size(900, 52);
        lblTieuDe.AutoSize = true; lblTieuDe.Font = new Font("Segoe UI Semibold", 11.5F); lblTieuDe.ForeColor = Color.FromArgb(23, 54, 111);
        lblTieuDe.Location = new Point(0, 6); lblTieuDe.Name = "lblTieuDe"; lblTieuDe.Text = "Hàng chờ hôm nay (theo thứ tự tiếp nhận)";
        btnKham.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnKham.Location = new Point(740, 0); btnKham.Name = "btnKham"; btnKham.Size = new Size(160, 38);
        btnKham.Text = "Khám bệnh"; btnKham.Click += btnKham_Click;
        dgvHangCho.Columns.AddRange(new DataGridViewColumn[] { colMa, colGio, colKhach, colTuoi, colNguon, colTrieuChung, colTrangThai });
        dgvHangCho.Dock = DockStyle.Fill; dgvHangCho.Name = "dgvHangCho";
        dgvHangCho.CellDoubleClick += dgvHangCho_CellDoubleClick;
        dgvHangCho.SelectionChanged += dgvHangCho_SelectionChanged;
        colMa.DataPropertyName = "MaHoSo"; colMa.HeaderText = "Mã hồ sơ"; colMa.Name = "colMa"; colMa.FillWeight = 70;
        colGio.DataPropertyName = "NgayGioKham"; colGio.HeaderText = "Tiếp nhận"; colGio.Name = "colGio"; colGio.FillWeight = 60; colGio.DefaultCellStyle.Format = "HH:mm";
        colKhach.DataPropertyName = "TenKhachHang"; colKhach.HeaderText = "Bệnh nhân"; colKhach.Name = "colKhach"; colKhach.FillWeight = 130;
        colTuoi.DataPropertyName = "Tuoi"; colTuoi.HeaderText = "Tuổi"; colTuoi.Name = "colTuoi"; colTuoi.FillWeight = 45;
        colNguon.DataPropertyName = "Nguon"; colNguon.HeaderText = "Nguồn"; colNguon.Name = "colNguon"; colNguon.FillWeight = 60;
        colTrieuChung.DataPropertyName = "TrieuChung"; colTrieuChung.HeaderText = "Triệu chứng"; colTrieuChung.Name = "colTrieuChung"; colTrieuChung.FillWeight = 160;
        colTrangThai.DataPropertyName = "TrangThai"; colTrangThai.HeaderText = "Trạng thái"; colTrangThai.Name = "colTrangThai"; colTrangThai.FillWeight = 85;
        //
        // ucHangChoKham
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(245, 249, 254);
        Controls.Add(khung);
        Controls.Add(flpThe);
        Name = "ucHangChoKham";
        Size = new Size(1000, 600);
        flpThe.ResumeLayout(false);
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHangCho).EndInit();
        pnlDau.ResumeLayout(false);
        pnlDau.PerformLayout();
        ResumeLayout(false);
    }

    private FlowLayoutPanel flpThe;
    private KmnClinic.GUI.Controls.TheThongKe theCho;
    private KmnClinic.GUI.Controls.TheThongKe theDang;
    private KmnClinic.GUI.Controls.TheThongKe theXong;
    private KmnClinic.GUI.Controls.KhungTrang khung;
    private DataGridView dgvHangCho;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colGio;
    private DataGridViewTextBoxColumn colKhach;
    private DataGridViewTextBoxColumn colTuoi;
    private DataGridViewTextBoxColumn colNguon;
    private DataGridViewTextBoxColumn colTrieuChung;
    private DataGridViewTextBoxColumn colTrangThai;
    private Panel pnlDau;
    private Button btnKham;
    private Label lblTieuDe;
}
