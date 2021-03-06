/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using System.Linq;

namespace ezFixUp.Components.Profile
{
    public partial class EditTopic : UserControl
    {
        private User user;
        protected Dictionary<string, Panel> topicsPanels = new Dictionary<string, Panel>();
        public string CssClass { set { EditTopicPanel1.CssClass = value; } }
        
        public User User
        {
            set
            {
                user = value;
                if (user != null)
                {
                    ViewState["Username"] = user.Username;
                }
                else
                    ViewState["Username"] = null;
            }
            get
            {
                if (user == null
                    && ViewState["Username"] != null)
                    user = User.Load((string)ViewState["Username"]);
                return user;
            }
        }

        public string CurrentTopicName
        {
            get { return this.hfCurrentTopic.Value; }
            set { this.hfCurrentTopic.Value = value; }
        }

        public string NextTopicName
        {
            get { return this.hfNextTopic.Value; }
            set { this.hfNextTopic.Value = value; }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        private Dictionary<int, object[]> AlreadyRemovedQuestions
        {
            get
            {
                return (Dictionary<int, object[]>)ViewState["AlreadyRemovedQuestions"] ?? new Dictionary<int, object[]>();
            }
            set { ViewState["AlreadyRemovedQuestions"] = value; }
        }

        public bool IsNext
        {
            get { return !hfIsNext.Value.IsNullOrEmpty(); } 
            set { hfIsNext.Value = value ? value.ToString() : String.Empty; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //LoadProfile();
            if (!IsPostBack)
            {
                LoadStrings();

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
            if ((postbackControl != null && (postbackControl.ID == "btnSave" || postbackControl.ID == "btnSkip" || postbackControl.ID == "btnBack")) 
                ||
                (Page.Request.Params["__EVENTTARGET"] != null && Page.Request.Params["__EVENTTARGET"].StartsWith("ctl00$cphContent$EditTopics")))
            {
                LoadProfile();
                ((Page as PageBase).Master as Site).Head1.Focus();
                //((Page as PageBase).Master as Site).TabIndex = -1;
            }
                
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            if (plhProfile.Controls.Count == 0)
            {
                LoadProfile();
            }
            UpdateTopicPanelsDisplayMode();
            int completedPct = GetProfileCompletePercentage();
            completed.Width = completedPct.ToString() + "%";
            notCompleted.Width = (100 - completedPct).ToString() + "%";
            notCompleted.BgColor = (completedPct < 100) ? "Grey" : "Transparent";
        }
        protected int CurrentTopicIndex
        {
            get
            {
                var keysList = topicsPanels.Keys.ToList();
                return keysList.IndexOf(CurrentTopicName);
            }
            set
            {
                CurrentTopicName = topicsPanels.ElementAt(value).Key;
                UpdateTopicPanelsDisplayMode();
            }
        }
        private void UpdateTopicPanelsDisplayMode()
        {//input: true for next and false for previous
            
            if (!lblError.Text.IsNullOrEmpty())
                return;
            
            var keysList = topicsPanels.Keys.ToList();
            int idx = CurrentTopicIndex;
            const int minIdx = 0;
            int maxIdx = keysList.Count - 1;
            
            if (NextTopicName.IsNullOrEmpty())
            {
                NextTopicName = CurrentTopicName;
            }
            else if (IsNext)
            {
                int nextIdx = (idx < maxIdx) ? ++idx : 0;
                btnSkip.Visible = (idx < maxIdx);
                btnBack.Visible = true;
                NextTopicName = keysList[nextIdx];
            }
            else
            {
                int prevIdx = (idx > minIdx) ? --idx : maxIdx;
                btnBack.Visible = (idx > minIdx);
                btnSkip.Visible = true;
                NextTopicName = keysList[prevIdx];
            }

            var enumerator = topicsPanels.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var keyValue = enumerator.Current;
                if (keyValue.Value.Style["display"] != null)
                    keyValue.Value.Style.Remove("display");
                keyValue.Value.Style.Add("display", keyValue.Key == NextTopicName ? "block" : "none");
            }
            CurrentTopicName = NextTopicName;
            //while (enumerator.MoveNext())
            //{
            //        var keyValue = enumerator.Current;
            //        keyValue.Value.Style.Remove("display");
            //        keyValue.Value.Style.Add("display", "none");

            //    if (keyValue.Key == CurrentTopicName)
            //    {
            //        if (Visible)
            //        {
            //            if (NextTopicName.IsNullOrEmpty())
            //            { // first load
            //                if (topicsPanels[CurrentTopicName].Style["display"] != null)
            //                    topicsPanels[CurrentTopicName].Style.Remove("display");
            //                topicsPanels[CurrentTopicName].Style.Add("display", "block");
            //                NextTopicName = CurrentTopicName;
            //            }
            //            else
            //            { // not first load
            //                if (enumerator.MoveNext())
            //                {
            //                    NextTopicName = enumerator.Current.Key;
            //                }
            //                else
            //                {
            //                    var newEnumerator = topicsPanels.GetEnumerator();
            //                    NextTopicName = (newEnumerator.MoveNext()) ? newEnumerator.Current.Key : CurrentTopicName;
            //                }
            //                if (topicsPanels[NextTopicName].Style["display"] != null)
            //                    topicsPanels[NextTopicName].Style.Remove("display");
            //                topicsPanels[NextTopicName].Style.Add("display", "block");
            //            }
            //        }
            //    }
            //}
            //CurrentTopicName = NextTopicName;
        }

        public int GetProfileCompletePercentage()
        {
            if (CurrentUserSession == null || !CurrentUserSession.IsAuthorized)
                return 0;

            int result = 0;
            int answered = 0;
            int total = 0;
            ProfileTopic[] profileTopics = ProfileTopic.Fetch();
            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions == null)
                        continue;
                    foreach (ProfileQuestion question in questions)
                    {
                        bool checkable = question.VisibleForFemale && question.VisibleForMale &&
                                         !question.VisibleForPaidOnly && question.EditStyle != ProfileQuestion.eEditStyle.Hidden;
                        if (checkable)
                        {
                            total++;
                            try
                            {
                                ProfileAnswer answer = question.FetchAnswer(CurrentUserSession.Username);
                                if (answer != null)
                                    answered++;
                            }
                            catch (NotFoundException notFoundException)
                            {
                                continue;
                            }
                        }
                    }
                }
            }
            result = answered * 100 / total;
            return result;
        }
        private void LoadStrings()
        {
            btnSave.Text = Lang.Trans("Save") + " �";
            btnSave.ToolTip = Lang.Trans("Save and continue to next topic");
            btnSkip.Text = Lang.Trans("Skip") + " �";
            btnSkip.ToolTip = Lang.Trans("Skip to next topic without saving");
            btnBack.Text = "� " + Lang.Trans("Back");
            btnBack.ToolTip = Lang.Trans("Back to previous topic without saving");
        }

        public void LoadProfile()
        {
            if (User == null)
            {
                return;
            }

            ProfileTopic[] profileTopics = ProfileTopic.Fetch();
            topicsPanels = new Dictionary<string, Panel>();

            if (profileTopics != null)
            {
                foreach (ProfileTopic topic in profileTopics)
                {
                    ProfileQuestion[] questions = topic.FetchQuestions();
                    if (questions == null)
                        continue;

                    var lbs = (LargeBoxStart)LoadControl("~/Components/LargeBoxStart.ascx");
                    if (Config.Misc.EnableProfileDataTranslation)
                        lbs.Title = Lang.Trans(topic.Name);
                    else
                        lbs.Title = topic.Name;
                    lbs.CssClass = "StandardBoxX";

                    ControlCollection ctlQuestions = null;
                    var tblColumns = new Table { CellPadding = 0, CellSpacing = 0, Width = Unit.Percentage(100) };

                    var tblRow = new TableRow();
                    tblColumns.Rows.Add(tblRow);
                    TableCell tblCell = null;
                    int questionNo = 0;
                    var dicQuestions = new Dictionary<int, object>();

                    bool hasVisibleQuestions = false;

                    #region Adding Questions

                    foreach (ProfileQuestion question in questions)
                    {
                        if (!question.IsVisible(User.Gender) || (question.EditableForPaidOnly && !CurrentUserSession.Paid))
                        {
                            continue;
                        }
                        if (question.EditStyle == ProfileQuestion.eEditStyle.Hidden)
                        {
                            continue;
                        }

                        hasVisibleQuestions = true;

                        tblCell = new TableCell();
                        ctlQuestions = tblCell.Controls;

                        IProfileQuestionComponent cProfile = null;

                        switch (question.EditStyle)
                        {
                            case ProfileQuestion.eEditStyle.SingleLine:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditSingleLine.ascx");
                                ((EditSingleLine)cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.MultiLine:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditMultiLine.ascx");
                                ((EditMultiLine)cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.SingleChoiceSelect:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditSingleChoiceSelect.ascx");
                                break;

                            case ProfileQuestion.eEditStyle.MultiChoiceCheck:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditMultiChoiceCheck.ascx");
                                ((EditMultiChoiceCheck)cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.SingleChoiceRadio:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditSingleChoiceRadio.ascx");
                                ((EditSingleChoiceRadio)cProfile).AdminMode = false;
                                break;

                            case ProfileQuestion.eEditStyle.MultiChoiceSelect:
                                cProfile = (IProfileQuestionComponent)
                                           LoadControl("~/Components/Profile/EditMultiChoiceSelect.ascx");
                                ((EditMultiChoiceSelect)cProfile).AdminMode = false;
                                break;

                            default:
                                break;
                        }

                        if (cProfile == null)
                        {
                            continue;
                        }

                        cProfile.User = User;
                        cProfile.Question = question;
                        ((Control)cProfile).ID = "Edit" + question.Id;
                        ctlQuestions.Add((Control)cProfile);
                        tblCell.CssClass="w100";//.Width = new Unit(Math.Round(100d / topic.EditColumns - 1),UnitType.Percentage);
                        tblRow.Cells.Add(tblCell);

                        dicQuestions.Add(question.Id, (Control)cProfile);

                        if (topic.EditColumns != 1 && ++questionNo%topic.EditColumns != 0) continue;
                        tblRow = new TableRow();
                        tblColumns.Rows.Add(tblRow);
                    }

                    #endregion

                    if (!hasVisibleQuestions)
                    {
                        continue;
                    }

                    var lbe = (LargeBoxEnd)LoadControl("~/Components/LargeBoxEnd.ascx");

                    topicsPanels.Add(topic.Name, new Panel { ID = "pnl" + topic.Name.Replace(" ", "") });
                    topicsPanels[topic.Name].Controls.Add(lbs);
                    topicsPanels[topic.Name].Controls.Add(tblColumns);
                    topicsPanels[topic.Name].Controls.Add(lbe);
                    plhProfile.Controls.Add(topicsPanels[topic.Name]);
                    SetCascadeQuestions(questions, dicQuestions);
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
                    var childClientIDsWithParentQuestionChoices = new Dictionary<string, object[]>();
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
            lblError.Text = String.Empty;
            litAlert.Text = String.Empty;
            IsNext = true;
            try
            {
                var toSave = new List<ProfileAnswer>();
                var toDelete = new List<ProfileAnswer>();
                var modifiedQuestionIDs = new List<int>();
                var parentControls = new List<Control>();
                GetParentControls(topicsPanels[CurrentTopicName], parentControls);
                GetAnswers(topicsPanels[CurrentTopicName], parentControls, toSave, toDelete, modifiedQuestionIDs);

                if (modifiedQuestionIDs.Count > 0)
                {
                    var newEvent = new Event(CurrentUserSession.Username) { Type = Event.eType.FriendUpdatedProfile };
                    var friendUpdatedProfile = new UpdatedProfile { QuestionIDs = modifiedQuestionIDs };
                    newEvent.DetailsXML = Misc.ToXml(friendUpdatedProfile);
                    newEvent.Save();
                }

                foreach (ProfileAnswer answer in toSave)
                    answer.Save();

                foreach (ProfileAnswer answer in toDelete)
                    answer.Delete();

                if (CurrentUserSession != null)
                {
                    CurrentUserSession.HasProfile = true;
                    CurrentUserSession.HasApprovedProfile =
                        User.HasProfile(CurrentUserSession.Username, true);
                }

                string message = "<br><b>" +
                                Lang.Trans("Your profile has been updated successfully!") +
                                "</b><br><br>";

                message = message +
                          Lang.Trans("Profiles with photos get 20 times more Response!");

                ((PageBase)Page).StatusPageMessage = message;

                ((PageBase)Page).StatusPageLinkSkindId = "UploadPhotos";
                ((PageBase)Page).StatusPageLinkText = Lang.Trans("Upload photos now!");
                ((PageBase)Page).StatusPageLinkURL = "~/ManageProfile.aspx?sel=photos";
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
            string[] usernames = User.FetchMutuallyFriends(CurrentUserSession.Username);

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
                                (DateTime.Now - User.UserSince).Days < Config.Users.AutoApproveAnswers)
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
                                            User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername,
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
                        //Response.Write("<script>alert('" + err.Message.Replace("'", "\\'") + "');</script>");
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
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion

        protected void btnSkip_Click(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;
            litAlert.Text = String.Empty;
            IsNext = true;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            lblError.Text = String.Empty;
            litAlert.Text = String.Empty;
            IsNext = false;
        }
    }
}