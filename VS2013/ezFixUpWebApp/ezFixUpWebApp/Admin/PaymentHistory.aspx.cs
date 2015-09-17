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

namespace ezFixUp.Admin
{
    public partial class PaymentHistory : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseAffiliatesPaymentHistory;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Affiliates.Enable)
                {
                    StatusPageMessage =
                        Lang.TransA("Affiliates option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                loadStrings();
                populateDropDown();
                populateGridView();
            }
        }

        private void loadStrings()
        {
            gvPaymentHistory.Columns[0].HeaderText = Lang.TransA("Affiliate");
            gvPaymentHistory.Columns[1].HeaderText = Lang.TransA("Amount");
            gvPaymentHistory.Columns[2].HeaderText = Lang.TransA("Date Paid");
            gvPaymentHistory.Columns[3].HeaderText = Lang.TransA("Notes");
            gvPaymentHistory.Columns[4].HeaderText = Lang.TransA("Private Notes");

            lblPaymentHistoryPerPage.Text = Lang.TransA("Payments per page");
        }

        private void populateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                ddAffiliatesPaymentHistoryPerPage.Items.Add(i.ToString());
            ddAffiliatesPaymentHistoryPerPage.SelectedValue = Config.AdminSettings.BrowseAffiliatesPaymentHistory.AffiliatePaymentHistoryPerPage.ToString();
        }

        private void populateGridView()
        {
            gvPaymentHistory.PageSize = Convert.ToInt32(ddAffiliatesPaymentHistoryPerPage.SelectedValue);
            
            AffiliateHistory[] affiliatesHistory = AffiliateHistory.Fetch(AffiliateHistory.eSortColumn.DatePaid);

            if (affiliatesHistory.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no affhiliates payment history!"), Misc.MessageType.Error);
                gvPaymentHistory.Visible = false;
                pnlPaymentHistoryPerPage.Visible = false;
            }
            else
            {
                loadPaymentHistory(affiliatesHistory);

                gvPaymentHistory.Visible = true;
                pnlPaymentHistoryPerPage.Visible = true;
            }
        }

        private void loadPaymentHistory(AffiliateHistory[] affiliatesHistory)
        {
            DataTable dtPaymentHistory = new DataTable("PaymentHistory");

            dtPaymentHistory.Columns.Add("ID");
            dtPaymentHistory.Columns.Add("AffiliateUsername");
            dtPaymentHistory.Columns.Add("Amount");
            dtPaymentHistory.Columns.Add("DatePaid");
            dtPaymentHistory.Columns.Add("Notes");
            dtPaymentHistory.Columns.Add("PrivateNotes");

            foreach (AffiliateHistory affiliateHistory in affiliatesHistory)
            {
                Affiliate affiliate = Affiliate.Fetch(affiliateHistory.AffiliateID);

                if (affiliate != null)
                {
                    dtPaymentHistory.Rows.Add(new object[]
                                          {
                                              affiliateHistory.ID,
                                              affiliate.Username,
                                              affiliateHistory.Amount.ToString("C"),
                                              affiliateHistory.DatePaid,
                                              affiliateHistory.Notes,
                                              affiliateHistory.PrivateNotes
                                          });    
                }
                else
                {
                    continue;
                }
            }

            gvPaymentHistory.DataSource = dtPaymentHistory;
            gvPaymentHistory.DataBind();

            gvPaymentHistory.Visible = dtPaymentHistory.Rows.Count > 0;
        }

        protected void ddAffiliatesPaymentHistoryPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPaymentHistory.PageSize = Convert.ToInt32(ddAffiliatesPaymentHistoryPerPage.SelectedValue);
            gvPaymentHistory.PageIndex = 0;
            populateGridView();
        }

        protected void gvPaymentHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPaymentHistory.PageIndex = e.NewPageIndex;
            populateGridView();
        }
    }
}
