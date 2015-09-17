using System;
using System.Web.UI;
using AspNetDating.Classes;
using Microsoft.IdentityModel.TokenProcessor;

namespace AspNetDating
{
    public partial class LoginCardSpace : PageBase
    {
        public LoginCardSpace()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                TryLogin();
            else
                ClientScript.RegisterClientScriptBlock(GetType(), "autosubmit", 
                    "window.onload = function() { document.form1.submit(); };", true);
        }

        protected void TryLogin()
        {
            string xmlToken;
            xmlToken = Request.Params["xmlToken"];
            if (xmlToken == null || xmlToken.Equals(""))
            {
                return;
            }

            Token token = new Token(xmlToken);
            UserSession user = null;
            try
            {
                string username = Classes.User.GetUsernameByTokenUniqueId(token.UniqueID);
                if (username == null)
                {
                    ClientScript.RegisterClientScriptBlock(GetType(), "error",
                        String.Format("var alert_string = '{0}';", Lang.Trans("This card is not associated with any account!")), 
                        true);
                    return;
                }
                user = new UserSession(username);
                Classes.User.AuthorizeByToken(token.UniqueID);
                user.Authorize(Session.SessionID);
            }
            catch (NotFoundException err)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "error", String.Format("var alert_string = '{0}';",
                                                                                         err.Message), true);
                return;
            }
            catch (AccessDeniedException err)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "error", String.Format("var alert_string = '{0}';",
                                                                                         err.Message), true);
                return;
            }
            catch (SmsNotConfirmedException err)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "error",
                    String.Format("var alert_string = '{0}'; window.opener.location = 'SmsConfirm.aspx?username={1}'; window.close();",
                    err.Message, user.Username), true);
                return;
            }
            catch (ArgumentException err)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "error", String.Format("var alert_string = '{0}';",
                                                                                         err.Message), true);
                return;
            }
            catch (Exception err)
            {
                Global.Logger.LogWarning(err);
                ClientScript.RegisterClientScriptBlock(GetType(), "error", String.Format("var alert_string = '{0}';",
                                                                                         err.Message), true);
                return;
            }

            ((PageBase)Page).CurrentUserSession = user;

            if (user.PrevLogin.Date != DateTime.Now.Date)
                Classes.User.AddScore(user.Username, Config.UserScores.DailyLogin, "Login");

            try
            {
                IPLogger.Log(user.Username, Request.UserHostAddress, IPLogger.ActionType.LoginCardSpace);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("LoginCardSpace IP Logger", err);
            }

            if (Request.Params["back_url"] != null
                && Request.Params["back_url"].Length > 0)
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "redirect",
                    String.Format("window.opener.location = '{0}'; window.close();",
                    user.Username), true);
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(GetType(), "redirect",
                    String.Format("window.opener.location = '{0}'; window.close();",
                    Config.Urls.Home + "/Home.aspx"), true);
            }
        }
    }
}