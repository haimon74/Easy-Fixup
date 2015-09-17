using System;
using System.Collections;
using System.Drawing;
using System.Web.Caching;
using System.Web.UI.DataVisualization.Charting;
using ezFixUp.Classes;
using System.Collections.Generic;
using Title=System.Web.UI.DataVisualization.Charting.Title;

namespace ezFixUp.Components.Groups
{
    public partial class GroupPoll : System.Web.UI.UserControl
    {
        public int GroupID
        {
            get
            {
                if (ViewState["CurrentGroupId"] != null)
                {
                    return (int)ViewState["CurrentGroupId"];
                }
                else
                {
                    throw new Exception("The field groupID is not set!");
                }
            }
            set
            {
                ViewState["CurrentGroupId"] = value;
            }
        }

        public int TopicID
        {
            get
            {
                if (ViewState["CurrentTopicID"] != null)
                {
                    return (int)ViewState["CurrentTopicID"];
                }
                else
                {
                    Visible = false;
                    return -1;
                    //                    throw new Exception("The topic ID is not set!");
                }
            }
            set { ViewState["CurrentTopicID"] = value; }
        }

        protected Group CurrentGroup
        {
            get
            {
                if (Page is ShowGroupTopics)
                {
                    return ((ShowGroupTopics)Page).Group;
                }
                else
                {
                    return Group.Fetch(GroupID);
                }
            }
        }

        public GroupTopic CurrentTopic
        {
            get
            {
                int topicID;
                if (Int32.TryParse((Request.Params["tid"]), out topicID) && ViewState["CurrentTopic"] == null)
                {
                    ViewState["CurrentTopic"] = GroupTopic.Fetch(topicID);
                }

                //                if (ViewState["CurrentTopic"] == null)
                //                {
                //                    ((PageBase) Page).StatusPageMessage = Lang.Trans("This topic doesn't exist!");
                //                    Response.Redirect("~/ShowStatus.aspx");
                //                }

                return ViewState["CurrentTopic"] as GroupTopic;
            }
        }

        public GroupMember CurrentGroupMember
        {
            get
            {
                if (Page is ShowGroupTopics)
                {
                    return ((ShowGroupTopics)Page).CurrentGroupMember;
                }
                else if (CurrentUserSession != null)
                {
                    return GroupMember.Fetch(GroupID, CurrentUserSession.Username);
                }
                else
                {
                    return null;
                }
            }
        }

        private UserSession CurrentUserSession
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

            if ((bool)Page.Items["ShowChoices"] && (CurrentUserSession != null && !GroupTopic.HasVoted(CurrentUserSession.Username, TopicID)) && !CurrentTopic.Locked)
            {
                loadChoices();
                mvPoll.SetActiveView(viewChoices);
            }
            else
            {
                calculateResults();
                mvPoll.SetActiveView(viewResults);
            }
        }

        private void loadStrings()
        {
            btnVote.Text = Lang.Trans("Vote");
        }

        private void loadChoices()
        {
            GroupPollsChoice[] groupPollsChoices = GroupPollsChoice.FetchByGroupTopic(TopicID);

            if (groupPollsChoices.Length > 0)
            {
                SortedList<int, string> slChoices = new SortedList<int, string>();

                foreach (GroupPollsChoice choice in groupPollsChoices)
                {
                    slChoices.Add(choice.ID, choice.Answer);
                }

                rblChoices.DataSource = slChoices;
                rblChoices.DataValueField = "Key";
                rblChoices.DataTextField = "Value";
                rblChoices.DataBind();
            }
            else
            {
                rblChoices.Visible = false;
            }
        }

        private void calculateResults()
        {
            ChartPollResults.Titles["Title1"].Text = "Results".Translate();
            ChartPollResults.Series["Default"]["PieLabelStyle"] = "Outside";

            GroupPollsChoice[] groupPollsChoices = GroupPollsChoice.FetchByGroupTopic(TopicID);
            if (groupPollsChoices.Length > 0)
            {
                Hashtable groupPollsAnswers = GroupPollsAnwer.GetNumberOfAnswers(TopicID);
                ArrayList answers = new ArrayList();

                foreach (GroupPollsChoice choice in groupPollsChoices)
                {
                    int numberOfAnswers = groupPollsAnswers.ContainsKey(choice.ID)
                                              ? (int)groupPollsAnswers[choice.ID]
                                              : 0;
                    answers.Add(new Answer(choice.ID, choice.Answer, numberOfAnswers));
                }

                answers.Sort();
                answers.Reverse();

                List<string> xValues = new List<string>();
                List<int> yValues = new List<int>();
                foreach (Answer answer in answers)
                {
                    if (answer.NumberOfAnswers == 0) continue;

                    xValues.Add(answer.AnswerText.Replace(",", ""));
                    yValues.Add(answer.NumberOfAnswers);
                }

                ChartPollResults.Series["Default"].Points.DataBindXY(xValues, yValues);
            }
        }

        private struct Answer : IComparable
        {
            private int choiceID;
            private string answerText;
            private int numberOfAnswers;

            public Answer(int choiceID, string answerText, int numberOfAnswers)
            {
                this.choiceID = choiceID;
                this.answerText = answerText;
                this.numberOfAnswers = numberOfAnswers;
            }

            public int ChoiceID
            {
                get { return choiceID; }
                set { choiceID = value; }
            }

            public string AnswerText
            {
                get { return answerText; }
                set { answerText = value; }
            }

            public int NumberOfAnswers
            {
                get { return numberOfAnswers; }
                set { numberOfAnswers = value; }
            }

            #region IComparable Members

            public int CompareTo(object obj)
            {
                if (obj == null)
                {
                    return 1;
                }

                Answer answer = (Answer)obj;
                return this.numberOfAnswers.CompareTo(answer.numberOfAnswers);
            }

            #endregion
        }

        protected void btnVote_Click(object sender, EventArgs e)
        {
            if (CurrentUserSession != null && !String.IsNullOrEmpty(rblChoices.SelectedValue))
            {
                int choiceID = Convert.ToInt32(rblChoices.SelectedValue);
                GroupPollsAnwer answer = new GroupPollsAnwer(TopicID, CurrentUserSession.Username, choiceID);

                answer.Save();
            }
        }
    }
}