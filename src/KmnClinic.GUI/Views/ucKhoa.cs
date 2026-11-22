using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Danh mục khoa (Quản trị viên). Kiểu nhập liệu cổ điển: chọn dòng trên lưới để xem,
/// bấm Thêm/Sửa để mở ô nhập, Lưu để ghi, Hủy để bỏ. Khoa còn phòng hoặc bác sĩ thì không xóa được.
/// </summary>
public partial class ucKhoa : UserControl
{
    private readonly KhoaBUS _bus = new();
    private CheDoNhap _cheDo = CheDoNhap.Xem;

    public ucKhoa()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgv);
        DatCheDo(CheDoNhap.Xem);
        NapLai();
    }

    private KhoaDTO? DangChon => dgv.CurrentRow?.DataBoundItem as KhoaDTO;

    private void NapLai(string? chonMa = null)
    {
        ThongBaoLoi.Chay(this, () =>
        {
            var ds = _bus.DanhSach(txtTim.Text);
            dgv.DataSource = ds;
            lblDem.Text = $"{ds.Count} khoa";
            GiaoDien.ChonDong(dgv, chonMa is null ? null : (object o) => ((KhoaDTO)o).MaKhoa == chonMa);
            HienDongChon();
        });
    }

    private void HienDongChon()
    {
        if (_cheDo != CheDoNhap.Xem) return;
        var k = DangChon;
        txtMa.Text = k?.MaKhoa;
        txtTen.Text = k?.TenKhoa;
        txtMoTa.Text = k?.MoTa;
    }

    private void DatCheDo(CheDoNhap cheDo)
    {
        _cheDo = cheDo;
        var nhap = cheDo != CheDoNhap.Xem;
        txtTen.ReadOnly = txtMoTa.ReadOnly = !nhap;
        btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = !nhap;
        btnLuu.Enabled = btnHuy.Enabled = nhap;
        dgv.Enabled = txtTim.Enabled = !nhap;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        HienDongChon(); // ô chọn/lưới chỉ có dữ liệu sau khi control đã hiện lên form
    }

    private void dgv_SelectionChanged(object? sender, EventArgs e) => HienDongChon();

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void btnThem_Click(object? sender, EventArgs e)
    {
        DatCheDo(CheDoNhap.Them);
        txtMa.Text = "(tự sinh)";
        txtTen.Clear();
        txtMoTa.Clear();
        txtTen.Focus();
    }

    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (DangChon is null) { MessageBox.Show(this, "Hãy chọn một khoa.", "Khoa"); return; }
        DatCheDo(CheDoNhap.Sua);
        txtTen.Focus();
    }

    private void btnXoa_Click(object? sender, EventArgs e)
    {
        var k = DangChon;
        if (k is null) { MessageBox.Show(this, "Hãy chọn một khoa.", "Khoa"); return; }
        if (MessageBox.Show(this, $"Xóa khoa \"{k.TenKhoa}\" ({k.MaKhoa})?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        if (ThongBaoLoi.Chay(this, () => _bus.Xoa(k.MaKhoa))) NapLai();
    }

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        string? ma = null;
        var ok = ThongBaoLoi.Chay(this, () =>
        {
            if (_cheDo == CheDoNhap.Them) ma = _bus.Them(txtTen.Text, txtMoTa.Text);
            else { ma = DangChon!.MaKhoa; _bus.Sua(ma, txtTen.Text, txtMoTa.Text); }
        });
        if (!ok) return;
        DatCheDo(CheDoNhap.Xem);
        NapLai(ma);
    }

    private void btnHuy_Click(object? sender, EventArgs e)
    {
        DatCheDo(CheDoNhap.Xem);
        HienDongChon();
    }
}
