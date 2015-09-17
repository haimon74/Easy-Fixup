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
using System.Data;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using ezFixUp.Components;
using CheckBox = System.Web.UI.WebControls.CheckBox;
using Control = System.Web.UI.Control;
using Image = System.Drawing.Image;
using Message = ezFixUp.Classes.Message;
using RadioButton = System.Web.UI.WebControls.RadioButton;
using TextBox = System.Web.UI.WebControls.TextBox;
using User = ezFixUp.Classes.User;

//using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace ezFixUp
{
    public partial class SendMessagePopup : PageBase
    {

        public Message.eMessageType MessageType
        {
            set { ViewState["MessageType"] = value; }
            get { return (Message.eMessageType)ViewState["MessageType"]; }
        }

        protected int EcardTypeID { get; set; }

        protected string MessageBodyClientId
        {
            get { return txtMessageBody.ClientID; }
        }

        [Serializable]
        public class SendMsgUserClass
        {
            public string Username { get; set; }
            //public string DisplayName { get; set; }
            public string Location { get; set; }
            public int Age { get; set; }
            public bool IsDeleted { get; set; }
            public string PrimaryPhotoUrl { get; set; }
        }

        public List<int> SelectedGiftsIds
        {
            set { GiftsGallery1.SelectedGiftTypes = value; }
            get { return GiftsGallery1.SelectedGiftTypes; }
        }
        public SendMsgUserClass ToUser
        {
            set { ViewState["SendMessageToUser"] = value; }
            get { return (SendMsgUserClass)ViewState["SendMessageToUser"]; }
        }
        public SendMsgUserClass FromUser
        {
            set { ViewState["SendMessageFromUser"] = value; }
            get { return (SendMsgUserClass)ViewState["SendMessageFromUser"]; }
        }
        public SendMsgUserClass AboutUser
        {
            set { ViewState["SendMessageAboutUser"] = value; }
            get { return (SendMsgUserClass)ViewState["SendMessageAboutUser"]; }
        }
        
        public int CurrentSmiliesPage
        {
            set
            {
                ViewState["CurrentSmiliesPage"] = value;
            }
            get
            {
                return (int)(ViewState["CurrentSmiliesPage"] ?? 0);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                setMessageType();
                checkRequestLegit();
                loadStrings();
                preparePage();
            }
        }

        private void setMessageType()
        {
            string msgType = (Request.Params["type"] ?? "msg").ToLower();
            switch (msgType)
            {
                case "wink":
                    MessageType = Message.eMessageType.Wink;
                    break;
                case "gift":
                    MessageType = Message.eMessageType.Gift;
                    break;
                case "invitation":
                    MessageType = Message.eMessageType.Invitation;
                    break;
                case "answer":
                    MessageType = Message.eMessageType.MutualFriendAnswerAbout;
                    break;
                case "ask":
                    MessageType = Message.eMessageType.MutualFriendAskAbout;
                    break;
                case "request":
                    MessageType = Message.eMessageType.RequestMatchmaking;
                    break;
                case "matchmaking":
                    MessageType = Message.eMessageType.Matchmaking;
                    break;
                case "msg":
                default:
                    MessageType = Message.eMessageType.Message;
                    break;
            }
        }

        private void loadSmileys()
        {
            var lUsedSmilies = new List<string>();
            var dtSmilies = new DataTable();
            dtSmilies.Columns.Add("Key");
            dtSmilies.Columns.Add("Image");
            dtSmilies.Columns.Add("Description");
            dtSmilies.Columns.Add("IsSecondary");

            foreach (string key in Smilies.dSmileys.Keys)
            {
                Smiley smiley = Smilies.dSmileys[key];
                if (lUsedSmilies.IndexOf(smiley.Image) >= 0) continue;
                lUsedSmilies.Add(smiley.Image);

                var row = dtSmilies.NewRow();
                row.ItemArray = new object[]
                                 {
                                     smiley.Key, Config.Urls.Home + "/Smilies/" + smiley.Image,
                                     smiley.Description, smiley.Secondary
                                 };

                dtSmilies.Rows.Add(row);
            }

            dtSmilies.DefaultView.Sort = "IsSecondary";

            var pagedSource = new PagedDataSource
                                  {
                                      AllowPaging = true,
                                      PageSize = 41,
                                      DataSource = dtSmilies.DefaultView,
                                  };
            if (CurrentSmiliesPage < 0) CurrentSmiliesPage = pagedSource.PageCount - 1;
            if (CurrentSmiliesPage >= pagedSource.PageCount) CurrentSmiliesPage = 0;
            pagedSource.CurrentPageIndex = CurrentSmiliesPage;

            dlSmilies.DataSource = pagedSource;
            dlSmilies.DataBind();
        }

        private void checkMessagesLimitReached(out bool shouldPayWithCredits)
        {
            shouldPayWithCredits = false;

            if (CurrentUserSession.Username == Config.Users.SystemUsername)
                return;

            if (Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female)
                return;

            int maxBillingPlanMessages = 0;
            int maxLevelMessages = 0;
            Message[] messages = null;
            int msgCount = 0;
            bool hasLevel = (CurrentUserSession.Level != null);
            switch (MessageType)
            {
                case Message.eMessageType.Wink:
                    maxLevelMessages = hasLevel ? CurrentUserSession.Level.Restrictions.MaxWinksPerDay : 3;
                    maxBillingPlanMessages = CurrentUserSession.BillingPlanOptions.MaxWinksPerDay.Value;
                    messages = Message.FetchSentMessagesForLast24Hours(
                                                CurrentUserSession.Username,
                                                (int)Message.eMessageType.Wink);
                    msgCount = messages != null ? messages.Length : 0;
                    break;
                case Message.eMessageType.Gift:
                case Message.eMessageType.Invitation:
                case Message.eMessageType.RequestMatchmaking:
                case Message.eMessageType.Matchmaking:
                    maxLevelMessages = -1;
                    maxBillingPlanMessages = -1;
                    break;
                default:
                case Message.eMessageType.Message:
                case Message.eMessageType.MutualFriendAskAbout:
                case Message.eMessageType.MutualFriendAnswerAbout:
                    maxLevelMessages = hasLevel ? CurrentUserSession.Level.Restrictions.MaxMessagesPerDay : 3;
                    maxBillingPlanMessages = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Value;
                    messages = Message.FetchSentMessagesForLast24Hours(
                                                CurrentUserSession.Username,
                                                (int)Message.eMessageType.Message);
                    msgCount = messages != null ? messages.Length : 0;
                    break;
            }


            if (maxBillingPlanMessages == -1 || maxLevelMessages == -1)
            {
                shouldPayWithCredits = true;
                return;
            }

            int maxMessages = Math.Max(maxBillingPlanMessages, maxLevelMessages);

            //if (CurrentUserSession.Username == Config.Users.SystemUsername
            //    || Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female
            //    || CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Value == -1 /*Config.Users.MembersMaxMessagesPerDay == -1*/)
            //    return;

            if (msgCount >= maxMessages)
            { //TODO: check this logic at billing phase
                if (!CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.EnableCreditsPayment &&
                    !CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.UpgradableToNextPlan)
                {
                    StatusPageMessage = Lang.Trans(String.Format(
                        "You've exceeded the number of {0}s you can send per day!", MessageType.ToString().ToLower()));
                    StatusPageLinkText = "Billing".Translate();
                    StatusPageLinkURL = "ProfileBilling.aspx";
                    //Response.Redirect("Mailbox.aspx");
                    return;
                }
                else if (CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.EnableCreditsPayment &&
                    CurrentUserSession.Credits >= CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits)
                {
                    shouldPayWithCredits = true;
                    // the user has enough credits
                }
                else
                {
                    string toUsername = Request.Params["to_user"];
                    bool toUsernamePaid = Classes.User.IsPaidMember(toUsername);

                    if (!(Config.Users.NonPaidMembersCanRespondToPaidMembers && toUsernamePaid && Message.MessagesExist(toUsername, CurrentUserSession.Username)))
                    {
                        Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay;
                        Response.Redirect("ManageProfile.aspx?sel=payment");
                        return;
                    }
                }
            }

            //if (Classes.User.IsNonPaidMember(CurrentUserSession.Username))
            //{
            //    if (messages.Length >= Config.Users.UnpaidMembersMaxMessagesPerDay)
            //    {
            //        Response.Redirect("ManageProfile.aspx?sel=payment");
            //    }
            //}

            //if (messages.Length >= Config.Users.MembersMaxMessagesPerDay)
            //{
            //    StatusPageMessage = Lang.Trans("You've exceeded the number of messages you can send per day!");
            //    Response.Redirect("ShowStatus.aspx");
            //}

            #region checkContactedUsersLimitReached

            List<string> uniqueUsers = new List<string>();

            if (messages != null)
            {
                foreach (Message message in messages)
                    AddUniqueItems(uniqueUsers, message.ToUser.Username);
            }
            if (uniqueUsers.Count >= Config.Users.MaxContactedUsersPerDay)
            {
                StatusPageMessage = Lang.Trans("You've exceeded the number of users you can contact per day!");
                StatusPageLinkText = "Billing".Translate();
                StatusPageLinkURL = "ProfileBilling.aspx";
                //Response.Redirect("Mailbox.aspx");
            }

            #endregion
        }

        private List<string> AddUniqueItems(List<string> list, string aItem)
        {
            bool found = false;
            foreach (string item in list)
            {
                if (item == aItem)
                {
                    found = true;
                    break;
                }
            }

            if (!found)
                list.Add(aItem);

            return list;
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
            this.btnSend.Click += new EventHandler(this.btnSend_Click);
        }

        #endregion

        private void loadStrings()
        {
            string displayName = CurrentUserSession.GetUserDisplayedNameByUsername(ToUser.Username);
            //LargeBoxStart1.Title = String.Format(Lang.Trans("{0} Composing Message to {1}"), CurrentUserSession.Name, displayName);
            
            btnSend.Text = Lang.Trans("Send");
            //lnkUpgradeNow.Text = Lang.Trans("Upgrade Now to Respond to this email");
            lbAddGift.Text = Lang.Trans("Add gift");
            string src = Request.Params["src"];
            
            string type = Request.Params["type"];
            if (type == "wink")
            {
                var clientProxy = new Services.Service();
                ddlWinkPhrase.DataSource = clientProxy.GetAllWinks();
                ddlWinkPhrase.DataTextField = "Name";
                ddlWinkPhrase.DataValueField = "Value";
                ddlWinkPhrase.DataBind();
                foreach (ListItem item in ddlWinkPhrase.Items)
                {
                    item.Text = item.Text.Translate();
                }
            }
            if (type == "ask" || type == "answer")
            {
                cbMfQuestion1.Text = "Does profile has correct information?".Translate();
                lblMfAnswer1.Text = cbMfQuestion1.Text;
                cbMfQuestion2.Text = "Does profile's photo correct?".Translate();
                lblMfAnswer2.Text = cbMfQuestion2.Text;
                cbMfQuestion3.Text = "Does he/she has a big heart?".Translate();
                lblMfAnswer3.Text = cbMfQuestion3.Text;
                cbMfQuestion4.Text = "What do you think about her?".Translate();
                lblMfAnswer4.Text = cbMfQuestion4.Text;
                cbMfQuestion5.Text = "Do you think we match?".Translate();
                lblMfAnswer5.Text = cbMfQuestion5.Text;
                lblMutualFriendMsgTitle.Text =
                    (type == "ask")
                        ? "Hi, I would like to ask some questions about our mutual friend - {0}:".Translate()
                        : "Hi, Here are the answers about our mutual friend - {0}:".Translate();
                //lblMutualFriendMsgTitle.Text = String.Format(lblMutualFriendMsgTitle.Text, AboutUser.Username);
                //LargeBoxStart1.Title =
                    //(type == "ask")
                    //    ? Lang.Trans("Edit Message:  Ask {0} About {1}")
                    //    : Lang.Trans("Edit Message:  Answer {0} About {1}");
                //? String.Format(Lang.Trans("Edit Message:  Ask {0} About {1}"),  ToUser.Username, AboutUser.Username)
                //: String.Format(Lang.Trans("Edit Message:  Answer {0} About {1}"), ToUser.Username, AboutUser.Username);
            }
            else if (type == "matchmaking")
            {
                //LargeBoxStart1.Title = Lang.Trans("Edit Message:  Matchmaking {0} with {1}");
                //LargeBoxStart1.Title = String.Format(Lang.Trans("Edit Message:  Matchmaking {0} with {1}"), ToUser.Username, AboutUser.Username);
            }
        }
        private string ComposePrimaryPhotoUrl(Classes.User user)
        {
            Classes.Photo primaryPhoto = null;
            try { primaryPhoto = user.GetPrimaryPhoto(); }
            catch (NotFoundException) { }
            catch (Exception err) { Log(err); }

            const int photoWidthHeight = 90;
            bool hasPrimaryPhoto = (primaryPhoto != null && primaryPhoto.Approved);

            return hasPrimaryPhoto
                    ? ImageHandler.RenderImageTag(primaryPhoto.Id,
                                                  photoWidthHeight, photoWidthHeight,
                                                  "photoframe", false, true)
                    : ((user.FacebookID.HasValue)
                        ? ImageHandler.RenderFacebookImageTag(user.FacebookID.Value, ImageHandler.eFacebookImageType.Default, "photoframe")
                        : ImageHandler.RenderImageTag(user.Gender,
                                                  photoWidthHeight, photoWidthHeight,
                                                  "photoframe", false, true));
        }
        private void checkRequestLegit()
        {
            #region Check if message request is legit

            Classes.User fromUser = null;
            try
            {
                fromUser = Classes.User.Load(CurrentUserSession.Username);
                FromUser = (fromUser == null) ? null : new SendMsgUserClass
                {
                    Username = CurrentUserSession.Username,
                    IsDeleted = fromUser.Deleted,
                    PrimaryPhotoUrl = ComposePrimaryPhotoUrl(fromUser)
                };

            }
            catch (NotFoundException)
            {
                Response.Redirect(MatchmakerHelper.CurrentHomePage);
                return;
            }

            bool isMfMsg = MessageType == Message.eMessageType.MutualFriendAskAbout ||
                           MessageType == Message.eMessageType.MutualFriendAnswerAbout ||
                           MessageType == Message.eMessageType.RequestMatchmaking ||
                           MessageType == Message.eMessageType.Matchmaking;
            string toUsername = Request.Params["to_user"];
            string aboutUsername = Request.Params["about"] ?? Request.Params["with_user"];

            if (toUsername.IsNullOrEmpty() || (isMfMsg && aboutUsername.IsNullOrEmpty()))
            {
                //Response.Redirect("~/MailBox.aspx");
                return;
            }

            Classes.User toUser = null;
            Classes.User aboutUser = null;
            try
            {
                toUser = Classes.User.Load(toUsername);
                ToUser = (toUser == null) ? null : new SendMsgUserClass
                                   {
                                       Username = toUsername,
                                       Age = toUser.Age,
                                       Location = toUser.Country,
                                       IsDeleted = toUser.Deleted,
                                       PrimaryPhotoUrl = ComposePrimaryPhotoUrl(toUser)
                                   };
                if (isMfMsg)
                {
                    aboutUser = Classes.User.Load(aboutUsername);
                    AboutUser = (aboutUser == null) ? null : new SendMsgUserClass
                    {
                        Username = aboutUsername,
                        Location = aboutUser.Country,
                        Age = aboutUser.Age,
                        IsDeleted = aboutUser.Deleted,
                        PrimaryPhotoUrl = ComposePrimaryPhotoUrl(aboutUser)
                    };
                }
            }
            catch (NotFoundException)
            {
                ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists") + " [" +
                    (toUser == null ? toUsername : aboutUsername) + "]";
                //Response.Redirect("~/Mailbox.aspx");
                return;
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception err)
            {
                Log(err);
                //Response.Redirect("~/MailBox.aspx");
                return;
            }
            if (ToUser != null && (!isMfMsg || AboutUser != null))
            {
                if (ToUser.IsDeleted || (isMfMsg && AboutUser.IsDeleted))
                {
                    ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists") +
                                        " [" + (ToUser.IsDeleted ? ToUser.Username : AboutUser.Username) + "]";
                    //Response.Redirect("~/Mailbox.aspx");
                    return;
                }
            }

            #endregion

            #region Check can current user send message

            string reason = "";

            if (ToUser != null && FromUser != null)
            {

                if (Message.MessagesExist(ToUser.Username, FromUser.Username)
                     || CanSendMessage(fromUser, toUser, out reason)
                     || Classes.User.IsUserInFavouriteList(ToUser.Username, FromUser.Username)
                     || CurrentUserSession.IsAdmin())
                    return;
                else if (!Classes.User.IsGroupOwner(fromUser, toUser))
                {
                    StatusPageMessage = reason;
                    //Response.Redirect("~/ShowStatus.aspx");
                    return;
                }
            }

            #endregion
        }

        private bool CanSendMessage(Classes.User fromUser, Classes.User toUser, out string reason)
        {
            bool canSend = false;
            switch (MessageType)
            {
                case Message.eMessageType.Wink:
                    canSend = Classes.User.CanSendWink(fromUser, toUser, out reason);
                    break;
                case Message.eMessageType.Gift:
                    canSend = Classes.User.CanSendGift(fromUser, toUser, out reason);
                    break;

                case Message.eMessageType.Invitation:
                    canSend = Classes.User.CanSendInvitation(fromUser, toUser, out reason);
                    break;
                case Message.eMessageType.MutualFriendAnswerAbout:
                    canSend = Classes.User.CanSendMessage(fromUser, toUser, out reason);
                    break;
                case Message.eMessageType.MutualFriendAskAbout:
                    canSend = Classes.User.CanSendMessage(fromUser, toUser, out reason);
                    break;
                case Message.eMessageType.Message:
                case Message.eMessageType.Matchmaking:
                case Message.eMessageType.RequestMatchmaking:
                default:
                    canSend = Classes.User.CanSendMessage(fromUser, toUser, out reason);
                    break;
            }
            return canSend;
        }
        private int GetCreditsPerMaxMessageTypePerDay()
        {
            switch (MessageType)
            {
                case Message.eMessageType.Wink:
                    return CurrentUserSession.BillingPlanOptions.MaxWinksPerDay.Credits;
                case Message.eMessageType.Message:
                case Message.eMessageType.Matchmaking:
                case Message.eMessageType.RequestMatchmaking:
                case Message.eMessageType.MutualFriendAskAbout:
                    return CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits;
                case Message.eMessageType.Gift:
                case Message.eMessageType.Invitation:
                case Message.eMessageType.MutualFriendAnswerAbout:
                default:
                    return 0;
            }
        }

        private Control[] _formElements;
        protected Control[] FormElements
        {
            get
            {
                if (_formElements == null)
                {
                    _formElements = new Control[]
                                   {
                                       this.tblAbout,
                                       this.pnlMutualFriendMessage,
                                       this.mfQuestions,
                                       this.mfAnswers,
                                       this.pnlMessageEdit,
                                       this.pnlSmilies,
                                       this.pnlWinkSelector
                                   };
                }
                return _formElements;
            }
        }
        private void HideFormElements()
        {
            Control[] formElements = FormElements;
            foreach (Control control in formElements)
            {
                control.Visible = false;
            }
        }
        private void ViewMutualFriendAskForm()
        {
            this.pnlMutualFriendMessage.Visible = true;
            this.tblAbout.Visible = true;
            this.tblAbout.Style["visibility"] = "visible";
            this.mfQuestions.Visible = true;
        }
        private void ViewMutualFriendAnswerForm()
        {
            this.pnlMutualFriendMessage.Visible = true;
            this.tblAbout.Visible = true;
            this.tblAbout.Style["visibility"] = "visible";
            this.mfAnswers.Visible = true;
        }
        private void ViewWinkMessageForm()
        {
            this.pnlWinkSelector.Visible = true;
        }
        private void ViewInvitationMessageForm()
        { //TODO:missing
        }
        private void ViewMatchMessageForm()
        { //TODO: missing
            this.tblAbout.Style["visibility"] = "visible";
            this.cbSendee1.Visible = true;
            this.cbSendee2.Visible = true;
            this.cbSendee1.Text = "To Send".Translate();
            this.cbSendee2.Text = "To Send".Translate();
            this.cbSendee1.ToolTip = "Check/Uncheck to select which user to send message".Translate();
            this.cbSendee2.ToolTip = "Check/Uncheck to select which user to send message".Translate();
            this.pnlMessageEdit.Visible = true;
            this.tblAbout.Visible = true;
        }
        private void ViewGiftMessageForm()
        {
            this.pnlMessageEdit.Visible = true;
            GiftsGallery1.PanelVisible = true;
        }
        private void ViewMessageEdit()
        {
            this.pnlMessageEdit.Visible = true;
            //this.pnlSmilies.Visible = true;
            loadSmileys();
        }
        private void preparePage()
        {
            HideFormElements();
            switch (MessageType)
            {
                case Message.eMessageType.Wink:
                    ViewWinkMessageForm();
                    break;
                case Message.eMessageType.Gift:
                    ViewGiftMessageForm();
                    break;
                case Message.eMessageType.Invitation:
                    ViewInvitationMessageForm();
                    break;
                case Message.eMessageType.MutualFriendAskAbout:
                    ViewMutualFriendAskForm();
                    break;
                case Message.eMessageType.MutualFriendAnswerAbout:
                    ViewMutualFriendAnswerForm();
                    break;
                case Message.eMessageType.Matchmaking:
                    ViewMatchMessageForm();
                    break;
                case Message.eMessageType.Message:
                default:
                    ViewMessageEdit();
                    break;
            }
            string toUsername = Request.Params["to_user"];

            bool shouldPayWithCredits;
            checkMessagesLimitReached(out shouldPayWithCredits);

            
            #region PaidMemberCheck


            //string username = CurrentUserSession.Username;

            //if (Classes.User.IsNonPaidMember(username))
            //{
            //    if (Classes.User.CanRespondToMail(username, toUsername))
            //    {
            //        if (Config.Users.NonPaidMembersCanUsePrewrittenResponsesOnly)
            //        {
            //            //show prewritten responses and hide message textbox
            //            ShowPreWrittenResponses();
            //        }
            //        else
            //            pnlPreWrittenResponse.Visible = false;
            //    }
            //    else
            //    {
            //        Response.Redirect("ManageProfile.aspx?sel=payment");
            //    }
            //}
            //else if (!Config.Users.PaymentRequired && Config.Credits.Required  
            //    && Config.Users.NonPaidMembersCanUsePrewrittenResponsesOnly 
            //    && CurrentUserSession.Credits < Config.Credits.CreditsPerMessage)
            //{
            //    ShowPreWrittenResponses();
            //}
            //else pnlPreWrittenResponse.Visible = false;

            #endregion



            Classes.User toUser = null;

            if (ToUser != null)
            {
                string inviteFbIDs = "";
                if (IsUserNotRegistered(ToUser.Username))
                    inviteFbIDs += ToUser.Username;

                #region Set photo and nicknames
                ltrPhotoTo.Text = ToUser.PrimaryPhotoUrl;
                lblToAge.Text = ToUser.Age.ToString();
                //using (var db = new ezFixUpDataContext())
                {
                    //lblToUsername.Text = ToUser.Username;
                    string displayName = CurrentUserSession.GetUserDisplayedNameByUsername(ToUser.Username);
                    lblToUsername.Text = displayName;
                }
                ((PageBase)Page).LastViewedUsername = ToUser.Username;
                ((PageBase)Page).LastViewedName = lblToUsername.Text;

                lblToLocation.Text = ToUser.Location;

                if (this.tblAbout.Style["visibility"] == "visible" && AboutUser != null)
                {
                    if (IsUserNotRegistered(AboutUser.Username))
                        inviteFbIDs += "," + AboutUser.Username;

                    ltrAboutPhoto.Text = AboutUser.PrimaryPhotoUrl;
                    //using (var db = new ezFixUpDataContext())
                    {
                        //lblAboutUsername.Text = AboutUser.Username;
                        string displayName = CurrentUserSession.GetUserDisplayedNameByUsername(AboutUser.Username);
                        lblAboutUsername.Text = displayName;
                    }
                    lblAboutAge.Text = AboutUser.Age.ToString();
                    lblAboutLocation.Text = AboutUser.Location;
                }
                string strScript = @"var waitForFB = window.setInterval( function() { if (FB) sendAppRequestToFacebookIds('" + inviteFbIDs + "'); },2000);";
                if (inviteFbIDs.IsNotNullOrEmpty() && !ClientScript.IsStartupScriptRegistered("inviteToFacebookAppScript"))
                    ScriptManager.RegisterStartupScript(btnSend, btnSend.GetType(), "inviteToFacebookAppScript", @strScript, true);
                
                /*
                if (pnlWinkSelector.Visible && ddlLanguage != null &&
                    CurrentUserSession.LanguageId > 0 && CascadingWinkLanguage.SelectedValue.IsNotNullOrEmpty())
                {
                    CascadingWinkLanguage.SelectedValue = CurrentUserSession.LanguageId.ToString();
                }
                 */
                string type = Request.Params["type"];
                if (type == "ask" || type == "answer" || type == "matchmaking")
                {
                    //LargeBoxStart1.Title =
                        //String.Format(LargeBoxStart1.Title, lblToUsername.Text, lblAboutUsername.Text);
                    if (type == "ask" || type == "answer")
                        lblMutualFriendMsgTitle.Text = String.Format(lblMutualFriendMsgTitle.Text, lblAboutUsername.Text);
                }

                #endregion

                #region Load previous messages

                int repliedTo = -1;
                try
                {
                    repliedTo = Convert.ToInt32(Request.Params["src_id"]);
                }
                catch (InvalidCastException)
                {
                }

                if (repliedTo > 0)
                {
                    DataTable dtPrevMessages = new DataTable();
                    dtPrevMessages.Columns.Add("Username");
                    dtPrevMessages.Columns.Add("Message");

                    int messageId = repliedTo;
                    int counter = 0;

                    while (messageId != -1 && messageId != 0 && counter <= 10)
                    {
                        counter++;
                        Message message;
                        try
                        {
                            message = Message.Fetch(messageId);
                        }
                        catch (NotFoundException)
                        {
                            break;
                        }
                        if (message.fromUsername != CurrentUserSession.Username
                            && message.toUsername != CurrentUserSession.Username)
                        {
                            break;
                        }
                        if ((message.fromUsername == CurrentUserSession.Username
                             && message.FromFolder == Message.eFolder.Deleted)
                            || (message.toUsername == CurrentUserSession.Username
                                && message.ToFolder == Message.eFolder.Deleted))
                        {
                            break;
                        }

                        string body = message.Body;
                        body = Server.HtmlEncode(body);
                        body = body.Replace("\n", "<br>");
                        Smilies.Process(ref body);

                        dtPrevMessages.Rows.Add(new object[] { message.fromUsername, body });

                        messageId = message.RepliedTo;
                    }

                    rptPreviousMessages.DataSource = dtPrevMessages;
                    rptPreviousMessages.DataBind();

                    pnlPreviousMessages.Visible = true;
                }

                #endregion
            }
        }

        /// <summary>
        /// Replaces the email.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        private static string ReplaceEmail(string input, string replacement)
        {
            string pattern = @"(([^<>()[\]\\.,;:\s@\""]+"
                               + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                               + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                               + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                               + @"[a-zA-Z]{2,}))";

            return Regex.Replace(input, pattern, replacement);
        }

        /// <summary>
        /// Replaces the phone.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns></returns>
        private static string ReplacePhone(string input, string replacement)
        {
            string result = input;
            string[] patterns = {
                                @"[2-9]\d{2}-\d{3}-\d{4}",                                         //Matches: 800-555-5555|||333-444-5555|||212-666-1234
                                @"((\(\d{3}\) ?)|(\d{3}[- \.]))?\d{3}[- \.]\d{4}(\s(x\d+)?){0,1}", //Matches: (123) 456-7890|||(123) 456-7890 x123
                                @"(\(\d{3}\)[- ]?|\d{3}[- ])?\d{3}[- ]\d{4}"                       //Matches: (555)555-5555|||(555) 555-5555|||555-5555
                                };

            foreach (string pattern in patterns)
            {
                result = Regex.Replace(result, pattern, replacement);
            }

            return result;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            #region Check can current user send message

            User toUser = null;
            User fromUser = null;
            User aboutUser = null;

            try
            {
                toUser = Classes.User.Load(ToUser.Username);
                fromUser = Classes.User.Load(CurrentUserSession.Username);
                if (AboutUser != null)
                    aboutUser = Classes.User.Load(AboutUser.Username);
            }
            catch (NotFoundException)
            {
                Response.Redirect(MatchmakerHelper.CurrentHomePage);
                return;
            }

            string reason;
            if (!Message.MessagesExist(toUser.Username, fromUser.Username) &&
                    !Classes.User.CanSendMessage(fromUser, toUser, out reason) && !CurrentUserSession.IsAdmin())
            {
                if (!Classes.User.IsGroupOwner(fromUser, toUser))
                {
                    StatusPageMessage = reason;
                    //Response.Redirect("~/Mailbox.aspx");
                    return;
                }
            }

            #endregion

            try
            {
                var toUsername = ToUser.Username;
                ((PageBase)Page).LastViewedUsername = ToUser.Username;
                ((PageBase)Page).LastViewedName = lblToUsername.Text;

                if (Classes.User.IsUserBlocked(toUsername, CurrentUserSession.Username))
                {
                    StatusPageMessage =
                        String.Format(Lang.Trans("You are currently blocked from sending messages to {0}"), toUsername);
                    //Response.Redirect("Mailbox.aspx");
                }

                if (MessageType == Message.eMessageType.Matchmaking && AboutUser != null &&
                    Classes.User.IsUserBlocked(AboutUser.Username, CurrentUserSession.Username))
                {
                    StatusPageMessage =
                        String.Format(Lang.Trans("You are currently blocked from sending messages to {0}"), AboutUser.Username);
                    //Response.Redirect("Mailbox.aspx");
                }

                var msg = new Message(CurrentUserSession.Username, toUsername);
                if (aboutUser != null)
                {
                    msg.AboutUser = aboutUser;
                }
                switch (MessageType)
                {

                    case Message.eMessageType.MutualFriendAskAbout:
                        if (aboutUser != null)
                        {
                            msg.AboutUser = aboutUser;
                            IList<CheckBox> questions = new List<CheckBox>
                                                       {
                                                           cbMfQuestion1, cbMfQuestion2, cbMfQuestion3, cbMfQuestion4,
                                                           cbMfQuestion5
                                                       };
                            IList<CheckBox> checkedQuestions = questions.Where(cb => cb.Checked).ToList();
                            msg.Subject = "Inquire about our mutual friend".Translate() + " - " + AboutUser.Username;
                            foreach (CheckBox cb in checkedQuestions)
                            {
                                msg.Subject += " #" + questions.IndexOf(cb);
                                msg.Body += cb.Text + "\r\n";
                            }
                            string inMsgBody = HttpUtility.HtmlEncode(txtMutualFriendMsgContent.Text).Trim();
                            msg.Body += Config.Misc.EnableBadWordsFilterMessage
                                        ? Parsers.ProcessBadWords(inMsgBody)
                                        : inMsgBody;
                        }
                        break;
                    case Message.eMessageType.MutualFriendAnswerAbout:
                        if (aboutUser != null)
                        {
                            msg.AboutUser = aboutUser;
                            IList<WebControl> answers = new List<WebControl>
                                                       {
                                                           rblMfAnswer1, rblMfAnswer2, rblMfAnswer3, 
                                                           txtMfAnswer4,txtMfAnswer5
                                                       };
                            IList<WebControl> enabledAnswers = answers.Where(rb => rb.Enabled).ToList();
                            msg.Subject = "Information about our mutual friend".Translate() + " - " + AboutUser.Username;
                            foreach (WebControl wc in enabledAnswers)
                            {
                                if (wc is TextBox || wc is RadioButton)
                                {
                                    msg.Subject += " #" + answers.IndexOf(wc);
                                    msg.Body += wc.ToolTip + ":\r\n ";
                                    if (wc is TextBox)
                                        msg.Body += (wc as TextBox).Text;
                                    else if (wc is RadioButtonList)
                                        msg.Body += (wc as RadioButtonList).SelectedValue;
                                    msg.Body += "\r\n";
                                }
                            }
                        }
                        break;
                    case Message.eMessageType.Wink:
                        if (ddlWinkPhrase.SelectedIndex < 0) return;
                        msg.Subject = String.Format("{0} #{1}", ddlWinkPhrase.SelectedItem.Text, ddlWinkPhrase.SelectedValue);
                        msg.Body = ddlWinkPhrase.SelectedItem.Text;
                        break;

                    case Message.eMessageType.Gift:
                    case Message.eMessageType.Message:
                    case Message.eMessageType.Matchmaking:
                    default:
                        if (MessageType == Message.eMessageType.Matchmaking)
                            msg.AboutUser = aboutUser;

                        string inputMsgSubject = /*HttpUtility.HtmlEncode(txtSubject.Text).Trim() ??*/
                            (String.Format("{0} To: ", MessageType)).Translate() + lblToUsername.Text;

                        msg.Subject = Config.Misc.EnableBadWordsFilterMessage
                                    ? Parsers.ProcessBadWords(inputMsgSubject)
                                    : inputMsgSubject;

                        string inputMsgBody = HttpUtility.HtmlEncode(txtMessageBody.Text).Trim();
                        if (inputMsgBody.Length == 0 && MessageType != Message.eMessageType.Gift) return;

                        msg.Body = Config.Misc.EnableBadWordsFilterMessage
                                    ? Parsers.ProcessBadWords(inputMsgBody)
                                    : inputMsgBody;

                        if (Config.Misc.EnableMessageFilter)
                        {
                            msg.Body = ReplaceEmail(msg.Body, "***");
                            msg.Body = ReplacePhone(msg.Body, "***");
                            msg.Subject = ReplaceEmail(msg.Subject, "***");
                            msg.Subject = ReplacePhone(msg.Subject, "***");
                        }
                        break;
                }
                if (msg.Subject.IsNullOrEmpty()) msg.Subject = "Empty subject".Translate();
                if (msg.Body.IsNullOrEmpty()) msg.Body = "Empty body".Translate();

                if (Request.Params["src"] != null && Request.Params["src"] == "message"
                    && Request.Params["src_id"] != null)
                {
                    try { msg.RepliedTo = Convert.ToInt32(Request.Params["src_id"]); }
                    catch (InvalidCastException) { }
                }

                bool shouldPayWithCredits;
                checkMessagesLimitReached(out shouldPayWithCredits);
            
                if (shouldPayWithCredits)
                {
                    int creditToPay = GetCreditsPerMaxMessageTypePerDay();
                    int creditsLeft = fromUser.Credits - creditToPay; //Config.Credits.CreditsPerMessage;
                    if (creditsLeft < creditToPay) //was 0..
                    {
                        fromUser.Credits = 1818;
                        fromUser.Update(true);
                        CurrentUserSession.Credits = fromUser.Credits;
                        creditsLeft = fromUser.Credits - creditToPay;
                    }
                    if (!Config.Credits.ChargeOneTimePerMember) // charge every time
                    {
                        if (creditsLeft < creditToPay) //was 0..
                        {
                            Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay;
                            Response.Redirect("~/ManageProfile.aspx?sel=payment");
                            return;
                        }

                        fromUser.Credits -= creditToPay; //Config.Credits.CreditsPerMessage;
                        fromUser.Update(true);
                        CurrentUserSession.Credits = fromUser.Credits;
                    }
                    else
                    {
                        bool isCharged = Classes.EstablishedCommunication.Fetch(fromUser.Username, toUser.Username) != null;

                        if (!isCharged)
                        {
                            if (creditsLeft >= 0)
                            {
                                var establishedCommunication =
                                    new Classes.EstablishedCommunication(fromUser.Username, toUser.Username);

                                establishedCommunication.Save();

                                fromUser.Credits -= creditToPay;
                                fromUser.Update(true);
                                CurrentUserSession.Credits = fromUser.Credits;
                            }
                            else
                            {
                                Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay;
                                Response.Redirect("~/ManageProfile.aspx?sel=payment");
                                return;
                            }
                        }
                    }
                }
                //    }
                //}

                if (Config.Misc.EnableSpamDetection)
                {
                    if (fromUser.SpamSuspected)
                    {
                        StatusPageMessage =
                            Lang.Trans(
                                "This account is flagged for review! You will not be able to send messages untill the review is completed!");
                        //Response.Redirect("Mailbox.aspx");
                        return;
                    }

                    string message = Regex.Replace(msg.Body, @"[^\w]*", "").ToLower();

                    if (message.Length > 2000)
                    {
                        message = message.Substring(0, 2000);
                    }

                    int sentMessages = MessagesSandBox.Save(CurrentUserSession.Username, message);

                    if (sentMessages > Config.Misc.MaxSameMessages)
                    {
                        fromUser.SpamSuspected = true;
                        fromUser.Update();

                        StatusPageMessage =
                            Lang.Trans(
                                "This account is flagged for review! You will not be able to send messages untill the review is completed!");
                        //Response.Redirect("Mailbox.aspx");
                        return;
                    }
                }

                Message msg1 = null, msg2 = null;
                if (MessageType != Message.eMessageType.Matchmaking)
                {
                    msg.Send();
                }
                else
                {
                    if (cbSendee1.Checked)
                    {
                        msg1 = new Message(msg.fromUsername, msg.toUsername, msg.aboutUsername);
                        msg1.Subject = msg.Subject;
                        msg1.Body = msg.Body;
                        msg1.Send();
                    }
                    if (cbSendee2.Checked)
                    {
                        msg2 = new Message(msg.fromUsername, msg.aboutUsername, msg.toUsername);
                        msg2.Subject = msg.Subject;
                        msg2.Body = msg.Body;
                        msg2.Send();
                    }
                }
                if (MessageType == Message.eMessageType.Wink)
                {
                    WinkMessage.Insert(msg.Id, int.Parse(ddlWinkPhrase.SelectedValue));
                }
                if (GiftsGallery1.Visible && SelectedGiftsIds != null)
                {
                    foreach (int selectedGiftId in SelectedGiftsIds)
                    {
                        if (MessageType != Message.eMessageType.Matchmaking)
                        {
                            GiftMessage.Insert(msg.Id, selectedGiftId);
                        }
                        else
                        {
                            if (msg1 != null)
                                GiftMessage.Insert(msg1.Id, selectedGiftId);
                            if (msg2 != null)
                                GiftMessage.Insert(msg2.Id, selectedGiftId);
                        }
                        Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.SentGift, "SentGift");
                    }
                }
                Classes.User.AddScore(toUsername, Config.UserScores.ReceivedMessage, "ReceivedMessage");
                if (msg.RepliedTo > 0)
                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.RepliedToMessage, "RepliedToMessage");
                else if (MessageType == Message.eMessageType.Wink)
                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.SentWink, "SentWink");
                else if (MessageType == Message.eMessageType.Message || MessageType == Message.eMessageType.Gift)
                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.SentMessage, "SentMessage");

                lblMsgSent.InnerText = String.Format("{0} - {1}",
                                                  Lang.Trans(MessageType.ToString()),
                                                  Lang.Trans("has been sent successfully!"));
                SelectedGiftsIds = null;
                msgView.Visible = false;
                msgSentResult.Visible = true;
            }
            catch (ThreadAbortException)
            {
            }
            catch (NotFoundException)
            {
                StatusPageMessage = Lang.Trans("The message could not be delivered because the target user was deleted");
            }
            catch (ArgumentException ex)
            {
                StatusPageMessage = ex.Message;
            }
            catch (Exception err)
            {
                StatusPageMessage = Lang.Trans("Message could not be delivered!");
                Log(err);
            }
            string username1 = ToUser.Username;
            string username2 = AboutUser != null ? AboutUser.Username : String.Empty;
            string action = null;

        }
        private bool IsUserNotRegistered(string username)
        {
            User u = Classes.User.Load(username);
            return (u != null && u.LoginCount == 0);            
        }

        protected void ibtnPrevSmilies_Click(object sender, ImageClickEventArgs e)
        {
            CurrentSmiliesPage--;
            loadSmileys();
        }

        protected void ibtnNextSmilies_Click(object sender, ImageClickEventArgs e)
        {
            CurrentSmiliesPage++;
            loadSmileys();
        }

        public static string GetGiftsThumbnailsTable(HttpContext context, string uniqueID, string data)
        {
            StringBuilder sb = new StringBuilder();
            GiftType[] gifts = GiftType.Fetch(true, null);
            sb.AppendLine("<table>");
            int i = 0;
            foreach (GiftType giftType in gifts)
            {
                //Image bitMap = new BitMap();

                Image img = byteArrayToImage(giftType.Content);
                string imgPath = HttpContext.Current.Server.MapPath("images/gift" + giftType.ID + ".jpg");
                img.Save(imgPath, ImageFormat.Jpeg);
                if (i % 5 == 0) sb.Append("<tr>");
                sb.AppendFormat("<td style=\"background-image:url({0});width:{1}px;height:{2}px;\"></td>"
                    , imgPath, img.Width, img.Height);
                if (i % 5 == 4) sb.Append("</tr>");
                i++;
            }
            sb.AppendLine("</table>");
            return sb.ToString();
        }
        public static bool ThumbnailCallback() { return false; }
        //public void Example_GetThumb(PaintEventArgs e)
        //{
        //    Image.GetThumbnailImageAbort myCallback =
        //            new Image.GetThumbnailImageAbort(ThumbnailCallback);
        //    Bitmap myBitmap = new Bitmap("Climber.jpg");
        //    Image myThumbnail = myBitmap.GetThumbnailImage(
        //    40, 40, myCallback, IntPtr.Zero);
        //    e.Graphics.DrawImage(myThumbnail, 150, 75);
        //}
        public static Image byteArrayToImage(byte[] byteArrayIn)
        {
            Image.GetThumbnailImageAbort myCallback = ThumbnailCallback;
            var ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage.GetThumbnailImage(90, 110, myCallback, IntPtr.Zero);
        }
        public static byte[] imageToByteArray(Image imageIn)
        {
            var ms = new MemoryStream();
            imageIn.Save(ms, ImageFormat.Gif);
            return ms.ToArray();
        }

        protected void lbAddGift_Click(object sender, EventArgs e)
        {
            GiftsGallery1.PanelVisible = true;
        }
    }
}