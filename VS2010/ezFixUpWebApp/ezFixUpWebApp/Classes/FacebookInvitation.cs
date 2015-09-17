using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web;
using ezFixUp.Classes.ezFixUpParallel;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class FacebookInvitation
    {
        #region fields

        private int? id;
        public string RequestId;
        public string SenderId;
        public string RecipientId;
        public DateTime CreatedTime;
        public bool IsCredited;

        #endregion

        #region Constructors

        private FacebookInvitation()
        {}

        public FacebookInvitation(string requestId, string senderId, string recipientId, DateTime createdTime)
        {
            RequestId = requestId;
            SenderId = senderId;
            RecipientId = recipientId;
            CreatedTime = createdTime;
        }
        
        #endregion

        #region Properties

        public int ID
        {
            get 
            {
                if (id.HasValue)
                    return id.Value;
                else throw new Exception("ID is not set");
            }
        }
        
        #endregion

        #region Methods

        public static int[] Search(string senderId, string recipientId, DateTime fromTime)
        {
            //using (var conn = Config.DB.Open())
            {
                List<int> lInvitationIds = new List<int>();

                using (var reader = SqlHelper.GetDB().ExecuteReader("SearchFacebookInvitations",
                                                    senderId,recipientId,fromTime))
                {
                    while (reader.Read())
                    {
                        int id = (int) reader["ID"];
                        lInvitationIds.Add(id);
                    }
                    reader.Close();
                }

                return lInvitationIds.ToArray();
            }
        }
        public static FacebookInvitation[] FetchInvitationsByFacebookId(string senderId, string recipientId)
        {
            List<FacebookInvitation> list = new List<FacebookInvitation>();
            
            int[] ids = Search(senderId, recipientId, DateTime.Now.AddYears(-1));
            
            foreach (var id in ids)
            {
                list.Add(Fetch(id));
            }
            return list.ToArray();
        }

        /// <summary>
        /// Fetches an FacebookInvitation by specified id from DB. If the it doesn't exist returns NULL.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static FacebookInvitation Fetch(int id)
        {
            //using (var conn = Config.DB.Open())
            {
                FacebookInvitation invitation = null;

                using (var reader = SqlHelper.GetDB().ExecuteReader("LoadFacebookInvitation", id))
                {
                    if (reader.Read())
                    {
                        invitation = new FacebookInvitation();

                        invitation.id = (int) reader["ID"];
                        invitation.CreatedTime = (DateTime) reader["fi_created_time"];
                        invitation.SenderId = (string) reader["fi_sender_id"];
                        invitation.RecipientId = (string) reader["fi_recipient_id"];
                        invitation.RequestId = (string) reader["fi_request_id"];
                        invitation.IsCredited = (bool) reader["fi_is_credited"];
                    }
                    reader.Close();
                }
                return invitation;
            }
        }

        /// <summary>
        /// Saves this instance in DB.
        /// If id for this instance is NULL it inserts new record in DB otherwise updates the record.
        /// </summary>
        public void Save()
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveFacebookInvitation", id,
                                                        RequestId, SenderId, RecipientId, CreatedTime);
                
                if (id == null)
                {
                    id = Convert.ToInt32(result);
                }
            }
        }

        #endregion
    }

}
