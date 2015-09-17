using System;
using System.Data;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class RatePhotos : PageBase
    {
        protected int CurrentPhotoId
        {
            get { return (int) (ViewState["CurrentPhotoId"] ?? 0); }
            set { ViewState["CurrentPhotoId"] = value; }
        }

        protected int PrevPhotoId
        {
            get { return (int)(ViewState["PrevPhotoId"] ?? 0); }
            set { ViewState["PrevPhotoId"] = value; }
        }

        protected string CurrentPhotoOwner
        {
            get { return ViewState["CurrentPhotoOwner"] as string; }
            set { ViewState["CurrentPhotoOwner"] = value; }
        }

        protected decimal Rating
        {
            get { return (decimal) (ViewState["Rating"] ?? 0m); }
            set { ViewState["Rating"] = value; }
        }

        protected int Votes
        {
            get { return (int) (ViewState["Votes"] ?? 0); }
            set { ViewState["Votes"] = value; }
        }

        protected int CurrentVote
        {
            get { return (int) (ViewState["CurrentVote"] ?? 0); }
            set { ViewState["CurrentVote"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!Config.Ratings.EnableRatePhotos)
                {
                    Response.Redirect("~/Default.aspx");
                    return;
                }

                var permissionCheckResult = CurrentUserSession.CanRatePhotos();

                if (permissionCheckResult == PermissionCheckResult.Yes ||
                    (CurrentUserSession.Level != null &&
                                       CurrentUserSession.Level.Restrictions.CanRatePhotos))
                {
                }
                else if (permissionCheckResult == PermissionCheckResult.YesButMoreCreditsNeeded ||
                         permissionCheckResult == PermissionCheckResult.YesButPlanUpgradeNeeded)
                {
                    Global.GetSessionState()["BillingPlanOption"] = CurrentUserSession.BillingPlanOptions.CanRatePhotos;
                    Response.Redirect("~/ManageProfile.aspx?sel=payment");
                    return;
                }
                else if (permissionCheckResult == PermissionCheckResult.No)
                {
                    StatusPageMessage = Lang.Trans("You are not allowed to rate photos!");
                    Response.Redirect("ShowStatus.aspx");
                    return;
                }

                //if (!Config.Ratings.EnableRatePhotos
                //    || (!CurrentUserSession.BillingPlanOptions.CanRatePhotos
                //            && (CurrentUserSession.Level == null || !CurrentUserSession.Level.Restrictions.CanRatePhotos)))
                //{
                //    Response.Redirect("~/Default.aspx");
                //    return;
                //}

                loadStrings();
                bindVotes();
                loadPhoto();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            pnlRatedPhoto.Visible = PrevPhotoId != 0;
            pnlRating.Visible = PrevPhotoId != 0;
            cvRatePhotos.Visible = PrevPhotoId == 0 && CurrentPhotoId != 0;
        }

        private void loadStrings()
        {
            cvRatePhotos.Text = "Rate Photos".Translate();
            SmallBoxStart1.Title = "Rating".Translate();
            LargeBoxStart1.Title = "Rate photos".Translate();
            txtFrom.Text = Config.Users.MinAge.ToString();
            txtTo.Text = Config.Users.MaxAge.ToString();
            ddGender.Items.Add(new ListItem("All".Translate(), "-1"));

            if (!Config.Users.DisableGenderInformation)
            {
                ddGender.Items.Add(new ListItem(Lang.Trans("Male"), ((int)Classes.User.eGender.Male).ToString()));
                ddGender.Items.Add(new ListItem(Lang.Trans("Female"), ((int)Classes.User.eGender.Female).ToString()));
                if (Config.Users.CouplesSupport)
                {
                    ddGender.Items.Add(new ListItem(Lang.Trans("Couple"), ((int)Classes.User.eGender.Couple).ToString()));
                }

                ddGender.SelectedValue = ((int)CurrentUserSession.InterestedIn).ToString();
            }
            
            pnlGender.Visible = !Config.Ratings.LimitToInterestedGender && !Config.Users.DisableGenderInformation;
            pnlAge.Visible = !Config.Users.DisableAgeInformation;
        }

        protected void rptRating_ItemCreated(object sender, RepeaterItemEventArgs e)
        {
        }

        private void bindVotes()
        {
            DataTable dtVotes = new DataTable();
            dtVotes.Columns.Add("Vote", typeof(int));
            int votes = (Config.Ratings.MaxRating - Config.Ratings.MinRating) + 1;
            for (int i = Config.Ratings.MinRating; i <= votes; i++) dtVotes.Rows.Add(new object[] { i });
            rptVotes.DataSource = dtVotes;
            rptVotes.DataBind();
        }

        protected void rptRating_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            int vote = Convert.ToInt32(e.CommandArgument);
            CurrentVote = vote;
            
            try
            {
                PhotoRating.RatePhoto(CurrentUserSession.Username, CurrentPhotoId, vote);

                LoadRating(CurrentPhotoId);
            }
            catch (NullReferenceException)
            {
                Response.Redirect("default.aspx?back_url=" + Server.UrlEncode(Request.Url.AbsoluteUri));
            }
            catch (FormatException)
            {
                //do nothing
            }
            catch (Exception err)
            {
                ExceptionLogger.Log(Request, err);
            }

            loadPhoto();
            pnlRating.Visible = true;
        }

        private void loadPhoto()
        {
            int minAge;
            int maxAge;
            if (!Int32.TryParse(txtFrom.Text.Trim(), out minAge)) minAge = Config.Users.MinAge;
            if (!Int32.TryParse(txtTo.Text.Trim(), out maxAge)) maxAge = Config.Users.MaxAge;
            int photoID = 0;
            if (ddGender.SelectedValue == "-1")
                photoID = Classes.User.GetRandomPhotoId(CurrentUserSession.Username, null, minAge, maxAge);
            else
                photoID =
                    Classes.User.GetRandomPhotoId(CurrentUserSession.Username,
                                                  (User.eGender) Int32.Parse(ddGender.SelectedValue), minAge, maxAge);
            PrevPhotoId = CurrentPhotoId;

            if (photoID == 0)
            {
                CurrentPhotoId = 0;
                pnlVotes.Visible = false;
                pnlCurrentPhoto.Visible = false;
                lblError.Text = "There are no photos to vote for".Translate();
                return;
            }
            pnlVotes.Visible = true;
            pnlCurrentPhoto.Visible = true;
            ltrPhoto.Text = ImageHandler.RenderImageTag(photoID, 450, 450, "photoframe", false, true);
            CurrentPhotoId = photoID;
            Photo photo = null;
            try
            {
                photo = Photo.Fetch(photoID);
            }
            catch (NotFoundException) { return; }
            CurrentPhotoOwner = photo.Username;
            lblName.Text = Server.HtmlEncode(photo.Name);
            lblName.Visible = photo.Name.Trim().Length > 0;
            lblDescription.Text = Server.HtmlEncode(photo.Description);
            lblDescription.Visible = photo.Description.Trim().Length > 0;
            lnkUser.HRef = UrlRewrite.CreateShowUserUrl(photo.Username);
            lnkUser.InnerText = photo.Username;
            lblAge.Text = photo.User.Age.ToString();
            lblLocation.Text = photo.User.LocationString;
            lblLocation.Visible = Config.Users.LocationPanelVisible;
        }

        private void LoadRating(int photoID)
        {
            // Show rating
            try
            {
                var photoRating = new PhotoRating(photoID);

                Rating = photoRating.AverageVote;
                Votes = photoRating.Votes;
            }
            catch (NotFoundException)
            {
                Rating = 0;
                Votes = 0;
            }
        }

        protected void ddGender_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrevPhotoId = 0;
            CurrentPhotoId = 0;
            loadPhoto();
        }

        protected void txtFrom_TextChanged(object sender, EventArgs e)
        {
            PrevPhotoId = 0;
            CurrentPhotoId = 0;
            loadPhoto();
        }

        protected void txtTo_TextChanged(object sender, EventArgs e)
        {
            PrevPhotoId = 0;
            CurrentPhotoId = 0;
            loadPhoto();
        }
    }
}
