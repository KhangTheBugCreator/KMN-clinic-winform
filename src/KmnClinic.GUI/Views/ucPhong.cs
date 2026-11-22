using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Danh mục phòng làm việc (Quản trị viên). Mỗi phòng thuộc một khoa (RB11).
/// Phòng đã có lịch làm việc thì không đổi khoa và không xóa được.
/// </summary>
public partial class ucPhong : UserControl
{
    private const string TatCaKhoa = "(Tất cả khoa)";
    private readonly PhongBUS _bus = new();
    private readonly KhoaBUS _khoaBus = new();
    private CheDoNhap _cheDo = CheDoNhap.Xem;
    private bool _dangNap;

    public ucPhong()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgv);
        ThongBaoLoi.Chay(this, () =>
        {
            var khoa = _khoaBus.DanhSach();
            _dangNap = true;
            cboKhoa.DisplayMember = nameof(KhoaDTO.TenKhoa);
            cboKhoa.ValueMember = nameof(KhoaDTO.MaKhoa);
            cboKhoa.DataSource = khoa;
            cboLocKhoa.Items.Add(TatCaKhoa);
            cboLocKhoa.Items.AddRange(khoa.ToArray<object>());
            cboLocKhoa.SelectedIndex = 0;
            _dangNap = false;
        });
        DatCheDo(CheDoNhap.Xem);
        NapLai();
    }

    private PhongDTO? DangChon => dgv.CurrentRow?.DataBoundItem as PhongDTO;

    private void NapLai(string? chonMa = null)
    {
        if (_dangNap) return;
        ThongBaoLoi.Chay(this, () =>
        {
            var locKhoa = (cboLocKhoa.SelectedItem as KhoaDTO)?.MaKhoa;
            var ds = _bus.DanhSach(txtTim.Text, locKhoa);
            dgv.DataSource = ds;
            lblDem.Text = $"{ds.Count} phòng";
            GiaoDien.ChonDong(dgv, chonMa is null ? null : (object o) => ((PhongDTO)o).MaPhong == chonMa);
            HienDongChon();
        });
    }

    private void HienDongChon()
    {
        if (_cheDo != CheDoNhap.Xem) return;
        var p = DangChon;
        txtMa.Text = p?.MaPhong;
        txtTen.Text = p?.TenPhong;
        if (p is null) cboKhoa.SelectedIndex = -1;
        else cboKhoa.SelectedValue = p.MaKhoa;
    }

    private void DatCheDo(CheDoNhap cheDo)
    {
        _cheDo = cheDo;
        var nhap = cheDo != CheDoNhap.Xem;
        txtTen.ReadOnly = !nhap;
        cboKhoa.Enabled = nhap;
        btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = !nhap;
        btnLuu.Enabled = btnHuy.Enabled = nhap;
        dgv.Enabled = txtTim.Enabled = cboLocKhoa.Enabled = !nhap;
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
        // Đang lọc theo khoa nào thì chọn sẵn khoa đó
        if (cboLocKhoa.SelectedItem is KhoaDTO k) cboKhoa.SelectedValue = k.MaKhoa;
        else cboKhoa.SelectedIndex = -1;
        txtTen.Focus();
    }

    private void btnSua_Click(object? sender, EventArgs e)
    {
        var p = DangChon;
        if (p is null) { MessageBox.Show(this, "Hãy chọn một phòng.", "Phòng làm việc"); return; }
        DatCheDo(CheDoNhap.Sua);
        // RB11: phòng đã có lịch thì không đổi khoa, khóa luôn ô chọn khoa cho rõ
        cboKhoa.Enabled = p.SoLichLamViec == 0;
        txtTen.Focus();
    }

    private void btnXoa_Click(object? sender, EventArgs e)
    {
        var p = DangChon;
        if (p is null) { MessageBox.Show(this, "Hãy chọn một phòng.", "Phòng làm việc"); return; }
        if (MessageBox.Show(this, $"Xóa phòng \"{p.TenPhong}\" ({p.MaPhong})?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes) return;
        if (ThongBaoLoi.Chay(this, () => _bus.Xoa(p.MaPhong))) NapLai();
    }

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        string? ma = null;
        var maKhoa = cboKhoa.SelectedValue as string;
        var ok = ThongBaoLoi.Chay(this, () =>
        {
            if (_cheDo == CheDoNhap.Them) ma = _bus.Them(txtTen.Text, maKhoa);
            else { ma = DangChon!.MaPhong; _bus.Sua(ma, txtTen.Text, maKhoa); }
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
