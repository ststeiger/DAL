
/*jshint curly: false */
/*jslint browser: true */


--http://outofhanwell.wordpress.com/2006/06/05/jslint-considered-harmful/


--http://bnshah.wordpress.ncsu.edu/jslint-final-project/




-- You can create local and global temporary tables. 
-- Local temporary tables are visible only in the current session; 
-- global temporary tables are visible to all sessions.
-- Prefix local temporary table names with single number sign (#table_name), 
-- and prefix global temporary table names with a double number sign (##table_name).

-- Table variables (DECLARE @t TABLE) are visible only to the connection that creates it, 
-- and are deleted when the batch or stored procedure ends.


-- A local temporary table created in a stored procedure 
-- is dropped automatically when the stored procedure completes. 
-- The table can be referenced by any nested stored procedures 
-- executed by the stored procedure that created the table. 
-- The table cannot be referenced by the process 
-- which called the stored procedure that created the table.

-- A local temporary table created within a stored procedure or trigger 
-- is distinct from a temporary table with the same name 
-- created before the stored procedure or trigger is called.


-- All other local temporary tables are dropped automatically 
-- at the end of the current session.

-- Local temporary tables are deleted when the scope that created them is closed. 

-- Global temporary tables (CREATE TABLE ##t) are visible to everyone, 
-- or available across all sessions, 
-- and are deleted when all connections that have referenced them have closed.

-- Global temporary tables (CREATE TABLE ##t) are visible to 
-- any user and any connection after they are created, 
-- and are deleted when all users that are referencing the table disconnect 
-- from the instance of SQL Server.


-- Global temporary tables are automatically dropped when the session that created the table ends 
-- and all other tasks have stopped referencing them. 
-- The association between a task and a table is maintained only 
-- for the life of a single Transact-SQL statement. 
-- This means that a global temporary table is dropped 
-- at the completion of the last Transact-SQL statement 
-- that was actively referencing the table when the creating session ended.



-- Tempdb permanent tables (USE tempdb CREATE TABLE t) 
-- are visible to everyone, and are deleted when the server is restarted.


-- What is the difference between a connection and a session? 
-- Connection = 1+ Session, e.g. exec in stored procedure (scope) 
-- A session is a physical container for information exchange.
-- Normally there is one session on each connection, 
-- but there could be multiple session on a single connection 
-- (Multiple Active Result Sets, MARS) 
-- and there are sessions that have no connection (SSB activated procedures, system sessions)
-- There are also connections w/o sessions, namely connections used for non-TDS purposes, 
-- like database mirroring sys.dm_db_mirroring_connections 
-- or Service Broker connections sys.dm_broker_connections.


-- Connection pooling (with any modern version of SQL Server) 
-- will call sp_reset_connection when reusing a connection. 
-- This stored proc, among other things, 
-- drops any temporary tables that the connection owns.
-- Global temp tables aren't scoped to a connection - so they would not be dropped. 



IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable
SELECT * 
INTO #tempTable 
FROM T_Benutzer;
SELECT * FROM #tempTable  
-- Same table doesn't give an error, as this is another session/scope 
EXEC('
SELECT * 
INTO #tempTable 
FROM T_Benutzer;
SELECT * FROM #tempTable  
')
IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable



--IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable

--EXEC('
--SELECT * 
--INTO #tempTable 
--FROM T_Benutzer;
--SELECT * FROM #tempTable  
--')

--SELECT * FROM #tempTable  

--IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable



IF OBJECT_ID('tempdb..##globalTempTable') IS NOT NULL DROP TABLE ##globalTempTable

EXEC('
SELECT * 
INTO ##globalTempTable 
FROM T_Benutzer;
SELECT * FROM ##globalTempTable  
')

SELECT * FROM ##globalTempTable  

IF OBJECT_ID('tempdb..##globalTempTable') IS NOT NULL DROP TABLE ##globalTempTable

