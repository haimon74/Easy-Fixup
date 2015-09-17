-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.FetchEcardCategories
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	ecc_id AS[ID],
		--ecc_parentid AS ParentID,
		ecc_name AS Title
FROM EcardCategories
WHERE	(@ID IS NULL OR ecc_id = @ID)
		--AND (@ParentID IS NULL OR ecc_parentid = @ParentID)
		AND (@Title IS NULL OR ecc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ecc_name END

END
GO

CREATE PROCEDURE dbo.FetchWinkCategories
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
		--ecc_parentid AS ParentID,
		wc_name AS Title
FROM WinkCategories
WHERE	(@ID IS NULL OR wc_id = @ID)
		--AND (@ParentID IS NULL OR wc_parentid = @ParentID)
		AND (@Title IS NULL OR wc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN wc_name END

END
GO

CREATE PROCEDURE dbo.FetchGiftCategories
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	gc_id AS[ID],
		--ecc_parentid AS ParentID,
		gc_name AS Title
FROM GiftCategories
WHERE	(@ID IS NULL OR gc_id = @ID)
		--AND (@ParentID IS NULL OR gc_parentid = @ParentID)
		AND (@Title IS NULL OR gc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN gc_name END

END
GO
