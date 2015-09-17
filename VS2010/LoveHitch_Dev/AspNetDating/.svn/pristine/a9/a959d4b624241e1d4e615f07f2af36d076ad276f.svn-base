using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Xml;
using AspNetDating.Classes;

namespace AspNetDating
{
    public class RssHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Params["action"] == null) return;

            switch (context.Request.Params["action"].ToLower())
            {
                case "news":
                    GenerateNewsRSS(context);
                    break;
                case "newusers":
                    GenerateNewUsersRSS(context);
                    break;
            }
        }

        private static void GenerateNewsRSS(HttpContext context)
        {
            byte[] xmlResponse;

            string cacheKey = String.Format("RssHandler_GenerateNewsRSS");
            if (context.Cache[cacheKey] != null)
            {
                xmlResponse = context.Cache[cacheKey] as byte[];
            }
            else
            {
                News[] news = News.FetchAsArray(1);

                MemoryStream ms = new MemoryStream();
                XmlTextWriter objX = new XmlTextWriter(ms, Encoding.UTF8);

                objX.WriteStartDocument();
                objX.WriteStartElement("rss");
                objX.WriteAttributeString("version", "2.0");
                objX.WriteStartElement("channel");
                objX.WriteElementString("title", Config.Misc.SiteTitle + " - " + Lang.Trans("News"));
                objX.WriteElementString("link", Config.Urls.Home);
                objX.WriteElementString("description", Lang.Trans("Latest news!"));
                objX.WriteElementString("copyright", Lang.Trans("(c) 2011, All rights reserved."));
                objX.WriteElementString("ttl", "1440");

                if (news != null)
                {
                    foreach (News singleNews in news)
                    {
                        objX.WriteStartElement("item");
                        objX.WriteElementString("title", singleNews.Title);
                        objX.WriteElementString("description", singleNews.Text);
                        objX.WriteElementString("link", Config.Urls.Home + "/News.aspx?id=" + singleNews.ID);
                        objX.WriteElementString("pubDate", singleNews.PublishDate.ToString("R"));
                        objX.WriteEndElement();
                    }
                }

                objX.WriteEndElement();
                objX.WriteEndElement();
                objX.WriteEndDocument();
                objX.Flush();
                objX.Close();

                xmlResponse = ms.ToArray();

                context.Cache.Insert(cacheKey, xmlResponse, null, DateTime.Now.AddHours(1),
                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            context.Response.BinaryWrite(xmlResponse);
            context.Response.End();
        }

        private static void GenerateNewUsersRSS(HttpContext context)
        {
            byte[] xmlResponse;

            string cacheKey = String.Format("RssHandler_GenerateNewUsersRSS");
            if (context.Cache[cacheKey] != null)
            {
                xmlResponse = context.Cache[cacheKey] as byte[];
            }
            else
            {
                NewUsersSearch nuSearch = new NewUsersSearch();
                nuSearch.PhotoReq = true;
                nuSearch.UsersCount = 10;
                UserSearchResults nuResults = nuSearch.GetResults();

                MemoryStream ms = new MemoryStream();
                XmlTextWriter objX = new XmlTextWriter(ms, Encoding.UTF8);

                objX.WriteStartDocument();
                objX.WriteStartElement("rss");
                objX.WriteAttributeString("version", "2.0");
                objX.WriteStartElement("channel");
                objX.WriteElementString("title", Config.Misc.SiteTitle + " - " + Lang.Trans("New Users"));
                objX.WriteElementString("link", Config.Urls.Home);
                objX.WriteElementString("description", Lang.Trans("They've just joined us!"));
                objX.WriteElementString("copyright", Lang.Trans("(c) 2011, All rights reserved."));
                objX.WriteElementString("ttl", "30");

                if (nuResults != null && nuResults.Usernames != null)
                {
                    foreach (string username in nuResults.Usernames)
                    {
                        User user = User.Load(username);
                        int age = (int) (DateTime.Now.Subtract(user.Birthdate).TotalDays/365.25);
                        string descr = String.Format(Lang.Trans("<img src=\"{2}\"><br>Gender: {0}<br>Age:{1}"),
                                                     user.Gender, age, Config.Urls.Home +
                                                                       "/Image.ashx?id=" +
                                                                       Photo.GetPrimary(username).Id +
                                                                       "&width=90&height=90&cache=1");

                        objX.WriteStartElement("item");
                        objX.WriteElementString("title", username);
                        objX.WriteElementString("description", descr);
                        objX.WriteElementString("link", Config.Urls.Home + "/ShowUser.aspx?uid=" + username);
                        objX.WriteElementString("pubDate", user.UserSince.ToString("R"));
                        objX.WriteEndElement();
                    }
                }

                objX.WriteEndElement();
                objX.WriteEndElement();
                objX.WriteEndDocument();
                objX.Flush();
                objX.Close();

                xmlResponse = ms.ToArray();

                context.Cache.Insert(cacheKey, xmlResponse, null, DateTime.Now.AddMinutes(10),
                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            context.Response.Clear();
            context.Response.ContentType = "application/xml";
            context.Response.BinaryWrite(xmlResponse);
            context.Response.End();
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}