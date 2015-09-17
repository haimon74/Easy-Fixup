using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using AspNetDating.Classes;
using eStreamBG.Dating;

namespace AspNetDating.Mobile
{
    public class PageBase : InternalPage
    {
        /// <summary>
        /// Set to false if the page will be visible to non-logged users
        /// </summary>
        public bool RequiresAuthorization = true;
        /// <summary>
        /// Set to true if you do not want IM available on the page
        /// </summary>
        public bool DoNotRegisterIMJavascript = false;

        /// <summary>
        /// Gets the current user session.
        /// </summary>
        /// <returns></returns>
        public static UserSession GetCurrentUserSession()
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Session["UserSession"] as UserSession;
            }

            return null;
        }
        /// <summary>
        /// Sets the current user session.
        /// </summary>
        /// <param name="newUserSession">The new user session.</param>
        public static void SetCurrentUserSession(UserSession newUserSession)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session["UserSession"] = newUserSession;
            }
        }
        /// <summary>
        /// Gets or sets the current user session.
        /// </summary>
        /// <value>The current user session.</value>
        public new UserSession CurrentUserSession
        {
            get { return base.CurrentUserSession as UserSession; }
            set
            {
                base.CurrentUserSession = value;

                if (value != null) // it will be null if the user has been logout
                {
                    if (Session["LanguageIdExplicit"] == null
                    || (bool)Session["LanguageIdExplicit"] == false)
                    {
                        LanguageId = value.LanguageId;
                    }
                    else
                    {
                        CurrentUserSession.LanguageId = LanguageId;
                        CurrentUserSession.Update();
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the status page message.
        /// </summary>
        /// <value>The status page message.</value>
        public string StatusPageMessage
        {
            set { Session["ShowStatus_Message"] = value; }
            get
            {
                if (Session["ShowStatus_Message"] == null)
                    return null;
                else
                    return (string)Session["ShowStatus_Message"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link URL.
        /// </summary>
        /// <value>The status page link URL.</value>
        public string StatusPageLinkURL
        {
            set { Session["ShowStatus_LinkURL"] = value; }
            get
            {
                if (Session["ShowStatus_LinkURL"] == null)
                    return null;
                else
                    return (string)Session["ShowStatus_LinkURL"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link text.
        /// </summary>
        /// <value>The status page link text.</value>
        public string StatusPageLinkText
        {
            set { Session["ShowStatus_LinkText"] = value; }
            get
            {
                if (Session["ShowStatus_LinkText"] == null)
                    return null;
                else
                    return (string)Session["ShowStatus_LinkText"];
            }
        }

        /// <summary>
        /// Gets or sets the status page link skind id.
        /// </summary>
        /// <value>The status page link skind id.</value>
        public string StatusPageLinkSkindId
        {
            set { Session["ShowStatus_LinkSkindId"] = value; }
            get
            {
                if (Session["ShowStatus_LinkSkindId"] == null)
                    return null;
                else
                    return (string)Session["ShowStatus_LinkSkindId"];
            }
        }

        /// <summary>
        /// Gets or sets the language id.
        /// </summary>
        /// <value>The language id.</value>
        public int LanguageId
        {
            set
            {
                Session["LanguageId"] = value;

                HttpCookie cookie = new HttpCookie("LanguageId", value.ToString());
                cookie.Expires = DateTime.Now.AddMonths(6);
                Response.Cookies.Add(cookie);

                if (IsUserAuthorized())
                {
                    if (CurrentUserSession.LanguageId != value)
                    {
                        CurrentUserSession.LanguageId = value;
                        CurrentUserSession.Update();
                    }
                }
                else
                {
                    Session["LanguageIdExplicit"] = true;
                }
            }
            get
            {
                return GetLanguageId();
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event to initialize the page.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            if (RequiresAuthorization)
            {
                ProcessValidation();
            }

            if (CurrentUserSession != null)
                CurrentUserSession.UpdateLastOnline(false);

            CommunityFaceControlValidation();

            /*
            if (Config.Misc.EnableIntegratedIM && IsUserAuthorized() && !DoNotRegisterIMJavascript)
            {
                ClientScript.RegisterClientScriptInclude("IntegratedIM", "IM/functions.js");
            }
            */

            base.OnInit(e);
        }

        /// <summary>
        /// Gets the language id.
        /// </summary>
        /// <returns></returns>
        public static int GetLanguageId()
        {
            HttpContext ctx = HttpContext.Current;
            if (ctx.Session["LanguageId"] == null)
            {
                int languageId = Config.Misc.DefaultLanguageId;
                if (ctx.Request.Cookies["LanguageId"] != null)
                {
                    try
                    {
                        languageId = Convert.ToInt32(ctx.Request.Cookies["LanguageId"].Value);
                    }
                    catch (FormatException) { }
                }
                else
                {
                    //                    bool match = false;
                    //                    foreach (var language in Language.FetchAll())
                    //                    {
                    //                        if (!language.Active) continue;
                    //
                    //                        if (language.BrowserLanguages != null)
                    //                        {
                    //                            foreach (string browserLanguage in language.BrowserLanguages.Split(','))
                    //                            {
                    //                                foreach (var userLanguage in ctx.Request.UserLanguages)
                    //                                {
                    //                                    if (CultureInfo.CreateSpecificCulture(browserLanguage.ToLowerInvariant()).Name
                    //                                        == CultureInfo.CreateSpecificCulture(userLanguage.ToLowerInvariant()).Name)
                    //                                    {
                    //                                        languageId = language.Id;
                    //                                        match = true;
                    //                                        break;
                    //                                    }
                    //                                }
                    //                                if (match) break;
                    //                            }
                    //                            if (match) break;
                    //                        }
                    //                        
                    //                        if (language.IpCountries != null)
                    //                        {
                    //                            string countryCode = IPToCountry.GetCountry(ctx.Request.UserHostAddress);
                    //                            foreach (string ipCountry in language.IpCountries.Split(','))
                    //                            {
                    //                                if (Config.Users.GetCountryByCode(countryCode).ToLowerInvariant() == ipCountry.ToLowerInvariant()
                    //                                    || countryCode.ToLowerInvariant() == ipCountry.ToLowerInvariant())
                    //                                {
                    //                                    languageId = language.Id;
                    //                                    match = true;
                    //                                    break;
                    //                                }
                    //                            }
                    //                            if (match) break;
                    //                        }
                    //                    }

                    var languages = new List<Language>(Language.FetchAll());
                    var found = (from l in languages
                                 where l.Active &&
                                 (
                                    (!String.IsNullOrEmpty(l.BrowserLanguages) &&
                                    Array.Exists(l.BrowserLanguages.Split(','),
                                        browserLanguage => ctx.Request.UserLanguages.FirstOrDefault(
                                            userLanguage =>
                                                CultureInfo.CreateSpecificCulture(browserLanguage.ToLowerInvariant()).Name
                                                == CultureInfo.CreateSpecificCulture(userLanguage.Split(';')[0].ToLowerInvariant()).Name
                                            ) != null))
                                    ||
                                    (!String.IsNullOrEmpty(l.IpCountries) &&
                                    Array.Exists(l.IpCountries.Split(','),
                                        ipCountry =>
                                            ipCountry.ToLowerInvariant() == Config.Users.GetCountryByCode(
                                                IPToCountry.GetCountry(ctx.Request.UserHostAddress)).ToLowerInvariant()
                                        || ipCountry.ToLowerInvariant() == IPToCountry.GetCountry(
                                                                            ctx.Request.UserHostAddress).ToLowerInvariant()))
                                 )
                                 select new { ID = l.Id }).FirstOrDefault();

                    if (found != null && found.ID != 0) languageId = found.ID;
                }

                ctx.Session["LanguageId"] = languageId;
                return languageId;
            }
            return (int)ctx.Session["LanguageId"];
        }

        /// <summary>
        /// Processes the validation.
        /// </summary>
        protected void ProcessValidation()
        {
            if (!IsUserAuthorized())
            {
                Response.Redirect("~/mobile/default.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }
        }

        /// <summary>
        /// Determines whether the user is authorized.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if the user is authorized; otherwise, <c>false</c>.
        /// </returns>
        public bool IsUserAuthorized()
        {
            if (CurrentUserSession == null)
                return false;

            return CurrentUserSession.IsAuthorized;
        }

        /// <summary>
        /// Logs the specified exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void Log(Exception ex)
        {
            ExceptionLogger.Log(Request, ex);
        }

        private void CommunityFaceControlValidation()
        {
            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.IsAdmin())
                {
                    return;
                }

                if (Config.CommunityFaceControlSystem.EnableCommunityFaceControl
                    && !CurrentUserSession.FaceControlApproved
                    && !(Page is ManageMemberProfile) && !(Page is ShowStatus))
                {
                    if (!CurrentUserSession.HasProfile)
                    {
                        StatusPageMessage = Lang.Trans("You must complete your profile in order to continue!");
                    }
                    else if (Photo.Search(-1, CurrentUserSession.Username, null, null, null, null, null).Length < Config.CommunityFaceControlSystem.MinPhotosRequired)
                    {
                        StatusPageMessage =
                            String.Format(Lang.Trans("You must upload at least {0} photos in order to continue!"),
                                          Config.CommunityFaceControlSystem.MinPhotosRequired);
                    }
                    else
                    {
                        StatusPageMessage = Lang.Trans("Your profile is completed and is awaiting approval from the rest of the users!");
                    }

                    Response.Redirect("Default.aspx");
                }
            }
        }
    }
}
