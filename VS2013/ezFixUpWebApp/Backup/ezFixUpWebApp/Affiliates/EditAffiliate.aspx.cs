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
    public partial class EditAffiliate : AffiliatePageBase
    {
        public Affiliate Affiliate
        {
            get
            {
                if (ViewState["CurrentAffiliate"] == null)
                {
                    ViewState["CurrentAffiliate"] = Affiliate.Fetch(CurrentAffiliateSession.ID);
                }

                return ViewState["CurrentAffiliate"] as Affiliate;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Affiliate == null) return;
            }
        }
    }
}
