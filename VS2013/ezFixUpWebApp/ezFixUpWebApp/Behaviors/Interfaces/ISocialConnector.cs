using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ezFixUp.Behaviors
{
    interface ISocialConnector
    {
        string Add(Classes.UserSession userSession, string username);
        string Remove(Classes.UserSession userSession, string username);
    }
}
