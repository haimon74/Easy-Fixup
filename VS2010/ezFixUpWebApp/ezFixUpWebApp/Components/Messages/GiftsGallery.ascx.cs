using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Messages
{
    public partial class GiftsGallery : UserControl, ICallbackEventHandler
    {
        public int GalleryLayoutWidth = 740 * 67 / 100;
        public int GalleryLayoutHeight;
        public int GridColumnCount = Config.Gift.GridColumnCount;
        public int GridRowCount = Config.Gift.GridRowCount;
        public int ListPhotoWidthPx { get { return GalleryLayoutWidth * 23 / 100; } }
        public int GridBorderPx = 3;
        public int CategoryId { get; set; }
        public GiftType[] GiftsForCategory
        {
            get { return GiftTypes.Where(g => g.CategoryId == CategoryId).Select(x=>x).ToArray(); }
        }
        public GiftType[] GiftTypes
        {
            get
            {
                if (Cache["ActiveGiftTypes"] == null)
                {
                    var gifts = GiftType.Fetch(true, null, GiftType.eSortColumn.None);
                    Cache.Add("ActiveGiftTypes", gifts,
                        null, Cache.NoAbsoluteExpiration, TimeSpan.FromMinutes(5),
                        CacheItemPriority.NotRemovable, null
                        );
                }
                return Cache["ActiveGiftTypes"] as GiftType[];
            }
        }
        public List<int> SelectedGiftTypes
        {
            set { Global.GetSessionState()["SelectedGiftsIds"] = value; }
            get
            {
                var retVal = new List<int>();
                if (Global.GetSessionState().ContainsKey("SelectedGiftsIds"))
                    retVal = (List<int>)Global.GetSessionState()["SelectedGiftsIds"];
                return retVal; 
            }
        }
        public bool PanelVisible
        {
            set
            {
                lblSelectedGiftsIds.Text = String.Empty;
                foreach (int gid in SelectedGiftTypes)
                {
                    lblSelectedGiftsIds.Text += gid + ",";
                }
                var displayStr2 = value ? "block" : "none";
                var updateDisplay2 = this.galleryView.Style["display"] != displayStr2;
                this.galleryView.Style["display"] = displayStr2;
                if (updateDisplay2) UpdatePanel2.Update();

                var displayStr3 = (value || lblSelectedGiftsIds.Text.IsNotNullOrEmpty()) ? "block" : "none";
                var updateDisplay3 = this.pnlGiftsGallerySelectId.Style["display"] != displayStr3;
                pnlGiftsGallerySelectId.Style["display"] = displayStr3;
                if (updateDisplay3) UpdatePanel3.Update();

                this.btnComplete.Style["display"] = value ? "inline" : "none";
                UpdatePanel4.Update();
            }
        }
        public GiftType[] DisplayResults
        {
            get
            {
                int viewableCount = GridRowCount * GridColumnCount;
                int toSkipCount = (CurrentPage - 1)*viewableCount;
                return GiftTypes.Where(x => x.CategoryId == CategoryId).Skip(toSkipCount).Take(viewableCount).ToArray();
            }
        }
        public int CurrentPage
        {
            set
            {
                Trace.Write("GiftsGallery.ascx.cs", "CurrentPage = " + value);
                ViewState["CurrentPage"] = value;
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (_updateHistory && PaginatorEnabled
                    && scriptManager != null && scriptManager.IsInAsyncPostBack)
                {
                    scriptManager.AddHistoryPoint("page", value.ToString());
                }
                PreparePaginator();
            }
            get
            {
                if (ViewState["CurrentPage"] == null
                    || (int)ViewState["CurrentPage"] == 0)
                {
                    return 1;
                }
                return (int)ViewState["CurrentPage"];
            }
        }

        public bool PaginatorEnabled
        {
            get { return _paginatorVisible; }
            set
            {
                _paginatorVisible = value;
                pnlPaginator.Visible = value;
            }
        }

        public bool UseCache
        {
            set { _useCache = value; }
        }

        public bool EnableGridSupport { get; set; }

        public bool GridMode
        {
            set { ViewState["GiftsGallery_GridMode"] = value; }
            get
            {
                if (ViewState["GiftsGallery_GridMode"] is bool)
                    return (bool)ViewState["GiftsGallery_GridMode"];
                return false;
            }
        }

        
        private bool _useCache;
        
        private bool _updateHistory = true;
        private bool _paginatorVisible = true;

        public string CallBackFunctionInvocationStr;

        protected void Page_Load(object sender, EventArgs e)
        {
            Trace.Write("GiftsGallery.ascx.cs", "Page_Load");
            CallBackFunctionInvocationStr = Page.ClientScript.GetCallbackEventReference(this,
                "message", "processCallbackResult", "context", "processCallbackError", true);
            dlGiftsGallery.RepeatColumns = GridColumnCount;
            GalleryLayoutHeight = GridRowCount * (3 * GridBorderPx +  GalleryLayoutWidth + 30);
            dlGiftsGallery.Width = new Unit((GalleryLayoutWidth - GridBorderPx * (3 * GridColumnCount + 1)) / GridColumnCount);
            //if (!Page.IsPostBack)
            {
                LoadStrings();
                LoadGiftCategories();
            }
            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
        }

        private void SetPaginatorButtonsCSS()
        {
            if (Global.GetSessionState()["Direction"] != null && Global.GetSessionState()["Direction"].Equals("rtl"))
            {
                lnkFirst.CssClass = "lastPageBtn";
                lnkPrev.CssClass = "nextPageBtn";
                lnkNext.CssClass = "prevPageBtn";
                lnkLast.CssClass = "firstPageBtn";
            }
            else
            {
                lnkFirst.CssClass = "firstPageBtn";
                lnkPrev.CssClass = "prevPageBtn";
                lnkNext.CssClass = "nextPageBtn";
                lnkLast.CssClass = "lastPageBtn";
            }
            lnkFirst.Visible = lnkFirst.Enabled;
            lnkPrev.Visible = lnkPrev.Enabled;
            lnkNext.Visible = lnkNext.Enabled;
            lnkLast.Visible = lnkLast.Enabled;
        }

        protected override void OnPreRender(EventArgs e)
        {
            LoadResultsPage();

            if (PaginatorEnabled)
            {
                PreparePaginator();
            }
            base.OnPreRender(e);
        }
        private void LoadGiftCategories()
        {
            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("Title");
            dtCategories.Columns.Add("ID");
            dtCategories.Rows.Clear();
            GiftCategory[] categories = GiftCategory.FetchCategories(EcardCategory.eSortColumn.Title);
            if (categories.Length > 0)
            {
                plhGiftsCategories.Controls.Clear();
                var htmlTable = new HtmlTable();
                //var buttons = categories.Select(giftCategory => new Button
                //                                                    {
                //                                                        ID = "btnGiftCat_" + giftCategory.ID,
                //                                                        Text = giftCategory.Title,
                //                                                        ForeColor = Color.Purple,
                //                                                        BackColor = Color.SkyBlue,
                //                                                        BorderStyle = BorderStyle.Solid,
                //                                                        BorderWidth = Unit.Pixel(2),
                //                                                        ToolTip = giftCategory.ID.ToString()
                //                                                    });
                htmlTable.Width = "100%";
                htmlTable.CellSpacing = 1;
                htmlTable.CellPadding = 2;
                htmlTable.Border = 1;
                htmlTable.BorderColor= "transparent";
                plhGiftsCategories.Controls.Add(htmlTable);
                foreach (var giftCategory in categories)
                {
                    //btn.Click += btnGiftCat_Click;
                    var lb = (AsyncLinkButton)LoadControl("~/Components/Messages/AsyncLinkButton.ascx");
                    lb.Title = giftCategory.Title;
                    lb.CategoryId = giftCategory.ID;
                    lb.LinkButtonClickCallback = SetCategoryIdCallback;
                    var cell = new HtmlTableCell();
                    cell.BgColor = "DarkTurquoise";
                    cell.Attributes["class"] = "giftCategory";
                    cell.Controls.Add(lb);
                    var row = new HtmlTableRow();
                    row.Cells.Add(cell);
                    htmlTable.Rows.Add(row);
                    var trigger = new AsyncPostBackTrigger { ControlID = lb.GetLinkButton().UniqueID, EventName = "Click" };
                    UpdatePanel1.Triggers.Add(trigger);
                    if (Page.Master != null) ((Site)Page.Master).ScriptManager.RegisterAsyncPostBackControl(lb);
                }
                CategoryId = categories.First().ID;
            }
        }
        private void LoadStrings()
        {
            btnComplete.Text = Lang.Trans("Close");
            btnComplete.ToolTip = Lang.Trans("Close gifts gallery");
            btnCancel.Text = Lang.Trans("Cancel");
            lnkFirst.ToolTip = /*@"◄◄  " +*/ Lang.Trans("First");
            lnkPrev.ToolTip = /*@"◄  " +*/ Lang.Trans("Prev");
            lnkNext.ToolTip = Lang.Trans("Next") /*+ @"  ►"*/;
            lnkLast.ToolTip = Lang.Trans("Last") /*+ @"  ►►"*/;
        }
        
        private void PreparePaginator()
        {
            int giftsPerPage = dlGiftsGallery.Visible ? Config.Gift.GiftsPerPage : 1;
            bool toEnable = (DisplayResults != null && CurrentPage > 1);
            lnkFirst.Enabled = toEnable;
            lnkPrev.Enabled = toEnable;
            toEnable = (DisplayResults != null && CurrentPage < (GiftsForCategory.Length / giftsPerPage));
            lnkLast.Enabled = toEnable;
            lnkNext.Enabled = toEnable;

            SetPaginatorButtonsCSS();

            if (DisplayResults != null && DisplayResults.Length > 0)
            {
                int fromCount = (CurrentPage - 1) * giftsPerPage + 1;
                int toCount = CurrentPage * giftsPerPage;
                if (GiftsForCategory.Length < toCount)
                {
                    toCount = GiftsForCategory.Length;
                }

                //lblPager.Text = String.Format(
                //    Lang.Trans("Showing {0}-{1} from {2} total"),
                //    fromCount, toCount, GiftsForCategory.Length);
                lblPager.Text = Lang.Trans("Page") + String.Format(" {0} / {1} ", CurrentPage, GetMaxPages());
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }
        private void LoadResultsPage()
        {
            var dtResults = new DataTable("GiftsGalleryResults");

            dtResults.Columns.Add("GiftId", typeof(int));
            dtResults.Columns.Add("GiftCategoryId", typeof(int));
            dtResults.Columns.Add("GiftUrl");
            dtResults.Columns.Add("Price");
            dtResults.Columns.Add("Name");
            dtResults.Columns.Add("Phrase");

            if (dlGiftsGallery.Visible && GiftsForCategory != null)
            {
                Trace.Write("Loading page " + CurrentPage);
                GiftType[] gifts = (PaginatorEnabled) ? DisplayResults : GiftsForCategory;
                const int MULTIPLY_BY = 1;
                for (int i = 0; i < gifts.Length * MULTIPLY_BY; i++)
                {
                    GiftType gift = gifts[i % gifts.Length];
                    dtResults.Rows.Add(new object[]{gift.ID, gift.CategoryId, 
                        GiftImageHandler.CreateImageUrl(gift.ID,80,80),gift.Price, 
                        gift.Name.Translate(), gift.Phrase.Translate()});
                }
                dlGiftsGallery.DataSource = dtResults;
                dlGiftsGallery.DataBind();
            }
        }
        public void SetCategoryIdCallback(int categoryId)
        {
            if (CategoryId != categoryId)
            {
                CategoryId = categoryId;
                CurrentPage = 1;
                LoadResultsPage();
            }
        }

        private void btnGiftCat_Click(object sender, EventArgs e)
        {
            int catId;
            if (sender is Button && int.TryParse((sender as Button).ToolTip, out catId) && CategoryId != catId)
            {
                CategoryId = catId;
                CurrentPage = 1;
                LoadResultsPage();
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage = 1;
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkNext_Click(object sender, EventArgs e)
        {
            if (GiftsForCategory == null)
                return;
            else
            {
                if (CurrentPage < GetMaxPages())
                {
                    CurrentPage++;
                }
            }
        }
        private int GetMaxPages()
        {
            int div = GiftsForCategory.Length/Config.Gift.GiftsPerPage;
            int mod = GiftsForCategory.Length%Config.Gift.GiftsPerPage;
            return (mod == 0) ? div : div + 1;
        }
        /// <summary>
        /// Handles the Click event of the lnkLast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lnkLast_Click(object sender, EventArgs e)
        {
            if (GiftsForCategory == null)
                return;
            else
            {
                if (CurrentPage < GetMaxPages())
                {
                    CurrentPage = GetMaxPages();
                }
            }
        }

        void scriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            if (DisplayResults == null)
                return;

            int navigatePage;
            try
            {
                navigatePage = e.State.Count == 0 ? 1 : Convert.ToInt32(e.State[0]);
            }
            catch (FormatException)
            {
                navigatePage = 1;
            }
            int usersPerPage = dlGiftsGallery.Visible ? Config.Gift.GiftsPerPage: 0;
            if (navigatePage <= DisplayResults.Length && navigatePage > 0)
            {
                _updateHistory = false;
                CurrentPage = navigatePage;
            }
        }

        protected void btnComplete_Click(object sender, EventArgs e)
        {
            PanelVisible = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            SelectedGiftTypes = new List<int>();
            this.PanelVisible = false;
        }

        protected void LinkButtonCategoryClick(object sender, EventArgs e)
        {
            LinkButton lb = sender as LinkButton;
            if (lb == null) return;
            SetSelectLinkButtonCss(lb);
            CategoryId = int.Parse(lb.AccessKey);
            LoadResultsPage();
        }
        private void SetSelectLinkButtonCss(LinkButton selectedLinkButton)
        {
            var controls = pnlGiftsCategoriesMenu.Controls.OfType<LinkButton>();
            foreach (LinkButton lb in controls)
            {
                lb.BackColor = (lb == selectedLinkButton) ? Color.DeepPink : Color.SkyBlue;
                lb.ForeColor = (lb == selectedLinkButton) ? Color.White : Color.DarkGray;
            }
        }

        protected void DataListGiftsCategoriesItemCreated(object sender, DataListItemEventArgs e)
        {
            LinkButton lbCategory = (LinkButton)e.Item.Controls.OfType<LinkButton>().FirstOrDefault();
            if (lbCategory != null)
            {
                AsyncPostBackTrigger trigger = new AsyncPostBackTrigger {ControlID = lbCategory.ID, EventName = "Click"};
                UpdatePanel1.Triggers.Add(trigger);
            }
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
        #region Implementation of ICallbackEventHandler

        public void RaiseCallbackEvent(string eventArgument)
        {
            SelectedGiftTypes = eventArgument
                .Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                .Select(g => int.Parse(g)).ToList();
        }

        public string GetCallbackResult()
        {
            return lblSelectedGiftsIds.Text;
        }

        #endregion
    }
}