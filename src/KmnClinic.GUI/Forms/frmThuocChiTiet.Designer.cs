namespace KmnClinic.GUI.Forms;

partial class frmThuocChiTiet
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
        lblMa = new Label();
        txtMa = new TextBox();
        lblTen = new Label();
        txtTen = new TextBox();
        lblNsx = new Label();
        txtNsx = new TextBox();
        lblHamLuong = new Label();
        numHamLuong = new NumericUpDown();
        lblDvt = new Label();
        cboDvt = new ComboBox();
        lblDonGia = new Label();
        numDonGia = new NumericUpDown();
        lblTon = new Label();
        numTon = new NumericUpDown();
        chkXacNhanTon = new CheckBox();
        lblMoTa = new Label();
        txtMoTa = new TextBox();
        pnlNut = new FlowLayoutPanel();
        btnHuy = new Button();
        btnLuu = new Button();
        tlp.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)numHamLuong).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numDonGia).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numTon).BeginInit();
        pnlNut.SuspendLayout();
        SuspendLayout();
        //
        // tlp
        //
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 140F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblMa, 0, 0);
        tlp.Controls.Add(txtMa, 1, 0);
        tlp.Controls.Add(lblTen, 0, 1);
        tlp.Controls.Add(txtTen, 1, 1);
        tlp.Controls.Add(lblNsx, 0, 2);
        tlp.Controls.Add(txtNsx, 1, 2);
        tlp.Controls.Add(lblHamLuong, 0, 3);
        tlp.Controls.Add(numHamLuong, 1, 3);
        tlp.Controls.Add(lblDvt, 0, 4);
        tlp.Controls.Add(cboDvt, 1, 4);
        tlp.Controls.Add(lblDonGia, 0, 5);
        tlp.Controls.Add(numDonGia, 1, 5);
        tlp.Controls.Add(lblTon, 0, 6);
        tlp.Controls.Add(numTon, 1, 6);
        tlp.Controls.Add(chkXacNhanTon, 1, 7);
        tlp.Controls.Add(lblMoTa, 0, 8);
        tlp.Controls.Add(txtMoTa, 1, 8);
        tlp.Dock = DockStyle.Fill;
        tlp.Name = "tlp";
        tlp.Padding = new Padding(24, 20, 24, 8);
        tlp.RowCount = 9;
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // nhãn
        //
        lblMa.Anchor = AnchorStyles.Left; lblMa.AutoSize = true; lblMa.Text = "Mã thuốc";
        lblTen.Anchor = AnchorStyles.Left; lblTen.AutoSize = true; lblTen.Text = "Tên thuốc *";
        lblNsx.Anchor = AnchorStyles.Left; lblNsx.AutoSize = true; lblNsx.Text = "Nhà sản xuất";
        lblHamLuong.Anchor = AnchorStyles.Left; lblHamLuong.AutoSize = true; lblHamLuong.Text = "Hàm lượng";
        lblDvt.Anchor = AnchorStyles.Left; lblDvt.AutoSize = true; lblDvt.Text = "Đơn vị tính *";
        lblDonGia.Anchor = AnchorStyles.Left; lblDonGia.AutoSize = true; lblDonGia.Text = "Đơn giá (đ) *";
        lblTon.Anchor = AnchorStyles.Left; lblTon.AutoSize = true; lblTon.Text = "Số lượng tồn";
        lblMoTa.AutoSize = true; lblMoTa.Margin = new Padding(3, 10, 3, 0); lblMoTa.Text = "Mô tả";
        //
        // ô nhập
        //
        txtMa.Anchor = AnchorStyles.Left | AnchorStyles.Right; txtMa.Name = "txtMa"; txtMa.ReadOnly = true;
        txtTen.Anchor = AnchorStyles.Left | AnchorStyles.Right; txtTen.MaxLength = 100; txtTen.Name = "txtTen";
        txtNsx.Anchor = AnchorStyles.Left | AnchorStyles.Right; txtNsx.MaxLength = 100; txtNsx.Name = "txtNsx";
        numHamLuong.Anchor = AnchorStyles.Left; numHamLuong.DecimalPlaces = 2; numHamLuong.Maximum = 100000; numHamLuong.Name = "numHamLuong"; numHamLuong.Size = new Size(160, 30);
        cboDvt.Anchor = AnchorStyles.Left; cboDvt.Name = "cboDvt"; cboDvt.Size = new Size(160, 30);
        cboDvt.Items.AddRange(new object[] { "Viên", "Chai", "Gói", "Ống", "Tuýp", "Lọ", "Hộp" });
        numDonGia.Anchor = AnchorStyles.Left; numDonGia.Increment = 500; numDonGia.Maximum = 100000000; numDonGia.Name = "numDonGia"; numDonGia.Size = new Size(160, 30); numDonGia.ThousandsSeparator = true;
        numTon.Anchor = AnchorStyles.Left; numTon.Maximum = 1000000; numTon.Name = "numTon"; numTon.Size = new Size(160, 30); numTon.ThousandsSeparator = true;
        chkXacNhanTon.Anchor = AnchorStyles.Left; chkXacNhanTon.AutoSize = true; chkXacNhanTon.ForeColor = Color.FromArgb(229, 83, 75);
        chkXacNhanTon.Name = "chkXacNhanTon"; chkXacNhanTon.Text = "Tôi xác nhận sửa trực tiếp số lượng tồn (kiểm kho)";
        chkXacNhanTon.CheckedChanged += chkXacNhanTon_CheckedChanged;
        txtMoTa.Dock = DockStyle.Fill; txtMoTa.MaxLength = 500; txtMoTa.Multiline = true; txtMoTa.Name = "txtMoTa"; txtMoTa.ScrollBars = ScrollBars.Vertical;
        //
        // pnlNut
        //
        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Dock = DockStyle.Bottom;
        pnlNut.FlowDirection = FlowDirection.RightToLeft;
        pnlNut.Name = "pnlNut";
        pnlNut.Padding = new Padding(24, 8, 24, 16);
        pnlNut.Size = new Size(560, 64);
        btnHuy.DialogResult = DialogResult.Cancel; btnHuy.Name = "btnHuy"; btnHuy.Size = new Size(100, 38); btnHuy.Text = "Hủy";
        btnLuu.Name = "btnLuu"; btnLuu.Size = new Size(120, 38); btnLuu.Text = "Lưu"; btnLuu.Click += btnLuu_Click;
        //
        // frmThuocChiTiet
        //
        AcceptButton = btnLuu;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        CancelButton = btnHuy;
        ClientSize = new Size(560, 560);
        Controls.Add(tlp);
        Controls.Add(pnlNut);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmThuocChiTiet";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Thuốc";
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)numHamLuong).EndInit();
        ((System.ComponentModel.ISupportInitialize)numDonGia).EndInit();
        ((System.ComponentModel.ISupportInitialize)numTon).EndInit();
        pnlNut.ResumeLayout(false);
        ResumeLayout(false);
    }

    private TableLayoutPanel tlp;
    private Label lblMa;
    private TextBox txtMa;
    private Label lblTen;
    private TextBox txtTen;
    private Label lblNsx;
    private TextBox txtNsx;
    private Label lblHamLuong;
    private NumericUpDown numHamLuong;
    private Label lblDvt;
    private ComboBox cboDvt;
    private Label lblDonGia;
    private NumericUpDown numDonGia;
    private Label lblTon;
    private NumericUpDown numTon;
    private CheckBox chkXacNhanTon;
    private Label lblMoTa;
    private TextBox txtMoTa;
    private FlowLayoutPanel pnlNut;
    private Button btnHuy;
    private Button btnLuu;
}
