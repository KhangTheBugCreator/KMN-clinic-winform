using KmnClinic.DTO;
using KmnClinic.GUI.Theme;
using KmnClinic.GUI.Views;

namespace KmnClinic.GUI.Forms;

/// <summary>
/// Form chính: menu bên trái theo vai trò, thanh tiêu đề, vùng nội dung thay UserControl.
/// Menu chỉ hiện chức năng của vai trò đang đăng nhập; BUS vẫn kiểm tra quyền lần nữa (từ W0).
/// </summary>
public partial class frmChinh : Form
{
    private sealed record MucMenu(string? Nhom, string Ten, Func<UserControl> Tao);

    private Button? _nutDangChon;

    /// <summary>true khi người dùng bấm Đăng xuất (Program quay lại form đăng nhập).</summary>
    public bool DangXuat { get; private set; }

    public frmChinh()
    {
        InitializeComponent();
        GiaoDien.Nut(btnDangXuat, GiaoDien.KieuNut.Phu);
        picLogo.Image = GiaoDien.Logo();
        var nv = PhienDangNhap.NhanVien!;
        lblNguoiDung.Text = nv.HoTenNv;
        lblVaiTro.Text = nv.TenKhoa is null ? nv.ChucVu : $"{nv.ChucVu} · {nv.TenKhoa}";
        Text = $"KMN Clinic – {nv.ChucVu}";
        DungMenu(MenuTheoVaiTro(nv.ChucVu));
    }

    private static List<MucMenu> MenuTheoVaiTro(string vaiTro) => vaiTro switch
    {
        ChucVu.QuanTriVien =>
        [
            new(null, "Tổng quan", () => new ucTongQuanQuanTri()),
            new("DANH MỤC", "Khoa", () => new ucChuaLam("Khoa", "W1")),
            new(null, "Phòng làm việc", () => new ucChuaLam("Phòng làm việc", "W1")),
            new(null, "Ca làm việc", () => new ucChuaLam("Ca làm việc", "W1")),
            new(null, "Thuốc", () => new ucThuoc()),
            new("NHÂN SỰ", "Nhân viên", () => new ucChuaLam("Nhân viên", "W2")),
            new(null, "Khách hàng", () => new ucChuaLam("Khách hàng", "W2")),
            new(null, "Lịch làm việc", () => new ucLichLamViec(true)),
            new("VẬN HÀNH", "Lịch hẹn", () => new ucChuaLam("Tra cứu lịch hẹn", "W9")),
            new(null, "Hồ sơ khám", () => new ucChuaLam("Tra cứu hồ sơ khám", "W9")),
            new(null, "Hóa đơn", () => new ucChuaLam("Tra cứu hóa đơn", "W9")),
            new(null, "Báo cáo", () => new ucChuaLam("Báo cáo, xuất Excel", "W8")),
        ],
        ChucVu.LeTan =>
        [
            new(null, "Lịch hẹn", () => new ucLichHen()),
            new(null, "Tiếp nhận", () => new ucTiepNhan()),
            new(null, "Khách hàng", () => new ucChuaLam("Khách hàng", "W2")),
        ],
        ChucVu.BacSi =>
        [
            new(null, "Hàng chờ khám", () => new ucHangChoKham()),
            new(null, "Lịch trực của tôi", () => new ucLichLamViec(false)),
            new(null, "Lịch hẹn của tôi", () => new ucChuaLam("Lịch hẹn của tôi", "W6")),
            new(null, "Hồ sơ đã khám", () => new ucChuaLam("Hồ sơ đã khám", "W6")),
        ],
        ChucVu.ThuNgan =>
        [
            new(null, "Hóa đơn chờ thu", () => new ucHoaDonChoThu()),
            new(null, "Lịch sử thanh toán", () => new ucChuaLam("Lịch sử thanh toán", "W7")),
        ],
        _ => []
    };

    private void DungMenu(List<MucMenu> menu)
    {
        flpMenu.SuspendLayout();
        flpMenu.Controls.Clear();
        menu.Add(new("TÀI KHOẢN", "Đổi mật khẩu", () => new ucChuaLam("Đổi mật khẩu", "W0")));
        Button? dauTien = null;
        foreach (var muc in menu)
        {
            if (muc.Nhom is not null)
                flpMenu.Controls.Add(new Label
                {
                    Text = muc.Nhom, AutoSize = false, Size = new Size(212, 34),
                    TextAlign = ContentAlignment.BottomLeft, ForeColor = Mau.Gray,
                    Font = GiaoDien.ChuNho, Margin = new Padding(0, 8, 0, 4), Padding = new Padding(12, 0, 0, 0)
                });
            var nut = new Button
            {
                Text = muc.Ten, Tag = muc, Size = new Size(212, 40), TextAlign = ContentAlignment.MiddleLeft,
                FlatStyle = FlatStyle.Flat, ForeColor = Mau.SidebarChu, BackColor = Mau.SidebarNen,
                Font = new Font("Segoe UI", 10.5F), Padding = new Padding(12, 0, 0, 0), Margin = new Padding(0, 0, 0, 2),
                Cursor = Cursors.Hand
            };
            nut.FlatAppearance.BorderSize = 0;
            nut.FlatAppearance.MouseOverBackColor = Mau.SidebarChon;
            nut.Click += (_, _) => MoTrang(nut);
            flpMenu.Controls.Add(nut);
            dauTien ??= nut;
        }
        flpMenu.ResumeLayout();
        if (dauTien is not null) MoTrang(dauTien);
    }

    private void MoTrang(Button nut)
    {
        if (_nutDangChon is not null)
        {
            _nutDangChon.BackColor = Mau.SidebarNen;
            _nutDangChon.ForeColor = Mau.SidebarChu;
        }
        nut.BackColor = Mau.SidebarChon;
        nut.ForeColor = Color.White;
        _nutDangChon = nut;

        var muc = (MucMenu)nut.Tag!;
        lblTieuDe.Text = muc.Ten;
        var trang = muc.Tao();
        trang.Dock = DockStyle.Fill;
        pnlNoiDung.SuspendLayout();
        var cu = pnlNoiDung.Controls.Cast<Control>().ToArray();
        pnlNoiDung.Controls.Clear();
        pnlNoiDung.Controls.Add(trang);
        pnlNoiDung.ResumeLayout();
        foreach (var c in cu) c.Dispose();
    }

    private void btnDangXuat_Click(object? sender, EventArgs e)
    {
        if (MessageBox.Show("Đăng xuất khỏi phần mềm?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        DangXuat = true;
        Close();
    }
}
