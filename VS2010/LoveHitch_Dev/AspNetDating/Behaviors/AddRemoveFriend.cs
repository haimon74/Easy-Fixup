using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetDating.Classes;

namespace AspNetDating.Behaviors
{
    public class AddRemoveFriend : ISocialConnector
    {

        #region ISocialConnector Members

        public string Add(UserSession userSession, string username)
        {
            string resultString = "";
            User.eAddFriendResult result = userSession.AddToFriends(username);
            switch (result)
            {
                case Classes.User.eAddFriendResult.eAlreadyAdded:
                    resultString = Lang.Trans("The user you selected already exists in your friend list");
                    break;
                case Classes.User.eAddFriendResult.eInvalidUsername:
                    resultString = Lang.Trans("No such user!");
                    break;
                case Classes.User.eAddFriendResult.eMaximumFriendsReached:
                    resultString =
                        String.Format(
                            Lang.Trans(
                                "The maximum friends of {0} is reached! Please remove someone and try again!"),
                            Config.Users.MaxFriendUsers);
                    break;
                case Classes.User.eAddFriendResult.eSuccess:
                    if (Classes.User.IsUserInFriendList(username, userSession.Username))
                    {
                        resultString = String.Format(Lang.Trans("{0} has been added to your friend list"), username);
                        AddNewFriendFriendEvent(username, userSession.Username);
                        AddNewFriendFriendEvent(userSession.Username, username);
                    }
                    else
                    {
                        resultString = String.Format(Lang.Trans("Friendship request has been sent to {0}"), username);
                    }

                    if (Config.Users.NewEventNotification)
                    {
                        int imageID = 0;
                        User user = null;
                        try
                        {
                            user = Classes.User.Load(username);
                        }
                        catch (NotFoundException) { break; }

                        try
                        {
                            imageID = Photo.GetPrimary(userSession.Username).Id;
                        }
                        catch (NotFoundException)
                        {
                            imageID = ImageHandler.GetPhotoIdByGender(userSession.Gender);
                        }
                        string text = String.Format("User {0} added you to friends".Translate(),
                                              "<b>" + userSession.Username + "</b>");

                        string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                        Classes.User.SendOnlineEventNotification(userSession.Username, user.Username, text,
                                                                 thumbnailUrl,
                                                                 UrlRewrite.CreateShowUserUrl(
                                                                     userSession.Username));
                    }
                    break;
            }
            return resultString;
        }

        public string Remove(Classes.UserSession userSession, string username)
        {
            User user = null;
            string resultString = "";
            try
            {
                user = Classes.User.Load(username);
                bool isFriendship1 = Classes.User.IsUserInFriendList(username, userSession.Username);
                bool isFriendship2 = userSession.IsUserInFriendList(username);

                if (isFriendship1 || isFriendship2)
                {
                    if (isFriendship2)
                    {
                        userSession.RemoveFromFriends(username);
                        resultString = String.Format(Lang.Trans("{0} has been removed from your friend list"), username);
                    }
                    else
                    {
                        resultString = String.Format(Lang.Trans("Friendship request from {0} has been denied"), username);
                    }

                    if (isFriendship1)
                        user.RemoveFromFriends(userSession.Username);

                    if (isFriendship1 && isFriendship2)
                    {
                        AddRemovedFriendFriendEvent(username, userSession.Username);
                        AddRemovedFriendFriendEvent(userSession.Username, username);
                    }

                }
                else
                {
                    resultString = Lang.Trans("No such friend!");
                }
            }
            catch (NotFoundException)
            {
                resultString = Lang.Trans("No such user!");
            }
            return resultString;
        }

        #endregion



        private void AddNewFriendFriendEvent(string username, string favoriteUsername)
        {
            Event newEvent = new Event(username);

            newEvent.Type = Event.eType.NewFriendFriend;
            NewFriendFriend newFriendFriend = new NewFriendFriend();
            newFriendFriend.Username = favoriteUsername;
            newEvent.DetailsXML = Misc.ToXml(newFriendFriend);

            newEvent.Save();

            if (Config.Users.NewEventNotification)
            {
                string[] usernames = Classes.User.FetchMutuallyFriends(username);

                string text = String.Format("{0} and {1} are now friends".Translate(),
                                              "<b>" + username + "</b>", favoriteUsername);
                int imageID = 0;
                try
                {
                    imageID = Photo.GetPrimary(username).Id;
                }
                catch (NotFoundException)
                {
                    User user = null;
                    try
                    {
                        user = Classes.User.Load(username);
                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    catch (NotFoundException) { return; }
                }
                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);

                foreach (string friendUsername in usernames)
                {
                    if (favoriteUsername == friendUsername) continue;

                    Classes.User.SendOnlineEventNotification(username, friendUsername, text, thumbnailUrl,
                                                         UrlRewrite.CreateShowUserUrl(favoriteUsername));
                }
            }
        }

        private void AddRemovedFriendFriendEvent(string username, string favoriteUsername)
        {
            Event newEvent = new Event(username);

            newEvent.Type = Event.eType.RemovedFriendFriend;
            RemovedFriendFriend newFriendFriend = new RemovedFriendFriend();
            newFriendFriend.Username = favoriteUsername;
            newEvent.DetailsXML = Misc.ToXml(newFriendFriend);

            newEvent.Save();

            if (Config.Users.NewEventNotification)
            {
                string[] usernames = Classes.User.FetchMutuallyFriends(username);

                string text = String.Format("{0} {1} are no longer friends".Translate(),
                                              "<b>" + username + "</b>", favoriteUsername);
                int imageID = 0;
                try
                {
                    imageID = Photo.GetPrimary(username).Id;
                }
                catch (NotFoundException)
                {
                    User user = null;
                    try
                    {
                        user = Classes.User.Load(username);
                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    catch (NotFoundException) { return; }
                }
                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);

                foreach (string friendUsername in usernames)
                {
                    if (favoriteUsername == friendUsername) continue;

                    Classes.User.SendOnlineEventNotification(username, friendUsername, text, thumbnailUrl,
                                                         UrlRewrite.CreateShowUserUrl(favoriteUsername));
                }
            }
        }
    }
}