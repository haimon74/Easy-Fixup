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
using AspNetDating.Affiliates;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class CommissionsHistory : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseAffiliateCommissions;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                populateDropDown();
                populateGridView(null);
            }
        }

        private void loadStrings()
        {
            gvCommissions.Columns[0].HeaderText = Lang.TransA("Username");
            gvCommissions.Columns[1].HeaderText = Lang.TransA("Amount");
            gvCommissions.Columns[2].HeaderText = Lang.TransA("Date");
            gvCommissions.Columns[3].HeaderText = Lang.TransA("Notes");

            lblAffiliateCommissionsPerPage.Text = Lang.TransA("Commissions per page");
            ddAffiliates.Items.Add(new ListItem("", "-1"));

            Affiliate[] affiliates = Affiliate.Fetch();

            if (affiliates.Length > 0)
            {
                foreach (Affiliate affiliate in affiliates)
                {
                    ddAffiliates.Items.Add(new ListItem(affiliate.Username, affiliate.ID.ToString()));
                }
            }
        }

        private void populateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                ddAffiliateCommissionsPerPage.Items.Add(i.ToString());
            ddAffiliateCommissionsPerPage.SelectedValue = Config.AdminSettings.BrowseAffiliateCommissoinsHistory.AffiliateCommissionsHistoryPerPage.ToString();
        }

        private void populateGridView(int? affiliateID)
        {
            gvCommissions.PageSize = Convert.ToInt32(ddAffiliateCommissionsPerPage.SelectedValue);

            AffiliateCommission[] affiliateCommissions = null;

            if (affiliateID == null)
            {
                affiliateCommissions = AffiliateCommission.Fetch(AffiliateCommission.eSortColumn.TimeStamp);
            }
            else
            {
                affiliateCommissions =
                    AffiliateCommission.FetchByAffiliateID(affiliateID.Value, AffiliateCommission.eSortColumn.TimeStamp);
            }

            if (affiliateCommissions.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There is no commission history yet!"), Misc.MessageType.Error);
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

        protected void ddAffiliates_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateGridView();
        }

        private void populateGridView()
        {
            if (ddAffiliates.SelectedValue == "-1")
            {
                populateGridView(null);
            }
            else
            {
                populateGridView(Convert.ToInt32(ddAffiliates.SelectedValue));
            }
        }
    }
}
