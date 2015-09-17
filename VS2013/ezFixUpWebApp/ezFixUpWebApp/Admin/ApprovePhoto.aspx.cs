/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Admin
{
    /// <summary>
    /// Summary description for ApprovePhoto.
    /// </summary>
    public partial class ApprovePhoto : AdminPageBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApprovePhoto"/> class.
        /// </summary>
        public ApprovePhoto()
        {
            RequiresAuthorization = true;
        }


        private int photoID;

        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                photoID = Convert.ToInt32(Request.Params["pid"]);
            }
            catch (Exception)
            {
                return;
            }

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnApprove.Enabled = false;
                    btnReject.Enabled = false;
                }
                
                LoadStrings();
                btnReject.Attributes.Add("onclick",
                                         String.Format("javascript: return confirm('{0}')",
                                                       Lang.TransA("Do you really want to reject this photo?")));

                if (!Config.Photos.EnableExplicitPhotos)
                    chkExplicitPhoto.Visible = false;

                LoadPage();
            }
        }

        private void LoadStrings()
        {
            btnApprove.Text = Lang.TransA("Approve");
            btnReject.Text = Lang.TransA("Reject");
            btnCancel.Text = Lang.TransA("Cancel");
            chkExplicitPhoto.Text = Lang.TransA("Explicit Photo");
        }

        private void LoadPage()
        {
            try
            {
                imgBigPhoto.ImageUrl = String.Format("{0}{1}{2}", Config.Urls.Home, "/Image.ashx?id=", photoID);
            }
            catch (InvalidCastException)
            {
            }

            Photo photo = Photo.Fetch(Convert.ToInt32(photoID));

            if (photo != null)
            {
                lnkUsername.InnerText = photo.User.Username;
                lnkUsername.HRef = "~/ShowUser.aspx?uid=" + photo.User.Username;
                lblPhotoName.Text = photo.Name;
                lblPhotoDescription.Text = photo.Description;
            }
        }

        #region Web Form Designer generated code

        /// <summary>
        /// Raises the <see cref="E:Init"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.photoApproval;
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

        /// <summary>
        /// Handles the Click event of the btnApprove control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            Photo photo = Photo.Fetch(photoID);
            if (Config.Photos.EnableExplicitPhotos)
            {
                photo.ExplicitPhoto = chkExplicitPhoto.Checked;
                if (photo.ExplicitPhoto && Config.Photos.MakeExplicitPhotosPrivate)
                {
                    photo.PrivatePhoto = true;
                }
            }

            if (photo.ManualApproval)
            {
                photo.ManualApproval = false;
                photo.Save(false);
                CommunityPhotoApproval.DeleteByPhotoID(photo.Id);
                Classes.User.SetPhotoModerationApprovalScore(photo.Id, true,
                                                                 Config.CommunityModeratedSystem.ScoresForCorrectOpinion,
                                                                 Config.CommunityModeratedSystem.PenaltyForIncorrectOpinion);
            }

            photo.ApprovePhoto(CurrentAdminSession.Username);

            if (!photo.PrivatePhoto)
            {
                #region Add Event

                Event newEvent = new Event(photo.Username);

                newEvent.Type = Event.eType.NewFriendPhoto;
                NewFriendPhoto newFriendPhoto = new NewFriendPhoto();
                newFriendPhoto.PhotoID = photo.Id;
                newEvent.DetailsXML = Misc.ToXml(newFriendPhoto);

                newEvent.Save();

                string[] usernames = Classes.User.FetchMutuallyFriends(photo.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification)
                    {
                        string text = String.Format("Your friend {0} has uploaded a new photo".TranslateA(),
                                                      "<b>" + photo.Username + "</b>");
                        string thumbnailUrl = ImageHandler.CreateImageUrl(photo.Id, 50, 50, false, true, true);
                        Classes.User.SendOnlineEventNotification(photo.Username, friendUsername,
                                                                 text, thumbnailUrl,
                                                                 UrlRewrite.CreateShowUserPhotosUrl(photo.Username));
                    }
                }

                #endregion
            }
            

            Classes.User.AddScore(photo.Username, Config.UserScores.ApprovedPhoto, "ApprovedPhoto");
            try
            {
                MiscTemplates.ApprovePhotoMessage approvePhotoMessageTemplate =
                    new MiscTemplates.ApprovePhotoMessage(photo.User.LanguageId);
                Message.Send(Config.Users.SystemUsername, photo.User.Username, approvePhotoMessageTemplate.Message, 0);
            }
            catch (NotFoundException ex)
            {
                Log(ex);
            }

            Response.Redirect("ApprovePhotos.aspx");
        }

        /// <summary>
        /// Handles the Click event of the btnReject control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            Photo photo = Photo.Fetch(photoID);
            MiscTemplates.RejectPhotoMessage rejectPhotoMessageTemplate =
                new MiscTemplates.RejectPhotoMessage(photo.User.LanguageId);
            string reasonMessage = rejectPhotoMessageTemplate.WithReasonMessage;

            if (txtReason.Text != "")
            {
                reasonMessage = reasonMessage.Replace("%%REASON%%", txtReason.Text);
                try
                {
                    Message.Send(Config.Users.SystemUsername, photo.User.Username, reasonMessage, 0);
                }
                catch (NotFoundException) { }
            }
            else
            {
                try
                {
                    Message.Send(Config.Users.SystemUsername, photo.User.Username,
                                 rejectPhotoMessageTemplate.WithNoReasonMessage, 0);
                }
                catch(NotFoundException){}
            }

            if (photo.ManualApproval)
            {
                Classes.User.SetPhotoModerationApprovalScore(photo.Id, false,
                                                                 Config.CommunityModeratedSystem.ScoresForCorrectOpinion,
                                                                 Config.CommunityModeratedSystem.PenaltyForIncorrectOpinion);
            }
            
            Photo.Delete(photoID);
            Classes.User.AddScore(photo.Username, Config.UserScores.RejectedPhoto, "RejectedPhoto");

            Response.Redirect("ApprovePhotos.aspx");
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ApprovePhotos.aspx");
        }
    }
}