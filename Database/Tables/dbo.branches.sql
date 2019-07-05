SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[branches] (
		[branchId]       [int] NOT NULL,
		[branchname]     [varchar](50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
		CONSTRAINT [PK_branches]
		PRIMARY KEY
		CLUSTERED
		([branchId])
	ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[branches] SET (LOCK_ESCALATION = TABLE)
GO
