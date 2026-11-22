using KmnClinic.DTO;
using KmnClinic.GUI.DuLieuGia;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Tổng quan của Quản trị viên. Module W8 thay dữ liệu giả bằng ThongKeBUS.</summary>
public partial class ucTongQuanQuanTri : UserControl
{
    public ucTongQuanQuanTri()
    {
        InitializeComponent();
        GiaoDien.Luoi(dgvSapHet);

        var hoaDon = Gia.HoaDon();
        var sapHet = Gia.Thuoc.Where(t => t.SoLuongTon < 20).OrderBy(t => t.SoLuongTon).ToList();
        theDoanhThu.GiaTri = GiaoDien.Tien(4_000_000);
        theDoanhThu.GhiChu = "+8% so với hôm qua";
        theLuotKham.GiaTri = Gia.HoSoHomNay().Count.ToString();
        theLuotKham.GhiChu = "4 khoa đang có ca trực";
        theLichHen.GiaTri = Gia.LichHen().Count(l => l.TrangThai == TrangThai.DaDat).ToString();
        theLichHen.GhiChu = "Lễ tân cần gọi xác nhận";
        theThuoc.GiaTri = sapHet.Count.ToString();
        theThuoc.GhiChu = "Tồn dưới 20";
        dgvSapHet.DataSource = sapHet;

        VeBieuDo();
    }

    private void VeBieuDo()
    {
        var duLieu = Gia.DoanhThu14Ngay();
        bieuDo.DatDuLieu(duLieu.Select(x => x.ngay.ToString("dd/MM")).ToArray(), duLieu.Select(x => x.trieu).ToArray());
    }
}
