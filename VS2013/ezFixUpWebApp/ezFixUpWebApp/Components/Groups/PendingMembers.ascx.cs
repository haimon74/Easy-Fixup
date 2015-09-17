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
    public partial class PendingMembers : System.Web.UI.UserControl
    {
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

        private UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Pending members");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            loadPendingMembers();
        }

        private void loadPendingMembers()
        {
            DataTable dtMembers = new DataTable("Pending Members");

            dtMembers.Columns.Add("GroupID");
            dtMembers.Columns.Add("Username");
            dtMembers.Columns.Add("Age");
            dtMembers.Columns.Add("Gender");
            dtMembers.Columns.Add("ImageID", typeof(int));
            dtMembers.Columns.Add("Answer");

            GroupMember[] groupMembers = GroupMember.Fetch(GroupID, false);

            if (groupMembers.Length > 0)
            {
                foreach (GroupMember groupMember in groupMembers)
                {
                    // if group member is invited
//                    if (CurrentGroup.AccessLevel == Group.eAccessLevel.Private)
//                    {
//                        continue;
//                    }
                    if (groupMember.InvitedBy != null)
                    {
                        continue;
                    }

                    User user = null;
                    int imageID = 0;

                    try
                    {
                        user = User.Load(groupMember.Username);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }

                    try
                    {
                        imageID = Photo.GetPrimary(groupMember.Username).Id;
                    }
                    catch (NotFoundException)
                    {
                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }

                    dtMembers.Rows.Add(new object[]
                                   {
                                       groupMember.GroupID,
                                       groupMember.Username,
                                       user.Age,
                                       user.Gender,
                                       imageID,
                                       groupMember.JoinAnswer
                                   });
                } 
            }
            
            if (dtMembers.Rows.Count == 0)
            {
                lblError.Text = Lang.Trans("There are no pending members.");
            }
            
            dlPendingMembers.DataSource = dtMembers;
            dlPendingMembers.DataBind();
        }

        protected void dlPendingMembers_ItemCommand(object source, DataListCommandEventArgs e)
        {
            string currentUsername = ((PageBase) Page).CurrentUserSession.Username;
            string pendingUsername = (string) e.CommandArgument;

            GroupMember groupMember = GroupMember.Fetch(GroupID, pendingUsername);

            switch (e.CommandName)
            {
                case "Approve" :
                    if (groupMember != null)
                    {
                        groupMember.Active = true;
                        groupMember.Save();
                        CurrentGroup.ActiveMembers++;
                        CurrentGroup.Save();

                        try
                        {
                            User toUser = User.Load(pendingUsername);
                            MiscTemplates.ApproveGroupMemberMessage approveGroupMemberTemplate =
                                new MiscTemplates.ApproveGroupMemberMessage(toUser.LanguageId);
                            Message msg = new Message(currentUsername, pendingUsername);
                            msg.Body = approveGroupMemberTemplate.Message.Replace("%%GROUP%%",
                                Parsers.ProcessGroupName(CurrentGroup.Name));
                            msg.Send();

                            #region Add Event

                            Event newEvent = new Event(pendingUsername);

                            newEvent.Type = Event.eType.FriendJoinedGroup;
                            FriendJoinedGroup friendJoinedGroup = new FriendJoinedGroup();
                            friendJoinedGroup.GroupID = CurrentGroup.ID;
                            newEvent.DetailsXML = Misc.ToXml(friendJoinedGroup);

                            newEvent.Save();

                            int imageID = 0;
                            try
                            {
                                imageID = Photo.GetPrimary(pendingUsername).Id;
                            }
                            catch (NotFoundException)
                            {
                                imageID = ImageHandler.GetPhotoIdByGender(toUser.Gender);
                            }

                            string[] usernames = User.FetchMutuallyFriends(pendingUsername);
                            foreach (string friendUsername in usernames)
                            {
                                if (Config.Users.NewEventNotification)
                                {
                                    string text =
                                        String.Format("Your friend {0} has joined the {1} group".Translate(),
                                                      "<b>" + pendingUsername + "</b>",
                                                      Server.HtmlEncode(CurrentGroup.Name));
                                    string thumbnailUrl =
                                        ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                    User.SendOnlineEventNotification(pendingUsername, friendUsername,
                                                                             text, thumbnailUrl,
                                                                             UrlRewrite.CreateShowGroupUrl(
                                                                                 CurrentGroup.ID.ToString()));
                                }
                            }

                            #endregion
                        }
                        catch (NotFoundException)
                        {
                        }
                    }
                    break;

                case "Reject" :
                    if (groupMember != null)
                    {
                        GroupMember.Delete(GroupID, pendingUsername);

                        try
                        {
                            User user = User.Load(pendingUsername);
                            MiscTemplates.RejectGroupMemberMessage rejectGroupMemberTemplate =
                                new MiscTemplates.RejectGroupMemberMessage(user.LanguageId);
                            Message msg = new Message(currentUsername, pendingUsername);
                            msg.Body = rejectGroupMemberTemplate.Message.Replace("%%GROUP%%",
                                Parsers.ProcessGroupName(CurrentGroup.Name));
                            msg.Send();
                        }
                        catch (NotFoundException)
                        {
                        }
                    }
                    break;
            }
        }

        protected void dlPendingMembers_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            Label lblUsernameText = (Label)e.Item.FindControl("lblUsernameText");
            Label lblAgeText = (Label)e.Item.FindControl("lblAgeText");
            Label lblGenderText = (Label)e.Item.FindControl("lblGenderText");
            Label lblAnswerText = (Label)e.Item.FindControl("lblAnswerText");
            Label lblUsername = (Label) e.Item.FindControl("lblUsername");
            Label lblAge = (Label)e.Item.FindControl("lblAge");
            Label lblGender = (Label)e.Item.FindControl("lblGender");
            Label lblAnswer = (Label)e.Item.FindControl("lblAnswer");
            
            if (CurrentGroup.JoinQuestion.Length == 0)
            {
                HtmlControl pnlAnswer = (HtmlControl)e.Item.FindControl("pnlAnswer");
                pnlAnswer.Visible = false;
            }

            if (lblUsernameText != null)
            {
                lblUsernameText.Text = Lang.Trans("Username");
            }

            if (lblAgeText != null)
            {
                lblAgeText.Text = Lang.Trans("Age");
            }

            if (lblGenderText != null)
            {
                lblGenderText.Text = Lang.Trans("Gender");
            }

            if (lblAnswerText != null)
                lblAnswerText.Text = Lang.Trans("Answer");

            if (lblUsername != null)
            {
                lblUsername.Text = (string) DataBinder.Eval(e.Item.DataItem, "Username");
            }

            if (lblAge != null)
            {
                lblAge.Text = (string) DataBinder.Eval(e.Item.DataItem, "Age");
            }

            if (lblGender != null)
            {
                lblGender.Text = (string) DataBinder.Eval(e.Item.DataItem, "Gender");
            }

            if (lblAnswer != null)
                lblAnswer.Text = (string) DataBinder.Eval(e.Item.DataItem, "Answer");
        }
    }
}