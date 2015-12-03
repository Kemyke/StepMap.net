CREATE TABLE stepmap.UserRole
	(
	Id int NOT NULL IDENTITY (1, 1),
	Name nvarchar(50) NOT NULL
	)  ON [PRIMARY]
GO

ALTER TABLE stepmap.UserRole ADD CONSTRAINT
	PK_UserRole PRIMARY KEY CLUSTERED 
	(
	Id
	) 
GO

SET IDENTITY_INSERT [stepmap].[UserRole] ON 
GO
INSERT [stepmap].[UserRole] ([Id], [Name]) VALUES (1, N'Member')
GO
INSERT [stepmap].[UserRole] ([Id], [Name]) VALUES (2, N'GroupAdmin')
GO
INSERT [stepmap].[UserRole] ([Id], [Name]) VALUES (3, N'Admin')
GO
SET IDENTITY_INSERT [stepmap].[UserRole] OFF
GO
