using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Danh sách nhân viên (Quản trị viên): tìm theo mã, họ tên, SĐT; lọc theo chức vụ; thêm, sửa, xóa.</summary>
public partial class ucNhanVien : UserControl
{
    private const string TatCa = "(Tất cả chức vụ)";
    private readonly NhanVienBUS _bus = new();
    private readonly KhoaBUS _khoaBus = new();
    private bool _dangNap = true;

    public ucNhanVien()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgv);
        txtTim.PlaceholderText = "Mã, họ tên, số điện thoại...";
        cboChucVu.Items.Add(TatCa);
        cboChucVu.Items.AddRange(ChucVu.TatCa);
        cboChucVu.SelectedIndex = 0;
        _dangNap = false;
        NapLai();
    }

    private NhanVienDTO? DangChon => dgv.CurrentRow?.DataBoundItem as NhanVienDTO;

    private void NapLai(string? chonMa = null)
    {
        if (_dangNap) return;
        ThongBaoLoi.Chay(this, () =>
        {
            var chucVu = cboChucVu.SelectedIndex > 0 ? cboChucVu.SelectedItem as string : null;
            var ds = _bus.DanhSach(txtTim.Text, chucVu);
            dgv.DataSource = ds;
            lblDem.Text = $"{ds.Count} nhân viên · {ds.Count(n => n.ChucVu == ChucVu.BacSi)} bác sĩ";
            GiaoDien.ChonDong(dgv, chonMa is null ? null : (object o) => ((NhanVienDTO)o).MaNv == chonMa);
        });
    }

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void MoChiTiet(NhanVienDTO? nv)
    {
        List<KhoaDTO> khoa = [];
        if (!ThongBaoLoi.Chay(this, () => khoa = _khoaBus.DanhSach())) return;
        using var f = new frmNhanVienChiTiet(nv, khoa);
        if (f.ShowDialog(this) == DialogResult.OK) NapLai(f.MaDaLuu);
    }

    private void btnThem_Click(object? sender, EventArgs e) => MoChiTiet(null);

    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (DangChon is null) { MessageBox.Show(this, "Hãy chọn một nhân viên.", "Nhân viên"); return; }
        MoChiTiet(DangChon);
    }

    private void btnXoa_Click(object? sender, EventArgs e)
    {
        var nv = DangChon;
        if (nv is null) { MessageBox.Show(this, "Hãy chọn một nhân viên.", "Nhân viên"); return; }
        if (MessageBox.Show(this, $"Xóa nhân viên \"{nv.HoTenNv}\" ({nv.MaNv})?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        if (ThongBaoLoi.Chay(this, () => _bus.Xoa(nv.MaNv))) NapLai();
    }

    private void dgv_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnSua_Click(sender, e);
    }
}
