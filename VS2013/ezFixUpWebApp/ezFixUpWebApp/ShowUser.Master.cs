using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class ShowUser : MasterPage
    {
        private UserSession CurrentUserSession
        {
            get { return Page.CurrentUserSession; }
        }

        private User _viewedUser;

        public User ViewedUser
        {
            set
            {
                _viewedUser = value;
                ViewState["ShowUser_ViewedUsername"] = value.Username;
                Page.LastViewedUsername = value.Username;
                Page.LastViewedName = value.Name;
            }
            get
            {
                return _viewedUser ?? Classes.User.Load((string)ViewState["ShowUser_ViewedUsername"]);
            }
        }

        //private bool IMLocked
        //{
        //    get
        //    {
        //        return Config.Credits.Required && Config.Credits.CreditsForIM > 0 &&
        //            (CurrentUserSession == null ||
        //            (Config.Credits.Required && CurrentUserSession.Username != ViewedUser.Username &&
        //             !(Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female) &&
        //             !UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM, null)));
        //    }
        //}

        protected string StrTargetUserID
        {
            get { return ViewedUser.Username; }
        }

        protected string StrUserID
        {
            get { return CurrentUserSession.Username; }
        }

        private new PageBase Page
        {
            get { return base.Page as PageBase; }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            var myHtmlLink = new HtmlLink { Href = "Styles/Site.css" };
            myHtmlLink.Attributes.Add("rel", "stylesheet");
            myHtmlLink.Attributes.Add("type", "text/css");
            Page.Header.Controls.Add(myHtmlLink);
            //myHtmlLink.Href = "~/scripts/fmt_vmenu/css/fmtstyle.css";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MatchToFriend1.IsVisible = MatchmakerHelper.IsMatchmakerState;

            if (CurrentUserSession == null && Config.Users.RegistrationRequiredToBrowse)
                Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Request.Url.PathAndQuery.Substring(1));
            //Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.PathAndQuery.Substring(1)));

            if (Config.Users.CompletedProfileRequiredToBrowseSearch &&
                CurrentUserSession != null && !CurrentUserSession.HasProfile)
                Response.Redirect("ManageProfile.aspx?err=profnotcompl");

            if (!Page.IsPostBack)
            {
                LoadStrings();

                if (Request.Params["uid"] != null)
                {
                    try
                    {
                        User user = Classes.User.Load(Request.Params["uid"]);

                        if (user.Deleted)
                        {
                            if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                                Page.StatusPageMessage = "This user has been deleted!".Translate();
                            else
                                Page.StatusPageMessage =
                                    String.Format(
                                        "This user has been deleted for the following reason:<br><br>{0}".Translate(),
                                        user.DeleteReason);
                            Response.Redirect(MatchmakerHelper.CurrentHomePage);
                        }

                        ViewedUser = user;

                        if (Page is ShowUserPage && CurrentUserSession != null)
                        {
                            if (StrTargetUserID == StrUserID)
                                Response.Redirect("ManageProfile.aspx");

                            // Save profile view
                            if (!CurrentUserSession.IsOptionEnabled(eUserOptions.DisableProfileViews))
                            {
                                Classes.User.SaveProfileView(
                                    CurrentUserSession.Username, ViewedUser.Username);

                                Classes.User.AddScore(CurrentUserSession.Username,
                                              Config.UserScores.ViewingProfile, "ViewingProfile");
                                Classes.User.AddScore(ViewedUser.Username,
                                              Config.UserScores.ViewedProfile, "ViewedProfile");

                                if (Config.Users.NewEventNotification &&
                                    CurrentUserSession.Username != ViewedUser.Username
                                    && (ViewedUser.IsOnline() || Classes.User.IsUsingNotifier(ViewedUser.Username)))
                                {
                                    var text = String.Format("User {0} is viewing your profile!".Translate(),
                                                             "<b>" + CurrentUserSession.Username + "</b>");
                                    int imageID;
                                    try
                                    {
                                        imageID = CurrentUserSession.GetPrimaryPhoto().Id;
                                    }
                                    catch (NotFoundException)
                                    {
                                        imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                                    }
                                    var thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                    var notification = new GenericEventNotification
                                                           {
                                                               Recipient = ViewedUser.Username,
                                                               Sender = CurrentUserSession.Username,
                                                               Text = text,
                                                               ThumbnailUrl = thumbnailUrl,
                                                               RedirectUrl =
                                                                   UrlRewrite.CreateShowUserUrl(
                                                                       CurrentUserSession.Username)
                                                           };
                                    RealtimeNotification.SendNotification(notification);
                                }
                            }
                        }

                        #region show/hide IM link

                        string reason;
                        if (Config.Misc.EnableIntegratedIM &&
                            CurrentUserSession != null &&
                            !CurrentUserSession.StealthMode &&
                            ViewedUser.IsOnline() &&
                            CurrentUserSession.Username != ViewedUser.Username &&
                            Classes.User.CanSendMessage(CurrentUserSession, ViewedUser, out reason))
                        {
                            pnlInstantMessenger.Visible = true;

                            var permissionCheckResult = CurrentUserSession.CanIM();

                            if (permissionCheckResult == PermissionCheckResult.No)
                                pnlInstantMessenger.Visible = false;
                            else
                            {
                                string root = HttpRuntime.AppDomainAppVirtualPath.TrimEnd('/');
                                var sectionUnlocked = UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, ViewedUser.Username, UnlockedSection.SectionType.IM, null);
                                if (permissionCheckResult == PermissionCheckResult.Yes || sectionUnlocked)
                                {
                                    lnkInstantMessenger.HRef = "#";
                                    lnkInstantMessenger.Attributes.Add("onclick",
                                                                       String.Format(
                                                                           "window.open('AjaxChat/MessengerWindow.aspx?init=1&target={0}', 'ajaxim_{1}_{0}', 'width=650,height=400,resizable=1,menubar=0,status=0,toolbar=0'); return false;",
                                                                           ViewedUser.Username,
                                                                           CurrentUserSession.Username));
                                    lnkInstantMessenger.Target = "AjaxIM_" + ViewedUser.Username;
                                }
                                else if (permissionCheckResult == PermissionCheckResult.YesWithCredits)
                                {
                                    string url = String.Format(
                                        "if (confirm('{4}')) window.open('{0}/LaunchIM.aspx?targetUsername={1}', 'ajaxim_{2}_{3}', 'width=650,height=400,resizable=1,menubar=0,status=0,toolbar=0'); return false;",
                                        root, StrTargetUserID,
                                        Regex.Replace(StrUserID, @"[^A-Za-z0-9]", "_"),
                                        Regex.Replace(StrTargetUserID, @"[^A-Za-z0-9]", "_"),
                                        String.Format(Lang.Trans("Opening the chat session will subtract {0} credits from your balance."), CurrentUserSession.BillingPlanOptions.CanIM.Credits /*Config.Credits.CreditsForIM*/));

                                    lnkInstantMessenger.Attributes.Add("onclick", url);
                                    lnkInstantMessenger.Attributes.Add("href", "");
                                }
                                else if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                                {
                                    lnkInstantMessenger.Visible = false;
                                    lnkInstantMessengerPay.Visible = true;
                                }
                            }
                        }
                        else pnlInstantMessenger.Visible = false;

                        #endregion

                        #region set "allow/disallow user to view your private photos" links

                        if (Config.Photos.EnablePrivatePhotos &&
                            CurrentUserSession != null &&
                            CurrentUserSession.HasPrivatePhotos())
                        {
                            if (CurrentUserSession.HasUserAccessToPrivatePhotos(user.Username))
                                pnlGrantAccessToPrivatePhotos.Visible = false;
                            else
                                pnlDenyAccessToPrivatePhotos.Visible = false;
                        }
                        else
                        {
                            pnlGrantAccessToPrivatePhotos.Visible = false;
                            pnlDenyAccessToPrivatePhotos.Visible = false;
                        }

                        #endregion

                        #region set "allow/disallow user to view your private video" links

                        if (
                            CurrentUserSession != null &&
                            (CurrentUserSession.HasPrivateVideo()
                             || CurrentUserSession.HasPrivateVideoUpload()))
                        {
                            if (CurrentUserSession.HasUserAccessToPrivateVideo(user.Username))
                                pnlGrantAccessToPrivateVideo.Visible = false;
                            else
                                pnlDenyAccessToPrivateVideo.Visible = false;
                        }
                        else
                        {
                            pnlGrantAccessToPrivateVideo.Visible = false;
                            pnlDenyAccessToPrivateVideo.Visible = false;
                        }

                        #endregion

                        #region set "allow/disallow user to view your private audio" links

                        if (Config.Misc.EnableAudioUpload && CurrentUserSession != null &&
                            CurrentUserSession.HasPrivateAudio())
                        {
                            if (CurrentUserSession.HasUserAccessToPrivateAudio(user.Username))
                                pnlGrantAccessToPrivateAudio.Visible = false;
                            else
                                pnlDenyAccessToPrivateAudio.Visible = false;
                        }
                        else
                        {
                            pnlGrantAccessToPrivateAudio.Visible = false;
                            pnlDenyAccessToPrivateAudio.Visible = false;
                        }

                        #endregion

                        #region activate/deactivate RealPerson Verification links

                        if (Config.Users.EnableRealPersonVerificationFunctionality &&
                            CurrentUserSession != null)
                        {

                            using (var db = new Model.ezFixUpDataContext())
                            {
                                var verifiedByThisUser =
                                    db.VerifiedUsers.Any(u => u.vu_verifiedby == CurrentUserSession.Username
                                                                   && u.vu_verifieduser == ViewedUser.Username);

                                if (verifiedByThisUser || CurrentUserSession.IsUserVerified(user.Username))
                                    pnlCertifyUserIsGenuine.Visible = false;
                                else
                                    pnlRemoveVerifiedUserStatus.Visible = false;
                            }
                        }
                        else
                        {
                            pnlCertifyUserIsGenuine.Visible = false;
                            pnlRemoveVerifiedUserStatus.Visible = false;
                        }

                        #endregion

                        #region Set "block/unlblock user" links

                        if (CurrentUserSession != null)
                        {
                            if (CurrentUserSession.IsUserBlocked(ViewedUser.Username))
                                pnlBlockUser.Visible = false;
                            else
                                pnlUnblockUser.Visible = false;
                        }
                        else
                        {
                            pnlBlockUser.Visible = false;
                            pnlUnblockUser.Visible = false;
                        }

                        #endregion

                        #region Enable/Disable ViewBlog

                        pnlBlog.Visible = Config.Misc.EnableBlogs && Blog.HasPosts(ViewedUser.Username);

                        #endregion

                        #region Enable/Disable ViewEvents

                        pnlViewEvents.Visible = Config.Users.EnableUserEventsPage;

                        #endregion

                        #region Add report abuse option

                        if (Config.AbuseReports.UserCanReportProfileAbuse
                            && (CurrentUserSession != null && (CurrentUserSession.BillingPlanOptions.UserCanReportAbuse.Value
                            || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanReportAbuse)))
                        {
                            pnlReportAbuseLink.Visible = true;
                        }

                        #endregion

                        #region Show blog if param is supplied

                        // Left for compatibility with old links to blog posts
                        if (Request.Params["bpid"] != null && !(Page is ShowUserBlog))
                        {
                            try
                            {
                                int blogPostId = Convert.ToInt32(Request.Params["bpid"]);
                                Response.Redirect(UrlRewrite.CreateShowUserBlogUrl(ViewedUser.Username,
                                    blogPostId));
                                return;
                            }
                            catch (ArgumentException)
                            {
                            }
                        }

                        #endregion

                        #region Set meta tags

                        Parser parse = delegate(string text)
                                           {
                                               string result = text
                                                   .Replace("%%USERNAME%%", user.Username)
                                                   .Replace("%%AGE%%", user.Age.ToString())
                                                   .Replace("%%GENDER%%", Lang.Trans(user.Gender.ToString()))
                                                   .Replace("%%COUNTRY%%", user.Country)
                                                   .Replace("%%STATE%%", user.State)
                                                   .Replace("%%ZIP%%", user.ZipCode)
                                                   .Replace("%%CITY%%", user.City);

                                               var regex = new Regex(@"%%Q_(\d+)%%");
                                               Match match = regex.Match(result);
                                               while (match.Success)
                                               {
                                                   foreach (Capture capture in match.Groups[1].Captures)
                                                   {
                                                       int questionId;
                                                       if (!int.TryParse(capture.Value, out questionId)) continue;
                                                       try
                                                       {
                                                           ProfileAnswer answer =
                                                               ProfileAnswer.Fetch(user.Username, questionId);
                                                           result =
                                                               result.Replace(String.Format("%%Q_{0}%%", questionId),
                                                                              Server.HtmlEncode(answer.Value));
                                                       }
                                                       catch (NotFoundException)
                                                       {
                                                           continue;
                                                       }
                                                   }
                                                   match = match.NextMatch();
                                               }

                                               return result;
                                           };

                        Page.Header.Title = parse(Config.SEO.ShowUserTitleTemplate);

                        var metaDesc = new HtmlMeta
                                           {
                                               ID = "Description",
                                               Name = "description",
                                               Content = parse(Config.SEO.ShowUserMetaDescriptionTemplate)
                                           };
                        Page.Header.Controls.Add(metaDesc);

                        var metaKeywords = new HtmlMeta
                                               {
                                                   ID = "Keywords",
                                                   Name = "keywords",
                                                   Content = parse(Config.SEO.ShowUserMetaKeywordsTemplate)
                                               };
                        Page.Header.Controls.Add(metaKeywords);

                        #endregion

                        #region Show AddThis link

                        if (Config.ThirdPartyServices.UseAddThis)
                        {
                            divAddThis.Visible = true;
                            divAddThis.InnerHtml = Config.ThirdPartyServices.AddThisCode;
                        }

                        #endregion
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (ArgumentException)
                    {
                        Response.Redirect(MatchmakerHelper.CurrentHomePage);
                    }
                    catch (NotFoundException)
                    {
                        Response.Redirect(MatchmakerHelper.CurrentHomePage);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError(err);
                        Response.Redirect(MatchmakerHelper.CurrentHomePage);
                    }
                }
                else
                {
                    Response.Redirect(MatchmakerHelper.CurrentHomePage);
                }

                #region Show/Hide links

                if (CurrentUserSession != null)
                {
                    if (Config.Users.EnableFavorites)
                    {
                        bool isFavorite = CurrentUserSession.IsUserInFavouriteList(ViewedUser.Username);
                        pnlRemoveFromFavourites.Visible = isFavorite;
                        pnlAddToFavourites.Visible = !isFavorite;
                    }
                    else
                    {
                        pnlRemoveFromFavourites.Visible = false;
                        pnlAddToFavourites.Visible = false;
                    }

                    if (Config.Users.EnableFriends)
                    {
                        bool isFriend = CurrentUserSession.IsUserInFriendList(ViewedUser.Username);
                        pnlRemoveFromFriends.Visible = isFriend;
                        pnlAddToFriends.Visible = !isFriend;
                        //pnlViewMutualFriends.Visible = isFriend;
                    }
                    else
                    {
                        pnlAddToFriends.Visible = false;
                        pnlRemoveFromFriends.Visible = false;
                        //pnlViewMutualFriends.Visible = false;
                    }
                }
                else
                {
                    pnlRemoveFromFavourites.Visible = false;
                    pnlAddToFavourites.Visible = false;
                    pnlAddToFriends.Visible = false;
                    pnlRemoveFromFriends.Visible = false;
                    //pnlViewMutualFriends.Visible = false;
                }

                if (Page is ShowUserPage && Config.ThirdPartyServices.UseGoogleTranslateForMessages)
                {
                    divTranslate.Visible = true;
                }

                #endregion
            }

            #region Apply profile skin

            if (ViewedUser.ProfileSkin != null || Request.Params["skin"] != null)
            {
                var cssSkinCommon = new HtmlLink();
                cssSkinCommon.Attributes.Add("rel", "stylesheet");
                cssSkinCommon.Attributes.Add("type", "text/css");
                cssSkinCommon.Href = "Skins/common.css";
                Page.Header.Controls.Add(cssSkinCommon);

                var cssSkin = new HtmlLink();
                cssSkin.Attributes.Add("rel", "stylesheet");
                cssSkin.Attributes.Add("type", "text/css");
                cssSkin.Href = Request.Params["skin"] ?? ViewedUser.ProfileSkin;
                Page.Header.Controls.Add(cssSkin);
            }

            #endregion

            PrepareLinks();
        }

        private void LoadStrings()
        {
            SmallBoxStart2.Title = Lang.Trans("User Profile");
        }

        private void PrepareLinks()
        {
            if (Page is ShowUserPage)
                lnkViewProfile.Disabled = true;
            else
                lnkViewProfile.HRef = UrlRewrite.CreateShowUserUrl(ViewedUser.Username);

            if (Page is ShowUserPhotos)
                lnkViewPhotos.Disabled = true;
            else
                lnkViewPhotos.HRef = UrlRewrite.CreateShowUserPhotosUrl(ViewedUser.Username);

            if (Page is ShowUserBlog)
                lnkViewBlog.Disabled = true;
            else
                lnkViewBlog.HRef = UrlRewrite.CreateShowUserBlogUrl(ViewedUser.Username);

            if (Page is ShowUserEvents)
                lnkViewEvents.Disabled = true;
            else
                lnkViewEvents.HRef = UrlRewrite.CreateShowUserEventsUrl(ViewedUser.Username);

            if (Page is ReportUserAbuse)
                lnkReportAbuse.Disabled = true;
            else
                lnkReportAbuse.HRef = UrlRewrite.CreateReportUserAbuseUrl(ViewedUser.Username);

            pnlSendGift.Visible = CurrentUserSession != null && CurrentUserSession.CanSendGifts() != PermissionCheckResult.No;
            pnlSendWink.Visible = CurrentUserSession != null && CurrentUserSession.CanSendWinks() != PermissionCheckResult.No;
            if (CurrentUserSession == null || CurrentUserSession.Username == ViewedUser.Username)
            {
                lnkSendMessage.Disabled = true;
                lnkSendGift.Disabled = true;
                lnkSendWink.Disabled = true;
            }
            else
            {
                lnkSendGift.HRef = String.Format("SendGift_to_{0}.aspx", ViewedUser.Username);
                lnkSendWink.HRef = String.Format("SendWink_to_{0}.aspx", ViewedUser.Username);
                lnkSendMessage.HRef = String.Format("SendMessage_to_{0}.aspx", ViewedUser.Username);
            }

            //lnkSendToFriend.HRef = "SendProfile.aspx?uid=" + ViewedUser.Username;
            if (CurrentUserSession == null || CurrentUserSession.Username == ViewedUser.Username)
            {
                pnlAddToFavourites.Visible = false;
                pnlAddToFriends.Visible = false;
                pnlCertifyUserIsGenuine.Visible = false;
                pnlRemoveVerifiedUserStatus.Visible = false;
                pnlGrantAccessToPrivatePhotos.Visible = false;
                pnlDenyAccessToPrivatePhotos.Visible = false;
                pnlBlockUser.Visible = false;
                pnlUnblockUser.Visible = false;
                //pnlViewMutualFriends.Visible = false;
            }
            if (!Config.Users.EnableFriendsConnectionSearch)
            {
                //pnlViewMutualFriends.Visible = false;
            }
            lnkAddToFavourites.HRef = String.Format("AddRemoveFavorite.aspx?uid={0}&cmd={1}&src={2}",
                                                    ViewedUser.Username, "add", "profile");
            lnkRemoveFromFavourites.HRef = String.Format("AddRemoveFavorite.aspx?uid={0}&cmd={1}&src={2}",
                                                         ViewedUser.Username, "remove", "profile");
            lnkAddToFriends.HRef = String.Format("AddRemoveFriend.aspx?uid={0}&cmd={1}&src={2}",
                                                    ViewedUser.Username, "add", "profile");
            lnkRemoveFromFriends.HRef = String.Format("AddRemoveFriend.aspx?uid={0}&cmd={1}&src={2}",
                                                         ViewedUser.Username, "remove", "profile");
            const string spanFormat = "<span>{0}</span>";
            lnkBlockUser.Text = String.Format(spanFormat, "Block this User".Translate());
            lnkUnblockUser.Text = String.Format(spanFormat, "Unblock this User".Translate());
            lnkGrantAccess.Text = String.Format(spanFormat, "Grant Access to my Private Photos".Translate());
            lnkDenyAccess.Text = String.Format(spanFormat, "Revoke Access to my Private Photos".Translate());
            lnkGrantVideoAccess.Text = String.Format(spanFormat, "Grant Access to my Private Video".Translate());
            lnkDenyVideoAccess.Text = String.Format(spanFormat, "Revoke Access to my Private Video".Translate());
            lnkGrantAudioAccess.Text = String.Format(spanFormat, "Grant Access to my Private Audio".Translate());
            lnkDenyAudioAccess.Text = String.Format(spanFormat, "Revoke Access to my Private Audio".Translate());
            lnkCertifyUserIsGenuine.Text = String.Format(spanFormat, Lang.Trans("Certify that this user is Genuine"));
            lnkRemoveVerifiedUserStatus.Text = String.Format(spanFormat, Lang.Trans("Remove \"Certified\" status"));
            //lnkViewMutualFriends.Text = String.Format(spanFormat,"View mutual friends".Translate());
        }

        #region Nested type: Parser

        private delegate string Parser(string s);

        #endregion

        /// <summary>
        /// Handles the Click event of the lnkShowInterest control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkShowInterestClick(object sender, EventArgs e)
        {
            //            if (CurrentUserSession == null)
            //                Response.Redirect("~/Login.aspx");
            //
            //            try
            //            {
            //                bool sent = Interest.Send(CurrentUserSession.Username, ViewedUser.Username);
            //
            //                if (sent)
            //                {
            //                    Page.StatusPageMessage = String.Format("{0} will now be informed that you have shown interest in their profile.".Translate(), 
            //                        ViewedUser.Username);
            //
            //                    #region Add NewInterest Event
            //
            //                    Event[] evt = Event.Fetch(ViewedUser.Username, null, Event.eType.NewInterest, DateTime.Now.Date);
            //
            //                    if (evt.Length > 0)
            //                    {
            //                        NewInterest newInterest =
            //                            Misc.FromXml<NewInterest>(evt[0].DetailsXML);
            //                        if (!newInterest.Usernames.Contains(CurrentUserSession.Username))
            //                        {
            //                            newInterest.Usernames.Add(CurrentUserSession.Username);
            //                            evt[0].DetailsXML = Misc.ToXml(newInterest);
            //                            evt[0].Save();
            //                        }
            //                    }
            //                    else
            //                    {
            //                        Event newEvent = new Event(ViewedUser.Username);
            //
            //                        newEvent.Type = Event.eType.NewInterest;
            //                        NewInterest newInterest = new NewInterest();
            //                        newInterest.Usernames.Add(CurrentUserSession.Username);
            //                        newEvent.DetailsXML = Misc.ToXml(newInterest);
            //
            //                        newEvent.Save();
            //                    }
            //
            //                    #endregion
            //                }
            //                else
            //                {
            //                    Page.StatusPageMessage = String.Format("You have already shown interest in {0}".Translate(), 
            //                        ViewedUser.Username);
            //                }
            //            }
            //            catch (NotFoundException)
            //            {
            //                Page.StatusPageMessage = "The user no longer exists!".Translate();
            //            }
            //
            //            Page.StatusPageLinkText = "Back to profile".Translate();
            //            Page.StatusPageLinkURL = UrlRewrite.CreateShowUserUrl(ViewedUser.Username);
            //
            //            Response.Redirect("ShowStatus.aspx");
        }

        /// <summary>
        /// Handles the Click event of the lnkBlockUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkBlockUserClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.BlockUser(ViewedUser.Username);
            Response.Redirect("~/BlockedUsersList.aspx");
        }

        /// <summary>
        /// Handles the Click event of the lnkUnblockUser control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkUnblockUserClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.UnblockUser(ViewedUser.Username);
            pnlBlockUser.Visible = true;
            pnlUnblockUser.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lnkGrantAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkGrantAccessClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.SetAccessToPrivatePhotos(ViewedUser.Username, true);
            pnlDenyAccessToPrivatePhotos.Visible = true;
            pnlGrantAccessToPrivatePhotos.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lnkDenyAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkDenyAccessClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.SetAccessToPrivatePhotos(ViewedUser.Username, false);
            pnlDenyAccessToPrivatePhotos.Visible = false;
            pnlGrantAccessToPrivatePhotos.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkGrantVideoAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkGrantVideoAccessClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.SetAccessToPrivateVideo(ViewedUser.Username, true);
            pnlDenyAccessToPrivateVideo.Visible = true;
            pnlGrantAccessToPrivateVideo.Visible = false;
        }

        /// <summary>
        /// Handles the Click event of the lnkDenyVideoAccess control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkDenyVideoAccessClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.SetAccessToPrivateVideo(ViewedUser.Username, false);
            pnlDenyAccessToPrivateVideo.Visible = false;
            pnlGrantAccessToPrivateVideo.Visible = true;
        }

        protected void LnkGrantAudioAccessClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.SetAccessToPrivateAudio(ViewedUser.Username, true);
            pnlDenyAccessToPrivateAudio.Visible = true;
            pnlGrantAccessToPrivateAudio.Visible = false;
        }

        protected void LnkDenyAudioAccessClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.SetAccessToPrivateAudio(ViewedUser.Username, false);
            pnlDenyAccessToPrivateAudio.Visible = false;
            pnlGrantAccessToPrivateAudio.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkCertifyUserIsGenuine control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkCertifyUserIsGenuineClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.SetAsVerified(ViewedUser.Username);
            pnlCertifyUserIsGenuine.Visible = false;
            pnlRemoveVerifiedUserStatus.Visible = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkRemoveVerifiedUserStatus control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void LnkRemoveVerifiedUserStatusClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("~/LoginThroughFacebook.aspx");
            else
                CurrentUserSession.RemoveVerifiedStatus(ViewedUser.Username);
            pnlCertifyUserIsGenuine.Visible = true;
            pnlRemoveVerifiedUserStatus.Visible = false;
        }

        protected void LnkViewMutualFriendsClick(object sender, EventArgs e)
        {
            var search = new MutualFriendsSearch
            {
                Viewer = CurrentUserSession.Username,
                Viewed = ViewedUser.Username
            };

            Global.GetSessionState()["MutualFriendsSearch"] = search;
            Response.Redirect("~/Search.aspx");
        }

        protected void LnkViewFriendsConnectionClick(object sender, EventArgs e)
        {
            var search = new FriendsConnectionSearch
            {
                Viewer = CurrentUserSession.Username,
                Viewed = ViewedUser.Username
            };

            Global.GetSessionState()["FriendsConnectionSearch"] = search;
            Response.Redirect("~/Search.aspx");
        }

        protected void LnkInstantMessengerPayClick(object sender, EventArgs e)
        {
            if (CurrentUserSession == null) return;
            Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanIM;
            Response.Redirect("~/ManageProfile.aspx?sel=payment");
        }
    }
}