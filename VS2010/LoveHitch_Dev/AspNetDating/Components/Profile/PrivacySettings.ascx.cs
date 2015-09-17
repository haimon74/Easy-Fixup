using System;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    public partial class PrivacySettings : System.Web.UI.UserControl
    {
        public string CssClass { set { LargeBoxStart1.CssClass = value; } }
        
        private User user;
        
        public User User
        {
            set
            {
                user = value;
                if (user != null)
                {
                    ViewState["Username"] = user.Username;
                }
                else
                    ViewState["Username"] = null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (ViewState["Settings_ControlsPopulated"] == null)
            {
                ViewState["Settings_ControlsPopulated"] = true;
                populateControls();
            }
        }

        private void loadStrings()
        {
            lblError.Text = String.Empty;

            LargeBoxStart1.Title = "Privacy Settings".Translate();

            #region Privacy Settings

            hlVisibilitySettings.Title = "Visibility settings".Translate();

            if (!Config.Users.RegistrationRequiredToBrowse)
            {
                ddViewProfile.Items.Add(new ListItem("Everyone".Translate(), ((int)User.ePrivacyLevel.Everyone).ToString()));
                ddViewPhotos.Items.Add(new ListItem("Everyone".Translate(), ((int)User.ePrivacyLevel.Everyone).ToString()));
                ddViewFriends.Items.Add(new ListItem("Everyone".Translate(), ((int)User.ePrivacyLevel.Everyone).ToString()));
                //ddViewVideos.Items.Add(new ListItem("Everyone".Translate(), ((int)User.ePrivacyLevel.Everyone).ToString()));
                //ddViewBlog.Items.Add(new ListItem("Everyone".Translate(), ((int)User.ePrivacyLevel.Everyone).ToString()));
            }

            ddViewProfile.Items.Add(new ListItem("Registered users only".Translate(), ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()));
            ddViewProfile.Items.Add(new ListItem("Friends only".Translate(), ((int)User.ePrivacyLevel.FriendsOnly).ToString()));
            ddViewProfile.Items.Add(new ListItem("Friends of friends".Translate(), ((int)User.ePrivacyLevel.FriendsOfFriends).ToString()));

            ddViewPhotos.Items.Add(new ListItem("Registered users only".Translate(), ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()));
            ddViewPhotos.Items.Add(new ListItem("Friends only".Translate(), ((int)User.ePrivacyLevel.FriendsOnly).ToString()));
            ddViewPhotos.Items.Add(new ListItem("Friends of friends".Translate(), ((int)User.ePrivacyLevel.FriendsOfFriends).ToString()));

            ddViewFriends.Items.Add(new ListItem("Registered users only".Translate(), ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()));
            ddViewFriends.Items.Add(new ListItem("Friends only".Translate(), ((int)User.ePrivacyLevel.FriendsOnly).ToString()));
            ddViewFriends.Items.Add(new ListItem("Friends of friends".Translate(), ((int)User.ePrivacyLevel.FriendsOfFriends).ToString()));

            //ddViewVideos.Items.Add(new ListItem("Registered users only".Translate(), ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()));
            //ddViewVideos.Items.Add(new ListItem("Friends only".Translate(), ((int)User.ePrivacyLevel.FriendsOnly).ToString()));
            //ddViewVideos.Items.Add(new ListItem("Friends of friends".Translate(), ((int)User.ePrivacyLevel.FriendsOfFriends).ToString()));

            //ddViewBlog.Items.Add(new ListItem("Registered users only".Translate(), ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()));
            //ddViewBlog.Items.Add(new ListItem("Friends only".Translate(), ((int)User.ePrivacyLevel.FriendsOnly).ToString()));
            //ddViewBlog.Items.Add(new ListItem("Friends of friends".Translate(), ((int)User.ePrivacyLevel.FriendsOfFriends).ToString()));

            cbDisableProfileViews.Text = Lang.Trans("Don't show me when view user profiles");
            cbHideGroupMembership.Text = Lang.Trans("Hide group membership");
            cbHideFriends.Text = Lang.Trans("Hide friends");

            if (!Config.UserScores.EnableUserLevels)
            {
                pnlDisableLevelIcon.Visible = false;
            }

            if (!Config.Groups.EnableGroups)
            {
                pnlHideGroupMembership.Visible = false;
            }

            #endregion

            #region Events Settings

            hlEventsSettings.Title = "Events Settings".Translate();

            cblEventsSettings.Items.Add(new ListItem("Birthday".Translate(), ((ulong) Event.eType.FriendBirthday).ToString()));
            cblEventsSettings.Items.Add(new ListItem("Updated Profile".Translate(), ((ulong)Event.eType.FriendUpdatedProfile).ToString()));
            //cblEventsSettings.Items.Add(new ListItem("Enters Contest".Translate(), ((ulong)Event.eType.FriendEntersContest).ToString()));
            cblEventsSettings.Items.Add(new ListItem("New Profile Comment".Translate(), ((ulong)Event.eType.NewProfileComment).ToString()));
            cblEventsSettings.Items.Add(new ListItem("New Photo Comment".Translate(), ((ulong)Event.eType.NewPhotoComment).ToString()));
            cblEventsSettings.Items.Add(new ListItem("New Photo".Translate(), ((ulong)Event.eType.NewFriendPhoto).ToString()));
            //cblEventsSettings.Items.Add(new ListItem("New Video Upload".Translate(), ((ulong)Event.eType.NewFriendVideoUpload).ToString()));
            //cblEventsSettings.Items.Add(new ListItem("New Blog Post".Translate(), ((ulong)Event.eType.NewFriendBlogPost).ToString()));
            cblEventsSettings.Items.Add(new ListItem("New Friend".Translate(), ((ulong)Event.eType.NewFriendFriend).ToString()));
//            cblEventsSettings.Items.Add(new ListItem("New Group Event", ((ulong)Event.eType.NewGroupEvent).ToString()));
            cblEventsSettings.Items.Add(new ListItem("Updated Status".Translate(), ((ulong)Event.eType.FriendUpdatedStatus).ToString()));
            cblEventsSettings.Items.Add(new ListItem("Tagged on Photo".Translate(), ((ulong)Event.eType.TaggedOnPhoto).ToString()));
            //cblEventsSettings.Items.Add(new ListItem("New Audio Upload".Translate(), ((ulong)Event.eType.NewFriendAudioUpload).ToString()));
            //cblEventsSettings.Items.Add(new ListItem("New Youtube Upload".Translate(), ((ulong)Event.eType.NewFriendYouTubeUpload).ToString()));
            cblEventsSettings.Items.Add(new ListItem("Removed Friend".Translate(), ((ulong)Event.eType.RemovedFriendFriend).ToString()));
            cblEventsSettings.Items.Add(new ListItem("New Friend Relationship".Translate(), ((ulong)Event.eType.NewFriendRelationship).ToString()));
            cblEventsSettings.Items.Add(new ListItem("Removed Friend Relationship".Translate(), ((ulong)Event.eType.RemovedFriendRelationship).ToString()));

            //if (Config.Groups.EnableGroups)
            //{
            //    cblEventsSettings.Items.Add(new ListItem("Attending Group Event".Translate(), ((ulong)Event.eType.FriendAttendingEvent).ToString()));
            //    cblEventsSettings.Items.Add(new ListItem("Joined Group".Translate(), ((ulong)Event.eType.FriendJoinedGroup).ToString()));
            //    cblEventsSettings.Items.Add(new ListItem("Left Group".Translate(), ((ulong)Event.eType.FriendLeftGroup).ToString()));
            //    cblEventsSettings.Items.Add(new ListItem("New Group".Translate(), ((ulong)Event.eType.NewFriendGroup).ToString()));
            //    cblEventsSettings.Items.Add(new ListItem("New Group Topic".Translate(), ((ulong)Event.eType.NewGroupTopic).ToString()));
            //    cblEventsSettings.Items.Add(new ListItem("New Group Photo".Translate(), ((ulong)Event.eType.NewGroupPhoto).ToString()));
            //}

            #endregion

            btnSaveChanges.Text = Lang.Trans("Save") + " » ";
        }

        private void populateControls()
        {
            #region Load Privacy Settings

            User user = User;

            cbDisableLevelIcon.Checked = user.IsOptionEnabled(eUserOptions.DisableLevelIcon);
            cbDisableProfileViews.Checked = user.IsOptionEnabled(eUserOptions.DisableProfileViews);
            cbHideFriends.Checked = user.IsOptionEnabled(eUserOptions.HideFriends);
            cbHideGroupMembership.Checked = user.IsOptionEnabled(eUserOptions.HideGroupMembership);
            cbDisableLevelIcon.Text = Lang.Trans("Hide user level icon");

            ListItem li = new ListItem("Everyone".Translate(), ((int)User.ePrivacyLevel.Everyone).ToString());
            if (user.IsOptionEnabled(eUserOptions.VisitorsCanViewProfile))
            {
                if (ddViewProfile.Items.Contains(li))
                    ddViewProfile.SelectedValue = ((int)User.ePrivacyLevel.Everyone).ToString();
                else ddViewProfile.SelectedIndex = 0;
            }
            else if (user.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewProfile))
            {
                ddViewProfile.SelectedValue = ((int)User.ePrivacyLevel.FriendsOfFriends).ToString();
            }
            else
                ddViewProfile.SelectedValue = user.IsOptionEnabled(eUserOptions.UsersCanViewProfile)
                                                  ? ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()
                                                  : ((int)User.ePrivacyLevel.FriendsOnly).ToString();
            if (user.IsOptionEnabled(eUserOptions.VisitorsCanViewPhotos))
            {
                if (ddViewPhotos.Items.Contains(li))
                    ddViewPhotos.SelectedValue = ((int)User.ePrivacyLevel.Everyone).ToString();
                else ddViewPhotos.SelectedIndex = 0;
            }
            else if (user.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewPhotos))
            {
                ddViewPhotos.SelectedValue = ((int)User.ePrivacyLevel.FriendsOfFriends).ToString();
            }
            else
                ddViewPhotos.SelectedValue = user.IsOptionEnabled(eUserOptions.UsersCanViewPhotos)
                                                  ? ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()
                                                  : ((int)User.ePrivacyLevel.FriendsOnly).ToString();
            if (user.IsOptionEnabled(eUserOptions.VisitorsCanViewFriends))
            {
                if (ddViewFriends.Items.Contains(li))
                    ddViewFriends.SelectedValue = ((int)User.ePrivacyLevel.Everyone).ToString();
                else ddViewFriends.SelectedIndex = 0;
            }
            else if (user.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewFriends))
            {
                ddViewFriends.SelectedValue = ((int)User.ePrivacyLevel.FriendsOfFriends).ToString();
            }
            else
                ddViewFriends.SelectedValue = user.IsOptionEnabled(eUserOptions.UsersCanViewFriends)
                                                  ? ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()
                                                  : ((int)User.ePrivacyLevel.FriendsOnly).ToString();
            //if (user.IsOptionEnabled(eUserOptions.VisitorsCanViewVideos))
            //{
            //    if (ddViewVideos.Items.Contains(li))
            //        ddViewVideos.SelectedValue = ((int)User.ePrivacyLevel.Everyone).ToString();
            //    else ddViewVideos.SelectedIndex = 0;
            //}
            //else if (user.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewVideos))
            //{
            //    ddViewVideos.SelectedValue = ((int)User.ePrivacyLevel.FriendsOfFriends).ToString();
            //}
            //else
            //    ddViewVideos.SelectedValue = user.IsOptionEnabled(eUserOptions.UsersCanViewVideos)
            //                                      ? ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()
            //                                      : ((int)User.ePrivacyLevel.FriendsOnly).ToString();
            //if (user.IsOptionEnabled(eUserOptions.VisitorsCanViewBlog))
            //{
            //    if (ddViewBlog.Items.Contains(li))
            //        ddViewBlog.SelectedValue = ((int)User.ePrivacyLevel.Everyone).ToString();
            //    else ddViewBlog.SelectedIndex = 0;
            //}
            //else if (user.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewBlog))
            //{
            //    ddViewBlog.SelectedValue = ((int)User.ePrivacyLevel.FriendsOfFriends).ToString();
            //}
            //else
            //    ddViewBlog.SelectedValue = user.IsOptionEnabled(eUserOptions.UsersCanViewBlog)
            //                                      ? ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString()
            //                                      : ((int)User.ePrivacyLevel.FriendsOnly).ToString();
            #endregion

            #region Load Events Settings

            foreach (ListItem item in cblEventsSettings.Items)
            {
                item.Selected = Event.IsEventsSettingEnabled((Event.eType) ((ulong) Convert.ToInt64(item.Value)),
                                                             ((PageBase) Page).CurrentUserSession);
            }

            #endregion
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;

            ulong options = ((PageBase) Page).CurrentUserSession.Options;

            options &= ~(ulong) eUserOptions.DisableProfileViews;
            options &= ~(ulong) eUserOptions.HideFriends;
            options &= ~(ulong)eUserOptions.HideGroupMembership;
            options &= ~(ulong)eUserOptions.DisableLevelIcon;
            options &= ~(ulong)eUserOptions.VisitorsCanViewProfile;
            options &= ~(ulong)eUserOptions.UsersCanViewProfile;
            options &= ~(ulong)eUserOptions.FriendsOfFriendsCanViewProfile;
            options &= ~(ulong)eUserOptions.VisitorsCanViewPhotos;
            options &= ~(ulong)eUserOptions.UsersCanViewPhotos;
            options &= ~(ulong)eUserOptions.FriendsOfFriendsCanViewPhotos;
            options &= ~(ulong)eUserOptions.VisitorsCanViewFriends;
            options &= ~(ulong)eUserOptions.UsersCanViewFriends;
            options &= ~(ulong)eUserOptions.FriendsOfFriendsCanViewFriends;
            options &= ~(ulong)eUserOptions.VisitorsCanViewVideos;
            options &= ~(ulong)eUserOptions.UsersCanViewVideos;
            options &= ~(ulong)eUserOptions.FriendsOfFriendsCanViewVideos;
            options &= ~(ulong)eUserOptions.VisitorsCanViewBlog;
            options &= ~(ulong)eUserOptions.UsersCanViewBlog;
            options &= ~(ulong)eUserOptions.FriendsOfFriendsCanViewBlog;

            if (cbDisableProfileViews.Checked) options |= (ulong)eUserOptions.DisableProfileViews;
            if (cbHideFriends.Checked) options |= (ulong)eUserOptions.HideFriends;
            if (cbHideGroupMembership.Checked) options |= (ulong)eUserOptions.HideGroupMembership;
            if (cbDisableLevelIcon.Checked) options |= (ulong)eUserOptions.DisableLevelIcon;

            if (ddViewProfile.SelectedValue == ((int)User.ePrivacyLevel.Everyone).ToString())
                options |= (ulong)eUserOptions.VisitorsCanViewProfile;
            else if (ddViewProfile.SelectedValue == ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString())
                options |= (ulong)eUserOptions.UsersCanViewProfile;
            else if (ddViewProfile.SelectedValue == ((int)User.ePrivacyLevel.FriendsOfFriends).ToString())
                options |= (ulong)eUserOptions.FriendsOfFriendsCanViewProfile;
            if (ddViewPhotos.SelectedValue == ((int)User.ePrivacyLevel.Everyone).ToString())
                options |= (ulong)eUserOptions.VisitorsCanViewPhotos;
            else if (ddViewPhotos.SelectedValue == ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString())
                options |= (ulong)eUserOptions.UsersCanViewPhotos;
            else if (ddViewPhotos.SelectedValue == ((int)User.ePrivacyLevel.FriendsOfFriends).ToString())
                options |= (ulong)eUserOptions.FriendsOfFriendsCanViewPhotos;
            if (ddViewFriends.SelectedValue == ((int)User.ePrivacyLevel.Everyone).ToString())
                options |= (ulong)eUserOptions.VisitorsCanViewFriends;
            else if (ddViewFriends.SelectedValue == ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString())
                options |= (ulong)eUserOptions.UsersCanViewFriends;
            else if (ddViewFriends.SelectedValue == ((int)User.ePrivacyLevel.FriendsOfFriends).ToString())
                options |= (ulong)eUserOptions.FriendsOfFriendsCanViewFriends;
            //if (ddViewVideos.SelectedValue == ((int)User.ePrivacyLevel.Everyone).ToString())
            //    options |= (ulong)eUserOptions.VisitorsCanViewVideos;
            //else if (ddViewVideos.SelectedValue == ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString())
            //    options |= (ulong)eUserOptions.UsersCanViewVideos;
            //else if (ddViewVideos.SelectedValue == ((int)User.ePrivacyLevel.FriendsOfFriends).ToString())
            //    options |= (ulong)eUserOptions.FriendsOfFriendsCanViewVideos;
            //if (ddViewBlog.SelectedValue == ((int)User.ePrivacyLevel.Everyone).ToString())
            //    options |= (ulong)eUserOptions.VisitorsCanViewBlog;
            //else if (ddViewBlog.SelectedValue == ((int)User.ePrivacyLevel.RegisteredUsersOnly).ToString())
            //    options |= (ulong)eUserOptions.UsersCanViewBlog;
            //else if (ddViewBlog.SelectedValue == ((int)User.ePrivacyLevel.FriendsOfFriends).ToString())
            //    options |= (ulong)eUserOptions.FriendsOfFriendsCanViewBlog;

            User.Options = options;

            #region Events Settings

            ulong eventsSettings = 0;

            foreach (ListItem item in cblEventsSettings.Items)
            {
                if (item.Selected) eventsSettings |= (ulong) Convert.ToInt64(item.Value);
            }

            User.EventsSettings = eventsSettings;

            #endregion

            User.Update(false);

            ((PageBase) Page).CurrentUserSession.EventsSettings = eventsSettings;
            ((PageBase)Page).CurrentUserSession.Options = options;

            lblError.CssClass = "message";
            lblError.Text = Lang.Trans("Your account has been successfully updated!");
            lblError.Focus();
        }
    }
}