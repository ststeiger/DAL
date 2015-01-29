
namespace DB.Abstraction
{

    partial class cOracle : cDAL
    {
        // http://www.mono-project.com/Oracle


        ////////////////////////////// Schema //////////////////////////////


        public override System.Data.DataTable GetDataBases()
        {
            return GetDataBases(dbOwner.all);
        }

        /*
         SELECT VARIABLE_NAME, VARIABLE_VALUE FROM INFORMATION_SCHEMA.GLOBAL_VARIABLES WHERE VARIABLE_NAME = 'VERSION'
         
         1) To view database

select * from v$database;

2) To view instance

select * from v$instance;

3) To view all users

select * from all_users;
         http://nixcraft.com/databases-servers/1971-oracle-show-databases-list.html
        */

        // http://www.coderanch.com/t/80812/Oracle-OAS/list-all-databases-Oracle
        public System.Data.DataTable GetDataBases(dbOwner ShowDBs)
        {
            string strSQL = @"
            select * from user_tablespaces;   
            ";
            /*
            if ((uint)(ShowDBs & dbOwner.user) != 0)
            {
                strSQL += @"
                WHERE owner_sid != 0x01 
                ";
            }

            strSQL += @"
            ORDER BY name ASC 
            ";
            */

            //string strOldInitialCatalog = this.m_ConnectionString.InitialCatalog;
            //this.m_ConnectionString.InitialCatalog = "master";

            System.Data.DataTable dt = GetDataTable(strSQL);

            //this.m_ConnectionString.InitialCatalog = strOldInitialCatalog;
            //strOldInitialCatalog = null;

            return dt;
        } // End Function GetDataBases


        protected bool DatabaseExists(string strDataBaseName)
        {
            throw new System.NotImplementedException();
        } // End Function TableHasColumn


        public override void CreateDB()
        {
            CreateDB("", "", "");
        } // End Sub CreateDB


        // http://web.firebirdsql.org/dotnetfirebird/create-a-new-database-from-an-sql-script.html
        public override void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
        {
            throw new System.NotImplementedException("cOracleSchema.CreateDb");

            try
            {
                try
                {
                    System.Console.WriteLine("Create DB");
                    // Create a new database
                    //System.Data.OracleClient.OracleConnection.CreateDatabase(this.m_ConnectionString.ConnectionString);
                }
                catch (System.Data.OracleClient.OracleException ex)
                {
                    if(ex.ErrorCode == 335544344)
                        Log("This database already exists.");
                    else
                        Log(ex.Message);
                }
            } // End Try
            catch (System.Exception ex)
            {
                if (Log("cOracleSchema.CreateDB(string strDBname, string strDataLocation, string strLogLocation)", ex, "CreateDB"))
                    throw;
            } // End Catch
            finally
            {
                System.Threading.Thread.Sleep(2000); // Wait for disk-write complete
            } // End Finally
        } // End Sub CreateDB


        // http://bytes.com/topic/oracle/answers/599929-query-get-list-tables
        // http://stackoverflow.com/questions/205736/oracle-get-list-of-all-tables
        public override System.Data.DataTable GetTables()
        {
            //string strSQL = @"select * from tab;";
            string strSQL = @"
            SELECT owner, table_name
            FROM dba_tables
            ";

            return GetDataTable(strSQL);
        } // End Function GetTables


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetViews()
        {
            string strSQL = @"SELECT 
                                rdb$relation_name AS TABLE_NAME 
                            FROM rdb$relations
                            WHERE (rdb$view_blr IS NOT NULL)  
                            AND 
                            (
                                rdb$system_flag IS NULL 
                                OR 
                                rdb$system_flag = 0
                            ); ";

            return GetDataTable(strSQL);
        } // End Function GetViews


        public override System.Data.DataTable GetProcedures()
        {
            string strSQL = @"
			SELECT 
				a.RDB$PROCEDURE_NAME AS PROCEDURE_NAME  
			FROM RDB$PROCEDURES a 
			";

            return GetDataTable(strSQL);
        } // End Function GetProcedures


        public override System.Data.DataTable GetFunctions()
        {
            string strSQL = @"
			SELECT 
				a.RDB$FUNCTION_NAME AS FUNCTION_NAME 
			FROM RDB$FUNCTIONS a
			";

            return GetDataTable(strSQL);
        } // End Function GetFunctions


        public override System.Data.DataTable GetRoutines()
        {
            string strSQL = @"
            SELECT 
				a.RDB$FUNCTION_NAME AS ROUTINE_NAME 
			FROM RDB$FUNCTIONS a
			
			UNION
			
			SELECT 
				b.RDB$PROCEDURE_NAME AS ROUTINE_NAME  
			FROM RDB$PROCEDURES b 
            ";

            return GetDataTable(strSQL);
        } // End Function GetRoutines


        public override System.Data.DataTable GetColumnNames()
        {
            string strSQL = @"SELECT 
                                 f.rdb$relation_name AS TABLE_NAME 
                                ,f.rdb$field_name AS COLUMN_NAME 
                            FROM rdb$relation_fields f
                            JOIN rdb$relations r 
                                ON f.rdb$relation_name = r.rdb$relation_name
                                AND (r.rdb$view_blr IS NULL) 
                                AND 
                                (
                                    r.rdb$system_flag IS NULL 
                                    OR 
                                    r.rdb$system_flag = 0
                                )
                            ORDER BY 1, f.rdb$field_position;";

            return GetDataTable(strSQL);
        } // End Function GetColumnNames


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetColumnNamesForTable(string strTableName)
        {
            if (!string.IsNullOrEmpty(strTableName))
                strTableName = strTableName.Trim();

            if (string.IsNullOrEmpty(strTableName))
                return null;

            strTableName = strTableName.ToUpper().Replace("'", "''");
            

            string strSQL = @"SELECT 
                                 f.rdb$relation_name AS TABLE_NAME 
                                ,f.rdb$field_name AS COLUMN_NAME 
                            FROM rdb$relation_fields f
                            JOIN rdb$relations r 
                                ON f.rdb$relation_name = r.rdb$relation_name
                                AND (r.rdb$view_blr IS NULL) 
                                AND 
                                (
                                    r.rdb$system_flag IS NULL 
                                    OR 
                                    r.rdb$system_flag = 0
                                )
                            WHERE f.rdb$relation_name = '" + strTableName + @"'
                            ORDER BY 1, f.rdb$field_position;";

            return GetDataTable(strSQL);
        } // End Function GetColumnNamesForTable


        public override bool TableExists(string strTableName)
        {
            if (!string.IsNullOrEmpty(strTableName))
                strTableName = strTableName.Trim();

            if(string.IsNullOrEmpty(strTableName))
                return false;

            strTableName = strTableName.ToUpper().Replace("'", "''");

            string strSQL = @"
            SELECT 
                COUNT(*)  
            FROM rdb$relations
            WHERE (rdb$view_blr IS NULL) 
            AND 
            (
                rdb$system_flag IS NULL 
                OR 
                rdb$system_flag = 0
            )
            AND (rdb$relation_name = '" + strTableName + @"')
            ;";

            return ExecuteScalar<bool>(strSQL);
        } // End Function TableExists


        public override bool IsTableEmpty(string strTableName)
        {
            if (!string.IsNullOrEmpty(strTableName))
                strTableName = strTableName.Trim();

            if (string.IsNullOrEmpty(strTableName))
                return true;

            strTableName = strTableName.ToUpper().Replace("'", "''");

            string strSQL = "SELECT COUNT(*) FROM " + strTableName + "";

            return !ExecuteScalar<bool>(strSQL);
        } // End Function IsTableEmpty


        public override bool TableHasColumn(string strTableName, string strColumnName)
        {
            if (!string.IsNullOrEmpty(strTableName))
                strTableName = strTableName.Trim();

            if (string.IsNullOrEmpty(strTableName))
                return false;

            strTableName = strTableName.ToUpper().Replace("'", "''");

            if (!string.IsNullOrEmpty(strColumnName))
                strColumnName = strColumnName.Trim();

            if (string.IsNullOrEmpty(strColumnName))
                return false;

            strColumnName = strColumnName.ToUpper().Replace("'", "''");

            string strSQL = @"
            SELECT COUNT(*) 
            FROM rdb$relation_fields f
            JOIN rdb$relations r 
                ON f.rdb$relation_name = r.rdb$relation_name
                AND (r.rdb$view_blr IS NULL) 
                AND 
                (
                    r.rdb$system_flag IS NULL 
                    OR 
                    r.rdb$system_flag = 0
                )
            WHERE f.rdb$relation_name = '" + strTableName + @"'
                AND (f.rdb$field_name = '" + strColumnName + @"');
            ";

            return ExecuteScalar<bool>(strSQL);
        } // End Function TableHasColumn

        ////////////////////////////// End Schema //////////////////////////////


        public override DataBaseEngine_t DBtype   // overriding property
        {
            get
            {
                return this.m_dbtDBtype;
            }
        } // End Property DBtype


        public override string DBversion   // overriding property
        {
            get
            {
                // System.Data.DataTable dt = DAL.GetDataTable("SELECT * FROM v$version");
                // http://en.wikipedia.org/wiki/Oracle_Database#Version_numbering
                // http://dbametrix.wordpress.com/2009/12/22/oracle-version-number-detail-explaination/
                // http://dbataj.blogspot.com/2008/07/how-to-find-oracle-version.html

                return "";
            }
        } // End Property DBversion


        public static void Test()
        {
            cDAL DAL = new cOracle();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cOracle


} // End Namespace DataBase.Tools
