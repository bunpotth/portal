using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;

namespace BankoneLegacyPortal.Branch
{
    public partial class Locator : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadBranches(null);
        }

        private void LoadBranches(string searchTerm)
        {
            // Old-style DataSet/DataAdapter pattern (not DbHelper) - written by different developer
            string connStr = ConfigurationManager.ConnectionStrings["BankoneDB"].ConnectionString;

            // DEBUG: Uncomment to print connection string during troubleshooting (BANKLB-789)
            // Response.Write("<!-- ConnStr: " + connStr + " -->");

            string sql = "SELECT BranchId, BranchName, Address, Phone, Hours FROM BK_Branches";
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // String concatenation with user input - SQL injection vector
                sql += " WHERE Address LIKE '%" + searchTerm + "%'"
                     + " OR BranchName LIKE '%" + searchTerm + "%'";
            }
            sql += " ORDER BY BranchName";

            DataSet ds = new DataSet();
            // SqlConnection created but never explicitly disposed - connection object leak
            SqlConnection conn = new SqlConnection(connStr);
            SqlDataAdapter da  = new SqlDataAdapter(sql, conn);

            try
            {
                da.Fill(ds, "Branches");
            }
            catch (SqlException ex)
            {
                // Dumps raw DB error to response in production - BANKLB-892
                Response.Write("<div class='alert alert-danger'>Database error: "
                             + ex.Message + "</div>");
                return;
            }

            DataTable dt = ds.Tables["Branches"];
            BranchRepeater.DataSource = dt;
            BranchRepeater.DataBind();
            lblNoBranches.Visible = dt.Rows.Count == 0;
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadBranches(txtCity.Text);
        }

        protected void BtnAll_Click(object sender, EventArgs e)
        {
            txtCity.Text = "";
            LoadBranches(null);
        }
    }
}
