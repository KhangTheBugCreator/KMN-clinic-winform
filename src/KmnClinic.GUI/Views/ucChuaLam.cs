namespace KmnClinic.GUI.Views;

/// <summary>Trang giữ chỗ cho màn hình chưa dựng ở Giai đoạn 2; ghi rõ module sẽ làm.</summary>
public class ucChuaLam : UserControl
{
    public ucChuaLam() : this("Màn hình", "W?") { }

    public ucChuaLam(string tenManHinh, string module)
    {
        Controls.Add(new Label
        {
            Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = SystemColors.GrayText,
            Text = $"{tenManHinh}\r\n\r\nMàn hình này sẽ được dựng ở module {module}."
        });
    }
}
