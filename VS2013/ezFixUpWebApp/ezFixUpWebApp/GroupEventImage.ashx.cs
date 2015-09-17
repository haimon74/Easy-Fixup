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
    public class GroupEventImage : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            int groupEventID = 0;
            int maxWidth = 0, maxHeight = 0;
            try
            {
                groupEventID = Convert.ToInt32(context.Request.Params["id"]);
            }
            catch (Exception) // invalid group event id parameter
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
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupEventID{0}_{1}_{2}.jpg",
                                                groupEventID, maxWidth, maxHeight, groupEventID % 10);
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupEventID{0}_{1}_{2}.jpg",
                                                groupEventID, maxWidth, maxHeight, groupEventID % 10);
                string cacheKey = String.Format("GroupEventImage_Exists_{0}", cacheUrl);
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

            Image image = GroupEvent.LoadImage(groupEventID);

            if (image != null)
            {
                if (maxWidth > 0 && maxHeight > 0)
                {
                    image = Photo.ResizeImage(image, maxWidth, maxHeight);
                }

                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";

                if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
                {
                    string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/groupEventID{0}_{1}_{2}.jpg",
                        groupEventID, maxWidth, maxHeight, Math.Abs(groupEventID % 10));
                    string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\groupEventID{0}_{1}_{2}.jpg",
                        groupEventID, maxWidth, maxHeight, Math.Abs(groupEventID % 10));
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
