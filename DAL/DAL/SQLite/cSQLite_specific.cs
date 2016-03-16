
#define USE_MONO_SQLITE

#if USE_MONO_SQLITE
using SQLiteConnection = Mono.Data.Sqlite.SqliteConnection;
using SQLiteCommand = Mono.Data.Sqlite.SqliteCommand;
using SQLiteDataAdapter = Mono.Data.Sqlite.SqliteDataAdapter;
using SQLiteParameter = Mono.Data.Sqlite.SqliteParameter;
//using SqlLite_Regex = Mono.Data.Sqlite.SqlLite_Regex;

using SQLiteConnectionStringBuilder = Mono.Data.Sqlite.SqliteConnectionStringBuilder;
using SQLiteFactory = Mono.Data.Sqlite.SqliteFactory;

#else
using System.Data.SQLite;
#endif


namespace DB.Abstraction
{

	partial class cSQLite : cDAL
    {

		#if USE_MONO_SQLITE
		internal const string SQLITE_DLL = "sqlite3.dll";
		internal const string SQLITE_SOURCE_DLL = "System.Data.SQLite.x{0}.dll";
		#else
		internal const string SQLITE_DLL = "SQLite.Interop.dll";
		internal const string SQLITE_SOURCE_DLL = "SQLite.Interop.x{0}.dll";
		#endif

		protected SQLiteConnectionStringBuilder m_ConnectionString;
        

		public override bool IsSQLite
        {
            get { return true; }
        }


        // http://en.csharp-online.net/CSharp_FAQ:_How_can_one_constructor_call_another
		public cSQLite()
            : this("")
        {
            // Crap !
        } // End Constructor 0


        public cSQLite(string strConnectionString)
        {
            CopySQLite();

            this.m_dbtDBtype = DataBaseEngine_t.SQLite;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            //this.m_dblDBversion = 8.0;
            this.m_ConnectionString = new SQLiteConnectionStringBuilder(strConnectionString);
        } // End Constructor 1


        public static string CombinePath(params string[] path)
        {
            string strPath = path[0];

            for (int i = 1; i < path.Length; ++i)
            {
                strPath = System.IO.Path.Combine(strPath, path[i]);
            }

            return strPath;
        } // End Function CombinePath


		public static void CopySQLite()
		{
			string strTargetDir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
			string strSource = CombinePath(strTargetDir, "Libs", string.Format(SQLITE_SOURCE_DLL, System.IntPtr.Size * 8));

			strTargetDir = System.IO.Path.Combine(strTargetDir, SQLITE_DLL);

            if (System.Environment.OSVersion.Platform == System.PlatformID.Unix)
			{
				if (System.IO.File.Exists(strTargetDir))
					System.IO.File.Delete(strTargetDir);

				return;
			}

			System.IO.File.Copy(strSource, strTargetDir, true);
		} // End Sub CopySQLite


        public override string GetConnectionString(string strDb)
        {
			if(string.IsNullOrEmpty(strDb))
            	return this.m_ConnectionString.ConnectionString;

            SQLiteConnectionStringBuilder csb = new SQLiteConnectionStringBuilder(this.m_ConnectionString.ConnectionString);
            csb.DataSource = strDb;
			string strRetVal = csb.ConnectionString;
			
            // csb.Clear ();
			csb = null;

			return strRetVal;
        } // End Function GetConnectionString


        public System.Data.Common.DbProviderFactory GetFactory()
        {
            //AddFactoryClasses();
            System.Data.Common.DbProviderFactory providerFactory = null;

            providerFactory = this.GetFactory(typeof(SQLiteFactory));

            return providerFactory;
        } // End Function GetFactory



        public override string Insert_Unicode(object obj)
        {
            string strText = GetTextFromObject(obj);

            if (string.IsNullOrEmpty(strText))
                return "NULL";

            strText = strText.Replace("'", "''");

            return "N'" + strText + "'";
        } // End Function Insert_Unicode


        // http://stackoverflow.com/questions/7412944/convert-datetime-to-hex-equivalent-in-vb-net
        public string DateTimeAsHexString(System.DateTime dt)
        {
            //dt = System.DateTime.Parse("2012-04-27 14:32:13.510");
            //dt = New DateTime(2012, 4, 27, 14, 32, 13, 510)
            System.DateTime zero = new System.DateTime(1900, 1, 1);

            System.TimeSpan ts = dt - zero;
            System.TimeSpan ms = ts.Subtract(new System.TimeSpan(ts.Days, 0, 0, 0));

            //resolution for datetime: 3.33 ms ...
            //BUG @ New DateTime(2012, 12, 31, 23, 59, 59, 999)
            //string hex = "0x" + ts.Days.ToString("X8") + ((int)(ms.TotalMilliseconds / 3.3333333333)).ToString("X8");
            string hex = "0x" + ts.Days.ToString("X8") + System.Convert.ToInt32(
                System.Math.Floor(ms.TotalMilliseconds / 3.3333333333)).ToString("X8");

            return hex;
        } // End Function DateTimeAsHexString


        // http://msdn.microsoft.com/en-us/library/ms190646.aspx
        public string GetExecutionPlanAsXML(string strExpression)
        {
            try
            {
                this.Execute("SET SHOWPLAN_XML ON;");
                // Safely execute a query.
                //strExpression = "SELECT * ROM T_Benutzer";
                strExpression = this.ExecuteScalar<string>(strExpression);
            }
            catch
            {
                this.Execute("SET SHOWPLAN_XML OFF;");
                throw;
            }

            this.Execute("SET SHOWPLAN_XML OFF;");
            
            return strExpression;
        } // End Function GetExecutionPlanAsXML




        public override VersionInfo ServerVersionInfo()
        {
            VersionInfo vi = new VersionInfo();

            vi.RawVersion = ExecuteScalar<string>("SELECT @@version");
            vi.BitField = ExecuteScalar<int>("SELECT @@microsoftversion");


            // @@VERSION
            // SELECT @@microsoftversion
            // SELECT @@microsoftversion / 0x01000000 AS Major 
            // SELECT (@@microsoftversion & 0xFF0000) / 0x10000 AS Release 
            // SELECT @@microsoftversion & 0xFFFF AS Build

            // exec xp_msver
            // --MicrosoftVersion: 0xFFFFFFFF
            // --FF Mainversion:   0xFF000000
            // --FF Release:       0x00FF0000
            // --FFFF Build:       0x0000FFFF
            // -- Bitshift:
            // -- 0xABCD / 0x100 = AB

            //            string strSQL = @"
            //DECLARE @ver int 
            //SET @ver = @@microsoftversion / 0x01000000 
            //--SELECT @ver
            //
            //IF ( @ver = 7 )
            //   SELECT 'SQL Server 7'
            //ELSE IF ( @ver = 8 )
            //   SELECT 'SQL Server 2000'
            //ELSE IF ( @ver = 9 )
            //   SELECT 'SQL Server 2005'
            //ELSE IF ( @ver = 10 AND ((@@microsoftversion & 0xFF0000) / 0x10000) = 50 )
            //   SELECT 'SQL Server 2008 R2'
            //ELSE IF ( @ver = 10 )
            //   SELECT 'SQL Server 2008 R1'
            //ELSE IF ( @ver = 11 )
            //   SELECT 'SQL Server 2012'
            //ELSE IF ( @ver > 11 )
            //   SELECT 'New untested SQL Server Version'
            //ELSE
            //   SELECT 'Unsupported SQL Server Version'
            //";   


            vi.Major = vi.BitField / 0x01000000;
            vi.Release = (vi.BitField & 0xFF0000) / 0x10000;
            vi.Build = vi.BitField & 0xFFFF;

            if (vi.Major == 7)
                vi.Version = "SQL Server 7";
            else if (vi.Major == 8)
                vi.Version = "SQL Server 2000";
            else if (vi.Major == 9)
                vi.Version = "SQL Server 2005";
            else if (vi.Major == 10 && vi.Release == 50)
                vi.Version = "SQL Server 8 R2";
            else if (vi.Major == 10)
                vi.Version = "SQL Server 8 R1";
            else if (vi.Major == 11)
                vi.Version = "SQL Server 2012";
            else if (vi.Major > 11)
                vi.Version = "SQL Server New Version";
            else // Pre SQL-Server 7
                vi.Version = "Unsupported SQL Server Version";

            return vi;
        } // End Function ServerVersionInfo


        public override bool BulkCopy(string strDestinationTable, System.Data.DataTable dt)
        {
            return BulkCopy(strDestinationTable, dt, false);
        }


        // BulkCopy("dbo.T_Benutzer", dt)
        public override bool BulkCopy(string strDestinationTable, System.Data.DataTable dt, bool bWithDelete)
        {
            try
            {

                strDestinationTable = "[" + strDestinationTable + "]";

                if (bWithDelete)
                {
                    this.Execute("DELETE FROM " + strDestinationTable.Replace("'","''"));
                }


                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlbulkcopyoptions.aspx
                System.Data.SqlClient.SqlBulkCopyOptions bcoOptions = //System.Data.SqlClient.SqlBulkCopyOptions.CheckConstraints |
                                                                      System.Data.SqlClient.SqlBulkCopyOptions.KeepNulls | 
                                                                      System.Data.SqlClient.SqlBulkCopyOptions.KeepIdentity;

                // http://stackoverflow.com/questions/6651809/sqlbulkcopy-insert-with-identity-column
                // http://msdn.microsoft.com/en-us/library/ms186335.aspx
                System.Data.SqlClient.SqlBulkCopy BulkCopyInstance = new System.Data.SqlClient.SqlBulkCopy(this.m_ConnectionString.ConnectionString, bcoOptions);
                foreach(System.Data.DataColumn dc in dt.Columns)
                {

                    BulkCopyInstance.ColumnMappings.Add(dc.ColumnName, "[" + dc.ColumnName + "]");
                }
                BulkCopyInstance.DestinationTableName = strDestinationTable;

                /*
                string strSQL = "INSERT INTO " + BulkCopyInstance.DestinationTableName + Environment.NewLine + "(" + Environment.NewLine;


                for(int i=0; i < dt.Columns.Count; ++i)
                {
                    if(i==0)
                        strSQL += "       [" + dt.Columns[i].ColumnName + "]" + Environment.NewLine;
                    else
                        strSQL += "      ,[" + dt.Columns[i].ColumnName + "]" + Environment.NewLine;
                    //BulkCopyInstance.ColumnMappings.Add(dc.ColumnName, dc.ColumnName); 
                }
                strSQL += ") " + Environment.NewLine + "Values "+ Environment.NewLine + "(" + Environment.NewLine;

                for (int i = 0; i < dt.Columns.Count; ++i)
                {
                    if (i == 0)
                        strSQL += "       @parameter" + i.ToString() + Environment.NewLine;
                    else
                        strSQL += "      ,@parameter" + i.ToString() + Environment.NewLine;
                    //BulkCopyInstance.ColumnMappings.Add(dc.ColumnName, dc.ColumnName); 
                }

                strSQL += "); ";

                // http://www.codeproject.com/KB/cs/MultipleInsertsIn1dbTrip.aspx#_Toc196622244
                System.Data.IDbCommand idbc = this.CreateCommand(strSQL);

                for (int i = 0; i < dt.Rows.Count; ++i)
                {

                    for (int j = 0; j < dt.Columns.Count; ++j)
                    {
                        this.AddParameter(idbc, "parameter" + j.ToString(), dt.Rows[i][j]);
                    }

                    //this.Execute(idbc);
                    this.ExecuteWithoutTransaction(idbc);
                    idbc.Parameters.Clear();
                }

                //MsgBox(strSQL);
                */
                BulkCopyInstance.WriteToServer(dt);
                BulkCopyInstance.Close();
                BulkCopyInstance = null;
            }
            catch (System.Exception ex)
            {
                if (Log("cSQLite_specific.cs ==> BulkCopy", ex, "BulkCopy: Copy dt to " + strDestinationTable))
                    throw;
                //COR.Logging.WriteLogFile("FEHLER", "Ausnahme in COR.SQL.MSSQL.BulkCopy");
                //COR.Logging.WriteLogFile("FEHLER", ex.Message);
                //COR.Logging.WriteLogFile("FEHLER", "-----------------------------------------------------------------");
                //COR.Logging.WriteLogFile("FEHLER", ex.StackTrace.ToString());
                //Console.WriteLine(ex.Message.ToString() + Environment.NewLine + ex.StackTrace.ToString()); //MsgBoxStyle.Critical, "FEHLER ...");
                //COR.Logging.WriteLogFile("MELDUNG", "-----------------------------------------------------------------");
            }

            return false;
        } // End Function BulkCopy


        public System.Data.DataTable ListTriggers()
        {
            string strSQL = @"
SELECT 
     sysobjects.name AS trigger_name 
    ,USER_NAME(sysobjects.uid) AS trigger_owner 
    ,s.name AS table_schema 
    ,OBJECT_NAME(parent_obj) AS table_name 
    ,OBJECTPROPERTY( id, 'ExecIsUpdateTrigger') AS isupdate 
    ,OBJECTPROPERTY( id, 'ExecIsDeleteTrigger') AS isdelete 
    ,OBJECTPROPERTY( id, 'ExecIsInsertTrigger') AS isinsert 
    ,OBJECTPROPERTY( id, 'ExecIsAfterTrigger') AS isafter 
    ,OBJECTPROPERTY( id, 'ExecIsInsteadOfTrigger') AS isinsteadof 
    ,OBJECTPROPERTY(id, 'ExecIsTriggerDisabled') AS [disabled] 
FROM sysobjects 

INNER JOIN sys.tables AS t 
    ON sysobjects.parent_obj = t.object_id 
    
INNER JOIN sys.schemas AS s 
    ON t.schema_id = s.schema_id 
    
WHERE sysobjects.type = 'TR' 
";

            return this.GetDataTable(strSQL);
        } // End Function ListTriggers


        public override System.Data.IDbCommand CreatePagedCommand(string strSQL, ulong ulngPageNumber, ulong ulngPageSize)
        {
            ulong ulngStartIndex = ((ulngPageSize * ulngPageNumber) - ulngPageSize) + 1;
            ulong ulngEndIndex = ulngStartIndex + ulngPageSize - 1;


            strSQL = this.RemoveCstyleComments(strSQL);
            strSQL = this.RemoveSingleLineSqlComments(strSQL);

            System.Text.RegularExpressions.Regex r = new System.Text.RegularExpressions.Regex("SELECT", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            strSQL = r.Replace(strSQL, "SELECT TOP 99.999999999999999999999 PERCENT ", 1);
            r = null;



#if false
-- Thy that not implements proper paging methods shall suffer the performance hit 
-- That way we can keep the query the same on all systems 
--SELECT TOP 99.999999999999999999999 PERCENT 

WITH WorkAround_2 
AS 
(
    -- WorkAround_1
    SELECT 
         ANY_UID
        ,ANY_Object
        ,ANY_Table
        ,ANY_FieldPrfx
        ,ANY_TicketNr
        ,ANY_Prio
        ,ANY_Date
        ,ANY_Status
    FROM T_Anything 
    ORDER BY T_Anything.ANY_Datum ASC
)
,
WorkAround_3 AS
(
    SELECT 
         * 
        ,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS rowid 
    FROM WorkAround_2 
) 

SELECT * FROM WorkAround_3 
WHERE rowid > 0 and rowid < 11 
#endif



            string strSQLinternal = @"
WITH WorkAround_2 
AS 
(
	-- WorkAround_1
    " + strSQL + @"
	
)
,
WorkAround_3 AS
(
	SELECT 
		 * 
		,ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS fhasdfsdajflsadfjasdflk_RowId_jfsalkdfjlasdfkasdfjlk 
	FROM WorkAround_2 
) 
SELECT * FROM WorkAround_3 
WHERE fhasdfsdajflsadfjasdflk_RowId_jfsalkdfjlasdfkasdfjlk BETWEEN @ulngStartIndex AND @ulngEndIndex  
";


            //strSQL = RemoveRowNumberFromSqlStatement(strSQL);
            
            System.Data.IDbCommand cmd = this.CreateCommand(strSQLinternal);

            this.AddParameter(cmd, "ulngStartIndex", ulngStartIndex);
            this.AddParameter(cmd, "ulngEndIndex", ulngEndIndex);
            return cmd;
        }


        public override System.Exception ShrinkDb(string DbName, int Percent)
        {
            string strSQL = null;
            throw new System.NotImplementedException("ShrinkDb");
            try
            {
                System.Console.WriteLine("Shrink DB");
            }
            catch (System.Exception ex)
            {
                Log("cSQLite_specific.cs -> ShrinkDb", ex, strSQL);
                return ex;
            }

            return null;
        }


        public override System.Exception BackupDbOnServer(string DbName, string BackupToFilename)
        {
            string strSQL = null;

            throw new System.NotImplementedException("BackupDbOnServer");

            try
            {
                // http://syntaxhelp.com/SQLServer/Backup/FULL
                BackupToFilename = FixPathForWindows(BackupToFilename);

                if (!BackupToFilename.EndsWith(".bak", System.StringComparison.OrdinalIgnoreCase))
                    BackupToFilename += ".bak";

                // Cannot do that - path is path on db server, not application server
                //if (System.IO.File.Exists(BackupToFilename))
                //    System.IO.File.Delete(BackupToFilename);

                // WITH FORMAT does overwrite existing files

                // http://msdn.microsoft.com/en-us/library/ms187510.aspx
                strSQL = string.Format(@"
BACKUP DATABASE {0} 
TO DISK = '{1}' 
    WITH FORMAT 
       ,MEDIANAME = 'Z_SQLServerBackups' 
       ,NAME = 'Full Backup of {2}' 
;
", EscapeDbName(DbName), StringEscapeString(BackupToFilename), StringEscapeString(DbName));

                ExecuteWithoutTransaction(strSQL);
            }
            catch (System.Exception ex)
            {
                Log("cSQLite_specific.cs -> BackupDbOnServer", ex, strSQL);
                return ex;
            }

            return null;
        } // End Function BackupDbOnServer


        protected static object GetPropValue(object src, string propName)
        {
            if (src == null)
                throw new System.ArgumentNullException("src");

            if (string.IsNullOrEmpty(propName))
                throw new System.ArgumentNullException("propName");

            return src.GetType().GetProperty(propName).GetValue(src, null);
        }


        public override string DatabaseCreateScript(string DbName, string NewDbName)
        {
            throw new System.NotImplementedException("cSQLite_specific.cs ==> DatabaseCreateScript");
        } // End Function DatabaseCreateScript


        public virtual string DatabaseCreateScript(string DbName)
        {
            return DatabaseCreateScript(DbName, DbName);
        }


        protected class DbRestoreInfo
        {
            public string NameOfDb = null;
            public string NameToRestore = null;
            public string physical_name = null;
        }


        public override void RestoreDatabase(string DbName, string strFileName)
        {
            throw new System.NotImplementedException("cSQLite_specific.cs ==> RestoreDatabase");
        } // End Sub RestoreDatabase


        public override void RemapDbUserLogins(string DbName)
        {
            throw new System.NotImplementedException("cSQLite_specific.cs ==> RemapDbUserLogins");
        } // End Sub RemapDbUserLogins


    } // End Class cSQLite


} // End Namespace DataBase.Tools
