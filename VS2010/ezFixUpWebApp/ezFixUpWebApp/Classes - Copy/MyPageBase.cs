/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
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
using eStreamBG.Dating;
using Facebook;


namespace ezFixUp.Classes
{
    /// <summary>
    /// This class is the base of all ezFixUp pages
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
                usernames = Data.User.AuthorizeByFacebookID(id);
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

            Data.Plugins.Events.OnPreInitInvoke(this, e);
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
            //TODO - run in inherited class
            //CommunityFaceControlValidation();

            /*
            if (Config.Misc.EnableIntegratedIM && IsUserAuthorized() && !DoNotRegisterIMJavascript)
            {
                ClientScript.RegisterClientScriptInclude("IntegratedIM", "IM/functions.js");
            }
            */

            Data.Plugins.Events.OnInitInvoke(this, e);
            base.OnInit(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.InitComplete"/> event after page initialization.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInitComplete(EventArgs e)
        {
            Data.Plugins.Events.OnInitCompleteInvoke(this, e);
            base.OnInitComplete(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreLoad"/> event after postback data is loaded into the page server controls but before the <see cref="M:System.Web.UI.Control.OnLoad(System.EventArgs)"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreLoad(EventArgs e)
        {
            Data.Plugins.Events.OnPreLoadInvoke(this, e);
            base.OnPreLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Load"/> event.
        /// </summary>
        /// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            Data.Plugins.Events.OnLoadInvoke(this, e);
            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.LoadComplete"/> event at the end of the page load stage.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoadComplete(EventArgs e)
        {
            Data.Plugins.Events.OnLoadCompleteInvoke(this, e);
            base.OnLoadComplete(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            Data.Plugins.Events.OnPreRenderInvoke(this, e);
            base.OnPreRender(e);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Page.PreRenderComplete"/> event after the <see cref="M:System.Web.UI.Page.OnPreRenderComplete(System.EventArgs)"/> event and before the page is rendered.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnPreRenderComplete(EventArgs e)
        {
            Data.Plugins.Events.OnPreRenderCompleteInvoke(this, e);
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
                    bgDic["ezFixUp.Data.UpdateFacebookFriendsDataBackgroundWorker"];

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

        //private void CommunityFaceControlValidation()
        //{
        //    if (CurrentUserSession != null)
        //    {
        //        if (CurrentUserSession.IsAdmin())
        //        {
        //            return;
        //        }

        //        if (Config.CommunityFaceControlSystem.EnableCommunityFaceControl
        //            && !CurrentUserSession.FaceControlApproved
        //            && !(Page is ManageMemberProfile) && !(Page is EditSalutePhoto) && !(Page is ShowStatus))
        //        {
        //            if (!CurrentUserSession.HasProfile)
        //            {
        //                StatusPageLinkSkindId = "";
        //                StatusPageLinkText = "Manage Profile";
        //                StatusPageLinkURL = Config.Urls.Home + "/manageprofile.aspx";
        //                StatusPageMessage = Lang.Trans("You must complete your profile in order to continue!");
        //            }
        //            else if (Photo.Search(-1, CurrentUserSession.Username, null, null, null, null, null).Length < Config.CommunityFaceControlSystem.MinPhotosRequired)
        //            {
        //                StatusPageLinkSkindId = "UploadPhotos";
        //                StatusPageLinkText = Lang.Trans("Upload Photos");
        //                StatusPageLinkURL = "~/manageprofile.aspx?sel=photos";
        //                StatusPageMessage =
        //                    String.Format(Lang.Trans("You must upload at least {0} photos in order to continue!"),
        //                                  Config.CommunityFaceControlSystem.MinPhotosRequired);
        //            }
        //            else
        //            {
        //                StatusPageMessage = Lang.Trans("Your profile is completed and is awaiting approval from the rest of the users!");
        //            }

        //            Response.Redirect(MatchmakerHelper.CurrentHomePage);
        //        }
        //    }
        //}

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
}