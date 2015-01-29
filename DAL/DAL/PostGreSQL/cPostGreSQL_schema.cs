
namespace DB.Abstraction
{


    // http://www.justskins.com/forums/view-definition-truncated-in-133590.html
    // http://webhelp.esri.com/arcgisserver/9.3/java/index.htm#geodatabases/views_in_postgresql.htm
    // http://www.peachpit.com/articles/article.aspx?p=31206&seqNum=5
    // http://dev.mysql.com/doc/refman/5.0/en/connector-odbc-configuration-dsn-windows.html
    partial class cPostGreSQL : cDAL
    {
        

        ////////////////////////////// Schema //////////////////////////////


		
		public override System.Data.DataTable GetTypes()
		{
			// http://stackoverflow.com/questions/3660787/how-to-list-custom-types-using-postgres-information-schema
			string strSQL = @"
SELECT 
	 n.nspname AS Schema 
	,pg_catalog.format_type(t.oid, NULL) AS Name 
	,pg_catalog.obj_description(t.oid, 'pg_type') AS Description 
	
	,t.typcategory 
	 
	,
	CASE t.typcategory
		WHEN 'N' THEN 
			CASE WHEN pg_catalog.format_type(t.oid, NULL) IN ('oid', 'regclass', 'regconfig', 'regdictionary', 'regoper', 'regoperator', 'regproc', 'regprocedure', 'regtype')
				THEN 998
				ELSE 1
			END
		
				/* CASE 
					WHEN NOT pg_catalog.format_type(t.oid, NULL) 
				ILIKE 'reg%' 
					THEN 1 
				ELSE 998 
			END */
		WHEN 'S' THEN 2
		WHEN 'D' THEN 3
		WHEN 'B' THEN 4
		WHEN 'V' THEN 4
		WHEN 'T' THEN 5
		WHEN 'I' THEN 6
		WHEN 'A' THEN 7
		WHEN 'G' THEN 8
		WHEN 'U' THEN 
			CASE pg_catalog.format_type(t.oid, NULL) 
				WHEN 'uuid' 
					THEN -1
				WHEN 'macaddr' THEN 6
				ELSE 9
			END
			
		WHEN 'P' THEN 996
		WHEN 'X' THEN 997
		--WHEN 'N' AND pg_catalog.format_type(t.oid, NULL) ILIKE 'reg%' THEN 998 
		ELSE 999 
	END AS typpreced 
	
	,
	CASE t.typcategory
		WHEN 'A' THEN 'Array types' 
		WHEN 'B' THEN 'Boolean types' 
		WHEN 'C' THEN 'Composite types' 
		WHEN 'D' THEN 'Date/time types' 
		WHEN 'E' THEN 'Enum types' 
		WHEN 'G' THEN 'Geometric types' 
		WHEN 'I' THEN 'Network address types' 
		WHEN 'N' THEN 
				CASE WHEN pg_catalog.format_type(t.oid, NULL) IN ('oid', 'regclass', 'regconfig', 'regdictionary', 'regoper', 'regoperator', 'regproc', 'regprocedure', 'regtype')
					THEN 'regTypes (numeric)'
					ELSE 'Numeric types'
				END 
		WHEN 'P' THEN 'Pseudo-types' 
		WHEN 'R' THEN 'Range types' 
		WHEN 'S' THEN 'String types' 
		WHEN 'T' THEN 'Timespan types' 
		WHEN 'U' THEN 
			CASE pg_catalog.format_type(t.oid, NULL) 
				WHEN 'uuid' 
					THEN 'UID' 
				WHEN 'macaddr' THEN 'Network address types' 
				ELSE 'User-defined types'
			END
		WHEN 'V' THEN 'Bit-string types' 
		WHEN 'X' THEN 'unknown type' 
		ELSE 'No description, please update from http://www.postgresql.org/docs/9.3/static/catalog-pg-type.html#CATALOG-TYPCATEGORY-TABLE'
	END AS GroupName 

	,t.typlen
	
	,
	CASE pg_catalog.format_type(t.oid, NULL) 
		WHEN '""char""' THEN -999 
		WHEN 'real' THEN 996 
		WHEN 'double precision' THEN 997 
		WHEN 'money' THEN 998 
		WHEN 'numeric' THEN 999 
		WHEN 'pg_node_tree'THEN 999
		ELSE t.typlen 
	END AS sortlen 
	
	--,t.*
FROM pg_catalog.pg_type AS t 

LEFT JOIN pg_catalog.pg_namespace AS n 
	ON n.oid = t.typnamespace 
	
WHERE 
(
	t.typrelid = 0 
	OR 
	(
		SELECT c.relkind = 'c' 
		FROM pg_catalog.pg_class AS c 
		WHERE c.oid = t.typrelid 
	) 
)
AND NOT EXISTS
(
	SELECT 1 
	FROM pg_catalog.pg_type AS el 
	WHERE el.oid = t.typelem 
	AND el.typarray = t.oid 
) 
AND pg_catalog.pg_type_is_visible(t.oid) 

ORDER BY typpreced, t.typcategory, sortlen, Name 
";

			return GetDataTable(strSQL);
		}




        public override string GetDataBasesQueryText(dbOwner ShowDBs)
        {
            string strSQL = @"
SELECT 
	 datname AS name 
	,datcollate AS collation_name 
	,NULL AS owner_sid 
	,NULL AS create_date 
	,0 AS compatibility_level 
	,datdba 
	,encoding 
	,datctype 
	,datistemplate 
	,datallowconn 
	,datconnlimit 
	,datlastsysoid 
	,datfrozenxid 
	,dattablespace 
	,datacl 
FROM pg_database 
WHERE datistemplate = false 
            
ORDER BY datname ASC ; 
";

            return strSQL;
        }


        // http://dba.stackexchange.com/questions/1285/how-do-i-list-all-databases-and-tables-with-psql-command-line-tool
        public System.Data.DataTable GetDataBases(dbOwner ShowDBs)
        {
            System.Data.DataTable dt = GetDataTable(GetDataBasesQueryText());

            return dt;
        } // End Function GetDataBases


		public override System.Data.DataTable GetDataBases()
		{
			return GetDataBases(dbOwner.all);
		}


        protected bool DatabaseExists(string strDataBaseName)
        {
            throw new System.NotImplementedException();
        } // End Function TableHasColumn









        public override void CreateDB()
        {
            CreateDB("", "", "");
        }


        // http://web.firebirdsql.org/dotnetfirebird/create-a-new-database-from-an-sql-script.html
        public override void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
        {
            throw new System.NotImplementedException("cPostGreSQL_schema.cs ==> CreateDB");

            try
            {

                try
                {
                    System.Console.WriteLine("Create DB");
                    // Create a new database
                    //Npgsql.NpgsqlConnection.CreateDatabase(this.m_ConnectionString.ConnectionString);
                }
                catch (Npgsql.NpgsqlException ex)
                {
                    if(ex.ErrorCode == 335544344)
                        Log("This database already exists.");
                    else
                        Log(ex.Message);
                }
            } // End Try
            catch (System.Exception ex)
            {
                if (Log("cPostGreSQL_schema.cs ==> CreateDB(string strDBname, string strDataLocation, string strLogLocation)", ex, "CreateDB"))
                    throw;
            } // End Catch
            finally
            {
                System.Threading.Thread.Sleep(2000); // Wait for disk-write complete
            } // End Finally
        }


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetTables()
        {
            string strCatalog = this.m_ConnectionString.Database;

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return null;

			return GetTables(strCatalog);
        } // End Function GetTables



        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetTables(string strInitialCatalog)
        {
            //string strCatalog = this.m_DatabaseConfiguration.strInitialCatalog;

            //if (!string.IsNullOrEmpty(strCatalog))
            //    strCatalog = strCatalog.Trim();

            //if (string.IsNullOrEmpty(strCatalog))
            //    return null;

            //strCatalog = strCatalog.Replace("'", "''");

            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
AND TABLE_CATALOG = @strInitialCatalog 
AND TABLE_SCHEMA NOT IN
( 
	 'pg_catalog' 
	,'information_schema' 
)

ORDER BY TABLE_SCHEMA, TABLE_NAME ASC 
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

                return this.GetDataTable(cmd, strInitialCatalog);
            }

        } // End Function GetTables
		

		public override System.Data.DataTable GetForeignKeyDependencies()
		{
			string strSQL = @"
WITH RECURSIVE Fkeys AS 
(
    SELECT DISTINCT 
         KCU1.TABLE_NAME AS OnTable 
        ,KCU2.TABLE_NAME AS AgainstTable 
    FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC 

    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU1 
        ON KCU1.CONSTRAINT_CATALOG = RC.CONSTRAINT_CATALOG  
        AND KCU1.CONSTRAINT_SCHEMA = RC.CONSTRAINT_SCHEMA 
        AND KCU1.CONSTRAINT_NAME = RC.CONSTRAINT_NAME 

    LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2 
        ON KCU2.CONSTRAINT_CATALOG =  RC.UNIQUE_CONSTRAINT_CATALOG  
        AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA 
        AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME 
        AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION 
)

,MyData AS 
( 
    SELECT 
         TABLE_NAME AS OnTable  
        ,FKeys.againstTable AS AgainstTable
    FROM INFORMATION_SCHEMA.TABLES 

    LEFT JOIN FKeys
        ON TABLE_NAME = FKeys.onTable  

    WHERE (1=1) 
        AND TABLE_TYPE = 'BASE TABLE'
        AND TABLE_SCHEMA = 'public'
        --AND TABLE_NAME NOT IN ('sysdiagrams', 'dtproperties') 
)


,MyRecursion AS 
(
    -- base case
    SELECT  
         OnTable AS TableName 
        ,1 AS Lvl 
    FROM MyData
    WHERE 1=1
    AND AgainstTable IS NULL 

    -- recursive case
    UNION ALL 

    SELECT 
         OnTable AS TableName
        ,r.Lvl + 1 AS Lvl 
    FROM MyData d 

    INNER JOIN MyRecursion r 
        ON d.AgainstTable = r.TableName 
)

SELECT 
     MAX(Lvl) AS Lvl 
    ,TableName
    --,strSql = 'delete from [' + tablename + ']'
FROM 
    MyRecursion
GROUP BY
    TableName

ORDER BY lvl


/*
ORDER BY 

     2 ASC
    ,1 ASC

*/
";
			
			return this.GetDataTable(strSQL);
		}
		
		
        
        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetViews(string strInitialCatalog)
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
AND table_schema NOT IN 
( 
	 'information_schema' 
	,'pg_catalog' 
); 
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

                return this.GetDataTable(cmd, strInitialCatalog);
            }

        } // End Function GetViews


        public override System.Data.DataTable GetProcedures()
        {
			string strSQL = @"
SELECT 
	 MAX(ir.routine_catalog) AS routine_catalog 
	,MAX(ir.ROUTINE_SCHEMA) AS routine_schema 
	,MAX(ir.ROUTINE_TYPE) AS routine_type 
	
	,MAX(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_name 
	,MAX(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_display_name 
	,MAX(ir.routine_name) AS orig_routine_name 
	,ir.specific_name AS specific_name 
	--,ip.data_type 
	--,ip.udt_name 
FROM information_schema.routines AS ir 

LEFT JOIN information_schema.parameters AS ip 
	ON ip.specific_name = ir.specific_name 
	
WHERE (1=1) 
AND ir.data_type = 'void' 
--AND ir.routine_catalog = @strInitialCatalog 

AND ir.ROUTINE_SCHEMA NOT IN 
( 
	 'pg_catalog' 
	,'information_schema' 
) 

GROUP BY ir.specific_name 

ORDER BY routine_display_name ASC 
";

            return GetDataTable(strSQL);
        } // End Function GetProcedures


		public override System.Data.DataTable GetFunctions()
		{
            string strCatalog = this.m_ConnectionString.Database;

			if (!string.IsNullOrEmpty(strCatalog))
				strCatalog = strCatalog.Trim();

			if (string.IsNullOrEmpty(strCatalog))
				return null;

			return GetFunctions (strCatalog);
		} // End Function GetFunctions


		public override System.Data.DataTable GetFunctions(string strCatalog)
        {
            string strSQL = @"
SELECT
	 max(ir.routine_name) AS orig_routine_name 
	,max(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_name 
	,max(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_display_name 
	,ir.specific_name AS specific_name
	--,ip.data_type
	--,ip.udt_name
FROM information_schema.routines AS ir 

LEFT JOIN information_schema.parameters as ip
	on ip.specific_name = ir.specific_name 

WHERE (1=1) 
AND ir.routine_type = 'FUNCTION' 
AND ir.DATA_TYPE NOT IN('TABLE', 'record') 
AND routine_catalog = @strCatalog 
-- AND routine_schema = 'crapbot' 

AND ir.routine_schema NOT IN 
(
	 'pg_catalog'
	,'information_schema'
)

GROUP BY ir.specific_name 

ORDER BY ir.SPECIFIC_NAME ASC 
";

            using(System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strCatalog", strCatalog);

                return GetDataTable(cmd, strCatalog);
            } // End Using cmd

        } // End Function GetFunctions


        public override System.Data.DataTable GetRoutines()
        {
            string strCatalog = this.m_ConnectionString.Database;

			if (!string.IsNullOrEmpty(strCatalog))
				strCatalog = strCatalog.Trim();

			if (string.IsNullOrEmpty(strCatalog))
				return null;

			return GetRoutines (strCatalog);
        } // End Function GetRoutines


        public override System.Data.DataTable GetRoutines(string strInitialCatalog)
        {
            string strSQL = @"
SELECT 
	 MAX(ir.routine_catalog) AS routine_catalog 
	,MAX(ir.ROUTINE_SCHEMA) AS routine_schema 
	,MAX(ir.ROUTINE_TYPE) AS routine_type 
	,MAX(ir.routine_name) AS orig_routine_name 
	,MAX(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_name 
	,MAX(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_display_name 
	,ir.specific_name AS specific_name 
	--,ip.data_type 
	--,ip.udt_name 
FROM information_schema.routines AS ir 

LEFT JOIN information_schema.parameters AS ip 
	ON ip.specific_name = ir.specific_name 
	
WHERE (1=1) 
-- AND ir.data_type = 'void' 
AND ir.routine_catalog = @strInitialCatalog 

AND ir.ROUTINE_SCHEMA NOT IN 
( 
	 'pg_catalog' 
	,'information_schema' 
) 

GROUP BY ir.specific_name 

ORDER BY routine_catalog, routine_schema, routine_type, routine_name ASC, specific_name ASC, routine_display_name ASC 
";

            return GetDataTable(strSQL, strInitialCatalog);
        } // End Function GetRoutines


        public override System.Data.DataTable GetTableValuedFunctions(string strInitialCatalog)
        {
            string strSQL = @"
SELECT 
	 MAX(ir.routine_name) AS orig_routine_name 
	,MAX(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_name 
	,MAX(ir.routine_name) || COALESCE('(' || string_agg(ip.udt_name, ', ') || ')', '') AS routine_display_name 
	,ir.specific_name AS specific_name 
	--,ip.data_type 
	--,ip.udt_name 
FROM information_schema.routines AS ir 

LEFT JOIN information_schema.parameters AS ip 
	ON ip.specific_name = ir.specific_name 
	
WHERE (1=1) 
AND ir.DATA_TYPE IN('TABLE', 'record') 

AND ir.routine_catalog = @strInitialCatalog 

AND ir.ROUTINE_SCHEMA NOT IN 
( 
	 'pg_catalog' 
	,'information_schema' 
) 

GROUP BY ir.specific_name 

ORDER BY ir.SPECIFIC_NAME ASC 
";

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strInitialCatalog", strInitialCatalog);

                return this.GetDataTable(cmd, strInitialCatalog);
            } // End Using cmd

        } // End Function GetTableValuedFunctions


        public override string GetTableSelectText(string strTableName)
        {
            string strNewLine = "\r\n"; //  Environment.NewLine
            string strSQL = strNewLine + "SELECT " + strNewLine;

            using (System.Data.DataTable dt = GetColumnNamesForTable(strTableName))
            {

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);

                    if (i == 0)
                        strSQL += "     \"" + strColumnName + "\" " + strNewLine;
                    else
                        strSQL += "    ,\"" + strColumnName + "\" " + strNewLine;
                } // Next i

            } // End Using dt

            strSQL += "FROM \"" + strTableName + "\" " + strNewLine;

            return strSQL;
        } // End Function GetTableSelectText


        public override string GetTableDeleteText(string strTableName)
        { 
            string strNewLine = "\r\n"; //  Environment.NewLine
            string strSQL = strNewLine + "DELETE FROM \"" + strTableName + "\" " + strNewLine;
            strSQL += "WHERE " + "(1=2) " + strNewLine;

            return strSQL;
        }


        public override string GetTableCreateText(string strTableName)
        {
            string strNewLine = "\r\n"; //  Environment.NewLine
            string strSQL = strNewLine + "CREATE TABLE \"" + strTableName + "\" " + strNewLine;
            strSQL += "(" + strNewLine;

            using (System.Data.DataTable dt = GetColumnNamesForTable(strTableName))
            {

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);
                    string strDataType = System.Convert.ToString(dt.Rows[i]["DATA_TYPE"]);
                    string strCharacterMaximumLength = System.Convert.ToString(dt.Rows[i]["CHARACTER_MAXIMUM_LENGTH"]);
                    string strIsNullable = System.Convert.ToString(dt.Rows[i]["IS_NULLABLE"]);

                    if (System.StringComparer.OrdinalIgnoreCase.Equals(strIsNullable, "yes"))
                    {
                        strIsNullable = "NULL";
                    }
                    else
                        strIsNullable = "NOT NULL";

                    if (!string.IsNullOrEmpty(strCharacterMaximumLength))
                    {
                        if (strCharacterMaximumLength == "-1")
                            strCharacterMaximumLength = "MAX";

                        strCharacterMaximumLength = "(" + strCharacterMaximumLength + ")";
                    }
                    else
                        strCharacterMaximumLength = "";

                    if (i == 0)
                        strSQL += "     \"" + strColumnName + "\" " + strDataType + strCharacterMaximumLength + " " + strIsNullable + strNewLine;
                    else
                        strSQL += "    ,\"" + strColumnName + "\" " + strDataType + strCharacterMaximumLength + " " + strIsNullable + strNewLine;

                } // Next i

                strSQL += ");" + strNewLine;
            } // End using dt


            strSQL += strNewLine + "ALTER TABLE \"" + strTableName + "\" OWNER TO " + this.m_ConnectionString.UserName + ";" + strNewLine;

            /*
            using (System.Data.DataTable dt = GetPrimaryKeysForTable(strTableName))
            {
                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    strSQL += strNewLine + strNewLine;// dbo.KEY_NAME

                    string strKeyName = System.Convert.ToString(dr["KEY_NAME"]);
                    string strClusterType = System.Convert.ToString(dr["KEY_CLUSTERTYPE"]);

                    // http://sqlblog.com/blogs/john_paul_cook/archive/2009/09/16/script-to-create-all-primary-keys.aspx
                    string strKeyColumns = GetPrimaryKeyColumns(strTableName, strKeyName);

                    // http://sqlserverplanet.com/sql/sql-server-add-primary-key/
                    strSQL += @"ALTER TABLE [" + strTableName + "] ADD CONSTRAINT [" + strKeyName + "] " + strNewLine;
                    strSQL += @"PRIMARY KEY " + strClusterType + " (" + strKeyColumns + ");" + strNewLine;
                } // Next dr 

            } // End Using dt 
            */

            return strSQL;
        } // End Function GetTableCreateText 


        public override System.Data.DataTable GetColumnNames()
        {
            string strCatalog = "public"; // this.m_DatabaseConfiguration.strInitialCatalog;

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return null;

            strCatalog = strCatalog.Replace("'", "''");

            string strSQL = @"SELECT * 
            FROM INFORMATION_SCHEMA.columns
            WHERE table_schema = '" + strCatalog + @"' 
            ORDER BY table_name, ordinal_position
            ";

            return GetDataTable(strSQL);
        } // End Function GetColumnNames


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetColumnNamesForTable(string strTableName)
        {
            string strCatalog = this.m_ConnectionString.Database;

			if (string.IsNullOrEmpty(strCatalog))
				return null;

			if (string.IsNullOrEmpty(strTableName))
				return null;

            strCatalog = strCatalog.Trim();
            strTableName = strTableName.Trim();

			return GetColumnNamesForTable(strTableName, strCatalog);
        } // End Function GetColumnNamesForTable


        public override System.Data.DataTable GetColumnNamesForTable(string strTableName, string strDbName)
        {
            string strSQL = @"
            SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME ILIKE @strTableName 
            ORDER BY TABLE_NAME, ORDINAL_POSITION 
            ";

            System.Data.DataTable dt = null;

            using (System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strTableName", strTableName);
                dt = this.GetDataTable(cmd, strDbName);
            } // End using cmd 

            return dt;
        } // End Function GetColumnNamesForTable
        

        public override bool TableExists(string strTableName)
        {
            if (!string.IsNullOrEmpty(strTableName))
                strTableName = strTableName.Trim();

            if (string.IsNullOrEmpty(strTableName))
                return false;

            strTableName = strTableName.Replace("'", "''");

            string strCatalog = this.m_ConnectionString.Database;

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return false;

            strCatalog = strCatalog.Replace("'", "''");


            string strSQL = @"
            SELECT COUNT(*) 
            FROM INFORMATION_SCHEMA.tables
            WHERE table_schema = '" + strCatalog + @"'
            AND table_name = '" + strTableName + @"'
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

            strTableName = strTableName.Replace("'", "''");

            if (!string.IsNullOrEmpty(strColumnName))
                strColumnName = strColumnName.Trim();

            if (string.IsNullOrEmpty(strColumnName))
                return false;

            strColumnName = strColumnName.Replace("'", "''");


            string strCatalog = this.m_ConnectionString.Database;

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return false;

            strCatalog = strCatalog.Replace("'", "''");


            string strSQL = @"SELECT * 
            FROM INFORMATION_SCHEMA.columns
            WHERE table_schema = '" + strCatalog + @"'
            AND table_name = '" + strTableName + @"'
            AND column_name = '" + strColumnName + @"' 
            ORDER BY table_name, ordinal_position
            ";

            return ExecuteScalar<bool>(strSQL);
        } // End Function TableHasColumn

        ////////////////////////////// End Schema //////////////////////////////


        public override DataBaseEngine_t DBtype   // overriding property
        {
            get
            {
                return m_dbtDBtype;
            }
        } // End Property DBtype


        public override string DBversion   // overriding property
        {
            get
            {
                return "";
            }
        } // End Property DBversion


        public static void Test()
        {
            cDAL DAL = new cPostGreSQL();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cPostGreSQL


} // End Namespace DataBase.Tools
