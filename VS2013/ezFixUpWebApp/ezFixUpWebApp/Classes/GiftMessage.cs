using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace ezFixUp.Classes
{
    public class GiftMessage
    {
        #region fields

        private int? id = null;
        private int giftId;
        private int messageId;
        
        #endregion

        #region Constructors

        private GiftMessage()
        {
        }

        public GiftMessage(int giftId, int msgId)
        {
            this.giftId = giftId;
            this.messageId = msgId;
        }

        public GiftMessage(int? id, int giftId, int msgId)
        {
            this.id = id;
            this.giftId = giftId;
            this.messageId = msgId;
        }

        #endregion

        #region Properties

        public int MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public int GiftId
        {
            get { return giftId; }
            set { giftId = value; }
        }

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
        
        #endregion

        #region Methods

        public GiftMessage[] FetchByMessageId()
        {
            return FetchByMessageId(MessageId);
        }

        public static GiftMessage[] FetchByMessageId(int msgId)
        {
            var giftsOfMessages = new List<GiftMessage>();

            //using (var conn = Config.DB.Open())
            {
                using (var reader = SqlHelper.GetDB().ExecuteReader("FetchGiftsOfMessage", msgId))
                {

                    while (reader.Read())
                    {
                        var giftMessage = new GiftMessage
                            {
                                id = (int) reader["Id"],
                                GiftId = (int) reader["GiftId"],
                                MessageId = (int) reader["MessageId"]
                            };
                        giftsOfMessages.Add(giftMessage);
                    }
                    reader.Close();
                }
            }
            return giftsOfMessages.ToArray();
        }

        public static int Insert(int msgId, int giftId)
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveGiftMessage", null, msgId, giftId);
                return Convert.ToInt32(result);
            }
        }
        public static void Update(int id, int msgId, int giftId)
        {
            //using (var conn = Config.DB.Open())
            {
                object result = SqlHelper.GetDB().ExecuteScalar( "SaveGiftMessage", id, msgId, giftId);
            }
        }
        //public static void Delete(int? id, int? msgId)
        //{
        //    //using (var conn = Config.DB.Open())
        //    {
        //        SqlHelper.GetDB().ExecuteNonQuery( "DeleteWinkMessage", id, msgId);
        //    }
        //}
        #endregion
    }
}
