/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using ezFixUp.Classes;

namespace ezFixUp {
	public partial class _default : PageBase {
		public _default() {
			RequiresAuthorization = false;
		}

		protected override void OnPreInit(EventArgs e) {
			if(Config.Misc.EnableMobileVersion && isMobileBrowser()) {
				Response.Redirect("~/mobile/default.aspx");
                //Server.Transfer("~/mobile/default.aspx");
				return;
			}

			if(false && Properties.Settings.Default.AutoAddWWW
					&& !Request.Url.Host.ToLowerInvariant().Contains("www.")
					&& Config.Urls.Home.ToLowerInvariant().Contains("www.")) {
				Response.Redirect(Config.Urls.Home);
                //Server.Transfer(Config.Urls.Home);
			}

			base.OnPreInit(e);
		}

		protected void Page_Load(object sender, EventArgs e) {
			
            if(CurrentUserSession != null && CurrentUserSession.IsAuthorized) 
            {                
				Response.Redirect(MatchmakerHelper.CurrentHomePage);
                //Server.Transfer(MatchmakerHelper.CurrentHomePage);
			} 
            else if(Request.UrlReferrer != null) {
			
                String referer = Request.UrlReferrer.AbsoluteUri;

				if (referer.StartsWith("https://apps.facebook.com/ezFixUp_local/") || referer.StartsWith("https://apps.facebook.com/ezFixUp/")
                    || referer.StartsWith("http://apps.facebook.com/ezFixUp_local/") || referer.StartsWith("http://apps.facebook.com/ezFixUp/")
                    || referer.StartsWith("apps.facebook.com/ezFixUp_local/") || referer.StartsWith("apps.facebook.com/ezFixUp/"))
                {
                    ezFixUpFacebookApp app;

					if((Request.QueryString["access_token"] != null) && (Request.QueryString["access_token"].Length > 0)) {
                        Server.Transfer("LoginThroughFacebook.aspx?access_token" + Request.QueryString["access_token"]);
					} else {
                        //Server.Transfer("LoginThroughFacebook.aspx?facebook=1");
                        app = new ezFixUpFacebookApp();
                        Dictionary<String, Object> p = new Dictionary<String, object>();
                        
                        p["req_perms"] = ConfigurationManager.AppSettings["FacebookPermissions"];
                        p["next"] = Config.Urls.FacebookHome + "/LoginThroughFacebook.aspx";
                        p["canvas"] = 1;
                        p["fbconnect"] = 0;

                        Response.Write("<html>\n");
                        Response.Write("<head>\n");
                        Response.Write("<script type='text/javascript' src='https://connect.facebook.net/en_US/all.js'></script>\n");
                        Response.Write("</head>\n");
                        Response.Write("<body>\n");
                        Response.Write("<script type='text/javascript'>\n");
                        Response.Write("if (parent != self) \n");
                        Response.Write("	top.location.href = \"" + app.GetLoginUrl(p).ToString() + @"&v=1.0" + "\";\n");
                        //Response.Write("	top.location.href = \"" + app.GetLoginUrl(p).ToString() + "\";\n");
                        Response.Write("else\n");
                        Response.Write("	self.location.href = \"" + app.GetLoginUrl(p).ToString() + @"&v=1.0" + "\";\n");
                        //Response.Write("	self.location.href = \"" + app.GetLoginUrl(p).ToString() + "\";\n");
                        Response.Write("</script>\n");
                        Response.Write("</body>\n");
                        Response.Write("</html>\n");
					}
				}
			}

			if(!Page.IsPostBack) {
				preparePage();
				prepareRssLinks();
			}

			loadComponents();
		}

		private void preparePage() {
			#region Check is the user is invited to register

			string invitedByUsername = Request.QueryString["invitedBy"];
			if(!string.IsNullOrEmpty(invitedByUsername)) {
				// ReSharper disable PossibleNullReferenceException
				Response.Cookies["invitedBy"].Value = invitedByUsername;
				Response.Cookies["invitedBy"].Expires = DateTime.Now.AddDays(7);
				// ReSharper restore PossibleNullReferenceException
			}

			#endregion

			#region Check is the user coming to register through affiliate

			string affiliateID = Request.QueryString["affid"];
			if(!string.IsNullOrEmpty(affiliateID)) {
				// ReSharper disable PossibleNullReferenceException
				Response.Cookies["affiliateID"].Value = affiliateID;
				Response.Cookies["affiliateID"].Expires = DateTime.Now.AddDays(7);
				// ReSharper restore PossibleNullReferenceException
			}

			#endregion

			#region Check Remember Me option

			if(Request.Cookies["rememberMe"] != null) {
                Response.Redirect("~/Login.aspx");
			}

			#endregion
		}

		private void loadComponents() {
			string cacheKey = String.Format("_default_loadComponents_{0}", Theme);
			XDocument xdoc;
			if(Cache[cacheKey] != null) {
				xdoc = XDocument.Parse((string)Cache[cacheKey]);
			} else {

				var schemaPath = Server.MapPath("~/App_Themes/" + Theme + "/default.xml");
				if(!File.Exists(schemaPath)) return;
				xdoc = XDocument.Load(schemaPath);
				Cache.Insert(cacheKey, xdoc.ToString(), null, Cache.NoAbsoluteExpiration,
						TimeSpan.FromMinutes(10), CacheItemPriority.High, null);
			}
			var components = (from c in xdoc.Descendants("Body")
												select c).First().Elements();
			foreach(var component in components) {
				switch(component.Name.LocalName) {
					case "Component":
						XAttribute srcAttribute = component.Attribute("Src");
						if(srcAttribute == null) continue;
						var control = LoadControl(srcAttribute.Value);
						XAttribute idAttribute = component.Attribute("ID");
						if(idAttribute != null)
							control.ID = idAttribute.Value;
						XAttribute skinAttribute = component.Attribute("SkinID");
						if(skinAttribute != null)
							control.SkinID = skinAttribute.Value;
						XAttribute optionsAttribute = component.Attribute("Options");
						if(control is IOptions && optionsAttribute != null)
							((IOptions)control).Options = optionsAttribute.Value;
						plhComponents.Controls.Add(control);
						break;
					case "HtmlContent":
						if(!string.IsNullOrEmpty(component.Value)) {
							plhComponents.Controls.Add(new LiteralControl(component.Value));
						}
						break;
					case "ContentView":
						if(!String.IsNullOrEmpty(component.Value)) {
							Components.ContentView cv = new Components.ContentView();
							cv.Text = component.Value;
							XAttribute key = component.Attribute("Key");
							if(key != null) cv.Key = key.Value;
							plhComponents.Controls.Add(cv);
						}
						break;
				}
			}
		}

		private void prepareRssLinks() {
			var rssNewUsers = new HtmlLink();
			rssNewUsers.Attributes.Add("rel", "alternate");
			rssNewUsers.Attributes.Add("type", "application/rss+xml");
			rssNewUsers.Attributes.Add("title", Config.Misc.SiteTitle + " - " + Lang.Trans("New Users"));
			rssNewUsers.Attributes.Add("href", "Rss.ashx?action=newusers");
			Header.Controls.Add(rssNewUsers);

			var rssNews = new HtmlLink();
			rssNews.Attributes.Add("rel", "alternate");
			rssNews.Attributes.Add("type", "application/rss+xml");
			rssNews.Attributes.Add("title", Config.Misc.SiteTitle + " - " + Lang.Trans("News"));
			rssNews.Attributes.Add("href", "Rss.ashx?action=news");
			Header.Controls.Add(rssNews);
		}

		private static bool isMobileBrowser() {
			HttpContext context = HttpContext.Current;

			if(context.Request.Browser.IsMobileDevice) {
				return true;
			}
			if(context.Request.ServerVariables["HTTP_X_WAP_PROFILE"] != null) {
				return true;
			}
			if(context.Request.ServerVariables["HTTP_ACCEPT"] != null &&
					context.Request.ServerVariables["HTTP_ACCEPT"].ToLower().Contains("wap")) {
				return true;
			}
			if(context.Request.ServerVariables["HTTP_USER_AGENT"] != null) {
				//Create a list of all mobile types
				string[] mobiles =
						new[]
                        {
                            "240x320", "blackberry", "symbian", "android",
                            "wireless", "nokia", "phone", "iphone"
                        };

				for(int i = 0; i < mobiles.Length; i++) {
					if(context.Request.ServerVariables["HTTP_USER_AGENT"]
							.IndexOf(mobiles[i], StringComparison.InvariantCultureIgnoreCase) >= 0) {
						return true;
					}
				}
			}

			return false;
		}
	}
}