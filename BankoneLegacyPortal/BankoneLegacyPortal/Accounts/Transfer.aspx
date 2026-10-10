<%@ Page Title="Transfer Funds" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transfer.aspx.cs" Inherits="BankoneLegacyPortal.Accounts.Transfer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>Transfer Funds</h2>
    </div>

    <%-- MultiView session wizard: state passed through Session, not URL or form tokens --%>
    <%-- No CSRF protection - BANKLB-445 --%>
    <asp:MultiView ID="TransferWizard" runat="server" ActiveViewIndex="0">

        <%-- ===== STEP 1: Enter details ===== --%>
        <asp:View ID="viewStep1" runat="server">
            <h4 class="navy-text">Step 1 of 3 &mdash; Enter Transfer Details</h4>
            <asp:PlaceHolder ID="Step1Error" runat="server" Visible="false">
                <div class="alert alert-danger">
                    <asp:Literal ID="Step1ErrorText" runat="server" />
                </div>
            </asp:PlaceHolder>
            <div class="col-md-5">
                <div class="form-group">
                    <label>From Account</label>
                    <asp:DropDownList ID="ddlFromAccount" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label>To Account</label>
                    <asp:DropDownList ID="ddlToAccount" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <label>Amount ($)</label>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control" placeholder="0.00" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAmount"
                        CssClass="text-danger" Display="Dynamic" ErrorMessage="Amount is required." />
                </div>
                <div class="form-group">
                    <label>Memo <small class="text-muted">(optional)</small></label>
                    <asp:TextBox ID="txtMemo" runat="server" CssClass="form-control" MaxLength="100" />
                </div>
                <asp:Button ID="BtnStep1Next" runat="server" Text="Next &raquo;" CssClass="btn btn-primary" OnClick="Step1_Next" />
            </div>
        </asp:View>

        <%-- ===== STEP 2: Confirm ===== --%>
        <asp:View ID="viewStep2" runat="server">
            <h4 class="navy-text">Step 2 of 3 &mdash; Confirm Transfer</h4>
            <div class="col-md-6">
                <table class="table table-bordered">
                    <tr><th width="140">From Account</th><td><asp:Literal ID="litConfirmFrom" runat="server" /></td></tr>
                    <tr><th>To Account</th><td><asp:Literal ID="litConfirmTo" runat="server" /></td></tr>
                    <tr><th>Amount</th><td><strong class="txn-credit"><asp:Literal ID="litConfirmAmount" runat="server" /></strong></td></tr>
                    <tr><th>Memo</th><td><asp:Literal ID="litConfirmMemo" runat="server" /></td></tr>
                </table>
                <div class="bankone-alert">
                    <strong>Please review carefully.</strong>
                    Transfers are processed immediately. Online reversal is not available &mdash; contact a branch to dispute.
                </div>
                <asp:Button ID="BtnStep2Back" runat="server" Text="&laquo; Back" CssClass="btn btn-default" OnClick="Step2_Back" CausesValidation="false" />
                &nbsp;
                <asp:Button ID="BtnStep2Confirm" runat="server" Text="Confirm Transfer" CssClass="btn btn-danger" OnClick="Step2_Confirm" />
            </div>
        </asp:View>

        <%-- ===== STEP 3: Receipt ===== --%>
        <asp:View ID="viewStep3" runat="server">
            <h4 class="navy-text">Step 3 of 3 &mdash; Transfer Complete</h4>
            <div class="col-md-7">
                <div class="alert alert-success">
                    <strong>&#10003; Transfer Successful!</strong><br />
                    <asp:Literal ID="litReceiptDetails" runat="server" />
                </div>
                <p>
                    <a href="/Accounts/Summary" class="btn btn-primary">Return to Account Summary</a>
                    &nbsp;
                    <a href="/Accounts/Transactions" class="btn btn-default">View Transactions</a>
                </p>
            </div>
        </asp:View>

    </asp:MultiView>
</asp:Content>
