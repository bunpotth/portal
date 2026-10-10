# Bankone Legacy Corporate Portal

A legacy ASP.NET WebForms corporate banking portal built to demonstrate the **AiDM Brownfield modernization workflow** for upgrading aging .NET Framework applications to modern .NET.

---

## Overview

This repository contains a purpose-built legacy codebase that authentically replicates the patterns, anti-patterns, and technical debt found in real enterprise banking portals from the early 2010s. It serves as the **"before"** artifact in an ASP.NET modernization demo.

| Property | Value |
|---|---|
| Framework | .NET Framework 4.5 |
| App type | ASP.NET WebForms |
| Authentication | FormsAuthentication (classic) |
| Data access | Hand-rolled ADO.NET, no ORM |
| UI | Bootstrap 3.1.1, jQuery 1.10.2 (CDN) |
| Database | SQL Server LocalDB v11.0 |
| Version | 2.3 (build 20131015) |

---

## Project Structure

```
BankoneLegacyPortal/
├── BankoneLegacyPortal.sln
└── BankoneLegacyPortal/
    ├── Web.config                    # debug=true, customErrors=Off, cleartext connstr
    ├── Global.asax.cs                # DB seeder, stack-trace dumper, BeginRequest on all reqs
    ├── Site.Master                   # Navy/gold Bootstrap 3 navbar, table-based footer
    ├── App_Code/DbHelper.cs          # Static helper: no DI, no using blocks, MD5, SQL injection
    ├── Default.aspx                  # Dashboard — UpdatePanel widgets + Timer
    ├── About.aspx                    # Bank about page
    ├── Account/                      # Login · Register · ChangePassword
    ├── Accounts/
    │   ├── Summary.aspx              # GridView + SqlDataSource, ViewState ON
    │   ├── Transactions.aspx         # DataList + inline SQL date filter
    │   └── Transfer.aspx             # MultiView session wizard, no CSRF token
    ├── Branch/
    │   └── Locator.aspx              # DataSet/DataAdapter, Response.Write DB errors
    └── Admin/
        └── UserManagement.aspx       # GridView RowCommand, inline DELETE SQL
```

---

## Getting Started

### Prerequisites

- Visual Studio 2013 or later
- .NET Framework 4.5
- SQL Server LocalDB v11.0 (included with VS 2013)

### Run Locally

1. Open `BankoneLegacyPortal/BankoneLegacyPortal.sln` in Visual Studio
2. Press **F5** — the LocalDB database is created and seeded automatically on first run
3. Navigate to `http://localhost:50000/`

### Default Credentials

| Role | Username | Password |
|---|---|---|
| Admin | `admin` | `Admin@1234` |
| Customer | `alice.mercer` | `Password1` |
| Customer | `bob.tran` | `Password1` |
| Customer | `carol.simms` | `Password1` |

---

## Legacy Anti-Patterns (Modernization Targets)

Each page is deliberately built with a specific anti-pattern that maps to an AiDM Brownfield modernization pain point:

| File | Legacy Pattern | Modernization Target |
|---|---|---|
| `App_Code/DbHelper.cs` | Static class, no DI, no `using` blocks, MD5 passwords, SQL injection | DI + `IDbConnectionFactory` + bcrypt |
| `Accounts/Summary.aspx` | `SqlDataSource` + `GridView`, ViewState ON | Repository pattern + Razor component |
| `Accounts/Transactions.aspx` | `DataList` + inline SQL string concatenation | EF Core + typed models + minimal API |
| `Accounts/Transfer.aspx` | `MultiView` session wizard, no CSRF token | Stateless form + antiforgery middleware |
| `Branch/Locator.aspx` | `DataSet`/`DataAdapter`, `Response.Write` error dumps | Typed models + `ILogger` + middleware |
| `Admin/UserManagement.aspx` | `GridView` `RowCommand`, inline `DELETE` SQL, no transactions | EF Core + authorization policy |
| `Global.asax.cs` | `Application_Error` exposes stack traces, `BeginRequest` on static files | Middleware pipeline + structured logging |
| `Web.config` | `debug=true`, `customErrors=Off`, cleartext connection string | `appsettings.json` + environment secrets |

### Known Issues (Open JIRA Tickets)

- **BANKLB-334** — SqlConnection not disposed in `DbHelper.ExecuteQuery` (connection pool exhaustion under load)
- **BANKLB-445** — CSRF protection missing on Transfer page
- **BANKLB-510** — Fund transfer uses two separate UPDATEs with no DB transaction (data integrity risk)
- **BANKLB-512** — Race condition in `Register.aspx` account number generation
- **BANKLB-778** — MD5 password hashing — upgrade to bcrypt *(open since 2011)*
- **BANKLB-892** — `Application_Error` and `customErrors=Off` expose stack traces in production

---

## Seed Data

The database is auto-seeded on `Application_Start` with:

- **4 users** — 1 Admin + 3 customers (Alice Mercer, Bob Tran, Carol Simms)
- **6 accounts** — Checking + Savings per customer
- **18 transactions** across accounts (October 2013)
- **4 branch locations** in Dallas, TX

---

## Design Specification

Full design document: [`docs/superpowers/specs/2026-10-10-bankone-legacy-portal-design.md`](docs/superpowers/specs/2026-10-10-bankone-legacy-portal-design.md)
