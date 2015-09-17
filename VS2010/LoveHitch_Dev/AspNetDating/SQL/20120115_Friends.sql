/*
   יום שני 16 ינואר 201221:58:06
   User: 
   Server: .\SQLEXPRESS
   Database: dbLoveHitch2012Jan10th
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
ALTER TABLE dbo.Friends ADD
	fb_mutualy_friends text NULL
GO
ALTER TABLE dbo.Friends SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Friends', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Friends', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Friends', 'Object', 'CONTROL') as Contr_Per 