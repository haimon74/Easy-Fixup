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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace ezFixUp.Classes
{
    /// <summary>
    /// Summary description for News.
    /// </summary>
    public class News
    {
        #region Properties

        private int id;

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int ID
        {
            get { return id; }
        }

        private string text;

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        private DateTime publishDate;

        /// <summary>
        /// Gets or sets the publish date.
        /// </summary>
        /// <value>The publish date.</value>
        public DateTime PublishDate
        {
            get { return publishDate; }
            set { publishDate = value; }
        }

        private string title;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private int languageID;

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>The language id.</value>
        public int LanguageId
        {
            get { return languageID; }
            set { languageID = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="News"/> class.
        /// </summary>
        public News()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="News"/> class.
        /// </summary>
        /// <param name="newsID">The news ID.</param>
        /// <param name="languageID">The language ID.</param>
        public News(int newsID, int languageID)
        {
            id = newsID;
            this.languageID = languageID;
        }

        /// <summary>
        /// Fetches all news with specified language from database.
        /// </summary>
        /// <returns>DataTabale with all news.</returns>
        public static DataTable FetchAsTable(int languageID)
        {
            return FetchAsTable(Int32.MaxValue, languageID);
        }

        /// <summary>
        /// Fetches specified number of news with specified language.
        /// </summary>
        /// <param name="newsCount">Represents the number of news for fetching.</param>
        /// <param name="languageID">The language ID.</param>
        /// <returns>News as DataTable.</returns>
        public static DataTable FetchAsTable(int newsCount, int languageID)
        {
            DataSet dsNews =
                SqlHelper.GetDB().ExecuteDataSet( "FetchNews", -1, newsCount, languageID);
            return dsNews.Tables[0];
        }

        /// <summary>
        /// Fetches all news with specified language from database.
        /// </summary>
        /// <returns>All news from database as News[].</returns>
        public static News[] FetchAsArray(int languageID)
        {
            return FetchNews(-1, Int32.MaxValue, languageID);
        }

        /// <summary>
        /// Fetches a specified news with specified language.
        /// </summary>
        /// <param name="id">Represents news id.</param>
        /// <param name="languageID">The language ID.</param>
        /// <returns>A news with specified id.</returns>
        public static News Fetch(int id, int languageID)
        {
            News[] news = FetchNews(id, 1, languageID);

            if (news.Length > 0)
                return news[0];
            else
                return null;
        }

        /// <summary>
        /// Fetches specified number of news with specified id and language. For Example:
        /// when id is -1 and newsCount is Int32.MaxValue it fetches all news.
        /// </summary>
        /// <param name="id">Represents news id.</param>
        /// <param name="newsCount">Represents number of news to fetch.</param>
        /// <param name="languageID">The language ID.</param>
        /// <returns></returns>
        private static News[] FetchNews(int id, int newsCount, int languageID)
        {
            //using (var conn = Config.DB.Open())
            {
                List<News> lNews = new List<News>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchNews", id, newsCount, languageID))
                {

                    while (reader.Read())
                    {
                        News news = new News();
                        news.id = (int) reader["NID"];
                        news.Text = (string) reader["Text"];
                        news.PublishDate = (DateTime) reader["Date"];
                        news.Title = (string) reader["Title"];

                        lNews.Add(news);
                    }
                    reader.Close();
                }
                return lNews.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "SaveNews",
                                          (id > 0) ? (object) id : null, text, publishDate, title, languageID);
            }
        }

        /// <summary>
        /// Deletes the specified news ID.
        /// </summary>
        /// <param name="newsID">The news ID.</param>
        public static void Delete(int newsID)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "DeleteNews", newsID);
            }
        }
    }
}