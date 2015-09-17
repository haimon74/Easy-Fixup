
/****** Object:  StoredProcedure [dbo].[SaveWinkCategory]    Script Date: 06/13/2011 01:14:31 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveWinkCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveWinkCategory]
GO

/****** Object:  StoredProcedure [dbo].[SaveWinkCategory]    Script Date: 06/13/2011 01:14:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[SaveWinkCategory]
	@ID INT,
	@ParentID INT,
	@Active BIT,
	@Title NVARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

IF @ID IS NULL
	BEGIN
		INSERT INTO WinkCategories (
		wc_language_id, 
		wc_name, wc_active) VALUES (
		@ParentID, 
		@Title, @Active)
		SELECT @@Identity
	END

ELSE
	UPDATE WinkCategories
	SET wc_language_id = @ParentID,
		wc_name = @Title,
		wc_active = @Active
	WHERE wc_id = @ID
END


GO


