using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class AdPhoto
    {
        #region fields

        private int? id = null;
        private int adID;
        private string description = String.Empty;
        private System.Drawing.Image image;

        #endregion

        #region Constructors

        private AdPhoto()
        {
        }

        public AdPhoto(int adID)
        {
            this.adID = adID;
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

        public int AdID
        {
            get { return adID; }
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
        /// Gets or sets the image.
        /// Throws "NotFoundException".
        /// </summary>
        /// <value>The image.</value>
        public System.Drawing.Image Image
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

                if (image.Width > Config.Ads.AdPhotoMaxWidth
                    || image.Height > Config.Ads.AdPhotoMaxHeight)
                    image = Photo.ResizeImage(image, Config.Ads.AdPhotoMaxWidth,
                        Config.Ads.AdPhotoMaxHeight);
            }
        }

        #endregion

        #region Methods

        public static AdPhoto[] Fetch()
        {
            return Fetch(null, null, null);
        }

        public static AdPhoto Fetch(int id)
        {
            AdPhoto[] adPhotos = Fetch(id, null, null);

            if (adPhotos.Length > 0)
            {
                return adPhotos[0];
            }
            return null;
        }

        public static AdPhoto[] FetchByAdID(int adID)
        {
            return Fetch(null, adID, null);
        }

        private static AdPhoto[] Fetch(int? id, int? adID, int? numberOfPhotos)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchAdPhotos",
                                                                id, adID, numberOfPhotos);

                List<AdPhoto> lAdPhotos = new List<AdPhoto>();

                while (reader.Read())
                {
                    AdPhoto adPhoto = new AdPhoto();

                    adPhoto.id = (int)reader["ID"];
                    adPhoto.adID = (int)reader["AdID"];
                    adPhoto.description = (string)reader["Description"];

                    lAdPhotos.Add(adPhoto);
                }

                return lAdPhotos.ToArray();
            }
        }

        public void Save()
        {
            MemoryStream imageStream = new MemoryStream();
            Image.Save(imageStream, ImageFormat.Jpeg);
            imageStream.Position = 0;
            BinaryReader reader = new BinaryReader(imageStream);
            byte[] bytesImage = reader.ReadBytes((int)imageStream.Length);

            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveAdPhoto",
                                                        id, adID, description, bytesImage);

                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static void Delete(int id)
        {
            Delete(id, null);
        }

        private static void Delete(int? id, int? adID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteAdPhoto", id, adID);
            }
        }

        public static System.Drawing.Image LoadImage(int adPhotoID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader( "FetchAdPhoto", adPhotoID);

                if (reader.Read())
                {
                    byte[] buffer = (byte[])reader["Image"];
                    MemoryStream imageStream = new MemoryStream(buffer);
                    return System.Drawing.Image.FromStream(imageStream);
                }
                throw new NotFoundException
                    (Lang.Trans("The requested ad photo does not exist!"));
            }
        }

        #endregion
    }
}
