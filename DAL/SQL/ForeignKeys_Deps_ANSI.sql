
-- IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable;

;WITH FK_Dependencies AS 
(
	SELECT DISTINCT 
		 IST.TABLE_SCHEMA AS OnSchema 
		,IST.TABLE_NAME AS OnTable 
		,KCU2.TABLE_SCHEMA AS AgainstSchema 
		,KCU2.TABLE_NAME AS AgainstTable 
	-- INTO #tempTable 
	FROM information_schema.tables AS IST 

	LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KCU1 
		ON KCU1.TABLE_CATALOG = IST.TABLE_CATALOG  
		AND KCU1.TABLE_SCHEMA = IST.TABLE_SCHEMA 
		AND KCU1.TABLE_NAME = IST.TABLE_NAME 
		
	LEFT JOIN INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS AS RC 
		ON RC.CONSTRAINT_CATALOG = KCU1.CONSTRAINT_CATALOG 
		AND RC.CONSTRAINT_SCHEMA = KCU1.CONSTRAINT_SCHEMA 
		AND RC.CONSTRAINT_NAME = KCU1.CONSTRAINT_NAME 
		
	LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KCU2 
		ON KCU2.CONSTRAINT_CATALOG = RC.UNIQUE_CONSTRAINT_CATALOG  
		AND KCU2.CONSTRAINT_SCHEMA = RC.UNIQUE_CONSTRAINT_SCHEMA 
		AND KCU2.CONSTRAINT_NAME = RC.UNIQUE_CONSTRAINT_NAME 
		AND KCU2.ORDINAL_POSITION = KCU1.ORDINAL_POSITION 
		AND NOT 
		(
			KCU2.TABLE_SCHEMA = IST.TABLE_SCHEMA 
			AND 
			KCU2.TABLE_NAME = IST.TABLE_NAME 
		)
		
	WHERE TABLE_TYPE = 'BASE TABLE' 
	AND IST.TABLE_NAME NOT IN ('sysdiagrams') 
),



 
--;WITH 
FK_DependencyResolution AS 
( 
    -- base case 
    SELECT 
		 FK_Dependencies.OnSchema AS TableSchema 
        ,FK_Dependencies.OnTable AS TableName 
        ,1 AS Lvl 
	FROM FK_Dependencies AS FK_Dependencies 
    -- FROM #tempTable AS FK_Dependencies 
    WHERE 1=1 
	AND AgainstTable IS NULL 
    
    -- Recursive case
    UNION ALL 
    
    SELECT 
         d.OnSchema AS TableSchema 
        ,d.OnTable AS TableName 
        ,r.Lvl + 1 AS Lvl 
    FROM FK_Dependencies AS d 
    -- FROM #tempTable AS d 
    
    INNER JOIN FK_DependencyResolution AS r 
        ON r.TableName = d.AgainstTable 
        AND r.TableSchema = d.AgainstSchema 
) 

SELECT TOP 999999999999999999 
     MAX(Lvl) AS Lvl 
    ,TableSchema 
    ,TableName 
    ,'DELETE FROM ' + QUOTENAME(TableSchema) + '.' + QUOTENAME(TableName) + '; ' AS DeleteCmd 
FROM FK_DependencyResolution 

GROUP BY TableSchema, TableName 

ORDER BY lvl DESC, TableName 
OPTION (MAXRECURSION 0);

-- IF OBJECT_ID('tempdb..#tempTable') IS NOT NULL DROP TABLE #tempTable;
