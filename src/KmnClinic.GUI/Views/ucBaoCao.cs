using System.Data;
using KmnClinic.BUS;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Báo cáo của quản trị viên: chọn loại và kỳ, xem trên lưới (DataTable từ SqlDataAdapter), xuất CSV mở bằng Excel.
/// </summary>
public partial class ucBaoCao : UserControl
{
    private readonly ThongKeBUS _bus = new();
    private DataTable? _bang;

    public ucBaoCao()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvBaoCao);
        // Mỗi loại báo cáo có cột khác nhau: để lưới tự sinh cột từ DataTable (GiaoDien.Luoi tắt mặc định này)
        dgvBaoCao.AutoGenerateColumns = true;
        cboLoai.Items.AddRange(ThongKeBUS.CacBaoCao);
        dtpDen.Value = DateTime.Today;
        dtpTu.Value = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
        cboLoai.SelectedIndex = 0;
    }

    /// <summary>Tồn kho là số hiện tại, không theo kỳ.</summary>
    private void cboLoai_SelectedIndexChanged(object? sender, EventArgs e)
        => dtpTu.Enabled = dtpDen.Enabled = cboLoai.SelectedIndex != 3;

    private void btnXem_Click(object? sender, EventArgs e)
    {
        DataTable bang = new();
        if (!ThongBaoLoi.Chay(this, () => bang = _bus.LapBaoCao(cboLoai.SelectedIndex, dtpTu.Value, dtpDen.Value))) return;
        _bang = bang;
        dgvBaoCao.DataSource = null; // xóa cột của báo cáo trước
        dgvBaoCao.DataSource = bang;
        var tien = bang.Columns.Cast<DataColumn>().LastOrDefault(c => c.ColumnName is "Tổng tiền" or "Doanh thu" or "Thành tiền");
        lblTong.Text = $"{bang.Rows.Count} dòng" +
                       (tien is null ? "" : $" · {tien.ColumnName}: {GiaoDien.Tien(bang.AsEnumerable().Sum(r => r.Field<decimal>(tien)))}");
    }

    /// <summary>Định dạng cột theo kiểu dữ liệu: tiền có phân cách nghìn, ngày giờ dd/MM/yyyy HH:mm.</summary>
    private void dgvBaoCao_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewColumn c in dgvBaoCao.Columns)
        {
            if (c.ValueType == typeof(decimal))
            {
                c.DefaultCellStyle.Format = c.HeaderText == "Hàm lượng" ? "0.##" : "#,0";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (c.ValueType == typeof(int)) c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            else if (c.ValueType == typeof(DateTime)) c.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
        }
    }

    private void btnXuat_Click(object? sender, EventArgs e)
    {
        if (_bang is null)
        {
            MessageBox.Show(this, "Hãy bấm Xem để lập báo cáo trước khi xuất.", "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        var loai = cboLoai.Text;
        var ky = cboLoai.SelectedIndex == 3 ? $"ngày {DateTime.Today:dd/MM/yyyy}" : $"từ {dtpTu.Value:dd/MM/yyyy} đến {dtpDen.Value:dd/MM/yyyy}";
        using var luu = new SaveFileDialog
        {
            Filter = "CSV mở bằng Excel (*.csv)|*.csv",
            FileName = $"BaoCao_{DateTime.Now:yyyyMMdd_HHmm}.csv",
        };
        if (luu.ShowDialog(this) != DialogResult.OK) return;
        if (ThongBaoLoi.Chay(this, () => ThongKeBUS.XuatCsv(_bang, luu.FileName, $"KMN Clinic - {loai} {ky}")))
            MessageBox.Show(this, $"Đã xuất {_bang.Rows.Count} dòng ra {luu.FileName}.", "Xuất báo cáo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
