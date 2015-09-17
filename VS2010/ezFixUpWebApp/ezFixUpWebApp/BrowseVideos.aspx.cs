using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class BrowseVideos : PageBase
    {
        private bool paginatorVisible = true;
        private bool updateHistory = true;

        public BrowseVideos()
        {
            RequiresAuthorization = false;
        }

        public VideoUploadSearchResults VideoUploadResults
        {
            set
            {
                if (ViewState["VideoUploadSearchResults_guid"] == null)
                {
                    ViewState["VideoUploadSearchResults_guid"] = Guid.NewGuid().ToString();
                }

                if (value != null && value.Ids.Length == 0)
                    value = null;

                Global.GetSessionState()["SearchResults" + ViewState["VideoUploadSearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["VideoUploadSearchResults_guid"] != null)
                {
                    return (VideoUploadSearchResults)
                           Global.GetSessionState()["SearchResults" + ViewState["VideoUploadSearchResults_guid"]];
                }
                return null;
            }
        }

        public VideoEmbedsSearchResults VideoEmbedResults
        {
            set
            {
                if (ViewState["VideoEmbedsSearchResults_guid"] == null)
                {
                    ViewState["VideoEmbedsSearchResults_guid"] = Guid.NewGuid().ToString();
                }

                if (value != null && value.Ids.Length == 0)
                    value = null;

                Global.GetSessionState()["SearchResults" + ViewState["VideoEmbedsSearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["VideoEmbedsSearchResults_guid"] != null)
                {
                    return (VideoEmbedsSearchResults)
                           Global.GetSessionState()["SearchResults" + ViewState["VideoEmbedsSearchResults_guid"]];
                }
                return null;
            }
        }

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
                if (!Config.Misc.EnableVideoUpload && !Config.Misc.EnableYouTubeVideos)
                {
                    Response.Redirect("~/Home.aspx");
                    return;
                }

                LoadStrings();
            }

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
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

        private void LoadStrings()
        {
            ddGender.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
            ddGender.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                ddGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
            }

            if (CurrentUserSession != null && !Config.Users.DisableGenderInformation)
            {
                if (Config.Users.InterestedInFieldEnabled)
                    ddGender.SelectedValue = ((int)CurrentUserSession.InterestedIn).ToString();
                else
                {
                    switch (CurrentUserSession.Gender)
                    {
                        case Classes.User.eGender.Male:
                            ddGender.SelectedValue = ((int)Classes.User.eGender.Female).ToString();
                            break;
                        case Classes.User.eGender.Female:
                            ddGender.SelectedValue = ((int)Classes.User.eGender.Male).ToString();
                            break;
                        case Classes.User.eGender.Couple:
                            break;
                        default:
                            break;
                    }
                }
            }

            if (Config.Misc.EnableVideoUpload)
                ddVideoType.Items.Add(new ListItem("Uploaded Video".Translate(), "Uploaded Video"));
            if (Config.Misc.EnableYouTubeVideos)
                ddVideoType.Items.Add(new ListItem("Embedded Video".Translate(), "Embedded Video"));
            txtFromAge.Text = Config.Users.MinAge.ToString();
            txtToAge.Text = Config.Users.MaxAge.ToString();
            pnlGenderFilterOnline.Visible = !Config.Users.DisableGenderInformation;
            pnlAgeFilterOnline.Visible = !Config.Users.DisableAgeInformation;
            LargeBoxStart.Title = "Browse Videos".Translate();
            SmallBoxStart2.Title = "Search Options".Translate();
            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");
            btnSearch.Text = "Search".Translate();
            PaginatorEnabled = false;
        }

        private void PreparePaginator()
        {
            int videosPerPage = Config.Search.VideosPerPage;
            if (VideoUploadResults == null && VideoEmbedResults == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (VideoUploadResults == null && VideoEmbedResults == null
                || VideoUploadResults != null && CurrentPage >= VideoUploadResults.GetTotalPages(videosPerPage)
                || VideoEmbedResults != null && CurrentPage >= VideoEmbedResults.GetTotalPages(videosPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (VideoUploadResults != null && VideoUploadResults.Ids.Length > 0)
            {
                int fromVideo = (CurrentPage - 1) * videosPerPage + 1;
                int toVideo = CurrentPage * videosPerPage;
                if (VideoUploadResults.Ids.Length < toVideo)
                {
                    toVideo = VideoUploadResults.Ids.Length;
                }

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromVideo, toVideo, VideoUploadResults.Ids.Length);
            }
            else if (VideoEmbedResults != null && VideoEmbedResults.Ids.Length > 0)
            {
                int fromVideo = (CurrentPage - 1) * videosPerPage + 1;
                int toVideo = CurrentPage * videosPerPage;
                if (VideoEmbedResults.Ids.Length < toVideo)
                {
                    toVideo = VideoEmbedResults.Ids.Length;
                }

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromVideo, toVideo, VideoEmbedResults.Ids.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        private void LoadResultsPage()
        {
            var dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("Title", typeof(string));
            dtResults.Columns.Add("ThumbnailUrl", typeof(string));
            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("Age");
            dtResults.Columns.Add("Gender");

            if (VideoUploadResults != null || VideoEmbedResults != null)
            {
                VideoUpload[] videoUploads = null;
                VideoEmbed[] videoEmbeds = null;
                if (PaginatorEnabled)
                {
                    int videosPerPage = Config.Search.VideosPerPage;
                    if (VideoUploadResults != null)
                        videoUploads = VideoUploadResults.GetPage(CurrentPage, videosPerPage);
                    else if (VideoEmbedResults != null)
                        videoEmbeds = VideoEmbedResults.GetPage(CurrentPage, videosPerPage);
                }
                else
                {
                    if (VideoUploadResults != null)
                        videoUploads = VideoUploadResults.Get();
                    else if (VideoEmbedResults != null)
                        videoEmbeds = VideoEmbedResults.Get();
                }

                if (videoUploads != null && videoUploads.Length > 0)
                {
                    foreach (VideoUpload videoUpload in videoUploads)
                    {
                        User user;
                        try
                        {
                            user = Classes.User.Load(videoUpload.Username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        string videoThumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png", Config.Urls.Home,
                                                            user.Username, videoUpload.Id);
                        string age = !Config.Users.DisableGenderInformation ? user.Age.ToString() : String.Empty;
                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        dtResults.Rows.Add(new object[]
                                               {
                                                   String.Empty,
                                                   videoThumbnail,
                                                   user.Username, age, gender
                                               });
                    }
                }
                else if (videoEmbeds != null && videoEmbeds.Length > 0)
                {
                    foreach (VideoEmbed videoEmbed in videoEmbeds)
                    {
                        User user;
                        try
                        {
                            user = Classes.User.Load(videoEmbed.Username);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }

                        string age = !Config.Users.DisableGenderInformation ? user.Age.ToString() : String.Empty;
                        string gender = !Config.Users.DisableGenderInformation ? user.Gender.ToString() : String.Empty;
                        dtResults.Rows.Add(new object[]
                                               {
                                                   videoEmbed.Title,
                                                   videoEmbed.ThumbUrl,
                                                   user.Username, age, gender
                                               });
                    }
                }
            }
            else
            {
                dlVideos.Visible = false;
            }

            dlVideos.DataSource = dtResults;
            dlVideos.DataBind();
        }

        void scriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            if (VideoUploadResults == null && VideoEmbedResults == null)
                Response.Redirect("~/Home.aspx");

            int navigatePage;
            try
            {
                navigatePage = e.State.Count == 0 ? 1 : Convert.ToInt32(e.State[0]);
            }
            catch (FormatException)
            {
                navigatePage = 1;
            }
            int videosPerPage = Config.Search.VideosPerPage;
            if (VideoUploadResults != null)
            {
                if (navigatePage <= VideoUploadResults.GetTotalPages(videosPerPage) && navigatePage > 0)
                {
                    updateHistory = false;
                    CurrentPage = navigatePage;
                }
            }
            else if (VideoEmbedResults != null)
            {
                if (navigatePage <= VideoEmbedResults.GetTotalPages(videosPerPage) && navigatePage > 0)
                {
                    updateHistory = false;
                    CurrentPage = navigatePage;
                }
            }
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage = 1;
            }
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (VideoUploadResults == null && VideoEmbedResults == null)
                Response.Redirect("~/Home.aspx");

            int videosPerPage = Config.Search.VideosPerPage;
            if (VideoUploadResults != null)
            {
                if (CurrentPage < VideoUploadResults.GetTotalPages(videosPerPage))
                {
                    CurrentPage++;
                }
            }
            else if (VideoEmbedResults != null)
            {
                if (CurrentPage < VideoEmbedResults.GetTotalPages(videosPerPage))
                {
                    CurrentPage++;
                }
            }
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (VideoUploadResults == null && VideoEmbedResults == null)
                Response.Redirect("~/Home.aspx");

            int videosPerPage = Config.Search.VideosPerPage;
            if (VideoUploadResults != null)
            {
                if (CurrentPage < VideoUploadResults.GetTotalPages(videosPerPage))
                {
                    CurrentPage = VideoUploadResults.GetTotalPages(videosPerPage);
                }
            }
            else if (VideoUploadResults != null)
            {
                if (CurrentPage < VideoEmbedResults.GetTotalPages(videosPerPage))
                {
                    CurrentPage = VideoEmbedResults.GetTotalPages(videosPerPage);
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddVideoType.SelectedValue == "Uploaded Video")
            {
                VideoUploadsSearch vuSearch = new VideoUploadsSearch();
                vuSearch.Approved = true;
                vuSearch.IsPrivate = false;
                vuSearch.Gender = (User.eGender) Convert.ToInt32(ddGender.SelectedValue);
                int minAge = Config.Users.MinAge;
                Int32.TryParse(txtFromAge.Text, out minAge);
                vuSearch.MinAge = minAge;
                int maxAge = Config.Users.MaxAge;
                Int32.TryParse(txtToAge.Text, out maxAge);
                vuSearch.MaxAge = maxAge;
                vuSearch.SortAsc = true;
                vuSearch.SortColumn = VideoUploadsSearch.eSortColumn.ID;
                VideoUploadResults = vuSearch.GetResults();
                VideoEmbedResults = null;
            }
            else if (ddVideoType.SelectedValue == "Embedded Video")
            {
                VideoEmbedsSearch veSearch = new VideoEmbedsSearch();
                veSearch.Gender = (User.eGender)Convert.ToInt32(ddGender.SelectedValue);
                int minAge = Config.Users.MinAge;
                Int32.TryParse(txtFromAge.Text, out minAge);
                veSearch.MinAge = minAge;
                int maxAge = Config.Users.MaxAge;
                Int32.TryParse(txtToAge.Text, out maxAge);
                if (txtKeyword.Text.Trim().Length > 0)
                    veSearch.Keyword = txtKeyword.Text.Trim();
                veSearch.SortColumn = VideoEmbedsSearch.eSortColumn.ID;
                veSearch.SortAsc = true;
                VideoEmbedResults = veSearch.GetResults();
                VideoUploadResults = null;
            }

            PaginatorEnabled = true;
        }

        protected void dlVideos_ItemCreated(object sender, DataListItemEventArgs e)
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

        protected void ddVideoType_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlKeyword.Visible = ddVideoType.SelectedValue == "Embedded Video";
            txtKeyword.Text = String.Empty;
        }
    }
}
