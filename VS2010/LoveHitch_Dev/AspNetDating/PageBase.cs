/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using AspNetDating.Classes;
using eStreamBG.Dating;
using Facebook;


namespace AspNetDating
{
    /// <summary>
    /// This class is the base of all AspNetDating pages
    /// </summary>
    public class PageBase : InternalPage
    {
        protected IList<BeginEventHandler> AsyncPageBeginHandlersList;
        protected IList<EndEventHandler> AsyncPageEndHandlersList;
        private DateTime initTime = DateTime.Now;
        protected Dictionary<string, List<Control>> ControlsLogicalGroupsDic { get; set; }
        protected TimeMeasure timestamps = new TimeMeasure();

        //public class UserSession
        /// <summary>
        /// Set to false if the page will be visible to non-logged users
        /// </summary>
        public bool RequiresAuthorization = true;
        /// <summary>
        /// Set to true if you do not want IM available on the page
        /// </summary>
        public bool DoNotRegisterIMJavascript = false;

        public static string PleaseChooseTranslated { get { return "--- " + Lang.Translate("please choose") + " ---"; } }
        public static string CloseTranslated { get { return "Close".Translate(); } }
        public static string SendTranslated { get { return "Send".Translate(); } }
        public static string SubmitTranslated { get { return "Submit".Translate(); } }
        public static string SelectTranslated { get { return "Select".Translate(); } }
        public static string DeleteTranslated { get { return "Delete".Translate(); } }
        public static string DisableTranslated { get { return "Disable".Translate(); } }
        public static string DonotShowThisTipAgainTranslated { get { return "Do not Show This Tip Again".Translate(); } }
        public static string HideTranslated { get { return "Hide".Translate(); } }
        public static string DonotShowTranslated { get { return "Don't Show Again".Translate(); } }
        public static string CancelTranslated { get { return "Cancel".Translate(); } }
        public static string BackTranslated { get { return "Back".Translate(); } }
        public static string LastProfileTranslated { get { return "Last viewed profile".Translate(); } }

        public static string MatchRequestConfirmationPopupContentTemplateTranslated
        {
            get { return "This Action will Send a Matchmaking Request with {0}.<br>Do You Confirm?".Translate(); }
        }

        public static string MailboxTranslated { get { return "Mailbox".Translate(); } }
        public static string SearchTranslated { get { return "Search".Translate(); } }
        public static string MatchmakingFromFriendsTranslated { get { return "From Friends".Translate(); } }
        public static string MatchmakingBySearchTranslated { get { return "By Search".Translate(); } }

        /// <summary>
        /// Gets the current user session.
        /// </summary>
        /// <returns></returns>
        public static UserSession GetCurrentUserSession()
        {
            if (HttpContext.Current != null && HttpContext.Current.Session != null && HttpContext.Current.Session["UserSession"] != null)
            {
                return HttpContext.Current.Session["UserSession"] as UserSession;
            }

            return null;
        }

        virtual protected IAsyncResult AsyncWorkBeginMethod(object sender, EventArgs e, AsyncCallback cb, object extraData)
        {
            return null;
        }
        virtual protected void AsyncWorkEndMethod(IAsyncResult asyncResult)
        {
        }

        /// <summary>
        /// Sets the current user session.
        /// </summary>
        /// <param name="newUserSession">The new user session.</param>
        public static void SetCurrentUserSession(UserSession newUserSession)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session["UserSession"] = newUserSession;
            }
        }
        /// <summary>
        /// Gets or sets the current user session.
        /// </summary>
        /// <value>The current user session.</value>
        public new UserSession CurrentUserSession
        {
            get { return base.CurrentUserSession as UserSession; }
            set
            {
                base.CurrentUserSession = value;
                PageBase.SetCurrentUserSession(value);

                if (value != null) // it will be null if the user has been logout
                {
                    if (Session["LanguageIdExplicit"] == null
                        || (bool)Session["LanguageIdExplicit"] == false)
                    {
                        LanguageId = value.LanguageId;
                    }
                    else
                    {
                        CurrentUserSession.LanguageId = LanguageId;
                        CurrentUserSession.Update();
                    }
                }
            }
        }

        public string LastViewedName
        {
            set { Session["LastViewedName"] = value; }
            get { return (string)Session["LastViewedName"] ?? ((CurrentUserSession != null) ? CurrentUserSession.Name : "Home Page".Translate()); }
        }

        public string LastViewedUsername
        {
            set { Session["LastViewedUsername"] = value; IsToRedirectToLastViewedUsername = true; }
            get { return (string)Session["LastViewedUsername"] ?? ((CurrentUserSession != null) ? CurrentUserSession.Username : "Home Page".Translate()); }
        }
        public static void SetIsToRedirectToLastViewedUsername(bool isRedirect)
        {
            HttpContext.Current.Session["IsToRedirectToLastViewedUsername"] = isRedirect;
        }
        public bool IsToRedirectToLastViewedUsername
        {
            set { Session["IsToRedirectToLastViewedUsername"] = value; }
            get { return (bool)(Session["IsToRedirectToLastViewedUsername"] ?? true); }
        }
        public string GetLastViewedProfileUrl()
        {
            if (LastViewedUsername.IsNotNullOrEmpty())
                return String.Format("ShowUser_uid_{0}.aspx", LastViewedUsername);
            return CurrentUserSession == null ? "Home.aspx" : "ManageProfile.aspx";
        }

        public void TryLoginThroughFacebook(LoveHitchFacebookApp facebook, long id, string accessToken, string backUrl)
        {
            FacebookHelper.SessionFacebookApp = facebook;
            FacebookHelper.CurrentSessionAccessToken = accessToken;
            FacebookHelper.CurrentSessionUserID = id;
            FacebookHelper.IsCurrentSessionConnected = true;
            FacebookHelper.SessionConnectTriesElapsed = 0;

            SetCurrentUserSession(null);
            string[] usernames = null;
            UserSession userSession;

            try
            {
                usernames = Classes.User.AuthorizeByFacebookID(id);
                userSession = new UserSession(usernames[0]);
                ((PageBase)Page).CurrentUserSession = userSession;
                MatchmakerHelper.CurrentUsername = userSession.Username;
                userSession.Authorize(Session.SessionID);
                userSession.LoggedInThroughFacebook = true;
                //facebook.Session.UserId = id;
                userSession.SetFacebookApp(facebook);
                //Response.Redirect(backUrl.ToLower().Contains("loginthroughfacebook.aspx") ? "home.aspx" : backUrl);
                userSession.ToUpdateFacebookFriendsData = true;
            }
            catch (NotFoundException)
            {
                if (CurrentUserSession != null)
                {
                    CurrentUserSession.FacebookID = id;
                    CurrentUserSession.Update();
                    MatchmakerHelper.CurrentUsername = CurrentUserSession.Username;
                    if (!CurrentUserSession.IsAuthorized) CurrentUserSession.Authorize(Session.SessionID);
                    CurrentUserSession.LoggedInThroughFacebook = true;
                    if (CurrentUserSession.FacebookID.HasValue && CurrentUserSession.FacebookID > 0)
                    {
                        var qry = "SELECT uid1, uid2 FROM friend " +
                            " WHERE uid1 IN (SELECT uid2 FROM friend WHERE uid1=me()) " +
                            " AND   uid2 IN (SELECT uid2 FROM friend WHERE uid1=me())";
                        JsonArray mutualFriendsByQuery = FacebookHelper.GetFqlResult(qry);
                        Session["FacebookMutualFriends"] =
                            mutualFriendsByQuery
                                .Select(o => new MutualFriendItem
                                {
                                    FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
                                    MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
                                })
                                .ToList();
                        FacebookHelper.AddFriends(CurrentUserSession, 0, HttpContext.Current, null);
                        Response.Redirect(backUrl ?? MatchmakerHelper.CurrentHomePage);
                        //Server.Transfer(backUrl ?? MatchmakerHelper.CurrentHomePage);
                    }
                }
                else
                {
                    Response.Redirect("Registration.aspx?facebook=1&back_url=" + backUrl);
                    //Server.Transfer("Registration.aspx?facebook=1&back_url=" + backUrl);
                    return;
                }
            }
            catch (AccessDeniedException err)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                StatusPageMessage = err.Message;
                parameters["next"] = "ShowStatus.aspx";
                //Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                Response.Redirect("ShowStatus.aspx");
                //Server.Transfer("ShowStatus.aspx");
                return;
            }
            catch (SmsNotConfirmedException)
            {
                Response.Redirect("SmsConfirm.aspx?username=" + usernames[0]);
                return;
            }
            catch (ArgumentException err)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                StatusPageMessage = err.Message;
                parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                //Response.Redirect(facebook.GetLogoutUrl(parameters).ToString());
                Response.Redirect("ShowStatus.aspx");
                //Server.Transfer("ShowStatus.aspx");
                return;
            }
            catch (Exception err)
            {
                IDictionary<string, object> parameters = new Dictionary<string, object>();
                Global.Logger.LogWarning(err);
                StatusPageMessage = err.Message;
                parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                Response.Redirect("ShowStatus.aspx");
                //Server.Transfer("ShowStatus.aspx"); 
                return;
            }
            //Response.Redirect(backUrl.ToLower().Contains("loginthroughfacebook.aspx") ? "default.aspx" : backUrl);
            Response.Redirect(MatchmakerHelper.CurrentHomePage);
            //Response.Redirect("ManageProfile.aspx");
            //Server.Transfer("default.aspx", false);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreInit"/> event at the beginning of page initialization.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreInit(EventArgs e)
        {
            //base.OnPreInit(e); 
            Trace.IsEnabled = Config.DebugSettings.EnableTrace;
            //Convert.ToBoolean(ConfigurationManager.AppSettings["IsTraceEnabled"]);

            if (Request.Params["theme"] != null)
            {
                Theme = Request.Params["theme"];
                Session["theme"] = Request.Params["theme"];
            }
            else if (Session["theme"] != null)
            {
                Theme = (string)Session["theme"];
            }
            else
            {
                Language language = Language.Fetch(LanguageId);
                if (!String.IsNullOrEmpty(language.Theme))
                {
                    if (Directory.Exists(Server.MapPath("~/App_Themes/" + language.Theme)))
                        Theme = language.Theme;
                }
                else
                {
                    if (!string.IsNullOrEmpty(Config.Misc.SiteTheme))
                    {
                        Theme = Config.Misc.SiteTheme;
                    }
                }
            }
            Page.EnableEventValidation = false;
            Page.ViewStateEncryptionMode = ViewStateEncryptionMode.Never;

            Classes.Plugins.Events.OnPreInitInvoke(this, e);
            base.OnPreInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //base.OnInit(e);  
            if (RequiresAuthorization)
            {
                ProcessValidation();
            }

            CommunityFaceControlValidation();

            /*
            if (Config.Misc.EnableIntegratedIM && IsUserAuthorized() && !DoNotRegisterIMJavascript)
            {
                ClientScript.RegisterClientScriptInclude("IntegratedIM", "IM/functions.js");
            }
            */

            Classes.Plugins.Events.OnInitInvoke(this, e);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.InitComplete"/> event after page initialization.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInitComplete(EventArgs e)
        {
            Classes.Plugins.Events.OnInitCompleteInvoke(this, e);
            base.OnInitComplete(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreLoad"/> event after postback data is loaded into the page server controls but before the <see cref="M:System.Web.UI.Control.OnLoad(System.EventArgs)"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreLoad(EventArgs e)
        {
            Classes.Plugins.Events.OnPreLoadInvoke(this, e);
            base.OnPreLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Classes.Plugins.Events.OnLoadInvoke(this, e);
            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.LoadComplete"/> event at the end of the page load stage.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            Classes.Plugins.Events.OnLoadCompleteInvoke(this, e);
            base.OnLoadComplete(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            Classes.Plugins.Events.OnPreRenderInvoke(this, e);
            base.OnPreRender(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreRenderComplete"/> event after the <see cref="M:System.Web.UI.Page.OnPreRenderComplete(System.EventArgs)"/> event and before the page is rendered.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            Classes.Plugins.Events.OnPreRenderCompleteInvoke(this, e);
            base.OnPreRenderComplete(e);
        }
        private static string[] aspNetFormElements = new string[] 
                                                      { 
                                                        "__EVENTTARGET",
                                                        "__EVENTARGUMENT",
                                                        "__VIEWSTATE",
                                                        "__EVENTVALIDATION",
                                                        "__VIEWSTATEENCRYPTED",
                                                      };

        //protected override void Render(HtmlTextWriter writer)
        //{
        //    StringWriter stringWriter = new StringWriter();
        //    HtmlTextWriter htmlWriter = new HtmlTextWriter(stringWriter);
        //    base.Render(htmlWriter);
        //    string html = stringWriter.ToString();
        //    int formStart = html.IndexOf("<form");
        //    int endForm = -1;
        //    if (formStart >= 0)
        //        endForm = html.IndexOf(">", formStart);

        //    if (endForm >= 0)
        //    {
        //        StringBuilder viewStateBuilder = new StringBuilder();
        //        foreach (string element in aspNetFormElements)
        //        {
        //            int startPoint = html.IndexOf("<input type=\"hidden\" name=\"" + element + "\"");
        //            if (startPoint >= 0 && startPoint > endForm)
        //            {
        //                int endPoint = html.IndexOf("/>", startPoint);
        //                if (endPoint >= 0)
        //                {
        //                    endPoint += 2;
        //                    string viewStateInput = html.Substring(startPoint, endPoint - startPoint);
        //                    html = html.Remove(startPoint, endPoint - startPoint);
        //                    viewStateBuilder.Append(viewStateInput).Append("\r\n");
        //                }
        //            }
        //        }

        //        if (viewStateBuilder.Length > 0)
        //        {
        //            viewStateBuilder.Insert(0, "\r\n");
        //            html = html.Insert(endForm + 1, viewStateBuilder.ToString());
        //        }
        //    }

        //    writer.Write(html);
        //}

        /// <summary>
        /// Gets or sets the Page's Tip Message.
        /// </summary>
        /// <value>The page Tip Message.</value>
        public string TipMessage
        {
            set { Session["PageBase_TipMessage"] = value; }
            get
            {
                string retVal = (string)Session["PageBase_TipMessage"] ?? String.Empty;
                return retVal;
            }
        }

        /// <summary>
        /// Gets or sets the Page's Tip Title.
        /// </summary>
        /// <value>The page Tip Title.</value>
        public string TipTitle
        {
            set { Session["PageBase_TipTitle"] = value; }
            get
            {
                string retVal = (string)Session["PageBase_TipTitle"] ?? String.Empty;
                return retVal;
            }
        }

        /// <summary>
        /// Gets or sets the Page's Status Message.
        /// </summary>
        /// <value>The status page message.</value>
        public string StatusPageMessage
        {
            set { Session["ShowStatus_Message"] = value; }
            get
            {
                string retVal = (string)Session["ShowStatus_Message"] ?? String.Empty;
                return retVal;
            }
        }
        public static void SetSessionStatusPageMessage(string message)
        {
            HttpContext.Current.Session["ShowStatus_Message"] = message;
        }
        /// <summary>
        /// Gets or sets the status page link URL.
        /// </summary>
        /// <value>The status page link URL.</value>
        public string StatusPageLinkURL
        {
            set { Session["ShowStatus_LinkURL"] = value; }
            get
            {
                if (((string)Session["ShowStatus_LinkURL"]).IsNullOrEmpty())
                    return "SearchList.aspx";
                else
                    return (string)Session["ShowStatus_LinkURL"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link text.
        /// </summary>
        /// <value>The status page link text.</value>
        public string StatusPageLinkText
        {
            set { Session["ShowStatus_LinkText"] = value; }
            get
            {
                if (((string)Session["ShowStatus_LinkText"]).IsNullOrEmpty())
                    return "Search".Translate();
                else
                    return (string)Session["ShowStatus_LinkText"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link skind id.
        /// </summary>
        /// <value>The status page link skind id.</value>
        public string StatusPageLinkSkindId
        {
            set { Session["ShowStatus_LinkSkindId"] = value; }
            get
            {
                if (Session["ShowStatus_LinkSkindId"] == null)
                    return null;
                else
                    return (string)Session["ShowStatus_LinkSkindId"];
            }
        }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>The language id.</value>
        public int LanguageId
        {
            set { SetLanguageId(value); }
            get { return GetLanguageId(); }
        }
        public static void SetLanguageId(int id)
        {
            HttpContext.Current.Session["LanguageId"] = id;

            HttpCookie cookie = new HttpCookie("LanguageId", id.ToString());
            cookie.Expires = DateTime.Now.AddMonths(6);
            HttpContext.Current.Response.Cookies.Add(cookie);
            var user = GetCurrentUserSession();
            if (IsUserAuthorized())
            {
                if (user.LanguageId != id)
                {
                    user.LanguageId = id;
                    user.Update();
                }
            }
            else
            {
                HttpContext.Current.Session["LanguageIdExplicit"] = true;
            }
        }
        /// <summary>
        /// Gets the language id.
        /// </summary>
        /// <returns></returns>
        public static int GetLanguageId()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Session["LanguageId"] == null)
            {
                int languageId = Config.Misc.DefaultLanguageId;
                if (ctx.Request.Cookies["LanguageId"] != null)
                {
                    try
                    {
                        languageId = Convert.ToInt32(ctx.Request.Cookies["LanguageId"].Value);
                    }
                    catch (FormatException) { }
                }
                else
                {
                    var languages = new List<Language>(Language.FetchAll());

                    string userCountry = IPToCountry.GetCountry(ctx.Request.UserHostAddress) ?? String.Empty;
                    string userCountryByCode = Config.Users.GetCountryByCode(userCountry) ?? String.Empty;
                    string[] userLanguages = ctx.Request.UserLanguages ?? new string[0];

                    var found = (from l in languages
                                 where l.Active &&
                                 (
                                    (!String.IsNullOrEmpty(l.BrowserLanguages) &&
                                    Array.Exists(l.BrowserLanguages.Split(','),
                                        browserLanguage => userLanguages.FirstOrDefault(
                                            userLanguage =>
                                                CreateSpecificCultureNameOrDefault(browserLanguage)
                                                == CreateSpecificCultureNameOrDefault(userLanguage.Split(';')[0])
                                            ) != null))
                                    ||
                                    (!String.IsNullOrEmpty(l.IpCountries) &&
                                    Array.Exists(l.IpCountries.Split(','),
                                        ipCountry =>
                                            ipCountry.ToLowerInvariant() == userCountryByCode.ToLowerInvariant()
                                        || ipCountry.ToLowerInvariant() == userCountry.ToLowerInvariant()))
                                 )
                                 select new { ID = l.Id }).FirstOrDefault();

                    if (found != null && found.ID != 0) languageId = found.ID;
                }

                ctx.Session["LanguageId"] = languageId;
                return languageId;
            }
            return (int)ctx.Session["LanguageId"];
        }

        private static string CreateSpecificCultureNameOrDefault(string name)
        {
            try
            {
                return CultureInfo.CreateSpecificCulture(name.ToLowerInvariant()).Name;
            }
            catch
            {
                return null;
            }
        }

        public void AddUpdateFacebookFriendsDataWorker(LoveHitchFacebookApp facebook, long id, string accessToken)
        {
            var clsWorker = new FacebookFriendsDataBackgroundWorkerClass(this, Context, facebook, id, accessToken);
            var bgDic = (Dictionary<string, LovehitchBackgroundWorker>)Global.AppBackgroundWorkersDic;
            if (bgDic != null)
            {
                UpdateFacebookFriendsDataBackgroundWorker bgWorker =
                    (UpdateFacebookFriendsDataBackgroundWorker)
                    bgDic["AspNetDating.Classes.UpdateFacebookFriendsDataBackgroundWorker"];

                if (bgWorker != null && facebook != null && CurrentUserSession.FacebookID.HasValue)
                {
                    lock (UpdateFacebookFriendsDataBackgroundWorker.WorkersList)
                    {
                        UpdateFacebookFriendsDataBackgroundWorker.WorkersList.Add(clsWorker);
                        CurrentUserSession.ToUpdateFacebookFriendsData = false;
                    }
                }
            }
        }
        /// <summary>
        /// Processes the validation.
        /// </summary>
        protected void ProcessValidation()
        {
            if (!IsUserAuthorized())
            {
                Response.Redirect("~/LoginThroughFacebook.aspx?back_url=" + Request.Url.PathAndQuery.Substring(1));
                //Response.Redirect("~/LoginThroughFacebook.aspx?back_url=" + Server.UrlEncode(Request.Url.PathAndQuery.Substring(1)));
            }
        }

        private void CommunityFaceControlValidation()
        {
            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.IsAdmin())
                {
                    return;
                }

                if (Config.CommunityFaceControlSystem.EnableCommunityFaceControl
                    && !CurrentUserSession.FaceControlApproved
                    && !(Page is ManageMemberProfile) && !(Page is EditSalutePhoto) && !(Page is ShowStatus))
                {
                    if (!CurrentUserSession.HasProfile)
                    {
                        StatusPageLinkSkindId = "";
                        StatusPageLinkText = "Manage Profile";
                        StatusPageLinkURL = Config.Urls.Home + "/manageprofile.aspx";
                        StatusPageMessage = Lang.Trans("You must complete your profile in order to continue!");
                    }
                    else if (Photo.Search(-1, CurrentUserSession.Username, null, null, null, null, null).Length < Config.CommunityFaceControlSystem.MinPhotosRequired)
                    {
                        StatusPageLinkSkindId = "UploadPhotos";
                        StatusPageLinkText = Lang.Trans("Upload Photos");
                        StatusPageLinkURL = "~/manageprofile.aspx?sel=photos";
                        StatusPageMessage =
                            String.Format(Lang.Trans("You must upload at least {0} photos in order to continue!"),
                                          Config.CommunityFaceControlSystem.MinPhotosRequired);
                    }
                    else
                    {
                        StatusPageMessage = Lang.Trans("Your profile is completed and is awaiting approval from the rest of the users!");
                    }

                    Response.Redirect(MatchmakerHelper.CurrentHomePage);
                }
            }
        }

        public static bool IsUserAuthorized()
        {
            var user = GetCurrentUserSession();
            if (user == null)
                return false;

            return user.IsAuthorized;
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void Log(Exception ex)
        {
            ExceptionLogger.Log(Request, ex);
        }

        protected void Page_PreInit(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_PreInit");
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_Init");
        }

        protected void Page_InitComplete(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_InitComplete");
        }

        protected void Page_PreLoad(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_PreLoad");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_Load");
        }

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_LoadComplete");
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_PreRender");
        }

        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_PreRenderComplete");
        }

        protected void Page_SaveStateComplete(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_SaveStateComplete");
        }

        protected void Page_Unload(object sender, EventArgs e)
        {
            timestamps.AddTimestamp("PageBase.Page_Unload");
            var rows = String.Join("\r\n", timestamps.Timestamps.Select(
                x => String.Format("{0} - {1}\r\n", x.Key, x.Value.ToLongTimeString().Replace(":", "_"))).ToArray());
            Session["Timestamps"] = String.Format("{0}", rows);
            //if (CurrentUserSession != null && CurrentUserSession.ToUpdateFacebookFriendsData)
            //{
            //    HttpContext.Current.Response.Write("<script type='text/javascript'>");
            //    HttpContext.Current.Response.Write("alert('Please wait while we are synching with your facebook friends');");
            //    HttpContext.Current.Response.Write("</script>");
            //}
#if DEBUG
            if (/*!Page.IsAsync && */ !Page.IsPostBack /*&& false*/)
            {
                var outStr = Session["Timestamps"].ToString();
                //HttpContext.Current.Response.Write(outStr);
            }
#endif
            HttpContext.Current.Response.Flush();
            if (CurrentUserSession != null && CurrentUserSession.ToUpdateFacebookFriendsData)
            {
                var facebook = CurrentUserSession.GetFacebookApp();
                if (facebook != null)
                {
                    var bw = new FacebookFriendsDataBackgroundWorkerClass(this, HttpContext.Current, facebook, facebook.UserId, facebook.AccessToken);
                    bw.UpdateFriendsData();
                    //UpdateFacebookFriendsdata(facebook, CurrentUserSession.FacebookID.Value, facebook.AccessToken, null);
                }
            }
        }
    }

    public class FacebookFriendsDataBackgroundWorkerClass
    {
        private PageBase _pageBase;
        private HttpContext _context;
        private TimeMeasure timestamps;
        private LoveHitchFacebookApp _facebook;
        private long _id;
        private string _accessToken;

        public FacebookFriendsDataBackgroundWorkerClass(PageBase pageBase, HttpContext context,
                                                   LoveHitchFacebookApp facebook, long id, string accessToken)
        {
            _pageBase = pageBase;
            _context = context;
            timestamps = new TimeMeasure();
            _facebook = facebook;
            _id = id;
            _accessToken = accessToken;
        }
        public void UpdateFriendsData()
        {
            IList<FacebookHelper.FacebookFriendData> friendsData = FacebookHelper.GetFriendsDataList(_context, _facebook, _id);
            timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata");
            #region obsolete 1
            //int totalFriendsCount = friendsData.Count;
            //int limit = 40;
            //var lstFriendsMutualCount = new List<MutualFriendItem>();
            //for (int offset = 0; offset < totalFriendsCount; offset += limit)
            //{
            //    var qry1 = "SELECT uid2 FROM friend WHERE uid1=me()";
            //    var qry2tmpl =
            //        "select uid, mutual_friend_count from user where mutual_friend_count > 00 and mutual_friend_count <= 10 and uid in " +
            //        "( SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset {1})";
            //    var qry2 = String.Format(qry2tmpl, limit, offset);
            //    JsonArray res = FacebookHelper.GetFqlResult(qry2);
            //    lstFriendsMutualCount =
            //        lstFriendsMutualCount.Union(
            //                              res.Select(o => new MutualFriendItem
            //                                        {
            //                                            FriendID = long.Parse((string)((JsonObject)o)["uid"]),
            //                                            MutualFriendID = ((JsonObject)o)["mutual_friend_count"] == null ? 0 : long.Parse((string)((JsonObject)o)["mutual_friend_count"])
            //                                        })).ToList();
            //}
            //var groups = lstFriendsMutualCount.GroupBy(o => o.FriendID);
            //int groupsCount = groups.Count();
            //long x = lstFriendsMutualCount.Sum(o => o.MutualFriendID);

            //time[1] = DateTime.Now;

            //var lstFacebookSessionMutualFriends = new List<MutualFriendItem>();
            //long counter = 0;
            //long limitCount = 20;
            ////long skipCount = 0;
            //int qryCount = 0;
            //for (long i = 0; i < totalFriendsCount; i += limitCount)
            //{
            //    //if (counter + lstFriendsMutualCount[i].MutualFriendID < 400 && limitCount < 80)
            //    //if (counter + lstFriendsMutualCount[i].MutualFriendID < 300 && limitCount < 30)
            //    //{
            //    //    limitCount++;
            //    //    counter += lstFriendsMutualCount[i].MutualFriendID;
            //    //}
            //    //else
            //    {
            //        qryCount++;
            //        //counter = lstFriendsMutualCount[i].MutualFriendID;
            //        var qry1tmpl =
            //            "select uid from user where mutual_friend_count > 00 and mutual_friend_count <= 10 and uid in (SELECT uid2 FROM friend WHERE uid1=me()  limit {0} offset {1}) ";
            //        var qry1 = String.Format(qry1tmpl, limitCount, i);
            //        //for (long j = 0; j < totalFriendsCount; j += 1260)
            //        {
            //            //var qry2tmpl = "SELECT uid2 FROM friend WHERE uid1=me() limit 1260 offset {0}";
            //            //var qry2 = String.Format(qry2tmpl, j);
            //            var qry2 = "SELECT uid2 FROM friend WHERE uid1=me()";
            //            //limitCount = 1;
            //            //skipCount = i-1;
            //            var qry3tmpl = "select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})";
            //            var qry3 = String.Format(qry3tmpl, qry1, qry2);
            //            JsonArray res = FacebookHelper.GetFqlResult(qry3);
            //            var res1 = res.Select(o => new MutualFriendItem
            //                                           {
            //                                               FriendID = long.Parse((string) ((JsonObject) o)["uid1"]),
            //                                               MutualFriendID =
            //                                                   long.Parse((string) ((JsonObject) o)["uid2"])
            //                                           });
            //            var res2 = res1.Select(o => new MutualFriendItem
            //                                            {
            //                                                FriendID = o.MutualFriendID,
            //                                                MutualFriendID = o.FriendID
            //                                            });
            //            lstFacebookSessionMutualFriends = lstFacebookSessionMutualFriends.Union(res1).Union(res2).ToList();
            //        }
            //    }
            //}
            //time[2] = DateTime.Now;
            //time[2] = DateTime.Now;

            ////int maxIter = totalFriendsCount/500 + (totalFriendsCount%500 != 0 ? 1 : 0);
            //var lstFacebookSessionMutualFriends = new List<MutualFriendItem>();
            //var qry0 = "SELECT uid2 FROM friend WHERE uid1=me()";
            //int limit = 600;
            //int step = 10;
            //for (int i = 1; i < 140; i += step)
            //{
            //    for (int offset = 0; offset < totalFriendsCount; offset += limit)
            //    {
            //        var qry1tmpl = "SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset {1}";
            //        var qry1 = String.Format(qry1tmpl, limit, offset);
            //        var qry2tmpl = "select uid from user where mutual_friend_count >= {0} and mutual_friend_count < {1} and uid in " +
            //                       "( SELECT uid1 FROM friend WHERE uid1 IN ({2}) )";
            //        var qry2_a = String.Format(qry2tmpl, i, i+step, qry1);
            //        var qry2_b = "select uid from user where mutual_friend_count >= 1 and mutual_friend_count < 100 and uid in " +
            //                       "( SELECT uid2 FROM friend WHERE uid1=me() )";
            //        var qry3tmpl = "select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})";
            //        var qry3 = String.Format(qry3tmpl, qry2_a, qry2_b);
            //        JsonArray res = FacebookHelper.GetFqlResult(qry3);
            //        lstFacebookSessionMutualFriends =
            //            lstFacebookSessionMutualFriends.Union(
            //                res.Select(o => new MutualFriendItem
            //                                    {
            //                                        FriendID = long.Parse((string) ((JsonObject) o)["uid1"]),
            //                                        MutualFriendID = long.Parse((string) ((JsonObject) o)["uid2"])
            //                                    })).ToList();
            //    }
            //    step += 10;
            //    limit -= 100;
            //}
            //var qry00 = String.Format(qry0tmpl,1000,0);
            //var qry01 = String.Format(qry0tmpl,1000,1000);
            //var qry02 = String.Format(qry0tmpl,1000,2000);
            //var qry03 = String.Format(qry0tmpl,1000,3000);
            //var qry04 = String.Format(qry0tmpl,1000,4000);
            //var qry5000 = String.Format(qry0tmpl,5000,0);
            //var qry2tmpl = String.Format(
            //    "select uid,mutual_friend_count from user where mutual_friend_count > 0 and mutual_friend_count < 20 and uid in " +
            //           "( SELECT uid1 FROM friend WHERE uid1 IN ({0}) )");
            //var qry20 = String.Format(qry2tmpl, qry00);
            //var qry21 = String.Format(qry2tmpl, qry01);
            //var qry22 = String.Format(qry2tmpl, qry02);
            //var qry23 = String.Format(qry2tmpl, qry03);
            //var qry24 = String.Format(qry2tmpl, qry04);
            //var qry5 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry20, qry5000);
            //JsonArray res1 = FacebookHelper.GetFqlResult(qry5);
            //var qry6 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry21, qry5000);
            //JsonArray res2 = FacebookHelper.GetFqlResult(qry6);
            //var qry7 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry22, qry5000);
            //JsonArray res3 = FacebookHelper.GetFqlResult(qry7);
            //var qry8 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry23, qry5000);
            //JsonArray res4 = FacebookHelper.GetFqlResult(qry8);
            //var qry9 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry24, qry5000);
            //JsonArray res5 = FacebookHelper.GetFqlResult(qry9);
            //var qry10 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry0);
            //JsonArray res6 = FacebookHelper.GetFqlResult(qry10);
            //var qry11 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry1);
            //JsonArray res7 = FacebookHelper.GetFqlResult(qry11);

            //var qry3 = String.Format(
            //    "select uid from user where friend_count > 0 and friend_count < 1000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry2000 = String.Format(
            //    "select uid from user where friend_count >= 1000 and friend_count < 2000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry3000 = String.Format(
            //    "select uid from user where friend_count >= 2000 and friend_count < 3000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry4000 = String.Format(
            //    "select uid from user where friend_count >= 3000 and friend_count < 4000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry5000 = String.Format(
            //    "select uid from user where friend_count >= 4000 and friend_count < 6000 and uid in " +
            //           "(SELECT uid1 FROM friend " +
            //           " WHERE uid1 IN ({0}) " +
            //           " AND   uid2 IN ({0})" +
            //           ")", qry0);
            //var qry5 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1000, qry0);
            //JsonArray res1 = FacebookHelper.GetFqlResult(qry5);
            //var qry6 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry2000, qry0);
            //JsonArray res2 = FacebookHelper.GetFqlResult(qry6);
            //var qry7 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry3000, qry0);
            //JsonArray res3 = FacebookHelper.GetFqlResult(qry7);
            //var qry8 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry4000, qry0);
            //JsonArray res4 = FacebookHelper.GetFqlResult(qry8);
            //var qry9 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry5000, qry0);
            //JsonArray res5 = FacebookHelper.GetFqlResult(qry9);
            //var qry10 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry0);
            //JsonArray res6 = FacebookHelper.GetFqlResult(qry10);
            //var qry11 = String.Format("select uid1,uid2 from friend where uid1 in ({0}) and uid2 in ({1})", qry1, qry1);
            //JsonArray res7 = FacebookHelper.GetFqlResult(qry11);
            //int count = res1.Count + res2.Count + res3.Count + res4.Count + res5.Count;
            //JsonArray friendsWithMutualFriends = FacebookHelper.GetFqlResult(qry1);

            //int itr = 0;
            //var qryTemplate = "SELECT uid1, uid2 FROM friend " +
            //                  " WHERE (uid1 IN (SELECT uid2 FROM friend WHERE uid1=me()) or uid1 IN (SELECT uid1 FROM friend WHERE uid2=me())" +
            //                  " AND   (uid2 IN (SELECT uid2 FROM friend WHERE uid1=me()) or uid2 IN (SELECT uid1 FROM friend WHERE uid2=me())";
            //                  //" limit 5000 offset {0} ";
            //var qry = String.Format(qryTemplate, 0);
            #endregion obsolete 1

            string[] fbFriendsIds =
                friendsData.Select(f => f.UserId.ToString())
                           .Take(300).ToArray();

            bool loadOnlyWithMutualFriends = true;
            #region obsolete 2
            //try
            //{
            //    var qry = "SELECT uid1, uid2 FROM friend " +
            //                " WHERE uid1 IN (SELECT uid2 FROM friend WHERE uid1=me()) " +
            //                " AND   uid2 IN (SELECT uid2 FROM friend WHERE uid1=me())";
            //    //JsonArray mutualFriendsByQuery = FacebookHelper.GetFqlResult(facebook, qry);

            //    //lstFacebookSessionMutualFriends = mutualFriendsByQuery
            //    //                                        .Select(o => new MutualFriendItem
            //    //                                        {
            //    //                                            FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
            //    //                                            MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
            //    //                                        })
            //    //                                    .ToList();
            //    FacebookHelper.GetAsyncFqlResult(facebook, qry, FetchFriends_FacebookResultCallback, null);
            //    while (mutualFriendsQueryCount.Counter > 0) { }
            //    loadOnlyWithMutualFriends = !Classes.User.IsUsernameTaken(lstFacebookSessionMutualFriends.Last().FriendID.ToString());
            //}
            //catch (FacebookApiException exc)
            #endregion obsolete 2
            {
                var maxFriends = fbFriendsIds.Length;
                var limit = 200 / (2 + (friendsData.Count / 500));
                var qryTmpl = "SELECT uid1, uid2 FROM friend " +
                    //" WHERE uid1 IN (SELECT uid2 FROM friend WHERE uid1=me() limit {0} offset  {1} ) " +
                            " WHERE uid1 IN ( {0} ) " +
                            " AND   uid2 IN (SELECT uid2 FROM friend WHERE uid1=me())";
                for (int offset = 0; offset < maxFriends; offset += limit)
                {
                    var friendsList = fbFriendsIds.Skip(offset).Take(limit).ToList();
                    var friendsSeperated = String.Join(",", fbFriendsIds, offset, fbFriendsIds.Length - offset);
                    var qry = String.Format(qryTmpl, friendsSeperated);

                    lock (mutualFriendsQueryCount)
                    {
                        mutualFriendsQueryCount.Counter++;
                    }
                    FacebookHelper.GetAsyncFqlResult(_facebook, qry, FetchFriends_FacebookResultCallback, null);
                    #region obsolete 3
                    //JsonArray mutualFriendsByQuery = FacebookHelper.GetFqlResult(facebook, qry);

                    //lstFacebookSessionMutualFriends = lstFacebookSessionMutualFriends.Union(
                    //                                      mutualFriendsByQuery
                    //                                            .Select(o => new MutualFriendItem
                    //                                            {
                    //                                                FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
                    //                                                MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
                    //                                            })).ToList();
                    #endregion obsolete 3
                }
                timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata start FacebookHelper.GetAsyncFqlResult");
                while (mutualFriendsQueryCount.Counter > 0) { Thread.Sleep(100); }
                timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata end FacebookHelper.GetAsyncFqlResult");
                loadOnlyWithMutualFriends = !Classes.User.IsUsernameTaken(lstFacebookSessionMutualFriends.Last().FriendID.ToString());
                #region obsolete 4
                //IDictionary<string, object> parameters = new Dictionary<string, object>();
                //Global.Logger.LogWarning(exc);
                //StatusPageMessage = exc.Message;
                //parameters["next"] = Config.Urls.Home + "/ShowStatus.aspx";
                //Response.Redirect("ShowStatus.aspx");
                //return;
                #endregion obsolete 4
            }
            #region obsolete 5
            //while (mutualFriendsByQuery.Count > 4999)
            //{
            //    itr++;
            //    qry = String.Format(qryTemplate, 5000*itr);
            //    mutualFriendsByQuery = FacebookHelper.GetFqlResult(qry);
            //    lstFacebookSessionMutualFriends =
            //        lstFacebookSessionMutualFriends
            //            .Union(mutualFriendsByQuery
            //                    .Select(o => new MutualFriendItem
            //                    {
            //                        FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
            //                        MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
            //                    }))
            //                    .ToList();
            //}
            #endregion obsolete 5
            var dic1 = _context.GetFacebookMutualFriendsDic(); //(Dictionary<long, List<MutualFriendItem>>)Application["FacebookMutualFriendsDic"];
            dic1[_id] = lstFacebookSessionMutualFriends;
            //FacebookHelper.AddFriends(CurrentUserSession, loadOnlyWithMutualFriends ? 0 : 40, HttpContext.Current, null);
            FacebookHelper.AddFriends(_pageBase.CurrentUserSession, -1, _context, null);
            timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata after FacebookHelper.AddFriends");
            var dic2 = (Dictionary<string, LovehitchBackgroundWorker>)Global.AppBackgroundWorkersDic;
            if (_context.Session != null && _pageBase.CurrentUserSession.FacebookID.HasValue)
            {
                if (dic2 != null)
                {
                    //var facebook = CurrentUserSession.GetFacebookApp();
                    var bgWorker1 =
                        (AddNewFacebookFriendsBackgroundWorker)
                           dic2["AspNetDating.Classes.AddNewFacebookFriendsBackgroundWorker"];

                    if (bgWorker1 != null && _facebook != null && _pageBase.CurrentUserSession.FacebookID.HasValue)
                    {
                        _facebook.Session.UserId = _pageBase.CurrentUserSession.FacebookID.Value;
                        lock (bgWorker1.FacebookAppList)
                        {
                            if (!bgWorker1.FacebookAppList.Contains(_facebook))
                                bgWorker1.FacebookAppList.Add(_facebook);
                        }
                    }
                }
            }
            timestamps.AddTimestamp("PageBase.UpdateFacebookFriendsdata after bgWorker1.FacebookAppList.Add(facebook)");
            //_pageBase.CurrentUserSession.ToUpdateFacebookFriendsData = false;
        }
        public void FetchFriends_FacebookResultCallback(FacebookAsyncResult asyncResult)
        {
            if (asyncResult.IsCompleted && asyncResult.Error == null && asyncResult.Result != null)
            {
                lock (lstFacebookSessionMutualFriends)
                {
                    lstFacebookSessionMutualFriends =
                        lstFacebookSessionMutualFriends.Union(
                            ((JsonArray)((JsonObject)asyncResult.Result)["data"])
                                .Select(o => new MutualFriendItem
                                {
                                    FriendID = long.Parse((string)((JsonObject)o)["uid1"]),
                                    MutualFriendID = long.Parse((string)((JsonObject)o)["uid2"])
                                })).ToList();
                }
            }

            lock (mutualFriendsQueryCount)
            {
                mutualFriendsQueryCount.Counter--;
            }

            //System.Diagnostics.Debug.WriteLine(asyncResult.Result);
        }
        private List<MutualFriendItem> lstFacebookSessionMutualFriends = new List<MutualFriendItem>();

        class MutualFriendsQueryCounterClass
        {
            public MutualFriendsQueryCounterClass()
            {
                Counter = 0;
            }
            public int Counter;
        }

        private MutualFriendsQueryCounterClass mutualFriendsQueryCount = new MutualFriendsQueryCounterClass();

    }
}