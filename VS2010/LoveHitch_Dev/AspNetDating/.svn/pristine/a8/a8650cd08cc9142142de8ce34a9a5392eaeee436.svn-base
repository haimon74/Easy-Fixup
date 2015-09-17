using System;
using AspNetDating.Classes;
using Microsoft.IdentityModel.TokenProcessor;

namespace AspNetDating
{
    public partial class PresentInfoCard : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                RequestCard();
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "autosubmit",
                    "window.onload = function() { document.form1.submit(); };", true);
        }

        protected void RequestCard()
        {
            string xmlToken;
            xmlToken = Request.Params["xmlToken"];
            if (xmlToken == null || xmlToken.Equals(""))
            {
                return;
            }
            else
            {
                Token token = new Token(xmlToken);
                CurrentUserSession.TokenUniqueId = token.UniqueID;
                CurrentUserSession.Update();
                StatusPageMessage = Lang.Trans("Information card saved successfully.");
                ClientScript.RegisterClientScriptBlock(GetType(), "redirect",
                    String.Format("window.opener.location = '{0}'; window.close();",
                    Config.Urls.Home + "/ShowStatus.aspx"), true);
                return;
            }
        }
    }
}