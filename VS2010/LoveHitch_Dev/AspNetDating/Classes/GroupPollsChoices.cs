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
    public class GroupPollsChoice
    {
        #region fields

        private int? id = null;
        private int groupTopicID;
        private string answer;
 
        #endregion

        #region Constructors

        private GroupPollsChoice()
        {
        }

        public GroupPollsChoice(int groupTopicID)
        {
            this.groupTopicID = groupTopicID;
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

        public int GroupTopicID
        {
            get { return groupTopicID; }
        }

        public string Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches group poll choice by the specified id from DB.
        /// If the group poll choice doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupPollsChoice Fetch(int id)
        {
            GroupPollsChoice[] groupPollsChoices = Fetch(id, null);

            if (groupPollsChoices.Length > 0)
            {
                return groupPollsChoices[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all group poll choices from DB by specified group topic id.
        /// If there are no group poll choices in DB it returns an empty array.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <returns></returns>
        public static GroupPollsChoice[] FetchByGroupTopic(int groupTopicID)
        {
            return Fetch(null, groupTopicID);
        }

        /// <summary>
        /// Fetches all group poll choices from DB.
        /// If there are no group poll choices in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupPollsChoice[] Fetch()
        {
            return Fetch(null, null);
        }

        /// <summary>
        /// Fetches group poll choices by specified parameters.
        /// It returns an empty array if there are no group poll choices in DB by specified arguments.
        /// If these arguments are null it returns all group poll choices from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <returns></returns>
        private static GroupPollsChoice[] Fetch(int? id, int? groupTopicID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchGroupPollsChoices",
                                                               id, groupTopicID);

                List<GroupPollsChoice> lGroupPollsChoice = new List<GroupPollsChoice>();

                while (reader.Read())
                {
                    GroupPollsChoice groupPollChoice = new GroupPollsChoice();

                    groupPollChoice.id = (int)reader["ID"];
                    groupPollChoice.groupTopicID = (int)reader["GroupTopicID"];
                    groupPollChoice.answer = (string)reader["Answer"];

                    lGroupPollsChoice.Add(groupPollChoice);
                }

                return lGroupPollsChoice.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance into DB.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveGroupPollsChoice",
                                                        id, groupTopicID, answer);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes group poll choice from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteGroupPollsChoice", id);
            }
        }

        #endregion
    }
}
