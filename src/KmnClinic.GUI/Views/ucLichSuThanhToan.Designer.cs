namespace KmnClinic.GUI.Views;

partial class ucLichSuThanhToan
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
        lblTu = new Label();
        dtpTu = new DateTimePicker();
        lblDen = new Label();
        dtpDen = new DateTimePicker();
        chkMoiNgay = new CheckBox();
        chkCuaToi = new CheckBox();
        txtTim = new TextBox();
        btnXem = new Button();
        khung = new Panel();
        dgvHoaDon = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colNgay = new DataGridViewTextBoxColumn();
        colKhach = new DataGridViewTextBoxColumn();
        colHoSo = new DataGridViewTextBoxColumn();
        colSdt = new DataGridViewTextBoxColumn();
        colBacSi = new DataGridViewTextBoxColumn();
        colTongTien = new DataGridViewTextBoxColumn();
        colThuNgan = new DataGridViewTextBoxColumn();
        lblDem = new Label();
        pnlCongCu.SuspendLayout();
        flpLoc.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).BeginInit();
        SuspendLayout();
        //
        // pnlCongCu
        //
        pnlCongCu.Controls.Add(flpLoc);
        pnlCongCu.Dock = DockStyle.Top;
        pnlCongCu.Name = "pnlCongCu";
        pnlCongCu.Padding = new Padding(0, 0, 0, 12);
        pnlCongCu.Size = new Size(1100, 56);
        //
        // flpLoc
        //
        flpLoc.Controls.Add(lblTu);
        flpLoc.Controls.Add(dtpTu);
        flpLoc.Controls.Add(lblDen);
        flpLoc.Controls.Add(dtpDen);
        flpLoc.Controls.Add(chkMoiNgay);
        flpLoc.Controls.Add(chkCuaToi);
        flpLoc.Controls.Add(txtTim);
        flpLoc.Controls.Add(btnXem);
        flpLoc.Dock = DockStyle.Fill;
        flpLoc.Name = "flpLoc";
        flpLoc.WrapContents = false;
        lblTu.AutoSize = true; lblTu.Margin = new Padding(0, 8, 4, 0); lblTu.Name = "lblTu"; lblTu.Text = "Từ";
        dtpTu.Format = DateTimePickerFormat.Custom; dtpTu.CustomFormat = "dd/MM/yyyy"; dtpTu.Margin = new Padding(0, 4, 8, 0);
        dtpTu.Name = "dtpTu"; dtpTu.Size = new Size(140, 30); dtpTu.ValueChanged += BoLoc_Changed;
        lblDen.AutoSize = true; lblDen.Margin = new Padding(0, 8, 4, 0); lblDen.Name = "lblDen"; lblDen.Text = "đến";
        dtpDen.Format = DateTimePickerFormat.Custom; dtpDen.CustomFormat = "dd/MM/yyyy"; dtpDen.Margin = new Padding(0, 4, 8, 0);
        dtpDen.Name = "dtpDen"; dtpDen.Size = new Size(140, 30); dtpDen.ValueChanged += BoLoc_Changed;
        chkMoiNgay.AutoSize = true; chkMoiNgay.Margin = new Padding(0, 8, 16, 0); chkMoiNgay.Name = "chkMoiNgay"; chkMoiNgay.Text = "Mọi ngày";
        chkMoiNgay.CheckedChanged += BoLoc_Changed;
        chkCuaToi.AutoSize = true; chkCuaToi.Margin = new Padding(0, 8, 16, 0); chkCuaToi.Name = "chkCuaToi"; chkCuaToi.Text = "Chỉ hóa đơn tôi thu";
        chkCuaToi.CheckedChanged += BoLoc_Changed;
        txtTim.Margin = new Padding(0, 4, 0, 0); txtTim.Name = "txtTim"; txtTim.PlaceholderText = "Mã hóa đơn, hồ sơ, tên, SĐT..."; txtTim.Size = new Size(240, 30);
        txtTim.TextChanged += BoLoc_Changed;
        btnXem.Margin = new Padding(8, 2, 0, 0); btnXem.Name = "btnXem"; btnXem.Size = new Size(120, 30); btnXem.Text = "Xem / in lại"; btnXem.Click += btnXem_Click;
        //
        // khung
        //
        khung.Controls.Add(dgvHoaDon);
        khung.Controls.Add(lblDem);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        dgvHoaDon.Columns.AddRange(new DataGridViewColumn[] { colMa, colNgay, colHoSo, colKhach, colSdt, colBacSi, colTongTien, colThuNgan });
        dgvHoaDon.Dock = DockStyle.Fill;
        dgvHoaDon.Name = "dgvHoaDon";
        dgvHoaDon.CellDoubleClick += dgvHoaDon_CellDoubleClick;
        colMa.DataPropertyName = "MaHoaDon"; colMa.HeaderText = "Mã hóa đơn"; colMa.Name = "colMa"; colMa.FillWeight = 70;
        colNgay.DataPropertyName = "NgayGioThanhToan"; colNgay.HeaderText = "Thu lúc"; colNgay.Name = "colNgay"; colNgay.FillWeight = 90;
        colNgay.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        colHoSo.DataPropertyName = "MaHoSo"; colHoSo.HeaderText = "Hồ sơ"; colHoSo.Name = "colHoSo"; colHoSo.FillWeight = 60;
        colKhach.DataPropertyName = "TenKhachHang"; colKhach.HeaderText = "Khách hàng"; colKhach.Name = "colKhach"; colKhach.FillWeight = 120;
        colSdt.DataPropertyName = "SdtKhachHang"; colSdt.HeaderText = "SĐT"; colSdt.Name = "colSdt"; colSdt.FillWeight = 75;
        colBacSi.DataPropertyName = "TenBacSi"; colBacSi.HeaderText = "Bác sĩ"; colBacSi.Name = "colBacSi"; colBacSi.FillWeight = 110;
        colTongTien.DataPropertyName = "TongTien"; colTongTien.HeaderText = "Tổng tiền"; colTongTien.Name = "colTongTien"; colTongTien.FillWeight = 75;
        colTongTien.DefaultCellStyle.Format = "#,0"; colTongTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colThuNgan.DataPropertyName = "ThuNgan"; colThuNgan.HeaderText = "Thu ngân"; colThuNgan.Name = "colThuNgan"; colThuNgan.FillWeight = 110;
        lblDem.Dock = DockStyle.Bottom; lblDem.Name = "lblDem";
        lblDem.Size = new Size(900, 30); lblDem.TextAlign = ContentAlignment.BottomLeft;
        //
        // ucLichSuThanhToan
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(khung);
        Controls.Add(pnlCongCu);
        Name = "ucLichSuThanhToan";
        Size = new Size(1100, 600);
        pnlCongCu.ResumeLayout(false);
        flpLoc.ResumeLayout(false);
        flpLoc.PerformLayout();
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHoaDon).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlCongCu;
    private FlowLayoutPanel flpLoc;
    private Label lblTu;
    private DateTimePicker dtpTu;
    private Label lblDen;
    private DateTimePicker dtpDen;
    private CheckBox chkMoiNgay;
    private CheckBox chkCuaToi;
    private TextBox txtTim;
    private Button btnXem;
    private Panel khung;
    private DataGridView dgvHoaDon;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colNgay;
    private DataGridViewTextBoxColumn colKhach;
    private DataGridViewTextBoxColumn colHoSo;
    private DataGridViewTextBoxColumn colSdt;
    private DataGridViewTextBoxColumn colBacSi;
    private DataGridViewTextBoxColumn colTongTien;
    private DataGridViewTextBoxColumn colThuNgan;
    private Label lblDem;
}
