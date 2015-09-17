using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Groups
{
    public partial class InviteFriends : System.Web.UI.UserControl
    {
        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

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
                    return ((ShowGroup)Page).Group;
                }
                else
                {
                    return Group.Fetch(GroupID);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            btnSend.Visible = true;

            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            btnSend.Text = Lang.Trans("Send");
            LargeBoxStart1.Title = Lang.Trans("Invite Friends");
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (txtFriend1.Text.Trim().Length == 0
                && txtFriend2.Text.Trim().Length == 0
                && txtFriend3.Text.Trim().Length == 0
                && txtFriend4.Text.Trim().Length == 0
                && txtFriend5.Text.Trim().Length == 0
                && txtFriend6.Text.Trim().Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter username!");
                return;
            }

            if (txtFriend1.Text.Trim().Length != 0 && txtFriend1.Text.Trim() == CurrentUserSession.Username
                || txtFriend2.Text.Trim().Length != 0 && txtFriend2.Text.Trim() == CurrentUserSession.Username
                || txtFriend3.Text.Trim().Length != 0 && txtFriend3.Text.Trim() == CurrentUserSession.Username
                || txtFriend4.Text.Trim().Length != 0 && txtFriend4.Text.Trim() == CurrentUserSession.Username
                || txtFriend5.Text.Trim().Length != 0 && txtFriend5.Text.Trim() == CurrentUserSession.Username
                || txtFriend6.Text.Trim().Length != 0 && txtFriend6.Text.Trim() == CurrentUserSession.Username)
            {
                lblError.Text = Lang.Trans("You are already a member for this group!");
                return;
            }

            List<string> invitedUsers = new List<string>();
            List<string> unexistedUsers = new List<string>();
            List<string> deletedUsers = new List<string>();
            List<string> inactiveUsers = new List<string>();
            List<string> alreadyMembers = new List<string>();
            List<string> exceededMembers = new List<string>();

            checkUser(txtFriend1, invitedUsers, unexistedUsers, deletedUsers, inactiveUsers, alreadyMembers, exceededMembers);
            checkUser(txtFriend2, invitedUsers, unexistedUsers, deletedUsers, inactiveUsers, alreadyMembers, exceededMembers);
            checkUser(txtFriend3, invitedUsers, unexistedUsers, deletedUsers, inactiveUsers, alreadyMembers, exceededMembers);
            checkUser(txtFriend4, invitedUsers, unexistedUsers, deletedUsers, inactiveUsers, alreadyMembers, exceededMembers);
            checkUser(txtFriend5, invitedUsers, unexistedUsers, deletedUsers, inactiveUsers, alreadyMembers, exceededMembers);
            checkUser(txtFriend6, invitedUsers, unexistedUsers, deletedUsers, inactiveUsers, alreadyMembers, exceededMembers);

            string[] strUnexistedUsers = unexistedUsers.ToArray();
            string[] strDeletedUsers = deletedUsers.ToArray();
            string[] strInactiveUsers = inactiveUsers.ToArray();
            string[] strAlreadyMembers = alreadyMembers.ToArray();
            string[] strExceededMembers = exceededMembers.ToArray();
            string strUnexisted = "";
            string strDeleted = "";
            string strInactive = "";
            string strAlready = "";
            string strExceeded = "";

            if (strUnexistedUsers.Length > 0)
            {
                strUnexisted = strUnexistedUsers.Length == 1 ? Lang.Trans("doesn't exist") : Lang.Trans("doesn't exists");    
            }

            if (strDeletedUsers.Length > 0)
            {
                strDeleted = strDeletedUsers.Length == 1 ? Lang.Trans("is deleted") : Lang.Trans("are deleted");
            }

            if (strInactiveUsers.Length > 0)
            {
                strInactive = strInactiveUsers.Length == 1 ? Lang.Trans("is not active") : Lang.Trans("are not active");
            }

            if (strAlreadyMembers.Length > 0)
            {
                strAlready = strAlreadyMembers.Length == 1 ? Lang.Trans("is already a member") : Lang.Trans("are already members");
            }

            if (strExceededMembers.Length > 0)
            {
                strExceeded = strExceededMembers.Length == 1 ? Lang.Trans("has reached his groups limit") : Lang.Trans("has reached their groups limit");
            }

            lblError.Text = String.Format("{0} {1} <br> {2} {3} <br> {4} {5} <br> {6} {7} <br> {8} {9}",
                String.Join(", ", strUnexistedUsers), strUnexisted,
                String.Join(", ", strDeletedUsers), strDeleted,
                String.Join(", ", strInactiveUsers), strInactive,
                String.Join(", ", strAlreadyMembers), strAlready,
                String.Join(", ", strExceededMembers), strExceeded);

            if (strUnexistedUsers.Length > 0 || strDeletedUsers.Length > 0
                || strInactiveUsers.Length > 0 || strAlreadyMembers.Length > 0 ||
                strExceededMembers.Length > 0)
            {
                return;
            }

            if (GroupMember.InvitationsCount(CurrentUserSession.Username, DateTime.Now.AddDays(-7)) >= Config.Groups.MaxInvitations)
            {
                lblError.Text = Lang.Trans("You have reached your invitations limit.");
                return;
            }

            MiscTemplates.InviteGroupMemberMessage inviteGroupMemberTemplate = null;

            foreach (string username in invitedUsers)
            {
                try
                {
                    User recipient  = User.Load(username);

                    GroupMember groupMember = new GroupMember(GroupID, username);
                    groupMember.InvitedBy = CurrentUserSession.Username;
                    groupMember.Save();

                    inviteGroupMemberTemplate = new MiscTemplates.InviteGroupMemberMessage(recipient.LanguageId);
                    Message msg = new Message(((PageBase)Page).CurrentUserSession.Username, username);
                    msg.Body = inviteGroupMemberTemplate.Message.Replace("%%SENDER%%", CurrentUserSession.Username);
                    msg.Body = msg.Body.Replace("%%GROUP%%", Parsers.ProcessGroupName(CurrentGroup.Name));
                    msg.Send();
                }
                catch (NotFoundException)
                {
                }
            }

            pnlInviteFriends.Visible = false;
            btnSend.Visible = false;

            lblError.CssClass = "message";
            lblError.Text = Lang.Trans("The invitation has been sent to your friends.");
        }

        private void checkUser(TextBox txtBox, List<string> invitedUsers, List<string> unexistedUsers, List<string> deletedUsers, List<string> inactiveUsers, List<string> alreadyMembers, List<string> exceedGroupsMembers)
        {
            try
            {
                User user = User.Load(txtBox.Text.Trim());
                if (!user.Deleted && user.Active)
                {
                    if (GroupMember.IsMember(user.Username, GroupID))
                    {
                        alreadyMembers.Add(user.Username);
                        return;
                    }

                    int memberOf = Group.FetchGroupsByUsername(user.Username).Length;
                    int maxGroupsPermitted = 0;// Config.Groups.MaxGroupsPerMember;
                    if (CurrentUserSession != null && CurrentUserSession.BillingPlanOptions.MaxGroupsPerMember.Value > maxGroupsPermitted)
                        maxGroupsPermitted = CurrentUserSession.BillingPlanOptions.MaxGroupsPerMember.Value;
                    if (CurrentUserSession != null && CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.MaxGroupsPerMember > maxGroupsPermitted)
                        maxGroupsPermitted = CurrentUserSession.Level.Restrictions.MaxGroupsPerMember;

                    if (memberOf >= maxGroupsPermitted)
                    {
                        exceedGroupsMembers.Add(user.Username);
                        return;
                    }

                    if (GroupMember.IsBanned(user.Username, GroupID))
                    {
                        GroupBan.Delete(GroupID, user.Username);
                    }
                    
                    invitedUsers.Add(user.Username);
                }
                else if (user.Deleted)
                {
                    deletedUsers.Add(user.Username);
                }
                else
                {
                    inactiveUsers.Add(user.Username);
                }
            }
            catch(ArgumentException)
            {
                
            }
            catch (NotFoundException)
            {
                unexistedUsers.Add(txtBox.Text);
            }
        }

        public void ShowPnlInviteFriends()
        {
            pnlInviteFriends.Visible = true;
        }

        /// <summary>
        /// Empties the text boxes.
        /// </summary>
        public void EmptyTextBoxes()
        {
            txtFriend1.Text = "";
            txtFriend2.Text = "";
            txtFriend3.Text = "";
            txtFriend4.Text = "";
            txtFriend5.Text = "";
            txtFriend6.Text = "";
        }
    }
}