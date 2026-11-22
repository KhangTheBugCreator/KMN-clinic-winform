using System.ComponentModel;

namespace KmnClinic.GUI.Controls;

/// <summary>Ô số liệu đơn giản: khung viền, tiêu đề, con số, ghi chú.</summary>
public class TheThongKe : Control
{
    private string _tieuDe = "Tiêu đề";
    private string _giaTri = "0";
    private string _ghiChu = "";

    public TheThongKe()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = SystemColors.Window;
        Size = new Size(200, 80);
        Margin = new Padding(0, 0, 8, 0);
    }

    [Category("KMN"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string TieuDe { get => _tieuDe; set { _tieuDe = value; Invalidate(); } }

    [Category("KMN"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string GiaTri { get => _giaTri; set { _giaTri = value; Invalidate(); } }

    [Category("KMN"), DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
    public string GhiChu { get => _ghiChu; set { _ghiChu = value; Invalidate(); } }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        g.Clear(BackColor);
        ControlPaint.DrawBorder3D(g, ClientRectangle, Border3DStyle.SunkenOuter);
        TextRenderer.DrawText(g, _tieuDe, Font, new Rectangle(8, 6, Width - 16, 18), SystemColors.GrayText, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        using var so = new Font(Font.FontFamily, 14F, FontStyle.Bold);
        TextRenderer.DrawText(g, _giaTri, so, new Rectangle(8, 24, Width - 16, 30), SystemColors.ControlText, TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        TextRenderer.DrawText(g, _ghiChu, Font, new Rectangle(8, 56, Width - 16, 18), SystemColors.GrayText, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
    }
}
