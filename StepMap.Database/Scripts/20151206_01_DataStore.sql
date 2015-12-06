﻿CREATE TABLE stepmap.DataStore
	(
	Id int NOT NULL IDENTITY (1, 1),
	[Key] nvarchar(100) NOT NULL,
	Value nvarchar(500) NOT NULL
	) 
GO

ALTER TABLE stepmap.DataStore ADD CONSTRAINT
	PK_DataStore PRIMARY KEY CLUSTERED 
	(
	Id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
