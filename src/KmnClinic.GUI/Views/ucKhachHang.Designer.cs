namespace KmnClinic.GUI.Views;

partial class ucKhachHang
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
        lblTim = new Label();
        txtTim = new TextBox();
        cboGioiTinh = new ComboBox();
        btnThem = new Button();
        btnSua = new Button();
        btnXoa = new Button();
        dgv = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colTen = new DataGridViewTextBoxColumn();
        colNgaySinh = new DataGridViewTextBoxColumn();
        colGioiTinh = new DataGridViewTextBoxColumn();
        colSdt = new DataGridViewTextBoxColumn();
        colDiaChi = new DataGridViewTextBoxColumn();
        colSoHen = new DataGridViewTextBoxColumn();
        colSoHoSo = new DataGridViewTextBoxColumn();
        lblDem = new Label();
        pnlCongCu.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).BeginInit();
        SuspendLayout();
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
        txtTim.TabIndex = 0;
        txtTim.TextChanged += BoLoc_Changed;
        //
        // cboGioiTinh
        //
        cboGioiTinh.Location = new Point(342, 9);
        cboGioiTinh.Name = "cboGioiTinh";
        cboGioiTinh.Size = new Size(100, 23);
        cboGioiTinh.TabIndex = 1;
        cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGioiTinh.SelectedIndexChanged += BoLoc_Changed;
        //
        // btnThem
        //
        btnThem.Location = new Point(466, 7);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(84, 27);
        btnThem.TabIndex = 2;
        btnThem.Text = "Thêm";
        btnThem.Click += btnThem_Click;
        //
        // btnSua
        //
        btnSua.Location = new Point(556, 7);
        btnSua.Name = "btnSua";
        btnSua.Size = new Size(84, 27);
        btnSua.TabIndex = 3;
        btnSua.Text = "Sửa";
        btnSua.Click += btnSua_Click;
        //
        // btnXoa
        //
        btnXoa.Location = new Point(646, 7);
        btnXoa.Name = "btnXoa";
        btnXoa.Size = new Size(84, 27);
        btnXoa.TabIndex = 4;
        btnXoa.Text = "Xóa";
        btnXoa.Click += btnXoa_Click;
        //
        // pnlCongCu
        //
        pnlCongCu.Controls.Add(lblTim);
        pnlCongCu.Controls.Add(txtTim);
        pnlCongCu.Controls.Add(cboGioiTinh);
        pnlCongCu.Controls.Add(btnThem);
        pnlCongCu.Controls.Add(btnSua);
        pnlCongCu.Controls.Add(btnXoa);
        pnlCongCu.Dock = DockStyle.Top;
        pnlCongCu.Name = "pnlCongCu";
        pnlCongCu.Size = new Size(1000, 42);
        //
        // dgv
        //
        dgv.Columns.AddRange(new DataGridViewColumn[] { colMa, colTen, colNgaySinh, colGioiTinh, colSdt, colDiaChi, colSoHen, colSoHoSo });
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 5;
        dgv.CellDoubleClick += dgv_CellDoubleClick;
        //
        // các cột
        //
        colMa.DataPropertyName = "MaKh"; colMa.HeaderText = "Mã"; colMa.Name = "colMa"; colMa.FillWeight = 55;
        colTen.DataPropertyName = "HoTenKh"; colTen.HeaderText = "Họ tên"; colTen.Name = "colTen"; colTen.FillWeight = 150;
        colNgaySinh.DataPropertyName = "NgaySinhKh"; colNgaySinh.HeaderText = "Ngày sinh"; colNgaySinh.Name = "colNgaySinh"; colNgaySinh.FillWeight = 75;
        colNgaySinh.DefaultCellStyle.Format = "dd/MM/yyyy";
        colGioiTinh.DataPropertyName = "GioiTinhKh"; colGioiTinh.HeaderText = "Giới tính"; colGioiTinh.Name = "colGioiTinh"; colGioiTinh.FillWeight = 55;
        colSdt.DataPropertyName = "SdtKh"; colSdt.HeaderText = "Số điện thoại"; colSdt.Name = "colSdt"; colSdt.FillWeight = 85;
        colDiaChi.DataPropertyName = "DiaChiKh"; colDiaChi.HeaderText = "Địa chỉ"; colDiaChi.Name = "colDiaChi"; colDiaChi.FillWeight = 220;
        colSoHen.DataPropertyName = "SoLichHen"; colSoHen.HeaderText = "Lịch hẹn"; colSoHen.Name = "colSoHen"; colSoHen.FillWeight = 55;
        colSoHen.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colSoHoSo.DataPropertyName = "SoHoSo"; colSoHoSo.HeaderText = "Hồ sơ"; colSoHoSo.Name = "colSoHoSo"; colSoHoSo.FillWeight = 50;
        colSoHoSo.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //
        // lblDem
        //
        lblDem.Dock = DockStyle.Bottom;
        lblDem.Name = "lblDem";
        lblDem.Size = new Size(1000, 26);
        lblDem.TextAlign = ContentAlignment.MiddleLeft;
        //
        // ucKhachHang
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dgv);
        Controls.Add(lblDem);
        Controls.Add(pnlCongCu);
        Name = "ucKhachHang";
        Padding = new Padding(8);
        Size = new Size(1000, 600);
        pnlCongCu.ResumeLayout(false);
        pnlCongCu.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)dgv).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlCongCu;
    private Label lblTim;
    private TextBox txtTim;
    private ComboBox cboGioiTinh;
    private Button btnThem;
    private Button btnSua;
    private Button btnXoa;
    private DataGridView dgv;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colTen;
    private DataGridViewTextBoxColumn colNgaySinh;
    private DataGridViewTextBoxColumn colGioiTinh;
    private DataGridViewTextBoxColumn colSdt;
    private DataGridViewTextBoxColumn colDiaChi;
    private DataGridViewTextBoxColumn colSoHen;
    private DataGridViewTextBoxColumn colSoHoSo;
    private Label lblDem;
}
