using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Mobile
{
    public partial class Home : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                preparePage();
            }
        }

        private void loadStrings()
        {
            lnkLogout.Text = Lang.Trans("Logout");
            lnkViewProfileViewers.Text = Lang.Trans("View");
            lnkUsersOnline.Text = Lang.Trans("View");
            lnkViewMutualVotes.Text = Lang.Trans("View Mutual");
            lnkEditStatusText.Text = "Edit".Translate();
            lnkUpdateStatusText.Text = "Update".Translate();

            lblTitle.InnerText = String.Format("Welcome {0}".Translate(), CurrentUserSession.Username);
        }

        private void preparePage()
        {
            #region Show Profile Views

            if (Config.Users.EnableWhoViewedMyProfile)
            {
                lblProfileViews.Text = CurrentUserSession.ProfileViews.ToString();
                if (CurrentUserSession.ProfileViews == 0)
                {
                    lnkViewProfileViewers.Enabled = false;
                }
                liWhoViewedMyProfile.Visible = true;
            }
            else
            {
                liWhoViewedMyProfile.Visible = false;
            }

            #endregion

            #region Show Rating

            if (Config.Ratings.EnableProfileRatings)
            {
                pnlRating.Visible = true;
                try
                {
                    UserRating userRating = UserRating.FetchRating(CurrentUserSession.Username);

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
                int score = UserVotes.FetchVotesScore(CurrentUserSession.Username);
                if (score > 0)
                {
                    pnlVotes.Visible = true;
                    lblVotes.Text = score.ToString();
                }
                else
                {
                    pnlVotes.Visible = false;
                }
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
                UsersSince = CurrentUserSession.PrevLogin
            };
            UserSearchResults nuResults = nuSearch.GetResultsFromCache(true);
            if (nuResults == null)
            {
                pnlNewUsers.Visible = false;
            }
            else
            {
                lnkNewUsers.Text = nuResults.Usernames.Length == 1 ?
                    Lang.Trans("There is one new user since your last visit!") :
                    String.Format(Lang.Trans("There are <b>{0}</b> new users since your last visit!"), nuResults.Usernames.Length);

                pnlNewUsers.Visible = true;
            }

            #endregion

            #region Load Online Users

            var oSearch = new OnlineSearch();
            UserSearchResults oResults = oSearch.GetResults();

            if (oResults == null)
            {
                pnlUsersOnline.Visible = false;
            }
            else
            {
                lblUsersOnline.Text = oResults.Usernames.Length == 1 ?
                    Lang.Trans("There is one online user!") :
                    String.Format(Lang.Trans("There are <b>{0}</b> online users!"), oResults.Usernames.Length);

                pnlUsersOnline.Visible = true;
            }

            #endregion

            #region Show Unread Messages

            int unreadMsgCount = Message.SearchUnread(CurrentUserSession.Username).Length;
            if (unreadMsgCount > 0)
            {
                pnlNewMessages.Visible = true;

                if (unreadMsgCount == 1)
                {
                    if (lblNewMessages != null)
                    {
                        lblNewMessages.Text = Lang.Trans("You have <b>1</b> unread message!");
                        lnkNewMessages.Text = Lang.Trans("View");
                    }
                    else
                    {
                        lnkNewMessages.Text = Lang.Trans("You have <b>1</b> unread message!");
                    }
                }
                else
                {
                    if (lblNewMessages != null)
                    {
                        lblNewMessages.Text = String.Format(
                            Lang.Trans("You have <b>{0}</b> unread messages!"), unreadMsgCount);
                        lnkNewMessages.Text = Lang.Trans("View");
                    }
                    else
                    {
                        lnkNewMessages.Text = String.Format(
                            Lang.Trans("You have <b>{0}</b> unread messages!"), unreadMsgCount);
                    }
                }
            }
            else
            {
                pnlNewMessages.Visible = false;
            }

            #endregion

            #region Show Status text

            if (Config.Users.EnableUserStatusText)
            {
                pnlStatusText.Visible = !Config.Misc.SiteIsPaid || Classes.User.IsPaidMember(CurrentUserSession.Username);
                lblStatusText.Text = Server.HtmlEncode(CurrentUserSession.StatusText) ?? "Not set".Translate();
            }

            #endregion
        }

        protected void lnkViewProfileViewers_Click(object sender, EventArgs e)
        {
            Global.GetSessionState()["ShowProfileViewers"] = true;
            Response.Redirect("Search.aspx");
        }

        protected void lnkViewMutualVotes_Click(object sender, EventArgs e)
        {
            Global.GetSessionState()["MutualVoteSearch"] = true;
            Response.Redirect("Search.aspx");
        }

        protected void lnkNewUsers_Click(object sender, EventArgs e)
        {
            Global.GetSessionState()["NewUsersSearch"] = true;
            Response.Redirect("Search.aspx");
        }

        protected void lnkUsersOnline_Click(object sender, EventArgs e)
        {
            Global.GetSessionState()["OnlineUsersSearch"] = true;
            Response.Redirect("Search.aspx");
        }

        protected void lnkNewMessages_Click(object sender, EventArgs e)
        {
            Response.Redirect("Mailbox.aspx");
        }

        protected void lnkEditStatusText_Click(object sender, EventArgs e)
        {
            txtStatusText.Text = CurrentUserSession.StatusText ?? String.Empty;
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
                CurrentUserSession.StatusText = status;
                CurrentUserSession.Update();

                #region Add FriendUpdatedStatus Event & realtime notifications

                Event newEvent = new Event(CurrentUserSession.Username) { Type = Event.eType.FriendUpdatedStatus };

                var friendUpdatedStatus = new FriendUpdatedStatus { Status = status };
                newEvent.DetailsXML = Misc.ToXml(friendUpdatedStatus);

                newEvent.Save();

                string[] usernames = Classes.User.FetchMutuallyFriends(CurrentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification &&
                        (Classes.User.IsOnline(friendUsername) || Classes.User.IsUsingNotifier(friendUsername)))
                    {
                        var text = String.Format("Your friend {0} has changed their status to \"{1}\"".Translate(),
                                                 "<b>" + CurrentUserSession.Username + "</b>", status);
                        var imageID = 0;
                        try
                        {
                            imageID = CurrentUserSession.GetPrimaryPhoto().Id;
                        }
                        catch (NotFoundException)
                        {
                            imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                        }
                        var thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                        var notification = new GenericEventNotification
                        {
                            Recipient = friendUsername,
                            Sender = CurrentUserSession.Username,
                            Text = text,
                            ThumbnailUrl = thumbnailUrl,
                            RedirectUrl = UrlRewrite.CreateMobileShowUserUrl(CurrentUserSession.Username)
                        };
                        RealtimeNotification.SendNotification(notification);
                    }
                }

                #endregion
            }
            else
            {
                lblStatusText.Text = "Not set".Translate();
                CurrentUserSession.StatusText = null;
                CurrentUserSession.Update();
            }

            pnlEditStatusText.Visible = false;
            pnlViewStatusText.Visible = true;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["rememberMe"] != null)
            {
                var cookie = new HttpCookie("rememberMe") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }

            ((PageBase)Page).CurrentUserSession = null;

            Response.Redirect("default.aspx");
        }
    }
}
