
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.FacebookCities
	(
	fc_key nvarchar(10) NOT NULL,
	fc_name nvarchar(30) NOT NULL,
	fc_subtext nvarchar(30) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.FacebookCities ADD CONSTRAINT
	PK_FacebookCities PRIMARY KEY CLUSTERED 
	(
	fc_key
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.FacebookCities SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.FacebookCities', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.FacebookCities', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.FacebookCities', 'Object', 'CONTROL') as Contr_Per 
GO





/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
CREATE TABLE dbo.FacebookCountries
	(
	fc_id int NOT NULL IDENTITY (1, 1),
	fc_country_code nvarchar(3) NOT NULL,
	fc_name nvarchar(50) NOT NULL,
	fc_supports_region bit NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.FacebookCountries SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.FacebookCountries', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.FacebookCountries', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.FacebookCountries', 'Object', 'CONTROL') as Contr_Per 

GO



CREATE PROCEDURE [dbo].[FetchFacebookCitiesBySubtext]
(
	@SubText	nvarchar(30)
)
AS
	SELECT * from FacebookCities
	WHERE fc_subtext like ('%'+@SubText)

GO


CREATE PROCEDURE [dbo].[FetchFacebookRegionsByCountry]
(
	@Country	nvarchar(30)
)
AS
	SELECT distinct fc_subtext from FacebookCities
	WHERE fc_subtext like ('%, '+@Country)

GO


CREATE PROCEDURE [dbo].[AddFacebookCity]
	(
	@Name	nvarchar(30),
	@SubText	nvarchar(30)
)
AS
	INSERT INTO FacebookCities
			(fc_name, fc_subtext)
		VALUES
			(@Name,@SubText)
GO



CREATE PROCEDURE [dbo].[AddFacebookCountry]
	(
	@Code	nvarchar(3),
	@Name	nvarchar(50),
	@IsSupportsRegion bit = NULL
)
AS
	INSERT INTO FacebookCountries
			(fc_country_code, fc_name, fc_supports_region)
		VALUES
			(@Code,@Name,@IsSupportsRegion)
GO



CREATE PROCEDURE [dbo].[FetchFacebookCountries]
AS
	SELECT * from FacebookCountries
GO




CREATE PROCEDURE [dbo].[FetchFacebookCities]
AS
	SELECT * from FacebookCities
GO

