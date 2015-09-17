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
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    [Serializable]
    public class GroupTopic
    {
        #region fields

        private int? id = null;
        private int groupID;
        private string username;
        private string name;
        private DateTime dateCreated = DateTime.Now;
        private DateTime dateUpdated = DateTime.Now;
        private int posts;
        private bool locked;
        private DateTime? stickyUntil = null;
        private bool isPoll = false;
        private int views = 0;
        private eSortColumn sortColumn;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            DateCreated,
            DateUpdated,
        }

        #endregion

        #region Constructors

        private GroupTopic()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupTopic"/> class.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        public GroupTopic(int groupID, string username)
        {
            this.groupID = groupID;
            this.username = username;
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

        /// <summary>
        /// Gets the group ID.
        /// The property is read-only.
        /// </summary>
        /// <value>The group ID.</value>
        public int GroupID
        {
            get { return groupID; }
            set { groupID = value; }
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
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }

        /// <summary>
        /// Gets or sets the date updated.
        /// </summary>
        /// <value>The date updated.</value>
        public DateTime DateUpdated
        {
            get { return dateUpdated; }
            set { dateUpdated = value; }
        }

        /// <summary>
        /// Gets or sets the posts.
        /// </summary>
        /// <value>The posts.</value>
        public int Posts
        {
            get { return posts; }
            set { posts = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="GroupTopic"/> is locked.
        /// </summary>
        /// <value><c>true</c> if locked; otherwise, <c>false</c>.</value>
        public bool Locked
        {
            get { return locked; }
            set { locked = value; }
        }

        /// <summary>
        /// Gets or sets the sticky until.
        /// </summary>
        /// <value>The sticky until.</value>
        public DateTime? StickyUntil
        {
            get { return stickyUntil; }
            set { stickyUntil = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is poll.
        /// </summary>
        /// <value><c>true</c> if this instance is poll; otherwise, <c>false</c>.</value>
        public bool IsPoll
        {
            get { return isPoll; }
            set { isPoll = value; }
        }

        public int Views
        {
            get { return views; }
            set { views = value; }
        }
        /// <summary>
        /// Gets or sets the sort column.
        /// </summary>
        /// <value>The sort column.</value>
        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group topics from DB.
        /// If there are no group topics in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupTopic[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Fetches group topic by specified id from DB.
        /// If the group topic doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupTopic Fetch(int id)
        {
            GroupTopic[] groupTopic = Fetch(id, null, null, null, null, null, null);

            if (groupTopic.Length > 0)
            {
                return groupTopic[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all group topics from DB by specified group id.
        /// If there are no group topics in DB it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static GroupTopic[] FetchByGroup(int groupID)
        {
            return Fetch(null, groupID, null, null, null, null, null);
        }

        public static GroupTopic[] Fetch(int groupID, string username)
        {
            return Fetch(null, groupID, username, null, null, null, null);
        }

        public static GroupTopic[] Fetch(string username)
        {
            return Fetch(null, null, username, null, null, null, null);
        }

        //public static Group[] FetchNewTopics

        /// <summary>
        /// Fetches the specified number of not sticky topics for the specified group id.
        /// If there are no group topics in DB by specified arguments it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="numberOfTopics">The number of topics.</param>
        /// <returns></returns>
        public static GroupTopic[] FetchActiveTopics(int groupID, int numberOfTopics)
        {
            return Fetch(null, groupID, null, null, null, null, numberOfTopics);
        }

        public static GroupTopic[] FetchUpdatedGroupTopicsUserHasSubscribedTo(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchUpdatedGroupTopicsUserHasSubscribedTo",
                                                               username);

                List<GroupTopic> lGroupTopics = new List<GroupTopic>();

                while (reader.Read())
                {
                    GroupTopic groupTopic = new GroupTopic();

                    groupTopic.id = (int) reader["ID"];
                    groupTopic.groupID = (int) reader["GroupID"];
                    groupTopic.name = (string) reader["Name"];

                    lGroupTopics.Add(groupTopic);
                }

                return lGroupTopics.ToArray();
            }
        }
        
        public static Hashtable FetchNewTopicsCountByGroups(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchNewTopicsCountByGroups",
                                                               username);

                Hashtable hash = new Hashtable();
                
                while (reader.Read())
                {
                    hash.Add(reader["GroupID"], new object[] {reader["Name"], reader["Count"] });
                }

                return hash;
            }           
        }

        /// <summary>
        /// Fetches the new topics.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupTopic[] FetchNewTopics(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchNewTopics",
                                                               username);

                List<GroupTopic> lGroupTopics = new List<GroupTopic>();

                while (reader.Read())
                {
                    lGroupTopics.Add(Fetch((int)reader["GroupTopicID"]));
                }

                return lGroupTopics.ToArray();
            }
        }

        public static int Count(int groupID)
        {
            return Count(groupID, null);
        }

        private static int Count(int groupID, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.GetDB().ExecuteScalar( "FetchGroupTopicsCount", groupID, username));
            }
        }

        /// <summary>
        /// Fetches group topics by specified arguments.
        /// It returns an empty array if there are no group topics in DB by specified arguments.
        /// If these arguments are null it returns all group topics from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <param name="locked">The locked.</param>
        /// <param name="stickyUntil">The sticky until.</param>
        /// <returns></returns>
        private static GroupTopic[] Fetch(int? id, int? groupID, string username, string name,
                                            bool? locked, DateTime? stickyUntil, int? numberOfTopics)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var groupTopics = from gt in db.GroupTopics
                                  where (!id.HasValue || id == gt.gt_id)
                                        && (!groupID.HasValue || groupID == gt.g_id)
                                        && (username == null || username == gt.u_username)
                                        && (name == null || name == gt.gt_name)
                                        && (!locked.HasValue || locked == gt.gt_locked)
                                        && (!stickyUntil.HasValue || gt.gt_stickyuntil <= stickyUntil)
                                        orderby gt.gt_dateupdated descending 
                                        orderby gt.gt_stickyuntil > DateTime.Now descending 
                                  select new GroupTopic
                                             {
                                                 id = gt.gt_id,
                                                 groupID = gt.g_id,
                                                 username = gt.u_username,
                                                 name = gt.gt_name,
                                                 dateCreated = gt.gt_datecreated,
                                                 dateUpdated = gt.gt_dateupdated,
                                                 posts = gt.gt_posts,
                                                 locked = gt.gt_locked,
                                                 isPoll = gt.gt_poll,
                                                 views = gt.gt_views,
                                                 stickyUntil = (gt.gt_stickyuntil.HasValue && gt.gt_stickyuntil < DateTime.Now) ?
                                                                null : gt.gt_stickyuntil
                                             };

                if (numberOfTopics.HasValue)
                    groupTopics = groupTopics.Take(numberOfTopics.Value);

                return groupTopics.ToArray();
                
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
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveGroupTopic",
                                                        id, groupID, username, name, dateCreated,
                                                        dateUpdated, posts, locked, stickyUntil, isPoll, views);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes group topic from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteGroupTopic", id);
            }
        }

        /// <summary>
        /// Searches group topics by spcified arguments.
        /// It returns an empty array if there are no group topics in DB by specified arguments.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="name">The name.</param>
        /// <param name="locked">The locked.</param>
        /// <param name="stickyUntil">The sticky until.</param>
        /// <param name="keyword">The keyword.</param>
        /// <returns></returns>
        public static int[] Search(int? groupID, string username, string name,
                                            bool? locked, bool? sticky, DateTime? stickyUntil, string keyword, bool searchInPosts)
        {
            bool bSticky = sticky ?? false;

            using (var db = new Model.ezFixUpDataContext())
            {
                var groupTopicIds = from gt in db.GroupTopics
                                    where (!groupID.HasValue || groupID == gt.g_id)
                                          && (username == null || username == gt.u_username)
                                          && (name == null || name == gt.gt_name)
                                          && (!locked.HasValue || locked == gt.gt_locked)
                                          && (!sticky.HasValue ||
                                              (bSticky && gt.gt_stickyuntil.HasValue) ||
                                              (!bSticky && !gt.gt_stickyuntil.HasValue))
                                          && (!stickyUntil.HasValue || gt.gt_stickyuntil <= stickyUntil)
                                          && (keyword == null ||
                                              (gt.gt_name.Contains(keyword) ||
                                               (searchInPosts &&
                                                (from gp in db.GroupPosts
                                                 where gp.gp_post.Contains(keyword)
                                                 select gp.gt_id).Contains(gt.gt_id)
                                               )
                                              )
                                             )
                                    orderby gt.gt_dateupdated descending
                                    orderby gt.gt_stickyuntil > DateTime.Now descending
                                    select gt.gt_id;

                return groupTopicIds.ToArray();
            }
        }

        public static bool HasVoted(string username, int topicID)
        {
            if (GroupPollsAnwer.Fetch(topicID, username) != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetIconString(GroupTopic groupTopic)
        {
            string icon = String.Empty;

            if (groupTopic.Locked && groupTopic.StickyUntil == null)
            {
                icon = @"<img src=""images/topiclocked.gif"" />";
            }
            else if (!groupTopic.Locked && groupTopic.StickyUntil != null)
            {
                icon = @"<img src=""images/topicsticky.gif"" />";
            }
            else if (groupTopic.Locked && groupTopic.StickyUntil != null)
            {
                icon = @"<img src=""images/topicboth.gif"" />";
            }
            else if (!groupTopic.Locked && groupTopic.StickyUntil == null && groupTopic.IsPoll)
            {
                icon = @"<img src=""images/topicpoll.gif"" />";
            }
            else if (groupTopic.StickyUntil == null)
            {
                icon = @"<img src=""images/topic.gif"" />";
            }

            return icon;
        }

        #endregion
    }

    [Serializable]
    public class GroupTopicSearchResults : SearchResults<int, GroupTopic>
    {
        public int[] GroupTopics
        {
            get
            {
                if (Results == null)
                    return new int[0];
                else
                    return Results;
            }
            set { Results = value; }
        }

        public new int GetTotalPages(int topicsPerPage)
        {
            return base.GetTotalPages(topicsPerPage);
        }

        protected override GroupTopic LoadResult(int id)
        {
            return GroupTopic.Fetch(id);
        }
        protected override GroupTopic[] LoadResults(int[] ids)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Use this method to get the search results.
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <param name="topicsPerPage">The topics per page.</param>
        /// <returns></returns>
        public new GroupTopic[] GetPage(int Page, int topicsPerPage)
        {
            return base.GetPage(Page, topicsPerPage);
        }

        public GroupTopic[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }
}
