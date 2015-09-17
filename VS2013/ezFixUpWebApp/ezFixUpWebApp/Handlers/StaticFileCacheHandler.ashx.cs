using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace ezFixUp.Handlers
{
    /// <summary>
    /// Summary description for StaticFileCacheHandler
    /// </summary>
    public class StaticFileCacheHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            FileInfo fi = new FileInfo(context.Request.PhysicalPath);
            context.Response.ClearHeaders();
            context.Response.AddFileDependency(context.Request.PhysicalPath);
            //context.Response.Cache.SetETagFromFileDependencies();
            context.Response.Cache.SetLastModifiedFromFileDependencies();
            context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);


            context.Response.Cache.SetValidUntilExpires(true);
            //context.Response.Cache.SetCacheability(HttpCacheability.Public);
            context.Response.Cache.SetExpires(DateTime.Now.AddMonths(1));
            //context.Response.Cache.SetLastModified(fi.CreationTime);
            context.Response.Cache.SetRevalidation(HttpCacheRevalidation.None);
            
            //Guid hash = new Guid();
            //context.Response.Cache.SetETag( "\"" + hash.ToString().Replace( "-", "" ) + "\"" );
            //context.Response.Cache.SetETag("\"" + context.Application["EtagTimestamp"].ToString());
            try
            {
                var textIfModifiedSince = context.Request.Headers["If-Modified-Since"];
                bool isModified = true;

                if (!string.IsNullOrEmpty(textIfModifiedSince))
                {
                    var ifModifiedSince = DateTime.Parse(textIfModifiedSince);
                    if (fi.LastWriteTime <= ifModifiedSince)
                    {
                        context.Response.Status = "304 Not Modified";
                        context.Response.End();
                        isModified = false;
                        return;
                    }
                }
                if (isModified)
                {
                    context.Response.WriteFile(context.Request.PhysicalPath);
                }
            }
            catch (Exception ex) { }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}