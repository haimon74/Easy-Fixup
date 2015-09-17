using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components.Profile;

namespace AspNetDating.Mobile
{
    public partial class Profile : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                LoadProfile();
                if (Config.Users.CompletedProfileRequiredToBrowseSearch &&
                    ((PageBase)Page).CurrentUserSession != null &&
                    !((PageBase)Page).CurrentUserSession.HasProfile &&
                    Request.Params["err"] == "profnotcompl"
                    )
                {
                    litAlert.Text = "<script>var alert_string = '" +
                                    Lang.Trans("You must complete your profile in order to view other profiles!").
                                        Replace("'", "\\'") +
                                    "';</script>";
                }
            }

            Control postbackControl = Global.GetPostBackControl(Page);
            if ((postbackControl != null && postbackControl.ID == "btnSave") ||
                (Page.Request.Params["__EVENTTARGET"] != null && Page.Request.Params["__EVENTTARGET"].StartsWith("ctl00$cphContent$EditProfile1")))
                LoadProfile();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (plhProfile.Controls.Count == 0)
            {
                LoadProfile();
            }
        }

        private void loadStrings()
        {
            lblTitle.InnerText = "Profile".Translate();
            btnSave.Text = Lang.Trans(" Save Changes >> ");
        }

        public void LoadProfile()
        {
            if (User == null)
            {
                return;
            }

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();
            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions == null)
                        continue;

                    var divSeparator = new HtmlGenericControl("div");
                    divSeparator.Attributes.Add("class", "SeparatorLine");
                    var divTopic = new HtmlGenericControl("div");
                    divTopic.Attributes.Add("class", "TopicHeader");
                    divTopic.InnerText = Config.Misc.EnableProfileDataTranslation ? Lang.Trans(topic.Name) : topic.Name;
//                    if (plhProfile.Controls.Count > 0)
//                        lbs.CssClass = "StandardBoxX";

                    plhProfile.Controls.Add(divTopic);
                    plhProfile.Controls.Add(divSeparator);

                    ControlCollection ctlQuestions = null;
                    var tblColumns = new PlaceHolder();
//                    tblColumns.CellPadding = 0;
//                    tblColumns.CellSpacing = 0;
//                    tblColumns.Width = Unit.Percentage(100);
                    plhProfile.Controls.Add(tblColumns);
//                    var tblRow = new TableRow();
//                    tblColumns.Rows.Add(tblRow);
//                    TableCell tblCell = null;
                    //int questionNo = 0;
                    Dictionary<int, object> dicQuestions = new Dictionary<int, object>();

                    bool hasVisibleQuestions = false;

                    #region Adding Questions

                    foreach (ProfileQuestion question in questions)
                    {
                        if (!question.IsVisible(CurrentUserSession.Gender) || (question.EditableForPaidOnly && !CurrentUserSession.Paid))
                        {
                            continue;
                        }
                        if (question.EditStyle == ProfileQuestion.eEditStyle.Hidden)
                        {
                            continue;
                        }

                        hasVisibleQuestions = true;

//                        tblCell = new TableCell();
//                        ctlQuestions = tblCell.Controls;
                        ctlQuestions = tblColumns.Controls;
                        IProfileQuestionComponent cProfile = null;

                        switch (question.EditStyle)
                        {
                            case ProfileQuestion.eEditStyle.SingleLine:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditSingleLine.ascx");
                                ((EditSingleLine) cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.MultiLine:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditMultiLine.ascx");
                                ((EditMultiLine) cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.SingleChoiceSelect:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditSingleChoiceSelect.ascx");
                                break;

                            case ProfileQuestion.eEditStyle.MultiChoiceCheck:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditMultiChoiceCheck.ascx");
                                ((EditMultiChoiceCheck) cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.SingleChoiceRadio:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditSingleChoiceRadio.ascx");
                                ((EditSingleChoiceRadio) cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.MultiChoiceSelect:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditMultiChoiceSelect.ascx");
                                ((EditMultiChoiceSelect) cProfile).AdminMode = false;
                                break;

                            default:
                                break;
                        }

                        if (cProfile == null)
                        {
                            continue;
                        }

                        cProfile.User = CurrentUserSession;
                        cProfile.Question = question;
                        ((Control) cProfile).ID = "Edit" + question.Id;
                        ctlQuestions.Add((Control) cProfile);

//                        tblRow.Cells.Add(tblCell);

                        dicQuestions.Add(question.Id, (Control)cProfile);

//                        if (topic.EditColumns == 1 || ++questionNo%topic.EditColumns == 0)
//                        {
//                            tblRow = new TableRow();
//                            tblColumns.Rows.Add(tblRow);
//                        }
                    }

                    SetCascadeQuestions(questions, dicQuestions);

                    #endregion

                    if (!hasVisibleQuestions)
                    {
                        plhProfile.Controls.Remove(divTopic);
                        plhProfile.Controls.Remove(tblColumns);
                        continue;
                    }

//                    var lbe = (LargeBoxEnd) LoadControl("~/Components/LargeBoxEnd.ascx");
//                    plhProfile.Controls.Add(lbe);
                }
            }
            else
            {
                btnSave.Visible = false;
            }
        }

        private void SetCascadeQuestions(ProfileQuestion[] questions, Dictionary<int, object> dicQuestions)
        {
            List<int> lHiddenParentQuestions = new List<int>();

            foreach (ProfileQuestion question in questions)
            {
                ProfileQuestion[] childQuestions =
                    questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();

                bool isParent = childQuestions.Length > 0;
                bool isChild = question.ParentQuestionID.HasValue;
                if (!dicQuestions.ContainsKey(question.Id)) continue; // if current question is hidden
                Control currentQuestionControl = (Control)dicQuestions[question.Id];

                if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                    ((ICascadeQuestionComponent)currentQuestionControl).GenerateResetValuesJS();

                if (isParent)
                {
                    Dictionary<string, object[]> childClientIDsWithParentQuestionChoices = new Dictionary<string, object[]>();
                    GetChildrenClientIDs(question, questions, dicQuestions, childClientIDsWithParentQuestionChoices);

                    if ((currentQuestionControl as ICascadeQuestionComponent) != null)
                        ((ICascadeQuestionComponent)currentQuestionControl).GenerateJSForChildVisibility(childClientIDsWithParentQuestionChoices);
                    else
                        new Exception(String.Format("{0} control must implement ICascadeQuestionComponent",
                                                    currentQuestionControl.ID));
                }

                if (isChild)
                {
                    // if parent question is hidden hide the child
                    if (!dicQuestions.ContainsKey(question.ParentQuestionID.Value)
                        || lHiddenParentQuestions.Contains(question.ParentQuestionID.Value))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        ((IProfileQuestionComponent)currentQuestionControl).UserControlPanel.Attributes.Add("style",
                                                                                                             "display:none");
                        ((IProfileQuestionComponent)currentQuestionControl).Answer.Value = String.Empty;
                        continue;
                    }
                    Control currentQuestionParentControl = (Control)dicQuestions[question.ParentQuestionID.Value];
                    string[] parentAnswers =
                        ((IProfileQuestionComponent)currentQuestionParentControl).Answer.Value.Split(
                            new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                    if (!question.ParentQuestionChoices.Split(':').Any(parentChoice => parentAnswers.Contains(parentChoice)))
                    {
                        lHiddenParentQuestions.Add(question.Id);
                        ((IProfileQuestionComponent)currentQuestionControl).UserControlPanel.Attributes.Add("style",
                                                                                                             "display:none");
                        ((IProfileQuestionComponent)currentQuestionControl).Answer.Value = String.Empty;
                    }
                }
            }
        }

        private void GetChildrenClientIDs(ProfileQuestion question, ProfileQuestion[] questions, Dictionary<int, object> dicQuestions, Dictionary<string, object[]> childClientIDsWithParentQuestionChoices)
        {
            ProfileQuestion[] childQuestions =
                questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();
            if (childQuestions.Length > 0)
            {
                foreach (ProfileQuestion childQuestion in childQuestions)
                {
                    var childClientIDs = new List<string>();

                    // child question is not visible so skip it
                    if (dicQuestions.ContainsKey(childQuestion.Id))
                    {
                        string childClientID =
                            ((IProfileQuestionComponent)dicQuestions[childQuestion.Id]).UserControlPanel.ClientID;
                        string[] parentQuestionChoices =
                            childQuestion.ParentQuestionChoices.Split(new[] { ":" }, StringSplitOptions.RemoveEmptyEntries);

                        childClientIDsWithParentQuestionChoices.Add(childClientID,
                                                                    new object[] { parentQuestionChoices, childClientIDs });
                    }

                    PopulateChildrenIDs(childQuestion, questions, dicQuestions, childClientIDs);
                }
            }
        }

        private void PopulateChildrenIDs(ProfileQuestion question, ProfileQuestion[] questions, Dictionary<int, object> dicQuestions, List<string> childClientIDs)
        {
            ProfileQuestion[] childQuestions =
                questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();
            if (childQuestions.Length > 0)
            {
                foreach (ProfileQuestion childQuestion in childQuestions)
                {

                    // child question is not visible so skip it
                    if (dicQuestions.ContainsKey(childQuestion.Id))
                    {
                        string childClientID =
                            ((IProfileQuestionComponent)dicQuestions[childQuestion.Id]).UserControlPanel.ClientID;

                        childClientIDs.Add(childClientID);
                    }

                    PopulateChildrenIDs(childQuestion, questions, dicQuestions, childClientIDs);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var toSave = new List<ProfileAnswer>();
                var toDelete = new List<ProfileAnswer>();
                List<int> modifiedQuestionIDs = new List<int>();
                List<Control> parentControls = new List<Control>();
                GetParentControls(plhProfile, parentControls);
                GetAnswers(plhProfile, parentControls, toSave, toDelete, modifiedQuestionIDs);

                if (modifiedQuestionIDs.Count > 0)
                {
                    var newEvent = new Event(CurrentUserSession.Username);

                    newEvent.Type = Event.eType.FriendUpdatedProfile;
                    var friendUpdatedProfile = new UpdatedProfile();
                    friendUpdatedProfile.QuestionIDs = modifiedQuestionIDs;
                    newEvent.DetailsXML = Misc.ToXml(friendUpdatedProfile);

                    newEvent.Save();
                }

                foreach (ProfileAnswer answer in toSave)
                {
                    answer.Save();
                }
                foreach (ProfileAnswer answer in toDelete)
                {
                    answer.Delete();
                }

                if (CurrentUserSession != null)
                {
                    CurrentUserSession.HasProfile = true;
                    CurrentUserSession.HasApprovedProfile =
                        Classes.User.HasProfile(CurrentUserSession.Username, true);
                }

                string message = Lang.Trans
                    ("<br><b>Your profile has been updated successfully!</b><br><br>");

                message = message +
                          Lang.Trans("Profiles with photos get 20 times more Response!");

                StatusPageMessage = message;

//                ((PageBase)Page).StatusPageLinkSkindId = "UploadPhotos";
//                ((PageBase)Page).StatusPageLinkText = Lang.Trans("Upload photos now!");
//                ((PageBase)Page).StatusPageLinkURL = "~/ManageProfile.aspx?sel=photos";

//                Response.Redirect("ShowStatus.aspx");
            }
            catch (AnswerRequiredException)
            {
            }
        }

        private void GetParentControls(Control control, List<Control> parentControls)
        {
            foreach (Control ctrl in control.Controls)
            {
                if (ctrl is IProfileQuestionComponent) parentControls.Add(ctrl);
                else GetParentControls(ctrl, parentControls);
            }
        }

        private void GetAnswers(Control control, List<Control> parentControls, List<ProfileAnswer> toSave, List<ProfileAnswer> toDelete, List<int> modifiedQuestionIDs)
        {
            string[] usernames = Classes.User.FetchMutuallyFriends(CurrentUserSession.Username);

            foreach (Control ctl in control.Controls)
            {
                if (ctl is IProfileQuestionComponent)
                {
                    try
                    {
                        ProfileAnswer answer = ((IProfileQuestionComponent)ctl).Answer;
                        ProfileQuestion question = ((IProfileQuestionComponent)ctl).Answer.Question;
                        if (question.ParentQuestionID.HasValue)
                        {
                            var currentQuestionParentControl =
                                parentControls.Cast<IProfileQuestionComponent>().FirstOrDefault(
                                    c => c.Answer.Question.Id == question.ParentQuestionID);
                            if (currentQuestionParentControl != null)
                            {
                                string[] parentAnswers = (currentQuestionParentControl).Answer.Value.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
                                if (!question.ParentQuestionChoices.Split(':').Any(parentChoice => parentAnswers.Contains(parentChoice)))
                                {
                                    answer.Value = String.Empty; // the control is not visible and his answers should be deleted
                                }
                            }
                        }
                        if (answer.Value != String.Empty)
                        {
                            if (Config.Misc.EnableBadWordsFilterProfile &&
                                (answer.Question.EditStyle == ProfileQuestion.eEditStyle.SingleLine ||
                                 answer.Question.EditStyle == ProfileQuestion.eEditStyle.MultiLine))
                            {
                                answer.Value = Parsers.ProcessBadWords(answer.Value);
                            }

                            if (answer.Question.RequiresApproval &&
                                (DateTime.Now - CurrentUserSession.UserSince).Days < Config.Users.AutoApproveAnswers)
                            {
                                if (CurrentUserSession.Level != null)
                                {
                                    if (!CurrentUserSession.Level.Restrictions.AutoApproveAnswers
                                        && !CurrentUserSession.BillingPlanOptions.AutoApproveAnswers.Value)
                                    {
                                        rejectAnswer(answer);
                                    }
                                }
                                else if (!CurrentUserSession.BillingPlanOptions.AutoApproveAnswers.Value)
                                {
                                    rejectAnswer(answer);
                                }
                            }
                            toSave.Add(answer);

                            if (answer.Approved)
                            {
                                ProfileAnswer oldAnswer = null;
                                try
                                {
                                    oldAnswer = ProfileAnswer.Fetch(answer.User.Username, answer.Question.Id);
                                }
                                catch (NotFoundException)
                                {
                                    continue;
                                }

                                if (oldAnswer.Value != answer.Value)
                                {
                                    if (!modifiedQuestionIDs.Contains(answer.Question.Id))
                                        modifiedQuestionIDs.Add(answer.Question.Id);

                                    foreach (string friendUsername in usernames)
                                    {
                                        if (Config.Users.NewEventNotification)
                                        {
                                            string text = String.Format("Your friend {0} has updated the \"{1}\" section in their profile".Translate(),
                                            "<b>" + CurrentUserSession.Username + "</b>", answer.Question.Name);
                                            int imageID = 0;
                                            try
                                            {
                                                imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                                            }
                                            catch (NotFoundException)
                                            {
                                                User user = null;
                                                try
                                                {
                                                    user = Classes.User.Load(CurrentUserSession.Username);
                                                    imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                                                }
                                                catch (NotFoundException)
                                                {
                                                    continue;
                                                }
                                            }
                                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                            Classes.User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername,
                                                                             text, thumbnailUrl,
                                                                             UrlRewrite.CreateShowUserUrl(
                                                                                 CurrentUserSession.Username));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            toDelete.Add(answer);
                        }
                    }
                    catch (AnswerRequiredException err)
                    {
                        lblError.CssClass = "error";
                        lblError.Text = err.Message;
                        litAlert.Text = "<script>var alert_string = '" + err.Message.Replace("'", "\\'") + "';</script>";
                        throw err;
                    }
                }
                else
                {
                    GetAnswers(ctl, parentControls, toSave, toDelete, modifiedQuestionIDs);
                }
            }
        }

        private void rejectAnswer(ProfileAnswer answer)
        {
            try
            {
                ProfileAnswer oldAnswer =
                    ProfileAnswer.Fetch(answer.User.Username,
                                        answer.Question.Id);

                if (oldAnswer.Value != answer.Value || !oldAnswer.Approved)
                {
                    answer.Approved = false;
                }
            }
            catch (NotFoundException)
            {
                answer.Approved = false;
            }
        }
    }
}
