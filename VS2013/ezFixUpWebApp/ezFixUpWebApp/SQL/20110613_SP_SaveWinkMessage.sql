
/****** Object:  StoredProcedure [dbo].[SaveWinkMessage]    Script Date: 06/14/2011 00:08:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveWinkMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveWinkMessage]
GO


/****** Object:  StoredProcedure [dbo].[SaveWinkMessage]    Script Date: 06/14/2011 00:08:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SaveWinkMessage]
	(
		@ID INT,
		@MessageID INT,
		@WinkID INT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO WinkMessaages (wm_message_id, wm_wink_id)
					VALUES (@MessageID, @WinkID)
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE WinkMessaages
		SET wm_message_id = @MessageID,
			wm_wink_id = @WinkID
		WHERE wm_id = @ID
	END



GO


