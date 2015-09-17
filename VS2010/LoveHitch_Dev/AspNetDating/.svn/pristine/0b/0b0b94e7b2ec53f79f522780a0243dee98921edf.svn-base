using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Caching;
using System.Xml.Serialization;

namespace AspNetDating.Classes
{
    public class RealtimeNotification
    {
        public string Type
        {
            get
            {
                return GetType().Name;
            }
        }
        public string Recipient { get; set; }
        public string Text { get; set; }

        public static void SendNotification(RealtimeNotification notification)
        {
            List<RealtimeNotification> notifications;
            string cacheKey = "RealtimeNotifications_" + notification.Recipient.ToLower();
            if (HttpContext.Current.Cache[cacheKey] is List<RealtimeNotification>)
                notifications = (List<RealtimeNotification>)HttpContext.Current.Cache[cacheKey];
            else
                notifications = new List<RealtimeNotification>();

            notifications.Add(notification);

            HttpContext.Current.Cache.Insert(cacheKey, notifications, null, Cache.NoAbsoluteExpiration,
                                             TimeSpan.FromMinutes(Config.Users.OnlineCheckTime * 2));
        }

        public static List<RealtimeNotification> RetrieveNotifications(string recipient)
        {
            List<RealtimeNotification> notifications = null;
            string cacheKey = "RealtimeNotifications_" + recipient.ToLower();
            if (HttpContext.Current.Cache[cacheKey] is List<RealtimeNotification>)
            {
                notifications = (List<RealtimeNotification>)HttpContext.Current.Cache[cacheKey];
                HttpContext.Current.Cache.Remove(cacheKey);
            }
            return notifications;
        }
    }

    [XmlInclude(typeof(NewMessageNotification))]
    public class GenericEventNotification : RealtimeNotification
    {
        public string Sender { get; set; }
        public string ThumbnailUrl { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class NewMessageNotification : GenericEventNotification
    {
        public int MessageId { get; set; }

        public static NewMessageNotification FromMessage(Message message)
        {
            int imageID;
            try
            {
                imageID = message.FromUser.GetPrimaryPhoto().Id;
            }
            catch (NotFoundException)
            {
                imageID = ImageHandler.GetPhotoIdByGender(message.FromUser.Gender);
            }
            var notification = new NewMessageNotification
            {
                MessageId = message.Id,
                Sender = message.fromUsername,
                Recipient = message.toUsername,
                ThumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true),
                Text = String.Format("You have a new message from <b>{0}</b>!".Translate(),
                    message.fromUsername),
                RedirectUrl = Config.Urls.Home + "/ShowMessage.aspx?mid=" + message.Id
            };
            return notification;
        }
    }

    public class AccountDeletedNotification : RealtimeNotification
    {
    }
}