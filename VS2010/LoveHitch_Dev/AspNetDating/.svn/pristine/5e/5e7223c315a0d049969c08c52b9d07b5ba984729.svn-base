using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetDating.Model;

namespace AspNetDating.Classes
{
    public class EventComment
    {
        #region fields

        private int id;
        private int eventID;
        private string username;
        private DateTime date = DateTime.Now;
        private string comment;

        public enum eSortColumn
        {
            None,
            EventID,
            Username,
            Date
        }

        #endregion

        #region Constructors

        public EventComment()
        {
        }

        public EventComment(int eventID, string username)
        {
            this.eventID = eventID;
            this.username = username;
        }

        #endregion

        #region Properties

        public int ID
        {
            get { return id; }
        }

        public int EventID
        {
            get { return eventID; }
        }

        public string Username
        {
            get { return username; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        #endregion

        #region Methods

        public static EventComment[] Fetch()
        {
            return Fetch(null, null, null, null, eSortColumn.None);
        }

        public static EventComment[] Fetch(int eventID, eSortColumn sortColumn)
        {
            return Fetch(null, eventID, null, null, sortColumn);
        }

        private static EventComment[] Fetch(int? id, int? eventID, string username, int? numberOfComments, eSortColumn sortColumn)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var comments = from c in db.EventComments
                               where (!id.HasValue || c.ec_id == id)
                                     && (!eventID.HasValue || eventID == c.e_id)
                                     && (username == null || username == c.u_username)
                               select new EventComment
                                          {
                                              id = c.ec_id,
                                              eventID = c.e_id,
                                              username = c.u_username,
                                              comment = c.ec_comment,
                                              date = c.ec_date
                                          };
                
                if (sortColumn == eSortColumn.Date)
                {
                    comments = comments.OrderByDescending(c => c.date);
                }
                else if (sortColumn == eSortColumn.Username)
                {
                    comments = comments.OrderBy(c => c.username);
                }
                else if (sortColumn == eSortColumn.EventID)
                {
                    comments = comments.OrderBy(c => c.eventID);
                }

                if (numberOfComments.HasValue)
                {
                    comments = comments.Take(numberOfComments.Value);
                }
                
                return comments.ToArray();
            }
        }

        public void Save()
        {
            if (comment != null && comment.Length > 2000)
            {
                Global.Logger.LogWarning(
                    String.Format("EventComment {0} was not saved because it is more than 2000 chars", id));
                return;
            }

            using (var db = new AspNetDatingDataContext())
            {
                var eventComment = new Model.EventComment()
                {
                    ec_id = id,
                    e_id = eventID,
                    u_username = username,
                    ec_date = date,
                    ec_comment = comment
                };
                if (id == 0)
                    db.EventComments.InsertOnSubmit(eventComment);
                else
                {
                    db.EventComments.Attach(eventComment, true);
                }

                db.SubmitChanges();

                if (id == 0)
                    id = eventComment.ec_id;
            }
        }

        public static void Delete(int id)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var eventComment = db.EventComments.Single(c => c.ec_id == id);
                db.EventComments.DeleteOnSubmit(eventComment);
                db.SubmitChanges();
            }
        }

        public static void DeleteByEventID(int id)
        {
            using (var db = new AspNetDatingDataContext())
            {
                var comments = db.EventComments.Where(c => c.e_id == id);
                db.EventComments.DeleteAllOnSubmit(comments);
                db.SubmitChanges();
            }
        }

        #endregion
    }
}
