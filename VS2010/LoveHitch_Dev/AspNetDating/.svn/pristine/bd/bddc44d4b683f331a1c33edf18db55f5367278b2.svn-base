using System;
using System.Globalization;
using System.Threading;
using System.Timers;
using System.Xml;
using Timer = System.Timers.Timer;

namespace AspNetDating.Classes
{
    public class GoogleSitemaps
    {
        private static Timer timerGenerateSitemap;
        private static bool generateSitemapsLock = false;

        internal static void InitializeTimers()
        {
            timerGenerateSitemap = new Timer();
            timerGenerateSitemap.AutoReset = true;
            timerGenerateSitemap.Interval = TimeSpan.FromHours(6).TotalMilliseconds;
            timerGenerateSitemap.Elapsed += timerGenerateSitemap_Elapsed;
            timerGenerateSitemap.Start();
        }

        private static void timerGenerateSitemap_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Subtract(DBSettings.Get("GoogleSitemaps_LastGenerated", DateTime.Now)) >=
                TimeSpan.FromDays(7))
            {
                ThreadPool.QueueUserWorkItem(AsyncGenerateSitemaps);
                DBSettings.Set("GoogleSitemaps_LastGenerated", DateTime.Now);
            }
        }

        private static void AsyncGenerateSitemaps(object data)
        {
            if (generateSitemapsLock)
                return;

            try
            {
                generateSitemapsLock = true;

                GenerateSitemaps();
            }
            catch (Exception err)
            {
                Global.Logger.LogError("AsyncGenerateSitemaps", err);
            }
            finally
            {
                generateSitemapsLock = false;
            }
        }

        private static void GenerateSitemaps()
        {
            SiteMapDataSet sitemap = new SiteMapDataSet();

            SiteMapDataSet.urlsetRow urlSet = sitemap.urlset.AddurlsetRow();

            #region Basic pages

            sitemap.url.AddurlRow(new Uri(Config.Urls.Home), null, "weekly", 1, urlSet);
            sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/Login.aspx"), null, "monthly", 0.1m, urlSet);
            sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/Registration.aspx"), null, "monthly", 0.8m, urlSet);
            sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/Search2.aspx"), null, "monthly", 0.1m, urlSet);
            if (Config.Ratings.EnableProfileRatings)
                sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/TopUsers.aspx"), null, "daily", 0.7m, urlSet);
            if (Config.Ratings.EnablePhotoRatings)
                sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/TopPhotos.aspx"), null, "daily", 0.7m, urlSet);

            #endregion

            #region Photo contests

            if (Config.Ratings.EnablePhotoContests)
            {
                sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/PhotoContests.aspx"), null, "weekly", 0.5m, urlSet);
                PhotoContest[] contests = PhotoContest.Load(null);
                foreach (PhotoContest contest in contests)
                {
                    if (contest.DateEnds.HasValue && contest.DateEnds.Value < DateTime.Now)
                    {
                        sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/PhotoContest.aspx?cid=" + contest.Id), 
                            null, "yearly", 0.2m, urlSet);
                    }
                    else
                    {
                        sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/PhotoContest.aspx?cid=" + contest.Id),
                            null, "daily", 0.5m, urlSet);
                    }
                }
            }

            #endregion

            Language[] langs = Language.FetchAll();
            IFormatProvider mmddFormat = new CultureInfo(String.Empty, false);

            #region News

            foreach (Language lang in langs)
            {
                if (!lang.Active) continue;
                foreach (News news in News.FetchAsArray(lang.Id))
                {
                    sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/News.aspx?id=" + news.ID),
                        news.PublishDate.ToString("yyy-MM-dd", mmddFormat), "monthly", 0.5m, urlSet);
                }
            }

            #endregion

            #region Content pages

            foreach (Language lang in langs)
            {
                if (!lang.Active) continue;
                foreach (ContentPage page in ContentPage.FetchContentPages(lang.Id, ContentPage.eSortColumn.None))
                {
                    if (page.URL != null || page.VisibleFor == ContentPage.eVisibility.LoggedOnUsers
                        || page.VisibleFor == ContentPage.eVisibility.Paid
                        || page.VisibleFor == ContentPage.eVisibility.Unpaid
                        || (!page.HeaderPosition.HasValue && !page.FooterPosition.HasValue)) continue;
                    sitemap.url.AddurlRow(new Uri(UrlRewrite.CreateContentPageUrl(page.ID)),
                        null, "weekly", 0.5m, urlSet);
                }
            }

            #endregion

            #region Groups

            if (Config.Groups.EnableGroups)
            {
                sitemap.url.AddurlRow(new Uri(Config.Urls.Home + "/Groups.aspx"),
                                      null, "weekly", 0.5m, urlSet);
                foreach (Group group in Group.Fetch(true, Group.eSortColumn.None))
                {
                    sitemap.url.AddurlRow(new Uri(UrlRewrite.CreateShowGroupUrl(group.ID.ToString())),
                                          null, "weekly", 0.5m, urlSet);

                    if (group.IsPermissionEnabled(eGroupPermissions.ViewGalleryNonMembers))
                        sitemap.url.AddurlRow(new Uri(UrlRewrite.CreateShowGroupPhotosUrl(group.ID.ToString())),
                                              null, "weekly", 0.5m, urlSet);

                    if (group.IsPermissionEnabled(eGroupPermissions.ViewMessageBoardNonMembers))
                    {
                        sitemap.url.AddurlRow(new Uri(UrlRewrite.CreateShowGroupTopicsUrl(group.ID.ToString())),
                                              null, "daily", 0.8m, urlSet);
                        foreach (GroupTopic topic in GroupTopic.FetchByGroup(group.ID))
                        {
                            sitemap.url.AddurlRow(
                                new Uri(UrlRewrite.CreateShowGroupTopicsUrl(group.ID.ToString(), topic.ID.ToString())),
                                topic.DateUpdated.ToString("yyy-MM-dd", mmddFormat), "weekly", 0.6m, urlSet);
                        }
                    }

                    if (group.IsPermissionEnabled(eGroupPermissions.ViewEventsNonMembers))
                    {
                        sitemap.url.AddurlRow(new Uri(UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString())),
                                              null, "daily", 0.8m, urlSet);
                        foreach (GroupEvent groupEvent in GroupEvent.FetchByGroupID(group.ID))
                        {
                            sitemap.url.AddurlRow(
                                new Uri(
                                    UrlRewrite.CreateShowGroupEventsUrl(group.ID.ToString(), groupEvent.ID.ToString())),
                                null, "weekly", 0.6m, urlSet);
                        }
                    }
                }
            }

            #endregion

            #region Profiles

            BasicSearch search = new BasicSearch();
            search.Deleted = false;
            search.Active = true;
            search.hasAnswer_isSet = false;
            search.hasPhoto_isSet = false;

            foreach (string username in search.GetResults().Usernames)
            {
                sitemap.url.AddurlRow(new Uri(UrlRewrite.CreateShowUserUrl(username)),
                    null, "weekly", 0.5m, urlSet);

                if (Config.Misc.EnableBlogs)
                {
                    Blog blog = Blog.Load(username);
                    if (blog != null)
                    {
                        foreach (BlogPost post in BlogPost.Fetch(blog.Id))
                        {
                            sitemap.url.AddurlRow(new Uri(UrlRewrite.CreateShowUserBlogUrl(username, post.Id)),
                                null, "monthly", 0.6m, urlSet);
                        }
                    }
                }
            }

            #endregion

            XmlDataDocument sitemapDoc = new XmlDataDocument(sitemap);
            XmlTextWriter writer = new XmlTextWriter(Config.Directories.Home + "/sitemap.xml",
                System.Text.Encoding.UTF8);
            writer.Formatting = Formatting.Indented;
            writer.Indentation = 1;
            writer.IndentChar = '\t';
            writer.WriteStartDocument();
            sitemapDoc.DocumentElement.WriteContentTo(writer);
            writer.Close();
        }
    }
}