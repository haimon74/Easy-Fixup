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
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    ///		Summary description for AdminHeader.
    /// </summary>
    public partial class AdminHeader : UserControl
    {

        protected int PendingPhotos
        {
            get
            {
                Photo[] photos = null;
                if (Config.CommunityFaceControlSystem.EnableCommunityFaceControl)
                    photos = Photo.FetchNonApproved(true);
                else photos = Photo.FetchNonApproved();
                if (photos == null)
                    return 0;
                else return photos.Length;
            }
        }

        protected int PendingAnswers
        {
            get
            {
                return ProfileAnswer.FetchNonApproved().Length;
            }
        }

        protected int NewUsersForTheLast24hours
        {
            get
            {
                NewUsersSearch nuSearch = new NewUsersSearch();
                nuSearch.ProfileReq = false;
                nuSearch.UsersSince = DateTime.Now.Subtract(new TimeSpan(24, 0, 0));
                UserSearchResults nuResults = nuSearch.GetResults();

                if (nuResults == null)
                    return 0;
                else return nuResults.Usernames.Length;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareControls();
        }

        private void PrepareControls()
        {
            if (Page is AdminPageBase &&
                ((AdminPageBase) Page).CurrentAdminSession != null)
            {
                lblWelcome.Text = String.Format(
                    Lang.TransA("Welcome <b>{0}</b>"),
                    ((AdminPageBase) Page).CurrentAdminSession.Username);
                lnkLogout.Text = Lang.TransA("Logout");
                pnlLogout.Visible = true;
            }
            else
            {
                pnlLogout.Visible = false;
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lnkLogout.Click += new EventHandler(lnkLogout_Click);
        }

        #endregion

        private void lnkLogout_Click(object sender, EventArgs e)
        {
            ((AdminPageBase) Page).CurrentAdminSession = null;
            Session["temp_photos"] = null;
            Response.Redirect("Login.aspx");
        }
    }
}