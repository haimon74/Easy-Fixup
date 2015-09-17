using System;
using ezFixUp.Classes;

namespace ezFixUp.Behaviors
{
    public class AddRemoveFavorite : ISocialConnector
    {

        #region ISocialConnector Members

        public string Add(UserSession userSession, string username)
        {
            string resultString = "";
            User.eAddFavouriteResult result =
                    userSession.AddToFavourites(username);
            switch (result)
            {
                case Classes.User.eAddFavouriteResult.eAlreadyAdded:
                    resultString = Lang.Trans("The user you selected already exists in your favourite list");
                    break;
                case Classes.User.eAddFavouriteResult.eInvalidUsername:
                    resultString = Lang.Trans("No such user!");
                    break;
                case Classes.User.eAddFavouriteResult.eMaximumFavouritesReached:
                    resultString =
                        String.Format(
                            Lang.Trans(
                                "The maximum favourite users of {0} is reached! Please remove someone and try again!"),
                            Config.Users.MaxFavouriteUsers);
                    break;
                case Classes.User.eAddFavouriteResult.eSuccess:
                    resultString = String.Format(Lang.Trans("{0} has been added to your favourite list"), username);

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
                if (userSession.IsUserInFavouriteList(username))
                {
                    userSession.RemoveFromFavourites(username);

                    resultString = String.Format(Lang.Trans("{0} has been removed from your favourite list"), username);
                }
                else
                {
                    resultString = Lang.Trans("No such user!");
                }
            }
            catch (NotFoundException)
            {
                resultString = Lang.Trans("No such user!");
            }
            return resultString;
        }

        #endregion

    }
}