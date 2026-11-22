using System.Drawing.Drawing2D;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Controls;

/// <summary>Panel nền trắng bo góc, viền nhạt — "card" chứa bảng hoặc form con.</summary>
public class KhungTrang : Panel
{
    public KhungTrang()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = Color.White;
        Padding = new Padding(16);
    }

    protected override void OnPaintBackground(PaintEventArgs e)
    {
        e.Graphics.Clear(Parent?.BackColor ?? Mau.Sky2);
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        var r = new Rectangle(0, 0, Width - 1, Height - 1);
        using var p = GiaoDien.VienBo(r, 12);
        using var nen = new SolidBrush(BackColor);
        e.Graphics.FillPath(nen, p);
        using var vien = new Pen(Mau.Border);
        e.Graphics.DrawPath(vien, p);
    }
}
