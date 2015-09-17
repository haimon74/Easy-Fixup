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
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    /// <summary>
    ///		Summary description for EditSingleChoiceRadio.
    /// </summary>
    public partial class EditSingleChoiceRadio : UserControl, IProfileQuestionComponent, ICascadeQuestionComponent
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
        }

        private bool adminMode;

        public bool AdminMode
        {
            set
            {
                adminMode = value;
                if (value)
                {
                    //lblName.CssClass = "font_css";
                    //rbValues.CssClass = "font_css";
                    pnlAdmin.Visible = true;
                    pnlUser.Visible = false;
                }
                else
                {
                    pnlAdmin.Visible = false;
                    pnlUser.Visible = true;
                }
            }

            get { return adminMode; }
        }

        public string Description
        {
            get { return lblDescription.Text; }
            set { lblDescription.Text = value; }
        }

        public string Hint
        {
            get { return lblHint.Text; }
            set { lblHint.Text = value; }
        }

        public string QuestionID
        {
            get { return hidQuestionId.Value; }
            set { hidQuestionId.Value = value; }
        }

        private User user;

        public User User
        {
            set { user = value; }
            get { return user; }
        }

        public string Value
        {
            get { return rbValues.SelectedValue; }
            set
            {
                try
                {
                    rbValues.SelectedValue = value;
                }
                catch
                {
                    // No suitable value was found; use default
                }
            }
        }

        //public string Name
        //{
        //    get { return lblName.Text; }
        //    set { lblName.Text = value; }
        //}

        private bool required;

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        public ProfileQuestion Question
        {
            set
            {
                QuestionID = value.Id.ToString();
                required = value.Required;

                if (!required)
                    rbValues.Items.Add("");

                ProfileChoice[] choices = value.MatchField.HasValue
                                              ? ProfileChoice.FetchByQuestionID(value.MatchField.Value)
                                              : value.FetchChoices();
                if (choices != null)
                {
                    foreach (ProfileChoice choice in choices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                            rbValues.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Value));
                        else
                            rbValues.Items.Add(choice.Value);
                    }
                }

                if (Config.Misc.EnableProfileDataTranslation)
                {
                    hlName.Title = Lang.Trans(value.Name);
                    Description = Lang.Trans(value.Description);
                    Hint = Lang.Trans(value.Hint);
                }
                else
                {
                    hlName.Title = value.Name;
                    Description = value.Description;
                    Hint = value.Hint;
                }

                try
                {
                    ProfileAnswer answer =
                        value.FetchAnswer(User.Username);
                    Value = answer.Value;
                }
                catch (NotFoundException)
                {
                    // The user has't answered yet
                }
            }
        }

        public ProfileAnswer Answer
        {
            get
            {
                // should be changed if the first value is not empty string
                ProfileAnswer answer = new ProfileAnswer(
                    User.Username,
                    Convert.ToInt32(hidQuestionId.Value));

                if (Value.Trim().Length == 0)
                {
                    if (required && !adminMode)
                        throw new AnswerRequiredException(hlName.Title);
                    else
                    {
                        answer.Value = "";
                        return answer;
                    }
                }

                answer.Value = Value;
                return answer;
            }
        }

        public HtmlGenericControl UserControlPanel
        {
            get { return pnlID; }
        }

        public void GenerateJSForChildVisibility(Dictionary<string, object[]> dChildClientIDsWithParentQuestionChoices)
            {
            bool translate = Config.Misc.EnableProfileDataTranslation;
            string js = "<script>";

            for (int i = 0; i < rbValues.Items.Count; i++)
            {
                js += "$get('" + rbValues.ClientID + "_" + i + "')" + ".onclick = new Function(\"";

                foreach (var pair in dChildClientIDsWithParentQuestionChoices)
                {
                    string childClientID = pair.Key;
                    string[] parentQuestionChoices = (string[]) pair.Value[0];
                    List<string> childQuestionsClientIDs = (List<string>) pair.Value[1];

                    js += "if(this.checked && (";

                    for (int j = 0; j < parentQuestionChoices.Length; j++)
                    {
                        string parentQuestionChoice = parentQuestionChoices[j];
                        if (translate)
                            parentQuestionChoice =
                                parentQuestionChoices[j].Translate().Replace("\\", "\\\\").Replace("'", "\\\\'").
                                    Replace("\"", "\\\"");
                        js += "$($get('" + rbValues.ClientID + "_" + i + "')).next('label').text() == '" + parentQuestionChoice + "'" +
                              (j == parentQuestionChoices.Length - 1 ? String.Empty : " || ");
            }

                    js += ")) {$get('" + childClientID + "').style.display = '';} else {$get('" + childClientID +
                          "').style.display = 'none';" + "Reset" + childClientID + "();";
                          foreach (string childQuestionClientID in childQuestionsClientIDs)
                          {
                              js += "$get('" + childQuestionClientID + "').style.display = 'none';" + "Reset" +
                                    childQuestionClientID + "();";
        }
                          
                          js += "}";
                }

                js += "\");";
            }

            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID, js);
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$('#" + rbValues.ClientID + " tr td input').each(function(){ this.checked = false;}); }";
            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID + "Reset", js);
        }

        #region Web Form Designer generated code

        protected override void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        }

        #endregion
    }
}