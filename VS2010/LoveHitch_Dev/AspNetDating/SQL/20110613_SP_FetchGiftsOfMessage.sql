USE [LoveHitch2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchGiftsOfMessage]    Script Date: 06/14/2011 00:21:13 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchGiftsOfMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchGiftsOfMessage]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchGiftsOfMessage]    Script Date: 06/14/2011 00:21:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchGiftsOfMessage]
(
	@MessageID	INT = 0	
)
AS
	SELECT TOP 10
		gm_id AS Id, gm_gift_id AS GiftId, gm_message_id AS MessageId
	
	FROM GiftsMessages
	WHERE (@MessageID = 0 OR @MessageID = gm_id)

GO


