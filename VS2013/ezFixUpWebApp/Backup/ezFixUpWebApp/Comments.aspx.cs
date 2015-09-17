using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class Comments : PageBase
    {
        protected Repeater rptPhotoComments;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadProfileComments();
                loadPicturesComments();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            lblMessage.Text = !pnlProfileComments.Visible && !pnlPicturesComments.Visible
                                  ? Lang.Trans("There are no comments.")
                                  : String.Empty;
        }

        private void loadStrings()
        {
            hlUserComments.Title = Lang.Trans("Profile Comments");
            hlPicturesComments.Title = Lang.Trans("Photo Comments");
            lnkViewAllComments.Text = Lang.Trans("View All Comments");
            LargeBoxStart1.Title = Lang.Trans("Comments");
        }

        private void loadProfileComments()
        {
            DataTable dtComments = new DataTable();
            dtComments.Columns.Add("Id", typeof (int));
            dtComments.Columns.Add("DatePosted", typeof (DateTime));
            dtComments.Columns.Add("FromUsername", typeof (string));
            dtComments.Columns.Add("CommentText", typeof (string));

            int countLimit = -1;
            if (ViewState["ViewProfile_ViewAllComments"] == null) countLimit = Config.Profiles.NumberOfProfileCommentsToShow;
            Comment[] comments = Comment.Load(CurrentUserSession.Username, countLimit);
            if (comments.Length < Config.Profiles.NumberOfProfileCommentsToShow) divViewAllComments.Visible = false;

            foreach (Comment comment in comments)
            {
                dtComments.Rows.Add(new object[]
                                        {
                                            comment.Id, comment.DatePosted, comment.FromUsername,
                                            Server.HtmlEncode(comment.CommentText)
                                        });
            }

            rptProfileComments.DataSource = dtComments;
            rptProfileComments.DataBind();

            pnlProfileComments.Visible = dtComments.Rows.Count > 0;
        }

        private void loadPicturesComments()
        {
            Photo[] photos = Photo.Fetch(CurrentUserSession.Username);

            DataTable dtPhotos = new DataTable("Photos");
            dtPhotos.Columns.Add("PhotoID", typeof(int));

            if (photos != null && photos.Length > 0)
                foreach (Photo photo in photos)
                {
                    if (!photo.Approved || PhotoComment.FetchByPhotoID(photo.Id).Length == 0) continue;

                    dtPhotos.Rows.Add(new object[]
                                          {
                                              photo.Id,
                                          });
                }

            rptPicturesComments.DataSource = dtPhotos;
            rptPicturesComments.DataBind();

            pnlPicturesComments.Visible = dtPhotos.Rows.Count > 0;
        }

        protected void rptComments_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDeleteComment = (LinkButton)e.Item.FindControl("lnkDeleteComment");
            lnkDeleteComment.Attributes.Add("onclick",
                                            String.Format("javascript: return confirm('{0}')",
                                                          Lang.Trans("Do you really want to remove this comment?")));
        }

        protected void rptComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);

                Comment.Delete(commentId);

                loadProfileComments();
            }
        }

        protected void rptPhotoComments_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkDeleteComment = (LinkButton)e.Item.FindControl("lnkDeleteComment");
            lnkDeleteComment.Attributes.Add("onclick",
                                            String.Format("javascript: return confirm('{0}')",
                                                          Lang.Trans("Do you really want to remove this comment?")));
        }

        protected void rptPhotoComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "DeleteComment")
            {
                int commentId = Convert.ToInt32(e.CommandArgument);
                PhotoComment.Delete(commentId);
            }

            loadPicturesComments();
        }

        protected void lnkViewAllComments_Click(object sender, EventArgs e)
        {
            divViewAllComments.Visible = false;
            ViewState["ViewProfile_ViewAllComments"] = true;

            loadProfileComments();
        }

        protected void rptPicturesComments_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            RepeaterItem item = e.Item;
            if ((item.ItemType == ListItemType.Item) ||
                (item.ItemType == ListItemType.AlternatingItem))
            {
                rptPhotoComments = (Repeater)item.FindControl("rptPhotoComments");
                HtmlGenericControl pnlViewAllPhotoComments = (HtmlGenericControl)e.Item.FindControl("pnlViewAllPhotoComments");

                int photoID = (int)DataBinder.Eval(e.Item.DataItem, "PhotoID");

                DataTable dtComments = new DataTable();
                dtComments.Columns.Add("ID", typeof(int));
                dtComments.Columns.Add("DatePosted", typeof(DateTime));
                dtComments.Columns.Add("FromUsername", typeof(string));
                dtComments.Columns.Add("CommentText", typeof(string));

                int? countLimit = null;
                if (ViewState[String.Format("ViewProfile_ViewAllPhotoComments_{0}", photoID)] == null)
                    countLimit = Config.Profiles.NumberOfPhotoCommentsToShow;
                else pnlViewAllPhotoComments.Visible = false;

                PhotoComment[] comments = countLimit.HasValue
                                              ? PhotoComment.FetchByPhotoID(photoID, countLimit.Value)
                                              : PhotoComment.FetchByPhotoID(photoID);

                if (comments.Length < Config.Profiles.NumberOfPhotoCommentsToShow) pnlViewAllPhotoComments.Visible = false;

                foreach (PhotoComment comment in comments)
                {
                    dtComments.Rows.Add(new object[]
                                        {
                                            comment.ID, comment.Date, comment.Username,
                                            Server.HtmlEncode(comment.Comment)
                                        });
                }

                rptPhotoComments.DataSource = dtComments;
                rptPhotoComments.DataBind();
            }
        }

        protected void rptPicturesComments_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item && e.Item.ItemType != ListItemType.AlternatingItem)
                return;

            LinkButton lnkViewAllPhotoComments = (LinkButton) e.Item.FindControl("lnkViewAllPhotoComments");
            lnkViewAllPhotoComments.Text = Lang.Trans("View All Comments");
        }

        protected void rptPicturesComments_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "ViewAllPhotoComments")
            {
                ViewState[String.Format("ViewProfile_ViewAllPhotoComments_{0}", Convert.ToInt32(e.CommandArgument))] = true;

                loadPicturesComments();
            }
        }
    }
}
