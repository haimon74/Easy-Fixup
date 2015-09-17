using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ezFixUp.Components
{
    [ToolboxData("<{0}:ContentView runat=server></{0}:ContentView>")]
    [AspNetHostingPermission(SecurityAction.Demand,
        Level = AspNetHostingPermissionLevel.Minimal)]
    [ParseChildren(true, "Text")]
    [DefaultProperty("Text")]
    public class ContentView : WebControl
    {
        private bool fetchFromDatabase;
        private string key;

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
                var s = (String) ViewState["Text"];
                return s ?? String.Empty;
            }

            set { ViewState["Text"] = value; }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (fetchFromDatabase)
            {
                var parentPage = Page as PageBase;
                var mobileParentPage = Page as Mobile.PageBase;
                if (parentPage == null && mobileParentPage == null)
                {
                    throw new Exception("ContentView control can be put in PageBase descendants only!");
                }

                Classes.ContentView cv = Classes.ContentView.FetchContentView(key,
                                                                              parentPage != null
                                                                                  ? parentPage.LanguageId
                                                                                  : mobileParentPage.LanguageId);
                if (cv != null && cv.Content.Length > 0)
                    Text = cv.Content;
                else
                {
                    if (cv == null)
                        cv = new Classes.ContentView(key,
                                                     parentPage != null
                                                         ? parentPage.LanguageId
                                                         : mobileParentPage.LanguageId);

                    cv.Content = Text.Trim();
                    cv.Save();
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
            var state = (object[]) savedState;
            base.LoadViewState(state[0]);
            key = (string) state[1];
            fetchFromDatabase = (bool) state[2];
        }

        protected override object SaveViewState()
        {
            return new[] {base.SaveViewState(), key, fetchFromDatabase};
        }
    }
}