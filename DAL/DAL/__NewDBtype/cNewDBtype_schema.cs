
namespace DB.Abstraction
{

    partial class cNewDBtype : cDAL
    {

        ////////////////////////////// Schema //////////////////////////////

        public override void CreateDB()
        {
            CreateDB("", "", "");
        }


        // http://web.firebirdsql.org/dotnetfirebird/create-a-new-database-from-an-sql-script.html
        public override void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
        {
            throw new System.NotImplementedException("Sub CreateDB is not implemented for cNewDBtype.");
        }


		public override string GetConnectionString (string strDb)
		{
            throw new System.NotImplementedException();
		}


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetTables()
        {
            string strCatalog = "strInitialCatalog";

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return null;

            strCatalog = strCatalog.Replace("'", "''");

            string strSQL = @"
            SELECT * 
            FROM INFORMATION_SCHEMA.tables 
            WHERE table_schema = '" + strCatalog + "' ";

            return GetDataTable(strSQL);
        } // End Function GetTables


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetViews()
        {
            string strCatalog = "strInitialCatalog";

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return null;

            strCatalog = strCatalog.Replace("'", "''");

            string strSQL = @"
            SELECT * 
            FROM INFORMATION_SCHEMA.views 
            WHERE table_schema = '" + strCatalog + "' ";


            return GetDataTable(strSQL);
        } // End Function GetViews


        public override System.Data.DataTable GetProcedures()
        {
            string strCatalog = "strInitialCatalog";

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return null;

            strCatalog = strCatalog.Replace("'", "''");


            string strSQL = @"
			SELECT * FROM INFORMATION_SCHEMA.routines 
            WHERE routine_schema = '" + strCatalog + @"' 
            AND routine_type = 'PROCEDURE' 
			";

            return GetDataTable(strSQL);
        } // End Function GetProcedures


        public override System.Data.DataTable GetFunctions()
        {
            string strCatalog = "strInitialCatalog";

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return null;

            strCatalog = strCatalog.Replace("'", "''");


            string strSQL = @"
			SELECT * FROM INFORMATION_SCHEMA.routines 
            WHERE routine_schema = '" + strCatalog + @"' 
            AND routine_type = 'FUNCTION' 
			";

            return GetDataTable(strSQL);
        } // End Function GetFunctions


        // http://www.firebirdfaq.org/faq174/
        public override System.Data.DataTable GetColumnNamesForTable(string strTableName)
        {
            string strCatalog = "strInitialCatalog";

            if (!string.IsNullOrEmpty(strCatalog))
                strCatalog = strCatalog.Trim();

            if (string.IsNullOrEmpty(strCatalog))
                return null;

            strCatalog = strCatalog.Replace("'", "''");


            if (!string.IsNullOrEmpty(strTableName))
                strTableName = strTableName.Trim();

            if (string.IsNullOrEmpty(strTableName))
                return null;

            strTableName = strTableName.Replace("'", "''");


            string strSQL = @"
SELECT * 
FROM INFORMATION_SCHEMA.columns
WHERE table_schema = '" + strCatalog + @"'
AND table_name = '" + strTableName + @"'
ORDER BY table_name, ordinal_position
";

            return GetDataTable(strSQL);
        } // End Function GetColumnNamesForTable


        public override bool TableExists(string strTableName)
        {
            if (!string.IsNullOrEmpty(strTableName))
                strTableName = strTableName.Trim();

            if (string.IsNullOrEmpty(strTableName))
                return false;

            strTableName = strTableName.Replace("'", "''");

            string strCatalog = "strInitialCatalog";

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


            string strCatalog = "strInitialCatalog";

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
            cDAL DAL = new cNewDBtype();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cNewDBtype


} // End Namespace DataBase.Tools
