using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Lễ tân tiếp nhận: khách có hẹn đúng ngày (Q10; "Đã đặt" thì xác nhận + tiếp nhận một lần, Q3) và khách vãng lai (RB41).
/// Tiếp nhận tạo Ho_so_kham (ma_nv_tiep_nhan = lễ tân đang đăng nhập) và chuyển lịch hẹn sang "Đã khám" (RB15).
/// Module W5 thay dữ liệu giả bằng TiepNhanBUS.
/// </summary>
public partial class ucTiepNhan : UserControl
{
    private readonly List<LichHenDTO> _hen = Gia.LichHen().Where(l => l.NgayKham == Gia.HomNay && l.TrangThai != TrangThai.DaHuy).ToList();
    private readonly List<HoSoKhamDTO> _hoSo = Gia.HoSoHomNay();
    private KhachHangDTO? _khach;

    public ucTiepNhan()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHen);
        GiaoDien.Luoi(dgvHoSo);
        cboBacSi.Items.AddRange(["Nguyễn Văn Hùng · Nội tổng quát · Phòng 101", "Phạm Thị Hồng Nhung · Nhi · Phòng 201",
            "Lâm Thị Kiều Oanh · Tai Mũi Họng · Phòng 301", "Quách Thị Minh Thư · Da liễu · Phòng 401"]);
        cboBacSi.SelectedIndex = 0;
        NapLai();
    }

    private void NapLai()
    {
        dgvHen.DataSource = null;
        dgvHen.DataSource = _hen.OrderBy(h => h.TrangThai == TrangThai.DaKham).ThenBy(h => h.TenCa).ToList();
        dgvHoSo.DataSource = null;
        dgvHoSo.DataSource = _hoSo.OrderByDescending(h => h.NgayGioKham).ToList();
        dgvHen_SelectionChanged(null, EventArgs.Empty);
    }

    private void dgvHen_SelectionChanged(object? sender, EventArgs e)
    {
        var h = dgvHen.CurrentRow?.DataBoundItem as LichHenDTO;
        btnTiepNhanHen.Enabled = h?.TrangThai is TrangThai.DaDat or TrangThai.DaXacNhan;
        btnTiepNhanHen.Text = h?.TrangThai == TrangThai.DaDat ? "Xác nhận và tiếp nhận" : "Tiếp nhận và tạo hồ sơ";
    }

    private void btnTiepNhanHen_Click(object? sender, EventArgs e)
    {
        if (dgvHen.CurrentRow?.DataBoundItem is not LichHenDTO h) return;
        h.TrangThai = TrangThai.DaKham;
        _hoSo.Add(new HoSoKhamDTO
        {
            MaHoSo = $"HS{25 + _hoSo.Count - 4:0000}", NgayGioKham = DateTime.Now, TenKhachHang = h.TenKhachHang,
            TenBacSi = h.TenBacSi, TrieuChung = h.TrieuChungBanDau, MaLichHen = h.MaLichHen, TrangThai = TrangThai.ChoKham
        });
        NapLai();
    }

    private void btnTim_Click(object? sender, EventArgs e)
    {
        _khach = Gia.KhachHang.FirstOrDefault(k => k.SdtKh == txtSdt.Text.Trim());
        txtHoTen.Enabled = _khach is null;
        txtHoTen.Text = _khach?.HoTenKh ?? "";
        lblKhach.ForeColor = _khach is null ? Color.Red : Color.DarkGreen;
        lblKhach.Text = _khach is null ? "Khách mới: nhập họ tên bên dưới." : $"Đã có: {_khach.MaKh} – {_khach.HoTenKh}";
    }

    private void btnTiepNhanVangLai_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtHoTen.Text)) { MessageBox.Show("Hãy tìm khách theo SĐT hoặc nhập họ tên khách mới.", "Tiếp nhận"); return; }
        _hoSo.Add(new HoSoKhamDTO
        {
            MaHoSo = $"HS{25 + _hoSo.Count - 4:0000}", NgayGioKham = DateTime.Now, TenKhachHang = txtHoTen.Text.Trim(),
            TenBacSi = cboBacSi.Text.Split(" · ")[0], TrieuChung = txtTrieuChung.Text.Trim(), TrangThai = TrangThai.ChoKham
        });
        txtSdt.Clear(); txtHoTen.Clear(); txtTrieuChung.Clear(); lblKhach.Text = ""; txtHoTen.Enabled = true;
        NapLai();
    }
}
