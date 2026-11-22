using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Đăng nhập và đổi mật khẩu (RB35–RB37). Quản lý nhân viên thêm ở W2.</summary>
public class NhanVienBUS
{
    public const int DoDaiMatKhauToiThieu = 8;
    private const string SaiThongTin = "Số điện thoại hoặc mật khẩu không đúng.";

    private readonly NhanVienDAL _dal = new();

    /// <summary>
    /// Đăng nhập bằng số điện thoại + mật khẩu. Đúng thì lưu vào PhienDangNhap và trả về nhân viên.
    /// Dữ liệu mẫu lưu mật khẩu bản gốc: đăng nhập đúng lần đầu sẽ băm lại và ghi đè (RB35).
    /// Không nói rõ sai số điện thoại hay sai mật khẩu, để người lạ không dò được số nào có tài khoản.
    /// </summary>
    public NhanVienDTO DangNhap(string sdt, string matKhau)
    {
        sdt = (sdt ?? "").Trim();
        matKhau ??= "";
        if (sdt.Length == 0) throw new LoiNghiepVu("Vui lòng nhập số điện thoại.");
        if (matKhau.Length < DoDaiMatKhauToiThieu)
            throw new LoiNghiepVu($"Mật khẩu tối thiểu {DoDaiMatKhauToiThieu} ký tự.");

        var tim = _dal.TimTheoSdt(sdt);
        if (tim is null || !MatKhau.KiemTra(matKhau, tim.Value.MatKhauDaLuu))
            throw new LoiNghiepVu(SaiThongTin);

        var nv = tim.Value.NhanVien;
        if (!MatKhau.LaDaBam(tim.Value.MatKhauDaLuu))
            _dal.CapNhatMatKhau(nv.MaNv, MatKhau.Bam(matKhau));

        PhienDangNhap.DangNhap(nv);
        return nv;
    }

    /// <summary>Nhân viên đang đăng nhập đổi mật khẩu của chính mình.</summary>
    public void DoiMatKhau(string matKhauCu, string matKhauMoi, string nhapLai)
    {
        var nv = PhienDangNhap.NhanVien ?? throw new LoiNghiepVu("Phiên đăng nhập đã kết thúc. Vui lòng đăng nhập lại.");
        matKhauCu ??= ""; matKhauMoi ??= ""; nhapLai ??= "";

        if (matKhauCu.Length == 0) throw new LoiNghiepVu("Vui lòng nhập mật khẩu hiện tại.");
        if (matKhauMoi.Length < DoDaiMatKhauToiThieu)
            throw new LoiNghiepVu($"Mật khẩu mới tối thiểu {DoDaiMatKhauToiThieu} ký tự.");
        if (matKhauMoi != nhapLai) throw new LoiNghiepVu("Nhập lại mật khẩu mới không khớp.");
        if (matKhauMoi == matKhauCu) throw new LoiNghiepVu("Mật khẩu mới phải khác mật khẩu hiện tại.");

        var daLuu = _dal.LayMatKhau(nv.MaNv) ?? throw new LoiNghiepVu("Không tìm thấy tài khoản. Vui lòng đăng nhập lại.");
        if (!MatKhau.KiemTra(matKhauCu, daLuu)) throw new LoiNghiepVu("Mật khẩu hiện tại không đúng.");

        _dal.CapNhatMatKhau(nv.MaNv, MatKhau.Bam(matKhauMoi));
    }
}
