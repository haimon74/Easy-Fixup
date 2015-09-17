using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;
using AspNetDating.Classes;
using AspNetDating.Handlers;

namespace AspNetDating
{
    public class GiftImageHandler : LoveHitchBaseAsyncHandler
    {
        public override IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, Object extraData)
        {
            ItsAsynchOperation = new AsynchOperation(cb, context, extraData);
            return base.BeginProcessRequest(context, cb, extraData);
        }
        private HttpContext _context;
        public override void EndProcessRequest(IAsyncResult result)
        {
            var context = ItsAsynchOperation.Context;
            base.EndProcessRequest(result);
            string Id = context.Request.Params["id"];

            int MaxWidth = 0, MaxHeight = 0;
            try
            {
                if (context.Request.Params["width"] != null && context.Request.Params["height"] != null)
                {
                    MaxWidth = Convert.ToInt32(context.Request.Params["width"]);
                    MaxHeight = Convert.ToInt32(context.Request.Params["height"]);
                }
            }
            catch (ArgumentException)
            {
                // Invalid paramerers; no need to log the error
            }

            if (Id == null || Id == "0") ShowNoImage(context, MaxWidth, MaxHeight);

            int photoId;
            Int32.TryParse(Id, out photoId);

            if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/giftphoto_{0}_{1}_{2}.jpg",
                                                photoId, MaxWidth, MaxHeight, Math.Abs(photoId % 10));
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format(@"\{3}\giftphoto_{0}_{1}_{2}.jpg",
                                                photoId, MaxWidth, MaxHeight, Math.Abs(photoId % 10));
                
                FileInfo fi = new FileInfo(cacheFile);
                context.Response.ClearHeaders();
                context.Response.AddFileDependency(cacheFile);
                //context.Response.Cache.SetETagFromFileDependencies();
                context.Response.Cache.SetLastModifiedFromFileDependencies();
                context.Response.Cache.SetCacheability(HttpCacheability.ServerAndPrivate);

                context.Response.Cache.SetValidUntilExpires(true);
                //context.Response.Cache.SetCacheability(HttpCacheability.Public);
                context.Response.Cache.SetExpires(DateTime.Now.AddMonths(1));
                //context.Response.Cache.SetLastModified(fi.CreationTime);
                context.Response.Cache.SetRevalidation(HttpCacheRevalidation.None);

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
                        string cacheKey = String.Format("GiftPhotos_Exists_{0}", cacheUrl);
                        if (context.Cache[cacheKey] != null)
                        {
                            //context.Response.Redirect(cacheUrl, false);
                            context.Response.ContentType = "image/jpeg";
                            context.Response.WriteFile(cacheFile);
                            return;
                        }
                        if (File.Exists(cacheFile))
                        {
                            context.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                              Cache.NoAbsoluteExpiration,
                                              TimeSpan.FromHours(1),
                                              CacheItemPriority.NotRemovable,
                                              null);

                            //context.Response.Redirect(cacheUrl, false);
                            context.Response.ContentType = "image/jpeg";
                            context.Response.WriteFile(cacheFile);
                            return;
                        }
                    }
                }
                catch (Exception ex) { }

            }
            
            if (context.Request.Params["cache"] != null && context.Request.Params["cache"] == "1")
            {
                string cacheKey = String.Format("GiftPhotos_ProcessRequest_{0}_{1}_{2}", Id, MaxWidth, MaxHeight);
                if (context.Cache[cacheKey] != null)
                {
                    context.Response.Clear();
                    context.Response.ContentType = "image/jpeg";
                    context.Response.BinaryWrite((byte[])context.Cache[cacheKey]);
                    return;
                }
            }

            
            try
            {
                Image image;
                bool dispose = true;

                image = ByteArrayToImage(GiftType.Fetch(photoId).Content);
                
                lock (image)
                {
                    ShowGiftImage(image, context, MaxWidth, MaxHeight, photoId, dispose);
                }
            }
            catch (Exception err)
            {
                if (context.Request.Params["debug"] != null)
                    Global.Logger.LogError(err);
                ShowNoImage(context, MaxWidth, MaxHeight);
            }
        }
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
             MemoryStream ms = new MemoryStream(byteArrayIn);
             Image returnImage = Image.FromStream(ms);
             return returnImage;
        }
        public override bool IsReusable
        {
            get { return true; }
        }

        private static void ShowNoImage(HttpContext context, int maxWidth, int maxHeight)
        {
            ShowGiftImage(
                Image.FromFile(context.Server.MapPath("~/GiftImages") + "/no_photo_large.png"), context,
                maxWidth, maxHeight, 0);
        }

        private static void ShowGiftImage(Image image, HttpContext context, 
                                          int maxWidth, int maxHeight, int photoId)
        {
            ShowGiftImage(image, context, maxWidth, maxHeight, photoId, true);
        }

        private static void ShowGiftImage(Image image, HttpContext context, 
                                          int maxWidth, int maxHeight, int photoId, bool disposeImage)
        {
            var format = image.RawFormat;
            
            try
            {
                if (maxWidth > 0 && maxHeight > 0)
                    image = photoId > 0 
                        ? Photo.ResizeRectangleImage(image, maxWidth, maxHeight) 
                        : Photo.ResizeImage(image, maxWidth, maxHeight);
            }
            catch (Exception err)
            {
                ExceptionLogger.Log(context.Request, err);
                throw;
            }

            context.Response.Clear();
            context.Response.ContentType = format.Guid == ImageFormat.Png.Guid ? "image/png" : "image/jpeg";

            if (context.Request.Params["cache"] != null && context.Request.Params["cache"] == "1")
            {
                string cacheKey = String.Format("GiftPhotos_ProcessRequest_{0}_{1}_{2}",
                    photoId, maxWidth, maxHeight);

                using (var ms = new MemoryStream())
                {
                    image.Save(ms, format);
                    image.Dispose();

                    context.Cache.Insert(cacheKey, ms.ToArray(), null, Cache.NoAbsoluteExpiration,
                                         TimeSpan.FromMinutes(15));
                    ms.WriteTo(context.Response.OutputStream);
                }
            }
            else if (context.Request.Params["diskCache"] != null && context.Request.Params["diskCache"] == "1")
            {
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/giftphoto_{0}_{1}_{2}.jpg",
                    photoId, maxWidth, maxHeight, Math.Abs(photoId % 10));
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format("/{3}/giftphoto_{0}_{1}_{2}.jpg",
                    photoId, maxWidth, maxHeight, Math.Abs(photoId % 10));
                try
                {
                    if (!File.Exists(cacheFile))
                    {
                        image.Save(cacheFile, format);
                        image.Dispose();
                    }
                    //context.Response.Redirect(cacheUrl, false);
                    context.Response.ContentType = "image/jpeg";
                    context.Response.WriteFile(cacheFile); 
                }
                catch (Exception err)
                {
                    Global.Logger.LogError(err, cacheFile);
                    WriteToOutputStream(context, image, format);
                    image.Dispose();
                }
            }
            else
            {
                WriteToOutputStream(context, image, format);

                if (disposeImage)
                    image.Dispose();
            }
        }

        private static void WriteToOutputStream(HttpContext context, Image image, ImageFormat format)
        {
            if (format.Guid == ImageFormat.MemoryBmp.Guid)
                format = ImageFormat.Jpeg;
            if (format.Guid == ImageFormat.Png.Guid || image.RawFormat.Guid == ImageFormat.Png.Guid)
            {
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Png);

                    stream.WriteTo(context.Response.OutputStream);
                }
            }
            else
            {
                image.Save(context.Response.OutputStream, format);
            }
        }

        public static string RenderImageTag(int imageId, int width, int height, string cssClass)
        {
            string imageUrl = CreateImageUrl(imageId, width, height);
            string imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }

        public static string CreateImageUrl(int imageId, int width, int height)
        {
            string imageUrl = null;

            string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/giftphoto_{0}_{1}_{2}.jpg",
                imageId, width, height, Math.Abs(imageId % 10));
            string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format("/{3}/giftphoto_{0}_{1}_{2}.jpg",
                imageId, width, height, Math.Abs(imageId % 10));
            string cacheKey = String.Format("GiftPhotos_Exists_{0}", cacheUrl);

            if ((HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) || File.Exists(cacheFile))
            {
                imageUrl = String.Format("{4}/{3}/giftphoto_{0}_{1}_{2}.jpg",
                    imageId, width, height, Math.Abs(imageId % 10), Config.Urls.ImagesCacheHome);
            }

            if (imageUrl != null && HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] == null)
            {
                HttpContext.Current.Cache.Add(cacheKey, true, new CacheDependency(cacheFile),
                                              Cache.NoAbsoluteExpiration, TimeSpan.FromHours(1),
                                              CacheItemPriority.NotRemovable, null);
            }
            if (imageUrl == null)
            {
                imageUrl =
                    String.Format(
                        "{3}/GiftImage.ashx?id={0}&width={1}&height={2}&diskCache=1",
                        imageId, width, height, Config.Urls.Home);
            }

            return imageUrl;
        }

        public static void DeleteDiskCache(int imageId)
        {
            var cachePath = String.Format(@"{0}\images\cache\{1}",
                Config.Directories.Home, Math.Abs(imageId % 10));
            var cacheMask = String.Format(@"giftphoto_{0}_*.jpg", imageId);
            foreach (var file in Directory.GetFiles(cachePath, cacheMask))
            {
                try
                {
                    File.Delete(file);
                }
                catch (Exception err)
                {
                    Global.Logger.LogInfo(err);
                }
            }
        }
    }
}