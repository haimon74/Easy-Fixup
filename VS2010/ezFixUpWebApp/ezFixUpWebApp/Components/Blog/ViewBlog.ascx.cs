using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Blog
{
    /// <summary>
    ///		Summary description for Settings.
    /// </summary>
    public partial class ViewBlogCtrl : UserControl
    {
        protected AddPostCtrl AddPost1;
        private Classes.Blog currentBlog;
        protected HeaderLine hlBlog;
        protected HeaderLine hlUserComments;
        protected LargeBoxStart LargeBoxStart;
        private User user;

        public int BlogId
        {
            set { ViewState["ViewBlog_BlogId"] = value; }
            get
            {
                if (ViewState["ViewBlog_BlogId"] == null && username != null)
                {
                    Classes.Blog blog = Classes.Blog.Load(username);
                    if (blog != null)
                        BlogId = blog.Id;
                }

                if (ViewState["ViewBlog_BlogId"] is int)
                    return (int) ViewState["ViewBlog_BlogId"];
                return -1;
            }
        }

        public int BlogPostId
        {
            set { ViewState["ViewBlog_BlogPostId"] = value; }
            get
            {
                if (ViewState["ViewBlog_BlogPostId"] is int)
                    return (int) ViewState["ViewBlog_BlogPostId"];
                return -1;
            }
        }

        public Classes.Blog CurrentBlog
        {
            get
            {
                if (currentBlog == null && BlogId != -1)
                    currentBlog = Classes.Blog.Load(BlogId);
                return currentBlog;
            }
        }

        public UserSession CurrentUserSession
        {
            get { return ((PageBase) Page).CurrentUserSession; }
        }

        public string username
        {
            set { ViewState["ViewBlog_Username"] = value; }
            get { return ViewState["ViewBlog_Username"] as string; }
        }

        public string Username
        {
            set { username = value; }
        }

        public User User
        {
            set
            {
                user = value;
                if (user != null) username = user.Username;
            }
            get
            {
                if (user == null && username != null)
                    user = User.Load(username);
                return user;
            }
        }

        private bool FirstLoad
        {
            get { return ViewState["FirstLoad"] == null ? true : false; }
            set { ViewState["FirstLoad"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            AddPost1.SaveChangesClickEvent += AddPost1_SaveChangesClickEvent;
            AddPost1.CancelClickEvent += AddPost1_CancelClickEvent;
        }

        private void AddPost1_CancelClickEvent(object sender, EventArgs e)
        {
            ShowBlogAndReloadPosts();
        }

        private void AddPost1_SaveChangesClickEvent(object sender, EventArgs e)
        {
            ShowBlogAndReloadPosts();
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (Config.Misc.EnableBlogs && BlogId != -1)
            {
                if (FirstLoad)
                {
                    LoadStrings();
                }
                LoadBlogPosts(false);
            }

            if (FirstLoad && !string.IsNullOrEmpty(Request.Params["bpid"]))
            {
                int blogPostId;
                if (int.TryParse(Request.Params["bpid"], out blogPostId))
                    LoadBlogPost(blogPostId);
            }

            FirstLoad = false;

            pnlBlogComments.Visible = !User.IsOptionEnabled(eUserOptions.DisableBlogComments);
            base.OnPreRender(e);
        }

        private void LoadStrings()
        {
            if (CurrentBlog != null)
            {
                hlBlog.Title = Server.HtmlEncode(CurrentBlog.Description);
                LargeBoxStart.Title = Server.HtmlEncode(CurrentBlog.Name);
                lnkBackToBlog.Text = Lang.Trans("<< Back to blog");
                lnkBackToBlog2.Text = Lang.Trans("<< Back to blog");
                lnkEditPost.Text = Lang.Trans("Edit Post");
                lnkDeletePost.Text = Lang.Trans("Delete Post");
                lnkDeletePost.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}');",
                                                                      Lang.Trans(
                                                                          "Do you really want to delete this post?")));

                if (CurrentUserSession != null && CurrentUserSession.Username == CurrentBlog.Username)
                {
                    divManagePost.Visible = true;
                }

                btnSubmitNewComment.Text = Lang.Trans("Submit Comment");
                hlUserComments.Title = Lang.Trans("User Comments");
            }
        }

        private void LoadBlogPosts(bool reload)
        {
            if (!(Page is MemberBlog) &&
                rptBlogPosts.Items.Count > 0 && !reload) return;

            var dtBlogPosts = new DataTable();
            dtBlogPosts.Columns.Add("Id", typeof (int));
            dtBlogPosts.Columns.Add("Title", typeof (string));
            dtBlogPosts.Columns.Add("Content", typeof (string));
            dtBlogPosts.Columns.Add("DatePosted", typeof (DateTime));

            BlogPost[] blogPosts = null;

            if (CurrentUserSession != null && CurrentUserSession.Username == CurrentBlog.Username)
                blogPosts = BlogPost.Fetch(BlogId);
            else blogPosts = BlogPost.Fetch(BlogId, true);

            foreach (BlogPost blogPost in blogPosts)
            {
                blogPost.Title = Server.HtmlEncode(blogPost.Title);
                blogPost.Content = stripHtml(blogPost.Content);
                if (blogPost.Content.Length > 300)
                {
                    blogPost.Content = blogPost.Content.Substring(0, 300) + "...";
                }

                dtBlogPosts.Rows.Add(new object[]
                                         {
                                             blogPost.Id, blogPost.Title, blogPost.Content,
                                             blogPost.DatePosted
                                         });
            }

            rptBlogPosts.DataSource = dtBlogPosts;
            rptBlogPosts.DataBind();
        }

        protected static string stripHtml(string strHtml)
        {
            //Strips the HTML tags from strHTML 
            var objRegExp
                = new Regex("<(.|\n)+?>");

            // Replace all tags with a space, otherwise words either side 
            // of a tag might be concatenated 
            string strOutput = objRegExp.Replace(strHtml, " ");

            // Replace all < and > with < and > 
            strOutput = strOutput.Replace("<", "<");
            strOutput = strOutput.Replace(">", ">");

            return strOutput;
        }

        protected static string stripDangerousHtml(string strHtml)
        {
            //Strips the HTML tags from strHTML 
            var objRegExp
                = new Regex("<(script|object|form|input|textbox|select)(.|\n)+?>", RegexOptions.IgnoreCase);

            // Replace all tags with a space, otherwise words either side 
            // of a tag might be concatenated 
            string strOutput = objRegExp.Replace(strHtml, " ");

            return strOutput;
        }

        private void rptBlogPosts_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewPost")
            {
                BlogPost blogPost;
                try
                {
                    int blogPostId = Convert.ToInt32(e.CommandArgument);
                    if (CurrentUserSession != null && CurrentUserSession.Username != CurrentBlog.Username)
                    {
                        BlogPost.IncreaseReadCounter(blogPostId);
                    }

                    BlogPostId = blogPostId;
                    blogPost = BlogPost.Load(blogPostId);
                }
                catch (NotFoundException)
                {
                    return;
                }
                catch (ArgumentException)
                {
                    return;
                }

                LoadBlogPost(blogPost);
                LoadComments(blogPost.Id);
            }
        }

        public void LoadBlogPost(int blogPostId)
        {
            BlogPost blogPost;
            try
            {
                if (CurrentUserSession != null && CurrentUserSession.Username != CurrentBlog.Username)
                {
                    BlogPost.IncreaseReadCounter(blogPostId);
                }

                BlogPostId = blogPostId;
                blogPost = BlogPost.Load(blogPostId);
            }
            catch (NotFoundException)
            {
                return;
            }
            catch (ArgumentException)
            {
                return;
            }

            LoadBlogPost(blogPost);
            LoadComments(blogPost.Id);
        }

        private void LoadBlogPost(BlogPost blogPost)
        {
            lblDate.Text = blogPost.DatePosted.ToLongDateString();
            lblTitle.Text = Server.HtmlEncode(blogPost.Title);
            lblContent.Text = stripDangerousHtml(blogPost.Content);
            lblDirectLink.Text = UrlRewrite.CreateShowUserBlogUrl(CurrentBlog.Username, BlogPostId);
            divViewPost.Visible = true;
            divViewBlog.Visible = false;
        }

        private void LoadComments(int blogPostId)
        {
            var dtComments = new DataTable();
            dtComments.Columns.Add("Id", typeof (int));
            dtComments.Columns.Add("DatePosted", typeof (DateTime));
            dtComments.Columns.Add("Username", typeof (string));
            dtComments.Columns.Add("CommentText", typeof (string));
            dtComments.Columns.Add("CanDelete", typeof (bool));

            BlogPostComment[] comments = BlogPostComment.Fetch(blogPostId);

            if (CurrentUserSession != null)
                ShowHideComments();
            else
                spanAddNewComment.Visible = false;

            Classes.Blog blog = Classes.Blog.Load(BlogId);

            int commentsFromCurrentUser = 0;
            foreach (BlogPostComment comment in comments)
            {
                bool canDelete = false;
                if (CurrentUserSession != null)
                {
                    if (comment.Username == CurrentUserSession.Username)
                    {
                        if (DateTime.Now.Subtract(comment.DatePosted) < TimeSpan.FromHours(1))
                        {
                            spanAddNewComment.Visible = false;
                        }
                        canDelete = true;
                        commentsFromCurrentUser++;
                        if (commentsFromCurrentUser > 10)
                        {
                            spanAddNewComment.Visible = false;
                        }
                    }

                    if (blog.Username == CurrentUserSession.Username)
                    {
                        canDelete = true;
                        spanAddNewComment.Visible = true;
                    }
                }

                dtComments.Rows.Add(new object[]
                                        {
                                            comment.Id, comment.DatePosted, comment.Username,
                                            Server.HtmlEncode(comment.CommentText), canDelete
                                        });
            }

            rptComments.DataSource = dtComments;
            rptComments.DataBind();
        }

        private void ShowHideComments()
        {
            Classes.Blog blog = Classes.Blog.Load(BlogId);
            if (User.IsUserBlocked(blog.Username, CurrentUserSession.Username) || !CanAddComments())
                spanAddNewComment.Visible = false;
            else
                spanAddNewComment.Visible = true;
        }

        private bool CanAddComments()
        {
            if (ViewState["CanAddComments"] == null)
            {
                ViewState["CanAddComments"] =
                    (CurrentUserSession.CanAddComments() == PermissionCheckResult.Yes ||
                                          (CurrentUserSession.Level != null &&
                                           CurrentUserSession.Level.Restrictions.UserCanAddComments)) &&
                    BlogPostComment.Fetch(BlogPostId, CurrentUserSession.Username).Length < Config.Users.MaxComments;
            }

            return (bool)ViewState["CanAddComments"];
        }

        protected void btnSubmitNewComment_Click(object sender, EventArgs e)
        {
            if (txtNewComment.Text.Trim() == "")
            {
                return;
            }

            if (CurrentUserSession != null)
            {
                string commentText = Config.Misc.EnableBadWordsFilterBlogs
                                         ? Parsers.ProcessBadWords(txtNewComment.Text)
                                         : txtNewComment.Text;
                BlogPostComment comment = BlogPostComment.Create(BlogPostId, CurrentUserSession.Username, commentText);

                comment.Save();
            }

            LoadComments(BlogPostId);
        }

        private void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                BlogPostComment comment;
                try
                {
                    comment = BlogPostComment.Load(commentId);
                }
                catch (NotFoundException)
                {
                    return;
                }

                Classes.Blog blog = Classes.Blog.Load(BlogId);
                if (CurrentUserSession != null && (comment.Username == CurrentUserSession.Username
                        || blog.Username == CurrentUserSession.Username))
                {
                    BlogPostComment.Delete(commentId);
                }

                LoadComments(BlogPostId);
            }
        }

        private void rptComments_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            var lnkDeleteComment = (LinkButton) e.Item.FindControl("lnkDeleteComment");
            lnkDeleteComment.Attributes.Add("onclick",
                                            String.Format("javascript: return confirm('{0}')",
                                                          Lang.Trans("Do you really want to remove this comment?")));
        }

        protected void lnkBackToBlog_Click(object sender, EventArgs e)
        {
            lblDate.Text = "";
            lblTitle.Text = "";
            lblContent.Text = "";
            divViewBlog.Visible = true;
            divViewPost.Visible = false;
        }

        protected void lnkEditPost_Click(object sender, EventArgs e)
        {
            BlogPost blogPost;
            try
            {
                blogPost = BlogPost.Load(BlogPostId);
            }
            catch (NotFoundException)
            {
                return;
            }
            catch (ArgumentException)
            {
                return;
            }
            if (blogPost.BlogId != BlogId)
            {
                return;
            }

            AddPost1.EditBlogPost(BlogPostId);
            divViewPost.Visible = false;
            AddPost1.Visible = true;
        }

        protected void lnkDeletePost_Click(object sender, EventArgs e)
        {
            BlogPost blogPost;
            try
            {
                blogPost = BlogPost.Load(BlogPostId);
            }
            catch (NotFoundException)
            {
                return;
            }
            catch (ArgumentException)
            {
                return;
            }
            if (blogPost.BlogId != BlogId)
            {
                return;
            }

            BlogPost.Delete(BlogPostId);

            ShowBlogAndReloadPosts();
        }

        public void ShowBlogAndReloadPosts()
        {
            lnkBackToBlog_Click(null, null);
            divViewBlog.Visible = true;
            LoadBlogPosts(true);
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
            this.rptBlogPosts.ItemCommand +=
                new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptBlogPosts_ItemCommand);
            this.rptComments.ItemCommand +=
                new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rptComments_ItemCommand);
            this.rptComments.ItemCreated += new RepeaterItemEventHandler(rptComments_ItemCreated);
        }

        #endregion
    }
}