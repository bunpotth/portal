<%@ Page Title="Account Summary" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Summary.aspx.cs" Inherits="BankoneLegacyPortal.Accounts.Summary" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>Account Summary</h2>
        <small>
            Customer: <asp:Literal ID="litCustomerName" runat="server" /> &nbsp;&bull;&nbsp;
            Account: <asp:Literal ID="litAccountNumber" runat="server" />
        </small>
    </div>

    <%-- SqlDataSource with connection string from config and inline SELECT --%>
    <%-- NOTE: SelectCommand is set dynamically in code-behind from Session["UserId"] --%>
    <asp:SqlDataSource ID="AccountsDataSource" runat="server"
        ConnectionString='<%$ ConnectionStrings:BankoneDB %>'
        SelectCommand="SELECT AccountId, AccountNumber, AccountType, Balance, OpenedDate FROM BK_Accounts WHERE UserId = 0 ORDER BY AccountType">
    </asp:SqlDataSource>

    <%-- GridView with ViewState ON (default) - generates large __VIEWSTATE hidden field --%>
    <asp:GridView ID="AccountsGrid" runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-bordered table-hover"
        EnableViewState="true"
        OnRowCommand="AccountsGrid_RowCommand"
        DataKeyNames="AccountId"
        EmptyDataText="No accounts found.">
        <HeaderStyle CssClass="bg-primary" />
        <Columns>
            <asp:BoundField DataField="AccountNumber" HeaderText="Account #" />
            <asp:BoundField DataField="AccountType"   HeaderText="Type" />
            <asp:BoundField DataField="Balance" HeaderText="Current Balance"
                DataFormatString="${0:0.00}" HtmlEncode="false" />
            <asp:BoundField DataField="OpenedDate" HeaderText="Date Opened"
                DataFormatString="{0:MM/dd/yyyy}" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton CommandName="ViewTxn"
                        CommandArgument='<%# Eval("AccountId") %>'
                        runat="server" CssClass="btn btn-xs btn-default">
                        View Transactions
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <div id="divTotalsRow" runat="server"
         style="text-align:right; font-weight:bold; padding:8px; border-top:2px solid #C8960C;">
        Total Balance Across All Accounts:
        <asp:Literal ID="litTotalBalance" runat="server" />
    </div>

    <p style="font-size:11px; color:#888; margin-top:10px;">
        Balances shown are as of <asp:Literal ID="litAsOf" runat="server" />.
        For the most current balance please call (214) 555-0100.
    </p>
</asp:Content>
