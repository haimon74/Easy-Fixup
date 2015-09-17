using System;
using ezFixUp.Classes;
using ezFixUp.Components;
using ezFixUp.Components.Blog;

namespace ezFixUp
{
    public partial class MemberBlog : PageBase
    {

        protected SmallBoxStart SmallBoxStart1;
        protected SettingsCtrl Settings;
        protected AddPostCtrl AddPost;
        protected ViewBlogCtrl ViewBlog;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                CheckPermissions();

                switch (Request.Params["sel"])
                {
                    default:
                        lnkNewPost_Click(null, null);
                        break;
                }
            }

            LoadStrings();
            LoadData();
        }

        private void CheckPermissions()
        {
            var permissionCheckResult = CurrentUserSession.CanCreateBlogs();

            if (permissionCheckResult == PermissionCheckResult.Yes ||
                (CurrentUserSession.Level != null &&
                                   CurrentUserSession.Level.Restrictions.CanCreateBlogs))
            {
            }
            else if (permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                     permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
            {
                Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanCreateBlogs;
                Response.Redirect("~/ManageProfile.aspx?sel=payment");
                return;
            }
            else if (permissionCheckResult == PermissionCheckResult.No)
            {
                StatusPageMessage = Lang.Trans("You are not allowed to create blogs!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        private void LoadStrings()
        {
            lnkEditBlog.Text = Lang.Trans("Blog Settings");
            lnkNewPost.Text = Lang.Trans("New Post");
            lnkViewBlog.Text = Lang.Trans("View Blog");

            if (SmallBoxStart1 != null)
                SmallBoxStart1.Title = Lang.Trans("Blog Management");
        }

        private void LoadData()
        {
            Blog blog = Blog.Load(CurrentUserSession.Username);
            if (blog == null)
            {
                blog = Blog.Create(CurrentUserSession.Username);
                blog.Name = String.Format(Lang.Trans("{0}'s blog"), CurrentUserSession.Username);
                blog.Description = String.Format(Lang.Trans("My blog in {0}"), Config.Misc.SiteTitle);
                blog.Save();
            }

            ViewBlog.BlogId = blog.Id;
            ViewBlog.User = CurrentUserSession;
        }

        private void EnableSideLinks()
        {
            lnkEditBlog.Enabled = true;
            lnkNewPost.Enabled = true;
            lnkViewBlog.Enabled = true;
        }

        private void HideControls()
        {
            Settings.Visible = false;
            AddPost.Visible = false;
            ViewBlog.Visible = false;
        }

        protected void lnkEditBlog_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkEditBlog.Enabled = false;

            HideControls();
            Settings.Visible = true;
        }

        protected void lnkNewPost_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkNewPost.Enabled = false;

            HideControls();
            AddPost.Visible = true;
        }

        protected void lnkViewBlog_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkViewBlog.Enabled = false;

            HideControls();
            ViewBlog.Visible = true;
        }
    }
}