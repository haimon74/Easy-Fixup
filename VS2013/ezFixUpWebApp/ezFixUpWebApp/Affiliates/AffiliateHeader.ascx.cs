using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Affiliates
{
    public partial class AffiliateHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareControls();
        }

        public Affiliate Affiliate
        {
            get
            {
                if (ViewState["CurrentAffiliate"] == null)
                {
                    ViewState["CurrentAffiliate"] = Affiliate.Fetch(((AffiliatePageBase)Page).CurrentAffiliateSession.ID);
                }

                return ViewState["CurrentAffiliate"] as Affiliate;
            }
        }
        
        private void PrepareControls()
        {
            if (Page is AffiliatePageBase &&
                ((AffiliatePageBase)Page).CurrentAffiliateSession != null)
            {
                lblWelcome.Text = String.Format(
                    Lang.Trans("Welcome <b>{0}</b>"),
                    ((AffiliatePageBase)Page).CurrentAffiliateSession.Username);
                lnkLogout.Text = Lang.Trans("Logout");
                pnlLogout.Visible = true;
            }
            else
            {
                pnlLogout.Visible = false;
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            ((AffiliatePageBase)Page).CurrentAffiliateSession = null;
            Response.Redirect("Login.aspx");
        }
    }
}