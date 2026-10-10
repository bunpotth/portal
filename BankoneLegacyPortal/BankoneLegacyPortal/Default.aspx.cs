using System;
using System.Data;
using System.Web.UI;

namespace BankoneLegacyPortal
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                // Load full name from session; fall back to DB if session was cleared
                string fullName = Session["FullName"] as string;
                if (string.IsNullOrEmpty(fullName))
                {
                    DataRow user = DbHelper.GetUser(User.Identity.Name);
                    if (user != null)
                    {
                        fullName = user["FullName"].ToString();
                        // Repopulate session - BANKLB-299: session repopulation not logged
                        Session["FullName"]       = fullName;
                        Session["AccountNumber"]  = user["AccountNumber"].ToString();
                        Session["UserId"]         = user["UserId"].ToString();
                        Session["UserRole"]       = user["Role"].ToString();
                    }
                }
                litFullName.Text = fullName ?? User.Identity.Name;

                // Last login from session - not stored in DB (BANKLB-299)
                string lastLogin = Session["LastLoginDisplay"] as string;
                litLastLogin.Text = string.IsNullOrEmpty(lastLogin) ? "First login" : lastLogin;
                Session["LastLoginDisplay"] = DateTime.Now.ToString("MM/dd/yyyy h:mm tt");

                LoadQuickBalance();
                lblAlertTime.Text = DateTime.Now.ToString("h:mm tt");
            }
        }

        private void LoadQuickBalance()
        {
            string userId = Session["UserId"] as string;
            if (string.IsNullOrEmpty(userId)) return;

            // Direct SQL concatenation with session-derived value - no parameterization
            string sql = "SELECT TOP 1 AccountNumber, AccountType, Balance "
                       + "FROM BK_Accounts WHERE UserId = " + userId
                       + " AND AccountType = 'Checking'";
            DataTable dt = DbHelper.ExecuteQuery(sql);

            if (dt.Rows.Count > 0)
            {
                lblBalance.Text = string.Format(
                    "<strong>{0}</strong> ({1})<br/>Balance: <strong style='color:#006600;'>${2:0.00}</strong>",
                    dt.Rows[0]["AccountNumber"],
                    dt.Rows[0]["AccountType"],
                    Convert.ToDecimal(dt.Rows[0]["Balance"]));
            }
            else
            {
                lblBalance.Text = "<em>No checking account on file.</em>";
            }
        }

        protected void btnRefreshBalance_Click(object sender, EventArgs e)
        {
            LoadQuickBalance();
        }

        protected void AlertTimer_Tick(object sender, EventArgs e)
        {
            lblAlertTime.Text = DateTime.Now.ToString("h:mm:ss tt");
        }
    }
}
