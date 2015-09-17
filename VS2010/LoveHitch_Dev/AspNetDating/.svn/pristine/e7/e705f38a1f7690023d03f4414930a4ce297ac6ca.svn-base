using System;
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class BrowseContactUs : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();

                dpFromDate.MinYear = 2011;
                dpToDate.MinYear = 2011;
                dpFromDate.MaxYear = DateTime.Now.Year;
                dpToDate.MaxYear = DateTime.Now.Year;
            }
        }

        private string SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return "LastOnline";
                }
                else
                {
                    return (string)ViewState["sortField"];
                }
            }
            set { ViewState["sortField"] = value; }
        }

        private bool SortAsc
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

        private void LoadStrings()
        {
            btnSearch.Text = Lang.TransA("Search");

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");

            ddReportType.Items.Add(new ListItem(Lang.TransA(AbuseReport.ReportType.Profile.ToString()), ((int)AbuseReport.ReportType.Profile).ToString()));
            ddReportType.Items.Add(new ListItem(Lang.TransA(AbuseReport.ReportType.Photo.ToString()), ((int)AbuseReport.ReportType.Photo).ToString()));
            ddReportType.Items.Add(new ListItem(Lang.TransA(AbuseReport.ReportType.Message.ToString()), ((int)AbuseReport.ReportType.Message).ToString()));

            ddReviewed.Items.Add(Boolean.TrueString);
            ddReviewed.Items.Add(Boolean.FalseString);

            ddReviewed.SelectedValue = Boolean.FalseString;

            dgAbuseReports.Columns[0].HeaderText = Lang.TransA("Date Reported");
            dgAbuseReports.Columns[1].HeaderText = Lang.TransA("Reported By");
            dgAbuseReports.Columns[2].HeaderText = Lang.TransA("Reported User");
            dgAbuseReports.Columns[3].HeaderText = Lang.TransA("Report");
            dgAbuseReports.Columns[4].HeaderText = Lang.TransA("Type");
            dgAbuseReports.Columns[5].HeaderText = Lang.TransA("Actions");
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.abuseReports;               
            base.OnInit(e);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Results = null;
            LoadContactUsMessages();
            LoadResultsPage();    
        }

        public ContactUsSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (ContactUsSearchResults)
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
            set {
                 pnlPaginator.Visible = value;
            }
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(Config.AbuseReports.ReportsPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.ContactUsIds.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * Config.AbuseReports.ReportsPerPage + 1;
                int toUser = CurrentPage * Config.AbuseReports.ReportsPerPage;
                if (Results.ContactUsIds.Length < toUser)
                    toUser = Results.ContactUsIds.Length;

                lblPager.Text = String.Format(
                     Lang.TransA("Showing {0}-{1} from {2} total"),
                     fromUser, toUser, Results.ContactUsIds.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void LoadResultsPage()
        {
            PreparePaginator();

            DataTable dtContactUs = new DataTable("ContactUs");
            
            dtContactUs.Columns.Add("ReportId");
            dtContactUs.Columns.Add("ReportedBy");
            dtContactUs.Columns.Add("ReportedUser");
            dtContactUs.Columns.Add("Report");
            dtContactUs.Columns.Add("DateReported");
            dtContactUs.Columns.Add("ReportType", typeof(Classes.ContactUs.ContactType));
            dtContactUs.Columns.Add("ReportedItemId");
            //dtContactUs.Columns.Add("ReportedMessage");
            dtContactUs.Columns.Add("RowIndex");

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);

                Classes.ContactUs[] contactUsArr = Results.GetPage(CurrentPage, Config.AbuseReports.ReportsPerPage);

                if (contactUsArr != null && contactUsArr.Length > 0)
                {
                    int rowIndex = 0;
                    foreach (Classes.ContactUs contactUs in contactUsArr)
                    {
                        dtContactUs.Rows.Add(new object[]
                                                    {
                                                        contactUs.ID,
                                                        contactUs.ReportedBy,
                                                        contactUs.ReportedUser,
                                                        contactUs.Report,
                                                        contactUs.DateReported,
                                                        contactUs.Type,
                                                        contactUs.TargetID,
                                                        rowIndex++
                                  }
                            );
                    }
                }
            }

            Trace.Write("Binding...");

            dgAbuseReports.DataSource = dtContactUs;
            dgAbuseReports.DataBind();
        }

        private void LoadContactUsMessages()
        {
            if (Results == null)
            {
                string reportedBy = null, reportedUser = null;
                Classes.ContactUs.ContactType? reportType = null;
                DateTime? fromDate = null, toDate = null;
                bool? reviewed = null;

                if (txtReportedBy.Text.Length > 0)
                    reportedBy = txtReportedBy.Text;
                if (txtReportedUser.Text.Length > 0)
                    reportedUser = txtReportedUser.Text;
                if (ddReportType.SelectedValue.Length > 0)
                    reportType = (Classes.ContactUs.ContactType)Int32.Parse(ddReportType.SelectedValue);
                if (dpFromDate.ValidDateEntered)
                    fromDate = dpFromDate.SelectedDate;
                if (dpToDate.ValidDateEntered)
                    toDate = dpToDate.SelectedDate;
                if (ddReviewed.SelectedValue.Length > 0)
                    reviewed = Boolean.Parse(ddReviewed.SelectedValue);

                Results = new ContactUsSearchResults();

                Results.ContactUsIds = 
                Classes.ContactUs.Search(reportedBy, reportedUser, reportType, null, reviewed, fromDate, toDate, SortField, SortAsc);

                if (Results.ContactUsIds.Length == 0)
                {
                    PaginatorEnabled = false;
                    dgAbuseReports.Visible = false;

                    MessageBox.Show(Lang.TransA("There are no contact us messages found."), Misc.MessageType.Error);
                    return;
                }
                else
                {
                    PaginatorEnabled = true;
                    dgAbuseReports.Visible = true;
                }

                CurrentPage = 1;
            }
        }

        protected void dgAbuseReports_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField)
                {
                    SortAsc = !SortAsc;
                }

                SortField = e.SortExpression;
            }

            Results = null;
            LoadContactUsMessages();
            LoadResultsPage();
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
            if (CurrentPage < Results.GetTotalPages(Config.AbuseReports.ReportsPerPage))
                CurrentPage++;

            LoadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.AbuseReports.ReportsPerPage))
                CurrentPage = Results.GetTotalPages(Config.AbuseReports.ReportsPerPage);

            LoadResultsPage();
        }

        protected void dgAbuseReports_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            if (e.CommandName == "Sort")
                return;
            
            HiddenField hidReportId = (HiddenField)e.Item.FindControl("hidReportId");
            int reportId = Convert.ToInt32(hidReportId.Value);
            switch(e.CommandName)
            {
                case "MarkReviewed":
                    Classes.ContactUs report = Classes.ContactUs.Fetch(reportId);
                    if (report != null)
                    {
                        report.Reviewed = true;
                        report.Save();
                    }                    
                    break;
                /*
                case "DeleteMessage":
                    int messagesId = Convert.ToInt32(e.CommandArgument);
                    MarkRelatedReportsAsReviewed(AbuseReport.ReportType.Message,null, messagesId);
                    Message.Delete(messagesId);
                    break;
                case "DeletePhoto":
                    int photoId = Convert.ToInt32(e.CommandArgument);
                    MarkRelatedReportsAsReviewed(AbuseReport.ReportType.Photo,null, photoId);
                    Photo.Delete(photoId);                   
                    break;
                case "DeleteUser":
                    {
                        string username = (string) e.CommandArgument;
                        TextBox txtReason = e.Item.FindControl("txtReason1") as TextBox;
                        MarkRelatedReportsAsReviewed(null, username, null);
                        Classes.User.Delete(username, txtReason.Text); 
                    }
                    break;
                case "DeleteUserAndTheirMessages":
                    {
                        string username = (string)e.CommandArgument;
                        TextBox txtReason = e.Item.FindControl("txtReason2") as TextBox;
                        Message[] messages = Message.FetchOutbox(username);
                        foreach (Message message in messages)
                        {
                            message.ToFolder = Message.eFolder.Deleted;
                        }
                        MarkRelatedReportsAsReviewed(null, username, null);
                        Classes.User.Delete(username, txtReason.Text); 
                    }
                    break;
                 */
            }
            
            btnSearch_Click(null, null);
        }
        /*
        private void MarkRelatedReportsAsReviewed(AbuseReport.ReportType? type, string reportedUser, int? targetId)
        {
            int[] reportIds = AbuseReport.Search(null, reportedUser, type, targetId, false, null, null,
                               null, true);
            foreach (int id in reportIds)
            {
                AbuseReport abuseReport = AbuseReport.Fetch(id);
                abuseReport.Reviewed = true;
                abuseReport.Save();
            }         
        }
        */
        protected void dgAbuseReports_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            Button btnDeleteUser = (Button)e.Item.FindControl("btnDeleteUser");
            Button btnDeleteUserAndTheirMessages = (Button)e.Item.FindControl("btnDeleteUserAndTheirMessages");
            LinkButton lnkMarkReviewed = (LinkButton)e.Item.FindControl("lnkMarkReviewed");
            LinkButton lnkDeleteReportedMessage = (LinkButton)e.Item.FindControl("lnkDeleteReportedMessage");
            LinkButton lnkDeleteReportedPhoto = (LinkButton)e.Item.FindControl("lnkDeleteReportedPhoto");

            if (!HasWriteAccess)
            {
                btnDeleteUser.Enabled = false;
                btnDeleteUserAndTheirMessages.Enabled = false;
                lnkMarkReviewed.Enabled = false;
                lnkDeleteReportedMessage.Enabled = false;
                lnkDeleteReportedPhoto.Enabled = false;
            }
            else
            {
               /* lnkDeleteUser.Attributes.Add("onclick",
                                             String.Format("javascript: return confirm('{0}')",
                                                           Lang.TransA("Do you really want to delete this user?")));

                lnkDeleteUserAndTheirMessages.Attributes.Add("onclick",
                                                             String.Format("javascript: return confirm('{0}')",
                                                                           Lang.TransA("Do you really want to delete this user and his/her messages?")));*/
            }
        }
        
    }
}
