
/****** Object:  StoredProcedure [dbo].[FetchFriends]    Script Date: 03/01/2012 21:16:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchFriends]
	(
		@Username varchar(20)
	)
AS
	SELECT f_username as Friend FROM Friends (NOLOCK), Users (NOLOCK)
		WHERE @Username = Friends.u_username AND
			(Friends.f_username = Users.u_username) AND
			 Users.u_deleted = 0

			 

/****** Object:  StoredProcedure [dbo].[FetchFavourites]    Script Date: 03/01/2012 21:17:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchFavourites]
(
	@Username varchar(20)
)
 AS
	SELECT f_username as Favourite FROM Favourites (NOLOCK), Users (NOLOCK)
		WHERE @Username = Favourites.u_username AND
			Favourites.f_username = Users.u_username AND
			 Users.u_deleted = 0

			 


/****** Object:  StoredProcedure [dbo].[FetchMutuallyFriends]    Script Date: 03/01/2012 21:19:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchMutuallyFriends]
	(
		@Username varchar(20)
	)
 AS

	SELECT 
		f_username as Friend 
	FROM 
		Friends (NOLOCK) as F1

	WHERE u_username = @Username
		AND EXISTS (SELECT u_username FROM Users WHERE Users.u_username = F1.f_username
					AND Users.u_deleted = 0 
					AND Users.u_profilevisible = 1)
		AND EXISTS (SELECT f_username FROM Friends as F2 WHERE F2.f_username = @Username
					AND F2.u_username = F1.f_username)

					

/****** Object:  StoredProcedure [dbo].[FetchActiveSubscription]    Script Date: 03/01/2012 21:20:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		Subscriptions (NOLOCK)
	WHERE
		u_username = @Username AND
		s_confirmed = 1 AND
		s_cancelled = 0

		

/****** Object:  StoredProcedure [dbo].[FetchAdmins]    Script Date: 03/01/2012 21:21:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchAdmins]
(
	@Username	VARCHAR(20) = null
)
AS
	SELECT
		a_password as Password,
		a_username as Username,
		a_lastlogin as LastLogin,
		a_privileges as [Privileges]
	FROM 
		Admins (NOLOCK)
	WHERE
		@Username IS null OR @Username = a_username


		

/****** Object:  StoredProcedure [dbo].[FetchAdPhoto]    Script Date: 03/01/2012 21:23:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAdPhoto]
	(
		@ID int
	)
AS

SELECT ap_image as [Image]
FROM AdPhotos (NOLOCK)
WHERE ap_id = @ID



/****** Object:  StoredProcedure [dbo].[FetchAdPhotos]    Script Date: 03/02/2012 17:16:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAdPhotos]
	(
		@ID int,
		@AdID int,
		@NumberOfPhotos int
	)
AS

IF @NumberOfPhotos IS NOT NULL
	SET ROWCOUNT @NumberOfPhotos
	
SELECT	ap_id as [ID],
		a_id as AdID,
		ap_description as Description
FROM AdPhotos (NOLOCK)
WHERE	(@ID IS NULL OR ap_id = @ID)
		AND (@AdID IS NULL OR a_id = @AdID)
ORDER BY ap_id

SET ROWCOUNT 0



/****** Object:  StoredProcedure [dbo].[FetchAds]    Script Date: 03/02/2012 17:18:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAds]
	(
		@ID int,
		@CategoryID int,
		@PostedBy varchar(20),
		@Date datetime,
		@ExpirationDate datetime,
		@Approved bit,
		@NumberOfAds int,
		@SortColumn int
	)
AS

IF @NumberOfAds IS NOT NULL
	SET ROWCOUNT @NumberOfAds
SELECT	a_id as [ID],
		ac_id as CategoryID,
		a_postedby as PostedBy,
		a_date as [Date],
		a_expirationdate as ExpirationDate,
		a_location as Location,
		a_subject as Subject,
		a_description as Description,
		a_approved as Approved
FROM	Ads (NOLOCK)
WHERE	(@ID IS NULL OR a_id = @ID)
		AND (@CategoryID IS NULL OR ac_id = @CategoryID)
		AND (@PostedBy IS NULL OR a_postedby = @PostedBy)
		AND (@Date IS NULL OR a_date = @Date)
		AND (@ExpirationDate IS NULL OR a_expirationdate = @ExpirationDate)
		AND (@Approved IS NULL OR a_approved = @Approved)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN a_date END DESC,
		CASE WHEN @SortColumn = 2 THEN a_expirationdate END DESC,
		CASE WHEN @SortColumn = 3 THEN ac_id END,
		CASE WHEN @SortColumn = 4 THEN a_postedby END,
		CASE WHEN @SortColumn = 5 THEN a_approved END
SET ROWCOUNT 0



/****** Object:  StoredProcedure [dbo].[FetchAdsCategories]    Script Date: 03/02/2012 17:19:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAdsCategories]
	(
		@ID INT,
		@ParentID INT,
		@Title NVARCHAR(200),
		@SortColumn INT
	)
AS

SELECT	ac_id AS[ID],
		ac_parentid AS ParentID,
		ac_title AS Title
FROM AdsCategories (NOLOCK)
WHERE	(@ID IS NULL OR ac_id = @ID)
		AND (@ParentID IS NULL OR ac_parentid = @ParentID)
		AND (@Title IS NULL OR ac_title = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ac_title END

		

/****** Object:  StoredProcedure [dbo].[FetchAffiliateBannerImage]    Script Date: 03/02/2012 17:20:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAffiliateBannerImage] 
	(
		@ID INT
	)
AS

SELECT ab_image AS [Image]
FROM AffiliateBanners  (NOLOCK)
WHERE ab_id = @ID



/****** Object:  StoredProcedure [dbo].[FetchAffiliateBanners]    Script Date: 03/02/2012 17:21:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAffiliateBanners] 
	(
		@ID			INT,
		@Name		NVARCHAR(200),
		@Deleted	BIT
	)
AS

SELECT	ab_id AS [ID],
		ab_name AS [Name],
		ab_deleted AS Deleted
		
FROM	AffiliateBanners (NOLOCK)

WHERE	(@ID IS NULL OR ab_id = @ID)
		AND (@Name IS NULL OR ab_name = @Name)
		AND (@Deleted IS NULL OR ab_deleted = @Deleted)

		

/****** Object:  StoredProcedure [dbo].[FetchAffiliateCommissions]    Script Date: 03/02/2012 17:21:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		
FROM	AffiliateCommissions (NOLOCK)

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

		

/****** Object:  StoredProcedure [dbo].[FetchAffiliates]    Script Date: 03/02/2012 17:22:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAffiliates] 
	(
		@ID				INT,
		@Username		VARCHAR(20),
		@Deleted		BIT,
		@Active			BIT,
		@Percentage		INT,
		@FixedAmount	MONEY,
		@Recurrent		BIT,
		@Balance		MONEY,
		@RequestPayment BIT,
		@SortColumn		INT
	)
AS

SELECT	a_id AS ID,
		a_username AS Username,
		a_password AS Password,
		a_name AS [Name],
		a_email AS Email,
		a_siteurl AS SiteURL,
		a_payment_details AS PaymentDetails,
		a_deleted AS Deleted,
		a_active AS Active,
		a_percentage AS Percentage,
		a_fixed_amount AS FixedAmount,
		a_recurrent AS Recurrent,
		a_balance AS Balance,
		a_request_payment AS RequestPayment
		
FROM	Affiliates (NOLOCK)

WHERE	(@ID IS NULL OR a_id = @ID)
		AND (@Username IS NULL OR a_username = @Username)
		AND (@Deleted IS NULL OR a_deleted = @Deleted)
		AND (@Active IS NULL OR a_active = @Active)
		AND (@Percentage IS NULL OR a_percentage = @Percentage)
		AND (@FixedAmount IS NULL OR a_fixed_amount = @FixedAmount)
		AND (@Recurrent IS NULL OR a_recurrent = @Recurrent)
		AND (@Balance IS NULL OR a_balance = @Balance)
		AND (@RequestPayment IS NULL OR a_request_payment = @RequestPayment)

ORDER BY
		CASE WHEN @SortColumn = 1 THEN a_username END,
		CASE WHEN @SortColumn = 2 THEN a_name END,
		CASE WHEN @SortColumn = 3 THEN a_deleted END,
		CASE WHEN @SortColumn = 4 THEN a_active END,
		CASE WHEN @SortColumn = 5 THEN a_balance END

		

/****** Object:  StoredProcedure [dbo].[FetchAffiliatesHistory]    Script Date: 03/02/2012 17:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAffiliatesHistory] 
	(
		@ID				INT,
		@AffiliateID	INT,
		@Amount			MONEY,
		@DatePaid		DATETIME,
		@SortColumn		INT
	)
AS

SELECT	ah_id AS ID,
		a_id AS AffiliateID,
		ah_amount AS Amount,
		ah_date_paid AS DatePaid,
		ah_notes AS Notes,
		ah_private_notes AS PrivateNotes

FROM	AffiliatesHistory (NOLOCK)

WHERE	(@ID IS NULL OR ah_id = @ID)
		AND (@AffiliateID IS NULL OR a_id = @AffiliateID)
		AND (@Amount IS NULL OR ah_amount = @Amount)
		AND (@DatePaid IS NULL OR ah_date_paid = @DatePaid)
		
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ah_amount END,
		CASE WHEN @SortColumn = 2 THEN ah_date_paid END DESC

		

/****** Object:  StoredProcedure [dbo].[FetchAskAboutQuestion]    Script Date: 03/02/2012 17:23:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchAskAboutQuestion] 
	(
		@Id	int,
		@Name nvarchar(50)
	)
AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_display as QuestionDisplay
			
		FROM AskAboutQuestions (NOLOCK)
			
		WHERE	(@Id = -1 OR q_id = @Id)
				AND (@Name IS NULL	OR q_name = @Name)


				

/****** Object:  StoredProcedure [dbo].[FetchAskAboutQuestions]    Script Date: 03/02/2012 17:23:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchAskAboutQuestions] 

AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_display as QuestionDisplay
			
		FROM AskAboutQuestions (NOLOCK)
			
		ORDER BY
			q_order


/****** Object:  StoredProcedure [dbo].[FetchAverageVote]    Script Date: 03/02/2012 17:23:45 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchAverageVote]
(
	@FromUsername	VARCHAR(20)
)
AS
SELECT
	COUNT(r_rating) as Votes,
	AVG(CAST(r_rating as decimal)) as AverageVote
FROM
	Ratings (NOLOCK)
WHERE
	r_fromusername = @FromUsername

	

/****** Object:  StoredProcedure [dbo].[FetchBillingPlan]    Script Date: 03/02/2012 17:25:58 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchBillingPlan]
(
	@ID int = null
)
 AS
	SELECT
		p_id as PlanID,
		p_title as Title,
		p_amount as Amount,
		p_billing_cycle	as Cycle,
		p_billing_cycle_unit as CycleUnit,
		p_options AS Options
	FROM
		BillingPlans (NOLOCK)
	WHERE
		(@ID IS NULL AND p_deleted = 0)
		OR 
		p_id = @ID

		

/****** Object:  StoredProcedure [dbo].[FetchBillingPlanBySubscriptionID]    Script Date: 03/02/2012 17:26:05 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchBillingPlanBySubscriptionID]
(
	@SubscriptionID int
)
 AS
	SELECT
		BillingPlans.p_id as PlanID,
		p_title as Title,
		p_amount as Amount,
		p_billing_cycle	as Cycle,
		p_billing_cycle_unit as CycleUnit,
		p_options AS Options
	FROM
		BillingPlans (NOLOCK), Subscriptions (NOLOCK)
	WHERE
		Subscriptions.s_id = @SubscriptionID AND  BillingPlans.p_id = Subscriptions.p_id

		

/****** Object:  StoredProcedure [dbo].[FetchBillingPlanIDByPlanData]    Script Date: 03/02/2012 17:26:10 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchBillingPlanIDByPlanData]
(
	@Amount money,
	@Cycle int,
	@CycleUnit tinyint
) AS

	
SELECT TOP 1
	p_id
	FROM
		BillingPlans (NOLOCK)
	WHERE
		@Amount = p_amount AND 
		@Cycle = p_billing_cycle AND 
		@CycleUnit = p_billing_cycle_unit
	ORDER BY p_id DESC

	

/****** Object:  StoredProcedure [dbo].[FetchBlockedUsers]    Script Date: 03/02/2012 17:28:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchBlockedUsers]
	(
		@Username VARCHAR(20)
	)
AS

SELECT	bu_blockeduser AS BlockedUser,
		bu_blockdate AS BlockedOn
FROM BlockedUsers (NOLOCK)
WHERE bu_userblocker = @Username



/****** Object:  StoredProcedure [dbo].[FetchBlogPostComments]    Script Date: 03/02/2012 17:29:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchBlogPostComments]
(
	@Id			INT = -1,
	@BlogPostId INT = -1,
	@Username VARCHAR(20),
	@Approved		BIT = NULL
)
AS

SELECT
	bpc_id	as Id,
	bp_id	as BlogPostId,
	u_username as Username,
	bpc_text	as CommentText,
	bpc_dateposted	as DatePosted,
	bpc_approved		as Approved
FROM
	BlogPostComments (NOLOCK)
WHERE
	(@Id IS NULL OR @Id = -1 OR bpc_id = @Id)
	AND (@BlogPostId IS NULL OR @BlogPostId = -1 OR bp_id = @BlogPostId)
	AND (@Username IS NULL OR u_username = @Username)
	AND (@Approved IS NULL OR bpc_approved = @Approved)
ORDER BY
	bpc_dateposted DESC

	

/****** Object:  StoredProcedure [dbo].[FetchBlogPosts]    Script Date: 03/02/2012 17:29:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchBlogPosts]
(
	@BlogId INT = -1,
	@BlogPostId INT = -1,
	@Approved BIT
)
AS

SELECT
	bp_id		as Id,
	b_id		as BlogId,
	bp_title	as Title,
	bp_content	as Content,
	bp_dateposted	as DatePosted,
	bp_reads	as Reads,
	bp_approved as Approved
FROM
	BlogPosts WITH (NOLOCK)
WHERE
	(@BlogId IS NULL OR @BlogId = -1 OR b_id = @BlogId)
	AND (@BlogPostId IS NULL OR @BlogPostId = -1 OR bp_id = @BlogPostId)
	AND (@Approved IS NULL OR bp_approved = @Approved)
ORDER BY
	bp_dateposted DESC

	

/****** Object:  StoredProcedure [dbo].[FetchCategories]    Script Date: 03/02/2012 17:34:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCategories]
	(
		@ID		INT,
		@Name	NVARCHAR(200),
		@Order	INT
	)
AS

SELECT
		c_id	AS ID,
		c_name	AS [Name],
		c_order	AS [Order],
		c_userscancreategroups AS UsersCanCreateGroups

FROM	Categories WITH (NOLOCK)

WHERE	(@ID IS NULL OR c_id = @ID)
		AND (@Name IS NULL OR c_name = @Name)
		AND (@Order IS NULL OR c_order = @Order)
		
ORDER BY c_order



/****** Object:  StoredProcedure [dbo].[FetchChatAdmins]    Script Date: 03/02/2012 17:34:41 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchChatAdmins]
(
	@Username	VARCHAR(20) = null
)
AS
	SELECT
		ca_password as Password,
		ca_username as Username,
		ca_lastlogin as LastLogin
	FROM 
		ChatAdmins WITH (NOLOCK)
	WHERE
		@Username = null OR @Username = ca_username

		
		
		

/****** Object:  StoredProcedure [dbo].[FetchCategoriesByGroup]    Script Date: 03/02/2012 17:34:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCategoriesByGroup]
	(
		@GroupID	INT
	)
AS

SELECT	
		c.c_id AS ID,
		c.c_name AS [Name],
		c.c_order AS [Order],
		c.c_userscancreategroups AS UsersCanCreateGroups

FROM Categories (NOLOCK) AS c INNER JOIN GroupsCategories (NOLOCK) AS gc
			ON gc.c_id = c.c_id
		WHERE gc.g_id = @GroupID

		
		

/****** Object:  StoredProcedure [dbo].[FetchChatIgnores]    Script Date: 03/02/2012 17:38:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchChatIgnores] 
(
	@UserId	int,
	@IgnoredUserId	int
)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		cu_id as UserId,
		ciu_ignoreduserid as IgnoredUserId,
		ciu_ignoredate	as IgnoreDate
	FROM
		ChatIgnoredUsers WITH (NOLOCK)
	WHERE
		(@UserId IS NULL OR cu_id = @UserId) AND
		(@IgnoredUserId IS NULL OR ciu_ignoreduserid = @IgnoredUserId)
END



/****** Object:  StoredProcedure [dbo].[FetchChatBans]    Script Date: 03/02/2012 17:37:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchChatBans]
	@Id int = null,
	@ChatRoomId int = null,
	@UserId int = null,
	@UserIp varchar(15) = null,
	@ExpiresAfter datetime = getdate
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		cb_id as Id,
		cr_id as ChatRoomId,
		cu_id as UserId,
		cb_ip as UserIp,
		cb_date as BanDate,
		cb_dateexpires as BanExpires
	FROM
		ChatBans WITH (NOLOCK)
	WHERE
		(@Id is null OR @Id = cu_id)
		AND (@ChatRoomId is null or @ChatRoomId = cr_id)
		AND (@UserId is null or @UserId = cu_id)
		AND (@UserIp is null or @UserIp = cb_ip)
		AND (@ExpiresAfter is null or cb_dateexpires is null or @ExpiresAfter < cb_dateexpires)
END





/****** Object:  StoredProcedure [dbo].[FetchChatRooms]    Script Date: 03/02/2012 17:41:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchChatRooms] 
(
	@ID	INT,
	@Name	NVARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT 
		cr_id		as Id,
		cr_name		as Name,
		cr_topic	as Topic,
		cr_password as Password,
		cr_maxusers as MaxUsers,
		cr_visible	as Visible
	FROM ChatRooms WITH (NOLOCK)
	WHERE
		(@ID IS NULL OR @ID = cr_id) AND
		(@Name IS NULL OR @Name = cr_name)
END

GO


/****** Object:  StoredProcedure [dbo].[FetchChatTranslationKeys]    Script Date: 03/02/2012 17:42:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchChatTranslationKeys] 
(
	@LanguageId	INT = 1
)
AS

SELECT 
	DISTINCT ct_key as [Key]
FROM
	ChatTranslations WITH (NOLOCK)


/****** Object:  StoredProcedure [dbo].[FetchChatTranslation]    Script Date: 03/02/2012 17:42:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchChatTranslation] 
(
	@LanguageId	INT = 1,
	@Key		NVARCHAR(400)
)
AS

SELECT
	ct_value as Value
FROM
	ChatTranslations WITH (NOLOCK)
WHERE
	l_id = @LanguageId AND
	ct_key = @Key


/****** Object:  StoredProcedure [dbo].[FetchChatSetting]    Script Date: 03/02/2012 17:41:54 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchChatSetting]
(
	@Key	varchar(80)
)
 AS
	SELECT cs_value FROM ChatSettings  WITH (NOLOCK) WHERE @Key = cs_key



/****** Object:  StoredProcedure [dbo].[FetchChatRoomAdmins]    Script Date: 03/02/2012 17:41:01 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchChatRoomAdmins] 
(
	@ChatRoomId int
)
AS

SELECT cu_id as UserId FROM ChatRoomAdmins WHERE cr_id = @ChatRoomId

GO


/****** Object:  StoredProcedure [dbo].[FetchChatMessages]    Script Date: 03/02/2012 17:40:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchChatMessages] 
(
	@RoomId	int,
	@FromId	int = null,
	@SenderId	int = null,
	@TargetId	int = null,
	@TargetIdCanBeNull	bit = 0,
	@NotIgnoredById	int = null
)
AS
BEGIN
	SET NOCOUNT ON;

    SELECT top 50
		cm_id	as Id,
		cm_type as [Type],
		cm_time	as [Time],
		cr_id	as RoomId,
		cu_id_sender	as SenderId,
		cm_senderdisplayname	as SenderDisplayName,
		cu_id_target	as TargetId,
		cm_text	as [Text],
		cm_texthtml	as TextHtml
	FROM
		ChatMessages
	WHERE (@RoomId IS NULL OR cr_id = @RoomId)
		AND (@FromId IS NULL OR cm_id > @FromId)
		AND (@SenderId IS NULL OR cu_id_sender = @SenderId)
		AND (@TargetId IS NULL OR cu_id_target = @TargetId OR 
				(@TargetIdCanBeNull = 1 AND cu_id_target IS NULL))
		AND (@NotIgnoredById IS NULL OR NOT EXISTS
				(SELECT 1 FROM ChatIgnoredUsers 
				WHERE cu_id = @NotIgnoredById 
					AND ciu_ignoreduserid = cu_id_sender))
	ORDER BY cm_id DESC
END

GO


USE [dbezFixUp2012Jan10th]
GO
/****** Object:  StoredProcedure [dbo].[FetchChatUsers]    Script Date: 03/02/2012 17:46:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchChatUsers]
	@Id int = null,
	@Username varchar(100) = null,
	@DisplayName nvarchar(200) = null,
	@Ip varchar(15) = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT
		cu_id as Id,
		cu_username as Username,
		cu_displayname as DisplayName,
		cu_password	as Password,
		cu_gender as Gender,
		cu_begintime as BeginTime,
		cu_lastactive as LastActive,
		cu_ip as Ip,
		cu_lastactiveguid as [Guid],
		cu_thumbimage as ThumbImage
	FROM
		ChatUsers WITH (NOLOCK)
	WHERE
		(@Id is null OR @Id = cu_id)
		AND (@Username is null OR @Username = cu_username)
		AND (@DisplayName is null OR @DisplayName = cu_displayname)
		AND (@Ip is null OR @Ip = cu_ip)
END
/****** Object:  StoredProcedure [dbo].[FetchClientChatTranslations]    Script Date: 03/02/2012 17:47:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchClientChatTranslations] 
(
	@LanguageId	INT = 1
)
AS

SELECT
	ct_key as [Key],
	ct_value as [Value]
FROM
	ChatTranslations WITH (NOLOCK)
WHERE
	l_id = @LanguageId AND ct_clientside = 1

/****** Object:  StoredProcedure [dbo].[FetchComments]    Script Date: 03/02/2012 17:47:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchComments]
(
	@Id			INT = NULL,
	@FromUsername	VARCHAR(20) = NULL,
	@ToUsername		VARCHAR(20) = NULL,
	@Approved		BIT = NULL
)
AS

SELECT
	c_id	as Id,
	c_from_username	as FromUsername,
	c_to_username		as ToUsername,
	c_comment_text		as CommentText,
	c_date_posted		as DatePosted,
	c_approved		as Approved
FROM
	Comments WITH (NOLOCK)
WHERE
	(@Id IS NULL OR @Id = -1 OR c_id = @Id)
	AND (@FromUsername IS NULL OR c_from_username = @FromUsername)
	AND (@ToUsername IS NULL OR c_to_username = @ToUsername)
	AND (@Approved IS NULL OR c_approved = @Approved)
ORDER BY
	c_date_posted DESC

/****** Object:  StoredProcedure [dbo].[FetchCommunityPhotoApproval]    Script Date: 03/02/2012 17:48:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCommunityPhotoApproval]
	(
		@ID INT,
		@PhotoID INT,
		@Username VARCHAR(20),
		@FromDate DATETIME,
		@ToDate DATETIME,
		@SortColumn INT
	)
AS

SELECT	cpa_id AS ID,
		u_username AS Username,
		p_id AS PhotoID,
		cpa_approved AS Approved,
		cpa_timestamp AS [Date]
		
FROM CommunityPhotoApproval WITH (NOLOCK)

WHERE	(@ID IS NULL OR cpa_id = @ID)
		AND (@PhotoID IS NULL OR p_id = @PhotoID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@FromDate IS NULL OR cpa_timestamp >= @FromDate) AND (@ToDate IS NULL OR cpa_timestamp <= @ToDate)

ORDER BY

CASE WHEN @SortColumn = 1 THEN u_username END,
CASE WHEN @SortColumn = 2 THEN cpa_timestamp END DESC


/****** Object:  StoredProcedure [dbo].[FetchCommunityProfileApproval]    Script Date: 03/02/2012 17:49:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCommunityProfileApproval]
	(
		@ID INT,
		@Username VARCHAR(20),
		@ApprovedBy VARCHAR(20),
		@FromDate DATETIME,
		@ToDate DATETIME,
		@SortColumn INT
	)
AS

SELECT	cpa_id AS ID,
		u_username AS Username,
		cpa_approvedby AS ApprovedBy,
		cpa_approved AS Approved,
		cpa_timestamp AS [Date]
		
FROM CommunityProfileApproval WITH (NOLOCK)

WHERE	(@ID IS NULL OR cpa_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@ApprovedBy IS NULL OR cpa_approvedby = @ApprovedBy)
		AND (@FromDate IS NULL OR cpa_timestamp >= @FromDate) AND (@ToDate IS NULL OR cpa_timestamp <= @ToDate)

ORDER BY

CASE WHEN @SortColumn = 1 THEN u_username END,
CASE WHEN @SortColumn = 2 THEN cpa_approvedby END,
CASE WHEN @SortColumn = 3 THEN cpa_timestamp END DESC


/****** Object:  StoredProcedure [dbo].[FetchContentPages]    Script Date: 03/02/2012 17:50:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchContentPages]
( 
	@ID			INT,
	@Language	INT,
	@SortColumn	INT
)

AS
 
SELECT cp_id AS [ID],
	 cp_title AS Title,
	 cp_content AS Content,
	 cp_header_position AS HeaderPosition,
	 cp_footer_position AS FooterPosition,
	 cp_visiblefor as VisibleFor,
	 cp_url AS URL,
	 cp_metadescription AS MetaDescription,
	 cp_metakeyword AS MetaKeyword,
	 [l_id] AS [LanguageID],
	 cp_urlrewrite as UrlRewrite

FROM ContentPage WITH (NOLOCK)

WHERE (@ID IS NULL OR @ID = cp_id) 
	AND (@Language IS NULL OR @Language = l_id)

ORDER BY
		CASE WHEN @SortColumn = 1 THEN cp_header_position END, cp_footer_position,
		CASE WHEN @SortColumn = 2 THEN cp_footer_position END, cp_header_position

/****** Object:  StoredProcedure [dbo].[FetchContentViews]    Script Date: 03/02/2012 17:50:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchContentViews]
( 
	@Key varchar(50),
	@Language INT
)

AS
	SELECT cv_key AS [Key],
		 cv_content AS [Content],
		 l_id as [Language]
	FROM ContentViews WITH (NOLOCK)
	WHERE (@Key IS NULL OR @Key = cv_key) AND
		  (@Language IS NULL OR @Language = l_id)


/****** Object:  StoredProcedure [dbo].[FetchCreditsHistory]    Script Date: 03/02/2012 17:50:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCreditsHistory]
	(
		@ID INT,
		@Username VARCHAR(20),
		@Date DATETIME,
		@SortColumn INT
	)
AS

SELECT	ch_id AS [ID],
		u_username AS Username,
		ch_date AS [DATE],
		ch_service AS Service,
		ch_amount AS Amount

FROM CreditsHistory WITH (NOLOCK)

WHERE	(@ID IS NULL OR ch_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Date IS NULL OR ch_date = @Date)

ORDER BY
	CASE WHEN @SortColumn = 1 THEN ch_date END DESC,
	CASE WHEN @SortColumn = 2 THEN u_username END

	
/****** Object:  StoredProcedure [dbo].[FetchCreditsHistoryByUsername]    Script Date: 03/02/2012 17:53:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCreditsHistoryByUsername]
	(
		@Username VARCHAR(20)
	)
AS

SELECT	ch_date AS [Date],
		ch_service AS [Service],
		SUM(ch_amount) AS Credits

FROM CreditsHistory

WHERE u_username = @Username

GROUP BY ch_date, ch_service

ORDER BY ch_date DESC


/****** Object:  StoredProcedure [dbo].[FetchDetailsOfNonApprovedPhotos]    Script Date: 03/02/2012 17:53:43 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchDetailsOfNonApprovedPhotos]
 AS
	SELECT
		u_username	as Username,
		p_name		as Name,
		p_description	as Description,
		p_id		as PhotoID,
		p_explicit	as [Explicit],
		p_private	as Private
	FROM
		Photos WITH (NOLOCK)
	WHERE
		p_approved=0


/****** Object:  StoredProcedure [dbo].[FetchCreditsPackages]    Script Date: 03/02/2012 17:53:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchCreditsPackages]
	(
		@ID INT,
		@Name NVARCHAR(200),
		@Quantity INT,
		@Price MONEY,
		@SortColumn INT
	)
AS

SELECT	cp_id AS [ID],
		cp_name AS [Name],
		cp_quantity AS Quantity,
		cp_price AS Price
		
FROM CreditsPackages WITH (NOLOCK)

WHERE	(@ID IS NULL OR cp_id = @ID)
		AND (@Name IS NULL OR cp_id = @Name)
		AND (@Quantity IS NULL OR cp_id = @Quantity)
		AND (@Price IS NULL OR cp_id = @Price)
		
ORDER BY
		CASE WHEN @SortColumn = 1 THEN cp_price END


/****** Object:  StoredProcedure [dbo].[FetchDiscounts]    Script Date: 03/02/2012 17:53:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchDiscounts]
(
	@ID int
)
 AS
	SELECT 
		d_id as Id,
		d_amount as Amount,
		d_type as Type,
		d_field as Field,
		d_match as Match
	FROM
		Discounts WITH (NOLOCK)
	WHERE (@ID = -1 OR @ID = d_id)


/****** Object:  StoredProcedure [dbo].[FetchEcardCategories]    Script Date: 03/02/2012 17:53:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
ALTER PROCEDURE [dbo].[FetchEcardCategories]
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	ecc_id AS[ID],
		--ecc_parentid AS ParentID,
		ecc_name AS Title
FROM EcardCategories WITH (NOLOCK)
WHERE	(@ID IS NULL OR ecc_id = @ID)
		--AND (@ParentID IS NULL OR ecc_parentid = @ParentID)
		AND (@Title IS NULL OR ecc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ecc_name END

END


/****** Object:  StoredProcedure [dbo].[FetchEcards]    Script Date: 03/02/2012 17:57:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchEcards]
	(
		@ID INT,
		@EcardTypeID INT,
		@FromUsername VARCHAR(20),
		@ToUsername VARCHAR(20),
		@IsOpened BIT,
		@SortColumn INT
	)
AS

SELECT	ec_id AS [ID],
		ect_id AS EcardTypeID,
		ec_from_username AS FromUsername,
		ec_to_username AS ToUsername,
		ec_date AS [Date],
		ec_message AS Message,
		ec_deleted_by_fromuser AS DeletedByFromUser,
		ec_deleted_by_touser AS DeletedByToUser,
		ec_is_opened AS IsOpened
FROM Ecards

WHERE
		(@ID IS NULL OR ec_id = @ID)
		AND (@EcardTypeID IS NULL OR ect_id = @EcardTypeID)
		AND (@FromUsername IS NULL OR ec_deleted_by_fromuser = 0)
		AND (@ToUsername IS NULL OR ec_deleted_by_touser = 0)
		AND (@FromUsername IS NULL OR ec_from_username = @FromUsername)
		AND (@ToUsername IS NULL OR ec_to_username = @ToUsername)
		AND (@IsOpened IS NULL OR ec_is_opened = @IsOpened)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ec_date END DESC

/****** Object:  StoredProcedure [dbo].[FetchEcardTypes]    Script Date: 03/02/2012 17:58:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchEcardTypes]
	(
		@ID INT,
		@CategoryID INT,
		@Name NVARCHAR(200),
		@Type INT,
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	ect_id AS [ID],
		ect_category_id AS [CategoryID],
		ect_name AS [Name],
		ect_phrase AS [Phrase],
		ect_type AS Type,
		ect_active AS Active
		
FROM	EcardTypes WITH (NOLOCK)
WHERE	(@ID IS NULL OR ect_id = @ID)
		AND (@CategoryID IS NULL OR ect_category_id = @CategoryID)
		AND (@Name IS NULL OR ect_name = @Name)
		AND (@Type IS NULL OR ect_type = @Type)
		AND (@Active IS NULL OR ect_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ect_name END


/****** Object:  StoredProcedure [dbo].[FetchEcardTypeContent]    Script Date: 03/02/2012 17:58:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchEcardTypeContent]
	(
		@ID INT
	)
AS

SELECT ect_content AS Content
FROM EcardTypes WITH (NOLOCK)
WHERE ect_id = @ID


/****** Object:  StoredProcedure [dbo].[FetchEmails]    Script Date: 03/02/2012 17:58:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchEmails]
 AS
	SELECT
		u_email as Email
	FROM
		Users WITH (NOLOCK)
	WHERE
		u_active = 1 AND u_deleted = 0 AND u_face_control_approved = 1


/****** Object:  StoredProcedure [dbo].[FetchEmailTemplate]    Script Date: 03/02/2012 17:58:29 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchEmailTemplate]
(
	@name varchar(50)
)
 AS
	SELECT
		et_subject as Subject,
		et_body	    as Body
	FROM
		EmailTemplates WITH (NOLOCK)
	WHERE
		et_name = @name

		
/****** Object:  StoredProcedure [dbo].[FetchEstablishedCommunications]    Script Date: 03/02/2012 18:01:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchEstablishedCommunications]
	(
		@FromUsername VARCHAR(20),
		@ToUsername VARCHAR(20),
		@Date DATETIME
	)
AS

SELECT	ec_from_username AS FromUsername,
		ec_to_username AS ToUsername,
		ec_date AS [Date]
		
FROM EstablishedCommunications WITH (NOLOCK)
		
WHERE	(@FromUsername IS NULL OR ec_from_username = @FromUsername)
		AND (@ToUsername IS NULL OR ec_to_username = @ToUsername)
		AND (@Date IS NULL OR ec_date = @Date)


/****** Object:  StoredProcedure [dbo].[FetchFAQ]    Script Date: 03/02/2012 18:01:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchFAQ] 
	(
		@Id	int,
		@Name nvarchar(50)
	)
AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_question as QuestionDisplay,
			q_answer as AnswerDisplay,
			q_category_id as CategoryId			
		FROM FAQs WITH (NOLOCK)
			
		WHERE	(@Id = -1 OR q_id = @Id)
				AND (@Name IS NULL	OR q_name = @Name)

/****** Object:  StoredProcedure [dbo].[FetchFaqCategories]    Script Date: 03/02/2012 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[FetchFaqCategories]
	@ID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	fc_id AS[ID],
		--ecc_parentid AS ParentID,
		fc_name AS Title
FROM FaqCategories WITH (NOLOCK)
WHERE	(@ID IS NULL OR fc_id = @ID)
		--AND (@ParentID IS NULL OR fc_parentid = @ParentID)
		AND (@Title IS NULL OR fc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN fc_name END

END


/****** Object:  StoredProcedure [dbo].[FetchFAQs]    Script Date: 03/02/2012 18:01:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchFAQs] 
	(
		@CategoryId	int
	)
AS

SELECT		q_id			AS Id,
			q_name		AS	[Name],
			q_active		AS	Active,
			q_question as QuestionDisplay,
			q_answer as AnswerDisplay,
			q_category_id as CategoryId
			
		FROM FAQs WITH (NOLOCK)
		WHERE q_category_id = @CategoryId
		
		ORDER BY
			q_order


/****** Object:  StoredProcedure [dbo].[FetchFavoritesNewPhotos]    Script Date: 03/02/2012 21:28:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchFavoritesNewPhotos]
	(
		@Username VARCHAR(20),
		@FromDate DATETIME
	)
AS

SELECT DISTINCT f.f_username AS Username
FROM favourites (NOLOCK) as f, photos (NOLOCK) as p 
WHERE	f.u_username = @Username
		AND f.f_username = p.u_username
		AND p.p_approved = 1
		AND p.p_approvedtimestamp > @FromDate


/****** Object:  StoredProcedure [dbo].[FetchFavouriteTimeStamp]    Script Date: 03/02/2012 21:29:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchFavouriteTimeStamp]
(
	@Username	varchar(20),
	@Favourite	varchar(20)
)
 AS
	SELECT f_timestamp FROM Favourites  (NOLOCK)
	WHERE @Username = u_username AND @Favourite = f_username

	
/****** Object:  StoredProcedure [dbo].[FetchGiftCategories]    Script Date: 03/02/2012 21:31:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchGiftCategories]
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	gc_id AS[ID],
		--ecc_parentid AS ParentID,
		gc_name AS Title
FROM GiftCategories (NOLOCK)
WHERE	(@ID IS NULL OR gc_id = @ID)
		--AND (@ParentID IS NULL OR gc_parentid = @ParentID)
		AND (@Title IS NULL OR gc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN gc_name END

END

/****** Object:  StoredProcedure [dbo].[FetchGiftsOfMessage]    Script Date: 03/02/2012 21:32:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchGiftsOfMessage]
(
	@MessageID	INT = 0	
)
AS
	SELECT TOP 10
		gm_id AS Id, gm_gift_id AS GiftId, gm_message_id AS MessageId
	
	FROM GiftsMessages (NOLOCK)
	WHERE (@MessageID = 0 OR @MessageID = gm_message_id)

/****** Object:  StoredProcedure [dbo].[FetchGiftTypeContent]    Script Date: 03/02/2012 21:35:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchGiftTypeContent]
	(
		@ID INT
	)
AS

SELECT gt_content AS Content
FROM GiftTypes (NOLOCK)
WHERE gt_id = @ID



/****** Object:  StoredProcedure [dbo].[FetchGiftTypes]    Script Date: 03/02/2012 21:36:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[FetchGiftTypes]
	(
		@ID INT,
		@CategoryID INT,
		@Price INT,
		@Name NVARCHAR(200),
		@Type INT,
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	gt_id AS [ID],
		gt_category_id AS [CategoryID],
		gt_price AS [Price],
		gt_name AS [Name],
		gt_phrase AS [Phrase],
		gt_type AS ItsType,
		gt_active AS Active
		
FROM	GiftTypes (NOLOCK)
WHERE	(@ID IS NULL OR gt_id = @ID)
		AND (@CategoryID IS NULL OR gt_category_id = @CategoryID)
		AND (@Price IS NULL OR gt_price = @Price)
		AND (@Name IS NULL OR gt_name = @Name)
		AND (@Type IS NULL OR gt_type = @Type)
		AND (@Active IS NULL OR gt_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN gt_name END



/****** Object:  StoredProcedure [dbo].[FetchHourlyStatsForNewUsers]    Script Date: 03/02/2012 21:37:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchHourlyStatsForNewUsers]
	(
		@From DATETIME,
		@To DATETIME,
		@Type INT
	)
AS

IF (@Type = 0) -- hourly
	BEGIN
		SELECT	hs_time AS [Time],
				hs_newusers AS NewUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
	END
ELSE IF (@Type = 1) -- daily
	BEGIN
		SELECT	CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME) AS [Time],
				SUM(hs_newusers) AS NewUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME)
	END
ELSE IF (@Type = 2) -- weekly
	BEGIN
		SELECT	CAST( FLOOR( CAST( MIN(hs_time) AS FLOAT))AS DATETIME) AS [Time],
				SUM(hs_newusers) AS NewUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY datepart(year, hs_time), datepart(week, hs_time)
	END
ELSE IF (@Type = 3) -- monthly
	BEGIN
		SELECT  CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
					CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
					CAST(1 AS VARCHAR(2)) AS DATETIME) AS [Time],
		SUM(hs_newusers) AS NewUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
						CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
						CAST(1 AS VARCHAR(2)) AS DATETIME)
	END


/****** Object:  StoredProcedure [dbo].[FetchHourlyStatsForOnlineUsers]    Script Date: 03/02/2012 21:37:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchHourlyStatsForOnlineUsers]
	(
		@From DATETIME,
		@To DATETIME,
		@Type INT
	)
AS

IF (@Type = 0) -- hourly
	BEGIN
		SELECT	hs_time AS [Time],
				hs_usersonline AS OnlineUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
	END
ELSE IF (@Type = 1) -- daily
	BEGIN
		SELECT	CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME) AS [Time],
				AVG(hs_usersonline) AS OnlineUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME)
	END
ELSE IF (@Type = 2) -- weekly
	BEGIN
		SELECT	CAST( FLOOR( CAST( MIN(hs_time) AS FLOAT))AS DATETIME) AS [Time],
				AVG(hs_usersonline) AS OnlineUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY datepart(year, hs_time), datepart(week, hs_time)
	END
ELSE IF (@Type = 3) -- monthly
	BEGIN
		SELECT  CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
					CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
					CAST(1 AS VARCHAR(2)) AS DATETIME) AS [Time],
		SUM(hs_usersonline) AS OnlineUsers
		FROM	HourlyStats (NOLOCK)
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
						CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
						CAST(1 AS VARCHAR(2)) AS DATETIME)
	END


/****** Object:  StoredProcedure [dbo].[FetchInactiveUsers]    Script Date: 03/02/2012 21:38:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchInactiveUsers]
(
	@ForTheLastXDays	int
)
 AS

SELECT u_username as Username
	FROM Users  (NOLOCK)
WHERE u_active = 0 AND 
	datediff(day,u_usersince,getdate()) > @ForTheLastXDays AND u_face_control_approved = 1


/****** Object:  StoredProcedure [dbo].[FetchInterests]    Script Date: 03/02/2012 21:39:22 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchInterests]
(
	@Id			INT = 0,
	@FromUsername	VARCHAR(20) = NULL,
	@ToUsername		VARCHAR(20) = NULL,
	@NewInterests		BIT
)
AS
	SELECT
		i_id			AS Id,
		i_from_username	AS FromUsername,
		i_to_username		AS ToUsername,
		i_date_posted		AS DatePosted,
		i_deleted_by_fromuser	AS DeletedByFromUser,
		i_deleted_by_touser	AS DeletedByToUser

	FROM Interests (NOLOCK)
	WHERE (@Id = 0 OR @Id = i_id)
		AND (@FromUsername IS NULL OR i_deleted_by_fromuser = 0)
		AND (@ToUsername IS NULL OR i_deleted_by_touser = 0)
		AND (@FromUsername IS NULL OR 
				(@FromUsername = i_from_username))
		AND (@ToUsername IS NULL OR 
				(@NewInterests = 0 AND @ToUsername = i_to_username)  
			OR
				(@NewInterests = 1 AND @ToUsername =  i_to_username AND
					EXISTS (SELECT u_username FROM Users  (NOLOCK) WHERE u_username = @ToUsername AND i_date_posted > u_prevlogin)))
	ORDER BY i_date_posted DESC



/****** Object:  StoredProcedure [dbo].[FetchLanguage]    Script Date: 03/02/2012 21:41:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchLanguage] 
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
			l_theme as Theme,
			l_dir as Direction
			
		FROM Languages (NOLOCK)
			
		WHERE	(@Id = -1 OR l_id = @Id)
				AND (@Name IS NULL	OR l_language = @Name)

/****** Object:  StoredProcedure [dbo].[FetchLanguages]    Script Date: 03/02/2012 21:41:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchLanguages]
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
		Languages (NOLOCK)
	ORDER BY
		l_order


/****** Object:  StoredProcedure [dbo].[FetchMatchmakings]    Script Date: 03/02/2012 21:42:25 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[FetchMatchmakings]
(
	@Id			INT = 0,
	@MatchmakerUsername	VARCHAR(20) = null,
	@ToFriendUsername	VARCHAR(20) = null,
	@WithFriendUsername	VARCHAR(20) = null,
	@Status		VARCHAR(20) = null,
	@ToFriendAck	BIT = null,
	@WithFriendAck	BIT = null	
)
AS
	SELECT TOP 100
		mm_id			AS Id,
		mm_matchmaker_username AS MatchmakerUsername,
		mm_to_friend_username AS ToFriendUsername,
		mm_with_friend_username AS WithFriendUsername,
		mm_status AS MatchStatus,
		mm_to_friend_ack AS ToFriendAck,
		mm_with_friend_ack AS WithFriendAck,
		mm_timestamp AS MatchDateTime
		
	FROM Matchmakings (NOLOCK)
	WHERE (@Id = 0 OR @Id = mm_id)
	AND (@MatchmakerUsername IS NULL OR @MatchmakerUsername = mm_matchmaker_username)
	AND (@ToFriendUsername IS NULL OR @ToFriendUsername = mm_to_friend_username)
	AND (@WithFriendUsername IS NULL OR @WithFriendUsername = mm_with_friend_username)
	AND (@Status IS NULL OR @Status = mm_status)
	AND (@ToFriendAck IS NULL OR mm_to_friend_ack = @ToFriendAck)
	AND (@WithFriendAck IS NULL OR mm_with_friend_ack = @WithFriendAck)
	ORDER BY mm_timestamp DESC


/****** Object:  StoredProcedure [dbo].[FetchMessages]    Script Date: 03/02/2012 21:42:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchMessages]
(
	@Id			INT = 0,
	@FromUsername	VARCHAR(20) = null,
	@FromFolder		INT = 0,
	@ToUsername		VARCHAR(20) = null,
	@ToFolder		INT = 0,
	@InPastDays		INT = 0,
	@PendingApproval	BIT = null,
	@New			BIT,
	@IsRead			BIT = NULL,
	@Keyword		NVARCHAR(200)
)
AS
	SELECT TOP 1000
		m_id			AS Id
	
	FROM Messages (NOLOCK)
	WHERE (@Id = 0 OR @Id = m_id)
	AND (@FromUsername IS NULL OR @FromUsername = m_from_username)
	AND (@FromFolder = 0 OR @FromFolder = m_from_folder)
	AND (@ToUsername IS NULL OR @ToUsername = m_to_username)
	AND (@ToFolder = 0 OR @ToFolder = m_to_folder)
	AND (@InPastDays = 0 OR (datediff(day, m_timestamp, getdate()) <= @InPastDays))
	AND (@PendingApproval IS NULL OR m_pending_approval = @PendingApproval)
	AND (@New = 0 OR @New = m_new)
	AND (@IsRead IS NULL OR m_is_read = @IsRead)
	AND (@Keyword IS NULL OR m_body like '%' + @Keyword + '%')
	ORDER BY m_timestamp DESC


/****** Object:  StoredProcedure [dbo].[FetchMessagesCount]    Script Date: 03/02/2012 21:43:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchMessagesCount]
	(
		@From DATETIME,
		@To DATETIME
	)

AS

SELECT COUNT(1)
FROM Messages (NOLOCK)
WHERE m_timestamp BETWEEN @From AND @To


/****** Object:  StoredProcedure [dbo].[FetchMessagesSandBox]    Script Date: 03/02/2012 21:44:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchMessagesSandBox]
	(
		@Username VARCHAR(20),
		@Message NVARCHAR(100),
		@FromDate DATETIME,
		@ToDate DATETIME,
		@MaxSameMessage INT,
		@SelectOnlyOneRecord BIT
	)
AS

IF @SelectOnlyOneRecord IS NOT NULL
	SET ROWCOUNT 1
	
SELECT ms_message AS Message
FROM MessagesSandBox (NOLOCK)
WHERE	(@Username IS NULL OR u_username = @Username)
		AND (@Message IS NULL OR ms_message = @Message)
		AND (@FromDate IS NULL OR ms_datetime >= @FromDate)
		AND (@ToDate IS NULL OR ms_datetime <= @ToDate)
		AND  ms_count > @MaxSameMessage
ORDER BY ms_count DESC

SET ROWCOUNT 0



/****** Object:  StoredProcedure [dbo].[FetchMutuallyFavourites]    Script Date: 03/02/2012 21:44:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchMutuallyFavourites]
(
	@Username varchar(20)
)
 AS

	SELECT 
		f_username as Favourite 
	FROM 
		Favourites (NOLOCK) as F1

	WHERE u_username = @Username
		AND EXISTS (SELECT u_username FROM Users WHERE Users.u_username = F1.f_username
					AND Users.u_deleted = 0 
					AND Users.u_profilevisible = 1)
		AND EXISTS (SELECT f_username FROM Favourites as F2 WHERE F2.f_username = @Username
					AND F2.u_username = F1.f_username)




/****** Object:  StoredProcedure [dbo].[FetchNewCommentsCount]    Script Date: 03/02/2012 21:46:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchNewCommentsCount]
	(
		@Username	VARCHAR(20)
	)
AS
DECLARE @LastOnline datetime

SELECT @LastOnline = u_prevlogin FROM Users (NOLOCK) where u_username = @Username

SELECT COUNT(1) AS Comments
FROM Comments (NOLOCK)
WHERE c_to_username = @Username AND c_date_posted > @LastOnline


/****** Object:  StoredProcedure [dbo].[FetchNewPhotoCommentsCount]    Script Date: 03/02/2012 21:46:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchNewPhotoCommentsCount]
	(
		@Username	VARCHAR(20)
	)
AS
DECLARE @LastOnline datetime

SELECT @LastOnline = u_prevlogin FROM Users (NOLOCK) where u_username = @Username

SELECT COUNT(1) AS PhotoComments
FROM PhotoComments (NOLOCK) AS pc INNER JOIN Photos  (NOLOCK) AS p
		ON pc.p_id = p.p_id 
WHERE p.u_username = @Username AND pc.pc_date > @LastOnline


/****** Object:  StoredProcedure [dbo].[FetchNews]    Script Date: 03/02/2012 21:47:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchNews]
( 
	@ID INT,
	@NewsCount INT,
	@LanguageID INT
)
AS

SET ROWCOUNT @NewsCount

	IF (	@ID <> -1 AND 
		NOT EXISTS(SELECT n_id FROM News (NOLOCK) WHERE @ID = n_id AND @LanguageID = l_id) AND 
		EXISTS (SELECT n_id FROM News  (NOLOCK) WHERE @LanguageID = l_id)
	    )
		SELECT TOP 1 
			n_id as NID,
			n_date as Date,
			n_text as Text,
			n_title as Title
		FROM
			News (NOLOCK)
		WHERE
			@LanguageID = l_id
		ORDER BY
			n_date DESC
	ELSE
		SELECT
			n_id as NID,
			n_date as Date,
			n_text as Text,
			n_title as Title
		FROM
			News (NOLOCK)
	
		WHERE 
			(@ID = -1 OR @ID = n_id) AND @LanguageID = l_id
		ORDER BY
			n_date DESC

SET ROWCOUNT 0



/****** Object:  StoredProcedure [dbo].[FetchNewTopics]    Script Date: 03/02/2012 21:48:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchNewTopics]
	(
		@Username	VARCHAR(20)
	)
AS

DECLARE @LastOnline datetime

SELECT @LastOnline = u_prevlogin FROM Users (NOLOCK) where u_username = @Username

SELECT gt_id AS GroupTopicID
FROM GroupTopics  (NOLOCK) gt
	JOIN GroupMembers  (NOLOCK) gm ON gt.g_id = gm.g_id
	--JOIN Groups	g ON gt.g_id = g.g_id
WHERE gm.u_username = @Username AND gt_datecreated > @LastOnline


/****** Object:  StoredProcedure [dbo].[FetchNonApprovedAnswers]    Script Date: 03/02/2012 21:50:29 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchNonApprovedAnswers]
AS
	SELECT
		u_username	as Username,
		pq_id		as QuestionID,
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers (NOLOCK)
	WHERE
		pa_approved=0



/****** Object:  StoredProcedure [dbo].[FetchNonApprovedPhoto]    Script Date: 03/02/2012 21:50:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchNonApprovedPhoto]
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
	
FROM Photos p (NOLOCK)
WHERE	p.p_approved = 0
		AND p.p_private = 0
		AND p.p_manual_approval = 0
		AND NOT EXISTS (SELECT 1 FROM CommunityPhotoApproval AS cpa WHERE cpa.p_id = p.p_id AND cpa.u_username = @ApprovedBy)
		AND p.u_username != @ApprovedBy
		AND p.u_username != 'admin'
ORDER BY NEWID()




/****** Object:  StoredProcedure [dbo].[FetchNonFaceControlApprovedUser]    Script Date: 03/02/2012 21:51:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchNonFaceControlApprovedUser]
	(
		@ApprovedBy VARCHAR(20),
		@Gender INT,
		@MinPhotosRequired INT
	)
AS

DECLARE @HasProfile BIT
SELECT
		u_password as Password,
		u.u_username as Username,
		u_email as Email,
		u_name as Name,
		u_gender as Gender,
		u_interested_in as InterestedIn,
		u_birthdate as Birthdate,
		u_birthdate2 as Birthdate2,
		u_active as Active,
		u_smsconfirmed as SmsConfirmed,
		u_usersince as UserSince,
		u_prevlogin as PrevLogin,
		u_lastlogin as LastLogin,
		u_logincount as LoginCount,
		u_lastonline as LastOnline,
		u_profileviews as ProfileViews,
		u_profilevisible as ProfileVisible,
		u_receiveemails as ReceiveEmails,
		u_country as Country,
		u_state as State,
		u_zip_code as ZipCode,
		u_city as City,
		u_deleted as Deleted,
		u_paid_member as Paid,
		u_lastsessionid as LastSessionID,
		u_message_verifications_left as MessageVerificationsLeft,
		u_signup_ip as SignupIP,
		l_id as LanguageId,
		u_billingdetails as BillingDetails,
		u_invited_by as InvitedBy,
		u_incoming_messages_restrictions as IncomingMessagesRestrictions,
		u_deletereason as DeleteReason,
		a_id as AffiliateID,
		u_options as Options,
		u_longitude as Longitude,
		u_latitude	as Latitude,
		u_score as Score,
		u_tokenuniqueid as TokenUniqueId,
		[u_personalizationinfo] AS PersonalizationInfo,
		u_credits AS Credits,
		u_moderationscore AS ModerationScores,
		u_spamsuspected AS SpamSuspected,
		u_face_control_approved AS FaceControlApproved,
		u_myspaceid AS MySpaceID,
		u_facebookid AS FacebookID

	FROM Users (NOLOCK) AS u
	WHERE	
			(dbo.fnHasProfile(u.u_username, 0) = 1)
			AND(SELECT COUNT(1) FROM Photos p WHERE p.u_username = u.u_username) >= @MinPhotosRequired
			AND u.u_deleted = 0
			AND u.u_active = 1
			AND (@Gender IS NULL OR u.u_gender = @Gender)
			AND u.u_face_control_approved = 0
			AND NOT EXISTS (SELECT 1 FROM CommunityProfileApproval AS cpa WHERE cpa.u_username = u.u_username AND cpa.cpa_approvedby = @ApprovedBy)
			AND u.u_username != @ApprovedBy
			AND u.u_username != 'admin'
	ORDER BY NEWID()

	
	
	/****** Object:  StoredProcedure [dbo].[FetchPhoto]    Script Date: 03/02/2012 21:52:28 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPhoto]
(
	@ID	int
)
AS
	SELECT
		p_image	as Image
	FROM
		Photos
	WHERE
		p_id = @ID


/****** Object:  StoredProcedure [dbo].[FetchPhotoAlbums]    Script Date: 03/02/2012 21:52:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPhotoAlbums]
	(
		@ID int,
		@Username varchar(20),
		@Name nvarchar(50),
		@Access int
	)
AS

SELECT	pa_id as ID,
		u_username as Username,
		pa_name as [Name],
		pa_access as Access,
		pa_cover_photoid as CoverPhotoID
		
FROM	PhotoAlbums (NOLOCK)

WHERE	(@ID IS NULL OR pa_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Name IS NULL OR pa_name = @Name)
		AND (@Access IS NULL OR pa_access = @Access)


/****** Object:  StoredProcedure [dbo].[FetchPhotoComments]    Script Date: 03/02/2012 21:52:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPhotoComments]
	(
		@ID					INT,
		@PhotoID			INT,
		@Username			VARCHAR(20),
		@NumberOfComments	INT
	)
AS

IF @NumberOfComments IS NOT NULL
	SET ROWCOUNT @NumberOfComments

SELECT	pc_id AS [ID],
		p_id AS PhotoID,
		u_username AS Username,
		pc_comment AS Comment,
		pc_date AS [Date]

FROM PhotoComments (NOLOCK)

WHERE	(@ID IS NULL OR pc_id = @ID)
		AND (@PhotoID IS NULL OR p_id = @PhotoID)
		AND (@Username IS NULL OR u_username = @Username)
		
ORDER BY pc_date DESC

SET ROWCOUNT 0



/****** Object:  StoredProcedure [dbo].[FetchPhotoContestVotePercentage]    Script Date: 03/02/2012 21:53:24 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPhotoContestVotePercentage]
	@Entry1Id		INT,
	@Entry2Id		INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @Picked1Count INT
	DECLARE @Picked2Count INT

	SELECT
		@Picked1Count = COUNT(pce_id_picked)
	FROM
		PhotoContestVotes (NOLOCK)
	WHERE
		pce_id_picked = @Entry1Id AND pce_id_nonpicked = @Entry2Id

	SELECT
		@Picked2Count = COUNT(pce_id_picked)
	FROM
		PhotoContestVotes (NOLOCK)
	WHERE
		pce_id_picked = @Entry2Id AND pce_id_nonpicked = @Entry1Id

	IF (@Picked1Count = 0 AND @Picked2Count = 0) 
		SELECT NULL
	ELSE
		SELECT (100.0 / CAST((@Picked1Count + @Picked2Count) AS DECIMAL)) 
				* CAST(@Picked1Count AS DECIMAL)

END



/****** Object:  StoredProcedure [dbo].[FetchPhotoDetails]    Script Date: 03/02/2012 21:54:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPhotoDetails]
(
	@Id			INT = -1,
	@Username	VARCHAR(20) = NULL,
	@PhotoAlbumID INT,
	@Approved	BIT = NULL,
	@ManualApproved BIT,
	@FaceControlEnabled BIT
)
AS

IF @FaceControlEnabled IS NULL
	BEGIN
		SELECT
			p_id		as Id,
			u_username	as Username,
			pa_id as PhotoAlbumID,
			p_name		as Name,
			p_description	as Description,
			p_approved	as Approved,
			p_primary	as [Primary],
			p_explicit	as [Explicit],
			p_private	as Private,
			p_facecrop	as FaceCrop,
			p_approvedtimestamp as ApprovedDate,
			p_manual_approval AS ManualApproval
		FROM
			Photos (NOLOCK)
		WHERE
			(@Id = -1 OR @Id IS NULL OR p_id = @Id)
			AND (@Username IS NULL OR u_username = @Username)
			AND (@PhotoAlbumID = -1 OR pa_id = @PhotoAlbumID OR @PhotoAlbumID IS NULL AND pa_id IS NULL) -- when @PhotoAlbumID is null it returns photo from default album
			AND (@Approved IS NULL OR p_approved = @Approved)
			AND (@ManualApproved IS NULL OR p_manual_approval = @ManualApproved)
	END

ELSE
	BEGIN
		SELECT
			p_id		as Id,
			p.u_username	as Username,
			pa_id as PhotoAlbumID,
			p_name		as Name,
			p_description	as Description,
			p_approved	as Approved,
			p_primary	as [Primary],
			p_explicit	as [Explicit],
			p_private	as Private,
			p_facecrop	as FaceCrop,
			p_approvedtimestamp as ApprovedDate,
			p_manual_approval AS ManualApproval
		FROM
			Photos (NOLOCK) AS p, Users (NOLOCK) AS u
		WHERE
			(@Id = -1 OR @Id IS NULL OR p_id = @Id)
			AND (@Username IS NULL OR p.u_username = @Username)
			AND (@Approved IS NULL OR p.p_approved = @Approved)
			AND (@ManualApproved IS NULL OR p.p_manual_approval = @ManualApproved)
			AND  p.u_username = u.u_username AND u.u_face_control_approved = @FaceControlEnabled
	END




/****** Object:  StoredProcedure [dbo].[FetchPhotoRating]    Script Date: 03/02/2012 21:54:53 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPhotoRating]
(
	@ID	int
)
AS
SELECT
	COUNT(pr_rating) as Votes,
	AVG(CAST(pr_rating as decimal)) as AverageVote
FROM
	PhotoRatings (NOLOCK)
WHERE
	p_id = @ID




/****** Object:  StoredProcedure [dbo].[FetchPhotoVote]    Script Date: 03/02/2012 21:55:15 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPhotoVote]
(
	@FromUser	VARCHAR(20),
	@PhotoID	INT
)
AS
	SELECT 
		pr_rating as Vote
	FROM
		PhotoRatings (NOLOCK)
	WHERE
		pr_fromusername = @FromUser
		AND p_id = @PhotoID



/****** Object:  StoredProcedure [dbo].[FetchPrimaryPhotoDetails]    Script Date: 03/02/2012 21:55:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchPrimaryPhotoDetails]
(
	@Username	VARCHAR(30)
)
AS
	SELECT TOP 1
		p_id		as ID,
		pa_id		as PhotoAlbumID,
		p_name		as Name,
		p_description	as Description,
		p_approved	as Approved,
		p_explicit	as Explicit,
		p_facecrop	as FaceCrop
	FROM
		Photos (NOLOCK)
	WHERE
		u_username = @Username AND
		p_approved = 1 AND 
		p_private = 0
	ORDER BY
		p_primary DESC, p_explicit ASC



/****** Object:  StoredProcedure [dbo].[FetchProfileAnswer]    Script Date: 03/02/2012 21:56:03 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchProfileAnswer]
(
	@Username	varchar(20),
	@QuestionID	int
)
AS
	SELECT
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers (NOLOCK)
	WHERE
		u_username = @Username
		AND pq_id = @QuestionID



/****** Object:  StoredProcedure [dbo].[FetchProfileAnswerByQuestion]    Script Date: 03/02/2012 21:56:25 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchProfileAnswerByQuestion]
(
	@QuestionID	int
)
AS
	SELECT
		u_username	as Username,
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers (NOLOCK)
	WHERE
		pq_id = @QuestionID



/****** Object:  StoredProcedure [dbo].[FetchProfileAnswerByUser]    Script Date: 03/02/2012 21:56:45 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchProfileAnswerByUser]
(
	@Username	VARCHAR(20)
)
AS
	SELECT
		pq_id		as QuestionID,
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers (NOLOCK)
	WHERE
		u_username = @Username




/****** Object:  StoredProcedure [dbo].[FetchProfileChoice]    Script Date: 03/02/2012 21:57:05 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchProfileChoice]
(
	@ID	int = null
)
AS
	IF (@ID IS NULL)
		SELECT
			pc_id		as ID,
			pq_id		as QuestionID,
			pc_value	as Value
		FROM
			ProfileChoices (NOLOCK)
	ELSE
		SELECT
			pq_id		as QuestionID,
			pc_value	as Value
		FROM
			ProfileChoices (NOLOCK)
		WHERE
			pc_id = @ID




/****** Object:  StoredProcedure [dbo].[FetchProfileChoiceByQuestion]    Script Date: 03/02/2012 21:57:32 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchProfileChoiceByQuestion]
(
	@QuestionID	int = null
)
AS
	SELECT
		pc_id		as ID,
		pc_value	as Value
	FROM
		ProfileChoices (NOLOCK)
	WHERE
		pq_id = @QuestionID



/****** Object:  StoredProcedure [dbo].[FetchProfileQuestion]    Script Date: 03/02/2012 21:58:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchProfileQuestion]
(
	@ID	int = null
)
AS
	IF (@ID IS NULL)
		SELECT
			pq_id		as ID,
			pt_id		as TopicID,
			pq_name	as Name,
			pq_altname	as AltName,
			pq_description	as Description,
			pq_hint		as Hint,
			pq_edit_style	as EditStyle,
			pq_show_style	as ShowStyle,
			pq_search_style	as SearchStyle,
			pq_required	as Required,
			pq_priority	as Priority,
			pq_requires_approval as RequiresApproval,
			pq_visible_male		as VisibleForMale,
			pq_visible_female	as VisibleForFemale,
			pq_visible_couple	as VisibleForCouple,
			pq_match_field	as MatchField,
			pq_viewpaidonly as ViewPaidOnly,
			pq_editpaidonly as EditPaidOnly,
			pq_parent_question_id as ParentQuestionID,
			pq_parent_question_choices as ParentQuestionChoices
		FROM
			ProfileQuestions
		ORDER BY
			pq_priority ASC
	ELSE
		SELECT
			pt_id		as TopicID,
			pq_name	as Name,
			pq_altname	as AltName,
			pq_description	as Description,
			pq_hint		as Hint,
			pq_edit_style	as EditStyle,
			pq_show_style	as ShowStyle,
			pq_search_style	as SearchStyle,
			pq_required	as Required,
			pq_priority	as Priority,
			pq_requires_approval as RequiresApproval,
			pq_visible_male		as VisibleForMale,
			pq_visible_female	as VisibleForFemale,
			pq_visible_couple	as VisibleForCouple,
			pq_match_field	as MatchField,
			pq_viewpaidonly as ViewPaidOnly,
			pq_editpaidonly as EditPaidOnly,
			pq_parent_question_id as ParentQuestionID,
			pq_parent_question_choices as ParentQuestionChoices
		FROM
			ProfileQuestions (NOLOCK)
		WHERE
			pq_id = @ID

			
			
/****** Object:  StoredProcedure [dbo].[FetchProfileQuestionByTopic]    Script Date: 03/02/2012 21:58:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchProfileQuestionByTopic]
(
	@TopicID	int
)
AS
	SELECT
		pq_id		as ID,
		pq_name	as Name,
		pq_altname	as AltName,
		pq_description	as Description,
		pq_hint		as Hint,
		pq_edit_style	as EditStyle,
		pq_show_style	as ShowStyle,
		pq_search_style	as SearchStyle,
		pq_required	as Required,
		pq_priority	as Priority,
		pq_requires_approval as RequiresApproval,
		pq_visible_male		as VisibleForMale,
		pq_visible_female	as VisibleForFemale,
		pq_visible_couple	as VisibleForCouple,
		pq_match_field as MatchField,
		pq_viewpaidonly as ViewPaidOnly,
		pq_editpaidonly as EditPaidOnly,
		pq_parent_question_id as ParentQuestionID,
		pq_parent_question_choices as ParentQuestionChoices
	FROM
		ProfileQuestions (NOLOCK)
	WHERE
		pt_id = @TopicID
	ORDER BY
		pq_priority ASC



/****** Object:  StoredProcedure [dbo].[FetchProfileTopic]    Script Date: 03/02/2012 21:59:02 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchProfileTopic]
(
	@ID	int = null
)
AS
	IF (@ID IS NULL)
		SELECT
			pt_id		as ID,
			pt_name	as Name,
			pt_priority	as Priority,
			pt_editcolumns	as EditColumns,
			pt_viewcolumns	as ViewColumns
		FROM
			ProfileTopics (NOLOCK)
		ORDER BY
			pt_priority ASC
	ELSE
		SELECT
			pt_name	as Name,
			pt_priority	as Priority,
			pt_editcolumns	as EditColumns,
			pt_viewcolumns	as ViewColumns
		FROM
			ProfileTopics (NOLOCK)
		WHERE
			pt_id = @ID



/****** Object:  StoredProcedure [dbo].[FetchProfileView]    Script Date: 03/02/2012 22:00:14 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchProfileView]
(
	@ViewerUsername	VARCHAR(20),
	@ViewedUsername	VARCHAR(20)
)
AS

SELECT TOP 1
	pv_timestamp as [Timestamp]
FROM 
	ProfileViews (NOLOCK)
WHERE
	pv_viewed = @ViewedUsername
	AND pv_viewer = @ViewerUsername
ORDER BY
	pv_timestamp DESC


/****** Object:  StoredProcedure [dbo].[FetchProfileViewDate]    Script Date: 03/02/2012 22:00:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchProfileViewDate]
(
	@ViewerUsername	VARCHAR(20),
	@ViewedUsername	VARCHAR(20)
)
AS

SELECT TOP 1
	pv_timestamp as [Timestamp]
FROM 
	ProfileViews (NOLOCK)
WHERE
	pv_viewed = @ViewedUsername
	AND pv_viewer = @ViewerUsername
ORDER BY
	pv_timestamp DESC



/****** Object:  StoredProcedure [dbo].[FetchProfileViews]    Script Date: 03/02/2012 22:01:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchProfileViews]
(
	@ViewedUsername	VARCHAR(20),
	@FromDate		DATETIME = NULL
)
AS

SELECT TOP 100
	pv_viewer as ViewerUsername
FROM 
	ProfileViews (NOLOCK)
WHERE
	pv_viewed = @ViewedUsername
	AND (@FromDate IS NULL OR pv_timestamp >= @FromDate)
ORDER BY
	pv_timestamp DESC


/****** Object:  StoredProcedure [dbo].[FetchRating]    Script Date: 03/02/2012 22:01:25 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchRating]
(
	@Username	VARCHAR(20)
)
AS

/*
SELECT
	COUNT(r_rating) as Votes,
	AVG(CAST(r_rating as decimal)) as AverageVote
FROM
	Ratings
WHERE
	r_tousername = @Username
*/

DECLARE @AverageIPVote FLOAT
DECLARE @IPVoteCount INT
DECLARE @IPVoteUniqueCount INT
DECLARE @AverageNoIPVote FLOAT
DECLARE @NoIPVoteCount INT

SELECT 
	@IPVoteUniqueCount = COUNT(AvgVote),
	@IPVoteCount = SUM(VotesCount),
	@AverageIPVote = AVG(AvgVote)
FROM
(
	SELECT
		COUNT(r_rating) as VotesCount,
		AVG(CAST(r_rating as decimal)) as AvgVote
	FROM
		Ratings (NOLOCK)
	WHERE
		r_tousername = @Username
		and r_ip != '0.0.0.0'
	GROUP BY r_ip
) as AvgVotes

SELECT
	@NoIPVoteCount = COUNT(r_rating),
	@AverageNoIPVote = AVG(CAST(r_rating as decimal))
FROM
	Ratings (NOLOCK)
WHERE
	r_tousername = @Username
	and r_ip = '0.0.0.0'

IF (@AverageIPVote IS NULL) SET @AverageIPVote = 0
IF (@AverageNoIPVote IS NULL) SET @AverageNoIPVote = 0
IF (@IPVoteCount IS NULL) SET @IPVoteCount = 0
IF (@NoIPVoteCount IS NULL) SET @NoIPVoteCount = 0
IF (@IPVoteUniqueCount IS NULL) SET @IPVoteUniqueCount = 0

IF (@IPVoteUniqueCount + @NoIPVoteCount = 0) 
	SELECT NULL as Votes, NULL as AverageVote
ELSE
SELECT @IPVoteCount + @NoIPVoteCount as Votes, 
	(@AverageIPVote * @IPVoteUniqueCount + @AverageNoIPVote * @NoIPVoteCount) 
		/ (@IPVoteUniqueCount + @NoIPVoteCount) as AverageVote



/****** Object:  StoredProcedure [dbo].[FetchScheduledAnnouncement]    Script Date: 03/02/2012 22:02:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchScheduledAnnouncement]
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

FROM	ScheduledAnnouncements (NOLOCK)

WHERE	(@ID IS NULL OR sa_id = @ID)
		AND (@Gender IS NULL OR sa_gender = @Gender)
		AND (@PaidMember IS NULL OR sa_paidmember = @PaidMember)
		AND (@HasPhotos IS NULL OR sa_hasphotos = @HasPhotos)
		AND (@HasProfile IS NULL OR sa_hasprofile = @HasProfile)
		AND (@LanguageID IS NULL OR sa_languageid = @LanguageID)
		AND (@Country IS NULL OR sa_country = @Country)
		AND (@Region IS NULL OR sa_region = @Region)
		AND (@Type IS NULL OR sa_type = @Type)




/****** Object:  StoredProcedure [dbo].[FetchSubscription]    Script Date: 03/02/2012 22:03:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
		Subscriptions (NOLOCK)
	WHERE
		(@ID IS NULL OR s_id = @ID) AND
		(@Custom IS NULL OR s_custom = @Custom)


/****** Object:  StoredProcedure [dbo].[FetchTopEntries]    Script Date: 03/02/2012 22:03:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchTopEntries]
	@ContestId		INT,
	@FavCount		INT = 10
AS
BEGIN
	SET NOCOUNT ON;

	
/*
	SELECT
		pce_id	as Id,
		SUM(@FavCount - pcr_value + 1) as Score
	FROM
		PhotoContestRanks as R1
	WHERE
		pc_id = @ContestId
		AND (SELECT COUNT(*) FROM PhotoContestRanks as R2 
			WHERE R1.u_username = R2.u_username) >= @FavCount
	GROUP BY
		pce_id
	ORDER BY
		Score DESC
*/

	SELECT
		R1.pce_id	as Id,
		SUM(CAST((@FavCount * 1.5) as INT) - pcr_value + 1 * (ABS(U1.u_gender - U2.u_gender) + 1)) as Score
	FROM
		PhotoContestRanks (NOLOCK) as R1,
		PhotoContestEntries (NOLOCK) as E1,
		Users (NOLOCK) as U1,
		Users (NOLOCK) as U2
	WHERE
		R1.pc_id = @ContestId AND R1.pcr_value <= @FavCount
		AND (SELECT COUNT(*) FROM PhotoContestRanks (NOLOCK) as R2 
			WHERE R1.u_username = R2.u_username) >= @FavCount
		AND R1.u_username = U1.u_username
		AND E1.pce_id = R1.pce_id
		AND E1.u_username = U2.u_username
	GROUP BY
		R1.pce_id
	ORDER BY
		Score DESC
		
END


/****** Object:  StoredProcedure [dbo].[FetchSetting]    Script Date: 03/02/2012 22:03:32 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchSetting]
(
	@Key	varchar(80)
)
 AS
	SELECT s_value FROM Settings (NOLOCK) WHERE @Key = s_key



/****** Object:  StoredProcedure [dbo].[FetchUserVote]    Script Date: 03/02/2012 22:08:31 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchUserVote]
(
	@FromUsername	VARCHAR(20),
	@ToUsername		VARCHAR(20)
)
AS

SELECT 
	v_score as Score
FROM 
	Votes (NOLOCK)
WHERE
	v_fromusername = @FromUsername
	AND v_tousername = @ToUsername

/****** Object:  StoredProcedure [dbo].[FetchUserVerificationsCount]    Script Date: 03/02/2012 22:08:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchUserVerificationsCount]
(
	@VerifiedBy	VARCHAR(20),
	@VerifiedUser	VARCHAR(20)
)
AS
SELECT
	COUNT(vu_verifieduser)
FROM
	VerifiedUsers (NOLOCK)
WHERE
	vu_verifieduser = @VerifiedUser AND
	(@VerifiedBy IS NULL OR vu_verifiedby = @VerifiedBy)

/****** Object:  StoredProcedure [dbo].[FetchUsersInRadius]    Script Date: 03/02/2012 22:07:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchUsersInRadius]
(
--User related data
	@HasAnswer BIT,
	@Gender	INT,
	@FromBirthdate	DATETIME,
	@ToBirthdate	DATETIME,
	@PhotoReq	BIT,
--Radius related data
	@SouthLat	float,
	@NorthLat	float,
	@WestLong	float,
	@EastLong	float,
	@Longitude	float,
	@Latitude	float,
	@Radius		float,
--Max user count
	@MaxResults	int
)
 AS
	SET ROWCOUNT @MaxResults

	SELECT 
		u_username as Username
	FROM Users (NOLOCK)
	WHERE
		CASE 
			WHEN u_latitude IS NULL 
				THEN 999.0
				ELSE u_latitude
		END  >= @SouthLat  AND 
		CASE 
			WHEN u_latitude IS NULL 
				THEN 999.0
				ELSE u_latitude
		END  <= @NorthLat  AND 
		CASE 
			WHEN u_longitude IS NULL 
				THEN 999.0
				ELSE u_longitude
		END  >= @WestLong  AND 
		CASE 
			WHEN u_longitude IS NULL 
				THEN 999.0
				ELSE u_longitude
		END  <= @EastLong AND
		SQRT(SQUARE(69.1*(u_latitude - @Latitude)) + SQUARE(53*(u_longitude - @Longitude))) <= @Radius AND
		(@HasAnswer IS NULL OR EXISTS ( 
               SELECT pa_value FROM ProfileAnswers (NOLOCK) 
                    WHERE Users.u_username = ProfileAnswers.u_username)) AND
		 u_gender = @Gender AND
		 u_birthdate >= @FromBirthdate AND
		 u_birthdate <= @ToBirthdate AND
		 (@PhotoReq = 0 OR EXISTS (
			SELECT p_id FROM Photos (NOLOCK) 
			WHERE Users.u_username = Photos.u_username
			AND Photos.p_approved = 1)) AND			 
		 u_active = 1 AND
		 u_deleted=0 AND
		 u_profilevisible=1
		 AND u_face_control_approved = 1
	ORDER BY u_country, u_state, u_city

	SET ROWCOUNT 0

/****** Object:  StoredProcedure [dbo].[FetchUsernamesWithFavoriteUser]    Script Date: 03/02/2012 22:07:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchUsernamesWithFavoriteUser]
	(
		@Username VARCHAR(20)
	)
AS

SELECT u_username AS Username
FROM Favourites (NOLOCK)
WHERE f_username = @Username

/****** Object:  StoredProcedure [dbo].[FetchUserByGuid]    Script Date: 03/02/2012 22:07:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchUserByGuid]
	(
		@Guid VARCHAR(36)
	)
AS

SELECT pg_username AS Username
FROM PendingGuids (NOLOCK)
WHERE @Guid = pg_guid

/****** Object:  StoredProcedure [dbo].[FetchTranslationKeys]    Script Date: 03/02/2012 22:06:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchTranslationKeys] 
(
	@AdminPanel INT
	--@LanguageId	INT = 1
)
AS

SELECT 
	DISTINCT t_key as [Key]
FROM
	Translations (NOLOCK)
WHERE t_adminpanel = @AdminPanel

/****** Object:  StoredProcedure [dbo].[FetchTranslation]    Script Date: 03/02/2012 22:06:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchTranslation] 
(
	@LanguageId	INT = 1,
	@Key		NVARCHAR(400),
	@AdminPanel INT
)
AS

DECLARE @Value NVARCHAR(400)

(SELECT @Value = t_value
			FROM
				Translations (NOLOCK)
			WHERE
				l_id = @LanguageId
				AND t_key = @Key
				AND t_adminpanel = @AdminPanel)
				
IF (@Value IS NULL)
BEGIN
	SELECT @Value = t_value
			FROM
				Translations (NOLOCK)
			WHERE
				l_id = @LanguageId
				AND t_key = @Key
				AND t_adminpanel = 0
				
	IF (@Value IS NOT NULL)
		INSERT INTO [Translations] (
			[l_id],
			[t_key],
			[t_adminpanel],
			[t_value]
		) VALUES ( 
			@LanguageId,
			@Key,
			@AdminPanel,
			@Value ) 
END

SELECT @Value AS Value

--SELECT TOP 1 
--	t_value as Value
--FROM
--	Translations
--WHERE
--	l_id = @LanguageId
--	AND t_key = @Key
--	AND t_adminpanel = @AdminPanel

/****** Object:  StoredProcedure [dbo].[FetchTopPhotoDetails]    Script Date: 03/02/2012 22:05:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[FetchTopPhotoDetails]
(
	@Username	VARCHAR(20),
	@MinVotes	int
)
AS
	SELECT TOP 1
		p_id		as [ID],
		pa_id		as PhotoAlbumID,
		p_name		as [Name],
		p_description	as [Description],
		p_approved	as Approved,
		p_explicit	as [Explicit],
		p_primary	as [Primary],
		(
		               SELECT TOP 1 AVG( CAST(pr_rating AS DECIMAL))
		               FROM PhotoRatings (NOLOCK)
		               WHERE 
				(SELECT count(*) FROM PhotoRatings (NOLOCK) WHERE PhotoRatings.p_id = Photos.p_id AND
				Photos.u_username = @Username) >= @MinVotes AND
				PhotoRatings.p_id = Photos.p_id AND
				Photos.u_username = @Username
		) as AvgRating
	FROM
		Photos (NOLOCK)
	WHERE
		u_username = @Username AND
		p_approved = 1 AND 
		p_private = 0
	ORDER BY
		AvgRating DESC

		
		

/****** Object:  StoredProcedure [dbo].[FetchWinkCategories]    Script Date: 03/02/2012 22:10:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[FetchWinkCategories]
	@ID INT,
	@ParentID INT,
	@Title NVARCHAR(50),
	@SortColumn INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT	wc_id AS[ID],
		wc_language_id AS LanguageID,
		wc_name AS Title
FROM WinkCategories (NOLOCK)
WHERE	(@ID IS NULL OR wc_id = @ID)
		AND (@ParentID IS NULL OR wc_language_id = @ParentID)
		AND (@Title IS NULL OR wc_name = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN wc_name END

END

/****** Object:  StoredProcedure [dbo].[FetchWinkTypes]    Script Date: 03/02/2012 22:11:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[FetchWinkTypes]
	(
		@ID INT,
		@CategoryID INT,
		@Name NVARCHAR(200),
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	wt_id AS [ID],
		wt_category_id AS [CategoryID],
		wt_name AS [Name],
		wt_phrase AS [Phrase],
		wt_active AS Active
		
FROM	WinkTypes (NOLOCK)
WHERE	(@ID IS NULL OR wt_id = @ID)
		AND (@CategoryID IS NULL OR wt_category_id = @CategoryID)
		AND (@Name IS NULL OR wt_name = @Name)
		AND (@Active IS NULL OR wt_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN wt_name END




/****** Object:  StoredProcedure [dbo].[FetchWinkMessage]    Script Date: 03/02/2012 22:11:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[FetchWinkMessage]
(
	@MessageID	INT = 0	
)
AS
	SELECT TOP 1
		wm_id AS Id, wm_wink_id AS WinkId, wm_message_id AS MessageId
	
	FROM WinkMessaages (NOLOCK)
	WHERE (@MessageID = 0 OR @MessageID = wm_id)


/****** Object:  StoredProcedure [dbo].[FetchVotesScore]    Script Date: 03/02/2012 22:10:39 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[FetchVotesScore]
(
	@Username	VARCHAR(20)
)
AS

SELECT 
	SUM(v_score) as Score
FROM 
	Votes  (NOLOCK)
WHERE
	v_tousername = @Username

	
	
/****** Object:  StoredProcedure [dbo].[GetChatOnlineUsers]    Script Date: 03/02/2012 22:15:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetChatOnlineUsers]
	@ChatRoomId int,
	@OnlineTimeout int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT cra_userid as UserId,
		cra_roomid as RoomId,
		cra_username as Username,
		cra_displayname as DisplayName,
		cra_lastonline as LastOnline
	FROM ChatRoomActivity (NOLOCK)
	WHERE (@ChatRoomId IS NULL OR cra_roomid = @ChatRoomId)
		AND DATEADD(s, @OnlineTimeout, cra_lastonline) > getdate()

END

/****** Object:  StoredProcedure [dbo].[HasProfile]    Script Date: 03/02/2012 22:15:41 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[HasProfile]
(
	@Username varchar(20),
	@Approved bit
) AS

IF (@Approved = 1)
BEGIN
	 IF EXISTS(
		SELECT pa_value FROM ProfileAnswers (NOLOCK)
			WHERE @Username = ProfileAnswers.u_username AND pa_approved = 1) AND
	NOT EXISTS(
		SELECT pa_value FROM ProfileAnswers (NOLOCK)
			WHERE @Username = ProfileAnswers.u_username AND pa_approved = 0)
				
		SELECT CAST (1 AS BIT)
	ELSE
		SELECT CAST(0 AS BIT)
END
ELSE
BEGIN
	DECLARE @Gender INT
	SET @Gender = (SELECT u_gender FROM Users (NOLOCK) WHERE u_username=@Username)

	IF ( (SELECT COUNT(*) FROM ProfileQuestions (NOLOCK) WHERE pq_required=1 AND 
				CASE @Gender
					WHEN 1 THEN pq_visible_male
					WHEN 2 THEN pq_visible_female	
					WHEN 3 THEN pq_visible_couple 
				END = 1) > 0 )
	BEGIN
		IF EXISTS(SELECT pq_id from ProfileQuestions (NOLOCK) WHERE pq_required=1 AND 
				CASE @Gender
					WHEN 1 THEN pq_visible_male
					WHEN 2 THEN pq_visible_female	
					WHEN 3 THEN pq_visible_couple 
				END = 1
				AND pq_id NOT IN 
				(SELECT pq_id FROM ProfileAnswers (NOLOCK) WHERE 
				u_username = @Username)
			)
			SELECT CAST(0 AS BIT)
		ELSE
			SELECT CAST (1 AS BIT)
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT pa_value FROM ProfileAnswers (NOLOCK) WHERE u_username=@Username)
			SELECT CAST (1 AS BIT)
		ELSE
			SELECT CAST (0 AS BIT)
	END	
END

/****** Object:  StoredProcedure [dbo].[GetRandomPhotoId]    Script Date: 03/02/2012 22:15:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetRandomPhotoId]
	(
		@Username VARCHAR(20),
		@Gender INT,
		@FromBirthdate	DATETIME,
		@ToBirthdate	DATETIME,
		@MaxTimeAway	INT
	)
AS

SELECT TOP 1 Photos.p_id
FROM Photos (NOLOCK) INNER JOIN Users (NOLOCK) ON Photos.u_username = Users.u_username
WHERE	p_approved = 1
		AND p_explicit = 0
		AND p_private = 0
		AND (@Username IS NULL OR Photos.u_username != @Username)
		AND (@Gender IS NULL OR u_gender = @Gender)
		AND (@FromBirthdate IS NULL OR (
				u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		  AND (@ToBirthdate IS NULL OR (
				u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
		AND NOT EXISTS (SELECT 1 FROM PhotoRatings (NOLOCK) WHERE pr_fromusername = @Username AND Photos.p_id = PhotoRatings.p_id)
		AND (u_options & 8 != 8)
		-- VisitorsCanViewPhotos is enabled OR UsersCanViewPhotos is enabled
		AND (u_options & 4096 = 4096 OR u_options & 8192 = 8192)
		AND (@MaxTimeAway = 0 OR DATEDIFF(day,u_lastlogin,getdate()) < @MaxTimeAway)
ORDER BY NEWID()

/****** Object:  StoredProcedure [dbo].[GetOnlineUsers]    Script Date: 03/02/2012 22:15:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetOnlineUsers]
	@Username nvarchar(100) = NULL,
	@PresenceTimeout int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT cp_username as Username
	FROM ChatPresence (NOLOCK)
	WHERE (@Username IS NULL OR cp_username = @Username)
		AND DATEADD(s, @PresenceTimeout, cp_lastonline) > getdate()

END


/****** Object:  StoredProcedure [dbo].[IsPaidMember]    Script Date: 03/02/2012 22:20:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[IsPaidMember]
(
	@Username varchar(20)
) AS
 IF EXISTS(
	SELECT
		 u_username
	FROM
		Users (NOLOCK)
	WHERE
		@Username = u_username AND u_paid_member = 1
	)
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)


/****** Object:  StoredProcedure [dbo].[IrregularSearchUsers]    Script Date: 03/02/2012 22:20:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IrregularSearchUsers]
	(
		@Date	DATETIME
	)
AS

SELECT u_username AS Username
FROM Users (NOLOCK)
WHERE	CAST (FLOOR (CAST (u_lastlogin AS FLOAT)) AS DATETIME) = @Date
		AND u_deleted = 0

/****** Object:  StoredProcedure [dbo].[IsEmailUsed]    Script Date: 03/02/2012 22:20:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IsEmailUsed]
(
	@Email	VARCHAR(250),
	@Username VARCHAR(20)
)
AS
	IF (@Username IS NULL)
	BEGIN
		IF ( (SELECT count(u_email) FROM Users (NOLOCK) WHERE u_email = @Email) >0 )
			SELECT CAST (1 as BIT)
		ELSE
			SELECT CAST (0 as BIT)
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT u_username FROM Users (NOLOCK) WHERE u_email = @Email AND u_username <> @Username)
			SELECT CAST (1 as BIT)
		ELSE
			SELECT CAST (0 as BIT)
	END


/****** Object:  StoredProcedure [dbo].[IsTrialPeriodExpired]    Script Date: 03/02/2012 22:22:24 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[IsTrialPeriodExpired]
(
	@Username varchar(20),
	@TrialPeriod int
) AS
DECLARE @SubscriptionDate DateTime

SELECT @SubscriptionDate = u_usersince FROM Users (NOLOCK) WHERE u_username = @Username

IF (DATEADD(day,@TrialPeriod, @SubscriptionDate) < getdate())
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)


/****** Object:  StoredProcedure [dbo].[IsUserBlocked]    Script Date: 03/02/2012 22:22:50 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IsUserBlocked]
(
	@UserBlocker varchar(20),
	@BlockedUser varchar(20)
)
 AS
	IF ((SELECT COUNT(*)  FROM BlockedUsers (NOLOCK) WHERE @UserBlocker = bu_userblocker AND @BlockedUser = bu_blockeduser) <> 0)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)


/****** Object:  StoredProcedure [dbo].[IsUserInFavouriteList]    Script Date: 03/02/2012 22:23:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IsUserInFavouriteList]
(
	@Username varchar(20),
	@Favourite varchar(20)
)
 AS
	IF EXISTS (SELECT u_username  FROM Favourites (NOLOCK) WHERE @Username = u_username AND @Favourite = f_username)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)


/****** Object:  StoredProcedure [dbo].[IsUserInFriendList]    Script Date: 03/02/2012 22:23:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IsUserInFriendList]
	(
		@Username varchar(20),
		@Friend varchar(20)
	)
 AS
	IF EXISTS (SELECT u_username  FROM Friends (NOLOCK) WHERE @Username = u_username AND @Friend = f_username)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)


/****** Object:  StoredProcedure [dbo].[IsUsernameTaken]    Script Date: 03/02/2012 22:24:12 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[IsUsernameTaken]
(
	@Username	VARCHAR(20)
)
AS
	IF ((SELECT COUNT(*)  FROM Users (NOLOCK)
		 WHERE (@Username = u_username) OR 
		       (u_facebookid != null and @Username = CONVERT(varchar, u_facebookid))) 
		 > 0)
	
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)



/****** Object:  StoredProcedure [dbo].[IsUserVerified]    Script Date: 03/02/2012 22:24:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IsUserVerified]
(
	@VerifiedBy	VARCHAR(20),
	@VerifiedUser	VARCHAR(20),
	@MinUserVotes	INT,
	@ByAdmin	BIT
)
AS
IF (@ByAdmin = 1)
	BEGIN
		SELECT u_verifiedbyadmin FROM Users (NOLOCK) WHERE u_username = @VerifiedUser
	END
ELSE
	BEGIN
		IF @MinUserVotes <= (SELECT COUNT(vu_verifieduser)
					FROM VerifiedUsers (NOLOCK)
					WHERE vu_verifieduser = @VerifiedUser AND
					(@VerifiedBy IS NULL OR vu_verifiedby = @VerifiedBy))
			SELECT  CAST(1 AS BIT)
		ELSE
			SELECT CAST(0 AS BIT)
	END


/****** Object:  StoredProcedure [dbo].[IsValidPendingGuid]    Script Date: 03/02/2012 22:25:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[IsValidPendingGuid] 
(
	@Username varchar(20),
	@Guid varchar(36)
)
AS
 IF EXISTS(
	SELECT
		 pg_guid as Guid
	FROM
		PendingGuids (NOLOCK)
	WHERE
		@Username = pg_username AND
		@Guid = pg_guid
	)
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)



/****** Object:  StoredProcedure [dbo].[LoadAbuseReport]    Script Date: 03/02/2012 22:26:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadAbuseReport]
(
	@ID int
)
AS
SELECT
	ar_id as [ID],
	ar_reportedby as ReportedBy,
	ar_reporteduser as ReportedUser,
	ar_type as [Type],
	ar_report as Report,
	ar_targetid as TargetId,
	ar_reviewed as Reviewed,
	ar_datereported as DateReported
FROM
	AbuseReports (NOLOCK)
WHERE
	ar_id = @ID



	
/****** Object:  StoredProcedure [dbo].[MessagesExist]    Script Date: 03/02/2012 22:34:27 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[MessagesExist]
(
	@FromUsername varchar(20),
	@ToUsername varchar(20)
) AS
 IF EXISTS(
	SELECT
		1
	FROM
		MessagesHistoryCache (NOLOCK)
	WHERE
		@FromUsername = mhc_fromusername AND @ToUsername = mhc_tousername
	)
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)

/****** Object:  StoredProcedure [dbo].[LoadMessage]    Script Date: 03/02/2012 22:29:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadMessage]
(
	@Id			INT
)
AS
	SELECT
		m_id			AS Id,
		m_from_username	AS FromUsername,
		m_from_folder		AS FromFolder,
		m_to_username		AS ToUsername,
		m_to_folder		AS ToFolder,
		m_about_username		AS AboutUsername,
		m_body			AS Body,
		m_timestamp		AS Timestamp,
		m_replied_to		AS RepliedTo,
		m_is_read		AS IsRead,
		m_pending_approval	AS PendingApproval
	
	FROM Messages
	WHERE @Id = m_id

	
	
/****** Object:  StoredProcedure [dbo].[SendMessage]    Script Date: 01/09/2012 10:30:13 ******/
SET ANSI_NULLS OFF



/****** Object:  StoredProcedure [dbo].[LoadEmailQueue]    Script Date: 03/02/2012 22:29:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadEmailQueue]   
 (   
      @Id          int = -1,   
      @To          varchar(250) = null,   
      @MinTries     int = -1,   
      @MaxTries     int = -1,   
      @MinNextTryDate     datetime = null   
 )   
 AS   
    
 SELECT   
      eq_id          as [Id],   
      eq_from          as [From],   
      eq_to          as [To],  
      eq_fromname as FromName,
      eq_toname as ToName, 
      eq_cc          as CC,   
      eq_bcc          as BCC,   
      eq_subject     as Subject,   
      eq_body     as Body,   
      eq_tries          as Tries,   
      eq_last_try     as LastTry,   
      eq_last_error     as LastError,   
      eq_next_try     as NextTry   
 FROM   
      EmailQueue   (NOLOCK)
 WHERE   
      (@Id = -1 OR eq_id = @Id)   
      AND (@To IS NULL OR eq_to = @To)   
      AND (@MinTries = -1 OR eq_tries >= @MinTries)   
      AND (@MaxTries = -1 OR eq_tries <= @MaxTries)   
      AND (@MinNextTryDate IS NULL OR eq_next_try IS NULL OR eq_next_try <= @MinNextTryDate)


/****** Object:  StoredProcedure [dbo].[LoadContactUs]    Script Date: 03/02/2012 22:28:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[LoadContactUs]
(
	@ID int
)
AS
SELECT
	cu_id as [ID],
	cu_reportedby as ReportedBy,
	cu_reporteduser as ReportedUser,
	cu_type as [Type],
	cu_report as Report,
	cu_targetid as TargetId,
	cu_reviewed as Reviewed,
	cu_datereported as DateReported
FROM
	ContactUs (NOLOCK)
WHERE
	cu_id = @ID


/****** Object:  StoredProcedure [dbo].[LoadPhotoContest]    Script Date: 03/02/2012 22:30:17 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadPhotoContest]
	@Id	INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		pc_id			as Id,
		pc_name			as Name,
		pc_gender		as Gender,
		pc_description	as Description,
		pc_terms		as Terms,
		pc_minage		as MinAge,
		pc_maxage		as MaxAge,
		pc_datecreated	as DateCreated,
		pc_dateends		as DateEnds
	FROM
		PhotoContests (NOLOCK)
	WHERE
		@Id IS NULL OR pc_id = @Id
	ORDER BY pc_datecreated DESC
END


/****** Object:  StoredProcedure [dbo].[LoadPhotoContestEntry]    Script Date: 03/02/2012 22:30:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadPhotoContestEntry]
	@Id			INT = NULL,
	@ContestId	INT = NULL,
	@Username	VARCHAR(20) = NULL,
	@PhotoId	INT = NULL,
	@NotRankedBy	VARCHAR(20) = NULL,
	@CountLimit	INT = NULL,
	@Randomize	BIT = 0
AS
BEGIN
	IF (@CountLimit IS NOT NULL) SET ROWCOUNT @CountLimit

	SELECT
		pce_id		as Id,
		pc_id		as ContestId,
		u_username	as Username,
		p_id		as PhotoId
	FROM
		PhotoContestEntries (NOLOCK) as pce
	WHERE
		(@Id IS NULL OR pce_id = @Id)
		AND (@ContestId IS NULL OR pc_id = @ContestId)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@PhotoId IS NULL OR p_id = @PhotoId)
		AND (@NotRankedBy IS NULL 
			/*
			OR NOT EXISTS (SELECT u_username FROM PhotoContestVotes
				WHERE PhotoContestVotes.u_username = @NotRankedBy AND
					(pce_id_picked = pce_id OR pce_id_nonpicked = pce_id)))
			*/
			OR NOT EXISTS (SELECT pc_id FROM PhotoContestRanks (NOLOCK) as pcr WHERE 
				pcr.pce_id = pce.pce_id AND u_username = @NotRankedBy))
		AND (@NotRankedBy IS NULL OR u_username != @NotRankedBy)
	ORDER BY 
		CASE 
			WHEN @Randomize = 1 THEN NEWID()
		END

	IF (@CountLimit IS NOT NULL) SET ROWCOUNT 0

END


/****** Object:  StoredProcedure [dbo].[LoadPhotoContestRanks]    Script Date: 03/02/2012 22:31:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadPhotoContestRanks]
	@Username	VARCHAR(20) = NULL,
	@ContestId	INT = NULL,
	@EntryId	INT = NULL,
	@Value		INT = NULL
AS
BEGIN
	SELECT
		u_username	as Username,
		pc_id		as ContestId,
		pce_id		as EntryId,
		pcr_value	as Value
	FROM
		PhotoContestRanks (NOLOCK)
	WHERE
		(@Username IS NULL OR u_username = @Username)
		AND (@ContestId IS NULL OR pc_id = @ContestId)
		AND (@EntryId IS NULL OR pce_id = @EntryId)
		AND (@Value IS NULL OR pcr_value = @Value)
	ORDER BY
		pcr_value ASC
END


/****** Object:  StoredProcedure [dbo].[LoadSavedSearch]    Script Date: 03/02/2012 22:31:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadSavedSearch]
(
	@ID int,
	@Username varchar(20),
	@Name nvarchar(100),
	@EmailMatches bit
)
 AS
	
SELECT
	ss_id as [ID],
	u_username as Username,
	ss_name as [Name],
	ss_gender as Gender,
	ss_country as Country,
	ss_state as State,
	ss_zip as Zip,
	ss_city as City,
	ss_agefrom as AgeFrom,
	ss_ageto as AgeTo,
	ss_photoreq as PhotoRequired,
	ss_choiceids as ChoiceIDs,
	ss_emailmatches as EmailMatches,
	ss_nextemaildate as NextEmailDate,
	ss_emailfrequency as EmailFrequency

FROM
	SavedSearches (NOLOCK)

WHERE
	(@ID IS NULL OR @ID = ss_id)
	AND (@Username IS NULL OR @Username = u_username)
	AND (@Name IS NULL OR @Name = ss_name)
	AND (@EmailMatches IS NULL OR (ss_emailmatches = @EmailMatches AND ss_nextemaildate < GETDATE()))


/****** Object:  StoredProcedure [dbo].[LoadUser]    Script Date: 03/02/2012 22:32:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[LoadUser] 
(
	@Username	VARCHAR(20)
)
AS
	SELECT
		u_password as Password,
		u_username as Username,
		u_email as Email,
		u_name as Name,
		u_gender as Gender,
		u_interested_in as InterestedIn,
		u_birthdate as Birthdate,
		u_birthdate2 as Birthdate2,
		u_active as Active,
		u_smsconfirmed as SmsConfirmed,
		u_usersince as UserSince,
		u_prevlogin as PrevLogin,
		u_lastlogin as LastLogin,
		u_logincount as LoginCount,
		u_lastonline as LastOnline,
		u_profileviews as ProfileViews,
		u_profilevisible as ProfileVisible,
		u_receiveemails as ReceiveEmails,
		u_country as Country,
		u_state as State,
		u_zip_code as ZipCode,
		u_city as City,
		u_deleted as Deleted,
		u_paid_member as Paid,
		u_lastsessionid as LastSessionID,
		u_message_verifications_left as MessageVerificationsLeft,
		u_signup_ip as SignupIP,
		l_id as LanguageId,
		u_billingdetails as BillingDetails,
		u_invited_by as InvitedBy,
		u_incoming_messages_restrictions as IncomingMessagesRestrictions,
		u_deletereason as DeleteReason,
		a_id as AffiliateID,
		u_options as Options,
		u_longitude as Longitude,
		u_latitude	as Latitude,
		u_score as Score,
		u_tokenuniqueid as TokenUniqueId,
		[u_personalizationinfo] AS PersonalizationInfo,
		u_credits AS Credits,
		u_moderationscore AS ModerationScores,
		u_spamsuspected AS SpamSuspected,
		u_face_control_approved AS FaceControlApproved,
		u_profile_skin as ProfileSkin,
		u_status_text as StatusText,
		u_featuredmember as FeaturedMember,
		u_myspaceid AS MySpaceID,
		u_facebookid AS FacebookID,
		u_eventssettings AS EventsSettings

	FROM 
		Users (NOLOCK)
	WHERE
		u_username = @Username


/****** Object:  StoredProcedure [dbo].[LoadUserByEmail]    Script Date: 03/02/2012 22:32:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[LoadUserByEmail]
(
	@Email	varchar(250)
)
 AS
SELECT
		u_username as Username
	FROM 
		Users (NOLOCK)
	WHERE
		u_email = @Email AND u_deleted = 0
ORDER BY u_usersince DESC


/****** Object:  StoredProcedure [dbo].[LoadUserByTokenUniqueId]    Script Date: 03/02/2012 22:33:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[LoadUserByTokenUniqueId]
(
	@TokenUniqueId varchar(100)
)
 AS
SELECT TOP 1
		u_username as Username
	FROM 
		Users (NOLOCK)
	WHERE
		u_tokenuniqueid = @TokenUniqueId AND u_deleted = 0
	ORDER BY
		u_active DESC, u_smsconfirmed DESC


/****** Object:  StoredProcedure [dbo].[LoadUsers]    Script Date: 03/02/2012 22:33:34 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[LoadUsers] 
(
	@Usernames	NVARCHAR(max)
)
AS
	DECLARE @SQL nvarchar(max)
	SET @SQL = '
	SELECT
		u_password as Password,
		u_username as Username,
		u_email as Email,
		u_name as Name,
		u_gender as Gender,
		u_interested_in as InterestedIn,
		u_birthdate as Birthdate,
		u_birthdate2 as Birthdate2,
		u_active as Active,
		u_smsconfirmed as SmsConfirmed,
		u_usersince as UserSince,
		u_prevlogin as PrevLogin,
		u_lastlogin as LastLogin,
		u_logincount as LoginCount,
		u_lastonline as LastOnline,
		u_profileviews as ProfileViews,
		u_profilevisible as ProfileVisible,
		u_receiveemails as ReceiveEmails,
		u_country as Country,
		u_state as State,
		u_zip_code as ZipCode,
		u_city as City,
		u_deleted as Deleted,
		u_paid_member as Paid,
		u_lastsessionid as LastSessionID,
		u_message_verifications_left as MessageVerificationsLeft,
		u_signup_ip as SignupIP,
		l_id as LanguageId,
		u_billingdetails as BillingDetails,
		u_invited_by as InvitedBy,
		u_incoming_messages_restrictions as IncomingMessagesRestrictions,
		u_deletereason as DeleteReason,
		a_id as AffiliateID,
		u_options as Options,
		u_longitude as Longitude,
		u_latitude	as Latitude,
		u_score as Score,
		u_tokenuniqueid as TokenUniqueId,
		[u_personalizationinfo] AS PersonalizationInfo,
		u_credits AS Credits,
		u_moderationscore AS ModerationScores,
		u_spamsuspected AS SpamSuspected,
		u_face_control_approved AS FaceControlApproved,
		u_profile_skin as ProfileSkin,
		u_status_text as StatusText,
		u_featuredmember as FeaturedMember,
		u_myspaceid AS MySpaceID,
		u_facebookid AS FacebookID,
		u_eventssettings AS EventsSettings

	FROM 
		Users (NOLOCK)
	WHERE
		u_username in (' + @Usernames + ')'
	EXEC(@SQL)


	
	/****** Object:  StoredProcedure [dbo].[MutualFriendsSearch]    Script Date: 03/02/2012 22:35:28 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[MutualFriendsSearch]
	(
		@Viewer VARCHAR(20),
		@Viewed VARCHAR(20)
	)
AS

SELECT 
		f_username as Friend 
	FROM 
		Friends (NOLOCK) as F1

	WHERE u_username = @Viewer
		AND EXISTS (SELECT u_username FROM Users (NOLOCK) WHERE Users.u_username = F1.f_username
					AND Users.u_deleted = 0 
					AND Users.u_profilevisible = 1)
		AND EXISTS (SELECT f_username FROM Friends (NOLOCK) as F2 WHERE F2.f_username = @Viewer
					AND F2.u_username = F1.f_username) 
		AND f_username IN 				
							(SELECT 
								f_username as Friend 
							FROM 
								Friends (NOLOCK) as F1

							WHERE u_username = @Viewed
								AND EXISTS (SELECT u_username FROM Users (NOLOCK) WHERE Users.u_username = F1.f_username
											AND Users.u_deleted = 0 
											AND Users.u_profilevisible = 1)
								AND EXISTS (SELECT f_username FROM Friends (NOLOCK) as F2 WHERE F2.f_username = @Viewed
											AND F2.u_username = F1.f_username))



/****** Object:  StoredProcedure [dbo].[MutualVoteSearch]    Script Date: 03/02/2012 22:35:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[MutualVoteSearch]
(
	@Username		VARCHAR(20)
)
AS
	SELECT V1.v_tousername as Username
	FROM Votes (NOLOCK) as V1, Votes (NOLOCK) as V2
	WHERE V1.v_fromusername = @Username
		AND V1.v_score = 1
		AND V2.v_tousername = @Username
		AND V2.v_score = 1
		AND V1.v_tousername = V2.v_fromusername
	ORDER BY V1.v_timestamp DESC


/****** Object:  StoredProcedure [dbo].[MySearchUsers]    Script Date: 03/02/2012 22:37:27 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[MySearchUsers]
 AS
	SELECT
		u_username as Username
	FROM
		Users (NOLOCK)
	WHERE
		u_active = 1 AND u_deleted = 0 AND u_face_control_approved = 1


/****** Object:  StoredProcedure [dbo].[NewUsersSearch]    Script Date: 03/02/2012 22:37:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[NewUsersSearch] 
( 
     @Gender     INT = NULL, 
     @UsersSince     DATETIME = NULL, 
     @ProfileReq     BIT = 0, 
     @PhotoReq     BIT = 0,
     @UsersCount   INT = 0,
	 @FromBirthdate	datetime,
	 @ToBirthdate	datetime,
	 @FeaturedMember BIT = NULL
) 
AS 
     IF (@UsersCount > 0)
	SET ROWCOUNT  @UsersCount

     SELECT 
          u_username as Username 
     FROM 
          Users (NOLOCK)
     WHERE 
          ((@UsersSince IS NULL) OR (u_usersince >= @UsersSince)) 
          AND ((@Gender IS NULL) OR (u_gender = @Gender)) 
          AND ((@PhotoReq = 0) OR (EXISTS ( 
               SELECT TOP 1 p_id 
               FROM Photos  (NOLOCK)
               WHERE
		 Photos.u_username = Users.u_username AND
		 p_approved = 1 AND
		 p_private = 0
	))) 
          AND ((@ProfileReq = 0) OR (EXISTS ( 
               SELECT pa_value FROM ProfileAnswers  (NOLOCK)
                    WHERE Users.u_username = ProfileAnswers.u_username) 
               AND u_profilevisible = 1))  
          AND u_active = 1 
          AND u_face_control_approved = 1
          AND u_profilevisible = 1 
          AND u_deleted = 0 
		  AND (@FromBirthdate IS NULL OR (
				u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		  AND (@ToBirthdate IS NULL OR (
				u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
		 AND (@FeaturedMember IS NULL OR u_featuredmember = @FeaturedMember)
     ORDER BY 
          u_usersince DESC

     SET ROWCOUNT 0


/****** Object:  StoredProcedure [dbo].[OnlineSearchUsers]    Script Date: 03/02/2012 22:38:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[OnlineSearchUsers]
(
	@FromDatetime	datetime,
	@Gender			int,
	@FromBirthdate	datetime,
	@ToBirthdate	datetime
)
AS
	SELECT
		u_username as Username
	FROM
		Users (NOLOCK)
	WHERE
		u_lastonline >= @FromDatetime
		AND
		u_profilevisible = 1
		AND
		(@Gender IS NULL OR u_gender = @Gender)
		AND (@FromBirthdate IS NULL OR (
				u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		AND (@ToBirthdate IS NULL OR (
				u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
	ORDER BY
		u_lastonline DESC

		

		
/****** Object:  StoredProcedure [dbo].[SearchPhotoContestEntries]    Script Date: 03/02/2012 22:40:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchPhotoContestEntries]
	(
		@ContestID		INT,
		@Username		VARCHAR(20),
		@SortColumn		VARCHAR(50)
	)
AS

SELECT
		pce_id AS ID
		
FROM PhotoContestEntries (NOLOCK) pce
	JOIN PhotoContests (NOLOCK) pc ON pce.pc_id = pc.pc_id
WHERE
	(@ContestID IS NULL OR @ContestID = pce.pc_id) AND
	(@Username IS NULL OR @Username = u_username)
ORDER BY
	CASE @SortColumn
		WHEN 'Username' THEN u_username
		WHEN 'ContestName' THEN pc_name
	END


/****** Object:  StoredProcedure [dbo].[SearchPhotoContests]    Script Date: 03/02/2012 22:40:26 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchPhotoContests]
	(
		@Active			BIT,
		@SortColumn		VARCHAR(50)
	)
AS

SELECT
		pc_id AS ID
		
FROM PhotoContests (NOLOCK)

WHERE
	(@Active IS NULL OR 
	 (@Active = 1 AND (pc_dateends IS NULL OR pc_dateends > getdate())) OR
	 (@Active = 0 AND pc_dateends < getdate()))
ORDER BY
	CASE @SortColumn
		WHEN 'DateCreated' THEN pc_datecreated
		WHEN 'DateEnds' THEN pc_dateends
	END,
	CASE @SortColumn
		WHEN 'Name' THEN pc_name
	END,
	CASE @SortColumn
		WHEN 'Gender' THEN pc_gender
	END


/****** Object:  StoredProcedure [dbo].[SearchPhotos]    Script Date: 03/02/2012 22:41:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[SearchPhotos]
(
	@Id			INT = -1,
	@Username	VARCHAR(20) = NULL,
	@Approved	BIT = NULL,
	@Primary	BIT = NULL,
	@Explicit	BIT = NULL,
	@Private	BIT = NULL
)
AS
	SELECT TOP 1000
		p_id			AS Id
	
	FROM Photos (NOLOCK)
	WHERE 
		(@Id = -1 OR @Id IS NULL OR p_id = @Id)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Approved IS NULL OR p_approved = @Approved)
		AND (@Primary IS NULL OR p_primary = @Primary)
		AND (@Explicit IS NULL OR p_explicit = @Explicit)
		AND (@Private IS NULL OR p_private = @Private)
	ORDER BY 
		p_id DESC


/****** Object:  StoredProcedure [dbo].[TopPhotosSearch]    Script Date: 03/02/2012 22:42:06 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[TopPhotosSearch] 
( 
     @Gender     INT, 
     @MinVotes     INT, 
     @MaxTimeAway INT, 
     @MaxResults INT 
) 
AS 
     SET ROWCOUNT @MaxResults 
 
     SELECT 
          DISTINCT Users.u_username as Username ,
	(
               SELECT AVG( CAST(pr_rating AS DECIMAL) ) 
               FROM PhotoRatings  (NOLOCK)
               WHERE PhotoRatings.p_id = Photos.p_id  
	) as AvgRating
     FROM 
          Users (NOLOCK), Photos  (NOLOCK)
     WHERE 
          u_gender = @Gender 
          AND Users.u_username = Photos.u_username 
          AND DATEDIFF(day,u_lastlogin,getdate()) < @MaxTimeAway 
          AND u_profilevisible = 1 
          AND u_deleted = 0 
          AND p_approved = 1
          AND p_private = 0
          AND ( 
               SELECT COUNT(pr_rating)  
               FROM PhotoRatings   (NOLOCK)
               WHERE PhotoRatings.p_id = Photos.p_id) >= @MinVotes 
     ORDER BY 
        AvgRating DESC 
 
     SET ROWCOUNT 0


/****** Object:  StoredProcedure [dbo].[TopUsersSearch]    Script Date: 03/02/2012 22:42:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[TopUsersSearch]
(
	@Gender	INT,
	@MinVotes	INT,
	@MaxTimeAway INT,
	@MaxResults INT
)
AS
	SET ROWCOUNT @MaxResults

	SELECT
		u_username as Username
	FROM
		Users (NOLOCK)
	WHERE
		u_gender = @Gender
		AND (
			SELECT COUNT(r_rating) 
			FROM Ratings  (NOLOCK)
			WHERE Ratings.r_tousername = Users.u_username) >= @MinVotes
		AND EXISTS (
			SELECT TOP 1 p_id
			FROM Photos (NOLOCK)
			WHERE Photos.u_username = Users.u_username AND p_approved = 1 AND p_private = 0 )
		AND DATEDIFF(day,u_lastlogin,getdate()) < @MaxTimeAway
		AND u_profilevisible = 1
		AND u_deleted = 0
	ORDER BY
		(
			dbo.fnFetchAverageRating(Users.u_username)) DESC

	SET ROWCOUNT 0


/****** Object:  StoredProcedure [dbo].[UsernameSearchUsers]    Script Date: 03/02/2012 22:43:23 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[UsernameSearchUsers]
(
	@Username	    VARCHAR(22),
	@HasAnswer	    BIT
	--,@OnlyLoggedIn	BIT  
)
AS
	SELECT
		u_username as Username
	FROM
		Users (NOLOCK)
	WHERE
		(u_username like @Username OR u_name like @Username)
		AND (@HasAnswer IS NULL OR EXISTS ( 
               SELECT pa_value FROM ProfileAnswers  (NOLOCK)
                    WHERE Users.u_username = ProfileAnswers.u_username))
		AND u_profilevisible = 1
		AND u_active = 1
		AND u_deleted = 0
		AND u_face_control_approved = 1
		AND u_logincount > 0
	ORDER BY u_lastonline DESC

