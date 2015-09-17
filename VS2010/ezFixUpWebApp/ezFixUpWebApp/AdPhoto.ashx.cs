using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using ezFixUp.Classes;

namespace ezFixUp
{
    public class AdPhoto : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int id = 0;
            int maxWidth = 0, maxHeight = 0;
            try
            {
                id = Convert.ToInt32(context.Request.Params["id"]);
            }
            catch (Exception) // invalid parameter
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
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/adID{0}_{1}_{2}.jpg",
                                           id, maxWidth, maxHeight, id % 10);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\adID{0}_{1}_{2}.jpg",
                                           id, maxWidth, maxHeight, id % 10);
                string cacheKey = String.Format("AdPhoto_Exists_{0}", cacheUrl);
                if (context.Cache[cacheKey] != null)
                {
                    context.Response.Redirect(cacheUrl + "?seed=" + new Random().NextDouble(), false);
                    return;
                }
                if (File.Exists(cacheFile))
                {
                    context.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                      Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1), CacheItemPriority.NotRemovable,
                                      null);

                    context.Response.Redirect(cacheUrl + "?seed=" + new Random().NextDouble(), false);
                    return;
                }
            }

            Image photo = null;
            try
            {
                photo = Classes.AdPhoto.LoadImage(id);
            }
            catch (NotFoundException)
            {
                photo = Image.FromFile(context.Server.MapPath("~/Images") + "/defaultadicon.jpg");
            }

            if (photo != null)
            {
                if (maxWidth > 0 && maxHeight > 0)
                {
                    photo = Photo.ResizeImage(photo, maxWidth, maxHeight);
                }

                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";

                if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
                {
                    string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/adID{0}_{1}_{2}.jpg",
                        id, maxWidth, maxHeight, Math.Abs(id % 10));
                    string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\adID{0}_{1}_{2}.jpg",
                        id, maxWidth, maxHeight, Math.Abs(id % 10));
                    try
                    {
                        if (!File.Exists(cacheFile))
                        {
                            photo.Save(cacheFile, ImageFormat.Jpeg);
                            photo.Dispose();
                        }
                        context.Response.Redirect(cacheUrl + "?seed=" + new Random().NextDouble(), false);
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError(err, cacheFile);
                        photo.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                        photo.Dispose();
                    }
                }
                else
                {
                    photo.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                }
            }
        }

        public static string RenderImageTag(int imageId, int width, int height, string cssClass, bool diskCache)
        {
            string imageUrl = CreateImageUrl(imageId, width, height, diskCache);
            string imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }

        public static string CreateImageUrl(int imageId, int width, int height, bool diskCache)
        {
            string imageUrl = null;

            if (diskCache)
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/adID{0}_{1}_{2}.jpg",
                    imageId, width, height, imageId % 10);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\adID{0}_{1}_{2}.jpg",
                    imageId, width, height, imageId % 10);
                string cacheKey = String.Format("AdPhotos_Exists_{0}", cacheUrl);

                if ((HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) || File.Exists(cacheFile))
                {
                    imageUrl = String.Format("{4}/{3}/adID{0}_{1}_{2}.jpg",
                        imageId, width, height, imageId % 10, Config.Urls.ImagesCacheHome);
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
                string additionalParams = String.Empty;
                if (diskCache)
                    additionalParams += "&diskCache=1";
                imageUrl =
                    String.Format(
                        "{4}/AdPhoto.ashx?id={0}&width={1}&height={2}{3}",
                        imageId, width, height, additionalParams, Config.Urls.Home);
            }

            return imageUrl;
        }

        #endregion
    }
}
