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
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using ezFixUp.Components;

namespace ezFixUp
{
    public partial class FAQs : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
                SetActiveLinkButton(lnkCategory1);
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lnkCategory1.Click += new EventHandler(this.lnkCategory_Click);
            this.lnkCategory2.Click += new EventHandler(this.lnkCategory_Click);
            this.lnkCategory3.Click += new EventHandler(this.lnkCategory_Click);
        }

        #endregion

        private void LoadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("FAQs Categories");
            lnkCategory1.Text = Lang.Trans("FAQ 1");
            lnkCategory2.Text = Lang.Trans("FAQ 2");
            lnkCategory3.Text = Lang.Trans("FAQ 3");
        }

        private void SetActiveLinkButton(LinkButton lbActive)
        {
            lnkCategory1.CssClass = "SideMenuLink";
            lnkCategory2.CssClass = "SideMenuLink";
            lnkCategory3.CssClass = "SideMenuLink";
            lbActive.CssClass = "SideMenuLink active";
        }

        private void lnkCategory_Click(object sender, EventArgs e)
        {
            if (sender == null || !(sender is LinkButton))
                return;

            var lb = sender as LinkButton;
            SetActiveLinkButton(lb);
            
            

        }
    }
}
