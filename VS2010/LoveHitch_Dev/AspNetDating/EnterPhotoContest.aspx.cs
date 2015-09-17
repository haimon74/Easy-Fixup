using System;
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class EnterPhotoContestPage : PageBase
    {
        private int contestId
        {
            get
            {
                if (ViewState["ContestId"] != null)
                    return (int)ViewState["ContestId"];
                else
                    return 1;
            }
            set
            {
                ViewState["ContestId"] = value;
            }
        }

        public EnterPhotoContestPage()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                preparePage();
            }
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Enter Photo Contest");
            btnEnterContest.Text = Lang.Trans("Enter Contest");
            lnkBackToContest.Text = Lang.Trans("Back to Contest");
        }

        private void preparePage()
        {
            #region Load contest

            try
            {
                contestId = Convert.ToInt32(Request.Params["cid"]);
            }
            catch
            {
                StatusPageMessage = Lang.Trans("Please select a contest!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            PhotoContest contest = PhotoContest.Load(contestId);
            if (contest == null)
            {
                StatusPageMessage = Lang.Trans("Invalid contest!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            if (contest.Gender.HasValue && contest.Gender != CurrentUserSession.Gender)
            {
                StatusPageMessage = Lang.Trans("Photos of your gender are not accepted in this contest!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            if (contest.MinAge.HasValue && CurrentUserSession.Age < contest.MinAge.Value)
            {
                StatusPageMessage = String.Format(Lang.Trans("You need to be at least {0} years old to enter this contest!"),
                    contest.MinAge.Value);
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            if (contest.MaxAge.HasValue && CurrentUserSession.Age > contest.MaxAge.Value)
            {
                StatusPageMessage = String.Format(Lang.Trans("You need to be at most {0} years old to enter this contest!"),
                    contest.MaxAge.Value);
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            if (contest.DateEnds.HasValue && contest.DateEnds < DateTime.Now)
            {
                StatusPageMessage = Lang.Trans("This contest has already ended!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }
            PhotoContestEntry[] prevEntries =
                PhotoContestEntry.Load(null, contestId, CurrentUserSession.Username, null);
            if (prevEntries != null && prevEntries.Length > 0)
            {
                StatusPageMessage = Lang.Trans("You already participate in this contest!");
                Response.Redirect("ShowStatus.aspx");
                return;
            }

            lblContestName.Text = contest.Name;
            lblStartDate.Text = contest.DateCreated.ToShortDateString();
            if (contest.DateEnds.HasValue)
                lblEndDate.Text = contest.DateEnds.Value.ToShortDateString();
            else
                lblEndDate.Text = Lang.Trans("n/a");
            lblContestTerms.Text = contest.Terms;

            #endregion

            #region Load user photos

            Photo[] photos = Photo.Fetch(CurrentUserSession.Username);

            DataTable dtPhotos = new DataTable("Photos");
            dtPhotos.Columns.Add("PhotoId", typeof (int));
            dtPhotos.Columns.Add("Name");
            dtPhotos.Columns.Add("Description");

            if (photos == null || photos.Length == 0)
            {
                StatusPageMessage = Lang.Trans("You do not have any photos! Upload some photos and then try again.");
                Response.Redirect("ShowStatus.aspx");
                return;
            }

            foreach (Photo photo in photos)
            {
                if (!photo.Approved || photo.PrivatePhoto) continue;

                dtPhotos.Rows.Add(new object[] {photo.Id, photo.Name, photo.Description});
            }

            if (dtPhotos.Rows.Count == 0)
            {
                StatusPageMessage = Lang.Trans("You do not have any suitable photos! The photo should be approved and not marked as private.");
                Response.Redirect("ShowStatus.aspx");
                return;
            }

            dlPhotos.DataSource = dtPhotos;
            dlPhotos.DataBind();

            #endregion
        }

        protected void btnEnterContest_Click(object sender, EventArgs e)
        {
            int photoId = 0;
            for (int i = 0; i < dlPhotos.Items.Count; i++)
            {
                RadioButton rb = (RadioButton)dlPhotos.Items[i].FindControl("rbPhoto");
                if (rb == null) continue;
                if (rb.Checked)
                {
                    HiddenField hid = (HiddenField)dlPhotos.Items[i].FindControl("hidPhotoId");
                    int.TryParse(hid.Value, out photoId);
                    break;
                }
            }
            if (photoId == 0)
            {
                lblError.Text = Lang.Trans("Please select a photo!");
                return;
            }

            PhotoContestEntry prevEntry = PhotoContestEntry.Load(contestId, CurrentUserSession.Username);
            if (prevEntry != null)
            {
                lblError.Text = Lang.Trans("You already participate in this contest!");
                return;
            }

            PhotoContestEntry entry = new PhotoContestEntry(contestId, CurrentUserSession.Username, photoId);
            entry.Save();

            #region Add Event

            Event newEvent = new Event(CurrentUserSession.Username);

            newEvent.Type = Event.eType.FriendEntersContest;
            FriendEntersContest friendEntersContest = new FriendEntersContest();
            friendEntersContest.PhotoContestEntriesID = entry.Id;
            newEvent.DetailsXML = Misc.ToXml(friendEntersContest);

            newEvent.Save();

            PhotoContest contest = PhotoContest.Load(entry.ContestId);
            string[] usernames = Classes.User.FetchMutuallyFriends(CurrentUserSession.Username);

            foreach (string friendUsername in usernames)
            {
                if (Config.Users.NewEventNotification)
                {
                    if (contest != null)
                    {
                        string text =
                                    String.Format("Your friend {0} has entered the {1} contest".Translate(),
                                                  "<b>" + CurrentUserSession.Username + "</b>", contest.Name);
                        string thumbnailUrl = ImageHandler.CreateImageUrl(photoId, 50, 50, false, true, false);
                        Classes.User.SendOnlineEventNotification(CurrentUserSession.Username, friendUsername, text,
                                                                 thumbnailUrl, "PhotoContest.aspx?cid" + contest.Id);
                    }
                }
            }

            #endregion

            StatusPageMessage = Lang.Trans("Your contest entry has been saved!");
            StatusPageLinkText = Lang.Trans("Back to contest");
            StatusPageLinkURL = "PhotoContest.aspx?cid=" + contestId;
            Response.Redirect("ShowStatus.aspx");
        }

        protected void lnkBackToContest_Click(object sender, EventArgs e)
        {
            Response.Redirect("PhotoContest.aspx?cid=" + contestId);
        }
    }
}