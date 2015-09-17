/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using System.Net;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using ezFixUp.Classes.ContactsExtractor;


namespace ezFixUp
{
    /// <summary>
    /// Summary description for InviteFriend.
    /// </summary>
    public partial class ImportFriends : PageBase
    {
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.TraceEnabled = false;
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "700px", "300px");

            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        private void LoadStrings()
        {
            btnSubmit.Text = Lang.Trans("Submit");
            btnImport.Text = "Import".Translate();
            WideBoxStart1.Title = Lang.Trans("Invite friends from your address book");

            var inviteFriendTemplate =
                new EmailTemplates.InviteFriend(LanguageId);
            string value = inviteFriendTemplate.GetFormattedBody(CurrentUserSession.Name);
            value = value.Replace("%%USERNAME%%", CurrentUserSession.Username);
            if (ckeditor != null)
                ckeditor.Text = value;
            else if (htmlEditor != null)
                htmlEditor.Content = value;

            dgContacts.Columns[1].HeaderText = "Name".Translate();
            dgContacts.Columns[2].HeaderText = "Email".Translate();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSubmit.Click += new EventHandler(btnSubmit_Click);
        }

        #endregion

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            var inviteFriendTemplate = new EmailTemplates.InviteFriend(LanguageId);

            HtmlInputCheckBox cbCheck;
            foreach (DataGridItem item in dgContacts.Items)
            {
                cbCheck = (HtmlInputCheckBox)item.FindControl("cbSelect");
                if (cbCheck.Checked)
                {
                    string content = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
                    Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, ((Label)item.FindControl("lblName")).Text,
                               ((Label)item.FindControl("lblEmail")).Text,
                               inviteFriendTemplate.GetFormattedSubject(CurrentUserSession.Name), content,
                               false);
                }
            }

            StatusPageMessage = Lang.Trans("The invitation email has been sent to your friend.");
            Response.Redirect("ShowStatus.aspx");
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                Classes.User.ValidateEmail(txtEmail.Value + "@" + ddEmailProvider.SelectedValue);
            }
            //invalid email
            catch
            {
                lblError.Text = Lang.Trans("Invalid email!");
                return;
            }

            try
            {
                IMailContactExtract extractor;
                switch (ddEmailProvider.SelectedValue)
                {
                    case "gmail.com":
                        extractor = new GmailExtract();
                        break;
                    case "yahoo.com":
                        extractor = new YahooExtract();
                        break;
                    case "live.com":
                    case "hotmail.com":
                        extractor = new LiveExtract();
                        break;
                    default:
                        lblError.Text = Lang.Trans("The mail service is not supported!");
                        return;
                }

                MailContactList contactList;
                bool success =
                    extractor.Extract(
                        new NetworkCredential(txtEmail.Value + "@" + ddEmailProvider.SelectedValue, txtPassword.Value),
                        out contactList);
                if (!success)
                {
                    lblError.Text = "The site was unable to obtain the address book!";
                    return;
                }

                DataTable dtContacts = new DataTable();
                dtContacts.Columns.Add("Name");
                dtContacts.Columns.Add("Email");

                foreach (var contact in contactList)
                {
                    if (string.IsNullOrEmpty(contact.Email) || !contact.Email.Contains("@")) continue;
                    if (string.IsNullOrEmpty(contact.Name))
                        contact.Name = contact.Email.Remove(contact.Email.IndexOf("@"));
                    dtContacts.Rows.Add(new object[]
                                            {
                                                contact.Name,
                                                contact.Email
                                            });
                }

                dgContacts.DataSource = dtContacts;
                dgContacts.DataBind();
                dgContacts.Visible = dtContacts.Rows.Count > 0;
            }
            catch (Exception ex)
            {
                Log(ex);
                return;
            }

            mvImportFriends.SetActiveView(viewMessage);
        }
    }
}