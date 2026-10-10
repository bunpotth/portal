using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BankoneLegacyPortal.Accounts
{
    public partial class Summary : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
                return;
            }

            if (!IsPostBack)
                LoadAccounts();
        }

        private void LoadAccounts()
        {
            string userId = Session["UserId"] as string;

            litCustomerName.Text  = Session["FullName"] as string ?? User.Identity.Name;
            litAccountNumber.Text = Session["AccountNumber"] as string ?? "N/A";
            litAsOf.Text          = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");

            if (string.IsNullOrEmpty(userId)) return;

            // Direct SQL concatenation - Session["UserId"] assumed trusted (not validated)
            string sql = "SELECT AccountId, AccountNumber, AccountType, Balance, OpenedDate "
                       + "FROM BK_Accounts WHERE UserId = " + userId
                       + " ORDER BY AccountType";
            DataTable dt = DbHelper.ExecuteQuery(sql);

            AccountsGrid.DataSource = dt;
            AccountsGrid.DataBind();

            // Total calculated in a C# loop instead of SQL SUM() - BANKLB-600
            decimal total = 0;
            foreach (DataRow row in dt.Rows)
                total += Convert.ToDecimal(row["Balance"]);

            litTotalBalance.Text = string.Format(
                "<span style='color:#003366; font-size:16px;'>${0:0.00}</span>", total);
        }

        protected void AccountsGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ViewTxn")
            {
                Session["SelectedAccountId"] = e.CommandArgument.ToString();
                Response.Redirect("~/Accounts/Transactions");
            }
        }
    }
}
