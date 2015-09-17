using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Caching;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    [Serializable]
    public class Ad
    {
        #region fields

        private int? id = null;
        private int categoryID;
        private string postedBy = null;
        private DateTime date = DateTime.Now;
        private DateTime expirationDate;
        private string location = String.Empty;
        private string subject = String.Empty;
        private string description = String.Empty;
        private bool approved = false;
        
        private eSortColumn sortColumn;

        public enum eSortColumn
        {
            None,
            Date,
            ExpirationDate,
            CategoryID,
            PostedBy,
            Approved
        }

        #endregion

        #region Constructors

        private Ad(){}

        public Ad(int categoryID, string postedBy)
        {
            this.categoryID = categoryID;
            this.postedBy = postedBy;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// The property is read-only.
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

        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }

        public string PostedBy
        {
            get { return postedBy; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public DateTime ExpirationDate
        {
            get { return expirationDate; }
            set { expirationDate = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        #region Methods

        public static Ad[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, null, eSortColumn.None);
        }

        public static Ad[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, null, null, sortColumn);
        }

        public static Ad[] Fetch(bool approved, eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, approved, null, sortColumn);
        }

        public static Ad Fetch(int id)
        {
            string cacheKey = String.Format("Ad_Fetch_Id_{0}", id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as Ad;
            }

            Ad[] ads = Fetch(id, null, null, null, null, null, null, eSortColumn.None);

            if (ads.Length > 0 && HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, ads[0], null, DateTime.Now.AddMinutes(10),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return ads.Length > 0 ? ads[0] : null;
        }

        public static Ad[] Fetch(string postedBy)
        {
            return Fetch(null, null, postedBy, null, null, true, null, eSortColumn.Date);
        }

        public static Ad[] FetchByCategory(int categoryID, bool approved, eSortColumn eSortColumn)
        {
            return Fetch(null, categoryID, null, null, null, approved, null, eSortColumn);
        }

        private static Ad[] Fetch(int? id, int? categoryID, string postedBy,
                                            DateTime? date, DateTime? expirationDate, bool? approved,
                                            int? numberOfAds, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader(
                                                               "FetchAds",
                                                               id, categoryID, postedBy, date, expirationDate,
                                                               approved, numberOfAds, sortColumn);

                List<Ad> ads = new List<Ad>();

                while (reader.Read())
                {
                    Ad ad = new Ad();

                    ad.id = (int)reader["ID"];
                    ad.categoryID = (int) reader["CategoryID"];
                    ad.postedBy = (string) reader["PostedBy"];
                    ad.date = (DateTime) reader["Date"];
                    ad.expirationDate = (DateTime) reader["ExpirationDate"];
                    ad.location = (string) reader["Location"];
                    ad.subject = (string)reader["Subject"];
                    ad.description = (string) reader["Description"];
                    ad.approved = (bool) reader["Approved"];

                    ads.Add(ad);
                }

                return ads.ToArray();
            }
        }

        public static int[] Search(int? categoryID, string postedBy,
                                            DateTime? date, DateTime? expirationDate, bool? approved,
                                            string keyword, int? numberOfAds, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "SearchAds",
                                                                categoryID, postedBy, date, expirationDate, approved,
                                                                keyword, numberOfAds, sortColumn);

                List<int> lGroupIDs = new List<int>();

                while (reader.Read())
                {
                    lGroupIDs.Add((int)reader["ID"]);
                }

                return lGroupIDs.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveAd",
                                                            id, categoryID, postedBy,
                                                            date, expirationDate, location, subject, description, approved);

                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }

                string cacheKey = String.Format("Ad_Fetch_Id_{0}", id);
                if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        public static void Delete(int id)
        {
            DeleteBy(id, null, null);
        }

        private static void DeleteBy(int? id, int? categoryID, string postedBy)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteAd", id, categoryID, postedBy);
            }

            string cacheKey = String.Format("Ad_Fetch_Id_{0}", id);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                HttpContext.Current.Cache.Remove(cacheKey);
            }
        }

        #endregion
    }

    [Serializable]
    public class AdSearchResults : SearchResults<int, Ad>
    {
        public int[] Ads
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

        public new int GetTotalPages(int adsPerPage)
        {
            return base.GetTotalPages(adsPerPage);
        }

        protected override Ad LoadResult(int id)
        {
            return Ad.Fetch(id);
        }
        protected override Ad[] LoadResults(int[] ids)
        {
            throw new NotImplementedException();
        }
        
        public new Ad[] GetPage(int Page, int adsPerPage)
        {
            return base.GetPage(Page, adsPerPage);
        }

        public Ad[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }

    public class AdComment
    {
        #region Fields

        private int? id;
        private int adID;
        private string username;
        private string commentText;
        private DateTime date = DateTime.Now;

        public enum eSortColumn
        {
            None,
            Date
        }

        #endregion

        #region Properties

        public int ID
        {
            get
            {
                if (id.HasValue)
                {
                    return id.Value;
                }
                throw new Exception("The field ID is not set!");
            }
        }

        public int AdID
        {
            get { return adID; }
        }

        public string Username
        {
            get { return username; }
        }

        public string CommentText
        {
            get { return commentText; }
            set { commentText = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        #endregion

        private AdComment()
        {
        }

        public AdComment(int adID, string username)
        {
            this.adID = adID;
            this.username = username;
        }

        public static AdComment[] Fetch()
        {
            return Fetch(null, null, null, null, null, eSortColumn.None);
        }

        public static AdComment Fetch(int id)
        {
            AdComment[] comments = Fetch(id, null, null, null, null, eSortColumn.None);
            return comments.Length > 0 ? comments[0] : null;
        }

        public static AdComment[] Fetch(int adID, string username)
        {
            return Fetch(null, adID, username, null, null, eSortColumn.None);
        }

        public static AdComment[] FetchByAdID(int adID, int numberOfCommnets, eSortColumn sortColumn)
        {
            return Fetch(null, adID, null, null, numberOfCommnets, sortColumn);
        }

        public static AdComment[] FetchByAdID(int adID, eSortColumn sortColumn)
        {
            return Fetch(null, adID, null, null, null, sortColumn);
        }

        private static AdComment[] Fetch(int? id, int? adID, string username, DateTime? date, int? numberOfComments, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchAdComments",
                                                               id, adID, username, date, numberOfComments, sortColumn);

                List<AdComment> lAdComments = new List<AdComment>();

                while (reader.Read())
                {
                    AdComment adComment = new AdComment();

                    adComment.id = (int) reader["ID"];
                    adComment.adID = (int) reader["AdID"];
                    adComment.username = (string) reader["Username"];
                    adComment.commentText = (string) reader["Comment"];
                    adComment.date = (DateTime) reader["Date"];

                    lAdComments.Add(adComment);

                }

                return lAdComments.ToArray();
            }
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar(
                                                        "SaveAdComment", id, adID, username, commentText, date);
                if (id == -1)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static void Delete(int id)
        {
            DeleteBy(id, null, null);
        }

        private static void DeleteBy(int? id, int? adID, string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteAdComment", id, adID, username);
            }
        }
    }
}
