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
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    ///		Summary description for AdminMenu.
    /// </summary>
    public partial class AdminMenu : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowHideMenuLinks();
            }
        }

        public void ShowHideMenuLinks()
        {
            /*
             * if (Config.Users.MessageVerificationEnabled)
                pnlSpamCheck.Visible = true;
            else
                pnlSpamCheck.Visible = false;

            if (Config.Misc.EnableVideoProfile)
                pnlApproveVideos.Visible = true;
            else
                pnlApproveVideos.Visible = false;

            if (Config.AbuseReports.UserCanReportMessageAbuse ||
                Config.AbuseReports.UserCanReportPhotoAbuse ||
                Config.AbuseReports.UserCanReportProfileAbuse)
                pnlAbuseReports.Visible = true;
            else 
                pnlAbuseReports.Visible = false;
             */
            pnlApproveSalutePhotos.Visible = Config.Photos.EnableSalutePhoto;
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