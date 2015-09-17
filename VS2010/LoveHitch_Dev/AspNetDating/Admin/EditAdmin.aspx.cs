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
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditAdmin.
    /// </summary>
    public partial class EditAdmin : AdminPageBase
    {
        protected MessageBox MessageBox;

        private string username;
        private bool newAdmin;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Params["uid"] != null)
            {
                username = Request.Params["uid"];
                newAdmin = false;
            }
            else
            {
                username = null;
                newAdmin = true;
            }


            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }
                InitControls();
                LoadStrings();
                if (Config.AdminSettings.AdminPermissionsEnabled)
                    PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            if (newAdmin)
            {
                lblNewPassword.Text = Lang.TransA("Password");
                lblConfirmNewPassword.Text = Lang.TransA("Confirm password");
            }
            else
            {
                lblNewPassword.Text = Lang.TransA("New password");
                lblConfirmNewPassword.Text = Lang.TransA("Confirm new password");
            }

            if (Config.AdminSettings.AdminPermissionsEnabled)
            {
                lblAccountPermissions.Text = Lang.TransA("Account Permissions");
                gridPermissions.Columns[0].HeaderText = Lang.TransA("Sections");
                gridPermissions.Columns[1].HeaderText = Lang.TransA("Read");
                gridPermissions.Columns[2].HeaderText = Lang.TransA("Write");
            }

            btnSave.Text = Lang.TransA("Save");
        }

        private void InitControls()
        {
            if (!Config.AdminSettings.AdminPermissionsEnabled)
                trPermissions.Visible = false;

            if (newAdmin)
            {
                trCurrentPassword.Visible = false;
            }
            else
            {
                trUsername.Visible = false;

                if (username == Config.Users.SystemUsername)
                {
                    if (CurrentAdminSession.Username != Config.Users.SystemUsername)
                    {
                        tblAll.Visible = false;
                        MessageBox.Show(Lang.TransA("Access denied!"), Misc.MessageType.Error);
                    }

                    trPermissions.Visible = false;
                }
            }
        }

        private void PopulateDataGrid()
        {
            Classes.Admin admin;

            if (newAdmin)
            {
                admin = new Classes.Admin();
            }
            else
            {
                admin = Classes.Admin.Fetch(username);
            }

            if (admin != null)
            {
                #region BindPermissions

                DataTable dtPermissions = new DataTable("Permissions");
                dtPermissions.Columns.Add("Section");
                dtPermissions.Columns.Add("Read", typeof (Boolean));
                dtPermissions.Columns.Add("Write", typeof (Boolean));

                Classes.Admin.RawAdminPrivileges rawdata =
                    Classes.Admin.GetRawDataFromPrivileges(admin.Privileges);

                for (int i = 0; i < rawdata.lSections.Count; ++i)
                {
                    dtPermissions.Rows.Add(new object[]
                                               {
                                                   rawdata.lSections[i],
                                                   rawdata.lReadPermissions[i],
                                                   rawdata.lWritePermissions[i]
                                               }
                        );
                }

                gridPermissions.DataSource = dtPermissions;
                gridPermissions.DataBind();

                #endregion
            }
            else
            {
                //this admin no longer exists
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageAdmins;
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
            
            Classes.Admin admin = null;

            if (newAdmin)
            {
                admin = new Classes.Admin();
            }
            else
            {
                admin = Classes.Admin.Fetch(username);
            }

            if (admin != null)
            {
                #region Saving username

                if (newAdmin)
                {
                    try
                    {
                        Classes.User.ValidateUsername(txtUsername.Text);
                        if (Classes.Admin.IsUsernameTaken(txtUsername.Text))
                        {
                            MessageBox.Show(Lang.TransA("Username is already taken"), Misc.MessageType.Error);
                            return;
                        }
                        admin.Username = txtUsername.Text;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Misc.MessageType.Error);
                        return;
                    }
                }

                #endregion

                #region Saving passwords

                if (ValidatePasswords(admin))
                {
                    if (txtNewPassword.Text != "")
                    {
                        try
                        {
                            admin.Password = txtNewPassword.Text;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, Misc.MessageType.Error);
                            return;
                        }
                    }
                }
                else return;

                #endregion

                #region Saving permissions

                if (Config.AdminSettings.AdminPermissionsEnabled && admin.Username != Config.Users.SystemUsername)
                {
                    Classes.Admin.RawAdminPrivileges rawprivileges =
                        new Classes.Admin.RawAdminPrivileges();

                    foreach (DataGridItem item in gridPermissions.Items)
                    {
                        string section = ((Literal) item.FindControl("litSection")).Text;
                        bool read = ((CheckBox) item.FindControl("cbReadPermission")).Checked;
                        bool write = ((CheckBox) item.FindControl("cbWritePermission")).Checked;

                        rawprivileges.lSections.Add(section);
                        rawprivileges.lReadPermissions.Add(read);
                        rawprivileges.lWritePermissions.Add(write);
                    }

                    admin.Privileges = Classes.Admin.GetPrivilegesFromRawData(rawprivileges);
                }

                #endregion

                if (newAdmin)
                {
                    Classes.Admin.Create(admin);
                    MessageBox.Show(Lang.TransA("The account has been successfully created"), Misc.MessageType.Success);
                }
                else
                {
                    admin.Update();
                    if (CurrentAdminSession.Username == admin.Username)
                        CurrentAdminSession.Load();
                    MessageBox.Show(Lang.TransA("The account has been successfully updated"), Misc.MessageType.Success);
                }
            }
            else
            {
                //this admin no longer exists
            }
        }

        private bool ValidatePasswords(Classes.Admin admin)
        {
            bool AllBlank = (
                                ((newAdmin) ? true : txtCurrentPassword.Text.Trim() == "") &&
                                txtNewPassword.Text.Trim() == "" &&
                                txtConfirmNewPassword.Text.Trim() == "");

            bool AllFilledIn = (
                                   ((newAdmin) ? true : txtCurrentPassword.Text.Trim() != "") &&
                                   txtNewPassword.Text.Trim() != "" &&
                                   txtConfirmNewPassword.Text.Trim() != "");

            if (!(AllBlank || AllFilledIn))
            {
                MessageBox.Show(Lang.TransA("Please fill in all password fields or leave them blank!"), Misc.MessageType.Error);
                return false;
            }

            if (AllFilledIn && txtNewPassword.Text != txtConfirmNewPassword.Text)
            {
                MessageBox.Show(Lang.TransA("Password fields do not match!"), Misc.MessageType.Error);
                return false;
            }

            if (!newAdmin && admin != null)
            {
                if (AllFilledIn && !admin.IsPasswordIdentical(txtCurrentPassword.Text))
                {
                    MessageBox.Show(Lang.TransA("The specified current password is wrong!"), Misc.MessageType.Error);
                    return false;
                }
            }

            return true;
        }
    }
}