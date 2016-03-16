
#if !__MonoCS__


namespace DB.Abstraction
{

    partial class cOleDB : cDAL
    {

        ////////////////////////////// Schema //////////////////////////////

        public override System.Data.DataTable GetDataBases()
        {
            throw new System.NotImplementedException("cOleDB_schema.GetDataBases not implemented.");
        } // End Function GetTables


        protected bool DatabaseExists(string strDataBaseName)
        {
            if (!string.IsNullOrEmpty(strDataBaseName))
                strDataBaseName = strDataBaseName.Trim();

            if (string.IsNullOrEmpty(strDataBaseName))
                return false;

            strDataBaseName = strDataBaseName.Replace("'", "''");

            throw new System.NotImplementedException("cOleDB_schema.DatabaseExists not implemented.");
        } // End Function TableHasColumn


        public override void CreateDB()
        {
            throw new System.NotImplementedException("cOleDB_schema.cs ==> CreateDB");
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
            this.m_SqlConnection.ConnectionString = this.m_ConnectionString.ConnectionString;
            

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
            System.Data.DataTable dt = null;


            using (System.Data.OleDb.OleDbConnection dbcon = new System.Data.OleDb.OleDbConnection(this.m_ConnectionString.ConnectionString))
            {
                lock (dbcon)
                {
                    try
                    {
                        if (dbcon.State != System.Data.ConnectionState.Open)
                            dbcon.Open();

                        //string[] tblrestrictions = new string[] {null, null, null, "TABLE"};
                        //System.Data.DataTable dt1 = dbcon.GetSchema("tables", tblrestrictions);
                        // Note:
                        // Restrictions are a string array in the following format: 
                        // {TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE}.

                        // http://forums.asp.net/t/976267.aspx/1?Record+s+cannot+be+read+no+read+permission+on+MSysObjects+
                        // http://www.devart.com/dotconnect/mysql/docs/MetaData.html
                        // http://msdn.microsoft.com/en-us/library/ms254934(v=vs.80).aspx
                        // dt = dbcon.GetSchema("MetaDataCollections");
                        dt = dbcon.GetSchema("tables");
                        //dt = dbcon.GetSchema("columns");
                        //dt = dbcon.GetSchema("views");
                        //dt = dbcon.GetSchema("restrictions");
                        //dt = dbcon.GetSchema("procedures");
                        //dt = dbcon.GetSchema("DataTypes");
                        //dt = dbcon.GetSchema("Indexes");
                        //dt = dbcon.GetSchema("ReservedWords");
                        //dt = dbcon.GetSchema("DataSourceInformation");
                    }
                    catch (System.Exception ex)
                    {
                        if (Log("cOleDB_schema.GetTables", ex, "dbcon.GetSchema(\"tables\")"))
                            throw;
                    }
                    finally
                    {
                        if (dbcon.State != System.Data.ConnectionState.Closed)
                            dbcon.Close();
                    }
                } // End Lock dbcon


            } // End Using dbcon

            return dt;
        } // End Function GetTables


        public override System.Data.DataTable GetViews()
        {
            System.Data.DataTable dt = null;


            using (System.Data.OleDb.OleDbConnection dbcon = new System.Data.OleDb.OleDbConnection(this.m_ConnectionString.ConnectionString))
            {
                lock (dbcon)
                {
                    try
                    {
                        if (dbcon.State != System.Data.ConnectionState.Open)
                            dbcon.Open();

                        //string[] tblrestrictions = new string[] {null, null, null, "TABLE"};
                        //System.Data.DataTable dt1 = dbcon.GetSchema("tables", tblrestrictions);
                        // Note:
                        // Restrictions are a string array in the following format: 
                        // {TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE}.

                        // http://forums.asp.net/t/976267.aspx/1?Record+s+cannot+be+read+no+read+permission+on+MSysObjects+
                        // http://www.devart.com/dotconnect/mysql/docs/MetaData.html
                        // http://msdn.microsoft.com/en-us/library/ms254934(v=vs.80).aspx
                        dt = dbcon.GetSchema("views");
                        //dt = dbcon.GetSchema("columns");
                        //dt = dbcon.GetSchema("restrictions");
                        //dt = dbcon.GetSchema("procedures");
                        //dt = dbcon.GetSchema("DataTypes");
                        //dt = dbcon.GetSchema("Indexes");
                        //dt = dbcon.GetSchema("ReservedWords");
                        //dt = dbcon.GetSchema("DataSourceInformation");
                    }
                    catch (System.Exception ex)
                    {
                        if (Log("cOleDB_schema.GetViews", ex, "dbcon.GetSchema(\"views\")"))
                            throw;
                    }
                    finally
                    {
                        if (dbcon.State != System.Data.ConnectionState.Closed)
                            dbcon.Close();
                    }
                } // End Lock dbcon


            } // End Using dbcon

            return dt;
        } // End Function GetViews


        public override System.Data.DataTable GetProcedures()
        {
            System.Data.DataTable dt = null;


            using (System.Data.OleDb.OleDbConnection dbcon = new System.Data.OleDb.OleDbConnection(this.m_ConnectionString.ConnectionString))
            {
                lock (dbcon)
                {
                    try
                    {
                        if (dbcon.State != System.Data.ConnectionState.Open)
                            dbcon.Open();

                        //string[] tblrestrictions = new string[] {null, null, null, "TABLE"};
                        //System.Data.DataTable dt1 = dbcon.GetSchema("tables", tblrestrictions);
                        // Note:
                        // Restrictions are a string array in the following format: 
                        // {TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE}.

                        // http://forums.asp.net/t/976267.aspx/1?Record+s+cannot+be+read+no+read+permission+on+MSysObjects+
                        // http://www.devart.com/dotconnect/mysql/docs/MetaData.html
                        // http://msdn.microsoft.com/en-us/library/ms254934(v=vs.80).aspx
                        dt = dbcon.GetSchema("procedures");
                        //dt = dbcon.GetSchema("columns");
                        //dt = dbcon.GetSchema("restrictions");
                        //dt = dbcon.GetSchema("DataTypes");
                        //dt = dbcon.GetSchema("Indexes");
                        //dt = dbcon.GetSchema("ReservedWords");
                        //dt = dbcon.GetSchema("DataSourceInformation");
                    }
                    catch (System.Exception ex)
                    {
                        if (Log("cOleDB_schema.GetProcedures", ex, "dbcon.GetSchema(\"views\")"))
                            throw;
                    }
                    finally
                    {
                        if (dbcon.State != System.Data.ConnectionState.Closed)
                            dbcon.Close();
                    }
                } // End Lock dbcon


            } // End Using dbcon

            return dt;
        } // End Function GetProcedures


        public override System.Data.DataTable GetFunctions()
        {
            throw new System.NotImplementedException("cOleDB_schema.GetFunctions not implemented.");
        } // End Function GetFunctions


        public override System.Data.DataTable GetColumnNames()
        {
            System.Data.DataTable dt = null;


            using (System.Data.OleDb.OleDbConnection dbcon = new System.Data.OleDb.OleDbConnection(this.m_ConnectionString.ConnectionString))
            {
                lock (dbcon)
                {
                    try
                    {
                        if (dbcon.State != System.Data.ConnectionState.Open)
                            dbcon.Open();

                        //string[] tblrestrictions = new string[] {null, null, null, "TABLE"};
                        //System.Data.DataTable dt1 = dbcon.GetSchema("tables", tblrestrictions);
                        // Note:
                        // Restrictions are a string array in the following format: 
                        // {TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE}.

                        // http://forums.asp.net/t/976267.aspx/1?Record+s+cannot+be+read+no+read+permission+on+MSysObjects+
                        // http://www.devart.com/dotconnect/mysql/docs/MetaData.html
                        // http://msdn.microsoft.com/en-us/library/ms254934(v=vs.80).aspx
                        dt = dbcon.GetSchema("columns");
                        //dt = dbcon.GetSchema("restrictions");
                        //dt = dbcon.GetSchema("DataTypes");
                        //dt = dbcon.GetSchema("Indexes");
                        //dt = dbcon.GetSchema("ReservedWords");
                        //dt = dbcon.GetSchema("DataSourceInformation");
                    }
                    catch (System.Exception ex)
                    {
                        if (Log("cOleDB_schema.GetColumns", ex, "dbcon.GetSchema(\"views\")"))
                            throw;
                    }
                    finally
                    {
                        if (dbcon.State != System.Data.ConnectionState.Closed)
                            dbcon.Close();
                    }
                } // End Lock dbcon


            } // End Using dbcon

            return dt;
        } // End Function GetColumnNames


        public override System.Data.DataTable GetColumnNamesForTable(string strTableName)
        {
            strTableName = strTableName.Replace("'", "''");
            System.Data.DataTable dtSchemaTable = null;

            using (System.Data.IDataReader idr = ExecuteReader("SELECT * FROM " + strTableName) )
            {
                dtSchemaTable = idr.GetSchemaTable();
                idr.Close();
            }

            return dtSchemaTable;
        } // End Function GetColumnNamesForTable


        public override bool TableExists(string strTableName)
        {
            strTableName = strTableName.Replace("'","''");
            bool bReturnValue = false;

            using (System.Data.OleDb.OleDbConnection dbcon = new System.Data.OleDb.OleDbConnection(this.m_ConnectionString.ConnectionString))
            {
                lock(dbcon)
                {
                    try
                    {
                        if (dbcon.State != System.Data.ConnectionState.Open)
                            dbcon.Open();

                            //string[] tblrestrictions = new string[] {null, null, null, "TABLE"};
                            //System.Data.DataTable dt1 = dbcon.GetSchema("tables", tblrestrictions);
                            // Note:
                            // Restrictions are a string array in the following format: 
                            // {TABLE_CATALOG, TABLE_SCHEMA, TABLE_NAME, TABLE_TYPE}.


                        // http://forums.asp.net/t/976267.aspx/1?Record+s+cannot+be+read+no+read+permission+on+MSysObjects+
                        using(System.Data.DataTable dt = dbcon.GetSchema("tables"))
                        {
                            int i = dt.Select("TABLE_NAME like '" + strTableName + "'").Length;
                            if(i > 0)
                                bReturnValue = true;
                            dt.Clear();
                            dt.Dispose();
                            
                            /*
                            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();
                            foreach(System.Data.DataColumn dc in dt.Columns)
                            {
                                ls.Add(dc.ColumnName);
                            }
                            MsgBox(string.Join(" ", ls.ToArray()));
                            */

                        }
                        
                    }
                    catch (System.Exception ex)
                    {
                        if (Log("cOleDB_schema.TableExists", ex, "GetSchema(\"ExportTables\")"))
                            throw;
                    }
                    finally
                    {
                        if (dbcon.State != System.Data.ConnectionState.Closed)
                            dbcon.Close();
                    }
                } // End Lock dbcon
                

            } // End Using dbcon
           
            /*
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
             */
            return bReturnValue;
        } // End Function TableExists


        public override void DropTable(string strTableName)
        {
            if (TableExists(strTableName))
            {
                string strSQL = @"DROP TABLE " + strTableName + " ";
                Execute(strSQL);
            }

        } // End Function TableExists


        public override bool IsTableEmpty(string strTableName)
        {
            string strSQL = "SELECT COUNT(*) FROM [" + strTableName.Replace("'", "''") + "]";

            return !ExecuteScalar<bool>(strSQL);
        } // End Function IsTableEmpty


        public override bool TableHasColumn(string strTableName, string strColumnName)
        {
            System.Data.DataTable dt = GetColumnNamesForTable(strTableName);
            int i = dt.Select("ColumnName like '" + strColumnName + "'").Length;
            dt.Clear();
            dt.Dispose();

            if (i > 0)
                return true;

            return false;
        } // End Function TableHasColumn


        public override void ExportTables()
        {
            System.Data.DataTable dt = GetTables();

            foreach (System.Data.DataRow dr in dt.Rows)
            {
                GetDataTableAndWriteXML(dr["table_name"].ToString());
            } // Next dr
        } // End Sub ExportTables


        // select * from INFORMATION_SCHEMA.VIEW_TABLE_USAGE
        // select * from INFORMATION_SCHEMA.VIEW_COLUMN_USAGE
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
            }
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
WITH TablesCTE(SchemaName, TableName, TableID, Ordinal) AS
(
    SELECT
        OBJECT_SCHEMA_NAME(so.object_id) AS SchemaName,
        OBJECT_NAME(so.object_id) AS TableName,
        so.object_id AS TableID,
        0 AS Ordinal
    FROM sys.objects AS so
    WHERE
        so.type = 'U'
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
            cDAL DAL = new cOleDB();
            DAL.Execute("SELECT * FROM T_Benutzer");
            System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
        } // End Sub Test


    } // End Class cOleDB


} // End Namespace DataBase.Tools

#endif
