using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Danh mục ca làm việc (Quản trị viên). Giờ bắt đầu phải nhỏ hơn giờ kết thúc (RB3).
/// Ca đã được xếp lịch thì chỉ được đổi tên, không đổi giờ và không xóa.
/// </summary>
public partial class ucCa : UserControl
{
    private readonly CaBUS _bus = new();
    private CheDoNhap _cheDo = CheDoNhap.Xem;

    public ucCa()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgv);
        DatCheDo(CheDoNhap.Xem);
        NapLai();
    }

    private CaDTO? DangChon => dgv.CurrentRow?.DataBoundItem as CaDTO;

    private void NapLai(string? chonMa = null)
    {
        ThongBaoLoi.Chay(this, () =>
        {
            // Ca chỉ có vài dòng nên lấy hết rồi lọc tại chỗ
            var tk = txtTim.Text.Trim();
            var ds = _bus.DanhSach()
                .Where(c => tk.Length == 0 || c.MaCa.Contains(tk, StringComparison.OrdinalIgnoreCase)
                                           || c.TenCa.Contains(tk, StringComparison.OrdinalIgnoreCase))
                .ToList();
            dgv.DataSource = ds;
            lblDem.Text = $"{ds.Count} ca";
            GiaoDien.ChonDong(dgv, chonMa is null ? null : (object o) => ((CaDTO)o).MaCa == chonMa);
            HienDongChon();
        });
    }

    private void HienDongChon()
    {
        if (_cheDo != CheDoNhap.Xem) return;
        var c = DangChon;
        txtMa.Text = c?.MaCa;
        txtTen.Text = c?.TenCa;
        dtpBatDau.Value = DateTime.Today + (c?.GioBatDau ?? new TimeSpan(7, 0, 0));
        dtpKetThuc.Value = DateTime.Today + (c?.GioKetThuc ?? new TimeSpan(11, 0, 0));
    }

    private void DatCheDo(CheDoNhap cheDo)
    {
        _cheDo = cheDo;
        var nhap = cheDo != CheDoNhap.Xem;
        txtTen.ReadOnly = !nhap;
        dtpBatDau.Enabled = dtpKetThuc.Enabled = nhap;
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
        txtTen.Focus();
    }

    private void btnSua_Click(object? sender, EventArgs e)
    {
        var c = DangChon;
        if (c is null) { MessageBox.Show(this, "Hãy chọn một ca.", "Ca làm việc"); return; }
        DatCheDo(CheDoNhap.Sua);
        // Ca đã được xếp lịch thì chỉ đổi tên
        dtpBatDau.Enabled = dtpKetThuc.Enabled = c.SoLichLamViec == 0;
        txtTen.Focus();
    }

    private void btnXoa_Click(object? sender, EventArgs e)
    {
        var c = DangChon;
        if (c is null) { MessageBox.Show(this, "Hãy chọn một ca.", "Ca làm việc"); return; }
        if (MessageBox.Show(this, $"Xóa ca \"{c.TenCa}\" ({c.MaCa})?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        if (ThongBaoLoi.Chay(this, () => _bus.Xoa(c.MaCa))) NapLai();
    }

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        string? ma = null;
        var bd = dtpBatDau.Value.TimeOfDay;
        var kt = dtpKetThuc.Value.TimeOfDay;
        var ok = ThongBaoLoi.Chay(this, () =>
        {
            if (_cheDo == CheDoNhap.Them) ma = _bus.Them(txtTen.Text, bd, kt);
            else { ma = DangChon!.MaCa; _bus.Sua(ma, txtTen.Text, bd, kt); }
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
