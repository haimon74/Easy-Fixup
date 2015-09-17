using System;
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class ShowUserPhotos : PageBase
    {
        public ShowUserPhotos()
        {
            RequiresAuthorization = Config.Users.RegistrationRequiredToBrowse || 
                Config.Users.OnlyRegisteredCanViewPhotos;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            ViewPhotosCtrl.User = ((ShowUser) Master).ViewedUser;
            Classes.User ViewedUser = ((ShowUser)Master).ViewedUser;

            if (!IsPostBack)
            {
                SmallBoxStart1.Title = "Photo Albums".Translate();

                if (!Config.Photos.FreeMembersCanViewPhotosOfPaidMembers)
                {
                    if (CurrentUserSession == null)
                    {
                        if (Classes.User.IsPaidMember(ViewedUser.Username))
                        {
                            StatusPageMessage = "Only paid members can view those photos".Translate();
                            Response.Redirect(GetLastViewedProfileUrl());
                        }
                    }
                    else if (!Classes.User.IsPaidMember(CurrentUserSession.Username)
                                && Classes.User.IsPaidMember(ViewedUser.Username))
                    {
                        StatusPageMessage = "Only paid members can view those photos".Translate();
                        Response.Redirect(GetLastViewedProfileUrl());
                    }
                }

                if (CurrentUserSession == null)
                {
                    if (!ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewPhotos))
                    {
                        if (ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewPhotos))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=pro", Request.Params["uid"]));
                        else if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewPhotos))
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=pfof", Request.Params["uid"]));
                        else Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=pfo", Request.Params["uid"]));
                    }
                }
                else
                {
                    if (ViewedUser.Username != CurrentUserSession.Username
                        && !ViewedUser.IsOptionEnabled(eUserOptions.VisitorsCanViewPhotos)
                        && !ViewedUser.IsOptionEnabled(eUserOptions.UsersCanViewPhotos))
                    {
                        if (ViewedUser.IsOptionEnabled(eUserOptions.FriendsOfFriendsCanViewPhotos))
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
                                    Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=pfof", Request.Params["uid"]));
                                }
                            }
                        }
                        else if (!ViewedUser.IsUserInFriendList(CurrentUserSession.Username))
                        {
                            Response.Redirect(String.Format("~/ShowUserError.aspx?uid={0}&error=pfo", Request.Params["uid"]));
                        }
                    }
                }

                #region Load Photo Album's strings

                PhotoAlbum[] photoAlbums = PhotoAlbum.Fetch(ViewedUser.Username);

                DataTable dtPhotoAlbums = new DataTable();
                dtPhotoAlbums.Columns.Add("PhotoAlbumID");
                dtPhotoAlbums.Columns.Add("PhotoAlbumName");
                dtPhotoAlbums.Columns.Add("NumberOfPhotos");

                int[] photosIDs = Photo.Search(-1, ViewedUser.Username, null, true, null, null, null);

                if (photosIDs.Length > 0)
                {
                    dtPhotoAlbums.Rows.Add(new object[]
                                               {
                                                   null,
                                                   String.Format("{0}'s Photos".Translate(), ViewedUser.Username),
                                                   photosIDs.Length
                                               });
                    if (!String.IsNullOrEmpty(Request.Params["paid"]))
                        ViewPhotosCtrl.PhotoAlbumID = Convert.ToInt32(Request.Params["paid"]);
                }
                else if (photoAlbums.Length > 0)
                {
                    ViewPhotosCtrl.PhotoAlbumID = String.IsNullOrEmpty(Request.Params["paid"])
                                                      ? photoAlbums[0].ID
                                                      : Convert.ToInt32(Request.Params["paid"]);
                }

                foreach (PhotoAlbum photoAlbum in photoAlbums)
                {
                    photosIDs = Photo.Search(-1, ViewedUser.Username, photoAlbum.ID, true, null, null, null);
                    if (photosIDs.Length == 0) continue;
                    dtPhotoAlbums.Rows.Add(new object[] { photoAlbum.ID, photoAlbum.Name, photosIDs.Length });
                }

                dlPhotoAlbums.DataSource = dtPhotoAlbums;
                dlPhotoAlbums.DataBind();

                // Do not show photo album links when there is just one album
                pnlPhotoAlbums.Visible = Config.Users.EnablePhotoAlbums && dtPhotoAlbums.Rows.Count > 1;

                #endregion
            }
            base.OnPreRender(e);
        }

        protected void dlPhotoAlbums_ItemCommand(object source, System.Web.UI.WebControls.DataListCommandEventArgs e)
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
                        Classes.User ViewedUser = ((ShowUser)Master).ViewedUser;
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

                ViewPhotosCtrl.PhotoAlbumID = photoAlbumID;
                ViewPhotosCtrl.loadPhotos = true;
                ViewPhotosCtrl.loadComments = Config.Photos.EnablePhotoComments;
                ViewPhotosCtrl.BigPhotoImageTag = String.Empty;
            }
        }
    }
}