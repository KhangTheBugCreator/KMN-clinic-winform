namespace KmnClinic.GUI.Forms;

partial class frmNhapTrieuChung
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null)) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblTieuDe = new Label();
        lblTrieuChung = new Label();
        txtTrieuChung = new TextBox();
        btnDongY = new Button();
        btnHuy = new Button();
        SuspendLayout();
        //
        // lblTieuDe
        //
        lblTieuDe.Location = new Point(16, 12);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new Size(428, 40);
        //
        // lblTrieuChung
        //
        lblTrieuChung.AutoSize = true;
        lblTrieuChung.Location = new Point(16, 56);
        lblTrieuChung.Name = "lblTrieuChung";
        lblTrieuChung.Text = "Triệu chứng (lưu vào hồ sơ khám):";
        //
        // txtTrieuChung
        //
        txtTrieuChung.Location = new Point(16, 80);
        txtTrieuChung.MaxLength = 255;
        txtTrieuChung.Multiline = true;
        txtTrieuChung.Name = "txtTrieuChung";
        txtTrieuChung.ScrollBars = ScrollBars.Vertical;
        txtTrieuChung.Size = new Size(428, 90);
        txtTrieuChung.TabIndex = 0;
        //
        // btnDongY
        //
        btnDongY.Location = new Point(248, 184);
        btnDongY.Name = "btnDongY";
        btnDongY.Size = new Size(96, 28);
        btnDongY.TabIndex = 1;
        btnDongY.Text = "Tiếp nhận";
        btnDongY.Click += btnDongY_Click;
        //
        // btnHuy
        //
        btnHuy.DialogResult = DialogResult.Cancel;
        btnHuy.Location = new Point(348, 184);
        btnHuy.Name = "btnHuy";
        btnHuy.Size = new Size(96, 28);
        btnHuy.TabIndex = 2;
        btnHuy.Text = "Hủy";
        //
        // frmNhapTrieuChung
        //
        AcceptButton = btnDongY;
        CancelButton = btnHuy;
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(460, 226);
        Controls.Add(lblTieuDe);
        Controls.Add(lblTrieuChung);
        Controls.Add(txtTrieuChung);
        Controls.Add(btnDongY);
        Controls.Add(btnHuy);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "frmNhapTrieuChung";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Tiếp nhận khách có hẹn";
        ResumeLayout(false);
        PerformLayout();
    }

    private Label lblTieuDe;
    private Label lblTrieuChung;
    private TextBox txtTrieuChung;
    private Button btnDongY;
    private Button btnHuy;
}
