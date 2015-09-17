
/****** Object:  StoredProcedure [dbo].[SaveAskAboutQuestion]    Script Date: 08/27/2011 02:12:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SaveAskAboutQuestion]
(
	@Id			int = -1,
	@Name		nvarchar(50),
	@Active		bit = 1,
	@QuestionDisplay varchar(200) = null
)
AS

IF (@Id IS NULL)
BEGIN
	DECLARE @next_order int
	IF ((SELECT MAX(q_order) FROM AskAboutQuestions) IS NULL) 
	BEGIN SET @next_order = 1 END
	ELSE BEGIN SET @next_order = 1+(SELECT MAX(q_order) FROM AskAboutQuestions) END
	INSERT INTO AskAboutQuestions
	(q_name, q_active, q_display, q_order)
	VALUES
	(@Name, @Active, @QuestionDisplay, @next_order)

	SELECT @@Identity
END
ELSE
	UPDATE AskAboutQuestions
	SET q_name = @Name,
		q_active = @Active,
		q_display = @QuestionDisplay
	WHERE q_id = @Id

GO


