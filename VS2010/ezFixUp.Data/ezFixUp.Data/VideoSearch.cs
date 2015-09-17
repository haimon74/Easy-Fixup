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
using ezFixUp.Data;
using ezFixUp.Data.LoveHitchParallel;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    [Serializable]
    public class VideoUploadSearchResults : SearchResults<int, VideoUpload>
    {
        public int[] Ids
        {
            get { return Results; }
            set { Results = value; }
        }

        public int GetTotalPages()
        {
            return GetTotalPages(Config.Search.VideosPerPage);
        }

        public new int GetTotalPages(int videosPerPage)
        {
            return base.GetTotalPages(videosPerPage);
        }

        protected override VideoUpload LoadResult(int id)
        {
            return VideoUpload.Load(id);
        }
        protected override VideoUpload[] LoadResults(int[] ids)
        {
            List<int> keysList = new List<int>();// ids.ToList();
            foreach (var id in ids)
            {
                keysList.Add(id);
            }
            List<VideoUpload> resultList = new List<VideoUpload>();
            if (keysList.Count > 0)
            {
                var parameterizedThreadsList = new ParameterizedThreadsList<int, VideoUpload>(
                                                    keysList, resultList,
                                                    (new LoadVideosResultThreadClass(keysList, resultList)).DoWorkThread,
                                                    HttpContext.Current);
                parameterizedThreadsList.RunAsBackgroundThreads = true;
                parameterizedThreadsList.ExecuteParallelWork(5);
            }
            return resultList.ToArray();
        }

        /// <summary>
        /// Use this method to get the search results
        /// Number of videos per page is defined in Config.Search
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <returns>Array of usernames</returns>
        public VideoUpload[] GetPage(int Page)
        {
            return GetPage(Page, Config.Search.VideosPerPage);
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="videosPerPage">Videos per page</param>
        /// <returns>Array of usernames</returns>
        public new VideoUpload[] GetPage(int Page, int videosPerPage)
        {
            return base.GetPage(Page, videosPerPage);
        }

        public VideoUpload[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }

    public class VideoUploadsSearch
    {
        #region Properties

        private string username = null;
        private User.eGender? gender = null;
        private int? maxAge;
        private int? minAge;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public User.eGender? Gender
        {
            get { return gender; }
            set
            {
                gender = value;
            }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        private int videosCount = 0;

        public int VideosCount
        {
            get { return videosCount; }
            set { videosCount = value; }
        }

        private bool? isPrivate = null;

        public bool? IsPrivate
        {
            get { return isPrivate; }
            set { isPrivate = value; }
        }

        private bool? approved = null;

        public bool? Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        public enum eSortColumn
        {
            None,
            ID,
            Username
        }

        private eSortColumn sortColumn;

        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        private bool sortAsc = true;

        public bool SortAsc
        {
            get { return sortAsc; }
            set { sortAsc = value; }
        }

        #endregion

        public VideoUploadSearchResults GetResults(bool useCache)
        {
            if (!useCache || HttpContext.Current == null)
                return GetResults();

            string cacheKey = String.Format("VideoUploadsSearch_{0}_{1}_{2}_{3}_{4}",
                                            username, gender, approved, isPrivate, videosCount);

            Cache cache = HttpContext.Current.Cache;
            if (cache[cacheKey] != null && cache[cacheKey] is VideoUploadSearchResults)
            {
                return (VideoUploadSearchResults)cache[cacheKey];
            }
            else
            {
                VideoUploadSearchResults results = GetResults();
                if (results != null)
                    cache.Add(cacheKey, results, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                return results;
            }
        }

        public VideoUploadSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) 
                    SqlHelper.GetDB().ExecuteReader( "VideoUploadsSearch", username, gender,
                                            !maxAge.HasValue
                                                ? null
                                                : (object)
                                                  DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1)*365.25)),
                                            !minAge.HasValue
                                                ? null
                                                : (object) DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value*365.25)),
                                            approved, isPrivate,
                                            videosCount, sortColumn);

                List<int> lResults = new List<int>();

                while (reader.Read())
                {
                    lResults.Add((int)reader["Id"]);
                }

                if (!sortAsc) lResults.Reverse();

                if (lResults.Count > 0)
                {
                    VideoUploadSearchResults results = new VideoUploadSearchResults();
                    results.Ids = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }

    [Serializable]
    public class VideoEmbedsSearchResults : SearchResults<int, VideoEmbed>
    {
        public int[] Ids
        {
            get { return Results; }
            set { Results = value; }
        }

        public int GetTotalPages()
        {
            return GetTotalPages(Config.Search.VideosPerPage);
        }

        public new int GetTotalPages(int videosPerPage)
        {
            return base.GetTotalPages(videosPerPage);
        }

        protected override VideoEmbed LoadResult(int id)
        {
            return VideoEmbed.Load(id);
        }
        protected override VideoEmbed[] LoadResults(int[] ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Use this method to get the search results
        /// Number of videos per page is defined in Config.Search
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <returns>Array of usernames</returns>
        public VideoEmbed[] GetPage(int Page)
        {
            return GetPage(Page, Config.Search.VideosPerPage);
        }

        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="Page">Page number</param>
        /// <param name="videosPerPage">Videos per page</param>
        /// <returns>Array of usernames</returns>
        public new VideoEmbed[] GetPage(int Page, int videosPerPage)
        {
            return base.GetPage(Page, videosPerPage);
        }

        public VideoEmbed[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }

    public class VideoEmbedsSearch
    {
        #region Properties

        private string username = null;
        private User.eGender? gender = null;
        private int? maxAge;
        private int? minAge;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public User.eGender? Gender
        {
            get { return gender; }
            set
            {
                gender = value;
            }
        }

        public int? MinAge
        {
            get { return minAge; }
            set { minAge = value; }
        }

        public int? MaxAge
        {
            get { return maxAge; }
            set { maxAge = value; }
        }

        private string keyword;
        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        private int videosCount = 0;

        public int VideosCount
        {
            get { return videosCount; }
            set { videosCount = value; }
        }

        public enum eSortColumn
        {
            None,
            ID,
            Username
        }

        private eSortColumn sortColumn;

        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        private bool sortAsc = true;

        public bool SortAsc
        {
            get { return sortAsc; }
            set { sortAsc = value; }
        }

        #endregion

        public VideoEmbedsSearchResults GetResults(bool useCache)
        {
            if (!useCache || HttpContext.Current == null)
                return GetResults();

            string cacheKey = String.Format("VideoEmbedsSearch_{0}_{1}_{2}",
                                            username, gender, videosCount);

            Cache cache = HttpContext.Current.Cache;
            if (cache[cacheKey] != null && cache[cacheKey] is VideoEmbedsSearchResults)
            {
                return (VideoEmbedsSearchResults)cache[cacheKey];
            }
            else
            {
                VideoEmbedsSearchResults results = GetResults();
                if (results != null)
                    cache.Add(cacheKey, results, null, DateTime.Now.Add(TimeSpan.FromMinutes(10)),
                              Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
                return results;
            }
        }

        public VideoEmbedsSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader)
                    SqlHelper.GetDB().ExecuteReader( "VideoEmbedsSearch", username, gender,
                                            !maxAge.HasValue
                                                ? null
                                                : (object)
                                                  DateTime.Now.Subtract(TimeSpan.FromDays((maxAge.Value + 1)*365.25)),
                                            !minAge.HasValue
                                                ? null
                                                : (object) DateTime.Now.Subtract(TimeSpan.FromDays(minAge.Value*365.25)),
                                            keyword,
                                            videosCount, sortColumn);

                List<int> lResults = new List<int>();

                while (reader.Read())
                {
                    lResults.Add((int)reader["Id"]);
                }

                if (!sortAsc) lResults.Reverse();

                if (lResults.Count > 0)
                {
                    VideoEmbedsSearchResults results = new VideoEmbedsSearchResults();
                    results.Ids = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }
}