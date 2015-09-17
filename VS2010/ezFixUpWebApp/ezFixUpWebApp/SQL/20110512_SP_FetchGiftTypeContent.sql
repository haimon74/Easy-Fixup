USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchGiftTypeContent]    Script Date: 05/13/2011 05:04:44 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchGiftTypeContent]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchGiftTypeContent]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchGiftTypeContent]    Script Date: 05/13/2011 05:04:44 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchGiftTypeContent]
	(
		@ID INT
	)
AS

SELECT gt_content AS Content
FROM GiftTypes
WHERE gt_id = @ID

GO


