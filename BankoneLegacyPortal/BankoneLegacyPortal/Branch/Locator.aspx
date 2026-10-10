<%@ Page Title="Branch Locator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Locator.aspx.cs" Inherits="BankoneLegacyPortal.Branch.Locator" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>Branch Locator</h2>
        <small>Find your nearest Bankone branch or ATM</small>
    </div>

    <div class="row" style="margin-bottom:20px;">
        <div class="col-md-4">
            <label>Search by City or Address</label>
            <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" placeholder="e.g. Dallas, Northpark..." />
        </div>
        <div class="col-md-3" style="padding-top:25px;">
            <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="BtnSearch_Click" />
            <asp:Button ID="BtnAll" runat="server" Text="All Branches" CssClass="btn btn-default" OnClick="BtnAll_Click" CausesValidation="false" />
        </div>
    </div>

    <%-- Repeater chosen over GridView for HTML control --%>
    <asp:Repeater ID="BranchRepeater" runat="server">
        <HeaderTemplate><div class="row"></HeaderTemplate>
        <ItemTemplate>
            <div class="col-md-6" style="margin-bottom:15px;">
                <div class="panel panel-bankone account-card">
                    <div class="panel-heading">
                        <h3 class="panel-title"><%# Eval("BranchName") %></h3>
                    </div>
                    <div class="panel-body">
                        <address>
                            <strong><%# Eval("BranchName") %></strong><br />
                            <%# Eval("Address") %><br />
                            <abbr title="Phone">P:</abbr> <%# Eval("Phone") %>
                        </address>
                        <p><strong>Hours:</strong> <%# Eval("Hours") %></p>
                    </div>
                </div>
            </div>
        </ItemTemplate>
        <FooterTemplate></div></FooterTemplate>
    </asp:Repeater>

    <asp:Label ID="lblNoBranches" runat="server" Visible="false">
        <div class="alert alert-info">No branches found matching your search. Try a broader term.</div>
    </asp:Label>

    <p class="text-muted" style="font-size:11px; margin-top:15px;">
        For ATM locations call (214) 555-0100 or visit www.bankone.com/atm
    </p>
</asp:Content>
