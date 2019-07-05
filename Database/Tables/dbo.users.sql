SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[users] (
		[userId]       [int] NOT NULL,
		[username]     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[password]     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		[userType]     [int] NOT NULL,
		[branchId]     [int] NOT NULL,
		CONSTRAINT [PK_users]
		PRIMARY KEY
		CLUSTERED
		([userId])
	ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[users] SET (LOCK_ESCALATION = TABLE)
GO
