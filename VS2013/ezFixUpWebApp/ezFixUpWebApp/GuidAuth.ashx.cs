using System;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using ezFixUp.Classes;

namespace ezFixUp
{
    public class GuidAuth : IHttpHandler, IRequiresSessionState
    {
        private static Dictionary<string, string> GuidAuths = new Dictionary<string, string>();

        public static string Create(string username)
        {
            Guid guid = Guid.NewGuid();

            lock (GuidAuths)
            {
                GuidAuths.Add(guid.ToString(), username);
            }

            return guid.ToString();
        }

        #region IHttpHandler Members

        ///<summary>
        ///Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"></see> interface.
        ///</summary>
        ///
        ///<param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests. </param>
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["guid"] == null
                || !GuidAuths.ContainsKey(context.Request.Params["guid"]))
            {
                // Invalid or missing guid
                return;
            }

            string username = GuidAuths[context.Request.Params["guid"]];
            UserSession userSession;
            try
            {
                userSession = new UserSession(username);
                PageBase.SetCurrentUserSession(userSession);
                MatchmakerHelper.CurrentUsername = userSession.Username;
                userSession.Authorize(context.Session.SessionID);
            }
            catch (Exception err)
            {
                Global.Logger.LogError(err);
                return;
            }

            PageBase.SetCurrentUserSession(userSession);

            if (context.Request.Params["target"] != null)
            {
                switch (context.Request.Params["target"])
                {
                    case "mail":
                        context.Response.Redirect(Config.Urls.Home + "/MailBox.aspx");
                        break;
                    case "msg":
                        context.Response.Redirect(Config.Urls.Home + "/ShowMessage.aspx?mid=" +
                                                  context.Request.Params["mid"]);
                        break;
                    default:
                        context.Response.Redirect(Config.Urls.Home);
                        break;
                }
            }
            else
            {
                context.Response.Redirect(Config.Urls.Home);
            }
        }

        ///<summary>
        ///Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        ///</summary>
        ///
        ///<returns>
        ///true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.
        ///</returns>
        ///
        public bool IsReusable
        {
            get { return true; }
        }

        #endregion
    }
}