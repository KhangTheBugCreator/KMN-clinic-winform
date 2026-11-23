using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Lịch hẹn khách đặt vào ca của bác sĩ đang đăng nhập (chỉ xem; xác nhận, hủy do lễ tân làm).</summary>
public partial class ucLichHenCuaToi : UserControl
{
    private readonly KhamBenhBUS _bus = new();
    private bool _dangNap = true;

    public ucLichHenCuaToi()
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

    private void NapLai()
    {
        if (_dangNap) return;
        dtpNgay.Enabled = !chkMoiNgay.Checked;
        List<LichHenDTO> ds = [];
        if (!ThongBaoLoi.Chay(this, () => ds = _bus.LichHenCuaToi(chkMoiNgay.Checked ? null : dtpNgay.Value.Date,
                cboTrangThai.SelectedIndex > 0 ? cboTrangThai.Text : null))) return;
        dgvLichHen.DataSource = ds;
        lblDem.Text = $"{ds.Count} lịch hẹn · {ds.Count(l => l.TrangThai is TrangThai.DaDat or TrangThai.DaXacNhan)} chưa đến khám · " +
                      $"{ds.Count(l => l.TrangThai == TrangThai.DaKham)} đã tiếp nhận";
    }

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();
}
