
#if WITH_GPL

namespace DB.Abstraction
{

    partial class cMySQL : cDAL
    {

        protected MySql.Data.MySqlClient.MySqlConnection m_SqlConnection;
        protected MySql.Data.MySqlClient.MySqlConnectionStringBuilder m_ConnectionString;
        

        public override bool IsMySql
        {
            get { return true; }
        }


        public cMySQL()
            : this("")
        {
            // Crap ! 
        } // End Constructor


        public cMySQL(string strConnectionString)
        {
            //this.m_DatabaseConfiguration = dbcDBconfig;
            this.m_dbtDBtype = DataBaseEngine_t.MySQL;
            this.m_providerFactory = this.GetFactory();
            this.m_dictScriptTemplates = GetSQLtemplates();
            this.m_dblDBversion = 5.5;
            this.m_ConnectionString = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder(strConnectionString);

            this.m_SqlConnection = new MySql.Data.MySqlClient.MySqlConnection(strConnectionString);
        } // End Constructor 2


		public override string GetConnectionString(string strDb)
		{
			if (string.IsNullOrEmpty (strDb)) 
				return m_ConnectionString.ConnectionString;

			string strRetVal = null;
			MySql.Data.MySqlClient.MySqlConnectionStringBuilder mcsb = new MySql.Data.MySqlClient.MySqlConnectionStringBuilder();
			mcsb.ConnectionString = m_ConnectionString.ConnectionString;
			mcsb.Database = strDb;
			strRetVal = mcsb.ConnectionString;
			mcsb.Clear();
			mcsb = null;

			return strRetVal;
		} // End Function GetConnectionString



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


            // Simple one or 0 arguments
            // if (StringComparer.OrdinalIgnoreCase.Equals("lcase", strFunctionName))
            // { return "LOWER(" + strArguments + ") "; }





            string[] astrArguments = GetArguments(strArguments);

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals("ilike", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " LIKE " + astrArguments[1] + " ) ";
                return strTerm;
            } // End iLike

            if (System.StringComparer.InvariantCultureIgnoreCase.Equals("like", strFunctionName))
            {
                string strTerm = "( " + astrArguments[0] + " LIKE BINARY " + astrArguments[1] + " ) ";
                return strTerm;
            } // End Like

            // if (StringComparer.OrdinalIgnoreCase.Equals("left", strFunctionName))
            // { string strTerm = "LPAD(" + astrArguments[0] + ", " + astrArguments[1] + ", '') "; return strTerm;}

            return "ODBC FUNCTION \"" + strFunctionName + "\" not defined in abstraction layer...";
        } // End Function OdbcFunctionReplacementCallback



        public System.Data.Common.DbProviderFactory GetFactory()
        {
            //AddFactoryClasses();
            System.Data.Common.DbProviderFactory providerFactory = null;
            //providerFactory = System.Data.Common.DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
            providerFactory = this.GetFactory(typeof(MySql.Data.MySqlClient.MySqlClientFactory));
            return providerFactory;
        }


        public void DisableForeignKeys()
        {
            // http://gauravsohoni.wordpress.com/2009/03/09/mysql-disable-foreign-key-checks-or-constraints/

            string strSQL = @"
            SET @@foreign_key_checks = 0;
            --DELETE FROM users where id > 45;
            SET @@foreign_key_checks = 1;
             ";

            System.Console.WriteLine(strSQL);
            throw new System.NotImplementedException("DisableForeignKeys not implemented");
        }


        public void UpdateSequenceForTable()
        {
            // http://webobserve.blogspot.ch/2011/02/reset-mysql-table-autoincrement.html
            // if table have existing records the auto increment value will reset to one higer than the maximum record (max value + 1) .
            // if  table have no records then it will automatically set as 1.
            string strSQL = @"ALTER TABLE tablename AUTO_INCREMENT = 1;";

            System.Console.WriteLine(strSQL);
            throw new System.NotImplementedException("UpdateSequenceForTable not implemented");
        }




        public void myxxx()
        {
            MySql.Data.MySqlClient.MySqlConnection connection = new MySql.Data.MySqlClient.MySqlConnection("constring");
            var bl = new MySql.Data.MySqlClient.MySqlBulkLoader(connection);
            bl.TableName = "mytable";
            bl.FieldTerminator = ",";
            bl.LineTerminator = "\r\n";
            bl.FileName = "myfileformytable.csv";
            bl.NumberOfLinesToSkip = 1;
            var inserted = bl.Load();
            System.Diagnostics.Debug.Print(inserted + " rows inserted."); 
        }


    } // End Class cMySQL

    public class xxx
    {

        protected static System.Type GetNullableType(System.Type type)
        {
            // Use Nullable.GetUnderlyingType() to remove the Nullable<T> wrapper if type is already nullable. 
            type = System.Nullable.GetUnderlyingType(type);
            if (type.IsValueType)
                return typeof(System.Nullable<>).MakeGenericType(type);
            else
                return type;
        } // End Function GetNullableType



        protected static bool IsDecimalPointType(object obj)
        {
            System.Collections.Generic.List<System.Type> ls = new System.Collections.Generic.List<System.Type>();
            ls.Add(typeof(float));
            ls.Add(typeof(System.Single));
            ls.Add(typeof(double));
            ls.Add(typeof(decimal));

            foreach (System.Type t in ls)
            {
                System.Type tNullable = GetNullableType(t);

                if (object.ReferenceEquals(obj, t))
                    return true;

                if (object.ReferenceEquals(obj, tNullable))
                    return true;
            }

            return false;
        } // End Function IsDecimalPointType


        /// <param name="dt">The source Data Table</param>
        /// <param name="filename">The destination file.</param>
        /// <param name="options">The options used to write the file</param>
        public static void DataTableToCsv(System.Data.DataTable dt, string strFilename, System.Text.Encoding enc, string strDelimiter, bool bWithTitle)
        {
            string strNewLineChars = System.Environment.NewLine;

            using (System.IO.StreamWriter fs = new System.IO.StreamWriter(strFilename, false, enc))
            {
                if (bWithTitle)
                {
                    for (int i = 0; i < dt.Columns.Count; ++i)
                    {
                        if (i > 0)
                            fs.Write(strDelimiter);

                        string strColumnName = dt.Columns[i].ColumnName;

                        if (strColumnName.Contains(strDelimiter) || strColumnName.Contains("\n") || strColumnName.Contains("\r"))
                            strColumnName = "\"" + strColumnName.Replace("\"", "\"\"") + "\"";

                        fs.Write(strColumnName);
                    } // Next i

                    fs.Write(strNewLineChars);
                } // End if (bWithTitle)


                foreach (System.Data.DataRow dr in dt.Rows)
                {
                    object[] fields = dr.ItemArray;

                    for (int i = 0; i < fields.Length; i++)
                    {
                        if (i > 0)
                            fs.Write(strDelimiter);

                        string strStringToWrite = "";
                        if (fields[i] != null)
                        {
                            if (object.ReferenceEquals(dt.Columns[i].DataType, typeof(System.DateTime)))
                            {
                                strStringToWrite = System.Convert.ToDateTime(fields[i]).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else if (IsDecimalPointType(dt.Columns[i].DataType))
                            {
                                strStringToWrite = ((decimal) fields[i]).ToString("0.#######");
                            }
                            else
                                strStringToWrite = System.Convert.ToString(fields[i]);

                            if (strStringToWrite.Contains(strDelimiter) || strStringToWrite.Contains("\n") || strStringToWrite.Contains("\r"))
                                strStringToWrite = "\"" + strStringToWrite.Replace("\"","\"\"") + "\"";
                        } // End if (fields[i] != null)

                        fs.Write(strStringToWrite);
                        //fs.Write(options.ValueToString(fields[i]));
                    } // Next i

                    fs.Write(strNewLineChars);
                } // Next dr
                fs.Close();
            } // End Using fs

        } // End Sub DataTableToCsv


    } // End Class xxx

} // End Namespace DB.Abstraction

#endif
