
/****** Object:  StoredProcedure [dbo].[SaveGiftMessage]    Script Date: 06/14/2011 00:08:55 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveGiftMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveGiftMessage]
GO


/****** Object:  StoredProcedure [dbo].[SaveGiftMessage]    Script Date: 06/14/2011 00:08:55 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SaveGiftMessage]
	(
		@ID INT,
		@MessageID INT,
		@GiftID INT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO GiftsMessages (gm_message_id, gm_gift_id)
					VALUES (@MessageID, @GiftID)
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE GiftsMessages
		SET gm_message_id = @MessageID,
			gm_gift_id = @GiftID
		WHERE gm_id = @ID
	END



GO


