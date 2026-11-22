using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>Bảng Khoa.</summary>
public class KhoaDAL
{
    private const string SqlKhoa = @"
SELECT k.ma_khoa, k.ten_khoa, k.mo_ta,
       (SELECT COUNT(*) FROM Phong_lam_viec p WHERE p.ma_khoa = k.ma_khoa) AS so_phong,
       (SELECT COUNT(*) FROM Nhan_vien n WHERE n.ma_khoa = k.ma_khoa)      AS so_bac_si
FROM Khoa k";

    /// <summary>Danh sách khoa, lọc theo mã hoặc tên (tuKhoa rỗng thì lấy hết).</summary>
    public List<KhoaDTO> DanhSach(string? tuKhoa = null)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlKhoa + " WHERE @q IS NULL OR k.ten_khoa LIKE @q OR k.ma_khoa LIKE @q ORDER BY k.ten_khoa")
            .P("@q", SqlDbType.NVarChar, 60, TimKiem.MauLike(tuKhoa));
        return Doc(cmd);
    }

    public KhoaDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlKhoa + " WHERE k.ma_khoa = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        return Doc(cmd).FirstOrDefault();
    }

    /// <summary>Thêm khoa, trả về mã mới sinh (K0001...).</summary>
    public string Them(string ten, string? moTa) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.Khoa);
        using var cmd = conn.Lenh("INSERT INTO Khoa (ma_khoa, ten_khoa, mo_ta) VALUES (@ma, @ten, @mt)", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@ten", SqlDbType.NVarChar, 50, ten).P("@mt", SqlDbType.NVarChar, 255, moTa);
        cmd.ExecuteNonQuery();
        return ma;
    });

    public bool Sua(string ma, string ten, string? moTa)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh("UPDATE Khoa SET ten_khoa = @ten, mo_ta = @mt WHERE ma_khoa = @ma")
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@ten", SqlDbType.NVarChar, 50, ten).P("@mt", SqlDbType.NVarChar, 255, moTa);
        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// Xóa khoa chỉ khi không còn phòng và bác sĩ. Đếm và xóa trong cùng transaction, khóa các dòng đếm
    /// để máy khác không thêm phòng/bác sĩ vào khoa giữa lúc đếm và lúc xóa.
    /// Kết quả DangDuocDung: SoLuongDangDung = số phòng, SoLuongDangDung2 = số bác sĩ.
    /// </summary>
    public KetQuaGhi Xoa(string ma) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        int soPhong, soBs;
        using (var c = conn.Lenh(@"
SELECT (SELECT COUNT(*) FROM Phong_lam_viec WITH (UPDLOCK, HOLDLOCK) WHERE ma_khoa = @ma),
       (SELECT COUNT(*) FROM Nhan_vien WITH (UPDLOCK, HOLDLOCK) WHERE ma_khoa = @ma)", tx).P("@ma", SqlDbType.VarChar, 10, ma))
        using (var r = c.ExecuteReader())
        {
            r.Read();
            soPhong = r.GetInt32(0);
            soBs = r.GetInt32(1);
        }
        if (soPhong > 0 || soBs > 0) return new KetQuaGhi(TrangThaiGhi.DangDuocDung, soPhong, soBs);

        using var xoa = conn.Lenh("DELETE FROM Khoa WHERE ma_khoa = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        return xoa.ExecuteNonQuery() > 0 ? KetQuaGhi.ThanhCong : KetQuaGhi.KhongTimThay;
    });

    private static List<KhoaDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<KhoaDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new KhoaDTO
            {
                MaKhoa = r.GetString(0), TenKhoa = r.GetString(1), MoTa = r.IsDBNull(2) ? null : r.GetString(2),
                SoPhong = r.GetInt32(3), SoBacSi = r.GetInt32(4),
            });
        return ds;
    }
}
