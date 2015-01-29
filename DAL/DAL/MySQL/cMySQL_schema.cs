
#if WITH_GPL

namespace DB.Abstraction
{
	
	partial class cMySQL : cDAL
	{
		////////////////////////////// Schema //////////////////////////////
		

		
		public override System.Data.DataTable GetTypes()
		{
			string strSQL = @"      SELECT * FROM ( SELECT 'tinyint' AS name, 1 AS GroupId, '' AS descriptionid, 'Numbers' AS GroupName, 'int8. 
Signed min: -128, Signed max: 127
Unsigned min: 0, Unsigned max: 255' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'smallint' AS name, 1 AS category, '' AS descriptionid, 'Numbers' AS categoryname, 'int16.
Signed min: -32768, Signed max: 32767
Unsigned min: 0, Unsigned max: 65535' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'mediumint' AS name, 1 AS category, '' AS descriptionid, 'Numbers' AS categoryname, 'int24
Signed min: -8388608, Signed max: 8388607
Unsigned min: 0, Unsigned max: 16777215' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'int' AS name, 1 AS category, '' AS descriptionid, 'Numbers' AS categoryname, 'int32.
Signed min: -2147483648, Signed max: 2147483647
Unsigned min: 0, Unsigned max: 4294967295' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'bigint' AS name, 1 AS category, '' AS descriptionid, 'Numbers' AS categoryname, 'int64.
Signed min: -9223372036854775808, Signed max: 9223372036854775807
Unsigned min: 0, Unsigned max: 18446744073709551615' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'decimal' AS name, 1 AS category, '' AS descriptionid, 'Numbers' AS categoryname, 'Fixed-Point Types (Exact Value)
Synax: decimal [ (p[ ,s] )] 
In standard SQL, the syntax DECIMAL(p) is equivalent to DECIMAL(p, 0). 
p: Precision
   The maximum total number of decimal digits that will be stored, both to the left and to the right of the decimal point. 
   The precision must be a value from 1 through the maximum precision of 65 (64 from MySQL 5.0.3 to 5.0.5). 
   The default precision is 10.
s: Scale
   The number of decimal digits that will be stored to the right of the decimal point. 
   This number is substracted from p to determine the maximum number of digits to the left of the decimal point. 
   Scale must be a value from 0 through p. 
   Scale can be specified only if precision is specified. 
   The default scale is 0; therefore, 0 <= s <= p. 
   Maximum storage sizes vary, based on the precision.
   If the scale is 0, DECIMAL values contain no decimal point or fractional part.

In MySQL, NUMERIC is implemented as DECIMAL, so the following remarks about DECIMAL apply equally to NUMERIC.

As of MySQL 5.0.3, DECIMAL values are stored in binary format. 
Previously, they were stored as strings, with one character used for each digit of the value, the decimal point (if the scale is greater than 0), and the “-” sign (for negative numbers).

Before MySQL 5.0.3, the maximum range of DECIMAL values is the same as for DOUBLE, but the actual range for a given DECIMAL column can be constrained by the precision or scale for a given column. When such a column is assigned a value with more digits following the decimal point than are permitted by the specified scale, the value is converted to that scale. (The precise behavior is operating system-specific, but generally the effect is truncation to the permissible number of digits.)

http://dev.mysql.com/doc/refman/5.0/en/fixed-point-types.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'float' AS name, 1 AS category, '' AS descriptionid, 'Numbers' AS categoryname, 'Floating-Point Types (Approximate Value) 
Four bytes for single-precision values. 

FLOAT[(M[,D])] [ZEROFILL] stores floating point numbers in the range of 
Signed min: -3.402823466E+38, Signed max: 3.402823466E+38
Unsigned min: ?, Unsigned max: ?
The closest signed float to zero is
1.175494351E-38 positive
-1.175494351E-38 negative


MySQL also treats REAL as a synonym for DOUBLE PRECISION (a nonstandard variation), unless the REAL_AS_FLOAT SQL mode is enabled. 

MySQL permits a nonstandard syntax: FLOAT(M,D) or REAL(M,D) or DOUBLE PRECISION(M,D). Here, “(M,D)” means than values can be stored with up to M digits in total, of which D digits may be after the decimal point. For example, a column defined as FLOAT(7,4) will look like -999.9999 when displayed. MySQL performs rounding when storing values, so if you insert 999.00009 into a FLOAT(7,4) column, the approximate result is 999.0001.

If precision isn''t specified, or <= 24, it''s SINGLE precision, otherwise FLOAT is DOUBLE precision. 
When specified alone, precision can range from 0 to 53. 
If the scale is defined, too, precision may be up to 255, scale up to 253.

http://dev.mysql.com/doc/refman/5.0/en/floating-point-types.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'double' AS name, 1 AS category, '' AS descriptionid, 'Numbers' AS categoryname, 'Floating-Point Types (Approximate Value) 
Eight bytes for double-precision values.

DOUBLE[(M,D)] [ZEROFILL] stores floating point numbers in the range of 
Signed min: -1.7976931348623157E+308, Signed max: 1.7976931348623157E+308
Unsigned min: ?, Unsigned max: ?
The closest signed double to zero is
2.2250738585072014E-308 positive
-2.2250738585072014E-308 negative

A precision from 24 to 53 results in an 8-byte double-precision DOUBLE column.

MySQL also treats REAL as a synonym for DOUBLE PRECISION (a nonstandard variation), unless the REAL_AS_FLOAT SQL mode is enabled. 

MySQL permits a nonstandard syntax: FLOAT(M,D) or REAL(M,D) or DOUBLE PRECISION(M,D). Here, “(M,D)” means than values can be stored with up to M digits in total, of which D digits may be after the decimal point. For example, a column defined as FLOAT(7,4) will look like -999.9999 when displayed. MySQL performs rounding when storing values, so if you insert 999.00009 into a FLOAT(7,4) column, the approximate result is 999.0001.

http://dev.mysql.com/doc/refman/5.0/en/floating-point-types.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'date' AS name, 2 AS category, '' AS descriptionid, 'Date and time' AS categoryname, 'The DATE type is used for values with a date part but no time part. MySQL retrieves and displays DATE values in ''YYYY-MM-DD'' format. The supported range is ''1000-01-01'' to ''9999-12-31''.' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'datetime' AS name, 2 AS category, '' AS descriptionid, 'Date and time' AS categoryname, 'The DATETIME type is used for values that contain both date and time parts. MySQL retrieves and displays DATETIME values in ''YYYY-MM-DD HH:MM:SS'' format. The supported range is ''1000-01-01 00:00:00'' to ''9999-12-31 23:59:59''.
http://dev.mysql.com/doc/refman/5.6/en/datetime.html
' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'timestamp' AS name, 2 AS category, '' AS descriptionid, 'Date and time' AS categoryname, 'The TIMESTAMP data type is used for values that contain both date and time parts. TIMESTAMP has a range of ''1970-01-01 00:00:01'' UTC to ''2038-01-19 03:14:07'' UTC. A DATETIME or TIMESTAMP value can include a trailing fractional seconds part in up to microseconds (6 digits) precisio.

http://dev.mysql.com/doc/refman/5.6/en/datetime.html
' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'time' AS name, 2 AS category, '' AS descriptionid, 'Date and time' AS categoryname, 'MySQL retrieves and displays TIME values in ''HH:MM:SS'' format (or ''HHH:MM:SS'' format for large hours values). TIME values may range from ''-838:59:59'' to ''838:59:59''. The hours part may be so large because the TIME type can be used not only to represent a time of day (which must be less than 24 hours), but also elapsed time or a time interval between two events (which may be much greater than 24 hours, or even negative).

http://dev.mysql.com/doc/refman/5.6/en/time.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'year' AS name, 2 AS category, '' AS descriptionid, 'Date and time' AS categoryname, 'As a 4-digit number/string in the range 1901 to 2155. As a 1- or 2-digit number/string in the range ''0'' to ''99''. Values in the ranges ''0'' to ''69'' and ''70'' to ''99'' are converted to YEAR values in the ranges 2000 to 2069 and 1970 to 1999. For YEAR(4), the result has a display value of 0000 and an internal value of 0000. To specify zero for YEAR(4) and have it be interpreted as 2000, specify it as a string ''0'' or ''00''.

http://dev.mysql.com/doc/refman/5.6/en/year.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'char' AS name, 3 AS category, '' AS descriptionid, 'Strings' AS categoryname, 'char(length). The length can be any value from 0 to 255. When CHAR values are stored, they are right-padded with spaces to the specified length. When CHAR values are retrieved, trailing spaces are removed unless the PAD_CHAR_TO_FULL_LENGTH SQL mode is enabled.' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'varchar' AS name, 3 AS category, '' AS descriptionid, 'Strings' AS categoryname, 'Values in VARCHAR columns are variable-length strings. The length can be specified as a value from 0 to 65,535. The effective maximum length of a VARCHAR is subject to the maximum row size (65,535 bytes, which is shared among all columns) and the character set used. ' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'tinytext' AS name, 3 AS category, '' AS descriptionid, 'Strings' AS categoryname, 'L + 1 bytes, where L < 2^8' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'text' AS name, 3 AS category, '' AS descriptionid, 'Strings' AS categoryname, 'L + 2 bytes, where L < 2^16' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'mediumtext' AS name, 3 AS category, '' AS descriptionid, 'Strings' AS categoryname, 'L + 3 bytes, where L < 2^24' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'longtext' AS name, 3 AS category, '' AS descriptionid, 'Strings' AS categoryname, 'L + 4 bytes, where L < 2^32' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'enum' AS name, 4 AS category, '' AS descriptionid, 'Lists' AS categoryname, '1 or 2 bytes, depending on the number of enumeration values (65''535 values maximum)' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'set' AS name, 4 AS category, '' AS descriptionid, 'Lists' AS categoryname, '1, 2, 3, 4, or 8 bytes, depending on the number of set members (64 members maximum)' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'bit' AS name, 5 AS category, '' AS descriptionid, 'Binary' AS categoryname, 'bit(length). As of MySQL 5.0.3, the BIT data type is used to store bit-field values. A type of BIT(M) enables storage of M-bit values. M can range from 1 to 64.
To specify bit values, b''value'' notation can be used. value is a binary value written using zeros and ones. For example, b''111'' and b''10000000'' represent 7 and 128, respectively. 
If you assign a value to a BIT(M) column that is less than M bits long, the value is padded on the left with zeros. For example, assigning a value of b''101'' to a BIT(6) column is, in effect, the same as assigning b''000101''.

http://dev.mysql.com/doc/refman/5.0/en/bit-type.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'binary' AS name, 5 AS category, '' AS descriptionid, 'Binary' AS categoryname, 'The permissible maximum length is 0 to 255 bytes.' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'varbinary' AS name, 5 AS category, '' AS descriptionid, 'Binary' AS categoryname, 'The permissible maximum length is 0 to 65''535 bytes.' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'tinyblob' AS name, 5 AS category, '' AS descriptionid, 'Binary' AS categoryname, 'L + 1 bytes, where L < 2^8' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'blob' AS name, 5 AS category, '' AS descriptionid, 'Binary' AS categoryname, 'Binary large object. L + 2 bytes, where L < 2^16' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'mediumblob' AS name, 5 AS category, '' AS descriptionid, 'Binary' AS categoryname, 'Medium sized binary large object. L + 3 bytes, where L < 2^24' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'longblob' AS name, 5 AS category, '' AS descriptionid, 'Binary' AS categoryname, 'L + 4 bytes, where L < 2^32' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'geometry' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '
GEOMETRY can store geometry values of any type. 
The other single-value types (POINT, LINESTRING, and POLYGON) restrict their values to a particular geometry type.

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'point' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '
POINT(X,Y)

1: INSERT INTO Site (1, GeomFromText( ''POINT(48.19976 16.45572)'' )     ); -- Any version (technically wrong)
2: INSERT INTO Site (2, POINT(48.19976, 16.45572) );  -- MySQL >= 5.1.35 
3: INSERT INTO Site ( GeomFromWKB( Point(1,2) ) ); -- MySQL < 5.1.35

Concatenating and then parsing text seems intuitively slower and more error-prone than functions that accept proper variables as input, 
so I can''t think of any reason to craft concatenated strings and use the text-based functions.

The fact that the 1st method works (in spite of being technically wrong) on newer servers 
and the 2nd method doesn''t work at all prior to MySQL 5.1.35 might explain why examples 
were written using approach 1 - to avoid the issue entirely.

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'linestring' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '
INSERT INTO Site (
   LineStringFromWKB(
      LineString(
         geoPoint
        ,GeomFromText(''POINT(52.5177, -0.0968)'')
     )
   )
)


Example: 

SELECT
    placeName
   ,ROUND(GLength(
       LineStringFromWKB(
          LineString(
              geoPoint
             ,GeomFromText(''POINT(52.5177, -0.0968)'')
          )
      )
   )) AS distance
FROM spatialTable
ORDER BY distance ASC;

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'polygon' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '

Examples:
SET @g = ''POLYGON((0 0,10 0,10 10,0 10,0 0),(5 5,7 5,7 7,5 7, 5 5))'';
INSERT INTO geom VALUES (GeomFromText(@g));

SELECT * FROM sunzones WHERE MBRWITHIN( POINT( 863575.082797506, 2137306.79465704 ) , polygon ) 

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'multipoint' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '
Hold collections of point(s) 

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'multilinestring' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '
Hold collections of linestring(s)

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'multipolygon' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '
Hold collections of polygon(s) 

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 

UNION SELECT * FROM ( SELECT 'geometrycollection' AS name, 6 AS category, '' AS descriptionid, 'Geometry' AS categoryname, '
GEOMETRYCOLLECTION can store a collection of objects of any type. 
The other collection types (MULTIPOINT, MULTILINESTRING, MULTIPOLYGON, and GEOMETRYCOLLECTION) 
restrict collection members to those having a particular geometry type.

Example:
SET @g = ''GEOMETRYCOLLECTION(POINT(1 1),LINESTRING(0 0,1 1,2 2,3 3,4 4))'';
INSERT INTO geom VALUES (GeomFromText(@g));

http://dev.mysql.com/doc/refman/5.0/en/mysql-spatial-datatypes.html' AS description) AS t WHERE (1=1) 
";

			return GetDataTable(strSQL);
		}


		public override System.Data.DataTable GetDataBases()
		{
			return GetDataBases(dbOwner.all);
		}
		
		
		/*
         SELECT VARIABLE_NAME, VARIABLE_VALUE FROM INFORMATION_SCHEMA.GLOBAL_VARIABLES WHERE VARIABLE_NAME = 'VERSION'
        */
		
		// http://stackoverflow.com/questions/3499372/get-list-of-mysql-databases-and-server-version
		public System.Data.DataTable GetDataBases(dbOwner ShowDBs)
		{
			string strSQL = @"
SELECT 
    schema_name AS Name 
    ,
    CASE WHEN schema_name IN ('mysql', 'information_schema', 'performance_schema') 
        THEN 0 
        ELSE 1
    END AS Sort 
FROM INFORMATION_SCHEMA.SCHEMATA 
-- WHERE schema_name NOT IN ('mysql', 'information_schema', 'performance_schema')
ORDER BY Sort, Name 
";
			/*
            if ((uint)(ShowDBs & dbOwner.user) != 0)
            {
                strSQL += @"
                WHERE owner_sid != 0x01 
                ";
            }

            strSQL += @"
            ORDER BY name ASC 
            ";
            */
			
			//string strOldInitialCatalog = this.m_ConnectionString.InitialCatalog;
			//this.m_ConnectionString.InitialCatalog = "master";
			
			System.Data.DataTable dt = GetDataTable(strSQL);
			
			//this.m_ConnectionString.InitialCatalog = strOldInitialCatalog;
			//strOldInitialCatalog = null;
			
			return dt;
		} // End Function GetDataBases
		
		
		
		protected bool DatabaseExists(string strDataBaseName)
		{
            throw new System.NotImplementedException();
		} // End Function TableHasColumn
		
		
		public override void CreateDB()
		{
			CreateDB("", "", "");
		}
		
		
		// http://web.firebirdsql.org/dotnetfirebird/create-a-new-database-from-an-sql-script.html
		public override void CreateDB(string strDBname, string strDataLocation, string strLogLocation)
		{
            throw new System.NotImplementedException("cMySQL_schema.cs ==> CreateDB");
			try
			{
				try
				{
                    System.Console.WriteLine("Create Db");
					// Create a new database
					//MySql.Data.MySqlClient.MySqlConnection.CreateDatabase(this.m_ConnectionString.ConnectionString);
				}
				catch (MySql.Data.MySqlClient.MySqlException ex)
				{
					if(ex.ErrorCode == 335544344)
						Log("This database already exists.");
					else
                        Log(ex.Message);
				}
			} // End Try
            catch (System.Exception ex)
			{
                if (Log("cMySQL_schema.cs ==> CreateDB(string strDBname, string strDataLocation, string strLogLocation)", ex, "CreateDB"))
                    throw;
			} // End Catch
			finally
			{
				System.Threading.Thread.Sleep(2000); // Wait for disk-write complete
			} // End Finally
		}
		
		
		public virtual long GetLastInsertId()
		{
			// http://viralpatel.net/blogs/get-autoincrement-value-after-insert-query-in-mysql/
			string strSQL = "SELECT LAST_INSERT_ID();";
			return ExecuteScalar<long>(strSQL);
		}
		
		
		public override System.Data.DataTable GetTables() 
		{ 
			return GetTables (null);
		}
		
		
		// http://www.firebirdfaq.org/faq174/
		public override System.Data.DataTable GetTables(string strDb)
		{
			string strCatalog = strDb;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = m_ConnectionString.Database;
			
			
			
			using (System.Data.IDbCommand cmd = CreateCommand()) 
			{
				cmd.CommandText = @"
SELECT * 
FROM INFORMATION_SCHEMA.tables 
WHERE table_schema = @strCatalog ";
				
				this.AddParameter(cmd, "strCatalog", strCatalog);
				
				return GetDataTable(cmd, strDb);
			}
			
		} // End Function GetTables
		
		
		public override System.Data.DataTable GetViews() 
		{ 
			return GetViews (null);
		}
		
		
		// http://www.firebirdfaq.org/faq174/
		public override System.Data.DataTable GetViews(string strDb)
		{
			string strCatalog = strDb;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = m_ConnectionString.Database;
			
			
			using (System.Data.IDbCommand cmd = CreateCommand()) 
			{
				cmd.CommandText = @"
SELECT * 
FROM INFORMATION_SCHEMA.views 
WHERE table_schema = @strCatalog ";
				
				this.AddParameter(cmd, "strCatalog", strCatalog);
				
				return GetDataTable(cmd, strDb);
			} // End Using cmd
			
		} // End Function GetViews
		
		
		public override System.Data.DataTable GetProcedures()
		{
			return GetProcedures(null);
		}
		
		
		public override System.Data.DataTable GetProcedures(string strDb)
		{
			string strCatalog = strDb;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = this.m_ConnectionString.Database;
			
			using (System.Data.IDbCommand cmd = CreateCommand()) 
			{
				cmd.CommandText = @"
SELECT * 
FROM INFORMATION_SCHEMA.routines 
WHERE routine_schema = @strCatalog 
AND routine_type = 'PROCEDURE' 
";
				
				this.AddParameter(cmd, "strCatalog", strCatalog);
				
				return GetDataTable(cmd, strDb);
			}
			
		} // End Function GetProcedures
		
		
		public override System.Data.DataTable GetFunctions()
		{
			return GetFunctions (null);
		}
		
		
		public override System.Data.DataTable GetFunctions(string strDb)
		{
			string strCatalog = strDb;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = this.m_ConnectionString.Database;
			
			using (System.Data.IDbCommand cmd = CreateCommand()) 
			{
				cmd.CommandText = @"
SELECT * 
FROM INFORMATION_SCHEMA.routines 
WHERE routine_schema = @strCatalog 
AND routine_type = 'FUNCTION' 
";
				
				this.AddParameter(cmd, "strCatalog", strCatalog);
				
				return GetDataTable(cmd, strDb);
			}
			
		} // End Function GetFunctions
		
		
		public override System.Data.DataTable GetRoutines()
		{
			return GetRoutines (null);
		}
		
		
		public override System.Data.DataTable GetRoutines(string strDb)
		{
			string strCatalog = strDb;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = this.m_ConnectionString.Database;
			
			using (System.Data.IDbCommand cmd = CreateCommand()) 
			{
				cmd.CommandText = @"
SELECT * 
FROM INFORMATION_SCHEMA.routines 
WHERE routine_schema = @strCatalog 
";
				
				this.AddParameter(cmd, "strCatalog", strCatalog);
				
				return GetDataTable(cmd, strDb);
			}
			
		} // End Function GetRoutines
		
		
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
						strSQL += "     `" + strColumnName + "` " + strNewLine;
					else
						strSQL += "    ,`" + strColumnName + "` " + strNewLine;
				} // Next i
				
			} // End Using dt
			
			strSQL += "FROM `" + strTableName + "` " + strNewLine;
			
			return strSQL;
		} // End Function GetTableSelectText
		
		
		public override string GetTableDeleteText(string strTableName)
		{
			string strNewLine = "\r\n"; //  Environment.NewLine
			string strSQL = strNewLine + "DELETE FROM `" + strTableName + "` " + strNewLine;
			strSQL += "WHERE " + "(1=2) " + strNewLine;
			
			return strSQL;
		}
		
		
		public override string GetTableCreateText(string strTableName)
		{
			string strNewLine = "\r\n"; //  Environment.NewLine
			string strSQL = strNewLine + "CREATE TABLE `" + strTableName + "` " + strNewLine;
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
					}
					else
						strCharacterMaximumLength = "";
					
					if (i == 0)
						strSQL += "     `" + strColumnName + "` " + strDataType + strCharacterMaximumLength + " " + strIsNullable + strNewLine;
					else
						strSQL += "    ,`" + strColumnName + "` " + strDataType + strCharacterMaximumLength + " " + strIsNullable + strNewLine;
					
				} // Next i
				
				strSQL += ") ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8;" + strNewLine;
			} // End using dt
			
			
			//strSQL += strNewLine + "ALTER TABLE \"" + strTableName + "\" OWNER TO " + this.m_ConnectionString.UserName + ";" + strNewLine;
			
			/*
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
                    strSQL += @"ALTER TABLE [" + strTableName + "] ADD CONSTRAINT [" + strKeyName + "] " + strNewLine;
                    strSQL += @"PRIMARY KEY " + strClusterType + " (" + strKeyColumns + ");" + strNewLine;
                } // Next dr 

            } // End Using dt 
            */
			
			return strSQL;
		} // End Function GetTableCreateText 
		
		
		public override System.Data.DataTable GetColumnNames()
		{
			return GetColumnNames (null);
		}
		
		// override
		public System.Data.DataTable GetColumnNames(string strDb)
		{
			string strCatalog = strDb;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = this.m_ConnectionString.Database;
			
			using (System.Data.IDbCommand cmd = CreateCommand()) 
			{
				cmd.CommandText = @"
SELECT * 
FROM INFORMATION_SCHEMA.columns 
WHERE table_schema = @strCatalog 
ORDER BY table_name, ordinal_position 
";
				
				this.AddParameter(cmd, "strCatalog", strCatalog);
				
				return GetDataTable(cmd, strDb);
			}
			
		} // End Function GetColumnNames
		
		
		public override System.Data.DataTable GetRoutineParameters(string strRoutineName)
		{
			return GetRoutineParameters (strRoutineName, null);
		}
		
		
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
				
				dt = this.GetDataTable(cmd, strDbName);
			} // End Using cmd
			
			return dt;
		} // End Function GetRoutineParameters
		
		
		public override System.Data.DataTable GetColumnNamesForTable(string strTableName)
		{
			return GetColumnNamesForTable (strTableName, null);
		}
		
		
		// http://www.firebirdfaq.org/faq174/
		public override System.Data.DataTable GetColumnNamesForTable(string strTableName, string strDbName)
		{
			System.Data.DataTable dt = null;
			string strCatalog = strDbName;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = m_ConnectionString.Database;
			
			using(System.Data.IDbCommand cmd = this.CreateCommand())
			{
				cmd.CommandText = @"
SELECT * 
FROM INFORMATION_SCHEMA.columns
WHERE table_schema = @strCatalog 
AND table_name = @strTableName 
ORDER BY table_name, ordinal_position
";
				
				this.AddParameter(cmd, "strCatalog",strCatalog);
				this.AddParameter(cmd, "strTableName", strTableName);
				
				dt = this.GetDataTable(cmd, strDbName);
			} // End Using cmd
			
			return dt;
		} // End Function GetColumnNamesForTable
		
		
		public override bool TableExists(string strTableName)
		{
			return TableExists (strTableName, null);
		}
		
		
		// override
		public bool TableExists(string strTableName, string strDb)
		{
			bool bRetval = false;
			string strCatalog = strDb;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = m_ConnectionString.Database;
			
			using(System.Data.IDbCommand cmd = this.CreateCommand())
			{
				cmd.CommandText = @"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.tables
WHERE table_schema = @strCatalog 
AND table_name = @strTableName 
";
				
				this.AddParameter(cmd, "strCatalog",strCatalog);
				this.AddParameter(cmd, "strTableName", strTableName);
				
				bRetval = ExecuteScalar<bool>(cmd);
			} // End Using cmd
			
			return bRetval;
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
			bool bRetval = false;
			string strCatalog = null;
			
			if (string.IsNullOrEmpty (strCatalog))
				strCatalog = m_ConnectionString.Database;
			
			using(System.Data.IDbCommand cmd = this.CreateCommand())
			{
				cmd.CommandText = @"
SELECT COUNT(*) 
FROM INFORMATION_SCHEMA.columns 
WHERE table_schema = @strCatalog 
AND table_name = @strTableName 
AND column_name = @strColumnName 
ORDER BY table_name, ordinal_position 
";
				
				this.AddParameter(cmd, "strCatalog",strCatalog);
				this.AddParameter(cmd, "strTableName", strTableName);
				this.AddParameter(cmd, "strColumnName", strColumnName);
				
				bRetval = ExecuteScalar<bool>(cmd);
			} // End Using cmd
			
			return bRetval;
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
				string strSQL = "SELECT version()";
				return ExecuteScalar<string>(strSQL);
			}
		} // End Property DBversion
		
		
		public static void Test()
		{
			cDAL DAL = new cMySQL();
			DAL.Execute("SELECT * FROM T_Benutzer");
			System.Console.WriteLine("x = {0}, y = {1}", DAL.DBtype, DAL.DBversion);
		} // End Sub Test
		
		
	} // End Class cMySQL
	
	
} // End Namespace DataBase.Tools

#endif
