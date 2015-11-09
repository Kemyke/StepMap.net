CREATE TABLE stepmap.Step
	(
	Id int NOT NULL,
	Name nvarchar(MAX) NOT NULL,
	Deadline datetime2(7) NOT NULL,
	FinishDate datetime2(7) NULL,
	SentReminder int NOT NULL,
	ProjectId int NOT NULL
	)  
GO

ALTER TABLE stepmap.Step ADD CONSTRAINT PK_Step PRIMARY KEY CLUSTERED 
	(
		Id
	) 
GO

ALTER TABLE stepmap.Step ADD CONSTRAINT
	FK_Project_Step FOREIGN KEY
	(
	ProjectId
	) REFERENCES stepmap.Step
	(
	Id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 	
GO