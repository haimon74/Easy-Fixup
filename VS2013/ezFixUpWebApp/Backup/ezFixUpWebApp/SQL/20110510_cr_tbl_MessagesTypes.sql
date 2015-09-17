USE [ezFixUp2ndMarch2011]
GO

/****** Object:  Table [dbo].[MessagesTypes]    Script Date: 05/11/2011 00:13:47 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MessagesTypes]') AND type in (N'U'))
DROP TABLE [dbo].[MessagesTypes]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  Table [dbo].[MessagesTypes]    Script Date: 05/11/2011 00:13:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[MessagesTypes](
	[mt_id] [int] IDENTITY(1,1) NOT NULL,
	[mt_name] [nvarchar](50) NOT NULL,
	[mt_active] [bit] NOT NULL,
 CONSTRAINT [PK_MessagesTypes] PRIMARY KEY CLUSTERED 
(
	[mt_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


