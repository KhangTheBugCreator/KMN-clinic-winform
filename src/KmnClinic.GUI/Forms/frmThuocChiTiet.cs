using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Thêm / sửa thuốc. Quyết định Q8 của bản web: sửa tồn trực tiếp phải tick ô xác nhận
/// (không tick thì số tồn trong CSDL giữ nguyên); không có chức năng xóa thuốc.
/// </summary>
public partial class frmThuocChiTiet : Form
{
    private readonly ThuocBUS _bus = new();
    private readonly ThuocDTO? _thuoc;

    /// <summary>Mã thuốc vừa thêm/sửa, để danh sách chọn lại đúng dòng.</summary>
    public string? MaDaLuu { get; private set; }

    public frmThuocChiTiet(ThuocDTO? thuoc)
    {
        InitializeComponent();
        _thuoc = thuoc;
        if (thuoc is null)
        {
            Text = "Thêm thuốc";
            txtMa.Text = "(tự sinh)";
            cboDvt.SelectedIndex = 0;
            chkXacNhanTon.Visible = false;
        }
        else
        {
            Text = $"Sửa thuốc {thuoc.MaThuoc}";
            txtMa.Text = thuoc.MaThuoc;
            txtTen.Text = thuoc.TenThuoc;
            txtNsx.Text = thuoc.Nsx;
            numHamLuong.Value = Math.Min(numHamLuong.Maximum, thuoc.HamLuong ?? 0);
            cboDvt.Text = thuoc.DonViTinh;
            numDonGia.Value = Math.Min(numDonGia.Maximum, thuoc.DonGiaThuoc);
            numTon.Value = Math.Min(numTon.Maximum, thuoc.SoLuongTon);
            numTon.Enabled = false;
            txtMoTa.Text = thuoc.MoTa;
        }
    }

    private void chkXacNhanTon_CheckedChanged(object? sender, EventArgs e) => numTon.Enabled = chkXacNhanTon.Checked;

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        var t = new ThuocDTO
        {
            MaThuoc = _thuoc?.MaThuoc ?? "",
            TenThuoc = txtTen.Text,
            Nsx = txtNsx.Text,
            HamLuong = numHamLuong.Value,
            DonViTinh = cboDvt.Text,
            DonGiaThuoc = numDonGia.Value,
            SoLuongTon = (int)numTon.Value,
            MoTa = txtMoTa.Text,
        };
        var ok = ThongBaoLoi.Chay(this, () =>
        {
            if (_thuoc is null) MaDaLuu = _bus.Them(t);
            else { _bus.Sua(t, chkXacNhanTon.Checked); MaDaLuu = t.MaThuoc; }
        });
        if (ok) DialogResult = DialogResult.OK;
    }
}
