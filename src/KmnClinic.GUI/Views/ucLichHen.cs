using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Lễ tân quản lý lịch hẹn: xác nhận (Đã đặt → Đã xác nhận), hủy (từ Đã đặt / Đã xác nhận, RB14–RB15), đặt lịch hộ khách gọi điện.
/// Module W4 (đặt lịch hộ) và W5 (xác nhận, hủy) thay dữ liệu giả bằng LichHenBUS.
/// </summary>
public partial class ucLichHen : UserControl
{
    private readonly List<LichHenDTO> _ds = Gia.LichHen();

    public ucLichHen()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvLichHen);
        GiaoDien.BadgeTrangThai(dgvLichHen, "colTrangThai");
        GiaoDien.Nut(btnDatLich);
        GiaoDien.Nut(btnXacNhan, GiaoDien.KieuNut.ThanhCong);
        GiaoDien.Nut(btnHuy, GiaoDien.KieuNut.Phu);
        dtpNgay.Value = Gia.HomNay;
        cboTrangThai.SelectedIndex = 0;
        NapLai();
    }

    private LichHenDTO? DangChon => dgvLichHen.CurrentRow?.DataBoundItem as LichHenDTO;

    private void NapLai()
    {
        dtpNgay.Enabled = !chkMoiNgay.Checked;
        var tk = txtTim.Text.Trim();
        var ds = _ds.Where(l =>
                (chkMoiNgay.Checked || l.NgayKham.Date == dtpNgay.Value.Date)
                && (cboTrangThai.SelectedIndex <= 0 || l.TrangThai == cboTrangThai.Text)
                && (tk.Length == 0 || l.TenKhachHang.Contains(tk, StringComparison.OrdinalIgnoreCase) || l.SdtKhachHang.Contains(tk)))
            .OrderBy(l => l.NgayKham).ThenBy(l => l.TenCa).ToList();
        dgvLichHen.DataSource = ds;
        lblDem.Text = $"{ds.Count} lịch hẹn · {ds.Count(l => l.TrangThai == TrangThai.DaDat)} chờ xác nhận";
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
        lh.TrangThai = TrangThai.DaXacNhan;
        lh.LeTanXacNhan = PhienDangNhap.NhanVien?.HoTenNv; // RB: ghi lễ tân xác nhận (câu hỏi truy vết 1)
        NapLai();
    }

    private void btnHuy_Click(object? sender, EventArgs e)
    {
        if (DangChon is not { } lh) return;
        if (MessageBox.Show($"Hủy lịch hẹn {lh.MaLichHen} của {lh.TenKhachHang}?", "Hủy lịch hẹn",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
        lh.TrangThai = TrangThai.DaHuy;
        NapLai();
    }

    private void btnDatLich_Click(object? sender, EventArgs e)
    {
        using var f = new frmDatLichHo();
        if (f.ShowDialog(this) == DialogResult.OK && f.LichHenMoi is { } moi)
        {
            _ds.Add(moi);
            chkMoiNgay.Checked = true;
            NapLai();
        }
    }
}
