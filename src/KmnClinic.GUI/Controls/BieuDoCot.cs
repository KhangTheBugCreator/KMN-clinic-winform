
namespace KmnClinic.GUI.Controls;

/// <summary>
/// Biểu đồ cột đơn giản tự vẽ bằng GDI+ (không cần thư viện ngoài):
/// trục Y có vạch chia tròn số, cột, nhãn trục X, giá trị khi rê chuột.
/// </summary>
public class BieuDoCot : Control
{
    private string[] _nhan = [];
    private double[] _giaTri = [];
    private int _dangRe = -1;

    public BieuDoCot()
    {
        SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        BackColor = SystemColors.Window;
    }

    public void DatDuLieu(string[] nhan, double[] giaTri)
    {
        _nhan = nhan;
        _giaTri = giaTri;
        Invalidate();
    }

    private Rectangle VungVe => new(48, 12, Math.Max(10, Width - 60), Math.Max(10, Height - 44));

    private static double BuocChia(double max)
    {
        var tho = max / 4;
        var muoi = Math.Pow(10, Math.Floor(Math.Log10(tho)));
        foreach (var b in new[] { 1, 2, 2.5, 5, 10 }) if (b * muoi >= tho) return b * muoi;
        return 10 * muoi;
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        var g = e.Graphics;
        if (_giaTri.Length == 0) return;
        var v = VungVe;
        var buoc = BuocChia(_giaTri.Max());
        var tran = Math.Ceiling(_giaTri.Max() / buoc) * buoc;

        using var luoi = new Pen(Color.Gainsboro);
        for (double m = 0; m <= tran + 1e-9; m += buoc)
        {
            var y = v.Bottom - (int)(m / tran * v.Height);
            g.DrawLine(luoi, v.Left, y, v.Right, y);
            TextRenderer.DrawText(g, m.ToString("0.#"), Font, new Rectangle(0, y - 10, v.Left - 8, 20), SystemColors.GrayText, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
        }

        var o = (float)v.Width / _giaTri.Length;
        var rong = Math.Max(4, o * 0.6f);
        for (int i = 0; i < _giaTri.Length; i++)
        {
            var cao = (float)(_giaTri[i] / tran * v.Height);
            var x = v.Left + i * o + (o - rong) / 2;
            var r = Rectangle.Round(new RectangleF(x, v.Bottom - cao, rong, cao));
            g.FillRectangle(i == _dangRe ? Brushes.Navy : Brushes.SteelBlue, r);
            if (i < _nhan.Length && (o >= 34 || i % 2 == 0))
                TextRenderer.DrawText(g, _nhan[i], Font, new Rectangle((int)(v.Left + i * o) - 10, v.Bottom + 6, (int)o + 20, 20), SystemColors.GrayText, TextFormatFlags.HorizontalCenter);
            if (i == _dangRe)
                TextRenderer.DrawText(g, _giaTri[i].ToString("0.#"), Font, new Rectangle((int)x - 20, r.Top - 22, (int)rong + 40, 20), SystemColors.ControlText, TextFormatFlags.HorizontalCenter);
        }
    }

    protected override void OnMouseMove(MouseEventArgs e)
    {
        base.OnMouseMove(e);
        var v = VungVe;
        var moi = _giaTri.Length == 0 || e.X < v.Left || e.X > v.Right ? -1 : (int)((e.X - v.Left) / ((float)v.Width / _giaTri.Length));
        if (moi != _dangRe) { _dangRe = moi; Invalidate(); }
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _dangRe = -1;
        Invalidate();
    }
}
