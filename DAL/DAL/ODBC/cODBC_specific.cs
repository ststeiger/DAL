
namespace DB.Abstraction
{

    partial class cODBC : cDAL
    {

        protected System.Data.Odbc.OdbcConnection m_SqlConnection;
        protected System.Data.Odbc.OdbcConnectionStringBuilder m_ConnectionString;
        

        public override bool IsOdbc
        {
            get { return true; }
        }


        public cODBC()
            : this("")
        {
            // Crap ! 
        } // End Constructor


        public cODBC(string strConnectionString)
        {
            //this.m_DatabaseConfiguration = dbcDBconfig;
            this.m_dbtDBtype = DataBaseEngine_t.ODBC;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 0.0;
            this.m_ConnectionString = new System.Data.Odbc.OdbcConnectionStringBuilder();

            this.m_SqlConnection = new System.Data.Odbc.OdbcConnection(strConnectionString);
        } // End Constructor 2


        public override string GetConnectionString(string strDb)
        {
            //this.m_ConnectionString.DataSource = m_DatabaseConfiguration.strServerName;
            //this.m_ConnectionString.DataSource = m_DatabaseConfiguration.strInstanceName;
            // if (m_DatabaseConfiguration.iPort <= 0) throw new Exception("Ports <= 0 are invalid.");



            //this.m_ConnectionString.Port = 3050; // m_DatabaseConfiguration.iPort;
            //this.m_ConnectionString.Database = m_DatabaseConfiguration.strInitialCatalog;

            //m_ConnectionString.IntegratedSecurity = m_DatabaseConfiguration.bIntegratedSecurity;
            //m_ConnectionString.UserID = m_DatabaseConfiguration.strUserName;
            //m_ConnectionString.Password = DB.Abstraction.cDAL.SecureString2String(m_DatabaseConfiguration.ssSecurePassword);

            //m_ConnectionString.ConnectionTimeout = m_DatabaseConfiguration.iConnectionTimeout;


            //Console.WriteLine(m_ConnectionString.ConnectionString);
            return m_ConnectionString.ConnectionString; ;
        } // End Function GetConnectionString


        public System.Data.Common.DbProviderFactory GetFactory()
        {
            System.Data.Common.DbProviderFactory providerFactory = null;
            
            providerFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.Odbc");
            return providerFactory;
        }


        public override System.Data.IDbConnection GetConnection()
        {
            System.Data.Odbc.OdbcConnection odbcc = new System.Data.Odbc.OdbcConnection(m_ConnectionString.ConnectionString);

            return odbcc;
        }


        public override System.Data.DataTable GetEntireTable(string strTableName)
        {
            return GetDataTable("SELECT * FROM \"" + strTableName.Replace("'", "''") + "\" ");
        }


    } // End Class cODBC


} // End Namespace DataBase.Tools
