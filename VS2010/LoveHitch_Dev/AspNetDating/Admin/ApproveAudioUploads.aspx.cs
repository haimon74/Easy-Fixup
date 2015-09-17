using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ApproveAudioUploads : AdminPageBase
    {
        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.audioApproval;

            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Misc.EnableAudioUpload)
                {
                    StatusPageMessage = Lang.TransA("Audio upload profile option is not currently switched on!\n You can do this from Settings at Site Management section.");
                    StatusPageMessageType = Misc.MessageType.Error;
                    Response.Redirect("~/Admin/ShowStatus.aspx");
                    return;
                }

                PopulateDropDown();
                PopulateDataGrid();
            }
        }

        private void PopulateDataGrid()
        {
            dgPendingAudio.PageSize = Convert.ToInt32(ddAudioPerPage.SelectedValue);
            AudioUpload[] audioUploads = AudioUpload.Load(null, null, false, null);

            if (audioUploads.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no audio uploads waiting for approval!"),
                    Misc.MessageType.Error);
                dgPendingAudio.Visible = false;
                pnlAudioPerPage.Visible = false;
            }
            else
            {
                bindUsernames(audioUploads);

                dgPendingAudio.Visible = true;
                pnlAudioPerPage.Visible = true;
            }
        }

        private void bindUsernames(IEnumerable<AudioUpload> audioUploads)
        {
            dgPendingAudio.Columns[0].HeaderText = Lang.TransA("Username");
            dgPendingAudio.Columns[1].HeaderText = Lang.TransA("Audio file");

            DataTable dtUsernames = new DataTable("Usernames");
            dtUsernames.Columns.Add("Id");
            dtUsernames.Columns.Add("Username");
            dtUsernames.Columns.Add("AudioUrl");

            foreach (AudioUpload audioUpload in audioUploads)
            {
                string audioUrl = String.Format("{0}/UserFiles/{1}/audio_{2}.mp3", Config.Urls.Home,
                                                audioUpload.Username, audioUpload.Id);
                dtUsernames.Rows.Add(new object[] { audioUpload.Id, audioUpload.Username, audioUrl });
            }

            dtUsernames.DefaultView.Sort = "Username";

            dgPendingAudio.DataSource = dtUsernames;
            try
            {
                dgPendingAudio.DataBind();
            }
            catch (HttpException)
            {
                dgPendingAudio.CurrentPageIndex = 0;
                dgPendingAudio.DataBind();
            }
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                ddAudioPerPage.Items.Add(i.ToString());
            ddAudioPerPage.SelectedValue = 5.ToString();
        }

        protected void dgPendingAudio_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            AudioUpload audioUpload = AudioUpload.Load(Convert.ToInt32(e.CommandArgument));

            if (e.CommandName == "approve")
            {
                if (audioUpload != null)
                {
                    audioUpload.Approved = true;
                    audioUpload.Save();

                    if (!audioUpload.IsPrivate)
                    {
                        #region Add NewFriendAudioUpload Event

                        Event newEvent = new Event(audioUpload.Username);

                        newEvent.Type = Event.eType.NewFriendAudioUpload;
                        NewFriendAudioUpload newFriendAudioUpload = new NewFriendAudioUpload();
                        newFriendAudioUpload.AudioUploadID = audioUpload.Id;
                        newEvent.DetailsXML = Misc.ToXml(newFriendAudioUpload);

                        newEvent.Save();

                        string[] usernames = Classes.User.FetchMutuallyFriends(audioUpload.Username);

                        foreach (string friendUsername in usernames)
                        {
                            if (Config.Users.NewEventNotification)
                            {
                                string text = String.Format("Your friend {0} has uploaded a new audio".TranslateA(),
                                                      "<b>" + audioUpload.Username + "</b>");
                                int imageID = 0;
                                try
                                {
                                    imageID = Photo.GetPrimary(audioUpload.Username).Id;
                                }
                                catch (NotFoundException)
                                {
                                    User user = null;
                                    try
                                    {
                                        user = Classes.User.Load(audioUpload.Username);
                                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                                    }
                                    catch (NotFoundException) { break; }
                                }
                                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                Classes.User.SendOnlineEventNotification(audioUpload.Username, friendUsername,
                                                                         text, thumbnailUrl,
                                                                         UrlRewrite.CreateShowUserUrl(audioUpload.Username));
                            }
                        }

                        #endregion
                    }

                    Classes.User.AddScore(audioUpload.Username, Config.UserScores.ApprovedAudio,
                        "ApprovedAudio");

                    #region e-mail notification

                    try
                    {
                        Classes.User user = Classes.User.Load(audioUpload.Username);

                        MiscTemplates.ApproveAudioMessage approveAudioMessageTemplate =
                            new MiscTemplates.ApproveAudioMessage(user.LanguageId);
                        Message.Send(Config.Users.SystemUsername, user.Username, approveAudioMessageTemplate.Message, 0);
                    }
                    catch (NotFoundException ex)
                    {
                        Log(ex);
                    }

                    #endregion

                    PopulateDataGrid();
                }
            }
            else if (e.CommandName == "reject")
            {
                if (audioUpload != null)
                {
                    AudioUpload.Delete(Convert.ToInt32(e.CommandArgument));

                    Classes.User.AddScore(audioUpload.Username, Config.UserScores.RejectedAudio,
                        "RejectedAudio");

                    #region e-mail notification

                    try
                    {
                        Classes.User user = Classes.User.Load(audioUpload.Username);

                        MiscTemplates.RejectAudioMessage rejectAudioMessageTemplate =
                            new MiscTemplates.RejectAudioMessage(user.LanguageId);
                        Message.Send(Config.Users.SystemUsername, user.Username, rejectAudioMessageTemplate.Message, 0);
                    }
                    catch (NotFoundException ex)
                    {
                        Log(ex);
                    }

                    #endregion

                    PopulateDataGrid();
                }
            }
        }

        protected void dgPendingAudio_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            if (!HasWriteAccess)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    Button btnApprove = e.Item.FindControl("btnApprove") as Button;
                    Button btnReject = e.Item.FindControl("btnReject") as Button;
                    if (btnApprove != null)
                        btnApprove.Enabled = false;
                    if (btnReject != null)
                        btnReject.Enabled = false;
                }
            }
        }

        protected void ddAudioPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPendingAudio.PageSize = Convert.ToInt32(ddAudioPerPage.SelectedValue);
            dgPendingAudio.CurrentPageIndex = 0;
            PopulateDataGrid();
        }

        protected void dgPendingAudio_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPendingAudio.CurrentPageIndex = e.NewPageIndex;
            PopulateDataGrid();
        }
    }
}
