/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Admin
{
    /// <summary>
    /// Summary description for EditQuestion.
    /// </summary>
    public partial class EditQuestion : AdminPageBase
    {
        protected MessageBox MessageBox;
        //protected System.Web.UI.WebControls.Label lblMessage;

        private int questionID;

        private string QuestionID
        {
            get
            {
                string param = Request.Params["qid"];

                if (param == null || param.Trim() == String.Empty)
                {
                    return null;
                }
                else
                {
                    foreach (char ch in param)
                    {
                        if (Char.IsLetter(ch))
                        {
                            return null;
                        }
                    }
                }

                return param;
            }
        }


        private DataTable Choices
        {
            get
            {
                if (Session["QuestionChoices"] == null)
                {
                    DataTable dtChoices = new DataTable("Choices");
                    dtChoices.Columns.Add("ChoiceID");
                    dtChoices.Columns.Add("Value");
                    dtChoices.PrimaryKey = new DataColumn[]
                        {dtChoices.Columns["ChoiceID"]};

                    Session["QuestionChoices"] = dtChoices;

                    ProfileChoice[] choices;
                    choices = ProfileChoice.FetchByQuestionID(questionID);

                    if (choices != null)
                    {
                        foreach (ProfileChoice choice in choices)
                        {
                            dtChoices.Rows.Add(new object[] {choice.Id.ToString(), choice.Value});
                        }
                    }
                    return dtChoices;
                }
                else
                {
                    return (DataTable) Session["QuestionChoices"];
                }
            }

            set { Session["QuestionChoices"] = value; }
        }


        private string NewTempID
        {
            get
            {
                if (Session["LastCount"] == null)
                {
                    Session["LastCount"] = 0;
                }

                int id = (int) Session["LastCount"];
                ++id;
                Session["LastCount"] = id;
                return "TempID" + id.ToString();
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (QuestionID == null)
            {
                return;
            }
            questionID = Convert.ToInt32(QuestionID);

            if (!Page.IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnDeleteSelectedChoices.Enabled = false;
                    btnSave.Enabled = false;
                    btnAddNewChoices.Enabled = false;
                }
                LoadStrings();
                PopulateDataFields();
                PopulateDataGrid();
                dropEditStyle_SelectedIndexChanged(null, null);
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (ddCondition.SelectedValue == "-1")
            {
                cblConditionAnswers.Items.Clear();
                pnlConditionAnswers.Visible = false;
                pnlRequired.Visible = true;
            }
            else
            {
                pnlConditionAnswers.Visible = true;
                cbRequired.Checked = false;
                pnlRequired.Visible = false;
            }
        }

        private void LoadStrings()
        {
            cbVisibleToMale.Text = Lang.TransA("Males");
            cbVisibleToFemale.Text = Lang.TransA("Females");
            cbVisibleToCouple.Text = Lang.TransA("Couples");

            cbVisibleToCouple.Visible = Config.Users.CouplesSupport;

            if (Config.Users.DisableGenderInformation)
            {
                cbVisibleToMale.Checked = true;
                cbVisibleToFemale.Checked = true;
                cbVisibleToCouple.Checked = true;
                pnlAppliesTo.Visible = false;
            }

            foreach (string style in Enum.GetNames(typeof (ProfileQuestion.eEditStyle)))
            {
                dropEditStyle.Items.Add(style);
            }
            foreach (string style in Enum.GetNames(typeof (ProfileQuestion.eShowStyle)))
            {
                dropShowStyle.Items.Add(style);
            }
            foreach (string style in Enum.GetNames(typeof (ProfileQuestion.eSearchStyle)))
            {
                dropSearchStyle.Items.Add(style);
            }

            for (int i = 1; i <= 5; ++i)
            {
                dropNewChoicesCount.Items.Add(i.ToString());
            }

            ddMatchFieldQuestion.Items.Add(new ListItem(String.Empty, "-1"));
            ddCondition.Items.Add(new ListItem("Show always".Translate(), "-1"));

            ProfileQuestion[] profileQuestions = ProfileQuestion.Fetch();

            foreach (ProfileQuestion profileQuestion in profileQuestions)
            {
                ddMatchFieldQuestion.Items.Add(new ListItem(profileQuestion.Name, profileQuestion.Id.ToString()));

                if (profileQuestion.Id != Convert.ToInt32(QuestionID)
                        && profileQuestion.SearchStyle != ProfileQuestion.eSearchStyle.RangeChoiceSelect
                        && (profileQuestion.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceSelect
                    || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.SingleChoiceRadio
                    || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceSelect
                    || profileQuestion.EditStyle == ProfileQuestion.eEditStyle.MultiChoiceCheck))
                    ddCondition.Items.Add(new ListItem(profileQuestion.Name, profileQuestion.Id.ToString()));
            }

            btnAddNewChoices.Text = Lang.TransA("Add");
            btnCancel.Text = Lang.TransA("Cancel");
            btnSave.Text = Lang.TransA("Save");

            btnDeleteSelectedChoices.Text = Lang.TransA("Delete selected choices");
            btnDeleteSelectedChoices.Attributes.Add("onclick",
                                                    String.Format("javascript: return confirm('{0}')",
                                                                  Lang.TransA(
                                                                      "Do you really want to delete selected choices?")));
            Choices = null;
        }

        private void PopulateDataFields()
        {
            ProfileQuestion question;
            question = ProfileQuestion.Fetch(questionID);
            txtName.Text = question.Name;
            txtAltName.Text = question.AltName;
            txtDescription.Text = question.Description;
            txtHint.Text = question.Hint;
            dropEditStyle.SelectedValue =
                Enum.GetName(typeof (ProfileQuestion.eEditStyle), question.EditStyle);
            dropShowStyle.SelectedValue =
                Enum.GetName(typeof (ProfileQuestion.eShowStyle), question.ShowStyle);
            dropSearchStyle.SelectedValue =
                Enum.GetName(typeof (ProfileQuestion.eSearchStyle), question.SearchStyle);
            cbRequired.Checked = question.Required;
            cbRequiresApproval.Checked = question.RequiresApproval;
            cbVisibleOnlyForPaidMembers.Checked = question.VisibleForPaidOnly;
            cbEditableOnlyByPaidMembers.Checked = question.EditableForPaidOnly;
            cbVisibleToMale.Checked = question.VisibleForMale;
            cbVisibleToFemale.Checked = question.VisibleForFemale;
            cbVisibleToCouple.Checked = question.VisibleForCouple;
            ddMatchFieldQuestion.SelectedValue = question.MatchField.HasValue
                                                     ? question.MatchField.Value.ToString()
                                                     : "-1";
            ddCondition.SelectedValue = question.ParentQuestionID.HasValue
                                            ? question.ParentQuestionID.Value.ToString()
                                            : "-1";
            if (question.ParentQuestionChoices != null)
            {
                List<string> parentQuestionChoices = new List<string>();
                foreach (string parentQuestionChoice in question.ParentQuestionChoices.Split(':'))
                {
                    parentQuestionChoices.Add(parentQuestionChoice);
                }

                ProfileChoice[] choices = ProfileChoice.FetchByQuestionID(Convert.ToInt32(ddCondition.SelectedValue));
                if (choices != null)
                {
                    foreach (ProfileChoice choice in choices)
                    {
                        cblConditionAnswers.Items.Add(new ListItem(choice.Value, choice.Id.ToString()));

                        if (parentQuestionChoices.Exists(c => c == choice.Value))
                            cblConditionAnswers.Items.FindByValue(choice.Id.ToString()).Selected = true;
                    }
                }
            }
        }

        private void PopulateDataGrid()
        {
            dgChoices.DataSource = Choices;
            dgChoices.DataBind();

            if (Choices.Rows.Count == 0)
            {
                MessageBox.Show(Lang.TransA("There are no existing choices for that question! " +
                                           "Please click on \"Add new choice\" to create new one."), Misc.MessageType.Error);
                dgChoices.Visible = false;
            }
            else
            {
                dgChoices.Visible = true;
            }
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.topicsQuestions;
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dropEditStyle.SelectedIndexChanged += new EventHandler(this.dropEditStyle_SelectedIndexChanged);
            this.btnAddNewChoices.Click += new EventHandler(this.btnAddNewChoices_Click);
            this.btnDeleteSelectedChoices.Click += new EventHandler(this.btnDeleteSelectedChoices_Click);
            this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
            this.btnSave.Click += new EventHandler(this.btnSave_Click);
        }

        #endregion

        private void btnAddNewChoices_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            UpdateDataSource();
            for (int i = 0; i < Convert.ToInt32(dropNewChoicesCount.SelectedValue); ++i)
            {
                Choices.Rows.Add(new object[] {NewTempID, ""});
            }
            Trace.Write("DataTable row count:" + Choices.Rows.Count.ToString());
            PopulateDataGrid();
        }

        private void UpdateDataSource()
        {
            foreach (DataGridItem item in dgChoices.Items)
            {
                HtmlInputCheckBox cbSelect = (HtmlInputCheckBox) item.FindControl("cbSelect");
                TextBox txtValue = (TextBox) item.FindControl("txtValue");

                string id = cbSelect.Value;
                string _value = txtValue.Text;

                Choices.Rows.Find(id)["Value"] = _value;
            }
        }

        private void btnDeleteSelectedChoices_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            UpdateDataSource();

            foreach (DataGridItem item in dgChoices.Items)
            {
                HtmlInputCheckBox cbSelect =
                    (HtmlInputCheckBox) item.FindControl("cbSelect");
                if (cbSelect.Checked)
                {
                    string choiceID = cbSelect.Value;
                    if (choiceID.IndexOf("Temp") == -1)
                    {
                        ProfileChoice.Delete(Convert.ToInt32(choiceID));
                    }

                    Choices.Rows.Find(choiceID).Delete();
                }
            }

            PopulateDataGrid();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            ProfileQuestion question = ProfileQuestion.Fetch(questionID);
            question.Name = txtName.Text;
            question.AltName = txtAltName.Text;
            question.Description = txtDescription.Text;
            question.Hint = txtHint.Text;
            question.EditStyle = (ProfileQuestion.eEditStyle)
                                 Enum.Parse(typeof (ProfileQuestion.eEditStyle), dropEditStyle.SelectedValue);
            question.ShowStyle = (ProfileQuestion.eShowStyle)
                                 Enum.Parse(typeof (ProfileQuestion.eShowStyle), dropShowStyle.SelectedValue);
            question.SearchStyle = (ProfileQuestion.eSearchStyle)
                                   Enum.Parse(typeof (ProfileQuestion.eSearchStyle), dropSearchStyle.SelectedValue);
            question.Required = cbRequired.Checked;
            question.RequiresApproval = cbRequiresApproval.Checked;
            question.VisibleForPaidOnly = cbVisibleOnlyForPaidMembers.Checked;
            question.EditableForPaidOnly = cbEditableOnlyByPaidMembers.Checked;
            question.VisibleForMale = cbVisibleToMale.Checked;
            question.VisibleForFemale = cbVisibleToFemale.Checked;
            question.VisibleForCouple = cbVisibleToCouple.Checked;
            question.MatchField = ddMatchFieldQuestion.SelectedValue != "-1"
                                      ? (int?) Convert.ToInt32(ddMatchFieldQuestion.SelectedValue)
                                      : null;

            foreach (DataGridItem item in dgChoices.Items)
            {
                HtmlInputCheckBox cbSelect =
                    (HtmlInputCheckBox) item.FindControl("cbSelect");
                TextBox txtValue = (TextBox) item.FindControl("txtValue");

                string choiceID = cbSelect.Value;
                string _value = txtValue.Text;

                if (_value.Trim() != String.Empty)
                {
                    ProfileChoice choice;

                    if (choiceID.IndexOf("Temp") == -1)
                    {
                        choice = new ProfileChoice(Convert.ToInt32(choiceID));
                    }
                    else
                    {
                        choice = new ProfileChoice();
                    }

                    choice.QuestionID = questionID;
                    choice.Value = _value;
                    choice.Save();
                }
                Choices = null;
            }

            question.ParentQuestionChoices = null;
            question.ParentQuestionID = null;
            if (ddCondition.SelectedValue != "-1")
            {
                foreach (ListItem item in cblConditionAnswers.Items)
                {
                    if (item.Selected) question.ParentQuestionChoices += item.Text + ":";
                }
                if (question.ParentQuestionChoices != null)
                    question.ParentQuestionChoices = question.ParentQuestionChoices.TrimEnd(':');
                else
                {
                    MessageBox.Show(Lang.TransA("Please select conditional answers!"), Misc.MessageType.Error);
                    return;
                }
                question.ParentQuestionID = Convert.ToInt32(ddCondition.SelectedValue);
            }

            question.Save();

            try
            {
                ProfileQuestion[] questions = null;
                questions = ProfileTopic.Fetch(question.TopicID).FetchQuestions();
                if (questions != null)
                {
                    ProfileQuestion[] childQuestions = questions.Where(q => q.ParentQuestionID.HasValue && q.ParentQuestionID.Value == question.Id).ToArray();
                    if (childQuestions.Length > 0) // this question is parent
                    {
                        ProfileChoice[] parentChoices = question.FetchChoices();
                        if (parentChoices != null)
                        {
                            List<string> lParentChoices = new List<string>();
                            foreach (ProfileChoice parentChoice in parentChoices)
                                lParentChoices.Add(parentChoice.Value);

                            foreach (ProfileQuestion childQuestion in childQuestions)
                            {
                                string[] childParentQuestionChoices = childQuestion.ParentQuestionChoices.Split(new string[] { ":" },
                                                                                      StringSplitOptions.RemoveEmptyEntries);
                                if (childParentQuestionChoices.Any(c => !lParentChoices.Contains(c)))
                                {
                                    childQuestion.ParentQuestionChoices = String.Empty;
                                    var existedChoices =
                                        childParentQuestionChoices.Where(c => lParentChoices.Contains(c));
                                    foreach (string existedChoice in existedChoices)
                                        childQuestion.ParentQuestionChoices += existedChoice + ":";
                                    childQuestion.ParentQuestionChoices = childQuestion.ParentQuestionChoices.TrimEnd(':');
                                    childQuestion.Save();
                                }
                            }
                        }
                    }
                }
            }
            catch (NotFoundException)
            {
            }

            string url = String.Format("EditTopic.aspx?tid={0}", question.TopicID);
            Response.Redirect(url);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Choices = null;
            ProfileQuestion question = ProfileQuestion.Fetch(questionID);
            string url = String.Format("EditTopic.aspx?tid={0}", question.TopicID);
            Response.Redirect(url);
        }

        private void dropEditStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (dropEditStyle.SelectedValue)
            {
                case "Hidden":
                case "Custom":
                case "SingleLine":
                case "MultiLine":
                    MessageBox.Visible = false;
                    pnlAnswers.Visible = false;
                    pnlMatchField.Visible = false;
//                    pnlShowCondition.Visible = false;
//                    pnlConditionAnswers.Visible = false;
//                    ddCondition.SelectedValue = "-1";
                    break;
                default:
                    MessageBox.Visible = true;
                    pnlAnswers.Visible = ddMatchFieldQuestion.SelectedValue == "-1";
                    pnlMatchField.Visible = true;
                    pnlShowCondition.Visible = true;
                    pnlConditionAnswers.Visible = true;
                    PopulateDataGrid();
                    break;
            }
        }

        protected void dropSearchStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
//            switch (dropSearchStyle.SelectedValue)
//            {
//                case "RangeChoiceSelect" :
//                    pnlShowCondition.Visible = false;
//                    pnlConditionAnswers.Visible = false;
//                    ddCondition.SelectedValue = "-1";
//                    break;
//                default:
//                    pnlShowCondition.Visible = true;
//                    pnlConditionAnswers.Visible = true;
//                    break;
//            }
        }

        protected void ddMatchFieldQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlAnswers.Visible = ddMatchFieldQuestion.SelectedValue == "-1";
            pnlSearchStyle.Visible = ddMatchFieldQuestion.SelectedValue == "-1";
            dropSearchStyle.SelectedValue = ProfileQuestion.eSearchStyle.Hidden.ToString();
        }

        protected void ddCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddCondition.SelectedValue != "-1")
            {
                cblConditionAnswers.Items.Clear();
                ProfileChoice[] choices = ProfileChoice.FetchByQuestionID(Convert.ToInt32(ddCondition.SelectedValue));
                foreach (ProfileChoice choice in choices)
                {
                    cblConditionAnswers.Items.Add(new ListItem(choice.Value, choice.Id.ToString()));
                }
            }
        }
    }
}