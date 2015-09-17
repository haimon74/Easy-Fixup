using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    public partial class ManagePolls : AdminPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStrings();
                PopulatePolls();
            }
        }

        private void LoadStrings()
        {
            btnDelete.Text = "Delete".TranslateA();
            btnDelete.Attributes.Add("onclick", String.Format("javascript: return confirm('{0}')",
                Lang.TransA("Do you really want to delete selected poll?")));

            btnSave.Text = "Save".TranslateA();
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.polls;

            base.OnInit(e);
        }

        private void PopulatePolls()
        {
            ddPolls.Items.Clear();
            Poll[] polls = Poll.Fetch();

            ddPolls.Items.Add(new ListItem("", ""));
            ddPolls.Items.Add(new ListItem("Add new poll".TranslateA(), "-1"));

            foreach (var poll in polls)
            {
                ddPolls.Items.Add(new ListItem(poll.Title, poll.ID.ToString()));
            }

            divPoll.Visible = false;
            btnDelete.Visible = false;
        }

        private void PopulateDataList(PollChoice[] choices)
        {
            DataTable dtChoices = new DataTable();
            dtChoices.Columns.Add("ID");
            dtChoices.Columns.Add("Number");
            dtChoices.Columns.Add("ChoiceValue");

            if (choices == null || choices.Length == 0)
            {
                for (var i=0; i<10;++i)
                {
                    dtChoices.Rows.Add(new object[] { 0, i + 1, String.Empty });
                }
            }
            else
            {
                for (var i=0; i<choices.Length; ++i)
                {
                    dtChoices.Rows.Add(new object[] {choices[i].ID, i + 1, choices[i].Answer});
                }

                for (var i = choices.Length; i < 10; ++i)
                {
                    dtChoices.Rows.Add(new object[] { 0, i + 1, String.Empty });
                }
            }

            dlChoices.DataSource = dtChoices;
            dlChoices.DataBind();
        }

        protected void ddPolls_SelectedIndexChanged(object sender, EventArgs e)
        {
            divPoll.Visible = true;

            if (ddPolls.SelectedValue == "-1")
            {
                txtQuestion.Text = String.Empty;

                dpFromDate.SelectedDate = DateTime.Now;
                dpToDate.SelectedDate = DateTime.Now.AddMonths(1);
                dpShowResultsUntil.SelectedDate = DateTime.Now.AddMonths(2);

                PopulateDataList(null);
                btnDelete.Visible = false;
                return;
            }

            if (ddPolls.SelectedValue != String.Empty)
            {
                btnDelete.Visible = true;
                Poll poll = Poll.Fetch(Int32.Parse(ddPolls.SelectedValue));
                if (poll != null)
                {
                    PollChoice[] choices = PollChoice.FetchByPollID(poll.ID);
                    txtQuestion.Text = poll.Title;
                    PopulateDataList(choices);
                    dpFromDate.SelectedDate = poll.StartDate;
                    dpToDate.SelectedDate = poll.EndDate;
                    dpShowResultsUntil.SelectedDate = poll.ShowResultsUntil;
                }
                return;
            }
            else
            {
                divPoll.Visible = false;
                btnDelete.Visible = false;
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (txtQuestion.Text.Trim().Length == 0)
            {
                MessageBox.Show("The question text field must not be empty!".TranslateA(),
                    Misc.MessageType.Error);
                return;
            }

            int nonEmptyTextFields = 0;
            foreach (DataListItem item in dlChoices.Items)
            {
                TextBox txtChoiceValue = (TextBox)item.FindControl("txtChoiceValue");
                if (txtChoiceValue.Text.Trim().Length > 0)
                    nonEmptyTextFields++;
            }

            if (nonEmptyTextFields < 2)
            {
                MessageBox.Show("The question must have at least 2 possible answers!".TranslateA(),
                    Misc.MessageType.Error);
                return;
            }

            if (dpToDate.SelectedDate.Date < dpFromDate.SelectedDate.Date)
            {
                MessageBox.Show("End Date must be posterior to Start Date!".Translate(),
                    Misc.MessageType.Error);
                return;                
            }

            if (dpShowResultsUntil.SelectedDate.Date < dpToDate.SelectedDate.Date)
            {
                MessageBox.Show("Show Results Until date must be greater than or equal to End Date!".Translate(),
                    Misc.MessageType.Error);
                return;
            }

            Poll poll;
            int pollID;
            if (!Int32.TryParse(ddPolls.SelectedValue, out pollID))
                return;

            if (pollID == -1)
            {
                poll = new Poll(txtQuestion.Text);
                poll.StartDate = dpFromDate.SelectedDate;
                poll.EndDate = dpToDate.SelectedDate;
                poll.ShowResultsUntil = dpShowResultsUntil.SelectedDate;
                poll.Save();
                ddPolls.Items.Add(new ListItem(txtQuestion.Text, poll.ID.ToString()));
                ddPolls.SelectedValue = poll.ID.ToString();
            }
            else
            {
                poll = Poll.Fetch(pollID);

                if (poll == null)
                    return;

                ddPolls.SelectedItem.Text = txtQuestion.Text;
                poll.Title = txtQuestion.Text;
                poll.StartDate = dpFromDate.SelectedDate;
                poll.EndDate = dpToDate.SelectedDate;
                poll.ShowResultsUntil = dpShowResultsUntil.SelectedDate;
                poll.Save();
            }

            foreach (DataListItem item in dlChoices.Items)
            {
                TextBox txtChoiceValue = (TextBox)item.FindControl("txtChoiceValue");
                HiddenField hidID = (HiddenField)item.FindControl("hidID");

                int id;
                if (Int32.TryParse(hidID.Value, out id))
                {
                    if (id > 0)
                    {
                        PollChoice choice = PollChoice.Fetch(id);
                        if (choice != null)
                        {
                            if (txtChoiceValue.Text.Trim().Length > 0)
                            {
                                choice.Answer = txtChoiceValue.Text;
                                choice.Save();
                            }
                            else
                            {
                                PollChoice.Delete(id);
                            }
                        }
                    }
                    else if (id == 0 && txtChoiceValue.Text.Trim().Length > 0)
                    {
                        PollChoice choice = 
                            new PollChoice(poll.ID, txtChoiceValue.Text);

                        choice.Save();
                    }
                }
            }

            var newChoices = PollChoice.FetchByPollID(poll.ID);
            PopulateDataList(newChoices);

            MessageBox.Show("The poll has been saved successfully!".TranslateA(), Misc.MessageType.Success);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            var pollID = Int32.Parse(ddPolls.SelectedValue);

            if (ddPolls.SelectedValue.Length > 0 && Int32.Parse(ddPolls.SelectedValue) > 0)
            {
                Poll.Delete(pollID);
                ddPolls.Items.Remove(ddPolls.Items.FindByValue(pollID.ToString()));
            }

            ddPolls.SelectedValue = String.Empty;
            btnDelete.Visible = false;
            divPoll.Visible = false;
        }
    }
}
