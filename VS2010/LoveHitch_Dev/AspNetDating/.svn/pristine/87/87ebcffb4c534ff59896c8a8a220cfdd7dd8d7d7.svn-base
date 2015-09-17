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
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditAffiliate : AdminPageBase
    {
        public int AffiliateID
        {
            get
            {
                if (ViewState["CurrentAffiliateID"] != null)
                {
                    return (int)ViewState["CurrentAffiliateID"];
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                ViewState["CurrentAffiliateID"] = value;
            }
        }

        /// <summary>
        /// Gets the affiliate from DB and saves it in 'ViewState'.
        /// If the affiliate doesn't exist it returns NULL.
        /// </summary>
        /// <value>The affiliate.</value>
        public Affiliate Affiliate
        {
            get
            {
                if (ViewState["CurrentAffiliate"] == null)
                {
                    ViewState["CurrentAffiliate"] = Affiliate.Fetch(AffiliateID);
                }

                return ViewState["CurrentAffiliate"] as Affiliate;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editAffiliates;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int affiliateID;
                if (Int32.TryParse(Request.Params["id"], out affiliateID))
                {
                    AffiliateID = affiliateID;
                    EditAffiliateCtrl1.AffiliateID = affiliateID;
                }
                else
                {
                    return;
                }

                if (Affiliate == null) return;
            }
        }
    }
}
