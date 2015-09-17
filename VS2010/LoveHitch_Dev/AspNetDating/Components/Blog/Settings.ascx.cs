using System;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Blog
{
    /// <summary>
    ///		Summary description for Settings.
    /// </summary>
    public partial class SettingsCtrl : UserControl
    {
        protected LargeBoxStart LargeBoxStart;
        protected HeaderLine hlBlogSettings;


        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";

            if (!Page.IsPostBack)
            {
                LoadStrings();
                PopulateControls();
            }
        }

        private void LoadStrings()
        {
            LargeBoxStart.Title = Lang.Trans("Settings");
            hlBlogSettings.Title = Lang.Trans("Blog Settings");
            btnSaveChanges.Text = Lang.Trans(" Save Changes >> ");
        }

        private void PopulateControls()
        {
            Classes.Blog blog = Classes.Blog.Load(((PageBase) Page).CurrentUserSession.Username);
            txtName.Text = blog.Name;
            txtDescription.Text = blog.Description;
        }

        private bool ValidateData()
        {
            lblError.CssClass = "error";

            if (txtName.Text.Trim().Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter blog name!");
                return false;
            }

            if (txtDescription.Text.Trim().Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter blog description!");
                return false;
            }

            return true;
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

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Classes.Blog blog = Classes.Blog.Load(((PageBase) Page).CurrentUserSession.Username);
                if (Config.Misc.EnableBadWordsFilterBlogs)
                {
                    blog.Name = Parsers.ProcessBadWords(txtName.Text.Trim());
                    blog.Description = Parsers.ProcessBadWords(txtDescription.Text.Trim());
                }
                else
                {
                    blog.Name = txtName.Text.Trim();
                    blog.Description = txtDescription.Text.Trim();    
                }
                blog.Save();
            }
        }
    }
}