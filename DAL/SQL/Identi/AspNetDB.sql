
-- ALTER TABLE aspnet_Applications ADD  CONSTRAINT DF__aspnet_Ap__Appli__08EA5793  DEFAULT (newid()) FOR ApplicationId
-- ALTER TABLE aspnet_Membership ADD  DEFAULT ((0)) FOR PasswordFormat
-- ALTER TABLE aspnet_Paths ADD  DEFAULT (newid()) FOR PathId
-- ALTER TABLE aspnet_PersonalizationPerUser ADD  DEFAULT (newid()) FOR Id
-- ALTER TABLE aspnet_Roles ADD  DEFAULT (newid()) FOR RoleId
-- ALTER TABLE aspnet_Users ADD  DEFAULT (newid()) FOR UserId
-- ALTER TABLE aspnet_Users ADD  DEFAULT (NULL) FOR MobileAlias
-- ALTER TABLE aspnet_Users ADD  DEFAULT ((0)) FOR IsAnonymous


-- uniqueidentifier ==> uuid 
-- datetime ==> timestamp without time zone
-- ntext ==> text 
-- bit ==> boolean 
-- image ==> bytea 

-- http://stackoverflow.com/questions/54500/storing-images-in-postgresql
-- bytea being a "normal" column also means the value being read completely into memory 
-- when you fetch it. Blobs, in contrast, you can stream into stdout. 
-- That helps in reducing the server memory footprint. 
-- Especially, when you store 4-6 MPix images.
--  Large Object Binary (LOB) f
-- bytea is PostgreSQL's version of a BLOB. From the fine manual:
-- The output format depends on the configuration parameter bytea_output; 
-- the default is hex. (Note that the hex format was introduced in PostgreSQL 9.0; 
-- earlier versions and some tools don't understand it.)

CREATE TABLE aspnet_Applications(
	ApplicationId uuid NOT NULL,
	ApplicationName national character varying(256) NOT NULL,
	LoweredApplicationName national character varying(256) NOT NULL,
	Description national character varying(256) NULL 
);


CREATE TABLE aspnet_Membership(
	ApplicationId uuid NOT NULL,
	UserId uuid NOT NULL,
	Password national character varying(128) NOT NULL,
	PasswordFormat int NOT NULL,
	PasswordSalt national character varying(128) NOT NULL,
	MobilePIN national character varying(16) NULL,
	Email national character varying(256) NULL,
	LoweredEmail national character varying(256) NULL,
	PasswordQuestion national character varying(256) NULL,
	PasswordAnswer national character varying(128) NULL,
	IsApproved boolean NOT NULL,
	IsLockedOut boolean NOT NULL,
	CreateDate timestamp without time zone NOT NULL,
	LastLoginDate timestamp without time zone NOT NULL,
	LastPasswordChangedDate timestamp without time zone NOT NULL,
	LastLockoutDate timestamp without time zone NOT NULL,
	FailedPasswordAttemptCount int NOT NULL,
	FailedPasswordAttemptWindowStart timestamp without time zone NOT NULL,
	FailedPasswordAnswerAttemptCount int NOT NULL,
	FailedPasswordAnswerAttemptWindowStart timestamp without time zone NOT NULL,
	Comment text NULL 
) ;


CREATE TABLE aspnet_Paths(
	ApplicationId uuid NOT NULL,
	PathId uuid NOT NULL,
	Path national character varying(256) NOT NULL,
	LoweredPath national character varying(256) NOT NULL 
); 


CREATE TABLE aspnet_PersonalizationAllUsers(
	PathId uuid NOT NULL,
	PageSettings bytea NOT NULL,
	LastUpdatedDate timestamp without time zone NOT NULL 
);


CREATE TABLE aspnet_PersonalizationPerUser(
	Id uuid NOT NULL,
	PathId uuid NULL,
	UserId uuid NULL,
	PageSettings bytea NOT NULL,
	LastUpdatedDate timestamp without time zone NOT NULL 
); 


CREATE TABLE aspnet_Profile(
	UserId uuid NOT NULL,
	PropertyNames text NOT NULL,
	PropertyValuesString text NOT NULL,
	PropertyValuesBinary bytea NOT NULL,
	LastUpdatedDate timestamp without time zone NOT NULL 
);


CREATE TABLE aspnet_Roles(
	ApplicationId uuid NOT NULL,
	RoleId uuid NOT NULL,
	RoleName national character varying(256) NOT NULL,
	LoweredRoleName national character varying(256) NOT NULL,
	Description national character varying(256) NULL 
); 


CREATE TABLE aspnet_SchemaVersions(
	Feature national character varying(128) NOT NULL,
	CompatibleSchemaVersion national character varying(128) NOT NULL,
	IsCurrentVersion boolean NOT NULL 
);


CREATE TABLE aspnet_Users(
	ApplicationId uuid NOT NULL,
	UserId uuid NOT NULL,
	UserName national character varying(256) NOT NULL,
	LoweredUserName national character varying(256) NOT NULL,
	MobileAlias national character varying(16) NULL,
	IsAnonymous boolean NOT NULL,
	LastActivityDate timestamp without time zone NOT NULL 
);


CREATE TABLE aspnet_UsersInRoles(
	UserId uuid NOT NULL,
	RoleId uuid NOT NULL
);


CREATE TABLE aspnet_WebEvent_Events(
	EventId char(32) NOT NULL,
	EventTimeUtc timestamp without time zone NOT NULL,
	EventTime timestamp without time zone NOT NULL,
	EventType national character varying(256) NOT NULL,
	EventSequence decimal(19, 0) NOT NULL,
	EventOccurrence decimal(19, 0) NOT NULL,
	EventCode int NOT NULL,
	EventDetailCode int NOT NULL,
	Message national character varying(1024) NULL,
	ApplicationPath national character varying(256) NULL,
	ApplicationVirtualPath national character varying(256) NULL,
	MachineName national character varying(256) NOT NULL,
	RequestUrl national character varying(1024) NULL,
	ExceptionType national character varying(256) NULL,
	Details text NULL 
);
