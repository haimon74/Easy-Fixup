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

namespace AspNetDating.Admin
{
    /// <summary>
    ///		Summary description for MessageBox.
    /// </summary>
    public partial class MessageBox : UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
        }


        private string cssClass;

        public string CssClass
        {
            get { return cssClass; }
            set { cssClass = value; }
        }

        public void Show(string message, Classes.Misc.MessageType messageType)
        {
            divMessage.InnerHtml = message;
            switch (messageType)
            {
                case Classes.Misc.MessageType.Error:
                    divMessage.Attributes.Add("class", "msgboxerror");
                    break;
                case Classes.Misc.MessageType.Success:
                    divMessage.Attributes.Add("class", "msgboxsuccess");
                    break;
            }
            this.Focus();
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