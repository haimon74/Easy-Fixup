using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;
using System.Web.Caching;

namespace ezFixUp.Classes
{
    [Serializable]
    public class PhotoContestEntry
    {
        #region Internal static fields

        static Dictionary<int, List<int>> contestEntriesRanks = new Dictionary<int, List<int>>();
        static Dictionary<int, DateTime> contestEntriesRanksLastUpdated = new Dictionary<int, DateTime>();

        #endregion

        #region Fields

        private int? id;
        private int contestId;
        private string username;
        private int photoId;

        #endregion

        #region Properties

        public int Id
        {
            get { return id.Value; }
            set { id = value; }
        }

        public int ContestId
        {
            get { return contestId; }
            set { contestId = value; }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public int PhotoId
        {
            get { return photoId; }
            set { photoId = value; }
        }

        #endregion

        private PhotoContestEntry()
        {
        }

        public PhotoContestEntry(int contestId, string username, int photoId)
        {
            this.contestId = contestId;
            this.username = username;
            this.photoId = photoId;
        }

        public static PhotoContestEntry[] LoadByContest(int contestId)
        {
            string cacheKey = String.Format("PhotoContestEntry_LoadByContest_{0}", contestId);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as PhotoContestEntry[];
            }

            PhotoContestEntry[] entries = Load(null, contestId, null, null);

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, entries, null, DateTime.Now.AddMinutes(10),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return entries;
        }

        public static PhotoContestEntry Load(int id)
        {
            PhotoContestEntry[] entries = Load(id, null, null, null);
            if (entries.Length >= 1)
                return entries[0];
            else
                return null;
        }

        public static PhotoContestEntry Load(int contestId, string username)
        {
            PhotoContestEntry[] entries = Load(null, contestId, username, null);
            if (entries.Length >= 1)
                return entries[0];
            else
                return null;
        }

        public static PhotoContestEntry[] Load(int? id, int? contestId, string username, int? photoId)
        {
            return Load(id, contestId, username, photoId, null, null);
        }

        public static PhotoContestEntry[] Load(int? id, int? contestId, string username, int? photoId,
                                               string notRankedBy, int? countLimit)
        {
            return Load(id, contestId, username, photoId, notRankedBy, countLimit, false);
        }

        public static PhotoContestEntry[] Load(int? id, int? contestId, string username, int? photoId,
                                               string notRankedBy, int? countLimit, bool randomize)
        {
            List<PhotoContestEntry> entries = new List<PhotoContestEntry>();
            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("LoadPhotoContestEntry", id, contestId,
                                                                    username, photoId, notRankedBy, countLimit,
                                                                    randomize))
                {
                    while (reader.Read())
                    {
                        PhotoContestEntry entry = new PhotoContestEntry();

                        entry.id = (int) reader["Id"];
                        entry.contestId = (int) reader["ContestId"];
                        entry.username = (string) reader["Username"];
                        entry.photoId = (int) reader["PhotoId"];
                        entries.Add(entry);
                    }
                    reader.Close();
                }
            }

            return entries.ToArray();
        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SavePhotoContestEntry", id, contestId,
                                                        username, photoId);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }

            string cacheKey = String.Format("PhotoContestEntry_LoadByContest_{0}", contestId);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        public static int[] FetchTop(int contestId)
        {
            return FetchTop(contestId, null);
        }

        public static int[] FetchTop(int contestId, int? maxCount)
        {
            List<int> entriesIds = new List<int>();

            List<int> entriesRanks;
            lock (contestEntriesRanks)
            {
                if (!contestEntriesRanks.ContainsKey(contestId))
                    contestEntriesRanks.Add(contestId, new List<int>());
                entriesRanks = contestEntriesRanks[contestId];
            }

            bool updateRanks = false;
            lock (contestEntriesRanksLastUpdated)
            {
                if (!contestEntriesRanksLastUpdated.ContainsKey(contestId))
                    contestEntriesRanksLastUpdated.Add(contestId, DateTime.MinValue);
                if (contestEntriesRanksLastUpdated[contestId] < DateTime.Now.AddMinutes(-10))
                {
                    updateRanks = true;
                    contestEntriesRanksLastUpdated[contestId] = DateTime.Now;
                }
            }

            if (updateRanks)
            {
                List<int> updatedEntries = new List<int>();
                //using (var conn = Config.DB.Open())
                {
                    using (var reader = SqlHelper.GetDB().ExecuteReader("FetchTopEntries", contestId,
                                                                        Config.Ratings.FavoriteEntriesCount))
                    {
                        while (reader.Read())
                            updatedEntries.Add((int) reader["Id"]);
                        reader.Close();
                    }
                }
                lock (entriesRanks)
                {
                    entriesRanks.Clear();
                    entriesRanks.AddRange(updatedEntries);
                }
            }

            int count = 0;
            lock (entriesRanks)
            {
                foreach (int entryId in entriesRanks)
                {
                    entriesIds.Add(entryId);
                    if (maxCount.HasValue && ++count >= maxCount) break;
                }
            }

            return entriesIds.ToArray();
        }

        public static int FindRank(int contestId, int entryId)
        {
            lock (contestEntriesRanks)
            {
                if (!contestEntriesRanks.ContainsKey(contestId)) return -1;

                lock (contestEntriesRanks[contestId])
                {
                    return contestEntriesRanks[contestId].IndexOf(entryId) + 1;
                }
            }
        }

        public static void DeleteByPhoto(int photoId)
        {
            PhotoContestEntry[] entries = Load(null, null, null, photoId);

            if (entries != null && entries.Length > 0)
            {
                foreach (PhotoContestEntry entry in entries)
                {
                    Delete(entry.Id);
                }
            }
        }
        
        public static void DeleteByUsername(string username)
        {
            PhotoContestEntry[] entries = Load(null, null, username, null);

            if (entries != null && entries.Length > 0)
            {
                foreach (PhotoContestEntry entry in entries)
                {
                    Delete(entry.Id);
                }
            }
        }

        public static void Delete(PhotoContestEntry entry)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeletePhotoContestEntry", entry.id);
            }

            string cacheKey = String.Format("PhotoContestEntry_LoadByContest_{0}", entry.contestId);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }

            lock (contestEntriesRanks)
            {
                if (contestEntriesRanksLastUpdated.ContainsKey(entry.contestId))
                    contestEntriesRanksLastUpdated[entry.contestId] = DateTime.MinValue;
            }
        }

        public static void Delete(int id)
        {
            PhotoContestEntry entry = Load(id);
            Delete(entry);
        }
    }

    [Serializable]
    public class PhotoContestEntriesSearchResults : SearchResults<int, PhotoContestEntry>
    {
        public int[] PhotoContestEntries
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

        public new int GetTotalPages(int entriesPerPage)
        {
            return base.GetTotalPages(entriesPerPage);
        }

        protected override PhotoContestEntry LoadResult(int id)
        {
            return PhotoContestEntry.Load(id);
        }
        protected override PhotoContestEntry[] LoadResults(int[] ids)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Use this method to get the search results.
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <param name="photoContestEntriesPerPage">The entries per page.</param>
        /// <returns></returns>
        public new PhotoContestEntry[] GetPage(int Page, int photoContestEntriesPerPage)
        {
            return base.GetPage(Page, photoContestEntriesPerPage);
        }
    }

    public class PhotoContestEntriesSearch
    {
        #region Properties

        private int? contestID = null;

        public int? ContestID
        {
            get { return contestID; }
            set { contestID = value; }
        }

        private string username = null;

        public string Username
        {
            get { return username; }
            set { username = value; }
        }
        
        private string sortColumn;

        public string SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        private bool sortAsc;

        public bool SortAsc
        {
            get { return sortAsc; }
            set { sortAsc = value; }
        }

        #endregion

        public PhotoContestEntriesSearch()
        {
            // Set defaults
            SortColumn = "Username";
            SortAsc = false;
        }

        public PhotoContestEntriesSearchResults GetResults()
        {
            //using (var conn = Config.DB.Open())
            {
                List<int> lResults = new List<int>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("SearchPhotoContestEntries",
                                                                    contestID, username, sortColumn))
                {

                    while (reader.Read())
                    {
                        lResults.Add((int) reader["ID"]);
                    }
                    reader.Close();
                }
                if (!sortAsc) lResults.Reverse();

                if (lResults.Count > 0)
                {
                    PhotoContestEntriesSearchResults results = new PhotoContestEntriesSearchResults();
                    results.PhotoContestEntries = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }        
}