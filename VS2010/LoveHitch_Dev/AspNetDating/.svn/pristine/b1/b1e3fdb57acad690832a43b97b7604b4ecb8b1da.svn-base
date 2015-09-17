using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ManageGroupCategories : AdminPageBase
    {
        public int EditedCategoryID
        {
            get
            {
                if (ViewState["EditedCategoryID"] != null)
                {
                    return (int)ViewState["EditedCategoryID"];
                }
                else
                {
                    return -1;
                }
            }

            set
            {
                ViewState["EditedCategoryID"] = value;
                ViewState["EditedCategory"] = null;
            }
        }

        public Category EditedCategory
        {
            get
            {
                if (ViewState["EditedCategory"] == null)
                {
                    ViewState["EditedCategory"] = Category.Fetch(EditedCategoryID);
                }

                return ViewState["EditedCategory"] as Category;
            }

            set
            {
                ViewState["EditedCategory"] = value;
            }
        }

        private DataTable DataSource
        {
            get { return ViewState["CategoriesDataSource"] as DataTable; }
            set { ViewState["CategoriesDataSource"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.manageGroupCategories;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Groups.EnableGroups)
                {
                    StatusPageMessage = Lang.TransA("Groups option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                if (!HasWriteAccess)
                {
                    btnDeleteSelectedCategories.Enabled = false;
                    btnSave.Enabled = false;
                }

                loadStrings();
                populateDataGrid();
                mvCategories.SetActiveView(viewCategories);
            }
        }

        private void populateDataGrid()
        {
            DataTable dtCategories = new DataTable("Categories");

            dtCategories.Columns.Add("CategoryID");
            dtCategories.Columns.Add("Name");

            Category[] categories = Category.Fetch();

            if (categories.Length > 0)
            {
                foreach (Category category in categories)
                {
                    dtCategories.Rows.Add(new object[]
                                              {
                                                  category.ID,
                                                  category.Name
                                              });
                }

                dgCategories.Visible = true;
            }
            else
            {
                MessageBox.Show(Lang.TransA("There are no categories!"), Misc.MessageType.Error);
                dgCategories.Visible = false;
            }

            DataSource = dtCategories;
            dgCategories.DataSource = dtCategories;
            dgCategories.DataBind();
        }

        private void loadStrings()
        {
            dgCategories.Columns[1].HeaderText = Lang.TransA("Name");
            dgCategories.Columns[2].HeaderText = Lang.TransA("Order");
            btnAddNewCategory.Text = Lang.TransA("Add new category");
            btnDeleteSelectedCategories.Text = Lang.TransA("Delete selected category");
            btnDeleteSelectedCategories.Attributes.Add("onclick",
                                                   String.Format("javascript: return confirm('{0}')",
                                                                 Lang.TransA(
                                                                     "Do you really want to delete selected categories?")));
            btnSave.Text = Lang.TransA(" Save Changes >> ");
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void changeOrder(DataGridCommandEventArgs e, int categoryID)
        {
            string direction = (string)(e.CommandArgument);

            switch (direction)
            {
                case "Up":
                    Category.ChangeOrder(categoryID, eDirections.Up);
                    break;
                case "Down":
                    Category.ChangeOrder(categoryID, eDirections.Down);
                    break;
            }
        }

        private void editCategory(DataGridCommandEventArgs e, int categoryID)
        {
            lblText.Text = Lang.TransA("Edit Category Name");
            EditedCategoryID = categoryID;
            txtName.Text = EditedCategory.Name;
            cbUsersCanCreateGroups.Checked = EditedCategory.UsersCanCreateGroups;

            mvCategories.SetActiveView(viewCategory);
        }

        protected void dgCategories_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)e.Item.FindControl("cbSelect");
            int categoryID = Convert.ToInt32(cbSelect.Value);

            switch (e.CommandName)
            {
                case "EditCategory":
                    editCategory(e, categoryID);
                    break;

                case "ChangeOrder":
                    if (!HasWriteAccess) return;
                    changeOrder(e, categoryID);
                    populateDataGrid();
                    break;

                case "ViewGroups" :
                    Response.Redirect("~/Admin/BrowseGroups.aspx?gid=" + Convert.ToInt32(e.CommandArgument));
                    break;
            }
        }

        protected void dgCategories_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            //remove the upper arrow if the current item is the first one
            if (e.Item.ItemIndex == 0)
            {
                (e.Item.FindControl("lnkUp") as LinkButton).Visible = false;
                //return;
            }

            //remove the lower arrow if the current item is the last one
            int lastItemIndex = DataSource.Rows.Count - 1;
            if (e.Item.ItemIndex == lastItemIndex)
                (e.Item.FindControl("lnkDown") as LinkButton).Visible = false;
        }

        protected void dgCategories_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkUp = e.Item.FindControl("lnkUp") as LinkButton;
            LinkButton lnkDown = e.Item.FindControl("lnkDown") as LinkButton;

            if (!HasWriteAccess)
            {
                if (lnkUp != null)
                    lnkUp.Enabled = false;

                if (lnkDown != null)
                    lnkDown.Enabled = false;
            }
        }

        protected void btnAddNewCategory_Click(object sender, EventArgs e)
        {
            lblText.Text = Lang.TransA("Add Category");
            txtName.Text = "";
            cbUsersCanCreateGroups.Checked = false;
            EditedCategoryID = -1;
            EditedCategory = null;
            mvCategories.SetActiveView(viewCategory);
        }

        protected void btnDeleteSelectedCategories_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess) return;

            bool hasCategoryWithGroup = false;

            foreach (DataGridItem item in dgCategories.Items)
            {
                HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)item.FindControl("cbSelect");
                if (cbSelect.Checked)
                {
                    int categoryID = Convert.ToInt32(cbSelect.Value);

                    int groups = Group.FetchGroupsCount(categoryID);

                    if (groups > 0)
                    {
                        hasCategoryWithGroup = true;
                        continue;
                    }
                    
                    Category.Delete(categoryID);
                }
            }

            if (hasCategoryWithGroup)
            {
                MessageBox.Show(Lang.TransA("There are groups in selected categories. Please remove groups first."), Misc.MessageType.Error);
            }

            populateDataGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess) return;

            if (txtName.Text.Trim().Length == 0)
            {
                MessageBox.Show(Lang.TransA("Please enter category name!"), Misc.MessageType.Error);
                return;
            }

            Category category = null;

            if (EditedCategory == null)
            {
                category = new Category();
            }
            else
            {
                category = EditedCategory;
            }

            category.Name = txtName.Text.Trim();
            category.UsersCanCreateGroups = cbUsersCanCreateGroups.Checked;
            category.Save();

            populateDataGrid();

            mvCategories.SetActiveView(viewCategories);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mvCategories.SetActiveView(viewCategories);
        }
    }
}
