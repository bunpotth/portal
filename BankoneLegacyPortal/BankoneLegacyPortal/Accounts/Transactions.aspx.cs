using System;
using System.Data;
using System.Web.UI;

namespace BankoneLegacyPortal.Accounts
{
    public partial class Transactions : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
                return;
            }

            if (!IsPostBack)
                LoadTransactions(null, null);
        }

        private void LoadTransactions(string fromDate, string toDate)
        {
            string accountId = Session["SelectedAccountId"] as string;
            string userId    = Session["UserId"] as string;

            if (string.IsNullOrEmpty(accountId))
            {
                // Default to first account for this user
                string findSql = "SELECT TOP 1 AccountId, AccountNumber FROM BK_Accounts WHERE UserId = " + userId;
                DataTable accts = DbHelper.ExecuteQuery(findSql);
                if (accts.Rows.Count > 0)
                {
                    accountId = accts.Rows[0]["AccountId"].ToString();
                    litAccountNumber.Text = accts.Rows[0]["AccountNumber"].ToString();
                    Session["SelectedAccountId"] = accountId;
                }
            }
            else
            {
                object accNum = DbHelper.ExecuteScalar(
                    "SELECT AccountNumber FROM BK_Accounts WHERE AccountId = " + accountId);
                litAccountNumber.Text = accNum != null ? accNum.ToString() : "N/A";
            }

            if (string.IsNullOrEmpty(accountId)) return;

            // Build SQL inline - string concatenation with user input (SQL injection)
            string sql = "SELECT TransactionId, TransactionDate, Description, Amount, TransactionType "
                       + "FROM BK_Transactions WHERE AccountId = " + accountId;

            if (!string.IsNullOrEmpty(fromDate))
                sql += " AND TransactionDate >= '" + fromDate + "'";
            if (!string.IsNullOrEmpty(toDate))
                sql += " AND TransactionDate <= '" + toDate + " 23:59:59'";

            sql += " ORDER BY TransactionDate DESC";

            // Debug line left in from testing - BANKLB-789
            // Response.Write("<!-- DEBUG SQL: " + sql + " -->");

            DataTable dt = DbHelper.ExecuteQuery(sql);
            TransactionList.DataSource = dt;
            TransactionList.DataBind();

            lblNoRecords.Visible = dt.Rows.Count == 0;

            // Running total - C# loop, not SQL (BANKLB-600)
            decimal credits = 0, debits = 0;
            foreach (DataRow row in dt.Rows)
            {
                decimal amt = Convert.ToDecimal(row["Amount"]);
                if (row["TransactionType"].ToString() == "CR") credits += amt;
                else debits += amt;
            }
            litRunningTotal.Text = string.Format(
                "<span class='txn-credit'>+${0:0.00} credits</span> / "
              + "<span class='txn-debit'>-${1:0.00} debits</span>",
                credits, debits);
        }

        protected void BtnFilter_Click(object sender, EventArgs e)
        {
            LoadTransactions(txtFromDate.Text, txtToDate.Text);
        }

        protected void BtnAll_Click(object sender, EventArgs e)
        {
            txtFromDate.Text = "";
            txtToDate.Text   = "";
            LoadTransactions(null, null);
        }
    }
}
