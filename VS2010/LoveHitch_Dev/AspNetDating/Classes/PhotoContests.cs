using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using AspNetDating.Classes.LoveHitchParallel;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class PhotoContest
    {
        #region Fields

        private int? id;
        private string name;
        private User.eGender? gender;
        private string description;
        private string terms;
        private int? minAge;
        private int? maxAge;
        private DateTime dateCreated;
        private DateTime? dateEnds;

        #endregion

        #region Properties

        public int Id
        {
            get { return id.Value; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public User.eGender? Gender
        {
            get { return gender; }
            set { gender = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Terms
        {
            get { return terms; }
            set { terms = value; }
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

        public DateTime DateCreated
        {
            get { return dateCreated; }
            set { dateCreated = value; }
        }

        public DateTime? DateEnds
        {
            get { return dateEnds; }
            set { dateEnds = value; }
        }

        #endregion

        private PhotoContest()
        {
        }

        public PhotoContest(string name, string description)
        {
            this.name = name;
            this.description = description;
            dateCreated = DateTime.Now;
        }

        public static PhotoContest[] LoadAll()
        {
            return Load(null);
        }

        public static PhotoContest Load(int id)
        {
            PhotoContest[] contests = Load((int?) id);
            if (contests.Length >= 1)
                return contests[0];
            else
                return null;
        }

        public static PhotoContest[] Load(int? id)
        {
            List<PhotoContest> contests = new List<PhotoContest>();
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "LoadPhotoContest", id);

                while (reader.Read())
                {
                    PhotoContest contest = new PhotoContest();

                    contest.id = (int) reader["Id"];
                    contest.name = (string) reader["Name"];
                    if (reader["Gender"] != DBNull.Value)
                        contest.gender = (User.eGender) reader["Gender"];
                    contest.description = (string) reader["Description"];
                    contest.terms = (string) reader["Terms"];
                    if (reader["MinAge"] is int)
                        contest.minAge = (int) reader["MinAge"];
                    if (reader["MaxAge"] is int)
                        contest.maxAge = (int)reader["MaxAge"];
                    contest.dateCreated = (DateTime)reader["DateCreated"];
                    if (reader["DateEnds"] is DateTime)
                        contest.dateEnds = (DateTime)reader["DateEnds"];

                    contests.Add(contest);
                }
            }

            return contests.ToArray();
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SavePhotoContest", id, name, gender,
                                                        description, terms, minAge, maxAge, dateCreated, dateEnds);

                if (!id.HasValue)
                    id = Convert.ToInt32(result);
            }
        }
        
        public static void Delete(int contestId)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteScalar(conn, "DeletePhotoContest", contestId);
            }            
        }
    }

    [Serializable]
    public class PhotoContestSearchResults : SearchResults<int, PhotoContest>
    {
        public int[] PhotoContests
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

        public new int GetTotalPages(int groupsPerPage)
        {
            return base.GetTotalPages(groupsPerPage);
        }

        protected override PhotoContest LoadResult(int id)
        {
            return PhotoContest.Load(id);
        }
        protected override PhotoContest[] LoadResults(int[] ids)
        {
            List<int> keysList = new List<int>();// ids.ToList();
            foreach (var id in ids)
            {
                keysList.Add(id);
            }
            List<PhotoContest> resultList = new List<PhotoContest>();
            if (keysList.Count > 0)
            {
                var parameterizedThreadsList = new ParameterizedThreadsList<int, PhotoContest>(
                                                    keysList, resultList,
                                                    (new LoadPhotosContestResultThreadClass(keysList, resultList)).DoWorkThread,
                                                    HttpContext.Current);
                parameterizedThreadsList.RunAsBackgroundThreads = true;
                parameterizedThreadsList.ExecuteParallelWork(5);
            }
            return resultList.ToArray();
        }

        
        /// <summary>
        /// Use this method to get the search results.
        /// </summary>
        /// <param name="Page">The page.</param>
        /// <param name="photoContestsPerPage">The contests per page.</param>
        /// <returns></returns>
        public new PhotoContest[] GetPage(int Page, int photoContestsPerPage)
        {
            return base.GetPage(Page, photoContestsPerPage);
        }
    }

    public class PhotoContestSearch
    {
        #region Properties

        private bool? active = null;

        public bool? Active
        {
            get { return active.Value; }
            set { active = value; }
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

        public PhotoContestSearch()
        {
            // Set defaults
            SortColumn = "DateCreated";
            SortAsc = false;
        }

        public PhotoContestSearchResults GetResults()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "SearchPhotoContests",
                                            active, sortColumn);

                List<int> lResults = new List<int>();

                while (reader.Read())
                {
                    lResults.Add((int)reader["ID"]);
                }

                if (!sortAsc) lResults.Reverse();

                if (lResults.Count > 0)
                {
                    PhotoContestSearchResults results = new PhotoContestSearchResults();
                    results.PhotoContests = lResults.ToArray();
                    return results;
                }
                else
                    return null;
            }
        }
    }    
}