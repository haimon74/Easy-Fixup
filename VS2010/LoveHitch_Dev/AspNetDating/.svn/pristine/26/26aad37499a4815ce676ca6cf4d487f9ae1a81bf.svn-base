using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using AspNetDating.Classes;

namespace AspNetDating.Mobile
{
    public partial class SendMessage : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                checkRequestLegit();
                loadStrings();
                preparePage();
            }
        }

        private void checkMessagesLimitReached(out bool shouldPayWithCredits)
        {
            shouldPayWithCredits = false;

            if (CurrentUserSession.Username == Config.Users.SystemUsername)
                return;

            if (Config.Users.FreeForFemales && CurrentUserSession.Gender == Classes.User.eGender.Female)
                return;

            int maxBillingPlanMessages = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Value;
            int maxLevelMessages = 0;
            if (CurrentUserSession.Level != null)
                maxLevelMessages = CurrentUserSession.Level.Restrictions.MaxMessagesPerDay;

            if (maxBillingPlanMessages == -1 || maxLevelMessages == -1)
                return;

            int maxMessages = Math.Max(maxBillingPlanMessages, maxLevelMessages);

            Message[] messages =
                Message.FetchSentMessagesForLast24Hours(CurrentUserSession.Username, 1);

            if (messages.Length >= maxMessages)
            {
                if (CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.EnableCreditsPayment &&
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
                        if (CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.EnableCreditsPayment)
                        {
                            StatusPageMessage = "You need more credits in order to use this feature!".Translate();
                            Response.Redirect("Home.aspx");
                            return;
                        }
                        else
                        {
                            StatusPageMessage = Lang.Trans("You've exceeded the number of messages you can send per day!");
                            Response.Redirect("Home.aspx");
                            return;
                        }
                    }
                }
            }

            //if (Classes.User.IsNonPaidMember(CurrentUserSession.Username))
            //{
            //    if (messages.Length >= Config.Users.UnpaidMembersMaxMessagesPerDay)
            //    {
            //        StatusPageMessage = "You have to be paid member in order to use this feature!".Translate();
            //        Response.Redirect("Home.aspx");
            //    }
            //}

            //if (messages.Length >= Config.Users.MembersMaxMessagesPerDay)
            //{
            //    StatusPageMessage = Lang.Trans("You've exceeded the number of messages you can send per day!");
            //}

            #region checkContactedUsersLimitReached

            List<string> uniqueUsers = new List<string>();

            foreach (Message message in messages)
                AddUniqueItems(uniqueUsers, message.ToUser.Username);

            if (uniqueUsers.Count >= Config.Users.MaxContactedUsersPerDay)
            {
                StatusPageMessage = Lang.Trans("You've exceeded the number of users you can contact per day!");
                Response.Redirect("Home.aspx");
                return;
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

        private void loadStrings()
        {
            lblTitle.InnerText = Lang.Trans("Message");
            btnSend.Text = Lang.Trans("Send Message");
            //lnkUpgradeNow.Text = Lang.Trans("Upgrade Now to Respond to this email");

            if (Request.Params["src"] != null && Request.Params["src"] == "message")
            {
                lnkBack.Text = Lang.Trans("Back to Message");
                lnkBack.CommandArgument = "message";
            }
            else if (Request.Params["src"] == "profile")
            {
                lnkBack.Text = Lang.Trans("Back to Profile");
                lnkBack.CommandArgument = "profile";
            }
            else if (Request.Params["src"] == "favorites")
            {
                lnkBack.Text = Lang.Trans("Back to Favorites");
                lnkBack.CommandArgument = "favorites";
            }
            else if (Request.Params["src"] == "friends")
            {
                lnkBack.Text = Lang.Trans("Back to Friends");
                lnkBack.CommandArgument = "friends";
            }
        }

        private void checkRequestLegit()
        {
            #region Check if message request is legit

            if (string.IsNullOrEmpty(Request.Params["to_user"]))
            {
                Response.Redirect("MailBox.aspx");
                return;
            }

            string toUsername = Request.Params["to_user"];
            User toUser = null;
            try
            {
                toUser = Classes.User.Load(toUsername);
                if (toUser.Deleted)
                {
                    ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists");
                    return;
                }
            }
            catch (NotFoundException)
            {
                ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists");
                return;
            }
            catch (ThreadAbortException)
            {
                return;
            }
            catch (Exception err)
            {
                Log(err);
                Response.Redirect("MailBox.aspx");
                return;
            }
            ViewState["SendMessage_ToUsername"] = toUser.Username;

            #endregion

            #region Check can current user send message

            User fromUser = null;
            try
            {
                fromUser = Classes.User.Load(CurrentUserSession.Username);
            }
            catch (NotFoundException)
            {
                Response.Redirect(MatchmakerHelper.CurrentHomePage);
                return;
            }

            string reason = "";
            if (!Message.MessagesExist(toUser.Username, fromUser.Username)
                && !Classes.User.CanSendMessage(fromUser, toUser, out reason)
                && !Classes.User.IsUserInFavouriteList(toUser.Username, fromUser.Username)
                && !CurrentUserSession.IsAdmin())
            {
                if (!Classes.User.IsGroupOwner(fromUser, toUser))
                {
                    StatusPageMessage = reason;
                    return;
                }
            }

            #endregion
        }

        private void preparePage()
        {
            string toUsername = Request.Params["to_user"];

            bool shouldPayWithCredits;
            checkMessagesLimitReached(out shouldPayWithCredits);

            #region Set photo and nicknames

            User toUser = null;

            try
            {
                toUser = Classes.User.Load(toUsername);
            }
            catch (NotFoundException)
            {
                ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists");
            }

            Photo primaryPhoto = null;
            try
            {
                primaryPhoto = toUser.GetPrimaryPhoto();
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
                ltrPhoto.Text = ImageHandler.RenderImageTag(toUser.Gender, 90, 90, "ImgBorder", false, true);
            }
            else
            {
                ltrPhoto.Text = ImageHandler.RenderImageTag(primaryPhoto.Id, 90, 90, "ImgBorder", false, true);
            }

            lblFromUsername.Text = CurrentUserSession.Username;
            lblToUsername.Text = toUser.Username;

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

        protected void btnSend_Click(object sender, EventArgs e)
        {
            #region Check can current user send message

            Classes.User toUser = null;
            User fromUser = null;
            try
            {
                toUser = Classes.User.Load((string)ViewState["SendMessage_ToUsername"]);
                fromUser = Classes.User.Load(CurrentUserSession.Username);
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
                    return;
                }
            }

            #endregion

            try
            {
                var toUsername = (string)ViewState["SendMessage_ToUsername"];

                if (Classes.User.IsUserBlocked(toUsername, CurrentUserSession.Username))
                {
                    StatusPageMessage =
                        String.Format(Lang.Trans("You are currently blocked from sending messages to {0}"), toUsername);
                    return;
                }

                var msg = new Message(CurrentUserSession.Username,
                                          toUsername)
                {
                    Body =
                        (Config.Misc.EnableBadWordsFilterMessage
                             ? Parsers.ProcessBadWords(txtMessageBody.Text.Trim())
                             : txtMessageBody.Text.Trim())
                };

                if (Config.Misc.EnableMessageFilter)
                {
                    msg.Body = ReplaceEmail(msg.Body, "***");
                    msg.Body = ReplacePhone(msg.Body, "***");
                }

                if (txtMessageBody.Text.Trim().Length == 0)
                {
                    return;
                }

                if (Request.Params["src"] != null && Request.Params["src"] == "message"
                    && Request.Params["src_id"] != null)
                {
                    try
                    {
                        msg.RepliedTo = Convert.ToInt32(Request.Params["src_id"]);
                    }
                    catch (InvalidCastException)
                    {
                    }
                }

                bool shouldPayWithCredits;
                checkMessagesLimitReached(out shouldPayWithCredits);

                if (shouldPayWithCredits)
                {
                    int creditsLeft = fromUser.Credits - CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits; //Config.Credits.CreditsPerMessage;

                    if (!Config.Credits.ChargeOneTimePerMember) // charge every time
                    {
                        if (creditsLeft < 0)
                        {
                            StatusPageMessage = "You have to be paid memeber in order to use this feature!".Translate();
                            Response.Redirect("Home.aspx");
                            return;
                        }

                        fromUser.Credits -= CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits;// Config.Credits.CreditsPerMessage;
                        fromUser.Update(true);
                        CurrentUserSession.Credits = fromUser.Credits;
                    }
                    else
                    {
                        bool isCharged = EstablishedCommunication.Fetch(fromUser.Username,
                            toUser.Username) != null;

                        if (!isCharged)
                        {
                            if (creditsLeft >= 0)
                            {
                                var establishedCommunication =
                                    new EstablishedCommunication(fromUser.Username, toUser.Username);

                                establishedCommunication.Save();

                                fromUser.Credits -= CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits;
                                fromUser.Update(true);
                                CurrentUserSession.Credits = fromUser.Credits;
                            }
                            else
                            {
                                StatusPageMessage = "You have to be paid memeber in order to use this feature!".Translate();
                                Response.Redirect("Home.aspx");
                                return;
                            }
                        }
                    }
                }

                if (Config.Misc.EnableSpamDetection)
                {
                    if (fromUser.SpamSuspected)
                    {
                        StatusPageMessage =
                            Lang.Trans(
                                "This account is flagged for review! You will not be able to send messages untill the review is completed!");
                        return;
                    }

                    string message = Regex.Replace(msg.Body, @"[^\w]*", "").ToLower();

                    if (message.Length > 100)
                    {
                        message = message.Substring(0, 100);
                    }

                    int sentMessages = MessagesSandBox.Save(CurrentUserSession.Username, message);

                    if (sentMessages > Config.Misc.MaxSameMessages)
                    {
                        fromUser.SpamSuspected = true;
                        fromUser.Update();

                        StatusPageMessage =
                            Lang.Trans(
                                "This account is flagged for review! You will not be able to send messages untill the review is completed!");
                        return;
                    }
                }

                msg.Send();

                Classes.User.AddScore(toUsername, Config.UserScores.ReceivedMessage, "ReceivedMessage");
                if (msg.RepliedTo > 0)
                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.RepliedToMessage,
                        "RepliedToMessage");
                else
                    Classes.User.AddScore(CurrentUserSession.Username, Config.UserScores.SentMessage,
                        "SentMessage");

                StatusPageMessage = Lang.Trans
                    ("<b>Your message has been sent successfully!</b>");
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

            Response.Redirect("Mailbox.aspx");
        }

        protected void lnkBack_Click(object sender, EventArgs e)
        {
            switch (lnkBack.CommandArgument)
            {
                case "profile":
                    Response.Redirect(UrlRewrite.CreateMobileShowUserUrl((string)ViewState["SendMessage_ToUsername"]));
                    break;
                case "favorites":
//                    Response.Redirect("~/Favorites.aspx");
                    break;
                case "friends":
//                    Response.Redirect("~/Friends.aspx");
                    break;
                default:
                case "message":
                    if (Request.Params["src"] != null && Request.Params["src"] == "message"
                        && Request.Params["src_id"] != null)
                    {
                        try
                        {
                            int mid = Convert.ToInt32(Request.Params["src_id"]);
                            Response.Redirect("ShowMessage.aspx?mid=" + mid);
                        }
                        catch (InvalidCastException)
                        {
                            Response.Redirect("Mailbox.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("Mailbox.aspx");
                    }
                    break;
            }
        }
    }
}
