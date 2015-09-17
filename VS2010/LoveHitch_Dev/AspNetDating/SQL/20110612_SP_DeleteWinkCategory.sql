
/****** Object:  StoredProcedure [dbo].[DeleteWinkCategory]    Script Date: 06/13/2011 00:58:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteWinkCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteWinkCategory]
GO


/****** Object:  StoredProcedure [dbo].[DeleteWinkCategory]    Script Date: 06/13/2011 00:58:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DeleteWinkCategory]
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM WinkTypes WHERE	wt_category_id = @ID

DELETE FROM WinkCategories WHERE wc_id = @ID

COMMIT


GO


