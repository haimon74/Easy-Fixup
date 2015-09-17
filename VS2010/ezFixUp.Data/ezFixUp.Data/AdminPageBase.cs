/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using ezFixUp.Data;
using eStreamBG.Dating;

namespace ezFixUp.Admin
{
    public class AdminPageBase : InternalAdminPage
    {
        public bool RequiresAuthorization = true;

        private Data.Admin.eAccess privileges = Data.Admin.eAccess.None;
        protected Data.Admin.eAccess Privileges
        {
            set
            {
                privileges = value;
            }
        }
        
        protected bool HasReadAccess
        {
            get
            {                
                bool hasReadAccess = true;

                if (CurrentAdminSession.Username != Config.Users.SystemUsername && Config.AdminSettings.AdminPermissionsEnabled)
                {
                    hasReadAccess = (privileges & Data.Admin.eAccess.Read) != 0;
                }
                
                return hasReadAccess;
            }
        }
        
        public bool HasWriteAccess
        {
            get
            {
                if (Config.AdminSettings.ReadOnly)
                    return false;
                
                bool hasWriteAccess = true;

                if (CurrentAdminSession.Username != Config.Users.SystemUsername && Config.AdminSettings.AdminPermissionsEnabled)
                {
                    hasWriteAccess = (privileges & Data.Admin.eAccess.Write) != 0;
                }
                
                return hasWriteAccess;
            }
        }

        public new AdminSession CurrentAdminSession
        {
            get { return base.CurrentAdminSession as AdminSession; }
            set { base.CurrentAdminSession = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (RequiresAuthorization)
            {
                ProcessValidation();
                
                if (!HasReadAccess)
                    Response.Redirect("Home.aspx?err=1");
            }

            base.OnInit(e);
        }

        public string StatusPageMessage
        {
            set { Session["ShowStatus_Message"] = value; }
            get
            {
                if (Session["ShowStatus_Message"] == null)
                    return null;
                else
                    return (string) Session["ShowStatus_Message"];
            }
        }

        /// <summary>
        /// Gets or sets the type of the status page message.
        /// </summary>
        /// <value>The type of the status page message.</value>
        public Misc.MessageType? StatusPageMessageType
        {
            set { Session["ShowStatus_MessageType"] = value; }
            get
            {
                if (Session["ShowStatus_MessageType"] == null)
                    return null;
                else
                    return (Misc.MessageType)Session["ShowStatus_MessageType"];
            }
        }

        protected void ProcessValidation()
        {
            if (!IsUserAuthorized())
            {
                Response.Redirect("Login.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }
        }

        public bool IsUserAuthorized()
        {
            if (CurrentAdminSession == null)
                return false;

            return CurrentAdminSession.IsAuthorized;
        }

        public void Log(Exception ex)
        {
            ExceptionLogger.Log(Request, ex);
        }
    }
}