using System;
using System.Data;
using ezFixUp.Classes;

namespace ezFixUp.Components
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