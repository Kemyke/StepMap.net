CREATE TABLE stepmap.[User]
	(
	Id int NOT NULL,
	Name nvarchar(MAX) NOT NULL,
	Email nvarchar(MAX) NOT NULL,
	PasswordHash nvarchar(MAX) NOT NULL,
	LastLogin datetime2(7) NOT NULL
	)  
GO

ALTER TABLE stepmap.[User] ADD CONSTRAINT PK_User PRIMARY KEY CLUSTERED 
	(
		Id
	) 
GO