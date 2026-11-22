using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>
/// Danh mục thuốc: Quản trị viên, Bác sĩ, Thu ngân xem; chỉ Quản trị viên thêm/sửa.
/// Không có xóa thuốc (giữ lịch sử đơn thuốc). Sửa tồn kho phải xác nhận (Q8).
/// </summary>
public class ThuocBUS
{
    private readonly ThuocDAL _dal = new();

    public List<ThuocDTO> DanhSach(string? tuKhoa = null, bool chiSapHet = false)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.BacSi, ChucVu.ThuNgan);
        return _dal.DanhSach(tuKhoa, chiSapHet);
    }

    public int DemSapHet()
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien, ChucVu.BacSi, ChucVu.ThuNgan);
        return _dal.DemSapHet();
    }

    /// <summary>Thêm thuốc, trả về mã mới.</summary>
    public string Them(ThuocDTO thuoc)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var t = ChuanHoa(thuoc);
        return KiemTra.Ghi(() => _dal.Them(t));
    }

    /// <summary>
    /// Sửa thuốc. suaTon = true khi quản trị viên đã tick "xác nhận sửa trực tiếp số lượng tồn";
    /// nếu false thì số tồn trong CSDL giữ nguyên, bỏ qua giá trị SoLuongTon truyền vào.
    /// </summary>
    public void Sua(ThuocDTO thuoc, bool suaTon)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var t = ChuanHoa(thuoc);
        t.MaThuoc = thuoc.MaThuoc;
        if (!KiemTra.Ghi(() => _dal.Sua(t, suaTon))) throw new LoiNghiepVu("Không tìm thấy thuốc.");
    }

    private static ThuocDTO ChuanHoa(ThuocDTO t)
    {
        if (t.HamLuong is not > 0) throw new LoiNghiepVu("Hàm lượng phải lớn hơn 0.");
        if (t.DonGiaThuoc <= 0) throw new LoiNghiepVu("Đơn giá phải lớn hơn 0.");
        if (t.DonGiaThuoc != decimal.Truncate(t.DonGiaThuoc)) throw new LoiNghiepVu("Đơn giá tính theo đồng, không có phần lẻ.");
        if (t.SoLuongTon < 0) throw new LoiNghiepVu("Số lượng tồn không được âm.");
        return new ThuocDTO
        {
            TenThuoc = KiemTra.BatBuoc(t.TenThuoc, "tên thuốc", 100),
            Nsx = KiemTra.BatBuoc(t.Nsx, "nhà sản xuất", 200),
            HamLuong = decimal.Round(t.HamLuong.Value, 2),
            DonViTinh = KiemTra.BatBuoc(t.DonViTinh, "đơn vị tính", 20),
            DonGiaThuoc = t.DonGiaThuoc,
            SoLuongTon = t.SoLuongTon,
            MoTa = KiemTra.TuyChon(t.MoTa, "mô tả", 255),
        };
    }
}
