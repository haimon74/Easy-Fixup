using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AjaxControlToolkit;

namespace AspNetDating.Admin
{
    public partial class EditPopupTips : AdminPageBase
    {
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "98%", "500px");
            //var ajaxFileUpload = htmlEditorExtender2.AjaxFileUpload;
            //ajaxFileUpload.AllowedFileTypes = "png,jpg,jpeg,gif";

            if (Request.QueryString["preview"] == "1" && !string.IsNullOrEmpty(Request.QueryString["fileId"]))
            {
                var fileId = Request.QueryString["fileId"];
                var fileContents = (byte[])Session["fileContents_" + fileId];
                var fileContentType = (string)Session["fileContentType_" + fileId];

                if (fileContents != null)
                {
                    Response.Clear();
                    Response.ContentType = fileContentType;
                    Response.BinaryWrite(fileContents);
                    Response.End();
                }
            }

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

            var items1 = new List<ListItem>();
            var items2 = new List<ListItem>();
            var items3 = new List<ListItem>();
            for (int i = 0; i <= 50; i++)
            {
                items1.Add(new ListItem((i).ToString()));
                items2.Add(new ListItem((i * 7).ToString()));
                items3.Add(new ListItem((i * 100).ToString()));
            }
            ddlCreditsTrigger.Items.AddRange(items3.ToArray());
            ddlTimesToShow.Items.AddRange(items1.ToArray());
            ddlDaysCountTrigger.Items.AddRange(items1.ToArray());
            ddlLoginCountTrigger.Items.AddRange(items1.ToArray());
            ddlNoSpendingDaysTrigger.Items.AddRange(items2.ToArray());
        }

        private void PopulateDDContentKeys(int languageID)
        {
            ddContentKey.Items.Clear();

            ddContentKey.Items.Add(new ListItem("", "-1"));
            ddContentKey.Items.Add(new ListItem("Add New Popup Tip", "0"));

            TipPopup[] tipPopups = TipPopup.FetchTipsByLanguage(languageID);

            foreach (TipPopup tp in tipPopups)
            {
                if (tp.ID.HasValue)
                    ddContentKey.Items.Add(new ListItem(tp.Key, tp.ID.Value.ToString()));
            }
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

            int id = Convert.ToInt32(ddContentKey.SelectedValue);

            if (id < 0) return;

            //string content = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
            string content = advancedHtmlEditor.Text;
            string key = txtKeyName.Text;
            int languageID = Convert.ToInt32(ddLanguage.SelectedItem.Value);
            int showCount = Convert.ToInt32(ddlTimesToShow.SelectedValue);
            int loginCount = Convert.ToInt32(ddlLoginCountTrigger.SelectedValue);
            int daysCount = Convert.ToInt32(ddlDaysCountTrigger.SelectedValue);
            int minCredit = Convert.ToInt32(ddlCreditsTrigger.SelectedValue);
            int noUseDaysCount = Convert.ToInt32(ddlNoSpendingDaysTrigger.SelectedValue);

            TipPopup tp = id == 0 ? new TipPopup(key, languageID)
                                   : TipPopup.Fetch(id);

            tp.Content = content;
            tp.DaysCountTrigger = daysCount;
            tp.IsMatchmakerTip = cbIsMatchmaker.Checked;
            tp.LoginCountTrigger = loginCount;
            tp.MinCreditsTrigger = minCredit;
            tp.NoSpendingDaysTrigger = noUseDaysCount;
            tp.ShowPageName = txtPagesNames.Text;
            tp.ShowTimes = showCount;
            tp.Title = txtTitle.Text;

            tp.Save();

            PopulateDDContentKeys(languageID);
            ddContentKey.SelectedValue = String.Format("{0}", tp.ID ?? -1);

            Messagebox1.Show("New Tip Saved Successfuly.", Misc.MessageType.Success);
        }

        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            divEdit.Visible = false;

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
                divEdit.Visible = false;
                return;
            }
            else if (ddContentKey.SelectedItem.Value == "0")
            {
                txtKeyName.Enabled = true;
                divEdit.Visible = true;
                txtPagesNames.Text = "";
                txtKeyName.Text = "";
                txtTitle.Text = "";
            }
            else
            {
                txtKeyName.Enabled = false;
                divEdit.Visible = true;
                string key = ddContentKey.SelectedItem.Text;
                TipPopup tp = TipPopup.Fetch(Int32.Parse(ddContentKey.SelectedValue));
                ddlCreditsTrigger.SelectedValue = tp.MinCreditsTrigger.ToString();
                ddlDaysCountTrigger.SelectedValue = tp.DaysCountTrigger.ToString();
                ddlLoginCountTrigger.SelectedValue = tp.LoginCountTrigger.ToString();
                ddlNoSpendingDaysTrigger.SelectedValue = tp.NoSpendingDaysTrigger.ToString();
                ddlTimesToShow.SelectedValue = tp.ShowTimes.ToString();
                txtPagesNames.Text = tp.ShowPageName;
                txtKeyName.Text = tp.Key;
                cbIsMatchmaker.Checked = tp.IsMatchmakerTip;
                txtTitle.Text = tp.Title;
                //if (ckeditor != null)
                //    ckeditor.Text = tp.Content;
                //else if (htmlEditor != null)
                //    htmlEditor.Content = tp.Content;
                advancedHtmlEditor.Text = tp.Content.Replace('\"', '\'').Replace("&lt;","<").Replace("&gt;",">");
            }
        }

        protected void ajaxUpload1_OnUploadComplete(object sender, AjaxFileUploadEventArgs e)
        {
            if (e.ContentType.Contains("jpg") || e.ContentType.Contains("gif")
            || e.ContentType.Contains("png") || e.ContentType.Contains("jpeg"))
            {
                Session["fileContentType_" + e.FileId] = e.ContentType;
                Session["fileContents_" + e.FileId] = e.GetContents();
            }

            // Set PostedUrl to preview the uploaded file.         
            e.PostedUrl = string.Format("?preview=1&fileId={0}", e.FileId);
            
            // Generate file path
            string filePath = "~/Images/" + e.FileName;
            var ajaxFileUpload = htmlEditorExtender2.AjaxFileUpload;
            //ajaxFileUpload.AllowedFileTypes = "jpg,jpeg,gif,png";
            // Save upload file to the file system
            ajaxFileUpload.SaveAs(MapPath(filePath));
            // Update client with saved image path
            e.PostedUrl = Page.ResolveUrl(filePath);
        }

        protected void ajaxFileUpload_OnUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            if (e.ContentType.Contains("jpg") || e.ContentType.Contains("gif")
                || e.ContentType.Contains("png") || e.ContentType.Contains("jpeg"))
            {
                Session["fileContentType_" + e.FileId] = e.ContentType;
                Session["fileContents_" + e.FileId] = e.GetContents();
            }

            // Set PostedUrl to preview the uploaded file.         
            e.PostedUrl = string.Format("?preview=1&fileId={0}", e.FileId);
        }    
    }
}
