Bankone Corporate Online Banking Portal
Version 2.3 Build 20131015
========================================

DESCRIPTION:
  Internal corporate online banking portal for Bankone Corporation customers.
  Provides account management, transaction history, fund transfers, and
  branch locator functionality.

TECH STACK:
  - .NET Framework 4.5
  - ASP.NET WebForms
  - Bootstrap 3.1.1 (CDN - see BANKLB-421 for local hosting task)
  - jQuery 1.10.2 (CDN - BANKLB-601: upgrade to 2.x pending)
  - SQL Server 2012 LocalDB (dev) / SQL Server 2008 R2 on BKDB01 (prod)
  - Custom FormsAuthentication + hand-rolled ADO.NET

TO RUN (Development):
  1. Open BankoneLegacyPortal.sln in Visual Studio 2013
  2. Ensure SQL Server LocalDB v11.0 is installed
  3. Press F5 - LocalDB catalog will be created and seeded on first run
  4. Navigate to http://localhost:50000/

DEFAULT CREDENTIALS (Dev/Test ONLY - do not use in production):
  Admin:    username=admin         password=Admin@1234
  Customer: username=alice.mercer  password=Password1
            username=bob.tran      password=Password1
            username=carol.simms   password=Password1

DEPLOYMENT:
  See \\fileserver\bankone\docs\deployment-guide-v2.3.pdf
  Ask J.Thompson or D.Kowalski for production DB migration scripts.
  Production connection string is in \\fileserver\bankone\docs\web-config-prod.txt
  (not checked into source control - BANKLB-330)

KNOWN ISSUES (JIRA project: BANKLB):
  BANKLB-201: Switch Admin check to [Authorize(Roles="Admin")] when MVC migration done
  BANKLB-299: Track last login timestamp properly in DB
  BANKLB-334: SqlConnection not disposed in DbHelper.ExecuteQuery - pool exhaustion under load
  BANKLB-421: CDN dependency for Bootstrap/jQuery - must go local before prod firewall change
  BANKLB-441: DB seed failure swallowed silently - add proper alerting
  BANKLB-445: CSRF protection missing on Transfer page
  BANKLB-503: Application_BeginRequest runs on static file requests - perf overhead
  BANKLB-512: Race condition in Register.aspx account number generation
  BANKLB-601: jQuery upgrade from 1.10.2 to 2.x (blocked on IE8 support - awaiting decision)
  BANKLB-667: SSL certificate renewal needed (expired 2012-09-30)
  BANKLB-778: Upgrade password hashing from MD5 to bcrypt (open since 2011, see Security audit 2011-Q3)
  BANKLB-789: Commented-out SQL debug output left in Transactions.aspx.cs
  BANKLB-892: Application_Error and customErrors expose stack traces in production
  BANKLB-901: DbHelper.cs needs DI refactor before MVC migration

CONTACTS:
  Dev lead:   J.Thompson  (ext 4421)
  DBA:        D.Kowalski  (ext 4387)
  IT Security: M.Rodriguez (ext 4501)

Last full deployment to production: 2013-10-15 by J.Thompson
Last DB schema change: 2013-09-02 (added BK_Branches table)
