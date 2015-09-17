/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Drawing;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for Settings.
    /// </summary>
    public partial class Settings : AdminPageBase
    {
        protected MessageBox MessageBox;
        protected AdminMenu AdminMenu1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }
                
                LoadStrings();
            }
            //do not move this function
            LoadTable();
        }

        private void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
        }

        private bool ValidatePasswords()
        {
            bool AllBlank = (txtCurrentPassword.Text.Trim() == "" &&
                             txtNewPassword.Text.Trim() == "" &&
                             txtConfirmNewPassword.Text.Trim() == "");

            bool AllFilledIn = (txtCurrentPassword.Text.Trim() != "" &&
                                txtNewPassword.Text.Trim() != "" &&
                                txtConfirmNewPassword.Text.Trim() != "");

            if (!(AllBlank || AllFilledIn))
            {
                MessageBox.Show(Lang.TransA("Please fill in all password fields or leave them blank!"), Misc.MessageType.Error);
                return false;
            }

            if (AllFilledIn && txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                MessageBox.Show(Lang.TransA("New password fields do not match!"), Misc.MessageType.Error);
                return false;
            }

            if (AllFilledIn && !CurrentAdminSession.IsPasswordIdentical(txtCurrentPassword.Text))
            {
                MessageBox.Show(Lang.TransA("The specified current password is wrong!"), Misc.MessageType.Error);
                return false;
            }

            return true;
        }

        private void LoadTable()
        {
            Reflection.GenerateSettingsTable(phSettings, typeof (Config));
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.generalSettings;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            try
            {
                Reflection.SaveTableSettings(phSettings, typeof (Config));
                AdminMenu1.ShowHideMenuLinks();
            }
            catch (FormatException)
            {
                MessageBox.Show(Lang.TransA("Invalid values!"), Misc.MessageType.Error);
                return;
            }

            if (ValidatePasswords())
            {
                if (txtNewPassword.Text != "")
                {
                    CurrentAdminSession.Password = txtNewPassword.Text;
                    CurrentAdminSession.Update();
                }

                MessageBox.Show(Lang.TransA("Settings have been successfully updated!"), Misc.MessageType.Success);
            }
        }
    }
}