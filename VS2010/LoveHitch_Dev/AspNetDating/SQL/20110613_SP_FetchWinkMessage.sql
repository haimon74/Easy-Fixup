
/****** Object:  StoredProcedure [dbo].[FetchWinkMessage]    Script Date: 06/14/2011 00:38:39 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchWinkMessage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchWinkMessage]
GO


/****** Object:  StoredProcedure [dbo].[FetchWinkMessage]    Script Date: 06/14/2011 00:38:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[FetchWinkMessage]
(
	@MessageID	INT = 0	
)
AS
	SELECT TOP 1
		wm_id AS Id, wm_wink_id AS WinkId, wm_message_id AS MessageId
	
	FROM WinkMessaages
	WHERE (@MessageID = 0 OR @MessageID = wm_id)


GO


