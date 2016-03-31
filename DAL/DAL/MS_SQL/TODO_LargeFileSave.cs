
namespace DB.Abstraction
{

    internal class FileSaveExample
    {

        public static void SaveFile(string fileName)
        {
            string strSQL = @"
INSERT INTO T_AP_Dokumente 
(
	 DK_UID
	,DK_Objekt_UID
	,DK_DKAT_UID
	,DK_Bezeichnung
	,DK_Datei
	,DK_Dateiformat
	,DK_Status
	,DK_IsUpload
	,DK_IsDefault
	,DK_File
	,DK_BE_ID
	,DK_Mut_Date
	,DK_DK_UID
) 
SELECT
	 NEWID() AS DK_UID -- uniqueidentifier
	,@__DK_Objekt_UID AS DK_Objekt_UID -- uniqueidentifier
	,(SELECT TOP 1 DKAT_UID FROM T_AP_Ref_DokumentKategorie WHERE DKAT_Lang_DE = 'Kunstbild') AS DK_DKAT_UID -- uniqueidentifier
	,@__DK_Bezeichnung AS DK_Bezeichnung -- varchar(50)
	,@__DK_Datei AS DK_Datei -- varchar(255)
	,'.png' AS DK_Dateiformat -- varchar(25)
	,666 AS DK_Status -- int
	,1 AS DK_IsUpload -- bit
	,0 AS DK_IsDefault -- bit
	,@__file AS DK_File -- varbinary(max)
	,(SELECT TOP 1 BE_ID FROM T_Benutzer WHERE BE_User = 'administrator') AS DK_BE_ID -- int
	,CURRENT_TIMESTAMP AS DK_Mut_Date -- datetime
	,NULL AS DK_DK_UID -- uniqueidentifier
;

";
            byte[] ba = System.IO.File.ReadAllBytes(@"D:\Stefan.Steiger\Pictures\Physical_world.svg");




            using (System.Data.IDbCommand cmd = SQL.CreateCommand(strSQL))
            {
                SQL.AddParameter(cmd, "__DK_Objekt_UID", "66666666-6666-6666-6666-666666666667");
                SQL.AddParameter(cmd, "__DK_Bezeichnung", "Test1");
                SQL.AddParameter(cmd, "__DK_Datei", "Test2");

                // SQL.SaveFile(fileName, cmd);
                SQL.SaveFile(ba, cmd);
            } // End Using cmd

            strSQL = @"
SELECT * 
  -- DELETE 
FROM T_AP_Dokumente 
WHERE DK_Status = 666   
";


            SQL.RetrieveFile(strSQL, @"D:\testnew.svg", "DK_File");
        } // End Sub SaveFile
    }



    internal class SQL
    {


        public static bool Log(System.Exception ex)
        {
            return Log(ex, null);
        } // End Function Log 


        public static bool Log(System.Exception ex, System.Data.IDbCommand cmd)
        {
            return Log(null, ex, cmd);
        } // End Function Log 


        public static bool Log(string location, System.Exception ex, System.Data.IDbCommand cmd)
        {
            if (location != null)
                Notify(location);

            Notify(ex.Message);

            if (cmd != null)
                Notify(cmd.CommandText);

            return true;
        } // End Function Log 


        public static void Notify(object obj)
        {
            string text = "NULL";
            if (obj != null)
                text = obj.ToString();
            System.Console.WriteLine(text);

            string caption = System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetExecutingAssembly().Location);
            System.Windows.Forms.MessageBox.Show(text, caption, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
        } // End Function Notify 


        public static string GetConnectionString()
        {
            System.Data.SqlClient.SqlConnectionStringBuilder csb = new System.Data.SqlClient.SqlConnectionStringBuilder();
            csb.IntegratedSecurity = true;

            if (!csb.IntegratedSecurity)
            {
                csb.UserID = "ArtImportWebServices";
                csb.Password = "TOP_SECRET";
            }

            csb.DataSource = System.Environment.MachineName;
            csb.InitialCatalog = "COR_Basic";

            return csb.ConnectionString;
        } // End Function GetConnectionString 


        public static System.Data.Common.DbConnection GetConnection()
        {
            return new System.Data.SqlClient.SqlConnection(GetConnectionString());
        } // End Function GetConnection 


        public static System.Data.Common.DbCommand CreateCommand()
        {
            return CreateCommand(null);
        } // End Function CreateCommand


        public static System.Data.Common.DbCommand CreateCommand(string SQL)
        {
            return CreateCommand(SQL, 30);
        } // End Function CreateCommand


        private static byte[] ReadAllBytesShareRead(string fileName)
        {
            byte[] file;

            using (System.IO.FileStream stream = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {

                using (System.IO.BinaryReader reader = new System.IO.BinaryReader(stream))
                {
                    file = reader.ReadBytes((int)stream.Length);
                } // End Using reader

            } // End Using stream 

            return file;
        }


        public static System.Data.Common.DbCommand CreateCommand(string SQL, int timeout)
        {
            System.Data.Common.DbCommand cmd = new System.Data.SqlClient.SqlCommand(SQL);
            cmd.CommandTimeout = timeout;

            return cmd;
        } // End Function CreateCommand


        public static void SaveFile(string fileName, string SQL)
        {
            SaveFile(fileName, SQL, null);
        } // End Sub SaveFile


        // https://stackoverflow.com/questions/2579373/saving-any-file-to-in-the-database-just-convert-it-to-a-byte-array
        public static void SaveFile(string fileName, string SQL, string paramName)
        {
            byte[] file = ReadAllBytesShareRead(fileName);
            SaveFile(file, SQL, paramName);
        } // End Sub SaveFile



        public static void SaveFile(byte[] file, string SQL)
        {
            SaveFile(file, SQL, null);
        } // End Sub SaveFile


        public static void SaveFile(byte[] file, string SQL, string paramName)
        {
            using (System.Data.Common.DbCommand cmd = CreateCommand(SQL))
            {
                SaveFile(file, cmd, paramName);
            } // End Using cmd 

        } // End Sub SaveFile



        public static void SaveFile(string fileName, System.Data.IDbCommand cmd)
        {
            SaveFile(fileName, cmd, null);
        }


        public static void SaveFile(string fileName, System.Data.IDbCommand cmd, string paramName)
        {
            byte[] file = ReadAllBytesShareRead(fileName);
            SaveFile(file, cmd, paramName);
        } // End Sub SaveFile


        public static void SaveFile(byte[] file, System.Data.IDbCommand cmd)
        {
            SaveFile(file, cmd, null);
        }


        public static void SaveFile(byte[] file, System.Data.IDbCommand cmd, string paramName)
        {
            if (string.IsNullOrEmpty(paramName))
                paramName = "__file";

            if (!paramName.StartsWith("@"))
                paramName = "@" + paramName;

            if (!cmd.Parameters.Contains(paramName))
                AddParameter(cmd, paramName, file);

            ExecuteNonQuery(cmd);
        } // End Sub SaveFile




        // http://stackoverflow.com/questions/2885335/clr-sql-assembly-get-the-bytestream
        // http://stackoverflow.com/questions/891617/how-to-read-a-image-by-idatareader
        // http://stackoverflow.com/questions/4103406/extracting-a-net-assembly-from-sql-server-2005
        public static void RetrieveFile(string sql, string path)
        {
            RetrieveFile(sql, path, "data");
        } // End Sub RetrieveFile 


        public static void RetrieveFile(string sql, string path, string columnName)
        {

            using (System.Data.IDbCommand cmd = CreateCommand(sql, 0))
            {
                RetrieveFile(cmd, columnName, path);
            } // End Using cmd 

        } // End Sub RetrieveFile 


        public static void RetrieveFile(System.Data.IDbCommand cmd, string path)
        {
            RetrieveFile(cmd, null, path);
        } // End Sub RetrieveFile 


        // http://stackoverflow.com/questions/2885335/clr-sql-assembly-get-the-bytestream
        // http://stackoverflow.com/questions/891617/how-to-read-a-image-by-idatareader
        // http://stackoverflow.com/questions/4103406/extracting-a-net-assembly-from-sql-server-2005
        public static void RetrieveFile(System.Data.IDbCommand cmd, string columnName, string path)
        {
            using (System.Data.IDataReader reader = ExecuteReader(cmd, System.Data.CommandBehavior.SequentialAccess | System.Data.CommandBehavior.CloseConnection))
            {
                bool hasRows = reader.Read();
                if (hasRows)
                {
                    const int BUFFER_SIZE = 1024 * 1024 * 10; // 10 MB
                    byte[] buffer = new byte[BUFFER_SIZE];

                    int col = string.IsNullOrEmpty(columnName) ? 0 : reader.GetOrdinal(columnName);
                    int bytesRead = 0;
                    int offset = 0;

                    // Write the byte stream out to disk
                    using (System.IO.FileStream bytestream = new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None))
                    {
                        while ((bytesRead = (int)reader.GetBytes(col, offset, buffer, 0, BUFFER_SIZE)) > 0)
                        {
                            bytestream.Write(buffer, 0, bytesRead);
                            offset += bytesRead;
                        } // Whend

                        bytestream.Close();
                    } // End Using bytestream 

                } // End if (!hasRows)

                reader.Close();
            } // End Using reader

        } // End Function RetrieveFile


        public static int ExecuteNonQuery(string SQL)
        {
            int iAffected = 0;
            using (System.Data.Common.DbCommand cmd = CreateCommand(SQL))
            {
                iAffected = ExecuteNonQuery(cmd);
            } // End Using cmd 

            return iAffected;
        } // End Function ExecuteNonQuery 


        public static int ExecuteNonQuery(System.Data.IDbCommand cmd)
        {
            int iAffected = -1;

            try
            {

                using (System.Data.IDbConnection idbConn = GetConnection())
                {

                    lock (idbConn)
                    {

                        lock (cmd)
                        {

                            cmd.Connection = idbConn;

                            if (cmd.Connection.State != System.Data.ConnectionState.Open)
                                cmd.Connection.Open();

                            using (System.Data.IDbTransaction idbtTrans = idbConn.BeginTransaction())
                            {

                                try
                                {
                                    cmd.Transaction = idbtTrans;

                                    iAffected = cmd.ExecuteNonQuery();
                                    idbtTrans.Commit();
                                } // End Try
                                catch (System.Data.Common.DbException ex)
                                {
                                    if (idbtTrans != null)
                                        idbtTrans.Rollback();

                                    iAffected = -2;

                                    if (Log(ex))
                                        throw;
                                } // End catch
                                finally
                                {
                                    if (cmd.Connection.State != System.Data.ConnectionState.Closed)
                                        cmd.Connection.Close();
                                } // End Finally

                            } // End Using idbtTrans

                        } // End lock cmd

                    } // End lock idbConn

                } // End Using idbConn

            } // End Try
            catch (System.Exception ex)
            {
                iAffected = -3;
                if (Log(ex, cmd))
                    throw;
            }
            finally
            {

            }

            return iAffected;
        } // End Function ExecuteNonQuery 


        public static System.Data.IDataReader ExecuteReader(System.Data.IDbCommand cmd)
        {
            return ExecuteReader(cmd, System.Data.CommandBehavior.CloseConnection);
        }


        public static System.Data.IDataReader ExecuteReader(System.Data.IDbCommand cmd, System.Data.CommandBehavior behav)
        {
            System.Data.IDataReader idr = null;

            lock (cmd)
            {
                System.Data.IDbConnection idbc = GetConnection();
                cmd.Connection = idbc;

                if (cmd.Connection.State != System.Data.ConnectionState.Open)
                    cmd.Connection.Open();

                try
                {
                    idr = cmd.ExecuteReader(behav);
                }
                catch (System.Exception ex)
                {
                    if (Log(ex, cmd))
                        throw;
                }
            } // End Lock cmd

            return idr;
        } // End Function ExecuteReader


        public virtual System.Data.DataTable GetDataTable(string strSQL)
        {
            System.Data.DataTable dt = null;

            using (System.Data.IDbCommand cmd = CreateCommand(strSQL))
            {
                dt = GetDataTable(cmd);
            } // End Using cmd

            return dt;
        } // End Function GetDataTable


        public virtual System.Data.DataTable GetDataTable(System.Data.IDbCommand cmd)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            using (System.Data.IDbConnection idbc = GetConnection())
            {

                lock (idbc)
                {

                    lock (cmd)
                    {

                        try
                        {
                            cmd.Connection = idbc;

                            using (System.Data.Common.DbDataAdapter daQueryTable = new System.Data.SqlClient.SqlDataAdapter())
                            {
                                daQueryTable.SelectCommand = (System.Data.Common.DbCommand)cmd;
                                daQueryTable.Fill(dt);
                            } // End Using daQueryTable

                        } // End Try
                        catch (System.Data.Common.DbException ex)
                        {
                            if (Log("SQL.GetDataTable(System.Data.IDbCommand cmd)", ex, cmd))
                                throw;
                        }// End Catch
                        finally
                        {
                            if (idbc.State != System.Data.ConnectionState.Closed)
                                idbc.Close();
                        } // End Finally

                    } // End lock cmd

                } // End lock idbc

            } // End Using idbc

            return dt;
        } // End Function GetDataTable


        // From Type to DBType
        protected static System.Data.DbType GetDbType(System.Type type)
        {
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/c6f3ab91-2198-402a-9a18-66ce442333a6
            string strTypeName = type.Name;
            System.Data.DbType DBtype = System.Data.DbType.String; // default value

            try
            {
                if (object.ReferenceEquals(type, typeof(System.DBNull)))
                {
                    return DBtype;
                }

                if (object.ReferenceEquals(type, typeof(System.Byte[])))
                {
                    return System.Data.DbType.Binary;
                }

                DBtype = (System.Data.DbType)System.Enum.Parse(typeof(System.Data.DbType), strTypeName, true);

                // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
                // http://msdn.microsoft.com/en-us/library/bbw6zyha(v=vs.71).aspx
                if (DBtype == System.Data.DbType.UInt64)
                    DBtype = System.Data.DbType.Int64;
            }
            catch (System.Exception)
            {
                // add error handling to suit your taste
            }

            return DBtype;
        } // End Function GetDbType


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue)
        {
            return AddParameter(command, strParameterName, objValue, System.Data.ParameterDirection.Input);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad)
        {
            if (objValue == null)
            {
                //throw new ArgumentNullException("objValue");
                objValue = System.DBNull.Value;
            } // End if (objValue == null)

            System.Type tDataType = objValue.GetType();
            System.Data.DbType dbType = GetDbType(tDataType);

            return AddParameter(command, strParameterName, objValue, pad, dbType);
        } // End Function AddParameter


        public static System.Data.IDbDataParameter AddParameter(System.Data.IDbCommand command, string strParameterName, object objValue, System.Data.ParameterDirection pad, System.Data.DbType dbType)
        {
            System.Data.IDbDataParameter parameter = command.CreateParameter();

            if (!strParameterName.StartsWith("@"))
            {
                strParameterName = "@" + strParameterName;
            } // End if (!strParameterName.StartsWith("@"))

            parameter.ParameterName = strParameterName;
            parameter.DbType = dbType;
            parameter.Direction = pad;

            // Es ist keine Zuordnung von DbType UInt64 zu einem bekannten SqlDbType vorhanden.
            // No association  DbType UInt64 to a known SqlDbType

            if (objValue == null)
                parameter.Value = System.DBNull.Value;
            else
                parameter.Value = objValue;

            command.Parameters.Add(parameter);
            return parameter;
        } // End Function AddParameter


    } // End Internal Class SQL




    // https://blog.tallan.com/2011/08/22/using-sqlfilestream-with-c-to-access-sql-server-filestream-data/
    // https://designingefficientsoftware.wordpress.com/2011/03/03/efficient-file-io-from-csharp/
    // http://www.codeproject.com/Articles/128657/How-Do-I-Use-SQL-File-Stream
    public class LargeFileSafe
    {


        public static void TestSimpleInsert(string strFile)
        {
            System.DateTime dtStartSimpleInsert = System.DateTime.Now;
            SimpleInsert(strFile);
            System.DateTime dtEndSimpleInsert = System.DateTime.Now;
            System.TimeSpan tsSimpleInsert = dtEndSimpleInsert.Subtract(dtStartSimpleInsert);
            System.Console.WriteLine(tsSimpleInsert.TotalSeconds);
        } // End Sub TestSimpleInsert 


        //tsSimpleInsert = {00:05:27.8863909}
        //tsSimpleInsert.TotalSeconds = 327.8863909


        public static void TestChunkInsert(string strFile)
        {
            string strSQL = @"
CREATE TABLE dbo._____save
(
     uid uniqueidentifier NULL 
    ,data varbinary(max) NULL 
    ,filename nvarchar(255) NULL 
) ; 
";
            SQL.ExecuteNonQuery(strSQL);


            System.DateTime dtStartSave = System.DateTime.Now;
            InitLargeFileSave(strFile);
            System.DateTime dtEndSave = System.DateTime.Now;
            RetrieveFile(strFile, @"d:\test.comp");
            System.DateTime dtEndRetrieve = System.DateTime.Now;

            System.TimeSpan tsSave = dtEndSave.Subtract(dtStartSave);
            System.TimeSpan tsRetrieve = dtEndRetrieve.Subtract(dtEndSave);

            System.Console.WriteLine(tsSave); //tsSave = {00:01:27.6114341}
            System.Console.WriteLine(tsRetrieve); //tsRetrieve = {00:01:35.8800561}

            System.Console.WriteLine(tsSave.TotalSeconds); //tsSave.TotalSeconds = 87.6114341
            System.Console.WriteLine(tsRetrieve.TotalSeconds); //tsRetrieve.TotalSeconds = 95.88005609999999
        } // End Sub TestChunkInsert 


        public static void SimpleInsert(string fileName)
        {
            string strSQL = @"
INSERT INTO _____save 
(
	 [uid]
	,[data]
	,[filename]
)
VALUES
(
	 NEWID() --uid, uniqueidentifier
	,@data -- varbinary(max)
	,@filename -- nvarchar(255)
)
;
";

            byte[] ba = System.IO.File.ReadAllBytes(fileName);

            using (System.Data.IDbCommand cmd = SQL.CreateCommand(strSQL, 0))
            {
                SQL.AddParameter(cmd, "data", ba);
                SQL.AddParameter(cmd, "filename", fileName);

                SQL.ExecuteNonQuery(cmd);
            } // End Using cmd 

        } // End Sub SimpleInsert 




        // http://stackoverflow.com/questions/2885335/clr-sql-assembly-get-the-bytestream
        // http://stackoverflow.com/questions/891617/how-to-read-a-image-by-idatareader
        // http://stackoverflow.com/questions/4103406/extracting-a-net-assembly-from-sql-server-2005
        public static void RetrieveFile(string fileName, string path)
        {
            string sql = @"
--DECLARE @__filename nvarchar(255) 
--SET @__filename = 'lkik' 

SELECT 
	 uid
	,data
	,filename
FROM _____save
WHERE filename = @__filename 
";


            using (System.Data.IDbCommand cmd = SQL.CreateCommand(sql, 0))
            {
                SQL.AddParameter(cmd, "__filename", fileName);

                using (System.Data.IDataReader reader = SQL.ExecuteReader(cmd))
                {
                    bool hasRows = reader.Read();
                    if (hasRows)
                    {
                        const int BUFFER_SIZE = 1024 * 1024 * 10; // 10 megs
                        byte[] buffer = new byte[BUFFER_SIZE];

                        int col = reader.GetOrdinal("data");
                        int bytesRead = 0;
                        int offset = 0;

                        // write the byte stream out to disk
                        //using (System.IO.FileStream bytestream = new System.IO.FileStream(path, System.IO.FileMode.CreateNew))
                        using (System.IO.FileStream bytestream = new System.IO.FileStream(path, System.IO.FileMode.Create))
                        {
                            // SqlBytes bytes = reader.GetSqlBytes(0);
                            while ((bytesRead = (int)reader.GetBytes(col, offset, buffer, 0, BUFFER_SIZE)) > 0)
                            {
                                bytestream.Write(buffer, 0, bytesRead);
                                offset += bytesRead;
                            } // Whend

                            bytestream.Close();
                        } // End Using bytestream 

                    } // End if (!hasRows)

                    reader.Close();
                } // End Using reader

            } // End Using cmd

        } // End Function RetrieveFile


        public static void InitLargeFileSave(string fileName)
        {
            string strSQL = @"
INSERT INTO _____save 
(
	 [uid]
	,[data]
	,[filename]
)
VALUES
(
	 NEWID() -- uid, uniqueidentifier
	,@data -- data, varbinary(max)
	,@fileName  -- nvarchar(255)
    --,N'D:\Temp\SQL\20140911\COR_Basic_20140617.BAK' -- nvarchar(255)
);
";

            string constr = SQL.GetConnectionString();
            const int BUFFER_SIZE = 1024 * 1024 * 10; // 10 megs
            byte[] buffer = new byte[BUFFER_SIZE];


            using (System.Data.IDbConnection con = SQL.GetConnection())
            {
                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();

                using (System.Data.IDbTransaction trn = con.BeginTransaction())
                {
                    //using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(strSQL, con))

                    using (System.Data.IDbCommand cmd = SQL.CreateCommand(strSQL))
                    {
                        cmd.Connection = con;
                        cmd.Transaction = trn;

                        System.Data.IDbDataParameter dataParam = SQL.AddParameter(cmd, "data", null, System.Data.ParameterDirection.Input, System.Data.DbType.Binary);
                        System.Data.IDbDataParameter lengthParam = SQL.AddParameter(cmd, "length", 0);
                        SQL.AddParameter(cmd, "fileName", fileName);

                        //System.Data.IDbDataParameter dataParam = cmd.CreateParameter();
                        //dataParam.ParameterName = "@data";
                        //dataParam.DbType = System.Data.DbType.Binary;
                        //cmd.Parameters.Add(dataParam);

                        //System.Data.IDbDataParameter lengthParam = cmd.CreateParameter();
                        //lengthParam.ParameterName = "@length";
                        //lengthParam.DbType = System.Data.DbType.Int32;
                        //cmd.Parameters.Add(lengthParam);

                        //System.Data.IDbDataParameter fileNameParam = cmd.CreateParameter();
                        //fileNameParam.ParameterName = "@fileName";
                        //fileNameParam.DbType = System.Data.DbType.String;
                        //fileNameParam.Value = fileName;
                        //cmd.Parameters.Add(fileNameParam);




                        // System.Data.SqlClient.SqlParameter dataParam = cmd.Parameters.Add("@data", System.Data.SqlDbType.VarBinary);
                        // System.Data.SqlClient.SqlParameter lengthParam = cmd.Parameters.Add("@length", System.Data.SqlDbType.Int);
                        // System.Data.SqlClient.SqlParameter fileNameParam = cmd.Parameters.Add("@fileName", System.Data.SqlDbType.NVarChar);




                        using (System.IO.Stream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                        {
                            int readBytes = 0;
                            long fileSize = fs.Length;
                            long cIndex = 0;

                            while (cIndex < fileSize)
                            {
                                if (cIndex + BUFFER_SIZE > fileSize)
                                    readBytes = (int)(fileSize - cIndex);
                                else
                                    readBytes = BUFFER_SIZE;


                                fs.Read(buffer, 0, readBytes);

                                if (cIndex == 0)
                                    cmd.CommandText = strSQL; // Insert
                                else // Update
                                    cmd.CommandText = "UPDATE _____save SET Data.Write(@data, NULL, NULL) WHERE fileName = @fileName";

                                dataParam.Value = buffer;
                                dataParam.Size = readBytes;
                                lengthParam.Value = readBytes;

                                cmd.ExecuteNonQuery();
                                cIndex += BUFFER_SIZE;
                            } // Whend

                        } // End Using fs 

                    } // End Using cmd 

                    trn.Commit();
                } // End Using trn

                if (con.State != System.Data.ConnectionState.Closed)
                    con.Close();
            } // End Using con

        } // End Sub InitLargeFileSave 


        // http://stackoverflow.com/questions/9200675/blob-files-in-sql-database-as-chunk
        public static void SqlServerLargeFileChuncked_tooSlow(string fileName)
        {
            string sql = "UPDATE _____save SET Data.Write(@data, LEN(data), @length) WHERE fileName = @fileName";
            byte[] buffer = new byte[1024 * 1024 * 10];

            string constr = SQL.GetConnectionString();

            using (System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(constr))
            using (System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(sql, con))
            {
                cmd.CommandText = sql;

                System.Data.SqlClient.SqlParameter dataParam = cmd.Parameters.Add("@data", System.Data.SqlDbType.VarBinary);
                System.Data.SqlClient.SqlParameter lengthParam = cmd.Parameters.Add("@length", System.Data.SqlDbType.Int);
                System.Data.SqlClient.SqlParameter fileNameParam = cmd.Parameters.Add("@fileName", System.Data.SqlDbType.NVarChar);

                fileNameParam.Value = fileName;


                if (con.State != System.Data.ConnectionState.Open)
                    con.Open();


                using (System.IO.Stream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    int readBytes = 0;
                    long fileSize = fs.Length;
                    long cIndex = 0;

                    while (cIndex < fileSize)
                    {
                        if (cIndex + buffer.Length > fileSize)
                            readBytes = (int)(fileSize - cIndex);
                        else
                            readBytes = buffer.Length;



                        fs.Read(buffer, 0, readBytes);

                        dataParam.Value = buffer;
                        dataParam.Size = readBytes;
                        lengthParam.Value = readBytes;

                        cmd.ExecuteNonQuery();
                        cIndex += buffer.Length;
                    } // Whend

                } // End Using fs 

            } // End Using cmd

        } // End Sub SqlServerLargeFileChuncked


    } // End Class LargeFileSafe


} // End Namespace DB.Abstraction
