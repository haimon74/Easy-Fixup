USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[SendMessage]    Script Date: 05/12/2011 04:26:37 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SendMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SendMessage]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[SendMessage]    Script Date: 05/12/2011 04:26:37 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SendMessage]
(
	@FromUsername	VARCHAR(20),
	@ToUsername		VARCHAR(20),
	@Subject		NVARCHAR(50),
	@Body			NVARCHAR(3500),
	@RepliedTo		INT,
	@PendingApproval	BIT
)
AS

	IF NOT EXISTS (SELECT * FROM MessagesHistoryCache WHERE mhc_fromusername = @FromUsername AND mhc_tousername = @ToUsername)
		INSERT INTO MessagesHistoryCache (mhc_fromusername, mhc_tousername)
			VALUES (@FromUsername, @ToUsername)

	INSERT INTO Messages
		(m_from_username, m_to_username, m_subject, m_body, m_replied_to, m_pending_approval)
	VALUES
		(@FromUsername, @ToUsername, @Subject, @Body, @RepliedTo, @PendingApproval)
	SELECT @@IDENTITY

GO


