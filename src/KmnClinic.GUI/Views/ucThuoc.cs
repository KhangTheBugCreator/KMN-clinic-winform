using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Danh mục thuốc (Quản trị viên). Không có nút xóa: thuốc đã kê trong đơn cũ phải giữ lại.</summary>
public partial class ucThuoc : UserControl
{
    private readonly ThuocBUS _bus = new();

    public ucThuoc()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvThuoc);
        NapLai();
    }

    private void NapLai(string? chonMa = null)
    {
        ThongBaoLoi.Chay(this, () =>
        {
            var ds = _bus.DanhSach(txtTim.Text, chkSapHet.Checked);
            dgvThuoc.DataSource = ds;
            lblDem.Text = $"{ds.Count} thuốc · {_bus.DemSapHet()} thuốc sắp hết (tồn ≤ {ThuocDTO.NguongSapHet})";
            GiaoDien.ChonDong(dgvThuoc, chonMa is null ? null : (object o) => ((ThuocDTO)o).MaThuoc == chonMa);
        });
    }

    private ThuocDTO? DangChon => dgvThuoc.CurrentRow?.DataBoundItem as ThuocDTO;

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void btnThem_Click(object? sender, EventArgs e)
    {
        using var f = new frmThuocChiTiet(null);
        if (f.ShowDialog(this) == DialogResult.OK) NapLai(f.MaDaLuu);
    }

    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (DangChon is null) { MessageBox.Show(this, "Hãy chọn một thuốc.", "Thuốc"); return; }
        using var f = new frmThuocChiTiet(DangChon);
        if (f.ShowDialog(this) == DialogResult.OK) NapLai(f.MaDaLuu);
    }

    private void dgvThuoc_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnSua_Click(sender, e);
    }

    private void dgvThuoc_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var col = dgvThuoc.Columns[e.ColumnIndex].Name;
        if (col == "colDonGia" && e.Value is decimal gia) { e.Value = GiaoDien.Tien(gia); e.FormattingApplied = true; }
        if (col == "colTon" && e.Value is int ton && ton <= ThuocDTO.NguongSapHet)
            e.CellStyle!.ForeColor = Color.Red;
    }
}
