/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;


namespace ezFixUp.Classes
{
    /// <summary>
    /// This class handles the user rating
    /// </summary>
    public class UserRating
    {
        #region Properties

        private int votes = 0;

        /// <summary>
        /// Gets the votes.
        /// </summary>
        /// <value>The votes.</value>
        public int Votes
        {
            get { return votes; }
        }

        private decimal averageVote = 0;

        /// <summary>
        /// Gets the average vote.
        /// </summary>
        /// <value>The average vote.</value>
        public decimal AverageVote
        {
            get { return averageVote; }
        }

        #endregion

        private UserRating()
        {
        }

        /// <summary>
        /// Fetches the rating.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static UserRating FetchRating(string username)
        {
            string cacheKey = String.Format("UserRating_FetchRating_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as UserRating;
            }

            UserRating rating = new UserRating();
            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchRating", username))
                {

                    if (reader.Read())
                    {
                        if (reader["AverageVote"] == DBNull.Value)
                            throw new NotFoundException("There are no votes for the specified user!");

                        rating.votes = (int) reader["Votes"];
                        rating.averageVote = Convert.ToDecimal(reader["AverageVote"]);
                    }
                    reader.Close();
                }
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, rating, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromMinutes(30), CacheItemPriority.NotRemovable, null);
            }

            return rating;
        }

        /// <summary>
        /// Rates the user.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="toUser">To user.</param>
        /// <param name="rating">The rating.</param>
        public static void RateUser(string fromUser, string toUser, int rating)
        {
            if (rating < Config.Ratings.MinRating
                || rating > Config.Ratings.MaxRating)
            {
                throw new ArgumentException("Invalid rating!");
            }

            if (fromUser == toUser)
            {
                throw new ArgumentException("You can't rate yourself!");
            }

            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "RateUser", fromUser, toUser, rating);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("UserRating_FetchRating_{0}", toUser);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
                cacheKey = String.Format("UserRating_FetchAverageVote_{0}", fromUser);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Fetches the vote.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="toUser">To user.</param>
        /// <returns></returns>
        public static int FetchVote(string fromUser, string toUser)
        {
            //using (var conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.GetDB().ExecuteScalar( "FetchVote", fromUser, toUser);

                if (result == null || result == DBNull.Value)
                {
                    throw new NotFoundException("There is no such vote!");
                }

                int vote = Convert.ToInt32(result);
                return vote;
            }
        }

        /// <summary>
        /// Fetches the average vote.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <returns></returns>
        public static UserRating FetchAverageVote(string fromUsername)
        {
            string cacheKey = String.Format("UserRating_FetchAverageVote_{0}", fromUsername);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as UserRating;
            }

            UserRating rating = new UserRating();

            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "FetchAverageVote", fromUsername);

                if (reader.Read())
                {
                    if (reader["AverageVote"] == DBNull.Value)
                        throw new NotFoundException("There are no votes from the specified user!");

                    rating.votes = (int) reader["Votes"];
                    rating.averageVote = Convert.ToDecimal(reader["AverageVote"]);
                }
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, rating, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromMinutes(30), CacheItemPriority.NotRemovable, null);
            }

            return rating;
        }
    }

    /// <summary>
    /// This class handles the photo ratings
    /// </summary>
    public class PhotoRating
    {
        #region Properties

        private int votes = 0;

        /// <summary>
        /// Gets the votes.
        /// </summary>
        /// <value>The votes.</value>
        public int Votes
        {
            get { return votes; }
        }

        private decimal averageVote = 0;

        /// <summary>
        /// Gets the average vote.
        /// </summary>
        /// <value>The average vote.</value>
        public decimal AverageVote
        {
            get { return averageVote; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="PhotoRating"/> class.
        /// </summary>
        /// <param name="photoID">The photo ID.</param>
        public PhotoRating(int photoID)
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "FetchPhotoRating", photoID);

                if (reader.Read())
                {
                    if (reader["AverageVote"] == DBNull.Value)
                        throw new NotFoundException("There are no votes for the specified photo!");

                    votes = (int) reader["Votes"];
                    averageVote = Convert.ToDecimal(reader["AverageVote"]);
                }
            }
        }

        /// <summary>
        /// Fetches the rating.
        /// </summary>
        /// <param name="photoID">The photo ID.</param>
        /// <returns></returns>
        public static PhotoRating FetchRating(int photoID)
        {
            return new PhotoRating(photoID);
        }

        /// <summary>
        /// Rates the photo.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="rating">The rating.</param>
        public static void RatePhoto(string fromUser, int photoID, int rating)
        {
            if (rating < Config.Ratings.MinRating
                || rating > Config.Ratings.MaxRating)
            {
                throw new ArgumentException("Invalid rating!");
            }

            Photo[] photos = Photo.Fetch(fromUser);
            if (photos != null)
                foreach (Photo photo in photos)
                {
                    if (photo.Id == photoID)
                        throw new ArgumentException("You can't rate your photo!");
                }

            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "RatePhoto", fromUser, photoID, rating);
            }
        }

        /// <summary>
        /// Fetches the vote.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="photoID">The photo ID.</param>
        /// <returns></returns>
        public static int FetchVote(string fromUser, int photoID)
        {
            //using (var conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.GetDB().ExecuteScalar( "FetchPhotoVote", fromUser, photoID);

                if (result == null || result == DBNull.Value)
                {
                    throw new NotFoundException("There is no such vote!");
                }

                int vote = Convert.ToInt32(result);
                return vote;
            }
        }
    }
}