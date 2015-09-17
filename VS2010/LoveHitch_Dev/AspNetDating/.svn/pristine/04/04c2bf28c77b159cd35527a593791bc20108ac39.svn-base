using System;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditGifts : AdminPageBase
    {
        protected bool ShowExistedGiftType
        {
            get
            {
                if (ViewState["ShowExistedGiftType"] == null) return false;
                return (bool) ViewState["ShowExistedGiftType"];
            }
            set { ViewState["ShowExistedGiftType"] = value; }
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
                PopulateGiftCategoriesDdl();
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

        protected void PopulateGiftCategoriesDdl()
        {
            ddCategory.Items.Clear();
            ddCategory.Items.Add(new ListItem("", "-1"));
            GiftCategory[] gifts = GiftCategory.FetchCategories(EcardCategory.eSortColumn.Title);
            foreach (GiftCategory gc in gifts)
            {
                ddCategory.Items.Add(new ListItem(gc.Title, gc.ID.ToString()));
            }
            ddCategory.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
        }

        private void populateDDName()
        {
            ddName.Items.Clear();

            ddName.Items.Add(new ListItem("", "-1"));

            int categoryId;
            GiftType[] gifts =
                (int.TryParse(ddCategory.SelectedValue, out categoryId))
                    ? GiftType.FetchGiftsForCategory(categoryId)
                    : GiftType.FetchGiftsForCategory(null);
            foreach (GiftType et in gifts)
            {
                ddName.Items.Add(new ListItem(et.Name, et.ID.ToString()));
            }

            ddName.Items.Add(new ListItem(Lang.TransA("- Add new -"), "-2"));
        }

        protected void ddCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool emptySelected = (ddCategory.SelectedValue == "-1");
            bool addNewSelected = (ddCategory.SelectedValue == "-2");
            if ( emptySelected || addNewSelected)
            {
                ShowExistedGiftType = false;
                pnlEditGiftType.Visible = false;
                btnSaveCategory.Enabled = true;
                ddName.Items.Clear();
                if (emptySelected)
                {
                    pnlEditGiftCategory.Visible = false;
                    btnDeleteCategory.Enabled = false;
                }
                else //if (addNewSelected)
                {
                    pnlEditGiftCategory.Visible = true;
                    txtCategoryName.Text = String.Empty;
                    cbCategoryActive.Checked = true;
                }
                return;
            }
            btnSaveCategory.Enabled = true;
            btnDeleteCategory.Enabled = true;
            ShowExistedGiftType = false;
            pnlEditGiftType.Visible = false;
            pnlEditGiftCategory.Visible = true;
            populateDDName();

            GiftCategory giftCategory = GiftCategory.Fetch(Convert.ToInt32(ddCategory.SelectedValue));
            if (giftCategory != null)
            {
                txtCategoryName.Text = giftCategory.Title;
                //cbCategoryActive.Checked = ecardCategory.Active;
            }
        }
        protected void ddName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddName.SelectedValue == "-1")
            {
                ShowExistedGiftType = false;
                pnlEditGiftType.Visible = false;
                btnSave.Enabled = false;
                btnDelete.Enabled = false;
                return;
            }

            if (ddName.SelectedValue == "-2")
            {
                ShowExistedGiftType = false;
                txtName.Text = String.Empty;
                cbActive.Checked = true;
                tdImage.Visible = false;
                pnlEditGiftType.Visible = true;
                pnlEditGiftCategory.Visible = false;
                btnSave.Enabled = true;
                btnDelete.Enabled = false;
                return;
            }

            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            ShowExistedGiftType = true;
            pnlEditGiftType.Visible = true;
            pnlEditGiftCategory.Visible = false;

            int giftId;
            if (int.TryParse(ddName.SelectedValue, out giftId))
            {
                GiftType giftType = GiftType.Fetch(giftId);

                if (giftType != null)
                {
                    txtName.Text = giftType.Name;
                    txtPhrase.Text = giftType.Phrase;
                    txtPrice.Text = giftType.Price.ToString();
                    cbActive.Checked = giftType.Active;
                    tdImage.Visible = true;
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

            GiftCategory giftCategory = null;
            int gcId;
            giftCategory = 
                (int.TryParse(ddCategory.SelectedValue, out gcId)) 
                    ? GiftCategory.Fetch(gcId) ?? new GiftCategory()
                    : new GiftCategory();
            giftCategory.Title = txtCategoryName.Text.Trim();
            //ecardCategory.Active = cbCategoryActive.Checked;
            giftCategory.Save();

            PopulateGiftCategoriesDdl();
            ddCategory.SelectedValue = giftCategory.ID.ToString();
            Messagebox1.Show(Lang.TransA("The gift category has been modified successfully!"), Misc.MessageType.Success);
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
                pnlEditGiftCategory.Visible = false;
                return;
            }

            GiftCategory.Delete(id);
            txtCategoryName.Text = String.Empty;
            cbCategoryActive.Checked = false;
            PopulateGiftCategoriesDdl();
            populateDDName();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (txtName.Text.IsNullOrEmpty())
            {
                Messagebox1.Show(Lang.TransA("Please enter name"), Misc.MessageType.Error);
                return;
            }
            int price;
            if (txtPrice.Text.IsNullOrEmpty() || !int.TryParse(txtPrice.Text, out price))
            {
                Messagebox1.Show(Lang.TransA("Please enter integer credits price"), Misc.MessageType.Error);
                return;
            }
            
            if (txtPhrase.Text.IsNullOrEmpty())
            {
                Messagebox1.Show(Lang.TransA("Please enter phrase"), Misc.MessageType.Error);
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
                    case ".jpeg":
                    case ".jpe":
                        mimeType = "image/jpeg";
                        break;
                    case ".png":
                        mimeType = "image/png";
                        break;
                    default:
                        Messagebox1.Show(Lang.TransA("Invalid image file format!"), Misc.MessageType.Error);
                        return;
                }
            }
            
            GiftType giftType = null;

            giftType = GiftType.Fetch(Convert.ToInt32(ddName.SelectedValue)) ?? new GiftType();

            giftType.Name = txtName.Text.Trim();
            giftType.Phrase = txtPhrase.Text.Trim();
            giftType.Active = cbActive.Checked;
            giftType.CategoryId = int.Parse(ddCategory.SelectedValue);
            giftType.Price = price;

            if (content.Length > 0) giftType.Content = content;
            else if (mimeType.Length > 0 || ShowExistedGiftType)
            {
                giftType.Save();
                
                tdImage.Visible = true;

                populateDDName();
                ddName.SelectedValue = giftType.ID.ToString();
                Messagebox1.Show(Lang.TransA("The gift has been modified successfully!"), Misc.MessageType.Success);
            }
            else 
            {
                Messagebox1.Show(Lang.TransA("Invalid file format!"), Misc.MessageType.Error);
                return;
            }
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
                pnlEditGiftType.Visible = false;
                return;
            }

            GiftType.Delete(id);
            txtName.Text = String.Empty;
            cbActive.Checked = false;
            tdImage.Visible = false;
            populateDDName();
        }
    }
}
