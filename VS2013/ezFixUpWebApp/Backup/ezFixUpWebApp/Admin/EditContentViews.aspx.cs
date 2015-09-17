using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class EditContentViews : AdminPageBase
    {
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "98%", "500px");

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }

                LoadStrings();
                PopulateDropDown();
            }
        }

        private void PopulateDropDown()
        {
            ddLanguage.Items.Add(new ListItem("", "-1"));

            foreach (Language language in Language.FetchAll())
            {
                if (!language.Active) continue;
                ddLanguage.Items.Add(new ListItem(language.Name, language.Id.ToString()));
            }

            if (ddLanguage.Items.Count <= 2)
            {
                if (ddLanguage.Items.Count == 2)
                    ddLanguage.SelectedIndex = 1;
                trLanguage.Visible = false;
                ddLanguage_SelectedIndexChanged(this, null);
            }
            else
            {
                trLanguage.Visible = true;
                trPageName.Visible = false;
            }
        }

        private void PopulateDDContentKeys(int languageID)
        {
            ddContentKey.Items.Clear();

            ddContentKey.Items.Add(new ListItem("", "-1"));

            ContentView[] contentViews = ContentView.FetchContentView(languageID);

            foreach (ContentView cv in contentViews)
            {
                ddContentKey.Items.Add(cv.Key);
            }

            trPageName.Visible = contentViews.Length > 0;
        }        

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editContentPages;            
            base.OnInit(e);
        }

        public void LoadStrings()
        {
            btnSave.Text = Lang.TransA("Save");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            string content = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
            string key = ddContentKey.SelectedValue;
            int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);

            Classes.ContentView cv = Classes.ContentView.FetchContentView(key, languageID);

            if (cv == null)
            {
                cv = new Classes.ContentView(key, languageID);
                cv.Content = content;
            }
            else
            {
                cv.Content = content;
                cv.LanguageID = languageID;
            }

            cv.Save();
            PopulateDDContentKeys(languageID);
            ddContentKey.SelectedValue = cv.Key;            
        }

        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            divEditContentView.Visible = false;

            if (ddLanguage.SelectedItem.Value == "-1")
            {
                trPageName.Visible = false;
            }
            else
            {
                int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);
                trPageName.Visible = true;
                PopulateDDContentKeys(languageID);
            }
        }

        protected void ddContentKey_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddContentKey.SelectedItem.Value == "-1")
            {
                divEditContentView.Visible = false;
                return;
            }
            else
            {
                divEditContentView.Visible = true;
                string key = ddContentKey.SelectedItem.Value;
                ContentView cv = ContentView.FetchContentView(key, Int32.Parse(ddLanguage.SelectedValue));
                if (ckeditor != null)
                    ckeditor.Text = cv.Content;
                else if (htmlEditor != null)
                        htmlEditor.Content = cv.Content;
            }            
        }
    }
}
