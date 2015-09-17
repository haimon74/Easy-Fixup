/* ASPnetDating 
 * Copyright (C) 2003-2009 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using News = AspNetDating.Classes.News;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditNews.
    /// </summary>
    public partial class EditNews : AdminPageBase
    {
        protected MessageBox MessageBox;
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;


        int? EditedNewsID
        {
            get
            {
                return ViewState["EditedNewsID"] as int?;
            }

            set{
                ViewState["EditedNewsID"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "500px", "200px");

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnSave.Enabled = false;
                }

                LoadStrings();
                PopulateDropDown();
                if (ddLanguage.SelectedItem.Value != "-1")
                {
                    PopulateDataGrid();
                }
            }
        }

        private void LoadStrings()
        {
            dgNews.Columns[0].HeaderText = Lang.TransA("News Title");
            dgNews.Columns[1].HeaderText = Lang.TransA("Publish Date");

            btnAddNews.Text = Lang.TransA("Add news");
            btnSave.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void PopulateDataGrid()
        {
            DataTable dtNews = new DataTable("News");
            dtNews.Columns.Add("NID");
            dtNews.Columns.Add("LanguageId");
            dtNews.Columns.Add("Text");
            dtNews.Columns.Add("Date", typeof(DateTime));
            dtNews.Columns.Add("Title");

            int language = Convert.ToInt32(ddLanguage.SelectedItem.Value);
            if (language > 0)
            {
                News[] newsArr = Classes.News.FetchAsArray(language);

                if (newsArr.Length > 0)
                {
                    foreach (News news in newsArr)
                        dtNews.Rows.Add(
                            new object[] { news.ID.ToString(), news.LanguageId, news.Text, news.PublishDate, news.Title });
                }
            }


            dgNews.DataSource = dtNews;
            dgNews.DataBind();

            if (dtNews.Rows.Count == 0)
            {
                MessageBox.Show(
                    Lang.TransA("There are no existing news! Please click on \"Add news\" to create new ones."),
                    Misc.MessageType.Error);
                dgNews.Visible = false;
            }
            else
            {
                dgNews.Visible = true;
            }
        }

        private void PopulateDropDown()
        {
            ddLanguage.Items.Add(new ListItem("", "-1"));

            foreach (Language language in Language.FetchAll())
            {
                ddLanguage.Items.Add(new ListItem(language.Name, language.Id.ToString()));
            }

            if (ddLanguage.Items.Count <= 2)
            {
                if (ddLanguage.Items.Count == 2)
                    ddLanguage.SelectedIndex = 1;
                pnlLanguage.Visible = false;
                ddLanguage_SelectedIndexChanged(this, null);
            }
            else
                pnlLanguage.Visible = true;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.news;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void btnAddNews_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            pnlAddEditNews.Visible = true;
            EditedNewsID = null;

            Calendar.SelectedDate = DateTime.Now;
            
            if (htmlEditor != null)
                htmlEditor.Content = String.Empty;
            if (ckeditor != null)
                ckeditor.Text = String.Empty;

            txtNewsTitle.Text = String.Empty;

            btnAddNews.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            News news;
            int language = Convert.ToInt32(ddLanguage.SelectedItem.Value);

            if (EditedNewsID.HasValue)

                news = new News(EditedNewsID.Value, language);
            else
                news = new News();

            news.LanguageId = language;
            //if (htmlEditor != null)
            news.Text = txtEditor.Text;// htmlEditor.Content;
            //if (ckeditor != null)
            //    news.Text = ckeditor.Text;
            news.PublishDate = DateTime.Now;// Calendar.SelectedDate;
            news.Title = txtNewsTitle.Text;
            news.Save();
            txtNewsTitle.Text = String.Empty;
            txtEditor.Text = String.Empty;

            MessageBox.Show(Lang.TransA("News saved successfully!"), Misc.MessageType.Success);
            PopulateDataGrid();
            pnlAddEditNews.Visible = false;
            btnAddNews.Visible = true;
        }

        protected void ddLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAddEditNews.Visible = false;
            if (ddLanguage.SelectedItem.Value == "-1")
            {
                divNews.Visible = false;
            }
            else
            {
                divNews.Visible = true;
                PopulateDataGrid();
            }
        }

        protected void dgNews_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int newsID = Int32.Parse((string)e.CommandArgument);
                Classes.News.Delete(newsID);
                PopulateDataGrid();
                pnlAddEditNews.Visible = false;
                btnAddNews.Visible = true;
            }
            else if (e.CommandName == "Edit")
            {
                if (!HasWriteAccess)
                    return;

                pnlAddEditNews.Visible = true;
                btnAddNews.Visible = false;

                int newsID = Int32.Parse((string)e.CommandArgument);
                var news = Classes.News.Fetch(newsID, Int32.Parse(ddLanguage.SelectedValue));

                Calendar.SelectedDate = news.PublishDate;
                
                if (htmlEditor != null)
                    htmlEditor.Content = news.Text;
                if (ckeditor != null)
                    ckeditor.Text = news.Text;

                txtNewsTitle.Text = news.Title;
                EditedNewsID = news.ID;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlAddEditNews.Visible = false;
            btnAddNews.Visible = true;
        }

        protected void dgNews_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            var lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;

            if (lnkDelete != null)
            {
                lnkDelete.Attributes.Add("onclick",
                                                     String.Format("javascript: return confirm('{0}')",
                                                                   Lang.TransA("Do you really want to delete selected news?")));

                if (!HasWriteAccess)
                {
                    lnkDelete.Enabled = false;
                }
            }
        }
    }
}