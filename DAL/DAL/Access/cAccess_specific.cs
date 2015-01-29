
namespace DB.Abstraction
{

    partial class cAccess : cDAL
    {


        protected System.Data.Odbc.OdbcConnectionStringBuilder m_ConnectionString;
        

        public override bool IsMsAccess
        {
            get { return true; }
        }


        public cAccess()
            : this("")
        {
        } // End Constructor



        public cAccess(string strConnectionString)
        {
            //this.m_DatabaseConfiguration = dbcDBconfig;
            this.m_dbtDBtype = DataBaseEngine_t.Access;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 0.0;
            this.m_ConnectionString = new System.Data.Odbc.OdbcConnectionStringBuilder();
        } // End Constructor 2


        public override string GetConnectionString(string strDb)
        {
            //this.m_ConnectionString.DataSource = m_DatabaseConfiguration.strServerName;
            //this.m_ConnectionString.DataSource = m_DatabaseConfiguration.strInstanceName;
            //if (m_DatabaseConfiguration.iPort <= 0)
            //    throw new Exception("Ports <= 0 are invalid.");

            

            this.m_ConnectionString.Driver = "{Microsoft Access Driver (*.mdb)}";
            this.m_ConnectionString.Add("Dbq", (string)this.m_ConnectionString["strInitialCatalog"]);



            //System.Windows.Forms.MessageBox.Show(this.m_ConnectionString.ConnectionString);


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
            return GetDataTable("SELECT * FROM " + strTableName.Replace("'", "''") + "");
        }


    } // End Class cAccess


} // End Namespace DataBase.Tools
