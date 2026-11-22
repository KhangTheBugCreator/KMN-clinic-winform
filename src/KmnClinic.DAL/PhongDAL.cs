using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>Bảng Phong_lam_viec.</summary>
public class PhongDAL
{
    private const string SqlPhong = @"
SELECT p.ma_phong, p.ten_phong, p.ma_khoa, k.ten_khoa,
       (SELECT COUNT(*) FROM Lich_lam_viec l WHERE l.ma_phong = p.ma_phong) AS so_llv
FROM Phong_lam_viec p JOIN Khoa k ON k.ma_khoa = p.ma_khoa";

    /// <summary>Danh sách phòng, lọc theo từ khóa (mã, tên) và theo khoa (null = mọi khoa).</summary>
    public List<PhongDTO> DanhSach(string? tuKhoa = null, string? maKhoa = null)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlPhong + @"
WHERE (@q IS NULL OR p.ten_phong LIKE @q OR p.ma_phong LIKE @q) AND (@khoa IS NULL OR p.ma_khoa = @khoa)
ORDER BY p.ten_phong")
            .P("@q", SqlDbType.NVarChar, 60, TimKiem.MauLike(tuKhoa))
            .P("@khoa", SqlDbType.VarChar, 10, string.IsNullOrEmpty(maKhoa) ? null : maKhoa);
        return Doc(cmd);
    }

    public PhongDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlPhong + " WHERE p.ma_phong = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        return Doc(cmd).FirstOrDefault();
    }

    public string Them(string ten, string maKhoa) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.Phong);
        using var cmd = conn.Lenh("INSERT INTO Phong_lam_viec (ma_phong, ten_phong, ma_khoa) VALUES (@ma, @ten, @khoa)", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@ten", SqlDbType.NVarChar, 50, ten).P("@khoa", SqlDbType.VarChar, 10, maKhoa);
        cmd.ExecuteNonQuery();
        return ma;
    });

    /// <summary>
    /// Sửa phòng. Không cho đổi khoa khi phòng đã có lịch làm việc (RB11: phòng thuộc khoa của bác sĩ trực;
    /// trigger chỉ kiểm tra lúc tạo lịch nên đổi khoa sau đó sẽ làm lịch cũ sai RB11).
    /// DangDuocDung: SoLuongDangDung = số lịch làm việc của phòng.
    /// </summary>
    public KetQuaGhi Sua(string ma, string ten, string maKhoa) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        string khoaCu;
        int soLich;
        using (var c = conn.Lenh(@"
SELECT p.ma_khoa, (SELECT COUNT(*) FROM Lich_lam_viec l WITH (UPDLOCK, HOLDLOCK) WHERE l.ma_phong = p.ma_phong)
FROM Phong_lam_viec p WITH (UPDLOCK) WHERE p.ma_phong = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma))
        using (var r = c.ExecuteReader())
        {
            if (!r.Read()) return KetQuaGhi.KhongTimThay;
            khoaCu = r.GetString(0);
            soLich = r.GetInt32(1);
        }
        if (khoaCu != maKhoa && soLich > 0) return new KetQuaGhi(TrangThaiGhi.DangDuocDung, soLich);

        using var cmd = conn.Lenh("UPDATE Phong_lam_viec SET ten_phong = @ten, ma_khoa = @khoa WHERE ma_phong = @ma", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@ten", SqlDbType.NVarChar, 50, ten).P("@khoa", SqlDbType.VarChar, 10, maKhoa);
        cmd.ExecuteNonQuery();
        return KetQuaGhi.ThanhCong;
    });

    /// <summary>Xóa phòng chưa từng được xếp lịch (giữ lịch sử sử dụng phòng). DangDuocDung: số lịch làm việc.</summary>
    public KetQuaGhi Xoa(string ma) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        using (var c = conn.Lenh("SELECT COUNT(*) FROM Lich_lam_viec WITH (UPDLOCK, HOLDLOCK) WHERE ma_phong = @ma", tx)
                   .P("@ma", SqlDbType.VarChar, 10, ma))
        {
            var soLich = (int)c.ExecuteScalar()!;
            if (soLich > 0) return new KetQuaGhi(TrangThaiGhi.DangDuocDung, soLich);
        }
        using var xoa = conn.Lenh("DELETE FROM Phong_lam_viec WHERE ma_phong = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        return xoa.ExecuteNonQuery() > 0 ? KetQuaGhi.ThanhCong : KetQuaGhi.KhongTimThay;
    });

    private static List<PhongDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<PhongDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new PhongDTO
            {
                MaPhong = r.GetString(0), TenPhong = r.GetString(1), MaKhoa = r.GetString(2), TenKhoa = r.GetString(3),
                SoLichLamViec = r.GetInt32(4),
            });
        return ds;
    }
}
