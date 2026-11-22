using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Thêm / sửa nhân viên (Quản trị viên). Chỉ Bác sĩ chọn khoa (RB6). Mật khẩu bắt buộc khi thêm;
/// khi sửa thì để trống là giữ nguyên, nhập vào là đặt lại mật khẩu. Ảnh là đường dẫn https, xem trước bằng PictureBox.
/// </summary>
public partial class frmNhanVienChiTiet : Form
{
    private readonly NhanVienBUS _bus = new();
    private readonly NhanVienDTO? _nv;

    /// <summary>Mã nhân viên vừa thêm/sửa, để danh sách chọn lại đúng dòng.</summary>
    public string? MaDaLuu { get; private set; }

    public frmNhanVienChiTiet(NhanVienDTO? nv, List<KhoaDTO> dsKhoa)
    {
        InitializeComponent();
        _nv = nv;
        cboChucVu.Items.AddRange(ChucVu.TatCa);
        cboKhoa.DisplayMember = nameof(KhoaDTO.TenKhoa);
        cboKhoa.ValueMember = nameof(KhoaDTO.MaKhoa);
        cboKhoa.DataSource = dsKhoa;

        if (nv is null)
        {
            Text = "Thêm nhân viên";
            txtMa.Text = "(tự sinh)";
            dtpNgaySinh.Checked = false;
            cboChucVu.SelectedItem = ChucVu.LeTan;
            lblGhiChuMatKhau.Text = "(bắt buộc, tối thiểu 8 ký tự)";
        }
        else
        {
            Text = $"Sửa nhân viên {nv.MaNv}";
            txtMa.Text = nv.MaNv;
            txtHoTen.Text = nv.HoTenNv;
            if (nv.NgaySinhNv is { } ns) dtpNgaySinh.Value = ns;
            dtpNgaySinh.Checked = nv.NgaySinhNv is not null; // bỏ tick = không có ngày sinh (cột cho phép NULL)
            txtSdt.Text = nv.SdtNv;
            cboChucVu.SelectedItem = nv.ChucVu;
            txtAnh.Text = nv.DuongDanAnh;
            lblGhiChuMatKhau.Text = "(để trống nếu không đổi)";
            if (nv.SoLienKet > 0)
            {
                // Đã có lịch làm việc, lịch hẹn, hồ sơ hoặc hóa đơn: không đổi chức vụ/khoa
                cboChucVu.Enabled = false;
                lblLoi.ForeColor = SystemColors.GrayText;
                lblLoi.Text = $"Nhân viên đã có {nv.SoLienKet} dữ liệu liên quan nên không đổi được chức vụ và khoa.";
            }
        }
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // Combo có DataSource chỉ nhận SelectedValue sau khi form đã tạo xong
        if (_nv?.MaKhoa is { } maKhoa) cboKhoa.SelectedValue = maKhoa;
        CapNhatOKhoa();
        if (!string.IsNullOrWhiteSpace(txtAnh.Text)) XemAnh();
    }

    private void cboChucVu_SelectedIndexChanged(object? sender, EventArgs e) => CapNhatOKhoa();

    /// <summary>RB6: chỉ Bác sĩ có khoa.</summary>
    private void CapNhatOKhoa()
    {
        var laBacSi = (cboChucVu.SelectedItem as string) == ChucVu.BacSi;
        cboKhoa.Enabled = laBacSi && cboChucVu.Enabled;
        if (!laBacSi) cboKhoa.SelectedIndex = -1;
        else if (cboKhoa.SelectedIndex < 0 && cboKhoa.Items.Count > 0) cboKhoa.SelectedIndex = 0;
    }

    private void btnXemAnh_Click(object? sender, EventArgs e) => XemAnh();

    private void XemAnh()
    {
        picAnh.Image = null;
        try
        {
            var url = KiemTraAnh();
            if (url is not null) picAnh.LoadAsync(url); // tải nền, không làm đơ form
        }
        catch (LoiNghiepVu ex)
        {
            lblLoi.ForeColor = Color.Red;
            lblLoi.Text = ex.Message;
        }
    }

    private string? KiemTraAnh()
    {
        var url = txtAnh.Text.Trim();
        if (url.Length == 0) return null;
        if (!url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) throw new LoiNghiepVu("Đường dẫn ảnh phải bắt đầu bằng https://");
        return url;
    }

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        lblLoi.ForeColor = Color.Red;
        lblLoi.Text = "";
        var nv = new NhanVienDTO
        {
            MaNv = _nv?.MaNv ?? "",
            HoTenNv = txtHoTen.Text,
            NgaySinhNv = dtpNgaySinh.Checked ? dtpNgaySinh.Value.Date : null,
            SdtNv = txtSdt.Text,
            ChucVu = cboChucVu.SelectedItem as string ?? "",
            MaKhoa = cboKhoa.SelectedValue as string,
            DuongDanAnh = txtAnh.Text,
        };
        try
        {
            if (_nv is null) MaDaLuu = _bus.Them(nv, txtMatKhau.Text);
            else { _bus.Sua(nv, txtMatKhau.Text); MaDaLuu = nv.MaNv; }
            DialogResult = DialogResult.OK;
        }
        catch (LoiNghiepVu ex)
        {
            lblLoi.Text = ex.Message;
        }
        catch (Exception ex)
        {
            ThongBaoLoi.Hien(ex, this);
        }
    }
}
