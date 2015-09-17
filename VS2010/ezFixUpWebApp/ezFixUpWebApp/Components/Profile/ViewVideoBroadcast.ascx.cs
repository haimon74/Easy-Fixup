/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Web.UI;
using ezFixUp.Classes;

namespace ezFixUp.Components.Profile
{
    public partial class ViewVideoBroadcast : UserControl
    {
        private User user;

        public User User
        {
            set
            {
                user = value;
                if (user != null)
                {
                    ViewState["Username"] = user.Username;
                }
                else
                    ViewState["Username"] = null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        private Guid? videoGuid;

        public Guid VideoGuid
        {
            get
            {
                if (!videoGuid.HasValue)
                    videoGuid = ViewState["ViewBroadcast_Guid"] as Guid?;
                return videoGuid.Value;
            }
            set
            {
                videoGuid = value;
                ViewState["ViewBroadcast_Guid"] = videoGuid.Value;
            }
        }

        public string FlashServer
        {
            get
            {
                return Config.Misc.FlashServerForVideoBroadcast;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            ((Site)(Page.Master.Master ?? Page.Master)).ScriptManager.CompositeScript.Scripts.Add(
                new ScriptReference("scripts/VideoBroadcast.js"));
        }
    }
}