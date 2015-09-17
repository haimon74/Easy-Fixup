USE [LoveHitch2ndMarch2011]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WinkMessaages_WinkTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[WinkMessaages]'))
ALTER TABLE [dbo].[WinkMessaages] DROP CONSTRAINT [FK_WinkMessaages_WinkTypes]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[WinkMessaages]    Script Date: 05/12/2011 02:35:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WinkMessaages]') AND type in (N'U'))
DROP TABLE [dbo].[WinkMessaages]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[WinkMessaages]    Script Date: 05/12/2011 02:35:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WinkMessaages](
	[wm_id] [int] IDENTITY(1,1) NOT NULL,
	[wm_message_id] [int] NOT NULL,
	[wm_wink_id] [int] NOT NULL,
 CONSTRAINT [PK_WinkMessaages] PRIMARY KEY CLUSTERED 
(
	[wm_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[WinkMessaages]  WITH CHECK ADD  CONSTRAINT [FK_WinkMessaages_WinkTypes] FOREIGN KEY([wm_wink_id])
REFERENCES [dbo].[WinkTypes] ([wt_id])
GO

ALTER TABLE [dbo].[WinkMessaages] CHECK CONSTRAINT [FK_WinkMessaages_WinkTypes]
GO


