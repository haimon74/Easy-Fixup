USE [ezFixUp2ndMarch2011]
GO

/****** Objgt:  StoredProcedure [dbo].[FetchGiftTypes]    Script Date: 05/13/2011 04:30:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchGiftTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchGiftTypes]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Objgt:  StoredProcedure [dbo].[FetchGiftTypes]    Script Date: 05/13/2011 04:30:31 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[FetchGiftTypes]
	(
		@ID INT,
		@CategoryID INT,
		@Name NVARCHAR(200),
		@Type INT,
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	gt_id AS [ID],
		gt_category_id AS [CategoryID],
		gt_name AS [Name],
		gt_phrase AS [Phrase],
		gt_type AS ItsType,
		gt_active AS Active
		
FROM	GiftTypes
WHERE	(@ID IS NULL OR gt_id = @ID)
		AND (@CategoryID IS NULL OR gt_category_id = @CategoryID)
		AND (@Name IS NULL OR gt_name = @Name)
		AND (@Type IS NULL OR gt_type = @Type)
		AND (@Active IS NULL OR gt_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN gt_name END


GO


