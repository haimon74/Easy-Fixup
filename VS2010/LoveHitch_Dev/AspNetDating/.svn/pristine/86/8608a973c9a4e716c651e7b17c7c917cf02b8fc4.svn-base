using System;
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class Groups : PageBase
    {

        public Groups()
        {
            RequiresAuthorization = false;
        }

        protected bool ShowPnlPendingActions
        {
            get
            {
                if (ViewState["ShowPnlPendingActions"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool) ViewState["ShowPnlPendingActions"];
                }
            }
            set { ViewState["ShowPnlPendingActions"] = value; }
        }

        protected int? ViewedCategoryID
        {
            get { return (int?) ViewState["ViewedCategoryID"]; }
            set { ViewState["ViewedCategoryID"] = value; }
        }

        private eGroupType GroupType
        {
            get
            {
                if (ViewState["GroupType"] == null)
                {
                    return eGroupType.None;
                }

                return (eGroupType) ViewState["GroupType"];
            }
            set
            {
                ViewState["GroupType"] = value;
            }
        }
        private string username;
        private bool FetchGroupMember
        {
            get
            {
                if (ViewState["FetchGroupMember"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState["FetchGroupMember"];
                }
            }
            set { ViewState["FetchGroupMember"] = value; }
        }
        private enum eGroupType
        {
            None,
            ByCategory,
            MyGroups,
            NewGroups
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Groups.EnableGroups)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (CurrentUserSession == null && Config.Groups.OnlyRegisteredUsersCanBrowseGroups)
                    Response.Redirect("~/LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));


                if (CurrentUserSession != null)
                {
                    var permissionCheckResult = CurrentUserSession.CanBrowseGroups();

                    if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                    {
                        Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanBrowseGroups;
                        Response.Redirect("~/ManageProfile.aspx?sel=payment");
                        return;
                    }

                    if (permissionCheckResult == PermissionCheckResult.No)
                    {
                        StatusPageMessage = Lang.Trans("You are not allowed to browse groups!");
                        Response.Redirect("ShowStatus.aspx");
                        return;
                    }
                }

                loadStrings();
                loadCategories();


                if (Request.Params["show"] != null && Request.Params["show"] == "mg" && CurrentUserSession != null)
                {
                    lnkMyGroups_Click(null, null);
                }
                else if (Request.Params["show"] != null && Request.Params["show"] == "ng" && CurrentUserSession != null)
                {
                    lnkNewGroups_Click(null, null);
                }
                else if (Request.Params["show"] != null && Request.Params["show"] == "pi" && CurrentUserSession != null)
                {
                    lnkPendingInvitations_Click(null, null);
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (GroupType != eGroupType.None)
                bindGroups();

//            pnlSearchParameters.Visible = ViewedCategoryID != null && Results.Groups.Length != 0;
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            lnkBrowseGroups.Text = Lang.Trans("Browse Groups");
            lnkMyGroups.Text = Lang.Trans("My Groups");
            lnkNewGroups.Text = Lang.Trans("New Groups");
            lnkPendingInvitations.Text = Lang.Trans("Pending Invitations");
            lnkCreateGroup.Text = Lang.Trans("Create a Group");
            LargeBoxStart1.Title = Lang.Trans("Group Categories");
            btnSearchGroup.Text = Lang.Trans("Search");
            btnSearchGroupByCategory.Text = Lang.Trans("Search");
            pnlMyGroups.Visible = CurrentUserSession != null;
           
            pnlPendingInvitations.Visible = CurrentUserSession != null;
            mvGroups.SetActiveView(viewCategories);
            lnkBrowseGroups.Enabled = false;

            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");
        }

        private void loadCategories()
        {
            DataTable dtCategories = new DataTable("Categories");

            dtCategories.Columns.Add("CategoryID");
            dtCategories.Columns.Add("Name");
            dtCategories.Columns.Add("Order");
            dtCategories.Columns.Add("GroupsNumber");

            Category[] categories = Category.Fetch();

            if (categories.Length > 0)
            {
                foreach (Category category in categories)
                {
                    int groups = Group.FetchGroupsCount(category.ID, true);

                    dtCategories.Rows.Add(new object[]
                                          {
                                              category.ID,
                                              Server.HtmlEncode(category.Name),
                                              category.Order,
                                              groups
                                          });
                }

                dlCategories.DataSource = dtCategories;
                dlCategories.DataBind();    
            }
            else
            {
                lblError.Text = Lang.Trans("There are no categories.");
                pnlFilterGroup.Visible = false;
            }
            
            bool userCanCreateGroups = Array.Exists(categories, delegate(Category match) { return match.UsersCanCreateGroups; });
            pnlCreateGroup.Visible = CurrentUserSession != null && (userCanCreateGroups || CurrentUserSession.IsAdmin());
        }

        private void bindGroups()
        {
            preparePaginator();

            DataTable dtGroups = new DataTable("Groups");

            dtGroups.Columns.Add("GroupID");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("Description");
            dtGroups.Columns.Add("IsVisibleLinkMore", typeof(bool));
            dtGroups.Columns.Add("DateCreated");
            dtGroups.Columns.Add("Approved");
            dtGroups.Columns.Add("AccessLevel");
            dtGroups.Columns.Add("Owner");
            dtGroups.Columns.Add("MemberType");
            dtGroups.Columns.Add("Pending");
            dtGroups.Columns.Add("MembersCount");

            Group[] groups = null;

            if (Results == null)
            {
                Results = new GroupSearchResults();

                if (GroupType == eGroupType.ByCategory)
                {
                    Results.Groups =
                            Group.Search(ViewedCategoryID, null, null, null, true, null, null, null, null, false, null,
                                         Group.eSortColumn.ActiveMembers);
                }
                else if (GroupType == eGroupType.MyGroups)
                {
                    Results.Groups = Group.SearchGroupsByUsername(username, Group.eSortColumn.ActiveMembers);
                }
                else if (GroupType == eGroupType.NewGroups)
                {
                    Results.Groups =
                        Group.Search(null, null, null, null, true, null, null, null, null, false,
                                     Config.Groups.NumberOfNewGroups, Group.eSortColumn.DateCreated);
                }

                if (Results.Groups.Length == 0)
                {
                    if (ShowPnlPendingActions)
                    {
                        lblError.Text = Lang.Trans("There are no pending invitations.");
                    }
                    else if (GroupType == eGroupType.MyGroups)
                    {
                        lblError.Text = Lang.Trans("You are not a member of any group.");
                    }
                    else if (GroupType == eGroupType.NewGroups)
                    {
                        lblError.Text = Lang.Trans("There are no new groups.");
                    }
                    else if (GroupType == eGroupType.ByCategory)
                    {
                        lblError.Text = Lang.Trans("There are no groups.");
                    }

                    PaginatorEnabled = false;
                    dlGroups.Visible = false;
                    pnlSearchParameters.Visible = false;
                    return;
                }
                else
                {
                    PaginatorEnabled = !ShowPnlPendingActions ? true : false;
                }

                CurrentPage = 1;
            }

            groups = !ShowPnlPendingActions ? Results.GetPage(CurrentPage, Config.Groups.GroupsPerPage) : Results.Get();

            if (groups != null && groups.Length > 0)
            {
                foreach (Group group in groups)
                {
                    if (group == null)
                        continue;

                    bool isVisibleLinkMore = false;
                    string MemberType = String.Empty;
                    string pending = String.Empty;
                    GroupMember groupMember = null;

                    if (FetchGroupMember)
                    {
                        groupMember = GroupMember.Fetch(group.ID, CurrentUserSession.Username);
                        if (groupMember != null)
                        {
                            MemberType = groupMember.Active
                                             ?
                                         "(" + Lang.Trans(groupMember.Type.ToString()) + ")"
                                             :
                                         "(" + Lang.Trans("Pending") + ")";
                        }

                        if (group.Owner == CurrentUserSession.Username)
                        {
                            MemberType = "(" + Lang.Trans("Owner") + ")";
                        }

                        if (!group.Approved)
                        {
                            pending = "(" + Lang.Trans("-- pending approval --") + ")"; 
                        }
                    }

                    if (ShowPnlPendingActions)
                    {
                        // if group member is not invited or is alreay invited
                        if (groupMember != null && (groupMember.InvitedBy == null || groupMember.Active))
                        {
                            continue;
                        }
                    }

                    string more = group.Description.Length > 200 ? "..." : String.Empty;
                    string accessLevel = String.Empty;
                    switch(group.AccessLevel){
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
                                                true) + more,
                                              isVisibleLinkMore = group.Description.Length > 200,
                                              group.DateCreated.Add(Config.Misc.TimeOffset).ToShortDateString(),
                                              group.Approved,
                                              accessLevel,
                                              group.Owner,
                                              MemberType,
                                              pending,
                                              group.ActiveMembers
                                          });
                }

                if (ShowPnlPendingActions && dtGroups.Rows.Count == 0)
                {
                    lblError.Text = Lang.Trans("There are no pending invitations.");
                }
            }

            dlGroups.Visible = dtGroups.Rows.Count > 0;
            dlGroups.DataSource = dtGroups;
            dlGroups.DataBind();
        }

        private void enableMenuLinks()
        {
            lnkBrowseGroups.Enabled = true;
            lnkMyGroups.Enabled = true;
            lnkNewGroups.Enabled = true;
            lnkPendingInvitations.Enabled = true;
            lnkCreateGroup.Enabled = true;
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
                //bindGroups();
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

        protected void lnkBrowseGroups_Click(object sender, EventArgs e)
        {
            ShowPnlPendingActions = false;
            GroupType = eGroupType.None;

            mvGroups.SetActiveView(viewCategories);

            enableMenuLinks();
            lnkBrowseGroups.Enabled = false;

            LargeBoxStart1.Title = Lang.Trans("Group Categories");

            txtGroupToSearch.Text = "";
            txtSearchGroupByCategory.Text = "";
            cbSearchInDescription.Checked = false;
            cbSearchInDescriptionByCategory.Checked = false;
        }

        protected void lnkMyGroups_Click(object sender, EventArgs e)
        {
            Results = null;

            ShowPnlPendingActions = false;

            GroupType = eGroupType.MyGroups;
            ViewedCategoryID = null;
            username = CurrentUserSession.Username;
            FetchGroupMember = true;
            pnlSearchParameters.Visible = false;
            mvGroups.SetActiveView(viewGroups);

            enableMenuLinks();
            lnkMyGroups.Enabled = false;

            LargeBoxStart1.Title = Lang.Trans("My Groups");
        }

        protected void lnkNewGroups_Click(object sender, EventArgs e)
        {
            Results = null;

            ShowPnlPendingActions = false;

            GroupType = eGroupType.NewGroups;
            ViewedCategoryID = null;
            username = null;
            FetchGroupMember = false;
            pnlSearchParameters.Visible = false;

            mvGroups.SetActiveView(viewGroups);

            enableMenuLinks();
            lnkNewGroups.Enabled = false;

            LargeBoxStart1.Title = Lang.Trans("New Groups");
        }

        protected void lnkCreateGroup_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/CreateGroup.aspx?cid=" + ViewedCategoryID);
        }

        protected void dlCategories_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "ViewGroups")
            {
                LargeBoxStart1.Title = Lang.Trans("Groups");

                ViewedCategoryID = Convert.ToInt32(e.CommandArgument);
                Results = null;

                GroupType = eGroupType.ByCategory;
                username = null;
                FetchGroupMember = false;
                pnlSearchParameters.Visible = true;

                mvGroups.SetActiveView(viewGroups);
                lnkBrowseGroups.Enabled = true;
            }
        }

        protected void lnkPendingInvitations_Click(object sender, EventArgs e)
        {
            ShowPnlPendingActions = true;

            Results = null;

            GroupType = eGroupType.MyGroups;
            username = CurrentUserSession.Username;
            FetchGroupMember = true;
            pnlSearchParameters.Visible = false;

            mvGroups.SetActiveView(viewGroups);
            LargeBoxStart1.Title = Lang.Trans("Pending Invitations");
            enableMenuLinks();
            lnkPendingInvitations.Enabled = false;
        }

        protected void dlGroups_ItemCreated(object sender, DataListItemEventArgs e)
        {
            LinkButton lnkAccept = (LinkButton)e.Item.FindControl("lnkAccept");
            LinkButton lnkReject = (LinkButton)e.Item.FindControl("lnkReject");

            if (lnkAccept != null)
            {
                lnkAccept.Text = Lang.Trans("Accept");    
            }

            if (lnkReject != null)
            {
                lnkReject.Text = Lang.Trans("Reject");    
            }
        }

        protected void dlGroups_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int groupID = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Accept" :
                    GroupMember groupMember =
                        GroupMember.Fetch(groupID, CurrentUserSession.Username);

                    if (groupMember != null)
                    {
                        groupMember.Active = true;
                        groupMember.Save();

                        Group group = Group.Fetch(groupID);

                        if (group != null)
                        {
                            group.ActiveMembers++;
                            group.Save();    
                        }

                        #region Add Event

                        Event newEvent = new Event(CurrentUserSession.Username);

                        newEvent.Type = Event.eType.FriendJoinedGroup;
                        FriendJoinedGroup friendJoinedGroup = new FriendJoinedGroup();
                        friendJoinedGroup.GroupID = groupID;
                        newEvent.DetailsXML = Misc.ToXml(friendJoinedGroup);

                        newEvent.Save();

                        string[] usernames = Classes.User.FetchMutuallyFriends(CurrentUserSession.Username);

                        foreach (string friendUsername in usernames)
                        {
                            if (Config.Users.NewEventNotification)
                            {
                                if (group != null)
                                {
                                    string text =
                                        String.Format("Your friend {0} has joined the {1} group".Translate(),
                                                      "<b>" + CurrentUserSession.Username + "</b>",
                                                      Server.HtmlEncode(group.Name));
                                    int imageID = 0;
                                    try
                                    {
                                        imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                                    }
                                    catch (NotFoundException)
                                    {
                                        imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                                    }
                                    string thumbnailUrl =
                                        ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                    Classes.User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername,
                                                                             text,
                                                                             thumbnailUrl,
                                                                             UrlRewrite.CreateShowGroupUrl(
                                                                                 group.ID.ToString()));
                                }
                            }
                        }

                        #endregion
                    }
                    break;

                case "Reject" :
                    GroupMember.Delete(Convert.ToInt32(e.CommandArgument), CurrentUserSession.Username);
                    break;
            }

            Response.Redirect("~/Groups.aspx?show=mg");
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupsPerPage))
                CurrentPage++;
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupsPerPage))
                CurrentPage = Results.GetTotalPages(Config.Groups.GroupsPerPage);
        }

        protected void btnSearchGroup_Click(object sender, EventArgs e)
        {
            LargeBoxStart1.Title = Lang.Trans("Groups");

            int[] groupIDs;

            groupIDs =
                Group.Search(null, null, null, null, true, null, null, null, txtGroupToSearch.Text.Trim(), cbSearchInDescription.Checked,
                             null, Group.eSortColumn.Name);
            
            GroupSearchResults results = new GroupSearchResults();
            results.Groups = groupIDs;

            SearchResults1.Results = results;

            enableMenuLinks();

            mvGroups.SetActiveView(viewGroupSearchResults);
        }

        protected void btnSearchGroupByCategory_Click(object sender, EventArgs e)
        {
            LargeBoxStart1.Title = Lang.Trans("Groups");

            int[] groupIDs;

            groupIDs =
                Group.Search(ViewedCategoryID, null, null, null, true, null, null, null,
                             txtSearchGroupByCategory.Text.Trim(), cbSearchInDescriptionByCategory.Checked, null, Group.eSortColumn.Name);

            GroupSearchResults results = new GroupSearchResults();
            results.Groups = groupIDs;

            SearchResults1.Results = results;

            enableMenuLinks();

            mvGroups.SetActiveView(viewGroupSearchResults);
        }
    }
}
