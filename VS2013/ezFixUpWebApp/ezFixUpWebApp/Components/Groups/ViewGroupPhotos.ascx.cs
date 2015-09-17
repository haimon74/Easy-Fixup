using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class ViewGroupPhotos : UserControl
    {
        private View CurrentView;

        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int) ViewState["CurrentGroupId"];
                }
                throw new Exception("The group ID is not set!");
            }
            set { ViewState["CurrentGroupId"] = value; }
        }

        /// <summary>
        /// Gets or sets the group photo ID.
        /// Represents group photo ID which will be edited or deleted.
        /// </summary>
        /// <value>The group photo ID.</value>
        public int GroupPhotoID
        {
            get
            {
                if (ViewState["CurrentGroupPhotoID"] != null)
                {
                    return (int) ViewState["CurrentGroupPhotoID"];
                }
                throw new Exception("The group photo ID is not set!");
            }
            set { ViewState["CurrentGroupPhotoID"] = value; }
        }

        protected Group CurrentGroup
        {
            get
            {
                if (Page is ShowGroupPhotos)
                {
                    return ((ShowGroupPhotos) Page).Group;
                }
                return Group.Fetch(GroupID);
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        public GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroupPhotos)
                {
                    return ((ShowGroupPhotos) Page).CurrentGroupMember;
                }
                if (CurrentUserSession != null)
                {
                    return GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                }
                return null;
            }
        }

        public bool BtnUploadPhotoVisible
        {
            set { btnUploadPhoto.Visible = value; }
        }

        public GroupPhotoSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (GroupPhotoSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                preparePaginator();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int) ViewState["CurrentPage"] == 0)
                    return 1;
                return (int) ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            set { pnlPaginator.Visible = value; }
        }

        public event EventHandler UploadPhotoClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            CurrentView = viewGroupPhotosView;

            if (!IsPostBack)
            {
                loadStrings();
                mvGroupPhotos.SetActiveView(viewGroupPhotosView);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadGroupPhotos();
            loadResultsPage();

            mvGroupPhotos.SetActiveView(CurrentView);

            Page.RegisterJQuery();
            Page.RegisterJQueryLightbox();

            Page.Header.Controls.Add(new LiteralControl(
                "<link href=\"images/jquery.lightbox.css\" rel=\"stylesheet\" type=\"text/css\" />"));
            ScriptManager.RegisterStartupScript(this, typeof(ViewGroupPhotos), "lightbox",
                "$(function() {$('div.gallery-img a').lightBox();});", true);
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Group Photos");

            rbList.Items.Add(new ListItem(Lang.Trans("Delete photo"), "deletePhoto"));
            rbList.Items.Add(new ListItem(Lang.Trans("Delete all group photos of this member from this group"),
                                          "deleteAll"));

            ddBanPeriod.Items.Add(new ListItem("", "-1"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("1 day"), "1"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("3 days"), "3"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("week"), "7"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("2 weeks"), "14"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("month"), "30"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("3 months"), "90"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("6 months"), "180"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("Forever"), "0"));

            cbKickMember.Text = Lang.Trans("Kick Member");
            cbBanMember.Text = Lang.Trans("Ban Member");

            btnSubmit.Text = Lang.Trans("Submit");
            btnCancel.Text = Lang.Trans("Cancel");
            btnUpdate.Text = Lang.Trans("Update");
            btnCancelUpdate.Text = Lang.Trans("Cancel");
            btnUploadPhoto.Text = Lang.Trans("Upload Photo");
            btnUploadPhoto.Visible = CurrentUserSession != null &&
                                     ((CurrentGroupMember != null && CurrentGroupMember.Active) ||
                                      CurrentUserSession.IsAdmin());

            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");

            lnkRunSlideshow.Text = Lang.Trans("View as a slideshow");
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(Config.Groups.GroupPhotosPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.GroupPhotos.Length > 0)
            {
                int fromPhoto = (CurrentPage - 1)*Config.Groups.GroupPhotosPerPage + 1;
                int toPhoto = CurrentPage*Config.Groups.GroupPhotosPerPage;
                if (Results.GroupPhotos.Length < toPhoto)
                    toPhoto = Results.GroupPhotos.Length;

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromPhoto, toPhoto, Results.GroupPhotos.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        private void loadResultsPage()
        {
            preparePaginator();

            var dtGroupPosts = new DataTable("SearchResults");

            dtGroupPosts.Columns.Add("GroupPhotoID");
            dtGroupPosts.Columns.Add("GroupID");
            dtGroupPosts.Columns.Add("Username");
            dtGroupPosts.Columns.Add("Name");
            dtGroupPosts.Columns.Add("Description");
            dtGroupPosts.Columns.Add("Date");

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);

                GroupPhoto[] groupPhotos = Results.GetPage(CurrentPage, Config.Groups.GroupPhotosPerPage);

                if (groupPhotos != null && groupPhotos.Length > 0)
                {
                    foreach (GroupPhoto groupPhoto in groupPhotos)
                    {
                        if (groupPhoto == null) // somebody else delete this photo
                        {
                            continue;
                        }

                        dtGroupPosts.Rows.Add(new object[]
                                                  {
                                                      groupPhoto.ID,
                                                      groupPhoto.GroupID,
                                                      groupPhoto.Username,
                                                      groupPhoto.Name.Replace('\'', '’'),
                                                      groupPhoto.Description.Replace('\'', '’'),
                                                      groupPhoto.Date.ToShortDateString()
                                                  });
                    }
                }
            }

            Trace.Write("Binding...");

            dlGroupPhotos.DataSource = dtGroupPosts;
            dlGroupPhotos.DataBind();
        }

        private void loadGroupPhotos()
        {
            if (Results == null)
            {
                Results = new GroupPhotoSearchResults
                              {
                                  GroupPhotos = GroupPhoto.Search(GroupID, null, null, null, null,
                                                                  GroupPhoto.eSortColumn.DateUploaded)
                              };

                if (Results.GroupPhotos.Length == 0)
                {
                    PaginatorEnabled = false;
                    dlGroupPhotos.Visible = false;

                    lblError.Text = Lang.Trans("There are no group photos.");
                    return;
                }
                PaginatorEnabled = true;
                dlGroupPhotos.Visible = true;

                CurrentPage = 1;
            }
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
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupPhotosPerPage))
                CurrentPage++;
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupPhotosPerPage))
                CurrentPage = Results.GetTotalPages(Config.Groups.GroupPhotosPerPage);
        }

        protected void dlGroupPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int groupPhotoID = Convert.ToInt32(e.CommandArgument);
            GroupPhotoID = groupPhotoID;

            GroupPhoto groupPhoto = GroupPhoto.Fetch(groupPhotoID);

            if (groupPhoto != null)
            {
                switch (e.CommandName)
                {
                    case "Edit":
                        txtName.Text = groupPhoto.Name;
                        txtDescription.Text = groupPhoto.Description;

                        CurrentView = viewEditGroupPhoto;
                        break;

                    case "Delete":
                        bool isMember = false;
                        bool isActive = false;

                        GroupMember groupMember = GroupMember.Fetch(GroupID, groupPhoto.Username);

                        if (groupMember != null)
                        {
                            isMember = true;
                            isActive = groupMember.Active;
                        }

                        cbKickMember.Visible =
                            isMember && isActive
                            && groupPhoto.Username != CurrentUserSession.Username
                            && groupPhoto.Username != Config.Users.SystemUsername
                            && groupPhoto.Username != CurrentGroup.Owner;

                        bool isBanned =
                            isMember && isActive
                            && !GroupMember.IsBanned(groupPhoto.Username, GroupID)
                            && groupPhoto.Username != CurrentUserSession.Username
                            && groupPhoto.Username != Config.Users.SystemUsername
                            && groupPhoto.Username != CurrentGroup.Owner;

                        cbBanMember.Visible = isBanned;
                        ddBanPeriod.Visible = isBanned;

                        cbKickMember.Checked = false;
                        cbBanMember.Checked = false;

                        if (CurrentUserSession.Username != groupPhoto.Username)
                        {
                            CurrentView = viewDeleteOptions;
                        }
                        else
                        {
                            GroupPhoto.Delete(groupPhotoID);

                            Response.Redirect(UrlRewrite.CreateShowGroupPhotosUrl(GroupID.ToString()));
                        }
                        break;
                }
            }
        }

        protected void dlGroupPhotos_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var username = (string) DataBinder.Eval(e.Item.DataItem, "Username");

            var lnkEdit = (LinkButton) e.Item.FindControl("lnkEdit");
            var lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");
            var ulEditControls = (HtmlGenericControl)e.Item.FindControl("ulEditControls");

            if (lnkEdit != null && lnkDelete != null)
            {
                lnkEdit.Text = Lang.Trans("Edit");
                lnkDelete.Text = Lang.Trans("Delete");

                lnkEdit.Visible = false;
                lnkDelete.Visible = false;
                ulEditControls.Visible = false;

                if (CurrentUserSession != null)
                {
                    if (CurrentGroupMember == null || !CurrentGroupMember.Active) // is not member
                    {
                        if (CurrentGroup.IsPermissionEnabled(eGroupPermissions.UploadPhotoNonMembers))
                        {
                            if (CurrentUserSession.Username == username)
                            {
                                lnkEdit.Visible = true;
                                lnkDelete.Visible = true;
                                ulEditControls.Visible = true;

                                lnkDelete.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                                                  Lang.Trans(
                                                                                      "Are you sure you want to delete this photo?")));
                            }
                        }
                    }
                    else //is member
                    {
                        if (CurrentGroupMember.Type == GroupMember.eType.Member)
                        {
                            if (CurrentGroup.IsPermissionEnabled(eGroupPermissions.UploadPhotoMembers))
                            {
                                if (CurrentUserSession.Username == username)
                                {
                                    lnkEdit.Visible = true;
                                    lnkDelete.Visible = true;
                                    ulEditControls.Visible = true;

                                    lnkDelete.Attributes.Add("onclick",
                                                             String.Format("javascript: return confirm('{0}')",
                                                                           Lang.Trans(
                                                                               "Are you sure you want to delete this photo?")));
                                }
                            }
                        }
                        else if (CurrentGroupMember.Type == GroupMember.eType.VIP)
                        {
                            if (CurrentGroup.IsPermissionEnabled(eGroupPermissions.UploadPhotoMembers))
                            {
                                if (CurrentUserSession.Username == username)
                                {
                                    lnkEdit.Visible = true;
                                    lnkDelete.Visible = true;
                                    ulEditControls.Visible = true;

                                    lnkDelete.Attributes.Add("onclick",
                                                             String.Format("javascript: return confirm('{0}')",
                                                                           Lang.Trans(
                                                                               "Are you sure you want to delete this photo?")));
                                }
                            }
                        }
                        else if (CurrentGroupMember.Type == GroupMember.eType.Admin ||
                                 CurrentGroupMember.Type == GroupMember.eType.Moderator)
                        {
                            lnkEdit.Visible = true;
                            lnkDelete.Visible = true;
                            ulEditControls.Visible = true;
                        }
                    }
                }

                if (CurrentUserSession != null && CurrentUserSession.IsAdmin())
                {
                    lnkEdit.Visible = true;
                    lnkDelete.Visible = true;
                    ulEditControls.Visible = true;
                }
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null || (CurrentGroupMember == null && !CurrentUserSession.IsAdmin())
                || (CurrentGroupMember != null
                    && CurrentGroupMember.Type != GroupMember.eType.Admin
                    && CurrentGroupMember.Type != GroupMember.eType.Moderator))
                return;

            bool kicked = false;

            GroupPhoto groupPhoto = GroupPhoto.Fetch(GroupPhotoID);

            if (groupPhoto != null)
            {
                if (cbBanMember.Checked)
                {
                    if (ddBanPeriod.SelectedValue != "-1")
                    {
                        var groupBan = new GroupBan(GroupID, groupPhoto.Username);

                        GroupMember.Delete(GroupID, groupPhoto.Username); // kick member

                        kicked = true;

                        groupBan.Expires = ddBanPeriod.SelectedValue == "0" ? DateTime.Now.AddYears(5) : 
                            DateTime.Now.AddDays(Double.Parse(ddBanPeriod.SelectedValue));

                        groupBan.Save();
                    }
                    else
                    {
                        lblError.Text = Lang.Trans("Please select a period.");
                        CurrentView = viewDeleteOptions;
                        return;
                    }
                }

                if (cbKickMember.Checked && !kicked)
                {
                    GroupMember.Delete(GroupID, groupPhoto.Username);

                    kicked = true;
                }

                if (kicked)
                {
                    CurrentGroup.ActiveMembers--;
                    CurrentGroup.Save();
                }

                switch (rbList.SelectedValue)
                {
                    case "deletePhoto":
                        GroupPhoto.Delete(GroupPhotoID);
                        break;

                    case "deleteAll":
                        GroupPhoto.Delete(GroupID, groupPhoto.Username);
                        break;
                }

                Results = null;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CurrentView = viewGroupPhotosView;
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null || (CurrentGroupMember == null && !CurrentUserSession.IsAdmin())) 
                return;

            string name = Config.Misc.EnableBadWordsFilterGroups
                              ? Parsers.ProcessBadWords(txtName.Text.Trim())
                              : txtName.Text.Trim();
            string description = Config.Misc.EnableBadWordsFilterGroups
                                     ? Parsers.ProcessBadWords(txtDescription.Text.Trim())
                                     : txtDescription.Text.Trim();

            if (name.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter name");
                CurrentView = viewEditGroupPhoto;
                return;
            }

            if (description.Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter description");
                CurrentView = viewEditGroupPhoto;
                return;
            }

            GroupPhoto groupPhoto = GroupPhoto.Fetch(GroupPhotoID);

            if (groupPhoto != null)
            {
                groupPhoto.Name = name;
                groupPhoto.Description = description;

                groupPhoto.Save();
            }
        }

        protected void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            CurrentView = viewGroupPhotosView;
        }

        protected void btnUploadPhoto_Click(object sender, EventArgs e)
        {
            OnUploadPhotoClick(e);
        }

        protected virtual void OnUploadPhotoClick(EventArgs e)
        {
            if (UploadPhotoClick != null)
            {
                UploadPhotoClick(this, e);
            }
        }

        protected void lnkRunSlideshow_Click(object sender, EventArgs e)
        {
            if (divSlideshow.Visible)
            {
                divSlideshow.Visible = false;
                mvGroupPhotos.Visible = true;
                lnkRunSlideshow.Text = Lang.Trans("View as a slideshow");
            }
            else
            {
                divSlideshow.Visible = true;
                mvGroupPhotos.Visible = false;
                lnkRunSlideshow.Text = Lang.Trans("Back to photos");
            }
        }
    }
}