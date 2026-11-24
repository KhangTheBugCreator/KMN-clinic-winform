namespace KmnClinic.GUI.Views;

partial class ucBaoCao
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
        cboLoai = new ComboBox();
        lblTu = new Label();
        dtpTu = new DateTimePicker();
        lblDen = new Label();
        dtpDen = new DateTimePicker();
        btnXem = new Button();
        btnXuat = new Button();
        khung = new Panel();
        dgvBaoCao = new DataGridView();
        lblTong = new Label();
        pnlCongCu.SuspendLayout();
        flpLoc.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBaoCao).BeginInit();
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
        flpLoc.Controls.Add(cboLoai);
        flpLoc.Controls.Add(lblTu);
        flpLoc.Controls.Add(dtpTu);
        flpLoc.Controls.Add(lblDen);
        flpLoc.Controls.Add(dtpDen);
        flpLoc.Controls.Add(btnXem);
        flpLoc.Controls.Add(btnXuat);
        flpLoc.Dock = DockStyle.Fill;
        flpLoc.Name = "flpLoc";
        flpLoc.WrapContents = false;
        cboLoai.DropDownStyle = ComboBoxStyle.DropDownList; cboLoai.Margin = new Padding(0, 4, 16, 0); cboLoai.Name = "cboLoai"; cboLoai.Size = new Size(280, 30);
        cboLoai.SelectedIndexChanged += cboLoai_SelectedIndexChanged;
        lblTu.AutoSize = true; lblTu.Margin = new Padding(0, 8, 4, 0); lblTu.Name = "lblTu"; lblTu.Text = "Từ";
        dtpTu.Format = DateTimePickerFormat.Custom; dtpTu.CustomFormat = "dd/MM/yyyy"; dtpTu.Margin = new Padding(0, 4, 8, 0);
        dtpTu.Name = "dtpTu"; dtpTu.Size = new Size(140, 30);
        lblDen.AutoSize = true; lblDen.Margin = new Padding(0, 8, 4, 0); lblDen.Name = "lblDen"; lblDen.Text = "đến";
        dtpDen.Format = DateTimePickerFormat.Custom; dtpDen.CustomFormat = "dd/MM/yyyy"; dtpDen.Margin = new Padding(0, 4, 8, 0);
        dtpDen.Name = "dtpDen"; dtpDen.Size = new Size(140, 30);
        btnXem.Margin = new Padding(8, 2, 0, 0); btnXem.Name = "btnXem"; btnXem.Size = new Size(110, 30); btnXem.Text = "Xem"; btnXem.Click += btnXem_Click;
        btnXuat.Margin = new Padding(8, 2, 0, 0); btnXuat.Name = "btnXuat"; btnXuat.Size = new Size(150, 30); btnXuat.Text = "Xuất Excel (CSV)"; btnXuat.Click += btnXuat_Click;
        //
        // khung
        //
        khung.Controls.Add(dgvBaoCao);
        khung.Controls.Add(lblTong);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        dgvBaoCao.Dock = DockStyle.Fill;
        dgvBaoCao.Name = "dgvBaoCao";
        dgvBaoCao.DataBindingComplete += dgvBaoCao_DataBindingComplete;
        lblTong.Dock = DockStyle.Bottom; lblTong.Name = "lblTong";
        lblTong.Size = new Size(900, 30); lblTong.TextAlign = ContentAlignment.BottomLeft;
        //
        // ucBaoCao
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(khung);
        Controls.Add(pnlCongCu);
        Name = "ucBaoCao";
        Size = new Size(1100, 600);
        pnlCongCu.ResumeLayout(false);
        flpLoc.ResumeLayout(false);
        flpLoc.PerformLayout();
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvBaoCao).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlCongCu;
    private FlowLayoutPanel flpLoc;
    private ComboBox cboLoai;
    private Label lblTu;
    private DateTimePicker dtpTu;
    private Label lblDen;
    private DateTimePicker dtpDen;
    private Button btnXem;
    private Button btnXuat;
    private Panel khung;
    private DataGridView dgvBaoCao;
    private Label lblTong;
}
