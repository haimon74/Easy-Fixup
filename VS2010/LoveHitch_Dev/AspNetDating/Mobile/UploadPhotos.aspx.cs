using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using Image=System.Drawing.Image;

namespace AspNetDating.Mobile
{
    public partial class UploadPhotos : PageBase
    {
        private bool loadPhotos;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack)
            {
                loadPhotos = true;
            }

            if (loadPhotos)
                LoadPhotos();
        }

        private void LoadStrings()
        {
            cvNote.Text = "Upload photos notes".Translate();
            lblTitle.InnerText = "Upload Photos".Translate();
            chkPrivatePhoto.Text = Lang.Trans("Set this photo as private");
            lnkSave.Text = Lang.Trans("Save");
            lnkCancel.Text = Lang.Trans("Cancel");
            lblPhoto.Text = Lang.Trans("Photo:");
            btnUpload.Text = Lang.Trans("Upload");
        }

        public void LoadPhotos()
        {
            if (User == null) return;

            int allPhotos = Photo.Search(-1, CurrentUserSession.Username, -1, null, null, null, null).Length;
            Photo[] photos = Photo.Fetch(CurrentUserSession.Username, null);

            var dtPhotos = new DataTable("Photos");
            dtPhotos.Columns.Add("PhotoId");
            dtPhotos.Columns.Add("Name");
            dtPhotos.Columns.Add("Description");
            dtPhotos.Columns.Add("Approved");
            dtPhotos.Columns.Add("Primary");
            dtPhotos.Columns.Add("Private", typeof(bool));

            if (photos != null && photos.Length > 0)
                foreach (Photo photo in photos)
                {
                    dtPhotos.Rows.Add(new object[]
                                          {
                                              photo.Id,
                                              photo.Name,
                                              photo.Description,
                                              photo.Approved,
                                              photo.Primary,
                                              photo.PrivatePhoto
                                          });
                }

            int maxPhotos = ((PageBase)Page).CurrentUserSession.BillingPlanOptions.MaxPhotos.Value;
            if (CurrentUserSession.Level != null && maxPhotos < CurrentUserSession.Level.Restrictions.MaxPhotos)
                maxPhotos = CurrentUserSession.Level.Restrictions.MaxPhotos;
            pnlEditImage.Visible = allPhotos < maxPhotos;
//            if (allPhotos < maxPhotos)
//            {
//                dtPhotos.Rows.Add(new object[]
//                                      {
//                                          0, //photo.Id,
//                                          "", //photo.Name,
//                                          "", //photo.Description,
//                                          true, //photo.Approved,
//                                          false, //photo.Primary
//                                          false //photo.PrivatePhoto
//                                      });
//            }

            dlPhotos.DataSource = dtPhotos;
            dlPhotos.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            Image image;
            try
            {
                image = Image.FromStream
                    (ufPhoto.PostedFile.InputStream);
            }
            catch
            {
                lblError.Text = Lang.Trans("Invalid image!");
                return;
            }

            Photo photo;

            if (Session["temp_photo"] == null)
                photo = new Photo();
            else
                photo = (Photo)Session["temp_photo"];

            if (image.Height < Config.Photos.PhotoMinHeight
                || image.Width < Config.Photos.PhotoMinWidth)
            {
                lblError.Text = Lang.Trans("The photo is too small!");
                return;
            }

            photo.Image = image;

            string tempFileName;

            if (!Misc.GetTempFileName(out tempFileName))
                tempFileName = Path.GetTempFileName();

            photo.Image.Save(tempFileName);
            Session["temp_photo_fileName"] = tempFileName;

            if (Config.Photos.AutoApprovePhotos
                || CurrentUserSession.BillingPlanOptions.AutoApprovePhotos.Value
                || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.AutoApprovePhotos)
            {
                photo.Approved = true;
            }
            else
            {
                photo.Approved = false;
            }

            photo.ExplicitPhoto = false;

            photo.Image = null;
            Session["temp_photo"] = photo;

            if (!Config.Photos.EnablePrivatePhotos
                ||
                Config.CommunityFaceControlSystem.EnableCommunityFaceControl && !CurrentUserSession.FaceControlApproved)
                chkPrivatePhoto.Visible = false;
            else
                chkPrivatePhoto.Visible = true;

//            divFileUploadControls.Visible = false;
//            divImageRotateControls.Visible = true;
        }

        protected void lnkSave_Click(object sender, EventArgs e)
        {
            var photo = (Photo)Session["temp_photo"];

            if (photo == null)
            {
                if (ufPhoto.HasFile)
                {
                    btnUpload_Click(null, null);
                    photo = (Photo)Session["temp_photo"];
                }
                else
                {
                    lblError.Text = Lang.Trans("Please upload image first!");
                    return;
                }
            }

            // ReSharper disable AssignNullToNotNullAttribute
            photo.Image = Image.FromFile(Session["temp_photo_fileName"] as string);
            // ReSharper restore AssignNullToNotNullAttribute

            if (photo.Id > 0)
            {
                try
                {
                    string cacheFileDir = Config.Directories.ImagesCacheDirectory + "/" + photo.Id % 10;
                    string cacheFileMask = String.Format("photo_{0}_*.jpg", photo.Id);
                    foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                    {
                        File.Delete(file);
                    }
                    cacheFileMask = String.Format("photoface_{0}_*.jpg", photo.Id);
                    foreach (string file in Directory.GetFiles(cacheFileDir, cacheFileMask))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception err)
                {
                    Global.Logger.LogError(err);
                }
            }

            photo.User = CurrentUserSession;
            photo.Name = String.Empty;
            photo.Description = String.Empty;
            photo.PrivatePhoto = chkPrivatePhoto.Checked;

            bool isNewPhoto = photo.Id == 0;

            photo.Save(true);

            if (photo.Approved && !photo.PrivatePhoto && isNewPhoto)
            {
                #region Add NewFriendPhoto Event

                Event newEvent = new Event(CurrentUserSession.Username);

                newEvent.Type = Event.eType.NewFriendPhoto;
                NewFriendPhoto newFriendPhoto = new NewFriendPhoto();
                newFriendPhoto.PhotoID = photo.Id;
                newEvent.DetailsXML = Misc.ToXml(newFriendPhoto);

                newEvent.Save();

                string[] usernames = Classes.User.FetchMutuallyFriends(CurrentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification)
                    {
                        string text = String.Format("Your friend {0} has uploaded a new photo".Translate(),
                                                  "<b>" + photo.Username + "</b>");
                        string thumbnailUrl = ImageHandler.CreateImageUrl(photo.Id, 50, 50, false, true, true);
                        Classes.User.SendOnlineEventNotification(photo.Username, friendUsername,
                                                                 text, thumbnailUrl,
                                                                 UrlRewrite.CreateShowUserPhotosUrl(photo.Username));
                    }
                }

                #endregion
            }

            photo.Image.Dispose();

            try
            {
                if (Session["temp_photo_fileName"] != null)
                    File.Delete((string)Session["temp_photo_fileName"]);
            }
            catch (IOException)
            {
            }

            Session["temp_photo"] = null;
            Session["temp_photo_fileName"] = null;

//            pnlEditImage.Visible = false;
            dlPhotos.Visible = true;

            loadPhotos = true;
        }

        /// <summary>
        /// Handles the Click event of the lnkCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkCancel_Click(object sender, EventArgs e)
        {
            Session["temp_photo"] = null;
            Session["temp_photo_fileName"] = null;

//            pnlEditImage.Visible = false;
            dlPhotos.Visible = true;
            loadPhotos = true;
            chkPrivatePhoto.Visible = false;
        }

        protected void dlPhotos_ItemCreated(object sender, DataListItemEventArgs e)
        {
            var lnkDeletePhoto = (LinkButton)e.Item.FindControl("lnkDeletePhoto");
            lnkDeletePhoto.Attributes.Add("onclick",
                                          String.Format("javascript: return confirm('{0}')",
                                                        Lang.Trans("Do you really want to delete this photo?")));
        }

        protected void dlPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int photoId;
            Photo photo;

            switch (e.CommandName)
            {
                case "DeletePhoto":
                    photoId = Convert.ToInt32(e.CommandArgument);
                    try
                    {
                        photo = Photo.Fetch(photoId);
                    }
                    catch (NotFoundException)
                    {
                        return;
                    }

                    if (photo.User.Username == CurrentUserSession.Username)
                    {
                        try
                        {
                            photo.Delete();

                            Event[] events = Event.Fetch(photo.Username, (ulong)Event.eType.NewFriendPhoto, 1000);

                            foreach (Event ev in events)
                            {
                                NewFriendPhoto newFriendPhoto = Misc.FromXml<NewFriendPhoto>(ev.DetailsXML);
                                if (newFriendPhoto.PhotoID == photo.Id)
                                {
                                    Event.Delete(ev.ID);
                                    break;
                                }
                            }
                        }
                        catch (NotFoundException)
                        {
                        }
                    }
                    loadPhotos = true;
                    break;

                case "PrimaryPhoto":
                    photoId = Convert.ToInt32(e.CommandArgument);
                    try
                    {
                        photo = Photo.Fetch(photoId);
                    }
                    catch (NotFoundException)
                    {
                        return;
                    }
                    Photo.SetPrimary(CurrentUserSession.Username, photo);

                    loadPhotos = true;
                    break;
            }
        }
    }
}
