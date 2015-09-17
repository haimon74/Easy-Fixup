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
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    /// <summary>
    /// Represents blog. Provides methods to create, retrieve and update.
    /// </summary>
    public class Blog
    {
        #region Fields

        private int id;
        private string username;
        private string name;
        private string description;
        private DateTime dateCreated;
        private string settings;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the id of the blog.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        /// <summary>
        /// Gets or sets the username of the blog.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Gets or sets the name of the blog.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the description of the blog.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets creation date of the blog.
        /// </summary>
        /// <value>The date created.</value>
        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }

        #endregion

        private Blog()
        {
        }

        /// <summary>
        /// Creates blog for the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static Blog Create(string username)
        {
            Blog blog = new Blog();
            blog.id = -1;
            blog.username = username;
            return blog;
        }

        /// <summary>
        /// Saves blog in DB.
        /// </summary>
        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveBlog", id, username, name, description,
                                                        settings);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
                else if (HttpContext.Current != null)
                {
                    string cacheKey = String.Format("Blog_Load_{0}_{1}", id, null);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                        HttpContext.Current.Cache.Remove(cacheKey);
                    cacheKey = String.Format("Blog_Load_{0}_{1}", -1, username);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                        HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        /// <summary>
        /// Loads blog from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static Blog Load(int id)
        {
            return Load(id, null);
        }

        /// <summary>
        /// Loads blog from DB by specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static Blog Load(string username)
        {
            return Load(-1, username);
        }

        /// <summary>
        /// Loads blog from DB by specified id or username.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="username">The username.</param>
        /// <returns>Blog object.</returns>
        private static Blog Load(int id, string username)
        {
            string cacheKey = String.Format("Blog_Load_{0}_{1}", id, username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Blog;
            }

            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("LoadBlog", id, username))
                {

                    if (reader.Read())
                    {
                        Blog blog = new Blog();

                        blog.id = (int) reader["Id"];
                        blog.username = (string) reader["Username"];
                        blog.name = (string) reader["Name"];
                        blog.description = (string) reader["Description"];
                        blog.dateCreated = (DateTime) reader["DateCreated"];
                        if (reader["Settings"] is string)
                            blog.settings = (string) reader["Settings"];

                        reader.Close();

                        if (HttpContext.Current != null)
                        {
                            HttpContext.Current.Cache.Insert(cacheKey, blog, null, Cache.NoAbsoluteExpiration,
                                                             TimeSpan.FromMinutes(30), CacheItemPriority.NotRemovable,
                                                             null);
                        }

                        return blog;
                    }
                    else
                        return null;
                }
            }
        }

        /// <summary>
        /// Determines whether this blog has posts.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has posts; otherwise, <c>false</c>.
        /// </returns>
        public bool HasPosts()
        {
            return HasPosts(username);
        }

        /// <summary>
        /// Determines whether the blog for the specified username has posts.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if the specified username has posts; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasPosts(string username)
        {
            string cacheKey = String.Format("Blog_HasPosts_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return (bool) HttpContext.Current.Cache[cacheKey];
            }

            bool hasBlogPosts = false;
            Blog blog = Load(username);
            if (blog != null)
            {
                BlogPost[] blogPosts = BlogPost.Fetch(blog.Id, true);
                if (blogPosts != null && blogPosts.Length > 0) hasBlogPosts = true;
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, hasBlogPosts, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromHours(1), CacheItemPriority.NotRemovable, null);
            }

            return hasBlogPosts;
        }
    }

    public class BlogPost
    {
        #region Fields

        private int id;
        private int blogId;
        private string title;
        private string content;
        private DateTime datePosted;
        private int reads;
        private bool approved = !Config.Misc.EnableBlogPostApproval;

        public enum eSortColumn
        {
            None,
            DatePosted,
            Title,
            Reads
        }

        #endregion

        #region Properties

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int BlogId
        {
            get { return blogId; }
            set { blogId = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public DateTime DatePosted
        {
            get { return datePosted; }
            set { datePosted = value; }
        }

        public int Reads
        {
            get { return reads; }
            set { reads = value; }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        #endregion

        private BlogPost()
        {
        }

        public static BlogPost Create(int blogId, string title, string content)
        {
            BlogPost blogPost = new BlogPost();
            blogPost.id = -1;
            blogPost.blogId = blogId;
            blogPost.title = title;
            blogPost.content = content;
            blogPost.datePosted = DateTime.Now;
            blogPost.reads = 0;
            return blogPost;
        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveBlogPost", id, blogId, title, content, approved);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);

                    if (HttpContext.Current != null)
                    {
                        Blog blog = Blog.Load(blogId);
                        string cacheKey = String.Format("Blog_HasPosts_{0}", blog.Username);
                        if (HttpContext.Current.Cache[cacheKey] != null)
                            HttpContext.Current.Cache.Remove(cacheKey);
                    }
                }
            }
        }

        public static BlogPost Load(int blogPostId)
        {
            BlogPost[] blogPosts = Fetch(-1, blogPostId, null);
            if (blogPosts.Length >= 1)
                return blogPosts[0];
            else
                throw new NotFoundException("The requested blog post does not exist!");
        }

        public static BlogPost[] Fetch(int blogId)
        {
            return Fetch(blogId, -1, null);
        }

        public static BlogPost[] Fetch(int blogId, bool approved)
        {
            return Fetch(blogId, -1, approved);
        }

        private static BlogPost[] Fetch(int blogId, int blogPostId, bool? approved)
        {
            List<BlogPost> lBlogPosts = new List<BlogPost>();

            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchBlogPosts", blogId, blogPostId, approved))
                {

                    while (reader.Read())
                    {
                        BlogPost blogPost = new BlogPost();

                        blogPost.id = (int) reader["Id"];
                        blogPost.blogId = (int) reader["BlogId"];
                        blogPost.title = (string) reader["Title"];
                        blogPost.content = (string) reader["Content"];
                        blogPost.datePosted = (DateTime) reader["DatePosted"];
                        blogPost.reads = (int) reader["Reads"];
                        blogPost.approved = (bool) reader["Approved"];

                        lBlogPosts.Add(blogPost);
                    }
                    reader.Close();
                }
            }

            return lBlogPosts.ToArray();
        }

        public static int[] Search(int? blogID, string username, string title, string content, bool? approved, DateTime? datePosted,
                                    string keyword, bool searchInContent, int? numberOfBlogPosts, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                List<int> lBlogPostsIDs = new List<int>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("SearchBlogPosts",
                                              blogID, username, title, content, approved,datePosted,
                                              keyword, searchInContent, numberOfBlogPosts,sortColumn))
                {
                    while (reader.Read())
                    {
                        lBlogPostsIDs.Add((int) reader["ID"]);
                    }
                    reader.Close();
                }
                return lBlogPostsIDs.ToArray();
            }
        }

        public static void Delete(int blogPostId)
        {
            //using (var conn = Config.DB.Open())
            {
                BlogPost blogPost = Load(blogPostId);
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteBlogPost", blogPostId);

                if (HttpContext.Current != null)
                {
                    Blog blog = Blog.Load(blogPost.BlogId);
                    string cacheKey = String.Format("Blog_HasPosts_{0}", blog.Username);
                    if (HttpContext.Current.Cache[cacheKey] != null)
                        HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        public static void IncreaseReadCounter(int blogPostId)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "IncreaseBlogPostReadCounter", blogPostId);
            }
        }
    }

    public class BlogPostComment
    {
        #region Fields

        private int id;
        private int blogPostId;
        private string username;
        private string commentText;
        private DateTime datePosted;
        private bool approved;

        #endregion

        #region Properties

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int BlogPostId
        {
            get { return blogPostId; }
            set { blogPostId = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public string CommentText
        {
            get { return commentText; }
            set { commentText = value; }
        }

        public DateTime DatePosted
        {
            get { return datePosted; }
            set { datePosted = value; }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        #endregion

        private BlogPostComment()
        {
        }

        public static BlogPostComment Create(int blogPostId, string username, string commentText)
        {
            BlogPostComment blogPostComment = new BlogPostComment();
            blogPostComment.id = -1;
            blogPostComment.blogPostId = blogPostId;
            blogPostComment.username = username;
            blogPostComment.commentText = commentText;
            blogPostComment.datePosted = DateTime.Now;
            blogPostComment.approved = true;
            return blogPostComment;
        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar(
                                                        "SaveBlogPostComment", id, blogPostId, username, commentText,
                                                        approved);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static BlogPostComment Load(int id)
        {
            BlogPostComment[] blogPostComments = Fetch(id, -1, null, false, true);
            if (blogPostComments.Length >= 1)
                return blogPostComments[0];
            else
                throw new NotFoundException("The requested comment does not exist!");
        }

        public static BlogPostComment[] Fetch(int blogPostId)
        {
            return Fetch(-1, blogPostId, null, true, true);
        }

        public static BlogPostComment[] Fetch(int blogPostID, string username)
        {
            return Fetch(-1, blogPostID, username, true, true);
        }

        public static BlogPostComment[] Fetch(int id, int blogPostId, string username, bool approveFilter,
                                              bool approved)
        {
            List<BlogPostComment> lComments = new List<BlogPostComment>();

            //using (var conn = Config.DB.Open())
            {
                //HACK approveFilter = false
                approveFilter = false;

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchBlogPostComments",
                                              id, blogPostId, username,approveFilter ? (object) approved : null))
                {
                    while (reader.Read())
                    {
                        BlogPostComment blogPostComment = new BlogPostComment();

                        blogPostComment.id = (int) reader["Id"];
                        blogPostComment.blogPostId = (int) reader["BlogPostId"];
                        blogPostComment.username = (string) reader["Username"];
                        blogPostComment.commentText = (string) reader["CommentText"];
                        blogPostComment.datePosted = (DateTime) reader["DatePosted"];
                        blogPostComment.approved = (bool) reader["Approved"];

                        lComments.Add(blogPostComment);
                    }
                    reader.Close();
                }
            }

            return lComments.ToArray();
        }

        public static void Delete(int id)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteBlogPostComment", id);
            }
        }
    }
}