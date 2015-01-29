
namespace DB.Abstraction
{


    partial class cOracle : cDAL
    {

        protected System.Data.OracleClient.OracleConnection m_SqlConnection;
        protected System.Data.OracleClient.OracleConnectionStringBuilder m_ConnectionString;


#if false
        public void databaseFilePut(string varFilePath)
        {
            byte[] file;
            using (System.IO.Stream stream = new System.IO.FileStream(varFilePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
            {
                using (System.IO.BinaryReader reader = new System.IO.BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                }
            }

            using (System.Data.IDbCommand cmd = this.CreateCommand("INSERT INTO Raporty (RaportPlik) Values(@File)"))
            {
                this.AddParameter(cmd, "@file", file);
                //sqlWrite.Parameters.Add("@File", System.Data.SqlDbType.VarBinary, file.Length).Value = file;
                this.ExecuteNonQuery(cmd);
            }
        }
#endif 


        // https://social.msdn.microsoft.com/Forums/en-US/dc1b053d-f0d5-48f8-ad82-fb6d96d27f80/how-to-insert-or-update-blob-data-for-sqlserver-based-on-buffersize?forum=adodotnetdataproviders
        public void WriteLargeFileChunckedOracle(string path)
        {
            byte[] buffer = new byte[1024];
            using (System.Data.OracleClient.OracleCommand cmd = null)
            {
                using (System.Data.OracleClient.OracleDataReader dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                {
                    // this code segment works well for Oracle database
                    System.Data.OracleClient.OracleLob lob = dataReader.GetOracleLob(1);
                    using (System.IO.Stream fileStream = System.IO.File.OpenRead(path))
                    {
                        lob.SetLength(0);

                        int readBytes = 0;
                        while ((readBytes = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            lob.Write(buffer, 0, readBytes);
                        } // Whend 

                    } // End Using fileStream
                    
                } // End Using dataReader

            } // End Using cmd
        } // End Sub WriteLargeFileChunckedOracle




        public override bool IsOracle
        {
            get { return true; }
        }


        public cOracle()
            : this("")
        {
            // Crap !
        } // End Constructor


        public cOracle(string strConnectionString)
        {
            //this.m_DatabaseConfiguration = dbcDBconfig;
            this.m_dbtDBtype = DataBaseEngine_t.Oracle;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 11.0;
            this.m_ConnectionString = new System.Data.OracleClient.OracleConnectionStringBuilder(strConnectionString);

            this.m_SqlConnection = new System.Data.OracleClient.OracleConnection(strConnectionString);
        } // End Constructor 2


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder()
        {
            return new OracleUniversalConnectionStringBuilder();
        }


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder(string connectionString)
        {
            return new OracleUniversalConnectionStringBuilder(connectionString);
        }


        public override string GetConnectionString(string strDb)
        {
            return this.m_ConnectionString.ConnectionString; ;
        } // End Function GetConnectionString


        public override bool SelfTest()
        {
            return this.ExecuteScalar<bool>("SELECT CAST('true' AS bit) AS res FROM dual; ");
        }


        // http://www.orafaq.com/faq/difference_between_truncate_delete_and_drop_commands
        public void RestoreDroppedTable(string strTableName)
        {
            strTableName = strTableName.Replace("'", "''");

            // FLASHBACK TABLE emp TO BEFORE DROP;
            string strSQL = "FLASHBACK TABLE " + strTableName + " TO BEFORE DROP;";

            this.Execute(strSQL);
        }


        public System.Data.Common.DbProviderFactory GetFactory()
        {
            System.Data.Common.DbProviderFactory providerFactory = null;
            
            providerFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.OracleClient");
            return providerFactory;
        } // End Function GetFactory


        public override System.Data.IDbConnection GetConnection()
        {
            System.Data.OracleClient.OracleConnection orcon = new System.Data.OracleClient.OracleConnection(m_ConnectionString.ConnectionString);

            return orcon;
        } // End Function GetConnection


        public override System.Data.DataTable GetEntireTable(string strTableName)
        {
            return GetDataTable("SELECT * FROM \"" + strTableName.Replace("'", "''") + "\" ");
        } // End Function GetEntireTable


    } // End Class cOracle


} // End Namespace DataBase.Tools
