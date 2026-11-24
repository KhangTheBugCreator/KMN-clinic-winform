using System.Drawing.Printing;
using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Thu ngân xác nhận thu tiền: Chưa thanh toán → Đã thanh toán, ghi giờ thu và thu ngân (RB31, RB32), trigger trừ tồn kho (RB33)
/// trong cùng transaction. Hóa đơn đã thu mở ở chế độ chỉ xem để in lại. In A4 bằng PrintDocument
/// (chọn máy in "Microsoft Print to PDF" để lưu PDF). Ô "Khách đưa" chỉ để tính tiền trả lại, không lưu.
/// </summary>
public partial class frmThuTien : Form
{
    private readonly ThanhToanBUS _bus = new();
    private readonly string _ma;
    private HoaDonDTO _hd = new();

    public frmThuTien(string maHoaDon)
    {
        InitializeComponent();
        _ma = maHoaDon;
        GiaoDien.Luoi(dgvChiTiet);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        if (!ThongBaoLoi.Chay(this, () => _hd = _bus.ChiTiet(_ma)))
        {
            BeginInvoke(Close);
            return;
        }
        HienHoaDon();
        if (!_hd.DaThanhToan) numKhachDua.Value = Math.Min(numKhachDua.Maximum, Math.Ceiling(_hd.TongTien / 50000) * 50000);
    }

    private void HienHoaDon()
    {
        Text = $"{(_hd.DaThanhToan ? "Hóa đơn" : "Thu tiền")} – {_hd.MaHoaDon}";
        lblTieuDe.Text = $"Hóa đơn {_hd.MaHoaDon} · {_hd.TrangThaiThanhToan}" +
                         (_hd.DaThanhToan ? $" lúc {_hd.NgayGioThanhToan:HH:mm dd/MM/yyyy} ({_hd.ThuNgan})" : "");
        lblThongTin.Text = $"Khách: {_hd.TenKhachHang} · {_hd.SdtKhachHang} · Chẩn đoán: {_hd.ChanDoanBenh}\r\n" +
                           $"Bác sĩ: {_hd.TenBacSi} ({_hd.TenKhoa}) · Hồ sơ {_hd.MaHoSo} · Khám {_hd.NgayKham:dd/MM/yyyy HH:mm}";
        dgvChiTiet.Rows.Clear();
        dgvChiTiet.Rows.Add("Tiền công khám", 1, GiaoDien.Tien(_hd.TienCongKham), GiaoDien.Tien(_hd.TienCongKham));
        foreach (var t in _hd.Thuoc)
            dgvChiTiet.Rows.Add(TenDong(t), t.SoLuongThuoc, GiaoDien.Tien(t.DonGiaThuoc), GiaoDien.Tien(t.ThanhTien));
        lblTong.Text = $"Tổng cộng: {GiaoDien.Tien(_hd.TongTien)}";

        var daThu = _hd.DaThanhToan;
        lblKhachDua.Visible = numKhachDua.Visible = lblTraLai.Visible = btnXacNhan.Visible = !daThu;
        btnXemIn.Text = daThu ? "In lại hóa đơn" : "Xem trước bản in";
        btnDong.Text = "Đóng";
    }

    private static string TenDong(ChiTietDonThuocDTO t) => $"{t.TenThuoc} {t.HamLuong:0.##} ({t.DonViTinh.ToLowerInvariant()})";

    private void numKhachDua_ValueChanged(object? sender, EventArgs e)
    {
        var con = numKhachDua.Value - _hd.TongTien;
        lblTraLai.ForeColor = con >= 0 ? Color.DarkGreen : Color.Red;
        lblTraLai.Text = con >= 0 ? $"Trả lại khách: {GiaoDien.Tien(con)}" : $"Còn thiếu: {GiaoDien.Tien(-con)}";
    }

    private void btnXacNhan_Click(object? sender, EventArgs e)
    {
        if (numKhachDua.Value < _hd.TongTien)
        {
            MessageBox.Show(this, "Số tiền khách đưa chưa đủ.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            numKhachDua.Focus();
            return;
        }
        if (MessageBox.Show(this, $"Xác nhận đã thu {GiaoDien.Tien(_hd.TongTien)} của {_hd.TenKhachHang}?\r\nSau khi xác nhận, kho sẽ trừ thuốc và không hoàn tác được.",
                "Xác nhận thu tiền", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        if (!ThongBaoLoi.Chay(this, () => _hd = _bus.ThanhToan(_ma)))
        {
            ThongBaoLoi.Chay(this, () => _hd = _bus.ChiTiet(_ma)); // có thể người khác vừa thu: hiện trạng thái mới
            HienHoaDon();
            return;
        }
        HienHoaDon();
        if (MessageBox.Show(this, "Đã thu tiền. In hóa đơn cho khách?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            btnXemIn_Click(sender, e);
    }

    private void btnXemIn_Click(object? sender, EventArgs e)
    {
        using var doc = new PrintDocument { DocumentName = $"Hoa don {_hd.MaHoaDon}" };
        doc.DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
        doc.BeginPrint += (_, _) => _dongDaIn = 0; // xem trước rồi in: mỗi lần bắt đầu lại từ trang đầu
        doc.PrintPage += VeHoaDon;
        using var xem = new PrintPreviewDialog { Document = doc, Width = 900, Height = 1000, StartPosition = FormStartPosition.CenterParent };
        xem.ShowDialog(this);
    }

    /// <summary>Số dòng thuốc đã vẽ ở các trang trước.</summary>
    private int _dongDaIn;

    /// <summary>Vẽ hóa đơn trên giấy A4 (đơn vị 1/100 inch). Đơn thuốc dài thì sang trang.</summary>
    private void VeHoaDon(object? sender, PrintPageEventArgs e)
    {
        var g = e.Graphics!;
        var trai = e.MarginBounds.Left;
        var phai = e.MarginBounds.Right;
        var day = e.MarginBounds.Bottom;
        float y = e.MarginBounds.Top;
        using var fTieuDe = new Font("Segoe UI", 16, FontStyle.Bold);
        using var fDam = new Font("Segoe UI", 10, FontStyle.Bold);
        using var fThuong = new Font("Segoe UI", 10);
        using var fNho = new Font("Segoe UI", 8.5f);
        using var fTong = new Font("Segoe UI", 13, FontStyle.Bold);
        var den = Brushes.Black;
        var xam = Brushes.DimGray;
        using var phaiCanh = new StringFormat { Alignment = StringAlignment.Far };
        float[] cot = [trai, trai + 380, trai + 440, trai + 560];

        var dauTrang = _dongDaIn == 0;
        if (dauTrang)
        {
            if (GiaoDien.Logo() is { } logo) g.DrawImage(logo, trai, y, 180, 60);
            g.DrawString("KMN Clinic – Phòng khám Đa khoa\n126 Nguyễn Thiện Thành, Vĩnh Long", fThuong, xam, phai, y + 8, phaiCanh);
            y += 84;
            g.DrawString($"HÓA ĐƠN THANH TOÁN {_hd.MaHoaDon}", fTieuDe, den, trai, y); y += 40;
            g.DrawString($"Khách hàng: {_hd.TenKhachHang} ({_hd.MaKh})    SĐT: {_hd.SdtKhachHang}", fThuong, den, trai, y); y += 20;
            if (!string.IsNullOrEmpty(_hd.DiaChi)) { g.DrawString($"Địa chỉ: {_hd.DiaChi}", fThuong, den, trai, y); y += 20; }
            g.DrawString($"Bác sĩ: {_hd.TenBacSi} ({_hd.TenKhoa})    Hồ sơ: {_hd.MaHoSo}    Ngày khám: {_hd.NgayKham:dd/MM/yyyy HH:mm}", fThuong, den, trai, y); y += 20;
            g.DrawString($"Chẩn đoán: {_hd.ChanDoanBenh}", fThuong, den, new RectangleF(trai, y, phai - trai, 40)); y += 22;
            if (!string.IsNullOrEmpty(_hd.LoiDan)) { g.DrawString($"Lời dặn: {_hd.LoiDan}", fThuong, den, new RectangleF(trai, y, phai - trai, 40)); y += 22; }
            y += 12;
        }

        string[] dau = ["Nội dung", "SL", "Đơn giá", "Thành tiền"];
        for (int i = 0; i < 4; i++) g.DrawString(dau[i], fDam, den, cot[i], y);
        y += 22; g.DrawLine(Pens.Gray, trai, y, phai, y); y += 6;

        void Dong(string nd, string? phu, int sl, decimal gia)
        {
            g.DrawString(nd, fThuong, den, cot[0], y);
            g.DrawString(sl.ToString(), fThuong, den, cot[1], y);
            g.DrawString(GiaoDien.Tien(gia), fThuong, den, cot[2], y);
            g.DrawString(GiaoDien.Tien(sl * gia), fThuong, den, cot[3], y);
            y += 20;
            if (!string.IsNullOrEmpty(phu)) { g.DrawString(phu, fNho, xam, cot[0] + 12, y); y += 18; }
            y += 4;
        }
        if (dauTrang) Dong("Tiền công khám", null, 1, _hd.TienCongKham);
        while (_dongDaIn < _hd.Thuoc.Count)
        {
            if (y > day - 140) { e.HasMorePages = true; return; } // chừa chỗ cho phần tổng
            var t = _hd.Thuoc[_dongDaIn++];
            Dong(TenDong(t), t.LieuDung, t.SoLuongThuoc, t.DonGiaThuoc);
        }
        g.DrawLine(Pens.Gray, trai, y, phai, y); y += 10;
        g.DrawString($"Tiền khám: {GiaoDien.Tien(_hd.TienCongKham)}    Tiền thuốc: {GiaoDien.Tien(_hd.TienThuoc)}", fThuong, den, phai, y, phaiCanh); y += 24;
        g.DrawString($"TỔNG CỘNG: {GiaoDien.Tien(_hd.TongTien)}", fTong, den, phai, y, phaiCanh); y += 40;
        g.DrawString(_hd.DaThanhToan
            ? $"Đã thanh toán lúc {_hd.NgayGioThanhToan:HH:mm dd/MM/yyyy} · Thu ngân: {_hd.ThuNgan}"
            : "Chưa thanh toán (bản xem trước)", fThuong, xam, trai, y);
        e.HasMorePages = false;
    }
}
