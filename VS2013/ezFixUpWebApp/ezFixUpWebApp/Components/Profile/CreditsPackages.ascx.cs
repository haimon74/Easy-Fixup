using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ezFixUp.Classes;

namespace ezFixUp.Components.Profile
{
    public partial class CreditsPackages : System.Web.UI.UserControl
    {
        private User user;

        public User User
        {
            set
            {
                user = value;
                if (user != null)
                {
                    ViewState["Username"] = user.Username;
                }
                else
                    ViewState["Username"] = null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadCreditsPackages();
            }
        }

        private void loadStrings()
        {
            LargeBoxStart.Title = Lang.Trans("Credits");
            hlPaymentSettings.Title = Lang.Trans("Credits Details");
            btnNext.Text = Lang.Trans("Submit");

            if (ddPaymentProcessors.Items.Count == 0)
            {
                foreach (string paymentProcessor in Config.AdminSettings.Payments.PaymentProcessors)
                {
                    ddPaymentProcessors.Items.Add(paymentProcessor);
                }
            }
        }

        private void loadCreditsPackages()
        {
            CreditsPackage[] creditsPackages = CreditsPackage.Fetch();

            if (creditsPackages.Length > 0)
            {
                foreach (CreditsPackage package in creditsPackages)
                {
                    rlCreditsPackages.Items.Add(new ListItem(package.Name + " - " + package.Quantity + "/" + package.Price.ToString("C"), package.ID.ToString()));
                }
            }
        }

        protected void ddPaymentProcessors_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnNext_Click(object sender, EventArgs e)
        {

        }
    }
}