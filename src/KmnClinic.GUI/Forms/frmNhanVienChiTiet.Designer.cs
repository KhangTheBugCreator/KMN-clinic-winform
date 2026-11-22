namespace KmnClinic.GUI.Forms;

partial class frmNhanVienChiTiet
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
        lblSdt = new Label();
        txtSdt = new TextBox();
        lblChucVu = new Label();
        cboChucVu = new ComboBox();
        lblKhoa = new Label();
        cboKhoa = new ComboBox();
        lblMatKhau = new Label();
        txtMatKhau = new TextBox();
        lblGhiChuMatKhau = new Label();
        lblAnh = new Label();
        txtAnh = new TextBox();
        btnXemAnh = new Button();
        picAnh = new PictureBox();
        lblLoi = new Label();
        btnLuu = new Button();
        btnHuy = new Button();
        ((System.ComponentModel.ISupportInitialize)picAnh).BeginInit();
        SuspendLayout();
        //
        // lblMa
        //
        lblMa.AutoSize = true;
        lblMa.Location = new Point(14, 19);
        lblMa.Name = "lblMa";
        lblMa.Text = "Mã nhân viên:";
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
        txtHoTen.Size = new Size(240, 23);
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
        dtpNgaySinh.ShowCheckBox = true;
        //
        // lblSdt
        //
        lblSdt.AutoSize = true;
        lblSdt.Location = new Point(14, 115);
        lblSdt.Name = "lblSdt";
        lblSdt.Text = "Số điện thoại:";
        //
        // txtSdt
        //
        txtSdt.Location = new Point(130, 112);
        txtSdt.Name = "txtSdt";
        txtSdt.Size = new Size(130, 23);
        txtSdt.TabIndex = 3;
        txtSdt.MaxLength = 10;
        //
        // lblChucVu
        //
        lblChucVu.AutoSize = true;
        lblChucVu.Location = new Point(14, 147);
        lblChucVu.Name = "lblChucVu";
        lblChucVu.Text = "Chức vụ:";
        //
        // cboChucVu
        //
        cboChucVu.Location = new Point(130, 144);
        cboChucVu.Name = "cboChucVu";
        cboChucVu.Size = new Size(160, 23);
        cboChucVu.TabIndex = 4;
        cboChucVu.DropDownStyle = ComboBoxStyle.DropDownList;
        cboChucVu.SelectedIndexChanged += cboChucVu_SelectedIndexChanged;
        //
        // lblKhoa
        //
        lblKhoa.AutoSize = true;
        lblKhoa.Location = new Point(14, 179);
        lblKhoa.Name = "lblKhoa";
        lblKhoa.Text = "Khoa:";
        //
        // cboKhoa
        //
        cboKhoa.Location = new Point(130, 176);
        cboKhoa.Name = "cboKhoa";
        cboKhoa.Size = new Size(240, 23);
        cboKhoa.TabIndex = 5;
        cboKhoa.DropDownStyle = ComboBoxStyle.DropDownList;
        //
        // lblMatKhau
        //
        lblMatKhau.AutoSize = true;
        lblMatKhau.Location = new Point(14, 211);
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.Text = "Mật khẩu:";
        //
        // txtMatKhau
        //
        txtMatKhau.Location = new Point(130, 208);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.Size = new Size(160, 23);
        txtMatKhau.TabIndex = 6;
        txtMatKhau.MaxLength = 100;
        txtMatKhau.UseSystemPasswordChar = true;
        //
        // lblGhiChuMatKhau
        //
        lblGhiChuMatKhau.Location = new Point(300, 208);
        lblGhiChuMatKhau.Name = "lblGhiChuMatKhau";
        lblGhiChuMatKhau.TabIndex = 7;
        lblGhiChuMatKhau.AutoSize = true;
        lblGhiChuMatKhau.ForeColor = SystemColors.GrayText;
        //
        // lblAnh
        //
        lblAnh.AutoSize = true;
        lblAnh.Location = new Point(14, 243);
        lblAnh.Name = "lblAnh";
        lblAnh.Text = "Đường dẫn ảnh:";
        //
        // txtAnh
        //
        txtAnh.Location = new Point(130, 240);
        txtAnh.Name = "txtAnh";
        txtAnh.Size = new Size(400, 23);
        txtAnh.TabIndex = 8;
        txtAnh.MaxLength = 1000;
        //
        // btnXemAnh
        //
        btnXemAnh.Location = new Point(536, 240);
        btnXemAnh.Name = "btnXemAnh";
        btnXemAnh.Size = new Size(84, 23);
        btnXemAnh.TabIndex = 9;
        btnXemAnh.Text = "Xem ảnh";
        btnXemAnh.Click += btnXemAnh_Click;
        //
        // picAnh
        //
        picAnh.Name = "picAnh";
        picAnh.BorderStyle = BorderStyle.FixedSingle;
        picAnh.Location = new Point(470, 16);
        picAnh.Size = new Size(150, 180);
        picAnh.SizeMode = PictureBoxSizeMode.Zoom;
        //
        // lblLoi
        //
        lblLoi.ForeColor = Color.Red;
        lblLoi.Location = new Point(14, 276);
        lblLoi.Name = "lblLoi";
        lblLoi.Size = new Size(612, 20);
        //
        // btnLuu
        //
        btnLuu.Location = new Point(440, 306);
        btnLuu.Name = "btnLuu";
        btnLuu.Size = new Size(90, 28);
        btnLuu.TabIndex = 10;
        btnLuu.Text = "Lưu";
        btnLuu.Click += btnLuu_Click;
        //
        // btnHuy
        //
        btnHuy.DialogResult = DialogResult.Cancel;
        btnHuy.Location = new Point(536, 306);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new Size(90, 28);
        btnHuy.TabIndex = 11;
        btnHuy.Text = "Hủy";
        //
        // frmNhanVienChiTiet
        //
        AcceptButton = btnLuu;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = btnHuy;
        ClientSize = new Size(640, 346);
        Controls.AddRange(new Control[] { lblMa, txtMa, lblHoTen, txtHoTen, lblNgaySinh, dtpNgaySinh, lblSdt, txtSdt, lblChucVu, cboChucVu, lblKhoa, cboKhoa, lblMatKhau, txtMatKhau, lblGhiChuMatKhau, lblAnh, txtAnh, btnXemAnh, picAnh, lblLoi, btnLuu, btnHuy });
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmNhanVienChiTiet";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Nhân viên";
        ((System.ComponentModel.ISupportInitialize)picAnh).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblMa;
    private TextBox txtMa;
    private Label lblHoTen;
    private TextBox txtHoTen;
    private Label lblNgaySinh;
    private DateTimePicker dtpNgaySinh;
    private Label lblSdt;
    private TextBox txtSdt;
    private Label lblChucVu;
    private ComboBox cboChucVu;
    private Label lblKhoa;
    private ComboBox cboKhoa;
    private Label lblMatKhau;
    private TextBox txtMatKhau;
    private Label lblGhiChuMatKhau;
    private Label lblAnh;
    private TextBox txtAnh;
    private Button btnXemAnh;
    private PictureBox picAnh;
    private Label lblLoi;
    private Button btnLuu;
    private Button btnHuy;
}
