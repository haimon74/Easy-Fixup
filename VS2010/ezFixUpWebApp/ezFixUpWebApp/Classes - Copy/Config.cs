/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Hosting;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Data
{
    public class Config
    {
        public class WebConfigSettings
        {
            public static string FacebookImagesFetchGridSize
            {
                get { return ConfigurationManager.AppSettings["facebook_image_size"]; }
            }
        }

        #region Nested type: Server Settings

        [Reflection.DescriptionAttribute("Server Settings")]
        public class ServerSettings
        {
            [Reflection.DescriptionAttribute("Domain URL")]
            public static string DomainURL
            {
                get { return DBSettings.Get("ServerSettings_DomainURL", "https://lovehitch.com"); }
                set { DBSettings.Set("ServerSettings_DomainURL", value); }
            }

            [Reflection.DescriptionAttribute("DB Connection String (TBD)")]
            public static string ConnectionString
            {
                get { return DBSettings.Get("ServerSettings_ConnectionString", @"Server=localhost\sqe;Database=lhdb;User Id=lhdba;Password=lovaLoaver21!"); }
                set { DBSettings.Set("ServerSettings_ConnectionString", value); }
            }

            [Reflection.DescriptionAttribute("Images Folder")]
            public static bool FacebookDefaultImageIsNormal
            {
                get { return DBSettings.Get("FacebookSettings_IsImageSizeNormal", true); }
                set { DBSettings.Set("FacebookSettings_IsImageSizeNormal", value); }
            }
        }
        #endregion

        #region Nested type: Facebook Settings

        [Reflection.DescriptionAttribute("Facebook Settings")]
        public class FacebookSettings
        {
            [Reflection.DescriptionAttribute("Facebook App Key")]
            public static string FacebookApplicationId
            {
                get { return DBSettings.Get("FacebookSettings_AppId", "139801212735949"); }
                set { DBSettings.Set("FacebookSettings_AppId", value); }
            }

            [Reflection.DescriptionAttribute("Facebook App Secret")]
            public static string FacebookAppSecret
            {
                get { return DBSettings.Get("FacebookSettings_AppSecret", "880d7ba96e17bd1b22ee68cf3f4ad83e"); }
                set { DBSettings.Set("FacebookSettings_AppSecret", value); }
            }

            [Reflection.DescriptionAttribute("Default Image Size (check for normal, uncheck for large)")]
            public static bool FacebookDefaultImageIsNormal
            {
                get { return DBSettings.Get("FacebookSettings_IsImageSizeNormal", true); }
                set { DBSettings.Set("FacebookSettings_IsImageSizeNormal", value); }
            }
        }
        #endregion

        #region Nested type: DEBUG Settings

        [Reflection.DescriptionAttribute("DEBUG Settings")]
        public class DebugSettings
        {
            [Reflection.DescriptionAttribute("Enable Debug Traces")]
            public static bool EnableTrace
            {
                get { return DBSettings.Get("DebugSettings_EnableTrace", false); }
                set { DBSettings.Set("DebugSettings_EnableTrace", value); }
            }

            [Reflection.DescriptionAttribute("Maximum Parallel Threads")]
            public static int MaxParallelThreads
            {
                get { return DBSettings.Get("DebugSettings_MaxThreads", 5); }
                set { DBSettings.Set("DebugSettings_MaxThreads", value); }
            }

            [Reflection.DescriptionAttribute("Enable Timers")]
            public static bool EnableTimers
            {
                get { return DBSettings.Get("DebugSettings_EnableTimers", true); }
                set { DBSettings.Set("DebugSettings_EnableTimers", value); }
            }

            [Reflection.DescriptionAttribute("Enable Caching")]
            public static bool EnableCaching
            {
                get { return DBSettings.Get("DebugSettings_EnableCaching", true); }
                set { DBSettings.Set("DebugSettings_EnableCaching", value); }
            }
        }

        #endregion

        #region Nested type: AbuseReports

        [Reflection.DescriptionAttribute("Abuse Reports")]
        public class AbuseReports
        {
            [Reflection.DescriptionAttribute("User can report profile abuse")]
            public static bool UserCanReportProfileAbuse
            {
                get { return DBSettings.Get("AbuseReports_UserCanReportProfileAbuse", false); }
                set { DBSettings.Set("AbuseReports_UserCanReportProfileAbuse", value); }
            }

            [Reflection.DescriptionAttribute("User can report photo abuse")]
            public static bool UserCanReportPhotoAbuse
            {
                get { return DBSettings.Get("AbuseReports_UserCanReportPhotoAbuse", false); }
                set { DBSettings.Set("AbuseReports_UserCanReportPhotoAbuse", value); }
            }

            [Reflection.DescriptionAttribute("User can report message abuse")]
            public static bool UserCanReportMessageAbuse
            {
                get { return DBSettings.Get("AbuseReports_UserCanReportMessageAbuse", false); }
                set { DBSettings.Set("AbuseReports_UserCanReportMessageAbuse", value); }
            }

            [Reflection.DescriptionAttribute("Abuse Reports per Page")]
            public static int ReportsPerPage
            {
                get { return DBSettings.Get("AbuseReports_AbuseReportsPerPage", 20); }
                set { DBSettings.Set("AbuseReports_AbuseReportsPerPage", value); }
            }
        }

        #endregion

        #region Nested type: AdminSettings

        public class AdminSettings
        {
            public static bool ExecuteTimers
            {
                get { return Properties.Settings.Default.ExecuteTimers; }
            }

            /// <summary>
            /// Gets a value indicating whether [read only].
            /// </summary>
            /// <value><c>true</c> if [read only]; otherwise, <c>false</c>.</value>
            public static bool ReadOnly
            {
                get { return Properties.Settings.Default.AdminReadOnly; }
            }

            public static bool AdminPermissionsEnabled
            {
                get { return Properties.Settings.Default.AdminPermissions; }
            }

            #region Nested type: ApproveAnswers

            public class ApproveAnswers
            {
                private static int answersPerPage = 10;

                /// <summary>
                /// Gets or sets the answers per page.
                /// </summary>
                /// <value>The answers per page.</value>
                public static int AnswersPerPage
                {
                    get { return answersPerPage; }
                    set { answersPerPage = value; }
                }
            }

            public class ApproveBlogPosts
            {
                private static int blogPostsPerPage = 10;

                public static int BlogPostsPerPage
                {
                    get { return blogPostsPerPage; }
                    set { blogPostsPerPage = value; }
                }
            }

            #endregion

            #region Nested type: ApproveComments

            public class ApproveComments
            {
                private static bool autoApprove = true;

                private static int commentsPerPage = 10;

                /// <summary>
                /// Gets or sets a value indicating whether [auto approve].
                /// </summary>
                /// <value><c>true</c> if [auto approve]; otherwise, <c>false</c>.</value>
                public static bool AutoApprove
                {
                    get { return autoApprove; }
                    set { autoApprove = value; }
                }

                /// <summary>
                /// Gets or sets the comments per page.
                /// </summary>
                /// <value>The comments per page.</value>
                public static int CommentsPerPage
                {
                    get { return commentsPerPage; }
                    set { commentsPerPage = value; }
                }
            }

            #endregion

            #region Nested type: ApproveGroups

            public class ApproveGroups
            {
                private static int groupsPerPage = 10;

                /// <summary>
                /// Gets or sets the groups per page.
                /// </summary>
                /// <value>The groups per page.</value>
                public static int GroupsPerPage
                {
                    get { return groupsPerPage; }
                    set { groupsPerPage = value; }
                }
            }

            #endregion

            #region Nested type: ApproveAds

            public class ApproveAds
            {
                private static int adsPerPage = 10;

                public static int AdsPerPage
                {
                    get { return adsPerPage; }
                    set { adsPerPage = value; }
                }
            }

            #endregion

            #region Nested type: ApprovePhotos

            public class ApprovePhotos
            {
                private static int photosPerPage = 5;

                /// <summary>
                /// Gets or sets the photos per page.
                /// </summary>
                /// <value>The photos per page.</value>
                public static int PhotosPerPage
                {
                    get { return photosPerPage; }
                    set { photosPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseAffiliateCommissoinsHistory

            public class BrowseAffiliateCommissoinsHistory
            {
                private static int affiliateCommissionsPerPage = 20;

                /// <summary>
                /// Gets or sets the affiliate commissions per page.
                /// </summary>
                /// <value>The affiliates payment history per page.</value>
                public static int AffiliateCommissionsHistoryPerPage
                {
                    get { return affiliateCommissionsPerPage; }
                    set { affiliateCommissionsPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseAffiliates

            public class BrowseAffiliates
            {
                private static int affiliatesPerPage = 20;

                /// <summary>
                /// Gets or sets the affiliates per page.
                /// </summary>
                /// <value>The affiliates per page.</value>
                public static int AffiliatesPerPage
                {
                    get { return affiliatesPerPage; }
                    set { affiliatesPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseAffiliatesPaymentHistory

            public class BrowseAffiliatesPaymentHistory
            {
                private static int affiliatesPaymentHistoryPerPage = 20;

                /// <summary>
                /// Gets or sets the affiliates payment history per page.
                /// </summary>
                /// <value>The affiliates payment history per page.</value>
                public static int AffiliatePaymentHistoryPerPage
                {
                    get { return affiliatesPaymentHistoryPerPage; }
                    set { affiliatesPaymentHistoryPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseGroups

            public class BrowseGroups
            {
                private static int groupsPerPage = 20;

                /// <summary>
                /// Gets or sets the groups per page.
                /// </summary>
                /// <value>The groups per page.</value>
                public static int GroupsPerPage
                {
                    get { return groupsPerPage; }
                    set { groupsPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseMessages

            public class BrowseMessages
            {
                private static int messagesPerPage = 20;

                /// <summary>
                /// Gets or sets the Messages per page.
                /// </summary>
                /// <value>The Messages per page.</value>
                public static int MessagesPerPage
                {
                    get { return messagesPerPage; }
                    set { messagesPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowsePhotos

            public class BrowsePhotos
            {
                private static int photosPerPage = 20;

                /// <summary>
                /// Gets or sets the Photos per page.
                /// </summary>
                /// <value>The Photos per page.</value>
                public static int PhotosPerPage
                {
                    get { return photosPerPage; }
                    set { photosPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseSpamSuspects

            public class BrowseSpamSuspects
            {
                private static int usersPerPage = 20;

                /// <summary>
                /// Gets or sets the users per page.
                /// </summary>
                /// <value>The users per page.</value>
                public static int UsersPerPage
                {
                    get { return usersPerPage; }
                    set { usersPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseUsers

            public class BrowseUsers
            {
                private static int usersPerPage = 20;

                /// <summary>
                /// Gets or sets the users per page.
                /// </summary>
                /// <value>The users per page.</value>
                public static int UsersPerPage
                {
                    get { return usersPerPage; }
                    set { usersPerPage = value; }
                }
            }

            #endregion

            #region Nested type: BrowseVideoUploads

            public class BrowseVideoUploads
            {
                private static int videoUploadsPerPage = 20;

                /// <summary>
                /// Gets or sets the Messages per page.
                /// </summary>
                /// <value>The Messages per page.</value>
                public static int VideoUploadsPerPage
                {
                    get { return videoUploadsPerPage; }
                    set { videoUploadsPerPage = value; }
                }
            }

            #endregion

            #region Nested type: Payments

            public class Payments
            {
                public static string[] PaymentProcessors
                {
                    get
                    {
                        var paymentProcessors = new string[Properties.Settings.Default.PaymentProcessors.Count];
                        Properties.Settings.Default.PaymentProcessors.CopyTo(paymentProcessors, 0);
                        return paymentProcessors;
                    }
                }

                public static bool PayPalSandbox
                {
                    get { return Properties.Settings.Default.PayPalSandBox; }
                }

                public static string PayPalEmail
                {
                    get { return Properties.Settings.Default.PayPalEmail; }
                }

                public static string AlertPayEmail
                {
                    get { return Properties.Settings.Default.AlertPayEmail; }
                }
            }

            #endregion
        }

        #endregion

        #region Nested type: Affiliates

        [Reflection.DescriptionAttribute("Affiliate Settings")]
        public class Affiliates
        {
            [Reflection.DescriptionAttribute("Check to enable affiliates")]
            [Reflection.HintAttribute("Enable/disable affiliates.")]
            public static bool Enable
            {
                get { return DBSettings.Get("Affiliates_Enable", true); }
                set { DBSettings.Set("Affiliates_Enable", value); }
            }

            /// <summary>
            /// Gets or sets the length of the username min.
            /// </summary>
            /// <value>The length of the username min.</value>
            [Reflection.DescriptionAttribute("Username minimum length")]
            [Reflection.HintAttribute("Define the minimal username length. Do NOT change on already running site!")]
            public static int UsernameMinLength
            {
                get { return DBSettings.Get("Affiliates_UsernameMinLength", 3); }
                set { DBSettings.Set("Affiliates_UsernameMinLength", value); }
            }

            /// <summary>
            /// Gets or sets the length of the username max.
            /// </summary>
            /// <value>The length of the username max.</value>
            [Reflection.DescriptionAttribute("Username maximum length")]
            [Reflection.HintAttribute("Define the maximal username length. Do NOT change on already running site!")]
            public static int UsernameMaxLength
            {
                get { return DBSettings.Get("Affiliates_UsernameMaxLength", 20); }
                set { DBSettings.Set("Affiliates_UsernameMaxLength", value); }
            }

            [Reflection.DescriptionAttribute("Password minimum length")]
            [Reflection.HintAttribute("Define the minimal password length. Recommended value is 3.")]
            public static int PasswordMinLength
            {
                get { return DBSettings.Get("Affiliates_PasswordMinLength", 3); }
                set { DBSettings.Set("Affiliates_PasswordMinLength", value); }
            }

            /// <summary>
            /// Gets or sets the length of the password max.
            /// </summary>
            /// <value>The length of the password max.</value>
            [Reflection.DescriptionAttribute("Password maximum length")]
            [Reflection.HintAttribute("Define the maximal password length.")]
            public static int PasswordMaxLength
            {
                get { return DBSettings.Get("Affiliates_PasswordMaxLength", 250); }
                set { DBSettings.Set("Affiliates_PasswordMaxLength", value); }
            }

            [Reflection.DescriptionAttribute("Minimum sum to request payment")]
            [Reflection.HintAttribute("Define the minimal sum to request payment.")]
            public static int PaymentRequestMinSum
            {
                get { return DBSettings.Get("Affiliates_PaymentRequestMinSum", 50); }
                set { DBSettings.Set("Affiliates_PaymentRequestMinSum", value); }
            }

            [Reflection.DescriptionAttribute("Percentage for affiliates")]
            [Reflection.HintAttribute("Specify percentage for affiliates.")]
            public static int Percentage
            {
                get { return DBSettings.Get("Affiliates_Percentage", 35); }
                set { DBSettings.Set("Affiliates_Percentage", value); }
            }

            [Reflection.DescriptionAttribute("Fixed amount for affiliates")]
            [Reflection.HintAttribute("Specify fixed amount for affiliates.")]
            public static decimal FixedAmount
            {
                get { return DBSettings.Get("Affiliates_FixedAmount", 0M); }
                set { DBSettings.Set("Affiliates_FixedAmount", value); }
            }

            [Reflection.DescriptionAttribute("Check to enable recurrent payment for affiliates")]
            [Reflection.HintAttribute("Enable/disable recurrent payment for affiliates.")]
            public static bool Recurrent
            {
                get { return DBSettings.Get("Affiliates_Recurrent", false); }
                set { DBSettings.Set("Affiliates_Recurrent", value); }
            }
        }

        #endregion

        #region Nested type: Ads

        [Reflection.DescriptionAttribute("Classifieds Settings")]
        public class Ads
        {
            [Reflection.DescriptionAttribute("Check to enable classifieds")]
            [Reflection.HintAttribute("Enable/disable classifieds.")]
            public static bool Enable
            {
                get { return DBSettings.Get("Ads_Enable", true); }
                set { DBSettings.Set("Ads_Enable", value); }
            }

            [Reflection.DescriptionAttribute("Only registered users can browse classifieds")]
            [Reflection.HintAttribute("When it is checked only registered users can browse classifieds")]
            public static bool OnlyRegisteredUsersCanBrowseClassifieds
            {
                get { return DBSettings.Get("Ads_OnlyRegisteredUsersCanBrowseClassifieds", false); }
                set { DBSettings.Set("Ads_OnlyRegisteredUsersCanBrowseClassifieds", value); }
            }

            [Reflection.DescriptionAttribute("Maximum group photo width")]
            [Reflection.HintAttribute("Specify the maximum width of group photo.")]
            public static int AdPhotoMaxWidth
            {
                get { return DBSettings.Get("Ads_AdPhotoMaxWidth", 450); }
                set { DBSettings.Set("Ads_AdPhotoMaxWidth", value); }
            }

            [Reflection.DescriptionAttribute("Maximum group photo height")]
            [Reflection.HintAttribute("Specify the maximum height of group photo.")]
            public static int AdPhotoMaxHeight
            {
                get { return DBSettings.Get("Ads_AdPhotoMaxHeight", 450); }
                set { DBSettings.Set("Ads_AdPhotoMaxHeight", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of photos per ad")]
            [Reflection.HintAttribute("Specify the maximum number of photos per ad.")]
            public static int MaxPhotosPerAd
            {
                get { return DBSettings.Get("Ads_MaxPhotosPerAd", 5); }
                set { DBSettings.Set("Ads_MaxPhotosPerAd", value); }
            }

            [Reflection.DescriptionAttribute("Ads per page")]
            [Reflection.HintAttribute("Specify ads per page.")]
            public static int AdsPerPage
            {
                get { return DBSettings.Get("Ads_AdsPerPage", 10); }
                set { DBSettings.Set("Ads_AdsPerPage", value); }
            }

            [Reflection.DescriptionAttribute("Classified comments")]
            [Reflection.HintAttribute("Check to allow members to post comments.")]
            public static bool EnableAdComments
            {
                get { return DBSettings.Get("Ads_EnableAdComments", true); }
                set { DBSettings.Set("Ads_EnableAdComments", value); }
            }
        }

        #endregion

        #region Nested type: CommunityFaceControlSystem

        [Reflection.DescriptionAttribute("Community Face Control System")]
        public class CommunityFaceControlSystem
        {
            [Reflection.DescriptionAttribute("Enable community face control")]
            public static bool EnableCommunityFaceControl
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_EnableCommunityFaceControl", false); }
                set { DBSettings.Set("CommunityFaceControlSystem_EnableCommunityFaceControl", value); }
            }

            [Reflection.DescriptionAttribute("Minimum photos required for face control ")]
            public static int MinPhotosRequired
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_MinPhotosRequired", 3); }
                set { DBSettings.Set("CommunityFaceControlSystem_MinPhotosRequired", value); }
            }

            [Reflection.DescriptionAttribute("Minimum scores to allow profile moderation")]
            public static int MinimumScoresToAllowModeration
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_MinimumScoresToAllowModeration", -20); }
                set { DBSettings.Set("CommunityFaceControlSystem_MinimumScoresToAllowModeration", value); }
            }

            [Reflection.DescriptionAttribute("Show only gender of interest")]
            public static bool ShowOnlyGenderOfInterest
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_ShowOnlyGenderOfInterest", true); }
                set { DBSettings.Set("CommunityFaceControlSystem_ShowOnlyGenderOfInterest", value); }
            }

            [Reflection.DescriptionAttribute(
                "Required number of votes to determine whether the profile will be approved or rejected")]
            public static int RequiredNumberOfVotesToDetermine
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_RequiredNumberOfVotesToDetermine", 50); }
                set { DBSettings.Set("CommunityFaceControlSystem_RequiredNumberOfVotesToDetermine", value); }
            }

            [Reflection.DescriptionAttribute("Required percentage of votes to approve the profile")]
            public static int RequiredPercentageToApproveProfile
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_RequiredPercentageToApproveProfile", 80); }
                set { DBSettings.Set("CommunityFaceControlSystem_RequiredPercentageToApproveProfile", value); }
            }

            [Reflection.DescriptionAttribute("Scores for correct opinion")]
            public static int ScoresForCorrectOpinion
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_ScoresForCorrectOpinion", 1); }
                set { DBSettings.Set("CommunityFaceControlSystem_ScoresForCorrectOpinion", value); }
            }

            [Reflection.DescriptionAttribute("Penalty for incorrect opinion")]
            public static int PenaltyForIncorrectOpinion
            {
                get { return DBSettings.Get("CommunityFaceControlSystem_PenaltyForIncorrectOpinion", 2); }
                set { DBSettings.Set("CommunityFaceControlSystem_PenaltyForIncorrectOpinion", value); }
            }
        }

        #endregion

        #region Nested type: CommunityModeratedSystem

        [Reflection.DescriptionAttribute("Community Moderated System")]
        public class CommunityModeratedSystem
        {
            [Reflection.DescriptionAttribute("Enable community photo approval")]
            public static bool EnableCommunityPhotoApproval
            {
                get { return DBSettings.Get("CommunityModeratedSystem_EnableCommunityPhotoApproval", false); }
                set { DBSettings.Set("CommunityModeratedSystem_EnableCommunityPhotoApproval", value); }
            }

            [Reflection.DescriptionAttribute(
                "Required number of votes to determine whether the photo will be approved or rejected")]
            public static int RequiredNumberOfVotesToDetermine
            {
                get { return DBSettings.Get("CommunityModeratedSystem_RequiredNumberOfVotesToDetermine", 50); }
                set { DBSettings.Set("CommunityModeratedSystem_RequiredNumberOfVotesToDetermine", value); }
            }

            [Reflection.DescriptionAttribute("Required percentage of votes to approve the photo")]
            public static int RequiredPercentageToApprovePhoto
            {
                get { return DBSettings.Get("CommunityModeratedSystem_RequiredPercentageToApprovePhoto", 80); }
                set { DBSettings.Set("CommunityModeratedSystem_RequiredPercentageToApprovePhoto", value); }
            }

            [Reflection.DescriptionAttribute("Required percentage of votes to reject the photo")]
            public static int RequiredPercentageToRejectPhoto
            {
                get { return DBSettings.Get("CommunityModeratedSystem_RequiredPercentageToRejectPhoto", 80); }
                set { DBSettings.Set("CommunityModeratedSystem_RequiredPercentageToRejectPhoto", value); }
            }

            [Reflection.DescriptionAttribute("Scores for correct opinion")]
            public static int ScoresForCorrectOpinion
            {
                get { return DBSettings.Get("CommunityModeratedSystem_ScoresForCorrectOpinion", 1); }
                set { DBSettings.Set("CommunityModeratedSystem_ScoresForCorrectOpinion", value); }
            }

            [Reflection.DescriptionAttribute("Penalty for incorrect opinion")]
            public static int PenaltyForIncorrectOpinion
            {
                get { return DBSettings.Get("CommunityModeratedSystem_PenaltyForIncorrectOpinion", 2); }
                set { DBSettings.Set("CommunityModeratedSystem_PenaltyForIncorrectOpinion", value); }
            }

            [Reflection.DescriptionAttribute("Minimum scores to allow photo moderation")]
            public static int MinimumScoresToAllowModeration
            {
                get { return DBSettings.Get("CommunityModeratedSystem_MinimumScoresToAllowModeration", -20); }
                set { DBSettings.Set("CommunityModeratedSystem_MinimumScoresToAllowModeration", value); }
            }

            [Reflection.DescriptionAttribute("Enable Top Moderators")]
            public static bool EnableTopModerators
            {
                get { return DBSettings.Get("CommunityModeratedSystem_EnableTopModerators", true); }
                set { DBSettings.Set("CommunityModeratedSystem_EnableTopModerators", value); }
            }

            [Reflection.DescriptionAttribute("Top Moderators count")]
            public static int TopModeratorsCount
            {
                get { return DBSettings.Get("CommunityModeratedSystem_TopModeratorsCount", 5); }
                set { DBSettings.Set("CommunityModeratedSystem_TopModeratorsCount", value); }
            }

            [Reflection.DescriptionAttribute("Maximum time away to be listed as top moderator (in days)")]
            [
                Reflection.HintAttribute(
                    "Users who have not logged in more than XX days will not appear in the top moderators page.")]
            public static int TopModeratorsMaxTimeAway
            {
                get { return DBSettings.Get("CommunityModeratedSystem_TopModeratorsMaxTimeAway", 30); }
                set { DBSettings.Set("CommunityModeratedSystem_TopModeratorsMaxTimeAway", value); }
            }

            [Reflection.DescriptionAttribute(
                "Maximum photo abuse reports after which the photo will be manual reviewed by admin")]
            public static int MaxPhotoAbuseReportsForManualApproval
            {
                get { return DBSettings.Get("CommunityModeratedSystem_MaxPhotoAbuseReportsForManualApproval", 5); }
                set { DBSettings.Set("CommunityModeratedSystem_MaxPhotoAbuseReportsForManualApproval", value); }
            }

            [Reflection.DescriptionAttribute(
                "Maximum photo abuse reports after which the photo will be deleted automatically")]
            public static int MaxPhotoAbuseReportsToDeletePhoto
            {
                get { return DBSettings.Get("CommunityModeratedSystem_MaxPhotoAbuseReportsToDeletePhoto", 20); }
                set { DBSettings.Set("CommunityModeratedSystem_MaxPhotoAbuseReportsToDeletePhoto", value); }
            }
        }

        #endregion

        #region Nested type: Credits

        [Reflection.DescriptionAttribute("Credits Settings")]
        public class Credits
        {
            //[Reflection.DescriptionAttribute("Require credits")]
            //[Reflection.HintAttribute("Check to enable credits.")]
            //public static bool Required
            //{
            //    get { return DBSettings.Get("Credits_Required", false); }
            //    set { DBSettings.Set("Credits_Required", value); }
            //}

            [Reflection.DescriptionAttribute("Charge credits one time per member")]
            [Reflection.HintAttribute("Check to enable charging credits one time per member")]
            public static bool ChargeOneTimePerMember
            {
                get { return DBSettings.Get("Credits_ChargeOneTimePerMember", true); }
                set { DBSettings.Set("Credits_ChargeOneTimePerMember", value); }
            }

            //[Reflection.DescriptionAttribute("The price of a message in credits")]
            //[Reflection.HintAttribute("The price of a message in credits")]
            //public static int CreditsPerMessage
            //{
            //    get { return DBSettings.Get("Credits_CreditsPerMessage", 1); }
            //    set { DBSettings.Set("Credits_CreditsPerMessage", value); }
            //}

            //[Reflection.Description("Credits for user photos")]
            //public static int CreditsForUserPhotos
            //{
            //    get { return DBSettings.Get("Credits_CreditsForUserPhotos", 1); }
            //    set { DBSettings.Set("Credits_CreditsForUserPhotos", value); }
            //}

            [Reflection.Description("Photo Unlock period")]
            [Reflection.HintAttribute("The period is in days and may be fractional")]
            public static double PhotoUnlockPeriod
            {
                get { return DBSettings.Get("Credits_PhotoUnlockPeriod", 1.0); }
                set { DBSettings.Set("Credits_PhotoUnlockPeriod", value); }
            }

            //[Reflection.Description("Credits for user video")]
            //public static int CreditsForUserVideo
            //{
            //    get { return DBSettings.Get("Credits_CreditsForUserVideo", 1); }
            //    set { DBSettings.Set("Credits_CreditsForUserVideo", value); }
            //}

            [Reflection.Description("Video Unlock period")]
            [Reflection.HintAttribute("The period is in days and may be fractional")]
            public static double VideoUnlockPeriod
            {
                get { return DBSettings.Get("Credits_VideoUnlockPeriod", 1.0); }
                set { DBSettings.Set("Credits_VideoUnlockPeriod", value); }
            }

            //[Reflection.Description("Credits for IM")]
            //public static int CreditsForIM
            //{
            //    get { return DBSettings.Get("Credits_CreditsForIM", 1); }
            //    set { DBSettings.Set("Credits_CreditsForIM", value); }
            //}

            [Reflection.Description("IM Unlock period")]
            [Reflection.HintAttribute("The period is in days and may be fractional")]
            public static double IMUnlockPeriod
            {
                get { return DBSettings.Get("Credits_IMUnlockPeriod", 1.0); }
                set { DBSettings.Set("Credits_IMUnlockPeriod", value); }
            }

            //[Reflection.DescriptionAttribute("Credits for blog post")]
            //public static int CreditsForBlogPost
            //{
            //    get { return DBSettings.Get("Credits_CreditsForBlogPost", 1); }
            //    set { DBSettings.Set("Credits_CreditsForBlogPost", value); }
            //}

            //[Reflection.DescriptionAttribute("Credits for e-card")]
            //public static int CreditsForEcard
            //{
            //    get { return DBSettings.Get("Credits_CreditsForEcard", 1); }
            //    set { DBSettings.Set("Credits_CreditsForEcard", value); }
            //}

            //[Reflection.Description("Credits for video stream")]
            //public static int CreditsForVideoStream
            //{
            //    get { return DBSettings.Get("Credits_CreditsForVideoStream", 1); }
            //    set { DBSettings.Set("Credits_CreditsForVideoStream", value); }
            //}

            [Reflection.Description("Video stream Unlock period")]
            [Reflection.HintAttribute("The period is in days and may be fractional")]
            public static double VideoStreamUnlockPeriod
            {
                get { return DBSettings.Get("Credits_VideoStreamUnlockPeriod", 1.0); }
                set { DBSettings.Set("Credits_VideoStreamUnlockPeriod", value); }
            }
        }

        #endregion

        #region Nested type: DB

        public class DB
        {
            public const string ISOLATED_FILE_NAME = "Settings.xml";

            private static string connectionString;

            /// <summary>
            /// Gets the connection string.
            /// </summary>
            /// <value>The connection string.</value>
            public static string ConnectionString
            {
                get
                {
                    if (connectionString == null)
                    {
                        if (ConfigurationManager.ConnectionStrings["ezFixUp"] != null)
                        {
                            connectionString = ConfigurationManager.ConnectionStrings["ezFixUp"].ConnectionString;
                        }
                        else
                        {
                            IsolatedStorageFile isoStore =
                                IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                                                             | IsolatedStorageScope.Assembly, null, null);

                            if (!IsolatedFileExists(ISOLATED_FILE_NAME))
                            {
                                throw new NotFoundException("The isolated file " + ISOLATED_FILE_NAME + " was not found");
                            }

                            var iStream =
                                new IsolatedStorageFileStream(ISOLATED_FILE_NAME,
                                                              FileMode.Open, FileAccess.Read, FileShare.Read, isoStore);

                            var doc = new XmlDocument();
                            doc.Load(iStream);
                            XmlElement root = doc.DocumentElement;
                            // ReSharper disable PossibleNullReferenceException
                            XmlNode node = root.SelectSingleNode("//Settings/DB/ConnectionString");
                            // ReSharper restore PossibleNullReferenceException
                            string database, UID, PWD;
                            string server = database = UID = PWD = "";
                            foreach (XmlAttribute att in node.Attributes)
                            {
                                switch (att.Name)
                                {
                                    case "server":
                                        server = att.Value;
                                        break;
                                    case "database":
                                        database = att.Value;
                                        break;
                                    case "UID":
                                        UID = att.Value;
                                        break;
                                    case "PWD":
                                        PWD = att.Value;
                                        break;
                                }
                            }

                            if (server == "" || database == "" || UID == "" || PWD == "")
                            {
                                throw new Exception("Some of the connection string elements are empty or does not exist");
                            }

                            iStream.Close();
                            isoStore.Close();

                            connectionString =
                                String.Format("server={0};database={1};UID={2};PWD={3}", server, database, UID, PWD);
                            //#endif
                        }
                    }
                    return connectionString;
                }
            }

            /// <summary>
            /// Gets or sets the installed version.
            /// </summary>
            /// <value>The installed version.</value>
            public static Version InstalledVersion
            {
                get
                {
                    return SettingsTableExists()
                               ? new Version(DBSettings.Get("DB_InstalledVersion", (new Version(4, 0, 0)).ToString()))
                               : new Version(0, 0, 0);
                }

                set
                {
                    if (SettingsTableExists())
                    {
                        DBSettings.Set("DB_InstalledVersion", value.ToString());
                    }
                    else
                    {
                        throw new Exception("Sql scripts should be installed first!");
                    }
                }
            }

            /// <summary>
            /// Checks if the isolated file exists.
            /// </summary>
            /// <param name="filename">The filename.</param>
            /// <returns></returns>
            public static bool IsolatedFileExists(string filename)
            {
                IsolatedStorageFile isoStore =
                    IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                                                 | IsolatedStorageScope.Assembly, null, null);

                string[] fileNames = isoStore.GetFileNames(filename);

                if (Array.IndexOf(fileNames, filename) == -1)
                {
                    return false;
                }
                return true;
            }

            /// <summary>
            /// Parses the connection string.
            /// </summary>
            /// <param name="connString">The connection string.</param>
            /// <param name="server">The server.</param>
            /// <param name="database">The database.</param>
            /// <param name="UID">The UID.</param>
            /// <param name="PWD">The PWD.</param>
            public static void ParseConnectionString(string connString,
                                                     out string server, out string database, out string UID,
                                                     out string PWD)
            {
                server = database = UID = PWD = "";

                string[] elements = connString.Split(';');
                for (int i = 0; i < elements.Length; ++i)
                {
                    if (elements[i].IndexOf("server=") != -1)
                    {
                        server = elements[i].Replace("server=", "");
                    }
                    else if (elements[i].IndexOf("database=") != -1)
                    {
                        database = elements[i] = elements[i].Replace("database=", "");
                    }
                    else if (elements[i].IndexOf("UID=") != -1)
                    {
                        UID = elements[i] = elements[i].Replace("UID=", "");
                    }
                    else if (elements[i].IndexOf("PWD=") != -1)
                    {
                        PWD = elements[i] = elements[i].Replace("PWD=", "");
                    }
                }
            }

            /// <summary>
            /// Sets the connection string.
            /// </summary>
            /// <param name="server">The server.</param>
            /// <param name="database">The database.</param>
            /// <param name="userID">The user ID.</param>
            /// <param name="password">The password.</param>
            public static void SetConnectionString(string server, string database, string userID, string password)
            {
                if (server == "" || database == "" || userID == "" || password == "")
                {
                    throw new Exception("Some of the connection string arguments are empty");
                }

                IsolatedStorageFile isoStore =
                    IsolatedStorageFile.GetStore(IsolatedStorageScope.User
                                                 | IsolatedStorageScope.Assembly, null, null);

                var doc = new XmlDocument();

                if (!IsolatedFileExists(ISOLATED_FILE_NAME))
                {
                    doc.LoadXml("<Settings></Settings>");
                    XmlElement root = doc.DocumentElement;
                    XmlElement DBElement = doc.CreateElement("DB");
                    XmlElement ConnectionStringElement = doc.CreateElement("ConnectionString");
                    // ReSharper disable PossibleNullReferenceException
                    root.AppendChild(DBElement);
                    // ReSharper restore PossibleNullReferenceException
                    DBElement.AppendChild(ConnectionStringElement);

                    #region Appends all elements to ConnectionStringElement

                    ConnectionStringElement.SetAttribute("server", server);
                    ConnectionStringElement.SetAttribute("database", database);
                    ConnectionStringElement.SetAttribute("UID", userID);
                    ConnectionStringElement.SetAttribute("PWD", password);

                    #endregion
                }
                else
                {
                    var iStream =
                        new IsolatedStorageFileStream(ISOLATED_FILE_NAME, FileMode.Open, isoStore);
                    doc.Load(iStream);
                    XmlNode node = doc.SelectSingleNode("//Settings/DB/ConnectionString");

                    foreach (XmlAttribute att in node.Attributes)
                    {
                        switch (att.Name)
                        {
                            case "server":
                                att.Value = server;
                                break;
                            case "database":
                                att.Value = database;
                                break;
                            case "UID":
                                att.Value = userID;
                                break;
                            case "PWD":
                                att.Value = password;
                                break;
                        }
                    }
                    iStream.Close();
                }
                var encoding = new UTF8Encoding();
                var oStream =
                    new IsolatedStorageFileStream(ISOLATED_FILE_NAME, FileMode.Create, isoStore);
                oStream.Write(encoding.GetBytes(doc.OuterXml), 0, doc.OuterXml.Length);
                oStream.Close();
            }

            /// <summary>
            /// Checks if the settings table exists.
            /// </summary>
            /// <returns></returns>
            public static bool SettingsTableExists()
            {
                using (SqlConnection conn = Open())
                {
                    return (bool)SqlHelper.GetDB().ExecuteScalar( CommandType.Text,
                                                          "IF EXISTS(SELECT * FROM sysobjects WHERE id = object_id(N'[Settings]') AND OBJECTPROPERTY(id, N'IsUserTable') = 1) SELECT CAST (1 AS BIT) ELSE SELECT CAST(0 AS BIT)");
                }
            }

            /// <summary>
            /// Executes the batch.
            /// </summary>
            /// <param name="filename">The filename.</param>
            public static void ExecuteBatch(string filename)
            {
                using (SqlConnection conn = Open())
                {
                    // Load the sql code to reset/build the database
                    StreamReader r = File.OpenText(filename);
                    string sqlBatch = r.ReadToEnd();
                    r.Close();

                    var regex = new Regex("GO\r\n");
                    string[] commands = regex.Split(sqlBatch);

                    foreach (string command in commands)
                    {
                        SqlHelper.GetDB().ExecuteNonQuery( CommandType.Text, command);
                    }
                }
            }

            /// <summary>
            /// Opens this instance.
            /// </summary>
            /// <returns></returns>
            public static SqlConnection Open()
            {
                var conSql = new SqlConnection(ConnectionString);
                conSql.Open();
                return conSql;
            }
        }

        #endregion

        #region Nested type: Directories

        public class Directories
        {
            private static string home;

            public static string Home
            {
                get
                {
                    if (home == null)
                    {
                        home = HostingEnvironment.MapPath("~");
                    }
                    return home;
                }
                set { home = value; }
            }

            public static string UserImagesDirectory
            {
                get
                {
                    if (String.IsNullOrEmpty(Properties.Settings.Default.UserImagesDirectory))
                    {
                        return HostingEnvironment.MapPath("~/UserImages");
                    }
                    return Properties.Settings.Default.UserImagesDirectory.TrimEnd('/');
                }
            }

            public static string ImagesCacheDirectory
            {
                get
                {
                    if (String.IsNullOrEmpty(Properties.Settings.Default.ImagesCacheDirectory))
                    {
                        return HostingEnvironment.MapPath("~/Images/cache");
                    }
                    return Properties.Settings.Default.ImagesCacheDirectory.TrimEnd('/');
                }
            }

            public static string Smilies
            {
                get { return Home + @"\Smilies"; }
            }
        }

        #endregion

        #region Nested type: ErrorLogging

        [Reflection.DescriptionAttribute("Error Logging")]
        public class ErrorLogging
        {
            /// <summary>
            /// Gets or sets a value indicating whether [log errors to file].
            /// </summary>
            /// <value><c>true</c> if [log errors to file]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Log errors to file")]
            public static bool LogErrorsToFile
            {
                get
                {
                    const bool defaultValue = true;

                    try
                    {
                        return DBSettings.Get("ErrorLogging_LogErrorsToFile", defaultValue);
                    }
                    catch (SqlException)
                    {
                        return defaultValue;
                    }
                }
                set { DBSettings.Set("ErrorLogging_LogErrorsToFile", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [send errors to developers].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [send errors to developers]; otherwise, <c>false</c>.
            /// </value>
            [Reflection.DescriptionAttribute("Send errors to the developers")]
            public static bool SendErrorsToDevelopers
            {
                get
                {
                    const bool defaultValue = true;
                    try
                    {
                        return DBSettings.Get("ErrorLogging_SendErrorsToDevelopers", defaultValue);
                    }
                    catch (SqlException)
                    {
                        return defaultValue;
                    }
                }
                set { DBSettings.Set("ErrorLogging_SendErrorsToDevelopers", value); }
            }

            [Reflection.DescriptionAttribute("Save detailed IP log")]
            public static bool SaveIPLog
            {
                get
                {
                    const bool defaultValue = false;
                    try
                    {
                        return DBSettings.Get("ErrorLogging_SaveIPLog", defaultValue);
                    }
                    catch (SqlException)
                    {
                        return defaultValue;
                    }
                }
                set { DBSettings.Set("ErrorLogging_SaveIPLog", value); }
            }
        }

        #endregion

        #region Nested type: Files

        public class Files
        {
            public static string LanguageFile = Directories.Home +
                                                @"\Langs\English.xml";
        }

        #endregion

        #region Nested type: Groups

        [Reflection.DescriptionAttribute("Group settings")]
        public class Groups
        {
            [Reflection.DescriptionAttribute("Enable Groups")]
            [Reflection.HintAttribute("Check to enable groups.")]
            public static bool EnableGroups
            {
                get { return DBSettings.Get("Groups_EnableGroups", false); }
                set { DBSettings.Set("Groups_EnableGroups", value); }
            }

            [Reflection.DescriptionAttribute("Automatically delete a group after XX days of inactivity")]
            [Reflection.HintAttribute("Check to enable automatically delete a group after XX days of inactivity.")]
            public static int AutoDeleteGroups
            {
                get { return DBSettings.Get("Groups_AutoDeleteGroups", 90); }
                set { DBSettings.Set("Groups_AutoDeleteGroups", value); }
            }

            [Reflection.DescriptionAttribute("Enable auto approve groups")]
            [Reflection.HintAttribute("Check to enable auto approve groups.")]
            public static bool AutoApproveGroups
            {
                get { return DBSettings.Get("Groups_AutoApproveGroups", false); }
                set { DBSettings.Set("Groups_AutoApproveGroups", value); }
            }

            [Reflection.DescriptionAttribute("Enable Group Announcement")]
            [Reflection.HintAttribute("Check to enable group announcement.")]
            public static bool EnableGroupAnnouncement
            {
                get { return DBSettings.Get("Groups_EnableGroupAnnouncement", false); }
                set { DBSettings.Set("Groups_EnableGroupAnnouncement", value); }
            }

            [Reflection.DescriptionAttribute("Enable Ajax Chat for Groups")]
            [Reflection.HintAttribute("Check to enable ajax chat for groups.")]
            public static bool EnableAjaxChat
            {
                get { return DBSettings.Get("Groups_EnableAjaxChat", false); }
                set { DBSettings.Set("Groups_EnableAjaxChat", value); }
            }

            [Reflection.DescriptionAttribute("Users must be logged on to browse groups")]
            [Reflection.HintAttribute("Check to restrict browsing groups to members only.")]
            public static bool OnlyRegisteredUsersCanBrowseGroups
            {
                get { return DBSettings.Get("Groups_OnlyRegisteredUsersCanBrowseGroups", false); }
                set { DBSettings.Set("Groups_OnlyRegisteredUsersCanBrowseGroups", value); }
            }

            //[Reflection.DescriptionAttribute("Users must be paid to browse groups")]
            //[Reflection.HintAttribute("Check to restrict browsing groups to paid members only.")]
            //public static bool OnlyPaidUsersCanBrowseGroups
            //{
            //    get { return DBSettings.Get("Groups_OnlyPaidUsersCanBrowseGroups", false); }
            //    set { DBSettings.Set("Groups_OnlyPaidUsersCanBrowseGroups", value); }
            //}

            //[Reflection.DescriptionAttribute("Allow users to create groups")]
            //[Reflection.HintAttribute("Check to allow users to create groups.")]
            //public static bool UsersCanCreateGroups
            //{
            //    get { return DBSettings.Get("Groups_UsersCanCreateGroups", true); }
            //    set { DBSettings.Set("Groups_UsersCanCreateGroups", value); }
            //}

            //[Reflection.DescriptionAttribute("Users must be paid to join groups")]
            //[Reflection.HintAttribute("Check to restrict joining groups to paid members only.")]
            //public static bool OnlyPaidUsersCanJoinGroups
            //{
            //    get { return DBSettings.Get("Groups_OnlyPaidUsersCanJoinGroups", false); }
            //    set { DBSettings.Set("Groups_OnlyPaidUsersCanJoinGroups", value); }
            //}

            [Reflection.DescriptionAttribute("Groups per page")]
            [Reflection.HintAttribute("Specify groups per page.")]
            public static int GroupsPerPage
            {
                get { return DBSettings.Get("Groups_GroupsPerPage", 20); }
                set { DBSettings.Set("Groups_GroupsPerPage", value); }
            }

            [Reflection.DescriptionAttribute("Group photos per page")]
            [Reflection.HintAttribute("Specify group photos per page.")]
            public static int GroupPhotosPerPage
            {
                get { return DBSettings.Get("Groups_GroupPhotosPerPage", 20); }
                set { DBSettings.Set("Groups_GroupPhotosPerPage", value); }
            }

            [Reflection.DescriptionAttribute("Number of new groups")]
            [Reflection.HintAttribute("Specify the number of new groups.")]
            public static int NumberOfNewGroups
            {
                get { return DBSettings.Get("Groups_NumberOfNewGroups", 20); }
                set { DBSettings.Set("Groups_NumberOfNewGroups", value); }
            }

            [Reflection.DescriptionAttribute("Number of new groups rows")]
            [Reflection.HintAttribute("Specify the number of new groups rows.")]
            public static int NumberOfNewGroupsRows
            {
                get { return DBSettings.Get("Groups_NumberOfNewGroupsRows", 2); }
                set { DBSettings.Set("Groups_NumberOfNewGroupsRows", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of topics on group home page")]
            [Reflection.HintAttribute("Specify maximum number of topics to show on group home page.")]
            public static int MaxTopicsOnGroupHomePage
            {
                get { return DBSettings.Get("Groups_MaxTopicsOnGroupHomePage", 10); }
                set { DBSettings.Set("Groups_MaxTopicsOnGroupHomePage", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of group members on group home page")]
            [Reflection.HintAttribute("Specify maximum members to show on group home page.")]
            public static int MaxGroupMembersOnGroupHomePage
            {
                get { return DBSettings.Get("Groups_MaxGroupMembersOnGroupHomePage", 10); }
                set { DBSettings.Set("Groups_MaxGroupMembersOnGroupHomePage", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of group photos on group home page")]
            [Reflection.HintAttribute("Specify maximum group photos to show on group home page.")]
            public static int MaxGroupPhotosOnGroupHomePage
            {
                get { return DBSettings.Get("Groups_MaxGroupPhotosOnGroupHomePage", 10); }
                set { DBSettings.Set("Groups_MaxGroupPhotosOnGroupHomePage", value); }
            }

            [Reflection.DescriptionAttribute("Maximum posts to delete topic")]
            [Reflection.HintAttribute(
                "Specify maximum number of posts after which the topic cannot be deleted by the owner.")]
            public static int MaxPostsToDeleteTopic
            {
                get { return DBSettings.Get("Groups_MaxPostsToDeleteTopic", 10); }
                set { DBSettings.Set("Groups_MaxPostsToDeleteTopic", value); }
            }

            //[Reflection.DescriptionAttribute("Maximum groups per member")]
            //[Reflection.HintAttribute("Specify maximum groups in which the user can be member.")]
            //public static int MaxGroupsPerMember
            //{
            //    get { return DBSettings.Get("Groups_MaxGroupsPerMember", 30); }
            //    set { DBSettings.Set("Groups_MaxGroupsPerMember", value); }
            //}

            [Reflection.DescriptionAttribute("Maximum invitations")]
            [Reflection.HintAttribute("Specify maximum number of invitations which can be sent for a private group.")]
            public static int MaxInvitations
            {
                get { return DBSettings.Get("Groups_MaxInvitations", 10); }
                set { DBSettings.Set("Groups_MaxInvitations", value); }
            }

            [Reflection.DescriptionAttribute("Maximum topics per group")]
            [Reflection.HintAttribute("Specify maximum number of topics per group for 24 hours for one member.")]
            public static int MaxTopicsPerGroupForDay
            {
                get { return DBSettings.Get("Groups_MaxTopicsPerGroupForDay", 5); }
                set { DBSettings.Set("Groups_MaxTopicsPerGroupForDay", value); }
            }

            [Reflection.DescriptionAttribute("Maximum topics for all groups")]
            [Reflection.HintAttribute("Specify maximum number of topics for all groups for 24 hours for one member.")]
            public static int MaxTopicsForGroupsForDay
            {
                get { return DBSettings.Get("Groups_MaxTopicsForGroupsForDay", 10); }
                set { DBSettings.Set("Groups_MaxTopicsForGroupsForDay", value); }
            }

            [Reflection.DescriptionAttribute("Maximum icon width")]
            [Reflection.HintAttribute("Specify the maximum width of an icon.")]
            public static int IconMaxWidth
            {
                get { return DBSettings.Get("Groups_IconMaxWidth", 450); }
                set { DBSettings.Set("Groups_IconMaxWidth", value); }
            }

            [Reflection.DescriptionAttribute("Maximum icon height")]
            [Reflection.HintAttribute("Specify the maximum height of an icon.")]
            public static int IconMaxHeight
            {
                get { return DBSettings.Get("Groups_IconMaxHeight", 450); }
                set { DBSettings.Set("Groups_IconMaxHeight", value); }
            }

            [Reflection.DescriptionAttribute("Maximum group photo width")]
            [Reflection.HintAttribute("Specify the maximum width of group photo.")]
            public static int GroupPhotoMaxWidth
            {
                get { return DBSettings.Get("Groups_GroupPhotoMaxWidth", 450); }
                set { DBSettings.Set("Groups_GroupPhotoMaxWidth", value); }
            }

            [Reflection.DescriptionAttribute("Maximum group photo height")]
            [Reflection.HintAttribute("Specify the maximum height of group photo.")]
            public static int GroupPhotoMaxHeight
            {
                get { return DBSettings.Get("Groups_GroupPhotoMaxHeight", 450); }
                set { DBSettings.Set("Groups_GroupPhotoMaxHeight", value); }
            }

            [Reflection.DescriptionAttribute("Maximum group event image width")]
            [Reflection.HintAttribute("Specify the maximum width of group event image.")]
            public static int GroupEventImageMaxWidth
            {
                get { return DBSettings.Get("Groups_GroupEventImageMaxWidth", 450); }
                set { DBSettings.Set("Groups_GroupEventImageMaxWidth", value); }
            }

            [Reflection.DescriptionAttribute("Maximum group event image height")]
            [Reflection.HintAttribute("Specify the maximum height of group event image.")]
            public static int GroupEventImageMaxHeight
            {
                get { return DBSettings.Get("Groups_GroupEventImageMaxHeight", 450); }
                set { DBSettings.Set("Groups_GroupEventImageMaxHeight", value); }
            }

            [Reflection.DescriptionAttribute("Maximum members to delete group")]
            [Reflection.HintAttribute("Specify maximum members after which the group cannot be deleted.")]
            public static int MaxGroupMembersToDeleteGroup
            {
                get { return DBSettings.Get("Groups_MaxGroupMembersToDeleteGroup", 30); }
                set { DBSettings.Set("Groups_MaxGroupMembersToDeleteGroup", value); }
            }

            [Reflection.DescriptionAttribute("Group topics per page")]
            [Reflection.HintAttribute("Specify group topics per page.")]
            public static int GroupTopicsPerPage
            {
                get { return DBSettings.Get("Groups_GroupTopicsPerPage", 20); }
                set { DBSettings.Set("Groups_GroupTopicsPerPage", value); }
            }

            [Reflection.DescriptionAttribute("Group posts per page")]
            [Reflection.HintAttribute("Specify group posts per page.")]
            public static int GroupPostsPerPage
            {
                get { return DBSettings.Get("Groups_GroupPostsPerPage", 20); }
                set { DBSettings.Set("Groups_GroupPostsPerPage", value); }
            }

            [Reflection.DescriptionAttribute("Enable moderated group invitation")]
            [Reflection.HintAttribute("Check to enable invitation for moderated group.")]
            public static bool EnableModeratedGroupInvitation
            {
                get { return DBSettings.Get("Groups_EnableModeratedGroupInvitation", false); }
                set { DBSettings.Set("Groups_EnableModeratedGroupInvitation", value); }
            }

            [Reflection.DescriptionAttribute("Enable public group invitation")]
            [Reflection.HintAttribute("Check to enable invitation for public group.")]
            public static bool EnablePublicGroupInvitation
            {
                get { return DBSettings.Get("Groups_EnablePublicGroupInvitation", false); }
                set { DBSettings.Set("Groups_EnablePublicGroupInvitation", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of group events on group home page")]
            [Reflection.HintAttribute("Specify maximum number of group events to show on group home page.")]
            public static int MaxGroupEventsOnGroupHomePage
            {
                get { return DBSettings.Get("Groups_MaxGroupEventsOnGroupHomePage", 5); }
                set { DBSettings.Set("Groups_MaxGroupEventsOnGroupHomePage", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of group events on home page")]
            [Reflection.HintAttribute("Specify maximum number of group events to show on home page.")]
            public static int MaxGroupEventsOnHomePage
            {
                get { return DBSettings.Get("Groups_MaxGroupEventsOnHomePage", 5); }
                set { DBSettings.Set("Groups_MaxGroupEventsOnHomePage", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of poll choices in group polls")]
            [Reflection.HintAttribute("Specify maximum number of poll choices in group polls.")]
            public static int NumberOfGroupPollsChoices
            {
                get { return DBSettings.Get("Groups_NumberOfGroupPollsChoices", 10); }
                set { DBSettings.Set("Groups_NumberOfGroupPollsChoices", value); }
            }
        }

        #endregion

        #region Nested type: Mailing

        [Reflection.DescriptionAttribute("Mailing settings")]
        public class Mailing
        {
            [Reflection.DescriptionAttribute("Retry count")]
            public static int RetryCount
            {
                get { return DBSettings.Get("Mailing_RetryCount", 24); }
                set { DBSettings.Set("Mailing_RetryCount", value); }
            }

            [Reflection.DescriptionAttribute("Retry interval(in hours)")]
            public static int RetryInterval
            {
                get { return DBSettings.Get("Mailing_RetryInterval", 1); }
                set { DBSettings.Set("Mailing_RetryInterval", value); }
            }

            [Reflection.DescriptionAttribute("Enable address book importer")]
            public static bool EnableAddressBookImporter
            {
                get { return DBSettings.Get("Mailing_EnableAddressBookImporter", true); }
                set { DBSettings.Set("Mailing_EnableAddressBookImporter", value); }
            }
        }

        #endregion

        #region Nested type: Maintenance

        [Reflection.DescriptionAttribute("Maintenance")]
        public class Maintenance
        {
            [Reflection.DescriptionAttribute("Not activated user deletion time interval (in days)")]
            [Reflection.HintAttribute(
                "Interval (in days) after which the system will delete accounts that have not completed the e-mail confirmation."
                )]
            public static int NotActivatedUsersPurgePeriod
            {
                get { return DBSettings.Get("Maintenance_NotActivatedUsersPurgePeriod", 7); }
                set { DBSettings.Set("Maintenance_NotActivatedUsersPurgePeriod", value); }
            }
        }

        #endregion

        #region Nested type: Misc

        [Reflection.DescriptionAttribute("Miscellaneous settings")]
        public class Misc
        {
            public static int PopupShowTimeMiliSeconds
            {
                get { return DBSettings.Get("Misc_PopupShowTimeMiliSeconds", 15000); }
                set { DBSettings.Set("Misc_PopupShowTimeMiliSeconds", value); }
            }

            public static bool EnableFirstRunWizard
            {
                get { return DBSettings.Get("Misc_EnableFirstRunWizard", false); }
                set { DBSettings.Set("Misc_EnableFirstRunWizard", value); }
            }

            public static string GoogleAnalyticsTrackingCode
            {
                get
                {
                    return DBSettings.Get("Misc_GoogleAnalyticsTrackingCode", "");
                }
                set { DBSettings.Set("Misc_GoogleAnalyticsTrackingCode", value); }
            }

            [Reflection.DescriptionAttribute("Enable Spam Detection")]
            public static bool EnableSpamDetection
            {
                get { return DBSettings.Get("Misc_EnableSpamDetection", false); }
                set { DBSettings.Set("Misc_EnableSpamDetection", value); }
            }

            [Reflection.DescriptionAttribute("Enable Profile Topics&Questions translation")]
            public static bool EnableProfileDataTranslation
            {
                get { return DBSettings.Get("Misc_EnableProfileDataTranslation", false); }
                set { DBSettings.Set("Misc_EnableProfileDataTranslation", value); }
            }

            [Reflection.DescriptionAttribute("Enable Instant Messenger")
            ]
            public static bool EnableIntegratedIM
            {
                get
                {
                    return false; DBSettings.Get("Misc_EnableIntegratedIM", false);
                }
                set { DBSettings.Set("Misc_EnableIntegratedIM", value); }
            }

            //We will no longer support video profiles
            //[Reflection.DescriptionAttribute("Enable Video Profile Integration (valid subscription required)")]
            public static bool EnableVideoProfile
            {
                get { return DBSettings.Get("Misc_EnableVideoProfile", false); }
                set { DBSettings.Set("Misc_EnableVideoProfile", value); }
            }

            [Reflection.DescriptionAttribute("Enable Ajax Chat")]
            public static bool EnableAjaxChat
            {
                get
                {
                    return false; DBSettings.Get("Misc_EnableAjaxChat", false);
                }
                set { DBSettings.Set("Misc_EnableAjaxChat", value); }
            }

            [Reflection.DescriptionAttribute("Enable Mobile version")]
            public static bool EnableMobileVersion
            {
                get { return DBSettings.Get("Misc_EnableMobileVersion", false); }
                set { DBSettings.Set("Misc_EnableMobileVersion", value); }
            }

            [Reflection.DescriptionAttribute("Enable Blogs")]
            public static bool EnableBlogs
            {
                get { return DBSettings.Get("Misc_EnableBlogs", false); }
                set { DBSettings.Set("Misc_EnableBlogs", value); }
            }

            [Reflection.DescriptionAttribute("Enable blog post approval")]
            public static bool EnableBlogPostApproval
            {
                get { return DBSettings.Get("Misc_EnableBlogPostApproval", false); }
                set { DBSettings.Set("Misc_EnableBlogPostApproval", value); }
            }

            //[Reflection.DescriptionAttribute("Elapsed days of blog creation")]
            public static int ElapsedDaysOfBlogCreation
            {
                get { return DBSettings.Get("Misc_ElapsedDaysOfBlogCreation", 30); }
                set { DBSettings.Set("Misc_ElapsedDaysOfBlogCreation", value); }
            }

            [Reflection.DescriptionAttribute("Number of new blogs")]
            [Reflection.HintAttribute("Specify the number of new blogs.")]
            public static int NumberOfNewBlogs
            {
                get { return DBSettings.Get("Misc_NumberOfNewBlogs", 5); }
                set { DBSettings.Set("Misc_NumberOfNewBlogs", value); }
            }

            [Reflection.DescriptionAttribute("Enable Video Upload  (Video Converter plugin required)")]
            public static bool EnableVideoUpload
            {
                get { return DBSettings.Get("Misc_EnableVideoUpload", false); }
                set { DBSettings.Set("Misc_EnableVideoUpload", value); }
            }

            //[Reflection.DescriptionAttribute("Maximum Video Uploads")]
            //public static int MaxVideoUploads
            //{
            //    get { return DBSettings.Get("Misc_MaxVideoUploads", 3); }
            //    set { DBSettings.Set("Misc_MaxVideoUploads", value); }
            //}

            [Reflection.DescriptionAttribute("Enable YouTube videos")]
            public static bool EnableYouTubeVideos
            {
                get { return DBSettings.Get("Misc_EnableYouTubeVideos", false); }
                set { DBSettings.Set("Misc_EnableYouTubeVideos", value); }
            }

            //[Reflection.DescriptionAttribute("Max YouTube videos")]
            //public static int MaxYouTubeVideos
            //{
            //    get { return DBSettings.Get("Misc_MaxYouTubeVideos", 6); }
            //    set { DBSettings.Set("Misc_MaxYouTubeVideos", value); }
            //}

            [Reflection.DescriptionAttribute("Enable Audio Upload")]
            public static bool EnableAudioUpload
            {
                get { return DBSettings.Get("Misc_EnableAudioUpload", false); }
                set { DBSettings.Set("Misc_EnableAudioUpload", value); }
            }

            //[Reflection.DescriptionAttribute("Maximum Audio uploads")]
            //public static int MaxAudioUploads
            //{
            //    get { return DBSettings.Get("Misc_MaxAudioUploads", 3); }
            //    set { DBSettings.Set("Misc_MaxAudioUploads", value); }
            //}

            [Reflection.DescriptionAttribute("Enable Video Broadcast on Profile")]
            public static bool EnableProfileVideoBroadcast
            {
                get { return DBSettings.Get("Misc_EnableProfileVideoBroadcast", false); }
                set { DBSettings.Set("Misc_EnableProfileVideoBroadcast", value); }
            }

            [Reflection.DescriptionAttribute("Flash Server for Video Broadcast")]
            public static string FlashServerForVideoBroadcast
            {
                get { return DBSettings.Get("Misc_FlashServerForVideoBroadcast", "rtmp://127.0.0.1/ezFixUp"); }
                set { DBSettings.Set("Misc_FlashServerForVideoBroadcast", value); }
            }

            [Reflection.DescriptionAttribute("Enable bad words filter for blogs")]
            public static bool EnableBadWordsFilterBlogs
            {
                get { return DBSettings.Get("Misc_EnableBadWordsFilterBlogs", true); }
                set { DBSettings.Set("Misc_EnableBadWordsFilterBlogs", value); }
            }

            [Reflection.DescriptionAttribute("Enable bad words filter for groups")]
            public static bool EnableBadWordsFilterGroups
            {
                get { return DBSettings.Get("Misc_EnableBadWordsFilterGroups", true); }
                set { DBSettings.Set("Misc_EnableBadWordsFilterGroups", value); }
            }

            [Reflection.DescriptionAttribute("Enable bad words filter for comments")]
            public static bool EnableBadWordsFilterComments
            {
                get { return DBSettings.Get("Misc_EnableBadWordsFilterComments", true); }
                set { DBSettings.Set("Misc_EnableBadWordsFilterComments", value); }
            }

            [Reflection.DescriptionAttribute("Enable bad words filter for messages")]
            public static bool EnableBadWordsFilterMessage
            {
                get { return DBSettings.Get("Misc_EnableBadWordsFilterMessage", true); }
                set { DBSettings.Set("Misc_EnableBadWordsFilterMessage", value); }
            }

            [Reflection.DescriptionAttribute("Enable bad words filter for profile")]
            public static bool EnableBadWordsFilterProfile
            {
                get { return DBSettings.Get("Misc_EnableBadWordsFilterProfile", true); }
                set { DBSettings.Set("Misc_EnableBadWordsFilterProfile", value); }
            }

            [Reflection.DescriptionAttribute("Enable Captcha")]
            public static bool EnableCaptcha
            {
                get { return DBSettings.Get("Misc_EnableCaptcha", false); }
                set { DBSettings.Set("Misc_EnableCaptcha", value); }
            }

            [Reflection.DescriptionAttribute("Enable Vista Gadgets")]
            public static bool EnableGadgets
            {
                get { return DBSettings.Get("Misc_EnableGadgets", false); }
                set { DBSettings.Set("Misc_EnableGadgets", value); }
            }

            [Reflection.DescriptionAttribute("Gadgets prefix")]
            public static string GadgetsPrefix
            {
                get { return DBSettings.Get("Misc_GadgetsPrefix", "ezFixUp Admin Stats"); }
                set { DBSettings.Set("Misc_GadgetsPrefix", value); }
            }

            [Reflection.DescriptionAttribute("Enable CoolIris (formerly PicLens) feeds")]
            public static bool EnableCoolIris
            {
                get { return DBSettings.Get("Misc_EnableCoolIris", true); }
                set { DBSettings.Set("Misc_EnableCoolIris", value); }
            }

            [Reflection.DescriptionAttribute("Enable MySpace Integration")]
            public static bool EnableMySpaceIntegration
            {
                get { return DBSettings.Get("Misc_EnableMySpaceIntegration", false); }
                set { DBSettings.Set("Misc_EnableMySpaceIntegration", value); }
            }

            [Reflection.DescriptionAttribute("Enable Facebook Integration")]
            public static bool EnableFacebookIntegration
            {
                get { return DBSettings.Get("Misc_EnableFacebookIntegration", true); }
                set { DBSettings.Set("Misc_EnableFacebookIntegration", value); }
            }

            [Reflection.DescriptionAttribute("Enable Twitter Integration")]
            public static bool EnableTwitterIntegration
            {
                get { return DBSettings.Get("Misc_EnableTwitterIntegration", false); }
                set { DBSettings.Set("Misc_EnableTwitterIntegration", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether in user messages phones and emails to
            /// be filtered.
            /// </summary>
            /// <value><c>true</c> if [enable message filter]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Enable e-mail and phone filtering in messages")]
            [Reflection.HintAttribute("Check to enable user messages filter.")]
            public static bool EnableMessageFilter
            {
                get { return DBSettings.Get("Misc_EnableMessageFilter", false); }
                set { DBSettings.Set("Misc_EnableMessageFilter", value); }
            }

            [Reflection.DescriptionAttribute("Maximum number of same messages")]
            public static int MaxSameMessages
            {
                get { return DBSettings.Get("Misc_MaxSpamMessages", 10); }
                set { DBSettings.Set("Misc_MaxSpamMessages", value); }
            }

            /// <summary>
            /// Gets or sets the time offset int.
            /// </summary>
            /// <value>The time offset int.</value>
            [Reflection.DescriptionAttribute("Time offset")]
            public static int TimeOffsetInt
            {
                get { return DBSettings.Get("Misc_TimeOffsetInt", 0); }
                set { DBSettings.Set("Misc_TimeOffsetInt", value); }
            }

            /// <summary>
            /// Gets the time offset.
            /// </summary>
            /// <value>The time offset.</value>
            public static TimeSpan TimeOffset
            {
                get { return TimeSpan.FromHours(TimeOffsetInt); }
            }

            /// <summary>
            /// Gets or sets the site title.
            /// </summary>
            /// <value>The site title</value>
            [Reflection.DescriptionAttribute("Site title")]
            public static string SiteTitle
            {
                get { return DBSettings.Get("Misc_SiteTitle", "Your Site Name"); }
                set { DBSettings.Set("Misc_SiteTitle", value); }
            }

            /// <summary>
            /// Gets or sets the site email.
            /// </summary>
            /// <value>The site email.</value>
            [Reflection.DescriptionAttribute("Site email")]
            public static string SiteEmail
            {
                get { return DBSettings.Get("Misc_SiteEmail", "noreply@lovehitch.com"); }
                set { DBSettings.Set("Misc_SiteEmail", value); }
            }

            /// <summary>
            /// Gets or sets the site theme.
            /// </summary>
            /// <value>The site theme.</value>
            public static string SiteTheme
            {
                get { return DBSettings.Get("Misc_SiteTheme", String.Empty); }
                set { DBSettings.Set("Misc_SiteTheme", value); }
            }

            /// <summary>
            /// Gets or sets the number of news.
            /// </summary>
            [Reflection.DescriptionAttribute("Number of news")]
            public static int NumberOfNews
            {
                get { return DBSettings.Get("Misc_NumberOfNews", 5); }
                set { DBSettings.Set("Misc_NumberOfNews", value); }
            }

            public static int DefaultLanguageId
            {
                get { return DBSettings.Get("Misc_DefaultLanguageId", 1); }
                set { DBSettings.Set("Misc_DefaultLanguageId", value); }
            }

            public static string BadWords
            {
                get
                {
                    return DBSettings.Get("Misc_BadWords",
                                          "damn\ndyke\nfuck\nshit\n@$$\namcik\nandskota\narschloch\narse\nasshole\nassrammer\nayir\nb!+ch\nb!tch\nb17ch\nb1tch\nbastard\nbi+ch\nbi7ch\nbitch\nboiolas\nbollock\nbreasts\nbuceta\nbutt-pirate\nc0ck\ncabron\ncawk\ncazzo\nchink\nchraa\nchuj\ncipa\nclits\nCock\ncum\ncunt\nd4mn\ndaygo\ndego\ndick\ndike\ndildo\ndirsa\ndupa\ndziwka\nejackulate\nEkrem\nEkto\nenculer\nfaen\nfag\nfanculo\nfanny\nfatass\nfcuk\nfeces\nfeg\nFelcher\nficken\nfitt\nFlikker\nforeskin\nFotze\nFu(\nfuk\nfutkretzn\nfux0r\ngay\ngook\nguiena\nh0r\nh4x0r\nhell\nhelvete\nhoer\nhonkey\nhore\nHuevon\nhui\ninjun\njism\njizz\nkanker\nkawk\nkike\nklootzak\nkraut\nknulle\nkuk\nkuksuger\nKurac\nkurwa\nkusi\nkyrpa\nl3i+ch\nl3itch\nlesbian\nlesbo\nmamhoon\nmasturbat\nmerd\nmibun\nmonkleigh\nmotherfucker\nmofo\nmouliewop\nmuie\nmulkku\nmuschi\nnazis\nnepesaurio\nnigga\nnigger\nnutsack\norospu\npaska\nperse\nphuck\npicka\npierdol\npillu\npimmel\npimpis\npiss\npizda\npoontsee\npoop\nporn\np0rn\npr0n\npreteen\npula\npule\npusse\npussy\nputa\nputo\nqahbeh\nqueef\nrautenberg\nschaffer\nscheiss\nschlampe\nschmuck\nscrew\nscrotum\nsh!t\nsharmuta\nsharmute\nshemale\nshipal\nshiz\nskribz\nskurwysyn\nslut\nsmut\nsphencter\nspic\nspierdalaj\nsplooge\nsuka\nteets\nb00b\nteez\ntesticle\ntitt\ntits\ntwat\nvittu\nw00se\nwank\nwetback\nwhoar\nwichser\nwop\nyed\nzabourah");
                }

                set { DBSettings.Set("Misc_BadWords", value); }
            }

            public static bool EnableBadWordsRegularExpressions
            {
                get { return DBSettings.Get("Misc_EnableBadWordsRegularExpressions", false); }
                set { DBSettings.Set("Misc_EnableBadWordsRegularExpressions", value); }
            }

            [Reflection.DescriptionAttribute("Bad words replacement")]
            public static string BadWordsReplacement
            {
                get { return DBSettings.Get("Misc_BadWordsReplacement", "*****"); }
                set { DBSettings.Set("Misc_BadWordsReplacement", value); }
            }

            [Reflection.DescriptionAttribute("Days of inactivity to send reminder e-mail")]
            public static int NotVisitedSiteDays
            {
                get { return DBSettings.Get("Misc_NotVisitedSiteDays", 30); }
                set { DBSettings.Set("Misc_NotVisitedSiteDays", value); }
            }

            [Reflection.DescriptionAttribute("Stop users with ad blockers")]
            public static bool StopUsersWithAdBlocker
            {
                get { return DBSettings.Get("Misc_StopUsersWithAdBlocker", false); }
                set { DBSettings.Set("Misc_StopUsersWithAdBlocker", value); }
            }

            [Reflection.DescriptionAttribute("Lock inner home page layout")]
            public static bool LockHomePageLayout
            {
                get { return DBSettings.Get("Misc_LockHomePageLayout", false); }
                set { DBSettings.Set("Misc_LockHomePageLayout", value); }
            }

            [Reflection.DescriptionAttribute("Show polls on inner home page")]
            public static bool ShowPolls
            {
                get { return DBSettings.Get("Misc_ShowPolls", true); }
                set { DBSettings.Set("Misc_ShowPolls", value); }
            }

            //[Reflection.DescriptionAttribute("Do not show banners to paid members")]
            //public static bool DoNotShowBannersToPaidMembers
            //{
            //    get { return DBSettings.Get("Misc_DoNotShowBannersToPaidMembers", false); }
            //    set { DBSettings.Set("Misc_DoNotShowBannersToPaidMembers", value); }
            //}

            [Reflection.DescriptionAttribute("Use CKEditor")]
            public static bool UseCKEditor
            {
                get { return DBSettings.Get("Misc_UseCKEditor", false); }
                set { DBSettings.Set("Misc_UseCKEditor", value); }
            }

            public static bool SiteIsPaid
            {
                get
                {
                    var plans = BillingPlan.Fetch();
                    if (plans.Length > 0)
                        return true;

                    return Config.Users.GetNonPayingMembersOptions().ContainsOptionWithEnabledCredits;
                }
            }
        }

        #endregion

        #region Nested type: Photos

        [Reflection.DescriptionAttribute("Photo Settings")]
        public class Photos
        {
            /// <summary>
            /// Gets or sets a value indicating whether [auto approve photos].
            /// </summary>
            /// <value><c>true</c> if [auto approve photos]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Enable auto approve photos")]
            [Reflection.HintAttribute("Check to enable auto approve photos.")]
            public static bool AutoApprovePhotos
            {
                get { return DBSettings.Get("Photos_AutoApprovePhotos", false); }
                set { DBSettings.Set("Photos_AutoApprovePhotos", value); }
            }

            /// <summary>
            /// Gets or sets the max photos on main page.
            /// </summary>
            /// <value>The max photos on main page.</value>
            [Reflection.DescriptionAttribute("Maximum photos on main page (in rows)")]
            [
                Reflection.HintAttribute(
                    "Specify the number of rows of photos that appear on New Members section of the main page.")]
            public static int MaxRowsPhotosOnMainPage
            {
                get { return DBSettings.Get("Photos_MaxRowsPhotosOnMainPage", 2); }
                set { DBSettings.Set("Photos_MaxRowsPhotosOnMainPage", value); }
            }

            /// <summary>
            /// Gets or sets the max photos on matchmaker page.
            /// </summary>
            /// <value>The max photos on matchmaker page.</value>
            [Reflection.DescriptionAttribute("Maximum photos on matchmaker page (in rows)")]
            [
                Reflection.HintAttribute(
                    "Specify the number of rows of photos that appear on friends section of the MatchMaker page.")]
            public static int MaxRowsPhotosOnMatchMakerPage
            {
                get { return DBSettings.Get("Photos_MaxRowsPhotosOnMatchMakerPage", 2); }
                set { DBSettings.Set("Photos_MaxRowsPhotosOnMatchMakerPage", value); }
            }

            /// <summary>
            /// Gets or sets the max videos on main page.
            /// </summary>
            /// <value>The max videos on main page.</value>
            [Reflection.DescriptionAttribute("Maximum videos on main page (in rows)")]
            [
                Reflection.HintAttribute(
                    "Specify the number of rows of videos that appear on New Members section of the main page.")]
            public static int MaxRowsVideosOnMainPage
            {
                get { return DBSettings.Get("Videos_MaxRowsVideosOnMainPage", 2); }
                set { DBSettings.Set("Videos_MaxRowsVideosOnMainPage", value); }
            }

            /// <summary>
            /// Gets or sets the max photos.
            /// </summary>
            /// <value>The max photos.</value>
            [Reflection.DescriptionAttribute("Maximum photos per user")]
            [Reflection.HintAttribute("Specify the maximum number of photos per user.")]
            public static int MaxPhotos
            {
                get { return DBSettings.Get("Photos_MaxPhotos", 10); }
                set { DBSettings.Set("Photos_MaxPhotos", value); }
            }

            /// <summary>
            /// Gets or sets the width of the photo max.
            /// </summary>
            /// <value>The width of the photo max.</value>
            [Reflection.DescriptionAttribute("Maximum photo width")]
            [Reflection.HintAttribute("Specify the maximum width of a photo.")]
            public static int PhotoMaxWidth
            {
                get { return DBSettings.Get("Photos_PhotoMaxWidth", 450); }
                set { DBSettings.Set("Photos_PhotoMaxWidth", value); }
            }

            /// <summary>
            /// Gets or sets the height of the photo max.
            /// </summary>
            /// <value>The height of the photo max.</value>
            [Reflection.DescriptionAttribute("Maximum photo height")]
            [Reflection.HintAttribute("Specify the maximum height of a photo.")]
            public static int PhotoMaxHeight
            {
                get { return DBSettings.Get("Photos_PhotoMaxHeight", 450); }
                set { DBSettings.Set("Photos_PhotoMaxHeight", value); }
            }

            [Reflection.DescriptionAttribute("Minimum photo width")]
            [Reflection.HintAttribute("Specify the minimum width of a photo.")]
            public static int PhotoMinWidth
            {
                get { return DBSettings.Get("Photos_PhotoMinWidth", 100); }
                set { DBSettings.Set("Photos_PhotoMinWidth", value); }
            }

            [Reflection.DescriptionAttribute("Minimum photo height")]
            [Reflection.HintAttribute("Specify the minimum height of a photo.")]
            public static int PhotoMinHeight
            {
                get { return DBSettings.Get("Photos_PhotoMinHeight", 100); }
                set { DBSettings.Set("Photos_PhotoMinHeight", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [do watermark].
            /// </summary>
            /// <value><c>true</c> if [do watermark]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Apply watermark on photos")]
            [Reflection.HintAttribute("Check to enable photo watermarking.")]
            public static bool DoWatermark
            {
                get { return DBSettings.Get("Photos_DoWatermark", true); }
                set { DBSettings.Set("Photos_DoWatermark", value); }
            }

            /// <summary>
            /// Gets or sets the watermark transparency.
            /// </summary>
            /// <value>The watermark transparency.</value>
            [Reflection.DescriptionAttribute("Watermark transparency level")]
            [Reflection.HintAttribute("Watermark transparency level (0-250). Recommended value is 150.")]
            public static int WatermarkTransparency
            {
                get { return DBSettings.Get("Photos_WatermarkTransparency", 150); }
                set { DBSettings.Set("Photos_WatermarkTransparency", value); }
            }

            /// <summary>
            /// Gets or sets the watermark position.
            /// </summary>
            /// <value>The watermark position.</value>
            [Reflection.DescriptionAttribute("Watermark position")]
            [Reflection.HintAttribute("Watermark position.")]
            public static Photo.eWatermarkPosition WatermarkPosition
            {
                get
                {
                    string enumElement = DBSettings.Get("Photos_WatermarkPosition",
                                                        Photo.eWatermarkPosition.BottomRight.ToString());
                    return (Photo.eWatermarkPosition)
                           Reflection.StringToEnum(typeof(Photo.eWatermarkPosition), enumElement);
                }
                set { DBSettings.Set("Photos_WatermarkPosition", value.ToString()); }
                //= Photo.eWatermarkPosition.BottomRight;
            }

            /// <summary>
            /// Gets or sets the min width to watermark.
            /// </summary>
            /// <value>The min width to watermark.</value>
            [Reflection.DescriptionAttribute("Minimum picture width to apply watermark")]
            [
                Reflection.HintAttribute(
                    "Minimum picture width to apply watermark. It is recommeded to set this value bigger than the thumbnail size."
                    )]
            public static int MinWidthToWatermark
            {
                get { return DBSettings.Get("Photos_MinWidthToWatermark", 101); }
                set { DBSettings.Set("Photos_MinWidthToWatermark", value); }
            }

            /// <summary>
            /// Gets or sets the min height to watermark.
            /// </summary>
            /// <value>The min height to watermark.</value>
            [Reflection.DescriptionAttribute("Minimum picture height to apply watermark")]
            [
                Reflection.HintAttribute(
                    "Minimum picture height to apply watermark. It is recommeded to set this value bigger than the thumbnail size."
                    )]
            public static int MinHeightToWatermark
            {
                get { return DBSettings.Get("Photos_MinHeightToWatermark", 101); }
                set { DBSettings.Set("Photos_MinHeightToWatermark", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [admin can mark photos as explicit].
            /// </summary>
            /// <value><c>true</c> if [admin can mark photos as explicit]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Enable explicit photos")]
            [
                Reflection.HintAttribute(
                    "Checking will allow administrators to tag expicit photos. Those photos will only be displayed to registered users. Visitors will see censored image."
                    )]
            public static bool EnableExplicitPhotos
            {
                get { return DBSettings.Get("Photos_EnableExplicitPhotos", false); }
                set { DBSettings.Set("Photos_EnableExplicitPhotos", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [user can mark photos as private].
            /// </summary>
            /// <value><c>true</c> if [user can mark photos as private]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Enable private photos")]
            [
                Reflection.HintAttribute(
                    "Checking will allow members to upload private photos. Those photos will be visible only to members chosen by the person who uploaded the photos."
                    )]
            public static bool EnablePrivatePhotos
            {
                get { return DBSettings.Get("Photos_EnablePrivatePhotos", false); }
                set { DBSettings.Set("Photos_EnablePrivatePhotos", value); }
            }

            [Reflection.DescriptionAttribute("Make explicit photos private")]
            [
                Reflection.HintAttribute(
                    "Checking will mark all explicit photos as private."
                    )]
            public static bool MakeExplicitPhotosPrivate
            {
                get { return DBSettings.Get("Photos_MakeExplicitPhotosPrivate", false); }
                set { DBSettings.Set("Photos_MakeExplicitPhotosPrivate", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [user photo to be shown for each message in the mailbox].
            /// </summary>
            /// <value><c>true</c> if [ser photo to be shown for each message in the mailbox]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Show Thumbnails in Mailbox")]
            [Reflection.HintAttribute("Check to show thumbnails next to each mail in the mailbox.")]
            public static bool ShowThumbnailsInMailbox
            {
                get { return DBSettings.Get("Photos_ShowThumbnailsInMailbox", false); }
                set { DBSettings.Set("Photos_ShowThumbnailsInMailbox", value); }
            }

            [Reflection.DescriptionAttribute("Find faces in photos (FaceFinder plugin required)")]
            public static bool FindFacesForThumbnails
            {
                get { return DBSettings.Get("Photos_FindFacesForThumbnails", false); }
                set { DBSettings.Set("Photos_FindFacesForThumbnails", value); }
            }

            [Reflection.DescriptionAttribute("Photo comments")]
            [Reflection.HintAttribute("Check to allow members to post comments.")]
            public static bool EnablePhotoComments
            {
                get { return DBSettings.Get("Photos_EnablePhotoComments", true); }
                set { DBSettings.Set("Photos_EnablePhotoComments", value); }
            }

            [Reflection.DescriptionAttribute("Photo notes")]
            [Reflection.HintAttribute("Check to allow members to add photo notes.")]
            public static bool EnablePhotoNotes
            {
                get { return DBSettings.Get("Photos_EnablePhotoNotes", false); }
                set { DBSettings.Set("Photos_EnablePhotoNotes", value); }
            }

            [Reflection.DescriptionAttribute("Enable jQuery.Popeye")]
            [Reflection.HintAttribute("Check to enable the popeye plugin for the user primary photo.")]
            public static bool EnableJQueryPopeye
            {
                get { return DBSettings.Get("Photos_EnableJQueryPopeye", true); }
                set { DBSettings.Set("Photos_EnableJQueryPopeye", value); }
            }

            [Reflection.DescriptionAttribute("Free members can view photos of paid members")]
            [Reflection.HintAttribute("Check to allow free members to view photos of paid members.")]
            public static bool FreeMembersCanViewPhotosOfPaidMembers
            {
                get { return DBSettings.Get("Photos_FreeMembersCanViewPhotosOfPaidMembers", true); }
                set { DBSettings.Set("Photos_FreeMembersCanViewPhotosOfPaidMembers", value); }
            }

            [Reflection.DescriptionAttribute("Use square thumbnails")]
            [Reflection.HintAttribute("Check to use square thumbnails.")]
            public static bool UseSquareThumbnails
            {
                get { return DBSettings.Get("Photos_UseSquareThumbnails", false); }
                set { DBSettings.Set("Photos_UseSquareThumbnails", value); }
            }

            [Reflection.DescriptionAttribute("Use lazy images loading")]
            [Reflection.HintAttribute("Check to use lazy images loading.")]
            public static bool UseLazyImagesLoading
            {
                get { return DBSettings.Get("Photos_UseLazyImagesLoading", false); }
                set { DBSettings.Set("Photos_UseLazyImagesLoading", value); }
            }

            [Reflection.DescriptionAttribute("Enable salute photo")]
            [Reflection.HintAttribute("Check to enable salute photo.")]
            public static bool EnableSalutePhoto
            {
                get { return DBSettings.Get("Photos_EnableSalutePhoto", false); }
                set { DBSettings.Set("Photos_EnableSalutePhoto", value); }
            }

            [Reflection.DescriptionAttribute("Enable photo stack effect")]
            [Reflection.HintAttribute("Check to enable photo stack effect.")]
            public static bool EnablePhotoStack
            {
                get { return DBSettings.Get("Photos_EnablePhotoStack", true); }
                set { DBSettings.Set("Photos_EnablePhotoStack", value); }
            }
        }

        #endregion

        #region Nested type: BackwardCompatibility

        [Reflection.DescriptionAttribute("Backward Compatibility Settings")]
        public class BackwardCompatibility
        {
            [Reflection.DescriptionAttribute("Use classic search page")]
            [Reflection.HintAttribute("Check to enable auto approve photos.")]
            public static bool UseClassicSearchPage
            {
                get { return DBSettings.Get("BackwardCompatibility_UseClassicSearchPage", false); }
                set { DBSettings.Set("BackwardCompatibility_UseClassicSearchPage", value); }
            }
        }

        #endregion

        #region Nested type: Profiles

        [Reflection.DescriptionAttribute("Profile Settings")]
        public class Profiles
        {
            /// <summary>
            /// Gets or sets the max topic columns.
            /// </summary>
            /// <value>The max topic columns.</value>
            [Reflection.DescriptionAttribute("Maximum topic columns")]
            public static int MaxTopicColumns
            {
                get { return DBSettings.Get("Profiles_MaxTopicColumns", 5); }
                set { DBSettings.Set("Profiles_MaxTopicColumns", value); }
            }

            [Reflection.DescriptionAttribute("Number of profile comments to show")]
            public static int NumberOfProfileCommentsToShow
            {
                get { return DBSettings.Get("Profiles_NumberOfProfileCommentsToShow", 5); }
                set { DBSettings.Set("Profiles_NumberOfProfileCommentsToShow", value); }
            }

            [Reflection.DescriptionAttribute("Number of videos to show")]
            public static int NumberOfProfileVideosToShow
            {
                get { return DBSettings.Get("Profiles_NumberOfProfileVideosToShow", 6); }
                set { DBSettings.Set("Profiles_NumberOfProfileVideosToShow", value); }
            }

            [Reflection.DescriptionAttribute("Number of photo comments to show")]
            public static int NumberOfPhotoCommentsToShow
            {
                get { return DBSettings.Get("Profiles_NumberOfPhotoCommentsToShow", 5); }
                set { DBSettings.Set("Profiles_NumberOfPhotoCommentsToShow", value); }
            }
        }

        #endregion

        #region Nested type: Ratings

        [Reflection.DescriptionAttribute("Rating Settings")]
        public class Ratings
        {
            /// <summary>
            /// Gets or sets the min rating.
            /// </summary>
            /// <value>The min rating.</value>
            [Reflection.DescriptionAttribute("Minimum rating")]
            public static int MinRating
            {
                get { return DBSettings.Get("Ratings_MinRating", 1); }
                set { DBSettings.Set("Ratings_MinRating", value); }
            }

            /// <summary>
            /// Gets or sets the max rating.
            /// </summary>
            /// <value>The max rating.</value>
            [Reflection.DescriptionAttribute("Maximum rating")]
            public static int MaxRating
            {
                get { return DBSettings.Get("Ratings_MaxRating", 10); }
                set { DBSettings.Set("Ratings_MaxRating", value); }
            }

            /// <summary>
            /// Gets or sets the top users count.
            /// </summary>
            /// <value>The top users count.</value>
            [Reflection.DescriptionAttribute("TopUser count")]
            public static int TopUsersCount
            {
                get { return DBSettings.Get("Ratings_TopUsersCount", 5); }
                set { DBSettings.Set("Ratings_TopUsersCount", value); }
            }

            /// <summary>
            /// Gets or sets the top photos count.
            /// </summary>
            /// <value>The top photos count.</value>
            [Reflection.DescriptionAttribute("TopPhoto count")]
            public static int TopPhotosCount
            {
                get { return DBSettings.Get("Ratings_TopPhotosCount", 5); }
                set { DBSettings.Set("Ratings_TopPhotosCount", value); }
            }

            /// <summary>
            /// Gets or sets the top users min votes.
            /// </summary>
            /// <value>The top users min votes.</value>
            [Reflection.DescriptionAttribute("Minimum required votes for TopUser list")]
            public static int TopUsersMinVotes
            {
                get { return DBSettings.Get("Ratings_TopUsersMinVotes", 10); }
                set { DBSettings.Set("Ratings_TopUsersMinVotes", value); }
            }

            /// <summary>
            /// Gets or sets the top photos min votes.
            /// </summary>
            /// <value>The top photos min votes.</value>
            [Reflection.DescriptionAttribute("Minimum required votes for TopPhotos list")]
            public static int TopPhotosMinVotes
            {
                get { return DBSettings.Get("Ratings_TopPhotosMinVotes", 10); }
                set { DBSettings.Set("Ratings_TopPhotosMinVotes", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [user can rate another user].
            /// </summary>
            /// <value><c>true</c> if [user can rate another user]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Enable Profile Ratings")]
            public static bool EnableProfileRatings
            {
                get { return DBSettings.Get("Photos_EnableProfileRatings", true); }
                set { DBSettings.Set("Photos_EnableProfileRatings", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [user can rate photos].
            /// </summary>
            /// <value><c>true</c> if [user can rate photos]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Enable Photo Ratings")]
            public static bool EnablePhotoRatings
            {
                get { return DBSettings.Get("Photos_EnablePhotoRatings", false); }
                set { DBSettings.Set("Photos_EnablePhotoRatings", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [user can vote for another user].
            /// </summary>
            /// <value><c>true</c> if [user can vote for another user]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Enable Profile Voting")]
            public static bool EnableProfileVoting
            {
                get { return DBSettings.Get("Photos_EnableProfileVoting", false); }
                set { DBSettings.Set("Photos_EnableProfileVoting", value); }
            }

            [Reflection.DescriptionAttribute("Enable Photo Contests")]
            public static bool EnablePhotoContests
            {
                get { return DBSettings.Get("Contest_EnablePhotoContests", false); }
                set { DBSettings.Set("Contest_EnablePhotoContests", value); }
            }

            [Reflection.DescriptionAttribute("Minimum number of entries to start photo contest")]
            public static int MinPhotosToStartContest
            {
                get { return DBSettings.Get("Contest_MinPhotosToStartContest", 2); }
                set { DBSettings.Set("Contest_MinPhotosToStartContest", value); }
            }

            [Reflection.DescriptionAttribute("Number of favorite entries to use for comparison")]
            public static int FavoriteEntriesCount
            {
                get { return DBSettings.Get("Contest_FavoriteEntriesCount", 10); }
                set { DBSettings.Set("Contest_FavoriteEntriesCount", value); }
            }

            [Reflection.DescriptionAttribute("Number of entries to show in the top entries chart")]
            public static int TopEntriesCount
            {
                get { return DBSettings.Get("Contest_TopEntriesCount", 12); }
                set { DBSettings.Set("Contest_TopEntriesCount", value); }
            }

            /// <summary>
            /// Gets or sets the max accounts per IP.
            /// </summary>
            /// <value>The max accounts per IP.</value>
            [Reflection.DescriptionAttribute("Disable voting for IP if accounts are more than")]
            public static int MaxAccountsPerIP
            {
                get { return DBSettings.Get("Ratings_MaxAccountsPerIP", 0); }
                set { DBSettings.Set("Ratings_MaxAccountsPerIP", value); }
            }

            [Reflection.DescriptionAttribute("Enable voting after specified period (in days)")]
            public static int MinDaysToVote
            {
                get { return DBSettings.Get("Ratings_MinDaysToVote", 0); }
                set { DBSettings.Set("Ratings_MinDaysToVote", value); }
            }

            [Reflection.DescriptionAttribute("Enable voting after specified logins")]
            public static int MinLoginsToVote
            {
                get { return DBSettings.Get("Ratings_MinLoginsToVote", 0); }
                set { DBSettings.Set("Ratings_MinLoginsToVote", value); }
            }

            [Reflection.DescriptionAttribute("Enable voting after specified profile views")]
            public static int MinViewsToVote
            {
                get { return DBSettings.Get("Ratings_MinViewsToVote", 0); }
                set { DBSettings.Set("Ratings_MinViewsToVote", value); }
            }

            [Reflection.DescriptionAttribute("Enable rate photos (hot or not page)")]
            public static bool EnableRatePhotos
            {
                get { return DBSettings.Get("Ratings_EnableRatePhotos", false); }
                set { DBSettings.Set("Ratings_EnableRatePhotos", value); }
            }

            [Reflection.DescriptionAttribute("Check to enable limit to interested gender for photo vote")]
            public static bool LimitToInterestedGender
            {
                get { return DBSettings.Get("Ratings_LimitToInterestedGender", false); }
                set { DBSettings.Set("Ratings_LimitToInterestedGender", value); }
            }

            [Reflection.DescriptionAttribute("Rate photos for users active within XX days")]
            public static int RatePhotosForUsersActiveWithinXXDays
            {
                get { return DBSettings.Get("Ratings_RatePhotosForUsersActiveWithinXXDays", 30); }
                set { DBSettings.Set("Ratings_RatePhotosForUsersActiveWithinXXDays", value); }
            }
        }

        #endregion

        #region Nested type: Search

        [Reflection.DescriptionAttribute("Search Settings")]
        public class Search
        {
            /// <summary>
            /// Gets or sets the results per page.
            /// </summary>
            /// <value>The results per page.</value>
            public static int ResultsPerPage
            {
                get { return 6; return DBSettings.Get("Search_UsersPerPage", 6); }
                set { DBSettings.Set("Search_UsersPerPage", value); }
            }

            /// <summary>
            /// Gets or sets the users per page.
            /// </summary>
            /// <value>The users per page.</value>
            [Reflection.DescriptionAttribute("Users per page")]
            public static int UsersPerPage
            {
                get { return 6; return DBSettings.Get("Search_UsersPerPage", 6); }
                set { DBSettings.Set("Search_UsersPerPage", value); }
            }

            /// <summary>
            /// Gets or sets the videos per page.
            /// </summary>
            /// <value>The videos per page.</value>
            [Reflection.DescriptionAttribute("Videos per page")]
            public static int VideosPerPage
            {
                get { return DBSettings.Get("Search_VideosPerPage", 12); }
                set { DBSettings.Set("Search_VideosPerPage", value); }
            }

            /// <summary>
            /// Gets or sets the users per page when viewing as grid.
            /// </summary>
            /// <value>The users per page.</value>
            [Reflection.DescriptionAttribute("Users per page (grid)")]
            public static int UsersPerPageGrid
            {
                get { return 30; return DBSettings.Get("Search_UsersPerPageGrid", 30); }
                set { DBSettings.Set("Search_UsersPerPageGrid", value); }
            }

            [Reflection.DescriptionAttribute("Enable filter for online users")]
            [Reflection.HintAttribute("Check to enable filter for online users.")]
            public static bool FilterOnlineUsers
            {
                get { return DBSettings.Get("Search_FilterOnlineUsers", false); }
                set { DBSettings.Set("Search_FilterOnlineUsers", value); }
            }

            /// <summary>
            /// Enable/Disable Distance Search.
            /// </summary>
            /// <value>is enabled?</value>
            [Reflection.DescriptionAttribute("Distance Search enabled")]
            public static bool DistanceSearchEnabled
            {
                get { return DBSettings.Get("Search_DistanceSearchEnabled", false); }
                set { DBSettings.Set("Search_DistanceSearchEnabled", value); }
            }

            //[Reflection.DescriptionAttribute("Use precise method for distance calculation")]
            //[Reflection.HintAttribute("Using the precise method would cause higher cpu load")]
            //public static bool UsePreciseMethod
            //{
            //    get { return DBSettings.Get("Search_UsePreciseMethod", false); }
            //    set { DBSettings.Set("Search_UsePreciseMethod", value); }
            //}            

            [Reflection.DescriptionAttribute("Show distance from online user")]
            [Reflection.HintAttribute("Enable show distance from online user.")]
            public static bool ShowDistanceFromOnlineUser
            {
                get { return DBSettings.Get("Search_ShowDistanceFromOnlineUser", false); }
                set { DBSettings.Set("Search_ShowDistanceFromOnlineUser", value); }
            }

            [Reflection.DescriptionAttribute("Measure distance in kilometers")]
            [Reflection.HintAttribute("if checked, measured in kilometers, otherwise in miles")]
            public static bool MeasureDistanceInKilometers
            {
                get { return DBSettings.Get("Search_MeasureDistanceInKilometers", false); }
                set { DBSettings.Set("Search_MeasureDistanceInKilometers", value); }
            }

            /// <summary>
            /// Gets or sets the maximum returned users for Distance Search.
            /// </summary>
            /// <value>maximum users value.</value>
            [Reflection.DescriptionAttribute("Maximum users returned for Distance Search")]
            public static int DistanceSearchMaxUsers
            {
                get { return DBSettings.Get("Search_DistanceSearchMaxUsers", 150); }
                set { DBSettings.Set("Search_DistanceSearchMaxUsers", value); }
            }

            /// <summary>
            /// Gets or sets the maximum distance from user for Distance Search.
            /// </summary>
            /// <value>maximum distance value.</value>
            [Reflection.DescriptionAttribute("Maximum distance from User for Distance Search")]
            [Reflection.HintAttribute("Measured in miles or kilometers. Depends on the selection made above.")]
            public static double DistanceSearchMaxDistance
            {
                get { return DBSettings.Get("Search_DistanceSearchMaxDistance", 1000.0); }
                set { DBSettings.Set("Search_DistanceSearchMaxDistance", value); }
            }

            [Reflection.DescriptionAttribute("Default to custom search")]
            [Reflection.HintAttribute("Check to enable default to custom search")]
            public static bool DefaultToCustomSearch
            {
                get { return DBSettings.Get("Search_DefaultToCustomSearch", false); }
                set { DBSettings.Set("Search_DefaultToCustomSearch", value); }
            }
        }

        #endregion

        #region Nested type: SEO

        [Reflection.DescriptionAttribute("SEO settings")]
        public class SEO
        {
            [Reflection.DescriptionAttribute("Enable Url Rewriting")]
            public static bool EnableUrlRewriting
            {
                get { return DBSettings.Get("SEO_EnableUrlRewriting", true); }
                set { DBSettings.Set("SEO_EnableUrlRewriting", value); }
            }

            [Reflection.DescriptionAttribute("Template for Default title")]
            [Reflection.HintAttribute("Use %%NAME%% where you want to put name of the site")]
            public static string DefaultTitleTemplate
            {
                get { return DBSettings.Get("SEO_DefaultTitleTemplate", "%%NAME%%"); }
                set { DBSettings.Set("SEO_DefaultTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Default meta description")]
            public static string DefaultMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_DefaultMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_DefaultMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Default meta keywords")]
            public static string DefaultMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_DefaultMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_DefaultMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUser.aspx title")]
            public static string ShowUserTitleTemplate
            {
                get
                {
                    return DBSettings.Get("SEO_ShowUserTitleTemplate",
                                          "%%USERNAME%%'s profile - %%AGE%% years old %%GENDER%% from %%COUNTRY%%");
                }
                set { DBSettings.Set("SEO_ShowUserTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUser.aspx meta description")]
            public static string ShowUserMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUser.aspx meta keywords")]
            public static string ShowUserMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Ads.aspx title")]
            public static string AdsTitleTemplate
            {
                get { return DBSettings.Get("SEO_AdsTitleTemplate", ""); }
                set { DBSettings.Set("SEO_AdsTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Ads.aspx meta description")]
            public static string AdsMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_AdsMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_AdsMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Ads.aspx meta keywords")]
            public static string AdsMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_AdsMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_AdsMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ChangeLostPassword.aspx title")]
            public static string ChangeLostPasswordTitleTemplate
            {
                get
                {
                    return DBSettings.Get("SEO_ChangeLostPasswordTitleTemplate", "");
                }
                set { DBSettings.Set("SEO_ChangeLostPasswordTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ChangeLostPassword.aspx meta description")]
            public static string ChangeLostPasswordMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ChangeLostPasswordMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_ChangeLostPasswordMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ChangeLostPassword.aspx meta keywords")]
            public static string ChangeLostPasswordMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ChangeLostPasswordMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_ChangeLostPasswordMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Ads.aspx title")]
            public static string DefaultPageTitleTemplate
            {
                get { return DBSettings.Get("SEO_DefaultPageTitleTemplate", ""); }
                set { DBSettings.Set("SEO_DefaultPageTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Default.aspx meta description")]
            public static string DefaultPageMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_DefaultPageMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_DefaultPageMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Default.aspx meta keywords")]
            public static string DefaultPageMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_DefaultPageMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_DefaultPageMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Groups.aspx title")]
            public static string GroupsTitleTemplate
            {
                get { return DBSettings.Get("SEO_GroupsTitleTemplate", ""); }
                set { DBSettings.Set("SEO_GroupsTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Groups.aspx meta description")]
            public static string GroupsMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_GroupsMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_GroupsMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Groups.aspx meta keywords")]
            public static string GroupsMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_GroupsMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_GroupsMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Login.aspx title")]
            public static string LoginTitleTemplate
            {
                get { return DBSettings.Get("SEO_LoginTitleTemplate", ""); }
                set { DBSettings.Set("SEO_LoginTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Login.aspx meta description")]
            public static string LoginMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_LoginMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_LoginMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Login.aspx meta keywords")]
            public static string LoginMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_LoginMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_LoginMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for LostPassword.aspx title")]
            public static string LostPasswordTitleTemplate
            {
                get { return DBSettings.Get("SEO_LostPasswordTitleTemplate", ""); }
                set { DBSettings.Set("SEO_LostPasswordTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for LostPassword.aspx meta description")]
            public static string LostPasswordMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_LostPasswordMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_LostPasswordMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for LostPassword.aspx meta keywords")]
            public static string LostPasswordMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_LostPasswordMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_LostPasswordMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for News.aspx title")]
            public static string NewsTitleTemplate
            {
                get { return DBSettings.Get("SEO_NewsTitleTemplate", ""); }
                set { DBSettings.Set("SEO_NewsTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for News.aspx meta description")]
            public static string NewsMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_NewsMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_NewsMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for News.aspx meta keywords")]
            public static string NewsMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_NewsMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_NewsMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Registration.aspx title")]
            public static string RegisterTitleTemplate
            {
                get { return DBSettings.Get("SEO_RegisterTitleTemplate", ""); }
                set { DBSettings.Set("SEO_RegisterTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Registration.aspx meta description")]
            public static string RegisterMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_RegisterMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_RegisterMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Registration.aspx meta keywords")]
            public static string RegisterMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_RegisterMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_RegisterMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Search.aspx title")]
            public static string SearchTitleTemplate
            {
                get { return DBSettings.Get("SEO_SearchTitleTemplate", ""); }
                set { DBSettings.Set("SEO_SearchTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Search.aspx meta description")]
            public static string SearchMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_SearchMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_SearchMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for Search.aspx meta keywords")]
            public static string SearchMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_SearchMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_SearchMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for SendProfile.aspx title")]
            public static string SendProfileTitleTemplate
            {
                get { return DBSettings.Get("SEO_SendProfileTitleTemplate", ""); }
                set { DBSettings.Set("SEO_SendProfileTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for SendProfile.aspx meta description")]
            public static string SendProfileMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_SendProfileMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_SendProfileMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for SendProfile.aspx meta keywords")]
            public static string SendProfileMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_SendProfileMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_SendProfileMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowAd.aspx title")]
            [Reflection.HintAttribute(
                "Use %%CATEGORY%%, %%SUBJECT%%, %%DATE%%, %%EXPIRATIONDATE%%, %%LOCATION%%, %%POSTEDBY%% where you want to put respectively category of the classified, subject, date of creation and expiration, location and username of the poster"
                )]
            public static string ShowAdTitleTemplate
            {
                get
                {
                    return DBSettings.Get("SEO_ShowAdTitleTemplate",
                                          "%%SUBJECT%%");
                }
                set { DBSettings.Set("SEO_ShowAdTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowAd.aspx meta description")]
            public static string ShowAdMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowAdMetaDescriptionTemplate", "%%SUBJECT%%"); }
                set { DBSettings.Set("SEO_ShowAdMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowAd.aspx meta keywords")]
            public static string ShowAdMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowAdMetaKeywordsTemplate", "%%SUBJECT%%"); }
                set { DBSettings.Set("SEO_ShowAdMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroup.aspx title")]
            [Reflection.HintAttribute(
                "Use %%CATEGORIES%%, %%DATECREATED%%, %%TYPE%%, %%MEMBERS%%, %%OWNER%% where you want to put respectively categories of the group, date of creation, type, number of members and the owner"
                )]
            public static string ShowGroupTitleTemplate
            {
                get
                {
                    return DBSettings.Get("SEO_ShowGroupTitleTemplate",
                                          "%%NAME%%");
                }
                set { DBSettings.Set("SEO_ShowGroupTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroup.aspx meta description")]
            public static string ShowGroupMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupMetaDescriptionTemplate", "%%NAME%%"); }
                set { DBSettings.Set("SEO_ShowGroupMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroup.aspx meta keywords")]
            public static string ShowGroupMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupMetaKeywordsTemplate", "%%NAME%%"); }
                set { DBSettings.Set("SEO_ShowGroupMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupEvents.aspx title")]
            public static string ShowGroupEventsTitleTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupEventsTitleTemplate", ""); }
                set { DBSettings.Set("SEO_ShowGroupEventsTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupEvents.aspx meta description")]
            public static string ShowGroupEventsMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupEventsMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_ShowGroupEventsMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupEvents.aspx meta keywords")]
            public static string ShowGroupEventsMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupEventsMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_ShowGroupEventsMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupPhotos.aspx title")]
            public static string ShowGroupPhotosTitleTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupPhotosTitleTemplate", ""); }
                set { DBSettings.Set("SEO_ShowGroupPhotosTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupPhotos.aspx meta description")]
            public static string ShowGroupPhotosMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupPhotosMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_ShowGroupPhotosMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupPhotos.aspx meta keywords")]
            public static string ShowGroupPhotosMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupPhotosMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_ShowGroupPhotosMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupTopics.aspx title")]
            [Reflection.HintAttribute(
                "Use %%GROUP%%, %%NAME%%, %%USERNAME%% where you want to put respectively group of the topic, name of the topic and username of the creator"
                )]
            public static string ShowGroupTopicTitleTemplate
            {
                get
                {
                    return DBSettings.Get("SEO_ShowGroupTopicTitleTemplate",
                                          "%%NAME%%");
                }
                set { DBSettings.Set("SEO_ShowGroupTopicTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupTopics.aspx meta description")]
            public static string ShowGroupTopicMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupTopicMetaDescriptionTemplate", "%%NAME%%"); }
                set { DBSettings.Set("SEO_ShowGroupTopicMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowGroupTopics.aspx meta keywords")]
            public static string ShowGroupTopicMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowGroupTopicMetaKeywordsTemplate", "%%NAME%%"); }
                set { DBSettings.Set("SEO_ShowGroupTopicMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserBlog.aspx title")]
            public static string ShowUserBlogTitleTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserBlogTitleTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserBlogTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserBlog.aspx meta description")]
            public static string ShowUserBlogMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserBlogMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserBlogMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserBlog.aspx meta keywords")]
            public static string ShowUserBlogMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserBlogMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserBlogMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserEvents.aspx title")]
            public static string ShowUserEventsTitleTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserEventsTitleTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserEventsTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserEvents.aspx meta description")]
            public static string ShowUserEventsMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserEventsMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserEventsMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserEvents.aspx meta keywords")]
            public static string ShowUserEventsMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserEventsMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserEventsMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserPhotos.aspx title")]
            public static string ShowUserPhotosTitleTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserPhotosTitleTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserPhotosTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserPhotos.aspx meta description")]
            public static string ShowUserPhotosMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserPhotosMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserPhotosMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for ShowUserPhotos.aspx meta keywords")]
            public static string ShowUserPhotosMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_ShowUserPhotosMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_ShowUserPhotosMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for SmsConfirm.aspx title")]
            public static string SmsConfirmTitleTemplate
            {
                get { return DBSettings.Get("SEO_SmsConfirmTitleTemplate", ""); }
                set { DBSettings.Set("SEO_SmsConfirmTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for SmsConfirm.aspx meta description")]
            public static string SmsConfirmMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_SmsConfirmMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_SmsConfirmMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for SmsConfirm.aspx meta keywords")]
            public static string SmsConfirmMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_SmsConfirmMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_SmsConfirmMetaKeywordsTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for TopCharts.aspx title")]
            public static string TopChartsTitleTemplate
            {
                get { return DBSettings.Get("SEO_TopChartsTitleTemplate", ""); }
                set { DBSettings.Set("SEO_TopChartsTitleTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for TopCharts.aspx meta description")]
            public static string TopChartsMetaDescriptionTemplate
            {
                get { return DBSettings.Get("SEO_TopChartsMetaDescriptionTemplate", ""); }
                set { DBSettings.Set("SEO_TopChartsMetaDescriptionTemplate", value); }
            }

            [Reflection.DescriptionAttribute("Template for TopCharts.aspx meta keywords")]
            public static string TopChartsMetaKeywordsTemplate
            {
                get { return DBSettings.Get("SEO_TopChartsMetaKeywordsTemplate", ""); }
                set { DBSettings.Set("SEO_TopChartsMetaKeywordsTemplate", value); }
            }
        }

        #endregion

        #region Nested type: ThirdPartyServices

        [Reflection.DescriptionAttribute("Third Party Services")]
        public class ThirdPartyServices
        {
            [Reflection.DescriptionAttribute("Google Maps API key")]
            public static string GoogleMapsAPIKey
            {
                get { return DBSettings.Get("ThirdPartyServices_GoogleMapsAPIKey", ""); }
                set { DBSettings.Set("ThirdPartyServices_GoogleMapsAPIKey", value); }
            }

            [Reflection.DescriptionAttribute("Get Missing Coordinates From Google Maps")]
            public static bool GetMissingCoordinatesFromGoogleMaps
            {
                get { return DBSettings.Get("ThirdPartyServices_GetMissingCoordinatesFromGoogleMaps", true); }
                set { DBSettings.Set("ThirdPartyServices_GetMissingCoordinatesFromGoogleMaps", value); }
            }

            [Reflection.DescriptionAttribute("Show Google Maps for Group Events")]
            public static bool ShowGoogleMapsForGroupEvents
            {
                get { return DBSettings.Get("ThirdPartyServices_ShowGoogleMapsForGroupEvents", true); }
                set { DBSettings.Set("ThirdPartyServices_ShowGoogleMapsForGroupEvents", value); }
            }

            [Reflection.DescriptionAttribute("Use Google Translate for Received Messages")]
            public static bool UseGoogleTranslateForMessages
            {
                get { return DBSettings.Get("ThirdPartyServices_UseGoogleTranslateForMessages", false); }
                set { DBSettings.Set("ThirdPartyServices_UseGoogleTranslateForMessages", value); }
            }

            [Reflection.DescriptionAttribute("Use AddThis sharing and bookmark service")]
            public static bool UseAddThis
            {
                get { return DBSettings.Get("ThirdPartyServices_UseAddThis", false); }
                set { DBSettings.Set("ThirdPartyServices_UseAddThis", value); }
            }

            [Reflection.DescriptionAttribute("AddThis sharing and bookmark code")]
            public static string AddThisCode
            {
                get { return DBSettings.Get("ThirdPartyServices_AddThisCode", "<a href=\"https://www.addthis.com/bookmark.php?v=250&pub=xa-4a48e43f4e1b71c2\" onmouseover=\"return addthis_open(this, '', '[URL]', '[TITLE]')\" onmouseout=\"addthis_close()\" onclick=\"return addthis_sendto()\"><img src=\"https://s7.addthis.com/static/btn/lg-share-en.gif\" width=\"125\" height=\"16\" alt=\"Bookmark and Share\" style=\"border:0\"/></a><script type=\"text/javascript\" src=\"https://s7.addthis.com/js/250/addthis_widget.js?pub=xa-4a48e43f4e1b71c2\"></script>"); }
                set { DBSettings.Set("ThirdPartyServices_AddThisCode", value); }
            }
        }

        #endregion

        #region Nested type: Urls

        [Reflection.DescriptionAttribute("URLs")]
        public class Urls
        {
            /// <summary>
            /// Gets or sets the home.
            /// </summary>
            /// <value>The home.</value>
            public static string ImagesHome
            {
                get
                {
                    if (String.IsNullOrEmpty(Properties.Settings.Default.ImagesHomeURL))
                    {
                        return Home + "/images";
                    }
                    return Properties.Settings.Default.ImagesHomeURL.TrimEnd('/');
                }
            }

            public static string ImagesCacheHome
            {
                get
                {
                    if (String.IsNullOrEmpty(Properties.Settings.Default.ImagesCacheURL))
                    {
                        return Home + "/images/cache";
                    }
                    return Properties.Settings.Default.ImagesCacheURL.TrimEnd('/');
                }
            }

            /// <summary>
            /// Gets or sets the home.
            /// </summary>
            /// <value>The home.</value>
            public static string Home
            {
                get
                {
                    return string.IsNullOrEmpty(Properties.Settings.Default.HomeURL)
                        ? ServerSettings.DomainURL
                        : Properties.Settings.Default.HomeURL.TrimEnd('/');
                }
            }

            /// <summary>
            /// Gets or sets the facebook app home.
            /// </summary>
            /// <value>The home.</value>
            public static string FacebookHome
            {
                get
                {
                    return string.IsNullOrEmpty(Properties.Settings.Default.FacebookHome)
                        ? ServerSettings.DomainURL
                        : Properties.Settings.Default.FacebookHome.TrimEnd('/');
                }
            }

            public static string HomeMobile
            {
                get
                {
                    return Home + "/Mobile";
                    //if (string.IsNullOrEmpty(Properties.Settings.Default.HomeURL))
                    //{
                    //    return "https://localhost/Mobile";
                    //}
                    //return Properties.Settings.Default.HomeURL + "/Mobile".TrimEnd('/');
                }
            }

            public static string SecureHome
            {
                get { return Home.Replace("https://", "https://"); }
            }

            public static string ActivateAccount
            {
                get { return Home + "/ActivateAccount.aspx"; }
            }

            public static string ActivatePassword
            {
                get { return Home + "/ChangeLostPassword.aspx"; }
            }

            public static string ThankYou
            {
                get { return Home + "/ThankYou.aspx"; }
            }

            public static string PayPalIPN
            {
                get { return Home + "/PayPal/IpnHandler.aspx"; }
            }

            public static string Cancel
            {
                get { return Home + "/Cancel.aspx"; }
            }
        }

        #endregion

        #region Nested type: Users

        [Reflection.DescriptionAttribute("User Settings")]
        public class Users
        {
            private static readonly string DefaultNonPayingMembersOptions = Data.Misc.ToXml(new BillingPlanOptions());
            //private static Countries dscountries;

            public static string[] ReservedUsernames = { "admin", "support", "webmaster", "system" };

            public static string SystemUsername = "admin";

            /// <summary>
            /// Gets or sets a value indicating whether [users are activated automatically].
            /// </summary>
            /// <value><c>true</c> if [auto activated]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Auto activate users")]
            [Reflection.HintAttribute(
                "if checked, users will not receive activation email upon registration and will be activated automatically"
                )]
            public static bool AutoActivateUsers
            {
                //TODO: check that this is configurable via admin settings
                get { return DBSettings.Get("Users_AutoActivateUsers", false); }
                set { DBSettings.Set("Users_AutoActivateUsers", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [auto approve answers].
            /// </summary>
            /// <value><c>true</c> if [auto approve answers]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Auto approve answers for members registered more than xx days ago")]
            [Reflection.HintAttribute("Auto approve answers for members registered more than xx days ago")]
            public static int AutoApproveAnswers
            {
                get { return DBSettings.Get("Users_AutoApproveAnswersForUsersRegisteredMoreThanXXDaysAgo", 365); }
                set { DBSettings.Set("Users_AutoApproveAnswersForUsersRegisteredMoreThanXXDaysAgo", value); }
            }


            //indicates whether the user should provide location info during the registration
            /// <summary>
            /// Gets or sets a value indicating whether [location panel visible].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [location panel visible]; otherwise, <c>false</c>.
            /// </value>
            [Reflection.DescriptionAttribute("Ask user for location")]
            [
                Reflection.HintAttribute(
                    "Uncheck if running local site. Checking will enable fields for city, state, zip, country.")]
            public static bool LocationPanelVisible
            {
                get { return DBSettings.Get("Users_LocationPanelVisible", true); }
                set { DBSettings.Set("Users_LocationPanelVisible", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether payment is required.
            /// </summary>
            /// <value><c>true</c> if is payment required; otherwise, <c>false</c>.</value>
            //[Reflection.DescriptionAttribute("Require payment")]
            //[
            //    Reflection.HintAttribute(
            //        "Unchecking this box will switch the site to free mode. Checking it will enable site payments.")]
            //public static bool PaymentRequired
            //{
            //    get { return DBSettings.Get("Users_PaymentRequired", false); }
            //    set { DBSettings.Set("Users_PaymentRequired", value); }
            //}

            [Reflection.DescriptionAttribute("Require sms confirmation (must be integrated with sms gateway)")]
            public static bool SmsConfirmationRequired
            {
                get { return DBSettings.Get("Users_SmsConfirmationRequired", false); }
                set { DBSettings.Set("Users_SmsConfirmationRequired", value); }
            }

            /// <summary>
            /// Gets or sets new users' free trial period.
            /// </summary>
            /// <value>trial period in days</value>
            [Reflection.DescriptionAttribute("Trial period (in days)")]
            [
                Reflection.HintAttribute(
                    "Initial Period (in days) during which members will not be required to pay in order to communicate with other members. Set to 0 to disable."
                    )]
            public static int TrialPeriod
            {
                get { return DBSettings.Get("Users_TrialPeriod", 0); }
                set { DBSettings.Set("Users_TrialPeriod", value); }
            }


            /// <summary>
            /// Gets or sets a value indicating whether [couples support].
            /// </summary>
            /// <value><c>true</c> if [couples support]; otherwise, <c>false</c>.</value>
            [Reflection.DescriptionAttribute("Support for couples")]
            [
                Reflection.HintAttribute(
                    "Check to enable support for couples. Profile questions may require additional settings to support couples."
                    )]
            public static bool CouplesSupport
            {
                get { return DBSettings.Get("Users_CouplesSupport", false); }
                set { DBSettings.Set("Users_CouplesSupport", value); }
            }


            //maximum time away from the site in days to be listed in top users list
            /// <summary>
            /// Gets or sets the top user max time away.
            /// </summary>
            /// <value>The top user max time away.</value>
            [Reflection.DescriptionAttribute("Maximum time away to be listed as top user (in days)")]
            [
                Reflection.HintAttribute(
                    "Users who have not logged in more than XX days will not appear in the top users page.")]
            public static int TopUserMaxTimeAway
            {
                get { return DBSettings.Get("Users_TopUserMaxTimeAway", 30); }
                set { DBSettings.Set("Users_TopUserMaxTimeAway", value); }
            }

            //private static Countries dsCountries
            //{
            //    get
            //    {
            //        if (dscountries == null)
            //        {
            //            dscountries = new Countries();
            //            dscountries.ReadXml(HttpContext.Current.Server.MapPath("~/Countries.xml"));
            //        }

            //        return dscountries;
            //    }
            //}
            public static XDocument GetCountriesXml(HttpContext context)
            {
                return xdCountries;
            }
            private static XDocument xdCountries
            {
                get
                {
                    string cacheKey = "xdCountries";
                    if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                    {
                        return HttpContext.Current.Cache[cacheKey] as XDocument;
                    }

                    if (HttpContext.Current != null)
                    {
                        var document =
                            XDocument.Load(HttpContext.Current.Server.MapPath("~/Countries.xml"));

                        HttpContext.Current.Cache.Add(cacheKey, document, null, Cache.NoAbsoluteExpiration,
                            TimeSpan.FromMinutes(10), CacheItemPriority.High, null);

                        return document;
                    }

                    return null;
                }
            }

            public static string[] Countries
            {
                get { return GetCountries(); }
            }

            public static string DefaultCountry
            {
                get
                {
                    try
                    {
                        string country = Properties.Settings.Default.DefaultCountry;

                        if (!String.IsNullOrEmpty(country))
                        {
                            if (Array.IndexOf(Countries, country) == -1)
                                throw new Exception(
                                    "The specified default country in your web.config file does not match one of the countries in Countries.xml");

                            return country;
                        }
                        if (HttpContext.Current != null && PreSelectCountryBasedOnIP)
                        {
                            string userCountryCode = IPToCountry.GetCountry(HttpContext.Current.Request.UserHostAddress);

                            if (userCountryCode == null)
                                return String.Empty;

                            return GetCountryByCode(userCountryCode) ?? String.Empty;
                        }
                        return String.Empty;
                    }
                    catch (Exception ex)
                    {
                        Global.Logger.LogError(ex);
                        return String.Empty;
                    }
                }
            }

            public static string ForceCountry
            {
                get
                {
                    string country = Properties.Settings.Default.ForceCountry;

                    if (!string.IsNullOrEmpty(country))
                    {
                        if (Array.IndexOf(Countries, country) == -1)
                            throw new Exception(
                                "The specified forced country in your web.config file does not match one of the countries in Countries.xml");

                        return country;
                    }
                    return String.Empty;
                }
            }

            public static string ForceRegion
            {
                get
                {
                    string region = Properties.Settings.Default.ForceRegion;

                    if (!string.IsNullOrEmpty(region))
                    {
                        return region;
                    }
                    return String.Empty;
                }
            }

            public static string ForceCity
            {
                get
                {
                    string city = Properties.Settings.Default.ForceCity;

                    if (!string.IsNullOrEmpty(city))
                    {
                        return city;
                    }
                    return String.Empty;
                }
            }

            [Reflection.DescriptionAttribute("Preselect country based on IP address")]
            public static bool PreSelectCountryBasedOnIP
            {
                get { return DBSettings.Get("Users_PreSelectCountryBasedOnIP", false); }
                set { DBSettings.Set("Users_PreSelectCountryBasedOnIP", value); }
            }

            [Reflection.DescriptionAttribute("Enable Windows CardSpace support")]
            public static bool EnableCardSpace
            {
                get { return DBSettings.Get("Users_EnableCardSpace", false); }
                set { DBSettings.Set("Users_EnableCardSpace", value); }
            }

            //[Reflection.DescriptionAttribute("Enable e-cards")]
            //public static bool EnableEcards
            //{
            //    get { return DBSettings.Get("Users_EnableEcards", false); }
            //    set { DBSettings.Set("Users_EnableEcards", value); }
            //}

            [Reflection.DescriptionAttribute("Enable Favorites")]
            public static bool EnableFavorites
            {
                get { return DBSettings.Get("Users_EnableFavorites", true); }
                set { DBSettings.Set("Users_EnableFavorites", value); }
            }

            [Reflection.DescriptionAttribute("Enable Friends")]
            public static bool EnableFriends
            {
                get { return DBSettings.Get("Users_EnableFriends", true); }
                set { DBSettings.Set("Users_EnableFriends", value); }
            }

            [Reflection.DescriptionAttribute("Enable Friends Connection Search")]
            public static bool EnableFriendsConnectionSearch
            {
                get { return DBSettings.Get("Users_EnableFriendsConnection", true); }
                set { DBSettings.Set("Users_EnableFriendsConnection", value); }
            }

            [Reflection.DescriptionAttribute("Enable Photo albums")]
            public static bool EnablePhotoAlbums
            {
                get { return DBSettings.Get("Users_EnablePhotoAlbums", false); }
                set { DBSettings.Set("Users_EnablePhotoAlbums", value); }
            }

            /// <summary>
            /// Gets or sets the length of the username min.
            /// </summary>
            /// <value>The length of the username min.</value>
            [Reflection.DescriptionAttribute("Username minimum length")]
            [Reflection.HintAttribute("Define the minimal username length. Do NOT change on already running site!")]
            public static int UsernameMinLength
            {
                get { return DBSettings.Get("Users_UsernameMinLength", 6); }
                set { DBSettings.Set("Users_UsernameMinLength", value); }
            }

            /// <summary>
            /// Gets or sets the length of the username max.
            /// </summary>
            /// <value>The length of the username max.</value>
            [Reflection.DescriptionAttribute("Username maximum length")]
            [Reflection.HintAttribute("Define the maximal username length. Do NOT change on already running site!")]
            public static int UsernameMaxLength
            {
                get { return DBSettings.Get("Users_UsernameMaxLength", 20); }
                set { DBSettings.Set("Users_UsernameMaxLength", value); }
            }

            /// <summary>
            /// Gets or sets the length of the password min.
            /// </summary>
            /// <value>The length of the password min.</value>
            [Reflection.DescriptionAttribute("Password minimum length")]
            [Reflection.HintAttribute("Define the minimal password length. Recommended value is at least 6 characters.")]
            public static int PasswordMinLength
            {
                get { return DBSettings.Get("Users_PasswordMinLength", 6); }
                set { DBSettings.Set("Users_PasswordMinLength", value); }
            }

            /// <summary>
            /// Gets or sets the length of the password max.
            /// </summary>
            /// <value>The length of the password max.</value>
            [Reflection.DescriptionAttribute("Password maximum length")]
            [Reflection.HintAttribute("Define the maximal password length.")]
            public static int PasswordMaxLength
            {
                get { return DBSettings.Get("Users_PasswordMaxLength", 250); }
                set { DBSettings.Set("Users_PasswordMaxLength", value); }
            }

            /// <summary>
            /// Gets or sets the min age.
            /// </summary>
            /// <value>The min age.</value>
            [Reflection.DescriptionAttribute("Minimal member age")]
            [Reflection.HintAttribute("Specify the minimal member age.")]
            public static int MinAge
            {
                get { return DBSettings.Get("Users_MinAge", 18); }
                set { DBSettings.Set("Users_MinAge", value); }
            }

            /// <summary>
            /// Gets or sets the max age.
            /// </summary>
            /// <value>The max age.</value>
            [Reflection.DescriptionAttribute("Maximal member age")]
            [Reflection.HintAttribute("Specify the maximal member age.")]
            public static int MaxAge
            {
                get { return DBSettings.Get("Users_MaxAge", 81); }
                set { DBSettings.Set("Users_MaxAge", value); }
            }

            /// <summary>
            /// Gets or sets the min age for explicit photos.
            /// </summary>
            /// <value>The min age for explicit photos.</value>
            [Reflection.DescriptionAttribute("Minimum age to see explicit photos")]
            [Reflection.HintAttribute("Specify minimum age to see explicit photos.")]
            public static int MinAgeForExplicitPhotos
            {
                get { return DBSettings.Get("Users_MinAgeForExplicitPhotos", 18); }
                set { DBSettings.Set("Users_MinAgeForExplicitPhotos", value); }
            }

            /// <summary>
            /// Gets or sets the max favourite users.
            /// </summary>
            /// <value>The max favourite users.</value>
            [Reflection.DescriptionAttribute("Maximum favourite users")]
            [
                Reflection.HintAttribute(
                    "Limit the maximum amount of users that each member can add to the Favourites page.")]
            public static int MaxFavouriteUsers
            {
                get { return DBSettings.Get("Users_MaxFavouriteUsers", 10); }
                set { DBSettings.Set("Users_MaxFavouriteUsers", value); }
            }

            [Reflection.DescriptionAttribute("Maximum friend users")]
            [
                Reflection.HintAttribute(
                    "Limit the maximum amount of users that each member can add to the Friends page.")]
            public static int MaxFriendUsers
            {
                get { return DBSettings.Get("Users_MaxFriendUsers", 5000); }
                set { DBSettings.Set("Users_MaxFriendUsers", value); }
            }

            [Reflection.DescriptionAttribute("Maximum hops in friends connection search")]
            [
                Reflection.HintAttribute(
                    "Limit the maximum amount of users that the site will process for searching a connection.")]
            public static int MaxFriendsHops
            {
                get { return DBSettings.Get("Users_MaxFriendsHops", 5); }
                set { DBSettings.Set("Users_MaxFriendsHops", value); }
            }

            [Reflection.DescriptionAttribute("Maximum comments")]
            [
                Reflection.HintAttribute(
                    "Limit the maximum amount of comments that each member can add to the profile, photo and blog post.")]
            public static int MaxComments
            {
                get { return DBSettings.Get("Users_MaxComments", 5); }
                set { DBSettings.Set("Users_MaxComments", value); }
            }

            /// <summary>
            /// Gets or sets the online check time.
            /// </summary>
            /// <value>The online check time in minutes.</value>
            [Reflection.DescriptionAttribute("Online check time interval (in minutes)")]
            [
                Reflection.HintAttribute(
                    "Lower values make the Who is Online screen more accurate but also increase server load.")]
            public static int OnlineCheckTime
            {
                get { return DBSettings.Get("Users_OnlineCheckTime", 1); }
                set { DBSettings.Set("Users_OnlineCheckTime", value); }
            }

            /// <summary>
            /// Gets or sets the online check time.
            /// </summary>
            /// <value>The online check time in minutes.</value>
            public static int NotifierCheckTime
            {
                get { return DBSettings.Get("Users_NotifierCheckTime", 5); }
                set { DBSettings.Set("Users_NotifierCheckTime", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [the user would be asked about his/her interest]
            /// </summary>
            [Reflection.DescriptionAttribute("Enable \"Interested in\" field")]
            [
                Reflection.HintAttribute(
                    "Allows users to specify their preferred gender. The setting is later used to optimize some of the screens within the site."
                    )]
            public static bool InterestedInFieldEnabled
            {
                get { return true; }//DBSettings.Get("Users_InterestedInFieldEnabled", true); }
                set { DBSettings.Set("Users_InterestedInFieldEnabled", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether users will receive e-mail notifications by default
            /// </summary>
            [Reflection.DescriptionAttribute("Users will receive e-mail notifications by default")]
            [Reflection.HintAttribute("Indicating whether users will receive e-mail notifications by default")]
            public static bool EmailNotificationsDefault
            {
                get { return DBSettings.Get("Users_EmailNotificationsDefault", false); }
                set { DBSettings.Set("Users_EmailNotificationsDefault", value); }
            }


            /// <summary>
            /// Gets or sets a value indicating whether [only registered can view photos].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [only registered can view photos]; otherwise, <c>false</c>.
            /// </value>
            [Reflection.DescriptionAttribute("Require registration to view photos")]
            [
                Reflection.HintAttribute(
                    "Uncheck to allow non-registered visitors to view member photos. Checking will redirect visitors to the registration page."
                    )]
            public static bool OnlyRegisteredCanViewPhotos
            {
                get { return DBSettings.Get("Users_OnlyRegisteredCanViewPhotos", false); }
                set { DBSettings.Set("Users_OnlyRegisteredCanViewPhotos", value); }
            }

            [Reflection.DescriptionAttribute("Profile comments")]
            [Reflection.HintAttribute("Check to allow members to post comments.")]
            public static bool EnableProfileComments
            {
                get { return DBSettings.Get("Users_EnableProfileComments", true); }
                set { DBSettings.Set("Users_EnableProfileComments", value); }
            }

            //[Reflection.DescriptionAttribute("Limit members to this number of messages per day")]
            //[Reflection.HintAttribute("The maximum amount of messages per day per user.")]
            //public static int MembersMaxMessagesPerDay
            //{
            //    get { return DBSettings.Get("Users_MembersMaxMessagesPerDay", 20); }
            //    set { DBSettings.Set("Users_MembersMaxMessagesPerDay", value); }
            //}

            /// <summary>
            /// Gets or sets the number of messages per day that the unpaid users are limited to.
            /// </summary>
            /// <value>The number of messages.</value>
            //[Reflection.DescriptionAttribute("Limit unpaid members to this number of messages per day")]
            //[Reflection.HintAttribute("")]
            //public static int UnpaidMembersMaxMessagesPerDay
            //{
            //    get { return DBSettings.Get("Users_UnpaidMembersMaxMessagesPerDay", 100); }
            //    set { DBSettings.Set("Users_UnpaidMembersMaxMessagesPerDay", value); }
            //}

            [Reflection.DescriptionAttribute("Maximum users contacted per day")]
            [Reflection.HintAttribute("Maximum number of users that can be contacted per day by a single user.")]
            public static int MaxContactedUsersPerDay
            {
                get { return DBSettings.Get("Users_MaxContactedUsersPerDay", 20); }
                set { DBSettings.Set("Users_MaxContactedUsersPerDay", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [non-paid members can respond to paid members].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [non-paid members can respond to paid members]; otherwise, <c>false</c>.
            /// </value>
            [Reflection.DescriptionAttribute("Non-paid members can respond to paid members")]
            [
                Reflection.HintAttribute(
                    "Check to allow free members to respond to mails send from paid members. Free members will still not be able to initiate a conversation."
                    )]
            public static bool NonPaidMembersCanRespondToPaidMembers
            {
                get { return DBSettings.Get("Users_NonPaidMembersCanRespondToPaidMembers", true); }
                set { DBSettings.Set("Users_NonPaidMembersCanRespondToPaidMembers", value); }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [non-paid members can use prewritten responses only].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [non-paid members can use prewritten responses only]; otherwise, <c>false</c>.
            /// </value>
            //[Reflection.DescriptionAttribute("Non-paid members can use prewritten responses only")]
            //[Reflection.HintAttribute("Allow non-paid members reply with pre-written responses.")]
            //public static bool NonPaidMembersCanUsePrewrittenResponsesOnly
            //{
            //    get { return DBSettings.Get("Users_NonPaidMembersCanUsePrewrittenResponsesOnly", false); }
            //    set { DBSettings.Set("Users_NonPaidMembersCanUsePrewrittenResponsesOnly", value); }
            //}

            //[Reflection.DescriptionAttribute("Enable pre-written messages")]
            //[Reflection.HintAttribute("Allow users to use pre-written messages.")]
            //public static bool EnablePrewrittenMessages
            //{
            //    get { return DBSettings.Get("Users_EnablePrewrittenMessages", false); }
            //    set { DBSettings.Set("Users_EnablePrewrittenMessages", value); }
            //}

            /// <summary>
            /// Gets or sets a value indicating whether [members can certify that a particular user is genuine].
            /// </summary>
            /// <value>
            /// 	<c>true</c> if [members can certify that a particular user is genuine]; otherwise, <c>false</c>.
            /// </value>
            [Reflection.DescriptionAttribute("Enable \"Real Person Verification\" functionality")]
            [Reflection.HintAttribute("Allow users to certify that other users are genuine.")]
            public static bool EnableRealPersonVerificationFunctionality
            {
                get { return DBSettings.Get("Users_EnableRealPersonVerificationFunctionality", false); }
                set { DBSettings.Set("Users_EnableRealPersonVerificationFunctionality", value); }
            }

            [Reflection.DescriptionAttribute("Enable \"Real Person Verification\" functionality(Admin)")]
            [Reflection.HintAttribute("Allow admins to certify that users are genuine.")]
            public static bool EnableRealPersonVerificationFunctionalityAdmin
            {
                get { return DBSettings.Get("Users_EnableRealPersonVerificationFunctionalityAdmin", false); }
                set { DBSettings.Set("Users_EnableRealPersonVerificationFunctionalityAdmin", value); }
            }


            [Reflection.DescriptionAttribute("Enable zodiac sign")]
            [Reflection.HintAttribute("Check to enable zodiac sign.")]
            public static bool EnableZodiacSign
            {
                get { return DBSettings.Get("Users_EnableZodiacSign", true); }
                set { DBSettings.Set("Users_EnableZodiacSign", value); }
            }

            [Reflection.DescriptionAttribute("Enable who viewed my profile")]
            [Reflection.HintAttribute("Check to enable who viewed my profile.")]
            public static bool EnableWhoViewedMyProfile
            {
                get { return DBSettings.Get("Users_EnableWhoViewedMyProfile", true); }
                set { DBSettings.Set("Users_EnableWhoViewedMyProfile", value); }
            }

            [Reflection.DescriptionAttribute("Show distance from user")]
            [Reflection.HintAttribute("Enable show distance from user.")]
            public static bool ShowDistanceFromOnlineUser
            {
                get { return DBSettings.Get("Users_ShowDistanceFromOnlineUser", false); }
                set { DBSettings.Set("Users_ShowDistanceFromOnlineUser", value); }
            }

            /// <summary>
            /// Gets or sets the minimum user votes to mark member as verified.
            /// </summary>
            /// <value>The number of votes.</value>
            [Reflection.DescriptionAttribute("Minimum User Votes to mark member as Verified")]
            [
                Reflection.HintAttribute(
                    "When XX users certify that a user is genuine that user will receive a Verified status.")]
            public static int MinimumUserVotesToMarkMemberAsVerified
            {
                get { return DBSettings.Get("Users_MinimumUserVotesToMarkMemberAsVerified", 3); }
                set { DBSettings.Set("Users_MinimumUserVotesToMarkMemberAsVerified", value); }
            }

            [Reflection.DescriptionAttribute("Message Verification Enabled (spam check)")]
            [
                Reflection.HintAttribute(
                    "Spam prevention option. Checking will put messages on hold until they are approved by administrator."
                    )]
            public static bool MessageVerificationEnabled
            {
                get { return DBSettings.Get("Users_MessageVerificationEnabled", false); }
                set { DBSettings.Set("Users_MessageVerificationEnabled", value); }
            }

            [Reflection.DescriptionAttribute("Message Verifications Count")]
            [
                Reflection.HintAttribute(
                    "Set the amount of messages that have to be approved by administrator. Further messages will not require approval."
                    )]
            public static int MessageVerificationsCount
            {
                get { return DBSettings.Get("Users_MessageVerificationsCount", 5); }
                set { DBSettings.Set("Users_MessageVerificationsCount", value); }
            }

            [Reflection.DescriptionAttribute("Allow users to see message status")]
            [Reflection.HintAttribute("When enabled the user will see icons if their message has been read or deleted.")]
            public static bool UsersCanSeeMessageStatus
            {
                get { return DBSettings.Get("Users_UsersCanSeeMessageStatus", false); }
                set { DBSettings.Set("Users_UsersCanSeeMessageStatus", value); }
            }

            [Reflection.DescriptionAttribute("Registration required to browse")]
            [
                Reflection.HintAttribute(
                    "Uncheck to allow visitors to browse and view profiles. Check to redirect visitors to the registration page."
                    )]
            public static bool RegistrationRequiredToBrowse
            {
                get { return DBSettings.Get("Users_RegistrationRequiredToBrowse", true); }
                set { DBSettings.Set("Users_RegistrationRequiredToBrowse", value); }
            }

            [Reflection.DescriptionAttribute("Registration required to search")]
            [
                Reflection.HintAttribute(
                    "Uncheck to allow visitors to search for other profiles. Check to redirect visitors to the registration page."
                    )]
            public static bool RegistrationRequiredToSearch
            {
                get { return DBSettings.Get("Users_RegistrationRequiredToSearch", false); }
                set { DBSettings.Set("Users_RegistrationRequiredToSearch", value); }
            }

            [Reflection.DescriptionAttribute("Check for duplicate emails")]
            [Reflection.HintAttribute("Enable/disable check for duplicate emails.")]
            public static bool CheckForDuplicateEmails
            {
                get { return DBSettings.Get("Users_CheckForDuplicateEmails", true); }
                set { DBSettings.Set("Users_CheckForDuplicateEmails", value); }
            }

            [Reflection.DescriptionAttribute("User must have completed profile to browse/search")]
            public static bool CompletedProfileRequiredToBrowseSearch
            {
                get { return DBSettings.Get("Users_CompletedProfileRequiredToBrowseSearch", false); }
                set { DBSettings.Set("Users_CompletedProfileRequiredToBrowseSearch", value); }
            }

            /// <summary>
            /// Gets or sets the invitation code needed to register.
            /// </summary>
            /// <value>The invitation code.</value>
            [Reflection.DescriptionAttribute("Invitation Code needed to register")]
            [
                Reflection.HintAttribute(
                    "If you are running a private site you can set an invitation code that will be required upon registration."
                    )]
            public static string InvitationCode
            {
                get { return DBSettings.Get("Users_InvitationCode", String.Empty); }
                set { DBSettings.Set("Users_InvitationCode", value); }
            }

            [Reflection.DescriptionAttribute("Notify user of new messages (while logged on)")]
            [Reflection.HintAttribute("Check to enable real-time notification for new messages.")]
            public static bool NewMessageNotification
            {
                get { return DBSettings.Get("Users_NewMessageNotification", true); }
                set { DBSettings.Set("Users_NewMessageNotification", value); }
            }

            [Reflection.DescriptionAttribute("Notify user of new events (while logged on)")]
            [Reflection.HintAttribute("Check to enable real-time notification for new events.")]
            public static bool NewEventNotification
            {
                get { return DBSettings.Get("Users_NewEventNotification", true); }
                set { DBSettings.Set("Users_NewEventNotification", value); }
            }

            [Reflection.DescriptionAttribute("Free For Females")]
            [Reflection.HintAttribute("Check to enable free-for-females mode.")]
            public static bool FreeForFemales
            {
                get { return DBSettings.Get("Users_FreeForFemales", false); }
                set { DBSettings.Set("Users_FreeForFemales", value); }
            }

            [Reflection.DescriptionAttribute("Enable Stealth Mode")]
            [Reflection.HintAttribute("This option provides users with the ability to log on in stealth mode")]
            public static bool ShowStealthMode
            {
                get { return DBSettings.Get("Users_ShowStealthMode", false); }
                set { DBSettings.Set("Users_ShowStealthMode", value); }
            }

            [Reflection.DescriptionAttribute("Enable welcome message")]
            [Reflection.HintAttribute("If it is checked every new registered user will receive a welcome message.")]
            public static bool SendWelcomeMessage
            {
                get { return DBSettings.Get("Users_SendWelcomeMessage", true); }
                set { DBSettings.Set("Users_SendWelcomeMessage", value); }
            }

            [Reflection.DescriptionAttribute("Show friends new photos on home page")]
            [Reflection.HintAttribute("If it is checked the user can see new photots of his firends on home page.")]
            public static bool ShowFriendsNewPhotosOnHomePage
            {
                get { return DBSettings.Get("Users_ShowFriendsNewPhotosOnHomePage", false); }
                set { DBSettings.Set("Users_ShowFriendsNewPhotosOnHomePage", value); }
            }

            [Reflection.DescriptionAttribute("Show friends new blog posts on home page")]
            [Reflection.HintAttribute("If it is checked the user can see new blog posts of his firends on home page.")]
            public static bool ShowFriendsNewBlogPostsOnHomePage
            {
                get { return DBSettings.Get("Users_ShowFriendsNewBlogPosts", false); }
                set { DBSettings.Set("Users_ShowFriendsNewBlogPosts", value); }
            }

            [Reflection.DescriptionAttribute("Require user photo to show in new users")]
            [Reflection.HintAttribute("If it is checked only users with photos will be shown in the new users box.")]
            public static bool RequirePhotoToShowInNewUsers
            {
                get { return DBSettings.Get("Users_RequirePhotoToShowInNewUsers", true); }
                set { DBSettings.Set("Users_RequirePhotoToShowInNewUsers", value); }
            }

            [Reflection.DescriptionAttribute("Require user profile to show in new users")]
            [Reflection.HintAttribute("If it is checked only users with profiles will be shown in the new users box.")]
            public static bool RequireProfileToShowInNewUsers
            {
                get { return DBSettings.Get("Users_RequireProfileToShowInNewUsers", false); }
                set { DBSettings.Set("Users_RequireProfileToShowInNewUsers", value); }
            }

            [Reflection.DescriptionAttribute("Require user profile to show in searches")]
            [Reflection.HintAttribute("If it is checked only users with profiles will be shown in searches.")]
            public static bool RequireProfileToShowInSearch
            {
                get { return DBSettings.Get("Users_RequireProfileToShowInSearch", false); }
                set { DBSettings.Set("Users_RequireProfileToShowInSearch", value); }
            }

            [Reflection.DescriptionAttribute("Flash uploads")]
            [Reflection.HintAttribute("Check to enable flash file uploader (English only)")]
            public static bool EnableFlashUploads
            {
                get { return DBSettings.Get("Users_EnableFlashUploads", false); }
                set { DBSettings.Set("Users_EnableFlashUploads", value); }
            }

            [Reflection.DescriptionAttribute("Silverlight uploads")]
            [Reflection.HintAttribute("Check to enable silverlight file uploader")]
            public static bool EnableSilverlightUploads
            {
                get { return DBSettings.Get("Users_EnableSilverlightUploads", false); }
                set { DBSettings.Set("Users_EnableSilverlightUploads", value); }
            }

            [Reflection.DescriptionAttribute("Webcam photo capture")]
            [Reflection.HintAttribute("Check to enable webcam photo capture")]
            public static bool EnableWebcamPhotoCapture
            {
                get { return DBSettings.Get("Users_EnableWebcamPhotoCapture", false); }
                set { DBSettings.Set("Users_EnableWebcamPhotoCapture", value); }
            }

            [Reflection.DescriptionAttribute("Enable profile skins")]
            [Reflection.HintAttribute("Check to enable profile skins.")]
            public static bool EnableProfileSkins
            {
                get { return DBSettings.Get("Users_EnableProfileSkins", true); }
                set { DBSettings.Set("Users_EnableProfileSkins", value); }
            }

            [Reflection.DescriptionAttribute("Enable user status")]
            [Reflection.HintAttribute("Check to enable user status.")]
            public static bool EnableUserStatusText
            {
                get { return DBSettings.Get("Users_EnableUserStatusText", true); }
                set { DBSettings.Set("Users_EnableUserStatusText", value); }
            }

            [Reflection.DescriptionAttribute("Show match percentage")]
            [Reflection.HintAttribute("Check to show match percentage.")]
            public static bool ShowMatchPercentage
            {
                get { return DBSettings.Get("Users_ShowMatchPercentage", false); }
                set { DBSettings.Set("Users_ShowMatchPercentage", value); }
            }

            [Reflection.DescriptionAttribute("Show featured members on the home page")]
            [Reflection.HintAttribute("Check to show featured members on the home page.")]
            public static bool ShowFeaturedMemberOnHomePage
            {
                get { return DBSettings.Get("Users_ShowFeaturedMemberOnHomePage", false); }
                set { DBSettings.Set("Users_ShowFeaturedMemberOnHomePage", value); }
            }

            [Reflection.DescriptionAttribute("Enable relationship status")]
            [Reflection.HintAttribute("Check to enable relationship status.")]
            public static bool EnableRelationshipStatus
            {
                get { return DBSettings.Get("Users_EnableRelationshipStatus", true); }
                set { DBSettings.Set("Users_EnableRelationshipStatus", value); }
            }

            [Reflection.DescriptionAttribute("Disable gender information")]
            [Reflection.HintAttribute("Check to disable gender information.")]
            public static bool DisableGenderInformation
            {
                get { return DBSettings.Get("Users_DisableGenderInformation", false); }
                set { DBSettings.Set("Users_DisableGenderInformation", value); }
            }

            [Reflection.DescriptionAttribute("Disable age information")]
            [Reflection.HintAttribute("Check to disable age information.")]
            public static bool DisableAgeInformation
            {
                get { return DBSettings.Get("Users_DisableAgeInformation", false); }
                set { DBSettings.Set("Users_DisableAgeInformation", value); }
            }

            [Reflection.DescriptionAttribute("Enable user events page")]
            [Reflection.HintAttribute("Check to allow members to view events page.")]
            public static bool EnableUserEventsPage
            {
                get { return DBSettings.Get("Users_EnableUserEventsPage", true); }
                set { DBSettings.Set("Users_EnableUserEventsPage", value); }
            }

            [Reflection.DescriptionAttribute("Enable event comments")]
            [Reflection.HintAttribute("Check to allow members to add event comments.")]
            public static bool EnableEventComments
            {
                get { return DBSettings.Get("Users_EnableEventComments", true); }
                set { DBSettings.Set("Users_EnableEventComments", value); }
            }

            [Reflection.DescriptionAttribute("Redirect after logout URL")]
            [Reflection.HintAttribute("Specify URL where users will be redirected after logout")]
            public static string RedirectAfterLogout
            {
                get { return DBSettings.Get("Users_RedirectAfterLogout", String.Empty); }
                set { DBSettings.Set("Users_RedirectAfterLogout", value); }
            }

            private static object countriesByCodeLock = new object();
            public static string GetCountryByCode(string code)
            {
                lock (countriesByCodeLock)
                {
                    string cacheKey = String.Format("Config_GetCountryByCode_{0}", code);
                    if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                    {
                        return HttpContext.Current.Cache[cacheKey] as string;
                    }

                    var country = (from el in xdCountries.Descendants("Country")
                                   let c = el.Attribute("Code")
                                   let name = el.Attribute("Name")
                                   where c != null && c.Value == code
                                   select name.Value).FirstOrDefault();

                    if (HttpContext.Current != null && country != null)
                    {
                        HttpContext.Current.Cache.Add(cacheKey, country, null, Cache.NoAbsoluteExpiration,
                                                      TimeSpan.FromHours(1), CacheItemPriority.High, null);
                    }

                    return country;
                }
            }

            private static object countriesLock = new object();
            public static string[] GetCountries()
            {
                lock (countriesLock)
                {
                    string cacheKey = "Config_GetCountries";
                    if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                    {
                        return HttpContext.Current.Cache[cacheKey] as string[];
                    }

                    var countries = (from c in xdCountries.Descendants("Country")
                                     let name = c.Attribute("Name")
                                     where name != null && name.Value.Length > 0
                                     orderby (string)name
                                     select (string)name).ToArray();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Add(cacheKey, countries, null, Cache.NoAbsoluteExpiration,
                                                      TimeSpan.FromHours(1), CacheItemPriority.High, null);
                    }

                    return countries;
                }
            }

            private static object regionsLock = new object();
            public static string[] GetRegions(string country)
            {
                lock (regionsLock)
                {
                    string cacheKey = String.Format("Config_GetRegions_{0}", country);
                    if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                    {
                        return HttpContext.Current.Cache[cacheKey] as string[];
                    }

                    var regions = (from c in xdCountries.Descendants("Country")
                                   where (string)c.Attribute("Name") == country
                                   select c).Elements("Region").Attributes("Name").
                        Select(r => r.Value).OrderBy(r => r).ToArray();

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Add(cacheKey, regions, null, Cache.NoAbsoluteExpiration,
                                                      TimeSpan.FromMinutes(10), CacheItemPriority.High, null);
                    }

                    return regions;
                }
            }

            public static string[] GetCities(string country, string region)
            {
                var cntry = (from c in xdCountries.Descendants("Country")
                             where (string)c.Attribute("Name") == country
                             select c);


                var cities = (from r in cntry.Elements("Region")
                              where (string)r.Attribute("Name") == region
                              select r).Elements("City").Attributes("Name").
                        Select(r => r.Value).OrderBy(r => r).ToArray();

                return cities;
            }

            public static string MapFacebookCityToLoveHitchCity(HttpContext context, string fbCity, out string region, out string country)
            {
                region = null;
                country = null;
                if (fbCity.IsNullOrEmpty()) return null;
                try
                {
                    string[] tokens = fbCity.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    string cityName = tokens[0].Trim();
                    string countryName = tokens.Length > 1 ? tokens[tokens.Length - 1].Trim() : String.Empty;

                    var cntry = (from c in xdCountries.Descendants("Country")
                                 where (string)c.Attribute("Name") == countryName
                                 select c).FirstOrDefault();


                    if (cntry != null)
                    {
                        country = countryName;
                        var rgns = (from r in cntry.Elements("Region")
                                    select r).ToList();
                        foreach (var rgn in rgns)
                        {
                            var city = (from c in rgn.Elements("City")
                                        where (string)c.Attribute("Name") == cityName
                                        select c).FirstOrDefault();
                            if (city != null)
                            {
                                region = (string)rgn.Attribute("Name");
                                return cityName;
                            }
                        }
                        foreach (var rgn in rgns)
                        {
                            var city = (from c in rgn.Elements("City")
                                        where ((string)c.Attribute("Name")).StartsWith(cityName)
                                        select c).FirstOrDefault();
                            if (city != null)
                            {
                                region = (string)rgn.Attribute("Name");
                                return (string)city.Attribute("Name");
                            }
                        }
                    }
                    else
                    {
                        var countries = (from c in xdCountries.Descendants("Country")
                                         where ((string)c.Attribute("Name")).StartsWith(countryName) || ((string)c.Attribute("Name")).Contains(countryName)
                                         select c).ToList();

                        foreach (var ctry in countries)
                        {
                            var rgns = (from r in ctry.Elements("Region")
                                        select r).ToList();

                            foreach (var rgn in rgns)
                            {
                                var city = (from c in rgn.Elements("City")
                                            where ((string)c.Attribute("Name")).Equals(cityName)
                                            select c).FirstOrDefault();
                                if (city != null)
                                {
                                    region = (string)rgn.Attribute("Name");
                                    country = (string)ctry.Attribute("Name");
                                    return cityName;
                                }
                            }
                            foreach (var rgn in rgns)
                            {
                                var city = (from c in rgn.Elements("City")
                                            where ((string)c.Attribute("Name")).StartsWith(cityName)
                                            select c).FirstOrDefault();
                                if (city != null)
                                {
                                    region = (string)rgn.Attribute("Name");
                                    country = (string)ctry.Attribute("Name");
                                    return (string)city.Attribute("Name");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex) { }
                return null;
            }

            public static string[] GetUsersWithinRadius(Location inLocation, bool? hasAnswer, User.eGender gender,
                                                        int maxAge,
                                                        int minAge, bool photoReq, double inRadius,
                                                        int maxResults)
            {
                if (null == inLocation)
                    throw new ArgumentNullException("inLocation", "Null location passed in.");
                if (inRadius <= 0.0)
                    throw new ArgumentOutOfRangeException("inRadius", inRadius, "Invalid value for radius passed in.");

                if (Double.MinValue == inLocation.Latitude)
                    throw new ArgumentException("inLocation.Latitude",
                                                string.Format(
                                                    "The database does not contain latitude information for {0}, {1}.",
                                                    inLocation.City, inLocation.State));
                if (Double.MinValue == inLocation.Longitude)
                    throw new ArgumentException("inLocation.Longitude",
                                                string.Format(
                                                    "The database does not contain longitude information for {0}, {1}.",
                                                    inLocation.City, inLocation.State));

                RadiusBox radBox = RadiusBox.Create(inLocation, inRadius);


                using (SqlConnection conn = DB.Open())
                {
                    SqlDataReader reader = (SqlDataReader)
                        SqlHelper.GetDB().ExecuteReader(
                                                "FetchUsersInRadius",
                                                hasAnswer,
                                                (int)gender,
                                                DateTime.Now.Subtract(TimeSpan.FromDays(maxAge * 365.25)),
                                                DateTime.Now.Subtract(TimeSpan.FromDays(minAge * 365.25)),
                                                photoReq,
                                                radBox.BottomLine,
                                                radBox.TopLine,
                                                radBox.LeftLine,
                                                radBox.RightLine,
                                                inLocation.Longitude,
                                                inLocation.Latitude,
                                                radBox.Radius,
                                                maxResults);

                    var lResults = new List<string>();

                    while (reader.Read())
                    {
                        var username = (string)reader["Username"];

                        //if (reader["Longitude"] is double && reader["Latitude"] is double)
                        //{
                        //    Location loc = new Location();

                        //    double distance = Distance.GetDistance(inLocation, loc);
                        //    if (distance <= radBox.Radius)
                        lResults.Add(username);
                        //}
                    }

                    return lResults.ToArray();
                }
            }

            public static Location GetLocation(string country, string region, string city)
            {
                var cntry = (from c in xdCountries.Descendants("Country")
                             where (string)c.Attribute("Name") == country
                             select c);

                var rgn = (from r in cntry.Elements("Region")
                           where (string)r.Attribute("Name") == region.Trim()
                           select r);

                var loc = (from c in rgn.Elements("City")
                           where (string)c.Attribute("Name") == city
                           select new Location
                                      {
                                          Longitude = ConvertLongitudeLatitudeToDouble((string)c.Attribute("Longitude")),
                                          Latitude = ConvertLongitudeLatitudeToDouble((string)c.Attribute("Latitude"))
                                      }).FirstOrDefault();

                if (loc == null || loc.Latitude == Double.MinValue || loc.Longitude == Double.MinValue)
                    return null;

                return loc;

                //foreach (Countries.CountryRow countryRow in dsCountries.Country.Select(
                //    String.Format("Name = '{0}'", country)))
                //{
                //    foreach (Countries.RegionRow regionRow in dscountries.Region.Select(
                //        String.Format("Country_Id = '{0}' AND Name = '{1}'", countryRow.Country_Id, region)))
                //    {
                //        foreach (Countries.CityRow cityRow in dscountries.City.Select(
                //            String.Format("Region_Id = '{0}' AND Name = '{1}'", regionRow.Region_Id, city)))
                //        {
                //            var loc = new Location
                //                          {
                //                              Longitude = ConvertLongitudeLatitudeToDouble(cityRow.Longitude),
                //                              Latitude = ConvertLongitudeLatitudeToDouble(cityRow.Latitude)
                //                          };

                //            if (loc.Latitude == Double.MinValue || loc.Longitude == Double.MinValue)
                //                return null;
                //            return loc;
                //        }
                //    }
                //}

                //return null;
            }

            public static Location GetLocation(User user)
            {
                if (user != null && user.Longitude.HasValue && user.Latitude.HasValue)
                {
                    var loc = new Location { Longitude = user.Longitude.Value, Latitude = user.Latitude.Value };
                    return loc;
                }
                return null;
            }

            private static double ConvertLongitudeLatitudeToDouble(string longlat)
            {
                double result = Double.MinValue;

                if (longlat.Length == 0)
                    return result;

                if (!longlat.Contains("."))
                {
                    char ch = longlat[longlat.Length - 1];

                    switch (Char.ToUpper(ch, CultureInfo.InvariantCulture))
                    {
                        case 'N':
                        case 'E':
                            longlat = longlat.Substring(0, longlat.Length - 1);
                            longlat = '+' + longlat.Substring(0, longlat.Length - 2) + '.' +
                                      longlat.Substring(longlat.Length - 2, 2);
                            break;
                        case 'W':
                        case 'S':
                            longlat = longlat.Substring(0, longlat.Length - 1);
                            longlat = '-' + longlat.Substring(0, longlat.Length - 2) + '.' +
                                      longlat.Substring(longlat.Length - 2, 2);
                            break;
                    }
                }

                Double.TryParse(longlat, NumberStyles.Float, CultureInfo.InvariantCulture, out result);

                return result;
            }

            public static BillingPlanOptions GetNonPayingMembersOptions()
            {
                return
                    Data.Misc.FromXml<BillingPlanOptions>(DBSettings.Get("Users_NonPayingMembersOptions",
                                                                            DefaultNonPayingMembersOptions));
            }

            public static void SetNonPayingMembersOptions(BillingPlanOptions options)
            {
                DBSettings.Set("Users_NonPayingMembersOptions", Data.Misc.ToXml(options));
            }
        }

        #endregion

        #region Nested type: UserScores

        [Reflection.DescriptionAttribute("User Scores and Levels")]
        public class UserScores
        {
            [Reflection.DescriptionAttribute("Enable user levels")]
            public static bool EnableUserLevels
            {
                get { return DBSettings.Get("UserScores_EnableUserLevels", false); }
                set { DBSettings.Set("UserScores_EnableUserLevels", value); }
            }

            [Reflection.DescriptionAttribute("Show level icons")]
            public static bool ShowLevelIcons
            {
                get { return DBSettings.Get("UserScores_ShowLevelIcons", false); }
                set { DBSettings.Set("UserScores_ShowLevelIcons", value); }
            }

            [Reflection.DescriptionAttribute("Score for login (once per day)")]
            public static int DailyLogin
            {
                get { return DBSettings.Get("UserScores_DailyLogin", 20); }
                set { DBSettings.Set("UserScores_DailyLogin", value); }
            }

            [Reflection.DescriptionAttribute("Score for viewing profile")]
            public static int ViewingProfile
            {
                get { return DBSettings.Get("UserScores_ViewingProfile", 1); }
                set { DBSettings.Set("UserScores_ViewingProfile", value); }
            }

            [Reflection.DescriptionAttribute("Score for viewed profile")]
            public static int ViewedProfile
            {
                get { return DBSettings.Get("UserScores_ViewedProfile", 1); }
                set { DBSettings.Set("UserScores_ViewedProfile", value); }
            }

            [Reflection.DescriptionAttribute("Score for left comment")]
            public static int LeftComment
            {
                get { return DBSettings.Get("UserScores_LeftComment", 1); }
                set { DBSettings.Set("UserScores_LeftComment", value); }
            }

            [Reflection.DescriptionAttribute("Score for received comment")]
            public static int ReceivedComment
            {
                get { return DBSettings.Get("UserScores_ReceivedComment", 1); }
                set { DBSettings.Set("UserScores_ReceivedComment", value); }
            }

            [Reflection.DescriptionAttribute("Score for received message")]
            public static int ReceivedMessage
            {
                get { return DBSettings.Get("UserScores_ReceivedMessage", 2); }
                set { DBSettings.Set("UserScores_ReceivedMessage", value); }
            }

            [Reflection.DescriptionAttribute("Score for sent message")]
            public static int SentMessage
            {
                get { return DBSettings.Get("UserScores_SentMessage", 3); }
                set { DBSettings.Set("UserScores_SentMessage", value); }
            }

            [Reflection.DescriptionAttribute("Score for sent wink")]
            public static int SentWink
            {
                get { return DBSettings.Get("UserScores_SentWink", 1); }
                set { DBSettings.Set("UserScores_SentWink", value); }
            }

            [Reflection.DescriptionAttribute("Score for sent gift")]
            public static int SentGift
            {
                get { return DBSettings.Get("UserScores_SentGift", 5); }
                set { DBSettings.Set("UserScores_SentGift", value); }
            }

            [Reflection.DescriptionAttribute("Score for reply to message")]
            public static int RepliedToMessage
            {
                get { return DBSettings.Get("UserScores_RepliedToMessage", 4); }
                set { DBSettings.Set("UserScores_RepliedToMessage", value); }
            }

            [Reflection.DescriptionAttribute("Score for approved photo")]
            public static int ApprovedPhoto
            {
                get { return DBSettings.Get("UserScores_ApprovedPhoto", 10); }
                set { DBSettings.Set("UserScores_ApprovedPhoto", value); }
            }

            [Reflection.DescriptionAttribute("Score for rejected photo")]
            public static int RejectedPhoto
            {
                get { return DBSettings.Get("UserScores_RejectedPhoto", -50); }
                set { DBSettings.Set("UserScores_RejectedPhoto", value); }
            }

            [Reflection.DescriptionAttribute("Score for approved video")]
            public static int ApprovedVideo
            {
                get { return DBSettings.Get("UserScores_ApprovedVideo", 20); }
                set { DBSettings.Set("UserScores_ApprovedVideo", value); }
            }

            [Reflection.DescriptionAttribute("Score for rejected video")]
            public static int RejectedVideo
            {
                get { return DBSettings.Get("UserScores_RejectedVideo", -50); }
                set { DBSettings.Set("UserScores_RejectedVideo", value); }
            }

            [Reflection.DescriptionAttribute("Score for approved audio")]
            public static int ApprovedAudio
            {
                get { return DBSettings.Get("UserScores_ApprovedAudio", 20); }
                set { DBSettings.Set("UserScores_ApprovedAudio", value); }
            }

            [Reflection.DescriptionAttribute("Score for rejected audio")]
            public static int RejectedAudio
            {
                get { return DBSettings.Get("UserScores_RejectedAudio", -50); }
                set { DBSettings.Set("UserScores_RejectedAudio", value); }
            }

            [Reflection.DescriptionAttribute("Score for new topic")]
            public static int NewTopic
            {
                get { return DBSettings.Get("UserScores_NewTopic", 5); }
                set { DBSettings.Set("UserScores_NewTopic", value); }
            }

            [Reflection.DescriptionAttribute("Score for new post")]
            public static int NewPost
            {
                get { return DBSettings.Get("UserScores_NewPost", 5); }
                set { DBSettings.Set("UserScores_NewPost", value); }
            }

            [Reflection.DescriptionAttribute("Score for new post in users topic")]
            public static int NewPostsOnUserTopic
            {
                get { return DBSettings.Get("UserScores_NewPostsOnUserTopic", 1); }
                set { DBSettings.Set("UserScores_NewPostsOnUserTopic", value); }
            }

            [Reflection.DescriptionAttribute("Score for deleted topic")]
            public static int DeletedTopic
            {
                get { return DBSettings.Get("UserScores_DeletedTopic", -100); }
                set { DBSettings.Set("UserScores_DeletedTopic", value); }
            }

            [Reflection.DescriptionAttribute("Score for deleted post")]
            public static int DeletedPost
            {
                get { return DBSettings.Get("UserScores_DeletedPost", -50); }
                set { DBSettings.Set("UserScores_DeletedPost", value); }
            }

            [Reflection.DescriptionAttribute("Score for new registered member invited by ")]
            public static int InvitedBy
            {
                get { return DBSettings.Get("UserScores_InvitedBy", 500); }
                set { DBSettings.Set("UserScores_InvitedBy", value); }
            }

            [Reflection.DescriptionAttribute("Trigger counter for new registered member invited by user")]
            public static int InvitedByCounter
            {
                get { return DBSettings.Get("UserScores_InvitedByCounter", 10); }
                set { DBSettings.Set("UserScores_InvitedByCounter", value); }
            }
        }

        #endregion

        #region Nested type: WebParts

        public class WebParts
        {
            public static WebPartInfo[] CurrentContextAllParts
            {
                get
                {
                    return MatchmakerHelper.IsMatchmakerState
                            ? AllMatchmakerParts
                            : AllSinglsParts;
                }
            }

            public static WebPartInfo[] AllMatchmakerParts = new WebPartInfo[]
            {
                new WebPartInfo()
                {
                    Name = "My Profile",
                    Description = "This component shows your profile details.",
                    ControlPath = "~/Components/WebParts/MyProfileWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "My Single Male Friends",
                    Description = "This component shows a list of your male single friends with photos.",
                    ControlPath = "~/Components/WebParts/MaleFriendsWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "My Single Female Friends",
                    Description = "This component shows a list of your female single friends with photos.",
                    ControlPath = "~/Components/WebParts/FemaleFriendsWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Matchmaking In Process",
                    Description = "This component shows your matchmaking in process.",
                    ControlPath = "~/Components/WebParts/MatchmakingInProcessWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Top Voted Users",
                    Description = "This component shows a list of top voted users. You can filter by gender.",
                    ControlPath = "~/Components/WebParts/TopUsersWebPart.ascx",
                    RequirementsMet = () => Config.Ratings.EnableProfileVoting,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "Upcoming Events",
                    Description = "This component shows all upcoming events in the groups you are member of.",
                    ControlPath = "~/Components/WebParts/UpcomingEventsWebPart.ascx",
                    RequirementsMet = () => Config.Groups.EnableGroups,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "User Events",
                    Description = "This component shows a list of user events. You can specify the number of events and choose for which type of events you will be informed.",
                    ControlPath = "~/Components/WebParts/UserEventsWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "New Topics",
                    Description = "This component shows all new topics in the groups you are member of.",
                    ControlPath = "~/Components/WebParts/NewTopicsWebPart.ascx",
                    RequirementsMet = () => Config.Groups.EnableGroups,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "New Videos",
                    Description = "This component shows the new uploaded and approved user videos. You can filter by gender.",
                    ControlPath = "~/Components/WebParts/NewVideosWebPart.ascx",
                    RequirementsMet = () => false,//Config.Misc.EnableVideoUpload,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "Popular Blog Posts",
                    Description = "This component shows the most popular blog posts for the last month.",
                    ControlPath = "~/Components/WebParts/PopularBlogPostsWebPart.ascx",
                    RequirementsMet = () => false,//Config.Misc.EnableBlogs,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "New Groups",
                    Description = "This component shows the latest created and approved groups in the site.",
                    ControlPath = "~/Components/WebParts/NewGroupsWebPart.ascx",
                    RequirementsMet = () => Config.Groups.EnableGroups,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Start to Pair",
                    Description = "This component provides a handy quick search box.",
                    ControlPath = "~/Components/WebParts/QuickPairSearchWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Birthdays",
                    Description = "This component shows a list of users who have birthdays today or within the next few days.",
                    ControlPath = "~/Components/WebParts/BirthdayBoxWebPart.ascx",
                    RequirementsMet = () => !Config.Users.DisableAgeInformation,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Friends Online",
                    Description = "This component shows all your friends who are currently online.",
                    ControlPath = "~/Components/WebParts/FriendsOnlineBoxWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Polls",
                    Description = "This component displays the current site polls",
                    ControlPath = "~/Components/WebParts/PollWebPart.ascx",
                    RequirementsMet = () => false,//Config.Misc.ShowPolls,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "News",
                    Description = "This component shows the latest site news.",
                    ControlPath = "~/Components/WebParts/NewsBoxWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                }
            };
            public static WebPartInfo[] AllSinglsParts = new WebPartInfo[]
            {
                new WebPartInfo()
                {
                    Name = "My Profile",
                    Description = "This component shows your profile details.",
                    ControlPath = "~/Components/WebParts/MyProfileWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "New Users",
                    Description = "This component shows a list of new users with photos. You can filter by gender and age.",
                    ControlPath = "~/Components/WebParts/NewUsersWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Top Voted Users",
                    Description = "This component shows a list of top voted users. You can filter by gender.",
                    ControlPath = "~/Components/WebParts/TopUsersWebPart.ascx",
                    RequirementsMet = () => Config.Ratings.EnableProfileVoting,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "Upcoming events",
                    Description = "This component shows all upcoming events in the groups you are member of.",
                    ControlPath = "~/Components/WebParts/UpcomingEventsWebPart.ascx",
                    RequirementsMet = () => Config.Groups.EnableGroups,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "User Events",
                    Description = "This component shows a list of user events. You can specify the number of events and choose for which type of events you will be informed.",
                    ControlPath = "~/Components/WebParts/UserEventsWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "New Topics",
                    Description = "This component shows all new topics in the groups you are member of.",
                    ControlPath = "~/Components/WebParts/NewTopicsWebPart.ascx",
                    RequirementsMet = () => Config.Groups.EnableGroups,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "New Videos",
                    Description = "This component shows the new uploaded and approved user videos. You can filter by gender.",
                    ControlPath = "~/Components/WebParts/NewVideosWebPart.ascx",
                    RequirementsMet = () => false,//Config.Misc.EnableVideoUpload,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "Popular Blog Posts",
                    Description = "This component shows the most popular blog posts for the last month.",
                    ControlPath = "~/Components/WebParts/PopularBlogPostsWebPart.ascx",
                    RequirementsMet = () => false,//Config.Misc.EnableBlogs,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "New Groups",
                    Description = "This component shows the latest created and approved groups in the site.",
                    ControlPath = "~/Components/WebParts/NewGroupsWebPart.ascx",
                    RequirementsMet = () => Config.Groups.EnableGroups,
                    Zone = WebPartZone.HomePageRightZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Quick Search",
                    Description = "This component provides a handy quick search box.",
                    ControlPath = "~/Components/WebParts/SearchBoxWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Birthdays",
                    Description = "This component shows a list of users who have birthdays today or within the next few days.",
                    ControlPath = "~/Components/WebParts/BirthdayBoxWebPart.ascx",
                    RequirementsMet = () => !Config.Users.DisableAgeInformation,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Friends Online",
                    Description = "This component shows all your friends who are currently online.",
                    ControlPath = "~/Components/WebParts/FriendsOnlineBoxWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                },
                new WebPartInfo()
                {
                    Name = "Polls",
                    Description = "This component displays the current site polls",
                    ControlPath = "~/Components/WebParts/PollWebPart.ascx",
                    RequirementsMet = () => false,//Config.Misc.ShowPolls,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = false
                },
                new WebPartInfo()
                {
                    Name = "News",
                    Description = "This component shows the latest site news.",
                    ControlPath = "~/Components/WebParts/NewsBoxWebPart.ascx",
                    RequirementsMet = () => true,
                    Zone = WebPartZone.HomePageLeftZone,
                    IsVisibleByDefault = true
                }
            };

            public static WebPartInfo[] GetAvailableWebParts(WebPartZone zone, bool? visibleByDefault)
            {
                return
                    Array.FindAll(CurrentContextAllParts,
                                  p =>
                                  p.Zone == zone && p.RequirementsMet() &&
                                  (!visibleByDefault.HasValue || p.IsVisibleByDefault == visibleByDefault));
            }

            public static WebPartInfo[] GetAvailableWebParts(WebPartZone zone)
            {
                return GetAvailableWebParts(zone, null);
            }
        }
        #endregion

        #region Nested class:Gift
        public class Gift
        {
            public static int GridColumnCount { get { return 5; } }
            public static int GridRowCount { get { return 3; } }
            public static int GiftsPerPage { get { return GridColumnCount * GridRowCount; } }
        }
        #endregion
    }
}