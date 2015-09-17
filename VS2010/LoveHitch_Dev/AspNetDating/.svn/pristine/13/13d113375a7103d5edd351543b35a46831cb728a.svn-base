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
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    public partial class ContactUs : PageBase
    {
        protected SideMenuStart SmallBoxStart1;
        protected LargeBoxStart LargeBoxStart1;
        protected LargeBoxStart LargeBoxStart2;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
            }
        }

        private void LoadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("More Information");
            lnkFAQ.Text = Lang.Trans("FAQs");
            var items = ddlCategory.Items;
            foreach (ListItem li in items)
            {
                li.Text = li.Text.Translate();
            }
            btnSend.Text = "Send".Translate();
            LargeBoxStart1.Title = "Contact Us".Translate();
            //lnkAboutUs.Text = Lang.Trans("About Us");
            //lnkPrivacyPolicy.Text = Lang.Trans("Privacy Policy");
            //lnkTermsOfUse.Text = Lang.Trans("Terms of Use");
            //lnkMore.Text = Lang.Trans("Other");
        }

        private bool IsFullyFilled()
        {
            return txtName.Text.IsNotNullOrEmpty() 
                && txtEmail.Text.IsNotNullOrEmpty() 
                && txtContent.Text.IsNotNullOrEmpty();
        }
        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (IsFullyFilled())
            {
                Classes.ContactUs contactUs = new Classes.ContactUs(HttpUtility.HtmlEncode(txtName.Text),
                                                                    HttpUtility.HtmlEncode(txtEmail.Text),
                                                                    (Classes.ContactUs.ContactType)
                                                                    ddlCategory.SelectedIndex);
                contactUs.DateReported = DateTime.Now;
                contactUs.Report = HttpUtility.HtmlEncode(txtContent.Text);
                contactUs.Save();
                ((PageBase)Page).StatusPageMessage = Lang.Trans("Your message has been sent successufuly!");
                Response.Redirect("ShowStatus.aspx");
            }
            else
            {
                Visible = true;
                lblError.Text = Lang.Trans("Please fill in missing text field!");
                lblError.Visible = true;
                return;
            }
        }
    }
}
