
/****** Object:  StoredProcedure [dbo].[FetchWinkTypes]    Script Date: 06/04/2011 16:50:24 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchWinkTypes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchWinkTypes]
GO


/****** Object:  StoredProcedure [dbo].[FetchWinkTypes]    Script Date: 06/04/2011 16:50:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[FetchWinkTypes]
	(
		@ID INT,
		@CategoryID INT,
		@Name NVARCHAR(200),
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	wt_id AS [ID],
		wt_category_id AS [CategoryID],
		wt_name AS [Name],
		wt_phrase AS [Phrase],
		wt_active AS Active
		
FROM	WinkTypes
WHERE	(@ID IS NULL OR wt_id = @ID)
		AND (@CategoryID IS NULL OR wt_category_id = @CategoryID)
		AND (@Name IS NULL OR wt_name = @Name)
		AND (@Active IS NULL OR wt_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN wt_name END



GO


