using System;
using ezFixUp.Components.Groups;
using ezFixUp.Classes;
using EditPost = ezFixUp.Components.Groups.EditPost;
using ViewPosts = ezFixUp.Components.Groups.ViewPosts;

namespace ezFixUp
{
    public partial class ShowGroupTopics : PageBase
    {
        public ShowGroupTopics()
        {
            RequiresAuthorization = false;
        }

        /// <summary>
        /// Gets the 'gid' parameter from Query String.
        /// It returns -1 if the parameter is not set in ViewState.
        /// </summary>
        /// <value>The group ID.</value>
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int) ViewState["CurrentGroupId"];
                }
                return -1;
            }
            set { ViewState["CurrentGroupId"] = value; }
        }

        /// <summary>
        /// Gets or sets the viewed topic ID from to ViewState.
        /// It returns -1 if there are no data in ViewState.
        /// </summary>
        /// <value>The viewed topic ID.</value>
        public int ViewedTopicID
        {
            get
            {
                if (ViewState["ViewedTopicID"] != null)
                {
                    return (int) ViewState["ViewedTopicID"];
                }
                return -1;
            }
            set { ViewState["ViewedTopicID"] = value; }
        }

        private Group _group;
        public Group Group
        {
            get
            {
                if (_group == null)
                    _group = Group.Fetch(GroupID);

                return _group;
            }
        }

        public GroupMember CurrentGroupMember
        {
            get
            {
                if (ViewState["CurrentGroupMember_IsNull"] != null &&
                    (bool) ViewState["CurrentGroupMember_IsNull"])
                {
                    return null;
                }
                if (ViewState["CurrentGroupMember"] == null)
                {
                    if (CurrentUserSession != null)
                    {
                        GroupMember groupMember = GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                        if (groupMember == null)
                        {
                            ViewState["CurrentGroupMember_IsNull"] = true;
                        }
                        else
                        {
                            ViewState["CurrentGroupMember"] = groupMember;
                            ViewState["CurrentGroupMember_IsNull"] = false;
                        }
                    }
                }

                return ViewState["CurrentGroupMember"] as GroupMember;
            }

            set
            {
                ViewState["CurrentGroupMember_IsNull"] = value == null;
                ViewState["CurrentGroupMember"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EditPost1.ViewPosts += EditPost1_ViewPosts;
            EditPost1.UpdatePost += EditPost1_UpdatePost;
            EditPost1.UpdateTopic += EditPost1_UpdateTopic;
            EditPost1.CancelStartNewTopicClick += EditPost1_CancelStartNewTopicClick;
            EditPost1.CancelUpdateTopic += EditPost1_CancelUpdateTopic;
            EditPost1.CancelNewPostClick += EditPost1_CancelNewPostClick;
            EditPost1.CancelUpdatePostClick += EditPost1_CancelUpdatePostClick;
            ViewPosts1.ReplyClick += ViewPosts1_ReplyClick;
            ViewPosts1.EditPostClick += ViewPosts1_EditPostClick;
            ViewTopics1.SearchTopicClick += ViewTopics1_SearchTopicClick;
            SearchTopicResults1.SearchTopicClick += SearchTopicResults1_SearchTopicClick;

            #region Show AddThis link

            if (Config.ThirdPartyServices.UseAddThis)
            {
                divAddThis.Visible = true;
                divAddThis.InnerHtml = Config.ThirdPartyServices.AddThisCode;
            }

            #endregion

            if (!IsPostBack)
            {
                if (CurrentUserSession == null && Config.Groups.OnlyRegisteredUsersCanBrowseGroups)
                    Response.Redirect("~/LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

                if (CurrentUserSession != null)
                {
                    var permissionCheckResult = CurrentUserSession.CanBrowseGroups();

                    if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                    {
                        Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanBrowseGroups;
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

                int groupID;
                if (Int32.TryParse((Request.Params["gid"]), out groupID))
                {
                    GroupID = groupID;

                    if (Group == null)
                    {
                        ((PageBase) Page).StatusPageMessage = Lang.Trans("The Group no longer exists!");
                        Response.Redirect("~/ShowStatus.aspx");
                    }

                    ViewTopics1.GroupID = groupID;
                    ViewPosts1.GroupID = groupID;
                    ViewPosts1.TopicID = -1;
                    EditPost1.GroupID = groupID;
                    SearchTopicResults1.GroupID = groupID;

                    lnkOpenAjaxChat.NavigateUrl = String.Format("{0}/AjaxChat/ChatWindow.aspx?id={1}",
                                                                Config.Urls.Home, groupID);
                    lnkOpenAjaxChat.Target = String.Format("ajaxchat{0}", groupID);

                    if (!Group.Approved &&
                        (CurrentUserSession == null ||
                         (CurrentUserSession.Username != Group.Owner && !CurrentUserSession.IsAdmin())))
                    {
                        StatusPageMessage = Lang.Trans("This group is not approved yet.");

                        Response.Redirect("~/ShowStatus.aspx");
                        return;
                    }
                }
                else
                {
                    ViewTopics1.GroupID = -1;
                    ViewPosts1.GroupID = -1;
                    ViewPosts1.TopicID = -1;
                    EditPost1.GroupID = -1;
                    SearchTopicResults1.GroupID = -1;

                    return;
                }

                if (
                    !GroupMember.HasPermission(CurrentUserSession, CurrentGroupMember, Group,
                                               eGroupPermissionType.ViewMessageBoard))
                {
                    StatusPageMessage = Lang.Trans("You are not authorized to view this message board.");

                    Response.Redirect("~/ShowStatus.aspx");
                    return;
                }

                loadStrings();
                mvTopics.SetActiveView(viewMain);
                enableMenuLinks();
                lnkMessageBoard.Enabled = false;

                int topicID;
                if (Int32.TryParse((Request.Params["tid"]), out topicID))
                {
                    ViewPosts1.GroupID = GroupID;
                    ViewPosts1.TopicID = topicID;
                    EditPost1.TopicID = topicID;
                    ViewedTopicID = topicID;

                    if (CurrentUserSession != null)
                    {
                        GroupTopic currentTopic = GroupTopic.Fetch(ViewedTopicID);

                        GroupTopicSubscription groupTopicSubscription =
                            GroupTopicSubscription.Fetch(CurrentUserSession.Username, ViewedTopicID, GroupID);

                        if (currentTopic != null)
                        {
                            currentTopic.Views++;
                            currentTopic.Save();

                            if (groupTopicSubscription != null)
                            {
                                groupTopicSubscription.DateUpdated = currentTopic.DateUpdated;
                                groupTopicSubscription.Save();
                            }
                        }
                    }

                    mvTopics.SetActiveView(viewPosts);
                    enableMenuLinks();
                }

                if (Global.GetSessionState().ContainsKey("SearchNewTopics"))
                {
                    ViewTopics1.TxtTopicToSearch = Global.GetSessionState()["SearchNewTopics"] as string;
                    ViewTopics1.CbSearchInPosts = (bool) Global.GetSessionState()["SearchNewTopicsInPosts"];
                    ViewTopics1.btnSearch_Click(null, null);

                    Global.GetSessionState().Remove("SearchNewTopics");
                    Global.GetSessionState().Remove("SearchNewTopicsInPosts");
                }
            }
        }

        protected void SearchTopicResults1_SearchTopicClick(object sender, SearchTopicEventArgs eventArgs)
        {
            ViewTopics1_SearchTopicClick(sender, eventArgs);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            setMenu();
        }

        private void setMenu()
        {
            Group group = Group;

            if (group == null)
                Response.Redirect("Groups.aspx");

            if (CurrentUserSession == null) // is not logged in
            {
                pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);

                pnlAjaxChat.Visible = false;
                pnlStartNewTopic.Visible = false;
                pnlSubscribeForTopic.Visible = false;
                pnlUnsubscribeFromTopic.Visible = false;
                pnlEditTopic.Visible = false;
                pnlDeleteTopic.Visible = false;
                pnlAddNewPost.Visible = false;
            }
            else if (CurrentGroupMember == null) // is not member
            {
                pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);

                pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                       (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                        (CurrentUserSession.Level != null &&
                                         CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                       group.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers));

                pnlStartNewTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers) &&
                                           !ViewPosts1.Visible;
                pnlSubscribeForTopic.Visible =
                    group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers) &&
                    ViewPosts1.Visible &&
                    (ViewedTopicID != -1 &&
                     !GroupTopicSubscription.IsSubscribed(CurrentUserSession.Username,
                                                          ViewedTopicID));

                pnlUnsubscribeFromTopic.Visible = !pnlSubscribeForTopic.Visible && ViewPosts1.Visible;

                pnlEditTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers) &&
                                       ViewPosts1.Visible &&
                                       (!ViewPosts1.CurrentTopic.Locked &&
                                        CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);

                pnlDeleteTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers) &&
                                         ViewPosts1.Visible &&
                                         (CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);

                pnlAddNewPost.Visible = group.IsPermissionEnabled(eGroupPermissions.AddPostNonMembers) &&
                                        ViewPosts1.Visible &&
                                        (!ViewPosts1.CurrentTopic.Locked);
            }
            else if (!CurrentGroupMember.Active) // is not active member
            {
                pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);

                pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                       (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                        (CurrentUserSession.Level != null &&
                                         CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                       group.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers));

                pnlStartNewTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers) &&
                                           !ViewPosts1.Visible;

                pnlEditTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers) &&
                                       ViewPosts1.Visible &&
                                       (!ViewPosts1.CurrentTopic.Locked &&
                                        CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);

                pnlDeleteTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers) &&
                                         ViewPosts1.Visible &&
                                         (CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);

                pnlAddNewPost.Visible = group.IsPermissionEnabled(eGroupPermissions.AddPostNonMembers) &&
                                        ViewPosts1.Visible &&
                                        (!ViewPosts1.CurrentTopic.Locked);

                pnlSubscribeForTopic.Visible =
                    group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers) &&
                    ViewPosts1.Visible &&
                    (ViewedTopicID != -1 &&
                     !GroupTopicSubscription.IsSubscribed(CurrentUserSession.Username,
                                                          ViewedTopicID));
                pnlUnsubscribeFromTopic.Visible = !pnlSubscribeForTopic.Visible && ViewPosts1.Visible;
            }
            else // is member
            {
                if (CurrentGroupMember.Type == GroupMember.eType.Member)
                {
                    pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryMembers);
                    pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersMembers);
                    pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardMembers);
                    pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsMembers);

                    pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                           (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                            (CurrentUserSession.Level != null &&
                                             CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                           group.IsPermissionEnabled(eGroupPermissions.UseChatMembers));

                    pnlStartNewTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicMembers) &&
                                               !ViewPosts1.Visible;
                    pnlEditTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicMembers) &&
                                           ViewPosts1.Visible &&
                                           (!ViewPosts1.CurrentTopic.Locked &&
                                            CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);
                    pnlDeleteTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicMembers) &&
                                             ViewPosts1.Visible &&
                                             (CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);
                    pnlAddNewPost.Visible = group.IsPermissionEnabled(eGroupPermissions.AddPostMembers) &&
                                            ViewPosts1.Visible &&
                                            (!ViewPosts1.CurrentTopic.Locked);
                    pnlSubscribeForTopic.Visible =
                        group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardMembers) &&
                        ViewPosts1.Visible &&
                        (ViewedTopicID != -1 &&
                         !GroupTopicSubscription.IsSubscribed(CurrentUserSession.Username,
                                                              ViewedTopicID));
                    pnlUnsubscribeFromTopic.Visible = !pnlSubscribeForTopic.Visible && ViewPosts1.Visible;
                }
                else if (CurrentGroupMember.Type == GroupMember.eType.VIP)
                {
                    pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryVip);
                    pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersVip);
                    pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardVip);
                    pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsVip);
                    pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                           (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                            (CurrentUserSession.Level != null &&
                                             CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                           group.IsPermissionEnabled(eGroupPermissions.UseChatVip));

                    pnlStartNewTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicVip) &&
                                               !ViewPosts1.Visible;
                    pnlEditTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicVip) &&
                                           ViewPosts1.Visible &&
                                           (!ViewPosts1.CurrentTopic.Locked &&
                                            CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);
                    pnlDeleteTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.AddTopicVip) &&
                                             ViewPosts1.Visible &&
                                             (CurrentUserSession.Username == ViewPosts1.CurrentTopic.Username);
                    pnlAddNewPost.Visible = group.IsPermissionEnabled(eGroupPermissions.AddPostVip) &&
                                            ViewPosts1.Visible &&
                                            (!ViewPosts1.CurrentTopic.Locked);
                    pnlSubscribeForTopic.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardVip) &&
                                                   ViewPosts1.Visible &&
                                                   (ViewedTopicID != -1 &&
                                                    !GroupTopicSubscription.IsSubscribed(CurrentUserSession.Username,
                                                                                         ViewedTopicID));
                    pnlUnsubscribeFromTopic.Visible = !pnlSubscribeForTopic.Visible && ViewPosts1.Visible;
                }
                else if (CurrentGroupMember.Type == GroupMember.eType.Admin ||
                         CurrentGroupMember.Type == GroupMember.eType.Moderator)
                {
                    pnlGroupGallery.Visible = true;
                    pnlGroupMembers.Visible = true;
                    pnlMessageBoard.Visible = true;
                    pnlGroupEvents.Visible = true;
                    pnlAjaxChat.Visible = Config.Groups.EnableAjaxChat &&
                                          (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                           (CurrentUserSession.Level != null &&
                                            CurrentUserSession.Level.Restrictions.UserCanUseChat));
                    pnlStartNewTopic.Visible = true;
                    pnlEditTopic.Visible = ViewPosts1.Visible;
                    pnlDeleteTopic.Visible = ViewPosts1.Visible;
                    pnlAddNewPost.Visible = ViewPosts1.Visible;
                    pnlSubscribeForTopic.Visible =
                        !GroupTopicSubscription.IsSubscribed(CurrentUserSession.Username, ViewedTopicID) &&
                        ViewPosts1.Visible;
                    pnlUnsubscribeFromTopic.Visible = !pnlSubscribeForTopic.Visible && ViewPosts1.Visible;
                }
            }

            if (CurrentUserSession != null && CurrentUserSession.IsAdmin())
            {
                pnlGroupGallery.Visible = true;
                pnlGroupMembers.Visible = true;
                pnlMessageBoard.Visible = true;
                pnlGroupEvents.Visible = true;
                pnlAjaxChat.Visible = Group.Approved && (Config.Groups.EnableAjaxChat &&
                                                         (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                                          (CurrentUserSession.Level != null &&
                                                           CurrentUserSession.Level.Restrictions.UserCanUseChat)));
                pnlStartNewTopic.Visible = !ViewPosts1.Visible;
                pnlEditTopic.Visible = ViewPosts1.Visible;
                pnlDeleteTopic.Visible = ViewPosts1.Visible;
                pnlAddNewPost.Visible = ViewPosts1.Visible;
                pnlSubscribeForTopic.Visible =
                    !GroupTopicSubscription.IsSubscribed(CurrentUserSession.Username, ViewedTopicID) &&
                    ViewPosts1.Visible;
                pnlUnsubscribeFromTopic.Visible = !pnlSubscribeForTopic.Visible && ViewPosts1.Visible;
            }

            Page.Items.Add("ShowChoices", pnlAddNewPost.Visible);

            pnlNavLine.Visible = lnkStartNewTopic.Visible || lnkSubscribeForTopic.Visible ||
                                 lnkUnsubscribeFromTopic.Visible || lnkAddNewPost.Visible || lnkEditTopic.Visible ||
                                 lnkDeleteTopic.Visible;
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            lnkGroupHome.Text = Lang.Trans("Group Home");
            lnkGroupGallery.Text = Lang.Trans("Group Gallery");
            lnkGroupMembers.Text = Lang.Trans("Group Members");
            lnkMessageBoard.Text = Lang.Trans("Message Board");
            lnkGroupEvents.Text = Lang.Trans("Group Events");
            lnkOpenAjaxChat.Text = Lang.Trans("Start Group Chat");
            lnkStartNewTopic.Text = Lang.Trans("Start New Topic");
            lnkSubscribeForTopic.Text = Lang.Trans("Subscribe for this topic");
            lnkUnsubscribeFromTopic.Text = Lang.Trans("Unsubscribe from this topic");
            lnkEditTopic.Text = Lang.Trans("Edit Topic");
            lnkAddNewPost.Text = Lang.Trans("Add New Post");
            lnkDeleteTopic.Text = Lang.Trans("Delete Topic");
            lnkBrowseGroups.Text = Lang.Trans("Back to Groups");

            lnkDeleteTopic.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                                   Lang.Trans(
                                                                       "Are you sure you want to delete this topic?")));
        }

        protected void lnkStartNewTopic_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null && !CurrentUserSession.IsAdmin())
            {
                if (!canStartNewTopic(CurrentUserSession.Username) &&
                    CurrentGroupMember.Type == GroupMember.eType.Member)
                {
                    mvTopics.SetActiveView(viewMain);
                    ViewTopics1.ShowMessage(Misc.MessageType.Error, Lang.Trans("You have reached your topics limit."));
                    return;
                }
            }

            EditPost1.Type = EditPost.eType.AddTopic;

            mvTopics.SetActiveView(viewStartNewTopic);
            enableMenuLinks();
            lnkStartNewTopic.Enabled = false;
        }

        protected void lnkMessageBoard_Click(object sender, EventArgs e)
        {
            Page.Header.Title = Config.Misc.SiteTitle;
            ViewTopics1.TxtTopicToSearch = "";
            ViewTopics1.CbSearchInPosts = false;
            mvTopics.SetActiveView(viewMain);
            enableMenuLinks();
            lnkMessageBoard.Enabled = false;
        }

        private void enableMenuLinks()
        {
            lnkGroupMembers.Enabled = true;
            lnkGroupGallery.Enabled = true;
            lnkOpenAjaxChat.Enabled = true;
            lnkMessageBoard.Enabled = true;
            lnkStartNewTopic.Enabled = true;
            lnkSubscribeForTopic.Enabled = true;
            lnkUnsubscribeFromTopic.Enabled = true;
            lnkEditTopic.Enabled = true;
            lnkDeleteTopic.Enabled = true;
            lnkAddNewPost.Enabled = true;
        }

        protected void lnkGroupHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupUrl(GroupID.ToString()));
        }

        private void DeleteTopicAndEvents(int topicID, bool isModerator, bool isAdmin)
        {
            if (isAdmin ||
                ((isModerator || ViewPosts1.CurrentTopic.Username == CurrentUserSession.Username) &&
                ViewPosts1.CurrentTopic.Posts <= Config.Groups.MaxPostsToDeleteTopic)
               )
            {
                GroupTopic topic = GroupTopic.Fetch(topicID);

                if (topic != null)
                {
                    Event[] events = Event.Fetch(topic.Username, (ulong) Event.eType.NewGroupTopic, 1000);

                    foreach (Event e in events)
                    {
                        NewGroupTopic newGroupTopic = Misc.FromXml<NewGroupTopic>(e.DetailsXML);
                        if (newGroupTopic.GroupTopicID == topicID)
                        {
                            Event.Delete(e.ID);
                            break;
                        }
                    }
                    
                    GroupTopic.Delete(topicID);

                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.DeletedTopic, "DeletedTopic");
                }

                enableMenuLinks();
                lnkMessageBoard.Enabled = false;
                mvTopics.SetActiveView(viewMain);
            }
            else
            {
                ViewPosts1.ShowMessage(Misc.MessageType.Error,
                                       Lang.Trans(
                                           String.Format(
                                               "You can't delete this topic because it has more than {0} posts",
                                               Config.Groups.MaxPostsToDeleteTopic)));
            }
        }

        protected void lnkDeleteTopic_Click(object sender, EventArgs e)
        {
            int topicID;
            if (Int32.TryParse(Request.Params["tid"], out topicID))
            {
                if (CurrentUserSession != null)
                {
                    if (!CurrentUserSession.IsAdmin())
                    {
                        if (CurrentGroupMember == null || !CurrentGroupMember.Active) // is not a member
                        {
                            if (Group.IsPermissionEnabled(eGroupPermissions.AddTopicNonMembers))
                            {
                                DeleteTopicAndEvents(topicID, false, false);
                            }
                        }
                        else // is a member
                        {
                            if (CurrentGroupMember.Type == GroupMember.eType.Member)
                            {
                                if (Group.IsPermissionEnabled(eGroupPermissions.AddTopicMembers))
                                {
                                    DeleteTopicAndEvents(topicID, false, false);
                                }
                            }
                            else if (CurrentGroupMember.Type == GroupMember.eType.VIP)
                            {
                                if (Group.IsPermissionEnabled(eGroupPermissions.AddTopicVip))
                                {
                                    DeleteTopicAndEvents(topicID, false, false);
                                }
                            }
                            else if (CurrentGroupMember.Type == GroupMember.eType.Admin)
                            {
                                DeleteTopicAndEvents(topicID, false, true);
                            }
                            else if (CurrentGroupMember.Type == GroupMember.eType.Moderator)
                            {
                                DeleteTopicAndEvents(topicID, true, false);
                            }
                        }
                    }
                    else // super admin can delete topic regardless of restriction "max posts to delete topic"
                    {
                        DeleteTopicAndEvents(topicID, false, true);
                    }
                }
            }
        }

        protected void lnkAddNewPost_Click(object sender, EventArgs e)
        {
            EditPost1.Type = EditPost.eType.AddPost;

            mvTopics.SetActiveView(viewStartNewTopic);
            enableMenuLinks();
            lnkAddNewPost.Enabled = false;
        }

        protected void lnkMyGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=mg");
        }

        protected void lnkEditTopic_Click(object sender, EventArgs e)
        {
            EditPost1.TopicID = ViewedTopicID;
            EditPost1.Type = EditPost.eType.EditTopic;
            EditPost1.LoadTopic = true;

            mvTopics.SetActiveView(viewStartNewTopic);
            enableMenuLinks();
            lnkEditTopic.Enabled = false;
        }

        protected void EditPost1_CancelUpdatePostClick(object sender, EventArgs e)
        {
            mvTopics.SetActiveView(viewPosts);
            enableMenuLinks();
        }

        protected void EditPost1_CancelNewPostClick(object sender, EventArgs e)
        {
            mvTopics.SetActiveView(viewPosts);
            enableMenuLinks();
        }

        protected void EditPost1_CancelStartNewTopicClick(object sender, EventArgs e)
        {
            mvTopics.SetActiveView(viewMain);
            enableMenuLinks();
            lnkMessageBoard.Enabled = false;
        }

        protected void EditPost1_ViewPosts(object sender, EventArgs e)
        {
            ViewPosts1.PageNumber = -1; // show last page

            mvTopics.SetActiveView(viewPosts);
            enableMenuLinks();
        }

        protected void EditPost1_UpdatePost(object sender, UpdatePostEventArgs e)
        {
            ViewPosts1.PageNumber = e.CurrentPage;

            mvTopics.SetActiveView(viewPosts);
            enableMenuLinks();
        }

        protected void ViewPosts1_ReplyClick(object sender, ViewPosts.ReplyClickEventArgs eventArgs)
        {
            GroupPost groupPost = GroupPost.Fetch(eventArgs.PostID);

            if (groupPost != null)
            {
                EditPost1.Type = EditPost.eType.AddPost;
                EditPost1.Post = String.Format("[quote=\"{0}\"]{1}[/quote]\n", groupPost.Username, groupPost.Post);

                mvTopics.SetActiveView(viewStartNewTopic);
                enableMenuLinks();
                lnkAddNewPost.Enabled = false;
            }
        }

        protected void ViewPosts1_EditPostClick(object sender, ViewPosts.EditPostClickEventArgs eventArgs)
        {
            GroupPost groupPost = GroupPost.Fetch(eventArgs.PostID);

            if (groupPost != null)
            {
                EditPost1.Type = EditPost.eType.EditPost;

                EditPost1.Post = groupPost.Post;
                EditPost1.PostID = eventArgs.PostID;
                EditPost1.PageNumber = eventArgs.CurrentPage;

                mvTopics.SetActiveView(viewStartNewTopic);
                enableMenuLinks();
                lnkAddNewPost.Enabled = false;
            }
        }

        protected void EditPost1_UpdateTopic(object sender, UpdateTopicEventArgs e)
        {
            ViewTopics1.Results = null; // populate new topics

            enableMenuLinks();
            lnkMessageBoard.Enabled = false;

            mvTopics.SetActiveView(viewMain);
        }

        protected void EditPost1_CancelUpdateTopic(object sender, EventArgs e)
        {
            enableMenuLinks();
            mvTopics.SetActiveView(viewPosts);
        }

        protected void ViewTopics1_SearchTopicClick(object sender, SearchTopicEventArgs eventArgs)
        {
            var results = new GroupTopicSearchResults {GroupTopics = eventArgs.TopicIDs};
            SearchTopicResults1.Results = results;

            enableMenuLinks();

            mvTopics.SetActiveView(viewSearchResults);
        }

        private bool canStartNewTopic(string username)
        {
            int topicsForLast24Hours = 0;

            GroupTopic[] groupTopics = GroupTopic.Fetch(GroupID, username);

            if (groupTopics.Length == 0)
            {
                return true;
            }
            foreach (GroupTopic groupTopic in groupTopics)
            {
                if (groupTopic.DateCreated >= DateTime.Now.AddHours(-24))
                {
                    topicsForLast24Hours++;
                }
            }

            if (topicsForLast24Hours >= Config.Groups.MaxTopicsPerGroupForDay)
            {
                return false;
            }

            GroupTopic[] allGroupTopics = GroupTopic.Fetch(username);

            int topicsForAllGroupsForLast24Hours = 0;

            if (allGroupTopics.Length == 0)
            {
                return true;
            }
            foreach (GroupTopic groupTopic in allGroupTopics)
            {
                if (groupTopic.DateCreated >= DateTime.Now.AddHours(-24))
                {
                    topicsForAllGroupsForLast24Hours++;
                }
            }

            return topicsForAllGroupsForLast24Hours < Config.Groups.MaxTopicsForGroupsForDay;
        }

        protected void lnkGroupGallery_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupPhotosUrl(GroupID.ToString()));
        }

        protected void lnkGroupMembers_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupUrl(GroupID.ToString(), "gmembers"));
        }

        protected void lnkBrowseGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx");
        }

        protected void lnkSubscribeForTopic_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null && ViewedTopicID != -1)
            {
                GroupTopic groupTopic = GroupTopic.Fetch(ViewedTopicID);

                if (groupTopic != null)
                {
                    var groupTopicSubscription = new GroupTopicSubscription(CurrentUserSession.Username, ViewedTopicID,
                                                                            GroupID)
                                                     {DateUpdated = groupTopic.DateUpdated};
                    groupTopicSubscription.Save();
                }
            }
        }

        protected void lnkUnsubscribeFromTopic_Click(object sender, EventArgs e)
        {
            int topicID;

            if (CurrentUserSession != null && Int32.TryParse((Request.Params["tid"]), out topicID))
            {
                GroupTopicSubscription groupTopicSubscription =
                    GroupTopicSubscription.Fetch(CurrentUserSession.Username, topicID, GroupID);

                if (groupTopicSubscription != null)
                {
                    GroupTopicSubscription.Delete(groupTopicSubscription.ID);
                }
            }
        }

        protected void lnkGroupEvents_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupEventsUrl(GroupID.ToString()));
        }
    }
}