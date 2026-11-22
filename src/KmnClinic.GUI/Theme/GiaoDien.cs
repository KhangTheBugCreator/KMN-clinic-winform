using System.Drawing.Drawing2D;

namespace KmnClinic.GUI.Theme;

/// <summary>
/// Các hàm định dạng dùng chung để mọi màn hình cùng một kiểu.
/// Gọi trong constructor của form/UserControl, sau InitializeComponent().
/// </summary>
public static class GiaoDien
{
    public static readonly Font ChuTieuDe = new("Segoe UI Semibold", 15F);
    public static readonly Font ChuTieuDeNho = new("Segoe UI Semibold", 11.5F);
    public static readonly Font ChuDam = new("Segoe UI Semibold", 10F);
    public static readonly Font ChuNho = new("Segoe UI", 9F);

    public enum KieuNut { Chinh, Phu, Nguy, ThanhCong }

    /// <summary>Định dạng nút phẳng theo màu thương hiệu.</summary>
    public static void Nut(Button nut, KieuNut kieu = KieuNut.Chinh)
    {
        nut.FlatStyle = FlatStyle.Flat;
        nut.FlatAppearance.BorderSize = kieu == KieuNut.Phu ? 1 : 0;
        nut.Cursor = Cursors.Hand;
        nut.Font = ChuDam;
        nut.UseVisualStyleBackColor = false;
        (nut.BackColor, nut.ForeColor, nut.FlatAppearance.BorderColor) = kieu switch
        {
            KieuNut.Chinh => (Mau.Blue, Color.White, Mau.Blue),
            KieuNut.Nguy => (Mau.Red, Color.White, Mau.Red),
            KieuNut.ThanhCong => (Mau.Green, Color.White, Mau.Green),
            _ => (Color.White, Mau.Navy, Mau.Border)
        };
        nut.FlatAppearance.MouseOverBackColor = kieu == KieuNut.Phu ? Mau.Sky : ControlPaint.Dark(nut.BackColor, 0.05f);
    }

    /// <summary>Định dạng DataGridView: chỉ đọc, chọn cả dòng, tiêu đề màu nhạt, dòng cao vừa phải.</summary>
    public static void Luoi(DataGridView g)
    {
        g.BackgroundColor = Color.White;
        g.BorderStyle = BorderStyle.None;
        g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        g.GridColor = Mau.Border;
        g.RowHeadersVisible = false;
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.AllowUserToResizeRows = false;
        g.ReadOnly = true;
        g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        g.EnableHeadersVisualStyles = false;
        g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        g.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
        g.ColumnHeadersHeight = 40;
        g.ColumnHeadersDefaultCellStyle.BackColor = Mau.Sky2;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Mau.Muted;
        g.ColumnHeadersDefaultCellStyle.Font = ChuDam;
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Mau.Sky2;
        g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        g.DefaultCellStyle.ForeColor = Mau.Text;
        g.DefaultCellStyle.SelectionBackColor = Mau.Sky;
        g.DefaultCellStyle.SelectionForeColor = Mau.Navy;
        g.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);
        g.RowTemplate.Height = 38;
        g.AutoGenerateColumns = false;
    }

    /// <summary>Vẽ ô trạng thái thành "badge" bo tròn màu theo trạng thái.</summary>
    public static void BadgeTrangThai(DataGridView g, string tenCot)
    {
        g.CellPainting += (s, e) =>
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || g.Columns[e.ColumnIndex].Name != tenCot || e.Graphics is null) return;
            e.PaintBackground(e.CellBounds, true);
            var text = e.FormattedValue?.ToString() ?? "";
            if (text.Length == 0) { e.Handled = true; return; }
            var mau = Mau.TheoTrangThai(text);
            var font = ChuNho;
            var kt = TextRenderer.MeasureText(e.Graphics, text, font);
            var rect = new Rectangle(e.CellBounds.X + 8, e.CellBounds.Y + (e.CellBounds.Height - 24) / 2, kt.Width + 14, 24);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            using (var path = VienBo(rect, 12))
            using (var nen = new SolidBrush(Color.FromArgb(38, mau)))
                e.Graphics.FillPath(nen, path);
            TextRenderer.DrawText(e.Graphics, text, font, rect, ControlPaint.Dark(mau, 0.1f),
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            e.Handled = true;
        };
    }

    /// <summary>Hiển thị số tiền dạng 150.000 đ.</summary>
    public static string Tien(decimal soTien) => soTien.ToString("#,0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " đ";

    public static GraphicsPath VienBo(Rectangle r, int banKinh)
    {
        var d = banKinh * 2;
        var p = new GraphicsPath();
        p.AddArc(r.X, r.Y, d, d, 180, 90);
        p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
        p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
        p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
        p.CloseFigure();
        return p;
    }

    /// <summary>Logo KMN Clinic (nhúng trong file .exe).</summary>
    public static Image? Logo()
    {
        var s = typeof(GiaoDien).Assembly.GetManifestResourceStream("KmnClinic.logo.png");
        return s is null ? null : Image.FromStream(s);
    }
}
