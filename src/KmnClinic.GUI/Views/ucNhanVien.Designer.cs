namespace KmnClinic.GUI.Views;

partial class ucNhanVien
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
        cboChucVu = new ComboBox();
        btnThem = new Button();
        btnSua = new Button();
        btnXoa = new Button();
        dgv = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colTen = new DataGridViewTextBoxColumn();
        colNgaySinh = new DataGridViewTextBoxColumn();
        colSdt = new DataGridViewTextBoxColumn();
        colChucVu = new DataGridViewTextBoxColumn();
        colKhoa = new DataGridViewTextBoxColumn();
        colLienKet = new DataGridViewTextBoxColumn();
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
        // cboChucVu
        //
        cboChucVu.Location = new Point(342, 9);
        cboChucVu.Name = "cboChucVu";
        cboChucVu.Size = new Size(150, 23);
        cboChucVu.TabIndex = 1;
        cboChucVu.DropDownStyle = ComboBoxStyle.DropDownList;
        cboChucVu.SelectedIndexChanged += BoLoc_Changed;
        //
        // btnThem
        //
        btnThem.Location = new Point(516, 7);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(84, 27);
        btnThem.TabIndex = 2;
        btnThem.Text = "Thêm";
        btnThem.Click += btnThem_Click;
        //
        // btnSua
        //
        btnSua.Location = new Point(606, 7);
        btnSua.Name = "btnSua";
        btnSua.Size = new Size(84, 27);
        btnSua.TabIndex = 3;
        btnSua.Text = "Sửa";
        btnSua.Click += btnSua_Click;
        //
        // btnXoa
        //
        btnXoa.Location = new Point(696, 7);
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
        pnlCongCu.Controls.Add(cboChucVu);
        pnlCongCu.Controls.Add(btnThem);
        pnlCongCu.Controls.Add(btnSua);
        pnlCongCu.Controls.Add(btnXoa);
        pnlCongCu.Dock = DockStyle.Top;
        pnlCongCu.Name = "pnlCongCu";
        pnlCongCu.Size = new Size(1000, 42);
        //
        // dgv
        //
        dgv.Columns.AddRange(new DataGridViewColumn[] { colMa, colTen, colNgaySinh, colSdt, colChucVu, colKhoa, colLienKet });
        dgv.Dock = DockStyle.Fill;
        dgv.Name = "dgv";
        dgv.TabIndex = 5;
        dgv.CellDoubleClick += dgv_CellDoubleClick;
        //
        // các cột
        //
        colMa.DataPropertyName = "MaNv"; colMa.HeaderText = "Mã"; colMa.Name = "colMa"; colMa.FillWeight = 55;
        colTen.DataPropertyName = "HoTenNv"; colTen.HeaderText = "Họ tên"; colTen.Name = "colTen"; colTen.FillWeight = 160;
        colNgaySinh.DataPropertyName = "NgaySinhNv"; colNgaySinh.HeaderText = "Ngày sinh"; colNgaySinh.Name = "colNgaySinh"; colNgaySinh.FillWeight = 75;
        colNgaySinh.DefaultCellStyle.Format = "dd/MM/yyyy";
        colSdt.DataPropertyName = "SdtNv"; colSdt.HeaderText = "Số điện thoại"; colSdt.Name = "colSdt"; colSdt.FillWeight = 85;
        colChucVu.DataPropertyName = "ChucVu"; colChucVu.HeaderText = "Chức vụ"; colChucVu.Name = "colChucVu"; colChucVu.FillWeight = 85;
        colKhoa.DataPropertyName = "TenKhoa"; colKhoa.HeaderText = "Khoa"; colKhoa.Name = "colKhoa"; colKhoa.FillWeight = 120;
        colLienKet.DataPropertyName = "SoLienKet"; colLienKet.HeaderText = "Dữ liệu liên quan"; colLienKet.Name = "colLienKet"; colLienKet.FillWeight = 80;
        colLienKet.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //
        // lblDem
        //
        lblDem.Dock = DockStyle.Bottom;
        lblDem.Name = "lblDem";
        lblDem.Size = new Size(1000, 26);
        lblDem.TextAlign = ContentAlignment.MiddleLeft;
        //
        // ucNhanVien
        //
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(dgv);
        Controls.Add(lblDem);
        Controls.Add(pnlCongCu);
        Name = "ucNhanVien";
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
    private ComboBox cboChucVu;
    private Button btnThem;
    private Button btnSua;
    private Button btnXoa;
    private DataGridView dgv;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colTen;
    private DataGridViewTextBoxColumn colNgaySinh;
    private DataGridViewTextBoxColumn colSdt;
    private DataGridViewTextBoxColumn colChucVu;
    private DataGridViewTextBoxColumn colKhoa;
    private DataGridViewTextBoxColumn colLienKet;
    private Label lblDem;
}
