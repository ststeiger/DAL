
namespace DB.Abstraction
{

    partial class cFireBird : cDAL
    {

        ////////////////////////////// Schema //////////////////////////////



		public override System.Data.DataTable GetTypes()
		{
			// http://stackoverflow.com/questions/3660787/how-to-list-custom-types-using-postgres-information-schema
			string strSQL = @"
SELECT 
     typ.RDB$FIELD_NAME
	,typ.RDB$TYPE
	,typ.RDB$TYPE_NAME
	,typ.RDB$DESCRIPTION
	,typ.RDB$SYSTEM_FLAG
FROM RDB$TYPES AS typ 

WHERE (1=1) 
AND typ.RDB$FIELD_NAME = 'RDB$FIELD_TYPE'
;";

			return GetDataTable(strSQL);
		}


        public override void CreateDB()
        {
            CreateDB("", "", "");
        }


        // http://web.firebirdsql.org/dotnetfirebird/create-a-new-database-from-an-sql-script.html
        public override void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
        {
            try
            {
                System.Threading.Monitor.Enter(this.m_SqlConnection);

                try
                {
                    // Create a new database
                    FirebirdSql.Data.FirebirdClient.FbConnection.CreateDatabase(this.m_ConnectionString.ConnectionString);
                }
                catch (FirebirdSql.Data.FirebirdClient.FbException ex)
                {
                    if(ex.ErrorCode == 335544344)
                        Log("This database already exists.");
                    else
                        Log(ex.Message);
                }
            } // End Try
            catch (System.Exception ex)
            {
                if (Log("cFirebird_schema.cs ==> CreateDB(string strDBname, string strDataLocation, string strLogLocation)", ex, this.m_ConnectionString.ConnectionString))
                    throw;
            } // End Catch
            finally
            {
                System.Threading.Monitor.Exit(this.m_SqlConnection);
                //System.Threading.Thread.Sleep(2000); // Wait for disk-write complete
            } // End Finally
        }



		public override string GetDataBasesQueryText(dbOwner ShowDBs)
		{
			string filename = System.IO.Path.GetFileName(this.m_ConnectionString.Database);
			filename = filename.Replace("'","''");

			string strSQL = string.Format(@"
SELECT 
	 '{0}' AS name 
	,'datcollate' AS collation_name 
	,NULL AS owner_sid 
	,NULL AS create_date 
	,0 AS compatibility_level 
FROM RDB$DATABASE;
", filename);

			return strSQL;
		}

		public System.Data.DataTable GetDataBases(dbOwner ShowDBs)
		{
			System.Data.DataTable dt = GetDataTable(GetDataBasesQueryText());

			return dt;
		} 


		public override System.Data.DataTable GetDataBases()
		{
			return GetDataBases(dbOwner.all);
		}


        // http://www.firebirdfaq.org/faq174/
		public override System.Data.DataTable GetTables(string strInitialCatalog)
		{
			//string strCatalog = this.m_DatabaseConfiguration.strInitialCatalog;

			//if (!string.IsNullOrEmpty(strCatalog))
			//    strCatalog = strCatalog.Trim();

			//if (string.IsNullOrEmpty(strCatalog))
			//    return null;

			string strSQL = @"
SELECT 
	 RDB$VIEW_BLR
	,RDB$VIEW_SOURCE
	,RDB$DESCRIPTION
	,RDB$RELATION_ID
	,RDB$SYSTEM_FLAG
	,RDB$DBKEY_LENGTH
	,RDB$FORMAT
	,RDB$FIELD_ID
	,RDB$RELATION_NAME AS table_name 
	,RDB$SECURITY_CLASS
	,RDB$EXTERNAL_FILE
	,RDB$RUNTIME
	,RDB$EXTERNAL_DESCRIPTION
	,RDB$OWNER_NAME
	,RDB$DEFAULT_CLASS
	,RDB$FLAGS
	,RDB$RELATION_TYPE 
FROM RDB$RELATIONS AS rel 
WHERE rdb$view_blr IS NULL 
AND 
( 
    rdb$system_flag IS NULL 
    OR 
    rdb$system_flag = 0 
) 
ORDER BY table_name 
;
";

			using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
			{
				// this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

				return this.GetDataTable(cmd, strInitialCatalog);
			}

		} // End Function GetTables


		// http://www.firebirdfaq.org/faq174/
		public override System.Data.DataTable GetViews(string strInitialCatalog)
		{
			//string strCatalog = this.m_DatabaseConfiguration.strInitialCatalog;

			//if (!string.IsNullOrEmpty(strCatalog))
			//    strCatalog = strCatalog.Trim();

			//if (string.IsNullOrEmpty(strCatalog))
			//    return null;

			string strSQL = @"
SELECT 
	 RDB$VIEW_BLR
	,RDB$VIEW_SOURCE
	,RDB$DESCRIPTION
	,RDB$RELATION_ID
	,RDB$SYSTEM_FLAG
	,RDB$DBKEY_LENGTH
	,RDB$FORMAT
	,RDB$FIELD_ID
	,RDB$RELATION_NAME AS table_name 
	,RDB$SECURITY_CLASS
	,RDB$EXTERNAL_FILE
	,RDB$RUNTIME
	,RDB$EXTERNAL_DESCRIPTION
	,RDB$OWNER_NAME
	,RDB$DEFAULT_CLASS
	,RDB$FLAGS
	,RDB$RELATION_TYPE 
FROM RDB$RELATIONS AS rel 
WHERE rdb$view_blr IS NOT NULL 
AND 
( 
    rdb$system_flag IS NULL 
    OR 
    rdb$system_flag = 0 
) 
ORDER BY table_name 
;
";

			using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
			{
				// this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

				return this.GetDataTable(cmd, strInitialCatalog);
			}

		} // End Function GetViews




		public override System.Data.DataTable GetProcedures(string strDB)
        {
            string strSQL = @"
SELECT 
	proc.RDB$PROCEDURE_NAME AS ROUTINE_NAME  
FROM RDB$PROCEDURES AS proc 

ORDER BY ROUTINE_NAME ASC 
;";

            return GetDataTable(strSQL);
        } // End Function GetProcedures


		public override System.Data.DataTable GetFunctions(string strDB)
        {
            string strSQL = @"
SELECT 
	fn.RDB$FUNCTION_NAME AS ROUTINE_NAME 
FROM RDB$FUNCTIONS AS fn
WHERE fn.RDB$SYSTEM_FLAG=0

ORDER BY ROUTINE_NAME ASC 
;";

            return GetDataTable(strSQL);
        } // End Function GetFunctions


        public override System.Data.DataTable GetRoutines(string initialCatalog)
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


		public override System.Data.DataTable GetRoutineParameters(string strRoutineName, string strDbName)
		{
			System.Data.DataTable dt = null;

			string strSQL = @"
SELECT 
     params.rdb$procedure_name AS routine_name 
	,params.rdb$parameter_name AS parameter_name 
	,params.rdb$parameter_name AS SPECIFIC_NAME 
	,params.RDB$PARAMETER_NUMBER AS ordinal_position 
    
	,
    CASE params.RDB$PARAMETER_TYPE 
    	WHEN 0 THEN 'out' 
    	ELSE 'in' -- 1
    END AS parameter_mode 
    
	,fields.rdb$field_name 
	,fields.rdb$field_type 
	,typ.RDB$TYPE_NAME AS data_type 
	,fields.RDB$FIELD_LENGTH AS CHARACTER_MAXIMUM_LENGTH 
	,fields.RDB$FIELD_PRECISION AS NUMERIC_PRECISION 
    ,fields.RDB$FIELD_SCALE AS NUMERIC_SCALE 
    -- ,ms_sql.is_result
	-- ,ms_sql.as_locator
FROM rdb$procedure_parameters AS params 

LEFT JOIN rdb$fields AS fields
    ON fields.rdb$field_name = params.rdb$field_source
    
LEFT JOIN RDB$TYPES AS typ 
    ON typ.RDB$FIELD_NAME = 'RDB$FIELD_TYPE' 
	AND typ.RDB$TYPE = fields.RDB$FIELD_TYPE 
	    
WHERE (1=1) 
AND (params.rdb$procedure_name = @in_strRoutineName) 
-- AND ORDINAL_POSITION > 0 
ORDER BY ROUTINE_NAME, SPECIFIC_NAME, ORDINAL_POSITION 
";

			using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
			{
				this.AddParameter(cmd, "in_strRoutineName", strRoutineName);

				//this.GetDataTable(cmd,"");
				dt = this.GetDataTable(cmd, strDbName);
			} // End Using cmd


			return dt;
		} // End Function GetRoutineParameters


        public override System.Data.DataTable GetColumnNames()
        {
            string strSQL = @"
SELECT 
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
ORDER BY 1, f.rdb$field_position; 
";

            return GetDataTable(strSQL);
        } // End Function GetColumnNames



		// http://www.alberton.info/firebird_sql_meta_info.html#.UwoJctvX3Qp

        // http://www.firebirdfaq.org/faq174/
		public override System.Data.DataTable GetColumnNamesForTable(string tableName, string dbName)
        {
            if (!string.IsNullOrEmpty(tableName))
                tableName = tableName.Trim();

            if (string.IsNullOrEmpty(tableName))
                return null;

            tableName = tableName.ToLower().Replace("'", "''");


            string strSQL = @"

SELECT 
     r.RDB$RELATION_NAME AS table_name 
    ,r.RDB$FIELD_NAME AS column_name 
    ,r.RDB$FIELD_POSITION AS ordinal_position 
     
    ,f.RDB$FIELD_TYPE AS DataTypeId 
    ,typ.RDB$TYPE_NAME AS data_type 
     
    ,CASE WHEN typ.RDB$TYPE_NAME = 'BLOB' THEN f.RDB$FIELD_SUB_TYPE ELSE '' END AS SubType 
    ,CASE WHEN typ.RDB$TYPE_NAME = 'BLOB' THEN sub.RDB$TYPE_NAME ELSE '' END AS SubTypeName 
    ,f.RDB$FIELD_LENGTH AS CHARACTER_MAXIMUM_LENGTH 
    ,f.RDB$FIELD_PRECISION AS NUMERIC_PRECISION 
    ,f.RDB$FIELD_SCALE AS NUMERIC_SCALE 
    ,MIN(rc.RDB$CONSTRAINT_TYPE) AS ConstraintType 
    ,MIN(i.RDB$INDEX_NAME) AS Idx 
    ,CASE WHEN r.RDB$NULL_FLAG = 1 THEN 'NO' ELSE 'YES' END AS Is_NULLABLE 
    ,r.RDB$DEFAULT_VALUE AS DefaultValue 
FROM RDB$RELATION_FIELDS AS r 

LEFT JOIN RDB$FIELDS AS f 
    ON r.RDB$FIELD_SOURCE = f.RDB$FIELD_NAME 
    
INNER JOIN rdb$relations AS rel 
    ON rel.rdb$relation_name = r.rdb$relation_name 
    
LEFT JOIN RDB$INDEX_SEGMENTS AS s 
    ON s.RDB$FIELD_NAME = r.RDB$FIELD_NAME 
    
LEFT JOIN RDB$INDICES AS i 
    ON i.RDB$INDEX_NAME = s.RDB$INDEX_NAME 
    AND i.RDB$RELATION_NAME = r.RDB$RELATION_NAME 
    
LEFT JOIN RDB$RELATION_CONSTRAINTS AS rc 
    ON rc.RDB$INDEX_NAME = s.RDB$INDEX_NAME 
	AND rc.RDB$INDEX_NAME = i.RDB$INDEX_NAME 
	AND rc.RDB$RELATION_NAME = i.RDB$RELATION_NAME 
	
LEFT JOIN RDB$REF_CONSTRAINTS AS REFC 
    ON rc.RDB$CONSTRAINT_NAME = refc.RDB$CONSTRAINT_NAME 
    
LEFT JOIN RDB$TYPES AS typ 
    ON typ.RDB$FIELD_NAME = 'RDB$FIELD_TYPE' 
	AND typ.RDB$TYPE = f.RDB$FIELD_TYPE 
	
LEFT JOIN RDB$TYPES AS sub 
    ON sub.RDB$FIELD_NAME = 'RDB$FIELD_SUB_TYPE' 
	AND sub.RDB$TYPE = f.RDB$FIELD_SUB_TYPE 
    
WHERE LOWER(r.RDB$RELATION_NAME) = '" + tableName.Replace("'", "''") + @"' 

AND rel.rdb$view_blr IS NULL 

AND 
( 
    rel.rdb$system_flag IS NULL 
    OR 
    rel.rdb$system_flag = 0 
) 

GROUP BY table_name, column_name, DataTypeId, data_type, SubType, SubTypeName, CHARACTER_MAXIMUM_LENGTH, NUMERIC_PRECISION, NUMERIC_SCALE, Is_NULLABLE, DefaultValue, ordinal_position 

ORDER BY table_name, ordinal_position
;
";

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
                string strSQL = "SELECT rdb$get_context('SYSTEM', 'ENGINE_VERSION') as version from rdb$database;";
                return ExecuteScalar<string>(strSQL);
            }
        } // End Property DBversion


        public static void Test()
        {
            cDAL DAL = new cFireBird();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cFirebird_SQL


} // End Namespace DataBase.Tools
