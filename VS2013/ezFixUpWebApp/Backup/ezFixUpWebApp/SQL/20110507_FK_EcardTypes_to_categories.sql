/*
   יום ראשון 08 מאי 201101:35:56
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
ALTER TABLE dbo.WinkCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.WinkCategories', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.GiftCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.GiftCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.GiftCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.GiftCategories', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.ECardCategories SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.ECardCategories', 'Object', 'CONTROL') as Contr_Per BEGIN TRANSACTION
GO
ALTER TABLE dbo.EcardTypes ADD CONSTRAINT
	FK_EcardTypes_ECardCategories FOREIGN KEY
	(
	ect_category_id
	) REFERENCES dbo.ECardCategories
	(
	ecc_id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.EcardTypes ADD CONSTRAINT
	FK_EcardTypes_GiftCategories FOREIGN KEY
	(
	ect_category_id
	) REFERENCES dbo.GiftCategories
	(
	gc_id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.EcardTypes ADD CONSTRAINT
	FK_EcardTypes_WinkCategories FOREIGN KEY
	(
	ect_category_id
	) REFERENCES dbo.WinkCategories
	(
	wc_id
	) ON UPDATE  NO ACTION 
	 ON DELETE  NO ACTION 
	
GO
ALTER TABLE dbo.EcardTypes SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.EcardTypes', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.EcardTypes', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.EcardTypes', 'Object', 'CONTROL') as Contr_Per 