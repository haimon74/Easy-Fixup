using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class EditAdCtrl : System.Web.UI.UserControl
    {
        public int AdID
        {
            get { return (int)ViewState["CurrentAdID"]; }
            set { ViewState["CurrentAdID"] = value; }
        }

        public Ad Ad
        {
            get
            {
                if (Page is EditAd)
                {
                    return ((EditAd)Page).Ad;
                }
                return Ad.Fetch(AdID);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            loadStrings();
            loadAdData();
        }

        private void loadStrings()
        {
            AdsCategory[] categories = AdsCategory.FetchCategories(AdsCategory.eSortColumn.Title);
            if (categories.Length > 0)
            {
                foreach (var category in categories)
                {
                    ddCategories.Items.Add(new ListItem(category.Title, category.ID.ToString()));
                }
            }

            btnSave.Text = Lang.TransA(" Save Changes >> ");
            if (!((AdminPageBase)Page).HasWriteAccess) btnSave.Enabled = false;
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void loadAdData()
        {
            #region Populate Controls

            AdsCategory category = AdsCategory.Fetch(Ad.CategoryID);
            if (category != null)
            {
                ddCategories.SelectedValue = category.ParentID.ToString();
                ddCategories_SelectedIndexChanged(null, null);
                txtExpiratoinDate.Text = Ad.ExpirationDate.ToShortDateString();
                txtLocation.Text = Ad.Location;
                txtSubject.Text = Ad.Subject;
                txtDescription.Text = Ad.Description;

                ddApproved.Items.Add(new ListItem(Lang.TransA("Yes")));
                ddApproved.Items.Add(new ListItem(Lang.TransA("No")));

                ddApproved.SelectedIndex = Ad.Approved ? 0 : 1;
            }

            #endregion
        }

        protected void loadPhotos(int adID)
        {
            DataTable dtPhotos = new DataTable();
            dtPhotos.Columns.Add("AdPhotoID", typeof(int));

            Classes.AdPhoto[] adPhotos = Classes.AdPhoto.FetchByAdID(adID);

            int[] numberOfPhotoUploads = new int[Config.Ads.MaxPhotosPerAd - adPhotos.Length];
            rptAddPhoto.DataSource = numberOfPhotoUploads;
            rptAddPhoto.DataBind();

            foreach (var adPhoto in adPhotos)
            {
                dtPhotos.Rows.Add(new object[] { adPhoto.ID });
            }

            rptAdPhotos.DataSource = dtPhotos;
            rptAdPhotos.DataBind();
        }

        protected void ddCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddSubcategories.Items.Clear();

            AdsCategory[] subcategories = AdsCategory.FetchSubcategories(Int32.Parse(ddCategories.SelectedValue),
                                                                             AdsCategory.eSortColumn.Title);
            foreach (var subcategory in subcategories)
            {
                ddSubcategories.Items.Add(new ListItem(subcategory.Title, subcategory.ID.ToString()));
            }

            int categoryID;
            if (!IsPostBack
                && !String.IsNullOrEmpty(Request.Params["cid"])
                && Int32.TryParse(Request.Params["cid"], out categoryID))
            {
                ddSubcategories.SelectedValue = categoryID.ToString();
            }

            ddSubcategories.Visible = ddCategories.SelectedValue != "-1";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (pnlEditAd.Visible)
            {
                string subject = txtSubject.Text.Trim();
                string location = txtLocation.Text.Trim();
                string description = txtDescription.Text.Trim();
                DateTime expirationDate = DateTime.Parse(txtExpiratoinDate.Text.Trim());

                #region validate fields

                if (subject.Length == 0)
                {
                    MessageBox.Show("Please enter subject".TranslateA(), Misc.MessageType.Error);
                    return;
                }

                if (location.Length == 0)
                {
                    MessageBox.Show("Please enter location".TranslateA(), Misc.MessageType.Error);
                    return;
                }

                if (description.Length == 0)
                {
                    MessageBox.Show("Please enter message".TranslateA(), Misc.MessageType.Error);
                    return;
                }

                #endregion

                Ad.CategoryID = Int32.Parse(ddSubcategories.SelectedValue);
                Ad.Subject = subject;
                Ad.Location = location;
                Ad.Description = description;
                Ad.ExpirationDate = expirationDate;
                Ad.Approved = ddApproved.SelectedIndex == 0 ? true : false;
                Ad.Save();

                pnlEditAd.Visible = false;
                pnlUploadAdPhotos.Visible = true;

                loadPhotos(Ad.ID);
            }
            else
            {
                foreach (RepeaterItem item in rptAddPhoto.Items)
                {
                    TextBox txtAdPhotoDescription = (TextBox) item.FindControl("txtAdPhotoDescription");
                    FileUpload fuAdPhoto = (FileUpload) item.FindControl("fuAdPhoto");

                    if (fuAdPhoto.PostedFile.FileName.Length == 0) continue;

                    System.Drawing.Image image = null;
                    try
                    {
                        image = System.Drawing.Image.FromStream(fuAdPhoto.PostedFile.InputStream);
                    }
                    catch
                    {
                        MessageBox.Show(Lang.TransA("Invalid image!"), Misc.MessageType.Error);
                        return;
                    }

                    var photo = new Classes.AdPhoto(AdID)
                                    {
                                        Image = image,
                                        Description = txtAdPhotoDescription.Text.Trim()
                                    };
                    photo.Save();

                    string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + AdID%10;
                    string cacheFileMask = String.Format("adID{0}_*.jpg", AdID);
                    foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                    {
                        File.Delete(file);
                    }
                }

                Response.Redirect("~/Admin/ApproveAds.aspx");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Admin/ApproveAds.aspx");
        }

        protected void rptAdPhotos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeletePhoto")
            {
                Classes.AdPhoto.Delete(Convert.ToInt32(e.CommandArgument));

                loadPhotos(AdID);
            }
        }

        protected void rptAdPhotos_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            var lnkDelete = (LinkButton)e.Item.FindControl("lnkDelete");
            lnkDelete.Text = "Delete".TranslateA();
        }
    }
}