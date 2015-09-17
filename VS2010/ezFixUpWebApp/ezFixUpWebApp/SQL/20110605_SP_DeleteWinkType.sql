
/****** Object:  StoredProcedure [dbo].[DeleteWinkType]    Script Date: 06/05/2011 23:29:12 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[DeleteWinkType]
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM WinkMessaages WHERE wm_wink_id = @ID

DELETE FROM WinkTypes WHERE	wt_id = @ID

COMMIT

GO


