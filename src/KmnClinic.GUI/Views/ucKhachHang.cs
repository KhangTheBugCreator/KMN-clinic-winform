using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Danh sách khách hàng. Quản trị viên và Lễ tân thêm, sửa; chỉ Quản trị viên thấy nút Xóa.
/// Khách đã có lịch hẹn hoặc hồ sơ khám thì không xóa được.
/// </summary>
public partial class ucKhachHang : UserControl
{
    private const string TatCa = "(Tất cả)";
    private readonly KhachHangBUS _bus = new();
    private bool _dangNap = true;

    public ucKhachHang()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgv);
        txtTim.PlaceholderText = "Mã, họ tên, số điện thoại...";
        cboGioiTinh.Items.Add(TatCa);
        cboGioiTinh.Items.AddRange(KhachHangBUS.GioiTinh);
        cboGioiTinh.SelectedIndex = 0;
        btnXoa.Visible = PhanQuyen.Co(ChucVu.QuanTriVien);
        _dangNap = false;
        NapLai();
    }

    private KhachHangDTO? DangChon => dgv.CurrentRow?.DataBoundItem as KhachHangDTO;

    private void NapLai(string? chonMa = null)
    {
        if (_dangNap) return;
        ThongBaoLoi.Chay(this, () =>
        {
            var gt = cboGioiTinh.SelectedIndex > 0 ? cboGioiTinh.SelectedItem as string : null;
            var ds = _bus.DanhSach(txtTim.Text, gt);
            dgv.DataSource = ds;
            lblDem.Text = $"{ds.Count} khách hàng";
            GiaoDien.ChonDong(dgv, chonMa is null ? null : (object o) => ((KhachHangDTO)o).MaKh == chonMa);
        });
    }

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void btnThem_Click(object? sender, EventArgs e)
    {
        // Ô tìm đang là một số điện thoại thì điền sẵn vào form thêm
        var tk = txtTim.Text.Trim();
        using var f = new frmKhachHangChiTiet(null, tk.Length == 10 && tk.All(char.IsDigit) ? tk : null);
        if (f.ShowDialog(this) == DialogResult.OK)
        {
            txtTim.Clear();
            NapLai(f.MaDaLuu);
        }
    }

    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (DangChon is null) { MessageBox.Show(this, "Hãy chọn một khách hàng.", "Khách hàng"); return; }
        using var f = new frmKhachHangChiTiet(DangChon);
        if (f.ShowDialog(this) == DialogResult.OK) NapLai(f.MaDaLuu);
    }

    private void btnXoa_Click(object? sender, EventArgs e)
    {
        var kh = DangChon;
        if (kh is null) { MessageBox.Show(this, "Hãy chọn một khách hàng.", "Khách hàng"); return; }
        if (MessageBox.Show(this, $"Xóa khách hàng \"{kh.HoTenKh}\" ({kh.MaKh})?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        if (ThongBaoLoi.Chay(this, () => _bus.Xoa(kh.MaKh))) NapLai();
    }

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnSua_Click(sender, e);
    }
}
