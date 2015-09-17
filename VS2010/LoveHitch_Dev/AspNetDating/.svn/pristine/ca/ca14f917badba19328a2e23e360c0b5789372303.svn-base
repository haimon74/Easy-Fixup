using System;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditEcardTypes : AdminPageBase
    {
        protected bool ShowExistedEcardType
        {
            get
            {
                if (ViewState["ShowExistedEcardType"] == null) return false;
                return (bool) ViewState["ShowExistedEcardType"];
            }
            set { ViewState["ShowExistedEcardType"] = value; }
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
                //populateDDName();
                PopulateEcardCategoriesDdl();
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

        protected void PopulateEcardCategoriesDdl()
        {
            ddCategory.Items.Clear();
            ddCategory.Items.Add(new ListItem("", "-1"));

            foreach (EcardCategory ec in EcardCategory.FetchCategories(EcardCategory.eSortColumn.Title))
            {
                ddCategory.Items.Add(new ListItem(ec.Title, ec.ID.ToString()));
            }
            ddCategory.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
        }

        protected void populateDDName()
        {
            ddName.Items.Clear();
            ddName.Items.Add(new ListItem("", "-1"));
            int categoryId;
            EcardType[] ecardTypes = 
                (int.TryParse(ddCategory.SelectedValue, out categoryId))
                    ? EcardType.FetchEcardsForCategory(categoryId)
                    : EcardType.FetchEcardsForCategory(null);
            
            foreach (EcardType et in ecardTypes)
            {
                ddName.Items.Add(new ListItem(et.Name, et.ID.ToString()));
            }
            ddName.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCategory.SelectedValue == "-1")
            {
                ShowExistedEcardType = false;
                pnlEditEcardType.Visible = false;
                pnlEditEcardCategory.Visible = false;
                btnSaveCategory.Enabled = true;
                btnDeleteCategory.Enabled = false;
                return;
            }

            if (ddCategory.SelectedValue == "-2")
            {
                pnlEditEcardCategory.Visible = true;
                pnlEditEcardType.Visible = false;
                ShowExistedEcardType = false;
                btnSaveCategory.Enabled = true;
                txtCategoryName.Text = String.Empty;
                cbCategoryActive.Checked = true;
                return;
            }
            btnSaveCategory.Enabled = true;
            btnDeleteCategory.Enabled = true;
            ShowExistedEcardType = false;
            pnlEditEcardType.Visible = false;
            pnlEditEcardCategory.Visible = true;
            populateDDName();

            EcardCategory ecardCategory = EcardCategory.Fetch(Convert.ToInt32(ddCategory.SelectedValue));
            if (ecardCategory != null)
            {
                txtCategoryName.Text = ecardCategory.Title;
                //cbCategoryActive.Checked = ecardCategory.Active;
            }
        }
        protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddName.SelectedValue == "-1")
            {
                ShowExistedEcardType = false;
                pnlEditEcardType.Visible = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }

            if (ddName.SelectedValue == "-2")
            {
                ShowExistedEcardType = false;
                txtName.Text = String.Empty;
                cbActive.Checked = true;
                tdImage.Visible = false;
                tdFlash.Visible = false;
                pnlEditEcardType.Visible = true;
                pnlEditEcardCategory.Visible = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                return;
            }

            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            ShowExistedEcardType = true;
            pnlEditEcardType.Visible = true;
            pnlEditEcardCategory.Visible = false;

            int ecardId;
            if (int.TryParse(ddName.SelectedValue, out ecardId))
            {
                EcardType ecardType = EcardType.Fetch(ecardId);

                if (ecardType != null)
                {
                    txtName.Text = ecardType.Name;
                    cbActive.Checked = ecardType.Active;
                    tdImage.Visible = ecardType.Type == EcardType.eType.Image;
                    tdFlash.Visible = !tdImage.Visible;
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

            EcardCategory ecardCategory = null;
            ecardCategory = EcardCategory.Fetch(Convert.ToInt32(ddCategory.SelectedValue)) ?? new EcardCategory();
            ecardCategory.Title = txtCategoryName.Text.Trim();
            //ecardCategory.Active = cbCategoryActive.Checked;
            ecardCategory.Save();

            populateDDName();
            PopulateEcardCategoriesDdl();
            ddCategory.SelectedValue = ecardCategory.ID.ToString();
            Messagebox1.Show(Lang.TransA("The e-card category has been modified successfully!"), Misc.MessageType.Success);    
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

            EcardCategory.Delete(id);
            txtCategoryName.Text = String.Empty;
            cbCategoryActive.Checked = false;
            populateDDName();
            PopulateEcardCategoriesDdl();
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

            byte[] content = new byte[fuContent.PostedFile.InputStream.Length];

            while (true)
            {
                int bytesRead = fuContent.PostedFile.InputStream.Read(content, 0, content.Length);
                if (bytesRead == 0) break;
            }

            string mimeType = String.Empty;
            string extension = System.IO.Path.GetExtension(fuContent.PostedFile.FileName).ToLower();

            if (fuContent.PostedFile.FileName.Length > 0)
            {
                switch (extension)
                {
                    case ".gif":
                        mimeType = "image/gif";
                        break;
                    case ".jpg":
                        mimeType = "image/jpeg";
                        break;
                    case ".jpeg":
                        mimeType = "image/jpeg";
                        break;
                    case ".jpe":
                        mimeType = "image/jpeg";
                        break;
                    case ".png":
                        mimeType = "image/png";
                        break;
                    case ".swf":
                        mimeType = "application/x-shockwave-flash";
                        break;
                    default:
                        Messagebox1.Show(Lang.TransA("Invalid file format!"), Misc.MessageType.Error);
                        return;
                }
            }
            EcardType ecardType = null;
            ecardType = EcardType.Fetch(Convert.ToInt32(ddName.SelectedValue));

            if (ecardType == null)
            {
                ecardType = new EcardType();
            }
            ecardType.Name = txtName.Text.Trim();
            ecardType.Active = cbActive.Checked;
            if (content.Length > 0) ecardType.Content = content;
            else if (!ShowExistedEcardType)
            {
                Messagebox1.Show(Lang.TransA("Invalid file format!"), Misc.MessageType.Error);
                return;
            }
            if (mimeType.Length > 0)
                ecardType.Type = mimeType.Split('/')[0] == "image" ? EcardType.eType.Image : EcardType.eType.Flash;
            ecardType.Save();

            tdImage.Visible = ecardType.Type == EcardType.eType.Image;
            tdFlash.Visible = !tdImage.Visible;

            populateDDName();
            PopulateEcardCategoriesDdl();
            ddName.SelectedValue = ecardType.ID.ToString();
            Messagebox1.Show(Lang.TransA("The e-card has been modified successfully!"), Misc.MessageType.Success);    
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

            EcardType.Delete(id);
            txtName.Text = String.Empty;
            cbActive.Checked = false;
            tdImage.Visible = false;
            tdFlash.Visible = false;
            populateDDName();
            PopulateEcardCategoriesDdl();
        }
    }
}
