using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.SessionState;
using ezFixUp.Classes;

namespace ezFixUp
{
    public class GroupIcon : IHttpHandler, IReadOnlySessionState
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int groupID = 0;
            int maxWidth = 0, maxHeight = 0;
            try
            {
                groupID = Convert.ToInt32(context.Request.Params["groupID"]);
            }
            catch (Exception) // invalid groupID parameter
            {
                return;
            }

            try
            {
                maxWidth = Convert.ToInt32((context.Request.Params["width"]));
                maxHeight = Convert.ToInt32((context.Request.Params["height"]));
            }
            catch (ArgumentException) // parameters are not presented or they are invalid
            {
            }

            if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupID{0}_{1}_{2}.jpg",
                                                groupID, maxWidth, maxHeight, groupID % 10);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupID{0}_{1}_{2}.jpg",
                                                groupID, maxWidth, maxHeight, groupID % 10);
                string cacheKey = String.Format("GroupIcon_Exists_{0}", cacheUrl);
                if (context.Cache[cacheKey] != null)
                {
                    context.Response.Redirect(cacheUrl, false);
                    return;
                }
                if (File.Exists(cacheFile))
                {
                    context.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                      Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), CacheItemPriority.NotRemovable,
                                      null);

                    context.Response.Redirect(cacheUrl, false);
                    return;
                }
            }

            Image icon = Group.LoadIcon(groupID);

            if (icon != null)
            {
                if (maxWidth > 0 && maxHeight > 0)
                {
                    icon = Photo.ResizeImage(icon, maxWidth, maxHeight);
                }

                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";

                if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
                {
                    string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupID{0}_{1}_{2}.jpg",
                        groupID, maxWidth, maxHeight, Math.Abs(groupID % 10));
                    string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupID{0}_{1}_{2}.jpg",
                        groupID, maxWidth, maxHeight, Math.Abs(groupID % 10));
                    try
                    {
                        if (!File.Exists(cacheFile))
                        {
                            icon.Save(cacheFile, ImageFormat.Jpeg);
                            icon.Dispose();
                        }
                        context.Response.Redirect(cacheUrl, false);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError(err, cacheFile);
                        icon.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                        icon.Dispose();
                    }
                }
                else
                {
                    icon.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                }
            }
        }

        public static string CreateImageUrl(int groupID, int width, int height, bool diskCache)
        {
            string imageUrl = null;

            if (diskCache)
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupID{0}_{1}_{2}.jpg",
                    groupID, width, height, groupID % 10);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupID{0}_{1}_{2}.jpg",
                    groupID, width, height, groupID % 10);
                string cacheKey = String.Format("GroupIcon_Exists_{0}", cacheUrl);

                if ((HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) || File.Exists(cacheFile))
                {
                    imageUrl = String.Format("{4}/{3}/groupID{0}_{1}_{2}.jpg",
                        groupID, width, height, groupID % 10, Config.Urls.ImagesCacheHome);
                }

                if (imageUrl != null && HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] == null)
                {
                    HttpContext.Current.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                                  Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1),
                                                  CacheItemPriority.NotRemovable, null);
                }
            }

            if (imageUrl == null)
            {
                string additionalParams = "";
                if (diskCache)
                    additionalParams += "&diskCache=1";
                imageUrl =
                    String.Format(
                        "{4}/GroupIcon.ashx?groupID={0}&width={1}&height={2}{3}",
                        groupID, width, height, additionalParams, Config.Urls.Home);
            }

            return imageUrl;
        }

        #endregion
    }
}