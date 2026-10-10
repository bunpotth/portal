namespace BankoneLegacyPortal.Accounts
{
    public partial class Transfer
    {
        protected global::System.Web.UI.WebControls.MultiView TransferWizard;
        protected global::System.Web.UI.WebControls.View viewStep1;
        protected global::System.Web.UI.WebControls.PlaceHolder Step1Error;
        protected global::System.Web.UI.WebControls.Literal Step1ErrorText;
        protected global::System.Web.UI.WebControls.DropDownList ddlFromAccount;
        protected global::System.Web.UI.WebControls.DropDownList ddlToAccount;
        protected global::System.Web.UI.WebControls.TextBox txtAmount;
        protected global::System.Web.UI.WebControls.TextBox txtMemo;
        protected global::System.Web.UI.WebControls.Button BtnStep1Next;
        protected global::System.Web.UI.WebControls.View viewStep2;
        protected global::System.Web.UI.WebControls.Literal litConfirmFrom;
        protected global::System.Web.UI.WebControls.Literal litConfirmTo;
        protected global::System.Web.UI.WebControls.Literal litConfirmAmount;
        protected global::System.Web.UI.WebControls.Literal litConfirmMemo;
        protected global::System.Web.UI.WebControls.Button BtnStep2Back;
        protected global::System.Web.UI.WebControls.Button BtnStep2Confirm;
        protected global::System.Web.UI.WebControls.View viewStep3;
        protected global::System.Web.UI.WebControls.Literal litReceiptDetails;
    }
}
