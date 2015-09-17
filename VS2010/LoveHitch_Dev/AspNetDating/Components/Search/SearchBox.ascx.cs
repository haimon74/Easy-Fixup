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
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetDating.Components.Search
{
    public partial class SearchBox : UserControl
    {
        protected SearchResults SearchResults;
        protected Button btnBasicSearchGo;
        protected LargeBoxStart LargeBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
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

        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}