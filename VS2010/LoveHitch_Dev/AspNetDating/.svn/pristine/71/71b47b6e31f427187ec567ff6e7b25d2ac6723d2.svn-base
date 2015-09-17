using System;
using System.Web;
using AspNetDating.Classes;

namespace AspNetDating
{
    /// <summary>
    /// The class handles the affiliate tracker
    /// </summary>
    public class AffTracker : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary>
        /// Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler"/> interface.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string htmlOutput = String.Format("document.write('<a href=\"{0}\" target=\"new\"><img src=\"{1}\" border=\"0\"></a>');",
                                              Config.Urls.Home + "/default.aspx?affid=" + context.Request.Params["aff"],
                                              Config.Urls.Home + "/AffiliateBannerImage.ashx?id=" +
                                              context.Request.Params["bid"]);
            context.Response.Write(htmlOutput);
        }

        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"/> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"/> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return false; }
        }

        #endregion
    }
}