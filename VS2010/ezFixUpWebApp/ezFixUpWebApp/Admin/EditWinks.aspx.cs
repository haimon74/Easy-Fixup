using System;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditWinks : AdminPageBase
    {
        protected bool ShowExistedWinkType
        {
            get
            {
                if (ViewState["ShowExistedWinkType"] == null) return false;
                return (bool) ViewState["ShowExistedWinkType"];
            }
            set { ViewState["ShowExistedWinkType"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editEcards;
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                PopulateLanguagesDdl();
                PopulateEcardCategoriesDdlForLanguage(1);
                //populateDDName();
            }
        }

        private void loadStrings()
        {
            if (!HasWriteAccess)
            {
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                btnSaveCategory.Enabled = false;
                btnDeleteCategory.Enabled = false;
            }

            btnSave.Text = Lang.TransA("Save");
            btnDelete.Text = Lang.TransA("Delete");
            btnSaveCategory.Text = Lang.TransA("Save");
            btnDeleteCategory.Text = Lang.TransA("Delete");
        }

        protected void PopulateLanguagesDdl()
        {
            ddLanguage.Items.Clear();
            Language[] languages = Language.FetchAll();
            foreach (Language language in languages)
            {
                ddLanguage.Items.Add(new ListItem(language.Name,language.Id.ToString(),language.Active));
            }
            ddLanguage.SelectedValue = "1";
        }
        protected void PopulateEcardCategoriesDdlForLanguage(int languageId)
        {
            ddCategory.Items.Clear();
            ddCategory.Items.Add(new ListItem("", "-1"));
            EcardCategory[] winkCategories = WinkCategory.FetchCategoriesByLaguageId(languageId, EcardCategory.eSortColumn.Title);
            foreach (WinkCategory wc in winkCategories)
            {
                ddCategory.Items.Add(new ListItem(wc.Title, wc.ID.ToString()));
            }
            ddCategory.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
            ddCategory.SelectedIndex = 0;
        }

        private void populateDDName()
        {
            ddName.Items.Clear();
            ddName.Items.Add(new ListItem("", "-1"));
            int categoryId;
            WinkType[] winks = 
                (int.TryParse(ddCategory.SelectedValue, out categoryId))
                    ? WinkType.FetchWinksForCategory(categoryId)
                    : WinkType.FetchWinksForCategory(null); 
            foreach (WinkType et in winks)
            {
                ddName.Items.Add(new ListItem(et.Name, et.ID.ToString()));
            }

            ddName.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
            ddName.SelectedIndex = 0;
        }
        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            int langId = int.Parse(ddLanguage.SelectedValue);
            PopulateEcardCategoriesDdlForLanguage(langId);
        }
        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCategory.SelectedValue == "-1")
            {
                ShowExistedWinkType = false;
                pnlEditEcardType.Visible = false;
                pnlEditEcardCategory.Visible = false;
                btnSaveCategory.Enabled = true;
                btnDeleteCategory.Enabled = false;
                ddName.Items.Clear();
                return;
            }

            if (ddCategory.SelectedValue == "-2")
            {
                pnlEditEcardCategory.Visible = true;
                pnlEditEcardType.Visible = false;
                ShowExistedWinkType = false;
                btnSaveCategory.Enabled = true;
                txtCategoryName.Text = String.Empty;
                cbCategoryActive.Checked = true;
                ddName.Items.Clear();
                return;
            }
            btnSaveCategory.Enabled = true;
            btnDeleteCategory.Enabled = true;
            ShowExistedWinkType = false;
            pnlEditEcardType.Visible = false;
            pnlEditEcardCategory.Visible = true;
            populateDDName();

            WinkCategory winkCategory = WinkCategory.Fetch(Convert.ToInt32(ddCategory.SelectedValue));
            if (winkCategory != null)
            {
                txtCategoryName.Text = winkCategory.Title;
                //cbCategoryActive.Checked = ecardCategory.Active;
            }
        }
        protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddName.SelectedValue == "-1")
            {
                ShowExistedWinkType = false;
                pnlEditEcardType.Visible = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }

            if (ddName.SelectedValue == "-2")
            {
                ShowExistedWinkType = false;
                txtName.Text = String.Empty;
                txtPhrase.Text = String.Empty;
                cbActive.Checked = true;
                pnlEditEcardType.Visible = true;
                pnlEditEcardCategory.Visible = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                return;
            }

            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            ShowExistedWinkType = true;
            pnlEditEcardType.Visible = true;
            pnlEditEcardCategory.Visible = false;

            int ecardId;
            if (int.TryParse(ddName.SelectedValue, out ecardId))
            {
                WinkType ecardType = WinkType.Fetch(ecardId);

                if (ecardType != null)
                {
                    txtName.Text = ecardType.Name;
                    txtPhrase.Text = ecardType.Phrase;
                    cbActive.Checked = ecardType.Active;
                }
            }
        }

        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (txtCategoryName.Text.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter category name"), Misc.MessageType.Error);
                return;
            }

            WinkCategory winkCategory = null;
            int wcId;
            winkCategory =
                (int.TryParse(ddCategory.SelectedValue, out wcId))
                    ? WinkCategory.Fetch(wcId) ?? new WinkCategory()
                    : new WinkCategory();
            winkCategory.Title = txtCategoryName.Text.Trim();
            int langId = int.Parse(ddLanguage.SelectedValue);
            winkCategory.ParentID = langId;
            winkCategory.IsActive = cbCategoryActive.Checked;
            winkCategory.Save();
            PopulateEcardCategoriesDdlForLanguage(langId);
            txtCategoryName.Text = String.Empty;
            Messagebox1.Show(Lang.TransA("The wink category has been modified successfully!"), Misc.MessageType.Success);
        }
        protected void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            int id = Convert.ToInt32(ddCategory.SelectedValue);
            if (id == -1 || id == -2)
            {
                txtCategoryName.Text = String.Empty;
                cbCategoryActive.Checked = false;
                ddCategory.SelectedIndex = 0;
                pnlEditEcardCategory.Visible = false;
                return;
            }

            WinkCategory.Delete(id);
            txtCategoryName.Text = String.Empty;
            cbCategoryActive.Checked = false;
            int langId = int.Parse(ddLanguage.SelectedValue);
            PopulateEcardCategoriesDdlForLanguage(langId);
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (txtName.Text.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter name"), Misc.MessageType.Error);
                return;
            }
            if (txtPhrase.Text.Length == 0)
            {
                Messagebox1.Show(Lang.TransA("Please enter phrase"), Misc.MessageType.Error);
                return;
            }

            WinkType ecardType = null;

            ecardType = WinkType.Fetch(Convert.ToInt32(ddName.SelectedValue)) ?? new WinkType();

            ecardType.Name = txtName.Text.Trim();
            ecardType.Active = cbActive.Checked;
            ecardType.Phrase = txtPhrase.Text.Trim();
            ecardType.CategoryId = int.Parse(ddCategory.SelectedValue);

            if (ecardType.Name.IsNullOrEmpty() || ecardType.Phrase.IsNullOrEmpty())
            {
                Messagebox1.Show(Lang.TransA("Missing Fields!"), Misc.MessageType.Error);
                return;
            }
            ecardType.Save();

            populateDDName();
            ddName.SelectedValue = ecardType.ID.ToString();
            Messagebox1.Show(Lang.TransA("The wink has been modified successfully!"), Misc.MessageType.Success);    
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            int id = Convert.ToInt32(ddName.SelectedValue);
            if (id == -1 || id == -2)
            {
                txtName.Text = String.Empty;
                cbActive.Checked = false;
                ddName.SelectedIndex = 0;
                pnlEditEcardType.Visible = false;
                return;
            }

            WinkType.Delete(id);
            txtName.Text = String.Empty;
            cbActive.Checked = false;
            populateDDName();
        }
    }
}
