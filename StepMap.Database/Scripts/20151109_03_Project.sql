CREATE TABLE [stepmap].[Project](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Position] [int] NOT NULL,
	[StartDate] [datetime2](7) NOT NULL,
	[GoodPoint] [int] NOT NULL,
	[BadPoint] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [stepmap].[Project]  WITH CHECK ADD  CONSTRAINT [FK_User_Project] FOREIGN KEY([UserId])
REFERENCES [stepmap].[Project] ([Id])
GO

ALTER TABLE [stepmap].[Project] CHECK CONSTRAINT [FK_User_Project]
GO
