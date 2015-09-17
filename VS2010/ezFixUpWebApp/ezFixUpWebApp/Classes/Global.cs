using System.Collections.Generic;
using System.Web;

namespace ezFixUp
{
    public partial class Global
    {
        static Global()
        {
            ApplicationStateInstance.Add("UserStateInstancesDic", new Dictionary<string, Dictionary<string, object>>());
            ApplicationStateInstance.Add("AnonymousStateInstancesDic", new Dictionary<string, Dictionary<string, object>>());
        }

        public static Dictionary<string, object> ApplicationStateInstance = new Dictionary<string, object>();

        public static Dictionary<string, Dictionary<string, object>> UserStateInstancesDic
        {
            get { return ApplicationStateInstance["UserStateInstancesDic"] as Dictionary<string, Dictionary<string, object>>; }
        } 
        
        public static Dictionary<string, object> GetUserStateInstance(string username)
        {
            var sessions = UserStateInstancesDic;
            return (sessions.ContainsKey(username)) ? sessions[username] : null;
        }

        public static Dictionary<string, Dictionary<string, object>> AnonymousStateInstancesDic
        {
            get { return ApplicationStateInstance["AnonymousStateInstancesDic"] as Dictionary<string, Dictionary<string, object>>; }
        }

        public static Dictionary<string, object> GetAnonymousStateInstance(string sessionId)
        {
            var sessions = AnonymousStateInstancesDic;
            return (sessions.ContainsKey(sessionId)) ? sessions[sessionId] : null;
        }

        public static Dictionary<string, object> GetSessionState(string sessionId = null)
        {
            if (sessionId != null)
            {
                var sessions = UserStateInstancesDic;
                return (sessions.ContainsKey(sessionId)) ? sessions[sessionId] : null;
            }
            else if (HttpContext.Current != null)
            {
                var cookie = HttpContext.Current.Request.Cookies.Get("ASP.NET_SessionId");
                
                if (cookie != null)
                {
                    sessionId = cookie.Value;
                    var sessions = UserStateInstancesDic;
                    return (sessions.ContainsKey(sessionId)) ? sessions[sessionId] : null;
                }
            }
            return null;
        } 
    }
}