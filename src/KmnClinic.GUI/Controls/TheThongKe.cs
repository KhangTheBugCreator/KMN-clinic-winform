using System.ComponentModel;
using System.Drawing.Drawing2D;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Controls;

/// <summary>Thẻ số liệu (giống thẻ thống kê ở dashboard web): tiêu đề nhỏ, con số lớn, ghi chú.</summary>
public class TheThongKe : Control
{
    private string _tieuDe = "Tiêu đề";
    private string _giaTri = "0";
    private string _ghiChu = "";
    private Color _mauNhan = Mau.Blue;

    public TheThongKe()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.SupportsTransparentBackColor, true);
        BackColor = Color.Transparent;
        Size = new Size(240, 112);
        Margin = new Padding(0, 0, 16, 0);
    }

    [Category("KMN"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string TieuDe { get => _tieuDe; set { _tieuDe = value; Invalidate(); } }

    [Category("KMN"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string GiaTri { get => _giaTri; set { _giaTri = value; Invalidate(); } }

    [Category("KMN"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string GhiChu { get => _ghiChu; set { _ghiChu = value; Invalidate(); } }

    [Category("KMN"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public Color MauNhan { get => _mauNhan; set { _mauNhan = value; Invalidate(); } }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new Rectangle(0, 0, Width - 1, Height - 1);
        using (var p = GiaoDien.VienBo(r, 14))
        {
            using var nen = new SolidBrush(Color.White);
            g.FillPath(nen, p);
            using var vien = new Pen(Mau.Border);
            g.DrawPath(vien, p);
        }
        // Dải màu nhấn bên trái
        using (var nhan = new SolidBrush(_mauNhan))
            g.FillRectangle(nhan, 0, 18, 4, Height - 36);

        TextRenderer.DrawText(g, _tieuDe, GiaoDien.ChuNho, new Rectangle(20, 14, Width - 30, 20), Mau.Muted, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        using var so = new Font("Segoe UI Semibold", 20F);
        TextRenderer.DrawText(g, _giaTri, so, new Rectangle(18, 36, Width - 30, 40), Mau.Navy, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        TextRenderer.DrawText(g, _ghiChu, GiaoDien.ChuNho, new Rectangle(20, 80, Width - 30, 20), _mauNhan, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }
}
