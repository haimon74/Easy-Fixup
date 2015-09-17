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
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class GroupTopicSubscription
    {
        #region fields

        private int? id = null;
        private string username;
        private int groupTopicID;
        private int groupID;
        private DateTime dateUpdated = DateTime.Now;

        #endregion

        #region Constructors

        private GroupTopicSubscription()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupTopicSubscription"/> class.
        /// </summary>
        /// <param name="username">The username of the subscriber.</param>
        /// <param name="groupTopicID">The ID of the topic.</param>
        /// <param name="groupID">The group ID which topic belongs to.</param>
        public GroupTopicSubscription(string username, int groupTopicID, int groupID)
        {
            this.username = username;
            this.groupTopicID = groupTopicID;
            this.groupID = groupID;
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

        public string Username
        {
            get { return username; }
        }

        public int GroupTopicID
        {
            get { return groupTopicID; }
        }

        public int GroupID
        {
            get { return groupID; }
        }

        public DateTime DateUpdated
        {
            get { return dateUpdated; }
            set { dateUpdated = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group topic subscriptions from DB.
        /// If there are no group topic subscriptions in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupTopicSubscription[] Fetch()
        {
            return Fetch(null, null, null, null, null);
        }

        /// <summary>
        /// Fetches group topic subscription by specified id from DB.
        /// If the group topic subscription doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupTopicSubscription Fetch(int id)
        {
            GroupTopicSubscription[] groupTopicSubscriptions = Fetch(id, null, null, null, null);

            if (groupTopicSubscriptions.Length > 0)
            {
                return groupTopicSubscriptions[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches group topic subscription by specified arguments from DB.
        /// If the group topic subscription doesn't exist returns NULL.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static GroupTopicSubscription Fetch(string username, int groupTopicID, int groupID)
        {
            GroupTopicSubscription[] groupTopicSubscriptions = Fetch(null, username, groupTopicID, groupID, null);

            if (groupTopicSubscriptions.Length > 0)
            {
                return groupTopicSubscriptions[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all group topic subscriptions by specified username.
        /// If there are no group topic subscriptions in DB for the specified username
        /// it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupTopicSubscription[] Fetch(string username)
        {
            return Fetch(null, username, null, null, null);
        }

        public static GroupTopicSubscription Fetch(string username, int groupTopicID)
        {
            GroupTopicSubscription[] groupTopicSubscriptions = Fetch(null, username, groupTopicID, null, null);

            if (groupTopicSubscriptions.Length > 0)
            {
                return groupTopicSubscriptions[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches group topic subscription by specified group topic ID.
        /// If there are no group topic subscription in DB for the specified group topic ID
        /// it returns NULL.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <returns></returns>
        public static GroupTopicSubscription FetchByGroupTopicID(int groupTopicID)
        {
            GroupTopicSubscription[] groupTopicSubscriptions = Fetch(null, null, groupTopicID, null, null);

            if (groupTopicSubscriptions.Length > 0)
            {
                return groupTopicSubscriptions[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all group topic subscriptions by specified group ID.
        /// If there are no group topic subscriptions in DB for the specified group ID
        /// it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static GroupTopicSubscription[] FetchByGroupID(int groupID)
        {
            return Fetch(null, null, null, groupID, null);
        }

        /// <summary>
        /// Fetches group topic subscriptions by specified arguments.
        /// It returns an empty array if there are no group topic subscriptions in DB by specified arguments.
        /// If these arguments are null it returns all group topic subscriptions from DB.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="dateUpdated">The date updated.</param>
        /// <returns></returns>
        public static GroupTopicSubscription[] Fetch(int? id, string username, int? groupTopicID, int? groupID, DateTime? dateUpdated)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchGroupTopicSubscriptions",
                                                               id, username, groupTopicID, groupID, dateUpdated);

                List<GroupTopicSubscription> lGroupTopicSubscriptions = new List<GroupTopicSubscription>();
                
                while (reader.Read())
                {
                    GroupTopicSubscription groupTopicSubscription = new GroupTopicSubscription();
                    
                    groupTopicSubscription.id = (int) reader["ID"];
                    groupTopicSubscription.username = (string) reader["Username"];
                    groupTopicSubscription.groupTopicID = (int) reader["GroupTopicID"];
                    groupTopicSubscription.groupID = (int) reader["GroupID"];
                    groupTopicSubscription.dateUpdated = (DateTime) reader["DateUpdated"];

                    lGroupTopicSubscriptions.Add(groupTopicSubscription);
                }

                return lGroupTopicSubscriptions.ToArray();
            }
        }

        public static bool IsSubscribed(string username, int groupTopicID)
        {
            if (Fetch(username, groupTopicID) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Saves this instance in DB. If the ID of this instance is NULL it inserts new record in DB
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveGroupTopicSubscription",
                                        id, username, groupTopicID, groupID, dateUpdated);

                if (!id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes group topic subscription from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteGroupTopicSubscription", id);
            }
        }

        #endregion
    }
}
