USE [LoveHitch2ndMarch2011]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WinkTypes_WinkCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[WinkTypes]'))
ALTER TABLE [dbo].[WinkTypes] DROP CONSTRAINT [FK_WinkTypes_WinkCategories]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_WinkTypes_wt_phrase]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[WinkTypes] DROP CONSTRAINT [DF_WinkTypes_wt_phrase]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftTypes_wt_active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[WinkTypes] DROP CONSTRAINT [DF_GiftTypes_wt_active]
END

GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[WinkTypes]    Script Date: 05/10/2011 23:27:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WinkTypes]') AND type in (N'U'))
DROP TABLE [dbo].[WinkTypes]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[WinkTypes]    Script Date: 05/10/2011 23:27:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[WinkTypes](
	[wt_id] [int] IDENTITY(1,1) NOT NULL,
	[wt_name] [nvarchar](200) NOT NULL,
	[wt_phrase] [nvarchar](250) NOT NULL,
	[wt_active] [bit] NOT NULL,
	[wt_category_id] [int] NULL,
 CONSTRAINT [PK_WinkTypes] PRIMARY KEY CLUSTERED 
(
	[wt_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[WinkTypes]  WITH CHECK ADD  CONSTRAINT [FK_WinkTypes_WinkCategories] FOREIGN KEY([wt_category_id])
REFERENCES [dbo].[WinkCategories] ([wc_id])
GO

ALTER TABLE [dbo].[WinkTypes] CHECK CONSTRAINT [FK_WinkTypes_WinkCategories]
GO

ALTER TABLE [dbo].[WinkTypes] ADD  CONSTRAINT [DF_WinkTypes_wt_phrase]  DEFAULT ('') FOR [wt_phrase]
GO

ALTER TABLE [dbo].[WinkTypes] ADD  CONSTRAINT [DF_GiftTypes_wt_active]  DEFAULT ((0)) FOR [wt_active]
GO


