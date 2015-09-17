/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    [Editable]
    public partial class PopularBlogPostsWebPart : WebPartUserControl
    {
        private bool? ControlLoaded
        {
            get
            {
                return ViewState["ControlLoaded"] as bool?;
            }

            set
            {
                ViewState["ControlLoaded"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Popular Blog Posts");
            if (!IsPostBack)
            {
            }
        }

        private void LoadStrings()
        {
            //LargeBoxStart1.Title = Lang.Trans("Popular Blogs");
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!ControlLoaded.HasValue)
            {
                LoadStrings();
                LoadNewBlogs();

                ControlLoaded = true;
            }
        }

        private void LoadNewBlogs()
        {
            DataTable dtBlogs = new DataTable("PopularBlogPosts");

            dtBlogs.Columns.Add("BlogID", typeof (int));
            dtBlogs.Columns.Add("BlogName");
            dtBlogs.Columns.Add("BlogPostID", typeof (int));
            dtBlogs.Columns.Add("BlogPostTitle");
            dtBlogs.Columns.Add("DatePosted");
            dtBlogs.Columns.Add("Username");
            dtBlogs.Columns.Add("ImageID", typeof (int));

            int[] blogPostsIDs =
                BlogPost.Search(null, null, null, null, true,
                                DateTime.Now.Subtract(
                                    TimeSpan.FromDays(Config.Misc.ElapsedDaysOfBlogCreation)), null,
                                false, Config.Misc.NumberOfNewBlogs, BlogPost.eSortColumn.Reads);

            if (blogPostsIDs.Length > 0)
            {
                foreach (int postsID in blogPostsIDs)
                {
                    try
                    {
                        BlogPost blogPost = BlogPost.Load(postsID);

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
                //lblError.Text = Lang.Trans("There are no popular blog posts.");
                rptNewBlogs.Visible = false;
            }

            if (dtBlogs.Rows.Count > 0)
                mvNewBlogs.SetActiveView(vNewBlogs);
            else
                mvNewBlogs.SetActiveView(vNoNewBlogs);

            rptNewBlogs.DataSource = dtBlogs;
            rptNewBlogs.DataBind();
        }
    }
}