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
ALTER TABLE dbo.Users ADD
	u_invitedby_count int 
GO
ALTER TABLE dbo.Users SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
select Has_Perms_By_Name(N'dbo.Users', 'Object', 'ALTER') as ALT_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'VIEW DEFINITION') as View_def_Per, Has_Perms_By_Name(N'dbo.Users', 'Object', 'CONTROL') as Contr_Per 

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
		u_eventssettings AS EventsSettings,
		u_invitedby_count AS InvitedByCount
	FROM 
		Users (NOLOCK)
	WHERE
		u_username = @Username


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
		u_eventssettings AS EventsSettings,
		u_invitedby_count AS InvitedByCount

	FROM 
		Users (NOLOCK)
	WHERE
		u_username in (' + @Usernames + ')'
	EXEC(@SQL)


	
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
	@Country	NVARCHAR(100),
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
	@EventsSettings BIGINT,
	@InvitedByCount INT
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
		u_eventssettings = @EventsSettings,
		u_invitedby_count = @InvitedByCount

	WHERE
		u_username = @Username

		
		
		
		
		
		
		
		
		
		
		
		
GO
ALTER PROCEDURE [dbo].[CreateUser]
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
	@ReceiveEmails BIT,
	@Country	NVARCHAR(100),
	@State		NVARCHAR(100),
	@ZipCode	VARCHAR(15),
	@City		NVARCHAR(100),
	@UserIP	VARCHAR(15),
	@MessageVerificationsLeft INT,
	@LanguageId	INT,
	@BillingDetails NTEXT,
	@InvitedBy	VARCHAR(20),
	@IncomingMessagesRestrictions	NTEXT,
	@AffiliateID	INT,
	@Options	BIGINT,
	@Longitude	float,
	@Latitude	float,
	@TokenUniqueId	VARCHAR(100) = NULL,
	@Credits INT,
	@ModerationScore INT,
	@SpamSuspected BIT,
	@FaceControlApproved BIT,
	@ProfileSkin NVARCHAR(100) = NULL,
	@StatusText NVARCHAR(50),
	@FeaturedMember BIT,
	@MySpaceID VARCHAR(50),
	@FacebookID BIGINT,
	@EventsSettings BIGINT,
	@InvitedByCount INT
)
AS
	INSERT INTO Users
		(u_username, u_password, u_email, u_name, u_gender, u_interested_in, u_birthdate,u_birthdate2, u_active, u_receiveemails,
		u_country, u_state, u_zip_code, u_city, u_signup_ip, u_message_verifications_left, l_id, u_billingdetails,
		u_invited_by, u_incoming_messages_restrictions, a_id, u_options, u_longitude, u_latitude, u_tokenuniqueid,
		u_credits, u_moderationscore, u_spamsuspected, u_face_control_approved, u_profile_skin, u_status_text, u_featuredmember,
		 u_myspaceid, u_facebookid, u_eventssettings, u_invitedby_count)
	VALUES
		(@Username, @Password, @Email, @Name, @Gender, @InterestedIn, @Birthdate, @Birthdate2, @Active, @ReceiveEmails,
		 @Country, @State, @ZipCode, @City, @UserIP, @MessageVerificationsLeft, @LanguageId, @BillingDetails, 
		 @InvitedBy, @IncomingMessagesRestrictions, @AffiliateID, @Options, @Longitude, @Latitude, @TokenUniqueId, 
		 @Credits, @ModerationScore, @SpamSuspected, @FaceControlApproved, @ProfileSkin, @StatusText, @FeaturedMember,
		 @MySpaceID, @FacebookID, @EventsSettings, @InvitedByCount)

		 
GO

CREATE PROCEDURE [dbo].[FetchActiveElapsedSubscriptions] 
(
	@FromRenewDate datetime,
	@ToRenewDate datetime
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
		s_confirmed = 1 AND
		s_cancelled = 0 AND
		s_renew_date >= @FromRenewDate AND
		s_renew_date <= @ToRenewDate

		
		
GO		
		