USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchEcardTypes]    Script Date: 05/10/2011 19:29:14 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchEcardTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchEcardTypes]
GO

USE [ezFixUp2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[FetchEcardTypes]    Script Date: 05/10/2011 19:29:14 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchEcardTypes]
	(
		@ID INT,
		@CategoryID INT,
		@Name NVARCHAR(200),
		@Type INT,
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	ect_id AS [ID],
		ect_category_id AS [CategoryID],
		ect_name AS [Name],
		ect_phrase AS [Phrase],
		ect_type AS Type,
		ect_active AS Active
		
FROM	EcardTypes
WHERE	(@ID IS NULL OR ect_id = @ID)
		AND (@CategoryID IS NULL OR ect_category_id = @CategoryID)
		AND (@Name IS NULL OR ect_name = @Name)
		AND (@Type IS NULL OR ect_type = @Type)
		AND (@Active IS NULL OR ect_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ect_name END

GO


