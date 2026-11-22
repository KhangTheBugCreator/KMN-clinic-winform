namespace KmnClinic.GUI.Views;

partial class ucPhong
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
        lblKhoa = new Label();
        cboKhoa = new ComboBox();
        btnThem = new Button();
        btnSua = new Button();
        btnXoa = new Button();
        btnLuu = new Button();
        btnHuy = new Button();
        pnlTim = new Panel();
        lblTim = new Label();
        txtTim = new TextBox();
        cboLocKhoa = new ComboBox();
        lblDem = new Label();
        dgv = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colTen = new DataGridViewTextBoxColumn();
        colKhoa = new DataGridViewTextBoxColumn();
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
        lblMa.Text = "Mã phòng:";
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
        lblTen.Text = "Tên phòng:";
        //
        // txtTen
        //
        txtTen.Location = new Point(120, 58);
        txtTen.Name = "txtTen";
        txtTen.Size = new Size(320, 23);
        txtTen.TabIndex = 1;
        txtTen.MaxLength = 50;
        //
        // lblKhoa
        //
        lblKhoa.AutoSize = true;
        lblKhoa.Location = new Point(14, 93);
        lblKhoa.Name = "lblKhoa";
        lblKhoa.Text = "Thuộc khoa:";
        //
        // cboKhoa
        //
        cboKhoa.Location = new Point(120, 90);
        cboKhoa.Name = "cboKhoa";
        cboKhoa.Size = new Size(320, 23);
        cboKhoa.TabIndex = 2;
        cboKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
        //
        // btnThem
        //
        btnThem.Location = new Point(120, 126);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(84, 28);
        btnThem.TabIndex = 3;
        btnThem.Text = "Thêm";
        btnThem.Click += btnThem_Click;
        //
        // btnSua
        //
        btnSua.Location = new Point(210, 126);
        btnSua.Name = "btnSua";
        btnSua.Size = new Size(84, 28);
        btnSua.TabIndex = 4;
        btnSua.Text = "Sửa";
        btnSua.Click += btnSua_Click;
        //
        // btnXoa
        //
        btnXoa.Location = new Point(300, 126);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new Size(84, 28);
        btnXoa.TabIndex = 5;
        btnXoa.Text = "Xóa";
        btnXoa.Click += btnXoa_Click;
        //
        // btnLuu
        //
        btnLuu.Location = new Point(390, 126);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new Size(84, 28);
        btnLuu.TabIndex = 6;
        btnLuu.Text = "Lưu";
        btnLuu.Click += btnLuu_Click;
        //
        // btnHuy
        //
        btnHuy.Location = new Point(480, 126);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new Size(84, 28);
        btnHuy.TabIndex = 7;
        btnHuy.Text = "Hủy";
        btnHuy.Click += btnHuy_Click;
        //
        // grpThongTin
        //
        grpThongTin.Controls.Add(lblMa);
        grpThongTin.Controls.Add(txtMa);
        grpThongTin.Controls.Add(lblTen);
        grpThongTin.Controls.Add(txtTen);
        grpThongTin.Controls.Add(lblKhoa);
        grpThongTin.Controls.Add(cboKhoa);
        grpThongTin.Controls.Add(btnThem);
        grpThongTin.Controls.Add(btnSua);
        grpThongTin.Controls.Add(btnXoa);
        grpThongTin.Controls.Add(btnLuu);
        grpThongTin.Controls.Add(btnHuy);
        grpThongTin.Dock = DockStyle.Top;
        grpThongTin.Name = "grpThongTin";
        grpThongTin.Size = new Size(1000, 168);
        grpThongTin.Text = "Thông tin phòng làm việc";
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
        txtTim.TabIndex = 8;
        txtTim.TextChanged += BoLoc_Changed;
        //
        // cboLocKhoa
        //
        cboLocKhoa.Location = new Point(346, 9);
        cboLocKhoa.Name = "cboLocKhoa";
        cboLocKhoa.Size = new Size(220, 23);
        cboLocKhoa.TabIndex = 9;
        cboLocKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLocKhoa.SelectedIndexChanged += BoLoc_Changed;
        //
        // lblDem
        //
        lblDem.AutoSize = true;
        lblDem.Location = new Point(582, 12);
        lblDem.Name = "lblDem";
        //
        // pnlTim
        //
        pnlTim.Controls.Add(lblTim);
        pnlTim.Controls.Add(txtTim);
        pnlTim.Controls.Add(cboLocKhoa);
        pnlTim.Controls.Add(lblDem);
        pnlTim.Dock = DockStyle.Top;
        pnlTim.Name = "pnlTim";
        pnlTim.Size = new Size(1000, 40);
        //
        // dgv
        //
        dgv.Columns.AddRange(new DataGridViewColumn[] { colMa, colTen, colKhoa, colSoLich });
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 10;
        dgv.SelectionChanged += dgv_SelectionChanged;
        //
        // các cột
        //
        colMa.DataPropertyName = "MaPhong"; colMa.HeaderText = "Mã"; colMa.Name = "colMa"; colMa.FillWeight = 60;
        colTen.DataPropertyName = "TenPhong"; colTen.HeaderText = "Tên phòng"; colTen.Name = "colTen"; colTen.FillWeight = 180;
        colKhoa.DataPropertyName = "TenKhoa"; colKhoa.HeaderText = "Khoa"; colKhoa.Name = "colKhoa"; colKhoa.FillWeight = 180;
        colSoLich.DataPropertyName = "SoLichLamViec"; colSoLich.HeaderText = "Số lịch làm việc"; colSoLich.Name = "colSoLich"; colSoLich.FillWeight = 80;
        colSoLich.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //
        // ucPhong
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dgv);
        Controls.Add(pnlTim);
        Controls.Add(grpThongTin);
        Name = "ucPhong";
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
    private Label lblKhoa;
    private ComboBox cboKhoa;
    private Button btnThem;
    private Button btnSua;
    private Button btnXoa;
    private Button btnLuu;
    private Button btnHuy;
    private Panel pnlTim;
    private Label lblTim;
    private TextBox txtTim;
    private ComboBox cboLocKhoa;
    private Label lblDem;
    private DataGridView dgv;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colTen;
    private DataGridViewTextBoxColumn colKhoa;
    private DataGridViewTextBoxColumn colSoLich;
}
