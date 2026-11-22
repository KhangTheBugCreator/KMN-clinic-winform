using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>Lý do không xếp/sửa/xóa được lịch làm việc. BUS đổi thành câu thông báo.</summary>
public enum LoiXepLich
{
    Khong,
    KhongTimThayBacSi,
    KhongPhaiBacSi,     // RB10
    KhongTimThayPhong,
    KhongTimThayCa,
    PhongKhacKhoa,      // RB11
    TrungLich,          // RB12, RB13
    KhongTimThayLich,
    DaQua,
    DaCoLichHen,
    DaCoKhachVangLai,
}

/// <summary>Một ngày bị trùng: bác sĩ đã có lịch ca đó (RB13) hoặc phòng đã có bác sĩ khác (RB12).</summary>
public sealed record TrungLich(DateTime Ngay, bool TrungBacSi, string TenBacSi, string TenPhong);

public sealed class KetQuaXepLich
{
    public LoiXepLich Loi { get; init; }
    public List<TrungLich> DanhSachTrung { get; init; } = [];
    public List<string> CacMaMoi { get; init; } = [];
    /// <summary>Số lịch hẹn / hồ sơ vãng lai làm lịch không đổi được; ngày của lịch đã qua.</summary>
    public int SoLuong { get; init; }
    public DateTime? Ngay { get; init; }

    public static KetQuaXepLich ThanhCong(List<string>? ma = null) => new() { CacMaMoi = ma ?? [] };
    public static KetQuaXepLich LoiDon(LoiXepLich loi, int soLuong = 0, DateTime? ngay = null) => new() { Loi = loi, SoLuong = soLuong, Ngay = ngay };
}

/// <summary>
/// Bảng Lich_lam_viec. Trigger TG_LICH_LAM_VIEC_KiemTra (RB9, RB10, RB11) và hai UNIQUE (RB12, RB13) là lớp chặn cuối;
/// DAL kiểm tra trước trong cùng transaction để báo rõ ai đang dùng phòng, bác sĩ đang ở phòng nào.
/// </summary>
public class LichLamViecDAL
{
    private const string SqlLich = @"
SELECT l.ma_lich_lam_viec, l.ngay_lam_viec, c.ma_ca, c.ten_ca, c.gio_bat_dau, c.gio_ket_thuc,
       n.ma_nv, n.ho_ten_nv, k.ma_khoa, k.ten_khoa, p.ma_phong, p.ten_phong,
       (SELECT COUNT(*) FROM Lich_hen h WHERE h.ma_lich_lam_viec = l.ma_lich_lam_viec) AS so_hen,
       (SELECT COUNT(*) FROM Lich_hen h WHERE h.ma_lich_lam_viec = l.ma_lich_lam_viec
            AND h.trang_thai IN (N'Đã đặt', N'Đã xác nhận')) AS so_hen_con
FROM Lich_lam_viec l
JOIN Ca_lam_viec c ON c.ma_ca = l.ma_ca
JOIN Nhan_vien n ON n.ma_nv = l.ma_nv
JOIN Phong_lam_viec p ON p.ma_phong = l.ma_phong
JOIN Khoa k ON k.ma_khoa = p.ma_khoa";

    /// <summary>Lịch trong khoảng ngày [tu, den], lọc theo bác sĩ, khoa, phòng, ca (null = không lọc).</summary>
    public List<LichLamViecDTO> DanhSach(DateTime tu, DateTime den, string? maNv = null, string? maKhoa = null,
        string? maPhong = null, string? maCa = null)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlLich + @"
WHERE l.ngay_lam_viec BETWEEN @tu AND @den
  AND (@nv IS NULL OR l.ma_nv = @nv) AND (@khoa IS NULL OR p.ma_khoa = @khoa)
  AND (@phong IS NULL OR l.ma_phong = @phong) AND (@ca IS NULL OR l.ma_ca = @ca)
ORDER BY l.ngay_lam_viec, c.gio_bat_dau, p.ten_phong")
            .P("@tu", SqlDbType.Date, 0, tu.Date).P("@den", SqlDbType.Date, 0, den.Date)
            .P("@nv", SqlDbType.VarChar, 10, Rong(maNv)).P("@khoa", SqlDbType.VarChar, 10, Rong(maKhoa))
            .P("@phong", SqlDbType.VarChar, 10, Rong(maPhong)).P("@ca", SqlDbType.VarChar, 10, Rong(maCa));
        return Doc(cmd);
    }

    public LichLamViecDTO? Tim(string ma)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlLich + " WHERE l.ma_lich_lam_viec = @ma").P("@ma", SqlDbType.VarChar, 10, ma);
        return Doc(cmd).FirstOrDefault();
    }

    /// <summary>Bác sĩ (có khoa) để chọn khi xếp lịch; maKhoa null = mọi khoa.</summary>
    public List<NhanVienDTO> DanhSachBacSi(string? maKhoa = null)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(@"
SELECT n.ma_nv, n.ho_ten_nv, k.ma_khoa, k.ten_khoa
FROM Nhan_vien n JOIN Khoa k ON k.ma_khoa = n.ma_khoa
WHERE n.chuc_vu = N'Bác sĩ' AND (@khoa IS NULL OR n.ma_khoa = @khoa)
ORDER BY k.ten_khoa, n.ho_ten_nv").P("@khoa", SqlDbType.VarChar, 10, Rong(maKhoa));
        var ds = new List<NhanVienDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new NhanVienDTO
            {
                MaNv = r.GetString(0), HoTenNv = r.GetString(1), ChucVu = ChucVu.BacSi, MaKhoa = r.GetString(2), TenKhoa = r.GetString(3),
            });
        return ds;
    }

    /// <summary>
    /// Xếp một bác sĩ vào cùng phòng, cùng ca cho các ngày đã cho: tất cả hoặc không ngày nào
    /// (một ngày trùng thì cả lô bị hủy, báo đủ các ngày trùng).
    /// </summary>
    public KetQuaXepLich Them(List<DateTime> cacNgay, string maNv, string maPhong, string maCa) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var loi = KiemTraNguonLuc(conn, tx, maNv, maPhong, maCa);
        if (loi != LoiXepLich.Khong) return KetQuaXepLich.LoiDon(loi);
        var trung = TimTrung(conn, tx, null, cacNgay, maNv, maPhong, maCa);
        if (trung.Count > 0) return new KetQuaXepLich { Loi = LoiXepLich.TrungLich, DanhSachTrung = trung };

        var cacMa = new List<string>();
        foreach (var ngay in cacNgay.OrderBy(d => d))
        {
            var ma = MaSoGenerator.TaoMa(conn, tx, LoaiMa.LichLamViec);
            using var cmd = conn.Lenh(@"
INSERT INTO Lich_lam_viec (ma_lich_lam_viec, ngay_lam_viec, ma_nv, ma_phong, ma_ca) VALUES (@ma, @ngay, @nv, @phong, @ca)", tx)
                .P("@ma", SqlDbType.VarChar, 10, ma).P("@ngay", SqlDbType.Date, 0, ngay.Date)
                .P("@nv", SqlDbType.VarChar, 10, maNv).P("@phong", SqlDbType.VarChar, 10, maPhong).P("@ca", SqlDbType.VarChar, 10, maCa);
            cmd.ExecuteNonQuery();
            cacMa.Add(ma);
        }
        return KetQuaXepLich.ThanhCong(cacMa);
    });

    /// <summary>
    /// Sửa lịch. Chỉ sửa được lịch chưa qua và chưa phát sinh lịch hẹn hay hồ sơ vãng lai: trigger không kiểm tra lại
    /// RB16, RB19, RB41 cho dữ liệu cũ nên đổi bác sĩ, phòng, ngày, ca sẽ làm sai lịch sử.
    /// </summary>
    public KetQuaXepLich Sua(string ma, DateTime ngay, string maNv, string maPhong, string maCa) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var khoa = KhoaVaKiemTraDuocDoi(conn, tx, ma);
        if (khoa.Loi != LoiXepLich.Khong) return khoa;
        var loi = KiemTraNguonLuc(conn, tx, maNv, maPhong, maCa);
        if (loi != LoiXepLich.Khong) return KetQuaXepLich.LoiDon(loi);
        var trung = TimTrung(conn, tx, ma, [ngay], maNv, maPhong, maCa);
        if (trung.Count > 0) return new KetQuaXepLich { Loi = LoiXepLich.TrungLich, DanhSachTrung = trung };

        using var cmd = conn.Lenh(@"
UPDATE Lich_lam_viec SET ngay_lam_viec = @ngay, ma_nv = @nv, ma_phong = @phong, ma_ca = @ca WHERE ma_lich_lam_viec = @ma", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@ngay", SqlDbType.Date, 0, ngay.Date)
            .P("@nv", SqlDbType.VarChar, 10, maNv).P("@phong", SqlDbType.VarChar, 10, maPhong).P("@ca", SqlDbType.VarChar, 10, maCa);
        cmd.ExecuteNonQuery();
        return KetQuaXepLich.ThanhCong();
    });

    public KetQuaXepLich Xoa(string ma) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var khoa = KhoaVaKiemTraDuocDoi(conn, tx, ma);
        if (khoa.Loi != LoiXepLich.Khong) return khoa;
        using var cmd = conn.Lenh("DELETE FROM Lich_lam_viec WHERE ma_lich_lam_viec = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        cmd.ExecuteNonQuery();
        return KetQuaXepLich.ThanhCong();
    });

    /// <summary>RB10 (phải là bác sĩ), RB11 (phòng thuộc khoa của bác sĩ), phòng và ca tồn tại.</summary>
    private static LoiXepLich KiemTraNguonLuc(SqlConnection conn, SqlTransaction tx, string maNv, string maPhong, string maCa)
    {
        using var c = conn.Lenh(@"
SELECT (SELECT chuc_vu FROM Nhan_vien WHERE ma_nv = @nv),
       (SELECT ma_khoa FROM Nhan_vien WHERE ma_nv = @nv),
       (SELECT ma_khoa FROM Phong_lam_viec WHERE ma_phong = @phong),
       (SELECT COUNT(*) FROM Ca_lam_viec WHERE ma_ca = @ca)", tx)
            .P("@nv", SqlDbType.VarChar, 10, maNv).P("@phong", SqlDbType.VarChar, 10, maPhong).P("@ca", SqlDbType.VarChar, 10, maCa);
        using var r = c.ExecuteReader();
        r.Read();
        var chucVu = r.IsDBNull(0) ? null : r.GetString(0);
        var khoaBs = r.IsDBNull(1) ? null : r.GetString(1);
        var khoaPhong = r.IsDBNull(2) ? null : r.GetString(2);
        if (chucVu is null) return LoiXepLich.KhongTimThayBacSi;
        if (chucVu != ChucVu.BacSi) return LoiXepLich.KhongPhaiBacSi;
        if (khoaPhong is null) return LoiXepLich.KhongTimThayPhong;
        if (r.GetInt32(3) == 0) return LoiXepLich.KhongTimThayCa;
        if (khoaPhong != khoaBs) return LoiXepLich.PhongKhacKhoa;
        return LoiXepLich.Khong;
    }

    /// <summary>RB12 (phòng/ca/ngày) và RB13 (bác sĩ/ca/ngày). Khóa phạm vi (UPDLOCK, HOLDLOCK) để hai máy xếp cùng lúc không lọt.</summary>
    private static List<TrungLich> TimTrung(SqlConnection conn, SqlTransaction tx, string? boQuaMa, List<DateTime> cacNgay,
        string maNv, string maPhong, string maCa)
    {
        using var c = conn.Lenh(@"
SELECT l.ngay_lam_viec, l.ma_nv, n.ho_ten_nv, p.ten_phong
FROM Lich_lam_viec l WITH (UPDLOCK, HOLDLOCK)
JOIN Nhan_vien n ON n.ma_nv = l.ma_nv JOIN Phong_lam_viec p ON p.ma_phong = l.ma_phong
WHERE l.ma_ca = @ca AND l.ngay_lam_viec BETWEEN @tu AND @den AND (l.ma_nv = @nv OR l.ma_phong = @phong)
  AND (@bo IS NULL OR l.ma_lich_lam_viec <> @bo)
ORDER BY l.ngay_lam_viec", tx)
            .P("@ca", SqlDbType.VarChar, 10, maCa).P("@nv", SqlDbType.VarChar, 10, maNv).P("@phong", SqlDbType.VarChar, 10, maPhong)
            .P("@tu", SqlDbType.Date, 0, cacNgay.Min().Date).P("@den", SqlDbType.Date, 0, cacNgay.Max().Date)
            .P("@bo", SqlDbType.VarChar, 10, boQuaMa);
        var chon = cacNgay.Select(d => d.Date).ToHashSet();
        var ds = new List<TrungLich>();
        using var r = c.ExecuteReader();
        while (r.Read())
        {
            var ngay = r.GetDateTime(0);
            if (chon.Contains(ngay)) ds.Add(new TrungLich(ngay, r.GetString(1) == maNv, r.GetString(2), r.GetString(3)));
        }
        return ds;
    }

    /// <summary>Khóa lịch; báo lỗi nếu lịch không có, đã qua, đã có lịch hẹn hoặc hồ sơ vãng lai.</summary>
    private static KetQuaXepLich KhoaVaKiemTraDuocDoi(SqlConnection conn, SqlTransaction tx, string ma)
    {
        using var c = conn.Lenh(@"
SELECT l.ngay_lam_viec,
       (SELECT COUNT(*) FROM Lich_hen h WITH (UPDLOCK, HOLDLOCK) WHERE h.ma_lich_lam_viec = l.ma_lich_lam_viec),
       (SELECT COUNT(*) FROM Ho_so_kham s WITH (UPDLOCK, HOLDLOCK)
         WHERE s.ma_lich_hen IS NULL AND s.ma_nv_kham = l.ma_nv AND CAST(s.ngay_gio_kham AS date) = l.ngay_lam_viec)
FROM Lich_lam_viec l WITH (UPDLOCK) WHERE l.ma_lich_lam_viec = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        using var r = c.ExecuteReader();
        if (!r.Read()) return KetQuaXepLich.LoiDon(LoiXepLich.KhongTimThayLich);
        var ngay = r.GetDateTime(0);
        int soHen = r.GetInt32(1), soVangLai = r.GetInt32(2);
        if (ngay < DateTime.Today) return KetQuaXepLich.LoiDon(LoiXepLich.DaQua, ngay: ngay);
        if (soHen > 0) return KetQuaXepLich.LoiDon(LoiXepLich.DaCoLichHen, soHen);
        if (soVangLai > 0) return KetQuaXepLich.LoiDon(LoiXepLich.DaCoKhachVangLai, soVangLai);
        return KetQuaXepLich.ThanhCong();
    }

    private static string? Rong(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();

    private static List<LichLamViecDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<LichLamViecDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new LichLamViecDTO
            {
                MaLichLamViec = r.GetString(0), NgayLamViec = r.GetDateTime(1), MaCa = r.GetString(2), TenCa = r.GetString(3),
                GioBatDau = r.GetTimeSpan(4), GioKetThuc = r.GetTimeSpan(5), MaNv = r.GetString(6), TenBacSi = r.GetString(7),
                MaKhoa = r.GetString(8), TenKhoa = r.GetString(9), MaPhong = r.GetString(10), TenPhong = r.GetString(11),
                SoLichHen = r.GetInt32(12), SoLichHenConHieuLuc = r.GetInt32(13),
            });
        return ds;
    }
}
