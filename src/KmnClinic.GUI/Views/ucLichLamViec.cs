using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Lịch làm việc dạng lưới tuần: dòng = phòng × ca, cột = 7 ngày.
/// Quản trị viên xem mọi khoa và xếp lịch; bác sĩ chỉ thấy ca của mình (quyết định M3 của bản web).
/// Module W3 thay dữ liệu giả bằng LichLamViecBUS.
/// </summary>
public partial class ucLichLamViec : UserControl
{
    private readonly bool _laQuanTri;
    private DateTime _thu2;

    public ucLichLamViec() : this(true) { }

    public ucLichLamViec(bool laQuanTri)
    {
        InitializeComponent();
        _laQuanTri = laQuanTri;
        GiaoDien.Luoi(dgvLich);
        dgvLich.SelectionMode = DataGridViewSelectionMode.CellSelect;
        dgvLich.RowTemplate.Height = 58;
        dgvLich.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        dgvLich.CellFormatting += dgvLich_CellFormatting;
        foreach (var b in new[] { btnTruoc, btnSau, btnHomNay })
        cboKhoa.Items.Add("Tất cả khoa");
        foreach (var k in Gia.Khoa) cboKhoa.Items.Add(k.TenKhoa);
        cboKhoa.SelectedIndex = 0;
        cboKhoa.Visible = btnXepLich.Visible = laQuanTri;
        lblGhiChu.Text = laQuanTri
            ? "Chỉ xếp lịch từ ngày mai trở đi. Một phòng một ca một ngày chỉ có một bác sĩ (RB12); bác sĩ không trực hai nơi cùng ca (RB13)."
            : "Các ô có tên bạn là ca trực của bạn. Lịch do quản trị viên xếp; bác sĩ chỉ xem.";

        _thu2 = DauTuan(Gia.HomNay);
        Ve();
    }

    private static DateTime DauTuan(DateTime d) => d.Date.AddDays(-(((int)d.DayOfWeek + 6) % 7));

    private void Ve()
    {
        lblTuan.Text = $"{_thu2:dd/MM} – {_thu2.AddDays(6):dd/MM/yyyy}";
        var lich = Gia.LichTuan(_thu2);
        if (cboKhoa.SelectedIndex > 0) lich = lich.Where(l => l.TenKhoa == cboKhoa.Text).ToList();
        var toi = PhienDangNhap.NhanVien?.HoTenNv;

        dgvLich.Columns.Clear();
        dgvLich.Rows.Clear();
        dgvLich.Columns.Add("colPhong", "Phòng / Ca");
        dgvLich.Columns[0].FillWeight = 150;
        string[] thu = ["Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "CN"];
        for (int i = 0; i < 7; i++)
        {
            var c = dgvLich.Columns.Add($"colNgay{i}", $"{thu[i]}  {_thu2.AddDays(i):dd/MM}");
            dgvLich.Columns[c].Tag = _thu2.AddDays(i);
        }

        foreach (var nhom in lich.GroupBy(l => (l.TenPhong, l.TenKhoa, l.TenCa)))
        {
            var dong = new object[8];
            dong[0] = $"{nhom.Key.TenPhong} · {nhom.Key.TenCa}\r\n{nhom.Key.TenKhoa}";
            foreach (var l in nhom)
            {
                var i = (l.NgayLamViec - _thu2).Days;
                dong[i + 1] = _laQuanTri || l.TenBacSi == toi ? l.TenBacSi : "";
            }
            dgvLich.Rows.Add(dong);
        }
    }

    private void dgvLich_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.RowIndex < 0 || e.ColumnIndex < 1) return;
        var ngay = (DateTime)dgvLich.Columns[e.ColumnIndex].Tag!;
        if (ngay == Gia.HomNay) e.CellStyle!.BackColor = Color.LightYellow;
        if (!_laQuanTri && e.Value is string s && s.Length > 0)
        {
            e.CellStyle!.BackColor = Color.PaleGreen;
        }
    }

    private void btnTruoc_Click(object? sender, EventArgs e) { _thu2 = _thu2.AddDays(-7); Ve(); }
    private void btnSau_Click(object? sender, EventArgs e) { _thu2 = _thu2.AddDays(7); Ve(); }
    private void btnHomNay_Click(object? sender, EventArgs e) { _thu2 = DauTuan(Gia.HomNay); Ve(); }
    private void cboKhoa_SelectedIndexChanged(object? sender, EventArgs e) { if (_thu2 != default) Ve(); }

    private void btnXepLich_Click(object? sender, EventArgs e)
        => MessageBox.Show("Form xếp lịch hàng loạt (chọn bác sĩ, phòng, ca, khoảng ngày) sẽ dựng ở module W3.", "Xếp lịch");
}
