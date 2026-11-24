namespace KmnClinic.GUI.Views;

partial class ucHoaDonChoThu
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
        theChoThu = new KmnClinic.GUI.Controls.TheThongKe();
        theTienCho = new KmnClinic.GUI.Controls.TheThongKe();
        theDaThu = new KmnClinic.GUI.Controls.TheThongKe();
        theCuaToi = new KmnClinic.GUI.Controls.TheThongKe();
        khung = new Panel();
        dgvHoaDon = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colHoSo = new DataGridViewTextBoxColumn();
        colNgay = new DataGridViewTextBoxColumn();
        colKhach = new DataGridViewTextBoxColumn();
        colBacSi = new DataGridViewTextBoxColumn();
        colKhoa = new DataGridViewTextBoxColumn();
        colSoThuoc = new DataGridViewTextBoxColumn();
        colTong = new DataGridViewTextBoxColumn();
        colTrangThai = new DataGridViewTextBoxColumn();
        pnlDau = new Panel();
        txtTim = new TextBox();
        btnThu = new Button();
        btnLamMoi = new Button();
        pnlKhoang = new Panel();
        flpThe.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
        pnlDau.SuspendLayout();
        SuspendLayout();
        //
        // flpThe
        //
        flpThe.Controls.Add(theChoThu);
        flpThe.Controls.Add(theTienCho);
        flpThe.Controls.Add(theDaThu);
        flpThe.Controls.Add(theCuaToi);
        flpThe.Dock = DockStyle.Top;
        flpThe.Name = "flpThe";
        flpThe.Size = new Size(1000, 90);
        flpThe.Padding = new Padding(0, 0, 0, 8);
        flpThe.WrapContents = false;
        theChoThu.Name = "theChoThu"; theChoThu.TieuDe = "Hóa đơn chờ thu";         theTienCho.Name = "theTienCho"; theTienCho.TieuDe = "Tổng tiền chờ thu";         theDaThu.Name = "theDaThu"; theDaThu.TieuDe = "Doanh thu hôm nay";
        theCuaToi.Name = "theCuaToi"; theCuaToi.TieuDe = "Tôi đã thu hôm nay";
                //
        // khung
        //
        khung.Controls.Add(dgvHoaDon);
        khung.Controls.Add(pnlDau);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        // nút dock phải (thêm sau dock trước): Thu tiền sát phải, rồi Làm mới
        pnlDau.Controls.Add(txtTim);
        pnlDau.Controls.Add(btnLamMoi);
        pnlDau.Controls.Add(pnlKhoang);
        pnlDau.Controls.Add(btnThu);
        pnlDau.Dock = DockStyle.Top; pnlDau.Name = "pnlDau"; pnlDau.Size = new Size(900, 52); pnlDau.Padding = new Padding(0, 0, 0, 20);
        pnlKhoang.Dock = DockStyle.Right; pnlKhoang.Name = "pnlKhoang"; pnlKhoang.Size = new Size(8, 32);
        btnLamMoi.Dock = DockStyle.Right; btnLamMoi.Name = "btnLamMoi"; btnLamMoi.Size = new Size(110, 32); btnLamMoi.Text = "Làm mới"; btnLamMoi.Click += btnLamMoi_Click;
        txtTim.Location = new Point(0, 4); txtTim.Name = "txtTim"; txtTim.PlaceholderText = "Tìm mã hóa đơn, tên hoặc SĐT khách..."; txtTim.Size = new Size(320, 30);
        txtTim.TextChanged += txtTim_TextChanged;
        btnThu.Dock = DockStyle.Right; btnThu.Name = "btnThu"; btnThu.Size = new Size(160, 30);
        btnThu.Text = "Thu tiền"; btnThu.Click += btnThu_Click;
        dgvHoaDon.Columns.AddRange(new DataGridViewColumn[] { colMa, colHoSo, colNgay, colKhach, colBacSi, colKhoa, colSoThuoc, colTong, colTrangThai });
        dgvHoaDon.Dock = DockStyle.Fill; dgvHoaDon.Name = "dgvHoaDon";
        dgvHoaDon.CellDoubleClick += dgvHoaDon_CellDoubleClick;
        colMa.DataPropertyName = "MaHoaDon"; colMa.HeaderText = "Mã hóa đơn"; colMa.Name = "colMa"; colMa.FillWeight = 75;
        colHoSo.DataPropertyName = "MaHoSo"; colHoSo.HeaderText = "Hồ sơ"; colHoSo.Name = "colHoSo"; colHoSo.FillWeight = 65;
        colKhoa.DataPropertyName = "TenKhoa"; colKhoa.HeaderText = "Khoa"; colKhoa.Name = "colKhoa"; colKhoa.FillWeight = 90;
        colSoThuoc.DataPropertyName = "SoThuoc"; colSoThuoc.HeaderText = "Số thuốc"; colSoThuoc.Name = "colSoThuoc"; colSoThuoc.FillWeight = 50;
        colNgay.DataPropertyName = "NgayKham"; colNgay.HeaderText = "Ngày khám"; colNgay.Name = "colNgay"; colNgay.FillWeight = 95; colNgay.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        colKhach.DataPropertyName = "TenKhachHang"; colKhach.HeaderText = "Khách hàng"; colKhach.Name = "colKhach"; colKhach.FillWeight = 130;
        colBacSi.DataPropertyName = "TenBacSi"; colBacSi.HeaderText = "Bác sĩ"; colBacSi.Name = "colBacSi"; colBacSi.FillWeight = 130;
        colTong.DataPropertyName = "TongTien"; colTong.HeaderText = "Tổng tiền"; colTong.Name = "colTong"; colTong.FillWeight = 85; colTong.DefaultCellStyle.Format = "#,0 đ";
        colTong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colTrangThai.DataPropertyName = "TrangThaiThanhToan"; colTrangThai.HeaderText = "Trạng thái"; colTrangThai.Name = "colTrangThai"; colTrangThai.FillWeight = 110;
        //
        // ucHoaDonChoThu
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(khung);
        Controls.Add(flpThe);
        Name = "ucHoaDonChoThu";
        Size = new Size(1000, 600);
        flpThe.ResumeLayout(false);
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
        pnlDau.ResumeLayout(false);
        pnlDau.PerformLayout();
        ResumeLayout(false);
    }

    private FlowLayoutPanel flpThe;
    private KmnClinic.GUI.Controls.TheThongKe theChoThu;
    private KmnClinic.GUI.Controls.TheThongKe theTienCho;
    private KmnClinic.GUI.Controls.TheThongKe theDaThu;
    private KmnClinic.GUI.Controls.TheThongKe theCuaToi;
    private Panel khung;
    private DataGridView dgvHoaDon;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colHoSo;
    private DataGridViewTextBoxColumn colNgay;
    private DataGridViewTextBoxColumn colKhach;
    private DataGridViewTextBoxColumn colBacSi;
    private DataGridViewTextBoxColumn colKhoa;
    private DataGridViewTextBoxColumn colSoThuoc;
    private DataGridViewTextBoxColumn colTong;
    private DataGridViewTextBoxColumn colTrangThai;
    private Panel pnlDau;
    private TextBox txtTim;
    private Button btnThu;
    private Button btnLamMoi;
    private Panel pnlKhoang;
}
