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
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    /// <summary>
    ///		Summary description for News.
    /// </summary>
    public partial class News : UserControl
    {
        protected SmallBoxStart SmallBoxStart1;

        #region Properties

        /// <summary>
        /// Gets or sets the number of news to show.
        /// </summary>
        public int Count
        {
            get
            {
                if (ViewState["NumberOfNews"] == null)
                {
                    return Config.Misc.NumberOfNews;
                }
                else return (int) ViewState["NumberOfNews"];
            }
            set { ViewState["NumberOfNews"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (SmallBoxStart1 != null)
                SmallBoxStart1.Title = Lang.Trans("News");

            LoadNews();

            base.OnPreRender(e);
        }


        private void LoadNews()
        {
            int languageId = 1;
            if (Page is PageBase)
            {
                languageId = ((PageBase) Page).LanguageId;
            }

            DataView dvNews = new DataView
                (Classes.News.FetchAsTable(Count, languageId));

            if (dvNews.Table.Rows.Count != 0)
            {
                rptNews.DataSource = dvNews;
                rptNews.DataBind();
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
    }
}