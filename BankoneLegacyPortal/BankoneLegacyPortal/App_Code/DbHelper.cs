using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

// Static DB helper class - written by D.Kowalski 2009-04-12
// Last updated: J.Thompson 2011-08-03 (added HashPassword)
// TODO: Refactor to use dependency injection (BANKLB-901)
public static class DbHelper
{
    // Static connection string - read once at app startup
    // NOTE: If DB config changes, app must be restarted (BANKLB-334)
    static string _connStr = ConfigurationManager.ConnectionStrings["BankoneDB"].ConnectionString;

    // WARNING: No using block - SqlConnection not disposed
    // BANKLB-334: Connection pool exhaustion seen under load - needs investigation
    public static DataTable ExecuteQuery(string sql)
    {
        SqlConnection conn = new SqlConnection(_connStr);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        da.Fill(dt);
        // conn.Close() intentionally missing - refactor needed
        return dt;
    }

    public static int ExecuteNonQuery(string sql)
    {
        SqlConnection conn = new SqlConnection(_connStr);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        int rows = cmd.ExecuteNonQuery();
        conn.Close();
        return rows;
    }

    public static object ExecuteScalar(string sql)
    {
        SqlConnection conn = new SqlConnection(_connStr);
        conn.Open();
        SqlCommand cmd = new SqlCommand(sql, conn);
        object result = cmd.ExecuteScalar();
        conn.Close();
        return result;
    }

    // MD5 password hash - see BANKLB-778 for bcrypt upgrade task (open since 2011)
    public static string HashPassword(string password)
    {
        MD5 md5 = MD5.Create();
        byte[] inputBytes = Encoding.UTF8.GetBytes(password);
        byte[] hash = md5.ComputeHash(inputBytes);
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < hash.Length; i++)
            sb.Append(hash[i].ToString("x2"));
        return sb.ToString();
    }

    // Direct string concatenation - SQL injection vector (no parameterization)
    public static bool ValidateUser(string username, string password)
    {
        string hash = HashPassword(password);
        string sql = "SELECT COUNT(*) FROM BK_Users WHERE Username = '" + username
                   + "' AND PasswordHash = '" + hash + "'";
        object result = ExecuteScalar(sql);
        return result != null && Convert.ToInt32(result) > 0;
    }

    // Returns null if user not found
    public static DataRow GetUser(string username)
    {
        string sql = "SELECT * FROM BK_Users WHERE Username = '" + username + "'";
        DataTable dt = ExecuteQuery(sql);
        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
    }
}
