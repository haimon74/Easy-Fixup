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
using System.Web.UI.WebControls;
using ezFixUp.Classes;


namespace ezFixUp
{
    /// <summary>
    /// Summary description for InviteFriend.
    /// </summary>
    public partial class InviteFriend : PageBase
    {
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "500px", "200px");

            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        private void LoadStrings()
        {
            btnSubmit.Text = Lang.Trans("Submit");
            WideBoxStart1.Title = Lang.Trans("Invite a Friend");

            var inviteFriendTemplate = new EmailTemplates.InviteFriend(LanguageId);
            string value = inviteFriendTemplate.GetFormattedBody(CurrentUserSession.Name);
            value = value.Replace("%%USERNAME%%", CurrentUserSession.Username);
            if (ckeditor != null)
                ckeditor.Text = value;
            if (htmlEditor != null)
                htmlEditor.Content = value;
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
            try
            {
                Classes.User.ValidateEmail(txtFriendsEmail.Value.Trim());
            }
                //invalid email
            catch
            {
                lblError.Text = Lang.Trans("Invalid email!");
                return;
            }

            try
            {
                var inviteFriendTemplate = new EmailTemplates.InviteFriend(LanguageId);
                string body = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
                Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, txtFriendsName.Value, txtFriendsEmail.Value.Trim(),
                           inviteFriendTemplate.GetFormattedSubject(CurrentUserSession.Name), body,
                           false);
            }
            catch (Exception ex)
            {
                Log(ex);
                return;
            }

            StatusPageMessage = Lang.Trans("The invitation email has been sent to your friend.");
            Response.Redirect("ShowStatus.aspx");
        }
    }
}