
namespace DB.Abstraction
{


    partial class cPostGreSQL : cDAL
    {
        protected Npgsql.NpgsqlConnection m_SqlConnection;
        protected Npgsql.NpgsqlConnectionStringBuilder m_ConnectionString;


        public override bool IsPostGreSql
        {
            get { return true; }
        }


        public cPostGreSQL()
            : this("")
        {
            // Crap !
        }


        public cPostGreSQL(string strConnectionString)
        {
            //this.m_DatabaseConfiguration = dbcDBconfig;
            this.m_dbtDBtype = DataBaseEngine_t.PostGreSQL;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 8.5;
            this.m_ConnectionString = new Npgsql.NpgsqlConnectionStringBuilder(strConnectionString);

            this.m_SqlConnection = new Npgsql.NpgsqlConnection(strConnectionString);
        } // End Constructor 2


        public void DeleteLargeObject(int noid)
        {
            using (Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(GetConnectionString()))
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                using (Npgsql.NpgsqlTransaction trans = connection.BeginTransaction())
                {
                    NpgsqlTypes.LargeObjectManager lbm = new NpgsqlTypes.LargeObjectManager(connection);
                    lbm.Delete(noid);

                    trans.Commit();

                    if (connection.State != System.Data.ConnectionState.Closed)
                        connection.Close();
                } // End Using trans 

            } // End Using connection

        } // End Sub DeleteLargeObject 


        //it's a method that connects to a database and return the picture oid
        public int takeOID(int idOfOID)
        {
            string pytanko = string.Format("SELECT drawing FROM T_Drawings WHERE idOfOID = " + idOfOID.ToString());
            string[] wartosci = new string[] { }; // pyro.OddajSelectArray(pytanko);
            int number = int.Parse(wartosci[0].ToString());
            return number;
        }


        // pobierz: download
        // Rysunek: drawing
        //take a picture from database and convert to Image type
        public System.Drawing.Image GetDrawing(int idOfOID)
        {
            System.Drawing.Image img;

            using (Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(GetConnectionString()))
            {
                if (connection.State != System.Data.ConnectionState.Open)
                    connection.Open();

                using (Npgsql.NpgsqlTransaction trans = connection.BeginTransaction())
                {
                    NpgsqlTypes.LargeObjectManager lbm = new NpgsqlTypes.LargeObjectManager(connection);
                    NpgsqlTypes.LargeObject lo = lbm.Open(takeOID(idOfOID), NpgsqlTypes.LargeObjectManager.READWRITE); //take picture oid from metod takeOID
                    byte[] buf = new byte[lo.Size()];
                    buf = lo.Read(lo.Size());
                    using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                    {
                        ms.Write(buf, 0, lo.Size());
                        img = System.Drawing.Image.FromStream(ms);
                    } // End Using ms

                    lo.Close();
                    trans.Commit();

                    if (connection.State != System.Data.ConnectionState.Closed)
                        connection.Close();
                } // End Using trans

            } // End Using connection

            return img;
        } // End Function GetDrawing


        public System.Drawing.Image GetLargeDrawing(int idOfOID)
        {
            System.Drawing.Image img;

            using (Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(GetConnectionString()))
            {
                lock (connection)
                {
                    if (connection.State != System.Data.ConnectionState.Open)
                        connection.Open();

                    using (Npgsql.NpgsqlTransaction trans = connection.BeginTransaction())
                    {
                        NpgsqlTypes.LargeObjectManager lbm = new NpgsqlTypes.LargeObjectManager(connection);
                        NpgsqlTypes.LargeObject lo = lbm.Open(takeOID(idOfOID), NpgsqlTypes.LargeObjectManager.READWRITE); //take picture oid from metod takeOID
                        byte[] buffer = new byte[32768];

                        using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                        {
                            int read;
                            while ((read = lo.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                ms.Write(buffer, 0, read);
                            } // Whend

                            img = System.Drawing.Image.FromStream(ms);
                        } // End Using ms

                        lo.Close();
                        trans.Commit();

                        if (connection.State != System.Data.ConnectionState.Closed)
                            connection.Close();
                    } // End Using trans

                } // End lock connection

            } // End Using connection

            return img;
        } // End Function GetLargeDrawing


        // http://stackoverflow.com/questions/14509747/inserting-large-object-into-postgresql-returns-53200-out-of-memory-error
        // https://github.com/npgsql/Npgsql/wiki/User-Manual
        public int InsertLargeObject()
        {
            int noid;
            byte[] BinaryData = new byte[123];

            // Npgsql.NpgsqlCommand cmd ;
            // long lng = cmd.LastInsertedOID;

            using (Npgsql.NpgsqlConnection connection = new Npgsql.NpgsqlConnection(GetConnectionString()))
            {
                using (Npgsql.NpgsqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        NpgsqlTypes.LargeObjectManager manager = new NpgsqlTypes.LargeObjectManager(connection);
                        noid = manager.Create(NpgsqlTypes.LargeObjectManager.READWRITE);
                        NpgsqlTypes.LargeObject lo = manager.Open(noid, NpgsqlTypes.LargeObjectManager.READWRITE);

                        // lo.Write(BinaryData);
                        int i = 0;
                        do
                        {
                            int length = 1000;
                            if (i + length > BinaryData.Length)
                                length = BinaryData.Length - i;

                            byte[] chunk = new byte[length];
                            System.Array.Copy(BinaryData, i, chunk, 0, length);
                            lo.Write(chunk, 0, length);
                            i += length;
                        } while (i < BinaryData.Length);

                        lo.Close();
                        transaction.Commit();
                    } // End Try
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    } // End Catch

                    return noid;
                } // End Using transaction 

            } // End using connection

        } // End Function InsertLargeObject 


        public static void MyRead()
        {
            byte[] input = System.IO.File.ReadAllBytes(@"D:\Stefan.Steiger\Pictures\33077_640.jpg");

            const int bufferlength = 32768;
            using (System.IO.Stream output = System.IO.File.Create(@"d:\fffxxx.bin"))
            {
                int read = 0;
                while(read + bufferlength <= input.Length)
                {
                    output.Write(input, read, bufferlength);
                    read += bufferlength;
                } // Whend

                int rest = input.Length - read;
                if (rest > 0)
                    output.Write(input, read, rest);
            } //End using output

            byte[] outputb = System.IO.File.ReadAllBytes(@"D:\fffxxx.bin");
            System.Console.WriteLine(input.Length);
            System.Console.WriteLine(outputb.Length);
        } // End Sub MyRead


        protected override string OdbcFunctionReplacementCallback(System.Text.RegularExpressions.Match mThisMatch)
        {
            // Get the matched string.
            string strExpression = mThisMatch.Groups[1].Value;
            string strFunctionName = mThisMatch.Groups[2].Value;
            string strArguments = mThisMatch.Groups[3].Value;

            if (System.Text.RegularExpressions.Regex.IsMatch(strArguments, strOdbcMatchingPattern))
            {
                strArguments = System.Text.RegularExpressions.Regex.Replace(strArguments, strOdbcMatchingPattern, OdbcFunctionReplacementCallback);
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("lcase", strFunctionName))
            {
                return "LOWER(" + strArguments + ") ";
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("ucase", strFunctionName))
            {
                return "UPPER(" + strArguments + ") ";
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("now", strFunctionName))
            {
                return "CURRENT_TIMESTAMP ";
            }





            string[] astrArguments = GetArguments(strArguments);

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals("ilike", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " ILIKE " + astrArguments[1] + " ) ";
                return strTerm;
            }

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals("like", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " LIKE " + astrArguments[1] + " ) ";
                return strTerm;
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("left", strFunctionName))
            {
                string strTerm = "LPAD(" + astrArguments[0] + ", " + astrArguments[1] + ", '') ";
                return strTerm;
            }

            if (System.StringComparer.OrdinalIgnoreCase.Equals("right", strFunctionName))
            {
                string strTerm = "SUBSTRING(" + astrArguments[0] + " FROM CHAR_LENGTH(" + astrArguments[0] + " ) - ( " + astrArguments[1] + " ) + 1 ) ";
                return strTerm;
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("concat", strFunctionName))
            {
                string strTerm = astrArguments[0] + " || " + astrArguments[1];
                return strTerm;
            }


            //AND EXTRACT(day from HIDOC_Datum - timestamp '2011-12-07' ) = 0

            if (System.StringComparer.OrdinalIgnoreCase.Equals("timestampdiff", strFunctionName))
            {
                string strTerm = "";
                if (System.StringComparer.OrdinalIgnoreCase.Equals("SQL_TSI_DAY", astrArguments[0]))
                {
                    strTerm = "abs(extract(day from " + astrArguments[1] + " - " + astrArguments[2] + " )) ";
                }
                else
                {
                    throw new System.NotImplementedException();
                }

                return strTerm;
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("dayofmonth", strFunctionName))
            {
                string strTerm = "date_part('day', " + astrArguments[0] + ") ";
                return strTerm;
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("month", strFunctionName))
            {
                string strTerm = "date_part('month', " + astrArguments[0] + ") ";
                return strTerm;
            }


            if (System.StringComparer.OrdinalIgnoreCase.Equals("year", strFunctionName))
            {
                string strTerm = "date_part('year', " + astrArguments[0] + ") ";
                return strTerm;
            }

            return "ODBC FUNCTION \"" + strFunctionName + "\" not defined in abstraction layer...";
        } // End Function OdbcFunctionReplacementCallback


        public void SetInitialPassword()
        {
            // su - postgres
            // psql -p 5432    or // psql -p 5433 
            // ALTER USER postgres WITH ENCRYPTED PASSWORD 'password';
            // \q
        } // End Sub SetInitialPassword 


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder()
        {
            return new PostGreUniversalConnectionStringBuilder();
        }


        public override UniversalConnectionStringBuilder NewConnectionStringBuilder(string connectionString)
        {
            return new PostGreUniversalConnectionStringBuilder(connectionString);
        }


        public override string GetConnectionString(string strDb)
        {
            string con = m_ConnectionString.ConnectionString;
            if (string.IsNullOrEmpty(strDb))
                return con;

            Npgsql.NpgsqlConnectionStringBuilder csb = new Npgsql.NpgsqlConnectionStringBuilder(m_ConnectionString.ConnectionString);
            csb.Database = strDb;
            con = csb.ConnectionString;
            // System.Console.WriteLine(con);
            csb = null;
            return con;


            /*
            this.m_ConnectionString.Host = m_DatabaseConfiguration.strServerName;
            Console.WriteLine (this.m_ConnectionString.Host);

            if (m_DatabaseConfiguration.iPort <= 0)
                this.m_ConnectionString.Port = 5432;
            else
                this.m_ConnectionString.Port = m_DatabaseConfiguration.iPort;

            this.m_ConnectionString.Database = m_DatabaseConfiguration.strInitialCatalog;

            if (m_DatabaseConfiguration.bIntegratedSecurity)
                this.m_ConnectionString.IntegratedSecurity = true;
            else
            {
                this.m_ConnectionString.IntegratedSecurity = false;
                m_ConnectionString.UserName = m_DatabaseConfiguration.strUserName;
                m_ConnectionString.Password = DB.Abstraction.cDAL.SecureString2String(m_DatabaseConfiguration.ssSecurePassword);
            }

            m_ConnectionString.Timeout = m_DatabaseConfiguration.iConnectionTimeout;

            Console.WriteLine (m_ConnectionString.ConnectionString);
            */

        } // End Function GetConnectionString


        // http://npgsql.projects.postgresql.org/docs/manual/UserManual.html
        public System.Data.Common.DbProviderFactory GetFactory()
        {
            //AddFactoryClasses();
            System.Data.Common.DbProviderFactory providerFactory = null;
            providerFactory = this.GetFactory(typeof(Npgsql.NpgsqlFactory));
            //providerFactory = this.GetFactory("Npgsql.NpgsqlFactory, Npgsql");
            //providerFactory = Mono.Sucks.DbProviderFactories.GetFactory("Npgsql");
            //providerFactory = System.Data.Common.DbProviderFactories.GetFactory("Npgsql");

            return providerFactory;
        } // End Function GetFactory


        protected override string GetTextFromObject(object obj)
        {
            string strText = null;

            if (obj != null)
            {
                if (object.ReferenceEquals(obj.GetType(), typeof(System.DateTime)))
                {
                    System.DateTime dtDateTime = (System.DateTime)obj;
                    strText = dtDateTime.ToString("yyyy-MM-dd");
                }
                else if (object.ReferenceEquals(obj.GetType(), typeof(bool)))
                {
                    bool bValue = (bool)obj;
                    if (bValue)
                        strText = "true";
                    else
                        strText = "false";
                }
                else if (object.ReferenceEquals(obj.GetType().BaseType, typeof(System.Text.Encoding)))
                {
                    System.Text.Encoding enc = (System.Text.Encoding)obj;
                    //strText = enc.EncodingName;
                    //strText = enc.BodyName;
                    //strText = enc.HeaderName;
                    //strText = enc.CodePage.ToString(); // 20127
                    //strText = enc.WindowsCodePage.ToString() ; // 1252
                    strText = enc.WebName;
                }
                else
                    strText = obj.ToString();
            } // End if (obj != null)

            return strText;
        } // End Function GetTextFromObject


        public override System.Data.IDbConnection GetConnection()
        {
            Npgsql.NpgsqlConnection npgsqlc = new Npgsql.NpgsqlConnection(m_ConnectionString.ConnectionString);
            return npgsqlc;
        } // End Function GetConnection


        public override System.Data.DataTable GetEntireTable(string strTableName)
        {
            return GetDataTable("SELECT * FROM " + this.QuoteObject( strTableName) + " ");
        } // End Function GetEntireTable


        public void UpdateAllSequences()
        {
            //  Updates all the sequences to have a next value of max+1 excluding the list passed 
            // -- http://devgrok.blogspot.ch/2010/01/updating-nextval-of-all-sequences-in.html

            string strSQL = @"
CREATE OR REPLACE FUNCTION fn_fixsequences_DAL(excludes text) RETURNS integer AS
$BODY$
DECLARE
themax BIGINT;
mytables RECORD;
num integer;
BEGIN
num := 0;
FOR mytables IN
SELECT relname, ns.nspname, a.attname, pg_get_serial_sequence(c.relname, a.attname) as seq
FROM pg_catalog.pg_attribute a 

INNER JOIN pg_catalog.pg_class c 
    ON c.oid=a.attrelid 

inner join pg_catalog.pg_attrdef d
    ON d.adrelid = a.attrelid AND d.adnum = a.attnum AND a.atthasdef
    
LEFT JOIN pg_catalog.pg_namespace ns 
    ON ns.oid = c.relnamespace 

WHERE
pg_catalog.pg_table_is_visible(c.oid)
AND a.attnum > 0 
AND NOT a.attisdropped 
--AND atttypid=20
AND relname NOT LIKE 'pg_%' 
AND relname NOT LIKE excludes
AND ns.nspname NOT IN ('information_schema', 'pg_catalog')
AND c.relkind='r'
AND NOT pg_get_serial_sequence(c.relname, a.attname) IS null
LOOP
    EXECUTE 'SELECT MAX('||mytables.attname||') FROM '||mytables.nspname||'.'||mytables.relname||';' INTO themax;
    IF (themax IS null OR themax < 0) THEN
        themax := 0;
    END IF;
    themax := themax +1;
    EXECUTE 'ALTER SEQUENCE ' || mytables.seq || ' RESTART WITH '||themax;
    num := num + 1;
END LOOP;
RETURN num;
END;
$BODY$
LANGUAGE 'plpgsql' VOLATILE;

--COMMENT ON FUNCTION fn_fixsequences_DAL() IS 'Updates all the sequences to have a next value of max+1 excluding the list passed';
";

            this.ExecuteNonQuery(strSQL);
            strSQL = "SELECT fn_fixsequences_DAL(''); ";
            this.ExecuteNonQuery(strSQL);
        } // End Function UpdateAllSequences


        public override string GetInsertString(string strTableName)
        {
            string strSQL;

            using (System.Data.DataTable dt = this.GetColumnNamesForTable(strTableName))
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("INSERT INTO ");
                sb.AppendLine(EscapeTableName(strTableName));
                sb.AppendLine("( ");

                // @
                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    if (i == 0)
                        sb.Append("     ");
                    else
                        sb.Append("    ,");

                    sb.AppendLine(System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]));
                } // Next i



                sb.AppendLine(") ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");

                for (int i = 0; i < dt.Rows.Count; ++i)
                {
                    if (i == 0)
                        sb.Append("     ");
                    else
                        sb.Append("    ,");

                    sb.Append("__");
                    sb.Append(System.Convert.ToString(dt.Rows[i]["COLUMN_NAME"]));
                    sb.Append(" -- ");
                    sb.Append(System.Convert.ToString(dt.Rows[i]["DATA_TYPE"]));


                    string strMaxLen = System.Convert.ToString(dt.Rows[i]["CHARACTER_MAXIMUM_LENGTH"]);

                    if (!string.IsNullOrEmpty(strMaxLen))
                    {

                        sb.Append("(");
                        sb.Append(strMaxLen);
                        sb.Append(")");
                    }

                    sb.Append(", ");

                    if (System.StringComparer.OrdinalIgnoreCase.Equals(System.Convert.ToString(dt.Rows[i]["IS_NULLABLE"]), "YES"))
                        sb.AppendLine("NULL");
                    else
                        sb.AppendLine("NOT NULL");
                } // Next i

                // ,@COLUMN_NAME

                sb.AppendLine(") ");

                strSQL = sb.ToString();
                sb = null;
            } // End Using dt 

            return strSQL;
        } // End Function GetInsertString


    } // End Class cPostGreSQL


} // End Namespace DataBase.Tools
