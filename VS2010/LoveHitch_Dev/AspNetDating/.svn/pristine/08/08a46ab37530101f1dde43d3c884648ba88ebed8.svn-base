using System;
using System.Drawing;
using System.IO;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class EditSalutePhoto : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
                LoadPhoto();
            }
        }

        private void LoadStrings()
        {
            cvNote.Text = "Upload Salute Photo Notes".Translate();
            if (!Config.Photos.EnableSalutePhoto) Response.Redirect(MatchmakerHelper.CurrentHomePage);
            LargeBoxStart1.Title = "Upload Salute Photo".Translate();
            SmallBoxStart1.Title = Lang.Trans("Profile Management");
            lblPhoto.Text = Lang.Trans("Photo:");
            btnUpload.Text = Lang.Trans("Upload");
            chkPrivatePhoto.Text = Lang.Trans("Set this photo as private");
            if (!Config.Photos.EnablePrivatePhotos
                ||
                Config.CommunityFaceControlSystem.EnableCommunityFaceControl && !CurrentUserSession.FaceControlApproved)
                chkPrivatePhoto.Visible = false;
            btnRotateLeft.Text = "Rotate Left".Translate();
            btnRotateRight.Text = "Rotate Right".Translate();
            lnkSave.Text = Lang.Trans("Save");
            lnkCancel.Text = Lang.Trans("Cancel");
        }

        private void LoadPhoto()
        {
            Photo salutePhoto = Photo.FetchSalute(CurrentUserSession.Username);
            if (salutePhoto != null)
            {
                if (salutePhoto.User.Username == CurrentUserSession.Username)
                {
                    string tempFileName;

                    if (!Misc.GetTempFileName(out tempFileName))
                        tempFileName = Path.GetTempFileName();

                    salutePhoto.Image.Save(tempFileName);
                    salutePhoto.Image = null;
                    Session["temp_photo"] = salutePhoto;
                    Session["temp_photo_fileName"] = tempFileName;
                    txtName.Text = salutePhoto.Name;
                    txtDescription.Text = salutePhoto.Description;
                    chkPrivatePhoto.Checked = salutePhoto.PrivatePhoto;
                    if (salutePhoto.ExplicitPhoto)
                        chkPrivatePhoto.Enabled = false;
                    divImageRotateControls.Visible = true;
                }
                else return;
            }
            else
            {
                Session["temp_photo"] = null;
                Session["temp_photo_fileName"] = null;
                txtName.Text = "";
                txtDescription.Text = "";
                chkPrivatePhoto.Checked = false;

                lblPhoto.Visible = true;
                ufPhoto.Visible = true;
                btnUpload.Visible = true;
                divImageRotateControls.Visible = false;
            }
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
            chkPrivatePhoto.Enabled = true;

            photo.Image = null;
            Session["temp_photo"] = photo;

            divImageRotateControls.Visible = true;
        }

        protected void btnRotateLeft_Click(object sender, EventArgs e)
        {
            var tempPhotoFile = (string)Session["temp_photo_fileName"];
            if (string.IsNullOrEmpty(tempPhotoFile)) return;
            using (Image img = Image.FromFile(tempPhotoFile))
            {
                img.RotateFlip(RotateFlipType.Rotate270FlipNone);

                string newTempPhotoFile;
                if (!Misc.GetTempFileName(out newTempPhotoFile))
                    newTempPhotoFile = Path.GetTempFileName();

                img.Save(newTempPhotoFile);
                Session["temp_photo_fileName"] = newTempPhotoFile;
            }
            try
            {
                File.Delete(tempPhotoFile);
            }
            catch (IOException)
            {
            }
        }

        protected void btnRotateRight_Click(object sender, EventArgs e)
        {
            var tempPhotoFile = (string)Session["temp_photo_fileName"];
            if (string.IsNullOrEmpty(tempPhotoFile)) return;
            using (Image img = Image.FromFile(tempPhotoFile))
            {
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);

                string newTempPhotoFile;
                if (!Misc.GetTempFileName(out newTempPhotoFile))
                    newTempPhotoFile = Path.GetTempFileName();

                img.Save(newTempPhotoFile);
                Session["temp_photo_fileName"] = newTempPhotoFile;
            }
            try
            {
                File.Delete(tempPhotoFile);
            }
            catch (IOException)
            {
            }
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
            photo.Name = Config.Misc.EnableBadWordsFilterProfile ? Parsers.ProcessBadWords(txtName.Text) : txtName.Text;
            photo.Description = Config.Misc.EnableBadWordsFilterProfile
                                    ? Parsers.ProcessBadWords(txtDescription.Text)
                                    : txtDescription.Text;
            photo.PrivatePhoto = chkPrivatePhoto.Checked;
            photo.Salute = true;

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

            Response.Redirect("~/ManageProfile.aspx?sel=photos");
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
            Response.Redirect("~/ManageProfile.aspx?sel=photos");
        }
    }
}
