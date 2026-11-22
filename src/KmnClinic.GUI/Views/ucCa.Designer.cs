namespace KmnClinic.GUI.Views;

partial class ucCa
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        grpThongTin = new GroupBox();
        lblMa = new Label();
        txtMa = new TextBox();
        lblTen = new Label();
        txtTen = new TextBox();
        lblBatDau = new Label();
        dtpBatDau = new DateTimePicker();
        lblKetThuc = new Label();
        dtpKetThuc = new DateTimePicker();
        btnThem = new Button();
        btnSua = new Button();
        btnXoa = new Button();
        btnLuu = new Button();
        btnHuy = new Button();
        pnlTim = new Panel();
        lblTim = new Label();
        txtTim = new TextBox();
        lblDem = new Label();
        dgv = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colTen = new DataGridViewTextBoxColumn();
        colBatDau = new DataGridViewTextBoxColumn();
        colKetThuc = new DataGridViewTextBoxColumn();
        colSoLich = new DataGridViewTextBoxColumn();
        grpThongTin.SuspendLayout();
        pnlTim.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();
        //
        // lblMa
        //
        lblMa.AutoSize = true;
        lblMa.Location = new Point(14, 29);
        lblMa.Name = "lblMa";
        lblMa.Text = "Mã ca:";
        //
        // txtMa
        //
        txtMa.Location = new Point(120, 26);
        txtMa.Name = "txtMa";
        txtMa.Size = new Size(120, 23);
        txtMa.TabIndex = 0;
        txtMa.ReadOnly = true;
        txtMa.TabStop = false;
        //
        // lblTen
        //
        lblTen.AutoSize = true;
        lblTen.Location = new Point(14, 61);
        lblTen.Name = "lblTen";
        lblTen.Text = "Tên ca:";
        //
        // txtTen
        //
        txtTen.Location = new Point(120, 58);
        txtTen.Name = "txtTen";
        txtTen.Size = new Size(240, 23);
        txtTen.TabIndex = 1;
        txtTen.MaxLength = 20;
        //
        // lblBatDau
        //
        lblBatDau.AutoSize = true;
        lblBatDau.Location = new Point(14, 93);
        lblBatDau.Name = "lblBatDau";
        lblBatDau.Text = "Giờ bắt đầu:";
        //
        // dtpBatDau
        //
        dtpBatDau.Location = new Point(120, 90);
        dtpBatDau.Name = "dtpBatDau";
        dtpBatDau.Size = new Size(90, 23);
        dtpBatDau.TabIndex = 2;
        dtpBatDau.CustomFormat = "HH:mm";
        dtpBatDau.Format = DateTimePickerFormat.Custom;
        dtpBatDau.ShowUpDown = true;
        //
        // lblKetThuc
        //
        lblKetThuc.AutoSize = true;
        lblKetThuc.Location = new Point(14, 125);
        lblKetThuc.Name = "lblKetThuc";
        lblKetThuc.Text = "Giờ kết thúc:";
        //
        // dtpKetThuc
        //
        dtpKetThuc.Location = new Point(120, 122);
        dtpKetThuc.Name = "dtpKetThuc";
        dtpKetThuc.Size = new Size(90, 23);
        dtpKetThuc.TabIndex = 3;
        dtpKetThuc.CustomFormat = "HH:mm";
        dtpKetThuc.Format = DateTimePickerFormat.Custom;
        dtpKetThuc.ShowUpDown = true;
        //
        // btnThem
        //
        btnThem.Location = new Point(120, 158);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(84, 28);
        btnThem.TabIndex = 4;
        btnThem.Text = "Thêm";
        btnThem.Click += btnThem_Click;
        //
        // btnSua
        //
        btnSua.Location = new Point(210, 158);
        btnSua.Name = "btnSua";
        btnSua.Size = new Size(84, 28);
        btnSua.TabIndex = 5;
        btnSua.Text = "Sửa";
        btnSua.Click += btnSua_Click;
        //
        // btnXoa
        //
        btnXoa.Location = new Point(300, 158);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new Size(84, 28);
        btnXoa.TabIndex = 6;
        btnXoa.Text = "Xóa";
        btnXoa.Click += btnXoa_Click;
        //
        // btnLuu
        //
        btnLuu.Location = new Point(390, 158);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new Size(84, 28);
        btnLuu.TabIndex = 7;
        btnLuu.Text = "Lưu";
        btnLuu.Click += btnLuu_Click;
        //
        // btnHuy
        //
        btnHuy.Location = new Point(480, 158);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new Size(84, 28);
        btnHuy.TabIndex = 8;
        btnHuy.Text = "Hủy";
        btnHuy.Click += btnHuy_Click;
        //
        // grpThongTin
        //
        grpThongTin.Controls.Add(lblMa);
        grpThongTin.Controls.Add(txtMa);
        grpThongTin.Controls.Add(lblTen);
        grpThongTin.Controls.Add(txtTen);
        grpThongTin.Controls.Add(lblBatDau);
        grpThongTin.Controls.Add(dtpBatDau);
        grpThongTin.Controls.Add(lblKetThuc);
        grpThongTin.Controls.Add(dtpKetThuc);
        grpThongTin.Controls.Add(btnThem);
        grpThongTin.Controls.Add(btnSua);
        grpThongTin.Controls.Add(btnXoa);
        grpThongTin.Controls.Add(btnLuu);
        grpThongTin.Controls.Add(btnHuy);
        grpThongTin.Dock = DockStyle.Top;
        grpThongTin.Name = "grpThongTin";
        grpThongTin.Size = new Size(1000, 200);
        grpThongTin.Text = "Thông tin ca làm việc";
        //
        // lblTim
        //
        lblTim.AutoSize = true;
        lblTim.Location = new Point(2, 12);
        lblTim.Name = "lblTim";
        lblTim.Text = "Tìm kiếm:";
        //
        // txtTim
        //
        txtTim.Location = new Point(70, 9);
        txtTim.Name = "txtTim";
        txtTim.Size = new Size(260, 23);
        txtTim.TabIndex = 9;
        txtTim.TextChanged += BoLoc_Changed;
        //
        // lblDem
        //
        lblDem.AutoSize = true;
        lblDem.Location = new Point(346, 12);
        lblDem.Name = "lblDem";
        //
        // pnlTim
        //
        pnlTim.Controls.Add(lblTim);
        pnlTim.Controls.Add(txtTim);
        pnlTim.Controls.Add(lblDem);
        pnlTim.Dock = DockStyle.Top;
        pnlTim.Name = "pnlTim";
        pnlTim.Size = new Size(1000, 40);
        //
        // dgv
        //
        dgv.Columns.AddRange(new DataGridViewColumn[] { colMa, colTen, colBatDau, colKetThuc, colSoLich });
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 10;
        dgv.SelectionChanged += dgv_SelectionChanged;
        //
        // các cột
        //
        colMa.DataPropertyName = "MaCa"; colMa.HeaderText = "Mã"; colMa.Name = "colMa"; colMa.FillWeight = 60;
        colTen.DataPropertyName = "TenCa"; colTen.HeaderText = "Tên ca"; colTen.Name = "colTen"; colTen.FillWeight = 140;
        colBatDau.DataPropertyName = "GioBatDau"; colBatDau.HeaderText = "Bắt đầu"; colBatDau.Name = "colBatDau"; colBatDau.FillWeight = 80;
        colBatDau.DefaultCellStyle.Format = "hh\\:mm";
        colKetThuc.DataPropertyName = "GioKetThuc"; colKetThuc.HeaderText = "Kết thúc"; colKetThuc.Name = "colKetThuc"; colKetThuc.FillWeight = 80;
        colKetThuc.DefaultCellStyle.Format = "hh\\:mm";
        colSoLich.DataPropertyName = "SoLichLamViec"; colSoLich.HeaderText = "Số lịch làm việc"; colSoLich.Name = "colSoLich"; colSoLich.FillWeight = 90;
        colSoLich.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //
        // ucCa
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dgv);
        Controls.Add(pnlTim);
        Controls.Add(grpThongTin);
        Name = "ucCa";
        Padding = new Padding(8);
        Size = new Size(1000, 600);
        grpThongTin.ResumeLayout(false);
        grpThongTin.PerformLayout();
        pnlTim.ResumeLayout(false);
        pnlTim.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
    }

    private GroupBox grpThongTin;
    private Label lblMa;
    private TextBox txtMa;
    private Label lblTen;
    private TextBox txtTen;
    private Label lblBatDau;
    private DateTimePicker dtpBatDau;
    private Label lblKetThuc;
    private DateTimePicker dtpKetThuc;
    private Button btnThem;
    private Button btnSua;
    private Button btnXoa;
    private Button btnLuu;
    private Button btnHuy;
    private Panel pnlTim;
    private Label lblTim;
    private TextBox txtTim;
    private Label lblDem;
    private DataGridView dgv;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colTen;
    private DataGridViewTextBoxColumn colBatDau;
    private DataGridViewTextBoxColumn colKetThuc;
    private DataGridViewTextBoxColumn colSoLich;
}
