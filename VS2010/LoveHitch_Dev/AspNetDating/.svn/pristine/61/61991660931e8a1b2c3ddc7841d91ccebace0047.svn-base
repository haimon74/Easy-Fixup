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

namespace AspNetDating.Components.Profile
{
    /// <summary>
    ///		Summary description for RecordVideo.
    /// </summary>
    public partial class RecordVideo : UserControl
    {
        protected VideoRecorder VideoRecorder1;
        protected HeaderLine HeaderLine1;

        private bool IsPrivate
        {
            get
            {
                bool? isPrivate = VideoProfile.IsPrivate(this.User.Username);

                if (!isPrivate.HasValue || isPrivate.Value == false)
                    return false;
                else
                    return true;
            }

            set
            {
                VideoProfile.Save(this.User.Username, value);
            }
        }
        
        private User user;

        public User User
        {
            set
            {
                user = value;
                if (user != null)
                {
                    ViewState["Username"] = user.Username;
                }
                else
                    ViewState["Username"] = null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HeaderLine1.Title = Lang.Trans("Record Video");
            lnkSetPrivate.Text = Lang.Trans("Set Private");
            lnkSetPublic.Text = Lang.Trans("Set Public");            
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            VideoRecorder1.strTargetUsername = User.Username;
            lnkSetPrivate.Enabled = !IsPrivate;
            lnkSetPublic.Enabled = IsPrivate;            
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

        protected void lnkSetPublic_Click(object sender, EventArgs e)
        {
            IsPrivate = false;
        }

        protected void lnkSetPrivate_Click(object sender, EventArgs e)
        {
            IsPrivate = true;
        }
    }
}