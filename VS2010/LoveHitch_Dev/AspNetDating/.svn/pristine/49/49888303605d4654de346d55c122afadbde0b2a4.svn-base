using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetDating.Classes;

namespace AspNetDating.Behaviors
{
    public class AddRemoveBlock : ISocialConnector
    {

        #region ISocialConnector Members

        public string Add(UserSession userSession, string username)
        {
            string resultString = "";
            if (!userSession.IsUserBlocked(username))
            {
                userSession.BlockUser(username);
                resultString = String.Format(Lang.Trans("{0} has been added to your blocked list"), username);
            }
            else
            {
                resultString = Lang.Trans("Already in blocked list!");
            }
            
            return resultString;
        }

        public string Remove(Classes.UserSession userSession, string username)
        {
            string resultString = "";
            if (userSession.IsUserBlocked(username))
            {
                userSession.BlockUser(username);
                resultString = String.Format(Lang.Trans("{0} has been removed from your blocked list"), username);
            }
            else
            {
                resultString = Lang.Trans("This user is not in your blocked list!");
            }

            return resultString;
        }

        #endregion

    }
}