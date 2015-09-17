using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.WebParts
{
    public partial class PollWebPart : WebPartUserControl
    {
        private int CurrentPollID
        {
            get
            {
                return (int)ViewState["CurrentPollID"];
            }

            set
            {
                ViewState["CurrentPollID"] = value;
            }
        }

        private bool FirstLoad
        {
            get
            {
                if (ViewState["FirstLoad"] == null)
                {
                    ViewState["FirstLoad"] = false;
                    return true;
                }

                return false;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = Lang.Translate("Polls");
            fbVote.Text = "Vote".Translate();
            fbResults.Text = "Results".Translate();
            fbBack.Text = "Back".Translate();
        }

        private bool ShowPoll(int? id)
        {
            mvPolls.SetActiveView(vPoll);

            Poll poll;

            if (id.HasValue)
                poll = Poll.Fetch(id.Value);
            else
                poll = Poll.FetchRandom(false, ((PageBase) Page).CurrentUserSession.Username);

            if (poll != null)
            {
                CurrentPollID = poll.ID;
                lblQuestion.Text = poll.Title;
                PopulatePollRepeater(poll.ID);
                return true;
            }

            return false;
        }

        private bool ShowResults(int? pollID, bool showBackButton)
        {
            mvPolls.SetActiveView(vPollResults);

            fbBack.Visible = showBackButton;

            Poll poll;

            if (pollID.HasValue)
                poll = Poll.Fetch(pollID.Value);
            else
                poll = Poll.FetchRandom(true, ((PageBase)Page).CurrentUserSession.Username);

            if (poll != null)
            {
                lblQuestion2.Text = poll.Title;
                PopulateResultsRepeater(poll.ID);
                return true;
            }

            return false;
        }

        private void PopulatePollRepeater(int pollID)
        {
            PollChoice[] choices = PollChoice.FetchByPollID(pollID);

            DataTable dtChoices = new DataTable();
            dtChoices.Columns.Add("ID");
            dtChoices.Columns.Add("ChoiceValue");

            foreach (var choice in choices)
            {
                dtChoices.Rows.Add(new object[] {choice.ID, choice.Answer});
            }

            rptPoll.DataSource = dtChoices;
            rptPoll.DataBind();
        }

        private void PopulateResultsRepeater(int pollID)
        {
            Dictionary<int,int> results = Poll.FetchResults(pollID);

            DataTable dtResults = new DataTable();
            dtResults.Columns.Add("ChoiceValue");
            dtResults.Columns.Add("Percentage", typeof(double));
            dtResults.Columns.Add("Votes", typeof(int));

            int totalVotes = results.Sum(r => r.Value);
            lblTotalVotes.Text = String.Format("Total Votes: {0}".Translate(), totalVotes);

            foreach (var pair in results)
            {
                int choiceID = pair.Key;
                var choice = PollChoice.Fetch(choiceID);
                string answer = String.Empty;
                if (choice != null)
                    answer = choice.Answer;
 
                int votes = pair.Value;
                double percentage = .0;
                
                if (totalVotes > 0)
                    percentage = (double)votes / totalVotes;

                dtResults.Rows.Add(new object[] { answer, percentage, votes });
            }

            rptResults.DataSource = dtResults;
            rptResults.DataBind();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (FirstLoad)
            {
                bool pollShowed = ShowPoll(null);
                if (!pollShowed)
                {
                    if (!ShowResults(null, false))
                        mvPolls.SetActiveView(vNoPolls);
                }
            }
        }

        protected void fbVote_Click(object sender, EventArgs e)
        {
            string username = ((PageBase)Page).CurrentUserSession.Username;

            foreach (RepeaterItem item in rptPoll.Items)
            {
                var radioButton = (GroupRadioButton) item.FindControl("rbChoice");
                var hidID = (HiddenField)item.FindControl("hidID");
                int choiceID = Int32.Parse(hidID.Value);

                if (radioButton.Checked)
                {
                    if (!Poll.IsAnswered(CurrentPollID, username))
                    {
                        Poll.AddAnswer(CurrentPollID, username, choiceID);
                        break;
                    }
                }
            }

            ShowResults(CurrentPollID, false);
        }

        protected void fbResults_Click(object sender, EventArgs e)
        {
            ShowResults(CurrentPollID, true);
        }

        protected void fbBack_Click(object sender, EventArgs e)
        {
            ShowPoll(CurrentPollID);
        }
    }
}