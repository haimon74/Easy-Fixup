
/****** Object:  StoredProcedure [dbo].[SaveGiftCategory]    Script Date: 06/13/2011 01:20:18 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveGiftCategory]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveGiftCategory]
GO

/****** Object:  StoredProcedure [dbo].[SaveGiftCategory]    Script Date: 06/13/2011 01:20:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SaveGiftCategory]
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
		INSERT INTO GiftCategories (
		--gc_parentid, 
		gc_name, gc_active) VALUES (
		--@ParentID, 
		@Title, @Active)
		SELECT @@Identity
	END

ELSE
	UPDATE GiftCategories
	SET --gc_parentid = @ParentID,
		gc_name = @Title, gc_active = @Active
	WHERE gc_id = @ID
END

GO


