USE [LoveHitch2ndMarch2011]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Messages_MessagesTypes]') AND parent_object_id = OBJECT_ID(N'[dbo].[Messages]'))
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Messages_MessagesTypes]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Messages_Users_FROM]') AND parent_object_id = OBJECT_ID(N'[dbo].[Messages]'))
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Messages_Users_FROM]
GO

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Messages_Users_TO]') AND parent_object_id = OBJECT_ID(N'[dbo].[Messages]'))
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [FK_Messages_Users_TO]
GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_from_folder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_from_folder]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_to_folder]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_to_folder]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_timestamp]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_timestamp]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_replied_to]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_replied_to]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_is_read]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_is_read]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_pending_approval]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_pending_approval]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_new]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_new]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_Messages_m_type_id]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Messages] DROP CONSTRAINT [DF_Messages_m_type_id]
END

GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[Messages]    Script Date: 05/12/2011 03:57:20 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Messages]') AND type in (N'U'))
DROP TABLE [dbo].[Messages]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  Table [dbo].[Messages]    Script Date: 05/12/2011 03:57:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Messages](
	[m_id] [int] IDENTITY(1,1) NOT NULL,
	[m_from_username] [varchar](20) NOT NULL,
	[m_from_folder] [int] NOT NULL,
	[m_to_username] [varchar](20) NOT NULL,
	[m_to_folder] [int] NOT NULL,
	[m_body] [nvarchar](3500) NOT NULL,
	[m_timestamp] [datetime] NOT NULL,
	[m_replied_to] [int] NOT NULL,
	[m_is_read] [bit] NOT NULL,
	[m_pending_approval] [bit] NOT NULL,
	[m_new] [bit] NOT NULL,
	[m_type_id] [int] NULL,
	[m_subject] [nvarchar](50) NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[m_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_MessagesTypes] FOREIGN KEY([m_type_id])
REFERENCES [dbo].[MessagesTypes] ([mt_id])
GO

ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_MessagesTypes]
GO

ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users_FROM] FOREIGN KEY([m_from_username])
REFERENCES [dbo].[Users] ([u_username])
GO

ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users_FROM]
GO

ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users_TO] FOREIGN KEY([m_to_username])
REFERENCES [dbo].[Users] ([u_username])
GO

ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users_TO]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_from_folder]  DEFAULT ((2)) FOR [m_from_folder]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_to_folder]  DEFAULT ((1)) FOR [m_to_folder]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_timestamp]  DEFAULT (getdate()) FOR [m_timestamp]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_replied_to]  DEFAULT ((0)) FOR [m_replied_to]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_is_read]  DEFAULT ((0)) FOR [m_is_read]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_pending_approval]  DEFAULT ((0)) FOR [m_pending_approval]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_new]  DEFAULT ((1)) FOR [m_new]
GO

ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_type_id]  DEFAULT ((1)) FOR [m_type_id]
GO


