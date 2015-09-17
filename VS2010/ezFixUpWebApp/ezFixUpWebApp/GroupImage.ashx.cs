using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Collections;
using System.Web.Caching;
using System.Web.Services;
using System.Web.Services.Protocols;
using ezFixUp.Classes;

namespace ezFixUp
{
    public class GroupImage : IHttpHandler
    {
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            int groupPhotoID = 0;
            int maxWidth = 0, maxHeight = 0;

            try
            {
                groupPhotoID = Int32.Parse(context.Request.Params["gpid"]);
            }
            catch (Exception) // invalid group photo ID parameter
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
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupPhoto{0}_{1}_{2}.jpg",
                                                groupPhotoID, maxWidth, maxHeight, groupPhotoID % 10);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupPhoto{0}_{1}_{2}.jpg",
                                                groupPhotoID, maxWidth, maxHeight, groupPhotoID % 10);
                string cacheKey = String.Format("GroupPhotos_Exists_{0}", cacheUrl);
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

            Image image = null;

            try
            {
                image = GroupPhoto.LoadImage(groupPhotoID);    
            }
            catch (NotFoundException)
            {
                return;
            }

            if (maxWidth > 0 && maxHeight > 0)
            {
                image = GroupPhoto.ResizeImage(image, maxWidth, maxHeight);
            }

            context.Response.Clear();
            context.Response.ContentType = "image/jpeg";

            if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupPhoto{0}_{1}_{2}.jpg",
                    groupPhotoID, maxWidth, maxHeight, Math.Abs(groupPhotoID % 10));
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupPhoto{0}_{1}_{2}.jpg",
                    groupPhotoID, maxWidth, maxHeight, Math.Abs(groupPhotoID % 10));
                try
                {
                    if (!File.Exists(cacheFile))
                    {
                        image.Save(cacheFile, ImageFormat.Jpeg);
                        image.Dispose();
                    }
                    context.Response.Redirect(cacheUrl, false);
                }
                catch (Exception err)
                {
                    Global.Logger.LogError(err, cacheFile);
                    image.Save(context.Response.OutputStream, ImageFormat.Jpeg);
                    image.Dispose();
                }
            }
            else
            {
                image.Save(context.Response.OutputStream, ImageFormat.Jpeg);    
            }
        }

        public static string CreateImageUrl(int imageId, int width, int height, bool diskCache)
        {
            string imageUrl = null;

            if (diskCache)
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupPhoto{0}_{1}_{2}.jpg",
                    imageId, width, height, imageId % 10);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupPhoto{0}_{1}_{2}.jpg",
                    imageId, width, height, imageId % 10);
                string cacheKey = String.Format("GroupPhotos_Exists_{0}", cacheUrl);

                if ((HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) || File.Exists(cacheFile))
                {
                    imageUrl = String.Format("{4}/{3}/groupPhoto{0}_{1}_{2}.jpg",
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
                string additionalParams = "";
                if (diskCache)
                    additionalParams += "&diskCache=1";
                imageUrl =
                    String.Format(
                        "{4}/GroupImage.ashx?gpid={0}&width={1}&height={2}{3}",
                        imageId, width, height, additionalParams, Config.Urls.Home);
            }

            return imageUrl;
        }
    }
}
