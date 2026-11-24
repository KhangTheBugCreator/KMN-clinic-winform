using System.Data;
using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Quản trị viên tra cứu lịch hẹn, hồ sơ khám hoặc hóa đơn (chỉ xem). Lọc theo ngày khám, trạng thái, từ khóa.
/// Hồ sơ và hóa đơn mở được chi tiết hồ sơ kèm đơn thuốc.
/// </summary>
public partial class ucTraCuu : UserControl
{
    private readonly TraCuuBUS _bus = new();
    private readonly LoaiTraCuu _loai;
    private bool _dangNap = true;

    public ucTraCuu() : this(LoaiTraCuu.HoSo) { } // cho Designer

    public ucTraCuu(LoaiTraCuu loai)
    {
        InitializeComponent();
        _loai = loai;
        GiaoDien.Luoi(dgvKetQua);
        dgvKetQua.AutoGenerateColumns = true; // cột lấy từ DataTable, khác nhau theo loại
        cboTrangThai.Items.Add("Mọi trạng thái");
        cboTrangThai.Items.AddRange(TraCuuBUS.CacTrangThai(loai));
        cboTrangThai.SelectedIndex = 0;
        dtpDen.Value = DateTime.Today;
        dtpTu.Value = DateTime.Today.AddDays(-30);
        if (loai == LoaiTraCuu.LichHen) dtpDen.Value = DateTime.Today.AddDays(30); // lịch hẹn có cả ngày tới
        btnXem.Visible = loai != LoaiTraCuu.LichHen;
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        _dangNap = false;
        NapLai();
    }

    private void NapLai()
    {
        if (_dangNap) return;
        dtpTu.Enabled = dtpDen.Enabled = !chkMoiNgay.Checked;
        DataTable bang = new();
        if (!ThongBaoLoi.Chay(this, () => bang = _bus.TimKiem(_loai,
                chkMoiNgay.Checked ? null : dtpTu.Value.Date, chkMoiNgay.Checked ? null : dtpDen.Value.Date,
                cboTrangThai.SelectedIndex > 0 ? cboTrangThai.Text : null, txtTim.Text))) return;
        dgvKetQua.DataSource = bang;
        lblDem.Text = $"{bang.Rows.Count} dòng" + (bang.Rows.Count >= 1000 ? " (chỉ hiện 1.000 dòng mới nhất, hãy thu hẹp bộ lọc)" : "") +
                      (bang.Columns.Contains("Tổng tiền") ? $" · tổng {GiaoDien.Tien(bang.AsEnumerable().Sum(r => r.Field<decimal>("Tổng tiền")))}" : "");
        btnXem.Enabled = bang.Rows.Count > 0;
    }

    private void BoLoc_Changed(object? sender, EventArgs e) => NapLai();

    private void dgvKetQua_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
    {
        foreach (DataGridViewColumn c in dgvKetQua.Columns)
        {
            if (c.ValueType == typeof(decimal))
            {
                c.DefaultCellStyle.Format = "#,0";
                c.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            else if (c.ValueType == typeof(DateTime))
                c.DefaultCellStyle.Format = c.HeaderText == "Ngày khám" && _loai == LoaiTraCuu.LichHen ? "dd/MM/yyyy" : "dd/MM/yyyy HH:mm";
        }
    }

    private void btnXem_Click(object? sender, EventArgs e)
    {
        if (_loai == LoaiTraCuu.LichHen || dgvKetQua.CurrentRow?.DataBoundItem is not DataRowView dong) return;
        var ma = (string)dong["Mã hồ sơ"];
        HoSoKhamDTO hs = new();
        List<ChiTietDonThuocDTO> thuoc = [];
        if (!ThongBaoLoi.Chay(this, () => (hs, thuoc) = _bus.ChiTietHoSo(ma))) return;
        var noiDung =
            $"Hồ sơ {hs.MaHoSo} · {hs.TrangThai}\r\n" +
            $"Khám lúc {hs.NgayGioKham:dd/MM/yyyy HH:mm} · {hs.Nguon}{(hs.MaLichHen is null ? "" : " " + hs.MaLichHen)} · Lễ tân: {hs.TenLeTan}\r\n\r\n" +
            $"Bệnh nhân: {hs.TenKhachHang} ({hs.MaKh}) · {hs.GioiTinh}, {hs.Tuoi} tuổi · {hs.SdtKhachHang}\r\n" +
            $"Bác sĩ: {hs.TenBacSi} ({hs.TenKhoa})\r\n\r\n" +
            $"Triệu chứng: {hs.TrieuChung}\r\nChẩn đoán: {hs.ChanDoanBenh}\r\nLời dặn: {hs.LoiDan}\r\n" +
            $"Tiền công khám: {GiaoDien.Tien(hs.TienCongKham)}\r\n\r\n" +
            (thuoc.Count == 0 ? "Không kê thuốc.\r\n" : "Đơn thuốc:\r\n" + string.Concat(thuoc.Select(t =>
                $"  - {t.TenThuoc} {t.HamLuong:0.##}: {t.SoLuongThuoc} {t.DonViTinh.ToLowerInvariant()} × {GiaoDien.Tien(t.DonGiaThuoc)} = {GiaoDien.Tien(t.ThanhTien)} ({t.LieuDung})\r\n"))) +
            (hs.MaHoaDon is null ? "\r\nChưa lập hóa đơn." : $"\r\nHóa đơn {hs.MaHoaDon}: {GiaoDien.Tien(hs.TongTien ?? 0)}");
        using var f = new Form
        {
            Text = $"Hồ sơ {hs.MaHoSo} – {hs.TenKhachHang}", StartPosition = FormStartPosition.CenterParent, ShowInTaskbar = false,
            MinimizeBox = false, ClientSize = new Size(720, 520), AutoScaleMode = AutoScaleMode.Dpi,
        };
        f.Controls.Add(new TextBox
        {
            Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, Text = noiDung,
            BackColor = SystemColors.Window, Font = new Font("Segoe UI", 10F),
        });
        f.ShowDialog(this);
    }

    private void dgvKetQua_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnXem_Click(sender, e);
    }
}
