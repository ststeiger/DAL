
// http://stackoverflow.com/questions/1429898/from-a-sybase-database-how-i-can-get-table-description-field-names-and-types
namespace DB.Abstraction
{


    public abstract class cDAL
    {

        // http://stackoverflow.com/questions/1244953/internal-abstract-class-how-to-hide-usage-outside-assembly
        // Thy shall implement providers inside this assembly
        internal cDAL()
        {
        }

        public bool m_bLogDetails = true;


        public enum DataBaseEngine_t : int
        {
            MS_SQL = 0,
            Oracle = 1,
            MySQL = 2,
            PostGreSQL = 3,
            FireBird = 4,
            Sybase_ASE = 5,
            Sybase_ASA = 6,
            Access = 7,
            SQLite = 7,
            OleDB = 996,
            ODBC = 997,
            Generic = 998,
            Not_Supported = 999
        } // End Enum DataBaseEngine_t


        protected DataBaseEngine_t m_dbtDBtype = DataBaseEngine_t.Not_Supported;
        protected double m_dblDBversion = 1.0;
        protected System.Data.Common.DbProviderFactory m_providerFactory = null;


        public virtual UniversalConnectionStringBuilder NewConnectionStringBuilder()
        {
            return new GenericUniversalConnectionStringBuilder();
        }


        public virtual UniversalConnectionStringBuilder NewConnectionStringBuilder(string connectionString)
        {
            return new GenericUniversalConnectionStringBuilder(connectionString);
        }


        public virtual bool SelfTest()
        {
            return this.ExecuteScalar<bool>("SELECT CAST('true' AS bit) AS res; ");
        }


        public virtual bool IsMsAccess
        {
            get { return false; }
        }


        public virtual bool IsFireBird
        {
            get { return false; }
        }


        public virtual bool IsSQLite
        {
            get { return false; }
        }


        public virtual bool IsMsSql
        {
            get { return false; }
        }


        public virtual bool IsMySql
        {
            get { return false; }
        }


        public virtual bool IsOdbc
        {
            get { return false; }
        }


        public virtual bool IsOleDb
        {
            get { return false; }
        }


        public virtual bool IsOracle
        {
            get { return false; }
        }


        public virtual bool IsPostGreSql
        {
            get { return false; }
        }


        public virtual bool IsSqLite
        {
            get { return false; }
        }


        public virtual bool IsSybase
        {
            get { return false; }
        }


        protected NET20_Substitute.Generics.HashSet<string> m_ReservedKeywords;
        protected object m_objReservedKeywordsLock = new object();

        public NET20_Substitute.Generics.HashSet<string> ReservedKeywords
        {
            get
            {
                if (m_ReservedKeywords != null)
                    return m_ReservedKeywords;

                lock (m_objReservedKeywordsLock)
                {
                    if (m_ReservedKeywords == null)
                        m_ReservedKeywords = GetReservedKeywords();
                }

                return m_ReservedKeywords;
            }
        }

        protected NET20_Substitute.Generics.HashSet<string> m_CurrentKeywords;
        protected object m_objCurrentKeywordsLock = new object();

        public NET20_Substitute.Generics.HashSet<string> CurrentKeywords
        {
            get
            {
                if (m_CurrentKeywords != null)
                    return m_ReservedKeywords;

                lock (m_objCurrentKeywordsLock)
                {
                    if (m_CurrentKeywords == null)
                        m_CurrentKeywords = GetCurrentKeywords();
                }

                return m_CurrentKeywords;
            }
        }

        protected NET20_Substitute.Generics.HashSet<string> m_FutureKeywords;
        protected object m_objFutureKeywordsLock = new object();

        public NET20_Substitute.Generics.HashSet<string> FutureKeywords
        {
            get
            {
                if (m_FutureKeywords != null)
                    return m_FutureKeywords;


                lock (m_objFutureKeywordsLock)
                {
                    if (m_FutureKeywords == null)
                        m_FutureKeywords = GetFutureKeywords();
                }

                return m_FutureKeywords;
            }
        }


        protected NET20_Substitute.Generics.HashSet<string> m_OdbcKeywords;
        protected object m_objOdbcKeywordsLock = new object();

        public NET20_Substitute.Generics.HashSet<string> OdbcKeywords
        {
            get
            {
                if (m_OdbcKeywords != null)
                    return m_OdbcKeywords;

                lock (m_objOdbcKeywordsLock)
                {
                    if (m_OdbcKeywords == null)
                        m_OdbcKeywords = GetOdbcKeywords();
                }

                return m_OdbcKeywords;
            }
        }


        public virtual bool IsReservedKeyword(string Keyword)
        {
            return ReservedKeywords.Contains(Keyword);
        }

        public virtual bool IsCurrentKeyword(string Keyword)
        {
            return CurrentKeywords.Contains(Keyword);
        }

        public virtual bool IsFutureKeyword(string Keyword)
        {
            return FutureKeywords.Contains(Keyword);
        }


        public virtual bool IsOdbcKeyword(string Keyword)
        {
            return OdbcKeywords.Contains(Keyword);
        }


        protected virtual NET20_Substitute.Generics.HashSet<string> GetReservedKeywords()
        {
            return new NET20_Substitute.Generics.HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
        }

        protected virtual NET20_Substitute.Generics.HashSet<string> GetCurrentKeywords()
        {
            return new NET20_Substitute.Generics.HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
        }

        protected virtual NET20_Substitute.Generics.HashSet<string> GetFutureKeywords()
        {
            return new NET20_Substitute.Generics.HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
        }
        protected virtual NET20_Substitute.Generics.HashSet<string> GetOdbcKeywords()
        {
            return new NET20_Substitute.Generics.HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
        }


        protected string m_DefaultSchema;
        public virtual string DefaultSchema
        {

            get
            {
                if (m_DefaultSchema != null)
                    return m_DefaultSchema;

                m_DefaultSchema = "dbo";
                return m_DefaultSchema;
            }

        } // End Property DefaultSchema 


        protected string m_DefaultCatalog;
        public virtual string DefaultDatabase
        {

            get
            {
                if (m_DefaultCatalog != null)
                    return m_DefaultCatalog;

                System.Data.Common.DbConnectionStringBuilder dbcsb = new System.Data.Common.DbConnectionStringBuilder();
                dbcsb.ConnectionString = this.GetConnectionString();

                m_DefaultCatalog = System.Convert.ToString(dbcsb["Initial Catalog"]);
                return m_DefaultCatalog;
            }

        } // End Property DefaultDatabase 


        public virtual void InsertUpdateTable(string TableName, System.Data.DataTable dt)
        {
            using (System.Data.IDbConnection idbc = GetConnection())
            {

                lock (idbc)
                {

                    using (System.Data.Common.DbCommand cmd = this.m_providerFactory.CreateCommand())
                    {

                        lock (cmd)
                        {
                            cmd.CommandText = "SELECT * FROM " + this.QuoteObjectWhereNecessary(TableName);

                            try
                            {
                                cmd.Connection = (System.Data.Common.DbConnection)idbc;

                                using (System.Data.Common.DbDataAdapter daQueryTable = this.m_providerFactory.CreateDataAdapter())
                                {
                                    daQueryTable.SelectCommand = cmd;

                                    using (System.Data.Common.DbCommandBuilder cb = this.m_providerFactory.CreateCommandBuilder())
                                    {
                                        cb.DataAdapter = daQueryTable;
                                        daQueryTable.SelectCommand = cmd;
                                        daQueryTable.InsertCommand = cb.GetInsertCommand();
                                        daQueryTable.UpdateCommand = cb.GetUpdateCommand();

                                        daQueryTable.Update(dt);
                                    }


                                } // End Using daQueryTable

                            } // End Try
                            catch (System.Data.Common.DbException ex)
                            {
                                //COR.Debug.MsgBox("Exception executing ExecuteInTransaction: " + ex.Message);
                                if (Log("cDAL.InsertUpdateTable", ex, cmd.CommandText))
                                    throw ex;
                            }// End Catch
                            finally
                            {
                                if (idbc.State != System.Data.ConnectionState.Closed)
                                    idbc.Close();
                            } // End Finally

                        } // End lock cmd

                    } // End Using System.Data.IDbCommand cmd 

                } // End lock idbc

            } // End Using idbc

            //return dt;

        } // End Sub InsertUpdateTable 



        protected static string strStaticConnectionString = null;

        // Requires reference to System.Configuration
        // http://stackoverflow.com/questions/6582970/separate-connectionstrings-and-mailsettings-from-web-config-possible
        public static string FetchConnectionString(string strIntitialCatalog)
        {
            string strReturnValue = null;
            string strProviderName = null;


            if (string.IsNullOrEmpty(strStaticConnectionString))
            {
                string strConnectionStringName = System.Environment.MachineName;

                if (string.IsNullOrEmpty(strConnectionStringName))
                {
                    strConnectionStringName = "LocalSqlServer";
                }



                // Walk through the collection and return the first 
                // connection string matching the connectionString name.
                System.Configuration.ConnectionStringSettingsCollection settings = System.Configuration.ConfigurationManager.ConnectionStrings;

                if ((settings != null))
                {
                    foreach (System.Configuration.ConnectionStringSettings cs in settings)
                    {
                        if (System.StringComparer.OrdinalIgnoreCase.Equals(cs.Name, strConnectionStringName))
                        {
                            strReturnValue = cs.ConnectionString;
                            strProviderName = cs.ProviderName;
                            break; // TODO: might not be correct. Was : Exit For
                        } // if (StringComparer.OrdinalIgnoreCase.Equals(cs.Name, strConnectionStringName)) 

                    } // Next cs

                } // End if ((settings != null))

                if (string.IsNullOrEmpty(strReturnValue))
                {
                    strConnectionStringName = "server";

                    System.Configuration.ConnectionStringSettings conString = System.Configuration.ConfigurationManager.ConnectionStrings[strConnectionStringName];

                    if (conString != null)
                    {
                        strReturnValue = conString.ConnectionString;
                    } // if (conString != null)

                } // End if (string.IsNullOrEmpty(strReturnValue))

                if (string.IsNullOrEmpty(strReturnValue))
                {
                    throw new System.ArgumentNullException("ConnectionString \"" + strConnectionStringName + "\" in file web.config xor separate file connections.config.");
                }

                settings = null;
                strConnectionStringName = null;
            }
            else
            {
                if (string.IsNullOrEmpty(strIntitialCatalog))
                {
                    return strStaticConnectionString;
                }

                strReturnValue = strStaticConnectionString;
            }

            System.Data.Common.DbConnectionStringBuilder sb = new System.Data.Common.DbConnectionStringBuilder();
            sb.ConnectionString = strReturnValue;
            //InitFactory(strProviderName);
            //System.Data.Common.DbConnectionStringBuilder sb = GetConnectionStringBuilder(strReturnValue);


            if (string.IsNullOrEmpty(strStaticConnectionString))
            {

                if (!IEnumerableExtensions.Contains(sb.Keys, "Integrated Security") 
                    || !System.Convert.ToBoolean(sb["Integrated Security"])
                 )
                {
                    sb["Password"] = Tools.Cryptography.DES.DeCrypt(System.Convert.ToString(sb["Password"]));
                }

                strReturnValue = sb.ConnectionString;
                strStaticConnectionString = strReturnValue;
            } // End if (string.IsNullOrEmpty(strStaticConnectionString))


            if (!string.IsNullOrEmpty(strIntitialCatalog))
            {
                sb["Database"] = strIntitialCatalog;
            }


            strReturnValue = sb.ConnectionString;
            sb = null;

            return strReturnValue;
        } // FetchConnectionString


        public virtual bool isDataBaseConnectionOpen(System.Data.IDbConnection idbc)
        {
            if (idbc.State == System.Data.ConnectionState.Open)
                return true;

            return false;
        } // End Function isDataBaseConnectionOpen


        public virtual void OpenSQLConnection(System.Data.IDbConnection idbc)
        {
            //SQL-Connection String setzen mit TCP-Port
            if (idbc.State != System.Data.ConnectionState.Open)
            {

                try
                {
                    idbc.Open();

                    if (m_bLogDetails == true)
                        Log("Successfully created DB-connection.");
                } // End Try
                catch (System.Data.Common.DbException ex)
                {
                    if (Log("Error opening SQL-datatbase connection." 
                        + System.Environment.NewLine + "ConnectionString: " + idbc.ConnectionString 
                        + System.Environment.NewLine + ex.Message))
                        throw;
                } // End Catch

            } // End if (idbc.State != System.Data.ConnectionState.Open)

        } // End Sub OpenSQLConnection


        public virtual System.Data.IDbCommand CreateCommand()
        {
            return CreateCommand("");
        } // End Function CreateCommand

        /*
        public virtual System.Data.IDbCommand CreateCommand(string strSQL)
        {
            System.Data.IDbCommand idbc = this.m_providerFactory.CreateCommand();
            idbc.CommandText = strSQL;
			
            return idbc;
        } // End Function CreateCommand
        */

        public virtual System.Data.IDbCommand CreateCommand(string strSQL)
        {
            System.Data.IDbCommand idbc = this.m_providerFactory.CreateCommand();
            idbc.CommandTimeout = 300;
            idbc.CommandText = strSQL;

            return idbc;
        } // End Function CreateCommand


        public virtual System.Data.IDbCommand CreateLimitedCommand(string strSQL, int iLimit)
        {
            strSQL = string.Format(strSQL, "", "LIMIT " + iLimit.ToString());
            return CreateCommand(strSQL);
        }


        public virtual System.Data.IDbCommand CreateLimitedOdbcCommand(string strSQL, int iLimit)
        {
            strSQL = ReplaceOdbcFunctions(strSQL);
            return CreateLimitedCommand(strSQL, iLimit);
        }


        public System.Data.IDbCommand CreatePagedCommand(string strSQL, int ulngStartIndex, int ulngEndIndex)
        {
            return CreatePagedCommand(strSQL, (ulong)ulngStartIndex, (ulong)ulngEndIndex);
        }


        public virtual System.Data.IDbCommand CreatePagedCommand(string strSQL, ulong ulngPageNumber, ulong ulngPageSize)
        {
            ulong ulngStartIndex = ((ulngPageSize * ulngPageNumber) - ulngPageSize) + 1;
            ulong ulngEndIndex = ulngStartIndex + ulngPageSize - 1;

            strSQL += " " + System.Environment.NewLine + "OFFSET " + ulngStartIndex.ToString() + " " + System.Environment.NewLine + "LIMIT " + ulngPageSize.ToString() + " ";

            //OFFSET @ulngStartIndex 
            //LIMIT (@ulngEndIndex - @ulngStartIndex) 

            System.Data.IDbCommand cmd = this.CreateCommand(strSQL);

            // fsck... {fn NOW()}
            // cmd.CommandText = string.Format(strSQL, " /* TOP 1 */ ", "OFFSET 0 FETCH NEXT 1 ROWS ONLY");

            this.AddParameter(cmd, "ulngStartIndex", ulngStartIndex);
            this.AddParameter(cmd, "ulngEndIndex", ulngEndIndex);
            return cmd;
        }

        public virtual System.Data.DataTable GetTypes()
        {
            throw new System.NotImplementedException("cDAL.GetTypes");
        }


        public virtual void xxx()
        {
            //Npgsql.NpgsqlParameter param = new Npgsql.NpgsqlParameter();
            System.Data.SqlClient.SqlParameter prm = new System.Data.SqlClient.SqlParameter();
            prm.SqlDbType = System.Data.SqlDbType.Udt;
            prm.UdtTypeName = "GEOMETRY";
        }


        public virtual System.Data.IDbCommand CommandFromScript(string strFileName)
        {
            string strSQL = GetEmbeddedSQLscript(strFileName);
            return CreateCommand(strSQL);
        } // End Function CommandFromScript


        public virtual System.Data.IDbCommand CreateCommandFromFile(string strFileName)
        {
            return CommandFromScript(strFileName);
        } // End Function CommandFromScript


        public virtual int Execute(System.Data.IDbCommand cmd)
        {
            int iAffected = -1;

            try
            {

                using (System.Data.IDbConnection idbConn = GetConnection())
                {

                    lock (idbConn)
                    {

                        lock (cmd)
                        {

                            cmd.Connection = idbConn;

                            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                cmd.Connection.Open();

                            using (System.Data.IDbTransaction idbtTrans = idbConn.BeginTransaction())
                            {

                                try
                                {
                                    cmd.Transaction = idbtTrans;

                                    iAffected = cmd.ExecuteNonQuery();
                                    idbtTrans.Commit();
                                } // End Try
                                catch (System.Data.Common.DbException ex)
                                {
                                    if (idbtTrans != null)
                                        idbtTrans.Rollback();

                                    iAffected = -2;

                                    if (Log(ex))
                                        throw;
                                } // End catch
                                finally
                                {
                                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                        cmd.Connection.Close();
                                } // End Finally

                            } // End Using idbtTrans

                        } // End lock cmd

                    } // End lock idbConn

                } // End Using idbConn

            } // End Try
            catch (System.Exception ex)
            {
                iAffected = -3;
                if (Log(ex, cmd))
                    throw;
            }
            finally
            {

            }

            return iAffected;
        } // End Function Execute


        public virtual int Execute(string strSQL)
        {
            int iReturnValue = -1;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                iReturnValue = Execute(cmd);
            }

            return iReturnValue;
        } // End Sub Execute


        public virtual System.Exception Execute(System.Data.IDbCommand cmd, bool bReturnError)
        {
            System.Exception exReturnValue = null;

            using (System.Data.IDbConnection idbc = GetConnection())
            {

                lock (idbc)
                {
                    // pfff, mono C# compiler problem...
                    //cmd = new System.Data.SqlClient.SqlCommand(strSQL, this.m_SqlConnection);

                    lock (cmd)
                    {

                        try
                        {
                            cmd.Connection = idbc;

                            if (idbc.State != System.Data.ConnectionState.Open)
                                idbc.Open();

                            cmd.ExecuteNonQuery();
                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            Log("cDAL.Execute(System.Data.IDbCommand cmd, bool bReturnError)", ex, cmd.CommandText, false);
                            exReturnValue = ex;
                        } // End Catch
                        finally
                        {
                            if (idbc.State != System.Data.ConnectionState.Closed)
                                idbc.Close();

                        } // End Finally

                    } // End Lock cmd

                } // Unlock idbc

            } // End Using idbc

            return exReturnValue;
        } // End Function Execute


        public virtual System.Exception Execute(string strSQL, bool bReturnError)
        {
            System.Exception exReturnValue = null;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                exReturnValue = Execute(cmd, bReturnError);
            } // End Using cmd

            return exReturnValue;
        } // End Function Execute


        public virtual void ExecuteGoSeparatedScript(string strAllScripts)
        {
            DAL.Scripting.ScriptSplitter MyScriptSplitter = new DAL.Scripting.ScriptSplitter(strAllScripts);
            foreach (string strScript in MyScriptSplitter)
            {
                ExecuteWithoutTransaction(strScript, 600);
            } // Next strScript
        }


        public virtual int ExecuteWithoutTransaction(string strSQL)
        {
            return ExecuteWithoutTransaction(strSQL, 30);
        }


        public virtual int ExecuteWithoutTransaction(string strSQL, int iTimeout)
        {
            int iReturnValue = -1;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                iReturnValue = ExecuteWithoutTransaction(cmd, iTimeout);
            }

            return iReturnValue;
        } // End Sub Execute


        public virtual int ExecuteWithoutTransaction(System.Data.IDbCommand cmd)
        {
            return ExecuteWithoutTransaction(cmd, 30);
        }


        public virtual int ExecuteWithoutTransaction(System.Data.IDbCommand cmd, int iTimeout)
        {
            int iAffected = -1;

            try
            {

                using (System.Data.IDbConnection idbConn = GetConnection())
                {

                    lock (idbConn)
                    {

                        lock (cmd)
                        {
                            cmd.Connection = idbConn;
                            cmd.CommandTimeout = iTimeout; 

                            try
                            {
                                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                    cmd.Connection.Open();

                                iAffected = cmd.ExecuteNonQuery();
                            }
                            catch (System.Exception ex)
                            {
                                if (Log(ex, cmd.CommandText))
                                    throw;
                            }
                            finally
                            {
                                if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                    cmd.Connection.Close();
                            } // End Finally

                        } // End lock cmd

                    } // End lock idbConn

                } // End Using idbConn

            } // End Try
            catch (System.Exception ex)
            {
                iAffected = -3;
                if (Log(ex, cmd.CommandText))
                    throw;
            }
            finally
            {

            }

            return iAffected;
        } // End Function Execute


        public virtual int ExecuteNonQueryFromFile(string strScript) // Alias
        {
            return Execute(CommandFromScript(strScript));
        }


        public virtual int ExecuteNonQuery(System.Data.IDbCommand cmd) // Alias
        {
            return Execute(cmd);
        }


        public virtual int ExecuteNonQuery(string strSQL) // Alias
        {
            return Execute(strSQL);
        }


        public virtual System.Data.DataTable GetLogins()
        {
            string strSQL = @"SELECT * FROM sys.syslogins ";

            return GetDataTable(strSQL);
        }


        public class VersionInfo
        {
            public int BitField;
            public string RawVersion;
            public string Version;
            public int Major;
            public int Release;
            public int Build;
        }


        public virtual VersionInfo ServerVersionInfo()
        {
            throw new System.NotImplementedException("cDAL.ServerVersionInfo");
        } // End Function ServerVersionInfo


        public virtual bool IsCurrentUserInRole(string RoleName)
        {
            string strSQL = @"
SELECT COUNT(*) FROM sys.login_token 
WHERE LOWER(name) = @__rolename
";

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                AddParameter(cmd, "__rolename", RoleName);
                return ExecuteScalar<bool>(cmd);
            }

        } // End Function IsCurrentUserInRole



        // http://stackoverflow.com/questions/2885335/clr-sql-assembly-get-the-bytestream
        // http://stackoverflow.com/questions/891617/how-to-read-a-image-by-idatareader
        // http://stackoverflow.com/questions/4103406/extracting-a-net-assembly-from-sql-server-2005
        public virtual void SaveAssembly(string assemblyName, string path)
        {
            string sql = @"
--DECLARE @__assemblyname nvarchar(260)
--SET @__assemblyname = 'Microsoft.SqlServer.Types'


SELECT 
	 A.name
	,AF.content 
FROM sys.assembly_files AS AF 

INNER JOIN sys.assemblies AS A 
	ON AF.assembly_id = A.assembly_id 
	
WHERE AF.file_id = 1 
AND A.name = @__assemblyname
;

";

            using (System.Data.IDbCommand cmd = this.CreateCommand(sql))
            {
                AddParameter(cmd, "__assemblyname", assemblyName);

                using (System.Data.IDataReader reader = ExecuteReader(cmd))
                {
                    reader.Read();
                    //SqlBytes bytes = reader.GetSqlBytes(0);
                    const int BUFFER_SIZE = 1024;
                    byte[] buffer = new byte[BUFFER_SIZE];

                    int col = reader.GetOrdinal("content");
                    int bytesRead = 0;
                    int offset = 0;

                    // write the byte stream out to disk
                    using (System.IO.FileStream bytestream = new System.IO.FileStream(path, System.IO.FileMode.CreateNew))
                    {

                        while ((bytesRead = (int)reader.GetBytes(col, offset, buffer, 0, BUFFER_SIZE)) > 0)
                        {
                            bytestream.Write(buffer, 0, bytesRead);
                            offset += bytesRead;
                        } // Whend

                        bytestream.Close();
                    } // End Using bytestream 

                    reader.Close();
                } // End Using reader

            } // End Using cmd

        } // End Function SaveAssembly


        public void SqlMissmanagemetStudioBackupFileTree()
        {
            string strSQL = @"
--insert #fixdrv (Name, Size) 

EXECUTE master.dbo.xp_fixeddrives  -- Type: Fixed
--EXECUTE master.dbo.xp_fixeddrives 1   -- Type: Remote
--EXECUTE master.dbo.xp_fixeddrives 2   -- Type: Removable
--EXECUTE master.dbo.xp_fixeddrives 3 -- Type: 'CD-ROM'

DECLARE @Path nvarchar(255)
DECLARE @Name nvarchar(255)


SET @Path = N'C:'
SET @Path = N'D:\Temp\'
EXECUTE master.dbo.xp_dirtree @Path, 1, 1 
";
            System.Console.WriteLine(strSQL);
        }


        public void Get10RandomResults()
        {
            // http://stackoverflow.com/questions/8674718/best-way-to-select-random-rows-postgresql
            string strSQL = "SELECT * FROM T ORDER BY RANDOM() LIMIT 10"; // PG

            // http://stackoverflow.com/questions/4329396/mysql-select-10-random-rows-from-600k-rows-fast
            strSQL = "SELECT column FROM table ORDER BY RAND() LIMIT 10"; // MySQL
            strSQL = @"
SELECT r1.* 
  FROM T_Benutzer AS r1 
  
   JOIN
       (
			SELECT 
			(
				RAND() * (SELECT MAX(BE_ID) FROM T_Benutzer)
             ) AS BE_ID 
        ) 
        AS r2 
	--ON r1.BE_ID = R2.BE_ID
        
 WHERE r1.BE_ID >= r2.BE_ID
 ORDER BY r1.BE_ID ASC
 --LIMIT 1
";


            strSQL = "SELECT TOP 10 column FROM table ORDER BY NEWID() "; // SQL-Server

            //SQL-Server fast:
            strSQL += @"
SELECT * FROM T_BlogTips WHERE TIP_UID IN 
(SELECT TOP 10  TIP_UID FROM T_BlogTips ORDER BY NEWID())
";

            // Firebird
            strSQL += @"

SELECT FIRST 1 
	t1.* 
FROM table t1 

LEFT JOIN Table t2 
	ON t2.pk = t1.pk 
	AND t2.cond = 1 
	
ORDER BY 
	CASE 
		WHEN t2.Cond = 1 
			THEN 0 
		ELSE rand() 
	END 
"; // Firebird

        }


        public virtual System.Exception ShrinkDb(string DbName, int Percent)
        {
            throw new System.NotImplementedException("cDAL.ShrinkDb");
        }


        public virtual System.Exception BackupDbOnServer(string DbName, string BackupToFilename)
        {
            throw new System.NotImplementedException("cDAL.BackupDbOnServer");
        }


        public virtual void RestoreDatabase(string DbName, string strFileName)
        {
            throw new System.NotImplementedException("cDAL.RestoreDatabase");
        }

        public virtual void RestoreDatabase(string DbName, string strFileName, int iTimeout)
        {
            throw new System.NotImplementedException("cDAL.RestoreDatabase");
        }


        public virtual void RemapDbUserLogins(string DbName)
        {
            throw new System.NotImplementedException("cDAL.RemapDbUserLogins");
        }


        public virtual string DropDatabaseScript(string DbName)
        {
            string strSQL = string.Format(@"
IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}')
    ALTER DATABASE {1} SET SINGLE_USER WITH ROLLBACK IMMEDIATE 
GO 


IF  EXISTS (SELECT name FROM sys.databases WHERE name = N'{0}') 
    DROP DATABASE {1} 
GO 


", StringEscapeString(DbName), EscapeDbName(DbName));

            return strSQL;
        }


        public virtual void DropDatabase(string DbName)
        {
            string strSQL = DropDatabaseScript(DbName);
            ExecuteGoSeparatedScript(strSQL);
        }


        public virtual string DatabaseDropCreateScript(string DbName, string NewDbName)
        {

            string strScript = DropDatabaseScript(NewDbName);
            strScript += DatabaseCreateScript(DbName, NewDbName);

            return strScript;
        }


        public virtual string DatabaseCreateScript(string DbName, string NewDbName)
        {
            throw new System.NotImplementedException("cDAL.DatabaseCreateScript");
        }


        public virtual string EscapeDbName(string DbName)
        {
            return string.Format("[{0}]", DbName);
        }


        public virtual string StringEscapeString(string strSqlString)
        {
            return strSqlString.Replace("'", "''");
        }


        public virtual string FixPathForWindows(string strSqlString)
        {
            return strSqlString.Replace('\'', '\\');
        }


        public virtual string GetConnectionString()
        {
            return GetConnectionString(null);
        }


        public abstract string GetConnectionString(string strDb);


        public virtual System.Data.IDbConnection GetConnection()
        {
            return GetConnection(null);
        }


        public System.Data.IDbConnection GetConnection(string strDb)
        {
            System.Data.Common.DbConnection con = m_providerFactory.CreateConnection();
            con.ConnectionString = GetConnectionString(strDb);

            return con;
        }


        public virtual void Insert<T>(object objInsertValue)
        {
            System.Type tTypeToInsert = typeof(T);
            this.Insert(tTypeToInsert, objInsertValue, tTypeToInsert.Name);
        } // End Sub InsertClassProfiles


        public virtual void Insert<T>(object objInsertValue, string strTableName)
        {
            System.Type tt = typeof(T);
            this.Insert(tt, objInsertValue, strTableName);
        } // End Sub InsertClassProfiles


        public virtual void Insert(System.Type tTypeToInsert, object objInsertValue)
        {
            this.Insert(tTypeToInsert, objInsertValue, tTypeToInsert.Name);
        }


        //typeof(cProfile)
        public virtual void Insert(System.Type tTypeToInsert, object objInsertValue, string strTableName)
        {
            System.Reflection.FieldInfo[] fields = tTypeToInsert.GetFields();
            System.Reflection.PropertyInfo[] properties = tTypeToInsert.GetProperties();


            //string[] astrFieldNames = fields.Select(c => c.Name).ToArray();
            //string[] astrFieldNames = fields.ToNameArray();
            string[] astrFieldNames = MyExtensionMethods.ToNameArray(fields);
            //string[] astrPropertyNames = properties.Select(c => c.Name).ToArray();
            //string[] astrPropertyNames = properties.ToNameArray();
            string[] astrPropertyNames = MyExtensionMethods.ToNameArray(properties);

            System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, System.Type>> lsFieldNames =
                new System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, System.Type>>();


            foreach (string strFieldName in astrFieldNames)
            {
                lsFieldNames.Add(new System.Collections.Generic.KeyValuePair<string, System.Type>
                    (strFieldName, typeof(System.Reflection.FieldInfo)));
            }

            foreach (string strPropertyName in astrPropertyNames)
            {
                lsFieldNames.Add(new System.Collections.Generic.KeyValuePair<string, System.Type>
                    (strPropertyName, typeof(System.Reflection.PropertyInfo)));
            }

            string strSQL = @"
SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = @strTableName
ORDER BY ORDINAL_POSITION 
";

            System.Collections.Generic.List<string> astrDbFields = null;
            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strTableName", strTableName);
                astrDbFields = (System.Collections.Generic.List<string>)this.GetList<string>(cmd);
            }


            for (int i = lsFieldNames.Count - 1; i > -1; --i)
            {
                //if (!astrDbFields.Contains(lsFieldNames[i].Key, StringComparer.OrdinalIgnoreCase))
                if (!MyExtensionMethods.Contains(astrDbFields, lsFieldNames[i].Key, System.StringComparer.OrdinalIgnoreCase))
                    lsFieldNames.Remove(lsFieldNames[i]);
            }

            strSQL = "INSERT INTO " + this.EscapeTableName(strTableName) + "( " + System.Environment.NewLine;
            for (int i = 0; i < lsFieldNames.Count; ++i)
            {
                strSQL += i == 0 ? "     " : "    ,";
                strSQL += "    " + lsFieldNames[i].Key + System.Environment.NewLine;
            } // Next i
            strSQL += ") " + System.Environment.NewLine;

            strSQL += "VALUES ( " + System.Environment.NewLine;

            for (int i = 0; i < lsFieldNames.Count; ++i)
            {
                strSQL += i == 0 ? "     " : "    ,";
                strSQL += "@__in_" + lsFieldNames[i].Key + System.Environment.NewLine;
            } // Next i
            strSQL += ") " + System.Environment.NewLine;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                for (int i = 0; i < lsFieldNames.Count; ++i)
                {
                    object objValue = null;

                    if (object.ReferenceEquals(lsFieldNames[i].Value, typeof(System.Reflection.FieldInfo)))
                        objValue = tTypeToInsert.GetField(lsFieldNames[i].Key).GetValue(objInsertValue);
                    else if (object.ReferenceEquals(lsFieldNames[i].Value, typeof(System.Reflection.PropertyInfo)))
                        objValue = tTypeToInsert.GetProperty(lsFieldNames[i].Key).GetValue(objInsertValue, null);
                    else
                        throw new System.Exception("No such type or property '" + lsFieldNames[i].Key + "'");

                    //object objValue = Profile.GetValue(lsFieldNames[i]);
                    this.AddParameter(cmd, "__in_" + lsFieldNames[i].Key, objValue);
                } // Next i


                string strPK = astrDbFields[0];

                using (System.Data.IDbCommand cmd2 = this.CreateCommand("DELETE FROM " + this.EscapeTableName(strTableName) + " WHERE " + this.EscapeTableName(strPK) + " = @__in_pk"))
                {
                    object objPK = null;
                    if (object.ReferenceEquals(lsFieldNames[0].Value, typeof(System.Reflection.FieldInfo)))
                        objPK = tTypeToInsert.GetField(lsFieldNames[0].Key).GetValue(objInsertValue);
                    else if (object.ReferenceEquals(lsFieldNames[0].Value, typeof(System.Reflection.PropertyInfo)))
                        objPK = tTypeToInsert.GetProperty(lsFieldNames[0].Key).GetValue(objInsertValue, null);
                    else
                        throw new System.Exception("No such type or property '" + lsFieldNames[0].Key + "'");

                    this.AddParameter(cmd2, "__in_pk", objPK);
                    this.ExecuteNonQuery(cmd2);
                } // End Using cmd2

                this.ExecuteNonQuery(cmd);
            } // End Using cmd

        } // End Sub InsertClassProfiles


        public virtual T ExecuteScalar<T>(System.Data.IDbCommand cmd)
        {
            return this.ExecuteScalar<T>(cmd, null);
        }


        public virtual T ExecuteScalar<T>(System.Data.IDbCommand cmd, string strDbName)
        {
            string strReturnValue = null;
            System.Type tReturnType = null;
            object objReturnValue = null;

            lock (cmd)
            {

                using (System.Data.IDbConnection idbc = GetConnection(strDbName))
                {
                    cmd.Connection = idbc;

                    lock (cmd.Connection)
                    {

                        try
                        {
                            tReturnType = typeof(T);

                            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                cmd.Connection.Open();

                            objReturnValue = cmd.ExecuteScalar();

                            if (objReturnValue != null)
                            {

                                if (!object.ReferenceEquals(tReturnType, typeof(System.Byte[])))
                                {
                                    strReturnValue = objReturnValue.ToString();
                                    objReturnValue = null;
                                } // End if (!object.ReferenceEquals(tReturnType, typeof(System.Byte[])))

                            } // End if (objReturnValue != null)

                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            if (Log("cDAL.ExecuteScalar<T>(string strSQL)", ex, cmd.CommandText))
                                throw;
                        
                        } // End Catch
                        finally
                        {
                            if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                cmd.Connection.Close();
                        } // End Finally

                    } // End lock (cmd.Connection)

                } // End using idbc

            } // End lock (cmd)


            try
            {
                if (object.ReferenceEquals(tReturnType, typeof(object)))
                {
                    return InlineTypeAssignHelper<T>(objReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(string)))
                {
                    return InlineTypeAssignHelper<T>(strReturnValue);
                } // End if string
                else if (object.ReferenceEquals(tReturnType, typeof(bool)))
                {
                    bool bReturnValue = false;
                    bool bSuccess = bool.TryParse(strReturnValue, out bReturnValue);

                    if (bSuccess)
                        return InlineTypeAssignHelper<T>(bReturnValue);

                    if (strReturnValue == "0")
                        return InlineTypeAssignHelper<T>(false);

                    return InlineTypeAssignHelper<T>(true);
                } // End if bool
                else if (object.ReferenceEquals(tReturnType, typeof(int)))
                {
                    int iReturnValue = int.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(iReturnValue);
                } // End if int
                else if (object.ReferenceEquals(tReturnType, typeof(uint)))
                {
                    uint uiReturnValue = uint.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(uiReturnValue);
                } // End if uint
                else if (object.ReferenceEquals(tReturnType, typeof(long)))
                {
                    long lngReturnValue = long.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(lngReturnValue);
                } // End if long
                else if (object.ReferenceEquals(tReturnType, typeof(ulong)))
                {
                    ulong ulngReturnValue = ulong.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(ulngReturnValue);
                } // End if ulong
                else if (object.ReferenceEquals(tReturnType, typeof(float)))
                {
                    float fltReturnValue = float.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(fltReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(double)))
                {
                    double dblReturnValue = double.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(dblReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(System.Net.IPAddress)))
                {
                    System.Net.IPAddress ipaAddress = null;

                    if (string.IsNullOrEmpty(strReturnValue))
                        return InlineTypeAssignHelper<T>(ipaAddress);

                    ipaAddress = System.Net.IPAddress.Parse(strReturnValue);
                    return InlineTypeAssignHelper<T>(ipaAddress);
                } // End if IPAddress
                else if (object.ReferenceEquals(tReturnType, typeof(System.Byte[])))
                {
                    if (objReturnValue == System.DBNull.Value)
                        return InlineTypeAssignHelper<T>(null);

                    return InlineTypeAssignHelper<T>(objReturnValue);
                }
                else if (object.ReferenceEquals(tReturnType, typeof(System.Guid)))
                {
                    if (string.IsNullOrEmpty(strReturnValue))
                        return InlineTypeAssignHelper<T>(null);

                    return InlineTypeAssignHelper<T>(new System.Guid(strReturnValue));
                } // End if System.Guid
                else // No datatype matches
                {
                    throw new System.NotImplementedException("ExecuteScalar<T>: This type is not yet defined.");
                } // End else of if tReturnType = datatype

            } // End Try
            catch (System.Exception ex)
            {
                if (Log("cDAL.cs ==> ExecuteScalar<T>(string strSQL)", ex, cmd.CommandText))
                    throw;
            } // End Catch

            return InlineTypeAssignHelper<T>(null);
        } // End Function ExecuteScalar(cmd)


        public virtual T ExecuteScalar<T>(string strSQL)
        {
            return ExecuteScalar<T>(strSQL, null);
        }

        public virtual T ExecuteWithLimitScalar<T>(string strSQL, int iLimit)
        {
            return ExecuteWithLimitScalar<T>(strSQL, iLimit, null);
        }


        public virtual T ExecuteScalar<T>(string strSQL, string strDbName)
        {
            T tReturnValue = default(T);

            // pfff, mono C# compiler problem...
            //sqlCMD = new System.Data.SqlClient.SqlCommand(strSQL, m_SqlConnection);
            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                tReturnValue = ExecuteScalar<T>(cmd, strDbName);
            } // End Using cmd

            return tReturnValue;
        } // End Function ExecuteScalar(strSQL)

        public virtual T ExecuteWithLimitScalar<T>(string strSQL, int iLimit, string strDbName)
        {
            T tReturnValue = default(T);

            // pfff, mono C# compiler problem...
            //sqlCMD = new System.Data.SqlClient.SqlCommand(strSQL, m_SqlConnection);
            using (System.Data.IDbCommand cmd = CreateLimitedCommand(strSQL, iLimit))
            {
                tReturnValue = ExecuteScalar<T>(cmd, strDbName);
            } // End Using cmd

            return tReturnValue;
        } // End Function ExecuteScalar(strSQL)





        public virtual T ExecuteScalarFromFile<T>(string strScript)
        {
            return ExecuteScalar<T>(CommandFromScript(strScript));
        }


        public virtual System.Data.DataTable GetDataTable(string strSQL)
        {
            return GetDataTable(strSQL, null);
        } // End Function GetDataTable


        public virtual System.Data.DataTable GetDataTable(string strSQL, string strInitialCatalog)
        {
            System.Data.DataTable dt = null;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                dt = GetDataTable(cmd, strInitialCatalog);
            } // End Using cmd

            return dt;
        } // End Function GetDataTable


        public virtual System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd)
        {
            return GetDataTable(cmd, null);
        }


        public virtual System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd, string strDb)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            using (System.Data.IDbConnection idbc = GetConnection(strDb))
            {

                lock (idbc)
                {

                    lock (cmd)
                    {

                        try
                        {
                            cmd.Connection = idbc;

                            using (System.Data.Common.DbDataAdapter daQueryTable = this.m_providerFactory.CreateDataAdapter())
                            {
                                daQueryTable.SelectCommand = (System.Data.Common.DbCommand)cmd;
                                daQueryTable.Fill(dt);
                            } // End Using daQueryTable

                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            if (Log("cDAK.GetDataTable(System.Data.IDbCommand cmd)", ex, cmd.CommandText))
                                throw;
                        }// End Catch
                        finally
                        {
                            if (idbc.State != System.Data.ConnectionState.Closed)
                                idbc.Close();
                        } // End Finally

                    } // End lock cmd

                } // End lock idbc

            } // End Using idbc

            return dt;
        } // End Function GetDataTable




        public abstract System.Data.DataTable GetEntireTable(string strTableName);


        public virtual System.Data.DataSet GetDataSet(System.Data.IDbCommand cmd)
        {
            string strTableName = "NewDataSet";
            System.Data.DataSet ds = new System.Data.DataSet(strTableName);

            using (System.Data.IDbConnection idbc = GetConnection())
            {

                lock (idbc)
                {

                    lock (cmd)
                    {

                        try
                        {
                            cmd.Connection = idbc;

                            using (System.Data.Common.DbDataAdapter daQueryTable = this.m_providerFactory.CreateDataAdapter())
                            {
                                daQueryTable.SelectCommand = (System.Data.Common.DbCommand)cmd;
                                daQueryTable.Fill(ds);
                            } // End Using daQueryTable

                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            //COR.Debug.MsgBox("Exception executing ExecuteInTransaction: " + ex.Message);
                            if (Log("cDAL.GetDataTable(System.Data.IDbCommand cmd)", ex, cmd.CommandText))
                                throw;
                        }// End Catch
                        finally
                        {
                            if (idbc.State != System.Data.ConnectionState.Closed)
                                idbc.Close();
                        } // End Finally

                    } // End lock cmd

                } // End lock idbc

            } // End Using idbc

            return ds;
        } // End Function GetDataSet


        public virtual System.Data.DataSet GetDataSet(string strSQL)
        {
            System.Data.DataSet ds = null;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                ds = GetDataSet(cmd);
            }

            return ds;
        } // End Function GetDataSet


        public virtual System.Data.IDataReader ExecuteReader(System.Data.IDbCommand cmd)
        {
            System.Data.IDataReader idr = null;

            lock (cmd)
            {
                System.Data.IDbConnection idbc = GetConnection();
                cmd.Connection = idbc;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();

                try
                {
                    idr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
                }
                catch (System.Exception ex)
                {
                    if (Log(ex, cmd))
                        throw;
                }
            } // End Lock cmd

            return idr;
        } // End Function ExecuteReader


        public virtual System.Data.IDataReader ExecuteReader(string strSQL)
        {
            System.Data.IDataReader idr = null;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                idr = ExecuteReader(cmd);
            } // End Using cmd

            return idr;
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteDbReader(System.Data.IDbCommand cmd)
        {
            return (System.Data.Common.DbDataReader)ExecuteReader(cmd);
        } // End Function ExecuteReader


        public virtual System.Data.Common.DbDataReader ExecuteDbReader(string strSQL)
        {
            return (System.Data.Common.DbDataReader)ExecuteReader(strSQL);
        } // End Function ExecuteReader


        public virtual object ExecuteStoredProcedure(System.Data.IDbCommand cmd)
        {
            object objReturnValue = null;
            try
            {

                lock (cmd)
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    //// System.Data.SqlClient.SqlParameter returnValue = new System.Data.SqlClient.SqlParameter();
                    System.Data.IDbDataParameter returnValue = cmd.CreateParameter();
                    returnValue.Direction = System.Data.ParameterDirection.ReturnValue;

                    //this.AddParameter(cmd, "name", null, System.Data.ParameterDirection.ReturnValue);
                    cmd.Parameters.Add(returnValue);

                    using (System.Data.IDbConnection con = this.GetConnection())
                    {
                        lock (con)
                        {
                            cmd.Connection = con;

                            try
                            {
                                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                    cmd.Connection.Open();

                                cmd.ExecuteNonQuery();

                                //Assert.AreEqual(123, (int)returnValue.Value);
                                //System.Diagnostics.Debug.Assert(123 == (int)((System.Data.IDataParameter)cmd.Parameters["name"]).Value);
                                //System.Diagnostics.Debug.Assert(123 == (int)returnValue.Value);
                                objReturnValue = returnValue.Value;
                            }
                            catch (System.Exception ex)
                            {
                                if (Log("cDAL.ExecuteStoredProcedure", ex, GetParametrizedQueryText(cmd), true))
                                    throw;
                            }
                            finally
                            {
                                if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                    cmd.Connection.Close();
                            } // End Finally

                        } // End Lock con

                    } // End using con

                } // End Lock cmd
            }
            catch (System.Exception ex)
            {
                if (Log("cDAL.ExecuteStoredProcedure", ex, "sql", true))
                    throw;
            } // End catch

            return objReturnValue;
        } // End Function ExecuteStoredProcedure


        public virtual object ExecuteStoredProcedure(string strProcedureName)
        {
            object objReturnValue = null;

            //// using (SqlCommand cmd = new SqlCommand("TestProc", cn))
            using (System.Data.IDbCommand cmd = this.CreateCommand(strProcedureName))
            {
                objReturnValue = ExecuteStoredProcedure(cmd);
            } // End Using cmd

            return objReturnValue;
        } // End Function ExecuteStoredProcedure


        public virtual System.Data.IDataReader ExecuteReaderFromStoredProcedure(string strStoredProcedureName)
        {
            using (System.Data.IDbCommand cmd = this.CreateCommand(strStoredProcedureName))
            {
                return this.ExecuteReaderFromStoredProcedure(cmd);
            } // End Using cmd

        } // End Function ExecuteReaderFromStoredProcedure


        public virtual System.Data.IDataReader ExecuteReaderFromStoredProcedure(System.Data.IDbCommand cmd)
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return this.ExecuteReader(cmd);
        } // End Function ExecuteReaderFromStoredProcedure


        public virtual System.Data.DataTable GetDatatableFromStoredProcedure(string strStoredProcedureName)
        {
            return GetDatatableFromStoredProcedure(strStoredProcedureName, null);
        } // End Function GetDatatableFromStoredProcedure


        public virtual System.Data.DataTable GetDatatableFromStoredProcedure(string strStoredProcedureName, string strInitialCatalog)
        {

            using (System.Data.IDbCommand cmd = CreateCommand(strStoredProcedureName))
            {
                return GetDatatableFromStoredProcedure(cmd, strInitialCatalog);
            } // End Using cmd

        } // End Function GetDatatableFromStoredProcedure


        public virtual System.Data.DataTable GetDatatableFromStoredProcedure(System.Data.IDbCommand cmd, string strInitialCatalog)
        {
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            return this.GetDataTable(cmd);
        } // End Function GetDatatableFromStoredProcedure


        public virtual string QuoteObject(string ObjectName)
        {
            if (string.IsNullOrEmpty(ObjectName))
                throw new System.ArgumentNullException("ObjectName");

            return "\"" + ObjectName.Replace("\"", "\"\"") + "\"";
        }


        public virtual bool ObjectNameNeedsEscaping(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                return false;

            if (this.IsReservedKeyword(objectName))
                return true;

            if (objectName.StartsWith("@"))
                return true;


            // The underscore (_), at sign (@), or number sign (#).
            // %^&({}+-/ ]['''
            char[] lsIllegalCharacters = "+-*/%<>=&|^(){}[]\"'´`\t\n\r \\,.;?!~¨¦§°¢£€".ToCharArray();

            for (int i = 0; i < lsIllegalCharacters.Length; ++i)
            {
                if (objectName.IndexOf(lsIllegalCharacters[i]) != -1)
                    return true;
            }

            return false;
        } // End Function MustEscape 


        public virtual string QuoteObjectWhereNecessary(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                throw new System.ArgumentNullException("ObjectName");

            if (ObjectNameNeedsEscaping(objectName))
                return QuoteObject(objectName);

            return objectName;
        } // End Function QuoteObjectWhereNecessary


        public virtual string BracketObject(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                throw new System.ArgumentNullException("objectName");

            return "[" + objectName.Replace("]", "]]") + "]";
        }


        public virtual string BracketObjectWhereNecessary(string objectName)
        {
            if (string.IsNullOrEmpty(objectName))
                throw new System.ArgumentNullException("objectName");

            if (ObjectNameNeedsEscaping(objectName))
                return BracketObject(objectName);

            return objectName;
        } // End Function QuoteObjectWhereNecessary


        public virtual string GetObjectDefinition(string strObjectName)
        {
            return GetObjectDefinition(strObjectName, null);
        }


        public virtual string GetObjectDefinition(string strObjectName, string strDbName)
        {
            string strReturnValue = null;
            using (System.Data.IDbCommand cmd = this.CreateCommand("SELECT OBJECT_DEFINITION(OBJECT_ID(@objname)) AS objdef"))
            {
                this.AddParameter(cmd, "objname", strObjectName);
                strReturnValue = this.ExecuteScalar<string>(cmd, strDbName);
            } // End Using cmd

            return strReturnValue;
        } // End Function GetObjectDefinition


        public virtual string DecryptPassword(string strEncryptedPassword)
        {
            return DB.Abstraction.Tools.Cryptography.AES.DeCrypt(strEncryptedPassword);
        }


        public enum dbOwner : uint
        {
            system = 1 << 0,
            user = 1 << 1,
            all = 3
        } // End Enum dbOwner


        public virtual string GetDataBasesQueryText()
        {
            return GetDataBasesQueryText(dbOwner.all);
        }


        public virtual string GetDataBasesQueryText(dbOwner ShowDBs)
        {
            throw new System.NotImplementedException("GetDataBasesQueryText not implemented");
        }


        public virtual System.Data.DataTable GetDataBases()
        {
            throw new System.NotImplementedException("GetDataBases not implemented");
        } // End Function GetDataBases


        public virtual System.Data.DataTable GetSystemDataBases()
        {
            throw new System.NotImplementedException("GetSystemDataBases not implemented");
        } // End Function GetSystemDataBases


        public virtual T GetClass<T>(System.Data.IDbCommand cmd)
        {
            T tThisClassInstance = System.Activator.CreateInstance<T>();
            return GetClass<T>(cmd, tThisClassInstance);
        }


        public static System.Reflection.MemberInfo GetMemberInfo(System.Type t, string strName)
        {
            System.Reflection.MemberInfo mi = t.GetField(strName, m_CaseSensitivity);

            if (mi == null)
                mi = t.GetProperty(strName, m_CaseSensitivity);

            return mi;
        } // End Function GetMemberInfo


        public static void SetMemberValue(object obj, System.Reflection.MemberInfo mi, object objValue)
        {
            if (mi is System.Reflection.FieldInfo)
            {
                System.Reflection.FieldInfo fi = (System.Reflection.FieldInfo)mi;
                fi.SetValue(obj, MyChangeType(objValue, fi.FieldType));
                return;
            }

            if (mi is System.Reflection.PropertyInfo)
            {
                System.Reflection.PropertyInfo pi = (System.Reflection.PropertyInfo)mi;
                pi.SetValue(obj, MyChangeType(objValue, pi.PropertyType), null);
                return;
            }

            // Else silently ignore value
        } // End Sub SetMemberValue



        public virtual T GetClass<T>(System.Data.IDbCommand cmd, T tThisClassInstance)
        {
            System.Type t = typeof(T);

            lock (cmd)
            {
                using (System.Data.IDataReader idr = ExecuteReader(cmd))
                {

                    lock (idr)
                    {

                        while (idr.Read())
                        {

                            for (int i = 0; i < idr.FieldCount; ++i)
                            {
                                string strName = idr.GetName(i);
                                object objVal = idr.GetValue(i);

                                System.Reflection.MemberInfo mi = GetMemberInfo(t, strName);
                                SetMemberValue(tThisClassInstance, mi, objVal);

                                /*
                                System.Reflection.FieldInfo fi = t.GetField(strName, m_CaseSensitivity);
                                if (fi != null)
                                    fi.SetValue(tThisClassInstance, MyChangeType(objVal, fi.FieldType));
                                else
                                {
                                    System.Reflection.PropertyInfo pi = t.GetProperty(strName, m_CaseSensitivity);
                                    if (pi != null)
                                        pi.SetValue(tThisClassInstance, MyChangeType(objVal, pi.PropertyType), null);

                                } // End else of if (fi != null)
                                */
                            } // Next i

                            break;
                        } // Whend

                        idr.Close();
                    } // End Lock idr

                } // End Using idr

            } // End lock cmd

            return tThisClassInstance;
        } // End Function GetClass


        public virtual T GetClass<T>(string strSQL)
        {
            T tReturnClassInstance = default(T);

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                tReturnClassInstance = GetClass<T>(cmd);
            } // End Using cmd

            return tReturnClassInstance;
        } // End Function GetClass


        public static System.Nullable<T> ToNullable<T>(object s) where T : struct
        {
            System.Nullable<T> result = new System.Nullable<T>();
            //try
            //{
            //if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
            {
                System.ComponentModel.TypeConverter conv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                result = (T)conv.ConvertFrom(s);
            }
            //}
            //catch { }

            return result;
        } // End Function ToNullable<T>


        public static System.Nullable<T> ToNullable2<T>(object s) where T : struct
        {
            System.Nullable<T> result = new System.Nullable<T>();
            //try
            //{
            //if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
            {
                System.ComponentModel.TypeConverter conv = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                result = (T)conv.ConvertFrom(s);
            }
            //}
            //catch { }
            return result;
        }


        public static bool IsNullable<T>(T t)
        { return false; }


        public static bool IsNullable<T>(T? t) where T : struct
        { return true; }


        public static bool IsNullable(object obj)
        {
            if (obj == null)
                return false;

            System.Type t = obj.GetType();
            return t.IsGenericType
                && object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(System.Nullable<>));
        }


        public static bool IsNullable(System.Type t)
        {
            if (t == null)
                return false;

            return t.IsGenericType && object.ReferenceEquals(t.GetGenericTypeDefinition(), typeof(System.Nullable<>));
        } // End Function IsNullable


        //protected const System.Reflection.BindingFlags m_CaseSensitivity = System.Reflection.BindingFlags.IgnoreCase;
        protected const System.Reflection.BindingFlags m_CaseSensitivity = System.Reflection.BindingFlags.Instance
            | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase
            ;


        public void SetValue<T>(T inst, System.Reflection.FieldInfo fieldInfo, object obj)
        {
            fieldInfo.SetValue(inst, obj);

        }




#if false
        public static Action<T, object> GetSetter<T>(System.Reflection.FieldInfo fieldInfo)
        {
            System.Linq.Expressions.ParameterExpression targetExp = System.Linq.Expressions.Expression.Parameter(typeof(object), fieldInfo.Name);
            System.Linq.Expressions.ParameterExpression valueExp = System.Linq.Expressions.Expression.Parameter(typeof(object));

            System.Linq.Expressions.MemberExpression fieldExp = System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression.Convert(targetExp, fieldInfo.DeclaringType), fieldInfo.Name);
            System.Linq.Expressions.BinaryExpression assignExp = System.Linq.Expressions.Expression.Assign(fieldExp, System.Linq.Expressions.Expression.Convert(valueExp, fieldInfo.FieldType));

            var setter = System.Linq.Expressions.Expression.Lambda<Action<T, object>>(assignExp, targetExp, valueExp).Compile();

            return setter;
        }



        public static void SetValue_DotNet4<T>(T inst, System.Reflection.FieldInfo fieldInfo, object obj)
        {
            System.Linq.Expressions.ParameterExpression targetExp = System.Linq.Expressions.Expression.Parameter(typeof(object), fieldInfo.Name);
            System.Linq.Expressions.ParameterExpression valueExp = System.Linq.Expressions.Expression.Parameter(typeof(object));
            
            System.Linq.Expressions.MemberExpression fieldExp = System.Linq.Expressions.Expression.Field(System.Linq.Expressions.Expression.Convert(targetExp, fieldInfo.DeclaringType), fieldInfo.Name);
            System.Linq.Expressions.BinaryExpression assignExp = System.Linq.Expressions.Expression.Assign(fieldExp, System.Linq.Expressions.Expression.Convert(valueExp, fieldInfo.FieldType));
            
            var setter = System.Linq.Expressions.Expression.Lambda<Action<T, object>>(assignExp, targetExp, valueExp).Compile();

            setter(inst, obj);
        }

#endif


        // Anything else than a struct or a class
        public virtual bool IsSimpleType(System.Type tThisType)
        {

            if (tThisType.IsPrimitive)
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.String)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.DateTime)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.Guid)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.Decimal)))
            {
                return true;
            }

            if (object.ReferenceEquals(tThisType, typeof(System.Object)))
            {
                return true;
            }

            return false;
        } // End Function IsSimpleType


        public static object MyChangeType(object objVal, System.Type t)
        {
            if (objVal == null || object.ReferenceEquals(objVal, System.DBNull.Value))
            {
                return null;
            }

            //getbasetype
            System.Type tThisType = objVal.GetType();

            bool bNullable = IsNullable(t);
            if (bNullable)
            {
                t = System.Nullable.GetUnderlyingType(t);
            }

            if (object.ReferenceEquals(t, typeof(string)) && object.ReferenceEquals(tThisType, typeof(System.Guid)))
            {
                return objVal.ToString();
            }

            return System.Convert.ChangeType(objVal, t);
        } // End Function MyChangeType


        public virtual System.Collections.Generic.List<T> GetList<T>(System.Data.IDbCommand cmd)
        {
            System.Collections.Generic.List<T> lsReturnValue = new System.Collections.Generic.List<T>();
            T tThisValue = default(T);
            System.Type tThisType = typeof(T);

            lock (cmd)
            {
                using (System.Data.IDataReader idr = ExecuteReader(cmd))
                {

                    lock (idr)
                    {


                        if (IsSimpleType(tThisType))
                        {
                            while (idr.Read())
                            {
                                object objVal = idr.GetValue(0);
                                tThisValue = (T)MyChangeType(objVal, typeof(T));
                                //tThisValue = System.Convert.ChangeType(objVal, T),

                                lsReturnValue.Add(tThisValue);
                            } // End while (idr.Read())

                        }
                        else
                        {
                            int myi = idr.FieldCount;

                            System.Reflection.FieldInfo[] fis = new System.Reflection.FieldInfo[idr.FieldCount];
                            //Action<T, object>[] setters = new Action<T, object>[idr.FieldCount];

                            for (int i = 0; i < idr.FieldCount; ++i)
                            {
                                string strName = idr.GetName(i);
                                System.Reflection.FieldInfo fi = tThisType.GetField(strName, m_CaseSensitivity);
                                fis[i] = fi;

                                //if (fi != null)
                                //    setters[i] = GetSetter<T>(fi);
                            } // Next i


                            while (idr.Read())
                            {
                                //idr.GetOrdinal("")
                                tThisValue = System.Activator.CreateInstance<T>();

                                // Console.WriteLine(idr.FieldCount);
                                for (int i = 0; i < idr.FieldCount; ++i)
                                {
                                    string strName = idr.GetName(i);
                                    object objVal = idr.GetValue(i);

                                    //System.Reflection.FieldInfo fi = t.GetField(strName, m_CaseSensitivity);
                                    if (fis[i] != null)
                                    //if (fi != null)
                                    {
                                        //fi.SetValue(tThisValue, System.Convert.ChangeType(objVal, fi.FieldType));
                                        fis[i].SetValue(tThisValue, MyChangeType(objVal, fis[i].FieldType));
                                    } // End if (fi != null) 
                                    else
                                    {
                                        System.Reflection.PropertyInfo pi = tThisType.GetProperty(strName, m_CaseSensitivity);
                                        if (pi != null)
                                        {
                                            //pi.SetValue(tThisValue, System.Convert.ChangeType(objVal, pi.PropertyType), null);
                                            pi.SetValue(tThisValue, MyChangeType(objVal, pi.PropertyType), null);
                                        } // End if (pi != null)

                                        // Else silently ignore value
                                    } // End else of if (fi != null)

                                    //Console.WriteLine(strName);
                                } // Next i

                                lsReturnValue.Add(tThisValue);
                            } // Whend

                        } // End if IsSimpleType(tThisType)

                        idr.Close();
                    } // End Lock idr

                } // End Using idr

            } // End lock cmd

            return lsReturnValue;
        } // End Function GetList


        public virtual System.Collections.Generic.List<T> GetList<T>(string strSQL)
        {
            System.Collections.Generic.List<T> lsReturnValue = null;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                lsReturnValue = GetList<T>(cmd);
            } // End Using cmd

            return lsReturnValue;
        } // End Function GetList


        public virtual System.Collections.Generic.List<T> GetListLimited<T>(string strSQL, int iLimit)
        {
            System.Collections.Generic.List<T> lsReturnValue = null;

            using (System.Data.IDbCommand cmd = CreateLimitedCommand(strSQL, iLimit))
            {
                lsReturnValue = GetList<T>(cmd);
            } // End Using cmd

            return lsReturnValue;
        } // End Function GetList


        public virtual string GetViewCreationScript(string strSQL)
        {
            return "";
        }


        public virtual string GetFunctionCreationScript(string strSQL)
        {
            return "";
        }


        public virtual string GetProcedureCreationScript(string strSQL)
        {
            return "";
        }


        public virtual bool BulkCopy(string strDestinationTable, System.Data.DataTable dt)
        {
            throw new System.NotImplementedException("BulkCopy not implemented");
        }


        public virtual bool BulkCopy(string strDestinationTable, System.Data.DataTable dt, bool bWithDelete)
        {
            throw new System.NotImplementedException("BulkCopy not implemented");
        }


        public virtual System.Data.DataTable GetForeignKeyDependencies()
        {
            throw new System.NotImplementedException("cDAL.GetForeignKeyDependencies not implemented");
        }


        public virtual void ExportTables()
        {
            throw new System.NotImplementedException("ExportTables not implemented");
        }


        public virtual void ExportViews()
        {
            throw new System.NotImplementedException("ExportViews not implemented");
        }


        public virtual void ExportFunctions()
        {
            throw new System.NotImplementedException("ExportFunctions not implemented");
        }


        public virtual void ExportProcedures()
        {
            throw new System.NotImplementedException("ExportProcedures not implemented");
        }


        public virtual void ExportDatabase()
        {
            throw new System.NotImplementedException("ExportDatabase not implemented");
        } // End Sub ExportDatabase


        public virtual void ImportTableData()
        {
            throw new System.NotImplementedException("ImportTableData not implemented");
        } // End Sub ImportTableData


        protected void GetViewsAndWriteSQL(string strViewName)
        {
            string strText = GetViewCreationScript(strViewName);

            string strPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            strPath = System.IO.Path.Combine(strPath, "SRF_Export_sts");
            strPath = System.IO.Path.Combine(strPath, "Views");

            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            strPath = System.IO.Path.Combine(strPath, strViewName + ".sql");
            System.Console.WriteLine(strPath);

            if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);

            System.IO.File.WriteAllText(strPath, strText, System.Text.Encoding.Unicode);
        } // End Sub GetViewsAndWriteSQL


        protected void GetFunctionsAndWriteSQL(string strViewName)
        {
            string strText = GetFunctionCreationScript(strViewName);

            string strPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            strPath = System.IO.Path.Combine(strPath, "SRF_Export_sts");
            strPath = System.IO.Path.Combine(strPath, "Functions");

            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            strPath = System.IO.Path.Combine(strPath, strViewName + ".sql");
            System.Console.WriteLine(strPath);

            if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);

            System.IO.File.WriteAllText(strPath, strText, System.Text.Encoding.Unicode);
        } // End Sub GetViewsAndWriteSQL


        protected void GetProceduresAndWriteSQL(string strProcedureName)
        {
            string strText = GetProcedureCreationScript(strProcedureName);

            string strPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            strPath = System.IO.Path.Combine(strPath, "SRF_Export_sts");
            strPath = System.IO.Path.Combine(strPath, "Procedures");

            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            strPath = System.IO.Path.Combine(strPath, strProcedureName + ".sql");
            System.Console.WriteLine(strPath);

            if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);

            System.IO.File.WriteAllText(strPath, strText, System.Text.Encoding.Unicode);
        } // End Sub GetProceduresAndWriteSQL


        protected void GetDataTableAndWriteXML(string strTableName)
        {
            System.Data.DataTable dt = GetDataTable("SELECT * FROM " + EscapeTableName(strTableName));
            dt.TableName = strTableName;

            string strPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            strPath = System.IO.Path.Combine(strPath, "SRF_Export_sts");
            strPath = System.IO.Path.Combine(strPath, "Tables");

            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            strPath = System.IO.Path.Combine(strPath, strTableName + ".xml");
            System.Console.WriteLine(strPath);

            if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);

            dt.WriteXml(strPath, System.Data.XmlWriteMode.WriteSchema, false);
        } // End Sub GetDataTableAndWriteXML


        public virtual string Insert_Bool(bool bInput)
        {
            if (bInput)
                return "1";

            return "0";
        } // End Function Insert_Bool


        public virtual string Insert_Bool(object objInput)
        {
            if (objInput != null)
                return Insert_Bool(objInput.ToString());

            return "0";
        } // End Function Insert_Bool


        public virtual string Insert_Bool(string strText)
        {
            if (string.IsNullOrEmpty(strText))
                return "0";

            strText = strText.ToLower();

            bool bResult;
            if (bool.TryParse(strText, out bResult))
            {
                if (bResult)
                    return "1";

                return "0";
            }

            double dblResult;
            if (double.TryParse(strText, out dblResult))
            {
                if (dblResult != 0.0)
                    return "1";

                return "0";
            }

            return "0";
        } // End Function Insert_Bool


        public string Insert_DateTime(object obj)
        {
            string strText = null;

            if (obj != null)
            {
                if (object.ReferenceEquals(obj.GetType(), typeof(System.DateTime)))
                {
                    System.DateTime dtDateTime = (System.DateTime)obj;
                    //strText = dtDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fffffff"); // Max precision .NET
                    strText = dtDateTime.ToString("yyyy-MM-ddTHH:mm:ss.fff"); // Max precision SQL
                    //strText = dtDateTime.ToString("yyyyMMdd"); // Proper date
                    // Access NEEDS dtDateTime.ToString("yyyy-MM-dd");

                } // End if (object.ReferenceEquals(obj.GetType(), typeof(DateTime)))
                else
                    strText = obj.ToString();
            } // End if (obj != null)


            if (string.IsNullOrEmpty(strText))
                return "NULL";

            strText = strText.Replace("'", "''");

            return "'" + strText + "'";
        } // End Function Insert_DateTime


        public string Insert_ASCII(object obj)
        {
            string strText = GetTextFromObject(obj);

            if (string.IsNullOrEmpty(strText))
                return "NULL";

            strText = strText.Replace("'", "''");

            return "'" + strText + "'";
        } // End Function Insert_ASCII


        public virtual string Insert_Unicode(object obj)
        {
            string strText = GetTextFromObject(obj);

            if (string.IsNullOrEmpty(strText))
                return "NULL";

            strText = strText.Replace("'", "''");

            return "'" + strText + "'";
        } // End Function Insert_Unicode


        protected virtual string GetTextFromObject(object obj)
        {
            string strText = null;

            if (obj != null)
            {
                if (object.ReferenceEquals(obj.GetType(), typeof(System.DateTime)))
                {
                    System.DateTime dtDateTime = (System.DateTime)obj;
                    //strText = dtDateTime.ToString("yyyy-MM-dd");
                    strText = dtDateTime.ToString("yyyy-MM-ddTHH:mm:ss"); // ISO 8601
                }
                else if (object.ReferenceEquals(obj.GetType(), typeof(bool)))
                {
                    bool bValue = (bool)obj;
                    if (bValue)
                        strText = "true";
                    else
                        strText = "false";
                }
                else if (object.ReferenceEquals(obj.GetType().BaseType, typeof(System.Text.Encoding)))
                {
                    System.Text.Encoding enc = (System.Text.Encoding)obj;
                    //strText = enc.EncodingName;
                    //strText = enc.BodyName;
                    //strText = enc.HeaderName;
                    //strText = enc.CodePage.ToString(); // 20127
                    //strText = enc.WindowsCodePage.ToString() ; // 1252
                    strText = enc.WebName;
                }
                else
                    strText = obj.ToString();
            } // End if (obj != null)

            return strText;
        } // End Function GetTextFromObject


        // From Type to DBType
        protected virtual System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType DBtype = System.Data.DbType.String; // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return DBtype;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }

                DBtype = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);

                // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
                // http://msdn.microsoft.com/en-us/library/bbw6zyha(v=vs.71).aspx
                if (DBtype == System.Data.DbType.UInt64)
                    DBtype = System.Data.DbType.Int64;
            }
            catch (System.Exception)
            {
                // add error handling to suit your taste
            }

            return DBtype;
        } // End Function GetDbType


        protected virtual string SqlTypeFromDbType(System.Data.DbType type)
        {
            string strRetVal = null;

            // http://msdn.microsoft.com/en-us/library/cc716729.aspx
            switch (type)
            {
                case System.Data.DbType.Guid:
                    strRetVal = "uniqueidentifier";
                    break;
                case System.Data.DbType.Date:
                    strRetVal = "date";
                    break;
                case System.Data.DbType.Time:
                    strRetVal = "time(7)";
                    break;
                case System.Data.DbType.DateTime:
                    strRetVal = "datetime";
                    break;
                case System.Data.DbType.DateTime2:
                    strRetVal = "datetime2";
                    break;
                case System.Data.DbType.DateTimeOffset:
                    strRetVal = "datetimeoffset(7)";
                    break;

                case System.Data.DbType.StringFixedLength:
                    strRetVal = "nchar(MAX)";
                    break;
                case System.Data.DbType.String:
                    strRetVal = "nvarchar(MAX)";
                    break;

                case System.Data.DbType.AnsiStringFixedLength:
                    strRetVal = "char(MAX)";
                    break;
                case System.Data.DbType.AnsiString:
                    strRetVal = "varchar(MAX)";
                    break;

                case System.Data.DbType.Single:
                    strRetVal = "real";
                    break;
                case System.Data.DbType.Double:
                    strRetVal = "float";
                    break;
                case System.Data.DbType.Decimal:
                    strRetVal = "decimal(19, 5)";
                    break;
                case System.Data.DbType.VarNumeric:
                    strRetVal = "numeric(19, 5)";
                    break;

                case System.Data.DbType.Boolean:
                    strRetVal = "bit";
                    break;
                case System.Data.DbType.SByte:
                case System.Data.DbType.Byte:
                    strRetVal = "tinyint";
                    break;
                case System.Data.DbType.Int16:
                    strRetVal = "smallint";
                    break;
                case System.Data.DbType.Int32:
                    strRetVal = "int";
                    break;
                case System.Data.DbType.Int64:
                    strRetVal = "bigint";
                    break;
                case System.Data.DbType.Xml:
                    strRetVal = "xml";
                    break;
                case System.Data.DbType.Binary:
                    strRetVal = "varbinary(MAX)"; // or image
                    break;
                case System.Data.DbType.Currency:
                    strRetVal = "money";
                    break;
                case System.Data.DbType.Object:
                    strRetVal = "sql_variant";
                    break;
                    
                case System.Data.DbType.UInt16:
                case System.Data.DbType.UInt32:
                case System.Data.DbType.UInt64:
                    throw new System.NotImplementedException("Uints not mapped - MySQL only");
            }

            return strRetVal;
        } // End Function SqlTypeFromDbType


        public virtual System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue)
        {
            return AddParameter(command, strParameterName, objValue, System.Data.ParameterDirection.Input);
        } // End Function AddParameter


        public virtual System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad)
        {
            if (objValue == null)
            {
                //throw new ArgumentNullException("objValue");
                objValue = System.DBNull.Value;
            } // End if (objValue == null)

            System.Type tDataType = objValue.GetType();
            System.Data.DbType dbType = GetDbType(tDataType);

            return AddParameter(command, strParameterName, objValue, pad, dbType);
        } // End Function AddParameter


        public virtual System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad, System.Data.DbType dbType)
        {
            System.Data.IDbDataParameter parameter = command.CreateParameter();

            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            } // End if (!strParameterName.StartsWith("@"))

            parameter.ParameterName = strParameterName;
            parameter.DbType = dbType;
            parameter.Direction = pad;

            // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
            // No association  DbType UInt64 to a known SqlDbType

            if (objValue == null)
                parameter.Value = System.DBNull.Value;
            else
                parameter.Value = objValue;

            command.Parameters.Add(parameter);
            return parameter;
        } // End Function AddParameter


        public virtual T GetParameterValue<T>(System.Data.IDbCommand idbc, string strParameterName)
        {
            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            }

            return InlineTypeAssignHelper<T>(((System.Data.IDbDataParameter)idbc.Parameters[strParameterName]).Value);
        } // End Function GetParameterValue<T>


        public virtual void CreateTable(string strTableName)
        {
            string strSQL = GetEmbeddedSQLscript("CreateTable_[" + strTableName + "].sql");
            CreateTable(strTableName, strSQL);
        }


        public virtual void CreateTable(string strTableName, string strSQL)
        {
            throw new System.NotImplementedException("cDAL.CreateTable not implemented.");
        }


        public virtual void DropTable(string strTableName)
        {
            throw new System.NotImplementedException("cDAL.DropTable not implemented.");
        }


        public void ExecuteScripts(string strSQL)
        {
            Subtext.Scripting.SqlScriptRunner sr = new Subtext.Scripting.SqlScriptRunner(strSQL);
            ExecuteScripts(ref sr, true);
        }


        public void ExecuteScript(string strScriptName)
        {
            Subtext.Scripting.SqlScriptRunner sr = GetScriptRunner(strScriptName);
            ExecuteScripts(ref sr, true);
        }


        public void ExecuteScripts(ref Subtext.Scripting.SqlScriptRunner sr)
        {
            ExecuteScripts(ref sr, false);
        }


        public void ExecuteScripts(ref Subtext.Scripting.SqlScriptRunner sr, bool bDispose)
        {
            foreach (Subtext.Scripting.Script script in sr.ScriptCollection)
            {
                string strSQL = script.ScriptText;
                Execute(strSQL);
                strSQL = null;
            } // Next script

            if (bDispose)
            {
                sr.ScriptCollection.Clear();
                sr.TemplateParameters.Clear();
                sr = null;
            } // End if(bDispose)

        } // End Sub ExecuteSQLstatement


        protected static T InlineTypeAssignHelper<T>(object UTO)
        {
            if (UTO == null)
            {
                T NullSubstitute = default(T);
                return NullSubstitute;
            }
            return (T)UTO;
        } // End Template InlineTypeAssignHelper


        ////////////////////////////// Schema //////////////////////////////

        public virtual void CreateDB()
        {
            CreateDB("", "", "");
        }


        // http://web.firebirdsql.org/dotnetfirebird/create-a-new-database-from-an-sql-script.html
        public virtual void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
        {
            try
            {

                try
                {
                    // Create a new database
                    //System.Data.Odbc.OdbcConnection.CreateDatabase(this.m_ConnectionString.ConnectionString);
                }
                catch (System.Data.Odbc.OdbcException ex)
                {
                    if (ex.ErrorCode == 335544344)
                        Log("This database already exists.");
                    else
                        Log(ex.Message);
                }
            } // End Try
            catch (System.Exception ex)
            {
                if (Log("cDAL.CreateDB(string strDBname, string strDataLocation, string strLogLocation)", ex, "CreateDB"))
                    throw;
            } // End Catch
            finally
            {
                //System.Threading.Thread.Sleep(2000); // Wait for disk-write complete
            } // End Finally
        }


        public virtual System.Data.DataTable GetAllColumnInformation(string TableName)
        {
            return GetDataTable(string.Format(@"
SELECT 
	* 
FROM information_schema.columns 
WHERE (1=1) 
AND TABLE_NAME = '{0}' 

ORDER BY ordinal_position
", this.SqlEscapeString(TableName)));

        }





        public virtual System.Data.DataTable GetTables()
        {
            return GetTables(null);
        }


        public virtual System.Data.DataTable GetTables(string strInitialCatalog)
        {
            //string strCatalog = this.m_DatabaseConfiguration.strInitialCatalog;

            //if (!string.IsNullOrEmpty(strCatalog))
            //    strCatalog = strCatalog.Trim();

            //if (string.IsNullOrEmpty(strCatalog))
            //    return null;

            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
AND TABLE_CATALOG = @strInitialCatalog 

ORDER BY TABLE_SCHEMA, TABLE_NAME ASC 
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

                return this.GetDataTable(cmd, strInitialCatalog);
            }

        } // End Function GetTables


        public virtual System.Data.DataTable GetViews()
        {
            return GetViews(null);
        }


        public virtual System.Data.DataTable GetViews(string strInitialCatalog)
        {
            if (string.IsNullOrEmpty(strInitialCatalog))
                return null;

            strInitialCatalog = strInitialCatalog.Trim();

            string strSQL = @"
SELECT * 
FROM INFORMATION_SCHEMA.views 
WHERE (1=1) 
AND table_catalog = @strInitialCatalog 
--AND table_schema = xxx
;
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

                return this.GetDataTable(cmd, strInitialCatalog);
            }

        } // End Function GetViews


        public virtual System.Data.DataTable GetProcedures()
        {
            return GetProcedures(null);
        }

        public virtual System.Data.DataTable GetProcedures(string strInitialCatalog)
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE (1=1) 
AND DATA_TYPE IS NULL 

ORDER BY SPECIFIC_NAME ASC 
";

            return this.GetDataTable(strSQL, strInitialCatalog);
        }


        public virtual System.Data.DataTable GetFunctions()
        {
            return GetFunctions(null);
        }


        public virtual System.Data.DataTable GetFunctions(string strInitialCatalog)
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE (1=1) 
AND DATA_TYPE NOT IN('TABLE', 'record') 
AND routine_catalog = @strCatalog 
-- AND routine_schema = 'crapbot' 
AND routine_type = 'FUNCTION' 

ORDER BY SPECIFIC_NAME ASC 
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strCatalog", strInitialCatalog);

                return GetDataTable(cmd, strInitialCatalog);
            } // End Using cmd

        }


        public abstract System.Data.DataTable GetRoutines();
        public virtual System.Data.DataTable GetRoutines(string strInitialCatalog) 
        { 
            throw new System.NotImplementedException("GetRoutines(string strInitialCatalog)"); 
        }

        public virtual System.Data.DataTable GetTableValuedFunctions()
        {
            return GetTableValuedFunctions(null);
        }

        public virtual System.Data.DataTable GetTableValuedFunctions(string strInitialCatalog)
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE (1=1) 
AND DATA_TYPE IN('TABLE', 'record') 
AND routine_catalog = @strInitialCatalog  

ORDER BY SPECIFIC_NAME ASC 
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

                return this.GetDataTable(cmd, strInitialCatalog);
            } // End Using cmd

        } // End Function GetTableValuedFunctions



        public abstract System.Data.DataTable GetColumnNames();


        public abstract System.Data.DataTable GetColumnNamesForTable(string strTableName);
        public virtual System.Data.DataTable GetColumnNamesForTable(string strTableName, string strDbName)
        {
            throw new System.NotImplementedException("cDAL.GetColumnNamesForTable(string strTableName, string strDbName) not implemented for this db type.");
            //return null;
        }


        public virtual System.Data.DataTable GetRoutineParameters(string strRoutineName)
        {
            return GetRoutineParameters(strRoutineName, null);
        }


        public virtual System.Data.DataTable GetRoutineParameters(string strRoutineName, string strDbName)
        {
            System.Data.DataTable dt = null;

            string strSQL = @"
SELECT 
--ip.*
	 ip.parameter_name
	,ip.parameter_mode
	,ip.data_type
	,ip.character_maximum_length
	,ip.is_result
	,ip.as_locator
FROM information_schema.routines AS ir 

LEFT JOIN information_schema.parameters AS ip 
	ON ip.specific_name = ir.specific_name 

WHERE (1=1) 
AND (ir.routine_name = @in_strRoutineName) 
AND ORDINAL_POSITION > 0 
ORDER BY ir.ROUTINE_NAME, ir.SPECIFIC_NAME, ip.ORDINAL_POSITION 
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "in_strRoutineName", strRoutineName);

                //this.GetDataTable(cmd,"");
                dt = this.GetDataTable(cmd, strDbName);
            } // End Using cmd


            return dt;
        } // End Function GetRoutineParameters


        public virtual string[] GetTableColumns(string strTableName)
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            using (System.Data.DataTable dt = GetColumnNamesForTable(strTableName))
            {

                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    ls.Add(System.Convert.ToString(dr["COLUMN_NAME"]));
                } // Next dr 

            } // End Using dt

            string[] astrReturnValue = ls.ToArray();
            ls.Clear();
            ls = null;

            return astrReturnValue;
        } // End Function GetTableColumns


        public virtual string GenerateSqlInserts(string strTableName)
        {
            return GenerateSqlInserts(strTableName, strTableName);
        }


        public virtual string GenerateSqlInserts(string strSourceTableName, string strTargetTableName)
        {
            string[] Felder = GetTableColumns(strSourceTableName);
            System.Data.DataTable dt = GetDataTable("SELECT * FROM " + EscapeTableName(strSourceTableName));

            return GenerateSqlInserts(Felder, dt, strTargetTableName);
        }


        public virtual string GenerateSqlInserts(string[] aryColumns,
                                    System.Data.DataTable dtTable,
                                    string sTargetTableName)
        {
            return GenerateSqlInserts(aryColumns, dtTable, sTargetTableName, false);
        }


        public virtual string GenerateSqlInserts(string[] aryColumns,
                                            System.Data.DataTable dtTable,
                                            string sTargetTableName, bool bDebug)
        {
            string sSqlInserts = string.Empty;
            System.Text.StringBuilder sbSqlStatements = new System.Text.StringBuilder(string.Empty);

            // create the columns portion of the INSERT statement
            string sColumns = string.Empty;
            foreach (string colname in aryColumns)
            {
                if (sColumns != string.Empty)
                    sColumns += ", ";

                sColumns += EscapeColumnName(colname);
            } // Next colname

            // loop thru each record of the datatable
            foreach (System.Data.DataRow drow in dtTable.Rows)
            {
                // loop thru each column, and include
                // the value if the column is in the array
                string sValues = string.Empty;
                foreach (string col in aryColumns)
                {
                    if (sValues != string.Empty)
                        sValues += ", ";

                    // need to do a case to check the column-value types
                    // (quote strings(check for dups first), convert bools)

                    try
                    {
                        if (drow[col] == System.DBNull.Value)
                        {
                            if (bDebug)
                                sValues += "NULL" + " -- " + col + System.Environment.NewLine;
                            else
                                sValues += "NULL";
                        }
                        else
                        {
                            if (bDebug)
                                sValues += Insert_Unicode(drow[col]) + " -- " + col + System.Environment.NewLine;
                            else
                                sValues += Insert_Unicode(drow[col]);
                        }
                        /*
                        sType = drow[col].GetType().ToString();
                        switch (sType.Trim().ToLower())
                        {
                            case "system.boolean":
                                sValues += Insert_Unicode(drow[col]); 
                                //sValues += (Convert.ToBoolean(drow[col]) == true ? "1" : "0");
                                break;

                            case "system.string":
                                sValues += Insert_Unicode(drow[col]);   
                                break;

                            case "system.datetime":
                                sValues += Insert_Unicode(drow[col]);
                                break;

                            default:
                                if (drow[col] == System.DBNull.Value)
                                    sValues += "NULL";
                                else
                                    sValues += Convert.ToString(drow[col]);
                                    sValues += Insert_Unicode(drow[col]);
                                break;
                        }
                        */
                    }
                    catch
                    {
                        if (bDebug)
                            sValues += Insert_Unicode(drow[col]) + " -- " + col + System.Environment.NewLine;
                        else
                            sValues += Insert_Unicode(drow[col]);
                    }
                } // Next col 

                //   INSERT INTO Tabs(Name) 
                //      VALUES('Referrals')
                // write the insert line out to the stringbuilder
                string snewsql = string.Format("INSERT INTO {0}({1}) ", EscapeTableName(sTargetTableName), sColumns);
                sbSqlStatements.Append(snewsql);
                sbSqlStatements.AppendLine();
                snewsql = string.Format("VALUES({0});", sValues);
                sbSqlStatements.Append(snewsql);
                sbSqlStatements.AppendLine();
                sbSqlStatements.AppendLine();
            } // Next drow

            sSqlInserts = sbSqlStatements.ToString();
            return sSqlInserts;
        } // End Function GenerateSqlInserts



        public virtual string EscapeColumnName(string strColumnName)
        {
            return EscapeTableName(strColumnName);
        }


        public virtual string EscapeSchemaName(string strSchemaName)
        {
            return EscapeTableName(strSchemaName);
        }


        public virtual string EscapeTableName(string strTableName)
        {
            return "\"" + strTableName.Replace("\"", "\"\"") + "\"";
        }


        public virtual string SqlEscapeString(string unsafeSqlString)
        {
            if (string.IsNullOrEmpty(unsafeSqlString))
                return unsafeSqlString;

            return unsafeSqlString.Replace("'", "''");
        }

        public virtual string CreateSafeUpdateTableCommand(string strTableName)
        {
            string strSQL = @"
UPDATE " + EscapeTableName(strTableName) + @" 
SET 
";

            using (System.Data.DataTable dt = this.GetColumnNamesForTable(strTableName))
            {

                for (int i = 0; i < this.GetColumnNamesForTable(strTableName).Rows.Count; ++i)
                {
                    if (i != 0)
                    {
                        strSQL += ",";
                    }
                    strSQL += EscapeTableName(System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]));
                    strSQL += "-- Definition " + System.Environment.NewLine;


                } // Next strColumnName
            }
            strSQL += @"
FROM " + EscapeTableName(strTableName) + @" AS " + EscapeTableName(strTableName + "_ForJoining") + @"
WHERE (1=1) 
";

            return strSQL;
        }


        public virtual string CreateSafeDeleteTableCommand(string strTableName)
        {
            string strSQL = @"
BEGIN TRANSACTION
	DELETE FROM " + EscapeTableName(strTableName) + @"  OUTPUT DELETED.*
	WHERE(1=1)
ROLLBACK TRANSACTION
";

            return strSQL;
        }




        public virtual string GetTableSelectText(string strTableName) { throw new System.NotImplementedException("GetTableSelectText not implemented."); }

        public virtual string GetTableCreateText(string strTableName, string strNewTableName) { throw new System.NotImplementedException("GetTableCreateText not implemented."); }
        public virtual string GetTableCreateText(string strTableName)
        {
            return GetTableCreateText(strTableName, strTableName);
        }


        public virtual System.Collections.Generic.List<string> SplitScript(string strSQL)
        {
            return DAL.Scripting.ScriptSplitter.SplitScript(strSQL);
        }


        public virtual string GetTableDropText(string schemaname, string strTableName)
        {
            string strNewLine = "\r\n"; //  Environment.NewLine
            string escSchemaName = schemaname.Replace("'", "''");
            string escTableName = strTableName.Replace("'", "''");

            string strSQL = strNewLine + "IF  0 < (SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE LOWER(TABLE_SCHEMA) = LOWER('" + escSchemaName + "') AND LOWER(TABLE_NAME) = LOWER('" + escTableName + "')) " + strNewLine;
            strSQL += "DROP TABLE [" + escSchemaName + "]." + EscapeTableName(strTableName) + " " + strNewLine + strNewLine;

            return strSQL;
        } // End Function GetTableDropText


        public virtual string GetTableDropCreateText(string strSchema, string strTableName)
        {
            string strNewLine = "\r\n";
            string strDropCreateScript = GetTableDropText(strSchema, strTableName) + strNewLine + "GO" + strNewLine + strNewLine + strNewLine + GetTableCreateText(strTableName, strTableName) + strNewLine + strNewLine + "GO" + strNewLine;
            return strDropCreateScript;
        }


        public virtual System.Data.DataTable GetPrimaryKeys() { throw new System.NotImplementedException("GetPrimaryKeys not implemented."); }
        public virtual System.Data.DataTable GetPrimaryKeysForTable(string strTableName) { throw new System.NotImplementedException("GetPrimaryKeys(string strTableName) not implemented."); }


        public virtual string GetTableInsertTemplate(string strTableName) { throw new System.NotImplementedException("GetTableInsertTemplate(string strTableName) not implemented."); }
        public virtual string GetTableInsertText(string strTableName) { throw new System.NotImplementedException("GetTableInsertText(string strTableName) not implemented."); }
        public virtual string GetInsertScript(string strTableName) { throw new System.NotImplementedException("GetInsertScript(string strTableName) not implemented."); }



        public virtual string GetTableDeleteText(string strTableName)
        {
            string strNewLine = "\r\n"; //  Environment.NewLine
            string strSQL = strNewLine + "DELETE FROM " + EscapeTableName(strTableName) + " " + strNewLine;
            strSQL += "WHERE " + "(1=2) " + strNewLine;

            return strSQL;
        } // End Function GetTableDeleteText


        public abstract bool TableExists(string strTableName);
        public abstract bool IsTableEmpty(string strTableName);
        public abstract bool TableHasColumn(string strTableName, string strColumnName);

        ////////////////////////////// End Schema //////////////////////////////

        public virtual DataBaseEngine_t DBtype   // Abstract property
        {
            get { return this.m_dbtDBtype; }
        } // End Property DBtype


        public abstract string DBversion   // Abstract property
        {
            get;
        } // End Property DBversion


        public static void MsgBox(object obj)
        {
            MsgBox(obj, "");
        } // End Sub MsgBox


        public static void MsgBox(object obj, string strTitle)
        {
            if (obj != null)
            {
                if (obj.GetType().ToString().EndsWith("Exception"))
                {
                    System.Exception ex = (System.Exception)obj;
                    string strMessage = "";
                    if (ex != null)
                    {
                        strMessage = ex.Message;
                        if (ex.InnerException != null)
                        {
                            strMessage += System.Environment.NewLine;
                            strMessage += ex.InnerException.Message;
                            strMessage += "Inner stacktrace: " + ex.InnerException.StackTrace;
                        } // End if (ex.InnerException != null)
                        strMessage += "Stacktrace: " + ex.StackTrace;
                    } // End if (ex != null)

                    //System.Windows.Forms.MessageBox.Show(strMessage);
                    strMessage = null;
                } // End if(obj.GetType().ToString().EndsWith("Exception"))
                else
                    System.Console.WriteLine(obj.ToString());
                //System.Windows.Forms.MessageBox.Show(obj.ToString());
            } // End if (obj != null)
            else
                System.Console.WriteLine("obj = null");
            //System.Windows.Forms.MessageBox.Show("obj = null");
        } // End Sub MsgBox


        public System.Data.Common.DbProviderFactory GetFactory<T>()
        {
            System.Type t = typeof(T);
            return GetFactory(t);
        } // End Function GetFactory


        public virtual System.Data.Common.DbProviderFactory GetFactory(string assemblyType)
        {
#if TARGET_JVM // case insensitive GetType is not supported
			Type type = Type.GetType (assemblyType, false);
#else
            System.Type type = System.Type.GetType(assemblyType, false, true);
#endif

            return GetFactory(type);
        } // End Function GetFactory


        public virtual System.Data.Common.DbProviderFactory GetFactory(System.Type type)
        {
            if (type != null && type.IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))
            {
                // Provider factories are singletons with Instance field having
                // the sole instance
                System.Reflection.FieldInfo field = type.GetField("Instance"
                    , System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static
                );

                if (field != null)
                {
                    return (System.Data.Common.DbProviderFactory)field.GetValue(null);
                    //return field.GetValue(null) as DbProviderFactory;
                } // End if (field != null)

            } // End if (type != null && type.IsSubclassOf(typeof(System.Data.Common.DbProviderFactory)))

            throw new System.Configuration.ConfigurationErrorsException("DataProvider is missing!");
            //throw new System.Configuration.ConfigurationException("DataProvider is missing!");
        } // End Function GetFactory


        public virtual string GetInsertString(string strTableName)
        {
            string strSQL;

            using (System.Data.DataTable dt = this.GetColumnNamesForTable(strTableName))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("INSERT INTO ");
                sb.AppendLine(EscapeTableName(strTableName));
                sb.AppendLine("( ");

                // @
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    if (i == 0)
                        sb.Append("     ");
                    else
                        sb.Append("    ,");

                    sb.AppendLine(System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]));
                } // Next i



                sb.AppendLine(") ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    if (i == 0)
                        sb.Append("     ");
                    else
                        sb.Append("    ,");

                    sb.Append("__@");
                    sb.Append(System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]));
                    sb.Append(" -- ");
                    sb.Append(System.Convert.ToString(dt.Rows[i]["DATA_TYPE"]));


                    string strMaxLen = System.Convert.ToString(dt.Rows[i]["CHARACTER_MAXIMUM_LENGTH"]);

                    if (!string.IsNullOrEmpty(strMaxLen))
                    {
                        sb.Append("(");
                        sb.Append(strMaxLen);
                        sb.Append(")");
                    } // End if (!string.IsNullOrEmpty(strMaxLen))

                    sb.Append(", ");

                    if (System.StringComparer.OrdinalIgnoreCase.Equals(System.Convert.ToString(dt.Rows[i]["IS_NULLABLE"]), "YES"))
                        sb.AppendLine("NULL");
                    else
                        sb.AppendLine("NOT NULL");
                } // Next i

                // ,@COLUMN_NAME

                sb.AppendLine(") ");

                strSQL = sb.ToString();
                sb = null;
            } // End Using using (System.Data.DataTable dt

            return strSQL;
        } // End Function GetInsertString




        // http://www.yaldex.com/sql_server/progsqlsvr-APP-A-SECT-1.html
        public virtual string GetAvailableFactoryClasses()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            System.Data.DataTable dt = System.Data.Common.DbProviderFactories.GetFactoryClasses();

            foreach (System.Data.DataRow row in dt.Rows)
            {

                sb.AppendLine(string.Format("Name:                  {0}", row["Name"]));
                sb.AppendLine(string.Format("Description:           {0}", row["Description"]));
                sb.AppendLine(string.Format("InvariantName:         {0}", row["InvariantName"]));
                sb.AppendLine(string.Format("AssemblyQualifiedName: {0}", row["AssemblyQualifiedName"]));
            } // Next row

            return sb.ToString();
        } // End FunctionGetAvailableFactoryClasses


        public virtual void AddFactoryClasses()
        {
            // http://msdn.microsoft.com/en-us/data/dd363565
            // http://programming.2be-it.com/?p=160
            // http://stackoverflow.com/questions/1117683/add-a-dbproviderfactory-without-an-app-config
            try
            {
                var dataSet = System.Configuration.ConfigurationManager.GetSection("system.data") as System.Data.DataSet;
                dataSet.Tables[0].Rows.Add("Firebird Data Provider", ".Net Framework Data Provider for Firebird", "FirebirdSql.Data.FirebirdClient", "FirebirdSql.Data.FirebirdClient.FirebirdClientFactory, FirebirdSql.Data.FirebirdClient");
                dataSet.Tables[0].Rows.Add("MySQL Data Provider", ".Net Framework Data Provider for MySQL", "MySql.Data.MySqlClient", "MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data");
                dataSet.Tables[0].Rows.Add("Npgsql Data Provider", ".Net Framework Data Provider for Postgresql Server", "Npgsql", "Npgsql.NpgsqlFactory, Npgsql");
                //dataSet.Tables[0].Rows.Add("SQLite Data Provider", ".Net Framework Data Provider for SQLite", "System.Data.SQLite", "System.Data.SQLite.SQLiteFactory, System.Data.SQLite"); 
            }
            catch (System.Data.ConstraintException)
            { }
            // FirebirdSql.Data.FirebirdClient.FirebirdClientFactory, FirebirdSql.Data.FirebirdClient, Version=2.6.5.0, Culture=neutral, PublicKeyToken=3750abcc3150b00c
            // Npgsql.NpgsqlFactory, Npgsql, Version=2.0.11.91, Culture=neutral, PublicKeyToken=5d8b90d52f46fda7
            // MySql.Data.MySqlClient.MySqlClientFactory, MySql.Data, Version=6.3.7.0, Culture=neutral, PublicKeyToken=c5687fc88969c44d
        } // End Sub AddFactoryClasses


        public static cDAL CreateInstance()
        {
            throw new System.NotImplementedException("CreateInstance");
        } // End Function CreateInstance


        public static cDAL CreateInstance(DataBaseEngine_t DataBaseEngine, string strConnectionString)
        {
            string strBaseNameSpace = typeof(cMS_SQL).Namespace;
            string strType = DataBaseEngine.ToString();

            try
            {
                return (cDAL)System.Activator.CreateInstance(
                    System.Type.GetType(strBaseNameSpace + ".c" + strType)
                    , new object[] { strConnectionString }
                );
            } // End try
            catch (System.Exception ex)
            {
                MsgBox(ex);
            } // End catch

            return null;
        } // End Function CreateInstance


        public static cDAL CreateInstance(string strInputType, string strConnectionString)
        {
            if (string.IsNullOrEmpty(strInputType))
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                string strMethodName = stackTrace.GetFrame(1).GetMethod().Name;
                throw new System.NullReferenceException("Error in cDAL.CreateInstance: strInputType is NULL or empty. " 
                    + System.Environment.NewLine + "Calling method: \"" + strMethodName + "\".");
            } // End if (string.IsNullOrEmpty(strInputType))

            if (string.IsNullOrEmpty(strConnectionString))
            {
                System.Diagnostics.StackTrace stackTrace = new System.Diagnostics.StackTrace();
                string strMethodName = stackTrace.GetFrame(1).GetMethod().Name;
                throw new System.NullReferenceException("Error in cDAL.CreateInstance: strConnectionString is NULL or empty. " 
                    + System.Environment.NewLine + "Calling method: \"" + strMethodName + "\".");
            } // End if (string.IsNullOrEmpty(strConnectionString))


            if (string.IsNullOrEmpty(strInputType))
            {
                throw new System.ArgumentNullException(strInputType);
            } // End if (string.IsNullOrEmpty(strInputType))


            string strBaseNameSpace = typeof(cMS_SQL).Namespace;

            string strType = null;
            foreach (string strEnumName in System.Enum.GetNames(typeof(DataBaseEngine_t)))
            {

                if (System.StringComparer.OrdinalIgnoreCase.Equals(strInputType, strEnumName))
                {
                    strType = strEnumName;
                    break;
                } // End if (StringComparer.OrdinalIgnoreCase.Equals(strInputType, strEnumName))

            } // Next strEnumName

            if (string.IsNullOrEmpty(strType))
            {
                throw new System.InvalidOperationException("There's no DataBaseEngine_t member called \"" + strInputType + "\".");
            } // End if (string.IsNullOrEmpty(strType))


            try
            {
                return (cDAL)System.Activator.CreateInstance(
                    System.Type.GetType(strBaseNameSpace + ".c" + strType)
                    , new object[] { strConnectionString }
                );
            } // End try
            catch (System.Exception ex)
            {
                MsgBox(ex);
            } // End catch

            return null;
        } // End Function CreateInstance


        public static cDAL CreateInstance(string strType)
        {
            string strBaseNameSpace = typeof(cMS_SQL).Namespace;

            foreach (string strEnumName in System.Enum.GetNames(typeof(DataBaseEngine_t)))
            {

                if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, strEnumName))
                {
                    strType = strEnumName;
                    break;
                } // End if (StringComparer.OrdinalIgnoreCase.Equals(strType, strEnumName))

            } // Next strEnumName


            try
            {
                return (cDAL)System.Activator.CreateInstance(System.Type.GetType(strBaseNameSpace + ".c" + strType));
            } // End try
            catch (System.Exception ex)
            {
                MsgBox(ex);
            } // End Catch


            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "MS_SQL"))
                return new cMS_SQL();

            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "Oracle"))
                return new cOracle();

#if WITH_GPL
            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "MySQL"))
                return new cMySQL();
#endif

            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "PostGreSQL"))
                return new cPostGreSQL();

            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "FireBird"))
                return new cFireBird();

            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "Access"))
                return new cAccess();

#if !__MonoCS__
            // System.Data.OleDb.OleDbConnectionStringBuilder not implemented in mono
            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "OleDb"))
                return new cOleDB();
#endif

            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "ODBC"))
                return new cODBC();

            if (System.StringComparer.OrdinalIgnoreCase.Equals(strType, "Sybase"))
                throw new System.Exception("Sybase connector for .NET not available anywhere, neither for free nor pirated.");

            throw new System.NotImplementedException("Either you made a typo, or this database type is not supported.");
        } // End Function CreateInstance


        protected System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> m_dictScriptTemplates = null;


        private static System.Reflection.Assembly getWebApplicationAssemblyEverywhere()
        {
            System.Reflection.Assembly assReturnValue = null;

            // assReturnValue = System.Web.Compilation.BuildManager.GetGlobalAsaxType().BaseType.Assembly() ' Requires .NET 4

            // I get "This method cannot be called during the application's pre-start initialization stage." error
            // while trying to use this method in ASP.NET MVC.
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.ApplicationInstance == null)
            {
                assReturnValue = System.Reflection.Assembly.GetEntryAssembly();

                // NULL in web application
                if (assReturnValue != null)
                    return assReturnValue;

                if (System.Web.HttpContext.Current != null)
                {
                    string AspNetNamespace = "ASP";

                    //System.Web.IHttpHandler handler = context.CurrentHandler;
                    System.Web.IHttpHandler handler = System.Web.HttpContext.Current.Handler;
                    if (handler == null)
                        return null;

                    System.Type tHandler = handler.GetType();
                    while (tHandler != null && !object.ReferenceEquals(tHandler, typeof(object)) && tHandler.Namespace == AspNetNamespace)
                        tHandler = tHandler.BaseType;

                    return tHandler.Assembly;
                } // End if (System.Web.HttpContext.Current != null)

                return assReturnValue;
            } // End if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.ApplicationInstance == null)

            // System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.ApplicationInstance == null
            System.Type tApplicatonInstance = System.Web.HttpContext.Current.ApplicationInstance.GetType();

            while (tApplicatonInstance != null && tApplicatonInstance.Namespace == "ASP")
            {
                tApplicatonInstance = tApplicatonInstance.BaseType;
            }


            assReturnValue = tApplicatonInstance == null ? null : tApplicatonInstance.Assembly;

            if (assReturnValue == null)
            {
                throw new System.NullReferenceException("Could not determine web application assembly");
            }

            return assReturnValue;
        } // End Function getWebApplicationAssemblyEverywhere


        public static System.Reflection.Assembly GetMVCentryAssembly()
        {
            System.Reflection.Assembly[] AppAssemblies = System.AppDomain.CurrentDomain.GetAssemblies();

            foreach (System.Reflection.Assembly ass in AppAssemblies)
            {

                if (ass != null && ass.FullName != null)
                {
                    // str = ass.GetName().Name; // System.Security.SecurityException: 
                    // Request for the permission of type 'System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089' failed.

                    // Since Assembly.GetName method needs to accessing the actual file location of the assembly, 
                    // and this operation requires the caller has FileIOPermissionAccess.PathDiscovery permission, 
                    // if the caller has no such permission, 
                    // in other words, the caller is partial trust, then a Security Exception will be thrown.

                    // Assembly.FullName doesn't try to locate the physical assembly file, a partial trust caller is allowed.
                    if (ass.FullName.StartsWith("Mono.Posix", System.StringComparison.OrdinalIgnoreCase))
                        continue;

                    else if (ass.FullName.StartsWith("FirebirdSql.Data.FirebirdClient", System.StringComparison.OrdinalIgnoreCase))
                        continue;

                } // End if (ass != null && ass.FullName != null)

                try
                {
                    foreach (System.Type t in ass.GetTypes())
                    {

                        if (t.Name == "MvcApplication")
                        {
                            return t.Assembly;
                        } // End if (t.Name == "MvcApplication")

                    } // Next t
                }
                catch (System.Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }

            } // Next ass

            return null;
        } // End Function GetMVCentryAssembly


        /// <summary>
        /// Version of 'GetEntryAssembly' that works with web applications
        /// </summary>
        /// <returns>The entry assembly, or the first called assembly in a web application</returns>
        public static System.Reflection.Assembly GetMVCentryAssembly_OnlyPartiallyWorking()
        {
            var result = System.Reflection.Assembly.GetEntryAssembly();

            // if none (ex: web application)
            if (result == null)
            {
                // current method
                System.Reflection.MethodBase methodCurrent = null;
                // number of frames to skip
                int framestoSkip = 1;


                // loop until we cannot got further in the stacktrace
                do
                {
                    // get the stack frame, skipping the given number of frames
                    System.Diagnostics.StackFrame stackFrame = new System.Diagnostics.StackFrame(framestoSkip);

                    methodCurrent = stackFrame.GetMethod();
                    // if found
                    if ((methodCurrent != null)
                        // and if that method is not excluded from the stack trace
                        //&& (methodCurrent.GetAttribute<ExcludeFromStackTraceAttribute>(false) == null)
                        )
                    {
                        // get its type
                        var typeCurrent = methodCurrent.DeclaringType;

                        if (typeCurrent != null)
                        {
                            // if valid
                            if (typeCurrent != typeof(System.RuntimeMethodHandle))
                            {
                                // get its assembly
                                var assembly = typeCurrent.Assembly;

                                string str = assembly.FullName;

                                // if valid
                                if (!assembly.GlobalAssemblyCache
                                    // && !assembly.IsDynamic
                                    //&& (assembly.GetAttribute<System.CodeDom.Compiler.GeneratedCodeAttribute>() == null)
                                    )
                                {
                                    // then we found a valid assembly, get it as a candidate
                                    result = assembly;
                                }

                            } // End if (typeCurrent != typeof(RuntimeMethodHandle))

                        } // End if (typeCurrent != null)
                        else
                            break;
                    }

                    // increase number of frames to skip
                    framestoSkip++;
                } // while we have a working method
                while (methodCurrent != null);
            }
            return result;
        } // End Function GetMVCentryAssembly


        private static System.Reflection.Assembly getWebApplicationAssembly()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;

            if (context == null)
                return null;

            string AspNetNamespace = "ASP";

            //System.Web.IHttpHandler handler = context.CurrentHandler;
            System.Web.IHttpHandler handler = context.Handler;
            if (handler == null)
                return null;

            System.Type type = handler.GetType();
            while (type != null && !object.ReferenceEquals(type, typeof(object)) && type.Namespace == AspNetNamespace)
                type = type.BaseType;

            return type.Assembly;
        } // End Function getWebApplicationAssembly


        protected static System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> GetSQLtemplates()
        {
            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> dictTempScripts = 
                new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>
                    (System.StringComparer.OrdinalIgnoreCase);

            System.Reflection.Assembly asmDataSourceAssembly = System.Reflection.Assembly.GetEntryAssembly();


            if (asmDataSourceAssembly == null)
                asmDataSourceAssembly = GetMVCentryAssembly();

            // http://stackoverflow.com/questions/756031/using-the-web-application-version-number-from-an-assembly-asp-net-c
            if (asmDataSourceAssembly == null)
                asmDataSourceAssembly = getWebApplicationAssembly();

            if (asmDataSourceAssembly == null)
            {
                //throw new Exception("asmDataSourceAssembly not found.");
                return null;
            } // End if(asmDataSourceAssembly == null)

            string str = asmDataSourceAssembly.FullName;

            //System.Reflection.Assembly asmDataSourceAssembly = System.Reflection.Assembly.GetCallingAssembly();
            //System.Windows.Forms.MessageBox.Show(asmDataSourceAssembly.FullName);
            //Console.WriteLine("Used assembly: " + asmDataSourceAssembly.Location);

            string strRessourceRoot = null;
            bool bFound = false;
            foreach (string strThisRessourceName in asmDataSourceAssembly.GetManifestResourceNames())
            {
                if (strThisRessourceName.EndsWith("RessourceBase.location.txt"))
                {
                    strRessourceRoot = strThisRessourceName.Substring(0, strThisRessourceName.Length - "RessourceBase.location.txt".Length);
                    //Console.WriteLine(strRessourceRoot);
                    bFound = true;
                    break;
                } // End if (strThisRessourceName.EndsWith("RessourceBase.location")) 

            } // Next strThisRessourceName

            if (!bFound)
                return null;

            foreach (string strThisRessourceName in asmDataSourceAssembly.GetManifestResourceNames())
            {
                if (strThisRessourceName.Length <= strRessourceRoot.Length)
                    continue;

                string strRessource = strThisRessourceName.Substring(strRessourceRoot.Length);

                if (strRessource.StartsWith("SQL"))
                {
                    strRessource = strRessource.Substring("SQL.".Length);
                    int iFileNameStartPos = strRessource.IndexOf('.') + 1;

                    string strRessourceName = strRessource.Substring(iFileNameStartPos);
                    string strFolder = strRessource.Substring(0, iFileNameStartPos - 1);
                    //System.Collections.Generic.Dictionary<string, string> dictScripts = 

                    if (!dictTempScripts.ContainsKey(strFolder))
                    {
                        dictTempScripts.Add(strFolder,
                            new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase)
                        );
                    } // End if (!dictScripts.ContainsKey(strFolder))

                    //Console.WriteLine(strRessourceName);
                    dictTempScripts[strFolder].Add(strRessourceName, strThisRessourceName);
                } // End if(strRessource.StartsWith("SQL"))

            } // Next strThisRessourceName


            if (dictTempScripts.ContainsKey("All"))
            {

                foreach (string strScript in dictTempScripts["All"].Keys)
                {
                    foreach (string strThisFolder in dictTempScripts.Keys)
                    {
                        if (!System.StringComparer.Ordinal.Equals(strThisFolder, "All"))
                        {
                            if (!dictTempScripts[strThisFolder].ContainsKey(strScript))
                                dictTempScripts[strThisFolder].Add(strScript, dictTempScripts["All"][strScript]);
                        } // End if (!StringComparer.Ordinal.Equals(strThisFolder, "All"))

                    } // Next strThisFolder

                } // Next strScript

            } // End if (dictTempScripts.ContainsKey("All"))


            System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>> dictScripts = 
                new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, string>>
                    (System.StringComparer.OrdinalIgnoreCase);

            foreach (string strThisFolder in dictTempScripts.Keys)
            {
                //Console.WriteLine(strThisFolder);
                dictScripts.Add(strThisFolder, 
                    new System.Collections.Generic.Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase));

                foreach (string strThisKey in dictTempScripts[strThisFolder].Keys)
                {
                    //Console.WriteLine(dictTempScripts[strThisFolder][strThisKey]);
                    System.IO.Stream strm = asmDataSourceAssembly.GetManifestResourceStream(dictTempScripts[strThisFolder][strThisKey]);
                    System.IO.StreamReader sr = new System.IO.StreamReader(strm);

                    dictScripts[strThisFolder].Add(strThisKey, sr.ReadToEnd());
                    sr.Close();
                    sr.Dispose();
                } // Next strThisKey

                dictTempScripts[strThisFolder].Clear();
            } // Next strThisFolder

            asmDataSourceAssembly = null;
            dictTempScripts.Clear();
            dictTempScripts = null;

            return dictScripts;
        } // End Function GetSQLtemplates


        public Subtext.Scripting.SqlScriptRunner GetScriptRunner(string strScriptName)
        {
            string strSQL = GetEmbeddedSQLscript(strScriptName);
            Subtext.Scripting.SqlScriptRunner sr = new Subtext.Scripting.SqlScriptRunner(strSQL);
            strSQL = null;
            return sr;
        } // End Function GetScriptRunner


        public string GetEmbeddedSQLscript(string strScriptName)
        {
            //string strDebug = this.m_dbtDBtype.ToString();
            string strDBtype = this.m_dbtDBtype.ToString();

            if (m_dictScriptTemplates == null)
                return null;

            if (m_dictScriptTemplates.ContainsKey(strDBtype))
            {
                if (m_dictScriptTemplates[strDBtype].ContainsKey(strScriptName))
                    return m_dictScriptTemplates[strDBtype][strScriptName];
                else
                    throw new System.Exception("Script \"" + strScriptName + "\" not found in folder \"" + strDBtype + "\" ...");
            } // End if(m_dictScriptTemplates.ContainsKey(strDBtype))

            if (m_dictScriptTemplates.ContainsKey("All"))
            {
                if (m_dictScriptTemplates["All"].ContainsKey(strScriptName))
                    return m_dictScriptTemplates["All"][strScriptName];
                else
                    throw new System.Exception("Script \"" + strScriptName + "\" not found in folder \"All\" ...");
            }
            else
                throw new System.Exception("No scripts in folder \"All\" ...");

            //return null;
        } // End Function GetEmbeddedSQLscript


        // dictTempScripts[strThisFolder][strThisKey] = sr.ReadToEnd();
        // COR.DataManagement.SQL.GetEmbeddedSQLscript("");
        protected string GetEmbeddedSQLscript_old(string strScriptName)
        {
            string strRessourceName = "";

            foreach (string strThisRessourceName in System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames())
            {

                //Console.WriteLine(strThisRessourceName);
                if (strThisRessourceName.EndsWith(strScriptName))
                {
                    System.Console.WriteLine(strThisRessourceName);
                    strRessourceName = strThisRessourceName;
                    break;
                } // End if (strThisRessourceName.EndsWith(strScriptName))

            } // Next strThisRessourceName

            string strFileContent = "";

            if (!string.IsNullOrEmpty(strRessourceName))
            {
                System.IO.Stream strm = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(strRessourceName);
                System.IO.StreamReader sr = new System.IO.StreamReader(strm);
                strFileContent = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();
            }
            else
            {
                throw new System.Exception("Ressource \"" + strScriptName + "\" not found.");
            } // End else of if (!string.IsNullOrEmpty(strRessourceName))

            return strFileContent;
        } // End Function GetEmbeddedSQLscript_old


        //<System.Runtime.CompilerServices.Extension()> _
        public static string SecureString2String(System.Security.SecureString securePassword)
        {
            string strReturnValue = null;

            if (securePassword == null)
            {
                return strReturnValue;
            } // End if (securePassword == null)

            System.IntPtr unmanagedString = System.IntPtr.Zero;
            try
            {
                unmanagedString = System.Runtime.InteropServices.Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                strReturnValue = System.Runtime.InteropServices.Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }

            return strReturnValue;
        } // End Function SecureString2String
        //http://blogs.msdn.com/b/fpintos/archive/2009/06/12/how-to-properly-convert-securestring-to-string.aspx


        public static System.Security.SecureString String2SecureString(string strInputString)
        {
            System.Security.SecureString ssReturnValue = null;

            if (!string.IsNullOrEmpty(strInputString))
            {
                ssReturnValue = new System.Security.SecureString();
                foreach (char chThisChar in strInputString.ToCharArray())
                {
                    ssReturnValue.AppendChar(chThisChar);
                } // Next chThisChar

                ssReturnValue.MakeReadOnly();
            } // End if (!string.IsNullOrEmpty(strInputString))

            if (strInputString == "")
            {
                ssReturnValue = new System.Security.SecureString();
                return ssReturnValue;
            } // End if(strInputString == "")

            return ssReturnValue;
        } // End Function String2SecureString

        // Credits: http://web.archive.org/web/20100123183531/http://blogs.msdn.com/bclteam/archive/2005/03/15/396452.aspx
        protected const string strOdbcMatchingPattern = "({fn\\s*(.+?)\\s*\\(([^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!)))\\s*\\)\\s*})";

        protected virtual string ReplaceOdbcFunctions(string strSQL)
        {
            if (string.IsNullOrEmpty(strSQL))
            {
                return strSQL;
            }

            //Dim strOdbcMatchingPattern As String = "{fn LCASE(.*)}"
            //strOdbcMatchingPattern = "<customtag>(.+?)</customtag>"
            //strOdbcMatchingPattern = "{fn\s*(LCASE)\s*\((.+?)\s*\)\s*}"
            //'strOdbcMatchingPattern = "{fn\s*(.+?)\s*\((.+?)\s*\)\s*}"
            //'strOdbcMatchingPattern = "(<[^<>]*(((?<Open><)[^<>]*)+((?<Close-Open>>)[^<>]*)+)*(?(Open)(?!))>)"
            //strOdbcMatchingPattern = "({[^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!))})" ' match parantheses

            //strOdbcMatchingPattern = "({fn\s*[^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!))})" ' match parantheses
            //string strOdbcMatchingPattern = "({fn\\s*(.+?)\\s*\\(([^{}]*(((?<Open>{)[^{}]*)+((?<Close-Open>})[^{}]*)+)*(?(Open)(?!)))\\s*\\)\\s*})";
            // match parantheses

            //str = System.Text.RegularExpressions.Regex.Replace(str, strPattern, "equivalent")


            string strReturnValue = "";
            strReturnValue = System.Text.RegularExpressions.Regex.Replace(strSQL, strOdbcMatchingPattern, OdbcFunctionReplacementCallback);
            return strReturnValue;
        } // End Function ReplaceOdbcFunctions


        // MsgBox(String.Join(Environment.NewLine, GetArguments("bla")));
        protected virtual string[] GetArguments(string strAllArguments)
        {
            string EscapeCharacter = System.Convert.ToChar(8).ToString();

            strAllArguments = strAllArguments.Replace("''", EscapeCharacter);

            bool bInString = false;
            int iLastSplitAt = 0;

            System.Collections.Generic.List<string> lsArguments = new System.Collections.Generic.List<string>();


            int iInFunction = 0;

            for (int i = 0; i < strAllArguments.Length; i++)
            {
                char strCurrentChar = strAllArguments[i];

                if (strCurrentChar == '\'')
                    bInString = !bInString;


                if (bInString)
                    continue;


                if (strCurrentChar == '(')
                    iInFunction += 1;


                if (strCurrentChar == ')')
                    iInFunction -= 1;



                if (strCurrentChar == ',')
                {

                    if (iInFunction == 0)
                    {
                        string strExtract = "";
                        if (iLastSplitAt != 0)
                        {
                            strExtract = strAllArguments.Substring(iLastSplitAt + 1, i - iLastSplitAt - 1);
                        }
                        else
                        {
                            strExtract = strAllArguments.Substring(iLastSplitAt, i - iLastSplitAt);
                        }

                        strExtract = strExtract.Replace(EscapeCharacter, "''");
                        lsArguments.Add(strExtract);
                        iLastSplitAt = i;
                    } // End if (iInFunction == 0)

                } // End if (strCurrentChar == ',')

            } // Next i


            string strExtractLast = "";
            if (lsArguments.Count > 0)
            {
                strExtractLast = strAllArguments.Substring(iLastSplitAt + 1);
            }
            else
            {
                strExtractLast = strAllArguments.Substring(iLastSplitAt);
            }

            strExtractLast = strExtractLast.Replace(EscapeCharacter, "''");
            lsArguments.Add(strExtractLast);

            string[] astrResult = lsArguments.ToArray();
            lsArguments.Clear();
            lsArguments = null;

            return astrResult;
        } // End Function GetArguments



        protected virtual string[] GetArguments_RegexSuxs(string strAllArguments)
        {
            //Dim strPattern As String = "\s*(\w+|\'(\'\'|.)*\')\s*(\,|$)"
            string strPattern = "\\s*(\\w+|\\'(\\'\\'|[^'])*\\')\\s*(\\,|$)";

            // strAllArguments = "'hello d''amato, d''alambert world', 'test', foo, bar";
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(strAllArguments, strPattern);

            System.Collections.Generic.List<string> lsArgumentList = new System.Collections.Generic.List<string>();

            foreach (System.Text.RegularExpressions.Match ma in mc)
            {
                //Console.WriteLine(ma.Groups(1).Value)
                lsArgumentList.Add(ma.Groups[1].Value);
            }

            string[] astrArgumentList = lsArgumentList.ToArray();
            lsArgumentList.Clear();
            lsArgumentList = null;

            return astrArgumentList;
        } // End Function GetArguments_RegexSuxs


        protected virtual string OdbcFunctionReplacementCallback(System.Text.RegularExpressions.Match mThisMatch)
        {
            // Get the matched string.
            string strExpression = mThisMatch.Groups[1].Value;
            string strFunctionName = mThisMatch.Groups[2].Value;
            string strArguments = mThisMatch.Groups[3].Value;

            if (System.Text.RegularExpressions.Regex.IsMatch(strArguments, strOdbcMatchingPattern))
            {
                strArguments = System.Text.RegularExpressions.Regex.Replace(strArguments, strOdbcMatchingPattern, OdbcFunctionReplacementCallback);
            }


            // Simple one or 0 arguments
            // if (StringComparer.OrdinalIgnoreCase.Equals("lcase", strFunctionName))
            // { return "LOWER(" + strArguments + ") "; }





            string[] astrArguments = GetArguments(strArguments);

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals("ilike", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " LIKE " + astrArguments[1] + " ) ";
                return strTerm;
            }

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals("like", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " COLLATE Latin1_General_BIN LIKE " + astrArguments[1] + " ) ";
                return strTerm;
            }



            // if (StringComparer.OrdinalIgnoreCase.Equals("left", strFunctionName))
            // { string strTerm = "LPAD(" + astrArguments[0] + ", " + astrArguments[1] + ", '') "; return strTerm;}

            return "ODBC FUNCTION \"" + strFunctionName + "\" not defined in abstraction layer...";
        } // End Function OdbcFunctionReplacementCallback


        public static string RowNumberReplacementCallback(System.Text.RegularExpressions.Match mThisMatch)
        {
            //string strExpression = mThisMatch.Groups[1].Value;
            //string strFunctionName  = mThisMatch.Groups[2].Value;
            //string strArguments = mThisMatch.Groups[3].Value;

            string strFieldName = mThisMatch.Groups["Alias"].Value;

            if (string.IsNullOrEmpty(strFieldName))
                strFieldName = "PagingRowId";

            return "1 AS " + strFieldName + " ";
        } // End Function RowNumberReplacementCallback


        public static string RemoveRowNumberFromSqlStatement(string strSQL)
        {
            const string strRowNumberOverPattern = @"(row_number\s+?\(\)\s*(OVER)\s*\(([^\(\)]*(((?<Open>\()[^\(\)]*)+((?<Close-Open>\))[^\(\)]*)+)*(?(Open)(?!)))\s*\)\s*)(\s*AS\s*(?<Alias>\w+))?\s*";


            if (System.Text.RegularExpressions.Regex.IsMatch(strSQL, strRowNumberOverPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
                return System.Text.RegularExpressions.Regex.Replace(strSQL, strRowNumberOverPattern, RowNumberReplacementCallback, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            return strSQL;
        } // End Function RemoveRowNumberFromSqlStatement


        public virtual string RemoveCstyleComments(string strInput)
        {
            if (string.IsNullOrEmpty(strInput))
                return strInput;

            // http://stackoverflow.com/questions/462843/improving-fixing-a-regex-for-c-style-block-comments 
            string strPattern = @"/\*(?>(?:(?>[^*]+)|\*(?!/))*)\*/";  // Works ! 

            string strOutput = System.Text.RegularExpressions.Regex.Replace(strInput, strPattern, string.Empty, System.Text.RegularExpressions.RegexOptions.Multiline);
            return strOutput;
        } // End Function RemoveCstyleComments 


        public virtual string RemoveSingleLineSqlComments(string strSQLwithComments)
        {
            if (string.IsNullOrEmpty(strSQLwithComments))
                return strSQLwithComments;

            strSQLwithComments = strSQLwithComments.Replace("\r", "\n");
            strSQLwithComments = strSQLwithComments.Replace("\n\n", "\n");

            string[] astrLines = strSQLwithComments.Split('\n');

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (string strThisLine in astrLines)
            {
                if (!string.IsNullOrEmpty(strThisLine))
                {
                    string strTrimmedLine = strThisLine.Trim();
                    if (!strTrimmedLine.StartsWith("--"))
                    {
                        if (!string.IsNullOrEmpty(strTrimmedLine))
                        {
                            sb.AppendLine(strTrimmedLine);
                        } // End if (!string.IsNullOrEmpty(strTrimmedLine))

                    } // End if (!strTrimmedLine.StartsWith("--"))

                } // End if (!string.IsNullOrEmpty(strThisLine))

            } // Next strThisLine

            string strReturnValue = sb.ToString();
            sb = null;

            return strReturnValue;
        } // End Function RemoveSingleLineSqlComments


        public virtual string ConvertTopNStatement(string strSQL)
        {
            if (string.IsNullOrEmpty(strSQL))
                return strSQL;

            strSQL = RemoveSingleLineSqlComments(strSQL);
            string strPattern = "SELECT[\\s]*TOP[\\s]*(\\d*)[\\s]*";

            System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(strSQL, strPattern, System.Text.RegularExpressions.RegexOptions.Singleline);

            if ((match.Success))
            {
                string strLimit = match.Groups[1].Value;

                strSQL = System.Text.RegularExpressions.Regex.Replace(strSQL, strPattern, "", System.Text.RegularExpressions.RegexOptions.Singleline);
                strSQL = "SELECT " + System.Environment.NewLine + strSQL + " " + System.Environment.NewLine
                    + "LIMIT " + strLimit + " " + System.Environment.NewLine;
            } // End if ((match.Success))

            return strSQL;
        } // End Function ConvertTopNStatement


        // http://www.sqlusa.com/bestpractices/datetimeconversion/
        const string DATEFORMAT = "{0:yyyyMMdd}"; // YYYYMMDD ISO date format works at any language setting - international standard
        const string DATETIMEFORMAT = "{0:yyyy'-'MM'-'dd'T'HH:mm:ss.fff}"; // ISO 8601 format: international standard - works with any language setting


        public virtual string GetParametrizedQueryText(System.Data.IDbCommand cmd)
        {
            string strReturnValue = "";

            try
            {
                System.Text.StringBuilder msg = new System.Text.StringBuilder();
                System.DateTime dtLogTime = System.DateTime.UtcNow;
                

                if (cmd == null || string.IsNullOrEmpty(cmd.CommandText))
                {
                    return strReturnValue;
                } // End if (cmd == null || string.IsNullOrEmpty(cmd.CommandText))


                if (cmd.Parameters != null && cmd.Parameters.Count > 0)
                {
                    msg.AppendLine("-- ***** Listing Parameters *****");

                    foreach (System.Data.IDataParameter idpThisParameter in cmd.Parameters)
                    {
                        // http://msdn.microsoft.com/en-us/library/cc716729.aspx
                        msg.AppendLine(string.Format("DECLARE {0} AS {1} -- DbType: {2}", idpThisParameter.ParameterName, SqlTypeFromDbType(idpThisParameter.DbType), idpThisParameter.DbType.ToString()));
                    } // Next idpThisParameter

                    msg.AppendLine(System.Environment.NewLine);
                    msg.AppendLine(System.Environment.NewLine);

                    foreach (System.Data.IDataParameter idpThisParameter in cmd.Parameters)
                    {
                        string strParameterValue = null;
                        if (object.ReferenceEquals(idpThisParameter.Value, System.DBNull.Value))
                        {
                            strParameterValue = "NULL";
                        }
                        else
                        {
                            if (idpThisParameter.DbType == System.Data.DbType.Date)
                                strParameterValue = System.String.Format(DATEFORMAT, idpThisParameter.Value);
                            else if (idpThisParameter.DbType == System.Data.DbType.DateTime || idpThisParameter.DbType == System.Data.DbType.DateTime2)
                                strParameterValue = System.String.Format(DATETIMEFORMAT, idpThisParameter.Value);
                            else
                                strParameterValue = idpThisParameter.Value.ToString();

                            strParameterValue = "'" + strParameterValue.Replace("'", "''") + "'";
                        }

                        msg.AppendLine(string.Format("SET {0} = {1}", idpThisParameter.ParameterName, strParameterValue));
                    } // Next idpThisParameter

                    msg.AppendLine("-- ***** End Parameter Listing *****");
                    msg.AppendLine(System.Environment.NewLine);
                } // End if cmd.Parameters != null || cmd.Parameters.Count > 0 



                msg.AppendLine(string.Format("-- ***** Start Query from {0} *****", dtLogTime.ToString(DATEFORMAT)));
                msg.AppendLine(cmd.CommandText);
                msg.AppendLine(string.Format("-- ***** End Query from {0} *****", dtLogTime.ToString(DATEFORMAT)));
                msg.AppendLine(System.Environment.NewLine);

                strReturnValue = msg.ToString();
                msg = null;
            } // End Try
            catch (System.Exception ex)
            {
                strReturnValue = "Error in Function cDAL.GetParametrizedQueryText";
                strReturnValue += System.Environment.NewLine;
                strReturnValue += ex.Message;
            } // End Catch

            return strReturnValue;
        } // End Function GetParametrizedQueryText


        public static bool Log(object obj)
        {
            return Log(obj, (string)null);
        } // End Sub Log(object obj)


        public static bool Log(object obj, System.Data.IDbCommand cmd)
        {
            string strSQL = "";
            foreach (System.Data.IDbDataParameter para in cmd.Parameters)
            {
                strSQL += para.ParameterName + ":\t" + para.Value.ToString() + "" + System.Environment.NewLine;
            } // Next para

            strSQL += System.Environment.NewLine + System.Environment.NewLine + System.Environment.NewLine;

            strSQL += cmd.CommandText;

            return Log("", obj, strSQL);
        } // End Sub Log


        public static bool Log(object obj, string strSQL)
        {
            return Log("", obj, strSQL);
        } // End Sub Log


        public static bool Log(string strLocation, object obj, string strSQL)
        {
            return Log(strLocation, obj, strSQL, true);
        } // End Sub Log


        public static bool Log(string strLocation, object obj, string strSQL, bool bRethrow)
        {
            if (!string.IsNullOrEmpty(strLocation))
            {
                System.Console.WriteLine("Error in " + strLocation + ": " + System.Environment.NewLine + System.Environment.NewLine);
            } // End if (!string.IsNullOrEmpty(strLocation))

            if (!string.IsNullOrEmpty(strSQL))
            {
                System.Console.WriteLine("SQL statement: " + System.Environment.NewLine + strSQL + System.Environment.NewLine + System.Environment.NewLine);
            } // End if(!string.IsNullOrEmpty(strSQL))

            if (obj != null)
            {
                if (obj.GetType().ToString().EndsWith("Exception"))
                {
                    System.Exception ex = (System.Exception)obj;
                    string strMessage = "";
                    if (ex != null)
                    {
                        strMessage = ex.Message;
                        if (ex.InnerException != null)
                        {
                            strMessage += System.Environment.NewLine;
                            strMessage += ex.InnerException.Message + System.Environment.NewLine + System.Environment.NewLine;
                            strMessage += "Inner stacktrace: " + System.Environment.NewLine 
                                + ex.InnerException.StackTrace + System.Environment.NewLine
                                + System.Environment.NewLine;
                        } // End if (ex.InnerException != null)
                        strMessage += System.Environment.NewLine + System.Environment.NewLine + "Stacktrace: " + ex.StackTrace + System.Environment.NewLine;
                    } // End if (ex != null)

                    System.Console.WriteLine(strMessage);
                    strMessage = null;
                } // End if(obj.GetType().ToString().EndsWith("Exception"))
                else
                    System.Console.WriteLine(obj.ToString());
            } // End if (obj != null)
            else
                System.Console.WriteLine("obj = null");


            try
            {
                System.Console.WriteLine(System.Environment.NewLine + System.Environment.NewLine);
                System.Console.WriteLine(new string('=', System.Console.WindowWidth));
                System.Console.WriteLine(System.Environment.NewLine + System.Environment.NewLine);
            }
            catch //(Exception ex)
            {

            }

            return bRethrow;
        } // End Sub Log


    } // End Abstract Class cDAL


} // End Namespace DataBase.Tools
