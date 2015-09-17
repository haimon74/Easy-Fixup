USE [ezFixUp2ndMarch2011]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_GiftsMessages_GiftTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[GiftsMessages]'))
ALTER TABLE [dbo].[GiftsMessages] DROP CONSTRAINT [FK_GiftsMessages_GiftTypes]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  Table [dbo].[GiftsMessages]    Script Date: 05/12/2011 02:40:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GiftsMessages]') AND type in (N'U'))
DROP TABLE [dbo].[GiftsMessages]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  Table [dbo].[GiftsMessages]    Script Date: 05/12/2011 02:40:31 ******/
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


