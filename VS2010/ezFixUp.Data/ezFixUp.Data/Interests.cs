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
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class Interest
    {
        #region Properties

        private int id;

        public int Id
        {
            get { return id; }
        }

        private string fromUsername;

        public string FromUsername
        {
            get { return fromUsername; }
        }

        private string toUsername;

        public string ToUsername
        {
            get { return toUsername; }
        }

        private DateTime datePosted;

        public DateTime DatePosted
        {
            get { return datePosted; }
        }

        private bool deletedByFromUser;

        public bool DeletedByFromUser
        {
            get { return deletedByFromUser; }
        }

        private bool deletedByToUser;

        public bool DeletedByToUser
        {
            get { return deletedByToUser; }
        }

        #endregion

        public enum eFolder
        {
            None = 0,
            Received = 1,
            Sent = 2
        }

        private enum eStatus
        {
            AlreadySent = 0,
            Sent = 1,
            Updated = 2
        }

        public Interest()
        {
        }

        /// <summary>
        /// Sends the specified from username.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <returns></returns>
//        public static bool Send(string fromUsername, string toUsername)
//        {
//            eStatus status;
//            using (SqlConnection conn = Config.DB.Open())
//            {
//                status = (eStatus) SqlHelper.GetDB().ExecuteScalar(
//                                                           "SendInterest", fromUsername, toUsername);
//            }
//
//            switch (status)
//            {
//                case eStatus.Sent:
//                    User targetUser;
//                    try
//                    {
//                         targetUser = User.Load(toUsername);
//                     }
//                     catch (NotFoundException) { throw; }
//                    
//                    if (targetUser.ReceiveEmails)
//                    {
//                        MiscTemplates.ShowInterestMessage showInterestMessageTemplate =
//                            new MiscTemplates.ShowInterestMessage(targetUser.LanguageId);
//                        string subject = showInterestMessageTemplate.Subject;
//                        string body = showInterestMessageTemplate.Message.Replace("%%USER%%", fromUsername);
//                        try
//                        {
//                            Email.Send(Config.Misc.SiteEmail, targetUser.Email, subject, body, false);
//                            //Message.Send(Config.Users.SystemUsername,toUsername,body,0);
//                        }
//                        catch (Exception ex)
//                        {
//                            ExceptionLogger.Log("Interests.Send", ex);
//                        }
//                    }
//                    return true;
//                case eStatus.Updated:
//                    return true;
//                case eStatus.AlreadySent:
//                default:
//                    return false;
//            }
//        }

        /// <summary>
        /// Fetches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Interest Fetch(int id)
        {
            Interest[] interests = Fetch(id, null, null, false);
            if (interests.Length > 0)
            {
                return interests[0];
            }
            else
            {
                throw new NotFoundException
                    (Lang.Trans("The requested interest does not exist!"));
            }
        }

        /// <summary>
        /// Fetches the specified from username.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <returns></returns>
        public static Interest Fetch(string fromUsername, string toUsername)
        {
            Interest[] interests = Fetch(0, fromUsername, toUsername, false);
            if (interests.Length > 0)
            {
                return interests[0];
            }
            else
            {
                throw new NotFoundException
                    (Lang.Trans("The requested interest does not exist!"));
            }
        }

        public static Interest[] FetchReceived(string username, bool newInterests)
        {
            return Fetch(0, null, username, newInterests);
        }

        public static Interest[] FetchSent(string username)
        {
            return Fetch(0, username, null, false);
        }

        public static Interest[] Fetch(int id, string fromUsername, string toUsername, bool newInterests)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader( "FetchInterests", id, fromUsername, toUsername, newInterests);

                List<Interest> lInterests = new List<Interest>();

                while (reader.Read())
                {
                    Interest interest = new Interest();

                    interest.id = (int) reader["Id"];
                    interest.fromUsername = (string) reader["FromUsername"];
                    interest.toUsername = (string) reader["ToUsername"];
                    interest.datePosted = (DateTime) reader["DatePosted"];
                    interest.deletedByFromUser = (bool) reader["DeletedByFromUser"];
                    interest.deletedByToUser = (bool) reader["DeletedByToUser"];

                    lInterests.Add(interest);
                }

                return lInterests.ToArray();
            }
        }

        private void DeleteBy(bool fromUser /*sent interest*/,
                              bool toUser /*received interest*/)
        {
            if (id > 0)
            {
                using (SqlConnection conn = Config.DB.Open())
                {
                    SqlHelper.GetDB().ExecuteNonQuery(
                                              "DeleteInterest", id, fromUser, toUser);
                }
            }
        }

        public void DeleteFromReceivedInterests()
        {
            DeleteBy(false, true);
        }

        public void DeleteFromSentInterests()
        {
            DeleteBy(true, false);
        }
    }
}