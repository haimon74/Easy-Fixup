USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[GiftTypes]    Script Date: 05/10/2011 23:06:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GiftTypes](
	[gt_id] [int] IDENTITY(1,1) NOT NULL,
	[gt_name] [nvarchar](200) NOT NULL,
	[gt_phrase] [nvarchar](250) NOT NULL,
	[gt_content] [image] NULL,
	[gt_type] [int] NOT NULL,
	[gt_active] [bit] NOT NULL,
	[gt_category_id] [int] NULL,
 CONSTRAINT [PK_GiftTypes] PRIMARY KEY CLUSTERED 
(
	[gt_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[GiftTypes]  WITH CHECK ADD  CONSTRAINT [FK_GiftTypes_GiftCategories] FOREIGN KEY([gt_category_id])
REFERENCES [dbo].[GiftCategories] ([gc_id])
GO

ALTER TABLE [dbo].[GiftTypes] CHECK CONSTRAINT [FK_GiftTypes_GiftCategories]
GO


ALTER TABLE [dbo].[GiftTypes] ADD  CONSTRAINT [DF_GiftTypes_gt_phrase]  DEFAULT ('') FOR [gt_phrase]
GO

ALTER TABLE [dbo].[GiftTypes] ADD  CONSTRAINT [DF_GiftTypes_gt_active]  DEFAULT ((0)) FOR [gt_active]
GO


BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.GiftTypes ADD CONSTRAINT
	DF_GiftTypes_gt_type DEFAULT 1 FOR gt_type
GO
ALTER TABLE dbo.GiftTypes SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.GiftTypes', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.GiftTypes', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.GiftTypes', 'Object', 'CONTROL') as Contr_Per 