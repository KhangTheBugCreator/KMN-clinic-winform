using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Lịch làm việc dạng lưới tuần: dòng = phòng × ca, cột = 7 ngày (Thứ 2 → Chủ nhật).
/// Quản trị viên thấy mọi phòng, mọi ca (kể cả ô trống) để xếp, sửa, xóa; bác sĩ chỉ thấy các ca của mình.
/// </summary>
public partial class ucLichLamViec : UserControl
{
    private readonly bool _laQuanTri;
    private readonly LichLamViecBUS _bus = new();
    private List<PhongDTO> _phong = [];
    private List<CaDTO> _ca = [];
    private DateTime _thu2;
    private bool _dangNap = true;
    private readonly string _ghiChu;

    public ucLichLamViec() : this(true) { }

    public ucLichLamViec(bool laQuanTri)
    {
        InitializeComponent();
        _laQuanTri = laQuanTri;
        GiaoDien.Luoi(dgvLich);
        dgvLich.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLich.RowTemplate.Height = 44;
        dgvLich.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvLich.CellFormatting += dgvLich_CellFormatting;
        cboKhoa.Visible = btnXepLich.Visible = btnSua.Visible = btnXoa.Visible = laQuanTri;
        lblGhiChu.Text = laQuanTri
            ? "Bấm đúp ô trống để xếp, ô có tên để sửa; phím Delete để xóa. Chỉ xếp từ ngày mai; một phòng một ca một ngày một bác sĩ (RB12), bác sĩ không trực hai nơi cùng ca (RB13)."
            : "Các ca trực của bạn trong tuần. Lịch do quản trị viên xếp; số trong ngoặc là lịch hẹn còn hiệu lực.";
        _ghiChu = lblGhiChu.Text;
        _thu2 = LichLamViecBUS.DauTuan(DateTime.Today);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        ThongBaoLoi.Chay(this, () =>
        {
            _ca = new CaBUS().DanhSach().OrderBy(c => c.GioBatDau).ToList();
            if (_laQuanTri)
            {
                _phong = new PhongBUS().DanhSach().OrderBy(p => p.TenKhoa).ThenBy(p => p.TenPhong).ToList();
                cboKhoa.Items.Add("Tất cả khoa");
                foreach (var k in new KhoaBUS().DanhSach()) cboKhoa.Items.Add(k);
                cboKhoa.SelectedIndex = 0;
            }
        });
        _dangNap = false;
        Ve();
    }

    private string? MaKhoaLoc => (cboKhoa.SelectedItem as KhoaDTO)?.MaKhoa;

    private void Ve()
    {
        if (_dangNap) return;
        lblTuan.Text = $"{_thu2:dd/MM} – {_thu2.AddDays(6):dd/MM/yyyy}";
        List<LichLamViecDTO> lich = [];
        if (!ThongBaoLoi.Chay(this, () => lich = _bus.DanhSach(_thu2, _thu2.AddDays(6), MaKhoaLoc))) return;

        dgvLich.Rows.Clear();
        dgvLich.Columns.Clear();
        dgvLich.Columns.Add("colPhong", "Phòng / Ca");
        dgvLich.Columns[0].FillWeight = 150;
        string[] thu = ["Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "CN"];
        for (int i = 0; i < 7; i++)
        {
            var c = dgvLich.Columns.Add($"colNgay{i}", $"{thu[i]}  {_thu2.AddDays(i):dd/MM}");
            dgvLich.Columns[c].Tag = _thu2.AddDays(i);
            dgvLich.Columns[c].SortMode = DataGridViewColumnSortMode.NotSortable;
        }

        // Quản trị viên: mọi phòng × ca (ô trống để xếp). Bác sĩ: chỉ các dòng có ca của mình.
        var dong = _laQuanTri
            ? (from p in _phong where MaKhoaLoc is null || p.MaKhoa == MaKhoaLoc
               from c in _ca select (p.MaPhong, p.TenPhong, p.TenKhoa, c.MaCa, Ca: c.ToString())).ToList()
            : lich.Select(l => (l.MaPhong, l.TenPhong, l.TenKhoa, l.MaCa, Ca: $"{l.TenCa} ({l.GioBatDau:hh\\:mm} – {l.GioKetThuc:hh\\:mm})"))
                  .Distinct().ToList();
        var thuTuCa = _ca.Select((c, i) => (c.MaCa, i)).ToDictionary(x => x.MaCa, x => x.i);
        if (!_laQuanTri) dong = dong.OrderBy(d => d.TenPhong).ThenBy(d => thuTuCa.GetValueOrDefault(d.MaCa)).ToList();

        foreach (var d in dong)
        {
            var r = dgvLich.Rows[dgvLich.Rows.Add()];
            r.Tag = (d.MaPhong, d.MaCa);
            r.Cells[0].Value = $"{d.TenPhong} · {d.Ca}\r\n{d.TenKhoa}";
            foreach (var l in lich.Where(l => l.MaPhong == d.MaPhong && l.MaCa == d.MaCa))
            {
                var o = r.Cells[(l.NgayLamViec.Date - _thu2).Days + 1];
                o.Tag = l;
                o.Value = l.SoLichHenConHieuLuc > 0 ? $"{l.TenBacSi} ({l.SoLichHenConHieuLuc})" : l.TenBacSi;
            }
        }
        lblGhiChu.Text = dgvLich.Rows.Count == 0 && !_laQuanTri ? "Tuần này bạn chưa có ca trực nào." : _ghiChu;
    }

    private void dgvLich_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 1) return;
        var ngay = (DateTime)dgvLich.Columns[e.ColumnIndex].Tag!;
        if (ngay == DateTime.Today) e.CellStyle!.BackColor = Color.LightYellow;
        else if (ngay < DateTime.Today) e.CellStyle!.ForeColor = SystemColors.GrayText;
    }

    private LichLamViecDTO? LichDangChon => dgvLich.CurrentCell?.Tag as LichLamViecDTO;

    private void dgvLich_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (!_laQuanTri || e.RowIndex < 0 || e.ColumnIndex < 1) return;
        var o = dgvLich.Rows[e.RowIndex].Cells[e.ColumnIndex];
        if (o.Tag is LichLamViecDTO l) { SuaLich(l); return; }
        var (maPhong, maCa) = ((string, string))dgvLich.Rows[e.RowIndex].Tag!;
        MoForm(new frmXepLich(null, (DateTime)dgvLich.Columns[e.ColumnIndex].Tag!, maPhong, maCa));
    }

    private void dgvLich_KeyDown(object? sender, KeyEventArgs e)
    {
        if (_laQuanTri && e.KeyCode == Keys.Delete) { btnXoa_Click(sender, e); e.Handled = true; }
    }

    private void btnXepLich_Click(object? sender, EventArgs e) => MoForm(new frmXepLich(null));

    private void btnSua_Click(object? sender, EventArgs e)
    {
        if (LichDangChon is { } l) SuaLich(l);
        else MessageBox.Show(this, "Hãy chọn một ô có lịch để sửa.", "Sửa lịch");
    }

    private void SuaLich(LichLamViecDTO l)
    {
        if (l.NgayLamViec.Date <= DateTime.Today)
        {
            MessageBox.Show(this, "Lịch hôm nay hoặc đã qua không sửa được để giữ lịch sử.", "Sửa lịch",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        MoForm(new frmXepLich(l));
    }

    private void btnXoa_Click(object? sender, EventArgs e)
    {
        if (LichDangChon is not { } l)
        {
            MessageBox.Show(this, "Hãy chọn một ô có lịch để xóa.", "Xóa lịch");
            return;
        }
        if (MessageBox.Show(this, $"Xóa lịch của {l.TenBacSi} ở {l.TenPhong}, {l.TenCa} ngày {l.NgayLamViec:dd/MM/yyyy}?",
                "Xóa lịch", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        if (ThongBaoLoi.Chay(this, () => _bus.Xoa(l.MaLichLamViec))) Ve();
    }

    private void MoForm(frmXepLich f)
    {
        using (f)
        {
            if (f.ShowDialog(this) != DialogResult.OK) return;
            if (f.NgayDaLuu is { } d) _thu2 = LichLamViecBUS.DauTuan(d);
        }
        Ve();
    }

    private void btnTruoc_Click(object? sender, EventArgs e) { _thu2 = _thu2.AddDays(-7); Ve(); }
    private void btnSau_Click(object? sender, EventArgs e) { _thu2 = _thu2.AddDays(7); Ve(); }
    private void btnHomNay_Click(object? sender, EventArgs e) { _thu2 = LichLamViecBUS.DauTuan(DateTime.Today); Ve(); }
    private void cboKhoa_SelectedIndexChanged(object? sender, EventArgs e) => Ve();
}
