using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// 
    /// </summary>
    public class AffiliateHistory
    {
        #region fields

        private int? id = null;
        private int affiliateID;
        private decimal amount;
        private DateTime datePaid;
        private string notes;
        private string privateNotes;
        private eSortColumn sortColumn;


        /// <summary>
        /// Specify on which column affiliates will be sorted.
        /// </summary>
        public enum eSortColumn
        {
            /// <summary>
            /// No sort.
            /// </summary>
            None,
            /// <summary>
            /// On amount created column.
            /// </summary>
            Amount,
            /// <summary>
            /// On date paid column.
            /// </summary>
            DatePaid
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        public int? ID
        {
            get { return id; }
        }

        /// <summary>
        /// Gets the affiliate ID.
        /// </summary>
        /// <value>The affiliate ID.</value>
        public int AffiliateID
        {
            get { return affiliateID; }
        }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        /// <summary>
        /// Gets or sets the date payed.
        /// </summary>
        /// <value>The date payed.</value>
        public DateTime DatePaid
        {
            get { return datePaid; }
            set { datePaid = value; }
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes
        {
            get { return notes; }
            set { notes = value; }
        }

        /// <summary>
        /// Gets or sets the private notes.
        /// </summary>
        /// <value>The private notes.</value>
        public string PrivateNotes
        {
            get { return privateNotes; }
            set { privateNotes = value; }
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

        #endregion

        private AffiliateHistory()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateHistory"/> class.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        public AffiliateHistory(int affiliateID)
        {
            this.affiliateID = affiliateID;
        }

        #region Methods

        /// <summary>
        /// Fetches all affiliates history from DB.
        /// If there are no affiliates in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static AffiliateHistory[] Fetch()
        {
            return Fetch(null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches the specified sort column.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static AffiliateHistory[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches affiliate history by specified id from DB.
        /// If the affiliate history doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static AffiliateHistory Fetch(int id)
        {
            AffiliateHistory[] affiliatesHistory = Fetch(id, null, null, null, eSortColumn.None);

            if (affiliatesHistory.Length > 0)
            {
                return affiliatesHistory[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all affiliates history by specified affiliate ID from DB.
        /// It returns an empty array if there are no affiliates history in DB
        /// by specified affiliate ID.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <returns></returns>
        public static AffiliateHistory[] FetchByAffiliateID(int affiliateID)
        {
            return Fetch(null, affiliateID, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches the by affiliate ID.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static AffiliateHistory[] FetchByAffiliateID(int affiliateID, eSortColumn sortColumn)
        {
            return Fetch(null, affiliateID, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches affiliates history by specified arguments.
        /// It returns an empty array if there are no affiliates history in DB by specified arguments.
        /// If these arguments are null it returns all affiliates history from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="datePayed">The date payed.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static AffiliateHistory[] Fetch(int? id, int? affiliateID, decimal? amount, DateTime? datePayed, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchAffiliatesHistory", id, affiliateID, amount, datePayed, sortColumn);

                List<AffiliateHistory> lAffiliatesHistory = new List<AffiliateHistory>();

                while (reader.Read())
                {
                    AffiliateHistory affiliate = new AffiliateHistory();

                    affiliate.id = (int)reader["ID"];
                    affiliate.affiliateID = (int)reader["AffiliateID"];
                    affiliate.amount = (decimal)reader["Amount"];
                    affiliate.datePaid = (DateTime)reader["DatePaid"];
                    affiliate.notes = reader["Notes"] == DBNull.Value ? null : (string) reader["Notes"];
                    affiliate.privateNotes = reader["PrivateNotes"] == DBNull.Value ? null : (string)reader["PrivateNotes"];

                    lAffiliatesHistory.Add(affiliate);
                }

                return lAffiliatesHistory.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance in DB. If the field id is null it inserts new record in DB,
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveAffiliateHistory",
                                        id, affiliateID, amount, datePaid, notes, privateNotes);

                if (!id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes affiliate history by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            DeleteBy(id, null);
        }

        /// <summary>
        /// Deletes all affiliates history by specified affiliate ID.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        public static void DeleteByAffiliateID(int affiliateID)
        {
            DeleteBy(null, affiliateID);
        }

        /// <summary>
        /// Deletes affiliate history from DB by specified ID or affiliate ID.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="affiliateID">The affiliate ID.</param>
        private static void DeleteBy(int? id, int? affiliateID)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteAffiliateHistory", id, affiliateID);
            }
        }

        #endregion
    }
}
