namespace KmnClinic.GUI.Views;

partial class ucKhoa
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
        lblMoTa = new Label();
        txtMoTa = new TextBox();
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
        colMoTa = new DataGridViewTextBoxColumn();
        colSoPhong = new DataGridViewTextBoxColumn();
        colSoBacSi = new DataGridViewTextBoxColumn();
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
        lblMa.Text = "Mã khoa:";
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
        lblTen.Text = "Tên khoa:";
        //
        // txtTen
        //
        txtTen.Location = new Point(120, 58);
        txtTen.Name = "txtTen";
        txtTen.Size = new Size(320, 23);
        txtTen.TabIndex = 1;
        txtTen.MaxLength = 50;
        //
        // lblMoTa
        //
        lblMoTa.AutoSize = true;
        lblMoTa.Location = new Point(14, 93);
        lblMoTa.Name = "lblMoTa";
        lblMoTa.Text = "Mô tả:";
        //
        // txtMoTa
        //
        txtMoTa.Location = new Point(120, 90);
        txtMoTa.Name = "txtMoTa";
        txtMoTa.Size = new Size(560, 23);
        txtMoTa.TabIndex = 2;
        txtMoTa.MaxLength = 255;
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
        grpThongTin.Controls.Add(lblMoTa);
        grpThongTin.Controls.Add(txtMoTa);
        grpThongTin.Controls.Add(btnThem);
        grpThongTin.Controls.Add(btnSua);
        grpThongTin.Controls.Add(btnXoa);
        grpThongTin.Controls.Add(btnLuu);
        grpThongTin.Controls.Add(btnHuy);
        grpThongTin.Dock = DockStyle.Top;
        grpThongTin.Name = "grpThongTin";
        grpThongTin.Size = new Size(1000, 168);
        grpThongTin.Text = "Thông tin khoa";
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
        dgv.Columns.AddRange(new DataGridViewColumn[] { colMa, colTen, colMoTa, colSoPhong, colSoBacSi });
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 9;
        dgv.SelectionChanged += dgv_SelectionChanged;
        //
        // các cột
        //
        colMa.DataPropertyName = "MaKhoa"; colMa.HeaderText = "Mã"; colMa.Name = "colMa"; colMa.FillWeight = 60;
        colTen.DataPropertyName = "TenKhoa"; colTen.HeaderText = "Tên khoa"; colTen.Name = "colTen"; colTen.FillWeight = 160;
        colMoTa.DataPropertyName = "MoTa"; colMoTa.HeaderText = "Mô tả"; colMoTa.Name = "colMoTa"; colMoTa.FillWeight = 300;
        colSoPhong.DataPropertyName = "SoPhong"; colSoPhong.HeaderText = "Số phòng"; colSoPhong.Name = "colSoPhong"; colSoPhong.FillWeight = 60;
        colSoPhong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colSoBacSi.DataPropertyName = "SoBacSi"; colSoBacSi.HeaderText = "Số bác sĩ"; colSoBacSi.Name = "colSoBacSi"; colSoBacSi.FillWeight = 60;
        colSoBacSi.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //
        // ucKhoa
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dgv);
        Controls.Add(pnlTim);
        Controls.Add(grpThongTin);
        Name = "ucKhoa";
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
    private Label lblMoTa;
    private TextBox txtMoTa;
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
    private DataGridViewTextBoxColumn colMoTa;
    private DataGridViewTextBoxColumn colSoPhong;
    private DataGridViewTextBoxColumn colSoBacSi;
}
