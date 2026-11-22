namespace KmnClinic.GUI.Forms;

partial class frmXepLich
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
        lblBacSi = new Label();
        cboBacSi = new ComboBox();
        lblPhong = new Label();
        cboPhong = new ComboBox();
        lblCa = new Label();
        cboCa = new ComboBox();
        lblTuNgay = new Label();
        dtpTuNgay = new DateTimePicker();
        lblDenNgay = new Label();
        dtpDenNgay = new DateTimePicker();
        lblThu = new Label();
        flpThu = new FlowLayoutPanel();
        chkT2 = new CheckBox();
        chkT3 = new CheckBox();
        chkT4 = new CheckBox();
        chkT5 = new CheckBox();
        chkT6 = new CheckBox();
        chkT7 = new CheckBox();
        chkCN = new CheckBox();
        lblLoi = new Label();
        pnlNut = new FlowLayoutPanel();
        btnHuy = new Button();
        btnLuu = new Button();
        tlp.SuspendLayout();
        flpThu.SuspendLayout();
        pnlNut.SuspendLayout();
        SuspendLayout();
        //
        // tlp
        //
        tlp.ColumnCount = 2;
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
        tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        tlp.Controls.Add(lblBacSi, 0, 0);
        tlp.Controls.Add(cboBacSi, 1, 0);
        tlp.Controls.Add(lblPhong, 0, 1);
        tlp.Controls.Add(cboPhong, 1, 1);
        tlp.Controls.Add(lblCa, 0, 2);
        tlp.Controls.Add(cboCa, 1, 2);
        tlp.Controls.Add(lblTuNgay, 0, 3);
        tlp.Controls.Add(dtpTuNgay, 1, 3);
        tlp.Controls.Add(lblDenNgay, 0, 4);
        tlp.Controls.Add(dtpDenNgay, 1, 4);
        tlp.Controls.Add(lblThu, 0, 5);
        tlp.Controls.Add(flpThu, 1, 5);
        tlp.Controls.Add(lblLoi, 1, 6);
        tlp.Dock = DockStyle.Fill;
        tlp.Name = "tlp";
        tlp.Padding = new Padding(16, 16, 16, 4);
        tlp.RowCount = 7;
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
        tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // nhãn và ô nhập
        //
        lblBacSi.Anchor = AnchorStyles.Left; lblBacSi.AutoSize = true; lblBacSi.Name = "lblBacSi"; lblBacSi.Text = "Bác sĩ:";
        cboBacSi.Anchor = AnchorStyles.Left | AnchorStyles.Right; cboBacSi.DropDownStyle = ComboBoxStyle.DropDownList; cboBacSi.Name = "cboBacSi";
        lblPhong.Anchor = AnchorStyles.Left; lblPhong.AutoSize = true; lblPhong.Name = "lblPhong"; lblPhong.Text = "Phòng:";
        cboPhong.Anchor = AnchorStyles.Left | AnchorStyles.Right; cboPhong.DropDownStyle = ComboBoxStyle.DropDownList; cboPhong.Name = "cboPhong";
        lblCa.Anchor = AnchorStyles.Left; lblCa.AutoSize = true; lblCa.Name = "lblCa"; lblCa.Text = "Ca:";
        cboCa.Anchor = AnchorStyles.Left | AnchorStyles.Right; cboCa.DropDownStyle = ComboBoxStyle.DropDownList; cboCa.Name = "cboCa";
        lblTuNgay.Anchor = AnchorStyles.Left; lblTuNgay.AutoSize = true; lblTuNgay.Name = "lblTuNgay"; lblTuNgay.Text = "Từ ngày:";
        dtpTuNgay.Anchor = AnchorStyles.Left; dtpTuNgay.Format = DateTimePickerFormat.Custom; dtpTuNgay.CustomFormat = "dd/MM/yyyy"; dtpTuNgay.Name = "dtpTuNgay"; dtpTuNgay.Size = new Size(170, 23);
        lblDenNgay.Anchor = AnchorStyles.Left; lblDenNgay.AutoSize = true; lblDenNgay.Name = "lblDenNgay"; lblDenNgay.Text = "Đến ngày:";
        dtpDenNgay.Anchor = AnchorStyles.Left; dtpDenNgay.Format = DateTimePickerFormat.Custom; dtpDenNgay.CustomFormat = "dd/MM/yyyy"; dtpDenNgay.Name = "dtpDenNgay"; dtpDenNgay.Size = new Size(170, 23);
        lblThu.Anchor = AnchorStyles.Left; lblThu.AutoSize = true; lblThu.Name = "lblThu"; lblThu.Text = "Các thứ:";
        flpThu.Controls.Add(chkT2);
        flpThu.Controls.Add(chkT3);
        flpThu.Controls.Add(chkT4);
        flpThu.Controls.Add(chkT5);
        flpThu.Controls.Add(chkT6);
        flpThu.Controls.Add(chkT7);
        flpThu.Controls.Add(chkCN);
        flpThu.Dock = DockStyle.Fill; flpThu.Margin = new Padding(0); flpThu.Name = "flpThu"; flpThu.WrapContents = false;
        chkT2.AutoSize = true; chkT2.Name = "chkT2"; chkT2.Text = "T2"; chkT2.Checked = true;
        chkT3.AutoSize = true; chkT3.Name = "chkT3"; chkT3.Text = "T3"; chkT3.Checked = true;
        chkT4.AutoSize = true; chkT4.Name = "chkT4"; chkT4.Text = "T4"; chkT4.Checked = true;
        chkT5.AutoSize = true; chkT5.Name = "chkT5"; chkT5.Text = "T5"; chkT5.Checked = true;
        chkT6.AutoSize = true; chkT6.Name = "chkT6"; chkT6.Text = "T6"; chkT6.Checked = true;
        chkT7.AutoSize = true; chkT7.Name = "chkT7"; chkT7.Text = "T7"; chkT7.Checked = true;
        chkCN.AutoSize = true; chkCN.Name = "chkCN"; chkCN.Text = "CN"; chkCN.Checked = true;
        cboBacSi.SelectedIndexChanged += cboBacSi_SelectedIndexChanged;
        dtpTuNgay.ValueChanged += dtpTuNgay_ValueChanged;
        lblLoi.Dock = DockStyle.Fill; lblLoi.ForeColor = Color.DarkRed; lblLoi.Name = "lblLoi";
        //
        // pnlNut
        //
        pnlNut.Controls.Add(btnHuy);
        pnlNut.Controls.Add(btnLuu);
        pnlNut.Dock = DockStyle.Bottom;
        pnlNut.FlowDirection = FlowDirection.RightToLeft;
        pnlNut.Name = "pnlNut";
        pnlNut.Padding = new Padding(12, 6, 12, 6);
        pnlNut.Size = new Size(460, 44);
        btnHuy.DialogResult = DialogResult.Cancel; btnHuy.Name = "btnHuy"; btnHuy.Size = new Size(90, 28); btnHuy.Text = "Hủy";
        btnLuu.Name = "btnLuu"; btnLuu.Size = new Size(90, 28); btnLuu.Text = "Lưu"; btnLuu.Click += btnLuu_Click;
        //
        // frmXepLich
        //
        AcceptButton = btnLuu;
        CancelButton = btnHuy;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(460, 330);
        Controls.Add(tlp);
        Controls.Add(pnlNut);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmXepLich";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Xếp lịch làm việc";
        tlp.ResumeLayout(false);
        tlp.PerformLayout();
        flpThu.ResumeLayout(false);
        flpThu.PerformLayout();
        pnlNut.ResumeLayout(false);
        ResumeLayout(false);
    }

    private TableLayoutPanel tlp;
    private Label lblBacSi;
    private ComboBox cboBacSi;
    private Label lblPhong;
    private ComboBox cboPhong;
    private Label lblCa;
    private ComboBox cboCa;
    private Label lblTuNgay;
    private DateTimePicker dtpTuNgay;
    private Label lblDenNgay;
    private DateTimePicker dtpDenNgay;
    private Label lblThu;
    private FlowLayoutPanel flpThu;
    private CheckBox chkT2;
    private CheckBox chkT3;
    private CheckBox chkT4;
    private CheckBox chkT5;
    private CheckBox chkT6;
    private CheckBox chkT7;
    private CheckBox chkCN;
    private Label lblLoi;
    private FlowLayoutPanel pnlNut;
    private Button btnHuy;
    private Button btnLuu;
}
