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
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class GroupBan
    {
        #region fields

        private int? id;
        private int groupID;
        private string username;
        private DateTime date = DateTime.Now;
        private DateTime expires;

        #endregion

        #region Constructors

        private GroupBan()
        {}

        public GroupBan(int groupID, string username)
        {
            this.groupID = groupID;
            this.username = username;
        }
        
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
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }


        /// <summary>
        /// Gets or sets the expires.
        /// </summary>
        /// <value>The expires.</value>
        public DateTime Expires
        {
            get { return expires; }
            set { expires = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group bans from DB.
        /// If there are no group bans in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupBan[] Fetch()
        {
            return Fetch(null, null, null, null);
        }

        /// <summary>
        /// Fetches group ban by specified id from DB. If the group ban doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupBan Fetch(int id)
        {
            GroupBan[] groupBans = Fetch(id, null, null, null);
            
            if (groupBans.Length > 0)
            {
                return groupBans[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches group bans by specified group ID.
        /// If there are no group bans in DB for the specified group ID it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static GroupBan[] FetchByGroupID(int groupID)
        {
            return Fetch(null, groupID, null, null);
        }

        /// <summary>
        /// Fetches group bans by specified username.
        /// If there are no group bans in DB for the specified username it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupBan[] Fetch(string username)
        {
            return Fetch(null, null, username, null);
        }

        /// <summary>
        /// Fetches group bans by specified group ID and username.
        /// If there are no group bans in DB for the specified arguments it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupBan[] Fetch(int groupID, string username)
        {
            return Fetch(null, groupID, username, null);
        }

        /// <summary>
        /// Fetches group bans by specified arguments.
        /// It returns an empty array if there are no group bans in DB by specified arguments.
        /// If these arguments are null it returns all group bans from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private static GroupBan[] Fetch(int? id, int? groupID, string username, DateTime? date)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchGroupBans",
                                                                        id, groupID, username, date);

                List<GroupBan> lGroupBans = new List<GroupBan>();

                while (reader.Read())
                {
                    GroupBan groupBan = new GroupBan();

                    groupBan.id = (int) reader["ID"];
                    groupBan.groupID = (int) reader["GroupID"];
                    groupBan.username = (string) reader["Username"];
                    groupBan.date = (DateTime) reader["Date"];
                    groupBan.expires = (DateTime) reader["Expires"];

                    lGroupBans.Add(groupBan);
                }

                return lGroupBans.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance in DB.
        /// If id for this instance is NULL it inserts new record in DB otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveGroupBan", id, groupID, username, date, expires);
                
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes group ban by specified ID.
        /// </summary>
        /// <param name="ID">The ID.</param>
        public static void Delete(int ID)
        {
            Delete(ID, null, null);
        }

        /// <summary>
        /// Deletes all group bans by specified group ID and username.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        public static void Delete(int groupID, string username)
        {
            Delete(null, groupID, username);
        }

        /// <summary>
        /// Deletes all group bans from DB by specified parameters.
        /// If all arguments are NULL it deletes all group bans!
        /// </summary>
        /// <param name="ID">The ID.</param>
        private static void Delete(int? ID, int? groupID, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteGroupBan", ID, groupID, username);
            }
        }

        #endregion
    }
}
