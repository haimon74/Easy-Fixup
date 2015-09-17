ALTER TABLE dbo.EcardTypes add column ect_phrase NVARCHAR(250)
ALTER TABLE dbo.EcardTypes add column ect_category_id INT


ALTER PROCEDURE [dbo].[SaveEcardType]
	(
		@ID INT,
		@Name NVARCHAR(200),
		@Phrase NVARCHAR(250),
		@Content IMAGE,
		@Type INT,
		@Active BIT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO EcardTypes (ect_name, ect_phrase, ect_content, ect_type, ect_active)
					VALUES (@Name, @Phrase, @Content, @Type, @Active)
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE EcardTypes
		SET ect_name = @Name,
			ect_phrase = @Phrase,
			ect_content = @Content,
			ect_type = @Type,
			ect_active = @Active
		WHERE ect_id = @ID
	END

	
	
	
ALTER PROCEDURE [dbo].[FetchEcardTypes]
	(
		@ID INT,
		@Name NVARCHAR(200),
		@Type INT,
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	ect_id AS [ID],
		ect_name AS [Name],
		ect_phrase AS [Phrase],
		ect_type AS Type,
		ect_active AS Active
		
FROM	EcardTypes
WHERE	(@ID IS NULL OR ect_id = @ID)
		AND (@Name IS NULL OR ect_name = @Name)
		AND (@Type IS NULL OR ect_type = @Type)
		AND (@Active IS NULL OR ect_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ect_name END

		
		
		
		
		