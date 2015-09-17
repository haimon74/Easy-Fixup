USE [LoveHitch2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[SaveGiftType]    Script Date: 05/13/2011 04:49:07 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveGiftType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveGiftType]
GO

USE [LoveHitch2ndMarch2011]
GO

/****** Object:  StoredProcedure [dbo].[SaveGiftType]    Script Date: 05/13/2011 04:49:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SaveGiftType]
	(
		@ID INT,
		@CategoryID INT,
		@Name NVARCHAR(200),
		@Phrase NVARCHAR(250),
		@Content IMAGE,
		@Type INT,
		@Active BIT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO GiftTypes (gt_category_id, gt_name, gt_phrase, gt_content, gt_type, gt_active)
					VALUES (@CategoryID, @Name, @Phrase, @Content, @Type, @Active)
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE GiftTypes
		SET gt_category_id=@CategoryID,
			gt_name = @Name,
			gt_phrase = @Phrase,
			gt_content = @Content,
			gt_type = @Type,
			gt_active = @Active
		WHERE gt_id = @ID
	END


GO


