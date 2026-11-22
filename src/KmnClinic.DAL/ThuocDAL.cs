using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>Bảng Thuoc. Không có xóa: thuốc đã nằm trong đơn cũ phải giữ lại để xem lịch sử.</summary>
public class ThuocDAL
{
    private const string SqlThuoc = @"
SELECT t.ma_thuoc, t.ten_thuoc, t.nsx, t.ham_luong, t.don_vi_tinh, t.don_gia_thuoc, t.so_luong_ton, t.mo_ta
FROM Thuoc t";

    /// <summary>Danh sách thuốc, lọc theo mã/tên/nhà sản xuất; chiSapHet = chỉ thuốc tồn ≤ ngưỡng.</summary>
    public List<ThuocDTO> DanhSach(string? tuKhoa = null, bool chiSapHet = false)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlThuoc + @"
WHERE (@q IS NULL OR t.ten_thuoc LIKE @q OR t.nsx LIKE @q OR t.ma_thuoc LIKE @q)
  AND (@sapHet = 0 OR t.so_luong_ton <= @nguong)
ORDER BY " + (chiSapHet ? "t.so_luong_ton, t.ten_thuoc" : "t.ten_thuoc, t.ham_luong"))
            .P("@q", SqlDbType.NVarChar, 210, TimKiem.MauLike(tuKhoa))
            .P("@sapHet", chiSapHet ? 1 : 0)
            .P("@nguong", ThuocDTO.NguongSapHet);
        return Doc(cmd);
    }

    /// <summary>Số thuốc có tồn ≤ ngưỡng sắp hết.</summary>
    public int DemSapHet()
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh("SELECT COUNT(*) FROM Thuoc WHERE so_luong_ton <= @nguong").P("@nguong", ThuocDTO.NguongSapHet);
        return (int)cmd.ExecuteScalar()!;
    }

    public ThuocDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlThuoc + " WHERE t.ma_thuoc = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        return Doc(cmd).FirstOrDefault();
    }

    public string Them(ThuocDTO t) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.Thuoc);
        using var cmd = GanThamSo(conn.Lenh(@"
INSERT INTO Thuoc (ma_thuoc, ten_thuoc, nsx, ham_luong, don_vi_tinh, don_gia_thuoc, so_luong_ton, mo_ta)
VALUES (@ma, @ten, @nsx, @hl, @dvt, @gia, @ton, @mt)", tx), t).P("@ma", SqlDbType.VarChar, 10, ma);
        cmd.ExecuteNonQuery();
        return ma;
    });

    /// <summary>
    /// Sửa thuốc. suaTon = false thì không đụng cột so_luong_ton, để không ghi đè số tồn vừa bị
    /// thu ngân trừ ở máy khác (Q8: chỉ sửa tồn khi quản trị viên xác nhận kiểm kho).
    /// Đổi đơn giá không ảnh hưởng đơn đã kê vì Chi_tiet_don_thuoc lưu đơn giá lúc kê (RB28).
    /// </summary>
    public bool Sua(ThuocDTO t, bool suaTon)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = GanThamSo(conn.Lenh(@"
UPDATE Thuoc SET ten_thuoc = @ten, nsx = @nsx, ham_luong = @hl, don_vi_tinh = @dvt, don_gia_thuoc = @gia, mo_ta = @mt"
            + (suaTon ? ", so_luong_ton = @ton" : "") + " WHERE ma_thuoc = @ma"), t)
            .P("@ma", SqlDbType.VarChar, 10, t.MaThuoc);
        return cmd.ExecuteNonQuery() > 0;
    }

    private static SqlCommand GanThamSo(SqlCommand c, ThuocDTO t) => c
        .P("@ten", SqlDbType.NVarChar, 100, t.TenThuoc)
        .P("@nsx", SqlDbType.NVarChar, 200, t.Nsx)
        .P("@hl", t.HamLuong)
        .P("@dvt", SqlDbType.NVarChar, 20, t.DonViTinh)
        .P("@gia", t.DonGiaThuoc)
        .P("@ton", t.SoLuongTon)
        .P("@mt", SqlDbType.NVarChar, 255, t.MoTa);

    private static List<ThuocDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<ThuocDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new ThuocDTO
            {
                MaThuoc = r.GetString(0), TenThuoc = r.GetString(1), Nsx = r.GetString(2), HamLuong = r.GetDecimal(3),
                DonViTinh = r.GetString(4), DonGiaThuoc = r.GetDecimal(5), SoLuongTon = r.GetInt32(6),
                MoTa = r.IsDBNull(7) ? null : r.GetString(7),
            });
        return ds;
    }
}
