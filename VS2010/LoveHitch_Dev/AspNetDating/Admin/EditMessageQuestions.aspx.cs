using System;
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using System.Globalization;

namespace AspNetDating.Admin
{
    public partial class EditMessageQuestions : AdminPageBase
    {
        private int? QuestionId
        {
            get { return ViewState["QuestionId"] as int?; }
            set { ViewState["QuestionId"] = value; }
        }

        private DataTable DataSource
        {
            get { return ViewState["EditQuestionsDataSource"] as DataTable; }
            set { ViewState["EditQuestionsDataSource"] = value; }
        }

        private delegate string Parser(string s);
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!HasWriteAccess)
                {
                    btnAdd.Enabled = false;
                    btnSave.Enabled = false;
                }
                
                LoadStrings();
                PopulateDataGrid();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.AskAboutQuestions;
            base.OnInit(e);
        }

        private void LoadStrings()
        {
            btnAdd.Text = Lang.TransA("Add a new question");
            btnSave.Text = Lang.TransA("Save");
            btnCancel.Text = Lang.TransA("Cancel");

            #region Set Grid's Headers
            dgQuestions.Columns[0].HeaderText = Lang.TransA("Question");
            dgQuestions.Columns[1].HeaderText = Lang.TransA("Active");
            dgQuestions.Columns[2].HeaderText = Lang.TransA("Order");
            dgQuestions.Columns[3].HeaderText = Lang.TransA("Commands");
            #endregion         
        }
        
        private void PopulateDataGrid()
        {
            DataTable dtQuestions = new DataTable("Questions");
            
            dtQuestions.Columns.Add("QuestionID");
            dtQuestions.Columns.Add("QuestionName");
            dtQuestions.Columns.Add("QuestionDisplay");
            dtQuestions.Columns.Add("IsActive", typeof(bool));
            
            AskAboutQuestion[] questions = AskAboutQuestion.FetchAll();

            foreach (AskAboutQuestion question in questions)
            {
                dtQuestions.Rows.Add(new object[]
                                         {
                                             question.Id,
                                             question.Name,
                                             question.QuestionDisplay,
                                             question.Active
                                         });
            }

            DataSource = dtQuestions;
            
            dgQuestions.DataSource = dtQuestions;
            dgQuestions.DataBind();
        }

        private void ChangeOrder(DataGridCommandEventArgs e, int id)
        {
            string direction = e.CommandName;

            switch (direction)
            {
                case "MoveUp":
                    AskAboutQuestion.ChangeOrder(id, eDirections.Up);
                    break;
                case "MoveDown":
                    AskAboutQuestion.ChangeOrder(id, eDirections.Down);
                    break;
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;
            
            txtQuestionName.Text = "";
            cbActive.Checked = false;
            txtQuestionDisplay.Text = String.Empty;
            
            pnlQuestions.Visible = false;
            pnlQuestion.Visible = true;
            
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlQuestions.Visible = true;
            pnlQuestion.Visible = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasWriteAccess)
                return;

            if (QuestionId == null)
            {
                AskAboutQuestion question = 
                    AskAboutQuestion.Create(txtQuestionName.Text, 
                                            txtQuestionDisplay.Text, 
                                            cbActive.Checked);
                question.Save();
            }
            else
            {
                AskAboutQuestion aQuestion = AskAboutQuestion.Fetch(QuestionId.Value);
                aQuestion.Name = txtQuestionName.Text;
                aQuestion.QuestionDisplay = txtQuestionDisplay.Text;
                aQuestion.Active = cbActive.Checked;
                aQuestion.Save();

                QuestionId = null;
            }

            PopulateDataGrid();
            pnlQuestion.Visible = false;
            pnlQuestions.Visible = true;
        }
        
        protected void dgQuestions_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int questionId = Convert.ToInt32(e.CommandArgument);
            
            if (e.CommandName == "EditQuestion")
            {
                AskAboutQuestion question = AskAboutQuestion.Fetch(questionId);
                pnlQuestion.Visible = true;
                pnlQuestions.Visible = false;
                txtQuestionName.Text = question.Name;
                cbActive.Checked = question.Active;
                txtQuestionDisplay.Text = question.QuestionDisplay;
                QuestionId = question.Id;
            }
            else if (e.CommandName == "DeleteQuestion")
            {
                if (!HasWriteAccess)
                    return;
                
                Language.Delete(Convert.ToInt32(e.CommandArgument));
                PopulateDataGrid();
            }
            else
            {
                if (!HasWriteAccess)
                    return;
                
                ChangeOrder(e, questionId);
                PopulateDataGrid();
            }
        }

        protected void dgQuestions_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemIndex == -1)
                return;

            LinkButton lnkUp = e.Item.FindControl("lnkUp") as LinkButton;
            LinkButton lnkDown = e.Item.FindControl("lnkDown") as LinkButton;
            LinkButton lnkDelete = e.Item.FindControl("lnkDelete") as LinkButton;

            if (!HasWriteAccess)
            {
                if (lnkUp != null)
                    lnkUp.Enabled = false;

                if (lnkDown != null)
                    lnkDown.Enabled = false;

                if (lnkDelete != null)
                    lnkDelete.Enabled = false;
            }
        }

        protected void dgQuestions_ItemCreated(object sender, DataGridItemEventArgs e)
        {
            //created item is header or footer
            if (e.Item.ItemIndex == -1)
                return;

            //remove the upper arrow if the current item is the first one
            if (e.Item.ItemIndex == 0)
            {
                (e.Item.FindControl("lnkUp") as LinkButton).Visible = false;
            }

            //remove the lower arrow if the current item is the last one
            int lastItemIndex = DataSource.Rows.Count - 1;
            if (e.Item.ItemIndex == lastItemIndex)
                (e.Item.FindControl("lnkDown") as LinkButton).Visible = false;
        }

        private Parser parse = delegate(string text)
                                   {
                                       return String.Join(",", text.Split(','));
                                   };
    }
}
