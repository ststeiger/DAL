IF  NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = N'techonthenet')
CREATE LOGIN techonthenet
WITH PASSWORD = 'abc'
,DEFAULT_LANGUAGE=English
--,DEFAULT_LANGUAGE=German
--,DEFAULT_LANGUAGE=French
--,DEFAULT_LANGUAGE=Italian
,CHECK_POLICY=OFF
;
GO 
-- CREATE LOGIN [<domainName>\<loginName>] FROM WINDOWS;

EXEC sys.sp_addsrvrolemember @loginame = 'techonthenet', @rolename = N'sysadmin'
--ALTER LOGIN @username DISABLE


-- http://www.techonthenet.com/sql_server/users/create_login.php
-- CREATE USER [Bla] FOR LOGIN techonthenet



EXEC sp_dropsrvrolemember @loginame = 'techonthenet', @rolename = N'sysadmin'








-- Msg 15081, Level 16, State 1, Procedure sp_dropsrvrolemember 
-- ,Line 34 - Membership of the public role cannot be changed. 
-- You might consider removing the PUBIC role from your query. 
-- It is incorrectly never reported on, which is detrimental to 
-- the reliability of the outcome of your query. 
-- Commenting out the first CASE statement will suffice.


;WITH CTE_Role (name,role,type_desc)
AS
(
	SELECT 
		 principal.name 
		,role.name AS role 
		,principal.Type_Desc 
	FROM sys.server_principals AS principal 
	
	LEFT JOIN sys.server_role_members AS membership 
		ON membership.member_principal_id = principal.Principal_id 
		
	LEFT JOIN sys.server_principals AS role 
		ON role.Principal_id= membership.Role_principal_id 
		AND role.type_desc = 'SERVER_ROLE' 
		
	WHERE (1=1) 
	AND principal.Type_Desc <> 'SERVER_ROLE' 
	AND principal.is_disabled = 0 
	
	
	UNION ALL 


	SELECT 
		 principal.name 
		,'ControlServer' 
		,principal.type_desc AS loginType 
	FROM sys.server_principals AS principal  

	INNER JOIN sys.server_permissions AS permission 
		ON permission.grantee_principal_id = principal.principal_id 
		
	WHERE (1=1) 
	AND permission.class = 100 
	AND permission.type = 'CL' 
	AND permission.state = 'G' 
)

SELECT 
	 name 
	,Type_Desc 
	
	,CASE WHEN [public] = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS [Public] 
	,CASE WHEN sysadmin = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS SysAdmin 
	,CASE WHEN securityadmin = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS SecurityAdmin 
	,CASE WHEN serveradmin = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS ServerAdmin 
	,CASE WHEN setupadmin = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS SetupAdmin 
	,CASE WHEN processadmin = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS ProcessAdmin 
	,CASE WHEN diskadmin = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS DiskAdmin 
	,CASE WHEN dbcreator = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS DBCreator 
	,CASE WHEN bulkadmin = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS BulkAdmin 
	,CASE WHEN ControlServer = 1 THEN CAST('true' AS bit) ELSE CAST('false' AS bit) END AS ControlServer 
FROM CTE_Role 

PIVOT
(
	COUNT(role) FOR role IN ([public],sysadmin,securityadmin,serveradmin,setupadmin,processadmin,diskadmin,dbcreator,bulkadmin,ControlServer) 
) AS pvt 

WHERE Type_Desc NOT IN ('SERVER_ROLE') 
-- AND name = 'techonthenet' 

ORDER BY 
	name,type_desc 
	-- type_desc, name 
	