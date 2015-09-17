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
    public class AffiliateCommission
    {
        #region fields

        private int? id = null;
        private int affiliateID;
        private int? paymentHistoryID;
        private string username;
        private DateTime timeStamp = DateTime.Now;
        private string notes;
        private decimal amount;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            TimeStamp,
            Amount,
            Username
        }

        #endregion

        #region Constructors

        private AffiliateCommission()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateCommission"/> class.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        public AffiliateCommission(int affiliateID)
        {
            this.affiliateID = affiliateID;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AffiliateCommission"/> class.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <param name="paymentHistoryID">The payment history ID.</param>
        public AffiliateCommission(int affiliateID, int paymentHistoryID)
        {
            this.affiliateID = affiliateID;
            this.paymentHistoryID = paymentHistoryID;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
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
        /// Gets the affiliate ID.
        /// </summary>
        /// <value>The affiliate ID.</value>
        public int AffiliateID
        {
            get { return affiliateID; }
        }

        /// <summary>
        /// Gets the payment history ID.
        /// </summary>
        /// <value>The payment history ID.</value>
        public int? PaymentHistoryID
        {
            get
            {
                return paymentHistoryID;
            }
        }

        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
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
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all affiliate commissions from DB.
        /// If there are no affiliate commissions in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static AffiliateCommission[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches all affiliate commissions from DB and sorts them by specified sort column.
        /// If there are no affiliate commissions in DB it returns an empty array.
        /// </summary>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static AffiliateCommission[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches affiliate commission by specified id from DB.
        /// If the affiliate commission doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static AffiliateCommission Fetch(int id)
        {
            AffiliateCommission[] affiliateCommissions = Fetch(id, null, null, null, null, null, eSortColumn.None);

            if (affiliateCommissions.Length > 0)
            {
                return affiliateCommissions[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all affiliate commissions from DB by specified affiliate id.
        /// If there are no affiliate commissions in DB it returns an empty array.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <returns></returns>
        public static AffiliateCommission[] FetchByAffiliateID(int affiliateID)
        {
            return Fetch(null, affiliateID, null, null, null, null, eSortColumn.None);
        }

        public static AffiliateCommission[] FetchByAffiliateID(int affiliateID, eSortColumn sortColumn)
        {
            return Fetch(null, affiliateID, null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches all affiliate commissions from DB by specified payment history id.
        /// If there are no affiliate commissions in DB it returns an empty array.
        /// </summary>
        /// <param name="paymentHistoryID">The payment history ID.</param>
        /// <returns></returns>
        public static AffiliateCommission[] FetchByPaymentHistoryID(int paymentHistoryID)
        {
            return Fetch(null, null, paymentHistoryID, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches all affiliate commissions from DB by specified affiliate ID and username.
        /// If there are no affiliate commissions by specified  in DB it returns an empty array.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static AffiliateCommission[] Fetch(int affiliateID, string username)
        {
            return Fetch(null, affiliateID, null, username, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches affiliate commissions by specified arguments.
        /// It returns an empty array if there are no affiliate commissions in DB by specified arguments.
        /// If these arguments are null it returns all affiliate commissions from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <param name="paymentHistoryID">The payment history ID.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="timeStamp">The time stamp.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static AffiliateCommission[] Fetch(int? id, int? affiliateID, int? paymentHistoryID, string username, decimal? amount, DateTime? timeStamp, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn, "FetchAffiliateCommissions", id, affiliateID, paymentHistoryID, username, amount,
                                            timeStamp, sortColumn);

                List<AffiliateCommission> lAffiliateCommissions = new List<AffiliateCommission>();

                while (reader.Read())
                {
                    AffiliateCommission affiliateCommission = new AffiliateCommission();

                    affiliateCommission.id = (int) reader["ID"];
                    affiliateCommission.affiliateID = (int) reader["AffiliateID"];
                    affiliateCommission.paymentHistoryID = reader["PaymentHistoryID"] == DBNull.Value ? null : (int?) reader["PaymentHistoryID"];
                    affiliateCommission.username = (string) reader["Username"];
                    affiliateCommission.amount = (decimal) reader["Amount"];
                    affiliateCommission.timeStamp = (DateTime) reader["TimeStamp"];
                    affiliateCommission.notes = (string) reader["Notes"];

                    lAffiliateCommissions.Add(affiliateCommission);
                }

                return lAffiliateCommissions.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance in DB. If the ID of this instance is NULL it inserts new record in DB
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.ExecuteScalar(conn, "SaveAffiliateCommission", id, affiliateID, paymentHistoryID, username, amount,
                                            timeStamp, notes);

                if (!id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes affiliate commission from DB by specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            DeleteBy(id, null, null, null);
        }

        /// <summary>
        /// Deletes all affiliate commissions from DB by specified affiliate id.
        /// </summary>
        /// <param name="affiliateID">The affiliate ID.</param>
        public static void DeleteByAffiliateID(int affiliateID)
        {
            DeleteBy(null, affiliateID, null, null);
        }

        /// <summary>
        /// Deletes all affiliate commissions from DB by specified payment history id.
        /// </summary>
        /// <param name="paymentHistoryID">The payment history ID.</param>
        public static void DeleteByPaymentHistoryID(int paymentHistoryID)
        {
            DeleteBy(null, null, paymentHistoryID, null);
        }

        public static void DeleteByUsername(string username)
        {
            DeleteBy(null, null, null, username);
        }

        /// <summary>
        /// Deletes all affiliate commissions from DB by specified arguments.
        /// IF ALL ARGUMENTS ARE NULL IT DELETES ALL AFFILIATE COMMISSIONS.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="affiliateID">The affiliate ID.</param>
        /// <param name="paymentHistoryID">The payment history ID.</param>
        private static void DeleteBy(int? id, int? affiliateID, int? paymentHistoryID, string username)
        {
            using(SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteAffiliateCommission", id, affiliateID, paymentHistoryID, username);
            }
        }

        #endregion

        public static void ApplyCommission(string username, int? paymentHistoryID, decimal amount, string notes)
        {
            User user = null;

            try
            {
                user = User.Load(username);
            }
            catch (NotFoundException)
            {
                return;
            }

            // user is registered through affiliate
            if (user!= null && user.AffiliateID != null)
            {
                Affiliate affiliate = Affiliate.Fetch(user.AffiliateID.Value);

                if (affiliate != null)
                {
                    if (!affiliate.Recurrent)
                    {
                        AffiliateCommission[] affiliateCommission = Fetch(affiliate.ID, user.Username);

                        if (affiliateCommission.Length > 0) return;
                    }

                    decimal commission = 0;

                    if (affiliate.Percentage != null)
                    {
                        commission += amount * (affiliate.Percentage.Value / 100M);
                    }
                    
                    if (affiliate.FixedAmount != null)
                    {
                        commission += affiliate.FixedAmount.Value;
                    }

                    using (SqlConnection conn = Config.DB.Open())
                    {
                        SqlHelper.ExecuteNonQuery(conn, "ApplyAffiliateCommission", affiliate.ID, paymentHistoryID,
                                                  user.Username, commission, notes);
                    } 
                }
            }
        }
    }
}
