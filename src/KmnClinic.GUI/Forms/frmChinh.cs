using KmnClinic.BUS;
using KmnClinic.DTO;
using KmnClinic.GUI.Views;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Form chính: thanh menu trên cùng theo vai trò, thanh trạng thái ở dưới, vùng giữa hiện màn hình nghiệp vụ (UserControl).
/// Menu chỉ có chức năng của vai trò đang đăng nhập; BUS vẫn kiểm tra quyền lần nữa (từ W0).
/// </summary>
public partial class frmChinh : Form
{
    private sealed record MucMenu(string Nhom, string Ten, Func<UserControl> Tao);

    /// <summary>true khi người dùng chọn Đăng xuất (Program quay lại form đăng nhập).</summary>
    public bool DangXuat { get; private set; }

    public frmChinh()
    {
        InitializeComponent();
        var nv = PhienDangNhap.NhanVien!;
        lblNguoiDung.Text = $"Người dùng: {nv.HoTenNv} ({nv.MaNv})";
        lblVaiTro.Text = nv.TenKhoa is null ? $"Vai trò: {nv.ChucVu}" : $"Vai trò: {nv.ChucVu} - {nv.TenKhoa}";
        lblNgay.Text = $"Ngày: {DateTime.Today:dd/MM/yyyy}";
        DungMenu(MenuTheoVaiTro(nv.ChucVu));
    }

    private static List<MucMenu> MenuTheoVaiTro(string vaiTro) => vaiTro switch
    {
        ChucVu.QuanTriVien =>
        [
            new("&Danh mục", "Khoa", () => new ucKhoa()),
            new("&Danh mục", "Phòng làm việc", () => new ucPhong()),
            new("&Danh mục", "Ca làm việc", () => new ucCa()),
            new("&Danh mục", "Thuốc", () => new ucThuoc()),
            new("&Nhân sự", "Nhân viên", () => new ucNhanVien()),
            new("&Nhân sự", "Khách hàng", () => new ucKhachHang()),
            new("&Nhân sự", "Lịch làm việc", () => new ucLichLamViec(true)),
            new("&Tra cứu", "Lịch hẹn", () => new ucChuaLam("Tra cứu lịch hẹn", "W9")),
            new("&Tra cứu", "Hồ sơ khám", () => new ucChuaLam("Tra cứu hồ sơ khám", "W9")),
            new("&Tra cứu", "Hóa đơn", () => new ucChuaLam("Tra cứu hóa đơn", "W9")),
            new("&Báo cáo", "Tổng quan", () => new ucTongQuanQuanTri()),
            new("&Báo cáo", "Báo cáo thống kê", () => new ucChuaLam("Báo cáo, xuất Excel", "W8")),
        ],
        ChucVu.LeTan =>
        [
            new("&Lễ tân", "Tổng quan hôm nay", () => new ucTongQuanLeTan()),
            new("&Lễ tân", "Lịch hẹn", () => new ucLichHen()),
            new("&Lễ tân", "Tiếp nhận khách", () => new ucTiepNhan()),
            new("&Lễ tân", "Khách hàng", () => new ucKhachHang()),
        ],
        ChucVu.BacSi =>
        [
            new("&Khám bệnh", "Hàng chờ khám", () => new ucHangChoKham()),
            new("&Khám bệnh", "Hồ sơ đã khám", () => new ucHoSoCuaToi()),
            new("&Lịch", "Lịch trực của tôi", () => new ucLichLamViec(false)),
            new("&Lịch", "Lịch hẹn của tôi", () => new ucLichHenCuaToi()),
        ],
        ChucVu.ThuNgan =>
        [
            new("&Thu ngân", "Hóa đơn chờ thu", () => new ucHoaDonChoThu()),
            new("&Thu ngân", "Lịch sử thanh toán", () => new ucChuaLam("Lịch sử thanh toán", "W7")),
        ],
        _ => []
    };

    private void DungMenu(List<MucMenu> menu)
    {
        ToolStripMenuItem? dauTien = null;
        foreach (var nhom in menu.GroupBy(m => m.Nhom))
        {
            var mnuNhom = new ToolStripMenuItem(nhom.Key);
            foreach (var muc in nhom)
            {
                var mnu = new ToolStripMenuItem(muc.Ten) { Tag = muc };
                mnu.Click += (_, _) => MoTrang(muc);
                mnuNhom.DropDownItems.Add(mnu);
                dauTien ??= mnu;
            }
            menuChinh.Items.Add(mnuNhom);
        }
        if (dauTien?.Tag is MucMenu dau) MoTrang(dau);
    }

    private void MoTrang(MucMenu muc)
    {
        lblTieuDe.Text = muc.Ten.ToUpper();
        Text = $"KMN Clinic - {muc.Ten}";
        var trang = muc.Tao();
        trang.Dock = DockStyle.Fill;
        var cu = pnlNoiDung.Controls.Cast<Control>().ToArray();
        pnlNoiDung.SuspendLayout();
        pnlNoiDung.Controls.Clear();
        pnlNoiDung.Controls.Add(trang);
        pnlNoiDung.ResumeLayout();
        foreach (var c in cu) c.Dispose();
    }

    private void mnuDoiMatKhau_Click(object? sender, EventArgs e)
    {
        using var f = new frmDoiMatKhau();
        f.ShowDialog(this);
    }

    private void mnuDangXuat_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("Đăng xuất khỏi phần mềm?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        DangXuat = true;
        Close();
    }

    private void mnuThoat_Click(object? sender, EventArgs e) => Close();
}
