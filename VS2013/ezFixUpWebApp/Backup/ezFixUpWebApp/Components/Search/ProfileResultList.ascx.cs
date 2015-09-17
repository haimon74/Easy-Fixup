/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
//#define _MULTIPLY_RESULTS_HAIM
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using ezFixUp.Services;
using Message = ezFixUp.Classes.Message;
using Photo = ezFixUp.Classes.Photo;
using PhotoRating = ezFixUp.Classes.PhotoRating;
using ProfileAnswer = ezFixUp.Classes.ProfileAnswer;
using User = ezFixUp.Classes.User;
using UserLevel = ezFixUp.Classes.UserLevel;
using VideoProfile = ezFixUp.Classes.VideoProfile;
using VideoUpload = ezFixUp.Classes.VideoUpload;


namespace ezFixUp.Components.Search
{
    public partial class ProfileResultList : UserControl
    {
        public enum ResultTypeEnum
        {
            NewMembers, MySingleMaleFriends, MySingleFemaleFriends, MyMatches, MyFavorites, MyFriends
        } ;

        public ResultTypeEnum ResultType { get; set; }
        public int ResultLayoutWidth = 620;
        public int ResultLayoutHeight;
        public int ListPhotoWidthPx { get { return 163; /*ResultLayoutWidth * ((100 / 2) - 2) / 100;*/ } }
        public int GridBorderPx = 2;
        public int RepeatColumns { get; set; }
        public int LimitRows { get; set; }

        /// <summary>
        /// Sets a value indicating whether [paginator enabled].
        /// </summary>
        /// <value><c>true</c> if [paginator enabled]; otherwise, <c>false</c>.</value>
        private bool _paginatorVisible = true;

        protected bool showCity = Config.Users.LocationPanelVisible;
        protected bool showGender = !Config.Users.DisableGenderInformation;
        protected bool showAge = !Config.Users.DisableAgeInformation;
        protected bool showIcons = true;
        protected bool showModerationScore;
        protected bool showRating;
        protected bool showSlogan = true;
        protected bool showTopPhoto;
        protected bool showFriendsPath;
        protected bool useCache;
        private bool updateHistory = true;

        private string ProfileResultsGuid { get; set; }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        public UserSearchResults Results
        {
            set
            {
                if (ViewState[ProfileResultsGuid] == null)
                {
                    ViewState[ProfileResultsGuid] = Guid.NewGuid().ToString();
                }

                if (value != null && value.Usernames != null && value.Usernames.Length == 0)
                    value = null;

                Global.GetSessionState()["ProfileResultList" + ViewState[ProfileResultsGuid]] = value;

                //CurrentPage = 1;
            }
            get
            {
                if (ViewState[ProfileResultsGuid] != null)
                {
                    return (UserSearchResults)
                           Global.GetSessionState()["ProfileResultList" + ViewState[ProfileResultsGuid]];
                }
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            set
            {
                Trace.Write("ProfileResultList.ascx.cs", "CurrentPage = " + value);
                ViewState["CurrentPage"] = value;
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
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                {
                    return 1;
                }
                return (int)ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            get { return _paginatorVisible; }
            set
            {
                _paginatorVisible = value;
                pnlPaginator.Visible = value;
            }
        }

        public bool ShowIcons
        {
            set { showIcons = value; }
        }

        /// <summary>
        /// Gets or sets the value indicating whether [show gender].
        /// </summary>
        public bool ShowGender
        {
            get { return showGender; }
            set { showGender = value; }
        }

        public bool ShowAge
        {
            get { return showAge; }
            set { showAge = value; }
        }

        /// <summary>
        /// Sets a value indicating whether [show top photo instead of primary].
        /// </summary>
        public bool ShowTopPhoto
        {
            set { showTopPhoto = value; }
        }

        /// <summary>
        /// Sets a value indicating whether [show slogan].
        /// </summary>
        /// <value><c>true</c> if [show slogan]; otherwise, <c>false</c>.</value>
        public bool ShowSlogan
        {
            set { showSlogan = value; }
        }

        /// <summary>
        /// Sets a value indicating wheteher [show viewed on].
        /// </summary>
        /// <value></value>
        public bool ShowViewedOn
        {
            set { ViewState["ShowViewedOn"] = value; }
            get
            {
                return ViewState["ShowViewedOn"] == null
                           ? false
                           :
                               (bool)ViewState["ShowViewedOn"];
            }
        }

        public string ShowViewedOnUsername
        {
            set { ViewState["ShowViewedOnUsername"] = value; }
            get
            {
                return ViewState["ShowViewedOnUsername"] == null
                           ? ""
                           :
                               (string)ViewState["ShowViewedOnUsername"];
            }
        }

        public bool ShowZodiacSign
        {
            set { ViewState["ShowZodiacSign"] = value; }
            get
            {
                return !Config.Users.DisableAgeInformation && (ViewState["ShowZodiacSign"] == null
                                                                   ? true
                                                                   : (bool)ViewState["ShowZodiacSign"]);
            }
        }

        public bool ShowDistance
        {
            set { ViewState["ShowDistance"] = value; }
            get
            {
                return ViewState["ShowDistance"] == null
                           ? false
                           :
                               (bool)ViewState["ShowDistance"];
            }
        }

        //protected bool showLastOnline = true;

        /// <summary>
        /// Sets a value indicating whether [show last online].
        /// </summary>
        /// <value><c>true</c> if [show last online]; otherwise, <c>false</c>.</value>
        public bool ShowLastOnline
        {
            set { ViewState["ShowLastOnline"] = value; }
            get
            {
                return ViewState["ShowLastOnline"] == null
                           ? false
                           :
                               (bool)ViewState["ShowLastOnline"];
            }
        }

        public bool ShowRating
        {
            set { showRating = value; }
        }

        public bool ShowCity
        {
            set { showCity = value; }
        }

        public bool ShowModerationScore
        {
            get { return showModerationScore; }
            set { showModerationScore = value; }
        }

        public bool ShowFriendsPath
        {
            set
            {
                showFriendsPath = value;
                if (value)
                {
                    PaginatorEnabled = false;
                    EnableGridSupport = false;
                    ShowIcons = false;
                    ShowSlogan = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the group ID. This property should be set if this control will be used in the
        /// group section. If it not exists in ViewState it will return NULL otherwise return group ID.
        /// </summary>
        /// <value>The group ID.</value>
        public int? GroupID
        {
            get { return ViewState["GroupID"] == null ? null : (int?)ViewState["GroupID"]; }
            set { ViewState["GroupID"] = value; }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        public bool UseCache
        {
            set { useCache = value; }
        }

        public bool EnableGridSupport { get; set; }

        public bool GridMode
        {
            set { ViewState["ProfileResultList_GridMode"] = value; }
            get
            {
                if (ViewState["ProfileResultList_GridMode"] is bool)
                    return (bool)ViewState["ProfileResultList_GridMode"];
                return false;
            }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Write("ProfileResultList.ascx.cs", "Page_Load");
            ShowLastOnline = false;
            this.ProfileResultsGuid = this.UniqueID + "_guid";
            dlUsers.Width = ResultLayoutWidth;
            //ModalPopupExtender1.BehaviorID = dlUsers.UniqueID + "_ModalBehavior";
            //dlUsers.DataBinding += this.OnDataBinding;
            if (!Page.IsPostBack)
            {
                LoadStrings();
                switch (ResultType)
                {
                    case ResultTypeEnum.NewMembers:
                        LoadNewMembers();
                        break;
                    case ResultTypeEnum.MySingleMaleFriends:
                        LoadMySingleMaleFriends();
                        break;
                    case ResultTypeEnum.MySingleFemaleFriends:
                        LoadMySingleFemaleFriends();
                        break;
                    case ResultTypeEnum.MyMatches:
                        LoadNewMembers();
                        break;
                    case ResultTypeEnum.MyFriends:
                        LoadNewMembers();
                        break;
                    case ResultTypeEnum.MyFavorites:
                        LoadNewMembers();
                        break;

                }
            }
            dlUsers.Visible = !GridMode;
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
        }

        public EventHandler OnDataBinding { get; set; }

        private void SetPaginatorButtonsCSS()
        {
            if (Global.GetSessionState()["Direction"] != null && Global.GetSessionState()["Direction"].Equals("rtl"))
            {
                lnkFirst.CssClass = "lastPageBtn";
                lnkPrev.CssClass = "nextPageBtn";
                lnkNext.CssClass = "prevPageBtn";
                lnkLast.CssClass = "firstPageBtn";
            }
            else
            {
                lnkFirst.CssClass = "firstPageBtn";
                lnkPrev.CssClass = "prevPageBtn";
                lnkNext.CssClass = "nextPageBtn";
                lnkLast.CssClass = "lastPageBtn";
            }
            lnkFirst.Visible = lnkFirst.Enabled;
            lnkPrev.Visible = lnkPrev.Enabled;
            lnkNext.Visible = lnkNext.Enabled;
            lnkLast.Visible = lnkLast.Enabled;
        }

        protected override void OnPreRender(EventArgs e)
        {
            LoadResultsPage();
            if (PaginatorEnabled)
            {
                PreparePaginator();
            }
            base.OnPreRender(e);
        }

        /// <summary>
        /// Loads the strings.
        /// </summary>
        private void LoadStrings()
        {
            lnkFirst.ToolTip = /*@"◄◄  " +*/ Lang.Trans("First");
            lnkPrev.ToolTip = /*@"◄  " +*/ Lang.Trans("Prev");
            lnkNext.ToolTip = Lang.Trans("Next") /*+ @"  ►"*/;
            lnkLast.ToolTip = Lang.Trans("Last") /*+ @"  ►►"*/;
        }

        /// <summary>
        /// Prepares the paginator.
        /// </summary>
        private void PreparePaginator()
        {
            int usersPerPage = dlUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
            bool toEnable = (Results != null && CurrentPage > 1);
            lnkFirst.Enabled = toEnable;
            lnkPrev.Enabled = toEnable;
            toEnable = (Results != null && CurrentPage < Results.GetTotalPages(usersPerPage));
            lnkLast.Enabled = toEnable;
            lnkNext.Enabled = toEnable;

            SetPaginatorButtonsCSS();

            if (Results != null && Results.Usernames != null && Results.Usernames.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * usersPerPage + 1;
                int toUser = CurrentPage * usersPerPage;
                if (Results.Usernames.Length < toUser)
                {
                    toUser = Results.Usernames.Length;
                }

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Usernames.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        /// <summary>
        /// Loads the results page.
        /// </summary>
        private void LoadResultsPage()
        {
            var dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("Name");
            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("PhotoId", typeof(int));
            dtResults.Columns.Add("PhotoUrl");
            dtResults.Columns.Add("Slogan");
            dtResults.Columns.Add("Age");
            dtResults.Columns.Add("Gender");
            dtResults.Columns.Add("InterestedIn");
            dtResults.Columns.Add("ViewedOnString");
            dtResults.Columns.Add("LastOnlineString");
            dtResults.Columns.Add("Rating");
            dtResults.Columns.Add("Location");
            dtResults.Columns.Add("Distance");
            dtResults.Columns.Add("ZodiacSign1", typeof(User.eZodiacSign));
            dtResults.Columns.Add("ZodiacSign2", typeof(User.eZodiacSign));
            dtResults.Columns.Add("MessageHistory", typeof(bool));
            dtResults.Columns.Add("VerifiedByUsers", typeof(bool));
            dtResults.Columns.Add("VerifiedByAdmin", typeof(bool));
            dtResults.Columns.Add("Blocked", typeof(bool));
            dtResults.Columns.Add("PrivatePhoto", typeof(bool));
            dtResults.Columns.Add("VideoProfile", typeof(bool));
            dtResults.Columns.Add("HasBlog", typeof(bool));
            dtResults.Columns.Add("GroupMemberType");
            dtResults.Columns.Add("GroupMemberJoinDate");
            dtResults.Columns.Add("GroupMemberIsAdmin", typeof(bool));
            dtResults.Columns.Add("GroupMemberIsModerator", typeof(bool));
            dtResults.Columns.Add("GroupMemberIsVip", typeof(bool));
            dtResults.Columns.Add("UserLevel", typeof(UserLevel));
            dtResults.Columns.Add("HideUserLevelIcon", typeof(bool));
            dtResults.Columns.Add("IsBroadcastingVideo", typeof(bool));
            dtResults.Columns.Add("ModerationScore", typeof(int));
            dtResults.Columns.Add("MutualFriend1_Name");
            dtResults.Columns.Add("MutualFriend2_Name");
            dtResults.Columns.Add("MutualFriend3_Name");
            dtResults.Columns.Add("MutualFriend1_Username");
            dtResults.Columns.Add("MutualFriend2_Username");
            dtResults.Columns.Add("MutualFriend3_Username");
            dtResults.Columns.Add("MutualFriend1_PhotoId", typeof(int));
            dtResults.Columns.Add("MutualFriend2_PhotoId", typeof(int));
            dtResults.Columns.Add("MutualFriend3_PhotoId", typeof(int));
            dtResults.Columns.Add("MutualFriend1_PhotoUrl");
            dtResults.Columns.Add("MutualFriend2_PhotoUrl");
            dtResults.Columns.Add("MutualFriend3_PhotoUrl");
            dtResults.Columns.Add("MutualFriend1_PopBG");
            dtResults.Columns.Add("MutualFriend2_PopBG");
            dtResults.Columns.Add("MutualFriend3_PopBG");
            dtResults.Columns.Add("AddFriendPostbackUrl");
            dtResults.Columns.Add("AddFavoritePostbackUrl");
            dtResults.Columns.Add("SendMsgPostbackUrl");
            dtResults.Columns.Add("SendGiftPostbackUrl");
            dtResults.Columns.Add("SendWinkPostbackUrl");
            dtResults.Columns.Add("AskAbout1_PostbackUrl");
            dtResults.Columns.Add("AskAbout2_PostbackUrl");
            dtResults.Columns.Add("AskAbout3_PostbackUrl");
            dtResults.Columns.Add("MatchmakingIsVisible", typeof(bool));
            dtResults.Columns.Add("AddFriendIsVisible", typeof(bool));
            dtResults.Columns.Add("WinkIsVisible", typeof(bool));
            dtResults.Columns.Add("MatchmakingToolTip");
            dtResults.Columns.Add("MatchmakingOnClientClick");

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);

                User[] users;
                if (PaginatorEnabled)
                {
                    int usersPerPage = dlUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
                    users = Results.GetPageBulk(CurrentPage, usersPerPage);
                }
                else
                {
                    int usersPerPage = RepeatColumns * LimitRows;
                    users = Results.GetPageBulk(CurrentPage, usersPerPage);
                    //users = Results.Get();
                }

                if (users != null && users.Length > 0)
                {
                    Location from = null;

                    if (CurrentUserSession != null)
                    {
                        from =
                            Config.Users.GetLocation(CurrentUserSession);
                    }

                    bool calculateDistance = from == null ? false : true;
                    int photoId = 0;
                    string photoUrl = String.Empty;

                    foreach (User user in users)
                    {
                        #region Gets User Photo

                        Photo photo = null;
                        try
                        {
                            if (showTopPhoto)
                                photo = user.GetTopPhoto();
                            else
                                photo = user.GetPrimaryPhoto();
                        }
                        catch (NotFoundException)
                        {
                        }

                        #endregion

                        #region Shows Slogan

                        string slogan = "";
                        try
                        {
                            ProfileAnswer sloganAnswer = user.FetchSlogan();

                            if (sloganAnswer.Approved)
                            {
                                slogan = sloganAnswer.Value;
                            }
                            else
                            {
                                slogan = Lang.Trans("-- pending approval --");
                            }
                        }
                        catch (NotFoundException)
                        {
                        }

                        #endregion

                        #region Shows User or Photo Rating

                        string ratingString = "";
                        if (showRating)
                        {
                            try
                            {
                                if (showTopPhoto)
                                {
                                    if (photo == null)
                                        throw new NotFoundException();
                                    PhotoRating photoRating = PhotoRating.FetchRating(photo.Id);
                                    ratingString = String.Format(
                                        Lang.Trans("{0} ({1} votes)"),
                                        photoRating.AverageVote.ToString("0.00"), photoRating.Votes);
                                }
                                else
                                {
                                    UserRating userRating = UserRating.FetchRating(user.Username);
                                    ratingString = String.Format(
                                        Lang.Trans("{0} ({1} votes)"),
                                        userRating.AverageVote.ToString("0.00"), userRating.Votes);
                                }
                            }
                            catch (NotFoundException)
                            {
                                ratingString = Lang.Trans("no rating");
                            }
                        }

                        #endregion

                        #region User's Age

                        string age = null;

                        if (!Config.Users.DisableAgeInformation)
                        {
                            if (Config.Users.CouplesSupport && user.Gender == User.eGender.Couple)
                            {
                                age = Lang.Trans("him") + " " +
                                      ((int)(DateTime.Now.Subtract(user.Birthdate).TotalDays / 365.25)) +
                                      ", " + Lang.Trans("her") + " " +
                                      ((int)(DateTime.Now.Subtract(user.Birthdate2).TotalDays / 365.25));
                            }
                            else
                            {
                                age = user.Age.ToString();
                            }
                        }


                        #endregion

                        #region User's gender

                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString().Translate() : String.Empty;
                        string interestedIn = !Config.Users.DisableGenderInformation ? user.InterestedIn.ToString() : String.Empty;
                        
                        #endregion

                        #region Show Distance

                        string distance = "";

                        if (calculateDistance)
                        {
                            if (Config.Search.ShowDistanceFromOnlineUser && CurrentUserSession != null && ShowDistance)
                            {
                                //                             ShowDistance = true;
                                Location to = Config.Users.GetLocation(user);

                                //ZipCode.DoLookupByZipCode(user.ZipCode);

                                if (to != null && user.Username != CurrentUserSession.Username) // location exist in DB
                                {
                                    char units = Config.Search.MeasureDistanceInKilometers ? 'k' : 'm';
                                    double _distance = Distance.GetDistance(from, to, units);
                                    string measure = Config.Search.MeasureDistanceInKilometers
                                                         ? Lang.Trans("kilometers")
                                                         : Lang.Trans("miles");
                                    distance = string.Format("{0:F1}", _distance) + " " + measure;
                                }
                            }
                        }

                        #endregion

                        User.eZodiacSign zodiacSign1 = 0;
                        User.eZodiacSign? zodiacSign2 = null;
                        bool messageHistoryExists = false;
                        bool verifiedByUsers = false;
                        bool verifiedByAdmin = false;
                        bool blocked = false;
                        bool hasVideoProfile = false;
                        bool hasPrivatePhoto = false;
                        bool hasBlog = false;
                        string memberType = "";
                        string joinDate = DateTime.Now.ToShortDateString();
                        bool isAdmin = false;
                        bool isModerator = false;
                        bool isVip = false;
                        bool isBroadcasting = false;

                        if (showIcons && dlUsers.Visible)
                        {
                            #region zodiac sign icon

                            if (Config.Users.EnableZodiacSign && ShowZodiacSign)
                            {
                                zodiacSign1 = user.ZodiacSign1;
                                zodiacSign2 = user.ZodiacSign2;
                            }

                            #endregion

                            #region message history exists

                            if (CurrentUserSession == null)
                                messageHistoryExists = false;
                            else
                            {
                                messageHistoryExists =
                                    Message.MessagesExist(user.Username, CurrentUserSession.Username) ||
                                    Message.MessagesExist(CurrentUserSession.Username, user.Username);
                            }

                            #endregion

                            #region verified icons

                            if (Config.Users.EnableRealPersonVerificationFunctionality)
                                verifiedByUsers = User.IsUserVerified(user.Username, false);

                            if (Config.Users.EnableRealPersonVerificationFunctionalityAdmin)
                                verifiedByAdmin = User.IsUserVerified(user.Username, true);

                            #endregion

                            #region blocked icon

                            if (CurrentUserSession != null)
                                blocked = CurrentUserSession.IsUserBlocked(user.Username);
                            else
                                blocked = false;

                            #endregion

                            #region video icon

                            if ((Config.Misc.EnableVideoProfile && VideoProfile.HasVideoProfile(user.Username))
                                || (Config.Misc.EnableVideoUpload && VideoUpload.HasVideoUpload(user.Username)))
                                hasVideoProfile = true;

                            #endregion

                            #region private photo icon

                            if (Config.Photos.EnablePrivatePhotos && user.HasPrivatePhotos())
                                hasPrivatePhoto = true;

                            #endregion

                            #region blog icon

                            if (Config.Misc.EnableBlogs && Classes.Blog.HasPosts(user.Username))
                                hasBlog = true;

                            #endregion

                            #region video broadcast icon

                            if (Config.Misc.EnableProfileVideoBroadcast &&
                                VideoBroadcast.GetBroadcast(user.Username).HasValue)
                                isBroadcasting = true;

                            #endregion
                        }

                        string viewedOnString = null;

                        if (ShowViewedOn)
                        {
                            TimeSpan diff =
                                DateTime.Now.Subtract(User.FetchProfileViewDate(user.Username, ShowViewedOnUsername));
                            viewedOnString = User.TimespanToString(diff);
                        }
                        bool hasPhoto = photo != null && photo.Approved;
                        photoId = hasPhoto ? photo.Id : ImageHandler.GetPhotoIdByGender(user.Gender);

                        if (!hasPhoto && user.FacebookID.HasValue && user.FacebookID.Value > 0)
                        {
                            photoUrl = ImageHandler.CreateFacebookImageUrl(user.FacebookID.Value,
                                                                           ImageHandler.eFacebookImageType.Normal);
                        }
                        else
                        {
                            photoUrl = ImageHandler.CreateImageUrl(photoId,
                                                       ListPhotoWidthPx, ListPhotoWidthPx,
                                                       photoId <= 0, true, true);
                        }
                        var dummyMF = new SearchService.MutualFriendsClass { PhotoId = -99, Name = String.Empty, Username = String.Empty, PhotoUrl = String.Empty, AlreadyRequested = true };
                        var mf = new SearchService.MutualFriendsClass[3];
                        var client = new SearchService();
                        if (CurrentUserSession != null && !string.IsNullOrEmpty(CurrentUserSession.Username))
                        {
                            //var tempMF = client.GetMutualFriendsArray(CurrentUserSession.Username, user.Username, 3);
                            if (user.Username == "100000455123889") 
                            {
                                ((PageBase)Page).Log(new Exception());
                            }
                            var lstMutualFriends =
                                UsersRelationsState.GetMutualFriendsList(CurrentUserSession.Username,
                                                                         user.Username,
                                                                         ImageHandler.eFacebookImageType.Square);
                            lstMutualFriends = lstMutualFriends.Where(f => f.Name != null).ToList();
                            for (int i = 0; i < 3; i++)
                            {
                                mf[i] = (lstMutualFriends.Count < i + 1) ? dummyMF : lstMutualFriends[i];
                            }
                        }
                        else
                        {
                            mf = new[] { dummyMF, dummyMF, dummyMF };
                        }

                        string matchmakingToolTip, matchmakingOnClientClick;
                        bool isAppUser = user.LoginCount > 0;
                        //using ( var db = new ezFixUpDataContext() )
                        {
                            string displayName = CurrentUserSession.GetUserDisplayedNameByUsername(user.Username);
                            if (MatchmakerHelper.IsMatchmakerState && MatchmakerHelper.IsMatchToFriendAlredySelected)
                            {
                                matchmakingToolTip = String.Format("Match with {0}".Translate(), displayName);
                                matchmakingOnClientClick = "setFriendToMatchWith(this.id);return false;";
                            }
                            else
                            {
                                matchmakingToolTip = String.Format("Match to {0}".Translate(), displayName);
                                matchmakingOnClientClick = "setFriendToMatch(this.id);closeMutualFriendsModal();return false;";
                            }
                            var locationStr = user.LocationString.Replace(",", ",<br/>");
                            bool matchmakeIconIsVisible = (CurrentUserSession != null && user.InterestedIn != User.eGender.Matchmaker && (MatchmakerHelper.IsMatchmakerState || CurrentUserSession.IsUserInFriendList(user.Username)));
                            bool addFriendIconIsVisible = (CurrentUserSession != null && !MatchmakerHelper.IsMatchmakerState && !CurrentUserSession.IsUserInFriendList(user.Username));
                            string interestedInStr =
                                String.Format("{0}{1}", user.InterestedIn != User.eGender.Matchmaker ? "Interested in_".Translate() : ""
                                                      , interestedIn.Translate());

                            dtResults.Rows.Add(new object[]
                                                   {
                                                       displayName, user.Username, photoId, photoUrl,
                                                       slogan, age, gender.ToLower().Translate(),
                                                       interestedInStr  , 
                                                       viewedOnString, user.LastOnlineString,
                                                       ratingString, locationStr,
                                                       distance, zodiacSign1, zodiacSign2,
                                                       messageHistoryExists, verifiedByUsers, verifiedByAdmin, blocked,
                                                       hasPrivatePhoto, hasVideoProfile, hasBlog,
                                                       memberType, joinDate, isAdmin, isModerator, isVip, user.Level,
                                                       user.IsOptionEnabled(eUserOptions.DisableLevelIcon),
                                                       isBroadcasting, user.ModerationScores,
                                                       mf[0].Name, mf[1].Name, mf[2].Name,
                                                       mf[0].Username, mf[1].Username, mf[2].Username,
                                                       mf[0].PhotoId, mf[1].PhotoId, mf[2].PhotoId,
                                                       mf[0].PhotoUrl, mf[1].PhotoUrl, mf[2].PhotoUrl,
                                                       mf[0].AlreadyRequested ? "MfPopIconsBg1" : "MfPopIconsBg0",
                                                       mf[1].AlreadyRequested ? "MfPopIconsBg1" : "MfPopIconsBg0",
                                                       mf[2].AlreadyRequested ? "MfPopIconsBg1" : "MfPopIconsBg0",
                                                       String.Format("jQuery.get('./handlers/AddRemoveConnection.ashx?cmd=add&type=friend&id={0}', function(data){1});return false;", 
                                                                    user.Username, "{$('#actionResultPopup').html(data);$('#actionResultPopup').dialog('open');}"),
                                                       String.Format("jQuery.get('./handlers/AddRemoveConnection.ashx?cmd=add&type=favorite&id={0}', function(data){1});return false;", 
                                                                    user.Username, "{$('#actionResultPopup').html(data);$('#actionResultPopup').dialog('open');}"),
                                                       
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}');$('#sendMessageModal').dialog('open');return false;", user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=gift');$('#sendMessageModal').dialog('open');return false;", user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=wink');$('#sendMessageModal').dialog('open');return false;", user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=ask&about={1}');$('#sendMessageModal').dialog('open');return false;", 
                                                                     mf[0].Username, user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=ask&about={1}');$('#sendMessageModal').dialog('open');return false;", 
                                                                     mf[1].Username, user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=ask&about={1}');$('#sendMessageModal').dialog('open');return false;", 
                                                                     mf[2].Username, user.Username),
                                                       //(isAppUser && MatchmakerHelper.IsMatchmakerState) || (CurrentUserSession != null && CurrentUserSession.IsUserInFriendList(user.Username)),
                                                       matchmakeIconIsVisible,
                                                       addFriendIconIsVisible,
                                                       user.InterestedIn != User.eGender.Matchmaker,
                                                       matchmakingToolTip,
                                                       matchmakingOnClientClick
                                                   });
                        }
                    }

                }
            }
            if (dlUsers.Visible)
            {
                dlUsers.DataSource = dtResults;
                dlUsers.DataBind();
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage = 1;
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkNext_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect(MatchmakerHelper.CurrentHomePage);

            int usersPerPage = dlUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
            if (Results != null)
                if (CurrentPage < Results.GetTotalPages(usersPerPage))
                {
                    CurrentPage++;
                }
        }

        /// <summary>
        /// Handles the Click event of the lnkLast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkLast_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect(MatchmakerHelper.CurrentHomePage);

            int usersPerPage = dlUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
            if (Results != null)
                if (CurrentPage < Results.GetTotalPages(usersPerPage))
                {
                    CurrentPage = Results.GetTotalPages(usersPerPage);
                }
        }

        protected void lnkShowDetails_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            dlUsers.Visible = true;
            GridMode = false;
        }
        protected void dlUsers_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var pnlIcons = (HtmlGenericControl)e.Item.FindControl("pnlIcons");

            if (showIcons)
            {
                pnlIcons.Visible = true;
            }
            else
                pnlIcons.Visible = false;
        }

        protected void dlUsers_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            var pnlManageGroupMembers = e.Item.FindControl("pnlManageGroupMembers") as HtmlGenericControl;
            if (pnlManageGroupMembers != null)
                pnlManageGroupMembers.Visible = false;

            if (GroupID == null) // execute this event handler only if this control is used in the group section
            {
                return;
            }

            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            if (CurrentUserSession != null)
            {
                var lnkMakeAdmin = (LinkButton)e.Item.FindControl("lnkMakeAdmin");
                var lnkRemoveAdmin = (LinkButton)e.Item.FindControl("lnkRemoveAdmin");
                var lnkMakeModerator = (LinkButton)e.Item.FindControl("lnkMakeModerator");
                var lnkRemoveModerator = (LinkButton)e.Item.FindControl("lnkRemoveModerator");
                var lnkMakeVip = (LinkButton)e.Item.FindControl("lnkMakeVip");
                var lnkRemoveVip = (LinkButton)e.Item.FindControl("lnkRemoveVip");
                var lnkDeleteMember = (LinkButton)e.Item.FindControl("lnkDeleteMember");

                lnkMakeAdmin.Text = Lang.Trans("Make Administrator");
                lnkRemoveAdmin.Text = Lang.Trans("Remove as Administrator");
                lnkMakeModerator.Text = Lang.Trans("Make Moderator");
                lnkRemoveModerator.Text = Lang.Trans("Remove as Moderator");
                lnkMakeVip.Text = Lang.Trans("Make V.I.P.");
                lnkRemoveVip.Text = Lang.Trans("Remove as V.I.P.");
                lnkDeleteMember.Text = Lang.Trans("Delete Member");

                var liMakeAdmin = (HtmlGenericControl)e.Item.FindControl("liMakeAdmin");
                var liRemoveAdmin = (HtmlGenericControl)e.Item.FindControl("liRemoveAdmin");
                var liMakeModerator = (HtmlGenericControl)e.Item.FindControl("liMakeModerator");
                var liRemoveModerator = (HtmlGenericControl)e.Item.FindControl("liRemoveModerator");
                var liMakeVip = (HtmlGenericControl)e.Item.FindControl("liMakeVip");
                var liRemoveVip = (HtmlGenericControl)e.Item.FindControl("liRemoveVip");
                var liDeleteMember = (HtmlGenericControl)e.Item.FindControl("liDeleteMember");

                liMakeAdmin.Visible = false;
                liRemoveAdmin.Visible = false;
                liMakeModerator.Visible = false;
                liRemoveModerator.Visible = false;
                liMakeVip.Visible = false;
                liRemoveVip.Visible = false;
                liDeleteMember.Visible = false;
            }
        }

        void scriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            if (Results == null)
                Response.Redirect(MatchmakerHelper.CurrentHomePage);

            int navigatePage;
            try
            {
                navigatePage = e.State.Count == 0 ? 1 : Convert.ToInt32(e.State[0]);
            }
            catch (FormatException)
            {
                navigatePage = 1;
            }
            int usersPerPage = dlUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
            if (navigatePage <= Results.GetTotalPages(usersPerPage)
                && navigatePage > 0)
            {
                updateHistory = false;
                CurrentPage = navigatePage;
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lnkFirst.Click += new EventHandler(lnkFirst_Click);
            this.lnkPrev.Click += new EventHandler(lnkPrev_Click);
            this.lnkNext.Click += new EventHandler(lnkNext_Click);
            this.lnkLast.Click += new EventHandler(lnkLast_Click);
        }

        #endregion

        protected void dlUsersGrid_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (Config.Users.DisableAgeInformation && Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlGenderAge = (HtmlGenericControl)e.Item.FindControl("pnlGenderAge");
                pnlGenderAge.Visible = false;
            }
            else if (Config.Users.DisableAgeInformation || Config.Users.DisableGenderInformation)
            {
                HtmlGenericControl pnlDelimiter = (HtmlGenericControl)e.Item.FindControl("pnlDelimiter");
                pnlDelimiter.Visible = false;
            }
        }

        protected void UserItemClicked(object sender, ImageClickEventArgs e)
        {
            //ModalPopupExtender1.DynamicContextKey = Label2.Text;
        }

        protected void DialogButtonSendWink_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        protected void LoadNewMembers()
        {
            NewUsersSearch search = new NewUsersSearch
                                        {
                                            Gender = CurrentUserSession.InterestedIn,
                                            MaxAge =
                                                Math.Min(CurrentUserSession.IncomingMessagesRestrictions.AgeTo,
                                                         Config.Users.MaxAge),
                                            MinAge =
                                                Math.Max(CurrentUserSession.IncomingMessagesRestrictions.AgeFrom,
                                                         Config.Users.MinAge),
                                            UsersCount = RepeatColumns * LimitRows,
                                            LoginCountThreshold = 1,
                                            PageNumber = 0
                                        };
            Results = search.GetResults();
        }
        protected void LoadMySingleFriends(User.eGenderSearch gender)
        {
            var user = PageBase.GetCurrentUserSession();
            var genderFriendsSearch = new QuickSingleFriendsSearch
            {
                UsersCount = RepeatColumns * LimitRows,
                PageNumber = 0,
                MaxAge = Math.Min(user.IncomingMessagesRestrictions.AgeTo, Config.Users.MaxAge),
                MinAge = Math.Max(user.IncomingMessagesRestrictions.AgeFrom, Config.Users.MinAge),
                MatchmakerUsername = user.Username,
                FriendGender = gender
            };
            Results = genderFriendsSearch.GetResults();
        }
        protected void LoadMySingleMaleFriends()
        {
            LoadMySingleFriends(User.eGenderSearch.Male);
        }
        protected void LoadMySingleFemaleFriends()
        {
            LoadMySingleFriends(User.eGenderSearch.Female);
        }
        protected void LoadAllMySingleFriends()
        {
            LoadMySingleFriends(User.eGenderSearch.All);
        }
        protected void LoadMyFavorite()
        {
            var user = CurrentUserSession;
            if (user != null)
                Results.Usernames = user.FetchFavouriteUsers().Select(x => x.Username).ToArray();
        }
        protected void LoadAllMyFriends()
        {
            var user = CurrentUserSession;
            if (user != null)
                Results.Usernames = user.UserFriends.Select(x => x.Username).ToArray();
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}