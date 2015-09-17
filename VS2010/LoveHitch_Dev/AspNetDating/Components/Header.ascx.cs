using System;
using System.Collections;
using System.Collections.Generic;
/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components
{
    public partial class Header : UserControl
    {
        #region Server Controls

        protected HtmlAnchor lnkAboutUs;
        protected HtmlAnchor lnkContactUs;
        protected HtmlAnchor lnkFavourites;
        protected HtmlAnchor lnkHome;
        protected HtmlAnchor lnkMail;
        protected HtmlAnchor lnkProfile;
        protected HtmlAnchor lnkSearch;
        protected HtmlAnchor lnkSubscription;
        protected HtmlAnchor lnkTopPhotos;
        protected HtmlAnchor lnkTopUsers;
        protected HtmlGenericControl tdAboutUs;
        protected HtmlGenericControl tdContactUs;

        #endregion

        public Dictionary<HtmlGenericControl, string> MenuItems = new Dictionary<HtmlGenericControl, string>();

        protected int OnlineUsersCount
        {
            //get{return ((IList<int>) Application["UsersOnline"]).Sum();}
            get { return Global.AppUsersOnline.Sum(); }
        }
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
            set { ((PageBase)Page).CurrentUserSession = value; }
        }
        /*
		protected void Page_Init(object sender, EventArgs e) {
			// Define an HtmlLink control.            
			HtmlLink myHtmlLink = new HtmlLink();
			myHtmlLink.Href = "Styles/Site.css";
			myHtmlLink.Attributes.Add("rel", "stylesheet");
			myHtmlLink.Attributes.Add("type", "text/css");
			// Add the HtmlLink to the Head section of the page.
			Page.Header.Controls.Add(myHtmlLink);
			myHtmlLink.Href = "~/scripts/apycom7royalblue/menu.css";
		}
        */
        protected void Page_Load(object sender, EventArgs e)
        {

            MenuItems.Add(liHomePage, ((PageBase)Page).CurrentUserSession == null ? "Default.aspx" : MatchmakerHelper.CurrentHomePage);
            MenuItems.Add(liFavorites, "FavoritesList.aspx");
            MenuItems.Add(liFriends, "FriendsList.aspx");
            MenuItems.Add(liSearch, "SearchList.aspx");
            MenuItems.Add(liProfile, "ManageProfile.aspx");
            MenuItems.Add(liMailbox, "Mailbox.aspx");
            MenuItems.Add(liMatchmaker, "Matchmaker.aspx");
            MenuItems.Add(liSubscription, "ManageProfile.aspx?sel=payment");
            MenuItems.Add(liInvite, "InviteFriends.aspx");
            var topMenuBgColorActive = (MatchmakerHelper.IsMatchmakerState) ? "#0163A2" : "dodgerblue";
            var topMenuBgColorNormal = (MatchmakerHelper.IsMatchmakerState) ? "dodgerblue" : "#0163A2";
            foreach (HtmlGenericControl li in MenuItems.Keys)
            {
                bool isActive = Request.RawUrl.ToLower().Contains(MenuItems[li].ToLower());
                li.Style["background"] = isActive ? topMenuBgColorActive : "transparent";
            }
            liMatchmaker.Style["background"] = String.Format("{0} !important",
                MatchmakerHelper.IsMatchmakerState ? "#00508D" : "#D0222C");
            var isLoggedIn = CurrentUserSession != null;
            beforeLoginView.Visible = !isLoggedIn;
            welcomeLogout.Visible = isLoggedIn;
            if (!IsPostBack)
            {
                PrepareStrings();
                //pnlNavi.Visible = !(Page is _default);
            }
            //imgLogo.Src = String.Format("~/App_Themes/{0}/{1}", MatchmakerHelper.SiteTheme, "logo_header.png");
            //if(Request.Browser.Browser == "IE" && Request.Browser.MajorVersion < 7 && imgLogo != null) {
            //    imgLogo.Attributes.Add("onload", "fixPng(this);");
            //}

            //LoadPages();
        }

        private void PrepareStrings()
        {
            pnlLanguage.Visible = true;
            if (Page is PageBase && CurrentUserSession != null)
            {
                string username = CurrentUserSession.Name.IsNotNullOrEmpty()
                    ? CurrentUserSession.Name
                    : CurrentUserSession.Username;
                lblWelcome.Text = String.Format(
                    Lang.Trans("Welcome <b>{0}</b>"),  username);
                lnkLogout.Text = Lang.Trans("Logout");
                string credits = CurrentUserSession.Credits == 1 ? Lang.Trans("credit") : Lang.Trans("credits");
                lblCredits.Text = "(" + CurrentUserSession.Credits + ") " + credits + "&nbsp;|&nbsp;";
                lblCredits.Visible = CurrentUserSession.BillingPlanOptions.ContainsOptionWithEnabledCredits;
                pnlLoginView.Visible = true;

                pnlLoginView.Style["direction"] = Language.Fetch(Convert.ToInt32(Context.Session["LanguageId"])).Direction;
                var permissionCheckResult = CurrentUserSession.CanUseChat();

                tdAjaxChat.Visible = Config.Misc.EnableAjaxChat
                                                         && (permissionCheckResult != PermissionCheckResult.No
                                                                 ||
                                                                 (CurrentUserSession.Level != null &&
                                                                    CurrentUserSession.Level.Restrictions.UserCanUseChat));

                if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                        permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    lnkAjaxChatPay.Visible = true;
                    lnkAjaxChat.Visible = false;
                    //lnkAjaxChat.HRef = "~/ManageProfile.aspx?sel=payment";
                    //lnkAjaxChat.Target = null;
                }
                else
                {
                    lnkAjaxChatPay.Visible = false;
                    lnkAjaxChat.Visible = true;
                    //lnkAjaxChat.HRef = "~/AjaxChat/ChatWindow.aspx";
                    //lnkAjaxChat.Target = "_ajaxchat";
                }

                tdContests.Visible = Config.Ratings.EnablePhotoContests;
                tdBroadcast.Visible = Config.Misc.EnableProfileVideoBroadcast;
                tdReviewNewPhotos.Visible = Config.CommunityModeratedSystem.EnableCommunityPhotoApproval &&
                                                                        (CurrentUserSession.Level == null ||
                                                                         CurrentUserSession.Level.Restrictions.AllowToModeratePhotos) &&
                                                                        CurrentUserSession.ModerationScores >=
                                                                        Config.CommunityModeratedSystem.MinimumScoresToAllowModeration;
                tdReviewNewProfiles.Visible = Config.CommunityFaceControlSystem.EnableCommunityFaceControl &&
                                                                            (CurrentUserSession.Level == null ||
                                                                             CurrentUserSession.Level.Restrictions.AllowToParticipateInFaceControl) &&
                                                                            CurrentUserSession.ModerationScores >=
                                                                            Config.CommunityFaceControlSystem.MinimumScoresToAllowModeration;
                liFavorites.Visible = Config.Users.EnableFavorites;
                liFriends.Visible = Config.Users.EnableFriends;

                if (Config.Misc.SiteIsPaid &&
                        !(Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female))
                {
                    if (liSubscription != null)
                        liSubscription.Visible = true;
                }
                else
                {
                    if (liSubscription != null)
                        liSubscription.Visible = false;
                }
            }
            else
            {
                liFavorites.Visible = false;
                liFriends.Visible = false;
                liMailbox.Visible = false;
                liProfile.Visible = false;
                tdBlog.Visible = false;
                tdRatePhotos.Visible = false;

                if (liSubscription != null)
                    liSubscription.Visible = false;

                //pnlLoginView.Visible = false;
            }

            if (Config.Users.DisableGenderInformation
                    || !Config.Ratings.EnablePhotoRatings
                    && !Config.Ratings.EnableProfileRatings
                    && (!Config.CommunityModeratedSystem.EnableTopModerators ||
                            !Config.CommunityModeratedSystem.EnableCommunityPhotoApproval))
            {
                tdTopCharts.Visible = false;
            }

            if (!Config.Misc.EnableBlogs)
            {
                tdBlog.Visible = false;
            }
            else if (CurrentUserSession != null)
            {
                if (CurrentUserSession.Level != null)
                {
                    if (!CurrentUserSession.Level.Restrictions.CanCreateBlogs &&
                            CurrentUserSession.CanCreateBlogs() == PermissionCheckResult.No)
                    {
                        tdBlog.Visible = false;
                    }
                }
                else if (CurrentUserSession.CanCreateBlogs() == PermissionCheckResult.No)
                {
                    tdBlog.Visible = false;
                }
            }

            if (!Config.Ratings.EnableRatePhotos)
            {
                tdRatePhotos.Visible = false;
            }
            else if (CurrentUserSession != null)
            {
                if (CurrentUserSession.Level != null)
                {
                    if (!CurrentUserSession.Level.Restrictions.CanRatePhotos &&
                            CurrentUserSession.CanRatePhotos() == PermissionCheckResult.No)
                    {
                        tdRatePhotos.Visible = false;
                    }
                }
                else if (CurrentUserSession.CanRatePhotos() == PermissionCheckResult.No)
                {
                    tdRatePhotos.Visible = false;
                }
            }

            if (!Config.Groups.EnableGroups)
            {
                tdGroups.Visible = false;
            }

            if (!Config.Ads.Enable)
            {
                tdAds.Visible = false;
            }

            tdVideos.Visible = Config.Misc.EnableVideoUpload || Config.Misc.EnableYouTubeVideos;

            if (!Page.IsPostBack)
            {
                string[] lang = Request.UserLanguages;
                foreach (Language language in Language.FetchAll())
                {
                    if (!language.Active) continue;
                    var listItem = new ListItem(language.Name, language.Id.ToString());
                    if (language.Id == PageBase.GetLanguageId())
                        listItem.Selected = true;
                    ddLanguages.Items.Add(listItem);
                }

                if (ddLanguages.Items.Count <= 1)
                {
                    if (ddLanguages.Items.Count == 1)
                        ddLanguages.SelectedIndex = 0;
                    ((PageBase)Page).LanguageId = Convert.ToInt32(ddLanguages.SelectedValue);
                    pnlLanguage.Visible = false;
                }
            }
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            if (Request.Cookies["rememberMe"] != null)
            {
                var cookie = new HttpCookie("rememberMe") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(cookie);
            }

            string redirectUrl = (Config.Users.RedirectAfterLogout.Length > 0)
                ? Config.Users.RedirectAfterLogout
                : "~/default.aspx";

            bool logoutFromFacebook = false;
            if (CurrentUserSession != null)
            {
                logoutFromFacebook = CurrentUserSession.LoggedInThroughFacebook;
                if (Config.Misc.EnableFacebookIntegration && logoutFromFacebook)
                {
                    FacebookHelper.SessionClear(redirectUrl);
                }
                CurrentUserSession = null;
            }
            MatchmakerHelper.CurrentUsername = null;
            MatchmakerHelper.SetMatchmakerState(false);

            Classes.MySpace.DataAvailability.RevokeAccess(Context);
            Response.Redirect(redirectUrl, true);
        }

        protected void ddLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            ((PageBase)Page).LanguageId = (Convert.ToInt32(ddLanguages.SelectedValue));
            UserSession.UpdatePopupTips(CurrentUserSession);
            Response.Redirect(Request.Url.ToString());
        }

        private void LoadPages()
        {
            var lContentPages = new ArrayList();

            Classes.ContentPage[] contentPages =
                    Classes.ContentPage.FetchContentPages(ddLanguages.SelectedItem == null ? PageBase.GetLanguageId() :
                                                                                                    Convert.ToInt32(ddLanguages.SelectedValue),
                                                                                                Classes.ContentPage.eSortColumn.HeaderPosition);

            bool loggedOn = CurrentUserSession != null;
            bool isPaid = CurrentUserSession != null && Classes.User.IsPaidMember(CurrentUserSession.Username);

            foreach (Classes.ContentPage contentPage in contentPages)
            {
                if (contentPage.HeaderPosition != null)
                {
                    if (
                            ((loggedOn && ((contentPage.VisibleFor & Classes.ContentPage.eVisibility.LoggedOnUsers) != 0 ||
                                    contentPage.VisibleFor == Classes.ContentPage.eVisibility.Paid && isPaid ||
                                    contentPage.VisibleFor == Classes.ContentPage.eVisibility.Unpaid && !isPaid)))
                            ||
                            ((!loggedOn && (contentPage.VisibleFor & Classes.ContentPage.eVisibility.NotLoggedOnUsers) != 0))
                         )
                        lContentPages.Add(contentPage);
                }
            }

            rptPages.DataSource = lContentPages.ToArray();
            rptPages.DataBind();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void lnkAjaxChatPay_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null)
            {
                Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.UserCanUseChat;
                Response.Redirect("~/ManageProfile.aspx?sel=payment");
            }
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}