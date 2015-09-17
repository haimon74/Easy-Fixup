using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditAffiliateCtrl : System.Web.UI.UserControl
    {
        public int AffiliateID
        {
            get { return (int)ViewState["CurrentAffiliateID"]; }
            set { ViewState["CurrentAffiliateID"] = value; }
        }

        public Affiliate Affiliate
        {
            get
            {
                if (Page is EditAffiliate)
                {
                    return ((EditAffiliate)Page).Affiliate;
                }
                else
                {
                    return Affiliate.Fetch(AffiliateID);
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadAffiliateData();
            }
        }

        private void loadStrings()
        {
            if (!((AdminPageBase)Page).HasWriteAccess) btnSave.Enabled = false;

            btnSave.Text = Lang.TransA(" Save Changes >> ");
            btnCancel.Text = Lang.TransA("Cancel");

            ddDeleted.Items.Add(new ListItem(Lang.TransA("No"), "0"));
            ddDeleted.Items.Add(new ListItem(Lang.TransA("Yes"), "1"));
            ddActive.Items.Add(new ListItem(Lang.TransA("No"), "0"));
            ddActive.Items.Add(new ListItem(Lang.TransA("Yes"), "1"));
            ddRecurrent.Items.Add(new ListItem(Lang.TransA("No"), "0"));
            ddRecurrent.Items.Add(new ListItem(Lang.TransA("Yes"), "1"));
        }

        private void loadAffiliateData()
        {
            #region Populate Controls

            txtName.Text = Affiliate.Name;
            txtEmail.Text = Affiliate.Email;
            txtSiteURL.Text = Affiliate.SiteURL;
            txtPaymentDetails.Text = Affiliate.PaymentDetails;
            txtPercentage.Text = Affiliate.Percentage == null ? "0" : Affiliate.Percentage.ToString();
            txtFixedAmount.Text = Affiliate.FixedAmount == null ? "0" : String.Format("{0:F2}", Affiliate.FixedAmount);
            txtBalance.Text = String.Format("{0:F2}", Affiliate.Balance);
            ddDeleted.SelectedIndex = Affiliate.Deleted ? 1 : 0;
            ddActive.SelectedIndex = Affiliate.Active? 1 : 0;
            ddRecurrent.SelectedIndex = Affiliate.Recurrent ? 1 : 0;

            #endregion
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!((AdminPageBase)Page).HasWriteAccess) return;

            #region Set fields

            string password = txtPassword.Text.Trim();
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string siteURL = txtSiteURL.Text.Trim();
            string paymentDetails = txtPaymentDetails.Text.Trim();
            string percentage = txtPercentage.Text.Trim();
            string fixedAmount = txtFixedAmount.Text.Trim();
            string balance = txtBalance.Text.Trim();
            bool deleted = ddDeleted.SelectedValue == "0" ? false : true;
            bool active = ddActive.SelectedValue == "0" ? false : true;
            bool recurrent = ddRecurrent.SelectedValue == "0" ? false : true;

            #endregion

            #region Validate fields

            if (name.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter name!"), Misc.MessageType.Error);
                return;
            }

            if (email.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter email!"), Misc.MessageType.Error);
                return;
            }

            if (siteURL.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter site URL!"), Misc.MessageType.Error);
                return;
            }

            if (paymentDetails.Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter payment details!"), Misc.MessageType.Error);
                return;
            }

            int intPercentage;
            if (!Int32.TryParse(percentage, out intPercentage))
            {
                MessageBox.Show(Lang.TransA("Please enter valid percentage!"), Misc.MessageType.Error);
                return;
            }

            decimal decFixedAmount;
            if (!Decimal.TryParse(fixedAmount, out decFixedAmount))
            {
                MessageBox.Show(Lang.TransA("Please enter valid fixed amount!"), Misc.MessageType.Error);
                return;
            }

            decimal decBalance;
            if (!Decimal.TryParse(balance, out decBalance))
            {
                MessageBox.Show(Lang.TransA("Please enter valid balance!"), Misc.MessageType.Error);
                return;
            }

            #endregion

            if (password.Length > 0)
            {
                Affiliate.Password = password;
            }

            Affiliate.Name = name;
            Affiliate.Email = email;
            Affiliate.SiteURL = siteURL;
            Affiliate.PaymentDetails = paymentDetails;
            Affiliate.Percentage = intPercentage;
            Affiliate.FixedAmount = decFixedAmount;
            Affiliate.Balance = decBalance;
            Affiliate.Deleted = deleted;
            Affiliate.Active = active;
            Affiliate.Recurrent = recurrent;

            Affiliate.Save();

            MessageBox.Show(Lang.TransA("The affiliate has been successfully updated!"), Misc.MessageType.Success);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/BrowseAffiliates.aspx");
        }
    }
}