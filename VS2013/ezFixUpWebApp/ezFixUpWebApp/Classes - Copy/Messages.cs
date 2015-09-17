/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using ezFixUp.Data.LoveHitchParallel;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Data
{
    /// <summary>
    /// This class handles the user messages
    /// </summary>
    [Serializable]
    public class Message
    {
        #region Properties

        private int id;
        private int msgTypeId;
        
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>The id.</value>
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int MessageTypeId
        {
            get { return msgTypeId; }
            set { msgTypeId = value; }
        }

        /// <summary>
        /// The username of the sender
        /// </summary>
        public string fromUsername;

        private User fromUser;

        /// <summary>
        /// Gets or sets from user.
        /// </summary>
        /// <value>From user.</value>
        [XmlIgnore]
        public User FromUser
        {
            get
            {
                try
                {
                    if (fromUser == null)
                        fromUser = User.Load(fromUsername);
                }
                catch (NotFoundException err)
                {
                    ExceptionLogger.Log("Message.FromUser", err);
                    throw;
                }

                return fromUser;
            }
            set
            {
                fromUser = value;
                fromUsername = value.Username;
            }
        }

        /// <summary>
        /// The username of the recipient
        /// </summary>
        public string toUsername;

        private User toUser;

        /// <summary>
        /// Gets or sets to user.
        /// </summary>
        /// <value>To user.</value>
        [XmlIgnore]
        public User ToUser
        {
            get
            {
                try
                {
                    if (toUser == null)
                        toUser = User.Load(toUsername);
                }catch(NotFoundException err)
                {
                    ExceptionLogger.Log("Message.ToUser", err);
                    throw;
                }
                
                return toUser;
            }
            set
            {
                toUser = value;
                toUsername = value.Username;
            }
        }

        /// <summary>
        /// The username of the about enquired
        /// </summary>
        public string aboutUsername;

        private User aboutUser;

        /// <summary>
        /// Gets or sets about user enquired.
        /// </summary>
        /// <value>About user.</value>
        [XmlIgnore]
        public User AboutUser
        {
            get
            {
                try
                {
                    if (aboutUser == null)
                        aboutUser = User.Load(aboutUsername);
                }catch(NotFoundException err)
                {
                    ExceptionLogger.Log("Message.AboutUser", err);
                    throw;
                }
                
                return aboutUser;
            }
            set
            {
                aboutUser = value;
                aboutUsername = value.Username;
            }
        }

        public enum eMessageType
        {
            Message = 1,
            Gift = 2,
            Wink = 3,
            Invitation = 4,
            Matchmaking = 5,
            MutualFriendAskAbout = 6,
            MutualFriendAnswerAbout = 7,
            RequestMatchmaking = 8
        } ;
        /// <summary>
        /// The message folder
        /// </summary>
        public enum eFolder
        {
            /// <summary>
            /// No folder
            /// </summary>
            None = 0,
            /// <summary>
            /// The Inbox
            /// </summary>
            Inbox = 1,
            /// <summary>
            /// The Outbox
            /// </summary>
            Outbox = 2,
            /// <summary>
            /// The Trash
            /// </summary>
            Trash = 3,

            /// <summary>
            /// The message is deleted
            /// </summary>
            Deleted = int.MaxValue
        }

        private eFolder fromFolder = eFolder.Outbox;

        /// <summary>
        /// Gets or sets from folder.
        /// </summary>
        /// <value>From folder.</value>
        public eFolder FromFolder
        {
            get { return fromFolder; }
            set
            {
                fromFolder = value;

                if (id > 0)
                {
                    using (SqlConnection conn = Config.DB.Open())
                    {
                        SqlHelper.GetDB().ExecuteNonQuery( "UpdateMessage",
                            id, null, (int)fromFolder, null, null, null);
                    }
                }
            }
        }

        private eFolder toFolder = eFolder.Inbox;

        /// <summary>
        /// Gets or sets to folder.
        /// </summary>
        /// <value>To folder.</value>
        public eFolder ToFolder
        {
            get { return toFolder; }
            set
            {
                toFolder = value;

                if (id > 0)
                {
                    using (SqlConnection conn = Config.DB.Open())
                    {
                        SqlHelper.GetDB().ExecuteNonQuery( "UpdateMessage", 
                            id, null, null, (int) toFolder, null, null);
                    }
                }
            }
        }

        private string subject;

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        public string Subject
        {
            get { return subject; }
            set
            {
                subject = value;

                if (value.Length > 50)
                    subject = subject.Substring(0, 50);
            }
        }

        private string body;

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        /// <value>The body.</value>
        public string Body
        {
            get { return body; }
            set
            {
                body = value;

                if (value.Length > 3500)
                    body = body.Substring(0, 3500);
            }
        }

        private DateTime timestamp = DateTime.Now;

        /// <summary>
        /// Gets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp
        {
            get { return timestamp; }
        }

        private int repliedTo;

        /// <summary>
        /// Gets or sets the replied to.
        /// </summary>
        /// <value>The replied to.</value>
        public int RepliedTo
        {
            get { return repliedTo; }
            set { repliedTo = value; }
        }

        private bool isRead;

        /// <summary>
        /// Gets or sets a value indicating whether this instance is read.
        /// </summary>
        /// <value><c>true</c> if this instance is read; otherwise, <c>false</c>.</value>
        public bool IsRead
        {
            get { return isRead; }
            set
            {
                isRead = value;

                if (id > 0)
                {
                    using (SqlConnection conn = Config.DB.Open())
                    {
                        SqlHelper.GetDB().ExecuteNonQuery( "UpdateMessage",
                            id, null, null, null, isRead, null);
                    }
                }
            }
        }

        private bool pendingApproval = false;

        /// <summary>
        /// Gets a value indicating whether [pending approval].
        /// </summary>
        /// <value><c>true</c> if [pending approval]; otherwise, <c>false</c>.</value>
        public bool PendingApproval
        {
            get { return pendingApproval; }
        }

        #endregion

        private Message()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        public Message(string fromUsername, string toUsername)
        {
            this.fromUsername = fromUsername;
            this.toUsername = toUsername;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="aboutUsername">About username.</param>
        public Message(string fromUsername, string toUsername, string aboutUsername)
        {
            this.fromUsername = fromUsername;
            this.toUsername = toUsername;
            this.aboutUsername = aboutUsername;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="toUser">To user.</param>
        public Message(User fromUser, User toUser)
        {
            FromUser = fromUser;
            ToUser = toUser;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Message"/> class.
        /// </summary>
        /// <param name="fromUser">From user.</param>
        /// <param name="toUser">To user.</param>
        /// <param name="aboutUser">About user.</param>
        public Message(User fromUser, User toUser, User aboutUser)
        {
            FromUser = fromUser;
            ToUser = toUser;
            AboutUser = aboutUser;
        }

        /// <summary>
        /// Searches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="fromUsername">From username.</param>
        /// <param name="fromFolder">From folder.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="toFolder">To folder.</param>
        /// <param name="inPastDays">The in past days.</param>
        /// <param name="filterApproved">if set to <c>true</c> [filter approved].</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <param name="newMessages">if set to <c>true</c> [new messages].</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="msgTypeId">to differ metween msg/gift/wink</param>
        /// <returns></returns>
        public static int[] Search(int id, string fromUsername, eFolder fromFolder, string toUsername,
                                   eFolder toFolder, int inPastDays, bool filterApproved, bool approved,
                                   bool newMessages, string keyword, int? msgTypeId)
        {
            return Search(id, fromUsername, fromFolder, toUsername, toFolder, inPastDays, 
                filterApproved, approved, newMessages, null, keyword, msgTypeId);
        }

        /// <summary>
        /// Searches the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="fromUsername">From username.</param>
        /// <param name="fromFolder">From folder.</param>
        /// <param name="toUsername">To username.</param>
        /// <param name="toFolder">To folder.</param>
        /// <param name="inPastDays">The in past days.</param>
        /// <param name="filterApproved">if set to <c>true</c> [filter approved].</param>
        /// <param name="approved">if set to <c>true</c> [approved].</param>
        /// <param name="newMessages">if set to <c>true</c> [new messages].</param>
        /// <param name="isRead">The is read.</param>
        /// <param name="keyword">The keyword.</param>
        /// <param name="msgTypeId">The Message Type ID (1=msg, 2=wink, 3=gift)</param>
        /// <returns></returns>
        public static int[] Search(int id, string fromUsername, eFolder fromFolder, string toUsername, 
            eFolder toFolder, int inPastDays, bool filterApproved, bool approved, bool newMessages, 
            bool? isRead, string keyword, int? msgTypeId)
        {
            User user = PageBase.GetCurrentUserSession();
            string currentUsername = user != null ? user.Username : null;
            
            bool isFromCurrent = fromUsername == currentUsername;
            bool isToCurrent = toUsername == currentUsername;
            
            if (!(isFromCurrent || isToCurrent)) return null;
            
            using (var db = new Model.ezFixUpDataContext())
            {
                var messageIds = 
                    (from m in db.Messages
                     where (id == 0 || m.m_id == id)
                           && (fromUsername == null || (isFromCurrent && m.m_from_username == fromUsername) || (isToCurrent && m.m_from_username.StartsWith(fromUsername)))
                           && (fromFolder == 0 || (int) fromFolder == m.m_from_folder)
                           && (toUsername == null || (isToCurrent && m.m_to_username == toUsername) ||  (isFromCurrent && m.m_to_username.StartsWith(toUsername)))
                           && (toFolder == 0 || (int) toFolder == m.m_to_folder)
                           && (inPastDays == 0 || (DateTime.Now - m.m_timestamp).Days <= inPastDays)
                           && (!filterApproved || !approved == m.m_pending_approval)
                           && (!newMessages || m.m_new)
                           && (!isRead.HasValue || m.m_is_read == isRead)
                           && (keyword == null || m.m_body.Contains(keyword)|| m.m_subject.Contains(keyword))
                           && (msgTypeId == null || m.m_type_id == msgTypeId)
                     orderby m.m_timestamp descending
                     select m.m_id).Take(1000).ToList();

                return (messageIds != null && messageIds.Count() > 0) ? messageIds.ToArray() : null;
            }
            
            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = (SqlDataReader)
            //        SqlHelper.GetDB().ExecuteReader( "FetchMessages",
            //                                id == 0 ? 0 : id,
            //                                fromUsername,
            //                                fromFolder == eFolder.None ? 0 : (int) fromFolder,
            //                                toUsername,
            //                                toFolder == eFolder.None ? 0 : (int) toFolder,
            //                                inPastDays,
            //                                filterApproved ? (object) !approved : null,
            //                                newMessages,
            //                                isRead,
            //                                keyword);

            //    List<int> lMessageIds = new List<int>();

            //    while (reader.Read())
            //    {
            //        lMessageIds.Add((int)reader["Id"]);
            //    }

            //    return lMessageIds.ToArray();
            //}
        }

        public static bool HasRaplies(int messageID, string fromUsername, string toUsername)
            {
                using (var db = new Model.ezFixUpDataContext())
                {
                    var hasReplies = db.Messages.Count(m => m.m_replied_to == messageID
                                                        && m.m_from_username == fromUsername
                                                        && m.m_to_username == toUsername) > 0;
                    return hasReplies;
                }
            }

        /// <summary>
        /// Searches the unread.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static int[] SearchUnread(string username)
        {
            return Search(0, null, eFolder.None, username, eFolder.Inbox, 0, true, true, false, false, null, null);
        }

        private static Message[] Fetch(int Id, string fromUsername, eFolder fromFolder, string toUsername,
                                       eFolder toFolder, int inPastDays, bool filterApproved, bool approved,
                                       bool newMessages, int? msgTypeId)
        {
            int[] messageIds = Search(Id, fromUsername, fromFolder, toUsername, toFolder, 
                inPastDays, filterApproved, approved, newMessages, null, msgTypeId);

            return messageIds == null ? null : messageIds.Select(messageId => Fetch(messageId)).ToArray();
        }

        /// <summary>
        /// Fetch message by id
        /// </summary>
        /// <param name="id">ID of the message</param>
        /// <returns>A Message object</returns>
        /// <exception cref="NotFoundException">A message with this ID was not found</exception>
        public static Message Fetch(int id)
        {
            using (var db = new Model.ezFixUpDataContext())
            {
                var message = (from m in db.Messages
                                 where (m.m_id == id)
                                 select new Message
                                            {
                                                id = m.m_id,
                                                fromUsername = m.m_from_username,
                                                fromFolder = (eFolder) m.m_from_folder,
                                                toUsername = m.m_to_username,
                                                toFolder = (eFolder) m.m_to_folder,
                                                aboutUsername = m.m_about_username,
                                                subject = m.m_subject,
                                                body = m.m_body,
                                                timestamp = m.m_timestamp,
                                                repliedTo = m.m_replied_to,
                                                isRead = m.m_is_read,
                                                pendingApproval = m.m_pending_approval
                                            }).FirstOrDefault();

                if (message == null)
                    throw new NotFoundException
                        (Lang.Trans("The requested message does not exist!"));

                return message;
            }
            //using (SqlConnection conn = Config.DB.Open())
            //{
            //    SqlDataReader reader = (SqlDataReader)
            //        SqlHelper.GetDB().ExecuteReader( "LoadMessage", Id);

            //    if (reader.Read())
            //    {
            //        Message msg = new Message();

            //        msg.id = (int) reader["Id"];
            //        msg.fromUsername = (string) reader["FromUsername"];
            //        msg.fromFolder = (eFolder) (int) reader["FromFolder"];
            //        msg.toUsername = (string) reader["ToUsername"];
            //        msg.toFolder = (eFolder) (int) reader["ToFolder"];
            //        msg.body = (string) reader["Body"];
            //        msg.timestamp = (DateTime) reader["Timestamp"];
            //        msg.repliedTo = (int) reader["RepliedTo"];
            //        msg.isRead = (bool) reader["IsRead"];
            //        msg.pendingApproval = (bool) reader["PendingApproval"];

            //        return msg;
            //    }
            //    else
            //    {
            //        throw new NotFoundException
            //            (Lang.Trans("The requested message does not exist!"));
            //    }
            //}
        }

        private static Message[] Fetch(int id, string fromUsername, eFolder fromFolder,
                                       string toUsername, eFolder toFolder, int inPastDays, bool approved,
                                       bool newMessages, int? msgTypeId)
        {
            return Fetch(id, fromUsername, fromFolder, // true for filter approved
                         toUsername, toFolder, inPastDays,true, approved, newMessages, msgTypeId);
        }

        private static Message[] Fetch(int Id, string fromUsername, eFolder fromFolder,
                                       string toUsername, eFolder toFolder, int inPastDays, bool newMessages, int? msgTypeId)
        {
            return Fetch(Id, fromUsername, fromFolder,
                         toUsername, toFolder, inPastDays, false, false, newMessages, msgTypeId);
        }

        /// <summary>
        /// Approves the message.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void ApproveMessage(int id)
        {
            if (id > 0)
            {
                using (SqlConnection conn = Config.DB.Open())
                {
                    SqlHelper.GetDB().ExecuteNonQuery( "UpdateMessage",
                        id, null, null, null, null, false /*pending approval=false*/);
                }
            }
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public static void Delete(int id)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "DeleteMessage", id);
            }
        }

        /// <summary>
        /// Fetches the non approved.
        /// </summary>
        /// <returns></returns>
        public static Message[] FetchNonApproved()
        {
            return Fetch(0, null, eFolder.None, null, eFolder.None, 0, false, false, null);
        }

        /// <summary>
        /// Fetch incoming messages for a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchInbox(string username)
        {
            return FetchInbox(username, null);
        }

        /// <summary>
        /// Fetch incoming messages for a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="fromUsername">From username.</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchInbox(string username, string fromUsername)
        {
            return Fetch(0, fromUsername, eFolder.None, username, eFolder.Inbox, 0, true, false, null);
        }

        /// <summary>
        /// Ares the new messages.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static bool AreNewMessages(string username)
        {
            string cacheKey = String.Format("Message_NewMessageNotification_{0}", username);
            if (HttpContext.Current != null
                && HttpContext.Current.Cache[cacheKey] != null
                && (bool) HttpContext.Current.Cache[cacheKey])
            {
                HttpContext.Current.Cache.Remove(cacheKey);
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Fetches the new messages.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static Message[] FetchNewMessages(string username)
        {
            return Fetch(0, null, eFolder.None, username, eFolder.Inbox, 0, true, true, null);
        }

        /// <summary>
        /// Fetch incoming messages for a user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchInbox(User user)
        {
            return FetchInbox(user.Username);
        }

        /// <summary>
        /// Fetch outgoing messages for a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchOutbox(string username)
        {
            return FetchOutbox(username, null);
        }

        /// <summary>
        /// Fetch outgoing messages for a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="toUsername">To username.</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchOutbox(string username, string toUsername)
        {
            return Fetch(0, username, eFolder.Outbox, toUsername, eFolder.None, 0, true, false, null);
        }

        /// <summary>
        /// Fetch outgoing messages for a user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchOutbox(User user)
        {
            return FetchOutbox(user.Username);
        }

        /// <summary>
        /// Fetch trashed messages for a user
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchTrash(string username)
        {
            return Fetch(0, null, eFolder.None, username, eFolder.Trash, 0, true, false, null);
        }

        /// <summary>
        /// Fetch trashed messages for a user
        /// </summary>
        /// <param name="user">User object</param>
        /// <returns>Array of Message objects</returns>
        public static Message[] FetchTrash(User user)
        {
            return FetchTrash(user.Username);
        }

        /// <summary>
        /// Fetches the sent messages for the last 24 hours.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="msgTypeId">to differ between msg/gift/wink</param>
        /// <returns></returns>
        public static Message[] FetchSentMessagesForLast24Hours(string sender, int? msgTypeId)
        {
            return Fetch(0, sender, eFolder.None, null, eFolder.None, 1, false, msgTypeId);
        }

        /// <summary>
        /// Sends the message
        /// </summary>
        public void Send()
        {
            bool pendingApproval = false;
            
            //check if FromUser and ToUser exist in the database
            try
            {
                User fu = FromUser;
                User tu = ToUser;
                User au = aboutUsername.IsNotNullOrEmpty() ? aboutUser : null;
            }
            catch (NotFoundException) { throw; }
            
            if (Config.Users.MessageVerificationEnabled)
            {
                if (FromUser.MessageVerificationsLeft > 0)
                    pendingApproval = true;
            }
            else
                pendingApproval = false;

            if (body.Length > 3500)
                throw new ArgumentException(Lang.Trans("Your message is too long and cannot be delivered!"));
                
            using (SqlConnection conn = Config.DB.Open())
            {
                id = Convert.ToInt32(
                    SqlHelper.GetDB().ExecuteScalar( "SendMessage", 
                        fromUsername, toUsername, aboutUsername, subject, body, 
                        repliedTo, pendingApproval));
            }

            if (!pendingApproval)
            {
                ToUser.SendMessageNotification(fromUsername, body);
            }

            /*
            if (Config.Users.NewMessageNotification && ToUser.IsOnline()
                && HttpContext.Current != null)
            {
                string cacheKey = String.Format("Message_NewMessageNotification_{0}", ToUser.Username);
                HttpContext.Current.Cache.Insert(cacheKey, true, null, DateTime.Now.AddMinutes(5),
                                                 Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);
            }
            */

            if (!pendingApproval && (ToUser.IsOnline() || User.IsUsingNotifier(toUsername)))
                RealtimeNotification.SendNotification(NewMessageNotification.FromMessage(this));
        }

        /// <summary>
        /// Sends a message
        /// </summary>
        /// <param name="fromUsername">Sender username</param>
        /// <param name="toUsername">Recipient username</param>
        /// <param name="body">Message body</param>
        /// <param name="repliedTo">Id of the message that we're replying to</param>
        public static void Send(string fromUsername, string toUsername, string body, int repliedTo)
        {
            Message msg = new Message(fromUsername, toUsername);
            msg.body = body;
            msg.repliedTo = repliedTo;
            msg.Send();
        }

        /// <summary>
        /// Sends the welcome message.
        /// </summary>
        /// <param name="user">The user.</param>
        public static void SendWelcomeMessage(User user)
        {
            MiscTemplates.WelcomeMessage welcomeMessageTemplate = new MiscTemplates.WelcomeMessage(user.LanguageId);
            string welcomeMessage = welcomeMessageTemplate.GetFormattedMessage(user.Username, Config.Misc.SiteTitle);
            
            if (welcomeMessage.Length > 3500)
                welcomeMessage = welcomeMessage.Substring(0, 3499);


            Send(Config.Users.SystemUsername, user.Username, welcomeMessage, 0);
        }

        /// <summary>
        /// Messageses the exist.
        /// </summary>
        /// <param name="fromUsername">From username.</param>
        /// <param name="toUsername">To username.</param>
        /// <returns></returns>
        public static bool MessagesExist(string fromUsername, string toUsername)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return (bool)
                       SqlHelper.GetDB().ExecuteScalar(
                                               "MessagesExist", fromUsername, toUsername);
            }
        }

        /// <summary>
        /// Clears the new message flags.
        /// </summary>
        /// <param name="username">The username.</param>
        [Obsolete]
        public static void ClearNewMessageFlags(string username)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery(
                                          "ClearNewMessageFlags", username);
            }
        }
    }

    /// <summary>
    /// The message search results class
    /// </summary>
    [Serializable]
    public class MessageSearchResults : SearchResults<int, Message>
    {
        public MessageSearchResults()
        {
            Results = new int[0];
        }

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public int[] Messages
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

        /// <summary>
        /// Gets the total pages.
        /// </summary>
        /// <param name="messagesPerPage">The messages per page.</param>
        /// <returns></returns>
        public new int GetTotalPages(int messagesPerPage)
        {
            return base.GetTotalPages(messagesPerPage);
        }

        /// <summary>
        /// Loads the result.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        protected override Message LoadResult(int id)
        {
            return Message.Fetch(id);
        }
        protected override Message[] LoadResults(int[] ids)
        {
            List<int> keysList = ids.ToList();
            List<Message> resultList = new List<Message>();
            if (keysList.Count > 0)
            {
                var parameterizedThreadsList = new ParameterizedThreadsList<int, Message>(
                                                    keysList, resultList,
                                                    (new LoadMessagesResultThreadClass(keysList, resultList)).DoWorkThread,
                                                    HttpContext.Current);
                parameterizedThreadsList.RunAsBackgroundThreads = true;
                parameterizedThreadsList.ExecuteParallelWork(5);
            }
            return resultList.ToArray();
        }

        
        /// <summary>
        /// Use this method to get the search results
        /// </summary>
        /// <param name="page">Page number</param>
        /// <param name="messagesPerPage">messagesPerPage</param>
        /// <returns>Array of messages</returns>
        public new Message[] GetPage(int page, int messagesPerPage)
        {
            return base.GetPage(page, messagesPerPage);
        }

        /// <summary>
        /// Gets this instance.
        /// </summary>
        /// <returns></returns>
        public Message[] Get()
        {
            return GetPage(1, Int32.MaxValue);
        }
    }
}