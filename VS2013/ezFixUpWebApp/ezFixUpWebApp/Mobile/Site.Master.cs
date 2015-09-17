using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ezFixUp.Mobile
{
    public partial class Site : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (((PageBase) Page).CurrentUserSession == null)
                {
                    liHome.Visible = false;
                    liSearch.Visible = false;
                    liProfile.Visible = false;
                    liMailbox.Visible = false;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            divMessage.Visible = !String.IsNullOrEmpty(((PageBase)Page).StatusPageMessage);

            if (!String.IsNullOrEmpty(((PageBase)Page).StatusPageMessage))
            {
                divMessage.InnerHtml = ((PageBase)Page).StatusPageMessage;
                ((PageBase)Page).StatusPageMessage = null;
            }

            if (!String.IsNullOrEmpty(((PageBase)Page).StatusPageLinkURL))
            {
                ((PageBase)Page).StatusPageLinkURL = String.Empty;
                ((PageBase)Page).StatusPageLinkText = String.Empty;
                ((PageBase)Page).StatusPageLinkSkindId = String.Empty;
            }
        }
    }
}
