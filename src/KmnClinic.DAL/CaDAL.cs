using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>Bảng Ca_lam_viec.</summary>
public class CaDAL
{
    private const string SqlCa = @"
SELECT c.ma_ca, c.ten_ca, c.gio_bat_dau, c.gio_ket_thuc,
       (SELECT COUNT(*) FROM Lich_lam_viec l WHERE l.ma_ca = c.ma_ca) AS so_llv
FROM Ca_lam_viec c";

    public List<CaDTO> DanhSach()
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlCa + " ORDER BY c.gio_bat_dau, c.ten_ca");
        return Doc(cmd);
    }

    public CaDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlCa + " WHERE c.ma_ca = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        return Doc(cmd).FirstOrDefault();
    }

    public string Them(string ten, TimeSpan batDau, TimeSpan ketThuc) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.Ca);
        using var cmd = conn.Lenh("INSERT INTO Ca_lam_viec (ma_ca, ten_ca, gio_bat_dau, gio_ket_thuc) VALUES (@ma, @ten, @bd, @kt)", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@ten", SqlDbType.NVarChar, 20, ten)
            .P("@bd", SqlDbType.Time, 0, batDau).P("@kt", SqlDbType.Time, 0, ketThuc);
        cmd.ExecuteNonQuery();
        return ma;
    });

    /// <summary>
    /// Sửa ca. Ca đã được xếp lịch thì chỉ được đổi tên: đổi giờ sẽ làm sai lịch hẹn và lịch sử đã có.
    /// DangDuocDung: SoLuongDangDung = số lịch làm việc dùng ca.
    /// </summary>
    public KetQuaGhi Sua(string ma, string ten, TimeSpan batDau, TimeSpan ketThuc) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        TimeSpan bdCu, ktCu;
        int soLich;
        using (var c = conn.Lenh(@"
SELECT c.gio_bat_dau, c.gio_ket_thuc, (SELECT COUNT(*) FROM Lich_lam_viec l WITH (UPDLOCK, HOLDLOCK) WHERE l.ma_ca = c.ma_ca)
FROM Ca_lam_viec c WITH (UPDLOCK) WHERE c.ma_ca = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma))
        using (var r = c.ExecuteReader())
        {
            if (!r.Read()) return KetQuaGhi.KhongTimThay;
            bdCu = r.GetTimeSpan(0);
            ktCu = r.GetTimeSpan(1);
            soLich = r.GetInt32(2);
        }
        if ((bdCu != batDau || ktCu != ketThuc) && soLich > 0) return new KetQuaGhi(TrangThaiGhi.DangDuocDung, soLich);

        using var cmd = conn.Lenh("UPDATE Ca_lam_viec SET ten_ca = @ten, gio_bat_dau = @bd, gio_ket_thuc = @kt WHERE ma_ca = @ma", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@ten", SqlDbType.NVarChar, 20, ten)
            .P("@bd", SqlDbType.Time, 0, batDau).P("@kt", SqlDbType.Time, 0, ketThuc);
        cmd.ExecuteNonQuery();
        return KetQuaGhi.ThanhCong;
    });

    /// <summary>Xóa ca chưa được dùng trong lịch làm việc nào. DangDuocDung: số lịch làm việc.</summary>
    public KetQuaGhi Xoa(string ma) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        using (var c = conn.Lenh("SELECT COUNT(*) FROM Lich_lam_viec WITH (UPDLOCK, HOLDLOCK) WHERE ma_ca = @ma", tx)
                   .P("@ma", SqlDbType.VarChar, 10, ma))
        {
            var soLich = (int)c.ExecuteScalar()!;
            if (soLich > 0) return new KetQuaGhi(TrangThaiGhi.DangDuocDung, soLich);
        }
        using var xoa = conn.Lenh("DELETE FROM Ca_lam_viec WHERE ma_ca = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        return xoa.ExecuteNonQuery() > 0 ? KetQuaGhi.ThanhCong : KetQuaGhi.KhongTimThay;
    });

    private static List<CaDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<CaDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new CaDTO
            {
                MaCa = r.GetString(0), TenCa = r.GetString(1), GioBatDau = r.GetTimeSpan(2), GioKetThuc = r.GetTimeSpan(3),
                SoLichLamViec = r.GetInt32(4),
            });
        return ds;
    }
}
