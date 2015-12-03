CREATE TABLE stepmap.UserState
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO

ALTER TABLE stepmap.UserState ADD CONSTRAINT
	PK_UserState PRIMARY KEY CLUSTERED 
	(
	Id
	) 
GO

SET IDENTITY_INSERT [stepmap].[UserState] ON 
GO
INSERT [stepmap].[UserState] ([Id], [Name]) VALUES (1, N'NotActivatedYet')
GO
INSERT [stepmap].[UserState] ([Id], [Name]) VALUES (2, N'Active')
GO
INSERT [stepmap].[UserState] ([Id], [Name]) VALUES (3, N'Inactive')
GO
SET IDENTITY_INSERT [stepmap].[UserState] OFF
GO