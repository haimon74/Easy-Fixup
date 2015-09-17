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
    public partial class GroupMembers : System.Web.UI.UserControl
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

        private Group CurrentGroup
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

        public string Username
        {
            get
            {
                if (ViewState["Username"] != null)
                {
                    return (string)ViewState["Username"];
                }
                else
                {
                    throw new Exception("The username is not set!");
                }
            }
            set
            {
                ViewState["Username"] = value;
            }
        }

        public UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        public GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroup)
                {
                    return ((ShowGroup)Page).CurrentGroupMember;
                }
                else if (Page is ShowGroupPhotos)
                {
                    return ((ShowGroupPhotos)Page).CurrentGroupMember;
                }
                else if (CurrentUserSession != null)
                {
                    return GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                }
                else
                {
                    return null;
                }
            }
        }

        private bool IsFilterClicked
        {
            get
            {
                if (ViewState["IsFilterClicked"] == null)
                {
                    return false;
                }
                else
                {
                    return (bool)ViewState["IsFilterClicked"];    
                }
            }
            set { ViewState["IsFilterClicked"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            SearchResults1.DeleteMemberClick += new EventHandler(SearchResults1_DeleteMemberClick);

            if (!IsPostBack)
            {
                mvViewGroupMembers.SetActiveView(viewGroupMembers);
                loadStrings();
            }
        }

        protected void SearchResults1_DeleteMemberClick(object sender, EventArgs e)
        {
            Username = (string) ((DataListCommandEventArgs) e).CommandArgument;
            mvViewGroupMembers.SetActiveView(viewDeleteOpitions);
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Members");

            rbList.Items.Add(new ListItem(Lang.Trans("Delete all gallery photos from this group"), "deleteGroupPhotos"));
            rbList.Items.Add(new ListItem(Lang.Trans("Delete all posts of this member"), "deleteGroupPosts"));

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

            #region Filter group members strings

            btnFilter.Text = Lang.Trans("Filter");

            txtFrom.Text = Config.Users.MinAge.ToString();
            txtTo.Text = Config.Users.MaxAge.ToString();

            if (Config.Users.DisableAgeInformation)
            {
                pnlAge.Visible = false;
            }

            if (Config.Users.DisableGenderInformation)
            {
                pnlGender.Visible = false;
            }

            ddFilterByGender.Items.Clear();
            ddFilterByGender.Items.Add(new ListItem(Lang.Trans("All"), "-1"));

            if (!Config.Users.DisableGenderInformation)
            {
                ddFilterByGender.Items.Add(new ListItem(Lang.Trans("Male"), ((int) User.eGender.Male).ToString()));
                ddFilterByGender.Items.Add(new ListItem(Lang.Trans("Female"), ((int) User.eGender.Female).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    ddFilterByGender.Items.Add(
                        new ListItem(Lang.Trans("Couple"), ((int)User.eGender.Couple).ToString()));
                }
            }

            ddGroupMemberType.Items.Clear();
            ddGroupMemberType.Items.Add(new ListItem(Lang.Trans("All"), "-1"));
            ddGroupMemberType.Items.Add(new ListItem(Lang.Trans("Administrator"), ((int)GroupMember.eType.Admin).ToString()));
            ddGroupMemberType.Items.Add(new ListItem(Lang.Trans("Moderator"), ((int)GroupMember.eType.Moderator).ToString()));
            ddGroupMemberType.Items.Add(new ListItem(Lang.Trans("V.I.P."), ((int)GroupMember.eType.VIP).ToString()));
            ddGroupMemberType.Items.Add(new ListItem(Lang.Trans("Member"), ((int)GroupMember.eType.Member).ToString()));


            #endregion
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (IsFilterClicked)
            {
                executeFilterSearch();
            }
            else
            {
                executeSearch();    
            }
        }

        private void executeSearch()
        {
            if (SearchResults1.Results != null) return;

            GroupUsersSearch search = new GroupUsersSearch();

            search.GroupID = GroupID;
            search.Active = true;
            search.SortColumn = GroupUsersSearch.eSortColumn.LastOnline;

            SearchResults1.ShowLastOnline = true;
            SearchResults1.Results = search.GetResults();
            SearchResults1.GroupID = GroupID;
        }

        private void executeFilterSearch()
        {
            if (SearchResults1.Results != null) return;

            GroupUsersSearch search = new GroupUsersSearch();

            try
            {
                search.MinAge = Convert.ToInt32(txtFrom.Text.Trim());
            }
            catch
            {
            }
            try
            {
                search.MaxAge = Convert.ToInt32(txtTo.Text.Trim());
            }
            catch
            {
            }
            search.Gender = (User.eGender)Convert.ToInt32(ddFilterByGender.SelectedValue);
            search.Type = (GroupMember.eType)Convert.ToInt32(ddGroupMemberType.SelectedValue);

            search.GroupID = GroupID;
            search.Active = true;
            search.SortColumn = GroupUsersSearch.eSortColumn.LastOnline;

            SearchResults1.ShowLastOnline = true;
            SearchResults1.Results = search.GetResults();
            SearchResults1.GroupID = GroupID;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession == null || (CurrentGroupMember == null && !CurrentUserSession.IsAdmin())
                || (CurrentGroupMember != null
                    && CurrentGroupMember.Type != GroupMember.eType.Admin))
                return;

            bool kicked = false;

            if (cbBanMember.Checked)
            {
                if (ddBanPeriod.SelectedValue != "-1")
                {
                    GroupBan groupBan = new GroupBan(GroupID, Username);

                    GroupMember.Delete(GroupID, Username); // kick member

                    kicked = true;

                    if (ddBanPeriod.SelectedValue == "0") // ban forever
                    {
                        groupBan.Expires = DateTime.Now.AddYears(5);
                    }
                    else
                    {
                        groupBan.Expires = DateTime.Now.AddDays(Double.Parse(ddBanPeriod.SelectedValue));
                    }

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
                GroupMember.Delete(GroupID, Username);

                kicked = true;
            }

            if (kicked)
            {
                CurrentGroup.ActiveMembers--;
                CurrentGroup.Save();
            }

            if (cbKickMember.Checked || cbBanMember.Checked && ddBanPeriod.SelectedValue != "-1")
            {
                try
                {
                    User user = User.Load(Username);
                    MiscTemplates.DeleteGroupMemberMessage deleteGroupMemberTemplate =
                        new MiscTemplates.DeleteGroupMemberMessage(user.LanguageId);
                    if (CurrentUserSession != null)
                    {
                        Message msg = new Message(CurrentUserSession.Username, Username);
                        string message = deleteGroupMemberTemplate.Message;
                        message = message.Replace("%%GROUP%%", Parsers.ProcessGroupName(CurrentGroup.Name));
                        msg.Body = message;
                        msg.Send();
                    }
                }
                catch (NotFoundException)
                {
                }
            }

            switch (rbList.SelectedValue)
            {
                case "deleteGroupPhotos":
                    GroupPhoto.Delete(GroupID, Username);
                    break;

                case "deleteGroupPosts":
                    GroupPost.Delete(Username);
                    User.AddScore(Username, Config.UserScores.DeletedPost, "DeletedPost");
                    break;
            }

            SearchResults1.Results = null;

            mvViewGroupMembers.SetActiveView(viewGroupMembers);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvViewGroupMembers.SetActiveView(viewGroupMembers);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            IsFilterClicked = true;
            SearchResults1.Results = null;
        }
    }
}