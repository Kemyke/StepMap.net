CREATE TABLE stepmap.Reminder
	(
	Id int NOT NULL IDENTITY (1, 1),
	EmailAddress varchar(200) NOT NULL,
	Subject nvarchar(MAX) NOT NULL,
	Message nvarchar(MAX) NOT NULL,
	SentDate datetime2(7) NOT NULL,
	StepId int NOT NULL
	) 
GO

ALTER TABLE stepmap.Reminder ADD CONSTRAINT
	PK_Reminder PRIMARY KEY CLUSTERED 
	(
	Id
	) 
GO

ALTER TABLE stepmap.Reminder ADD CONSTRAINT
	FK_Reminder_Step FOREIGN KEY
	(
	StepId
	) REFERENCES stepmap.Step
	(
	Id
	) 
GO
