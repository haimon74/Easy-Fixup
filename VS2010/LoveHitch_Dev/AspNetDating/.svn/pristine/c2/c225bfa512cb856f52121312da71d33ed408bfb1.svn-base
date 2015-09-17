using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AspNetDating.Classes
{
    public static class PerformanceTimers
    {
        private static Dictionary<Guid, DateTime> LogRequests = new Dictionary<Guid, DateTime>();
        private static Dictionary<string, TimeSpan> LogTimer = new Dictionary<string, TimeSpan>();
        private static Dictionary<string, int> LogCounter = new Dictionary<string, int>();

        public static Guid LogStartRequest()
        {
            var guid = Guid.NewGuid();

            lock (LogRequests)
            {
                LogRequests.Add(guid, DateTime.Now);
            }

            return guid;
        }

        public static void LogEndRequest(Guid guid, string key)
        {
            lock (LogRequests)
            {
                LogPerformanceData(key, DateTime.Now.Subtract(LogRequests[guid]));
                LogRequests.Remove(guid);
            }
        }

        public static void LogPerformanceData(string key, TimeSpan time)
        {
            lock (LogTimer)
            {
                if (!LogTimer.ContainsKey(key))
                    LogTimer.Add(key, time);
                else
                    LogTimer[key].Add(time);
            }

            lock (LogCounter)
            {
                if (!LogCounter.ContainsKey(key))
                    LogCounter.Add(key, 1);
                else
                    LogCounter[key]++;
            }
        }

        public static void WriteDataToLogFile()
        {
            lock (LogTimer) lock (LogCounter)
            {
                var dump = new StringBuilder("Dumping performance timers\n");
                foreach (var key in LogTimer.Keys)
                {
                    dump.AppendFormat("{0} x {1} = {2}\n", key, LogCounter[key], LogTimer[key]);
                }
                Global.Logger.LogStatus("PerformanceTimers", dump);
            }
        }
    }
}
