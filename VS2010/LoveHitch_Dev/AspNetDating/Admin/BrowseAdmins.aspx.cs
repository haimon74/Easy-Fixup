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
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for BrowseAdmins.
    /// </summary>
    public partial class BrowseAdmins : AdminPageBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected Button AddAdmin;


        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAddAdmin.Enabled = false;
                }
                
                LoadStrings();
                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            btnAddAdmin.Text = Lang.TransA("Add Admin");
            gridAdmins.Columns[0].HeaderText = Lang.TransA("Username");
            gridAdmins.Columns[1].HeaderText = Lang.TransA("Last login");
            gridAdmins.Columns[2].HeaderText = Lang.TransA("Actions");
        }

        private void PopulateDataGrid()
        {
            Classes.Admin[] admins = Classes.Admin.Fetch();

            if (admins.Length != 0)
            {
                DataTable dtAdmins = new DataTable("Admins");
                dtAdmins.Columns.Add("Username");
                dtAdmins.Columns.Add("LastLogin");

                foreach (Classes.Admin admin in admins)
                {
                    dtAdmins.Rows.Add(new object[]
                                          {
                                              admin.Username,
                                              admin.LastLogin
                                          }
                        );
                }

                dtAdmins.DefaultView.Sort = "Username";

                gridAdmins.DataSource = dtAdmins;
                gridAdmins.DataBind();
            }
            else
            {
                //no admins?!
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
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
            this.gridAdmins.ItemCommand +=
                new System.Web.UI.WebControls.DataGridCommandEventHandler(this.gridAdmins_ItemCommand);
            this.gridAdmins.ItemCreated += new DataGridItemEventHandler(gridAdmins_ItemCreated);
            this.gridAdmins.ItemDataBound += new DataGridItemEventHandler(gridAdmins_ItemDataBound);
        }

        #endregion

        private void gridAdmins_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                Response.Redirect("EditAdmin.aspx?uid=" + e.CommandArgument.ToString());
            }
            else if (e.CommandName == "Delete")
            {
                if (!HasWriteAccess)
                    return;

                string username = (string) e.CommandArgument;
                Classes.Admin.Delete(username);
                PopulateDataGrid();
            }
        }

        /// <summary>
        /// Handles the Click event of the AddAdmin control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void AddAdmin_Click(object sender, EventArgs e)
        {
            Response.Redirect("EditAdmin.aspx");
        }

        private void gridAdmins_ItemCreated(object sender, DataGridItemEventArgs e)
        {

        }

        private void gridAdmins_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");

            if (((Literal) e.Item.FindControl("litUsername")).Text == Config.Users.SystemUsername)
                lnkDelete.Visible = false;

            if (!HasWriteAccess)
                lnkDelete.Enabled = false;
            else
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this account?")));                
        }
    }
}