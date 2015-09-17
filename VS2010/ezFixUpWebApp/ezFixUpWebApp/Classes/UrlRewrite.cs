/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Configuration;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace ezFixUp.Classes
{
    public class UrlRewrite : IConfigurationSectionHandler
    {
        protected XmlNode _oRules = null;

        protected UrlRewrite()
        {
        }

        public string GetSubstitution(string zPath)
        {
            foreach (XmlNode oNode in _oRules.SelectNodes("rule"))
            {
                var oReg = new Regex(oNode.SelectSingleNode("url/text()").Value, RegexOptions.IgnoreCase);
                Match oMatch = oReg.Match(zPath);

                if (oMatch.Success)
                {
                    return oReg.Replace(zPath, oNode.SelectSingleNode("rewrite/text()").Value);
                }
            }

            foreach (ContentPage page in ContentPage.FetchContentPages(null, null, ContentPage.eSortColumn.None))
            {
                if (page.UrlRewrite == null) continue;

                if (zPath.EndsWith("/" + page.UrlRewrite, true, CultureInfo.InvariantCulture))
                {
                    return String.Format("ContentPage.aspx?id={0}", page.ID); 
                }
            }

            return zPath;
        }

        internal static void Process()
        {
            var oUrlRewrite = (UrlRewrite) ConfigurationManager.GetSection("urlRewriting/rewriteRules");

            string zSubst = oUrlRewrite.GetSubstitution(HttpContext.Current.Request.Path);

            if (zSubst.Length > 0 && zSubst != HttpContext.Current.Request.Path)
            {
                HttpContext.Current.RewritePath(zSubst);
            }
        }

        public static string CreateShowUserUrl(string username)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowUser_uid_{1}.aspx"
                             : "{0}/ShowUser.aspx?uid={1}";
            return String.Format(url, Config.Urls.Home, username);
        }

        public static string CreateShowUserPhotosUrl(string username)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowUserPhotos_uid_{1}.aspx"
                             : "{0}/ShowUserPhotos.aspx?uid={1}";
            return String.Format(url, Config.Urls.Home, username);
        }

        [Obsolete("Deprecated. Use CreateShowUserBlogUrl(string username) instead.")]
        public static string CreateShowUserUrl(string username, int blogPostId)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowUser_uid_{1}_bpid_{2}.aspx"
                             : "{0}/ShowUser.aspx?uid={1}&bpid={2}";
            return String.Format(url, Config.Urls.Home, username, blogPostId);
        }

        public static string CreateShowUserBlogUrl(string username)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowUserBlog_uid_{1}.aspx"
                             : "{0}/ShowUserBlog.aspx?uid={1}";
            return String.Format(url, Config.Urls.Home, username);
        }

        public static string CreateShowUserBlogUrl(string username, int blogPostId)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowUserBlog_uid_{1}_bpid_{2}.aspx"
                             : "{0}/ShowUserBlog.aspx?uid={1}&bpid={2}";
            return String.Format(url, Config.Urls.Home, username, blogPostId);
        }

        public static string CreateShowUserEventsUrl(string username)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowUserEvents_uid_{1}.aspx"
                             : "{0}/ShowUserEvents.aspx?uid={1}";
            return String.Format(url, Config.Urls.Home, username);
        }

        public static string CreateReportUserAbuseUrl(string username)
        {
            return String.Format("{0}/ReportUserAbuse.aspx?uid={1}", Config.Urls.Home, username);
        }

        public static string CreateContentPageUrl(int id)
        {
            return CreateContentPageUrl(id, Config.SEO.EnableUrlRewriting);
        }

        public static string CreateContentPageUrl(int id, bool rewriteUrl)
        {
            string url;
            if (rewriteUrl)
            {
                ContentPage page = ContentPage.FetchContentPage(id);
                if (page.UrlRewrite != null)
                    url = String.Format("{0}/{1}", Config.Urls.Home, page.UrlRewrite);
                else
                    url = String.Format("{0}/ContentPage_id_{1}.aspx", Config.Urls.Home, id);
            }
            else
            {
                url = String.Format("{0}/ContentPage.aspx?id={1}", Config.Urls.Home, id);
            }

            return url;
        }

        public static string CreateShowGroupUrl(string id)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowGroup_id_{1}.aspx"
                             : "{0}/ShowGroup.aspx?id={1}";
            return String.Format(url, Config.Urls.Home, id);
        }

        public static string CreateShowGroupUrl(string id, string whatToShow)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowGroup_id_{1}_show_{2}.aspx"
                             : "{0}/ShowGroup.aspx?id={1}&show={2}";
            return String.Format(url, Config.Urls.Home, id, whatToShow);
        }

        public static string CreateShowGroupTopicsUrl(string gid)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowGroupTopics_gid_{1}.aspx"
                             : "{0}/ShowGroupTopics.aspx?gid={1}";
            return String.Format(url, Config.Urls.Home, gid);
        }

        public static string CreateShowGroupTopicsUrl(string gid, string tid)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowGroupTopics_gid_{1}_tid_{2}.aspx"
                             : "{0}/ShowGroupTopics.aspx?gid={1}&tid={2}";
            return String.Format(url, Config.Urls.Home, gid, tid);
        }

        public static string CreateShowGroupPhotosUrl(string gid)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowGroupPhotos_gid_{1}.aspx"
                             : "{0}/ShowGroupPhotos.aspx?gid={1}";
            return String.Format(url, Config.Urls.Home, gid);
        }

        public static string CreateShowGroupEventsUrl(string gid)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowGroupEvents_gid_{1}.aspx"
                             : "{0}/ShowGroupEvents.aspx?gid={1}";
            return String.Format(url, Config.Urls.Home, gid);
        }

        public static string CreateShowGroupEventsUrl(string gid, string eid)
        {
            string url = Config.SEO.EnableUrlRewriting
                             ?
                                 "{0}/ShowGroupEvents_gid_{1}_eid_{2}.aspx"
                             : "{0}/ShowGroupEvents.aspx?gid={1}&eid={2}";
            return String.Format(url, Config.Urls.Home, gid, eid);
        }

        #region Methods for Mobile version

        public static string CreateMobileShowUserUrl(string username)
        {
            string url = "{0}/ShowUser.aspx?uid={1}";
            return String.Format(url, Config.Urls.HomeMobile, username);
        }

        #endregion

        #region Implementation of IConfigurationSectionHandler

        public object Create(object parent, object configContext, XmlNode section)
        {
            _oRules = section;

            // TODO: Compile all Regular Expressions

            return this;
        }

        #endregion
    }
}