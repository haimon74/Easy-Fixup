USE [dbo]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsMessages_GiftTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsMessages]'))
ALTER TABLE [dbo].[GiftsMessages] DROP CONSTRAINT [FK_GiftsMessages_GiftTypes]
GO

/****** Object:  Table [dbo].[GiftsMessages]    Script Date: 05/18/2011 21:35:35 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftsMessages]') AND type in (N'U'))
DROP TABLE [dbo].[GiftsMessages]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecards_EcardTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecards]'))
ALTER TABLE [dbo].[Ecards] DROP CONSTRAINT [FK_Ecards_EcardTypes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecards_Users]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecards]'))
ALTER TABLE [dbo].[Ecards] DROP CONSTRAINT [FK_Ecards_Users]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Ecards_Users1]') AND parent_object_id = OBJECT_ID(N'[dbo].[Ecards]'))
ALTER TABLE [dbo].[Ecards] DROP CONSTRAINT [FK_Ecards_Users1]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Ecards_ec_is_opened]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Ecards] DROP CONSTRAINT [DF_Ecards_ec_is_opened]
END

GO

/****** Object:  Table [dbo].[Ecards]    Script Date: 05/18/2011 21:28:12 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Ecards]') AND type in (N'U'))
DROP TABLE [dbo].[Ecards]
GO


IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_WinkMessaages_WinkTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[WinkMessaages]'))
ALTER TABLE [dbo].[WinkMessaages] DROP CONSTRAINT [FK_WinkMessaages_WinkTypes]
GO

/****** Object:  Table [dbo].[WinkMessaages]    Script Date: 05/18/2011 21:27:27 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WinkMessaages]') AND type in (N'U'))
DROP TABLE [dbo].[WinkMessaages]
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

/****** Object:  Table [dbo].[WinkTypes]    Script Date: 05/18/2011 21:24:09 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WinkTypes]') AND type in (N'U'))
DROP TABLE [dbo].[WinkTypes]
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

/****** Object:  Table [dbo].[EcardTypes]    Script Date: 05/18/2011 21:23:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EcardTypes]') AND type in (N'U'))
DROP TABLE [dbo].[EcardTypes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftTypes_GiftCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftTypes]'))
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [FK_GiftTypes_GiftCategories]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftTypes_gt_phrase]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [DF_GiftTypes_gt_phrase]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftTypes_gt_type]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [DF_GiftTypes_gt_type]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftTypes_gt_active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [DF_GiftTypes_gt_active]
END

GO

/****** Object:  Table [dbo].[GiftTypes]    Script Date: 05/18/2011 21:22:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftTypes]') AND type in (N'U'))
DROP TABLE [dbo].[GiftTypes]
GO


/****** Object:  Table [dbo].[ECardCategories]    Script Date: 05/18/2011 21:07:11 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ECardCategories]') AND type in (N'U'))
DROP TABLE [dbo].[ECardCategories]
GO


/****** Object:  Table [dbo].[GiftCategories]    Script Date: 05/18/2011 21:07:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftCategories]') AND type in (N'U'))
DROP TABLE [dbo].[GiftCategories]
GO

/****** Object:  Table [dbo].[WinkCategories]    Script Date: 05/18/2011 21:08:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WinkCategories]') AND type in (N'U'))
DROP TABLE [dbo].[WinkCategories]
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

/****** Object:  Table [dbo].[WinkTypes]    Script Date: 05/18/2011 21:09:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[WinkTypes]') AND type in (N'U'))
DROP TABLE [dbo].[WinkTypes]
GO

/****** Object:  Table [dbo].[WinkTypes]    Script Date: 05/18/2011 21:09:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
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

/****** Object:  Table [dbo].[EcardTypes]    Script Date: 05/18/2011 21:12:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[EcardTypes]') AND type in (N'U'))
DROP TABLE [dbo].[EcardTypes]
GO



IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftTypes_GiftCategories]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftTypes]'))
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [FK_GiftTypes_GiftCategories]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftTypes_gt_phrase]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [DF_GiftTypes_gt_phrase]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftTypes_gt_type]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [DF_GiftTypes_gt_type]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_GiftTypes_gt_active]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[GiftTypes] DROP CONSTRAINT [DF_GiftTypes_gt_active]
END

GO


/****** Object:  Table [dbo].[GiftTypes]    Script Date: 05/18/2011 21:13:25 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftTypes]') AND type in (N'U'))
DROP TABLE [dbo].[GiftTypes]
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
CREATE TABLE dbo.ECardCategories
	(
	ecc_id int NOT NULL IDENTITY (1, 1),
	ecc_name nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ECardCategories ADD CONSTRAINT
	PK_ECardCategories PRIMARY KEY CLUSTERED 
	(
	ecc_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ECardCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'CONTROL') as Contr_Per 

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
CREATE TABLE dbo.GiftCategories
	(
	gc_id int NOT NULL IDENTITY (1, 1),
	gc_name nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.GiftCategories ADD CONSTRAINT
	PK_GiftCategories PRIMARY KEY CLUSTERED 
	(
	gc_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.GiftCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.GiftCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.GiftCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.GiftCategories', 'Object', 'CONTROL') as Contr_Per 


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
CREATE TABLE dbo.WinkCategories
	(
	wc_id int NOT NULL IDENTITY (1, 1),
	wc_name nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.WinkCategories ADD CONSTRAINT
	PK_WinkCategories PRIMARY KEY CLUSTERED 
	(
	wc_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.WinkCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'CONTROL') as Contr_Per 
/****** Object:  Table [dbo].[GiftTypes]    Script Date: 05/18/2011 21:13:25 ******/
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

ALTER TABLE [dbo].[GiftTypes] ADD  CONSTRAINT [DF_GiftTypes_gt_type]  DEFAULT ((1)) FOR [gt_type]
GO

ALTER TABLE [dbo].[GiftTypes] ADD  CONSTRAINT [DF_GiftTypes_gt_active]  DEFAULT ((0)) FOR [gt_active]
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



/****** Object:  Table [dbo].[EcardTypes]    Script Date: 05/18/2011 21:12:52 ******/
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

/****** Object:  Table [dbo].[Ecards]    Script Date: 05/18/2011 21:29:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Ecards](
	[ec_id] [int] IDENTITY(1,1) NOT NULL,
	[ect_id] [int] NOT NULL,
	[ec_from_username] [varchar](20) NOT NULL,
	[ec_to_username] [varchar](20) NOT NULL,
	[ec_date] [datetime] NOT NULL,
	[ec_message] [nvarchar](2000) NOT NULL,
	[ec_deleted_by_fromuser] [bit] NOT NULL,
	[ec_deleted_by_touser] [bit] NOT NULL,
	[ec_is_opened] [bit] NOT NULL,
 CONSTRAINT [PK_Ecards] PRIMARY KEY CLUSTERED 
(
	[ec_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Ecards]  WITH CHECK ADD  CONSTRAINT [FK_Ecards_EcardTypes] FOREIGN KEY([ect_id])
REFERENCES [dbo].[EcardTypes] ([ect_id])
GO

ALTER TABLE [dbo].[Ecards] CHECK CONSTRAINT [FK_Ecards_EcardTypes]
GO

ALTER TABLE [dbo].[Ecards]  WITH CHECK ADD  CONSTRAINT [FK_Ecards_Users] FOREIGN KEY([ec_from_username])
REFERENCES [dbo].[Users] ([u_username])
GO

ALTER TABLE [dbo].[Ecards] CHECK CONSTRAINT [FK_Ecards_Users]
GO

ALTER TABLE [dbo].[Ecards]  WITH CHECK ADD  CONSTRAINT [FK_Ecards_Users1] FOREIGN KEY([ec_to_username])
REFERENCES [dbo].[Users] ([u_username])
GO

ALTER TABLE [dbo].[Ecards] CHECK CONSTRAINT [FK_Ecards_Users1]
GO

ALTER TABLE [dbo].[Ecards] ADD  CONSTRAINT [DF_Ecards_ec_is_opened]  DEFAULT ((0)) FOR [ec_is_opened]
GO



/****** Object:  Table [dbo].[GiftsMessages]    Script Date: 05/18/2011 21:30:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[GiftsMessages](
	[gm_id] [int] IDENTITY(1,1) NOT NULL,
	[gm_gift_id] [int] NOT NULL,
	[gm_message_id] [int] NOT NULL,
 CONSTRAINT [PK_GiftsMessages] PRIMARY KEY CLUSTERED 
(
	[gm_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[GiftsMessages]  WITH CHECK ADD  CONSTRAINT [FK_GiftsMessages_GiftTypes] FOREIGN KEY([gm_gift_id])
REFERENCES [dbo].[GiftTypes] ([gt_id])
GO

ALTER TABLE [dbo].[GiftsMessages] CHECK CONSTRAINT [FK_GiftsMessages_GiftTypes]
GO



/****** Object:  Table [dbo].[WinkMessaages]    Script Date: 05/18/2011 21:31:44 ******/
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

