using System;
using System.Web.UI;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    ///		Summary description for DeleteButton.
    /// </summary>
    public partial class DeleteButton : UserControl
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

        protected AdminSession CurrentAdminSession
        {
            get
            {
                if (Page as AdminPageBase != null)
                    return ((AdminPageBase) Page).CurrentAdminSession;
                else
                    return null;
            }
        }

        protected string strSessID
        {
            get
            {
                if (CurrentAdminSession != null)
                    return CurrentAdminSession.LastSessionID;
                else
                    return String.Empty;
            }
        }

        protected string strUserID
        {
            get
            {
                if (CurrentAdminSession != null)
                    return CurrentAdminSession.Username;
                else
                    return String.Empty;
            }
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