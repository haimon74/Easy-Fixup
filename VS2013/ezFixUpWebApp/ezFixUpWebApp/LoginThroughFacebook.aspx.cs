using System;
using System.Configuration;
using ezFixUp.Classes;
using Facebook;

namespace ezFixUp
{
    public partial class LoginThroughFacebook : PageBase
    {
        public LoginThroughFacebook()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request["un"] != null && Request["pass"] != null)
            {
                var username = Request["un"];
                var password = Request["pass"];
                var script = "var f = document.getElementById('login_form');" +
                             "f.elements.namedItem('email').value = username;" +
                             "f.elements.namedItem('pass').value = password; ";

            }
            if (!IsPostBack)
            {
                if (Config.Misc.EnableFacebookIntegration)
                {
                    ezFixUpFacebookApp facebook = null;
                    long id = 0;
                    String accessToken = "";
                    string refererUrl = (Request.UrlReferrer != null) ? Request.UrlReferrer.PathAndQuery : null;
                    string backUrl = Request.QueryString["back_url"]
                                   ?? "Home.aspx";
                                   //((refererUrl.IsNotNullOrEmpty())
                                   //         ? refererUrl.Contains("back_url=")
                                   //                     ? refererUrl.Substring(refererUrl.IndexOf("back_url=") + 9)
                                   //                     : refererUrl.Substring(1)
                                            //: "Home.aspx");
                    if (backUrl.Contains("LoginThroughFacebook.aspx"))
                        backUrl = "Home.aspx";

                    if ((Request.QueryString["access_token"] != null) && (Request.QueryString["access_token"].Length > 0))
                    {
                        accessToken = Request.QueryString["access_token"];
                        facebook = new ezFixUpFacebookApp(accessToken);
                        if (Request.QueryString["fbUserID"] == null || (!long.TryParse(Request.QueryString["fbUserID"], out id)))
                        {
                            JsonObject user = facebook.Api("/me") as JsonObject;
                            id = long.Parse((String)user["id"]);
                        }
                    }
                    else if ((FacebookHelper.CurrentSessionAccessToken.IsNotNullOrEmpty()))
                    {
                        accessToken = FacebookHelper.CurrentSessionAccessToken;
                        facebook = new ezFixUpFacebookApp(accessToken);
                        JsonObject user = facebook.Api("/me") as JsonObject;
                        id = long.Parse((String)user["id"]);
                    }
                    else
                    {
                        facebook = new ezFixUpFacebookApp();
                        accessToken = facebook.AccessToken;
                        id = facebook.UserId;
                    }
                    if (id == 0)
                    {
                        if (FacebookHelper.SessionConnectTriesElapsed > 0)
                        {
                            FacebookHelper.SessionConnectTriesElapsed--;
                            Response.Write("<html>\n");
                            Response.Write("<body>\n");
                            Response.Write("<div id='fb-root'></div>\n");
                            Response.Write(
                                "<script type='text/javascript' src='http://connect.facebook.net/en_US/all.js'></script>\n");
                            Response.Write("<script type='text/javascript'>\n");
                            Response.Write("		FB.init({appId: '" + FacebookHelper.ApiKey +
                                           "', status: true, oauth: true, cookie: true, xfbml: true });");
                            Response.Write("		FB.login(function(response) {\n");
                            Response.Write("			if (response.authResponse) {\n");
                            Response.Write("				if (response.authResponse.accessToken) {\n");
                            //Response.Write("					alert('please wait'); \n");
                            //Response.Write("					window.onload = window.setTimeout('Tick()', 5000); \n");
                            //Response.Write("					function Tick() {window.location='LoginThroughFacebook.aspx?access_token=' + response.authResponse.accessToken + " + "'&fbUserID=' + response.authResponse.userID; }\n");
                            Response.Write("					if (top.iframe_canvas)  top.iframe_canvas.href='https://ezFixUp.com/LoginThroughFacebook.aspx?access_token=' + response.authResponse.accessToken + " + "'&fbUserID=' + response.authResponse.userID; \n");
                            Response.Write("					else window.location='LoginThroughFacebook.aspx?access_token=' + response.authResponse.accessToken + " + "'&fbUserID=' + response.authResponse.userID; \n");
                            Response.Write("				} else {\n");
                            Response.Write("					if (top.iframe_canvas)  top.iframe_canvas.href='https://ezFixUp.com/Login.aspx?back_url=" + backUrl + "&facebook=0'\n");
                            Response.Write("					else window.location='https://ezFixUp.com/Login.aspx?back_url=" + backUrl + "&facebook=0'\n");
                            Response.Write("				}\n");
                            Response.Write("			} else {window.location='https://ezFixUp.com/Login.aspx?facebook=0'");
                            Response.Write("	        }\n");
                            Response.Write("		}, {scope:'" + ConfigurationManager.AppSettings["FacebookPermissions"] + "'});\n");
                            Response.Write("</script>\n");
                            Response.Write("</body>\n");
                            Response.Write("</html>\n");
                            Response.Flush();
                        }
                        else
                        {
                            //FacebookHelper.SessionConnectTriesElapsed = 3;
                            Response.Redirect("~/Registration.aspx");  //TODO: should it be Login.aspx ?
                        }
                    }
                    else
                    {
                        facebook.Session.UserId = id;
                        TryLoginThroughFacebook(facebook, id, accessToken, backUrl);
                    }
                }
            }
        }

        
    }
}
