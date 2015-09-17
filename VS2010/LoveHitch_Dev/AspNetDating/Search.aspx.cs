/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;
using AspNetDating.Components.Search;
using SearchResults = AspNetDating.Components.Search.SearchResults;

namespace AspNetDating
{
    public partial class Search : PageBase
    {
        #region Server Controls


        protected SmallBoxStart SmallBoxStart1;
        protected DropDownList dropCity;
        protected DropDownList dropCity2;
        protected LargeBoxStart LargeBoxStart1, LargeBoxStart2, LargeBoxStart3, LargeBoxStart4;
        protected SearchResults1 SearchResults;
        protected HeaderLine CustomSearchHeaderLine;
        protected HeaderLine BasicSearchHeaderLine;
        protected HeaderLine UsernameSearchHeaderLine;
        protected HeaderLine KeywordSearchHeaderLine;
        protected HeaderLine DistanceSearchHeaderLine;

        #endregion

        public Search()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError2.Text = "";
            if (!Page.IsPostBack)
            {
                if (CurrentUserSession == null && Config.Users.RegistrationRequiredToSearch)
                    Response.Redirect("Login.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

                if (Config.Users.CompletedProfileRequiredToBrowseSearch &&
                    CurrentUserSession != null && !CurrentUserSession.HasProfile && !CurrentUserSession.IsAdmin())
                    Response.Redirect("ManageProfile.aspx?err=profnotcompl");

                SetDefaults();
                LoadStrings();
                LoadSavedSearches();
                CheckForRequests();
            }

            Page.RegisterJQuery();

            if (!Config.Search.DefaultToCustomSearch || IsPostBack)
                PrepareCustomSearchFields();
        }

        private void SetDefaults()
        {
            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.Country.Length > 0)
                    CascadingDropDownCountry.SelectedValue = CurrentUserSession.Country;
                if (CurrentUserSession.State.Length > 0)
                    CascadingDropDownState.SelectedValue = CurrentUserSession.State;
                if (CurrentUserSession.City.Length > 0)
                    CascadingDropDownCity.SelectedValue = CurrentUserSession.City;
                if (CurrentUserSession.Country.Length > 0)
                    CascadingDropDownCountry2.SelectedValue = CurrentUserSession.Country;
                if (CurrentUserSession.State.Length > 0)
                    CascadingDropDownState2.SelectedValue = CurrentUserSession.State;
                if (CurrentUserSession.City.Length > 0)
                    CascadingDropDownCity2.SelectedValue = CurrentUserSession.City;
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlSavedSearches.ItemCommand +=
                new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlSavedSearches_ItemCommand);
            this.dropGender2.SelectedIndexChanged += new EventHandler(dropGender2_SelectedIndexChanged);
        }

        #endregion

        private void LoadStrings()
        {
            #region Prepare Saved Searches Separator

            if (CurrentUserSession != null)
            {
                SmallBoxStart2.Visible = true;
                SmallBoxStart2.Title = Lang.Trans("Saved Searches");
                SmallBoxEnd2.Visible = true;
            }
            else
            {
                SmallBoxStart2.Visible = false;
                SmallBoxEnd2.Visible = false;
                divSavedSearches.Visible = false;
            }

            #endregion

            lblError2.Text = "";
            SmallBoxStart1.Title = Lang.Trans("Search Mode");
            LargeBoxStart1.Title = Lang.Trans("Search Terms");
            LargeBoxStart2.Title = Lang.Trans("Search Results");
            LargeBoxStart3.Title = Lang.Trans("Search Terms");
            LargeBoxStart4.Title = Lang.Trans("Search Terms");

            lblSavedSearchText.Text = Lang.Trans("Save this search as");
            lnkQuickSearch.Text = Lang.Trans("Quick Search");

            lnkAdvancedSearch.Text = Lang.Trans("Custom Search");
            if (Config.Search.DistanceSearchEnabled && CurrentUserSession != null/* &&
                CurrentUserSession.ZipCode != ""*/)
            {
                if (lnkDistanceSearch != null)
                    lnkDistanceSearch.Text = Lang.Trans("Distance Search");
                if (trDistanceSearch != null)
                    trDistanceSearch.Visible = true;
            }
            else
            {
                if (trDistanceSearch != null)
                    trDistanceSearch.Visible = false;
            }

            #region Filter online users strings

            btnFilterOnline.Text = Lang.Trans("Filter");

            ddFilterByGender.Items.Clear();
            ddFilterByGender.Items.Add(new ListItem(Lang.Trans("All"), "-1"));
            ddFilterByGender.Items.Add(new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            ddFilterByGender.Items.Add(new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));

            if (Config.Users.CouplesSupport)
            {
                ddFilterByGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            pnlGenderFilterOnline.Visible = !Config.Users.DisableGenderInformation;
            pnlAgeFilterOnline.Visible = !Config.Users.DisableAgeInformation;

            #endregion

            lnkOnlineSearch.Text = Lang.Trans("Who's Online?");
            lnkShowProfileViewers.Text = Lang.Trans("Who Viewed my Profile?");
            lnkShowProfileViewers.Visible = Config.Users.EnableWhoViewedMyProfile ? true : false;

            if (CurrentUserSession == null)
            {
                lnkShowProfileViewers.Enabled = false;
                lblSavedSearchText.Visible = false;
                pnlSavedSearchOptions.Visible = false;
            }

            #region Quick search strings

            BasicSearchHeaderLine.Title = Lang.Trans("Basic Search");
            pnlGenderQuickSearch.Visible = !Config.Users.DisableGenderInformation;
            pnlAgeRangeQuickSearch.Visible = !Config.Users.DisableAgeInformation;
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
                if (Config.Users.DisableGenderInformation)
                    dropGender.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
                else
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
            }

            txtAgeFrom.Text = Config.Users.MinAge.ToString();
            txtAgeTo.Text = Config.Users.MaxAge.ToString();

            btnBasicSearchGo.Text = Lang.Trans("Search");

            UsernameSearchHeaderLine.Title = Lang.Trans("Username Search");
            btnUsernameSearchGo.Text = Lang.Trans("Search");

            KeywordSearchHeaderLine.Title = Lang.Trans("Keyword Search");
            btnKeywordSearchGo.Text = Lang.Trans("Search");

            if (Config.Users.LocationPanelVisible)
                ShowLocation();
            else
                HideLocation();

            #endregion

            #region Custom search strings

            CustomSearchHeaderLine.Title = Lang.Trans("Custom Search");
            pnlGenderCustomSearch.Visible = !Config.Users.DisableGenderInformation;
            pnlAgeCustomSearch.Visible = !Config.Users.DisableAgeInformation;
            if (!Config.Users.InterestedInFieldEnabled || Config.Users.DisableGenderInformation)
                trInterestedIn2.Visible = false;
            else
            {
                dropInterestedIn2.Items.Clear();
                dropInterestedIn2.Items.Add(
                    new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
                dropInterestedIn2.Items.Add(
                    new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    dropInterestedIn2.Items.Add(
                        new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }

                if (Config.Users.InterestedInFieldEnabled && CurrentUserSession != null)
                {
                    dropInterestedIn2.SelectedValue = ((int)CurrentUserSession.Gender).ToString();
                }
            }
            dropGender2.Items.Clear();
            dropGender2.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender2.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender2.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            if (CurrentUserSession != null)
            {
                dropGender2.SelectedValue = ((int)CurrentUserSession.InterestedIn).ToString();
            }

            ddEmailFrequency.Items.Add(new ListItem(Lang.Trans("Weekly"), "7"));
            ddEmailFrequency.Items.Add(new ListItem(Lang.Trans("Bi-weekly"), "14"));
            ddEmailFrequency.Items.Add(new ListItem(Lang.Trans("Monthly"), "30"));

            txtAgeFrom2.Text = Config.Users.MinAge.ToString();
            txtAgeTo2.Text = Config.Users.MaxAge.ToString();

            btnCustomSearchGo.Text = Lang.Trans("Search");

            if (Config.Users.LocationPanelVisible)
                ShowLocation2();
            else
                HideLocation2();
            #endregion

            #region Distance search strings

            pnlGenderDistanceSearch.Visible = !Config.Users.DisableGenderInformation;
            pnlAgeDistanceSearch.Visible = !Config.Users.DisableAgeInformation;
            dropGender3.Items.Clear();
            dropGender3.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            dropGender3.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender3.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }
            if (Config.Users.InterestedInFieldEnabled && CurrentUserSession != null)
            {
                dropGender3.SelectedValue = ((int)CurrentUserSession.InterestedIn).ToString();
            }
            lblDistanceFromUser.Text = Config.Search.MeasureDistanceInKilometers
                                           ?
                                       Lang.Trans("Distance from user (in km)")
                                           : Lang.Trans("Distance from user (in miles)");

            DistanceSearchHeaderLine.Title = Lang.Trans("Distance Search");
            txtAgeFrom3.Text = Config.Users.MinAge.ToString();
            txtAgeTo3.Text = Config.Users.MaxAge.ToString();
            btnDistanceSearchGo.Text = Lang.Trans("Search");

            #endregion
        }

        private void CheckForRequests()
        {
            if (Config.Search.DefaultToCustomSearch)
            {
                Page.RegisterJQuery();
                PrepareCustomSearchFields();
                lnkAdvancedSearch_Click(null, null);
                return;
            }

            if (Session["BasicSearchRequest"] != null)
            {
                btnBasicSearchGo_Click(null, null);
                return;
            }

            if (Session["UsernameSearchRequest"] != null)
            {
                btnUsernameSearchGo_Click(null, null);
                return;
            }

            if (Session["NewUsersSearch"] != null)
            {
                Session.Remove("NewUsersSearch");
                NewUsersSearch();
                return;
            }

            if (Session["BroadcastingUsersSearch"] != null)
            {
                Session.Remove("BroadcastingUsersSearch");
                BroadcastingUsersSearch();
                return;
            }

            if (Session["OnlineUsersSearch"] != null)
            {
                Session.Remove("OnlineUsersSearch");
                lnkOnlineSearch_Click(null, null);
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

            if (Session["MutualFriendsSearch"] != null)
            {
                ShowMutualFriends();
                return;
            }
        }

        private void BroadcastingUsersSearch()
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;

            VideoBroadcastingSearch search = new VideoBroadcastingSearch();

            UserSearchResults results = search.GetResults();
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
            pnlFilterOnline.Visible = false;
        }

        private void PrepareCustomSearchFields()
        {
            PrepareCustomSearchFields(null, false);
        }

        private void LoadLastSearch()
        {
            PrepareCustomSearchFields(null, true);
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
            SavedSearch lastSearch = null;
            plhMaleSearchTerms.Controls.Clear();
            plhFemaleSearchTerms.Controls.Clear();
            plhCoupleSearchTerms.Controls.Clear();

            if (CurrentUserSession != null)
            {
                if (savedSearch != null)
                    lastSearch = savedSearch;
                else if (loadLastSearch)
                    lastSearch = SavedSearch.Load(CurrentUserSession.Username, "_lastsearch_");

                if (lastSearch != null)
                {
                    dropGender2.SelectedValue = ((int)lastSearch.Gender).ToString();
                }
            }

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();
            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    #region Add boxes for topics

                    LiteralControl ltrTopicHeading = new LiteralControl(
                        String.Format("<h3 class=\"SectionHeading\">{0}</h3>", Config.Misc.EnableProfileDataTranslation 
                        ? Lang.Trans(topic.Name) : topic.Name));

                    switch ((User.eGender)Int32.Parse(dropGender2.SelectedValue))
                    {
                        case Classes.User.eGender.Male:
                            plhMaleSearchTerms.Controls.Add(ltrTopicHeading);
                            break;
                        case Classes.User.eGender.Female:
                            plhFemaleSearchTerms.Controls.Add(ltrTopicHeading);
                            break;
                        case Classes.User.eGender.Couple:
                            plhCoupleSearchTerms.Controls.Add(ltrTopicHeading);
                            break;
                    }
                    #endregion

                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions != null)
                    {
                        Dictionary<int, object> dicQuestions = new Dictionary<int, object>();
                        foreach (ProfileQuestion question in questions)
                        {
                            bool maleVisible = question.VisibleForMale &&
                                               (Int32.Parse(dropGender2.SelectedValue) ==
                                                (int)Classes.User.eGender.Male);
                            bool femaleVisible = question.VisibleForFemale &&
                                                 (Int32.Parse(dropGender2.SelectedValue) ==
                                                  (int)Classes.User.eGender.Female);
                            bool coupleVisible = question.VisibleForCouple &&
                                                 (Int32.Parse(dropGender2.SelectedValue) ==
                                                  (int)Classes.User.eGender.Couple);
                            if (question.SearchStyle == ProfileQuestion.eSearchStyle.Hidden ||
                                !(question.VisibleForMale || question.VisibleForFemale || question.VisibleForCouple)
                                )
                            {
                                continue;
                            }

                            IProfileSearchComponent cProfile = null;

                            switch (question.SearchStyle)
                            {
                                case ProfileQuestion.eSearchStyle.MultiChoiceCheck:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchMultiChoiceCheck.ascx");
                                    break;

                                case ProfileQuestion.eSearchStyle.RangeChoiceSelect:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchRangeChoiceSelect.ascx");
                                    break;

                                case ProfileQuestion.eSearchStyle.MultiChoiceSelect:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchMultiChoiceSelect.ascx");
                                    break;

                                case ProfileQuestion.eSearchStyle.SingleChoice:
                                    cProfile = (IProfileSearchComponent)
                                               LoadControl("~/Components/Search/SearchSingleChoice.ascx");
                                    break;

                                default:
                                    break;
                            }
                            if (cProfile != null)
                                (cProfile as Control).ID = question.Id.ToString();

                            if (cProfile == null)
                            {
                                continue;
                            }
                            cProfile.Question = question;

                            if (lastSearch != null)
                            {
                                cProfile.ChoiceIds = lastSearch.ChoiceIds;

                                maleVisible = question.VisibleForMale &&
                                              (Int32.Parse(dropGender2.SelectedValue) == (int)Classes.User.eGender.Male);
                                femaleVisible = question.VisibleForFemale &&
                                                (Int32.Parse(dropGender2.SelectedValue) ==
                                                 (int)Classes.User.eGender.Female);
                                coupleVisible = question.VisibleForCouple &&
                                                (Int32.Parse(dropGender2.SelectedValue) ==
                                                 (int)Classes.User.eGender.Couple);

                                dropGender2_SelectedIndexChanged(null, null);
                            }

                            if (maleVisible)
                            {
                                plhMaleSearchTerms.Controls.Add((Control)cProfile);
                                dicQuestions.Add(question.Id, (Control)cProfile);
                            }
                            else if (femaleVisible)
                            {
                                plhFemaleSearchTerms.Controls.Add((Control)cProfile);
                                dicQuestions.Add(question.Id, (Control)cProfile);
                            }
                            else if (coupleVisible)
                            {
                                plhCoupleSearchTerms.Controls.Add((Control)cProfile);
                                dicQuestions.Add(question.Id, (Control)cProfile);
                            }
                        }

//                        if (lastSearch != null || IsPostBack && !loadLastSearch || loadLastSearch && savedSearch == null)
                        if (IsPostBack || Config.Search.DefaultToCustomSearch)
                            SetCascadeQuestions(questions, dicQuestions);
                    }

                    #region Clear/Close boxes
                    switch ((User.eGender)Int32.Parse(dropGender2.SelectedValue))
                    {
                        case Classes.User.eGender.Male:
                            if (plhMaleSearchTerms.Controls[plhMaleSearchTerms.Controls.Count - 1] is LiteralControl)
                            {
                                plhMaleSearchTerms.Controls.RemoveAt(plhMaleSearchTerms.Controls.Count - 1);
                            }
                            break;
                        case Classes.User.eGender.Female:
                            if (plhFemaleSearchTerms.Controls[plhFemaleSearchTerms.Controls.Count - 1] is LiteralControl)
                            {
                                plhFemaleSearchTerms.Controls.RemoveAt(plhFemaleSearchTerms.Controls.Count - 1);
                            }
                            break;
                        case Classes.User.eGender.Couple:
                            if (plhCoupleSearchTerms.Controls[plhCoupleSearchTerms.Controls.Count - 1] is LiteralControl)
                            {
                                plhCoupleSearchTerms.Controls.RemoveAt(plhCoupleSearchTerms.Controls.Count - 1);
                            }
                            break;
                    }

                    #endregion
                }

                if (lastSearch != null)
                {
                    if (Config.Users.LocationPanelVisible)
                    {
                        CascadingDropDownCountry2.SelectedValue = lastSearch.Country;
                        CascadingDropDownState2.SelectedValue = lastSearch.State;
                        CascadingDropDownCity2.SelectedValue = lastSearch.City;
                        txtZip2.Text = lastSearch.Zip;
                    }

                    dropGender2.SelectedValue = Convert.ToInt32(lastSearch.Gender).ToString();
                    txtAgeFrom2.Text = lastSearch.AgeFrom.ToString();
                    txtAgeTo2.Text = lastSearch.AgeTo.ToString();
                    cbPhotoReq2.Checked = lastSearch.PhotoRequired;
                }
            }
        }

        private void SetCascadeQuestions(ProfileQuestion[] questions, Dictionary<int, object> dicQuestions)
        {
            List<int> lHiddenParentQuestions = new List<int>();

            foreach (ProfileQuestion question in questions)
            {
                ProfileQuestion[] childQuestions =
                    questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();

                bool isParent = childQuestions.Length > 0;
                bool isChild = question.ParentQuestionID.HasValue;
                if (!dicQuestions.ContainsKey(question.Id)) // if current question is hidden
                    continue; 
                Control currentQuestionControl = (Control)dicQuestions[question.Id];

                if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                    ((ICascadeQuestionComponent)currentQuestionControl).GenerateResetValuesJS();

                if (isParent)
                {
                    Dictionary<string, object[]> childClientIDsWithParentQuestionChoices = new Dictionary<string, object[]>();
                    GetChildrenClientIDs(question, questions, dicQuestions, childClientIDsWithParentQuestionChoices);

                    if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                        ((ICascadeQuestionComponent)currentQuestionControl).GenerateJSForChildVisibility(childClientIDsWithParentQuestionChoices);
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
                    Control currentQuestionParentControl = (Control)dicQuestions[question.ParentQuestionID.Value];
                    List<string> parentAnswers = new List<string>();
                    foreach (ProfileAnswer answer in ((IProfileSearchComponent)currentQuestionParentControl).Answers)
                    {
                        parentAnswers.Add(answer.Value);
                    }
                    if (!question.ParentQuestionChoices.Split(':').Any(parentChoice => parentAnswers.Contains(parentChoice)))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        ((IProfileSearchComponent)currentQuestionControl).UserControlPanel.Attributes.Add("style",
                                                                                                             "display:none");
                        ((IProfileSearchComponent)currentQuestionControl).ClearAnswers();
                    }
                }
            }
        }

        private void GetChildrenClientIDs(ProfileQuestion question, ProfileQuestion[] questions, Dictionary<int, object> dicQuestions, Dictionary<string, object[]> childClientIDsWithParentQuestionChoices)
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

        private void PopulateChildrenIDs(ProfileQuestion question, ProfileQuestion[] questions, Dictionary<int, object> dicQuestions, List<string> childClientIDs)
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

        protected void lnkQuickSearch_Click(object sender, EventArgs e)
        {
            pnlQuickSearch.Visible = true;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = false;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;
        }

        protected void lnkAdvancedSearch_Click(object sender, EventArgs e)
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = true;
            pnlSearchResults.Visible = false;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;
            LoadLastSearch();
        }

        protected void lnkOnlineSearch_Click(object sender, EventArgs e)
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;

            ddFilterByGender.SelectedValue = "-1";
            txtFromFilterOnline.Text = Config.Users.MinAge.ToString();
            txtToFilterOnline.Text = Config.Users.MaxAge.ToString();

            OnlineSearch search = new OnlineSearch();

            UserSearchResults results = search.GetResults();
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;

            pnlFilterOnline.Visible = Config.Search.FilterOnlineUsers && results != null;
        }

        protected void lnkShowProfileViewers_Click(object sender, EventArgs e)
        {
            ShowProfileViewers();
        }

        private void NewUsersSearch()
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;

            NewUsersSearch nuSearch = new NewUsersSearch();
            nuSearch.PhotoReq = Config.Users.RequirePhotoToShowInNewUsers;
            nuSearch.ProfileReq = Config.Users.RequireProfileToShowInNewUsers;
            nuSearch.UsersSince = CurrentUserSession.PrevLogin;

            UserSearchResults nuResults = nuSearch.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = nuResults;
        }

        private void ShowProfileViewers()
        {
            if (CurrentUserSession == null) return;

            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;


            string[] profileViewersUsernames = Classes.User.FetchProfileViews(CurrentUserSession.Username,
                                                                              DateTime.MinValue);
            UserSearchResults pvResults = null;

            if (profileViewersUsernames != null)
            {
                pvResults = new UserSearchResults();
                pvResults.Usernames = profileViewersUsernames;
            }

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = true;
            SearchResults.ShowDistance = true;

            SearchResults.ShowViewedOnUsername = CurrentUserSession.Username;
            SearchResults.Results = pvResults;
        }

        private void ShowMutualVotes()
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;

            MutualVoteSearch mutualVoteSearch = new MutualVoteSearch();
            mutualVoteSearch.Username = CurrentUserSession.Username;

            UserSearchResults results = mutualVoteSearch.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        private void ShowMutualFriends()
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;

            var search = (MutualFriendsSearch) Session["MutualFriendsSearch"];
            UserSearchResults results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;

            Session.Remove("MutualFriendsSearch");

            if (results == null)
            {
                ShowFriendsConnection(search.Viewer, search.Viewed);
            }
        }

        private void ShowFriendsConnection(string viewer, string viewed)
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;

            var search = new FriendsConnectionSearch { Viewer = viewer, Viewed = viewed };
            UserSearchResults results = search.GetResults();

            SearchResults.ShowFriendsPath = true;
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        protected void btnBasicSearchGo_Click(object sender, EventArgs e)
        {
            BasicSearch search;

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
                        search.Country = dropCountry.SelectedValue;
                    }
                    if (dropRegion != null)
                    {
                        search.State = dropRegion.SelectedValue;
                    }
                    if (dropCity != null)
                    {
                        search.City = dropCity.SelectedValue;
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

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            UserSearchResults results = search.GetResults();

            SearchResults.Results = results;

            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;
        }

        protected void btnUsernameSearchGo_Click(object sender, EventArgs e)
        {
            UsernameSearch search;
            if (Session["UsernameSearchRequest"] == null)
            {
                search = new UsernameSearch();
                search.Username = txtUsername.Text.Trim();

                if (Config.Users.RequireProfileToShowInSearch)
                {
                    search.HasAnswer = true;
                }
            }
            else
            {
                search = (UsernameSearch)Session["UsernameSearchRequest"];
                Session.Remove("UsernameSearchRequest");
            }

            UserSearchResults results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;

            pnlQuickSearch.Visible = false;
            pnlFilterOnline.Visible = false;
            pnlSearchResults.Visible = true;
        }

        protected void btnKeywordSearchGo_Click(object sender, EventArgs e)
        {
            KeywordSearch search = new KeywordSearch();
            search.Keyword = txtKeyword.Text.Trim();

            if (CurrentUserSession != null 
                && Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
            {
                search.InterestedIn = CurrentUserSession.InterestedIn;
            }

            UserSearchResults results = search.GetResults();
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;

            pnlQuickSearch.Visible = false;
            pnlFilterOnline.Visible = false;
            pnlSearchResults.Visible = true;
        }

        protected void btnCustomSearchGo_Click(object sender, EventArgs e)
        {
            CustomSearch search = new CustomSearch();

            if (Config.Users.RequireProfileToShowInSearch)
            {
                search.HasAnswer = true;
            }
            else
            {
                search.hasAnswer_isSet = false;
            }

            search.Gender = (User.eGender)
                            Convert.ToInt32(dropGender2.SelectedValue);
            if (Config.Users.InterestedInFieldEnabled && !Config.Users.DisableGenderInformation)
                search.InterestedIn = (User.eGender)
                                      Convert.ToInt32(dropInterestedIn2.SelectedValue);
            try
            {
                search.MinAge = Convert.ToInt32(txtAgeFrom2.Text);
                search.MaxAge = Convert.ToInt32(txtAgeTo2.Text);
            }
            catch
            {
            }

            if (Config.Users.LocationPanelVisible)
            {
                if (dropCountry2 != null)
                {
                    search.Country = dropCountry2.SelectedValue;
                }
                if (dropRegion2 != null)
                {
                    search.State = dropRegion2.SelectedValue;
                }
                if (dropCity2 != null)
                {
                    search.City = dropCity2.SelectedValue;
                }

                if (txtZip2 != null)
                {
                    search.Zip = txtZip2.Text;
                }
            }

            if (cbPhotoReq2.Checked)
                search.HasPhoto = true;

            List<ProfileAnswer[]> lSearchTerms = new List<ProfileAnswer[]>();
            List<int> lSearchIDs = new List<int>();

            ControlCollection currentSearchControls = null;

            switch ((User.eGender)Int32.Parse(dropGender2.SelectedValue))
            {
                case Classes.User.eGender.Male:
                    currentSearchControls = plhMaleSearchTerms.Controls;
                    break;
                case Classes.User.eGender.Female:
                    currentSearchControls = plhFemaleSearchTerms.Controls;
                    break;
                case Classes.User.eGender.Couple:
                    currentSearchControls = plhCoupleSearchTerms.Controls;
                    break;
            }
            List<Control> parentControls = new List<Control>();
            GetParentControls(currentSearchControls, parentControls);
            foreach (Control ctrl in currentSearchControls)
            {
                if (ctrl is IProfileSearchComponent)
                {
                    IProfileSearchComponent searchTerm = (IProfileSearchComponent)ctrl;
                    
                    if (searchTerm.Answers != null && searchTerm.Answers.Length > 0)
                    {
                        ProfileQuestion question = ProfileQuestion.Fetch(searchTerm.Answers[0].Question.Id);
                        if (question.ParentQuestionID.HasValue)
                        {
                            var parentControl =
                                parentControls.Cast<IProfileSearchComponent>().FirstOrDefault(
                                    c =>
                                    c != null && c.Answers.Length > 0 &&
                                    c.Answers[0].Question.Id == question.ParentQuestionID);
                            if (parentControl != null)
                            {
                                string[] parentAnswers = parentControl.Answers.Select(a => a.Value).ToArray();
                                if (!question.ParentQuestionChoices.Split(':').Any(parentChoice => parentAnswers.Contains(parentChoice)))
                                {
                                    continue;
                                }
                            }
                        }

                        lSearchTerms.Add(searchTerm.Answers);
                        lSearchIDs.AddRange(searchTerm.ChoiceIds);
                    }
                }
            }

            if (CurrentUserSession != null)
            {
                SaveSavedSearch("_lastsearch_", lSearchIDs.ToArray());

                if (cbSaveSearch.Checked)
                {
                    if (txtSavedSearchName.Text == "")
                    {
                        return;
                    }
                    SaveSavedSearch(txtSavedSearchName.Text, lSearchIDs.ToArray());
                    LoadSavedSearches();
                }
            }

            search.Answers = lSearchTerms.ToArray();


            UserSearchResults results = search.GetResults();

            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;

            pnlCustomSearch.Visible = false;
            pnlFilterOnline.Visible = false;
            pnlSearchResults.Visible = true;
        }

        private DateTime findNextFriday(DateTime date)
        {
            do date = date.AddDays(1);
            while (date.DayOfWeek != DayOfWeek.Friday);

            return date;
        }

        private void GetParentControls(ControlCollection collection, List<Control> parentControls)
        {
            foreach (Control ctrl in collection)
            {
                if (ctrl is IProfileSearchComponent) parentControls.Add(ctrl);
            }
        }

        private void SaveSavedSearch(string name, int[] lSearchIDs)
        {
            User.eGender gender = (User.eGender)Convert.ToInt32(dropGender2.SelectedValue);

            string country = "";
            string state = "";
            string zip = "";
            string city = "";

            if (Config.Users.LocationPanelVisible)
            {
                country = dropCountry2.SelectedValue;
                state = dropRegion2.SelectedValue;
                city = dropCity2.SelectedValue;
                zip = txtZip2.Text;
            }

            int ageFrom = Config.Users.MinAge;
            int ageTo = Config.Users.MaxAge;

            try
            {
                ageFrom = Convert.ToInt32(txtAgeFrom2.Text);
                ageTo = Convert.ToInt32(txtAgeTo2.Text);
            }
            catch (FormatException)
            {
            }

            bool photoRequired = cbPhotoReq2.Checked;
            bool emailMatches = false;
            int emailFrequency = 7;
            DateTime? nextEmailDate = null;

            if (cbSaveSearch.Checked && name != "_lastsearch_")
            {
                emailMatches = cbEmailMe.Checked;
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

        /// <summary>
        /// Loads the saved searches.
        /// </summary>
        private void LoadSavedSearches()
        {
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

                SmallBoxStart2.Visible = dtSavedSearches.Rows.Count > 0;
                SmallBoxEnd2.Visible = SmallBoxStart2.Visible;
                divSavedSearches.Visible = SmallBoxStart2.Visible;
            }
        }

        protected void lnkDistanceSearch_Click(object sender, EventArgs e)
        {
            CascadingDropDownCountry2.ServiceMethod = "GetCountries";
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = false;
            pnlFilterOnline.Visible = false;
            pnlDistanceSearch.Visible = true;
        }

        protected void btnDistanceSearchGo_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("Login.aspx");
            
            DistanceSearch search = new DistanceSearch();
            if (Config.Users.RequireProfileToShowInSearch)
            {
                search.HasAnswer = true;
            }
            search.Gender = (User.eGender)
                            Convert.ToInt32(dropGender3.SelectedValue);
            try
            {
                search.MinAge = Convert.ToInt32(txtAgeFrom3.Text);
                search.MaxAge = Convert.ToInt32(txtAgeTo3.Text);
            }
            catch
            {
            }

            search.FromLocation = Config.Users.GetLocation(CurrentUserSession);
            try
            {
                search.Distance =
                    Config.Search.MeasureDistanceInKilometers
                        ?
                    Convert.ToDouble(txtDistanceFromUser.Text) * 0.621371192
                        : Convert.ToDouble(txtDistanceFromUser.Text);

                if (search.Distance > Config.Search.DistanceSearchMaxDistance)
                {
                    lblError2.Text = Config.Search.MeasureDistanceInKilometers
                                          ?
                                      String.Format(Lang.Trans("Maximum allowed distance value is {0} kilometers."),
                                                    Config.Search.DistanceSearchMaxDistance)
                                          :
                                      String.Format(Lang.Trans("Maximum allowed distance value is {0} miles."),
                                                    Config.Search.DistanceSearchMaxDistance);
                    return;
                }
            }
            catch
            {
                lblError2.Text = Lang.Trans("The distance value is invalid.");
                return;
            }

            if (cbPhotoReq3.Checked)
                search.PhotoReq = true;
            search.MaxResults = Config.Search.DistanceSearchMaxUsers;

            UserSearchResults results;
            try
            {
                results = search.GetResults();

                SearchResults.ShowLastOnline = true;
                SearchResults.ShowViewedOn = false;
                SearchResults.ShowDistance = true;
            }
            catch (ArgumentNullException)
            {
                lblError2.Text = Lang.Trans("Your zipcode is not in our database. You cannot use distance search.");
                return;
            }
            catch (ArgumentException ex)
            {
                lblError2.Text = Lang.Trans(ex.Message);
                return;
            }

            SearchResults.Results = results;

            pnlDistanceSearch.Visible = false;
            pnlFilterOnline.Visible = false;
            pnlSearchResults.Visible = true;

            lblError2.Text = "";
        }

        private void dlSavedSearches_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "SavedSearchView")
            {
                CascadingDropDownCountry2.ServiceMethod = "GetCountriesForSavedSearch";
                PrepareCustomSearchFields(SavedSearch.Load(Convert.ToInt32(e.CommandArgument)));
                txtSavedSearchName.Text = ((LinkButton)e.Item.FindControl("lnkSavedSearch")).Text;
                pnlQuickSearch.Visible = false;
                pnlCustomSearch.Visible = true;
                pnlSearchResults.Visible = false;
                pnlDistanceSearch.Visible = false;
                pnlFilterOnline.Visible = false;
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

        private void HideLocation()
        {
            if (pnlCountry != null)
            {
                pnlCountry.Visible = false;
            }
            if (pnlState != null)
            {
                pnlState.Visible = false;
            }
            if (pnlZip != null)
            {
                pnlZip.Visible = false;
            }
            if (pnlCity != null)
            {
                pnlCity.Visible = false;
            }
        }

        private void ShowLocation()
        {
            if (pnlCountry != null)
            {
                pnlCountry.Visible = true;
            }
            if (pnlState != null)
            {
                pnlState.Visible = true;
            }
            if (pnlZip != null)
            {
                pnlZip.Visible = true;
            }
            if (pnlCity != null)
            {
                pnlCity.Visible = true;
            }
        }

        private void HideLocation2()
        {
            if (pnlCountry2 != null)
            {
                pnlCountry2.Visible = false;
            }
            if (pnlState2 != null)
            {
                pnlState2.Visible = false;
            }
            if (pnlZip2 != null)
            {
                pnlZip2.Visible = false;
            }
            if (pnlCity2 != null)
            {
                pnlCity2.Visible = false;
            }
        }

        private void ShowLocation2()
        {
            if (pnlCountry2 != null)
            {
                pnlCountry2.Visible = true;
            }
            if (pnlState2 != null)
            {
                pnlState2.Visible = true;
            }
            if (pnlZip2 != null)
            {
                pnlZip2.Visible = true;
            }
            if (pnlCity2 != null)
            {
                pnlCity2.Visible = true;
            }
        }

        private void dropGender2_SelectedIndexChanged(object sender, EventArgs e)
        {
            plhMaleSearchTerms.Visible = false;
            plhFemaleSearchTerms.Visible = false;
            plhCoupleSearchTerms.Visible = false;

            switch ((User.eGender)Int32.Parse(dropGender2.SelectedValue))
            {
                case Classes.User.eGender.Male:
                    plhMaleSearchTerms.Visible = true;
                    break;
                case Classes.User.eGender.Female:
                    plhFemaleSearchTerms.Visible = true;
                    break;
                case Classes.User.eGender.Couple:
                    plhCoupleSearchTerms.Visible = true;
                    break;
            }
        }

        protected void btnFilterOnline_Click(object sender, EventArgs e)
        {
            pnlQuickSearch.Visible = false;
            pnlCustomSearch.Visible = false;
            pnlSearchResults.Visible = true;
            pnlDistanceSearch.Visible = false;

            OnlineSearch search = new OnlineSearch();

            if (Config.Search.FilterOnlineUsers)
            {
                try
                {
                    search.MinAge = Convert.ToInt32(txtFromFilterOnline.Text);
                    search.MaxAge = Convert.ToInt32(txtToFilterOnline.Text);
                }
                catch (FormatException)
                {
                    lblError.Text = Lang.Trans("Invalid age");
                    return;
                }

                if (ddFilterByGender.SelectedValue != "-1")
                {
                    search.Gender = (User.eGender)Convert.ToInt32(ddFilterByGender.SelectedValue);
                }
            }

            UserSearchResults results = search.GetResults();
            SearchResults.ShowLastOnline = true;
            SearchResults.ShowViewedOn = false;
            SearchResults.ShowDistance = true;

            SearchResults.Results = results;
        }

        protected void cbSaveSearch_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveSearch.Checked)
                pnlEmailMe.Visible = true;
            else
                pnlEmailMe.Visible = false;
        }
    }
}