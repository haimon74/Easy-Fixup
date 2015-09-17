using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using AspNetDating.Classes;
using AspNetDating.Services;

namespace AspNetDating.Mobile
{
    public partial class Search : PageBase
    {
        private bool paginatorVisible = true;
        protected bool showCity = Config.Users.LocationPanelVisible;
        protected bool showGender = !Config.Users.DisableGenderInformation;
        protected bool showAge = !Config.Users.DisableAgeInformation;
        private bool updateHistory = true;

        public Search()
        {
            RequiresAuthorization = false;
        }

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        public UserSearchResults Results
        {
            set
            {
                if (ViewState["SearchResults_guid"] == null)
                {
                    ViewState["SearchResults_guid"] = Guid.NewGuid().ToString();
                }

                if (value != null && value.Usernames.Length == 0)
                    value = null;

                Session["SearchResults" + ViewState["SearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["SearchResults_guid"] != null)
                {
                    return (UserSearchResults)
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (CurrentUserSession == null && Config.Users.RegistrationRequiredToSearch)
                    Response.Redirect("Default.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

                if (Config.Users.CompletedProfileRequiredToBrowseSearch &&
                    CurrentUserSession != null && !CurrentUserSession.HasProfile && !CurrentUserSession.IsAdmin())
                    Response.Redirect("ManageProfile.aspx?err=profnotcompl");

                mvSearch.SetActiveView(viewBasicSearch);

                LoadStrings();
                CheckForRequests();
            }

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (mvSearch.GetActiveView() == viewSearchResults)
            {
                lblTitle.InnerText = "Search Results".Translate();
                LoadResultsPage();

                if (PaginatorEnabled)
                {
                    PreparePaginator();
                }
            }
            else
            {
                lblTitle.InnerText = "Search Terms".Translate();
                Results = null;
            }

            base.OnPreRender(e);
        }

        private void LoadStrings()
        {
            if (Config.Users.LocationPanelVisible)
            {
                LoadCountries();
                ShowLocation();
            }
            else
                HideLocation();

            if (!Config.Users.InterestedInFieldEnabled || Config.Users.DisableGenderInformation)
                trInterestedIn.Visible = false;
            else
            {
                dropInterestedIn.Items.Clear();
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
                dropInterestedIn.Items.Add(
                    new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    dropInterestedIn.Items.Add(
                        new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }

                if (CurrentUserSession != null)
                {
                    dropInterestedIn.SelectedValue = ((int)CurrentUserSession.Gender).ToString();
                }
            }
            dropGender.Items.Clear();
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            if (CurrentUserSession != null)
            {
                if (Config.Users.InterestedInFieldEnabled)
                    dropGender.SelectedValue = ((int)CurrentUserSession.InterestedIn).ToString();
                else
                {
                    switch (CurrentUserSession.Gender)
                    {
                        case Classes.User.eGender.Male:
                            dropGender.SelectedValue = ((int)Classes.User.eGender.Female).ToString();
                            break;
                        case Classes.User.eGender.Female:
                            dropGender.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
                            break;
                        case Classes.User.eGender.Couple:
                            break;
                        default:
                            break;
                    }
                }
            }

            txtAgeFrom.Text = Config.Users.MinAge.ToString();
            txtAgeTo.Text = Config.Users.MaxAge.ToString();

            btnBasicSearchGo.Text = Lang.Trans("Search");

            lnkFirst.AlternateText = Lang.Trans("[ First ]");
            lnkPrev.AlternateText = Lang.Trans("[ Prev ]");
            lnkNext.AlternateText = Lang.Trans("[ Next ]");
            lnkLast.AlternateText = Lang.Trans("[ Last ]");
        }

        private void LoadCountries()
        {
            if (dropCountry.Items.Count > 0) return;

            dropCountry.Items.Add("");
            foreach (CascadingDropDownNameValue value in Service.GetCountries(true))
            {
                ListItem item = new ListItem(value.name, value.value);
                item.Selected = value.isDefaultValue;
                dropCountry.Items.Add(item);
            }

            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.Country.Length > 0)
                    dropCountry.SelectedValue = CurrentUserSession.Country;
            }

            if (dropCountry.SelectedIndex > 0)
                dropCountry_SelectedIndexChanged(null, null);

            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.State.Length > 0)
                {
                    dropRegion.SelectedValue = CurrentUserSession.State;
                    dropRegion_SelectedIndexChanged(null, null);
                }

                if (CurrentUserSession.City.Length > 0)
                    dropCity.SelectedValue = CurrentUserSession.City;
            }
        }

        private void HideLocation()
        {
            pnlCountry.Visible = false;
            pnlState.Visible = false;
            pnlZip.Visible = false;
            pnlCity.Visible = false;
        }

        private void ShowLocation()
        {
            pnlCountry.Visible = true;
            pnlState.Visible = true;
            pnlZip.Visible = true;
            pnlCity.Visible = true;
        }

        private void CheckForRequests()
        {
            if (Session["BasicSearchRequest"] != null)
            {
                btnBasicSearchGo_Click(null, null);
                return;
            }

//            if (Session["UsernameSearchRequest"] != null)
//            {
//                btnUsernameSearchGo_Click(null, null);
//                return;
//            }

            if (Session["NewUsersSearch"] != null)
            {
                Session.Remove("NewUsersSearch");
                NewUsersSearch();
                return;
            }

            if (Session["OnlineUsersSearch"] != null)
            {
                Session.Remove("OnlineUsersSearch");
                OnlineUsersSearch();
                return;
            }

            if (Session["ShowProfileViewers"] != null)
            {
                Session.Remove("ShowProfileViewers");
                ShowProfileViewers();
                return;
            }

            if (Session["MutualVoteSearch"] != null)
            {
                Session.Remove("MutualVoteSearch");
                ShowMutualVotes();
                return;
            }
        }

        private void NewUsersSearch()
        {
            mvSearch.SetActiveView(viewSearchResults);

            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.PhotoReq = Config.Users.RequirePhotoToShowInNewUsers;
            nuSearch.ProfileReq = Config.Users.RequireProfileToShowInNewUsers;
            nuSearch.UsersSince = CurrentUserSession.PrevLogin;

            UserSearchResults nuResults = nuSearch.GetResults();

            Results = nuResults;
        }

        private void OnlineUsersSearch()
        {
            mvSearch.SetActiveView(viewSearchResults);

            OnlineSearch search = new OnlineSearch();

            UserSearchResults results = search.GetResults();

            Results = results;
        }

        private void ShowProfileViewers()
        {
            if (CurrentUserSession == null) return;

            mvSearch.SetActiveView(viewSearchResults);

            string[] profileViewersUsernames = Classes.User.FetchProfileViews(CurrentUserSession.Username,
                                                                              DateTime.MinValue);
            UserSearchResults pvResults = null;

            if (profileViewersUsernames != null)
            {
                pvResults = new UserSearchResults();
                pvResults.Usernames = profileViewersUsernames;
            }

            Results = pvResults;
        }

        private void ShowMutualVotes()
        {
            mvSearch.SetActiveView(viewSearchResults);

            MutualVoteSearch mutualVoteSearch = new MutualVoteSearch();
            mutualVoteSearch.Username = CurrentUserSession.Username;

            UserSearchResults results = mutualVoteSearch.GetResults();

            Results = results;
        }

        /// <summary>
        /// Prepares the paginator.
        /// </summary>
        private void PreparePaginator()
        {
            int usersPerPage = rptUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
            if (Results == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (Results == null || CurrentPage >= Results.GetTotalPages(usersPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Usernames.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * usersPerPage + 1;
                int toUser = CurrentPage * usersPerPage;
                if (Results.Usernames.Length < toUser)
                {
                    toUser = Results.Usernames.Length;
                }

                lblPager.Text = String.Format(
                    Lang.Trans("{0}-{1} from {2}"),
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

            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("PhotoId", typeof(int));
            dtResults.Columns.Add("Slogan");
            dtResults.Columns.Add("Age");
            dtResults.Columns.Add("Gender");
            dtResults.Columns.Add("LastOnlineString");
            dtResults.Columns.Add("Location");

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);

                User[] users;
                if (PaginatorEnabled)
                {
                    int usersPerPage = rptUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
                    users = Results.GetPage(CurrentPage, usersPerPage);
                }
                else
                {
                    users = Results.Get();
                }

                if (users != null && users.Length > 0)
                {
                    foreach (User user in users)
                    {
                        #region Gets User Photo

                        Photo photo = null;
                        try
                        {
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

                        #region User's Age

                        string age = null;

                        if (!Config.Users.DisableAgeInformation)
                        {
                            if (Config.Users.CouplesSupport && user.Gender == Classes.User.eGender.Couple)
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

                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;

                        #endregion

                        int photoId;

                        if (photo == null || !photo.Approved)
                        {
                            photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                        }
                        else
                        {
                            photoId = photo.Id;
                        }

                        dtResults.Rows.Add(new object[]
                                               {
                                                   user.Username, photoId, slogan, age, gender, user.LastOnlineString,
                                                   user.LocationString
                                               });
                    }
                }
            }

            if (rptUsers.Visible)
            {
                rptUsers.DataSource = dtResults;
                rptUsers.DataBind();
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkFirst_Click(object sender, EventArgs e)
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
        protected void lnkPrev_Click(object sender, EventArgs e)
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
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect("Home.aspx");

            int usersPerPage = rptUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
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
        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect("Home.aspx");

            int usersPerPage = rptUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
            if (CurrentPage < Results.GetTotalPages(usersPerPage))
            {
                CurrentPage = Results.GetTotalPages(usersPerPage);
            }
        }

        protected void dropCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropRegion.Items.Clear();
            dropRegion.Items.Add("");

            foreach (string region in Config.Users.GetRegions(dropCountry.SelectedValue))
            {
                if (Config.Users.ForceRegion.Trim().Length > 0)
                {
                    if (region.ToLower() != Config.Users.ForceRegion.Trim().ToLower()) continue;
                    ListItem item = new ListItem(region, region);
                    item.Selected = true;
                    dropRegion.Items.Add(item);
                }
                else if (region.Length == 0)
                {
                    ListItem item = new ListItem(Lang.Trans("All"), " ");
                    item.Selected = true;
                    dropRegion.Items.Add(item);
                }
                else
                    dropRegion.Items.Add(new ListItem(region, region));
            }

            if (dropRegion.SelectedIndex > 0)
                dropRegion_SelectedIndexChanged(null, null);
            else
            {
                dropCity.Items.Clear();
                dropCity.Items.Add("");
            }
        }

        protected void dropRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            dropCity.Items.Clear();
            dropCity.Items.Add("");

            string[] cities = Config.Users.GetCities(dropCountry.SelectedValue,
                                                     dropRegion.SelectedValue.Trim());
            if (cities != null && cities.Length > 0)
            {
                foreach (string city in cities)
                {
                    bool isDefault = false;
                    if (Config.Users.ForceCity.Trim().Length > 0)
                    {
                        if (city.ToLower() != Config.Users.ForceCity.Trim().ToLower()) continue;
                        isDefault = true;
                    }
                    ListItem item = new ListItem(city, city);
                    item.Selected = isDefault;
                    dropCity.Items.Add(item);
                }
            }
            else
            {
                ListItem item = new ListItem("-", "-");
                item.Selected = true;
                dropCity.Items.Add(item);
            }
        }

        protected void rptUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }

        protected void rptUsers_ItemCreated(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void rptUsers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;
        }

        protected void btnBasicSearchGo_Click(object sender, EventArgs e)
        {
            BasicSearch search;

            mvSearch.SetActiveView(viewSearchResults);

            if (Session["BasicSearchRequest"] == null)
            {
                search = new BasicSearch();

                if (Config.Users.RequireProfileToShowInSearch)
                {
                    search.HasAnswer = true;
                }
                else
                {
                    search.hasAnswer_isSet = false;
                }

                search.Gender = (User.eGender)
                                Convert.ToInt32(dropGender.SelectedValue);
                if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
                    search.InterestedIn = (User.eGender)
                                          Convert.ToInt32(dropInterestedIn.SelectedValue);
                try
                {
                    search.MinAge = Convert.ToInt32(txtAgeFrom.Text);
                    search.MaxAge = Convert.ToInt32(txtAgeTo.Text);
                }
                catch
                {
                }

                if (Config.Users.LocationPanelVisible)
                {
                    if (dropCountry != null)
                    {
                        search.Country = dropCountry.SelectedValue.Trim();
                    }
                    if (dropRegion != null)
                    {
                        search.State = dropRegion.SelectedValue.Trim();
                    }
                    if (dropCity != null)
                    {
                        search.City = dropCity.SelectedValue.Trim();
                    }
                    if (txtZip != null)
                    {
                        search.Zip = txtZip.Text;
                    }
                }

                if (cbPhotoReq.Checked)
                    search.HasPhoto = true;
            }
            else
            {
                search = (BasicSearch)Session["BasicSearchRequest"];
                Session.Remove("BasicSearchRequest");
            }

            UserSearchResults results = search.GetResults();

            Results = results;
        }

        void scriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            if (Results == null)
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
            int usersPerPage = rptUsers.Visible ? Config.Search.UsersPerPage : Config.Search.UsersPerPageGrid;
            if (navigatePage <= Results.GetTotalPages(usersPerPage)
                && navigatePage > 0)
            {
                updateHistory = false;
                CurrentPage = navigatePage;
            }
        }
    }
}
