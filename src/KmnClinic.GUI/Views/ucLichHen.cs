using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Lễ tân quản lý lịch hẹn: xác nhận lịch khách tự đặt trên web (Đã đặt → Đã xác nhận), hủy (từ Đã đặt / Đã xác nhận, RB15),
/// đặt lịch hộ khách gọi điện (lịch đặt hộ được xác nhận luôn).
/// </summary>
public partial class ucLichHen : UserControl
{
    private readonly LichHenBUS _bus = new();
    private bool _dangNap = true;

    public ucLichHen()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvLichHen);
        dtpNgay.Value = DateTime.Today;
        cboTrangThai.SelectedIndex = 0;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        _dangNap = false;
        NapLai();
    }

    private LichHenDTO? DangChon => dgvLichHen.CurrentRow?.DataBoundItem as LichHenDTO;

    private void NapLai(string? chonMa = null)
    {
        if (_dangNap) return;
        chonMa ??= DangChon?.MaLichHen;
        dtpNgay.Enabled = !chkMoiNgay.Checked;
        List<LichHenDTO> ds = [];
        if (!ThongBaoLoi.Chay(this, () => ds = _bus.DanhSach(chkMoiNgay.Checked ? null : dtpNgay.Value.Date,
                cboTrangThai.SelectedIndex > 0 ? cboTrangThai.Text : null, txtTim.Text))) return;
        dgvLichHen.DataSource = ds;
        GiaoDien.ChonDong(dgvLichHen, chonMa is null ? null : o => ((LichHenDTO)o).MaLichHen == chonMa);
        lblDem.Text = $"{ds.Count} lịch hẹn · {ds.Count(l => l.TrangThai == TrangThai.DaDat)} chờ xác nhận (khách tự đặt trên web)";
        dgvLichHen_SelectionChanged(null, EventArgs.Empty);
    }

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void dgvLichHen_SelectionChanged(object? sender, EventArgs e)
    {
        var lh = DangChon;
        btnXacNhan.Enabled = lh?.TrangThai == TrangThai.DaDat;
        btnHuy.Enabled = lh?.TrangThai is TrangThai.DaDat or TrangThai.DaXacNhan;
    }

    private void btnXacNhan_Click(object? sender, EventArgs e)
    {
        if (DangChon is not { } lh) return;
        if (ThongBaoLoi.Chay(this, () => _bus.XacNhan(lh.MaLichHen))) NapLai(lh.MaLichHen);
    }

    private void btnHuy_Click(object? sender, EventArgs e)
    {
        if (DangChon is not { } lh) return;
        if (MessageBox.Show(this, $"Hủy lịch hẹn {lh.MaLichHen} của {lh.TenKhachHang} ({lh.NgayKham:dd/MM/yyyy}, {lh.CaKham})?", "Hủy lịch hẹn",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        if (ThongBaoLoi.Chay(this, () => _bus.Huy(lh.MaLichHen))) NapLai(lh.MaLichHen);
    }

    private void btnDatLich_Click(object? sender, EventArgs e)
    {
        using var f = new frmDatLichHo();
        if (f.ShowDialog(this) != DialogResult.OK || f.LichHenMoi is not { } moi) return;
        _dangNap = true;
        chkMoiNgay.Checked = false;
        dtpNgay.Value = moi.NgayKham;
        cboTrangThai.SelectedIndex = 0;
        txtTim.Clear();
        _dangNap = false;
        NapLai(moi.MaLichHen);
    }
}
