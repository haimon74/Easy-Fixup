using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Mobile
{
    public partial class ShowUser : PageBase
    {
        public bool loadPhotos;
        private bool updateHistory = true;
        private bool paginatorVisible = true;

        protected int CurrentPhotoId
        {
            get
            {
                if (ViewState["CurrentPhotoId"] != null)
                {
                    return (int)ViewState["CurrentPhotoId"];
                }
                return -1;
            }
            set { ViewState["CurrentPhotoId"] = value; }
        }

        protected int[] UserPhotosIDs
        {
            set
            {
                if (ViewState["PhotosIDs"] == null)
                {
                    ViewState["PhotosIDs"] = Guid.NewGuid().ToString();
                }

                if (value != null && value.Length == 0)
                    value = null;

                Global.GetSessionState()["SearchResults" + ViewState["PhotosIDs"]] = value;

                CurrentPhoto = 1;
            }
            get
            {
                if (ViewState["PhotosIDs"] != null)
                {
                    return (int[]) Global.GetSessionState()["SearchResults" + ViewState["PhotosIDs"]];
                }
                return new int[0];
            }
        }

        private User user;
        public new User User
        {
            set
            {
                user = value;
                ViewState["Username"] = user != null ? user.Username : null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        public int? PhotoAlbumID
        {
            get { return (int?)ViewState["PhotoAlbumID"]; }
            set { ViewState["PhotoAlbumID"] = value; }
        }

        public string BigPhotoImageTag
        {
            get { return ltrPhoto.Text; }
            set { ltrPhoto.Text = value; }
        }

        PermissionCheckResult? viewPhotosPermission;
        private PermissionCheckResult ViewPhotosPermission
        {
            get
            {
                if (!viewPhotosPermission.HasValue)
                    viewPhotosPermission = CurrentUserSession.CanViewPhotos();

                return viewPhotosPermission.Value;
            }
        }

        private bool CanViewPhoto
        {
            get
            {
                if (CurrentUserSession == null)
                {
                    return Config.Users.GetNonPayingMembersOptions().CanViewPhotos.Value;
                }

                if (CurrentUserSession.Username == User.Username || ViewPhotosPermission == PermissionCheckResult.Yes ||
                    UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos, null))
                    return true;

                return false;
            }
        }

        //private bool PhotosLocked
        //{
        //    get
        //    {
        //        return Config.Credits.Required && Config.Credits.CreditsForUserPhotos > 0 &&
        //            (CurrentUserSession == null ||
        //            (CurrentUserSession.Username != User.Username &&
        //             !(Config.Users.FreeForFemales && CurrentUserSession.Gender == User.eGender.Female) &&
        //             !UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos, null)));
        //    }
        //}

        public int CurrentPhoto
        {
            set
            {
                ViewState["CurrentPhoto"] = value;
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (updateHistory && PaginatorEnabled
                    && scriptManager != null && scriptManager.IsInAsyncPostBack)
                {
                    scriptManager.AddHistoryPoint("page", value.ToString());
                }
                PreparePaginator();
            }
            get
            {
                if (ViewState["CurrentPhoto"] == null
                    || (int)ViewState["CurrentPhoto"] == 0)
                {
                    return 1;
                }
                return (int)ViewState["CurrentPhoto"];
            }
        }

        public bool PaginatorEnabled
        {
            get { return paginatorVisible; }
            set
            {
                paginatorVisible = value;
                pnlPaginator.Visible = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUserSession == null && Config.Users.RegistrationRequiredToBrowse)
                    Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

                if (Config.Users.CompletedProfileRequiredToBrowseSearch &&
                    CurrentUserSession != null && !CurrentUserSession.HasProfile)
                    Response.Redirect("ManageProfile.aspx?err=profnotcompl");
                
                loadPhotos = true;

                if (Request.Params["uid"] != null)
                {
                    try
                    {
                        User user = User.Load(Request.Params["uid"]);

                        if (user.Deleted)
                        {
                            if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                                StatusPageMessage = "This user has been deleted!".Translate();
                            else
                                StatusPageMessage =
                                    String.Format(
                                        "This user has been deleted for the following reason:<br><br>{0}".Translate(),
                                        user.DeleteReason);
                            Response.Redirect("Home.aspx");
                        }

                        User = user;

                        #region Show/hide profile

                        if (CurrentUserSession == null)
                        {
                            if (!User.IsOptionEnabled(eUserOptions.VisitorsCanViewProfile))
                            {
                                if (User.IsOptionEnabled(eUserOptions.UsersCanViewProfile))
                                {
                                    StatusPageMessage = "This profile is visible for registered users only!".Translate();
                                    Response.Redirect("Home.aspx");
                                    return;
                                }
                                else if (User.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewProfile))
                                {
                                    StatusPageMessage = "This profile is visible for friends of friends!".Translate();
                                    Response.Redirect("Home.aspx");
                                    return;
                                }
                                else
                                {
                                    StatusPageMessage = "This profile is visible for friends only!".Translate();
                                    Response.Redirect("Home.aspx");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (User.Username != CurrentUserSession.Username
                                && !User.IsOptionEnabled(eUserOptions.VisitorsCanViewProfile)
                                && !User.IsOptionEnabled(eUserOptions.UsersCanViewProfile))
                            {
                                if (User.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewProfile))
                                {
                                    if (!User.IsUserInFriendList(CurrentUserSession.Username))
                                    {
                                        bool areFriends = false;
                                        string[] friends = User.FetchMutuallyFriends(User.Username);
                                        foreach (string friend in friends)
                                        {
                                            if (User.IsUserInFriendList(friend, CurrentUserSession.Username))
                                            {
                                                areFriends = true;
                                                break;
                                            }
                                        }
                                        if (!areFriends)
                                        {
                                            StatusPageMessage = "This profile is visible for friends of friends!".Translate();
                                            Response.Redirect("Home.aspx");
                                            return;
                                        }
                                    }
                                }
                                else if (!User.IsUserInFriendList(CurrentUserSession.Username))
                                {
                                    StatusPageMessage = "This profile is visible for friends only!".Translate();
                                    Response.Redirect("Home.aspx");
                                    return;
                                }
                            }
                        }

                        #endregion

                        #region Show/hide photos

                        if (!Config.Photos.FreeMembersCanViewPhotosOfPaidMembers)
                        {
                            if (CurrentUserSession == null)
                            {
                                if (Classes.User.IsPaidMember(User.Username))
                                {
                                    StatusPageMessage = "Only paid members can view those photos".Translate();
                                    loadPhotos = false;
                                }
                            }
                            else if (!Classes.User.IsPaidMember(CurrentUserSession.Username)
                                        && Classes.User.IsPaidMember(User.Username))
                            {
                                StatusPageMessage = "Only paid members can view those photos".Translate();
                                loadPhotos = false;
                            }
                        }

                        if (CurrentUserSession == null)
                        {
                            if (!User.IsOptionEnabled(eUserOptions.VisitorsCanViewPhotos))
                            {
                                if (User.IsOptionEnabled(eUserOptions.UsersCanViewPhotos))
                                {
                                    StatusPageMessage = "This photos are visible for registered users only!".Translate();
                                    loadPhotos = false;
                                }
                                else if (User.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewPhotos))
                                {
                                    StatusPageMessage = "This photos are visible for friends of friends!".Translate();
                                    loadPhotos = false;
                                }
                                else
                                {
                                    StatusPageMessage = "This photos are visible for friends only!".Translate();
                                    loadPhotos = false;
                                }
                            }
                        }
                        else
                        {
                            if (User.Username != CurrentUserSession.Username
                                && !User.IsOptionEnabled(eUserOptions.VisitorsCanViewPhotos)
                                && !User.IsOptionEnabled(eUserOptions.UsersCanViewPhotos))
                            {
                                if (User.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewPhotos))
                                {
                                    if (!User.IsUserInFriendList(CurrentUserSession.Username))
                                    {
                                        bool areFriends = false;
                                        string[] friends = Classes.User.FetchMutuallyFriends(User.Username);
                                        foreach (string friend in friends)
                                        {
                                            if (Classes.User.IsUserInFriendList(friend, CurrentUserSession.Username))
                                            {
                                                areFriends = true;
                                                break;
                                            }
                                        }
                                        if (!areFriends)
                                        {
                                            StatusPageMessage = "This photos are visible for friends of friends!".Translate();
                                            loadPhotos = false;
                                        }
                                    }
                                }
                                else if (!User.IsUserInFriendList(CurrentUserSession.Username))
                                {
                                    StatusPageMessage = "This photos are visible for friends only!".Translate();
                                    loadPhotos = false;
                                }
                            }
                        }

                        #endregion

                        #region Save profile view

                        if (CurrentUserSession != null &&
                            !CurrentUserSession.IsOptionEnabled(eUserOptions.DisableProfileViews))
                        {
                            User.SaveProfileView(
                                CurrentUserSession.Username, User.Username);

                            User.AddScore(CurrentUserSession.Username,
                                          Config.UserScores.ViewingProfile, "ViewingProfile");
                            User.AddScore(User.Username,
                                          Config.UserScores.ViewedProfile, "ViewedProfile");

                            if (Config.Users.NewEventNotification && 
                                (User.IsOnline() || User.IsUsingNotifier(User.Username)))
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
                                    Recipient = User.Username,
                                    Sender = CurrentUserSession.Username,
                                    Text = text,
                                    ThumbnailUrl = thumbnailUrl,
                                    RedirectUrl = UrlRewrite.CreateMobileShowUserUrl(CurrentUserSession.Username)
                                };
                                RealtimeNotification.SendNotification(notification);
                            }
                        }

                        #endregion

                        LoadStrings();
                        LoadProfile();
                    }
                    catch (ThreadAbortException)
                    {
                    }
                    catch (ArgumentException)
                    {
                        Response.Redirect("Home.aspx");
                    }
                    catch (NotFoundException)
                    {
                        Response.Redirect("Home.aspx");
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError(err);
                        Response.Redirect("Home.aspx");
                    }
                }
                else
                {
                    Response.Redirect("Home.aspx");
                }
            }

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (CurrentUserSession != null && !CanViewPhoto)
            {
                if (ViewPhotosPermission == PermissionCheckResult.YesButMoreCreditsNeeded || ViewPhotosPermission == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    StatusPageMessage = "You have to be paid member in order to view photos!".Translate();
                    Response.Redirect("Home.aspx");
                    return;
                }

                if (ViewPhotosPermission == PermissionCheckResult.No)
                {
                    StatusPageMessage = "You are not allowed to view photos!".Translate();
                    Response.Redirect("Home.aspx");
                    return;
                }
            }

            if (loadPhotos)
                LoadPhotos();

            if (PaginatorEnabled)
            {
                PreparePaginator();
            }
        }

        private void LoadStrings()
        {
            lblTitle.InnerText = Lang.Trans("User Profile");

            if (CurrentUserSession == null || CurrentUserSession.Username == User.Username)
            {
                lnkSendMessage.Visible = false;
            }
            else
            {
                lnkSendMessage.HRef = "SendMessage.aspx?to_user=" + User.Username + "&src=profile";
            }

            lnkPrev.AlternateText = Lang.Trans("[ Prev ]");
            lnkNext.AlternateText = Lang.Trans("[ Next ]");
            btnUnlockPhotos.Text = Lang.Trans("Unlock Photos");
        }

        public void LoadProfile()
        {
            if (User == null)
            {
                return;
            }

            Photo primaryPhoto = null;
            bool hasPhoto = false;
            try
            {
                primaryPhoto = user.GetPrimaryPhoto();
                hasPhoto = primaryPhoto.Approved ? true : false;
            }
            catch (NotFoundException)
            {
            }

            lblUsername.Text = User.Username;

            if (Config.Users.CouplesSupport && User.Gender == User.eGender.Couple)
            {
                lblAge.Text = Lang.Trans("him") + " " +
                              (User.Age) +
                              ", " + Lang.Trans("her") + " " +
                              ((int)(DateTime.Now.Subtract(User.Birthdate2).TotalDays / 365.25));
            }
            else
            {
                lblAge.Text = User.Age.ToString();
            }

            lblGender.Text = Lang.Trans(User.Gender.ToString());

            lblLastOnline.Text = User.LastOnlineString;

            #region Load answers

            // Load slogan
            try
            {
                ProfileAnswer slogan = User.FetchSlogan();
                lblSlogan.Text = slogan.Approved
                                     ? Server.HtmlEncode(slogan.Value)
                                     : Lang.Trans("-- pending approval --");
            }
            catch (NotFoundException)
            {
                lblSlogan.Text = Lang.Trans("-- no headline --");
            }

            DataTable dtTopics = new DataTable();
            dtTopics.Columns.Add("TopicID", typeof (int));
            dtTopics.Columns.Add("TopicName");

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();
            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions == null)
                        continue;

                    bool topicHasQuestion = false;
                    foreach (ProfileQuestion question in questions)
                    {
                        if (question.ShowStyle != ProfileQuestion.eShowStyle.Slogan
                            && question.ShowStyle != ProfileQuestion.eShowStyle.SkypeLink)
                        {
                            topicHasQuestion = true;
                            break;
                        }
                    }

                    if (!topicHasQuestion)
                        continue;

                    bool hasVisibleQuestions = false;
                    List<int> lHiddenParentQuestions = new List<int>();
                    foreach (ProfileQuestion question in questions)
                    {
                        if (!question.IsVisible(User.Gender)
                            || question.ShowStyle == ProfileQuestion.eShowStyle.Hidden
                            || question.ShowStyle == ProfileQuestion.eShowStyle.Slogan
                            || question.ShowStyle == ProfileQuestion.eShowStyle.SkypeLink
                            || (question.VisibleForPaidOnly && (CurrentUserSession == null || !CurrentUserSession.Paid))
                            ||
                            (question.ParentQuestionID.HasValue &&
                             lHiddenParentQuestions.Contains(question.ParentQuestionID.Value)))
                        {
                            lHiddenParentQuestions.Add(question.Id);
                            continue;
                        }

                        if (question.ParentQuestionID.HasValue)
                        {
                            ProfileQuestion parentQuestion = ProfileQuestion.Fetch(question.ParentQuestionID.Value);
                            List<string> lParentQuestionChoices =
                                new List<string>(question.ParentQuestionChoices.Split(':'));
                            string profileAnswerValue = String.Empty;
                            try
                            {
                                profileAnswerValue = parentQuestion.FetchAnswer(User.Username).Value;
                            }
                            catch (NotFoundException)
                            {
                            }
                            List<string> lChoices = new List<string>(profileAnswerValue.Split(':'));

                            if (!lParentQuestionChoices.Exists(parentChoice => lChoices.Contains(parentChoice)))
                            {
                                continue;
                            }
                        }

                        hasVisibleQuestions = true;
                    }

                    if (!hasVisibleQuestions)
                        continue;

                    dtTopics.Rows.Add(new object[]
                                           {
                                               topic.ID,
                                               Config.Misc.EnableProfileDataTranslation
                                                   ? Lang.Trans(topic.Name)
                                                   : topic.Name
                                           });
                }

                rptTopics.DataSource = dtTopics;
                rptTopics.DataBind();
            }

            #endregion

            #region Load Status text

            if (Config.Users.EnableUserStatusText)
            {
                lblStatusText.Text = Server.HtmlEncode(Parsers.TrimLongWords(User.StatusText, 20));
                pnlStatusText.Visible = User.StatusText != null && (!Config.Misc.SiteIsPaid || User.IsPaidMember(User.Username));
            }

            #endregion
        }

        public void LoadPhotos()
        {
            if (User == null) return;

            Photo[] photos = null;
            pnlUnlockPhotos.Visible = CurrentUserSession != null && ViewPhotosPermission == PermissionCheckResult.YesWithCredits && !CanViewPhoto;
            if (/*!pnlUnlockPhotos.Visible*/CanViewPhoto)
            {
                photos = Photo.Fetch(User.Username, PhotoAlbumID);
                PaginatorEnabled = true;
            }
            else
            {
                ltrPhoto.Text = String.Empty;
                PaginatorEnabled = false;
            }

            bool hasAccess = CurrentUserSession != null &&
                             (CurrentUserSession.Username == User.Username ||
                              User.HasUserAccessToPrivatePhotos(User.Username,CurrentUserSession.Username));

            var lPhotosIDs = new List<int>();
            if (photos != null && photos.Length > 0)
            {
                Photo primaryPhoto =
                    Array.Find(photos, p => p.Primary && p.Approved && (!p.PrivatePhoto || hasAccess)) ??
                    Array.Find(photos, p => p.Approved && (!p.PrivatePhoto || hasAccess));

                if (primaryPhoto != null && ltrPhoto.Text == "")
                {
                    CurrentPhotoId = primaryPhoto.Id;
                    string imageTag = ImageHandler.RenderImageTag(primaryPhoto.Id, 300, 300, null, false, true);
                    imageTag = imageTag.Replace("<img", "<img id=\"imgPhoto\"");
                    ltrPhoto.Text = imageTag;
                }

                foreach (Photo photo in photos)
                {
                    if (!photo.Approved) continue;
                    if (photo.PrivatePhoto && !hasAccess)
                        continue;

                    lPhotosIDs.Add(photo.Id);
                }
            }

            UserPhotosIDs = lPhotosIDs.ToArray();

            if (lPhotosIDs.Count == 0)
            {
                ltrPhoto.Visible = false;
            }
            else
            {
                ltrPhoto.Visible = true;
            }
        }

        protected void lnkPhoto_Click(object sender, EventArgs e)
        {
            int photoID = getNextPhotoID(UserPhotosIDs);
            string imageTag = ImageHandler.RenderImageTag(photoID, 300, 300, null, false, true);
            imageTag = imageTag.Replace("<img", "<img id=\"imgPhoto\"");
            ltrPhoto.Text = imageTag;
        }

        private int getNextPhotoID(int[] photosIDs)
        {
            int currentPhotoIndex = Array.FindIndex(photosIDs, p => p == CurrentPhotoId);
            int id = currentPhotoIndex == photosIDs.Length - 1 ? photosIDs[0] : photosIDs[currentPhotoIndex + 1];
            CurrentPhotoId = id;
            return id;
        }

        private int getPrevPhotoID(int[] photosIDs)
        {
            int currentPhotoIndex = Array.FindIndex(photosIDs, p => p == CurrentPhotoId);
            int id = currentPhotoIndex == 0 ? photosIDs[photosIDs.Length -1] : photosIDs[currentPhotoIndex - 1];
            CurrentPhotoId = id;
            return id;
        }

        protected void btnUnlockPhotos_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
            {
                Response.Redirect("Register.aspx");
                return;
            }

            if (UnlockedSection.IsSectionUnlocked(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos, null))
                return;

            if (CurrentUserSession.Credits - CurrentUserSession.BillingPlanOptions.CanViewPhotos.Credits < 0)
            {
                StatusPageMessage = "You have to be paid member in order to use this feature!".Translate();
                Response.Redirect("Home.aspx");
                return;
            }

            CurrentUserSession.Credits -= CurrentUserSession.BillingPlanOptions.CanViewPhotos.Credits;
            CurrentUserSession.Update(true);

            CreditsHistory creditsHistory = new Classes.CreditsHistory(CurrentUserSession.Username);
            creditsHistory.Amount = CurrentUserSession.BillingPlanOptions.CanViewPhotos.Credits;
            creditsHistory.Service = CreditsHistory.eService.ViewPhotos;
            creditsHistory.Save();

            UnlockedSection.UnlockSection(CurrentUserSession.Username, User.Username, UnlockedSection.SectionType.Photos,
                                          null, DateTime.Now.AddDays(Config.Credits.PhotoUnlockPeriod));
            loadPhotos = true;
        }

        protected void rptTopics_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) || (item.ItemType == ListItemType.AlternatingItem))
            {
                var rptQuestions = (Repeater)item.FindControl("rptQuestions");
                int topicID = (int)DataBinder.Eval(e.Item.DataItem, "TopicID");

                ProfileQuestion[] questions = ProfileQuestion.FetchByTopicID(topicID);

                if (questions == null)
                    return;

                DataTable dtAnswers = new DataTable();
                dtAnswers.Columns.Add("QuestionName");
                dtAnswers.Columns.Add("QuestionAltName");
                dtAnswers.Columns.Add("Answer");

                bool hasVisibleQuestions = false;
                List<int> lHiddenParentQuestions = new List<int>();

                foreach (ProfileQuestion question in questions)
                {
                    if (!question.IsVisible(User.Gender)
                            || question.ShowStyle == ProfileQuestion.eShowStyle.Hidden
                            || question.ShowStyle == ProfileQuestion.eShowStyle.Slogan
                            || question.ShowStyle == ProfileQuestion.eShowStyle.SkypeLink
                            || (question.VisibleForPaidOnly && (CurrentUserSession == null || !CurrentUserSession.Paid))
                            || (question.ParentQuestionID.HasValue && lHiddenParentQuestions.Contains(question.ParentQuestionID.Value)))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        continue;
                    }

                    if (question.ParentQuestionID.HasValue)
                    {
                        ProfileQuestion parentQuestion = ProfileQuestion.Fetch(question.ParentQuestionID.Value);
                        List<string> lParentQuestionChoices = new List<string>(question.ParentQuestionChoices.Split(':'));
                        string profileAnswerValue = String.Empty;
                        try
                        {
                            profileAnswerValue = parentQuestion.FetchAnswer(User.Username).Value;
                        }
                        catch (NotFoundException)
                        {
                        }
                        List<string> lChoices = new List<string>(profileAnswerValue.Split(':'));

                        if (!lParentQuestionChoices.Exists(parentChoice => lChoices.Contains(parentChoice)))
                        {
                            continue;
                        }
                    }

                    hasVisibleQuestions = true;

                    #region Load answer

                    string ans = String.Empty;
                    string questionAltName = String.Empty;

                    if (Config.Misc.EnableProfileDataTranslation)
                        questionAltName = Lang.Trans(question.AltName);
                    else
                        questionAltName = question.AltName;

                    try
                    {
                        ProfileAnswer answer = ProfileAnswer.Fetch(User.Username, question.Id);
                        if (answer.Value == String.Empty)
                        {
                            ans = Lang.Trans("-- no answer --");
                        }
                        else
                        {
                            string sAnswer = answer.Value;
                            sAnswer = sAnswer
                                .Replace("\n", "<br>");

                            string[] dataSource = sAnswer.Split(':');

                            if (question.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceCheck ||
                                question.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceSelect ||
                                question.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceRadio ||
                                question.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceSelect)
                            {
                                for (int i = 0; i < dataSource.Length; ++i)
                                {
                                    if (Config.Misc.EnableProfileDataTranslation)
                                        dataSource[i] = Lang.Trans(dataSource[i]);

                                    dataSource[i] = Server.HtmlEncode(dataSource[i]);
                                }
                            }


                            dataSource.ToList().ForEach(a => ans += " " + a);
                        }
                    }
                    catch (NotFoundException)
                    {
                        ans = Lang.Trans("-- no answer --");
                    }

                    #endregion

                    dtAnswers.Rows.Add(new object[]
                                        {
                                            question.Name, questionAltName, ans
                                        });
                }

                if (!hasVisibleQuestions)
                {
                    return;
                }

                rptQuestions.DataSource = dtAnswers;
                rptQuestions.DataBind();
            }
        }

        private void PreparePaginator()
        {
            if (UserPhotosIDs == null || UserPhotosIDs.Length == 0 || CurrentPhoto <= 1)
            {
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkPrev.Enabled = true;
            }
            if (UserPhotosIDs == null || UserPhotosIDs.Length == 0 || CurrentPhoto >= UserPhotosIDs.Length)
            {
                lnkNext.Enabled = false;
            }
            else
            {
                lnkNext.Enabled = true;
            }
            if (UserPhotosIDs != null && UserPhotosIDs.Length > 0)
            {
                lblPager.Text = String.Format(Lang.Trans("{0}/{1}"), CurrentPhoto, UserPhotosIDs.Length);
                PaginatorEnabled = true;
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
                PaginatorEnabled = false;
            }
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPhoto > 1)
            {
                CurrentPhoto--;
                int photoID = getPrevPhotoID(UserPhotosIDs);
                string imageTag = ImageHandler.RenderImageTag(photoID, 300, 300, null, false, true);
                imageTag = imageTag.Replace("<img", "<img id=\"imgPhoto\"");
                ltrPhoto.Text = imageTag;
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (UserPhotosIDs.Length == 0)
                Response.Redirect("Home.aspx");

            if (CurrentPhoto < UserPhotosIDs.Length)
            {
                CurrentPhoto++;
                int photoID = getNextPhotoID(UserPhotosIDs);
                string imageTag = ImageHandler.RenderImageTag(photoID, 300, 300, null, false, true);
                imageTag = imageTag.Replace("<img", "<img id=\"imgPhoto\"");
                ltrPhoto.Text = imageTag;
            }
        }

        void scriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            if (UserPhotosIDs.Length == 0)
                Response.Redirect("Home.aspx");

            int navigatePage;
            try
            {
                navigatePage = e.State.Count == 0 ? 1 : Convert.ToInt32(e.State[0]);
            }
            catch (FormatException)
            {
                navigatePage = 1;
            }
            if (navigatePage <= UserPhotosIDs.Length && navigatePage > 0)
            {
                updateHistory = false;
                CurrentPhoto = navigatePage;
            }
        }
    }
}
