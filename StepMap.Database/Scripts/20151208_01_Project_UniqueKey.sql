ALTER TABLE stepmap.Project ADD CONSTRAINT
	IX_Project UNIQUE NONCLUSTERED 
	(
	UserId,
	Position
	) 
