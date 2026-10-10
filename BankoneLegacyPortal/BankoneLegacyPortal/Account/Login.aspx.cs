using System;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;

namespace BankoneLegacyPortal.Account
{
    public partial class Login : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && !string.IsNullOrEmpty(Request.QueryString["ReturnUrl"]))
                Session["ReturnUrl"] = Request.QueryString["ReturnUrl"];
        }

        protected void LogIn(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = UserName.Text.Trim();
            string password = Password.Text;

            if (DbHelper.ValidateUser(username, password))
            {
                DataRow user = DbHelper.GetUser(username);
                string role = user != null ? user["Role"].ToString() : "Customer";

                // Store role in FormsAuthenticationTicket userData
                // Decoded in Global.Application_AuthenticateRequest
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                    version: 1,
                    name: username,
                    issueDate: DateTime.Now,
                    expiration: DateTime.Now.AddMinutes(30),
                    isPersistent: RememberMe.Checked,
                    userData: role);

                string encTicket = FormsAuthentication.Encrypt(ticket);
                Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket));

                if (user != null)
                {
                    Session["FullName"]       = user["FullName"].ToString();
                    Session["AccountNumber"]  = user["AccountNumber"].ToString();
                    Session["UserId"]         = user["UserId"].ToString();
                    Session["UserRole"]       = role;
                }

                string returnUrl = Session["ReturnUrl"] as string ?? "~/";
                Session.Remove("ReturnUrl");
                Response.Redirect(returnUrl);
            }
            else
            {
                ErrorMessage.Visible = true;
                FailureText.Text = "The username or password provided is incorrect. Please try again.";
            }
        }
    }
}
