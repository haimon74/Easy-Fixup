/*
   יום שלישי 18 ינואר 201118:31:22
   User: 
   Server: petit\sqe
   Database: ezFixUp7
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
ALTER TABLE dbo.Languages ADD
	l_dir varchar(5) NULL
GO
ALTER TABLE dbo.Languages ADD CONSTRAINT
	DF_Languages_l_dir DEFAULT 'ltr' FOR l_dir
GO
ALTER TABLE dbo.Languages SET (LOCK_ESCALATION = TABLE)
GO
update dbo.Languages set l_dir='ltr'
GO
update dbo.Languages set l_dir='rtl' where l_language='hebrew'
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Languages', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Languages', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Languages', 'Object', 'CONTROL') as Contr_Per 