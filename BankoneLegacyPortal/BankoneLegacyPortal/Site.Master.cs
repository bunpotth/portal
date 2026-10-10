using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace BankoneLegacyPortal
{
    public partial class SiteMaster : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Show Admin link only for Admin role
            liAdmin.Visible = Request.IsAuthenticated && User.IsInRole("Admin");
        }

        protected void LoginStatus_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            // Clear session on sign-out
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();
            Response.Redirect("~/");
        }
    }
}
