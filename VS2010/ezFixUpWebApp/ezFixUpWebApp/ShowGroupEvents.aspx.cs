using System;
using System.Data;
using ezFixUp.Classes;
using ezFixUp.Components.Groups;

namespace ezFixUp
{
    public partial class ShowGroupEvents : PageBase
    {
        public ShowGroupEvents()
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
                    return (int)ViewState["CurrentGroupId"];
                }
                return -1;
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        public int ViewedEventID
        {
            get
            {
                if (ViewState["ViewedEventID"] != null)
                {
                    return (int)ViewState["ViewedEventID"];
                }
                else
                {
                    return -1;
                }
            }
            set
            {
                ViewState["ViewedEventID"] = value;
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
                    (bool)ViewState["CurrentGroupMember_IsNull"])
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

        protected void Page_Load(object sender, EventArgs e)
        {
            #region Show AddThis link

            if (Config.ThirdPartyServices.UseAddThis)
            {
                divAddThis.Visible = true;
                divAddThis.InnerHtml = Config.ThirdPartyServices.AddThisCode;
            }

            #endregion

            if (!IsPostBack)
            {
                int groupID;
                if (!Int32.TryParse(Request.Params["gid"], out groupID))
                {
                    return;
                }
                GroupID = groupID;
                EditEvent1.GroupID = groupID;
                ViewEvents1.GroupID = groupID;
                lnkOpenAjaxChat.NavigateUrl = String.Format("{0}/AjaxChat/ChatWindow.aspx?id={1}",
                                                            Config.Urls.Home, groupID);
                lnkOpenAjaxChat.Target = String.Format("ajaxchat{0}", groupID);
                loadStrings();
                mvGroupEvents.SetActiveView(viewEvents);
                lnkGroupEvents.Enabled = false;

                int eventID;
                if (Int32.TryParse(Request.Params["eid"], out eventID))
                {
                    ViewedEventID = eventID;
                    ViewEvent1.EventID = eventID;
                    mvGroupEvents.SetActiveView(viewEvent);
                    enableMenuLinks();
                    lnkGroupEvents.Enabled = true;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            setMenu();
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
            lnkAddEvent.Text = Lang.Trans("Add event");
            lnkEditEvent.Text = Lang.Trans("Edit event");
            lnkDeleteEvent.Text = Lang.Trans("Delete event");
            lnkBrowseGroups.Text = Lang.Trans("Back to Groups");
        }

        private void enableMenuLinks()
        {
            lnkGroupHome.Enabled = true;
            lnkGroupGallery.Enabled = true;
            lnkGroupMembers.Enabled = true;
            lnkMessageBoard.Enabled = true;
            lnkGroupEvents.Enabled = true;
            lnkOpenAjaxChat.Enabled = true;
            lnkAddEvent.Enabled = true;
            lnkDeleteEvent.Enabled = true;
            lnkEditEvent.Enabled = true;
            
        }

        private void setMenu()
        {
            Group group = Group;
            if (CurrentUserSession == null)
            {
                pnlGroupGallery.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers);
                pnlGroupMembers.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMembersNonMembers);
                pnlMessageBoard.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers);
                pnlGroupEvents.Visible = group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers);
                pnlAjaxChat.Visible = false;
                pnlAddEvent.Visible = false;
                pnlEditEvent.Visible = false;
                pnlDeleteEvent.Visible = false;
            }
            else if (CurrentGroupMember == null)
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

                pnlAddEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers) && ViewEvents1.Visible;
                pnlEditEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers) && ViewEvent1.Visible &&
                                       CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;

                pnlDeleteEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers) && ViewEvent1.Visible &&
                                         CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;
            }
            else if (!CurrentGroupMember.Active)
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

                pnlAddEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers) && ViewEvents1.Visible;
                pnlEditEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers) && ViewEvent1.Visible &&
                                       CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;

                pnlDeleteEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers) && ViewEvent1.Visible &&
                                         CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;
            }
            else
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

                    pnlAddEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventMembers) && ViewEvents1.Visible;
                    pnlEditEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventMembers) && ViewEvent1.Visible &&
                                           CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;
                    pnlDeleteEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventMembers) && ViewEvent1.Visible &&
                                             CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;
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

                    pnlAddEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventVip) && ViewEvents1.Visible;
                    pnlEditEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventVip) && ViewEvent1.Visible &&
                                           CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;
                    pnlDeleteEvent.Visible = group.IsPermissionEnabled(eGroupPermissions.AddEventVip) && ViewEvent1.Visible &&
                                             CurrentUserSession.Username == ViewEvent1.CurrentEvent.Username;
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
                    pnlAddEvent.Visible = ViewEvents1.Visible;
                    pnlEditEvent.Visible = ViewEvent1.Visible;
                    pnlDeleteEvent.Visible = ViewEvent1.Visible;
                }
            }

            if (CurrentUserSession != null && CurrentUserSession.IsAdmin())
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
                pnlAddEvent.Visible = ViewEvents1.Visible;
                pnlEditEvent.Visible = ViewEvent1.Visible;
                pnlDeleteEvent.Visible = ViewEvent1.Visible;
            }

            pnlNavLine.Visible = lnkAddEvent.Visible || lnkEditEvent.Visible || lnkDeleteEvent.Visible;
        }

        protected void lnkGroupHome_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupUrl(GroupID.ToString()));
        }

        protected void lnkAddEvent_Click(object sender, EventArgs e)
        {
            mvGroupEvents.SetActiveView(viewEdit);
            EditEvent1.Type = EditEvent.eType.AddEvent;
            EditEvent1.EventID = -1;
            EditEvent1.EventDate = ViewEvents1.EventDate;
            
            enableMenuLinks();
            lnkAddEvent.Enabled = false;
        }

        protected void lnkEditEvent_Click(object sender, EventArgs e)
        {
            EditEvent1.Type = EditEvent.eType.EditEvent;
            EditEvent1.EventID = ViewedEventID;
            EditEvent1.LoadGroupEvent = true;

            enableMenuLinks();
            lnkEditEvent.Enabled = false;
            mvGroupEvents.SetActiveView(viewEdit);
        }

        protected void lnkDeleteEvent_Click(object sender, EventArgs e)
        {
            int eventID;

            if (Int32.TryParse(Request.Params["eid"], out eventID))
            {
                if (CurrentUserSession != null)
                {
                    if (!CurrentUserSession.IsAdmin())
                    {
                        if (CurrentGroupMember == null || !CurrentGroupMember.Active) // is not a member
                        {
                            if (Group.IsPermissionEnabled(eGroupPermissions.AddEventNonMembers))
                            {
                                if (ViewEvent1.CurrentEvent.Username == CurrentUserSession.Username)
                                {
                                    deleteGroupEvent(eventID);
                                }
                            }
                        }
                        else // is a member
                        {
                            if (CurrentGroupMember.Type == GroupMember.eType.Member)
                            {
                                if (Group.IsPermissionEnabled(eGroupPermissions.AddEventMembers))
                                {
                                    deleteGroupEvent(eventID);
                                }
                            }
                            else if (CurrentGroupMember.Type == GroupMember.eType.VIP)
                            {
                                if (Group.IsPermissionEnabled(eGroupPermissions.AddEventVip))
                                {
                                    deleteGroupEvent(eventID);
                                }
                            }
                            else if (CurrentGroupMember.Type == GroupMember.eType.Admin || CurrentGroupMember.Type == GroupMember.eType.Moderator)
                            {
                                deleteGroupEvent(eventID);
                            }
                        }
                    }
                    else // is super admin
                    {
                        deleteGroupEvent(eventID);
                    }
                }
            }
        }

        protected void lnkGroupEvents_Click(object sender, EventArgs e)
        {
            mvGroupEvents.SetActiveView(viewEvents);
            enableMenuLinks();
            lnkGroupEvents.Enabled = false;
//            Response.Redirect(UrlRewrite.CreateShowGroupEventsUrl(GroupID.ToString()));
        }

        private void deleteGroupEvent(int eventID)
        {
            GroupEvent.Delete(eventID);
            DataTable dtNewEvents = ViewEvents1.GroupEvents; // group events are stored in view state
            dtNewEvents.Rows.Find(eventID).Delete();
            ViewEvents1.GroupEvents = dtNewEvents;

            mvGroupEvents.SetActiveView(viewEvents);
        }

        protected void lnkGroupGallery_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupPhotosUrl(GroupID.ToString()));
        }

        protected void lnkGroupMembers_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupUrl(GroupID.ToString(), "gmembers"));
        }

        protected void lnkMessageBoard_Click(object sender, EventArgs e)
        {
            Response.Redirect(UrlRewrite.CreateShowGroupTopicsUrl(GroupID.ToString()));
        }

        protected void lnkBrowseGroups_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx");
        }
    }
}
