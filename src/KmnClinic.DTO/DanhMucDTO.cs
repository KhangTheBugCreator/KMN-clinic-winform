namespace KmnClinic.DTO;

/// <summary>Bảng Khoa.</summary>
public class KhoaDTO
{
    public string MaKhoa { get; set; } = "";
    public string TenKhoa { get; set; } = "";
    public string? MoTa { get; set; }
    public override string ToString() => TenKhoa;
}

/// <summary>Bảng Phong_lam_viec.</summary>
public class PhongDTO
{
    public string MaPhong { get; set; } = "";
    public string TenPhong { get; set; } = "";
    public string MaKhoa { get; set; } = "";
    public string TenKhoa { get; set; } = "";
    public override string ToString() => TenPhong;
}

/// <summary>Bảng Ca_lam_viec.</summary>
public class CaDTO
{
    public string MaCa { get; set; } = "";
    public string TenCa { get; set; } = "";
    public TimeSpan GioBatDau { get; set; }
    public TimeSpan GioKetThuc { get; set; }
    public string KhungGio => $"{GioBatDau:hh\\:mm} – {GioKetThuc:hh\\:mm}";
    public override string ToString() => $"{TenCa} ({KhungGio})";
}

/// <summary>Bảng Thuoc.</summary>
public class ThuocDTO
{
    public string MaThuoc { get; set; } = "";
    public string TenThuoc { get; set; } = "";
    public string? Nsx { get; set; }
    public decimal? HamLuong { get; set; }
    public string DonViTinh { get; set; } = "";
    public decimal DonGiaThuoc { get; set; }
    public int SoLuongTon { get; set; }
    public string? MoTa { get; set; }
    public override string ToString() => TenThuoc;
}
