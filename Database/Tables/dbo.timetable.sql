SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING OFF
GO
CREATE TABLE [dbo].[timetable] (
		[timetableId]     [int] NOT NULL,
		[userId]          [int] NOT NULL,
		[branchId]        [int] NOT NULL,
		[time]            [int] NOT NULL,
		CONSTRAINT [PK_timetable]
		PRIMARY KEY
		CLUSTERED
		([timetableId])
	ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[timetable]
	WITH CHECK
	ADD CONSTRAINT [FK_timetable_branches]
	FOREIGN KEY ([branchId]) REFERENCES [dbo].[branches] ([branchId])
ALTER TABLE [dbo].[timetable]
	CHECK CONSTRAINT [FK_timetable_branches]

GO
ALTER TABLE [dbo].[timetable]
	WITH CHECK
	ADD CONSTRAINT [FK_timetable_users]
	FOREIGN KEY ([userId]) REFERENCES [dbo].[users] ([userId])
ALTER TABLE [dbo].[timetable]
	CHECK CONSTRAINT [FK_timetable_users]

GO
ALTER TABLE [dbo].[timetable] SET (LOCK_ESCALATION = TABLE)
GO
