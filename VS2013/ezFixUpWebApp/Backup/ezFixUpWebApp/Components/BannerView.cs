using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    [ToolboxData("<{0}:BannerView runat=server></{0}:BannerView>")]
    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Text")]
    [DefaultProperty("Text")]
    public class BannerView : WebControl
    {
        private string key;
        private bool fetchFromDatabase;

        public string Key
        {
            get { return key; }
            set
            {
                if (key != value)
                {
                    key = value;
                    fetchFromDatabase = true;
                }
            }
        }

        [Bindable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Localizable(true)]
        public string Text
        {
            get
            {
                String s = (String)ViewState["Text"];
                return s ?? String.Empty;
            }

            set
            {
                ViewState["Text"] = value;
            }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!Page.IsPostBack/* && Config.Misc.DoNotShowBannersToPaidMembers*/
                && CurrentUserSession != null
                && CurrentUserSession.BillingPlanOptions.DoNotShowBanners.Value /*!User.IsNonPaidMember(CurrentUserSession.Username)*/)
            {
                Visible = false;
                return;
            }

            if (fetchFromDatabase)
            {
                PageBase parentPage = Page as PageBase;
                if (parentPage == null)
                    throw new Exception("BannerView control can be put in PageBase descendants only!");

                bool isBannerCodeFound = false;
                BannerCode[] bannerCodes =
                BannerCode.Fetch((BannerCode.ePosition)Enum.Parse(typeof(BannerCode.ePosition), key), BannerCode.eSortColumn.Priority);
                foreach (BannerCode bannerCode in bannerCodes)
                {
                    BannerCodeTarget target = Misc.FromXml<BannerCodeTarget>(bannerCode.Target);

                    if (CurrentUserSession == null)
                    {
                        if (target != null)
                        {
                            if ((target.VisibleFor == null || target.VisibleFor == BannerCode.eVisibleFor.NonLoggedInVisitors)
                                && String.IsNullOrEmpty(target.Region) && String.IsNullOrEmpty(target.City)
                                && target.Gender == null && target.Paid == null
                                && target.FromAge == null && target.ToAge == null
                                && !String.IsNullOrEmpty(target.Country)
                                && target.Country == Config.Users.GetCountryByCode(IPToCountry.GetCountry(HttpContext.Current.Request.UserHostAddress)))
                            {
                                Text = bannerCode.Code;
                                isBannerCodeFound = true;
                                break;
                            }
                        }
                    }
                    else if (target != null)
                    {
                        if ((target.VisibleFor == null || target.VisibleFor == BannerCode.eVisibleFor.LoggedInUsers)
                            && (String.IsNullOrEmpty(target.Country) || target.Country == CurrentUserSession.Country)
                            && (String.IsNullOrEmpty(target.Region) || target.Region == CurrentUserSession.State)
                            && (String.IsNullOrEmpty(target.City) || target.City == CurrentUserSession.City)
                            && (!target.Gender.HasValue || target.Gender == CurrentUserSession.Gender)
                            && (!target.Paid.HasValue || target.Paid == CurrentUserSession.Paid)
                            && (target.FromAge == null || target.FromAge <= CurrentUserSession.Age)
                            && (target.ToAge == null || target.ToAge >= CurrentUserSession.Age))
                        {
                            Text = bannerCode.Code;
                            isBannerCodeFound = true;
                            break;
                        }
                    }
                }

                if (!isBannerCodeFound)
                {
                    BannerCode bannerCode =
                        BannerCode.FetchDefault((BannerCode.ePosition) Enum.Parse(typeof (BannerCode.ePosition), key));

                    if (bannerCode != null) Text = bannerCode.Code;
                }

                fetchFromDatabase = false;
            }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(Text);
        }

        protected override void LoadViewState(object savedState)
        {
            object[] state = (object[])savedState;
            base.LoadViewState(state[0]);
            key = (string)state[1];
            fetchFromDatabase = (bool)state[2];
        }

        protected override object SaveViewState()
        {
            return new object[] { base.SaveViewState(), key, fetchFromDatabase };
        }
    }
}
