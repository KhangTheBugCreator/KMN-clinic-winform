using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Lễ tân tiếp nhận: khách có hẹn hôm nay (Q10; "Đã đặt" thì xác nhận và tiếp nhận một lần, Q3) và khách vãng lai với
/// bác sĩ đang trực (RB41, tính từ 30 phút trước ca). Tiếp nhận tạo hồ sơ khám ghi lễ tân đang đăng nhập; trigger tự
/// chuyển lịch hẹn sang "Đã khám".
/// </summary>
public partial class ucTiepNhan : UserControl
{
    private readonly LichHenBUS _lichHen = new();
    private readonly TiepNhanBUS _bus = new();
    private readonly KhachHangBUS _khachBus = new();
    private KhachHangDTO? _khach;

    public ucTiepNhan()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHen);
        GiaoDien.Luoi(dgvHoSo);
        colHsGio.DefaultCellStyle.Format = "HH:mm";
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        NapLai();
    }

    private LichHenDTO? HenDangChon => dgvHen.CurrentRow?.DataBoundItem as LichHenDTO;

    private void NapLai()
    {
        var chon = HenDangChon?.MaLichHen;
        var bsChon = (cboBacSi.SelectedItem as BacSiDangTrucDTO)?.MaNv;
        ThongBaoLoi.Chay(this, () =>
        {
            // Hẹn hôm nay chưa hủy: chưa tiếp nhận lên trước
            var hen = _lichHen.DanhSach(DateTime.Today, null, null).Where(h => h.TrangThai != TrangThai.DaHuy)
                .OrderBy(h => h.TrangThai == TrangThai.DaKham).ThenBy(h => h.GioBatDau).ThenBy(h => h.TenKhachHang).ToList();
            dgvHen.DataSource = hen;
            GiaoDien.ChonDong(dgvHen, chon is null ? null : o => ((LichHenDTO)o).MaLichHen == chon);
            lblHen.Text = $"Khách có hẹn hôm nay ({hen.Count(h => h.TrangThai != TrangThai.DaKham)} chưa tiếp nhận)";

            var bs = _bus.BacSiDangTruc();
            cboBacSi.DataSource = bs;
            if (bsChon is not null) cboBacSi.SelectedItem = bs.FirstOrDefault(b => b.MaNv == bsChon);
            btnTiepNhanVangLai.Enabled = bs.Count > 0;
            lblQuyTac.Text = bs.Count == 0 ? "Hiện không có bác sĩ nào đang trực." : "Bác sĩ trực từ 30 phút trước ca (RB41).";

            var hoSo = _bus.HoSoTheoNgay(DateTime.Today);
            dgvHoSo.DataSource = hoSo;
            lblHoSo.Text = $"Hồ sơ khám đã tiếp nhận hôm nay ({hoSo.Count}, {hoSo.Count(h => h.TrangThai == TrangThai.ChoKham)} chờ khám)";
        });
        dgvHen_SelectionChanged(null, EventArgs.Empty);
    }

    private void btnLamMoi_Click(object? sender, EventArgs e) => NapLai();

    private void dgvHen_SelectionChanged(object? sender, EventArgs e)
    {
        var h = HenDangChon;
        btnTiepNhanHen.Enabled = h?.TrangThai is TrangThai.DaDat or TrangThai.DaXacNhan;
        btnTiepNhanHen.Text = h?.TrangThai == TrangThai.DaDat ? "Xác nhận và tiếp nhận" : "Tiếp nhận và tạo hồ sơ";
    }

    private void btnTiepNhanHen_Click(object? sender, EventArgs e)
    {
        if (HenDangChon is not { } h) return;
        using var f = new frmNhapTrieuChung($"Tiếp nhận {h.TenKhachHang} – {h.CaKham}, bác sĩ {h.TenBacSi}", h.TrieuChungBanDau);
        if (f.ShowDialog(this) != DialogResult.OK) return;
        string ma = "";
        if (!ThongBaoLoi.Chay(this, () => ma = _bus.TiepNhanCoHen(h.MaLichHen, f.TrieuChung))) return;
        MessageBox.Show(this, $"Đã tạo hồ sơ {ma}. Mời khách {h.TenKhachHang} đến {h.TenPhong} gặp bác sĩ {h.TenBacSi}.",
            "Tiếp nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
        NapLai();
    }

    private void btnTim_Click(object? sender, EventArgs e)
    {
        KhachHangDTO? kh = null;
        if (!ThongBaoLoi.Chay(this, () => kh = _khachBus.TimTheoSdt(txtSdt.Text))) return;
        if (kh is null)
        {
            if (MessageBox.Show(this, "Chưa có khách với số điện thoại này. Thêm khách mới?", "Tiếp nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) { ChonKhach(null); return; }
            using var f = new frmKhachHangChiTiet(null, txtSdt.Text.Trim());
            if (f.ShowDialog(this) != DialogResult.OK || f.MaDaLuu is null) { ChonKhach(null); return; }
            ThongBaoLoi.Chay(this, () => kh = _khachBus.TimTheoSdt(txtSdt.Text));
        }
        ChonKhach(kh);
    }

    private void ChonKhach(KhachHangDTO? kh)
    {
        _khach = kh;
        txtHoTen.Text = kh?.HoTenKh ?? "";
        lblKhach.ForeColor = kh is null ? Color.DarkRed : Color.DarkGreen;
        lblKhach.Text = kh is null ? "Chưa chọn khách." : $"Đã có: {kh.MaKh} – {kh.GioiTinhHienThi}, sinh {kh.NgaySinhKh:dd/MM/yyyy}";
    }

    private void btnTiepNhanVangLai_Click(object? sender, EventArgs e)
    {
        // Đổi SĐT sau khi tìm mà chưa tìm lại thì không dùng khách cũ
        if (_khach is not null && _khach.SdtKh != txtSdt.Text.Trim()) ChonKhach(null);
        var bs = cboBacSi.SelectedItem as BacSiDangTrucDTO;
        string ma = "";
        if (!ThongBaoLoi.Chay(this, () => ma = _bus.TiepNhanVangLai(_khach?.MaKh, bs?.MaNv, txtTrieuChung.Text))) { NapLai(); return; }
        MessageBox.Show(this, $"Đã tạo hồ sơ {ma}. Mời khách {_khach!.HoTenKh} đến {bs!.TenPhong} gặp bác sĩ {bs.HoTenNv}.",
            "Tiếp nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
        txtSdt.Clear(); txtTrieuChung.Clear();
        ChonKhach(null);
        lblKhach.Text = "";
        NapLai();
    }
}
