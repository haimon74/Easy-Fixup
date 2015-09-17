USE [LoveHitchDev15June2011]
GO
/****** Object:  StoredProcedure [dbo].[FetchGiftsOfMessage]    Script Date: 07/20/2011 01:39:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchGiftsOfMessage]
(
	@MessageID	INT = 0	
)
AS
	SELECT TOP 10
		gm_id AS Id, gm_gift_id AS GiftId, gm_message_id AS MessageId
	
	FROM GiftsMessages
	WHERE (@MessageID = 0 OR @MessageID = gm_message_id)

