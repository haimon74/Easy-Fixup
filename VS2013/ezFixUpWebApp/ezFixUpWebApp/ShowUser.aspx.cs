using System;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class ShowUserPage : PageBase
    {
        public ShowUserPage()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            ViewProfileCtrl.User = ((ShowUser) Master).ViewedUser;
            Classes.User ViewedUser = ((ShowUser)Master).ViewedUser;
            if (CurrentUserSession != null && ViewedUser != null && ViewedUser.Username == CurrentUserSession.Username)
                Response.Redirect("~/ManageProfile.aspx");

            if (!IsPostBack)
            {
                if (CurrentUserSession == null)
                {
                    if (!ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewProfile))
                    {
                        if (ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewProfile))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=ro", Request.Params["uid"]));
                        else if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewProfile))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=fof", Request.Params["uid"]));
                        else Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=fo", Request.Params["uid"]));
                    }
                }
                else
                {
                    if (ViewedUser.Username != CurrentUserSession.Username
                        && !ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewProfile)
                        && !ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewProfile))
                    {
                        if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewProfile))
                        {
                            if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                            {
                                bool areFriends = false;
                                string[] friends = Classes.User.FetchMutuallyFriends(ViewedUser.Username);
                                foreach (string friend in friends)
                                {
                                    if (Classes.User.IsUserInFriendList(friend, CurrentUserSession.Username))
                                    {
                                        areFriends = true;
                                        break;
                                    }
                                }
                                if (!areFriends)
                                {
                                    Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=fof", Request.Params["uid"]));
                                }
                            }
                        }
                        else if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                        {
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=fo", Request.Params["uid"]));
                        }
                    }
                }
            }
            base.OnPreRender(e);
        }
    }
}