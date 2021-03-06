/*
   שבת 12 מאי 201223:17:12
   User: 
   Server: .\SQLEXPRESS
   Database: dbezFixUp20120408
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
CREATE TABLE dbo.FacebookInvitations
	(
	fi_id int NOT NULL IDENTITY (1, 1),
	fi_request_id nvarchar(20) NOT NULL,
	fi_sender_id nvarchar(20) NOT NULL,
	fi_recipient_id nvarchar(20) NOT NULL,
	fi_created_time datetime NOT NULL,
	fi_is_credited bit NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.FacebookInvitations ADD CONSTRAINT
	DF_FacebookInvitations_fi_is_credited DEFAULT 0 FOR fi_is_credited
GO
ALTER TABLE dbo.FacebookInvitations ADD CONSTRAINT
	PK_FacebookInvitations PRIMARY KEY CLUSTERED 
	(
	fi_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.FacebookInvitations SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.FacebookInvitations', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.FacebookInvitations', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.FacebookInvitations', 'Object', 'CONTROL') as Contr_Per 

GO



CREATE PROCEDURE [dbo].[SaveFacebookInvitation]
(
	@Id			int = -1,
	@SenderId		nvarchar(20),
	@RecipientId		nvarchar(20),
	@RequestId		nvarchar(20),
	@CreatedOn	datetime
)
AS

IF (@Id = -1)
BEGIN

	INSERT INTO FacebookInvitation
	(fi_request_id, fi_sender_id, fi_recipient_id,fi_created_time)
	VALUES
	(@RequestId, @SenderId, @RecipientId, @CreatedOn)
	
	SELECT @@Identity
END
ELSE
	UPDATE FacebookInvitation
	SET fi_request_id = @RequestId, 
		fi_sender_id = @SenderId, 
		fi_recipient_id = @RecipientId,
		fi_created_time = @CreatedOn
	WHERE fi_id = @Id

GO



CREATE PROCEDURE [dbo].[LoadFacebookInvitation]
(
	@Id int
)
AS

Select * from FacebookInvitation
	WHERE fi_id = @Id

GO




CREATE PROCEDURE [dbo].[SearchFacebookInvitations]
(
	@SenderId		nvarchar(20),
	@RecipientId		nvarchar(20),
	@FromTime	datetime
)
AS
SELECT
	fi_id as [ID]
FROM
	FacebookInvitation
WHERE
	(@SenderId IS NULL OR @SenderId = fi_sender_id) AND
	(@RecipientId IS NULL OR @RecipientId = fi_recipient_id) AND
	(@FromTime IS NULL or fi_created_time > @FromTime)
ORDER BY fi_created_time DESC 

GO
