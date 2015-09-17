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
    public partial class AffiliatePayments : AdminPageBase
    {
        protected int RecipientID
        {
            get { return (int) ViewState["RecipientID"]; }
            set { ViewState["RecipientID"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseAffiliatesPayments;

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
                mvAffiliateRequestPayments.SetActiveView(viewPaymentRequests);
                loadStrings();
                loadAffiliateRequestPayments();
            }
        }

        private void loadStrings()
        {
            btnPay.Text = Lang.TransA("Submit");
        }

        private void loadAffiliateRequestPayments()
        {
            DataTable dtAffiliateRequestPayments = new DataTable("AffiliateRequestPayments");

            dtAffiliateRequestPayments.Columns.Add("AffiliateID");
            dtAffiliateRequestPayments.Columns.Add("Username");
            dtAffiliateRequestPayments.Columns.Add("Balance");
            dtAffiliateRequestPayments.Columns.Add("PaymentDetails");


            Affiliate[] affiliates = Affiliate.Fetch(true);

            if (affiliates.Length > 0)
            {
                foreach (Affiliate affiliate in affiliates)
                {
                    dtAffiliateRequestPayments.Rows.Add(new object[]
                                                    {
                                                        affiliate.ID,
                                                        affiliate.Username,
                                                        affiliate.Balance.ToString("C"),
                                                        affiliate.PaymentDetails
                                                    });
                }
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no payment requests!"), Misc.MessageType.Error);
            }

            rptAffiliateRequestPayments.DataSource = dtAffiliateRequestPayments;
            rptAffiliateRequestPayments.DataBind();

            rptAffiliateRequestPayments.Visible = dtAffiliateRequestPayments.Rows.Count > 0;
        }

        protected void rptAffiliateRequestPayments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            Button btnMarkPaid = e.Item.FindControl("btnMarkPaid") as Button;

            if (btnMarkPaid != null)
            {
                btnMarkPaid.Text = Lang.TransA("Mark as Paid");

                if (!((AdminPageBase)Page).HasWriteAccess) btnMarkPaid.Enabled = false;
            }
        }

        protected void rptAffiliateRequestPayments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "Pay" :
                    RecipientID = Convert.ToInt32(e.CommandArgument);
                    Affiliate affiliate = Affiliate.Fetch(RecipientID);

                    if (affiliate != null)
                    {
                        txtAmount.Text = String.Format("{0:F2}", affiliate.Balance);
                    }

                    mvAffiliateRequestPayments.SetActiveView(viewPay);
                    break;
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (!((AdminPageBase)Page).HasWriteAccess) return;

            Affiliate affiliate = Affiliate.Fetch(RecipientID);

            if (affiliate != null)
            {
                AffiliateHistory affiliateHistory = new AffiliateHistory(affiliate.ID);
                affiliateHistory.Amount = affiliate.Balance;
                affiliateHistory.DatePaid = DateTime.Now;
                affiliateHistory.Notes = txtNotes.Text.Trim();
                affiliateHistory.PrivateNotes = txtPrivateNotes.Text.Trim();
                affiliateHistory.Save();

                decimal balance = 0;
                try
                {
                    balance = Convert.ToDecimal(txtAmount.Text);
                }
                catch (FormatException)
                {
                    MessageBox.Show(Lang.TransA("Please enter a valid amount!"), Misc.MessageType.Error);
                    return;
                }
                catch (OverflowException)
                {
                    MessageBox.Show(Lang.TransA("Please enter a valid amount!"), Misc.MessageType.Error);
                    return;
                }

                if (balance > affiliate.Balance)
                {
                    MessageBox.Show(Lang.TransA("Amount cannot be greater than affiliate balance!"), Misc.MessageType.Error);
                    return;
                }

                affiliate.Balance -= balance;
                affiliate.RequestPayment = false;
                affiliate.Save();

                #region Send an email

                MiscTemplates.AffiliatePaymentSent sendChargeNotificationEmail = new MiscTemplates.AffiliatePaymentSent();
                Email.Send(Config.Urls.Home, affiliate.Email,
                                                sendChargeNotificationEmail.GetFormattedSubject(Config.Misc.SiteTitle),
                                                sendChargeNotificationEmail.GetFormattedBody(Config.Urls.Home, affiliate.Username), false);

                #endregion
            }

            loadAffiliateRequestPayments();

            mvAffiliateRequestPayments.SetActiveView(viewPaymentRequests);
        }
    }
}
