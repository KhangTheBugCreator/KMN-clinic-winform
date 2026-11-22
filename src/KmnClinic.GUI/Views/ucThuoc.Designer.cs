namespace KmnClinic.GUI.Views;

partial class ucThuoc
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
        txtTim = new TextBox();
        chkSapHet = new CheckBox();
        flpNut = new FlowLayoutPanel();
        btnSua = new Button();
        btnThem = new Button();
        khung = new KmnClinic.GUI.Controls.KhungTrang();
        dgvThuoc = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colTen = new DataGridViewTextBoxColumn();
        colNsx = new DataGridViewTextBoxColumn();
        colHamLuong = new DataGridViewTextBoxColumn();
        colDvt = new DataGridViewTextBoxColumn();
        colDonGia = new DataGridViewTextBoxColumn();
        colTon = new DataGridViewTextBoxColumn();
        lblDem = new Label();
        pnlCongCu.SuspendLayout();
        flpLoc.SuspendLayout();
        flpNut.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvThuoc).BeginInit();
        SuspendLayout();
        //
        // pnlCongCu
        //
        pnlCongCu.Controls.Add(flpLoc);
        pnlCongCu.Controls.Add(flpNut);
        pnlCongCu.Dock = DockStyle.Top;
        pnlCongCu.Name = "pnlCongCu";
        pnlCongCu.Padding = new Padding(0, 0, 0, 12);
        pnlCongCu.Size = new Size(1000, 56);
        //
        // flpLoc
        //
        flpLoc.Controls.Add(txtTim);
        flpLoc.Controls.Add(chkSapHet);
        flpLoc.Dock = DockStyle.Fill;
        flpLoc.Name = "flpLoc";
        flpLoc.WrapContents = false;
        //
        // txtTim
        //
        txtTim.Margin = new Padding(0, 4, 16, 0);
        txtTim.Name = "txtTim";
        txtTim.PlaceholderText = "Tìm theo mã, tên thuốc, nhà sản xuất...";
        txtTim.Size = new Size(340, 30);
        txtTim.TextChanged += BoLoc_Changed;
        //
        // chkSapHet
        //
        chkSapHet.AutoSize = true;
        chkSapHet.ForeColor = Color.FromArgb(43, 52, 69);
        chkSapHet.Margin = new Padding(0, 8, 0, 0);
        chkSapHet.Name = "chkSapHet";
        chkSapHet.Text = "Chỉ thuốc sắp hết (tồn dưới 20)";
        chkSapHet.CheckedChanged += BoLoc_Changed;
        //
        // flpNut
        //
        flpNut.Controls.Add(btnSua);
        flpNut.Controls.Add(btnThem);
        flpNut.Dock = DockStyle.Right;
        flpNut.FlowDirection = FlowDirection.RightToLeft;
        flpNut.Name = "flpNut";
        flpNut.Size = new Size(300, 44);
        flpNut.WrapContents = false;
        //
        // btnSua
        //
        btnSua.Margin = new Padding(8, 0, 0, 0);
        btnSua.Name = "btnSua";
        btnSua.Size = new Size(110, 38);
        btnSua.Text = "Sửa";
        btnSua.Click += btnSua_Click;
        //
        // btnThem
        //
        btnThem.Margin = new Padding(0);
        btnThem.Name = "btnThem";
        btnThem.Size = new Size(140, 38);
        btnThem.Text = "+ Thêm thuốc";
        btnThem.Click += btnThem_Click;
        //
        // khung
        //
        khung.Controls.Add(dgvThuoc);
        khung.Controls.Add(lblDem);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        //
        // dgvThuoc
        //
        dgvThuoc.Columns.AddRange(new DataGridViewColumn[] { colMa, colTen, colNsx, colHamLuong, colDvt, colDonGia, colTon });
        dgvThuoc.Dock = DockStyle.Fill;
        dgvThuoc.Name = "dgvThuoc";
        dgvThuoc.CellDoubleClick += dgvThuoc_CellDoubleClick;
        dgvThuoc.CellFormatting += dgvThuoc_CellFormatting;
        //
        // các cột
        //
        colMa.DataPropertyName = "MaThuoc"; colMa.HeaderText = "Mã"; colMa.Name = "colMa"; colMa.FillWeight = 60;
        colTen.DataPropertyName = "TenThuoc"; colTen.HeaderText = "Tên thuốc"; colTen.Name = "colTen"; colTen.FillWeight = 180;
        colNsx.DataPropertyName = "Nsx"; colNsx.HeaderText = "Nhà sản xuất"; colNsx.Name = "colNsx"; colNsx.FillWeight = 120;
        colHamLuong.DataPropertyName = "HamLuong"; colHamLuong.HeaderText = "Hàm lượng"; colHamLuong.Name = "colHamLuong"; colHamLuong.FillWeight = 70;
        colDvt.DataPropertyName = "DonViTinh"; colDvt.HeaderText = "ĐVT"; colDvt.Name = "colDvt"; colDvt.FillWeight = 55;
        colDonGia.DataPropertyName = "DonGiaThuoc"; colDonGia.HeaderText = "Đơn giá"; colDonGia.Name = "colDonGia"; colDonGia.FillWeight = 80;
        colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colTon.DataPropertyName = "SoLuongTon"; colTon.HeaderText = "Tồn kho"; colTon.Name = "colTon"; colTon.FillWeight = 65;
        colTon.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //
        // lblDem
        //
        lblDem.Dock = DockStyle.Bottom;
        lblDem.ForeColor = Color.FromArgb(107, 118, 134);
        lblDem.Name = "lblDem";
        lblDem.Size = new Size(900, 30);
        lblDem.TextAlign = ContentAlignment.BottomLeft;
        //
        // ucThuoc
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(245, 249, 254);
        Controls.Add(khung);
        Controls.Add(pnlCongCu);
        Name = "ucThuoc";
        Size = new Size(1000, 600);
        pnlCongCu.ResumeLayout(false);
        flpLoc.ResumeLayout(false);
        flpLoc.PerformLayout();
        flpNut.ResumeLayout(false);
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvThuoc).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlCongCu;
    private FlowLayoutPanel flpLoc;
    private TextBox txtTim;
    private CheckBox chkSapHet;
    private FlowLayoutPanel flpNut;
    private Button btnThem;
    private Button btnSua;
    private KmnClinic.GUI.Controls.KhungTrang khung;
    private DataGridView dgvThuoc;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colTen;
    private DataGridViewTextBoxColumn colNsx;
    private DataGridViewTextBoxColumn colHamLuong;
    private DataGridViewTextBoxColumn colDvt;
    private DataGridViewTextBoxColumn colDonGia;
    private DataGridViewTextBoxColumn colTon;
    private Label lblDem;
}
