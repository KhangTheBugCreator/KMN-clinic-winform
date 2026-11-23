using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Bác sĩ khám một hồ sơ: triệu chứng, chẩn đoán, lời dặn, tiền công; kê đơn (không vượt tồn RB27, đơn giá chốt lúc kê RB28);
/// xem các lần khám trước của bệnh nhân. Mỗi lần thêm, sửa, xóa thuốc ghi thẳng vào CSDL. "Hoàn tất khám" lưu thông tin,
/// lập hóa đơn "Chưa thanh toán" chuyển thu ngân và khóa hồ sơ (Q4, RB22). Hồ sơ đã hoàn tất mở ở chế độ chỉ xem.
/// </summary>
public partial class frmKhamBenh : Form
{
    private readonly KhamBenhBUS _bus = new();
    private readonly string _maHoSo;
    private HoSoKhamDTO _hs = new();
    private List<ChiTietDonThuocDTO> _don = [];
    private bool _chiXem, _dangNap = true, _chuaLuu;

    /// <summary>Hồ sơ đã hoàn tất trong lần mở này (để màn gọi nạp lại).</summary>
    public bool DaHoanTat { get; private set; }

    public frmKhamBenh(string maHoSo)
    {
        InitializeComponent();
        _maHoSo = maHoSo;
        GiaoDien.Luoi(dgvLanTruoc);
        GiaoDien.Luoi(dgvDonThuoc);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        List<LanKhamTruocDTO> lanTruoc = [];
        List<ThuocDTO> thuoc = [];
        if (!ThongBaoLoi.Chay(this, () =>
            {
                _hs = _bus.ChiTiet(_maHoSo);
                _don = _bus.DonThuoc(_maHoSo);
                lanTruoc = _bus.LanKhamTruoc(_maHoSo);
                thuoc = new ThuocBUS().DanhSach();
            }))
        {
            BeginInvoke(Close);
            return;
        }
        _chiXem = _hs.DaHoanTat;
        Text = $"{(_chiXem ? "Xem hồ sơ" : "Khám bệnh")} – {_hs.MaHoSo} – {_hs.TenKhachHang}";
        lblTen.Text = _hs.TenKhachHang;
        lblThongTin.Text = $"{_hs.GioiTinh}, {_hs.Tuoi} tuổi · {_hs.MaKh} · {_hs.SdtKhachHang}\r\n" +
                           $"Hồ sơ {_hs.MaHoSo} · {_hs.Nguon}{(_hs.MaLichHen is null ? "" : " " + _hs.MaLichHen)}\r\n" +
                           $"Tiếp nhận {_hs.NgayGioKham:dd/MM/yyyy HH:mm} · {_hs.TrangThai}";
        txtTrieuChung.Text = _hs.TrieuChung;
        txtChanDoan.Text = _hs.ChanDoanBenh;
        txtLoiDan.Text = _hs.LoiDan;
        numTienCong.Value = _hs.TienCongKham;

        dgvLanTruoc.DataSource = lanTruoc;
        lblLanTruoc.Text = $"Các lần khám trước ({lanTruoc.Count})";
        dgvLanTruoc_SelectionChanged(null, EventArgs.Empty);

        // chỉ cho chọn thuốc còn hàng; thuốc đã trong đơn vẫn hiện để sửa
        cboThuoc.DataSource = thuoc.Where(t => t.SoLuongTon > 0 || _don.Any(d => d.MaThuoc == t.MaThuoc)).ToList();
        cboThuoc.SelectedIndex = -1;
        HienDon();

        if (_chiXem)
        {
            txtTrieuChung.ReadOnly = txtChanDoan.ReadOnly = txtLoiDan.ReadOnly = true;
            numTienCong.Enabled = flpKeThuoc.Enabled = btnLuuNhap.Enabled = btnHoanTat.Enabled = false;
            colXoa.Visible = false;
            btnHoanTat.Text = "Đã hoàn tất";
            lblKeThuoc.Text = "Đơn thuốc";
        }
        _dangNap = false;
        _chuaLuu = false;
        ActiveControl = _chiXem ? dgvDonThuoc : txtChanDoan;
    }

    private void HienDon()
    {
        dgvDonThuoc.DataSource = null;
        dgvDonThuoc.DataSource = _don;
        CapNhatTong();
        cboThuoc_SelectedIndexChanged(null, EventArgs.Empty);
    }

    private void NapDon() => ThongBaoLoi.Chay(this, () => _don = _bus.DonThuoc(_maHoSo));

    private void CapNhatTong()
    {
        var tienThuoc = _don.Sum(x => x.ThanhTien);
        lblTong.Text = $"Tiền thuốc {GiaoDien.Tien(tienThuoc)}  ·  Tổng {GiaoDien.Tien(tienThuoc + numTienCong.Value)}";
    }

    private void ODuLieu_Changed(object? sender, EventArgs e)
    {
        if (_dangNap) return;
        _chuaLuu = true;
        CapNhatTong();
    }

    private void numTienCong_ValueChanged(object? sender, EventArgs e) => ODuLieu_Changed(sender, e);

    private void dgvLanTruoc_SelectionChanged(object? sender, EventArgs e)
    {
        if (dgvLanTruoc.CurrentRow?.DataBoundItem is not LanKhamTruocDTO l)
        {
            lblLanTruocChiTiet.Text = dgvLanTruoc.RowCount == 0 ? "Bệnh nhân chưa có lần khám nào trước đây." : "";
            return;
        }
        lblLanTruocChiTiet.Text =
            $"{l.NgayGioKham:dd/MM/yyyy HH:mm} · {l.TenBacSi} ({l.TenKhoa})\r\n" +
            $"Triệu chứng: {l.TrieuChung}\r\n" +
            $"Chẩn đoán: {l.ChanDoanBenh}\r\n" +
            (string.IsNullOrEmpty(l.LoiDan) ? "" : $"Lời dặn: {l.LoiDan}\r\n") +
            (l.Thuoc.Count == 0 ? "Không kê thuốc." : "Thuốc: " + string.Join("; ", l.Thuoc));
    }

    /* ---------- Kê đơn ---------- */

    private void cboThuoc_Format(object? sender, ListControlConvertEventArgs e)
    {
        if (e.ListItem is ThuocDTO t)
            e.Value = $"{t.TenThuoc} {t.HamLuong:0.##} – còn {t.SoLuongTon} {t.DonViTinh.ToLowerInvariant()}";
    }

    private ChiTietDonThuocDTO? TrongDon(ThuocDTO? t) => t is null ? null : _don.FirstOrDefault(d => d.MaThuoc == t.MaThuoc);

    private void cboThuoc_SelectedIndexChanged(object? sender, EventArgs e)
        => btnThemThuoc.Text = TrongDon(cboThuoc.SelectedItem as ThuocDTO) is null ? "Thêm" : "Cập nhật";

    private void btnThemThuoc_Click(object? sender, EventArgs e)
    {
        if (cboThuoc.SelectedItem is not ThuocDTO t)
        {
            MessageBox.Show(this, "Vui lòng chọn thuốc.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            cboThuoc.Focus();
            return;
        }
        var co = TrongDon(t);
        var sl = (int)numSoLuong.Value;
        if (co is not null && MessageBox.Show(this,
                $"{t.TenThuoc} đã có trong đơn ({co.SoLuongThuoc} {co.DonViTinh.ToLowerInvariant()}, {co.LieuDung}).\r\n" +
                $"Sửa thành {sl} {t.DonViTinh.ToLowerInvariant()}, {txtLieuDung.Text.Trim()}?",
                "Sửa thuốc trong đơn", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        var ok = ThongBaoLoi.Chay(this, () =>
        {
            if (co is null) _bus.KeThuoc(_maHoSo, t.MaThuoc, sl, txtLieuDung.Text);
            else _bus.SuaThuoc(_maHoSo, t.MaThuoc, sl, txtLieuDung.Text);
        });
        NapDon();
        HienDon();
        if (!ok) return;
        GiaoDien.ChonDong(dgvDonThuoc, o => ((ChiTietDonThuocDTO)o).MaThuoc == t.MaThuoc);
        cboThuoc.SelectedIndex = -1;
        numSoLuong.Value = 1;
        txtLieuDung.Clear();
        cboThuoc.Focus();
    }

    /// <summary>Nhấp đúp một dòng đơn: đưa lên ô nhập để sửa.</summary>
    private void dgvDonThuoc_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (_chiXem || e.RowIndex < 0 || dgvDonThuoc.Rows[e.RowIndex].DataBoundItem is not ChiTietDonThuocDTO d) return;
        cboThuoc.SelectedItem = ((List<ThuocDTO>)cboThuoc.DataSource!).FirstOrDefault(t => t.MaThuoc == d.MaThuoc);
        numSoLuong.Value = Math.Clamp(d.SoLuongThuoc, (int)numSoLuong.Minimum, (int)numSoLuong.Maximum);
        txtLieuDung.Text = d.LieuDung;
        numSoLuong.Focus();
        numSoLuong.Select(0, numSoLuong.Text.Length);
    }

    private void dgvDonThuoc_CellContentClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (_chiXem || e.RowIndex < 0 || dgvDonThuoc.Columns[e.ColumnIndex].Name != "colXoa") return;
        if (dgvDonThuoc.Rows[e.RowIndex].DataBoundItem is not ChiTietDonThuocDTO d) return;
        if (MessageBox.Show(this, $"Xóa {d.TenThuoc} khỏi đơn?", "Xóa thuốc", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        ThongBaoLoi.Chay(this, () => _bus.XoaThuoc(_maHoSo, d.MaThuoc));
        NapDon();
        HienDon();
    }

    /* ---------- Lưu, hoàn tất ---------- */

    private bool LuuNhap() => ThongBaoLoi.Chay(this, () =>
    {
        _bus.Luu(_maHoSo, txtTrieuChung.Text, txtChanDoan.Text, txtLoiDan.Text, numTienCong.Value);
        _chuaLuu = false;
    });

    private void btnLuuNhap_Click(object? sender, EventArgs e)
    {
        if (LuuNhap()) MessageBox.Show(this, "Đã lưu. Hồ sơ vẫn đang khám, có thể sửa tiếp.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void btnHoanTat_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtChanDoan.Text))
        {
            MessageBox.Show(this, "Vui lòng nhập chẩn đoán trước khi hoàn tất khám (RB22).", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtChanDoan.Focus();
            return;
        }
        var tong = _don.Sum(x => x.ThanhTien) + numTienCong.Value;
        if (MessageBox.Show(this, $"Hoàn tất khám và chuyển hóa đơn {GiaoDien.Tien(tong)} cho thu ngân?\r\n" +
                "Sau khi hoàn tất, hồ sơ và đơn thuốc không sửa được nữa.",
                "Hoàn tất khám", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        string maHd = "";
        if (!ThongBaoLoi.Chay(this, () => maHd = _bus.HoanTat(_maHoSo, txtTrieuChung.Text, txtChanDoan.Text, txtLoiDan.Text, numTienCong.Value)))
        {
            NapDon(); // tồn có thể đã đổi: hiện lại đơn mới nhất
            HienDon();
            return;
        }
        _chuaLuu = false;
        DaHoanTat = true;
        MessageBox.Show(this, $"Đã hoàn tất khám. Hóa đơn {maHd} đã chuyển thu ngân.", Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        DialogResult = DialogResult.OK;
    }

    private void frmKhamBenh_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (_chiXem || !_chuaLuu || DialogResult == DialogResult.OK) return;
        var tl = MessageBox.Show(this, "Triệu chứng, chẩn đoán, lời dặn hoặc tiền công chưa được lưu. Lưu trước khi đóng?",
            "Đóng hồ sơ", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        if (tl == DialogResult.Cancel || (tl == DialogResult.Yes && !LuuNhap())) e.Cancel = true;
    }
}
