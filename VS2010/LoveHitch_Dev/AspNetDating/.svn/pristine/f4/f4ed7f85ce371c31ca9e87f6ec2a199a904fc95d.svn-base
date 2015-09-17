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
    public partial class EditAffiliateCtrl : System.Web.UI.UserControl
    {
        private AffiliateSession CurrentAffiliateSession
        {
            get { return ((AffiliatePageBase) Page).CurrentAffiliateSession; }
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
                    return Classes.Affiliate.Fetch(CurrentAffiliateSession.ID);
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

        private void loadAffiliateData()
        {
            Affiliate affiliate = Affiliate.Fetch(CurrentAffiliateSession.Username);

            if (affiliate != null)
            {
                txtName.Text = affiliate.Name;
                txtEmail.Text = affiliate.Email;
                txtSiteURL.Text = affiliate.SiteURL;
                txtPaymentDetails.Text = affiliate.PaymentDetails;
            }
        }

        private void loadStrings()
        {
            btnSave.Text = Lang.Trans(" Save Changes >> ");
            btnCancel.Text = Lang.Trans("Cancel");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (validateData())
            {
                if (txtNewPassword.Text != "")
                {
                    Affiliate.Password = txtNewPassword.Text.Trim();
                }

                Affiliate.Name = txtName.Text.Trim();
                Affiliate.Email = txtEmail.Text.Trim();
                Affiliate.SiteURL = txtSiteURL.Text.Trim();
                Affiliate.PaymentDetails = txtPaymentDetails.Text.Trim();
                Affiliate.Save();

                MessageBox.Show(Lang.Trans("Your account has been successfully updated!"), Misc.MessageType.Success);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private bool validateData()
        {
            string currentPassword = txtCurrentPassword.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string confirmNewPassword = txtConfirmNewPassword.Text.Trim();
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string siteURL = txtSiteURL.Text.Trim();
            string paymentDetails = txtPaymentDetails.Text.Trim();

            #region Validate Password

            bool AllBlank = (currentPassword == "" &&
                             newPassword == "" &&
                             confirmNewPassword == "");

            bool AllFilledIn = (currentPassword != "" &&
                                newPassword != "" &&
                                confirmNewPassword != "");

            if (!(AllBlank || AllFilledIn))
            {
                MessageBox.Show(Lang.Trans("Please fill in all password fields or leave them blank!"),
                                Misc.MessageType.Error);
                return false;
            }

            if (AllFilledIn && newPassword != confirmNewPassword)
            {
                MessageBox.Show(Lang.Trans("New password fields do not match!"), Misc.MessageType.Error);
                return false;
            }

            if (AllFilledIn && !Affiliate.IsPasswordIdentical(currentPassword))
            {
                MessageBox.Show(Lang.Trans("The specified current password is wrong!"), Misc.MessageType.Error);
                return false;
            }

            #endregion

            #region Validate Name

            if (name.Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please enter your name!"), Misc.MessageType.Error);
                return false;
            }

            #endregion

            #region Validate Email

            if (email.Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please specify e-mail address!"), Misc.MessageType.Error);
                return false;
            }

            #endregion

            #region Validate Site URL

            if (siteURL.Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please specify site url!"), Misc.MessageType.Error);
                return false;
            }

        #endregion

            #region Validate Payment details

            if (paymentDetails.Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please enter enter your payment details!"), Misc.MessageType.Error);
                return false;
            }

            #endregion

            return true;
        }
    }
}