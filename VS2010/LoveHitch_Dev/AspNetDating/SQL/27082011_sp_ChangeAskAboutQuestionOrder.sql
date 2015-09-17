
/****** Object:  StoredProcedure [dbo].[ChangeAskAboutQuestionOrder]    Script Date: 08/27/2011 02:06:07 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ChangeAskAboutQuestionOrder] 
	(
		@QuestionId int,
		@Direction varchar(4)
	)
AS

DECLARE @order int
	SET @order = (SELECT q_order FROM AskAboutQuestions WHERE q_id=@QuestionId)
	IF (@Direction = 'up')
		BEGIN
			DECLARE @previous_order int
			SET @previous_order = 
				(SELECT MAX(q_order) FROM AskAboutQuestions WHERE q_order <= @order - 1)
			IF (@previous_order IS NOT NULL)
				BEGIN
					UPDATE AskAboutQuestions
					SET q_order = @order
					WHERE q_order = @previous_order
					UPDATE AskAboutQuestions
					SET q_order = @previous_order
					WHERE q_id = @QuestionId
				END
		END
	ELSE
	IF (@Direction = 'down')
		BEGIN
		DECLARE @next_order int
			SET @next_order = 
				(SELECT MIN(q_order) FROM AskAboutQuestions WHERE q_order>= @order + 1)
			IF (@next_order IS NOT NULL)
				BEGIN
					UPDATE AskAboutQuestions 
					SET q_order = @order
					WHERE q_order = @next_order
					UPDATE AskAboutQuestions
					SET q_order = @next_order
					WHERE q_id = @QuestionId
				END
		END
GO


