
/****** Object:  StoredProcedure [dbo].[FetchAskAboutQuestion]    Script Date: 08/27/2011 01:36:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchAskAboutQuestion] 
	(
		@Id	int,
		@Name nvarchar(50)
	)
AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_display as QuestionDisplay
			
		FROM AskAboutQuestions
			
		WHERE	(@Id = -1 OR q_id = @Id)
				AND (@Name IS NULL	OR q_name = @Name)

GO


