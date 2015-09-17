/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Model;
using AspNetDating.Services;
using Group = AspNetDating.Classes.Group;
using GroupMember = AspNetDating.Classes.GroupMember;
using Message = AspNetDating.Classes.Message;
using Photo = AspNetDating.Classes.Photo;
using PhotoRating = AspNetDating.Classes.PhotoRating;
using ProfileAnswer = AspNetDating.Classes.ProfileAnswer;
using User = AspNetDating.Classes.User;
using UserLevel = AspNetDating.Classes.UserLevel;
using VideoProfile = AspNetDating.Classes.VideoProfile;
using VideoUpload = AspNetDating.Classes.VideoUpload;


namespace AspNetDating.Components.Search
{
    public partial class SearchResults1 : UserControl
    {
        private Group currentGroup;
        private GroupMember currentGroupMember;
        public int ResultLayoutWidth = 660;
        public int ResultLayoutHeight;
        public int GridColumnCount = 5;
        public int GridRowCount = 6;
        public int ListPhotoWidthPx { get { return 163;/* ResultLayoutWidth * (100 / GridColumnCount - 2) / 100;*/ } }
        public int GridBorderPx = 2;

        /// <summary>
        /// Sets a value indicating whether [paginator enabled].
        /// </summary>
        /// <value><c>true</c> if [paginator enabled]; otherwise, <c>false</c>.</value>
        private bool paginatorVisible = true;

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

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        //public UserSearchResults Results
        public FacebookAndUsersMergeResults Results
        {
            set
            {
                if (ViewState["SearchResults_guid"] == null)
                {
                    ViewState["SearchResults_guid"] = Guid.NewGuid().ToString();
                }
                if (value != null)
                {
                    int count = 0;
                    if (value.Usernames != null)
                        count += value.Usernames.Length;
                    if (value.FacebookIds != null)
                        count += value.FacebookIds.Length;
                    if (count == 0)
                        value = null;
                }
                Session["SearchResults" + ViewState["SearchResults_guid"]] = value;
                //CurrentPage = 1;
            }
            get
            {
                if (ViewState["SearchResults_guid"] != null)
                {
                    return (FacebookAndUsersMergeResults)
                           Session["SearchResults" + ViewState["SearchResults_guid"]];
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
                Trace.Write("SearchResults.ascx.cs", "CurrentPage = " + value);
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
            get { return paginatorVisible; }
            set
            {
                paginatorVisible = value;
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

        private Group CurrentGroup
        {
            get
            {
                if (GroupID != null)
                {
                    if (currentGroup == null)
                    {
                        currentGroup = Group.Fetch(GroupID.Value);
                    }
                }

                return currentGroup;
            }
        }

        private GroupMember CurrentGroupMember
        {
            get
            {
                if (GroupID != null && CurrentUserSession != null)
                {
                    if (currentGroupMember == null)
                    {
                        currentGroupMember = GroupMember.Fetch(GroupID.Value, CurrentUserSession.Username);
                    }
                }

                return currentGroupMember;
            }
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
            set { Session["SearchResults_GridMode"] = value; }
            get
            {
                if (Session["SearchResults_GridMode"] is bool)
                    return (bool)Session["SearchResults_GridMode"];
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
            Trace.Write("SearchResults.ascx.cs", "Page_Load");
            ShowLastOnline = false;
            dlUsersGrid.RepeatColumns = GridColumnCount;
            GridRowCount = Config.Search.UsersPerPageGrid / GridColumnCount;
            ResultLayoutHeight = GridRowCount * (3 * GridBorderPx + 10 * ResultLayoutWidth / 9);
            dlUsersGrid.Width = new Unit((ResultLayoutWidth - GridBorderPx * (3 * GridColumnCount + 1)) / GridColumnCount);
            if (!Page.IsPostBack)
            {
                LoadStrings();
            }
            dlUsersGrid.Visible = GridMode;
            dlUsers.Visible = !GridMode;
            lnkShowGrid.Enabled = !GridMode;
            lnkShowDetails.Enabled = GridMode;
            divSwitchModes.Visible = EnableGridSupport;
            lnkShowGrid.CssClass = GridMode ? "showPushedBtn" : "showReleasedBtn";
            lnkShowDetails.CssClass = !GridMode ? "showPushedBtn" : "showReleasedBtn";

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
        }

        private void SetPaginatorButtonsCSS()
        {
            if (Session["Direction"] != null && Session["Direction"].Equals("rtl"))
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
            //lnkFirst.CssClass += lnkFirst.Enabled ? "" : " opacityDisabled";
            //lnkPrev.CssClass += lnkPrev.Enabled ? "" : " opacityDisabled";
            //lnkNext.CssClass += lnkNext.Enabled ? "" : " opacityDisabled";
            //lnkLast.CssClass += lnkLast.Enabled ? "" : " opacityDisabled";

            lnkFirst.Visible = lnkFirst.Enabled;
            lnkPrev.Visible = lnkPrev.Enabled;
            lnkNext.Visible = lnkNext.Enabled;
            lnkLast.Visible = lnkLast.Enabled;
        }

        protected void OnPreRender(EventArgs e)
        {
            LoadResultsPage();

            if (PaginatorEnabled)
            {
                PreparePaginator();
            }
        }
        public void OnPagePreRenderComplete()
        {
            OnPreRender(null);
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
            //lbModalDenyFriend.Visible = isFriendsPage;
        }

        public event EventHandler DeleteMemberClick;

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
                var length = Results.Usernames.Length;
                if (Results.FacebookIds != null) length += Results.FacebookIds.Length;
                int fromUser = (CurrentPage - 1) * usersPerPage + 1;
                int toUser = CurrentPage * usersPerPage;
                if (length < toUser)
                {
                    toUser = length;
                }
                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, length);
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
                    users = Results.Get();
                }

                if (users != null && users.Length > 0)
                {
                    Location from = null;

                    if (CurrentUserSession != null)
                    {
                        from = Config.Users.GetLocation(CurrentUserSession);
                        //ZipCode.DoLookupByZipCode(CurrentUserSession.ZipCode);    
                    }

                    bool calculateDistance = from == null ? false : true;

                    //var appUsers = users.Where(u => u.Password.IsNotNullOrEmpty());
                    //var fbUsers = users.Where(u => u.Password.IsNullOrEmpty());
                    //var appUsers = users.Where(u => u.LoginCount > 0);
                    //var fbUsers = users.Where(u => u.LoginCount == 0);

                    int photoId = 0;
                    string photoUrl = String.Empty;

                    foreach (User user in users)
                    {
                        Photo photo = null;
                        string slogan = "";
                        string ratingString = "";
                        string age = "";
                        string distance = "";
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
                        string viewedOnString = "";
                        var dummyMF = new SearchService.MutualFriendsClass { PhotoId = -99, Username = String.Empty, Name = String.Empty, PhotoUrl = String.Empty, AlreadyRequested = true };
                        var mf = new SearchService.MutualFriendsClass[3] { dummyMF, dummyMF, dummyMF };
                        string matchmakingToolTip, matchmakingOnClientClick;

                        //bool isAppUser = appUsers.Contains(user);

                        //if (isAppUser)
                        {
                            #region Gets User Photo

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

                            #region Show Distance

                            if (calculateDistance)
                            {
                                if (Config.Search.ShowDistanceFromOnlineUser && CurrentUserSession != null &&
                                    ShowDistance)
                                {
                                    //                             ShowDistance = true;
                                    Location to = Config.Users.GetLocation(user);

                                    //ZipCode.DoLookupByZipCode(user.ZipCode);

                                    if (to != null && user.Username != CurrentUserSession.Username)
                                    // location exist in DB
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

                            if (ShowViewedOn)
                            {
                                TimeSpan diff =
                                    DateTime.Now.Subtract(User.FetchProfileViewDate(user.Username, ShowViewedOnUsername));
                                viewedOnString = User.TimespanToString(diff);
                            }

                            if (photo == null || !photo.Approved)
                            {
                                photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                            }
                            else
                            {
                                photoId = photo.Id;
                            }
                            if (dlUsersGrid.Visible)
                            {
                                photoUrl = ImageHandler.CreateImageUrl(photoId,
                                                   (int)dlUsersGrid.Width.Value,
                                                   (int)dlUsersGrid.Width.Value,
                                                   photoId < 0, true, true);
                            }
                            else
                            {
                                photoUrl = ImageHandler.CreateImageUrl(photoId,
                                                   ListPhotoWidthPx, ListPhotoWidthPx,
                                                   photoId < 0, true, true);
                            }

                            #region set group member data

                            if (GroupID != null) // it should be set if this control is used in the groups section
                            {
                                GroupMember groupMember = GroupMember.Fetch(GroupID.Value, user.Username);

                                if (groupMember != null && CurrentGroup != null)
                                {
                                    memberType = groupMember.Type.ToString();
                                    joinDate = groupMember.JoinDate.Add(Config.Misc.TimeOffset).ToShortDateString();
                                    isAdmin = groupMember.Type == GroupMember.eType.Admin ? true : false;
                                    isModerator = groupMember.Type == GroupMember.eType.Moderator ? true : false;
                                    isVip = groupMember.Type == GroupMember.eType.VIP ? true : false;

                                    if (CurrentGroup.Owner == groupMember.Username)
                                    {
                                        //do not Lang.Trans this ... this is done in the ascx file
                                        memberType = "Owner";
                                    }
                                }
                            }

                            #endregion
                        }
                        //else //if (fbUsers.Contains(user))
                        if (photoId < 0 && user.FacebookID.HasValue)
                        {
                            photoUrl = String.Format("https://graph.facebook.com/{0}/picture?type={1}",
                                                user.FacebookID, Config.FacebookSettings.FacebookDefaultImageIsNormal
                                                                            ? AspNetDating.ImageHandler.eFacebookImageType.Normal.ToString().ToLower()
                                                                            : AspNetDating.ImageHandler.eFacebookImageType.Large.ToString().ToLower());
                        }
                        bool isAppUser = user.LoginCount > 0;
                        if (!isAppUser)
                        {
                            matchmakingToolTip = "";
                            matchmakingOnClientClick = "return false;";
                        }
                        var lstMutualFriends = CurrentUserSession != null
                            ? UsersRelationsState.GetMutualFriendsList(CurrentUserSession.Username, user.Username, ImageHandler.eFacebookImageType.Square)
                            : new List<SearchService.MutualFriendsClass>();
                        lstMutualFriends = lstMutualFriends.Where(f => f.Name != null).ToList();
                        for (int i = 0; i < 3; i++)
                        {
                            mf[i] = (i < lstMutualFriends.Count) ? lstMutualFriends[i] : dummyMF;
                        }

                        //var client = new SearchService();
                        //if (CurrentUserSession != null && !string.IsNullOrEmpty(CurrentUserSession.Username))
                        //{
                        //    var appMF = client.GetMutualFriendsArray(CurrentUserSession.Username, user.Username, 3);

                        //    for (int i = 0; i < 3; i++)
                        //    {
                        //        mf[i] = (appMF.Length < i + 1) ? dummyMF : appMF[i];
                        //    }
                        //    if (appMF.Length < 3 && CurrentUserSession.FacebookID != null && user.FacebookID != null)
                        //    {
                        //        var fbMF = client.GetFacebookMutualFriendsArray(CurrentUserSession.Username,
                        //                                                        user.Username, 3 - appMF.Length);
                        //        for (int i = appMF.Length; i < 3; i++)
                        //        {
                        //            mf[i] = (fbMF.Length < i + 1) ? dummyMF : fbMF[i];
                        //        }
                        //    }
                        //}
                        #region User's gender

                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        string interestedIn = !Config.Users.DisableGenderInformation ? user.InterestedIn.ToString() : String.Empty;
                        
                        #endregion

                        var currentSessionUsername = CurrentUserSession != null
                                                         ? CurrentUserSession.Username
                                                         : String.Empty;

                        //using (var db = new AspNetDatingDataContext())
                        {
                            string displayName = UserSession.GetViewedUserDisplayedNameByUsername(currentSessionUsername, user.Username);
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
                                                       viewedOnString,
                                                       isAppUser ? user.LastOnlineString : "N/A".Translate(),
                                                       ratingString, user.LocationString,
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
                                                                    user.Username, "{$('#actionResultMessage').html(data);$('#actionResultPopup').dialog('open');}"),
                                                       String.Format("jQuery.get('./handlers/AddRemoveConnection.ashx?cmd=add&type=favorite&id={0}', function(data){1});return false;", 
                                                                    user.Username, "{$('#actionResultMessage').html(data);$('#actionResultPopup').dialog('open');}"),
                                                       
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}');$('#sendMessageModal').dialog('open');return false;", user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=gift');$('#sendMessageModal').dialog('open');return false;", user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=wink');$('#sendMessageModal').dialog('open');return false;", user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=ask&about={1}');$('#sendMessageModal').dialog('open');return false;", 
                                                                     mf[0].Username, user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=ask&about={1}');$('#sendMessageModal').dialog('open');return false;", 
                                                                     mf[1].Username, user.Username),
                                                       String.Format("$('#sendMessageFrame').attr('src', './SendMessagePopup.aspx?to_user={0}&type=ask&about={1}');$('#sendMessageModal').dialog('open');return false;", 
                                                                     mf[2].Username, user.Username),
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
            else
            {
                divSwitchModes.Visible = false;
            }

            if (dlUsers.Visible)
            {
                dlUsers.DataSource = dtResults;
                dlUsers.DataBind();
            }
            else if (dlUsersGrid.Visible)
            {
                dlUsersGrid.DataSource = dtResults;
                dlUsersGrid.DataBind();
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
            if (CurrentPage < Results.GetTotalPages(usersPerPage))
            {
                CurrentPage = Results.GetTotalPages(usersPerPage);
            }
        }

        protected void lnkShowGrid_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            lnkShowGrid.BorderStyle = BorderStyle.Inset;
            lnkShowDetails.BorderStyle = BorderStyle.Outset;
            lnkShowGrid.CssClass = "showPushedBtn";
            lnkShowDetails.CssClass = "showReleasedBtn";
            dlUsersGrid.Visible = true;
            dlUsers.Visible = false;
            lnkShowGrid.Enabled = false;
            lnkShowDetails.Enabled = true;
            GridMode = true;
        }

        protected void lnkShowDetails_Click(object sender, EventArgs e)
        {
            CurrentPage = 1;
            lnkShowDetails.BorderStyle = BorderStyle.Inset;
            lnkShowGrid.BorderStyle = BorderStyle.Outset;
            lnkShowDetails.CssClass = "showPushedBtn";
            lnkShowGrid.CssClass = "showReleasedBtn";
            dlUsersGrid.Visible = false;
            dlUsers.Visible = true;
            lnkShowGrid.Enabled = true;
            lnkShowDetails.Enabled = false;
            GridMode = false;
        }

        protected void dlUsers_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (CurrentUserSession == null || (CurrentGroupMember == null && !CurrentUserSession.IsAdmin())
                || (CurrentGroupMember != null
                    && CurrentGroupMember.Type != GroupMember.eType.Admin
                    && CurrentGroupMember.Type != GroupMember.eType.Moderator))
                return;

            GroupMember groupMember = GroupMember.Fetch(GroupID.Value, (string)e.CommandArgument);

            if (groupMember != null)
            {
                if (CurrentGroupMember != null && CurrentGroupMember.Type == GroupMember.eType.Admin)
                {
                    switch (e.CommandName)
                    {
                        case "MakeAdmin":
                            groupMember.Type = GroupMember.eType.Admin;
                            groupMember.Save();
                            break;

                        case "RemoveAdmin":
                            groupMember.Type = GroupMember.eType.Member;
                            groupMember.Save();
                            break;

                        case "MakeModerator":
                            groupMember.Type = GroupMember.eType.Moderator;
                            groupMember.Save();
                            break;

                        case "RemoveModerator":
                            groupMember.Type = GroupMember.eType.Member;
                            groupMember.Save();
                            break;
                    }
                }

                switch (e.CommandName)
                {
                    case "MakeVip":
                        groupMember.Type = GroupMember.eType.VIP;
                        groupMember.Save();
                        break;

                    case "RemoveVip":
                        groupMember.Type = GroupMember.eType.Member;
                        groupMember.Save();
                        break;

                    case "DeleteMember":
                        OnDeleteMemberClick(e);
                        break;
                }
            }
        }

        protected void dlUsers_ItemCreated(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            //var askAbout1 = (LinkButton)e.Item.FindControl("lbModalMF_AskAbout1");
            //var askAbout2 = (HtmlGenericControl)e.Item.FindControl("lbModalMF_AskAbout2");
            //var askAbout3 = (HtmlGenericControl)e.Item.FindControl("lbModalMF_AskAbout3");

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

                var isAdmin = (bool)DataBinder.Eval(e.Item.DataItem, "GroupMemberIsAdmin");
                var isModerator = (bool)DataBinder.Eval(e.Item.DataItem, "GroupMemberIsModerator");
                var isVip = (bool)DataBinder.Eval(e.Item.DataItem, "GroupMemberIsVip");

                if (CurrentGroupMember != null)
                {
                    if (CurrentGroup != null && CurrentGroupMember.Username == CurrentGroup.Owner)
                    {
                        if (CurrentGroupMember.Username == (string)DataBinder.Eval(e.Item.DataItem, "Username"))
                        {
                            liMakeAdmin.Visible = false;
                            liRemoveAdmin.Visible = false;
                            lnkMakeModerator.Visible = false;
                            liRemoveModerator.Visible = false;
                            liMakeVip.Visible = false;
                            liRemoveVip.Visible = false;
                            liDeleteMember.Visible = false;
                        }
                        else if (isAdmin)
                        {
                            pnlManageGroupMembers.Visible = true;
                            liRemoveAdmin.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                        else if (isModerator)
                        {
                            pnlManageGroupMembers.Visible = true;
                            liRemoveModerator.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                        else if (isVip)
                        {
                            pnlManageGroupMembers.Visible = true;
                            liRemoveVip.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                        else
                        {
                            pnlManageGroupMembers.Visible = true;
                            liMakeAdmin.Visible = true;
                            liMakeModerator.Visible = true;
                            liMakeVip.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                    }
                    else if (CurrentGroupMember.Type == GroupMember.eType.Admin)
                    {
                        if (isAdmin)
                        {
                        }
                        else if (isModerator)
                        {
                            pnlManageGroupMembers.Visible = true;
                            liRemoveModerator.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                        else if (isVip)
                        {
                            pnlManageGroupMembers.Visible = true;
                            liRemoveVip.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                        else
                        {
                            pnlManageGroupMembers.Visible = true;
                            liMakeModerator.Visible = true;
                            liMakeVip.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                    }
                    else if (CurrentGroupMember.Type == GroupMember.eType.Moderator)
                    {
                        if (!isAdmin && !isModerator)
                        {
                            pnlManageGroupMembers.Visible = true;
                            liDeleteMember.Visible = true;
                        }
                    }
                }
            }
        }

        protected void OnDeleteMemberClick(EventArgs e)
        {
            if (DeleteMemberClick != null)
            {
                DeleteMemberClick(this, e);
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

        //protected void BlockBtn_Clicked(object sender, EventArgs e)
        //{
        //    if (CurrentUserSession == null)
        //        Response.Redirect("~/Login.aspx");
        //    else
        //    {
        //        string viewed = hfViewed.Value.Trim();
        //        if (CurrentUserSession.IsUserBlocked(viewed))
        //        {
        //            CurrentUserSession.UnblockUser(viewed);
        //            lbModalBlock.ToolTip = Lang.Trans("Block user");
        //        }
        //        else
        //        {
        //            CurrentUserSession.BlockUser(viewed);
        //            lbModalBlock.ToolTip = Lang.Trans("Unblock user");
        //        }
        //    }
        //}

        protected void DialogButtonSendWink_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}