USE [LoveHitch14thAugust2011]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_AskAboutQuestions_q_order]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[AskAboutQuestions] DROP CONSTRAINT [DF_AskAboutQuestions_q_order]
END

GO

USE [LoveHitch14thAugust2011]
GO

/****** Object:  Table [dbo].[AskAboutQuestions]    Script Date: 08/27/2011 01:44:08 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AskAboutQuestions]') AND type in (N'U'))
DROP TABLE [dbo].[AskAboutQuestions]
GO


USE [LoveHitch14thAugust2011]
GO

/****** Object:  Table [dbo].[AskAboutQuestions]    Script Date: 08/27/2011 01:38:33 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[AskAboutQuestions](
	[q_id] [int] IDENTITY(1,1) NOT NULL,
	[q_name] [nvarchar](50) NOT NULL,
	[q_active] [bit] NOT NULL,
	[q_order] [int] NOT NULL,
	[q_display] [varchar](250) NOT NULL
 CONSTRAINT [PK_AskAboutQuestions] PRIMARY KEY CLUSTERED 
(
	[q_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[AskAboutQuestions] ADD  CONSTRAINT [DF_AskAboutQuestions_q_active]  DEFAULT ((0)) FOR [q_active]
GO

ALTER TABLE [dbo].[AskAboutQuestions] ADD  CONSTRAINT [DF_AskAboutQuestions_q_order]  DEFAULT ((1)) FOR [q_order]
GO


