using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Hàng chờ của bác sĩ đang đăng nhập: mọi hồ sơ của mình chưa có hóa đơn (Chờ khám, Đang khám), Đang khám lên trước.
/// Trạng thái suy ra: Chờ khám → Đang khám (đã có chẩn đoán hoặc thuốc) → Hoàn tất (đã lập hóa đơn, rời hàng chờ).
/// Tự nạp lại mỗi 30 giây để thấy khách lễ tân vừa tiếp nhận.
/// </summary>
public partial class ucHangChoKham : UserControl
{
    private readonly KhamBenhBUS _bus = new();
    private readonly System.Windows.Forms.Timer _hen = new() { Interval = 30_000 };
    private List<HoSoKhamDTO> _ds = [];
    private bool _dangNap = true;

    public ucHangChoKham()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHangCho);
        cboLoc.SelectedIndex = 0;
        _hen.Tick += (_, _) => NapLai(imLang: true);
        Disposed += (_, _) => _hen.Dispose();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        _dangNap = false;
        NapLai();
        _hen.Start();
    }

    private HoSoKhamDTO? DangChon => dgvHangCho.CurrentRow?.DataBoundItem as HoSoKhamDTO;

    /// <param name="imLang">Lần nạp tự động: lỗi kết nối thì bỏ qua, không bật hộp thoại.</param>
    private void NapLai(string? chonMa = null, bool imLang = false)
    {
        if (_dangNap) return;
        chonMa ??= DangChon?.MaHoSo;
        TongQuanBacSiDTO tq = new();
        List<LichLamViecDTO> lich = [];
        try
        {
            _ds = _bus.HangCho();
            tq = _bus.TongQuan();
            lich = _bus.LichLamViecSapToi();
        }
        catch (Exception ex)
        {
            if (!imLang) ThongBaoLoi.Hien(ex, this);
            return;
        }
        HienLuoi(chonMa);
        theCho.GiaTri = tq.ChoKham.ToString();
        theCho.GhiChu = "Nhấp đúp để bắt đầu khám";
        theDang.GiaTri = tq.DangKham.ToString();
        theDang.GhiChu = "Đã lưu chẩn đoán hoặc kê thuốc";
        theXong.GiaTri = tq.HoanTatHomNay.ToString();
        theXong.GhiChu = "Đã chuyển thu ngân";

        var homNay = DateTime.Today;
        var caHomNay = lich.Where(l => l.NgayLamViec == homNay).ToList();
        var caSau = lich.FirstOrDefault(l => l.NgayLamViec > homNay);
        theCa.GiaTri = caHomNay.Count == 0 ? "Không có" : string.Join(", ", caHomNay.Select(l => l.TenCa));
        theCa.GhiChu = caHomNay.Count > 0
            ? string.Join("; ", caHomNay.Select(l => $"{l.GioBatDau:hh\\:mm}–{l.GioKetThuc:hh\\:mm} {l.TenPhong}"))
            : caSau is null ? "7 ngày tới chưa có ca" : $"Ca tới: {caSau.NgayLamViec:dd/MM} {caSau.TenCa}";
    }

    private void HienLuoi(string? chonMa)
    {
        var loc = cboLoc.SelectedIndex > 0 ? cboLoc.Text : null;
        dgvHangCho.DataSource = loc is null ? _ds : _ds.Where(h => h.TrangThai == loc).ToList();
        GiaoDien.ChonDong(dgvHangCho, chonMa is null ? null : o => ((HoSoKhamDTO)o).MaHoSo == chonMa);
        btnKham.Enabled = dgvHangCho.RowCount > 0;
    }

    private void cboLoc_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (!_dangNap) HienLuoi(DangChon?.MaHoSo);
    }

    private void btnLamMoi_Click(object? sender, EventArgs e) => NapLai();

    private void dgvHangCho_SelectionChanged(object? sender, EventArgs e)
        => btnKham.Text = DangChon?.TrangThai == TrangThai.DangKham ? "Khám tiếp" : "Khám bệnh";

    private void btnKham_Click(object? sender, EventArgs e)
    {
        if (DangChon is not { } hs) return;
        _hen.Stop();
        using (var f = new frmKhamBenh(hs.MaHoSo)) f.ShowDialog(this);
        NapLai();
        _hen.Start();
    }

    private void dgvHangCho_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnKham_Click(sender, e);
    }
}
