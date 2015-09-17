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
using System.IO;
using System.Threading;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using AspNetDating.Classes;
using Google.GData.Client;
using Google.GData.YouTube;
using Google.YouTube;

namespace AspNetDating.Components.Profile
{
    public partial class UploadVideo : UserControl
    {
        public void InitControl()
        {
            mvVideo.Visible = false;
        }

        public bool FirstLoad
        {
            get
            {
                if (ViewState["FirstLoad"] == null)
                    return true;
                return (bool)ViewState["FirstLoad"];
            }
            set { ViewState["FirstLoad"] = value; }
        }

        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            cvNote.Text = "Upload Videos Notes".Translate();
            cvNote2.Text = "Embed Videos Notes".Translate();
            LargeBoxStart1.Title = Lang.Trans("Upload Video");
            btnShowEmbedVideo.Text = Lang.Trans("Embed Video");
            btnShowEmbedVideo.Visible = Config.Misc.EnableYouTubeVideos;
            btnShowUploadVideo.Text = Lang.Trans("Upload Video");
            btnShowUploadVideo.Visible = Config.Misc.EnableVideoUpload;
            btnShowRecordVideo.Text = Lang.Trans("Record Video");
            btnShowRecordVideo.Visible = Config.Misc.EnableVideoProfile;
            btnAddYouTubeVideo.Text = Lang.Trans("Add YouTube Video");
            HeaderLine1.Title = Lang.Trans("Upload Video File");
            HeaderLine2.Title = Lang.Trans("Embed Video from YouTube");
            btnUpload.Text = Lang.Trans("Upload");
            btnSearchVideoKeywords.Text = Lang.Trans("Search");
            btnEmbedVideo.Text = Lang.Trans("Embed this video");
            btnBack.Text = Lang.Trans("Back");
            lnkUploadMultipleFlash.Text = "Upload multiple videos with Flash".Translate();
            lnkUploadMultipleSilverlight.Text = "Upload multiple videos with Silverlight".Translate();
            lnkBackToVideoUploads.Text = "Back to video uploads".Translate();
            divUploadMultipleVideosFlash.Visible = Config.Users.EnableFlashUploads;
            divUploadMultipleVideosSilverlight.Visible = Config.Users.EnableSilverlightUploads;

            string cacheKey = "flashVideoUploadError_" + CurrentUserSession.Username;
            if (Cache.Get(cacheKey) != null)
            {
                var error = (string)Cache.Get(cacheKey);
                Page.ClientScript.RegisterStartupScript(GetType(), "error",
                                                    String.Format("var alert_string = '{0}';",
                                                    error), true);
                Cache.Remove(cacheKey);
            }

            cacheKey = "silverlightVideoUploadError_" + CurrentUserSession.Username;
            if (Cache.Get(cacheKey) != null)
            {
                var error = (string)Cache.Get(cacheKey);
                Page.ClientScript.RegisterStartupScript(GetType(), "error",
                                                    String.Format("var alert_string = '{0}';",
                                                    error), true);
                Cache.Remove(cacheKey);
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (FirstLoad)
            {
                int sum = Convert.ToInt32(Config.Misc.EnableVideoProfile) +
                          Convert.ToInt32(Config.Misc.EnableVideoUpload) +
                          Convert.ToInt32(Config.Misc.EnableYouTubeVideos);

                if (sum <= 1)
                    pnlButtons.Visible = false;

                if (sum == 1)
                {
                    if (Config.Misc.EnableVideoProfile)
                        btnShowRecordVideo_Click(null, null);
                    if (Config.Misc.EnableVideoUpload)
                        btnShowUploadVideo_Click(null, null);
                    if (Config.Misc.EnableYouTubeVideos)
                        btnShowEmbedVideo_Click(null, null);
                }

                FirstLoad = false;
            }
        }

        protected void btnDeleteVideoUpload_Click(object sender, EventArgs e)
        {
            foreach (VideoUpload videoUpload in VideoUpload.Load(null, ((PageBase)Page).CurrentUserSession.Username,
                                                          null, null, null, null))
            {
                videoUpload.Delete();
            }

            ((PageBase)Page).StatusPageMessage = Lang.Trans("Your video has been deleted!");
            Response.Redirect("ShowStatus.aspx");
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (mvVideo.GetActiveView() == vUploadVideo)
            {
                List<VideoUpload> videos = VideoUpload.Load(null, CurrentUserSession.Username, null, null, null, null);

                if (videos.Count >= CurrentUserSession.BillingPlanOptions.MaxVideoUploads.Value
                    && (CurrentUserSession.Level != null && videos.Count >= CurrentUserSession.Level.Restrictions.MaxVideoUploads))
                {
                    ((PageBase)Page).StatusPageMessage = Lang.Trans("You cannot upload more videos!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }
            }

            if (!fileVideo.HasFile)
            {
                lblError.Text = Lang.Trans("Please select video file!");
                return;
            }

            string tempfile;

            if (!Misc.GetTempFileName(out tempfile))
                tempfile = Path.GetTempFileName();

            fileVideo.SaveAs(tempfile);

            var videoUpload = new VideoUpload(((PageBase)Page).CurrentUserSession.Username);
            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.BillingPlanOptions.AutoApproveVideos.Value
                    || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.AutoApproveVideos)
                {
                    videoUpload.Approved = true;
                }
            }
            videoUpload.IsPrivate = cbPrivateVideo.Checked;
            videoUpload.Save(); // Save to get new ID

            if (videoUpload.Approved && !videoUpload.IsPrivate)
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
                        if (CurrentUserSession != null)
                        {
                            string text = String.Format("Your friend {0} has uploaded a new video".Translate(),
                                                          "<b>" + CurrentUserSession.Username + "</b>");
                            int imageID = 0;
                            try
                            {
                                imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                            }
                            catch (NotFoundException)
                            {
                                imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                            }
                            string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                            Classes.User.SendOnlineEventNotification(videoUpload.Username, friendUsername,
                                                                     text, thumbnailUrl,
                                                                     UrlRewrite.CreateShowUserUrl(videoUpload.Username));
                        }
                    }
                }

                #endregion
            }

            string userFilesPath = "~/UserFiles/" + ((PageBase)Page).CurrentUserSession.Username;
            string userFilesDir = Server.MapPath(userFilesPath);
            if (!Directory.Exists(userFilesDir))
            {
                Directory.CreateDirectory(userFilesDir);
            }

            File.Move(tempfile, userFilesDir + @"\video_" + videoUpload.Id + ".original");

            ThreadPool.QueueUserWorkItem(AsyncProcessVideo, videoUpload);

            ((PageBase)Page).StatusPageMessage = Lang.Trans("Your video has been queued for processing!");
            Response.Redirect("ShowStatus.aspx");
        }

        static void AsyncProcessVideo(Object stateInfo)
        {
            VideoUpload.ProcessVideoUpload((VideoUpload)stateInfo);
        }

        protected void btnSearchVideoKeywords_Click(object sender, EventArgs e)
        {
            if (txtVideoKeywords.Text.Trim().Length == 0)
                return;

            var dtVideos = new DataTable();
            dtVideos.Columns.Add("Title", typeof(string));
            dtVideos.Columns.Add("ThumbnailUrl", typeof(string));
            dtVideos.Columns.Add("VideoUrl", typeof(string));
            dtVideos.Columns.Add("ID", typeof(int));


            var settings = new YouTubeRequestSettings("eStream-AspNetDating", null,
                "AI39si7DZKHR_khn2HMQ-fABrE1J7OI4fT-LTH5fFexEn3Cfl-D2ZIpErZhZiKTAGoSHZA2e3Aeh9RAqrZ9lq7RxtAYbUFyf3g");
            var request = new YouTubeRequest(settings);
            var query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);

            //order results by the number of views (most viewed first)
            query.OrderBy = "viewCount";

            query.Query = txtVideoKeywords.Text.Trim();
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;


            Feed<Video> videoFeed;
            try
            {
                videoFeed = request.Get<Video>(query);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("YouTube search for " + txtVideoKeywords.Text, err);
                return;
            }
            foreach (var video in videoFeed.Entries)
            {
                if (video.Contents == null || video.Contents.Count == 0
                    || video.Thumbnails == null || video.Thumbnails.Count == 0)
                    continue;

                var thumbnailUrl = video.Thumbnails[0].Url;
                var videoUrl = video.Contents[0].Url;
                dtVideos.Rows.Add(new object[] { video.Title, thumbnailUrl, videoUrl, 0 });
            }

            /* // Old search implementation
            //string uri = "https://gdata.youtube.com/feeds/videos/-/" +
            //    txtVideoKeywords.Text.Trim().Replace(' ', '/');
            string uri = "https://gdata.youtube.com/feeds/api/videos?q=" +
                Server.UrlEncode(txtVideoKeywords.Text.Trim());

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(uri);
            XmlNamespaceManager nsMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            nsMgr.AddNamespace("def", "https://www.w3.org/2005/Atom");
            nsMgr.AddNamespace("media", "https://search.yahoo.com/mrss/");
            XmlNodeList nodes = xmlDoc.SelectNodes("/def:feed/def:entry", nsMgr);
            foreach (XmlNode node in nodes)
            {
                if (node.SelectSingleNode("media:group/media:content", nsMgr) == null)
                    continue;

                string title = node.SelectSingleNode("def:title", nsMgr).InnerText;
                string thumbUrl =
                    node.SelectSingleNode("media:group/media:thumbnail", nsMgr).Attributes["url"].InnerText;
                string videoUrl =
                    node.SelectSingleNode("media:group/media:content", nsMgr).Attributes["url"].InnerText;
                dtVideos.Rows.Add(new object[] { title, thumbUrl, videoUrl, 0 });
            }
            */

            dlVideos.DataSource = dtVideos;
            dlVideos.DataBind();

            divVideoPreview.Visible = false;
        }

        protected void dlVideos_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName == "SelectVideo")
            {
                string thumbUrl = e.CommandArgument.ToString().Split('|')[0];
                string videoUrl = e.CommandArgument.ToString().Split('|')[1];
                string title = e.CommandArgument.ToString().Split('|')[2];

                divVideoPreview.Visible = true;
                if (divVideoKeywords.Visible)
                    btnEmbedVideo.Visible = true;
                else
                    btnEmbedVideo.Visible = false;

                ltrVideoPreview.Text =
                    String.Format(
                        "<object width=\"425\" height=\"350\"><param name=\"movie\" value=\"{0}\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"{0}\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"350\"></embed></object>",
                        videoUrl);

                ViewState["UploadVideo_ThumbUrl"] = thumbUrl;
                ViewState["UploadVideo_VideoUrl"] = videoUrl;
                ViewState["UploadVideo_Title"] = title;
            }
            else if (e.CommandName == "RemoveVideo")
            {
                if (e.CommandArgument == null) return;
                int id = Convert.ToInt32(e.CommandArgument);

                if (id > 0)
                {
                    VideoEmbed.Delete(id);

                    Event[] events = Event.Fetch(CurrentUserSession.Username, (ulong)Event.eType.NewFriendYouTubeUpload, 1000);

                    foreach (Event ev in events)
                    {
                        var newFriendYouTubeUpload = Misc.FromXml<NewFriendYouTubeUpload>(ev.DetailsXML);
                        if (newFriendYouTubeUpload.YouTubeUploadID == id)
                        {
                            Event.Delete(ev.ID);
                            break;
                        }
                    }
                }

                ShowEmbeddedVideos();
            }
        }

        protected void btnEmbedVideo_Click(object sender, EventArgs e)
        {
            string thumbUrl = (string)ViewState["UploadVideo_ThumbUrl"];
            string videoUrl = (string)ViewState["UploadVideo_VideoUrl"];
            string title = (string)ViewState["UploadVideo_Title"];

            VideoEmbed embed = new VideoEmbed(((PageBase)Page).CurrentUserSession.Username, videoUrl);
            embed.ThumbUrl = thumbUrl;
            embed.Title = title;
            embed.Save();

            #region Add NewFriendYouTubeUpload Event

            Event newEvent = new Event(embed.Username);

            newEvent.Type = Event.eType.NewFriendYouTubeUpload;
            NewFriendYouTubeUpload newFriendYouTubeUpload = new NewFriendYouTubeUpload();
            newFriendYouTubeUpload.YouTubeUploadID = embed.Id;
            newEvent.DetailsXML = Misc.ToXml(newFriendYouTubeUpload);

            newEvent.Save();

            string[] usernames = Classes.User.FetchMutuallyFriends(embed.Username);

            foreach (string friendUsername in usernames)
            {
                if (Config.Users.NewEventNotification)
                {
                    if (CurrentUserSession != null)
                    {
                        string text = String.Format("Your friend {0} has uploaded a new video".Translate(),
                                                      "<b>" + CurrentUserSession.Username + "</b>");
                        int imageID = 0;
                        try
                        {
                            imageID = Photo.GetPrimary(CurrentUserSession.Username).Id;
                        }
                        catch (NotFoundException)
                        {
                            imageID = ImageHandler.GetPhotoIdByGender(CurrentUserSession.Gender);
                        }
                        string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                        Classes.User.SendOnlineEventNotification(embed.Username, friendUsername,
                                                                 text, thumbnailUrl,
                                                                 UrlRewrite.CreateShowUserUrl(embed.Username));
                    }
                }
            }

            #endregion

            ShowEmbeddedVideos();
        }

        protected void btnShowUploadVideo_Click(object sender, EventArgs e)
        {
            showUploadedVideos();

            rptVideoUploads.Visible = true;
            pnlUploadMultipleVideosFlash.Visible = false;
            pnlUploadMultipleVideosSilverlight.Visible = false;
            lnkBackToVideoUploads.Visible = false;
            mvVideo.Visible = true;
            mvVideo.SetActiveView(vUploadVideo);
        }

        protected void btnShowEmbedVideo_Click(object sender, EventArgs e)
        {
            mvVideo.Visible = true;
            mvVideo.SetActiveView(vEmbedVideo);
            ShowEmbeddedVideos();
        }

        private void ShowEmbeddedVideos()
        {
            UserSession userSession = ((PageBase)Page).CurrentUserSession;
            List<VideoEmbed> videoEmbeds = VideoEmbed.Load(null, userSession.Username, null);
            if (videoEmbeds.Count > 0)
            {
                divVideoPreview.Visible = false;
                divVideoKeywords.Visible = false;
                btnEmbedVideo.Visible = false;

                DataTable dtVideos = new DataTable();
                dtVideos.Columns.Add("Title", typeof(string));
                dtVideos.Columns.Add("ThumbnailUrl", typeof(string));
                dtVideos.Columns.Add("VideoUrl", typeof(string));
                dtVideos.Columns.Add("ID", typeof(int));

                foreach (VideoEmbed video in videoEmbeds)
                {
                    dtVideos.Rows.Add(new object[] { video.Title, video.ThumbUrl, video.VideoUrl, video.Id });
                }

                dlVideos.DataSource = dtVideos;
                dlVideos.DataBind();

                divVideoKeywords.Visible = false;

                if (videoEmbeds.Count < userSession.BillingPlanOptions.MaxVideos.Value
                    || (userSession.Level != null && videoEmbeds.Count < userSession.Level.Restrictions.MaxVideos))
                    pnlAddYouTubeVideoButton.Visible = true;
                else
                    pnlAddYouTubeVideoButton.Visible = false;
            }
            else
            {
                divVideoPreview.Visible = false;
                ltrVideoPreview.Text = String.Empty;
                dlVideos.DataSource = null;
                dlVideos.DataBind();

                divVideoKeywords.Visible = true;
                pnlAddYouTubeVideoButton.Visible = false;
            }

            pnlBackButton.Visible = false;
        }

        private void showUploadedVideos()
        {
            List<VideoUpload> videos = VideoUpload.Load(null, CurrentUserSession.Username, null, null, null, null);

            if (videos.Count < CurrentUserSession.BillingPlanOptions.MaxVideoUploads.Value
                || (CurrentUserSession.Level != null && videos.Count < CurrentUserSession.Level.Restrictions.MaxVideoUploads))
            {
                divUploadVideo.Visible = true;
                divUploadMultipleVideosFlash.Visible = Config.Users.EnableFlashUploads;
                divUploadMultipleVideosSilverlight.Visible = Config.Users.EnableSilverlightUploads;
            }
            else
            {
                divUploadVideo.Visible = false;
                divUploadMultipleVideosFlash.Visible = false;
                divUploadMultipleVideosSilverlight.Visible = false;
            }

            DataTable dtUploadedVideos = new DataTable("UploadedVideos");

            dtUploadedVideos.Columns.Add("ID");
            dtUploadedVideos.Columns.Add("Status");
            dtUploadedVideos.Columns.Add("ImageURL");

            foreach (VideoUpload video in videos)
            {
                string thumbnail = String.Format("{0}/UserFiles/{1}/video_{2}.png", Config.Urls.Home,
                                                                        CurrentUserSession.Username, video.Id);
                if (!File.Exists(Server.MapPath(String.Format("~/UserFiles/{0}/video_{1}.png",
                                                                CurrentUserSession.Username, video.Id))))
                {
                    thumbnail = ResolveUrl("~/Images/uploadedvideo.gif");
                }

                string status = String.Empty;

                if (!video.Processed)
                {
                    status = Lang.Trans("Your video is queued for processing!");
                }
                else if (!video.Approved)
                {
                    status = Lang.Trans("Your video is awaiting for approval!");
                }
                else
                {
                    status = Lang.Trans("Your video is approved and online!");
                }

                dtUploadedVideos.Rows.Add(new object[] { video.Id, status, thumbnail });
            }

            rptVideoUploads.DataSource = dtUploadedVideos;
            rptVideoUploads.DataBind();
        }

        protected void btnAddYouTubeVideo_Click(object sender, EventArgs e)
        {
            pnlAddYouTubeVideoButton.Visible = false;
            divVideoKeywords.Visible = true;
            divVideoPreview.Visible = false;
            pnlBackButton.Visible = true;
            dlVideos.DataSource = null;
            dlVideos.DataBind();
        }

        protected void btnShowRecordVideo_Click(object sender, EventArgs e)
        {
            mvVideo.Visible = true;
            mvVideo.SetActiveView(vRecordVideo);
            RecordVideo1.User = ((PageBase)Page).CurrentUserSession;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            ShowEmbeddedVideos();
        }

        protected void rptVideoUploads_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int videoUploadID = Convert.ToInt32(e.CommandArgument);
            if (e.CommandName == "Delete")
            {
                VideoUpload.Delete(videoUploadID);

                Event[] events = Event.Fetch(CurrentUserSession.Username, (ulong)Event.eType.NewFriendVideoUpload, 1000);

                foreach (Event ev in events)
                {
                    NewFriendVideoUpload newFriendVideoUpload = Misc.FromXml<NewFriendVideoUpload>(ev.DetailsXML);
                    if (newFriendVideoUpload.VideoUploadID == videoUploadID)
                    {
                        Event.Delete(ev.ID);
                        break;
                    }
                }

                showUploadedVideos();
            }
        }

        protected void rptVideoUploads_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Button btnDeleteVideoUpload = e.Item.FindControl("btnDeleteVideoUpload") as Button;
            if (btnDeleteVideoUpload != null)
            {
                btnDeleteVideoUpload.Attributes.Add("onclick",
                                             String.Format("javascript: return confirm('{0}')",
                                                           Lang.Trans("Do you really want to delete this video?")));
                btnDeleteVideoUpload.Text = Lang.Trans("Delete");
            }
        }

        protected void lnkUploadMultipleFlash_Click(object sender, EventArgs e)
        {
            Guid guid = Guid.NewGuid();
            flashUpload.QueryParameters = "type=video&guid=" + guid;

            Cache.Insert("flashUpload_" + guid, CurrentUserSession.Username, null, DateTime.Now.AddMinutes(30),
                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

            string jsscript = "<SCRIPT LANGUAGE=\"JavaScript\">" +
                                  "function flashUploadIsCompleted() {" +
                                  "window.location.href='ManageProfile.aspx?sel=videouploads'" +
                                  "}" +
                                  "//  End -->" +
                                  "</SCRIPT>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "FlashUploadedIsCompleted", jsscript);

            pnlUploadMultipleVideosFlash.Visible = true;
            pnlUploadMultipleVideosSilverlight.Visible = false;
            lnkBackToVideoUploads.Visible = true;
            divUploadVideo.Visible = false;
            rptVideoUploads.Visible = false;
            divUploadMultipleVideosFlash.Visible = false;
            divUploadMultipleVideosSilverlight.Visible = false;
        }

        protected void lnkUploadMultipleSilverlight_Click(object sender, EventArgs e)
        {
            Guid guid = Guid.NewGuid();
            Silverlight1.InitParameters = "type=video" + ",guid" + "=" + guid; // key/value pairs

            Cache.Insert("silverlightUpload_" + guid, CurrentUserSession.Username, null, DateTime.Now.AddMinutes(30),
                         Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null);

            string jsscript = "<SCRIPT LANGUAGE=\"JavaScript\">" +
                                  "function silverlightUploadIsCompleted() {" +
                                  "window.location.href='ManageProfile.aspx?sel=videouploads'" +
                                  "}" +
                                  "//  End -->" +
                                  "</SCRIPT>";

            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "SilverlightUploadedIsCompleted", jsscript);

            pnlUploadMultipleVideosFlash.Visible = false;
            pnlUploadMultipleVideosSilverlight.Visible = true;
            lnkBackToVideoUploads.Visible = true;
            divUploadVideo.Visible = false;
            rptVideoUploads.Visible = false;
            divUploadMultipleVideosFlash.Visible = false;
            divUploadMultipleVideosSilverlight.Visible = false;
        }

        protected void lnkBackToVideoUploads_Click(object sender, EventArgs e)
        {
            btnShowUploadVideo_Click(null, null);
        }
    }
}