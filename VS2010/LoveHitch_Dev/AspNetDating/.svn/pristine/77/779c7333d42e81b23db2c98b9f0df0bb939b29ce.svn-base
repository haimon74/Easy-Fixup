using System;
using System.Data;
using AspNetDating.Classes;

namespace AspNetDating
{
    public partial class PhotoContestPage : PageBase
    {
        #region Private viewstate preserved properties

        private int contestId
        {
            get
            {
                if (ViewState["contestId"] != null)
                    return (int) ViewState["contestId"];
                else
                    return 1;
            }
            set { ViewState["contestId"] = value; }
        }

        private PhotoContestEntry currentEntry
        {
            get { return ViewState["currentEntry"] as PhotoContestEntry; }
            set { ViewState["currentEntry"] = value; }
        }

        private int? currentEntryMinRank
        {
            get { return ViewState["currentEntryMinRank"] as int?; }
            set { ViewState["currentEntryMinRank"] = value; }
        }

        private int? currentEntryMaxRank
        {
            get { return ViewState["currentEntryMaxRank"] as int?; }
            set { ViewState["currentEntryMaxRank"] = value; }
        }

        private PhotoContestEntry leftEntry
        {
            get { return ViewState["leftEntry"] as PhotoContestEntry; }
            set { ViewState["leftEntry"] = value; }
        }

        private PhotoContestEntry rightEntry
        {
            get { return ViewState["rightEntry"] as PhotoContestEntry; }
            set { ViewState["rightEntry"] = value; }
        }

        #endregion

        public PhotoContestPage()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                preparePage();
                loadNextEntries();
                loadPersonalFavs();

                if (Request.Params["top"] == "1" || divContestHasEnded.Visible)
                    lnkViewTopEntries_Click(null, null);
            }
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Results");
            LargeBoxStart1.Title = Lang.Trans("Photo Contest");
            btnEnterContest.Text = Lang.Trans("Enter Contest");
            btnPickLeft.Text = Lang.Trans("Pick");
            btnPickRight.Text = Lang.Trans("Pick");
            lnkViewAllFavourites.Text = Lang.Trans("View all favorites");
            btnBackToPhotos.Text = Lang.Trans("Back to contest");
            btnBackToPhotos2.Text = Lang.Trans("Back to contest");
            lnkViewTopEntries.Text = Lang.Trans("View top entries");
            btnViewTopEntries.Text = Lang.Trans("View top entries");
            btnLeaveContest.Text = Lang.Trans("Remove my entry");
            btnLeaveContest.Attributes.Add("onclick", "return confirm('" + Lang.Trans("Do you really want to remove your entry?") + "')");
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

            hlContestName.Title = contest.Name;
            lblContestDescription.Text = contest.Description;

            PhotoContestEntry[] entries = PhotoContestEntry.LoadByContest(contestId);
            if (entries.Length < Config.Ratings.MinPhotosToStartContest)
            {
                divNotEnoughPhotos.Visible = true;
                divPhotos.Visible = false;
                btnViewTopEntries.Visible = false;
            }

            if (contest.DateEnds.HasValue && contest.DateEnds.Value < DateTime.Now)
            {
                divContestHasEnded.Visible = true;
                divPhotos.Visible = false;
                btnBackToPhotos.Visible = false;
                btnBackToPhotos2.Visible = false;
                btnEnterContest.Visible = false;
                btnViewTopEntries.Visible = false;
                divNoRankedMessage.Visible = false;
                lnkViewAllFavourites.Visible = false;
                lnkViewTopEntries.Visible = false;
                btnLeaveContest.Visible = false;
            }

            #endregion

            #region Load user entry

            PhotoContestEntry entry = PhotoContestEntry.Load(contestId, CurrentUserSession.Username);
            if (entry != null)
            {
                btnEnterContest.Visible = false;

                int currentRank = PhotoContestEntry.FindRank(contestId, entry.Id);
                if (currentRank > 0)
                {
                    lblCurrentRank.Text =
                        String.Format(Lang.Trans("Your entry is ranked #{0} among all entries!"), currentRank);
                }
                divContestRank.Visible = true;
            }

            #endregion
        }

        private void loadNextEntries()
        {
            if (!divPhotos.Visible) return;

            #region Prepare entries

            if (currentEntry == null)
            {
                if (Session["LastShowedEntries_" + contestId] is PhotoContestEntry[])
                {
                    leftEntry = ((PhotoContestEntry[])Session["LastShowedEntries_" + contestId])[0];
                    rightEntry = ((PhotoContestEntry[])Session["LastShowedEntries_" + contestId])[1];
                }
                else
                {
                    PhotoContestEntry[] entries = PhotoContestEntry.Load(null, contestId, null, null,
                                                                         CurrentUserSession.Username, 2, true);

                    if (entries == null || entries.Length < 2)
                    {
                        // TODO: show that there are no more entries
                        divPhotos.Visible = false;
                        return;
                    }
                    else
                    {
                        leftEntry = entries[0];
                        rightEntry = entries[1];
                    }
                }
            }
            else
            {
                PhotoContestRank[] ranks = PhotoContestRank.Load(CurrentUserSession.Username, contestId);

                if (ranks == null || ranks.Length == 0)
                {
                    // Should not happen
                    Global.Logger.LogWarning("PhotoContest_loadNextEntries",
                                             "Warning: There is a current photo but there are no ranks");
                    return;
                }

                leftEntry = currentEntry;
                if (currentEntryMinRank == null && currentEntryMaxRank == null
                    && ranks.Length > Config.Ratings.FavoriteEntriesCount)
                {
                    rightEntry = PhotoContestEntry.Load(ranks[Config.Ratings.FavoriteEntriesCount - 1].EntryId);
                }
                else
                {
                    if (currentEntryMinRank == null) currentEntryMinRank = 0;
                    if (currentEntryMaxRank == null) currentEntryMaxRank = ranks.Length + 1;

                    rightEntry = PhotoContestEntry.Load(
                        ranks[(currentEntryMinRank.Value + currentEntryMaxRank.Value)/2 - 1].EntryId);
                }
            }

            #endregion

            #region Display entries

            imgLeftPhoto.ImageUrl = ImageHandler.CreateImageUrl(leftEntry.PhotoId, 200, 300, false, false, false);
            lnkLeftPhoto.HRef = ImageHandler.CreateImageUrl(leftEntry.PhotoId, 800, 600, false, false, false);
            lnkLeftPhoto.Title = leftEntry.Username;
            linkLeftUsername.InnerHtml = leftEntry.Username;
            linkLeftUsername.HRef = UrlRewrite.CreateShowUserUrl(leftEntry.Username);
            linkLeftUsername.Target = "_new";

            imgRightPhoto.ImageUrl = ImageHandler.CreateImageUrl(rightEntry.PhotoId, 200, 300, false, false, false);
            lnkRightPhoto.HRef = ImageHandler.CreateImageUrl(rightEntry.PhotoId, 800, 600, false, false, false);
            lnkRightPhoto.Title = rightEntry.Username;
            linkRightUsername.InnerHtml = rightEntry.Username;
            linkRightUsername.HRef = UrlRewrite.CreateShowUserUrl(rightEntry.Username);
            linkRightUsername.Target = "_new";

            #endregion

            #region Save last two showed entries

            Session["LastShowedEntries_" + contestId] = new PhotoContestEntry[] { leftEntry, rightEntry };

            #endregion
        }

        private void loadPersonalFavs()
        {
            DataTable dtFavs = new DataTable();
            dtFavs.Columns.Add("EntryId", typeof (int));
            dtFavs.Columns.Add("PhotoId", typeof (int));
            dtFavs.Columns.Add("Username", typeof (string));
            dtFavs.Columns.Add("Rank", typeof (int));

            PhotoContestRank[] ranks = PhotoContestRank.Load(CurrentUserSession.Username, contestId);
            int count = 0;
            foreach (PhotoContestRank rank in ranks)
            {
                PhotoContestEntry entry = PhotoContestEntry.Load(rank.EntryId);
                dtFavs.Rows.Add(new object[] {rank.EntryId, entry.PhotoId, entry.Username, rank.Value});
                if (++count >= 3) break;
            }

            dlFavouriteEntries.DataSource = dtFavs;
            dlFavouriteEntries.DataBind();

            divPersonalFavs.Visible = dtFavs.Rows.Count > 0;

            lnkViewAllFavourites.Visible = ranks.Length > 3;
        }

        private void rankEntries(PhotoContestEntry pick, PhotoContestEntry nonpick)
        {
            Session["LastShowedEntries_" + contestId] = null;

            #region Check for duplicate ranking (usually caused by refreshing)

            string lastRanked = leftEntry.Id + "_" + rightEntry.Id;
            if ((string) Session["PhotoContestPage_lastRankEntries"] == lastRanked) return;
            Session["PhotoContestPage_lastRankEntries"] = lastRanked;

            #endregion

            #region Show last ranked

            divNoRankedMessage.Visible = false;
            divLastRankedMessage.Visible = true;

            imgLastLeft.ImageUrl = ImageHandler.CreateImageUrl(pick.PhotoId, 50, 50, false, false, false);
            linkLastLeft.InnerHtml = pick.Username;
            linkLastLeft.HRef = UrlRewrite.CreateShowUserUrl(pick.Username);
            linkLastLeft.Target = "_new";

            imgLastRight.ImageUrl = ImageHandler.CreateImageUrl(nonpick.PhotoId, 50, 50, false, false, false);
            linkLastRight.InnerHtml = nonpick.Username;
            linkLastRight.HRef = UrlRewrite.CreateShowUserUrl(nonpick.Username);
            linkLastRight.Target = "_new";

            lblVotersAgree.Text = String.Format(Lang.Trans("{0}% of voters agree"), Convert.ToInt32(
                                                                                       PhotoContestVotes.FetchPercentage
                                                                                           (pick.Id, nonpick.Id).
                                                                                           GetValueOrDefault(100)));

            #endregion

            #region Save vote

            try
            {
                PhotoContestVotes.SaveVote(CurrentUserSession.Username, pick.Id, nonpick.Id);
            }
            catch (Exception err)
            {
                Global.Logger.LogError("rankEntries", err);
            }

            #endregion

            #region Update ranking

            PhotoContestRank[] ranks = PhotoContestRank.Load(CurrentUserSession.Username, contestId);

            if (ranks == null || ranks.Length == 0)
            {
                PhotoContestRank rank1 = new PhotoContestRank(CurrentUserSession.Username, contestId,
                                                              pick.Id, 1);
                rank1.Save();
                PhotoContestRank rank2 = new PhotoContestRank(CurrentUserSession.Username, contestId,
                                                              nonpick.Id, 2);
                rank2.Save();
                currentEntry = null;
                currentEntryMinRank = null;
                currentEntryMaxRank = null;
                loadPersonalFavs();
            }
            else if (currentEntry == null)
            {
                currentEntry = pick;
                currentEntryMinRank = null;
                currentEntryMaxRank = null;
            }
            else
            {
                if (currentEntry.Id == pick.Id)
                {
                    // The user picked the newly introduced photo
                    foreach (PhotoContestRank rank in ranks)
                    {
                        // Find the rank of the other (already ranked) photo
                        if (rank.EntryId == nonpick.Id)
                        {
                            // Check if the picked photo reached first rank
                            if (rank.Value == 1)
                            {
                                PhotoContestRank newRank = new PhotoContestRank(CurrentUserSession.Username,
                                                                                contestId, pick.Id, 1);
                                newRank.Save();
                                currentEntry = null;
                                currentEntryMinRank = null;
                                currentEntryMaxRank = null;
                                loadPersonalFavs();
                            }
                                // Check if the picked photo found its rank
                            else if (rank.Value - currentEntryMinRank == 1)
                            {
                                PhotoContestRank newRank = new PhotoContestRank(CurrentUserSession.Username,
                                                                                contestId, pick.Id, rank.Value);
                                newRank.Save();
                                currentEntry = null;
                                currentEntryMinRank = null;
                                currentEntryMaxRank = null;
                                loadPersonalFavs();
                            }
                            else
                            {
                                // Limit the maximum rank
                                currentEntryMaxRank = rank.Value;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    // The user picked one if his previously ranked photos
                    foreach (PhotoContestRank rank in ranks)
                    {
                        // Find the rank of the picked (already ranked) photo
                        if (rank.EntryId == pick.Id)
                        {
                            // Check if the non picked photo is going out of the chart
                            if (rank.Value == Config.Ratings.FavoriteEntriesCount)
                            {
                                currentEntry = null;
                                currentEntryMinRank = null;
                                currentEntryMaxRank = null;
                            }
                                // Check if the picked photo found its rank
                            else if (currentEntryMaxRank - rank.Value == 1 || rank.Value == ranks.Length)
                            {
                                PhotoContestRank newRank = new PhotoContestRank(CurrentUserSession.Username,
                                                                                contestId, nonpick.Id, rank.Value + 1);
                                newRank.Save();
                                currentEntry = null;
                                currentEntryMinRank = null;
                                currentEntryMaxRank = null;
                                loadPersonalFavs();
                            }
                            else
                            {
                                // Limit the minimum rank
                                currentEntryMinRank = rank.Value;
                            }
                            break;
                        }
                    }
                }
            }

            #endregion
        }

        protected void btnEnterContest_Click(object sender, EventArgs e)
        {
            Response.Redirect("EnterPhotoContest.aspx?cid=" + contestId);
        }

        protected void btnPickLeft_Click(object sender, EventArgs e)
        {
            rankEntries(leftEntry, rightEntry);
            loadNextEntries();
        }

        protected void btnPickRight_Click(object sender, EventArgs e)
        {
            rankEntries(rightEntry, leftEntry);
            loadNextEntries();
        }

        protected void lnkViewAllFavourites_Click(object sender, EventArgs e)
        {
            DataTable dtFavs = new DataTable();
            dtFavs.Columns.Add("EntryId", typeof (int));
            dtFavs.Columns.Add("PhotoId", typeof (int));
            dtFavs.Columns.Add("Username", typeof (string));
            dtFavs.Columns.Add("Rank", typeof (int));

            PhotoContestRank[] ranks = PhotoContestRank.Load(CurrentUserSession.Username, contestId);
            int count = 0;
            foreach (PhotoContestRank rank in ranks)
            {
                PhotoContestEntry entry = PhotoContestEntry.Load(rank.EntryId);
                dtFavs.Rows.Add(new object[] {rank.EntryId, entry.PhotoId, entry.Username, rank.Value});
                if (++count >= Config.Ratings.FavoriteEntriesCount) break;
            }

            dlAllFavourites.DataSource = dtFavs;
            dlAllFavourites.DataBind();

            divPhotos.Visible = false;
            divViewTopEntries.Visible = false;
            divAllFavourites.Visible = true;
        }

        protected void lnkViewTopEntries_Click(object sender, EventArgs e)
        {
            if (divNotEnoughPhotos.Visible) return;

            DataTable dtTop = new DataTable();
            dtTop.Columns.Add("EntryId", typeof(int));
            dtTop.Columns.Add("PhotoId", typeof(int));
            dtTop.Columns.Add("Username", typeof(string));
            dtTop.Columns.Add("Rank", typeof(int));

            int[] entriesIds = PhotoContestEntry.FetchTop(contestId, Config.Ratings.TopEntriesCount);
            int count = 0;
            foreach (int entryId in entriesIds)
            {
                PhotoContestEntry entry = PhotoContestEntry.Load(entryId);
                if (entry == null) continue;
                dtTop.Rows.Add(new object[] { entryId, entry.PhotoId, entry.Username, ++count });
            }

            dlTopEntries.DataSource = dtTop;
            dlTopEntries.DataBind();

            divPhotos.Visible = false;
            divAllFavourites.Visible = false;
            divViewTopEntries.Visible = true;
            lnkViewTopEntries.Visible = false;
            btnViewTopEntries.Visible = false;
        }

        protected void btnBackToPhotos_Click(object sender, EventArgs e)
        {
            divPhotos.Visible = true;
            divAllFavourites.Visible = false;
            divViewTopEntries.Visible = false;
            lnkViewTopEntries.Visible = true;
            btnViewTopEntries.Visible = true;

            loadNextEntries();
        }

        protected void btnLeaveContest_Click(object sender, EventArgs e)
        {
            PhotoContestEntry entry = PhotoContestEntry.Load(contestId, CurrentUserSession.Username);
            if (entry != null)
            {
                PhotoContestEntry.Delete(entry);

                btnEnterContest.Visible = true;
                lblCurrentRank.Text = "";
                divContestRank.Visible = false;
            }
        }
    }
}