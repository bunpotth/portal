using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;

namespace BankoneLegacyPortal
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            SeedDatabase();
        }

        // Application_AuthenticateRequest: restore role from FormsAuthentication ticket userData
        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity.IsAuthenticated)
            {
                FormsIdentity id = HttpContext.Current.User.Identity as FormsIdentity;
                if (id != null)
                {
                    string[] roles = id.Ticket.UserData.Split(',');
                    HttpContext.Current.User = new GenericPrincipal(id, roles);
                }
            }
        }

        // Dumps full exception detail to response in production - BANKLB-892
        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();
            if (ex != null)
            {
                System.Diagnostics.Debug.WriteLine("[Bankone] Application Error: " + ex.Message);
                // TODO: Replace with proper logging - BANKLB-892
                // Response.Write("<pre>" + HttpUtility.HtmlEncode(ex.ToString()) + "</pre>");
                // Server.ClearError();
            }
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Force session timeout - also set in Web.config but teams have override here
            // NOTE: This runs even for unauthenticated requests (BANKLB-777)
            Session.Timeout = 20;
        }

        // Runs on every request including static files - performance concern (BANKLB-503)
        void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Session != null
                && HttpContext.Current.Session["_Init"] == null)
            {
                HttpContext.Current.Session["_Init"] = true;
                HttpContext.Current.Session["ServerTime"] = DateTime.Now.ToString();
            }
        }

        private void SeedDatabase()
        {
            string connStr = ConfigurationManager.ConnectionStrings["BankoneDB"].ConnectionString;
            try
            {
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    CreateTablesIfNotExist(conn);

                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(*) FROM BK_Users", conn);
                    int userCount = (int)checkCmd.ExecuteScalar();
                    if (userCount == 0)
                        InsertSeedData(conn);
                }
            }
            catch (Exception ex)
            {
                // Silently swallow - DB may not be ready yet - BANKLB-441
                System.Diagnostics.Debug.WriteLine("[Bankone] DB seed failed: " + ex.Message);
            }
        }

        private void CreateTablesIfNotExist(SqlConnection conn)
        {
            string ddl = @"
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BK_Users' AND xtype='U')
                CREATE TABLE BK_Users (
                    UserId       INT IDENTITY(1,1) PRIMARY KEY,
                    Username     NVARCHAR(50)  NOT NULL,
                    PasswordHash NVARCHAR(32)  NOT NULL,
                    FullName     NVARCHAR(100) NULL,
                    Email        NVARCHAR(100) NULL,
                    AccountNumber NVARCHAR(20) NULL,
                    Role         NVARCHAR(20)  NOT NULL DEFAULT 'Customer'
                );
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BK_Accounts' AND xtype='U')
                CREATE TABLE BK_Accounts (
                    AccountId    INT IDENTITY(1,1) PRIMARY KEY,
                    AccountNumber NVARCHAR(20) NOT NULL,
                    AccountType  NVARCHAR(30)  NULL,
                    Balance      DECIMAL(18,2) NOT NULL DEFAULT 0,
                    UserId       INT           NULL,
                    OpenedDate   DATETIME      NOT NULL DEFAULT GETDATE()
                );
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BK_Transactions' AND xtype='U')
                CREATE TABLE BK_Transactions (
                    TransactionId   INT IDENTITY(1,1) PRIMARY KEY,
                    AccountId       INT           NULL,
                    TransactionDate DATETIME      NOT NULL DEFAULT GETDATE(),
                    Description     NVARCHAR(200) NULL,
                    Amount          DECIMAL(18,2) NOT NULL DEFAULT 0,
                    TransactionType NVARCHAR(10)  NULL
                );
                IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='BK_Branches' AND xtype='U')
                CREATE TABLE BK_Branches (
                    BranchId   INT IDENTITY(1,1) PRIMARY KEY,
                    BranchName NVARCHAR(100) NULL,
                    Address    NVARCHAR(200) NULL,
                    Phone      NVARCHAR(20)  NULL,
                    Hours      NVARCHAR(100) NULL
                );";

            new SqlCommand(ddl, conn).ExecuteNonQuery();
        }

        private void InsertSeedData(SqlConnection conn)
        {
            string adminHash = ComputeMd5("Admin@1234");
            string pass1Hash = ComputeMd5("Password1");

            string users = string.Format(@"
                INSERT INTO BK_Users (Username, PasswordHash, FullName, Email, AccountNumber, Role) VALUES
                ('admin',        '{0}', 'System Administrator', 'admin@bankone.com',         'BK-00000', 'Admin'),
                ('alice.mercer', '{1}', 'Alice Mercer',         'alice.mercer@email.com',    'BK-00142', 'Customer'),
                ('bob.tran',     '{1}', 'Bob Tran',             'bob.tran@email.com',        'BK-00287', 'Customer'),
                ('carol.simms',  '{1}', 'Carol Simms',          'carol.simms@email.com',     'BK-00391', 'Customer');",
                adminHash, pass1Hash);
            new SqlCommand(users, conn).ExecuteNonQuery();

            string accounts = @"
                INSERT INTO BK_Accounts (AccountNumber, AccountType, Balance, UserId, OpenedDate) VALUES
                ('BK-00142-CHK', 'Checking',  4823.17, 2, '2008-03-14'),
                ('BK-00142-SAV', 'Savings',  12400.00, 2, '2008-03-14'),
                ('BK-00287-CHK', 'Checking',  1204.88, 3, '2009-07-22'),
                ('BK-00287-SAV', 'Savings',   3300.50, 3, '2009-07-22'),
                ('BK-00391-CHK', 'Checking',   789.32, 4, '2010-11-05'),
                ('BK-00391-SAV', 'Savings',  22000.00, 4, '2010-11-05');";
            new SqlCommand(accounts, conn).ExecuteNonQuery();

            string txn = @"
                INSERT INTO BK_Transactions (AccountId, TransactionDate, Description, Amount, TransactionType) VALUES
                (1,'2013-10-01','Direct Deposit - Employer',    3200.00,'CR'),
                (1,'2013-10-03','ATM Withdrawal - Main St',      200.00,'DR'),
                (1,'2013-10-05','Electric Bill - City Power',    124.50,'DR'),
                (1,'2013-10-08','Transfer from Savings',         500.00,'CR'),
                (1,'2013-10-10','Grocery Store - FreshMart',      87.23,'DR'),
                (1,'2013-10-14','Online Transfer - Utilities',   150.00,'DR'),
                (1,'2013-10-15','Direct Deposit - Employer',   3200.00,'CR'),
                (1,'2013-10-18','Phone Bill - TeleNet',           65.00,'DR'),
                (1,'2013-10-22','Gas Station - PetroMart',        52.00,'DR'),
                (1,'2013-10-25','Restaurant - The Grill',         34.75,'DR'),
                (2,'2013-10-01','Monthly Interest',               12.40,'CR'),
                (2,'2013-10-08','Transfer to Checking',          500.00,'DR'),
                (2,'2013-10-15','Monthly Interest',               12.40,'CR'),
                (3,'2013-10-01','Direct Deposit - Employer',   2500.00,'CR'),
                (3,'2013-10-04','Rent Payment - Eastview Apts', 900.00,'DR'),
                (3,'2013-10-07','ATM Withdrawal',                100.00,'DR'),
                (3,'2013-10-10','Online Purchase - TechStore',    45.99,'DR'),
                (3,'2013-10-15','Direct Deposit - Employer',   2500.00,'CR');";
            new SqlCommand(txn, conn).ExecuteNonQuery();

            string branches = @"
                INSERT INTO BK_Branches (BranchName, Address, Phone, Hours) VALUES
                ('Bankone Downtown Hub',     '100 Commerce St, Dallas, TX 75201',      '(214) 555-0100', 'Mon-Fri 9AM-5PM, Sat 9AM-1PM'),
                ('Bankone Westside Branch',  '450 Park Ave W, Dallas, TX 75204',       '(214) 555-0142', 'Mon-Fri 9AM-6PM, Sat 10AM-2PM'),
                ('Bankone Northpark Center', '8687 Park Ln, Dallas, TX 75231',         '(214) 555-0287', 'Mon-Fri 10AM-7PM, Sat 10AM-4PM'),
                ('Bankone Lakewood Office',  '6301 Gaston Ave, Dallas, TX 75214',      '(214) 555-0391', 'Mon-Fri 9AM-5PM');";
            new SqlCommand(branches, conn).ExecuteNonQuery();
        }

        private static string ComputeMd5(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hash) sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
