using System;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class ShowGroup : PageBase
    {
        public global::ezFixUp.Components.Groups.ViewGroup ViewGroup1;
        private delegate string Parser(string s);

        public ShowGroup()
        {
            RequiresAuthorization = false;
        }

        /// <summary>
        /// Gets the 'CurrentGroupId' from View State.
        /// It returns -1 if it is null.
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
                else
                {
                    return -1;
                }
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        /// <summary>
        /// Gets the group from DB and saves it in 'ViewState'.
        /// If the group doesn't exist it returns NULL.
        /// </summary>
        /// <value>The group.</value>
        public Group Group
        {
            get
            {
                if (ViewState["CurrentGroup"] == null)
                {
                    ViewState["CurrentGroup"] = Group.Fetch(GroupID);
                }

                return ViewState["CurrentGroup"] as Group;
            }
        }

        /// <summary>
        /// Gets the current group member. If the current user is not member for this group it returns NULL.
        /// </summary>
        /// <value>The current group member.</value>
        public GroupMember CurrentGroupMember
        {
            get
            {
                if (ViewState["CurrentGroupMember_IsNull"] != null &&
                    (bool) ViewState["CurrentGroupMember_IsNull"])
                {
                    return null;
                }
                else if (ViewState["CurrentGroupMember"] == null)
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
                if (value == null)
                {
                    ViewState["CurrentGroupMember_IsNull"] = true;
                }
                else
                {
                    ViewState["CurrentGroupMember_IsNull"] = false;
                }
                ViewState["CurrentGroupMember"] = value;
            }
        }

        public void ViewGroup1_MoreClick(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewGroupMembers);
            enableMenuLinks();
            lnkGroupMembers.Enabled = false;
        }

        protected void NewTopics1_ViewAllTopicsClick(object sender, EventArgs e)
        {
            lnkMessageBoard_Click(null, null);
        }

        protected void NewPhotos1_MoreClick(object sender, EventArgs e)
        {
            lnkGroupGallery_Click(null, null);
        }

        protected void ViewGroup1_MemberClickEvent(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewGroupMembers);
            enableMenuLinks();
            lnkGroupMembers.Enabled = false;
        }

        public void ViewGroup1_JoinPanelOpenEvent(object sender, EventArgs e)
        {
            enableMenuLinks();
            lnkJoinGroup.Enabled = false;
        }

        public void ViewGroup1_JoinPanelCloseEvent(object sender, EventArgs e)
        {
            enableMenuLinks();
            lnkGroupHome.Enabled = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ViewGroup1.NewMembers1.MoreClickEvent += new EventHandler(ViewGroup1_MoreClick);
            ViewGroup1.NewTopics1.ViewAllTopicsClick += new EventHandler(NewTopics1_ViewAllTopicsClick);
            ViewGroup1.NewPhotos1.MoreClick += new EventHandler(NewPhotos1_MoreClick);
            ViewGroup1.MemberClickEvent += new EventHandler(ViewGroup1_MemberClickEvent);
            ViewGroup1.JoinPanelOpenEvent += new EventHandler(ViewGroup1_JoinPanelOpenEvent);
            ViewGroup1.JoinPanelCloseEvent += new EventHandler(ViewGroup1_JoinPanelCloseEvent);

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
                if (!Int32.TryParse((Request.Params["id"]), out groupID))
                {
                    return;
                }
                else
                {
                    GroupID = groupID;
                    ViewGroup1.GroupID = groupID;
                    PendingMembers1.GroupID = groupID;
                    GroupMembers1.GroupID = groupID;
                    EditGroup1.GroupID = groupID;
                    GroupBans1.GroupID = groupID;
                    InviteFriends1.GroupID = groupID;
                    SendAnnoucement1.GroupID = groupID;
                    lnkOpenAjaxChat.NavigateUrl = String.Format("{0}/AjaxChat/ChatWindow.aspx?id={1}",
                                                                Config.Urls.Home, groupID);
                    lnkOpenAjaxChat.Target = String.Format("ajaxchat{0}", groupID);

                    if (Group == null) return;

                    if (Group.MinAge.HasValue && 
                        (CurrentUserSession == null || CurrentUserSession.Age < Group.MinAge && CurrentUserSession.Username != Group.Owner))
                    {
                        if (CurrentUserSession!= null && CurrentGroupMember != null)
                        {
                            GroupMember.Delete(Group.ID, CurrentUserSession.Username);
                            Group.ActiveMembers--;
                            Group.Save();
                        }

                        StatusPageMessage = CurrentUserSession == null
                                                ? Lang.Trans("You are not authorized to view this group.")
                                                : Lang.Trans("This group is age restricted.");

                        Response.Redirect("~/ShowStatus.aspx");
                        return;
                    }

                    if (!Group.Approved && (CurrentUserSession == null || (CurrentUserSession.Username != Group.Owner && !CurrentUserSession.IsAdmin())))
                    {
                        StatusPageMessage = Lang.Trans("This group is not approved yet.");

                        Response.Redirect("~/ShowStatus.aspx");
                        return;
                    }

                    if (!GroupMember.HasPermission(CurrentUserSession, CurrentGroupMember, Group, eGroupPermissionType.ViewGroup))
                    {
                        if (CurrentUserSession != null && CurrentGroupMember == null)
                        {
                            loadStrings();
                            hideMenuLinks();
                            lblJoinToGroupMessage.Text = Lang.Trans("You must be a member of this group in order to view it.");
                            lblJoinToGroupMessage.CssClass = "message";
                            pnlBrowseGroups.Visible = true;
                            mvGroup.SetActiveView(viewJoinGroup);
                            return;
                        }
                        else if (CurrentGroupMember != null)
                        {
                            // current group member should can leave current group even he can't view it
                            lblError.Text = Lang.Trans("You are not authorized to view this group.");
                            ViewGroup1.Visible = false;
                        }
                        else
                        {
                            StatusPageMessage = Lang.Trans("You are not authorized to view this group.");

                            Response.Redirect("~/ShowStatus.aspx");
                            return;
                        }
                    }
                }

                if (Request.Params["show"] != null && Request.Params["show"] == "gmembers")
                {
                    lnkGroupMembers_Click(null, null);
                    loadStrings();
                    return;
                }

                loadStrings();
                enableMenuLinks();
                lnkGroupHome.Enabled = false;
                mvGroup.SetActiveView(viewGroupHome);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            setMenu();
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Join Group");
            SmallBoxStart1.Title = Lang.Trans("Actions");
            lnkGroupHome.Text = Lang.Trans("Group Home");
            lnkGroupGallery.Text = Lang.Trans("Group Gallery");
            lnkGroupMembers.Text = Lang.Trans("Group Members");
            lnkGroupEvents.Text = Lang.Trans("Group Events");
            lnkInviteFriends.Text = Lang.Trans("Invite Friends");
            lnkMessageBoard.Text = Lang.Trans("Message Board");
            lnkManageGroup.Text = Lang.Trans("Manage Group");
            lnkGroupBans.Text = Lang.Trans("Banned Group Members");
            lnkSendAnnouncement.Text = Lang.Trans("Send Announcement");
            lnkPendingMembers.Text = Lang.Trans("Pending Members");
            lnkBrowseGroups.Text = Lang.Trans("Back to Groups");
            lnkJoinGroup.Text = Lang.Trans("Join Group");
            lnkLeaveGroup.Text = Lang.Trans("Leave Group");
            lnkOpenAjaxChat.Text = Lang.Trans("Start Group Chat");
            btnJoinToGroup.Text = Lang.Trans("Join Group");

            lnkLeaveGroup.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                    Lang.Trans("Are you sure you want to leave this group?")));

            string accessLevel = String.Empty;
            switch (Group.AccessLevel)
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

            string numberOfMembers = String.Empty;
            numberOfMembers =
                GroupMember.HasPermission(CurrentUserSession, CurrentGroupMember, Group,
                                          eGroupPermissionType.ViewMembers)
                    ? Group.ActiveMembers.ToString()
                    : String.Empty;

            Parser parse = delegate(string text)
                               {
                                   string result =
                                       text.Replace("%%NAME%%", Group.Name)
                                       .Replace("%%CATEGORIES%%",Group.GetCategoriesString(GroupID))
                                       .Replace("%%DATECREATED%%", Group.DateCreated.ToShortDateString())
                                       .Replace("%%TYPE%%", accessLevel)
                                       .Replace("%%MEMBERS%%", numberOfMembers)
                                       .Replace("%%OWNER%%", Group.Owner);

                                   return result;
                               };

            Header.Title = Config.SEO.ShowGroupTitleTemplate.Length > 0
                               ? parse(Config.SEO.ShowGroupTitleTemplate)
                               : Config.SEO.DefaultTitleTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle);

            HtmlMeta metaDesc = new HtmlMeta();
            metaDesc.ID = "Description";
            metaDesc.Name = "description";
            metaDesc.Content = Config.SEO.ShowGroupMetaDescriptionTemplate.Length > 0
                                   ? parse(Config.SEO.ShowGroupMetaDescriptionTemplate)
                                   : Config.SEO.DefaultMetaDescriptionTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle);
            Header.Controls.Add(metaDesc);

            HtmlMeta metaKeywords = new HtmlMeta();
            metaKeywords.ID = "Keywords";
            metaKeywords.Name = "keywords";
            metaKeywords.Content = Config.SEO.ShowGroupMetaKeywordsTemplate.Length > 0
                                       ? parse(Config.SEO.ShowGroupMetaKeywordsTemplate)
                                       : Config.SEO.DefaultMetaKeywordsTemplate.Replace("%%NAME%%",
                                                                                        Config.Misc.SiteTitle);
            Header.Controls.Add(metaKeywords);
        }

        private void enableMenuLinks()
        {
            lnkGroupHome.Enabled = true;
            lnkGroupGallery.Enabled = true;
            lnkGroupMembers.Enabled = true;
            lnkInviteFriends.Enabled = true;
            lnkMessageBoard.Enabled = true;
            lnkManageGroup.Enabled = true;
            lnkGroupBans.Enabled = true;
            lnkSendAnnouncement.Enabled = true;
            lnkPendingMembers.Enabled = true;
            lnkBrowseGroups.Enabled = true;
            lnkJoinGroup.Enabled = true;
            lnkLeaveGroup.Enabled = true;
            lnkOpenAjaxChat.Enabled = true;
        }

        private void showMenuLinks()
        {
            pnlGroupHome.Visible = true;
            pnlGroupGallery.Visible = true;
            pnlGroupMembers.Visible = true;
            pnlInviteFriends.Visible = true;
            pnlMessageBoard.Visible = true;
            pnlGroupEvents.Visible = true;
            pnlManageGroup.Visible = true;
            pnlGroupBans.Visible = true;
            pnlSendAnnouncement.Visible = true;
            pnlPendingMembers.Visible = true;
            pnlBrowseGroups.Visible = true;
            pnlJoinGroup.Visible = true;
            pnlLeaveGroup.Visible = true;
            pnlAjaxChat.Visible = true;
        }

        private void hideMenuLinks()
        {
            pnlGroupHome.Visible = false;
            pnlGroupGallery.Visible = false;
            pnlGroupMembers.Visible = false;
            pnlInviteFriends.Visible = false;
            pnlMessageBoard.Visible = false;
            pnlGroupEvents.Visible = false;
            pnlManageGroup.Visible = false;
            pnlGroupBans.Visible = false;
            pnlSendAnnouncement.Visible = false;
            pnlPendingMembers.Visible = false;
            pnlBrowseGroups.Visible = false;
            pnlJoinGroup.Visible = false;
            pnlLeaveGroup.Visible = false;
            pnlAjaxChat.Visible = false;
        }

        private void setMenu()
        {
            if (Group == null)
            {
                Response.Redirect("~/Groups.aspx");
                return;
            }

            Group group = Group;

            if (CurrentUserSession == null) // is not logged in
            {    
                pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);

                ViewGroup1.NewPhotos1.Visible = pnlGroupGallery.Visible;
                ViewGroup1.NewMembers1.Visible = pnlGroupMembers.Visible;
                ViewGroup1.NewTopics1.Visible = pnlMessageBoard.Visible;
                ViewGroup1.NewEvents1.Visible = pnlGroupEvents.Visible;
                pnlJoinGroup.Visible = false;
                pnlLeaveGroup.Visible = false;
                pnlManageGroup.Visible = false;
                pnlGroupBans.Visible = false;
                pnlAjaxChat.Visible = false;
            }
            else if (CurrentGroupMember == null) // is not member for this group
            {
                pnlJoinGroup.Visible = lnkGroupMembers.Enabled;
                pnlLeaveGroup.Visible = false;
                pnlManageGroup.Visible = false;
                pnlGroupBans.Visible = false;
                pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
                ViewGroup1.NewPhotos1.Visible = pnlGroupGallery.Visible;
                ViewGroup1.NewMembers1.Visible = pnlGroupMembers.Visible;
                ViewGroup1.NewTopics1.Visible = pnlMessageBoard.Visible;
                ViewGroup1.NewEvents1.Visible = pnlGroupEvents.Visible;
                pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                       (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                        (CurrentUserSession.Level != null &&
                                         CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                       group.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers));
            }
            else if (!CurrentGroupMember.Active) // is not active member
            {
                pnlJoinGroup.Visible = false;
                pnlLeaveGroup.Visible = lnkGroupMembers.Enabled;
                pnlManageGroup.Visible = false;
                pnlGroupBans.Visible = false;
                pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
                ViewGroup1.NewPhotos1.Visible = pnlGroupGallery.Visible;
                ViewGroup1.NewMembers1.Visible = pnlGroupMembers.Visible;
                ViewGroup1.NewTopics1.Visible = pnlMessageBoard.Visible;
                ViewGroup1.NewEvents1.Visible = pnlGroupEvents.Visible;
                pnlAjaxChat.Visible = (Config.Groups.EnableAjaxChat &&
                                       (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                       (CurrentUserSession.Level != null &&
                                        CurrentUserSession.Level.Restrictions.UserCanUseChat)) &&
                                      group.IsPermissionEnabled(eGroupPermissions.UseChatNonMembers));
            }
            else // is member
            {
                pnlJoinGroup.Visible = false;
                pnlLeaveGroup.Visible = lnkGroupMembers.Enabled;

                if (CurrentGroupMember.Type != GroupMember.eType.Admin)
                {
                    pnlManageGroup.Visible = false;
                    pnlGroupBans.Visible = false;
                }
                else
                {
                    pnlManageGroup.Visible = lnkGroupMembers.Enabled;
                    pnlGroupBans.Visible = lnkGroupMembers.Enabled;
                }

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

                    ViewGroup1.NewPhotos1.Visible = pnlGroupGallery.Visible;
                    ViewGroup1.NewMembers1.Visible = pnlGroupMembers.Visible;
                    ViewGroup1.NewTopics1.Visible = pnlMessageBoard.Visible;
                    ViewGroup1.NewEvents1.Visible = pnlGroupEvents.Visible;
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

                    ViewGroup1.NewPhotos1.Visible = pnlGroupGallery.Visible;
                    ViewGroup1.NewMembers1.Visible = pnlGroupMembers.Visible;
                    ViewGroup1.NewTopics1.Visible = pnlMessageBoard.Visible;
                    ViewGroup1.NewEvents1.Visible = pnlGroupEvents.Visible;
                }
                else if (CurrentGroupMember.Type == GroupMember.eType.Admin || CurrentGroupMember.Type == GroupMember.eType.Moderator)
                {
                    pnlGroupGallery.Visible = true;
                    pnlGroupMembers.Visible = true;
                    pnlMessageBoard.Visible = true;
                    pnlGroupEvents.Visible = true;
                    pnlAjaxChat.Visible = Config.Groups.EnableAjaxChat &&
                                          (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                          CurrentUserSession.Level != null &&
                                          CurrentUserSession.Level.Restrictions.UserCanUseChat);
                }
            }

            if (CurrentUserSession != null && CurrentUserSession.IsAdmin())
            {
                pnlManageGroup.Visible = lnkGroupMembers.Enabled;
                pnlGroupBans.Visible = lnkGroupMembers.Enabled;
                pnlAjaxChat.Visible = Config.Groups.EnableAjaxChat &&
                                          (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                                           CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits ||
                                          CurrentUserSession.Level != null &&
                                          CurrentUserSession.Level.Restrictions.UserCanUseChat);
            }

            if (!Group.Approved)
            {
                pnlJoinGroup.Visible = false;
                pnlAjaxChat.Visible = false;
            }

            if ((Group.AccessLevel == Group.eAccessLevel.Public && !Config.Groups.EnablePublicGroupInvitation || Group.AccessLevel == Group.eAccessLevel.Moderated && !Config.Groups.EnableModeratedGroupInvitation)
                || !Group.Approved
                || CurrentGroupMember == null
                || (CurrentGroupMember.Type != GroupMember.eType.Admin && CurrentGroupMember.Type != GroupMember.eType.Moderator))
            {
                pnlInviteFriends.Visible = false;
            }
            else
            {
                pnlInviteFriends.Visible = true;
            }

            if (Group.AccessLevel == Group.eAccessLevel.Moderated && Group.Approved && CurrentUserSession != null 
                && (CurrentUserSession.IsAdmin()
                 || CurrentGroupMember != null && CurrentGroupMember.Active && CurrentGroupMember.Type == GroupMember.eType.Admin))
            {
                pnlPendingMembers.Visible = true;
            }
            else
            {
                pnlPendingMembers.Visible = false;
            }

            if (Config.Groups.EnableGroupAnnouncement 
                && CurrentUserSession != null 
                && ((CurrentGroupMember != null && CurrentGroupMember.Type == GroupMember.eType.Admin) || CurrentUserSession.IsAdmin()))
            {
                pnlSendAnnouncement.Visible = true;
            }
            else
            {
                pnlSendAnnouncement.Visible = false;
            }

            pnlNavLine.Visible = pnlInviteFriends.Visible || pnlPendingMembers.Visible || pnlManageGroup.Visible ||
                                 pnlGroupBans.Visible || pnlSendAnnouncement.Visible || pnlJoinGroup.Visible ||
                                 pnlLeaveGroup.Visible;
        }

        protected void lnkJoinGroup_Click(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewGroupHome);
            ViewGroup1.btnJoinGroup_Click(sender,e);
        }

        protected void lnkLeaveGroup_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                if (Group.Owner == CurrentUserSession.Username)
                {
                    EditGroup1.ShowMessage(Misc.MessageType.Error, Lang.Trans("You are the owner of this group. Please transfer ownership of this group first."));
                    mvGroup.SetActiveView(viewManageGroup);
                    enableMenuLinks();
                    lnkManageGroup.Enabled = false;
                }
                else
                {
                    GroupMember.Delete(GroupID, CurrentUserSession.Username);

                    #region Add FriendLeftGroup Event

                    Event newEvent = new Event(CurrentUserSession.Username);

                    newEvent.Type = Event.eType.FriendLeftGroup;
                    FriendLeftGroup friendLeftGroup = new FriendLeftGroup();
                    friendLeftGroup.GroupID = GroupID;
                    newEvent.DetailsXML = Misc.ToXml(friendLeftGroup);

                    newEvent.Save();

                    string[] usernames = Classes.User.FetchMutuallyFriends(CurrentUserSession.Username);

                    foreach (string friendUsername in usernames)
                    {
                        if (Config.Users.NewEventNotification)
                        {
                            string text =
                                    String.Format("Your friend {0} has left the {1} group".Translate(),
                                                  "<b>" + CurrentUserSession.Username + "</b>",
                                                  Server.HtmlEncode(Group.Name));
                            int imageID = 0;
                            try
                            {
                                imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                            }
                            catch (NotFoundException)
                            {
                                imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                            }
                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                            Classes.User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername,
                                                                     text, thumbnailUrl,
                                                                     UrlRewrite.CreateShowGroupUrl(Group.ID.ToString()));
                        }
                    }

                    #endregion

                    if (CurrentGroupMember != null && CurrentGroupMember.Active)
                    {
                        Group.ActiveMembers--;
                        Group.Save();
                    }

                    Response.Redirect("~/Groups.aspx?show=mg");
                }    
            }
        }

        protected void lnkGroupMembers_Click(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewGroupMembers);
            enableMenuLinks();
            lnkGroupMembers.Enabled = false;

            showMenuLinks();
            pnlManageGroup.Visible = false;
            pnlGroupBans.Visible = false;
            pnlJoinGroup.Visible = false;
            pnlLeaveGroup.Visible = false;
        }

        protected void lnkGroupHome_Click(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewGroupHome);
            ViewGroup1.mvViewGroup.ActiveViewIndex = 0;
            enableMenuLinks();
            lnkGroupHome.Enabled = false;

            showMenuLinks();
        }

        protected void lnkGroupGallery_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupPhotosUrl(GroupID.ToString()));
        }

        protected void lnkPendingMembers_Click(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewPendingMembers);
            enableMenuLinks();
            lnkPendingMembers.Enabled = false;
        }

        protected void lnkManageGroup_Click(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewManageGroup);
            enableMenuLinks();
            lnkManageGroup.Enabled = false;
        }

        protected void lnkBrowseGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx");
        }

        protected void lnkMyGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=mg");
        }

        protected void lnkMessageBoard_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
        }

        protected void lnkInviteFriends_Click(object sender, EventArgs e)
        {
            InviteFriends1.ShowPnlInviteFriends();
            InviteFriends1.EmptyTextBoxes();

            mvGroup.SetActiveView(viewInviteFriends);
            enableMenuLinks();
            lnkInviteFriends.Enabled = false;
        }

        protected void btnJoinToGroup_Click(object sender, EventArgs e)
        {
            mvGroup.SetActiveView(viewGroupHome);
            ViewGroup1.btnJoinGroup_Click(sender, e);
//            Response.Redirect(UrlRewrite.CreateShowGroupUrl(GroupID.ToString()));
        }

        protected void lnkSendAnnouncement_Click(object sender, EventArgs e)
        {
            enableMenuLinks();
            lnkSendAnnouncement.Enabled = false;
            mvGroup.SetActiveView(viewSendAnnouncement);
        }

        protected void lnkGroupBans_Click(object sender, EventArgs e)
        {
            enableMenuLinks();
            lnkGroupBans.Enabled = false;
            mvGroup.SetActiveView(viewGroupBans);
        }

        protected void lnkGroupEvents_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupEventsUrl(GroupID.ToString()));
        }
    }
}
