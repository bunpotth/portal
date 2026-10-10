<%@ Page Title="About Bankone" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="BankoneLegacyPortal.About" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="page-header">
        <h2>About Bankone</h2>
    </div>
    <div class="row">
        <div class="col-md-8">
            <h3>Our Story</h3>
            <p>
                Founded in 1987, Bankone Corporation has grown from a single branch operation in Downtown Dallas
                to a regional banking institution with over 40 branch locations across the state of Texas.
                We are committed to delivering personalized financial services to individuals and businesses alike.
            </p>
            <p>
                Our online banking portal was launched in 2009 to provide customers with 24/7 access to their
                accounts, enabling real-time balance checks, fund transfers, and transaction history from any
                internet-connected device.
            </p>
            <h3>Our Mission</h3>
            <p>
                To provide our customers with secure, reliable, and accessible banking services while maintaining
                the highest standards of integrity and community investment.
            </p>
            <h3>Corporate Headquarters</h3>
            <address>
                <strong>Bankone Corporation, N.A.</strong><br />
                100 Commerce Street, Suite 500<br />
                Dallas, TX 75201<br />
                Phone: (214) 555-0100<br />
                Fax: (214) 555-0101<br />
                <a href="mailto:support@bankone.com">support@bankone.com</a>
            </address>
        </div>
        <div class="col-md-4">
            <div class="panel panel-bankone">
                <div class="panel-heading"><h3 class="panel-title">Quick Facts</h3></div>
                <div class="panel-body">
                    <ul class="list-unstyled">
                        <li><strong>Founded:</strong> 1987</li>
                        <li><strong>Headquarters:</strong> Dallas, TX</li>
                        <li><strong>Branches:</strong> 40+</li>
                        <li><strong>Employees:</strong> 1,200+</li>
                        <li><strong>FDIC Insured:</strong> Yes</li>
                        <li><strong>Online Banking:</strong> Since 2009</li>
                    </ul>
                </div>
            </div>
            <div class="panel panel-default" style="margin-top:15px;">
                <div class="panel-heading"><h3 class="panel-title">Regulatory</h3></div>
                <div class="panel-body" style="font-size:11px; color:#888;">
                    Bankone Corporation is regulated by the Office of the Comptroller of the Currency (OCC).
                    Member FDIC. Equal Housing Lender.
                </div>
            </div>
        </div>
    </div>
</asp:Content>
