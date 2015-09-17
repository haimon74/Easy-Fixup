using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using Facebook;
using System;

namespace AspNetDating
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

            LoveHitchFacebookApp facebook = null;
            if (Context.Session["facebook"] != null)
            {
                facebook = (LoveHitchFacebookApp)Context.Session["facebook"];
            }
            else if ((Context.Session["facebookAccessToken"] != null) && (((String)Context.Session["facebookAccessToken"]).Length > 0))
            {
                facebook = new LoveHitchFacebookApp((String)Context.Session["facebookAccessToken"]);
            }
            else if (Request.QueryString["access_token"] != null && Request.QueryString["access_token"].Length > 0)
            {
                facebook = new LoveHitchFacebookApp(Request.QueryString["access_token"]);
            }
            else
            {
                facebook = new LoveHitchFacebookApp();
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
            LoveHitchFacebookApp facebook = null;
            if (Context.Session["facebook"] != null)
            {
                facebook = (LoveHitchFacebookApp)Context.Session["facebook"];
            }
            else if ((Context.Session["facebookAccessToken"] != null) && (((String)Context.Session["facebookAccessToken"]).Length > 0))
            {
                facebook = new LoveHitchFacebookApp((String)Context.Session["facebookAccessToken"]);
            }
            else if (Request.QueryString["access_token"] != null && Request.QueryString["access_token"].Length > 0)
            {
                facebook = new LoveHitchFacebookApp(Request.QueryString["access_token"]);
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
            //                   "Invitation to LoveHitch".Translate(),
            //                   "https://www.LoveHitch.com or https://www.facebook.com/LoveHitch",
            //                   true);
            //    }   
            //}
        }
    }
}
