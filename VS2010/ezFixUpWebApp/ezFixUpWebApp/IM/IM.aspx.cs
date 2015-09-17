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
    /// Summary description for IM.
    /// </summary>
    public partial class IM : PageBase
    {
        public IM()
        {
            RequiresAuthorization = false;
            DoNotRegisterIMJavascript = true;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (strUserID == null || strUserRealName == null || strSex == null || strAge == null ||
                strLocation == null || strTargetUserID == null || strTargetUserRealName == null ||
                CurrentUserSession == null || CurrentUserSession.Username != strUserID)
            {
                Response.Clear();
                Response.Write("<script type=\"text/javascript\">window.close();</script>");
                return;
            }

            // check to see if there is already a request to open a window in the db
            if (isInitiator)
            {
                if (!isUserBlocked)
                {
                    bool exists = InstantMessenger.OpenWindowRequestExists(strUserID, strTargetUserID);

                    if (!exists)
                    {
                        // if not, insert a request to have a window opened up on the target user's machine
                        InstantMessenger.CreateOpenWindowRequest(strUserID, strTargetUserID);
                    }
                }
                else
                {
                    Response.Clear();
                    Response.Write(Lang.Trans("You have been blocked!"));
                    Response.Flush();
                    return;
                }
            }
            else
            {
                InstantMessenger.DeleteOpenWindowRequest(strTargetUserID, strUserID);
            }
        }

        protected bool isInitiator
        {
            get
            {
                string initiator = Request.QueryString["initiator"];
                if (initiator != null && initiator == "1")
                    return true;
                else
                    return false;
            }
        }

        protected string strSessID
        {
            get { return Session.SessionID; }
        }

        protected string strUserID
        {
            get { return Request.QueryString["myName"]; }
        }

        protected string strUserRealName
        {
            get { return Request.QueryString["myRealName"]; }
        }

        protected string strSex
        {
            get
            {
                if (Request.QueryString["chatToSex"] != null)
                    return Lang.Trans(Request.QueryString["chatToSex"]);
                else
                {
                    Response.Clear();
                    Response.Write("Flash Update Completed. Please close this window and try the chat again");
                    Response.Write("<script type=\"text/javascript\">window.close();</script>");
                    Response.Flush();
                    Response.Close();
                    return string.Empty;
                }
            }
        }

        protected string strAge
        {
            get { return Request.QueryString["chatToAge"]; }
        }

        protected string strLocation
        {
            get { return Request.QueryString["chatToLocation"]; }
        }

        protected string strTargetUserID
        {
            get { return Request.QueryString["chatToUsername"]; }
        }

        protected string strTargetUserRealName
        {
            get { return Request.QueryString["userRealName"]; }
        }

        protected bool isUserBlocked
        {
            get { return Classes.User.IsUserBlocked(strTargetUserID, strUserID); }
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