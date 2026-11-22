namespace KmnClinic.GUI.Forms;

partial class frmThuTien
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTieuDe = new Label();
        lblThongTin = new Label();
        dgvChiTiet = new DataGridView();
        colNoiDung = new DataGridViewTextBoxColumn();
        colSoLuong = new DataGridViewTextBoxColumn();
        colDonGia = new DataGridViewTextBoxColumn();
        colThanhTien = new DataGridViewTextBoxColumn();
        lblTong = new Label();
        lblKhachDua = new Label();
        numKhachDua = new NumericUpDown();
        lblTraLai = new Label();
        btnXemIn = new Button();
        btnDong = new Button();
        btnXacNhan = new Button();
        ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
        ((System.ComponentModel.ISupportInitialize)numKhachDua).BeginInit();
        SuspendLayout();
        //
        lblTieuDe.AutoSize = true; lblTieuDe.Font = new Font("Segoe UI Semibold", 15F); lblTieuDe.ForeColor = Color.FromArgb(23, 54, 111);
        lblTieuDe.Location = new Point(24, 18); lblTieuDe.Name = "lblTieuDe";
        lblThongTin.ForeColor = Color.FromArgb(107, 118, 134); lblThongTin.Location = new Point(24, 58); lblThongTin.Name = "lblThongTin"; lblThongTin.Size = new Size(712, 50);
        //
        dgvChiTiet.Columns.AddRange(new DataGridViewColumn[] { colNoiDung, colSoLuong, colDonGia, colThanhTien });
        dgvChiTiet.Location = new Point(24, 116); dgvChiTiet.Name = "dgvChiTiet"; dgvChiTiet.Size = new Size(712, 250);
        colNoiDung.HeaderText = "Nội dung"; colNoiDung.Name = "colNoiDung"; colNoiDung.FillWeight = 200;
        colSoLuong.HeaderText = "SL"; colSoLuong.Name = "colSoLuong"; colSoLuong.FillWeight = 50; colSoLuong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colDonGia.HeaderText = "Đơn giá"; colDonGia.Name = "colDonGia"; colDonGia.FillWeight = 80; colDonGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        colThanhTien.HeaderText = "Thành tiền"; colThanhTien.Name = "colThanhTien"; colThanhTien.FillWeight = 90; colThanhTien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        //
        lblTong.Font = new Font("Segoe UI Semibold", 16F); lblTong.ForeColor = Color.FromArgb(23, 54, 111);
        lblTong.Location = new Point(336, 378); lblTong.Name = "lblTong"; lblTong.Size = new Size(400, 40); lblTong.TextAlign = ContentAlignment.MiddleRight;
        lblKhachDua.AutoSize = true; lblKhachDua.Location = new Point(24, 438); lblKhachDua.Name = "lblKhachDua"; lblKhachDua.Text = "Khách đưa";
        numKhachDua.Increment = 10000; numKhachDua.Location = new Point(124, 434); numKhachDua.Maximum = 100000000; numKhachDua.Name = "numKhachDua";
        numKhachDua.Size = new Size(170, 30); numKhachDua.ThousandsSeparator = true; numKhachDua.ValueChanged += numKhachDua_ValueChanged;
        lblTraLai.AutoSize = true; lblTraLai.Font = new Font("Segoe UI Semibold", 10F); lblTraLai.ForeColor = Color.FromArgb(76, 175, 106);
        lblTraLai.Location = new Point(310, 438); lblTraLai.Name = "lblTraLai";
        //
        btnXemIn.Location = new Point(24, 496); btnXemIn.Name = "btnXemIn"; btnXemIn.Size = new Size(170, 40); btnXemIn.Text = "Xem trước bản in"; btnXemIn.Click += btnXemIn_Click;
        btnDong.DialogResult = DialogResult.Cancel; btnDong.Location = new Point(436, 496); btnDong.Name = "btnDong"; btnDong.Size = new Size(100, 40); btnDong.Text = "Đóng";
        btnXacNhan.Location = new Point(546, 496); btnXacNhan.Name = "btnXacNhan"; btnXacNhan.Size = new Size(190, 40); btnXacNhan.Text = "Xác nhận đã thu"; btnXacNhan.Click += btnXacNhan_Click;
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.White;
        CancelButton = btnDong;
        ClientSize = new Size(760, 560);
        Controls.AddRange(new Control[] { lblTieuDe, lblThongTin, dgvChiTiet, lblTong, lblKhachDua, numKhachDua, lblTraLai, btnXemIn, btnDong, btnXacNhan });
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmThuTien";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Thu tiền";
        ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
        ((System.ComponentModel.ISupportInitialize)numKhachDua).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTieuDe;
    private Label lblThongTin;
    private DataGridView dgvChiTiet;
    private DataGridViewTextBoxColumn colNoiDung;
    private DataGridViewTextBoxColumn colSoLuong;
    private DataGridViewTextBoxColumn colDonGia;
    private DataGridViewTextBoxColumn colThanhTien;
    private Label lblTong;
    private Label lblKhachDua;
    private NumericUpDown numKhachDua;
    private Label lblTraLai;
    private Button btnXemIn;
    private Button btnDong;
    private Button btnXacNhan;
}
