using System;
using System.Collections;
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
    public class GroupPollsAnwer
    {
        #region fields

        private int groupTopicID;
        private string username;
        private int groupPollChoiceID;

        #endregion

        #region Constructors

        private GroupPollsAnwer()
        {
        }

        public GroupPollsAnwer(int groupTopicID, string username, int groupPollChoiceID)
        {
            this.groupTopicID = groupTopicID;
            this.username = username;
            this.groupPollChoiceID = groupPollChoiceID;
        }

        #endregion

        #region Properties

        public int GroupTopicID
        {
            get { return groupTopicID; }
        }

        public string Username
        {
            get { return username; }
        }

        public int GroupPollChoiceID
        {
            get { return groupPollChoiceID; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group poll answers from DB.
        /// If there are no group poll answers in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupPollsAnwer[] Fetch ()
        {
            return Fetch(null, null, null);
        }

        /// <summary>
        /// Fetches group poll answer from DB by the specified group topic id and username.
        /// If the group poll answer doesn't exist returns NULL.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupPollsAnwer Fetch (int groupTopicID, string username)
        {
            GroupPollsAnwer[] groupPollsAnswers = Fetch(groupTopicID, username, null);

            if (groupPollsAnswers.Length > 0)
            {
                return groupPollsAnswers[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all group poll answers from DB by specified group topic id.
        /// If there are no group poll answers in DB it returns an empty array.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <returns></returns>
        public static GroupPollsAnwer[] Fetch(int groupTopicID)
        {
            return Fetch(groupTopicID, null, null);
        }

        /// <summary>
        /// Fetches all group poll answers from DB by specified username
        /// If there are no group poll answers in DB it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupPollsAnwer[] Fetch(string username)
        {
            return Fetch(null, username, null);
        }

        /// <summary>
        /// Fetches all group poll answers from DB by specified group poll choice id
        /// If there are no group poll answers in DB it returns an empty array.
        /// </summary>
        /// <param name="groupPollChoiceID">The group poll choice ID.</param>
        /// <returns></returns>
        public static GroupPollsAnwer[] FetchByGroupPollChoiceID(int groupPollChoiceID)
        {
            return Fetch(null, null, groupPollChoiceID);
        }

        /// <summary>
        /// Gets the number of group polls answers for the specified group topic id.
        /// It returns a hash table which contains the group polls choice id as a key and
        /// the number of answers as a value.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <returns></returns>
        public static Hashtable GetNumberOfAnswers(int groupTopicID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchGroupPollsAnswersNumber", groupTopicID);
                
                Hashtable results = new Hashtable();

                while (reader.Read())
                {
                    results.Add((int) reader["GroupPollsChoiceID"], (int) reader["NumberOfAnswers"]);
                }

                return results;
            }
        }

        /// <summary>
        /// Fetches group poll answers by specified parameters.
        /// It returns an empty array if there are no group poll answers in DB by specified arguments.
        /// If these arguments are null it returns all group poll answers from DB.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="groupPollChoiceID">The group poll choice ID.</param>
        /// <returns></returns>
        private static GroupPollsAnwer[] Fetch(int? groupTopicID, string username, int? groupPollChoiceID)
        {
            using(SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader( "FetchGroupPollsAnswers", groupTopicID, username, groupPollChoiceID);

                List <GroupPollsAnwer> lGroupPollsAnswer = new List<GroupPollsAnwer>();

                while (reader.Read())
                {
                    GroupPollsAnwer groupPollsAnswer = new GroupPollsAnwer();

                    groupPollsAnswer.groupTopicID = (int) reader["GroupTopicID"];
                    groupPollsAnswer.username = (string) reader["Username"];
                    groupPollsAnswer.groupPollChoiceID = (int) reader["GroupPollChoiceID"];

                    lGroupPollsAnswer.Add(groupPollsAnswer);
                }

                return lGroupPollsAnswer.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance into DB.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteScalar( "SaveGroupPollsAnswer", groupTopicID, username, groupPollChoiceID);
            }
        }

        /// <summary>
        /// Deletes group poll answer from DB by specified group topic id and username.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="username">The username.</param>
        public static void Delete(int groupTopicID, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteGroupPollsAnswer", groupTopicID, username);
            }
        }

        #endregion
    }
}
