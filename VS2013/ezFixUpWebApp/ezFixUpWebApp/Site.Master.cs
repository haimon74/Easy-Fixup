using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;


namespace ezFixUp
{
    public partial class Site : MasterPage
    {
        public ScriptManager ScriptManager
        {
            get
            {
                return ScriptManagerMaster;
            }
        }
        public void InitNotify(string StrSplash)
        {
            // Only do this on the first call to the page
            if ((!Page.IsCallback) && (!IsPostBack))
            {
                //Register loadingNotifier.js for showing the Progress Bar
                Response.Write(
@"<script type='text/javascript' src='scripts/loadingNotifier.js'></script>
  <script language='javascript' type='text/javascript'>
  function initWhenJQueryWindowLoad() {
        if (window.$ && jQuery) {
               $(window).load(function() {
                    initLoader('" + StrSplash + @"');
               });
        }
        else{window.setTimeout(initWhenJQueryWindowLoad, 50);}
  }
  initWhenJQueryWindowLoad()
</script>");
                // Send it to the client
                //Response.Flush();
            }
        }
        public void Notify(string strPercent, string strMessage)
        {
            // Only do this on the first call to the page
            if ((!Page.IsCallback) && (!IsPostBack))
            {
                //Update the Progress bar

                Response.Write(string.Format("<script language='javascript' type='text/javascript'>setProgress({0},'{1}'); </script>", strPercent, strMessage));
                //Response.Flush();

            }
        }
        protected void Page_Init(object sender, EventArgs e)
        {
            // Define an HtmlLink control.            
            //var myHtmlLink = new HtmlLink { Href = "Styles/Site.css" };
            //myHtmlLink.Attributes.Add("rel", "stylesheet");
            //myHtmlLink.Attributes.Add("type", "text/css");
            //Page.Header.Controls.Add(myHtmlLink);
            //myHtmlLink.Href = "Images/common.css";
            //Page.Header.Controls.Add(myHtmlLink);
            //myHtmlLink.Href = "css/hot-sneaks/jquery-ui-1.8.7.custom.css";
            //Page.Header.Controls.Add(myHtmlLink);
            //myHtmlLink.Href = "Images/common.css";
            //Page.Header.Controls.Add(myHtmlLink);
            //myHtmlLink.Href = "scripts/fmt_vmenu/css/fmtstyle.css";
            //Page.Header.Controls.Add(myHtmlLink);

            //Page.ClientScript.RegisterClientScriptInclude("jquery", "scripts/jquery-1.4.4.min.js");
            //Page.RegisterJQuery();
            //ScriptManager.CompositeScript.Scripts.Add(
            //        new ScriptReference("scripts/jquery-ui-1.8.7.custom.min.js"));
            //Page.ClientScript.RegisterClientScriptInclude("jqueryuilive", "scripts/jquery-ui.live.js");
            //Page.ClientScript.RegisterClientScriptInclude("fmt", "scripts/fmt_vmenu/fmt.js");

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Title = "ezFixUp";
            
            //if (!IsPostBack)
            {
                loadStrings();
            }
            if (!(Page is ShowStatus || IsPostBack))
            {
                if (Request.QueryString["InvitedBy"] != null)
                    Global.GetSessionState()["InvitedBy"] = Request.QueryString["InvitedBy"];

                Global.GetSessionState()["CurrentRawUrl"] = Request.Url.Authority + Request.RawUrl;
                Global.GetSessionState()["CurrentPage"] = Request.Url.Authority + Request.Url.LocalPath;

                if (Request.UrlReferrer != null && Request.Url.LocalPath != Request.UrlReferrer.LocalPath)
                    Global.GetSessionState()["RefererPage"] = Request.UrlReferrer.Authority + Request.UrlReferrer.LocalPath;
            }
            // Add script reference for online status checking
            if (Page is PageBase && PageBase.IsUserAuthorized())
            {
                //var service = new ServiceReference("Services/OnlineCheck.asmx");
                //ScriptManagerMaster.Services.Add(service);

                var basePage = (Page as PageBase);
                var curPageName = Request.Url.Segments[1].ToLower();
                var lstTips = basePage.CurrentUserSession.TipsToPageViewDic.ContainsKey(curPageName)
                    ? basePage.CurrentUserSession.TipsToPageViewDic[curPageName] : null;
                basePage.TipMessage = lstTips != null
                                            ? lstTips[0].Content.Replace('\"', '\'')
                                                                .Replace("&lt;", "<")
                                                                .Replace("&gt;", ">")
                                                                .Replace("\r\n", "<br/>")
                                                                .Replace("\n", "<br/>")
                                            : null;
                basePage.TipTitle = lstTips != null ? lstTips[0].Title : null;

                //ScriptManagerMaster.CompositeScript.Scripts.Add(
                //    new ScriptReference("scripts/OnlineCheck.js"));

                //Page.ClientScript.RegisterStartupScript(GetType(), "InitializeOnlineCheck",
                //                                        String.Format("InitializeOnlineCheck({0});",
                //                                                      Config.Users.OnlineCheckTime * 60000),
                //                                        true);
            }

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "body",
                                                        String.Format("var bodyId = '{0}';", body.ClientID), true);

            // Add script reference for user preview popup
            //            ScriptManagerMaster.CompositeScript.Scripts.Add(
            //                new ScriptReference("scripts/UserPreview.js"));
            //            Page.ClientScript.RegisterStartupScript(GetType(), "InitializeMouseTracking",
            //                                                    "InitializeMouseTracking();", true);

            // Add script for detecting ad blockers
            if (Config.Misc.StopUsersWithAdBlocker && !(Page is ShowStatus)
                && Request.Browser.Browser != "IE") // Adblock detector doesn't work with IE
            {
                ScriptManagerMaster.CompositeScript.Scripts.Add(
                    new ScriptReference("scripts/adblock_detector.js"));
            }

            // Add the common.js
            //            ScriptManagerMaster.CompositeScript.Scripts.Add(
            //                new ScriptReference("Images/common.js"));

            // Add the jQuery 1.4 minimal version not from google's CDN
            //ScriptManagerMaster.CompositeScript.Scripts.Add(
            //new ScriptReference("scripts/jquery-1.4.4.min.js"));
            //new ScriptReference("https://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"));


            // Add the jQuery UI 1.8.7 minimal version not from google's CDN
            //ScriptManagerMaster.CompositeScript.Scripts.Add(
            //new ScriptReference("scripts/jquery-ui-1.8.7.custom.min.js"));
            //new ScriptReference("https://ajax.googleapis.com/ajax/libs/jqueryui/1.8.7/jquery-ui.min.js"));

            // Add the jquery.exts.js
            //            ScriptManagerMaster.CompositeScript.Scripts.Add(
            //                new ScriptReference("scripts/jquery.exts.js"));

            // Add the JSON.js
            //            ScriptManagerMaster.CompositeScript.Scripts.Add(
            //                new ScriptReference("scripts/JSON.js"));
            /*
            ScriptManagerMaster.CompositeScript.Scripts.Add(
                new ScriptReference("scripts/apycom14steelblue/jquery.js"));

            ScriptManagerMaster.CompositeScript.Scripts.Add(
                new ScriptReference("scripts/apycom14steelblue/menu.js"));
            */

            //ScriptManagerMaster.CompositeScript.Scripts.Add(
            //    new ScriptReference("scripts/fmt_vmenu/fmt.js"));

            //            Page.ClientScript.RegisterStartupScript(GetType(), "ContentWraperAnimation",
            //                                                    "$(function() { $('#content-wrapper').fadeIn('slow'); });", true);

            //Page.ClientScript.RegisterClientScriptBlock(GetType(), "DisabledOpacity",
            //                                        "function setDisabledOpacity() { $('a').each(function(i){ " +
            //                                            "if ($(this).attr('disabled') == 'disabled') "+
            //                                                " $(this).addClass('opacity10pct'); }); }", true);

            // Add the images lazy loading script
            if (Config.Photos.UseLazyImagesLoading)
            {
                //                ScriptManagerMaster.CompositeScript.Scripts.Add(
                //                    new ScriptReference("scripts/jquery.lazyload.js"));
                //                Page.ClientScript.RegisterStartupScript(GetType(), "ImagesLazyLoad",
                //                                                        "$(function() { $('img').lazyload(); });", true);
            }

            if (Page is Search3)
            {
                // Add script for image rollovers
                //                ScriptManagerMaster.CompositeScript.Scripts.Add(
                //                    new ScriptReference("scripts/Rollover.js"));

                // Add script for tipsy tooltips
                //                ScriptManagerMaster.CompositeScript.Scripts.Add(
                //                    new ScriptReference("scripts/jquery.tipsy.js"));
            }

            if (!IsPostBack)
            {
                // Add dynamic class on the body element depending on the content page
                var index = Request.Url.Segments.Length - 1;
                var filename = Request.Url.Segments[index];
                var pageClass = filename.Split('.')[0].ToLower();

                // Add class based on the browser type and version
                var browserType = Request.Browser.Browser.Replace(" ", "") + Request.Browser.MajorVersion;

                body.Attributes.Add("class", pageClass + " " + browserType);

                if (Page is Ads)
                {
                    SetMetaTags(Config.SEO.AdsTitleTemplate, Config.SEO.AdsMetaDescriptionTemplate,
                                Config.SEO.AdsMetaKeywordsTemplate);
                }
                else if (Page is ChangeLostPassword)
                {
                    SetMetaTags(Config.SEO.ChangeLostPasswordTitleTemplate,
                                Config.SEO.ChangeLostPasswordMetaDescriptionTemplate,
                                Config.SEO.ChangeLostPasswordMetaKeywordsTemplate);
                }
                else if (Page is _default)
                {
                    SetMetaTags(Config.SEO.DefaultPageTitleTemplate, Config.SEO.DefaultPageMetaDescriptionTemplate,
                                Config.SEO.DefaultPageMetaKeywordsTemplate);
                }
                else if (Page is Groups)
                {
                    SetMetaTags(Config.SEO.GroupsTitleTemplate, Config.SEO.GroupsMetaDescriptionTemplate,
                                Config.SEO.GroupsMetaKeywordsTemplate);
                }
                else if (Page is Login)
                {
                    SetMetaTags(Config.SEO.LoginTitleTemplate, Config.SEO.LoginMetaDescriptionTemplate,
                                Config.SEO.LoginMetaKeywordsTemplate);
                }
                else if (Page is LostPassword)
                {
                    SetMetaTags(Config.SEO.LostPasswordTitleTemplate, Config.SEO.LostPasswordMetaDescriptionTemplate,
                                Config.SEO.LostPasswordMetaKeywordsTemplate);
                }
                else if (Page is NewsPage)
                {
                    SetMetaTags(Config.SEO.NewsTitleTemplate, Config.SEO.NewsMetaDescriptionTemplate,
                                Config.SEO.NewsMetaKeywordsTemplate);
                }
                else if (Page is Registration)
                {
                    SetMetaTags(Config.SEO.RegisterTitleTemplate, Config.SEO.RegisterMetaDescriptionTemplate,
                                Config.SEO.RegisterMetaKeywordsTemplate);
                }
                else if (Page is Search3)
                {
                    SetMetaTags(Config.SEO.SearchTitleTemplate, Config.SEO.SearchMetaDescriptionTemplate,
                                Config.SEO.SearchMetaKeywordsTemplate);
                }
                else if (Page is SendProfile)
                {
                    SetMetaTags(Config.SEO.SendProfileTitleTemplate, Config.SEO.SendProfileMetaDescriptionTemplate,
                                Config.SEO.SendProfileMetaKeywordsTemplate);
                }
                else if (Page is ShowGroupEvents)
                {
                    SetMetaTags(Config.SEO.ShowGroupEventsTitleTemplate,
                                Config.SEO.ShowGroupEventsMetaDescriptionTemplate,
                                Config.SEO.ShowGroupEventsMetaKeywordsTemplate);
                }
                else if (Page is ShowGroupPhotos)
                {
                    SetMetaTags(Config.SEO.ShowGroupPhotosTitleTemplate,
                                Config.SEO.ShowGroupPhotosMetaDescriptionTemplate,
                                Config.SEO.ShowGroupPhotosMetaKeywordsTemplate);
                }
                else if (Page is SmsConfirm)
                {
                    SetMetaTags(Config.SEO.SmsConfirmTitleTemplate, Config.SEO.SmsConfirmMetaDescriptionTemplate,
                                Config.SEO.SmsConfirmMetaKeywordsTemplate);
                }
                else if (Page is TopCharts)
                {
                    SetMetaTags(Config.SEO.TopChartsTitleTemplate, Config.SEO.TopChartsMetaDescriptionTemplate,
                                Config.SEO.TopChartsMetaKeywordsTemplate);
                }

                // Add default page title
                if (Page.Header.Title.Length == 0)
                {
                    Page.Header.Title = Config.SEO.DefaultTitleTemplate.Replace("%%NAME%%", Config.Misc.SiteTitle);
                }

                Control[] controls = new Control[Page.Header.Controls.Count];
                Page.Header.Controls.CopyTo(controls, 0);

                var descriptionMetaTag = Array.Find(controls,
                                                    c => c is HtmlMeta && ((HtmlMeta)c).Name.ToLower() == "description");
                var keywordsMetaTag = Array.Find(controls,
                                                 c => c is HtmlMeta && ((HtmlMeta)c).Name.ToLower() == "keywords");

                if (descriptionMetaTag == null)
                {
                    descriptionMetaTag = new HtmlMeta
                                             {
                                                 ID = "Description",
                                                 Name = "description",
                                                 Content =
                                                     Config.SEO.DefaultMetaDescriptionTemplate.Replace("%%NAME%%",
                                                                                                       Config.Misc.
                                                                                                           SiteTitle)
                                             };
                    Page.Header.Controls.Add(descriptionMetaTag);
                }
                if (keywordsMetaTag == null)
                {
                    keywordsMetaTag = new HtmlMeta
                                          {
                                              ID = "Keywords",
                                              Name = "keywords",
                                              Content =
                                                  Config.SEO.DefaultMetaKeywordsTemplate.Replace("%%NAME%%",
                                                                                                 Config.Misc.SiteTitle)
                                          };
                    Page.Header.Controls.Add(keywordsMetaTag);
                }

                // Add Google Analytics
                ltrGoogleAnalytics.Text = Config.Misc.GoogleAnalyticsTrackingCode;
                //ltrFacebookScript.Visible =
                //    Config.Misc.EnableFacebookIntegration &&
                //    (Page is Register || Page is _default || Page is Login || Page is Home ||
                //     Page is InviteFriendsFromFacebook);
            }
            if (Global.GetSessionState()["LanguageId"] == null)
                Global.GetSessionState()["LanguageId"] = PageBase.GetLanguageId();

            bool isRtl = Language.Fetch(Convert.ToInt32(Global.GetSessionState()["LanguageId"])).Direction == "rtl";
            Global.GetSessionState()["Direction"] = isRtl ? "rtl" : "ltr";
            body.Attributes["dir"] = isRtl ? "rtl" : "ltr";
            string param0 = isRtl ? "right" : "left";
            string param1 = isRtl ? "left" : "right";
            string rtlLng = " function rtlLayout() { jQuery(document).ready(function() {" +
                "changecss('div.language','text-align', 'right !important');" +
                "changecss('div.SectionContent','text-align', 'right !important');" +
                "changecss('.SideMenuWebpartContent','text-align', 'right !important');" +
                "changecss('table .ItemOfSearchResults1 td','text-align', 'right !important');" +
                "changecss('.SideMenuContent','text-align', 'right !important');" +
                "changecss('div.SideMenuBoxContent','text-align', 'right !important');" +
                "changecss('.BoxHeader','text-align', 'right !important');" +
                "changecss('.tar','text-align', 'left !important');" +
                "changecss('.tal','text-align', 'right !important');" +

                "changecss('.SideMenuContent', 'float', 'right !important');" +
                "changecss('.SideMenuBoxContent', 'float', 'right !important');" +
                "changecss('div#pnlLanguage', 'float', 'right !important');" +
                "changecss('#registration_lt', 'float', 'right !important');" +
                "changecss('div#pnlLanguage', 'float', 'right !important');" +
                "changecss('#right .StandardWebpartsWrap', 'float', 'right !important');" +
                "changecss('#left .StandardWebpartsWrap', 'float', 'right !important');" +
                "changecss('#left', 'float', 'right !important');" +
                "changecss('#left','margin', '0 1% 0 0 !important');" +
                "changecss('ul#navi li','float', 'right !important');" +
                "changecss('div.labelHdr','float', 'right !important');" +
                //"changecss('label.prettyCheckbox span.holderWrap','float', 'right !important');" +
                //"changecss('label.prettyCheckbox','float', 'right !important');" +
                "changecss('label.customUnchecked','background-position', 'right -1px !important');" +
                "changecss('label.customChecked','background-position', 'right -81px !important');" +
                "changecss('label.radioUnchecked','background-position', 'right -161px !important');" +
                "changecss('label.radioChecked','background-position', 'right -241px !important');" +
                "changecss('.fl','float', 'right !important');" +
                "changecss('.fr','float', 'left !important');" +

                "changecss('div.logout','float', 'left !important');" +
                "changecss('div.labelHdr a','float', 'left !important');" +
                "changecss('#registration_rt','float', 'left !important');" +
                "changecss('#right','float', 'left !important');" +
                "changecss('#right','margin', '0 0 0 1% !important');" +

                "changecss('.plus1', 'background-position', 'right center !important');" +
                "changecss('.minus1', 'background-position', 'right center !important');" +
                "changecss('div.expander a', 'padding', '0 18px 0 0');" +
                "changecss('span.customStyleSelectBoxInner', 'background-position', 'left center !important');" +
                "changecss('span.customStyleSelectBoxInner', 'text-align', 'right');" +
                //"changecss('select', 'background-position', 'left center !important');" +
                //"changecss('select', 'padding', '2px 0px 2px 13px  !important');" +

                "changecss('.logo_header > tr','text-align','right');" +

                "changecss('#fmt ul li ul.fmtDrpDwn', 'left', '-100%');" +
                "changecss('#fmt ul li ul.fmtDrpDwn ul', 'left', '-100%');});}";

            string ltrLng = " function rtlLayout() { jQuery(document).ready(function() {" +
                "changecss('div.language','text-align', 'left !important');" +
                "changecss('div.SectionContent','text-align', 'left !important');" +
                "changecss('.SideMenuWebpartContent','text-align', 'left !important');" +
                "changecss('table .ItemOfSearchResults1 td','text-align', 'left !important');" +
                "changecss('.SideMenuContent','text-align', 'left !important');" +
                "changecss('div.SideMenuBoxContent','text-align', 'left !important');" +
                "changecss('.BoxHeader','text-align', 'left !important');" +
                "changecss('.tar','text-align', 'right !important');" +
                "changecss('.tal','text-align', 'left !important');" +

                "changecss('.SideMenuContent', 'float', 'left !important');" +
                "changecss('.SideMenuBoxContent', 'float', 'left !important');" +
                "changecss('div#pnlLanguage', 'float', 'left !important');" +
                "changecss('#registration_lt', 'float', 'left !important');" +
                "changecss('div#pnlLanguage', 'float', 'left !important');" +
                "changecss('#right .StandardWebpartsWrap', 'float', 'left !important');" +
                "changecss('#left .StandardWebpartsWrap', 'float', 'left !important');" +
                "changecss('#left', 'float', 'left !important');" +
                "changecss('#left','margin', '0 0 0 1%  !important');" +
                "changecss('ul#navi li','float', 'left !important');" +
                "changecss('div.labelHdr','float', 'left !important');" +
                //"changecss('label.prettyCheckbox span.holderWrap','float', 'left !important');" +
                //"changecss('label.prettyCheckbox','float', 'left !important');" +
                "changecss('label.customUnchecked','background-position', 'left -1px !important');" +
                "changecss('label.customChecked','background-position', 'left -81px !important');" +
                "changecss('label.radioUnchecked','background-position', 'left -161px !important');" +
                "changecss('label.radioChecked','background-position', 'left -241px !important');" +
                "changecss('.fl','float', 'left !important');" +
                "changecss('.fr','float', 'right !important');" +

                "changecss('div.logout','float', 'right !important');" +
                "changecss('div.labelHdr a','float', 'right !important');" +
                "changecss('#registration_rt','float', 'right !important');" +
                "changecss('#right','float', 'right !important');" +
                "changecss('#right','margin', '0 1% 0 0 !important');" +

                "changecss('.plus1', 'background-position', 'left center !important');" +
                "changecss('.minus1', 'background-position', 'left center !important');" +
                "changecss('div.expander a', 'padding', '0 0 0 18px');" +
                "changecss('span.customStyleSelectBoxInner', 'background-position', 'right center !important');" +
                "changecss('span.customStyleSelectBoxInner', 'text-align', 'left');" +
                //"changecss('select', 'background-position', 'right center !important');" +
                //"changecss('select', 'padding', '2px 13px 2px 0px  !important');" +

                "changecss('#fmt ul li ul.fmtDrpDwn', 'left', '100%');" +
                "changecss('#fmt ul li ul.fmtDrpDwn ul', 'left', '100%');});}";

            //if (isRtl && !Page.ClientScript.IsStartupScriptRegistered("fixRightLeftDirection"))
            //    Page.ClientScript.RegisterStartupScript(GetType(), "fixRightToLeftDirection", rtlLng, true);
            //else if (!isRtl && !Page.ClientScript.IsStartupScriptRegistered("fixLeftRightDirection"))
            //    Page.ClientScript.RegisterStartupScript(GetType(), "fixLeftRightDirection", ltrLng, true);
        }

        private void loadStrings()
        {
            lbModalMF1_AskAbout.ToolTip = "Ask about".Translate();
            lbModalMF2_AskAbout.ToolTip = "Ask about".Translate();
            lbModalMF3_AskAbout.ToolTip = "Ask about".Translate();
            lbModalMF1_MatchReq.ToolTip = "Match request".Translate();
            lbModalMF2_MatchReq.ToolTip = "Match request".Translate();
            lbModalMF3_MatchReq.ToolTip = "Match request".Translate();
            
        }

        private void SetMetaTags(string title, string metaDescription, string metaKeywords)
        {
            Control descriptionMetaTag;
            Control keywordsMetaTag;

            Page.Header.Title = title;

            if (metaDescription.Length > 0)
            {
                descriptionMetaTag = new HtmlMeta
                {
                    ID = "Description",
                    Name = "description",
                    Content = metaDescription
                };
                Page.Header.Controls.Add(descriptionMetaTag);
            }

            if (metaKeywords.Length > 0)
            {
                keywordsMetaTag = new HtmlMeta
                {
                    ID = "Keywords",
                    Name = "keywords",
                    Content = metaKeywords
                };
                Page.Header.Controls.Add(keywordsMetaTag);
            }
        }
    }
}