
namespace DB.Abstraction
{

    partial class cODBC : cDAL
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
