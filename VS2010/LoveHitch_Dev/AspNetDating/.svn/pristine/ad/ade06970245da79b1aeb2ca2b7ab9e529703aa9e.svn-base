using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace AspNetDating.Classes
{
    /// <summary>
    /// Handles the profile video broadcast feature
    /// </summary>
    [Serializable]
    public class VideoBroadcastMessage
    {
        private DateTime timestamp = DateTime.Now;
        private string username;
        private string message;

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        /// <summary>
        /// Gets the timestamp string.
        /// </summary>
        /// <value>The timestamp string.</value>
        public string TimestampString
        {
            get
            {
                return timestamp.ToShortTimeString();
            }
        }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        /// <summary>
        /// Gets the profile URL.
        /// </summary>
        /// <value>The profile URL.</value>
        public string ProfileURL
        {
            get
            {
                return UrlRewrite.CreateShowUserUrl(username);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class VideoBroadcast
    {
        private static readonly Dictionary<string, Guid> VideoBroadcasts =
            new Dictionary<string, Guid>();
        private static readonly Dictionary<Guid, DateTime> VideoBroadcastsActivity =
            new Dictionary<Guid, DateTime>();
        private static readonly Dictionary<Guid, Dictionary<string, DateTime>> VideoBroadcastWatchers =
            new Dictionary<Guid, Dictionary<string, DateTime>>();
        private static readonly Dictionary<Guid, List<VideoBroadcastMessage>> VideoBroadcastMessages =
            new Dictionary<Guid, List<VideoBroadcastMessage>>();

        private static readonly Dictionary<Guid, List<string>> videoBroadcastBannedWatchers =
            new Dictionary<Guid, List<string>>();

        /// <summary>
        /// Adds the broadcast.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="guid">The GUID.</param>
        public static void AddBroadcast(string username, Guid guid)
        {
            lock (VideoBroadcasts)
            {
                if (VideoBroadcasts.ContainsKey(username))
                {
                    VideoBroadcasts.Remove(username);
                }
                VideoBroadcasts.Add(username, guid);
            }
            KeepBroadcastAlive(guid);
        }

        /// <summary>
        /// Keeps the broadcast alive.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        public static void KeepBroadcastAlive(Guid guid)
        {
            lock (VideoBroadcastsActivity)
            {
                if (!VideoBroadcastsActivity.ContainsKey(guid))
                {
                    VideoBroadcastsActivity.Add(guid, DateTime.Now);
                }
                else
                {
                    VideoBroadcastsActivity[guid] = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Determines whether [is broadcast alive] [the specified GUID].
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns>
        /// 	<c>true</c> if [is broadcast alive] [the specified GUID]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBroadcastAlive(Guid guid)
        {
            lock (VideoBroadcastsActivity)
            {
                if (VideoBroadcastsActivity.ContainsKey(guid)
                    && VideoBroadcastsActivity[guid] > DateTime.Now.Subtract(TimeSpan.FromSeconds(60)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the broadcast.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <returns></returns>
        public static Guid? GetBroadcast(string username)
        {
            Guid? guid;
            lock (VideoBroadcasts)
            {
                if (VideoBroadcasts.ContainsKey(username))
                {
                    guid = VideoBroadcasts[username];
                    if (guid.HasValue && VideoBroadcastsActivity.ContainsKey(guid.Value)
                            && VideoBroadcastsActivity[guid.Value] > DateTime.Now.Subtract(TimeSpan.FromSeconds(60)))
                    {
                        return guid.Value;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the broadcasts.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, Guid> GetBroadcasts()
        {
            Dictionary<string, Guid> results = new Dictionary<string, Guid>();
            lock (VideoBroadcasts)
            {
                foreach (KeyValuePair<string, Guid> broadcast in VideoBroadcasts)
                {
                    if (VideoBroadcastsActivity.ContainsKey(broadcast.Value)
                            && VideoBroadcastsActivity[broadcast.Value] > DateTime.Now.Subtract(TimeSpan.FromSeconds(60)))
                    {
                        results.Add(broadcast.Key, broadcast.Value);
                    }
                }
            }
            return results;
        }

        /// <summary>
        /// Updates the watcher.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="username">The username.</param>
        public static void UpdateWatcher(Guid guid, string username)
        {
            Dictionary<string, DateTime> watchers;
            lock (VideoBroadcastWatchers)
            {
                if (videoBroadcastBannedWatchers.ContainsKey(guid) && videoBroadcastBannedWatchers[guid].Contains(username))
                {
                    return;
                }

                if (!VideoBroadcastWatchers.ContainsKey(guid))
                {
                    VideoBroadcastWatchers.Add(guid, new Dictionary<string, DateTime>());
                }
                watchers = VideoBroadcastWatchers[guid];
                if (!watchers.ContainsKey(username))
                {
                    watchers.Add(username, DateTime.Now);
                }
                else
                {
                    watchers[username] = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Gets the watchers.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <returns></returns>
        public static string[] GetWatchers(Guid guid)
        {
            Dictionary<string, DateTime> watchers;
            string[] usernames;
            lock (VideoBroadcastWatchers)
            {
                if (!VideoBroadcastWatchers.ContainsKey(guid)) return null;
                watchers = VideoBroadcastWatchers[guid];

                var quitWatchers = watchers.Where(watcher =>
                                                  watcher.Value < DateTime.Now.Subtract(TimeSpan.FromSeconds(60))).ToArray();

                foreach (KeyValuePair<string, DateTime> pair in quitWatchers)
                {
                    watchers.Remove(pair.Key);
                }

                usernames = watchers.Keys.ToArray<string>();
            }
            return usernames;
        }

        public static void RemoveWatcher(string username,  Guid guid)
        {
            lock (VideoBroadcastWatchers)
            {
                if (VideoBroadcastWatchers.ContainsKey(guid))
                {
                    if (VideoBroadcastWatchers[guid].ContainsKey(username))
                        VideoBroadcastWatchers[guid].Remove(username);                    
                }
            }            
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="message">The message.</param>
        public static void AddMessage(Guid guid, VideoBroadcastMessage message)
        {
            List<VideoBroadcastMessage> messages;
            lock (VideoBroadcastMessages)
            {
                if (!VideoBroadcastMessages.ContainsKey(guid))
                {
                    VideoBroadcastMessages.Add(guid, new List<VideoBroadcastMessage>());
                }
                messages = VideoBroadcastMessages[guid];
                messages.Add(message);
            }
        }

        /// <summary>
        /// Fetches the messages.
        /// </summary>
        /// <param name="guid">The GUID.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public static VideoBroadcastMessage[] FetchMessages(Guid guid, int count)
        {
            VideoBroadcastMessage[] result;
            List<VideoBroadcastMessage> messages;
            lock (VideoBroadcastMessages)
            {
                if (!VideoBroadcastMessages.ContainsKey(guid)) return null;
                messages = VideoBroadcastMessages[guid];
                result = new VideoBroadcastMessage[messages.Count < count ? messages.Count : count];
                messages.CopyTo(messages.Count - result.Length, result, 0, result.Length);
            }
            return result;
        }

        public static void AddUserToBanList(string username, Guid guid)
        {
            lock (videoBroadcastBannedWatchers)
            {
                if (!videoBroadcastBannedWatchers.ContainsKey(guid))
                {
                    videoBroadcastBannedWatchers.Add(guid, new List<string>());
                }

                if (!videoBroadcastBannedWatchers[guid].Contains(username))
                    videoBroadcastBannedWatchers[guid].Add(username);
            }
        }

        public static void RemoveUserFromBanList(string username, Guid guid)
        {
            lock (videoBroadcastBannedWatchers)
            {
                if (videoBroadcastBannedWatchers.ContainsKey(guid))
                {
                    if (videoBroadcastBannedWatchers[guid].Contains(username))
                        videoBroadcastBannedWatchers[guid].Remove(username);
                }
            }
        }

        public static string[] GetBannedWatchers(Guid guid)
        {
            lock (videoBroadcastBannedWatchers)
            {
                if (!videoBroadcastBannedWatchers.ContainsKey(guid)) return new string[0];
                return videoBroadcastBannedWatchers[guid].ToArray();
            }
        }
    }
}