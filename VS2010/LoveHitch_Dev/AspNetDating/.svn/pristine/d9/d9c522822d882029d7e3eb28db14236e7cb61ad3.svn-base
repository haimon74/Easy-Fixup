using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Admin;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class BrowseAffiliates : AdminPageBase
    {
        private int AffiliatesPerPage
        {
            get { return Convert.ToInt32(ddAffiliatesPerPage.SelectedValue); }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseAffiliates;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Affiliates.Enable)
                {
                    StatusPageMessage =
                        Lang.TransA("Affiliates option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                loadStrings();
                preparePaginator();
                populateFilters();
            }
        }

        private void loadStrings()
        {
            gvAffiliates.Columns[0].HeaderText = Lang.TransA("Username");
            gvAffiliates.Columns[1].HeaderText = Lang.TransA("Name");
            gvAffiliates.Columns[2].HeaderText = Lang.TransA("Site URL");
            gvAffiliates.Columns[3].HeaderText = Lang.TransA("Balance");
            gvAffiliates.Columns[4].HeaderText = Lang.TransA("Request Payment");
            gvAffiliates.Columns[5].HeaderText = Lang.TransA("Commission");

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");

            btnSearch.Text = Lang.TransA("Search");
            btnGetCSV.Text = Lang.TransA("Download as CSV");
        }

        public AffiliateSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (AffiliateSearchResults)
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

        private void loadResultsPage()
        {
            preparePaginator();

            DataTable dtResults;

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);
                Affiliate[] affiliates = Results.GetPage(CurrentPage, AffiliatesPerPage);
                dtResults = FetchResultsDataTable(affiliates);
                tblHideSearch.Visible = true;
            }
            else
            {
                dtResults = new DataTable();
                tblHideSearch.Visible = false;
            }

            Trace.Write("Binding...");
            gvAffiliates.DataSource = dtResults;
            gvAffiliates.DataBind();
        }

        private void populateFilters()
        {
            for (int i = 5; i <= 50; i += 5)
                ddAffiliatesPerPage.Items.Add(i.ToString());
            ddAffiliatesPerPage.SelectedValue = Config.AdminSettings.BrowseAffiliates.AffiliatesPerPage.ToString();

            ddActive.Items.Add(new ListItem("", "-1"));
            ddActive.Items.Add(new ListItem(Lang.TransA("Yes")));
            ddActive.Items.Add(new ListItem(Lang.TransA("No")));

            ddDeleted.Items.Add(new ListItem("", "-1"));
            ddDeleted.Items.Add(new ListItem(Lang.TransA("Yes")));
            ddDeleted.Items.Add(new ListItem(Lang.TransA("No")));

            ddRequestPayment.Items.Add(new ListItem("", "-1"));
            ddRequestPayment.Items.Add(new ListItem(Lang.TransA("Yes")));
            ddRequestPayment.Items.Add(new ListItem(Lang.TransA("No")));
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(AffiliatesPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Affiliates.Length > 0)
            {
                int fromAffiliate = (CurrentPage - 1) * AffiliatesPerPage + 1;
                int toAffiliate = CurrentPage * AffiliatesPerPage;
                if (Results.Affiliates.Length < toAffiliate)
                    toAffiliate = Results.Affiliates.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromAffiliate, toAffiliate, Results.Affiliates.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        private void performSearch()
        {
            BasicSearchAffiliate search = new BasicSearchAffiliate();

            search.SortColumn = SortField;
            search.SortAsc = SortAsc;

            if (txtUsername.Text.Trim().Length > 0)
                search.Username = txtUsername.Text.Trim();

            if (txtName.Text.Trim().Length > 0)
                search.Name = txtName.Text.Trim();

            if (txtEmail.Text.Trim().Length > 0)
                search.Email = txtEmail.Text.Trim();

            if (txtSiteURL.Text.Trim().Length > 0)
                search.SiteURL= txtSiteURL.Text.Trim();

            if (ddDeleted.SelectedIndex > 0)
                search.Deleted= ddDeleted.SelectedIndex == 1;

            if (ddActive.SelectedIndex > 0)
                search.Active = ddActive.SelectedIndex == 1;

            if (ddRequestPayment.SelectedIndex > 0)
                search.RequestPayment = ddRequestPayment.SelectedIndex == 1;

            Results = search.GetResults();

            btnGetCSV.Visible = Results != null;

            loadResultsPage();
        }

        private static DataTable FetchResultsDataTable(Affiliate[] affiliates)
        {
            DataTable dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("AffiliateID");
            dtResults.Columns.Add("Username");
            dtResults.Columns.Add("Name");
            dtResults.Columns.Add("SiteURL");
            dtResults.Columns.Add("Balance");
            dtResults.Columns.Add("RequestPayment");
            dtResults.Columns.Add("Commission");

            if (affiliates != null && affiliates.Length > 0)
                foreach (Affiliate affiliate in affiliates)
                {
                    string commission = "";

                    if (affiliate.FixedAmount != null)
                    {
                        commission = affiliate.FixedAmount.Value.ToString("C");
                    }

                    if (affiliate.Percentage != null)
                    {
                        commission += " " + affiliate.Percentage + "%";
                    }

                    dtResults.Rows.Add(new object[]
                                               {
                                                   affiliate.ID,
                                                   affiliate.Username,
                                                   Parsers.ProcessAffiliateName(affiliate.Name),
                                                   affiliate.SiteURL,
                                                   affiliate.Balance.ToString("C"),
                                                   affiliate.RequestPayment,
                                                   commission
                                               });
                }

            return dtResults;
        }

        public Affiliate.eSortColumn SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return Affiliate.eSortColumn.Username;
                }
                else
                {
                    return (Affiliate.eSortColumn)ViewState["sortField"];
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            performSearch();
        }

        protected void gvAffiliates_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteAffiliate":
                    if (!HasWriteAccess) return;

                    Affiliate affiliate = Affiliate.Fetch(Convert.ToInt32(e.CommandArgument));

                    if (affiliate != null)
                    {
                        affiliate.Deleted = true;
                        affiliate.Save();

                        performSearch();
                    }

                    break;
            }
        }

        protected void gvAffiliates_RowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lnkDeleteGroup = e.Row.FindControl("lnkDeleteGroup") as LinkButton;

                if (lnkDeleteGroup != null)
                {
                    if (!HasWriteAccess)
                        lnkDeleteGroup.Enabled = false;
                    else
                        lnkDeleteGroup.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                        Lang.TransA("Are you sure you want to delete this affiliate?")));
                }    
            }
        }

        protected void gvAffiliates_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField.ToString())
                {
                    SortAsc = !SortAsc;
                }

                SortField = (Affiliate.eSortColumn)Enum.Parse(typeof(Affiliate.eSortColumn), e.SortExpression);
            }

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
            if (CurrentPage < Results.GetTotalPages(AffiliatesPerPage))
                CurrentPage++;

            loadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(AffiliatesPerPage))
                CurrentPage = Results.GetTotalPages(AffiliatesPerPage);

            loadResultsPage();
        }

        protected void ddAffiliatesPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
        }

        protected void btnGetCSV_Click(object sender, EventArgs e)
        {
            if (Results != null)
            {
                Response.Clear();
                Response.ContentType = "application/text";
                Response.ContentEncoding = Encoding.UTF8;
                Response.Charset = Encoding.UTF8.EncodingName;
                Response.AppendHeader("content-disposition", "attachment; filename=results.csv");
                Response.Write(Parsers.ParseCSV(FetchResultsDataTable(Results.Get())));
                Response.End();
            }
        }
    }
}
