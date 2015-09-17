using System;
using System.Web;
using System.Web.Services;

namespace AspNetDating.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SetSessionState : LoveHitchBaseAsyncHandler
    {
        public override IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
        {
            ItsAsynchOperation = new AsynchOperation(cb, context, extraData);
            return base.BeginProcessRequest(context, cb, extraData);
        }
        
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            var context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            string key = context.Request["key"];
            object val = context.Request["val"];
            if (key == null || val == null) return;
            context.Session[key] = val;
            return;
            //context.Response.ContentType = "text/plain";
            //context.Response.Write(val.ToString());
        }

        public override bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
