using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Mobile
{
    public partial class Mailbox : PageBase
    {
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

                Session["MessageSearchResultsMobile" + ViewState["MessageSearchResults_guid"]] = value;

                CurrentPage = 1;
            }
            get
            {
                if (ViewState["MessageSearchResults_guid"] != null)
                {
                    return (MessageSearchResults)
                           Session["MessageSearchResultsMobile" + ViewState["MessageSearchResults_guid"]];
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
                ViewState["Messages_CurrentPage"] = value;
                PreparePaginator();
            }
            get
            {
                if (ViewState["Messages_CurrentPage"] == null
                    || (int)ViewState["Messages_CurrentPage"] == 0)
                {
                    return 1;
                }
                return (int)ViewState["Messages_CurrentPage"];
            }
        }

        private Message.eFolder currentFolder
        {
            get
            {
                if (ViewState["mailbox_currentFolder"] != null)
                    return (Message.eFolder)ViewState["mailbox_currentFolder"];
                return Message.eFolder.Inbox;
            }
            set { ViewState["mailbox_currentFolder"] = value; }
        }

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
                int fromUser = (CurrentPage - 1) * messagesPerPage + 1;
                int toUser = CurrentPage * messagesPerPage;
                if (Results.Messages.Length < toUser)
                {
                    toUser = Results.Messages.Length;
                }

                lblPager.Text = String.Format(
                    Lang.Trans("{0}-{1} from {2}"),
                    fromUser, toUser, Results.Messages.Length);
            }
            else
            {
                lblPager.Text = Lang.Trans("No Results");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
                if (!string.IsNullOrEmpty(Page.Request.Params["uid"]))
                {
                    currentFolder = Message.eFolder.Inbox;
                }
                else
                {
                    LoadMessages();
//                    btnDelete.Attributes.Add("onclick",
//                                             String.Format("javascript: return confirm('{0}')",
//                                                           Lang.Trans("Do you really want to delete selected messages?")));
                }
            }
        }

        private void LoadStrings()
        {
            lblTitle.InnerText = Lang.Trans("Inbox");
//            btnDelete.Text = Lang.Trans("Delete selected messages");
            lnkFirst.AlternateText = Lang.Trans("[ First ]");
            lnkPrev.AlternateText = Lang.Trans("[ Prev ]");
            lnkNext.AlternateText = Lang.Trans("[ Next ]");
            lnkLast.AlternateText = Lang.Trans("[ Last ]");
        }

        private void LoadMessages()
        {
            Results.Messages = Message.Search(0, null, Message.eFolder.None, CurrentUserSession.Username,
                                     Message.eFolder.Inbox, 0, true, true, false, null, null);
            CurrentPage = 1;

            if (Results.Messages == null || Results.Messages.Length == 0)
            {
//                btnDelete.Visible = false;
                StatusPageMessage = Lang.Trans("There are no messages in your mailbox!");
                rptMessages.Visible = false;
            }
            else
            {
//                btnDelete.Visible = true;

                bindMessages();

                rptMessages.Visible = true;
            }
        }

        private void bindMessages()
        {
            DataTable dtMessages = new DataTable("Messages");
            dtMessages.Columns.Add("Id");
            dtMessages.Columns.Add("Date", typeof(DateTime));
            dtMessages.Columns.Add("Username");
            dtMessages.Columns.Add("Message");
            dtMessages.Columns.Add("IsRead");
            dtMessages.Columns.Add("PhotoID", typeof(int));

            if (CurrentPage > Results.GetTotalPages(10))
                CurrentPage = Results.GetTotalPages(10);

            Message[] messages = Results.GetPageBulk(CurrentPage, 10);

            foreach (Message msg in messages)
            {
                string body = CurrentUserSession.BillingPlanOptions.CanReadEmail.Value
                                  ? Server.HtmlEncode(msg.Body)
                                  : "Upgrade to read".Translate();
                DateTime timestamp = msg.Timestamp.Add(Config.Misc.TimeOffset);
                string username = msg.FromUser.Username;

                //thumbnail photoID
                int photoId = 0;

                User user;
                try
                {
                    user = Classes.User.Load(username);
                }
                catch (NotFoundException) { continue; }

                Photo photo = null;
                try
                {
                    photo = user.GetPrimaryPhoto();
                }
                catch (NotFoundException)
                {
                }

                #region Check user.Gender and set photoId

                    if (photo == null || !photo.Approved)
                    {
                        photoId = ImageHandler.GetPhotoIdByGender(user.Gender);
                    }
                    else
                    {
                        photoId = photo.Id;
                    }

                    #endregion

                dtMessages.Rows.Add(new object[]
                                        {
                                            msg.Id,
                                            timestamp,
                                            username,
                                            body.Length > 50 ? body.Substring(0, 50) + "..." : body,
                                            currentFolder == Message.eFolder.Inbox ? msg.IsRead : true,
                                            photoId
                                        });
            }

            dtMessages.DefaultView.Sort = "Date DESC";

            rptMessages.DataSource = dtMessages;
            rptMessages.DataBind();
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
                Response.Redirect("Home.aspx");

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
                Response.Redirect("Home.aspx");

            if (CurrentPage < Results.GetTotalPages(10))
            {
                CurrentPage = Results.GetTotalPages(10);
            }
            bindMessages();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            HtmlInputCheckBox cbCheck;
            foreach (DataGridItem item in rptMessages.Items)
            {
                cbCheck = (HtmlInputCheckBox)item.FindControl("cbSelect");
                Message message = Message.Fetch(Convert.ToInt32(cbCheck.Value));

                if (cbCheck.Checked)
                {
                    switch (currentFolder)
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
            }
        }
    }
}
