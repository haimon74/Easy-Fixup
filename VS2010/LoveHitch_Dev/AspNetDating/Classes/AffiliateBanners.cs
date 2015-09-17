using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Microsoft.ApplicationBlocks.Data;


namespace AspNetDating.Classes
{
    public class AffiliateBanner
    {
        #region fields

        private int? id = null;
        private string name = "";
        private bool deleted = false;
        private Image image = null;

        #endregion

        #region Properties

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
                    return -1;
                }
            }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public bool Deleted
        {
            get { return deleted; }
            set { deleted = value; }
        }

        /// <summary>
        /// Gets or sets the image.
        /// Throws "NotFoundException exception" if the specified affiliate banner image doesn't exist.
        /// </summary>
        /// <value>The image.</value>
        public Image Image
        {
            get
            {
                if (image == null)
                {
                    LoadImage();
                }

                return image;
            }

            set
            {
                image = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches affiliate banners by specified arguments.
        /// It returns an empty array if there are no affiliate banners in DB by specified arguments.
        /// If these arguments are null it returns all affiliate banners from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="deleted">The deleted.</param>
        /// <returns></returns>
        private static AffiliateBanner[] Fetch(int? id, string name, bool? deleted)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchAffiliateBanners", id, name, deleted);

                List<AffiliateBanner> lAffiliateBanners = new List<AffiliateBanner>();

                while (reader.Read())
                {
                    AffiliateBanner affiliateBanner = new AffiliateBanner();

                    affiliateBanner.id = (int) reader["ID"];
                    affiliateBanner.name = (string) reader["Name"];
                    affiliateBanner.deleted = (bool) reader["Deleted"];

                    lAffiliateBanners.Add(affiliateBanner);
                }

                return lAffiliateBanners.ToArray();
            }
        }

        /// <summary>
        /// Fetches all affiliate banners from DB.
        /// If there are no affiliate banners in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static AffiliateBanner[] Fetch()
        {
            return Fetch(null, null, null);
        }

        /// <summary>
        /// Fetches affiliate banner by specified id from DB.
        /// If the affiliate banner doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static AffiliateBanner Fetch(int id)
        {
            AffiliateBanner[] affiliateBanners = Fetch(id, null, null);

            if (affiliateBanners.Length > 0)
            {
                return affiliateBanners[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all deleted/not deleted affiliate banners from DB.
        /// If there are no deleted/not deleted affiliate banners in DB it returns an empty array.
        /// </summary>
        /// <param name="deleted">if set to <c>true</c> [deleted].</param>
        /// <returns></returns>
        public static AffiliateBanner[] Fetch(bool deleted)
        {
            return Fetch(null, null, deleted);
        }

        /// <summary>
        /// Saves this instance in DB. If the field id is null it inserts new record in DB,
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            MemoryStream imageStream = new MemoryStream();
            Image.Save(imageStream, Image.RawFormat);
            imageStream.Position = 0;
            BinaryReader reader = new BinaryReader(imageStream);
            byte[] bytesImage = reader.ReadBytes((int)imageStream.Length);

            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveAffiliateBanner",
                                                        id, name, deleted, bytesImage);

                if (!id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes affiliate banner from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteAffiliateBanner", id);
            }
        }

        /// <summary>
        /// Loads the image for this affiliate banner.
        /// Throws "NotFoundException exception" if the specified affiliate banner image doesn't exist.
        /// </summary>
        public void LoadImage()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchAffiliateBannerImage", ID);

                if (reader.Read())
                {
                    byte[] buffer = (byte[])reader["Image"];
                    MemoryStream imageStream = new MemoryStream(buffer);
                    image = Image.FromStream(imageStream);    
                }
                else
                {
                    throw new NotFoundException(Lang.Trans("The requested affiliate banner image does not exist!"));
                }
            }
        }

        /// <summary>
        /// Deletes the image for specified affiliate banner ID.
        /// </summary>
        /// <param name="affiliateBannerID">The affiliate banner ID.</param>
        public static void DeleteImage(int affiliateBannerID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn,
                                          "DeleteGroupIcon", affiliateBannerID);
            }
        }

        #endregion
    }
}
