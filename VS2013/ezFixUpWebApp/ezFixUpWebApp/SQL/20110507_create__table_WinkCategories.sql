/*
   יום ראשון 08 מאי 201101:10:42
   User: 
   Server: .\SQLEXPRESS
   Database: ezFixUp2ndMarch2011
   Application: 
*/

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
CREATE TABLE dbo.WinkCategories
	(
	wc_id int NOT NULL IDENTITY (1, 1),
	wc_name nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.WinkCategories ADD CONSTRAINT
	PK_WinkCategories PRIMARY KEY CLUSTERED 
	(
	wc_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.WinkCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'CONTROL') as Contr_Per 