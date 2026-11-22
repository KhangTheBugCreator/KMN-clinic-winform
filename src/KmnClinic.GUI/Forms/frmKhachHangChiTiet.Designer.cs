namespace KmnClinic.GUI.Forms;

partial class frmKhachHangChiTiet
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblMa = new Label();
        txtMa = new TextBox();
        lblHoTen = new Label();
        txtHoTen = new TextBox();
        lblNgaySinh = new Label();
        dtpNgaySinh = new DateTimePicker();
        lblGioiTinh = new Label();
        cboGioiTinh = new ComboBox();
        lblSdt = new Label();
        txtSdt = new TextBox();
        lblDiaChi = new Label();
        txtDiaChi = new TextBox();
        lblLoi = new Label();
        btnLuu = new Button();
        btnHuy = new Button();
        SuspendLayout();
        //
        // lblMa
        //
        lblMa.AutoSize = true;
        lblMa.Location = new Point(14, 19);
        lblMa.Name = "lblMa";
        lblMa.Text = "Mã khách hàng:";
        //
        // txtMa
        //
        txtMa.Location = new Point(130, 16);
        txtMa.Name = "txtMa";
        txtMa.Size = new Size(120, 23);
        txtMa.TabIndex = 0;
        txtMa.ReadOnly = true;
        txtMa.TabStop = false;
        //
        // lblHoTen
        //
        lblHoTen.AutoSize = true;
        lblHoTen.Location = new Point(14, 51);
        lblHoTen.Name = "lblHoTen";
        lblHoTen.Text = "Họ tên:";
        //
        // txtHoTen
        //
        txtHoTen.Location = new Point(130, 48);
        txtHoTen.Name = "txtHoTen";
        txtHoTen.Size = new Size(260, 23);
        txtHoTen.TabIndex = 1;
        txtHoTen.MaxLength = 50;
        //
        // lblNgaySinh
        //
        lblNgaySinh.AutoSize = true;
        lblNgaySinh.Location = new Point(14, 83);
        lblNgaySinh.Name = "lblNgaySinh";
        lblNgaySinh.Text = "Ngày sinh:";
        //
        // dtpNgaySinh
        //
        dtpNgaySinh.Location = new Point(130, 80);
        dtpNgaySinh.Name = "dtpNgaySinh";
        dtpNgaySinh.Size = new Size(130, 23);
        dtpNgaySinh.TabIndex = 2;
        dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
        dtpNgaySinh.Format = DateTimePickerFormat.Custom;
        //
        // lblGioiTinh
        //
        lblGioiTinh.AutoSize = true;
        lblGioiTinh.Location = new Point(14, 115);
        lblGioiTinh.Name = "lblGioiTinh";
        lblGioiTinh.Text = "Giới tính:";
        //
        // cboGioiTinh
        //
        cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGioiTinh.Location = new Point(130, 112);
        cboGioiTinh.Name = "cboGioiTinh";
        cboGioiTinh.Size = new Size(100, 23);
        cboGioiTinh.TabIndex = 3;
        //
        // lblSdt
        //
        lblSdt.AutoSize = true;
        lblSdt.Location = new Point(14, 147);
        lblSdt.Name = "lblSdt";
        lblSdt.Text = "Số điện thoại:";
        //
        // txtSdt
        //
        txtSdt.Location = new Point(130, 144);
        txtSdt.Name = "txtSdt";
        txtSdt.Size = new Size(130, 23);
        txtSdt.TabIndex = 5;
        txtSdt.MaxLength = 10;
        //
        // lblDiaChi
        //
        lblDiaChi.AutoSize = true;
        lblDiaChi.Location = new Point(14, 179);
        lblDiaChi.Name = "lblDiaChi";
        lblDiaChi.Text = "Địa chỉ:";
        //
        // txtDiaChi
        //
        txtDiaChi.Location = new Point(130, 176);
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.Size = new Size(310, 23);
        txtDiaChi.TabIndex = 6;
        txtDiaChi.MaxLength = 100;
        //
        // lblLoi
        //
        lblLoi.ForeColor = Color.Red;
        lblLoi.Location = new Point(14, 208);
        lblLoi.Name = "lblLoi";
        lblLoi.Size = new Size(432, 20);
        //
        // btnLuu
        //
        btnLuu.Location = new Point(254, 238);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new Size(90, 28);
        btnLuu.TabIndex = 7;
        btnLuu.Text = "Lưu";
        btnLuu.Click += btnLuu_Click;
        //
        // btnHuy
        //
        btnHuy.DialogResult = DialogResult.Cancel;
        btnHuy.Location = new Point(350, 238);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new Size(90, 28);
        btnHuy.TabIndex = 8;
        btnHuy.Text = "Hủy";
        //
        // frmKhachHangChiTiet
        //
        AcceptButton = btnLuu;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnHuy;
        ClientSize = new Size(460, 278);
        Controls.AddRange(new Control[] { lblMa, txtMa, lblHoTen, txtHoTen, lblNgaySinh, dtpNgaySinh, lblGioiTinh, cboGioiTinh, lblSdt, txtSdt, lblDiaChi, txtDiaChi, lblLoi, btnLuu, btnHuy });
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmKhachHangChiTiet";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Khách hàng";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblMa;
    private TextBox txtMa;
    private Label lblHoTen;
    private TextBox txtHoTen;
    private Label lblNgaySinh;
    private DateTimePicker dtpNgaySinh;
    private Label lblGioiTinh;
    private ComboBox cboGioiTinh;
    private Label lblSdt;
    private TextBox txtSdt;
    private Label lblDiaChi;
    private TextBox txtDiaChi;
    private Label lblLoi;
    private Button btnLuu;
    private Button btnHuy;
}
