using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;
using AspNetDating.Classes;

namespace AspNetDating
{
    public class WebcamUpload : IHttpHandler
    {
        public string Username { get; set; }

        private int? photoAlbumID = null;
        public int? PhotoAlbumID
        {
            get { return photoAlbumID; }
            set { photoAlbumID = value; }
        }

        public void ProcessRequest(HttpContext context)
        {
            int width = Convert.ToInt32(context.Request.Params["width"]);
            int height = Convert.ToInt32(context.Request.Params["height"]);

            string cacheKey = "webcamUpload_" + context.Request.Params["guid"];
            if (context.Cache.Get(cacheKey) != null)
            {
                Username = ((string)context.Cache.Get(cacheKey)).Split('|')[0];
                string photoAlbumID = ((string)context.Cache.Get(cacheKey)).Split('|')[1];
                if (photoAlbumID == "-1") PhotoAlbumID = null;
                else PhotoAlbumID = Convert.ToInt32(photoAlbumID);
            }

            if (Username != null)
            {
                User user = null;

                try
                {
                    user = User.Load(Username);
                }
                catch (NotFoundException)
                {
                    return;
                }

                Bitmap bmp = new Bitmap(width, height);
                Graphics graphics = Graphics.FromImage(bmp);
                SolidBrush brush = new SolidBrush(Color.White);
                graphics.FillRectangle(brush, 0, 0, width, height);

                for (int rows = 0; rows < height; rows++)
                {
                    string parameters = context.Request.Params["px" + rows];

                    // convert the string into an array of n elements
                    string[] crow = (parameters).Split(',');

                    for (int cols = 0; cols < width; cols++)
                    {
                        string value = crow[cols];
                        if (value.Length > 0)
                        {
                            StringBuilder hex = new StringBuilder(value);
                            while (hex.Length < 6)
                            {
                                hex.Insert(0, "0").Append(hex.ToString());
                            }

                            int r = Int32.Parse(hex.ToString().Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                            int g = Int32.Parse(hex.ToString().Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                            int b = Int32.Parse(hex.ToString().Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                            Color c = Color.FromArgb(r, g, b);
                            bmp.SetPixel(cols, rows, c);
                        }
                    }
                }

                if (Config.Photos.DoWatermark
                    && bmp.Width >= Config.Photos.MinWidthToWatermark
                    && bmp.Height >= Config.Photos.MinHeightToWatermark)
                {
                    Image watermark;
                    if (context.Cache["Watermark_Image"] == null)
                    {
                        string filename = context.Server.MapPath("~/Images") + "/Watermark.png";
                        watermark = Image.FromFile(filename);
                        context.Cache.Add("Watermark_Image", watermark, new CacheDependency(filename),
                                          Cache.NoAbsoluteExpiration, TimeSpan.FromHours(24),
                                          CacheItemPriority.NotRemovable, null);
                    }
                    else
                    {
                        watermark = (Image)context.Cache["Watermark_Image"];
                    }

                    try
                    {
                        lock (watermark)
                        {
                            Photo.ApplyWatermark(bmp, watermark, Config.Photos.WatermarkTransparency,
                                                 Config.Photos.WatermarkPosition);
                        }
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogWarning("Unable to apply watermark", err);
                    }
                }

                BillingPlanOptions billingPlanOptions = null;
                Subscription subscription = Subscription.FetchActiveSubscription(Username);
                if (subscription == null)
                    billingPlanOptions = new BillingPlanOptions();
                else
                {
                    BillingPlan plan = BillingPlan.Fetch(subscription.PlanID);
                    billingPlanOptions = plan.Options;
                }

                Photo photo = new Photo();

                photo.Image = bmp;
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

                photo.Save(true);

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
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
