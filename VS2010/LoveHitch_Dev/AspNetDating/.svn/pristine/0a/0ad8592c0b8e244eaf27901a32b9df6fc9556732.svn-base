using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ShowUserEvents : PageBase
    {
        public string ViewedUsername
        {
            get
            {
                if (ViewState["ViewedUsername"] == null)
                {
                    if (Request.Params["uid"] != null) ViewState["ViewedUsername"] = Request.Params["uid"];
                    else Response.Redirect("~/Default.aspx");
                }

                return (string) ViewState["ViewedUsername"];
            }
        }

        public ShowUserEvents()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Users.EnableUserEventsPage)
                    Response.Redirect("~");
                loadStrings();
            }
        }

        protected override void OnLoadComplete(EventArgs e)
        {
            ViewEvents1.User = ((ShowUser)Master).ViewedUser;
        }

        protected override void OnPreRender(EventArgs e)
        {
            User ViewedUser = ((ShowUser)Master).ViewedUser;

            if (!IsPostBack)
            {
                if (CurrentUserSession == null)
                {
                    if (!ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewProfile))
                    {
                        if (ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewProfile))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=ero", Request.Params["uid"]));
                        else if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewProfile))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=efof", Request.Params["uid"]));
                        else Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=efo", Request.Params["uid"]));
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

        private void loadStrings()
        {
        }
    }
}
