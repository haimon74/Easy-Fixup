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
    public partial class CommissionsHistory : AffiliatePageBase
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
                loadStrings();
                populateDropDown();
                populateGridView();
            }
        }

        private void loadStrings()
        {
            gvCommissions.Columns[0].HeaderText = Lang.Trans("Username");
            gvCommissions.Columns[1].HeaderText = Lang.Trans("Amount");
            gvCommissions.Columns[2].HeaderText = Lang.Trans("Date");
            gvCommissions.Columns[3].HeaderText = Lang.Trans("Notes");

            lblAffiliateCommissionsPerPage.Text = Lang.Trans("Commissions per page");
        }

        private void populateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                ddAffiliateCommissionsPerPage.Items.Add(i.ToString());
            ddAffiliateCommissionsPerPage.SelectedValue = Config.AdminSettings.BrowseAffiliateCommissoinsHistory.AffiliateCommissionsHistoryPerPage.ToString();
        }

        private void populateGridView()
        {
            gvCommissions.PageSize = Convert.ToInt32(ddAffiliateCommissionsPerPage.SelectedValue);

            AffiliateCommission[] affiliateCommissions = AffiliateCommission.FetchByAffiliateID(CurrentAffiliateSession.ID, AffiliateCommission.eSortColumn.TimeStamp);

            if (affiliateCommissions.Length == 0)
            {
                MessageBox.Show(Lang.Trans("You don't have commissions history yet!"), Misc.MessageType.Error);
                gvCommissions.Visible = false;
                pnlAffiliateCommissionsPerPage.Visible = false;
            }
            else
            {
                loadCommissions(affiliateCommissions);

                gvCommissions.Visible = true;
                pnlAffiliateCommissionsPerPage.Visible = true;
            }
        }

        private void loadCommissions(AffiliateCommission[] affiliateCommissions)
        {
            DataTable dtCommissions = new DataTable("Commissions");

            dtCommissions.Columns.Add("ID");
            dtCommissions.Columns.Add("Username");
            dtCommissions.Columns.Add("Amount");
            dtCommissions.Columns.Add("TimeStamp");
            dtCommissions.Columns.Add("Notes");

            foreach (AffiliateCommission affiliateCommission in affiliateCommissions)
            {
                dtCommissions.Rows.Add(new object[]
                                          {
                                              affiliateCommission.ID,
                                              affiliateCommission.Username,
                                              affiliateCommission.Amount.ToString("C"),
                                              affiliateCommission.TimeStamp,
                                              affiliateCommission.Notes,
                                          });
            }

            gvCommissions.DataSource = dtCommissions;
            gvCommissions.DataBind();

            gvCommissions.Visible = dtCommissions.Rows.Count > 0;
        }

        protected void gvCommissions_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCommissions.PageIndex = e.NewPageIndex;
            populateGridView();
        }

        protected void ddAffiliateCommissionsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvCommissions.PageSize = Convert.ToInt32(ddAffiliateCommissionsPerPage.SelectedValue);
            gvCommissions.PageIndex = 0;
            populateGridView();
        }
    }
}
