using System;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class ReviewNewPhotos : PageBase
    {
        protected int CurrentPhotoID
        {
            get
            {
                if (ViewState["CurrentPhotoID"] != null)
                {
                    return (int) ViewState["CurrentPhotoID"];
                }
                else
                {
                    return -1;
                }
            }

            set { ViewState["CurrentPhotoID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.CommunityModeratedSystem.EnableCommunityPhotoApproval
                                            || (CurrentUserSession.Level != null && !CurrentUserSession.Level.Restrictions.AllowToModeratePhotos)
                                            || CurrentUserSession.ModerationScores <
                                            Config.CommunityModeratedSystem.MinimumScoresToAllowModeration)
                {
                    StatusPageMessage = Lang.Trans("You are not allowed to moderate photos!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                loadStrings();
                loadNonApprovedPhoto();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Stats");
            SmallBoxStart2.Title = Lang.Trans("Rules");
            LargeBoxStart1.Title = Lang.Trans("Review new photos");
            btnApprove.Text = Lang.Trans("Approve");
            btnReject.Text = Lang.Trans("Reject");
            btnPass.Text = Lang.Trans("Pass");
        }

        private void loadNonApprovedPhoto()
        {
            Classes.User user = null;

            try
            {
                user = Classes.User.Load(CurrentUserSession.Username);
            }
            catch (NotFoundException) 
            {
                return;
            }

            CurrentUserSession.ModerationScores = user.ModerationScores;

            if (user.ModerationScores < Config.CommunityModeratedSystem.MinimumScoresToAllowModeration)
            {
                StatusPageMessage = Lang.Trans("You are not allowed to moderate photos!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }

            Photo photo = Photo.GetNonApproved(CurrentUserSession.Username);

            if (photo != null)
            {
                imgPhoto.ImageUrl = String.Format("~/Image.ashx?id={0}&width=450&height=450", photo.Id);
                lblPhotoName.Text = photo.Name;
                lblPhotoDescription.Text = photo.Description;
                lnkUsername.InnerText = photo.Username;
                lnkUsername.HRef = "~/ShowUser.aspx?uid=" + photo.Username;
                pnlPhotoName.Visible = photo.Name != String.Empty;
                pnlPhotoDescription.Visible = photo.Description != String.Empty;
                pnlUsername.Visible = true;
                btnApprove.Visible = true;
                btnReject.Visible = true;
                btnPass.Visible = true;
                CurrentPhotoID = photo.Id;

                // HACK: On image load call the "SetHeight()" javascript function to fix the layout
                imgPhoto.Attributes.Add("onload", "SetHeight();");
            }
            else
            {
                lblError.Text = Lang.Trans("There are no photos waiting for approval!");
                imgPhoto.Visible = false;
                pnlPhotoName.Visible = false;
                pnlPhotoDescription.Visible = false;
                pnlUsername.Visible = false;
                btnApprove.Visible = false;
                btnReject.Visible = false;
                btnPass.Visible = false;
                CurrentPhotoID = -1;
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (CurrentPhotoID != -1)
            {
                CommunityPhotoApproval cma = new CommunityPhotoApproval(CurrentPhotoID, CurrentUserSession.Username);
                cma.Approved = true;
                cma.Save();

                determinePhotoState();
                loadNonApprovedPhoto();
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (CurrentPhotoID != -1)
            {
                CommunityPhotoApproval cma = new CommunityPhotoApproval(CurrentPhotoID, CurrentUserSession.Username);
                cma.Save();

                determinePhotoState();
                loadNonApprovedPhoto();    
            }
            
        }

        protected void btnPass_Click(object sender, EventArgs e)
        {
            loadNonApprovedPhoto();
        }

        private void determinePhotoState()
        {
            CommunityPhotoApproval[] approvals = CommunityPhotoApproval.FetchByPhotoID(CurrentPhotoID);
            int votes = approvals.Length;

            if (votes == Config.CommunityModeratedSystem.RequiredNumberOfVotesToDetermine)
            {
                Photo photo = null;
                int approved = 0;
                int rejected = 0;

                foreach (CommunityPhotoApproval approval in approvals)
                {
                    if (approval.Approved) approved++;
                    else rejected++;
                }

                try
                {
                    photo = Photo.Fetch(CurrentPhotoID);
                }
                catch (NotFoundException)
                {
                    return;
                }

                if ((approved * 100) / Config.CommunityModeratedSystem.RequiredNumberOfVotesToDetermine >= Config.CommunityModeratedSystem.RequiredPercentageToApprovePhoto)
                {
                    photo.Approved = true;
                    photo.ApprovedDate = DateTime.Now;
                    photo.Save(false);

                    Classes.User.SetPhotoModerationApprovalScore(CurrentPhotoID, true,
                                                                 Config.CommunityModeratedSystem.ScoresForCorrectOpinion,
                                                                 Config.CommunityModeratedSystem.PenaltyForIncorrectOpinion);

                    CommunityPhotoApproval.DeleteByPhotoID(CurrentPhotoID);

                    MiscTemplates.ApprovePhotoMessage approvePhotoMessageTemplate =
                    new MiscTemplates.ApprovePhotoMessage(photo.User.LanguageId);
                    Message.Send(Config.Users.SystemUsername, photo.User.Username, approvePhotoMessageTemplate.Message, 0);

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
                    
                }
                else if ((rejected * 100) / Config.CommunityModeratedSystem.RequiredNumberOfVotesToDetermine >= Config.CommunityModeratedSystem.RequiredPercentageToRejectPhoto)
                {
                    Classes.User.SetPhotoModerationApprovalScore(CurrentPhotoID, false,
                                                                 Config.CommunityModeratedSystem.ScoresForCorrectOpinion,
                                                                 Config.CommunityModeratedSystem.PenaltyForIncorrectOpinion);

                    Photo.Delete(CurrentPhotoID);

                    MiscTemplates.RejectPhotoMessage rejectPhotoMessageTemplate =
                        new MiscTemplates.RejectPhotoMessage(photo.User.LanguageId);
                    Message.Send(Config.Users.SystemUsername, photo.User.Username,
                                 rejectPhotoMessageTemplate.WithNoReasonMessage, 0);
                }
                else
                {
                    photo.ManualApproval = true;
                    photo.Save(false);
                }
            }
        }
    }
}
