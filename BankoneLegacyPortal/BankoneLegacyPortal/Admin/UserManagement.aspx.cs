using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankoneLegacyPortal.Admin
{
    public partial class UserManagement : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Role check via Session string comparison instead of User.IsInRole
            // BANKLB-201: switch to [Authorize(Roles="Admin")] after MVC migration
            string role = Session["UserRole"] as string;
            if (!User.Identity.IsAuthenticated || role != "Admin")
            {
                Response.Redirect("~/");
                return;
            }

            if (!IsPostBack)
                LoadUsers(null);
        }

        private void LoadUsers(string search)
        {
            string sql = "SELECT UserId, Username, FullName, Email, AccountNumber, Role "
                       + "FROM BK_Users";

            if (!string.IsNullOrEmpty(search))
            {
                // Direct string concatenation in admin search - SQL injection
                sql += " WHERE Username LIKE '%" + search + "%'"
                     + " OR FullName LIKE '%" + search + "%'"
                     + " OR Email LIKE '%" + search + "%'";
            }

            sql += " ORDER BY UserId";

            DataTable dt = DbHelper.ExecuteQuery(sql);
            UsersGrid.DataSource = dt;
            UsersGrid.DataBind();
            lblCount.Text = string.Format("Showing {0} user(s).", dt.Rows.Count);
        }

        protected void UsersGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteUser")
            {
                string userId = e.CommandArgument.ToString();

                // Cascade delete via raw SQL - no FK constraints, no transaction
                // If any step fails, data will be in inconsistent state (BANKLB-510)
                DbHelper.ExecuteNonQuery(
                    "DELETE FROM BK_Transactions WHERE AccountId IN "
                  + "(SELECT AccountId FROM BK_Accounts WHERE UserId = " + userId + ")");
                DbHelper.ExecuteNonQuery(
                    "DELETE FROM BK_Accounts WHERE UserId = " + userId);
                DbHelper.ExecuteNonQuery(
                    "DELETE FROM BK_Users WHERE UserId = " + userId);

                lblMessage.Visible  = true;
                lblMessage.CssClass = "alert alert-success";
                lblMessage.Text     = string.Format("User ID {0} and all associated accounts deleted.", userId);
                LoadUsers(null);
            }
            else if (e.CommandName == "ResetPwd")
            {
                string username = e.CommandArgument.ToString();
                string newHash  = DbHelper.HashPassword("Password1");

                // Inline SQL - no parameterization; resets to hard-coded default
                DbHelper.ExecuteNonQuery(
                    "UPDATE BK_Users SET PasswordHash = '" + newHash
                  + "' WHERE Username = '" + username + "'");

                lblMessage.Visible  = true;
                lblMessage.CssClass = "alert alert-warning";
                lblMessage.Text     = "Password for '<strong>" + username
                                    + "</strong>' has been reset to: <code>Password1</code>";
                LoadUsers(null);
            }
        }

        protected void BtnSearch_Click(object sender, EventArgs e)
        {
            LoadUsers(txtSearch.Text);
        }

        protected void BtnShowAll_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            LoadUsers(null);
        }
    }
}
