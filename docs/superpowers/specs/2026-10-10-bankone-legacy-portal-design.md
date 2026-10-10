# Bankone Legacy Corporate Portal — Design Spec

**Date:** 2026-10-10  
**Purpose:** Demonstrate ASP.NET modernization with AiDM Brownfield workflow

---

## Approach

New standalone project at `BankoneLegacyPortal/` — self-contained, no changes to existing samples.

## Stack

- .NET Framework 4.5, ASP.NET WebForms
- Bootstrap 3.1.1 (CDN), jQuery 1.10.2 (CDN)
- FormsAuthentication (not OWIN/Identity)
- SQL Server LocalDB v11.0, hand-rolled ADO.NET (no EF)

## Structure

```
BankoneLegacyPortal/
├── BankoneLegacyPortal.sln
└── BankoneLegacyPortal/
    ├── BankoneLegacyPortal.csproj
    ├── Web.config / Global.asax / Site.Master
    ├── Default.aspx / About.aspx
    ├── App_Code/DbHelper.cs
    ├── Content/Site.css
    ├── Account/  (Login, Register, ChangePassword)
    ├── Accounts/ (Summary, Transactions, Transfer)
    ├── Branch/   (Locator)
    └── Admin/    (UserManagement)
```

## Branding

- Navy (#003366) / Gold (#C8960C) / Off-white (#F5F5F0)
- "BANKONE — Your Trusted Banking Partner Since 1987"
- Footer: © 2009 Bankone Corporation. Member FDIC. (deliberately stale)

## Legacy Anti-Patterns (per page)

| Page | Pattern | Modernization Signal |
|---|---|---|
| Accounts/Summary | GridView + SqlDataSource, ViewState ON | → Repository + Razor |
| Accounts/Transactions | DataList + inline SQL strings | → EF Core + minimal API |
| Accounts/Transfer | MultiView session wizard, no CSRF | → Stateless form + antiforgery |
| Branch/Locator | DataSet/DataAdapter, Response.Write debug | → Typed models + ILogger |
| Admin/UserManagement | GridView RowCommand, inline DELETE SQL | → EF Core + authorization policy |
| App_Code/DbHelper | Static class, no DI, no using blocks, MD5 | → DI + IDbConnectionFactory |
| Global.asax | Application_Error dumps stack trace | → Middleware + structured logging |
| Web.config | Cleartext connstr, debug=true, customErrors=Off | → appsettings.json + env secrets |

## DB Schema

- BK_Users (UserId, Username, PasswordHash MD5, FullName, Email, AccountNumber, Role)
- BK_Accounts (AccountId, AccountNumber, AccountType, Balance, UserId, OpenedDate)
- BK_Transactions (TransactionId, AccountId, TransactionDate, Description, Amount, TransactionType)
- BK_Branches (BranchId, BranchName, Address, Phone, Hours)

Seeded: admin + 3 customers (alice.mercer, bob.tran, carol.simms), accounts, 10+ transactions, 4 branches.

## Auth

FormsAuthentication with role stored in ticket userData. Application_AuthenticateRequest sets GenericPrincipal for role checks. MD5 password hashing (intentionally insecure).

## Known Issues (embedded as BANKLB- tickets)

BANKLB-334 (connection leaks), BANKLB-445 (CSRF missing), BANKLB-512 (race condition), BANKLB-778 (MD5→bcrypt open since 2011), BANKLB-892 (stack traces in production).
