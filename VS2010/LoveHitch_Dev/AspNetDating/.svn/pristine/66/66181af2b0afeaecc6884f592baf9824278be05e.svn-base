using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class CommunityProfileApproval
    {
        #region fields

        private int? id = null;
        private string username;
        private string approvedBy;
        private bool approved = false;
        private DateTime date = DateTime.Now;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            Username,
            ApprovedBy,
            Date
        }

        #endregion

        #region Constructors

        private CommunityProfileApproval()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityPhotoApproval"/> class.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="approvedBy">The approved by.</param>
        public CommunityProfileApproval(string username, string approvedBy)
        {
            this.username = username;
            this.approvedBy = approvedBy;
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
        /// <value>The group ID.</value>
        public string Username
        {
            get { return username; }
        }

        /// <summary>
        /// Gets the approved by.
        /// The property is read-only.
        /// </summary>
        /// <value>The approved by.</value>
        public string ApprovedBy
        {
            get { return approvedBy; }
        }

        public bool Approved
        {
            get { return approved; }
            set { approved = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches community profile approval from DB.
        /// If there are no community profile approval in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static CommunityProfileApproval[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, sortColumn);
        }

        public static CommunityProfileApproval[] Fetch(string username)
        {
            return Fetch(null, username, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches community profile approval by the specified id from DB.
        /// If the community profile approval doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static CommunityProfileApproval Fetch(int id)
        {
            CommunityProfileApproval[] communityProfileApproval = Fetch(id, null, null, null, null, eSortColumn.None);

            if (communityProfileApproval.Length > 0)
            {
                return communityProfileApproval[0];
            }
            else
            {
                return null;
            }    
        }

        /// <summary>
        /// Fetches community profile approval by the specified photo id from DB.
        /// If the community profile approval doesn't exist returns NULL.
        /// </summary>
        /// <param name="approvedBy">The approved by.</param>
        /// <returns></returns>
        public static CommunityProfileApproval[] FetchByApprovedBy(string approvedBy)
        {
            return Fetch(null, null, approvedBy, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches community profile approval by specified parameters.
        /// It returns an empty array if there are no community profile approval in DB by specified arguments.
        /// If these arguments are null it returns all community profile approval from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="username">The username.</param>
        /// <param name="approvedBy">The approved by.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static CommunityProfileApproval[] Fetch(int? id, string username, string approvedBy,
                                                        DateTime? fromDate, DateTime? toDate, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchCommunityProfileApproval",
                                                               id, username, approvedBy, fromDate, toDate,
                                                               sortColumn);

                List<CommunityProfileApproval> lApprovals = new List<CommunityProfileApproval>();

                while (reader.Read())
                {
                    CommunityProfileApproval communityProfileApproval = new CommunityProfileApproval();

                    communityProfileApproval.id = (int)reader["ID"];
                    communityProfileApproval.username = (string)reader["Username"];
                    communityProfileApproval.approvedBy = (string)reader["ApprovedBy"];
                    communityProfileApproval.date = (DateTime)reader["Date"];
                    communityProfileApproval.approved = (bool)reader["Approved"];

                    lApprovals.Add(communityProfileApproval);
                }

                return lApprovals.ToArray();
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
                object result = SqlHelper.ExecuteScalar(conn, "SaveCommunityProfileApproval",
                                                        id, username, approvedBy, approved, date);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes community profile approval from DB by specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void Delete(int id)
        {
            Delete(id, null ,null);
        }

        /// <summary>
        /// Deletes all community profile approval for specified approved by.
        /// </summary>
        /// <param name="approvedBy">The approved by.</param>
        public static void DeleteByApprovedBy(string approvedBy)
        {
            Delete(null, null, approvedBy);
        }

        /// <summary>
        /// Deletes all community profile approval for specified username and approve by.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="approvedBy">The approved by.</param>
        public static void Delete(string username, string approvedBy)
        {
            Delete(null, username, approvedBy);
        }

        /// <summary>
        /// Deletes all community profile approvals for specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static void Delete(string username)
        {
            Delete(null, username, null);
        }

        /// <summary>
        /// Deletes community profile approvals from DB by specified parameters.
        /// Deletes all community profile approvals from DB if all arguments are NULL!
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="username">The username.</param>
        /// <param name="approvedBy">The approved by.</param>
        private static void Delete(int? id, string username, string approvedBy)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.ExecuteNonQuery(conn, "DeleteCommunityProfileApproval", id, username, approvedBy);
            }
        }

        #endregion
    }
}
