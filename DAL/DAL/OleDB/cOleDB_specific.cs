
#if !__MonoCS__

namespace DB.Abstraction
{

    partial class cOleDB : cDAL
    {

        protected System.Data.OleDb.OleDbConnection m_SqlConnection;
        protected System.Data.OleDb.OleDbConnectionStringBuilder m_ConnectionString;


        public override bool IsOleDb
        {
            get { return true; }
        }


        public cOleDB()
            : this("")
        {
            // Crap !
        } // End Constructor


        public cOleDB(string strConnectionString)
        {
            //this.m_DatabaseConfiguration = dbcDBconfig;
            this.m_dbtDBtype = DataBaseEngine_t.OleDB;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 14.0;
            this.m_ConnectionString = new System.Data.OleDb.OleDbConnectionStringBuilder();
            this.m_SqlConnection = new System.Data.OleDb.OleDbConnection(strConnectionString);
        } // End Constructor 2


        public override string GetConnectionString()
        {
            throw new System.NotImplementedException("cOleDB_specific.cs ==> GetConnectionString");

            /*
            if (m_DatabaseConfiguration.bEmbeddedDatabase)
                m_ConnectionString.ServerType = FirebirdSql.Data.FirebirdClient.FbServerType.Embedded;
            else
                m_ConnectionString.ServerType = FirebirdSql.Data.FirebirdClient.FbServerType.Default;
            */

            //this.m_ConnectionString.DataSource = m_DatabaseConfiguration.strInstanceName;


            /*
            if (m_DatabaseConfiguration.iPort <= 0)
                this.m_ConnectionString.Port = 3050; // Default Port
            else
                this.m_ConnectionString.Port = m_DatabaseConfiguration.iPort;
            */

            //this.m_ConnectionString.DataSource = m_DatabaseConfiguration.strServerName;
            //this.m_ConnectionString.DataSource = m_DatabaseConfiguration.strInitialCatalog; // ???
            //this.m_ConnectionString.PersistSecurityInfo = false;
            //this.m_ConnectionString.Provider = m_DatabaseConfiguration.strProvider;
            //this.m_ConnectionString.OleDbServices = m_DatabaseConfiguration.iOleDbServices;
            //this.m_ConnectionString.FileName = "fooo"; // m_DatabaseConfiguration.strInitialCatalog; // ???

            //m_ConnectionString.IntegratedSecurity = m_DatabaseConfiguration.bIntegratedSecurity;
            //m_ConnectionString.UserID = m_DatabaseConfiguration.strUserName;
            //m_ConnectionString.Password = DB.Abstraction.cDAL.SecureString2String(m_DatabaseConfiguration.ssSecurePassword);

            //m_ConnectionString.ConnectionTimeout = m_DatabaseConfiguration.iConnectionTimeout;


            //Console.WriteLine(m_ConnectionString.ConnectionString);
            // return m_ConnectionString.ConnectionString; ;
        } // End Function GetConnectionString


        public override string GetConnectionString(string strDb)
        {
            return this.m_ConnectionString.ConnectionString;
        }


        public override void CreateTable(string strTableName, string strSQL)
        {
            if (TableExists(strTableName))
                DropTable(strTableName);

            ExecuteScripts(strSQL);
        }


        public System.Data.Common.DbProviderFactory GetFactory()
        {
            System.Data.Common.DbProviderFactory providerFactory = null;
            
            providerFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.OleDb");
            return providerFactory;
        }


        public override System.Data.IDbConnection GetConnection()
        {
            System.Data.OleDb.OleDbConnection oledbc = new System.Data.OleDb.OleDbConnection(m_ConnectionString.ConnectionString);

            return oledbc;
        }


        // BulkCopy("dbo.T_Benutzer", dt)
        public override bool BulkCopy(string strDestinationTable, System.Data.DataTable dt)
        {
            throw new System.NotImplementedException("cOleDB.BulkCopy not implemented.");
        } // End Sub BulkCopy


    } // End Class cOleDB


} // End Namespace DataBase.Tools

#endif
