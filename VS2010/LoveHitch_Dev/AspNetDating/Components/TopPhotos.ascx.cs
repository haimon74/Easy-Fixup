using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    public partial class TopPhotos : System.Web.UI.UserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
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

            if (CurrentUserSession == null && Config.Users.RegistrationRequiredToBrowse)
                Response.Redirect("LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));

            LoadTopPhotos();
        }

        private void LoadStrings()
        {
            lblNote.Text = String.Format(
                Lang.Trans("* Note: All photos have {0} or more votes"),
                Config.Ratings.TopPhotosMinVotes);
        }

        private void LoadTopPhotos()
        {
            TopPhotosSearch search = new TopPhotosSearch();

            search.Gender = Classes.User.eGender.Female;
            SearchResults1.ShowTopPhoto = true;
            SearchResults1.ShowIcons = false;
            SearchResults1.ShowDistance = false;
            SearchResults1.Results = search.GetResults();

            search.Gender = Classes.User.eGender.Male;
            SearchResults2.ShowTopPhoto = true;
            SearchResults2.ShowIcons = false;
            SearchResults2.ShowDistance = false;
            SearchResults2.Results = search.GetResults();

            if (Config.Users.CouplesSupport)
            {
                search.Gender = Classes.User.eGender.Couple;
                SearchResults3.ShowTopPhoto = true;
                SearchResults3.ShowIcons = false;
                SearchResults3.ShowDistance = false;
                SearchResults3.Results = search.GetResults();
                tdTopMalePhotos.Width = "33%";
                tdTopFemalePhotos.Width = "33%";
                tdTopCouplePhotos.Visible = true;
            }
            else
                tdTopCouplePhotos.Visible = false;
        }
    }
}