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

namespace ezFixUp.Components.Profile
{
    /// <summary>
    ///		Summary description for VideoRecorder.
    /// </summary>
    public partial class VideoRecorder : UserControl
    {
        protected string RootPath
        {
            get
            {
                return
                    Request.Url.GetLeftPart(UriPartial.Authority) + Request.ApplicationPath +
                    ((Request.ApplicationPath != "/") ? "/" : "");
            }
        }

        protected UserSession CurrentUserSession
        {
            get
            {
                if (Page as PageBase != null)
                    return ((PageBase) Page).CurrentUserSession;
                else
                    return null;
            }
        }

        protected string strSessID
        {
            get
            {
                if (CurrentUserSession != null)
                    return CurrentUserSession.LastSessionID;
                else
                    return String.Empty;
            }
        }

        protected string strUserID
        {
            get
            {
                if (adminUser)
                {
                    //hardcoded admin username from Users table
                    return "admin";
                }
                else
                {
                    if (CurrentUserSession != null)
                        return CurrentUserSession.Username;
                    else
                        return String.Empty;
                }
            }
        }

        private bool adminUser = false;

        public bool AdminUser
        {
            set { adminUser = value; }
        }

        private string targetUsername = null;

        public string strTargetUsername
        {
            get
            {
                if (targetUsername == null)
                    throw new Exception("Target username is not initialized!");
                return targetUsername;
            }

            set { targetUsername = value; }
        }

        private bool render = true;

        public bool bRender
        {
            get { return render; }
            set { render = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}