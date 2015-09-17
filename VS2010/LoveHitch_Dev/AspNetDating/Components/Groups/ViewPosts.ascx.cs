using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Groups
{
    public partial class ViewPosts : UserControl
    {
        #region Delegates

        public delegate void EditPostClickEventHandler(object sender, EditPostClickEventArgs eventArgs);

        public delegate void ReplyClickEventHandler(object sender, ReplyClickEventArgs eventArgs);

        #endregion

        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int) ViewState["CurrentGroupId"];
                }
                throw new NotFoundException("The group ID is not set!");
            }
            set { ViewState["CurrentGroupId"] = value; }
        }

        public int TopicID
        {
            get
            {
                if (ViewState["CurrentTopicId"] != null)
                {
                    return (int) ViewState["CurrentTopicId"];
                }
                throw new Exception("The topic ID is not set!");
            }
            set { ViewState["CurrentTopicId"] = value; }
        }

        private int PostID
        {
            get
            {
                if (ViewState["PostID"] != null)
                {
                    return (int) ViewState["PostID"];
                }
                throw new Exception("The post ID is not set!");
            }
            set { ViewState["PostID"] = value; }
        }

        public int? PageNumber { get; set; }

        protected Group CurrentGroup
        {
            get
            {
                if (Page is ShowGroupTopics)
                {
                    return ((ShowGroupTopics) Page).Group;
                }
                return Group.Fetch(GroupID);
            }
        }

        public GroupTopic CurrentTopic
        {
            get
            {
                int topicID;
                if (Int32.TryParse((Request.Params["tid"]), out topicID) && ViewState["CurrentTopic"] == null)
                {
                    ViewState["CurrentTopic"] = GroupTopic.Fetch(topicID);
                }

//                if (ViewState["CurrentTopic"] == null)
//                {
//                    ((PageBase) Page).StatusPageMessage = Lang.Trans("This topic doesn't exist!");
//                    Response.Redirect("~/ShowStatus.aspx");
//                }

                return ViewState["CurrentTopic"] as GroupTopic;
            }
        }

        public GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroupTopics)
                {
                    return ((ShowGroupTopics) Page).CurrentGroupMember;
                }
                if (CurrentUserSession != null)
                {
                    return GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                }
                return null;
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        public bool ShowLastPost { get; set; }

        public GroupPostSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (GroupPostSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                PreparePaginator();
                dlGroupPosts.DataSource = null;
                dlGroupPosts.DataBind();
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

        public event ReplyClickEventHandler ReplyClick;

        public event EditPostClickEventHandler EditPostClick;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                mvPosts.SetActiveView(viewPosts);
                GroupPoll1.TopicID = TopicID;
                loadStrings();
            }

            if (!Visible)
            {
                Results = null;
                ViewState["CurrentTopic"] = null;
            }
            else
            {
                if (CurrentTopic != null)
                    lblTopicName.Text = Parsers.ProcessGroupTopicName(CurrentTopic.Name);
                else
                {
                    try
                    {
                        Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
                    }
                    catch (NotFoundException)
                    {
                        ((PageBase) Page).StatusPageMessage = Lang.Trans("The Group no longer exists!");
                        Response.Redirect("~/ShowStatus.aspx");
                    }
                    return;
                }

                // Load the results in advance unless paginator is used
                var eventTarget = Page.Request.Params["__EVENTTARGET"];
                if (eventTarget != null && !eventTarget.EndsWith("lnkNext") 
                    && !eventTarget.EndsWith("lnkPrev") && !eventTarget.EndsWith("lnkNext") 
                    && !eventTarget.EndsWith("lnkLast"))
                {
                    loadResultsPage();
                }
            }

            #region set dynamic meta tag

            Parser parse = delegate(string text)
                               {
                                   string result = String.Empty;

                                   if (CurrentTopic != null)
                                   {
                                       result =
                                           text.Replace("%%NAME%%", CurrentTopic.Name)
                                               .Replace("%%GROUP%%", CurrentGroup.Name)
                                               .Replace("%%USERNAME%%", CurrentTopic.Username);
                                   }

                                   return result;
                               };


            Page.Header.Title = Config.SEO.ShowGroupTopicTitleTemplate.Length > 0
                                    ? parse(Config.SEO.ShowGroupTopicTitleTemplate)
                                    : Config.SEO.DefaultTitleTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle);

            Control[] controls = new Control[Page.Header.Controls.Count];
            Page.Header.Controls.CopyTo(controls, 0);
            var descriptionMetaTag = Array.Find(controls, c => c is HtmlMeta && c.ID == "Description");
            var keywordsMetaTag = Array.Find(controls, c => c is HtmlMeta && c.ID == "Keywords");

            if (descriptionMetaTag != null)
                Page.Header.Controls.Remove(descriptionMetaTag);
            if (keywordsMetaTag != null)
                Page.Header.Controls.Remove(keywordsMetaTag);

            var metaDesc = new HtmlMeta
                               {
                                   ID = "Description",
                                   Name = "description",
                                   Content = Config.SEO.ShowGroupTopicMetaDescriptionTemplate.Length > 0
                                   ? parse(Config.SEO.ShowGroupTopicMetaDescriptionTemplate)
                                   : Config.SEO.DefaultMetaDescriptionTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle)
                               };
            Page.Header.Controls.Add(metaDesc);

            var metaKeywords = new HtmlMeta
                                   {
                                       ID = "Keywords",
                                       Name = "keywords",
                                       Content = Config.SEO.ShowGroupTopicMetaKeywordsTemplate.Length > 0
                                       ? parse(Config.SEO.ShowGroupTopicMetaKeywordsTemplate)
                                       : Config.SEO.DefaultMetaKeywordsTemplate.Replace("%%NAME%%",
                                                                                        Config.Misc.SiteTitle)
                                   };
            Page.Header.Controls.Add(metaKeywords);

            #endregion
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            GroupPoll1.Visible = CurrentTopic != null && CurrentTopic.IsPoll &&
                                 mvPosts.GetActiveView() != viewDeleteOptions;

            loadPosts();
            loadResultsPage();
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Posts");

            rbList.Items.Add(new ListItem(Lang.Trans("Delete post"), "deletePost"));
            rbList.Items.Add(new ListItem(Lang.Trans("Delete all posts of this member from current topic"),
                                          "deleteFromTopic"));
            rbList.Items.Add(new ListItem(Lang.Trans("Delete all post of this member"), "deleteAll"));

            ddBanPeriod.Items.Add(new ListItem("", "-1"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("1 day"), "1"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("3 days"), "3"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("week"), "7"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("2 weeks"), "14"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("month"), "30"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("3 months"), "90"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("6 months"), "180"));
            ddBanPeriod.Items.Add(new ListItem(Lang.Trans("Forever"), "0"));

            ddWarnExpirationDate.Items.Add(new ListItem("", "-1"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("1 day"), "1"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("3 days"), "3"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("week"), "7"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("2 weeks"), "14"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("month"), "30"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("3 months"), "90"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("6 months"), "180"));
            ddWarnExpirationDate.Items.Add(new ListItem(Lang.Trans("Forever"), "0"));

            cbKickMember.Text = Lang.Trans("Kick Member");
            cbBanMember.Text = Lang.Trans("Ban Member");
            cbWarn.Text = "Warn Member".Translate();

            TBWEReason.WatermarkText = Lang.Trans("Type Reason Here");

            btnSubmit.Text = Lang.Trans("Submit");
            btnCancel.Text = Lang.Trans("Cancel");

            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");
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
            if (Results == null || CurrentPage >= Results.GetTotalPages(Config.Groups.GroupPostsPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.GroupPosts.Length > 0)
            {
                int fromPost = (CurrentPage - 1)*Config.Groups.GroupPostsPerPage + 1;
                int toPost = CurrentPage*Config.Groups.GroupPostsPerPage;
                if (Results.GroupPosts.Length < toPost)
                    toPost = Results.GroupPosts.Length;

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromPost, toPost, Results.GroupPosts.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        private void loadResultsPage()
        {
            if (dlGroupPosts.Items.Count > 0) return;

            PreparePaginator();

            var dtGroupPosts = new DataTable("SearchResults");

            dtGroupPosts.Columns.Add("GroupPostID");
            dtGroupPosts.Columns.Add("GroupTopicID");
            dtGroupPosts.Columns.Add("Username");
            dtGroupPosts.Columns.Add("DatePosted");
            dtGroupPosts.Columns.Add("DateEdited");
            dtGroupPosts.Columns.Add("EditNotes");
            dtGroupPosts.Columns.Add("Post");
            dtGroupPosts.Columns.Add("ImageID", typeof (int));
            dtGroupPosts.Columns.Add("UserLevel", typeof (UserLevel));
            dtGroupPosts.Columns.Add("HideUserLevelIcon", typeof (bool));
            dtGroupPosts.Columns.Add("IsWarned", typeof(bool));
            dtGroupPosts.Columns.Add("WarnReason");

            if (Results != null)
            {
                Trace.Write("Loading page " + CurrentPage);

                GroupPost[] groupPosts = Results.GetPage(CurrentPage, Config.Groups.GroupPostsPerPage);

                if (groupPosts != null && groupPosts.Length > 0)
                {
                    for (int i = 0; i < groupPosts.Length; i++)
                    {
                        GroupPost groupPost = groupPosts[i];

                        if (groupPost == null) // somebody else delete this post
                        {
                            continue;
                        }

                        int imageID = 0;
                        try
                        {
                            imageID = Photo.GetPrimary(groupPost.Username).Id;
                        }
                        catch (NotFoundException)
                        {
                            try
                            {
                                User user = User.Load(groupPost.Username);
                                imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                            }
                            catch (NotFoundException)
                            {
                            }
                        }

                        UserLevel userLevel = null;
                        bool hideUserLevelIcon = false;
                        if (Config.UserScores.EnableUserLevels && Config.UserScores.ShowLevelIcons)
                        {
                            User user = User.Load(groupPost.Username);
                            if (!user.Deleted)
                            {
                                userLevel = user.Level;
                                hideUserLevelIcon = user.IsOptionEnabled(eUserOptions.DisableLevelIcon);
                            }
                        }

                        bool isWarned = false;
                        string warnReason = String.Empty;
                        GroupMember groupMember = GroupMember.Fetch(GroupID, groupPost.Username);
                        if (groupMember != null && groupMember.IsWarned && groupMember.WarnExpirationDate > DateTime.Now)
                        {
                            isWarned = true;
                            warnReason = groupMember.WarnReason;
                        }

                        dtGroupPosts.Rows.Add(new object[]
                                                  {
                                                      groupPost.ID,
                                                      groupPost.GroupTopicID,
                                                      groupPost.Username,
                                                      groupPost.DatePosted.Add(Config.Misc.TimeOffset),
                                                      groupPost.DateEdited != null
                                                          ? groupPost.DateEdited.Value.Add(Config.Misc.TimeOffset).
                                                                ToString()
                                                          : null,
                                                      Server.HtmlEncode(groupPost.EditNotes),
                                                      Parsers.ProcessGroupPost(groupPost.Post),
                                                      imageID,
                                                      userLevel,
                                                      hideUserLevelIcon,
                                                      isWarned,
                                                      Server.HtmlEncode(warnReason)
                                                  });
                    }
                }
            }

            Trace.Write("Binding...");

            dlGroupPosts.DataSource = dtGroupPosts;
            dlGroupPosts.DataBind();
        }

        private void loadPosts()
        {
            if (Results == null)
            {
                Results = new GroupPostSearchResults
                              {
                                  GroupPosts = GroupPost.Search(TopicID, null, null, null, null,
                                                                GroupPost.eSortColumn.DatePosted)
                              };

                if (Results.GroupPosts.Length == 0)
                {
                    PaginatorEnabled = false;
                    dlGroupPosts.Visible = false;

                    lblError.Text = Lang.Trans("There are no posts.");
                    return;
                }
                PaginatorEnabled = true;
                dlGroupPosts.Visible = true;

                CurrentPage = PageNumber != null && PageNumber != -1 ? PageNumber.Value : 1;

                // go to last page
                if (PageNumber == -1)
                {
                    CurrentPage = Results.GetTotalPages(Config.Groups.GroupPostsPerPage);
                }
            }
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
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupPostsPerPage))
            {
                CurrentPage++;
            }
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Groups.GroupPostsPerPage))
            {
                CurrentPage = Results.GetTotalPages(Config.Groups.GroupPostsPerPage);
            }
        }

        protected void dlGroupPosts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var liReply = (HtmlGenericControl) e.Item.FindControl("liReply");
            var liEdit = (HtmlGenericControl) e.Item.FindControl("liEdit");
            var liDelete = (HtmlGenericControl) e.Item.FindControl("liDelete");

            var lnkReply = (LinkButton) e.Item.FindControl("lnkReply");
            var lnkEdit = (LinkButton) e.Item.FindControl("lnkEdit");
            var lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");


            var pnlDateEdited = (HtmlGenericControl) e.Item.FindControl("pnlDateEdited");
            var pnlEditNotes = (HtmlGenericControl) e.Item.FindControl("pnlEditNotes");

            if (liReply != null && liEdit != null && liDelete != null &&
                lnkReply != null && lnkEdit != null && lnkDelete != null &&
                pnlEditNotes != null && pnlDateEdited != null)
            {
                lnkReply.Text = Lang.Trans("Reply");
                lnkEdit.Text = Lang.Trans("Edit");
                lnkDelete.Text = Lang.Trans("Delete");

                liReply.Visible = false;
                liEdit.Visible = false;
                liDelete.Visible = false;

                if (DataBinder.Eval(e.Item.DataItem, "DateEdited") == DBNull.Value)
                {
                    pnlDateEdited.Visible = false;
                }

                if (DataBinder.Eval(e.Item.DataItem, "EditNotes") == DBNull.Value)
                {
                    pnlEditNotes.Visible = false;
                }

                if (CurrentUserSession != null)
                {
                    if (CurrentGroupMember == null || !CurrentGroupMember.Active) // is not member
                    {
                        if (CurrentGroup.IsPermissionEnabled(eGroupPermissions.AddPostNonMembers))
                        {
                            setActions(liEdit, liDelete, liReply, lnkDelete, e);
                        }
                    }
                    else // is member
                    {
                        if (CurrentGroupMember.Type == GroupMember.eType.Member)
                        {
                            if (CurrentGroup.IsPermissionEnabled(eGroupPermissions.AddPostMembers))
                            {
                                setActions(liEdit, liDelete, liReply, lnkDelete, e);
                            }
                        }
                        else if (CurrentGroupMember.Type == GroupMember.eType.VIP)
                        {
                            if (CurrentGroup.IsPermissionEnabled(eGroupPermissions.AddPostVip))
                            {
                                setActions(liEdit, liDelete, liReply, lnkDelete, e);
                            }
                        }
                        else if (CurrentGroupMember.Type == GroupMember.eType.Admin ||
                                 CurrentGroupMember.Type == GroupMember.eType.Moderator)
                        {
                            if (CurrentUserSession.Username == (string) DataBinder.Eval(e.Item.DataItem, "Username"))
                            {
                                liEdit.Visible = true;
                                liDelete.Visible = true;
                            }
                            else
                            {
                                liReply.Visible = true;
                                liDelete.Visible = true;
                            }
                        }
                    }

                    if (CurrentUserSession != null && CurrentUserSession.IsAdmin())
                    {
                        if (CurrentUserSession.Username == (string) DataBinder.Eval(e.Item.DataItem, "Username"))
                        {
                            liEdit.Visible = true;
                            liDelete.Visible = true;
                        }
                        else
                        {
                            liReply.Visible = true;
                            liDelete.Visible = true;
                        }
                    }
                }
            }
        }

        protected void dlGroupPosts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Reply":
                    OnReplyClick(TopicID, Convert.ToInt32(e.CommandArgument), CurrentPage);
                    break;

                case "Edit":
                    OnEditPostClick(TopicID, Convert.ToInt32(e.CommandArgument), CurrentPage);
                    break;

                case "Delete":
                    PostID = Convert.ToInt32(e.CommandArgument);
                    GroupPost groupPost = GroupPost.Fetch(PostID);

                    if (groupPost != null)
                    {
                        if (CurrentUserSession.Username != groupPost.Username)
                        {
                            bool isMember = false;
                            bool isActive = false;

                            GroupMember groupMember = GroupMember.Fetch(GroupID, groupPost.Username);

                            if (groupMember != null)
                            {
                                isMember = true;
                                isActive = groupMember.Active;
                            }

                            cbKickMember.Visible =
                                isMember && isActive
                                && groupPost.Username != CurrentUserSession.Username
                                && groupPost.Username != Config.Users.SystemUsername
                                && groupPost.Username != CurrentGroup.Owner;

                            bool isBanned =
                                isMember && isActive
                                && !GroupMember.IsBanned(groupPost.Username, GroupID)
                                && groupPost.Username != CurrentUserSession.Username
                                && groupPost.Username != Config.Users.SystemUsername
                                && groupPost.Username != CurrentGroup.Owner;

                            cbBanMember.Visible = isBanned;
                            ddBanPeriod.Visible = isBanned;

                            cbKickMember.Checked = false;
                            cbBanMember.Checked = false;

                            mvPosts.SetActiveView(viewDeleteOptions);
                            return;
                        }
                        else // currently logged in user is the owner of the post
                        {
                            GroupPost.Delete(PostID);

                            User.AddScore(groupPost.Username, Config.UserScores.DeletedPost, "DeletedPost");
                        }

                        if (CurrentTopic != null)
                        {
                            CurrentTopic.Posts--;
                        }
                        else
                        {
                            Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
                            return;
                        }

                        GroupTopic groupTopic = GroupTopic.Fetch(CurrentTopic.ID);

                        if (groupTopic == null)
                        {
                            DeleteNewGroupTopicEvent(CurrentTopic.Username, CurrentTopic.ID);

                            Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
                        }
                        else
                        {
                            Results = null;
                        }
                    }

                    break;
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

            GroupPost groupPost = GroupPost.Fetch(PostID);

            if (groupPost != null)
            {
                if (!cbKickMember.Checked && !cbBanMember.Checked && cbWarn.Checked)
                {
                    if (ddWarnExpirationDate.SelectedValue != "-1")
                    {
                        GroupMember groupMember = GroupMember.Fetch(GroupID, groupPost.Username);
                        if (groupMember != null)
                        {
                            groupMember.IsWarned = true;
                            groupMember.WarnReason = txtWarnReason.Text.Trim();
                            groupMember.WarnExpirationDate = ddWarnExpirationDate.SelectedValue == "0"
                                                                 ? DateTime.Now.AddYears(5)
                                                                 : DateTime.Now.AddDays(
                                                                       Double.Parse(ddWarnExpirationDate.SelectedValue));
                            groupMember.Save();

                            User user = null;
                            try
                            {
                                user = User.Load(groupMember.Username);
                                MiscTemplates.WarnGroupMemberMessage warnGroupMemberMessageTemplate =
                                    new MiscTemplates.WarnGroupMemberMessage(user.LanguageId);
                                Message.Send(CurrentUserSession.Username, user.Username,
                                             warnGroupMemberMessageTemplate.GetFormattedMessage(CurrentGroup.Name,
                                                                                                groupMember.WarnReason),
                                             0);
                            }
                            catch (NotFoundException){}
                        }
                    }
                    else
                    {
                        lblError.Text = Lang.Trans("Please select a period.");
                        return;
                    }
                }

                if (cbBanMember.Checked)
                {
                    if (ddBanPeriod.SelectedValue != "-1")
                    {
                        var groupBan = new GroupBan(GroupID, groupPost.Username);

                        GroupMember.Delete(GroupID, groupPost.Username); // kick member

                        kicked = true;

                        groupBan.Expires = ddBanPeriod.SelectedValue == "0" ? DateTime.Now.AddYears(5) : DateTime.Now.AddDays(Double.Parse(ddBanPeriod.SelectedValue));

                        groupBan.Save();
                    }
                    else
                    {
                        lblError.Text = Lang.Trans("Please select a period.");
                        return;
                    }
                }

                if (cbKickMember.Checked && !kicked)
                {
                    GroupMember.Delete(GroupID, groupPost.Username);

                    kicked = true;
                }

                if (kicked)
                {
                    CurrentGroup.ActiveMembers--;
                    CurrentGroup.Save();
                }

                switch (rbList.SelectedValue)
                {
                    case "deletePost":
                        GroupPost.Delete(PostID);
                        if (GroupTopic.Fetch(CurrentTopic.ID) == null) 
                            DeleteNewGroupTopicEvent(CurrentTopic.Username, CurrentTopic.ID);
                        User.AddScore(groupPost.Username, Config.UserScores.DeletedPost, "DeletedPost");
                        break;

                    case "deleteFromTopic":
                        GroupPost.Delete(TopicID, groupPost.Username);
                        if (GroupTopic.Fetch(CurrentTopic.ID) == null)
                            DeleteNewGroupTopicEvent(CurrentTopic.Username, CurrentTopic.ID);
                        User.AddScore(groupPost.Username, Config.UserScores.DeletedPost, "DeletedPost");
                        break;

                    case "deleteAll":
                        GroupPost.Delete(groupPost.Username, GroupID);
                        if (GroupTopic.Fetch(CurrentTopic.ID) == null)
                            DeleteNewGroupTopicEvent(CurrentTopic.Username, CurrentTopic.ID);
                        User.AddScore(groupPost.Username, Config.UserScores.DeletedPost, "DeletedPost");
                        break;
                }

                // when group posts has been deleted current group topic should be updated
                if (rbList.SelectedIndex != -1)
                {
                    ViewState["CurrentTopic"] = GroupTopic.Fetch(TopicID);
                }

                if (CurrentTopic == null) // current topic doesn't exist
                {
                    Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
                    return;
                }

                GroupTopic groupTopic = GroupTopic.Fetch(CurrentTopic.ID);

                if (groupTopic == null)
                {
                    Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
                    return;
                }
                Results = null;
                mvPosts.SetActiveView(viewPosts);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvPosts.SetActiveView(viewPosts);
        }

        public void ShowMessage(Misc.MessageType type, string message)
        {
            lblError.Text = message;
            switch (type)
            {
                case Misc.MessageType.Error:
                    lblError.CssClass = "error";
                    break;
                case Misc.MessageType.Success:
                    lblError.CssClass = "message";
                    break;
            }
        }

        private void OnReplyClick(int topicID, int postID, int currentPage)
        {
            ReplyClick(this, new ReplyClickEventArgs(topicID, postID, currentPage));
        }

        private void OnEditPostClick(int topicID, int postID, int currentPage)
        {
            EditPostClick(this, new EditPostClickEventArgs(topicID, postID, currentPage));
        }

        private void setActions(Control liEdit, Control liDelete, Control liReply,
                                WebControl lnkDelete, DataListItemEventArgs e)
        {
            if (!CurrentTopic.Locked)
            {
                if (CurrentUserSession.Username == (string) DataBinder.Eval(e.Item.DataItem, "Username"))
                {
                    liEdit.Visible = true;
                    liDelete.Visible = true;
                    lnkDelete.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                                      Lang.Trans(
                                                                          "Are you sure you want to delete this topic?")));
                }
                else
                {
                    liReply.Visible = true;
                }
            }
        }

        #region Nested type: EditPostClickEventArgs

        public class EditPostClickEventArgs : EventArgs
        {
            private readonly int currentPage;
            private readonly int postID;
            private readonly int topicID;

            public EditPostClickEventArgs(int topicID, int postID, int currentPage)
            {
                this.topicID = topicID;
                this.postID = postID;
                this.currentPage = currentPage;
            }

            public int TopicID
            {
                get { return topicID; }
            }

            public int PostID
            {
                get { return postID; }
            }

            public int CurrentPage
            {
                get { return currentPage; }
            }
        }

        #endregion

        #region Nested type: Parser

        private delegate string Parser(string s);

        #endregion

        #region Nested type: ReplyClickEventArgs

        public class ReplyClickEventArgs : EventArgs
        {
            private readonly int currentPage;
            private readonly int postID;
            private readonly int topicID;

            public ReplyClickEventArgs(int topicID, int postID, int currentPage)
            {
                this.topicID = topicID;
                this.postID = postID;
                this.currentPage = currentPage;
            }

            public int TopicID
            {
                get { return topicID; }
            }

            public int PostID
            {
                get { return postID; }
            }

            public int CurrentPage
            {
                get { return currentPage; }
            }
        }

        #endregion

        protected void cbWarn_CheckedChanged(object sender, EventArgs e)
        {
            txtWarnReason.Visible = cbWarn.Checked;
        }

        protected void cbKickMember_CheckedChanged(object sender, EventArgs e)
        {
            pnlWarn.Visible = !cbKickMember.Checked && !cbBanMember.Checked;
        }

        protected void cbBanMember_CheckedChanged(object sender, EventArgs e)
        {
            pnlWarn.Visible = !cbKickMember.Checked && !cbBanMember.Checked;
        }

        private void DeleteNewGroupTopicEvent(string username, int topicID)
        {
            Event[] events = Event.Fetch(username, (ulong)Event.eType.NewGroupTopic, 1000);

            foreach (Event ev in events)
            {
                NewGroupTopic newGroupTopic = Misc.FromXml<NewGroupTopic>(ev.DetailsXML);
                if (newGroupTopic.GroupTopicID == topicID)
                {
                    Event.Delete(ev.ID);
                    break;
                }
            }
        }
    }
}