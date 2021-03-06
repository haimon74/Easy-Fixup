--USE [ezFixUp7]
--GO
/****** Object:  StoredProcedure [dbo].[FetchLanguage]    Script Date: 01/20/2011 16:25:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchLanguage] 
	(
		@Id	int,
		@Name nvarchar(50)
	)
AS

SELECT		l_id			AS Id,
			l_language		AS	[Name],
			l_active		AS	Active,
			l_predefined	AS	Predefined,
			l_browser_languages as BrowserLanguages,
			l_ipcountries as IpCountries,
			l_theme as Theme,
			l_dir as Direction
			
		FROM Languages
			
		WHERE	(@Id = -1 OR l_id = @Id)
				AND (@Name IS NULL	OR l_language = @Name)
