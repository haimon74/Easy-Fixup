*** Object:  StoredProcedure [dbo].[LoadMessage]    Script Date: 01/09/2012 10:25:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadMessage]
(
	@Id			INT
)
AS
	SELECT
		m_id			AS Id,
		m_from_username	AS FromUsername,
		m_from_folder		AS FromFolder,
		m_to_username		AS ToUsername,
		m_to_folder		AS ToFolder,
		m_about_username		AS AboutUsername,
		m_body			AS Body,
		m_timestamp		AS Timestamp,
		m_replied_to		AS RepliedTo,
		m_is_read		AS IsRead,
		m_pending_approval	AS PendingApproval
	
	FROM Messages
	WHERE @Id = m_id

	
	
/****** Object:  StoredProcedure [dbo].[SendMessage]    Script Date: 01/09/2012 10:30:13 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[SendMessage]
(
	@FromUsername	VARCHAR(20),
	@ToUsername		VARCHAR(20),
	@AboutUsername	VARCHAR(20),
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
		(m_from_username, m_to_username, m_about_username, m_subject, m_body, m_replied_to, m_pending_approval)
	VALUES
		(@FromUsername, @ToUsername, @AboutUsername, @Subject, @Body, @RepliedTo, @PendingApproval)
	SELECT @@IDENTITY


	
	