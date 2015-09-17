using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Classes.MySpace;
using AuthorizationRequiredException=AspNetDating.Classes.AuthorizationRequiredException;
using Config=AspNetDating.Classes.Config;
using NotFoundException=AspNetDating.Classes.NotFoundException;

namespace AspNetDating
{
    public partial class LoginThroughMySpace : PageBase
    {
        public LoginThroughMySpace()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Config.Misc.EnableMySpaceIntegration)
                {
                    UserData userData = null;

                    try
                    {
                        userData = DataAvailability.GetUserData(Context, Context.Request.Url);
                    }
                    catch (AuthorizationRequiredException authex)
                    {
                        Context.Response.Redirect(authex.AuthorizationUri.AbsoluteUri);
                    }
                    catch(OAuth.Net.Common.OAuthRequestException)
                    {
                        DataAvailability.RevokeAccess(Context);
                        Response.Redirect("~/LoginThroughMySpace.aspx");
                    }

                    if (userData != null)
                    {
                        string[] usernames = Classes.User.FetchUsernamesByMySpaceID(new[] { userData.ID });

                        StatusPageMessage = 
                            "There is no user associated with your MySpace account!".Translate();

                        if (usernames.Length == 0)
                        {
                            DataAvailability.RevokeAccess(Context);
                            Context.Response.Redirect("ShowStatus.aspx");
                        }
                        else
                        {
                            UserSession userSession;

                            try
                            {
                                userSession = new UserSession(usernames[0]);
                                ((PageBase)Page).CurrentUserSession = userSession;
                                MatchmakerHelper.CurrentUsername = userSession.Username;
                                //user.StealthMode = cbStealthMode.Checked;
                                Classes.User.AuthorizeByMySpaceID(userData.ID);
                                userSession.Authorize(Session.SessionID);
                            }
                            catch (NotFoundException err)
                            {
                                DataAvailability.RevokeAccess(Context);
                                StatusPageMessage = err.Message;
                                Response.Redirect("ShowStatus.aspx");
                                return;
                            }
                            catch (AccessDeniedException err)
                            {
                                DataAvailability.RevokeAccess(Context);
                                StatusPageMessage = err.Message;
                                Response.Redirect("ShowStatus.aspx");
                                return;
                            }
                            catch (SmsNotConfirmedException)
                            {
                                Response.Redirect("SmsConfirm.aspx?username=" + usernames[0]);
                                return;
                            }
                            catch (ArgumentException err)
                            {
                                DataAvailability.RevokeAccess(Context);
                                StatusPageMessage = err.Message;
                                Response.Redirect("ShowStatus.aspx");
                                return;
                            }
                            catch (Exception err)
                            {
                                DataAvailability.RevokeAccess(Context);
                                Global.Logger.LogWarning(err);
                                StatusPageMessage = err.Message;
                                Response.Redirect("ShowStatus.aspx");
                                return;
                            }

                            CurrentUserSession = userSession;
                            Response.Redirect("Home.aspx");
                        }
                    }
                }                
            }
        }
    }
}
