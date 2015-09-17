using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    public partial class ApproveBlogPosts : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.blogPostApproval;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Misc.EnableBlogPostApproval)
                {
                    StatusPageMessage = Lang.TransA("Blog post approval option is not currently switched on!\n You can do this from Settings at \"User Management\" section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                if (!HasWriteAccess)
                {

                }
                LoadStrings();
                PopulateDropDown();
                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            lblBlogPostsPerPage.Text = Lang.TransA("Blog posts per page");
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                dropBlogPostsPerPage.Items.Add(i.ToString());
            dropBlogPostsPerPage.SelectedValue = Config.AdminSettings.ApproveBlogPosts.BlogPostsPerPage.ToString();
        }

        private void PopulateDataGrid()
        {
            dgPendingApproval.PageSize = Convert.ToInt32(dropBlogPostsPerPage.SelectedValue);

            BlogPost[] blogPosts = BlogPost.Fetch(-1, false);

            if (blogPosts.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no blog posts waiting for approval!"), Misc.MessageType.Error);
                dgPendingApproval.Visible = false;
                lblBlogPostsPerPage.Visible = false;
                dropBlogPostsPerPage.Visible = false;
            }
            else
            {
                bindBlogPosts(blogPosts);

                //lblMessage.Visible = false;
                dgPendingApproval.Visible = true;
                lblBlogPostsPerPage.Visible = true;
                dropBlogPostsPerPage.Visible = true;
            }
        }

        private void bindBlogPosts(BlogPost[] blogPosts)
        {
            dgPendingApproval.Columns[0].HeaderText = Lang.TransA("Username");
            dgPendingApproval.Columns[1].HeaderText = Lang.TransA("Title");
            dgPendingApproval.Columns[2].HeaderText = Lang.TransA("Content");

            DataTable dtAnswers = new DataTable("BlogPosts");
            dtAnswers.Columns.Add("ID");
            dtAnswers.Columns.Add("Username");
            dtAnswers.Columns.Add("Title");
            dtAnswers.Columns.Add("Content");
            dtAnswers.Columns.Add("DatePosted");

            foreach (BlogPost blogPost in blogPosts)
            {
                Blog blog = Blog.Load(blogPost.BlogId);
                if (blog == null) continue;

                dtAnswers.Rows.Add(new object[]
                                       {
                                           blogPost.Id,
                                           blog.Username,
                                           blogPost.Title,
                                           blogPost.Content.Length > 300
                                               ? Server.HtmlEncode(blogPost.Content.Substring(0, 300)) + "..."
                                               : Server.HtmlEncode(blogPost.Content),
                                           blogPost.DatePosted
                                       }
                    );
            }

            dgPendingApproval.DataSource = dtAnswers;
            try
            {
                dgPendingApproval.DataBind();
            }
            catch (HttpException)
            {
                dgPendingApproval.CurrentPageIndex = 0;
                dgPendingApproval.DataBind();
            }
        }

        protected void dgPendingApproval_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (e.CommandName == "Approve")
            {
                string[] parameters = ((string)e.CommandArgument).Split(':');
                if (parameters.Length == 2)
                {
                    string username = parameters[0];
                    int id = Convert.ToInt32(parameters[1]);
                    BlogPost blogPost = null;
                    try
                    {
                        blogPost = BlogPost.Load(id);
                    }
                    catch (NotFoundException) { return; }
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

                    PopulateDataGrid();
                }
            }
            else if (e.CommandName == "Reject")
            {
                string[] parameters = ((string)e.CommandArgument).Split(':');
                if (parameters.Length == 2)
                {
                    BlogPost.Delete(Convert.ToInt32(parameters[1]));
                    PopulateDataGrid();
                }
            }
        }

        protected void dgPendingApproval_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkReject = (LinkButton)e.Item.FindControl("lnkReject");

            lnkReject.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to reject this blog post?")));
        }

        protected void dgPendingApproval_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkApprove = (LinkButton)e.Item.FindControl("lnkApprove");
            LinkButton lnkReject = (LinkButton)e.Item.FindControl("lnkReject");

            if (!HasWriteAccess)
            {
                lnkApprove.Enabled = false;
                lnkReject.Enabled = false;
            }
        }

        protected void dgPendingApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPendingApproval.CurrentPageIndex = e.NewPageIndex;
            PopulateDataGrid();
        }

        protected void dropBlogPostsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPendingApproval.PageSize = Convert.ToInt32(dropBlogPostsPerPage.SelectedValue);
            dgPendingApproval.CurrentPageIndex = 0;
            PopulateDataGrid();
        }
    }
}
