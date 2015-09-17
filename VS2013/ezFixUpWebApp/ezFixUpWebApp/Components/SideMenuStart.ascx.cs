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


namespace ezFixUp.Components
{
    public partial class SideMenuStart : UserControl
    {
        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        protected string cssClass = "SideMenu";
        public string CssClass{set{cssClass = value;}}

        protected string borderStyle = "none";
        public string BorderStyle { set { borderStyle = value; } }

        protected void Page_Load(object sender, EventArgs e)
        {
            pnlHeader.Visible = (lblTitle.Text.Length > 0);
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