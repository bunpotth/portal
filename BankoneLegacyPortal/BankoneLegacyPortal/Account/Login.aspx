<%@ Page Title="Sign In" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="BankoneLegacyPortal.Account.Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="login-box">
        <h2>Bankone Online Banking</h2>
        <p class="text-muted">Sign in to access your accounts securely.</p>

        <asp:PlaceHolder ID="ErrorMessage" runat="server" Visible="false">
            <div class="alert alert-danger">
                <asp:Literal ID="FailureText" runat="server" />
            </div>
        </asp:PlaceHolder>

        <% if (Request.QueryString["registered"] == "1") { %>
        <div class="alert alert-success">Account created successfully. Please sign in.</div>
        <% } %>

        <div class="form-group">
            <label for="UserName">Username</label>
            <asp:TextBox ID="UserName" runat="server" CssClass="form-control" placeholder="Enter your username" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="Username is required." />
        </div>
        <div class="form-group">
            <label for="Password">Password</label>
            <asp:TextBox ID="Password" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                CssClass="text-danger" Display="Dynamic" ErrorMessage="Password is required." />
        </div>
        <div class="checkbox" style="margin-bottom:15px;">
            <label>
                <asp:CheckBox ID="RememberMe" runat="server" />
                Remember me on this computer
            </label>
        </div>
        <asp:Button ID="BtnLogin" runat="server" Text="Sign In" CssClass="btn btn-primary" OnClick="LogIn" />

        <p class="text-muted" style="margin-top:20px; font-size:11px;">
            <%-- BANKLB-667: SSL cert expired 2012-09-30, waiting on IT renewal --%>
            Protected by 128-bit SSL encryption. &copy; 2009 Bankone Corporation.
        </p>
    </div>
</asp:Content>
