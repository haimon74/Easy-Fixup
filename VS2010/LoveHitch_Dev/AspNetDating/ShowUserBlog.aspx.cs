using System;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ShowUserBlog : PageBase
    {
        public ShowUserBlog()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            ViewBlogCtrl.User = ((ShowUser)Master).ViewedUser;
            Classes.User ViewedUser = ((ShowUser)Master).ViewedUser;

            if (!IsPostBack)
            {
                if (CurrentUserSession == null)
                {
                    if (!ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewBlog))
                    {
                        if (ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewBlog))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=bro", Request.Params["uid"]));
                        else if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewBlog))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=bfof", Request.Params["uid"]));
                        else Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=bfo", Request.Params["uid"]));
                    }
                }
                else
                {
                    if (ViewedUser.Username != CurrentUserSession.Username
                        && !ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewBlog)
                        && !ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewBlog))
                    {
                        if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewBlog))
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
                                    Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=bfof", Request.Params["uid"]));
                                }
                            }
                        }
                        else if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                        {
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=bfo", Request.Params["uid"]));
                        }
                    }
                }
            }
            base.OnLoadComplete(e);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }
    }
}