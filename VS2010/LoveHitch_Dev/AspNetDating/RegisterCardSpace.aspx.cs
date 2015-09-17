using System;
using System.Web.UI;
using AspNetDating.Classes;
using Microsoft.IdentityModel.TokenProcessor;

namespace AspNetDating
{
    public partial class RegisterCardSpace : Page
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
                Session["Token"] = token;
                ClientScript.RegisterClientScriptBlock(GetType(), "redirect",
                    String.Format("window.opener.location = '{0}'; window.close();",
                    Config.Urls.Home + "/Registration.aspx?token=1"), true);
                return;
            }
        }
    }
}