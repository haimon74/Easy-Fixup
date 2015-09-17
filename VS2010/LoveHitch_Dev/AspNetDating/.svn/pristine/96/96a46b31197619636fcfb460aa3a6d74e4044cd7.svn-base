using System;
using System.Data;
using System.Web.UI;
using AspNetDating.Classes;


namespace AspNetDating.Components.WebParts
{
    [Themeable(true), Editable]
    public partial class MyProfileWebPart : WebPartUserControl
    {
        private UserSession _currentUserSession;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Page is PageBase)
                    _currentUserSession = ((PageBase)Page).CurrentUserSession;
                loadStrings();
                preparePage();
            }
        }
        private string AsBoldHtmlStr(object obj)
        {
            return String.Format(" <b>{0}</b> ", obj);
        }

        private void loadStrings()
        {
            this.Title = Lang.Translate("My Profile");
            lnkViewProfileViewers.Text = Lang.Trans("View");
            lnkUsersOnline.Text = Lang.Trans("View");
            lnkBlockedUsers.Text = Lang.Trans("View");
            lnkUsersBroadcasting.Text = Lang.Trans("View");
            lnkPendingInvitations.Text = Lang.Trans("View");
            lnkViewMutualVotes.Text = Lang.Trans("View Mutual");
            lnkEditStatusText.Text = "Edit".Translate();
            lnkUpdateStatusText.Text = "Update".Translate();

        }
        private void preparePage()
        {
            string youHaveStrTrans = Lang.Trans("You have");
            string thereAreTrans = Lang.Trans("There are");

            #region Load Photo

            Photo primaryPhoto = null;
            bool hasPhoto = false;
            try
            {
                primaryPhoto = _currentUserSession.GetPrimaryPhoto();
                hasPhoto = primaryPhoto != null && primaryPhoto.Approved ? true : false;
            }
            catch (NotFoundException)
            {
            }
            catch (Exception err)
            {
                if (Page is PageBase)
                    ((PageBase)Page).Log(err);
            }

            #region Check _currentUserSession.Gender and set photoId

            int photoId = hasPhoto ? primaryPhoto.Id : ImageHandler.GetPhotoIdByGender(_currentUserSession.Gender);

            #endregion

            if (false) //Config.Photos.EnablePhotoStack)
            {
                imgPhoto.ImageUrl = ImageHandler.CreateImageStackUrl(_currentUserSession.Username, 200, 150);
            }
            else
            {
                if (!hasPhoto && _currentUserSession.FacebookID.HasValue && _currentUserSession.FacebookID.Value > 0)
                {
                    imgPhoto.ImageUrl = ImageHandler.CreateFacebookImageUrl(_currentUserSession.FacebookID.Value,
                                                                            ImageHandler.eFacebookImageType.Normal);
                    
                }
                else
                {
                    imgPhoto.ImageUrl = ImageHandler.CreateImageUrl(photoId, 90, 90,
                                                            photoId <= 0, true, true);
                }
            }

            #endregion

            #region Show Profile Views

            bool isShowProfileViews = Config.Users.EnableWhoViewedMyProfile && _currentUserSession.ProfileViews > 0;
            lblProfileViews.Text = isShowProfileViews ? _currentUserSession.ProfileViews.ToString() : "";
            lnkViewProfileViewers.Visible = isShowProfileViews;
            liWhoViewedMyProfile.Visible = isShowProfileViews;
            
            #endregion

            #region Show Rating

            if (Config.Ratings.EnableProfileRatings)
            {
                pnlRating.Visible = true;
                try
                {
                    UserRating userRating = UserRating.FetchRating(_currentUserSession.Username);

                    lblRating.Text = String.Format(
                        Lang.Trans("{0} ({1} votes)"),
                        userRating.AverageVote.ToString("0.00"), userRating.Votes);
                }
                catch (NotFoundException)
                {
                    lblRating.Text = Lang.Trans("no rating");
                }
            }
            else pnlRating.Visible = false;

            #endregion

            #region Show Votes

            if (Config.Ratings.EnableProfileVoting)
            {
                int score = UserVotes.FetchVotesScore(_currentUserSession.Username);
                pnlVotes.Visible = (score > 0);
                lblVotes.Text = score.ToString();
            }
            else
            {
                pnlVotes.Visible = false;
            }

            #endregion

            #region Load New Users

            var nuSearch = new NewUsersSearch
                               {
                                   PhotoReq = Config.Users.RequirePhotoToShowInNewUsers,
                                   ProfileReq = Config.Users.RequireProfileToShowInNewUsers,
                                   UsersSince = _currentUserSession.PrevLogin,
                                   LoginCountThreshold = 1
                               };
            UserSearchResults nuResults = nuSearch.GetResultsFromCache(true);
            pnlNewUsers.Visible = (nuResults != null);
            if (pnlNewUsers.Visible)
            {
                lnkNewUsers.Text = nuResults.Usernames.Length == 1
                                       ? Lang.Trans("There is one new user since your last visit!")
                                       : thereAreTrans + AsBoldHtmlStr(nuResults.Usernames.Length) +
                                           Lang.Trans("new users since your last visit!");
            }
            #endregion

            #region Load Online Users

            var oSearch = new OnlineSearch();
            UserSearchResults oResults = oSearch.GetResults();
            pnlUsersOnline.Visible = (oResults != null);

            if (pnlUsersOnline.Visible)
            {
                lblUsersOnline.Text = oResults.Usernames.Length == 1
                                          ? Lang.Trans("There is one online registered user!")
                                          : thereAreTrans + AsBoldHtmlStr(oResults.Usernames.Length) + Lang.Trans("online registered users!");
            }
            #endregion

            #region Show users broadcasting video

            if (Config.Misc.EnableProfileVideoBroadcast)
            {
                var vbSearch = new VideoBroadcastingSearch();
                UserSearchResults vbResults = vbSearch.GetResults();

                bool hasResults = (vbResults != null);
                pnlUsersBroadcasting.Visible = hasResults;
                if (hasResults)
                {
                    lblUsersBroadcasting.Text = vbResults.Usernames.Length == 1
                                                    ? Lang.Trans("There is one user broadcasting video!")
                                                    : thereAreTrans + AsBoldHtmlStr(vbResults.Usernames.Length)+ Lang.Trans("users broadcasting video!");
                }
            }
            else
            {
                pnlUsersBroadcasting.Visible = false;
            }
            #endregion
            /*
            #region Load Blocked Users

            int blockedUsers = Classes.User.FetchBlockedUsers(_currentUserSession.Username).Count;
            bool hasBlockedUsers = blockedUsers != 0;
            pnlBlockedUsers.Visible = hasBlockedUsers;
            if (hasBlockedUsers)
            {
                lblBlockedUsers.Text = blockedUsers == 1
                                           ? Lang.Trans("There is one blocked user!")
                                           : thereAreTrans + AsBoldHtmlStr(blockedUsers)+Lang.Trans("blocked users!");
            }
            #endregion
            */
            #region Show Unread Messages

            bool hasUnreadMsgs = Message.SearchUnread(_currentUserSession.Username) != null;
            pnlNewMessages.Visible = hasUnreadMsgs;
            if (hasUnreadMsgs)
            {
                int unreadMsgCount = Message.SearchUnread(_currentUserSession.Username).Length;
                lnkNewMessages.Text = Lang.Trans("View");
                lblNewMessages.Text = unreadMsgCount == 1
                                          ? Lang.Trans("You have <b>1</b> unread message!")
                                          : youHaveStrTrans + AsBoldHtmlStr(unreadMsgCount) + Lang.Trans("unread messages!");
            }

            #endregion

            #region Show Relationship Requests

            if (Config.Users.EnableRelationshipStatus)
            {
                int relationshipRequests = Relationship.FetchRequests(_currentUserSession.Username).Length;
                bool hasRelReq = relationshipRequests > 0;
                pnlRelationshipRequests.Visible = hasRelReq;
                if (hasRelReq)
                {
                    lnkRelationshipRequests.Text = Lang.Trans("View");
                    lblRelationshipRequests.Text = relationshipRequests == 1
                                               ? Lang.Trans("You have <b>1</b> relationship request!")
                                               : youHaveStrTrans+ AsBoldHtmlStr(relationshipRequests)+ Lang.Trans("relationship requests!");
                }
            }
            else pnlRelationshipRequests.Visible = false;

            #endregion

            #region Show Friend Requests

            int friendRequests = Classes.User.FetchOpenFriendshipRequestsToUsernme(_currentUserSession.Username).Length;
            if (friendRequests > 0)
            {
                pnlFriendsRequests.Visible = true;

                if (friendRequests == 1)
                {
                    if (lblFriendsRequests != null)
                    {
                        lblFriendsRequests.Text = Lang.Trans("You have <b>1</b> friend request!");
                        lnkFriendsRequests.Text = Lang.Trans("View");
                    }
                    else
                    {
                        lnkFriendsRequests.Text = Lang.Trans("You have <b>1</b> friend request!");
                    }
                }
                else
                {
                    if (lblFriendsRequests != null)
                    {
                        lblFriendsRequests.Text = String.Format(
                            Lang.Trans("You have <b>{0}</b> friend requests!"), friendRequests);
                        lnkFriendsRequests.Text = Lang.Trans("View");
                    }
                    else
                    {
                        lnkFriendsRequests.Text = String.Format(
                            Lang.Trans("You have <b>{0}</b> friend requests!"), friendRequests);
                    }
                }
            }
            else
            {
                pnlFriendsRequests.Visible = false;
            }

            #endregion

            #region Show contest rankings

            if (Config.Ratings.EnablePhotoContests)
            {
                PhotoContestEntry[] entries = PhotoContestEntry.Load(null, null, _currentUserSession.Username, null);
                if (entries != null && entries.Length > 0)
                {
                    var dtRanks = new DataTable();
                    dtRanks.Columns.Add("Rank", typeof(int));
                    dtRanks.Columns.Add("ContestName", typeof(string));

                    foreach (PhotoContestEntry entry in entries)
                    {
                        int rank = PhotoContestEntry.FindRank(entry.ContestId, entry.Id);
                        if (rank > 0)
                        {
                            PhotoContest contest = PhotoContest.Load(entry.ContestId);
                            if (contest.DateEnds.HasValue && contest.DateEnds < DateTime.Now) continue;

                            dtRanks.Rows.Add(new object[] { rank, contest.Name });
                        }
                    }

                    if (dtRanks.Rows.Count > 0)
                    {
                        rptContestsRanks.DataSource = dtRanks;
                        rptContestsRanks.DataBind();
                        rptContestsRanks.Visible = true;
                    }
                }
            }

            #endregion

            #region Show Pending Invitations

            if (Config.Groups.EnableGroups)
            {
                int pendingInvitations = Group.FetchPendingInvitations(_currentUserSession.Username);
                bool hasPendingInv = (pendingInvitations > 0);
                pnlPendingInvitations.Visible = hasPendingInv;
                if (hasPendingInv)
                {
                    lnkPendingInvitations.Text = Lang.Trans("View");
                    lblPendingInvitatinos.Text = (pendingInvitations == 1)
                        ? Lang.Trans("You have <b>1</b> pending invitation!")    
                        : youHaveStrTrans + String.Format(" <b>{0}</b> ", pendingInvitations)+Lang.Trans("pending invitations!");
                }
            }
            #endregion

            #region Show group topic subscriptions

            if (Config.Groups.EnableGroups)
            {
                DataTable dtGroupTopicSubscriptions = new DataTable("GroupTopicSubscriptions");

                dtGroupTopicSubscriptions.Columns.Add("GroupTopicID");
                dtGroupTopicSubscriptions.Columns.Add("GroupTopicName");
                dtGroupTopicSubscriptions.Columns.Add("GroupID");
                dtGroupTopicSubscriptions.Columns.Add("GroupName");

                GroupTopic[] groupTopics =
                    GroupTopic.FetchUpdatedGroupTopicsUserHasSubscribedTo(_currentUserSession.Username);

                foreach (GroupTopic groupTopic in groupTopics)
                {
                    Group group = Group.Fetch(groupTopic.GroupID);

                    if (group != null)
                    {
                        dtGroupTopicSubscriptions.Rows.Add(new object[]
                                                           {
                                                               groupTopic.ID, groupTopic.Name, group.ID, group.Name
                                                           });
                    }
                }

                rptGroupTopicSubscriptions.DataSource = dtGroupTopicSubscriptions;
                rptGroupTopicSubscriptions.DataBind();
                rptGroupTopicSubscriptions.Visible = dtGroupTopicSubscriptions.Rows.Count > 0;
            }

            #endregion

            #region Show Status text

            if (Config.Users.EnableUserStatusText)
            {
                pnlStatusText.Visible = !Config.Misc.SiteIsPaid || Classes.User.IsPaidMember(_currentUserSession.Username);
                lblStatusText.Text = Server.HtmlEncode(_currentUserSession.StatusText) ?? "Not set".Translate();
            }

            #endregion

            #region Notify Facebook for the changed status

            //long? facebookID = _currentUserSession.FacebookID;
            //if (Config.Misc.EnableFacebookIntegration && facebookID.HasValue && facebookID > 0)
            //{
            //    divFaceBookStatusUpdate.Visible = true;
            //    pnlInviteFriendsFromFacebook.Visible = true;
            //}

            #endregion
        }

        protected void lnkUsersBroadcasting_Click(object sender, EventArgs e)
        {
            Session["BroadcastingUsersSearch"] = true;
            Response.Redirect("~/Search.aspx");
        }

        protected void lnkEditStatusText_Click(object sender, EventArgs e)
        {
            txtStatusText.Text = _currentUserSession.StatusText ?? String.Empty;
            pnlEditStatusText.Visible = true;
            pnlViewStatusText.Visible = false;
        }

        protected void lnkUpdateStatusText_Click(object sender, EventArgs e)
        {
            string status = String.Empty;

            status = txtStatusText.Text.Trim();

            if (status.Length > 0)
            {
                lblStatusText.Text = Server.HtmlEncode(status);
                _currentUserSession.StatusText = status;
                _currentUserSession.Update();

                #region Add FriendUpdatedStatus Event & realtime notifications

                Event newEvent = new Event(_currentUserSession.Username) { Type = Event.eType.FriendUpdatedStatus };

                var friendUpdatedStatus = new FriendUpdatedStatus { Status = status };
                newEvent.DetailsXML = Misc.ToXml(friendUpdatedStatus);

                newEvent.Save();

                string[] usernames = Classes.User.FetchMutuallyFriends(_currentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification &&
                        (Classes.User.IsOnline(friendUsername) || Classes.User.IsUsingNotifier(friendUsername)))
                    {
                        var text = String.Format("Your friend {0} has changed their status to \"{1}\"".Translate(),
                                                 "<b>" + _currentUserSession.Username + "</b>", status);
                        var imageID = 0;
                        try
                        {
                            imageID = _currentUserSession.GetPrimaryPhoto().Id;
                        }
                        catch (NotFoundException)
                        {
                            imageID = ImageHandler.GetPhotoIdByGender(_currentUserSession.Gender);
                        }
                        var thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                        var notification = new GenericEventNotification
                        {
                            Recipient = friendUsername,
                            Sender = _currentUserSession.Username,
                            Text = text,
                            ThumbnailUrl = thumbnailUrl,
                            RedirectUrl = UrlRewrite.CreateShowUserUrl(_currentUserSession.Username)
                        };
                        RealtimeNotification.SendNotification(notification);
                    }
                }

                #endregion

                // Update Twitter status
                if (Config.Misc.EnableTwitterIntegration && Twitter.HasCredentials(_currentUserSession.Username))
                {
                    try
                    {
                        Twitter.PublishTweet(_currentUserSession.Username, status);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError("Twitter", err);
                    }
                }
            }
            else
            {
                lblStatusText.Text = "Not set".Translate();
                _currentUserSession.StatusText = null;
                _currentUserSession.Update();
            }

            pnlEditStatusText.Visible = false;
            pnlViewStatusText.Visible = true;
        }

        protected void lnkNewEcards_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx?sel=recec");
        }

        protected void lnkFriendsRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/FriendsRequests.aspx");
        }

        protected void lnkRelationshipRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Friends.aspx");
        }

        protected void lnkNewUsers_Click(object sender, EventArgs e)
        {
            Session["SelectedSearch"] = "new_users";
            Response.Redirect("~/NewUsersList.aspx");
        }

        protected void lnkUsersOnline_Click(object sender, EventArgs e)
        {
            Session["SelectedSearch"] = "online_users";
            Response.Redirect("~/OnlineUsersList.aspx");
        }

        protected void lnkViewProfileViewers_Click(object sender, EventArgs e)
        {
            Session["SelectedSearch"] = "viewed_my_profile";
            Response.Redirect("~/ViewedMyProfileList.aspx");
        }

        protected void lnkViewMutualVotes_Click(object sender, EventArgs e)
        {
            Session["SelectedSearch"] = "mutual_votes";
            Response.Redirect("~/SessionSearchList.aspx");
        }

        protected void lnkNewMessages_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx");
        }

        protected void lnkEcards_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx?sel=recec");
        }

        protected void lnkSentEcards_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Mailbox.aspx?sel=sentec");
        }

        protected void lnkPendingInvitations_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Groups.aspx?show=pi");
        }

        protected void lnkncommingMessagesRestrictions_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/ManageProfile.aspx?sel=set");
        }

        protected void lnkBlockedUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/BlockedUsersList.aspx");
        }
    }
}