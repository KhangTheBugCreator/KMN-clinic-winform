using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Thu ngân: danh sách hóa đơn chờ thu. Module W7 thay dữ liệu giả bằng ThanhToanBUS.</summary>
public partial class ucHoaDonChoThu : UserControl
{
    private readonly List<HoaDonDTO> _ds = Gia.HoaDon();

    public ucHoaDonChoThu()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHoaDon);
        GiaoDien.BadgeTrangThai(dgvHoaDon, "colTrangThai");
        GiaoDien.Nut(btnThu);
        NapLai();
    }

    private void NapLai()
    {
        var tk = txtTim.Text.Trim();
        var cho = _ds.Where(h => h.TrangThaiThanhToan == TrangThai.ChuaThanhToan).ToList();
        dgvHoaDon.DataSource = cho.Where(h => tk.Length == 0
                || h.MaHoaDon.Contains(tk, StringComparison.OrdinalIgnoreCase)
                || h.TenKhachHang.Contains(tk, StringComparison.OrdinalIgnoreCase)
                || h.SdtKhachHang.Contains(tk))
            .OrderBy(h => h.NgayKham).ToList();
        theChoThu.GiaTri = cho.Count.ToString();
        theTienCho.GiaTri = GiaoDien.Tien(cho.Sum(h => h.TongTien));
        var daThu = _ds.Where(h => h.NgayGioThanhToan?.Date == Gia.HomNay).ToList();
        theDaThu.GiaTri = GiaoDien.Tien(daThu.Sum(h => h.TongTien));
        theDaThu.GhiChu = $"{daThu.Count} hóa đơn";
        btnThu.Enabled = cho.Count > 0;
    }

    private void txtTim_TextChanged(object? sender, EventArgs e) => NapLai();

    private void btnThu_Click(object? sender, EventArgs e)
    {
        if (dgvHoaDon.CurrentRow?.DataBoundItem is not HoaDonDTO hd) return;
        using var f = new frmThuTien(hd);
        if (f.ShowDialog(this) == DialogResult.OK) NapLai();
    }

    private void dgvHoaDon_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnThu_Click(sender, e);
    }
}
