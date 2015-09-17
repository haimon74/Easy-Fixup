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
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class CreditsPackages : AdminPageBase
    {
        protected int? CurrentPackageID
        {
            get { return ViewState["CurrentPackageID"] as int?; }
            set { ViewState["CurrentPackageID"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.browseCreditsPackages;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadPackages();
                mvCreditsPackages.SetActiveView(viewCreditsPackages);
            }
        }

        private void loadStrings()
        {
            dgCreditsPackages.Columns[0].HeaderText = Lang.TransA("Name");
            dgCreditsPackages.Columns[1].HeaderText = Lang.TransA("Quantity");
            dgCreditsPackages.Columns[2].HeaderText = Lang.TransA("Price");

            lblCreditsPackages.Text = Lang.TransA("Credits Packages");
            btnAddNewPackage.Text = Lang.TransA("Add New Package");
            btnSave.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");

            if (!HasWriteAccess)
                btnSave.Enabled = false;
        }

        private void loadPackages()
        {
            DataTable dtPackages = new DataTable("CreditsPackages");

            dtPackages.Columns.Add("ID");
            dtPackages.Columns.Add("Name");
            dtPackages.Columns.Add("Quantity");
            dtPackages.Columns.Add("Price");

            CreditsPackage[] creditsPackages = CreditsPackage.Fetch();

            if (creditsPackages.Length > 0)
            {
                foreach (CreditsPackage package in creditsPackages)
                {
                    dtPackages.Rows.Add(new object[]
                                            {
                                                package.ID,
                                                package.Name,
                                                package.Quantity, 
                                                package.Price.ToString("C")
                                            });
                }
            }
            else
            {
                MessageBox1.Show(Lang.TransA("There are no credits packages"), Misc.MessageType.Error);
            }

            lblCreditsPackages.Visible = dtPackages.Rows.Count > 0;
            dgCreditsPackages.Visible = dtPackages.Rows.Count > 0;

            dgCreditsPackages.DataSource = dtPackages;
            dgCreditsPackages.DataBind();
        }

        protected void btnAddNewPackage_Click(object sender, EventArgs e)
        {
            txtName.Text = String.Empty;
            txtQuantity.Text = String.Empty;
            txtPrice.Text = String.Empty;

            CurrentPackageID = null;

            mvCreditsPackages.SetActiveView(viewAddEditCreditsPackage);
        }

        protected void dgCreditsPackages_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            int id = Convert.ToInt32(e.CommandArgument);

            switch (e.CommandName)
            {
                case "Edit" :
                    CreditsPackage creditsPackage = CreditsPackage.Fetch(id);

                    if (creditsPackage != null)
                    {
                        CurrentPackageID = id;
                        txtName.Text = creditsPackage.Name;
                        txtQuantity.Text = creditsPackage.Quantity.ToString();
                        txtPrice.Text = creditsPackage.Price.ToString(".00");
                        mvCreditsPackages.SetActiveView(viewAddEditCreditsPackage);
                    }
                    break;
                case "Delete" :
                    CreditsPackage.Delete(id);
                    loadPackages();
                    mvCreditsPackages.SetActiveView(viewCreditsPackages);
                    break;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            #region validation

            string name = txtName.Text.Trim();
            int quantity = 0;
            decimal price = 0;

            if (name.Length == 0)
            {
                MessageBox1.Show(Lang.TransA("Please enter name"), Misc.MessageType.Error);
                return;
            }

            if (!Int32.TryParse(txtQuantity.Text.Trim(), out quantity))
            {
                MessageBox1.Show(Lang.TransA("Please enter valid quantity"), Misc.MessageType.Error);
                return;
            }

            if (quantity < 0)
            {
                MessageBox1.Show(Lang.TransA("The quantity can't be negative!"), Misc.MessageType.Error);
                return;
            }

            if (!Decimal.TryParse(txtPrice.Text.Trim(), out price))
            {
                MessageBox1.Show(Lang.TransA("Please enter valid price"), Misc.MessageType.Error);
                return;
            }

            if (price < 0)
            {
                MessageBox1.Show(Lang.TransA("The price can't be negative!"), Misc.MessageType.Error);
                return;
            }

            #endregion

            CreditsPackage creditsPackage;
            
            if (CurrentPackageID.HasValue)
            {
                creditsPackage = CreditsPackage.Fetch(CurrentPackageID.Value);                
            }
            else
            {
                creditsPackage = new CreditsPackage();
            } 

            creditsPackage.Name = name;
            creditsPackage.Quantity = quantity;
            creditsPackage.Price = price;

            creditsPackage.Save();

            loadPackages();

            mvCreditsPackages.SetActiveView(viewCreditsPackages);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvCreditsPackages.SetActiveView(viewCreditsPackages);
        }

        protected void dgCreditsPackages_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            LinkButton lnkEdit = (LinkButton)e.Item.FindControl("lnkEdit");

            if (!HasWriteAccess)
            {
                lnkDelete.Enabled = false;
                lnkEdit.Enabled = false;
            }
            else
                lnkDelete.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to delete this package?")));
        }
    }
}
