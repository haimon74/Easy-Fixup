using System;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class TopCharts : PageBase
    {
        public TopCharts()
        {
            RequiresAuthorization = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Config.Users.DisableGenderInformation
                    || (!Config.Ratings.EnablePhotoRatings && Config.Ratings.EnableRatePhotos)
                    && !Config.Ratings.EnableProfileRatings
                    &&
                    (!Config.CommunityModeratedSystem.EnableTopModerators ||
                     !Config.CommunityModeratedSystem.EnableCommunityPhotoApproval))
                {
                    Response.Redirect("~/Default.aspx");
                }

                loadStrings();

                int result = Convert.ToInt32(Config.Ratings.EnableProfileRatings) +
                             Convert.ToInt32(Config.Ratings.EnablePhotoRatings || Config.Ratings.EnableRatePhotos) +
                             Convert.ToInt32(Config.CommunityModeratedSystem.EnableCommunityPhotoApproval &&
                                             Config.CommunityModeratedSystem.EnableTopModerators);
                if (result == 1)
                {
                    pnlButtons.Visible = false;

                    if (Request.Params["show"] == null)
                    {
                        if (Config.Ratings.EnableProfileRatings)
                        {
                            btnShowTopUsers_Click(null, null);
                        }
                        else if (Config.Ratings.EnablePhotoRatings || Config.Ratings.EnableRatePhotos)
                        {
                            btnShowTopPhotos_Click(null, null);
                        }
                        else if (Config.CommunityModeratedSystem.EnableCommunityPhotoApproval &&
                                 Config.CommunityModeratedSystem.EnableTopModerators)
                        {
                            btnShowModerators_Click(null, null);
                        }
                    }
                }

                if (Request.Params["show"] == "users")
                {
                    mvTopCharts.SetActiveView(viewTopUsers);
                }
                else if (Request.Params["show"] == "photos")
                {
                    mvTopCharts.SetActiveView(viewTopPhotos);
                }
                else if (Request.Params["show"] == "moderators")
                {
                    mvTopCharts.SetActiveView(viewTopModerators);
                }
            }
        }

        private void loadStrings()
        {
            btnShowTopUsers.Text = Lang.Trans("Top Users");
            btnShowTopPhotos.Text = Lang.Trans("Top Photos");
            btnShowTopModerators.Text = Lang.Trans("Top Moderators");

            btnShowTopUsers.Visible = Config.Ratings.EnableProfileRatings;
            btnShowTopPhotos.Visible = Config.Ratings.EnablePhotoRatings || Config.Ratings.EnableRatePhotos;
            btnShowTopModerators.Visible = Config.CommunityModeratedSystem.EnableCommunityPhotoApproval &&
                                           Config.CommunityModeratedSystem.EnableTopModerators;
        }

        protected void btnShowTopUsers_Click(object sender, EventArgs e)
        {
            Response.Redirect("TopCharts.aspx?show=users");
        }

        protected void btnShowTopPhotos_Click(object sender, EventArgs e)
        {
            Response.Redirect("TopCharts.aspx?show=photos");
        }

        protected void btnShowModerators_Click(object sender, EventArgs e)
        {
            Response.Redirect("TopCharts.aspx?show=moderators");
        }
    }
}