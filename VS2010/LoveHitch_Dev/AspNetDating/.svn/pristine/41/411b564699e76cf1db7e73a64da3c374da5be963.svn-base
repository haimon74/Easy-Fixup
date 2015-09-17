using System;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class SendProfile : PageBase
    {
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        public SendProfile()
        {
            RequiresAuthorization = false;
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "100%", "200px");

            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        private void LoadStrings()
        {
            SmallBoxStart2.Title = Lang.Trans("Actions");
            lnkViewProfile.Text = Lang.Trans("Back to profile");
            btnSend.Text = Lang.Trans("Send");
            LargeBoxStart.Title = Lang.Trans("Send to a friend");
            if (CurrentUserSession != null)
            {
                txtSender.Text = CurrentUserSession.Username;
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Request.Params["uid"]))
            {
                EmailTemplates.SendProfileToFriend sendProfileToFriendMail = new EmailTemplates.SendProfileToFriend();
                if (CurrentUserSession != null) sendProfileToFriendMail.LanguageId = CurrentUserSession.LanguageId;
                string content = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
                string body = sendProfileToFriendMail.GetFormattedBody(txtSender.Text, txtRecipientName.Text,
                                                                       content, Request.Params["uid"]);
                if (body != "") //if the user exists in DB
                {
                    if (txtRecipientMail.Text.Length != 0)
                    {
                        try
                        {
                            Classes.User.ValidateEmail(txtRecipientMail.Text);
                        }
                        catch(ArgumentException)
                        {
                            lblError.Text = Lang.Trans("Please specify e-mail address!");
                            return;
                        }

                        Email.Send(Config.Misc.SiteEmail, txtRecipientMail.Text,
                           sendProfileToFriendMail.GetFormattedSubject(txtSender.Text), body, true);
                    }
                    else
                    {
                        lblError.Text = Lang.Trans("Please specify e-mail address!");
                        return;
                    }
                }
                else
                {
                    lblError.Text = Lang.Trans("The specified account does not exist!");
                    return;
                }

                pnlSendProfile.Visible = false;
                btnSend.Visible = false;
                lblError.Text = Lang.Trans("The profile has been sent to") + " " + txtRecipientName.Text;
            }
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            if (Request.Params["uid"] != null)
            {
                Response.Redirect(UrlRewrite.CreateShowUserUrl(Request.Params["uid"]));
            }
        }
    }
}
