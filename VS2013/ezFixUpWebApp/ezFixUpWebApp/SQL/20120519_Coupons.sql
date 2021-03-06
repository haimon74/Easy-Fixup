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
ALTER TABLE dbo.Coupons ADD
	c_only_new_user bit NOT NULL CONSTRAINT DF_Coupons_c_only_new_user DEFAULT 1
GO
ALTER TABLE dbo.Coupons SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Coupons', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Coupons', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Coupons', 'Object', 'CONTROL') as Contr_Per 

go

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[CreateCoupon] 
(
	@Username varchar(20),
	@StartDate datetime,
	@DueDate datetime,
	@Code nvarchar(10),
	@MaxUse int,
	@TypeId int,
	@OnlyNewUser bit = 1
)
AS
	INSERT INTO Coupons
		(c_code, c_username, c_start_date, c_due_date, c_max_use, c_used,c_type_id, c_only_new_user)
	VALUES
		(@Code, @Username, @StartDate, @DueDate, @MaxUse, 0,@TypeId,@OnlyNewUser)
	SELECT @@IDENTITY

go




SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCoupon]
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
		c_type_id as TypeId,
		c_only_new_user as OnlyNewUser
	FROM
		Coupons WITH (NOLOCK)
	WHERE (@Code = c_code)

	go
	
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[FetchCoupons]
 AS
	SELECT 
		c_id as Id,
		c_username as Username,
		c_code as Code,
		c_start_date as StartDate,
		c_due_date as DueDate,
		c_max_use as MaxUse,
		c_used as Used,
		c_type_id as TypeId,
		c_only_new_user as OnlyNewUser
	FROM
		Coupons WITH (NOLOCK)

		
		go
		
		
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UpdateCoupon] 
(
	@Username nvarchar(20),
	@StartDate datetime,
	@DueDate datetime,
	@Code nvarchar(10),
	@MaxUse int,
	@TypeId int,
	@OnlyNewUser bit = 1
)
AS
	UPDATE Coupons
	SET	c_username = @Username, 
		c_start_date = @StartDate, 
		c_due_date = @DueDate, 
		c_max_use = @MaxUse,
		c_type_id = @TypeId,
		c_only_new_user = @OnlyNewUser

	WHERE c_code = @Code
go


