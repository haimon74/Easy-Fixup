/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using AspNetDating.Classes;
using Image = System.Drawing.Image;

namespace AspNetDating.Components.Profile
{
    public partial class EditPhotos : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        protected LargeBoxStart LargeBoxStart1;
        public string CssClass { set { LargeBoxStart1.CssClass = value; } }
        
        private bool loadPhotos;

        /// <summary>
        /// 
        /// </summary>
        protected HeaderLine PhotoGuidelinesHeaderLine;

        /// <summary>
        /// 
        /// </summary>
        protected HeaderLine PostingPhotosHeaderLine;

        private User user;

        private bool FirstLoad
        {
            get { return ViewState["FirstLoad"] == null ? true : false; }
            set { ViewState["FirstLoad"] = value; }
        }

        /// <summary>
        /// Gets the current user session.
        /// </summary>
        /// <value>The current user session.</value>
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public User User
        {
            set
            {
                user = value;
                ViewState["Username"] = user != null ? user.Username : null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        protected int? EditedPhotoAlbumID
        {
            get { return (int?)ViewState["EditedPhotoAlbumID"]; }
            set { ViewState["EditedPhotoAlbumID"] = value; }
        }

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "asyncUploadError", 
                "function uploadError() { return; }", true);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "asyncUploadStarted", 
               "function uploadStarted() { return; }", true);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "asyncUploadComplete", 
                @"function uploadComplete(sender, args) {
                    var filename = 'AsyncFileUpload/Uploads/' + args.get_fileName();
                    $('#loadedImage').fadeOut(500, function() {
                        $(this).attr('src', filename);
                        $(this).fadeIn(500);
                    });
                    return;
                }", true);

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "asyncUploadBrowseTrigger", 
                @"function triggerAsyncImgUpload() {
                    var ctrlId = '#" + asyncImageUpload.ClientID +"';"+
                   @"var firedObj = $(ctrlId).find('input[type=file]');
                    var fireOnThis = document.getElementById(firedObj.attr('id'));
                    if (fireOnThis.click){fireOnThis.click()}
                    else if (document.createEvent) { 
                        var evObj = document.createEvent('MouseEvents');
                        evObj.initMouseEvent('click', true, true, window, 1, 12, 345, 7, 220, false, false, true, false, 0, null);
                        fireOnThis.dispatchEvent(evObj);}
                    else if (document.createEventObject){fireOnThis.fireEvent('onclick');}
                }"
                , true);

            /*"var filename = \"./AsyncFileUpload/Uploads/\" + args.get_fileName();"+
                //"jQuery(function($) {" +
                "$(\"#loadedImage\").fadeOut(500, function() {" +
                "$(this).attr(\"src\", filename);" +
                "$(this).fadeIn(500);});return;}", true);*/

            asyncImageUpload.UploadedComplete += asyncImageUpload_UploadedComplete;
                    
            if (!Page.IsPostBack)
            {
                LoadStrings();
            }

            string cacheKey = "flashPhotoUploadError_" + CurrentUserSession.Username;
            if (Cache.Get(cacheKey) != null)
            {
                var error = (string)Cache.Get(cacheKey);
                Page.ClientScript.RegisterStartupScript(GetType(), "error",
                                                        String.Format("var alert_string = '{0}';",
                                                                      error), true);
                Cache.Remove(cacheKey);
            }

            cacheKey = "silverlightPhotoUploadError_" + CurrentUserSession.Username;
            if (Cache.Get(cacheKey) != null)
            {
                var error = (string)Cache.Get(cacheKey);
                Page.ClientScript.RegisterStartupScript(GetType(), "error",
                                                        String.Format("var alert_string = '{0}';",
                                                                      error), true);
                Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (FirstLoad)
            {
                loadPhotos = true;
                FirstLoad = true;
            }

            if (loadPhotos)
                LoadPhotos();

            btnCreateEditPhotoAlbum.Text = EditedPhotoAlbumID == null ? "Create".Translate() : "Update".Translate();
        }

        private void LoadStrings()
        {
            cvNote.Text = "Upload photos notes".Translate();
            cvGuideLines.Text = String.Format("{0}<ul><li>{1}</li><li>{2}</li><li>{3}</li><li>{4}</li><li>{5}</li></ul>",
                "Upload photos guidelines".Translate()
                , "Upload photos guidelines no groups".Translate()
                , "Upload photos guidelines no over-sized".Translate()
                , "Upload photos guidelines crop empty".Translate()
                , "Upload photos guidelines no erotic".Translate()
                , "Upload photos guidelines no celeb copy".Translate()
                );
            
            LargeBoxStart1.Title = Lang.Trans("Upload Photos");

            PostingPhotosHeaderLine.Title = Lang.Trans("Posting Photos");
            PhotoGuidelinesHeaderLine.Title = Lang.Trans("Photo Guidelines");
            lnkSave.Text = Lang.Trans("Save");
            lnkCancel.Text = Lang.Trans("Cancel");
            lblPhoto.Text = Lang.Trans("Photo") + ":";
            btnUpload.Text = Lang.Trans("Upload");
            chkPrivatePhoto.Text = Lang.Trans("Set this photo as private");
            if (!Config.Photos.EnablePrivatePhotos
                ||
                Config.CommunityFaceControlSystem.EnableCommunityFaceControl && !CurrentUserSession.FaceControlApproved)
                chkPrivatePhoto.Visible = false;
            lnkCreateNewAlbum.Text = "Create a new album".Translate();
            lnkEditAlbum.Text = "Edit album".Translate();
            lnkDeleteAlbum.Text = "Delete album".Translate();
            lnkUploadMultipleFlash.Text = "Upload multiple photos with Flash".Translate();
            lnkUploadMultipleSilverlight.Text = "Upload multiple photos with Silverlight".Translate();
            lnkUploadViaWebCam.Text = "Upload photos via web cam".Translate();
            lnkBackToPhotos.Text = "Back to photos".Translate();
            btnRotateLeft.Text = "Rotate Left".Translate();
            btnRotateRight.Text = "Rotate Right".Translate();
            btnCancelCreatePhotoAlbum.Text = "Cancel".Translate();

            populateDDPhotoAlbums();
            ddPhotoAlbums.SelectedValue = String.IsNullOrEmpty(Request.Params["album"]) ? "-1" : Request.Params["album"];

            ddPhotoAlbumAccess.Items.Add(new ListItem("All".Translate(), ((int)PhotoAlbum.eAccess.All).ToString()));
            ddPhotoAlbumAccess.Items.Add(new ListItem("Friends only".Translate(),
                                                      ((int)PhotoAlbum.eAccess.FriendsOnly).ToString()));
            ddPhotoAlbumAccess.Items.Add(new ListItem("Friends and their friends".Translate(),
                                                      ((int)PhotoAlbum.eAccess.FriendsAndTheirFriends).ToString()));

            lnkSaveThumbnail.Text = "Save thumbnail".Translate();
            lnkSkipThumbnail.Text = "Do not crop thumbnail".Translate();

            btnAddNote.Text = "Add".Translate();
            lnkDoneAddingPhotoNotes.Text = "Back".Translate();

            webcamUpload.UploadPage = Request.Url.GetLeftPart(UriPartial.Authority) +
                                      Request.ApplicationPath.TrimEnd('/') + "/WebcamUpload.ashx";
        }

        /// <summary>
        /// Loads the photos.
        /// </summary>
        public void LoadPhotos()
        {
            if (User == null) return;

            int allPhotos = Photo.Search(-1, User.Username, -1, null, null, null, null).Length;
            int photoAlbumID = Convert.ToInt32(ddPhotoAlbums.SelectedValue);
            Photo[] photos = Photo.Fetch(User.Username, photoAlbumID == -1 ? (int?)null : photoAlbumID);

            var dtPhotos = new DataTable("Photos");
            dtPhotos.Columns.Add("PhotoId");
            dtPhotos.Columns.Add("PhotoUrl");
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
                                              ImageHandler.CreateImageUrl(photo.Id, 120, 120, false, true,true),
                                              photo.Name,
                                              photo.Description,
                                              photo.Approved,
                                              photo.Primary,
                                              photo.PrivatePhoto
                                          });
                }

            int maxPhotos = ((PageBase)Page).CurrentUserSession.BillingPlanOptions.MaxPhotos.Value;
            if (User.Level != null && maxPhotos < User.Level.Restrictions.MaxPhotos)
                maxPhotos = User.Level.Restrictions.MaxPhotos;
            if (allPhotos < maxPhotos)
            {
                dtPhotos.Rows.Add(new object[]
                                      {
                                          0, //photo.Id,
                                          ImageHandler.CreateImageUrl(0, 120, 120, false, true, true),
                                          "", //photo.Name,
                                          "", //photo.Description,
                                          true, //photo.Approved,
                                          false, //photo.Primary
                                          false //photo.PrivatePhoto
                                      });
            }

            dlPhotos.DataSource = dtPhotos;
            dlPhotos.DataBind();

            lnkCreateNewAlbum.Visible = allPhotos < maxPhotos;
            lnkDeleteAlbum.Visible = ddPhotoAlbums.SelectedValue != "-1";
            lnkEditAlbum.Visible = ddPhotoAlbums.SelectedValue != "-1";
            divUploadMultiplePhotosFlash.Visible = Config.Users.EnableFlashUploads && allPhotos < maxPhotos;
            divUploadMultiplePhotosSilverlight.Visible = Config.Users.EnableSilverlightUploads && allPhotos < maxPhotos;
            divUploadViaWebCam.Visible = Config.Users.EnableWebcamPhotoCapture && allPhotos < maxPhotos;
            mvPhotoAlbum.SetActiveView(viewPhotoAlbums);
            mvPhotoAlbum.Visible = Config.Users.EnablePhotoAlbums;
        }

        private void dlPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            int photoId;
            Photo photo;

            switch (e.CommandName)
            {
                case "UploadPhoto":
                    mvPhotoAlbum.Visible = false;
                    photoId = Convert.ToInt32(e.CommandArgument);
                    if (photoId > 0)
                    {
                        try
                        {
                            photo = Photo.Fetch(photoId);
                        }
                        catch (NotFoundException)
                        {
                            return;
                        }

                        if (photo.User.Username == User.Username)
                        {
                            FindFaces(photo);

                            string tempFileName;
                                
                            if (!Misc.GetTempFileName(out tempFileName))
                                tempFileName = Path.GetTempFileName();

                            photo.Image.Save(tempFileName);
                            photo.Image = null;
                            Session["temp_photo"] = photo;
                            Session["temp_photo_fileName"] = tempFileName;
                            txtName.Text = photo.Name;
                            txtDescription.Text = photo.Description;
                            chkPrivatePhoto.Checked = photo.PrivatePhoto;
                            if (photo.ExplicitPhoto)
                                chkPrivatePhoto.Enabled = false;

                            lblPhoto.Visible = false;
                            ufPhoto.Visible = false;
                            btnUpload.Visible = false;

                            divFileUploadControls.Visible = false;
                            divImageRotateControls.Visible = false;
                        }
                        else return;
                    }
                    else
                    {
                        Session["temp_photo"] = null;
                        Session["temp_photo_fileName"] = null;
                        plhPhotoFaces.Visible = false;
                        txtName.Text = "";
                        txtDescription.Text = "";
                        chkPrivatePhoto.Checked = false;
                        plhPhotoFaces.Visible = false;

                        lblPhoto.Visible = true;
                        ufPhoto.Visible = true;
                        btnUpload.Visible = true;
                        divFileUploadControls.Visible = true;
                        divImageRotateControls.Visible = false;
                    }

                    pnlEditImage.Visible = true;
                    dlPhotos.Visible = false;
                    pnlPhotoAlbum.Visible = Config.Users.EnablePhotoAlbums;
                    divUploadMultiplePhotosFlash.Visible = false;
                    divUploadMultiplePhotosSilverlight.Visible = false;
                    divUploadViaWebCam.Visible = false;
                    if (ddPhotoAlbums.SelectedValue == "-1")
                        lblPhotoAlbumName.Text = String.Format("{0}'s Photos".Translate(), CurrentUserSession.Username);
                    else
                    {
                        PhotoAlbum photoAlbum = PhotoAlbum.Fetch(Convert.ToInt32(ddPhotoAlbums.SelectedValue));
                        if (photoAlbum != null) lblPhotoAlbumName.Text = photoAlbum.Name;
                    }
                    break;

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

                    if (photo.User.Username == User.Username)
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
                    Photo.SetPrimary(User.Username, photo);

                    if (string.IsNullOrEmpty(photo.FaceCrop))
                    {
                        // Load cropping page
                        imgFaceCropPhoto.Src = ImageHandler.CreateImageUrl(photo.Id, 450, 450,
                                                                           false, true, true);

                        pnlManagePhotos.Visible = false;
                        pnlCropImage.Visible = true;
                        mvPhotoAlbum.Visible = false;

                        ///Page.RegisterJQuery();
                        ((Site) Page.Master).ScriptManager.CompositeScript.Scripts.Add(
                            new ScriptReference("scripts/jquery.Jcrop.js"));

                        var jcropHandlerScript = String.Format(
                            "$(function(){{$('#{0}').Jcrop({{aspectRatio: 1,onSelect: updateCoords}});}}); " +
                            "function updateCoords(c){{$('#{1}').val(c.x);$('#{2}').val(c.y);" +
                            "$('#{3}').val(c.w);$('#{4}').val(c.h);}};",
                            imgFaceCropPhoto.ClientID, hidCropX.ClientID, hidCropY.ClientID,
                            hidCropW.ClientID, hidCropH.ClientID);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "jCropHandling",
                            jcropHandlerScript, true);
                        Page.Header.Controls.Add(new LiteralControl(
                            "<link href=\"images/jquery.Jcrop.css\" rel=\"stylesheet\" type=\"text/css\" />"));
                        hidCropPhotoId.Value = photoId.ToString();
                    }
                    else
                    {
                        loadPhotos = true;
                    }
                    break;

                case "AddNotes":
                    photoId = Convert.ToInt32(e.CommandArgument);
                    try
                    {
                        photo = Photo.Fetch(photoId);
                    }
                    catch (NotFoundException)
                    {
                        return;
                    }

                    if (Config.Photos.EnablePhotoNotes)
                    {
                        // Load cropping page
                        imgAddPhotoNote.Src = ImageHandler.CreateImageUrl(photo.Id, 450, 450,
                                                                           false, true, true);
                        imgNoteCropPreview.Src = imgAddPhotoNote.Src;

                        pnlManagePhotos.Visible = false;
                        pnlPhotoNote.Visible = true;
                        tblCropPreview.Visible = true;
                        mvPhotoAlbum.Visible = false;

                        //Page.RegisterJQuery();
                        //((Site) Page.Master).ScriptManager.CompositeScript.Scripts.Add(
                        //    new ScriptReference("scripts/jquery.Jcrop.js"));

                        var jcropHandlerScript = String.Format(
                            "$(function(){{$('#{0}').Jcrop({{onSelect: showPreview,onChange: showPreview}});}}); " +
                            "function showPreview(c){{" +
                            "if (c.w == 0 || c.h == 0) return;" +
                            "$('#tblCropPreview').show();" + 
                            "var pw = c.w >= c.h ? 90 : 90 * c.w / c.h;" +
                            "var ph = c.h >= c.w ? 90 : 90 * c.h / c.w;" +
                            "jQuery('#divNoteCropPreview').css({{" +
                            "	width: Math.round(pw) + 'px'," +
                            "	height: Math.round(ph) + 'px'" +
                            "}});" +
                            "var rx = pw / c.w;" +
                            "var ry = ph / c.h;" +
                            "jQuery('#{1}').css({{" +
                            "	width: Math.round(rx * $('#{0}').width()) + 'px'," +
                            "	height: Math.round(ry * $('#{0}').height()) + 'px'," +
                            "	marginLeft: '-' + Math.round(rx * c.x) + 'px'," +
                            "	marginTop: '-' + Math.round(ry * c.y) + 'px'" +
                            "}});" +
                            "$('#{2}').val(c.x);$('#{3}').val(c.y);" +
                            "$('#{4}').val(c.w);$('#{5}').val(c.h);" +
                            "}};",
                            imgAddPhotoNote.ClientID, imgNoteCropPreview.ClientID,
                            hidNoteX.ClientID, hidNoteY.ClientID, hidNoteW.ClientID, hidNoteH.ClientID);
                        ScriptManager.RegisterStartupScript(Page, typeof(Page), "jCropHandling2",
                            jcropHandlerScript, true);
                        Page.Header.Controls.Add(new LiteralControl(
                            "<link href=\"images/jquery.Jcrop.css\" rel=\"stylesheet\" type=\"text/css\" />"));
                        hidNotePhotoId.Value = photoId.ToString();
                        populateDDNoteFriends();
                        loadPhotoNotes(photoId);
                    }
                    else
                    {
                        loadPhotos = true;
                    }
                    break;
            }
        }

        void asyncImageUpload_UploadedComplete(object sender, AsyncFileUploadEventArgs e)
        {
            Image image;
            try
            {
                string savePath = MapPath("~/AsyncFileUpload/Uploads/" + Path.GetFileName(e.FileName));
                asyncImageUpload.SaveAs(savePath);
                image = Image.FromFile(savePath);
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "imgUpdate",
                    String.Format("top.document.getElementById('tempPhoto').src='./AsyncFileUpload/Uploads/{0}';",
                    e.FileName), true);
                //image = Image.FromStream
                  //  (asyncImageUpload.PostedFile.InputStream);
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

            #region Find faces
            if (Config.Photos.FindFacesForThumbnails)
            {
                FindFaces(photo);
            }

            #endregion

            photo.Image = null;
            Session["temp_photo"] = photo;

            divFileUploadControls.Style.Add("display","none");
            divImageRotateControls.Visible = true;
            
        }
        protected void asyncUploadPhoto_OnClientUploadCompleted(object sender, EventArgs e)
        {
            Response.BinaryWrite(asyncImageUpload.FileBytes);
        }
		
        /// <summary>
        /// Handles the Click event of the btnUpload control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
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

            #region Find faces

            if (Config.Photos.FindFacesForThumbnails)
            {
                FindFaces(photo);
            }

            #endregion

            photo.Image = null;
            Session["temp_photo"] = photo;

            divFileUploadControls.Visible = false;
            divImageRotateControls.Visible = true;
        }

        private void FindFaces(Photo photo)
        {
            try
            {
                FaceFinderPlugin.FaceRegion[] regions = Photo.FindFaceRegions(photo.Image);
                if (regions != null && regions.Length > 0)
                {
                    dlPhotoFaces.DataSource = regions;
                    dlPhotoFaces.DataBind();
                    plhPhotoFaces.Visible = true;

                    if (photo.FaceCrop != null)
                    {
                        for (int i = 0; i < dlPhotoFaces.Items.Count; i++)
                        {
                            var hid = (HiddenField)dlPhotoFaces.Items[i].FindControl("hidFace");
                            if (hid == null) continue;
                            if (photo.FaceCrop == hid.Value)
                            {
                                var rb = (RadioButton)dlPhotoFaces.Items[i].FindControl("rbFace");
                                if (rb == null) continue;
                                rb.Checked = true;
                                rbNoFace.Checked = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        rbNoFace.Checked = true;
                    }
                }
                else
                {
                    plhPhotoFaces.Visible = false;
                }
            }
            catch (Exception err)
            {
                Global.Logger.LogWarning("UploadPhoto_FindFace", err);
            }
        }

        /// <summary>
        /// Handles the Click event of the lnkSave control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
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

            photo.User = User;
            photo.Name = Config.Misc.EnableBadWordsFilterProfile ? Parsers.ProcessBadWords(txtName.Text) : txtName.Text;
            photo.Description = Config.Misc.EnableBadWordsFilterProfile
                                    ? Parsers.ProcessBadWords(txtDescription.Text)
                                    : txtDescription.Text;
            photo.PrivatePhoto = chkPrivatePhoto.Checked;
            if (ddPhotoAlbums.SelectedValue != "-1") photo.PhotoAlbumID = Convert.ToInt32(ddPhotoAlbums.SelectedValue);

            #region Save face coordinates

            if (Config.Photos.FindFacesForThumbnails && plhPhotoFaces.Visible)
            {
                if (rbNoFace.Checked)
                {
                    photo.FaceCrop = null;
                }
                else
                {
                    foreach (DataListItem item in dlPhotoFaces.Items)
                    {
                        var rb = (RadioButton)item.FindControl("rbFace");
                        if (rb != null && rb.Checked)
                        {
                            var hid = (HiddenField)item.FindControl("hidFace");
                            photo.FaceCrop = hid.Value;
                            break;
                        }
                    }
                }
            }

            #endregion

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

                string[] usernames = User.FetchMutuallyFriends(CurrentUserSession.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification)
                    {
                        string text = String.Format("Your friend {0} has uploaded a new photo".Translate(),
                                                  "<b>" + photo.Username + "</b>");
                        string thumbnailUrl = ImageHandler.CreateImageUrl(photo.Id, 50, 50, false, true, true);
                        User.SendOnlineEventNotification(photo.Username, friendUsername,
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

            pnlEditImage.Visible = false;
            dlPhotos.Visible = true;
            pnlManagePhotos.Visible = true;
            loadPhotos = true;
            LoadPhotos();
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

            pnlEditImage.Visible = false;
            dlPhotos.Visible = true;
            loadPhotos = true;
            mvPhotoAlbum.Visible = Config.Users.EnablePhotoAlbums;
        }

        private void dlPhotos_ItemCreated(object sender, DataListItemEventArgs e)
        {
            var lnkDeletePhoto = (LinkButton)e.Item.FindControl("lnkDeletePhoto");
            lnkDeletePhoto.Attributes.Add("onclick",
                                          String.Format("javascript: return confirm('{0}')",
                                                        Lang.Trans("Do you really want to delete this photo?")));
        }

        protected void lnkUploadMultipleFlash_Click(object sender, EventArgs e)
        {
            string photoAlbumID = ddPhotoAlbums.SelectedValue;

            Guid guid = Guid.NewGuid();

            flashUpload.QueryParameters = "type=photo&guid=" + guid;

            Cache.Insert("flashUpload_" + guid, CurrentUserSession.Username + "|" + photoAlbumID, null,
                         DateTime.Now.AddMinutes(30),
                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

            string jsscript = "<SCRIPT LANGUAGE=\"JavaScript\">" +
                                    "function flashUploadIsCompleted() {" +
                                    String.Format("window.location.href='ManageProfile.aspx?sel=photos&album={0}'", photoAlbumID) +
                                    "}" +
                                    "//  End -->" +
                                    "</SCRIPT>";

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "FlashUploadedIsCompleted", jsscript);

            lnkBackToPhotos.Visible = true;
            pnlManagePhotos.Visible = false;
            pnlUploadMultiplePhotosFlash.Visible = true;
            pnlUploadMultiplePhotosSilverlight.Visible = false;
            pnlUploadViaWebCam.Visible = false;
            mvPhotoAlbum.Visible = false;
        }

        protected void lnkUploadMultipleSilverlight_Click(object sender, EventArgs e)
        {
            string photoAlbumID = ddPhotoAlbums.SelectedValue;

            Guid guid = Guid.NewGuid();

            Silverlight1.InitParameters = "type=photo" + ",guid" + "=" + guid;
            Cache.Insert("silverlightUpload_" + guid, CurrentUserSession.Username + "|" + photoAlbumID, null,
                         DateTime.Now.AddMinutes(30),
                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

            string jsscript = "<SCRIPT LANGUAGE=\"JavaScript\">" +
                                    "function silverlightUploadIsCompleted() {" +
                                    String.Format("window.location.href='ManageProfile.aspx?sel=photos&album={0}'", photoAlbumID) +
                                    "}" +
                                    "//  End -->" +
                                    "</SCRIPT>";

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "SilverlightUploadedIsCompleted", jsscript);

            lnkBackToPhotos.Visible = true;
            pnlManagePhotos.Visible = false;
            pnlUploadMultiplePhotosFlash.Visible = false;
            pnlUploadMultiplePhotosSilverlight.Visible = true;
            pnlUploadViaWebCam.Visible = false;
            mvPhotoAlbum.Visible = false;
        }

        protected void lnkUploadViaWebCam_Click(object sender, EventArgs e)
        {
            string photoAlbumID = ddPhotoAlbums.SelectedValue;

            Guid guid = Guid.NewGuid();

            webcamUpload.QueryParameters = "guid=" + guid;

            Cache.Insert("webcamUpload_" + guid, CurrentUserSession.Username + "|" + photoAlbumID, null,
                         DateTime.Now.AddMinutes(30),
                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

            string jsscript = "<SCRIPT LANGUAGE=\"JavaScript\">" +
                                    "function webcamUploadIsCompleted() {" +
                                    String.Format("window.location.href='ManageProfile.aspx?sel=photos&album={0}'", photoAlbumID) +
                                    "}" +
                                    "//  End -->" +
                                    "</SCRIPT>";

            Page.ClientScript.RegisterClientScriptBlock(GetType(), "WebcamUploadedIsCompleted", jsscript);

            lnkBackToPhotos.Visible = true;
            pnlManagePhotos.Visible = false;
            pnlUploadMultiplePhotosFlash.Visible = false;
            pnlUploadMultiplePhotosSilverlight.Visible = false;
            pnlUploadViaWebCam.Visible = true;
            mvPhotoAlbum.Visible = false;
        }

        protected void lnkBackToPhotos_Click(object sender, EventArgs e)
        {
            loadPhotos = true;
            lnkBackToPhotos.Visible = false;
            pnlManagePhotos.Visible = true;
            pnlUploadMultiplePhotosFlash.Visible = false;
            pnlUploadMultiplePhotosSilverlight.Visible = false;
            pnlUploadViaWebCam.Visible = false;
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dlPhotos.ItemCreated += new DataListItemEventHandler(dlPhotos_ItemCreated);
            this.dlPhotos.ItemCommand +=
                new System.Web.UI.WebControls.DataListCommandEventHandler(this.dlPhotos_ItemCommand);
        }

        #endregion

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

        protected void lnkCreateNewAlbum_Click(object sender, EventArgs e)
        {
            EditedPhotoAlbumID = null;
            txtPhotoAlbumName.Text = String.Empty;
            mvPhotoAlbum.SetActiveView(viewCreatePhotoAlbum);
            dlPhotos.DataSource = null;
            dlPhotos.DataBind();
            pnlManagePhotos.Visible = false;
        }

        protected void ddPhotoAlbums_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadPhotos = true;
        }

        protected void btnCreateEditPhotoAlbum_Click(object sender, EventArgs e)
        {
            string photoAlbumName = txtPhotoAlbumName.Text.Trim();
            if (photoAlbumName.Length == 0)
            {
                lblError.Text = "Please specify album name!".Translate();
                return;
            }

            PhotoAlbum photoAlbum = null;
            if (!EditedPhotoAlbumID.HasValue)
                photoAlbum = new PhotoAlbum(CurrentUserSession.Username)
                {
                    Name = photoAlbumName,
                    Access = (PhotoAlbum.eAccess)Convert.ToInt32(ddPhotoAlbumAccess.SelectedValue)
                };
            else
            {
                photoAlbum = PhotoAlbum.Fetch(EditedPhotoAlbumID.Value);
                photoAlbum.Name = photoAlbumName;
                photoAlbum.Access = (PhotoAlbum.eAccess) Convert.ToInt32(ddPhotoAlbumAccess.SelectedValue);
            }

            photoAlbum.Save();
            populateDDPhotoAlbums();
            ddPhotoAlbums.SelectedValue = photoAlbum.ID.ToString();
            pnlManagePhotos.Visible = true;
            loadPhotos = true;
            EditedPhotoAlbumID = null;
        }

        protected void btnCancelCreatePhotoAlbum_Click(object sender, EventArgs e)
        {
            mvPhotoAlbum.SetActiveView(viewPhotoAlbums);
            pnlManagePhotos.Visible = true;
            loadPhotos = true;
        }

        private void populateDDPhotoAlbums()
        {
            ddPhotoAlbums.Items.Clear();
            ddPhotoAlbums.Items.Add(new ListItem(
                                        String.Format("{0}'s Photos".Translate(), CurrentUserSession.Username), "-1"));
            PhotoAlbum[] photoAlbums = PhotoAlbum.Fetch(CurrentUserSession.Username);
            foreach (PhotoAlbum photoAlbum in photoAlbums)
            {
                ddPhotoAlbums.Items.Add(new ListItem(photoAlbum.Name, photoAlbum.ID.ToString()));
            }
        }

        private void populateDDNoteFriends()
        {
            ddNoteFriend.Items.Clear();
            ddNoteFriend.Items.Add("No".Translate());
            var friends = User.FetchMutuallyFriends(CurrentUserSession.Username);
            foreach (var friend in friends)
            {
                ddNoteFriend.Items.Add(friend);
            }
        }

        private void loadPhotoNotes(int photoId)
        {
            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("ImageCropUrl", typeof(string));
            dt.Columns.Add("Username", typeof(string));
            dt.Columns.Add("Notes", typeof(string));

            var notes = PhotoNote.Load(null, photoId, null);
            foreach (var note in notes)
            {
                var cropImageUrl = String.Format("Image.ashx?id={0}&width=50&height=50&findFace=1" +
                                                 "&faceX={1}&faceY={2}&faceW={3}&faceH={4}&exactCrop=1",
                                                 photoId, note.X, note.Y, note.Width, note.Height);
                dt.Rows.Add(new object[] {note.Id, cropImageUrl, note.Username, note.Notes});
            }

            rptNotes.DataSource = dt;
            rptNotes.DataBind();
        }

        protected void lnkSaveFaceCrop_Click(object sender, EventArgs e)
        {
            var photoId = Convert.ToInt32(hidCropPhotoId.Value);
            Photo photo;
            try
            {
                photo = Photo.Fetch(photoId);
                int cropX, cropY, cropW, cropH;
                if (int.TryParse(hidCropX.Value, out cropX) && int.TryParse(hidCropY.Value, out cropY)
                    && int.TryParse(hidCropW.Value, out cropW) && int.TryParse(hidCropH.Value, out cropH))
                {
                    photo.FaceCrop = String.Join("|", new[] {cropX.ToString(), cropY.ToString(),
                        cropW.ToString(), cropH.ToString()});
                    photo.Save(false);
                }
            }
            catch (NotFoundException)
            {
                return;
            }
            finally
            {
                loadPhotos = true;
                pnlManagePhotos.Visible = true;
                pnlCropImage.Visible = false;
                mvPhotoAlbum.Visible = Config.Users.EnablePhotoAlbums;
            }
        }

        protected void lnkCancelFaceCrop_Click(object sender, EventArgs e)
        {
            loadPhotos = true;
            pnlManagePhotos.Visible = true;
            pnlCropImage.Visible = false;
            mvPhotoAlbum.Visible = Config.Users.EnablePhotoAlbums;
        }

        protected void btnAddNote_Click(object sender, EventArgs e)
        {
            var note = new PhotoNote();
            if (txtNoteText.Text.Trim().Length > 0)
                note.Notes = txtNoteText.Text;
            if (ddNoteFriend.SelectedIndex > 0)
            {
                note.Username = ddNoteFriend.SelectedValue;
            }

            int noteX, noteY, noteW, noteH, notePhotoId;
            if (int.TryParse(hidNoteX.Value, out noteX) && int.TryParse(hidNoteY.Value, out noteY)
                && int.TryParse(hidNoteW.Value, out noteW) && int.TryParse(hidNoteH.Value, out noteH)
                && int.TryParse(hidNotePhotoId.Value, out notePhotoId)
                && !(note.Notes == null && note.Username == null))
            {
                note.X = noteX;
                note.Y = noteY;
                note.Width = noteW;
                note.Height = noteH;
                note.PhotoId = notePhotoId;

                txtNoteText.Text = String.Empty;
                ddNoteFriend.SelectedIndex = 0;
            }
            else
            {
                return;
            }
            note.Timestamp = DateTime.Now;
            note.Save();

            if (note.Username != null)
                AddTaggedOnPhotoEvent(note.Username, note.Id, note.PhotoId);

            loadPhotoNotes(notePhotoId);
            lnkDoneAddingPhotoNotes_Click(sender, e);
        }

        protected void lnkDeleteAlbum_Click(object sender, EventArgs e)
        {
            PhotoAlbum.Delete(Convert.ToInt32(ddPhotoAlbums.SelectedValue));

            populateDDPhotoAlbums();
            ddPhotoAlbums.SelectedValue = "-1"; // select the default album
            loadPhotos = true;
        }

        protected void rptNotes_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteNote":
                    int noteId = Convert.ToInt32(e.CommandArgument);
                    var notes = PhotoNote.Load(noteId, null, null);
                    if (notes.Length > 0)
                    {
                        notes[0].Delete();
                    }
                    loadPhotoNotes(Convert.ToInt32(hidNotePhotoId.Value));
                    break;
            }
        }

        protected void lnkDoneAddingPhotoNotes_Click(object sender, EventArgs e)
        {
            loadPhotos = true;
            pnlManagePhotos.Visible = true;
            pnlPhotoNote.Visible = false;
            mvPhotoAlbum.Visible = Config.Users.EnablePhotoAlbums;
        }

        private void AddTaggedOnPhotoEvent(string username, int noteID, int photoID)
        {
            Photo photo = null;
            try
            {
                photo = Photo.Fetch(photoID);
            }
            catch (NotFoundException) { return; }

            if (!photo.PrivatePhoto)
            {
                #region Add TaggedOnPhoto Event

                Event newEvent = new Event(CurrentUserSession.Username);

                newEvent.Type = Event.eType.TaggedOnPhoto;
                TaggedOnPhoto taggedPhoto = new TaggedOnPhoto();
                taggedPhoto.NoteID = noteID;
                newEvent.DetailsXML = Misc.ToXml(taggedPhoto);

                newEvent.Save();

                if (Config.Users.NewEventNotification)
                {
                    string text = String.Format("Your friend {0} has tagged you on their photo".Translate(),
                                              "<b>" + CurrentUserSession.Username + "</b>");
                    string thumbnailUrl = ImageHandler.CreateImageUrl(photoID, 50, 50, false, true, true);
                    User.SendOnlineEventNotification(CurrentUserSession.Username, username,
                                                             text, thumbnailUrl,
                                                             UrlRewrite.CreateShowUserPhotosUrl(CurrentUserSession.Username));
                }

                #endregion 
            }
        }

        protected void lnkEditAlbum_Click(object sender, EventArgs e)
        {
            if (ddPhotoAlbums.SelectedValue != "-1")
            {
                EditedPhotoAlbumID = Convert.ToInt32(ddPhotoAlbums.SelectedValue);
                PhotoAlbum album = PhotoAlbum.Fetch(EditedPhotoAlbumID.Value);
                if (album != null)
                {
                    txtPhotoAlbumName.Text = album.Name;
                    ddPhotoAlbumAccess.SelectedValue = ((int) album.Access).ToString();
                    mvPhotoAlbum.SetActiveView(viewCreatePhotoAlbum);
                    dlPhotos.DataSource = null;
                    dlPhotos.DataBind();
                    pnlManagePhotos.Visible = false;
                }
            }
        }
    }
}