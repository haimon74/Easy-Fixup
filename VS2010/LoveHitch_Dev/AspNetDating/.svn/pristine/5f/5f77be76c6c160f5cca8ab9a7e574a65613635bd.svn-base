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

namespace AspNetDating.Components
{
    public partial class HeaderLine : UserControl
    {

        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public string CssClass
        {
            set
            {
                lblTitle.CssClass = value;
            }
        }

        public bool IsLinkButtonVisible { get { return lbEdit.Visible; } set { lbEdit.Visible = value; } }
        public string LinkButtonText { get { return lbEdit.Text; } set { lbEdit.Text = value; } }
        public string LinkButtonToolTip { get { return lbEdit.ToolTip; } set { lbEdit.ToolTip = value; } }

        //public delegate void LinkButtonCallbackDelegate(string topicName);
        //public LinkButtonCallbackDelegate LinkButtonCallbackFunction;
        //public delegate void LinkButtonEventDelegate(object sender, EventArgs e);
        //public EventHandler LinkButtonEventCallback { get; set; }
        //public event LinkButtonEventDelegate LinkButtonClickEventHandler;

        public delegate void LinkButtonCallbackDelegate(string topicName);
        public LinkButtonCallbackDelegate LinkButtonClickCallback { set; get; }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (Page.Master != null && lbEdit.Visible)
            //{
            //    //((Site)Page.Master).ScriptManager.RegisterAsyncPostBackControl(this);
            //    ((Site)Page.Master).ScriptManager.RegisterAsyncPostBackControl(lbEdit);
            //    if (Page is ManageMemberProfile)
            //    {
            //        //lbEdit.Click += LinkButtonEventCallback;
            //        var trigger = new AsyncPostBackTrigger {ControlID = lbEdit.UniqueID, EventName = "Click"};
            //        ((ManageMemberProfile)Page).UpdatePanel1.Triggers.Add(trigger);
            //    }
            //}
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

        protected void LinkButtonClicked(object sender, EventArgs e)
        {
            LinkButtonClickCallback(lblTitle.Text);
            //if (LinkButtonClickEventHandler != null)
            //{
            //    LinkButtonClickEventHandler(sender, e);
            //    LinkButtonCallbackFunction(lblTitle.Text.Trim());
            //}
        }
    }
}