using System.Data;
using KmnClinic.DTO;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

public enum LoiKhamBenh
{
    Khong,
    KhongTimThayHoSo,   // không có hoặc của bác sĩ khác
    DaHoanTat,          // đã có hóa đơn, hồ sơ bị khóa (Q4)
    KhongTimThayThuoc,
    DaCoTrongDon,
    KhongCoTrongDon,
    ThieuTon,           // RB27
    ChuaChanDoan,       // RB22
    TonKhongDu,         // lúc hoàn tất, tồn đã giảm so với lúc kê
}

public sealed class KetQuaKhamBenh
{
    public LoiKhamBenh Loi { get; init; }
    public string? TenThuoc { get; init; }
    public int Ton { get; init; }
    public int SoLuong { get; init; }
    public List<ChiTietDonThuocDTO> ThuocThieu { get; init; } = [];
    public string? MaHoaDon { get; init; }
    public static readonly KetQuaKhamBenh ThanhCong = new();
}

/// <summary>
/// Bác sĩ khám: hồ sơ của chính bác sĩ, kê đơn, hoàn tất lập hóa đơn. Mọi thao tác ghi khóa dòng hồ sơ trước (UPDLOCK)
/// để không chen với Hoàn tất. Trigger TG_CTDT_KeDon tự lấy đơn giá lúc kê (RB28), TG_Hoa_don_KiemTra chặn RB22, tự tính tổng.
/// </summary>
public class KhamBenhDAL
{
    private const string SqlHoSo = $@"
SELECT hs.ma_ho_so, hs.ngay_gio_kham, hs.trieu_chung, hs.chan_doan_benh, hs.loi_dan, hs.tien_cong_kham, hs.ma_lich_hen,
       k.ma_kh, k.ho_ten_kh, k.gioi_tinh_kh, k.ngay_sinh_kh, k.sdt_kh, bs.ma_nv, bs.ho_ten_nv, kh.ten_khoa, lt.ho_ten_nv,
       {TiepNhanDAL.BieuThucTrangThai}, d.ma_hoa_don, d.tong_tien
FROM Ho_so_kham hs
JOIN Khach_hang k ON k.ma_kh = hs.ma_kh
JOIN Nhan_vien bs ON bs.ma_nv = hs.ma_nv_kham
JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
JOIN Nhan_vien lt ON lt.ma_nv = hs.ma_nv_tiep_nhan
LEFT JOIN Hoa_don d ON d.ma_ho_so = hs.ma_ho_so";

    /// <summary>Hàng chờ: mọi hồ sơ của bác sĩ chưa có hóa đơn (kể cả ngày trước còn sót). Đang khám lên trước, rồi theo giờ tiếp nhận.</summary>
    public List<HoSoKhamDTO> HangCho(string maBs)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlHoSo + $@"
WHERE hs.ma_nv_kham = @bs AND d.ma_hoa_don IS NULL
ORDER BY CASE {TiepNhanDAL.BieuThucTrangThai} WHEN N'Đang khám' THEN 0 ELSE 1 END, hs.ngay_gio_kham, hs.ma_ho_so")
            .P("@bs", SqlDbType.VarChar, 10, maBs);
        return Doc(cmd);
    }

    /// <summary>Hồ sơ của bác sĩ có lọc (null = không lọc), mới nhất trước.</summary>
    public List<HoSoKhamDTO> DanhSach(string maBs, DateTime? tu, DateTime? den, string? trangThai, string? tuKhoa)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlHoSo + $@"
WHERE hs.ma_nv_kham = @bs
  AND (@tu IS NULL OR hs.ngay_gio_kham >= @tu) AND (@den IS NULL OR hs.ngay_gio_kham < DATEADD(day, 1, @den))
  AND (@q IS NULL OR hs.ma_ho_so LIKE @q OR k.ho_ten_kh LIKE @q OR k.ma_kh LIKE @q OR k.sdt_kh LIKE @q OR hs.chan_doan_benh LIKE @q)
  AND (@tt IS NULL OR {TiepNhanDAL.BieuThucTrangThai} = @tt)
ORDER BY hs.ngay_gio_kham DESC, hs.ma_ho_so DESC")
            .P("@bs", SqlDbType.VarChar, 10, maBs)
            .P("@tu", SqlDbType.DateTime, 0, tu?.Date).P("@den", SqlDbType.DateTime, 0, den?.Date)
            .P("@q", SqlDbType.NVarChar, 60, TimKiem.MauLike(tuKhoa)).P("@tt", SqlDbType.NVarChar, 20, string.IsNullOrWhiteSpace(trangThai) ? null : trangThai);
        return Doc(cmd);
    }

    /// <summary>Một hồ sơ; maBs khác null thì chỉ lấy hồ sơ của bác sĩ đó (null nếu không có hoặc của bác sĩ khác).</summary>
    public HoSoKhamDTO? Tim(string ma, string? maBs)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh(SqlHoSo + " WHERE hs.ma_ho_so = @ma AND (@bs IS NULL OR hs.ma_nv_kham = @bs)")
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@bs", SqlDbType.VarChar, 10, maBs);
        return Doc(cmd).FirstOrDefault();
    }

    private static List<HoSoKhamDTO> Doc(SqlCommand cmd)
    {
        var ds = new List<HoSoKhamDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
        {
            var gioKham = r.GetDateTime(1);
            var ngaySinh = r.GetDateTime(10);
            var tuoi = gioKham.Year - ngaySinh.Year - (gioKham.Date < ngaySinh.Date.AddYears(gioKham.Year - ngaySinh.Year) ? 1 : 0);
            ds.Add(new HoSoKhamDTO
            {
                MaHoSo = r.GetString(0), NgayGioKham = gioKham, TrieuChung = r.GetString(2),
                ChanDoanBenh = DbHelper.ChuoiHoacNull(r, 3), LoiDan = DbHelper.ChuoiHoacNull(r, 4), TienCongKham = r.GetDecimal(5),
                MaLichHen = DbHelper.ChuoiHoacNull(r, 6), MaKh = r.GetString(7), TenKhachHang = r.GetString(8),
                GioiTinh = DbHelper.ChuoiHoacNull(r, 9) ?? "Khác", Tuoi = tuoi, SdtKhachHang = r.GetString(11),
                MaBacSi = r.GetString(12), TenBacSi = r.GetString(13), TenKhoa = r.GetString(14), TenLeTan = r.GetString(15),
                TrangThai = r.GetString(16), MaHoaDon = DbHelper.ChuoiHoacNull(r, 17), TongTien = r.IsDBNull(18) ? null : r.GetDecimal(18),
            });
        }
        return ds;
    }

    public List<ChiTietDonThuocDTO> DonThuoc(string maHoSo)
    {
        using var conn = DbHelper.MoKetNoi();
        return DonThuoc(conn, null, maHoSo);
    }

    private static List<ChiTietDonThuocDTO> DonThuoc(SqlConnection conn, SqlTransaction? tx, string maHoSo)
    {
        using var cmd = conn.Lenh(@"
SELECT ct.ma_thuoc, t.ten_thuoc, t.ham_luong, t.don_vi_tinh, ct.so_luong_thuoc, ct.lieu_dung, ct.don_gia_thuoc, t.so_luong_ton
FROM Chi_tiet_don_thuoc ct JOIN Thuoc t ON t.ma_thuoc = ct.ma_thuoc
WHERE ct.ma_ho_so = @ma ORDER BY t.ten_thuoc", tx).P("@ma", SqlDbType.VarChar, 10, maHoSo);
        var ds = new List<ChiTietDonThuocDTO>();
        using var r = cmd.ExecuteReader();
        while (r.Read())
            ds.Add(new ChiTietDonThuocDTO
            {
                MaThuoc = r.GetString(0), TenThuoc = r.GetString(1), HamLuong = r.IsDBNull(2) ? null : r.GetDecimal(2),
                DonViTinh = r.GetString(3), SoLuongThuoc = r.GetInt32(4), LieuDung = DbHelper.ChuoiHoacNull(r, 5),
                DonGiaThuoc = r.GetDecimal(6), SoLuongTon = r.GetInt32(7),
            });
        return ds;
    }

    /// <summary>Các lần khám đã hoàn tất trước hồ sơ này của cùng bệnh nhân (mọi bác sĩ, tối đa 20). Null nếu hồ sơ không phải của bác sĩ.</summary>
    public List<LanKhamTruocDTO>? LanKhamTruoc(string ma, string maBs)
    {
        using var conn = DbHelper.MoKetNoi();
        string maKh; DateTime moc;
        using (var c = conn.Lenh("SELECT ma_kh, ngay_gio_kham FROM Ho_so_kham WHERE ma_ho_so = @ma AND ma_nv_kham = @bs")
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@bs", SqlDbType.VarChar, 10, maBs))
        using (var r = c.ExecuteReader())
        {
            if (!r.Read()) return null;
            maKh = r.GetString(0); moc = r.GetDateTime(1);
        }
        var ds = new List<LanKhamTruocDTO>();
        using (var cmd = conn.Lenh(@"
SELECT TOP 20 hs.ma_ho_so, hs.ngay_gio_kham, bs.ho_ten_nv, kh.ten_khoa, hs.trieu_chung, hs.chan_doan_benh, hs.loi_dan
FROM Ho_so_kham hs
JOIN Nhan_vien bs ON bs.ma_nv = hs.ma_nv_kham
JOIN Khoa kh ON kh.ma_khoa = bs.ma_khoa
WHERE hs.ma_kh = @kh AND hs.ma_ho_so <> @ma AND hs.ngay_gio_kham <= @moc
  AND EXISTS (SELECT 1 FROM Hoa_don d WHERE d.ma_ho_so = hs.ma_ho_so)
ORDER BY hs.ngay_gio_kham DESC").P("@kh", SqlDbType.VarChar, 10, maKh).P("@ma", SqlDbType.VarChar, 10, ma).P("@moc", SqlDbType.DateTime, 0, moc))
        using (var r = cmd.ExecuteReader())
            while (r.Read())
                ds.Add(new LanKhamTruocDTO
                {
                    MaHoSo = r.GetString(0), NgayGioKham = r.GetDateTime(1), TenBacSi = r.GetString(2), TenKhoa = r.GetString(3),
                    TrieuChung = r.GetString(4), ChanDoanBenh = DbHelper.ChuoiHoacNull(r, 5), LoiDan = DbHelper.ChuoiHoacNull(r, 6),
                });
        foreach (var lan in ds)
            lan.Thuoc = DonThuoc(conn, null, lan.MaHoSo)
                .Select(t => $"{t.TenThuoc} {t.HamLuong:0.##} × {t.SoLuongThuoc} {t.DonViTinh.ToLowerInvariant()} ({t.LieuDung})").ToList();
        return ds;
    }

    public TongQuanBacSiDTO TongQuan(string maBs, DateTime bayGio)
    {
        using var conn = DbHelper.MoKetNoi();
        using var cmd = conn.Lenh($@"
SELECT
  ISNULL(SUM(CASE WHEN x.tt = N'Chờ khám' THEN 1 ELSE 0 END), 0),
  ISNULL(SUM(CASE WHEN x.tt = N'Đang khám' THEN 1 ELSE 0 END), 0),
  ISNULL(SUM(CASE WHEN x.tt IN (N'Hoàn tất', N'Đã thanh toán') AND x.ngay = @homNay THEN 1 ELSE 0 END), 0)
FROM (SELECT {TiepNhanDAL.BieuThucTrangThai} AS tt, CAST(hs.ngay_gio_kham AS date) AS ngay
      FROM Ho_so_kham hs LEFT JOIN Hoa_don d ON d.ma_ho_so = hs.ma_ho_so
      WHERE hs.ma_nv_kham = @bs AND (d.ma_hoa_don IS NULL OR hs.ngay_gio_kham >= @homNay)) x")
            .P("@bs", SqlDbType.VarChar, 10, maBs).P("@homNay", SqlDbType.Date, 0, bayGio.Date);
        using var r = cmd.ExecuteReader();
        r.Read();
        return new TongQuanBacSiDTO { ChoKham = r.GetInt32(0), DangKham = r.GetInt32(1), HoanTatHomNay = r.GetInt32(2) };
    }

    /* ---------- Ghi ---------- */

    /// <summary>Khóa hồ sơ của bác sĩ; trả lỗi nếu không có, của bác sĩ khác hoặc đã hoàn tất.</summary>
    private static LoiKhamBenh KhoaHoSo(SqlConnection conn, SqlTransaction tx, string ma, string maBs)
    {
        using var c = conn.Lenh(@"
SELECT hs.ma_nv_kham, (SELECT COUNT(*) FROM Hoa_don d WHERE d.ma_ho_so = hs.ma_ho_so)
FROM Ho_so_kham hs WITH (UPDLOCK, ROWLOCK) WHERE hs.ma_ho_so = @ma", tx).P("@ma", SqlDbType.VarChar, 10, ma);
        using var r = c.ExecuteReader();
        if (!r.Read() || r.GetString(0) != maBs) return LoiKhamBenh.KhongTimThayHoSo;
        return r.GetInt32(1) > 0 ? LoiKhamBenh.DaHoanTat : LoiKhamBenh.Khong;
    }

    private static void LuuThongTin(SqlConnection conn, SqlTransaction tx, string ma, string trieuChung, string? chanDoan, string? loiDan, decimal tienCong)
    {
        using var c = conn.Lenh(@"
UPDATE Ho_so_kham SET trieu_chung = @tc, chan_doan_benh = @cd, loi_dan = @ld, tien_cong_kham = @tien WHERE ma_ho_so = @ma", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@tc", SqlDbType.NVarChar, 255, trieuChung)
            .P("@cd", SqlDbType.NVarChar, 255, chanDoan).P("@ld", SqlDbType.NVarChar, 255, loiDan)
            .P("@tien", SqlDbType.Decimal, 0, tienCong);
        c.Parameters["@tien"].Precision = 12;
        c.ExecuteNonQuery();
    }

    /// <summary>Lưu nháp: chẩn đoán được để trống.</summary>
    public KetQuaKhamBenh Luu(string ma, string maBs, string trieuChung, string? chanDoan, string? loiDan, decimal tienCong)
        => DbHelper.TrongGiaoDich((conn, tx) =>
        {
            var loi = KhoaHoSo(conn, tx, ma, maBs);
            if (loi != LoiKhamBenh.Khong) return new KetQuaKhamBenh { Loi = loi };
            LuuThongTin(conn, tx, ma, trieuChung, chanDoan, loiDan, tienCong);
            return KetQuaKhamBenh.ThanhCong;
        });

    private static (string Ten, int Ton)? DocThuoc(SqlConnection conn, SqlTransaction tx, string maThuoc)
    {
        using var c = conn.Lenh("SELECT ten_thuoc, so_luong_ton FROM Thuoc WHERE ma_thuoc = @t", tx).P("@t", SqlDbType.VarChar, 10, maThuoc);
        using var r = c.ExecuteReader();
        return r.Read() ? (r.GetString(0), r.GetInt32(1)) : null;
    }

    /// <summary>Kê thêm một thuốc, không vượt tồn (RB27). Đơn giá do trigger lấy từ danh mục lúc kê (RB28).</summary>
    public KetQuaKhamBenh KeThuoc(string ma, string maBs, string maThuoc, int soLuong, string lieuDung) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var loi = KhoaHoSo(conn, tx, ma, maBs);
        if (loi != LoiKhamBenh.Khong) return new KetQuaKhamBenh { Loi = loi };
        if (DocThuoc(conn, tx, maThuoc) is not { } thuoc) return new KetQuaKhamBenh { Loi = LoiKhamBenh.KhongTimThayThuoc };
        var (ten, ton) = thuoc;
        using (var c = conn.Lenh("SELECT COUNT(*) FROM Chi_tiet_don_thuoc WHERE ma_ho_so = @ma AND ma_thuoc = @t", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@t", SqlDbType.VarChar, 10, maThuoc))
            if ((int)c.ExecuteScalar()! > 0) return new KetQuaKhamBenh { Loi = LoiKhamBenh.DaCoTrongDon, TenThuoc = ten };
        if (soLuong > ton) return new KetQuaKhamBenh { Loi = LoiKhamBenh.ThieuTon, TenThuoc = ten, Ton = ton, SoLuong = soLuong };
        // don_gia_thuoc chỉ để đủ cột: trigger INSTEAD OF lấy giá từ Thuoc
        using (var c = conn.Lenh(@"
INSERT INTO Chi_tiet_don_thuoc (ma_ho_so, ma_thuoc, so_luong_thuoc, lieu_dung, don_gia_thuoc)
SELECT @ma, ma_thuoc, @sl, @ld, don_gia_thuoc FROM Thuoc WHERE ma_thuoc = @t", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@t", SqlDbType.VarChar, 10, maThuoc)
            .P("@sl", SqlDbType.Int, 0, soLuong).P("@ld", SqlDbType.NVarChar, 255, lieuDung))
            c.ExecuteNonQuery();
        return KetQuaKhamBenh.ThanhCong;
    });

    /// <summary>Sửa số lượng, liều dùng. Đơn giá giữ nguyên như lúc kê (RB28).</summary>
    public KetQuaKhamBenh SuaThuoc(string ma, string maBs, string maThuoc, int soLuong, string lieuDung) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var loi = KhoaHoSo(conn, tx, ma, maBs);
        if (loi != LoiKhamBenh.Khong) return new KetQuaKhamBenh { Loi = loi };
        if (DocThuoc(conn, tx, maThuoc) is not { } thuoc) return new KetQuaKhamBenh { Loi = LoiKhamBenh.KhongTimThayThuoc };
        var (ten, ton) = thuoc;
        if (soLuong > ton) return new KetQuaKhamBenh { Loi = LoiKhamBenh.ThieuTon, TenThuoc = ten, Ton = ton, SoLuong = soLuong };
        using var c = conn.Lenh("UPDATE Chi_tiet_don_thuoc SET so_luong_thuoc = @sl, lieu_dung = @ld WHERE ma_ho_so = @ma AND ma_thuoc = @t", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@t", SqlDbType.VarChar, 10, maThuoc)
            .P("@sl", SqlDbType.Int, 0, soLuong).P("@ld", SqlDbType.NVarChar, 255, lieuDung);
        return c.ExecuteNonQuery() == 0 ? new KetQuaKhamBenh { Loi = LoiKhamBenh.KhongCoTrongDon, TenThuoc = ten } : KetQuaKhamBenh.ThanhCong;
    });

    public KetQuaKhamBenh XoaThuoc(string ma, string maBs, string maThuoc) => DbHelper.TrongGiaoDich((conn, tx) =>
    {
        var loi = KhoaHoSo(conn, tx, ma, maBs);
        if (loi != LoiKhamBenh.Khong) return new KetQuaKhamBenh { Loi = loi };
        using var c = conn.Lenh("DELETE FROM Chi_tiet_don_thuoc WHERE ma_ho_so = @ma AND ma_thuoc = @t", tx)
            .P("@ma", SqlDbType.VarChar, 10, ma).P("@t", SqlDbType.VarChar, 10, maThuoc);
        return c.ExecuteNonQuery() == 0 ? new KetQuaKhamBenh { Loi = LoiKhamBenh.KhongCoTrongDon } : KetQuaKhamBenh.ThanhCong;
    });

    /// <summary>
    /// Hoàn tất khám (Q4): lưu thông tin cuối, bắt buộc chẩn đoán (RB22), kiểm tra tồn kho hiện tại của từng thuốc,
    /// lập hóa đơn "Chưa thanh toán" (chưa có thu ngân). Trigger tự tính tổng tiền. Sau bước này hồ sơ bị khóa.
    /// </summary>
    public KetQuaKhamBenh HoanTat(string ma, string maBs, string trieuChung, string? chanDoan, string? loiDan, decimal tienCong)
        => DbHelper.TrongGiaoDich((conn, tx) =>
        {
            var loi = KhoaHoSo(conn, tx, ma, maBs);
            if (loi != LoiKhamBenh.Khong) return new KetQuaKhamBenh { Loi = loi };
            if (chanDoan is null) return new KetQuaKhamBenh { Loi = LoiKhamBenh.ChuaChanDoan };
            var thieu = DonThuoc(conn, tx, ma).Where(t => t.SoLuongThuoc > t.SoLuongTon).ToList();
            if (thieu.Count > 0) return new KetQuaKhamBenh { Loi = LoiKhamBenh.TonKhongDu, ThuocThieu = thieu };
            LuuThongTin(conn, tx, ma, trieuChung, chanDoan, loiDan, tienCong);
            var maHd = MaSoGenerator.TaoMa(conn, tx, LoaiMa.HoaDon);
            using (var c = conn.Lenh(@"
INSERT INTO Hoa_don (ma_hoa_don, tong_tien, ngay_gio_thanh_toan, trang_thai_thanh_toan, ma_ho_so, ma_nv)
VALUES (@hd, 0, NULL, N'Chưa thanh toán', @ma, NULL)", tx).P("@hd", SqlDbType.VarChar, 10, maHd).P("@ma", SqlDbType.VarChar, 10, ma))
                c.ExecuteNonQuery();
            return new KetQuaKhamBenh { MaHoaDon = maHd };
        });
}
