using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class GroupEventsComment
    {
        #region fields

        private int? id = null;
        private int groupEventID;
        private string username;
        private string comment = String.Empty;
        private DateTime date = DateTime.Now;

        public enum eSortColumn
        {
            None,
            Username,
            Date
        }
 
        #endregion

        #region Constructors

        private GroupEventsComment()
        {
        }

        public GroupEventsComment(int groupEventID, string username)
        {
            this.groupEventID = groupEventID;
            this.username = username;
        }

        #endregion

        #region Properties

        public int? Id
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

        public int GroupEventID
        {
            get { return groupEventID; }
        }

        public string Username
        {
            get { return username; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group event comments from DB.
        /// If there are no group event comments in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupEventsComment[] Fetch()
        {
            return Fetch(null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches group event comment by specified id from DB. If the group event comment doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupEventsComment Fetch(int id)
        {
            GroupEventsComment[] groupEventsComments = Fetch(id, null, null, eSortColumn.None);

            if (groupEventsComments.Length > 0)
            {
                return groupEventsComments[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches group event comments by specified group event id.
        /// If there are no group event comments in DB for the specified group event id it returns an empty array.
        /// </summary>
        /// <param name="groupEventID">The group event ID.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static GroupEventsComment[] FetchByGroupEventID(int groupEventID, eSortColumn sortColumn)
        {
            return Fetch(null, groupEventID, null, sortColumn);
        }

        /// <summary>
        /// Fetches group event comments by specified username.
        /// If there are no group event comments in DB for the specified username it returns an empty array.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupEventsComment[] Fetch(string username)
        {
            return Fetch(null, null, username, eSortColumn.None);
        }

        /// <summary>
        /// Fetches group event comments by specified arguments.
        /// It returns an empty array if there are no group event comments in DB by specified arguments.
        /// If these arguments are null it returns all group event comments from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupEventID">The group event ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static GroupEventsComment[] Fetch(int? id, int? groupEventID, string username, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                List <GroupEventsComment> lGroupEventsComments = new List<GroupEventsComment>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchGroupEventsComments",
                                                                    id, groupEventID, username, sortColumn))
                {

                    while (reader.Read())
                    {
                        GroupEventsComment groupEventsComment = new GroupEventsComment();

                        groupEventsComment.id = (int) reader["ID"];
                        groupEventsComment.groupEventID = (int) reader["GroupEventID"];
                        groupEventsComment.username = (string) reader["Username"];
                        groupEventsComment.comment = (string) reader["Comment"];
                        groupEventsComment.date = (DateTime) reader["Date"];

                        lGroupEventsComments.Add(groupEventsComment);
                    }
                    reader.Close();
                }

                return lGroupEventsComments.ToArray();
            }
        }

        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.GetDB().ExecuteScalar( "SaveGroupEventsComments", id, groupEventID, username, comment, date);

                if (id.HasValue)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        public static void Delete(int id)
        {
            DeleteBy(id, null, null);
        }

        public static void Delete(string username)
        {
            DeleteBy(null, null, username);
        }

        private static void DeleteBy(int? id, int? groupEventID, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteGroupEventsComments", id, groupEventID, username);
            }
        }

        #endregion
    }
}
