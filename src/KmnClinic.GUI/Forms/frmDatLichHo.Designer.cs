namespace KmnClinic.GUI.Forms;

partial class frmDatLichHo
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblBuoc1 = new Label();
        txtSdt = new TextBox();
        btnTim = new Button();
        lblKetQua = new Label();
        lblHoTen = new Label();
        txtHoTen = new TextBox();
        lblGioiTinh = new Label();
        cboGioiTinh = new ComboBox();
        lblNgaySinh = new Label();
        dtpNgaySinh = new DateTimePicker();
        lblBuoc4 = new Label();
        txtTrieuChung = new TextBox();
        lblBuoc2 = new Label();
        cboKhoa = new ComboBox();
        cboBacSi = new ComboBox();
        lblBuoc3 = new Label();
        dtpNgayKham = new DateTimePicker();
        lstCa = new ListBox();
        lblTomTat = new Label();
        btnDat = new Button();
        btnDong = new Button();
        SuspendLayout();
        //
        // Bước 1: khách hàng
        //
        lblBuoc1.AutoSize = true; lblBuoc1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblBuoc1.Location = new Point(24, 20); lblBuoc1.Name = "lblBuoc1"; lblBuoc1.Text = "1. Khách hàng";
        txtSdt.Location = new Point(24, 56); txtSdt.MaxLength = 10; txtSdt.Name = "txtSdt"; txtSdt.PlaceholderText = "Số điện thoại khách gọi đến"; txtSdt.Size = new Size(280, 30); txtSdt.TabIndex = 0;
        btnTim.Location = new Point(314, 54); btnTim.Name = "btnTim"; btnTim.Size = new Size(110, 34); btnTim.TabIndex = 1; btnTim.Text = "Tìm khách"; btnTim.Click += btnTim_Click; txtSdt.TextChanged += txtSdt_TextChanged;
        lblKetQua.Location = new Point(24, 94); lblKetQua.Name = "lblKetQua"; lblKetQua.Size = new Size(400, 44);
        lblKetQua.Text = "Nhập SĐT rồi bấm Tìm. Nếu chưa có, điền thông tin bên dưới để tạo khách mới.";
        lblHoTen.AutoSize = true; lblHoTen.Location = new Point(24, 146); lblHoTen.Name = "lblHoTen"; lblHoTen.Text = "Họ tên";
        txtHoTen.Location = new Point(124, 142); txtHoTen.Name = "txtHoTen"; txtHoTen.Size = new Size(300, 30); txtHoTen.TabIndex = 2;
        lblGioiTinh.AutoSize = true; lblGioiTinh.Location = new Point(24, 186); lblGioiTinh.Name = "lblGioiTinh"; lblGioiTinh.Text = "Giới tính";
        cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList; cboGioiTinh.Items.AddRange(new object[] { "Nam", "Nữ", "Khác" });
        cboGioiTinh.Location = new Point(124, 182); cboGioiTinh.Name = "cboGioiTinh"; cboGioiTinh.Size = new Size(110, 30); cboGioiTinh.TabIndex = 3;
        lblNgaySinh.AutoSize = true; lblNgaySinh.Location = new Point(24, 226); lblNgaySinh.Name = "lblNgaySinh"; lblNgaySinh.Text = "Ngày sinh";
        dtpNgaySinh.CustomFormat = "dd/MM/yyyy"; dtpNgaySinh.Format = DateTimePickerFormat.Custom;
        dtpNgaySinh.Location = new Point(124, 222); dtpNgaySinh.Name = "dtpNgaySinh"; dtpNgaySinh.Size = new Size(170, 30); dtpNgaySinh.TabIndex = 4;
        //
        // Bước 4: triệu chứng
        //
        lblBuoc4.AutoSize = true; lblBuoc4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblBuoc4.Location = new Point(24, 280); lblBuoc4.Name = "lblBuoc4"; lblBuoc4.Text = "4. Triệu chứng ban đầu";
        txtTrieuChung.Location = new Point(24, 314); txtTrieuChung.MaxLength = 255; txtTrieuChung.Multiline = true; txtTrieuChung.Name = "txtTrieuChung";
        txtTrieuChung.PlaceholderText = "Khách mô tả qua điện thoại (không bắt buộc)"; txtTrieuChung.Size = new Size(400, 120); txtTrieuChung.TabIndex = 9;
        //
        // Bước 2: khoa, bác sĩ
        //
        lblBuoc2.AutoSize = true; lblBuoc2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblBuoc2.Location = new Point(464, 20); lblBuoc2.Name = "lblBuoc2"; lblBuoc2.Text = "2. Chuyên khoa và bác sĩ";
        cboKhoa.DropDownStyle = ComboBoxStyle.DropDownList; cboKhoa.Location = new Point(464, 56); cboKhoa.Name = "cboKhoa"; cboKhoa.Size = new Size(400, 30); cboKhoa.TabIndex = 5;
        cboKhoa.SelectedIndexChanged += cboKhoa_SelectedIndexChanged;
        cboBacSi.DropDownStyle = ComboBoxStyle.DropDownList; cboBacSi.Location = new Point(464, 96); cboBacSi.Name = "cboBacSi"; cboBacSi.Size = new Size(400, 30); cboBacSi.TabIndex = 6;
        cboBacSi.SelectedIndexChanged += LuaChon_Changed;
        //
        // Bước 3: ngày, ca
        //
        lblBuoc3.AutoSize = true; lblBuoc3.Font = new Font("Segoe UI", 9F, FontStyle.Bold);         lblBuoc3.Location = new Point(464, 146); lblBuoc3.Name = "lblBuoc3"; lblBuoc3.Text = "3. Ngày khám và ca còn đặt được";
        dtpNgayKham.CustomFormat = "dddd, dd/MM/yyyy"; dtpNgayKham.Format = DateTimePickerFormat.Custom;
        dtpNgayKham.Location = new Point(464, 182); dtpNgayKham.Name = "dtpNgayKham"; dtpNgayKham.Size = new Size(260, 30); dtpNgayKham.TabIndex = 7;
        dtpNgayKham.ValueChanged += LuaChon_Changed;
        lstCa.IntegralHeight = false;
        lstCa.Location = new Point(464, 222); lstCa.Name = "lstCa"; lstCa.Size = new Size(400, 212); lstCa.TabIndex = 8;
        lstCa.SelectedIndexChanged += LuaChon_Changed;
        //
        // Tóm tắt, nút
        //
        lblTomTat.Location = new Point(24, 456); lblTomTat.Name = "lblTomTat"; lblTomTat.Padding = new Padding(10); lblTomTat.Size = new Size(840, 64);
        btnDat.Location = new Point(714, 536); btnDat.Name = "btnDat"; btnDat.Size = new Size(150, 30); btnDat.TabIndex = 10; btnDat.Text = "Đặt lịch"; btnDat.Click += btnDat_Click;
        btnDong.DialogResult = DialogResult.Cancel; btnDong.Location = new Point(594, 536); btnDong.Name = "btnDong"; btnDong.Size = new Size(110, 30); btnDong.TabIndex = 11; btnDong.Text = "Đóng";
        //
        // frmDatLichHo
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
                CancelButton = btnDong;
        ClientSize = new Size(888, 596);
        Controls.AddRange(new Control[] { lblBuoc1, txtSdt, btnTim, lblKetQua, lblHoTen, txtHoTen, lblGioiTinh, cboGioiTinh, lblNgaySinh, dtpNgaySinh,
            lblBuoc4, txtTrieuChung, lblBuoc2, cboKhoa, cboBacSi, lblBuoc3, dtpNgayKham, lstCa, lblTomTat, btnDat, btnDong });
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmDatLichHo";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Đặt lịch hộ khách gọi điện";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblBuoc1;
    private TextBox txtSdt;
    private Button btnTim;
    private Label lblKetQua;
    private Label lblHoTen;
    private TextBox txtHoTen;
    private Label lblGioiTinh;
    private ComboBox cboGioiTinh;
    private Label lblNgaySinh;
    private DateTimePicker dtpNgaySinh;
    private Label lblBuoc4;
    private TextBox txtTrieuChung;
    private Label lblBuoc2;
    private ComboBox cboKhoa;
    private ComboBox cboBacSi;
    private Label lblBuoc3;
    private DateTimePicker dtpNgayKham;
    private ListBox lstCa;
    private Label lblTomTat;
    private Button btnDat;
    private Button btnDong;
}
