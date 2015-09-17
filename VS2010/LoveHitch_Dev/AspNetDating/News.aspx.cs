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
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;
using AspNetDating.Components;
using News=AspNetDating.Classes.News;

namespace AspNetDating
{
    /// <summary>
    /// Summary description for News.
    /// </summary>
    public partial class NewsPage : PageBase
    {
        protected LargeBoxStart LargeBoxStart1;

        public NewsPage()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.Params["id"];

                if (id == null || id == "")
                {
                    return;
                }

                try
                {
                    int newsID = Convert.ToInt32(id);
                    ShowNews(newsID);
                }
                catch (Exception)
                {}

                HtmlLink rssNews = new HtmlLink();
                rssNews.Attributes.Add("rel", "alternate");
                rssNews.Attributes.Add("type", "application/rss+xml");
                rssNews.Attributes.Add("title", Config.Misc.SiteTitle + " - " + Lang.Trans("News"));
                rssNews.Attributes.Add("href", "Rss.ashx?action=news");
                Header.Controls.Add(rssNews);
            }
        }

        private void ShowNews(int id)
        {
            int languageId = 1;
            if (Page is PageBase)
            {
                languageId = ((PageBase) Page).LanguageId;
            }
            News news = News.Fetch(id, languageId);
            if (news == null) return;
            divNewsContent.InnerHtml = news.Text;
            LargeBoxStart1.Title = news.Title;
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}