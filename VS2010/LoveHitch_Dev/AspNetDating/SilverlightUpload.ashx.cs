using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.Services;
using System.Web.UI;
using AspNetDating.Classes;

namespace AspNetDating
{
    public class SilverlightUpload : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public string Username { get; set; }

        private int? photoAlbumID = null;
        public int? PhotoAlbumID
        {
            get { return photoAlbumID; }
            set { photoAlbumID = value; }
        }

        private static object threadLock = new object();

        public void ProcessRequest(HttpContext context)
        {
            string cacheKey = "silverlightUpload_" + context.Request.Headers["guid"];
            string type = context.Request.Headers["type"];
            if (context.Cache.Get(cacheKey) != null)
            {
                if (type == "photo")
                {
                    Username = ((string)context.Cache.Get(cacheKey)).Split('|')[0];
                    string photoAlbumID = ((string)context.Cache.Get(cacheKey)).Split('|')[1];
                    if (photoAlbumID == "-1") PhotoAlbumID = null;
                    else PhotoAlbumID = Convert.ToInt32(photoAlbumID);
                }
                else if (type == "video") Username = ((string)context.Cache.Get(cacheKey));
            }

            if (Username != null)
            {
                User user;
                try
                {
                    user = User.Load(Username);
                }
                catch (NotFoundException)
                {
                    return;
                }

                BillingPlanOptions billingPlanOptions = null;

                Subscription subscription = Subscription.FetchActiveSubscription(Username);

                if (subscription == null)
                    billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                else
                {
                    BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);
                    billingPlanOptions = plan.Options;
                }
                //if (!Config.Users.PaymentRequired)
                //{
                //    billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                //}
                //else
                //{
                //    var isNonPaidMember = !User.IsPaidMember(Username);

                //    if (isNonPaidMember)
                //    {
                //        billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                //    }
                //    else
                //    {
                //        Subscription subscription =
                //            Subscription.FetchActiveSubscription(Username);
                //        if (subscription == null)
                //            billingPlanOptions = new BillingPlanOptions();
                //        else
                //        {
                //            BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);
                //            billingPlanOptions = plan.Options;
                //        }
                //    }
                //}

                // if there was a file uploded
                if (context.Request.InputStream.Length > 0)
                {
                    if (type == "photo")
                    {
                        Image image;
                        try
                        {
                            image = Image.FromStream(context.Request.InputStream);
                        }
                        catch (Exception err)
                        {
                            Global.Logger.LogStatus("Image upload failed", err);
                            context.Cache.Insert("silverlightPhotoUploadError_" + Username,
                                                 Lang.Trans("Invalid image!"), null, DateTime.Now.AddMinutes(10),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                            return;
                        }

                        if (image.Height < Config.Photos.PhotoMinHeight || image.Width < Config.Photos.PhotoMinWidth)
                        {
                            string key = "silverlightPhotoUploadError_" + Username;
                            string error = Lang.Trans("The photo is too small!") + "\\n";
                            if (context.Cache.Get(key) != null)
                            {
                                error = context.Cache.Get(key) + error;
                            }
                            context.Cache.Insert(key, error, null, DateTime.Now.AddMinutes(10),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                            return;
                        }

                        var photo = new Photo
                                        {
                                            Image = image,
                                            ExplicitPhoto = false,
                                            User = user,
                                            PhotoAlbumID = PhotoAlbumID,
                                            Name = String.Empty,
                                            Description = String.Empty
                                        };

                        if (Config.Photos.AutoApprovePhotos
                            || billingPlanOptions.AutoApprovePhotos.Value
                            || user.Level != null && user.Level.Restrictions.AutoApprovePhotos)
                        {
                            photo.Approved = true;
                        }
                        else
                        {
                            photo.Approved = false;
                        }

                        lock (threadLock)
                        {
                            int allPhotos = Photo.Search(-1, Username, -1, null, null, null, null).Length;
                            int maxPhotos = billingPlanOptions.MaxPhotos.Value;
                            if (user.Level != null && maxPhotos < user.Level.Restrictions.MaxPhotos)
                                maxPhotos = user.Level.Restrictions.MaxPhotos;
                            if (allPhotos >= maxPhotos)
                            {
                                context.Cache.Insert("silverlightPhotoUploadError_" + Username,
                                                     String.Format(
                                                         Lang.Trans("You cannot have more than {0} photos!"),
                                                         maxPhotos), null, DateTime.Now.AddMinutes(30),
                                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                                return;
                            }

                            photo.Save(true);
                        }
                        
                        photo.Image.Dispose();

                        if (photo.Approved && !photo.PrivatePhoto)
                        {
                            #region Add NewFriendPhoto Event

                            var newEvent = new Event(photo.Username) { Type = Event.eType.NewFriendPhoto };

                            var newFriendPhoto = new NewFriendPhoto();
                            newFriendPhoto.PhotoID = photo.Id;
                            newEvent.DetailsXML = Misc.ToXml(newFriendPhoto);

                            newEvent.Save();

                            string[] usernames = User.FetchMutuallyFriends(photo.Username);

                            foreach (string friendUsername in usernames)
                            {
                                if (Config.Users.NewEventNotification)
                                {
                                    string text =
                                        String.Format("Your friend {0} has uploaded a new photo".Translate(),
                                                      "<b>" + photo.Username + "</b>");
                                    string thumbnailUrl = ImageHandler.CreateImageUrl(photo.Id, 50, 50, false, true,
                                                                                      true);
                                    User.SendOnlineEventNotification(photo.Username, friendUsername,
                                                                     text, thumbnailUrl,
                                                                     UrlRewrite.CreateShowUserPhotosUrl(
                                                                         photo.Username));
                                }
                            }

                            #endregion
                        }
                    }
                    else if (type == "video")
                    {
                        string tempfile;
                        
                        if (!Misc.GetTempFileName(out tempfile))
                            tempfile = Path.GetTempFileName();
                        using (FileStream fs = File.Create(tempfile))
                        {
                            int bytesRead = 0;
                            byte[] buffer = new byte[1024];
                            while ((bytesRead = context.Request.InputStream.Read(buffer, 0, buffer.Length)) > 0)
                            {
                                fs.Write(buffer, 0, bytesRead);
                            }
                        }
                        
                        VideoUpload videoUpload = new VideoUpload(Username);
                        if (billingPlanOptions.AutoApproveVideos.Value
                            || user.Level != null && user.Level.Restrictions.AutoApproveVideos)
                        {
                            videoUpload.Approved = true;
                        }

                        lock(threadLock)
                        {
                            List<VideoUpload> videos = VideoUpload.Load(null, Username, null, null, null, null);
                            int maxVideoUploads = 0; // Config.Misc.MaxVideoUploads;
                            if (maxVideoUploads < billingPlanOptions.MaxVideoUploads.Value)
                                maxVideoUploads = billingPlanOptions.MaxVideoUploads.Value;
                            if (user.Level != null && maxVideoUploads < user.Level.Restrictions.MaxVideoUploads)
                                maxVideoUploads = user.Level.Restrictions.MaxVideoUploads;
                            if (videos.Count >= maxVideoUploads)
                            {
                                context.Cache.Insert("silverlightVideoUploadError_" + Username,
                                                     String.Format(
                                                         Lang.Trans("You cannot have more than {0} video uploads!"),
                                                         maxVideoUploads),
                                                     null, DateTime.Now.AddMinutes(30),
                                                     Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                                return;
                            }

                            videoUpload.Save(); // Save to get new ID
                        }

                        if (videoUpload.Approved && !videoUpload.IsPrivate)
                        {
                            #region Add NewFriendVideoUpload Event

                            Event newEvent = new Event(videoUpload.Username);

                            newEvent.Type = Event.eType.NewFriendVideoUpload;
                            NewFriendVideoUpload newFriendVideoUpload = new NewFriendVideoUpload();
                            newFriendVideoUpload.VideoUploadID = videoUpload.Id;
                            newEvent.DetailsXML = Misc.ToXml(newFriendVideoUpload);

                            newEvent.Save();

                            string[] usernames = Classes.User.FetchMutuallyFriends(videoUpload.Username);

                            foreach (string friendUsername in usernames)
                            {
                                if (Config.Users.NewEventNotification)
                                {
                                    string text =
                                        String.Format("Your friend {0} has uploaded a new video".Translate(),
                                                      "<b>" + videoUpload.Username + "</b>");
                                    int imageID = 0;
                                    try
                                    {
                                        imageID = Photo.GetPrimary(videoUpload.Username).Id;
                                    }
                                    catch (NotFoundException)
                                    {
                                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                                    }
                                    string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true,
                                                                                      true);
                                    Classes.User.SendOnlineEventNotification(videoUpload.Username,
                                                                             friendUsername,
                                                                             text, thumbnailUrl,
                                                                             UrlRewrite.CreateShowUserUrl(
                                                                                 videoUpload.Username));
                                }
                            }

                            #endregion
                        }

                        string userFilesPath = "~/UserFiles/" + Username;
                        string userFilesDir = context.Server.MapPath(userFilesPath);
                        if (!Directory.Exists(userFilesDir))
                        {
                            Directory.CreateDirectory(userFilesDir);
                        }

                        File.Move(tempfile, userFilesDir + @"\video_" + videoUpload.Id + ".original");

                        ThreadPool.QueueUserWorkItem(AsyncProcessVideo, videoUpload);
                    }
                }
            }
        }

        static void AsyncProcessVideo(Object stateInfo)
        {
            VideoUpload.ProcessVideoUpload((VideoUpload)stateInfo);
        }

        #endregion
    }
}
