
namespace DB.Abstraction
{

    partial class cFireBird : cDAL
    {

        protected FirebirdSql.Data.FirebirdClient.FbConnection m_SqlConnection;
        protected FirebirdSql.Data.FirebirdClient.FbConnectionStringBuilder m_ConnectionString;
        

        public override bool IsFireBird
        {
            get { return true; }
        }


        public cFireBird()
            : this("")
        {
            // Crap ! 
        } // End Constructor


        public cFireBird(string strConnectionString)
        {
            this.m_dbtDBtype = DataBaseEngine_t.FireBird;
            this.m_dblDBversion = 2.5;
            this.m_providerFactory = this.GetFactory();
            //this.m_DatabaseConfiguration = new DataBase.Tools.cDataBaseConfiguration();
            this.m_ConnectionString = new FirebirdSql.Data.FirebirdClient.FbConnectionStringBuilder(strConnectionString);
            this.m_SqlConnection = new FirebirdSql.Data.FirebirdClient.FbConnection(strConnectionString);
        }


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder()
        {
            return new FirebirdUniversalConnectionStringBuilder();
        }


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder(string connectionString)
        {
            return new FirebirdUniversalConnectionStringBuilder(connectionString);
        }


        public override string GetConnectionString(string strDb)
        {
			if(string.IsNullOrEmpty(strDb) || string.IsNullOrEmpty( strDb.Trim()))
				return this.m_ConnectionString.ConnectionString;

            //if (m_DatabaseConfiguration.bEmbeddedDatabase)
            //    m_ConnectionString.ServerType = FirebirdSql.Data.FirebirdClient.FbServerType.Embedded;
            //else
            //    m_ConnectionString.ServerType = FirebirdSql.Data.FirebirdClient.FbServerType.Default;

            if (this.m_ConnectionString.Port <= 0)
                this.m_ConnectionString.Port = 3050; // Default Port

            //Console.WriteLine(m_ConnectionString.ConnectionString);
            return m_ConnectionString.ConnectionString; ;
        } // End Function GetConnectionString


        public override bool SelfTest()
        {
            return this.ExecuteScalar<bool>("SELECT CAST('true' AS bit) AS res FROM RDB$DATABASE; ");
        }


        public System.Data.Common.DbProviderFactory GetFactory()
        {
            //AddFactoryClasses();
            System.Data.Common.DbProviderFactory providerFactory = null;
            //public FirebirdSql.Data.FirebirdClient.FirebirdClientFactory fbc = null;
            providerFactory = this.GetFactory(typeof(FirebirdSql.Data.FirebirdClient.FirebirdClientFactory));
            //providerFactory = System.Data.Common.DbProviderFactories.GetFactory("FirebirdSql.Data.FirebirdClient");
            return providerFactory;
        }


        public override System.Data.IDbConnection GetConnection()
        {
            FirebirdSql.Data.FirebirdClient.FbConnection fbc = new FirebirdSql.Data.FirebirdClient.FbConnection(m_ConnectionString.ConnectionString);

            return fbc;
        }


        public override System.Data.DataTable GetEntireTable(string strTableName)
        {
            return GetDataTable("SELECT * FROM \"" + strTableName.Replace("'", "''") + "\" ");
        }


        // BulkCopy("dbo.T_Benutzer", dt)
        public override bool BulkCopy(string strDestinationTable, System.Data.DataTable dt)
        {
            throw new System.NotImplementedException("cFireBird.BulkCopy not implemented.");
        } // End Sub BulkCopy


        public override System.Data.IDbCommand CreatePagedCommand(string strSQL, ulong ulngPageNumber, ulong ulngPageSize)
        {
            ulong ulngStartIndex = ((ulngPageSize * ulngPageNumber) - ulngPageSize) + 1;
            ulong ulngEndIndex = ulngStartIndex + ulngPageSize - 1;


            strSQL = this.RemoveCstyleComments(strSQL);
            strSQL = this.RemoveSingleLineSqlComments(strSQL);

            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("SELECT", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            strSQL = r.Replace(strSQL, "SELECT FIRST " + ulngPageSize.ToString() + " SKIP " + ulngStartIndex.ToString() + " ", 1);
            r = null;

            System.Data.IDbCommand cmd = this.CreateCommand(strSQL);

            this.AddParameter(cmd, "ulngStartIndex", ulngStartIndex);
            this.AddParameter(cmd, "ulngEndIndex", ulngEndIndex);
            return cmd;
        } // End Function CreatePagedCommand




    } // End Class cFirebird


} // End Namespace DataBase.Tools
