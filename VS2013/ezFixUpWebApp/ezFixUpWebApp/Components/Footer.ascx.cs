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
using System.Collections;
using System.Web;
using System.Web.UI;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    public partial class Footer : UserControl
    {
        private int LanguageID
        {
            get { return ((PageBase) Page).LanguageId; }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected string FacebookConnectedUserId
        {
            get { return (CurrentUserSession != null && CurrentUserSession.FacebookID != null) ? CurrentUserSession.FacebookID.ToString() : String.Empty; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!(Page is _default))
            {
                bvDefaultFooter.Visible = false;
            }

            loadPages();
        }

        private void loadPages()
        {
            ArrayList lContentPages = new ArrayList();

            Classes.ContentPage[] contentPages =
                Classes.ContentPage.FetchContentPages(Convert.ToInt32(LanguageID),
                                                      Classes.ContentPage.eSortColumn.FooterPosition);

            bool loggedOn = CurrentUserSession != null;
            bool isPaid = CurrentUserSession != null && Classes.User.IsPaidMember(CurrentUserSession.Username);

            foreach (Classes.ContentPage contentPage in contentPages)
            {
                if (contentPage.FooterPosition != null)
                {
                    if (
                        ((loggedOn && ((contentPage.VisibleFor & Classes.ContentPage.eVisibility.LoggedOnUsers) != 0 ||
                            contentPage.VisibleFor == Classes.ContentPage.eVisibility.Paid && isPaid ||
                            contentPage.VisibleFor == Classes.ContentPage.eVisibility.Unpaid && !isPaid)))
                        ||
                        ((!loggedOn && (contentPage.VisibleFor & Classes.ContentPage.eVisibility.NotLoggedOnUsers) != 0))
                       )
                        lContentPages.Add(contentPage);
                }
            }

            rptPages.DataSource = lContentPages.ToArray();
            rptPages.DataBind();
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
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