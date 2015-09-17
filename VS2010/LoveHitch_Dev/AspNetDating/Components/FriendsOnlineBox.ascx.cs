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
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    public partial class FriendsOnlineBox : System.Web.UI.UserControl
    {
        UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                loadFriends();
            }
        }

        private void loadFriends()
        {
            DataTable dtFriends = new DataTable("Friends");

            dtFriends.Columns.Add("Username");

            string[] friendsUsernames = User.FetchFavouriteUsers(CurrentUserSession.Username);

            foreach (string username in friendsUsernames)
            {
                try
                {
                    User user = User.Load(username);

                    if (user.IsOnline())
                    {
                        dtFriends.Rows.Add(new object[] { username });
                    }
                }
                catch(NotFoundException)
                {
                    continue;
                }
            }

            if (dtFriends.Rows.Count > 0)
            {
                rptFriends.DataSource = dtFriends;
                rptFriends.DataBind();
            }
            else
            {
                Visible = false;
            }
        }
    }
}