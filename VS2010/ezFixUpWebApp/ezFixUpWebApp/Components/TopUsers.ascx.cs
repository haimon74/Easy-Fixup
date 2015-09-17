using System;
using System.Web.UI;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    public partial class TopUsersCtrl : UserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack && CurrentUserSession == null && Config.Users.RegistrationRequiredToBrowse)
                Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

            LoadTopUsers();
        }

        private void LoadStrings()
        {
            lblNote.Text = String.Format(
                Lang.Trans("* Note: All users have {0} or more votes"),
                Config.Ratings.TopUsersMinVotes);
        }

        private void LoadTopUsers()
        {
            TopUsersSearch search = new TopUsersSearch();

            search.Gender = User.eGender.Female;
            SearchResults1.ShowIcons = false;
            SearchResults1.ShowGender = false;
            SearchResults1.ShowDistance = false;
            SearchResults1.Results = search.GetResults();

            search.Gender = User.eGender.Male;
            SearchResults2.ShowIcons = false;
            SearchResults2.ShowGender = false;
            SearchResults2.ShowDistance = false;
            SearchResults2.Results = search.GetResults();

            if (Config.Users.CouplesSupport)
            {
                search.Gender = User.eGender.Couple;
                SearchResults3.ShowIcons = false;
                SearchResults3.ShowGender = false;
                SearchResults3.ShowDistance = false;
                SearchResults3.Results = search.GetResults();
                tdTopCouples.Visible = true;
            }
            else
                tdTopCouples.Visible = false;
        }
    }
}