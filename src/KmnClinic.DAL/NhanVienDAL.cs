using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>
/// Bảng Nhan_vien: đăng nhập, đổi mật khẩu (W0) và quản lý nhân viên (W2).
/// Danh sách không bao giờ đọc cột mat_khau_nv; chỉ TimTheoSdt/LayMatKhau đọc để BUS kiểm tra.
/// </summary>
public class NhanVienDAL
{
    /// <summary>Số dòng nghiệp vụ trỏ tới nhân viên: lịch làm việc, lịch hẹn (lễ tân), hồ sơ (khám/tiếp nhận), hóa đơn.</summary>
    private const string SqlSoLienKet = @"
  (SELECT COUNT(*) FROM Lich_lam_viec x WHERE x.ma_nv = nv.ma_nv)
+ (SELECT COUNT(*) FROM Lich_hen x WHERE x.ma_nv = nv.ma_nv)
+ (SELECT COUNT(*) FROM Ho_so_kham x WHERE x.ma_nv_kham = nv.ma_nv OR x.ma_nv_tiep_nhan = nv.ma_nv)
+ (SELECT COUNT(*) FROM Hoa_don x WHERE x.ma_nv = nv.ma_nv)";

    private const string SqlNhanVien = @"
SELECT nv.ma_nv, nv.ho_ten_nv, nv.ngay_sinh_nv, nv.sdt_nv, nv.chuc_vu, nv.ma_khoa, k.ten_khoa, nv.duong_dan_anh,
       " + SqlSoLienKet + @" AS so_lien_ket
FROM Nhan_vien nv LEFT JOIN Khoa k ON k.ma_khoa = nv.ma_khoa";

    // ------------------------------------------------------------ đăng nhập (W0)

    /// <summary>Tìm nhân viên theo số điện thoại (sdt_nv là UNIQUE). Trả về cả mật khẩu đã lưu để BUS kiểm tra.</summary>
    public (NhanVienDTO NhanVien, string MatKhauDaLuu)? TimTheoSdt(string sdt)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(@"
SELECT nv.ma_nv, nv.ho_ten_nv, nv.ngay_sinh_nv, nv.sdt_nv, nv.chuc_vu, nv.ma_khoa, k.ten_khoa, nv.duong_dan_anh, 0, nv.mat_khau_nv
FROM Nhan_vien nv LEFT JOIN Khoa k ON k.ma_khoa = nv.ma_khoa WHERE nv.sdt_nv = @sdt").P("@sdt", SqlDbType.VarChar, 10, sdt);
        using var r = cmd.ExecuteReader();
        if (!r.Read()) return null;
        return (DocNhanVien(r), r.GetString(9));
    }

    /// <summary>Lấy mật khẩu đã lưu của một nhân viên (null nếu không có nhân viên đó).</summary>
    public string? LayMatKhau(string maNv)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh("SELECT mat_khau_nv FROM Nhan_vien WHERE ma_nv = @ma").P("@ma", SqlDbType.VarChar, 10, maNv);
        return cmd.ExecuteScalar() as string;
    }

    /// <summary>Ghi mật khẩu mới (đã băm). Trả về true nếu cập nhật được một dòng.</summary>
    public bool CapNhatMatKhau(string maNv, string matKhauDaBam)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh("UPDATE Nhan_vien SET mat_khau_nv = @mk WHERE ma_nv = @ma")
            .P("@mk", SqlDbType.VarChar, 255, matKhauDaBam)
            .P("@ma", SqlDbType.VarChar, 10, maNv);
        return cmd.ExecuteNonQuery() == 1;
    }

    // ------------------------------------------------------------ quản lý nhân viên (W2)

    /// <summary>Danh sách nhân viên, lọc theo từ khóa (mã, họ tên, SĐT), chức vụ, khoa (null = không lọc).</summary>
    public List<NhanVienDTO> DanhSach(string? tuKhoa = null, string? chucVu = null, string? maKhoa = null)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlNhanVien + @"
WHERE (@q IS NULL OR nv.ho_ten_nv LIKE @q OR nv.sdt_nv LIKE @q OR nv.ma_nv LIKE @q)
  AND (@cv IS NULL OR nv.chuc_vu = @cv) AND (@khoa IS NULL OR nv.ma_khoa = @khoa)
ORDER BY nv.ma_nv")
            .P("@q", SqlDbType.NVarChar, 60, TimKiem.MauLike(tuKhoa))
            .P("@cv", SqlDbType.NVarChar, 20, string.IsNullOrEmpty(chucVu) ? null : chucVu)
            .P("@khoa", SqlDbType.VarChar, 10, string.IsNullOrEmpty(maKhoa) ? null : maKhoa);
        var ds = new List<NhanVienDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read()) ds.Add(DocNhanVien(r));
        return ds;
    }

    public NhanVienDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlNhanVien + " WHERE nv.ma_nv = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        using var r = cmd.ExecuteReader();
        return r.Read() ? DocNhanVien(r) : null;
    }

    public string Them(NhanVienDTO nv, string matKhauDaBam) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.NhanVien);
        using var cmd = GanThamSo(conn.Lenh(@"
INSERT INTO Nhan_vien (ma_nv, ho_ten_nv, ngay_sinh_nv, sdt_nv, chuc_vu, mat_khau_nv, ma_khoa, duong_dan_anh)
VALUES (@ma, @ten, @ns, @sdt, @cv, @mk, @khoa, @anh)", tx), ma, nv).P("@mk", SqlDbType.VarChar, 255, matKhauDaBam);
        cmd.ExecuteNonQuery();
        return ma;
    });

    /// <summary>
    /// Sửa thông tin nhân viên (mật khẩu mới null = giữ nguyên). Bị chặn khi:
    /// - đổi chức vụ hoặc khoa mà nhân viên đã có dữ liệu nghiệp vụ (trigger chỉ kiểm tra RB10, RB11, RB17, RB20, RB34
    ///   lúc thêm dòng mới, đổi sau đó làm dữ liệu cũ sai mà không ai báo) → DangDuocDung (số liên kết);
    /// - quản trị viên tự bỏ chức vụ quản trị của mình → TuThaoTacChinhMinh;
    /// - bỏ chức vụ của quản trị viên cuối cùng → HetQuanTriVien.
    /// </summary>
    public KetQuaGhi Sua(NhanVienDTO nv, string? matKhauMoiDaBam, string maNguoiSua) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var cu = KhoaVaDoc(conn, tx, nv.MaNv);
        if (cu is null) return KetQuaGhi.KhongTimThay;
        var (chucVuCu, khoaCu, soLienKet) = cu.Value;

        if ((chucVuCu != nv.ChucVu || khoaCu != nv.MaKhoa) && soLienKet > 0)
            return new KetQuaGhi(TrangThaiGhi.DangDuocDung, soLienKet);
        if (chucVuCu == ChucVu.QuanTriVien && nv.ChucVu != chucVuCu)
        {
            if (nv.MaNv == maNguoiSua) return new KetQuaGhi(TrangThaiGhi.TuThaoTacChinhMinh);
            if (!ConQuanTriKhac(conn, tx, nv.MaNv)) return new KetQuaGhi(TrangThaiGhi.HetQuanTriVien);
        }

        using var cmd = GanThamSo(conn.Lenh(@"
UPDATE Nhan_vien SET ho_ten_nv = @ten, ngay_sinh_nv = @ns, sdt_nv = @sdt, chuc_vu = @cv, ma_khoa = @khoa, duong_dan_anh = @anh"
            + (matKhauMoiDaBam is null ? "" : ", mat_khau_nv = @mk") + " WHERE ma_nv = @ma", tx), nv.MaNv, nv);
        if (matKhauMoiDaBam is not null) cmd.P("@mk", SqlDbType.VarChar, 255, matKhauMoiDaBam);
        cmd.ExecuteNonQuery();
        return KetQuaGhi.ThanhCong;
    });

    /// <summary>Xóa nhân viên chưa có dữ liệu nghiệp vụ; không tự xóa mình, không xóa quản trị viên cuối cùng.</summary>
    public KetQuaGhi Xoa(string ma, string maNguoiXoa) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var cu = KhoaVaDoc(conn, tx, ma);
        if (cu is null) return KetQuaGhi.KhongTimThay;
        if (ma == maNguoiXoa) return new KetQuaGhi(TrangThaiGhi.TuThaoTacChinhMinh);
        if (cu.Value.SoLienKet > 0) return new KetQuaGhi(TrangThaiGhi.DangDuocDung, cu.Value.SoLienKet);
        if (cu.Value.ChucVu == ChucVu.QuanTriVien && !ConQuanTriKhac(conn, tx, ma)) return new KetQuaGhi(TrangThaiGhi.HetQuanTriVien);

        using var xoa = conn.Lenh("DELETE FROM Nhan_vien WHERE ma_nv = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        return xoa.ExecuteNonQuery() > 0 ? KetQuaGhi.ThanhCong : KetQuaGhi.KhongTimThay;
    });

    /// <summary>Khóa dòng nhân viên và các dòng con (UPDLOCK, HOLDLOCK) để kết quả kiểm tra không đổi tới lúc ghi.</summary>
    private static (string ChucVu, string? MaKhoa, int SoLienKet)? KhoaVaDoc(SqlConnection conn, SqlTransaction tx, string ma)
    {
        using var c = conn.Lenh(@"
SELECT nv.chuc_vu, nv.ma_khoa,
  (SELECT COUNT(*) FROM Lich_lam_viec x WITH (UPDLOCK, HOLDLOCK) WHERE x.ma_nv = nv.ma_nv)
+ (SELECT COUNT(*) FROM Lich_hen x WITH (UPDLOCK, HOLDLOCK) WHERE x.ma_nv = nv.ma_nv)
+ (SELECT COUNT(*) FROM Ho_so_kham x WITH (UPDLOCK, HOLDLOCK) WHERE x.ma_nv_kham = nv.ma_nv OR x.ma_nv_tiep_nhan = nv.ma_nv)
+ (SELECT COUNT(*) FROM Hoa_don x WITH (UPDLOCK, HOLDLOCK) WHERE x.ma_nv = nv.ma_nv)
FROM Nhan_vien nv WITH (UPDLOCK) WHERE nv.ma_nv = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        using var r = c.ExecuteReader();
        if (!r.Read()) return null;
        return (r.GetString(0), r.IsDBNull(1) ? null : r.GetString(1), r.GetInt32(2));
    }

    private static bool ConQuanTriKhac(SqlConnection conn, SqlTransaction tx, string ma)
    {
        using var c = conn.Lenh("SELECT COUNT(*) FROM Nhan_vien WITH (UPDLOCK, HOLDLOCK) WHERE chuc_vu = @qtv AND ma_nv <> @ma", tx)
            .P("@qtv", SqlDbType.NVarChar, 20, ChucVu.QuanTriVien).P("@ma", SqlDbType.VarChar, 10, ma);
        return (int)c.ExecuteScalar()! > 0;
    }

    private static SqlCommand GanThamSo(SqlCommand c, string ma, NhanVienDTO nv) => c
        .P("@ma", SqlDbType.VarChar, 10, ma)
        .P("@ten", SqlDbType.NVarChar, 50, nv.HoTenNv)
        .P("@ns", SqlDbType.Date, 0, nv.NgaySinhNv?.Date)
        .P("@sdt", SqlDbType.VarChar, 10, nv.SdtNv)
        .P("@cv", SqlDbType.NVarChar, 20, nv.ChucVu)
        .P("@khoa", SqlDbType.VarChar, 10, nv.MaKhoa)
        .P("@anh", SqlDbType.NVarChar, 1000, nv.DuongDanAnh);

    internal static NhanVienDTO DocNhanVien(SqlDataReader r) => new()
    {
        MaNv = r.GetString(0),
        HoTenNv = r.GetString(1),
        NgaySinhNv = r.IsDBNull(2) ? null : r.GetDateTime(2),
        SdtNv = r.GetString(3),
        ChucVu = r.GetString(4),
        MaKhoa = r.IsDBNull(5) ? null : r.GetString(5),
        TenKhoa = r.IsDBNull(6) ? null : r.GetString(6),
        DuongDanAnh = r.IsDBNull(7) ? null : r.GetString(7),
        SoLienKet = r.GetInt32(8),
    };
}
