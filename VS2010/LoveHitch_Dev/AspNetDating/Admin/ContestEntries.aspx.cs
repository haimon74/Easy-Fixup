using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ContestEntries : AdminPageBase
    {
        private int PhotosPerPage
        {
            get { return Convert.ToInt32(dropPhotosPerPage.SelectedValue); }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Ratings.EnablePhotoContests)
                {
                    StatusPageMessage = Lang.TransA("Photo contest option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                LoadStrings();
                PopulateDropDownLists();
                PreparePaginator();
                
                int contestId;
                if (Int32.TryParse(Request.Params["cid"], out contestId))
                {
                    if (ddContests.Items.FindByValue(contestId.ToString()) != null)
                    {
                        ddContests.SelectedValue = contestId.ToString();
                        btnSearch_Click(null, null);
                    }
                }                
            }
        }

        private void PopulateDropDownLists()
        {
            ddContests.Items.Add(String.Empty);
            
            foreach (PhotoContest contest in PhotoContest.LoadAll())
            {
                ddContests.Items.Add(new ListItem(contest.Name, contest.Id.ToString()));
            }

            for (int i = 5; i <= 50; i += 5)
                dropPhotosPerPage.Items.Add(i.ToString());
            dropPhotosPerPage.SelectedValue = 20.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageContests;
            base.OnInit(e);
        }

        private void LoadStrings()
        {
            btnSearch.Text = Lang.TransA("Search");
            lblPhotosPerPage.Text = Lang.TransA("Photos per page");

            dgContestEntries.Columns[0].HeaderText = Lang.TransA("Username");
            dgContestEntries.Columns[1].HeaderText = Lang.TransA("Photo");
            dgContestEntries.Columns[2].HeaderText = Lang.TransA("Contest Name");
            dgContestEntries.Columns[3].HeaderText = Lang.TransA("Actions");


            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");
        }

        public PhotoContestEntriesSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (PhotoContestEntriesSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                PreparePaginator();
                LoadResultsPage();
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

        private void PreparePaginator()
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(PhotosPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.PhotoContestEntries.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * PhotosPerPage + 1;
                int toUser = CurrentPage * PhotosPerPage;
                if (Results.PhotoContestEntries.Length < toUser)
                    toUser = Results.PhotoContestEntries.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.PhotoContestEntries.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void LoadResultsPage()
        {
            PreparePaginator();

            DataTable dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("Id");
            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("PhotoId");
            dtResults.Columns.Add("ContestName");
            
            

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);

                PhotoContestEntry[] contestEntries = Results.GetPage(CurrentPage, PhotosPerPage);

                if (contestEntries != null && contestEntries.Length > 0)
                    foreach (PhotoContestEntry contestEntry in contestEntries)
                    {
                        PhotoContest contest = PhotoContest.Load(contestEntry.ContestId);

                        dtResults.Rows.Add(new object[]
                                               {
                                                   contestEntry.Id,
                                                   contestEntry.Username,
                                                   contestEntry.PhotoId,
                                                   contest.Name
                                               });
                    }

                tblHideSearch.Visible = true;
            }
            else
            {
                tblHideSearch.Visible = false;
            }

            Trace.Write("Binding...");
            dgContestEntries.DataSource = dtResults;
            dgContestEntries.DataBind();
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;

            LoadResultsPage();
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;

            LoadResultsPage();
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(PhotosPerPage))
                CurrentPage++;

            LoadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(PhotosPerPage))
                CurrentPage = Results.GetTotalPages(PhotosPerPage);

            LoadResultsPage();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        public string SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return "ContestName";
                }
                else
                {
                    return (string)ViewState["sortField"];
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
        
        private void PerformSearch()
        {
            string username = null;
            if (txtUsername.Text.Trim().Length > 0)
                username = txtUsername.Text.Trim();
            int? contestID = null;
            
            if (ddContests.SelectedIndex > 0)
                contestID = Int32.Parse(ddContests.SelectedValue);

            PhotoContestEntriesSearch search = new PhotoContestEntriesSearch();
            search.Username = username;
            search.ContestID = contestID;
			search.SortColumn = SortField;
			search.SortAsc = SortAsc;
            
            Results = search.GetResults();
            CurrentPage = 1;
        }

        protected void dropPhotosPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        protected void dgContestEntries_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField)
                {
                    SortAsc = !SortAsc;
                }

                SortField = e.SortExpression;
            }

            PerformSearch();
        }

        protected void dgContestEntries_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");

            if (!HasWriteAccess)
                lnkDelete.Enabled = false;
            else
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this photo?")));
        }

        protected void dgContestEntries_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch(e.CommandName)
            {
                case "Delete":
                    int id = Convert.ToInt32(e.CommandArgument);
                    PhotoContestEntry.Delete(id);
                    PerformSearch();                    
                    break;
            }            
        }        
        
    }
}
