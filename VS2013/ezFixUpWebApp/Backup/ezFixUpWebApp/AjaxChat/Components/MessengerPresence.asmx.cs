﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace Estream.AjaxChat.Components
{
    /// <summary>
    /// Summary description for WebService1
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class MessengerPresenceService : System.Web.Services.WebService
    {
        [ScriptMethod]
        [WebMethod]
        public void UpdateMessengerPresence()
        {
            return;
        }
    }
}