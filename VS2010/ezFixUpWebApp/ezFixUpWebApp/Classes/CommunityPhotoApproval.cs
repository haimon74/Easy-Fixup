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
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class CommunityPhotoApproval
    {
        #region fields

        private int? id = null;
        private int photoID;
        private string username;
        private bool approved = false;
        private DateTime date = DateTime.Now;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            Username,
            Date
        }

        #endregion

        #region Constructors

        private CommunityPhotoApproval()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="CommunityPhotoApproval"/> class.
        /// </summary>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="username">The username.</param>
        public CommunityPhotoApproval(int photoID, string username)
        {
            this.photoID = photoID;
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
        /// Gets the photo ID.
        /// The property is read-only.
        /// </summary>
        /// <value>The group ID.</value>
        public int PhotoID
        {
            get { return photoID; }
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
        /// Fetches community photo approval from DB.
        /// If there are no community photo approval in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static CommunityPhotoApproval[] Fetch(eSortColumn sortColumn)
        {
            return Fetch(null, null, null, null, null, sortColumn);
        }

        public static CommunityPhotoApproval[] Fetch(string username)
        {
            return Fetch(null, null, username, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches community photo approval by the specified id from DB. If the community photo approval doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static CommunityPhotoApproval Fetch(int id)
        {
            CommunityPhotoApproval[] communityPhotoApproval = Fetch(id, null, null, null, null, eSortColumn.None);

            if (communityPhotoApproval.Length > 0)
            {
                return communityPhotoApproval[0];
            }
            else
            {
                return null;
            }    
        }

        /// <summary>
        /// Fetches community photo approval by the specified photo id from DB. If the community photo approval doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static CommunityPhotoApproval[] FetchByPhotoID(int id)
        {
            return Fetch(null, id, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches community photo approval by specified parameters.
        /// It returns an empty array if there are no community photo approval in DB by specified arguments.
        /// If these arguments are null it returns all community photo approval from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static CommunityPhotoApproval[] Fetch(int? id, int? photoID, string username,
                                                        DateTime? fromDate, DateTime? toDate, eSortColumn sortColumn)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var photoApprovals = from cpa in db.CommunityPhotoApprovals
                                     where (!id.HasValue || id == cpa.cpa_id)
                                           && (!photoID.HasValue || photoID == cpa.p_id)
                                           && (username == null || username == cpa.u_username)
                                           && (!fromDate.HasValue || cpa.cpa_timestamp >= fromDate)
                                           && (!toDate.HasValue || cpa.cpa_timestamp <= toDate)
                                     select new CommunityPhotoApproval
                                                {
                                                    id = cpa.cpa_id,
                                                    username = cpa.u_username,
                                                    photoID = cpa.p_id,
                                                    approved = cpa.cpa_approved,
                                                    date = cpa.cpa_timestamp
                                                };
                switch (sortColumn)
                {
                    case eSortColumn.None:
                        break;
                    case eSortColumn.Username:
                        photoApprovals = photoApprovals.OrderBy(pa => pa.username);
                        break;
                    case eSortColumn.Date:
                        photoApprovals = photoApprovals.OrderByDescending(pa => pa.date);
                        break;
                    default:
                        break;
                }

                return photoApprovals.ToArray();
            }

            ////using (var conn = Config.DB.Open())
            //{
            //    var reader = SqlHelper.GetDB().ExecuteReader( "FetchCommunityPhotoApproval",
            //                                                   id, photoID, username, fromDate, toDate,
            //                                                   sortColumn);

            //    List<CommunityPhotoApproval> lGroupPost = new List<CommunityPhotoApproval>();

            //    while (reader.Read())
            //    {
            //        CommunityPhotoApproval communityPhotoApproval = new CommunityPhotoApproval();

            //        communityPhotoApproval.id = (int)reader["ID"];
            //        communityPhotoApproval.photoID = (int)reader["PhotoID"];
            //        communityPhotoApproval.username = (string)reader["Username"];
            //        communityPhotoApproval.date = (DateTime)reader["Date"];
            //        communityPhotoApproval.approved = (bool) reader["Approved"];

            //        lGroupPost.Add(communityPhotoApproval);
            //    }

            //    return lGroupPost.ToArray();
            //}
        }

        /// <summary>
        /// Saves this instance in DB. If the ID of this instance is NULL it inserts new record in DB
        /// otherwise updates the record.
        /// </summary>
        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveCommunityPhotoApproval",
                                                        id, photoID, username, approved, date);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        /// <summary>
        /// Deletes community photo approval from DB by specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static void Delete(int id)
        {
            Delete(id, null ,null);
        }

        /// <summary>
        /// Deletes all community photo approval for specified photo id.
        /// </summary>
        /// <param name="photoID">The photo ID.</param>
        public static void DeleteByPhotoID(int photoID)
        {
            Delete(null, photoID, null);
        }

        /// <summary>
        /// Deletes all community photo approval for specified photo ID and username.
        /// </summary>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="username">The username.</param>
        public static void Delete(int photoID, string username)
        {
            Delete(null, photoID, username);
        }

        /// <summary>
        /// Deletes all community photo approvals for specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static void Delete(string username)
        {
            Delete(null, null, username);
        }

        /// <summary>
        /// Deletes community photo approvals from DB by specified parameters.
        /// Deletes all community photo approvals from DB if all arguments are NULL!
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="photoID">The photo ID.</param>
        /// <param name="username">The username.</param>
        private static void Delete(int? id, int? photoID, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteCommunityPhotoApproval", id, photoID, username);
            }
        }

        #endregion
    }
}
