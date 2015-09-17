using System;
using AspNetDating.Classes;

namespace AspNetDating.Affiliates
{
    public partial class Register : AffiliatePageBase
    {

        public Register()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Config.Affiliates.Enable)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            btnRegister.Text = Lang.Trans("Register");
        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            #region Validate username

            if (txtUsername.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please specify username!"), Misc.MessageType.Error);
                return;
            }

            if (Affiliate.IsUsernameTaken(txtUsername.Text))
            {
                MessageBox.Show(Lang.Trans("Username is already taken!"), Misc.MessageType.Error);
                return;
            }

            foreach (string reservedUsername in Config.Users.ReservedUsernames)
            {
                if (reservedUsername == txtUsername.Text.ToLower())
                {
                    MessageBox.Show(Lang.Trans("Username is reserved!"), Misc.MessageType.Error);
                    return;
                }
            }

            try
            {
                Affiliate.ValidateUsername(txtUsername.Text);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, Misc.MessageType.Error);
                return;
            }

            #endregion

            #region Validate passwords

            if (txtPassword.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please specify password!"), Misc.MessageType.Error);
                return;
            }
            if (txtPasswordConfirm.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please verify password!"), Misc.MessageType.Error);
                return;
            }
            if (txtPassword.Text != txtPasswordConfirm.Text)
            {
                MessageBox.Show(Lang.Trans("Passwords do not match!"), Misc.MessageType.Error);
                return;
            }

            try
            {
                Affiliate.ValidatePassword(txtPassword.Text);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, Misc.MessageType.Error);
                return;
            }

            #endregion

            #region Validate name

            if (txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please enter your name!"), Misc.MessageType.Error);
                return;
            }

            #endregion

            #region Validate e-mail address

            try
            {
                if (txtEmail.Text.Trim().Length == 0)
                {
                    MessageBox.Show(Lang.Trans("Please specify e-mail address!"), Misc.MessageType.Error);
                    return;
                }
            }
            catch (ArgumentException err) // Invalid e-mail address
            {
                MessageBox.Show(err.Message, Misc.MessageType.Error);
                return;
            }

            #endregion

            #region Validate site URL

            if (txtSiteUrl.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please enter your site URL!"), Misc.MessageType.Error);
                return;
            }

            #endregion

            #region Validate payment details

            if (txtPaymentDetails.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please enter your payment details!"), Misc.MessageType.Error);
                return;
            }

            #endregion

            Affiliate affiliate = new Affiliate(txtUsername.Text.Trim());

            affiliate.Password = txtPassword.Text;
            affiliate.Name = txtName.Text.Trim();
            affiliate.Email = txtEmail.Text.Trim();
            affiliate.SiteURL = txtSiteUrl.Text.Trim();
            affiliate.PaymentDetails = txtPaymentDetails.Text.Trim();

            affiliate.Save();

            AffiliateSession affiliateSession = null;

            try
            {
                affiliateSession = new AffiliateSession(txtUsername.Text.Trim());
                affiliateSession.Authorize(txtPassword.Text.Trim());    
            }
            catch (NotFoundException err)
            {
                MessageBox.Show(err.Message, Misc.MessageType.Error);
                return;
            }
            catch (AccessDeniedException err)
            {
                MessageBox.Show(err.Message, Misc.MessageType.Error);
                return;
            }
            catch (Exception err)
            {
                IPLogger.Log(txtUsername.Text, Request.UserHostAddress, IPLogger.ActionType.AffiliateLoginFailed);

                MessageBox.Show(err.Message, Misc.MessageType.Error);
                return;
            }

            CurrentAffiliateSession = affiliateSession;

            Response.Redirect("~/Affiliates/Home.aspx");
        }
    }
}
