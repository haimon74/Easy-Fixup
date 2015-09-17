using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// The "Approve Groups" page
    /// </summary>
    public partial class ApproveGroups : AdminPageBase
    {
        private int GroupsPerPage
        {
            get { return Convert.ToInt32(ddGroupsPerPage.SelectedValue); }
        }

        private string[] GroupIDs
        {
            get
            {
                return ViewState["GroupIDs"] as string[];
            }

            set
            {
                ViewState["GroupIDs"] = value;
            }
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
                int fromGroup = (CurrentPage - 1) * GroupsPerPage + 1;
                int toGroup = CurrentPage * GroupsPerPage;
                if (Results.Groups.Length < toGroup)
                    toGroup = Results.Groups.Length;

                lblPager.Text = String.Format(
                    Lang.TransA("Showing {0}-{1} from {2} total"),
                    fromGroup, toGroup, Results.Groups.Length);
            }
            else
            {
                lblPager.Text = Lang.TransA("No Results");
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.groupApproval;

            base.OnInit(e);
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Groups.EnableGroups)
                {
                    StatusPageMessage = Lang.TransA("Groups option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                if (!HasWriteAccess)
                {

                }

                loadStrings();
                populateDropDown();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            bindGroups();
        }

        private void loadStrings()
        {
            lblGroupsPerPage.Text = Lang.TransA("Groups per page");

            dgPendingApproval.Columns[0].HeaderText = Lang.TransA("Group Icon");
            dgPendingApproval.Columns[1].HeaderText = Lang.TransA("Group Details");

            lnkFirst.Text = Lang.TransA("[ First ]");
            lnkPrev.Text = Lang.TransA("[ Prev ]");
            lnkNext.Text = Lang.TransA("[ Next ]");
            lnkLast.Text = Lang.TransA("[ Last ]");
        }

        private void populateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                ddGroupsPerPage.Items.Add(i.ToString());
            ddGroupsPerPage.SelectedValue = Config.AdminSettings.ApproveGroups.GroupsPerPage.ToString();
        }

        private void DisableViewState(DataGrid dg)
        {
            foreach (DataGridItem dgi in dg.Items)
            {
                dgi.EnableViewState = false;
            }
        }

        private void bindGroups()
        {
            preparePaginator();

            DataTable dtGroups = new DataTable("Groups");

            dtGroups.Columns.Add("GroupID", typeof(int));
            dtGroups.Columns.Add("Categories");
            dtGroups.Columns.Add("Name");
            dtGroups.Columns.Add("Description");
            dtGroups.Columns.Add("Approved");
            dtGroups.Columns.Add("AccessLevel");
            dtGroups.Columns.Add("Owner");
            dtGroups.Columns.Add("DateCreated");

            Group[] groups = null;

            if (Results == null)
            {
                Results = new GroupSearchResults();

                Results.Groups =
                        Group.Search(null, null, null, null, false, null, null, null, null, false, null,
                                     Group.eSortColumn.DateCreated);

                if (Results.Groups.Length == 0)
                {
                    MessageBox.Show(Lang.TransA("There are no groups waiting for approval!"), Misc.MessageType.Error);
                    PaginatorEnabled = false;
                    dgPendingApproval.Visible = false;
                    lblGroupsPerPage.Visible = false;
                    ddGroupsPerPage.Visible = false;
                    return;
                }

                CurrentPage = 1;
            }

            groups = Results.GetPage(CurrentPage, GroupsPerPage);

            if (groups != null && groups.Length > 0)
            {
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

                    string accessLevel = String.Empty;

                    switch (group.AccessLevel)
                    {
                        case Group.eAccessLevel.Public:
                            accessLevel = Lang.TransA("Public Group");
                            break;
                        case Group.eAccessLevel.Moderated:
                            accessLevel = Lang.TransA("Moderated Group");
                            break;
                        case Group.eAccessLevel.Private:
                            accessLevel = Lang.TransA("Private Group");
                            break;
                    }

                    dtGroups.Rows.Add(new object[]
                                       {
                                           group.ID,
                                           strCategories,
                                           Parsers.ProcessGroupName(group.Name),
                                           Parsers.ProcessGroupDescription(group.Description),
                                           group.Approved ? Lang.TransA("Yes") : Lang.TransA("No"),
                                           accessLevel,
                                           group.Owner,
                                           group.DateCreated
                                       });
                }
            }

            dgPendingApproval.Visible = dtGroups.Rows.Count > 0;
            dgPendingApproval.DataSource = dtGroups;
            dgPendingApproval.DataBind();
            StoreGroupIDs(dtGroups);
            DisableViewState(dgPendingApproval);
        }

        private void StoreGroupIDs(DataTable dataTable)
        {
            string[] ids = new string[dataTable.Rows.Count];
            for (int n = 0; n < dataTable.Rows.Count; ++n)
                ids[n] = dataTable.Rows[n]["GroupID"].ToString();

            GroupIDs = ids;
        }

        /// <summary>
        /// Handles the ItemCommand event of the dgPendingApproval control.
        /// </summary>
        /// <param name="source">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridCommandEventArgs"/> instance containing the event data.</param>
        protected void dgPendingApproval_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            int groupID = Int32.Parse(GroupIDs[e.Item.ItemIndex]);
            Group group = Group.Fetch(groupID/*Convert.ToInt32(e.CommandArgument)*/);

            if (group != null)
            {
                if (e.CommandName == "Approve")
                {
                    try
                    {
                        User owner = Classes.User.Load(group.Owner);
                        MiscTemplates.ApproveGroupMessage approveGroupTemplate =
                            new MiscTemplates.ApproveGroupMessage(owner.LanguageId);

                        #region Add NewFriendGroup Event

                        Event newEvent = new Event(group.Owner);

                        newEvent.Type = Event.eType.NewFriendGroup;
                        NewFriendGroup newFriendGroup = new NewFriendGroup();
                        newFriendGroup.GroupID = group.ID;
                        newEvent.DetailsXML = Misc.ToXml(newFriendGroup);

                        newEvent.Save();

                        int imageID = 0;
                        try
                        {
                            imageID = Photo.GetPrimary(group.Owner).Id;
                        }
                        catch (NotFoundException)
                        {
                            imageID = ImageHandler.GetPhotoIdByGender(owner.Gender);
                        }

                        string[] usernames = Classes.User.FetchMutuallyFriends(group.Owner);
                        foreach (string friendUsername in usernames)
                        {
                            if (Config.Users.NewEventNotification)
                            {
                                string text = String.Format("Your friend {0} has created the {1} group".TranslateA(),
                                                      "<b>" + group.Owner + "</b>", Parsers.ProcessGroupName(group.Name));
                                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                Classes.User.SendOnlineEventNotification(group.Owner, friendUsername, text, thumbnailUrl,
                                                                         UrlRewrite.CreateShowGroupUrl(group.ID.ToString()));
                            }
                        }

                        #endregion

                        Message msg = new Message(Config.Users.SystemUsername, group.Owner);
                        msg.Body = approveGroupTemplate.Message.Replace("%%GROUP%%", group.Name);
                        msg.Send();
                    }
                    catch (NotFoundException ex)
                    {
                        Log(ex);
                    }

                    group.Approved = true;
                    group.Save();
                }
                else if (e.CommandName == "Reject")
                {
                    string reason = String.Empty;
                    TextBox txtReason = (TextBox) e.Item.FindControl("txtReason");

                    try
                    {
                        User user = Classes.User.Load(group.Owner);
                        MiscTemplates.RejectGroupMessage rejectGroupTemplate =
                            new MiscTemplates.RejectGroupMessage(user.LanguageId);
                        Message msg = new Message(Config.Users.SystemUsername, group.Owner);

                        reason = rejectGroupTemplate.WithReasonMessage;

                        if (txtReason.Text.Trim() != String.Empty)
                        {
                            reason =
                                reason.Replace("%%REASON%%", txtReason.Text.Trim()).Replace("%%GROUP%%", group.Name);
                            msg.Body = reason;
                        }
                        else
                        {
                            msg.Body = rejectGroupTemplate.WithNoReasonMessage.Replace("%%GROUP%%", group.Name);
                        }
                        msg.Send();
                    }
                    catch (NotFoundException ex)
                    {
                        Log(ex);
                    }

                    Group.Delete(groupID/*Convert.ToInt32(e.CommandArgument)*/);
                }

                Results = null;
            }
        }

        /// <summary>
        /// Handles the ItemCreated event of the dgPendingApproval control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void dgPendingApproval_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            Button btnApprove = (Button)e.Item.FindControl("btnApprove");
            Button btnReject = (Button)e.Item.FindControl("btnReject");
            btnApprove.Text = Lang.TransA("Approve");
            btnReject.Text = Lang.TransA("Reject");

            btnReject.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to reject this group?")));
        }

        /// <summary>
        /// Handles the ItemDataBound event of the dgPendingApproval control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Web.UI.WebControls.DataGridItemEventArgs"/> instance containing the event data.</param>
        protected void dgPendingApproval_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            Button btnApprove = (Button)e.Item.FindControl("btnApprove");
            Button btnReject = (Button)e.Item.FindControl("btnReject");

            if (!HasWriteAccess)
            {
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the ddGroupsPerPage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void ddGroupsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentPage = 0;
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
            if (CurrentPage < Results.GetTotalPages(GroupsPerPage))
                CurrentPage++;
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(GroupsPerPage))
                CurrentPage = Results.GetTotalPages(GroupsPerPage);
        }
    }
}
