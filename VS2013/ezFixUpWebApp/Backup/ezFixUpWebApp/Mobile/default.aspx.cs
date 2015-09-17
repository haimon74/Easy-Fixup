using System;
using System.Web;
using ezFixUp.Classes;

namespace ezFixUp.Mobile
{
    public partial class _default : PageBase
    {
        public _default()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Global.GetSessionState()["HasAsyncJob"] = false;
                LoadStrings();
                isRememberMeActivated();

                if (CurrentUserSession != null && CurrentUserSession.IsAuthorized)
                {
                    Response.Redirect("Home.aspx");
                }
            }
        
        }
        private void LoadStrings()
        {
            lblTitle.InnerText = String.Format("Welcome to {0}".Translate(), Config.Misc.SiteTitle);
            btnLogin.Text = "Log in".Translate();
            cbStealthMode.Text = "Stealth mode".Translate();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length == 0)
            {
                showError(Lang.Trans("Please specify username!"));
                return;
            }
            if (txtPassword.Text.Length == 0)
            {
                showError(Lang.Trans("Please specify password!"));
                return;
            }

            UserSession user;
            try
            {
                user = new UserSession(txtUsername.Text);
                user.StealthMode = cbStealthMode.Checked;
                user.Authorize(txtPassword.Text, Session.SessionID);
            }
            catch (NotFoundException err)
            {
                showError(err.Message);
                return;
            }
            catch (AccessDeniedException err)
            {
                showError(err.Message);
                return;
            }
            catch (SmsNotConfirmedException)
            {
                Response.Redirect("SmsConfirm.aspx?username=" + txtUsername.Text);
                return;
            }
            catch (ArgumentException err)
            {
                showError(err.Message);
                return;
            }
            catch (Exception err)
            {
                Global.Logger.LogWarning(err);
                showError(err.Message);
                return;
            }

            ((PageBase)Page).CurrentUserSession = user;

            //if (Config.Users.NewMessageNotification)
            //    Message.ClearNewMessageFlags(user.Username);

            if (user.PrevLogin.Date != DateTime.Now.Date)
                Classes.User.AddScore(user.Username, Config.UserScores.DailyLogin, "Login");

            string guid = Classes.User.CreatePendingGuid(user.Username);
            Response.Cookies["rememberMe"].Value = guid;
            Response.Cookies["rememberMe"].Expires = DateTime.Now.AddDays(7);

            try
            {
                IPLogger.Log(user.Username, Request.UserHostAddress, IPLogger.ActionType.Login);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("LoginBox IP Logger", err);
            }

            if (!string.IsNullOrEmpty(Request.Params["back_url"]))
                Response.Redirect(Request.Params["back_url"]);
            else
                Response.Redirect("Home.aspx");
        }

        private void showError(string error)
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "error",
                                                    String.Format("var alert_string = {0}';",
                                                    error.Replace("'", "\\'")), true);
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
                    StatusPageMessage = err.Message;
                    return;
                }
                catch (AccessDeniedException err)
                {
                    StatusPageMessage = err.Message;
                    return;
                }
                catch (SmsNotConfirmedException err)
                {
                    StatusPageMessage = err.Message;
                    Response.Redirect("SmsConfirm.aspx?username=" + txtUsername.Text);
                    return;
                }
                catch (ArgumentException err)
                {
                    StatusPageMessage = err.Message;
                    return;
                }
                catch (Exception err)
                {
                    Global.Logger.LogWarning(err);
                    StatusPageMessage = err.Message;
                    return;
                }

                if (!string.IsNullOrEmpty(Request.Params["back_url"]))
                {
                    Response.Redirect(Request.Params["back_url"]);
                    return;
                }
                else
                {
                    Response.Redirect("Home.aspx");
                    return;
                }
            }
        }
    }
}