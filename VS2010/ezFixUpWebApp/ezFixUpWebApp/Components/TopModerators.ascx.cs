using System;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    public partial class TopModerators : System.Web.UI.UserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (CurrentUserSession == null && Config.Users.RegistrationRequiredToBrowse)
            {
                Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
                return;
            }

            loadTopModerators();
        }

        private void loadTopModerators()
        {
            var search = new TopModeratorsSearch {Gender = User.eGender.Female};

            SearchResults1.ShowIcons = false;
            SearchResults1.ShowGender = false;
            SearchResults1.ShowDistance = false;
            SearchResults1.Results = search.GetResults();

            search.Gender = User.eGender.Male;
            SearchResults2.ShowIcons = false;
            SearchResults2.ShowGender = false;
            SearchResults2.ShowDistance = false;
            SearchResults2.Results = search.GetResults();
        }
    }
}