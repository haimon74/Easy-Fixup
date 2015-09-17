using System;
using System.Data;
using System.IO;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Profile
{
    public partial class UploadAudio : System.Web.UI.UserControl
    {
        protected UserSession CurrentUserSession
        {
            get { return ((PageBase)Page).CurrentUserSession; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            showAudioUploads();
        }

        private void loadStrings()
        {
            cvNote.Text = "Upload Audio Notes".Translate();
            LargeBoxStart1.Title = Lang.Trans("Upload Audio");
            HeaderLine1.Title = Lang.Trans("Upload Audio File");
            btnUpload.Text = Lang.Trans("Upload");
        }

        private void showAudioUploads()
        {
            AudioUpload[] audioUploads = AudioUpload.Load(null, CurrentUserSession.Username, null, null);

            if (audioUploads.Length < CurrentUserSession.BillingPlanOptions.MaxAudioUploads.Value
                || (CurrentUserSession.Level != null && audioUploads.Length < CurrentUserSession.Level.Restrictions.MaxAudioUploads))
            {
                divUploadAudio.Visible = true;
            }
            else
            {
                divUploadAudio.Visible = false;
            }

            DataTable dtAudioUploads = new DataTable("AudioUploads");

            dtAudioUploads.Columns.Add("ID");
            dtAudioUploads.Columns.Add("Status");
            dtAudioUploads.Columns.Add("ImageURL");
            dtAudioUploads.Columns.Add("Title");

            foreach (AudioUpload audioUpload in audioUploads)
            {
                string thumbnail = ResolveUrl("~/Images/uploadedaudio.gif");
                string status = String.Empty;

                if (!audioUpload.Approved)
                {
                    status = Lang.Trans("Your audio is awaiting for approval!");
                }
                else
                {
                    status = Lang.Trans("Your audio is approved and online!");
                }

                dtAudioUploads.Rows.Add(new object[] { audioUpload.Id, status, thumbnail, audioUpload.Title });
            }

            rptAudioUploads.DataSource = dtAudioUploads;
            rptAudioUploads.DataBind();
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
                AudioUpload[] audioUploads = AudioUpload.Load(null, CurrentUserSession.Username, null, null);

                if (audioUploads.Length >= CurrentUserSession.BillingPlanOptions.MaxAudioUploads.Value
                    && (CurrentUserSession.Level != null && audioUploads.Length >= CurrentUserSession.Level.Restrictions.MaxAudioUploads))
                {
                    ((PageBase)Page).StatusPageMessage = Lang.Trans("You cannot upload more audio files!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

            if (!fileAudio.HasFile)
            {
                lblError.Text = Lang.Trans("Please select audio file!");
                return;
            }

            string title = txtTitle.Text.Length > 0 ? txtTitle.Text : Path.GetFileNameWithoutExtension(fileAudio.FileName);

            //string tempfile = Path.GetTempFileName();
            //fileAudio.SaveAs(tempfile);

            var audioUpload = new AudioUpload(CurrentUserSession.Username);
            if (CurrentUserSession != null)
            {
                if (CurrentUserSession.BillingPlanOptions.AutoApproveAudioUploads.Value
                    || CurrentUserSession.Level != null && CurrentUserSession.Level.Restrictions.AutoApproveAudioUploads)
                {
                    audioUpload.Approved = true;
                }
            }
            audioUpload.Title = title;
            audioUpload.IsPrivate = cbPrivateAudio.Checked;
            audioUpload.Save(); // Save to get new ID

            if (audioUpload.Approved && !audioUpload.IsPrivate)
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
                        string text = String.Format("Your friend {0} has uploaded a new audio".Translate(),
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
                                user = User.Load(audioUpload.Username);
                                imageID = ImageHandler.GetPhotoIdByGender(user.Gender);
                            }
                            catch (NotFoundException) { break; }
                        }
                        string thumbnailUrl = ImageHandler.CreateImageUrl(imageID, 50, 50, false, true, true);
                        User.SendOnlineEventNotification(audioUpload.Username, friendUsername,
                                                                 text, thumbnailUrl,
                                                                 UrlRewrite.CreateShowUserUrl(audioUpload.Username));
                    }
                }

                #endregion
            }

            string userFilesPath = "~/UserFiles/" + CurrentUserSession.Username;
            string userFilesDir = Server.MapPath(userFilesPath);
            if (!Directory.Exists(userFilesDir))
            {
                Directory.CreateDirectory(userFilesDir);
            }

            fileAudio.SaveAs(userFilesDir + @"\audio_" + audioUpload.Id + ".mp3");

            //File.Move(tempfile, userFilesDir + @"\audio_" + audioUpload.Id + ".mp3");
        }

        protected void rptAudioUploads_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                int audioUploadID = Convert.ToInt32(e.CommandArgument);

                AudioUpload.Delete(audioUploadID);
            }
        }

        protected void rptAudioUploads_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
            Button btnDeleteAudioUpload = e.Item.FindControl("btnDeleteAudioUpload") as Button;
            if (btnDeleteAudioUpload != null)
            {
                btnDeleteAudioUpload.Attributes.Add("onclick",
                                             String.Format("javascript: return confirm('{0}')",
                                                           Lang.Trans("Do you really want to delete this audio?")));
                btnDeleteAudioUpload.Text = Lang.Trans("Delete");
            }
        }
    }
}