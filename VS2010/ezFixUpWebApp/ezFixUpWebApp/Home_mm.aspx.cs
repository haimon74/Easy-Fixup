/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using ezFixUp.Classes;
using ezFixUp.Components.WebParts;
using WebPartZone = ezFixUp.Classes.WebPartZone;

namespace ezFixUp
{
    public partial class Home_mm : PageBase
    {
        private HttpContext _itsContext;
        private TimeMeasure timestamps = new TimeMeasure();

        private WebPartZone ZoneToAddPartsTo
        {
            get { return (WebPartZone)(ViewState["ZoneToAddPartsTo"] ?? WebPartZone.HomePageRightZone); }

            set { ViewState["ZoneToAddPartsTo"] = value; }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            var toSetState = Request.QueryString["set"] == "true";
            if (toSetState)
                MatchmakerHelper.SetMatchmakerState(true);
            else if (CurrentUserSession != null && !MatchmakerHelper.IsMatchmakerState)
                Server.Transfer("home.aspx");

            base.Page_Init(sender, e);
        }
        //protected override void OnInitComplete(EventArgs e)
        //{
        //    base.OnInitComplete(e);
        //}
        //protected override void OnPreLoad(EventArgs e)
        //{
        //    base.OnPreLoad(e);
        //}
        protected void Page_Load(object sender, EventArgs e)
        {
            _itsContext = HttpContext.Current;
            WebPartManager2.SelectedWebPartChanged += WebPartManager2SelectedWebPartChanged;
            WebPartManager2.DisplayModeChanged += WebPartManager2DisplayModeChanged;

            this.AsyncPageBeginHandlersList = new List<BeginEventHandler> { AsyncWorkBeginMethod };
            this.AsyncPageEndHandlersList = new List<EndEventHandler> { AsyncWorkEndMethod };

            for (int idx = 0; idx < AsyncPageBeginHandlersList.Count; idx++)
            {
                var asyncTask = new PageAsyncTask(AsyncPageBeginHandlersList[idx],
                                                  AsyncPageEndHandlersList[idx],
                                                  null, new HomePageAsyncState(this), true);
                RegisterAsyncTask(asyncTask);
            }
            base.Page_Load(sender, e);
        }

        protected override void OnPreRenderComplete(EventArgs e)
        {
            base.OnPreRenderComplete(e);

            if (WebPartManager2.WebParts.Count == 0)
            { 
                Response.Write("<script type='text/javascript'>window.location.reload();</script>");
                Response.Flush();
                Response.End();
            }

            var dic = (Dictionary<string, ezFixUpBackgroundWorker>)Global.AppBackgroundWorkersDic;
            if (Session != null && CurrentUserSession.FacebookID.HasValue)
            {
                if (dic != null)
                {
                    var facebook = CurrentUserSession.GetFacebookApp();
                    var bgWorker1 =
                        (AddNewFacebookFriendsBackgroundWorker)
                           dic["ezFixUp.Classes.AddNewFacebookFriendsBackgroundWorker"];

                    if (bgWorker1 != null && facebook != null && CurrentUserSession.FacebookID.HasValue)
                    {
                        facebook.Session.UserId = CurrentUserSession.FacebookID.Value;
                        lock (bgWorker1.FacebookAppList)
                        {
                            if (!bgWorker1.FacebookAppList.Contains(facebook))
                                bgWorker1.FacebookAppList.Add(facebook);
                        }
                    }
                }
            }
            //if (IsPostBack)
            //    Response.End();
        }
        protected override IAsyncResult AsyncWorkBeginMethod(object sender, EventArgs e, AsyncCallback cb, object state)
        {
            var result = ((HomePageAsyncState)state).ItsDelegate.BeginInvoke(cb, state);
            timestamps.AddTimestamp("Home.AsyncWorkBeginMethod");
            return result;
        }
        protected override void AsyncWorkEndMethod(IAsyncResult asyncResult)
        {
            var asyncState = (HomePageAsyncState)asyncResult.AsyncState;

            timestamps.AddTimestamp("Home.AsyncWorkEndMethod");

            //var transDic = asyncState.StringsToTranslateDic;

            //btnCancelCatalog.Text = transDic["Cancel"];
            //ezEditorZoneLeft.HeaderText = transDic["Edit"];
            //ezEditorZoneRight.HeaderText = transDic["Edit"];

            //if (Config.Misc.LockHomePageLayout)
            //{
            //    lnkCatalogForLeftParts.Visible = false;
            //    lnkCatalogForRightParts.Visible = false;
            //}
            //else
            //{
            //    lnkCatalogForLeftParts.Visible = true;
            //    lnkCatalogForRightParts.Visible = true;
            //    lnkCatalogForLeftParts.Text = transDic["Add Component"];
            //    lnkCatalogForRightParts.Text = transDic["Add Component"];
            //}

            //ezEditorZoneRight.OKVerb.Text = ezEditorZoneLeft.OKVerb.Text = Lang.Trans(ezEditorZoneLeft.OKVerb.Text);
            //ezEditorZoneRight.CancelVerb.Text =
            //    ezEditorZoneLeft.CancelVerb.Text = Lang.Trans(ezEditorZoneLeft.CancelVerb.Text);
            //ezEditorZoneRight.ApplyVerb.Text =
            //    ezEditorZoneLeft.ApplyVerb.Text = Lang.Trans(ezEditorZoneLeft.ApplyVerb.Text);
            //ezEditorZoneRight.InstructionText =
            //    ezEditorZoneLeft.InstructionText = Lang.Trans(ezEditorZoneLeft.InstructionText);
        }

        public class HomePageAsyncState //: ezFixUpAsyncResult
        {
            public AsyncTaskDelegate ItsDelegate;
            public delegate void AsyncTaskDelegate();

            public Home_mm ItsHomePageContext;

            public HomePageAsyncState(Home_mm itsHomePageContext)
            {
                ItsDelegate = new AsyncTaskDelegate(ExecuteAsyncTask);
                ItsHomePageContext = itsHomePageContext;
            }
            //public LoadStringsAsyncState()//(AsyncCallback cb, object state) 
            //    //: base(cb, state)
            //{
            //}
            protected void ExecuteAsyncTask()
            {
                if (HttpContext.Current == null) HttpContext.Current = ItsHomePageContext._itsContext;

                if (!ItsHomePageContext.IsPostBack)
                {
                    ItsHomePageContext.loadStrings();
                    ItsHomePageContext.preparePage();
                    ItsHomePageContext.WebPartManager2.ReOpenClosedWebParts();
                }
            }
        }
        private void WebPartManager2DisplayModeChanged(object sender, WebPartDisplayModeEventArgs e)
        {
            if (WebPartManager2.DisplayMode == WebPartManager.EditDisplayMode)
            {
                lnkCatalogForLeftParts.Visible = false;
                lnkCatalogForRightParts.Visible = false;
                pnlCatalog.Visible = false;
            }
            else
            {
                lnkCatalogForLeftParts.Visible = true;
                lnkCatalogForRightParts.Visible = true;
            }
        }

        private void WebPartManager2SelectedWebPartChanged(object sender, WebPartEventArgs e)
        {
            if (e.WebPart != null && WebPartManager2.DisplayMode == WebPartManager.EditDisplayMode)
            {
                if (e.WebPart.Zone == wpzHomePageLeftZone)
                {
                    pnlEditorZoneLeft.Visible = true;
                    pnlEditorZoneRight.Visible = false;
                }
                else if (e.WebPart.Zone == wpzHomePageRightZone)
                {
                    pnlEditorZoneLeft.Visible = false;
                    pnlEditorZoneRight.Visible = true;
                }
            }
        }

        public void loadStrings()
        {
            btnCancelCatalog.Text = Lang.Trans("Cancel");
            ezEditorZoneLeft.HeaderText = Lang.Trans("Edit");
            ezEditorZoneRight.HeaderText = Lang.Trans("Edit");

            if (Config.Misc.LockHomePageLayout)
            {
                lnkCatalogForLeftParts.Visible = false;
                lnkCatalogForRightParts.Visible = false;
            }
            else
            {
                lnkCatalogForLeftParts.Visible = true;
                lnkCatalogForRightParts.Visible = true;
                lnkCatalogForLeftParts.Text = Lang.Trans("Add Component");
                lnkCatalogForRightParts.Text = Lang.Trans("Add Component");
            }

            ezEditorZoneRight.OKVerb.Text = ezEditorZoneLeft.OKVerb.Text = Lang.Trans(ezEditorZoneLeft.OKVerb.Text);
            ezEditorZoneRight.CancelVerb.Text =
                ezEditorZoneLeft.CancelVerb.Text = Lang.Trans(ezEditorZoneLeft.CancelVerb.Text);
            ezEditorZoneRight.ApplyVerb.Text =
                ezEditorZoneLeft.ApplyVerb.Text = Lang.Trans(ezEditorZoneLeft.ApplyVerb.Text);
            ezEditorZoneRight.InstructionText =
                ezEditorZoneLeft.InstructionText = Lang.Trans(ezEditorZoneLeft.InstructionText);
        }

        public void preparePage()
        {
            if (HttpContext.Current == null) HttpContext.Current = _itsContext;
            #region Add Initial Web Parts

            bool notFirstTime = CurrentUserSession.PersonalizationInfo != null &&
                             ContainsKeyStartWith("WebPartManager2_~/Home_mm.aspx_");

            bool isDirty = !notFirstTime;

            if (isDirty)
            {
                WebPartInfo[] infos1 = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageRightZone, true);

                for (int i = 0; i < infos1.Length; ++i)
                    WebPartManager2.AddWebPartUserControl(infos1[i], wpzHomePageRightZone, i);

                WebPartInfo[] infos2 = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageLeftZone, true);

                for (int i = 0; i < infos2.Length; ++i)
                    WebPartManager2.AddWebPartUserControl(infos2[i], wpzHomePageLeftZone, i);
            }

            if (false)//!Config.Groups.EnableGroups)
            {
                foreach (WebPart part in WebPartManager2.WebParts)
                {
                    if (part.Controls[0] is NewGroupsWebPart)
                    {
                        WebPartManager2.DeleteWebPart(part);
                        isDirty = true;
                    }
                }
            }

            if (isDirty)
            {
                WebPartManager2.SetDirty();
                isDirty = false;
            }
            #endregion
        }
        private bool ContainsKeyStartWith(string personalizedPageName)
        {

            return CurrentUserSession.PersonalizationInfo.userPersonalizationData.Keys

                .Any(key => key.StartsWith(personalizedPageName));
        }

        protected void lnkCatalogForLeftParts_Click(object sender, EventArgs e)
        {
            BindLeftCatalog();
            pnlCatalog.Visible = true;
            lnkCatalogForLeftParts.Visible = false;
            lnkCatalogForRightParts.Visible = false;
        }

        protected void lnkCatalogForRightParts_Click(object sender, EventArgs e)
        {
            BindRightCatalog();
            pnlCatalog.Visible = true;
            lnkCatalogForLeftParts.Visible = false;
            lnkCatalogForRightParts.Visible = false;
        }

        private void BindRightCatalog()
        {
            IList<string> lstTitles = (from WebPart wp in wpzHomePageRightZone.WebParts select wp.Title.ToLower()).ToList();
            WebPartInfo[] infos = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageRightZone)
                .ToList().Where(x => !lstTitles.Contains(x.Name.ToLower())
                                  && !lstTitles.Contains(x.Name.Translate().ToLower())).ToArray();

            rptCatalogWebParts.DataSource = infos;
            rptCatalogWebParts.DataBind();

            ZoneToAddPartsTo = WebPartZone.HomePageRightZone;
        }

        private void BindLeftCatalog()
        {
            IList<string> lstTitles = (from WebPart wp in wpzHomePageLeftZone.WebParts select wp.Title.ToLower()).ToList();
            WebPartInfo[] infos = Config.WebParts.GetAvailableWebParts(WebPartZone.HomePageLeftZone)
                .ToList().Where(x => !lstTitles.Contains(x.Name.ToLower())
                                  && !lstTitles.Contains(x.Name.Translate().ToLower())).ToArray();

            rptCatalogWebParts.DataSource = infos;
            rptCatalogWebParts.DataBind();

            ZoneToAddPartsTo = WebPartZone.HomePageLeftZone;
        }

        protected void rptCatalogWebParts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Add")
            {
                WebPartInfo info = Array.Find(Config.WebParts.CurrentContextAllParts,
                    i => i.ControlPath == (string)e.CommandArgument);
                if (info != null)
                {
                    if (ZoneToAddPartsTo == WebPartZone.HomePageRightZone)
                        WebPartManager2.AddWebPartUserControl(info, wpzHomePageRightZone,
                                                              wpzHomePageRightZone.WebParts.Count);
                    else if (ZoneToAddPartsTo == WebPartZone.HomePageLeftZone)
                        WebPartManager2.AddWebPartUserControl(info, wpzHomePageLeftZone,
                                                              wpzHomePageLeftZone.WebParts.Count);

                    pnlCatalog.Visible = false;
                    lnkCatalogForLeftParts.Visible = true;
                    lnkCatalogForRightParts.Visible = true;
                    WebPartManager2.SetDirty();
                    Page.SaveStateComplete += new EventHandler(Page_Reload_Now);
                }
            }
            else if (e.CommandName == "Delete")
            {
                Page.SaveStateComplete += new EventHandler(Page_Reload_Now);
            }
        }
        protected void Page_Reload_Now(object sender, EventArgs e)
        {
            Page.Response.Redirect(Page.Request.Url.ToString(), false);
        }

        protected void btnCancelCatalog_Click(object sender, EventArgs e)
        {
            pnlCatalog.Visible = false;
            lnkCatalogForLeftParts.Visible = true;
            lnkCatalogForRightParts.Visible = true;
        }

        protected void imgbCloseCatalog_Click(object sender, ImageClickEventArgs e)
        {
            pnlCatalog.Visible = false;
            lnkCatalogForLeftParts.Visible = true;
            lnkCatalogForRightParts.Visible = true;
        }
    }
}