using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Hàng chờ của bác sĩ đang đăng nhập (chỉ hồ sơ của mình). Trạng thái suy ra như web:
/// Chờ khám → Đang khám (đã lưu nháp) → Hoàn tất (đã có hóa đơn). Module W6 thay dữ liệu giả bằng KhamBenhBUS.
/// </summary>
public partial class ucHangChoKham : UserControl
{
    private readonly List<HoSoKhamDTO> _ds = Gia.HoSoHomNay();

    public ucHangChoKham()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHangCho);
        NapLai();
    }

    private void NapLai()
    {
        dgvHangCho.DataSource = null;
        dgvHangCho.DataSource = _ds.OrderBy(h => h.TrangThai == TrangThai.HoanTat).ThenBy(h => h.NgayGioKham).ToList();
        theCho.GiaTri = _ds.Count(h => h.TrangThai == TrangThai.ChoKham).ToString();
        theDang.GiaTri = _ds.Count(h => h.TrangThai == TrangThai.DangKham).ToString();
        theXong.GiaTri = _ds.Count(h => h.TrangThai == TrangThai.HoanTat).ToString();
        theCho.GhiChu = "Mở hồ sơ để bắt đầu khám";
        theXong.GhiChu = "Đã chuyển thu ngân";
    }

    private void dgvHangCho_SelectionChanged(object? sender, EventArgs e)
    {
        var hs = dgvHangCho.CurrentRow?.DataBoundItem as HoSoKhamDTO;
        btnKham.Text = hs?.TrangThai == TrangThai.HoanTat ? "Xem hồ sơ" : "Khám bệnh";
    }

    private void btnKham_Click(object? sender, EventArgs e)
    {
        if (dgvHangCho.CurrentRow?.DataBoundItem is not HoSoKhamDTO hs) return;
        using var f = new frmKhamBenh(hs);
        f.ShowDialog(this);
        NapLai();
    }

    private void dgvHangCho_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnKham_Click(sender, e);
    }
}
