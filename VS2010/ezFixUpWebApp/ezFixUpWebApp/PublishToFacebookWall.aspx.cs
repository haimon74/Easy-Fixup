using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using Facebook;
using System;

namespace ezFixUp
{
    public partial class PublishToFacebookWall : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            divFacebook.Visible = Config.Misc.EnableFacebookIntegration;

            if (!Config.Misc.EnableFacebookIntegration)
            {
                Response.End();
                return;
            }

            //btnInvite.Text = "Invite".Translate();

            ezFixUpFacebookApp facebook = null;
            if (Global.GetSessionState()["facebook"] != null)
            {
                facebook = (ezFixUpFacebookApp)Global.GetSessionState()["facebook"];
            }
            else if ((Global.GetSessionState()["facebookAccessToken"] != null) && (((String)Global.GetSessionState()["facebookAccessToken"]).Length > 0))
            {
                facebook = new ezFixUpFacebookApp((String)Global.GetSessionState()["facebookAccessToken"]);
            }
            else if (Request.QueryString["access_token"] != null && Request.QueryString["access_token"].Length > 0)
            {
                facebook = new ezFixUpFacebookApp(Request.QueryString["access_token"]);
            }
            else
            {
                facebook = new ezFixUpFacebookApp();
            }
            DataTable dtFriends = new DataTable("friends");
            dtFriends.Columns.Add("Id", typeof (long));
            dtFriends.Columns.Add("Name");
            JsonObject fbUser = facebook.Api("/me") as JsonObject;
            long id = long.Parse((String)fbUser["id"]);

            if (id > 0)
            {
                divFacebookButton.Visible = false;
            }
        }

        protected void btnPublishClick(object sender, EventArgs e)
        {
            ezFixUpFacebookApp facebook = null;
            if (Global.GetSessionState()["facebook"] != null)
            {
                facebook = (ezFixUpFacebookApp)Global.GetSessionState()["facebook"];
            }
            else if ((Global.GetSessionState()["facebookAccessToken"] != null) && (((String)Global.GetSessionState()["facebookAccessToken"]).Length > 0))
            {
                facebook = new ezFixUpFacebookApp((String)Global.GetSessionState()["facebookAccessToken"]);
            }
            else if (Request.QueryString["access_token"] != null && Request.QueryString["access_token"].Length > 0)
            {
                facebook = new ezFixUpFacebookApp(Request.QueryString["access_token"]);
            }

            //foreach (ListItem li in cblFriends.Items)
            //{
            //    //if (li.Selected)
            //    {
            //        string uidStr = String.Format("/{0}", li.Value);
            //        JsonObject fbUser = facebook.Api(uidStr) as JsonObject;
            //        if (!fbUser.ContainsKey("email")) continue;
            //        string email = (String)fbUser["email"];
            //        if (email.IsNullOrEmpty()) continue;
            //        Email.Send(CurrentUserSession.Name, 
            //                   CurrentUserSession.Email, 
            //                   "Invitation to ezFixUp".Translate(),
            //                   "https://ezFixUp.com or https://www.facebook.com/ezFixUp",
            //                   true);
            //    }   
            //}
        }
    }
}
