<%@ Page Title="Change Password" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="BankoneLegacyPortal.Account.ChangePassword" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>Change Password</h2>
    </div>
    <div class="row">
        <div class="col-md-5">
            <asp:PlaceHolder ID="MessagePlaceholder" runat="server" Visible="false">
                <div id="MessageBox" runat="server" class="alert">
                    <asp:Literal ID="MessageText" runat="server" />
                </div>
            </asp:PlaceHolder>
            <div class="form-group">
                <label>Current Password</label>
                <asp:TextBox ID="CurrentPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="CurrentPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Current password is required." />
            </div>
            <div class="form-group">
                <label>New Password</label>
                <asp:TextBox ID="NewPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="NewPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="New password is required." />
            </div>
            <div class="form-group">
                <label>Confirm New Password</label>
                <asp:TextBox ID="ConfirmPassword" runat="server" TextMode="Password" CssClass="form-control" />
                <asp:CompareValidator runat="server"
                    ControlToValidate="ConfirmPassword" ControlToCompare="NewPassword"
                    CssClass="text-danger" Display="Dynamic" ErrorMessage="Passwords do not match." />
            </div>
            <asp:Button ID="BtnChange" runat="server" Text="Change Password" CssClass="btn btn-primary" OnClick="ChangePassword_Click" />
            &nbsp;
            <a href="~/" class="btn btn-default">Cancel</a>
        </div>
    </div>
</asp:Content>
