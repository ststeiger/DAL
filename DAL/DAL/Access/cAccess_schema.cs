
namespace DB.Abstraction
{

    partial class cAccess : cDAL
    {

        ////////////////////////////// Schema //////////////////////////////

        public override System.Data.DataTable GetDataBases()
        {
            string strSQL = @"
            SELECT 
	            name, 
	            owner_sid, 
	            create_date, 
	            compatibility_level, 
	            collation_name 
            FROM sys.databases
            ";


            //string strOldInitialCatalog = this.m_ConnectionString.InitialCatalog;
            //this.m_ConnectionString.InitialCatalog = "master";
            //this.m_SqlConnection.ConnectionString = this.m_ConnectionString.ConnectionString;

            System.Data.DataTable dt = GetDataTable(strSQL);

            //this.m_ConnectionString.InitialCatalog = strOldInitialCatalog;
            //this.m_SqlConnection.ConnectionString = this.m_ConnectionString.ConnectionString;
            //strOldInitialCatalog = null;

            return dt;
        } // End Function GetTables


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

            //string strOldInitialCatalog = this.m_ConnectionString.InitialCatalog;
            //this.m_ConnectionString.InitialCatalog = "master";
            
            bool bReturnValue = ExecuteScalar<bool>(strSQL);

            //this.m_ConnectionString.InitialCatalog = strOldInitialCatalog;
            //strOldInitialCatalog = null;

            return bReturnValue;
        } // End Function TableHasColumn


        public override void CreateDB()
        {
            throw new System.NotImplementedException("cAccess_schema.cs ==> CreateDB");
        }


        public override void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
        {
            //string strApplicationData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string strLocalApplicationData = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData);

            string strBasePath = System.IO.Path.Combine(strLocalApplicationData, "EasyDNS");


            System.Console.WriteLine(strBasePath);
            
            if(!System.IO.Directory.Exists(strBasePath))
                System.IO.Directory.CreateDirectory(strBasePath);

            

            if(!System.IO.Directory.Exists(strBasePath))
                throw new System.IO.DirectoryNotFoundException(strBasePath);

            
            strDataLocation = System.IO.Path.Combine(strBasePath, strDataLocation);
            strLogLocation = System.IO.Path.Combine(strBasePath, strLogLocation);
            System.Console.WriteLine(strDataLocation);
            System.Console.WriteLine(strLogLocation);



            string strSQLtemplate = GetEmbeddedSQLscript("Create_DataBase.sql");

            Subtext.Scripting.SqlScriptRunner srNewScript = new Subtext.Scripting.SqlScriptRunner(strSQLtemplate);

            strDBname = strDBname.Replace("'", "''");
            srNewScript.TemplateParameters["DB_NAME"].Value = "[" + strDBname + "]";
            srNewScript.TemplateParameters["DB_MDF_FILE"].Value = Insert_Unicode(strDataLocation);
            srNewScript.TemplateParameters["DB_LOG_FILE"].Value = Insert_Unicode(strLogLocation);
            srNewScript.TemplateParameters["DB_DESCRIPTION"].Value = Insert_Unicode(@"EasyDNS Database");

            //string strOldInitialCatalog = this.m_ConnectionString.InitialCatalog;
            //this.m_ConnectionString.InitialCatalog = "master";
            

            foreach (Subtext.Scripting.Script stcThisScript in srNewScript.ScriptCollection)
            {
                string strSQL = stcThisScript.ScriptText;
                System.Console.WriteLine(strSQL);
                Execute(strSQL);

                strSQL = null;
            }

            //this.m_ConnectionString.InitialCatalog = strOldInitialCatalog;
            //this.m_SqlConnection.ConnectionString = this.m_ConnectionString.ConnectionString;

            //strOldInitialCatalog = null;
            strSQLtemplate = null;
            srNewScript.ScriptCollection.Clear();
            srNewScript.TemplateParameters.Clear();
            srNewScript = null;
        }


        public override System.Data.DataTable GetTables()
        {
            string strSQL = @"
            SELECT MSysObjects.Name AS table_name
            FROM MSysObjects
            WHERE (((Left([Name],1))<> ""~"") 
            AND ((Left([Name],4))<>""MSys"") 
            AND ((MSysObjects.Type) In (1,4,6))) 
            ORDER BY MSysObjects.Name "
            ;

            return GetDataTable(strSQL);
        } // End Function GetTables


        public override System.Data.DataTable GetViews()
        {
            /*
            string strSQL = @"
            SELECT * FROM INFORMATION_SCHEMA.VIEWS 
            ORDER BY TABLE_NAME ASC 
            ";
            */

            throw new System.NotImplementedException("GetViews not implemented.");
            //return this.GetDataTable(strSQL); 
        } // End Function GetViews


        public override System.Data.DataTable GetProcedures()
        {
            throw new System.NotImplementedException("GetProcedures not implemented.");
        } // End Function GetProcedures


        public override System.Data.DataTable GetFunctions()
        {
            throw new System.NotImplementedException("GetFunctions not implemented.");
        } // End Function GetFunctions


        public override System.Data.DataTable GetRoutines()
        {
            throw new System.NotImplementedException("GetRoutines not implemented.");
        } // End Function GetRoutines


        public override System.Data.DataTable GetColumnNames()
        {
            throw new System.NotImplementedException("GetColumnNames not implemented.");
        } // End Function GetColumnNames


        public override System.Data.DataTable GetColumnNamesForTable(string strTableName)
        {
            throw new System.NotImplementedException("GetColumnNamesForTable not implemented.");

            //return GetDataTable(strSQL);
        } // End Function GetColumnNamesForTable


        public override bool TableExists(string strTableName)
        {
            strTableName = strTableName.Replace("'", "''");

            string strSQL = @"
            SELECT COUNT(*) 
            FROM MSysObjects
            WHERE (((Left([Name],1))<> ""~"") 
            AND ((Left([Name],4))<>""MSys"") 
            AND ((MSysObjects.Type) In (1,4,6))) 
            AND Name = '" + strTableName + "' "
            ;

            return ExecuteScalar<bool>(strSQL);
        } // End Function TableExists


        public override bool IsTableEmpty(string strTableName)
        {
            string strSQL = "SELECT COUNT(*) FROM [dbo].[" + strTableName.Replace("'", "''") + "]";

            return !ExecuteScalar<bool>(strSQL);
        } // End Function IsTableEmpty


        public override bool TableHasColumn(string strTableName, string strColumnName)
        {
            throw new System.Exception("Exception in GetFunctions");
        } // End Function TableHasColumn


        public override void ExportTables()
        {
            System.Data.DataTable dt = GetTables();

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                GetDataTableAndWriteXML(dr["table_name"].ToString());
            } // Next dr
        } // End Sub ExportTables


        public override string GetViewCreationScript(string strViewName)
        {
            throw new System.NotImplementedException("GetViewCreationScript not implemented.");
        } // End Function GetViewCreationScript


        public override string GetFunctionCreationScript(string strFunctionName)
        {
            throw new System.NotImplementedException("GetFunctionCreationScript not implemented.");
        } // End Function GetFunctionCreationScript


        public override string GetProcedureCreationScript(string strProcedureName)
        {
            throw new System.NotImplementedException("GetProcedureCreationScript not implemented.");
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
WITH TablesCTE(SchemaName, TableName, TableID, Ordinal) AS
(
    SELECT
        OBJECT_SCHEMA_NAME(so.object_id) AS SchemaName,
        OBJECT_NAME(so.object_id) AS TableName,
        so.object_id AS TableID,
        0 AS Ordinal
    FROM
        sys.objects AS so
    WHERE
        so.type = 'U'
        AND so.is_ms_Shipped = 0
    UNION ALL
    SELECT
        OBJECT_SCHEMA_NAME(so.object_id) AS SchemaName,
        OBJECT_NAME(so.object_id) AS TableName,
        so.object_id AS TableID,
        tt.Ordinal + 1 AS Ordinal
    FROM
        sys.objects AS so
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
        t.Ordinal,
        t.SchemaName,
        t.TableName,
        t.TableID
    FROM
        TablesCTE AS t
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
            cDAL DAL = new cAccess();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cAccess


} // End Namespace DataBase.Tools
