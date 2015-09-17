/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Microsoft.Practices.Unity;
using ezFixUp.Classes;
using ezFixUp.Classes.ezFixUpParallel;
using BitFactory.Logging;
using Estream.AjaxChat.Classes;
using Config = ezFixUp.Classes.Config;
using Language = ezFixUp.Classes.Language;
using Translation = ezFixUp.Classes.Translation;

namespace ezFixUp
{
    public partial class Global : HttpApplication, IHttpApplicationUserAdapter, IHttpApplicationSupportAvatars,
                          IHttpApplicationSupportProfiles, IHttpApplicationChatRoomProvider,
                          IHttpApplicationConnectionStringProvider, IHttpApplicationSupportPhotos
    {
        private static readonly CompositeLogger logger = new CompositeLogger();

        public static IUnityContainer GlobalContainer =  new UnityContainer();
        public static IServiceProvider GlobalService = EnterpriseLibraryContainer.CreateDefaultContainer();

        public static Dictionary<long, ezFixUpFacebookApp> FacebookAppDic
        {
            get
            {
                if (CurrentApplicationState["FacebookAppDic"] == null)
                {
                    CurrentApplicationState["FacebookAppDic"] = new Dictionary<long, ezFixUpFacebookApp>();
                }
                return CurrentApplicationState["FacebookAppDic"] as Dictionary<long, ezFixUpFacebookApp>;
            }
        }

        public static CompositeLogger Logger
        {
            get { return logger; }
        }
        private void InitLogger()
        {
            // create the file logger
            Logger fileLogger = new FileLogger(
                Server.MapPath(@"~\Logs\logfile.log"));

#if DEBUG
            fileLogger.SeverityThreshold = LogSeverity.Info;
#else
            fileLogger.SeverityThreshold = LogSeverity.Status;     
#endif

            Logger.AddLogger("File", fileLogger);
        }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        public Global()
        {
            InitializeComponent();
        }

        protected void Application_Start(Object sender, EventArgs e)
        {
            //var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            //section.Configure(GlobalContainer, "containerOne");
            GlobalContainer.AddNewExtension<EnterpriseLibraryCoreExtension>();
            GlobalContainer.RegisterType<Database, SqlDatabase>();
            
            //GlobalContainer.AddNewExtension<EnterpriseLibraryBlockExtension>(); 
            //Microsoft.Practices.EnterpriseLibrary.Data.Database db = Microsoft.Practices.EnterpriseLibrary.Data.DatabaseFactory.CreateDatabase();
            

            InitLogger();
            Logger.LogStatus("Application Started");
            CurrentApplicationState = HttpContext.Current.Application;
            //Application["UsersOnline"] = new List<int>();
            Global.AppUsersOnline = new List<int>();

            Application["EtagTimestamp"] = DateTime.Now.Ticks.ToString();

            if (Config.AdminSettings.ExecuteTimers)
            {
                EmailQueue.InitializeMailerTimer();
                Payments.InitializePaymentsTimer();
                BirthdayEmails.InitializeMailerTimer();
                FriendBirthdayEmails.InitializeMailerTimer();
                VisitSiteReminderEmails.InitializeMailerTimer();
                Maintenance.InitializeTimers();
                SavedSearchesEmailMatches.InitializeMailerTimer();
                InactiveGroups.InitializeTimer();
                HourlyStats.InitializeMailerTimer();
                //GoogleSitemaps.InitializeTimers();
                MessagesSandBox.InitializeMailerTimer();
                Event.InitializeEventsCleanupTimer();
                IPToCountry.InitializeUpdateIpDefinitionFilesTimer();
                ScheduledAnnouncementEmails.InitializeMailerTimer();

                if (Config.DebugSettings.EnableTimers)
                    FacebookPhotosWorker.InitializeWorkerTimer();
            }

            Classes.Plugins.InitializePlugins();

            for (int i = 0; i < 10; i++)
            {
                string relativePath = String.Format("~{0}/{1}", Config.Urls.ImagesCacheHome.Replace(Config.Urls.Home, ""), i);
                string cacheDirUrl = Server.MapPath(relativePath);
                string[] files = Directory.GetFiles(cacheDirUrl);
                foreach (string filename in files)
                {
                    try { File.Delete(filename); }
                    catch { }
                }
            }

            var countries = Config.Users.GetCountriesXml(HttpContext.Current);

            var languges = Language.FetchAll();
            var keys = Translation.FetchTranslationKeys(false);
            foreach (var language in languges)
            {
                foreach (var key in keys)
                {
                    Translation.FetchTranslation(language.Id, key, false);
                }
            }
            var topics = ProfileTopic.Fetch().ToList();
            foreach (ProfileTopic profileTopic in topics)
            {
                var questions = profileTopic.FetchQuestions().ToList();
                foreach (ProfileQuestion profileQuestion in questions)
                {
                    var a = profileQuestion.FetchAnswers();
                    var c = profileQuestion.FetchChoices();
                }
            }
            var gifts = GiftType.Fetch(true, null, GiftType.eSortColumn.None);
            HttpContext.Current.Cache.Add("ActiveGiftTypes", gifts,
                null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(20),
                CacheItemPriority.NotRemovable, null
                );
            Global.AppPreloadGiftsImages =
                gifts.Select(gift => GiftImageHandler.CreateImageUrl(gift.ID, 80, 80)).ToList();

            //TODO: remove user's friends list in logout & session_end()
            Global.AppFacebookFriendsDic = new Dictionary<long, List<FacebookHelper.FacebookFriendData>>();
            Global.AppFacebookMutualFriendsDic = new Dictionary<long, List<MutualFriendItem>>();
            //SqlDependency.Start(ConfigurationManager.ConnectionStrings["ezFixUpDB"].ConnectionString);
            Global.AppBackgroundWorkersDic = new Dictionary<string, ezFixUpBackgroundWorker>();
            var bgWorker0 = new UpdateFacebookFriendsDataBackgroundWorker(HttpContext.Current);
            ezFixUpBackgroundWorker.StartBackgroundWorker(bgWorker0);
            var bgWorker1 = new AddNewFacebookFriendsBackgroundWorker(HttpContext.Current);
            ezFixUpBackgroundWorker.StartBackgroundWorker(bgWorker1);
            var bgWorker2 = new FetchDbImageToCacheDirectoryBackgroundWorker(HttpContext.Current);
            ezFixUpBackgroundWorker.StartBackgroundWorker(bgWorker2);
            //ezFixUpBackgroundWorker.StartBackgroundWorker(new LoadAndSaveFacebookImageBackgroundWorker());
        }
        protected void Session_Start(Object sender, EventArgs e)
        {
            Config.Directories.Home = Server.MapPath("~");
            Response.SetCookie(new HttpCookie("sid", Session.SessionID));
            if (!UserStateInstancesDic.ContainsKey(Session.SessionID))
                Global.UserStateInstancesDic.Add(Session.SessionID, new Dictionary<string, object>());
            MatchmakerHelper.SetMatchmakerState(false);
            Global.GetSessionState()["LanguageId"] = 1;
            Global.GetSessionState()["MatchmakingRequestedDic"] = new Dictionary<string, List<string>>();
            Global.GetSessionState()["SearchResults_GridMode"] = false;
            FacebookHelper.SessionClear("default.aspx");
            Global.GetSessionState()["SessionTimeout"] = 10;
            Global.GetSessionState()["AsyncWorkList"] = new List<IWorkThreadClass>();
            Random r = new Random((int)(DateTime.Now.Ticks % int.MaxValue));
            //((IList<int>)Application["UsersOnline"]).Add(r.Next(5, 15));
            Global.AppUsersOnline.Add(r.Next(5, 15));
        }
        // HTTP Aplication Pipline Events Order:
        //1. Validate the request, which examines the information sent by the browser and determines whether it contains potentially malicious markup. For more information, see ValidateRequest and Script Exploits Overview.
        //2. Perform URL mapping, if any URLs have been configured in the UrlMappingsSection section of the Web.config file.
        //3. Raise the BeginRequest event.
        //4. Raise the AuthenticateRequest event.
        //5. Raise the PostAuthenticateRequest event.
        //6. Raise the AuthorizeRequest event.
        //7. Raise the PostAuthorizeRequest event.
        //8. Raise the ResolveRequestCache event.
        //9. Raise the PostResolveRequestCache event.
        //10. Raise the MapRequestHandler event. An appropriate handler is selected based on the file-name extension of the requested resource. The handler can be a native-code module such as the IIS 7.0 StaticFileModule or a managed-code module such as the PageHandlerFactory class (which handles .aspx files). 
        //11. Raise the PostMapRequestHandler event.
        //12. Raise the AcquireRequestState event.
        //13. Raise the PostAcquireRequestState event.
        //14. Raise the PreRequestHandlerExecute event.
        //15. Call the ProcessRequest method (or the asynchronous version IHttpAsyncHandler.BeginProcessRequest) of the appropriate IHttpHandler class for the request. For example, if the request is for a page, the current page instance handles the request.
        //16. Raise the PostRequestHandlerExecute event.
        //17. Raise the ReleaseRequestState event.
        //18. Raise the PostReleaseRequestState event.
        //19. Perform response filtering if the Filter property is defined.
        //20. Raise the UpdateRequestCache event.
        //20. Raise the PostUpdateRequestCache event.
        //20. Raise the LogRequest event.
        //20. Raise the PostLogRequest event.
        //20. Raise the EndRequest event.
        //20. Raise the PreSendRequestHeaders event.
        //20. Raise the PreSendRequestContent event.



        void Global_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            Stream prevUncompressedStream = app.Response.Filter;

            //if (!(app.Context.CurrentHandler is Page || app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler")) return;
            if (app.Request["HTTP_X_MICROSOFTAJAX"] != null)
                return;

            if (acceptEncoding == null || acceptEncoding.Length == 0)
                return;
            //var headers = app.Response.Headers;
            //if (headers["Vary"] != null && !headers["Vary"].Contains("Accept-Encoding"))
            app.Response.AppendHeader("Vary", "Accept-Encoding");

            acceptEncoding = acceptEncoding.ToLower();

            if (acceptEncoding.Contains("gzip"))
            {
                // gzip
                app.Response.Filter = new GZipStream(prevUncompressedStream,
                    CompressionMode.Compress);
                //if (headers["Content-Encoding"] != null && !headers["Content-Encoding"].Contains("gzip"))
                app.Response.AppendHeader("Content-Encoding", "gzip");
            }
            else if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
            {
                // defalte
                app.Response.Filter = new DeflateStream(prevUncompressedStream,
                    CompressionMode.Compress);
                //if (headers["Content-Encoding"] != null && !headers["Content-Encoding"].Contains("deflate"))
                app.Response.AppendHeader("Content-Encoding", "deflate");
            }
        }


        private void Global_PostReleaseRequestState(object sender, EventArgs e)
        {
            //Response.AppendHeader("Vary", "Accept-Encoding");

            //string contentType = Response.ContentType;

            ////if (contentType.Contains("text/") || contentType.Contains("javascript") //|| contentType.Contains( "text/css")
            //    //|| contentType.Contains( "application/x-javascript") //|| contentType.Contains( "text/javascript") //use when external ajax control toolkit scripts(CDN)
            ////    )
            //{


            //    string acceptEncoding =
            //        Request.Headers["Accept-Encoding"];

            //    if (acceptEncoding != null)
            //    {
            //        if (acceptEncoding.Contains("gzip"))
            //        {
            //            Response.Filter = new GZipStream(
            //                Response.Filter, CompressionMode.Compress);
            //            Response.AppendHeader(
            //                "Content-Encoding", "gzip");
            //        }
            //        else if (acceptEncoding.Contains("deflate"))
            //        {
            //            Response.Filter = new DeflateStream(
            //                Response.Filter, CompressionMode.Compress);
            //            Response.AppendHeader(
            //                "Content-Encoding", "deflate");
            //        }
            //    }
            //}






            Response.Cache.VaryByHeaders["Accept-Encoding"] = true;



            //HttpApplication app = sender as HttpApplication;
            //if (app == null) return;

            //string acceptEncoding = app.Request.Headers["Accept-Encoding"];
            //Stream prevUncompressedStream = app.Response.Filter;

            //if (!(app.Context.CurrentHandler is Page ||
            //    app.Context.CurrentHandler.GetType().Name == "SyncSessionlessHandler") ||
            //    app.Request["HTTP_X_MICROSOFTAJAX"] != null)
            //    return;

            //if (acceptEncoding == null || acceptEncoding.Length == 0)
            //    return;

            //acceptEncoding = acceptEncoding.ToLower();

            //if (acceptEncoding.Contains("deflate") || acceptEncoding == "*")
            //{
            //    // defalte
            //    app.Response.Filter = new DeflateStream(prevUncompressedStream,
            //        CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding", "deflate");
            //}
            //else if (acceptEncoding.Contains("gzip"))
            //{
            //    // gzip
            //    app.Response.Filter = new GZipStream(prevUncompressedStream,
            //        CompressionMode.Compress);
            //    app.Response.AppendHeader("Content-Encoding", "gzip");
            //}

            //TimeSpan cacheDuration = TimeSpan.FromDays(365);
            //FieldInfo maxAge = Context.Response.Cache.GetType().GetField("_maxAge",
            //    BindingFlags.Instance | BindingFlags.NonPublic);
            //maxAge.SetValue(Context.Response.Cache, cacheDuration);

            //Context.Response.Cache.SetCacheability(HttpCacheability.Public);
            //Context.Response.Cache.SetExpires(DateTime.Now.Add(cacheDuration));
            ////Context.Response.Cache.AppendCacheExtension("must-revalidate, proxy-revalidate");
            //Response.Buffer = true;
            //Response.BufferOutput = true;

            TimeSpan cacheDuration = TimeSpan.Zero;
            string contentType = Response.ContentType;

            switch (contentType)
            {
                case "image/png":
                case "image/jpeg":
                case "image/gif":
                case "application/octet-stream":
                    //cacheDuration = TimeSpan.FromDays(1);
                    cacheDuration = TimeSpan.FromSeconds(300);
                    break;
                case "application/javascript":
                case "application/x-javascript":
                    //cacheDuration = TimeSpan.FromDays(7);
                    cacheDuration = TimeSpan.FromSeconds(3660);
                    break;
                //case "text/css":
                case "text/javascript":
                    //cacheDuration = TimeSpan.FromDays(7);
                    cacheDuration = TimeSpan.FromSeconds(3600);
                    //if (!Request.RawUrl.Contains(".axd"))
                    //{
                    //    Response.Cache.VaryByHeaders["Accept-Encoding"] = true;
                    //    string acceptEncoding = Request.Headers["Accept-Encoding"];
                    //    if (acceptEncoding != null)
                    //    {
                    //        if (acceptEncoding.Contains("gzip"))
                    //        {
                    //            Response.Filter = new GZipStream(Response.Filter, CompressionMode.Compress);
                    //            Response.AppendHeader("Content-Encoding", "gzip");
                    //        }
                    //        else if (acceptEncoding.Contains("deflate"))
                    //        {
                    //            Response.Filter = new DeflateStream(Response.Filter, CompressionMode.Compress);
                    //            Response.AppendHeader("Content-Encoding", "deflate");
                    //        }
                    //    }
                    //}
                    break;
                case "application/json":
                case "text/xml":
                    cacheDuration = TimeSpan.FromSeconds(30);
                    break;
                case "text/html":
                case "text/plain":
                default:
                    cacheDuration = TimeSpan.FromSeconds(30);
                    break;
            }
            Context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);
            Context.Response.Cache.SetExpires(DateTime.Now.Add(cacheDuration));
            Context.Response.Cache.SetMaxAge(cacheDuration);
            Context.Response.Cache.SetProxyMaxAge(cacheDuration);
            Context.Response.Cache.SetNoTransforms();
            Context.Response.Cache.SetValidUntilExpires(true);
            Context.Response.Cache.AppendCacheExtension(
                   "must-revalidate, proxy-revalidate");

            FieldInfo maxAge = Context.Response.Cache.GetType().GetField("_maxAge",
                BindingFlags.Instance | BindingFlags.NonPublic);
            maxAge.SetValue(Context.Response.Cache, cacheDuration);

            //HttpWorkerRequest.
            //var response = HttpContext.Current.Response;
            //var acceptEncoding = HttpContext.Current.Request.Headers["Accept-Encoding"];

            //if ((!String.IsNullOrEmpty(acceptEncoding) && acceptEncoding.Contains("gzip"))
            //    || acceptEncoding.Contains("deflate"))
            //{
            //    if (acceptEncoding.Contains("deflate"))
            //    {
            //        response.Filter = new
            //            System.IO.Compression.DeflateStream(Response.Filter,
            //                                                System.IO.Compression.CompressionMode.Compress);
            //        response.AppendHeader("Content-Encoding", "deflate");
            //    }
            //    else
            //    {
            //        response.Filter = new
            //            System.IO.Compression.GZipStream(Response.Filter, System.IO.Compression.CompressionMode.Compress);
            //        response.AppendHeader("Content-Encoding", "gzip");
            //    }
            //}
            //response.AppendHeader("Vary", "Content-Encoding");





            //Response.AppendHeader("Connection", "Keep-Alive");
        }
        protected void Global_PreSendRequestHeaders(object sender, EventArgs e)
        {
            //Request.Headers["Connection"] = "Keep-Alive";


        }

        public static void AddCacheItem(string tableName, string cacheKey, object value)
        {
            SqlCacheDependency tableDependency = null;
            Cache DataCache = HttpRuntime.Cache;
            try
            {
                tableDependency = new SqlCacheDependency("ezFixUpDB", tableName);
            }

            // Handle the DatabaseNotEnabledForNotificationException with 
            // a call to the SqlCacheDependencyAdmin.EnableNotifications method. 
            catch (DatabaseNotEnabledForNotificationException exDBDis)
            {
                try
                {
                    SqlCacheDependencyAdmin.EnableNotifications("ezFixUpDB");
                }

                // If the database does not have permissions set for creating tables, 
                // the UnauthorizedAccessException is thrown. Handle it by redirecting 
                // to an error page. 
                catch (UnauthorizedAccessException exPerm)
                {
                }
            }

            // Handle the TableNotEnabledForNotificationException with 
            // a call to the SqlCacheDependencyAdmin.EnableTableForNotifications method. 
            catch (TableNotEnabledForNotificationException exTabDis)
            {
                try
                {
                    SqlCacheDependencyAdmin.EnableTableForNotifications("ezFixUpDB", tableName);
                }

                // If a SqlException is thrown, redirect to an error page. 
                catch (SqlException exc)
                {
                }
            }

            // If all the other code is successful, add MySource to the Cache 
            // with a dependency on SqlDep. If the Categories table changes, 
            // MySource will be removed from the Cache. Then generate a message 
            // that the data is newly created and added to the cache. 
            finally
            {
                DataCache.Insert(tableName, value, tableDependency,
                Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration);
            }
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            if (Config.SEO.EnableUrlRewriting)
                UrlRewrite.Process();

            Context.Items.Add("PerformanceLogGuid", PerformanceTimers.LogStartRequest());

        }

        private static HttpApplicationState _currentApplicationState;
        public static HttpApplicationState CurrentApplicationState
        {
            set { _currentApplicationState = value; }
            get { return _currentApplicationState; }
        }


        public static List<string> AppPreloadGiftsImages
        {
            get { return (List<string>)CurrentApplicationState["PreloadGiftsImages"]; }
            set { CurrentApplicationState["PreloadGiftsImages"] = value; }
        }
        public static List<int> AppUsersOnline
        {
            get { return (List<int>)CurrentApplicationState["UsersOnline"]; }
            set { CurrentApplicationState["UsersOnline"] = value; }
        }
        public static Dictionary<string, ezFixUpBackgroundWorker> AppBackgroundWorkersDic
        {
            get { return (Dictionary<string, ezFixUpBackgroundWorker>)CurrentApplicationState["BackgroundWorkersDic"]; }
            set { CurrentApplicationState["BackgroundWorkersDic"] = value; }
        }
        public static Dictionary<long, List<FacebookHelper.FacebookFriendData>> AppFacebookFriendsDic
        {
            get { return (Dictionary<long, List<FacebookHelper.FacebookFriendData>>)CurrentApplicationState["FacebookFriendsDic"]; }
            set { CurrentApplicationState["FacebookFriendsDic"] = value; }
        }
        public static Dictionary<long, List<MutualFriendItem>> AppFacebookMutualFriendsDic
        {
            get { return (Dictionary<long, List<MutualFriendItem>>)CurrentApplicationState["FacebookMutualFriendsDic"]; }
            set { CurrentApplicationState["FacebookMutualFriendsDic"] = value; }
        }
        //public static List<int> AppUsersOnline
        //{
        //    get { return (List<int>)CurrentApplicationState["UsersOnline"]; }
        //    set { CurrentApplicationState["UsersOnline"] = value; }
        //}

        protected void Application_EndRequest(Object sender, EventArgs e)
        {
            try
            {
                //SqlDependency.Stop(ConfigurationManager.ConnectionStrings["ezFixUpDB"].ConnectionString);

                var index = Request.Url.Segments.Length - 1;
                var filename = Request.Url.Segments[index].ToLower();
                if (filename.Contains("?"))
                    filename = filename.Split('?')[0];
                if (Context.Items.Contains("PerformanceLogGuid"))
                    PerformanceTimers.LogEndRequest((Guid)Context.Items["PerformanceLogGuid"],
                                                    filename);
            }
            catch (Exception exception)
            {
                var currentFilename = Request.CurrentExecutionFilePath.ToLower();
                if (Context.Items.Contains("PerformanceLogGuid"))
                    PerformanceTimers.LogEndRequest((Guid)Context.Items["PerformanceLogGuid"],
                                                    currentFilename);
            }

        }

        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
        }

        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex == null)
                return;

            if (ex.GetType() == typeof(ViewStateException) ||
                (ex.GetType() == typeof(HttpException) &&
                 ex.InnerException != null &&
                 ex.InnerException.GetType() == typeof(ViewStateException)))
                Response.Redirect(Request.Url.ToString());


            if (ex is HttpUnhandledException && ex.InnerException is COMException)
            {
                switch (((COMException)ex.InnerException).ErrorCode)
                {
                    case unchecked((int)0x80070040): //The specified network name is no longer available.
                    case unchecked((int)0x800703E3): //The I/O operation has been aborted because of either a thread exit or an application request. 
                    case unchecked((int)0x800704CD): //An operation was attempted on a nonexistent network connection
                    case unchecked((int)0x80070001): //Incorrect function.
                        return;
                }
            }

            try
            {
                ExceptionLogger.Log(Request, ex);
            }
            catch
            {
                ExceptionLogger.Log("Global_Application_Error", ex);
            }
        }

        protected void Session_End(Object sender, EventArgs e)
        {
            //((IList<int>)Application["UsersOnline"]).RemoveAt(0);
            Global.AppUsersOnline.RemoveAt(0);
            if (Request.Cookies["rememberMe"] != null)
            {
                var cookie = new HttpCookie("rememberMe") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }

            MatchmakerHelper.CurrentUsername = null;
            MatchmakerHelper.SetMatchmakerState(false);

            Classes.MySpace.DataAvailability.RevokeAccess(Context);
        }

        protected void Application_End(Object sender, EventArgs e)
        {
            PerformanceTimers.WriteDataToLogFile();
            var workerDic = Global.AppBackgroundWorkersDic;
            //((Dictionary<string, ezFixUpBackgroundWorker>)HttpContext.Current.Application["BackgroundWorkersDic"]);
            foreach (var bgWorkerItem in workerDic)
            {
                ezFixUpBackgroundWorker.StopBackgroundWorker(bgWorkerItem.Value);
            }
            Logger.LogWarning("Application Ending");
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // 
            // Global
            // 
            this.BeginRequest += new EventHandler(this.Global_BeginRequest);
            this.PreSendRequestHeaders += new EventHandler(this.Global_PreSendRequestHeaders);
            //this.PostReleaseRequestState += new EventHandler(this.Global_PostReleaseRequestState);
#if DEBUG
            //this.PreRequestHandlerExecute += new EventHandler(this.Global_PreRequestHandlerExecute);             
#endif
        }

        #endregion

        private void Global_BeginRequest(object sender, EventArgs e)
        {
        }

        public override string GetVaryByCustomString(HttpContext context, string arg)
        {
            string vary = null;
            foreach (string key in arg.Split(';'))
            {
                if (vary == null) vary = ""; else vary += ";";

                UserSession userSession = null;
                switch (key)
                {
                    case "InterestedIn":
                        userSession = PageBase.GetCurrentUserSession();
                        if (userSession == null) continue;
                        if (Config.Users.InterestedInFieldEnabled)
                        {
                            vary += userSession.InterestedIn.ToString();
                        }
                        else
                        {
                            switch (userSession.Gender)
                            {
                                case Classes.User.eGender.Male:
                                    vary += Classes.User.eGender.Female.ToString();
                                    break;
                                case Classes.User.eGender.Female:
                                    vary += Classes.User.eGender.Male.ToString();
                                    break;
                            }
                        }
                        break;
                    case "CurrentUsername":
                        userSession = PageBase.GetCurrentUserSession();
                        if (userSession == null) continue;
                        vary += userSession.Username;
                        break;
                    case "Language":
                        vary += PageBase.GetLanguageId();
                        break;
                }
            }

            return vary;
        }

        public static Control GetPostBackControl(Page page)
        {
            Control control = null;
            string ctrlname;

            ScriptManager scriptManager = ScriptManager.GetCurrent(page);
            if (page.IsPostBack && scriptManager != null && scriptManager.IsInAsyncPostBack)
            {
                ctrlname = ScriptManager.GetCurrent(page).AsyncPostBackSourceElementID;

                control = page.FindControl(ctrlname);
            }
            else
            {
                ctrlname = page.Request.Params["__EVENTTARGET"];
                if (ctrlname != null && ctrlname != String.Empty)
                {
                    control = page.FindControl(ctrlname);
                }
                // if __EVENTTARGET is null, the control is a button type and we need to 
                // iterate over the form collection to find it
                else
                {
                    string ctrlStr = String.Empty;
                    Control c = null;
                    foreach (string ctl in page.Request.Form)
                    {
                        if (ctl == null)
                            continue;

                        // handle ImageButton controls ...
                        if (ctl.EndsWith(".x") || ctl.EndsWith(".y"))
                        {
                            ctrlStr = ctl.Substring(0, ctl.Length - 2);
                            c = page.FindControl(ctrlStr);
                        }
                        else
                        {
                            c = page.FindControl(ctl);
                        }
                        if (c is Button ||
                            c is ImageButton)
                        {
                            control = c;
                            break;
                        }
                    }
                }
            }
            return control;
        }

        #region IHttpApplicationConnectionStringProvider Members

        public string GetConnectionString()
        {
            return Config.DB.ConnectionString;
        }

        #endregion

        #region IHttpApplicationUserAdapter Members

        public string GetLoginUrl()
        {
            return Config.Urls.Home;
        }

        public string GetCurrentlyLoggedInUsername()
        {
            if (HttpContext.Current != null && Global.GetSessionState()["UserSession"] != null)
            {
                UserSession sess = (UserSession)Global.GetSessionState()["UserSession"];
                return sess.Username;
            }
            else
            {
                return null;
            }
        }

        public bool IsRoomAdmin(string username, int chatRoomId)
        {
            if (chatRoomId == -1)
            {
                if (username == "admin")
                    return true;
                else
                    return false;
            }

            GroupMember member = GroupMember.Fetch(chatRoomId, username);
            if (member != null && (member.Type == GroupMember.eType.Admin || member.Type == GroupMember.eType.Moderator))
                return true;
            else
                return false;
        }

        public bool HasChatAccess(string username, int chatRoomId)
        {
            if (chatRoomId == -1)
                return true;

            if (Config.Groups.EnableGroups && Config.Groups.EnableAjaxChat)
            {
                GroupMember groupMember = GroupMember.Fetch(chatRoomId, username);

                if (groupMember == null || !groupMember.Active)
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }

        public string GetUserDisplayName(string username)
        {
            return username;
        }

        public bool UserExists(string username)
        {
            try
            {
                Classes.User.Load(username);
            }
            catch (NotFoundException)
            {
                return false;
            }

            return true;
        }

        public bool IsAdministrator(string username)
        {
            return username == "admin";
        }

        #endregion

        #region IHttpApplicationSupportAvatars Members

        public string GetUserAvatar(string username)
        {
            try
            {
                return ImageHandler.CreateImageUrl(Photo.GetPrimary(username).Id, 30, 30, false, true, true);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        #endregion
















        #region IHttpApplicationSupportProfiles Members

        public string GetUserProfileUrl(string username)
        {
            return Config.Urls.Home + "/ShowUser.aspx?uid=" + username;
        }

        #endregion

        #region IHttpApplicationChatRoomProvider Members

        public ChatRoom GetChatRoom(int chatRoomId)
        {
            if (!Config.Misc.EnableAjaxChat && !Config.Groups.EnableAjaxChat)
                return null;

            if (Config.Misc.EnableAjaxChat && chatRoomId == -1)
            {
                lock (ChatRooms)
                {
                    if (ChatRooms.ContainsKey(-1))
                    {
                        return ChatRooms[-1];
                    }
                    else
                    {
                        ChatRoom room = new ChatRoom(Estream.AjaxChat.Classes.Lang.Trans("Main Chat"));
                        room.Id = chatRoomId;
                        room.Topic = Estream.AjaxChat.Classes.Lang.Trans("Welcome to main chat");
                        ChatRooms.Add(-1, room);
                        return room;
                    }
                }
            }

            if (Config.Groups.EnableAjaxChat)
            {
                Group group = Group.Fetch(chatRoomId);
                if (group == null) return null;

                lock (ChatRooms)
                {
                    if (ChatRooms.ContainsKey(group.ID))
                    {
                        return ChatRooms[group.ID];
                    }
                    else
                    {
                        ChatRoom room = new ChatRoom(group.Name);
                        room.Id = chatRoomId;
                        room.Topic = Estream.AjaxChat.Classes.Lang.Trans(String.Format("Welcome to group {0}", group.Name));

                        return room;
                    }
                }
            }

            return null;
        }

        private static Dictionary<int, ChatRoom> ChatRooms = new Dictionary<int, ChatRoom>();

        #endregion

        #region IHttpApplicationSupportPhotos Members

        public string GetUserPhoto(string username, int width, int height)
        {
            try
            {
                return ImageHandler.CreateImageUrl(Photo.GetPrimary(username).Id, width, height, false, true, true);
            }
            catch (NotFoundException)
            {
                return null;
            }
        }

        #endregion
    }
}