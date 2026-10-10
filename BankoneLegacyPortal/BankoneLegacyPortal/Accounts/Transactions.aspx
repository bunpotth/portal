<%@ Page Title="Transaction History" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Transactions.aspx.cs" Inherits="BankoneLegacyPortal.Accounts.Transactions" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>Transaction History</h2>
        <small>Account: <asp:Literal ID="litAccountNumber" runat="server" /></small>
    </div>

    <%-- Date filter - full postback on click (no UpdatePanel) --%>
    <div class="row" style="margin-bottom:15px;">
        <div class="col-md-3">
            <label>From Date</label>
            <asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY" />
        </div>
        <div class="col-md-3">
            <label>To Date</label>
            <asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" placeholder="MM/DD/YYYY" />
        </div>
        <div class="col-md-3" style="padding-top:25px;">
            <%-- Full postback - UpdatePanel would have been better here --%>
            <asp:Button ID="BtnFilter" runat="server" Text="Filter" CssClass="btn btn-default" OnClick="BtnFilter_Click" />
            <asp:Button ID="BtnAll" runat="server" Text="Show All" CssClass="btn btn-link" OnClick="BtnAll_Click" CausesValidation="false" />
        </div>
    </div>

    <%-- DataList renders as HTML table rows (legacy approach vs GridView) --%>
    <asp:DataList ID="TransactionList" runat="server" Width="100%"
        CssClass="table table-bordered" GridLines="Both">
        <HeaderTemplate>
            <tr>
                <th style="background:#003366; color:#fff; padding:8px; width:120px;">Date</th>
                <th style="background:#003366; color:#fff; padding:8px;">Description</th>
                <th style="background:#003366; color:#fff; padding:8px; width:120px; text-align:right;">Amount</th>
                <th style="background:#003366; color:#fff; padding:8px; width:60px; text-align:center;">Type</th>
            </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td style="padding:6px;"><%# Eval("TransactionDate", "{0:MM/dd/yyyy}") %></td>
                <td style="padding:6px;"><%# Eval("Description") %></td>
                <td style="padding:6px; text-align:right;">
                    <span class='<%# (string)Eval("TransactionType") == "CR" ? "txn-credit" : "txn-debit" %>'>
                        <%# string.Format("{0}${1:0.00}",
                              (string)Eval("TransactionType") == "CR" ? "+" : "-",
                              Math.Abs(Convert.ToDecimal(Eval("Amount")))) %>
                    </span>
                </td>
                <td style="padding:6px; text-align:center;"><%# Eval("TransactionType") %></td>
            </tr>
        </ItemTemplate>
        <AlternatingItemTemplate>
            <tr style="background-color:#F9F9F9;">
                <td style="padding:6px;"><%# Eval("TransactionDate", "{0:MM/dd/yyyy}") %></td>
                <td style="padding:6px;"><%# Eval("Description") %></td>
                <td style="padding:6px; text-align:right;">
                    <span class='<%# (string)Eval("TransactionType") == "CR" ? "txn-credit" : "txn-debit" %>'>
                        <%# string.Format("{0}${1:0.00}",
                              (string)Eval("TransactionType") == "CR" ? "+" : "-",
                              Math.Abs(Convert.ToDecimal(Eval("Amount")))) %>
                    </span>
                </td>
                <td style="padding:6px; text-align:center;"><%# Eval("TransactionType") %></td>
            </tr>
        </AlternatingItemTemplate>
    </asp:DataList>

    <asp:Label ID="lblNoRecords" runat="server" Visible="false">
        <div class="alert alert-info">No transactions found for the selected period.</div>
    </asp:Label>

    <p style="font-size:11px; color:#888; margin-top:10px;">
        Showing up to 90 days of transaction history. For older records please visit a branch.
        <br />Running balance: <asp:Literal ID="litRunningTotal" runat="server" />
    </p>
</asp:Content>
