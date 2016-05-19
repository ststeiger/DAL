
namespace DB.Abstraction
{

    partial class cMS_SQL : cDAL
    {

        protected System.Data.SqlClient.SqlConnectionStringBuilder m_ConnectionString;


        public override bool IsMsSql
        {
            get { return true; }
        }


        // http://en.csharp-online.net/CSharp_FAQ:_How_can_one_constructor_call_another
        public cMS_SQL()
            : this("")
        {
            // Crap !
        } // End Constructor 0


        public cMS_SQL(string strConnectionString)
        {
            this.m_dbtDBtype = DataBaseEngine_t.MS_SQL;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 8.0;
            this.m_ConnectionString = new System.Data.SqlClient.SqlConnectionStringBuilder(strConnectionString);
        } // End Constructor 1


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder()
        {
            return new MS_SQLUniversalConnectionStringBuilder();
        }


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder(string connectionString)
        {
            return new MS_SQLUniversalConnectionStringBuilder(connectionString);
        }


        public override string DecryptPassword(string strEncryptedPassword)
        {
            return DB.Abstraction.Tools.Cryptography.DES.DeCrypt(strEncryptedPassword);
        }


        public override string GetConnectionString(string strDb)
        {
            if (string.IsNullOrEmpty(strDb))
                return this.m_ConnectionString.ConnectionString;


            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder(this.m_ConnectionString.ConnectionString);
            csb.InitialCatalog = strDb;
            string strRetVal = csb.ConnectionString;
            // csb.Clear ();
            csb = null;

            return strRetVal;
        }


        public System.Data.Common.DbProviderFactory GetFactory()
        {
            System.Data.Common.DbProviderFactory providerFactory = null;
            // providerFactory = System.Data.Common.DbProviderFactories.GetFactory("System.Data.SqlClient");
            providerFactory = this.GetFactory(typeof(System.Data.SqlClient.SqlClientFactory));

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
            string hex = "0x" + ts.Days.ToString("X8") 
                + System.Convert.ToInt32(
                    System.Math.Floor(ms.TotalMilliseconds / 3.3333333333)).ToString("X8"
                );

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


        public override System.Collections.Generic.List<string> GetComputedColumnNames(string tableSchema, string tableName)
        {
            System.Collections.Generic.List<string> lsComputedColumns = null;

            using (System.Data.IDbCommand cmd = CreateCommand(@"
SELECT syscc.name FROM sys.objects AS syso 
INNER JOIN sys.schemas AS sschema ON sschema.schema_id = syso.schema_id 
INNER JOIN sys.computed_columns  AS syscc ON syscc.object_id =  syso.object_id 
WHERE (1=1) 
AND syso.name = @__destTable 
AND sschema.name = @__destSchema 
AND syso.type = 'U '
AND syscc.is_computed = 1 
-- AND syscc.is_persisted = 1 
-- syscc.definition 
"))
            {
                this.AddParameter(cmd, "__destSchema", tableSchema);
                this.AddParameter(cmd, "__destTable", tableName);
                lsComputedColumns = this.GetList<string>(cmd);
            }

            return lsComputedColumns;
        }


        // BulkCopy("dbo.T_Benutzer", dt)
        public override bool BulkCopy(string tableSchema, string tableName, System.Data.DataTable dt, bool bWithDelete)
        {
            try
            {
                string sanitizedTableName = this.QuoteObjectWhereNecessary(tableName);

                // Ensure table is empty - and throw on foreign-key
                if (bWithDelete)
                {
                    this.Execute("DELETE FROM " + sanitizedTableName);
                }



                System.Collections.Generic.List<string> lsComputedColumns = GetComputedColumnNames(tableSchema, tableName);


                // http://msdn.microsoft.com/en-us/library/system.data.sqlclient.sqlbulkcopyoptions.aspx
                System.Data.SqlClient.SqlBulkCopyOptions bcoOptions = //System.Data.SqlClient.SqlBulkCopyOptions.CheckConstraints |
                                                                      System.Data.SqlClient.SqlBulkCopyOptions.KeepNulls |
                                                                      System.Data.SqlClient.SqlBulkCopyOptions.KeepIdentity;

                // http://stackoverflow.com/questions/6651809/sqlbulkcopy-insert-with-identity-column
                // http://msdn.microsoft.com/en-us/library/ms186335.aspx
                System.Data.SqlClient.SqlBulkCopy BulkCopyInstance = new System.Data.SqlClient.SqlBulkCopy(this.m_ConnectionString.ConnectionString, bcoOptions);
                foreach (System.Data.DataColumn dc in dt.Columns)
                {
                    // The column "foo" cannot be modified because it is either a computed column or...
                    if (MyExtensionMethods.Contains(lsComputedColumns, dc.ColumnName, System.StringComparer.InvariantCultureIgnoreCase))
                        continue;

                    BulkCopyInstance.ColumnMappings.Add(dc.ColumnName, "[" + dc.ColumnName.Replace("]", "]]") + "]");
                }
                BulkCopyInstance.DestinationTableName = sanitizedTableName;

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
                if (Log("cMS_SQL_specific.BulkCopy", ex, "BulkCopy: Copy dt to " + tableName))
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


        public override System.Data.IDbCommand CreateLimitedCommand(string strSQL, int iLimit)
        {
            strSQL = string.Format(strSQL, "TOP " + iLimit.ToString(), "");
            return CreateCommand(strSQL);
        }


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

            try
            {
                // http://technet.microsoft.com/en-us/library/ms190488.aspx
                // ShrinkMethod:
                // Default:	      Data in pages located at the end of a file is moved to pages earlier in the file. Files are truncated to reflect allocated space.
                // EmptyFile:	  Migrates all of the data from the referenced file to other files in the same filegroup. (DataFileand LogFile objects only).
                // NoTruncate:	  Data in pages located at the end of a file is moved to pages earlier in the file.
                // TruncateOnly:  Data distribution is not affected. Files are truncated to reflect allocated space, recovering free space at the end of any file.
                strSQL = string.Format(@"DBCC SHRINKDATABASE ({0}, {1});", EscapeDbName(DbName), Percent.ToString());

                ExecuteWithoutTransaction(strSQL);
            }
            catch (System.Exception ex)
            {
                Log("cMS_SQL_specific.cs -> ShrinkDb", ex, strSQL);
                return ex;
            }

            return null;
        }


        public override System.Exception BackupDbOnServer(string DbName, string BackupToFilename)
        {
            string strSQL = null;

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

                ExecuteWithoutTransaction(strSQL, 600);
            }
            catch (System.Exception ex)
            {
                Log("cMS_SQL_specific.cs -> BackupDbOnServer", ex, strSQL);
                return ex;
            }

            return null;
        } // End Function BackupDbOnServer


        public class csysdb
        {
            public csysdb() { }
            public virtual string name { get; set; }
            public virtual int database_id { get; set; }
            public virtual System.Nullable<int> source_database_id { get; set; }
            public virtual byte[] owner_sid { get; set; }
            public virtual System.DateTime create_date { get; set; }
            public virtual int compatibility_level { get; set; }
            public virtual string collation_name { get; set; }
            public virtual int user_access { get; set; }
            public virtual string user_access_desc { get; set; }
            public virtual System.Nullable<bool> is_read_only { get; set; }
            public virtual bool is_auto_close_on { get; set; }
            public virtual System.Nullable<bool> is_auto_shrink_on { get; set; }
            public virtual int state { get; set; }
            public virtual string state_desc { get; set; }
            public virtual System.Nullable<bool> is_in_standby { get; set; }
            public virtual System.Nullable<bool> is_cleanly_shutdown { get; set; }
            public virtual System.Nullable<bool> is_supplemental_logging_enabled { get; set; }
            public virtual System.Nullable<byte> snapshot_isolation_state { get; set; }
            public virtual string snapshot_isolation_state_desc { get; set; }
            public virtual System.Nullable<bool> is_read_committed_snapshot_on { get; set; }
            public virtual int recovery_model { get; set; }
            public virtual string recovery_model_desc { get; set; }
            public virtual System.Nullable<int> page_verify_option { get; set; }
            public virtual string page_verify_option_desc { get; set; }
            public virtual System.Nullable<bool> is_auto_create_stats_on { get; set; }
            public virtual System.Nullable<bool> is_auto_update_stats_on { get; set; }
            public virtual System.Nullable<bool> is_auto_update_stats_async_on { get; set; }
            public virtual System.Nullable<bool> is_ansi_null_default_on { get; set; }
            public virtual System.Nullable<bool> is_ansi_nulls_on { get; set; }
            public virtual System.Nullable<bool> is_ansi_padding_on { get; set; }
            public virtual System.Nullable<bool> is_ansi_warnings_on { get; set; }
            public virtual System.Nullable<bool> is_arithabort_on { get; set; }
            public virtual System.Nullable<bool> is_concat_null_yields_null_on { get; set; }
            public virtual System.Nullable<bool> is_numeric_roundabort_on { get; set; }
            public virtual System.Nullable<bool> is_quoted_identifier_on { get; set; }
            public virtual System.Nullable<bool> is_recursive_triggers_on { get; set; }
            public virtual System.Nullable<bool> is_cursor_close_on_commit_on { get; set; }
            public virtual System.Nullable<bool> is_local_cursor_default { get; set; }
            public virtual System.Nullable<bool> is_fulltext_enabled { get; set; }
            public virtual System.Nullable<bool> is_trustworthy_on { get; set; }
            public virtual System.Nullable<bool> is_db_chaining_on { get; set; }
            public virtual System.Nullable<bool> is_parameterization_forced { get; set; }
            public virtual bool is_master_key_encrypted_by_server { get; set; }
            public virtual bool is_published { get; set; }
            public virtual bool is_subscribed { get; set; }
            public virtual bool is_merge_published { get; set; }
            public virtual bool is_distributor { get; set; }
            public virtual bool is_sync_with_backup { get; set; }
            public virtual System.Guid service_broker_guid { get; set; }
            public virtual bool is_broker_enabled { get; set; }
            public virtual System.Nullable<int> log_reuse_wait { get; set; }
            public virtual string log_reuse_wait_desc { get; set; }
            public virtual bool is_date_correlation_on { get; set; }
            public virtual bool is_cdc_enabled { get; set; }
            public virtual System.Nullable<bool> is_encrypted { get; set; }
            public virtual System.Nullable<bool> is_honor_broker_priority_on { get; set; }
        }



        protected class cFileGroup
        {
            public int data_space_id = -1;
            public string filegroup = null;
        } // End Class cFileGroup


        protected class cMasterFile
        {
            public int file_id = -1;
            public string filegroup = null;
            public string type_desc = null;
            public int data_space_id = 0;
            public string name = null;
            public string physical_name = null;
            public string path = null;
            public string filename = null;

            public string FileExtension = null;

            public long Size = 0;

            public string pSize
            {
                get
                {

                    return FormatDbSize(Size);
                }
            } // End Property pSize



            public string FormatDbSize(long lngNumberOfBytes)
            {
                if (lngNumberOfBytes <= 0)
                    return "UNLIMITED";

                if (lngNumberOfBytes > 1024 * 1024)
                {
                    return System.Math.Ceiling((double)lngNumberOfBytes / (double)(1024 * 1024)).ToString() + "GB";
                }

                if (lngNumberOfBytes > 1024)
                {
                    return System.Math.Ceiling((double)lngNumberOfBytes / (double)(1024)).ToString() + "MB";
                }
                return lngNumberOfBytes.ToString() + "KB";
            }



            public long MaxSize = 0;

            public string pMaxSize
            {
                get
                {
                    return FormatDbSize(MaxSize);
                }
            } // End Property pMaxSize

            public long Growth = 0;

            public string pGrowth
            {
                get
                {
                    if (is_percent_growth)
                        return Growth.ToString() + "%";

                    //return Growth.ToString() + "KB";
                    return FormatDbSize(Growth);
                }
            } // End Property pGrowth

            public bool is_percent_growth = false;
        } // End Class cMasterFile




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
            string strNewLine = "\r\n";

            //using (System.Data.DataTable dt = Db.DAL.GetDataTable("SELECT * FROM master.sys.databases"))
            //{
            //    string SQL = SqlTableCreator.GetCreateFromDataTableSQL("tablename", dt);
            //}

            csysdb sysdb = null;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();


            using (System.Data.IDbCommand cmd = CreateCommand("SELECT * FROM sys.databases WHERE name LIKE @__in_dbname "))
            {
                AddParameter(cmd, "__in_dbname", DbName);
                sysdb = GetClass<csysdb>(cmd);
            }


            cMasterFile LogFileInfo = GetLogFileData(sysdb.database_id);
            System.Collections.Generic.IList<cFileGroup> lsFileGroups = GetFileGroups(sysdb.database_id);

            System.Collections.Generic.IList<cMasterFile> lsMasterFiles;


            sb.AppendFormat(@"
CREATE DATABASE {0} ON PRIMARY 
( 
", EscapeDbName(NewDbName));


            foreach (cFileGroup FileGroup in lsFileGroups)
            {
                //FileGroup.filegroup

                string strMasterFileSQL = @"

USE " + EscapeDbName(DbName) + @";

SELECT 
	 file_id 
	 ,FILEGROUP_NAME(data_space_id) as filegroup 
	--,file_guid
	--,type
	,type_desc
	,data_space_id
	,name
	,physical_name
	
	
	,LEFT(physical_name, LEN(physical_name) + 1 - CHARINDEX('\', REVERSE(physical_name))) AS path  
	,RIGHT(physical_name, CHARINDEX('\', REVERSE(physical_name)) -1) AS filename  
	,RIGHT(physical_name, CHARINDEX('.', REVERSE(physical_name))) AS FileExtension 

	,size * CONVERT(float,8) AS Size 
	,CASE WHEN max_size=-1 THEN -1 ELSE max_size * CONVERT(float,8) END AS MaxSize 
	,CAST(CASE is_percent_growth WHEN 1 THEN growth ELSE growth*8 END AS float) AS Growth 
FROM sys.master_files AS mf 
WHERE database_id = " + sysdb.database_id.ToString() + @" 
AND type_desc != 'LOG'
--AND data_space_id = " + FileGroup.data_space_id.ToString() + @" 

ORDER BY file_id, data_space_id 

";


                using (System.Data.IDbCommand cmd = CreateCommand(strMasterFileSQL))
                {
                    AddParameter(cmd, "__in_db_id", DbName);
                    lsMasterFiles = GetList<cMasterFile>(cmd);
                }


                int iCount = 0;
                int LastFileGroupId = 0;
                foreach (cMasterFile MasterFile in lsMasterFiles)
                {
                    string strNewFileName = MasterFile.physical_name;
                    if (!System.StringComparer.OrdinalIgnoreCase.Equals(DbName, NewDbName))
                    {
                        strNewFileName = System.IO.Path.Combine(MasterFile.path, NewDbName + "_" + MasterFile.file_id.ToString() + MasterFile.FileExtension);
                        strNewFileName = FixPathForWindows(strNewFileName);
                    }

                    if (System.StringComparer.OrdinalIgnoreCase.Equals(MasterFile.filegroup, "PRIMARY"))
                    {
                        sb.AppendFormat(@"
     NAME = N'" + StringEscapeString(MasterFile.name) + @"' 
    ,FILENAME = N'" + StringEscapeString(strNewFileName) + @"' 
    ,SIZE = {0} 
    ,MAXSIZE = {1} 
    ,FILEGROWTH = {2} 
) 
", MasterFile.pSize, MasterFile.pMaxSize, MasterFile.pGrowth);

                        LastFileGroupId = MasterFile.data_space_id;
                    }
                    else
                    {

                        if (LastFileGroupId != MasterFile.data_space_id)
                        {

                            sb.AppendFormat(@"
,FILEGROUP [" + MasterFile.filegroup + @"] " + (iCount == 0 ? "DEFAULT" : "") + @" 
    ( 
         NAME = N'" + StringEscapeString(MasterFile.name) + @"' 
        ,FILENAME = N'" + StringEscapeString(strNewFileName) + @"' 
        ,SIZE = {0} 
        ,MAXSIZE = {1} 
        ,FILEGROWTH = {2} 
    ) ", MasterFile.pSize, MasterFile.pMaxSize, MasterFile.pGrowth);


                        }
                        else
                        {
                            sb.AppendFormat(@"
    ,
    ( 
         NAME = N'" + StringEscapeString(MasterFile.name) + @"' 
        ,FILENAME = N'" + StringEscapeString(strNewFileName) + @"' 
        ,SIZE = {0} 
        ,MAXSIZE = {1} 
        ,FILEGROWTH = {2} 
    ) ", MasterFile.pSize, MasterFile.pMaxSize, MasterFile.pGrowth);

                        }

                        LastFileGroupId = MasterFile.data_space_id;
                        iCount++;

                    }


                } // Next cMasterFile


                string strLogFile = LogFileInfo.physical_name;
                if (!System.StringComparer.OrdinalIgnoreCase.Equals(DbName, NewDbName))
                {
                    strLogFile = System.IO.Path.Combine(LogFileInfo.path, NewDbName + "_LOG" + LogFileInfo.FileExtension);
                    strLogFile = FixPathForWindows(strLogFile);
                }

                // Logfile
                sb.AppendFormat(@"
LOG ON 
( 
     NAME = N'" + StringEscapeString(LogFileInfo.name) + @"' 
    ,FILENAME = N'" + StringEscapeString(strLogFile) + @"' 
    ,SIZE = {0} 
    ,MAXSIZE = {1} 
    ,FILEGROWTH = {2} 
) 
GO 
", LogFileInfo.pSize, LogFileInfo.pMaxSize, LogFileInfo.pGrowth);


            } // Next FileGroup





            if (sysdb.is_fulltext_enabled.HasValue && sysdb.is_fulltext_enabled.Value)
            {
                sb.AppendFormat(@"
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
BEGIN 
    EXECUTE {0}.[dbo].[sp_fulltext_database] @action = 'enable' 
END 
GO
"
                    , EscapeDbName(NewDbName)
                );
                sb.Append(strNewLine);
            } // End if (sysdb.is_fulltext_enabled.HasValue && sysdb.is_fulltext_enabled.Value)




            System.Collections.Generic.Dictionary<string, string> dict = new System.Collections.Generic.Dictionary<string, string>();

            // http://msdn.microsoft.com/en-us/library/bb522682.aspx
            dict.Add("ALLOW_SNAPSHOT_ISOLATION", "snapshot_isolation_state");
            dict.Add("ANSI_NULL_DEFAULT", "is_ansi_null_default_on");
            dict.Add("ANSI_NULLS", "is_ansi_nulls_on");
            dict.Add("ANSI_PADDING", "is_ansi_padding_on");
            dict.Add("ANSI_WARNINGS", "is_ansi_warnings_on");
            dict.Add("ARITHABORT", "is_arithabort_on");
            dict.Add("AUTO_CLOSE", "is_auto_close_on");
            dict.Add("AUTO_CREATE_STATISTICS", "is_auto_create_stats_on");
            dict.Add("AUTO_SHRINK", "is_auto_shrink_on");
            dict.Add("AUTO_UPDATE_STATISTICS", "is_auto_update_stats_on");
            dict.Add("AUTO_UPDATE_STATISTICS_ASYNC", "is_auto_update_stats_async_on");
            dict.Add("CONCAT_NULL_YIELDS_NULL", "is_concat_null_yields_null_on");
            dict.Add("CURSOR_CLOSE_ON_COMMIT", "is_cursor_close_on_commit_on");
            dict.Add("DATE_CORRELATION_OPTIMIZATION", "is_date_correlation_on");
            dict.Add("HONOR_BROKER_PRIORITY", "is_honor_broker_priority_on");
            dict.Add("NUMERIC_ROUNDABORT", "is_numeric_roundabort_on");
            dict.Add("QUOTED_IDENTIFIER", "is_quoted_identifier_on");
            dict.Add("READ_COMMITTED_SNAPSHOT", "is_read_committed_snapshot_on");
            dict.Add("RECURSIVE_TRIGGERS", "is_recursive_triggers_on");
            dict.Add("TRUSTWORTHY", "is_trustworthy_on");
            //dict.Add("ENCRYPTION", "is_encrypted"); // Der Status der Datenbankverschlüsselung kann nicht geändert werden, weil kein Verschlüsselungsschlüssel für die Datenbank festgelegt ist.


            string strKey = null;
            object value = null;
            string strValue = null;


            foreach (string strKeyParam in dict.Keys)
            {
                strKey = strKeyParam;
                value = GetPropValue(sysdb, dict[strKey]);

                strValue = System.Convert.ToBoolean(value) ? "ON" : "OFF";

                sb.AppendFormat(@"ALTER DATABASE {0} SET {1} {2} ", EscapeDbName(NewDbName), strKey, strValue);
                sb.Append(strNewLine);
                sb.AppendLine("GO");
                sb.Append(strNewLine);
            } // Next strKeyParam


            System.Collections.Generic.List<string> lsChainingNotAllowed = new System.Collections.Generic.List<string>(new string[] { "master", "model", "tempdb" });

            if (!lsChainingNotAllowed.Contains(DbName.ToLower()))
            {
                strKey = "DB_CHAINING";
                value = GetPropValue(sysdb, "is_db_chaining_on");
                strValue = System.Convert.ToBoolean(value) ? "ON" : "OFF";
                sb.AppendFormat("ALTER DATABASE	{0} SET {1} {2} {3}", EscapeDbName(NewDbName), strKey, strValue, strNewLine);
                sb.AppendLine("GO");
                sb.Append(strNewLine);
            } // End if (!lsChainingNotAllowed.Contains(DbName.ToLower()))



            strKey = "CURSOR_DEFAULT";
            value = GetPropValue(sysdb, "is_local_cursor_default");
            strValue = System.Convert.ToBoolean(value) ? "LOCAL" : "GLOBAL";
            sb.AppendFormat("ALTER DATABASE	{0} SET {1} {2} {3}", EscapeDbName(NewDbName), strKey, strValue, strNewLine);
            sb.AppendLine("GO");
            sb.Append(strNewLine);


            strKey = "";
            value = GetPropValue(sysdb, "is_read_only");
            strValue = System.Convert.ToBoolean(value) ? "READ_ONLY" : "READ_WRITE";
            sb.AppendFormat("ALTER DATABASE	{0} SET {1} {2} {3}", EscapeDbName(NewDbName), strKey, strValue, strNewLine);
            sb.AppendLine("GO");
            sb.Append(strNewLine);


            strKey = "PARAMETERIZATION";
            value = GetPropValue(sysdb, "is_parameterization_forced");
            strValue = System.Convert.ToBoolean(value) ? "FORCED" : "SIMPLE";
            sb.AppendFormat("ALTER DATABASE	{0} SET {1} {2} {3}", EscapeDbName(NewDbName), strKey, strValue, strNewLine);
            sb.AppendLine("GO");
            sb.Append(strNewLine);


            strKey = "";
            value = GetPropValue(sysdb, "is_broker_enabled");
            strValue = System.Convert.ToBoolean(value) ? "ENABLE_BROKER" : "DISABLE_BROKER";
            sb.AppendFormat("ALTER DATABASE {0} SET {1} {2} {3}", EscapeDbName(NewDbName), strKey, strValue, strNewLine);
            sb.AppendLine("GO");
            sb.Append(strNewLine);


            strKey = "PAGE_VERIFY ";
            value = GetPropValue(sysdb, "page_verify_option_desc");
            strValue = ((string)value);
            sb.AppendFormat("ALTER DATABASE	{0} SET {1} {2} {3}", EscapeDbName(NewDbName), strKey, strValue, strNewLine);
            sb.AppendLine("GO");
            sb.Append(strNewLine);


            strKey = "";
            value = GetPropValue(sysdb, "user_access_desc");
            strValue = ((string)value);
            sb.AppendFormat("ALTER DATABASE	{0} SET {1} {2} {3}", EscapeDbName(NewDbName), strKey, strValue, strNewLine);
            sb.AppendLine("GO");
            sb.Append(strNewLine);

            string strReturnvalue = sb.ToString();
            sb = null;

            return strReturnvalue;
        } // End Function DatabaseCreateScript


        public virtual string DatabaseCreateScript(string DbName)
        {
            return DatabaseCreateScript(DbName, DbName);
        }


        protected System.Collections.Generic.IList<cFileGroup> GetFileGroups(int db_id)
        {
            string strFileGroupsSQL = @"
SELECT DISTINCT 
	 data_space_id 
	,FILEGROUP_NAME(data_space_id) AS filegroup
FROM sys.master_files AS mf 
WHERE database_id = " + db_id + @" 
AND FILEGROUP_NAME(data_space_id) IS NOT NULL 

ORDER BY data_space_id 
";

            return GetList<cFileGroup>(strFileGroupsSQL); ;
        }


        protected cMasterFile GetLogFileData(int db_id)
        {

            string strLogFileSQL = @"
SELECT 
	 file_id 
	 ,FILEGROUP_NAME(data_space_id) 
	--,file_guid
	--,type
	,type_desc
	,data_space_id
	,name
	,physical_name
	
	,LEFT(physical_name, LEN(physical_name) + 1 - CHARINDEX('\', REVERSE(physical_name))) AS path 
	,RIGHT(physical_name, CHARINDEX('\', REVERSE(physical_name)) - 1) AS filename 
    ,RIGHT(physical_name, CHARINDEX('.', REVERSE(physical_name))) AS FileExtension 	

	,size * CONVERT(float, 8) AS Size 
	,CASE WHEN max_size = -1 THEN -1 ELSE max_size * CONVERT(float, 8) END AS MaxSize 
	,CAST(CASE is_percent_growth WHEN 1 THEN growth ELSE growth * 8 END AS float) AS Growth 
    ,is_percent_growth 
FROM sys.master_files AS mf 
WHERE database_id = " + db_id.ToString() + @" 

AND type_desc = 'LOG' 

ORDER BY file_id, data_space_id 
";

            return GetClass<cMasterFile>(strLogFileSQL);
        }


        protected class DbRestoreInfo
        {
            public string NameOfDb = null;
            public string NameToRestore = null;
            public string physical_name = null;
        }


        public override void RestoreDatabase(string DbName, string strFileName)
        {
            RestoreDatabase(DbName, strFileName, 30);
        }



        public override void RestoreDatabase(string DbName, string strFileName, int iTimeout)
        {
            string strDataFileQuery = @"


SELECT 
	  db.name AS NameOfDb 
	 ,mf.name AS NameToRestore 
	 ,mf.physical_name
FROM sys.master_files as mf

LEFT JOIN sys.databases AS db
	ON db.database_id = mf.database_id
	
WHERE db.name = '" + StringEscapeString(DbName) + @"' 
AND mf.type_desc = 'LOG'
";


            string strLogFileQuery = @"
SELECT 
	  db.name AS NameOfDb 
	 ,mf.name AS NameToRestore 
	 ,mf.physical_name
FROM sys.master_files as mf

LEFT JOIN sys.databases AS db
	ON db.database_id = mf.database_id
	
WHERE db.name = '" + StringEscapeString(DbName) + @"' 
AND mf.file_id = 1 
";

            DbRestoreInfo driData = GetClass<DbRestoreInfo>(strDataFileQuery);
            DbRestoreInfo driLogfile = GetClass<DbRestoreInfo>(strLogFileQuery);


            string @strSQL = string.Format(@"
RESTORE DATABASE {0}
FROM DISK = '" + StringEscapeString(strFileName) + @"' 
WITH REPLACE, MOVE '" + StringEscapeString(driData.NameToRestore) + @"' TO '" + StringEscapeString(driData.physical_name) + @"',
MOVE '" + StringEscapeString(driLogfile.NameToRestore) + @"' TO '" + StringEscapeString(driLogfile.physical_name) + @"'
", EscapeDbName(DbName));


            ExecuteWithoutTransaction(string.Format(@"ALTER DATABASE {0} SET SINGLE_USER WITH ROLLBACK IMMEDIATE ", EscapeDbName(DbName)));
            ExecuteWithoutTransaction(strSQL, iTimeout);
            ExecuteWithoutTransaction(string.Format("ALTER DATABASE {0} SET MULTI_USER", EscapeDbName(DbName)));
        } // End Sub RestoreDatabase


        public override void RemapDbUserLogins(string DbName)
        {
            string strSQL = @"
USE " + EscapeDbName(DbName) + @";

DECLARE @__DatabasePrincipal sysname --nvarchar(128) 
DECLARE @__ServerPrincipal sysname --nvarchar(128) 

DECLARE @__CurPrincipals CURSOR

SET @__CurPrincipals = CURSOR FOR
(
	-- sysname: nvarchar(128)
	-- Get Login for user 
	SELECT 
		 dp.name AS user_name 
		--,sp.name AS login_name 
		
		,
		CASE 
			WHEN sp.name IS NULL 
				THEN
					CASE 
						WHEN 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = dp.name COLLATE Latin1_General_CI_AS)
							THEN dp.name 
						WHEN dp.name LIKE '%[_]DE' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 3) COLLATE Latin1_General_CI_AS)
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 3) 
						WHEN dp.name LIKE '%DE' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 2) COLLATE Latin1_General_CI_AS)
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 2) 
						
						WHEN dp.name LIKE '%[_]EN' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 3) COLLATE Latin1_General_CI_AS)
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 3) 
						WHEN dp.name LIKE '%EN' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 2) COLLATE Latin1_General_CI_AS) 
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 2) 
							
						WHEN dp.name LIKE '%[_]FR' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 3) COLLATE Latin1_General_CI_AS)
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 3) 
						WHEN dp.name LIKE '%FR' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 2) COLLATE Latin1_General_CI_AS) 
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 2) 
							
						WHEN dp.name LIKE '%[_]IT' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 3) COLLATE Latin1_General_CI_AS)
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 3) 
						WHEN dp.name LIKE '%IT' AND 1 = (SELECT COUNT(*) FROM sys.server_principals  AS spSubSel WHERE spSubSel.name = SUBSTRING(dp.name, 1, LEN(dp.name) - 2) COLLATE Latin1_General_CI_AS) 
							THEN SUBSTRING(dp.name, 1, LEN(dp.name) - 2) 
							
						--ELSE 'foo' + SUBSTRING(dp.name, 1, LEN(dp.name) - 3)
						--ELSE N'SomeWebServices' 
						ELSE NULL --COLLATE Latin1_General_CI_AS
					END 
			ELSE sp.name COLLATE Latin1_General_CI_AS
		END AS CorrespondingUser 
		
	FROM sys.database_principals AS dp 

	LEFT JOIN sys.server_principals  AS sp 
		ON sp.sid = dp.sid 
		
	WHERE dp.type_desc = 'SQL_USER' 
	AND dp.sid IS NOT NULL 
	AND dp.sid != 0 

	AND sp.name IS NULL -- WHERE sids don't match 
	
)
OPEN @__CurPrincipals
	FETCH NEXT FROM @__CurPrincipals INTO @__DatabasePrincipal, @__ServerPrincipal 
	WHILE @@FETCH_STATUS = 0
	BEGIN
		PRINT @__DatabasePrincipal + N' = ' + @__ServerPrincipal 
		
		--EXEC sp_helptext 'sp_change_users_login' 
		
		DECLARE @Action varchar(10) -- REPORT / UPDATE_ONE / AUTO_FIX 
		DECLARE @UserNamePattern sysname  = NULL 
		DECLARE @LoginName sysname  = NULL 
		DECLARE @Password sysname  = NULL 
		
		
		SET @Action = 'Update_One' 
		SET @UserNamePattern = @__DatabasePrincipal -- N'SomeWebServicesDE' 
		SET @LoginName = @__ServerPrincipal -- N'SomeWebServices' 
		
		IF @LoginName IS NOT NULL
		BEGIN
			PRINT 'EXECUTE sp_change_users_login @Action = ''' + @Action + N''', @UserNamePattern = ''' + @UserNamePattern + N''', @LoginName = ''' + @LoginName + N''', @Password = ' + ISNULL(N'''' + @Password + N'''', N'NULL') 
			EXECUTE sp_change_users_login @Action, @UserNamePattern, @LoginName, @Password 
		END
		ELSE 
		BEGIN 
			PRINT N'Ignored user ' + @UserNamePattern
		END 
	FETCH NEXT FROM @__CurPrincipals INTO @__DatabasePrincipal, @__ServerPrincipal 
	END
CLOSE @__CurPrincipals
DEALLOCATE @__CurPrincipals
";

            ExecuteWithoutTransaction(strSQL);
        } // End Sub RemapDbUserLogins


    } // End Class cMS_SQL


} // End Namespace DataBase.Tools
