/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * http://www.aspnetdating.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at http://www.aspnetdating.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using ezFixUp.Classes;
using ezFixUp.Components;

namespace ezFixUp
{
    public partial class MemberProfile : PageBase
    {
        protected SmallBoxStart SmallBoxStart1;

        protected void Page_Load(object sender, EventArgs e)
        {
            Settings1.SettingsSaved += Settings1_SettingsSaved;
            if (!Page.IsPostBack)
            {
                LoadStrings();
                LoadData();

                switch (Request.Params["sel"])
                {
                    case "payment":
                        lnkSubscription_Click(null, null);
                        break;
                    case "photos":
                        lnkUploadPhotos_Click(null, null);
                        break;
                    case "set":
                        lnkSettings_Click(null, null);
                        break;
                    case "videouploads":
                        lnkUploadVideo_Click(null, null);
                        break;
                    default:
                        lnkEditProfile_Click(null, null);
                        break;
                }
            }
        }

        void Settings1_SettingsSaved(object sender, EventArgs e)
        {
            pnlEditSkin.Visible = Config.Users.EnableProfileSkins && ((CurrentUserSession.Level != null &&
                                                      CurrentUserSession.Level.Restrictions.UserCanUseSkin)
                                                    || CurrentUserSession.CanUseSkin() == PermissionCheckResult.Yes)
                                   && ((CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanEditSkin)
                                   || CurrentUserSession.CanEditSkin() == PermissionCheckResult.Yes) && !String.IsNullOrEmpty(CurrentUserSession.ProfileSkin);
        }

        private void LoadData()
        {
            EditProfile1.User = CurrentUserSession;
            ViewProfile1.User = CurrentUserSession;
            EditPhotos1.User = CurrentUserSession;
            ViewPhotos1.User = CurrentUserSession;
            ViewEvents1.User = CurrentUserSession;
            PrivacySettings1.User = CurrentUserSession;
            Settings1.User = CurrentUserSession;
            Billing1.User = CurrentUserSession;
            EditSkin1.User = CurrentUserSession;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        private void LoadStrings()
        {
            lnkEditProfile.Text = Lang.Trans("Edit Profile");
            lnkViewProfile.Text = Lang.Trans("View Profile");
            lnkUploadPhotos.Text = Lang.Trans("Upload Photos");
            lnkUploadSalutePhoto.Text = Lang.Trans("Upload Salute Photo");
            lnkViewPhotos.Text = Lang.Trans("View Photos");
            lnkViewEvents.Text = Lang.Trans("View Events");
            lnkPrivacySettings.Text = "Privacy Settings".Translate();
            lnkSettings.Text = Lang.Trans("Settings");
            lnkGadgets.Text = Lang.Trans("Gadgets");
            lnkEditSkin.Text = Lang.Trans("Edit Skin");
            lnkUploadVideo.Text = Lang.Trans("Upload Video");
            lnkUploadAudio.Text = Lang.Trans("Upload Audio");

            if (Config.Misc.SiteIsPaid &&
                !(Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female))
            {
                if (lnkSubscription != null)
                {
                    lnkSubscription.Text = Lang.Trans("Billing");
                }
            }
            else if (trSubscription != null)
            {
                trSubscription.Visible = false;
            }

            if (!Config.Misc.EnableAudioUpload)
            {
                pnlAudioUpload.Visible = false;
            }

            if (!Config.Misc.EnableGadgets)
            {
                trGadgets.Visible = false;
            }

            if (SmallBoxStart1 != null)
                SmallBoxStart1.Title = Lang.Trans("Profile Management");

            if (!Config.Misc.EnableVideoProfile && !Config.Misc.EnableVideoUpload && !Config.Misc.EnableYouTubeVideos)
                pnlUploadVideo.Visible = false;

            if (Config.Photos.EnableSalutePhoto) pnlSalutePhoto.Visible = true;

            pnlEditSkin.Visible = Config.Users.EnableProfileSkins && ((CurrentUserSession.Level != null &&
                                                      CurrentUserSession.Level.Restrictions.UserCanUseSkin)
                                                    || CurrentUserSession.CanUseSkin() == PermissionCheckResult.Yes)
                                   && ((CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.UserCanEditSkin)
                                   || CurrentUserSession.CanEditSkin() == PermissionCheckResult.Yes) && !String.IsNullOrEmpty(CurrentUserSession.ProfileSkin);
        }

        private void EnableSideLinks()
        {
            lnkEditProfile.Enabled = true;
            lnkViewProfile.Enabled = true;
            lnkUploadPhotos.Enabled = true;
            lnkUploadSalutePhoto.Enabled = true;
            lnkViewPhotos.Enabled = true;
            lnkViewEvents.Enabled = true;
            lnkSettings.Enabled = true;
            lnkPrivacySettings.Enabled = true;
            lnkSubscription.Enabled = true;
            lnkUploadAudio.Enabled = true;
            lnkUploadVideo.Enabled = true;
            lnkGadgets.Enabled = true;
            lnkEditSkin.Enabled = true;
        }

        private void HideControls()
        {
            EditPhotos1.Visible = false;
            EditProfile1.Visible = false;
            Billing1.Visible = false;
            UploadVideo1.Visible = false;
            UploadAudio1.Visible = false;
            PrivacySettings1.Visible = false;
            Settings1.Visible = false;
            ViewPhotos1.Visible = false;
            ViewEvents1.Visible = false;
            ViewProfile1.Visible = false;
            Gadgets1.Visible = false;
            EditSkin1.Visible = false;
        }

        protected void lnkEditProfile_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkEditProfile.Enabled = false;

            HideControls();
            EditProfile1.Visible = true;
        }

        protected void lnkViewProfile_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkViewProfile.Enabled = false;

            HideControls();
            ViewProfile1.Visible = true;
        }

        protected void lnkUploadPhotos_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkUploadPhotos.Enabled = false;

            HideControls();
            EditPhotos1.Visible = true;
        }

        protected void lnkViewPhotos_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkViewPhotos.Enabled = false;

            HideControls();
            ViewPhotos1.Visible = true;
        }

        protected void lnkSettings_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkSettings.Enabled = false;

            HideControls();
            Settings1.Visible = true;
        }

        protected void lnkSubscription_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkSubscription.Enabled = false;

            HideControls();
            Billing1.Visible = true;
        }

        protected void lnkUploadVideo_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkUploadVideo.Enabled = false;

            HideControls();
            UploadVideo1.Visible = true;
            UploadVideo1.FirstLoad = true;
            UploadVideo1.InitControl();
        }

        protected void lnkGadgets_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkGadgets.Enabled = false;

            HideControls();
            Gadgets1.Visible = true;
        }

        protected void lnkUploadAudio_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkUploadAudio.Enabled = false;

            HideControls();
            UploadAudio1.Visible = true;
        }

        protected void lnkEditSkin_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkEditSkin.Enabled = false;

            HideControls();
            EditSkin1.Visible = true;
        }

        protected void lnkPrivacySettings_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkPrivacySettings.Enabled = false;

            HideControls();
            PrivacySettings1.Visible = true;
        }

        protected void lnkViewEvents_Click(object sender, EventArgs e)
        {
            EnableSideLinks();
            lnkViewEvents.Enabled = false;

            HideControls();
            ViewEvents1.Visible = true;
        }

        protected void lnkUploadSalutePhoto_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/EditSalutePhoto.aspx");
        }
    }
}