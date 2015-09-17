using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class CreditsHistory
    {
        #region fields

        private int? id = null;
        private string username;
        private DateTime date = DateTime.Now;
        private eService service;
        private int amount = 0;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eService
        {
            None,
            SendMessage,
            ViewPhotos,
            ViewVideo,
            UseIM,
            CreateBlogPost,
            SendEcard,
            SendWink,
            SendGift,
            ViewVideoStream
        }

        public enum eSortColumn
        {
            None,
            Date,
            Username,
        }

        #endregion

        #region Constructors

        private CreditsHistory()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupPost"/> class.
        /// </summary>
        /// <param name="username">Represents the username that creates this post.</param>
        public CreditsHistory(string username)
        {
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
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public eService Service
        {
            get { return service; }
            set { service = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region Methods

        public static CreditsHistory[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, sortColumn);
        }

        public static CreditsHistory[] Fetch(string username)
        {
            return Fetch(null, username, null, eSortColumn.None);
        }

        public static CreditsHistory Fetch(int id)
        {
            CreditsHistory[] creditsHistory = Fetch(id, null, null, eSortColumn.None);

            if (creditsHistory.Length > 0)
            {
                return creditsHistory[0];
            }
            else
            {
                return null;
            }    
        }

        private static CreditsHistory[] Fetch(int? id, string username, DateTime? date, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                List<CreditsHistory> lCreditsHistory = new List<CreditsHistory>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchCreditsHistory",
                                                                    id, username, date, sortColumn))
                {

                    while (reader.Read())
                    {
                        CreditsHistory creditsHistory = new CreditsHistory();

                        creditsHistory.id = (int) reader["ID"];
                        creditsHistory.username = (string) reader["Username"];
                        creditsHistory.date = (DateTime) reader["Date"];
                        creditsHistory.service = (eService) reader["Service"];
                        creditsHistory.amount = (int) reader["Amount"];

                        lCreditsHistory.Add(creditsHistory);
                    }
                    reader.Close();
                }

                return lCreditsHistory.ToArray();
            }
        }

        public static DataTable FetchByUsername(string username)
        {
            //using (var conn = Config.DB.Open())
            {
                DataTable dtResults = new DataTable();

                dtResults.Columns.Add("Date");
                dtResults.Columns.Add("Credits");
                dtResults.Columns.Add("Service");

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchCreditsHistoryByUsername", username))
                {

                    while (reader.Read())
                    {
                        string service = String.Empty;
                        int srv = (int) reader["Service"];
                        if (srv == (int) eService.SendMessage)
                        {
                            service = Lang.Trans("Sent Message");
                        }
                        else if (srv == (int) eService.UseIM)
                        {
                            service = Lang.Trans("Instant Messenger");
                        }
                        else if (srv == (int) eService.ViewPhotos)
                        {
                            service = Lang.Trans("View Photos");
                        }
                        else if (srv == (int) eService.ViewVideo)
                        {
                            service = Lang.Trans("View Video");
                        }
                        else if (srv == (int) eService.CreateBlogPost)
                        {
                            service = Lang.Trans("View Blog Post");
                        }
                        else if (srv == (int) eService.SendEcard)
                        {
                            service = Lang.Trans("Send e-card");
                        }

                        dtResults.Rows.Add((DateTime) reader["Date"], (int) reader["Credits"], service);
                    }
                    reader.Close();
                }
                return dtResults;
            }
        }

//        public static int[] Search(int? groupTopicID, string username, DateTime? datePosted, DateTime? dateEdited, string keyword, eSortColumn sortColumn)
//        {
//            //using (var conn = Config.DB.Open())
//            {
//                var reader = SqlHelper.GetDB().ExecuteReader( "SearchGroupPosts",
//                                                               groupTopicID, username, datePosted,
//                                                               dateEdited, keyword, sortColumn);
//
//                List<int> lGroupPostsIDs = new List<int>();
//
//                while (reader.Read())
//                {
//                    lGroupPostsIDs.Add((int) reader["ID"]);    
//                }
//
//                return lGroupPostsIDs.ToArray();
//            }
//        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveCreditsHistory",
                                                        id, username, date, service, amount);
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
            Delete(id, null);
        }

        public static void Delete(string username)
        {
            Delete(null, username);
        }

        private static void Delete(int? id, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteCreditsHistory", id, username);
            }
        }

        #endregion
    }
}
