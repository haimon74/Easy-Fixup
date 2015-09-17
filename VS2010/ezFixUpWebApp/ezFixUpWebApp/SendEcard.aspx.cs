using System;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class SendEcard : PageBase
    {
        private TextBox ckeditor = null;
        private HtmlEditor htmlEditor = null;
        private PermissionCheckResult canSendEcardPermissionResult;

        protected string RecipientUsername
        {
            get
            {
                return (string)ViewState["RecipientUsername"];
            }
            set
            {
                ViewState["RecipientUsername"] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            Scripts.InitilizeHtmlEditor(this, phEditor, ref htmlEditor, ref ckeditor, "500px", "200px");

            canSendEcardPermissionResult = CurrentUserSession.CanSendEcards();

            if (canSendEcardPermissionResult == PermissionCheckResult.YesWithCredits)
                btnSend.OnClientClick = 
                    String.Format("return confirm(\"" + "Sending this e-card will subtract {0} credits from your balance.".Translate() +"\");",
                    CurrentUserSession.BillingPlanOptions.CanSendEcards.Credits);

            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        private void loadStrings()
        {
            //var result = CurrentUserSession.CanSendEcards();

            if (String.IsNullOrEmpty(Request.Params["uid"])
                || Request.Params["uid"] == Config.Users.SystemUsername
                || canSendEcardPermissionResult == PermissionCheckResult.No)
            {
                Response.Redirect("~/Default.aspx");
                return;
            }

            if (canSendEcardPermissionResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                canSendEcardPermissionResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
            {
                Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanSendEcards;
                Response.Redirect("ManageProfile.aspx?sel=payment");
            }

            RecipientUsername = Request.Params["uid"];

            WideBoxStart1.Title = "Send e-card".Translate();

            ddEcards.Items.Add(new ListItem("", "-1"));

            EcardType[] ecardTypes = EcardType.FetchEcards(true, null);

            foreach (EcardType ecardType in ecardTypes)
            {
                ddEcards.Items.Add(new ListItem(ecardType.Name, ecardType.ID.ToString()));
            }

            btnSend.Text = "Send".Translate();

            pnlImage.Visible = false;
            pnlFlash.Visible = false;
            pnlMessage.Visible = false;
            btnSend.Visible = false;
        }

        protected void ddEcards_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddEcards.SelectedValue == "-1")
            {
                pnlImage.Visible = false;
                pnlFlash.Visible = false;
                pnlMessage.Visible = false;
                btnSend.Visible = false;
                return;
            }

            EcardType ecardType = EcardType.Fetch(Convert.ToInt32(ddEcards.SelectedValue));

            if (ecardType != null)
            {
                pnlImage.Visible = ecardType.Type == EcardType.eType.Image;
                pnlFlash.Visible = !pnlImage.Visible;
                btnSend.Visible = true;
                pnlMessage.Visible = !Config.Misc.SiteIsPaid || Classes.User.IsPaidMember(CurrentUserSession.Username);
            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            EcardType ecardType = EcardType.Fetch(Convert.ToInt32(ddEcards.SelectedValue));

            if (ecardType != null)
            {
                //var permissionResult = CurrentUserSession.CanSendEcards();

                if (canSendEcardPermissionResult == PermissionCheckResult.No)
                {
                    StatusPageMessage = "You are not allowed to send e-cards".Translate();
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                if (canSendEcardPermissionResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                    canSendEcardPermissionResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanSendEcards;
                    Response.Redirect("ManageProfile.aspx?sel=payment");
                    return;
                }

                if (canSendEcardPermissionResult == PermissionCheckResult.YesWithCredits)
                {
                //if (Config.Credits.Required && CurrentUserSession.BillingPlanOptions.CanSendEcards.Credits > 0)
                //{
                //    if (!Config.Users.FreeForFemales ||
                //        CurrentUserSession.Gender != Classes.User.eGender.Female)
                //    {
                    int creditsCost = CurrentUserSession.BillingPlanOptions.CanSendEcards.Credits;
                    int creditsLeft = CurrentUserSession.Credits - creditsCost;

                    if (!Config.Credits.ChargeOneTimePerMember) // charge every time
                    {
                        if (creditsLeft < 0)
                        {
                            Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanSendEcards;
                            Response.Redirect("~/ManageProfile.aspx?sel=payment");
                            return;
                        }

                        var user = Classes.User.Load(CurrentUserSession.Username);
                        user.Credits -= creditsCost;
                        user.Update(true);
                        CurrentUserSession.Credits = user.Credits;
                    }
                    else
                    {
                        bool isCharged = EstablishedCommunication.Fetch(CurrentUserSession.Username,
                            RecipientUsername) != null;

                        if (!isCharged)
                        {
                            if (creditsLeft >= 0)
                            {
                                var establishedCommunication =
                                    new EstablishedCommunication(CurrentUserSession.Username, RecipientUsername);

                                establishedCommunication.Save();

                                var user = Classes.User.Load(CurrentUserSession.Username);
                                user.Credits -= creditsCost;
                                user.Update(true);
                                CurrentUserSession.Credits = user.Credits;
                            }
                            else
                            {
                                Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanSendEcards;
                                Response.Redirect("~/ManageProfile.aspx?sel=payment");
                                return;
                            }
                        }
                    }
                //    }
                }

                Ecard ecard = new Ecard(ecardType.ID, CurrentUserSession.Username, RecipientUsername);
                string message = htmlEditor != null ? htmlEditor.Content : ckeditor.Text;
                ecard.Message = message.Trim();
                ecard.Save();

                User recipient = null;
                try
                {
                    recipient = Classes.User.Load(RecipientUsername);
                }
                catch (NotFoundException)
                {
                    StatusPageMessage = "The user no longer exists!".Translate();
                    Response.Redirect("~/ShowStatus.aspx");
                    return;
                }

                if (Classes.User.IsUserBlocked(RecipientUsername, CurrentUserSession.Username))
                {
                    StatusPageMessage =
                        String.Format(Lang.Trans("You are currently blocked from sending e-cards to {0}"),
                                      RecipientUsername);
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                if (recipient.ReceiveEmails)
                {
                    EmailTemplates.SendEcard sendEcardTemplate = new EmailTemplates.SendEcard(recipient.LanguageId);

                    Email.Send(Config.Misc.SiteTitle, Config.Misc.SiteEmail, recipient.Name, recipient.Email,
                               sendEcardTemplate.GetFormattedSubject(CurrentUserSession.Username),
                               sendEcardTemplate.GetFormattedBody(CurrentUserSession.Username), false);
                }

                if (Config.Users.NewEventNotification)
                {
                    int imageID = 0;
                    try
                    {
                        imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                    }
                    catch (NotFoundException)
                    {
                        imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                    }
                    string text = "You have a new e-card!".Translate();

                    string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                    Classes.User.SendOnlineEventNotification(CurrentUserSession.Username, recipient.Username, text,
                                                             thumbnailUrl, "Mailbox.aspx?sel=recec");
                }

                StatusPageLinkText = "Back to profile".Translate();
                StatusPageLinkURL = UrlRewrite.CreateShowUserUrl(RecipientUsername);
                StatusPageMessage = Lang.Trans("<b>Your e-card was sent successfully!</b><br><br>");
                Response.Redirect("~/ShowStatus.aspx");    
            }
        }
    }
}
