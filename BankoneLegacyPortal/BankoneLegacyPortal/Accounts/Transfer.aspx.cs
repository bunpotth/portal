using System;
using System.Data;
using System.Web.UI;

namespace BankoneLegacyPortal.Accounts
{
    public partial class Transfer : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                Response.Redirect("~/Account/Login");
                return;
            }

            if (!IsPostBack)
                PopulateAccountDropdowns();
        }

        private void PopulateAccountDropdowns()
        {
            string userId = Session["UserId"] as string;
            // Inline SQL - no parameterization; string concat with session value
            string sql = "SELECT AccountId, "
                       + "AccountNumber + ' (' + AccountType + ') - Bal: $' + CAST(CAST(Balance AS DECIMAL(18,2)) AS NVARCHAR(20)) AS DisplayText "
                       + "FROM BK_Accounts WHERE UserId = " + userId;

            DataTable dt = DbHelper.ExecuteQuery(sql);

            ddlFromAccount.DataSource   = dt;
            ddlFromAccount.DataTextField  = "DisplayText";
            ddlFromAccount.DataValueField = "AccountId";
            ddlFromAccount.DataBind();

            ddlToAccount.DataSource   = dt;
            ddlToAccount.DataTextField  = "DisplayText";
            ddlToAccount.DataValueField = "AccountId";
            ddlToAccount.DataBind();
        }

        protected void Step1_Next(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            decimal amount;
            if (!decimal.TryParse(txtAmount.Text, out amount) || amount <= 0)
            {
                Step1Error.Visible = true;
                Step1ErrorText.Text = "Please enter a valid positive amount (e.g. 150.00).";
                return;
            }

            if (ddlFromAccount.SelectedValue == ddlToAccount.SelectedValue)
            {
                Step1Error.Visible = true;
                Step1ErrorText.Text = "From and To accounts must be different.";
                return;
            }

            // Store wizard state in Session - no CSRF token (BANKLB-445)
            Session["Xfer_FromId"]      = ddlFromAccount.SelectedValue;
            Session["Xfer_FromDisplay"] = ddlFromAccount.SelectedItem.Text;
            Session["Xfer_ToId"]        = ddlToAccount.SelectedValue;
            Session["Xfer_ToDisplay"]   = ddlToAccount.SelectedItem.Text;
            Session["Xfer_Amount"]      = amount.ToString("0.00");
            Session["Xfer_Memo"]        = txtMemo.Text.Trim();

            litConfirmFrom.Text   = ddlFromAccount.SelectedItem.Text;
            litConfirmTo.Text     = ddlToAccount.SelectedItem.Text;
            litConfirmAmount.Text = string.Format("${0:0.00}", amount);
            litConfirmMemo.Text   = string.IsNullOrEmpty(txtMemo.Text) ? "(none)" : txtMemo.Text;

            TransferWizard.ActiveViewIndex = 1;
        }

        protected void Step2_Back(object sender, EventArgs e)
        {
            TransferWizard.ActiveViewIndex = 0;
            PopulateAccountDropdowns();
        }

        protected void Step2_Confirm(object sender, EventArgs e)
        {
            // No CSRF check - request origin not validated - BANKLB-445
            string fromId = Session["Xfer_FromId"]      as string;
            string toId   = Session["Xfer_ToId"]        as string;
            decimal amount = Convert.ToDecimal(Session["Xfer_Amount"]);
            string memo   = Session["Xfer_Memo"]        as string ?? "";

            // Two separate UPDATEs - no DB transaction wrapping
            // If second UPDATE fails, money is lost (BANKLB-510)
            DbHelper.ExecuteNonQuery(
                "UPDATE BK_Accounts SET Balance = Balance - " + amount
              + " WHERE AccountId = " + fromId);

            DbHelper.ExecuteNonQuery(
                "UPDATE BK_Accounts SET Balance = Balance + " + amount
              + " WHERE AccountId = " + toId);

            // Record transactions - inline SQL with string concat
            string descFrom = "Transfer Out" + (memo.Length > 0 ? " - " + memo.Replace("'","''") : "");
            string descTo   = "Transfer In"  + (memo.Length > 0 ? " - " + memo.Replace("'","''") : "");

            DbHelper.ExecuteNonQuery(string.Format(
                "INSERT INTO BK_Transactions (AccountId,TransactionDate,Description,Amount,TransactionType) "
              + "VALUES ({0},GETDATE(),'{1}',{2},'DR'), ({3},GETDATE(),'{4}',{2},'CR')",
                fromId, descFrom, amount, toId, descTo));

            litReceiptDetails.Text = string.Format(
                "Transferred <strong>${0:0.00}</strong> from {1} to {2} on {3}.",
                amount,
                Session["Xfer_FromDisplay"],
                Session["Xfer_ToDisplay"],
                DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));

            // Partial session cleanup - FromDisplay/ToDisplay left in session (BANKLB-335)
            Session.Remove("Xfer_FromId");
            Session.Remove("Xfer_ToId");
            Session.Remove("Xfer_Amount");
            Session.Remove("Xfer_Memo");

            TransferWizard.ActiveViewIndex = 2;
        }
    }
}
