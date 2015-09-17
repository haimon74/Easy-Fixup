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
using System.IO;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class BroadcastVideo : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude("FlashInterop", "scripts/FlashInterop.js");
            string scriptText;

            using (TextReader reader = new StreamReader(Server.MapPath("~").TrimEnd('/') + "/scripts/FlashInteropIE.js"))
            {
                scriptText = reader.ReadToEnd();
            }

            ClientScript.RegisterClientScriptBlock(GetType(), "FlashInteropIE", scriptText, false);

            LoadStrings();

            btnStartBroadcast.Attributes.Add("onclick", 
                "window.open('BroadcastVideoWindow.aspx', 'broadcastvideo', " + 
                "'width=640,height=500,resizable=0,menubar=0,status=0,toolbar=0'); return false;");
        }

        private void LoadStrings()
        {
            cvBroadcastVideoNotes.Text = String.Format("<div class=\"separator\"></div>{0}",
                "Broadcast video notes".Translate());
            WideBoxStart1.Title = Lang.Trans("Live Video Broadcast");
            btnStartBroadcast.Text = Lang.Trans("Start Broadcast");
        }
    }
}