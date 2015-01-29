
IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable
CREATE TABLE #tempTable
(
	[BE_ID] [int] NOT NULL,
	[BE_Name] [varchar](50) NULL,
	[BE_Vorname] [varchar](50) NULL,
	[BE_User] [varchar](50) NULL,
	[BE_Passwort] [varchar](50) NULL,
	[BE_Language] [varchar](5) NULL,
	[BE_Hash] [varchar](50) NULL,
	[BE_Level] [tinyint] NOT NULL,
	[BE_isLDAPSync] [bit] NOT NULL,
	[BE_Domaene] [varchar](255) NULL,
	[BE_Email] [varchar](150) NULL,
	[BE_TelNr] [varchar](20) NULL,
	[BE_CurrencyID] [int] NULL,
	[BE_Hide] [bit] NOT NULL,
)

--The best way is to create the temporary table with the correct schema, then use INSERT ... EXEC
INSERT INTO #tempTable EXEC __foobar 1
SELECT * FROM #tempTable 
IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable



-- global temp table
--DECLARE @sql VARCHAR(4000)
--SET @sql = 'INSERT ##MYaTable EXEC __foobar 123'
--EXEC (@sql)
---- SELECT * FROM ##MYTable


