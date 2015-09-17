using Facebook;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI;
using AspNetDating.Classes;
using System.Web;

namespace AspNetDating
{
    public partial class Login : PageBase
    {
        public Login()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var user = ((PageBase) Page).CurrentUserSession;
                if ( user != null && user.IsAuthorized)
                    FacebookSyncLogin(user);
                loadStrings();
                isRememberMeActivated();
            }
        }

        private void loadStrings()
        {
            cbRememberMe.Text = Lang.Trans("Remember me on this computer.");
            cvLoginBoxNotes.Text = "Please register tool-tip".Translate();
            WideBoxStart1.Title = Lang.Trans("login | register");
            fbForgotPassword.Text = Lang.Trans("Forgot your password?");
            fbForgotPassword.ToolTip = Lang.Trans("Click here to retrieve your password.");
            fbLogin.Text = Lang.Trans("Enter");

            if (fbRegister != null)
                fbRegister.Text = Lang.Trans("Register now!");

            if (Config.Users.ShowStealthMode)
            {
                cbStealthMode.Visible = true;
                cbStealthMode.Text = Lang.Trans("stealth mode");
            }
            else
                cbStealthMode.Visible = false;

            divCardSpace.Visible = Config.Users.EnableCardSpace;
            divMySpace.Visible = Config.Misc.EnableMySpaceIntegration;
            if (Config.Misc.EnableMySpaceIntegration)
                btnUseMySpace.AlternateText = "Login with MySpace".Translate();
            divFacebook.Visible = Config.Misc.EnableFacebookIntegration;

            string fbFlag = Request.QueryString["facebook"];
            if (Config.Misc.EnableFacebookIntegration &&
                !String.IsNullOrEmpty(fbFlag) && fbFlag == "1")
            {
                if (FacebookHelper.IsCurrentSessionConnected && FacebookHelper.CurrentSessionUserID > 0)
                {
                    divCardSpace.Visible = false;
                    divFacebook.Visible = false;
                    divMySpace.Visible = false;
                }
            }
            else if (fbFlag == "0")
            {
                FacebookHelper.SessionConnectTriesElapsed = 0;
            }
        }

        private void isRememberMeActivated()
        {
            if (Request.Cookies["rememberMe"] != null)
            {
                string guid = Request.Cookies["rememberMe"].Value;
                string username = Classes.User.FetchUserByGuid(guid);
                Classes.User user = null;
                try
                {
                    user = Classes.User.Load(username);
                    UserSession userSession = new UserSession(user.Username);
                    if (!user.Active)
                    {
                        if (!user.SmsConfirmed && Config.Users.SmsConfirmationRequired)
                        {
                            throw new SmsNotConfirmedException
                                (Lang.Trans("This account is not yet SMS confirmed!"));
                        }

                        throw new AccessDeniedException
                            (Lang.Trans("This account is not yet activated!"));
                    }

                    if (user.Deleted)
                    {
                        if (user.DeleteReason == null || user.DeleteReason.Trim().Length == 0)
                            throw new AccessDeniedException
                                (Lang.Trans("This user has been deleted!"));

                        throw new AccessDeniedException
                            (String.Format(Lang.Trans("This user has been deleted ({0})"), user.DeleteReason));
                    }

                    if (Config.Misc.EnableFacebookIntegration && FacebookHelper.IsCurrentSessionConnected && FacebookHelper.CurrentSessionUserID > 0)
                    {
                        if (user.FacebookID.HasValue)
                        {
                            if (FacebookHelper.CurrentSessionUserID != user.FacebookID)
                            {
                                throw new NotFoundException("The last LoveHitch user's Facebook ID connected from this computer doesn't match the current logged in Facebook user ID");
                                //FacebookHelper.SessionClear(Config.Urls.Home + "/LoginThroughFacebook.aspx");
                            }
                        }
                        else
                        {
                            user.FacebookID = FacebookHelper.CurrentSessionUserID;
                            user.Update();
                        }
                    }

                    ((PageBase)Page).CurrentUserSession = userSession;
                    MatchmakerHelper.CurrentUsername = userSession.Username;
                    userSession.Authorize(Session.SessionID);
                    
                    if (user.PrevLogin.Date != DateTime.Now.Date)
                        Classes.User.AddScore(user.Username, Config.UserScores.DailyLogin, "Login");

                    try
                    {
                        IPLogger.Log(user.Username, Request.UserHostAddress, IPLogger.ActionType.Login);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError("LoginBox IP Logger", err);
                    }
                }
                catch (NotFoundException err)
                {
                    Response.Cookies["rememberMe"].Expires = DateTime.Now.AddDays(-1);
                    lblError.Text = err.Message;
                    lblError.Visible = true;
                    return;
                }
                catch (AccessDeniedException err)
                {
                    lblError.Text = err.Message;
                    lblError.Visible = true;
                    return;
                }
                catch (SmsNotConfirmedException err)
                {
                    lblError.Text = err.Message;
                    lblError.Visible = true;
                    Response.Redirect("SmsConfirm.aspx?username=" + txtUsername.Text);
                    return;
                }
                catch (ArgumentException err)
                {
                    lblError.Text = err.Message;
                    lblError.Visible = true;
                    return;
                }
                catch (Exception err)
                {
                    Global.Logger.LogWarning(err);
                    lblError.Text = err.Message;
                    lblError.Visible = true;
                    return;
                }

                if (!string.IsNullOrEmpty(Request.Params["back_url"]))
                {
                    Response.Redirect(Request.Params["back_url"]);
                    return;
                }
                Response.Redirect("Home.aspx");
                return;
            }
        }

        protected void lnkForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("LostPassword.aspx");
        }

        protected void imgbRegister_Click(object sender, ImageClickEventArgs e)
        {
            lnkRegister_Click(sender, null);
        }

        protected void lnkRegister_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please specify username!");
                return;
            }
            if (txtPassword.Text.Length == 0)
            {
                lblError.Text = Lang.Trans("Please specify password!");
                return;
            }

            UserSession user;
            try
            {
                user = new UserSession(txtUsername.Text);
                user.StealthMode = cbStealthMode.Checked;
                user.Authorize(txtPassword.Text, Session.SessionID);
                //if(Config.Misc.EnableMySpaceIntegration ||
                if (Config.Misc.EnableFacebookIntegration && user.FacebookID.HasValue && user.FacebookID > 0)
                {
                    FacebookHelper.AddFriends(user, 60, HttpContext.Current, null);
                }
            }
            catch (NotFoundException err)
            {
                lblError.Text = err.Message;
                return;
            }
            catch (AccessDeniedException err)
            {
                lblError.Text = err.Message;
                return;
            }
            catch (SmsNotConfirmedException err)
            {
                lblError.Text = err.Message;
                Response.Redirect("SmsConfirm.aspx?username=" + txtUsername.Text);
                return;
            }
            catch (ArgumentException err)
            {
                lblError.Text = err.Message;
                return;
            }
            catch (Exception err)
            {
                Global.Logger.LogWarning(err);
                lblError.Text = err.Message;
                return;
            }
            
            ((PageBase)Page).CurrentUserSession = user;
            FacebookSyncLogin(user);
            
            if (user.PrevLogin.Date != DateTime.Now.Date)
                Classes.User.AddScore(user.Username, Config.UserScores.DailyLogin, "Login");

            if (cbRememberMe.Checked || Request.Cookies["rememberMe"] != null)
            {
                string guid = Classes.User.CreatePendingGuid(user.Username);
                Response.Cookies["rememberMe"].Value = guid;
                Response.Cookies["rememberMe"].Expires = DateTime.Now.AddDays(7);
            }

            try
            {
                IPLogger.Log(user.Username, Request.UserHostAddress, IPLogger.ActionType.Login);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("LoginBox IP Logger", err);
            }

            if (Request.Params["timestamp"] != null && !string.IsNullOrEmpty(Request.Params["back_url"]) && 
                !string.IsNullOrEmpty(Properties.Settings.Default.AuthSecretKey))
            {
                var timestamp = Request.Params["timestamp"];
                var sha1 = new SHA1Managed();
                var paramBytes = Encoding.UTF8.GetBytes(user.Username + timestamp + 
                    Properties.Settings.Default.AuthSecretKey);
                var hashBytes = sha1.ComputeHash(paramBytes);
                var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

                var redirectUrl = String.Format("{3}{4}name={0}&timestamp={1}&hash={2}", user.Username, 
                    timestamp, hash, Request.Params["back_url"], 
                    Request.Params["back_url"].Contains("?") ? ":" : "?");
                if (!Config.Users.DisableGenderInformation)
                    redirectUrl += "&gender=" + user.Gender;
                Response.Redirect(redirectUrl);
            }
            else if (!string.IsNullOrEmpty(Request.Params["back_url"]))
                Response.Redirect(Request.Params["back_url"]);
            else
                Response.Redirect("Home.aspx");
        }
        private void FacebookSyncLogin(UserSession user)
        {
            if (Config.Misc.EnableFacebookIntegration)
            {
                long id = FacebookHelper.CurrentSessionUserID;
                LoveHitchFacebookApp facebook = FacebookHelper.SessionFacebookApp;

                if (facebook != null && id == 0)
                {
                    JsonObject fbUser = facebook.Api("/me") as JsonObject;
                    if (fbUser != null)
                        id = long.Parse((String) fbUser["id"]);
                }
                string backUrl = Request.Params["back_url"];
                backUrl = Config.Urls.Home + (!string.IsNullOrEmpty(backUrl) ? backUrl : "/Home.aspx");
                if (id > 0)
                {
                    if (FacebookHelper.IsCurrentSessionConnected)
                    {
                        if (user.FacebookID != id)
                        {
                            TryLoginThroughFacebook(facebook, id, FacebookHelper.CurrentSessionAccessToken, backUrl);
                        }
                    }
                    else
                    {
                        FacebookHelper.SessionClear(backUrl);
                    }
                }
                Response.Redirect(backUrl);
            }
        }
        protected void btnUseMySpace_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("~/LoginThroughMySpace.aspx");
        }
    }
}
