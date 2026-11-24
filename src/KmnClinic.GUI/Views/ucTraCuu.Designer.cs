namespace KmnClinic.GUI.Views;

partial class ucTraCuu
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
        cboTrangThai = new ComboBox();
        txtTim = new TextBox();
        btnXem = new Button();
        khung = new Panel();
        dgvKetQua = new DataGridView();
        lblDem = new Label();
        pnlCongCu.SuspendLayout();
        flpLoc.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvKetQua).BeginInit();
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
        flpLoc.Controls.Add(cboTrangThai);
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
        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList; cboTrangThai.Margin = new Padding(0, 4, 16, 0); cboTrangThai.Name = "cboTrangThai"; cboTrangThai.Size = new Size(160, 30);
        cboTrangThai.SelectedIndexChanged += BoLoc_Changed;
        txtTim.Margin = new Padding(0, 4, 0, 0); txtTim.Name = "txtTim"; txtTim.PlaceholderText = "Mã, tên, SĐT khách, bác sĩ..."; txtTim.Size = new Size(240, 30);
        txtTim.TextChanged += BoLoc_Changed;
        btnXem.Margin = new Padding(8, 2, 0, 0); btnXem.Name = "btnXem"; btnXem.Size = new Size(120, 30); btnXem.Text = "Xem chi tiết"; btnXem.Click += btnXem_Click;
        //
        // khung
        //
        khung.Controls.Add(dgvKetQua);
        khung.Controls.Add(lblDem);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        dgvKetQua.Dock = DockStyle.Fill;
        dgvKetQua.Name = "dgvKetQua";
        dgvKetQua.CellDoubleClick += dgvKetQua_CellDoubleClick;
        lblDem.Dock = DockStyle.Bottom; lblDem.Name = "lblDem";
        lblDem.Size = new Size(900, 30); lblDem.TextAlign = ContentAlignment.BottomLeft;
        //
        // ucTraCuu
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(khung);
        Controls.Add(pnlCongCu);
        Name = "ucTraCuu";
        Size = new Size(1100, 600);
        pnlCongCu.ResumeLayout(false);
        flpLoc.ResumeLayout(false);
        flpLoc.PerformLayout();
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvKetQua).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlCongCu;
    private FlowLayoutPanel flpLoc;
    private Label lblTu;
    private DateTimePicker dtpTu;
    private Label lblDen;
    private DateTimePicker dtpDen;
    private CheckBox chkMoiNgay;
    private ComboBox cboTrangThai;
    private TextBox txtTim;
    private Button btnXem;
    private Panel khung;
    private DataGridView dgvKetQua;
    private Label lblDem;
}
