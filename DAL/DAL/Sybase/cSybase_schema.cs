﻿
/*
sp_helptext 'information_schema.views'

SELECT  
	DB_NAME() AS TABLE_CATALOG,  
	SCHEMA_NAME(schema_id) AS TABLE_SCHEMA,  
	name AS TABLE_NAME,  
	object_definition(object_id) AS VIEW_DEFINITION,  
	
	CONVERT(varchar(7), 
	CASE with_check_option  
		WHEN 1 
			THEN 'CASCADE'  
		ELSE 'NONE' 
	END
	) AS CHECK_OPTION,  
	
	'NO' AS IS_UPDATABLE 
FROM sys.views 
*/


namespace DB.Abstraction
{


    partial class cSybase : cDAL
    {

        ////////////////////////////// Schema //////////////////////////////


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetTables()
        {
            string strSQL = @"SELECT 
                                rdb$relation_name AS TABLE_NAME 
                            FROM rdb$relations
                            WHERE (rdb$view_blr IS NULL) 
                            AND 
                            (
                                rdb$system_flag IS NULL 
                                OR 
                                rdb$system_flag = 0
                            );";

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
                return "";
            }
        } // End Property DBversion


        public static void Test()
        {
            cDAL DAL = new cODBC();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cODBC


} // End Namespace DataBase.Tools
