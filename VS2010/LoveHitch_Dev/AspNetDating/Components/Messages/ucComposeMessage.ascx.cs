using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Messages
{
    public partial class ucComposeMessage : UserControl
    {
        protected string MessageBodyClientId{get { return txtMessageBody.ClientID; }}
        public UserSession CurrentUserSession { get; set; }
        public string From { get { return lblFromUsername.Text; } set { lblFromUsername.Text = value; }}
        public string To { get { return lblToUsername.Text; } set { lblToUsername.Text = value; } }

        public int CurrentSmiliesPage
        {
            set{ViewState["CurrentSmiliesPage"] = value;}
            get{return (int)(ViewState["CurrentSmiliesPage"] ?? 0);}
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadStrings();
                loadSmileys();
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

        private void loadStrings()
        {
            LargeBoxStart1.Title = Lang.Trans("Message");
            btnSend.Text = Lang.Trans("Send Message");
            //lnkUpgradeNow.Text = Lang.Trans("Upgrade Now to Respond to this email");
        }

        private void preparePage()
        {
            string toUsername = Request.Params["to_user"];

            bool shouldPayWithCredits;
            checkMessagesLimitReached(out shouldPayWithCredits);

            if (shouldPayWithCredits)
            {
                btnSend.OnClientClick =
                    String.Format("return confirm(\"" + "Sending this message will subtract {0} credits from your balance.".Translate() + "\");",
                        CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.Credits);
            }

            #region Set photo and nicknames

            User toUser = null;
            try{toUser = Classes.User.Load(toUsername);}
            catch (NotFoundException)
            {
                ((PageBase)Page).StatusPageMessage = Lang.Trans("The user no longer exists");
                Response.Redirect("~/ShowStatus.aspx");
            }

            Photo primaryPhoto = null;
            try{primaryPhoto = toUser.GetPrimaryPhoto();}
            catch (NotFoundException){}
            catch (Exception err){/*Log(err);*/}

            if (primaryPhoto == null || !primaryPhoto.Approved)
            {
                ltrPhoto.Text = ImageHandler.RenderImageTag(toUser.Gender, 50, 50, "photoframe", false, true);
            }
            else
            {
                ltrPhoto.Text = ImageHandler.RenderImageTag(primaryPhoto.Id, 50, 50, "photoframe", false, true);
            }

            lblFromUsername.Text = CurrentUserSession.Username;
            lblToUsername.Text = toUser.Username;

            #endregion

            #region Load previous messages

            int repliedTo = -1;
            try{repliedTo = Convert.ToInt32(Request.Params["src_id"]);}
            catch (InvalidCastException){}

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
                    try{message = Message.Fetch(messageId);}
                    catch (NotFoundException){break;}

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
            }
            #endregion
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
            Message[] messages = Message.FetchSentMessagesForLast24Hours(CurrentUserSession.Username);

            if (messages.Length >= maxMessages)
            {
                if (!CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.EnableCreditsPayment &&
                    !CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay.UpgradableToNextPlan)
                {
                    Session["StatusPageMessage"] = Lang.Trans("You've exceeded the number of messages you can send per day!");
                    Response.Redirect("ShowStatus.aspx");
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
                        Session["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.MaxMessagesPerDay;
                        Response.Redirect("Profile.aspx?sel=payment");
                        return;
                    }
                }
            }
            #region checkContactedUsersLimitReached

            List<string> uniqueUsers = new List<string>();

            foreach (Message message in messages)
                AddUniqueItems(uniqueUsers, message.ToUser.Username);

            if (uniqueUsers.Count >= Config.Users.MaxContactedUsersPerDay)
            {
                Session["StatusPageMessage"] = Lang.Trans("You've exceeded the number of users you can contact per day!");
                Response.Redirect("ShowStatus.aspx");
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
    }
}