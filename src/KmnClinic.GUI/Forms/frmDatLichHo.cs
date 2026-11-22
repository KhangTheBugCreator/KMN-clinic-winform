using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Lễ tân đặt lịch hộ khách gọi điện (quyết định C2: khách không dùng app).
/// Thứ tự giống 5 bước đặt lịch của web: khách → khoa/bác sĩ → ngày/ca còn đặt được (RB16) → triệu chứng → xác nhận.
/// Đề xuất (chốt ở W4): lịch lễ tân đặt hộ coi như đã xác nhận. Trigger TG_LICH_HEN_KiemTra bắt INSERT phải là "Đã đặt",
/// nên W4 sẽ INSERT "Đã đặt" rồi UPDATE sang "Đã xác nhận" (ghi ma_nv lễ tân) trong cùng một transaction.
/// Module W4 thay dữ liệu giả bằng KhachHangBUS + LichHenBUS (chống trùng Q6 bằng UPDLOCK).
/// </summary>
public partial class frmDatLichHo : Form
{
    private sealed record SuatTrong(string TenCa, string KhungGio, string Phong, int ConLai)
    {
        public override string ToString() => $"{TenCa}  {KhungGio}  ·  {Phong}";
    }

    private KhachHangDTO? _khach;

    public LichHenDTO? LichHenMoi { get; private set; }

    public frmDatLichHo()
    {
        InitializeComponent();
        cboKhoa.Items.AddRange(Gia.Khoa.ToArray());
        cboKhoa.SelectedIndex = 0;
        cboGioiTinh.SelectedIndex = 0;
        dtpNgaySinh.Value = new DateTime(1990, 1, 1);
        dtpNgayKham.MinDate = Gia.HomNay;
        dtpNgayKham.Value = Gia.HomNay.AddDays(1);
        CapNhatTomTat();
    }

    private void btnTim_Click(object? sender, EventArgs e)
    {
        _khach = Gia.KhachHang.FirstOrDefault(k => k.SdtKh == txtSdt.Text.Trim());
        var moi = _khach is null;
        txtHoTen.Enabled = cboGioiTinh.Enabled = dtpNgaySinh.Enabled = moi;
        if (_khach is null)
        {
            lblKetQua.ForeColor = Color.Red;
            lblKetQua.Text = "Chưa có khách với SĐT này. Điền họ tên, giới tính, ngày sinh để tạo khách mới khi đặt lịch.";
            txtHoTen.Clear();
            txtHoTen.Focus();
        }
        else
        {
            lblKetQua.ForeColor = Color.DarkGreen;
            lblKetQua.Text = $"Đã có: {_khach.MaKh} – {_khach.HoTenKh}\r\n{_khach.DiaChiKh}";
            txtHoTen.Text = _khach.HoTenKh;
            cboGioiTinh.Text = _khach.GioiTinhKh;
            if (_khach.NgaySinhKh is { } ns) dtpNgaySinh.Value = ns;
        }
        CapNhatTomTat();
    }

    private void cboKhoa_SelectedIndexChanged(object? sender, EventArgs e)
    {
        var khoa = (KhoaDTO)cboKhoa.SelectedItem!;
        var chiSo = Gia.Khoa.IndexOf(khoa);
        cboBacSi.Items.Clear();
        cboBacSi.Items.Add("Bác sĩ bất kỳ còn chỗ");
        cboBacSi.Items.AddRange(Gia.BacSi.Skip(chiSo * 2).Take(2).Cast<object>().ToArray());
        cboBacSi.SelectedIndex = 0;
    }

    private void LuaChon_Changed(object? sender, EventArgs e)
    {
        if (sender != lstCa) NapCa();
        CapNhatTomTat();
    }

    private void NapCa()
    {
        lstCa.Items.Clear();
        var phong = Gia.Khoa.IndexOf((KhoaDTO)cboKhoa.SelectedItem!) * 2 + 1;
        // Dữ liệu giả: chủ nhật bác sĩ thứ hai nghỉ để thấy trường hợp ít ca
        foreach (var ca in Gia.Ca)
        {
            if (dtpNgayKham.Value.DayOfWeek == DayOfWeek.Sunday && ca.MaCa == "CA0002") continue;
            lstCa.Items.Add(new SuatTrong(ca.TenCa, ca.KhungGio, $"Phòng {phong}01", 8));
        }
        if (lstCa.Items.Count > 0) lstCa.SelectedIndex = 0;
    }

    private void CapNhatTomTat()
    {
        var khach = _khach?.HoTenKh ?? (txtHoTen.Text.Length > 0 ? txtHoTen.Text + " (khách mới)" : "chưa chọn khách");
        var suat = lstCa.SelectedItem as SuatTrong;
        lblTomTat.Text = $"Tóm tắt: {khach} · {cboKhoa.Text} · {cboBacSi.Text}\r\n"
                       + (suat is null ? "Chưa chọn ca" : $"{dtpNgayKham.Value:dd/MM/yyyy}, {suat}");
    }

    private void btnDat_Click(object? sender, EventArgs e)
    {
        if (txtSdt.Text.Trim().Length != 10) { MessageBox.Show(this, "Số điện thoại phải đủ 10 số.", Text); return; }
        if (_khach is null && string.IsNullOrWhiteSpace(txtHoTen.Text)) { MessageBox.Show(this, "Hãy tìm khách hoặc nhập họ tên khách mới.", Text); return; }
        if (lstCa.SelectedItem is not SuatTrong suat) { MessageBox.Show(this, "Hãy chọn một ca còn đặt được.", Text); return; }

        LichHenMoi = new LichHenDTO
        {
            MaLichHen = "LH0017", NgayGioDat = DateTime.Now, NgayKham = dtpNgayKham.Value.Date, TenCa = suat.TenCa,
            TenKhachHang = _khach?.HoTenKh ?? txtHoTen.Text.Trim(), SdtKhachHang = txtSdt.Text.Trim(),
            TenBacSi = cboBacSi.SelectedIndex > 0 ? cboBacSi.Text : Gia.BacSi[Gia.Khoa.IndexOf((KhoaDTO)cboKhoa.SelectedItem!) * 2],
            TenKhoa = cboKhoa.Text, TrieuChungBanDau = txtTrieuChung.Text.Trim(),
            TrangThai = TrangThai.DaXacNhan, LeTanXacNhan = PhienDangNhap.NhanVien?.HoTenNv
        };
        MessageBox.Show(this, $"Đã đặt lịch {LichHenMoi.MaLichHen} cho {LichHenMoi.TenKhachHang}.\r\nHãy đọc lại ngày giờ cho khách qua điện thoại.", Text,
            MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
    }
}
