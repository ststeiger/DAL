 
 --OBJECT OR COLUMN
 
 -- Checking for dependencies and dependency order in versions prior to SQL Server 2008 was not very reliable.  
 -- We had to write scripts using sp_depends 
 -- and then in SQL Server 2005, we used sys.sql_dependencies but both are not reliable.  
 -- In SQL Server 2008, the support is much more robust – the only exception 
 -- that we have seen so far is that the dependencies in the dynamic SQL code or the CLR code are not covered.  
 -- The new objects that are used for writing the dependency SQL are:
 
 
SELECT 
	 sysrefing.referencing_schema_name 
	,sysrefing.referencing_entity_name 
	,sysrefing.referencing_id 
	,sysrefing.referencing_class_desc 
FROM sys.dm_sql_referencing_entities ('dbo.tfu_RPT_FM_DATA_DIN277_Grob_Parkplaetze', 'OBJECT') AS sysrefing 
GO

SELECT 
	 referenced_schema_name 
	,referenced_entity_name 
	,referenced_id 
	,referenced_class_desc 
FROM sys.dm_sql_referenced_entities ('dbo.tfu_RPT_FM_DATA_DIN277_Grob', 'OBJECT') AS sysrefed 
GO



SELECT
	 OBJECT_SCHEMA_NAME(sysdep.referencing_id) AS Referencing_Schema 
	,OBJECT_NAME(sysdep.referencing_id) AS Referencing_Object_Name 
	,sysdep.referenced_schema_name AS Referenced_Schema 
	,sysdep.referenced_entity_name AS Referenced_Object_Name 
FROM sys.sql_expression_dependencies AS sysdep;
