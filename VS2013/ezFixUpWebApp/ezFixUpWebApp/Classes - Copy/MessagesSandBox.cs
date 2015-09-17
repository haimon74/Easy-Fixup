using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Timer=System.Timers.Timer;

namespace ezFixUp.Classes
{
    public static class MessagesSandBox
    {
        public static string FetchOnlyOneSpamMessage(string username)
        {
            string[] messages = Fetch(username, null, null, null, Config.Misc.MaxSameMessages, true);
            
            if (messages.Length > 0)
            {
                return messages[0];
            }
            else
            {
                return null;
            }
        }


        public static string[] Fetch(string username, string message, DateTime? fromDate, DateTime? toDate, int maxSameMessages, bool? fetchOnlyOneRecord)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                List<string> lMessages = new List<string>();
                SqlDataReader reader = (SqlDataReader) SqlHelper.GetDB().ExecuteReader( "FetchMessagesSandBox",
                    username, message, fromDate, toDate, maxSameMessages, fetchOnlyOneRecord);

                while (reader.Read())
                {
                    lMessages.Add((string) reader["Message"]);
                }

                return lMessages.ToArray();
            }
        }

        /// <summary>
        /// Saves the message into DB.
        /// Returns the number of message occurrence in DB.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public static int Save(string username, string message)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.GetDB().ExecuteScalar( "SaveMessagesSandBox", username, message));
            }
        }

        public static void DeleteMessagesSandBoxForLast24Hours()
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteMessagesSandBoxForLast24Hours");
            }
        }

        public static void DeleteMessagesSandBox(string username, string message)
        {
            using (SqlConnection conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "DeleteMessagesSandBox", username, message);
            }
        }

        private static Timer tMailer;
        private static bool mailerLock = false;

        public static void InitializeMailerTimer()
        {
            tMailer = new System.Timers.Timer();
            tMailer.AutoReset = true;
            tMailer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            tMailer.Elapsed += new ElapsedEventHandler(tMailer_Elapsed);
            tMailer.Start();

            // Run processing the 1st time
            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("MessagesSandBox_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(1))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessMailerQueue), null);
                DBSettings.Set("MessagesSandBox_LastTimerTick", DateTime.Now);
            }
        }

        private static void AsyncProcessMailerQueue(object data)
        {
            if (mailerLock)
            {
                return;
            }

            try
            {
                mailerLock = true;

                DeleteMessagesSandBoxForLast24Hours();

            }
            catch (Exception err)
            {
                Global.Logger.LogError("MessagesSandBox", err);
            }
            finally
            {
                mailerLock = false;
            }
        }
    }
}
