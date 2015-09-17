/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using AspNetDating.Components.Profile;

namespace AspNetDating.Admin
{
    public partial class ApproveVideoUploads : AdminPageBase
    {
        protected MessageBox MessageBox;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!Config.Misc.EnableVideoUpload)
                {
                    StatusPageMessage = Lang.TransA("Video upload profile option is not currently switched on!\n You can do this from Settings at Site Management section.");
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
            dgPendingVideos.PageSize = Convert.ToInt32(dropVideosPerPage.SelectedValue);
            List<VideoUpload> videoUploads = VideoUpload.Load(null, null, true, false, null, null);

            if (videoUploads.Count == 0)
            {
                MessageBox.Show(Lang.TransA("There are no video uploads waiting for approval!"), 
                    Misc.MessageType.Error);
                dgPendingVideos.Visible = false;
                pnlVideosPerPage.Visible = false;
            }
            else
            {
                bindUsernames(videoUploads);

                dgPendingVideos.Visible = true;
                pnlVideosPerPage.Visible = true;
            }
        }

        private void bindUsernames(IEnumerable<VideoUpload> videoUploads)
        {
            dgPendingVideos.Columns[0].HeaderText = Lang.TransA("Username");
            dgPendingVideos.Columns[1].HeaderText = Lang.TransA("Video");

            DataTable dtUsernames = new DataTable("Usernames");
            dtUsernames.Columns.Add("Id");
            dtUsernames.Columns.Add("Username");
            dtUsernames.Columns.Add("VideoUrl");

            foreach (VideoUpload videoUpload in videoUploads)
            {
                string videoUrl = String.Format("{0}/UserFiles/{1}/video_{2}.flv", Config.Urls.Home,
                                                videoUpload.Username, videoUpload.Id);
                dtUsernames.Rows.Add(new object[] {videoUpload.Id, videoUpload.Username, videoUrl});
            }

            dtUsernames.DefaultView.Sort = "Username";

            dgPendingVideos.DataSource = dtUsernames;
            try
            {
                dgPendingVideos.DataBind();
            }
            catch (HttpException)
            {
                dgPendingVideos.CurrentPageIndex = 0;
                dgPendingVideos.DataBind();
            }
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                dropVideosPerPage.Items.Add(i.ToString());
            dropVideosPerPage.SelectedValue = 5.ToString();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.videoApproval;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgPendingVideos.ItemCommand += new DataGridCommandEventHandler(dgPendingVideos_ItemCommand);
            this.dgPendingVideos.PageIndexChanged +=
                new DataGridPageChangedEventHandler(dgPendingVideos_PageIndexChanged);
            this.dropVideosPerPage.SelectedIndexChanged += new EventHandler(dropVideosPerPage_SelectedIndexChanged);
        }

        #endregion

        private void dgPendingVideos_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            if (!HasWriteAccess)
                return;

            VideoUpload videoUpload = VideoUpload.Load(Convert.ToInt32(e.CommandArgument));

            if (e.CommandName == "approve")
            {
                if (videoUpload != null)
                {
                    videoUpload.Approved = true;
                    videoUpload.Save();

                    if (!videoUpload.IsPrivate)
                    {
                        #region Add NewFriendVideoUpload Event

                        Event newEvent = new Event(videoUpload.Username);

                        newEvent.Type = Event.eType.NewFriendVideoUpload;
                        NewFriendVideoUpload newFriendVideoUpload = new NewFriendVideoUpload();
                        newFriendVideoUpload.VideoUploadID = videoUpload.Id;
                        newEvent.DetailsXML = Misc.ToXml(newFriendVideoUpload);

                        newEvent.Save();

                        string[] usernames = Classes.User.FetchMutuallyFriends(videoUpload.Username);

                        foreach (string friendUsername in usernames)
                        {
                            if (Config.Users.NewEventNotification)
                            {
                                string text = String.Format("Your friend {0} has uploaded a new video".TranslateA(),
                                                      "<b>" + videoUpload.Username + "</b>");
                                int imageID = 0;
                                try
                                {
                                    imageID = Photo.GetPrimary(videoUpload.Username).Id;
                                }
                                catch (NotFoundException)
                                {
                                    Classes.User user = null;
                                    try
                                    {
                                        user = Classes.User.Load(videoUpload.Username);
                                        imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                                    }
                                    catch (NotFoundException)
                                    {
                                        continue;
                                    }
                                }
                                string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                                Classes.User.SendOnlineEventNotification(videoUpload.Username, friendUsername,
                                                                         text, thumbnailUrl,
                                                                         UrlRewrite.CreateShowUserUrl(videoUpload.Username));
                            }
                        }

                        #endregion
                    }

                    Classes.User.AddScore(videoUpload.Username, Config.UserScores.ApprovedVideo, 
                        "ApprovedVideo");

                    #region e-mail notification

                    try
                    {
                        Classes.User user = Classes.User.Load(videoUpload.Username);

                        MiscTemplates.ApproveVideoMessage approveVideoMessageTemplate =
                            new MiscTemplates.ApproveVideoMessage(user.LanguageId);
                        Message.Send(Config.Users.SystemUsername, user.Username, approveVideoMessageTemplate.Message, 0);
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
                if (videoUpload != null)
                {
                    VideoUpload.Delete(Convert.ToInt32(e.CommandArgument));

                    Classes.User.AddScore(videoUpload.Username, Config.UserScores.RejectedVideo,
                        "RejectedVideo");

                    #region e-mail notification

                    try
                    {
                        Classes.User user = Classes.User.Load(videoUpload.Username);

                        MiscTemplates.RejectVideoMessage rejectVideoMessageTemplate =
                            new MiscTemplates.RejectVideoMessage(user.LanguageId);
                        Message.Send(Config.Users.SystemUsername, user.Username, rejectVideoMessageTemplate.Message, 0);
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

        private void dgPendingVideos_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            dgPendingVideos.CurrentPageIndex = e.NewPageIndex;
            PopulateDataGrid();
        }

        private void dropVideosPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            dgPendingVideos.PageSize = Convert.ToInt32(dropVideosPerPage.SelectedValue);
            dgPendingVideos.CurrentPageIndex = 0;
            PopulateDataGrid();
        }

        protected void dgPendingVideos_ItemCreated(object sender, DataGridItemEventArgs e)
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
    }
}