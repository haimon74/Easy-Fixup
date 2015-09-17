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
    public partial class Login : AffiliatePageBase
    {
        public Login()
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
            btnLogin.Text = Lang.Trans(" Login >> ");
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUsername.Text.Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please specify username!"), Misc.MessageType.Error);
                return;
            }
            if (txtPassword.Text.Length == 0)
            {
                MessageBox.Show(Lang.Trans("Please specify password!"), Misc.MessageType.Error);
                return;
            }

            AffiliateSession affiliate = null;
            try
            {
                affiliate = new AffiliateSession(txtUsername.Text);
                affiliate.Authorize(txtPassword.Text);

                IPLogger.Log(affiliate.Username, Request.UserHostAddress, IPLogger.ActionType.AffiliateLoginSuccess);
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

            ((AffiliatePageBase)Page).CurrentAffiliateSession = affiliate;

            if (Request.Params["back_url"] != null
                && Request.Params["back_url"].Length > 0)
                Response.Redirect(Request.Params["back_url"]);
            else
                Response.Redirect("Home.aspx");
        }
    }
}
