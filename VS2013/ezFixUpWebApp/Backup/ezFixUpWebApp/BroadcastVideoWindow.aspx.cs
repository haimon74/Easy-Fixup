/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Web.UI;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class BroadcastVideoWindow : PageBase
    {
        private Guid videoGuid;

        public string VideoGuid
        {
            get
            {
                return videoGuid.ToString();
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
            var service = new ServiceReference("Services/OnlineCheck.asmx");
            ScriptManager.Services.Add(service); 
            Page.ClientScript.RegisterClientScriptInclude("VideoBroadcast", "scripts/VideoBroadcast.js");

            // Add script reference for user preview popup
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "body",
                String.Format("var bodyId = '{0}';", ctl00_body.ClientID), true);
            Page.ClientScript.RegisterClientScriptInclude("UserPreview", "scripts/UserPreview.js");
            Page.ClientScript.RegisterStartupScript(GetType(), "InitializeMouseTracking",
                "InitializeMouseTracking();", true);

            LoadStrings();
            InitializeGuid();
        }

        private void InitializeGuid()
        {
            Guid? guid = VideoBroadcast.GetBroadcast(CurrentUserSession.Username);
            if (guid.HasValue)
            {
                videoGuid = guid.Value;
            }
            else
            {
                videoGuid = Guid.NewGuid();
                VideoBroadcast.AddBroadcast(CurrentUserSession.Username, videoGuid);
            }
        }

        private void LoadStrings()
        {
            Page.Header.Title = "Broadcast Live Video".Translate();
        }

        protected void TimerMessages_Tick(object sender, EventArgs e)
        {
            BindWatchers();
        }

        private void BindWatchers()
        {
            string[] watchers = VideoBroadcast.GetWatchers(videoGuid);

            dlUsers.DataSource = watchers;
            dlUsers.DataBind();            
        }

        protected void dlUsers_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
        {
            if (e.CommandName == "Block")
            {
                CurrentUserSession.BlockUser((string)e.CommandArgument);
                VideoBroadcast.AddUserToBanList((string)e.CommandArgument, videoGuid);
                VideoBroadcast.RemoveWatcher((string)e.CommandArgument, videoGuid);
                BindWatchers();
            }
        }
    }
}