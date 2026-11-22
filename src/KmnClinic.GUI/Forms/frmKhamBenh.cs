using System.ComponentModel;
using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Bác sĩ khám: chẩn đoán, lời dặn, tiền công, kê đơn (không vượt tồn RB27, lưu đơn giá lúc kê RB28).
/// "Hoàn tất khám" tạo hóa đơn "Chưa thanh toán" và khóa hồ sơ (Q4). Module W6 thay dữ liệu giả bằng KhamBenhBUS.
/// </summary>
public partial class frmKhamBenh : Form
{
    private readonly HoSoKhamDTO _hs;
    private readonly BindingList<ChiTietDonThuocDTO> _don;
    private readonly bool _chiXem;

    public frmKhamBenh(HoSoKhamDTO hs)
    {
        InitializeComponent();
        _hs = hs;
        _chiXem = hs.TrangThai == TrangThai.HoanTat;
        GiaoDien.Luoi(dgvLanTruoc);
        GiaoDien.Luoi(dgvDonThuoc);

        Text = $"Khám bệnh – {hs.MaHoSo} – {hs.TenKhachHang}";
        lblTen.Text = hs.TenKhachHang;
        lblThongTin.Text = $"{hs.GioiTinh}, {hs.Tuoi} tuổi · Mã hồ sơ {hs.MaHoSo}\r\n{hs.Nguon}{(hs.MaLichHen is null ? "" : " · " + hs.MaLichHen)} · Tiếp nhận {hs.NgayGioKham:HH:mm}";
        lblTrieuChung.Text = string.IsNullOrWhiteSpace(hs.TrieuChung) ? "(không ghi)" : hs.TrieuChung;
        dgvLanTruoc.Rows.Add(Gia.HomNay.AddDays(-42).ToString("dd/MM/yyyy"), "Viêm họng cấp", "Lâm Thị Kiều Oanh");
        dgvLanTruoc.Rows.Add(Gia.HomNay.AddDays(-160).ToString("dd/MM/yyyy"), "Rối loạn tiêu hóa", "Nguyễn Văn Hùng");

        cboThuoc.DisplayMember = "TenThuoc";
        cboThuoc.DataSource = Gia.Thuoc.Where(t => t.SoLuongTon > 0).ToList();
        _don = new BindingList<ChiTietDonThuocDTO>(hs.TrangThai == TrangThai.ChoKham ? [] : Gia.DonThuocMau());
        dgvDonThuoc.DataSource = _don;
        _don.ListChanged += (_, _) => CapNhatTong();
        numTienCong.Value = hs.TienCongKham > 0 ? hs.TienCongKham : 150000;
        txtChanDoan.Text = hs.ChanDoanBenh;
        txtLoiDan.Text = hs.LoiDan;
        CapNhatTong();

        if (_chiXem)
        {
            foreach (var c in new Control[] { txtChanDoan, txtLoiDan }) ((TextBox)c).ReadOnly = true;
            numTienCong.Enabled = flpKeThuoc.Enabled = btnLuuNhap.Enabled = btnHoanTat.Enabled = false;
            colXoa.Visible = false;
            btnHoanTat.Text = "Đã hoàn tất";
        }
    }

    private void CapNhatTong()
    {
        var tienThuoc = _don.Sum(x => x.ThanhTien);
        lblTong.Text = $"Tiền thuốc {GiaoDien.Tien(tienThuoc)}  ·  Tổng {GiaoDien.Tien(tienThuoc + numTienCong.Value)}";
    }

    private void numTienCong_ValueChanged(object? sender, EventArgs e) { if (_don is not null) CapNhatTong(); }

    private void btnThemThuoc_Click(object? sender, EventArgs e)
    {
        if (cboThuoc.SelectedItem is not ThuocDTO t) return;
        var sl = (int)numSoLuong.Value + _don.Where(x => x.MaThuoc == t.MaThuoc).Sum(x => x.SoLuongThuoc);
        if (sl > t.SoLuongTon)
        {
            MessageBox.Show(this, $"{t.TenThuoc} chỉ còn {t.SoLuongTon} {t.DonViTinh.ToLower()} trong kho (RB27).", "Không đủ tồn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        var co = _don.FirstOrDefault(x => x.MaThuoc == t.MaThuoc);
        if (co is not null) { co.SoLuongThuoc = sl; co.LieuDung = txtLieuDung.Text.Trim(); _don.ResetBindings(); }
        else _don.Add(new ChiTietDonThuocDTO { MaThuoc = t.MaThuoc, TenThuoc = t.TenThuoc, DonViTinh = t.DonViTinh, SoLuongThuoc = sl, LieuDung = txtLieuDung.Text.Trim(), DonGiaThuoc = t.DonGiaThuoc });
        txtLieuDung.Clear();
    }

    private void dgvDonThuoc_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (_chiXem || e.RowIndex < 0 || dgvDonThuoc.Columns[e.ColumnIndex].Name != "colXoa") return;
        _don.RemoveAt(e.RowIndex);
    }

    private void Ghi()
    {
        _hs.ChanDoanBenh = txtChanDoan.Text.Trim();
        _hs.LoiDan = txtLoiDan.Text.Trim();
        _hs.TienCongKham = numTienCong.Value;
    }

    private void btnLuuNhap_Click(object? sender, EventArgs e)
    {
        Ghi();
        _hs.TrangThai = TrangThai.DangKham;
        MessageBox.Show(this, "Đã lưu nháp. Hồ sơ vẫn ở trạng thái Đang khám.", Text);
    }

    private void btnHoanTat_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtChanDoan.Text)) { MessageBox.Show(this, "Hãy nhập chẩn đoán trước khi hoàn tất.", Text); txtChanDoan.Focus(); return; }
        var tong = _don.Sum(x => x.ThanhTien) + numTienCong.Value;
        if (MessageBox.Show(this, $"Hoàn tất khám và chuyển hóa đơn {GiaoDien.Tien(tong)} cho thu ngân?\r\nSau khi hoàn tất, hồ sơ và đơn thuốc không sửa được nữa.",
                "Hoàn tất khám", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        Ghi();
        _hs.TrangThai = TrangThai.HoanTat;
        DialogResult = DialogResult.OK;
    }
}
