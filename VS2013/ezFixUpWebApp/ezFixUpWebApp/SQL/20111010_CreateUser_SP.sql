USE [ezFixUp14thAugust2011]
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 10/10/2011 03:25:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
	@EventsSettings BIGINT
)
AS
	INSERT INTO Users
		(u_username, u_password, u_email, u_name, u_gender, u_interested_in, u_birthdate,u_birthdate2, u_active, u_receiveemails,
		u_country, u_state, u_zip_code, u_city, u_signup_ip, u_message_verifications_left, l_id, u_billingdetails,
		u_invited_by, u_incoming_messages_restrictions, a_id, u_options, u_longitude, u_latitude, u_tokenuniqueid,
		u_credits, u_moderationscore, u_spamsuspected, u_face_control_approved, u_profile_skin, u_status_text, u_featuredmember,
		 u_myspaceid, u_facebookid, u_eventssettings)
	VALUES
		(@Username, @Password, @Email, @Name, @Gender, @InterestedIn, @Birthdate, @Birthdate2, @Active, @ReceiveEmails,
		 @Country, @State, @ZipCode, @City, @UserIP, @MessageVerificationsLeft, @LanguageId, @BillingDetails, 
		 @InvitedBy, @IncomingMessagesRestrictions, @AffiliateID, @Options, @Longitude, @Latitude, @TokenUniqueId, 
		 @Credits, @ModerationScore, @SpamSuspected, @FaceControlApproved, @ProfileSkin, @StatusText, @FeaturedMember,
		 @MySpaceID, @FacebookID, @EventsSettings)
