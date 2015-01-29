
namespace DB.Abstraction
{


    partial class cMS_SQL : cDAL
    {

        ////////////////////////////// Schema //////////////////////////////
        

        // http://stackoverflow.com/questions/16343285/postgresql-how-to-list-all-available-datatypes
		public override System.Data.DataTable GetTypes()
		{
			string strSQL = @"
;WITH CTE AS
(
	SELECT 
		 name 
		
		,is_nullable
		,is_user_defined
		,is_assembly_type
		
		,
		CASE WHEN name IN ('char', 'varchar', 'text', 'ntext', 'datetime')
			THEN 1
			ELSE 0
		END AS IsDeprecated 
		
		,
		CASE 
			WHEN name = 'char' THEN 'nchar'
			WHEN name = 'varchar' THEN 'nvarchar'
			WHEN name = 'text' THEN '(n)varchar(MAX)'
			WHEN name = 'ntext' THEN 'nvarchar(MAX)'
			WHEN name = 'datetime' THEN 'Use date, time, datetime2 instead, or datetimeoffset when using a non-utc timezone'
		END DeprecatedReplacement 
		
		,
		CASE 
			WHEN name IN ('float', 'real', 'timestamp', 'sql_variant')
				THEN 1
			ELSE 0
		END as IsAntiPattern 
		
		,
		CASE 
			WHEN name IN ('float', 'real')
				THEN 'use decimal/numeric(x,y) instead'
			WHEN name = 'timestamp' 
				THEN 'is a random number, not an actual timestamp'
			WHEN name = 'sql_variant' 
				THEN 'Consider using type-safe type instead'
		END AS AntiPatternReplacement 
		 
		,
		CASE 
			WHEN name = 'nvarchar' 
				THEN max_length / 2 
			ELSE max_length	
		END AS max_length 
			
		,precision	
		,scale
		
		,CASE 
			WHEN name IN ('bit', 'tinyint', 'smallint', 'int', 'bigint')
				THEN 1 
				ELSE 0 
		END AS IsInteger 
		
		,CASE 
			WHEN name IN ('real', 'float', 'smallmoney', 'money', 'decimal', 'numeric')
				THEN 1
				ELSE 0
		END IsDecimal  
		
		,CASE 
			WHEN name IN ('bit', 'tinyint', 'smallint', 'int', 'bigint', 'real', 'float', 'smallmoney', 'money', 'decimal', 'numeric')
				THEN 1
				ELSE 0
		END AS [IsNumeric] 
		
		,CASE 
			WHEN name IN ('date', 'smalldatetime', 'time', 'datetime', 'datetime2', 'datetimeoffset')
				THEN 1
				ELSE 0
		END AS [IsDate] 
		
		,CASE 
			WHEN name IN ('geography', 'geometry', 'hierarchyid')
				THEN 1
				ELSE 0
		END AS [IsGeographic] 
		
		
		,CASE 
			WHEN name IN ('char', 'nchar', 'text', 'ntext', 'varchar', 'nvarchar', 'sysname')
				THEN 1
				ELSE 0
		END AS [IsText] 
		
		,
		CASE 
			WHEN name = 'nchar' THEN 1 
			WHEN name = 'char' THEN 2 
			WHEN name = 'nvarchar' THEN 3
			WHEN name = 'varchar' THEN 4
			WHEN name = 'ntext' THEN 5 
			WHEN name = 'text' THEN 6 
			WHEN name = 'sysname' THEN 7 
			ELSE 0 
		END AS TextSort  


		,
		CASE 
			WHEN name IN ('timestamp', 'uniqueidentifier')
				THEN 1
				ELSE 0
		END AS [IsId] 
		
		,
		CASE 
			WHEN name = 'uniqueidentifier' THEN 1 
			WHEN name = 'timestamp' THEN 2 
			ELSE 0 
		END AS IdSort   

		,
		CASE 
			WHEN name IN ('binary', 'varbinary', 'image', 'xml', 'sql_variant')
				THEN 1
				ELSE 0
		END AS [IsData] 
		
        ,
		CASE LOWER(name) 
			-- Ids
			WHEN 'uniqueidentifier' THEN 'A globally unique identifier (GUID).'
			WHEN 'timestamp' THEN 'A database-wide unique number that gets updated every time a row gets updated.'
								
			-- Integer
			WHEN 'bit' THEN 'Integer data with either a 1 or 0 value, or ''true''/''false'''
			WHEN 'tinyint' THEN 'Signed 8-Bit integer data from 0 through 255.'
			WHEN 'smallint' THEN 'Unsigned 16-Bit integer data from -2^15 (-32,768) through 2^15 - 1 (32,767).'
			WHEN 'int' THEN 'Unsigned 32-Bit integer (whole number) data from -2^31 (-2,147,483,648) through 2^31 - 1 (2,147,483,647).'
			WHEN 'bigint' THEN 'Unsigned 64-Bit integer (whole number) data from -2^63 (-9,223,372,036,854,775,808) through 2^63-1 (9,223,372,036,854,775,807)'
								 
			-- Decimal
			WHEN 'smallmoney' THEN 'Monetary data values from -214,748.3648 through +214,748.3647, with accuracy to a ten-thousandth of a monetary unit.'
			WHEN 'money' THEN 'Monetary data values from -2^63 (-922,337,203,685,477.5808) through 2^63 - 1 (+922,337,203,685,477.5807), with accuracy to a ten-thousandth of a monetary unit.'
			WHEN 'real' THEN 'Floating precision number data with the following valid values: -3.40E + 38 through -1.18E - 38, 0 and 1.18E - 38 through 3.40E + 38.'
			WHEN 'decimal' THEN 'Fixed precision and scale numeric data from -10^38 +1 through 10^38 –1. '
			WHEN 'numeric' THEN 'Functionally equivalent to decimal.'
			WHEN 'float' THEN 'Floating precision number data with the following valid values: -1.79E + 308 through -2.23E - 308, 0 and 2.23E + 308 through 1.79E + 308.'
			
			-- Date and Time
			WHEN 'date' THEN ''
			WHEN 'smalldatetime' THEN 'Date and time data from January 1, 1900, through June 6, 2079, with an accuracy of one minute.'
			WHEN 'time' THEN ''
			WHEN 'datetime' THEN 'Date and time data from January 1, 1753, through December 31, 9999, with an accuracy of three-hundredths of a second, or 3.33 milliseconds.'
			WHEN 'datetime2' THEN ''
			WHEN 'datetimeoffset' THEN ''
								 
			-- Text
			WHEN 'nchar' THEN 'unicode char'
			WHEN 'char' THEN 'ANSI char Fixed-length non-Unicode character data with a maximum length of 8,000 characters.'
			WHEN 'nvarchar' THEN 'Unicode text of varying length;  Use nvarchar(MAX) for large pieces of string data; SQL-92 standard notation: national character varying(length), maximum length 4000 unicode characters' 
			WHEN 'varchar' THEN 'ANSI text of varying length;  Use varchar(MAX) for large pieces of string data; SQL-92 standard notation: character varying(length), maximum length 8000 ANSI characters' 
			WHEN 'ntext' THEN 'Deprecated, use nvarchar(MAX) instead (aggregate and string functions don''t work on this type), Unicode text, max. 2^30 - 1 (1''073''741''823) bytes'
			WHEN 'text' THEN 'Deprecated, use varchar(MAX) instead (aggregate and string functions don''t work on this type), ANSI text, the ISO synonym for ntext is ''national text'', max. 2^31-1 (2''147''483''647) bytes = appx. 2 billion characters'
			WHEN 'sysname' THEN 'name of a table/column'
								 
			-- Geography
			WHEN 'geography' THEN ''
			WHEN 'geometry' THEN ''
			WHEN 'hierarchyid' THEN ''
								 
			-- Data
			WHEN 'xml' THEN ''
			WHEN 'image' THEN 'Variable-length binary data from size 0 through 2^31-1 (2''147''483''647) bytes.'
			WHEN 'binary' THEN ''
			WHEN 'varbinary' THEN ''
			WHEN 'sql_variant' THEN 'This is NOT typesafe. Try to never ever use this one, OK ?'

            ELSE 'no description added' 
		END AS [Description] 
	FROM sys.types 
	WHERE (1=1) 
    
	--SELECT * FROM SysTypes -- sql server 2000: WHERE xusertype > 256 -- user defined types

	--AND is_user_defined = 1

	--AND scale = 0
)

SELECT 
	CASE 
		WHEN [IsId] = 1 THEN 'ID'
		WHEN IsInteger = 1 THEN 'Integer'
		WHEN IsDecimal = 1 THEN 'Decimal'
		WHEN [IsDate] = 1 THEN 'Date/Time'
		WHEN [IsText] = 1 THEN 'String'
		WHEN [IsGeographic] = 1 THEN 'Geographic'
		WHEN IsData = 1 THEN 'Data'
	END AS GroupName
	
	,DENSE_RANK() OVER 
	(
		ORDER BY 
			CASE 
				WHEN [IsId] = 1 THEN 'ID'
				WHEN IsInteger = 1 THEN 'Integer'
				WHEN IsDecimal = 1 THEN 'Decimal'
				WHEN [IsDate] = 1 THEN 'Date/Time'
				WHEN [IsText] = 1 THEN 'String'
				WHEN [IsGeographic] = 1 THEN 'Geographic'
				WHEN IsData = 1 THEN 'Data'
			END  
	) AS GroupId 
	
	,CTE.* 
FROM CTE 
ORDER BY 
	 [IsId] DESC  
	,IdSort ASC 
	,IsInteger DESC 
	,IsDecimal DESC 
	,[IsNumeric] DESC 
	,[IsDate] DESC 
	,[IsText] DESC 
	,TextSort ASC 
	
	,[IsGeographic] DESC 
	
	
	
	,[IsData]
	,
	CASE 
		WHEN [IsNumeric] = 1 THEN precision 
		WHEN [IsDate] = 1 THEN precision 
		WHEN [IsData] = 1 THEN max_length 
		ELSE 0
	END 
	
	,name ASC 
";

			return GetDataTable(strSQL);
		}


        // http://sqlserverplanet.com/indexes/create-index/
        // http://www.codeproject.com/KB/database/Define_Index_in_SQLServer.aspx
        // http://blog.sqlauthority.com/2007/01/31/sql-server-reindexing-database-tables-and-update-statistics-on-tables/
        // http://msdn.microsoft.com/en-us/library/ms187348.aspx
        public System.Data.DataTable GetIndices()
        {
            string strSQL = @"
SELECT 
     ind.name 
    ,ind.index_id 
    ,ic.index_column_id 
    ,col.name 
    ,ind.* 
    ,ic.* 
    ,col.* 
FROM sys.indexes ind 

INNER JOIN sys.index_columns ic 
    ON  ind.object_id = ic.object_id 
    AND ind.index_id = ic.index_id 
    
INNER JOIN sys.columns col 
	ON ic.object_id = col.object_id 
	AND ic.column_id = col.column_id 
	
INNER JOIN sys.tables t 
	ON ind.object_id = t.object_id 
	
WHERE (1=1) 
    AND ind.is_primary_key = 0 
    AND ind.is_unique = 0 
    AND ind.is_unique_constraint = 0 
    AND t.is_ms_shipped = 0 
ORDER BY 
    t.name, ind.name, ind.index_id, ic.index_column_id 
";

            return this.GetDataTable(strSQL);
        }


        public System.Data.DataTable FindIdendityColumns()
        {
            // http://weblogs.asp.net/psteele/archive/2003/12/03/41051.aspx
            string strSQL = @"
SELECT 
	 TABLE_NAME + '.' + COLUMN_NAME AS TableAndColumn 
	,TABLE_NAME AS TABLE_NAME 
	,COLUMN_NAME AS COLUMN_NAME 
	,ORDINAL_POSITION AS ORDINAL_POSITION 
	,DATA_TYPE AS DATA_TYPE 
	
	,
	(
		SELECT IDENT_CURRENT('[dbo].[' + TABLE_NAME + ']')
	) AS CurrentIteratorValue 
	
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE (1=1) 
AND TABLE_SCHEMA = 'dbo' 
AND COLUMNPROPERTY(OBJECT_ID(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1 

AND TABLE_NAME IN
(
	SELECT TABLE_NAME 
	FROM INFORMATION_SCHEMA.TABLES 
	WHERE TABLE_TYPE = 'BASE TABLE' 
)

ORDER BY TABLE_NAME, ORDINAL_POSITION 
";
            return this.GetDataTable(strSQL);
        }


        public long GetCurrentIdendity(string strTableName)
        {
            strTableName = strTableName.Replace("'", "''");
            // http://blog.sqlauthority.com/2007/03/25/sql-server-identity-vs-scope_identity-vs-ident_current-retrieve-last-inserted-identity-of-record/
            // http://www.techrepublic.com/blog/datacenter/how-do-i-reseed-a-sql-server-identity-column/406
            // http://sqlservercodebook.blogspot.com/2008/03/to-get-current-identity-value-from.html
            string strSQL = @"SELECT IDENT_CURRENT('[dbo].[" + strTableName + "]')";

            return this.ExecuteScalar<long>(strSQL);
        }


        public override string GetDataBasesQueryText(dbOwner ShowDBs)
        {
            string strSQL = @"
SELECT 
     name 
    ,owner_sid 
    ,create_date 
    ,compatibility_level 
    ,collation_name 
FROM sys.databases 
";

            if ((uint)(ShowDBs & dbOwner.user) != 0)
            {
                strSQL += @"
WHERE owner_sid != 0x01 
";
            }

            strSQL += @"
ORDER BY name ASC 
";

            return strSQL;
        }



        public override System.Data.DataTable GetDataBases()
        {
            return GetDataBases(dbOwner.all);
        } // End Sub GetDataBases


        public System.Data.DataTable GetDataBases(dbOwner ShowDBs)
        {
            System.Data.DataTable dt = GetDataTable(GetDataBasesQueryText(ShowDBs), "master");

            return dt;
        } // End Function GetDataBases


        public override System.Data.DataTable GetSystemDataBases()
        {
            string strSQL = @"
SELECT 
     name 
	,collation_name 
	,owner_sid 
	,create_date 
	,compatibility_level 
FROM sys.databases 
WHERE owner_sid != 0x01 
            
ORDER BY name ASC 
";

            //System.Data.DataTable dt = GetDataTable(strSQL);
            return GetDataTable(strSQL);
        } // End Sub GetDataBases




        protected bool DatabaseExists(string strDataBaseName)
        {
            if (!string.IsNullOrEmpty(strDataBaseName))
                strDataBaseName = strDataBaseName.Trim();

            if (string.IsNullOrEmpty(strDataBaseName))
                return false;

            strDataBaseName = strDataBaseName.Replace("'", "''");

            string strSQL = @"
SELECT COUNT(*) 
FROM sys.databases
WHERE name = N'" + strDataBaseName + @"'  
";

            string strOldInitialCatalog = this.m_ConnectionString.InitialCatalog;
            this.m_ConnectionString.InitialCatalog = "master";

            bool bReturnValue = ExecuteScalar<bool>(strSQL);

            this.m_ConnectionString.InitialCatalog = strOldInitialCatalog;
            strOldInitialCatalog = null;

            return bReturnValue;
        } // End Function TableHasColumn


        public void ResetAutoIncrement(string strTableName)
        {
            //DELETE FROM tblName 
            string strSQL = "DBCC CHECKIDENT ([" + strTableName.Replace("'", "''") + "], RESEED, 0)";
            this.Execute(strSQL);
        }



        // http://msdn.microsoft.com/en-us/library/ms143508.aspx
        public void SetDBcollation(string strCollationName)
        {
            // ALTER DATABASE (database name here) COLLATE (collation name here)
            string strSQL = "ALTER DATABASE [" + this.m_ConnectionString.InitialCatalog.Replace("'","''") + "] COLLATE " + strCollationName + ";";
            this.Execute(strSQL);
        }

        // http://msdn.microsoft.com/en-us/library/ms143508.aspx
        public void SetDBcollation(string strDBname, string strCollationName)
        { 
            string strSQL = "ALTER DATABASE [" + strDBname.Replace("'","''") + "] COLLATE " + strCollationName.Replace("'","''") + ";";
            this.Execute(strSQL);
        }


        public string GetCollation(string strDBname)
        {
            // SELECT DATABASEPROPERTYEX('YOUR_DB_NAME', 'Collation') SQLCollation;
            string strSQL = "SELECT DATABASEPROPERTYEX(N'" + strDBname.Replace("'", "''") + "', 'Collation') SQLCollation";
            return this.ExecuteScalar<string>(strSQL);
        }


        public string GetFieldCollation(string strTableName, string strFieldName)
        {
            string strSQL = @"
SELECT 
	 name 
	,collation_name 
FROM sys.columns 
WHERE OBJECT_ID IN 
(
	SELECT         
		OBJECT_ID 
	FROM sys.objects 
	WHERE type = 'U' 
	AND name = N'" + strTableName.Replace("'", "''") + @"' 
) 
AND name = N'" + strFieldName.Replace("'", "''") + @"' 
";
            return this.ExecuteScalar<string>(strSQL);
        }


        public void RenameTable(string strOldTableName, string strNewTableName)
        {
            strOldTableName = strOldTableName.Replace("'", "''");
            strNewTableName = strNewTableName.Replace("'", "''");
            // for renaming a table
            // EXEC sp_rename 'Old_TableName', 'New_TableName'
            string strSQL = "EXEC sp_rename '" + strOldTableName + "', '" + strNewTableName + "'";
            this.Execute(strSQL);
        }


        public void RenameColumn(string strTable, string strOldColumnName, string strNewColumnName)
        {
            // for renaming a column
            // EXEC sp_rename 'TableName.[Old_ColumnName]', 'TableName.[New_ColumnName]', 'COLUMN'

            strTable = strTable.Replace("'","''");
            strOldColumnName = strOldColumnName.Replace("'", "''");
            strNewColumnName = strNewColumnName.Replace("'", "''");

            string strSQL = "EXEC sp_rename '[" + strTable + "].[" + strOldColumnName + "]', '[" + strTable + "].[" + strNewColumnName + "]', 'COLUMN'";
            this.Execute(strSQL);
        }


        public override void CreateDB()
        {
            string strDBname = this.m_ConnectionString.InitialCatalog;
            string strDataFile = strDBname + ".mdf";
            string strLogFile = strDBname + "_log.ldf";

            if (!DatabaseExists(strDBname))
                CreateDB(strDBname, strDataFile, strLogFile);
        } // End Sub CreateDB


        public override void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
        {
            //string strApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string strLocalApplicationData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

            string strBasePath = System.IO.Path.Combine(strLocalApplicationData, "EasyDNS");


            // System.Console.WriteLine(strBasePath);
            
            if(!System.IO.Directory.Exists(strBasePath))
                System.IO.Directory.CreateDirectory(strBasePath);


            if(!System.IO.Directory.Exists(strBasePath))
                throw new System.IO.DirectoryNotFoundException(strBasePath);

            
            strDataLocation = System.IO.Path.Combine(strBasePath, strDataLocation);
            strLogLocation = System.IO.Path.Combine(strBasePath, strLogLocation);
            // System.Console.WriteLine(strDataLocation);
            // System.Console.WriteLine(strLogLocation);



            string strSQLtemplate = GetEmbeddedSQLscript("Create_DataBase.sql");

            Subtext.Scripting.SqlScriptRunner srNewScript = new Subtext.Scripting.SqlScriptRunner(strSQLtemplate);

            strDBname = strDBname.Replace("'", "''");
            srNewScript.TemplateParameters["DB_NAME"].Value = "[" + strDBname + "]";
            srNewScript.TemplateParameters["DB_MDF_FILE"].Value = Insert_Unicode(strDataLocation);
            srNewScript.TemplateParameters["DB_LOG_FILE"].Value = Insert_Unicode(strLogLocation);
            srNewScript.TemplateParameters["DB_DESCRIPTION"].Value = Insert_Unicode(@"EasyDNS Database");

            string strOldInitialCatalog = this.m_ConnectionString.InitialCatalog;
            this.m_ConnectionString.InitialCatalog = "master";
            

            foreach (Subtext.Scripting.Script stcThisScript in srNewScript.ScriptCollection)
            {
                string strSQL = stcThisScript.ScriptText;
                // System.Console.WriteLine(strSQL);
                Execute(strSQL);

                strSQL = null;
            } // stcThisScript

            this.m_ConnectionString.InitialCatalog = strOldInitialCatalog;
            
            strOldInitialCatalog = null;
            strSQLtemplate = null;
            srNewScript.ScriptCollection.Clear();
            srNewScript.TemplateParameters.Clear();
            srNewScript = null;
        } // CreateDB


        public override System.Data.DataTable GetTables()
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
AND TABLE_NAME NOT IN 
(
     'dtproperties'
    ,'sysdiagrams'
)
ORDER BY TABLE_NAME ASC 
";

            return GetDataTable(strSQL);
        } // End Function GetTables


        public override System.Data.DataTable GetTables(string strInitialCatalog)
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE' 
AND TABLE_NAME NOT IN 
(
     'dtproperties'
    ,'sysdiagrams'
)
ORDER BY TABLE_NAME ASC 
";

            System.Data.DataTable dt = this.GetDataTable(strSQL, strInitialCatalog);

            return dt;
        } // End Function GetTables


        public override System.Data.DataTable GetViews()
        {
            return GetViews(null);
        } // End Function GetViews


        public override System.Data.DataTable GetViews(string strInitialCatalog)
        {
            string strSQL = @"
            SELECT * FROM INFORMATION_SCHEMA.VIEWS 
            ORDER BY TABLE_NAME ASC 
            ";

            return GetDataTable(strSQL, strInitialCatalog);
        } // End Function GetViews


        public override System.Data.DataTable GetProcedures()
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_TYPE = 'PROCEDURE' 
AND ROUTINE_NAME NOT IN 
(
     'fn_diagramobjects' 
    ,'sp_creatediagram' 
    ,'dt_adduserobject' 
    ,'dt_droppropertiesbyid' 
    ,'dt_dropuserobjectbyid' 
    ,'dt_generateansiname' 
    ,'dt_getobjwithprop' 
    ,'dt_getobjwithprop_u' 
    ,'dt_getpropertiesbyid' 
    ,'dt_getpropertiesbyid_u' 
    ,'dt_setpropertybyid' 
    ,'dt_setpropertybyid_u' 
    ,'dt_verstamp006' 
    ,'dt_verstamp007' 
    ,'sp_alterdiagram' 
    ,'sp_dropdiagram' 
    ,'sp_helpdiagramdefinition' 
    ,'sp_helpdiagrams' 
    ,'sp_renamediagram' 
    ,'sp_upgraddiagrams' 
)
ORDER BY ROUTINE_TYPE, ROUTINE_NAME ASC 
";

            return GetDataTable(strSQL);
        } // End Function GetProcedures


        public override System.Data.DataTable GetFunctions()
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE ROUTINE_TYPE = 'FUNCTION' 
AND ROUTINE_NAME NOT IN 
(
     'fn_diagramobjects' 
    ,'sp_creatediagram' 
    ,'dt_adduserobject' 
    ,'dt_droppropertiesbyid' 
    ,'dt_dropuserobjectbyid' 
    ,'dt_generateansiname' 
    ,'dt_getobjwithprop' 
    ,'dt_getobjwithprop_u' 
    ,'dt_getpropertiesbyid' 
    ,'dt_getpropertiesbyid_u' 
    ,'dt_setpropertybyid' 
    ,'dt_setpropertybyid_u' 
    ,'dt_verstamp006' 
    ,'dt_verstamp007' 
    ,'sp_alterdiagram' 
    ,'sp_dropdiagram' 
    ,'sp_helpdiagramdefinition' 
    ,'sp_helpdiagrams' 
    ,'sp_renamediagram' 
    ,'sp_upgraddiagrams' 
)
ORDER BY ROUTINE_TYPE, ROUTINE_NAME ASC 
";

            return GetDataTable(strSQL);
        } // End Function GetFunctions


        public override System.Data.DataTable GetRoutines()
        {
            return GetRoutines(null);
        } // End Function GetRoutines


        public override System.Data.DataTable GetRoutines(string strInitialCatalog)
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.ROUTINES 
WHERE (1=1) 
AND ROUTINE_NAME NOT IN 
(
     'fn_diagramobjects' 
    ,'sp_creatediagram' 
    ,'dt_adduserobject' 
    ,'dt_droppropertiesbyid' 
    ,'dt_dropuserobjectbyid' 
    ,'dt_generateansiname' 
    ,'dt_getobjwithprop' 
    ,'dt_getobjwithprop_u' 
    ,'dt_getpropertiesbyid' 
    ,'dt_getpropertiesbyid_u' 
    ,'dt_setpropertybyid' 
    ,'dt_setpropertybyid_u' 
    ,'dt_verstamp006' 
    ,'dt_verstamp007' 
    ,'sp_alterdiagram' 
    ,'sp_dropdiagram' 
    ,'sp_helpdiagramdefinition' 
    ,'sp_helpdiagrams' 
    ,'sp_renamediagram' 
    ,'sp_upgraddiagrams' 
)
ORDER BY ROUTINE_TYPE, ROUTINE_NAME ASC 
";

            return GetDataTable(strSQL, strInitialCatalog);
        } // End Function GetRoutines


        

        public override System.Data.DataTable GetColumnNames()
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE (1=1) 
AND TABLE_NAME NOT IN 
(
     'dtproperties'
    ,'sysdiagrams'
)
ORDER BY TABLE_NAME, ORDINAL_POSITION 
";

            return GetDataTable(strSQL);
        } // End Function GetColumnNames


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
                        strSQL += "     [" + strColumnName + "] " + strNewLine;
                    else
                        strSQL += "    ,[" + strColumnName + "] " + strNewLine;
                } // Next i

            } // End Using dt

            strSQL += "FROM [" + strTableName + "] " + strNewLine;

            return strSQL;
        } // End Function GetTableSelectText


        public override string GetTableInsertText(string strTableName)
        {
            string strNewLine = "\r\n"; //  Environment.NewLine
            string strSQL = strNewLine;


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
                        strIsNullable = "nullable";
                    }
                    else
                        strIsNullable = "not nullable";

                    if (!string.IsNullOrEmpty(strCharacterMaximumLength))
                    {
                        if (strCharacterMaximumLength == "-1")
                            strCharacterMaximumLength = "MAX";

                        strCharacterMaximumLength = "(" + strCharacterMaximumLength + ")";
                    }
                    else
                        strCharacterMaximumLength = "";

                    strSQL += "DECLARE @" + strColumnName + " [" + strDataType + "]" + strCharacterMaximumLength + "; --" + strIsNullable + strNewLine;
                } // Next i

                strSQL += strNewLine + strNewLine;

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);

                    strSQL += "SET @" + strColumnName + " = ''; " + strNewLine;
                } // Next i

                strSQL += strNewLine + strNewLine;

                strSQL += "INSERT INTO [" + strTableName + "] " + strNewLine;
                strSQL += "( " + strNewLine;

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);

                    if (i == 0)
                        strSQL += "     [" + strColumnName + "] " + strNewLine;
                    else
                        strSQL += "    ,[" + strColumnName + "] " + strNewLine;
                } // Next i


                strSQL += ") " + strNewLine;
                strSQL += "VALUES " + strNewLine;
                strSQL += "( " + strNewLine;

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);

                    if (i == 0)
                        strSQL += "     @" + strColumnName + " " + strNewLine;
                    else
                        strSQL += "    ,@" + strColumnName + " " + strNewLine;
                } // Next i

                strSQL += ") " + strNewLine;

            } // End Using dt

            return strSQL;
        } // End Function GetTableInsertText


		public override string GetInsertScript(string strTableName)
		{
			string strNewLine = "\r\n"; //  Environment.NewLine

			// http://ssmsaddins.codeplex.com/

			// To elaborate on this correct but brief answer: Older PostgreSQL versions used to let you write C-style escapes like \n in strings. 
			// This has been turned off by default in newer versions; this warning tells you that you're on a version where it still happens.

			// Backslash in string:
			// http://stackoverflow.com/questions/17581995/postgresql-nonstandard-use-of-escape-string
			// string strInsert = strNewLine + "SET standard_conforming_strings = ON;" + strNewLine + strNewLine;
			string strInsert = strNewLine; 
			System.Text.StringBuilder sb = new System.Text.StringBuilder();

			using (System.Data.DataTable dt = GetColumnNamesForTable(strTableName))
			{
                // TODO: Implement
                bool bEscapeTableName = false;

                if (bEscapeTableName)
                    strInsert += "INSERT INTO " + this.EscapeTableName(strTableName) + " " + strNewLine;
                else
                    strInsert += "INSERT INTO " + strTableName + " " + strNewLine;


				strInsert += "( " + strNewLine;

				for (int i = 0; i < dt.Rows.Count; ++i)
				{
					string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);
                    if (bEscapeTableName)
					{

						if (i == 0)
							strInsert += "     " + this.EscapeColumnName(strColumnName) + " " + strNewLine;
						else
							strInsert += "    ," + this.EscapeColumnName(strColumnName) + " " + strNewLine;
					}
					else
					{
						if (i == 0)
							strInsert += "     " + strColumnName + " " + strNewLine;
						else
							strInsert += "    ," + strColumnName + " " + strNewLine;
					}
				} // Next i


				strInsert += ") " + strNewLine;
				strInsert += "VALUES " + strNewLine;
				strInsert += "( ";

				// string strSQL = strNewLine;

				using (System.Data.DataTable dtData = GetDataTable("SELECT * FROM " + this.EscapeTableName(strTableName)))
				{
					for (int j = 0; j < dtData.Rows.Count; ++j)
					{
						sb.AppendLine(strInsert);

						for (int i = 0; i < dt.Rows.Count; ++i)
						{
							string strDataType = System.Convert.ToString(dt.Rows[i]["DATA_TYPE"]);
							// string strCharacterMaximumLength = System.Convert.ToString(dt.Rows[i]["CHARACTER_MAXIMUM_LENGTH"]);
							// string strIsNullable = System.Convert.ToString(dt.Rows[i]["IS_NULLABLE"]);

							string strValue = null;

							string strNvarChar = "";
                            if (System.StringComparer.OrdinalIgnoreCase.Equals(strDataType, "nvarchar"))
								strNvarChar = "N";


							if (System.StringComparer.OrdinalIgnoreCase.Equals(strDataType, "datetime")
                                || System.StringComparer.OrdinalIgnoreCase.Equals(strDataType, "datetime2"))
								strValue = System.Convert.ToDateTime(dtData.Rows[j][i]).ToString("yyyy-MM-ddTHH:mm:ss.fff");
							else
								strValue = System.Convert.ToString(dtData.Rows[j][i]);

							if (i == 0)
								strValue = "     " + strNvarChar + "'" + strValue.Replace("'", "''") + "'";
							else
								strValue = "    ," + strNvarChar + "'" + strValue.Replace("'", "''") + "'";

							sb.AppendLine(strValue);
						} // Next i 

						sb.AppendLine();
						sb.AppendLine(");");
					} // Next j

				} // End Using System.Data.DataTable dtData 

			} // End Using dt

			//
			sb.Insert(0, "SET standard_conforming_strings = ON;" + strNewLine + strNewLine);

			strInsert = sb.ToString();
			//sb.Clear();
			sb = null;

			return strInsert;
		} // End Function GetTableInsertText


        public override string GetTableInsertTemplate(string strTableName)
        {
            string strNewLine = "\r\n"; //  Environment.NewLine
            string strSQL = strNewLine;

            using (System.Data.DataTable dt = GetColumnNamesForTable(strTableName))
            {
                strSQL += "INSERT INTO [" + strTableName + "] " + strNewLine;
                strSQL += "( " + strNewLine;

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);

                    if (i == 0)
                        strSQL += "     [" + strColumnName + "] " + strNewLine;
                    else
                        strSQL += "    ,[" + strColumnName + "] " + strNewLine;
                } // Next i


                strSQL += ") " + strNewLine;
                strSQL += "VALUES " + strNewLine;
                strSQL += "( " + strNewLine;

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    string strColumnName = System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]);

                    string strDataType = System.Convert.ToString(dt.Rows[i]["DATA_TYPE"]);
                    string strCharacterMaximumLength = System.Convert.ToString(dt.Rows[i]["CHARACTER_MAXIMUM_LENGTH"]);
                    string strIsNullable = System.Convert.ToString(dt.Rows[i]["IS_NULLABLE"]);

                    if (System.StringComparer.OrdinalIgnoreCase.Equals(strIsNullable, "yes"))
                    {
                        strIsNullable = "nullable";
                    }
                    else
                        strIsNullable = "not nullable";

                    if (!string.IsNullOrEmpty(strCharacterMaximumLength))
                    {
                        if (strCharacterMaximumLength == "-1")
                            strCharacterMaximumLength = "MAX";

                        strCharacterMaximumLength = "(" + strCharacterMaximumLength + ")";
                    }
                    else
                        strCharacterMaximumLength = "";

                    if (i == 0)
                        strSQL += "     <" + strColumnName + ", " + strDataType + strCharacterMaximumLength + ", > " + strNewLine;
                    else
                        strSQL += "    ,<" + strColumnName + ", " + strDataType + strCharacterMaximumLength + ", > " + strNewLine;
                } // Next i

                strSQL += ") " + strNewLine;

            } // End Using dt

            return strSQL;
        } // End Function GetTableInsertTemplate


        public override string GetTableCreateText(string strTableName, string strNewTableName)
        {
            string strNewLine = "\r\n"; //  Environment.NewLine
            string strSQL = "CREATE TABLE [dbo]." + EscapeTableName(strNewTableName) + " " + strNewLine;
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
                    } // End if (!string.IsNullOrEmpty(strCharacterMaximumLength))
                    else
                        strCharacterMaximumLength = "";

                    if (i == 0)
                        strSQL += "     [" + strColumnName + "] [" + strDataType + "]" + strCharacterMaximumLength + " " + strIsNullable + strNewLine;
                    else
                        strSQL += "    ,[" + strColumnName + "] [" + strDataType + "]" + strCharacterMaximumLength + " " + strIsNullable + strNewLine;

                } // Next i

                strSQL += ")" + strNewLine;
            } // End using dt

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
                    strSQL += @"ALTER TABLE " + EscapeTableName(strNewTableName) + " ADD CONSTRAINT [" + strKeyName + "] " + strNewLine;
                    strSQL += @"PRIMARY KEY " + strClusterType + " (" + strKeyColumns + ");" + strNewLine;
                } // Next dr 

            } // End Using dt 
            
            //	COLUMN_DEFAULT 
            return strSQL; 
        } // End Function GetTableCreateText 


        public override System.Data.DataTable GetPrimaryKeys()
        {
            string strSQL = @"
SELECT 
     OBJECT_SCHEMA_NAME(kc.parent_object_id) AS KEY_SCHEMA
    ,OBJECT_NAME(kc.parent_object_id) AS KEY_TABLE
    ,OBJECT_NAME(kc.object_id) AS KEY_NAME
    ,kc.parent_object_id AS PARENT_OBECT_ID
    ,
    CASE 
		INDEXPROPERTY(kc.parent_object_id,OBJECT_NAME(kc.object_id ),'IsClustered')
		WHEN 1 THEN ' CLUSTERED'
        ELSE ' NONCLUSTERED'
	END AS KEY_CLUSTERTYPE
    
FROM sys.key_constraints AS kc

INNER JOIN sys.objects AS o
	ON kc.parent_object_id = o.object_id
	
WHERE kc.type = 'PK' 
AND o.type = 'U'
AND o.name NOT IN ('dtproperties','sysdiagrams')  
-- not true user tables

ORDER BY 
     QUOTENAME(OBJECT_SCHEMA_NAME(kc.parent_object_id))
    ,QUOTENAME(OBJECT_NAME(kc.parent_object_id));
";


            return GetDataTable(strSQL);
        } // End Function GetPrimaryKeysForTable


        public override System.Data.DataTable GetPrimaryKeysForTable(string strTableName)
        {
            strTableName = strTableName.Replace("'", "''");

            string strSQL = @"
SELECT 
     OBJECT_SCHEMA_NAME(kc.parent_object_id) AS KEY_SCHEMA 
    ,OBJECT_NAME(kc.parent_object_id) AS KEY_TABLE 
    ,OBJECT_NAME(kc.object_id) AS KEY_NAME 
    ,
    CASE 
		INDEXPROPERTY(kc.parent_object_id,OBJECT_NAME(kc.object_id ),'IsClustered') 
		WHEN 1 THEN 'CLUSTERED' 
        ELSE 'NONCLUSTERED' 
	END AS KEY_CLUSTERTYPE 
    
FROM sys.key_constraints AS kc

INNER JOIN sys.objects AS o
	ON kc.parent_object_id = o.object_id
	
WHERE kc.type = 'PK' 
AND o.type = 'U'
AND o.name NOT IN ('dtproperties','sysdiagrams')  
-- not true user tables
            
AND OBJECT_NAME(kc.parent_object_id) = '" + strTableName + @"'
--AND o.name = 'T_ZO_AP_Standort_DWG'
            
ORDER BY 
     QUOTENAME(OBJECT_SCHEMA_NAME(kc.parent_object_id))
    ,QUOTENAME(OBJECT_NAME(kc.parent_object_id));
";

            return GetDataTable(strSQL);
        } // End Function GetPrimaryKeysForTable


        // GetColumnDefinition
        protected string GetPrimaryKeyColumns(string strTableName, string strPKname)
        {
            strTableName = strTableName.Replace("'", "''");
            strPKname = strPKname.Replace("'", "''");

            string strSQL = @"
SELECT 
     tc.constraint_name 
	,tc.table_schema 
	,kcu.column_name 
	--,tc.constraint_type 
FROM information_schema.table_constraints AS tc 

INNER JOIN information_schema.key_column_usage AS kcu 
	ON kcu.table_name = tc.table_name 
	AND kcu.constraint_name = tc.constraint_name 
	
WHERE tc.table_name = '" + strTableName + @"' 
AND kcu.constraint_name = '" + strPKname + @"' 
AND tc.constraint_type = 'primary key' 

ORDER BY kcu.ordinal_position 
";

            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();
            using(System.Data.DataTable dt = GetDataTable(strSQL))
            {

                foreach(System.Data.DataRow dr in dt.Rows)
                {
                    ls.Add("[" + System.Convert.ToString(dr["column_name"]) + "]");
                } // Next dr

            } // End Using dt

            string strReturnValue = string.Join(", ", ls.ToArray());
            ls.Clear();
            ls = null;
            return strReturnValue;
        } // End Function GetColumnNamesForTable


        public override System.Data.DataTable GetColumnNamesForTable(string strTableName, string strDbName)
        {
            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = @strTableName 
ORDER BY TABLE_NAME, ORDINAL_POSITION 
";

            System.Data.DataTable dt = null;

            using(System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "strTableName", strTableName);
                dt = this.GetDataTable(cmd, strDbName);
            } // End Using cmd 

            return dt;
        }  // End Function GetColumnNamesForTable



        // GetColumnDefinition
        public override System.Data.DataTable GetColumnNamesForTable(string strTableName)
        {
            strTableName = strTableName.Replace("'", "''");

            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = N'" + strTableName + @"' 
ORDER BY TABLE_NAME, ORDINAL_POSITION 
";

            return GetDataTable(strSQL);
        } // End Function GetColumnNamesForTable


        // GetColumnDefinition
        public override System.Data.DataTable GetRoutineParameters(string strRoutineName, string strDbName)
        {
            System.Data.DataTable dt = null;

            string strSQL = @"
SELECT * FROM INFORMATION_SCHEMA.PARAMETERS
WHERE (specific_name = @in_strRoutineName) 
AND ORDINAL_POSITION > 0 
ORDER BY ORDINAL_POSITION 
";
            using(System.Data.IDbCommand cmd = this.CreateCommand(strSQL))
            {
                this.AddParameter(cmd, "in_strRoutineName", strRoutineName);

                //this.GetDataTable(cmd,"");
                dt = this.GetDataTable(cmd, strDbName);
            } // End Using cmd


            return dt;
        } // End Function GetColumnNamesForTable



        // http://www.w3schools.com/sql/sql_default.asp
        public void DropDefaultValue()
        {
            //string strSQL = @"ALTER TABLE Persons ALTER City DROP DEFAULT ";
            string strSQL = @"ALTER TABLE Persons ALTER COLUMN City DROP DEFAULT";

            this.Execute(strSQL);
        } // End Sub DropDefaultValue 



        public override bool TableExists(string strTableName)
        {
            string strSQL = @"
            SELECT COUNT(*) FROM sys.objects 
            WHERE object_id = OBJECT_ID(N'[dbo].[" + strTableName.Replace("'", "''") + @"]') AND type in (N'U')
            ";

            return ExecuteScalar<bool>(strSQL);
        } // End Function TableExists


        public override bool IsTableEmpty(string strTableName)
        {
            string strSQL = "SELECT COUNT(*) FROM [dbo].[" + strTableName.Replace("'", "''") + "]";

            return !ExecuteScalar<bool>(strSQL);
        } // End Function IsTableEmpty


        public override bool TableHasColumn(string strTableName, string strColumnName)
        {
            string strSQL = @"
            SELECT COUNT(*) FROM INFORMATION_SCHEMA.COLUMNS 
            WHERE TABLE_NAME = N'" + strTableName.Replace("'", "''") + @"' 
            AND COLUMN_NAME = N'" + strColumnName.Replace("'", "''") + @"' 
            ";

            return ExecuteScalar<bool>(strSQL);
        } // End Function TableHasColumn


        public override void ExportTables()
        {
            System.Data.DataTable dt = GetTables();

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                GetDataTableAndWriteXML(dr["table_name"].ToString());
            } // Next dr
        } // End Sub ExportTables


        // SELECT * FROM INFORMATION_SCHEMA.VIEW_TABLE_USAGE
        // SELECT * FROM INFORMATION_SCHEMA.VIEW_COLUMN_USAGE
        public string GetHelpText(string strDatabaseObject)
        {
            System.Data.IDataReader r = ExecuteReader("EXEC sp_helptext '" + strDatabaseObject.Replace("[", "").Replace("]", "").Trim() + "'");

            System.Text.StringBuilder builder = new System.Text.StringBuilder();
            while (r.Read())
            {
                string strThisLine = System.Convert.ToString(r["Text"]);

                // Append a string to the StringBuilder
                builder.Append(strThisLine);
                //builder.AppendLine(strThisLine)
            } // Whend
            r.Close();
            r.Dispose();
            return builder.ToString();
        } // End Function GetHelpText


        public override string GetViewCreationScript(string strViewName)
        { 
            return GetHelpText(strViewName); 
        } // End Function GetViewCreationScript


        public override string GetFunctionCreationScript(string strFunctionName)
        { 
            return GetHelpText(strFunctionName); 
        } // End Function GetFunctionCreationScript


        public override string GetProcedureCreationScript(string strProcedureName)
        { 
            return GetHelpText(strProcedureName); 
        } // End Function GetProcedureCreationScript


        public override void ExportViews()
        {
            System.Data.DataTable dt = GetViews();

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                GetViewsAndWriteSQL(dr["table_name"].ToString());
            } // Next dr
        } // End Sub ExportViews


        public override void ExportFunctions()
        {
            System.Data.DataTable dt = GetFunctions();

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                GetFunctionsAndWriteSQL(dr["routine_name"].ToString());
            } // Next dr
        } // End Sub ExportFunctions


        public override void ExportProcedures()
        {
            System.Data.DataTable dt = GetProcedures();

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                GetProceduresAndWriteSQL(dr["routine_name"].ToString());
            } // Next dr
        } // End Sub ExportProcedures


        public override void ExportDatabase()
        {
            ExportKeyDependency();

            ExportTables();
            ExportViews();
            ExportFunctions();
            ExportProcedures();
        } // End Sub ExportDatabase

        
        public override void ImportTableData()
        { 
            string strBasePath = @"D:\Stefan.Steiger\Desktop\ValidationResults\SRGSSRReis_Export_sts";
            
            System.Data.DataTable dtTableInOrder = new System.Data.DataTable();
            dtTableInOrder.ReadXml(strBasePath = System.IO.Path.Combine(System.IO.Path.Combine(strBasePath, "DependencyOrder"), "Foreign_Key_Depencency_Order.xml"));

            foreach (System.Data.DataRow dr in dtTableInOrder.Rows)
            {
                string strDestTable = System.Convert.ToString(dr["TableName"]);
                System.Console.WriteLine(strDestTable);
                string strPath = System.IO.Path.Combine(strBasePath, "Tables");
                strPath = System.IO.Path.Combine(strPath, strDestTable + ".xml");

                System.Console.WriteLine(strPath);
                
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.ReadXml(strPath);
                BulkCopy(strDestTable, dt);

                dt.Clear();
                dt.Dispose();
            } // Next dr


            dtTableInOrder.Clear();
            dtTableInOrder.Dispose();
        } // End Sub ImportTableData


        public override System.Data.DataTable GetForeignKeyDependencies()
        {
            string strSQL = @"
;WITH TablesCTE(SchemaName, TableName, TableID, Ordinal) AS
(
    SELECT
        OBJECT_SCHEMA_NAME(so.object_id) AS SchemaName,
        OBJECT_NAME(so.object_id) AS TableName,
        so.object_id AS TableID,
        0 AS Ordinal
    FROM sys.objects AS so
    WHERE so.type = 'U'
    AND so.is_ms_Shipped = 0
        
    UNION ALL

    SELECT
        OBJECT_SCHEMA_NAME(so.object_id) AS SchemaName,
        OBJECT_NAME(so.object_id) AS TableName,
        so.object_id AS TableID,
        tt.Ordinal + 1 AS Ordinal
    FROM sys.objects AS so 
    
    INNER JOIN sys.foreign_keys AS f 
        ON f.parent_object_id = so.object_id 
        AND f.parent_object_id != f.referenced_object_id 
        
    INNER JOIN TablesCTE AS tt
        ON f.referenced_object_id = tt.TableID
    
    WHERE
        so.type = 'U'
        AND so.is_ms_Shipped = 0
)

SELECT DISTINCT
     t.Ordinal
    ,t.SchemaName
    ,t.TableName
    ,t.TableID
FROM TablesCTE AS t 
    
INNER JOIN
    (
        SELECT
            itt.SchemaName as SchemaName,
            itt.TableName as TableName,
            itt.TableID as TableID,
            Max(itt.Ordinal) as Ordinal
        FROM
            TablesCTE AS itt
        GROUP BY
            itt.SchemaName,
            itt.TableName,
            itt.TableID
    ) AS tt
    ON t.TableID = tt.TableID
    AND t.Ordinal = tt.Ordinal 

ORDER BY
    t.Ordinal,
    t.TableName
";


            System.Data.DataTable dt = GetDataTable(strSQL);
            dt.TableName = "Foreign_Key_Depencency_Order";
            return dt;
        } // End Function GetForeignKeyDependencies


        public void ExportKeyDependency()
        {
            System.Data.DataTable dt = GetForeignKeyDependencies();

            string strPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
            strPath = System.IO.Path.Combine(strPath, "SRF_Export_sts");
            strPath = System.IO.Path.Combine(strPath, "DependencyOrder");
            if (!System.IO.Directory.Exists(strPath))
                System.IO.Directory.CreateDirectory(strPath);

            strPath = System.IO.Path.Combine(strPath, "Foreign_Key_Depencency_Order.xml");
            
            if (System.IO.File.Exists(strPath))
                System.IO.File.Delete(strPath);
            
            dt.WriteXml(strPath, System.Data.XmlWriteMode.WriteSchema, false);
        } // End Sub ExportKeyDependency
        

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
                return "";
            }
        } // End Property DBversion


        public static void Test()
        {
            cDAL DAL = new cMS_SQL();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cMS_SQL


} // End Namespace DataBase.Tools
