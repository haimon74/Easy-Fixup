using System;
using System.Collections;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using AspNetDating.Classes;
using AspNetDating.Components;

namespace AspNetDating
{
    public partial class ReviewNewUsers : PageBase
    {
        private User currentNewUser;
        protected User CurrentNewUser
        {
            get
            {
                if (currentNewUser == null && ViewState["NewUserUsername"] != null)
                {
                    try
                    {
                        currentNewUser = Classes.User.Load((string)ViewState["NewUserUsername"]);    
                    }
                    catch (NotFoundException)
                    {
                        currentNewUser = null;
                    }
                }

                return currentNewUser;
            }

            set
            {
                currentNewUser = value;
                if (currentNewUser != null)
                {
                    ViewState["NewUserUsername"] = currentNewUser.Username;
                }
                else
                {
                    ViewState["NewUserUsername"] = null;
                }
            }
        }

        private int CurrentPhotoId
        {
            get
            {
                if (ViewState["CurrentPhotoId"] != null)
                {
                    return (int)ViewState["CurrentPhotoId"];
                }
                else
                {
                    return -1;
                }
            }
            set { ViewState["CurrentPhotoId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.CommunityFaceControlSystem.EnableCommunityFaceControl
                                            || (CurrentUserSession.Level != null && !CurrentUserSession.Level.Restrictions.AllowToParticipateInFaceControl)
                                            || CurrentUserSession.ModerationScores <
                                            Config.CommunityFaceControlSystem.MinimumScoresToAllowModeration)
                {
                    StatusPageMessage = Lang.Trans("You are not allowed to moderate profiles!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                loadStrings();
                loadNewUser(true);
            }
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Stats");
            LargeBoxStart1.Title = Lang.Trans("Review new users");
            btnApprove.Text = Lang.Trans("Approve");
            btnReject.Text = Lang.Trans("Reject");
            btnPass.Text = Lang.Trans("Pass");
        }

        private void loadNewUser(bool loadNewUser)
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

            if (user.ModerationScores < Config.CommunityFaceControlSystem.MinimumScoresToAllowModeration)
            {
                StatusPageMessage = Lang.Trans("You are not allowed to moderate profiles!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }

            Classes.User.eGender? gender = null;
            if (Config.CommunityFaceControlSystem.ShowOnlyGenderOfInterest)
            {
                if (Config.Users.InterestedInFieldEnabled)
                {
                    gender = CurrentUserSession.InterestedIn;
                }
                else
                {
                    gender = CurrentUserSession.Gender == Classes.User.eGender.Male
                                 ? Classes.User.eGender.Female
                                 : Classes.User.eGender.Male;
                }
            }

            if (loadNewUser)
            {
                CurrentNewUser =
                Classes.User.GetNonFaceControlApprovedUser(CurrentUserSession.Username, gender,
                                                           Config.CommunityFaceControlSystem.MinPhotosRequired);
            }

            if (CurrentNewUser != null)
            {
                ltrPhoto.Text = String.Empty;
                loadPhotos();

                #region Load answers

                ProfileTopic[] profileTopics = ProfileTopic.Fetch();
                if (profileTopics != null)
                {
                    foreach (ProfileTopic topic in profileTopics)
                    {
                        ProfileQuestion[] questions = topic.FetchQuestions();
                        if (questions == null)
                            continue;
                        else
                        {
                            bool topicHasQuestion = false;
                            foreach (ProfileQuestion question in questions)
                            {
                                if (question.ShowStyle != ProfileQuestion.eShowStyle.Slogan)
                                {
                                    topicHasQuestion = true;
                                    break;
                                }
                                else
                                {
                                    #region setting up the Slogan

                                    try
                                    {
                                        ProfileAnswer answer = question.FetchAnswer(CurrentNewUser.Username);

                                        if (answer.Value == String.Empty)
                                            hlSlogan.Title = Lang.Trans("-- no headline --");
                                        else
                                        {
                                            if (answer.Approved)
                                                hlSlogan.Title = Server.HtmlEncode(answer.Value);
                                            else
                                                hlSlogan.Title = Lang.Trans("-- pending approval --");
                                        }
                                    }
                                    catch (NotFoundException)
                                    {
                                        hlSlogan.Title = Lang.Trans("-- no headline --");
                                    }

                                    #endregion
                                }
                            }

                            if (!topicHasQuestion)
                                continue;
                        }

                        HeaderLine lbs = (HeaderLine)LoadControl
                                                          ("~/Components/HeaderLine.ascx");
                        if (Config.Misc.EnableProfileDataTranslation)
                            lbs.Title = Lang.Trans(topic.Name);
                        else
                            lbs.Title = topic.Name;
                        plhProfile.Controls.Add(lbs);

                        ControlCollection ctlQuestions = null;
                        Table tblColumns = new Table();
                        tblColumns.CellPadding = 0;
                        tblColumns.CellSpacing = 0;
                        tblColumns.CssClass = "wrap-view";
                        plhProfile.Controls.Add(tblColumns);
                        TableRow tblRow = new TableRow();
                        tblColumns.Rows.Add(tblRow);
                        TableCell tblCell = null;
                        int questionNo = 0;

                        bool hasVisibleQuestions = false;

                        #region Adding Questions

                        foreach (ProfileQuestion question in questions)
                        {
                            if (!question.IsVisible(CurrentNewUser.Gender))
                            {
                                continue;
                            }
                            if (question.ShowStyle == ProfileQuestion.eShowStyle.Hidden ||
                                question.ShowStyle == ProfileQuestion.eShowStyle.Slogan)
                            {
                                continue;
                            }

                            hasVisibleQuestions = true;

                            tblCell = new TableCell();
                            tblCell.VerticalAlign = VerticalAlign.Top;
                            ctlQuestions = tblCell.Controls;

                            IProfileAnswerComponent cProfile = null;

                            switch (question.ShowStyle)
                            {
                                case ProfileQuestion.eShowStyle.SingleChoice:
                                    cProfile = (IProfileAnswerComponent)
                                               LoadControl("~/Components/Profile/ViewSingleChoice.ascx");
                                    break;
                                case ProfileQuestion.eShowStyle.SingleLine:
                                    cProfile = (IProfileAnswerComponent)
                                               LoadControl("~/Components/Profile/ViewSingleLine.ascx");
                                    break;
                                case ProfileQuestion.eShowStyle.MultiLine:
                                    cProfile = (IProfileAnswerComponent)
                                               LoadControl("~/Components/Profile/ViewMultiLine.ascx");
                                    break;
                                case ProfileQuestion.eShowStyle.MultiChoiceSmall:
                                    cProfile = (IProfileAnswerComponent)
                                               LoadControl("~/Components/Profile/ViewMultiChoiceSmall.ascx");
                                    break;
                                case ProfileQuestion.eShowStyle.MultiChoiceBig:
                                    cProfile = (IProfileAnswerComponent)
                                               LoadControl("~/Components/Profile/ViewMultiChoiceBig.ascx");
                                    break;

                                default:
                                    cProfile = (IProfileAnswerComponent)
                                               LoadControl("~/Components/Profile/ViewSingleChoice.ascx");
                                    break;
                            }

                            if (cProfile == null)
                            {
                                continue;
                            }

                            cProfile.LoadAnswer(CurrentNewUser.Username, question.Id);

                            ctlQuestions.Add((Control)cProfile);

                            tblRow.Cells.Add(tblCell);

                            if (topic.ViewColumns == 1 || ++questionNo % topic.ViewColumns == 0)
                            {
                                tblRow = new TableRow();
                                tblColumns.Rows.Add(tblRow);
                            }
                        }

                        #endregion

                        if (!hasVisibleQuestions)
                        {
                            plhProfile.Controls.Remove(lbs);
                            plhProfile.Controls.Remove(tblColumns);
                            continue;
                        }
                    }
                }

                #endregion

                pnlNewUser.Visible = true;
            }
            else
            {
                CurrentNewUser = null;
                pnlNewUser.Visible = false;
                lblError.Text = Lang.Trans("There are no users waiting for approval!");
            }
        }

        public void loadPhotos()
        {
            if (CurrentNewUser == null) return;

            Photo[] photos = Photo.Fetch(CurrentNewUser.Username);

            DataTable dtPhotos = new DataTable("Photos");
            dtPhotos.Columns.Add("PhotoId", typeof(int));
            dtPhotos.Columns.Add("Name");
            dtPhotos.Columns.Add("Description");

            bool hasAccess = CurrentUserSession != null && CurrentUserSession.Username != CurrentNewUser.Username;

            if (photos != null && photos.Length > 0)
                foreach (Photo photo in photos)
                {
                    if (photo.PrivatePhoto && !hasAccess)
                        continue;

                    if (ltrPhoto.Text == String.Empty)
                    {
                        CurrentPhotoId = photo.Id;
                        ltrPhoto.Text = ImageHandler.RenderImageTag(photo.Id, 450, 450, null, false, true);
                        lblPhotoName.Text = Server.HtmlEncode(photo.Name);
                        lblPhotoDescription.Text = Server.HtmlEncode(photo.Description);
                    }

                    dtPhotos.Rows.Add(new object[]
                                          {
                                              photo.Id,
                                              photo.Name,
                                              photo.Description
                                          });
                }

            if (dtPhotos.Rows.Count == 0)
            {
                lblError.Text = Lang.Trans("There are no photos!");
                lblPhotoName.Visible = false;
                lblPhotoDescription.Visible = false;
                dlPhotos.Visible = false;
                ltrPhoto.Visible = false;
            }
            else
            {
                ltrPhoto.Visible = true;
                lblError.Text = "";

                dlPhotos.DataSource = dtPhotos;
                dlPhotos.DataBind();
            }
        }

        private void determineProfileState()
        {
            CommunityProfileApproval[] approvals = CommunityProfileApproval.Fetch(CurrentNewUser.Username);
            int votes = approvals.Length;

            if (votes == Config.CommunityFaceControlSystem.RequiredNumberOfVotesToDetermine)
            {
                int approved = 0;

                foreach (CommunityProfileApproval approval in approvals)
                {
                    if (approval.Approved) approved++;
                }

                if ((approved * 100) / Config.CommunityFaceControlSystem.RequiredNumberOfVotesToDetermine >= Config.CommunityFaceControlSystem.RequiredPercentageToApproveProfile)
                {
                    CurrentNewUser.FaceControlApproved = true;
                    CurrentNewUser.Update();

                    Classes.User.SetProfileModerationApprovalScore(CurrentNewUser.Username, true,
                                                                 Config.CommunityFaceControlSystem.ScoresForCorrectOpinion,
                                                                 Config.CommunityFaceControlSystem.PenaltyForIncorrectOpinion);

                    MiscTemplates.ApproveProfileMessage approveProfileMessageTemplate =
                    new MiscTemplates.ApproveProfileMessage(CurrentNewUser.LanguageId);
                    Message.Send(Config.Users.SystemUsername, CurrentNewUser.Username, approveProfileMessageTemplate.Message, 0);
                }
                else
                {
                    Classes.User.SetProfileModerationApprovalScore(CurrentNewUser.Username, false,
                                                                 Config.CommunityFaceControlSystem.ScoresForCorrectOpinion,
                                                                 Config.CommunityFaceControlSystem.PenaltyForIncorrectOpinion);

                    CurrentNewUser.Deleted = true;
                    CurrentNewUser.Update();

                    NameValueCollection formatter = new NameValueCollection();
                    formatter.Add("RECIPIENT", CurrentNewUser.Username);
                    Email.SendTemplateEmail(typeof(EmailTemplates.RejectProfile),
                                            CurrentNewUser.Email, formatter, true, CurrentNewUser.LanguageId);
                }

                CommunityProfileApproval.Delete(CurrentNewUser.Username);
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (CurrentNewUser != null)
            {
                CommunityProfileApproval cma = new CommunityProfileApproval(CurrentNewUser.Username, CurrentUserSession.Username);
                cma.Approved = true;
                cma.Save();

                determineProfileState();
                loadNewUser(true);
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (CurrentNewUser != null)
            {
                CommunityProfileApproval cma = new CommunityProfileApproval(CurrentNewUser.Username, CurrentUserSession.Username);
                cma.Save();

                determineProfileState();
                loadNewUser(true);
            }
        }

        protected void btnPass_Click(object sender, EventArgs e)
        {
            loadNewUser(true);
        }

        protected void dlPhotos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName != "ShowPhoto") return;

            try
            {
                loadNewUser(false);

                int id = Convert.ToInt32(e.CommandArgument);
                Photo photo = Photo.Fetch(id);

                CurrentPhotoId = id;
                ltrPhoto.Text = ImageHandler.RenderImageTag(photo.Id, 450, 450, null, false, true);

                lblPhotoName.Text = Server.HtmlEncode(photo.Name);
                lblPhotoDescription.Text = Server.HtmlEncode(photo.Description);
            }
            catch (Exception err)
            {
                ExceptionLogger.Log(Request, err);
            }
        }
    }
}
