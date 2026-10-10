using System;
using System.Web.UI;

namespace BankoneLegacyPortal.Account
{
    public partial class Register : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Register_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = UserName.Text.Trim();

            // Username existence check - direct string concatenation (SQL injection)
            string checkSql = "SELECT COUNT(*) FROM BK_Users WHERE Username = '" + username + "'";
            object existing = DbHelper.ExecuteScalar(checkSql);
            if (existing != null && Convert.ToInt32(existing) > 0)
            {
                ErrorMessage.Visible = true;
                FailureText.Text = "That username is already taken. Please choose another.";
                return;
            }

            // Generate account number from max UserId - BANKLB-512: race condition possible
            object maxId = DbHelper.ExecuteScalar("SELECT MAX(UserId) FROM BK_Users");
            int nextId = (maxId == null || maxId == System.DBNull.Value ? 1 : Convert.ToInt32(maxId)) + 1;
            string accountNumber = "BK-" + nextId.ToString("D5");

            string hash = DbHelper.HashPassword(Password.Text);

            // String concatenation inserts - no parameterized queries
            string insertUser = string.Format(
                "INSERT INTO BK_Users (Username, PasswordHash, FullName, Email, AccountNumber, Role) "
              + "VALUES ('{0}', '{1}', '{2}', '{3}', '{4}', 'Customer')",
                username,
                hash,
                FullName.Text.Replace("'", "''"),
                Email.Text.Replace("'", "''"),
                accountNumber);
            DbHelper.ExecuteNonQuery(insertUser);

            // Retrieve new UserId and create initial account
            object newUserId = DbHelper.ExecuteScalar("SELECT UserId FROM BK_Users WHERE Username = '" + username + "'");
            if (newUserId != null)
            {
                string acctSuffix = AccountType.SelectedValue == "Checking" ? "CHK" : "SAV";
                string insertAcct = string.Format(
                    "INSERT INTO BK_Accounts (AccountNumber, AccountType, Balance, UserId, OpenedDate) "
                  + "VALUES ('{0}-{1}', '{2}', 0.00, {3}, GETDATE())",
                    accountNumber, acctSuffix, AccountType.SelectedValue, newUserId);
                DbHelper.ExecuteNonQuery(insertAcct);
            }

            Response.Redirect("~/Account/Login?registered=1");
        }
    }
}
