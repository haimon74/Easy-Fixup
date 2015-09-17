using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Caching;
using ezFixUp.Data;
//using ezFixUp.Handlers;

namespace ezFixUp
{
    public class ImageHelper 
    {        
        public static string RenderImageTag(User.eGender gender, int width, int height, string cssClass,
                                            bool useCache, bool diskCache)
        {
            int photoId = GetPhotoIdByGender(gender);

            return RenderImageTag(photoId, width, height, cssClass, useCache, diskCache);
        }

        public static string RenderImageTag(int imageId, int width, int height, string cssClass,
                                            bool useCache, bool diskCache)
        {
            return RenderImageTag(imageId, width, height, cssClass, useCache, diskCache, false);
        }

        public static string RenderImageTag(int imageId, int width, int height, string cssClass, bool useCache,
            bool diskCache, bool findFace)
        {
            string imageUrl = CreateImageUrl(imageId, width, height, useCache, diskCache, findFace);
            string imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }

        public static string RenderFacebookImageTag(long fbId, eFacebookImageType imageType, string cssClass)
        {
            string imageUrl = CreateFacebookImageUrl(fbId, imageType);
            string imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }

        public static string RenderImageStackTag(string username, int width, int height, string cssClass)
        {
            string imageUrl = CreateImageStackUrl(username, width, height);
            string imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, cssClass);
            return imageTag;
        }

        public static string RenderThemedImageTag(string imageName, int width, int height, string cssClass)
        {
            string imageUrl = String.Format("~/App_Themes/{0}/{1}", MatchmakerHelper.SiteTheme, imageName);
            string imageTag =
                String.Format("<img src=\"{0}\" class=\"{1}\" width=\"{2}\" height=\"{3}\" border=\"0\" />",
                                        imageUrl, cssClass, width, height);
            return imageTag;
        }
        public static string CreateMatchToImageUrl(UserSession sessionUser, int width, int height)
        {
            string url = String.Empty;
            if (MatchmakerHelper.MatchToUsername.IsNullOrEmpty())
            {
                if (sessionUser == null)
                    url = CreateImageUrl(-1, 200, 200, false, true, true);
                else
                    url = CreateImageUrl(sessionUser.Gender == User.eGender.Male ? -1 : -2, 200, 200, false, true, true);
            }
            else
            {
                var user = User.Load(MatchmakerHelper.MatchToUsername);

                if (MatchmakerHelper.MatchToFriendImageId > 0)
                    url = CreateImageUrl(MatchmakerHelper.MatchToFriendImageId, 200, 200, false, true, true);
                else if (user.FacebookID.HasValue)
                    url = CreateFacebookImageUrl(user.FacebookID.Value, eFacebookImageType.Large);
                else
                    url = CreateImageUrl(user.Gender == User.eGender.Male ? -1 : -2, 200, 200, false, true, true);
            }
            return url;
        }
        public static string CreateImageStackUrl(string username, int width, int height)
        {
            string imageUrl = null;

            string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/stacks/photostack_{0}_{1}_{2}.png",
                username, width, height);
            string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format("/stacks/photostack_{0}_{1}_{2}.png",
                username, width, height);
            string cacheKey = String.Format("Photos_Exists_{0}", cacheUrl);

            if ((HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null) || File.Exists(cacheFile))
            {
                imageUrl = String.Format("{3}/stacks/photostack_{0}_{1}_{2}.png",
                    username, width, height, Config.Urls.ImagesCacheHome);
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
                        "{3}/Image.ashx?username={0}&width={1}&height={2}&stack=1",
                        username, width, height, Config.Urls.Home);
            }

            return imageUrl;
        }

        public enum eFacebookImageType
        {
            Default,
            Small,
            Square,
            Normal,
            Large,
            Source
        } ;

        public static string CreateFacebookImageUrl(long facebookID, eFacebookImageType imgType)
        {
            string imgTypeStr = String.Format("{0}",
                (imgType == eFacebookImageType.Default)
                    ? (Config.FacebookSettings.FacebookDefaultImageIsNormal
                        ? eFacebookImageType.Normal
                        : eFacebookImageType.Large)
                    : imgType);
            imgTypeStr = imgTypeStr.ToString().ToLower();

            var photoUrl = String.Format("https://graph.facebook.com/{0}/picture?type={1}",
                                             facebookID, imgTypeStr);
            if (imgType == eFacebookImageType.Source)
            {
                //photoUrl = FacebookHelper.GetProfilePictureSource(FacebookApp facebook, long fbUserId)
            }
            return photoUrl;
        }
        public static string CreateImageUrl(int imageId, int width, int height, bool useCache, bool diskCache,
            bool findFace)
        {
            string imageUrl = null;

            if (diskCache)
            {
                var cacheDir = Math.Abs(imageId % 10).ToString();
                string cacheUrl = Config.Urls.ImagesCacheHome + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                    imageId, width, height, cacheDir, findFace ? "face" : "");
                string cacheFile = Config.Directories.ImagesCacheDirectory + String.Format("/{3}/photo{4}_{0}_{1}_{2}.jpg",
                    imageId, width, height, cacheDir, findFace ? "face" : "");
                string cacheKey = String.Format("Photos_Exists_{0}", cacheUrl);

                bool isCacheKeyExist = HttpContext.Current.Cache[cacheKey] != null;
                bool isContextNotNull = HttpContext.Current != null;
                bool isFileExist = File.Exists(cacheFile);

                if ((isContextNotNull && isCacheKeyExist) || isFileExist)
                {
                    imageUrl = String.Format("{5}/{3}/photo{4}_{0}_{1}_{2}.jpg",
                        imageId, width, height, cacheDir, findFace ? "face" : "", Config.Urls.ImagesCacheHome);
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
                if (useCache)
                    additionalParams += "&cache=1";
                if (diskCache)
                    additionalParams += "&diskCache=1";
                if (findFace)
                    additionalParams += "&findFace=1";
                imageUrl =
                    String.Format(
                        "{4}/Image.ashx?id={0}&width={1}&height={2}{3}",
                        imageId, width, height, additionalParams, Config.Urls.Home);
            }

            return imageUrl;
        }

        public static int GetPhotoIdByGender(User.eGender gender)
        {
            int photoId;

            switch (gender)
            {
                case User.eGender.Male:
                    photoId = -1;
                    break;
                case User.eGender.Female:
                    photoId = -2;
                    break;
                case User.eGender.Couple:
                    photoId = -3;
                    break;
                default:
                    photoId = 0;
                    break;
            }

            return photoId;
        }

        public static void DeleteDiskCache(int imageId)
        {
            var cacheDir = Math.Abs(imageId % 10).ToString();
            var cachePath = String.Format(@"{0}\images\cache\{1}",
                Config.Directories.Home, cacheDir);
            var cacheMask = String.Format(@"photo_{0}_*.jpg", imageId);
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
            cacheMask = String.Format(@"photoface_{0}_*.jpg", imageId);
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

        public static void DeleteImageStackCache(string username)
        {
            string cacheFolder = Config.Directories.ImagesCacheDirectory + "/stacks";

            foreach (var file in Directory.GetFiles(cacheFolder, @"photostack_" + username + "_*.png"))
            {
                File.Delete(file);
            }
        }        
    }
}