﻿
//using System.Collections.Generic; // .NET 3.5
using DB.Abstraction.NET20_Substitute.Generics;


namespace DB.Abstraction
{


    partial class cMS_SQL : cDAL
    {

      


        // http://msdn.microsoft.com/en-us/library/ms189822.aspx
        protected override HashSet<string> GetReservedKeywords()
        {
            HashSet<string> hs = new HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);

            hs.Add("ABSOLUTE");
            hs.Add("ACTION");
            hs.Add("ADA");
            hs.Add("ADD");
            hs.Add("ADMIN");
            hs.Add("AFTER");
            hs.Add("AGGREGATE");
            hs.Add("ALIAS");
            hs.Add("ALL");
            hs.Add("ALLOCATE");
            hs.Add("ALTER");
            hs.Add("AND");
            hs.Add("ANY");
            hs.Add("ARE");
            hs.Add("ARRAY");
            hs.Add("AS");
            hs.Add("ASC");
            hs.Add("ASENSITIVE");
            hs.Add("ASSERTION");
            hs.Add("ASYMMETRIC");
            hs.Add("AT");
            hs.Add("ATOMIC");
            hs.Add("AUTHORIZATION");
            hs.Add("AVG");
            hs.Add("BACKUP");
            hs.Add("BEFORE");
            hs.Add("BEGIN");
            hs.Add("BETWEEN");
            hs.Add("BINARY");
            hs.Add("BIT");
            hs.Add("BIT_LENGTH");
            hs.Add("BLOB");
            hs.Add("BOOLEAN");
            hs.Add("BOTH");
            hs.Add("BREADTH");
            hs.Add("BREAK");
            hs.Add("BROWSE");
            hs.Add("BULK");
            hs.Add("BY");
            hs.Add("CALL");
            hs.Add("CALLED");
            hs.Add("CARDINALITY");
            hs.Add("CASCADE");
            hs.Add("CASCADED");
            hs.Add("CASE");
            hs.Add("CAST");
            hs.Add("CATALOG");
            hs.Add("CHAR");
            hs.Add("CHAR_LENGTH");
            hs.Add("CHARACTER");
            hs.Add("CHARACTER_LENGTH");
            hs.Add("CHECK");
            hs.Add("CHECKPOINT");
            hs.Add("CLASS");
            hs.Add("CLOB");
            hs.Add("CLOSE");
            hs.Add("CLUSTERED");
            hs.Add("COALESCE");
            hs.Add("COLLATE");
            hs.Add("COLLATION");
            hs.Add("COLLECT");
            hs.Add("COLUMN");
            hs.Add("COMMIT");
            hs.Add("COMPLETION");
            hs.Add("COMPUTE");
            hs.Add("CONDITION");
            hs.Add("CONNECT");
            hs.Add("CONNECTION");
            hs.Add("CONSTRAINT");
            hs.Add("CONSTRAINTS");
            hs.Add("CONSTRUCTOR");
            hs.Add("CONTAINS");
            hs.Add("CONTAINSTABLE");
            hs.Add("CONTINUE");
            hs.Add("CONVERT");
            hs.Add("CORR");
            hs.Add("CORRESPONDING");
            hs.Add("COUNT");
            hs.Add("COVAR_POP");
            hs.Add("COVAR_SAMP");
            hs.Add("CREATE");
            hs.Add("CROSS");
            hs.Add("CUBE");
            hs.Add("CUME_DIST");
            hs.Add("CURRENT");
            hs.Add("CURRENT_CATALOG");
            hs.Add("CURRENT_DATE");
            hs.Add("CURRENT_DEFAULT_TRANSFORM_GROUP");
            hs.Add("CURRENT_PATH");
            hs.Add("CURRENT_ROLE");
            hs.Add("CURRENT_SCHEMA");
            hs.Add("CURRENT_TIME");
            hs.Add("CURRENT_TIMESTAMP");
            hs.Add("CURRENT_TRANSFORM_GROUP_FOR_TYPE");
            hs.Add("CURRENT_USER");
            hs.Add("CURSOR");
            hs.Add("CYCLE");
            hs.Add("DATA");
            hs.Add("DATABASE");
            hs.Add("DATE");
            hs.Add("DAY");
            hs.Add("DBCC");
            hs.Add("DEALLOCATE");
            hs.Add("DEC");
            hs.Add("DECIMAL");
            hs.Add("DECLARE");
            hs.Add("DEFAULT");
            hs.Add("DEFERRABLE");
            hs.Add("DEFERRED");
            hs.Add("DELETE");
            hs.Add("DENY");
            hs.Add("DEPTH");
            hs.Add("DEREF");
            hs.Add("DESC");
            hs.Add("DESCRIBE");
            hs.Add("DESCRIPTOR");
            hs.Add("DESTROY");
            hs.Add("DESTRUCTOR");
            hs.Add("DETERMINISTIC");
            hs.Add("DIAGNOSTICS");
            hs.Add("DICTIONARY");
            hs.Add("DISCONNECT");
            hs.Add("DISK");
            hs.Add("DISTINCT");
            hs.Add("DISTRIBUTED");
            hs.Add("DOMAIN");
            hs.Add("DOUBLE");
            hs.Add("DROP");
            hs.Add("DUMP");
            hs.Add("DYNAMIC");
            hs.Add("EACH");
            hs.Add("ELEMENT");
            hs.Add("ELSE");
            hs.Add("END");
            hs.Add("END-EXEC");
            hs.Add("EQUALS");
            hs.Add("ERRLVL");
            hs.Add("ESCAPE");
            hs.Add("EVERY");
            hs.Add("EXCEPT");
            hs.Add("EXCEPTION");
            hs.Add("EXEC");
            hs.Add("EXECUTE");
            hs.Add("EXISTS");
            hs.Add("EXIT");
            hs.Add("EXTERNAL");
            hs.Add("EXTRACT");
            hs.Add("FALSE");
            hs.Add("FETCH");
            hs.Add("FILE");
            hs.Add("FILLFACTOR");
            hs.Add("FILTER");
            hs.Add("FIRST");
            hs.Add("FLOAT");
            hs.Add("FOR");
            hs.Add("FOREIGN");
            hs.Add("FORTRAN");
            hs.Add("FOUND");
            hs.Add("FREE");
            hs.Add("FREETEXT");
            hs.Add("FREETEXTTABLE");
            hs.Add("FROM");
            hs.Add("FULL");
            hs.Add("FULLTEXTTABLE");
            hs.Add("FUNCTION");
            hs.Add("FUSION");
            hs.Add("GENERAL");
            hs.Add("GET");
            hs.Add("GLOBAL");
            hs.Add("GO");
            hs.Add("GOTO");
            hs.Add("GRANT");
            hs.Add("GROUP");
            hs.Add("GROUPING");
            hs.Add("HAVING");
            hs.Add("HOLD");
            hs.Add("HOLDLOCK");
            hs.Add("HOST");
            hs.Add("HOUR");
            hs.Add("IDENTITY");
            hs.Add("IDENTITY_INSERT");
            hs.Add("IDENTITYCOL");
            hs.Add("IF");
            hs.Add("IGNORE");
            hs.Add("IMMEDIATE");
            hs.Add("IN");
            hs.Add("INCLUDE");
            hs.Add("INDEX");
            hs.Add("INDICATOR");
            hs.Add("INITIALIZE");
            hs.Add("INITIALLY");
            hs.Add("INNER");
            hs.Add("INOUT");
            hs.Add("INPUT");
            hs.Add("INSENSITIVE");
            hs.Add("INSERT");
            hs.Add("INT");
            hs.Add("INTEGER");
            hs.Add("INTERSECT");
            hs.Add("INTERSECTION");
            hs.Add("INTERVAL");
            hs.Add("INTO");
            hs.Add("IS");
            hs.Add("ISOLATION");
            hs.Add("ITERATE");
            hs.Add("JOIN");
            hs.Add("KEY");
            hs.Add("KILL");
            hs.Add("LANGUAGE");
            hs.Add("LARGE");
            hs.Add("LAST");
            hs.Add("LATERAL");
            hs.Add("LEADING");
            hs.Add("LEFT");
            hs.Add("LESS");
            hs.Add("LEVEL");
            hs.Add("LIKE");
            hs.Add("LIKE_REGEX");
            hs.Add("LIMIT");
            hs.Add("LINENO");
            hs.Add("LN");
            hs.Add("LOAD");
            hs.Add("LOCAL");
            hs.Add("LOCALTIME");
            hs.Add("LOCALTIMESTAMP");
            hs.Add("LOCATOR");
            hs.Add("LOWER");
            hs.Add("MAP");
            hs.Add("MATCH");
            hs.Add("MAX");
            hs.Add("MEMBER");
            hs.Add("MERGE");
            hs.Add("METHOD");
            hs.Add("MIN");
            hs.Add("MINUTE");
            hs.Add("MOD");
            hs.Add("MODIFIES");
            hs.Add("MODIFY");
            hs.Add("MODULE");
            hs.Add("MONTH");
            hs.Add("MULTISET");
            hs.Add("NAMES");
            hs.Add("NATIONAL");
            hs.Add("NATURAL");
            hs.Add("NCHAR");
            hs.Add("NCLOB");
            hs.Add("NEW");
            hs.Add("NEXT");
            hs.Add("NO");
            hs.Add("NOCHECK");
            hs.Add("NONCLUSTERED");
            hs.Add("NONE");
            hs.Add("NORMALIZE");
            hs.Add("NOT");
            hs.Add("NULL");
            hs.Add("NULLIF");
            hs.Add("NUMERIC");
            hs.Add("OBJECT");
            hs.Add("OCCURRENCES_REGEX");
            hs.Add("OCTET_LENGTH");
            hs.Add("OF");
            hs.Add("OFF");
            hs.Add("OFFSETS");
            hs.Add("OLD");
            hs.Add("ON");
            hs.Add("ONLY");
            hs.Add("OPEN");
            hs.Add("OPENDATASOURCE");
            hs.Add("OPENQUERY");
            hs.Add("OPENROWSET");
            hs.Add("OPENXML");
            hs.Add("OPERATION");
            hs.Add("OPTION");
            hs.Add("OR");
            hs.Add("ORDER");
            hs.Add("ORDINALITY");
            hs.Add("OUT");
            hs.Add("OUTER");
            hs.Add("OUTPUT");
            hs.Add("OVER");
            hs.Add("OVERLAPS");
            hs.Add("OVERLAY");
            hs.Add("PAD");
            hs.Add("PARAMETER");
            hs.Add("PARAMETERS");
            hs.Add("PARTIAL");
            hs.Add("PARTITION");
            hs.Add("PASCAL");
            hs.Add("PATH");
            hs.Add("PERCENT");
            hs.Add("PERCENT_RANK");
            hs.Add("PERCENTILE_CONT");
            hs.Add("PERCENTILE_DISC");
            hs.Add("PIVOT");
            hs.Add("PLAN");
            hs.Add("POSITION");
            hs.Add("POSITION_REGEX");
            hs.Add("POSTFIX");
            hs.Add("PRECISION");
            hs.Add("PREFIX");
            hs.Add("PREORDER");
            hs.Add("PREPARE");
            hs.Add("PRESERVE");
            hs.Add("PRIMARY");
            hs.Add("PRINT");
            hs.Add("PRIOR");
            hs.Add("PRIVILEGES");
            hs.Add("PROC");
            hs.Add("PROCEDURE");
            hs.Add("PUBLIC");
            hs.Add("RAISERROR");
            hs.Add("RANGE");
            hs.Add("READ");
            hs.Add("READS");
            hs.Add("READTEXT");
            hs.Add("REAL");
            hs.Add("RECONFIGURE");
            hs.Add("RECURSIVE");
            hs.Add("REF");
            hs.Add("REFERENCES");
            hs.Add("REFERENCING");
            hs.Add("REGR_AVGX");
            hs.Add("REGR_AVGY");
            hs.Add("REGR_COUNT");
            hs.Add("REGR_INTERCEPT");
            hs.Add("REGR_R2");
            hs.Add("REGR_SLOPE");
            hs.Add("REGR_SXX");
            hs.Add("REGR_SXY");
            hs.Add("REGR_SYY");
            hs.Add("RELATIVE");
            hs.Add("RELEASE");
            hs.Add("REPLICATION");
            hs.Add("RESTORE");
            hs.Add("RESTRICT");
            hs.Add("RESULT");
            hs.Add("RETURN");
            hs.Add("RETURNS");
            hs.Add("REVERT");
            hs.Add("REVOKE");
            hs.Add("RIGHT");
            hs.Add("ROLE");
            hs.Add("ROLLBACK");
            hs.Add("ROLLUP");
            hs.Add("ROUTINE");
            hs.Add("ROW");
            hs.Add("ROWCOUNT");
            hs.Add("ROWGUIDCOL");
            hs.Add("ROWS");
            hs.Add("RULE");
            hs.Add("SAVE");
            hs.Add("SAVEPOINT");
            hs.Add("SCHEMA");
            hs.Add("SCOPE");
            hs.Add("SCROLL");
            hs.Add("SEARCH");
            hs.Add("SECOND");
            hs.Add("SECTION");
            hs.Add("SECURITYAUDIT");
            hs.Add("SELECT");
            hs.Add("SEMANTICKEYPHRASETABLE");
            hs.Add("SEMANTICSIMILARITYDETAILSTABLE");
            hs.Add("SEMANTICSIMILARITYTABLE");
            hs.Add("SENSITIVE");
            hs.Add("SEQUENCE");
            hs.Add("SESSION");
            hs.Add("SESSION_USER");
            hs.Add("SET");
            hs.Add("SETS");
            hs.Add("SETUSER");
            hs.Add("SHUTDOWN");
            hs.Add("SIMILAR");
            hs.Add("SIZE");
            hs.Add("SMALLINT");
            hs.Add("SOME");
            hs.Add("SPACE");
            hs.Add("SPECIFIC");
            hs.Add("SPECIFICTYPE");
            hs.Add("SQL");
            hs.Add("SQLCA");
            hs.Add("SQLCODE");
            hs.Add("SQLERROR");
            hs.Add("SQLEXCEPTION");
            hs.Add("SQLSTATE");
            hs.Add("SQLWARNING");
            hs.Add("START");
            hs.Add("STATE");
            hs.Add("STATEMENT");
            hs.Add("STATIC");
            hs.Add("STATISTICS");
            hs.Add("STDDEV_POP");
            hs.Add("STDDEV_SAMP");
            hs.Add("STRUCTURE");
            hs.Add("SUBMULTISET");
            hs.Add("SUBSTRING");
            hs.Add("SUBSTRING_REGEX");
            hs.Add("SUM");
            hs.Add("SYMMETRIC");
            hs.Add("SYSTEM");
            hs.Add("SYSTEM_USER");
            hs.Add("TABLE");
            hs.Add("TABLESAMPLE");
            hs.Add("TEMPORARY");
            hs.Add("TERMINATE");
            hs.Add("TEXTSIZE");
            hs.Add("THAN");
            hs.Add("THEN");
            hs.Add("TIME");
            hs.Add("TIMESTAMP");
            hs.Add("TIMEZONE_HOUR");
            hs.Add("TIMEZONE_MINUTE");
            hs.Add("TO");
            hs.Add("TOP");
            hs.Add("TRAILING");
            hs.Add("TRAN");
            hs.Add("TRANSACTION");
            hs.Add("TRANSLATE");
            hs.Add("TRANSLATE_REGEX");
            hs.Add("TRANSLATION");
            hs.Add("TREAT");
            hs.Add("TRIGGER");
            hs.Add("TRIM");
            hs.Add("TRUE");
            hs.Add("TRUNCATE");
            hs.Add("TRY_CONVERT");
            hs.Add("TSEQUAL");
            hs.Add("UESCAPE");
            hs.Add("UNDER");
            hs.Add("UNION");
            hs.Add("UNIQUE");
            hs.Add("UNKNOWN");
            hs.Add("UNNEST");
            hs.Add("UNPIVOT");
            hs.Add("UPDATE");
            hs.Add("UPDATETEXT");
            hs.Add("UPPER");
            hs.Add("USAGE");
            hs.Add("USE");
            hs.Add("USER");
            hs.Add("USING");
            hs.Add("VALUE");
            hs.Add("VALUES");
            hs.Add("VAR_POP");
            hs.Add("VAR_SAMP");
            hs.Add("VARCHAR");
            hs.Add("VARIABLE");
            hs.Add("VARYING");
            hs.Add("VIEW");
            hs.Add("WAITFOR");
            hs.Add("WHEN");
            hs.Add("WHENEVER");
            hs.Add("WHERE");
            hs.Add("WHILE");
            hs.Add("WIDTH_BUCKET");
            hs.Add("WINDOW");
            hs.Add("WITH");
            hs.Add("WITHIN");
            hs.Add("WITHINGROUP");
            hs.Add("WITHOUT");
            hs.Add("WORK");
            hs.Add("WRITE");
            hs.Add("WRITETEXT");
            hs.Add("XMLAGG");
            hs.Add("XMLATTRIBUTES");
            hs.Add("XMLBINARY");
            hs.Add("XMLCAST");
            hs.Add("XMLCOMMENT");
            hs.Add("XMLCONCAT");
            hs.Add("XMLDOCUMENT");
            hs.Add("XMLELEMENT");
            hs.Add("XMLEXISTS");
            hs.Add("XMLFOREST");
            hs.Add("XMLITERATE");
            hs.Add("XMLNAMESPACES");
            hs.Add("XMLPARSE");
            hs.Add("XMLPI");
            hs.Add("XMLQUERY");
            hs.Add("XMLSERIALIZE");
            hs.Add("XMLTABLE");
            hs.Add("XMLTEXT");
            hs.Add("XMLVALIDATE");
            hs.Add("YEAR");
            hs.Add("ZONE");

            return hs;
        }

        protected override HashSet<string> GetCurrentKeywords()
        {
            HashSet<string> hs = new HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
            hs.Add("ADD");
            hs.Add("ALL");
            hs.Add("ALTER");
            hs.Add("AND");
            hs.Add("ANY");
            hs.Add("AS");
            hs.Add("ASC");
            hs.Add("AUTHORIZATION");
            hs.Add("BACKUP");
            hs.Add("BEGIN");
            hs.Add("BETWEEN");
            hs.Add("BREAK");
            hs.Add("BROWSE");
            hs.Add("BULK");
            hs.Add("BY");
            hs.Add("CASCADE");
            hs.Add("CASE");
            hs.Add("CHECK");
            hs.Add("CHECKPOINT");
            hs.Add("CLOSE");
            hs.Add("CLUSTERED");
            hs.Add("COALESCE");
            hs.Add("COLLATE");
            hs.Add("COLUMN");
            hs.Add("COMMIT");
            hs.Add("COMPUTE");
            hs.Add("CONSTRAINT");
            hs.Add("CONTAINS");
            hs.Add("CONTAINSTABLE");
            hs.Add("CONTINUE");
            hs.Add("CONVERT");
            hs.Add("CREATE");
            hs.Add("CROSS");
            hs.Add("CURRENT");
            hs.Add("CURRENT_DATE");
            hs.Add("CURRENT_TIME");
            hs.Add("CURRENT_TIMESTAMP");
            hs.Add("CURRENT_USER");
            hs.Add("CURSOR");
            hs.Add("DATABASE");
            hs.Add("DBCC");
            hs.Add("DEALLOCATE");
            hs.Add("DECLARE");
            hs.Add("DEFAULT");
            hs.Add("DELETE");
            hs.Add("DENY");
            hs.Add("DESC");
            hs.Add("DISK");
            hs.Add("DISTINCT");
            hs.Add("DISTRIBUTED");
            hs.Add("DOUBLE");
            hs.Add("DROP");
            hs.Add("DUMP");
            hs.Add("ELSE");
            hs.Add("END");
            hs.Add("ERRLVL");
            hs.Add("ESCAPE");
            hs.Add("EXCEPT");
            hs.Add("EXEC");
            hs.Add("EXECUTE");
            hs.Add("EXISTS");
            hs.Add("EXIT");
            hs.Add("EXTERNAL");
            hs.Add("FETCH");
            hs.Add("FILE");
            hs.Add("FILLFACTOR");
            hs.Add("FOR");
            hs.Add("FOREIGN");
            hs.Add("FREETEXT");
            hs.Add("FREETEXTTABLE");
            hs.Add("FROM");
            hs.Add("FULL");
            hs.Add("FUNCTION");
            hs.Add("GOTO");
            hs.Add("GRANT");
            hs.Add("GROUP");
            hs.Add("HAVING");
            hs.Add("HOLDLOCK");
            hs.Add("IDENTITY");
            hs.Add("IDENTITY_INSERT");
            hs.Add("IDENTITYCOL");
            hs.Add("IF");
            hs.Add("IN");
            hs.Add("INDEX");
            hs.Add("INNER");
            hs.Add("INSERT");
            hs.Add("INTERSECT");
            hs.Add("INTO");
            hs.Add("IS");
            hs.Add("JOIN");
            hs.Add("KEY");
            hs.Add("KILL");
            hs.Add("LEFT");
            hs.Add("LIKE");
            hs.Add("LINENO");
            hs.Add("LOAD");
            hs.Add("MERGE");
            hs.Add("NATIONAL");
            hs.Add("NOCHECK");
            hs.Add("NONCLUSTERED");
            hs.Add("NOT");
            hs.Add("NULL");
            hs.Add("NULLIF");
            hs.Add("OF");
            hs.Add("OFF");
            hs.Add("OFFSETS");
            hs.Add("ON");
            hs.Add("OPEN");
            hs.Add("OPENDATASOURCE");
            hs.Add("OPENQUERY");
            hs.Add("OPENROWSET");
            hs.Add("OPENXML");
            hs.Add("OPTION");
            hs.Add("OR");
            hs.Add("ORDER");
            hs.Add("OUTER");
            hs.Add("OVER");
            hs.Add("PERCENT");
            hs.Add("PIVOT");
            hs.Add("PLAN");
            hs.Add("PRECISION");
            hs.Add("PRIMARY");
            hs.Add("PRINT");
            hs.Add("PROC");
            hs.Add("PROCEDURE");
            hs.Add("PUBLIC");
            hs.Add("RAISERROR");
            hs.Add("READ");
            hs.Add("READTEXT");
            hs.Add("RECONFIGURE");
            hs.Add("REFERENCES");
            hs.Add("REPLICATION");
            hs.Add("RESTORE");
            hs.Add("RESTRICT");
            hs.Add("RETURN");
            hs.Add("REVERT");
            hs.Add("REVOKE");
            hs.Add("RIGHT");
            hs.Add("ROLLBACK");
            hs.Add("ROWCOUNT");
            hs.Add("ROWGUIDCOL");
            hs.Add("RULE");
            hs.Add("SAVE");
            hs.Add("SCHEMA");
            hs.Add("SECURITYAUDIT");
            hs.Add("SELECT");
            hs.Add("SEMANTICKEYPHRASETABLE");
            hs.Add("SEMANTICSIMILARITYDETAILSTABLE");
            hs.Add("SEMANTICSIMILARITYTABLE");
            hs.Add("SESSION_USER");
            hs.Add("SET");
            hs.Add("SETUSER");
            hs.Add("SHUTDOWN");
            hs.Add("SOME");
            hs.Add("STATISTICS");
            hs.Add("SYSTEM_USER");
            hs.Add("TABLE");
            hs.Add("TABLESAMPLE");
            hs.Add("TEXTSIZE");
            hs.Add("THEN");
            hs.Add("TO");
            hs.Add("TOP");
            hs.Add("TRAN");
            hs.Add("TRANSACTION");
            hs.Add("TRIGGER");
            hs.Add("TRUNCATE");
            hs.Add("TRY_CONVERT");
            hs.Add("TSEQUAL");
            hs.Add("UNION");
            hs.Add("UNIQUE");
            hs.Add("UNPIVOT");
            hs.Add("UPDATE");
            hs.Add("UPDATETEXT");
            hs.Add("USE");
            hs.Add("USER");
            hs.Add("VALUES");
            hs.Add("VARYING");
            hs.Add("VIEW");
            hs.Add("WAITFOR");
            hs.Add("WHEN");
            hs.Add("WHERE");
            hs.Add("WHILE");
            hs.Add("WITH");
            hs.Add("WITHIN GROUP");
            hs.Add("WRITETEXT");

            return hs;
        }

        protected override HashSet<string> GetOdbcKeywords()
        {
            HashSet<string> hs = new HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
            hs.Add("ABSOLUTE");
            hs.Add("ACTION");
            hs.Add("ADA");
            hs.Add("ADD");
            hs.Add("ALL");
            hs.Add("ALLOCATE");
            hs.Add("ALTER");
            hs.Add("AND");
            hs.Add("ANY");
            hs.Add("ARE");
            hs.Add("AS");
            hs.Add("ASC");
            hs.Add("ASSERTION");
            hs.Add("AT");
            hs.Add("AUTHORIZATION");
            hs.Add("AVG");
            hs.Add("BEGIN");
            hs.Add("BETWEEN");
            hs.Add("BIT");
            hs.Add("BIT_LENGTH");
            hs.Add("BOTH");
            hs.Add("BY");
            hs.Add("CASCADE");
            hs.Add("CASCADED");
            hs.Add("CASE");
            hs.Add("CAST");
            hs.Add("CATALOG");
            hs.Add("CHAR");
            hs.Add("CHAR_LENGTH");
            hs.Add("CHARACTER");
            hs.Add("CHARACTER_LENGTH");
            hs.Add("CHECK");
            hs.Add("CLOSE");
            hs.Add("COALESCE");
            hs.Add("COLLATE");
            hs.Add("COLLATION");
            hs.Add("COLUMN");
            hs.Add("COMMIT");
            hs.Add("CONNECT");
            hs.Add("CONNECTION");
            hs.Add("CONSTRAINT");
            hs.Add("CONSTRAINTS");
            hs.Add("CONTINUE");
            hs.Add("CONVERT");
            hs.Add("CORRESPONDING");
            hs.Add("COUNT");
            hs.Add("CREATE");
            hs.Add("CROSS");
            hs.Add("CURRENT");
            hs.Add("CURRENT_DATE");
            hs.Add("CURRENT_TIME");
            hs.Add("CURRENT_TIMESTAMP");
            hs.Add("CURRENT_USER");
            hs.Add("CURSOR");
            hs.Add("DATE");
            hs.Add("DAY");
            hs.Add("DEALLOCATE");
            hs.Add("DEC");
            hs.Add("DECIMAL");
            hs.Add("DECLARE");
            hs.Add("DEFAULT");
            hs.Add("DEFERRABLE");
            hs.Add("DEFERRED");
            hs.Add("DELETE");
            hs.Add("DESC");
            hs.Add("DESCRIBE");
            hs.Add("DESCRIPTOR");
            hs.Add("DIAGNOSTICS");
            hs.Add("DISCONNECT");
            hs.Add("DISTINCT");
            hs.Add("DOMAIN");
            hs.Add("DOUBLE");
            hs.Add("DROP");
            hs.Add("ELSE");
            hs.Add("END");
            hs.Add("END-EXEC");
            hs.Add("ESCAPE");
            hs.Add("EXCEPT");
            hs.Add("EXCEPTION");
            hs.Add("EXEC");
            hs.Add("EXECUTE");
            hs.Add("EXISTS");
            hs.Add("EXTERNAL");
            hs.Add("EXTRACT");
            hs.Add("FALSE");
            hs.Add("FETCH");
            hs.Add("FIRST");
            hs.Add("FLOAT");
            hs.Add("FOR");
            hs.Add("FOREIGN");
            hs.Add("FORTRAN");
            hs.Add("FOUND");
            hs.Add("FROM");
            hs.Add("FULL");
            hs.Add("GET");
            hs.Add("GLOBAL");
            hs.Add("GO");
            hs.Add("GOTO");
            hs.Add("GRANT");
            hs.Add("GROUP");
            hs.Add("HAVING");
            hs.Add("HOUR");
            hs.Add("IDENTITY");
            hs.Add("IMMEDIATE");
            hs.Add("IN");
            hs.Add("INCLUDE");
            hs.Add("INDEX");
            hs.Add("INDICATOR");
            hs.Add("INITIALLY");
            hs.Add("INNER");
            hs.Add("INPUT");
            hs.Add("INSENSITIVE");
            hs.Add("INSERT");
            hs.Add("INT");
            hs.Add("INTEGER");
            hs.Add("INTERSECT");
            hs.Add("INTERVAL");
            hs.Add("INTO");
            hs.Add("IS");
            hs.Add("ISOLATION");
            hs.Add("JOIN");
            hs.Add("KEY");
            hs.Add("LANGUAGE");
            hs.Add("LAST");
            hs.Add("LEADING");
            hs.Add("LEFT");
            hs.Add("LEVEL");
            hs.Add("LIKE");
            hs.Add("LOCAL");
            hs.Add("LOWER");
            hs.Add("MATCH");
            hs.Add("MAX");
            hs.Add("MIN");
            hs.Add("MINUTE");
            hs.Add("MODULE");
            hs.Add("MONTH");
            hs.Add("NAMES");
            hs.Add("NATIONAL");
            hs.Add("NATURAL");
            hs.Add("NCHAR");
            hs.Add("NEXT");
            hs.Add("NO");
            hs.Add("NONE");
            hs.Add("NOT");
            hs.Add("NULL");
            hs.Add("NULLIF");
            hs.Add("NUMERIC");
            hs.Add("OCTET_LENGTH");
            hs.Add("OF");
            hs.Add("ON");
            hs.Add("ONLY");
            hs.Add("OPEN");
            hs.Add("OPTION");
            hs.Add("OR");
            hs.Add("ORDER");
            hs.Add("OUTER");
            hs.Add("OUTPUT");
            hs.Add("OVERLAPS");
            hs.Add("PAD");
            hs.Add("PARTIAL");
            hs.Add("PASCAL");
            hs.Add("POSITION");
            hs.Add("PRECISION");
            hs.Add("PREPARE");
            hs.Add("PRESERVE");
            hs.Add("PRIMARY");
            hs.Add("PRIOR");
            hs.Add("PRIVILEGES");
            hs.Add("PROCEDURE");
            hs.Add("PUBLIC");
            hs.Add("READ");
            hs.Add("REAL");
            hs.Add("REFERENCES");
            hs.Add("RELATIVE");
            hs.Add("RESTRICT");
            hs.Add("REVOKE");
            hs.Add("RIGHT");
            hs.Add("ROLLBACK");
            hs.Add("ROWS");
            hs.Add("SCHEMA");
            hs.Add("SCROLL");
            hs.Add("SECOND");
            hs.Add("SECTION");
            hs.Add("SELECT");
            hs.Add("SESSION");
            hs.Add("SESSION_USER");
            hs.Add("SET");
            hs.Add("SIZE");
            hs.Add("SMALLINT");
            hs.Add("SOME");
            hs.Add("SPACE");
            hs.Add("SQL");
            hs.Add("SQLCA");
            hs.Add("SQLCODE");
            hs.Add("SQLERROR");
            hs.Add("SQLSTATE");
            hs.Add("SQLWARNING");
            hs.Add("SUBSTRING");
            hs.Add("SUM");
            hs.Add("SYSTEM_USER");
            hs.Add("TABLE");
            hs.Add("TEMPORARY");
            hs.Add("THEN");
            hs.Add("TIME");
            hs.Add("TIMESTAMP");
            hs.Add("TIMEZONE_HOUR");
            hs.Add("TIMEZONE_MINUTE");
            hs.Add("TO");
            hs.Add("TRAILING");
            hs.Add("TRANSACTION");
            hs.Add("TRANSLATE");
            hs.Add("TRANSLATION");
            hs.Add("TRIM");
            hs.Add("TRUE");
            hs.Add("UNION");
            hs.Add("UNIQUE");
            hs.Add("UNKNOWN");
            hs.Add("UPDATE");
            hs.Add("UPPER");
            hs.Add("USAGE");
            hs.Add("USER");
            hs.Add("USING");
            hs.Add("VALUE");
            hs.Add("VALUES");
            hs.Add("VARCHAR");
            hs.Add("VARYING");
            hs.Add("VIEW");
            hs.Add("WHEN");
            hs.Add("WHENEVER");
            hs.Add("WHERE");
            hs.Add("WITH");
            hs.Add("WORK");
            hs.Add("WRITE");
            hs.Add("YEAR");
            hs.Add("ZONE");

            return hs;
        }


        protected override HashSet<string> GetFutureKeywords()
        {
            HashSet<string> hs = new HashSet<string>(System.StringComparer.InvariantCultureIgnoreCase);
            hs.Add("ABSOLUTE");
            hs.Add("ACTION");
            hs.Add("ADMIN");
            hs.Add("AFTER");
            hs.Add("AGGREGATE");
            hs.Add("ALIAS");
            hs.Add("ALLOCATE");
            hs.Add("ARE");
            hs.Add("ARRAY");
            hs.Add("ASENSITIVE");
            hs.Add("ASSERTION");
            hs.Add("ASYMMETRIC");
            hs.Add("AT");
            hs.Add("ATOMIC");
            hs.Add("BEFORE");
            hs.Add("BINARY");
            hs.Add("BIT");
            hs.Add("BLOB");
            hs.Add("BOOLEAN");
            hs.Add("BOTH");
            hs.Add("BREADTH");
            hs.Add("CALL");
            hs.Add("CALLED");
            hs.Add("CARDINALITY");
            hs.Add("CASCADED");
            hs.Add("CAST");
            hs.Add("CATALOG");
            hs.Add("CHAR");
            hs.Add("CHARACTER");
            hs.Add("CLASS");
            hs.Add("CLOB");
            hs.Add("COLLATION");
            hs.Add("COLLECT");
            hs.Add("COMPLETION");
            hs.Add("CONDITION");
            hs.Add("CONNECT");
            hs.Add("CONNECTION");
            hs.Add("CONSTRAINTS");
            hs.Add("CONSTRUCTOR");
            hs.Add("CORR");
            hs.Add("CORRESPONDING");
            hs.Add("COVAR_POP");
            hs.Add("COVAR_SAMP");
            hs.Add("CUBE");
            hs.Add("CUME_DIST");
            hs.Add("CURRENT_CATALOG");
            hs.Add("CURRENT_DEFAULT_TRANSFORM_GROUP");
            hs.Add("CURRENT_PATH");
            hs.Add("CURRENT_ROLE");
            hs.Add("CURRENT_SCHEMA");
            hs.Add("CURRENT_TRANSFORM_GROUP_FOR_TYPE");
            hs.Add("CYCLE");
            hs.Add("DATA");
            hs.Add("DATE");
            hs.Add("DAY");
            hs.Add("DEC");
            hs.Add("DECIMAL");
            hs.Add("DEFERRABLE");
            hs.Add("DEFERRED");
            hs.Add("DEPTH");
            hs.Add("DEREF");
            hs.Add("DESCRIBE");
            hs.Add("DESCRIPTOR");
            hs.Add("DESTROY");
            hs.Add("DESTRUCTOR");
            hs.Add("DETERMINISTIC");
            hs.Add("DIAGNOSTICS");
            hs.Add("DICTIONARY");
            hs.Add("DISCONNECT");
            hs.Add("DOMAIN");
            hs.Add("DYNAMIC");
            hs.Add("EACH");
            hs.Add("ELEMENT");
            hs.Add("END-EXEC");
            hs.Add("EQUALS");
            hs.Add("EVERY");
            hs.Add("EXCEPTION");
            hs.Add("FALSE");
            hs.Add("FILTER");
            hs.Add("FIRST");
            hs.Add("FLOAT");
            hs.Add("FOUND");
            hs.Add("FREE");
            hs.Add("FULLTEXTTABLE");
            hs.Add("FUSION");
            hs.Add("GENERAL");
            hs.Add("GET");
            hs.Add("GLOBAL");
            hs.Add("GO");
            hs.Add("GROUPING");
            hs.Add("HOLD");
            hs.Add("HOST");
            hs.Add("HOUR");
            hs.Add("IGNORE");
            hs.Add("IMMEDIATE");
            hs.Add("INDICATOR");
            hs.Add("INITIALIZE");
            hs.Add("INITIALLY");
            hs.Add("INOUT");
            hs.Add("INPUT");
            hs.Add("INT");
            hs.Add("INTEGER");
            hs.Add("INTERSECTION");
            hs.Add("INTERVAL");
            hs.Add("ISOLATION");
            hs.Add("ITERATE");
            hs.Add("LANGUAGE");
            hs.Add("LARGE");
            hs.Add("LAST");
            hs.Add("LATERAL");
            hs.Add("LEADING");
            hs.Add("LESS");
            hs.Add("LEVEL");
            hs.Add("LIKE_REGEX");
            hs.Add("LIMIT");
            hs.Add("LN");
            hs.Add("LOCAL");
            hs.Add("LOCALTIME");
            hs.Add("LOCALTIMESTAMP");
            hs.Add("LOCATOR");
            hs.Add("MAP");
            hs.Add("MATCH");
            hs.Add("MEMBER");
            hs.Add("METHOD");
            hs.Add("MINUTE");
            hs.Add("MOD");
            hs.Add("MODIFIES");
            hs.Add("MODIFY");
            hs.Add("MODULE");
            hs.Add("MONTH");
            hs.Add("MULTISET");
            hs.Add("NAMES");
            hs.Add("NATURAL");
            hs.Add("NCHAR");
            hs.Add("NCLOB");
            hs.Add("NEW");
            hs.Add("NEXT");
            hs.Add("NO");
            hs.Add("NONE");
            hs.Add("NORMALIZE");
            hs.Add("NUMERIC");
            hs.Add("OBJECT");
            hs.Add("OCCURRENCES_REGEX");
            hs.Add("OLD");
            hs.Add("ONLY");
            hs.Add("OPERATION");
            hs.Add("ORDINALITY");
            hs.Add("OUT");
            hs.Add("OUTPUT");
            hs.Add("OVERLAY");
            hs.Add("PAD");
            hs.Add("PARAMETER");
            hs.Add("PARAMETERS");
            hs.Add("PARTIAL");
            hs.Add("PARTITION");
            hs.Add("PATH");
            hs.Add("PERCENT_RANK");
            hs.Add("PERCENTILE_CONT");
            hs.Add("PERCENTILE_DISC");
            hs.Add("POSITION_REGEX");
            hs.Add("POSTFIX");
            hs.Add("PREFIX");
            hs.Add("PREORDER");
            hs.Add("PREPARE");
            hs.Add("PRESERVE");
            hs.Add("PRIOR");
            hs.Add("PRIVILEGES");
            hs.Add("RANGE");
            hs.Add("READS");
            hs.Add("REAL");
            hs.Add("RECURSIVE");
            hs.Add("REF");
            hs.Add("REFERENCING");
            hs.Add("REGR_AVGX");
            hs.Add("REGR_AVGY");
            hs.Add("REGR_COUNT");
            hs.Add("REGR_INTERCEPT");
            hs.Add("REGR_R2");
            hs.Add("REGR_SLOPE");
            hs.Add("REGR_SXX");
            hs.Add("REGR_SXY");
            hs.Add("REGR_SYY");
            hs.Add("RELATIVE");
            hs.Add("RELEASE");
            hs.Add("RESULT");
            hs.Add("RETURNS");
            hs.Add("ROLE");
            hs.Add("ROLLUP");
            hs.Add("ROUTINE");
            hs.Add("ROW");
            hs.Add("ROWS");
            hs.Add("SAVEPOINT");
            hs.Add("SCOPE");
            hs.Add("SCROLL");
            hs.Add("SEARCH");
            hs.Add("SECOND");
            hs.Add("SECTION");
            hs.Add("SENSITIVE");
            hs.Add("SEQUENCE");
            hs.Add("SESSION");
            hs.Add("SETS");
            hs.Add("SIMILAR");
            hs.Add("SIZE");
            hs.Add("SMALLINT");
            hs.Add("SPACE");
            hs.Add("SPECIFIC");
            hs.Add("SPECIFICTYPE");
            hs.Add("SQL");
            hs.Add("SQLEXCEPTION");
            hs.Add("SQLSTATE");
            hs.Add("SQLWARNING");
            hs.Add("START");
            hs.Add("STATE");
            hs.Add("STATEMENT");
            hs.Add("STATIC");
            hs.Add("STDDEV_POP");
            hs.Add("STDDEV_SAMP");
            hs.Add("STRUCTURE");
            hs.Add("SUBMULTISET");
            hs.Add("SUBSTRING_REGEX");
            hs.Add("SYMMETRIC");
            hs.Add("SYSTEM");
            hs.Add("TEMPORARY");
            hs.Add("TERMINATE");
            hs.Add("THAN");
            hs.Add("TIME");
            hs.Add("TIMESTAMP");
            hs.Add("TIMEZONE_HOUR");
            hs.Add("TIMEZONE_MINUTE");
            hs.Add("TRAILING");
            hs.Add("TRANSLATE_REGEX");
            hs.Add("TRANSLATION");
            hs.Add("TREAT");
            hs.Add("TRUE");
            hs.Add("UESCAPE");
            hs.Add("UNDER");
            hs.Add("UNKNOWN");
            hs.Add("UNNEST");
            hs.Add("USAGE");
            hs.Add("USING");
            hs.Add("VALUE");
            hs.Add("VAR_POP");
            hs.Add("VAR_SAMP");
            hs.Add("VARCHAR");
            hs.Add("VARIABLE");
            hs.Add("WHENEVER");
            hs.Add("WIDTH_BUCKET");
            hs.Add("WINDOW");
            hs.Add("WITHIN");
            hs.Add("WITHOUT");
            hs.Add("WORK");
            hs.Add("WRITE");
            hs.Add("XMLAGG");
            hs.Add("XMLATTRIBUTES");
            hs.Add("XMLBINARY");
            hs.Add("XMLCAST");
            hs.Add("XMLCOMMENT");
            hs.Add("XMLCONCAT");
            hs.Add("XMLDOCUMENT");
            hs.Add("XMLELEMENT");
            hs.Add("XMLEXISTS");
            hs.Add("XMLFOREST");
            hs.Add("XMLITERATE");
            hs.Add("XMLNAMESPACES");
            hs.Add("XMLPARSE");
            hs.Add("XMLPI");
            hs.Add("XMLQUERY");
            hs.Add("XMLSERIALIZE");
            hs.Add("XMLTABLE");
            hs.Add("XMLTEXT");
            hs.Add("XMLVALIDATE");
            hs.Add("YEAR");
            hs.Add("ZONE");

            return hs;
        }


    } // End Class


} // End Namespace
