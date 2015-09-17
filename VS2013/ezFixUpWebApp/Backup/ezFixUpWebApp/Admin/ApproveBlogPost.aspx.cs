using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class ApproveBlogPost : AdminPageBase
    {
        private int id;
        private string username;

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.blogPostApproval;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            username = Request.Params["uid"];
            if (username == null)
                return;

            try
            {
                id = Convert.ToInt32(Request.Params["bpid"]);
            }
            catch (Exception)
            {
                return;
            }

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnReject.Enabled = false;
                    btnSaveAndApprove.Enabled = false;
                }

                LoadStrings();

                btnReject.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to reject this blog post?")));

                LoadPage();
            }
        }

        private void LoadStrings()
        {
            btnSaveAndApprove.Text = Lang.TransA("Save and Approve");
            btnReject.Text = Lang.TransA("Reject");
            btnCancel.Text = Lang.TransA("Cancel");
        }

        private void LoadPage()
        {
            BlogPost blogPost = null;

            try
            {
                blogPost = BlogPost.Load(id);
            }
            catch (NotFoundException)
            {
                return;
            }

            Blog blog = Blog.Load(blogPost.BlogId);

            if (blog != null)
            {
                lblUsername.Text = blog.Username;
                txtTitle.Text = blogPost.Title;
                txtContent.Text = blogPost.Content;
            }
        }

        protected void btnSaveAndApprove_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            BlogPost blogPost = null;

            try
            {
                blogPost = BlogPost.Load(id);
            }
            catch (NotFoundException)
            {
                return;
            }

            if (txtTitle.Text.Trim().Length == 0 || txtContent.Text.Trim().Length == 0)
                btnReject_Click(null, null);
            else
            {
                blogPost.Title = txtTitle.Text.Trim();
                blogPost.Content = txtContent.Text.Trim();
            }
            blogPost.Approved = true;
            blogPost.Save();

            #region Add NewFriendBlogPost Event

            Event newEvent = new Event(username);

            newEvent.Type = Event.eType.NewFriendBlogPost;
            NewFriendBlogPost newFriendBlogPost = new NewFriendBlogPost();
            newFriendBlogPost.BlogPostID = blogPost.Id;
            newEvent.DetailsXML = Misc.ToXml(newFriendBlogPost);

            newEvent.Save();

            string[] usernames = Classes.User.FetchMutuallyFriends(username);

            foreach (string friendUsername in usernames)
            {
                if (Config.Users.NewEventNotification)
                {
                    string text = String.Format("Your friend {0} has a new blog post: {1}".TranslateA(),
                                            "<b>" + username + "</b>",
                                            Server.HtmlEncode(blogPost.Title));
                    int imageID = 0;
                    try
                    {
                        imageID = Photo.GetPrimary(username).Id;
                    }
                    catch (NotFoundException)
                    {
                        Classes.User user = null;
                        try
                        {
                            user = Classes.User.Load(username);
                            imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                        }
                        catch (NotFoundException)
                        {
                            continue;
                        }
                    }
                    string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                    Classes.User.SendOnlineEventNotification(username, friendUsername,
                                                             text, thumbnailUrl,
                                                             UrlRewrite.CreateShowUserBlogUrl(
                                                                 username, blogPost.Id));
                }
            }

            #endregion

            Response.Redirect("ApproveBlogPosts.aspx");
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            BlogPost.Delete(id);
            Response.Redirect("ApproveBlogPosts.aspx");
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ApproveBlogPosts.aspx");
        }
    }
}
