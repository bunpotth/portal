<%@ Page Title="Home" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BankoneLegacyPortal._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <asp:LoginView runat="server">

        <%-- Anonymous: marketing landing page --%>
        <AnonymousTemplate>
            <div class="jumbotron">
                <h1>Welcome to Bankone</h1>
                <p>Your Trusted Banking Partner Since 1987. Secure 24/7 online access to your accounts, transaction history, and banking services.</p>
                <p style="margin-top:20px;">
                    <a class="btn btn-warning btn-lg" runat="server" href="~/Account/Login">Sign In &raquo;</a>
                    &nbsp;
                    <a class="btn btn-default btn-lg" runat="server" href="~/Account/Register">Open an Account</a>
                </p>
            </div>
            <div class="row" style="margin-top:20px;">
                <div class="col-md-4">
                    <div class="panel panel-bankone">
                        <div class="panel-heading"><h3 class="panel-title">Online Banking</h3></div>
                        <div class="panel-body">
                            <p>View balances, download statements, and manage your Bankone accounts 24/7 from any browser.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-bankone">
                        <div class="panel-heading"><h3 class="panel-title">Fund Transfers</h3></div>
                        <div class="panel-body">
                            <p>Transfer funds between your Bankone accounts quickly and securely with our step-by-step transfer wizard.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="panel panel-bankone">
                        <div class="panel-heading"><h3 class="panel-title">Branch Network</h3></div>
                        <div class="panel-body">
                            <p>Find your nearest Bankone branch or ATM. We have 40+ locations across Texas to serve you.</p>
                        </div>
                    </div>
                </div>
            </div>
        </AnonymousTemplate>

        <%-- Authenticated: dashboard with UpdatePanel widgets --%>
        <LoggedInTemplate>
            <div class="jumbotron">
                <h1>Welcome back, <asp:Literal ID="litFullName" runat="server" />!</h1>
                <p>Last login: <asp:Literal ID="litLastLogin" runat="server" /></p>
                <p style="margin-top:15px;">
                    <a class="btn btn-warning btn-sm" href="/Accounts/Summary">My Accounts</a>
                    &nbsp;
                    <a class="btn btn-default btn-sm" href="/Accounts/Transactions">Transactions</a>
                    &nbsp;
                    <a class="btn btn-default btn-sm" href="/Accounts/Transfer">Transfer Funds</a>
                </p>
            </div>

            <div class="row" style="margin-top:20px;">

                <%-- Widget 1: Quick Balance - UpdatePanel with manual refresh --%>
                <div class="col-md-4">
                    <div class="panel panel-bankone">
                        <div class="panel-heading"><h3 class="panel-title">Quick Balance</h3></div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="upQuickBalance" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:Label ID="lblBalance" runat="server" Text="Loading..." />
                                    <br /><br />
                                    <asp:Button ID="btnRefreshBalance" runat="server"
                                        Text="Refresh" CssClass="btn btn-xs btn-default"
                                        OnClick="btnRefreshBalance_Click" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <%-- Widget 2: Alerts - UpdatePanel with Timer (auto-refresh every 60s) --%>
                <div class="col-md-4">
                    <div class="panel panel-bankone">
                        <div class="panel-heading"><h3 class="panel-title">Security Alerts</h3></div>
                        <div class="panel-body">
                            <asp:UpdatePanel ID="upAlerts" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div class="bankone-alert">
                                        <strong>Security Reminder:</strong> Bankone will never ask for your password via email or phone.
                                    </div>
                                    <span class="text-muted" style="font-size:11px;">
                                        Updated: <asp:Label ID="lblAlertTime" runat="server" />
                                    </span>
                                    <asp:Timer ID="AlertTimer" runat="server" Interval="60000" OnTick="AlertTimer_Tick" />
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                    </div>
                </div>

                <%-- Widget 3: Branch Hours - static, no UpdatePanel --%>
                <div class="col-md-4">
                    <div class="panel panel-bankone">
                        <div class="panel-heading"><h3 class="panel-title">Branch Hours</h3></div>
                        <div class="panel-body">
                            <p><strong>Downtown Hub:</strong><br />Mon-Fri 9AM-5PM, Sat 9AM-1PM</p>
                            <p><strong>Westside Branch:</strong><br />Mon-Fri 9AM-6PM, Sat 10AM-2PM</p>
                            <p><a href="/Branch/Locator">All branch locations &raquo;</a></p>
                        </div>
                    </div>
                </div>

            </div><%-- /row --%>
        </LoggedInTemplate>

    </asp:LoginView>

</asp:Content>
