/****** Object:  Table [dbo].[CreditsPackages]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CreditsPackages](
	[cp_id] [int] IDENTITY(1,1) NOT NULL,
	[cp_name] [nvarchar](200) NOT NULL,
	[cp_quantity] [int] NOT NULL,
	[cp_price] [money] NOT NULL,
 CONSTRAINT [PK_CreditsPackages] PRIMARY KEY CLUSTERED 
(
	[cp_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ContentViews]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContentViews](
	[cv_key] [varchar](50) NOT NULL,
	[l_id] [int] NOT NULL,
	[cv_content] [ntext] NOT NULL,
 CONSTRAINT [PK_ContentViews] PRIMARY KEY CLUSTERED 
(
	[cv_key] ASC,
	[l_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailTemplates]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailTemplates](
	[et_name] [varchar](50) NOT NULL,
	[et_subject] [nvarchar](500) NOT NULL,
	[et_body] [ntext] NOT NULL,
 CONSTRAINT [PK_EmailTemplates] PRIMARY KEY CLUSTERED 
(
	[et_name] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailQueue]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailQueue](
	[eq_id] [int] IDENTITY(1,1) NOT NULL,
	[eq_from] [varchar](250) NOT NULL,
	[eq_to] [varchar](250) NOT NULL,
	[eq_fromname] [nvarchar](250) NULL,
	[eq_toname] [nvarchar](250) NULL,
	[eq_cc] [varchar](250) NULL,
	[eq_bcc] [varchar](250) NULL,
	[eq_subject] [ntext] NOT NULL,
	[eq_body] [ntext] NOT NULL,
	[eq_tries] [int] NOT NULL,
	[eq_last_try] [datetime] NULL,
	[eq_last_error] [text] NULL,
	[eq_next_try] [datetime] NULL,
 CONSTRAINT [PK_EmailQueue] PRIMARY KEY CLUSTERED 
(
	[eq_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EcardTypes]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EcardTypes](
	[ect_id] [int] IDENTITY(1,1) NOT NULL,
	[ect_name] [nvarchar](200) NOT NULL,
	[ect_content] [image] NOT NULL,
	[ect_type] [int] NOT NULL,
	[ect_active] [bit] NOT NULL,
 CONSTRAINT [PK_EcardTypes] PRIMARY KEY CLUSTERED 
(
	[ect_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Discounts]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Discounts](
	[d_id] [int] IDENTITY(1,1) NOT NULL,
	[d_amount] [money] NOT NULL,
	[d_type] [int] NOT NULL,
	[d_field] [int] NOT NULL,
	[d_match] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Discounts] PRIMARY KEY CLUSTERED 
(
	[d_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Admins]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Admins](
	[a_username] [varchar](20) NOT NULL,
	[a_password] [varchar](250) NOT NULL,
	[a_lastlogin] [datetime] NOT NULL,
	[a_privileges] [text] NOT NULL,
	[a_lastsessionid] [varchar](32) NOT NULL,
 CONSTRAINT [PK__Admins__08EA5793] PRIMARY KEY CLUSTERED 
(
	[a_username] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AffiliateBanners]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AffiliateBanners](
	[ab_id] [int] IDENTITY(1,1) NOT NULL,
	[ab_name] [nvarchar](200) NOT NULL,
	[ab_image] [image] NOT NULL,
	[ab_deleted] [bit] NOT NULL,
 CONSTRAINT [PK_AffiliateBanners] PRIMARY KEY CLUSTERED 
(
	[ab_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AdsCategories]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdsCategories](
	[ac_id] [int] IDENTITY(1,1) NOT NULL,
	[ac_parentid] [int] NULL,
	[ac_title] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_AdsCategories] PRIMARY KEY CLUSTERED 
(
	[ac_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Affiliates]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Affiliates](
	[a_id] [int] IDENTITY(1,1) NOT NULL,
	[a_username] [varchar](20) NOT NULL,
	[a_password] [varchar](250) NOT NULL,
	[a_name] [nvarchar](100) NOT NULL,
	[a_email] [varchar](250) NOT NULL,
	[a_siteurl] [varchar](100) NOT NULL,
	[a_payment_details] [ntext] NOT NULL,
	[a_deleted] [bit] NOT NULL,
	[a_active] [bit] NOT NULL,
	[a_percentage] [int] NULL,
	[a_fixed_amount] [money] NULL,
	[a_recurrent] [bit] NOT NULL,
	[a_balance] [money] NOT NULL,
	[a_request_payment] [bit] NOT NULL,
 CONSTRAINT [PK_Affiliates] PRIMARY KEY CLUSTERED 
(
	[a_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AbuseReports]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AbuseReports](
	[ar_id] [int] IDENTITY(1,1) NOT NULL,
	[ar_reportedby] [varchar](20) NOT NULL,
	[ar_type] [int] NOT NULL,
	[ar_report] [nvarchar](2000) NOT NULL,
	[ar_reporteduser] [varchar](20) NOT NULL,
	[ar_targetid] [int] NULL,
	[ar_reviewed] [bit] NOT NULL,
	[ar_datereported] [datetime] NOT NULL,
 CONSTRAINT [PK_AbuseReports] PRIMARY KEY CLUSTERED 
(
	[ar_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[archive_ProfileAnswers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[archive_ProfileAnswers](
	[u_username] [varchar](20) NOT NULL,
	[pq_id] [int] NOT NULL,
	[pa_value] [nvarchar](4000) NOT NULL,
	[pa_approved] [bit] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[archive_Photos]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[archive_Photos](
	[p_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[p_image] [image] NULL,
	[p_name] [nvarchar](50) NOT NULL,
	[p_description] [nvarchar](200) NOT NULL,
	[p_approved] [bit] NOT NULL,
	[p_primary] [bit] NOT NULL,
	[p_explicit] [bit] NOT NULL,
	[p_private] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[archive_Messages]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[archive_Messages](
	[m_id] [int] NOT NULL,
	[m_from_username] [varchar](20) NOT NULL,
	[m_from_folder] [int] NOT NULL,
	[m_to_username] [varchar](20) NOT NULL,
	[m_to_folder] [int] NOT NULL,
	[m_body] [nvarchar](3500) NOT NULL,
	[m_timestamp] [datetime] NOT NULL,
	[m_replied_to] [int] NOT NULL,
	[m_is_read] [bit] NOT NULL,
	[m_pending_approval] [bit] NOT NULL,
	[m_new] [bit] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BannerCodes]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BannerCodes](
	[bc_id] [int] IDENTITY(1,1) NOT NULL,
	[bc_position] [int] NOT NULL,
	[bc_priority] [int] NOT NULL,
	[bc_target] [ntext] NULL,
	[bc_code] [ntext] NOT NULL,
 CONSTRAINT [PK_BannerCodes] PRIMARY KEY CLUSTERED 
(
	[bc_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BillingPlans]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillingPlans](
	[p_id] [int] IDENTITY(1,1) NOT NULL,
	[p_title] [nvarchar](200) NOT NULL,
	[p_amount] [money] NOT NULL,
	[p_billing_cycle] [int] NOT NULL,
	[p_billing_cycle_unit] [tinyint] NOT NULL,
	[p_deleted] [bit] NOT NULL,
	[p_options] [text] NULL,
 CONSTRAINT [PK_BillingPlans] PRIMARY KEY CLUSTERED 
(
	[p_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[c_id] [int] IDENTITY(1,1) NOT NULL,
	[c_name] [nvarchar](200) NOT NULL,
	[c_order] [int] NOT NULL,
	[c_userscancreategroups] [bit] NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[c_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatUsers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChatUsers](
	[cu_id] [int] IDENTITY(1,1) NOT NULL,
	[cu_username] [varchar](100) NOT NULL,
	[cu_displayname] [nvarchar](200) NULL,
	[cu_begintime] [datetime] NOT NULL,
	[cu_activetime] [datetime] NOT NULL,
	[cu_ip] [varchar](15) NULL,
 CONSTRAINT [PK_ChatUsers] PRIMARY KEY CLUSTERED 
(
	[cu_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ChatMessages]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChatMessages](
	[cm_id] [int] IDENTITY(1,1) NOT NULL,
	[cm_time] [datetime] NOT NULL,
	[cr_id] [int] NULL,
	[cu_id_sender] [int] NULL,
	[cm_senderdisplayname] [nvarchar](100) NOT NULL,
	[cu_id_target] [int] NULL,
	[cm_text] [nvarchar](2000) NOT NULL,
	[cm_texthtml] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_ChatMessages] PRIMARY KEY CLUSTERED 
(
	[cm_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ChatBans]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ChatBans](
	[cb_id] [int] IDENTITY(1,1) NOT NULL,
	[cr_id] [int] NULL,
	[cu_id] [int] NULL,
	[cb_ip] [varchar](15) NULL,
	[cb_date] [datetime] NOT NULL,
	[cb_dateexpires] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Groups]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Groups](
	[g_id] [int] IDENTITY(1,1) NOT NULL,
	[g_name] [nvarchar](250) NOT NULL,
	[g_description] [nvarchar](4000) NOT NULL,
	[g_icon] [image] NULL,
	[g_datecreated] [datetime] NOT NULL,
	[g_approved] [bit] NOT NULL,
	[g_accesslevel] [int] NOT NULL,
	[g_owner] [varchar](20) NOT NULL,
	[g_activemembers] [int] NOT NULL,
	[g_jointerms] [ntext] NOT NULL,
	[g_joinquestion] [ntext] NOT NULL,
	[g_permissions] [bigint] NOT NULL,
	[g_minage] [int] NULL,
	[g_autojoin] [bit] NOT NULL,
	[g_autojoincountry] [varchar](100) NULL,
	[g_autojoinregion] [nvarchar](100) NULL,
	[g_autojoincity] [nvarchar](100) NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[g_id] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_Groups] UNIQUE NONCLUSTERED 
(
	[g_name] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HourlyStats]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HourlyStats](
	[hs_time] [datetime] NOT NULL,
	[hs_usersonline] [int] NOT NULL,
	[hs_newusers] [int] NOT NULL,
	[hs_sentmessages] [int] NOT NULL,
 CONSTRAINT [PK_HourlyStats] PRIMARY KEY CLUSTERED 
(
	[hs_time] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[fnSplitter]    Script Date: 05/14/2010 18:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnSplitter]
  (@List varchar(4000), 
   @Del varchar(10) = ',')
RETURNS @ListTable TABLE 
  (ListID int IDENTITY , 
   Item varchar(200))
AS
BEGIN
  DECLARE @Pos int
  DECLARE @Item varchar(200)
  SET @Pos = CHARINDEX(@Del, @List)
  WHILE @Pos > 0
  BEGIN
    SET @Item = LTRIM(SUBSTRING(@List, 1, @Pos-1))
    INSERT @ListTable (Item)
    VALUES (@Item)
    SET @List = STUFF(@List, 1, @Pos+LEN(@Del)-1, '')
    SET @Pos = CHARINDEX(@Del, @List)    
  END
  
  INSERT @ListTable (Item)
  VALUES (LTRIM(@List))
  RETURN 
END
GO
/****** Object:  Table [dbo].[IPLogs]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IPLogs](
	[ipl_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ipl_ip] [varchar](15) NOT NULL,
	[ipl_action] [int] NOT NULL,
	[ipl_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_IPLogs] PRIMARY KEY CLUSTERED 
(
	[ipl_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PendingGuids]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PendingGuids](
	[pg_username] [varchar](20) NOT NULL,
	[pg_guid] [varchar](36) NOT NULL,
 CONSTRAINT [PK_PendingGuids] PRIMARY KEY CLUSTERED 
(
	[pg_guid] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MessagesHistoryCache]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MessagesHistoryCache](
	[mhc_fromusername] [varchar](20) NOT NULL,
	[mhc_tousername] [varchar](20) NOT NULL,
 CONSTRAINT [PK_MessagesHistoryCache] PRIMARY KEY CLUSTERED 
(
	[mhc_fromusername] ASC,
	[mhc_tousername] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Languages]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Languages](
	[l_id] [int] IDENTITY(1,1) NOT NULL,
	[l_language] [nvarchar](50) NOT NULL,
	[l_active] [bit] NOT NULL,
	[l_predefined] [bit] NOT NULL,
	[l_order] [int] NOT NULL,
	[l_browser_languages] [varchar](100) NULL,
	[l_ipcountries] [varchar](200) NULL,
	[l_theme] [varchar](100) NULL,
 CONSTRAINT [PK_Languages] PRIMARY KEY CLUSTERED 
(
	[l_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PhotoContests]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PhotoContests](
	[pc_id] [int] IDENTITY(1,1) NOT NULL,
	[pc_name] [nvarchar](500) NOT NULL,
	[pc_gender] [int] NULL,
	[pc_description] [ntext] NOT NULL,
	[pc_terms] [ntext] NOT NULL,
	[pc_minage] [int] NULL,
	[pc_maxage] [int] NULL,
	[pc_datecreated] [datetime] NOT NULL,
	[pc_dateends] [datetime] NULL,
 CONSTRAINT [PK_PhotoContests] PRIMARY KEY CLUSTERED 
(
	[pc_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Polls]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Polls](
	[p_id] [int] IDENTITY(1,1) NOT NULL,
	[p_title] [nvarchar](250) NOT NULL,
	[p_startdate] [datetime] NOT NULL,
	[p_enddate] [datetime] NOT NULL,
	[p_showresultsuntil] [datetime] NOT NULL,
 CONSTRAINT [PK_Polls] PRIMARY KEY CLUSTERED 
(
	[p_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfileTopics]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileTopics](
	[pt_id] [int] IDENTITY(1,1) NOT NULL,
	[pt_name] [nvarchar](250) NOT NULL,
	[pt_priority] [int] NOT NULL,
	[pt_editcolumns] [int] NOT NULL,
	[pt_viewcolumns] [int] NOT NULL,
 CONSTRAINT [PK_ProfileCategories] PRIMARY KEY CLUSTERED 
(
	[pt_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ScheduledAnnouncements]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ScheduledAnnouncements](
	[sa_id] [int] IDENTITY(1,1) NOT NULL,
	[sa_name] [nvarchar](100) NOT NULL,
	[sa_subject] [nvarchar](255) NOT NULL,
	[sa_body] [ntext] NOT NULL,
	[sa_gender] [int] NULL,
	[sa_paidmember] [bit] NULL,
	[sa_hasphotos] [bit] NULL,
	[sa_hasprofile] [bit] NULL,
	[sa_languageid] [int] NULL,
	[sa_country] [varchar](100) NULL,
	[sa_region] [nvarchar](100) NULL,
	[sa_type] [int] NOT NULL,
	[sa_date] [datetime] NULL,
	[sa_days] [int] NULL,
 CONSTRAINT [PK_ScheduledAnnouncements] PRIMARY KEY CLUSTERED 
(
	[sa_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Users]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Users](
	[u_username] [varchar](20) NOT NULL,
	[u_password] [varchar](250) NOT NULL,
	[u_email] [varchar](250) NOT NULL,
	[u_name] [nvarchar](250) NOT NULL,
	[u_gender] [int] NOT NULL,
	[u_birthdate] [datetime] NOT NULL,
	[u_birthdate2] [datetime] NULL,
	[u_usersince] [datetime] NOT NULL,
	[u_active] [bit] NOT NULL,
	[u_smsconfirmed] [bit] NOT NULL,
	[u_prevlogin] [datetime] NOT NULL,
	[u_lastlogin] [datetime] NOT NULL,
	[u_logincount] [int] NOT NULL,
	[u_lastonline] [datetime] NOT NULL,
	[u_profileviews] [int] NOT NULL,
	[u_profilevisible] [bit] NOT NULL,
	[u_receiveemails] [bit] NOT NULL,
	[u_country] [varchar](100) NOT NULL,
	[u_state] [nvarchar](100) NOT NULL,
	[u_zip_code] [varchar](15) NOT NULL,
	[u_city] [nvarchar](100) NOT NULL,
	[u_deleted] [bit] NOT NULL,
	[u_paid_member] [bit] NOT NULL,
	[u_signup_ip] [varchar](15) NOT NULL,
	[u_interested_in] [int] NULL,
	[u_lastsessionid] [varchar](32) NOT NULL,
	[u_message_verifications_left] [int] NOT NULL,
	[u_verifiedbyadmin] [bit] NOT NULL,
	[l_id] [int] NOT NULL,
	[u_incoming_messages_restrictions] [ntext] NULL,
	[u_billingdetails] [ntext] NULL,
	[u_invited_by] [varchar](20) NULL,
	[a_id] [int] NULL,
	[u_options] [bigint] NOT NULL,
	[u_deletereason] [nvarchar](200) NULL,
	[u_longitude] [float] NULL,
	[u_latitude] [float] NULL,
	[u_score] [int] NOT NULL,
	[u_tokenuniqueid] [varchar](100) NULL,
	[u_personalizationinfo] [ntext] NULL,
	[u_credits] [int] NOT NULL,
	[u_moderationscore] [int] NOT NULL,
	[u_spamsuspected] [bit] NOT NULL,
	[u_face_control_approved] [bit] NOT NULL,
	[u_profile_skin] [nvarchar](100) NULL,
	[u_status_text] [nvarchar](50) NULL,
	[u_featuredmember] [bit] NOT NULL,
	[u_myspaceid] [varchar](50) NULL,
	[u_facebookid] [bigint] NULL,
	[u_eventssettings] [bigint] NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VideoUploads]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoUploads](
	[vu_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[vu_processed] [bit] NOT NULL,
	[vu_approved] [bit] NOT NULL,
	[vu_private] [bit] NOT NULL,
 CONSTRAINT [PK_VideoUploads] PRIMARY KEY CLUSTERED 
(
	[vu_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[SP_GEN_INSERT_SQL]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
AUTHOR KARTHIK
DESCRIPTION BUILDS THE INSERT QUERY FOR ANY GIVEN TABLE
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------*/
CREATE PROCEDURE [dbo].[SP_GEN_INSERT_SQL]  
@TABLENAME NVARCHAR ( 2000 )
AS
BEGIN

DECLARE CUR_METADATA CURSOR FOR
SELECT NAME , XTYPE
FROM SYSCOLUMNS
WHERE ID IN ( SELECT ID FROM SYSOBJECTS WHERE NAME = @TABLENAME ) 
/* DECLARE VARIABLES*/
DECLARE @QRY1 NVARCHAR (4000 ) /* Has column names */
DECLARE @QRY2 NVARCHAR ( 4000) /* Has column values */

DECLARE @COLNAME NVARCHAR ( 4000) 
DECLARE @COLDATATYPE TINYINT
DECLARE @PRVCOLDATATYPE TINYINT 

DECLARE @CHARPREFIX NVARCHAR ( 5 )
DECLARE @CHARPOSTFIX NVARCHAR ( 5 )
DECLARE @COLUMNSEPARATOR CHAR ( 1 )
DECLARE @NULLCHAR CHAR ( 4 )

SET @CHARPREFIX = ''''
SET @CHARPOSTFIX = ''''
SET @COLUMNSEPARATOR = ','
SET @NULLCHAR = 'NULL'

/* Start building the query */
SET @QRY1 = "SELECT 'INSERT INTO " + @TABLENAME + "(" --+ " VALUES ( "

SET @QRY2 = '' 

OPEN CUR_METADATA

FETCH NEXT FROM CUR_METADATA INTO @COLNAME , @COLDATATYPE

WHILE @@FETCH_STATUS = 0
BEGIN 
/* Add the Column names to the query */
SET @QRY1 = @QRY1 + @COLNAME + ','

/* Add Column values - Prefix & postfix the column values with quotes if they belong to any of the following datatypes */
/* TEXT - 35 | SMALLDATETIME - 58 | DATETIME - 61 | NTEXT - 99 | VARCHAR - 167 | CHAR - 175 | NVARCHAR - 231 | NCHAR - 239  */
IF ( @COLDATATYPE = 35 ) OR ( @COLDATATYPE = 58 ) OR ( @COLDATATYPE = 61 ) OR ( @COLDATATYPE = 99 ) OR 
( @COLDATATYPE = 167 ) OR ( @COLDATATYPE = 175 ) OR ( @COLDATATYPE = 231 ) OR ( @COLDATATYPE = 239 ) 
BEGIN
IF @QRY2 = '' 
BEGIN
SET @QRY2 = @QRY2  + @CHARPREFIX  + @CHARPREFIX  + @CHARPREFIX + '+ISNULL(CAST(' + @COLNAME + ' AS NVARCHAR),' +  "'NULL'" + ')' --+ "'')"
END
ELSE
BEGIN
SET @QRY2 = @QRY2   + @CHARPREFIX  + @CHARPREFIX  + @CHARPREFIX +  '+ISNULL(CAST(' + @COLNAME + ' AS NVARCHAR),' +  "'NULL'" + ')' --+ "'')"
END
SET @QRY2 = @QRY2 + '+' + @CHARPOSTFIX + @CHARPOSTFIX + @CHARPOSTFIX + @COLUMNSEPARATOR
END
ELSE /*If the column is a number dont prefix/postfix quotes */
BEGIN
SET @QRY2 = @QRY2  + '''' +  '+ISNULL(CAST(' + @COLNAME + ' AS NVARCHAR),' +  "'NULL'" + ')'
SET @QRY2 = @QRY2 + '+'  + @CHARPOSTFIX + @COLUMNSEPARATOR
END
--PRINT @QRY2
SET @PRVCOLDATATYPE =  @COLDATATYPE 
FETCH NEXT FROM CUR_METADATA INTO @COLNAME , @COLDATATYPE 
END

CLOSE CUR_METADATA
DEALLOCATE CUR_METADATA

SET @QRY1 = SUBSTRING ( @QRY1 , 1 , LEN ( @QRY1 ) - 1 )
SET @QRY1 = @QRY1 + ')VALUES('  --+ "'" + "'" +"'"

SET @QRY2 = SUBSTRING ( @QRY2 , 1 , LEN ( @QRY2 )  - 1 )
SET @QRY2 = @QRY2 + ')' + @CHARPOSTFIX
SET @QRY2 = @QRY2 + ' FROM ' + @TABLENAME

/* Create a temporary table to hold the result of the query - i.e. it will have the INSERT statements */ 
CREATE TABLE #SQLTRACE ( SQL NVARCHAR ( 4000 ) ) 
--print  @QRY1 + @QRY2
SET @QRY2 =  ' INSERT INTO #SQLTRACE '  + @QRY1 + @QRY2
IF LEN ( @QRY2 )  < 4000 
BEGIN
SET NOCOUNT ON
--PRINT @QRY2
EXEC ( @QRY2 )
--PRINT  ' INSERT INTO #SQLTRACE '  + @QRY1 + @QRY2
--EXEC ( ' INSERT INTO #SQLTRACE '  + @QRY1 + @QRY2 )
UPDATE #SQLTRACE SET SQL = REPLACE ( SQL , "'NULL'" , @NULLCHAR )
SELECT * FROM #SQLTRACE 
END
ELSE
BEGIN
PRINT 'UNABLE TO GENERATE INSERT STATEMENTS FOR TABLE :' +@TABLENAME
END
SET NOCOUNT OFF
END /* END OF FILE */
GO
/****** Object:  Table [dbo].[Settings]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Settings](
	[s_key] [varchar](80) NOT NULL,
	[s_value] [ntext] NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[s_key] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Templates]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Templates](
	[t_key] [varchar](50) NOT NULL,
	[t_value] [ntext] NOT NULL,
	[t_description] [ntext] NOT NULL,
 CONSTRAINT [PK_Templates] PRIMARY KEY CLUSTERED 
(
	[t_key] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserLevels]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserLevels](
	[ul_id] [int] IDENTITY(1,1) NOT NULL,
	[ul_name] [nvarchar](100) NOT NULL,
	[ul_minscore] [int] NOT NULL,
	[ul_restrictions] [text] NULL,
 CONSTRAINT [PK_UserLevels] PRIMARY KEY CLUSTERED 
(
	[ul_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Votes]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Votes](
	[v_fromusername] [varchar](20) NOT NULL,
	[v_tousername] [varchar](20) NOT NULL,
	[v_score] [int] NOT NULL,
	[v_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[v_fromusername] ASC,
	[v_tousername] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[VoteUser]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[VoteUser]
(
	@FromUsername	VARCHAR(20),
	@ToUsername		VARCHAR(20),
	@Score			INT
)
AS
IF EXISTS (SELECT v_score FROM Votes WHERE v_fromusername = @FromUsername AND v_tousername = @ToUsername)
	UPDATE
		Votes
	SET
		v_score = @Score
	WHERE
		v_fromusername = @FromUsername
		AND v_tousername = @ToUsername
ELSE
BEGIN
	INSERT INTO Votes
		(v_fromusername, v_tousername, v_score)
	VALUES
		(@FromUsername, @ToUsername, @Score)

END
GO
/****** Object:  StoredProcedure [dbo].[UpdateSetting]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateSetting]
(
	@Key	varchar(80),
	@Value ntext
)
 AS
	IF (EXISTS(SELECT * FROM Settings WHERE @Key = s_key))
	UPDATE Settings
	SET
		s_value = @Value
	WHERE
		@Key = s_key
	ELSE
	INSERT INTO Settings
		(s_key, s_value)
	VALUES
		(@Key, @Value)
GO
/****** Object:  StoredProcedure [dbo].[VideoUploadsSearch]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[VideoUploadsSearch] 
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
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateUser]
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
/****** Object:  Table [dbo].[Subscriptions]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Subscriptions](
	[s_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[p_id] [int] NOT NULL,
	[s_order_date] [datetime] NOT NULL,
	[s_renew_date] [datetime] NOT NULL,
	[s_confirmed] [bit] NOT NULL,
	[s_cancelled] [bit] NOT NULL,
	[s_cancellation_requested] [bit] NOT NULL,
	[s_paymentprocessor] [varchar](50) NOT NULL,
	[s_custom] [varchar](50) NULL,
 CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED 
(
	[s_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TwitterCredentials]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TwitterCredentials](
	[u_username] [varchar](20) NOT NULL,
	[tc_username] [nvarchar](200) NOT NULL,
	[tc_password] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_TwitterCredentials] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Translations]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Translations](
	[l_id] [int] NOT NULL,
	[t_key] [nvarchar](400) NOT NULL,
	[t_adminpanel] [int] NOT NULL,
	[t_value] [nvarchar](400) NOT NULL,
 CONSTRAINT [PK_Translations_1] PRIMARY KEY CLUSTERED 
(
	[l_id] ASC,
	[t_key] ASC,
	[t_adminpanel] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[SearchPhotoContests]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchPhotoContests]
	(
		@Active			BIT,
		@SortColumn		VARCHAR(50)
	)
AS

SELECT
		pc_id AS ID
		
FROM PhotoContests

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
GO
/****** Object:  Table [dbo].[VideoEmbeds]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoEmbeds](
	[ve_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ve_thumburl] [varchar](200) NULL,
	[ve_videourl] [varchar](200) NOT NULL,
	[ve_title] [nvarchar](200) NULL,
	[ve_sourcetype] [int] NOT NULL,
 CONSTRAINT [PK_VideoEmbeds] PRIMARY KEY CLUSTERED 
(
	[ve_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VideoAccess]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoAccess](
	[va_videoviewer] [varchar](20) NOT NULL,
	[va_videoowner] [varchar](20) NOT NULL,
	[va_dateaccessgranted] [datetime] NOT NULL,
 CONSTRAINT [PK_VideoAccess_1] PRIMARY KEY CLUSTERED 
(
	[va_videoviewer] ASC,
	[va_videoowner] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[VerifiedUsers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VerifiedUsers](
	[vu_verifiedby] [varchar](20) NOT NULL,
	[vu_verifieduser] [varchar](20) NOT NULL,
	[vu_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_VerifiedUsers] PRIMARY KEY CLUSTERED 
(
	[vu_verifiedby] ASC,
	[vu_verifieduser] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserScoresLog]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserScoresLog](
	[usl_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[usl_timestamp] [datetime] NOT NULL,
	[usl_score] [int] NOT NULL,
	[usl_notes] [nvarchar](200) NULL,
 CONSTRAINT [PK_UserScoresLog] PRIMARY KEY CLUSTERED 
(
	[usl_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UnlockedSections]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UnlockedSections](
	[us_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[us_type] [int] NOT NULL,
	[us_targetusername] [varchar](20) NOT NULL,
	[us_targetid] [int] NULL,
	[us_unlockeduntil] [datetime] NOT NULL,
 CONSTRAINT [PK_UnlockedSections] PRIMARY KEY CLUSTERED 
(
	[us_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[UpdateLastOnline]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateLastOnline]
(
	@Username	VARCHAR(20)
)
AS
	UPDATE Users
	SET
		u_lastonline = GETDATE()
	WHERE
		u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[UpdateLastLogin]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateLastLogin]
(
	@Username	VARCHAR(20),
	@SessionID	VARCHAR(32)
)
AS
	UPDATE Users
	SET
		u_prevlogin = u_lastlogin,
		u_lastlogin = GETDATE(),
		u_logincount = u_logincount + 1,
		u_lastsessionid = COALESCE(@SessionID, u_lastsessionid)
	WHERE
		u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[UpdateBillingPlan]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateBillingPlan]
(
	@ID int,
	@Title nvarchar(200),
	@Amount money,
	@Cycle int,
	@CycleUnit TINYINT,
	@Options text
)
 AS
	UPDATE BillingPlans
	SET
		p_title = @Title,
		p_amount = @Amount,
		p_billing_cycle = @Cycle,
		p_billing_cycle_unit = @CycleUnit,
		p_options = @Options
	WHERE
		@ID = p_id
GO
/****** Object:  StoredProcedure [dbo].[UpdateAdminLastLogin]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateAdminLastLogin]
(
	@Username	VARCHAR(20),
	@SessionID	VARCHAR(32)
)
AS
	UPDATE Admins
	SET
		a_lastlogin = GETDATE(),
		a_lastsessionid = @SessionID
	WHERE
		a_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[UpdateAdmin]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateAdmin]
(
	@Username	VARCHAR(20),
	@Password	VARCHAR(250),
	@Privileges	VARCHAR(4000)
)
 AS
	UPDATE Admins
	SET
		a_password = @Password,
		a_lastlogin = getdate(),
		a_privileges = @Privileges
	WHERE
		a_username = @Username
GO
/****** Object:  Table [dbo].[VideoProfiles]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[VideoProfiles](
	[u_username] [varchar](20) NOT NULL,
	[vp_approved] [bit] NOT NULL,
	[vp_private] [bit] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[SaveUserLevel]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveUserLevel] 
	(
		@Id				INT,
		@Name			NVARCHAR(100),
		@MinScore		INT,
		@Restrictions	TEXT
	)
AS

IF @Id IS NULL
BEGIN
	INSERT INTO UserLevels
		(ul_name, ul_minscore, ul_restrictions)
		VALUES 
		(@Name, @MinScore, @Restrictions)
	
	SELECT @@Identity
END
ELSE
BEGIN
	UPDATE UserLevels
		SET ul_name = @Name, ul_minscore = @MinScore, ul_restrictions = @Restrictions
	WHERE ul_id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[SearchAbuseReports]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchAbuseReports]
(
	@ReportedBy	varchar(20),
	@ReportedUser varchar(250),
	@ReportType int,
	@ReportedId	int,
	@Reviewed bit,
	@FromDate datetime,
	@ToDate datetime,
	@SortColumn	varchar(50)
)
AS
SELECT
	ar_id as [ID]
FROM
	AbuseReports
WHERE
	(@ReportedBy IS NULL OR @ReportedBy = ar_reportedby) AND
	(@ReportedUser IS NULL OR @ReportedUser = ar_reporteduser) AND
	((@FromDate IS NULL AND ar_datereported > '1/1/2000') OR ar_datereported > @FromDate) AND
	((@ToDate IS NULL AND ar_datereported < '1/1/2100') OR ar_datereported < @ToDate) AND
	(@ReportType IS NULL OR @ReportType = ar_type) AND
	(@ReportedId IS NULL OR @ReportedId = ar_targetid) AND
	(@Reviewed IS NULL OR @Reviewed = ar_reviewed)
ORDER BY
	CASE @SortColumn
		WHEN 'ReportedBy' THEN ar_reportedby
		WHEN 'ReportedUser' THEN ar_reporteduser
	END,
	CASE @SortColumn
		WHEN 'DateReported' THEN ar_datereported
	END,
	CASE @SortColumn
		WHEN 'ReportType' THEN ar_type
	END
GO
/****** Object:  StoredProcedure [dbo].[SavePhotoContest]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePhotoContest] 
(
	@ID int,
	@Name nvarchar(500),
	@Gender int,
	@Description ntext,
	@Terms	ntext,
	@MinAge	int,
	@MaxAge	int,
	@DateCreated datetime,
	@DateEnds datetime
)
AS
IF (@ID IS NULL)
BEGIN
	INSERT INTO PhotoContests
		(pc_name, pc_gender, pc_description, pc_terms, pc_minage, pc_maxage, pc_datecreated, pc_dateends)
	VALUES
		(@Name, @Gender, @Description, @Terms, @MinAge, @MaxAge, @DateCreated, @DateEnds)
	SELECT @@IDENTITY
END
ELSE 
	UPDATE PhotoContests SET 
		pc_name = @Name, 
		pc_gender = @Gender,
		pc_description = @Description,
		pc_terms = @Terms,
		pc_minage = @MinAge,
		pc_maxage = @MaxAge,
		pc_datecreated = @DateCreated,
		pc_dateends = @DateEnds
	WHERE pc_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveScheduledAnnouncement]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveScheduledAnnouncement]
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
/****** Object:  StoredProcedure [dbo].[SaveVideoUpload]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveVideoUpload]
	@Id	INT = NULL,
	@Username	VARCHAR(20),
	@Processed	BIT = 0,
	@Approved	BIT = 0,
	@IsPrivate BIT = 0
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Id IS NULL)
	BEGIN
		INSERT INTO VideoUploads
			(u_username, vu_processed, vu_approved, vu_private)
		VALUES
			(@Username, @Processed, @Approved, @IsPrivate)

		SELECT @@Identity
	END
	ELSE
	BEGIN
		UPDATE VideoUploads
		SET 
			u_username = @Username, vu_processed = @Processed, vu_approved = @Approved, vu_private = @IsPrivate
		WHERE
			vu_id = @Id
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveProfileTopic]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveProfileTopic]
(
	@ID		int=null,
	@Name		nvarchar(250),
	@EditColumns	int,
	@ViewColumns	int
	
)
 AS
	if (@ID IS NULL)
	BEGIN
		DECLARE @Priority int
		SET @Priority = 
			(SELECT MAX(pt_priority) FROM ProfileTopics) + 1
		IF (@Priority IS NULL) 
			SET @Priority = 1
	
		INSERT INTO ProfileTopics
			(pt_name, pt_editcolumns, pt_viewcolumns, pt_priority)
		VALUES
			(@Name, @EditColumns, @ViewColumns, @Priority)
	END
	ELSE
		UPDATE ProfileTopics
		SET 
			pt_name = @Name,
			pt_editcolumns = @EditColumns,
			pt_viewcolumns = @ViewColumns
		WHERE
			pt_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveLanguage]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveLanguage]
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
/****** Object:  StoredProcedure [dbo].[SaveIPLog]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveIPLog]
(
	@Username varchar(20),
	@IP varchar(15),
	@Action int
)
AS

INSERT INTO IPLogs
	(u_username, ipl_ip, ipl_action)
VALUES
	(@Username, @IP, @Action)
GO
/****** Object:  StoredProcedure [dbo].[SaveHourlyStats]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveHourlyStats]
	(
		@DateTime DATETIME,
		@OnlineUsers INT,
		@NewUsers INT,
		@SentMessages INT
	)
AS

IF NOT EXISTS (SELECT 1 FROM HourlyStats WHERE hs_time = @DateTime)
		INSERT INTO HourlyStats (hs_time, hs_usersonline, hs_newusers, hs_sentmessages)
							VALUES (@DateTime, @OnlineUsers, @NewUsers, @SentMessages)
ELSE
	UPDATE HourlyStats
	SET hs_usersonline = @OnlineUsers,
		hs_newusers = @NewUsers,
		hs_sentmessages = @SentMessages
	WHERE hs_time = @DateTime
GO
/****** Object:  StoredProcedure [dbo].[SaveChatUser]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveChatUser]
	@Id int = null,
	@Username varchar(100),
	@DisplayName nvarchar(200),
	@BeginTime datetime = getdate,
	@ActiveTime datetime = getdate,
	@Ip varchar(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@Id IS NULL)
	BEGIN

		INSERT INTO ChatUsers
			(cu_username, cu_displayname, cu_begintime, cu_activetime, cu_ip)
		VALUES
			(@Username, @DisplayName, @BeginTime, @ActiveTime, @Ip)

		SELECT @@Identity

	END
	ELSE
	BEGIN

		UPDATE ChatUsers
		SET cu_username = @Username, cu_displayname = @DisplayName, cu_begintime = @BeginTime,
			cu_activetime = @ActiveTime, cu_ip = @Ip
		WHERE cu_id = @Id
	
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveChatMessage]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveChatMessage] 
	@Id int = null,
	@Time datetime = getdate,
	@ChatRoomId	int = null,
	@SenderUserId int = null,
	@SenderDisplayName nvarchar(100),
	@TargetUserId int = null,
	@Text nvarchar(2000),
	@TextHtml nvarchar(2000)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@Id IS NULL)
	BEGIN

		INSERT INTO ChatMessages
			(cm_time, cr_id, cu_id_sender, cm_senderdisplayname, cu_id_target, cm_text, cm_texthtml)
		VALUES
			(@Time, @ChatRoomId, @SenderUserId, @SenderDisplayName, @TargetUserId, @Text, @TextHtml)

		SELECT @@Identity

	END
	ELSE
	BEGIN

		UPDATE ChatMessages
		SET cm_time = @Time, cr_id = @ChatRoomId, cu_id_sender = @SenderUserId, 
			cm_senderdisplayname = @SenderDisplayName, cu_id_target = @TargetUserId, cm_text = @Text,
			cm_texthtml = @TextHtml
		WHERE cr_id = @Id
	
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveChatBan]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveChatBan]
	@Id int = null,
	@ChatRoomId int = null,
	@UserId int = null,
	@UserIp varchar(15) = null,
	@BanDate datetime = getdate,
	@BanExpires datetime = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    IF (@Id IS NULL)
	BEGIN

		INSERT INTO ChatBans
			(cr_id, cu_id, cb_ip, cb_date, cb_dateexpires)
		VALUES
			(@ChatRoomId, @UserId, @UserIp, @BanDate, @BanExpires)

		SELECT @@Identity

	END
	ELSE
	BEGIN

		UPDATE ChatBans
		SET cr_id = @ChatRoomId, cu_id = @UserId, cb_ip = @UserIp, cb_date = @BanDate,
			cb_dateexpires = @BanExpires
		WHERE cb_id = @Id
	
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveCategory]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveCategory] 
	(
		@ID		INT,
		@Name	NVARCHAR(200),
		@UsersCanCreateGroups BIT
	)
AS

IF (@ID IS NULL)
	BEGIN
		
		DECLARE @Priority int
		SET @Priority = 
			(SELECT MAX(c_order) FROM Categories) + 1
		IF (@Priority IS NULL) 
			SET @Priority = 1
			
		INSERT INTO Categories
		(c_name, c_order, c_userscancreategroups)
		VALUES
		(@Name, @Priority, @UsersCanCreateGroups)
		
		SELECT @@Identity
	END
	
ELSE
	BEGIN
		UPDATE Categories
		
		SET c_name = @Name,
			c_userscancreategroups = @UsersCanCreateGroups
			
		WHERE c_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[IrregularSearchUsers]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IrregularSearchUsers]
	(
		@Date	DATETIME
	)
AS

SELECT u_username AS Username
FROM Users
WHERE	CAST (FLOOR (CAST (u_lastlogin AS FLOAT)) AS DATETIME) = @Date
		AND u_deleted = 0
GO
/****** Object:  Table [dbo].[SavedSearches]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SavedSearches](
	[ss_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ss_name] [nvarchar](100) NOT NULL,
	[ss_gender] [int] NOT NULL,
	[ss_country] [varchar](100) NOT NULL,
	[ss_state] [nvarchar](100) NOT NULL,
	[ss_zip] [varchar](15) NOT NULL,
	[ss_city] [varchar](100) NOT NULL,
	[ss_agefrom] [int] NOT NULL,
	[ss_ageto] [int] NOT NULL,
	[ss_photoreq] [bit] NOT NULL,
	[ss_choiceids] [text] NOT NULL,
	[ss_emailmatches] [bit] NOT NULL,
	[ss_nextemaildate] [datetime] NULL,
	[ss_emailfrequency] [int] NOT NULL,
 CONSTRAINT [PK_SavedSearches] PRIMARY KEY CLUSTERED 
(
	[ss_id] ASC
) ON [PRIMARY],
 CONSTRAINT [IX_SavedSearches] UNIQUE NONCLUSTERED 
(
	[u_username] ASC,
	[ss_name] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[SaveDiscount]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveDiscount]
(
	@ID int,
	@Amount money,
	@Type int,
	@Field int,
	@Match nvarchar(100)
)
 AS
	IF (@ID = -1)
	BEGIN
		INSERT INTO Discounts
			(d_amount, d_type, d_field, d_match)
		VALUES
			(@Amount, @Type, @Field, @Match)
	
		SELECT @@IDENTITY
	END
	ELSE
		UPDATE Discounts
		SET
			d_amount =	@Amount,
			d_type	=	@Type,
			d_field	=	@Field,
			d_match =	@Match
		WHERE
			d_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveCreditsPackage]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveCreditsPackage]
	(
		@ID INT,
		@Name NVARCHAR(200),
		@Quantity INT,
		@Price MONEY
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO CreditsPackages (cp_name, cp_quantity, cp_price)
						VALUES (@Name, @Quantity, @Price)
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE CreditsPackages
		SET cp_name = @Name,
			cp_quantity = @Quantity,
			cp_price = @Price
		WHERE cp_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupIcon]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupIcon] 
	(
		@ID		INT,
		@Image	IMAGE
	)
AS

UPDATE Groups

SET g_icon = @Image

WHERE g_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveGroup]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroup] 
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
/****** Object:  StoredProcedure [dbo].[SaveContentView]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveContentView]
(
	@Key varchar(50),
	@Content NTEXT,
	@Language INT
)

 AS
IF @Key IS NOT NULL AND
   @Language IS NOT NULL AND
   NOT EXISTS (SELECT * FROM ContentViews WHERE @Key = cv_key AND @Language = l_id)
	INSERT INTO ContentViews 
		(cv_key, cv_content, l_id)
		VALUES
		(@Key, @Content, @Language)
ELSE
	UPDATE ContentViews
	SET	cv_content = @Content
	WHERE @Key = cv_key AND @Language = l_id
GO
/****** Object:  StoredProcedure [dbo].[SaveEmailQueue]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveEmailQueue]   
 (   
      @Id          int = -1,   
      @From          varchar(250),   
      @To          varchar(250),   
      @FromName    nvarchar(250),   
      @ToName      nvarchar(250),   
      @CC          varchar(250) = null,   
      @BCC          varchar(250) = null,   
      @Subject     ntext,   
      @Body          ntext,   
      @Tries          int = 0,   
      @LastTry     datetime = null,   
      @LastError     text = null,   
      @NextTry     datetime = null   
 )   
 AS   
    
 IF (@Id = -1)   
  BEGIN   
      INSERT INTO EmailQueue   
      (eq_from, eq_to, eq_fromname, eq_toname, eq_cc, eq_bcc, eq_subject, eq_body, eq_next_try)   
      VALUES   
      (@From, @To, @FromName, @ToName, @CC, @BCC, @Subject, @Body, @NextTry)   
    
      SELECT @@Identity   
 END   
 ELSE   
 BEGIN   
      UPDATE EmailQueue   
      SET eq_from = @From, eq_to = @To, eq_fromname = @FromName, eq_toname = @ToName, eq_cc = @CC,   
           eq_bcc = @BCC, eq_subject = @Subject, eq_body = @Body,   
           eq_tries = @Tries, eq_last_try = @LastTry, eq_last_error = @LastError,   
           eq_next_try = @NextTry
     WHERE eq_id = @Id   
 END
GO
/****** Object:  StoredProcedure [dbo].[SaveEcardTypeContent]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveEcardTypeContent]
	(
		@EcardTypeID INT,
		@Content IMAGE
	)
AS

UPDATE EcardTypes
SET ect_content = @Content
WHERE ect_id = @EcardTypeID
GO
/****** Object:  StoredProcedure [dbo].[SaveEcardType]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveEcardType]
	(
		@ID INT,
		@Name NVARCHAR(200),
		@Content IMAGE,
		@Type INT,
		@Active BIT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO EcardTypes (ect_name, ect_content, ect_type, ect_active)
					VALUES (@Name, @Content, @Type, @Active)
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE EcardTypes
		SET ect_name = @Name,
			ect_content = @Content,
			ect_type = @Type,
			ect_active = @Active
		WHERE ect_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveAffiliateBanner]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAffiliateBanner]
	(
		@ID			INT,
		@Name		NVARCHAR(200),
		@Deleted	BIT,
		@Image		IMAGE
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO AffiliateBanners (ab_name, ab_deleted, ab_image) VALUES (@Name, @Deleted, @Image)
		
		SELECT @@Identity
	END

ELSE

	BEGIN
		UPDATE AffiliateBanners
		SET ab_name = @Name, ab_deleted = @Deleted, ab_image = @Image
		WHERE ab_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveAffiliate]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAffiliate] 
	(
		@ID				INT,
		@Username		VARCHAR(20),
		@Password		VARCHAR(250),
		@Name			NVARCHAR(100),
		@Email			VARCHAR(250),
		@SiteURL		VARCHAR(100),
		@PaymentDetails NTEXT,
		@Deleted		BIT,
		@Active			BIT,
		@Percentage		INT,
		@FixedAmount	MONEY,
		@Recurrent		BIT,
		@Balance		MONEY,
		@RequestPayment BIT
	)
AS

IF (@ID IS NULL)

	BEGIN
		INSERT INTO Affiliates (a_username, a_password, a_name, a_email, a_siteurl, a_payment_details, a_deleted, a_active, a_percentage, a_fixed_amount, a_recurrent, a_balance, a_request_payment)
					VALUES (@Username, @Password, @Name, @Email, @SiteURL, @PaymentDetails, @Deleted, @Active, @Percentage, @FixedAmount, @Recurrent, @Balance, @RequestPayment)
		SELECT @@Identity
	END
	
ELSE

	BEGIN
		UPDATE	Affiliates
		SET		a_username = @Username,
				a_password = @Password,
				a_name = @Name,
				a_email = @Email,
				a_siteurl = @SiteURL,
				a_payment_details = @PaymentDetails,
				a_deleted = @Deleted,
				a_active = @Active,
				a_percentage = @Percentage,
				a_fixed_amount = @FixedAmount,
				a_recurrent = @Recurrent,
				a_balance = @Balance,
				a_request_payment = @RequestPayment
				
		WHERE a_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveAdsCategory]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAdsCategory]
	(
		@ID INT,
		@ParentID INT,
		@Title NVARCHAR(200)
	)
AS

IF @ID IS NULL
	BEGIN
		INSERT INTO AdsCategories (ac_parentid, ac_title) VALUES (@ParentID, @Title)
		SELECT @@Identity
	END

ELSE
	UPDATE AdsCategories
	SET ac_parentid = @ParentID,
		ac_title = @Title
	WHERE ac_id = @ID
GO
/****** Object:  Table [dbo].[ProfileViews]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProfileViews](
	[pv_viewer] [varchar](20) NOT NULL,
	[pv_viewed] [varchar](20) NOT NULL,
	[pv_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_ProfileViews] PRIMARY KEY CLUSTERED 
(
	[pv_viewer] ASC,
	[pv_viewed] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[RemovePendingGuid]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemovePendingGuid] 
(
	@Guid varchar(36),
	@Username varchar(20)
)
AS
	DELETE 
	FROM 
		PendingGuids
	WHERE
		(@Guid='' OR @Guid = pg_guid) AND
		(@Username='' OR @Username=pg_username)
GO
/****** Object:  StoredProcedure [dbo].[SaveAbuseReport]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAbuseReport]
(
	@ID	INT,
	@ReportedBy	varchar(20),
	@ReportedUser varchar(250),
	@ReportType int,	
	@Report	ntext,
	@Reviewed bit,
	@ReportedItemID int,
	@DateReported datetime
)
AS
	IF (@ID IS NULL)
		INSERT INTO AbuseReports
			(ar_reportedby, ar_reporteduser, ar_report, ar_type, ar_reviewed, ar_targetid, ar_datereported)
		VALUES
			(@ReportedBy, @ReportedUser, @Report, @ReportType, @Reviewed, @ReportedItemID, @DateReported)
	ELSE
		UPDATE AbuseReports
		SET
			ar_reportedby = @ReportedBy,
			ar_reporteduser = @ReportedUser,
			ar_report = @Report,
			ar_type = @ReportType,
			ar_reviewed = @Reviewed,
			ar_targetid = @ReportedItemID,
			ar_datereported = @DateReported
		WHERE
			ar_id = @ID
GO
/****** Object:  Table [dbo].[Relationships]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Relationships](
	[r_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[r_username] [varchar](20) NULL,
	[r_type] [int] NOT NULL,
	[r_pendingtype] [int] NULL,
	[r_accepted] [bit] NOT NULL,
	[r_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Relationships] PRIMARY KEY CLUSTERED 
(
	[r_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ratings]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ratings](
	[r_fromusername] [varchar](20) NOT NULL,
	[r_tousername] [varchar](20) NOT NULL,
	[r_rating] [int] NOT NULL,
	[r_timestamp] [datetime] NOT NULL,
	[r_ip] [varchar](15) NULL,
 CONSTRAINT [PK_Ratings] PRIMARY KEY CLUSTERED 
(
	[r_fromusername] ASC,
	[r_tousername] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MessagesSandbox]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MessagesSandbox](
	[u_username] [varchar](20) NOT NULL,
	[ms_message] [nvarchar](100) NOT NULL,
	[ms_count] [int] NOT NULL,
	[ms_datetime] [datetime] NOT NULL,
 CONSTRAINT [PK_MessagesSandbox] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC,
	[ms_message] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProfileQuestions]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileQuestions](
	[pq_id] [int] IDENTITY(1,1) NOT NULL,
	[pt_id] [int] NOT NULL,
	[pq_name] [nvarchar](250) NOT NULL,
	[pq_altname] [nvarchar](250) NOT NULL,
	[pq_description] [nvarchar](4000) NOT NULL,
	[pq_hint] [nvarchar](250) NOT NULL,
	[pq_edit_style] [int] NOT NULL,
	[pq_show_style] [int] NOT NULL,
	[pq_search_style] [int] NOT NULL,
	[pq_required] [bit] NOT NULL,
	[pq_priority] [int] NOT NULL,
	[pq_requires_approval] [bit] NOT NULL,
	[pq_visible_male] [bit] NOT NULL,
	[pq_visible_female] [bit] NOT NULL,
	[pq_visible_couple] [bit] NOT NULL,
	[pq_match_field] [int] NULL,
	[pq_viewpaidonly] [bit] NOT NULL,
	[pq_editpaidonly] [bit] NOT NULL,
	[pq_parent_question_id] [int] NULL,
	[pq_parent_question_choices] [nvarchar](3000) NULL,
 CONSTRAINT [PK_ProfileFields] PRIMARY KEY CLUSTERED 
(
	[pq_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PollChoices]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PollChoices](
	[pc_id] [int] IDENTITY(1,1) NOT NULL,
	[p_id] [int] NOT NULL,
	[pc_answer] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_PollChoices] PRIMARY KEY CLUSTERED 
(
	[pc_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PhotoContestEntries]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoContestEntries](
	[pce_id] [int] IDENTITY(1,1) NOT NULL,
	[pc_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[p_id] [int] NOT NULL,
 CONSTRAINT [PK_PhotoContestPhotos] PRIMARY KEY CLUSTERED 
(
	[pce_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[IsValidPendingGuid]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsValidPendingGuid] 
(
	@Username varchar(20),
	@Guid varchar(36)
)
AS
 IF EXISTS(
	SELECT
		 pg_guid as Guid
	FROM
		PendingGuids
	WHERE
		@Username = pg_username AND
		@Guid = pg_guid
	)
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[LoadAffiliate]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadAffiliate] 
	(
		@Username VARCHAR(20)
	)
AS

SELECT a_id, a_username, a_password
FROM Affiliates
WHERE a_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[LoadAdmin]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadAdmin]
(
	@Username varchar(20)
)
AS
	SELECT 
		a_username as Username,
		a_password as Password,
		a_lastlogin as LastLogin,
		a_privileges as [Privileges],
		a_lastsessionid as LastSessionID
	FROM
		Admins
	WHERE
		@Username = a_username
GO
/****** Object:  StoredProcedure [dbo].[LoadAbuseReport]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadAbuseReport]
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
	AbuseReports
WHERE
	ar_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[IsTrialPeriodExpired]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[IsTrialPeriodExpired]
(
	@Username varchar(20),
	@TrialPeriod int
) AS
DECLARE @SubscriptionDate DateTime

SELECT @SubscriptionDate = u_usersince FROM Users WHERE u_username = @Username

IF (DATEADD(day,@TrialPeriod, @SubscriptionDate) < getdate())
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[MessagesExist]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[MessagesExist]
(
	@FromUsername varchar(20),
	@ToUsername varchar(20)
) AS
 IF EXISTS(
	SELECT
		1
	FROM
		MessagesHistoryCache
	WHERE
		@FromUsername = mhc_fromusername AND @ToUsername = mhc_tousername
	)
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)
GO
/****** Object:  Table [dbo].[Messages]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Messages](
	[m_id] [int] IDENTITY(1,1) NOT NULL,
	[m_from_username] [varchar](20) NOT NULL,
	[m_from_folder] [int] NOT NULL,
	[m_to_username] [varchar](20) NOT NULL,
	[m_to_folder] [int] NOT NULL,
	[m_body] [nvarchar](3500) NOT NULL,
	[m_timestamp] [datetime] NOT NULL,
	[m_replied_to] [int] NOT NULL,
	[m_is_read] [bit] NOT NULL,
	[m_pending_approval] [bit] NOT NULL,
	[m_new] [bit] NOT NULL,
 CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED 
(
	[m_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[LoadVideoUploads]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadVideoUploads]
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
/****** Object:  Table [dbo].[PhotoAlbums]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoAlbums](
	[pa_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[pa_name] [nvarchar](50) NOT NULL,
	[pa_access] [int] NOT NULL,
	[pa_cover_photoid] [int] NULL,
 CONSTRAINT [PK_PhotoAlbums] PRIMARY KEY CLUSTERED 
(
	[pa_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PhotoAccess]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoAccess](
	[pa_photoowner] [varchar](20) NOT NULL,
	[pa_photoviewer] [varchar](20) NOT NULL,
	[pa_dateaccessgranted] [datetime] NOT NULL,
 CONSTRAINT [PK_PhotoAccess] PRIMARY KEY CLUSTERED 
(
	[pa_photoowner] ASC,
	[pa_photoviewer] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PaymentHistory]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PaymentHistory](
	[ph_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ph_paymentprocessor] [varchar](50) NOT NULL,
	[ph_amount] [money] NOT NULL,
	[ph_description] [nvarchar](200) NOT NULL,
	[ph_notes] [nvarchar](2000) NOT NULL,
	[ph_status] [int] NOT NULL,
	[ph_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_PaymentHistory] PRIMARY KEY CLUSTERED 
(
	[ph_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[OnlineSearchUsers]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[OnlineSearchUsers]
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
		Users
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
GO
/****** Object:  StoredProcedure [dbo].[LoadUserByTokenUniqueId]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[LoadUserByTokenUniqueId]
(
	@TokenUniqueId varchar(100)
)
 AS
SELECT TOP 1
		u_username as Username
	FROM 
		Users
	WHERE
		u_tokenuniqueid = @TokenUniqueId AND u_deleted = 0
	ORDER BY
		u_active DESC, u_smsconfirmed DESC
GO
/****** Object:  StoredProcedure [dbo].[LoadUserByEmail]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[LoadUserByEmail]
(
	@Email	varchar(250)
)
 AS
SELECT
		u_username as Username
	FROM 
		Users
	WHERE
		u_email = @Email AND u_deleted = 0
ORDER BY u_usersince DESC
GO
/****** Object:  StoredProcedure [dbo].[LoadUser]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadUser] 
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
		Users
	WHERE
		u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[LoadEmailQueue]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadEmailQueue]   
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
      EmailQueue   
 WHERE   
      (@Id = -1 OR eq_id = @Id)   
      AND (@To IS NULL OR eq_to = @To)   
      AND (@MinTries = -1 OR eq_tries >= @MinTries)   
      AND (@MaxTries = -1 OR eq_tries <= @MaxTries)   
      AND (@MinNextTryDate IS NULL OR eq_next_try IS NULL OR eq_next_try <= @MinNextTryDate)
GO
/****** Object:  StoredProcedure [dbo].[LoadPhotoContest]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadPhotoContest]
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
		PhotoContests
	WHERE
		@Id IS NULL OR pc_id = @Id
	ORDER BY pc_datecreated DESC
END
GO
/****** Object:  Table [dbo].[News]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[News](
	[n_id] [int] IDENTITY(1,1) NOT NULL,
	[n_date] [datetime] NOT NULL,
	[n_text] [ntext] NOT NULL,
	[n_title] [nvarchar](100) NOT NULL,
	[l_id] [int] NOT NULL,
 CONSTRAINT [PK_News] PRIMARY KEY CLUSTERED 
(
	[n_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[MySearchUsers]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MySearchUsers]
 AS
	SELECT
		u_username as Username
	FROM
		Users
	WHERE
		u_active = 1 AND u_deleted = 0 AND u_face_control_approved = 1
GO
/****** Object:  StoredProcedure [dbo].[MutualVoteSearch]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MutualVoteSearch]
(
	@Username		VARCHAR(20)
)
AS
	SELECT V1.v_tousername as Username
	FROM Votes as V1, Votes as V2
	WHERE V1.v_fromusername = @Username
		AND V1.v_score = 1
		AND V2.v_tousername = @Username
		AND V2.v_score = 1
		AND V1.v_tousername = V2.v_fromusername
	ORDER BY V1.v_timestamp DESC
GO
/****** Object:  Table [dbo].[GroupTopics]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupTopics](
	[gt_id] [int] IDENTITY(1,1) NOT NULL,
	[g_id] [int] NOT NULL,
	[gt_name] [nvarchar](200) NOT NULL,
	[gt_datecreated] [datetime] NOT NULL,
	[gt_dateupdated] [datetime] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gt_posts] [int] NOT NULL,
	[gt_locked] [bit] NOT NULL,
	[gt_stickyuntil] [datetime] NULL,
	[gt_poll] [bit] NOT NULL,
	[gt_views] [int] NOT NULL,
 CONSTRAINT [PK_GroupTopics] PRIMARY KEY CLUSTERED 
(
	[gt_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupsCategories]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupsCategories](
	[g_id] [int] NOT NULL,
	[c_id] [int] NOT NULL,
 CONSTRAINT [PK_GroupsCategories] PRIMARY KEY CLUSTERED 
(
	[g_id] ASC,
	[c_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Interests]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Interests](
	[i_id] [int] IDENTITY(1,1) NOT NULL,
	[i_from_username] [varchar](20) NOT NULL,
	[i_to_username] [varchar](20) NOT NULL,
	[i_date_posted] [datetime] NOT NULL,
	[i_deleted_by_fromuser] [bit] NOT NULL,
	[i_deleted_by_touser] [bit] NOT NULL,
 CONSTRAINT [PK_Interests] PRIMARY KEY CLUSTERED 
(
	[i_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[IsPaidMember]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[IsPaidMember]
(
	@Username varchar(20)
) AS
 IF EXISTS(
	SELECT
		 u_username
	FROM
		Users
	WHERE
		@Username = u_username AND u_paid_member = 1
	)
	SELECT  CAST(1 AS BIT)
ELSE
	SELECT CAST(0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[IsGroupNameUsed]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsGroupNameUsed] 
	(
		@Name	NVARCHAR(250)
	)
AS
	IF ( (SELECT count(g_name) FROM Groups WHERE g_name = @Name COLLATE SQL_Latin1_General_CP1_CS_AS) >0 )
			SELECT CAST (1 as BIT)
		ELSE
			SELECT CAST (0 as BIT)
GO
/****** Object:  StoredProcedure [dbo].[IsEmailUsed]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsEmailUsed]
(
	@Email	VARCHAR(250),
	@Username VARCHAR(20)
)
AS
	IF (@Username IS NULL)
	BEGIN
		IF ( (SELECT count(u_email) FROM Users WHERE u_email = @Email) >0 )
			SELECT CAST (1 as BIT)
		ELSE
			SELECT CAST (0 as BIT)
	END
	ELSE
	BEGIN
		IF EXISTS (SELECT u_username FROM Users WHERE u_email = @Email AND u_username <> @Username)
			SELECT CAST (1 as BIT)
		ELSE
			SELECT CAST (0 as BIT)
	END
GO
/****** Object:  Table [dbo].[Friends]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Friends](
	[u_username] [varchar](20) NOT NULL,
	[f_username] [varchar](20) NOT NULL,
	[f_timestamp] [datetime] NOT NULL,
	[f_accepted] [bit] NOT NULL,
 CONSTRAINT [PK_Friends] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC,
	[f_username] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupEvents]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupEvents](
	[ge_id] [int] IDENTITY(1,1) NOT NULL,
	[g_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ge_title] [nvarchar](200) NOT NULL,
	[ge_description] [ntext] NOT NULL,
	[ge_image] [image] NULL,
	[ge_date] [datetime] NOT NULL,
	[ge_location] [ntext] NOT NULL,
 CONSTRAINT [PK_GroupEvents] PRIMARY KEY CLUSTERED 
(
	[ge_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupBans]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupBans](
	[gb_id] [int] IDENTITY(1,1) NOT NULL,
	[g_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gb_expires] [datetime] NOT NULL,
	[gb_date] [datetime] NOT NULL,
 CONSTRAINT [PK_GroupBans] PRIMARY KEY CLUSTERED 
(
	[gb_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupIcon]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupIcon] 
	(
		@ID	INT
	)
AS
	SELECT
		g_icon	as Image
	FROM
		Groups
	WHERE
		g_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchGroups]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroups] 
	(
		@ID				INT,
		@Name			NVARCHAR(250),
		@Description	NVARCHAR(4000),
		@DateCreated	DATETIME,
		@Approved		BIT,
		@AccessLevel	INT,
		@Owner			VARCHAR(20),
		@ActiveMembers	INT,
		@NumberOfGroups INT,
		@SortColumn		INT
		
	)
AS

IF @NumberOfGroups IS NOT NULL
	SET ROWCOUNT @NumberOfGroups
	
SELECT
		g_id			AS ID,
		g_name			AS [Name],
		g_description	AS Description,
		g_datecreated	AS DateCreated,
		g_approved		AS Approved,
		g_accesslevel	AS AccessLevel,
		g_owner			AS Owner,
		g_activemembers AS ActiveMembers,
		g_jointerms		AS JoinTerms,
		g_joinquestion	AS JoinQuestion,
		g_permissions	AS [Permissions],
		g_minage		AS MinAge
		
		
FROM	Groups AS g

WHERE
		(@ID IS NULL OR g_id = @ID)
		AND (@Name IS NULL OR g_name = @Name)
		AND (@Description IS NULL OR g_description = @Description)
		AND (@DateCreated IS NULL OR g_datecreated = @DateCreated)
		AND (@Approved IS NULL OR g_approved = @Approved)
		AND (@AccessLevel IS NULL OR g_accesslevel = @AccessLevel)
		AND (@Owner IS NULL AND (EXISTS (SELECT 1 FROM Users WHERE Users.u_username = g.g_owner AND Users.u_deleted = 0) OR g_owner = @Owner))
		AND (@ActiveMembers IS NULL OR g_activemembers = @ActiveMembers)

ORDER BY
		CASE @SortColumn
			WHEN 1 THEN g_datecreated
		END DESC,
		
		CASE @SortColumn
			WHEN 2 THEN g_name
			WHEN 3 THEN g_owner
		END,
		
		CASE @SortColumn
			WHEN 4 THEN g_activemembers
		END DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchInactiveUsers]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchInactiveUsers]
(
	@ForTheLastXDays	int
)
 AS

SELECT u_username as Username
	FROM Users 
WHERE u_active = 0 AND 
	datediff(day,u_usersince,getdate()) > @ForTheLastXDays AND u_face_control_approved = 1
GO
/****** Object:  StoredProcedure [dbo].[FetchHourlyStatsForOnlineUsers]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchHourlyStatsForOnlineUsers]
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
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
	END
ELSE IF (@Type = 1) -- daily
	BEGIN
		SELECT	CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME) AS [Time],
				AVG(hs_usersonline) AS OnlineUsers
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME)
	END
ELSE IF (@Type = 2) -- weekly
	BEGIN
		SELECT	CAST( FLOOR( CAST( MIN(hs_time) AS FLOAT))AS DATETIME) AS [Time],
				AVG(hs_usersonline) AS OnlineUsers
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY datepart(year, hs_time), datepart(week, hs_time)
	END
ELSE IF (@Type = 3) -- monthly
	BEGIN
		SELECT  CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
					CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
					CAST(1 AS VARCHAR(2)) AS DATETIME) AS [Time],
		SUM(hs_usersonline) AS OnlineUsers
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
						CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
						CAST(1 AS VARCHAR(2)) AS DATETIME)
	END
GO
/****** Object:  StoredProcedure [dbo].[FetchHourlyStatsForNewUsers]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchHourlyStatsForNewUsers]
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
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
	END
ELSE IF (@Type = 1) -- daily
	BEGIN
		SELECT	CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME) AS [Time],
				SUM(hs_newusers) AS NewUsers
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST( FLOOR( CAST( hs_time AS FLOAT))AS DATETIME)
	END
ELSE IF (@Type = 2) -- weekly
	BEGIN
		SELECT	CAST( FLOOR( CAST( MIN(hs_time) AS FLOAT))AS DATETIME) AS [Time],
				SUM(hs_newusers) AS NewUsers
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY datepart(year, hs_time), datepart(week, hs_time)
	END
ELSE IF (@Type = 3) -- monthly
	BEGIN
		SELECT  CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
					CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
					CAST(1 AS VARCHAR(2)) AS DATETIME) AS [Time],
		SUM(hs_newusers) AS NewUsers
		FROM	HourlyStats
		WHERE	hs_time BETWEEN @From AND @To
		GROUP BY CAST(CAST(YEAR(hs_time) AS VARCHAR(4)) + '/' +
						CAST(MONTH(hs_time) AS VARCHAR(2)) + '/' +
						CAST(1 AS VARCHAR(2)) AS DATETIME)
	END
GO
/****** Object:  StoredProcedure [dbo].[FetchLanguages]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchLanguages]
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
/****** Object:  StoredProcedure [dbo].[FetchLanguage]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchLanguage] 
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
/****** Object:  StoredProcedure [dbo].[FetchDiscounts]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchDiscounts]
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
		Discounts
	WHERE (@ID = -1 OR @ID = d_id)
GO
/****** Object:  StoredProcedure [dbo].[FetchCreditsPackages]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchCreditsPackages]
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
		
FROM CreditsPackages

WHERE	(@ID IS NULL OR cp_id = @ID)
		AND (@Name IS NULL OR cp_id = @Name)
		AND (@Quantity IS NULL OR cp_id = @Quantity)
		AND (@Price IS NULL OR cp_id = @Price)
		
ORDER BY
		CASE WHEN @SortColumn = 1 THEN cp_price END
GO
/****** Object:  StoredProcedure [dbo].[FetchEmailTemplate]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchEmailTemplate]
(
	@name varchar(50)
)
 AS
	SELECT
		et_subject as Subject,
		et_body	    as Body
	FROM
		EmailTemplates
	WHERE
		et_name = @name
GO
/****** Object:  StoredProcedure [dbo].[FetchEmails]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchEmails]
 AS
	SELECT
		u_email as Email
	FROM
		Users
	WHERE
		u_active = 1 AND u_deleted = 0 AND u_face_control_approved = 1
GO
/****** Object:  StoredProcedure [dbo].[FetchEcardTypes]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchEcardTypes]
	(
		@ID INT,
		@Name NVARCHAR(200),
		@Type INT,
		@Active BIT,
		@SortColumn INT
	)
AS

SELECT	ect_id AS [ID],
		ect_name AS [Name],
		ect_type AS Type,
		ect_active AS Active
		
FROM	EcardTypes
WHERE	(@ID IS NULL OR ect_id = @ID)
		AND (@Name IS NULL OR ect_name = @Name)
		AND (@Type IS NULL OR ect_type = @Type)
		AND (@Active IS NULL OR ect_active = @Active)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ect_name END
GO
/****** Object:  StoredProcedure [dbo].[FetchEcardTypeContent]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchEcardTypeContent]
	(
		@ID INT
	)
AS

SELECT ect_content AS Content
FROM EcardTypes
WHERE ect_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchAffiliates]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAffiliates] 
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
		
FROM	Affiliates

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
GO
/****** Object:  StoredProcedure [dbo].[FetchBillingPlan]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchBillingPlan]
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
		BillingPlans
	WHERE
		(@ID IS NULL AND p_deleted = 0)
		OR 
		p_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchCategories]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchCategories]
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

FROM	Categories

WHERE	(@ID IS NULL OR c_id = @ID)
		AND (@Name IS NULL OR c_name = @Name)
		AND (@Order IS NULL OR c_order = @Order)
		
ORDER BY c_order
GO
/****** Object:  StoredProcedure [dbo].[FetchBillingPlanIDByPlanData]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchBillingPlanIDByPlanData]
(
	@Amount money,
	@Cycle int,
	@CycleUnit tinyint
) AS

	
SELECT TOP 1
	p_id
	FROM
		BillingPlans
	WHERE
		@Amount = p_amount AND 
		@Cycle = p_billing_cycle AND 
		@CycleUnit = p_billing_cycle_unit
	ORDER BY p_id DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchContentViews]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchContentViews]
( 
	@Key varchar(50),
	@Language INT
)

AS
	SELECT cv_key AS [Key],
		 cv_content AS [Content],
		 l_id as [Language]
	FROM ContentViews
	WHERE (@Key IS NULL OR @Key = cv_key) AND
		  (@Language IS NULL OR @Language = l_id)
GO
/****** Object:  StoredProcedure [dbo].[FetchChatUsers]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchChatUsers]
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
		cu_begintime as BeginTime,
		cu_activetime as ActiveTime,
		cu_ip as Ip
	FROM
		ChatUsers
	WHERE
		(@Id is null OR @Id = cu_id)
		AND (@Username is null OR @Username = cu_username)
		AND (@DisplayName is null OR @DisplayName = cu_displayname)
		AND (@Ip is null OR @Ip = cu_ip)
END
GO
/****** Object:  StoredProcedure [dbo].[FetchChatBans]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchChatBans]
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
		ChatBans
	WHERE
		(@Id is null OR @Id = cu_id)
		AND (@ChatRoomId is null or @ChatRoomId = cr_id)
		AND (@UserId is null or @UserId = cu_id)
		AND (@UserIp is null or @UserIp = cb_ip)
		AND (@ExpiresAfter is null or cb_dateexpires is null or @ExpiresAfter < cb_dateexpires)
END
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileTopic]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchProfileTopic]
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
			ProfileTopics
		ORDER BY
			pt_priority ASC
	ELSE
		SELECT
			pt_name	as Name,
			pt_priority	as Priority,
			pt_editcolumns	as EditColumns,
			pt_viewcolumns	as ViewColumns
		FROM
			ProfileTopics
		WHERE
			pt_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchSetting]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchSetting]
(
	@Key	varchar(80)
)
 AS
	SELECT s_value FROM Settings WHERE @Key = s_key
GO
/****** Object:  StoredProcedure [dbo].[FetchScheduledAnnouncement]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchScheduledAnnouncement]
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
/****** Object:  StoredProcedure [dbo].[FetchUserVote]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchUserVote]
(
	@FromUsername	VARCHAR(20),
	@ToUsername		VARCHAR(20)
)
AS

SELECT 
	v_score as Score
FROM 
	Votes 
WHERE
	v_fromusername = @FromUsername
	AND v_tousername = @ToUsername
GO
/****** Object:  StoredProcedure [dbo].[FetchVotesScore]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchVotesScore]
(
	@Username	VARCHAR(20)
)
AS

SELECT 
	SUM(v_score) as Score
FROM 
	Votes 
WHERE
	v_tousername = @Username
GO
/****** Object:  StoredProcedure [dbo].[FetchUserLevels]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchUserLevels]
(
	@Id		INT = NULL
)
AS
	SELECT
		ul_id	as Id,
		ul_name	as Name,
		ul_minscore	as MinScore,
		ul_restrictions	as Restrictions
	FROM
		UserLevels
	WHERE
		(@Id IS NULL OR ul_id = @Id)
	ORDER BY
		ul_minscore ASC
GO
/****** Object:  StoredProcedure [dbo].[FetchUserByGuid]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchUserByGuid]
	(
		@Guid VARCHAR(36)
	)
AS

SELECT pg_username AS Username
FROM PendingGuids
WHERE @Guid = pg_guid
GO
/****** Object:  Table [dbo].[GroupPhotos]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupPhotos](
	[gp_id] [int] IDENTITY(1,1) NOT NULL,
	[g_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gp_name] [nvarchar](80) NOT NULL,
	[gp_description] [nvarchar](200) NOT NULL,
	[gp_image] [image] NOT NULL,
	[gp_date] [datetime] NOT NULL,
 CONSTRAINT [PK_GroupPhotos] PRIMARY KEY CLUSTERED 
(
	[gp_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupMembers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupMembers](
	[g_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gm_type] [int] NOT NULL,
	[gm_joindate] [datetime] NOT NULL,
	[gm_invitedby] [varchar](20) NULL,
	[gm_active] [bit] NOT NULL,
	[gm_joinanswer] [ntext] NOT NULL,
	[gm_warned] [bit] NOT NULL,
	[gm_warn_reason] [nvarchar](200) NULL,
	[gm_warn_expirationdate] [datetime] NULL,
 CONSTRAINT [PK_GroupMembers] PRIMARY KEY CLUSTERED 
(
	[g_id] ASC,
	[u_username] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CommunityProfileApproval]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CommunityProfileApproval](
	[cpa_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[cpa_approvedby] [varchar](20) NOT NULL,
	[cpa_approved] [bit] NOT NULL,
	[cpa_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CommunityProfileApproval] PRIMARY KEY CLUSTERED 
(
	[cpa_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[ChangeProfileTopicOrder]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ChangeProfileTopicOrder]
(
	@ID 		int,
	@Direction 	varchar(4)
)
 AS
	DECLARE @priority int
	SET @priority = (SELECT pt_priority FROM ProfileTopics WHERE pt_id=@ID)
	IF (@Direction = 'up')
		BEGIN
			DECLARE @previous_priority int
			SET @previous_priority = (SELECT MAX(pt_priority) FROM ProfileTopics WHERE pt_priority <= @priority - 1)
			IF (@previous_priority IS NOT NULL)
				BEGIN
					UPDATE ProfileTopics 
					SET pt_priority = @priority
					WHERE pt_priority = @previous_priority
					UPDATE ProfileTopics
					SET pt_priority = @previous_priority
					WHERE pt_id = @ID
				END
		END
	ELSE
	IF (@Direction = 'down')
		BEGIN
			DECLARE @next_priority int
			SET @next_priority = (SELECT MIN(pt_priority) FROM ProfileTopics WHERE pt_priority >= @priority + 1)
			IF (@next_priority IS NOT NULL)
				BEGIN
					UPDATE ProfileTopics
					SET pt_priority = @priority
					WHERE pt_priority = @next_priority
					UPDATE ProfileTopics
					SET pt_priority = @next_priority
					WHERE pt_id = @ID
				END
		END
GO
/****** Object:  Table [dbo].[Blogs]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Blogs](
	[b_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[b_name] [nvarchar](200) NOT NULL,
	[b_description] [ntext] NOT NULL,
	[b_datecreated] [datetime] NOT NULL,
	[b_settings] [ntext] NULL,
 CONSTRAINT [PK_Blogs] PRIMARY KEY CLUSTERED 
(
	[b_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BlockedUsers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BlockedUsers](
	[bu_userblocker] [varchar](20) NOT NULL,
	[bu_blockeduser] [varchar](20) NOT NULL,
	[bu_blockdate] [datetime] NOT NULL,
 CONSTRAINT [PK_BlockedUsers] PRIMARY KEY CLUSTERED 
(
	[bu_userblocker] ASC,
	[bu_blockeduser] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AudioUploads]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AudioUploads](
	[au_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[au_title] [nvarchar](200) NOT NULL,
	[au_approved] [bit] NOT NULL,
	[au_private] [bit] NOT NULL,
 CONSTRAINT [PK_AudioUploads] PRIMARY KEY CLUSTERED 
(
	[au_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AudioAccess]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AudioAccess](
	[aa_audioowner] [varchar](20) NOT NULL,
	[aa_audioviewer] [varchar](20) NOT NULL,
	[aa_dateaccessgranted] [datetime] NOT NULL,
 CONSTRAINT [PK_AudioAccess] PRIMARY KEY CLUSTERED 
(
	[aa_audioowner] ASC,
	[aa_audioviewer] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AffiliatesHistory]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AffiliatesHistory](
	[ah_id] [int] IDENTITY(1,1) NOT NULL,
	[a_id] [int] NOT NULL,
	[ah_amount] [money] NOT NULL,
	[ah_date_paid] [datetime] NOT NULL,
	[ah_notes] [ntext] NULL,
	[ah_private_notes] [ntext] NULL,
 CONSTRAINT [PK_AffiliatesHistory] PRIMARY KEY CLUSTERED 
(
	[ah_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Ads]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ads](
	[a_id] [int] IDENTITY(1,1) NOT NULL,
	[ac_id] [int] NOT NULL,
	[a_postedby] [varchar](20) NOT NULL,
	[a_date] [datetime] NOT NULL,
	[a_expirationdate] [datetime] NOT NULL,
	[a_location] [nvarchar](200) NOT NULL,
	[a_subject] [nvarchar](200) NOT NULL,
	[a_description] [ntext] NOT NULL,
	[a_approved] [bit] NOT NULL,
 CONSTRAINT [PK_Ads] PRIMARY KEY CLUSTERED 
(
	[a_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Favourites]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Favourites](
	[u_username] [varchar](20) NOT NULL,
	[f_username] [varchar](20) NOT NULL,
	[f_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_Favourites] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC,
	[f_username] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[FetchAffiliateBanners]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAffiliateBanners] 
	(
		@ID			INT,
		@Name		NVARCHAR(200),
		@Deleted	BIT
	)
AS

SELECT	ab_id AS [ID],
		ab_name AS [Name],
		ab_deleted AS Deleted
		
FROM	AffiliateBanners

WHERE	(@ID IS NULL OR ab_id = @ID)
		AND (@Name IS NULL OR ab_name = @Name)
		AND (@Deleted IS NULL OR ab_deleted = @Deleted)
GO
/****** Object:  StoredProcedure [dbo].[FetchAffiliateBannerImage]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAffiliateBannerImage] 
	(
		@ID INT
	)
AS

SELECT ab_image AS [Image]
FROM AffiliateBanners
WHERE ab_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchAdsCategories]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAdsCategories]
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
FROM AdsCategories
WHERE	(@ID IS NULL OR ac_id = @ID)
		AND (@ParentID IS NULL OR ac_parentid = @ParentID)
		AND (@Title IS NULL OR ac_title = @Title)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ac_title END
GO
/****** Object:  StoredProcedure [dbo].[FetchAdmins]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchAdmins]
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
		Admins
	WHERE
		@Username IS null OR @Username = a_username
GO
/****** Object:  Table [dbo].[EstablishedCommunications]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EstablishedCommunications](
	[ec_from_username] [varchar](20) NOT NULL,
	[ec_to_username] [varchar](20) NOT NULL,
	[ec_date] [datetime] NOT NULL,
 CONSTRAINT [PK_EstablishedCommunications_1] PRIMARY KEY CLUSTERED 
(
	[ec_from_username] ASC,
	[ec_to_username] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteVideoUpload]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteVideoUpload]
	@Id	INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM
		VideoUploads
	WHERE
		vu_id = @Id
END
GO
/****** Object:  Table [dbo].[Events]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Events](
	[e_id] [int] IDENTITY(1,1) NOT NULL,
	[e_fromusername] [varchar](20) NULL,
	[e_fromgroup] [int] NULL,
	[e_type] [bigint] NOT NULL,
	[e_details] [nvarchar](4000) NULL,
	[e_date] [datetime] NOT NULL,
 CONSTRAINT [PK_Events] PRIMARY KEY CLUSTERED 
(
	[e_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Ecards]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Ecards](
	[ec_id] [int] IDENTITY(1,1) NOT NULL,
	[ect_id] [int] NOT NULL,
	[ec_from_username] [varchar](20) NOT NULL,
	[ec_to_username] [varchar](20) NOT NULL,
	[ec_date] [datetime] NOT NULL,
	[ec_message] [nvarchar](2000) NOT NULL,
	[ec_deleted_by_fromuser] [bit] NOT NULL,
	[ec_deleted_by_touser] [bit] NOT NULL,
	[ec_is_opened] [bit] NOT NULL,
 CONSTRAINT [PK_Ecards] PRIMARY KEY CLUSTERED 
(
	[ec_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteUserLevel]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteUserLevel] 
(
	@ID	INT
)
AS
BEGIN
	DELETE FROM UserLevels WHERE ul_id = @ID
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteUser]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteUser]
(
	@Username	VARCHAR(20),
	@DeleteReason	NVARCHAR(200)
)
 AS

UPDATE Users SET u_deleted = 1, u_deletereason = @DeleteReason WHERE  u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[DeleteScheduledAnnouncement]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteScheduledAnnouncement]
	(
		@ID int
	)
AS

DELETE FROM ScheduledAnnouncements
WHERE sa_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupIcon]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupIcon] 
	(
		@ID	INT
	)
AS

UPDATE Groups

SET g_icon = NULL

WHERE g_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteDiscount]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteDiscount]
(
	@ID	int
)
AS
	DELETE FROM Discounts WHERE d_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteCreditsPackage]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCreditsPackage]
	(
		@ID INT
	)
AS

DELETE FROM CreditsPackages WHERE cp_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteEmailQueue]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteEmailQueue]   
 (   
      @Id     int   
 )   
 AS   
    
 DELETE FROM EmailQueue WHERE eq_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[DeleteAdmin]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteAdmin]
(
	@Username	VARCHAR(20)
)
 AS

DELETE FROM Admins WHERE a_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[DeleteAffiliateBanner]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAffiliateBanner]
	(
		@ID INT
	)
AS

DELETE FROM AffiliateBanners
WHERE ab_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteAffiliate]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAffiliate] 
	(
		@ID		INT
	)
AS

UPDATE Affiliates
SET a_deleted = 1
WHERE a_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteBillingPlan]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteBillingPlan] 
(
	@ID int
)
AS
	UPDATE BillingPlans 
		SET p_deleted = 1 
		WHERE @ID = p_id
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Comments](
	[c_id] [int] IDENTITY(1,1) NOT NULL,
	[c_from_username] [varchar](20) NOT NULL,
	[c_to_username] [varchar](20) NOT NULL,
	[c_comment_text] [ntext] NOT NULL,
	[c_date_posted] [datetime] NOT NULL,
	[c_approved] [bit] NOT NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[c_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ContentPage]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ContentPage](
	[cp_id] [int] IDENTITY(1,1) NOT NULL,
	[cp_title] [nvarchar](100) NOT NULL,
	[cp_content] [ntext] NOT NULL,
	[cp_header_position] [int] NULL,
	[cp_footer_position] [int] NULL,
	[l_id] [int] NOT NULL,
	[cp_visiblefor] [int] NOT NULL,
	[cp_url] [varchar](250) NULL,
	[cp_metadescription] [nvarchar](200) NOT NULL,
	[cp_metakeyword] [nvarchar](200) NOT NULL,
	[cp_urlrewrite] [varchar](200) NULL,
 CONSTRAINT [PK_ContentPage] PRIMARY KEY CLUSTERED 
(
	[cp_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[CreatePendingGuid]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[CreatePendingGuid]
(
	@Username	varchar(20),
	@Guid		varchar(36)
)
 AS
	INSERT INTO PendingGuids
		(pg_username, pg_guid)
	VALUES
		(@Username, @Guid)
GO
/****** Object:  StoredProcedure [dbo].[CreateBillingPlan]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateBillingPlan]
(
	@Title nvarchar(200),
	@Amount money,
	@Cycle int,
	@CycleUnit TINYINT,
	@Options	text
)
 AS
	INSERT INTO BillingPlans
		(p_title, p_amount, p_billing_cycle, p_billing_cycle_unit, p_options)
	VALUES
		(@Title, @Amount, @Cycle, @CycleUnit, @Options)
GO
/****** Object:  StoredProcedure [dbo].[CreateAdmin]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateAdmin]
(
	@Username	VARCHAR(20),
	@Password	VARCHAR(250),
	@Privileges	text
)
AS
	INSERT INTO Admins
		(a_username, a_password,  a_privileges)
	VALUES
		(@Username, @Password, @Privileges)
GO
/****** Object:  StoredProcedure [dbo].[ChangeLanguageOrder]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ChangeLanguageOrder] 
	(
		@LanguageId int,
		@Direction varchar(4)
	)
AS

DECLARE @order int
	SET @order = (SELECT l_order FROM Languages WHERE l_id=@LanguageId)
	IF (@Direction = 'up')
		BEGIN
			DECLARE @previous_order int
			SET @previous_order = 
				(SELECT MAX(l_order) FROM Languages WHERE l_order <= @order - 1)
			IF (@previous_order IS NOT NULL)
				BEGIN
					UPDATE Languages
					SET l_order = @order
					WHERE l_order = @previous_order
					UPDATE Languages
					SET l_order = @previous_order
					WHERE l_id = @LanguageId
				END
		END
	ELSE
	IF (@Direction = 'down')
		BEGIN
		DECLARE @next_order int
			SET @next_order = 
				(SELECT MIN(l_order) FROM Languages WHERE l_order>= @order + 1)
			IF (@next_order IS NOT NULL)
				BEGIN
					UPDATE Languages 
					SET l_order = @order
					WHERE l_order = @next_order
					UPDATE Languages
					SET l_order = @next_order
					WHERE l_id = @LanguageId
				END
		END
GO
/****** Object:  StoredProcedure [dbo].[ChangeCategoryOrder]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ChangeCategoryOrder] 
	(
		@ID			INT,
		@Direction 	VARCHAR(4)
	)
AS
	DECLARE @priority int
	SET @priority = (SELECT c_order FROM Categories WHERE c_id=@ID)
	IF (@Direction = 'up')
		BEGIN
			DECLARE @previous_priority int
			SET @previous_priority = (SELECT MAX(c_order) FROM Categories WHERE c_order <= @priority - 1)
			IF (@previous_priority IS NOT NULL)
				BEGIN
					UPDATE Categories
					SET c_order = @priority
					WHERE c_order = @previous_priority
					UPDATE Categories
					SET c_order = @previous_priority
					WHERE c_id = @ID
				END
		END
	ELSE
	IF (@Direction = 'down')
		BEGIN
			DECLARE @next_priority int
			SET @next_priority = (SELECT MIN(c_order) FROM Categories WHERE c_order >= @priority + 1)
			IF (@next_priority IS NOT NULL)
				BEGIN
					UPDATE Categories
					SET c_order = @priority
					WHERE c_order = @next_priority
					UPDATE Categories
					SET c_order = @next_priority
					WHERE c_id = @ID
				END
		END
GO
/****** Object:  StoredProcedure [dbo].[BasicSearchAffiliate]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BasicSearchAffiliate] 
	(
		@Username		VARCHAR(20),
		@Name			NVARCHAR(100),
		@Email			VARCHAR(250),
		@SiteURL		VARCHAR(100),
		@Deleted		BIT,
		@Active			BIT,
		@RequestPayment BIT,
		@SortColumn	INT
	)
AS

SELECT a_id AS ID

FROM Affiliates

WHERE
	(@Username IS NULL OR a_username LIKE '%' + @Username + '%')
	AND (@Name IS NULL OR a_name LIKE '%' + @Name + '%')
	AND (@Email IS NULL OR a_email = '%' + @Email + '%')
	AND (@SiteURL IS NULL OR a_siteurl = '%' + @SiteURL + '%')
	AND (@Deleted IS NULL OR a_deleted = @Deleted)
	AND (@Active IS NULL OR a_active = @Active)
	AND (@RequestPayment IS NULL OR a_request_payment = @RequestPayment)
	
ORDER BY
	CASE WHEN @SortColumn = 1 THEN a_username END DESC,
	CASE WHEN @SortColumn = 2 THEN a_name END DESC,
	CASE WHEN @SortColumn = 3 THEN a_deleted END DESC,
	CASE WHEN @SortColumn = 4 THEN a_active END DESC,
	CASE WHEN @SortColumn = 5 THEN a_balance END DESC
GO
/****** Object:  StoredProcedure [dbo].[BirthdaySearchUsers]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BirthdaySearchUsers]
(
	@Birthdate datetime = getdate,
	@Gender     INT = NULL,
	@FromBirthdate	datetime,
	@ToBirthdate	datetime
)
AS
	SELECT
		u_username as Username
	FROM
		Users
	WHERE
		DAY(u_birthdate) = DAY(@Birthdate)
		AND MONTH(u_birthdate) = MONTH(@Birthdate)
		AND ((@Gender IS NULL) OR (u_gender = @Gender))
		AND (@FromBirthdate IS NULL OR (
				u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		AND (@ToBirthdate IS NULL OR (
				u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
		AND u_profilevisible = 1
		AND u_deleted = 0
		AND u_active = 1
		AND u_face_control_approved = 1
	ORDER BY
		u_lastonline DESC
GO
/****** Object:  Table [dbo].[CreditsHistory]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CreditsHistory](
	[ch_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ch_date] [datetime] NOT NULL,
	[ch_service] [int] NOT NULL,
	[ch_amount] [int] NOT NULL,
 CONSTRAINT [PK_CreditsHistory] PRIMARY KEY CLUSTERED 
(
	[ch_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[CreateUser]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateUser]
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
	@Country	VARCHAR(100),
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
GO
/****** Object:  StoredProcedure [dbo].[CreateSubscription]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateSubscription] 
(
	@Username varchar(20),
	@PlanID int,
	@PaymentProcessor varchar(50)
)
AS
	INSERT INTO Subscriptions
		(u_username, p_id, s_paymentprocessor)
	VALUES
		(@Username, @PlanID, @PaymentProcessor)
	SELECT @@IDENTITY
GO
/****** Object:  StoredProcedure [dbo].[BlockUser]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BlockUser] 
(
	@UserBlocker	varchar(20),
	@BlockedUser	varchar(20)
)
AS
	IF ((SELECT COUNT(*)  FROM BlockedUsers WHERE @UserBlocker = bu_userblocker AND @BlockedUser = bu_blockeduser) = 0)
		INSERT INTO BlockedUsers
			(bu_userblocker, bu_blockeduser)
		VALUES
			(@UserBlocker,@BlockedUser)
GO
/****** Object:  StoredProcedure [dbo].[BasicSearchGroup]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BasicSearchGroup] 
	(
		@CategoryID		INT,
		@Owner			VARCHAR(20),
		@Name			NVARCHAR(250),
		@DateCreated	DATETIME,
		@Approved		BIT,
		@AccessLevel	INT,
		@SortColumn		INT
	)
AS

SELECT g.g_id AS ID

FROM Groups AS g 

WHERE
	(@Owner IS NULL OR g.g_owner LIKE '%' + @Owner + '%')
	AND (@Name  IS NULL OR g.g_name LIKE '%' + @Name + '%')
	AND (@DateCreated IS NULL OR g.g_datecreated = @DateCreated)
	AND (@Approved IS NULL OR g.g_approved = @Approved)
	AND (@AccessLevel IS NULL OR g.g_accesslevel = @AccessLevel)
	AND (@CategoryID IS NULL OR EXISTS (SELECT c.c_id FROM GroupsCategories as c WHERE c.c_id = @CategoryID AND c.g_id = g.g_id))
	
ORDER BY
	CASE @SortColumn
		WHEN 1 THEN g.g_datecreated
	END DESC,
	CASE @SortColumn
		WHEN 2 THEN g.g_name		
		WHEN 3 THEN g.g_owner
	END,
	CASE @SortColumn	
		WHEN 4 THEN g.g_accesslevel
	END
GO
/****** Object:  StoredProcedure [dbo].[ConfirmSubscription]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ConfirmSubscription]
(
	@ID int
)
 AS
	UPDATE Subscriptions
	SET s_confirmed = 1
	WHERE s_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[ClearNewMessageFlags]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ClearNewMessageFlags]
(
	@Username	varchar(20)
)
AS
	UPDATE Messages SET m_new = 0
		WHERE m_to_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategoryFromGroups]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCategoryFromGroups] 
	(
		@CategoryID	INT
	)
AS

DELETE FROM GroupsCategories

WHERE c_id = @CategoryID
GO
/****** Object:  StoredProcedure [dbo].[DeleteCategory]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCategory] 
	(
		@ID INT
	)
AS

DELETE	FROM GroupsCategories
		
		WHERE c_id = @ID

DELETE	FROM Categories
		
		WHERE c_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteAudioUpload]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAudioUpload]
	(
		@Id	INT
	)
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM
		AudioUploads
	WHERE
		au_id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteComment]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteComment]
(
	@Id	INT
)
AS

DELETE FROM Comments
WHERE c_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[DeleteAdsCategory]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAdsCategory]
	(
		@ID INT
	)
AS

DELETE FROM Ads WHERE ac_id = @ID
DELETE FROM AdsCategories WHERE ac_parentid = @ID
DELETE FROM AdsCategories WHERE ac_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteAffiliateHistory]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAffiliateHistory] 
	(
		@ID				INT,
		@AffiliateID	INT
	)
AS

IF (@AffiliateID IS NULL)

DELETE FROM AffiliatesHistory
WHERE ah_id = @ID

ELSE

	DELETE FROM AffiliatesHistory
	WHERE a_id = @AffiliateID
GO
/****** Object:  StoredProcedure [dbo].[DeleteEcardType]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteEcardType]
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM Ecards WHERE ect_id = @ID

DELETE FROM EcardTypes WHERE	ect_id = @ID

COMMIT
GO
/****** Object:  StoredProcedure [dbo].[DeleteEcard]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteEcard]
	(
		@Id	INT,
		@DeletedByFromUser BIT=0,
		@DeletedByToUser BIT=0
	)
AS
IF (@DeletedByFromUser = 1)
UPDATE Ecards 
SET 	ec_deleted_by_fromuser = 1
WHERE
	ec_id = @Id

IF (@DeletedByToUser = 1)
UPDATE Ecards
SET 	ec_deleted_by_touser = 1
WHERE
	ec_id = @Id

IF EXISTS(SELECT ec_id FROM Ecards WHERE ec_deleted_by_fromuser = 1 AND ec_deleted_by_touser =1)
	DELETE FROM Ecards WHERE ec_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupBan]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupBan] 
	(
		@ID			INT,
		@GroupID	INT,
		@Username	VARCHAR(20)
	)
AS

DELETE FROM GroupBans WHERE
						(@ID IS NULL OR gb_id = @ID)
						AND (@GroupID IS NULL OR g_id = @GroupID)
						AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeleteCreditsHistory]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCreditsHistory]
	(
		@ID INT,
		@Username VARCHAR(20)
	)
AS

DELETE FROM CreditsHistory
WHERE	(@ID IS NULL OR ch_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeleteContentPage]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteContentPage]
(
	@ID AS INT
)
 AS

DELETE FROM ContentPage

	WHERE cp_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteCommunityProfileApproval]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCommunityProfileApproval]
	(
		@ID INT,
		@Username VARCHAR(20),
		@ApprovedBy VARCHAR(20)
	)
AS

DELETE FROM CommunityProfileApproval

WHERE	(@ID IS NULL OR cpa_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@ApprovedBy IS NULL OR cpa_approvedby = @ApprovedBy)
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupPhoto]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupPhoto]
	(
		@ID			INT,
		@GroupID	INT,
		@Username	VARCHAR(20)
	)
AS

DELETE FROM GroupPhotos

WHERE	(@ID IS NULL OR gp_id = @ID)
		AND (@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupMember]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupMember] 
	(
		@GroupID	INT,
		@Username	VARCHAR(20)
	)
AS

DELETE	FROM GroupMembers

		WHERE g_id = @GroupID AND u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupEventImage]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupEventImage] 
	(
		@ID	INT
	)
AS

UPDATE GroupEvents

SET ge_image = NULL

WHERE ge_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupFromCategories]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupFromCategories] 
	(
		@GroupID	INT
	)
AS

DELETE	FROM GroupsCategories

WHERE	g_id = @GroupID
GO
/****** Object:  StoredProcedure [dbo].[DeleteSavedSearch]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteSavedSearch]
(
	@ID INT
)
 AS

DELETE FROM SavedSearches

	WHERE ss_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteNews]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteNews]
(
	@ID int
)
 AS
	DELETE FROM News
	WHERE 
		n_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteMessagesSandBoxForLast24Hours]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteMessagesSandBoxForLast24Hours]
AS

DELETE FROM MessagesSandBox
WHERE DATEDIFF(HOUR, ms_datetime, GETDATE()) > 24
GO
/****** Object:  StoredProcedure [dbo].[DeleteMessagesSandBox]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteMessagesSandBox]
	(
		@Username VARCHAR(20),
		@Message NVARCHAR(100)
	)
AS

DELETE FROM MessagesSandBox
WHERE   (@Username IS NULL OR u_username = @Username)
		AND (@Message IS NULL OR ms_message = @Message)
GO
/****** Object:  StoredProcedure [dbo].[DeleteMessage]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteMessage]
(
	@Id	INT
)
AS

DELETE FROM Messages WHERE @Id = m_id
GO
/****** Object:  StoredProcedure [dbo].[DeleteLanguage]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteLanguage] 
	(
		@Id	int
	)
AS
-- =============== TODO: THIS PROCEDURE HAS TO DELETE -ONLY- NON-PREDEFINED LANGUAGES =======================
DELETE FROM	ContentPage	WHERE	l_id = @Id

DELETE FROM	News		WHERE	l_id = @Id

DELETE FROM	Translations WHERE	l_id = @Id

UPDATE Users			SET l_id = (SELECT TOP 1 l_id FROM Languages ORDER BY l_order) WHERE l_id = @Id

DELETE FROM Languages	WHERE	l_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[DeleteInterest]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteInterest]
(
	@Id	INT,
	@DeletedByFromUser BIT=0,
	@DeletedByToUser BIT=0
)
AS
IF (@DeletedByFromUser = 1)
UPDATE Interests 
SET 	i_deleted_by_fromuser = 1
WHERE
	i_id = @Id

IF (@DeletedByToUser = 1)
UPDATE Interests 
SET 	i_deleted_by_touser = 1
WHERE
	i_id = @Id

IF EXISTS(SELECT i_id FROM Interests WHERE i_deleted_by_fromuser = 1 AND i_deleted_by_touser =1)
	DELETE FROM Interests WHERE i_id = @Id
GO
/****** Object:  Table [dbo].[EventComments]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EventComments](
	[ec_id] [int] IDENTITY(1,1) NOT NULL,
	[e_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ec_date] [datetime] NOT NULL,
	[ec_comment] [nvarchar](2000) NOT NULL,
 CONSTRAINT [PK_EventComments] PRIMARY KEY CLUSTERED 
(
	[ec_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[DeleteVideoProfile]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteVideoProfile]
(
	@Username	VARCHAR(20)
)
 AS
	DELETE FROM VideoProfiles WHERE u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[DeleteVideoEmbed]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteVideoEmbed]
	@Id	INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM
		VideoEmbeds
	WHERE
		ve_id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[FetchActiveSubscription]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchActiveSubscription] 
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
/****** Object:  StoredProcedure [dbo].[DeleteEstablishedCommunication]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteEstablishedCommunication]
	(
	@FromUsername VARCHAR(20),
	@ToUsername VARCHAR(20)
	)
AS

DELETE FROM EstablishedCommunications WHERE ec_from_username = @FromUsername AND ec_to_username = @ToUsername
GO
/****** Object:  StoredProcedure [dbo].[FetchAds]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAds]
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
FROM	Ads
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
GO
/****** Object:  Table [dbo].[AdPhotos]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AdPhotos](
	[ap_id] [int] IDENTITY(1,1) NOT NULL,
	[a_id] [int] NOT NULL,
	[ap_description] [ntext] NOT NULL,
	[ap_image] [image] NOT NULL,
 CONSTRAINT [PK_AdPhotos] PRIMARY KEY CLUSTERED 
(
	[ap_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddUserScore]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddUserScore] 
(
	@Username	VARCHAR(20),
	@Score		INT,
	@Notes		NVARCHAR(200)
)
AS

	UPDATE Users
		SET u_score = u_score + @Score
		WHERE u_username = @Username

	INSERT INTO UserScoresLog (u_username, usl_score, usl_notes)
		VALUES (@Username, @Score, @Notes)
GO
/****** Object:  StoredProcedure [dbo].[AddToFriends]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddToFriends]
	(
	@Username	varchar(20),
	@Friend	varchar(20),
	@MaxFriendUsers int
)
AS
	IF EXISTS (SELECT 1 FROM Friends WHERE @Username = u_username AND @Friend = f_username)
		SELECT  2
	ELSE IF ( (SELECT COUNT(1)  FROM Friends WHERE @Username = u_username) >= @MaxFriendUsers)
		SELECT 3
	ELSE BEGIN
		INSERT INTO Friends
			(u_username, f_username)
		VALUES
			(@Username,@Friend)
	
		SELECT 1
	END
GO
/****** Object:  StoredProcedure [dbo].[AddToFavourites]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AddToFavourites] 
(
	@Username	varchar(20),
	@Favourite	varchar(20),
	@MaxFavouriteUsers int
)
AS
	IF ((SELECT COUNT(*)  FROM Favourites WHERE @Username = u_username AND @Favourite = f_username) <> 0)
		SELECT  2
	ELSE IF ( (SELECT COUNT(*)  FROM Favourites WHERE @Username = u_username) >= @MaxFavouriteUsers)
		SELECT 3
	ELSE BEGIN
		INSERT INTO Favourites
			(u_username, f_username)
		VALUES
			(@Username,@Favourite)
	
		SELECT 1
	END
GO
/****** Object:  Table [dbo].[AdComments]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AdComments](
	[ac_id] [int] IDENTITY(1,1) NOT NULL,
	[a_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ac_comment] [ntext] NOT NULL,
	[ac_date] [datetime] NOT NULL,
 CONSTRAINT [PK_AdComments] PRIMARY KEY CLUSTERED 
(
	[ac_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AffiliateCommissions]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AffiliateCommissions](
	[ac_id] [int] IDENTITY(1,1) NOT NULL,
	[a_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[ph_id] [int] NULL,
	[ac_timestamp] [datetime] NOT NULL,
	[ac_notes] [nvarchar](2000) NOT NULL,
	[ac_amount] [money] NOT NULL,
 CONSTRAINT [PK_AffiliateCommissions] PRIMARY KEY CLUSTERED 
(
	[ac_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[ApproveVideoProfile]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ApproveVideoProfile]
(
	@Username varchar(20)
)
 AS
UPDATE VideoProfiles SET vp_approved=1 WHERE u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[AssignRemoveGroupToFromCategory]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AssignRemoveGroupToFromCategory] 
	(
		@GroupID	INT,
		@CategoryID	INT,
		@Action		INT
	)
AS

IF @Action = 1	--assign

	BEGIN
--check is GroupID and CategoryID are valid and the pair doesn't exist in DB
		IF	EXISTS
			(SELECT g_name
			 FROM Groups
			 WHERE g_id = @GroupID)
			AND EXISTS
			(SELECT c_name
			 FROM Categories
			 WHERE c_id = @CategoryID)
			AND NOT EXISTS
			(SELECT * 
			 FROM GroupsCategories
			 WHERE g_id = @GroupID AND c_id = @CategoryID)
		INSERT INTO GroupsCategories
		(g_id, c_id)
		VALUES
		(@GroupID, @CategoryID)
			 
	END
			
ELSE			--remove	

	BEGIN
		DELETE FROM GroupsCategories
		WHERE g_id = @GroupID AND c_id = @CategoryID
	END
GO
/****** Object:  StoredProcedure [dbo].[BlockedSearchUsers]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BlockedSearchUsers] 
	(
		@Username	VARCHAR(20)
	)
AS

SELECT bu_blockeduser AS Username
FROM BlockedUsers
WHERE bu_userblocker = @Username
GO
/****** Object:  Table [dbo].[BlogPosts]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BlogPosts](
	[bp_id] [int] IDENTITY(1,1) NOT NULL,
	[b_id] [int] NOT NULL,
	[bp_title] [nvarchar](200) NOT NULL,
	[bp_content] [ntext] NOT NULL,
	[bp_dateposted] [datetime] NOT NULL,
	[bp_reads] [int] NOT NULL,
	[bp_approved] [bit] NOT NULL,
 CONSTRAINT [PK_BlogPosts] PRIMARY KEY CLUSTERED 
(
	[bp_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[ChangeProfileQuestionOrder]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ChangeProfileQuestionOrder]
(
	@TopicID	int,
	@QuestionID	int,
	@Direction	varchar(4)
)
 AS
	DECLARE @priority int
	SET @priority = (SELECT pq_priority FROM ProfileQuestions WHERE pq_id=@QuestionID)
	IF (@Direction = 'up')
		BEGIN
			DECLARE @previous_priority int
			SET @previous_priority = 
				(SELECT MAX(pq_priority) FROM ProfileQuestions WHERE pq_priority <= @priority - 1 AND @TopicID = pt_id)
			IF (@previous_priority IS NOT NULL)
				BEGIN
					UPDATE ProfileQuestions 
					SET pq_priority = @priority
					WHERE pq_priority = @previous_priority AND @TopicID = pt_id
					UPDATE ProfileQuestions
					SET pq_priority = @previous_priority
					WHERE pq_id = @QuestionID AND @TopicID = pt_id
				END
		END
	ELSE
	IF (@Direction = 'down')
		BEGIN
		DECLARE @next_priority int
			SET @next_priority = 
				(SELECT MIN(pq_priority) FROM ProfileQuestions WHERE pq_priority >= @priority + 1 AND @TopicID = pt_id)
			IF (@next_priority IS NOT NULL)
				BEGIN
					UPDATE ProfileQuestions 
					SET pq_priority = @priority
					WHERE pq_priority = @next_priority AND @TopicID = pt_id
					UPDATE ProfileQuestions
					SET pq_priority = @next_priority
					WHERE pq_id = @QuestionID AND @TopicID = pt_id
				END
		END
GO
/****** Object:  StoredProcedure [dbo].[CancelSubscription]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CancelSubscription]
(
	@ID int
)
 AS
	UPDATE Subscriptions
	SET s_cancelled = 1
	WHERE s_id = @ID
GO
/****** Object:  Table [dbo].[GroupEventsComments]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupEventsComments](
	[gec_id] [int] IDENTITY(1,1) NOT NULL,
	[ge_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gec_comment] [ntext] NOT NULL,
	[gec_date] [datetime] NOT NULL,
 CONSTRAINT [PK_GroupEventsComments] PRIMARY KEY CLUSTERED 
(
	[gec_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GroupEventsAttenders]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupEventsAttenders](
	[ge_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
 CONSTRAINT [PK_GroupEventsAttenders] PRIMARY KEY CLUSTERED 
(
	[ge_id] ASC,
	[u_username] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[FetchTranslationKeys]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchTranslationKeys] 
(
	@AdminPanel INT
	--@LanguageId	INT = 1
)
AS

SELECT 
	DISTINCT t_key as [Key]
FROM
	Translations
WHERE t_adminpanel = @AdminPanel
GO
/****** Object:  StoredProcedure [dbo].[FetchTranslation]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchTranslation] 
(
	@LanguageId	INT = 1,
	@Key		NVARCHAR(400),
	@AdminPanel INT
)
AS

DECLARE @Value NVARCHAR(400)

(SELECT @Value = t_value
			FROM
				Translations
			WHERE
				l_id = @LanguageId
				AND t_key = @Key
				AND t_adminpanel = @AdminPanel)
				
IF (@Value IS NULL)
BEGIN
	SELECT @Value = t_value
			FROM
				Translations
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
GO
/****** Object:  StoredProcedure [dbo].[FetchSubscription]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchSubscription] 
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
/****** Object:  UserDefinedFunction [dbo].[fnFetchAverageRating]    Script Date: 05/14/2010 18:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnFetchAverageRating] 
(
	@Username VARCHAR(20)
)
RETURNS FLOAT
AS
BEGIN

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
			Ratings
		WHERE
			r_tousername = @Username
			and r_ip != '0.0.0.0'
		GROUP BY r_ip
	) as AvgVotes

	SELECT
		@NoIPVoteCount = COUNT(r_rating),
		@AverageNoIPVote = AVG(CAST(r_rating as decimal))
	FROM
		Ratings
	WHERE
		r_tousername = @Username
		and r_ip = '0.0.0.0'

	IF (@AverageIPVote IS NULL) SET @AverageIPVote = 0
	IF (@AverageNoIPVote IS NULL) SET @AverageNoIPVote = 0
	IF (@IPVoteCount IS NULL) SET @IPVoteCount = 0
	IF (@NoIPVoteCount IS NULL) SET @NoIPVoteCount = 0
	IF (@IPVoteUniqueCount IS NULL) SET @IPVoteUniqueCount = 0

	DECLARE @AverageUserVote FLOAT
	IF (@IPVoteUniqueCount + @NoIPVoteCount = 0) 
		RETURN 0

	SET @AverageUserVote = 
		(@AverageIPVote * @IPVoteUniqueCount + @AverageNoIPVote * @NoIPVoteCount) 
			/ (@IPVoteUniqueCount + @NoIPVoteCount)

	-- Return the result of the function
	RETURN @AverageUserVote

END
GO
/****** Object:  StoredProcedure [dbo].[FetchVote]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchVote]
(
	@FromUser	VARCHAR(20),
	@ToUser	VARCHAR(20)
)
AS
	SELECT 
		r_rating as Vote
	FROM
		Ratings
	WHERE
		r_fromusername = @FromUser
		AND r_tousername = @ToUser
GO
/****** Object:  StoredProcedure [dbo].[FetchUsernamesWithFavoriteUser]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchUsernamesWithFavoriteUser]
	(
		@Username VARCHAR(20)
	)
AS

SELECT u_username AS Username
FROM Favourites
WHERE f_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[FetchUsernamesOfNonApprovedVideoProfiles]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchUsernamesOfNonApprovedVideoProfiles]
 AS
SELECT u_username as Username
 FROM VideoProfiles WHERE vp_approved=0
GO
/****** Object:  StoredProcedure [dbo].[FetchUserVerificationsCount]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchUserVerificationsCount]
(
	@VerifiedBy	VARCHAR(20),
	@VerifiedUser	VARCHAR(20)
)
AS
SELECT
	COUNT(vu_verifieduser)
FROM
	VerifiedUsers
WHERE
	vu_verifieduser = @VerifiedUser AND
	(@VerifiedBy IS NULL OR vu_verifiedby = @VerifiedBy)
GO
/****** Object:  StoredProcedure [dbo].[FetchRating]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchRating]
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
		Ratings
	WHERE
		r_tousername = @Username
		and r_ip != '0.0.0.0'
	GROUP BY r_ip
) as AvgVotes

SELECT
	@NoIPVoteCount = COUNT(r_rating),
	@AverageNoIPVote = AVG(CAST(r_rating as decimal))
FROM
	Ratings
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
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileViews]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchProfileViews]
(
	@ViewedUsername	VARCHAR(20),
	@FromDate		DATETIME = NULL
)
AS

SELECT TOP 100
	pv_viewer as ViewerUsername
FROM 
	ProfileViews
WHERE
	pv_viewed = @ViewedUsername
	AND (@FromDate IS NULL OR pv_timestamp >= @FromDate)
ORDER BY
	pv_timestamp DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileViewDate]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchProfileViewDate]
(
	@ViewerUsername	VARCHAR(20),
	@ViewedUsername	VARCHAR(20)
)
AS

SELECT TOP 1
	pv_timestamp as [Timestamp]
FROM 
	ProfileViews
WHERE
	pv_viewed = @ViewedUsername
	AND pv_viewer = @ViewerUsername
ORDER BY
	pv_timestamp DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileView]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchProfileView]
(
	@ViewerUsername	VARCHAR(20),
	@ViewedUsername	VARCHAR(20)
)
AS

SELECT TOP 1
	pv_timestamp as [Timestamp]
FROM 
	ProfileViews
WHERE
	pv_viewed = @ViewedUsername
	AND pv_viewer = @ViewerUsername
ORDER BY
	pv_timestamp DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchNumberOfUpcomingEvents]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNumberOfUpcomingEvents]
	(
		@Username	VARCHAR(20)
	)
AS

SELECT ge.g_id as GroupID, g_name as [Name], count(ge_id) as [Count] FROM GroupEvents ge
	JOIN GroupMembers gm ON ge.g_id = gm.g_id
	JOIN Groups	g ON ge.g_id = g.g_id
WHERE gm.u_username = @Username AND ge_date >= getdate() and ge_date <= dateadd(day,7,getdate())
Group By ge.g_id, g_name
GO
/****** Object:  StoredProcedure [dbo].[FetchPhotoAlbums]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPhotoAlbums]
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
		
FROM	PhotoAlbums

WHERE	(@ID IS NULL OR pa_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Name IS NULL OR pa_name = @Name)
		AND (@Access IS NULL OR pa_access = @Access)
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileQuestionByTopic]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchProfileQuestionByTopic]
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
		ProfileQuestions
	WHERE
		pt_id = @TopicID
	ORDER BY
		pq_priority ASC
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileQuestion]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchProfileQuestion]
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
			ProfileQuestions
		WHERE
			pq_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchComments]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchComments]
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
	Comments
WHERE
	(@Id IS NULL OR @Id = -1 OR c_id = @Id)
	AND (@FromUsername IS NULL OR c_from_username = @FromUsername)
	AND (@ToUsername IS NULL OR c_to_username = @ToUsername)
	AND (@Approved IS NULL OR c_approved = @Approved)
ORDER BY
	c_date_posted DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchBlockedUsers]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchBlockedUsers]
	(
		@Username VARCHAR(20)
	)
AS

SELECT	bu_blockeduser AS BlockedUser,
		bu_blockdate AS BlockedOn
FROM BlockedUsers
WHERE bu_userblocker = @Username
GO
/****** Object:  StoredProcedure [dbo].[FetchCategoriesByGroup]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchCategoriesByGroup]
	(
		@GroupID	INT
	)
AS

SELECT	
		c.c_id AS ID,
		c.c_name AS [Name],
		c.c_order AS [Order],
		c.c_userscancreategroups AS UsersCanCreateGroups

FROM Categories AS c INNER JOIN GroupsCategories AS gc
			ON gc.c_id = c.c_id
		WHERE gc.g_id = @GroupID
GO
/****** Object:  StoredProcedure [dbo].[FetchContentPages]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchContentPages]
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

FROM ContentPage

WHERE (@ID IS NULL OR @ID = cp_id) 
	AND (@Language IS NULL OR @Language = l_id)

ORDER BY
		CASE WHEN @SortColumn = 1 THEN cp_header_position END, cp_footer_position,
		CASE WHEN @SortColumn = 2 THEN cp_footer_position END, cp_header_position
GO
/****** Object:  StoredProcedure [dbo].[FetchCommunityProfileApproval]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchCommunityProfileApproval]
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
		
FROM CommunityProfileApproval

WHERE	(@ID IS NULL OR cpa_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@ApprovedBy IS NULL OR cpa_approvedby = @ApprovedBy)
		AND (@FromDate IS NULL OR cpa_timestamp >= @FromDate) AND (@ToDate IS NULL OR cpa_timestamp <= @ToDate)

ORDER BY

CASE WHEN @SortColumn = 1 THEN u_username END,
CASE WHEN @SortColumn = 2 THEN cpa_approvedby END,
CASE WHEN @SortColumn = 3 THEN cpa_timestamp END DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchBillingPlanBySubscriptionID]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchBillingPlanBySubscriptionID]
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
		BillingPlans, Subscriptions
	WHERE
		Subscriptions.s_id = @SubscriptionID AND  BillingPlans.p_id = Subscriptions.p_id
GO
/****** Object:  StoredProcedure [dbo].[FetchAverageVote]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAverageVote]
(
	@FromUsername	VARCHAR(20)
)
AS
SELECT
	COUNT(r_rating) as Votes,
	AVG(CAST(r_rating as decimal)) as AverageVote
FROM
	Ratings
WHERE
	r_fromusername = @FromUsername
GO
/****** Object:  StoredProcedure [dbo].[FetchAffiliatesHistory]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAffiliatesHistory] 
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

FROM	AffiliatesHistory

WHERE	(@ID IS NULL OR ah_id = @ID)
		AND (@AffiliateID IS NULL OR a_id = @AffiliateID)
		AND (@Amount IS NULL OR ah_amount = @Amount)
		AND (@DatePaid IS NULL OR ah_date_paid = @DatePaid)
		
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ah_amount END,
		CASE WHEN @SortColumn = 2 THEN ah_date_paid END DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchEcards]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchEcards]
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
GO
/****** Object:  StoredProcedure [dbo].[FetchCreditsHistoryByUsername]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchCreditsHistoryByUsername]
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
GO
/****** Object:  StoredProcedure [dbo].[FetchCreditsHistory]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchCreditsHistory]
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

FROM CreditsHistory

WHERE	(@ID IS NULL OR ch_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Date IS NULL OR ch_date = @Date)

ORDER BY
	CASE WHEN @SortColumn = 1 THEN ch_date END DESC,
	CASE WHEN @SortColumn = 2 THEN u_username END
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupEvents]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupEvents] 
	(
		@ID						INT,
		@GroupID				INT,
		@Username				VARCHAR(20),
		@FromDate				DATETIME,
		@ToDate					DATETIME,
		@NumberOfGroupEvents	INT,
		@SortColumn				INT
	)
AS

IF @NumberOfGroupEvents IS NOT NULL
	SET ROWCOUNT @NumberOfGroupEvents
	
SELECT	ge_id AS ID,
		g_id AS GroupID,
		u_username AS Username,
		ge_title AS Title,
		ge_description AS Description,
		ge_date AS [Date],
		ge_location AS Location

FROM GroupEvents

WHERE	(@ID IS NULL OR ge_id = @ID)
		AND (@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@FromDate IS NULL OR ge_date >= @FromDate) 
		AND (@ToDate IS NULL OR ge_date <= @ToDate) 

ORDER BY
	CASE WHEN @SortColumn = 1 THEN g_id END,
	CASE WHEN @SortColumn = 2 THEN u_username END,
	CASE WHEN @SortColumn = 3 THEN ge_date END
	
SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupEventImage]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupEventImage] 
	(
		@GroupEventID	INT
	)
AS
	SELECT
		ge_image AS [Image]
	FROM
		GroupEvents
	WHERE
		ge_id = @GroupEventID
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupBans]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupBans] 
	(
		@ID			INT,
		@GroupID	INT,
		@Username	VARCHAR(20),
		@Date		DATETIME
	)
AS

SELECT	gb_id AS ID,
		g_id AS GroupID,
		u_username AS Username,
		gb_date AS [Date],
		gb_expires AS Expires

FROM	GroupBans

WHERE	(@ID IS NULL OR gb_id = @ID)
		AND (@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Date IS NULL OR gb_date = @Date)
		AND gb_expires > GETDATE()
GO
/****** Object:  StoredProcedure [dbo].[FetchFriendTimeStamp]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchFriendTimeStamp]
	(
		@Username	varchar(20),
		@Friend	varchar(20)
	)
 AS
	SELECT f_timestamp FROM Friends WHERE @Username = u_username AND @Friend = f_username
GO
/****** Object:  StoredProcedure [dbo].[FetchFriends]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchFriends]
	(
		@Username varchar(20)
	)
AS
	SELECT f_username as Friend FROM Friends, Users
		WHERE @Username = Friends.u_username AND
			Friends.f_username = Users.u_username AND
			 Users.u_deleted = 0
GO
/****** Object:  StoredProcedure [dbo].[FetchFavouriteTimeStamp]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchFavouriteTimeStamp]
(
	@Username	varchar(20),
	@Favourite	varchar(20)
)
 AS
	SELECT f_timestamp FROM Favourites WHERE @Username = u_username AND @Favourite = f_username
GO
/****** Object:  StoredProcedure [dbo].[FetchFavourites]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchFavourites]
(
	@Username varchar(20)
)
 AS
	SELECT f_username as Favourite FROM Favourites, Users
		WHERE @Username = Favourites.u_username AND
			Favourites.f_username = Users.u_username AND
			 Users.u_deleted = 0
GO
/****** Object:  StoredProcedure [dbo].[FetchNewTopicsCountByGroups]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNewTopicsCountByGroups] 
	(
		@Username	VARCHAR(20)
	)
AS

DECLARE @LastOnline datetime

SELECT @LastOnline = u_prevlogin FROM Users where u_username = @Username

SELECT gt.g_id as GroupID, g_name as [Name], count(gt_id) as [Count] FROM GroupTopics gt
	JOIN GroupMembers gm ON gt.g_id = gm.g_id
	JOIN Groups	g ON gt.g_id = g.g_id
WHERE gm.u_username = @Username AND gt_datecreated > @LastOnline
Group By gt.g_id, g_name
GO
/****** Object:  StoredProcedure [dbo].[FetchNewTopics]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNewTopics]
	(
		@Username	VARCHAR(20)
	)
AS

DECLARE @LastOnline datetime

SELECT @LastOnline = u_prevlogin FROM Users where u_username = @Username

SELECT gt_id AS GroupTopicID
FROM GroupTopics gt
	JOIN GroupMembers gm ON gt.g_id = gm.g_id
	--JOIN Groups	g ON gt.g_id = g.g_id
WHERE gm.u_username = @Username AND gt_datecreated > @LastOnline
GO
/****** Object:  StoredProcedure [dbo].[FetchNews]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNews]
( 
	@ID INT,
	@NewsCount INT,
	@LanguageID INT
)
AS

SET ROWCOUNT @NewsCount

	IF (	@ID <> -1 AND 
		NOT EXISTS(SELECT n_id FROM News WHERE @ID = n_id AND @LanguageID = l_id) AND 
		EXISTS (SELECT n_id FROM News WHERE @LanguageID = l_id)
	    )
		SELECT TOP 1 
			n_id as NID,
			n_date as Date,
			n_text as Text,
			n_title as Title
		FROM
			News
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
			News
	
		WHERE 
			(@ID = -1 OR @ID = n_id) AND @LanguageID = l_id
		ORDER BY
			n_date DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchInterests]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchInterests]
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

	FROM Interests
	WHERE (@Id = 0 OR @Id = i_id)
		AND (@FromUsername IS NULL OR i_deleted_by_fromuser = 0)
		AND (@ToUsername IS NULL OR i_deleted_by_touser = 0)
		AND (@FromUsername IS NULL OR 
				(@FromUsername = i_from_username))
		AND (@ToUsername IS NULL OR 
				(@NewInterests = 0 AND @ToUsername = i_to_username)  
			OR
				(@NewInterests = 1 AND @ToUsername =  i_to_username AND
					EXISTS (SELECT u_username FROM Users WHERE u_username = @ToUsername AND i_date_posted > u_prevlogin)))
	ORDER BY i_date_posted DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchEstablishedCommunications]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchEstablishedCommunications]
	(
		@FromUsername VARCHAR(20),
		@ToUsername VARCHAR(20),
		@Date DATETIME
	)
AS

SELECT	ec_from_username AS FromUsername,
		ec_to_username AS ToUsername,
		ec_date AS [Date]
		
FROM EstablishedCommunications
		
WHERE	(@FromUsername IS NULL OR ec_from_username = @FromUsername)
		AND (@ToUsername IS NULL OR ec_to_username = @ToUsername)
		AND (@Date IS NULL OR ec_date = @Date)
GO
/****** Object:  StoredProcedure [dbo].[FetchNewCommentsCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNewCommentsCount]
	(
		@Username	VARCHAR(20)
	)
AS
DECLARE @LastOnline datetime

SELECT @LastOnline = u_prevlogin FROM Users where u_username = @Username

SELECT COUNT(1) AS Comments
FROM Comments
WHERE c_to_username = @Username AND c_date_posted > @LastOnline
GO
/****** Object:  StoredProcedure [dbo].[FetchMutuallyFriends]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchMutuallyFriends]
	(
		@Username varchar(20)
	)
 AS

	SELECT 
		f_username as Friend 
	FROM 
		Friends as F1

	WHERE u_username = @Username
		AND EXISTS (SELECT u_username FROM Users WHERE Users.u_username = F1.f_username
					AND Users.u_deleted = 0 
					AND Users.u_profilevisible = 1)
		AND EXISTS (SELECT f_username FROM Friends as F2 WHERE F2.f_username = @Username
					AND F2.u_username = F1.f_username)
GO
/****** Object:  StoredProcedure [dbo].[FetchMutuallyFavourites]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchMutuallyFavourites]
(
	@Username varchar(20)
)
 AS

	SELECT 
		f_username as Favourite 
	FROM 
		Favourites as F1

	WHERE u_username = @Username
		AND EXISTS (SELECT u_username FROM Users WHERE Users.u_username = F1.f_username
					AND Users.u_deleted = 0 
					AND Users.u_profilevisible = 1)
		AND EXISTS (SELECT f_username FROM Favourites as F2 WHERE F2.f_username = @Username
					AND F2.u_username = F1.f_username)
GO
/****** Object:  StoredProcedure [dbo].[FetchMessagesSandBox]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchMessagesSandBox]
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
FROM MessagesSandBox
WHERE	(@Username IS NULL OR u_username = @Username)
		AND (@Message IS NULL OR ms_message = @Message)
		AND (@FromDate IS NULL OR ms_datetime >= @FromDate)
		AND (@ToDate IS NULL OR ms_datetime <= @ToDate)
		AND  ms_count > @MaxSameMessage
ORDER BY ms_count DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchMessagesCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchMessagesCount]
	(
		@From DATETIME,
		@To DATETIME
	)

AS

SELECT COUNT(1)
FROM Messages
WHERE m_timestamp BETWEEN @From AND @To
GO
/****** Object:  StoredProcedure [dbo].[FetchMessages]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchMessages]
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
	
	FROM Messages
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
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupTopicsCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupTopicsCount] 
	(
		@GroupID	INT,
		@Username	VARCHAR(20)
	)
AS

SELECT	COUNT(gt_id)

FROM	GroupTopics

WHERE	(@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupTopics]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupTopics]
	(
		@ID				INT,
		@GroupID		INT,
		@Username		VARCHAR(20),
		@Name			NVARCHAR(200),
		@Locked			BIT,
		@StickyUntil	DATETIME,
		@NumberOfTopics INT
	)
AS

IF @NumberOfTopics IS NOT NULL
	SET ROWCOUNT @NumberOfTopics
	
SELECT
		gt_id AS ID,
		g_id AS GroupID,
		u_username AS Username,
		gt_name AS [Name],
		gt_datecreated AS DateCreated,
		gt_dateupdated AS DateUpdated,
		gt_posts AS Posts,
		gt_locked AS Locked,
		gt_stickyuntil AS StickyUntil,
		gt_poll AS IsPoll,
		gt_views AS Views
		
FROM GroupTopics

WHERE
		(@ID IS NULL OR gt_id = @ID)
		AND (@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Name IS NULL OR gt_name = @Name)
		AND (@Locked IS NULL OR gt_locked = @Locked)
		AND (@StickyUntil IS NULL OR gt_stickyuntil <= @StickyUntil)
		
ORDER BY 
	CASE WHEN gt_stickyuntil > getdate() THEN gt_stickyuntil
	END DESC,
	gt_dateupdated DESC
	
SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupsPendingInvitations]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupsPendingInvitations] 
	(
		@Username	VARCHAR(20)
	)
AS

SELECT	COUNT(1)

FROM	GroupMembers 

--AS gm
--INNER JOIN Groups AS g
--ON gm.g_id = g.g_id

-- the group is private and the specified member is not active
--WHERE	g.g_accesslevel = 3 AND gm.u_username = @Username AND gm.gm_active = 0

-- modification because of public and moderated group can invite also
WHERE gm_invitedby IS NOT NULL AND u_username = @Username AND gm_active = 0
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupsByUsername]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupsByUsername] 
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
/****** Object:  StoredProcedure [dbo].[FetchGroupsByCategoryCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupsByCategoryCount] 
	(
		@CategoryID	INT,
		@Approved	BIT
	)
AS

SELECT COUNT(g.g_id)

FROM GroupsCategories AS gc INNER JOIN Groups AS g 
									ON gc.g_id = g.g_id
									
WHERE gc.c_id = @CategoryID AND (@Approved IS NULL OR g.g_approved = @Approved)
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupsByCategory]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupsByCategory] 
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
/****** Object:  StoredProcedure [dbo].[FetchGroupPhotosCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupPhotosCount] 
	(
		@GroupID	INT,
		@Username	VARCHAR(20)
	)
AS

SELECT	COUNT(gp_id)

FROM	GroupPhotos

WHERE	(@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupPhotos]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupPhotos] 
	(
		@ID					INT,
		@GroupID			INT,
		@Username			VARCHAR(20),
		@Name				NVARCHAR(80),
		@Date				DATETIME,
		@NumberOfPhotos		INT,
		@SortColumn			INT		
	)
AS

IF @NumberOfPhotos IS NOT NULL
	SET ROWCOUNT @NumberOfPhotos
	
SELECT	gp_id AS ID,
		g_id AS GroupID,
		u_username AS Username,
		gp_name AS [Name],
		gp_description AS Description,
		gp_date AS [Date]

FROM	GroupPhotos

WHERE	(@ID IS NULL OR gp_id = @ID)
		AND (@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Name IS NULL OR gp_name = @Name)
		AND (@Date IS NULL OR u_username = @Date)
		
ORDER BY
		CASE WHEN @SortColumn = 1 THEN gp_date END DESC
		
SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupPhoto]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupPhoto] 
	(
		@ID	INT
	)
AS

SELECT gp_image AS [Image]

FROM GroupPhotos

WHERE gp_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupMembersCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupMembersCount] 
	(
		@GroupID	INT,
		@Type		INT,
		@Active		BIT
	)
AS

SELECT COUNT(g_id)

FROM GroupMembers
					
WHERE (@GroupID IS NULL OR g_id = @GroupID)
AND (@Type IS NULL OR gm_type = @Type)
AND (@Active IS NULL OR gm_active = @Active)
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupMembers]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupMembers] 
	(
		@GroupID			INT,
		@Username			VARCHAR(20),
		@Type				INT,
		@Active				BIT,
		@JoinDate			DATETIME,
		@InvitedBy			VARCHAR(20),
		@NumberOfMembers	INT,
		@SortColumn			INT
	)
AS

IF @NumberOfMembers IS NOT NULL
	SET ROWCOUNT @NumberOfMembers

SELECT
	g_id		 AS GroupID,
	u_username	 AS Username,
	gm_type		 AS Type,
	gm_active	 AS Active,
	gm_joindate  AS JoinDate,
	gm_invitedby AS InvitedBy,
	gm_joinanswer AS JoinAnswer,
	gm_warned AS IsWarned,
	gm_warn_reason AS WarnReason,
	gm_warn_expirationdate AS WarnExpirationDate

FROM	GroupMembers

WHERE	(@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Type IS NULL OR gm_type = @Type)
		AND (@Active IS NULL OR gm_active = @Active)
		AND (@JoinDate IS NULL OR gm_joindate = @JoinDate)
		AND (@InvitedBy IS NULL OR gm_invitedby = @InvitedBy)
		
ORDER BY
	CASE WHEN @SortColumn = 1 THEN gm_joindate
	END DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupInvitationsCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupInvitationsCount] 
	(
		@Username	VARCHAR(20),
		@FromDate	DATETIME
	)
AS

SELECT COUNT(g_id) FROM GroupMembers WHERE gm_invitedby = @Username AND gm_joindate >= @FromDate
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupEventsCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupEventsCount]
	(
		@GroupID	INT,
		@FromDate	DATETIME,
		@ToDate		DATETIME
	)
AS

SELECT COUNT(1)
FROM GroupEvents
WHERE	(@GroupID IS NULL OR g_id = @GroupID) AND
		(@FromDate IS NULL OR ge_date >= @FromDate) AND
		(@ToDate IS NULL OR ge_date <= @ToDate)
GO
/****** Object:  Table [dbo].[GroupPollsChoices]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GroupPollsChoices](
	[gpc_id] [int] IDENTITY(1,1) NOT NULL,
	[gt_id] [int] NOT NULL,
	[gpc_answer] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_GroupPollsChoices] PRIMARY KEY CLUSTERED 
(
	[gpc_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Trigger [friends_ins_trg]    Script Date: 05/14/2010 18:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[friends_ins_trg]

ON [dbo].[Friends]

AFTER INSERT

AS

DECLARE @Username1 VARCHAR(20) 
SELECT @Username1 = u_username FROM inserted

DECLARE @Username2 VARCHAR(20) 
SELECT @Username2 = f_username FROM inserted

IF EXISTS (SELECT 1 FROM Friends WHERE u_username = @Username2 AND f_username = @Username1)

	UPDATE	Friends
	SET		f_accepted = 1
	WHERE	(u_username = @Username1 AND f_username = @Username2)
			OR (u_username = @Username2 AND f_username = @Username1)
GO
/****** Object:  Trigger [friends_del_trg]    Script Date: 05/14/2010 18:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[friends_del_trg]

ON [dbo].[Friends]

FOR DELETE

AS

DECLARE @Username1 VARCHAR(20)
SELECT @Username1 = u_username FROM DELETED

DECLARE @Username2 VARCHAR(20)
SELECT @Username2 = f_username FROM DELETED

UPDATE Friends
SET f_accepted = 0
WHERE u_username = @Username2 AND f_username = @Username1
GO
/****** Object:  StoredProcedure [dbo].[HasPrivateAudio]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HasPrivateAudio]
	(
		@Username varchar(20)
	)
 AS
	IF EXISTS(SELECT au_id FROM AudioUploads WHERE @Username = u_username AND au_private = 1)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[GroupUsersSearch]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GroupUsersSearch] 
	(
		@GroupID		INT,
		@Gender			INT,
		@FromBirthdate	DATETIME,
		@ToBirthdate	DATETIME,
		@Type			INT,
		@Active			BIT,
		@JoinDate		DATETIME,
		@SortColumn		INT
	)
AS

SELECT	u.u_username AS Username

FROM GroupMembers AS gm INNER JOIN Users as u
						ON gm.u_username = u.u_username

WHERE	(@GroupID IS NULL OR gm.g_id = @GroupID)
		AND (@Gender IS NULL OR @Gender = -1 OR u.u_gender = @Gender)
		AND (@FromBirthdate IS NULL OR (
				u.u_birthdate >= @FromBirthdate AND (u.u_gender != 3 OR u.u_birthdate2 >= @FromBirthdate)))
		AND (@ToBirthdate IS NULL OR (
				u.u_birthdate <= @ToBirthdate AND (u.u_gender != 3 OR u.u_birthdate2 <= @ToBirthdate)))
		AND (u.u_deleted = 0)
		AND (@Type IS NULL OR @Type = -1 OR gm.gm_type = @Type)
		AND (@Active IS NULL OR gm.gm_active = @Active)
		AND (@JoinDate IS NULL OR gm.gm_joindate = @JoinDate)
				
ORDER BY
		CASE
			WHEN @SortColumn = 0 THEN gm.gm_joindate
			WHEN @SortColumn = 1 THEN u.u_lastonline
		END DESC
GO
/****** Object:  Table [dbo].[GroupTopicSubscriptions]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupTopicSubscriptions](
	[gts_id] [int] IDENTITY(1,1) NOT NULL,
	[gt_id] [int] NOT NULL,
	[g_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gts_dateupdated] [datetime] NOT NULL,
 CONSTRAINT [PK_GroupTopicSubscriptions] PRIMARY KEY CLUSTERED 
(
	[gts_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[HasVideoProfile]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[HasVideoProfile]
(
	@Username	VARCHAR(20)
)
 AS

IF EXISTS (SELECT u_username FROM VideoProfiles WHERE u_username = @Username AND vp_approved = 1)
	SELECT CAST (1 AS BIT)
ELSE
	SELECT CAST (0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[HasUserAccessToPrivateVideo]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HasUserAccessToPrivateVideo]
(
	@VideoOwner varchar(20),
	@VideoViewer varchar(20)
)
 AS
	IF EXISTS (SELECT va_videoowner  FROM VideoAccess WHERE @VideoOwner = va_videoowner AND @VideoViewer = va_videoviewer)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[HasUserAccessToPrivatePhotos]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[HasUserAccessToPrivatePhotos]
(
	@PhotoOwner varchar(20),
	@PhotoViewer varchar(20)
)
 AS
	IF EXISTS (SELECT pa_photoowner  FROM PhotoAccess WHERE @PhotoOwner = pa_photoowner AND @PhotoViewer = pa_photoviewer)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[HasUserAccessToPrivateAudio]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[HasUserAccessToPrivateAudio]
	(
		@AudioOwner varchar(20),
		@AudioViewer varchar(20)
	)
 AS
	IF EXISTS (SELECT aa_audioowner  FROM AudioAccess WHERE @AudioOwner = aa_audioowner AND @AudioViewer = aa_audioviewer)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[MutualFriendsSearch]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MutualFriendsSearch]
	(
		@Viewer VARCHAR(20),
		@Viewed VARCHAR(20)
	)
AS

SELECT 
		f_username as Friend 
	FROM 
		Friends as F1

	WHERE u_username = @Viewer
		AND EXISTS (SELECT u_username FROM Users WHERE Users.u_username = F1.f_username
					AND Users.u_deleted = 0 
					AND Users.u_profilevisible = 1)
		AND EXISTS (SELECT f_username FROM Friends as F2 WHERE F2.f_username = @Viewer
					AND F2.u_username = F1.f_username) 
		AND f_username IN 				
							(SELECT 
								f_username as Friend 
							FROM 
								Friends as F1

							WHERE u_username = @Viewed
								AND EXISTS (SELECT u_username FROM Users WHERE Users.u_username = F1.f_username
											AND Users.u_deleted = 0 
											AND Users.u_profilevisible = 1)
								AND EXISTS (SELECT f_username FROM Friends as F2 WHERE F2.f_username = @Viewed
											AND F2.u_username = F1.f_username))
GO
/****** Object:  StoredProcedure [dbo].[MoveGroupToCategory]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MoveGroupToCategory] 
	(
		@GroupID		INT,
		@OldCategoryID	INT,
		@NewCategoryID	INT
	)
AS

UPDATE	GroupsCategories

		SET c_id = @NewCategoryID
				
		WHERE g_id = @GroupID AND c_id = @OldCategoryID
GO
/****** Object:  StoredProcedure [dbo].[LoadMessage]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadMessage]
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
		m_body			AS Body,
		m_timestamp		AS Timestamp,
		m_replied_to		AS RepliedTo,
		m_is_read		AS IsRead,
		m_pending_approval	AS PendingApproval
	
	FROM Messages
	WHERE @Id = m_id
GO
/****** Object:  StoredProcedure [dbo].[LoadBlog]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadBlog]
(
	@Id			INT = -1,
	@Username	VARCHAR(20) = NULL
)
AS
	SELECT
		b_id		as Id,
		u_username	as Username,
		b_name		as Name,
		b_description	as Description,
		b_datecreated	as DateCreated,
		b_settings		as Settings
	FROM
		Blogs
	WHERE
		(@Id IS NULL OR @Id = -1 OR b_id = @Id)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[LoadAudioUploads]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadAudioUploads]
	(
		@Id	INT = NULL,
		@Username	VARCHAR(20) = NULL,
		@Approved	BIT = NULL,
		@IsPrivate BIT = NULL
	)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		au_id	as Id,
		u_username	as Username,
		au_title AS Title,
		au_approved	as Approved,
		au_private as Private
	FROM
		AudioUploads
	WHERE
		(@Id IS NULL OR au_id = @Id)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Approved IS NULL OR au_approved = @Approved)
		AND (@IsPrivate IS NULL OR au_private = @IsPrivate)
END
GO
/****** Object:  StoredProcedure [dbo].[LoadSavedSearch]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadSavedSearch]
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
	SavedSearches

WHERE
	(@ID IS NULL OR @ID = ss_id)
	AND (@Username IS NULL OR @Username = u_username)
	AND (@Name IS NULL OR @Name = ss_name)
	AND (@EmailMatches IS NULL OR (ss_emailmatches = @EmailMatches AND ss_nextemaildate < GETDATE()))
GO
/****** Object:  StoredProcedure [dbo].[LoadVideoEmbeds]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadVideoEmbeds]
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
/****** Object:  StoredProcedure [dbo].[IsSectionUnlocked]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsSectionUnlocked]
(
	@Username	varchar(20),
	@TargetUsername varchar(20),
	@SectionType int,	
	@TargetID int
)
AS	
	IF EXISTS(SELECT us_id FROM UnlockedSections 
				WHERE u_username = @Username
				AND us_targetusername = @TargetUsername
				AND us_type = @SectionType
				AND (@TargetID IS NULL OR us_targetid = @TargetID)
				AND (us_unlockeduntil > GETDATE()))
				SELECT CAST(1 AS BIT)
	ELSE
				SELECT CAST(0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[IsPrivateVideo]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsPrivateVideo]
	@Username varchar(20)
AS
BEGIN
	SELECT vp_private FROM VideoProfiles WHERE u_username = @Username
END
GO
/****** Object:  StoredProcedure [dbo].[IsUserVerified]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsUserVerified]
(
	@VerifiedBy	VARCHAR(20),
	@VerifiedUser	VARCHAR(20),
	@MinUserVotes	INT,
	@ByAdmin	BIT
)
AS
IF (@ByAdmin = 1)
	BEGIN
		SELECT u_verifiedbyadmin FROM Users WHERE u_username = @VerifiedUser
	END
ELSE
	BEGIN
		IF @MinUserVotes <= (SELECT COUNT(vu_verifieduser)
					FROM VerifiedUsers
					WHERE vu_verifieduser = @VerifiedUser AND
					(@VerifiedBy IS NULL OR vu_verifiedby = @VerifiedBy))
			SELECT  CAST(1 AS BIT)
		ELSE
			SELECT CAST(0 AS BIT)
	END
GO
/****** Object:  StoredProcedure [dbo].[IsUserInFriendList]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsUserInFriendList]
	(
		@Username varchar(20),
		@Friend varchar(20)
	)
 AS
	IF EXISTS (SELECT u_username  FROM Friends WHERE @Username = u_username AND @Friend = f_username)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[IsUserInFavouriteList]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsUserInFavouriteList]
(
	@Username varchar(20),
	@Favourite varchar(20)
)
 AS
	IF EXISTS (SELECT u_username  FROM Favourites WHERE @Username = u_username AND @Favourite = f_username)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[IsUserBlocked]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsUserBlocked]
(
	@UserBlocker varchar(20),
	@BlockedUser varchar(20)
)
 AS
	IF ((SELECT COUNT(*)  FROM BlockedUsers WHERE @UserBlocker = bu_userblocker AND @BlockedUser = bu_blockeduser) <> 0)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  Table [dbo].[PhotoContestRanks]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoContestRanks](
	[u_username] [varchar](20) NOT NULL,
	[pc_id] [int] NOT NULL,
	[pce_id] [int] NOT NULL,
	[pcr_value] [int] NOT NULL,
 CONSTRAINT [PK_PhotoContestRanks] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC,
	[pc_id] ASC,
	[pce_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PollAnswers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PollAnswers](
	[p_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[pc_id] [int] NOT NULL,
 CONSTRAINT [PK_PollAnswers] PRIMARY KEY CLUSTERED 
(
	[p_id] ASC,
	[u_username] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Photos]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Photos](
	[p_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[pa_id] [int] NULL,
	[p_image] [image] NULL,
	[p_name] [nvarchar](80) NOT NULL,
	[p_description] [nvarchar](200) NOT NULL,
	[p_approved] [bit] NOT NULL,
	[p_primary] [bit] NOT NULL,
	[p_explicit] [bit] NOT NULL,
	[p_private] [bit] NOT NULL,
	[p_approvedby] [varchar](20) NULL,
	[p_approvedtimestamp] [datetime] NULL,
	[p_facecrop] [varchar](20) NULL,
	[p_manual_approval] [bit] NOT NULL,
	[p_salute] [bit] NOT NULL,
 CONSTRAINT [PK_Photos] PRIMARY KEY CLUSTERED 
(
	[p_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProfileChoices]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProfileChoices](
	[pc_id] [int] IDENTITY(1,1) NOT NULL,
	[pq_id] [int] NOT NULL,
	[pc_value] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_ProfileFieldValues] PRIMARY KEY CLUSTERED 
(
	[pc_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProfileAnswers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ProfileAnswers](
	[u_username] [varchar](20) NOT NULL,
	[pq_id] [int] NOT NULL,
	[pa_value] [nvarchar](4000) NOT NULL,
	[pa_approved] [bit] NOT NULL,
 CONSTRAINT [PK_ProfileDetails] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC,
	[pq_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PhotoContestVotes]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoContestVotes](
	[u_username] [varchar](20) NOT NULL,
	[pce_id_picked] [int] NOT NULL,
	[pce_id_nonpicked] [int] NOT NULL,
 CONSTRAINT [PK_PhotoContestVotes] PRIMARY KEY CLUSTERED 
(
	[u_username] ASC,
	[pce_id_picked] ASC,
	[pce_id_nonpicked] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[RateUser]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RateUser]
(
	@FromUser	VARCHAR(20),
	@ToUser	VARCHAR(20),
	@Rating	INT
)
AS
IF EXISTS (SELECT r_rating FROM Ratings WHERE r_fromusername = @FromUser AND r_tousername = @ToUser)
	UPDATE
		Ratings
	SET
		r_rating = @Rating
	WHERE
		r_fromusername = @FromUser
		AND r_tousername = @ToUser
ELSE
BEGIN
	DECLARE @Ip VARCHAR(15)
	SELECT @Ip = u_signup_ip FROM Users WHERE u_username = @FromUser	

	INSERT INTO Ratings
		(r_fromusername, r_tousername, r_rating, r_ip)
	VALUES
		(@FromUser, @ToUser, @Rating, @Ip)

END
GO
/****** Object:  StoredProcedure [dbo].[RequestSubscriptionCancellation]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RequestSubscriptionCancellation] 
(
	@ID int
)
AS
	UPDATE Subscriptions SET s_cancellation_requested = 1 WHERE s_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[RemoveVerifiedUserStatus]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveVerifiedUserStatus] 
(
	@VerifiedBy	varchar(20),
	@VerifiedUser	varchar(20),
	@ByAdmin	BIT
)
AS
IF (@ByAdmin = 1)
	UPDATE Users SET u_verifiedbyadmin=0 WHERE u_username = @VerifiedUser
ELSE
	DELETE FROM VerifiedUsers WHERE
		 vu_verifiedby = @VerifiedBy AND
		 vu_verifieduser = @VerifiedUser
GO
/****** Object:  StoredProcedure [dbo].[RemoveFromFriends]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[RemoveFromFriends]
	(
		@Username varchar(20),
		@Friend varchar(20)
	)
 AS
	DELETE FROM Friends WHERE @Username = u_username AND @Friend = f_username
GO
/****** Object:  StoredProcedure [dbo].[RemoveFromFavourites]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RemoveFromFavourites]
(
	@Username varchar(20),
	@Favourite varchar(20)
)
 AS
	DELETE FROM Favourites WHERE @Username = u_username AND @Favourite = f_username
GO
/****** Object:  StoredProcedure [dbo].[SaveBlog]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveBlog]
(
	@Id			int = -1,
	@Username	varchar(20),
	@Name		nvarchar(200),
	@Description	ntext,
	@Settings		ntext
)
AS

IF (@Id = -1)
BEGIN

	INSERT INTO Blogs
	(u_username, b_name, b_description, b_settings)
	VALUES
	(@Username, @Name, @Description, @Settings)
	
	SELECT @@Identity
END
ELSE
	UPDATE Blogs
	SET b_name = @Name, b_description = @Description, b_settings = @Settings
	WHERE b_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[SaveAudioUpload]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAudioUpload]
	(
		@Id	INT = NULL,
		@Username	VARCHAR(20),
		@Title NVARCHAR(200),
		@Approved	BIT = 0,
		@IsPrivate BIT = 0
	)
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Id IS NULL)
	BEGIN
		INSERT INTO AudioUploads
			(u_username, au_title, au_approved, au_private)
		VALUES
			(@Username, @Title, @Approved, @IsPrivate)

		SELECT @@Identity
	END
	ELSE
	BEGIN
		UPDATE AudioUploads
		SET u_username = @Username,
			au_title = @Title,
			au_approved = @Approved,
			au_private = @IsPrivate
		WHERE
			au_id = @Id
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveAffiliateHistory]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAffiliateHistory] 
	(
		@ID				INT,
		@AffiliateID	INT,
		@Amount			MONEY,
		@DatePaid		DATETIME,
		@Notes			NTEXT,
		@PrivateNotes	NTEXT
	)
AS

IF (@ID IS NUll)

	BEGIN
		INSERT INTO AffiliatesHistory (a_id, ah_amount, ah_date_paid, ah_notes, ah_private_notes)
						VALUES (@AffiliateID, @Amount, @DatePaid, @Notes, @PrivateNotes)
		
		SELECT @@Identity
	END

ELSE

	BEGIN
		UPDATE AffiliatesHistory
		SET a_id = @AffiliateID,
			ah_amount = @Amount,
			ah_date_paid = @DatePaid,
			ah_notes = @Notes,
			ah_private_notes = @PrivateNotes
		WHERE ah_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveEcard]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveEcard]
	(
		@ID INT,
		@EcardTypeID INT,
		@FromUsername VARCHAR(20),
		@ToUsername VARCHAR(20),
		@Date DATETIME,
		@Message NVARCHAR(2000),
		@DeletedByFromuser BIT,
		@DeleteByToUser BIT,
		@IsOpened BIT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO Ecards (ect_id, ec_from_username, ec_to_username, ec_date, ec_message, ec_deleted_by_fromuser, ec_deleted_by_touser, ec_is_opened)
						VALUES (@EcardTypeID, @FromUsername, @ToUsername, @Date, @Message, @DeletedByFromuser, @DeleteByToUser, @IsOpened)
		SELECT @@Identity
		
	END
ELSE
	BEGIN
		UPDATE Ecards
		SET	ect_id = @EcardTypeID,
			ec_from_username = @FromUsername,
			ec_to_username = @ToUsername,
			ec_date = @Date,
			ec_message = @Message,
			ec_deleted_by_fromuser = @DeletedByFromuser,
			ec_deleted_by_touser = @DeleteByToUser,
			ec_is_opened = @IsOpened
		WHERE ec_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveContentPage]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveContentPage]
(
	@ID					INT,
	@Title				NVARCHAR(100),
	@Content			NTEXT,
	@HeaderPosition		INT,
	@FooterPosition		INT,
	@VisibleFor			INT,
	@URL				VARCHAR(250),
	@MetaDescription	NVARCHAR(200),
	@MetaKeyword		NVARCHAR(200),
	@Language			INT,
	@UrlRewrite			VARCHAR(200) = NULL
)

 AS

IF (@ID = -1)
	BEGIN
		INSERT INTO ContentPage
		(cp_title, cp_content, cp_header_position, cp_footer_position, cp_visiblefor, cp_url, cp_metadescription, cp_metakeyword, l_id, cp_urlrewrite)
		VALUES
		(@Title, @Content, @HeaderPosition, @FooterPosition, @VisibleFor, @URL, @MetaDescription, @MetaKeyword, @Language, @UrlRewrite)
	
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE ContentPage
		SET	cp_title = @Title,
			cp_content = @Content,
			cp_header_position = @HeaderPosition,
			cp_footer_position = @FooterPosition,
			cp_visiblefor = @VisibleFor,
			cp_url = @URL,
			cp_metadescription = @MetaDescription,
			cp_metakeyword = @MetaKeyword,
			l_id = @Language,
			cp_urlrewrite = @UrlRewrite

		WHERE @ID = cp_id
		
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveCommunityProfileApproval]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveCommunityProfileApproval]
	(
		@ID INT,
		@Username VARCHAR(20),
		@ApprovedBy VARCHAR(20),
		@Approved BIT,
		@Date DATETIME
	)
AS

IF (@ID IS NULL AND NOT EXISTS (SELECT 1 FROM CommunityProfileApproval WHERE u_username = @Username AND cpa_approvedby = @ApprovedBy))

		BEGIN
		
			INSERT INTO CommunityProfileApproval
			(u_username, cpa_approvedBy, cpa_approved, cpa_timestamp)
			VALUES
			(@Username, @ApprovedBy, @Approved, @Date)
			
			SELECT @@Identity
		END
	
ELSE

	BEGIN
	
		UPDATE CommunityProfileApproval
		
		SET u_username = @Username,
			cpa_approvedby = @ApprovedBy,
			cpa_approved = @Approved,
			cpa_timestamp = @Date
			
		WHERE cpa_id = @ID
			
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupEventImage]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupEventImage] 
	(
		@GroupEventID	INT,
		@Image			IMAGE
	)
AS

UPDATE GroupEvents

SET ge_image = @Image

WHERE ge_id = @GroupEventID
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupEvent]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupEvent] 
	(
		@ID				INT,
		@GroupID		INT,
		@Username		VARCHAR(20),
		@Title			NVARCHAR(200),
		@Description	NTEXT,
		@Date			DATETIME,
		@Location		NTEXT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO GroupEvents (g_id, u_username, ge_title, ge_description, ge_date, ge_location)
						VALUES (@GroupID, @Username, @Title, @Description, @Date, @Location)
		SELECT @@Identity
		
	END

ELSE

	BEGIN
		UPDATE GroupEvents
		SET	g_id = @GroupID,
			u_username = @Username,
			ge_title = @Title,
			ge_description = @Description,
			ge_date = @Date,
			ge_location = @Location
		WHERE ge_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupBan]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupBan] 
	(
		@ID			INT,
		@GroupID	INT,
		@Username	VARCHAR(20),
		@Date		DATETIME,
		@Expires	DATETIME
	)
AS

IF @ID IS NULL

	BEGIN
	
		INSERT INTO GroupBans
				(g_id, u_username, gb_date, gb_expires)
				VALUES
				(@GroupID, @Username, @Date, @Expires)
	END
	
ELSE
	
	BEGIN
		UPDATE	GroupBans
				
				SET g_id = @GroupID,
					u_username = @Username,
					gb_date = @Date,
					gb_expires = @Expires
		
		WHERE gb_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveEvent]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveEvent]
	(
		@ID INT,
		--@Username VARCHAR(20),
		@Type INT,
		@FromUsername VARCHAR(20),
		@FromGroup INT,
		@DetailsXML NVARCHAR(4000),
		@Date DATETIME
	)
AS

IF @ID IS NULL
	BEGIN
		INSERT INTO Events (e_type, e_fromusername, e_fromgroup, e_details, e_date)
						VALUES (@Type, @FromUsername, @FromGroup, @DetailsXML, @Date)
		
		SELECT CONVERT(Int,@@Identity) AS [Id]
	END
	
ELSE
	BEGIN
		UPDATE Events
		SET	e_type = @Type,
			e_fromusername = @FromUsername,
			e_fromgroup = @FromGroup,
			e_details = @DetailsXML,
			e_date = @Date
		WHERE e_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveEstablishedCommunication]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveEstablishedCommunication]
	(
		@FromUsername VARCHAR(20),
		@ToUsername VARCHAR(20),
		@Date DATETIME
	)
AS

IF NOT EXISTS (SELECT 1 FROM EstablishedCommunications WHERE ec_from_username = @FromUsername AND ec_to_username = @ToUsername) 
				
		INSERT INTO EstablishedCommunications (ec_from_username, ec_to_username, ec_date)
											VALUES (@FromUsername, @ToUsername, @Date)
ELSE
	UPDATE EstablishedCommunications
	SET ec_date = @Date
	WHERE ec_from_username = @FromUsername AND ec_to_username = @ToUsername
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupPhoto]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupPhoto] 
	(
		@ID				INT,
		@GroupID		INT,
		@Username		VARCHAR(20),
		@Name			NVARCHAR(80),
		@Description	NVARCHAR(200),
		@Date			DATETIME,
		@Image			IMAGE
	)
AS

IF @ID IS NULL

	BEGIN

		INSERT	INTO GroupPhotos
						(g_id, u_username, gp_name, gp_description, gp_date, gp_image)
				VALUES	
						(@GroupID, @Username, @Name, @Description, @Date, @Image)
				
		SELECT @@Identity
		
	END
		
ELSE

	BEGIN

		UPDATE	GroupPhotos
		
		SET		g_id = @GroupID,
				u_username = @Username,
				gp_name = @Name,
				gp_description = @Description,
				gp_date = @Date,
				gp_image = @Image
		
		WHERE gp_id = @ID
	
END
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupMember]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupMember] 
	(
		@GroupID	INT,
		@Username	VARCHAR(20),
		@Type		INT,
		@Active		BIT,
		@JoinDate	DATETIME,
		@InvitedBy	VARCHAR(20),
		@JoinAnswer NTEXT,
		@IsWarned	BIT,
		@WarnReason NVARCHAR(200),
		@WarnExpirationDate DATETIME
	)
AS

IF NOT EXISTS(SELECT g_id FROM GroupMembers WHERE g_id = @GroupID AND u_username = @Username)

	INSERT INTO GroupMembers
	(g_id, u_username, gm_type, gm_active, gm_joindate, gm_invitedby, gm_joinanswer, gm_warned, gm_warn_reason, gm_warn_expirationdate)
	VALUES
	(@GroupID, @Username, @Type, @Active, @JoinDate, @InvitedBy, @JoinAnswer, @IsWarned, @WarnReason, @WarnExpirationDate)
	
ELSE

	UPDATE	GroupMembers
		
		SET gm_type = @Type,
			gm_active = @Active,
			gm_joindate = @JoinDate,
			gm_invitedby = @InvitedBy,
			gm_joinanswer = @JoinAnswer,
			gm_warned = @IsWarned,
			gm_warn_reason = @WarnReason,
			gm_warn_expirationdate = @WarnExpirationDate
		
		WHERE g_id = @GroupID AND u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[SaveCreditsHistory]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveCreditsHistory]
	(
		@ID INT,
		@Username VARCHAR(20),
		@Date DATETIME,
		@Service INT,
		@Amount INT
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO CreditsHistory
					(u_username, ch_date, ch_service, ch_amount) VALUES (@Username, @Date, @Service, @Amount)
		SELECT @@Identity
	END
ELSE
	UPDATE CreditsHistory
	SET	u_username = @Username,
		ch_date = @Date,
		ch_service = @Service,
		ch_amount = @Amount
	WHERE ch_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveAd]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAd]
	(
		@ID int,
		@CategoryID int,
		@PostedBy varchar(20),
		@Date datetime,
		@ExpirationDate datetime,
		@Locatation nvarchar(200),
		@Subject nvarchar(200),
		@Description ntext,
		@Approved bit
	)
AS

IF @ID IS NULL
	BEGIN
		INSERT INTO Ads (ac_id, a_postedby, a_date, a_expirationdate, a_location, a_subject, a_description, a_approved)
					VALUES (@CategoryID, @PostedBy, @Date, @ExpirationDate, @Locatation, @Subject, @Description, @Approved)
		SELECT @@Identity
	END
ELSE
	UPDATE Ads
	SET	ac_id = @CategoryID,
		a_postedby = @PostedBy,
		a_date = @Date,
		a_expirationdate = @ExpirationDate,
		a_location = @Locatation,
		a_subject = @Subject,
		a_description = @Description,
		a_approved = @Approved
	WHERE a_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveComment]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveComment]
(
	@Id			int = -1,
	@FromUsername	varchar(20),
	@ToUsername		varchar(20),
	@CommentText		ntext,
	@DatePosted		datetime = getdate,
	@Approved		bit = 1
)
AS

-- create new comment
IF (@Id = -1)
BEGIN
--	SET @Id = (SELECT c_id from Comments where c_from_username=@FromUsername AND c_to_username=@ToUsername)
--	IF (@Id IS NULL)
	BEGIN
		INSERT INTO Comments
		(c_from_username, c_to_username, c_comment_text, c_date_posted, c_approved)
		VALUES
		(@FromUsername, @ToUsername, @CommentText, @DatePosted, @Approved)
	
		SELECT @@Identity
	END
--	ELSE GOTO update_comment
END
ELSE
update_comment:		-- update comment
	UPDATE Comments
	SET c_comment_text = @CommentText, c_approved = @Approved
	WHERE c_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupTopic]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupTopic] 
	(
		@ID				INT,
		@GroupID		INT,
		@Username		VARCHAR(20),
		@Name			NVARCHAR(200),
		@DateCreated	DATETIME,
		@DateUpdated	DATETIME,
		@Posts			INT,
		@Locked			BIT,
		@StickyUntil	DATETIME,
		@IsPoll			BIT,
		@Views			INT
	)
AS

IF @ID IS NULL

	BEGIN
	
		INSERT INTO GroupTopics
			(g_id, u_username, gt_name, gt_datecreated, gt_dateupdated, gt_posts, gt_locked, gt_stickyuntil, gt_poll, gt_views)
			VALUES 
			(@GroupID, @Username, @Name, @DateCreated, @DateUpdated, @Posts, @Locked, @StickyUntil, @IsPoll, @Views)
			
			SELECT @@Identity
	END

ELSE
	
	BEGIN
	
	UPDATE GroupTopics
	
			SET
				g_id = @GroupID,
				u_username = @Username,
				gt_name = @Name,
				gt_datecreated = @DateCreated,
				gt_dateupdated = @DateUpdated,
				gt_posts = @Posts,
				gt_locked = @Locked,
				gt_stickyuntil = @StickyUntil,
				gt_poll = @IsPoll,
				gt_views = @Views
				
	WHERE gt_id = @ID
	
	END
GO
/****** Object:  StoredProcedure [dbo].[SavePaymentHistory]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePaymentHistory]
(
	@Username varchar(20),
	@PaymentProcessor varchar(50),
	@Amount money,
	@Description nvarchar(200),
	@Notes nvarchar(2000),
	@Status int
)
AS

INSERT INTO PaymentHistory
(u_username, ph_paymentprocessor, ph_amount, ph_description, ph_notes, ph_status)
VALUES
(@Username, @PaymentProcessor, @Amount, @Description, @Notes, @Status)

SELECT @@Identity
GO
/****** Object:  StoredProcedure [dbo].[SaveNews]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveNews]
(
	@ID int=null,
	@Text ntext,
	@Date datetime,
	@Title nvarchar (100),
	@Language int
	
)
 AS
	IF (@ID IS NULL)
		INSERT INTO News
			(l_id, n_title, n_text, n_date)
		VALUES
			(@Language, @Title, @Text, @Date)
	ELSE
		UPDATE News
		SET
			l_id = @Language,
			n_title = @Title,
			n_text = @Text,
			n_date =@Date
		WHERE
			n_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveMessagesSandBox]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveMessagesSandBox]
	(
		@Username VARCHAR(20),
		@Message NVARCHAR(100)
	)
AS

IF NOT EXISTS (SELECT u_username FROM MessagesSandBox WHERE u_username = @Username AND ms_message = @Message)
	BEGIN
		INSERT INTO MessagesSandBox (u_username, ms_message, ms_count, ms_datetime)
					VALUES (@Username, @Message, 1, GETDATE())	
	END
ELSE
	BEGIN
		UPDATE MessagesSandBox
		SET ms_count = ms_count + 1,
			ms_datetime = GETDATE()
		WHERE u_username = @Username AND ms_message = @Message
	END
	
	SELECT ms_count FROM MessagesSandBox WHERE u_username = @Username AND ms_message = @Message
GO
/****** Object:  StoredProcedure [dbo].[SavePhotoAlbum]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePhotoAlbum]
	(
		@ID int,
		@Username varchar(20),
		@Name nvarchar(50),
		@Access int,
		@CoverPhotoID int
	)
AS

IF @ID IS NULL
	BEGIN
		INSERT INTO PhotoAlbums (u_username, pa_name, pa_access, pa_cover_photoid)
							VALUES (@Username, @Name, @Access, @CoverPhotoID)
		SELECT @@Identity
	END
ELSE
	UPDATE PhotoAlbums
	SET	u_username = @Username,
		pa_name = @Name,
		pa_access = @Access,
		pa_cover_photoid = @CoverPhotoID
	WHERE pa_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveProfileQuestion]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveProfileQuestion]
(
	@TopicID	int,
	@ID		int=null,
	@Name		nvarchar(250),
	@AltName	nvarchar(250),
	@Description	nvarchar(4000),
	@Hint		nvarchar(250),
	@EditStyle	int,
	@ShowStyle	int,
	@SearchStyle	int,
	@Required	bit,
	@RequiresApproval bit,
	@VisibleForMale		bit = 1,
	@VisibleForFemale	bit = 1,
	@VisibleForCouple	bit = 1,
	@MatchField int,
	@ViewPaidOnly bit,
	@EditPaidOnly bit,
	@ParentQuestionID int,
	@ParentQuestionChoices nvarchar(3000)
)
AS
	if (@ID is NULL)
	BEGIN
		DECLARE @Priority int
		SET @Priority = 
			(SELECT MAX(pq_priority) FROM ProfileQuestions WHERE pt_id = @TopicID) + 1
		IF (@Priority IS NULL) 
			SET @Priority = 1
	
		INSERT INTO ProfileQuestions
			(pt_id, pq_name, pq_altname, pq_description, pq_hint, pq_edit_style, pq_show_style, pq_search_style, pq_required, pq_priority, pq_requires_approval, pq_visible_male, pq_visible_female, pq_visible_couple, pq_match_field, pq_viewpaidonly, pq_editpaidonly, pq_parent_question_id, pq_parent_question_choices)
		VALUES
			(@TopicID, @Name, @AltName, @Description, @Hint, @EditStyle, @ShowStyle, @SearchStyle, @Required, @Priority, @RequiresApproval, @VisibleForMale, @VisibleForFemale, @VisibleForCouple, @MatchField, @ViewPaidOnly, @EditPaidOnly, @ParentQuestionID, @ParentQuestionChoices)
	END
	ELSE
		UPDATE ProfileQuestions
		SET 
			pq_name = @Name,
			pq_altname = @AltName,
			pq_description = @Description,
			pq_hint = @Hint,
			pq_edit_style = @EditStyle,
			pq_show_style = @ShowStyle,
			pq_search_style = @SearchStyle,
			pq_required = @Required,
			pq_requires_approval = @RequiresApproval,
			pq_visible_male = @VisibleForMale,
			pq_visible_female = @VisibleForFemale,
			pq_visible_couple = @VisibleForCouple,
			pq_match_field = @MatchField,
			pq_viewpaidonly = @ViewPaidOnly,
			pq_editpaidonly = @EditPaidOnly,
			pq_parent_question_id = @ParentQuestionID,
			pq_parent_question_choices = @ParentQuestionChoices
		WHERE
			pq_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveVideoProfile]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveVideoProfile]
(
	@Username	VARCHAR(20),
	@IsPrivate	BIT
)
 AS
	IF (@IsPrivate IS NULL)
	BEGIN
		IF NOT EXISTS (SELECT u_username FROM VideoProfiles WHERE u_username = @Username)
			INSERT INTO VideoProfiles (u_username) VALUES(@Username)
		ELSE
			UPDATE VideoProfiles SET vp_approved=0 WHERE u_username = @Username
	END
	ELSE
		UPDATE VideoProfiles SET vp_private = @IsPrivate WHERE u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[SaveVideoEmbed]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveVideoEmbed]
	@Id	INT = NULL,
	@Username	VARCHAR(20),
	@ThumbUrl	VARCHAR(100) = NULL,
	@VideoUrl	VARCHAR(100),
	@Title		NVARCHAR(100) = NULL,
	@SourceType	INT = 1
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Id IS NULL)
	BEGIN
		INSERT INTO VideoEmbeds
			(u_username, ve_thumburl, ve_videourl, ve_title, ve_sourcetype)
		VALUES
			(@Username, @ThumbUrl, @VideoUrl, @Title, @SourceType)

		SELECT @@Identity
	END
	ELSE
	BEGIN
		UPDATE VideoEmbeds
		SET 
			u_username = @Username, ve_thumburl = @ThumbUrl, ve_videourl = @VideoUrl, ve_title = @Title, ve_sourcetype = @SourceType
		WHERE
			ve_id = @Id
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveSavedSearch]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--=================== ALTER PROCEDURE SAVESAVEDSEARCHES ====================

CREATE PROCEDURE [dbo].[SaveSavedSearch]
(
	@ID int = -1,
	@Username varchar(20),
	@Name nvarchar(100),
	@Gender int,
	@Country varchar(100),
	@State nvarchar(100),
	@Zip varchar(15),
	@City varchar(100),
	@AgeFrom int, 
	@AgeTo int,
	@PhotoRequired bit,
	@ChoiceIDs text,
	@EmailMatches bit,
	@EmailFrequency int,
	@NextEmailDate datetime
)
AS

IF (@ID = -1)
	BEGIN
		INSERT INTO SavedSearches
		(u_username, ss_name, ss_gender, ss_country, ss_state, ss_zip, ss_city, ss_agefrom, ss_ageto, ss_photoreq, ss_choiceids, ss_emailmatches, ss_emailfrequency, ss_nextemaildate)
		VALUES
		(@Username, @Name, @Gender, @Country, @State, @Zip, @City, @AgeFrom, @AgeTo, @PhotoRequired, @ChoiceIDs, @EmailMatches, @EmailFrequency, @NextEmailDate)

		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE SavedSearches
		SET 	u_username = @Username, 
		        	ss_name = @Name, 
			ss_gender = @Gender,
			ss_country = @Country,
			ss_state = @State,
			ss_zip = @Zip,
			ss_city = @City,
			ss_agefrom = @AgeFrom,
			ss_ageto = @AgeTo,
			ss_photoreq = @PhotoRequired,
			ss_choiceids = @ChoiceIDs,
			ss_emailmatches = @EmailMatches,
			ss_emailfrequency = @EmailFrequency,
			ss_nextemaildate = @NextEmailDate
		
		WHERE	@ID = ss_id
			
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveProfileView]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveProfileView]
(
	@Viewer	VARCHAR(20),
	@Viewed	VARCHAR(20)
)
AS

IF NOT EXISTS (SELECT 1 FROM ProfileViews WHERE pv_viewer = @Viewer AND pv_viewed = @Viewed)
	BEGIN
		INSERT INTO ProfileViews
			(pv_viewer, pv_viewed)
		VALUES
			(@Viewer, @Viewed)
			
		UPDATE 	Users 
		SET u_profileviews = u_profileviews + 1
		WHERE u_username = @Viewed
	END

ELSE

	UPDATE TOP (1) ProfileViews
	SET pv_timestamp = getdate()
	WHERE pv_viewer = @Viewer AND pv_viewed = @Viewed
GO
/****** Object:  StoredProcedure [dbo].[SavePhotoContestEntry]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePhotoContestEntry]
	@Id			INT = NULL,
	@ContestId	INT,
	@Username	VARCHAR(20),
	@PhotoId	INT
AS
BEGIN
	SET NOCOUNT ON;

	IF (@Id IS NULL)
	BEGIN
		INSERT INTO PhotoContestEntries
			(pc_id, u_username, p_id)
		VALUES
			(@ContestId, @Username, @PhotoId)

		SELECT @@Identity
	END
	ELSE
	BEGIN
		UPDATE PhotoContestEntries
		SET 
			pc_id = @ContestId, u_username = @Username, p_id = @PhotoId
		WHERE
			pce_id = @Id
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SaveTranslation]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveTranslation] 
(
	@LanguageId	INT = 1,
	@Key		NVARCHAR(400),
	@Value		NVARCHAR(400),
	@AdminPanel INT
)
AS

IF NOT EXISTS (SELECT t_key FROM Translations 
				WHERE	
					l_id = @LanguageId AND
					t_key = @Key AND
					t_adminpanel = @AdminPanel)
BEGIN
	INSERT INTO Translations
		(l_id, t_key, t_value, t_adminpanel)
	VALUES
		(@LanguageId, @Key, @Value, @AdminPanel)
END
ELSE
BEGIN
	UPDATE Translations
	SET t_value = @Value
	WHERE 
		l_id = @LanguageId AND
		t_key = @Key AND
		t_adminpanel = @AdminPanel
	
END
GO
/****** Object:  StoredProcedure [dbo].[SearchGroupsByUsername]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchGroupsByUsername]
	(
		@Username	VARCHAR(20),
		@Approved	INT,
		@SortColumn	INT
	)
AS

SELECT
		g.g_id AS ID
		
FROM Groups AS g INNER JOIN GroupMembers AS gm
			ON gm.g_id = g.g_id
			
WHERE gm.u_username = @Username AND (@Approved IS NULL OR g.g_approved = @Approved)

ORDER BY
		CASE WHEN @SortColumn = 1 THEN g_datecreated END DESC,
		
		CASE WHEN @SortColumn = 2 THEN g_name
			 WHEN @SortColumn = 3 THEN g_owner
		END,
		
		CASE WHEN @SortColumn = 4 THEN g_activemembers END DESC
GO
/****** Object:  StoredProcedure [dbo].[SearchGroups]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchGroups] 
	(
		@CategoryID			INT,
		@Name				NVARCHAR(250),
		@Description		NVARCHAR(4000),
		@DateCreated		DATETIME,
		@Approved			BIT,
		@AccessLevel		INT,
		@Owner				VARCHAR(20),
		@ActiveMembers		INT,
		@Keyword			NVARCHAR(200),
		@SearchDescription	BIT,
		@NumberOfGroups		INT,
		@SortColumn			INT
		
	)
AS

IF @NumberOfGroups IS NOT NULL
	SET ROWCOUNT @NumberOfGroups

SELECT
		g_id AS ID	
FROM	Groups AS g

WHERE
		(@Name IS NULL OR g_name = @Name)
		AND (@Description IS NULL OR g_description = @Description)
		AND (@DateCreated IS NULL OR g_datecreated = @DateCreated)
		AND (@Approved IS NULL OR g_approved = @Approved)
		AND (@AccessLevel IS NULL OR g_accesslevel = @AccessLevel)
		AND (@Owner IS NULL OR g_owner = @Owner)
		AND (@ActiveMembers IS NULL OR g_activemembers = @ActiveMembers)
		AND (@Keyword IS NULL OR g_name like '%' + @Keyword + '%' OR (@SearchDescription = 1 AND g_description like '%' + @Keyword + '%'))
		AND (@CategoryID IS NULL OR EXISTS (SELECT c.c_id FROM GroupsCategories as c WHERE c.c_id = @CategoryID AND c.g_id = g.g_id))

ORDER BY
		CASE WHEN @SortColumn = 1 THEN g_datecreated END DESC,
		
		CASE WHEN @SortColumn = 2 THEN g_name
			 WHEN @SortColumn = 3 THEN g_owner
		END,
		
		CASE WHEN @SortColumn = 4 THEN g_activemembers END DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[SearchGroupPhotos]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchGroupPhotos]
	(
		@GroupID		INT,
		@Username		VARCHAR(20),
		@Name			NVARCHAR(80),
		@Date			DATETIME,
		@Keyword		NVARCHAR(200),
		@SortColumn		INT
	)
AS

SELECT	gp_id AS ID

FROM	GroupPhotos

WHERE	
		(@GroupID IS NULL OR g_id = @GroupID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Name IS NULL OR gp_name = @Name)
		AND (@Date IS NULL OR u_username = @Date)
		AND (@Keyword IS NULL OR gp_name like '%' + @Keyword + '%' OR gp_description like '%'+ @Keyword + '%')
		
ORDER BY
		CASE @SortColumn
			WHEN 1 THEN gp_date
		END DESC
GO
/****** Object:  StoredProcedure [dbo].[SearchGroupEvents]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchGroupEvents] 
	(
		@GroupID	INT,
		@Username	VARCHAR(20),
		@Title		NVARCHAR(200),
		@Keyword	NVARCHAR(200),
		@FromDate	DATETIME,
		@ToDate		DATETIME,
		@GroupMember VARCHAR(20),
		@NumberOfGroupEvents INT,
		@SortColumn INT
	)
AS

IF @NumberOfGroupEvents IS NOT NULL
	SET ROWCOUNT @NumberOfGroupEvents

SELECT ge_id AS [ID]
FROM GroupEvents AS ge, GroupMembers AS gm
WHERE	(@GroupID IS NULL OR ge.g_id = @GroupID)
		AND (@Username IS NULL OR ge.u_username = @Username)
		AND (@Title IS NULL OR ge_title = @Title)
		AND (@Keyword IS NULL OR ge_description LIKE '%' + @Keyword + '%')
		AND (@FromDate IS NULL OR ge_date >= @FromDate) AND (@ToDate IS NULL OR ge_date <= @ToDate)
		AND (@GroupMember IS NULL OR ge.g_id = gm.g_id AND gm.u_username = @GroupMember)
ORDER BY

CASE WHEN @SortColumn = 1 THEN ge.g_id END,
CASE WHEN @SortColumn = 2 THEN ge.u_username END,
CASE WHEN @SortColumn = 3 THEN ge_date END DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[VideoEmbedsSearch]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
/****** Object:  StoredProcedure [dbo].[UnlockSection]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UnlockSection]
(
	@Username	varchar(20),
	@TargetUsername varchar(20),
	@SectionType int,	
	@TargetID int,
	@UnlockedUntil datetime
)
AS
	DECLARE @EndDate DATETIME
	DECLARE @ID INT
	SET @ID = NULL
	SET @EndDate = NULL
	
	SELECT @EndDate = us_unlockeduntil, @ID = us_id FROM UnlockedSections 
				WHERE u_username = @Username
				AND us_targetusername = @TargetUsername
				AND us_type = @SectionType
				AND (@TargetID IS NULL OR us_targetid = @TargetID)
				
	IF (@EndDate IS NOT NULL)
	BEGIN
		IF (@EndDate < GETDATE())
			UPDATE UnlockedSections SET us_unlockeduntil = @UnlockedUntil
			WHERE @ID = us_id
	END
	ELSE
	BEGIN
		INSERT INTO UnlockedSections
			(u_username, us_targetusername, us_type, us_targetid, us_unlockeduntil)
		VALUES
			(@Username, @TargetUsername, @SectionType, @TargetID, @UnlockedUntil)
	END
GO
/****** Object:  StoredProcedure [dbo].[UnblockUser]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UnblockUser] 
(
	@UserBlocker	varchar(20),
	@BlockedUser	varchar(20)
)
AS
	DELETE FROM BlockedUsers WHERE
		 bu_userblocker = @UserBlocker AND
		 bu_blockeduser = @BlockedUser
GO
/****** Object:  StoredProcedure [dbo].[SetProfileModerationApprovalScore]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetProfileModerationApprovalScore]
	(
		@Username VARCHAR(20),
		@Approved BIT,
		@Scores INT,
		@Penalty INT
	)
AS

BEGIN TRANSACTION

UPDATE Users
SET u_moderationscore = u_moderationscore + @Scores
WHERE u_username IN (SELECT cpa.cpa_approvedby FROM CommunityProfileApproval AS cpa WHERE cpa.cpa_approved = @Approved AND cpa.u_username = @Username)

UPDATE Users
SET u_moderationscore = u_moderationscore - @Penalty
WHERE u_username IN (SELECT cpa.cpa_approvedby FROM CommunityProfileApproval AS cpa WHERE cpa.cpa_approved != @Approved AND cpa.u_username = @Username)

COMMIT TRANSACTION
GO
/****** Object:  StoredProcedure [dbo].[SearchPhotoContestEntries]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchPhotoContestEntries]
	(
		@ContestID		INT,
		@Username		VARCHAR(20),
		@SortColumn		VARCHAR(50)
	)
AS

SELECT
		pce_id AS ID
		
FROM PhotoContestEntries pce
	JOIN PhotoContests pc ON pce.pc_id = pc.pc_id
WHERE
	(@ContestID IS NULL OR @ContestID = pce.pc_id) AND
	(@Username IS NULL OR @Username = u_username)
ORDER BY
	CASE @SortColumn
		WHEN 'Username' THEN u_username
		WHEN 'ContestName' THEN pc_name
	END
GO
/****** Object:  StoredProcedure [dbo].[SearchAds]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchAds]
	(
		@CategoryID int,
		@PostedBy varchar(20),
		@Date datetime,
		@ExpirationDate datetime,
		@Approved bit,
		@Keyword nvarchar(200),
		@NumberOfAds int,
		@SortColumn int
	)
AS

IF @NumberOfAds IS NOT NULL
	SET ROWCOUNT @NumberOfAds

SELECT	a_id AS [ID]
FROM	Ads
WHERE	(@CategoryID IS NULL OR ac_id = @CategoryID)
		AND (@PostedBy IS NULL OR a_postedby = @PostedBy)
		AND (@Date IS NULL OR a_date = @Date)
		AND (@ExpirationDate IS NULL OR a_expirationdate > @ExpirationDate)
		AND (@Approved IS NULL OR a_approved = @Approved)
		AND (@Keyword IS NULL OR a_subject like '%' + @Keyword + '%' OR a_description like '%' + @Keyword + '%')
ORDER BY
		CASE WHEN @SortColumn = 1 THEN a_date END DESC,
		CASE WHEN @SortColumn = 2 THEN a_expirationdate END DESC,
		CASE WHEN @SortColumn = 3 THEN ac_id END ,
		CASE WHEN @SortColumn = 4 THEN a_postedby END,
		CASE WHEN @SortColumn = 5 THEN a_approved END

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[SetNextEmailDate]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetNextEmailDate]
	(
		@Id				INT,
		@NextEmailDate	DATETIME
	)
AS

UPDATE	SavedSearches

		SET		ss_nextemaildate = @NextEmailDate
		WHERE	ss_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[SetAsVerifiedUser]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetAsVerifiedUser] 
(
	@VerifiedBy	varchar(20),
	@VerifiedUser	varchar(20),
	@ByAdmin	BIT
)
AS
IF (@ByAdmin = 1)
	UPDATE Users SET u_verifiedbyadmin = 1 WHERE u_username = @VerifiedUser
ELSE
	IF NOT EXISTS (SELECT vu_verifiedby  FROM VerifiedUsers WHERE @VerifiedBy = vu_verifiedby AND @VerifiedUser = vu_verifieduser)
		INSERT INTO VerifiedUsers
			(vu_verifiedby, vu_verifieduser)
		VALUES
			(@VerifiedBy,@VerifiedUser)
GO
/****** Object:  StoredProcedure [dbo].[SetAsPaidUser]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SetAsPaidUser]
(
	@SubscriptionID int,
	@IsPaid	bit
)
 AS
	DECLARE @Username varchar(20)
	SET @Username = (SELECT u_username FROM Subscriptions WHERE @SubscriptionID = s_id)
	UPDATE Users
	SET
		u_paid_member = @IsPaid
	WHERE
		@Username = u_username
GO
/****** Object:  StoredProcedure [dbo].[SetAccessToPrivateVideo]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetAccessToPrivateVideo] 
(
	@VideoOwner	varchar(20),
	@VideoViewer	varchar(20),
	@GrantAccess	bit
)
AS
	IF (@GrantAccess = 1)
	BEGIN
		IF ((SELECT COUNT(*)  FROM VideoAccess WHERE @VideoOwner = va_videoowner AND @VideoViewer = va_videoviewer) = 0)
			INSERT INTO VideoAccess
				(va_videoowner, va_videoviewer)
			VALUES
				(@VideoOwner,@VideoViewer)
	END
	ELSE
		DELETE FROM VideoAccess WHERE
			 va_videoowner = @VideoOwner AND
			 va_videoviewer = @VideoViewer
GO
/****** Object:  StoredProcedure [dbo].[SetAccessToPrivatePhotos]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SetAccessToPrivatePhotos] 
(
	@PhotoOwner	varchar(20),
	@PhotoViewer	varchar(20),
	@GrantAccess	bit
)
AS
	IF (@GrantAccess = 1)
	BEGIN
		IF ((SELECT COUNT(*)  FROM PhotoAccess WHERE @PhotoOwner = pa_photoowner AND @PhotoViewer = pa_photoviewer) = 0)
			INSERT INTO PhotoAccess
				(pa_photoowner, pa_photoviewer)
			VALUES
				(@PhotoOwner,@PhotoViewer)
	END
	ELSE
		DELETE FROM PhotoAccess WHERE
			 pa_photoowner = @PhotoOwner AND
			 pa_photoviewer = @PhotoViewer
GO
/****** Object:  StoredProcedure [dbo].[SetAccessToPrivateAudio]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetAccessToPrivateAudio]
	(
		@AudioOwner	varchar(20),
		@AudioViewer	varchar(20),
		@GrantAccess	bit
	)
AS
	IF (@GrantAccess = 1)
	BEGIN
		IF NOT EXISTS(SELECT aa_audioowner  FROM AudioAccess WHERE @AudioOwner = aa_audioowner AND @AudioViewer = aa_audioviewer)
			INSERT INTO AudioAccess
				(aa_audioowner, aa_audioviewer)
			VALUES
				(@AudioOwner,@AudioViewer)
	END
	ELSE
		DELETE FROM AudioAccess WHERE
			 aa_audioowner = @AudioOwner AND
			 aa_audioviewer = @AudioViewer
GO
/****** Object:  StoredProcedure [dbo].[SendMessage]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SendMessage]
(
	@FromUsername	VARCHAR(20),
	@ToUsername		VARCHAR(20),
	@Body			NVARCHAR(4000),
	@RepliedTo		INT,
	@PendingApproval	BIT

)
AS

	IF NOT EXISTS (SELECT * FROM MessagesHistoryCache WHERE mhc_fromusername = @FromUsername AND mhc_tousername = @ToUsername)
		INSERT INTO MessagesHistoryCache (mhc_fromusername, mhc_tousername)
			VALUES (@FromUsername, @ToUsername)

	INSERT INTO Messages
		(m_from_username, m_to_username, m_body, m_replied_to, m_pending_approval)
	VALUES
		(@FromUsername, @ToUsername, @Body, @RepliedTo, @PendingApproval)
	SELECT @@IDENTITY
GO
/****** Object:  StoredProcedure [dbo].[SendInterest]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SendInterest]
(
	@FromUsername	VARCHAR(20),
	@ToUsername		VARCHAR(20)
)
AS
	DECLARE @Id int
	DECLARE @DeletedByFromUser bit
	DECLARE @DeletedByToUser bit
	DECLARE @Result int
	

	SELECT @Id = i_id,
		 @DeletedByFromUser = i_deleted_by_fromuser,
		 @DeletedByToUser = i_deleted_by_touser
	FROM Interests 
	WHERE i_from_username = @FromUsername AND 
		 i_to_username = @ToUsername

	IF (@Id IS NOT NULL)
	BEGIN
		IF @DeletedByFromUser = 0
			SET @Result = 0
		ELSE
		BEGIN
			IF @DeletedByToUser = 1
				SET @Result = 1
			ELSE
				SET @Result = 2

			UPDATE Interests
			SET i_date_posted = getdate(), i_deleted_by_touser = 0, i_deleted_by_fromuser = 0
			WHERE i_from_username = @FromUsername AND 
				 i_to_username = @ToUsername
		END
	END
	ELSE
	BEGIN
		SET @Result = 1

		INSERT INTO Interests
			(i_from_username, i_to_username)
		VALUES
			(@FromUsername, @ToUsername)
	END

	SELECT @Result
GO
/****** Object:  StoredProcedure [dbo].[UpdateSubscription]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UpdateSubscription](
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
/****** Object:  StoredProcedure [dbo].[UpdateMessage]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[UpdateMessage]
(
	@Id	INT,
	@FromFolder	INT = NULL,
	@ToFolder	INT = NULL,
	@IsRead	BIT = NULL,
	@PendingApproval BIT = NULL

)
AS
IF (@FromFolder IS NOT NULL)
	UPDATE Messages SET m_from_folder = @FromFolder WHERE m_id = @Id
IF (@ToFolder IS NOT NULL)
	UPDATE Messages SET m_to_folder = @ToFolder WHERE m_id = @Id
IF (@IsRead IS NOT NULL)
	UPDATE Messages SET m_is_read = @IsRead WHERE m_id = @Id
IF (@PendingApproval IS NOT NULL)
BEGIN
	IF (@PendingApproval = 0)
		UPDATE Users SET u_message_verifications_left = u_message_verifications_left - 1 
			WHERE u_username in (SELECT  m_from_username FROM Messages WHERE m_id = @Id)
				AND u_message_verifications_left > 0
	UPDATE Messages SET m_pending_approval = @PendingApproval WHERE m_id = @Id
END
GO
/****** Object:  StoredProcedure [dbo].[TopModeratorsSearch]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TopModeratorsSearch]
	(
		@Gender	INT,
		@MaxTimeAway INT,
		@MaxResults INT
	)
AS
	SET ROWCOUNT @MaxResults

	SELECT
		u_username as Username
	FROM
		Users
	WHERE
		u_gender = @Gender
		AND DATEDIFF(day,u_lastlogin,getdate()) < @MaxTimeAway
		AND EXISTS (
			SELECT TOP 1 p_id
			FROM Photos
			WHERE Photos.u_username = Users.u_username AND p_approved = 1 AND p_private = 0 )
		AND u_profilevisible = 1
		AND u_deleted = 0
	ORDER BY
		u_moderationscore DESC

	SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[UsernameSearchUsers]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UsernameSearchUsers]
(
	@Username	VARCHAR(22),
	@HasAnswer	BIT
)
AS
	SELECT
		u_username as Username
	FROM
		Users
	WHERE
		u_username like @Username
		AND (@HasAnswer IS NULL OR EXISTS ( 
               SELECT pa_value FROM ProfileAnswers 
                    WHERE Users.u_username = ProfileAnswers.u_username))
		AND u_profilevisible = 1
		AND u_active = 1
		AND u_deleted = 0
		AND u_face_control_approved = 1
	ORDER BY u_lastonline DESC
GO
/****** Object:  StoredProcedure [dbo].[SearchPhotos]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchPhotos]
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
	
	FROM Photos
	WHERE 
		(@Id = -1 OR @Id IS NULL OR p_id = @Id)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Approved IS NULL OR p_approved = @Approved)
		AND (@Primary IS NULL OR p_primary = @Primary)
		AND (@Explicit IS NULL OR p_explicit = @Explicit)
		AND (@Private IS NULL OR p_private = @Private)
	ORDER BY 
		p_id DESC
GO
/****** Object:  StoredProcedure [dbo].[SetPhotoAsPrivate]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SetPhotoAsPrivate]
(
	@Username	VARCHAR(20),
	@PhotoID	INT
)
AS
	UPDATE Photos
	SET p_private = 1
	WHERE u_username = @Username
	AND p_id = @PhotoID
GO
/****** Object:  StoredProcedure [dbo].[SetPrimaryPhoto]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SetPrimaryPhoto]
(
	@Username	VARCHAR(20),
	@PhotoID	INT
)
AS
	UPDATE Photos
	SET p_primary = 0
	WHERE u_username = @Username
	AND p_id != @PhotoID
	UPDATE Photos
	SET p_primary = 1
	WHERE u_username = @Username
	AND p_id = @PhotoID
GO
/****** Object:  StoredProcedure [dbo].[TopUsersSearch]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TopUsersSearch]
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
		Users
	WHERE
		u_gender = @Gender
		AND (
			SELECT COUNT(r_rating) 
			FROM Ratings 
			WHERE Ratings.r_tousername = Users.u_username) >= @MinVotes
		AND EXISTS (
			SELECT TOP 1 p_id
			FROM Photos
			WHERE Photos.u_username = Users.u_username AND p_approved = 1 AND p_private = 0 )
		AND DATEDIFF(day,u_lastlogin,getdate()) < @MaxTimeAway
		AND u_profilevisible = 1
		AND u_deleted = 0
	ORDER BY
		(
			dbo.fnFetchAverageRating(Users.u_username)) DESC

	SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[SearchBlogPosts]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchBlogPosts]
	(
		@BlogID INT,
		@Username VARCHAR(20),
		@Title NVARCHAR(200),
		@Content NTEXT,
		@Approved BIT,
		@DatePosted DATETIME,
		@Keyword NVARCHAR(200),
		@SearchContent BIT,
		@NumberOfBlogPosts INT,
		@SortColumn INT
		
	)
AS

IF @NumberOfBlogPosts IS NOT NULL
	SET ROWCOUNT @NumberOfBlogPosts

SELECT
		bp_id AS ID	
FROM	Blogs AS b INNER JOIN BlogPosts AS bp
						ON b.b_id = bp.b_id

WHERE
		(@BlogID IS NULL OR bp.b_id = @BlogID)
		AND (@Username IS NULL OR b.u_username = @Username)
		AND (@Title IS NULL OR bp_title = @Title)
		AND (@Approved IS NULL OR bp_approved = @Approved)
		AND (@DatePosted IS NULL OR bp_dateposted > @DatePosted)
		AND (@Keyword IS NULL OR bp_title like '%' + @Keyword + '%' OR (@SearchContent = 1 AND bp_content like '%' + @Keyword + '%'))

ORDER BY
		CASE WHEN @SortColumn = 1 THEN bp_dateposted END DESC,
		
		CASE WHEN @SortColumn = 2 THEN bp_title END,
		
		CASE WHEN @SortColumn = 3 THEN bp_reads END DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[SaveProfileChoice]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[SaveProfileChoice]
(
	@QuestionID int,
	@ID int=null,
	@Value nvarchar(250)
	
)
 AS
	IF (@ID IS NULL)
		INSERT INTO ProfileChoices
			(pq_id, pc_value)
		VALUES
			(@QuestionID, @Value)
	ELSE
		UPDATE ProfileChoices
		SET
			pc_value = @Value
		WHERE
			pc_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveProfileAnswer]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveProfileAnswer]
(
	@Username	VARCHAR(20),
	@QuestionID	INT,
	@Value		NVARCHAR(4000),
	@Approved	bit
)
AS
	/*DECLARE @requiresApproval bit
	DECLARE @oldValue NVARCHAR(4000)
	SELECT @requiresApproval = pq_requires_approval FROM ProfileQuestions WHERE @QuestionID = pq_id
	SELECT@oldValue = pa_value FROM ProfileAnswers WHERE u_username = @Username AND pq_id = @QuestionID*/
	IF EXISTS (SELECT * FROM ProfileAnswers WHERE u_username = @Username AND pq_id = @QuestionID)
		UPDATE 
			ProfileAnswers
		SET
			pa_value = @Value,
			pa_approved = @Approved
		WHERE
			 u_username = @Username 
			AND  pq_id = @QuestionID				
	ELSE
		INSERT INTO ProfileAnswers
			(u_username, pq_id, pa_value, pa_approved)
		VALUES
			(@Username, @QuestionID, @Value, @Approved)
/*	IF (@Value <> @oldValue)
		IF (@requiresApproval = 1)
			UPDATE 
				ProfileAnswers
			SET
				pa_approved = 0
			WHERE
				 u_username = @Username 
				AND  pq_id = @QuestionID*/
GO
/****** Object:  StoredProcedure [dbo].[SavePhotoContestVote]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePhotoContestVote]
	@Username			VARCHAR(20),
	@PickedEntryId		INT,
	@NonpickedEntryId	INT
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT * FROM PhotoContestVotes WHERE u_username = @Username
		AND pce_id_picked = @PickedEntryId AND pce_id_nonpicked = @NonpickedEntryId)
	BEGIN
		INSERT INTO PhotoContestVotes
			(u_username, pce_id_picked, pce_id_nonpicked)
		VALUES
			(@Username, @PickedEntryId, @NonpickedEntryId)
	END

END
GO
/****** Object:  StoredProcedure [dbo].[SavePhotoContestRank]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePhotoContestRank]
	@Username	VARCHAR(20),
	@ContestId	INT,
	@EntryId	INT,
	@Value		INT
AS
BEGIN
	SET NOCOUNT ON;

	IF NOT EXISTS (SELECT pce_id FROM PhotoContestRanks WHERE
		u_username = @Username AND pc_id = @ContestId AND pce_id = @EntryId)
	BEGIN
		BEGIN TRANSACTION

		-- Shift other rank values
		IF EXISTS (SELECT pce_id FROM PhotoContestRanks WHERE
			u_username = @Username AND pc_id = @ContestId AND pcr_value = @Value)
		BEGIN
			UPDATE PhotoContestRanks
				SET pcr_value = pcr_value + 1
			WHERE
				u_username = @Username AND pc_id = @ContestId 
				AND pcr_value >= @Value
		END

		INSERT INTO PhotoContestRanks
			(u_username, pc_id, pce_id, pcr_value)
		VALUES
			(@Username, @ContestId, @EntryId, @Value)

		COMMIT TRANSACTION
	END
	ELSE
	BEGIN
		UPDATE PhotoContestRanks
		SET 
			pcr_value = @Value
		WHERE
			u_username = @Username AND pc_id = @ContestId AND pce_id = @EntryId
	END
END
GO
/****** Object:  StoredProcedure [dbo].[SavePhoto]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SavePhoto]
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
/****** Object:  StoredProcedure [dbo].[SaveGroupTopicSubscription]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupTopicSubscription]
	(
		@ID		INT,
		@Username VARCHAR(20),
		@GroupTopicID INT,
		@GroupID	INT,
		@DateUpdated DATETIME
	)
AS

IF @ID IS NULL

	BEGIN
		INSERT INTO GroupTopicSubscriptions (u_username, gt_id, g_id, gts_dateupdated)
									VALUES (@Username, @GroupTopicID, @GroupID, @DateUpdated)
											
		SELECT @@Identity
	END

ELSE

	BEGIN
		UPDATE GroupTopicSubscriptions
		SET u_username = @Username,
			gt_id = @GroupTopicID,
			g_id = @GroupID,
			gts_dateupdated = @DateUpdated
			
		WHERE gts_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupPollsChoice]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupPollsChoice] 
	(
		@ID				INT,
		@GroupTopicID	INT,
		@Answer			NVARCHAR(200)
	)
AS

IF (@ID IS NULL)

	BEGIN
		INSERT INTO GroupPollsChoices
		(gt_id, gpc_answer) VALUES (@GroupTopicID, @Answer)
	END

ELSE

	BEGIN
		UPDATE GroupPollsChoices

		SET gt_id = @GroupTopicID,
			gpc_answer = @Answer
			
		WHERE gpc_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveBlogPost]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveBlogPost]
(
	@Id			int = -1,
	@BlogId		int,
	@Title		nvarchar(200),
	@Content	ntext,
	@Approved	bit
)
AS

IF (@Id = -1)
BEGIN

	INSERT INTO BlogPosts
	(b_id, bp_title, bp_content, bp_approved)
	VALUES
	(@BlogId, @Title, @Content, @Approved)
	
	SELECT @@Identity
END
ELSE
	UPDATE BlogPosts
	SET bp_title = @Title, bp_content = @Content, bp_approved = @Approved
	WHERE bp_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupEventsComments]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupEventsComments]
	(
		@ID	INT,
		@GroupEventID	INT,
		@Username		VARCHAR(20),
		@Comment		NTEXT,
		@Date			DATETIME
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO GroupEventsComments (ge_id, u_username, gec_comment, gec_date)
								VALUES (@GroupEventID, @Username, @Comment, @Date)
		SELECT @@Identity
	END
	
ELSE
	BEGIN
		UPDATE GroupEventsComments
		SET ge_id = @GroupEventID,
			u_username = @Username,
			gec_comment = @Comment,
			gec_date = @Date
		WHERE gec_id = @ID
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveAffiliateCommission]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAffiliateCommission] 
	(
		@ID					INT,
		@AffiliateID		INT,
		@PaymentHistoryID	INT,
		@Username			VARCHAR(20),
		@Amount				MONEY,
		@TimeStamp			DATETIME,
		@Notes				NVARCHAR(200)
	)
AS

IF (@ID IS NULL)
	BEGIN
		INSERT INTO AffiliateCommissions (a_id, ph_id, u_username, ac_amount, ac_timestamp, ac_notes)
					VALUES (@AffiliateID, @PaymentHistoryID, @Username, @Amount, @TimeStamp, @Notes)
		
		SELECT @@Identity
	END
ELSE
	BEGIN
		UPDATE AffiliateCommissions
		SET a_id = @AffiliateID,
			ph_id = @PaymentHistoryID,
			u_username = @Username,
			ac_amount = @Amount,
			ac_timestamp = @TimeStamp,
			ac_notes = @Notes
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveAdPhoto]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAdPhoto]
	(
		@ID int,
		@AdID int,
		@Description ntext,
		@Image image
	)
AS

IF @ID IS NULL
	BEGIN
		INSERT INTO AdPhotos (a_id, ap_description, ap_image) VALUES (@AdID, @Description, @Image)
		SELECT @@Identity
	END
ELSE
	UPDATE AdPhotos
	SET a_id = @AdID,
		ap_description = @Description,
		ap_image = @Image
	WHERE ap_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[SaveAdComment]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveAdComment]
	(
		@ID int,
		@AdID int,
		@Username varchar(20),
		@Comment ntext,
		@Date datetime
	)
AS

IF @ID IS NULL
	BEGIN
		INSERT INTO AdComments (a_id, u_username, ac_comment, ac_date)
							VALUES (@AdID, @Username, @Comment, @Date)
		SELECT @@Identity
	END
ELSE
	UPDATE AdComments
	SET a_id = @AdID,
		u_username = @Username,
		ac_comment = @Comment,
		ac_date = @Date
	WHERE ac_id = @ID
GO
/****** Object:  Table [dbo].[PhotoRatings]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoRatings](
	[pr_fromusername] [varchar](20) NOT NULL,
	[p_id] [int] NOT NULL,
	[pr_rating] [int] NOT NULL,
	[pr_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_PhotoRatings] PRIMARY KEY CLUSTERED 
(
	[pr_fromusername] ASC,
	[p_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PhotoNotes]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoNotes](
	[pn_id] [int] IDENTITY(1,1) NOT NULL,
	[p_id] [int] NOT NULL,
	[u_username] [varchar](20) NULL,
	[pn_notes] [nvarchar](200) NULL,
	[pn_timestamp] [datetime] NOT NULL,
	[pn_x] [int] NOT NULL,
	[pn_y] [int] NOT NULL,
	[pn_width] [int] NOT NULL,
	[pn_height] [int] NOT NULL,
 CONSTRAINT [PK_PhotoNotes] PRIMARY KEY CLUSTERED 
(
	[pn_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Trigger [photocontestrank_del_trg]    Script Date: 05/14/2010 18:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[photocontestrank_del_trg] 

ON [dbo].[PhotoContestRanks] 

FOR DELETE

AS

-- declare all variables!
DECLARE @Username VARCHAR(20)
DECLARE @ContestId INT
DECLARE @Value INT
 
-- declare the cursor
DECLARE DeletedRanks CURSOR FOR
SELECT u_username, pc_id, pcr_value
FROM deleted
 
OPEN DeletedRanks

FETCH DeletedRanks INTO @Username, @ContestId, @Value
 
-- start the main processing loop.
WHILE @@Fetch_Status = 0
   BEGIN

	UPDATE PhotoContestRanks
		SET pcr_value = pcr_value - 1
	WHERE
		u_username = @Username
		AND pc_id = @ContestId
		AND pcr_value >= @Value

	FETCH DeletedRanks INTO @Username, @ContestId, @Value

   END

CLOSE DeletedRanks
DEALLOCATE DeletedRanks
GO
/****** Object:  Table [dbo].[PhotoComments]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[PhotoComments](
	[pc_id] [int] IDENTITY(1,1) NOT NULL,
	[p_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[pc_comment] [ntext] NOT NULL,
	[pc_date] [datetime] NOT NULL,
 CONSTRAINT [PK_PhotoComments] PRIMARY KEY CLUSTERED 
(
	[pc_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[KeywordSearchUsers]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[KeywordSearchUsers]
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
/****** Object:  StoredProcedure [dbo].[LoadPhotoContestRanks]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadPhotoContestRanks]
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
		PhotoContestRanks
	WHERE
		(@Username IS NULL OR u_username = @Username)
		AND (@ContestId IS NULL OR pc_id = @ContestId)
		AND (@EntryId IS NULL OR pce_id = @EntryId)
		AND (@Value IS NULL OR pcr_value = @Value)
	ORDER BY
		pcr_value ASC
END
GO
/****** Object:  StoredProcedure [dbo].[LoadPhotoContestEntry]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[LoadPhotoContestEntry]
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
		PhotoContestEntries as pce
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
			OR NOT EXISTS (SELECT pc_id FROM PhotoContestRanks as pcr WHERE 
				pcr.pce_id = pce.pce_id AND u_username = @NotRankedBy))
		AND (@NotRankedBy IS NULL OR u_username != @NotRankedBy)
	ORDER BY 
		CASE 
			WHEN @Randomize = 1 THEN NEWID()
		END

	IF (@CountLimit IS NOT NULL) SET ROWCOUNT 0

END
GO
/****** Object:  StoredProcedure [dbo].[NewUsersSearch]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NewUsersSearch] 
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
          Users 
     WHERE 
          ((@UsersSince IS NULL) OR (u_usersince >= @UsersSince)) 
          AND ((@Gender IS NULL) OR (u_gender = @Gender)) 
          AND ((@PhotoReq = 0) OR (EXISTS ( 
               SELECT TOP 1 p_id 
               FROM Photos 
               WHERE
		 Photos.u_username = Users.u_username AND
		 p_approved = 1 AND
		 p_private = 0
	))) 
          AND ((@ProfileReq = 0) OR (EXISTS ( 
               SELECT pa_value FROM ProfileAnswers 
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
GO
/****** Object:  StoredProcedure [dbo].[HasProfile]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[HasProfile]
(
	@Username varchar(20),
	@Approved bit
) AS

IF (@Approved = 1)
BEGIN
	 IF EXISTS(
		SELECT pa_value FROM ProfileAnswers
			WHERE @Username = ProfileAnswers.u_username AND pa_approved = 1) AND
	NOT EXISTS(
		SELECT pa_value FROM ProfileAnswers
			WHERE @Username = ProfileAnswers.u_username AND pa_approved = 0)
				
		SELECT CAST (1 AS BIT)
	ELSE
		SELECT CAST(0 AS BIT)
END
ELSE
BEGIN
	DECLARE @Gender INT
	SET @Gender = (SELECT u_gender FROM Users WHERE u_username=@Username)

	IF ( (SELECT COUNT(*) FROM ProfileQuestions WHERE pq_required=1 AND 
				CASE @Gender
					WHEN 1 THEN pq_visible_male
					WHEN 2 THEN pq_visible_female	
					WHEN 3 THEN pq_visible_couple 
				END = 1) > 0 )
	BEGIN
		IF EXISTS(SELECT pq_id from ProfileQuestions WHERE pq_required=1 AND 
				CASE @Gender
					WHEN 1 THEN pq_visible_male
					WHEN 2 THEN pq_visible_female	
					WHEN 3 THEN pq_visible_couple 
				END = 1
				AND pq_id NOT IN 
				(SELECT pq_id FROM ProfileAnswers WHERE 
				u_username = @Username)
			)
			SELECT CAST(0 AS BIT)
		ELSE
			SELECT CAST (1 AS BIT)
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT pa_value FROM ProfileAnswers WHERE u_username=@Username)
			SELECT CAST (1 AS BIT)
		ELSE
			SELECT CAST (0 AS BIT)
	END	
END
GO
/****** Object:  StoredProcedure [dbo].[HasPrivatePhotos]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[HasPrivatePhotos]
(
	@Username varchar(20)
)
 AS
	IF ((SELECT COUNT(*)  FROM Photos WHERE @Username = u_username AND p_private = 1) <> 0)
		SELECT CAST ( 1 AS BIT)
	ELSE
		SELECT CAST( 0 AS BIT)
GO
/****** Object:  StoredProcedure [dbo].[IncreaseBlogPostReadCounter]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IncreaseBlogPostReadCounter]
	@Id	INT
AS
BEGIN

	UPDATE BlogPosts
	SET bp_reads = bp_reads + 1
	WHERE bp_id = @Id

END
GO
/****** Object:  UserDefinedFunction [dbo].[fnHasProfile]    Script Date: 05/14/2010 18:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fnHasProfile]
	(
		@Username varchar(20),
		@Approved bit
	)
RETURNS bit
AS
	BEGIN
	DECLARE @HasProfile BIT
	IF (@Approved = 1)
BEGIN
	 IF EXISTS(
		SELECT pa_value FROM ProfileAnswers
			WHERE @Username = ProfileAnswers.u_username AND pa_approved = 1) AND
	NOT EXISTS(
		SELECT pa_value FROM ProfileAnswers
			WHERE @Username = ProfileAnswers.u_username AND pa_approved = 0)
				
		SET @HasProfile = 1
	ELSE
		SET @HasProfile = 0
END
ELSE
BEGIN
	DECLARE @Gender INT
	SET @Gender = (SELECT u_gender FROM Users WHERE u_username=@Username)

	IF ( (SELECT COUNT(*) FROM ProfileQuestions WHERE pq_required=1 AND 
				CASE @Gender
					WHEN 1 THEN pq_visible_male
					WHEN 2 THEN pq_visible_female	
					WHEN 3 THEN pq_visible_couple 
				END = 1) > 0 )
	BEGIN
		IF EXISTS(SELECT pq_id from ProfileQuestions WHERE pq_required=1 AND 
				CASE @Gender
					WHEN 1 THEN pq_visible_male
					WHEN 2 THEN pq_visible_female	
					WHEN 3 THEN pq_visible_couple 
				END = 1
				AND pq_id NOT IN 
				(SELECT pq_id FROM ProfileAnswers WHERE 
				u_username = @Username)
			)
			SET @HasProfile = 0
		ELSE
			SET @HasProfile = 1
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT pa_value FROM ProfileAnswers WHERE u_username=@Username)
			SET @HasProfile = 1
		ELSE
			SET @HasProfile = 0
	END	
END
	RETURN @HasProfile
	END
GO
/****** Object:  Table [dbo].[GroupPollsAnswers]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupPollsAnswers](
	[gt_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gpc_id] [int] NOT NULL,
 CONSTRAINT [PK_GroupPollsAnswers] PRIMARY KEY CLUSTERED 
(
	[gt_id] ASC,
	[u_username] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupEventsComments]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupEventsComments]
	(
		@ID				INT,
		@GroupEventID	INT,
		@Username		VARCHAR(20),
		@SortColumn		INT
	)
AS

SELECT	gec_id AS ID,
		ge_id AS GroupEventID,
		u_username AS Username,
		gec_comment AS Comment,
		gec_date AS [Date]
FROM GroupEventsComments
WHERE	(@ID IS NULL OR gec_id = @ID)
		AND (@GroupEventID IS NULL OR ge_id = @GroupEventID)
		AND (@Username IS NULL OR u_username = @Username)

ORDER BY
		CASE WHEN @SortColumn = 1 THEN u_username END,
		CASE WHEN @SortColumn = 2 THEN gec_date END
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupEventsAttenders]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupEventsAttenders]
	(
		@GroupEventID	INT,
		@Username		VARCHAR(20)
	)
AS

SELECT u_username AS Username
FROM GroupEventsAttenders
WHERE	(@GroupEventID IS NULL OR ge_id = @GroupEventID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupPollsChoices]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupPollsChoices] 
	(
		@ID INT,
		@GroupTopicID INT
	)
AS

SELECT	gpc_id AS ID,
		gt_id AS GroupTopicID,
		gpc_answer AS Answer
		
FROM GroupPollsChoices

WHERE	(@ID IS NULL OR gpc_id = @ID)
		AND (@GroupTopicID IS NULL OR gt_id = @GroupTopicID)
ORDER BY gpc_id
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupTopicSubscriptions]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupTopicSubscriptions] 
	(
		@ID				INT,
		@Username		VARCHAR(20),
		@GroupTopicID	INT,
		@GroupID		INT,
		@DateUpdated	DATETIME
	)
AS

SELECT	gts_id AS [ID],
		gt_id AS GroupTopicID,
		g_id AS GroupID,
		u_username AS Username,
		gts_dateupdated AS DateUpdated
		
FROM GroupTopicSubscriptions

WHERE	(@ID IS NULL OR gts_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@GroupTopicID IS NULL OR gt_id = @GroupTopicID)
		AND (@GroupID IS NULL OR g_id = @GroupID)
		AND (@DateUpdated IS NULL OR gts_dateupdated = @DateUpdated)
GO
/****** Object:  StoredProcedure [dbo].[FetchNonApprovedAnswers]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchNonApprovedAnswers]
AS
	SELECT
		u_username	as Username,
		pq_id		as QuestionID,
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers
	WHERE
		pa_approved=0
GO
/****** Object:  StoredProcedure [dbo].[FetchFavoritesNewPhotos]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchFavoritesNewPhotos]
	(
		@Username VARCHAR(20),
		@FromDate DATETIME
	)
AS

SELECT DISTINCT f.f_username AS Username
FROM favourites as f, photos as p
WHERE	f.u_username = @Username
		AND f.f_username = p.u_username
		AND p.p_approved = 1
		AND p.p_approvedtimestamp > @FromDate
GO
/****** Object:  StoredProcedure [dbo].[FetchFavoritesNewBlogPosts]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchFavoritesNewBlogPosts]
	(
		@Username VARCHAR(20),
		@FromDate DATETIME
	)
AS

SELECT DISTINCT f.f_username AS Username, bp_id AS BlogPostID
FROM Favourites AS f, Blogs AS b, BlogPosts AS bp
WHERE f.f_username = b.u_username
	AND b.b_id = bp.b_id
	AND f.u_username = @Username
	AND bp.bp_dateposted > @FromDate
GO
/****** Object:  StoredProcedure [dbo].[FetchDetailsOfNonApprovedPhotos]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchDetailsOfNonApprovedPhotos]
 AS
	SELECT
		u_username	as Username,
		p_name		as Name,
		p_description	as Description,
		p_id		as PhotoID,
		p_explicit	as [Explicit],
		p_private	as Private
	FROM
		Photos
	WHERE
		p_approved=0
GO
/****** Object:  StoredProcedure [dbo].[FetchAffiliateCommissions]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAffiliateCommissions] 
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
/****** Object:  StoredProcedure [dbo].[FetchBlogPosts]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchBlogPosts]
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
	BlogPosts
WHERE
	(@BlogId IS NULL OR @BlogId = -1 OR b_id = @BlogId)
	AND (@BlogPostId IS NULL OR @BlogPostId = -1 OR bp_id = @BlogPostId)
	AND (@Approved IS NULL OR bp_approved = @Approved)
ORDER BY
	bp_dateposted DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileChoiceByQuestion]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchProfileChoiceByQuestion]
(
	@QuestionID	int = null
)
AS
	SELECT
		pc_id		as ID,
		pc_value	as Value
	FROM
		ProfileChoices
	WHERE
		pq_id = @QuestionID
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileChoice]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchProfileChoice]
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
			ProfileChoices
	ELSE
		SELECT
			pq_id		as QuestionID,
			pc_value	as Value
		FROM
			ProfileChoices
		WHERE
			pc_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileAnswerByUser]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchProfileAnswerByUser]
(
	@Username	VARCHAR(20)
)
AS
	SELECT
		pq_id		as QuestionID,
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers
	WHERE
		u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileAnswerByQuestion]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchProfileAnswerByQuestion]
(
	@QuestionID	int
)
AS
	SELECT
		u_username	as Username,
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers
	WHERE
		pq_id = @QuestionID
GO
/****** Object:  StoredProcedure [dbo].[FetchProfileAnswer]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[FetchProfileAnswer]
(
	@Username	varchar(20),
	@QuestionID	int
)
AS
	SELECT
		pa_value	as Value,
		pa_approved	as Approved
	FROM
		ProfileAnswers
	WHERE
		u_username = @Username
		AND pq_id = @QuestionID
GO
/****** Object:  StoredProcedure [dbo].[FetchPrimaryPhotoDetails]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPrimaryPhotoDetails]
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
		Photos
	WHERE
		u_username = @Username AND
		p_approved = 1 AND 
		p_private = 0
	ORDER BY
		p_primary DESC, p_explicit ASC
GO
/****** Object:  StoredProcedure [dbo].[FetchPhoto]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPhoto]
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
GO
/****** Object:  StoredProcedure [dbo].[FetchUsersInRadius]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchUsersInRadius]
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
	FROM Users
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
               SELECT pa_value FROM ProfileAnswers 
                    WHERE Users.u_username = ProfileAnswers.u_username)) AND
		 u_gender = @Gender AND
		 u_birthdate >= @FromBirthdate AND
		 u_birthdate <= @ToBirthdate AND
		 (@PhotoReq = 0 OR EXISTS (
			SELECT p_id FROM Photos 
			WHERE Users.u_username = Photos.u_username
			AND Photos.p_approved = 1)) AND			 
		 u_active = 1 AND
		 u_deleted=0 AND
		 u_profilevisible=1
		 AND u_face_control_approved = 1
	ORDER BY u_country, u_state, u_city

	SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchTopEntries]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchTopEntries]
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
		PhotoContestRanks as R1,
		PhotoContestEntries as E1,
		Users as U1,
		Users as U2
	WHERE
		R1.pc_id = @ContestId AND R1.pcr_value <= @FavCount
		AND (SELECT COUNT(*) FROM PhotoContestRanks as R2 
			WHERE R1.u_username = R2.u_username) >= @FavCount
		AND R1.u_username = U1.u_username
		AND E1.pce_id = R1.pce_id
		AND E1.u_username = U2.u_username
	GROUP BY
		R1.pce_id
	ORDER BY
		Score DESC
		
END
GO
/****** Object:  StoredProcedure [dbo].[FetchPhotoDetails]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPhotoDetails]
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
			Photos
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
			Photos AS p, Users AS u
		WHERE
			(@Id = -1 OR @Id IS NULL OR p_id = @Id)
			AND (@Username IS NULL OR p.u_username = @Username)
			AND (@Approved IS NULL OR p.p_approved = @Approved)
			AND (@ManualApproved IS NULL OR p.p_manual_approval = @ManualApproved)
			AND  p.u_username = u.u_username AND u.u_face_control_approved = @FaceControlEnabled
	END
GO
/****** Object:  StoredProcedure [dbo].[FetchPhotoContestVotePercentage]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPhotoContestVotePercentage]
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
		PhotoContestVotes
	WHERE
		pce_id_picked = @Entry1Id AND pce_id_nonpicked = @Entry2Id

	SELECT
		@Picked2Count = COUNT(pce_id_picked)
	FROM
		PhotoContestVotes
	WHERE
		pce_id_picked = @Entry2Id AND pce_id_nonpicked = @Entry1Id

	IF (@Picked1Count = 0 AND @Picked2Count = 0) 
		SELECT NULL
	ELSE
		SELECT (100.0 / CAST((@Picked1Count + @Picked2Count) AS DECIMAL)) 
				* CAST(@Picked1Count AS DECIMAL)

END
GO
/****** Object:  StoredProcedure [dbo].[FetchUpdatedGroupTopicsUserHasSubscribedTo]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchUpdatedGroupTopicsUserHasSubscribedTo] 
	(
		@Username	VARCHAR(20)
	)
AS
	SELECT	GroupTopics.gt_id AS [ID],
			GroupTopics.g_id AS GroupID,
			gt_name AS [Name]
	FROM
		GroupTopicSubscriptions, GroupTopics
	WHERE 
		GroupTopicSubscriptions.gt_id = GroupTopics.gt_id
		AND GroupTopicSubscriptions.gts_dateupdated != GroupTopics.gt_dateupdated
		AND GroupTopicSubscriptions.u_username = @Username
GO
/****** Object:  Table [dbo].[CommunityPhotoApproval]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CommunityPhotoApproval](
	[cpa_id] [int] IDENTITY(1,1) NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[p_id] [int] NOT NULL,
	[cpa_approved] [bit] NOT NULL,
	[cpa_timestamp] [datetime] NOT NULL,
 CONSTRAINT [PK_CommunityPhotoApproval] PRIMARY KEY CLUSTERED 
(
	[cpa_id] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BlogPostComments]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BlogPostComments](
	[bpc_id] [int] IDENTITY(1,1) NOT NULL,
	[bp_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[bpc_text] [ntext] NOT NULL,
	[bpc_dateposted] [datetime] NOT NULL,
	[bpc_approved] [bit] NOT NULL,
 CONSTRAINT [PK_BlogPostComments] PRIMARY KEY CLUSTERED 
(
	[bpc_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[BasicSearchUsers]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BasicSearchUsers]
(
	@Username		VARCHAR(20) = NULL,
	@Name			NVARCHAR(250) = NULL,
	@Gender			INT = NULL,
	@InterestedIn	INT = NULL,
	@FromBirthdate	DATETIME = NULL,
	@ToBirthdate	DATETIME = NULL,
	@City		NVARCHAR(100) = null,
	@Country	VARCHAR(100) = null,
	@State		NVARCHAR(100) = null,
	@Zip		VARCHAR(15) = null,
	@IP			VARCHAR(15) = NULL,
	@Email		VARCHAR(250) = NULL,
	@SpamSuspected BIT = NULL,
	@FaceControlApproved BIT = NULL,
	@HasPhoto	BIT = NULL,
	@HasAnswer	BIT = NULL,
	@Visible	BIT = NULL,
	@Active		BIT = NULL,
	@Deleted	BIT = NULL,
	@Paid		BIT = NULL,
	@LanguageID INT,
	@Date DATETIME, 
	@SortColumn VARCHAR(20)
)
AS
	SELECT
		Users.u_username as Username
	FROM
		Users
	WHERE
		(@Username IS NULL OR u_username like '%' + @Username + '%')
		AND (@Name IS NULL OR u_name like '%' + @Name + '%')
		AND (@Gender IS NULL OR u_gender = @Gender)
		AND (@InterestedIn IS NULL OR @InterestedIn = u_interested_in)
		AND (@FromBirthdate IS NULL OR (
				u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		AND (@ToBirthdate IS NULL OR (
				u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
		AND (@HasPhoto IS NULL OR (
				@HasPhoto = 1 AND EXISTS (
				SELECT p_id FROM Photos 
				WHERE Users.u_username = Photos.u_username
				AND Photos.p_approved = 1)) OR (
				@HasPhoto = 0 AND NOT EXISTS (
				SELECT p_id FROM Photos 
				WHERE Users.u_username = Photos.u_username
				AND Photos.p_approved = 1)))
		AND (@City IS NULL OR @City = '' OR u_city = @City)
		AND (@Country IS NULL OR @Country = '' OR u_country = @Country)
		AND (@State IS NULL OR @State = '' OR u_state = @State)
		AND (@Zip IS NULL OR @Zip = '' OR u_zip_code = @Zip)
		AND (@HasAnswer IS NULL OR (
				@HasAnswer = 1 AND EXISTS (
				SELECT pa_value FROM ProfileAnswers
				WHERE Users.u_username = ProfileAnswers.u_username)) OR (
				@HasAnswer = 0 AND NOT EXISTS (
				SELECT pa_value FROM ProfileAnswers
				WHERE Users.u_username = ProfileAnswers.u_username)))
		AND (@IP IS NULL OR u_signup_ip = @IP)
		AND (@Email IS NULL OR u_email = @Email)
		AND (@SpamSuspected IS NULL OR u_spamsuspected = @SpamSuspected)
		AND (@FaceControlApproved IS NULL OR u_face_control_approved = @FaceControlApproved)
		AND (@Visible IS NULL OR u_profilevisible = @Visible)
		AND (@Active IS NULL OR u_active = @Active)
		AND (@Deleted IS NULL OR u_deleted = @Deleted)
		AND (@Paid IS NULL OR u_paid_member = @Paid)
		AND (@LanguageID IS NULL OR l_id = @LanguageID)
		AND (@Date IS NULL OR u_lastlogin < @Date)
		
	ORDER BY
		CASE @SortColumn
			WHEN 'Username' THEN u_username
			WHEN 'Name' THEN u_name
			WHEN 'Email' THEN u_email
			WHEN 'SignupIP' THEN u_signup_ip
		END,
		CASE @SortColumn
			WHEN 'Gender' THEN u_gender
		END,		
		CASE @SortColumn
			WHEN 'LastOnline' THEN u_lastonline
			WHEN 'SignupDate' THEN u_usersince
		END
GO
/****** Object:  StoredProcedure [dbo].[AssignRemoveAttenderToFromGroupEvent]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AssignRemoveAttenderToFromGroupEvent]
	(
		@GroupEventID	INT,
		@Username		VARCHAR(20),
		@Action			INT
	)
AS

IF @Action = 1	--assign

	BEGIN
--check is GroupEventID and Username are valid and the pair doesn't exist in DB
		IF	EXISTS
			(SELECT ge_id
			 FROM GroupEvents
			 WHERE ge_id = @GroupEventID)
			AND EXISTS
			(SELECT u_username
			 FROM Users
			 WHERE u_username = @Username)
			AND NOT EXISTS
			(SELECT * 
			 FROM GroupEventsAttenders
			 WHERE ge_id = @GroupEventID AND u_username = @Username)
		INSERT INTO GroupEventsAttenders
		(ge_id, u_username)
		VALUES
		(@GroupEventID, @Username)
			 
	END
			
ELSE			--remove	

	BEGIN
		DELETE FROM GroupEventsAttenders
		WHERE ge_id = @GroupEventID AND u_username = @Username
	END
GO
/****** Object:  StoredProcedure [dbo].[ApprovePhoto]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ApprovePhoto]
(
	@ID	int,
	@Explicit bit,
	@Private bit,
	@AdminUsername varchar(20)
)
AS
	UPDATE Photos
	SET
		p_approved = 1,
		p_explicit = @Explicit,
		p_private = @Private,
		p_approvedby = @AdminUsername,
		p_approvedtimestamp = getdate()
	WHERE
		p_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[ApplyAffiliateCommission]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplyAffiliateCommission] 
	(
		@AffiliateID			INT,
		@PaymentHistoryID		INT,
		@Username				VARCHAR(20),
		@Commission				MONEY,
		@Notes					NVARCHAR(2000)
	)
AS

BEGIN TRAN

	INSERT INTO AffiliateCommissions (a_id, ph_id, u_username, ac_amount, ac_notes)
							VALUES	(@AffiliateID, @PaymentHistoryID, @Username, @Commission, @Notes)
							
	UPDATE Affiliates
	SET a_balance = a_balance + @Commission
	WHERE a_id = @AffiliateID
	
COMMIT
GO
/****** Object:  StoredProcedure [dbo].[FetchAdPhotos]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAdPhotos]
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
FROM AdPhotos
WHERE	(@ID IS NULL OR ap_id = @ID)
		AND (@AdID IS NULL OR a_id = @AdID)
ORDER BY ap_id

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchAdPhoto]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAdPhoto]
	(
		@ID int
	)
AS

SELECT ap_image as [Image]
FROM AdPhotos
WHERE ap_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchAdComments]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchAdComments]
	(
		@ID int,
		@AdID int,
		@Username varchar(20),
		@Date datetime,
		@NumberOfComments int,
		@SortColumn int
	)
AS

IF @NumberOfComments IS NOT NULL
	SET ROWCOUNT @NumberOfComments
	
SELECT	ac_id as [ID],
		a_id as AdID,
		u_username as Username,
		ac_comment as Comment,
		ac_date as [Date]
FROM AdComments
WHERE	(@ID IS NULL OR ac_id = @ID)
		AND (@AdID IS NULL OR a_id = @AdID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@Date IS NULL OR ac_date = @Date)
ORDER BY
		CASE WHEN @SortColumn = 1 THEN ac_date END DESC
		
SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[EventsCleanup]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[EventsCleanup]
	(
		@Date DATETIME
	)
AS

DELETE FROM EventComments
WHERE e_id in (SELECT e_id FROM Events WHERE e_date < @Date)

DELETE FROM Events
WHERE e_date < @Date
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupTopicSubscription]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupTopicSubscription] 
	(
		@ID	INT
	)
AS

DELETE FROM GroupTopicSubscriptions
WHERE gts_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteQuestion]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteQuestion]
(
	@ID int
)
 AS
BEGIN TRANSACTION
	DELETE FROM ProfileAnswers
	WHERE
		pq_id = @ID

	DELETE FROM ProfileChoices
	WHERE
		pq_id = @ID

	DELETE FROM ProfileQuestions
	WHERE
		pq_id = @ID
COMMIT TRANSACTION
GO
/****** Object:  StoredProcedure [dbo].[DeletePhotoContestEntry]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeletePhotoContestEntry]
	@Id			INT
AS
BEGIN
	
	BEGIN TRANSACTION

	DELETE FROM PhotoContestVotes
	WHERE pce_id_picked = @Id OR pce_id_nonpicked = @Id

	DELETE FROM PhotoContestRanks
	WHERE pce_id = @Id

	DELETE FROM PhotoContestEntries
	WHERE pce_id = @Id

	COMMIT TRANSACTION

END
GO
/****** Object:  StoredProcedure [dbo].[DeletePhotoContest]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeletePhotoContest]
	(
		@ID			int
	)
AS
BEGIN TRAN
	DELETE FROM PhotoContestRanks WHERE pc_id = @ID
	DELETE FROM PhotoContestVotes WHERE 
		pce_id_picked in 
			(SELECT pce_id FROM PhotoContestEntries WHERE pc_id = @ID)
	DELETE FROM PhotoContestEntries WHERE pc_id = @ID
	DELETE FROM PhotoContests WHERE pc_id = @ID
COMMIT
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupEventsComments]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupEventsComments]
	(
		@ID				INT,
		@GroupEventID	INT,
		@Username		VARCHAR(20)
	)
AS

DELETE FROM GroupEventsComments
WHERE	(@ID IS NULL OR gec_id = @ID)
		AND (@GroupEventID IS NULL OR ge_id = @GroupEventID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupEvent]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupEvent] 
	(
		@ID	INT
	)
AS

DELETE FROM GroupEventsAttenders
WHERE ge_id = @ID

DELETE FROM GroupEventsComments
WHERE ge_id = @ID

DELETE FROM GroupEvents
WHERE ge_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupPollsChoice]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupPollsChoice] 
	(
		@ID INT
	)
AS

DELETE FROM GroupPollsChoices
WHERE gpc_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteAffiliateCommission]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAffiliateCommission] 
	(
		@ID					INT,
		@AffiliateID		INT,
		@PaymentHistoryID	INT,
		@Username			VARCHAR(20)
	)
AS

DELETE FROM AffiliateCommissions
WHERE	(@ID IS NULL OR ac_id = @ID)
		AND (@AffiliateID IS NULL OR a_id = @AffiliateID)
		AND (@PaymentHistoryID IS NULL OR ph_id = @PaymentHistoryID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeleteAdComment]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAdComment]
	(
		@ID int,
		@AdID int,
		@Username varchar(20)
	)
AS

DELETE FROM AdComments
WHERE	(@ID IS NULL OR ac_id = @ID)
		AND (@AdID IS NULL OR a_id = @AdID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeleteAd]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAd]
	(
		@ID int,
		@CategoryID int,
		@PostedBy varchar(20)
	)
AS

DELETE FROM AdPhotos WHERE a_id = @ID

DELETE FROM AdComments WHERE a_id = @ID

DELETE FROM AdComments WHERE u_username = @PostedBy

DELETE FROM Ads
WHERE (@ID IS NULL OR a_id = @ID)
		AND (@CategoryID IS NULL OR ac_id = @CategoryID)
		AND (@PostedBy IS NULL OR a_postedby = @PostedBy)
GO
/****** Object:  StoredProcedure [dbo].[DeleteAdPhoto]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteAdPhoto]
	(
		@ID int,
		@AdID int
	)
AS

DELETE FROM AdPhotos
WHERE	(@ID IS NULL OR ap_id = @ID)
		AND (@AdID IS NULL OR a_id = @AdID)
GO
/****** Object:  StoredProcedure [dbo].[DeleteChoice]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteChoice]
(
	@ID int
)
 AS
	DELETE FROM ProfileChoices
	WHERE 
		pc_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[DeleteAnswer]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteAnswer]
(
	@Username	VARCHAR(20),
	@QuestionID	INT
)
 AS
	DELETE FROM
		ProfileAnswers
	WHERE
		@Username = u_username AND @QuestionID = pq_id
GO
/****** Object:  StoredProcedure [dbo].[DeleteBlogPostComment]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteBlogPostComment]
(
	@Id	INT
)
AS

DELETE FROM BlogPostComments
WHERE bpc_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[DeleteBlogPost]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteBlogPost] 
	@Id	INT
AS
BEGIN
	DELETE FROM BlogPostComments
	WHERE bp_id = @Id
	DELETE FROM BlogPosts
	WHERE bp_id = @Id

END
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupPollsAnswer]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupPollsAnswer] 
	(
		@GroupTopicID	INT,
		@Username		VARCHAR(20)
	)
AS

DELETE FROM GroupPollsAnswers
WHERE gt_id = @GroupTopicID AND u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[DeleteCommunityPhotoApproval]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteCommunityPhotoApproval]
	(
		@ID INT,
		@PhotoID INT,
		@Username VARCHAR(20)
	)
AS

DELETE FROM CommunityPhotoApproval

WHERE	(@ID IS NULL OR cpa_id = @ID)
		AND (@PhotoID IS NULL OR p_id = @PhotoID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeletePhotoComments]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[DeletePhotoComments]
	(
		@ID			INT,
		@PhotoID	INT,
		@Username	VARCHAR(20)
	)
AS

DELETE FROM PhotoComments
WHERE	(@ID IS NULL OR pc_id = @ID)
		AND (@PhotoID IS NULL OR p_id = @PhotoID)
		AND (@Username IS NULL OR u_username = @Username)
GO
/****** Object:  StoredProcedure [dbo].[DeletePhotoAlbum]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeletePhotoAlbum]
	(
		@ID int
	)
AS

BEGIN TRANSACTION
	
	DELETE FROM PhotoRatings WHERE p_id IN (SELECT p_id FROM Photos WHERE pa_id = @ID)

	DELETE FROM PhotoComments WHERE p_id IN (SELECT p_id FROM Photos WHERE pa_id = @ID)

	DELETE FROM CommunityPhotoApproval WHERE p_id IN (SELECT p_id FROM Photos WHERE pa_id = @ID)
	
	DELETE FROM Photos WHERE pa_id = @ID
	
	DELETE FROM PhotoAlbums WHERE pa_id = @ID
		
COMMIT TRANSACTION
GO
/****** Object:  StoredProcedure [dbo].[DeletePhoto]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeletePhoto] 
(
	@ID int
)
AS
BEGIN TRANSACTION
	DELETE FROM PhotoRatings WHERE p_id=@ID

	DELETE FROM PhotoComments WHERE p_id = @ID
	
	DELETE FROM CommunityPhotoApproval WHERE p_id = @ID

	DELETE FROM Photos
	WHERE
		p_id = @ID
COMMIT TRANSACTION
GO
/****** Object:  StoredProcedure [dbo].[DeleteTopic]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DeleteTopic]
(
	@ID int
)
AS

BEGIN TRANSACTION
	--Calling DeleteQuestion for each questions of that topic
	--------------------------------------------------------------------------------------------------
	DECLARE @qid int
	DECLARE get_questionid CURSOR FOR
	 SELECT pq_id FROM ProfileQuestions WHERE pt_id = @ID
	
	OPEN get_questionid
	FETCH NEXT FROM get_questionid INTO @qid
	WHILE @@FETCH_STATUS = 0
	BEGIN
		EXEC DeleteQuestion @qid
 		FETCH NEXT FROM get_questionid INTO @qid
	END
	CLOSE get_questionid
	DEALLOCATE get_questionid
	---------------------------------------------------------------------------------------------------

	DELETE FROM 	ProfileTopics
	WHERE
		pt_id = @ID
COMMIT TRANSACTION
GO
/****** Object:  StoredProcedure [dbo].[FetchTopPhotoDetails]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchTopPhotoDetails]
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
		               FROM PhotoRatings 
		               WHERE 
				(SELECT count(*) FROM PhotoRatings WHERE PhotoRatings.p_id = Photos.p_id AND
				Photos.u_username = @Username) >= @MinVotes AND
				PhotoRatings.p_id = Photos.p_id AND
				Photos.u_username = @Username
		) as AvgRating
	FROM
		Photos
	WHERE
		u_username = @Username AND
		p_approved = 1 AND 
		p_private = 0
	ORDER BY
		AvgRating DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchPhotoComments]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPhotoComments]
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

FROM PhotoComments

WHERE	(@ID IS NULL OR pc_id = @ID)
		AND (@PhotoID IS NULL OR p_id = @PhotoID)
		AND (@Username IS NULL OR u_username = @Username)
		
ORDER BY pc_date DESC

SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[FetchNonFaceControlApprovedUser]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNonFaceControlApprovedUser]
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

	FROM Users AS u
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
GO
/****** Object:  StoredProcedure [dbo].[FetchNonApprovedPhoto]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNonApprovedPhoto]
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
/****** Object:  StoredProcedure [dbo].[FetchPhotoVote]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPhotoVote]
(
	@FromUser	VARCHAR(20),
	@PhotoID	INT
)
AS
	SELECT 
		pr_rating as Vote
	FROM
		PhotoRatings
	WHERE
		pr_fromusername = @FromUser
		AND p_id = @PhotoID
GO
/****** Object:  StoredProcedure [dbo].[FetchPhotoRating]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchPhotoRating]
(
	@ID	int
)
AS
SELECT
	COUNT(pr_rating) as Votes,
	AVG(CAST(pr_rating as decimal)) as AverageVote
FROM
	PhotoRatings
WHERE
	p_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[FetchCommunityPhotoApproval]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchCommunityPhotoApproval]
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
		
FROM CommunityPhotoApproval

WHERE	(@ID IS NULL OR cpa_id = @ID)
		AND (@PhotoID IS NULL OR p_id = @PhotoID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@FromDate IS NULL OR cpa_timestamp >= @FromDate) AND (@ToDate IS NULL OR cpa_timestamp <= @ToDate)

ORDER BY

CASE WHEN @SortColumn = 1 THEN u_username END,
CASE WHEN @SortColumn = 2 THEN cpa_timestamp END DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchBlogPostComments]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchBlogPostComments]
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
	BlogPostComments
WHERE
	(@Id IS NULL OR @Id = -1 OR bpc_id = @Id)
	AND (@BlogPostId IS NULL OR @BlogPostId = -1 OR bp_id = @BlogPostId)
	AND (@Username IS NULL OR u_username = @Username)
	AND (@Approved IS NULL OR bpc_approved = @Approved)
ORDER BY
	bpc_dateposted DESC
GO
/****** Object:  StoredProcedure [dbo].[FetchNewPhotoCommentsCount]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchNewPhotoCommentsCount]
	(
		@Username	VARCHAR(20)
	)
AS
DECLARE @LastOnline datetime

SELECT @LastOnline = u_prevlogin FROM Users where u_username = @Username

SELECT COUNT(1) AS PhotoComments
FROM PhotoComments AS pc INNER JOIN Photos AS p
		ON pc.p_id = p.p_id 
WHERE p.u_username = @Username AND pc.pc_date > @LastOnline
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupPollsAnswersNumber]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupPollsAnswersNumber] 
	(
		@TopicID	INT
	)
AS

SELECT gpc_id AS GroupPollsChoiceID, COUNT(1) AS NumberOfAnswers FROM GroupPollsAnswers

WHERE gt_id = @TopicID

GROUP BY gpc_id
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupPollsAnswers]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupPollsAnswers] 
	(
		@GroupTopicID		INT,
		@Username			VARCHAR(20),
		@GroupPollChoiceID	INT
	)
AS

SELECT	gt_id AS GroupTopicID,
		u_username AS Username,
		gpc_id AS GroupPollChoiceID
		
FROM GroupPollsAnswers

WHERE	(@GroupTopicID IS NULL OR gt_id = @GroupTopicID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@GroupPollChoiceID IS NULL OR gpc_id = @GroupPollChoiceID)
GO
/****** Object:  StoredProcedure [dbo].[GetRandomPhotoId]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetRandomPhotoId]
	(
		@Username VARCHAR(20),
		@Gender INT,
		@FromBirthdate	DATETIME,
		@ToBirthdate	DATETIME,
		@MaxTimeAway	INT
	)
AS

SELECT TOP 1 Photos.p_id
FROM Photos INNER JOIN Users ON Photos.u_username = Users.u_username
WHERE	p_approved = 1
		AND p_explicit = 0
		AND p_private = 0
		AND (@Username IS NULL OR Photos.u_username != @Username)
		AND (@Gender IS NULL OR u_gender = @Gender)
		AND (@FromBirthdate IS NULL OR (
				u_birthdate >= @FromBirthdate AND (u_gender != 3 OR u_birthdate2 >= @FromBirthdate)))
		  AND (@ToBirthdate IS NULL OR (
				u_birthdate <= @ToBirthdate AND (u_gender != 3 OR u_birthdate2 <= @ToBirthdate)))
		AND NOT EXISTS (SELECT 1 FROM PhotoRatings WHERE pr_fromusername = @Username AND Photos.p_id = PhotoRatings.p_id)
		AND (u_options & 8 != 8)
		-- VisitorsCanViewPhotos is enabled OR UsersCanViewPhotos is enabled
		AND (u_options & 4096 = 4096 OR u_options & 8192 = 8192)
		AND (@MaxTimeAway = 0 OR DATEDIFF(day,u_lastlogin,getdate()) < @MaxTimeAway)
ORDER BY NEWID()
GO
/****** Object:  Table [dbo].[GroupPosts]    Script Date: 05/14/2010 18:01:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GroupPosts](
	[gp_id] [int] IDENTITY(1,1) NOT NULL,
	[gt_id] [int] NOT NULL,
	[u_username] [varchar](20) NOT NULL,
	[gp_dateposted] [datetime] NOT NULL,
	[gp_dateedited] [datetime] NULL,
	[gp_editnotes] [nvarchar](200) NULL,
	[gp_post] [ntext] NOT NULL,
 CONSTRAINT [PK_GroupPosts] PRIMARY KEY CLUSTERED 
(
	[gp_id] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[PerformMaintenance]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PerformMaintenance]
AS
BEGIN

delete from favourites where u_username in 
 (select u_username from users where u_deleted = 1)

delete from favourites where f_username in 
 (select u_username from users where u_deleted = 1)

delete from friends where u_username in 
 (select u_username from users where u_deleted = 1)

delete from friends where f_username in 
 (select u_username from users where u_deleted = 1)

delete from ratings where r_fromusername in 
 (select u_username from users where u_deleted = 1)

delete from ratings where r_tousername in 
 (select u_username from users where u_deleted = 1)

delete from votes where v_fromusername in 
 (select u_username from users where u_deleted = 1)
 or v_tousername in (select u_username from users where u_deleted = 1)

delete from comments where c_from_username in
 (select u_username from users where u_deleted = 1)

delete from comments where c_to_username in
 (select u_username from users where u_deleted = 1)

delete from events where e_fromusername in 
 (select u_username from users where u_deleted = 1)

delete from MessagesHistoryCache where mhc_fromusername in 
 (select u_username from users where u_deleted = 1)
 or mhc_tousername in (select u_username from users where u_deleted = 1)

begin transaction
insert into archive_Photos
  select p_id,u_username,p_image,p_name,p_description,p_approved,p_primary,p_explicit,p_private from Photos 
	where u_username in (select u_username from users where u_deleted = 1)

DELETE FROM PhotoRatings WHERE p_id in (select p_id from photos where u_username in (select u_username from users where u_deleted = 1))

DELETE FROM PhotoComments WHERE p_id in (select p_id from photos where u_username in (select u_username from users where u_deleted = 1))

DELETE FROM CommunityPhotoApproval WHERE p_id in (select p_id from photos where u_username in (select u_username from users where u_deleted = 1))

delete from Photos 
	where u_username in (select u_username from users where u_deleted = 1)
commit transaction

begin transaction
insert into archive_Messages
	select * from messages where m_from_folder = 2147483647 and m_to_folder = 2147483647
delete from messages where m_from_folder = 2147483647 and m_to_folder = 2147483647
commit transaction

begin transaction
insert into archive_messages
	select * from messages 
	 where m_from_username in (select u_username from users where u_deleted = 1)
	   and m_to_username in (select u_username from users where u_deleted = 1)
delete from messages 
 where m_from_username in (select u_username from users where u_deleted = 1)
   and m_to_username in (select u_username from users where u_deleted = 1)
commit transaction

begin transaction
insert into archive_profileanswers
	select * from profileanswers 
	 where u_username in (select u_username from users where u_deleted = 1)
delete from profileanswers 
 where u_username in (select u_username from users where u_deleted = 1)
commit transaction

-- deletes reports related to photos that are already deleted
DELETE FROM [AbuseReports] WHERE ar_reviewed = 0 AND [ar_type] = 1 AND NOT EXISTS (SELECT * FROM Photos WHERE p_id = [ar_targetid])
-- deletes reports related to users that are already deleted
DELETE FROM [AbuseReports] WHERE [ar_reviewed] = 0 AND NOT EXISTS (SELECT * FROM users WHERE [ar_reporteduser] = [u_username] AND u_deleted = 0)

-- delete group memberships for the deleted users
DELETE FROM GroupMembers WHERE u_username IN (select u_username from users where u_deleted = 1)

---//                                                        //---
---// Preserve only the last 100 profile views for each user //---
---//                                                        //---

-- declare all variables!
DECLARE  @Username VARCHAR(20)
 
-- declare the cursor
DECLARE ViewedUsers CURSOR FOR
SELECT    u_username
FROM      Users
WHERE (SELECT COUNT(*) FROM ProfileViews WHERE pv_viewed = u_username) > 100
 
OPEN ViewedUsers

FETCH ViewedUsers INTO @Username
 
-- start the main processing loop.
WHILE @@Fetch_Status = 0
   BEGIN

	DELETE FROM ProfileViews WHERE pv_viewed = @Username
			AND pv_viewer NOT IN (SELECT TOP 100 pv_viewer FROM ProfileViews
				WHERE pv_viewed = @Username ORDER BY pv_timestamp DESC)

   FETCH ViewedUsers INTO @Username

   END

CLOSE ViewedUsers
DEALLOCATE ViewedUsers

---//                                                   //---
---// Preserve only the last 200 messages for each user //---
---//                                                   //---

-- declare the cursor
DECLARE SiteUsers CURSOR FOR
SELECT    u_username
FROM      Users
WHERE (SELECT COUNT(m_id) FROM Messages WHERE m_to_username = u_username) > 200
 
OPEN SiteUsers

FETCH SiteUsers INTO @Username
 
-- start the main processing loop.
WHILE @@Fetch_Status = 0
   BEGIN

	INSERT INTO archive_Messages 
		SELECT * FROM Messages WHERE m_to_username = @Username
			AND m_id NOT IN (SELECT TOP 200 m_id FROM Messages 
				WHERE m_to_username = @Username AND m_to_folder = 1 ORDER BY m_id DESC)

	DELETE FROM Messages WHERE m_to_username = @Username
			AND m_id NOT IN (SELECT TOP 200 m_id FROM Messages 
				WHERE m_to_username = @Username AND m_to_folder = 1 ORDER BY m_id DESC)

   FETCH SiteUsers INTO @Username

   END

CLOSE SiteUsers
DEALLOCATE SiteUsers

---// Punish haters
update Users set u_score = u_score - 2000 where u_username in
(
	select pr_fromusername
	from PhotoRatings 
	group by pr_fromusername
	having AVG(cast(pr_rating as decimal)) < 2 and COUNT(pr_rating) > 100
)
delete from PhotoRatings where pr_fromusername in
(
	select pr_fromusername
	from PhotoRatings 
	group by pr_fromusername
	having AVG(cast(pr_rating as decimal)) < 2 and COUNT(pr_rating) > 100
)

END
GO
/****** Object:  StoredProcedure [dbo].[RatePhoto]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RatePhoto]
(
	@FromUser	VARCHAR(20),
	@PhotoID	INT,
	@Rating	INT
)
AS
IF EXISTS (SELECT pr_rating FROM PhotoRatings WHERE pr_fromusername = @FromUser AND p_id = @PhotoID)
	UPDATE
		PhotoRatings
	SET
		pr_rating = @Rating
	WHERE
		pr_fromusername = @FromUser
		AND p_id = @PhotoID
ELSE
	INSERT INTO PhotoRatings
		(pr_fromusername, p_id, pr_rating)
	VALUES
		(@FromUser, @PhotoID, @Rating)
GO
/****** Object:  StoredProcedure [dbo].[SaveCommunityPhotoApproval]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveCommunityPhotoApproval]
	(
		@ID INT,
		@PhotoID INT,
		@Username VARCHAR(20),
		@Approved BIT,
		@Date DATETIME
	)
AS

IF (@ID IS NULL AND NOT EXISTS (SELECT 1 FROM CommunityPhotoApproval WHERE p_id = @PhotoID AND u_username = @Username))

		BEGIN
		
			INSERT INTO CommunityPhotoApproval
			(p_id, u_username, cpa_approved, cpa_timestamp)
			VALUES
			(@PhotoID, @Username, @Approved, @Date)
			
			SELECT @@Identity
		END
	
ELSE

	BEGIN
	
		UPDATE CommunityPhotoApproval
		
		SET p_id = @PhotoID,
			u_username = @Username,
			cpa_approved = @Approved,
			cpa_timestamp = @Date
			
		WHERE cpa_id = @ID
			
	END
GO
/****** Object:  StoredProcedure [dbo].[SaveBlogPostComment]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveBlogPostComment]
(
	@Id			int = -1,
	@BlogPostId int,
	@Username	varchar(20),
	@CommentText	ntext,
	@Approved		bit = 1
)
AS

-- create new comment
IF (@Id = -1)
BEGIN
	INSERT INTO BlogPostComments
	(bp_id, u_username, bpc_text, bpc_approved)
	VALUES
	(@BlogPostId, @Username, @CommentText, @Approved)

	SELECT @@Identity
END
ELSE
	UPDATE BlogPostComments
	SET bpc_text = @CommentText, bpc_approved = @Approved
	WHERE bpc_id = @Id
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupPollsAnswer]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupPollsAnswer] 
	(
		@GroupTopicID		INT,
		@Username			VARCHAR(20),
		@GroupPollsChoiceID	INT
	)	
AS

IF NOT EXISTS (SELECT gt_id FROM GroupPollsAnswers WHERE gt_id = @GroupTopicID AND u_username = @Username)
	
	INSERT INTO GroupPollsAnswers (gt_id, u_username, gpc_id)
				VALUES (@GroupTopicID, @Username, @GroupPollsChoiceID)
ELSE

UPDATE 	GroupPollsAnswers
SET gpc_id = @GroupPollsChoiceID
WHERE gt_id = @GroupTopicID AND u_username = @Username
GO
/****** Object:  StoredProcedure [dbo].[SavePhotoComment]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SavePhotoComment]
	(
		@ID	INT = NULL,
		@PhotoID	INT,
		@Username	VARCHAR(20),
		@Comment	NTEXT,
		@Date		DATETIME = GETDATE
	)
AS

-- create new comment
IF (@ID IS NULL)
BEGIN
	SET @ID = (SELECT pc_id FROM PhotoComments WHERE p_id = @PhotoID AND u_username = @Username)
	IF (@ID IS NULL)
	BEGIN
		INSERT INTO PhotoComments
		(p_id, u_username, pc_comment, pc_date)
		VALUES
		(@PhotoID, @Username, @Comment, @Date)
	
		SELECT @@Identity
	END
	ELSE GOTO update_comment
END
ELSE
update_comment:		-- update comment
	UPDATE PhotoComments
	SET p_id = @PhotoID, u_username = @Username, pc_comment = @Comment, pc_date = @Date
	WHERE pc_id = @ID
GO
/****** Object:  StoredProcedure [dbo].[TopPhotosSearch]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[TopPhotosSearch] 
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
               FROM PhotoRatings 
               WHERE PhotoRatings.p_id = Photos.p_id  
	) as AvgRating
     FROM 
          Users, Photos 
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
               FROM PhotoRatings  
               WHERE PhotoRatings.p_id = Photos.p_id) >= @MinVotes 
     ORDER BY 
        AvgRating DESC 
 
     SET ROWCOUNT 0
GO
/****** Object:  StoredProcedure [dbo].[SetPhotoModerationApprovalScore]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SetPhotoModerationApprovalScore]
	(
		@PhotoID INT,
		@Approved BIT,
		@Scores INT,
		@Penalty INT
	)
AS

BEGIN TRANSACTION

UPDATE Users
SET u_moderationscore = u_moderationscore + @Scores
WHERE u_username IN (SELECT cpa.u_username FROM CommunityPhotoApproval AS cpa WHERE cpa.cpa_approved = @Approved AND cpa.p_id = @PhotoID)

UPDATE Users
SET u_moderationscore = u_moderationscore - @Penalty
WHERE u_username IN (SELECT cpa.u_username FROM CommunityPhotoApproval AS cpa WHERE cpa.cpa_approved != @Approved AND cpa.p_id = @PhotoID)

COMMIT TRANSACTION
GO
/****** Object:  StoredProcedure [dbo].[SearchGroupTopics]    Script Date: 05/14/2010 18:01:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchGroupTopics] 
	(
		@GroupID		INT,
		@Username		VARCHAR(20),
		@Name			NVARCHAR(200),
		@Locked			BIT,
		@Sticky			BIT = NULL,
		@StickyUntil	DATETIME,
		@Keyword		NVARCHAR(200),
		@SearchInPosts	BIT = 0
	)
AS



SELECT gt_id AS ID

FROM GroupTopics

WHERE
	(@GroupID IS NULL OR g_id = @GroupID)
	AND (@Username IS NULL OR u_username = @Username)
	AND (@Name IS NULL OR gt_name = @Name)
	AND (@Locked IS NULL OR gt_locked = @Locked)
	AND (@Sticky IS NULL OR 
			(@Sticky = 1 AND gt_stickyuntil IS NOT NULL) OR (@Sticky = 0 AND gt_stickyuntil IS NULL))
	AND (@StickyUntil IS NULL OR gt_stickyuntil <= @StickyUntil)
	AND (@Keyword IS NULL OR 
			(gt_name like '%' + @Keyword + '%' OR
			(@SearchInPosts = 1 AND EXISTS (SELECT 1 FROM GroupPosts WHERE GroupPosts.gt_id = GroupTopics.gt_id AND gp_post like '%' + @Keyword + '%'))))

ORDER BY 
	CASE WHEN gt_stickyuntil > getdate() THEN gt_stickyuntil
	END DESC,
	gt_dateupdated DESC
GO
/****** Object:  StoredProcedure [dbo].[SearchGroupPosts]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SearchGroupPosts] 
	(
		@GroupTopicID	INT,
		@Username		VARCHAR(20),
		@DatePosted		DATETIME,
		@DateEdited		DATETIME,
		@Keyword		NVARCHAR(200),
		@SortColumn		INT
	)
AS

SELECT
		gp_id AS ID
		
FROM GroupPosts

WHERE
		(@GroupTopicID IS NULL OR gt_id = @GroupTopicID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@DatePosted IS NULL OR gp_dateposted = @DatePosted)
		AND (@DateEdited IS NULL OR gp_dateedited = @DateEdited)
		AND (@Keyword IS NULL OR gp_post like '%' + @Keyword + '%')

ORDER BY
		CASE @SortColumn
			WHEN 1 THEN gp_dateposted
			WHEN 2 THEN gp_dateedited
		END ASC
GO
/****** Object:  StoredProcedure [dbo].[SaveGroupPost]    Script Date: 05/14/2010 18:01:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SaveGroupPost] 
	(
		@ID			INT,
		@GroupTopicID	INT,
		@Username	VARCHAR(20),
		@DatePosted DATETIME,
		@DateEdited DATETIME,
		@EditNotes	NVARCHAR(200),
		@Post		NTEXT
	)
AS

IF @ID IS NULL

	BEGIN
	
		IF EXISTS (SELECT gt_id FROM GroupTopics WHERE gt_id = @GroupTopicID)
			INSERT INTO GroupPosts
				(gt_id, u_username, gp_dateposted, gp_dateedited, gp_editnotes, gp_post)
				VALUES 
				(@GroupTopicID, @Username, @DatePosted, @DateEdited, @EditNotes, @Post)
			
			SELECT @@Identity
	END

ELSE
	
	BEGIN
	
	UPDATE GroupPosts
	
			SET
				gt_id = @GroupTopicID,
				u_username = @Username,
				gp_dateposted = @DatePosted,
				gp_dateedited = @DateEdited,
				gp_editnotes = @EditNotes,
				gp_post = @Post
				
	WHERE gp_id = @ID
	
	END
GO
/****** Object:  StoredProcedure [dbo].[PurgeUser]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PurgeUser]
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
/****** Object:  StoredProcedure [dbo].[IsDuplicateGroupPost]    Script Date: 05/14/2010 18:01:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsDuplicateGroupPost] 
	(
		@TopicID	INT,
		@Username	VARCHAR(20),
		@Post		NTEXT
	)
AS

IF 
	EXISTS(
			SELECT	gp_id
			FROM	GroupPosts
			WHERE	gt_id = @TopicID AND u_username = @Username AND gp_post LIKE @Post)
	
	SELECT 1

ELSE SELECT 0
GO
/****** Object:  Trigger [groupposts_del_trg]    Script Date: 05/14/2010 18:01:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[groupposts_del_trg] 

ON [dbo].[GroupPosts] 

FOR DELETE

AS

--DELETE FROM GroupTopicSubscriptions

--WHERE	gt_id in (select gt_id from deleted)
--		AND (select count (*) from GroupPosts where GroupPosts.gt_id = GroupTopics.gt_id) = 0

DELETE FROM GroupPollsAnswers
WHERE	gt_id in (select gt_id from deleted)
		AND (select count (*) from GroupPosts where GroupPosts.gt_id = GroupPollsAnswers.gt_id) = 0

DELETE FROM GroupPollsChoices
WHERE	gt_id in (select gt_id from deleted)
		AND (select count (*) from GroupPosts where GroupPosts.gt_id = GroupPollsChoices.gt_id) = 0

DELETE FROM GroupTopics
WHERE	gt_id in (select gt_id from deleted)
		AND (select count (*) from GroupPosts where GroupPosts.gt_id = GroupTopics.gt_id) = 0
GO
/****** Object:  StoredProcedure [dbo].[FetchGroupPosts]    Script Date: 05/14/2010 18:01:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchGroupPosts] 
	(
		@ID				INT,
		@GroupTopicID	INT,
		@Username		VARCHAR(20),
		@DatePosted		DATETIME,
		@DateEdited		DATETIME,
		@SortColumn		INT
	)
AS

SELECT
		gp_id AS ID,
		gt_id AS GroupTopicID,
		u_username AS Username,
		gp_dateposted AS DatePosted,
		gp_dateedited AS DateEdited,
		gp_editnotes AS EditNotes,
		gp_post AS Post
		
FROM GroupPosts

WHERE
		(@ID IS NULL OR gp_id = @ID)
		AND (@GroupTopicID IS NULL OR gt_id = @GroupTopicID)
		AND (@Username IS NULL OR u_username = @Username)
		AND (@DatePosted IS NULL OR gp_dateposted = @DatePosted)
		AND (@DateEdited IS NULL OR gp_dateedited = @DateEdited)

ORDER BY
		CASE @SortColumn
			WHEN 1 THEN gp_dateposted
			WHEN 2 THEN gp_dateedited
		END ASC
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupTopic]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupTopic] 
	(
		@ID INT
	)
AS

BEGIN TRAN

DELETE FROM GroupPollsAnswers

WHERE gt_id = @ID

DELETE FROM GroupPollsChoices

WHERE gt_id = @ID

DELETE FROM GroupPosts

WHERE gt_id = @ID

DELETE FROM GroupTopicSubscriptions

WHERE gt_id = @ID



DELETE FROM GroupTopics

WHERE gt_id = @ID

COMMIT
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroupPost]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroupPost] 
	(
		@ID				INT = NULL,
		@GroupId		INT = NULL,
		@GroupTopicID	INT = NULL,
		@Username		VARCHAR(20)
	)
AS

-- declare all variables! 
DECLARE @TopicId	INT
DECLARE @Count		INT		

-- declare the cursor
DECLARE Topics CURSOR FOR
SELECT DISTINCT gt_id
FROM GroupPosts
WHERE (@ID IS NULL OR gp_id = @ID)
		AND (@GroupId IS NULL OR gt_id IN (SELECT gt_id FROM GroupTopics WHERE g_id = @GroupId))
 		AND (@GroupTopicID IS NULL OR gt_id = @GroupTopicID)
		AND (@Username IS NULL OR u_username = @Username)

OPEN Topics

FETCH Topics INTO @TopicId

-- start the main processing loop.
WHILE @@Fetch_Status = 0
BEGIN

SELECT @Count = COUNT(gp_id) FROM GroupPosts WHERE gt_id = @TopicId
		AND (@ID IS NULL OR gp_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)

DELETE FROM GroupPosts WHERE gt_id = @TopicId
		AND (@ID IS NULL OR gp_id = @ID)
		AND (@Username IS NULL OR u_username = @Username)

IF (@Count > 0)
	UPDATE GroupTopics SET gt_posts = gt_posts - @Count
	WHERE gt_id = @TopicId

FETCH Topics INTO @TopicId

END

CLOSE Topics
DEALLOCATE Topics
GO
/****** Object:  StoredProcedure [dbo].[DeleteInactiveGroups]    Script Date: 05/14/2010 18:01:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteInactiveGroups]
	(
		@Date	DATETIME
	)
AS											
												
DECLARE @GroupId INT

-- declare the cursor
DECLARE GroupsForDeletion CURSOR FOR
SELECT g_id 
FROM Groups
WHERE 
	NOT EXISTS (SELECT 1 FROM GroupPhotos WHERE GroupPhotos.g_id = Groups.g_id AND gp_date > @Date)
	AND NOT EXISTS (SELECT 1 FROM GroupTopics, GroupPosts WHERE GroupTopics.g_id = Groups.g_id
					AND GroupTopics.gt_id = GroupPosts.gt_id AND gp_dateposted > @Date)
	AND Groups.g_datecreated < @Date
OPEN GroupsForDeletion

FETCH GroupsForDeletion INTO @GroupId
 
-- start the main processing loop.
WHILE @@Fetch_Status = 0
   BEGIN

		DELETE FROM GroupsCategories

		WHERE g_id = @GroupId

		DELETE FROM GroupMembers

		WHERE g_id = @GroupId

		DELETE FROM GroupPosts

		WHERE gt_id IN (SELECT gt_id FROM GroupTopics WHERE g_id = @GroupId)

		DELETE FROM GroupTopicSubscriptions
			WHERE g_id = @GroupId
			
		DELETE FROM GroupTopics

		WHERE g_id = @GroupId

		DELETE FROM GroupBans

		WHERE g_id = @GroupId

		DELETE FROM GroupPhotos

		WHERE g_id = @GroupId
		
		DELETE FROM GroupEventsComments 
			WHERE ge_id in (SELECT ge_id FROM GroupEvents WHERE g_id = @GroupId)		
		
		DELETE FROM GroupEventsAttenders

		WHERE ge_id IN (SELECT ge_id FROM GroupEvents WHERE g_id = @GroupId)

		DELETE FROM GroupEvents

		WHERE g_id = @GroupId
		
		DELETE FROM Events

		WHERE e_fromgroup = @GroupId

		DELETE FROM Groups

		WHERE g_id = @GroupId
	
   FETCH GroupsForDeletion INTO @GroupId

   END

CLOSE GroupsForDeletion
DEALLOCATE GroupsForDeletion
GO
/****** Object:  StoredProcedure [dbo].[DeleteGroup]    Script Date: 05/14/2010 18:01:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteGroup] 
	(
		@ID	INT
	)
AS

BEGIN TRAN

DELETE FROM GroupsCategories

WHERE g_id = @ID

DELETE FROM GroupMembers

WHERE g_id = @ID

DELETE FROM GroupPosts

WHERE gt_id IN (SELECT gt_id FROM GroupTopics WHERE g_id = @ID)

DELETE FROM GroupTopics

WHERE g_id = @ID

DELETE FROM GroupBans

WHERE g_id = @ID

DELETE FROM GroupPhotos

WHERE g_id = @ID

DELETE FROM GroupEventsAttenders

WHERE ge_id IN (SELECT ge_id FROM GroupEvents WHERE g_id = @ID)

DELETE FROM GroupEvents

WHERE g_id = @ID

DELETE FROM Events

WHERE e_fromgroup = @ID

DELETE FROM Groups

WHERE g_id = @ID

COMMIT
GO
/****** Object:  Default [DF_AbuseReports_ar_reviewed]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AbuseReports] ADD  CONSTRAINT [DF_AbuseReports_ar_reviewed]  DEFAULT ((0)) FOR [ar_reviewed]
GO
/****** Object:  Default [DF_AbuseReports_ar_datereported]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AbuseReports] ADD  CONSTRAINT [DF_AbuseReports_ar_datereported]  DEFAULT (getdate()) FOR [ar_datereported]
GO
/****** Object:  Default [DF_AdComments_ac_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AdComments] ADD  CONSTRAINT [DF_AdComments_ac_date]  DEFAULT (getdate()) FOR [ac_date]
GO
/****** Object:  Default [DF_Admins_a_lastlogin]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Admins] ADD  CONSTRAINT [DF_Admins_a_lastlogin]  DEFAULT (getdate()) FOR [a_lastlogin]
GO
/****** Object:  Default [DF_Admins_a_privileges]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Admins] ADD  CONSTRAINT [DF_Admins_a_privileges]  DEFAULT ('') FOR [a_privileges]
GO
/****** Object:  Default [DF_Admins_a_lastsessionid]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Admins] ADD  CONSTRAINT [DF_Admins_a_lastsessionid]  DEFAULT ('') FOR [a_lastsessionid]
GO
/****** Object:  Default [DF_Ads_a_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ads] ADD  CONSTRAINT [DF_Ads_a_date]  DEFAULT (getdate()) FOR [a_date]
GO
/****** Object:  Default [DF_Ads_a_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ads] ADD  CONSTRAINT [DF_Ads_a_approved]  DEFAULT ((0)) FOR [a_approved]
GO
/****** Object:  Default [DF_AffiliateCommissions_ac_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AffiliateCommissions] ADD  CONSTRAINT [DF_AffiliateCommissions_ac_timestamp]  DEFAULT (getdate()) FOR [ac_timestamp]
GO
/****** Object:  Default [DF_Affiliates_a_deleted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Affiliates] ADD  CONSTRAINT [DF_Affiliates_a_deleted]  DEFAULT ((0)) FOR [a_deleted]
GO
/****** Object:  Default [DF_Affiliates_a_active]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Affiliates] ADD  CONSTRAINT [DF_Affiliates_a_active]  DEFAULT ((1)) FOR [a_active]
GO
/****** Object:  Default [DF_Affiliates_a_percentage]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Affiliates] ADD  CONSTRAINT [DF_Affiliates_a_percentage]  DEFAULT ((35)) FOR [a_percentage]
GO
/****** Object:  Default [DF_Affiliates_a_fixed_amount]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Affiliates] ADD  CONSTRAINT [DF_Affiliates_a_fixed_amount]  DEFAULT ((0)) FOR [a_fixed_amount]
GO
/****** Object:  Default [DF_Affiliates_a_recurrent]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Affiliates] ADD  CONSTRAINT [DF_Affiliates_a_recurrent]  DEFAULT ((0)) FOR [a_recurrent]
GO
/****** Object:  Default [DF_Affiliates_a_request_payment]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Affiliates] ADD  CONSTRAINT [DF_Affiliates_a_request_payment]  DEFAULT ((0)) FOR [a_request_payment]
GO
/****** Object:  Default [DF_AudioAccess_aa_dateaccessgranted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AudioAccess] ADD  CONSTRAINT [DF_AudioAccess_aa_dateaccessgranted]  DEFAULT (getdate()) FOR [aa_dateaccessgranted]
GO
/****** Object:  Default [DF_BillingPlans_p_deleted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BillingPlans] ADD  CONSTRAINT [DF_BillingPlans_p_deleted]  DEFAULT ((0)) FOR [p_deleted]
GO
/****** Object:  Default [DF_BlockedUsers_bu_blockdate]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlockedUsers] ADD  CONSTRAINT [DF_BlockedUsers_bu_blockdate]  DEFAULT (getdate()) FOR [bu_blockdate]
GO
/****** Object:  Default [DF_BlogPostComments_bpc_dateposted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPostComments] ADD  CONSTRAINT [DF_BlogPostComments_bpc_dateposted]  DEFAULT (getdate()) FOR [bpc_dateposted]
GO
/****** Object:  Default [DF_BlogPostComments_bpc_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPostComments] ADD  CONSTRAINT [DF_BlogPostComments_bpc_approved]  DEFAULT ((1)) FOR [bpc_approved]
GO
/****** Object:  Default [DF_BlogPosts_bp_dateposted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPosts] ADD  CONSTRAINT [DF_BlogPosts_bp_dateposted]  DEFAULT (getdate()) FOR [bp_dateposted]
GO
/****** Object:  Default [DF_BlogPosts_bp_reads]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPosts] ADD  CONSTRAINT [DF_BlogPosts_bp_reads]  DEFAULT ((0)) FOR [bp_reads]
GO
/****** Object:  Default [DF_BlogPosts_bp_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPosts] ADD  CONSTRAINT [DF_BlogPosts_bp_approved]  DEFAULT ((1)) FOR [bp_approved]
GO
/****** Object:  Default [DF_Blogs_b_datecreated]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Blogs] ADD  CONSTRAINT [DF_Blogs_b_datecreated]  DEFAULT (getdate()) FOR [b_datecreated]
GO
/****** Object:  Default [DF_Categories_c_userscancreategroups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Categories] ADD  CONSTRAINT [DF_Categories_c_userscancreategroups]  DEFAULT ((0)) FOR [c_userscancreategroups]
GO
/****** Object:  Default [DF_ChatBans_cb_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ChatBans] ADD  CONSTRAINT [DF_ChatBans_cb_date]  DEFAULT (getdate()) FOR [cb_date]
GO
/****** Object:  Default [DF_ChatUsers_cu_begintime]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ChatUsers] ADD  CONSTRAINT [DF_ChatUsers_cu_begintime]  DEFAULT (getdate()) FOR [cu_begintime]
GO
/****** Object:  Default [DF_ChatUsers_cu_activetime]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ChatUsers] ADD  CONSTRAINT [DF_ChatUsers_cu_activetime]  DEFAULT (getdate()) FOR [cu_activetime]
GO
/****** Object:  Default [DF_Comments_c_date_posted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF_Comments_c_date_posted]  DEFAULT (getdate()) FOR [c_date_posted]
GO
/****** Object:  Default [DF_Comments_c_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Comments] ADD  CONSTRAINT [DF_Comments_c_approved]  DEFAULT ((1)) FOR [c_approved]
GO
/****** Object:  Default [DF_CommunityPhotoApproval_cpa_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[CommunityPhotoApproval] ADD  CONSTRAINT [DF_CommunityPhotoApproval_cpa_timestamp]  DEFAULT (getdate()) FOR [cpa_timestamp]
GO
/****** Object:  Default [DF_CommunityProfileApproval_cpa_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[CommunityProfileApproval] ADD  CONSTRAINT [DF_CommunityProfileApproval_cpa_timestamp]  DEFAULT (getdate()) FOR [cpa_timestamp]
GO
/****** Object:  Default [DF_ContentPage_cp_html_code]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ContentPage] ADD  CONSTRAINT [DF_ContentPage_cp_html_code]  DEFAULT ('') FOR [cp_content]
GO
/****** Object:  Default [DF_ContentPage_l_id]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ContentPage] ADD  CONSTRAINT [DF_ContentPage_l_id]  DEFAULT ((1)) FOR [l_id]
GO
/****** Object:  Default [DF_ContentPage_cp_visiblefor]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ContentPage] ADD  CONSTRAINT [DF_ContentPage_cp_visiblefor]  DEFAULT ((3)) FOR [cp_visiblefor]
GO
/****** Object:  Default [DF_ContentPage_cp_metadescription]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ContentPage] ADD  CONSTRAINT [DF_ContentPage_cp_metadescription]  DEFAULT ('') FOR [cp_metadescription]
GO
/****** Object:  Default [DF_ContentPage_cp_metakeyword]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ContentPage] ADD  CONSTRAINT [DF_ContentPage_cp_metakeyword]  DEFAULT ('') FOR [cp_metakeyword]
GO
/****** Object:  Default [DF_Ecards_ec_is_opened]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ecards] ADD  CONSTRAINT [DF_Ecards_ec_is_opened]  DEFAULT ((0)) FOR [ec_is_opened]
GO
/****** Object:  Default [DF_EcardTypes_ect_active]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[EcardTypes] ADD  CONSTRAINT [DF_EcardTypes_ect_active]  DEFAULT ((0)) FOR [ect_active]
GO
/****** Object:  Default [DF_EmailQueue_eq_tries]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[EmailQueue] ADD  CONSTRAINT [DF_EmailQueue_eq_tries]  DEFAULT ((0)) FOR [eq_tries]
GO
/****** Object:  Default [DF_EstablishedCommunications_ec_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[EstablishedCommunications] ADD  CONSTRAINT [DF_EstablishedCommunications_ec_date]  DEFAULT (getdate()) FOR [ec_date]
GO
/****** Object:  Default [DF_Events_e_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Events] ADD  CONSTRAINT [DF_Events_e_date]  DEFAULT (getdate()) FOR [e_date]
GO
/****** Object:  Default [DF_Favourites_f_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Favourites] ADD  CONSTRAINT [DF_Favourites_f_timestamp]  DEFAULT (getdate()) FOR [f_timestamp]
GO
/****** Object:  Default [DF_Friends_f_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Friends] ADD  CONSTRAINT [DF_Friends_f_timestamp]  DEFAULT (getdate()) FOR [f_timestamp]
GO
/****** Object:  Default [DF_Friends_f_accepted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Friends] ADD  CONSTRAINT [DF_Friends_f_accepted]  DEFAULT ((0)) FOR [f_accepted]
GO
/****** Object:  Default [DF_GroupBans_gb_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupBans] ADD  CONSTRAINT [DF_GroupBans_gb_date]  DEFAULT (getdate()) FOR [gb_date]
GO
/****** Object:  Default [DF_GroupEvents_ge_location]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEvents] ADD  CONSTRAINT [DF_GroupEvents_ge_location]  DEFAULT ('') FOR [ge_location]
GO
/****** Object:  Default [DF_GroupEventsComments_gec_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEventsComments] ADD  CONSTRAINT [DF_GroupEventsComments_gec_date]  DEFAULT (getdate()) FOR [gec_date]
GO
/****** Object:  Default [DF_GroupMembers_gm_joindate]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupMembers] ADD  CONSTRAINT [DF_GroupMembers_gm_joindate]  DEFAULT (getdate()) FOR [gm_joindate]
GO
/****** Object:  Default [DF_GroupMembers_gm_active]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupMembers] ADD  CONSTRAINT [DF_GroupMembers_gm_active]  DEFAULT ((1)) FOR [gm_active]
GO
/****** Object:  Default [DF_GroupMembers_gm_joinanswer]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupMembers] ADD  CONSTRAINT [DF_GroupMembers_gm_joinanswer]  DEFAULT ('') FOR [gm_joinanswer]
GO
/****** Object:  Default [DF_GroupMembers_gm_warned]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupMembers] ADD  CONSTRAINT [DF_GroupMembers_gm_warned]  DEFAULT ((0)) FOR [gm_warned]
GO
/****** Object:  Default [DF_GroupPosts_gp_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPosts] ADD  CONSTRAINT [DF_GroupPosts_gp_date]  DEFAULT (getdate()) FOR [gp_dateposted]
GO
/****** Object:  Default [DF_Groups_g_private]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_g_private]  DEFAULT ((0)) FOR [g_accesslevel]
GO
/****** Object:  Default [DF_Groups_g_members]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_g_members]  DEFAULT ((0)) FOR [g_activemembers]
GO
/****** Object:  Default [DF_Groups_g_jointerms]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_g_jointerms]  DEFAULT ('') FOR [g_jointerms]
GO
/****** Object:  Default [DF_Groups_g_joinquestion]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_g_joinquestion]  DEFAULT ('') FOR [g_joinquestion]
GO
/****** Object:  Default [DF_Groups_g_permissions]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_g_permissions]  DEFAULT ((131821055)) FOR [g_permissions]
GO
/****** Object:  Default [DF_Groups_g_autojoin]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Groups] ADD  CONSTRAINT [DF_Groups_g_autojoin]  DEFAULT ((0)) FOR [g_autojoin]
GO
/****** Object:  Default [DF_GroupTopics_gt_datecreated]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics] ADD  CONSTRAINT [DF_GroupTopics_gt_datecreated]  DEFAULT (getdate()) FOR [gt_datecreated]
GO
/****** Object:  Default [DF_GroupTopics_gt_dateupdated]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics] ADD  CONSTRAINT [DF_GroupTopics_gt_dateupdated]  DEFAULT (getdate()) FOR [gt_dateupdated]
GO
/****** Object:  Default [DF_GroupTopics_gt_replies]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics] ADD  CONSTRAINT [DF_GroupTopics_gt_replies]  DEFAULT ((0)) FOR [gt_posts]
GO
/****** Object:  Default [DF_GroupTopics_gt_locked]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics] ADD  CONSTRAINT [DF_GroupTopics_gt_locked]  DEFAULT ((0)) FOR [gt_locked]
GO
/****** Object:  Default [DF_GroupTopics_gt_poll]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics] ADD  CONSTRAINT [DF_GroupTopics_gt_poll]  DEFAULT ((0)) FOR [gt_poll]
GO
/****** Object:  Default [DF_GroupTopics_gt_views]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics] ADD  CONSTRAINT [DF_GroupTopics_gt_views]  DEFAULT ((0)) FOR [gt_views]
GO
/****** Object:  Default [DF_Interests_i_date_posted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Interests] ADD  CONSTRAINT [DF_Interests_i_date_posted]  DEFAULT (getdate()) FOR [i_date_posted]
GO
/****** Object:  Default [DF_Interests_i_deleted_by_fromuser]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Interests] ADD  CONSTRAINT [DF_Interests_i_deleted_by_fromuser]  DEFAULT ((0)) FOR [i_deleted_by_fromuser]
GO
/****** Object:  Default [DF_Interests_i_deleted_by_touser]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Interests] ADD  CONSTRAINT [DF_Interests_i_deleted_by_touser]  DEFAULT ((0)) FOR [i_deleted_by_touser]
GO
/****** Object:  Default [DF_IPLogs_ipl_action]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[IPLogs] ADD  CONSTRAINT [DF_IPLogs_ipl_action]  DEFAULT ((0)) FOR [ipl_action]
GO
/****** Object:  Default [DF_IPLogs_ipl_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[IPLogs] ADD  CONSTRAINT [DF_IPLogs_ipl_timestamp]  DEFAULT (getdate()) FOR [ipl_timestamp]
GO
/****** Object:  Default [DF_Languages_l_active]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_l_active]  DEFAULT ((0)) FOR [l_active]
GO
/****** Object:  Default [DF_Languages_l_predefined]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_l_predefined]  DEFAULT ((0)) FOR [l_predefined]
GO
/****** Object:  Default [DF_Languages_l_order]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Languages] ADD  CONSTRAINT [DF_Languages_l_order]  DEFAULT ((1)) FOR [l_order]
GO
/****** Object:  Default [DF_Messages_m_from_folder]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_from_folder]  DEFAULT ((2)) FOR [m_from_folder]
GO
/****** Object:  Default [DF_Messages_m_to_folder]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_to_folder]  DEFAULT ((1)) FOR [m_to_folder]
GO
/****** Object:  Default [DF_Messages_m_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_timestamp]  DEFAULT (getdate()) FOR [m_timestamp]
GO
/****** Object:  Default [DF_Messages_m_replied_to]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_replied_to]  DEFAULT ((0)) FOR [m_replied_to]
GO
/****** Object:  Default [DF_Messages_m_is_read]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_is_read]  DEFAULT ((0)) FOR [m_is_read]
GO
/****** Object:  Default [DF_Messages_m_pending_approval]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_pending_approval]  DEFAULT ((0)) FOR [m_pending_approval]
GO
/****** Object:  Default [DF_Messages_m_new]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages] ADD  CONSTRAINT [DF_Messages_m_new]  DEFAULT ((1)) FOR [m_new]
GO
/****** Object:  Default [DF_MessagesSandbox_ms_count]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[MessagesSandbox] ADD  CONSTRAINT [DF_MessagesSandbox_ms_count]  DEFAULT ((1)) FOR [ms_count]
GO
/****** Object:  Default [DF_MessagesSandbox_ms_datetime]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[MessagesSandbox] ADD  CONSTRAINT [DF_MessagesSandbox_ms_datetime]  DEFAULT (getdate()) FOR [ms_datetime]
GO
/****** Object:  Default [DF_News_n_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF_News_n_date]  DEFAULT (getdate()) FOR [n_date]
GO
/****** Object:  Default [DF__News__n_title__6C23FBB3]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF__News__n_title__6C23FBB3]  DEFAULT ('') FOR [n_title]
GO
/****** Object:  Default [DF__News__l_id__6D181FEC]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[News] ADD  CONSTRAINT [DF__News__l_id__6D181FEC]  DEFAULT ((1)) FOR [l_id]
GO
/****** Object:  Default [DF_PaymentHistory_ph_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PaymentHistory] ADD  CONSTRAINT [DF_PaymentHistory_ph_timestamp]  DEFAULT (getdate()) FOR [ph_timestamp]
GO
/****** Object:  Default [DF_PhotoAccess_pa_dateaccessgranted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoAccess] ADD  CONSTRAINT [DF_PhotoAccess_pa_dateaccessgranted]  DEFAULT (getdate()) FOR [pa_dateaccessgranted]
GO
/****** Object:  Default [DF_PhotoContests_pc_terms]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContests] ADD  CONSTRAINT [DF_PhotoContests_pc_terms]  DEFAULT ('') FOR [pc_terms]
GO
/****** Object:  Default [DF_PhotoContests_pc_datecreated]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContests] ADD  CONSTRAINT [DF_PhotoContests_pc_datecreated]  DEFAULT (getdate()) FOR [pc_datecreated]
GO
/****** Object:  Default [DF_PhotoNotes_pn_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoNotes] ADD  CONSTRAINT [DF_PhotoNotes_pn_timestamp]  DEFAULT (getdate()) FOR [pn_timestamp]
GO
/****** Object:  Default [DF_PhotoRatings_pr_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoRatings] ADD  CONSTRAINT [DF_PhotoRatings_pr_timestamp]  DEFAULT (getdate()) FOR [pr_timestamp]
GO
/****** Object:  Default [DF_Photos_p_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos] ADD  CONSTRAINT [DF_Photos_p_approved]  DEFAULT ((0)) FOR [p_approved]
GO
/****** Object:  Default [DF_Photos_p_primary]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos] ADD  CONSTRAINT [DF_Photos_p_primary]  DEFAULT ((0)) FOR [p_primary]
GO
/****** Object:  Default [DF_Photos_p_explicit]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos] ADD  CONSTRAINT [DF_Photos_p_explicit]  DEFAULT ((0)) FOR [p_explicit]
GO
/****** Object:  Default [DF_Photos_p_private]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos] ADD  CONSTRAINT [DF_Photos_p_private]  DEFAULT ((0)) FOR [p_private]
GO
/****** Object:  Default [DF_Photos_p_manual_approval]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos] ADD  CONSTRAINT [DF_Photos_p_manual_approval]  DEFAULT ((0)) FOR [p_manual_approval]
GO
/****** Object:  Default [DF_Photos_p_salute]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos] ADD  CONSTRAINT [DF_Photos_p_salute]  DEFAULT ((0)) FOR [p_salute]
GO
/****** Object:  Default [DF_ProfileAnswers_pa_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileAnswers] ADD  CONSTRAINT [DF_ProfileAnswers_pa_approved]  DEFAULT ((1)) FOR [pa_approved]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_name2]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_name2]  DEFAULT ('') FOR [pq_altname]
GO
/****** Object:  Default [DF_ProfileFields_pf_type]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileFields_pf_type]  DEFAULT ((0)) FOR [pq_edit_style]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_show_style]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_show_style]  DEFAULT ((0)) FOR [pq_show_style]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_search_style]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_search_style]  DEFAULT ((0)) FOR [pq_search_style]
GO
/****** Object:  Default [DF_ProfileFields_pf_required]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileFields_pf_required]  DEFAULT ((0)) FOR [pq_required]
GO
/****** Object:  Default [DF_ProfileFields_pf_priority]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileFields_pf_priority]  DEFAULT ((1)) FOR [pq_priority]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_requires_approval]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_requires_approval]  DEFAULT ((0)) FOR [pq_requires_approval]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_visible_male]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_visible_male]  DEFAULT ((1)) FOR [pq_visible_male]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_visible_female]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_visible_female]  DEFAULT ((1)) FOR [pq_visible_female]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_visible_couple]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_visible_couple]  DEFAULT ((1)) FOR [pq_visible_couple]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_paidonly]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_paidonly]  DEFAULT ((0)) FOR [pq_viewpaidonly]
GO
/****** Object:  Default [DF_ProfileQuestions_pq_editpaidonly]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions] ADD  CONSTRAINT [DF_ProfileQuestions_pq_editpaidonly]  DEFAULT ((0)) FOR [pq_editpaidonly]
GO
/****** Object:  Default [DF_ProfileCategories_pc_priority]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileTopics] ADD  CONSTRAINT [DF_ProfileCategories_pc_priority]  DEFAULT ((1)) FOR [pt_priority]
GO
/****** Object:  Default [DF_ProfileTopics_pt_columns]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileTopics] ADD  CONSTRAINT [DF_ProfileTopics_pt_columns]  DEFAULT ((1)) FOR [pt_editcolumns]
GO
/****** Object:  Default [DF_ProfileTopics_pt_viewcolumns]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileTopics] ADD  CONSTRAINT [DF_ProfileTopics_pt_viewcolumns]  DEFAULT ((1)) FOR [pt_viewcolumns]
GO
/****** Object:  Default [DF_ProfileViews_u_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileViews] ADD  CONSTRAINT [DF_ProfileViews_u_timestamp]  DEFAULT (getdate()) FOR [pv_timestamp]
GO
/****** Object:  Default [DF_Ratings_r_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ratings] ADD  CONSTRAINT [DF_Ratings_r_timestamp]  DEFAULT (getdate()) FOR [r_timestamp]
GO
/****** Object:  Default [DF_SavedSearches_ss_emailmatches]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[SavedSearches] ADD  CONSTRAINT [DF_SavedSearches_ss_emailmatches]  DEFAULT ((0)) FOR [ss_emailmatches]
GO
/****** Object:  Default [DF_SavedSearches_ss_emailfrequency]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[SavedSearches] ADD  CONSTRAINT [DF_SavedSearches_ss_emailfrequency]  DEFAULT ((7)) FOR [ss_emailfrequency]
GO
/****** Object:  Default [DF_Orders_o_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions] ADD  CONSTRAINT [DF_Orders_o_timestamp]  DEFAULT (getdate()) FOR [s_order_date]
GO
/****** Object:  Default [DF_Subscriptions_s_renew_date]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions] ADD  CONSTRAINT [DF_Subscriptions_s_renew_date]  DEFAULT (getdate()) FOR [s_renew_date]
GO
/****** Object:  Default [DF_Orders_o_confirmed]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions] ADD  CONSTRAINT [DF_Orders_o_confirmed]  DEFAULT ((0)) FOR [s_confirmed]
GO
/****** Object:  Default [DF_Subscriptions_s_cancelled]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions] ADD  CONSTRAINT [DF_Subscriptions_s_cancelled]  DEFAULT ((0)) FOR [s_cancelled]
GO
/****** Object:  Default [DF_Subscriptions_s_cancellation_requested]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions] ADD  CONSTRAINT [DF_Subscriptions_s_cancellation_requested]  DEFAULT ((0)) FOR [s_cancellation_requested]
GO
/****** Object:  Default [DF_Subscriptions_s_paymentprocessor]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions] ADD  CONSTRAINT [DF_Subscriptions_s_paymentprocessor]  DEFAULT ('') FOR [s_paymentprocessor]
GO
/****** Object:  Default [DF_Translations_l_id]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Translations] ADD  CONSTRAINT [DF_Translations_l_id]  DEFAULT ((1)) FOR [l_id]
GO
/****** Object:  Default [DF_Translations_t_adminpanel]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Translations] ADD  CONSTRAINT [DF_Translations_t_adminpanel]  DEFAULT ((0)) FOR [t_adminpanel]
GO
/****** Object:  Default [DF_Users_u_usersince]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_usersince]  DEFAULT (getdate()) FOR [u_usersince]
GO
/****** Object:  Default [DF_Users_u_active]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_active]  DEFAULT ((0)) FOR [u_active]
GO
/****** Object:  Default [DF_Users_u_smsconfirmed]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_smsconfirmed]  DEFAULT ((0)) FOR [u_smsconfirmed]
GO
/****** Object:  Default [DF_Users_u_prevlogin]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_prevlogin]  DEFAULT (getdate()) FOR [u_prevlogin]
GO
/****** Object:  Default [DF_Users_u_lastlogin]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_lastlogin]  DEFAULT (getdate()) FOR [u_lastlogin]
GO
/****** Object:  Default [DF_Users_u_logincount]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_logincount]  DEFAULT ((0)) FOR [u_logincount]
GO
/****** Object:  Default [DF_Users_u_lastonline]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_lastonline]  DEFAULT (getdate()) FOR [u_lastonline]
GO
/****** Object:  Default [DF_Users_u_profileviews]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_profileviews]  DEFAULT ((0)) FOR [u_profileviews]
GO
/****** Object:  Default [DF_Users_u_visible]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_visible]  DEFAULT ((1)) FOR [u_profilevisible]
GO
/****** Object:  Default [DF_Users_u_receiveemails]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_receiveemails]  DEFAULT ((1)) FOR [u_receiveemails]
GO
/****** Object:  Default [DF_Users_u_country]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_country]  DEFAULT ('') FOR [u_country]
GO
/****** Object:  Default [DF_Users_u_state]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_state]  DEFAULT ('') FOR [u_state]
GO
/****** Object:  Default [DF_Users_u_zip_code]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_zip_code]  DEFAULT ('') FOR [u_zip_code]
GO
/****** Object:  Default [DF_Users_u_city]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_city]  DEFAULT ('') FOR [u_city]
GO
/****** Object:  Default [DF_Users_u_deleted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_deleted]  DEFAULT ((0)) FOR [u_deleted]
GO
/****** Object:  Default [DF_Users_u_paid_member]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_paid_member]  DEFAULT ((0)) FOR [u_paid_member]
GO
/****** Object:  Default [DF_Users_u_signup_ip]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_signup_ip]  DEFAULT ('0.0.0.0') FOR [u_signup_ip]
GO
/****** Object:  Default [DF_Users_u_lastsessionid]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_lastsessionid]  DEFAULT ('') FOR [u_lastsessionid]
GO
/****** Object:  Default [DF_Users_u_message_verifications_left]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_message_verifications_left]  DEFAULT ((0)) FOR [u_message_verifications_left]
GO
/****** Object:  Default [DF_Users_u_verifiedbyadmin]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_verifiedbyadmin]  DEFAULT ((0)) FOR [u_verifiedbyadmin]
GO
/****** Object:  Default [DF_Users_u_language_id]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_language_id]  DEFAULT ((1)) FOR [l_id]
GO
/****** Object:  Default [DF_Users_u_options]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_options]  DEFAULT ((0)) FOR [u_options]
GO
/****** Object:  Default [DF_Users_u_score]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_score]  DEFAULT ((0)) FOR [u_score]
GO
/****** Object:  Default [DF_Users_u_credits]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_credits]  DEFAULT ((0)) FOR [u_credits]
GO
/****** Object:  Default [DF_Users_u_moderationscore]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_moderationscore]  DEFAULT ((0)) FOR [u_moderationscore]
GO
/****** Object:  Default [DF_Users_u_spamsuspect]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_spamsuspect]  DEFAULT ((0)) FOR [u_spamsuspected]
GO
/****** Object:  Default [DF_Users_u_face_control_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_face_control_approved]  DEFAULT ((1)) FOR [u_face_control_approved]
GO
/****** Object:  Default [DF_Users_u_featuredmember]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_u_featuredmember]  DEFAULT ((0)) FOR [u_featuredmember]
GO
/****** Object:  Default [DF_UserScoresLog_usl_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[UserScoresLog] ADD  CONSTRAINT [DF_UserScoresLog_usl_timestamp]  DEFAULT (getdate()) FOR [usl_timestamp]
GO
/****** Object:  Default [DF_VerifiedUsers_vu_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VerifiedUsers] ADD  CONSTRAINT [DF_VerifiedUsers_vu_timestamp]  DEFAULT (getdate()) FOR [vu_timestamp]
GO
/****** Object:  Default [DF_VideoAccess_va_dateaccessgranted]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoAccess] ADD  CONSTRAINT [DF_VideoAccess_va_dateaccessgranted]  DEFAULT (getdate()) FOR [va_dateaccessgranted]
GO
/****** Object:  Default [DF_VideoProfiles_vp_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoProfiles] ADD  CONSTRAINT [DF_VideoProfiles_vp_approved]  DEFAULT ((0)) FOR [vp_approved]
GO
/****** Object:  Default [DF_VideoProfiles_vp_private]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoProfiles] ADD  CONSTRAINT [DF_VideoProfiles_vp_private]  DEFAULT ((0)) FOR [vp_private]
GO
/****** Object:  Default [DF_VideoUploads_vu_processed]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoUploads] ADD  CONSTRAINT [DF_VideoUploads_vu_processed]  DEFAULT ((0)) FOR [vu_processed]
GO
/****** Object:  Default [DF_VideoUploads_vu_approved]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoUploads] ADD  CONSTRAINT [DF_VideoUploads_vu_approved]  DEFAULT ((0)) FOR [vu_approved]
GO
/****** Object:  Default [DF_VideoUploads_vu_private]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoUploads] ADD  CONSTRAINT [DF_VideoUploads_vu_private]  DEFAULT ((0)) FOR [vu_private]
GO
/****** Object:  Default [DF_Votes_v_timestamp]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Votes] ADD  CONSTRAINT [DF_Votes_v_timestamp]  DEFAULT (getdate()) FOR [v_timestamp]
GO
/****** Object:  ForeignKey [FK_AdComments_Ads]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AdComments]  WITH CHECK ADD  CONSTRAINT [FK_AdComments_Ads] FOREIGN KEY([a_id])
REFERENCES [dbo].[Ads] ([a_id])
GO
ALTER TABLE [dbo].[AdComments] CHECK CONSTRAINT [FK_AdComments_Ads]
GO
/****** Object:  ForeignKey [FK_AdComments_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AdComments]  WITH CHECK ADD  CONSTRAINT [FK_AdComments_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[AdComments] CHECK CONSTRAINT [FK_AdComments_Users]
GO
/****** Object:  ForeignKey [FK_AdPhotos_Ads]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AdPhotos]  WITH CHECK ADD  CONSTRAINT [FK_AdPhotos_Ads] FOREIGN KEY([a_id])
REFERENCES [dbo].[Ads] ([a_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AdPhotos] CHECK CONSTRAINT [FK_AdPhotos_Ads]
GO
/****** Object:  ForeignKey [FK_Ads_AdsCategories]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ads]  WITH CHECK ADD  CONSTRAINT [FK_Ads_AdsCategories] FOREIGN KEY([ac_id])
REFERENCES [dbo].[AdsCategories] ([ac_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Ads] CHECK CONSTRAINT [FK_Ads_AdsCategories]
GO
/****** Object:  ForeignKey [FK_Ads_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ads]  WITH CHECK ADD  CONSTRAINT [FK_Ads_Users] FOREIGN KEY([a_postedby])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Ads] CHECK CONSTRAINT [FK_Ads_Users]
GO
/****** Object:  ForeignKey [FK_AdsCategories_AdsCategories]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AdsCategories]  WITH CHECK ADD  CONSTRAINT [FK_AdsCategories_AdsCategories] FOREIGN KEY([ac_parentid])
REFERENCES [dbo].[AdsCategories] ([ac_id])
GO
ALTER TABLE [dbo].[AdsCategories] CHECK CONSTRAINT [FK_AdsCategories_AdsCategories]
GO
/****** Object:  ForeignKey [FK_AffiliateCommissions_Affiliates]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AffiliateCommissions]  WITH CHECK ADD  CONSTRAINT [FK_AffiliateCommissions_Affiliates] FOREIGN KEY([a_id])
REFERENCES [dbo].[Affiliates] ([a_id])
GO
ALTER TABLE [dbo].[AffiliateCommissions] CHECK CONSTRAINT [FK_AffiliateCommissions_Affiliates]
GO
/****** Object:  ForeignKey [FK_AffiliateCommissions_PaymentHistory]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AffiliateCommissions]  WITH CHECK ADD  CONSTRAINT [FK_AffiliateCommissions_PaymentHistory] FOREIGN KEY([ph_id])
REFERENCES [dbo].[PaymentHistory] ([ph_id])
GO
ALTER TABLE [dbo].[AffiliateCommissions] CHECK CONSTRAINT [FK_AffiliateCommissions_PaymentHistory]
GO
/****** Object:  ForeignKey [FK_AffiliateCommissions_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AffiliateCommissions]  WITH NOCHECK ADD  CONSTRAINT [FK_AffiliateCommissions_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[AffiliateCommissions] NOCHECK CONSTRAINT [FK_AffiliateCommissions_Users]
GO
/****** Object:  ForeignKey [FK_AffiliatesHistory_Affiliates]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AffiliatesHistory]  WITH CHECK ADD  CONSTRAINT [FK_AffiliatesHistory_Affiliates] FOREIGN KEY([a_id])
REFERENCES [dbo].[Affiliates] ([a_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AffiliatesHistory] CHECK CONSTRAINT [FK_AffiliatesHistory_Affiliates]
GO
/****** Object:  ForeignKey [FK_AudioAccess_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AudioAccess]  WITH CHECK ADD  CONSTRAINT [FK_AudioAccess_Users] FOREIGN KEY([aa_audioowner])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[AudioAccess] CHECK CONSTRAINT [FK_AudioAccess_Users]
GO
/****** Object:  ForeignKey [FK_AudioAccess_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AudioAccess]  WITH CHECK ADD  CONSTRAINT [FK_AudioAccess_Users1] FOREIGN KEY([aa_audioviewer])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[AudioAccess] CHECK CONSTRAINT [FK_AudioAccess_Users1]
GO
/****** Object:  ForeignKey [FK_AudioUploads_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[AudioUploads]  WITH CHECK ADD  CONSTRAINT [FK_AudioUploads_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[AudioUploads] CHECK CONSTRAINT [FK_AudioUploads_Users]
GO
/****** Object:  ForeignKey [FK_BlockedUsers_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlockedUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlockedUsers_Users] FOREIGN KEY([bu_userblocker])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[BlockedUsers] CHECK CONSTRAINT [FK_BlockedUsers_Users]
GO
/****** Object:  ForeignKey [FK_BlockedUsers_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlockedUsers]  WITH CHECK ADD  CONSTRAINT [FK_BlockedUsers_Users1] FOREIGN KEY([bu_blockeduser])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[BlockedUsers] CHECK CONSTRAINT [FK_BlockedUsers_Users1]
GO
/****** Object:  ForeignKey [FK_BlogPostComments_BlogPostComments]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPostComments]  WITH NOCHECK ADD  CONSTRAINT [FK_BlogPostComments_BlogPostComments] FOREIGN KEY([bp_id])
REFERENCES [dbo].[BlogPosts] ([bp_id])
GO
ALTER TABLE [dbo].[BlogPostComments] CHECK CONSTRAINT [FK_BlogPostComments_BlogPostComments]
GO
/****** Object:  ForeignKey [FK_BlogPostComments_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPostComments]  WITH NOCHECK ADD  CONSTRAINT [FK_BlogPostComments_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[BlogPostComments] CHECK CONSTRAINT [FK_BlogPostComments_Users]
GO
/****** Object:  ForeignKey [FK_BlogPosts_Blogs]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[BlogPosts]  WITH NOCHECK ADD  CONSTRAINT [FK_BlogPosts_Blogs] FOREIGN KEY([b_id])
REFERENCES [dbo].[Blogs] ([b_id])
GO
ALTER TABLE [dbo].[BlogPosts] CHECK CONSTRAINT [FK_BlogPosts_Blogs]
GO
/****** Object:  ForeignKey [FK_Blogs_Blogs]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Blogs]  WITH NOCHECK ADD  CONSTRAINT [FK_Blogs_Blogs] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Blogs] CHECK CONSTRAINT [FK_Blogs_Blogs]
GO
/****** Object:  ForeignKey [FK_Comments_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users] FOREIGN KEY([c_from_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users]
GO
/****** Object:  ForeignKey [FK_Comments_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users1] FOREIGN KEY([c_to_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users1]
GO
/****** Object:  ForeignKey [FK_CommunityPhotoApproval_Photos]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[CommunityPhotoApproval]  WITH CHECK ADD  CONSTRAINT [FK_CommunityPhotoApproval_Photos] FOREIGN KEY([p_id])
REFERENCES [dbo].[Photos] ([p_id])
GO
ALTER TABLE [dbo].[CommunityPhotoApproval] CHECK CONSTRAINT [FK_CommunityPhotoApproval_Photos]
GO
/****** Object:  ForeignKey [FK_CommunityPhotoApproval_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[CommunityPhotoApproval]  WITH CHECK ADD  CONSTRAINT [FK_CommunityPhotoApproval_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[CommunityPhotoApproval] CHECK CONSTRAINT [FK_CommunityPhotoApproval_Users]
GO
/****** Object:  ForeignKey [FK_CommunityProfileApproval_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[CommunityProfileApproval]  WITH CHECK ADD  CONSTRAINT [FK_CommunityProfileApproval_Users] FOREIGN KEY([cpa_approvedby])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[CommunityProfileApproval] CHECK CONSTRAINT [FK_CommunityProfileApproval_Users]
GO
/****** Object:  ForeignKey [FK2_CommunityProfileApproval_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[CommunityProfileApproval]  WITH CHECK ADD  CONSTRAINT [FK2_CommunityProfileApproval_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[CommunityProfileApproval] CHECK CONSTRAINT [FK2_CommunityProfileApproval_Users]
GO
/****** Object:  ForeignKey [FK_ContentPage_Languages]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ContentPage]  WITH CHECK ADD  CONSTRAINT [FK_ContentPage_Languages] FOREIGN KEY([l_id])
REFERENCES [dbo].[Languages] ([l_id])
GO
ALTER TABLE [dbo].[ContentPage] CHECK CONSTRAINT [FK_ContentPage_Languages]
GO
/****** Object:  ForeignKey [FK_CreditsHistory_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[CreditsHistory]  WITH CHECK ADD  CONSTRAINT [FK_CreditsHistory_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[CreditsHistory] CHECK CONSTRAINT [FK_CreditsHistory_Users]
GO
/****** Object:  ForeignKey [FK_Ecards_EcardTypes]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ecards]  WITH CHECK ADD  CONSTRAINT [FK_Ecards_EcardTypes] FOREIGN KEY([ect_id])
REFERENCES [dbo].[EcardTypes] ([ect_id])
GO
ALTER TABLE [dbo].[Ecards] CHECK CONSTRAINT [FK_Ecards_EcardTypes]
GO
/****** Object:  ForeignKey [FK_Ecards_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ecards]  WITH CHECK ADD  CONSTRAINT [FK_Ecards_Users] FOREIGN KEY([ec_from_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Ecards] CHECK CONSTRAINT [FK_Ecards_Users]
GO
/****** Object:  ForeignKey [FK_Ecards_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ecards]  WITH CHECK ADD  CONSTRAINT [FK_Ecards_Users1] FOREIGN KEY([ec_to_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Ecards] CHECK CONSTRAINT [FK_Ecards_Users1]
GO
/****** Object:  ForeignKey [FK_EstablishedCommunications_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[EstablishedCommunications]  WITH CHECK ADD  CONSTRAINT [FK_EstablishedCommunications_Users] FOREIGN KEY([ec_from_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[EstablishedCommunications] CHECK CONSTRAINT [FK_EstablishedCommunications_Users]
GO
/****** Object:  ForeignKey [FK_EstablishedCommunications_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[EstablishedCommunications]  WITH CHECK ADD  CONSTRAINT [FK_EstablishedCommunications_Users1] FOREIGN KEY([ec_to_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[EstablishedCommunications] CHECK CONSTRAINT [FK_EstablishedCommunications_Users1]
GO
/****** Object:  ForeignKey [FK_EventComments_EventComments]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[EventComments]  WITH CHECK ADD  CONSTRAINT [FK_EventComments_EventComments] FOREIGN KEY([e_id])
REFERENCES [dbo].[Events] ([e_id])
GO
ALTER TABLE [dbo].[EventComments] CHECK CONSTRAINT [FK_EventComments_EventComments]
GO
/****** Object:  ForeignKey [FK_EventComments_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[EventComments]  WITH CHECK ADD  CONSTRAINT [FK_EventComments_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[EventComments] CHECK CONSTRAINT [FK_EventComments_Users]
GO
/****** Object:  ForeignKey [FK_Events_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Groups] FOREIGN KEY([e_fromgroup])
REFERENCES [dbo].[Groups] ([g_id])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Groups]
GO
/****** Object:  ForeignKey [FK_Events_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Events]  WITH CHECK ADD  CONSTRAINT [FK_Events_Users] FOREIGN KEY([e_fromusername])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Events] CHECK CONSTRAINT [FK_Events_Users]
GO
/****** Object:  ForeignKey [FK_Favourites_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Favourites]  WITH CHECK ADD  CONSTRAINT [FK_Favourites_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Favourites] CHECK CONSTRAINT [FK_Favourites_Users]
GO
/****** Object:  ForeignKey [FK_Favourites_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Favourites]  WITH CHECK ADD  CONSTRAINT [FK_Favourites_Users1] FOREIGN KEY([f_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Favourites] CHECK CONSTRAINT [FK_Favourites_Users1]
GO
/****** Object:  ForeignKey [FK_Friends_Friends]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Friends] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Friends]
GO
/****** Object:  ForeignKey [FK_Friends_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Friends]  WITH CHECK ADD  CONSTRAINT [FK_Friends_Users] FOREIGN KEY([f_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Friends] CHECK CONSTRAINT [FK_Friends_Users]
GO
/****** Object:  ForeignKey [FK_GroupBans_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupBans]  WITH NOCHECK ADD  CONSTRAINT [FK_GroupBans_Groups] FOREIGN KEY([g_id])
REFERENCES [dbo].[Groups] ([g_id])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[GroupBans] CHECK CONSTRAINT [FK_GroupBans_Groups]
GO
/****** Object:  ForeignKey [FK_GroupBans_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupBans]  WITH NOCHECK ADD  CONSTRAINT [FK_GroupBans_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[GroupBans] CHECK CONSTRAINT [FK_GroupBans_Users]
GO
/****** Object:  ForeignKey [FK_GroupEvents_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEvents]  WITH CHECK ADD  CONSTRAINT [FK_GroupEvents_Groups] FOREIGN KEY([g_id])
REFERENCES [dbo].[Groups] ([g_id])
GO
ALTER TABLE [dbo].[GroupEvents] CHECK CONSTRAINT [FK_GroupEvents_Groups]
GO
/****** Object:  ForeignKey [FK_GroupEvents_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEvents]  WITH CHECK ADD  CONSTRAINT [FK_GroupEvents_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[GroupEvents] CHECK CONSTRAINT [FK_GroupEvents_Users]
GO
/****** Object:  ForeignKey [FK_GroupEventsAttenders_GroupEvents]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEventsAttenders]  WITH CHECK ADD  CONSTRAINT [FK_GroupEventsAttenders_GroupEvents] FOREIGN KEY([ge_id])
REFERENCES [dbo].[GroupEvents] ([ge_id])
GO
ALTER TABLE [dbo].[GroupEventsAttenders] CHECK CONSTRAINT [FK_GroupEventsAttenders_GroupEvents]
GO
/****** Object:  ForeignKey [FK_GroupEventsAttenders_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEventsAttenders]  WITH CHECK ADD  CONSTRAINT [FK_GroupEventsAttenders_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[GroupEventsAttenders] CHECK CONSTRAINT [FK_GroupEventsAttenders_Users]
GO
/****** Object:  ForeignKey [FK_GroupEventsComments_GroupEvents]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEventsComments]  WITH CHECK ADD  CONSTRAINT [FK_GroupEventsComments_GroupEvents] FOREIGN KEY([ge_id])
REFERENCES [dbo].[GroupEvents] ([ge_id])
GO
ALTER TABLE [dbo].[GroupEventsComments] CHECK CONSTRAINT [FK_GroupEventsComments_GroupEvents]
GO
/****** Object:  ForeignKey [FK_GroupEventsComments_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupEventsComments]  WITH CHECK ADD  CONSTRAINT [FK_GroupEventsComments_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[GroupEventsComments] CHECK CONSTRAINT [FK_GroupEventsComments_Users]
GO
/****** Object:  ForeignKey [FK_GroupMembers_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupMembers]  WITH CHECK ADD  CONSTRAINT [FK_GroupMembers_Groups] FOREIGN KEY([g_id])
REFERENCES [dbo].[Groups] ([g_id])
GO
ALTER TABLE [dbo].[GroupMembers] CHECK CONSTRAINT [FK_GroupMembers_Groups]
GO
/****** Object:  ForeignKey [FK_GroupMembers_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupMembers]  WITH CHECK ADD  CONSTRAINT [FK_GroupMembers_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[GroupMembers] CHECK CONSTRAINT [FK_GroupMembers_Users]
GO
/****** Object:  ForeignKey [FK_GroupPhotos_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPhotos]  WITH CHECK ADD  CONSTRAINT [FK_GroupPhotos_Groups] FOREIGN KEY([g_id])
REFERENCES [dbo].[Groups] ([g_id])
GO
ALTER TABLE [dbo].[GroupPhotos] CHECK CONSTRAINT [FK_GroupPhotos_Groups]
GO
/****** Object:  ForeignKey [FK_GroupPhotos_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPhotos]  WITH CHECK ADD  CONSTRAINT [FK_GroupPhotos_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[GroupPhotos] CHECK CONSTRAINT [FK_GroupPhotos_Users]
GO
/****** Object:  ForeignKey [FK_GroupPollsAnswers_GroupPollsChoices]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPollsAnswers]  WITH CHECK ADD  CONSTRAINT [FK_GroupPollsAnswers_GroupPollsChoices] FOREIGN KEY([gpc_id])
REFERENCES [dbo].[GroupPollsChoices] ([gpc_id])
GO
ALTER TABLE [dbo].[GroupPollsAnswers] CHECK CONSTRAINT [FK_GroupPollsAnswers_GroupPollsChoices]
GO
/****** Object:  ForeignKey [FK_GroupPollsAnswers_GroupTopics]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPollsAnswers]  WITH CHECK ADD  CONSTRAINT [FK_GroupPollsAnswers_GroupTopics] FOREIGN KEY([gt_id])
REFERENCES [dbo].[GroupTopics] ([gt_id])
GO
ALTER TABLE [dbo].[GroupPollsAnswers] CHECK CONSTRAINT [FK_GroupPollsAnswers_GroupTopics]
GO
/****** Object:  ForeignKey [FK_GroupPollsAnswers_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPollsAnswers]  WITH CHECK ADD  CONSTRAINT [FK_GroupPollsAnswers_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[GroupPollsAnswers] CHECK CONSTRAINT [FK_GroupPollsAnswers_Users]
GO
/****** Object:  ForeignKey [FK_GroupPollsChoices_GroupTopics]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPollsChoices]  WITH CHECK ADD  CONSTRAINT [FK_GroupPollsChoices_GroupTopics] FOREIGN KEY([gt_id])
REFERENCES [dbo].[GroupTopics] ([gt_id])
GO
ALTER TABLE [dbo].[GroupPollsChoices] CHECK CONSTRAINT [FK_GroupPollsChoices_GroupTopics]
GO
/****** Object:  ForeignKey [FK_GroupPosts_GroupTopics]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPosts]  WITH CHECK ADD  CONSTRAINT [FK_GroupPosts_GroupTopics] FOREIGN KEY([gt_id])
REFERENCES [dbo].[GroupTopics] ([gt_id])
GO
ALTER TABLE [dbo].[GroupPosts] CHECK CONSTRAINT [FK_GroupPosts_GroupTopics]
GO
/****** Object:  ForeignKey [FK_GroupPosts_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupPosts]  WITH NOCHECK ADD  CONSTRAINT [FK_GroupPosts_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[GroupPosts] NOCHECK CONSTRAINT [FK_GroupPosts_Users]
GO
/****** Object:  ForeignKey [FK_GroupsCategories_Categories]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupsCategories]  WITH CHECK ADD  CONSTRAINT [FK_GroupsCategories_Categories] FOREIGN KEY([c_id])
REFERENCES [dbo].[Categories] ([c_id])
GO
ALTER TABLE [dbo].[GroupsCategories] CHECK CONSTRAINT [FK_GroupsCategories_Categories]
GO
/****** Object:  ForeignKey [FK_GroupsCategories_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupsCategories]  WITH CHECK ADD  CONSTRAINT [FK_GroupsCategories_Groups] FOREIGN KEY([g_id])
REFERENCES [dbo].[Groups] ([g_id])
GO
ALTER TABLE [dbo].[GroupsCategories] CHECK CONSTRAINT [FK_GroupsCategories_Groups]
GO
/****** Object:  ForeignKey [FK_GroupTopics_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics]  WITH CHECK ADD  CONSTRAINT [FK_GroupTopics_Groups] FOREIGN KEY([g_id])
REFERENCES [dbo].[Groups] ([g_id])
GO
ALTER TABLE [dbo].[GroupTopics] CHECK CONSTRAINT [FK_GroupTopics_Groups]
GO
/****** Object:  ForeignKey [FK_GroupTopics_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopics]  WITH NOCHECK ADD  CONSTRAINT [FK_GroupTopics_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[GroupTopics] NOCHECK CONSTRAINT [FK_GroupTopics_Users]
GO
/****** Object:  ForeignKey [FK_GroupTopicSubscriptions_Groups]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopicSubscriptions]  WITH CHECK ADD  CONSTRAINT [FK_GroupTopicSubscriptions_Groups] FOREIGN KEY([g_id])
REFERENCES [dbo].[Groups] ([g_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupTopicSubscriptions] CHECK CONSTRAINT [FK_GroupTopicSubscriptions_Groups]
GO
/****** Object:  ForeignKey [FK_GroupTopicSubscriptions_GroupTopics1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopicSubscriptions]  WITH CHECK ADD  CONSTRAINT [FK_GroupTopicSubscriptions_GroupTopics1] FOREIGN KEY([gt_id])
REFERENCES [dbo].[GroupTopics] ([gt_id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupTopicSubscriptions] CHECK CONSTRAINT [FK_GroupTopicSubscriptions_GroupTopics1]
GO
/****** Object:  ForeignKey [FK_GroupTopicSubscriptions_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[GroupTopicSubscriptions]  WITH CHECK ADD  CONSTRAINT [FK_GroupTopicSubscriptions_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GroupTopicSubscriptions] CHECK CONSTRAINT [FK_GroupTopicSubscriptions_Users]
GO
/****** Object:  ForeignKey [FK_Interests_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Interests]  WITH CHECK ADD  CONSTRAINT [FK_Interests_Users] FOREIGN KEY([i_from_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Interests] CHECK CONSTRAINT [FK_Interests_Users]
GO
/****** Object:  ForeignKey [FK_Interests_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Interests]  WITH CHECK ADD  CONSTRAINT [FK_Interests_Users1] FOREIGN KEY([i_to_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Interests] CHECK CONSTRAINT [FK_Interests_Users1]
GO
/****** Object:  ForeignKey [FK_Messages_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users] FOREIGN KEY([m_from_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users]
GO
/****** Object:  ForeignKey [FK_Messages_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Messages]  WITH CHECK ADD  CONSTRAINT [FK_Messages_Users1] FOREIGN KEY([m_to_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Messages] CHECK CONSTRAINT [FK_Messages_Users1]
GO
/****** Object:  ForeignKey [FK_MessagesSandbox_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[MessagesSandbox]  WITH CHECK ADD  CONSTRAINT [FK_MessagesSandbox_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[MessagesSandbox] CHECK CONSTRAINT [FK_MessagesSandbox_Users]
GO
/****** Object:  ForeignKey [FK_News_Languages]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[News]  WITH NOCHECK ADD  CONSTRAINT [FK_News_Languages] FOREIGN KEY([l_id])
REFERENCES [dbo].[Languages] ([l_id])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[News] NOCHECK CONSTRAINT [FK_News_Languages]
GO
/****** Object:  ForeignKey [FK_PaymentHistory_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PaymentHistory]  WITH NOCHECK ADD  CONSTRAINT [FK_PaymentHistory_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[PaymentHistory] NOCHECK CONSTRAINT [FK_PaymentHistory_Users]
GO
/****** Object:  ForeignKey [FK_PhotoAccess_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoAccess]  WITH CHECK ADD  CONSTRAINT [FK_PhotoAccess_Users] FOREIGN KEY([pa_photoowner])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoAccess] CHECK CONSTRAINT [FK_PhotoAccess_Users]
GO
/****** Object:  ForeignKey [FK_PhotoAccess_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoAccess]  WITH CHECK ADD  CONSTRAINT [FK_PhotoAccess_Users1] FOREIGN KEY([pa_photoviewer])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoAccess] CHECK CONSTRAINT [FK_PhotoAccess_Users1]
GO
/****** Object:  ForeignKey [FK_PhotoAlbums_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoAlbums]  WITH CHECK ADD  CONSTRAINT [FK_PhotoAlbums_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoAlbums] CHECK CONSTRAINT [FK_PhotoAlbums_Users]
GO
/****** Object:  ForeignKey [FK_PhotoComments_Photos]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoComments]  WITH CHECK ADD  CONSTRAINT [FK_PhotoComments_Photos] FOREIGN KEY([p_id])
REFERENCES [dbo].[Photos] ([p_id])
GO
ALTER TABLE [dbo].[PhotoComments] CHECK CONSTRAINT [FK_PhotoComments_Photos]
GO
/****** Object:  ForeignKey [FK_PhotoComments_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoComments]  WITH CHECK ADD  CONSTRAINT [FK_PhotoComments_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoComments] CHECK CONSTRAINT [FK_PhotoComments_Users]
GO
/****** Object:  ForeignKey [FK_PhotoContestPhotos_PhotoContests]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContestEntries]  WITH CHECK ADD  CONSTRAINT [FK_PhotoContestPhotos_PhotoContests] FOREIGN KEY([pc_id])
REFERENCES [dbo].[PhotoContests] ([pc_id])
GO
ALTER TABLE [dbo].[PhotoContestEntries] CHECK CONSTRAINT [FK_PhotoContestPhotos_PhotoContests]
GO
/****** Object:  ForeignKey [FK_PhotoContestRanks_PhotoContestEntries]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContestRanks]  WITH CHECK ADD  CONSTRAINT [FK_PhotoContestRanks_PhotoContestEntries] FOREIGN KEY([pce_id])
REFERENCES [dbo].[PhotoContestEntries] ([pce_id])
GO
ALTER TABLE [dbo].[PhotoContestRanks] CHECK CONSTRAINT [FK_PhotoContestRanks_PhotoContestEntries]
GO
/****** Object:  ForeignKey [FK_PhotoContestRanks_PhotoContests]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContestRanks]  WITH CHECK ADD  CONSTRAINT [FK_PhotoContestRanks_PhotoContests] FOREIGN KEY([pc_id])
REFERENCES [dbo].[PhotoContests] ([pc_id])
GO
ALTER TABLE [dbo].[PhotoContestRanks] CHECK CONSTRAINT [FK_PhotoContestRanks_PhotoContests]
GO
/****** Object:  ForeignKey [FK_PhotoContestRanks_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContestRanks]  WITH CHECK ADD  CONSTRAINT [FK_PhotoContestRanks_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoContestRanks] CHECK CONSTRAINT [FK_PhotoContestRanks_Users]
GO
/****** Object:  ForeignKey [FK_PhotoContestVotes_PhotoContestEntries]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContestVotes]  WITH CHECK ADD  CONSTRAINT [FK_PhotoContestVotes_PhotoContestEntries] FOREIGN KEY([pce_id_nonpicked])
REFERENCES [dbo].[PhotoContestEntries] ([pce_id])
GO
ALTER TABLE [dbo].[PhotoContestVotes] CHECK CONSTRAINT [FK_PhotoContestVotes_PhotoContestEntries]
GO
/****** Object:  ForeignKey [FK_PhotoContestVotes_PhotoContestEntries1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContestVotes]  WITH CHECK ADD  CONSTRAINT [FK_PhotoContestVotes_PhotoContestEntries1] FOREIGN KEY([pce_id_picked])
REFERENCES [dbo].[PhotoContestEntries] ([pce_id])
GO
ALTER TABLE [dbo].[PhotoContestVotes] CHECK CONSTRAINT [FK_PhotoContestVotes_PhotoContestEntries1]
GO
/****** Object:  ForeignKey [FK_PhotoContestVotes_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoContestVotes]  WITH CHECK ADD  CONSTRAINT [FK_PhotoContestVotes_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoContestVotes] CHECK CONSTRAINT [FK_PhotoContestVotes_Users]
GO
/****** Object:  ForeignKey [FK_PhotoNotes_Photos]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoNotes]  WITH CHECK ADD  CONSTRAINT [FK_PhotoNotes_Photos] FOREIGN KEY([p_id])
REFERENCES [dbo].[Photos] ([p_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[PhotoNotes] CHECK CONSTRAINT [FK_PhotoNotes_Photos]
GO
/****** Object:  ForeignKey [FK_PhotoNotes_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoNotes]  WITH CHECK ADD  CONSTRAINT [FK_PhotoNotes_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoNotes] CHECK CONSTRAINT [FK_PhotoNotes_Users]
GO
/****** Object:  ForeignKey [FK_PhotoRatings_Photos]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoRatings]  WITH NOCHECK ADD  CONSTRAINT [FK_PhotoRatings_Photos] FOREIGN KEY([p_id])
REFERENCES [dbo].[Photos] ([p_id])
GO
ALTER TABLE [dbo].[PhotoRatings] CHECK CONSTRAINT [FK_PhotoRatings_Photos]
GO
/****** Object:  ForeignKey [FK_PhotoRatings_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PhotoRatings]  WITH CHECK ADD  CONSTRAINT [FK_PhotoRatings_Users] FOREIGN KEY([pr_fromusername])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PhotoRatings] CHECK CONSTRAINT [FK_PhotoRatings_Users]
GO
/****** Object:  ForeignKey [FK_Photos_PhotoAlbums]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos]  WITH CHECK ADD  CONSTRAINT [FK_Photos_PhotoAlbums] FOREIGN KEY([pa_id])
REFERENCES [dbo].[PhotoAlbums] ([pa_id])
GO
ALTER TABLE [dbo].[Photos] CHECK CONSTRAINT [FK_Photos_PhotoAlbums]
GO
/****** Object:  ForeignKey [FK_Photos_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Photos]  WITH NOCHECK ADD  CONSTRAINT [FK_Photos_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Photos] CHECK CONSTRAINT [FK_Photos_Users]
GO
/****** Object:  ForeignKey [FK_PollAnswers_PollChoices]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PollAnswers]  WITH CHECK ADD  CONSTRAINT [FK_PollAnswers_PollChoices] FOREIGN KEY([pc_id])
REFERENCES [dbo].[PollChoices] ([pc_id])
GO
ALTER TABLE [dbo].[PollAnswers] CHECK CONSTRAINT [FK_PollAnswers_PollChoices]
GO
/****** Object:  ForeignKey [FK_PollAnswers_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PollAnswers]  WITH NOCHECK ADD  CONSTRAINT [FK_PollAnswers_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[PollAnswers] CHECK CONSTRAINT [FK_PollAnswers_Users]
GO
/****** Object:  ForeignKey [FK_PollChoices_PollChoices]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[PollChoices]  WITH CHECK ADD  CONSTRAINT [FK_PollChoices_PollChoices] FOREIGN KEY([p_id])
REFERENCES [dbo].[Polls] ([p_id])
GO
ALTER TABLE [dbo].[PollChoices] CHECK CONSTRAINT [FK_PollChoices_PollChoices]
GO
/****** Object:  ForeignKey [FK_ProfileAnswers_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileAnswers]  WITH CHECK ADD  CONSTRAINT [FK_ProfileAnswers_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[ProfileAnswers] CHECK CONSTRAINT [FK_ProfileAnswers_Users]
GO
/****** Object:  ForeignKey [FK_ProfileDetails_ProfileFields]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileAnswers]  WITH CHECK ADD  CONSTRAINT [FK_ProfileDetails_ProfileFields] FOREIGN KEY([pq_id])
REFERENCES [dbo].[ProfileQuestions] ([pq_id])
GO
ALTER TABLE [dbo].[ProfileAnswers] CHECK CONSTRAINT [FK_ProfileDetails_ProfileFields]
GO
/****** Object:  ForeignKey [FK_ProfileFieldValues_ProfileFields]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileChoices]  WITH NOCHECK ADD  CONSTRAINT [FK_ProfileFieldValues_ProfileFields] FOREIGN KEY([pq_id])
REFERENCES [dbo].[ProfileQuestions] ([pq_id])
GO
ALTER TABLE [dbo].[ProfileChoices] CHECK CONSTRAINT [FK_ProfileFieldValues_ProfileFields]
GO
/****** Object:  ForeignKey [FK_ProfileFields_ProfileCategories]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileQuestions]  WITH NOCHECK ADD  CONSTRAINT [FK_ProfileFields_ProfileCategories] FOREIGN KEY([pt_id])
REFERENCES [dbo].[ProfileTopics] ([pt_id])
GO
ALTER TABLE [dbo].[ProfileQuestions] CHECK CONSTRAINT [FK_ProfileFields_ProfileCategories]
GO
/****** Object:  ForeignKey [FK_ProfileViews_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileViews]  WITH CHECK ADD  CONSTRAINT [FK_ProfileViews_Users] FOREIGN KEY([pv_viewer])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[ProfileViews] CHECK CONSTRAINT [FK_ProfileViews_Users]
GO
/****** Object:  ForeignKey [FK_ProfileViews_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[ProfileViews]  WITH CHECK ADD  CONSTRAINT [FK_ProfileViews_Users1] FOREIGN KEY([pv_viewed])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[ProfileViews] CHECK CONSTRAINT [FK_ProfileViews_Users1]
GO
/****** Object:  ForeignKey [FK_Ratings_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_Users] FOREIGN KEY([r_fromusername])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_Users]
GO
/****** Object:  ForeignKey [FK_Ratings_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Ratings]  WITH CHECK ADD  CONSTRAINT [FK_Ratings_Users1] FOREIGN KEY([r_tousername])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Ratings] CHECK CONSTRAINT [FK_Ratings_Users1]
GO
/****** Object:  ForeignKey [FK_Relationships_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Relationships]  WITH CHECK ADD  CONSTRAINT [FK_Relationships_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Relationships] CHECK CONSTRAINT [FK_Relationships_Users]
GO
/****** Object:  ForeignKey [FK_Relationships_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Relationships]  WITH CHECK ADD  CONSTRAINT [FK_Relationships_Users1] FOREIGN KEY([r_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Relationships] CHECK CONSTRAINT [FK_Relationships_Users1]
GO
/****** Object:  ForeignKey [FK_SavedSearches_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[SavedSearches]  WITH CHECK ADD  CONSTRAINT [FK_SavedSearches_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[SavedSearches] CHECK CONSTRAINT [FK_SavedSearches_Users]
GO
/****** Object:  ForeignKey [FK_Subscriptions_BillingPlans]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_BillingPlans] FOREIGN KEY([p_id])
REFERENCES [dbo].[BillingPlans] ([p_id])
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_BillingPlans]
GO
/****** Object:  ForeignKey [FK_Subscriptions_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Subscriptions]  WITH CHECK ADD  CONSTRAINT [FK_Subscriptions_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[Subscriptions] CHECK CONSTRAINT [FK_Subscriptions_Users]
GO
/****** Object:  ForeignKey [FK_Translations_Languages]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[Translations]  WITH NOCHECK ADD  CONSTRAINT [FK_Translations_Languages] FOREIGN KEY([l_id])
REFERENCES [dbo].[Languages] ([l_id])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[Translations] NOCHECK CONSTRAINT [FK_Translations_Languages]
GO
/****** Object:  ForeignKey [FK_TwitterCredentials_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[TwitterCredentials]  WITH CHECK ADD  CONSTRAINT [FK_TwitterCredentials_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[TwitterCredentials] CHECK CONSTRAINT [FK_TwitterCredentials_Users]
GO
/****** Object:  ForeignKey [FK_UnlockedSections_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[UnlockedSections]  WITH CHECK ADD  CONSTRAINT [FK_UnlockedSections_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[UnlockedSections] CHECK CONSTRAINT [FK_UnlockedSections_Users]
GO
/****** Object:  ForeignKey [FK_UnlockedSections_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[UnlockedSections]  WITH CHECK ADD  CONSTRAINT [FK_UnlockedSections_Users1] FOREIGN KEY([us_targetusername])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[UnlockedSections] CHECK CONSTRAINT [FK_UnlockedSections_Users1]
GO
/****** Object:  ForeignKey [FK_UserScoresLog_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[UserScoresLog]  WITH NOCHECK ADD  CONSTRAINT [FK_UserScoresLog_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
NOT FOR REPLICATION
GO
ALTER TABLE [dbo].[UserScoresLog] NOCHECK CONSTRAINT [FK_UserScoresLog_Users]
GO
/****** Object:  ForeignKey [FK_VerifiedUsers_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VerifiedUsers]  WITH CHECK ADD  CONSTRAINT [FK_VerifiedUsers_Users] FOREIGN KEY([vu_verifiedby])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[VerifiedUsers] CHECK CONSTRAINT [FK_VerifiedUsers_Users]
GO
/****** Object:  ForeignKey [FK_VerifiedUsers_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VerifiedUsers]  WITH CHECK ADD  CONSTRAINT [FK_VerifiedUsers_Users1] FOREIGN KEY([vu_verifieduser])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[VerifiedUsers] CHECK CONSTRAINT [FK_VerifiedUsers_Users1]
GO
/****** Object:  ForeignKey [FK_VideoAccess_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoAccess]  WITH CHECK ADD  CONSTRAINT [FK_VideoAccess_Users] FOREIGN KEY([va_videoowner])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[VideoAccess] CHECK CONSTRAINT [FK_VideoAccess_Users]
GO
/****** Object:  ForeignKey [FK_VideoAccess_Users1]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoAccess]  WITH CHECK ADD  CONSTRAINT [FK_VideoAccess_Users1] FOREIGN KEY([va_videoviewer])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[VideoAccess] CHECK CONSTRAINT [FK_VideoAccess_Users1]
GO
/****** Object:  ForeignKey [FK_VideoEmbeds_VideoEmbeds]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoEmbeds]  WITH CHECK ADD  CONSTRAINT [FK_VideoEmbeds_VideoEmbeds] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[VideoEmbeds] CHECK CONSTRAINT [FK_VideoEmbeds_VideoEmbeds]
GO
/****** Object:  ForeignKey [FK_VideoProfiles_Users]    Script Date: 05/14/2010 18:01:41 ******/
ALTER TABLE [dbo].[VideoProfiles]  WITH CHECK ADD  CONSTRAINT [FK_VideoProfiles_Users] FOREIGN KEY([u_username])
REFERENCES [dbo].[Users] ([u_username])
GO
ALTER TABLE [dbo].[VideoProfiles] CHECK CONSTRAINT [FK_VideoProfiles_Users]
GO
