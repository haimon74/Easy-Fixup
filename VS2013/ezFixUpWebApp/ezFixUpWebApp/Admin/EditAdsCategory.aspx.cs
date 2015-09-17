using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditAdsCategory : AdminPageBase
    {
        private int categoryID;
        private string CategoryID
        {
            get
            {
                string param = Request.Params["acid"];

                if (param == null || param.Trim() == String.Empty)
                {
                    return null;
                }
                else
                {
                    foreach (char ch in param)
                    {
                        if (Char.IsLetter(ch))
                        {
                            return null;
                        }
                    }
                }

                return param;
            }
        }

        private DataTable Subcategories
        {
            get
            {
                if (Global.GetSessionState()["Subcategories"] == null)
                {
                    DataTable dtSubcategories = new DataTable();
                    dtSubcategories.Columns.Add("ID");
                    dtSubcategories.Columns.Add("Title");
                    dtSubcategories.PrimaryKey = new DataColumn[] { dtSubcategories.Columns["ID"] };

                    Global.GetSessionState()["Subcategories"] = dtSubcategories;

                    AdsCategory[] subcategories = AdsCategory.FetchSubcategories(categoryID, AdsCategory.eSortColumn.Title);

                    if (subcategories.Length > 0)
                    {
                        foreach (AdsCategory subcategory in subcategories)
                        {
                            dtSubcategories.Rows.Add(new object[] { subcategory.ID, subcategory.Title });
                        }
                    }
                    return dtSubcategories;
                }
                else
                {
                    return (DataTable)Global.GetSessionState()["Subcategories"];
                }
            }

            set { Global.GetSessionState()["Subcategories"] = value; }
        }

        private string NewTempID
        {
            get
            {
                if (Global.GetSessionState()["LastCount"] == null)
                {
                    Global.GetSessionState()["LastCount"] = 0;
                }

                int id = (int)Global.GetSessionState()["LastCount"];
                ++id;
                Global.GetSessionState()["LastCount"] = id;
                return "TempID" + id.ToString();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editAdsCategories;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CategoryID == null)
            {
                return;
            }
            categoryID = Convert.ToInt32(CategoryID);

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnDeleteSelectedSubcategories.Enabled = false;
                    btnSave.Enabled = false;
                    btnAddNewSubcategories.Enabled = false;
                }
                LoadStrings();
                PopulateDataFields();
                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            for (int i = 1; i <= 5; ++i)
            {
                dropNewSubcategoriesCount.Items.Add(i.ToString());
            }

            btnAddNewSubcategories.Text = Lang.TransA("Add");
            btnCancel.Text = Lang.TransA("Cancel");
            btnSave.Text = Lang.TransA("Save");

            btnDeleteSelectedSubcategories.Text = Lang.TransA("Delete selected sub categories");
            btnDeleteSelectedSubcategories.Attributes.Add("onclick",
                                                    String.Format("javascript: return confirm('{0}')",
                                                                  Lang.TransA(
                                                                      "Do you really want to delete selected sub categories?")));
            Subcategories = null;
        }

        private void PopulateDataFields()
        {
            AdsCategory category = AdsCategory.Fetch(categoryID);

            if (category != null) txtCategoryTitle.Text = category.Title;
        }

        private void PopulateDataGrid()
        {
            dgSubcategories.DataSource = Subcategories;
            dgSubcategories.DataBind();

            if (Subcategories.Rows.Count == 0)
            {
                MessageBox.Show(Lang.TransA("There are no existing sub categories for that category! " +
                                           "Please click on \"Add new sub category\" to create new one."), Misc.MessageType.Error);
                dgSubcategories.Visible = false;
            }
            else
            {
                dgSubcategories.Visible = true;
            }
        }

        private void UpdateDataSource()
        {
            foreach (DataGridItem item in dgSubcategories.Items)
            {
                HtmlInputCheckBox cbSelect = (HtmlInputCheckBox)item.FindControl("cbSelect");
                TextBox txtTitle = (TextBox)item.FindControl("txtTitle");

                string id = cbSelect.Value;
                string title = txtTitle.Text;

                Subcategories.Rows.Find(id)["Title"] = title;
            }
        }

        protected void btnAddNewSubcategories_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            UpdateDataSource();
            for (int i = 0; i < Convert.ToInt32(dropNewSubcategoriesCount.SelectedValue); ++i)
            {
                Subcategories.Rows.Add(new object[] { NewTempID, "" });
            }
            PopulateDataGrid();
        }

        protected void btnDeleteSelectedSubcategories_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            UpdateDataSource();

            foreach (DataGridItem item in dgSubcategories.Items)
            {
                HtmlInputCheckBox cbSelect =
                    (HtmlInputCheckBox)item.FindControl("cbSelect");
                if (cbSelect.Checked)
                {
                    string id = cbSelect.Value;
                    if (id.IndexOf("Temp") == -1)
                    {
                        AdsCategory.Delete((Convert.ToInt32(id)));
                    }

                    Subcategories.Rows.Find(id).Delete();
                }
            }

            PopulateDataGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            AdsCategory category = AdsCategory.Fetch(categoryID);
            category.Title = txtCategoryTitle.Text.Trim();

            foreach (DataGridItem item in dgSubcategories.Items)
            {
                HtmlInputCheckBox cbSelect =
                    (HtmlInputCheckBox)item.FindControl("cbSelect");
                TextBox txtTitle = (TextBox)item.FindControl("txtTitle");

                string id = cbSelect.Value;
                string title = txtTitle.Text;

                if (title.Trim() != String.Empty)
                {
                    AdsCategory subcategory;

                    subcategory = id.IndexOf("Temp") != -1
                                      ? new AdsCategory(Convert.ToInt32(categoryID))
                                      : AdsCategory.Fetch(Convert.ToInt32(id));
                    subcategory.Title = title;
                    subcategory.Save();
                }
                Subcategories = null;
            }

            category.Save();

            Response.Redirect("EditAdsCategories.aspx?");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Subcategories = null;
            Response.Redirect("EditAdsCategories.aspx?");
        }
    }
}
