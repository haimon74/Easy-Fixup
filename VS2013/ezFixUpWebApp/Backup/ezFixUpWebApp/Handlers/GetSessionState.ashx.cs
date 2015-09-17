using System;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetSessionState : ezFixUpBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            string key = context.Request["key"];
            if (key == null) return;
            int seconds;
            int.TryParse(context.Request["sec"], out seconds);
            DateTime timeout = DateTime.Now.AddSeconds(seconds);
            string val = String.Empty; 

            while (!Global.GetSessionState(context.Session.SessionID).ContainsKey(key)  || (string.IsNullOrWhiteSpace(val) && DateTime.Now < timeout))
            {
                Thread.Sleep(100);
                if (Global.GetSessionState(context.Session.SessionID).ContainsKey(key))
                    val = (string)Global.GetSessionState(context.Session.SessionID)[key];
            }

            context.Response.ContentType = "text/plain";
            context.Response.Write(val);
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /*
        public class GetSessionState : ezFixUpBaseAsyncHandler
    {
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            var context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            string key = context.Request["key"];
            if (key == null) return;
            int seconds;
            int.TryParse(context.Request["sec"], out seconds);
            DateTime timeout = DateTime.Now.AddSeconds(seconds);
            string val = (string)context.Session[key];
            while (string.IsNullOrEmpty(val) && DateTime.Now < timeout)
                val = (string)context.Session[key];
            
            context.Response.ContentType = "text/plain";
            context.Response.Write(val);
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }*/
    }
}
