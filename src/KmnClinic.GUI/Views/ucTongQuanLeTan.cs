using KmnClinic.BUS;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Tổng quan trong ngày cho lễ tân: số lịch hẹn, hồ sơ tiếp nhận, bác sĩ đang trực.</summary>
public partial class ucTongQuanLeTan : UserControl
{
    private readonly TiepNhanBUS _bus = new();

    public ucTongQuanLeTan()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvBacSi);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        NapLai();
    }

    private void NapLai() => ThongBaoLoi.Chay(this, () =>
    {
        var t = _bus.TongQuan();
        theChoXacNhan.GiaTri = t.ChoXacNhan.ToString();
        theChoXacNhan.GhiChu = "khách tự đặt trên web";
        theHenHomNay.GiaTri = t.HenHomNay.ToString();
        theChoTiepNhan.GiaTri = t.ChoTiepNhan.ToString();
        theHoSo.GiaTri = t.HoSoHomNay.ToString();
        theVangLai.GiaTri = t.VangLaiHomNay.ToString();
        theBacSi.GiaTri = t.BacSiDangTruc.ToString();
        dgvBacSi.DataSource = _bus.BacSiDangTruc();
        lblCapNhat.Text = $"Cập nhật lúc {DateTime.Now:HH:mm}";
    });

    private void btnLamMoi_Click(object? sender, EventArgs e) => NapLai();
}
