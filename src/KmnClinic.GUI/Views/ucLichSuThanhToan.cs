using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Thu ngân: hóa đơn đã thu, lọc theo ngày thu, chỉ của mình, từ khóa; mở để xem và in lại.</summary>
public partial class ucLichSuThanhToan : UserControl
{
    private readonly ThanhToanBUS _bus = new();
    private bool _dangNap = true;

    public ucLichSuThanhToan()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHoaDon);
        dtpDen.Value = DateTime.Today;
        dtpTu.Value = DateTime.Today;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        _dangNap = false;
        NapLai();
    }

    private HoaDonDTO? DangChon => dgvHoaDon.CurrentRow?.DataBoundItem as HoaDonDTO;

    private void NapLai()
    {
        if (_dangNap) return;
        var chonMa = DangChon?.MaHoaDon;
        dtpTu.Enabled = dtpDen.Enabled = !chkMoiNgay.Checked;
        List<HoaDonDTO> ds = [];
        if (!ThongBaoLoi.Chay(this, () => ds = _bus.LichSu(
                chkMoiNgay.Checked ? null : dtpTu.Value.Date, chkMoiNgay.Checked ? null : dtpDen.Value.Date,
                chkCuaToi.Checked, txtTim.Text))) return;
        dgvHoaDon.DataSource = ds;
        GiaoDien.ChonDong(dgvHoaDon, chonMa is null ? null : o => ((HoaDonDTO)o).MaHoaDon == chonMa);
        lblDem.Text = $"{ds.Count} hóa đơn · tổng thu {GiaoDien.Tien(ds.Sum(h => h.TongTien))}";
        btnXem.Enabled = ds.Count > 0;
    }

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void btnXem_Click(object? sender, EventArgs e)
    {
        if (DangChon is not { } hd) return;
        using var f = new frmThuTien(hd.MaHoaDon);
        f.ShowDialog(this);
    }

    private void dgvHoaDon_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnXem_Click(sender, e);
    }
}
