using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BitFactory.Logging;
using System.Web;

namespace ezFixUp.Classes
{
    public class Global
    {
        private static HttpApplicationState _currentApplicationState;
        public static HttpApplicationState CurrentApplicationState
        {
            set { _currentApplicationState = value; }
            get { return _currentApplicationState; }
        }

        public static Dictionary<long, LoveHitchFacebookApp> FacebookAppDic
        {
            get
            {
                if (CurrentApplicationState["FacebookAppDic"] == null)
                {
                    CurrentApplicationState["FacebookAppDic"] = new Dictionary<long, LoveHitchFacebookApp>();
                }
                return CurrentApplicationState["FacebookAppDic"] as Dictionary<long, LoveHitchFacebookApp>;
            }
        }

        private static readonly CompositeLogger logger = new CompositeLogger();

        public static CompositeLogger Logger
        {
            get { return logger; }
        }
        public void InitLogger(HttpServerUtility server)
        {
            // create the file logger
            Logger fileLogger = new FileLogger(
                server.MapPath(@"~\Logs\logfile.log"));

#if DEBUG
            fileLogger.SeverityThreshold = LogSeverity.Info;
#else
            fileLogger.SeverityThreshold = LogSeverity.Status;     
#endif

            Logger.AddLogger("File", fileLogger);
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
        public static Dictionary<string, LovehitchBackgroundWorker> AppBackgroundWorkersDic
        {
            get { return (Dictionary<string, LovehitchBackgroundWorker>)CurrentApplicationState["BackgroundWorkersDic"]; }
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
        
    }
}
