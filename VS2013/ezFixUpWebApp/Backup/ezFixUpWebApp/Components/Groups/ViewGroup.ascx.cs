using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxChat.Classes;
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class ViewGroup : UserControl
    {
        public MultiView mvViewGroup;        
        public int GroupID
        {
            get
            {
                return (int)ViewState["CurrentGroupId"];
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        protected Group CurrentGroup
        {
            get
            {
                if (Page is ShowGroup)
                {
                    return ((ShowGroup) Page).Group;
                }
                else
                {
                    return Group.Fetch(GroupID);
                }
            }
        }

        protected GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroup)
                {
                    return ((ShowGroup)Page).CurrentGroupMember;
                }
                else
                {
                    return GroupMember.Fetch(GroupID, ((PageBase)Page).CurrentUserSession.Username);
                }
            }

            set
            {
                if (Page is ShowGroup)
                {
                    ((ShowGroup)Page).CurrentGroupMember = value;
                }
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }
        
        protected int ChatUsersOnline
        {
            get
            {
                return ChatRoomEvents.FetchOnlineUsers(GroupID).OnlineUsers.Count;
            }
        }

        public event EventHandler MemberClickEvent;
        public event EventHandler JoinPanelOpenEvent;
        public event EventHandler JoinPanelCloseEvent;

        public void ShowMessage(Misc.MessageType type, string message)
        {
            if (vGroupInfo.Visible)
            {
                switch (type)
                {
                    case Misc.MessageType.Error:
                        lblError.CssClass = "error";
                        break;
                    case Misc.MessageType.Success:
                        lblError.CssClass = "message";
                        break;
                }                
                lblError.Text = message;
            }
            else if (vJoinGroup.Visible)
            {
                switch (type)
                {
                    case Misc.MessageType.Error:
                        lblError2.CssClass = "error";
                        break;
                    case Misc.MessageType.Success:
                        lblError2.CssClass = "message";
                        break;
                }                
                lblError2.Text = message;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                if (Config.Groups.EnableAjaxChat
                    && (CurrentUserSession != null
                        && (CurrentUserSession.CanUseChat() == PermissionCheckResult.Yes ||
                            CurrentUserSession.CanUseChat() == PermissionCheckResult.YesWithCredits
                            || (CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanUseChat))
                        ))
                    pnlChatOnlineUsers.Visible = true;
                else
                    pnlChatOnlineUsers.Visible = false;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            loadGroup();
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Group Information");
            btnJoinGroup.Text = Lang.Trans("Join Group");
            btnJoinGroup2.Text = Lang.Trans("Join Group");
            cbAgree.Text = Lang.Trans("I accept the terms & conditions");
            
            NewMembers1.GroupID = GroupID;
            NewTopics1.GroupID = GroupID;
            NewPhotos1.GroupID = GroupID;
            NewEvents1.GroupID = GroupID;
        }

        private void loadGroup()
        {
            int groupID;

            if (CurrentGroup != null)
            {
                groupID = CurrentGroup.ID;
                if (vGroupInfo.Visible)
                {
                    lblGroupDescription.Text = Parsers.ProcessGroupDescription(CurrentGroup.Description);
                    hlGroupName.Title = Parsers.ProcessGroupName(CurrentGroup.Name);
                    lblCreated.Text = CurrentGroup.DateCreated.ToShortDateString();
                    switch (CurrentGroup.AccessLevel)
                    {
                        case Group.eAccessLevel.Public:
                            lblType.Text = Lang.Trans("Public Group");
                            break;
                        case Group.eAccessLevel.Moderated:
                            lblType.Text = Lang.Trans("Moderated Group");
                            break;
                        case Group.eAccessLevel.Private:
                            lblType.Text = Lang.Trans("Private Group");
                            break;
                    }
                    lnkOwner.InnerText = CurrentGroup.Owner;
                    lnkOwner.HRef = "~/ShowUser.aspx?uid=" + CurrentGroup.Owner;

                    if (CurrentUserSession != null && CurrentGroupMember == null && CurrentGroup.Approved)
                    {
                        btnJoinGroup.Visible = true;
                    }
                    else
                    {
                        btnJoinGroup.Visible = false;
                    }
                }
                else if(vJoinGroup.Visible)
                {
                    ltrTerms.Text = CurrentGroup.JoinTerms.Replace("\n", "<br />");
                    lblQuestion.Text = CurrentGroup.JoinQuestion;
                }

                lblCategories.Text = CurrentGroup.GetCategoriesString(groupID);
            }
            else
            {
                lblError.Text = Lang.Trans("The requested group doesn't exist.");
                return;
            }

            #region Set label members

            if (!GroupMember.HasPermission(CurrentUserSession, CurrentGroupMember, CurrentGroup, eGroupPermissionType.ViewMembers))
            {
                lblMembers.Text = CurrentGroup.ActiveMembers.ToString();
                lblMembers.Visible = true;
            }
            else
            {
                lnkMembers.Text = CurrentGroup.ActiveMembers.ToString();
                lblMembers.Visible = false;
            }

            #endregion

        }

        public void btnJoinGroup_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null)
                Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

            var permissionCheckResult = CurrentUserSession.CanJoinGroups();

            if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
            {
                Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanJoinGroups;
                Response.Redirect("~/ManageProfile.aspx?sel=payment");
                return;
            }
            else if (permissionCheckResult == PermissionCheckResult.No)
            {
                ((PageBase)Page).StatusPageMessage =
                    Lang.Trans(
                        "You are not allowed to join groups.");

                Response.Redirect("~/ShowStatus.aspx");
            }
            //if (!CurrentUserSession.IsAdmin() && Config.Users.PaymentRequired && Config.Groups.OnlyPaidUsersCanJoinGroups && User.IsNonPaidMember(CurrentUserSession.Username))
            //    Response.Redirect("~/ManageProfile.aspx?sel=payment");
            
            if (CurrentGroup != null)
            {
                if (!CurrentUserSession.IsAdmin() &&
                    (CurrentGroup.JoinTerms.Length > 0 ||
                     (CurrentGroup.AccessLevel == Group.eAccessLevel.Moderated && CurrentGroup.JoinQuestion.Length > 0)
                     ))
                {
                    mvViewGroup.SetActiveView(vJoinGroup);
                    OnJoinPanelOpen(new EventArgs());
                    pnlTerms.Visible = CurrentGroup.JoinTerms.Length > 0;
                    pnlQuestion.Visible = CurrentGroup.AccessLevel == Group.eAccessLevel.Moderated &&
                                          CurrentGroup.JoinQuestion.Length > 0;
                }
                else 
                    JoinGroup();
            }
        }
        
        private void JoinGroup()
        {
            JoinGroup(String.Empty);
        }
        
        private void JoinGroup(string answer)
        {
            if (CurrentUserSession != null)
            {
                if (CurrentGroup != null)
                {
                    if (CurrentGroup.AccessLevel == Group.eAccessLevel.Private && !CurrentUserSession.IsAdmin())
                    {
                        ((PageBase) Page).StatusPageMessage =
                            Lang.Trans(
                                "This is a private group and only invited users are allowed to join. Please use 'Pending Invitations' link in the Group section to join.");

                        Response.Redirect("~/ShowStatus.aspx");
                        return;
                    }

                    string username = CurrentUserSession.Username;

                    if (GroupMember.IsBanned(username, GroupID))
                    {
                        ShowMessage(Misc.MessageType.Success, Lang.Trans("You are banned!"));
                        return;
                    }

                    int memberOf = GroupMember.Fetch(username).Length;
                    int maxGroupsPermitted = 0;// Config.Groups.MaxGroupsPerMember;
                    if (CurrentUserSession.BillingPlanOptions.MaxGroupsPerMember.Value > maxGroupsPermitted)
                        maxGroupsPermitted = CurrentUserSession.BillingPlanOptions.MaxGroupsPerMember.Value;
                    if (CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.MaxGroupsPerMember > maxGroupsPermitted)
                        maxGroupsPermitted = CurrentUserSession.Level.Restrictions.MaxGroupsPerMember;

                    if (memberOf >= maxGroupsPermitted)
                    {
                        ShowMessage(Misc.MessageType.Error,
                                    String.Format(
                                        Lang.Trans(
                                            "You are already a member of {0} groups. Please leave one of them first."),
                                        maxGroupsPermitted));
                        return;
                    }

                    GroupMember groupMember = new GroupMember(CurrentGroup.ID, username);

                    groupMember.Active = CurrentGroup.AccessLevel == Group.eAccessLevel.Public
                                         || CurrentUserSession.IsAdmin()
                                             ? true
                                             : false;
                    groupMember.Type = CurrentUserSession.IsAdmin()
                                           ? GroupMember.eType.Admin
                                           : GroupMember.eType.Member;
                    groupMember.JoinAnswer = answer;
                    groupMember.Save();

                    if (groupMember.Active)
                    {
                        #region Add Event

                        Event newEvent = new Event(CurrentUserSession.Username);

                        newEvent.Type = Event.eType.FriendJoinedGroup;
                        FriendJoinedGroup friendJoinedGroup = new FriendJoinedGroup();
                        friendJoinedGroup.GroupID = CurrentGroup.ID;
                        newEvent.DetailsXML = Misc.ToXml(friendJoinedGroup);

                        newEvent.Save();

                        string[] usernames = User.FetchMutuallyFriends(CurrentUserSession.Username);

                        foreach (string friendUsername in usernames)
                        {
                            if (Config.Users.NewEventNotification)
                            {
                                string text =
                                    String.Format("Your friend {0} has joined the {1} group".Translate(),
                                                  "<b>" + CurrentUserSession.Username + "</b>",
                                                  Server.HtmlEncode(CurrentGroup.Name));
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
                                                                         text, thumbnailUrl,
                                                                         UrlRewrite.CreateShowGroupUrl(
                                                                             CurrentGroup.ID.ToString()));
                            }
                        }

                        #endregion
                    }

                    CurrentGroupMember = groupMember;

                    if (groupMember.Active)
                    {
                        CurrentGroup.ActiveMembers++;
                        CurrentGroup.Save();
                    }

                    if (CurrentGroup.AccessLevel == Group.eAccessLevel.Moderated && !CurrentUserSession.IsAdmin())
                    {
                        ((PageBase) Page).StatusPageMessage = Lang.Trans("Your join request has been sent.");

                        Response.Redirect("~/ShowStatus.aspx");
                    }
                }

                mvViewGroup.SetActiveView(vGroupInfo);
                OnJoinPanelClose(new EventArgs());
            }
        }

        protected void lnkMembers_Click(object sender, EventArgs e)
        {
            OnMemberClick(new EventArgs());
        }

        //Invoke delegates registered with the MoreClickEvent event.
        protected virtual void OnMemberClick(EventArgs e)
        {
            if (MemberClickEvent != null)
            {
                MemberClickEvent(this, e);
            }
        }

        protected virtual void OnJoinPanelOpen(EventArgs e)
        {
            if (JoinPanelOpenEvent != null)
            {
                JoinPanelOpenEvent(this, e);
            }
        }

        protected virtual void OnJoinPanelClose(EventArgs e)
        {
            if (JoinPanelCloseEvent != null)
            {
                JoinPanelCloseEvent(this, e);
            }
        }          

        protected void btnJoinGroup2_Click(object sender, EventArgs e)
        {
            if (pnlTerms.Visible && !cbAgree.Checked)
            {
                ShowMessage(Misc.MessageType.Error, Lang.Trans("You must agree the terms in order to join this group"));
                return;
            }
            
            if (pnlQuestion.Visible)
                JoinGroup(txtAnswer.Text.Trim());
            else 
                JoinGroup();
        }
    }
}