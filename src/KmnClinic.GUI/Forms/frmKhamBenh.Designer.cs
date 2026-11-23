namespace KmnClinic.GUI.Forms;

partial class frmKhamBenh
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
        khungLanTruoc = new Panel();
        dgvLanTruoc = new DataGridView();
        colLtNgay = new DataGridViewTextBoxColumn();
        colLtChanDoan = new DataGridViewTextBoxColumn();
        colLtBacSi = new DataGridViewTextBoxColumn();
        lblLanTruoc = new Label();
        lblLanTruocChiTiet = new Label();
        khungBenhNhan = new Panel();
        txtTrieuChung = new TextBox();
        lblTrieuChungTieuDe = new Label();
        lblThongTin = new Label();
        lblTen = new Label();
        khungKham = new Panel();
        tlpKham = new TableLayoutPanel();
        lblChanDoan = new Label();
        txtChanDoan = new TextBox();
        lblLoiDan = new Label();
        txtLoiDan = new TextBox();
        lblTienCong = new Label();
        numTienCong = new NumericUpDown();
        lblKeThuoc = new Label();
        flpKeThuoc = new FlowLayoutPanel();
        cboThuoc = new ComboBox();
        numSoLuong = new NumericUpDown();
        txtLieuDung = new TextBox();
        btnThemThuoc = new Button();
        dgvDonThuoc = new DataGridView();
        colTenThuoc = new DataGridViewTextBoxColumn();
        colSoLuong = new DataGridViewTextBoxColumn();
        colDvt = new DataGridViewTextBoxColumn();
        colLieuDung = new DataGridViewTextBoxColumn();
        colDonGia = new DataGridViewTextBoxColumn();
        colThanhTien = new DataGridViewTextBoxColumn();
        colXoa = new DataGridViewButtonColumn();
        pnlDuoi = new Panel();
        lblTong = new Label();
        btnHoanTat = new Button();
        btnLuuNhap = new Button();
        tlpNen.SuspendLayout();
        pnlTrai.SuspendLayout();
        khungLanTruoc.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLanTruoc).BeginInit();
        khungBenhNhan.SuspendLayout();
        khungKham.SuspendLayout();
        tlpKham.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numTienCong).BeginInit();
        flpKeThuoc.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numSoLuong).BeginInit();
        ((System.ComponentModel.ISupportInitialize)dgvDonThuoc).BeginInit();
        pnlDuoi.SuspendLayout();
        SuspendLayout();
        //
        // tlpNen
        //
        tlpNen.ColumnCount = 2;
        tlpNen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 32F));
        tlpNen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 68F));
        tlpNen.Controls.Add(pnlTrai, 0, 0);
        tlpNen.Controls.Add(khungKham, 1, 0);
        tlpNen.Dock = DockStyle.Fill;
        tlpNen.Name = "tlpNen";
        tlpNen.Padding = new Padding(16);
        tlpNen.RowCount = 1;
        tlpNen.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // pnlTrai
        //
        pnlTrai.Controls.Add(khungLanTruoc);
        pnlTrai.Controls.Add(khungBenhNhan);
        pnlTrai.Dock = DockStyle.Fill;
        pnlTrai.Margin = new Padding(0, 0, 8, 0);
        pnlTrai.Name = "pnlTrai";
        //
        // khungBenhNhan
        //
        khungBenhNhan.Controls.Add(txtTrieuChung);
        khungBenhNhan.Controls.Add(lblTrieuChungTieuDe);
        khungBenhNhan.Controls.Add(lblThongTin);
        khungBenhNhan.Controls.Add(lblTen);
        khungBenhNhan.Dock = DockStyle.Top;
        khungBenhNhan.Name = "khungBenhNhan";
        khungBenhNhan.Size = new Size(340, 230);
        lblTen.Dock = DockStyle.Top; lblTen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblTen.Name = "lblTen"; lblTen.Size = new Size(300, 36);
        lblThongTin.Dock = DockStyle.Top; lblThongTin.Name = "lblThongTin"; lblThongTin.Size = new Size(300, 66);
        lblTrieuChungTieuDe.Dock = DockStyle.Top; lblTrieuChungTieuDe.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblTrieuChungTieuDe.Name = "lblTrieuChungTieuDe"; lblTrieuChungTieuDe.Size = new Size(300, 26); lblTrieuChungTieuDe.Text = "Triệu chứng *";
        txtTrieuChung.Dock = DockStyle.Fill; txtTrieuChung.MaxLength = 255; txtTrieuChung.Multiline = true; txtTrieuChung.Name = "txtTrieuChung";
        txtTrieuChung.ScrollBars = ScrollBars.Vertical; txtTrieuChung.TextChanged += ODuLieu_Changed;
        //
        // khungLanTruoc
        //
        khungLanTruoc.Controls.Add(dgvLanTruoc);
        khungLanTruoc.Controls.Add(lblLanTruocChiTiet);
        khungLanTruoc.Controls.Add(lblLanTruoc);
        khungLanTruoc.Dock = DockStyle.Fill;
        khungLanTruoc.Name = "khungLanTruoc";
        lblLanTruoc.Dock = DockStyle.Top; lblLanTruoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblLanTruoc.Name = "lblLanTruoc"; lblLanTruoc.Padding = new Padding(0, 12, 0, 0); lblLanTruoc.Size = new Size(300, 44); lblLanTruoc.Text = "Các lần khám trước";
        dgvLanTruoc.Columns.AddRange(new DataGridViewColumn[] { colLtNgay, colLtChanDoan, colLtBacSi });
        dgvLanTruoc.Dock = DockStyle.Fill; dgvLanTruoc.Name = "dgvLanTruoc"; dgvLanTruoc.SelectionChanged += dgvLanTruoc_SelectionChanged;
        lblLanTruocChiTiet.BorderStyle = BorderStyle.FixedSingle; lblLanTruocChiTiet.Dock = DockStyle.Bottom; lblLanTruocChiTiet.Name = "lblLanTruocChiTiet";
        lblLanTruocChiTiet.Padding = new Padding(4); lblLanTruocChiTiet.Size = new Size(300, 170);
        colLtNgay.DataPropertyName = "NgayGioKham"; colLtNgay.DefaultCellStyle.Format = "dd/MM/yyyy"; colLtNgay.HeaderText = "Ngày"; colLtNgay.Name = "colLtNgay"; colLtNgay.FillWeight = 70;
        colLtChanDoan.DataPropertyName = "ChanDoanBenh"; colLtChanDoan.HeaderText = "Chẩn đoán"; colLtChanDoan.Name = "colLtChanDoan"; colLtChanDoan.FillWeight = 140;
        colLtBacSi.DataPropertyName = "TenBacSi"; colLtBacSi.HeaderText = "Bác sĩ"; colLtBacSi.Name = "colLtBacSi"; colLtBacSi.FillWeight = 100;
        //
        // khungKham
        //
        khungKham.Controls.Add(tlpKham);
        khungKham.Dock = DockStyle.Fill;
        khungKham.Margin = new Padding(8, 0, 0, 0);
        khungKham.Name = "khungKham";
        //
        // tlpKham
        //
        tlpKham.ColumnCount = 2;
        tlpKham.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 130F));
        tlpKham.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlpKham.Controls.Add(lblChanDoan, 0, 0);
        tlpKham.Controls.Add(txtChanDoan, 1, 0);
        tlpKham.Controls.Add(lblLoiDan, 0, 1);
        tlpKham.Controls.Add(txtLoiDan, 1, 1);
        tlpKham.Controls.Add(lblTienCong, 0, 2);
        tlpKham.Controls.Add(numTienCong, 1, 2);
        tlpKham.Controls.Add(lblKeThuoc, 0, 3);
        tlpKham.Controls.Add(flpKeThuoc, 1, 3);
        tlpKham.Controls.Add(dgvDonThuoc, 0, 4);
        tlpKham.Controls.Add(pnlDuoi, 0, 5);
        tlpKham.SetColumnSpan(dgvDonThuoc, 2);
        tlpKham.SetColumnSpan(pnlDuoi, 2);
        tlpKham.Dock = DockStyle.Fill;
        tlpKham.Name = "tlpKham";
        tlpKham.RowCount = 6;
        tlpKham.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        tlpKham.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
        tlpKham.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlpKham.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        tlpKham.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        tlpKham.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
        lblChanDoan.AutoSize = true; lblChanDoan.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblChanDoan.Margin = new Padding(0, 8, 0, 0); lblChanDoan.Text = "Chẩn đoán *";
        lblLoiDan.AutoSize = true; lblLoiDan.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblLoiDan.Margin = new Padding(0, 8, 0, 0); lblLoiDan.Text = "Lời dặn";
        lblTienCong.Anchor = AnchorStyles.Left; lblTienCong.AutoSize = true; lblTienCong.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblTienCong.Text = "Tiền công khám";
        lblKeThuoc.Anchor = AnchorStyles.Left; lblKeThuoc.AutoSize = true; lblKeThuoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblKeThuoc.Text = "Kê thuốc";
        txtChanDoan.Dock = DockStyle.Fill; txtChanDoan.Margin = new Padding(0, 4, 0, 4); txtChanDoan.MaxLength = 255; txtChanDoan.Multiline = true; txtChanDoan.Name = "txtChanDoan"; txtChanDoan.TextChanged += ODuLieu_Changed;
        txtLoiDan.Dock = DockStyle.Fill; txtLoiDan.Margin = new Padding(0, 4, 0, 4); txtLoiDan.MaxLength = 255; txtLoiDan.Multiline = true; txtLoiDan.Name = "txtLoiDan"; txtLoiDan.TextChanged += ODuLieu_Changed;
        numTienCong.Anchor = AnchorStyles.Left; numTienCong.Increment = 10000; numTienCong.Maximum = 999999999999; numTienCong.Name = "numTienCong";
        numTienCong.Size = new Size(160, 30); numTienCong.ThousandsSeparator = true; numTienCong.TextChanged += ODuLieu_Changed; numTienCong.ValueChanged += numTienCong_ValueChanged;
        //
        // flpKeThuoc
        //
        flpKeThuoc.Controls.Add(cboThuoc);
        flpKeThuoc.Controls.Add(numSoLuong);
        flpKeThuoc.Controls.Add(txtLieuDung);
        flpKeThuoc.Controls.Add(btnThemThuoc);
        flpKeThuoc.Dock = DockStyle.Fill;
        flpKeThuoc.Margin = new Padding(0);
        flpKeThuoc.Name = "flpKeThuoc";
        flpKeThuoc.WrapContents = false;
        cboThuoc.DropDownStyle = ComboBoxStyle.DropDownList; cboThuoc.FormattingEnabled = true; cboThuoc.Format += cboThuoc_Format;
        cboThuoc.SelectedIndexChanged += cboThuoc_SelectedIndexChanged; cboThuoc.Margin = new Padding(0, 8, 8, 0); cboThuoc.Name = "cboThuoc"; cboThuoc.Size = new Size(300, 30);
        numSoLuong.Margin = new Padding(0, 8, 8, 0); numSoLuong.Maximum = 100000; numSoLuong.Minimum = 1; numSoLuong.Name = "numSoLuong"; numSoLuong.Size = new Size(70, 30); numSoLuong.Value = 10;
        txtLieuDung.Margin = new Padding(0, 8, 8, 0); txtLieuDung.Name = "txtLieuDung"; txtLieuDung.MaxLength = 255; txtLieuDung.PlaceholderText = "Liều dùng (VD: sáng 1 viên, tối 1 viên)"; txtLieuDung.Size = new Size(260, 30);
        btnThemThuoc.Margin = new Padding(0, 6, 0, 0); btnThemThuoc.Name = "btnThemThuoc"; btnThemThuoc.Size = new Size(100, 36); btnThemThuoc.Text = "Thêm";
        btnThemThuoc.Click += btnThemThuoc_Click;
        //
        // dgvDonThuoc
        //
        dgvDonThuoc.Columns.AddRange(new DataGridViewColumn[] { colTenThuoc, colSoLuong, colDvt, colLieuDung, colDonGia, colThanhTien, colXoa });
        dgvDonThuoc.Dock = DockStyle.Fill; dgvDonThuoc.Margin = new Padding(0, 8, 0, 0); dgvDonThuoc.Name = "dgvDonThuoc";
        dgvDonThuoc.CellContentClick += dgvDonThuoc_CellContentClick;
        dgvDonThuoc.CellDoubleClick += dgvDonThuoc_CellDoubleClick;
        colTenThuoc.DataPropertyName = "TenThuoc"; colTenThuoc.HeaderText = "Thuốc"; colTenThuoc.Name = "colTenThuoc"; colTenThuoc.FillWeight = 130;
        colSoLuong.DataPropertyName = "SoLuongThuoc"; colSoLuong.HeaderText = "SL"; colSoLuong.Name = "colSoLuong"; colSoLuong.FillWeight = 40;
        colDvt.DataPropertyName = "DonViTinh"; colDvt.HeaderText = "ĐVT"; colDvt.Name = "colDvt"; colDvt.FillWeight = 45;
        colLieuDung.DataPropertyName = "LieuDung"; colLieuDung.HeaderText = "Liều dùng"; colLieuDung.Name = "colLieuDung"; colLieuDung.FillWeight = 190;
        colDonGia.DataPropertyName = "DonGiaThuoc"; colDonGia.HeaderText = "Đơn giá"; colDonGia.Name = "colDonGia"; colDonGia.FillWeight = 70; colDonGia.DefaultCellStyle.Format = "#,0";
        colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colThanhTien.DataPropertyName = "ThanhTien"; colThanhTien.HeaderText = "Thành tiền"; colThanhTien.Name = "colThanhTien"; colThanhTien.FillWeight = 75; colThanhTien.DefaultCellStyle.Format = "#,0";
        colThanhTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colXoa.HeaderText = ""; colXoa.Name = "colXoa"; colXoa.Text = "Xóa"; colXoa.UseColumnTextForButtonValue = true; colXoa.FillWeight = 40;
        //
        // pnlDuoi
        //
        pnlDuoi.Controls.Add(lblTong);
        pnlDuoi.Controls.Add(btnLuuNhap);
        pnlDuoi.Controls.Add(btnHoanTat);
        pnlDuoi.Dock = DockStyle.Fill;
        pnlDuoi.Margin = new Padding(0);
        pnlDuoi.Name = "pnlDuoi";
        pnlDuoi.Size = new Size(720, 60);
        lblTong.AutoSize = true; lblTong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblTong.Location = new Point(0, 18); lblTong.Name = "lblTong";
        btnLuuNhap.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnLuuNhap.Location = new Point(420, 12); btnLuuNhap.Name = "btnLuuNhap"; btnLuuNhap.Size = new Size(120, 30);
        btnLuuNhap.Text = "Lưu nháp"; btnLuuNhap.Click += btnLuuNhap_Click;
        btnHoanTat.Anchor = AnchorStyles.Top | AnchorStyles.Right; btnHoanTat.Location = new Point(550, 12); btnHoanTat.Name = "btnHoanTat"; btnHoanTat.Size = new Size(170, 30);
        btnHoanTat.Text = "Hoàn tất khám"; btnHoanTat.Click += btnHoanTat_Click;
        //
        // frmKhamBenh
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1200, 760);
        Controls.Add(tlpNen);
        MinimizeBox = false;
        MinimumSize = new Size(1100, 700);
        Name = "frmKhamBenh";
        ShowInTaskbar = false;
        FormClosing += frmKhamBenh_FormClosing;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Khám bệnh";
        tlpNen.ResumeLayout(false);
        pnlTrai.ResumeLayout(false);
        khungLanTruoc.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLanTruoc).EndInit();
        khungBenhNhan.ResumeLayout(false);
        khungKham.ResumeLayout(false);
        tlpKham.ResumeLayout(false);
        tlpKham.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numTienCong).EndInit();
        flpKeThuoc.ResumeLayout(false);
        flpKeThuoc.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numSoLuong).EndInit();
        ((System.ComponentModel.ISupportInitialize)dgvDonThuoc).EndInit();
        pnlDuoi.ResumeLayout(false);
        pnlDuoi.PerformLayout();
        ResumeLayout(false);
    }

    private TableLayoutPanel tlpNen;
    private Panel pnlTrai;
    private Panel khungBenhNhan;
    private Label lblTen;
    private Label lblThongTin;
    private Label lblTrieuChungTieuDe;
    private TextBox txtTrieuChung;
    private Panel khungLanTruoc;
    private Label lblLanTruoc;
    private DataGridView dgvLanTruoc;
    private Label lblLanTruocChiTiet;
    private DataGridViewTextBoxColumn colLtNgay;
    private DataGridViewTextBoxColumn colLtChanDoan;
    private DataGridViewTextBoxColumn colLtBacSi;
    private Panel khungKham;
    private TableLayoutPanel tlpKham;
    private Label lblChanDoan;
    private TextBox txtChanDoan;
    private Label lblLoiDan;
    private TextBox txtLoiDan;
    private Label lblTienCong;
    private NumericUpDown numTienCong;
    private Label lblKeThuoc;
    private FlowLayoutPanel flpKeThuoc;
    private ComboBox cboThuoc;
    private NumericUpDown numSoLuong;
    private TextBox txtLieuDung;
    private Button btnThemThuoc;
    private DataGridView dgvDonThuoc;
    private DataGridViewTextBoxColumn colTenThuoc;
    private DataGridViewTextBoxColumn colSoLuong;
    private DataGridViewTextBoxColumn colDvt;
    private DataGridViewTextBoxColumn colLieuDung;
    private DataGridViewTextBoxColumn colDonGia;
    private DataGridViewTextBoxColumn colThanhTien;
    private DataGridViewButtonColumn colXoa;
    private Panel pnlDuoi;
    private Label lblTong;
    private Button btnLuuNhap;
    private Button btnHoanTat;
}
