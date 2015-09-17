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
    public class GroupPost
    {
        #region fields

        private int? id = null;
        private int groupTopicID;
        private string username;
        private DateTime datePosted = DateTime.Now;
        private DateTime? dateEdited = null;
        private string editNotes = null;
        private string post;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            DatePosted,
            DateEdited,
        }

        #endregion

        #region Constructors

        private GroupPost()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupPost"/> class.
        /// </summary>
        /// <param name="groupTopicID">Represents the group that this post belongs to.</param>
        /// <param name="username">Represents the username that creates this post.</param>
        public GroupPost(int groupTopicID, string username)
        {
            this.groupTopicID = groupTopicID;
            this.username = username;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
        /// Throws "Exceptioin" exception.
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
        public int GroupTopicID
        {
            get { return groupTopicID; }
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
        /// Gets or sets the date posted.
        /// </summary>
        /// <value>The date posted.</value>
        public DateTime DatePosted
        {
            get { return datePosted; }
            set { datePosted = value; }
        }

        /// <summary>
        /// Gets or sets the date edited.
        /// </summary>
        /// <value>The date edited.</value>
        public DateTime? DateEdited
        {
            get { return dateEdited; }
            set { dateEdited = value; }
        }

        /// <summary>
        /// Gets or sets the edit notes.
        /// </summary>
        /// <value>The edit notes.</value>
        public string EditNotes
        {
            get { return editNotes; }
            set { editNotes = value; }
        }

        /// <summary>
        /// Gets or sets the post.
        /// </summary>
        /// <value>The post.</value>
        public string Post
        {
            get { return post; }
            set { post = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group posts from DB.
        /// If there are no group posts in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupPost[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, sortColumn);
        }

        public static GroupPost[] Fetch(string username)
        {
            return Fetch(null, null, username, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches group post by the specified id from DB. If the group post doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupPost Fetch(int id)
        {
            GroupPost[] groupPosts = Fetch(id, null, null, null, null, eSortColumn.None);

            if (groupPosts.Length > 0)
            {
                return groupPosts[0];
            }
            else
            {
                return null;
            }    
        }

        /// <summary>
        /// Fetches group posts by specified parameters.
        /// It returns an empty array if there are no group posts in DB by specified arguments.
        /// If these arguments are null it returns all group posts from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupTopicID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="datePosted">The date posted.</param>
        /// <param name="dateEdited">The date edited.</param>
        /// <returns></returns>
        private static GroupPost[] Fetch(int? id, int? groupTopicID, string username, DateTime? datePosted, DateTime? dateEdited, eSortColumn sortColumn)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var groupPosts = from gp in db.GroupPosts
                                 where (!id.HasValue || id == gp.gp_id)
                                       && (!groupTopicID.HasValue || groupTopicID == gp.gt_id)
                                       && (username == null || username == gp.u_username)
                                       //&& (!datePosted.HasValue || datePosted == gp.gp_dateposted)
                                       //&& (!dateEdited.HasValue || dateEdited == gp.gp_dateedited)
                                 select new GroupPost
                                            {
                                                id = gp.gp_id,
                                                groupTopicID = gp.gt_id,
                                                username = gp.u_username,
                                                dateEdited = gp.gp_dateedited,
                                                datePosted = gp.gp_dateposted,
                                                editNotes = gp.gp_editnotes,
                                                post = gp.gp_post
                                            };

                switch (sortColumn)
                {
                    case eSortColumn.None:
                        break;
                    case eSortColumn.DatePosted:
                        groupPosts = groupPosts.OrderBy(gp => gp.datePosted);
                        break;
                    case eSortColumn.DateEdited:
                        groupPosts = groupPosts.OrderBy(gp => gp.dateEdited);
                        break;
                    default:
                        break;
                }

                return groupPosts.ToArray();
            }

            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchGroupPosts",
            //                                                   id, groupTopicID, username, datePosted, dateEdited,
            //                                                   sortColumn);

            //    List<GroupPost> lGroupPost = new List<GroupPost>();

            //    while (reader.Read())
            //    {
            //        GroupPost groupPost = new GroupPost();

            //        groupPost.id = (int)reader["ID"];
            //        groupPost.groupTopicID = (int)reader["GroupTopicID"];
            //        groupPost.username = (string)reader["Username"];
            //        groupPost.datePosted = (DateTime)reader["DatePosted"];
            //        groupPost.dateEdited = reader["DateEdited"] == DBNull.Value ? null : (DateTime?)reader["DateEdited"];
            //        groupPost.editNotes = reader["EditNotes"] == DBNull.Value ? null : (string)reader["EditNotes"];
            //        groupPost.post = (string)reader["Post"];   
 
            //        lGroupPost.Add(groupPost);
            //    }

            //    return lGroupPost.ToArray();
            //}
        }

        /// <summary>
        /// Searches group posts by specified arguments.
        /// If theare no group posts by specified arguments it returns an empty array.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="datePosted">The date posted.</param>
        /// <param name="dateEdited">The date edited.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static int[] Search(int? groupTopicID, string username, DateTime? datePosted, DateTime? dateEdited, string keyword, eSortColumn sortColumn)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var groupPosts = from gp in db.GroupPosts
                                   where (!groupTopicID.HasValue || groupTopicID == gp.gt_id)
                                         && (username == null || username == gp.u_username)
                                         //&& (!datePosted.HasValue || datePosted == gp.gp_dateposted)
                                         //&& (!dateEdited.HasValue || dateEdited == gp.gp_dateedited)
                                         && (keyword == null || gp.gp_post.Contains(keyword))
                                   select new
                                              {
                                                  id = gp.gp_id,
                                                  datePosted = gp.gp_dateposted,
                                                  dateEdited = gp.gp_dateedited
                                              };
                switch (sortColumn)
                {
                    case eSortColumn.None:
                        break;
                    case eSortColumn.DatePosted:
                        groupPosts = groupPosts.OrderBy(gp => gp.datePosted);
                        break;
                    case eSortColumn.DateEdited:
                        groupPosts = groupPosts.OrderBy(gp => gp.dateEdited);
                        break;
                    default:
                        break;
                }

                return (from p in groupPosts select p.id).ToArray();
            }

            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "SearchGroupPosts",
            //                                                   groupTopicID, username, datePosted,
            //                                                   dateEdited, keyword, sortColumn);

            //    List<int> lGroupPostsIDs = new List<int>();

            //    while (reader.Read())
            //    {
            //        lGroupPostsIDs.Add((int) reader["ID"]);    
            //    }

            //    return lGroupPostsIDs.ToArray();
            //}
        }

        /// <summary>
        /// Saves this instance in DB. If the ID of this instance is NULL it inserts new record in DB
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveGroupPost",
                                                        id, groupTopicID, username, datePosted,
                                                        dateEdited, editNotes, post);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes group post from DB by specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void Delete(int id)
        {
            Delete(id, null, null ,null);
        }

        /// <summary>
        /// Deletes all group posts for specified group topic id.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <returns></returns>
        public static void DeleteByTopic(int groupTopicID)
        {
            Delete(null, null, groupTopicID, null);
        }

        /// <summary>
        /// Deletes all group posts for specified group topic ID and username.
        /// </summary>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static void Delete(int groupTopicID, string username)
        {
            Delete(null, null, groupTopicID, username);
        }

        /// <summary>
        /// Deletes all group posts for specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static void Delete(string username)
        {
            Delete(null, null, null, username);
        }

        /// <summary>
        /// Deletes all group posts for the specified username and group ID.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="groupID">The group ID.</param>
        public static void Delete(string username, int groupID)
        {
            Delete(null, groupID, null, username);
        }

        /// <summary>
        /// Deletes group posts from DB by specified parameters.
        /// Deletes all posts from DB if all arguments are NULL!
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="groupTopicID">The group topic ID.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        private static void Delete(int? id, int? groupID, int? groupTopicID, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteGroupPost", id, groupID, groupTopicID, username);
            }
        }

        public static bool IsDuplicate(int topicID, string username, string post)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToBoolean(SqlHelper.GetDB().ExecuteScalar( "IsDuplicateGroupPost", topicID, username, post));
            }
        }

        #endregion
    }

    [Serializable]
    public class GroupPostSearchResults : SearchResults<int, GroupPost>
    {
        public int[] GroupPosts
        {
            get
            {
                if (Results == null)
                    return new int[0];
                return Results;
            }
            set { Results = value; }
        }

        public new int GetTotalPages(int postsPerPage)
        {
            return base.GetTotalPages(postsPerPage);
        }

        protected override GroupPost LoadResult(int id)
        {
            return GroupPost.Fetch(id);
        }
        protected override GroupPost[] LoadResults(int[] ids)
        {
            throw new NotImplementedException();
        }
        
        
        /// <summary>
        /// Use this method to get the search results.
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <param name="postsPerPage">The topics per page.</param>
        /// <returns></returns>
        public new GroupPost[] GetPage(int Page, int postsPerPage)
        {
            return base.GetPage(Page, postsPerPage);
        }
    }
}
