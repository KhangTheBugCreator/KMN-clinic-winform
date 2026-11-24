using System.Data;
using KmnClinic.BUS;
using KmnClinic.DTO;

namespace KmnClinic.Tests;

/// <summary>W8: tổng quan quản trị, báo cáo, xuất CSV (chỉ đọc trên dữ liệu mẫu).</summary>
public class ThongKeTests : IDisposable
{
    private readonly CsdlThu _db = new();
    private readonly ThongKeBUS _tk = new();

    public ThongKeTests() => _db.DangNhap(ChucVu.QuanTriVien);
    public void Dispose() => _db.Dispose();

    private static void Loi(string thongBao, Action a) => Assert.Equal(thongBao, Assert.Throws<LoiNghiepVu>(a).Message);

    [Fact]
    public void TongQuan_14NgayDenHomNay_KhopCsdl()
    {
        var tq = _tk.TongQuan();
        Assert.Equal(14, tq.DoanhThu14Ngay.Count);
        Assert.Equal(DateTime.Today, tq.DoanhThu14Ngay[^1].Ngay);
        Assert.Equal(tq.DoanhThuHomNay, tq.DoanhThu14Ngay[^1].Tien);
        Assert.Equal(_db.GiaTri("SELECT COUNT(*) FROM Lich_hen WHERE trang_thai = N'Đã đặt'"), tq.LichHenChoXacNhan);
        Assert.Equal(_db.GiaTri("SELECT COUNT(*) FROM Hoa_don WHERE trang_thai_thanh_toan = N'Chưa thanh toán'"), tq.HoaDonChoThu);
        Assert.All(_tk.ThuocSapHet(), t => Assert.True(t.SoLuongTon <= ThuocDTO.NguongSapHet));
    }

    [Fact]
    public void BaoCao_DuLoai_VaKiemTraKy()
    {
        var tu = DateTime.Today.AddYears(-1).AddDays(1);
        var den = DateTime.Today;
        Assert.Equal("Tổng tiền", _tk.LapBaoCao(0, tu, den).Columns[7].ColumnName);
        var bs = _tk.LapBaoCao(1, tu, den);
        Assert.Equal(_db.GiaTri("SELECT COUNT(*) FROM Nhan_vien WHERE chuc_vu = N'Bác sĩ'"), bs.Rows.Count);
        Assert.Equal("Số lượng xuất", _tk.LapBaoCao(2, tu, den).Columns[4].ColumnName);
        Assert.Equal(_db.GiaTri("SELECT COUNT(*) FROM Thuoc"), _tk.LapBaoCao(3, tu, den).Rows.Count);
        Loi("Từ ngày phải trước hoặc bằng đến ngày.", () => _tk.LapBaoCao(0, den, tu));
        Loi("Kỳ báo cáo tối đa 1 năm.", () => _tk.LapBaoCao(0, den.AddYears(-1).AddDays(-1), den));
        _db.DangNhap(ChucVu.ThuNgan);
        Loi("Bạn không có quyền thực hiện chức năng này.", () => _tk.LapBaoCao(3, tu, den));
    }

    [Fact]
    public void XuatCsv_CoBom_BocOVaChanCongThuc()
    {
        var bang = new DataTable();
        bang.Columns.Add("Tên"); bang.Columns.Add("Ngày", typeof(DateTime)); bang.Columns.Add("Tiền", typeof(decimal));
        bang.Rows.Add("Nguyễn \"A\", B", new DateTime(2026, 11, 24, 9, 5, 0), 106000.5m);
        bang.Rows.Add("=1+1", new DateTime(2026, 11, 24), -5m);
        var file = Path.Combine(Path.GetTempPath(), $"kmn-{Guid.NewGuid():N}.csv");
        try
        {
            ThongKeBUS.XuatCsv(bang, file, "Báo cáo thử");
            var bytes = File.ReadAllBytes(file);
            Assert.Equal(new byte[] { 0xEF, 0xBB, 0xBF }, bytes[..3]);
            var dong = File.ReadAllLines(file);
            Assert.Equal(["Báo cáo thử", "", "Tên,Ngày,Tiền", "\"Nguyễn \"\"A\"\", B\",24/11/2026 09:05,106000.5", "'=1+1,24/11/2026,-5"], dong);
        }
        finally { File.Delete(file); }
    }
}
