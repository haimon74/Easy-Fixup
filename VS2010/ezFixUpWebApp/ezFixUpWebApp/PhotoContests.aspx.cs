using System;
using System.Data;
using ezFixUp.Classes;

namespace ezFixUp
{
    public partial class PhotoContestsPage : PageBase
    {
        public PhotoContestsPage()
        {
            RequiresAuthorization = true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadStrings();
                loadContests(true, false);
            }
        }

        private void loadStrings()
        {
            SmallBoxStart1.Title = Lang.Trans("Actions");
            LargeBoxStart1.Title = Lang.Trans("Photo Contests");
            lnkViewActiveContests.Text = Lang.Trans("Active Contests");
            lnkViewPastContests.Text = Lang.Trans("Past Contests");
        }

        private void loadContests(bool hidePast, bool hideActive)
        {
            #region Load contests

            PhotoContest[] contests = PhotoContest.Load(null);

            DataTable dtContests = new DataTable();
            dtContests.Columns.Add("ContestId", typeof (int));
            dtContests.Columns.Add("Name", typeof (string));
            dtContests.Columns.Add("Description", typeof (string));
            dtContests.Columns.Add("Entries", typeof (int));
            dtContests.Columns.Add("TopPhotoId1", typeof (int));
            dtContests.Columns.Add("TopPhotoId2", typeof(int));
            dtContests.Columns.Add("TopPhotoId3", typeof(int));

            foreach (PhotoContest contest in contests)
            {
                if (hidePast && contest.DateEnds.HasValue && contest.DateEnds.Value < DateTime.Now) continue;
                if (hideActive && (!contest.DateEnds.HasValue || contest.DateEnds.Value >= DateTime.Now)) continue;

                int entriesCount = PhotoContestEntry.LoadByContest(contest.Id).Length;
                int[] entriesIds = PhotoContestEntry.FetchTop(contest.Id);
                int topPhotoId1 = 0, topPhotoId2 = 0, topPhotoId3 = 0;
                if (contest.Gender.HasValue && contest.Gender.Value == Classes.User.eGender.Female)
                {
                    topPhotoId1 = -2;
                    topPhotoId2 = -2;
                    topPhotoId3 = -2;
                }
                else if (contest.Gender.HasValue && contest.Gender.Value == Classes.User.eGender.Male)
                {
                    topPhotoId1 = -1;
                    topPhotoId2 = -1;
                    topPhotoId3 = -1;
                }
                try
                {
                    if (entriesIds.Length > 0)
                        topPhotoId1 = PhotoContestEntry.Load(entriesIds[0]).PhotoId;
                    if (entriesIds.Length > 1)
                        topPhotoId2 = PhotoContestEntry.Load(entriesIds[1]).PhotoId;
                    if (entriesIds.Length > 2)
                        topPhotoId3 = PhotoContestEntry.Load(entriesIds[2]).PhotoId;
                }
                catch (Exception err)
                {
                    Global.Logger.LogError(err);
                }

                dtContests.Rows.Add(new object[]
                                        {
                                            contest.Id, contest.Name, contest.Description, entriesCount,
                                            topPhotoId1, topPhotoId2, topPhotoId3
                                        });
            }

            rptContests.DataSource = dtContests;
            rptContests.DataBind();

            #endregion
        }

        protected void lnkViewActiveContests_Click(object sender, EventArgs e)
        {
            lnkViewPastContests.Enabled = true;
            lnkViewActiveContests.Enabled = false;
            loadContests(true, false);
        }

        protected void lnkViewPastContests_Click(object sender, EventArgs e)
        {
            lnkViewPastContests.Enabled = false;
            lnkViewActiveContests.Enabled = true;
            loadContests(false, true);
        }
    }
}