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
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Data
{
    /// <summary>
    /// Summary description for ContentPages.
    /// </summary>
    public class ContentPage
    {
        #region fields

        #region eSortColumn enum

        /// <summary>
        /// The sort column
        /// </summary>
        public enum eSortColumn
        {
            /// <summary>
            /// Do not sort
            /// </summary>
            None,
            /// <summary>
            /// Sort by header position
            /// </summary>
            HeaderPosition,
            /// <summary>
            /// Sort by footer position
            /// </summary>
            FooterPosition
        }

        #endregion

        #region eVisibility enum

        /// <summary>
        /// Page visibility
        /// </summary>
        [Flags]
        public enum eVisibility
        {
            /// <summary>
            /// Visible only for logged in users
            /// </summary>
            LoggedOnUsers = 1,
            /// <summary>
            /// Visible only for guests (non logged in)
            /// </summary>
            NotLoggedOnUsers = 2,
            /// <summary>
            /// Visible for all
            /// </summary>
            All = 3,

            Paid = 4,
            Unpaid = 8
        }

        #endregion

        private string content = "";
        private int? footerPosition;
        private int? headerPosition;
        private int id = -1;
        private int languageID;

        private string metaDescription = String.Empty;
        private string metaKeyword = String.Empty;

        private eSortColumn sortColumn;
        private string title = "";
        private string url = String.Empty;
        private string urlRewrite;
        private eVisibility visibleFor = eVisibility.All;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Gets or sets the content.
        /// </summary>
        /// <value>The content.</value>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        /// <summary>
        /// Gets or sets the header position.
        /// </summary>
        /// <value>The header position.</value>
        public int? HeaderPosition
        {
            get { return headerPosition; }
            set { headerPosition = value; }
        }

        /// <summary>
        /// Gets or sets the footer position.
        /// </summary>
        /// <value>The footer position.</value>
        public int? FooterPosition
        {
            get { return footerPosition; }
            set { footerPosition = value; }
        }

        /// <summary>
        /// Gets or sets the visible for.
        /// </summary>
        /// <value>The visible for.</value>
        public eVisibility VisibleFor
        {
            get { return visibleFor; }
            set { visibleFor = value; }
        }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string URL
        {
            get { return url; }
            set { url = value; }
        }

        /// <summary>
        /// Gets or sets the meta description.
        /// </summary>
        /// <value>The meta description.</value>
        public string MetaDescription
        {
            get { return metaDescription; }
            set { metaDescription = value; }
        }

        /// <summary>
        /// Gets or sets the meta keyword.
        /// </summary>
        /// <value>The meta keyword.</value>
        public string MetaKeyword
        {
            get { return metaKeyword; }
            set { metaKeyword = value; }
        }

        /// <summary>
        /// Gets or sets the language ID.
        /// </summary>
        /// <value>The language ID.</value>
        public int LanguageID
        {
            get { return languageID; }
            set { languageID = value; }
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

        /// <summary>
        /// Gets or sets the URL rewrite.
        /// </summary>
        /// <value>The URL rewrite.</value>
        public string UrlRewrite
        {
            get { return urlRewrite; }
            set { urlRewrite = value; }
        }

        #endregion

        private ContentPage()
        {
        }

        /// <summary>
        /// Creates the specified title.
        /// </summary>
        /// <param name="title">The title.</param>
        /// <param name="content">The content.</param>
        /// <param name="headerPosition">The header position.</param>
        /// <param name="footerPosition">The footer position.</param>
        /// <param name="visibleFor">The visible for.</param>
        /// <param name="url">The URL.</param>
        /// <param name="metaDescription">The meta description.</param>
        /// <param name="metaKeyword">The meta keyword.</param>
        /// <param name="languageID">The language ID.</param>
        /// <param name="urlRewrite">The URL rewrite.</param>
        /// <returns></returns>
        public static ContentPage Create(string title, string content, int? headerPosition,
                                         int? footerPosition, eVisibility visibleFor,
                                         string url, string metaDescription, string metaKeyword, int languageID,
                                         string urlRewrite)
        {
            var cp = new ContentPage
                         {
                             title = title,
                             content = content,
                             headerPosition = headerPosition,
                             footerPosition = footerPosition,
                             visibleFor = visibleFor,
                             url = url,
                             metaDescription = metaDescription,
                             metaKeyword = metaKeyword,
                             languageID = languageID,
                             urlRewrite = urlRewrite
                         };

            return cp;
        }

        /// <summary>
        /// Fetches content page with the specified id.
        /// </summary>
        /// <param name="id">Represents page id</param>
        /// <returns>Content page with the specified id</returns>
        public static ContentPage FetchContentPage(int id)
        {
            ContentPage[] cp = FetchContentPages(id, null, eSortColumn.None);

            if (cp.Length > 0)
                return cp[0];
            else
                return null;
        }

        /// <summary>
        /// Fetches all content pages with the specified language from database
        /// and sorts them by specified column.
        /// </summary>
        /// <returns>Array of content pages.</returns>
        public static ContentPage[] FetchContentPages(int languageID, eSortColumn sortColumn)
        {
            ContentPage[] pages = FetchContentPages(null, languageID, sortColumn);
            return pages;
        }

        /// <summary>
        /// Fetches all content pages from database by specified id or language.
        /// </summary>
        /// <param name="id">Represents content page id.</param>
        /// <param name="languageID">The language ID.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns>Array of content pages.</returns>
        public static ContentPage[] FetchContentPages(object id, object languageID, eSortColumn sortColumn)
        {
            string cacheKey = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", id, languageID, sortColumn);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ContentPage[];
            }

            var lPages = new List<ContentPage>(5);

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader(
                                            "FetchContentPages", id, languageID, sortColumn);

                while (reader.Read())
                {
                    var contentPage = new ContentPage();
                    contentPage.id = (int) reader["ID"];
                    contentPage.title = (string) reader["Title"];
                    contentPage.content = (string) reader["Content"];
                    contentPage.headerPosition = reader["HeaderPosition"] == DBNull.Value
                                                     ? null
                                                     : (int?) reader["HeaderPosition"];
                    contentPage.footerPosition = reader["FooterPosition"] == DBNull.Value
                                                     ? null
                                                     : (int?) reader["FooterPosition"];
                    contentPage.visibleFor = (eVisibility) (int) reader["VisibleFor"];
                    contentPage.url = reader["URL"] == DBNull.Value ? null : (string) reader["URL"];
                    contentPage.metaDescription = (string) reader["MetaDescription"];
                    contentPage.metaKeyword = (string) reader["MetaKeyword"];
                    contentPage.languageID = (int) reader["LanguageID"];
                    contentPage.urlRewrite = reader["UrlRewrite"] as string;

                    lPages.Add(contentPage);
                }
            }

            ContentPage[] pages = lPages.ToArray();
            if (HttpContext.Current != null)
            {
                //Global.AddCacheItem("ContentPage", cacheKey, pages);
                HttpContext.Current.Cache.Insert(cacheKey, pages, null, DateTime.Now.AddMinutes(30),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return pages;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.GetDB().ExecuteScalar( "SaveContentPage",
                                            id, title, content, headerPosition,
                                            footerPosition, visibleFor, url,
                                            metaDescription, metaKeyword, languageID,
                                            urlRewrite);

                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }

            string cacheKey = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", id, null, eSortColumn.None);
            string cacheKey2 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, languageID,
                                             eSortColumn.None);
            string cacheKey3 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, languageID,
                                             eSortColumn.HeaderPosition);
            string cacheKey4 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, languageID,
                                             eSortColumn.FooterPosition);
            string cacheKey5 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, null, sortColumn);

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey2] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey2);
            }

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey3] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey3);
            }

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey4] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey4);
            }

            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey5] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey5);
            }
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            try
            {
                ContentPage page = FetchContentPage(id);

                string cacheKey = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", id, null, eSortColumn.None);
                string cacheKey2 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, page.LanguageID,
                                                 eSortColumn.None);
                string cacheKey3 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, page.LanguageID,
                                                 eSortColumn.HeaderPosition);
                string cacheKey4 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, page.LanguageID,
                                                 eSortColumn.FooterPosition);
                string cacheKey5 = String.Format("ContentPage_FetchContentPages_{0}_{1}_{2}", null, null,
                                                 eSortColumn.None);

                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }

                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey2] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey2);
                }

                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey3] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey3);
                }

                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey4] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey4);
                }

                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey5] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey5);
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogError(err);
            }

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteContentPage", id);
            }
        }
    }
}