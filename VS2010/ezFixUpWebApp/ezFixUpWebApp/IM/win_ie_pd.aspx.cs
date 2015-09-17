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
using ezFixUp.Classes;

namespace ezFixUp.IM
{
    /// <summary>
    /// Summary description for win_ie_pd.
    /// </summary>
    public partial class win_ie_pd_ : PageBase
    {
        public win_ie_pd_()
        {
            DoNotRegisterIMJavascript = true;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            // This page is only used by Windows IE so that page refreshes do not make a clicking sound.  It
            // checks your database to see if your users have any WM windows that need to be launched
            // Also, it updates your database telling your site that the user is currently on the site

            //This will need to be set to the userID of the currently logged in member
            String strUserID = CurrentUserSession == null ? null : CurrentUserSession.Username;

            // clear out any old values (that were requested over 15 monutes ago)
            InstantMessenger.DeleteOldOpenWindowRequests();

            bool bFoundPendingWMs = false;

            if (strUserID != null)
            {
                // update your database so that everyone knows this user is online right now
                //$query = "UPDATE users SET lastTimeOnline = getdate() WHERE userID = " + strUserID;	
                CurrentUserSession.UpdateLastOnline(false);

                // select a list of users who want to talk with the current user and we haven't opened a window for 5 mins
                //$query = "SELECT originatingUserID FROM im_pending_wm WHERE destinationUserID = " + strUserID + " AND ( openedWindowAt IS NULL OR dateDiff( minute, openedWindowAt, getdate() ) >= 5 )";
                // execute strQuery against your database
                string[] users = InstantMessenger.FetchPendingUsers(strUserID);
                bFoundPendingWMs = users.Length > 0;
            }

            if (bFoundPendingWMs)
            {
                // we need to launch WM windows so redirect to an image with a width of 2
                Response.Redirect("images/2PixelImage.jpg", true);
            }
            else
            {
                Response.Redirect("images/1PixelImage.jpg", true);
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}