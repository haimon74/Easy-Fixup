using System;
using System.ComponentModel;
using System.Web;
using System.Web.Services;
using ezFixUp.Classes;


namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoadAsyncUserDate : ezFixUpBaseHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            var curUser = PageBase.GetCurrentUserSession();
            if (curUser == null) return;
            var t1 = DateTime.Now;
            //lock (context.Session.SyncRoot)
            //{
            //    //UserClient userClient = new UserClient();
            //    //var worker = userClient.GetUserBackgroundWorker(context.Session, curUser);
            //    //worker.DoWork += bw_FetchUserCacheData; 
            //    //worker.DoWork += bw_FetchFacebookCacheData; 
            //    /////////worker.RunWorkerAsync();
            //    curUser.StartFetchUserCacheDataThread(context);
            //}
            UserSession.FetchUserCacheDataThread(context);
            var t2 = DateTime.Now;
            lock (context.Session.SyncRoot)
            {
                //curUser.StartFetchFacebookCacheDataThread(context);
            }
            var t3 = DateTime.Now;
            //context.Response.ContentType = "text/plain";
            context.Response.Write("true");
        }
        
        /*
        public class LoadAsyncUserDate : ezFixUpBaseAsyncHandler
    {
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            _context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            var curUser = PageBase.GetCurrentUserSession();
            if (curUser == null) return;
            curUser.StartFetchUserCacheDataThread(_context);
            curUser.StartFetchFacebookCacheDataThread(_context);
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
