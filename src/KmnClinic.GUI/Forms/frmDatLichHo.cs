using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Lễ tân đặt lịch hộ khách gọi điện hoặc đến quầy (quyết định C2: khách không dùng app).
/// Thứ tự giống 5 bước đặt lịch của web: khách → khoa/bác sĩ → ngày/ca còn đặt được (RB16) → triệu chứng → xác nhận.
/// Khách mới được thêm cùng transaction với lịch hẹn; lịch đặt hộ được xác nhận luôn, ghi tên lễ tân.
/// </summary>
public partial class frmDatLichHo : Form
{
    private sealed record LuaChon(string? Ma, string Ten)
    {
        public override string ToString() => Ten;
    }

    private readonly LichHenBUS _bus = new();
    private readonly LichLamViecBUS _lich = new();
    private readonly KhachHangBUS _khachBus = new();
    private KhachHangDTO? _khach;
    private bool _dangNap = true;

    public LichHenDTO? LichHenMoi { get; private set; }

    public frmDatLichHo()
    {
        InitializeComponent();
        cboGioiTinh.SelectedIndex = 0;
        dtpNgaySinh.MaxDate = DateTime.Today;
        dtpNgaySinh.Value = new DateTime(1990, 1, 1);
        dtpNgayKham.MinDate = DateTime.Today;
        dtpNgayKham.MaxDate = DateTime.Today.AddYears(1);
        dtpNgayKham.Value = DateTime.Today.AddDays(1);
        lstCa.Format += (_, e) =>
        {
            if (e.ListItem is LichLamViecDTO l)
                e.Value = $"{l.TenCa} {l.GioBatDau:hh\\:mm}–{l.GioKetThuc:hh\\:mm} · {l.TenBacSi} · {l.TenPhong} · {l.SoLichHenConHieuLuc} hẹn";
        };
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        ThongBaoLoi.Chay(this, () =>
        {
            cboKhoa.Items.Add(new LuaChon(null, "Tất cả chuyên khoa"));
            foreach (var k in new KhoaBUS().DanhSach()) cboKhoa.Items.Add(new LuaChon(k.MaKhoa, k.TenKhoa));
        });
        _dangNap = false;
        if (cboKhoa.Items.Count > 0) cboKhoa.SelectedIndex = 0;
        CapNhatTomTat();
    }

    private void txtSdt_TextChanged(object? sender, EventArgs e)
    {
        // Đổi số điện thoại sau khi tìm thì bỏ khách đã chọn, phải tìm lại.
        if (_khach is null || txtSdt.Text.Trim() == _khach.SdtKh) return;
        _khach = null;
        txtHoTen.Enabled = cboGioiTinh.Enabled = dtpNgaySinh.Enabled = true;
        lblKetQua.ForeColor = SystemColors.ControlText;
        lblKetQua.Text = "Số điện thoại đã đổi, bấm Tìm khách lại.";
        CapNhatTomTat();
    }

    private void btnTim_Click(object? sender, EventArgs e)
    {
        KhachHangDTO? kh = null;
        if (!ThongBaoLoi.Chay(this, () => kh = _khachBus.TimTheoSdt(txtSdt.Text))) return;
        _khach = kh;
        txtHoTen.Enabled = cboGioiTinh.Enabled = dtpNgaySinh.Enabled = kh is null;
        if (kh is null)
        {
            lblKetQua.ForeColor = Color.DarkRed;
            lblKetQua.Text = "Chưa có khách với SĐT này. Điền họ tên, giới tính, ngày sinh; khách mới được lưu cùng lịch hẹn.";
            txtHoTen.Clear();
            txtHoTen.Focus();
        }
        else
        {
            lblKetQua.ForeColor = Color.DarkGreen;
            lblKetQua.Text = $"Đã có: {kh.MaKh} – {kh.HoTenKh}\r\n{kh.DiaChiKh}";
            txtHoTen.Text = kh.HoTenKh;
            cboGioiTinh.Text = kh.GioiTinhHienThi;
            if (kh.NgaySinhKh is { } ns) dtpNgaySinh.Value = ns;
        }
        CapNhatTomTat();
    }

    private void cboKhoa_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_dangNap) return;
        var maKhoa = (cboKhoa.SelectedItem as LuaChon)?.Ma;
        cboBacSi.Items.Clear();
        cboBacSi.Items.Add(new LuaChon(null, "Bác sĩ bất kỳ"));
        ThongBaoLoi.Chay(this, () =>
        {
            foreach (var b in _lich.DanhSachBacSi(maKhoa))
                cboBacSi.Items.Add(new LuaChon(b.MaNv, maKhoa is null ? $"{b.HoTenNv} ({b.TenKhoa})" : b.HoTenNv));
        });
        cboBacSi.SelectedIndex = 0; // gọi LuaChon_Changed → nạp ca
    }

    private void LuaChon_Changed(object? sender, EventArgs e)
    {
        if (_dangNap) return;
        if (sender != lstCa) NapCa();
        CapNhatTomTat();
    }

    private void NapCa()
    {
        var maKhoa = (cboKhoa.SelectedItem as LuaChon)?.Ma;
        var maNv = (cboBacSi.SelectedItem as LuaChon)?.Ma;
        List<LichLamViecDTO> ds = [];
        ThongBaoLoi.Chay(this, () => ds = _bus.CaConDat(dtpNgayKham.Value.Date, maKhoa, maNv));
        lstCa.DataSource = ds;
        lblBuoc3.Text = ds.Count == 0 ? "3. Ngày khám: không còn ca nào đặt được, chọn ngày khác" : "3. Ngày khám và ca còn đặt được";
    }

    private void CapNhatTomTat()
    {
        var khach = _khach?.HoTenKh ?? (txtHoTen.Text.Trim().Length > 0 ? txtHoTen.Text.Trim() + " (khách mới)" : "chưa chọn khách");
        lblTomTat.Text = $"Tóm tắt: {khach}\r\n" + (lstCa.SelectedItem is LichLamViecDTO l
            ? $"{l.NgayLamViec:dd/MM/yyyy}, {l.TenCa} ({l.GioBatDau:hh\\:mm}–{l.GioKetThuc:hh\\:mm}), bác sĩ {l.TenBacSi}, {l.TenPhong}, khoa {l.TenKhoa}"
            : "Chưa chọn ca");
    }

    private void btnDat_Click(object? sender, EventArgs e)
    {
        var l = lstCa.SelectedItem as LichLamViecDTO;
        var khachMoi = _khach is null
            ? new KhachHangDTO
            {
                HoTenKh = txtHoTen.Text, SdtKh = txtSdt.Text, GioiTinhKh = cboGioiTinh.Text, NgaySinhKh = dtpNgaySinh.Value.Date,
            }
            : null;
        try
        {
            var (ma, maKh) = _bus.DatHo(_khach?.MaKh, khachMoi, l?.MaLichLamViec, txtTrieuChung.Text);
            LichHenMoi = new LichHenDTO
            {
                MaLichHen = ma, NgayGioDat = DateTime.Now, NgayKham = l!.NgayLamViec, TenCa = l.TenCa,
                TenKhachHang = _khach?.HoTenKh ?? txtHoTen.Text.Trim(), SdtKhachHang = txtSdt.Text.Trim(),
                TenBacSi = l.TenBacSi, TenKhoa = l.TenKhoa, TrieuChungBanDau = txtTrieuChung.Text.Trim(),
                TrangThai = TrangThai.DaXacNhan, LeTanXacNhan = PhienDangNhap.NhanVien?.HoTenNv,
            };
            MessageBox.Show(this,
                $"Đã đặt lịch {ma} cho {LichHenMoi.TenKhachHang} ({maKh}).\r\n"
                + $"{l.NgayLamViec:dd/MM/yyyy}, {l.TenCa} {l.GioBatDau:hh\\:mm}, bác sĩ {l.TenBacSi}, {l.TenPhong}.\r\n"
                + "Hãy đọc lại ngày giờ cho khách.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            DialogResult = DialogResult.OK;
        }
        catch (Exception ex)
        {
            ThongBaoLoi.Hien(ex, this);
            NapCa(); // ca có thể vừa bắt đầu hoặc khách vừa được đặt ở máy khác
        }
    }
}
