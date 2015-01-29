
-- AD 2008
-- 64 characters if using the userPrincipalName attribute. If using the sAMAccountName attribute, the common restriction is 20 characters or less.

-- http://stackoverflow.com/questions/386294/what-is-the-maximum-length-of-a-valid-email-address
-- The correct answer of 254 has now been accepted by the IETF following the erratum I submitted here.




-- Path = "<" [ A-d-l ":" ] Mailbox ">"
-- So the Mailbox element (i.e. the email address) has angle brackets around it to form a Path, so the Mailbox must be no more than 254 characters to keep the path under 256.

---- http://www.codeproject.com/Articles/689801/Understanding-and-Using-Simple-Membership-Provider


-- https://github.com/suhasj/SQLMembership-Identity-OWIN/blob/master/SQLMembership-Identity-OWIN/Migrations.sql





-- C:\Windows\Microsoft.NET\Framework\v4.0.30319
-- Install*.sql


-- https://www.adivo.com/samples/database/aspnetdb/aspnetdb.html

-- Common (InstallCommon.sql) 
   -- aspnet_Applications
   -- aspnet_SchemaVersions
   -- aspnet_Users
   -- vw_aspnet_Applications
   -- vw_aspnet_Users
   

   
-- Membership (InstallMembership.sql) 
   -- aspnet_Membership
   -- Creating the vw_aspnet_MembershipUsers view...
   
   
------ InstallPersistSqlState.sql
   
   
-- aspnet_Paths (InstallPersonalization.sql)
   -- aspnet_PersonalizationAllUsers
   -- aspnet_PersonalizationPerUser
   -- Creating the vw_aspnet_WebPartState_Paths view...
   -- Creating the vw_aspnet_WebPartState_Shared view...
   -- Creating the vw_aspnet_WebPartState_User view...

-- Profile (InstallProfile.SQL) 
   --aspnet_Profile
   --Creating the vw_aspnet_Profiles view...

-- Roles (InstallRoles.sql)
   -- aspnet_Roles
   -- aspnet_UsersInRoles
   -- Creating the vw_aspnet_Roles view...
   -- Creating the vw_aspnet_UsersInRoles view...
   
------ InstallSqlState.sql
------ InstallSqlStateTemplate.sql

   
-- InstallWebEvents (InstallWebEventSqlProvider.sql)
   -- aspnet_WebEvent_Events
   
   
   
-- Identity   
	-- CREATE TABLE AspNetUsers;
	-- CREATE TABLE AspNetRoles;
	-- CREATE TABLE AspNetUserRoles;
	-- CREATE TABLE AspNetUserClaims;
	-- CREATE TABLE AspNetUserLogins;
