/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using ezFixUp.Data.LoveHitchParallel;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    [Serializable]
    public class Photo
    {
        #region Properties

        private int id;

        public int Id
        {
            get { return id; }
        }

        private string extendedID;

        public string ExtendedID
        {
            set { extendedID = value; }
            get { return (extendedID ?? id.ToString()); }
        }

        private string username;

        public string Username
        {
            get { return username; }
        }

        private int? photoAlbumID = null;
        public int? PhotoAlbumID
        {
            get { return photoAlbumID; }
            set { photoAlbumID = value; }
        }

        private User user;

        public User User
        {
            get
            {
                if (user == null)
                    user = User.Load(username);
                return user;
            }
            set
            {
                user = value;
                username = value.Username;
            }
        }

        public Image image;

        public Image Image
        {
            get
            {
                if (image == null)
                    LoadImage();
                return image;
            }
            set
            {
                image = value;

                if (image != null && (image.Width > Config.Photos.PhotoMaxWidth
                    || image.Height > Config.Photos.PhotoMaxHeight))
                    image = ResizeImage(image, Config.Photos.PhotoMaxWidth, Config.Photos.PhotoMaxHeight);
            }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private bool approved;

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        private bool primary;

        public bool Primary
        {
            get { return primary; }
            set { primary = value; }
        }

        private bool explicitPhoto = false;

        public bool ExplicitPhoto
        {
            get { return explicitPhoto; }
            set { explicitPhoto = value; }
        }

        private bool privatePhoto = false;

        public bool PrivatePhoto
        {
            get { return privatePhoto; }
            set { privatePhoto = value; }
        }

        private decimal averageRating = Decimal.MinValue;

        public decimal AverageRating
        {
            get
            {
                try
                {
                    if (averageRating == Decimal.MinValue)
                        averageRating = PhotoRating.FetchRating(id).AverageVote;
                }
                catch (NotFoundException)
                {
                    averageRating = 0;
                }

                return averageRating;
            }
        }

        private string faceCrop;

        public string FaceCrop
        {
            get { return faceCrop; }
            set { faceCrop = value; }
        }

        private DateTime? approvedDate = DateTime.Now;

        public DateTime? ApprovedDate
        {
            get { return approvedDate; }
            set { approvedDate = value; }
        }

        private bool manualApproval;
        public bool ManualApproval
        {
            get { return manualApproval; }
            set { manualApproval = value; }
        }

        private bool salute;
        public bool Salute
        {
            get { return salute; }
            set { salute = value; }
        }

        #endregion

        protected void LoadImage()
        {
            try
            {
                if (Properties.Settings.Default.StorePhotosAsFiles)
                    LoadImageFromFile();
                else
                    LoadImageFromDB();
            }
            catch (NotFoundException)
            {
                if (Properties.Settings.Default.CheckAlternativePhotoStorage)
                {
                    if (Properties.Settings.Default.StorePhotosAsFiles)
                    {
                        LoadImageFromDB();
                        Save(true);
                    }
                    else
                    {
                        LoadImageFromFile();
                        Save(true);

                        var imageFile = String.Format(@"{0}\{1}\{2}.jpg", Config.Directories.UserImagesDirectory,
                                                                       Id % 100, Id);
                        if (File.Exists(imageFile))
                        {
                            try
                            {
                                File.Delete(imageFile);
                            }
                            catch (Exception err)
                            {
                                Global.Logger.LogWarning(err);
                            }
                        }
                    }
                }
                else
                {
                    throw;
                }
            }
        }

        private void LoadImageFromFile()
        {
            var imageFile = String.Format(@"{0}\{1}\{2}.jpg", Config.Directories.UserImagesDirectory,
                                                           Id % 100, Id);
            if (File.Exists(imageFile))
            {
                var fileStream = new FileStream(imageFile, FileMode.Open, FileAccess.Read,
                                                FileShare.ReadWrite);
                int capacity = Convert.ToInt32(fileStream.Length);
                var memoryStream = new MemoryStream(capacity);
                memoryStream.Write(new BinaryReader(fileStream).ReadBytes(capacity), 0, capacity);
                fileStream.Close();
                memoryStream.Position = 0;

                image = Image.FromStream(memoryStream);
            }
            else
            {
                throw new NotFoundException
                    (Lang.Trans("The requested photo does not exist!"));
            }
        }

        private void LoadImageFromDB()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader( "FetchPhoto", Id);

                if (reader.Read())
                {
                    var buffer = (byte[])reader["Image"];
                    var imageStream = new MemoryStream(buffer);
                    image = Image.FromStream(imageStream);
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The requested photo does not exist!"));
                }
            }
        }

        /// <summary>
        /// Fetches a photo by id
        /// </summary>
        /// <param name="Id">Id of the photo</param>
        /// <returns>Photo object</returns>
        public static Photo Fetch(int id)
        {
            Photo[] photos = Fetch(id, null, -1, null, null, null, null);

            if (photos != null)
            {
                return photos[0];
            }
            else
            {
                throw new NotFoundException
                    (Lang.Trans("The requested photo does not exist!"));
            }
        }

        /// <summary>
        /// Fetches all photos by user
        /// </summary>
        /// <param name="Username">Username of the user</param>
        /// <returns>Array of Photo objects</returns>
        public static Photo[] Fetch(string username)
        {
            string cacheKey = String.Format("Photo_FetchByUsername_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Photo[];
            }

            Photo[] photos = Fetch(-1, username, -1, null, null, null, null);

            if (photos != null && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, photos, null, DateTime.Now.AddMinutes(15),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return photos;
        }

        /// <summary>
        /// Fetches the photos by specified username and photo album id.
        /// If photoAlbumID is null it returns all photos from default photo album by specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="photoAlbumID">The photo album ID.</param>
        /// <returns></returns>
        public static Photo[] Fetch(string username, int? photoAlbumID)
        {
            string cacheKey = String.Format("Photo_FetchByUsernamePhotoAlbumID_{0}_{1}", username, photoAlbumID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Photo[];
            }

            Photo[] photos = Fetch(-1, username, photoAlbumID, null, null, null, null);

            if (photos != null && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, photos, null, DateTime.Now.AddMinutes(15),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return photos;
        }

        private static Photo[] Fetch(int id, string username, int? photoAlbumID, object approved, bool? manualApproved, bool? isFaceControlEnabled, bool? salute)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var photos = from p in db.Photos
                             join u in db.Users
                             on p.u_username equals u.u_username
                             where (id == -1 || id == -2 || id == p.p_id)
                             && (username == null || username == p.u_username)
                             && (photoAlbumID == -1 || photoAlbumID == p.pa_id || (!photoAlbumID.HasValue && p.pa_id == null))
                             && (approved == null || (approved as bool?) == p.p_approved)
                             && (!manualApproved.HasValue || manualApproved == p.p_manual_approval)
                             && (!isFaceControlEnabled.HasValue || isFaceControlEnabled == u.u_face_control_approved)
                             && (!salute.HasValue || salute == p.p_salute)
                             select new Photo
                                        {
                                            id = p.p_id,
                                            username = p.u_username,
                                            photoAlbumID = p.pa_id,
                                            name = p.p_name,
                                            description = p.p_description,
                                            approved = p.p_approved,
                                            primary = p.p_primary,
                                            explicitPhoto = p.p_explicit,
                                            privatePhoto = p.p_private,
                                            faceCrop = p.p_facecrop,
                                            approvedDate = p.p_approvedtimestamp,
                                            manualApproval = p.p_manual_approval,
                                            salute = p.p_salute
                                        };

                var photosArr = photos.ToArray();

                if (photosArr.Length > 0)
                    return photos.ToArray();
                else
                    return null;
            }
        }

        public static int[] Search(int id, string username, int? photoAlbumID, object approved, object primary, object explicitPhoto,
                                   object privatePhoto)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var photoIds = (from p in db.Photos
                                where (id == -1 || id == p.p_id)
                                      && (username == null || username == p.u_username)
                                      && (photoAlbumID == -1 || photoAlbumID == p.pa_id || (!photoAlbumID.HasValue && p.pa_id == null))
                                      && (approved == null || (approved as bool?) == p.p_approved)
                                      && (primary == null || (primary as bool?) == p.p_primary)
                                      && (explicitPhoto == null || (explicitPhoto as bool?) == p.p_explicit)
                                      && (privatePhoto == null || (privatePhoto as bool?) == p.p_private)
                                orderby p.p_id descending
                                select p.p_id).Take(1000);

                return photoIds.ToArray();
            }

            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = (SqlDataReader)
            //        SqlHelper.GetDB().ExecuteReader( "SearchPhotos", id, username, approved, primary, explicitPhoto,
            //                                privatePhoto);

            //    List<int> lPhotoIds = new List<int>();

            //    while (reader.Read())
            //    {
            //        lPhotoIds.Add((int) reader["Id"]);
            //    }

            //    return lPhotoIds.ToArray();
            //}
        }

        public static Photo[] FetchNonApproved()
        {
            return Fetch(-1, null, -1, false, null, null, null);
        }

        public static Photo[] FetchNonApproved(bool isFaceControlEnabled)
        {
            return Fetch(-1, null, -1, false, null, isFaceControlEnabled, null);
        }

        public static Photo[] FetchManualApproved()
        {
            return Fetch(-1, null, -1, false, true, null, null);
        }

        public static Photo FetchSalute(string username)
        {
            Photo[] photos = Fetch(-1, username, -1, null, null, null, true);
            if (photos != null)
                return photos[0]; // every user has only one salute photo
            return null;
        }

        public static Photo[] FetchNonApprovedSalute(bool? isFaceControlEnabled)
        {
            return Fetch(-1, null, -1, false, null, isFaceControlEnabled, true);
        }

        /// <summary>
        /// Saves image to DB
        /// </summary>
        public void Save(bool overwriteImage)
        {
            try
            {
                byte[] bytesImage = null;
                if (!Properties.Settings.Default.StorePhotosAsFiles)
                {
                    // TODO: Do not load and save image if overwriteImage == false
                    var imageStream = new MemoryStream();
                    this.Image.Save(imageStream, ImageFormat.Jpeg);
                    imageStream.Position = 0;
                    var reader = new BinaryReader(imageStream);
                    bytesImage = reader.ReadBytes((int)imageStream.Length);
                }

                using (SqlConnection conn = Config.DB.Open())
                {
                    object result = SqlHelper.GetDB().ExecuteScalar(
                                                            "SavePhoto", (id > 0) ? (object)id : null,
                                                            username, photoAlbumID, bytesImage, name,
                                                            description, approved, approvedDate, primary, explicitPhoto,
                                                            privatePhoto, faceCrop, manualApproval, salute);
                    if (id == 0)
                    {
                        id = Convert.ToInt32(result);
                    }
                }

                if (Properties.Settings.Default.StorePhotosAsFiles)
                {
                    if (overwriteImage)
                    {
                        var imageFolder = String.Format(@"{0}\{1}", Config.Directories.UserImagesDirectory, Id % 100);
                        var imageFile = String.Format(@"{0}\{1}.jpg", imageFolder, Id);

                        if (!Directory.Exists(imageFolder))
                            Directory.CreateDirectory(imageFolder);
                        if (File.Exists(imageFile))
                            File.Delete(imageFile);

                        this.Image.Save(imageFile);
                    }
                }

                if (HttpContext.Current != null)
                {
                    string cacheKey = String.Format("Photo_GetPrimary_{0}", username);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                    cacheKey = String.Format("User_HasPrivatePhotos_{0}", username);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                    cacheKey = String.Format("Photo_FetchByUsername_{0}", username);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                    cacheKey = String.Format("Photo_FetchByUsernamePhotoAlbumID_{0}_{1}", username, photoAlbumID);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }

                ImageHelper.DeleteDiskCache(Id);

                if (Config.Photos.EnablePhotoStack)
                {
                    ImageHelper.DeleteImageStackCache(username);
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Deletes image from DB
        /// </summary>
        public void Delete()
        {
            Delete(id);
        }

        /// <summary>
        /// Deletes image from DB
        /// </summary>
        /// <param name="Id">Id of the image</param>
        public static void Delete(int id)
        {
            Photo photo = Fetch(id);

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "DeletePhoto", id);
            }

            if (Properties.Settings.Default.StorePhotosAsFiles)
            {
                var imageFile = String.Format(@"{0}\{1}\{2}.jpg", Config.Directories.UserImagesDirectory,
                                                                         id % 100, id);
                if (File.Exists(imageFile))
                {
                    File.Delete(imageFile);
                }
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("Photo_GetPrimary_{0}", photo.username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
                cacheKey = String.Format("Photo_FetchByUsername_{0}", photo.username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
                cacheKey = String.Format("Photo_FetchByUsernamePhotoAlbumID_{0}_{1}", photo.username, photo.photoAlbumID);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
                ImageHelper.DeleteDiskCache(photo.id);
            }

            if (Config.Ratings.EnablePhotoContests)
            {
                PhotoContestEntry.DeleteByPhoto(id);
            }

            if (Config.Photos.EnablePhotoStack)
            {
                ImageHelper.DeleteImageStackCache(photo.Username);
            }
        }

        /// <summary>
        /// Fetches the primary photo.
        /// Throws "NotFoundException" exception.
        /// </summary>
        /// <param name="Username"></param>
        /// <returns>Photo object</returns>
        public static Photo GetPrimary(string username)
        {
            string cacheKey = String.Format("Photo_GetPrimary_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Photo;
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchPrimaryPhotoDetails", username);

                var photo = new Photo { username = username, primary = true };

                if (reader.Read())
                {
                    photo.id = (int)reader["ID"];
                    photo.photoAlbumID = reader["PhotoAlbumID"] != DBNull.Value ? (int?)reader["PhotoAlbumID"] : null;
                    photo.name = (string)reader["Name"];
                    photo.description = (string)reader["Description"];
                    photo.approved = (bool)reader["Approved"];
                    photo.explicitPhoto = (bool)reader["Explicit"];
                    if (reader["FaceCrop"] is string)
                        photo.faceCrop = (string)reader["FaceCrop"];

                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Cache.Insert(cacheKey, photo, null, DateTime.Now.AddMinutes(15),
                                                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                    }
                }
                else
                {
                    photo.id = 0;
                    if (username.IsNotNullOrEmpty() && User.Load(username) != null)
                        if (User.Load(username).Gender == User.eGender.Female)
                            photo.id = -2;
                        else if (User.Load(username).Gender == User.eGender.Male)
                            photo.id = -1;

                    photo.photoAlbumID = null;
                    photo.name = String.Empty;
                    photo.description = String.Empty;
                    photo.approved = false;
                    //throw new NotFoundException
                    //    (Lang.Trans("The requested photo does not exist!"));
                }

                return photo;
            }
        }

        public static int GetPrimaryOrDefaultId(string username)
        {
            try
            {
                return GetPrimary(username).Id;
            }
            catch (NotFoundException)
            {
                var user = User.Load(username);
                return (user == null || user.FacebookID.HasValue) ? 0 : ImageHelper.GetPhotoIdByGender(user.Gender);
            }
        }

        public static Photo GetTop(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchTopPhotoDetails", username, Config.Ratings.TopPhotosMinVotes);

                Photo photo = new Photo();

                photo.username = username;

                if (reader.Read())
                {
                    photo.id = (int)reader["ID"];
                    photo.photoAlbumID = reader["PhotoAlbumID"] != DBNull.Value ? (int?)reader["PhotoAlbumID"] : null;
                    photo.name = (string)reader["Name"];
                    photo.description = (string)reader["Description"];
                    photo.approved = (bool)reader["Approved"];
                    photo.explicitPhoto = (bool)reader["Explicit"];
                    photo.primary = (bool)reader["Primary"];
                    if (!(reader["AvgRating"] is DBNull))
                        photo.averageRating = (decimal)reader["AvgRating"];
                }
                else
                {
                    throw new NotFoundException
                        (Lang.Trans("The requested photo does not exist!"));
                }

                return photo;
            }
        }

        public static int GetTopOrPrimaryOrDefaultId(string username)
        {
            try
            {
                return GetTop(username).Id;
            }
            catch (NotFoundException)
            {
                return GetPrimaryOrDefaultId(username);
            }
        }

        /// <summary>
        /// Gets the non approved photo.
        /// If there is no non approved photo in DB it returns NULL.
        /// </summary>
        /// <returns></returns>
        public static Photo GetNonApproved(string approvedBy)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchNonApprovedPhoto", approvedBy);

                Photo photo = new Photo();

                if (reader.Read())
                {
                    photo.id = (int)reader["Id"];
                    photo.username = (string)reader["Username"];
                    photo.photoAlbumID = reader["PhotoAlbumID"] != DBNull.Value ? (int?)reader["PhotoAlbumID"] : null;
                    photo.name = (string)reader["Name"];
                    photo.description = (string)reader["Description"];
                    photo.approved = (bool)reader["Approved"];
                    photo.approvedDate = reader["ApprovedDate"] != DBNull.Value
                                             ? (DateTime?)reader["ApprovedDate"]
                                             : null;
                    photo.primary = (bool)reader["Primary"];
                    photo.explicitPhoto = (bool)reader["Explicit"];
                    photo.privatePhoto = (bool)reader["Private"];
                    if (reader["FaceCrop"] is string)
                        photo.faceCrop = (string)reader["FaceCrop"];
                    photo.manualApproval = (bool)reader["ManualApproval"];
                    photo.salute = (bool)reader["Salute"];

                    return photo;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Sets the primary photo
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="PhotoID"></param>
        public static void SetPrimary(string username, Photo photo)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteReader(
                                        "SetPrimaryPhoto", username, photo.Id);
            }
            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("Photo_GetPrimary_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
                cacheKey = String.Format("Photo_FetchByUsername_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
                cacheKey = String.Format("Photo_FetchByUsernamePhotoAlbumID_{0}_{1}", username, photo.PhotoAlbumID);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }

                if (Config.Photos.EnablePhotoStack)
                {
                    ImageHelper.DeleteImageStackCache(username);
                }
            }
        }

        /// <summary>
        /// Sets photo as private
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="PhotoID"></param>
        public static void SetAsPrivate(string username, int photoID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteReader(
                                        "SetPhotoAsPrivate", username, photoID);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("User_HasPrivatePhotos_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }

            if (Config.Photos.EnablePhotoStack)
            {
                ImageHelper.DeleteImageStackCache(username);
            }
        }

        public void ApprovePhoto(string adminUsername)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "ApprovePhoto", id, explicitPhoto, privatePhoto, adminUsername);
            }

            string cacheKey = String.Format("Photo_FetchByUsername_{0}", Username);

            if (HttpContext.Current.Cache[cacheKey] != null)
                HttpContext.Current.Cache.Remove(cacheKey);

            cacheKey = String.Format("Photo_FetchByUsernamePhotoAlbumID_{0}_{1}", username, photoAlbumID);
            if (HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }

            if (Config.Photos.EnablePhotoStack)
            {
                ImageHelper.DeleteImageStackCache(username);
            }
        }

        public static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            if (Config.Photos.UseSquareThumbnails && maxWidth == maxHeight
                && maxWidth < Config.Photos.PhotoMaxWidth && maxHeight < Config.Photos.PhotoMaxHeight)
            {
                return ResizeSquareImage(image, maxWidth, maxHeight);
            }

            int newWidth = image.Width, newHeight = image.Height;

            if ((double)maxWidth / (double)image.Width
                < (double)maxHeight / (double)image.Height
                && image.Width > maxWidth)
            {
                newWidth = maxWidth;
                newHeight = Convert.ToInt32(Convert.ToDouble(image.Height)
                                            * ((double)maxWidth / Convert.ToDouble(image.Width)));
            }
            else if ((double)maxHeight / /*(double)*/image.Height
                     <= (double)maxWidth / (double)image.Width
                     && image.Height > maxHeight)
            {
                newHeight = maxHeight;
                newWidth = Convert.ToInt32(Convert.ToDouble(image.Width)
                                           * ((double)maxHeight / Convert.ToDouble(image.Height)));
            }
            else
            {
                return image;
            }

            Bitmap bmp = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, newWidth, newHeight);
            g.Dispose();

            return bmp;
        }

        public static Image ResizeRectangleImage(Image image, int maxWidth, int maxHeight)
        {
            if (Config.Photos.UseSquareThumbnails && maxWidth == maxHeight
                && maxWidth < Config.Photos.PhotoMaxWidth && maxHeight < Config.Photos.PhotoMaxHeight)
            {
                return ResizeSquareImage(image, maxWidth, maxHeight);
            }

            int newWidth = image.Width, newHeight = image.Height;

            Rectangle srcRect;
            double widthFactor = (double)maxWidth / (double)image.Width;
            double heightFactor = (double)maxHeight / (double)image.Height;

            //if (widthFactor < heightFactor && image.Height > maxHeight)
            if (widthFactor < heightFactor && image.Height > maxHeight)
            {
                //var offset = (int)Math.Round(Math.Abs(image.Width*heightFactor - maxWidth)*heightFactor/2);
                //srcRect = new Rectangle(0, offset, image.Width - offset, image.Height);
                var offset = (int)Math.Round(Math.Abs(image.Width * heightFactor - maxWidth) / 2);
                srcRect = new Rectangle(offset, 0, image.Width - offset, image.Height);
            }
            //else if (heightFactor <= widthFactor && image.Width > maxWidth)
            else if (heightFactor <= widthFactor && image.Width > maxWidth)
            {
                //var offset = (int)Math.Round(Math.Abs(image.Height * widthFactor - maxHeight) * widthFactor / 2);
                //srcRect = new Rectangle(offset, 0, image.Width, image.Height - offset);
                var offset = (int)Math.Round(Math.Abs(image.Height * widthFactor - maxHeight) / 2);
                srcRect = new Rectangle(0, offset, image.Width, image.Height - offset);
            }
            else
            {
                return image;
            }

            var destRect = new Rectangle(0, 0, maxWidth, maxHeight);

            var bmp = new Bitmap(maxWidth, maxHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            g.Dispose();

            return bmp;
        }

        public static Image ResizeSquareImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            Rectangle srcRect = image.Width >= image.Height
                                    ? new Rectangle((image.Width - image.Height) / 2, 0, image.Height, image.Height)
                                    : new Rectangle(0, (image.Height - image.Width) / 2, image.Width, image.Width);

            var bmp = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, destRect, srcRect, GraphicsUnit.Pixel);
            g.Dispose();

            return bmp;
        }

        public static void RotateImage(Image image)
        {
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
        }

        public enum eWatermarkPosition
        {
            [Reflection.DescriptionAttribute("TopLeft")]
            TopLeft,
            [Reflection.DescriptionAttribute("TopRight")]
            TopRight,
            [Reflection.DescriptionAttribute("BottomLeft")]
            BottomLeft,
            [Reflection.DescriptionAttribute("BottomRight")]
            BottomRight,
            [Reflection.DescriptionAttribute("Center")]
            Center
        }

        public static void ApplyWatermark(Image image, Image watermark,
                                          int transparency, eWatermarkPosition position)
        {
            Graphics g = Graphics.FromImage(image);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            if (watermark != null)
            {
                float transparencyFactor = (float)(((double)transparency / 255.00) + Double.Epsilon);
                ImageAttributes imageAttributes = new ImageAttributes();
                float[][] colorMatrixElements = {
                                                    new float[] {1.0f, 0.0f, 0.0f, 0.0f, 0.0f},
                                                    new float[] {0.0f, 1.0f, 0.0f, 0.0f, 0.0f},
                                                    new float[] {0.0f, 0.0f, 1.0f, 0.0f, 0.0f},
                                                    new float[] {0.0f, 0.0f, 0.0f, transparencyFactor, 0.0f},
                                                    new float[] {0.0f, 0.0f, 0.0f, 0.0f, 1.0f}
                                                };
                ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                int PosX, PosY;
                switch (position)
                {
                    case eWatermarkPosition.TopLeft:
                        PosX = 0;
                        PosY = 0;
                        break;
                    case eWatermarkPosition.TopRight:
                        PosX = image.Width - watermark.Width;
                        PosY = 0;
                        break;
                    case eWatermarkPosition.BottomLeft:
                        PosX = 0;
                        PosY = image.Height - watermark.Height;
                        break;
                    case eWatermarkPosition.BottomRight:
                    default:
                        PosX = image.Width - watermark.Width;
                        PosY = image.Height - watermark.Height;
                        break;
                    case eWatermarkPosition.Center:
                        PosX = (image.Width - watermark.Width) / 2;
                        PosY = (image.Height - watermark.Height) / 2;
                        break;
                }
                g.DrawImage(watermark,
                            Rectangle.FromLTRB(
                                PosX,
                                PosY,
                                PosX + watermark.Width,
                                PosY + watermark.Height),
                            0, 0, watermark.Width, watermark.Height,
                            GraphicsUnit.Pixel,
                            imageAttributes);
            }

            g.Dispose();
        }

        public static FaceFinderPlugin.FaceRegion[] FindFaceRegions(Image image)
        {
            var ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            byte[] binaryImage = ms.ToArray();
            ms.Dispose();

            return FaceFinderPlugin.FindFaces(binaryImage);
        }

        public static Image CropFace(Image image, FaceFinderPlugin.FaceRegion region,
            int preferredWidth, int preferredHeight, bool expandCrop)
        {
            var bmpOriginal = new Bitmap(image);

            Rectangle rectCrop;
            if (expandCrop)
            {
                const double expandRatio = 0.9;
                int expandWidth = Convert.ToInt32(region.Width * expandRatio);
                if (region.Width + expandWidth < preferredWidth)
                    expandWidth = preferredWidth - region.Width;
                int expandHeight = Convert.ToInt32(region.Height * expandRatio);
                if (region.Height + expandHeight < preferredHeight)
                    expandHeight = preferredHeight - region.Height;

                rectCrop = new Rectangle(region.X - expandWidth / 2, region.Y - expandHeight / 2,
                                             region.Width + expandWidth, region.Height + expandHeight);
            }
            else
            {
                rectCrop = new Rectangle(region.X, region.Y, region.Width, region.Height);
            }

            if (rectCrop.X < 0) rectCrop.X = 0;
            if (rectCrop.Y < 0) rectCrop.Y = 0;
            if (rectCrop.X + rectCrop.Width > image.Width) rectCrop.Width = image.Width - rectCrop.X;
            if (rectCrop.Y + rectCrop.Height > image.Height) rectCrop.Height = image.Height - rectCrop.Y;
            var bmpCrop = new Bitmap(rectCrop.Width, rectCrop.Height, bmpOriginal.PixelFormat);
            Graphics gphCrop = Graphics.FromImage(bmpCrop);
            var rectDest = new Rectangle(0, 0, rectCrop.Width, rectCrop.Height);
            gphCrop.DrawImage(bmpOriginal, rectDest, rectCrop.X, rectCrop.Y, rectCrop.Width, rectCrop.Height,
                              GraphicsUnit.Pixel);
            gphCrop.Dispose();
            return bmpCrop;
        }

        public static Image CreatePhotoStack(string username, int width, int height)
        {
            const double resizeRatio = 0.65, primaryResizeRatio = 0.8;
            var photos = Fetch(username);
            if (photos == null || photos.Count() == 0)
                throw new NotFoundException
                    (Lang.Trans("The user doesn't have any photos!"));

            var rand = new Random();
            var bmp = new Bitmap(width, height);
            var g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            var topPhotos = photos.Where(p => p.Approved)
                .OrderByDescending(p => p.AverageRating + (p.Primary ? 100 : 0))
                .Take(4).Reverse();
            if (topPhotos.Count() > 0 && !topPhotos.Any(p => p.Primary)) topPhotos.Last().Primary = true;
            foreach (var photo in topPhotos)
            {
                if (photo.Primary)
                {
                    Image image = photo.Image;
                    if (photo.FaceCrop != null)
                    {
                        string[] coords = photo.FaceCrop.Split('|');
                        var faceRegion = new FaceFinderPlugin.FaceRegion
                                             {
                                                 X = int.Parse(coords[0]),
                                                 Y = int.Parse(coords[1]),
                                                 Width = int.Parse(coords[2]),
                                                 Height = int.Parse(coords[3])
                                             };
                        image = CropFace(image, faceRegion, (int)(width * primaryResizeRatio),
                                         (int)(height * primaryResizeRatio), true);
                    }
                    image = ResizeImage(image, (int)(width * primaryResizeRatio),
                                        (int)(height * primaryResizeRatio));
                    var x = (width - image.Width) / 2;
                    var y = (height - image.Height) / 2;

                    g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
                    g.RotateTransform(rand.Next(-5, +5));
                    g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);

                    g.DrawImage(image, x, y);
                    g.DrawLines(new Pen(Color.FromArgb(100, Color.Gray), 2),
                        new[] { new Point(x + image.Width + 3, y + 1), 
                            new Point(x + image.Width + 3, y + image.Height + 3),
                            new Point(x + 1, y + image.Height + 3) });
                    g.DrawLines(new Pen(Color.FromArgb(40, Color.Gray), 2),
                        new[] { new Point(x + image.Width + 5, y + 3), 
                            new Point(x + image.Width + 5, y + image.Height + 5),
                            new Point(x + 3, y + image.Height + 5) });
                    g.DrawRectangle(new Pen(Color.White, 3), x - 1, y - 1,
                        image.Width + 1, image.Height + 1);
                    g.DrawRectangle(new Pen(Color.FromArgb(150, Color.DarkGray), 1), x - 3, y - 3,
                        image.Width + 5, image.Height + 5);
                }
                else
                {
                    var image = ResizeImage(photo.Image, (int)(width * resizeRatio), (int)(height * resizeRatio));
                    var angle = rand.Next(-20, +20);
                    int minX, maxX, minY, maxY;
                    do
                    {
                        var angleRad = Math.PI * angle / 180.0;

                        var offsetX = (int)((image.Height + 7) * Math.Sin(Math.Abs(angleRad)));
                        minX = offsetX + 3;
                        maxX = width - image.Width - 4 - offsetX;

                        var offsetY = (int)((image.Width + 7) * Math.Sin(Math.Abs(angleRad)));
                        minY = offsetY + 3;
                        maxY = height - image.Height - 4 - offsetY;
                        angle = (int)(angle * 0.9);
                    } while (minX > maxX || minY > maxY);

                    var x = rand.Next(minX, maxX);
                    var y = rand.Next(minY, maxY);

                    g.TranslateTransform((float)image.Width / 2, (float)image.Height / 2);
                    g.RotateTransform(angle);
                    g.TranslateTransform(-(float)image.Width / 2, -(float)image.Height / 2);
                    g.DrawImage(image, x, y);
                    g.DrawLines(new Pen(Color.FromArgb(100, Color.Gray), 2),
                        new[] { new Point(x + image.Width + 3, y + 1), 
                            new Point(x + image.Width + 3, y + image.Height + 3),
                            new Point(x + 1, y + image.Height + 3) });
                    g.DrawLines(new Pen(Color.FromArgb(40, Color.Gray), 2),
                        new[] { new Point(x + image.Width + 5, y + 3), 
                            new Point(x + image.Width + 5, y + image.Height + 5),
                            new Point(x + 3, y + image.Height + 5) });
                    g.DrawRectangle(new Pen(Color.White, 3), x - 1, y - 1,
                        image.Width + 1, image.Height + 1);
                    g.DrawRectangle(new Pen(Color.FromArgb(150, Color.DarkGray), 1), x - 3, y - 3,
                        image.Width + 5, image.Height + 5);
                }

                g.ResetTransform();
            }

            g.Dispose();

            return bmp;
        }
    }

    [Serializable]
    public class PhotoSearchResults : SearchResults<int, Photo>
    {
        public int[] Photos
        {
            get
            {
                if (Results == null)
                    return new int[0];
                return Results;
            }
            set { Results = value; }
        }

        public new int GetTotalPages(int photosPerPage)
        {
            return base.GetTotalPages(photosPerPage);
        }

        protected override Photo LoadResult(int id)
        {
            return Photo.Fetch(id);
        }
        protected override Photo[] LoadResults(int[] ids)
        {
            List<int> keysList = ids.ToList();
            List<Photo> resultList = new List<Photo>();
            if (keysList.Count > 0)
            {
                var parameterizedThreadsList = new ParameterizedThreadsList<int, Photo>(
                                                    keysList, resultList,
                                                    (new LoadPhotosResultThreadClass(keysList, resultList)).DoWorkThread,
                                                    HttpContext.Current);
                parameterizedThreadsList.RunAsBackgroundThreads = true;
                parameterizedThreadsList.ExecuteParallelWork(5);
            }
            return resultList.ToArray();
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="photosPerPage">photosPerPage</param>
        /// <returns>Array of photos</returns>
        public new Photo[] GetPage(int Page, int photosPerPage)
        {
            return base.GetPage(Page, photosPerPage);
        }
    }
}