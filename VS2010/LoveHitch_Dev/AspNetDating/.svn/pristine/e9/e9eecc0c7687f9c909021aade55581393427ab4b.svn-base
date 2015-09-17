using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components.Search;
using System.Configuration;
using System.IO;

namespace AspNetDating
{
    public partial class Search3 : PageBase
    {
        private List<ProfileQuestion> profileQuestions;
        private Dictionary<int, object> dicQuestions;
        private FetchUsernamesResultsAsync asyncFetchUsernamesState;
        private HttpContext itsContext;

        public Search3()
        {
            RequiresAuthorization = false;
            asyncFetchUsernamesState = new FetchUsernamesResultsAsync { ItsSearchPageContext = this/*, Results = SearchResults*/};
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
                //StringWriter sw = userSession.SessionPagesDic["Search3.aspx"];
                //string outputStr = sw != null ? sw.ToString() : null;
                
                //if (false && sw != null && outputStr.IsNotNullOrEmpty())
                //{
                //    Response.Clear();
                //    Response.Write(outputStr);
                //    sw.Flush();
                //    sw.Close();
                //    userSession.SessionPagesDic["Search3.aspx"] = new StringWriter();
                //    Response.End();
                //}
                //else
                    PopulateGenderCombos(((PageBase)Page).CurrentUserSession);
            }


            MatchToFriend1.IsVisible = MatchmakerHelper.IsMatchmakerState;

            if (CurrentUserSession == null && Config.Users.RegistrationRequiredToSearch)
                Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

            if (Config.Users.CompletedProfileRequiredToBrowseSearch && CurrentUserSession != null &&
                    !CurrentUserSession.HasProfile && !CurrentUserSession.IsAdmin())
                Response.Redirect("ManageProfile.aspx?err=profnotcompl");

            ControlsLogicalGroupsDic =
                new Dictionary<string, List<Control>>
                    {
                        {"SideMenuLinks", (new[] {lnkQuickSearch, lnkAdvancedSearch, lnkShowOnlineUsers, lnkShowProfileViewers, lnkShowNewUsers}).ToList<Control>()}
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
            asyncState.ItsSearchPageContext.DataBind();
            //asyncState.ItsSearchPageContext.SearchResults.Results.Usernames = asyncState.Results;
        }

        public class FetchUsernamesResultsAsync
        {
            public Search3 ItsSearchPageContext;
            //public SearchResults1 Results;
            public AsyncTaskDelegate ItsDelegate;
            public delegate void AsyncTaskDelegate();

            public FetchUsernamesResultsAsync()
            {
                ItsDelegate = new AsyncTaskDelegate(ExecuteAsyncTask);
            }

            protected void ExecuteAsyncTask()
            {
                lock (this.ItsSearchPageContext.SearchResults)
                {
                    if (!((Site)ItsSearchPageContext.Page.Master).ScriptManager.IsInAsyncPostBack)
                    //if (ItsSearchPageContext.Page.IsPostBack)
                    {
                        ItsSearchPageContext.LoadStrings();
                        ItsSearchPageContext.Session["advancedSearch"] = false;

                        string startLocation = ItsSearchPageContext.Request.Params["loc"];

                        switch (startLocation)
                        {
                            case "online":
                                ItsSearchPageContext.lnkShowOnlineUsers_Click(ItsSearchPageContext.lnkShowOnlineUsers,
                                                                              null);
                                break;
                            case "viewers":
                                ItsSearchPageContext.lnkShowProfileViewers_Click(
                                    ItsSearchPageContext.lnkShowProfileViewers, null);
                                break;
                            case "new":
                                ItsSearchPageContext.lnkShowNewUsers_Click(ItsSearchPageContext.lnkShowNewUsers, null);
                                break;
                            case "detailed":
                            case "advance":
                                ItsSearchPageContext.lnkAdvancedSearch_Click(ItsSearchPageContext.lnkAdvancedSearch,
                                                                             null);
                                break;
                            case "quick":
                            case "session":
                            default:
                                ItsSearchPageContext.lnkQuickSearch_Click(ItsSearchPageContext.lnkQuickSearch, null);
                                break;
                        }
                        ItsSearchPageContext.timestamps.AddTimestamp("FetchUsernamesResultsAsync.ExecuteAsyncTask");
                    }
                    else
                    {
                        ItsSearchPageContext.PrepareCustomSearchFields(null, ItsSearchPageContext.RequiresAuthorization);
                        if (ItsSearchPageContext.hfDisplayAdvancedSearch.Value == "True")
                        {
                            var c = ItsSearchPageContext;
                            if (c.SearchResults == null || c.SearchResults.Results == null ||
                                c.SearchResults.Results.Usernames == null || c.SearchResults.Results.Usernames.Length == 0)
                                    ItsSearchPageContext.lnkAdvancedSearch_Click(ItsSearchPageContext.lnkAdvancedSearch, null);
                        }
                    }
                }
            }
        }

        private void PrepareDefaultFields()
        {
            //if (CurrentUserSession != null)
            {
                if (Request.Params["loc"] == "session")
                {
                    BasicSearch sessionSearch = (BasicSearch)Session["SessionSearchRequest"];

                    PopulateGenderCombos(null);
                    PopulateAgeCombos(null);

                    this.txtUsername.Text = sessionSearch.Username;
                    this.AgeFrom = sessionSearch.MinAge;
                    this.AgeTo = sessionSearch.MaxAge;
                    this.Country = sessionSearch.Country;
                    this.Region = sessionSearch.State;
                    this.City = sessionSearch.City;
                    //this.SearchGenderPicker1.LookingForGender = (int)sessionSearch.InterestedIn;
                    //this.SearchGenderPicker1.InterestedInGender = (int)sessionSearch.Gender;
                    this.InterestedInGender = (int)sessionSearch.InterestedIn;
                    this.LookingForGender = (int)sessionSearch.Gender;

                    if (txtUsername.Text.IsNotNullOrEmpty())
                    {
                        var search = new UsernameSearch { Username = txtUsername.Text };
                        var res = search.GetResults();
                        SearchResults.Results = new FacebookAndUsersMergeResults { Usernames = res != null ? res.Usernames : null };
                    }
                    else
                    {
                        if (CurrentUserSession != null) sessionSearch.ExcludeUsername.Add(CurrentUserSession.Username);
                        if (MatchmakerHelper.IsMatchToFriendAlredySelected) sessionSearch.ExcludeUsername.Add(MatchmakerHelper.MatchToUsername);
                        var res = sessionSearch.GetResults();
                        SearchResults.Results = new FacebookAndUsersMergeResults { Usernames = res != null ? res.Usernames : null };
                    }
                    PrepareCustomSearchFields();
                }
                else
                {
                    LoadLastSearch();
                }
            }
            //else
            //{
            //    SearchGenderPicker1.PopulateCombos(null);
            //    AgeRangePicker1.PopulateCombos(null);
            //    PrepareCustomSearchFields();
            //}
        }

        private void PrepareCustomSearchFields()
        {
            PrepareCustomSearchFields(null, false);
        }

        private void LoadLastSearch()
        {
            PrepareCustomSearchFields(null, !Page.IsPostBack);
        }

        private void PrepareCustomSearchFields(SavedSearch savedSearch)
        {
            PrepareCustomSearchFields(savedSearch, false);
        }

        /// <summary>
        /// Prepares the custom search fields.
        /// </summary>
        /// <param name="savedSearch">The saved search.</param>
        /// <param name="loadLastSearch">if set to <c>true</c> [load last search].</param>
        private void PrepareCustomSearchFields(SavedSearch savedSearch, bool loadLastSearch)
        {
            if (HttpContext.Current == null) HttpContext.Current = itsContext;

            profileQuestions = new List<ProfileQuestion>();
            dicQuestions = new Dictionary<int, object>();
            plhCustomSearch.Controls.Clear();

            SavedSearch lastSearch = null;

            if (CurrentUserSession != null)
            {
                if (savedSearch != null)
                    lastSearch = savedSearch;
                else if (loadLastSearch)
                {
                    string usernameToLoad = (MatchmakerHelper.IsMatchmakerState && MatchmakerHelper.IsMatchToFriendAlredySelected)
                                    ? MatchmakerHelper.MatchToUsername
                                    : CurrentUserSession.Username;
                    lastSearch = SavedSearch.Load(usernameToLoad, "_lastsearch_");
                }

                if (lastSearch != null)
                {
                    User user = (MatchmakerHelper.IsMatchmakerState && MatchmakerHelper.IsMatchToFriendAlredySelected)
                                    ? Classes.User.Load(MatchmakerHelper.MatchToUsername)
                                    : CurrentUserSession;
                    PopulateGenderCombos(user);
                    PopulateAgeCombos(user);
                    if (Config.Users.LocationPanelVisible)
                    {
                        this.Country = lastSearch.Country;
                        this.Region = lastSearch.State;
                        this.City = lastSearch.City;
                    }
                    cbPhotoReq.Checked = lastSearch.PhotoRequired;
                }
                else
                {
                    if (!Page.IsPostBack)
                    {
                        PopulateGenderCombos(CurrentUserSession);
                        PopulateAgeCombos(CurrentUserSession);
                    }
                }
            }

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();

            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    #region Add boxes for topics

                    var divTopicContainer = new HtmlGenericControl("div");

                    plhCustomSearch.Controls.Add(divTopicContainer);
                    var ltrTopicHeading = new LiteralControl(
                        String.Format("<a href=\"#\" onclick=\"return false;\" class=\"topicexpander \" tooltip='{0}'>{1}</a>", //plus1
                                      Lang.Translate("click to expand options"),
                                      Config.Misc.EnableProfileDataTranslation ? Lang.Trans(topic.Name) : topic.Name));

                    divTopicContainer.Controls.Add(ltrTopicHeading);

                    var ltrTopicBody = new HtmlGenericControl("div");
                    ltrTopicBody.Attributes.Add("class", "expandee topic_" + topic.ID);
                    ltrTopicBody.Style.Add("display", "none");
                    divTopicContainer.Controls.Add(ltrTopicBody);
                    #endregion

                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions != null)
                    {
                        foreach (ProfileQuestion question in questions)
                        {
                            if (question.SearchStyle == ProfileQuestion.eSearchStyle.Hidden ||
                                !(question.VisibleForMale || question.VisibleForFemale || question.VisibleForCouple))
                                continue;

                            IProfileSearchComponent cProfile = null;

                            switch (question.SearchStyle)
                            {
                                case ProfileQuestion.eSearchStyle.MultiChoiceSelect:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchMultiChoiceSelect.ascx");
                                    break;
                                case ProfileQuestion.eSearchStyle.MultiChoiceCheck:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchMultiChoiceCheck2.ascx");
                                    break;

                                case ProfileQuestion.eSearchStyle.RangeChoiceSelect:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchRangeChoiceSelect2.ascx");
                                    break;

                                case ProfileQuestion.eSearchStyle.SingleChoice:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchSingleChoice2.ascx");
                                    break;

                                default:
                                    break;
                            }
                            if (cProfile == null) continue;
                            //cProfile.UserControlPanel.Style.Add("padding-left", "5px");

                            ((Control)cProfile).ID = question.Id.ToString();
                            cProfile.Question = question;

                            if (lastSearch != null)
                            {
                                cProfile.ChoiceIds = lastSearch.ChoiceIds;
                            }

                            if (!question.VisibleForPaidOnly ||
                                (CurrentUserSession != null && CurrentUserSession.Paid))
                            {
                                var divQuestionContainer = new HtmlGenericControl("div");
                                divQuestionContainer.Controls.Add((Control)cProfile);
                                ltrTopicBody.Controls.Add(divQuestionContainer);

                                dicQuestions.Add(question.Id, (Control)cProfile);

                                if (!Config.Users.DisableGenderInformation)
                                {
                                    string genderClasses = (cProfile.UserControlPanel.Attributes["class"] ?? "")
                                                           + " " +
                                                           (question.VisibleForMale
                                                                ? "visibleformale"
                                                                : "invisibleformale")
                                                           + " " +
                                                           (question.VisibleForFemale
                                                                ? "visibleforfemale"
                                                                : "invisibleforfemale")
                                                           + " " +
                                                           (question.VisibleForCouple
                                                                ? "visibleforcouple"
                                                                : "invisibleforcouple");


                                    divQuestionContainer.Attributes["class"] = genderClasses.Trim();
                                    //divQuestionContainer.Style.Add("padding-left", "10px");

                                    if ((this.LookingForGender == (int)Classes.User.eGender.Male
                                         && !question.VisibleForMale) ||
                                        (this.LookingForGender == (int)Classes.User.eGender.Female
                                         && !question.VisibleForFemale) ||
                                        (this.LookingForGender == (int)Classes.User.eGender.Couple
                                         && !question.VisibleForCouple))
                                    {
                                        divQuestionContainer.Style["display"] = "none";
                                    }
                                }
                            }
                            profileQuestions.Add(question);
                        }
                    }

                    #region Remove empty topics

                    if (ltrTopicBody.Controls.Count == 0)
                    {
                        plhCustomSearch.Controls.Remove(divTopicContainer);
                    }

                    if (questions != null)
                    {
                        string genderClasses =
                            (questions.Any(q => q.VisibleForMale) ? "visibleformale" : "invisibleformale") + " " +
                            (questions.Any(q => q.VisibleForFemale) ? "visibleforfemale" : "invisibleforfemale") + " " +
                            (questions.Any(q => q.VisibleForCouple) ? "visibleforcouple" : "invisibleforcouple");

                        divTopicContainer.Attributes["class"] = genderClasses;

                        if ((this.LookingForGender == (int)Classes.User.eGender.Male
                             && !questions.Any(q => q.VisibleForMale)) ||
                            (this.LookingForGender == (int)Classes.User.eGender.Female
                             && !questions.Any(q => q.VisibleForFemale)) ||
                            (this.LookingForGender == (int)Classes.User.eGender.Couple
                             && !questions.Any(q => q.VisibleForCouple)))
                        {
                            divTopicContainer.Style["display"] = "none";
                        }
                    }

                    #endregion
                }
            }
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            SearchResults.OnPagePreRenderComplete();
            
            if (!IsPostBack)
            {
                LoadSavedSearches();
                PrepareDefaultFields();
                //LoadLastSearch();
                //btnSearch_Click(null, null);
            }

            // Expand topics where values are already selected
            var controls = new List<IProfileSearchComponent>(
                Select<IProfileSearchComponent>(plhCustomSearch));

            // Find all selections and unhide the topics
            foreach (var control in controls.Where(c => c.Answers.Length > 0))
            {
                var topicId = control.Answers[0].Question.TopicID;
                var topic = Select<HtmlGenericControl>(
                    plhCustomSearch,
                    c => ((HtmlGenericControl)c).Attributes["class"] != null &&
                        ((HtmlGenericControl)c).Attributes["class"].Contains("topic_" + topicId));
                if (topic == null || topic.Count() == 0) continue;
                topic.First().Style["display"] = "";
            }

            // Run cascade controls show/hide logic
            SetCascadeQuestions(profileQuestions.ToArray(), dicQuestions);
            base.OnPreRenderComplete(e);
        }

        private void LoadStrings()
        {
            CascadingDropDownCity.LoadingText = "[Loading cities...]".Translate();
            int langId = (int)(this.itsContext.Session["LanguageId"] ?? Config.Misc.DefaultLanguageId);
            SmallBoxStart1.Title = Lang.Trans(langId, "Quick Search", false);
            LargeBoxStart1.Title = Lang.Trans(langId, "Results List", false);
            cbPhotoReq.Text = Lang.Trans(langId, "With Photos", false);
            btnSearch.Text = Lang.Trans(langId, "Search", false);
            cbOnlineOnly.Text = Lang.Trans(langId, "Online Users", false);
            lnkQuickSearch.Text = Lang.Trans(langId, "Quick Search", false);
            lnkAdvancedSearch.Text = Lang.Trans(langId, "Detailed Search", false);
            lnkShowOnlineUsers.Text = Lang.Trans(langId, "Online Users", false);
            lnkShowProfileViewers.Text = Lang.Trans(langId, "Who Viewed my Profile?", false);
            SmallBoxStart2.Title = Lang.Trans(langId, "Search Options", false);
            lnkShowNewUsers.Text = Lang.Trans(langId, "New Users", false);

            if (CurrentUserSession != null)
            {
                cbSaveSearch.Text = "Save this search".Translate();
                cbEmailSavedSearch.Text = "Use it to send me new matches".Translate();
                ddEmailFrequency.Items.Add(new ListItem(Lang.Trans(langId, "Weekly", false), "7"));
                ddEmailFrequency.Items.Add(new ListItem(Lang.Trans(langId, "Semimonthly", false), "14"));
                ddEmailFrequency.Items.Add(new ListItem(Lang.Trans(langId, "Monthly", false), "30"));
            }
            else
            {
                cbSaveSearch.Visible = false;
            }
        }

        private void LoadSavedSearches()
        {
            if (HttpContext.Current == null) HttpContext.Current = itsContext;

            if (CurrentUserSession != null)
            {
                SavedSearch[] savedSearches = SavedSearch.Load(CurrentUserSession.Username);

                var dtSavedSearches = new DataTable();
                dtSavedSearches.Columns.Add("ID", typeof(int));
                dtSavedSearches.Columns.Add("Name", typeof(string));

                foreach (SavedSearch savedSearch in savedSearches)
                {
                    if (savedSearch.Name == "_lastsearch_")
                    {
                        continue;
                    }
                    dtSavedSearches.Rows.Add(new object[] { savedSearch.Id, savedSearch.Name });
                }

                dlSavedSearches.DataSource = dtSavedSearches;
                dlSavedSearches.DataBind();

                lnkSavedSearches.Visible = savedSearches.Length > 1;
            }
        }

        private void SaveSavedSearch(string name, int[] lSearchIDs)
        {
            timestamps.AddTimestamp("Search3.SaveSavedSearch");
            var gender = (User.eGender)Convert.ToInt32(this.LookingForGender);

            string country = "";
            string state = "";
            string zip = "";
            string city = "";

            if (Config.Users.LocationPanelVisible)
            {
                country = this.Country;
                state = this.Region;
                city = this.City;
                //zip = txtZip.Text;
            }

            int ageFrom = Config.Users.MinAge;
            int ageTo = Config.Users.MaxAge;

            try
            {
                ageFrom = Convert.ToInt32(this.AgeFrom);
                ageTo = Convert.ToInt32(this.AgeTo);
            }
            catch (FormatException)
            {
            }

            bool photoRequired = cbPhotoReq.Checked;
            bool emailMatches = false;
            int emailFrequency = 7;
            DateTime? nextEmailDate = null;

            if (cbSaveSearch.Checked && name != "_lastsearch_")
            {
                emailMatches = cbEmailSavedSearch.Checked;
                emailFrequency = Convert.ToInt32(ddEmailFrequency.SelectedValue);
                nextEmailDate = findNextFriday(DateTime.Now);
            }

            SavedSearch savedSearch = SavedSearch.Load(CurrentUserSession.Username, name);

            if (savedSearch == null)
                savedSearch = SavedSearch.Create(CurrentUserSession.Username, name,
                                                 gender, country, state, zip, city, ageFrom,
                                                 ageTo, photoRequired, lSearchIDs,
                                                 emailMatches, emailFrequency, nextEmailDate);
            else
            {
                savedSearch.Gender = gender;
                savedSearch.Country = country;
                savedSearch.State = state;
                savedSearch.Zip = zip;
                savedSearch.City = city;
                savedSearch.AgeFrom = ageFrom;
                savedSearch.AgeTo = ageTo;
                savedSearch.PhotoRequired = photoRequired;
                savedSearch.ChoiceIds = lSearchIDs;
                savedSearch.EmailMatches = emailMatches;
                savedSearch.EmailFrequency = emailFrequency;
                savedSearch.NextEmailDate = nextEmailDate;
            }
            savedSearch.Save();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null) HttpContext.Current = itsContext;

            if (txtUsername != null && txtUsername.Text != String.Empty)
            {
                UsernameSearch search = new UsernameSearch { Username = txtUsername.Text };

                if (Config.Users.RequireProfileToShowInSearch)
                {
                    search.HasAnswer = true;
                }
                var res = search.GetResults();
                SearchResults.Results = new FacebookAndUsersMergeResults { Usernames = res != null ? res.Usernames : null };
            }
            else
            {
                SearchResults.CurrentPage = 1;
                BasicSearch search = ((bool)Session["advancedSearch"]) ? new CustomSearch() : new BasicSearch();

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
                    if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
                        search.InterestedIn = (User.eGender)this.InterestedInGender;
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
                        search.Country = this.Country;
                        search.State = this.Region;
                        search.City = this.City;
                }
                if (CurrentUserSession != null)
                {
                    search.ViewerUsername = CurrentUserSession.Username;
                    search.SortColumn = "MutualFriendsCount";
                    search.SortColumn = "LastOnline";
                }

                if (search is CustomSearch)
                {
                    var lSearchTerms = new List<ProfileAnswer[]>();
                    var lSearchIDs = new List<int>();

                    var controls = new List<IProfileSearchComponent>(
                        Select<IProfileSearchComponent>(plhCustomSearch));

                    foreach (IProfileSearchComponent searchTerm in controls)
                    {
                        if (searchTerm.Answers != null && searchTerm.Answers.Length > 0)
                        {
                            ProfileQuestion question = ProfileQuestion.Fetch(searchTerm.Answers[0].Question.Id);

                            if (!Config.Users.DisableGenderInformation &&
                                (!question.VisibleForMale && search.Gender == Classes.User.eGender.Male
                                 || !question.VisibleForFemale && search.Gender == Classes.User.eGender.Female
                                 || !question.VisibleForCouple && search.Gender == Classes.User.eGender.Couple))
                                continue;

                            if (question.ParentQuestionID.HasValue)
                            {
                                var parentControl = controls.FirstOrDefault(
                                    c =>
                                    c != null && c.Answers.Length > 0 &&
                                    c.Answers[0].Question.Id == question.ParentQuestionID);
                                if (parentControl != null)
                                {
                                    string[] parentAnswers = parentControl.Answers.Select(a => a.Value).ToArray();
                                    if (
                                        !question.ParentQuestionChoices.Split(':').Any(
                                            parentChoice => parentAnswers.Contains(parentChoice)))
                                    {
                                        continue;
                                    }
                                }
                            }
                            lSearchTerms.Add(searchTerm.Answers);
                            lSearchIDs.AddRange(searchTerm.ChoiceIds);
                        }
                    }

                    if (Page.IsPostBack && CurrentUserSession != null)
                    {
                        SaveSavedSearch("_lastsearch_", lSearchIDs.ToArray());

                        if (cbSaveSearch.Checked)
                        {
                            if (txtSavedSearchName.Text.IsNotNullOrEmpty())
                            {
                                SaveSavedSearch(txtSavedSearchName.Text, lSearchIDs.ToArray());
                                LoadSavedSearches();
                            }
                        }
                    }
                    ((CustomSearch)search).Answers = lSearchTerms.ToArray();
                }
                search.HasPhoto = cbPhotoReq.Checked;
                if (CurrentUserSession != null)
                    search.ExcludeUsername.Add(CurrentUserSession.Username);
                if (MatchmakerHelper.IsMatchToFriendAlredySelected)
                    search.ExcludeUsername.Add(MatchmakerHelper.MatchToUsername);
                timestamps.AddTimestamp("Search3.btnSearch_Click");
                var res = search.GetResults();
                timestamps.AddTimestamp("Search3.btnSearch_Click");
                SearchResults.Results = new FacebookAndUsersMergeResults { Usernames = res != null ? res.Usernames : null };
            }
            SetSearchresultViewableProperties();
        }

        protected void dlSavedSearches_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "SavedSearchView")
            {
                this.CountriesServiceMethod = "GetCountriesForSavedSearch";
                PrepareCustomSearchFields(SavedSearch.Load(Convert.ToInt32(e.CommandArgument)));
                txtSavedSearchName.Text = ((LinkButton)e.Item.FindControl("lnkSavedSearch")).Text;
                btnSearch_Click(null, null);
            }
            if (e.CommandName == "SavedSearchDelete")
            {
                txtSavedSearchName.Text = "";
                cbSaveSearch.Checked = false;
                int id = Convert.ToInt32(e.CommandArgument);
                SavedSearch.Delete(id);
                LoadSavedSearches();
            }
        }

        #region Cascade questions methods

        private void SetCascadeQuestions(ProfileQuestion[] questions, Dictionary<int, object> dicQuestions)
        {
            var lHiddenParentQuestions = new List<int>();

            foreach (ProfileQuestion question in questions)
            {
                ProfileQuestion[] childQuestions =
                    questions.Where(q => q.ParentQuestionID.HasValue
                                         && q.ParentQuestionID.Value == question.Id).ToArray();

                bool isParent = childQuestions.Length > 0;
                bool isChild = question.ParentQuestionID.HasValue;
                if (!dicQuestions.ContainsKey(question.Id)) // if current question is hidden
                    continue;
                var currentQuestionControl = (Control)dicQuestions[question.Id];

                if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                    ((ICascadeQuestionComponent)currentQuestionControl).GenerateResetValuesJS();

                if (isParent)
                {
                    var childClientIDsWithParentQuestionChoices = new Dictionary<string, object[]>();
                    GetChildrenClientIDs(question, questions, dicQuestions, childClientIDsWithParentQuestionChoices);

                    if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                        ((ICascadeQuestionComponent)currentQuestionControl).GenerateJSForChildVisibility(
                            childClientIDsWithParentQuestionChoices);
                    else
                        new Exception(String.Format("{0} control must implement ICascadeQuestionComponent",
                                                    currentQuestionControl.ID));
                }

                if (isChild)
                {
                    // if parent question is hidden hide the child
                    if (!dicQuestions.ContainsKey(question.ParentQuestionID.Value)
                        || lHiddenParentQuestions.Contains(question.ParentQuestionID.Value))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        ((IProfileSearchComponent)currentQuestionControl).UserControlPanel.Attributes.Add("style",
                                                                                                           "display:none");
                        ((IProfileSearchComponent)currentQuestionControl).ClearAnswers();
                        continue;
                    }
                    var currentQuestionParentControl = (Control)dicQuestions[question.ParentQuestionID.Value];
                    var parentAnswers = new List<string>();
                    foreach (ProfileAnswer answer in ((IProfileSearchComponent)currentQuestionParentControl).Answers)
                    {
                        parentAnswers.Add(answer.Value);
                    }
                    if (
                        !question.ParentQuestionChoices.Split(':').Any(
                             parentChoice => parentAnswers.Contains(parentChoice)))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        ((IProfileSearchComponent)currentQuestionControl).UserControlPanel.Attributes.Add("style",
                                                                                                           "display:none");
                        ((IProfileSearchComponent)currentQuestionControl).ClearAnswers();
                    }
                }
            }
        }

        private void GetChildrenClientIDs(ProfileQuestion question, ProfileQuestion[] questions,
                                          Dictionary<int, object> dicQuestions,
                                          Dictionary<string, object[]> childClientIDsWithParentQuestionChoices)
        {
            ProfileQuestion[] childQuestions =
                questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();
            if (childQuestions.Length > 0)
            {
                foreach (ProfileQuestion childQuestion in childQuestions)
                {
                    var childClientIDs = new List<string>();

                    // child question is not visible so skip it
                    if (dicQuestions.ContainsKey(childQuestion.Id))
                    {
                        string childClientID =
                            ((IProfileSearchComponent)dicQuestions[childQuestion.Id]).UserControlPanel.ClientID;
                        string[] parentQuestionChoices =
                            childQuestion.ParentQuestionChoices.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                        childClientIDsWithParentQuestionChoices.Add(childClientID,
                                                                    new object[] { parentQuestionChoices, childClientIDs });
                    }

                    PopulateChildrenIDs(childQuestion, questions, dicQuestions, childClientIDs);
                }
            }
        }

        private void PopulateChildrenIDs(ProfileQuestion question, ProfileQuestion[] questions,
                                         Dictionary<int, object> dicQuestions, List<string> childClientIDs)
        {
            ProfileQuestion[] childQuestions =
                questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();
            if (childQuestions.Length > 0)
            {
                foreach (ProfileQuestion childQuestion in childQuestions)
                {
                    // child question is not visible so skip it
                    if (dicQuestions.ContainsKey(childQuestion.Id))
                    {
                        string childClientID =
                            ((IProfileSearchComponent)dicQuestions[childQuestion.Id]).UserControlPanel.ClientID;

                        childClientIDs.Add(childClientID);
                    }

                    PopulateChildrenIDs(childQuestion, questions, dicQuestions, childClientIDs);
                }
            }
        }

        #endregion

        #region Helper methods

        public static IEnumerable<T> Select<T>(Control from, Predicate<Control> predicate) where T : class
        {
            if (from is T)
            {
                if (predicate(from))
                    yield return from as T;
            }
            foreach (Control idx in from.Controls)
            {
                foreach (var idxInner in Select<T>(idx, predicate))
                {
                    yield return idxInner;
                }
            }
        }

        public static IEnumerable<T> Select<T>(Control from) where T : class
        {
            return Select<T>(from, idx => idx is T);
        }

        private DateTime findNextFriday(DateTime date)
        {
            do date = date.AddDays(1);
            while (date.DayOfWeek != DayOfWeek.Friday);

            return date;
        }

        #endregion

        protected void lnkQuickSearch_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            Session["advancedSearch"] = false;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Search", "Search Results", false);
            if (Request.Params["loc"] == "session")
            {
                PrepareDefaultFields();
            }
            //if (CurrentUserSession != null)
            //{
            //    if (Request.Params["loc"] == "session")
            //    {
            //        BasicSearch sessionSearch = (BasicSearch)Session["SessionSearchRequest"];

            //        this.txtUsername.Text = sessionSearch.Username;
            //        this.AgeRangePicker1.AgeFrom = sessionSearch.MinAge;
            //        this.AgeRangePicker1.AgeTo = sessionSearch.MaxAge;
            //        this.SearchLocationPicker1.Country = sessionSearch.Country;
            //        this.SearchLocationPicker1.Region = sessionSearch.State;
            //        this.SearchLocationPicker1.City = sessionSearch.City;
            //        this.SearchGenderPicker1.LookingForGender = (int)sessionSearch.InterestedIn;
            //        this.SearchGenderPicker1.InterestedInGender = (int)sessionSearch.Gender;

            //        if (txtUsername.Text.IsNotNullOrEmpty())
            //        {
            //            var search = new UsernameSearch {Username = txtUsername.Text};
            //            SearchResults.Results = search.GetResults();
            //        }
            //        else
            //        {
            //            SearchResults.Results = sessionSearch.GetResults();
            //        }
            //    }
            //}
            //if (SearchResults.Results == null || SearchResults.Results.Usernames.Length == 0)
            //{
            //    var searchNew = new NewUsersSearch
            //                        {
            //                            UsersSince = new DateTime(2010, 1, 1),
            //                            MinAge = 20,
            //                            MaxAge = 40,
            //                            PhotoReq = true,
            //                            PageNumber = 0,
            //                            UsersCount = 6
            //                        };

            //    SearchResults.Results = searchNew.GetResults();
            //}
            else
            {
                string userToExclude = CurrentUserSession != null ? CurrentUserSession.Username : "";
                var search = new BasicSearch();
                if (MatchmakerHelper.IsMatchToFriendAlredySelected)
                    search.ExcludeUsername.Add(MatchmakerHelper.MatchToUsername);
                if (CurrentUserSession != null)
                {
                    search.ViewerUsername = CurrentUserSession.Username;
                    search.ExcludeUsername.Add(CurrentUserSession.Username);
                    search.Gender = CurrentUserSession.InterestedIn;
                    search.InterestedIn = CurrentUserSession.Gender;
                    search.MaxAge = Math.Min(
                        CurrentUserSession.IncomingMessagesRestrictions.AgeTo,
                        Config.Users.MaxAge);
                    search.MinAge = Math.Max(
                        CurrentUserSession.IncomingMessagesRestrictions.AgeFrom,
                        Config.Users.MinAge);
                    search.SortColumn = "MutualFriendsCount";
                    search.SortColumn = "LastOnline";
                }
                else
                {
                    search.MaxAge = Config.Users.MaxAge;
                    search.MinAge = Config.Users.MinAge;
                }
                SearchResults.Results = new FacebookAndUsersMergeResults
                                            {
                                                Usernames = search.GetResults().Usernames,
                                                FacebookIds = null
                                            };
            }
        }

        protected void lnkAdvancedSearch_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            Session["advancedSearch"] = true;
            ProcessMenuButtonClick((LinkButton)sender, "Advanced Search", "Search Results", true);

            var search = new BasicSearch();
            if (CurrentUserSession != null)
            {
                search.ExcludeUsername.Add(CurrentUserSession.Username);
                search.ViewerUsername = CurrentUserSession.Username;
            }
            if (MatchmakerHelper.IsMatchToFriendAlredySelected)
                search.ExcludeUsername.Add(MatchmakerHelper.MatchToUsername);

            SearchResults.Results = new FacebookAndUsersMergeResults
            {
                Usernames = search.GetResults().Usernames,
                FacebookIds = null
            };
        }

        protected void lnkShowProfileViewers_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Search", "Who Viewed Me", true);
            var search = new ProfileViewedSearch
                             {
                                 ExcludeUsername = CurrentUserSession.Username,
                                 Username = CurrentUserSession.Username
                             };
            SearchResults.Results = new FacebookAndUsersMergeResults
            {
                Usernames = search.GetResults().Usernames,
                FacebookIds = null
            };
        }

        protected void lnkShowNewUsers_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Search", "New Users", true);
            //var search = new BasicSearch { Username = CurrentUserSession.Username, SortColumn = "SignupDate", SortAsc = false };
            NewUsersSearch search;
            if (CurrentUserSession != null)
            {
                search = new NewUsersSearch
                {
                    Gender = CurrentUserSession.InterestedIn,
                    MaxAge = Math.Min(
                       CurrentUserSession.IncomingMessagesRestrictions.AgeTo,
                       Config.Users.MaxAge),
                    MinAge = Math.Max(
                       CurrentUserSession.IncomingMessagesRestrictions.AgeFrom,
                       Config.Users.MinAge)
                };
            }
            else
            {
                search = new NewUsersSearch
                {
                    MaxAge = Config.Users.MaxAge,
                    MinAge = Config.Users.MinAge
                };
            }
            search.LoginCountThreshold = 1;
            //Results = search.GetResults();
            SearchResults.Results = new FacebookAndUsersMergeResults
            {
                Usernames = search.GetResults().Usernames,
                FacebookIds = null
            };
        }

        protected void lnkShowOnlineUsers_Click(object sender, EventArgs e)
        {
            if (HttpContext.Current == null)
                HttpContext.Current = itsContext;
            ProcessMenuButtonClick((LinkButton)sender, "Quick Search", "Online Users", true);
            var search = new BasicSearch { Username = CurrentUserSession.Username, SortColumn = "LastOnline", SortAsc = false };
            SearchResults.Results = new FacebookAndUsersMergeResults
                                        {
                                            Usernames = search.GetResults().Usernames,
                                            FacebookIds = null
                                        };
        }

        private void ProcessMenuButtonClick(LinkButton linkButton, string searchTitle, string resultTitle, bool needAuthorization)
        {
            RequiresAuthorization = needAuthorization;
            if (RequiresAuthorization) ProcessValidation();
            SetActiveLinkButton(linkButton);
            hfDisplayAdvancedSearch.Value = Session["advancedSearch"].ToString();
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
            lbActive.CssClass = "SideMenuLink active";
        }
        private void SetSearchresultViewableProperties()
        {
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;
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