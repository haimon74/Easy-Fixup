using System;
using System.Data;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    [Editable]
    public partial class FriendsOnlineBoxWebPart : WebPartUserControl
    {
        private UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Friends Online");
            if (CurrentUserSession != null)
            {
                loadFriends();
            }
        }

        private void loadFriends()
        {
            DataTable dtFriends = new DataTable("Friends");

            dtFriends.Columns.Add("Username");
            dtFriends.Columns.Add("Name");
            dtFriends.Columns.Add("StatusText");

            string[] friendsUsernames = User.FetchMutuallyFriends(CurrentUserSession.Username);
            string statusText = String.Empty;
            string nameText = String.Empty;

            foreach (string username in friendsUsernames)
            {
                try
                {
                    User user = User.Load(username);

                    if (user.IsOnline())
                    {
                        statusText = Server.HtmlEncode(user.StatusText);
                        using (var db = new Model.ezFixUpDataContext()){
                            nameText = Server.HtmlEncode(
                                CompiledQueries.FetchDisplayedName(db, CurrentUserSession.Username, user.Username));
                        }
                        dtFriends.Rows.Add(new object[] {username, nameText, statusText});
                    }
                }
                catch (NotFoundException)
                {
                    continue;
                }
            }

            if (dtFriends.Rows.Count > 0)
            {
                rptFriends.DataSource = dtFriends;
                rptFriends.DataBind();
                mvFriends.SetActiveView(vFriends);
            }
            else
            {
                mvFriends.SetActiveView(vNoFriends);
            }
        }
    }
}