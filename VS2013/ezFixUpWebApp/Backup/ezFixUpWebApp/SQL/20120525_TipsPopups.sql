IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TipsPopups]') AND type in (N'U'))
DROP TABLE [dbo].[TipsPopups]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE dbo.TipsPopups
	(
	tp_id int NOT NULL IDENTITY (1, 1),
	tp_language_id int NULL,
	tp_show_times int NULL,
	tp_login_count_trigger int NULL,
	tp_days_count_trigger int NULL,
	tp_page_name nvarchar(50) NULL,
	tp_min_credits_trigger int NULL,
	tp_no_spending_days_trigger int NULL,
	tp_content nvarchar(2000) NULL,
	tp_title nvarchar(250) NULL,
	tp_name_key nvarchar(250) NULL,
	tp_matchmaker_view bit NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.TipsPopups ADD CONSTRAINT
	PK_TipsPopups PRIMARY KEY CLUSTERED 
	(
	tp_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.TipsPopups SET (LOCK_ESCALATION = TABLE)
GO
select Has_Perms_By_Name(N'dbo.TipsPopups', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.TipsPopups', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.TipsPopups', 'Object', 'CONTROL') as Contr_Per 

go


IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_UserTipsStatus_uts_viewed_times]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[UserTipsStatus] DROP CONSTRAINT [DF_UserTipsStatus_uts_viewed_times]
END

GO

IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[DF_UserTipsStatus_uts_is_blocked]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[UserTipsStatus] DROP CONSTRAINT [DF_UserTipsStatus_uts_is_blocked]
END

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserTipsStatus]') AND type in (N'U'))
DROP TABLE [dbo].[UserTipsStatus]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE dbo.UserTipsStatus
	(
	uts_id int NOT NULL IDENTITY (1, 1),
	uts_tip_id int NOT NULL,
	uts_username nvarchar(20) NOT NULL,
	uts_viewed_times int NOT NULL,
	uts_is_blocked bit NOT NULL,
	uts_last_viewed_date datetime NOT NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.UserTipsStatus ADD CONSTRAINT
	DF_UserTipsStatus_uts_viewed_times DEFAULT 0 FOR uts_viewed_times
GO
ALTER TABLE dbo.UserTipsStatus ADD CONSTRAINT
	DF_UserTipsStatus_uts_is_blocked DEFAULT 0 FOR uts_is_blocked
GO
ALTER TABLE dbo.UserTipsStatus ADD CONSTRAINT
	PK_UserTipsStatus PRIMARY KEY CLUSTERED 
	(
	uts_id
	) WITH( STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]

GO
ALTER TABLE dbo.UserTipsStatus SET (LOCK_ESCALATION = TABLE)
GO
select Has_Perms_By_Name(N'dbo.UserTipsStatus', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.UserTipsStatus', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.UserTipsStatus', 'Object', 'CONTROL') as Contr_Per 

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateTip]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[CreateTip]
GO
CREATE PROCEDURE [dbo].[CreateTip]
	(
	@LanguageId int,
	@ShowTimes int,
	@LoginCountTrigger int,
	@DaysCountTrigger int,
	@ShowPageName nvarchar(50),
	@MinCreditsTrigger int,
	@NoSpendingDaysTrigger int,
	@ContentText nvarchar(2000),
	@TitleText nvarchar(250),
	@NameKey nvarchar(250),
	@IsMatchmakerTip bit
)
AS
	INSERT INTO TipsPopups	(
			tp_language_id,
			tp_show_times,
			tp_login_count_trigger,
			tp_days_count_trigger,
			tp_page_name,
			tp_min_credits_trigger,
			tp_no_spending_days_trigger,
			tp_content,
			tp_title,
			tp_name_key,
			tp_matchmaker_view
			)
		VALUES(
			@LanguageId,
			@ShowTimes,
			@LoginCountTrigger,
			@DaysCountTrigger,
			@ShowPageName,
			@MinCreditsTrigger,
			@NoSpendingDaysTrigger,
			@ContentText,
			@TitleText,
			@NameKey,
			@IsMatchmakerTip
			)
	SELECT @@Identity

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveTip]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveTip]
GO
CREATE PROCEDURE [dbo].[SaveTip]
	(
	@Id int,
	@NameKey nvarchar(250),
	@LanguageId int,
	@ShowTimes int,
	@LoginCountTrigger int,
	@DaysCountTrigger int,
	@ShowPageName nvarchar(50),
	@MinCreditsTrigger int,
	@NoSpendingDaysTrigger int,
	@ContentText nvarchar(2000),
	@TitleText nvarchar(250),
	@IsMatchmakerTip bit
)
AS
	UPDATE TipsPopups	
	SET
			tp_content=@ContentText,
			tp_title=@TitleText
	where tp_id = @Id

update TipsPopups
	set
			tp_show_times=@ShowTimes,
			tp_login_count_trigger=@LoginCountTrigger,
			tp_days_count_trigger=@DaysCountTrigger,
			tp_page_name=@ShowPageName,
			tp_min_credits_trigger=@MinCreditsTrigger,
			tp_no_spending_days_trigger=@NoSpendingDaysTrigger,
			tp_matchmaker_view=@IsMatchmakerTip
	where tp_name_key=@NameKey
	
go




IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchTip]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchTip]
GO
CREATE PROCEDURE [dbo].[FetchTip]
	(
	@ID int
	)
AS
	SELECT  tp_id as ID,
			tp_language_id as LanguageId,
			tp_show_times as ShowTimes,
			tp_login_count_trigger as LoginCountTrigger,
			tp_days_count_trigger as DaysCountTrigger,
			tp_page_name as ShowPageName,
			tp_min_credits_trigger as MinCreditsTrigger,
			tp_no_spending_days_trigger as NoSpendingDaysTrigger,
			tp_content as ContentText,
			tp_title as TitleText,
			tp_name_key as NameKey,
			tp_matchmaker_view as IsMatchmakerTip
	from TipsPopups	
	where tp_id=@ID

go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchTips]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchTips]
GO
CREATE PROCEDURE [dbo].[FetchTips]
	
AS
	SELECT  tp_id as ID,
			tp_language_id as LanguageId,
			tp_show_times as ShowTimes,
			tp_login_count_trigger as LoginCountTrigger,
			tp_days_count_trigger as DaysCountTrigger,
			tp_page_name as ShowPageName,
			tp_min_credits_trigger as MinCreditsTrigger,
			tp_no_spending_days_trigger as NoSpendingDaysTrigger,
			tp_content as ContentText,
			tp_title as TitleText,
			tp_name_key as NameKey,
			tp_matchmaker_view as IsMatchmakerTip
	from TipsPopups	

go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchTipsByNameKey]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchTipsByNameKey]
GO
CREATE PROCEDURE [dbo].[FetchTipsByNameKey]
	(@NameKey nvarchar(250))
AS
	SELECT  tp_id as ID,
			tp_language_id as LanguageId,
			tp_show_times as ShowTimes,
			tp_login_count_trigger as LoginCountTrigger,
			tp_days_count_trigger as DaysCountTrigger,
			tp_page_name as ShowPageName,
			tp_min_credits_trigger as MinCreditsTrigger,
			tp_no_spending_days_trigger as NoSpendingDaysTrigger,
			tp_content as ContentText,
			tp_title as TitleText,
			tp_name_key as NameKey,
			tp_matchmaker_view as IsMatchmakerTip
	from TipsPopups	
	where tp_name_key=@NameKey
go



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteTip]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteTip]
GO
CREATE PROCEDURE [dbo].[DeleteTip]
	(
	@NameKey nvarchar(250)
	)
AS
	delete from TipsPopups	
	where tp_name_key = @NameKey

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SaveUserTipStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[SaveUserTipStatus]
GO
create PROCEDURE [dbo].[SaveUserTipStatus]
	(
	@TipId	int,
	@Username	nvarchar(20),
	@ViewedTimes int,
	@IsBlocked bit,
	@LastViewedDate datetime	
)
AS
if exists (select uts_id from UserTipsStatus where uts_tip_id=@TipId and uts_username=@Username)
	begin
		update UserTipsStatus
		set	
			uts_viewed_times=@ViewedTimes,
			uts_is_blocked=@IsBlocked,
			uts_last_viewed_date=@LastViewedDate
		where uts_tip_id=@TipId and uts_username=@Username
	end
else
	begin
		INSERT INTO UserTipsStatus
			(uts_tip_id, uts_username, uts_viewed_times,uts_is_blocked,uts_last_viewed_date)
		VALUES (
			@TipId,
			@Username,
			@ViewedTimes,
			@IsBlocked,
			@LastViewedDate
			)
	SELECT @@Identity
	end
go

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchTipsStatusByUsername]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchTipsStatusByUsername]
GO
create PROCEDURE [dbo].[FetchTipsStatusByUsername]
	(
	@Username	nvarchar(20)	
)
AS
select 
	uts_tip_id as TipId,
	uts_viewed_times as ViewedTimes,
	uts_is_blocked as IsBlocked,
	uts_last_viewed_date as LastViewedDate
from UserTipsStatus 
where uts_username=@Username
	
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteTipsStatusForUsername]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteTipsStatusForUsername]
GO
create PROCEDURE [dbo].[DeleteTipsStatusForUsername]
	(
	@Username	nvarchar(20)	
)
AS
delete from UserTipsStatus 	
where uts_username=@Username
	
go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeleteUsernameTipStatus]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[DeleteUsernameTipStatus]
GO
create PROCEDURE [dbo].[DeleteUsernameTipStatus]
	(
	@Username	nvarchar(20),
	@TipId int	
)
AS
	delete from UserTipsStatus 	
	where uts_username=@Username and uts_tip_id=@TipId

go


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FetchTipsForLanguage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[FetchTipsForLanguage]
GO
create PROCEDURE [dbo].[FetchTipsForLanguage]
	(@LanguageId int = null)
AS
	SELECT  tp_id as ID,
			tp_language_id as LanguageId,
			tp_show_times as ShowTimes,
			tp_login_count_trigger as LoginCountTrigger,
			tp_days_count_trigger as DaysCountTrigger,
			tp_page_name as ShowPageName,
			tp_min_credits_trigger as MinCreditsTrigger,
			tp_no_spending_days_trigger as NoSpendingDaysTrigger,
			tp_content as ContentText,
			tp_title as TitleText,
			tp_name_key as NameKey,
			tp_matchmaker_view as IsMatchmakerTip
	from TipsPopups	
where @LanguageId is null or tp_language_id=@LanguageId

go


