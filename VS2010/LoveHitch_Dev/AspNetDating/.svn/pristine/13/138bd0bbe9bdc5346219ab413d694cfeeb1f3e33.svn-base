using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditAdsCategories : AdminPageBase
    {
        private DataTable DataSource
        {
            get { return ViewState["AdsCategoriesDataSource"] as DataTable; }
            set { ViewState["AdsCategoriesDataSource"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editAdsCategories;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadStrings();
            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAddNewCategory.Enabled = false;
                    btnDeleteSelectedCategories.Enabled = false;
                }

                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            dgAdsCategories.Columns[1].HeaderText = Lang.TransA("Title");

            btnAddNewCategory.Text = Lang.TransA("Add new category");
            btnDeleteSelectedCategories.Text = Lang.TransA("Delete selected category");
            btnDeleteSelectedCategories.Attributes.Add("onclick",
                                                   String.Format("javascript: return confirm('{0}')",
                                                                 Lang.TransA(
                                                                     "Do you really want to delete selected categories?")));
        }

        private void PopulateDataGrid()
        {
            AdsCategory[] categories = AdsCategory.FetchCategories(AdsCategory.eSortColumn.Title);

            if (categories.Length == 0)
            {
                MessageBox.Show(
                    Lang.TransA("There are no existing categories! Please click on \"Add new category\" to create new one."),
                    Misc.MessageType.Error);
                dgAdsCategories.Visible = false;
            }
            else
            {
                BindTopicDetails(categories);

                dgAdsCategories.Visible = true;
            }
        }

        private void BindTopicDetails(AdsCategory[] categories)
        {
            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("ID");
            dtCategories.Columns.Add("Title");

            foreach (AdsCategory category in categories)
            {
                dtCategories.Rows.Add(new object[]
                                      {
                                          category.ID,
                                          category.Title,
                                      }
                    );
            }

            DataSource = dtCategories;

            dgAdsCategories.DataSource = dtCategories;
            dgAdsCategories.DataBind();
        }

        private void EditCategory(DataGridCommandEventArgs e, int categoryID)
        {
            string url = String.Format("EditAdsCategory.aspx?acid={0}&new=1", categoryID);
            Response.Redirect(url);
        }

        protected void dgAdsCategories_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)e.Item.FindControl("cbSelect");
            int categoryID = Convert.ToInt32(cbSelect.Value);

            if (e.CommandName == "EditCategory")
            {
                EditCategory(e, categoryID);
            }
        }

        protected void btnAddNewCategory_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            AdsCategory category = new AdsCategory();
            category.Title = "NewCategory";
            category.Save();
            PopulateDataGrid();
        }

        protected void btnDeleteSelectedCategories_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            foreach (DataGridItem item in dgAdsCategories.Items)
            {
                HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)item.FindControl("cbSelect");
                if (cbSelect.Checked)
                {
                    int categoryID = Convert.ToInt32(cbSelect.Value);
                    AdsCategory.Delete(categoryID);
                }
            }
            PopulateDataGrid();
        }
    }
}
