﻿using System;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;

namespace AspNetDating.Handlers
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class LoveHitchBaseHandler : IHttpHandler, IRequiresSessionState
    {
        public virtual bool IsReusable
        {
            get{return false;}
        }
        //cannot implement - virtual not working
        virtual public void ProcessRequest(HttpContext context)
        {
            CacheProcessRequest(context,0,0,0);
        }
        static public void CacheProcessRequest(HttpContext context, int hours, int minues, int seconds)
        {
            // Cache this handler response for 1 hour.
            HttpCachePolicy c = context.Response.Cache;
            c.SetCacheability(HttpCacheability.Public);
            c.SetMaxAge(new TimeSpan(hours, minues, seconds));
        }
    }
}
