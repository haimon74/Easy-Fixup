using System;
using System.Data;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class ShowUserError : PageBase
    {
        public ShowUserError()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                processError();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack)
            {
                string error = Request.Params["error"];
                if (error == "pro" || error == "pfo" || error == "pfof")
                {
                    string username = Request.Params["uid"];

                    #region Load Photo Album's strings

                    PhotoAlbum[] photoAlbums = PhotoAlbum.Fetch(username);

                    DataTable dtPhotoAlbums = new DataTable();
                    dtPhotoAlbums.Columns.Add("PhotoAlbumID");
                    dtPhotoAlbums.Columns.Add("PhotoAlbumName");
                    dtPhotoAlbums.Columns.Add("NumberOfPhotos");

                    int[] photosIDs = Photo.Search(-1, username, null, true, null, null, null);

                    if (photosIDs.Length > 0)
                    {
                        dtPhotoAlbums.Rows.Add(new object[]
                                               {
                                                   null,
                                                   String.Format("{0}'s Photos".Translate(), username),
                                                   photosIDs.Length
                                               });
                    }

                    foreach (PhotoAlbum photoAlbum in photoAlbums)
                    {
                        photosIDs = Photo.Search(-1, username, photoAlbum.ID, true, null, null, null);
                        if (photosIDs.Length == 0) continue;
                        dtPhotoAlbums.Rows.Add(new object[] { photoAlbum.ID, photoAlbum.Name, photosIDs.Length });
                    }

                    dlPhotoAlbums.DataSource = dtPhotoAlbums;
                    dlPhotoAlbums.DataBind();

                    // Do not show photo album links when there is just one album
                    pnlPhotoAlbums.Visible = Config.Users.EnablePhotoAlbums && dtPhotoAlbums.Rows.Count > 1;

                    #endregion
                }
            }
        }

        private void processError()
        {
            LargeBoxStart1.Title = "Access Denied".Translate();

            if (String.IsNullOrEmpty(Request.Params["error"]))
            {
                Response.Redirect("~/Default.aspx");
            }
            else
            {
                LargeBoxStart1.Title = "Access Denied".Translate();

                string errorCode = Request.Params["error"];

                switch (errorCode)
                {
                    case "ro":
                        lblError.Text = "This profile is visible for registered users only!".Translate();
                        break;
                    case "fo":
                        lblError.Text = "This profile is visible for friends only!".Translate();
                        break;
                    case "fof":
                        lblError.Text = "This profile is visible for friends of friends!".Translate();
                        break;
                    case "pro":
                        lblError.Text = "This photos are visible for registered users only!".Translate();
                        break;
                    case "pfo":
                        lblError.Text = "This photos are visible for friends only!".Translate();
                        break;
                    case "pfof":
                        lblError.Text = "This photos are visible for friends of friends!".Translate();
                        break;
                    case "bro":
                        lblError.Text = "This blog is visible for registered users only!".Translate();
                        break;
                    case "bfo":
                        lblError.Text = "This blog is visible for friends only!".Translate();
                        break;
                    case "bfof":
                        lblError.Text = "This blog is visible for friends of friends!".Translate();
                        break;
                    case "ero":
                        lblError.Text = "This events are visible for registered users only!".Translate();
                        break;
                    case "efo":
                        lblError.Text = "This events are visible for friends only!".Translate();
                        break;
                    case "efof":
                        lblError.Text = "This events are visible for friends of friends!".Translate();
                        break;
                }
            }
        }

        protected void dlPhotoAlbums_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "ViewPhotoAlbum")
            {
                int? photoAlbumID = String.IsNullOrEmpty((string)e.CommandArgument)
                                                  ? null
                                                  : (int?)Int32.Parse((string)e.CommandArgument);

                if (photoAlbumID.HasValue)
                {
                    PhotoAlbum photoAlbum = PhotoAlbum.Fetch(photoAlbumID.Value);
                    if (photoAlbum != null)
                    {
                        User ViewedUser = ((ShowUser)Master).ViewedUser;
                        if (CurrentUserSession != null && ViewedUser.Username != CurrentUserSession.Username)
                        {
                            if (photoAlbum.Access == PhotoAlbum.eAccess.FriendsOnly)
                            {
                                if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                                {
                                    Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=pfo",
                                                                    Request.Params["uid"]));
                                    return;
                                }
                            }
                            else if (photoAlbum.Access == PhotoAlbum.eAccess.FriendsAndTheirFriends)
                            {
                                if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                                {
                                    bool areFriends = false;
                                    string[] friends = Classes.User.FetchMutuallyFriends(ViewedUser.Username);
                                    foreach (string friend in friends)
                                    {
                                        if (Classes.User.IsUserInFriendList(friend, CurrentUserSession.Username))
                                        {
                                            areFriends = true;
                                            break;
                                        }
                                    }
                                    if (!areFriends)
                                    {
                                        Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=pfof",
                                                                        Request.Params["uid"]));
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }

                Response.Redirect(String.Format("~/ShowUserPhotos.aspx?uid={0}&paid={1}", Request.Params["uid"],
                                                photoAlbumID));
            }
        }
    }
}
