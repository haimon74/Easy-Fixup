using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class BrowseVideoUploads : AdminPageBase
    {
        private int VideoUploadsPerPage
        {
            get { return Convert.ToInt32(ddVideoUploadsPerPage.SelectedValue); }
        }

        public VideoUploadsSearch.eSortColumn SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return VideoUploadsSearch.eSortColumn.ID;
                }
                else
                {
                    return (VideoUploadsSearch.eSortColumn)ViewState["sortField"];
                }
            }
            set { ViewState["sortField"] = value; }
        }

        public bool SortAsc
        {
            get
            {
                if (ViewState["sortAsc"] == null)
                {
                    return true;
                }
                else
                {
                    return Convert.ToBoolean(ViewState["sortAsc"]);
                }
            }
            set { ViewState["sortAsc"] = value; }
        }

        protected string VideoUploadUrl
        {
            get { return ViewState["VideoUploadUrl"] as string; }
            set { ViewState["VideoUploadUrl"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseVideoUploads;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                populateFilters();
                preparePaginator();
            }
        }

        private void loadStrings()
        {
            ddApproved.Items.Add(Lang.TransA("Yes"));
            ddApproved.Items.Add(Lang.TransA("No"));
            ddIsPrivate.Items.Add(Lang.TransA("Yes"));
            ddIsPrivate.Items.Add(Lang.TransA("No"));

            btnSearch.Text = Lang.TransA("Search");

            dgVideoUploads.Columns[0].HeaderText = Lang.TransA("Username");
            dgVideoUploads.Columns[2].HeaderText = Lang.TransA("Approved");
            dgVideoUploads.Columns[3].HeaderText = Lang.TransA("Private");

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");
        }

        private void populateFilters()
        {
            for (int i = 5; i <= 50; i += 5)
                ddVideoUploadsPerPage.Items.Add(i.ToString());
            ddVideoUploadsPerPage.SelectedValue = Config.Search.VideosPerPage.ToString();
        }

        public VideoUploadSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (VideoUploadSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                preparePaginator();
                loadResultsPage();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                    return 1;
                else
                    return (int)ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            set { pnlPaginator.Visible = value; }
        }

        private void preparePaginator()
        {
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(VideoUploadsPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Ids.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * VideoUploadsPerPage + 1;
                int toUser = CurrentPage * VideoUploadsPerPage;
                if (Results.Ids.Length < toUser)
                    toUser = Results.Ids.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Ids.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void loadResultsPage()
        {
            preparePaginator();

            DataTable dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("ID");
            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("Approved");
            dtResults.Columns.Add("IsPrivate");
            dtResults.Columns.Add("ThumbnailUrl");
            dtResults.Columns.Add("VideoUrl");

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);
                VideoUpload[] videoUploads = Results.GetPage(CurrentPage, VideoUploadsPerPage);

                if (videoUploads != null && videoUploads.Length > 0)
                    foreach (VideoUpload videoUpload in videoUploads)
                    {
                        string videoUploadUrl = String.Format("{0}/UserFiles/{1}/video_{2}.flv", Config.Urls.Home,
                                                                        videoUpload.Username, videoUpload.Id);
                        string videoThumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png", Config.Urls.Home,
                                                            videoUpload.Username, videoUpload.Id);
                        if (!File.Exists(Server.MapPath(String.Format("~/UserFiles/{0}/video_{1}.png",
                                                                        videoUpload.Username, videoUpload.Id))))
                        {
                            videoThumbnail = ResolveUrl("~/Images/uploadedvideo.gif");
                        }

                        dtResults.Rows.Add(new object[]
                                               {
                                                   videoUpload.Id,
                                                   videoUpload.Username,
                                                   videoUpload.Approved,
                                                   videoUpload.IsPrivate,
                                                   videoThumbnail,
                                                   videoUploadUrl
                                               });
                    }

                tblHideSearch.Visible = true;
            }
            else
            {
                tblHideSearch.Visible = false;
            }

            Trace.Write("Binding...");
            dgVideoUploads.DataSource = dtResults;
            dgVideoUploads.DataBind();
        }

        private void performSearch()
        {
            VideoUploadsSearch search = new VideoUploadsSearch();
            if (txtUsername.Text.Trim().Length > 0)
                search.Username = txtUsername.Text.Trim();
            if (ddApproved.SelectedIndex > 0)
                search.Approved = ddApproved.SelectedIndex == 1;
            if (ddIsPrivate.SelectedIndex > 0)
                search.IsPrivate = ddIsPrivate.SelectedIndex == 1;

            search.SortColumn = SortField;
            search.SortAsc = SortAsc;

            Results = search.GetResults();

            CurrentPage = 1;
            loadResultsPage();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            performSearch();
        }

        protected void ddVideoUploadsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;

            loadResultsPage();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            loadResultsPage();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(VideoUploadsPerPage))
                CurrentPage++;

            loadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(VideoUploadsPerPage))
                CurrentPage = Results.GetTotalPages(VideoUploadsPerPage);

            loadResultsPage();
        }

        protected void dgVideoUploads_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == ((int) SortField).ToString())
                {
                    SortAsc = !SortAsc;
                }

                SortField = (VideoUploadsSearch.eSortColumn) Convert.ToInt32(e.SortExpression);
            }

            performSearch();
        }

        protected void dgVideoUploads_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                if (!HasWriteAccess)
                    return;

                int id = Convert.ToInt32(e.CommandArgument);
                VideoUpload videoUpload = VideoUpload.Load(id);

                if (videoUpload != null)
                {
                    string path = Server.MapPath(String.Format("~/UserFiles/{0}/video_{1}.flv",
                                                               videoUpload.Username, videoUpload.Id));
                    VideoUpload.Delete(id);
                    File.Delete(path);
                    performSearch();
                }
            }
            else if (e.CommandName == "ViewVideo")
            {
                HtmlGenericControl pnlVideoUpload = (HtmlGenericControl) e.Item.FindControl("pnlVideoUpload");
                pnlVideoUpload.Visible = true;
                ImageButton imgbtnViewVideo = (ImageButton) e.Item.FindControl("imgbtnViewVideo");
                imgbtnViewVideo.Visible = false;
            }
        }

        protected void dgVideoUploads_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

            if (!HasWriteAccess)
                lnkDelete.Enabled = false;
            else
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this video?")));
        }
    }
}
