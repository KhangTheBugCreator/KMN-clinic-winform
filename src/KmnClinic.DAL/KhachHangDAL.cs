using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>Bảng Khach_hang. Không đọc và không ghi cột mat_khau_kh (khách không dùng phần mềm, C2).</summary>
public class KhachHangDAL
{
    private const string SqlKhachHang = @"
SELECT kh.ma_kh, kh.ho_ten_kh, kh.ngay_sinh_kh, kh.gioi_tinh_kh, kh.sdt_kh, kh.dia_chi_kh,
       (SELECT COUNT(*) FROM Lich_hen h WHERE h.ma_kh = kh.ma_kh)   AS so_lich_hen,
       (SELECT COUNT(*) FROM Ho_so_kham s WHERE s.ma_kh = kh.ma_kh) AS so_ho_so
FROM Khach_hang kh";

    /// <summary>
    /// Danh sách khách, mới nhất trước; lọc theo từ khóa (mã, họ tên, SĐT) và giới tính
    /// (gioiTinh: null = tất cả, "Nam"/"Nữ", "Khác" = cột gioi_tinh_kh NULL).
    /// </summary>
    public List<KhachHangDTO> DanhSach(string? tuKhoa = null, string? gioiTinh = null)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlKhachHang + @"
WHERE (@q IS NULL OR kh.ho_ten_kh LIKE @q OR kh.sdt_kh LIKE @q OR kh.ma_kh LIKE @q)
  AND (@gt IS NULL OR kh.gioi_tinh_kh = @gt OR (@gt = N'Khác' AND kh.gioi_tinh_kh IS NULL))
ORDER BY kh.ma_kh DESC")
            .P("@q", SqlDbType.NVarChar, 60, TimKiem.MauLike(tuKhoa))
            .P("@gt", SqlDbType.NVarChar, 5, string.IsNullOrEmpty(gioiTinh) ? null : gioiTinh);
        return Doc(cmd);
    }

    public KhachHangDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlKhachHang + " WHERE kh.ma_kh = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        return Doc(cmd).FirstOrDefault();
    }

    /// <summary>Tìm đúng theo số điện thoại (sdt_kh là UNIQUE); lễ tân dùng khi khách gọi điện đặt lịch (W4).</summary>
    public KhachHangDTO? TimTheoSdt(string sdt)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlKhachHang + " WHERE kh.sdt_kh = @sdt").P("@sdt", SqlDbType.VarChar, 10, sdt);
        return Doc(cmd).FirstOrDefault();
    }

    /// <summary>Thêm khách. mat_khau_kh để NULL: khách không có tài khoản (RB37).</summary>
    public string Them(KhachHangDTO kh) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.KhachHang);
        using var cmd = GanThamSo(conn.Lenh(@"
INSERT INTO Khach_hang (ma_kh, ho_ten_kh, ngay_sinh_kh, gioi_tinh_kh, sdt_kh, dia_chi_kh, mat_khau_kh)
VALUES (@ma, @ten, @ns, @gt, @sdt, @dc, NULL)", tx), ma, kh);
        cmd.ExecuteNonQuery();
        return ma;
    });

    public bool Sua(KhachHangDTO kh)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = GanThamSo(conn.Lenh(@"
UPDATE Khach_hang SET ho_ten_kh = @ten, ngay_sinh_kh = @ns, gioi_tinh_kh = @gt, sdt_kh = @sdt, dia_chi_kh = @dc
WHERE ma_kh = @ma"), kh.MaKh, kh);
        return cmd.ExecuteNonQuery() > 0;
    }

    /// <summary>Xóa khách chưa có lịch hẹn và hồ sơ. DangDuocDung: SoLuongDangDung = số lịch hẹn, SoLuongDangDung2 = số hồ sơ.</summary>
    public KetQuaGhi Xoa(string ma) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        int soHen, soHoSo;
        using (var c = conn.Lenh(@"
SELECT (SELECT COUNT(*) FROM Lich_hen WITH (UPDLOCK, HOLDLOCK) WHERE ma_kh = @ma),
       (SELECT COUNT(*) FROM Ho_so_kham WITH (UPDLOCK, HOLDLOCK) WHERE ma_kh = @ma)", tx).P("@ma", SqlDbType.VarChar, 10, ma))
        using (var r = c.ExecuteReader())
        {
            r.Read();
            soHen = r.GetInt32(0);
            soHoSo = r.GetInt32(1);
        }
        if (soHen > 0 || soHoSo > 0) return new KetQuaGhi(TrangThaiGhi.DangDuocDung, soHen, soHoSo);

        using var xoa = conn.Lenh("DELETE FROM Khach_hang WHERE ma_kh = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        return xoa.ExecuteNonQuery() > 0 ? KetQuaGhi.ThanhCong : KetQuaGhi.KhongTimThay;
    });

    private static SqlCommand GanThamSo(SqlCommand c, string ma, KhachHangDTO kh) => c
        .P("@ma", SqlDbType.VarChar, 10, ma)
        .P("@ten", SqlDbType.NVarChar, 50, kh.HoTenKh)
        .P("@ns", SqlDbType.Date, 0, kh.NgaySinhKh?.Date)
        .P("@gt", SqlDbType.NVarChar, 5, kh.GioiTinhKh)
        .P("@sdt", SqlDbType.VarChar, 10, kh.SdtKh)
        .P("@dc", SqlDbType.NVarChar, 100, kh.DiaChiKh);

    private static List<KhachHangDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<KhachHangDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new KhachHangDTO
            {
                MaKh = r.GetString(0), HoTenKh = r.GetString(1), NgaySinhKh = r.GetDateTime(2), GioiTinhKh = r.IsDBNull(3) ? null : r.GetString(3),
                SdtKh = r.GetString(4), DiaChiKh = r.IsDBNull(5) ? null : r.GetString(5),
                SoLichHen = r.GetInt32(6), SoHoSo = r.GetInt32(7),
            });
        return ds;
    }
}
