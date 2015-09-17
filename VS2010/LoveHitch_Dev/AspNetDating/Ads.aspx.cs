using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class Ads : PageBase
    {
        int? CategoryID
        {
            get { return (int?) ViewState["CategoryID"]; }
            set { ViewState["CategoryID"] = value; }
        }

        private string Keyword
        {
            get { return ViewState["Keyword"] as string; }
            set { ViewState["Keyword"] = value; }
        }

        private eAdType AdType
        {
            get
            {
                if (ViewState["AdType"] == null)
                {
                    return eAdType.None;
                }

                return (eAdType)ViewState["AdType"];
            }
            set
            {
                ViewState["AdType"] = value;
            }
        }

        private enum eAdType
        {
            None,
            ByCategory,
            MyAds,
            ByKeyword,
            ByUser
        }

        public AdSearchResults Results
        {
            set
            {
                ViewState["SearchResults"] = value;
                CurrentPage = 1;
            }
            get
            {
                return (AdSearchResults)
                       ViewState["SearchResults"];
            }
        }

        public int CurrentPage
        {
            set
            {
                ViewState["CurrentPage"] = value;
                preparePaginator();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                    return 1;
                else
                    return (int)ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            set { pnlPaginator.Visible = value; }
        }

        private void preparePaginator()
        {
            if (Results == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (Results == null || CurrentPage >= Results.GetTotalPages(Config.Ads.AdsPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Ads.Length > 0)
            {
                int fromAd = (CurrentPage - 1) * Config.Ads.AdsPerPage + 1;
                int toAd = CurrentPage * Config.Ads.AdsPerPage;
                if (Results.Ads.Length < toAd)
                    toAd = Results.Ads.Length;

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromAd, toAd, Results.Ads.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        public Ads()
        {
            RequiresAuthorization = Config.Ads.OnlyRegisteredUsersCanBrowseClassifieds;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Ads.Enable)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                if (CurrentUserSession != null)
                {
                    var permissionCheckResult = CurrentUserSession.CanBrowseClassifieds();

                    if (permissionCheckResult == PermissionCheckResult.Yes ||
                        (CurrentUserSession.Level != null
                                && CurrentUserSession.Level.Restrictions.UserCanBrowseClassifieds))
                    {
                    }
                    else if (permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                            permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
                    {
                        Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.UserCanBrowseClassifieds;
                        Response.Redirect("~/ManageProfile.aspx?sel=payment");
                        return;
                    }
                    else if (permissionCheckResult == PermissionCheckResult.No)
                    {
                        StatusPageMessage = Lang.Trans("You are not allowed to browse classifieds!");
                        Response.Redirect("ShowStatus.aspx");
                        return;
                    }

                }

                //if (CurrentUserSession != null
                //        && !(CurrentUserSession.BillingPlanOptions.UserCanBrowseClassifieds 
                //            || (CurrentUserSession.Level != null 
                //                && CurrentUserSession.Level.Restrictions.UserCanBrowseClassifieds)))
                //{
                //    StatusPageMessage = Lang.Trans("You are not allowed to browse classifieds!");
                //    Response.Redirect("ShowStatus.aspx");
                //    return;
                //}

                loadStrings();

                int categoryID;
                if (Request.Params["show"] != null && Request.Params["show"] == "ma" && CurrentUserSession != null)
                {
                    AdType = eAdType.MyAds;
                }
                if (!String.IsNullOrEmpty(Request.Params["keyword"]))
                {
                    AdType = eAdType.ByKeyword;
                    Keyword = Request.Params["keyword"];
                }
                else if (!String.IsNullOrEmpty(Request.Params["uid"]))
                {
                    AdType = eAdType.ByUser;
                }
                else if (String.IsNullOrEmpty(Request.Params["cid"]))
                {
                    mvCategories.SetActiveView(viewCategories);
                    loadCategories();
                }
                else if (Int32.TryParse(Request.Params["cid"], out categoryID))
                {
                    AdType = eAdType.ByCategory;
                    CategoryID = categoryID;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (AdType != eAdType.None) loadAds();

            setMenu();
        }

        private void setMenu()
        {
            pnlMyAds.Visible = CurrentUserSession != null;
            pnlPostAd.Visible = CurrentUserSession != null;
            SmallBoxStart1.Visible = pnlAllAds.Visible || pnlMyAds.Visible || pnlPostAd.Visible;
            SmallBoxEnd1.Visible = SmallBoxStart1.Visible;
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = "Classifieds".Translate();
            SmallBoxStart1.Title = "Actions".Translate();
            SmallBoxStart2.Title = "Search".Translate();
            fbSearch.Text = "Search".Translate();
            lnkPostAd.Text = "Add Classified".Translate();
            lnkAllAds.Text = "All Classifieds".Translate();
            lnkMyAds.Text = "My Classifieds".Translate();

            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");
        }

        private void loadCategories()
        {
            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("ID", typeof(int));
            dtCategories.Columns.Add("Title");
            
            AdsCategory[] categories = AdsCategory.FetchCategories(AdsCategory.eSortColumn.Title);

            if (categories.Length > 0)
            {
                foreach (var category in categories)
                {
                    dtCategories.Rows.Add(new object[] { category.ID, category.Title });
                }    
            }
            else
            {
                lblError.Text = "There are no categories!".Translate();
            }

            dlCategories.DataSource = dtCategories;
            dlCategories.DataBind();
            dlCategories.Visible = dtCategories.Rows.Count > 0;
        }

        private void loadAds()
        {
            mvCategories.SetActiveView(viewAds);
            pnlAllAds.Visible = true;

            DataTable dtAds = new DataTable();
            dtAds.Columns.Add("ID");
            dtAds.Columns.Add("PostedBy");
            dtAds.Columns.Add("Subject");
            dtAds.Columns.Add("AdPhotoID");
            dtAds.Columns.Add("Date");
            dtAds.Columns.Add("Pending");

            Ad[] ads = null;
            if (Results == null)
            {
                Results = new AdSearchResults();

                if (AdType == eAdType.ByCategory)
                    Results.Ads = Ad.Search(CategoryID, null, null, DateTime.Now, true, null, null,
                                                    Ad.eSortColumn.Date);
                else if (AdType == eAdType.MyAds)
                    Results.Ads = Ad.Search(null, CurrentUserSession.Username, null, DateTime.Now, null, null, null,
                                            Ad.eSortColumn.Date);
                else if (AdType == eAdType.ByUser)
                    Results.Ads = Ad.Search(null, Request.Params["uid"], null, DateTime.Now, true, null, null,
                                            Ad.eSortColumn.Date);
                else if (AdType == eAdType.ByKeyword)
                    Results.Ads = Ad.Search(CategoryID, null, null, DateTime.Now, true, Keyword, null, Ad.eSortColumn.Date);

                if (Results.Ads.Length == 0)
                {
                    if (AdType == eAdType.ByCategory)
                        lblError.Text = "There are no classifieds for this category!".Translate();
                    else if (AdType == eAdType.MyAds)
                        lblError.Text = "You don't have classifieds!".Translate();
                    else if (AdType == eAdType.ByKeyword)
                        lblError.Text = "There are no classifieds!".Translate();
                    PaginatorEnabled = false;
                    dlAds.Visible = false;
                    return;
                }

                PaginatorEnabled = true;

                CurrentPage = 1;
            }

            ads = Results.GetPage(CurrentPage, Config.Ads.AdsPerPage);
            if (ads != null && ads.Length > 0)
            {
                foreach (Ad ad in ads)
                {
                    Classes.AdPhoto[] photo = Classes.AdPhoto.FetchByAdID(ad.ID);

                    dtAds.Rows.Add(new object[]
                                   {
                                       ad.ID,
                                       ad.PostedBy,
                                       ad.Subject,
                                       photo.Length != 0 ? photo[0].ID : 0,
                                       ad.Date.ToShortDateString(),
                                       ad.Approved ? String.Empty : "(" + Lang.Trans("-- pending approval --") + ")"
                                   });
                }
            }
            else lblError.Text = "There are no classifieds for this category!".Translate();

            dlAds.DataSource = dtAds;
            dlAds.DataBind();
            dlAds.Visible = dtAds.Rows.Count > 0;
        }

        protected void dlCategories_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            DataListItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                rptSubcategories = (Repeater)item.FindControl("rptSubcategories");
                int categoryID = (int)DataBinder.Eval(e.Item.DataItem, "ID");
                DataTable dtSubcategories = new DataTable();
                dtSubcategories.Columns.Add("ID", typeof(int));
                dtSubcategories.Columns.Add("Title");

                AdsCategory[] subcategories = AdsCategory.FetchSubcategories(categoryID, AdsCategory.eSortColumn.Title);

                foreach (var subcategory in subcategories)
                {
                    dtSubcategories.Rows.Add(new object[] { subcategory.ID, subcategory.Title });
                }

                rptSubcategories.DataSource = dtSubcategories;
                rptSubcategories.DataBind();
                rptSubcategories.Visible = dtSubcategories.Rows.Count > 0;
            }
        }

        protected void lnkSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtKeyword.Text.Trim();

            if (keyword.Length == 0) return;

            Results = null;
            Keyword = keyword;
            AdType = eAdType.ByKeyword;
        }

        protected void lnkPostAd_Click(object sender, EventArgs e)
        {
            int activeAds = Ad.Search(null, CurrentUserSession.Username, null, DateTime.Now, null, null, null,
                                      Ad.eSortColumn.None).Length;


            int maxActiveAds = CurrentUserSession.BillingPlanOptions.MaxActiveAds.Value;
            if (CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.MaxActiveAds > maxActiveAds)
                maxActiveAds = CurrentUserSession.Level.Restrictions.MaxActiveAds;

            if (activeAds >= maxActiveAds)
            {
                StatusPageMessage = "You have reached the maximum number of active classifieds!".Translate();
                Response.Redirect("~/ShowStatus.aspx");
                return;
            }

            string param = CategoryID != null ? "?cid=" + CategoryID : String.Empty;
            Response.Redirect(String.Format("~/PostAd.aspx{0}", param));
        }

        protected void lnkAllClassifieds_Click(object sender, EventArgs e)
        {
            loadCategories();
            Results = null;
            CategoryID = null;
            pnlAllAds.Visible = false;
            enableMenuLinks();
//            lnkAllAds.Enabled = false;
            AdType = eAdType.None;
            mvCategories.SetActiveView(viewCategories);
        }

        protected void lnkMyClassifieds_Click(object sender, EventArgs e)
        {
            Results = null;
            enableMenuLinks();
            lnkMyAds.Enabled = false;
            AdType = eAdType.MyAds;
        }

        private void enableMenuLinks()
        {
            lnkAllAds.Enabled = true;
            lnkMyAds.Enabled = true;
            lnkPostAd.Enabled = true;
        }

        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage = 1;
        }

        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;
        }

        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Ads.AdsPerPage))
                CurrentPage++;
        }

        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (CurrentPage < Results.GetTotalPages(Config.Ads.AdsPerPage))
                CurrentPage = Results.GetTotalPages(Config.Ads.AdsPerPage);
        }
    }
}
