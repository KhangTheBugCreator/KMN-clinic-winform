using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Xếp lịch làm việc cho một bác sĩ. Thêm: chọn khoảng ngày và các thứ (từ ngày = đến ngày là xếp một ngày);
/// cả lô được lưu hoặc không ngày nào. Sửa: chỉ một ngày, chỉ khi lịch chưa có lịch hẹn và chưa qua.
/// Phòng được lọc theo khoa của bác sĩ (RB11).
/// </summary>
public partial class frmXepLich : Form
{
    private sealed record LuaChon(string Ma, string Ten, string MaKhoa);

    private readonly LichLamViecBUS _bus = new();
    private readonly LichLamViecDTO? _lich;
    private readonly List<PhongDTO> _phong;
    private readonly string? _maPhongChon;

    /// <summary>Ngày đầu tiên vừa xếp/sửa, để lưới chuyển tới tuần đó.</summary>
    public DateTime? NgayDaLuu { get; private set; }

    /// <param name="lich">Lịch cần sửa; null là thêm mới.</param>
    /// <param name="ngay">Ngày gợi ý khi thêm (ô vừa bấm trên lưới).</param>
    public frmXepLich(LichLamViecDTO? lich, DateTime? ngay = null, string? maPhong = null, string? maCa = null)
    {
        InitializeComponent();
        _lich = lich;
        _phong = new PhongBUS().DanhSach();
        _maPhongChon = lich?.MaPhong ?? maPhong;

        var bacSi = _bus.DanhSachBacSi().Select(b => new LuaChon(b.MaNv, $"{b.HoTenNv} ({b.TenKhoa})", b.MaKhoa ?? "")).ToList();
        // Ô phòng đã chọn sẵn thì chỉ gợi ý bác sĩ cùng khoa với phòng đó.
        var khoaPhong = _phong.FirstOrDefault(p => p.MaPhong == _maPhongChon)?.MaKhoa;
        if (lich is null && khoaPhong is not null) bacSi = bacSi.Where(b => b.MaKhoa == khoaPhong).ToList();
        cboBacSi.DisplayMember = nameof(LuaChon.Ten);
        cboBacSi.ValueMember = nameof(LuaChon.Ma);
        cboBacSi.DataSource = bacSi;
        cboPhong.DisplayMember = nameof(PhongDTO.TenPhong);
        cboPhong.ValueMember = nameof(PhongDTO.MaPhong);
        cboCa.DataSource = new CaBUS().DanhSach();
        cboCa.DisplayMember = nameof(CaDTO.TenCa);
        cboCa.ValueMember = nameof(CaDTO.MaCa);
        cboCa.Format += (_, e) => { if (e.ListItem is CaDTO c) e.Value = c.ToString(); };

        var ngayMai = DateTime.Today.AddDays(1);
        dtpTuNgay.MinDate = dtpDenNgay.MinDate = ngayMai;
        dtpTuNgay.MaxDate = dtpDenNgay.MaxDate = DateTime.Today.AddYears(1);
        if (lich is null)
        {
            Text = "Xếp lịch làm việc";
            var d = ngay is { } g && g.Date >= ngayMai ? g.Date : ngayMai;
            dtpTuNgay.Value = d;
            dtpDenNgay.Value = d;
            lblLoi.ForeColor = SystemColors.GrayText;
            lblLoi.Text = $"Từ ngày = đến ngày là xếp một ngày. Tối đa {LichLamViecBUS.SoNgayToiDa} ngày một lần; "
                + "một ngày bị trùng thì không ngày nào được lưu.";
        }
        else
        {
            Text = $"Sửa lịch {lich.MaLichLamViec}";
            lblTuNgay.Text = "Ngày:";
            lblDenNgay.Visible = dtpDenNgay.Visible = lblThu.Visible = flpThu.Visible = false;
            if (lich.NgayLamViec.Date >= ngayMai) dtpTuNgay.Value = lich.NgayLamViec.Date;
        }
        _maCaChon = lich?.MaCa ?? maCa;
    }

    private readonly string? _maCaChon;

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_lich is not null) cboBacSi.SelectedValue = _lich.MaNv;
        LocPhong();
        if (_maPhongChon is not null) cboPhong.SelectedValue = _maPhongChon;
        if (_maCaChon is not null) cboCa.SelectedValue = _maCaChon;
    }

    private void cboBacSi_SelectedIndexChanged(object? sender, EventArgs e) => LocPhong();

    /// <summary>RB11: chỉ hiện phòng thuộc khoa của bác sĩ đang chọn, giữ phòng đang chọn nếu còn hợp lệ.</summary>
    private void LocPhong()
    {
        if (_phong is null) return;
        var cu = cboPhong.SelectedValue as string;
        var khoa = (cboBacSi.SelectedItem as LuaChon)?.MaKhoa;
        cboPhong.DataSource = _phong.Where(p => p.MaKhoa == khoa).ToList();
        if (cu is not null) cboPhong.SelectedValue = cu;
    }

    private void dtpTuNgay_ValueChanged(object? sender, EventArgs e)
    {
        if (dtpDenNgay.Value < dtpTuNgay.Value) dtpDenNgay.Value = dtpTuNgay.Value;
    }

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        var nv = cboBacSi.SelectedValue as string;
        var phong = cboPhong.SelectedValue as string;
        var ca = cboCa.SelectedValue as string;
        try
        {
            if (_lich is null)
            {
                var thu = new List<DayOfWeek>();
                if (chkT2.Checked) thu.Add(DayOfWeek.Monday);
                if (chkT3.Checked) thu.Add(DayOfWeek.Tuesday);
                if (chkT4.Checked) thu.Add(DayOfWeek.Wednesday);
                if (chkT5.Checked) thu.Add(DayOfWeek.Thursday);
                if (chkT6.Checked) thu.Add(DayOfWeek.Friday);
                if (chkT7.Checked) thu.Add(DayOfWeek.Saturday);
                if (chkCN.Checked) thu.Add(DayOfWeek.Sunday);
                var ma = _bus.ThemHangLoat(dtpTuNgay.Value.Date, dtpDenNgay.Value.Date, thu, nv, phong, ca);
                MessageBox.Show(this, $"Đã xếp {ma.Count} ngày làm việc.", "Xếp lịch", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                _bus.Sua(_lich.MaLichLamViec, dtpTuNgay.Value.Date, nv, phong, ca);
            }
            NgayDaLuu = dtpTuNgay.Value.Date;
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            lblLoi.ForeColor = Color.DarkRed;
            lblLoi.Text = ThongBaoLoi.CauThongBao(ex);
        }
    }
}
