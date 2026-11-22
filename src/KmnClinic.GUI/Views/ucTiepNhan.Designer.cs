namespace KmnClinic.GUI.Views;

partial class ucTiepNhan
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tlp = new TableLayoutPanel();
        khungHen = new Panel();
        dgvHen = new DataGridView();
        colHenMa = new DataGridViewTextBoxColumn();
        colHenKhach = new DataGridViewTextBoxColumn();
        colHenCa = new DataGridViewTextBoxColumn();
        colHenBacSi = new DataGridViewTextBoxColumn();
        colHenTrangThai = new DataGridViewTextBoxColumn();
        pnlHenDuoi = new Panel();
        btnTiepNhanHen = new Button();
        lblHen = new Label();
        khungVangLai = new Panel();
        lblVangLai = new Label();
        txtSdt = new TextBox();
        btnTim = new Button();
        lblKhach = new Label();
        txtHoTen = new TextBox();
        cboBacSi = new ComboBox();
        txtTrieuChung = new TextBox();
        btnTiepNhanVangLai = new Button();
        lblQuyTac = new Label();
        khungHoSo = new Panel();
        dgvHoSo = new DataGridView();
        colHsMa = new DataGridViewTextBoxColumn();
        colHsGio = new DataGridViewTextBoxColumn();
        colHsKhach = new DataGridViewTextBoxColumn();
        colHsNguon = new DataGridViewTextBoxColumn();
        colHsBacSi = new DataGridViewTextBoxColumn();
        colHsTrangThai = new DataGridViewTextBoxColumn();
        lblHoSo = new Label();
        tlp.SuspendLayout();
        khungHen.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHen).BeginInit();
        pnlHenDuoi.SuspendLayout();
        khungVangLai.SuspendLayout();
        khungHoSo.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvHoSo).BeginInit();
        SuspendLayout();
        //
        // tlp
        //
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
        tlp.Controls.Add(khungHen, 0, 0);
        tlp.Controls.Add(khungVangLai, 1, 0);
        tlp.Controls.Add(khungHoSo, 0, 1);
        tlp.SetColumnSpan(khungHoSo, 2);
        tlp.Dock = DockStyle.Fill;
        tlp.Name = "tlp";
        tlp.RowCount = 2;
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
        //
        // khungHen
        //
        khungHen.Controls.Add(dgvHen);
        khungHen.Controls.Add(pnlHenDuoi);
        khungHen.Controls.Add(lblHen);
        khungHen.Dock = DockStyle.Fill;
        khungHen.Margin = new Padding(0, 0, 8, 8);
        khungHen.Name = "khungHen";
        lblHen.Dock = DockStyle.Top; lblHen.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblHen.Name = "lblHen"; lblHen.Size = new Size(500, 32); lblHen.Text = "Khách có hẹn hôm nay";
        dgvHen.Columns.AddRange(new DataGridViewColumn[] { colHenMa, colHenKhach, colHenCa, colHenBacSi, colHenTrangThai });
        dgvHen.Dock = DockStyle.Fill; dgvHen.Name = "dgvHen";
        dgvHen.SelectionChanged += dgvHen_SelectionChanged;
        colHenMa.DataPropertyName = "MaLichHen"; colHenMa.HeaderText = "Mã"; colHenMa.Name = "colHenMa"; colHenMa.FillWeight = 60;
        colHenKhach.DataPropertyName = "TenKhachHang"; colHenKhach.HeaderText = "Khách hàng"; colHenKhach.Name = "colHenKhach"; colHenKhach.FillWeight = 130;
        colHenCa.DataPropertyName = "TenCa"; colHenCa.HeaderText = "Ca"; colHenCa.Name = "colHenCa"; colHenCa.FillWeight = 65;
        colHenBacSi.DataPropertyName = "TenBacSi"; colHenBacSi.HeaderText = "Bác sĩ"; colHenBacSi.Name = "colHenBacSi"; colHenBacSi.FillWeight = 130;
        colHenTrangThai.DataPropertyName = "TrangThai"; colHenTrangThai.HeaderText = "Trạng thái"; colHenTrangThai.Name = "colHenTrangThai"; colHenTrangThai.FillWeight = 95;
        pnlHenDuoi.Controls.Add(btnTiepNhanHen);
        pnlHenDuoi.Dock = DockStyle.Bottom; pnlHenDuoi.Name = "pnlHenDuoi"; pnlHenDuoi.Size = new Size(500, 52);
        btnTiepNhanHen.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; btnTiepNhanHen.Location = new Point(270, 12); btnTiepNhanHen.Name = "btnTiepNhanHen";
        btnTiepNhanHen.Size = new Size(230, 30); btnTiepNhanHen.Text = "Tiếp nhận và tạo hồ sơ"; btnTiepNhanHen.Click += btnTiepNhanHen_Click;
        //
        // khungVangLai
        //
        khungVangLai.Controls.AddRange(new Control[] { lblVangLai, txtSdt, btnTim, lblKhach, txtHoTen, cboBacSi, txtTrieuChung, btnTiepNhanVangLai, lblQuyTac });
        khungVangLai.Dock = DockStyle.Fill;
        khungVangLai.Margin = new Padding(8, 0, 0, 8);
        khungVangLai.Name = "khungVangLai";
        lblVangLai.AutoSize = true; lblVangLai.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblVangLai.Location = new Point(16, 16); lblVangLai.Name = "lblVangLai"; lblVangLai.Text = "Khách vãng lai";
        txtSdt.Location = new Point(16, 52); txtSdt.MaxLength = 10; txtSdt.Name = "txtSdt"; txtSdt.PlaceholderText = "Số điện thoại"; txtSdt.Size = new Size(200, 30);
        btnTim.Location = new Point(224, 50); btnTim.Name = "btnTim"; btnTim.Size = new Size(90, 34); btnTim.Text = "Tìm"; btnTim.Click += btnTim_Click;
        lblKhach.Location = new Point(16, 88); lblKhach.Name = "lblKhach"; lblKhach.Size = new Size(380, 22);
        txtHoTen.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; txtHoTen.Location = new Point(16, 114); txtHoTen.Name = "txtHoTen";
        txtHoTen.PlaceholderText = "Họ tên (khách mới)"; txtHoTen.Size = new Size(380, 30);
        cboBacSi.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right; cboBacSi.DropDownStyle = ComboBoxStyle.DropDownList;
        cboBacSi.Location = new Point(16, 154); cboBacSi.Name = "cboBacSi"; cboBacSi.Size = new Size(380, 30);
        txtTrieuChung.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right; txtTrieuChung.Location = new Point(16, 194);
        txtTrieuChung.Multiline = true; txtTrieuChung.Name = "txtTrieuChung"; txtTrieuChung.PlaceholderText = "Triệu chứng"; txtTrieuChung.Size = new Size(380, 60);
        lblQuyTac.Anchor = AnchorStyles.Bottom | AnchorStyles.Left; lblQuyTac.Font = new Font("Segoe UI", 8.5F);         lblQuyTac.Location = new Point(16, 262); lblQuyTac.Name = "lblQuyTac"; lblQuyTac.Size = new Size(200, 40); lblQuyTac.Text = "Chỉ bác sĩ đang có ca trực (cho sớm 30 phút, RB41).";
        btnTiepNhanVangLai.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; btnTiepNhanVangLai.Location = new Point(226, 262); btnTiepNhanVangLai.Name = "btnTiepNhanVangLai";
        btnTiepNhanVangLai.Size = new Size(170, 30); btnTiepNhanVangLai.Text = "Tiếp nhận"; btnTiepNhanVangLai.Click += btnTiepNhanVangLai_Click;
        //
        // khungHoSo
        //
        khungHoSo.Controls.Add(dgvHoSo);
        khungHoSo.Controls.Add(lblHoSo);
        khungHoSo.Dock = DockStyle.Fill;
        khungHoSo.Margin = new Padding(0, 8, 0, 0);
        khungHoSo.Name = "khungHoSo";
        lblHoSo.Dock = DockStyle.Top; lblHoSo.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblHoSo.Name = "lblHoSo"; lblHoSo.Size = new Size(500, 32); lblHoSo.Text = "Hồ sơ khám đã tạo hôm nay";
        dgvHoSo.Columns.AddRange(new DataGridViewColumn[] { colHsMa, colHsGio, colHsKhach, colHsNguon, colHsBacSi, colHsTrangThai });
        dgvHoSo.Dock = DockStyle.Fill; dgvHoSo.Name = "dgvHoSo";
        colHsMa.DataPropertyName = "MaHoSo"; colHsMa.HeaderText = "Mã hồ sơ"; colHsMa.Name = "colHsMa"; colHsMa.FillWeight = 70;
        colHsGio.DataPropertyName = "NgayGioKham"; colHsGio.HeaderText = "Giờ tiếp nhận"; colHsGio.Name = "colHsGio"; colHsGio.FillWeight = 70;
        colHsGio.DefaultCellStyle.Format = "HH:mm";
        colHsKhach.DataPropertyName = "TenKhachHang"; colHsKhach.HeaderText = "Khách hàng"; colHsKhach.Name = "colHsKhach"; colHsKhach.FillWeight = 130;
        colHsNguon.DataPropertyName = "Nguon"; colHsNguon.HeaderText = "Nguồn"; colHsNguon.Name = "colHsNguon"; colHsNguon.FillWeight = 70;
        colHsBacSi.DataPropertyName = "TenBacSi"; colHsBacSi.HeaderText = "Bác sĩ"; colHsBacSi.Name = "colHsBacSi"; colHsBacSi.FillWeight = 130;
        colHsTrangThai.DataPropertyName = "TrangThai"; colHsTrangThai.HeaderText = "Trạng thái"; colHsTrangThai.Name = "colHsTrangThai"; colHsTrangThai.FillWeight = 90;
        //
        // ucTiepNhan
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(tlp);
        Name = "ucTiepNhan";
        Size = new Size(1100, 680);
        tlp.ResumeLayout(false);
        khungHen.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHen).EndInit();
        pnlHenDuoi.ResumeLayout(false);
        khungVangLai.ResumeLayout(false);
        khungVangLai.PerformLayout();
        khungHoSo.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvHoSo).EndInit();
        ResumeLayout(false);
    }

    private TableLayoutPanel tlp;
    private Panel khungHen;
    private DataGridView dgvHen;
    private DataGridViewTextBoxColumn colHenMa;
    private DataGridViewTextBoxColumn colHenKhach;
    private DataGridViewTextBoxColumn colHenCa;
    private DataGridViewTextBoxColumn colHenBacSi;
    private DataGridViewTextBoxColumn colHenTrangThai;
    private Panel pnlHenDuoi;
    private Button btnTiepNhanHen;
    private Label lblHen;
    private Panel khungVangLai;
    private Label lblVangLai;
    private TextBox txtSdt;
    private Button btnTim;
    private Label lblKhach;
    private TextBox txtHoTen;
    private ComboBox cboBacSi;
    private TextBox txtTrieuChung;
    private Button btnTiepNhanVangLai;
    private Label lblQuyTac;
    private Panel khungHoSo;
    private DataGridView dgvHoSo;
    private DataGridViewTextBoxColumn colHsMa;
    private DataGridViewTextBoxColumn colHsGio;
    private DataGridViewTextBoxColumn colHsKhach;
    private DataGridViewTextBoxColumn colHsNguon;
    private DataGridViewTextBoxColumn colHsBacSi;
    private DataGridViewTextBoxColumn colHsTrangThai;
    private Label lblHoSo;
}
