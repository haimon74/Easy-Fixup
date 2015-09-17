
/****** Object:  StoredProcedure [dbo].[DeleteGiftType]    Script Date: 06/05/2011 23:29:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteGiftType]
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM GiftsMessages WHERE gm_gift_id = @ID

DELETE FROM GiftTypes WHERE	gt_id = @ID

COMMIT

GO


