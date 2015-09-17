/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class GroupPhoto
    {
        #region fields

        private int? id = null;
        private int groupID;
        private string username;
        private string name = "";
        private string description = "";
        private DateTime date = DateTime.Now;
        private Image image;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            DateUploaded,
        }

        #endregion

        #region Constructors

        private GroupPhoto()
        {
        }

        public GroupPhoto(int groupID, string username)
        {
            this.groupID = groupID;
            this.username = username;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
        /// Throws "Exception" exception.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get
            {
                if (id.HasValue)
                {
                    return id.Value;
                }
                else
                {
                    throw new Exception("The field ID is not set!");
                }
            }
        }

        /// <summary>
        /// Gets the group ID.
        /// The property is read-only.
        /// </summary>
        /// <value>The group ID.</value>
        public int GroupID
        {
            get { return groupID; }
        }

        /// <summary>
        /// Gets the username.
        /// The property is read-only.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Gets or sets the image.
        /// Throws "NotFoundException".
        /// </summary>
        /// <value>The image.</value>
        public Image Image
        {
            get
            {
                if (image == null)
                    image = LoadImage(ID);
                return image;
            }
            set
            {
                image = value;

                if (image.Width > Config.Groups.GroupPhotoMaxWidth
                    || image.Height > Config.Groups.GroupPhotoMaxHeight)
                    image = ResizeImage(image, Config.Groups.GroupPhotoMaxWidth, 
                        Config.Groups.GroupPhotoMaxHeight);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group photos from DB.
        /// If there are no group photos in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupPhoto[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches group photo from DB by specified id.
        /// If the group photo doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupPhoto Fetch(int id)
        {
            GroupPhoto[] groupPhotos = Fetch(id, null, null, null, null, null, eSortColumn.None);

            if (groupPhotos.Length > 0)
            {
                return groupPhotos[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all group photos from DB by specified group ID.
        /// If there are no group photos in DB it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static GroupPhoto[] FetchByGroupID(int groupID, eSortColumn sortColumn)
        {
            string cacheKey = String.Format("GroupPhoto_FetchByGroupID_{0}", groupID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as GroupPhoto[];
            }

            GroupPhoto[] groupPhotos = Fetch(null, groupID, null, null, null, null, sortColumn);

            if (groupPhotos.Length > 0 && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, groupPhotos, null, DateTime.Now.AddMinutes(15),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return groupPhotos;
        }

        /// <summary>
        /// Fetches all group photos from DB by specified username.
        /// If there are no group photos in DB it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupPhoto[] Fetch(string username)
        {
            string cacheKey = String.Format("GroupPhoto_FetchByUsername_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as GroupPhoto[];
            }

            GroupPhoto[] groupPhotos = Fetch(null, null, username, null, null, null, eSortColumn.None);

            if (groupPhotos.Length > 0 && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, groupPhotos, null, DateTime.Now.AddMinutes(15),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return groupPhotos;
        }

        /// <summary>
        /// Fetches all group photos from DB by specified group ID and username.
        /// If ther are no group photos in DB by speficied arguments it return an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupPhoto[] Fetch(int groupID, string username)
        {
            return Fetch(null, groupID, username, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches the specified number of photos for the specified group ID
        /// and sorts them by specified column.
        /// If ther are no group photos in DB by speficied arguments it return an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="numberOfPhotos">The number of photos.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static GroupPhoto[] Fetch(int groupID, int numberOfPhotos, eSortColumn sortColumn)
        {
            string cacheKey = String.Format("GroupPhoto_FetchByGroupIDAndNumberOfPhotos_{0}", groupID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as GroupPhoto[];
            }

            GroupPhoto[] groupPhotos = Fetch(null, groupID, null, null, null, numberOfPhotos, sortColumn);

            if (groupPhotos.Length > 0 && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, groupPhotos, null, DateTime.Now.AddMinutes(15),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return groupPhotos;
        }


        /// <summary>
        /// Fetches group photos by specified arguments.
        /// It returns an empty array if there are no group photos in DB by specified arguments.
        /// If these arguments are null it returns all group photos from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <param name="date">The date.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static GroupPhoto[] Fetch(int? id, int? groupID, string username,
                                            string name, DateTime? date, int? numberOfPhotos, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                List<GroupPhoto> lGroupPhotos = new List<GroupPhoto>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchGroupPhotos",
                                                                    id, groupID, username, name, date, numberOfPhotos,
                                                                    sortColumn))
                {

                    while (reader.Read())
                    {
                        GroupPhoto groupPhoto = new GroupPhoto();

                        groupPhoto.id = (int) reader["ID"];
                        groupPhoto.groupID = (int) reader["GroupID"];
                        groupPhoto.username = (string) reader["Username"];
                        groupPhoto.name = (string) reader["Name"];
                        groupPhoto.description = (string) reader["Description"];
                        groupPhoto.date = (DateTime) reader["Date"];

                        lGroupPhotos.Add(groupPhoto);
                    }
                    reader.Close();
                }

                return lGroupPhotos.ToArray();
            }
        }

        public static int[] Search(int? groupID, string username, string name, DateTime? date, string keyword, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                List<int> lGroupPhotosIDs = new List<int>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("SearchGroupPhotos",
                                                                    groupID, username, name,
                                                                    date, keyword, sortColumn))
                {

                    while (reader.Read())
                    {
                        lGroupPhotosIDs.Add((int) reader["ID"]);
                    }
                    reader.Close();
                }
                return lGroupPhotosIDs.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance into DB.
        /// If the ID of this instance is NULL it inserts a new record in DB and returns the generated ID
        /// otherwise updates this instance.
        /// </summary>
        public void Save()
        {
            MemoryStream imageStream = new MemoryStream();
            Image.Save(imageStream, ImageFormat.Jpeg);
            imageStream.Position = 0;
            BinaryReader reader = new BinaryReader(imageStream);
            byte[] bytesImage = reader.ReadBytes((int)imageStream.Length);

            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveGroupPhoto",
                                                        id, groupID, username, name, description, date, bytesImage);

                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("GroupPhoto_FetchByUsername_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
                cacheKey = String.Format("GroupPhoto_FetchByGroupID_{0}", groupID);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
                cacheKey = String.Format("GroupPhoto_FetchByGroupIDAndNumberOfPhotos_{0}", groupID);
                if (HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        /// <summary>
        /// Deletes group photo from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            Delete(id, null, null);
        }

        /// <summary>
        /// Deletes all group photos from DB by specified group ID and username.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        public static void Delete(int groupID, string username)
        {
            Delete(null, groupID, username);
        }

        /// <summary>
        /// Deletes all group photos from DB by specified arguments.
        /// If all argumets are NULL it deletes all group photos!
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        private static void Delete(int? id, int? groupID, string username)
        {
            GroupPhoto groupPhoto = null;
            if (id.HasValue) groupPhoto = Fetch(id.Value);
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteGroupPhoto", id, groupID, username);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey;
                if (username != null || groupPhoto != null)
                {
                    cacheKey = String.Format("GroupPhoto_FetchByUsername_{0}", username ?? groupPhoto.username);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }
                if (groupID.HasValue || groupPhoto != null)
                {
                    cacheKey = String.Format("GroupPhoto_FetchByGroupID_{0}", groupID ?? groupPhoto.GroupID);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }
                if (groupID.HasValue || groupPhoto != null)
                {
                    cacheKey = String.Format("GroupPhoto_FetchByGroupIDAndNumberOfPhotos_{0}", groupID ?? groupPhoto.GroupID);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                    {
                        HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }
            }
        }

        /// <summary>
        /// Loads the image from DB.
        /// Throws "NotFoundException".
        /// </summary>
        /// <param name="groupPhotoID">The group photo ID.</param>
        /// <returns></returns>
        public static Image LoadImage(int groupPhotoID)
        {
            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchGroupPhoto", groupPhotoID))
                {

                    if (reader.Read())
                    {
                        byte[] buffer = (byte[]) reader["Image"];
                        reader.Close();
                        MemoryStream imageStream = new MemoryStream(buffer);
                        return System.Drawing.Image.FromStream(imageStream);
                    }
                    else
                    {
                        throw new NotFoundException
                            (Lang.Trans("The requested group photo does not exist!"));
                    }
                }
            }
        }

        public static Image ResizeImage(Image image, int MaxWidth, int MaxHeight)
        {
            int newWidth = image.Width, newHeight = image.Height;

            if ((double)MaxWidth / (double)image.Width
                < (double)MaxHeight / (double)image.Height
                && image.Width > MaxWidth)
            {
                newWidth = MaxWidth;
                newHeight = Convert.ToInt32(Convert.ToDouble(image.Height)
                                            * ((double)MaxWidth / Convert.ToDouble(image.Width)));
            }
            else if ((double)MaxHeight / /*(double)*/image.Height
                     <= (double)MaxWidth / (double)image.Width
                     && image.Height > MaxHeight)
            {
                newHeight = MaxHeight;
                newWidth = Convert.ToInt32(Convert.ToDouble(image.Width)
                                           * ((double)MaxHeight / Convert.ToDouble(image.Height)));
            }
            else
            {
                return image;
            }

            Bitmap bmp = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.DrawImage(image, 0, 0, newWidth, newHeight);

            return bmp;
        }

        /// <summary>
        /// Counts how many group photos the specified group id has.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static int Count(int groupID)
        {
            return Count(groupID, null);
        }

        public static int Count(string username)
        {
            return Count(null, username);
        }

        private static int Count(int? groupID, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.GetDB().ExecuteScalar( "FetchGroupPhotosCount", groupID, username));
            }
    }

        #endregion
    }

    [Serializable]
    public class GroupPhotoSearchResults : SearchResults<int, GroupPhoto>
    {
        public int[] GroupPhotos
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

        protected override GroupPhoto LoadResult(int id)
        {
            return GroupPhoto.Fetch(id);
        }
        protected override GroupPhoto[] LoadResults(int[] ids)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Use this method to get the search results.
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <param name="photosPerPage">The topics per page.</param>
        /// <returns></returns>
        public new GroupPhoto[] GetPage(int Page, int photosPerPage)
        {
            return base.GetPage(Page, photosPerPage);
        }
    }
}
