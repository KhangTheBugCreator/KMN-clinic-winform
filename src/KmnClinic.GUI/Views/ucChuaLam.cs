using KmnClinic.GUI.Controls;
using KmnClinic.GUI.Theme;

namespace KmnClinic.GUI.Views;

/// <summary>Trang giữ chỗ cho màn hình chưa dựng ở Giai đoạn 2; ghi rõ module sẽ làm.</summary>
public class ucChuaLam : UserControl
{
    public ucChuaLam() : this("Màn hình", "W?") { }

    public ucChuaLam(string tenManHinh, string module)
    {
        BackColor = Mau.Sky2;
        var khung = new KhungTrang { Dock = DockStyle.Fill };
        var lbl = new Label
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Mau.Muted,
            Font = GiaoDien.ChuTieuDeNho,
            Text = $"{tenManHinh}\r\n\r\nMàn hình này sẽ được dựng ở module {module}.\r\nGiai đoạn 2 chỉ dựng khung các màn hình chính để duyệt bố cục."
        };
        khung.Controls.Add(lbl);
        Controls.Add(khung);
    }
}
