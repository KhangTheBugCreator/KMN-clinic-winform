using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>
/// Tổng quan của quản trị viên: doanh thu hôm nay (so với hôm qua), lượt khám, lịch hẹn chờ xác nhận, thuốc sắp hết,
/// biểu đồ doanh thu 14 ngày.
/// </summary>
public partial class ucTongQuanQuanTri : UserControl
{
    private readonly ThongKeBUS _bus = new();

    public ucTongQuanQuanTri()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvSapHet);
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (DesignMode) return;
        TongQuanQuanTriDTO tq = new();
        List<ThuocDTO> sapHet = [];
        if (!ThongBaoLoi.Chay(this, () => { tq = _bus.TongQuan(); sapHet = _bus.ThuocSapHet(); })) return;

        theDoanhThu.GiaTri = GiaoDien.Tien(tq.DoanhThuHomNay);
        theDoanhThu.GhiChu = $"{tq.HoaDonHomNay} hóa đơn · hôm qua {GiaoDien.Tien(tq.DoanhThuHomQua)}";
        theLuotKham.GiaTri = tq.LuotKhamHomNay.ToString();
        theLuotKham.GhiChu = $"{tq.LuotKhamHomNay - tq.VangLaiHomNay} có hẹn · {tq.VangLaiHomNay} vãng lai";
        theLichHen.GiaTri = tq.LichHenChoXacNhan.ToString();
        theLichHen.GhiChu = $"Khách tự đặt · {tq.HoaDonChoThu} hóa đơn chờ thu";
        theThuoc.GiaTri = sapHet.Count.ToString();
        theThuoc.GhiChu = $"Tồn từ {ThuocDTO.NguongSapHet} trở xuống";
        dgvSapHet.DataSource = sapHet;

        bieuDo.DatDuLieu(tq.DoanhThu14Ngay.Select(x => x.Ngay.ToString("dd/MM")).ToArray(),
            tq.DoanhThu14Ngay.Select(x => (double)(x.Tien / 1000)).ToArray());
    }
}
