using System;
using System.Data;
using System.Web.UI.WebControls;
using AspNetDating.Classes;
using System.Globalization;

namespace AspNetDating.Admin
{
    public partial class EditFAQs : AdminPageBase
    {
        private int? QuestionId
        {
            get { return ViewState["QuestionId"] as int?; }
            set { ViewState["QuestionId"] = value; }
        }

        private DataTable DataSource
        {
            get { return ViewState["EditFAQsDataSource"] as DataTable; }
            set { ViewState["EditFAQsDataSource"] = value; }
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
                DataTable dt = FAQ.FetchCategories();
                ddlCategory.DataMember = "Text";
                ddlCategory.DataTextField = "Text";
                ddlCategory.DataValueField = "Value";
                ddlCategory.DataSource = dt;
                ddlCategory.DataBind();
                PopulateDataGrid(int.Parse(dt.Rows[0]["Value"].ToString()));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (CurrentAdminSession != null)
                Privileges = CurrentAdminSession.Privileges.FAQs;
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
        
        private void PopulateDataGrid(int categoryId)
        {
            DataTable dtQuestions = new DataTable("Questions");
            
            dtQuestions.Columns.Add("QuestionID");
            dtQuestions.Columns.Add("CategoryID");
            dtQuestions.Columns.Add("QuestionName");
            dtQuestions.Columns.Add("QuestionDisplay");
            dtQuestions.Columns.Add("AnswerDisplay");
            dtQuestions.Columns.Add("IsActive", typeof(bool));

            FAQ[] questions = FAQ.FetchAll(categoryId);

            foreach (FAQ question in questions)
            {
                dtQuestions.Rows.Add(new object[]
                                         {
                                             question.Id,
                                             question.CategoryId,
                                             question.Name,
                                             question.QuestionDisplay,
                                             question.AnswerDisplay,
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
                    FAQ.ChangeOrder(id, eDirections.Up);
                    break;
                case "MoveDown":
                    FAQ.ChangeOrder(id, eDirections.Down);
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
            txtAnswerDisplay.Text = String.Empty;
            
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
            int catId = int.Parse(ddlCategory.SelectedValue);
            if (QuestionId == null)
            {
                FAQ question = 
                    FAQ.Create(txtQuestionName.Text, 
                               txtQuestionDisplay.Text, 
                               txtAnswerDisplay.Text, 
                               cbActive.Checked,
                               catId
                               );
                question.Save();
                Language[] languages = Language.FetchAll();
                foreach (Language language in languages)
                {
                    Translation.SaveTranslation(language.Id, txtQuestionName.Text + "_Q", txtQuestionDisplay.Text, false);
                    Translation.SaveTranslation(language.Id, txtQuestionName.Text + "_A", txtAnswerDisplay.Text, false);
                }
            }
            else
            {
                FAQ aQuestion = FAQ.Fetch(QuestionId.Value);
                //aQuestion.Name = txtQuestionName.Text;
                aQuestion.QuestionDisplay = txtQuestionDisplay.Text;
                aQuestion.AnswerDisplay = txtAnswerDisplay.Text;
                aQuestion.Active = cbActive.Checked;
                aQuestion.CategoryId = catId;
                aQuestion.Save();

                QuestionId = null;
            }
            PopulateDataGrid(catId);
            pnlQuestion.Visible = false;
            pnlQuestions.Visible = true;
        }
        
        protected void dgQuestions_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            int questionId = Convert.ToInt32(e.CommandArgument);
            int catId = int.Parse(ddlCategory.SelectedValue);

            if (e.CommandName == "EditQuestion")
            {
                FAQ question = FAQ.Fetch(questionId);
                pnlQuestion.Visible = true;
                pnlQuestions.Visible = false;
                txtQuestionName.Text = question.Name;
                cbActive.Checked = question.Active;
                txtQuestionDisplay.Text = question.QuestionDisplay;
                txtAnswerDisplay.Text = question.AnswerDisplay;
                QuestionId = question.Id;
            }
            else if (e.CommandName == "DeleteQuestion")
            {
                if (!HasWriteAccess)
                    return;
                
                FAQ.Delete(Convert.ToInt32(e.CommandArgument));
                PopulateDataGrid(catId);
            }
            else
            {
                if (!HasWriteAccess)
                    return;
                
                ChangeOrder(e, questionId);
                PopulateDataGrid(catId);
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

        protected void ddlCategory_IndexChanged(object sender, EventArgs e)
        {
            PopulateDataGrid(int.Parse(ddlCategory.SelectedValue));
        }
    }
}
