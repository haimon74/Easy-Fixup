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

namespace ezFixUp.Components.Groups
{
    public partial class SearchResults : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            loadResultsPage();

            if (PaginatorEnabled)
            {
                preparePaginator();
            }

            base.OnPreRender(e);
        }

        private void loadStrings()
        {
            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");
        }

        public GroupSearchResults Results
        {
            set
            {
                if (ViewState["SearchResults_guid"] == null)
                {
                    ViewState["SearchResults_guid"] = Guid.NewGuid().ToString();
                }

                if (value != null && value.Groups.Length == 0)
                    value = null;

                Global.GetSessionState()["SearchResults" + ViewState["SearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["SearchResults_guid"] != null)
                {
                    return (GroupSearchResults)
                           Global.GetSessionState()["SearchResults" + ViewState["SearchResults_guid"]];
                }
                else
                {
                    return null;
                }
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
                Trace.Write("SearchResults.ascx.cs", "CurrentPage = " + value);
                ViewState["CurrentPage"] = value;
                preparePaginator();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                {
                    return 1;
                }
                else
                {
                    return (int)ViewState["CurrentPage"];
                }
            }
        }

        /// <summary>
        /// Sets a value indicating whether [paginator enabled].
        /// </summary>
        /// <value><c>true</c> if [paginator enabled]; otherwise, <c>false</c>.</value>
        private bool paginatorVisible = true;

        public bool PaginatorEnabled
        {
            get { return paginatorVisible; }
            set
            {
                paginatorVisible = value;
                pnlPaginator.Visible = value;
            }
        }

        /// <summary>
        /// Prepares the paginator.
        /// </summary>
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(Config.Groups.GroupsPerPage))
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
                int fromGroup = (CurrentPage - 1) * Config.Groups.GroupsPerPage + 1;
                int toGroup = CurrentPage * Config.Groups.GroupsPerPage;
                if (Results.Groups.Length < toGroup)
                    toGroup = Results.Groups.Length;

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromGroup, toGroup, Results.Groups.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        private void loadResultsPage()
        {
            DataTable dtGroups = new DataTable("Groups");

            dtGroups.Columns.Add("GroupID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("Description");
            dtGroups.Columns.Add("DateCreated");
            dtGroups.Columns.Add("Approved");
            dtGroups.Columns.Add("AccessLevel");
            dtGroups.Columns.Add("Owner");
            dtGroups.Columns.Add("MembersCount");

            if (Results != null)
            {
                Group[] groups = null;

                if (PaginatorEnabled)
                {
                    groups = Results.GetPage(CurrentPage, Config.Groups.GroupsPerPage);
                }
                else
                {
                    groups = Results.Get();
                }

                if (groups != null && groups.Length > 0)
                {
                    foreach (Group group in groups)
                    {
                        if (group == null)
                            continue;

                        string accessLevel = String.Empty;
                        switch (group.AccessLevel)
                        {
                            case Group.eAccessLevel.Public:
                                accessLevel = Lang.Trans("Public Group");
                                break;
                            case Group.eAccessLevel.Moderated:
                                accessLevel = Lang.Trans("Moderated Group");
                                break;
                            case Group.eAccessLevel.Private:
                                accessLevel = Lang.Trans("Private Group");
                                break;
                        }

                        dtGroups.Rows.Add(new object[]
                                          {
                                              group.ID,
                                              Parsers.ProcessGroupName(group.Name),
                                              Parsers.ProcessGroupDescription(
                                                Parsers.ShortenString(group.Description, 200, 5),
                                                true) + "...",
                                              group.DateCreated.Add(Config.Misc.TimeOffset).ToShortDateString(),
                                              group.Approved,
                                              accessLevel,
                                              group.Owner,
                                              group.ActiveMembers
                                          });
                    }
                }
            }

            dlGroups.Visible = dtGroups.Rows.Count > 0;
            dlGroups.DataSource = dtGroups;
            dlGroups.DataBind();
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
            if (Results == null)
            {
                Response.Redirect("~/Groups.aspx");
            }

            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupsPerPage))
            {
                CurrentPage++;
            }
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (Results == null)
            {
                Response.Redirect("~/Groups.aspx");
            }

            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupsPerPage))
            {
                CurrentPage = Results.GetTotalPages(Config.Groups.GroupsPerPage);
            }
        }
    }
}