USE [LoveHitch2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[UpdateMessage]    Script Date: 05/11/2011 01:01:30 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UpdateMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UpdateMessage]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[UpdateMessage]    Script Date: 05/11/2011 01:01:30 ******/
SET ANSI_NULLS OFF
GO

SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE [dbo].[UpdateMessage]
(
	@Id	INT,
	@MessageTypeId	INT = NULL,
	@FromFolder	INT = NULL,
	@ToFolder	INT = NULL,
	@IsRead	BIT = NULL,
	@PendingApproval BIT = NULL

)
AS
IF (@MessageTypeId IS NOT NULL)
	UPDATE Messages SET m_type_id = @MessageTypeId WHERE m_id = @Id
IF (@FromFolder IS NOT NULL)
	UPDATE Messages SET m_from_folder = @FromFolder WHERE m_id = @Id
IF (@ToFolder IS NOT NULL)
	UPDATE Messages SET m_to_folder = @ToFolder WHERE m_id = @Id
IF (@IsRead IS NOT NULL)
	UPDATE Messages SET m_is_read = @IsRead WHERE m_id = @Id
IF (@PendingApproval IS NOT NULL)
BEGIN
	IF (@PendingApproval = 0)
		UPDATE Users SET u_message_verifications_left = u_message_verifications_left - 1 
			WHERE u_username in (SELECT  m_from_username FROM Messages WHERE m_id = @Id)
				AND u_message_verifications_left > 0
	UPDATE Messages SET m_pending_approval = @PendingApproval WHERE m_id = @Id
END

GO


