using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Mobile
{
    public partial class ShowMessage : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadMessage();
                loadStrings();
            }
        }

        private void loadStrings()
        {
            lblTitle.InnerText = Lang.Trans("Message");
            lnkReply.Text = Lang.Trans("Reply");
            lnkDelete.Text = "Delete".Translate();
            lnkBack.Text = Lang.Trans("Back to Mailbox");
        }

        private void loadMessage()
        {
            #region Check if can show message

            if (!CurrentUserSession.BillingPlanOptions.CanReadEmail.Value)
            {
                StatusPageMessage = "You have to be paid member in order to use this feature!".Translate();
                Response.Redirect("Home.aspx");
                return;
            }

            #endregion

            #region Check if message request is legit

            if (string.IsNullOrEmpty(Request.Params["mid"]))
            {
                Response.Redirect("MailBox.aspx");
            }

            int messageId = 0;
            try
            {
                messageId = Convert.ToInt32(Request.Params["mid"]);
            }
            catch
            {
                // Invalid Message ID
                Response.Redirect("MailBox.aspx");
            }

            Message message = null;
            try
            {
                message = Message.Fetch(messageId);
            }
            catch (NotFoundException)
            {
                // Message does not exist
                Response.Redirect("MailBox.aspx");
            }
            catch (Exception err)
            {
                Log(err);
                Response.Redirect("MailBox.aspx");
            }

            if (message.FromUser.Username != CurrentUserSession.Username
                && message.ToUser.Username != CurrentUserSession.Username)
            {
                // User is neither the sender nor the recipient
                Response.Redirect("MailBox.aspx");
            }

            #endregion

            #region Disable reply button if the sender is a system user

            foreach (string systemUser in Config.Users.ReservedUsernames)
            {
                if (message.FromUser.Username.ToLower() == systemUser)
                    lnkReply.Enabled = false;
            }

            #endregion

            #region Show message

            Photo primaryPhoto = null;
            try
            {
                primaryPhoto = message.ToUser.Username == CurrentUserSession.Username
                                   ?
                               message.FromUser.GetPrimaryPhoto()
                                   : message.ToUser.GetPrimaryPhoto();
            }
            catch (NotFoundException)
            {
            }
            catch (Exception err)
            {
                Log(err);
            }

            if (primaryPhoto == null || !primaryPhoto.Approved)
            {
                ltrPhoto.Text =
                    ImageHandler.RenderImageTag(message.FromUser.Gender, 90, 90, "ImgBorder", false, true);
            }
            else
            {
                ltrPhoto.Text = ImageHandler.RenderImageTag(primaryPhoto.Id, 90, 90, "ImgBorder", false, true);
            }

            ViewState["ShowMessage_ReplyTo"] = message.ToUser.Username == CurrentUserSession.Username
                                                   ?
                                               message.FromUser.Username
                                                   : message.ToUser.Username;
            ViewState["ShowMessage_MessageId"] = message.Id;
            lblFromUsername.Text = message.FromUser.Username;
            lblToUsername.Text = message.ToUser.Username;
            lblMessageTime.Text = message.Timestamp.Add(Config.Misc.TimeOffset).ToString();
            lnkShowUser.HRef = UrlRewrite.CreateMobileShowUserUrl(message.ToUser.Username == CurrentUserSession.Username
                                                 ? message.FromUser.Username
                                                 : message.ToUser.Username);

            string body = message.Body;
            Parsers.ProcessMessage(ref body);
            lblMessage.Text = body;

            #endregion

            #region Load previous messages

            int repliedTo = message.RepliedTo;

            if (repliedTo > 0)
            {
                DataTable dtPrevMessages = new DataTable();
                dtPrevMessages.Columns.Add("Username");
                dtPrevMessages.Columns.Add("Message");

                int messId = repliedTo;
                int counter = 0;

                while (messId != -1 && messId != 0 && counter <= 10)
                {
                    counter++;
                    Message msg;
                    try
                    {
                        msg = Message.Fetch(messId);
                    }
                    catch (NotFoundException)
                    {
                        break;
                    }
                    if (msg.fromUsername != CurrentUserSession.Username
                        && msg.toUsername != CurrentUserSession.Username)
                    {
                        break;
                    }
                    if ((msg.fromUsername == CurrentUserSession.Username
                         && msg.FromFolder == Message.eFolder.Deleted)
                        || (msg.toUsername == CurrentUserSession.Username
                            && msg.ToFolder == Message.eFolder.Deleted))
                    {
                        break;
                    }

                    string messageBody = msg.Body;
                    messageBody = Server.HtmlEncode(messageBody);
                    messageBody = messageBody.Replace("\n", "<br>");
                    Smilies.Process(ref messageBody);

                    dtPrevMessages.Rows.Add(new object[] { msg.fromUsername, messageBody });

                    messId = msg.RepliedTo;
                }

                rptPreviousMessages.DataSource = dtPrevMessages;
                rptPreviousMessages.DataBind();

                pnlPreviousMessages.Visible = true;
            }


            #endregion

            #region Mark message as read if necessary

            if (!message.IsRead && message.ToUser.Username == CurrentUserSession.Username)
            {
                message.IsRead = true;
            }

            #endregion
        }

        protected void lnkReply_Click(object sender, EventArgs e)
        {
            Response.Redirect("SendMessage.aspx?to_user=" + ViewState["ShowMessage_ReplyTo"]
                              + "&src=message&src_id=" + ViewState["ShowMessage_MessageId"]);
        }

        protected void lnkDelete_Click(object sender, EventArgs e)
        {
            #region Check if message request is legit

            if (string.IsNullOrEmpty(Request.Params["mid"]))
            {
                Response.Redirect("MailBox.aspx");
            }

            int messageId = 0;
            try
            {
                messageId = Convert.ToInt32(Request.Params["mid"]);
            }
            catch
            {
                // Invalid Message ID
                Response.Redirect("MailBox.aspx");
            }

            Message message = null;
            try
            {
                message = Message.Fetch(messageId);
            }
            catch (NotFoundException)
            {
                // Message does not exist
                Response.Redirect("MailBox.aspx");
            }
            catch (Exception err)
            {
                Log(err);
                Response.Redirect("MailBox.aspx");
            }

            if (message.FromUser.Username != CurrentUserSession.Username
                && message.ToUser.Username != CurrentUserSession.Username)
            {
                // User is neither the sender nor the recipient
                Response.Redirect("MailBox.aspx");
            }

            #endregion

            if (message.FromUser.Username == CurrentUserSession.Username)
            {
                message.FromFolder = message.FromFolder == Message.eFolder.Trash
                                         ? Message.eFolder.Deleted
                                         : Message.eFolder.Trash;
            }
            if (message.ToUser.Username == CurrentUserSession.Username)
            {
                message.ToFolder = message.ToFolder == Message.eFolder.Trash
                                       ? Message.eFolder.Deleted
                                       : Message.eFolder.Trash;
            }

            Response.Redirect("Mailbox.aspx");
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Mailbox.aspx");
        }
    }
}
