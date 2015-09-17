using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Services;
using System.Web.Services;
using ezFixUp.Classes;

namespace ezFixUp.Services
{
    [WebService(Namespace = "ezFixUp.Services")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class OnlineCheck : WebService
    {
        #region Used by the Online Checker

        public struct NewEvents
        {
            public RealtimeNotification[] Notifications;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(UseHttpGet = true)]
        public NewEvents UpdateOnline()
        {
            UserSession CurrentUserSession = PageBase.GetCurrentUserSession();
            if (CurrentUserSession == null || !CurrentUserSession.IsAuthorized) return new NewEvents();
            var newEvents = new NewEvents();

            // Update last online
            CurrentUserSession.UpdateLastOnline(false);

            List<RealtimeNotification> notifications = RealtimeNotification.RetrieveNotifications(
                CurrentUserSession.Username);

            if (notifications != null)
            {
                foreach (var notification in notifications)
                {
                    if (notification.Type == "AccountDeletedNotification")
                    {
                        Global.GetSessionState()["UserSession"] = null;
                    }
                }
                newEvents.Notifications = notifications.ToArray();
            }

            return newEvents;
        }

        #endregion

        #region Used by UserPreview popup

        public struct UserPreviewInfo
        {
            public string ThumbnailUrl;
            public string Username;
            public string Gender;
            public int Age;
            public string LastOnline;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public UserPreviewInfo? LoadUserPreviewInfo(string username)
        {
            string cacheKey = String.Format("OnlineCheck_LoadUserPreviewInfo_{0}", username);
            if (HttpContext.Current != null && HttpContext.Current.Cache[cacheKey] != null
                && HttpContext.Current.Cache[cacheKey] is UserPreviewInfo)
            {
                var cachedInfo = (UserPreviewInfo) HttpContext.Current.Cache[cacheKey];
                if (Classes.User.IsOnline(cachedInfo.Username))
                    cachedInfo.LastOnline = "online now".Translate();
                return cachedInfo;
            }

            var info = new UserPreviewInfo();
            User user;
            try
            {
                user = Classes.User.Load(username);
            }
            catch (NotFoundException)
            {
                return null;
            }
            info.Username = user.Username;
            info.Gender = user.Gender.ToString().Translate();
            info.Age = user.Age;
            info.LastOnline = user.LastOnlineString;
            try
            {
                info.ThumbnailUrl = ImageHandler.CreateImageUrl(Photo.GetPrimary(username).Id, 90, 90,
                                                                false, true, true);
            }
            catch (NotFoundException)
            {
                // No thumbnail
            }

            if (HttpContext.Current != null)
            {
                HttpContext.Current.Cache.Insert(cacheKey, info, null, Cache.NoAbsoluteExpiration,
                                                 TimeSpan.FromMinutes(30), CacheItemPriority.NotRemovable, null);
            }

            return info;
        }

        #endregion

        #region Used when broadcasting video
        public struct EventsForVideoBroadcast
        {
            public string UsersWatchingString;
            public bool IsBroadcastAlive;
            public VideoBroadcastMessage[] Messages;
        }

        [WebMethod]
        public EventsForVideoBroadcast FetchNewEventsForVideoBroadcast(string videoGuid, string currentUser)
        {
            Guid theGuid = new Guid(videoGuid);
            EventsForVideoBroadcast events = new EventsForVideoBroadcast();

            string[] usersWatching = VideoBroadcast.GetWatchers(theGuid);
            int usersWatchingCount = usersWatching == null ? 0 : usersWatching.Length;
            events.UsersWatchingString = String.Format("{0} users are watching".Translate(), usersWatchingCount);
            VideoBroadcastMessage[] messages = VideoBroadcast.FetchMessages(theGuid, 20);
            if (messages != null)
            {
                events.Messages = messages;
            }

            //if current user is the broadcaster then keep the broadcast alive
            Guid? currentUserBroadcastGuid = VideoBroadcast.GetBroadcast(currentUser);
            if (currentUserBroadcastGuid.HasValue && currentUserBroadcastGuid.Value.ToString() == videoGuid)
                VideoBroadcast.KeepBroadcastAlive(theGuid);
            else
                VideoBroadcast.UpdateWatcher(theGuid, currentUser);

            events.IsBroadcastAlive = VideoBroadcast.IsBroadcastAlive(theGuid);

            if (events.IsBroadcastAlive)
            {
                foreach (string username in VideoBroadcast.GetBannedWatchers(theGuid))
                {
                    if (username == currentUser)
                    {
                        events.IsBroadcastAlive = false;
                        VideoBroadcast.RemoveUserFromBanList(currentUser, theGuid);
                    }
                }
            }

            return events;
        }

        [WebMethod]
        public void SendMessageForVideoBroadcast(string videoGuid, string text, string currentUser)
        {
            VideoBroadcastMessage message = new VideoBroadcastMessage();
            //message.Username = ((PageBase)Page).CurrentUserSession.Username;
            message.Username = currentUser;
            message.Message = text;// txtMessage.Text;
            message.Timestamp = DateTime.Now.Add(Config.Misc.TimeOffset);
            VideoBroadcast.AddMessage(new Guid(videoGuid), message);
            //txtMessage.Text = String.Empty;            
        }
        #endregion
    }
}