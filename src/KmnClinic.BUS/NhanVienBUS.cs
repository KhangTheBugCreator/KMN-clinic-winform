using KmnClinic.DAL;
using KmnClinic.DTO;

namespace KmnClinic.BUS;

/// <summary>Đăng nhập, đổi mật khẩu (RB35–RB37) và quản lý nhân viên (W2, chỉ Quản trị viên).</summary>
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

    // ------------------------------------------------------------ quản lý nhân viên (W2)

    public List<NhanVienDTO> DanhSach(string? tuKhoa = null, string? chucVu = null, string? maKhoa = null)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        return _dal.DanhSach(tuKhoa, chucVu, maKhoa);
    }

    /// <summary>Thêm nhân viên; mật khẩu bắt buộc (tối thiểu 8 ký tự), lưu dạng băm. Trả về mã mới.</summary>
    public string Them(NhanVienDTO nv, string matKhau)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var hopLe = ChuanHoa(nv);
        KiemTraMatKhauMoi(matKhau);
        return KiemTra.Ghi(() => _dal.Them(hopLe, MatKhau.Bam(matKhau)));
    }

    /// <summary>
    /// Sửa nhân viên. matKhauMoi rỗng = giữ mật khẩu cũ; có nhập thì đặt lại mật khẩu (khi nhân viên quên).
    /// Nhân viên đã có lịch làm việc, lịch hẹn, hồ sơ hoặc hóa đơn thì không đổi chức vụ/khoa.
    /// </summary>
    public void Sua(NhanVienDTO nv, string? matKhauMoi)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var hopLe = ChuanHoa(nv);
        hopLe.MaNv = nv.MaNv;
        string? bam = null;
        if (!string.IsNullOrEmpty(matKhauMoi))
        {
            KiemTraMatKhauMoi(matKhauMoi);
            bam = MatKhau.Bam(matKhauMoi);
        }
        var kq = KiemTra.Ghi(() => _dal.Sua(hopLe, bam, PhienDangNhap.NhanVien!.MaNv));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy nhân viên.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể đổi chức vụ hoặc khoa: nhân viên đã có {kq.SoLuongDangDung} dữ liệu lịch làm việc, lịch hẹn, hồ sơ hoặc hóa đơn theo chức vụ hiện tại.");
            case TrangThaiGhi.TuThaoTacChinhMinh:
                throw new LoiNghiepVu("Bạn không thể tự đổi chức vụ của mình khi đang ở vai trò quản trị viên.");
            case TrangThaiGhi.HetQuanTriVien:
                throw new LoiNghiepVu("Phòng khám phải còn ít nhất một quản trị viên.");
        }
    }

    public void Xoa(string maNv)
    {
        PhanQuyen.YeuCau(ChucVu.QuanTriVien);
        var kq = KiemTra.Ghi(() => _dal.Xoa(maNv, PhienDangNhap.NhanVien!.MaNv));
        switch (kq.TrangThai)
        {
            case TrangThaiGhi.KhongTimThay:
                throw new LoiNghiepVu("Không tìm thấy nhân viên.");
            case TrangThaiGhi.TuThaoTacChinhMinh:
                throw new LoiNghiepVu("Bạn không thể tự xóa chính mình.");
            case TrangThaiGhi.DangDuocDung:
                throw new LoiNghiepVu($"Không thể xóa: nhân viên đã có {kq.SoLuongDangDung} dữ liệu lịch làm việc, lịch hẹn, hồ sơ hoặc hóa đơn cần giữ lại.");
            case TrangThaiGhi.HetQuanTriVien:
                throw new LoiNghiepVu("Phòng khám phải còn ít nhất một quản trị viên.");
        }
    }

    /// <summary>Kiểm tra dữ liệu nhập. RB5: chức vụ hợp lệ. RB6: chỉ Bác sĩ có khoa (CK_Nhan_vien_khoa).</summary>
    private static NhanVienDTO ChuanHoa(NhanVienDTO nv)
    {
        var chucVu = (nv.ChucVu ?? "").Trim();
        if (chucVu.Length == 0) throw new LoiNghiepVu("Vui lòng chọn chức vụ.");
        if (!ChucVu.TatCa.Contains(chucVu)) throw new LoiNghiepVu("Chức vụ chỉ nhận Quản trị viên, Lễ tân, Bác sĩ hoặc Thu ngân.");

        string? maKhoa = null;
        if (chucVu == ChucVu.BacSi)
        {
            if (string.IsNullOrWhiteSpace(nv.MaKhoa)) throw new LoiNghiepVu("Bác sĩ phải thuộc một khoa (RB6).");
            maKhoa = nv.MaKhoa.Trim();
        }

        return new NhanVienDTO
        {
            HoTenNv = KiemTra.BatBuoc(nv.HoTenNv, "họ tên", 50),
            NgaySinhNv = KiemTra.NgaySinh(nv.NgaySinhNv),
            SdtNv = KiemTra.SoDienThoai(nv.SdtNv),
            ChucVu = chucVu,
            MaKhoa = maKhoa,
            DuongDanAnh = KiemTra.DuongDanAnh(nv.DuongDanAnh),
        };
    }

    private static void KiemTraMatKhauMoi(string? matKhau)
    {
        if (string.IsNullOrEmpty(matKhau) || matKhau.Length < DoDaiMatKhauToiThieu)
            throw new LoiNghiepVu($"Mật khẩu tối thiểu {DoDaiMatKhauToiThieu} ký tự.");
        if (matKhau.Length > 100) throw new LoiNghiepVu("Mật khẩu tối đa 100 ký tự.");
    }
}
