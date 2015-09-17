using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class BrowseGroups : AdminPageBase
    {
        private int GroupsPerPage
        {
            get { return Convert.ToInt32(ddGroupsPerPage.SelectedValue); }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseGroups;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Groups.EnableGroups)
                {
                    StatusPageMessage = Lang.TransA("Groups option is not currently switched on!\n You can do this from Settings at Site Management section.");
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
            btnSearch.Text = Lang.TransA("Search");
            btnGetCSV.Text = Lang.TransA("Download as CSV");

            dgGroups.Columns[1].HeaderText = Lang.TransA("Name");
            dgGroups.Columns[2].HeaderText = Lang.TransA("Owner");
            dgGroups.Columns[3].HeaderText = Lang.TransA("Category");
            dgGroups.Columns[4].HeaderText = Lang.TransA("Created on");
            dgGroups.Columns[5].HeaderText = Lang.TransA("Access level");
            dgGroups.Columns[6].HeaderText = Lang.TransA("Edit");

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");
        }

        public GroupSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (GroupSearchResults)
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

                Group[] groups = Results.GetPage(CurrentPage, GroupsPerPage);
                dtResults = FetchResultsDataTable(groups);
                tblHideSearch.Visible = true;
            }
            else
            {
                dtResults = new DataTable();
                tblHideSearch.Visible = false;
            }

            Trace.Write("Binding...");
            dgGroups.DataSource = dtResults;
            dgGroups.DataBind();
        }

        private void populateFilters()
        {
            for (int i = 5; i <= 50; i += 5)
                ddGroupsPerPage.Items.Add(i.ToString());
            ddGroupsPerPage.SelectedValue = Config.AdminSettings.BrowseGroups.GroupsPerPage.ToString();

            Category[] categories = Category.Fetch();

            ddCategory.Items.Add(new ListItem("", "-1"));

            if (categories.Length > 0)
            {
                foreach (Category category in categories)
                {
                    ddCategory.Items.Add(new ListItem(category.Name, category.ID.ToString()));
                }
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no categoies"), Misc.MessageType.Error);
            }

            ddApproved.Items.Add(new ListItem("", "-1"));
            ddApproved.Items.Add( new ListItem(Lang.TransA("Yes")));
            ddApproved.Items.Add( new ListItem(Lang.TransA("No")));

            ddAccessLevel.Items.Add(new ListItem("", "-1"));
            ddAccessLevel.Items.Add(new ListItem(Group.eAccessLevel.Public.ToString(), ((int)Group.eAccessLevel.Public).ToString()));
            ddAccessLevel.Items.Add(new ListItem(Group.eAccessLevel.Moderated.ToString(), ((int)Group.eAccessLevel.Moderated).ToString()));
            ddAccessLevel.Items.Add(new ListItem(Group.eAccessLevel.Private.ToString(), ((int)Group.eAccessLevel.Private).ToString()));

            if (Request.Params["gid"] != null && Request.Params["gid"] != "")
            {
                Category category = Category.Fetch(Convert.ToInt32(Request.Params["gid"]));
                ddCategory.SelectedValue = category.ID.ToString();
                btnSearch_Click(null, null);
            }
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(GroupsPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Groups.Length > 0)
            {
                int fromUser = (CurrentPage - 1) * GroupsPerPage + 1;
                int toUser = CurrentPage * GroupsPerPage;
                if (Results.Groups.Length < toUser)
                    toUser = Results.Groups.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Groups.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            performSearch();
        }

        private void performSearch()
        {
            BasicSearchGroup search = new BasicSearchGroup();

            search.SortColumn = SortField;
            search.SortAsc = SortAsc;

            if (ddCategory.SelectedIndex > 0)
                search.CategoryID = Convert.ToInt32(ddCategory.SelectedValue);

            if (txtOwner.Text.Trim().Length > 0)
                search.Owner = txtOwner.Text.Trim();

            if (txtName.Text.Trim().Length > 0)
                search.Name = txtName.Text.Trim();
            
            if (ddApproved.SelectedIndex > 0)
                search.Approved = ddApproved.SelectedIndex == 1;

            if (ddAccessLevel.SelectedIndex > 0)
                search.AccessLevel = (Group.eAccessLevel) Convert.ToInt32(ddAccessLevel.SelectedValue);

            Results = search.GetResults();

            btnGetCSV.Visible = Results != null;

            loadResultsPage();
        }

        private static DataTable FetchResultsDataTable(Group[] groups)
        {
            DataTable dtResults = new DataTable("SearchResults");

            dtResults.Columns.Add("GroupID");
            dtResults.Columns.Add("Category");
            dtResults.Columns.Add("Owner");
            dtResults.Columns.Add("Name");
            dtResults.Columns.Add("DateCreated");
            dtResults.Columns.Add("AccessLevel");

            if (groups != null && groups.Length > 0)
                foreach (Group group in groups)
                {
                    Category[] categories = Category.FetchCategoriesByGroup(group.ID);

                    List<string> lCategories = new List<string>();

                    foreach (Category category in categories)
                    {
                        lCategories.Add(category.Name);
                    }

                    string[] groupCategories = lCategories.ToArray();
                    string strCategories = String.Join(", ", groupCategories);

                    dtResults.Rows.Add(new object[]
                                               {
                                                   group.ID,
                                                   strCategories,
                                                   group.Owner,
                                                   Parsers.ProcessGroupName(group.Name),
                                                   group.DateCreated.ToShortDateString(),
                                                   group.AccessLevel
                                               });
                }


            return dtResults;
        }

        public Group.eSortColumn SortField
        {
            get
            {
                if (ViewState["sortField"] == null)
                {
                    return Group.eSortColumn.DateCreated;
                }
                else
                {
                    return (Group.eSortColumn) ViewState["sortField"];
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

        protected void dgGroups_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            if (e.SortExpression.Length != 0)
            {
                if (e.SortExpression == SortField.ToString())
                {
                    SortAsc = !SortAsc;
                }

                SortField = (Group.eSortColumn) Enum.Parse(typeof(Group.eSortColumn), e.SortExpression);
            }

            performSearch();
        }

        protected void dgGroups_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteGroup" :
                    if (!HasWriteAccess) return;
                    
                    Group group = Group.Fetch(Convert.ToInt32(e.CommandArgument));

                    if (group != null)
                    {
                        if (group.ActiveMembers <= Config.Groups.MaxGroupMembersToDeleteGroup)
                        {
                            Group.Delete(group.ID);

                            performSearch();
                        }
                        else
                        {
                            MessageBox.Show(Lang.TransA(
                                    String.Format("You cannot delete this group because it has more than {0} members.",
                                                  Config.Groups.MaxGroupMembersToDeleteGroup)), Misc.MessageType.Error);
                        }
                    }
                    
                    break;
            }
        }

        protected void dgGroups_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDeleteGroup = e.Item.FindControl("lnkDeleteGroup") as LinkButton;

            if (lnkDeleteGroup != null)
            {
                if (!HasWriteAccess)
                    lnkDeleteGroup.Enabled = false;
                else
                    lnkDeleteGroup.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                    Lang.TransA("Are you sure you want to delete this group?")));
            }
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
            if (CurrentPage < Results.GetTotalPages(GroupsPerPage))
                CurrentPage++;

            loadResultsPage();
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(GroupsPerPage))
                CurrentPage = Results.GetTotalPages(GroupsPerPage);

            loadResultsPage();
        }

        protected void ddGroupsPerPage_SelectedIndexChanged(object sender, EventArgs e)
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
