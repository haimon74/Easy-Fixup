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
    public partial class PaymentHistory : AffiliatePageBase
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
            lblPaymentHistory.Text = Lang.Trans("Payment history");
            btnRequestPayment.Text = Lang.Trans("Request payment");
            lblPaymentHistoryPerPage.Text = Lang.Trans("Payments per page");

            gvPaymentHistory.Columns[0].HeaderText = Lang.Trans("Amount");
            gvPaymentHistory.Columns[1].HeaderText = Lang.Trans("Date Paid");
            gvPaymentHistory.Columns[2].HeaderText = Lang.Trans("Notes");

            btnRequestPayment.Visible = Affiliate.Balance >= Config.Affiliates.PaymentRequestMinSum &&
                                        Affiliate.RequestPayment == false;
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

            AffiliateHistory[] affiliatesHistory = AffiliateHistory.FetchByAffiliateID(CurrentAffiliateSession.ID, AffiliateHistory.eSortColumn.DatePaid);

            if (affiliatesHistory.Length == 0)
            {
                MessageBox.Show(Lang.Trans("You don't have payment history yet!"), Misc.MessageType.Error);
                lblPaymentHistory.Visible = false;
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
            dtPaymentHistory.Columns.Add("AffiliateID");
            dtPaymentHistory.Columns.Add("Amount");
            dtPaymentHistory.Columns.Add("DatePaid");
            dtPaymentHistory.Columns.Add("Notes");

            foreach (AffiliateHistory affiliateHistory in affiliatesHistory)
            {
                dtPaymentHistory.Rows.Add(new object[]
                                          {
                                              affiliateHistory.ID,
                                              affiliateHistory.AffiliateID,
                                              affiliateHistory.Amount.ToString("C"),
                                              affiliateHistory.DatePaid,
                                              affiliateHistory.Notes,
                                          });    
            }

            gvPaymentHistory.DataSource = dtPaymentHistory;
            gvPaymentHistory.DataBind();

            gvPaymentHistory.Visible = dtPaymentHistory.Rows.Count > 0;
        }

        protected void btnRequestPayment_Click(object sender, EventArgs e)
        {
            Affiliate.RequestPayment = true;
            Affiliate.Save();

            btnRequestPayment.Visible = false;

            #region Send an email

            MiscTemplates.AffiliateRequestPayment sendRequestPaymentEmail = new MiscTemplates.AffiliateRequestPayment();
            Email.Send(Affiliate.Email, Config.Misc.SiteEmail,
                                            sendRequestPaymentEmail.GetFormattedSubject(Affiliate.Username),
                                            sendRequestPaymentEmail.GetFormattedBody(Affiliate.Username, Config.Users.SystemUsername), false);

            #endregion

            MessageBox.Show(Lang.Trans("Your payment request was sent."), Misc.MessageType.Success);
        }

        protected void gvPaymentHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvPaymentHistory.PageIndex = e.NewPageIndex;
            populateGridView();
        }

        protected void ddAffiliatesPaymentHistoryPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvPaymentHistory.PageSize = Convert.ToInt32(ddAffiliatesPaymentHistoryPerPage.SelectedValue);
            gvPaymentHistory.PageIndex = 0;
            populateGridView();
        }
    }
}
