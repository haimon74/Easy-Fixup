using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ShowAd : PageBase
    {
        private delegate string Parser(string s);

        public ShowAd()
        {
            RequiresAuthorization = Config.Ads.OnlyRegisteredUsersCanBrowseClassifieds;
        }

        public int AdID
        {
            get
            {
                if (ViewState["AdID"] != null)
                {
                    return (int)ViewState["AdID"];
                }
                return -1;
            }
            set
            {
                ViewState["AdID"] = value;
            }
        }

        protected string PostedBy
        {
            get { return ViewState["PostedBy"] as string; }
            set { ViewState["PostedBy"] = value; }
        }

        protected int CategoryID
        {
            get
            {
                if (ViewState["CategoryID"] != null)
                {
                    return (int)ViewState["CategoryID"];
                }
                return -1;
            }
            set
            {
                ViewState["CategoryID"] = value;
            }
        }

        protected int CurrentPhotoId
        {
            get
            {
                if (ViewState["CurrentPhotoId"] != null)
                {
                    return (int)ViewState["CurrentPhotoId"];
                }
                return -1;
            }
            set { ViewState["CurrentPhotoId"] = value; }
        }

        protected int[] PhotosIDs
        {
            get
            {
                if (ViewState["PhotosIDs"] != null)
                {
                    return (int[])ViewState["PhotosIDs"];
                }
                return new int[0];
            }
            set { ViewState["PhotosIDs"] = value; }
        }

        private bool loadComments;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            if (!Config.Ads.Enable)
            {
                Response.Redirect("~/Default.aspx");
            }

            if (CurrentUserSession != null)
            {
                var permissionCheckResult = CurrentUserSession.CanBrowseClassifieds();

                if (permissionCheckResult == PermissionCheckResult.Yes ||
                    (CurrentUserSession.Level != null
                            && CurrentUserSession.Level.Restrictions.UserCanBrowseClassifieds))
                {
                }
                else if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.UserCanBrowseClassifieds;
                    Response.Redirect("ManageProfile.aspx?sel=payment");
                    return;
                }
                else if (permissionCheckResult == PermissionCheckResult.No)
                {
                    StatusPageMessage = Lang.Trans("You are not allowed to browse classifieds!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }
            }

            int id;
            if (String.IsNullOrEmpty(Request.Params["id"]) || !Int32.TryParse(Request.Params["id"], out id))
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            AdID = id;
            loadAd(id);
            loadStrings();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            loadComments = Config.Ads.EnableAdComments;

            if (loadComments)
            {
                LoadComments();
                pnlUserComments.Visible = true;
            }
            else
            {
                pnlUserComments.Visible = false;
            }
        }

        private void loadStrings()
        {
            
        }

        private void loadAd(int id)
        {
            Ad ad = Ad.Fetch(id);

            if (ad != null)
            {
                CategoryID = ad.CategoryID;
                PostedBy = ad.PostedBy;

                lblSubject.Text = Server.HtmlEncode(ad.Subject);
                lblPostedBy.Text = ad.PostedBy;
                lblDate.Text = ad.Date.ToShortDateString();
                lblLocation.Text = ad.Location;
                lblDescription.Text = ad.Description;
                lnkPostedBy.HRef = UrlRewrite.CreateShowUserUrl(ad.PostedBy);
                lnkBackToCategory.HRef = "Ads.aspx?cid=" + CategoryID;
                lnkEdit.HRef = String.Format("PostAd.aspx?cid={0}&aid={1}", ad.CategoryID, id);

                if (CurrentUserSession != null) pnlEdit.Visible = ad.PostedBy == CurrentUserSession.Username;
                if (CurrentUserSession != null) pnlDelete.Visible = ad.PostedBy == CurrentUserSession.Username;

                #region load photos

                DataTable dtPhotos = new DataTable();
                dtPhotos.Columns.Add("ID", typeof(int));
                dtPhotos.Columns.Add("Description");

                Classes.AdPhoto[] adPhotos = Classes.AdPhoto.FetchByAdID(id);

                if (adPhotos.Length > 0)
                {
                    if (ltrPhoto.Text == String.Empty)
                    {
                        CurrentPhotoId = adPhotos[0].ID;
                        ltrPhoto.Text = AdPhoto.RenderImageTag(adPhotos[0].ID, 450, 450, "photoframe", true);
                        lblAdPhotoDescription.Text = Server.HtmlEncode(adPhotos[0].Description);
                    }

                    var lPhotosIDs = new List<int>();
                    foreach (Classes.AdPhoto photo in adPhotos)
                    {
                        lPhotosIDs.Add(photo.ID);
                        dtPhotos.Rows.Add(new object[] { photo.ID, photo.Description });
                    }

                    PhotosIDs = lPhotosIDs.ToArray();
                }
                else
                {
                    ltrPhoto.Text = AdPhoto.RenderImageTag(0, 450, 450, "photoframe", true);              
                }

                dlPhotos.DataSource = dtPhotos;
                dlPhotos.DataBind();
                dlPhotos.Visible = dtPhotos.Rows.Count > 0;

                #endregion

                #region Load Strings

                SmallBoxStart1.Title = "Actions".Translate();
                SmallBoxStart2.Title = "Search".Translate();
                LargeBoxStart1.Title = "Classified information".Translate();
                lnkEdit.InnerText = "Edit".Translate();
                lnkDelete.Text = "Delete".Translate();
                fbSearch.Text = "Search".Translate();
                hlUserComments.Title = Lang.Trans("User Comments");
                btnSubmitNewComment.Text = Lang.Trans("Submit Comment");
                lnkViewAllComments.Text = Lang.Trans("View All Comments");

                AdsCategory category = AdsCategory.Fetch(CategoryID);

                Parser parse = delegate(string text)
                {
                    string result =
                        text.Replace("%%SUBJECT%%", ad.Subject)
                        .Replace("%%CATEGORY%%", category != null ? category.Title : "")
                        .Replace("%%DATE%%", ad.Date.ToShortDateString())
                        .Replace("%%EXPIRATIONDATE%%", ad.ExpirationDate.ToShortDateString())
                        .Replace("%%LOCATION%%", ad.Location)
                        .Replace("%%POSTEDBY%%", ad.PostedBy);

                    return result;
                };

                Header.Title = parse(Config.SEO.ShowAdTitleTemplate);

                HtmlMeta metaDesc = new HtmlMeta();
                metaDesc.ID = "Description";
                metaDesc.Name = "description";
                metaDesc.Content = parse(Config.SEO.ShowAdMetaDescriptionTemplate);
                Header.Controls.Add(metaDesc);

                HtmlMeta metaKeywords = new HtmlMeta();
                metaKeywords.ID = "Keywords";
                metaKeywords.Name = "keywords";
                metaKeywords.Content = parse(Config.SEO.ShowAdMetaKeywordsTemplate);
                Header.Controls.Add(metaKeywords);

                if (category != null)
                    lnkBackToCategory.InnerText = String.Format("Back to {0}".Translate(), category.Title);

                if (CurrentUserSession == null)
                {
                    pnlEdit.Visible = false;
                    pnlDelete.Visible = false;
                    pnlMyAds.Visible = false;
                }

                #endregion
            }
        }

        private void LoadComments()
        {
            if (AdID != -1)
            {
                var dtComments = new DataTable();
                dtComments.Columns.Add("ID", typeof(int));
                dtComments.Columns.Add("Date", typeof(DateTime));
                dtComments.Columns.Add("Username", typeof(string));
                dtComments.Columns.Add("Comment", typeof(string));
                dtComments.Columns.Add("CanDelete", typeof(bool));

                int? countLimit = null;
                if (ViewState["ViewPhotos_ViewAllComments"] == null) countLimit = 5;
                AdComment[] comments = countLimit.HasValue
                                              ? AdComment.FetchByAdID(AdID, countLimit.Value, AdComment.eSortColumn.Date)
                                              : AdComment.FetchByAdID(AdID, AdComment.eSortColumn.Date);
                if (comments.Length < 5) divViewAllComments.Visible = false;

                if (CurrentUserSession != null)
                    showHideComments();
                else
                    spanAddNewComment.Visible = false;

                Ad ad = Ad.Fetch(AdID);
                foreach (AdComment comment in comments)
                {
                    bool canDelete = false;
                    if (CurrentUserSession != null)
                    {
                        if (comment.Username == CurrentUserSession.Username)
                        {
                            spanAddNewComment.Visible = false;
                            canDelete = true;
                        }
                        if (ad.PostedBy == CurrentUserSession.Username)
                        {
                            canDelete = true;
                        }
                    }

                    dtComments.Rows.Add(new object[]
                                            {
                                                comment.ID, comment.Date, comment.Username,
                                                Server.HtmlEncode(comment.CommentText), canDelete
                                            });
                }

                rptComments.DataSource = dtComments;
                rptComments.DataBind();

                if (CurrentUserSession == null)
                {
                    pnlUserComments.Visible = dtComments.Rows.Count > 0;
                }
            }
            else
            {
                pnlUserComments.Visible = false;
            }
        }

        private void showHideComments()
        {
            if (!CanAddComments())
                spanAddNewComment.Visible = false;
            else
                spanAddNewComment.Visible = true;
        }

        private bool CanAddComments()
        {
            if (ViewState["CanAddComments"] == null)
            {
                ViewState["CanAddComments"] =
                    Config.Ads.EnableAdComments &&
                    (CurrentUserSession.CanAddComments() == PermissionCheckResult.Yes ||
                        (CurrentUserSession.Level != null &&
                        CurrentUserSession.Level.Restrictions.UserCanAddComments)
                     );
            }

            return (bool)ViewState["CanAddComments"];
        }

        protected void lnkPhoto_Click(object sender, EventArgs e)
        {
            int photoID = getNextPhotoID(PhotosIDs);

            Classes.AdPhoto photo = Classes.AdPhoto.Fetch(photoID);

            if (photo != null)
            {
                ltrPhoto.Text = AdPhoto.RenderImageTag(photo.ID, 450, 450, "photoframe", true);
                lblAdPhotoDescription.Text = Server.HtmlEncode(photo.Description);
            }
        }

        private int getNextPhotoID(int[] photosIDs)
        {
            if (photosIDs.Length == 0) return -1;
            int currentPhotoIndex = Array.FindIndex(photosIDs, p => p == CurrentPhotoId);
            int id = currentPhotoIndex == photosIDs.Length - 1 ? photosIDs[0] : photosIDs[currentPhotoIndex + 1];
            CurrentPhotoId = id;
            return id;
        }

        protected void dlPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "ShowPhoto")
            {
                int id = Convert.ToInt32(e.CommandArgument);
                Classes.AdPhoto photo = Classes.AdPhoto.Fetch(id);

                CurrentPhotoId = id;

                string imageTag = AdPhoto.RenderImageTag(photo.ID, 450, 450, "photoframe", true);

                // HACK: On image load call the "SetHeight()" javascript function to fix the layout
                imageTag = imageTag.Replace("/>", " onload=\"SetHeight();\" />");

                ltrPhoto.Text = imageTag;

                lblAdPhotoDescription.Text = Server.HtmlEncode(photo.Description);
            }
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            Ad.Delete(Int32.Parse(Request.Params["id"]));
            Response.Redirect("~/Ads.aspx?show=ma");
        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {

        }

        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                AdComment comment = AdComment.Fetch(commentId);

                if (comment != null)
                {
                    if (CurrentUserSession != null)
                    {
                        AdComment.Delete(commentId);
                    }

                    loadComments = true;
                }
            }
        }

        protected void lnkViewAllComments_Click(object sender, EventArgs e)
        {
            divViewAllComments.Visible = false;
            ViewState["ViewPhotos_ViewAllComments"] = true;

            loadComments = true;
        }

        protected void btnSubmitNewComment_Click(object sender, EventArgs e)
        {
            if (txtNewComment.Text.Trim() == String.Empty)
            {
                return;
            }

            if (CurrentUserSession != null)
            {
                var comment = new AdComment(AdID, CurrentUserSession.Username)
                {
                    CommentText = (Config.Misc.EnableBadWordsFilterComments
                                   ? Parsers.ProcessBadWords(txtNewComment.Text)
                                   : txtNewComment.Text)
                };

                comment.Save();
            }

            loadComments = true;
        }

        protected void fbSearch_Click(object sender, EventArgs e)
        {
            Response.Redirect(String.Format("~/Ads.aspx?keyword={0}", txtKeyword.Text.Trim()));
            return;
        }
    }
}
