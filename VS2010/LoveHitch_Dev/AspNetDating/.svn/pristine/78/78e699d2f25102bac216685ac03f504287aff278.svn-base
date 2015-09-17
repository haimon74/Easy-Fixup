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

namespace AspNetDating.Affiliates
{
    public partial class AffiliateBanners : AffiliatePageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadAffiliateBanners();
            }
        }

        private void loadStrings()
        {
            
        }

        private void loadAffiliateBanners()
        {
            DataTable dtAffiliateBanners = new DataTable("AffiliateBanners");

            dtAffiliateBanners.Columns.Add("ID");
            dtAffiliateBanners.Columns.Add("Name");
            dtAffiliateBanners.Columns.Add("HTMLCode");


            AffiliateBanner[] affiliateBanners = AffiliateBanner.Fetch(false);

            if (affiliateBanners.Length > 0)
            {
                foreach (AffiliateBanner affiliateBanner in affiliateBanners)
                {
                    string htmlCode =
                        String.Format(
                            "<SCRIPT language='JavaScript1.1' SRC=\"{0}/AffTracker.ashx?aff={1}&bid={2}\"></SCRIPT>",
                            Config.Urls.Home, CurrentAffiliateSession.ID, affiliateBanner.ID);
                    dtAffiliateBanners.Rows.Add(new object[]
                                                    {
                                                        affiliateBanner.ID,
                                                        affiliateBanner.Name,
                                                        htmlCode
                                                    });
                }
            }
            else
            {
                MessageBox.Show(Lang.Trans("There are no affiliate banners!"), Misc.MessageType.Error);
            }

            rptAffiliateBanners.DataSource = dtAffiliateBanners;
            rptAffiliateBanners.DataBind();

            rptAffiliateBanners.Visible = dtAffiliateBanners.Rows.Count > 0;
        }
    }
}
