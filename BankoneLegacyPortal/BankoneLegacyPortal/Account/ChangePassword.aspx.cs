using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace BankoneLegacyPortal.Account
{
    public partial class ChangePassword : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
                Response.Redirect("~/Account/Login");
        }

        protected void ChangePassword_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            string username = User.Identity.Name;

            if (!DbHelper.ValidateUser(username, CurrentPassword.Text))
            {
                ShowMessage("alert-danger", "Current password is incorrect.");
                return;
            }

            string newHash = DbHelper.HashPassword(NewPassword.Text);

            // Direct string concatenation - no parameterization
            string sql = "UPDATE BK_Users SET PasswordHash = '" + newHash
                       + "' WHERE Username = '" + username + "'";
            DbHelper.ExecuteNonQuery(sql);

            ShowMessage("alert-success", "Your password has been changed successfully.");
            CurrentPassword.Text = "";
            NewPassword.Text = "";
            ConfirmPassword.Text = "";
        }

        private void ShowMessage(string cssClass, string text)
        {
            MessagePlaceholder.Visible = true;
            MessageBox.Attributes["class"] = "alert " + cssClass;
            MessageText.Text = text;
        }
    }
}
