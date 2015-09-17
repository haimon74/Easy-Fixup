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

namespace ezFixUp.Admin
{
    /// <summary>
    /// Summary description for EditProfile.
    /// </summary>
    public partial class EditProfile : AdminPageBase
    {
        protected EditProfileCtrl EditProfileCtrl1;
//		protected ezFixUp.Admin.EditPhotosCtrl EditPhotosCtrl1;
//		protected System.Web.UI.WebControls.LinkButton lnkEditProfile;
//		protected System.Web.UI.WebControls.LinkButton lnkEditPhotos;
        protected User user;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Params["username"] != null)
                {
                    user = Classes.User.Load(Request.Params["username"]);
                    EditProfileCtrl1.User = user;
                }
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.userAccounts;
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