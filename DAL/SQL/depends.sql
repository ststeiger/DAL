SELECT
	 OBJECT_SCHEMA_NAME(sysdep.referencing_id) AS Referencing_Schema 
	,OBJECT_NAME(sysdep.referencing_id) AS Referencing_Object_Name 
	,sysdep.referenced_schema_name AS Referenced_Schema 
	,sysdep.referenced_entity_name AS Referenced_Object_Name 
FROM sys.sql_expression_dependencies AS sysdep 

WHERE sysdep.referenced_entity_name NOT IN (SELECT TABLE_NAME FROM information_schema.tables )
AND OBJECT_NAME(sysdep.referencing_id) IN 
(
	SELECT SPECIFIC_NAME FROM information_schema.routines 
	WHERE ROUTINE_TYPE = 'FUNCTION'
	AND DATA_TYPE = 'TABLE' 
)

ORDER BY Referencing_Object_Name 
