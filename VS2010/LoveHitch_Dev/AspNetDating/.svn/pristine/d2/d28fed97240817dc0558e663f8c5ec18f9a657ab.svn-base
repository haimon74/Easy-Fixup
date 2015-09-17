using System;
using System.Data.SqlClient;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    public class WinkMessage
    {
        #region fields

        private int? id = null;
        private int winkId;
        private int messageId;
        
        #endregion

        #region Constructors

        private WinkMessage()
        {
        }

        public WinkMessage(int winkId, int msgId)
        {
            this.winkId = winkId;
            this.messageId = msgId;
        }

        public WinkMessage(int? id, int winkId, int msgId)
        {
            this.id = id;
            this.winkId = winkId;
            this.messageId = msgId;
        }

        #endregion

        #region Properties

        public int MessageId
        {
            get { return messageId; }
            set { messageId = value; }
        }

        public int WinkId
        {
            get { return winkId; }
            set { winkId = value; }
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

        public WinkMessage FetchByMessageId()
        {
            return FetchByMessageId(MessageId);
        }

        public static WinkMessage FetchByMessageId(int msgId)
        {
            WinkMessage winkMessage = null;

            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader = SqlHelper.ExecuteReader(conn, "FetchWinkMessage", msgId);

                if (reader.Read())
                {
                    winkMessage = new WinkMessage
                                      {
                                          id = (int)reader["Id"],
                                          WinkId = (int) reader["WinkId"],
                                          MessageId = (int) reader["MessageId"]
                                      };
                }
            }
            return winkMessage;
        }

        public static int Insert(int msgId, int winkId)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveWinkMessage", null, msgId, winkId);
                return Convert.ToInt32(result);
            }
        }
        public static void Update(int id, int msgId, int winkId)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                object result = SqlHelper.ExecuteScalar(conn, "SaveWinkMessage", id, msgId, winkId);
            }
        }
        //public static void Delete(int? id, int? msgId)
        //{
        //    using (SqlConnection conn = Config.DB.Open())
        //    {
        //        SqlHelper.ExecuteNonQuery(conn, "DeleteWinkMessage", id, msgId);
        //    }
        //}
        #endregion
    }
}
