namespace KmnClinic.GUI.Theme;

/// <summary>Chế độ của khung nhập liệu trên các màn hình danh mục.</summary>
public enum CheDoNhap { Xem, Them, Sua }

/// <summary>Vài hàm dùng chung cho các màn hình. Giao diện giữ kiểu WinForms mặc định, không tô màu trang trí.</summary>
public static class GiaoDien
{
    /// <summary>Cấu hình DataGridView cho danh sách chỉ đọc: chọn cả dòng, không thêm/xóa trực tiếp trên lưới.</summary>
    public static void Luoi(DataGridView g)
    {
        g.AllowUserToAddRows = false;
        g.AllowUserToDeleteRows = false;
        g.AllowUserToResizeRows = false;
        g.ReadOnly = true;
        g.MultiSelect = false;
        g.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        g.RowHeadersWidth = 24;
        g.BackgroundColor = SystemColors.Window;
        g.AutoGenerateColumns = false;
    }

    /// <summary>Chọn dòng đầu tiên thỏa điều kiện (vd. dòng vừa thêm/sửa); không có điều kiện thì giữ dòng đầu.</summary>
    public static void ChonDong(DataGridView g, Func<object, bool>? dieuKien)
    {
        if (dieuKien is null) return;
        foreach (DataGridViewRow r in g.Rows)
        {
            if (r.DataBoundItem is { } o && dieuKien(o))
            {
                g.CurrentCell = r.Cells[0];
                return;
            }
        }
    }

    /// <summary>Hiển thị số tiền dạng 150.000 đ.</summary>
    public static string Tien(decimal soTien) => soTien.ToString("#,0", System.Globalization.CultureInfo.GetCultureInfo("vi-VN")) + " đ";

    /// <summary>Logo KMN Clinic (nhúng trong file .exe), chỉ dùng khi in hóa đơn.</summary>
    public static Image? Logo()
    {
        var s = typeof(GiaoDien).Assembly.GetManifestResourceStream("KmnClinic.logo.png");
        return s is null ? null : Image.FromStream(s);
    }
}
