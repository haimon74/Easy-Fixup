
/****** Object:  StoredProcedure [dbo].[FetchWinkCategories]    Script Date: 06/13/2011 00:31:15 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchWinkCategories]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchWinkCategories]
GO

/****** Object:  StoredProcedure [dbo].[FetchWinkCategories]    Script Date: 06/13/2011 00:31:15 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[FetchWinkCategories]
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	wc_id AS[ID],
		wc_language_id AS LanguageID,
		wc_name AS Title
FROM WinkCategories
WHERE	(@ID IS NULL OR wc_id = @ID)
		AND (@ParentID IS NULL OR wc_language_id = @ParentID)
		AND (@Title IS NULL OR wc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN wc_name END

END

GO


