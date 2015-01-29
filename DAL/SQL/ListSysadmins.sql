
SELECT 
	 login.name AS [Name] 
	,principal.type_desc 
	,principal.is_disabled 
	,principal.create_date 
	,principal.modify_date 
	,principal.default_database_name 
FROM sys.server_principals AS principal 

INNER JOIN sys.server_role_members AS membership 
	ON principal.principal_id = membership.role_principal_id 
	
INNER JOIN sys.server_principals AS login 
	ON login.principal_id = membership.member_principal_id 
	
WHERE principal.type = 'R' 
AND principal.name = N'sysadmin' 
