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
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class ContentView
    {
        #region Properties

        private string key = null;

        private string content = "";

        private int languageID;

        public string Key
        {
            get { return key; }
        }

        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        public int LanguageID
        {
            get { return languageID; }
            set { languageID = value; }
        }

        #endregion

        private ContentView()
        {
        }
        
        public ContentView(string key, int languageID)
        {
            this.key = key;
            this.languageID = languageID;
        }

        /// <summary>
        /// Fetches content view by specified key.
        /// </summary>
        /// <param name="key">ContentView key identifier</param>
        /// <returns>Content View instance</returns>
        public static ContentView FetchContentView(string key, int languageID)
        {
            string cacheKey = null;
            
            if (key != null)
            {
                cacheKey = String.Format("ContentView_FetchContentView_{0}_{1}", key, languageID);
                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                {
                    return HttpContext.Current.Cache[cacheKey] as ContentView;
                }
            }
            
            ContentView[] cv = FetchContentViews(key, languageID);

            if (cv.Length > 0)
            {
                if (key != null && HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Insert(cacheKey, cv[0], null, DateTime.Now.AddMinutes(30),
                        Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                }
                
                return cv[0];
            }
            else
                return null;
        }

        /// <summary>
        /// Fetches all content views by the specified language from database.
        /// </summary>
        /// <returns>Array of content views.</returns>
        public static ContentView[] FetchContentView(int languageID)
        {
            string cacheKey = String.Format("ContentView_FetchContentViews_{0}", languageID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as ContentView[];
            }

            ContentView[] views = FetchContentViews(null, languageID);

            if (HttpContext.Current != null)
            {
                //Global.AddCacheItem("ContentPage", cacheKey, views);
                HttpContext.Current.Cache.Insert(cacheKey, views, null, DateTime.Now.AddMinutes(30),
                    Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return views;
        }

        /// <summary>
        /// Fetches all content views from database by specified key or language.
        /// </summary>
        /// <param name="key">Represents content view key.</param>
        /// <returns>Array of content views.</returns>
        private static ContentView[] FetchContentViews(string key, int? languageID)
        {
            List<ContentView> lViews = new List<ContentView>();
            
            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchContentViews", key, languageID))
                {

                    while (reader.Read())
                    {
                        ContentView contentView = new ContentView();
                        contentView.key = (string) reader["Key"];
                        contentView.content = (string) reader["Content"];
                        contentView.languageID = (int) reader["Language"];
                        lViews.Add(contentView);
                    }
                    reader.Close();
                }
            }
            return lViews.ToArray();
        }

        public void Save()
        {
            if (key == null)
                throw new Exception("key is not set");

            //using (var conn = Config.DB.Open())
            {
                    SqlHelper.GetDB().ExecuteNonQuery( "SaveContentView", key, content, languageID);
            }

            string cacheKey = String.Format("ContentView_FetchContentViews_{0}", languageID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }

            cacheKey = String.Format("ContentView_FetchContentView_{0}_{1}", key, languageID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }            
        }

        //public static void Delete(string key)
        //{
        //    try
        //    {
        //        ContentView view = FetchContentView(key);
        //        string cacheKey = String.Format("ContentView_FetchContentViews_{0}", view.languageID);
        //        if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
        //        {
        //            HttpContext.Current.Cache.Remove(cacheKey);
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //        Global.Logger.LogError(err);
        //    }

        //    //using (var conn = Config.DB.Open())
        //    {
        //        SqlHelper.GetDB().ExecuteNonQuery( "DeleteContentView", key);
        //    }
        //}        
    }
}
