using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Thêm / sửa thuốc. Quyết định Q8 của bản web: sửa tồn trực tiếp phải tick ô xác nhận; không có chức năng xóa thuốc.
/// </summary>
public partial class frmThuocChiTiet : Form
{
    private readonly ThuocDTO? _thuoc;

    public frmThuocChiTiet(ThuocDTO? thuoc)
    {
        InitializeComponent();
        _thuoc = thuoc;
        GiaoDien.Nut(btnLuu);
        GiaoDien.Nut(btnHuy, GiaoDien.KieuNut.Phu);
        if (thuoc is null)
        {
            Text = "Thêm thuốc";
            txtMa.Text = $"TH{Gia.Thuoc.Count + 18:0000} (tự sinh)";
            cboDvt.SelectedIndex = 0;
            chkXacNhanTon.Visible = false;
        }
        else
        {
            Text = $"Sửa thuốc {thuoc.MaThuoc}";
            txtMa.Text = thuoc.MaThuoc;
            txtTen.Text = thuoc.TenThuoc;
            txtNsx.Text = thuoc.Nsx;
            numHamLuong.Value = thuoc.HamLuong ?? 0;
            cboDvt.Text = thuoc.DonViTinh;
            numDonGia.Value = thuoc.DonGiaThuoc;
            numTon.Value = thuoc.SoLuongTon;
            numTon.Enabled = false;
            txtMoTa.Text = thuoc.MoTa;
        }
    }

    private void chkXacNhanTon_CheckedChanged(object? sender, EventArgs e) => numTon.Enabled = chkXacNhanTon.Checked;

    private void btnLuu_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTen.Text)) { MessageBox.Show(this, "Tên thuốc không được để trống.", Text); txtTen.Focus(); return; }
        if (string.IsNullOrWhiteSpace(cboDvt.Text)) { MessageBox.Show(this, "Hãy chọn đơn vị tính.", Text); return; }
        if (numDonGia.Value <= 0) { MessageBox.Show(this, "Đơn giá phải lớn hơn 0.", Text); return; }

        // Giai đoạn 2: chỉ sửa trong bộ nhớ để xem luồng thao tác. W1 sẽ gọi ThuocBUS.Them / ThuocBUS.Sua.
        var t = _thuoc ?? new ThuocDTO { MaThuoc = $"TH{Gia.Thuoc.Count + 18:0000}" };
        t.TenThuoc = txtTen.Text.Trim();
        t.Nsx = txtNsx.Text.Trim();
        t.HamLuong = numHamLuong.Value;
        t.DonViTinh = cboDvt.Text;
        t.DonGiaThuoc = numDonGia.Value;
        t.SoLuongTon = (int)numTon.Value;
        t.MoTa = txtMoTa.Text.Trim();
        if (_thuoc is null) Gia.Thuoc.Add(t);
        DialogResult = DialogResult.OK;
    }
}
