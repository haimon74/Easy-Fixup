/*
   שבת 28 אפריל 201221:28:53
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
CREATE TABLE dbo.Coupons
	(
	c_id int IDENTITY(10000,1),
	c_code nvarchar(10) NOT NULL,
	c_username nvarchar(20) NOT NULL,
	c_start_date datetime NOT NULL,
	c_due_date datetime NOT NULL,
	c_max_use int NOT NULL,
	c_type_id int NOT NULL,
	c_used int NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.Coupons ADD CONSTRAINT
	PK_Coupons PRIMARY KEY CLUSTERED 
	(
	c_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.Coupons SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Coupons', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Coupons', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Coupons', 'Object', 'CONTROL') as Contr_Per 

GO


CREATE PROCEDURE [dbo].[CreateCoupon] 
(
	@Username nvarchar(20),
	@StartDate datetime,
	@DueDate datetime,
	@Code nvarchar(10),
	@MaxUse int,
	@TypeId int
)
AS
	INSERT INTO Coupons
		(c_code, c_username, c_start_date, c_due_date, c_max_use, c_used,c_type_id)
	VALUES
		(@Code, @Username, @StartDate, @DueDate, @MaxUse, 0,@TypeId)
	SELECT @@IDENTITY
GO


CREATE PROCEDURE [dbo].[DeleteCoupon] 
(
	@Code nvarchar(10)
)
AS
	DELETE Coupons
	WHERE c_code = @Code
GO



CREATE PROCEDURE [dbo].[UpdateCoupon] 
(
	@Username nvarchar(20),
	@StartDate datetime,
	@DueDate datetime,
	@Code nvarchar(10),
	@MaxUse int,
	@TypeId int
)
AS
	UPDATE Coupons
	SET	c_username = @Username, 
		c_start_date = @StartDate, 
		c_due_date = @DueDate, 
		c_max_use = @MaxUse,
		c_type_id = @TypeId
	WHERE c_code = @Code
GO



CREATE PROCEDURE [dbo].[AddCouponUsedCounter] 
(
	@Code	NVARCHAR(10),
	@AddBy int = 1
)
AS

	UPDATE Coupons
	SET c_used = (c_used + @AddBy)
	WHERE c_code = @Code

GO



CREATE PROCEDURE [dbo].[FetchCoupon]
(
	@Code nvarchar(10)
)
 AS
	SELECT 
		c_id as Id,
		c_username as Username,
		c_code as Code,
		c_start_date as StartDate,
		c_due_date as DueDate,
		c_max_use as MaxUse,
		c_used as Used,
		c_type_id as TypeId
	FROM
		Coupons WITH (NOLOCK)
	WHERE (@Code = c_code)
GO


CREATE PROCEDURE [dbo].[FetchCoupons]
 AS
	SELECT 
		c_id as Id,
		c_username as Username,
		c_code as Code,
		c_start_date as StartDate,
		c_due_date as DueDate,
		c_max_use as MaxUse,
		c_used as Used,
		c_type_id as TypeId
	FROM
		Coupons WITH (NOLOCK)
GO


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
CREATE TABLE dbo.CouponTypes
	(
	ct_id int NOT NULL IDENTITY (1, 1),
	ct_name nvarchar(50) NOT NULL,
	ct_description nvarchar(500) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.CouponTypes ADD CONSTRAINT
	PK_CouponTypes PRIMARY KEY CLUSTERED 
	(
	ct_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.CouponTypes SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.CouponTypes', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.CouponTypes', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.CouponTypes', 'Object', 'CONTROL') as Contr_Per 

GO

INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('Premium 1 month','One month premium membership')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('Premium 2 month','Two month premium membership')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('Premium 3 month','Three month premium membership')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('10pct','Discount 10%')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('15pct','Discount 15%')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('20pct','Discount 20%')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('25pct','Discount 25%')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('30pct','Discount 30%')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('Gift 1','TBD')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('Gift 2','TBD')
INSERT INTO CouponTypes (ct_name,ct_description)
     VALUES('Gift 3','TBD')
GO


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
CREATE TABLE dbo.CouponUses
	(
	cu_code nvarchar(10) NOT NULL,
	cu_username nvarchar(20) NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.CouponUses SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.CouponUses', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.CouponUses', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.CouponUses', 'Object', 'CONTROL') as Contr_Per 


GO



CREATE PROCEDURE [dbo].[FetchCouponTypes]
 AS
	SELECT 
		ct_id as Id,
		ct_name as Name,
		ct_description as Descriptor
	FROM
		CouponTypes WITH (NOLOCK)

GO



CREATE PROCEDURE [dbo].[IsCouponUsedByUsername]
(
	@Username varchar(20),
	@Code nvarchar(10)
) AS

IF EXISTS (SELECT * from CouponUses where cu_username=@Username and cu_code=@Code)
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)



GO

CREATE PROCEDURE [dbo].[SaveCouponUsesByUsername]
(
	@Username varchar(20),
	@Code nvarchar(10)
) AS

INSERT INTO CouponUses (cu_code, cu_username) VALUES (@Code, @Username)

GO