using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.GUI.Forms;

/// <summary>Thêm / sửa khách hàng (Quản trị viên, Lễ tân). Khách không có tài khoản nên không có ô mật khẩu.</summary>
public partial class frmKhachHangChiTiet : Form
{
    private readonly KhachHangBUS _bus = new();
    private readonly KhachHangDTO? _kh;

    public string? MaDaLuu { get; private set; }

    /// <param name="kh">null = thêm mới.</param>
    /// <param name="sdtGoiY">Số điện thoại điền sẵn khi thêm (vd. lễ tân vừa tìm không thấy khách).</param>
    public frmKhachHangChiTiet(KhachHangDTO? kh, string? sdtGoiY = null)
    {
        InitializeComponent();
        _kh = kh;
        dtpNgaySinh.MaxDate = DateTime.Today;
        cboGioiTinh.Items.AddRange(KhachHangBUS.GioiTinh); // Nam, Nữ, Khác (Khác lưu NULL)
        if (kh is null)
        {
            Text = "Thêm khách hàng";
            txtMa.Text = "(tự sinh)";
            dtpNgaySinh.Value = DateTime.Today.AddYears(-30);
            txtSdt.Text = sdtGoiY;
            cboGioiTinh.SelectedIndex = 0;
        }
        else
        {
            Text = $"Sửa khách hàng {kh.MaKh}";
            txtMa.Text = kh.MaKh;
            txtHoTen.Text = kh.HoTenKh;
            if (kh.NgaySinhKh is { } ns) dtpNgaySinh.Value = ns;
            cboGioiTinh.SelectedItem = kh.GioiTinhHienThi;
            txtSdt.Text = kh.SdtKh;
            txtDiaChi.Text = kh.DiaChiKh;
        }
    }

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        lblLoi.Text = "";
        var kh = new KhachHangDTO
        {
            MaKh = _kh?.MaKh ?? "",
            HoTenKh = txtHoTen.Text,
            NgaySinhKh = dtpNgaySinh.Value.Date,
            GioiTinhKh = cboGioiTinh.SelectedItem as string,
            SdtKh = txtSdt.Text,
            DiaChiKh = txtDiaChi.Text,
        };
        try
        {
            if (_kh is null) MaDaLuu = _bus.Them(kh);
            else { _bus.Sua(kh); MaDaLuu = kh.MaKh; }
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
