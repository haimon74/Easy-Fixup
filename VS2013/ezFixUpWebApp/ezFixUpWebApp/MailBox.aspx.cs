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
using System.Data;
using System.Drawing;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;
using System.Collections.Generic;
using Message = ezFixUp.Classes.Message;
using Photo = ezFixUp.Classes.Photo;
using User = ezFixUp.Classes.User;

namespace ezFixUp
{
    public partial class MailBox : PageBase
    {
        private bool updateHistory = true;
        private PermissionCheckResult permission;
        protected bool IsNotWink;

        /// <summary>
        /// Gets or sets the results.
        /// </summary>
        /// <value>The results.</value>
        public MessageSearchResults Results
        {
            set
            {
                if (ViewState["MessageSearchResults_guid"] == null)
                {
                    ViewState["MessageSearchResults_guid"] = Guid.NewGuid().ToString();
                }

                Global.GetSessionState()["MessageSearchResults" + ViewState["MessageSearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["MessageSearchResults_guid"] != null)
                {
                    string key = "MessageSearchResults" + ViewState["MessageSearchResults_guid"];
                    if (Global.GetSessionState(sessionId).ContainsKey(key))
                        return (MessageSearchResults)Global.GetSessionState(sessionId)[key];
                }
                Results = new MessageSearchResults();
                return Results;
            }
        }

        /// <summary>
        /// Gets or sets the current page.
        /// </summary>
        /// <value>The current page.</value>
        public int CurrentPage
        {
            set
            {
                Trace.Write("MailBox.aspx.cs", "CurrentPage = " + value);
                ViewState["Messages_CurrentPage"] = value;
                ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
                if (updateHistory && scriptManager != null && scriptManager.IsInAsyncPostBack)
                {
                    scriptManager.AddHistoryPoint("page", value.ToString());
                }
                PreparePaginator();
            }
            get
            {
                if (ViewState["Messages_CurrentPage"] == null
                    || (int) ViewState["Messages_CurrentPage"] == 0)
                {
                    return 1;
                }
                return (int) ViewState["Messages_CurrentPage"];
            }
        }

        private string searchFromUsername;
        private string searchToUsername;

        protected void Page_Load(object sender, EventArgs e)
        {
            lblError.Text = "";
            permission = CurrentUserSession.CanReadEmail();

            if (!Page.IsPostBack)
            {
                LoadStrings();
                SetActiveLinkButton(lnkInbox);
                //if (Page.Request.Params["sel"] == "recec")
                //    lnkFolder_Click(lnkReceivedEcards, null);
                //else if (Page.Request.Params["sel"] == "sentec")
                //    lnkFolder_Click(lnkSentEcards, null);
                //if (Page.Request.Params["sel"] == "recgft")
                //    lnkFolder_Click(lnkReceivedGifts, null);
                //else if (Page.Request.Params["sel"] == "sentgft")
                //    lnkFolder_Click(lnkSentGifts, null);
                //if (Page.Request.Params["sel"] == "recwnk")
                //    lnkFolder_Click(lnkReceivedWinks, null);
                //else if (Page.Request.Params["sel"] == "sentwnk")
                //    lnkFolder_Click(lnkSentWinks, null);
                if (!string.IsNullOrEmpty(Page.Request.Params["uid"]))
                {
                    currentMessageFolder = Message.eFolder.Inbox;
                    txtSearchMail.Text = Page.Request.Params["uid"];
                    //pnlEcards.Visible = false;
                    btnFilter_Click(null, null);
                }
                else
                {
                    lnkFolder_Click(lnkInbox, null);
                    if (lnkInbox.ID == "lnkTrash")
                        btnDelete.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                                                            "Do you really want to delete selected messages?".Translate()));
                }
            }

            ScriptManager scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
                scriptManager.Navigate += scriptManager_Navigate;
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            SetEcardFolderDictionary();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lnkInbox.Click += new EventHandler(this.lnkFolder_Click);
            this.lnkOutbox.Click += new EventHandler(this.lnkFolder_Click);
            this.lnkTrash.Click += new EventHandler(this.lnkFolder_Click);
            this.btnDelete.Click += new EventHandler(this.btnDelete_Click);
            this.btnUndelete.Click += new EventHandler(this.btnUndelete_Click);
            this.btnMarkUnread.Click += new EventHandler(this.btnMarkUnread_Click);
            //this.lnkReceivedEcards.Click += new EventHandler(lnkFolder_Click);
            //this.lnkSentEcards.Click += new EventHandler(lnkFolder_Click);
            //this.lnkReceivedWinks.Click += new EventHandler(lnkFolder_Click);
            //this.lnkSentWinks.Click += new EventHandler(lnkFolder_Click);
            //this.lnkReceivedGifts.Click += new EventHandler(lnkFolder_Click);
            //this.lnkSentGifts.Click += new EventHandler(lnkFolder_Click);
            //this.btnDeleteEcard.Click += new EventHandler(btnDeleteEcard_Click);
            //this.dgEcards.PageIndexChanged += new DataGridPageChangedEventHandler(dgEcards_PageIndexChanged);
        }

        #endregion

        /// <summary>
        /// Prepares the paginator.
        /// </summary>
        private void PreparePaginator()
        {
            int messagesPerPage = 10;
            if (Results == null || CurrentPage <= 1)
            {
                lnkFirst.Enabled = false;
                lnkPrev.Enabled = false;
            }
            else
            {
                lnkFirst.Enabled = true;
                lnkPrev.Enabled = true;
            }
            if (Results == null || CurrentPage >= Results.GetTotalPages(messagesPerPage))
            {
                lnkLast.Enabled = false;
                lnkNext.Enabled = false;
            }
            else
            {
                lnkLast.Enabled = true;
                lnkNext.Enabled = true;
            }
            if (Results != null && Results.Messages.Length > 0)
            {
                int fromUser = (CurrentPage - 1)*messagesPerPage + 1;
                int toUser = CurrentPage*messagesPerPage;
                if (Results.Messages.Length < toUser)
                {
                    toUser = Results.Messages.Length;
                }

                lblPager.Text = String.Format(
                    Lang.Trans("Showing {0}-{1} from {2} total"),
                    fromUser, toUser, Results.Messages.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        private void LoadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Folders");
            lnkInbox.Text = Lang.Trans("Inbox");
            lnkOutbox.Text = Lang.Trans("Outbox");
            lnkTrash.Text = Lang.Trans("Trash");
            btnDelete.Text = Lang.Trans("Delete");
            btnUndelete.Text = Lang.Trans("Undelete");
            btnDelete.ToolTip = Lang.Trans("Delete selected messages");
            btnMarkUnread.Text = Lang.Trans("Unread");
            btnMarkUnread.ToolTip = Lang.Trans("Set selected messages as Unread Message");
            
            //lnkReceivedEcards.Text = Lang.Trans("Received e-cards");
            //lnkSentEcards.Text = Lang.Trans("Sent e-cards");
            //lnkReceivedWinks.Text = Lang.Trans("Received Winks");
            //lnkSentWinks.Text = Lang.Trans("Sent Winks");
            //lnkReceivedGifts.Text = Lang.Trans("Received Gifts");
            //lnkSentGifts.Text = Lang.Trans("Sent Gifts");
            //btnDeleteEcard.Text = Lang.Trans("Delete");
        
            btnFilter.Text = Lang.Trans("Filter");
            btnClearFilter.Text = Lang.Trans("Clear");
            lnkFirst.Text = Lang.Trans("[ First ]");
            lnkPrev.Text = Lang.Trans("[ Prev ]");
            lnkNext.Text = Lang.Trans("[ Next ]");
            lnkLast.Text = Lang.Trans("[ Last ]");

            //lnkReceivedEcards.Visible = true;
            //lnkSentEcards.Visible = CurrentUserSession.CanSendEcards() != PermissionCheckResult.No;
            //lnkReceivedWinks.Visible = true;
            //lnkSentWinks.Visible = CurrentUserSession.CanSendWinks() != PermissionCheckResult.No;
            //lnkReceivedGifts.Visible = true;
            //lnkSentGifts.Visible = CurrentUserSession.CanSendGifts() != PermissionCheckResult.No;
        }

        private Message.eFolder currentMessageFolder
        {
            get
            {
                return (Message.eFolder) (ViewState["messages_currentFolder"]
                                          ?? (ViewState["messages_currentFolder"] = Message.eFolder.Inbox));
            }
            set { ViewState["messages_currentFolder"] = value; }
        }

        //protected Ecard.eFolder currentEcardFolder
        //{
        //    get
        //    {
        //        return (Ecard.eFolder)(ViewState["ecard_currentFolder"]
        //                        ?? (ViewState["ecard_currentFolder"] = Ecard.eFolder.Received));
        //    }
        //    set { ViewState["ecard_currentFolder"] = value; }
        //}
        
        protected LinkButton CurrentActiveFolderLinkButton
        {
            get
            {
                string lbId = (string) (ViewState["mailbox_activeFolder"]
                                        ?? (ViewState["mailbox_activeFolder"] = lnkInbox.ID));
                LinkButton lb = (LinkButton)upMenu.FindControl(lbId);
                return lb;
            }
            set { ViewState["mailbox_activeFolder"] = value.ID; }
        }

        private void bindMessages()
        {
            DataTable dtMessages = new DataTable("Messages");
            dtMessages.Columns.Add("Id");
            dtMessages.Columns.Add("Date", typeof (DateTime));
            dtMessages.Columns.Add("Username");
            dtMessages.Columns.Add("DisplayName");
            dtMessages.Columns.Add("Subject");
            dtMessages.Columns.Add("Message");
            dtMessages.Columns.Add("IsRead");
            dtMessages.Columns.Add("IsDeleted");
            dtMessages.Columns.Add("IsRepliedTo");
            dtMessages.Columns.Add("MessageTypeIconClass");
            dtMessages.Columns.Add("ImageTag");
            dtMessages.Columns.Add("MessageTypeID", typeof (int));
            
            if (CurrentPage > Results.GetTotalPages(10))
                CurrentPage = Results.GetTotalPages(10);

            Message[] messages = Results.GetPageBulk(CurrentPage, 10);

            foreach (Message msg in messages)
            {
                string subject = Server.HtmlEncode(msg.Subject ?? String.Empty);

                string body = String.Empty;

                if (permission == PermissionCheckResult.Yes || msg.fromUsername == Config.Users.SystemUsername ||
                    permission == PermissionCheckResult.YesWithCredits && msg.IsRead)
                    body = Server.HtmlEncode(msg.Body);
                else if (permission == PermissionCheckResult.No)
                    body = "You are not allowed to view messages".Translate();
                else if (permission == PermissionCheckResult.YesButMoreCreditsNeeded &&
                         CurrentUserSession.BillingPlanOptions.CanReadEmail.UpgradableToNextPlan)
                    body = "Upgrade your plan or buy more credits to read the message".Translate();
                else if (permission == PermissionCheckResult.YesButMoreCreditsNeeded)
                    body = "Buy more credits to read the message".Translate();
                else if (permission == PermissionCheckResult.YesButPlanUpgradeNeeded)
                    body = "Upgrade to read".Translate();
                else if (permission == PermissionCheckResult.YesWithCredits)
                    body = "Click here to read the message".Translate();

                DateTime timestamp = msg.Timestamp.Add(Config.Misc.TimeOffset);
                string username = currentMessageFolder == Message.eFolder.Outbox
                                      ? msg.ToUser.Username
                                      : msg.FromUser.Username;
                //thumbnail photoID
                int photoId = 0;
                string imageUrl, imageTag = null;

                if (Config.Photos.ShowThumbnailsInMailbox)
                {
                    User user;
                    try
                    {
                        user = Classes.User.Load(username);
                    }
                    catch (NotFoundException)
                    {
                        continue;
                    }

                    Photo photo = null;
                    bool hasPhoto = false;
                    try
                    {
                        photo = user.GetPrimaryPhoto();
                        hasPhoto = photo!=null && photo.Approved ? true : false;
                    }
                    catch (NotFoundException)
                    {
                    }
                    
                    #region render Image Tag

                    photoId = hasPhoto ? photo.Id : ImageHandler.GetPhotoIdByGender(user.Gender);

                    if (!hasPhoto && user.FacebookID.HasValue && user.FacebookID.Value > 0)
                    {
                        imageUrl = ImageHandler.CreateFacebookImageUrl(user.FacebookID.Value,
                                                                       ImageHandler.eFacebookImageType.Square);
                        imageTag = String.Format("<img src=\"{0}\" class=\"{1}\" border=\"0\" />", imageUrl, "");
                    }
                    else
                    {
                        imageTag = ImageHandler.RenderImageTag(photoId, 50, 50, null,
                                                                photoId <= 0, true, true);
                    }                    
                    
                    #endregion
                }

                var permissionCheckResult = CurrentUserSession.CanSeeMessageStatus();

                bool canSeeMessageStatus = permissionCheckResult == PermissionCheckResult.Yes
                                           ||
                                           (CurrentUserSession.Level != null &&
                                            CurrentUserSession.Level.Restrictions.CanSeeMessageStatus);
                bool isRead = false;
                bool isDeleted = false;
                bool isRepliedTo = false;
                if (!canSeeMessageStatus)
                {
                    isRead = currentMessageFolder == Message.eFolder.Inbox ? msg.IsRead : true;
                }
                else
                {
                    isRead = msg.IsRead;
                    if (currentMessageFolder == Message.eFolder.Outbox && msg.ToFolder == Message.eFolder.Trash)
                    {
                        isDeleted = true;
                    }
                    isRepliedTo = Message.HasRaplies(msg.Id, msg.toUsername, msg.fromUsername);
                }
                //using (var db = new ezFixUpDataContext())
                {
                    var displayName = CurrentUserSession.GetUserDisplayedNameByUsername(username);
                    dtMessages.Rows.Add(new object[]
                                            {
                                                msg.Id,
                                                timestamp,
                                                username,
                                                displayName,
                                                (subject.Length > 20)
                                                    ? subject.Substring(0, 20) + "..."
                                                    : subject,
                                                (body.Length > 50)
                                                    ? body.Substring(0, 50) + "..."
                                                    : body,
                                                isRead,
                                                isDeleted,
                                                isRepliedTo,
                                                "TBD_MessageTypeIconClass",
                                                imageTag,
                                                msg.MessageTypeId
                                            });
                }
            }

            dtMessages.DefaultView.Sort = "Date DESC";

            gridMessages.DataSource = dtMessages;
            gridMessages.DataBind();

            if (!Config.Photos.ShowThumbnailsInMailbox)
            {
                foreach (DataGridColumn column in gridMessages.Columns)
                    if (column.HeaderText == Lang.Trans("Photo"))
                        column.Visible = false;
            }
        }

        private void SetActiveLinkButton(LinkButton lbActive)
        {
            //lnkReceivedEcards.CssClass = "SideMenuLink"
            ;
            //lnkSentEcards.CssClass = "SideMenuLink";
            //lnkReceivedWinks.CssClass = "SideMenuLink";
            //lnkSentWinks.CssClass = "SideMenuLink";
            //lnkReceivedGifts.CssClass = "SideMenuLink";
            //lnkSentGifts.CssClass = "SideMenuLink";
            lnkInbox.CssClass = "SideMenuLink";
            lnkOutbox.CssClass = "SideMenuLink";
            lnkTrash.CssClass = "SideMenuLink";
            lbActive.CssClass = "SideMenuLink active";
        }

        protected class MessageFolderClass
        {
            public string NotFoundErrorMsg { get; set; }
            public string Title { get; set; }
            public string[] ColumnsHeaders { get; set; }
            public LinkButton LinkClicked { get; set; }
            public DataGrid ItsDataGrid { get; set; }
        }

        private Dictionary<LinkButton, MessageFolderClass> FoldersData;

        private void SetEcardFolderDictionary()
        {
            FoldersData = new Dictionary<LinkButton, MessageFolderClass>
                              {
                                  {
                                      lnkInbox, new MessageFolderClass
                                                    {
                                                        NotFoundErrorMsg = "There are no messages in your mailbox!",
                                                        Title = "Inbox",
                                                        ItsDataGrid = gridMessages,
                                                        LinkClicked = lnkInbox,
                                                        ColumnsHeaders = new[] {"Photo", "Date", "Sender", "Message"}
                                                    }
                                      },
                                  {
                                      lnkOutbox, new MessageFolderClass
                                                     {
                                                         NotFoundErrorMsg = "There are no messages in your outbox!",
                                                         Title = "Outbox",
                                                         ItsDataGrid = gridMessages,
                                                         LinkClicked = lnkOutbox,
                                                         ColumnsHeaders =
                                                             new[] {"Photo", "Date", "Recipient", "Message"}
                                                     }
                                      },
                                  {
                                      lnkTrash, new MessageFolderClass
                                                    {
                                                        NotFoundErrorMsg = "There are no messages in the trash!",
                                                        Title = "Trash",
                                                        ItsDataGrid = gridMessages,
                                                        LinkClicked = lnkTrash,
                                                        ColumnsHeaders = new[] {"Photo", "Date", "Sender", "Message"}
                                                    }
                                      // },
                                      // {lnkReceivedEcards, new MessageFolderClass{
                                      //    NotFoundErrorMsg = "There are no received e-cards.", 
                                      //    Title = "Received e-cards",
                                      //    ItsDataGrid = dgEcards,
                                      //    LinkClicked= lnkReceivedEcards,
                                      //    ColumnsHeaders = new[]{"User","Age","E-card","Date","Last Online"}
                                      //}
                                      //},
                                      // {lnkSentEcards, new MessageFolderClass{
                                      //     NotFoundErrorMsg = "There are no sent e-cards.", 
                                      //     Title = "Sent e-cards",
                                      //     ItsDataGrid = dgEcards,
                                      //     LinkClicked = lnkSentEcards,
                                      //     ColumnsHeaders = new[]{"User","Age","E-card","Date","Last Online"}
                                      // }
                                      // },
                                      // {lnkReceivedGifts, new MessageFolderClass{
                                      //     NotFoundErrorMsg = "There are no received gifts.", 
                                      //     Title = "Received Gifts",
                                      //     ItsDataGrid = dgEcards,
                                      //     LinkClicked = lnkReceivedGifts,
                                      //     ColumnsHeaders = new[]{"User","Age","Gift","Date","Last Online"}
                                      // }
                                      // },
                                      // {lnkSentGifts, new MessageFolderClass{
                                      //     NotFoundErrorMsg = "There are no sent gifts.", 
                                      //     Title = "Sent Gifts",
                                      //     ItsDataGrid = dgEcards,
                                      //     LinkClicked = lnkSentGifts,
                                      //     ColumnsHeaders = new[]{"User","Age","Gift","Date","Last Online"}
                                      // }
                                      // },
                                      // {lnkReceivedWinks, new MessageFolderClass{
                                      //     NotFoundErrorMsg = "There are no received winks.", 
                                      //     Title = "Received Winks",
                                      //     ItsDataGrid = dgEcards,
                                      //     LinkClicked = lnkReceivedWinks,
                                      //     ColumnsHeaders = new[]{"User","Age","Wink","Date","Last Online"}
                                      // }
                                      // },
                                      // {lnkSentWinks, new MessageFolderClass{
                                      //     NotFoundErrorMsg = "There are no sent winks.", 
                                      //     Title = "Sent Winks",
                                      //     ItsDataGrid = dgEcards,
                                      //     LinkClicked = lnkSentWinks,
                                      //     ColumnsHeaders = new[]{"User","Age","Wink","Date","Last Online"}
                                      // }
                                      }
                              };
        }

        private void btnMarkUnread_Click(object sender, EventArgs e)
        {
            if (currentMessageFolder != Message.eFolder.Inbox)
                return;

            HtmlInputCheckBox cbCheck;
            foreach (DataGridItem item in gridMessages.Items)
            {
                cbCheck = (HtmlInputCheckBox) item.FindControl("cbSelect");
                if (!cbCheck.Checked) continue;

                Message message = Message.Fetch(Convert.ToInt32(cbCheck.Value));
                if (message == null) continue;
                message.IsRead = false;
            }
            lnkFolder_Click(CurrentActiveFolderLinkButton, null);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            HtmlInputCheckBox cbCheck;
            foreach (DataGridItem item in gridMessages.Items)
            {
                cbCheck = (HtmlInputCheckBox) item.FindControl("cbSelect");
                if (!cbCheck.Checked) continue;

                Message message = Message.Fetch(Convert.ToInt32(cbCheck.Value));
                if (message == null) continue;
                switch (currentMessageFolder)
                {
                    case Message.eFolder.Inbox:
                        message.ToFolder = Message.eFolder.Trash;
                        break;
                    case Message.eFolder.Outbox:
                        message.FromFolder = Message.eFolder.Deleted;
                        break;
                    case Message.eFolder.Trash:
                        message.ToFolder = Message.eFolder.Deleted;
                        break;
                }
            }
            lnkFolder_Click(CurrentActiveFolderLinkButton, null);
        }
        private void btnUndelete_Click(object sender, EventArgs e)
        {
            HtmlInputCheckBox cbCheck;
            foreach (DataGridItem item in gridMessages.Items)
            {
                cbCheck = (HtmlInputCheckBox) item.FindControl("cbSelect");
                if (!cbCheck.Checked) continue;

                Message message = Message.Fetch(Convert.ToInt32(cbCheck.Value));
                if (message == null) continue;
                switch (currentMessageFolder)
                {
                    case Message.eFolder.Trash:
                    default:
                        message.ToFolder = Message.eFolder.Inbox;
                        break;
                }
            }
            lnkFolder_Click(CurrentActiveFolderLinkButton, null);
        }

//        private void bindEcards(Ecard[] ecards)
//        {
//            DataTable dtEcards = new DataTable("Ecards");
//            dtEcards.Columns.Add("Id");
//            dtEcards.Columns.Add("Username");
//            dtEcards.Columns.Add("Age");
//            dtEcards.Columns.Add("EcardTypeID");
//            dtEcards.Columns.Add("EcardTypeName");
//            dtEcards.Columns.Add("IsOpened");
//            dtEcards.Columns.Add("DatePosted", typeof(DateTime));
//            dtEcards.Columns.Add("LastOnline");

//            foreach (Ecard ecard in ecards)
//            {
//                EcardType ecardType = EcardType.Fetch(ecard.EcardTypeID);

//                if (ecardType != null)
//                {
//                    DateTime datePosted = ecard.Date.Add(Config.Misc.TimeOffset);
//                    string otherUsername = currentEcardFolder == Ecard.eFolder.Received
//                                               ? ecard.FromUsername
//                                               : ecard.ToUsername;
//                    User user = Classes.User.Load(otherUsername);

//                    dtEcards.Rows.Add(new object[]
//                                         {
//                                             ecard.ID,
//                                             otherUsername,
//                                             user.Age,
//                                             ecardType.ID,
//                                             ecardType.Name,
//                                             ecard.IsOpened,
//                                             datePosted,
//                                             user.LastOnlineString
//                                         });    
//                }
//            }
////            dtEcards.DefaultView.Sort = "DatePosted DESC";

//            if (ecards.Length <= (dgEcards.CurrentPageIndex * dgEcards.PageSize))
//                dgEcards.CurrentPageIndex--;

//            dgEcards.DataSource = dtEcards;
//            dgEcards.DataBind();
//        }
        private void lnkFolder_Click(object sender, EventArgs e)
        {
            if (sender == null || !(sender is LinkButton))
                return;

            var lb = sender as LinkButton;
            CurrentActiveFolderLinkButton = lb;
            SetActiveLinkButton(lb);
            //Ecard[] ecards = null;
            bool hasMessages = false;
            DataGrid dg = FoldersData[lb].ItsDataGrid;
            bool isEcard = (lb != lnkInbox) && (lb != lnkOutbox) && (lb != lnkTrash);

            //if (isEcard)
            //{
            //    if (lb == lnkReceivedEcards || lb == lnkReceivedWinks || lb == lnkReceivedGifts)
            //    {
            //        currentEcardFolder = Ecard.eFolder.Received;
            //        if (lb == lnkReceivedGifts)
            //            ecards = Ecard.FetchReceived(CurrentUserSession.Username, EcardType.eType.Gift);
            //        else if (lb == lnkReceivedWinks )
            //            ecards = Ecard.FetchReceived(CurrentUserSession.Username, EcardType.eType.Wink);
            //        else ecards = Ecard.FetchReceivedEcards(CurrentUserSession.Username);
            //    }
            //    else if (lb == lnkSentEcards || lb == lnkSentGifts || lb == lnkSentWinks)
            //    {
            //        currentEcardFolder = Ecard.eFolder.Sent;
            //        if (lb == lnkSentGifts)
            //            ecards = Ecard.FetchSent(CurrentUserSession.Username, EcardType.eType.Gift);
            //        else if (lb == lnkSentWinks)
            //            ecards = Ecard.FetchSent(CurrentUserSession.Username, EcardType.eType.Wink);
            //        else ecards = Ecard.FetchSentEcards(CurrentUserSession.Username);
            //    }
            //    pnlEcards.Visible = true;
            //    pnlMailBox.Visible = false;
            //    dgEcards.CurrentPageIndex = 0;
            //    hasMessages = ecards != null && ecards.Length > 0;
            //    btnDeleteEcard.Visible = hasMessages;
            //    LargeBoxStart2.Title = Lang.Trans(FoldersData[lb].Title);
            //    lblMessage2.Text = hasMessages ? "" : Lang.Trans(FoldersData[lb].NotFoundErrorMsg);
            //    if (hasMessages)
            //    {
            //        CreateGridColumns(lb, dg);
            //        bindEcards(ecards);
            //    }
            //}
            //else
            {
                //pnlEcards.Visible = false;
                pnlMailBox.Visible = true;
                gridMessages.CurrentPageIndex = 0;
                LargeBoxStart1.Title = Lang.Trans(FoldersData[lb].Title);
                string fromUser;
                string toUser;
                Message.eFolder fromFolder;
                Message.eFolder toFolder;
                btnMarkUnread.Visible = lb.ID == "lnkInbox";

                switch (lb.ID)
                {
                    case "lnkInbox":
                        currentMessageFolder = Message.eFolder.Inbox;
                        fromUser = searchFromUsername;
                        fromFolder = Message.eFolder.None;
                        toUser = CurrentUserSession.Username;
                        toFolder = Message.eFolder.Inbox;
                        break;
                    case "lnkOutbox":
                        currentMessageFolder = Message.eFolder.Outbox;
                        fromUser = CurrentUserSession.Username;
                        fromFolder = Message.eFolder.Outbox;
                        toUser = searchToUsername;
                        toFolder = Message.eFolder.None;
                        break;
                    default:
                    case "lnkTrash":
                        currentMessageFolder = Message.eFolder.Trash;
                        fromUser = null;
                        fromFolder = Message.eFolder.None;
                        toUser = CurrentUserSession.Username;
                        toFolder = Message.eFolder.Trash;
                        break;
                }
                Results.Messages = Message.Search(0, fromUser, fromFolder, toUser, toFolder, 0, true, true, false, null,
                                                  null);
                CurrentPage = 1;
                hasMessages = Results.Messages != null && Results.Messages.Length > 0;
                btnUndelete.Visible = hasMessages && (lb == lnkTrash);
                btnDelete.Visible = hasMessages;
                btnMarkUnread.Visible = hasMessages && (lb == lnkInbox);
                lblError.Text = hasMessages ? "" : Lang.Trans(FoldersData[lb].NotFoundErrorMsg);
                if (hasMessages)
                {
                    CreateGridColumns(lb, dg);
                    bindMessages();
                    lblError.Visible = false;
                }
                else lblError.Visible = true;
            }
            dg.Visible = hasMessages;
        }

        private void CreateGridColumns(LinkButton lb, DataGrid dg)
        {
            for (int idx = 1; idx <= FoldersData[lb].ColumnsHeaders.Length; idx++)
            {
                string columnsHeader = FoldersData[lb].ColumnsHeaders[idx - 1];
                dg.Columns[idx].HeaderText = Lang.Trans(columnsHeader);
            }
        }

        //private void btnDeleteEcard_Click(object sender, EventArgs e)
        //{
        //    HtmlInputCheckBox cbCheck;
        //    foreach (DataGridItem item in dgEcards.Items)
        //    {
        //        cbCheck = (HtmlInputCheckBox)item.FindControl("cbSelect2");

        //        Ecard ecard = Ecard.Fetch(Convert.ToInt32(cbCheck.Value));

        //        if (cbCheck.Checked && ecard != null)
        //        {
        //            switch (currentEcardFolder)
        //            {
        //                case Ecard.eFolder.Received:
        //                    ecard.DeleteFromReceivedEcards();
        //                    break;
        //                case Ecard.eFolder.Sent:
        //                    ecard.DeleteFromSentEcards();
        //                    break;
        //            }
        //        }
        //    }
        //    lnkFolder_Click(currentActiveFolderLinkButton, null);
        //}
        //private void dgEcards_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        //{
        //    dgEcards.CurrentPageIndex = e.NewPageIndex;
        //    lnkFolder_Click(currentActiveFolderLinkButton, null);
        //}
        protected void btnFilter_Click(object sender, EventArgs e)
        {
            gridMessages.CurrentPageIndex = 0;

            if (txtSearchMail.Text.Trim() != "")
            {
                if (currentMessageFolder == Message.eFolder.Inbox)
                {
                    searchToUsername = null;
                    searchFromUsername = txtSearchMail.Text.Trim();
                }
                else if (currentMessageFolder == Message.eFolder.Outbox)
                {
                    searchFromUsername = null;
                    searchToUsername = txtSearchMail.Text.Trim();
                }
            }
            lnkFolder_Click(CurrentActiveFolderLinkButton, null);
        }

        protected void btnClearFilter_Click(object sender, EventArgs e)
        {
            gridMessages.CurrentPageIndex = 0;
            txtSearchMail.Text = "";
            lnkFolder_Click(CurrentActiveFolderLinkButton, null);
        }

        /// <summary>
        /// Handles the Click event of the lnkFirst control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage = 1;
            }
            bindMessages();
        }

        /// <summary>
        /// Handles the Click event of the lnkPrev control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkPrev_Click(object sender, EventArgs e)
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
            }
            bindMessages();
        }

        /// <summary>
        /// Handles the Click event of the lnkNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect(MatchmakerHelper.CurrentHomePage);

            if (CurrentPage < Results.GetTotalPages(10))
            {
                CurrentPage++;
            }
            bindMessages();
        }

        /// <summary>
        /// Handles the Click event of the lnkLast control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void lnkLast_Click(object sender, EventArgs e)
        {
            if (Results == null)
                Response.Redirect(MatchmakerHelper.CurrentHomePage);

            if (CurrentPage < Results.GetTotalPages(10))
            {
                CurrentPage = Results.GetTotalPages(10);
            }
            bindMessages();
        }

        private void scriptManager_Navigate(object sender, HistoryEventArgs e)
        {
            if (!pnlMailBox.Visible) return;

            int historyPage = 0;
            try
            {
                historyPage = e.State.Count == 0 ? 1 : Convert.ToInt32(e.State[0]);
            }
            catch (FormatException)
            {
            }
            if (historyPage < 0 || historyPage > Results.GetTotalPages(10)) historyPage = 0;

            updateHistory = false;
            CurrentPage = historyPage;
            bindMessages();
        }

        protected void gridMessages_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.DataItem != null)
            {
                bool isUnread = !Boolean.Parse((string) DataBinder.Eval(e.Item.DataItem, "IsRead"));
                e.Item.BackColor = isUnread ? Color.FromArgb(230,241,245) : Color.Transparent;
                
                if (permission == PermissionCheckResult.YesWithCredits && isUnread &&
                    ((string) DataBinder.Eval(e.Item.DataItem, "Username")) != Config.Users.SystemUsername &&
                    (currentMessageFolder == Message.eFolder.Inbox ||
                     currentMessageFolder == Message.eFolder.Trash &&
                     ((string) DataBinder.Eval(e.Item.DataItem, "Username")) != CurrentUserSession.Username))
                {
                    var anchor = e.Item.FindControl("lnkReadMessage") as HtmlAnchor;
                    if (anchor != null && CurrentUserSession.BillingPlanOptions.CanReadEmail.Credits > 0)
                    {
                        string onclick =
                            String.Format(
                                "return confirm('" +
                                "Reading this message will subtract {0} credits from your balance.".Translate() + "');",
                                CurrentUserSession.BillingPlanOptions.CanReadEmail.Credits);

                        anchor.Attributes.Add("onclick", onclick);
                    }
                }
            }
        }
    }
}