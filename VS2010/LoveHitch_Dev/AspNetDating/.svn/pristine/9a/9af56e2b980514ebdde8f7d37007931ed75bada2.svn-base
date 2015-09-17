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
    public partial class EditMultiChoiceCheck : UserControl, IProfileQuestionComponent, ICascadeQuestionComponent
    {
        protected Label Label;

        private bool adminMode;

        public bool AdminMode
        {
            set
            {
                adminMode = value;
                if (value)
                {
                    //lblDescription.CssClass = "font_css";
                    //lblName.CssClass = "font_css";
                    //cbValues.CssClass = "font_css";
                    if (tdAdmin != null)
                        tdAdmin.Visible = true;
                    if (tdUser != null)
                        tdUser.Visible = false;
                    if (pnlAdmin != null)
                        pnlAdmin.Visible = true;
                    if (pnlUser != null)
                        pnlUser.Visible = false;
                }
                else
                {
                    if (tdAdmin != null)
                        tdAdmin.Visible = false;
                    if (tdUser != null)
                        tdUser.Visible = true;
                    if (pnlAdmin != null)
                        pnlAdmin.Visible = false;
                    if (pnlUser != null)
                        pnlUser.Visible = true;
                }
            }

            get { return adminMode; }
        }

        public string QuestionID
        {
            get { return hidQuestionId.Value; }
            set { hidQuestionId.Value = value; }
        }

        public string Description
        {
            get { return lblDescription.Text; }
            set
            {
                pnlDescription.Visible = value != null && value.Length > 0;
                lblDescription.Text = value;
            }
        }

        public string Hint
        {
            get { return lblHint.Text; }
            set
            {
                pnlHint.Visible = value != null && value.Length > 0;
                lblHint.Text = value;
            }
        }

        public string Value
        {
            get
            {
                string ret = "";
                foreach (ListItem item in cbValues.Items)
                {
                    if (item.Selected)
                        ret += item.Value + ":";
                }
                return ret.TrimEnd(':');
            }
            set
            {
                foreach (string val in value.Split(':'))
                {
                    ListItem item = cbValues.Items.FindByValue(val);
                    if (item != null)
                        item.Selected = true;
                }
            }
        }

        private User user;
        protected HtmlGenericControl pnlUser;
        protected HtmlGenericControl pnlAdmin;

        public User User
        {
            set { user = value; }
            get { return user; }
        }

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

                ProfileChoice[] choices = value.MatchField.HasValue
                                              ? ProfileChoice.FetchByQuestionID(value.MatchField.Value)
                                              : value.FetchChoices();
                if (choices != null)
                {
                    foreach (ProfileChoice choice in choices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                        {
                            cbValues.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Value));                            
                        }
                        else
                            cbValues.Items.Add(choice.Value);
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
        }

        public ProfileAnswer Answer
        {
            get
            {
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

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void GenerateJSForChildVisibility(Dictionary<string, object[]> dChildClientIDsWithParentQuestionChoices)
        {
            bool translate = Config.Misc.EnableProfileDataTranslation;
            string js = "<script>";
            for (int i = 0; i < cbValues.Items.Count; i++)
            {
                js += "$get('" + cbValues.ClientID + "_" + i + "')" + ".onclick = new Function(\"";

                foreach (var pair in dChildClientIDsWithParentQuestionChoices)
                {
                    string childClientID = pair.Key;
                    string[] parentQuestionChoices = (string[]) pair.Value[0];
                    List<string> childQuestionsClientIDs = (List<string>)pair.Value[1];

                    js += "if(";

                    for (int j = 0; j < cbValues.Items.Count; j++)
                    {
                        js += "($get('" + cbValues.ClientID + "_" + j + "').checked && (";

                        for (int k = 0; k < parentQuestionChoices.Length; k++)
                        {
                            string parentQuestionChoice = parentQuestionChoices[k];
                            if (translate)
                                parentQuestionChoice =
                                    parentQuestionChoices[k].Translate().Replace("\\", "\\\\").Replace("'", "\\\\'").
                                        Replace("\"", "\\\"");
                            js += "$($get('" + cbValues.ClientID + "_" + j + "')).next('label').text() == '" +
                                  parentQuestionChoice + "'" +
                                  (k == parentQuestionChoices.Length - 1 ? String.Empty : " || ");
                        }

                        js += ")) " + (j == cbValues.Items.Count - 1 ? String.Empty : " || ");
                    }

                    js += ") {$get('" + childClientID + "').style.display = '';} else {$get('" + childClientID +
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
                  "{" + "$('#" + cbValues.ClientID + " tr td input').each(function(){ this.checked = false;}); }";
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