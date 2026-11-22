using System.Security.Cryptography;

namespace KmnClinic.BUS;

/// <summary>
/// Băm mật khẩu nhân viên trước khi lưu vào mat_khau_nv (RB35: không lưu bản gốc).
/// Định dạng giống bản web: PBKDF2-SHA256$vòng-lặp$salt-base64$hash-base64 (khoảng 90 ký tự, vừa varchar(255)).
/// </summary>
public static class MatKhau
{
    private const string TienTo = "PBKDF2-SHA256";
    private const int VongLap = 100_000;

    public static string Bam(string matKhau)
    {
        var salt = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(matKhau, salt, VongLap, HashAlgorithmName.SHA256, 32);
        return $"{TienTo}${VongLap}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    /// <summary>Giá trị đã lưu có phải dạng băm không (dữ liệu mẫu cũ lưu bản gốc "matkhau123").</summary>
    public static bool LaDaBam(string daLuu) => daLuu.StartsWith(TienTo + "$", StringComparison.Ordinal);

    /// <summary>
    /// So mật khẩu nhập với giá trị đã lưu.
    /// Giá trị đã băm: so bằng PBKDF2. Giá trị cũ chưa băm: so trực tiếp (thời gian hằng),
    /// rồi NhanVienBUS sẽ băm lại ngay sau khi đăng nhập đúng.
    /// </summary>
    public static bool KiemTra(string matKhau, string daLuu)
    {
        if (!LaDaBam(daLuu))
            return CryptographicOperations.FixedTimeEquals(
                System.Text.Encoding.UTF8.GetBytes(matKhau), System.Text.Encoding.UTF8.GetBytes(daLuu));

        var p = daLuu.Split('$');
        if (p.Length != 4 || !int.TryParse(p[1], out var vong)) return false;
        try
        {
            var hash = Rfc2898DeriveBytes.Pbkdf2(matKhau, Convert.FromBase64String(p[2]), vong, HashAlgorithmName.SHA256, 32);
            return CryptographicOperations.FixedTimeEquals(hash, Convert.FromBase64String(p[3]));
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
