using System;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ApprovePhotos2 : AdminPageBase
    {
        protected Button btnCancel;

        private int photoID
        {
            get
            {
                if (ViewState["photoID"] != null)
                    return (int) ViewState["photoID"];
                else
                    return 0;
            }
            set { ViewState["photoID"] = value; }
        }

        public ApprovePhotos2()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                LoadStrings();
                PopulateDropDown();
                PopulateDataGrid();
            }
        }

        private void LoadStrings()
        {
            lblPhotosPerPage.Text = Lang.TransA("Photos per page");

            btnApprove.Text = Lang.TransA("Approve");
            btnReject.Text = Lang.TransA("Reject");
            btnApproveAll.Text = Lang.TransA("Approve All");
            chkExplicitPhoto.Text = Lang.TransA("Explicit Photo");

            TBWEReason.WatermarkText = Lang.TransA("Type Reason Here");

            if (!HasWriteAccess)
            {
                btnApprove.Enabled = false;
                btnReject.Enabled = false;
                btnApproveAll.Enabled = false;
            }

            btnReject.Attributes.Add("onclick",
                                     String.Format("javascript: return confirm('{0}')",
                                                   Lang.TransA("Do you really want to reject this photo?")));

            if (!Config.Photos.EnableExplicitPhotos)
                trExplicitPhoto.Visible = false;

            if (CurrentAdminSession.Username != "admin")
            {
                btnApproveAll.Visible = false;
                cbDeleteAccount.Visible = false;
            }

            cbDeleteAccount.Text = Lang.TransA("Delete Account");
        }

        private void LoadImage()
        {
            try
            {
                imgBigPhoto.ImageUrl = String.Format("{0}{1}{2}", Config.Urls.Home, "/Image.ashx?id=", photoID);
            }
            catch (InvalidCastException)
            {
            }

            Photo photo = Photo.Fetch(Convert.ToInt32(photoID));

            if (photo != null)
            {
                lblUsername.Text = photo.User.Username;
                lblPhotoName.Text = photo.Name;
                lblPhotoDescription.Text = photo.Description;
            }
        }

        private void PopulateDropDown()
        {
            for (int i = 5; i <= 50; i += 5)
                dropPhotosPerPage.Items.Add(i.ToString());
            dropPhotosPerPage.SelectedValue = Config.AdminSettings.ApprovePhotos.PhotosPerPage.ToString();
        }

        private void PopulateDataGrid()
        {
            Photo[] photos = null;
            if (!Config.CommunityFaceControlSystem.EnableCommunityFaceControl)
            {
                photos = Photo.FetchNonApproved();
            }
            else
            {
                photos = Photo.FetchNonApproved(true);
            }

            if (photos == null || photos.Length == 0)
            {
                MessageBox.Show(Lang.TransA("There are no photos waiting for approval!"), Misc.MessageType.Error);
                listPendingApproval.Visible = false;
                pnlPhotosPerPage.Visible = false;
                stable.Visible = false;
                btnApproveAll.Visible = false;
            }
            else
            {
                bindPhotoDetails(photos);

                listPendingApproval.Visible = true;
                pnlPhotosPerPage.Visible = true;
                stable.Visible = true;
                btnApproveAll.Visible = true;
            }
        }

        private void bindPhotoDetails(Photo[] photos)
        {
            DataTable dtPhotoDetails = new DataTable("PhotoDetails");
            dtPhotoDetails.Columns.Add("Username");
            dtPhotoDetails.Columns.Add("Name");
            dtPhotoDetails.Columns.Add("Description");
            dtPhotoDetails.Columns.Add("PhotoID");

            int count = 0;
            foreach (Photo photo in photos)
            {
                if (count == 0) photoID = photo.Id;

                if (photo.Name == "")
                    photo.Name = Lang.TransA("N/A");

                if (photo.Description == "")
                    photo.Description = Lang.TransA("N/A");

                dtPhotoDetails.Rows.Add(new object[]
                                            {
                                                photo.User.Username,
                                                photo.Name,
                                                photo.Description.Length > 50
                                                    ? photo.Description.Substring(0, 50) + "..."
                                                    : photo.Description,
                                                photo.Id
                                            }
                    );

                if (++count == 50) break;
            }

            //dtPhotoDetails.DefaultView.Sort = "Username";

            listPendingApproval.DataSource = dtPhotoDetails;
            try
            {
                listPendingApproval.DataBind();
            }
            catch (HttpException)
            {
                //listPendingApproval.CurrentPageIndex = 0;
                listPendingApproval.DataBind();
            }

            LoadImage();
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.photoApproval;            
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            //gridPendingApproval.PageIndexChanged += new DataGridPageChangedEventHandler(gridPendingApproval_PageIndexChanged);
            this.dropPhotosPerPage.SelectedIndexChanged += new EventHandler(dropPhotosPerPage_SelectedIndexChanged);
            this.listPendingApproval.ItemCommand += new DataListCommandEventHandler(listPendingApproval_ItemCommand);
        }

        #endregion

        /*
		private void gridPendingApproval_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
		{
			gridPendingApproval.CurrentPageIndex = e.NewPageIndex;
			PopulateDataGrid();
		}
		*/

        private void dropPhotosPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            //gridPendingApproval.PageSize = Convert.ToInt32(dropPhotosPerPage.SelectedValue);
            //gridPendingApproval.CurrentPageIndex = 0;
            PopulateDataGrid();
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            Photo photo = Photo.Fetch(photoID);
            if (Config.Photos.EnableExplicitPhotos)
            {
                photo.ExplicitPhoto = chkExplicitPhoto.Checked;
                if (photo.ExplicitPhoto && Config.Photos.MakeExplicitPhotosPrivate)
                {
                    photo.PrivatePhoto = true;
                }
            }

            photo.ApprovePhoto(CurrentAdminSession.Username);

            if (!photo.PrivatePhoto)
            {
                #region Add Event

                Event newEvent = new Event(photo.Username);

                newEvent.Type = Event.eType.NewFriendPhoto;
                NewFriendPhoto newFriendPhoto = new NewFriendPhoto();
                newFriendPhoto.PhotoID = photo.Id;
                newEvent.DetailsXML = Misc.ToXml(newFriendPhoto);

                newEvent.Save();

                string[] usernames = Classes.User.FetchMutuallyFriends(photo.Username);

                foreach (string friendUsername in usernames)
                {
                    if (Config.Users.NewEventNotification)
                    {
                        string text = String.Format("Your friend {0} has uploaded a new photo".TranslateA(),
                                                      "<b>" + photo.Username + "</b>");
                        string thumbnailUrl = ImageHandler.CreateImageUrl(photo.Id, 50, 50, false, true, true);
                        Classes.User.SendOnlineEventNotification(photo.Username, friendUsername,
                                                                 text, thumbnailUrl,
                                                                 UrlRewrite.CreateShowUserPhotosUrl(photo.Username));
                    }
                }

                #endregion
            }

            Classes.User.AddScore(photo.Username, Config.UserScores.ApprovedPhoto, "ApprovedPhoto");

            try
            {
                MiscTemplates.ApprovePhotoMessage approvePhotoMessageTemplate =
                    new MiscTemplates.ApprovePhotoMessage(photo.User.LanguageId);
                Message.Send(Config.Users.SystemUsername, photo.User.Username, approvePhotoMessageTemplate.Message, 0);
            }
            catch (Exception ex)
            {
                Log(ex);
            }

            chkExplicitPhoto.Checked = false;

            PopulateDataGrid();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            Photo photo = Photo.Fetch(photoID);
            MiscTemplates.RejectPhotoMessage rejectPhotoMessageTemplate =
                new MiscTemplates.RejectPhotoMessage(photo.User.LanguageId);
            string reasonMessage = rejectPhotoMessageTemplate.WithReasonMessage;

            if (txtReason.Text != "")
            {
                reasonMessage = reasonMessage.Replace("%%REASON%%", txtReason.Text);
                Message.Send(Config.Users.SystemUsername, photo.User.Username, reasonMessage, 0);
            }
            else
                Message.Send(Config.Users.SystemUsername, photo.User.Username,
                             rejectPhotoMessageTemplate.WithNoReasonMessage, 0);

            Photo.Delete(photoID);
            Classes.User.AddScore(photo.Username, Config.UserScores.RejectedPhoto, "RejectedPhoto");

            if (cbDeleteAccount.Checked)
            {
                Classes.User.Delete(photo.Username, txtReason.Text);
            }

            PopulateDataGrid();

            txtReason.Text = "";
            cbDeleteAccount.Checked = false;
        }

        private void listPendingApproval_ItemCommand(object source, DataListCommandEventArgs e)
        {
            photoID = Convert.ToInt32(e.CommandArgument);
            LoadImage();
        }

        protected void btnApproveAll_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            foreach (DataListItem item in listPendingApproval.Items)
            {
                int photoId = Convert.ToInt32(((LinkButton) item.Controls[1]).CommandArgument);

                Photo photo = Photo.Fetch(photoId);

                photo.ApprovePhoto(CurrentAdminSession.Username);

                if (!photo.PrivatePhoto)
                {
                    #region Add Event

                    Event newEvent = new Event(photo.Username);

                    newEvent.Type = Event.eType.NewFriendPhoto;
                    NewFriendPhoto newFriendPhoto = new NewFriendPhoto();
                    newFriendPhoto.PhotoID = photo.Id;
                    newEvent.DetailsXML = Misc.ToXml(newFriendPhoto);

                    newEvent.Save();

                    string[] usernames = Classes.User.FetchMutuallyFriends(photo.Username);

                    foreach (string friendUsername in usernames)
                    {
                        if (Config.Users.NewEventNotification)
                        {
                            string text = String.Format("Your friend {0} has uploaded a new photo".TranslateA(),
                                                      "<b>" + photo.Username + "</b>");
                            string thumbnailUrl = ImageHandler.CreateImageUrl(photo.Id, 50, 50, false, true, true);
                            Classes.User.SendOnlineEventNotification(photo.Username, friendUsername,
                                                                     text, thumbnailUrl,
                                                                     UrlRewrite.CreateShowUserPhotosUrl(photo.Username));
                        }
                    }

                    #endregion 
                }

                try
                {
                    MiscTemplates.ApprovePhotoMessage approvePhotoMessageTemplate =
                        new MiscTemplates.ApprovePhotoMessage(photo.User.LanguageId);
                    Message.Send(Config.Users.SystemUsername, photo.User.Username, approvePhotoMessageTemplate.Message, 0);
                }
                catch (Exception ex)
                {
                    Log(ex);
                }
            }

            PopulateDataGrid();
        }
    }
}