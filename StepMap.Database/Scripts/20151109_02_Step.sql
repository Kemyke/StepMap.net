CREATE TABLE [stepmap].[Step](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NOT NULL,
	[Deadline] [datetime2](7) NOT NULL,
	[FinishDate] [datetime2](7) NULL,
	[SentReminder] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
 CONSTRAINT [PK_Step] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [stepmap].[Step]  WITH CHECK ADD  CONSTRAINT [FK_Project_Step] FOREIGN KEY([ProjectId])
REFERENCES [stepmap].[Step] ([Id])
GO

ALTER TABLE [stepmap].[Step] CHECK CONSTRAINT [FK_Project_Step]
GO


