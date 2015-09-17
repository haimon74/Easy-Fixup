SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[LoadUsers] 
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
		Users
	WHERE
		u_username in (' + @Usernames + ')'
	EXEC(@SQL)

GO


