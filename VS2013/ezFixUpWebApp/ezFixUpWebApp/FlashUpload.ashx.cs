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
using ezFixUp.Classes;

namespace ezFixUp
{
    public class FlashUpload : IHttpHandler
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
            // Example of using a passed in value in the query string to set a categoryId
            // Now you can do anything you need to witht the file.
            //int categoryId = 0;
            //if (!string.IsNullOrEmpty(context.Request.QueryString["CategoryID"]))
            //{
            //    int.TryParse(context.Request.QueryString["CategoryID"],out categoryId);
            //}
            //if (categoryId > 0)
            //{
            //}

            if (context.Request.Files.Count > 0)
            {
                // get the applications path

                //string uploadPath = context.Server.MapPath(context.Request.ApplicationPath + "/Upload");
                // loop through all the uploaded files

                string cacheKey = "flashUpload_" + context.Request.Params["guid"];
                if (context.Cache.Get(cacheKey) != null)
                {
                    if (context.Request.Params["type"] == "photo")
                    {
                        Username = ((string)context.Cache.Get(cacheKey)).Split('|')[0];
                        string photoAlbumID = ((string) context.Cache.Get(cacheKey)).Split('|')[1];
                        if (photoAlbumID == "-1") PhotoAlbumID = null;
                        else PhotoAlbumID = Convert.ToInt32(photoAlbumID);
                    }
                    else if (context.Request.Params["type"] == "video") Username = ((string)context.Cache.Get(cacheKey));
                }

                if (Username != null)
                {
                    User user = null;

                    try
                    {
                        user = Classes.User.Load(Username);
                    }
                    catch (NotFoundException)
                    {
                        return;
                    }

                    BillingPlanOptions billingPlanOptions = null;
                    if (!Config.Misc.SiteIsPaid)
                    {
                        billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                    }
                    else
                    {
                        var isNonPaidMember = !User.IsPaidMember(Username);

                        if (isNonPaidMember)
                        {
                            billingPlanOptions = Config.Users.GetNonPayingMembersOptions();
                        }
                        else
                        {
                            Subscription subscription =
                                Subscription.FetchActiveSubscription(Username);
                            if (subscription == null)
                                billingPlanOptions = Config.Users.GetNonPayingMembersOptions();//new BillingPlanOptions();
                            else
                            {
                                BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);
                                billingPlanOptions = plan.Options;
                            }
                        }
                    }

                    for (int j = 0; j < context.Request.Files.Count; j++)
                    {
                        // get the current file
                        HttpPostedFile uploadFile = context.Request.Files[j];
                        // if there was a file uploded
                        if (uploadFile.ContentLength > 0)
                        {
                            // use this if using flash to upload
                            //uploadFile.SaveAs(Path.Combine(uploadPath, uploadFile.FileName));

                            if (context.Request.Params["type"] == "photo")
                            {
                                Image image;
                                try
                                {
                                    image = Image.FromStream
                                        (uploadFile.InputStream);
                                }
                                catch (Exception err)
                                {
                                    Global.Logger.LogStatus("Image upload failed", err);
                                    context.Cache.Insert("flashPhotoUploadError_" + Username,
                                        Lang.Trans("Invalid image!"), null, DateTime.Now.AddMinutes(30),
                                        Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                                    return;
                                }

                                if (image.Height < Config.Photos.PhotoMinHeight || image.Width < Config.Photos.PhotoMinWidth)
                                {
                                    string key = "flashPhotoUploadError_" + Username;
                                    string error = uploadFile.FileName + " - " + Lang.Trans("The photo is too small!") + "\\n";
                                    if (context.Cache.Get(key) != null)
                                    {
                                        error = context.Cache.Get(key) + error;
                                    }
                                    context.Cache.Insert(key, error, null, DateTime.Now.AddMinutes(30),
                                        Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                                    return;
                                }

                                Photo photo = new Photo();

                                photo.Image = image;
                                photo.ExplicitPhoto = false;
                                photo.User = user;
                                photo.PhotoAlbumID = PhotoAlbumID;
                                photo.Name = String.Empty;
                                photo.Description = String.Empty;

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

                                lock(threadLock)
                                {
                                    int allPhotos = Photo.Search(-1, Username, -1, null, null, null, null).Length;
                                    int maxPhotos = billingPlanOptions.MaxPhotos.Value;
                                    if (user.Level != null && maxPhotos < user.Level.Restrictions.MaxPhotos)
                                        maxPhotos = user.Level.Restrictions.MaxPhotos;
                                    if (allPhotos >= maxPhotos)
                                    {
                                        context.Cache.Insert("flashPhotoUploadError_" + Username,
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

                                    Event newEvent = new Event(photo.Username);

                                    newEvent.Type = Event.eType.NewFriendPhoto;
                                    NewFriendPhoto newFriendPhoto = new NewFriendPhoto();
                                    newFriendPhoto.PhotoID = photo.Id;
                                    newEvent.DetailsXML = Misc.ToXml(newFriendPhoto);

                                    newEvent.Save();

                                    string[] usernames = Classes.User.FetchMutuallyFriends(photo.Username);

                                    foreach (string friendUsername in usernames)
                                    {
                                        if (Config.Users.NewEventNotification)
                                        {
                                            string text = String.Format("Your friend {0} has uploaded a new photo".Translate(),
                                                  "<b>" + photo.Username + "</b>");
                                            string thumbnailUrl = ImageHandler.CreateImageUrl(photo.Id, 50, 50, false, true, true);
                                            Classes.User.SendOnlineEventNotification(photo.Username, friendUsername,
                                                                                     text, thumbnailUrl,
                                                                                     UrlRewrite.CreateShowUserPhotosUrl(
                                                                                         photo.Username));
                                        }
                                    }

                                    #endregion
                                }
                            }
                            else if (context.Request.Params["type"] == "video")
                            {
                                string tempfile;
                                
                                if (!Misc.GetTempFileName(out tempfile))
                                    tempfile = Path.GetTempFileName();
                                uploadFile.SaveAs(tempfile);

                                VideoUpload videoUpload = new VideoUpload(Username);
                                if (billingPlanOptions.AutoApproveVideos.Value
                                    || user.Level != null && user.Level.Restrictions.AutoApproveVideos)
                                {
                                    videoUpload.Approved = true;
                                }

                                lock (threadLock)
                                {
                                    List<VideoUpload> videos = VideoUpload.Load(null, Username, null, null, null, null);
                                    int maxVideoUploads = 0;// Config.Misc.MaxVideoUploads;
                                    if (maxVideoUploads < billingPlanOptions.MaxVideoUploads.Value)
                                        maxVideoUploads = billingPlanOptions.MaxVideoUploads.Value;
                                    if (user.Level != null && maxVideoUploads < user.Level.Restrictions.MaxVideoUploads)
                                        maxVideoUploads = user.Level.Restrictions.MaxVideoUploads;
                                    if (videos.Count >= maxVideoUploads)
                                    {
                                        context.Cache.Insert("flashVideoUploadError_" + Username,
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
                                            string text = String.Format("Your friend {0} has uploaded a new video".Translate(),
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
                                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
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

//                            ((PageBase)Page).StatusPageMessage = Lang.Trans("Your video has been queued for processing!");
                            }
                        }
                    }
                }
            }
            // Used as a fix for a bugnq in mac flash player that makes the 
            // onComplete event not fire
            HttpContext.Current.Response.Write(" ");
        }

        static void AsyncProcessVideo(Object stateInfo)
        {
            VideoUpload.ProcessVideoUpload((VideoUpload)stateInfo);
        }

        #endregion
    }
}
