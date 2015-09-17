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
CREATE PROCEDURE dbo.SaveEcardCategory
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF @ID IS NULL
	BEGIN
		INSERT INTO ECardCategories (
		--ecc_parentid, 
		ecc_name) VALUES (
		--@ParentID, 
		@Title)
		SELECT @@Identity
	END

ELSE
	UPDATE EcardCategories
	SET --ecc_parentid = @ParentID,
		ecc_name = @Title
	WHERE ecc_id = @ID
END
GO

CREATE PROCEDURE dbo.SaveWinkCategory
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF @ID IS NULL
	BEGIN
		INSERT INTO WinkCategories (
		--wc_parentid, 
		wc_name) VALUES (
		--@ParentID, 
		@Title)
		SELECT @@Identity
	END

ELSE
	UPDATE WinkCategories
	SET --wc_parentid = @ParentID,
		wc_name = @Title
	WHERE wc_id = @ID
END
GO

CREATE PROCEDURE dbo.SaveGiftCategory
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF @ID IS NULL
	BEGIN
		INSERT INTO GiftCategories (
		--gc_parentid, 
		gc_name) VALUES (
		--@ParentID, 
		@Title)
		SELECT @@Identity
	END

ELSE
	UPDATE GiftCategories
	SET --gc_parentid = @ParentID,
		gc_name = @Title
	WHERE gc_id = @ID
END
GO
