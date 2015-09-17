using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class PostAd : PageBase
    {
        protected int? AdID
        {
            get { return (int?) ViewState["AdID"]; }
            set { ViewState["AdID"] = value; }
        }

        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        private void CheckPermission()
        {
            if (CurrentUserSession != null)
            {
                var permissionCheckResult = CurrentUserSession.CanPostAd();

                if (permissionCheckResult == PermissionCheckResult.Yes ||
                    (CurrentUserSession.Level != null
                            && CurrentUserSession.Level.Restrictions.UserCanPostAd))
                {
                }
                else if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.UserCanPostAd;
                    Response.Redirect("ManageProfile.aspx?sel=payment");
                    return;
                }
                else if (permissionCheckResult == PermissionCheckResult.No)
                {
                    StatusPageMessage = Lang.Trans("You are not allowed to post classifieds!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "500px", "200px");

            if (!IsPostBack)
            {
                if (!Config.Ads.Enable)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                CheckPermission();

                loadStrings();

                int adID;
                if (!String.IsNullOrEmpty(Request.Params["aid"]) && Int32.TryParse(Request.Params["aid"], out adID))
                {
                    populateAdData(adID);
                }
            }
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = "Actions".Translate();
            LargeBoxStart1.Title = "Post Classified".Translate();
            btnSave.Text = "Save".Translate();

            ddCategories.Items.Add(new ListItem("", "-1"));
            AdsCategory[] categories = AdsCategory.FetchCategories(AdsCategory.eSortColumn.Title);
            if (categories.Length > 0)
            {
                foreach (var category in categories)
                {
                    if (AdsCategory.FetchSubcategories(category.ID, AdsCategory.eSortColumn.None).Length == 0) continue;
                    ddCategories.Items.Add(new ListItem(category.Title, category.ID.ToString()));
                }
            }
            else
            {
                Response.Redirect("~/Ads.aspx");
                return;
            }

            ddExpiration.Items.Add(new ListItem(1.ToString(), 1.ToString()));
            for (var i = 5; i <= 30; i += 5)
                ddExpiration.Items.Add(new ListItem(i.ToString(), i.ToString()));
            ddExpiration.SelectedValue = "30";

            int categoryID;
            if (!String.IsNullOrEmpty(Request.Params["cid"]) && Int32.TryParse(Request.Params["cid"], out categoryID))
            {
                AdsCategory adsCategory = AdsCategory.Fetch(categoryID);
                ddCategories.SelectedValue = adsCategory.ParentID.ToString();
                ddCategories_SelectedIndexChanged(null, null);
            }
        }

        private void populateAdData(int id)
        {
            Ad ad = Ad.Fetch(id);
            if (ad != null)
            {
                txtSubject.Text = ad.Subject;
                txtLocation.Text = ad.Location;
                if (ckeditor != null)
                    ckeditor.Text = ad.Description;
                else if (htmlEditor != null)
                    htmlEditor.Content = ad.Description;
            }
        }

        protected void ddCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddSubcategories.Items.Clear();

            if (ddCategories.SelectedValue != "-1")
            {
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
            }

            ddSubcategories.Visible = ddCategories.SelectedValue != "-1";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            CheckPermission();

            if (pnlEditAd.Visible)
            {
                string subject = txtSubject.Text.Trim();
                string location = txtLocation.Text.Trim();
                string message = htmlEditor != null ? htmlEditor.Content.Trim() : ckeditor.Text.Trim();
                DateTime expirationDate = DateTime.Now.AddDays(Int32.Parse(ddExpiration.SelectedValue));

                #region validate fields

                if (ddCategories.SelectedValue == "-1")
                {
                    lblError.Text = "Please select category".Translate();
                    return;
                }

                if (subject.Length == 0)
                {
                    lblError.Text = "Please enter subject".Translate();
                    return;
                }

                if (location.Length == 0)
                {
                    lblError.Text = "Please enter location".Translate();
                    return;
                }

                if (message.Length == 0)
                {
                    lblError.Text = "Please enter message".Translate();
                    return;
                }

                #endregion

                Ad ad = null;
                int adID;
                if (!String.IsNullOrEmpty(Request.Params["aid"]) && Int32.TryParse(Request.Params["aid"], out adID))
                {
                    ad = Ad.Fetch(adID);
                }
                else 
                    ad = new Ad(Int32.Parse(ddSubcategories.SelectedValue), CurrentUserSession.Username)
                    {
                        Subject = subject,
                        Location = location,
                        Description = message,
                        ExpirationDate = expirationDate
                    };

                if (ad != null)
                {
                    if (CurrentUserSession.BillingPlanOptions.AutoApproveAds.Value
                                    || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.AutoApproveAds)
                    {
                        ad.Approved = true;
                    }
                    else
                    {
                        ad.Approved = false;
                    }

                    ad.CategoryID = Int32.Parse(ddSubcategories.SelectedValue);
                    ad.Subject = subject;
                    ad.Location = location;
                    ad.Description = message;
                    ad.ExpirationDate = expirationDate;
                    ad.Save();
                    AdID = ad.ID;

                    pnlEditAd.Visible = false;
                    pnlUploadAdPhotos.Visible = true;

                    loadPhotos(ad.ID);
                }
            }
            else
            {
                foreach (RepeaterItem item in rptAddPhoto.Items)
                {
                    TextBox txtAdPhotoDescription = (TextBox) item.FindControl("txtAdPhotoDescription");
                    FileUpload fuAdPhoto = (FileUpload)item.FindControl("fuAdPhoto");

                    if (fuAdPhoto.PostedFile.FileName.Length == 0) continue;

                    System.Drawing.Image image = null;
                    try
                    {
                        image = System.Drawing.Image.FromStream(fuAdPhoto.PostedFile.InputStream);
                    }
                    catch
                    {
                        lblError.Text = Lang.Trans("Invalid image!");
                        return;
                    }

                    Classes.AdPhoto photo = new Classes.AdPhoto(AdID.Value);
                    photo.Image = image;
                    photo.Description = txtAdPhotoDescription.Text.Trim();
                    photo.Save();

                    string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + AdID % 10;
                    string cacheFileMask = String.Format("adID{0}_*.jpg", AdID);
                    foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                    {
                        File.Delete(file);
                    }
                }

                Response.Redirect("~/ShowAd.aspx?id=" + AdID);
            }
        }

        protected void loadPhotos(int adID)
        {
            DataTable dtPhotos = new DataTable();
            dtPhotos.Columns.Add("AdPhotoID", typeof (int));

            Classes.AdPhoto[] adPhotos = Classes.AdPhoto.FetchByAdID(adID);

            int[] numberOfPhotoUploads = new int[Config.Ads.MaxPhotosPerAd - adPhotos.Length];
            rptAddPhoto.DataSource = numberOfPhotoUploads;
            rptAddPhoto.DataBind();

            foreach (var adPhoto in adPhotos)
            {
                dtPhotos.Rows.Add(new object[] {adPhoto.ID});
            }

            rptAdPhotos.DataSource = dtPhotos;
            rptAdPhotos.DataBind();
        }

        protected void rptAdPhotos_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            var lnkDelete = (LinkButton) e.Item.FindControl("lnkDelete");
            lnkDelete.Text = "Delete".Translate();
        }

        protected void rptAdPhotos_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeletePhoto")
            {
                Classes.AdPhoto.Delete(Convert.ToInt32(e.CommandArgument));

                loadPhotos(AdID.Value);
            }
        }
    }
}
