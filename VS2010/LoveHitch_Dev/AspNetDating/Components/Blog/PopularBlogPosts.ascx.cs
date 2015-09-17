using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AspNetDating.Classes;

namespace AspNetDating.Components.Blog
{
    public partial class PopularBlogPosts : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadNewBlogs();
            }
        }

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Popular Blog Posts");
        }

        private void loadNewBlogs()
        {
            DataTable dtBlogs = new DataTable("PopularBlogPosts");

            dtBlogs.Columns.Add("BlogID", typeof (int));
            dtBlogs.Columns.Add("BlogName");
            dtBlogs.Columns.Add("BlogPostID", typeof(int));
            dtBlogs.Columns.Add("BlogPostTitle");
            dtBlogs.Columns.Add("DatePosted");
            dtBlogs.Columns.Add("Username");
            dtBlogs.Columns.Add("ImageID", typeof (int));

            int[] blogPostsIDs =
                Classes.BlogPost.Search(null, null, null, null, true,
                                        DateTime.Now.Subtract(
                                            TimeSpan.FromDays(Classes.Config.Misc.ElapsedDaysOfBlogCreation)), null,
                                        false, Classes.Config.Misc.NumberOfNewBlogs, BlogPost.eSortColumn.Reads);

            if (blogPostsIDs.Length > 0)
            {
                foreach (int postsID in blogPostsIDs)
                {
                    try
                    {
                        BlogPost blogPost = Classes.BlogPost.Load(postsID);

                        Classes.Blog blog = Classes.Blog.Load(blogPost.BlogId);

                        if (blog != null)
                        {
                            int imageID = 0;
                            try
                            {
                                imageID = Photo.GetPrimary(blog.Username).Id;
                            }
                            catch (NotFoundException)
                            {
                                try
                                {
                                    User user = User.Load(blog.Username);
                                    imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                                }
                                catch (NotFoundException)
                                {
                                }
                            }
                            dtBlogs.Rows.Add(new object[]
                                             {
                                                 blog.Id,
                                                 blog.Name,
                                                 blogPost.Id,
                                                 blogPost.Title,
                                                 blogPost.DatePosted.ToShortDateString(),
                                                 blog.Username,
                                                 imageID
                                             });
                        }
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }
                }
            }
            else
            {
                lblError.Text = Lang.Trans("There are no popular blog posts.");
                rptNewBlogs.Visible = false;
            }

            rptNewBlogs.DataSource = dtBlogs;
            rptNewBlogs.DataBind();
        }
    }
}