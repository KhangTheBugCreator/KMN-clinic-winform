namespace KmnClinic.GUI.Views;

partial class ucTongQuanQuanTri
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
        theDoanhThu = new KmnClinic.GUI.Controls.TheThongKe();
        theLuotKham = new KmnClinic.GUI.Controls.TheThongKe();
        theLichHen = new KmnClinic.GUI.Controls.TheThongKe();
        theThuoc = new KmnClinic.GUI.Controls.TheThongKe();
        tlpDuoi = new TableLayoutPanel();
        khungBieuDo = new KmnClinic.GUI.Controls.KhungTrang();
        bieuDo = new KmnClinic.GUI.Controls.BieuDoCot();
        lblBieuDo = new Label();
        khungThuoc = new KmnClinic.GUI.Controls.KhungTrang();
        dgvSapHet = new DataGridView();
        colTenThuoc = new DataGridViewTextBoxColumn();
        colTon = new DataGridViewTextBoxColumn();
        lblSapHet = new Label();
        flpThe.SuspendLayout();
        tlpDuoi.SuspendLayout();
        khungBieuDo.SuspendLayout();
        khungThuoc.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)dgvSapHet).BeginInit();
        SuspendLayout();
        //
        // flpThe
        //
        flpThe.Controls.Add(theDoanhThu);
        flpThe.Controls.Add(theLuotKham);
        flpThe.Controls.Add(theLichHen);
        flpThe.Controls.Add(theThuoc);
        flpThe.Dock = DockStyle.Top;
        flpThe.Name = "flpThe";
        flpThe.Size = new Size(1000, 128);
        flpThe.WrapContents = false;
        //
        // các thẻ số liệu
        //
        theDoanhThu.Name = "theDoanhThu"; theDoanhThu.TieuDe = "Doanh thu hôm nay"; theDoanhThu.MauNhan = Color.FromArgb(76, 175, 106);
        theLuotKham.Name = "theLuotKham"; theLuotKham.TieuDe = "Lượt khám hôm nay"; theLuotKham.MauNhan = Color.FromArgb(31, 111, 209);
        theLichHen.Name = "theLichHen"; theLichHen.TieuDe = "Lịch hẹn chờ xác nhận"; theLichHen.MauNhan = Color.FromArgb(242, 163, 58);
        theThuoc.Name = "theThuoc"; theThuoc.TieuDe = "Thuốc sắp hết"; theThuoc.MauNhan = Color.FromArgb(229, 83, 75);
        //
        // tlpDuoi
        //
        tlpDuoi.ColumnCount = 2;
        tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 64F));
        tlpDuoi.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 36F));
        tlpDuoi.Controls.Add(khungBieuDo, 0, 0);
        tlpDuoi.Controls.Add(khungThuoc, 1, 0);
        tlpDuoi.Dock = DockStyle.Fill;
        tlpDuoi.Name = "tlpDuoi";
        tlpDuoi.RowCount = 1;
        tlpDuoi.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        //
        // khungBieuDo
        //
        khungBieuDo.Controls.Add(bieuDo);
        khungBieuDo.Controls.Add(lblBieuDo);
        khungBieuDo.Dock = DockStyle.Fill;
        khungBieuDo.Margin = new Padding(0, 0, 8, 0);
        khungBieuDo.Name = "khungBieuDo";
        bieuDo.Dock = DockStyle.Fill;
        bieuDo.Name = "bieuDo";
        lblBieuDo.Dock = DockStyle.Top; lblBieuDo.Font = new Font("Segoe UI Semibold", 11.5F); lblBieuDo.ForeColor = Color.FromArgb(23, 54, 111);
        lblBieuDo.Name = "lblBieuDo"; lblBieuDo.Size = new Size(500, 32); lblBieuDo.Text = "Doanh thu 14 ngày gần nhất (triệu đồng)";
        //
        // khungThuoc
        //
        khungThuoc.Controls.Add(dgvSapHet);
        khungThuoc.Controls.Add(lblSapHet);
        khungThuoc.Dock = DockStyle.Fill;
        khungThuoc.Margin = new Padding(8, 0, 0, 0);
        khungThuoc.Name = "khungThuoc";
        dgvSapHet.Columns.AddRange(new DataGridViewColumn[] { colTenThuoc, colTon });
        dgvSapHet.Dock = DockStyle.Fill;
        dgvSapHet.Name = "dgvSapHet";
        colTenThuoc.DataPropertyName = "TenThuoc"; colTenThuoc.HeaderText = "Thuốc"; colTenThuoc.Name = "colTenThuoc"; colTenThuoc.FillWeight = 180;
        colTon.DataPropertyName = "SoLuongTon"; colTon.HeaderText = "Tồn"; colTon.Name = "colTon"; colTon.FillWeight = 60;
        colTon.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        lblSapHet.Dock = DockStyle.Top; lblSapHet.Font = new Font("Segoe UI Semibold", 11.5F); lblSapHet.ForeColor = Color.FromArgb(23, 54, 111);
        lblSapHet.Name = "lblSapHet"; lblSapHet.Size = new Size(300, 32); lblSapHet.Text = "Thuốc cần nhập thêm";
        //
        // ucTongQuanQuanTri
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(245, 249, 254);
        Controls.Add(tlpDuoi);
        Controls.Add(flpThe);
        Name = "ucTongQuanQuanTri";
        Size = new Size(1000, 600);
        flpThe.ResumeLayout(false);
        tlpDuoi.ResumeLayout(false);
        khungBieuDo.ResumeLayout(false);
        khungThuoc.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)dgvSapHet).EndInit();
        ResumeLayout(false);
    }

    private FlowLayoutPanel flpThe;
    private KmnClinic.GUI.Controls.TheThongKe theDoanhThu;
    private KmnClinic.GUI.Controls.TheThongKe theLuotKham;
    private KmnClinic.GUI.Controls.TheThongKe theLichHen;
    private KmnClinic.GUI.Controls.TheThongKe theThuoc;
    private TableLayoutPanel tlpDuoi;
    private KmnClinic.GUI.Controls.KhungTrang khungBieuDo;
    private KmnClinic.GUI.Controls.BieuDoCot bieuDo;
    private Label lblBieuDo;
    private KmnClinic.GUI.Controls.KhungTrang khungThuoc;
    private DataGridView dgvSapHet;
    private DataGridViewTextBoxColumn colTenThuoc;
    private DataGridViewTextBoxColumn colTon;
    private Label lblSapHet;
}
