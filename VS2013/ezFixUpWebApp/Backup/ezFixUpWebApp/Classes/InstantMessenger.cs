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
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    /// <summary>
    /// Summary description for InstantMessenger.
    /// </summary>
    public static class InstantMessenger
    {
        private static readonly DataTable dtPendingWm;

        static InstantMessenger()
        {
            dtPendingWm = new DataTable();
            dtPendingWm.Columns.Add("originatingUserID", typeof(string));
            dtPendingWm.Columns.Add("destinationUserID", typeof(string));
            dtPendingWm.Columns.Add("openedWindowAt", typeof(DateTime));
            dtPendingWm.Columns.Add("insertedAt", typeof(DateTime));
        }

        /// <summary>
        /// Inserts a request to have a window opened up on the target user's machine
        /// </summary>
        /// <param name="originatingUserID">the originating user</param>
        /// <param name="destinationUserID">the destination user</param>
        public static void CreateOpenWindowRequest(string originatingUserID, string destinationUserID)
        {
            /*
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "imCreateOpenWindowRequest",
                                          originatingUserID, destinationUserID);
            }
            */
            dtPendingWm.Rows.Add(new object[] { originatingUserID, destinationUserID, null, DateTime.Now });
        }

        /// <summary>
        /// Removes open window requests from the database
        /// </summary>
        /// <param name="originatingUserID">the originating user</param>
        /// <param name="destinationUserID">the destination user</param>
        public static void DeleteOpenWindowRequest(string originatingUserID, string destinationUserID)
        {
            /*
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "imDeleteOpenWindowRequest", originatingUserID, destinationUserID);
            }
            */
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in dtPendingWm.Select(String.Format("originatingUserID='{0}' AND destinationUserID='{1}'",
                                             originatingUserID, destinationUserID)))
            {
                rows.Add(row);
            }
            foreach (DataRow row in rows)
            {
                row.Delete();
            }
        }

        /// <summary>
        /// Removes old open window requests from the database
        /// </summary>
        public static void DeleteOldOpenWindowRequests()
        {
            /*
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "imDeleteOldOpenWindowRequests");
            }
            */
            List<DataRow> rows = new List<DataRow>();
            foreach (DataRow row in dtPendingWm.Rows)
            {
                if (DateTime.Now.Subtract(((DateTime)row["insertedAt"])).Minutes >= 15
                    && (row["openedWindowAt"] == DBNull.Value
                    || DateTime.Now.Subtract(((DateTime)row["openedWindowAt"])).Minutes >= 5))
                {
                    rows.Add(row);
                }
            }
            foreach (DataRow row in rows)
            {
                row.Delete();
            }
        }

        /// <summary>
        /// Checks against the db whether a request to open a window already exists
        /// </summary>
        /// <param name="originatingUserID">the originating user</param>
        /// <param name="destinationUserID">the destination user</param>
        /// <returns></returns>
        public static bool OpenWindowRequestExists(string originatingUserID, string destinationUserID)
        {
            /*
            //using (var conn = Config.DB.Open())
            {
                return
                    (bool)
                    SqlHelper.GetDB().ExecuteScalar( "imOpenWindowRequestExists", originatingUserID, destinationUserID);
            }
            */
            DataRow[] rows = dtPendingWm.Select(String.Format("originatingUserID='{0}' AND destinationUserID='{1}'",
                                             originatingUserID, destinationUserID));
            if (rows != null && rows.Length > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Updates the db so we don't open this window again for a few minutes
        /// </summary>
        /// <param name="originatingUserID">the originating user</param>
        /// <param name="destinationUserID">the destination user</param>
        public static void SetWindowOpened(string originatingUserID, string destinationUserID)
        {
            /*
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "imSetWindowOpened", originatingUserID, destinationUserID);
            }
            */
            foreach (DataRow row in dtPendingWm.Select(String.Format("originatingUserID='{0}' AND destinationUserID='{1}'",
                                             originatingUserID, destinationUserID)))
            {
                row["openedWindowAt"] = DateTime.Now;
            }
        }

        /// <summary>
        /// Selects a list of users who want to talk with the current user and we haven't opened a window for 5 mins
        /// </summary>
        /// <param name="currentUserID">The username of the current user</param>
        /// <returns>The list of users who want to talk with the current user</returns>
        public static string[] FetchPendingUsers(string currentUserID)
        {
            /*
            //using (var conn = Config.DB.Open())
            {
                var reader = 
                    SqlHelper.GetDB().ExecuteReader( "imFetchPendingUsers", currentUserID);

                List<string> lResults = new List<string>();

                while (reader.Read())
                {
                    lResults.Add((string)reader["Username"]);
                }

                return lResults.ToArray();
            }
            */
            List<string> lResults = new List<string>();
            foreach (DataRow row in dtPendingWm.Select(String.Format("destinationUserID='{0}'", currentUserID)))
            {
                if (row["openedWindowAt"] == DBNull.Value
                    || DateTime.Now.Subtract(((DateTime)row["openedWindowAt"])).Minutes >= 5)
                {
                    lResults.Add((string)row["originatingUserID"]);
                }
            }
            return lResults.ToArray();
        }
    }
}