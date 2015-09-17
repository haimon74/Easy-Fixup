USE [ezFixUp2ndMarch2011]
GO
/****** Object:  Table [dbo].[VirtualGiftsCategories]    Script Date: 03/25/2011 22:54:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualGiftsCategories](
	[vgc_id] [int] IDENTITY(1,1) NOT NULL,
	[vgc_parent_id] [int] NULL,
	[vgc_name] [nvarchar](50) NULL,
 CONSTRAINT [PK_VirtualGiftsCategories] PRIMARY KEY CLUSTERED 
(
	[vgc_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WinksCategories]    Script Date: 03/25/2011 22:54:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WinksCategories](
	[wc_id] [int] IDENTITY(1,1) NOT NULL,
	[wc_name] [nvarchar](50) NULL,
	[wc_parent_id] [int] NULL,
 CONSTRAINT [PK_WinksCategories] PRIMARY KEY CLUSTERED 
(
	[wc_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Winks]    Script Date: 03/25/2011 22:54:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Winks](
	[w_id] [int] IDENTITY(1,1) NOT NULL,
	[w_text] [nvarchar](50) NULL,
	[w_image_location] [nvarchar](250) NULL,
	[wc_id] [int] NULL,
 CONSTRAINT [PK_Winks] PRIMARY KEY CLUSTERED 
(
	[w_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VirtualGifts]    Script Date: 03/25/2011 22:54:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VirtualGifts](
	[vg_id] [int] IDENTITY(1,1) NOT NULL,
	[vg_photo_location] [nvarchar](250) NULL,
	[vg_price_as_points] [int] NULL,
	[vg_name] [nvarchar](50) NULL,
	[vgc_id] [int] NULL,
 CONSTRAINT [PK_VirtualGifts] PRIMARY KEY CLUSTERED 
(
	[vg_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_VirtualGifts_VirtualGiftsCategories]    Script Date: 03/25/2011 22:54:28 ******/
ALTER TABLE [dbo].[VirtualGifts]  WITH CHECK ADD  CONSTRAINT [FK_VirtualGifts_VirtualGiftsCategories] FOREIGN KEY([vgc_id])
REFERENCES [dbo].[VirtualGiftsCategories] ([vgc_id])
GO
ALTER TABLE [dbo].[VirtualGifts] CHECK CONSTRAINT [FK_VirtualGifts_VirtualGiftsCategories]
GO
/****** Object:  ForeignKey [FK_VirtualGiftsCategories_VirtualGiftsCategories]    Script Date: 03/25/2011 22:54:28 ******/
ALTER TABLE [dbo].[VirtualGiftsCategories]  WITH CHECK ADD  CONSTRAINT [FK_VirtualGiftsCategories_VirtualGiftsCategories] FOREIGN KEY([vgc_parent_id])
REFERENCES [dbo].[VirtualGiftsCategories] ([vgc_id])
GO
ALTER TABLE [dbo].[VirtualGiftsCategories] CHECK CONSTRAINT [FK_VirtualGiftsCategories_VirtualGiftsCategories]
GO
/****** Object:  ForeignKey [FK_Winks_WinksCategories]    Script Date: 03/25/2011 22:54:28 ******/
ALTER TABLE [dbo].[Winks]  WITH CHECK ADD  CONSTRAINT [FK_Winks_WinksCategories] FOREIGN KEY([wc_id])
REFERENCES [dbo].[WinksCategories] ([wc_id])
GO
ALTER TABLE [dbo].[Winks] CHECK CONSTRAINT [FK_Winks_WinksCategories]
GO
/****** Object:  ForeignKey [FK_WinksCategories_WinksCategories]    Script Date: 03/25/2011 22:54:28 ******/
ALTER TABLE [dbo].[WinksCategories]  WITH CHECK ADD  CONSTRAINT [FK_WinksCategories_WinksCategories] FOREIGN KEY([wc_parent_id])
REFERENCES [dbo].[WinksCategories] ([wc_id])
GO
ALTER TABLE [dbo].[WinksCategories] CHECK CONSTRAINT [FK_WinksCategories_WinksCategories]
GO
