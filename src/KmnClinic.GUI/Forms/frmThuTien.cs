using KmnClinic.BUS;
using System.Drawing.Printing;
using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Thu ngân xác nhận thu tiền: Chưa thanh toán → Đã thanh toán, ghi thu ngân, trigger trừ tồn kho (RB33) trong cùng transaction.
/// In hóa đơn A4 bằng PrintDocument (chọn "Microsoft Print to PDF" để lưu PDF). Module W7 thay dữ liệu giả bằng ThanhToanBUS.
/// </summary>
public partial class frmThuTien : Form
{
    private readonly HoaDonDTO _hd;
    private readonly List<ChiTietDonThuocDTO> _thuoc = Gia.DonThuocMau();

    public frmThuTien(HoaDonDTO hd)
    {
        InitializeComponent();
        _hd = hd;
        GiaoDien.Luoi(dgvChiTiet);

        // Dữ liệu giả: tổng tiền tính lại từ đơn thuốc mẫu cho khớp bảng chi tiết
        _hd.TongTien = _hd.TienCongKham + _thuoc.Sum(t => t.ThanhTien);
        Text = $"Thu tiền – {hd.MaHoaDon}";
        lblTieuDe.Text = $"Hóa đơn {hd.MaHoaDon}";
        lblThongTin.Text = $"Khách: {hd.TenKhachHang} · {hd.SdtKhachHang}\r\nBác sĩ: {hd.TenBacSi} · Hồ sơ {hd.MaHoSo} · Khám {hd.NgayKham:dd/MM/yyyy HH:mm}";
        dgvChiTiet.Rows.Add("Tiền công khám", 1, GiaoDien.Tien(hd.TienCongKham), GiaoDien.Tien(hd.TienCongKham));
        foreach (var t in _thuoc)
            dgvChiTiet.Rows.Add($"{t.TenThuoc} ({t.DonViTinh.ToLower()})", t.SoLuongThuoc, GiaoDien.Tien(t.DonGiaThuoc), GiaoDien.Tien(t.ThanhTien));
        lblTong.Text = $"Tổng cộng: {GiaoDien.Tien(_hd.TongTien)}";
        numKhachDua.Value = Math.Ceiling(_hd.TongTien / 50000) * 50000;
    }

    private void numKhachDua_ValueChanged(object? sender, EventArgs e)
    {
        var con = numKhachDua.Value - _hd.TongTien;
        lblTraLai.ForeColor = con >= 0 ? Color.DarkGreen : Color.Red;
        lblTraLai.Text = con >= 0 ? $"Trả lại khách: {GiaoDien.Tien(con)}" : $"Còn thiếu: {GiaoDien.Tien(-con)}";
    }

    private void btnXacNhan_Click(object? sender, EventArgs e)
    {
        if (numKhachDua.Value < _hd.TongTien) { MessageBox.Show(this, "Số tiền khách đưa chưa đủ.", Text); return; }
        _hd.TrangThaiThanhToan = TrangThai.DaThanhToan;
        _hd.NgayGioThanhToan = DateTime.Now;
        _hd.ThuNgan = PhienDangNhap.NhanVien?.HoTenNv;
        if (MessageBox.Show(this, "Đã thu tiền. In hóa đơn cho khách?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            btnXemIn_Click(sender, e);
        DialogResult = DialogResult.OK;
    }

    private void btnXemIn_Click(object? sender, EventArgs e)
    {
        using var doc = new PrintDocument { DocumentName = $"Hoa don {_hd.MaHoaDon}" };
        doc.PrintPage += VeHoaDon;
        using var xem = new PrintPreviewDialog { Document = doc, Width = 900, Height = 1000, StartPosition = FormStartPosition.CenterParent };
        xem.ShowDialog(this);
    }

    /// <summary>Vẽ hóa đơn trên giấy A4 (đơn vị 1/100 inch).</summary>
    private void VeHoaDon(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        var trai = e.MarginBounds.Left;
        var phai = e.MarginBounds.Right;
        float y = e.MarginBounds.Top;
        using var fTieuDe = new Font("Segoe UI", 18, FontStyle.Bold);
        using var fDam = new Font("Segoe UI", 10, FontStyle.Bold);
        using var fThuong = new Font("Segoe UI", 10);
        var navy = Brushes.Black;
        var xam = Brushes.DimGray;
        var vien = Pens.Gray;
        var phaiCanh = new StringFormat { Alignment = StringAlignment.Far };

        if (GiaoDien.Logo() is { } logo) { g.DrawImage(logo, trai, y, 180, 60); }
        g.DrawString("KMN Clinic – Phòng khám Đa khoa\n126 Nguyễn Thiện Thành, Vĩnh Long", fThuong, xam, phai, y + 8, phaiCanh);
        y += 90;
        g.DrawString($"HÓA ĐƠN THANH TOÁN {_hd.MaHoaDon}", fTieuDe, navy, trai, y);
        y += 44;
        g.DrawString($"Khách hàng: {_hd.TenKhachHang}    SĐT: {_hd.SdtKhachHang}", fThuong, Brushes.Black, trai, y); y += 22;
        g.DrawString($"Bác sĩ: {_hd.TenBacSi}    Hồ sơ: {_hd.MaHoSo}    Ngày khám: {_hd.NgayKham:dd/MM/yyyy HH:mm}", fThuong, Brushes.Black, trai, y); y += 34;

        float[] cot = [trai, trai + 360, trai + 430, trai + 560];
        string[] dau = ["Nội dung", "SL", "Đơn giá", "Thành tiền"];
        for (int i = 0; i < 4; i++) g.DrawString(dau[i], fDam, navy, cot[i], y);
        y += 24; g.DrawLine(vien, trai, y, phai, y); y += 6;
        void Dong(string nd, int sl, decimal gia)
        {
            g.DrawString(nd, fThuong, Brushes.Black, cot[0], y);
            g.DrawString(sl.ToString(), fThuong, Brushes.Black, cot[1], y);
            g.DrawString(GiaoDien.Tien(gia), fThuong, Brushes.Black, cot[2], y);
            g.DrawString(GiaoDien.Tien(sl * gia), fThuong, Brushes.Black, cot[3], y);
            y += 24;
        }
        Dong("Tiền công khám", 1, _hd.TienCongKham);
        foreach (var t in _thuoc) Dong($"{t.TenThuoc} ({t.DonViTinh.ToLower()})", t.SoLuongThuoc, t.DonGiaThuoc);
        g.DrawLine(vien, trai, y, phai, y); y += 10;
        g.DrawString($"TỔNG CỘNG: {GiaoDien.Tien(_hd.TongTien)}", new Font("Segoe UI", 13, FontStyle.Bold), navy, phai, y, phaiCanh);
        y += 40;
        var trangThai = _hd.TrangThaiThanhToan == TrangThai.DaThanhToan
            ? $"Đã thanh toán lúc {_hd.NgayGioThanhToan:HH:mm dd/MM/yyyy} · Thu ngân: {_hd.ThuNgan}"
            : "Chưa thanh toán";
        g.DrawString(trangThai, fThuong, xam, trai, y);
    }
}
