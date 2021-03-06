/*
   יום ראשון 08 מאי 201101:10:42
   User: 
   Server: .\SQLEXPRESS
   Database: LoveHitch2ndMarch2011
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
CREATE TABLE dbo.ECardCategories
	(
	ecc_id int NOT NULL IDENTITY (1, 1),
	ecc_name nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ECardCategories ADD CONSTRAINT
	PK_ECardCategories PRIMARY KEY CLUSTERED 
	(
	ecc_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.ECardCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'CONTROL') as Contr_Per 