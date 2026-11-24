namespace KmnClinic.GUI.Views;

partial class ucLichHenCuaToi
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
        dtpNgay = new DateTimePicker();
        chkMoiNgay = new CheckBox();
        cboTrangThai = new ComboBox();
        khung = new Panel();
        dgvLichHen = new DataGridView();
        colMa = new DataGridViewTextBoxColumn();
        colNgayKham = new DataGridViewTextBoxColumn();
        colCa = new DataGridViewTextBoxColumn();
        colPhong = new DataGridViewTextBoxColumn();
        colKhach = new DataGridViewTextBoxColumn();
        colSdt = new DataGridViewTextBoxColumn();
        colTrieuChung = new DataGridViewTextBoxColumn();
        colTrangThai = new DataGridViewTextBoxColumn();
        colHoSo = new DataGridViewTextBoxColumn();
        lblDem = new Label();
        pnlCongCu.SuspendLayout();
        flpLoc.SuspendLayout();
        khung.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvLichHen).BeginInit();
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
        flpLoc.Controls.Add(dtpNgay);
        flpLoc.Controls.Add(chkMoiNgay);
        flpLoc.Controls.Add(cboTrangThai);
        flpLoc.Dock = DockStyle.Fill;
        flpLoc.Name = "flpLoc";
        flpLoc.WrapContents = false;
        dtpNgay.Format = DateTimePickerFormat.Custom; dtpNgay.CustomFormat = "dd/MM/yyyy"; dtpNgay.Margin = new Padding(0, 4, 8, 0);
        dtpNgay.Name = "dtpNgay"; dtpNgay.Size = new Size(140, 30); dtpNgay.ValueChanged += BoLoc_Changed;
        chkMoiNgay.AutoSize = true; chkMoiNgay.Margin = new Padding(0, 8, 16, 0); chkMoiNgay.Name = "chkMoiNgay"; chkMoiNgay.Text = "Mọi ngày";
        chkMoiNgay.CheckedChanged += BoLoc_Changed;
        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList; cboTrangThai.Margin = new Padding(0, 4, 16, 0); cboTrangThai.Name = "cboTrangThai"; cboTrangThai.Size = new Size(160, 30);
        cboTrangThai.Items.AddRange(new object[] { "Mọi trạng thái", "Đã đặt", "Đã xác nhận", "Đã khám", "Đã hủy" });
        cboTrangThai.SelectedIndexChanged += BoLoc_Changed;
        //
        // khung
        //
        khung.Controls.Add(dgvLichHen);
        khung.Controls.Add(lblDem);
        khung.Dock = DockStyle.Fill;
        khung.Name = "khung";
        dgvLichHen.Columns.AddRange(new DataGridViewColumn[] { colMa, colNgayKham, colCa, colPhong, colKhach, colSdt, colTrieuChung, colTrangThai, colHoSo });
        dgvLichHen.Dock = DockStyle.Fill;
        dgvLichHen.Name = "dgvLichHen";
        colMa.DataPropertyName = "MaLichHen"; colMa.HeaderText = "Mã"; colMa.Name = "colMa"; colMa.FillWeight = 60;
        colNgayKham.DataPropertyName = "NgayKham"; colNgayKham.HeaderText = "Ngày khám"; colNgayKham.Name = "colNgayKham"; colNgayKham.FillWeight = 75;
        colNgayKham.DefaultCellStyle.Format = "dd/MM/yyyy";
        colCa.DataPropertyName = "CaKham"; colCa.HeaderText = "Ca"; colCa.Name = "colCa"; colCa.FillWeight = 90;
        colPhong.DataPropertyName = "TenPhong"; colPhong.HeaderText = "Phòng"; colPhong.Name = "colPhong"; colPhong.FillWeight = 70;
        colKhach.DataPropertyName = "TenKhachHang"; colKhach.HeaderText = "Khách hàng"; colKhach.Name = "colKhach"; colKhach.FillWeight = 120;
        colSdt.DataPropertyName = "SdtKhachHang"; colSdt.HeaderText = "SĐT"; colSdt.Name = "colSdt"; colSdt.FillWeight = 80;
        colTrieuChung.DataPropertyName = "TrieuChungBanDau"; colTrieuChung.HeaderText = "Triệu chứng ban đầu"; colTrieuChung.Name = "colTrieuChung"; colTrieuChung.FillWeight = 140;
        colTrangThai.DataPropertyName = "TrangThai"; colTrangThai.HeaderText = "Trạng thái"; colTrangThai.Name = "colTrangThai"; colTrangThai.FillWeight = 80;
        colHoSo.DataPropertyName = "MaHoSo"; colHoSo.HeaderText = "Hồ sơ"; colHoSo.Name = "colHoSo"; colHoSo.FillWeight = 60;
        lblDem.Dock = DockStyle.Bottom; lblDem.Name = "lblDem";
        lblDem.Size = new Size(900, 30); lblDem.TextAlign = ContentAlignment.BottomLeft;
        //
        // ucLichHenCuaToi
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(khung);
        Controls.Add(pnlCongCu);
        Name = "ucLichHenCuaToi";
        Size = new Size(1100, 600);
        pnlCongCu.ResumeLayout(false);
        flpLoc.ResumeLayout(false);
        flpLoc.PerformLayout();
        khung.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvLichHen).EndInit();
        ResumeLayout(false);
    }

    private Panel pnlCongCu;
    private FlowLayoutPanel flpLoc;
    private DateTimePicker dtpNgay;
    private CheckBox chkMoiNgay;
    private ComboBox cboTrangThai;
    private Panel khung;
    private DataGridView dgvLichHen;
    private DataGridViewTextBoxColumn colMa;
    private DataGridViewTextBoxColumn colNgayKham;
    private DataGridViewTextBoxColumn colCa;
    private DataGridViewTextBoxColumn colPhong;
    private DataGridViewTextBoxColumn colKhach;
    private DataGridViewTextBoxColumn colSdt;
    private DataGridViewTextBoxColumn colTrieuChung;
    private DataGridViewTextBoxColumn colTrangThai;
    private DataGridViewTextBoxColumn colHoSo;
    private Label lblDem;
}
