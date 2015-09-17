SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Altering [dbo].[LoadVideoEmbeds]'
GO
ALTER PROCEDURE [LoadVideoEmbeds]
	@Id	INT = NULL,
	@Username	VARCHAR(20) = NULL,
	@NumberOfVideos INT
AS

IF @NumberOfVideos IS NOT NULL
	SET ROWCOUNT @NumberOfVideos
	
BEGIN
	SET NOCOUNT ON;

	SELECT
		ve_id	as Id,
		u_username	as Username,
		ve_thumburl as ThumbUrl,
		ve_videourl as VideoUrl,
		ve_title as Title,
		ve_sourcetype as SourceType
	FROM
		VideoEmbeds
	WHERE
		(@Id IS NULL OR ve_id = @Id)
		AND (@Username IS NULL OR u_username = @Username)
END

SET ROWCOUNT 0
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[ScheduledAnnouncements]'
GO
CREATE TABLE [dbo].[ScheduledAnnouncements]
(
[sa_id] [int] NOT NULL IDENTITY(1, 1),
[sa_name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[sa_subject] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[sa_body] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[sa_gender] [int] NULL,
[sa_paidmember] [bit] NULL,
[sa_hasphotos] [bit] NULL,
[sa_hasprofile] [bit] NULL,
[sa_languageid] [int] NULL,
[sa_country] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[sa_region] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[sa_type] [int] NOT NULL,
[sa_date] [datetime] NULL,
[sa_days] [int] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ScheduledAnnouncements] on [dbo].[ScheduledAnnouncements]'
GO
ALTER TABLE [dbo].[ScheduledAnnouncements] ADD CONSTRAINT [PK_ScheduledAnnouncements] PRIMARY KEY CLUSTERED  ([sa_id])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[FetchScheduledAnnouncement]'
GO
CREATE PROCEDURE FetchScheduledAnnouncement
	(
		@ID int,
		@Gender int,
		@PaidMember bit,
		@HasPhotos bit,
		@HasProfile bit,
		@LanguageID int,
		@Country varchar(100),
		@Region nvarchar(100),
		@Type int
	)
AS

SELECT	sa_id AS ID,
		sa_name AS [Name],
		sa_subject AS Subject,
		sa_body AS Body,
		sa_gender as Gender,
		sa_paidmember as PaidMember,
		sa_hasphotos as HasPhotos,
		sa_hasprofile as HasProfile,
		sa_languageid as LanguageID,
		sa_country as Country,
		sa_region as Region,
		sa_type as Type,
		sa_date as [Date],
		sa_days as Days

FROM	ScheduledAnnouncements

WHERE	(@ID IS NULL OR sa_id = @ID)
		AND (@Gender IS NULL OR sa_gender = @Gender)
		AND (@PaidMember IS NULL OR sa_paidmember = @PaidMember)
		AND (@HasPhotos IS NULL OR sa_hasphotos = @HasPhotos)
		AND (@HasProfile IS NULL OR sa_hasprofile = @HasProfile)
		AND (@LanguageID IS NULL OR sa_languageid = @LanguageID)
		AND (@Country IS NULL OR sa_country = @Country)
		AND (@Region IS NULL OR sa_region = @Region)
		AND (@Type IS NULL OR sa_type = @Type)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[SaveScheduledAnnouncement]'
GO
CREATE PROCEDURE SaveScheduledAnnouncement
	(
		@ID int,
		@Name nvarchar(100),
		@Subject nvarchar(255),
		@Body ntext,
		@Gender int,
		@PaidMember bit,
		@HasPhotos bit,
		@HasProfile bit,
		@LanguageID int,
		@Country varchar(100),
		@Region nvarchar(100),
		@Type int,
		@Date datetime,
		@Days int
	)
AS

IF @ID IS NULL
	BEGIN
		INSERT INTO ScheduledAnnouncements
			(sa_name, sa_subject, sa_body, sa_gender, sa_paidmember, sa_hasphotos, sa_hasprofile, sa_languageid, sa_country, sa_region, sa_type, sa_date, sa_days)
			VALUES (@Name, @Subject, @Body, @Gender, @PaidMember, @HasPhotos, @HasProfile, @LanguageID, @Country, @Region, @Type, @Date, @Days)
			
			SELECT @@Identity
	END
ELSE
	UPDATE ScheduledAnnouncements
	SET sa_name = @Name,
		sa_subject = @Subject,
		sa_body = @Body,
		sa_gender = @Gender,
		sa_paidmember = @PaidMember,
		sa_hasphotos = @HasPhotos,
		sa_hasprofile = @HasProfile,
		sa_languageid = @LanguageID,
		sa_country = @Country,
		sa_region = @Region,
		sa_type = @Type,
		sa_date = @Date,
		sa_days = @Days
	WHERE sa_id = @ID
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[DeleteScheduledAnnouncement]'
GO
CREATE PROCEDURE DeleteScheduledAnnouncement
	(
		@ID int
	)
AS

DELETE FROM ScheduledAnnouncements
WHERE sa_id = @ID
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[TwitterCredentials]'
GO
CREATE TABLE [dbo].[TwitterCredentials]
(
[u_username] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[tc_username] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[tc_password] [nvarchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_TwitterCredentials] on [dbo].[TwitterCredentials]'
GO
ALTER TABLE [dbo].[TwitterCredentials] ADD CONSTRAINT [PK_TwitterCredentials] PRIMARY KEY CLUSTERED  ([u_username])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Groups]'
GO
ALTER TABLE [dbo].[Groups] ADD
[g_autojoin] [bit] NOT NULL CONSTRAINT [DF_Groups_g_autojoin] DEFAULT ((0)),
[g_autojoincountry] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[g_autojoinregion] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[g_autojoincity] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchGroupsByUsername]'
GO
ALTER PROCEDURE [FetchGroupsByUsername] 
	(
		@Username	VARCHAR(20),
		@Approved	INT
	)
AS

SELECT
		g.g_id AS ID,
		g.g_name	AS	[Name],
		g.g_description	AS Description,
		g.g_approved AS Approved,
		g.g_datecreated AS DateCreated,
		g.g_accesslevel AS AccessLevel,
		g.g_owner AS Owner,
		g.g_activemembers AS ActiveMembers,		
		g.g_jointerms as JoinTerms,
		g.g_joinquestion as JoinQuestion,
		g.g_permissions AS [Permissions],
		g.g_minage		AS MinAge,
		g.g_autojoin as Autojoin,
		g.g_autojoincountry as AutojoinCountry,
		g.g_autojoinregion as AutojoinRegion,
		g.g_autojoincity as AutojoinCity
		
FROM Groups AS g INNER JOIN GroupMembers AS gm
			ON gm.g_id = g.g_id
			
WHERE gm.u_username = @Username AND (@Approved IS NULL OR g.g_approved = @Approved)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchAffiliateCommissions]'
GO
ALTER PROCEDURE [dbo].[FetchAffiliateCommissions] 
		(
		@ID					INT,
		@AffiliateID		INT,
		@PaymentHistoryID	INT,
		@Username			VARCHAR(20),
		@Amount				money,
		@TimeStamp			DATETIME,
		@SortColumn			INT
	)
AS

SELECT	ac_id AS ID,
		a_id AS AffiliateID,
		ph_id AS PaymentHistoryID,
		u_username AS Username,
		ac_amount AS Amount,
		ac_timestamp AS [TimeStamp],
		ac_notes AS Notes
		
FROM	AffiliateCommissions

WHERE	(@ID IS NULL OR ac_id = @ID)
		AND (@AffiliateID IS NULL OR a_id = @AffiliateID)
		AND (@PaymentHistoryID IS NULL OR ph_id = @PaymentHistoryID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Amount IS NULL OR ac_amount = @Amount)
		AND (@TimeStamp IS NULL OR ac_timestamp = @TimeStamp)

ORDER BY
		CASE WHEN @SortColumn = 1 THEN ac_timestamp END DESC,
		CASE WHEN @SortColumn = 2 THEN ac_amount END,
		CASE WHEN @SortColumn = 3 THEN u_username END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Subscriptions]'
GO
ALTER TABLE [dbo].[Subscriptions] ADD
[s_custom] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchActiveSubscription]'
GO
ALTER PROCEDURE [dbo].[FetchActiveSubscription] 
(
	@Username varchar(20)
)
AS
	SELECT
		s_id as ID,
		u_username as Username,
		p_id as PlanID,
		s_order_date as OrderDate,
		s_renew_date as RenewDate,
		s_confirmed as Confirmed,
		s_cancellation_requested as CancellationRequested,
		s_paymentprocessor as PaymentProcessor,
		s_custom as Custom
	FROM
		Subscriptions
	WHERE
		u_username = @Username AND
		s_confirmed = 1 AND
		s_cancelled = 0
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Photos]'
GO
ALTER TABLE [dbo].[Photos] ADD
[p_salute] [bit] NOT NULL CONSTRAINT [DF_Photos_p_salute] DEFAULT ((0))
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[LoadVideoUploads]'
GO
ALTER PROCEDURE [LoadVideoUploads]
	@Id	INT = NULL,
	@Username	VARCHAR(20) = NULL,
	@Processed	BIT = NULL,
	@Approved	BIT = NULL,
	@IsPrivate BIT = NULL,
	@NumberOfVideos INT
AS

IF @NumberOfVideos IS NOT NULL
	SET ROWCOUNT @NumberOfVideos
	
BEGIN
	SET NOCOUNT ON;

	SELECT
		vu_id	as Id,
		u_username	as Username,
		vu_processed	as Processed,
		vu_approved	as Approved,
		vu_private as Private
	FROM
		VideoUploads
	WHERE
		(@Id IS NULL OR vu_id = @Id)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Processed IS NULL OR vu_processed = @Processed)
		AND (@Approved IS NULL OR vu_approved = @Approved)
		AND (@IsPrivate IS NULL OR vu_private = @IsPrivate)
END

SET ROWCOUNT 0
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[Languages]'
GO
ALTER TABLE [dbo].[Languages] ADD
[l_theme] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[SaveLanguage]'
GO
ALTER PROCEDURE [SaveLanguage]
(
	@Id			int = -1,
	@Language		nvarchar(50),
	@Active		bit = 1,
	@BrowserLanguages varchar(100) = null,
	@IpCountries varchar(200) = null,
	@Theme varchar(100) = null
)
AS

IF (@Id IS NULL)
BEGIN
	INSERT INTO Languages
	(l_language, l_active, l_browser_languages, l_ipcountries, l_theme)
	VALUES
	(@Language, @Active, @BrowserLanguages, @IpCountries, @Theme)

	SELECT @@Identity
END
ELSE
	UPDATE Languages
	SET l_language = @Language,
		l_active = @Active,
		l_browser_languages = @BrowserLanguages,
		l_ipcountries= @IpCountries,
		l_theme = @Theme
	WHERE l_id = @Id
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[SaveGroup]'
GO
ALTER PROCEDURE [SaveGroup] 
	(
		@ID				INT,
		@Name			NVARCHAR(250),
		@Description	NVARCHAR(4000),
		@DateCreated	DATETIME,
		@Approved		BIT,
		@AccessLevel	INT,
		@Owner			VARCHAR(20),
		@ActiveMembers	INT,
		@JoinTerms		NTEXT,
		@JoinQuestion	NTEXT,
		@Permissions	BIGINT,
		@MinAge			INT,
		@Autojoin		BIT,
		@AutojoinCountry VARCHAR(100),
		@AutojoinRegion NVARCHAR(100),
		@AutojoinCity NVARCHAR(100)
	)
AS

IF (@ID IS NULL)

	IF EXISTS (SELECT g_name FROM Groups WHERE g_name = @Name)
		SELECT -1
		
	ELSE
		BEGIN
		
			INSERT INTO Groups
			(g_name, g_description, g_datecreated, g_approved, g_accesslevel, g_owner, g_activemembers, g_jointerms, g_joinquestion, g_permissions, g_minage, g_autojoin, g_autojoincountry, g_autojoinregion, g_autojoincity)
			VALUES
			(@Name, @Description, @DateCreated, @Approved, @AccessLevel, @Owner, @ActiveMembers, @JoinTerms, @JoinQuestion, @Permissions, @MinAge, @Autojoin, @AutojoinCountry, @AutojoinRegion, @AutojoinCity)
			
			SELECT @@Identity
		END
	
ELSE

	BEGIN
	
		UPDATE Groups
		
		SET g_name = @Name,
			g_description = @Description,
			g_datecreated = @DateCreated,
			g_approved = @Approved,
			g_accesslevel = @AccessLevel,
			g_owner = @Owner,
			g_activemembers = @ActiveMembers,
			g_jointerms = @JoinTerms,
			g_joinquestion = @JoinQuestion,
			g_permissions = @Permissions,
			g_minage = @MinAge,
			g_autojoin = @Autojoin,
			g_autojoincountry = @AutojoinCountry,
			g_autojoinregion = @AutojoinRegion,
			g_autojoincity = @AutojoinCity
			
		WHERE g_id = @ID
			
	END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchLanguage]'
GO
ALTER PROCEDURE [FetchLanguage] 
	(
		@Id	int,
		@Name nvarchar(50)
	)
AS

SELECT		l_id			AS Id,
			l_language		AS	[Name],
			l_active		AS	Active,
			l_predefined	AS	Predefined,
			l_browser_languages as BrowserLanguages,
			l_ipcountries as IpCountries,
			l_theme as Theme
			
		FROM Languages
			
		WHERE	(@Id = -1 OR l_id = @Id)
				AND (@Name IS NULL	OR l_language = @Name)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchLanguages]'
GO
ALTER PROCEDURE [FetchLanguages]
AS
	SELECT
		l_id 		as [Id],
		l_language	as Language,
		l_active		as Active,
		l_predefined	as Predefined,
		l_browser_languages as BrowserLanguages,
		l_ipcountries as IpCountries,
		l_theme as Theme
	FROM
		Languages
	ORDER BY
		l_order
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchSubscription]'
GO
ALTER PROCEDURE [dbo].[FetchSubscription] 
(
	@ID int,
	@Custom varchar(50)
)
AS
	SELECT
		s_id as ID,
		u_username as Username,
		p_id as PlanID,
		s_order_date as OrderDate,
		s_renew_date as RenewDate,
		s_confirmed as Confirmed,
		s_cancelled as Cancelled,
		s_cancellation_requested as CancellationRequested,
		s_paymentprocessor as PaymentProcessor,
		s_custom as Custom
	FROM
		Subscriptions
	WHERE
		(@ID IS NULL OR s_id = @ID) AND
		(@Custom IS NULL OR s_custom = @Custom)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchGroupsByCategory]'
GO
ALTER PROCEDURE [FetchGroupsByCategory] 
	(
		@CategoryID	INT,
		@Approved	BIT
	)
AS

SELECT
		g.g_id AS ID,
		g.g_name AS [Name],
		g.g_description AS Description,
		g.g_approved AS Approved,
		g.g_datecreated AS DateCreated,
		g.g_accesslevel AS AccessLevel,
		g.g_owner AS Owner,
		g.g_activemembers AS ActiveMembers,
		g.g_jointerms as JoinTerms,
		g.g_joinquestion as JoinQuestion,
		g.g_permissions AS [Permissions],
		g.g_minage		AS MinAge,
		g.g_autojoin as Autojoin,
		g.g_autojoincountry as AutojoinCountry,
		g.g_autojoinregion as AutojoinRegion,
		g.g_autojoincity as AutojoinCity
		
		FROM	Groups as g INNER JOIN GroupsCategories AS gc
			ON gc.g_id = g.g_id
			
		WHERE gc.c_id = @CategoryID AND (@Approved IS NULL OR g.g_approved = @Approved)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[VideoUploadsSearch]'
GO
ALTER PROCEDURE [dbo].[VideoUploadsSearch] 
( 
	@Username VARCHAR(20),
    @Gender     INT = NULL,
    @FromBirthdate	DATETIME,
	@ToBirthdate	DATETIME,
    @Approved BIT,
    @IsPrivate BIT, 
    @VideosCount   INT = 0,
    @SortColumn INT
) 
AS 
     IF (@VideosCount > 0)
	SET ROWCOUNT  @VideosCount

     SELECT 
          vu_id as Id
     FROM 
          VideoUploads
     WHERE 
		 vu_processed = 1
		 AND (@Username IS NULL OR u_username = @Username)
		 AND (@Approved IS NULL OR vu_approved = @Approved)
		 AND (@IsPrivate IS NULL OR vu_private = @IsPrivate)
         AND ((@Gender IS NULL) OR EXISTS (SELECT 1 FROM Users WHERE Users.u_username = VideoUploads.u_username AND u_gender = @Gender))
         AND (@FromBirthdate IS NULL 
						OR EXISTS(SELECT 1 FROM Users WHERE Users.u_username = VideoUploads.u_username
									AND u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		AND (@ToBirthdate IS NULL
						OR EXISTS (SELECT 1 FROM Users WHERE Users.u_username = VideoUploads.u_username
									AND u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
     ORDER BY 
          CASE WHEN @SortColumn = 1 THEN vu_id END DESC,
          CASE WHEN @SortColumn = 2 THEN u_username END

     SET ROWCOUNT 0
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[SavePhoto]'
GO
ALTER PROCEDURE [dbo].[SavePhoto]
(
	@ID	int = null,
	@Username varchar(20),
	@PhotoAlbumID int,
	@Image image = null,
	@Name	nvarchar(20),
	@Description nvarchar(200),
	@Approved bit,
	@ApprovedDate datetime,
	@Primary bit,
	@Explicit bit,
	@Private bit,
	@FaceCrop varchar(20) = null,
	@ManualApproval bit,
	@Salute bit
)
AS
	IF (@ID IS NULL)
		BEGIN
		INSERT INTO Photos
			(u_username, pa_id, p_image, p_name, p_description, p_approved, p_approvedtimestamp, p_primary, p_explicit, p_private, p_facecrop, p_manual_approval, p_salute)
		VALUES
			(@Username, @PhotoAlbumID, @Image, @Name, @Description, @Approved, @ApprovedDate, @Primary, @Explicit, @Private, @FaceCrop, @ManualApproval, @Salute)
		SELECT @@Identity
		END
	ELSE
		UPDATE Photos
		SET
			u_username = @Username,
			pa_id = @PhotoAlbumID,
			p_image = @Image,
			p_name = @Name,
			p_description = @Description,
			p_approved = @Approved,
			p_approvedtimestamp = @ApprovedDate,
			p_primary = @Primary,
			p_explicit = @Explicit,
			p_private = @Private,
			p_facecrop = @FaceCrop,
			p_manual_approval = @ManualApproval,
			p_salute = @Salute
		WHERE
			p_id = @ID
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[FetchNonApprovedPhoto]'
GO
ALTER PROCEDURE FetchNonApprovedPhoto
	(
		@ApprovedBy VARCHAR(20)
	)
AS

SELECT
	p.p_id		as Id,
	p.u_username	as Username,
	pa_id		as PhotoAlbumID,
	p_name		as Name,
	p_description	as Description,
	p_approved	as Approved,
	p_primary	as [Primary],
	p_explicit	as [Explicit],
	p_private	as Private,
	p_facecrop	as FaceCrop,
	p_approvedtimestamp as ApprovedDate,
	p_manual_approval AS ManualApproval,
	p_salute AS Salute
	
FROM Photos p 
WHERE	p.p_approved = 0
		AND p.p_private = 0
		AND p.p_manual_approval = 0
		AND NOT EXISTS (SELECT 1 FROM CommunityPhotoApproval AS cpa WHERE cpa.p_id = p.p_id AND cpa.u_username = @ApprovedBy)
		AND p.u_username != @ApprovedBy
		AND p.u_username != 'admin'
ORDER BY NEWID()
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[DeleteEcardType]'
GO
ALTER PROCEDURE DeleteEcardType
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM Ecards WHERE ect_id = @ID

DELETE FROM EcardTypes WHERE	ect_id = @ID

COMMIT
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[KeywordSearchUsers]'
GO
ALTER PROCEDURE [dbo].[KeywordSearchUsers]
(
	@Keyword		NVARCHAR(100),
	@InterestedIn	INT
)
AS
	SELECT Distinct
		Users.u_username as Username,
		u_lastonline
	FROM
		Users
	WHERE
		EXISTS (
			SELECT pa_value FROM ProfileAnswers
			WHERE Users.u_username = ProfileAnswers.u_username
			AND  pq_id NOT IN 
				(SELECT pq_id FROM [ProfileQuestions] WHERE pq_show_style = 0)
			AND ProfileAnswers.pa_value like '%'+ @Keyword + '%' ESCAPE '\')
		AND u_profilevisible = 1
		AND u_active = 1
		AND u_face_control_approved = 1
		AND u_deleted = 0
		AND (@InterestedIn IS NULL OR u_gender = @InterestedIn)
		-- the question is not hidden
	ORDER BY
		u_lastonline DESC
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[VideoEmbedsSearch]'
GO
CREATE PROCEDURE [dbo].[VideoEmbedsSearch] 
( 
	@Username VARCHAR(20),
    @Gender     INT = NULL,
    @FromBirthdate	DATETIME,
	@ToBirthdate	DATETIME,
	@Keyword NVARCHAR(100),
    @VideosCount   INT = 0,
    @SortColumn INT
) 
AS 
     IF (@VideosCount > 0)
	SET ROWCOUNT  @VideosCount

     SELECT 
          ve_id as Id
     FROM 
          VideoEmbeds
     WHERE (@Username IS NULL OR u_username = @Username)
         AND ((@Gender IS NULL) OR EXISTS (SELECT 1 FROM Users WHERE Users.u_username = VideoEmbeds.u_username AND u_gender = @Gender))
         AND (@FromBirthdate IS NULL 
						OR EXISTS(SELECT 1 FROM Users WHERE Users.u_username = VideoEmbeds.u_username
									AND u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		AND (@ToBirthdate IS NULL
						OR EXISTS (SELECT 1 FROM Users WHERE Users.u_username = VideoEmbeds.u_username
									AND u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
		AND (@Keyword IS NULL OR ve_title like '%'+ @Keyword + '%' ESCAPE '\')
     ORDER BY 
          CASE WHEN @SortColumn = 1 THEN ve_id END DESC,
          CASE WHEN @SortColumn = 2 THEN u_username END

     SET ROWCOUNT 0
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[UpdateUser]'
GO
ALTER PROCEDURE [dbo].[UpdateUser]
(
	@Username	VARCHAR(20),
	@Password	VARCHAR(250),
	@Email		VARCHAR(250),
	@Name		NVARCHAR(250),
	@Gender	INT,
	@InterestedIn	INT,
	@Birthdate	DATETIME,
	@Birthdate2	DATETIME = NULL,
	@Active	BIT,
	@SmsConfirmed BIT,
	@ReceiveEmails BIT,
	@ProfileVisible	BIT,
	@Country	VARCHAR(100),
	@State		NVARCHAR(100),
	@ZipCode	VARCHAR(15),
	@City		NVARCHAR(100),
	@Paid		bit,
	@MessageVerificationsLeft INT,
	@LanguageId	INT,
	@BillingDetails NTEXT,
	@InvitedBy	VARCHAR(20),
	@IncomingMessagesRestrictions	NTEXT,
	@Deleted	BIT,
	@AffiliateID	INT,
	@Options	BIGINT,
	@Longitude	float,
	@Latitude	FLOAT,
	@UpdatePaidStatus	BIT,
	@TokenUniqueId VARCHAR(100) = NULL,
	@PersonalizationInfo NTEXT,
	@Credits INT,
	@ModerationScore INT,
	@SpamSuspected BIT,
	@FaceControlApproved BIT,
	@ProfileSkin NVARCHAR(100) = NULL,
	@StatusText NVARCHAR(50),
	@FeaturedMember BIT,
	@MySpaceID VARCHAR(50),
	@FacebookID BIGINT,
	@EventsSettings BIGINT
)
AS
	UPDATE Users
	SET
		u_password = @Password, 
		u_email = @Email,
		u_name = @Name,
		u_gender = @Gender,
		u_interested_in = @InterestedIn,
		u_birthdate = @Birthdate,
		u_birthdate2 = @Birthdate2,
		u_active = @Active,
		u_smsconfirmed = @SmsConfirmed,
		u_receiveemails = @ReceiveEmails,
		u_profilevisible = @ProfileVisible,
		u_country = @Country,
		u_state = @State,
		u_zip_code = @ZipCode,
		u_city = @City,
		u_paid_member = CASE @UpdatePaidStatus WHEN 1 THEN @Paid ELSE [u_paid_member] END,
		u_message_verifications_left = @MessageVerificationsLeft,
		l_id = @LanguageId,
		u_billingdetails = @BillingDetails,
		u_invited_by = @InvitedBy,
		u_incoming_messages_restrictions = @IncomingMessagesRestrictions,
		u_deleted = @Deleted,
		a_id = @AffiliateID,
		u_options = @Options,
		u_longitude = @Longitude,
		u_latitude = @Latitude,
		u_tokenuniqueid = @TokenUniqueId,
		[u_personalizationinfo] = @PersonalizationInfo,
		u_credits = CASE @UpdatePaidStatus WHEN 1 THEN @Credits ELSE [u_credits] END,
		u_moderationscore = @ModerationScore,
		u_spamsuspected = @SpamSuspected,
		u_face_control_approved = @FaceControlApproved,
		u_profile_skin = @ProfileSkin,
		u_status_text = @StatusText,
		u_featuredmember = @FeaturedMember,
		u_myspaceid = @MySpaceID,
		u_facebookid = @FacebookID,
		u_eventssettings = @EventsSettings

	WHERE
		u_username = @Username
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[UpdateSubscription]'
GO
ALTER PROCEDURE [dbo].[UpdateSubscription](
	@ID int,
	@PlanID int,
	@OrderDate datetime,
	@RenewDate datetime,
	@Confirmed bit,
	@PaymentProcessor varchar(50),
	@Custom varchar(50)
)
 AS
	UPDATE Subscriptions
	SET
		p_id = @PlanID,
		s_order_date = @OrderDate,
		s_renew_date = @RenewDate,
		s_confirmed = @Confirmed,
		s_paymentprocessor = @PaymentProcessor,
		s_custom = @Custom
	WHERE
		@ID = s_id
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Altering [dbo].[PurgeUser]'
GO
ALTER PROCEDURE [dbo].[PurgeUser]
(
	@Username varchar(20)
)
 AS

	BEGIN TRANSACTION

	DELETE FROM Interests WHERE 
		i_from_username = @Username OR i_to_username = @Username

	DELETE FROM ProfileViews WHERE
		pv_viewer = @Username OR pv_viewed = @Username

	DELETE FROM Favourites WHERE
		u_username = @Username OR f_username = @Username

	DELETE FROM Messages WHERE
		m_from_username = @Username OR m_to_username = @Username
		
	DELETE FROM EstablishedCommunications WHERE
		ec_from_username = @Username OR ec_to_username = @Username

	DELETE FROM ProfileAnswers WHERE
		u_username = @Username

	DELETE FROM Ratings WHERE
		r_fromusername = @Username OR r_tousername = @Username

	DELETE FROM Subscriptions WHERE
		u_username = @Username

	DELETE FROM PhotoRatings WHERE
		p_id in (SELECT p_id FROM Photos WHERE u_username = @Username) OR
		pr_fromusername = @Username

	DELETE FROM PhotoAccess WHERE
		pa_photoowner = @Username OR pa_photoviewer = @Username
		
	DELETE FROM PhotoContestVotes WHERE
		pce_id_picked IN (SELECT pce_id FROM PhotoContestEntries WHERE
		u_username = @Username) OR
		pce_id_nonpicked IN (SELECT pce_id FROM PhotoContestEntries WHERE
		u_username = @Username) OR
		u_username = @Username
	
	DELETE FROM PhotoContestRanks WHERE
		pce_id in (SELECT pce_id FROM PhotoContestEntries WHERE
		u_username = @Username) OR u_username = @Username
	
	DELETE FROM PhotoContestEntries WHERE
		u_username = @Username

	DELETE FROM PhotoComments WHERE
		p_id IN (SELECT p_id FROM Photos WHERE u_username = @Username) OR
		u_username = @Username
		
	DELETE FROM CommunityPhotoApproval WHERE
	p_id IN (SELECT p_id FROM Photos WHERE u_username = @Username) OR
		u_username = @Username

	DELETE FROM Photos WHERE
		u_username = @Username

------------------- DELETE BLOG AND ALL RELATED DATA -------------
DECLARE @BId INT
SELECT @BId = b_id FROM Blogs WHERE u_username = @Username

	DELETE FROM BlogPostComments WHERE 
		bp_id in (SELECT bp_id FROM BlogPosts WHERE b_id = @BId)

	DELETE FROM BlogPostComments WHERE
		u_username = @Username

	DELETE FROM BlogPosts WHERE
		b_id = @BId

	DELETE FROM Blogs WHERE
		u_username = @Username
-------------------------------------------------------------------

	DELETE FROM Comments WHERE
		c_from_username = @Username OR c_to_username = @Username
		
	DELETE FROM AudioAccess WHERE
		aa_audioowner = @Username OR aa_audioviewer = @Username
	
	DELETE FROM VideoAccess WHERE
		va_videoowner = @Username OR va_videoviewer = @Username

	DELETE FROM VideoProfiles WHERE
		u_username = @Username

	DELETE FROM VerifiedUsers WHERE
		vu_verifiedby = @Username OR vu_verifieduser = @Username

	DELETE FROM BlockedUsers WHERE
		bu_userblocker = @Username OR bu_blockeduser = @Username

	DELETE FROM SavedSearches WHERE
		u_username = @Username

	DELETE FROM GroupPollsAnswers WHERE
		u_username = @Username
		
	UPDATE [Groups] SET g_activemembers = g_activemembers - 1 WHERE Groups.g_id IN (SELECT Groups.g_id FROM Groups, GroupMembers WHERE Groups.g_id = GroupMembers.g_id AND u_username = @Username)
		
	DELETE FROM GroupMembers WHERE
		u_username = @Username
		
	DELETE FROM GroupPosts WHERE
		u_username = @Username
		
	DELETE FROM GroupPhotos WHERE
		u_username = @Username
		
	DELETE FROM GroupTopics WHERE
		u_username = @Username
		
	DELETE FROM GroupBans WHERE
		u_username = @Username
		
	DELETE FROM GroupEventsComments WHERE
	u_username = @Username
	
	DELETE FROM GroupEventsAttenders
	WHERE u_username = @Username
		
	DELETE FROM GroupEvents WHERE
		u_username = @Username
		
	UPDATE [Groups] SET [g_owner] = 'admin' WHERE g_owner = @Username
		
	DELETE FROM CommunityProfileApproval WHERE
		u_username = @Username OR cpa_approvedby = @Username
		
	DELETE FROM MessagesSandBox WHERE
		u_username = @Username
	
	DELETE FROM AudioUploads WHERE
		u_username = @Username
		
	DELETE FROM [VideoEmbeds] WHERE
		[u_username] = @Username
	
	DELETE FROM EventComments WHERE
		e_id IN (SELECT e_id FROM Events WHERE e_fromusername = @Username) OR
		u_username = @Username
		
	DELETE FROM Events WHERE
		e_fromusername = @Username
		
	DELETE FROM [UserScoresLog] WHERE
		[u_username] = @Username
		
	DELETE FROM Friends WHERE
		u_username = @Username OR f_username = @Username
		
	DELETE FROM Relationships WHERE
		u_username = @Username OR r_username = @Username
		
	DELETE FROM AdComments WHERE
	u_username = @Username
	
	DELETE FROM Ads WHERE
	a_postedby = @Username
	
	DELETE FROM Ecards WHERE
		ec_from_username = @Username OR ec_to_username = @Username
		
	DELETE FROM PhotoAlbums WHERE
	u_username = @Username
	
	DELETE FROM CreditsHistory WHERE
	u_username = @Username
	
	DELETE FROM UnlockedSections WHERE
		u_username = @Username OR us_targetusername = @Username
	
	DELETE FROM PollAnswers WHERE u_username = @Username
		
	DELETE FROM Users WHERE u_username = @Username

	COMMIT TRANSACTION
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Discounts] on [dbo].[Discounts]'
GO
ALTER TABLE [dbo].[Discounts] ADD CONSTRAINT [PK_Discounts] PRIMARY KEY CLUSTERED  ([d_id])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_PhotoContestRanks] on [dbo].[PhotoContestRanks]'
GO
ALTER TABLE [dbo].[PhotoContestRanks] ADD CONSTRAINT [PK_PhotoContestRanks] PRIMARY KEY CLUSTERED  ([u_username], [pc_id], [pce_id])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_ProfileViews] on [dbo].[ProfileViews]'
GO
ALTER TABLE [dbo].[ProfileViews] ADD CONSTRAINT [PK_ProfileViews] PRIMARY KEY CLUSTERED  ([pv_viewer], [pv_viewed])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[TwitterCredentials]'
GO
ALTER TABLE [dbo].[TwitterCredentials] ADD
CONSTRAINT [FK_TwitterCredentials_Users] FOREIGN KEY ([u_username]) REFERENCES [dbo].[Users] ([u_username])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
update Settings set s_value = '<?xml version="1.0"?><BillingPlanOptions xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema"></BillingPlanOptions>' where s_key = 'Users_NonPayingMembersOptions'