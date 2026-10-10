<%@ Page Title="User Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.cs" Inherits="BankoneLegacyPortal.Admin.UserManagement" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>User Management <small class="text-muted">Admin Panel</small></h2>
    </div>

    <div class="row" style="margin-bottom:15px;">
        <div class="col-md-5">
            <label>Search</label>
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Username or full name..." />
                <span class="input-group-btn">
                    <asp:Button ID="BtnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="BtnSearch_Click" />
                </span>
            </div>
        </div>
        <div class="col-md-2" style="padding-top:25px;">
            <asp:Button ID="BtnShowAll" runat="server" Text="Show All" CssClass="btn btn-link" OnClick="BtnShowAll_Click" CausesValidation="false" />
        </div>
    </div>

    <asp:Label ID="lblMessage" runat="server" Visible="false" />

    <%-- GridView with ViewState ON - inline DELETE via RowCommand (no confirmation dialog server-side) --%>
    <asp:GridView ID="UsersGrid" runat="server"
        AutoGenerateColumns="false"
        CssClass="table table-bordered table-hover table-striped"
        EnableViewState="true"
        OnRowCommand="UsersGrid_RowCommand"
        DataKeyNames="UserId">
        <Columns>
            <asp:BoundField DataField="UserId"        HeaderText="ID" />
            <asp:BoundField DataField="Username"      HeaderText="Username" />
            <asp:BoundField DataField="FullName"      HeaderText="Full Name" />
            <asp:BoundField DataField="Email"         HeaderText="Email" />
            <asp:BoundField DataField="AccountNumber" HeaderText="Account #" />
            <asp:BoundField DataField="Role"          HeaderText="Role" />
            <asp:TemplateField HeaderText="Actions">
                <ItemTemplate>
                    <asp:LinkButton
                        CommandName="DeleteUser"
                        CommandArgument='<%# Eval("UserId") %>'
                        runat="server" CssClass="btn btn-xs btn-danger"
                        OnClientClick="return confirm('Delete user and all associated data?');">
                        Delete
                    </asp:LinkButton>
                    &nbsp;
                    <asp:LinkButton
                        CommandName="ResetPwd"
                        CommandArgument='<%# Eval("Username") %>'
                        runat="server" CssClass="btn btn-xs btn-warning">
                        Reset Pwd
                    </asp:LinkButton>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>

    <p class="text-muted">
        <asp:Label ID="lblCount" runat="server" />
    </p>
</asp:Content>
