using System;
using System.Data.Common;
using System.Linq;

namespace ezFixUp.Classes
{
    public class PhotoComment
    {
        #region fields

        private int? id = null;
        private int photoID;
        private string username;
        private string comment = String.Empty;
        private DateTime date = DateTime.Now;

        #endregion

        #region Constructors

        private PhotoComment()
        {
        }

        public PhotoComment(int photoID, string username)
        {
            this.photoID = photoID;
            this.username = username;
        }

        #endregion

        #region Properties

        public int? ID
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

        public int PhotoID
        {
            get { return photoID; }
        }

        public string Username
        {
            get { return username; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all photo comments from DB.
        /// If there are no photo comments in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static PhotoComment[] Fetch()
        {
            return Fetch(null, null, null, null);
        }

        /// <summary>
        /// Fetches photo comment by specified id from DB. If the photo comment doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static PhotoComment Fetch(int id)
        {
            PhotoComment[] photoComments = Fetch(id, null, null, null);

            if (photoComments.Length > 0)
            {
                return photoComments[0];
            }
            else
            {
                return null;
            }
        }

        public static PhotoComment[] Fetch(int photoID, string username)
        {
            return Fetch(null, photoID, username, null);
        }

        /// <summary>
        /// Fetches photo comments by specified photo id.
        /// If there are no photo comments in DB for the specified photo id it returns an empty array.
        /// </summary>
        /// <param name="photoID">The photo ID.</param>
        /// <returns></returns>
        public static PhotoComment[] FetchByPhotoID(int photoID)
        {
            return Fetch(null, photoID, null, null);
        }

        /// <summary>
        /// Fetches specified number of photo comments by specified photo id.
        /// If there are no photo comments in DB for the specified photo id it returns an empty array.
        /// </summary>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="numberOfComments">The number of comments.</param>
        /// <returns></returns>
        public static PhotoComment[] FetchByPhotoID(int photoID, int numberOfComments)
        {
            return Fetch(null, photoID, null, numberOfComments);
        }

        /// <summary>
        /// Fetches photo comments by specified username
        /// If there are no photo comments in DB for the specified username it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static PhotoComment[] Fetch(string username)
        {
            return Fetch(null, null, username, null);
        }

        /// <summary>
        /// Fetches photo comments by specified arguments.
        /// It returns an empty array if there are no photo comments in DB by specified arguments.
        /// If these arguments are null it returns all photo comments from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="numberOfComments">The number of comments.</param>
        /// <returns></returns>
        private static PhotoComment[] Fetch(int? id, int? photoID, string username, int? numberOfComments)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var photoComments = from pc in db.PhotoComments
                                    where (!id.HasValue || id == pc.pc_id)
                                          && (!photoID.HasValue || photoID == pc.p_id)
                                          && (username == null || username == pc.u_username)
                                    orderby pc.pc_date descending
                                    select new PhotoComment
                                               {
                                                   id = pc.pc_id,
                                                   photoID = pc.p_id,
                                                   username = pc.u_username,
                                                   comment = pc.pc_comment,
                                                   date = pc.pc_date
                                               };
                if (numberOfComments.HasValue)
                    photoComments = photoComments.Take(numberOfComments.Value);

                return photoComments.ToArray();
            }

            ////using (var conn = Config.DB.Open())
            //{
            //    var reader = 
            //        SqlHelper.GetDB().ExecuteReader( "FetchPhotoComments", id, photoID, username, numberOfComments);

            //    List <PhotoComment> lPhotoComments = new List<PhotoComment>();

            //    while (reader.Read())
            //    {
            //        PhotoComment photoComment = new PhotoComment();

            //        photoComment.id = (int) reader["ID"];
            //        photoComment.photoID = (int) reader["PhotoID"];
            //        photoComment.username = (string) reader["Username"];
            //        photoComment.comment = (string) reader["Comment"];
            //        photoComment.date = (DateTime) reader["Date"];

            //        lPhotoComments.Add(photoComment);
            //    }

            //    return lPhotoComments.ToArray();
            //}
        }

        public static int FetchNewPhotoCommentsCount(string username)
        {
            //using (var conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.GetDB().ExecuteScalar( "FetchNewPhotoCommentsCount", username));
            }
        }

        /// <summary>
        /// Saves this instance in DB. If the field id is null it inserts new record in DB,
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.GetDB().ExecuteScalar( "SavePhotoComment", id, photoID, username, comment, date);

                if (!id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes photo comment from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            DeleteBy(id, null, null);
        }

        /// <summary>
        /// Deletes photo comments from DB by specified arguments.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="username">The username.</param>
        private static void DeleteBy(int? id, int? photoID, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeletePhotoComments", id, photoID, username);
            }
        }

        #endregion
    }
}
