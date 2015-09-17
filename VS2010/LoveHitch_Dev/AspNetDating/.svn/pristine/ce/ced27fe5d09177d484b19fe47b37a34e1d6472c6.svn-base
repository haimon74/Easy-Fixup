using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class Ecard
    {
        #region fields

        private int? id = null;
        private int ecardTypeID;
        private string fromUsername = null;
        private string toUsername = null;
        private DateTime date = DateTime.Now;
        private string message = String.Empty;
        private bool deletedByFromUser = false;
        private bool deletedByToUser = false;
        private bool isOpened = false;
        private eSortColumn sortColumn;

        /// <summary>
        /// Specifies the colomn on which to sort.
        /// </summary>
        public enum eSortColumn
        {
            None,
            Date
        }

        public enum eFolder
        {
            None = 0,
            Received = 1,
            Sent = 2
        }

        #endregion

        #region Constructors

        private Ecard()
        {
        }

        public Ecard(int ecardTypeId, string fromUsername, string toUsername)
        {
            this.ecardTypeID = ecardTypeId;
            this.fromUsername = fromUsername;
            this.toUsername = toUsername;
        }

        public Ecard(int ecardTypeId, string fromUsername, string toUsername, string message)
        {
            this.ecardTypeID = ecardTypeId;
            this.fromUsername = fromUsername;
            this.toUsername = toUsername;
            this.message = message;
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
                else
                {
                    throw new Exception("The field ID is not set!");
                }
            }
        }

        public int EcardTypeID
        {
            get { return ecardTypeID; }
        }

        public string FromUsername
        {
            get { return fromUsername; }
        }

        public string ToUsername
        {
            get { return toUsername; }
        }

        public DateTime Date
        {
            get { return date; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public bool DeletedByFromUser
        {
            get { return deletedByFromUser; }
        }

        public bool DeletedByToUser
        {
            get { return deletedByToUser; }
        }

        public bool IsOpened
        {
            get { return isOpened; }
            set { isOpened = value; }
        }

        public eSortColumn SortColumn
        {
            get { return sortColumn; }
            set { sortColumn = value; }
        }

        #endregion

        #region Methods

        public static Ecard[] Fetch()
        {
            return Fetch(null, null, null, null, null, eSortColumn.None);
        }

        public static Ecard Fetch(int id)
        {
            Ecard[] ecards = Fetch(id, null, null, null, null, eSortColumn.None);

            if (ecards.Length > 0)
            {
                return ecards[0];
            }

            return null;
        }

        private static Ecard[] Fetch(int? id, int? ecardTypeID, string fromUsername, string toUsername,
                                        bool? isOpened, eSortColumn sortColumn)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchEcards",
                                                               id,
                                                               ecardTypeID,
                                                               fromUsername,
                                                               toUsername,
                                                               isOpened,
                                                               sortColumn);

                List<Ecard> lEcards = new List<Ecard>();

                while (reader.Read())
                {
                    Ecard ecard = new Ecard();

                    ecard.id = (int)reader["ID"];
                    ecard.ecardTypeID= (int) reader["EcardTypeID"];
                    ecard.fromUsername = (string) reader["FromUsername"];
                    ecard.toUsername = (string) reader["ToUsername"];
                    ecard.date = (DateTime) reader["Date"];

                    ecard.message = (string) reader["Message"];
                    ecard.deletedByFromUser = (bool) reader["DeletedByFromUser"];
                    ecard.deletedByToUser = (bool) reader["DeletedByToUser"];
                    ecard.isOpened = (bool) reader["IsOpened"];

                    lEcards.Add(ecard);
                }

                return lEcards.ToArray();
            }
        }

        public static Ecard[] FetchUnread(string username)
        {
            return Fetch(null, null, null, username, false, eSortColumn.None);
        }

        public static Ecard[] FetchUnreadEcards(string username)
        {
            return FetchReceivedEcards(username).Where(x => x.IsOpened == false).ToArray();
        }
        
        public static Ecard[] FetchUnread(string username, EcardType.eType type)
        {
            return FetchReceived(username, type).Where(x=> x.IsOpened==false).ToArray();
        }

        public static Ecard[] FetchReceived(string username)
        {
            return Fetch(null, null, null, username, null, eSortColumn.Date);
        }

        public static Ecard[] FetchReceivedEcards(string username)
        {
            var images = FetchReceived(username, EcardType.eType.Image).ToList();
            var animations = FetchReceived(username, EcardType.eType.Flash).ToList();
            return (new List<Ecard>().Concat(animations).Concat(images)).ToArray();
        }

        public static Ecard[] FetchReceived(string username, EcardType.eType type)
        {
            return FetchReceived(username)
                .Where(x => EcardType.Fetch(x.EcardTypeID).Type == type).ToArray();
        }

        public static Ecard[] FetchSentEcards(string username)
        {
            var images = FetchSent(username, EcardType.eType.Image).ToList();
            var animations = FetchSent(username, EcardType.eType.Flash).ToList();
            return (new List<Ecard>().Concat(animations).Concat(images)).ToArray();
        }

        public static Ecard[] FetchSentInLast24Hours(string username, EcardType.eType type)
        {
            return FetchSent(username)
                .Where(x => x.Date > DateTime.Now.AddHours(-24) && 
                            EcardType.Fetch(x.EcardTypeID).Type == type)
                .ToArray();
        }

        public static Ecard[] FetchSent(string username, EcardType.eType type)
        {
            return FetchSent(username)
            .Where(x => EcardType.Fetch(x.EcardTypeID).Type == type).ToArray();
        }

        public static Ecard[] FetchSent(string username)
        {
            return Fetch(null, null, username, null, null, eSortColumn.Date);
        }

        public void Save()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveEcard",
                                                        id,
                                                        ecardTypeID,
                                                        fromUsername,
                                                        toUsername,
                                                        date,
                                                        message,
                                                        deletedByFromUser,
                                                        deletedByToUser,
                                                        isOpened);
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        private void DeleteBy(bool fromUser /*sent ecard*/, bool toUser /*received ecard*/)
        {
            if (id > 0)
            {
                using (SqlConnection conn = Config.DB.Open())
                {
                    SqlHelper.ExecuteNonQuery(conn,
                                              "DeleteEcard", id, fromUser, toUser);
                }
            }
        }

        public void DeleteFromReceivedEcards()
        {
            DeleteBy(false, true);
        }

        public void DeleteFromSentEcards()
        {
            DeleteBy(true, false);
        }

        #endregion
    }
}
