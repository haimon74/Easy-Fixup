USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[DeleteGiftCategory]    Script Date: 06/13/2011 01:04:41 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteGiftCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteGiftCategory]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[DeleteGiftCategory]    Script Date: 06/13/2011 01:04:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[DeleteGiftCategory]
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM GiftTypes WHERE	gt_category_id = @ID

DELETE FROM GiftCategories WHERE gc_id = @ID

COMMIT


GO


