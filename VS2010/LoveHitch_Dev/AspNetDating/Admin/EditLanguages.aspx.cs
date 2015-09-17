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
using AspNetDating.Classes;
using System.Globalization;

namespace AspNetDating.Admin
{
    public partial class EditLanguages : AdminPageBase
    {
        private int? LanguageId
        {
            get { return ViewState["LanguageId"] as int?; }
            set { ViewState["LanguageId"] = value; }
        }

        private DataTable DataSource
        {
            get { return ViewState["EditLanguagesDataSource"] as DataTable; }
            set { ViewState["EditLanguagesDataSource"] = value; }
        }

        private delegate string Parser(string s);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAdd.Enabled = false;
                    btnSave.Enabled = false;
                }
                
                LoadStrings();
                PopulateDataGrid();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.editLanguages;
            base.OnInit(e);
        }

        private void LoadStrings()
        {
            btnAdd.Text = Lang.TransA("Add new language");
            btnSave.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");

            #region Set Grid's Headers
            dgLanguages.Columns[0].HeaderText = Lang.TransA("Language");
            dgLanguages.Columns[1].HeaderText = Lang.TransA("Active");
            dgLanguages.Columns[2].HeaderText = Lang.TransA("Order");
            dgLanguages.Columns[3].HeaderText = Lang.TransA("Commands");
            #endregion         
        }
        
        private void PopulateDataGrid()
        {
            DataTable dtLanguages = new DataTable("Languages");
            
            dtLanguages.Columns.Add("LanguageID");
            dtLanguages.Columns.Add("LanguageName");
            dtLanguages.Columns.Add("IsActive", typeof(bool));
            dtLanguages.Columns.Add("Predefined", typeof(bool));
            
            Language[] languages = Language.FetchAll();
            
            foreach (Language language in languages)
            {
                dtLanguages.Rows.Add(new object[]
                                         {
                                             language.Id,
                                             language.Name,
                                             language.Active,
                                             language.Predefined
                                         });
            }

            DataSource = dtLanguages;
            
            dgLanguages.DataSource = dtLanguages;
            dgLanguages.DataBind();
        }

        private void ChangeOrder(DataGridCommandEventArgs e, int languageId)
        {
            string direction = e.CommandName;

            switch (direction)
            {
                case "MoveUp":
                    Language.ChangeOrder(languageId, eDirections.Up);
                    break;
                case "MoveDown":
                    Language.ChangeOrder(languageId, eDirections.Down);
                    break;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            txtLanguageName.Text = "";
            cbActive.Checked = false;
            txtBrowserLanguages.Text = String.Empty;
            txtIpCountries.Text = String.Empty;
            txtTheme.Text = String.Empty;
            
            pnlLanguages.Visible = false;
            pnlLanguage.Visible = true;
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlLanguages.Visible = true;
            pnlLanguage.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (txtBrowserLanguages.Text.Trim().Length > 0)
            {
                foreach (string browserLanguage in txtBrowserLanguages.Text.Split(','))
                {
                    try
                    {
                        CultureInfo.CreateSpecificCulture(browserLanguage.ToLowerInvariant());
                    }
                    catch (ArgumentException)
                    {
                        MessageBox.Show(String.Format("{0} is not a valid browser language!", browserLanguage), Misc.MessageType.Error);
                        return;
                    }
                }
            }

            if (LanguageId == null)
            {
                Language language = Language.Create(txtLanguageName.Text, cbActive.Checked);
                language.BrowserLanguages = txtBrowserLanguages.Text.Trim().Length == 0
                                                ? null
                                                : txtBrowserLanguages.Text.Trim();
                language.IpCountries = txtIpCountries.Text.Trim().Length == 0 ? null : txtIpCountries.Text.Trim();
                language.Theme = txtTheme.Text.Trim().Length == 0 ? null : txtTheme.Text.Trim();
                language.Save();
            }
            else
            {
                Language language = Language.Fetch(LanguageId.Value);
                language.Name = txtLanguageName.Text;
                language.Active = cbActive.Checked;
                language.BrowserLanguages = txtBrowserLanguages.Text.Trim();
                language.IpCountries = txtIpCountries.Text.Trim();
                language.Theme = txtTheme.Text.Trim();
                language.Save();

                LanguageId = null;
            }

            PopulateDataGrid();
            pnlLanguage.Visible = false;
            pnlLanguages.Visible = true;
        }
        
        protected void dgLanguages_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int languageId = Convert.ToInt32(e.CommandArgument);
            
            if (e.CommandName == "EditLanguage")
            {
                Language language = Language.Fetch(languageId);
                pnlLanguage.Visible = true;
                pnlLanguages.Visible = false;
                txtLanguageName.Text = language.Name;
                cbActive.Checked = language.Active;
                txtBrowserLanguages.Text = language.BrowserLanguages;
                txtIpCountries.Text = language.IpCountries;
                txtTheme.Text = language.Theme;

                LanguageId = language.Id;
            }
            else if (e.CommandName == "DeleteLanguage")
            {
                if (!HasWriteAccess)
                    return;
                
                Language.Delete(Convert.ToInt32(e.CommandArgument));
                PopulateDataGrid();
            }
            else
            {
                if (!HasWriteAccess)
                    return;
                
                ChangeOrder(e, languageId);
                PopulateDataGrid();
            }
        }

        protected void dgLanguages_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkUp = e.Item.FindControl("lnkUp") as LinkButton;
            LinkButton lnkDown = e.Item.FindControl("lnkDown") as LinkButton;
            LinkButton lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;

            if (!HasWriteAccess)
            {
                if (lnkUp != null)
                    lnkUp.Enabled = false;

                if (lnkDown != null)
                    lnkDown.Enabled = false;

                if (lnkDelete != null)
                    lnkDelete.Enabled = false;
            }
        }

        protected void dgLanguages_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            //remove the upper arrow if the current item is the first one
            if (e.Item.ItemIndex == 0)
            {
                (e.Item.FindControl("lnkUp") as LinkButton).Visible = false;
            }

            //remove the lower arrow if the current item is the last one
            int lastItemIndex = DataSource.Rows.Count - 1;
            if (e.Item.ItemIndex == lastItemIndex)
                (e.Item.FindControl("lnkDown") as LinkButton).Visible = false;
        }

        private Parser parse = delegate(string text)
                                   {
                                       return String.Join(",", text.Split(','));
                                   };
    }
}
