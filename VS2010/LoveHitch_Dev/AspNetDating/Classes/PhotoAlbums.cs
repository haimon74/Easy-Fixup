using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class PhotoAlbum
    {
        #region fields

        private int? id = null;
        private string username = null;
        private string name = String.Empty;
        private eAccess access = eAccess.All;
        private int? coverPhotoID = null;
        
        public enum eAccess
        {
            All,
            FriendsOnly,
            FriendsAndTheirFriends
        }

        #endregion

        #region Constructors

        private PhotoAlbum(){}

        public PhotoAlbum(string username)
        {
            this.username = username;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
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

        public string Username
        {
            get { return username; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public eAccess Access
        {
            get { return access; }
            set { access = value; }
        }

        public int? CoverPhotoID
        {
            get { return coverPhotoID; }
            set { coverPhotoID = value; }
        }

        #endregion

        #region Methods

        public static PhotoAlbum[] Fetch()
        {
            return Fetch(null, null, null, null);
        }

        public static PhotoAlbum Fetch(int id)
        {
            string cacheKey = String.Format("PhotoAlbum_Fetch_Id_{0}", id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as PhotoAlbum;
            }

            PhotoAlbum[] photoAlbums = Fetch(id, null, null, null);

            if (photoAlbums.Length > 0 && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, photoAlbums[0], null, DateTime.Now.AddMinutes(10),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return photoAlbums.Length > 0 ? photoAlbums[0] : null;
        }

        public static PhotoAlbum[] Fetch(string username)
        {
            return Fetch(null, username, null, null);
        }

        private static PhotoAlbum[] Fetch(int? id, string username, string name, eAccess? access)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn,
                                                               "FetchPhotoAlbums",
                                                               id, username, name, access);

                List<PhotoAlbum> photoAlbums = new List<PhotoAlbum>();

                while (reader.Read())
                {
                    PhotoAlbum photoAlbum = new PhotoAlbum();

                    photoAlbum.id = (int)reader["ID"];
                    photoAlbum.username = (string) reader["Username"];
                    photoAlbum.name = (string) reader["Name"];
                    photoAlbum.access = (eAccess) reader["Access"];
                    photoAlbum.coverPhotoID = reader["CoverPhotoID"] != DBNull.Value ? (int?) reader["CoverPhotoID"] : null;

                    photoAlbums.Add(photoAlbum);
                }

                return photoAlbums.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SavePhotoAlbum",
                                                            id, username, name, access, coverPhotoID);

                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }

                string cacheKey = String.Format("PhotoAlbum_Fetch_Id_{0}", id);
                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeletePhotoAlbum", id);
            }

            string cacheKey = String.Format("PhotoAlbum_Fetch_Id_{0}", id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        #endregion
    }
}
