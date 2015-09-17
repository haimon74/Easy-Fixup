using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using System.Configuration;
using System.IO;


namespace AspNetDating
{
    public partial class Friends3 : PageBase
    {
        private List<ProfileQuestion> profileQuestions;
        private Dictionary<int, object> dicQuestions;
        private FetchUsernamesResultsAsync asyncFetchUsernamesState;
        private HttpContext itsContext;


        public Friends3()
        {
            RequiresAuthorization = false;
            asyncFetchUsernamesState = new FetchUsernamesResultsAsync { ItsFriendsPageContext = this };
            itsContext = HttpContext.Current;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if ((comboAgeFrom.Items.Count < 2 || comboAgeTo.Items.Count < 2))
            {
                PopulateAgeCombos(null);
            }

            if (!IsPostBack)
            {
                //var userSession = PageBase.GetCurrentUserSession();
                //StringWriter sw = userSession.SessionPagesDic["Friends3.aspx"];
                //string outputStr = sw != null ? sw.ToString() : null;

                //if (false && sw != null && outputStr.IsNotNullOrEmpty())
                //{
                //    Response.Clear();
                //    Response.Write(outputStr);
                //    sw.Flush();
                //    sw.Close();
                //    userSession.SessionPagesDic["Friends3.aspx"] = new StringWriter();
                //    Response.End();
                //}
                //else
                    PopulateGenderCombos(((PageBase)Page).CurrentUserSession);
            }


            MatchToFriend1.IsVisible = MatchmakerHelper.IsMatchmakerState;

            if (CurrentUserSession == null)
                Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

            if (Config.Users.CompletedProfileRequiredToBrowseSearch && CurrentUserSession != null &&
                    !CurrentUserSession.HasProfile && !CurrentUserSession.IsAdmin())
                Response.Redirect("ManageProfile.aspx?err=profnotcompl");

            ControlsLogicalGroupsDic =
                new Dictionary<string, List<Control>>
                    {
                        {"SideMenuLinks", (new[] {lnkShowFavorites, lnkQuickFriendsSearch, lnkShowBlockedUsers, lnkShowOnlineFriends, lnkShowFavorites, lnkShowBlockedUsers}).ToList<Control>()},
                        {"HideBlockedUsersLink", (new[] {lnkQuickFriendsSearch, lnkShowOnlineFriends, lnkShowFavorites, lnkShowBlockedUsers}).ToList<Control>()}
                    };
            var task = new PageAsyncTask(AsyncWorkBeginMethod, AsyncWorkEndMethod,
                                         null, asyncFetchUsernamesState, true);
            RegisterAsyncTask(task);
            base.Page_Load(sender, e);
        }


        protected override IAsyncResult AsyncWorkBeginMethod(object sender, EventArgs e, AsyncCallback cb, object state)
        {
            var result = ((FetchUsernamesResultsAsync)state).ItsDelegate.BeginInvoke(cb, state);
            return result;
        }
        protected override void AsyncWorkEndMethod(IAsyncResult asyncResult)
        {
            var asyncState = (FetchUsernamesResultsAsync)asyncResult.AsyncState;
            //asyncState.ItsFriendsPageContext.DataBind();
            //asyncState.ItsSearchPageContext.SearchResults.Results.Usernames = asyncState.Results;
        }
        protected override void OnPreRenderComplete(EventArgs e)
        {
            SearchResults.OnPagePreRenderComplete();

            base.OnPreRenderComplete(e);
        }

        public class FetchUsernamesResultsAsync
        {
            public Friends3 ItsFriendsPageContext;
            public AsyncTaskDelegate ItsDelegate;
            public delegate void AsyncTaskDelegate();

            public FetchUsernamesResultsAsync()
            {
                ItsDelegate = new AsyncTaskDelegate(ExecuteAsyncTask);
            }

            protected void ExecuteAsyncTask()
            {
                lock (this.ItsFriendsPageContext.SearchResults)
                {
                    if (!((Site)ItsFriendsPageContext.Page.Master).ScriptManager.IsInAsyncPostBack)
                    {
                        ItsFriendsPageContext.LoadStrings();
                        //ItsFriendsPageContext.PrepareDefaultFields();
                        ItsFriendsPageContext.Session["advancedSearch"] = false;
                        //ItsFriendsPageContext.btnSearch_Click(null, null);
                        string startLocation = ItsFriendsPageContext.Request.Params["loc"];
                        if (ItsFriendsPageContext.Session["MatchToUsername"] != null)
                            startLocation = String.Format("single{0}s",
                                Classes.User.Load(ItsFriendsPageContext.Session["MatchToUsername"].ToString()).InterestedIn.ToString().ToLower());
                        bool asFavoritesPage = false;
                        switch (startLocation)
                        {
                            //case "online":
                            //    lnkShowOnlineFriends_Click(lnkShowOnlineFriends, null);
                            //    break;
                            case "requests":
                                ItsFriendsPageContext.lnkFriendsRequests_Click(ItsFriendsPageContext.lnkFriendsRequests, null);
                                break;
                            case "favorites":
                                ItsFriendsPageContext.lnkShowFavorites_Click(ItsFriendsPageContext.lnkShowFavorites, null);
                                asFavoritesPage = true;
                                break;
                            case "blocked":
                                ItsFriendsPageContext.lnkShowBlockedUsers_Click(ItsFriendsPageContext.lnkShowBlockedUsers, null);
                                asFavoritesPage = true;
                                break;
                            case "singlemales":
                                ItsFriendsPageContext.MySingleFriends(Classes.User.eGenderSearch.Male);
                                break;
                            case "singlefemales":
                                ItsFriendsPageContext.MySingleFriends(Classes.User.eGenderSearch.Female);
                                break;
                            case "friends":
                            default:
                                ItsFriendsPageContext.lnkQuickSearch_Click(ItsFriendsPageContext.lnkQuickFriendsSearch, null);
                                break;
                        }

                        ItsFriendsPageContext.liShowFavorites.Visible = asFavoritesPage;
                        ItsFriendsPageContext.panelShowBlockedUsers.Visible = asFavoritesPage;
                        ItsFriendsPageContext.liShowFriends.Visible = !asFavoritesPage;
                        ItsFriendsPageContext.liShowFriendsRequests.Visible = !asFavoritesPage;
                        ItsFriendsPageContext.liShowSingleMaleFriends.Visible = false;//!asFavoritesPage;
                        ItsFriendsPageContext.liShowSingleFemaleFriends.Visible = false;//!asFavoritesPage;
                    }
                }
            }
        }


        private void MySingleFriends(User.eGenderSearch gender)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick(lnkQuickFriendsSearch,
                "Quick Friends Search",
                "Friends Search Results", false);

            if (CurrentUserSession == null) return;

            var search = new QuickSingleFriendsSearch
                             {
                                 MatchMaker = CurrentUserSession,
                                 MatchmakerUsername = CurrentUserSession.Username,
                                 FriendGender = gender
                             };
            //if (CurrentUserSession != null) search.ExcludeUsername.Add(CurrentUserSession.Username);
            //if (MatchmakerHelper.IsMatchToFriendAlredySelected) search.ExcludeUsername.Add(MatchmakerHelper.MatchToUsername);            
            SearchResults.Results = new FacebookAndUsersMergeResults
                                        {
                                            Usernames = search.GetResults().Usernames,
                                            FacebookIds = null
                                        };
        }

        private void PrepareDefaultFields()
        {
            PopulateGenderCombos(CurrentUserSession);
            PopulateAgeCombos(CurrentUserSession);
        }

        private void LoadStrings()
        {
            CascadingDropDownCity.LoadingText = "[Loading cities...]".Translate();
            int langId = (int)(this.itsContext.Session["LanguageId"] ?? Config.Misc.DefaultLanguageId);
            SmallBoxStart1.Title = Lang.Trans(langId, "Quick Friends Search", false);
            LargeBoxStart1.Title = Lang.Trans(langId, "Friends Results List", false);
            cbPhotoReq.Text = Lang.Trans(langId, "With Photo", false);
            btnSearch.Text = Lang.Trans(langId, "Search", false);
            cbOnlineOnly.Text = Lang.Trans(langId, "Online Users", false);
            lnkQuickFriendsSearch.Text = Lang.Trans(langId, "Friends List", false);
            lnkFriendsRequests.Text = Lang.Trans(langId, "Friends Requests", false);
            lnkShowOnlineFriends.Text = Lang.Trans(langId, "Online Friends", false);
            lnkShowFavorites.Text = Lang.Trans(langId, "Favorite Users", false);
            lnkShowBlockedUsers.Text = Lang.Trans(langId, "Blocked Users", false);
            SmallBoxStart2.Title = Lang.Trans(langId, "Search Options", false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            bool isUsernameSearch = !IsPostBack || txtUsername.Text.Trim().Length > 0;
            BasicFriendsSearch search = new BasicFriendsSearch
                                            {
                                                Username = CurrentUserSession.Username,
                                                FriendUsername = isUsernameSearch ? txtUsername.Text.Trim() : null
                                            };

            if (Config.Users.RequireProfileToShowInSearch)
            {
                search.HasAnswer = true;
            }
            else
            {
                search.hasAnswer_isSet = false;
            }
            if (!Config.Users.DisableGenderInformation)
            {
                search.Gender = (User.eGender)this.LookingForGender;
                //if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
                //    search.InterestedIn = (User.eGender) SearchGenderPicker1.InterestedInGender;
            }
            if (!Config.Users.DisableAgeInformation)
            {
                try
                {
                    search.MinAge = this.AgeFrom;
                    search.MaxAge = this.AgeTo;
                }
                catch
                {
                }
            }
            if (Config.Users.LocationPanelVisible)
            {
                {
                    search.Country = (this.Country.IsNullOrEmpty()) ? "" : this.Country;
                    search.State = (this.Region.IsNullOrEmpty()) ? "" : this.Region;
                    search.City = (this.City.IsNullOrEmpty()) ? "" : this.City;
                }
            }

            if (cbPhotoReq.Checked)
                search.HasPhoto = true;

            if (CurrentUserSession != null) search.ExcludeUsername.Add(CurrentUserSession.Username);
            if (MatchmakerHelper.IsMatchToFriendAlredySelected) search.ExcludeUsername.Add(MatchmakerHelper.MatchToUsername);
            var results = new FacebookAndUsersMergeResults
                           {
                               Usernames = search.GetResults().Usernames,
                               FacebookIds = null
                           };
            SearchResults.Results = results;
            SetSearchresultViewableProperties();
        }

        protected void lnkQuickSearch_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Friends Search", "Friends Search Results", false);
            if (CurrentUserSession != null)
            {
                var search1 = new BasicFriendsSearch
                {
                    Username = CurrentUserSession.Username,
                    Active = true,
                    Deleted = false,
                    SortByColumn = OrderByColumnEnum.FacebookMutualFriends,
                    FriendUsername = String.Empty
                };
                if (CurrentUserSession != null) search1.ExcludeUsername.Add(CurrentUserSession.Username);
                if (MatchmakerHelper.IsMatchToFriendAlredySelected) search1.ExcludeUsername.Add(MatchmakerHelper.MatchToUsername);
                DateTime time1 = DateTime.Now;
                var res1 = search1.GetResults();
                DateTime time2 = DateTime.Now;
                TimeSpan delta = time2/* DateTime.Now*/ - time1;
                string[] usernames = (res1 != null) ? res1.Usernames : null;
                //var search2 = new FacebookFriendsSearch();
                //var res2 = search2.GetResults();
                //string[] fbFriendsIds = (res2 !=null) ? res2.Usernames : null;
                var results = new FacebookAndUsersMergeResults
                                {
                                    Usernames = usernames,
                                    FacebookIds = null//fbFriendsIds
                                };
                SearchResults.Results = results;
            }
        }

        protected void lnkShowOnlineFriends_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;

            if (CurrentUserSession == null) return;

            ProcessMenuButtonClick((LinkButton)sender, "Quick Friends Search", "Online Friends", true);
            var search = new OnlineFriendsSearch
                             {
                                 Username = CurrentUserSession.Username,
                                 Active = true,
                                 Deleted = false,
                                 SortByColumn = OrderByColumnEnum.LastOnline,
                                 FriendUsername = String.Empty
                             };
            search.ExcludeUsername.Add(CurrentUserSession.Username);
            var results = new FacebookAndUsersMergeResults
                            {
                                Usernames = search.GetResults().Usernames,
                                FacebookIds = null
                            };
            SearchResults.Results = results;
        }

        protected void lnkShowFavorites_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Friends Search", "Favorites Users", true);
            var results = new FacebookAndUsersMergeResults
                              {
                                  Usernames = Classes.User.FetchFavouriteUsers(CurrentUserSession.Username),
                                  FacebookIds = null
                              };
            SearchResults.Results = results;
        }

        protected void lnkShowBlockedUsers_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Friends Search", "Blocked Users", true);
            var results = new FacebookAndUsersMergeResults
            {
                Usernames = Classes.User.FetchBlockedUsers(CurrentUserSession.Username).Keys.ToArray(),
                FacebookIds = null
            };
            SearchResults.Results = results;
        }

        private void ProcessMenuButtonClick(LinkButton linkButton, string searchTitle, string resultTitle, bool needAuthorization)
        {
            RequiresAuthorization = needAuthorization;
            if (RequiresAuthorization) ProcessValidation();
            SetActiveLinkButton(linkButton);
            hfDisplayAdvancedSearch.Value = String.Empty;
            LargeBoxStart1.Title = resultTitle.Translate();
            SmallBoxStart1.Title = searchTitle.Translate();
            SetSearchresultViewableProperties();
        }

        private void SetActiveLinkButton(LinkButton lbActive)
        {
            var sideMenuLinks = ControlsLogicalGroupsDic["SideMenuLinks"];
            if (sideMenuLinks != null)
                foreach (LinkButton linkButton in sideMenuLinks)
                {
                    linkButton.CssClass = "SideMenuLink";
                }
            //var hideBlockedUsersLinkList = ControlsLogicalGroupsDic["HideBlockedUsersLink"];
            //if (hideBlockedUsersLinkList != null)
            //    panelShowBlockedUsers.Visible = hideBlockedUsersLinkList.IndexOf(lbActive) < 0;
            //panelShowBlockedUsers.Visible = true;

            lbActive.CssClass = "SideMenuLink active";
        }
        private void SetSearchresultViewableProperties()
        {
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;
        }

        protected void lnkFriendsRequests_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Friends Search", "Friends Requests", true);
            var results = new FacebookAndUsersMergeResults
            {
                Usernames = Classes.User.FetchOpenFriendshipRequestsToUsernme(CurrentUserSession.Username).ToArray(),
                FacebookIds = null
            };
            SearchResults.Results = results;
        }






        private bool _ageFromIsSet;
        public int AgeFrom
        {
            get { return Convert.ToInt32(this.comboAgeFrom.SelectedValue); }
            set
            {
                this.comboAgeFrom.SelectedValue = value.ToString();
                _ageFromIsSet = true;
            }
        }

        private bool _ageToIsSet;
        public int AgeTo
        {
            get { return Convert.ToInt32(this.comboAgeTo.SelectedValue); }
            set
            {
                this.comboAgeTo.SelectedValue = value.ToString();
                _ageToIsSet = true;
            }
        }

        private int _minAge;
        private int _maxAge;
        public int MinAge
        {
            get { return _minAge = (_minAge == 0 || _minAge < Config.Users.MinAge) ? Config.Users.MinAge : _minAge; }
            set { _minAge = (value == 0 || value < Config.Users.MinAge) ? Config.Users.MinAge : value; }
        }
        public int MaxAge
        {
            get
            {
                _maxAge = (_maxAge == 0 || _maxAge > Config.Users.MaxAge) ? Config.Users.MaxAge : _maxAge;
                return (_maxAge < MinAge) ? MinAge : _maxAge;
            }
            set { _maxAge = (value == 0 || value > Config.Users.MaxAge) ? Config.Users.MaxAge : value; }
        }

        public void PopulateAgeCombos(User user)
        {
            comboAgeFrom.Items.Clear();
            comboAgeTo.Items.Clear();
            for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; i++)
            {
                comboAgeTo.Items.Add(i.ToString());
                comboAgeFrom.Items.Add(i.ToString());
            }
            comboAgeFrom.SelectedValue = user != null && user.Age - 10 > MinAge ? (user.Age - 10).ToString() : MinAge.ToString();
            comboAgeTo.SelectedValue = user != null && user.Age + 10 < MaxAge ? (user.Age + 10).ToString() : MaxAge.ToString();
        }

        protected void comboAgeToSelectIndexChange(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }
        protected void comboAgeFromSelectIndexChange(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }



        private bool _lookingForGenderIsSet
        {
            get
            {
                return ViewState["lookingForGenderIsSet"] != null &&
                                      (bool)ViewState["lookingForGenderIsSet"];
            }
            set { ViewState["lookingForGenderIsSet"] = value; }
        }
        public int LookingForGender
        {
            get
            {
                return _lookingForGenderIsSet ? int.Parse(this.dropGender.SelectedValue) : 0;
            }
            set
            {
                this.dropGender.SelectedValue = value.ToString();
                _lookingForGenderIsSet = true;
            }
        }

        private bool _interestedInGenderIsSet
        {
            get
            {
                return ViewState["interestedInGenderIsSet"] != null &&
                       (bool)ViewState["interestedInGenderIsSet"];
            }
            set { ViewState["interestedInGenderIsSet"] = value; }
        }
        public int InterestedInGender
        {
            get
            {
                return _interestedInGenderIsSet ? int.Parse(this.dropInterestedIn.SelectedValue) : 0;
            }
            set
            {
                this.dropInterestedIn.SelectedValue = value.ToString();
                _interestedInGenderIsSet = true;
            }
        }

        public void PopulateGenderCombos(User user)
        {
            dropGender.Items.Clear();
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Men"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Women"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couples"), ((int)Classes.User.eGender.Couple).ToString()));
            }
            dropInterestedIn.Items.Clear();
            dropInterestedIn.Items.Add(
                new ListItem(Lang.Trans("Men"), ((int)Classes.User.eGender.Male).ToString()));
            dropInterestedIn.Items.Add(
                new ListItem(Lang.Trans("Women"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Couples"), ((int)Classes.User.eGender.Couple).ToString()));
            }
            if (user != null)
            {
                InterestedInGender = ((int)user.Gender);
                LookingForGender = ((int)user.InterestedIn);
                bool toEnable = !(Page is Search3) || MatchmakerHelper.IsMatchmakerState ||
                    Convert.ToBoolean(ConfigurationManager.AppSettings["ENABLE_CHANGE_MY_GENDER_IN_SEARCH"]);
                dropGender.Enabled = toEnable;
                dropInterestedIn.Enabled = toEnable;
            }
            else
            {
                if (!_interestedInGenderIsSet)
                    InterestedInGender = ((int)Classes.User.eGender.Female);
                if (!_lookingForGenderIsSet)
                    LookingForGender = ((int)Classes.User.eGender.Male);
            }
        }



        public string Country
        {
            get
            {
                string result = this.dropCountry.SelectedValue;
                if (!Page.IsPostBack && this.dropCountry.SelectedValue.IsNullOrEmpty() &&
                        CascadingDropDownCountry.SelectedValue != null &&
                        CascadingDropDownCountry.SelectedValue.Trim(':') != "")
                    result = CascadingDropDownCountry.SelectedValue;
                return result;
            }
            set { CascadingDropDownCountry.SelectedValue = value; }
        }
        public string City
        {
            get
            {
                string result = this.dropCity.SelectedValue;
                if (!Page.IsPostBack && string.IsNullOrEmpty(dropCity.SelectedValue) &&
                        CascadingDropDownCity.SelectedValue != null &&
                        CascadingDropDownCity.SelectedValue.Trim(':') != "")
                    result = CascadingDropDownCity.SelectedValue;
                return result;
            }
            set { CascadingDropDownCity.SelectedValue = value; }
        }
        public string Region
        {
            get
            {
                string result = this.dropRegion.SelectedValue;
                if (!Page.IsPostBack && dropRegion.SelectedValue.IsNullOrEmpty() &&
                        CascadingDropDownState.SelectedValue != null &&
                        CascadingDropDownState.SelectedValue.Trim(':') != "")
                    result = CascadingDropDownState.SelectedValue;
                return result;
            }
            set { CascadingDropDownState.SelectedValue = value; }
        }
        public string CountriesServiceMethod { set { this.CascadingDropDownCountry.ServiceMethod = value; } }




        public void PopulateLocationControls(User user)
        {
            if (!Config.Users.LocationPanelVisible) return;

            if (dropCountry != null && user.Country.IsNotNullOrEmpty())
            {
                CascadingDropDownCountry.SelectedValue = user.Country;
            }
            if (dropRegion != null && user.State.IsNotNullOrEmpty())
            {
                CascadingDropDownState.SelectedValue = user.State;
            }
            if (dropCity != null && user.City.IsNotNullOrEmpty())
            {
                CascadingDropDownCity.SelectedValue = user.City;
            }
        }
    }
}