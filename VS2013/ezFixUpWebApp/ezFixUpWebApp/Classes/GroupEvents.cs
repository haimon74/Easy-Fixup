using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Caching;

namespace ezFixUp.Classes
{
    /// <summary>
    /// This class handles the group events
    /// </summary>
    [Serializable]
    public class GroupEvent
    {
        #region fields

        private int? id = null;
        private int groupID;
        private string username;
        private string title;
        private string description;
        private Image image;
        private DateTime date = DateTime.Now;
        private string location = String.Empty;

        /// <summary>
        /// Column to sort by
        /// </summary>
        public enum eSortColumn
        {
            /// <summary>
            /// Doesn't sort
            /// </summary>
            None,
            /// <summary>
            /// Sorts by group id
            /// </summary>
            GroupID,
            /// <summary>
            /// Sorts by username
            /// </summary>
            Username,
            /// <summary>
            /// Sorts by date
            /// </summary>
            Date
        }

        /// <summary>
        /// Specifies the action which will be perform.
        /// </summary>
        private enum eAction
        {
            Assign = 1,
            Remove = 2
        }

        #endregion

        #region Constructors

        private GroupEvent()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupEvent"/> class.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        public GroupEvent(int groupID, string username)
        {
            this.groupID = groupID;
            this.username = username;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the ID.
        /// </summary>
        /// <value>The ID.</value>
        public int? ID
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
        /// Gets the group ID.
        /// </summary>
        /// <value>The group ID.</value>
        public int GroupID
        {
            get { return groupID; }
        }

        /// <summary>
        /// Gets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        /// <value>The image.</value>
        public Image Image
        {
            get { return image; }
            set { image = value; }
        }

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Location
        {
            get { return location; }
            set { location = value; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fetches all group events from DB.
        /// If there are no group events in DB it returns an empty array.
        /// </summary>
        /// <returns></returns>
        public static GroupEvent[] Fetch()
        {
            return Fetch(null, null, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches group event by specified id from DB.
        /// If the group event doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static GroupEvent Fetch(int id)
        {
            GroupEvent[] groupEvents = Fetch(id, null, null, null, null, null, eSortColumn.None);

            if (groupEvents.Length > 0)
            {
                return groupEvents[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Fetches all group events by specified username from DB.
        /// If the group doesn't exist returns NULL.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static GroupEvent[] Fetch(string username)
        {
            return Fetch(null, null, username, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches group events by the specified group id from DB.
        /// If there are no group events in DB for the specified group id it returns an empty array.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static GroupEvent[] FetchByGroupID(int groupID)
        {
            return Fetch(null, groupID, null, null, null, null, eSortColumn.None);
        }

        /// <summary>
        /// Fetches the specified group ID.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static GroupEvent[] Fetch(int groupID, eSortColumn sortColumn)
        {
            return Fetch(null, groupID, null, null, null, null, sortColumn);
        }

        /// <summary>
        /// Fetches the specified group ID.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="date">The date.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static GroupEvent[] Fetch(int groupID, DateTime date, eSortColumn sortColumn)
        {
            return Fetch(null, groupID, null, date.Date, date.AddHours(23).AddMinutes(59).AddSeconds(59), null, sortColumn);
        }

        /// <summary>
        /// Fetches the specified number of group events for the specified group
        /// and sorts them by specified sort column.
        /// If there are no group events in DB it returns an empty array.
        /// </summary>
        /// <param name="numberOfGroupEvents">The number of group events.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static GroupEvent[] Fetch(int numberOfGroupEvents, int groupID, DateTime? fromDate, DateTime? toDate, eSortColumn sortColumn)
        {
            string cacheKey = String.Format("GroupEvent_FetchByGroupID_{0}", groupID);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null)
            {
                return HttpContext.Current.Cache[cacheKey] as GroupEvent[];
            }

            GroupEvent[] groupEvents = Fetch(null, groupID, null, fromDate, toDate, numberOfGroupEvents, sortColumn);

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, groupEvents, null, DateTime.Now.AddMinutes(30),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }

            return groupEvents;
        }

        /// <summary>
        /// Fetches group events by specified arguments.
        /// It returns an empty array if there are no group events in DB by specified arguments.
        /// If these arguments are null it returns all group events from DB.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="numberOfGroupEvents">The number of group events.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        private static GroupEvent[] Fetch(int? id, int? groupID, string username, DateTime? fromDate, DateTime? toDate, int? numberOfGroupEvents, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                List <GroupEvent> lGroupEvents = new List<GroupEvent>();

                using (
                    var reader = SqlHelper.GetDB()
                                          .ExecuteReader("FetchGroupEvents", id, groupID, username, fromDate, toDate,
                                                         numberOfGroupEvents, sortColumn))
                {

                    while (reader.Read())
                    {
                        GroupEvent groupEvent = new GroupEvent();

                        groupEvent.id = (int) reader["ID"];
                        groupEvent.groupID = (int) reader["GroupID"];
                        groupEvent.username = (string) reader["Username"];
                        groupEvent.title = (string) reader["Title"];
                        groupEvent.description = (string) reader["Description"];
                        groupEvent.date = (DateTime) reader["Date"];
                        groupEvent.location = (string) reader["Location"];

                        lGroupEvents.Add(groupEvent);
                    }
                    reader.Close();
                }

                return lGroupEvents.ToArray();
            }
        }

        /// <summary>
        /// Saves this instance into DB.
        /// </summary>
        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result =
                    SqlHelper.GetDB().ExecuteScalar( "SaveGroupEvent",
                                            id, groupID, username, title, description, date, location);

                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }

                string cacheKey = String.Format("GroupEvent_FetchByGroupID_{0}", groupID);
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }
        }

        /// <summary>
        /// Deletes group event from DB by the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            GroupEvent groupEvent = Fetch(id);
            if (groupEvent != null)
            {
                string cacheKey = String.Format("GroupEvent_FetchByGroupID_{0}", groupEvent.GroupID);
                if (HttpContext.Current != null)
                {
                    HttpContext.Current.Cache.Remove(cacheKey);
                }
            }

            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteGroupEvent", id);
            }
        }

        /// <summary>
        /// Searches the specified group ID.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="username">The username.</param>
        /// <param name="title">The title.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <param name="sortColumn">The sort column.</param>
        /// <returns></returns>
        public static int[] Search(int? groupID, string username, string title,
                                    string keyword, DateTime? fromDate, DateTime? toDate, string groupMember, int? numberOfGroupEvents, eSortColumn sortColumn)
        {
            //using (var conn = Config.DB.Open())
            {
                List<int> lGroupPhotosIDs = new List<int>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("SearchGroupEvents",
                                                                    groupID, username, title, keyword, fromDate, toDate,
                                                                    groupMember, numberOfGroupEvents, sortColumn))
                {

                    while (reader.Read())
                    {
                        lGroupPhotosIDs.Add((int) reader["ID"]);
                    }
                    reader.Close();
                }
                return lGroupPhotosIDs.ToArray();
            }
        }

        /// <summary>
        /// Counts the specified group ID.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <returns></returns>
        public static int Count (int groupID)
        {
            return CountBy(groupID, null, null);
        }

        /// <summary>
        /// Counts the specified group ID.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        public static int Count(int groupID, DateTime date)
        {
            return CountBy(groupID, date.Date, date.AddHours(23).AddMinutes(59).AddSeconds(59));
        }

        /// <summary>
        /// Counts the specified group ID.
        /// </summary>
        /// <param name="groupID">The group ID.</param>
        /// <param name="fromDate">From date.</param>
        /// <param name="toDate">To date.</param>
        /// <returns></returns>
        public static int Count(int groupID, DateTime? fromDate, DateTime? toDate)
        {
            return CountBy(groupID, fromDate, toDate);
        }

        private static int CountBy(int? groupID, DateTime? fromDate, DateTime? toDate)
        {
            //using (var conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.GetDB().ExecuteScalar( "FetchGroupEventsCount", groupID, fromDate, toDate));
            }
        }

        /// <summary>
        /// Fetches the number of upcoming events.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static Hashtable FetchNumberOfUpcomingEvents(string username)
        {
            //using (var conn = Config.DB.Open())
            {
                Hashtable hash = new Hashtable();

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchNumberOfUpcomingEvents", username))
                {

                    while (reader.Read())
                    {
                        hash.Add(reader["GroupID"], new object[] {reader["Name"], reader["Count"]});
                    }
                    reader.Close();
                }
                return hash;
            }
        }

        /// <summary>
        /// Loads the image for the specified group event id.
        /// Returns 'PHOTO NOT AVAILABLE' image if the group event doesn't have a image.
        /// Returns NULL if the specified group event doesn't exist.
        /// </summary>
        /// <param name="groupEventID">The group ID.</param>
        /// <returns>Image object.</returns>
        public static Image LoadImage(int groupEventID)
        {
            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchGroupEventImage", groupEventID))
                {

                    if (reader.Read())
                    {
                        if (reader["Image"] is DBNull)
                        {
                            reader.Close();
                            return
                                Image.FromFile(HttpContext.Current.Server.MapPath("~/Images") +
                                               "/no_photo_group_event.png");
                        }
                        else
                        {
                            byte[] buffer = (byte[]) reader["Image"];
                            reader.Close();
                            MemoryStream imageStream = new MemoryStream(buffer);
                            return Image.FromStream(imageStream);
                        }
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Saves the image for the specified group event.
        /// </summary>
        /// <param name="groupEventID">The goup ID.</param>
        /// <param name="image">The image.</param>
        public static void SaveImage(int groupEventID, Image image)
        {
            if (image.Width > Config.Groups.GroupEventImageMaxWidth || image.Height > Config.Groups.GroupEventImageMaxHeight)
            {
                image = Photo.ResizeImage(image, Config.Groups.GroupEventImageMaxWidth, Config.Groups.GroupEventImageMaxHeight);
            }

            MemoryStream imageStream = new MemoryStream();
            image.Save(imageStream, ImageFormat.Jpeg);
            imageStream.Position = 0;
            BinaryReader reader = new BinaryReader(imageStream);
            byte[] bytesImage = reader.ReadBytes((int)imageStream.Length);

            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "SaveGroupEventImage", groupEventID, bytesImage);
            }
        }

        /// <summary>
        /// Deletes the image for the specified group event.
        /// </summary>
        /// <param name="groupEventID">The goup ID.</param>
        public static void DeleteImage(int groupEventID)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "DeleteGroupEventImage", groupEventID);
            }
        }

        /// <summary>
        /// Gets the attenders.
        /// </summary>
        /// <param name="groupEventID">The group event ID.</param>
        /// <returns></returns>
        public static string[] GetAttenders(int groupEventID)
        {
            return FetchAttenders(groupEventID, null);
        }

        /// <summary>
        /// Determines whether the specified user is attending to the event.
        /// </summary>
        /// <param name="groupEventID">The group event ID.</param>
        /// <param name="username">The username.</param>
        /// <returns>
        /// 	<c>true</c> if the specified user is attending; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsAttender(int groupEventID, string username)
        {
            if (FetchAttenders(groupEventID, username).Length > 0) return true;
            else return false;
        }

        private static string[] FetchAttenders(int? groupEventID, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                List<string> lAttenders = new List<string>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchGroupEventsAttenders", 
                                                            groupEventID, username))
                {

                    while (reader.Read())
                    {
                        lAttenders.Add((string) reader["Username"]);
                    }
                    reader.Close();
                }
                return lAttenders.ToArray();
            }
        }

        /// <summary>
        /// Sets the attender.
        /// </summary>
        /// <param name="groupEventID">The group event ID.</param>
        /// <param name="username">The username.</param>
        public static void SetAttender(int groupEventID, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "AssignRemoveAttenderToFromGroupEvent", groupEventID, username,
                                          eAction.Assign);
            }
        }

        /// <summary>
        /// Deletes the attender.
        /// </summary>
        /// <param name="groupEventID">The group event ID.</param>
        /// <param name="username">The username.</param>
        public static void DeleteAttender(int groupEventID, string username)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "AssignRemoveAttenderToFromGroupEvent", groupEventID, username,
                                          eAction.Remove);
            }
        }

        /// <summary>
        /// Gets the friends attenders.
        /// </summary>
        /// <param name="groupEventID">The group event ID.</param>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static string[] GetFriendsAttenders(int groupEventID, string username)
        {
            List<string> lFriends = new List<string>();
            string[] friendsUsernames = Classes.User.FetchMutuallyFriends(username);

            foreach (string friendUsername in friendsUsernames)
            {
                if (IsAttender(groupEventID, friendUsername))
                {
                    lFriends.Add(friendUsername);
                }
            }

            return lFriends.ToArray();
        }

        #endregion
    }
}
