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
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class ManageContests : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageContests;

            base.OnInit(e);
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
                preparePaginator();
            }
        }

        public PhotoContestSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (PhotoContestSearchResults)
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
        
        private int? EditedContest
        {
            get
            {
                return ViewState["EditedContest"] as int?;
            }
            
            set
            {
                ViewState["EditedContest"] = value;
            }
        }

        private int ContestsPerPage
        {
            get { return 2; }
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(ContestsPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.PhotoContests.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * ContestsPerPage + 1;
                int toUser = CurrentPage * ContestsPerPage;
                if (Results.PhotoContests.Length < toUser)
                    toUser = Results.PhotoContests.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.PhotoContests.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }        

        private void LoadStrings()
        {
            btnAddNewContest.Text = Lang.TransA("Add New Contest");
            btnSearch.Text = Lang.TransA("Search");
            
            ddActive.Items.Clear();
            ddActive.Items.Add(String.Empty);
            ddActive.Items.Add(new ListItem(Lang.TransA("Active"), true.ToString()));
            ddActive.Items.Add(new ListItem(Lang.TransA("Inactive"), false.ToString()));

            dgContests.Columns[0].HeaderText = Lang.TransA("Name");
            dgContests.Columns[1].HeaderText = Lang.TransA("Gender");
            dgContests.Columns[2].HeaderText = Lang.TransA("Age Range");
            dgContests.Columns[3].HeaderText = Lang.TransA("Date Created");
            dgContests.Columns[4].HeaderText = Lang.TransA("Date Ends");
            dgContests.Columns[5].HeaderText = Lang.TransA("Actions");

            dpDateEnds.MinYear = DateTime.Now.Year;
            dpDateEnds.MaxYear = DateTime.Now.Year + 5;

            ddGender.Items.Clear();
            ddGender.Items.Add(String.Empty);

            if (!Config.Users.DisableGenderInformation)
            {
                foreach (User.eGender gender in Enum.GetValues(typeof(User.eGender)))
                {
                    ddGender.Items.Add(new ListItem(gender.ToString(), ((int)gender).ToString()));
                }
            }
            else
            {
                dgContests.Columns[1].Visible = false;
                pnlGender.Visible = false;
            }

            ddFromAge.Items.Clear();
            ddToAge.Items.Clear();
            ddFromAge.Items.Add(String.Empty);
            ddToAge.Items.Add(String.Empty);

            if (!Config.Users.DisableAgeInformation)
            {
                for (int i = Config.Users.MinAge; i <= Config.Users.MaxAge; ++i)
                {
                    ddFromAge.Items.Add(i.ToString());
                    ddToAge.Items.Add(i.ToString());
                }
            }
            else
            {
                dgContests.Columns[2].Visible = false;
                pnlAge.Visible = false;
            }
            
            if (!HasWriteAccess)
                btnSave.Enabled = false;

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");
            
            btnSave.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void loadResultsPage()
        {
            
            
            //if (contests.Length == 0)
            //{
            //    MessageBox.Show(Lang.TransA("There are no photo contests created!"), Misc.MessageType.Error);
            //    dgContests.Visible = false;
            //}
            //else
            //{
            DataTable dtContests = new DataTable("SearchResults");
            dtContests.Columns.Add("Id");
            dtContests.Columns.Add("Name");
            dtContests.Columns.Add("Gender");
            dtContests.Columns.Add("AgeRange");
            dtContests.Columns.Add("DateCreated");
            dtContests.Columns.Add("DateEnds");
            
            if (Results != null)
            {
                PhotoContest[] contests = Results.GetPage(CurrentPage, ContestsPerPage);                

                if (contests != null && contests.Length > 0)                
                foreach (PhotoContest contest in contests)
                {
                    string ageRange = String.Empty;
                    if (contest.MinAge != null && contest.MaxAge != null)
                    {
                        ageRange = contest.MinAge + "-" + contest.MaxAge;
                    }
                    else if (contest.MinAge == null && contest.MaxAge != null)
                    {
                        ageRange = "<=" + contest.MaxAge;
                    }
                    else if (contest.MinAge != null && contest.MaxAge == null)
                    {
                        ageRange = ">=" + contest.MinAge;
                    }

                    dtContests.Rows.Add(new object[]
                                            {
                                                contest.Id,
                                                contest.Name,
                                                contest.Gender != null? Lang.TransA(contest.Gender.ToString()):String.Empty,
                                                ageRange,
                                                contest.DateCreated.ToShortDateString(),
                                                contest.DateEnds.HasValue?contest.DateEnds.Value.ToShortDateString():String.Empty
                                            }
                        );
                }

                tblHideSearch.Visible = true;
            }
            else
            {
                tblHideSearch.Visible = false;
            }

            dgContests.DataSource = dtContests;
            dgContests.DataBind();

            dgContests.Visible = true;
            //}
        }
        
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            performSearch();
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
            if (CurrentPage < Results.GetTotalPages(ContestsPerPage))
                CurrentPage++;

            loadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(ContestsPerPage))
                CurrentPage = Results.GetTotalPages(ContestsPerPage);

            loadResultsPage();
        }

        private void performSearch()
        {
            PhotoContestSearch search = new PhotoContestSearch();


            search.SortColumn = SortField;
            search.SortAsc = SortAsc;

            if (ddActive.SelectedIndex > 0)
                search.Active = Boolean.Parse(ddActive.SelectedValue);

            Results = search.GetResults();

            //loadResultsPage();
        }

        public string SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return "DateCreated";
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

        protected void dgContests_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField.ToString())
                {
                    SortAsc = !SortAsc;
                }

                SortField = e.SortExpression;
            }

            performSearch();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            if (txtName.Text.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter a contest name"), Misc.MessageType.Error);
                return;
            }
            
            PhotoContest contest;
            if (EditedContest.HasValue)
            {
                contest = PhotoContest.Load(EditedContest.Value);
                if (contest == null)
                {
                    MessageBox.Show(Lang.TransA("The contest does not exist"), Misc.MessageType.Error);
                    return;
                }

                contest.Name = txtName.Text;
                contest.Description = txtDescription.Text;
            }
            else
            {
                contest = new PhotoContest(txtName.Text, txtDescription.Text);
                contest.DateCreated = DateTime.Now;
            }
            
            contest.Terms = txtTerms.Text;
            
            if (ddFromAge.SelectedIndex > 0)
                contest.MinAge = Int32.Parse(ddFromAge.SelectedValue);
            else 
                contest.MinAge = null;
            
            if (ddToAge.SelectedIndex > 0)
                contest.MaxAge = Int32.Parse(ddToAge.SelectedValue);
            else
                contest.MaxAge = null;
            
            if (ddGender.SelectedIndex > 0)
                contest.Gender = (User.eGender) Int32.Parse(ddGender.SelectedValue);
            else 
                contest.Gender = null;

            if (dpDateEnds.ValidDateEntered)
                contest.DateEnds = dpDateEnds.SelectedDate;
            else
                contest.DateEnds = null;
            
            contest.Save();

            pnlSearchInfo.Visible = true;
            pnlAddNewContest.Visible = false;
            btnAddNewContest.Visible = true;
            
            performSearch();
        }

        protected void btnAddNewContest_Click(object sender, EventArgs e)
        {
            ClearContestFields();
            EditedContest = null;
            pnlSearchInfo.Visible = false;
            pnlAddNewContest.Visible = true;
            btnAddNewContest.Visible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlSearchInfo.Visible = true;         
            pnlAddNewContest.Visible = false;
            btnAddNewContest.Visible = true;
        }

        protected void dgContests_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteContest":
                    if (!HasWriteAccess)
                        return;
                    PhotoContest.Delete(Int32.Parse((string)e.CommandArgument));
                    performSearch();
                    break;
                case "EditContest":
                    ClearContestFields();
                    EditedContest = Int32.Parse((string)e.CommandArgument);
                    PhotoContest contest =
                        PhotoContest.Load(Int32.Parse((string) e.CommandArgument));

                    txtName.Text = contest.Name;
                    txtDescription.Text = contest.Description;
                    txtTerms.Text = contest.Terms;
                    if (contest.Gender.HasValue)
                        ddGender.SelectedValue = ((int)contest.Gender).ToString();
                    if (contest.MinAge.HasValue)
                        ddFromAge.SelectedValue = contest.MinAge.ToString();
                    if (contest.MinAge.HasValue)
                        ddToAge.SelectedValue = contest.MaxAge.ToString();
                    if (contest.DateEnds.HasValue)
                        dpDateEnds.SelectedDate = contest.DateEnds.Value;

                    pnlSearchInfo.Visible = false;
                    pnlAddNewContest.Visible = true;
                    btnAddNewContest.Visible = false;
                    
                    break;
                case "ViewEntries":
                    Response.Redirect(String.Format("ContestEntries.aspx?cid={0}", e.CommandArgument));
                    break;
            }
        }

        protected void dgContests_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item  && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            ((LinkButton) e.Item.FindControl("lnkViewEntries")).Text = Lang.TransA("View Entries");
            ((LinkButton)e.Item.FindControl("lnkEditContest")).Text = Lang.TransA("Edit");
            LinkButton lnkDeleteContest = (LinkButton)e.Item.FindControl("lnkDeleteContest");
            
            lnkDeleteContest.Text = Lang.TransA("Delete");
            if (!HasWriteAccess)
                lnkDeleteContest.Enabled = false;
            else
                lnkDeleteContest.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                    Lang.TransA("Are you sure you want to delete this contest?")));
        }
        
        private void ClearContestFields()
        {
            txtName.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtTerms.Text = String.Empty;
            ddFromAge.SelectedIndex = 0;
            ddToAge.SelectedIndex = 0;
            ddGender.SelectedIndex = 0;
                
            dpDateEnds.Reset();
        }
    }
}
