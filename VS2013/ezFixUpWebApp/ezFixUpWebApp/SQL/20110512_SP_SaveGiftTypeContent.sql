USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[SaveGiftTypeContent]    Script Date: 05/13/2011 05:06:50 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveGiftTypeContent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveGiftTypeContent]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[SaveGiftTypeContent]    Script Date: 05/13/2011 05:06:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaveGiftTypeContent]
	(
		@GiftTypeID INT,
		@Content IMAGE
	)
AS

UPDATE GiftTypes
SET gt_content = @Content
WHERE gt_id = @GiftTypeID

GO


