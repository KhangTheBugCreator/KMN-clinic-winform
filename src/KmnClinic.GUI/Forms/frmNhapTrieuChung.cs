namespace KmnClinic.GUI.Forms;

/// <summary>Hỏi triệu chứng khi tiếp nhận khách có hẹn (điền sẵn triệu chứng ban đầu lúc đặt lịch).</summary>
public partial class frmNhapTrieuChung : Form
{
    public string TrieuChung => txtTrieuChung.Text;

    public frmNhapTrieuChung(string tieuDe, string? goiY)
    {
        InitializeComponent();
        lblTieuDe.Text = tieuDe;
        txtTrieuChung.Text = goiY ?? "";
    }

    private void btnDongY_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTrieuChung.Text))
        {
            MessageBox.Show(this, "Vui lòng nhập triệu chứng.", Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        DialogResult = DialogResult.OK;
    }
}
