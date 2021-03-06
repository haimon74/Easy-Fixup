/*
   יום ראשון 12 יוני 201122:11:33
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
ALTER TABLE dbo.Languages SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Languages', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Languages', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Languages', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.EcardTypes
	DROP CONSTRAINT FK_EcardTypes_WinkCategories
GO
ALTER TABLE dbo.WinkCategories ADD
	wc_language_id int NOT NULL CONSTRAINT DF_WinkCategories_wc_language_id DEFAULT 1
GO
ALTER TABLE dbo.WinkCategories ADD CONSTRAINT
	FK_WinkCategories_Languages FOREIGN KEY
	(
	wc_language_id
	) REFERENCES dbo.Languages
	(
	l_id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.WinkCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.EcardTypes SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.EcardTypes', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.EcardTypes', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.EcardTypes', 'Object', 'CONTROL') as Contr_Per 