using System.Data;
using Microsoft.Data.SqlClient;

namespace KmnClinic.DAL;

/// <summary>
/// Lớp trợ giúp ADO.NET dùng chung cho mọi DAL.
/// Mọi câu lệnh đều truyền giá trị bằng SqlParameter, không nối chuỗi SQL.
/// </summary>
public static class DbHelper
{
    /// <summary>Chuỗi kết nối, GUI đặt một lần lúc khởi động (đọc từ appsettings.json).</summary>
    public static string ChuoiKetNoi { get; set; } = "";

    private const int SoLanThuToiDa = 3;

    /// <summary>Mở một kết nối mới. Người gọi dùng "using" để đóng.</summary>
    public static SqlConnection MoKetNoi()
    {
        if (string.IsNullOrWhiteSpace(ChuoiKetNoi))
            throw new InvalidOperationException("Chưa cấu hình chuỗi kết nối CSDL (ConnectionStrings:PhongKham trong appsettings.json).");
        var conn = new SqlConnection(ChuoiKetNoi);
        conn.Open();
        return conn;
    }

    /// <summary>Thử mở kết nối; trả về null nếu được, hoặc câu lỗi nếu không.</summary>
    public static string? KiemTraKetNoi()
    {
        try
        {
            using var conn = MoKetNoi();
            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    /// <summary>
    /// Chạy một khối công việc trong transaction: commit khi xong, rollback khi có lỗi.
    /// Nếu SQL Server chọn transaction này làm nạn nhân deadlock (lỗi 1205) thì chạy lại cả khối, tối đa 3 lần.
    /// Khối công việc chỉ được thao tác CSDL (vì có thể bị chạy lại).
    /// </summary>
    public static T TrongGiaoDich<T>(Func<SqlConnection, SqlTransaction, T> congViec,
        IsolationLevel mucCoLap = IsolationLevel.ReadCommitted)
    {
        for (int lan = 1; ; lan++)
        {
            try
            {
                using var conn = MoKetNoi();
                using var tx = conn.BeginTransaction(mucCoLap);
                try
                {
                    var kq = congViec(conn, tx);
                    tx.Commit();
                    return kq;
                }
                catch
                {
                    try { tx.Rollback(); } catch { /* kết nối có thể đã đóng */ }
                    throw;
                }
            }
            catch (SqlException ex) when (ex.Number == 1205 && lan < SoLanThuToiDa)
            {
                Thread.Sleep(Random.Shared.Next(20, 80) * lan);
            }
        }
    }

    public static void TrongGiaoDich(Action<SqlConnection, SqlTransaction> congViec,
        IsolationLevel mucCoLap = IsolationLevel.ReadCommitted)
        => TrongGiaoDich<bool>((c, t) => { congViec(c, t); return true; }, mucCoLap);

    /// <summary>Chạy câu SELECT, đổ kết quả vào DataTable (dùng SqlDataAdapter) để gắn lên DataGridView.</summary>
    public static DataTable LayBang(string sql, params SqlParameter[] thamSo)
    {
        using var conn = MoKetNoi();
        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(thamSo);
        using var da = new SqlDataAdapter(cmd);
        var bang = new DataTable();
        da.Fill(bang);
        return bang;
    }

    // ----- Hàm mở rộng ngắn gọn cho SqlCommand / SqlDataReader -----

    public static SqlCommand Lenh(this SqlConnection conn, string sql, SqlTransaction? tx = null) => new(sql, conn, tx);

    /// <summary>Thêm tham số; null được đổi thành DBNull.</summary>
    public static SqlCommand P(this SqlCommand cmd, string ten, object? giaTri)
    {
        cmd.Parameters.AddWithValue(ten, giaTri ?? DBNull.Value);
        return cmd;
    }

    /// <summary>Thêm tham số có kiểu rõ ràng (dùng cho varchar để SQL Server không phải đổi kiểu ngầm).</summary>
    public static SqlCommand P(this SqlCommand cmd, string ten, SqlDbType kieu, int doDai, object? giaTri)
    {
        cmd.Parameters.Add(ten, kieu, doDai).Value = giaTri ?? DBNull.Value;
        return cmd;
    }

    public static string? ChuoiHoacNull(this SqlDataReader r, string cot)
    {
        var i = r.GetOrdinal(cot);
        return r.IsDBNull(i) ? null : r.GetString(i);
    }

    public static DateTime? NgayHoacNull(this SqlDataReader r, string cot)
    {
        var i = r.GetOrdinal(cot);
        return r.IsDBNull(i) ? null : r.GetDateTime(i);
    }
}
