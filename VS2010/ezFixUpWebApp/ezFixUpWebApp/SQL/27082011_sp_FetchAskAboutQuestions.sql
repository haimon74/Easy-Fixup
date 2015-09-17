
/****** Object:  StoredProcedure [dbo].[FetchAskAboutQuestions]    Script Date: 08/27/2011 01:36:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[FetchAskAboutQuestions] 

AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_display as QuestionDisplay
			
		FROM AskAboutQuestions
			
		ORDER BY
			q_order

GO


