<%@ Page Title="Open an Account" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="BankoneLegacyPortal.Account.Register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>Open a Bankone Account</h2>
        <small>Fill in your details below to get started</small>
    </div>
    <div class="row">
        <div class="col-md-6">
            <asp:PlaceHolder ID="ErrorMessage" runat="server" Visible="false">
                <div class="alert alert-danger">
                    <asp:Literal ID="FailureText" runat="server" />
                </div>
            </asp:PlaceHolder>

            <div class="form-group">
                <label>Full Name</label>
                <asp:TextBox ID="FullName" runat="server" CssClass="form-control" placeholder="e.g. Jane Smith" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="FullName"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Full name is required." />
            </div>
            <div class="form-group">
                <label>Email Address</label>
                <asp:TextBox ID="Email" runat="server" CssClass="form-control" TextMode="Email" placeholder="you@email.com" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Email"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Email is required." />
            </div>
            <div class="form-group">
                <label>Choose a Username</label>
                <asp:TextBox ID="UserName" runat="server" CssClass="form-control" placeholder="Lowercase letters and dots only" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Username is required." />
            </div>
            <div class="form-group">
                <label>Password</label>
                <asp:TextBox ID="Password" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Password"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Password is required." />
                <p class="help-block" style="font-size:11px;">Minimum 8 characters.</p>
            </div>
            <div class="form-group">
                <label>Initial Account Type</label>
                <asp:DropDownList ID="AccountType" runat="server" CssClass="form-control">
                    <asp:ListItem Value="Checking">Checking Account</asp:ListItem>
                    <asp:ListItem Value="Savings">Savings Account</asp:ListItem>
                </asp:DropDownList>
            </div>
            <asp:Button ID="BtnRegister" runat="server" Text="Open Account" CssClass="btn btn-primary" OnClick="Register_Click" />
        </div>
        <div class="col-md-4 col-md-offset-1">
            <div class="panel panel-bankone" style="margin-top:20px;">
                <div class="panel-heading"><h3 class="panel-title">New Account Benefits</h3></div>
                <div class="panel-body">
                    <ul class="list-unstyled">
                        <li>&#10003; No monthly fees on Checking</li>
                        <li>&#10003; Free online bill pay</li>
                        <li>&#10003; 24/7 online access</li>
                        <li>&#10003; FDIC insured up to $250,000</li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
