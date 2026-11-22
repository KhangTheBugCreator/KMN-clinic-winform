namespace KmnClinic.GUI.Views;

partial class ucTongQuanLeTan
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        flpThe = new FlowLayoutPanel();
        theChoXacNhan = new KmnClinic.GUI.Controls.TheThongKe();
        theHenHomNay = new KmnClinic.GUI.Controls.TheThongKe();
        theChoTiepNhan = new KmnClinic.GUI.Controls.TheThongKe();
        theHoSo = new KmnClinic.GUI.Controls.TheThongKe();
        theVangLai = new KmnClinic.GUI.Controls.TheThongKe();
        theBacSi = new KmnClinic.GUI.Controls.TheThongKe();
        dgvBacSi = new DataGridView();
        colTen = new DataGridViewTextBoxColumn();
        colKhoa = new DataGridViewTextBoxColumn();
        colPhong = new DataGridViewTextBoxColumn();
        colCa = new DataGridViewTextBoxColumn();
        colCho = new DataGridViewTextBoxColumn();
        lblBacSi = new Label();
        pnlDuoi = new Panel();
        btnLamMoi = new Button();
        lblCapNhat = new Label();
        flpThe.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvBacSi).BeginInit();
        pnlDuoi.SuspendLayout();
        SuspendLayout();
        //
        // flpThe
        //
        flpThe.Controls.Add(theChoXacNhan);
        flpThe.Controls.Add(theHenHomNay);
        flpThe.Controls.Add(theChoTiepNhan);
        flpThe.Controls.Add(theHoSo);
        flpThe.Controls.Add(theVangLai);
        flpThe.Controls.Add(theBacSi);
        flpThe.Dock = DockStyle.Top;
        flpThe.Name = "flpThe";
        flpThe.Padding = new Padding(0, 0, 0, 8);
        flpThe.Size = new Size(1000, 90);
        flpThe.WrapContents = false;
        theChoXacNhan.Name = "theChoXacNhan"; theChoXacNhan.TieuDe = "Lịch chờ xác nhận"; theChoXacNhan.Size = new Size(158, 80);
        theHenHomNay.Name = "theHenHomNay"; theHenHomNay.TieuDe = "Lịch hẹn hôm nay"; theHenHomNay.Size = new Size(158, 80);
        theChoTiepNhan.Name = "theChoTiepNhan"; theChoTiepNhan.TieuDe = "Hẹn chờ tiếp nhận"; theChoTiepNhan.Size = new Size(158, 80);
        theHoSo.Name = "theHoSo"; theHoSo.TieuDe = "Hồ sơ tiếp nhận hôm nay"; theHoSo.Size = new Size(158, 80);
        theVangLai.Name = "theVangLai"; theVangLai.TieuDe = "Khách vãng lai hôm nay"; theVangLai.Size = new Size(158, 80);
        theBacSi.Name = "theBacSi"; theBacSi.TieuDe = "Bác sĩ đang trực"; theBacSi.Size = new Size(158, 80);
        //
        // lblBacSi
        //
        lblBacSi.Dock = DockStyle.Top; lblBacSi.Font = new Font("Segoe UI", 9F, FontStyle.Bold); lblBacSi.Name = "lblBacSi"; lblBacSi.Size = new Size(1000, 28);
        lblBacSi.Text = "Bác sĩ đang trực (nhận khách vãng lai từ 30 phút trước ca)"; lblBacSi.TextAlign = ContentAlignment.MiddleLeft;
        //
        // dgvBacSi
        //
        dgvBacSi.Columns.AddRange(new DataGridViewColumn[] { colTen, colKhoa, colPhong, colCa, colCho });
        dgvBacSi.Dock = DockStyle.Fill; dgvBacSi.Name = "dgvBacSi";
        colTen.DataPropertyName = "HoTenNv"; colTen.HeaderText = "Bác sĩ"; colTen.Name = "colTen"; colTen.FillWeight = 130;
        colKhoa.DataPropertyName = "TenKhoa"; colKhoa.HeaderText = "Khoa"; colKhoa.Name = "colKhoa"; colKhoa.FillWeight = 100;
        colPhong.DataPropertyName = "TenPhong"; colPhong.HeaderText = "Phòng"; colPhong.Name = "colPhong"; colPhong.FillWeight = 70;
        colCa.DataPropertyName = "CaTruc"; colCa.HeaderText = "Ca trực"; colCa.Name = "colCa"; colCa.FillWeight = 110;
        colCho.DataPropertyName = "SoDangCho"; colCho.HeaderText = "Hồ sơ chưa xong"; colCho.Name = "colCho"; colCho.FillWeight = 80;
        //
        // pnlDuoi
        //
        pnlDuoi.Controls.Add(btnLamMoi);
        pnlDuoi.Controls.Add(lblCapNhat);
        pnlDuoi.Dock = DockStyle.Bottom; pnlDuoi.Name = "pnlDuoi"; pnlDuoi.Size = new Size(1000, 40);
        btnLamMoi.Location = new Point(0, 6); btnLamMoi.Name = "btnLamMoi"; btnLamMoi.Size = new Size(100, 28); btnLamMoi.Text = "Làm mới"; btnLamMoi.Click += btnLamMoi_Click;
        lblCapNhat.AutoSize = true; lblCapNhat.Location = new Point(110, 12); lblCapNhat.Name = "lblCapNhat";
        //
        // ucTongQuanLeTan
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        Controls.Add(dgvBacSi);
        Controls.Add(lblBacSi);
        Controls.Add(flpThe);
        Controls.Add(pnlDuoi);
        Name = "ucTongQuanLeTan";
        Size = new Size(1000, 600);
        flpThe.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvBacSi).EndInit();
        pnlDuoi.ResumeLayout(false);
        pnlDuoi.PerformLayout();
        ResumeLayout(false);
    }

    private FlowLayoutPanel flpThe;
    private KmnClinic.GUI.Controls.TheThongKe theChoXacNhan;
    private KmnClinic.GUI.Controls.TheThongKe theHenHomNay;
    private KmnClinic.GUI.Controls.TheThongKe theChoTiepNhan;
    private KmnClinic.GUI.Controls.TheThongKe theHoSo;
    private KmnClinic.GUI.Controls.TheThongKe theVangLai;
    private KmnClinic.GUI.Controls.TheThongKe theBacSi;
    private DataGridView dgvBacSi;
    private DataGridViewTextBoxColumn colTen;
    private DataGridViewTextBoxColumn colKhoa;
    private DataGridViewTextBoxColumn colPhong;
    private DataGridViewTextBoxColumn colCa;
    private DataGridViewTextBoxColumn colCho;
    private Label lblBacSi;
    private Panel pnlDuoi;
    private Button btnLamMoi;
    private Label lblCapNhat;
}
