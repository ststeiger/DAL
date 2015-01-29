
;WITH FK_Dependencies AS 
(
	SELECT  
		 OnSchema.name AS OnSchema 
		,OnTable.name AS OnTable 
		,AgainstSchema.name AS AgainstSchema 
		,AgainstTable.name AS AgainstTable 
	FROM sys.objects AS OnTable 
	
	LEFT JOIN sys.schemas AS OnSchema 
		ON OnSchema.schema_id = OnTable.schema_id 
		
	LEFT JOIN sys.foreign_keys AS fk 
		ON fk.parent_object_id = OnTable.object_id 
		
	LEFT JOIN sys.objects AS AgainstTable 
		ON AgainstTable.object_id = fk.referenced_object_id 
		AND AgainstTable.TYPE = 'U' 
		AND AgainstTable.Name <> OnTable.Name 
		
	LEFT JOIN sys.schemas AS AgainstSchema 
		ON AgainstSchema.schema_id = AgainstTable.schema_id 
		
	WHERE (1=1) 
	AND OnTable.TYPE = 'U'  
	AND OnTable.name NOT IN ('sysdiagrams') 
) 
,FK_DependencyResolution AS 
( 
    -- base case 
    SELECT 
		 FK_Dependencies.OnSchema AS TableSchema 
        ,FK_Dependencies.OnTable AS TableName 
        ,1 AS Lvl 
    FROM FK_Dependencies 
    WHERE 1=1 
	AND AgainstTable IS NULL 
    
    -- recursive case
    UNION ALL 
    
    SELECT 
         d.OnSchema AS TableSchema 
        ,d.OnTable AS TableName 
        ,r.Lvl + 1 AS Lvl 
    FROM FK_Dependencies AS d 
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
