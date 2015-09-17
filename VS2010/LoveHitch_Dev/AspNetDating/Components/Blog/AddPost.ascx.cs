using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Blog
{
    /// <summary>
    ///		Summary description for Settings.
    /// </summary>
    public partial class AddPostCtrl : UserControl
    {
        protected LargeBoxStart LargeBoxStart;
        protected HeaderLine hlBlogSettings;
        public event EventHandler SaveChangesClickEvent;
        public event EventHandler CancelClickEvent;
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;
        private PermissionCheckResult permissionCheckResult;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this.Page, phEditor, ref htmlEditor, ref ckeditor, "500px", "200px");
            lblError.Text = "";

            if (CurrentUserSession != null)
            {
                permissionCheckResult = CurrentUserSession.CanCreateBlogs();
                if (permissionCheckResult == PermissionCheckResult.YesWithCredits)
                {
                    btnSaveChanges.OnClientClick =
                        String.Format("return confirm(\"" + "Posting this blog post will subtract {0} credits from your balance.".Translate() + "\");",
                            CurrentUserSession.BillingPlanOptions.CanCreateBlogs.Credits);
                }
            }

            if (!Page.IsPostBack)
            {
                LoadStrings();
            }
        }

        public int BlogPostId
        {
            set { ViewState["AddPostCtrl_BlogPostId"] = value; }
            get
            {
                if (ViewState["AddPostCtrl_BlogPostId"] is int)
                    return (int) ViewState["AddPostCtrl_BlogPostId"];
                return -1;
            }
        }

        private UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        public void EditBlogPost(int blogPostId)
        {
            BlogPost blogPost;
            try
            {
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
            txtName.Text = blogPost.Title;
            if (ckeditor != null)
                ckeditor.Text = blogPost.Content;
            else if (htmlEditor != null)
                htmlEditor.Content = blogPost.Content;
            LargeBoxStart.Visible = false;
            btnSaveChanges.Text = Lang.Trans(" Update Post >> ");
            btnCancel.Visible = true;
        }

        private void LoadStrings()
        {
            LargeBoxStart.Title = Lang.Trans("Add Post");
            hlBlogSettings.Title = Lang.Trans("Post Details");
            btnSaveChanges.Text = Lang.Trans(" Add Post >> ");
            btnCancel.Text = Lang.Trans("Cancel");
        }

        private bool ValidateData()
        {
            lblError.CssClass = "error";

            if (txtName.Text.Trim().Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter post title!");
                return false;
            }

            string content = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;

            if (content.Trim().Length == 0)
            {
                lblError.Text = Lang.Trans("Please enter post text!");
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
            //if (((PageBase) Page).CurrentUserSession != null)
            //{
            //    try
            //    {
            //        string userFilesPath = "~/UserFiles/" + ((PageBase) Page).CurrentUserSession.Username;
            //        string userFilesDir = Server.MapPath(userFilesPath);
            //        if (!Directory.Exists(userFilesDir))
            //        {
            //            Directory.CreateDirectory(userFilesDir);
            //        }
            //    }
            //    catch (Exception err)
            //    {
            //        Global.Logger.LogError(err);
            //    }
            //}
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        #endregion

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                Classes.Blog blog = Classes.Blog.Load(CurrentUserSession.Username);

                if (permissionCheckResult == PermissionCheckResult.Yes ||
                    (CurrentUserSession.Level != null &&
                                       CurrentUserSession.Level.Restrictions.CanCreateBlogs))
                {
                }
                else if (permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded ||
                         permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded)
                {
                    Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanCreateBlogs;
                    Response.Redirect("~/ManageProfile.aspx?sel=payment");
                    return;
                }
                else if (permissionCheckResult == PermissionCheckResult.No)
                {
                    ((PageBase)Page).StatusPageMessage = Lang.Trans("You are not allowed to create blogs!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                BlogPost blogPost = null;
                string content = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
                if (BlogPostId == -1)
                {
                    //if (Config.Credits.Required && Config.Credits.CreditsForBlogPost > 0)
                    //{
                    #region Charge credits

                        //if (!Config.Users.FreeForFemales ||
                        //    CurrentUserSession.Gender != User.eGender.Female)
                        //{
                    if (permissionCheckResult == PermissionCheckResult.YesWithCredits)
                    {
                        int creditsLeft = CurrentUserSession.Credits - CurrentUserSession.BillingPlanOptions.CanCreateBlogs.Credits;

                        if (creditsLeft < 0)
                        {
                            Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanCreateBlogs;
                            Response.Redirect("~/ManageProfile.aspx?sel=payment");
                            return;
                        }

                        var user = Classes.User.Load(CurrentUserSession.Username);
                        user.Credits -= CurrentUserSession.BillingPlanOptions.CanCreateBlogs.Credits;
                        user.Update(true);
                        CurrentUserSession.Credits = user.Credits;
                    }       
                        //}

                    #endregion
                    //}

                    if (Config.Misc.EnableBadWordsFilterBlogs)
                    {
                        blogPost = BlogPost.Create(blog.Id, Parsers.ProcessBadWords(txtName.Text.Trim()),
                                                   Parsers.ProcessBadWords(content.Trim()));
                    }
                    else
                    {
                        blogPost = BlogPost.Create(blog.Id, txtName.Text.Trim(), content.Trim());    
                    }
                }
                else
                {
                    blogPost = BlogPost.Load(BlogPostId);

                    if (Config.Misc.EnableBadWordsFilterBlogs)
                    {
                        blogPost.Title = Parsers.ProcessBadWords(txtName.Text.Trim());
                        blogPost.Content = Parsers.ProcessBadWords(content.Trim());
                    }
                    else
                    {
                        blogPost.Title = txtName.Text.Trim();
                        blogPost.Content = content.Trim();    
                    }
                }
                blogPost.Save();

                if (BlogPostId == -1 && blogPost.Approved)
                {
                    #region Add NewFriendBlogPost Event

                    Event newEvent = new Event(CurrentUserSession.Username);

                    newEvent.Type = Event.eType.NewFriendBlogPost;
                    NewFriendBlogPost newFriendBlogPost = new NewFriendBlogPost();
                    newFriendBlogPost.BlogPostID = blogPost.Id;
                    newEvent.DetailsXML = Misc.ToXml(newFriendBlogPost);

                    newEvent.Save();

                    string[] usernames = User.FetchMutuallyFriends(CurrentUserSession.Username);

                    foreach (string friendUsername in usernames)
                    {
                        if (Config.Users.NewEventNotification)
                        {
                            string text = String.Format("Your friend {0} has a new blog post: {1}".Translate(),
                                                    "<b>" + CurrentUserSession.Username + "</b>",
                                                    Server.HtmlEncode(blogPost.Title));
                            int imageID = 0;
                            try
                            {
                                imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                            }
                            catch (NotFoundException)
                            {
                                imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                            }
                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                            Classes.User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername,
                                                                     text, thumbnailUrl,
                                                                     UrlRewrite.CreateShowUserBlogUrl(
                                                                         CurrentUserSession.Username, blogPost.Id));
                        }
                    }

                    #endregion
                }

                txtName.Text = "";
                if (ckeditor != null)
                    ckeditor.Text = "";
                else if (htmlEditor != null)
                    htmlEditor.Content = "";
                if (BlogPostId != -1)
                {
                    BlogPostId = -1;
                    lblError.Text = Lang.Trans("Post has been edited successfully.");
                    Visible = false;
                    OnSaveChangesClick(e);
                }
                else
                {
                    lblError.Text = Lang.Trans("Post has been added successfully.");
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Visible = false;
            OnCancelClick(e);
        }

        protected virtual void OnSaveChangesClick(EventArgs e)
        {
            if (SaveChangesClickEvent != null)
            {
                SaveChangesClickEvent(this, e);
            }
        }


        protected virtual void OnCancelClick(EventArgs e)
        {
            if (CancelClickEvent != null)
            {
                CancelClickEvent(this, e);
            }
        }           
    }
}