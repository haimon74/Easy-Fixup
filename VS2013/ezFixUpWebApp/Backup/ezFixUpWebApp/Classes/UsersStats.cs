using System;
using System.Collections;
using System.Data.SqlClient;
using System.Threading;
using System.Timers;
using Timer=System.Timers.Timer;

namespace ezFixUp.Classes
{
    public static class HourlyStats
    {
        public enum eType
        {
            Hourly,
            Daily,
            Weekly,
            Monthly
        }

        private static Timer tMailer;
        private static bool mailerLock = false;

        public static void InitializeMailerTimer()
        {
            tMailer = new Timer();
            tMailer.AutoReset = true;
            tMailer.Interval = TimeSpan.FromHours(1).TotalMilliseconds;
            tMailer.Elapsed += new ElapsedEventHandler(tMailer_Elapsed);
            tMailer.Start();

            // Run processing the 1st time
            tMailer_Elapsed(null, null);
        }

        private static void tMailer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DateTime dtLastTick = DBSettings.Get("HourlyStats_LastTimerTick", DateTime.Now);

            if (DateTime.Now.Subtract(dtLastTick) >= TimeSpan.FromHours(1))
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(AsyncProcessMailerQueue), null);
                DBSettings.Set("HourlyStats_LastTimerTick", DateTime.Now);
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

                int usersOnline = 0;
                int newUsers = 0;
                int sentMessages = 0;
                DateTime to =
                    new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
                DateTime from = to - TimeSpan.FromHours(1);

                OnlineSearch onlineUsersSearch = new OnlineSearch();
                UserSearchResults results = onlineUsersSearch.GetResults();
                usersOnline = results != null ? results.Usernames.Length : 0;
                NewUsersSearch newUsersSearch = new NewUsersSearch();
                newUsersSearch.ProfileReq = false;
                newUsersSearch.UsersSince = DateTime.Now.AddHours(-1);
                results = newUsersSearch.GetResults();
                newUsers = results != null ? results.Usernames.Length : 0;
                sentMessages = FetchMessagesCount(from, to);

                Save(DateTime.Now, usersOnline, newUsers, sentMessages);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("HourlyStats", err);
            }
            finally
            {
                mailerLock = false;
            }
        }

        public static void Save(DateTime dateTime, int usersOnline, int newUsers, int sentMessages)
        {
            //using (var conn = Config.DB.Open())
            {
                SqlHelper.GetDB().ExecuteNonQuery( "SaveHourlyStats",
                                            new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, 0, 0),
                                            usersOnline, newUsers, sentMessages);
            }
        }

        /// <summary>
        /// Fetches the messages from DB which are sent between specified interval.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static int FetchMessagesCount(DateTime from, DateTime to)
        {
            //using (var conn = Config.DB.Open())
            {
                return Convert.ToInt32(SqlHelper.GetDB().ExecuteScalar( "FetchMessagesCount", from, to));
            }
        }

        public static Hashtable FetchNewUsersStatsHourly(DateTime from, DateTime to)
        {
            return FetchHourlyStatsForNewUsers(from, to, eType.Hourly);
        }

        public static Hashtable FetchNewUsersStatsDaily(DateTime from, DateTime to)
        {
            return FetchHourlyStatsForNewUsers(from, to, eType.Daily);
        }

        public static Hashtable FetchNewUsersStatsWeekly(DateTime from, DateTime to)
        {
            return FetchHourlyStatsForNewUsers(from, to, eType.Weekly);
        }

        public static Hashtable FetchNewUsersStatsMonthly(DateTime from, DateTime to)
        {
            return FetchHourlyStatsForNewUsers(from, to, eType.Monthly);
        }

        public static Hashtable FetchHourlyStatsForNewUsers(DateTime from, DateTime to, eType type)
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = SqlHelper.GetDB().ExecuteReader( "FetchHourlyStatsForNewUsers", from, to, type);

                Hashtable htStats = new Hashtable();

                while(reader.Read())
                {
                    htStats.Add((DateTime) reader["Time"], (int) reader["NewUsers"]);
                }

                return htStats;
            }
        }

        public static Hashtable FetchHourlyStatsForOnlineUsers(DateTime from, DateTime to, eType type)
        {
            //using (var conn = Config.DB.Open())
            {
                var reader = SqlHelper.GetDB().ExecuteReader( "FetchHourlyStatsForOnlineUsers", from, to, type);

                Hashtable htStats = new Hashtable();

                while (reader.Read())
                {
                    htStats.Add((DateTime)reader["Time"], (int)reader["OnlineUsers"]);
                }

                return htStats;
            }
        }
    }
}
