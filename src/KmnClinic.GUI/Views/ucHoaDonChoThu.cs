using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Forms;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Thu ngân: hóa đơn chờ thu (bác sĩ đã hoàn tất khám), khám sớm nhất lên đầu. Tự nạp lại mỗi 30 giây
/// để thấy hóa đơn mới.
/// </summary>
public partial class ucHoaDonChoThu : UserControl
{
    private readonly ThanhToanBUS _bus = new();
    private readonly System.Windows.Forms.Timer _hen = new() { Interval = 30_000 };
    private bool _dangNap = true;

    public ucHoaDonChoThu()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvHoaDon);
        _hen.Tick += (_, _) => NapLai(imLang: true);
        Disposed += (_, _) => _hen.Dispose();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        _dangNap = false;
        NapLai();
        _hen.Start();
    }

    private HoaDonDTO? DangChon => dgvHoaDon.CurrentRow?.DataBoundItem as HoaDonDTO;

    /// <param name="imLang">Lần nạp tự động: lỗi thì bỏ qua, không bật hộp thoại.</param>
    private void NapLai(string? chonMa = null, bool imLang = false)
    {
        if (_dangNap) return;
        chonMa ??= DangChon?.MaHoaDon;
        List<HoaDonDTO> ds;
        TongQuanThuNganDTO tq;
        try
        {
            ds = _bus.ChoThu(txtTim.Text);
            tq = _bus.TongQuan();
        }
        catch (Exception ex)
        {
            if (!imLang) ThongBaoLoi.Hien(ex, this);
            return;
        }
        dgvHoaDon.DataSource = ds;
        GiaoDien.ChonDong(dgvHoaDon, chonMa is null ? null : o => ((HoaDonDTO)o).MaHoaDon == chonMa);
        theChoThu.GiaTri = tq.ChoThu.ToString();
        theChoThu.GhiChu = "Nhấp đúp để thu tiền";
        theTienCho.GiaTri = GiaoDien.Tien(tq.TienChoThu);
        theDaThu.GiaTri = GiaoDien.Tien(tq.DoanhThuHomNay);
        theDaThu.GhiChu = $"{tq.DaThuHomNay} hóa đơn";
        theCuaToi.GiaTri = tq.CuaToiHomNay.ToString();
        theCuaToi.GhiChu = "hóa đơn";
        btnThu.Enabled = ds.Count > 0;
    }

    private void txtTim_TextChanged(object? sender, EventArgs e) => NapLai();

    private void btnLamMoi_Click(object? sender, EventArgs e) => NapLai();

    private void btnThu_Click(object? sender, EventArgs e)
    {
        if (DangChon is not { } hd) return;
        _hen.Stop();
        using (var f = new frmThuTien(hd.MaHoaDon)) f.ShowDialog(this);
        NapLai();
        _hen.Start();
    }

    private void dgvHoaDon_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0) btnThu_Click(sender, e);
    }
}
