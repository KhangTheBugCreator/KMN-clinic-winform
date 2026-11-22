using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Danh mục thuốc (Quản trị viên). Module W1 thay dữ liệu giả bằng ThuocBUS.</summary>
public partial class ucThuoc : UserControl
{
    private const int NguongSapHet = 20;

    public ucThuoc()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvThuoc);
        GiaoDien.Nut(btnThem);
        GiaoDien.Nut(btnSua, GiaoDien.KieuNut.Phu);
        NapLai();
    }

    private void NapLai()
    {
        var tk = txtTim.Text.Trim();
        var ds = Gia.Thuoc.Where(t =>
                (tk.Length == 0
                 || t.MaThuoc.Contains(tk, StringComparison.OrdinalIgnoreCase)
                 || t.TenThuoc.Contains(tk, StringComparison.OrdinalIgnoreCase)
                 || (t.Nsx ?? "").Contains(tk, StringComparison.OrdinalIgnoreCase))
                && (!chkSapHet.Checked || t.SoLuongTon < NguongSapHet))
            .ToList();
        dgvThuoc.DataSource = ds;
        lblDem.Text = $"{ds.Count} thuốc · {Gia.Thuoc.Count(t => t.SoLuongTon < NguongSapHet)} thuốc sắp hết";
    }

    private ThuocDTO? DangChon => dgvThuoc.CurrentRow?.DataBoundItem as ThuocDTO;

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void btnThem_Click(object? sender, EventArgs e)
    {
        using var f = new frmThuocChiTiet(null);
        if (f.ShowDialog(this) == DialogResult.OK) NapLai();
    }

    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (DangChon is null) { MessageBox.Show("Hãy chọn một thuốc.", "Thuốc"); return; }
        using var f = new frmThuocChiTiet(DangChon);
        if (f.ShowDialog(this) == DialogResult.OK) NapLai();
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
        if (col == "colTon" && e.Value is int ton && ton < NguongSapHet)
        {
            e.CellStyle!.ForeColor = Mau.Red;
            e.CellStyle.Font = GiaoDien.ChuDam;
        }
    }
}
