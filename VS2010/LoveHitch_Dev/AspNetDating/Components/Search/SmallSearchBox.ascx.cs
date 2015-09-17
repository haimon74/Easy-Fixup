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
using AspNetDating.Classes;

namespace AspNetDating.Components.Search
{
    /// <summary>
    ///		Summary description for SmallSearchBox.
    /// </summary>
    public partial class SmallSearchBox : UserControl
    {
        protected SmallBoxStart SmallBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
            if (!Page.IsPostBack)
            {
                LoadStrings();
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
        }

        #endregion

        private void LoadStrings()
        {
            if (SmallBoxStart1 != null)
                SmallBoxStart1.Title = Lang.Trans("Quick Search");

            txtAgeFrom.Text = Config.Users.MinAge.ToString();
            txtAgeTo.Text = Config.Users.MaxAge.ToString();

            dropGender.Items.Add(
                new ListItem(Lang.Trans("Male"), ((int) User.eGender.Male).ToString()));
            dropGender.Items.Add(
                new ListItem(Lang.Trans("Female"), ((int) User.eGender.Female).ToString()));
            if (Config.Users.CouplesSupport)
            {
                dropGender.Items.Add(
                    new ListItem(Lang.Trans("Couple"), ((int) User.eGender.Couple).ToString()));
            }
            btnBasicSearchGo.Text = Lang.Trans("Search");
        }

        protected void btnBasicSearchGo_Click(object sender, EventArgs e)
        {
            BasicSearch search = new BasicSearch();

            if (Config.Users.RequireProfileToShowInSearch)
            {
                search.HasAnswer = true;
            }
            else
            {
                search.hasAnswer_isSet = false;
            }

            search.Gender = (User.eGender)
                            Convert.ToInt32(dropGender.SelectedValue);
            try
            {
                search.MinAge = Convert.ToInt32(txtAgeFrom.Text);
                search.MaxAge = Convert.ToInt32(txtAgeTo.Text);
            }
            catch
            {
            }
            search.HasPhoto = cbPhotoReq.Checked;

            Session["BasicSearchRequest"] = search;
            Response.Redirect("Search.aspx");
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}