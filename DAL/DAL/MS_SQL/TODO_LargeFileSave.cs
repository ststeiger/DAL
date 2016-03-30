
namespace DB.Abstraction
{


    internal class SQL
    {
        public static System.Data.Common.DbCommand CreateCommand()
        {
            return null;
        }

        public static System.Data.Common.DbCommand CreateCommand(string SQL)
        {
            return null;
        }

        public static System.Data.Common.DbCommand CreateCommand(string SQL, int timeout)
        {
            return null;
        }

        public static string GetConnectionString()
        {
            return null;
        }


        public static System.Data.Common.DbConnection GetConnection()
        {
            return null;
        }

        public static int ExecuteNonQuery(string SQL)
        {
            return 0;
        }

        public static int ExecuteNonQuery(System.Data.IDbCommand cmd)
        {
            return 0;
        }

        public static System.Data.Common.DbDataReader ExecuteReader(System.Data.IDbCommand cmd)
        {
            return null;
        }


        public static System.Data.Common.DbParameter AddParameter(System.Data.IDbCommand cmd, string paramName, object value)
        {
            // System.Data.IDbDataParameter
            return null;
        }

        public static System.Data.Common.DbParameter AddParameter(System.Data.IDbCommand cmd, string paramName, object value, System.Data.ParameterDirection dir, System.Data.DbType typ)
        {
            // System.Data.IDbDataParameter
            return null;
        }


    }


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
