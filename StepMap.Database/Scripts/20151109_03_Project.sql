CREATE TABLE stepmap.Project
	(
	Id int NOT NULL,
	Name nvarchar(MAX) NOT NULL,
	Position int NOT NULL,
	StartDate datetime2(7) NOT NULL,
	GoodPoint int NOT NULL,
	BadPoint int NOT NULL,
	NextStepId int NULL,
	UserId int NOT NULL
	)  

ALTER TABLE stepmap.Project ADD CONSTRAINT PK_Project PRIMARY KEY CLUSTERED 
(
	Id
) 
GO

ALTER TABLE stepmap.Project ADD CONSTRAINT
	FK_Step_Project FOREIGN KEY
	(
	NextStepId
	) REFERENCES stepmap.Project
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 	
GO

ALTER TABLE stepmap.Project ADD CONSTRAINT
	FK_User_Project FOREIGN KEY
	(
	UserId
	) REFERENCES stepmap.Project
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 	
GO