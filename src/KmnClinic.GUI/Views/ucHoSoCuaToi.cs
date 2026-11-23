using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Hồ sơ khám của bác sĩ đang đăng nhập, lọc theo khoảng ngày, trạng thái, từ khóa. Mở hồ sơ chưa hoàn tất để khám tiếp,
/// hồ sơ đã hoàn tất chỉ xem.
/// </summary>
public partial class ucHoSoCuaToi : UserControl
{
    private readonly KhamBenhBUS _bus = new();
    private bool _dangNap = true;

    public ucHoSoCuaToi()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHoSo);
        dtpDen.Value = DateTime.Today;
        dtpTu.Value = DateTime.Today.AddDays(-30);
        cboTrangThai.SelectedIndex = 0;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        _dangNap = false;
        NapLai();
    }

    private HoSoKhamDTO? DangChon => dgvHoSo.CurrentRow?.DataBoundItem as HoSoKhamDTO;

    private void NapLai(string? chonMa = null)
    {
        if (_dangNap) return;
        chonMa ??= DangChon?.MaHoSo;
        dtpTu.Enabled = dtpDen.Enabled = !chkMoiNgay.Checked;
        List<HoSoKhamDTO> ds = [];
        if (!ThongBaoLoi.Chay(this, () => ds = _bus.DanhSach(
                chkMoiNgay.Checked ? null : dtpTu.Value.Date, chkMoiNgay.Checked ? null : dtpDen.Value.Date,
                cboTrangThai.SelectedIndex > 0 ? cboTrangThai.Text : null, txtTim.Text))) return;
        dgvHoSo.DataSource = ds;
        GiaoDien.ChonDong(dgvHoSo, chonMa is null ? null : o => ((HoSoKhamDTO)o).MaHoSo == chonMa);
        lblDem.Text = $"{ds.Count} hồ sơ · {ds.Count(h => h.DaHoanTat)} đã hoàn tất · {ds.Count(h => !h.DaHoanTat)} chưa hoàn tất";
        btnXem.Enabled = ds.Count > 0;
    }

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void btnXem_Click(object? sender, EventArgs e)
    {
        if (DangChon is not { } hs) return;
        using (var f = new frmKhamBenh(hs.MaHoSo)) f.ShowDialog(this);
        NapLai(hs.MaHoSo);
    }

    private void dgvHoSo_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnXem_Click(sender, e);
    }
}
