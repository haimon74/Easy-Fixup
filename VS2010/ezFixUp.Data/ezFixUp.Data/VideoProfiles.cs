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
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    /// <summary>
    /// Summary description for VideoProfiles.
    /// </summary>
    public class VideoProfile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VideoProfile"/> class.
        /// </summary>
        public VideoProfile()
        {
        }

        #region Properties

//		private string username;
//		public string Username
//		{
//			get{return username;}
//		}
//		private bool approved = false;
//		public bool Approved
//		{
//			get{return approved;}
//		}

        #endregion

        /// <summary>
        /// Saves the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void Save(string username)
        {
            Save(username, null);
        }

        /// <summary>
        /// Saves the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="isPrivate">if set to <c>true</c> [is private].</param>
        public static void Save(string username, bool isPrivate)
        {
            Save(username, (bool?)isPrivate);
        }

        private static void Save(string username, bool? isPrivate)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "SaveVideoProfile",
                                          username, isPrivate);
            }

            if (HttpContext.Current != null)
            {
                string cacheKey = String.Format("VideoProfile_HasVideoProfile_{0}", username);
                if (HttpContext.Current.Cache[cacheKey] != null)
                    HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Deletes the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void Delete(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteVideoProfile",
                                          username);
            }
        }

        /// <summary>
        /// Sets the approved.
        /// </summary>
        /// <param name="username">The username.</param>
        public static void SetApproved(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "ApproveVideoProfile",
                                          username);
            }
        }

        /// <summary>
        /// Determines whether the specified user has video profile.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if the specified user has video profile; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasVideoProfile(string username)
        {
            string cacheKey = String.Format("VideoProfile_HasVideoProfile_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (bool) HttpContext.Current.Cache[cacheKey];
            }

            bool result;
            using (SqlConnection conn = Config.DB.Open())
            {
                result = (bool) SqlHelper.GetDB().ExecuteScalar( "HasVideoProfile", username);
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, result, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromMinutes(15), CacheItemPriority.NotRemovable, null);
            }

            return result;
        }

        /// <summary>
        /// Determines whether the specified username is private.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static bool? IsPrivate(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool?)SqlHelper.GetDB().ExecuteScalar( "IsPrivateVideo",
                                                      username);
            }
        }
        
        /// <summary>
        /// Fetches the non approved.
        /// </summary>
        /// <returns></returns>
        public static string[] FetchNonApproved()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader(
                                            "FetchUsernamesOfNonApprovedVideoProfiles");

                List<string> lUsernames = new List<string>();

                string username;

                while (reader.Read())
                {
                    username = (string) reader["Username"];

                    lUsernames.Add(username);
                }

                return lUsernames.ToArray();
            }
        }
    }
}