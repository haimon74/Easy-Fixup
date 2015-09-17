
/****** Object:  StoredProcedure [dbo].[SaveWinkType]    Script Date: 06/04/2011 16:45:52 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveWinkType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveWinkType]
GO


/****** Object:  StoredProcedure [dbo].[SaveWinkType]    Script Date: 06/04/2011 16:45:52 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SaveWinkType]
	(
		@ID INT,
		@CategoryID INT,
		@Name NVARCHAR(200),
		@Phrase NVARCHAR(250),
		@Active BIT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO WinkTypes (wt_category_id, wt_name, wt_phrase, wt_active)
					VALUES (@CategoryID, @Name, @Phrase, @Active)
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE WinkTypes
		SET wt_category_id=@CategoryID,
			wt_name = @Name,
			wt_phrase = @Phrase,
			wt_active = @Active
		WHERE wt_id = @ID
	END



GO


