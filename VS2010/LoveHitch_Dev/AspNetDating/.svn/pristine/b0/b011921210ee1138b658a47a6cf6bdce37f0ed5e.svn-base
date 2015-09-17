
/****** Object:  Table [dbo].[Matchmakings]    Script Date: 08/08/2011 21:32:57 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Matchmakings]') AND type in (N'U'))
DROP TABLE [dbo].[Matchmakings]
GO

USE [LoveHitchDev15June2011]
GO

/****** Object:  Table [dbo].[Matchmakings]    Script Date: 08/08/2011 21:32:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Matchmakings](
	[mm_id] [int] IDENTITY(1,1) NOT NULL,
	[mm_matchmaker_username] [varchar](20) NOT NULL,
	[mm_to_friend_username] [varchar](20) NOT NULL,
	[mm_with_friend_username] [varchar](20) NOT NULL,
	[mm_timestamp] [datetime] NOT NULL,
	[mm_to_friend_ack] [bit] NOT NULL,
	[mm_with_friend_ack] [bit] NULL,
	[mm_status] [nvarchar](50) NULL,
 CONSTRAINT [PK_Matchmakings] PRIMARY KEY CLUSTERED 
(
	[mm_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


