--insert an admi user
insert into users(firstname,lastname,username,email,password,salt,PhotoPath,uniqueid,isactive,isemailverified,createduserid,modifieduserid,deleteduserid,createddate,modifieddate,deleteddate)
values('sysadmin','admin','sreehariis@gmail.com','sreehariis@gmail.com','KIgTQL4tdWmiyUnhHMNKW53YuPlPe/vXx5Q+5FSkM6dEqSAYOiZ6GFE6ExnffZRhSyrEFyGW72SQHbd7z0PeSQ==','a20189jd','../img/profile_small.jpg','abcd-1234',1,0,1,null,null,getdate(),null,null)

GO

INSERT INTO [dbo].[Settings]
           ([Name]
           ,[Value]
           ,[CreatedUserId]
           ,[ModifiedUserId]
           ,[DeletedUserId]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[DeletedDate])
     VALUES
           ('SMTP'
           ,'{
  "FROMADDRESS": "sreehari.aranghat@attristech.com",
  "FRIENDLYNAME": "SUPPORT PANDA"
}'
           ,1
           ,NULL
           ,NULL
           ,getdate()
           ,null
           ,null)



		   INSERT INTO [dbo].[Settings]
           ([Name]
           ,[Value]
           ,[CreatedUserId]
           ,[ModifiedUserId]
           ,[DeletedUserId]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[DeletedDate])
     VALUES
           ('EMAILTEMPLATE'
           ,'{"PATH" : "C:\\SupportPanda\\EmailTemplates"}'
           ,1
           ,NULL
           ,NULL
           ,getdate()
           ,null
           ,null)


	   INSERT INTO [dbo].[Settings]
           ([Name]
           ,[Value]
           ,[CreatedUserId]
           ,[ModifiedUserId]
           ,[DeletedUserId]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[DeletedDate])
     VALUES
           ('GENERAL'
           ,'{
  "RootDomain": "http://supportpanda.net",
  "ApplicationName": "Support Panda"
}'
           ,1
           ,NULL
           ,NULL
           ,getdate()
           ,null
           ,null)


		   INSERT INTO [dbo].[Settings]
           ([Name]
           ,[Value]
           ,[CreatedUserId]
           ,[ModifiedUserId]
           ,[DeletedUserId]
           ,[CreatedDate]
           ,[ModifiedDate]
           ,[DeletedDate])
     VALUES
           ('NOSQL'
           ,'{
  "MongoDbUrl": "mongodb://localhost:27017",
  "MongoDb": "SupportPanda"
}'
           ,1
           ,NULL
           ,NULL
           ,getdate()
           ,null
           ,null)



/*------------------------------------------ Add Permissions ---------------------------------------------------------*/

INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_GENERAL_MANAGE_AGENTS','General','Manage Agents and Agent Roles','Allow user to manage agents and their roles')
INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_GENERAL_MANAGE_AGENTGROUPS','General','Manage Agent groups','Allow user to manage agent groups')

INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_MANAGE_SETTINGS','Administration','Manage Application Settings','User can manage one or more application settings')
INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_MANAGE_APPLICATIONTHEME','Administration','Change or Update application theme','Allows the user to change or update application graphics such as logo, color pallete etc.')

INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_SECURITY_PASSWORDPOLICY','Security','Set password policy','Allows user to update password policy')
INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_SECURITY_MANAGEAUTH','Security','Manage authentication settings','Allow user to change authentication mechanism');
INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_SECURITY_MANAGEROLES','Security','Roles Management','Allow user create or modify application Roles');

INSERT INTO Permissions(Name,GroupName,Title,Description) VALUES('PER_UPDATE_CUSTOMERFIELD','Forms','Can Update Customer Field','Allow the user to update or modify custom customer fields')


INSERT INTO Roles(Name,Description,ClientId,IsSystemRole,CreatedUserId,CreatedDate) VALUES('Administrator','Administrator for the system',NULL,1,1,GETDATE())

DECLARE @AdminRoleId INT
SET @AdminRoleId = SCOPE_IDENTITY()

INSERT INTO RolePermissions
SELECT @AdminRoleId,PermissionId FROM Permissions