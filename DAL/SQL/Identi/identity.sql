
-- ALTER TABLE AspNetUsers ADD  DEFAULT (NULL) FOR MobileAlias
-- ALTER TABLE AspNetUsers ADD  DEFAULT ((0)) FOR IsAnonymous


CREATE TABLE AspNetUsers
(
	Id national character varying(128) NOT NULL,
	UserName text NULL,
	PasswordHash text NULL,
	SecurityStamp text NULL,
	Discriminator national character varying(128) NOT NULL,
	ApplicationId uuid NOT NULL,
	LegacyPasswordHash text NULL,
	LoweredUserName national character varying(256) NOT NULL,
	MobileAlias national character varying(16) NULL,
	IsAnonymous boolean NOT NULL,
	LastActivityDate timestamp without time zone NOT NULL,
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
);



CREATE TABLE AspNetUserRoles
(
	UserId national character varying(128) NOT NULL,
	RoleId national character varying(128) NOT NULL 
); 


CREATE TABLE AspNetUserLogins
(
	UserId national character varying(128) NOT NULL,
	LoginProvider national character varying(128) NOT NULL,
	ProviderKey national character varying(128) NOT NULL
);


CREATE TABLE AspNetUserClaims
(
	Id int  NOT NULL, -- IDENTITY(1,1)
	ClaimType text NULL,
	ClaimValue text NULL,
	User_Id national character varying(128) NOT NULL 
);
