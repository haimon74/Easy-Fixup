USE [dbo]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EcardTypes_ECardCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[EcardTypes]'))
ALTER TABLE [dbo].[EcardTypes] DROP CONSTRAINT [FK_EcardTypes_ECardCategories]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EcardTypes_GiftCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[EcardTypes]'))
ALTER TABLE [dbo].[EcardTypes] DROP CONSTRAINT [FK_EcardTypes_GiftCategories]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_EcardTypes_WinkCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[EcardTypes]'))
ALTER TABLE [dbo].[EcardTypes] DROP CONSTRAINT [FK_EcardTypes_WinkCategories]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_EcardTypes_ect_phrase]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EcardTypes] DROP CONSTRAINT [DF_EcardTypes_ect_phrase]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_EcardTypes_ect_active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[EcardTypes] DROP CONSTRAINT [DF_EcardTypes_ect_active]
END

GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[EcardTypes]    Script Date: 05/18/2011 20:36:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EcardTypes]') AND type in (N'U'))
DROP TABLE [dbo].[EcardTypes]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[EcardTypes]    Script Date: 05/18/2011 20:36:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[EcardTypes](
	[ect_id] [int] IDENTITY(1,1) NOT NULL,
	[ect_name] [nvarchar](200) NOT NULL,
	[ect_phrase] [nvarchar](250) NOT NULL,
	[ect_content] [image] NULL,
	[ect_type] [int] NOT NULL,
	[ect_active] [bit] NOT NULL,
	[ect_category_id] [int] NULL,
 CONSTRAINT [PK_EcardTypes] PRIMARY KEY CLUSTERED 
(
	[ect_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[EcardTypes]  WITH CHECK ADD  CONSTRAINT [FK_EcardTypes_ECardCategories] FOREIGN KEY([ect_category_id])
REFERENCES [dbo].[ECardCategories] ([ecc_id])
GO

ALTER TABLE [dbo].[EcardTypes] CHECK CONSTRAINT [FK_EcardTypes_ECardCategories]
GO

ALTER TABLE [dbo].[EcardTypes]  WITH CHECK ADD  CONSTRAINT [FK_EcardTypes_GiftCategories] FOREIGN KEY([ect_category_id])
REFERENCES [dbo].[GiftCategories] ([gc_id])
GO

ALTER TABLE [dbo].[EcardTypes] CHECK CONSTRAINT [FK_EcardTypes_GiftCategories]
GO

ALTER TABLE [dbo].[EcardTypes]  WITH CHECK ADD  CONSTRAINT [FK_EcardTypes_WinkCategories] FOREIGN KEY([ect_category_id])
REFERENCES [dbo].[WinkCategories] ([wc_id])
GO

ALTER TABLE [dbo].[EcardTypes] CHECK CONSTRAINT [FK_EcardTypes_WinkCategories]
GO

ALTER TABLE [dbo].[EcardTypes] ADD  CONSTRAINT [DF_EcardTypes_ect_phrase]  DEFAULT ('') FOR [ect_phrase]
GO

ALTER TABLE [dbo].[EcardTypes] ADD  CONSTRAINT [DF_EcardTypes_ect_active]  DEFAULT ((0)) FOR [ect_active]
GO


