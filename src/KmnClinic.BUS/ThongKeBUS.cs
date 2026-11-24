using System.Data;
using System.Globalization;
using System.Text;
using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Thống kê, báo cáo cho quản trị viên (W8). Chỉ đọc.</summary>
public class ThongKeBUS
{
    public static readonly string[] CacBaoCao = ["Doanh thu theo hóa đơn", "Lượt khám, doanh thu theo bác sĩ", "Thuốc đã xuất", "Tồn kho thuốc"];

    private readonly ThongKeDAL _dal = new();
    public Func<DateTime> BayGio { get; set; } = () => DateTime.Now;

    public TongQuanQuanTriDTO TongQuan()
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        return _dal.TongQuan(BayGio());
    }

    public List<ThuocDTO> ThuocSapHet()
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        return new ThuocDAL().DanhSach(null, true).OrderBy(t => t.SoLuongTon).ToList();
    }

    /// <summary>Lập báo cáo theo chỉ số trong CacBaoCao, kỳ [tu, den] tính cả hai đầu.</summary>
    public DataTable LapBaoCao(int loai, DateTime tu, DateTime den)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        if (tu.Date > den.Date) throw new LoiNghiepVu("Từ ngày phải trước hoặc bằng đến ngày.");
        if (tu.Date < den.Date.AddYears(-1)) throw new LoiNghiepVu("Kỳ báo cáo tối đa 1 năm.");
        return loai switch
        {
            0 => _dal.BaoCaoDoanhThu(tu, den),
            1 => _dal.BaoCaoBacSi(tu, den),
            2 => _dal.BaoCaoThuoc(tu, den),
            3 => _dal.BaoCaoTonKho(),
            _ => throw new LoiNghiepVu("Vui lòng chọn loại báo cáo."),
        };
    }

    /// <summary>
    /// Ghi bảng ra file CSV UTF-8 có BOM, dấu phẩy, để Excel mở thẳng được tiếng Việt. Ngày ghi dd/MM/yyyy HH:mm,
    /// số ghi dạng thuần (không phân cách nghìn) để Excel hiểu là số.
    /// </summary>
    public static void XuatCsv(DataTable bang, string duongDan, string? tieuDe = null)
    {
        var sb = new StringBuilder();
        if (tieuDe is not null) sb.AppendLine(O(tieuDe)).AppendLine();
        sb.AppendLine(string.Join(",", bang.Columns.Cast<DataColumn>().Select(c => O(c.ColumnName))));
        foreach (DataRow dong in bang.Rows)
            sb.AppendLine(string.Join(",", dong.ItemArray.Select(v => v switch
            {
                null or DBNull => "",
                DateTime d => d.TimeOfDay == TimeSpan.Zero ? d.ToString("dd/MM/yyyy") : d.ToString("dd/MM/yyyy HH:mm"),
                IFormattable so => O(so.ToString(null, CultureInfo.InvariantCulture)),
                _ => O(v.ToString()!),
            })));
        File.WriteAllText(duongDan, sb.ToString(), new UTF8Encoding(true));
    }

    /// <summary>Bọc ô CSV trong ngoặc kép nếu có dấu phẩy, ngoặc kép, xuống dòng; chặn công thức (= + - @) khi mở bằng Excel.</summary>
    private static string O(string s)
    {
        if (s.Length > 0 && "=+-@".Contains(s[0]) && !decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out _)) s = "'" + s;
        return s.IndexOfAny([',', '"', '\n', '\r']) >= 0 ? "\"" + s.Replace("\"", "\"\"") + "\"" : s;
    }
}
