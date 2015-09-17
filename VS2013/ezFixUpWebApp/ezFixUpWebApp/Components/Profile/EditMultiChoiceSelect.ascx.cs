/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using ezFixUp.Classes;

namespace ezFixUp.Components.Profile
{
    /// <summary>
    ///		Summary description for EditMultiChoiceSelect.
    /// </summary>
    public partial class EditMultiChoiceSelect : UserControl, IProfileQuestionComponent, ICascadeQuestionComponent
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
                    //lbValues.CssClass = "font_css";
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

        private bool required;

        public bool Required
        {
            get { return required; }
            set { required = value; }
        }

        public string Value
        {
            get
            {
                string ret = "";
                foreach (ListItem item in lbValues.Items)
                {
                    if (item.Selected)
                    {
                        if (item.Value == String.Empty)
                            continue;
                        ret += item.Value + ":";
                    }
                }
                return ret.TrimEnd(':');
            }
            set
            {
                foreach (string val in value.Split(':'))
                {
                    ListItem item = lbValues.Items.FindByValue(val);
                    if (item != null)
                        item.Selected = true;
                }
            }
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

        //public string Name
        //{
        //    get { return lblName.Text; }
        //    set { lblName.Text = value; }
        //}

        public ProfileQuestion Question
        {
            set
            {
                QuestionID = value.Id.ToString();
                required = value.Required;

                if (!required)
                    lbValues.Items.Add("");

                ProfileChoice[] choices = value.MatchField.HasValue
                                              ? ProfileChoice.FetchByQuestionID(value.MatchField.Value)
                                              : value.FetchChoices();
                if (choices != null)
                {
                    foreach (ProfileChoice choice in choices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                            lbValues.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Value));
                        else
                            lbValues.Items.Add(choice.Value);
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
            string js =
                String.Format("javascript: var selectedOptions = []; $('#{0} option:selected').each(function(i, selected){{selectedOptions[i] = $(selected).text();}});", lbValues.ClientID);

            js += "var match;";

            foreach (var pair in dChildClientIDsWithParentQuestionChoices)
            {
                string childClientID = pair.Key;
                string[] parentQuestionChoices = (string[]) pair.Value[0];
                List<string> childQuestionsClientIDs = (List<string>)pair.Value[1];

                js += "match = false; for (var j=0; j < selectedOptions.length; ++j){ if(";
                for (int i = 0; i < parentQuestionChoices.Length; ++i)
                {
                    string parentQuestionChoice = parentQuestionChoices[i];
                    if (translate)
                        parentQuestionChoice =
                            parentQuestionChoices[i].Translate().Replace("\\", "\\\\").Replace("'", "\\\\'").
                                Replace("\"", "\\\"");
                    js += "selectedOptions[j] == '" + parentQuestionChoice + "'" +
                          (i == parentQuestionChoices.Length - 1 ? String.Empty : " || ");
                }
                js += "){match = true; break;} } if(match) { $get('" + childClientID +
                      "').style.display = '';} else {$get('" + childClientID + "').style.display = 'none';" + "Reset" +
                      childClientID + "();";
                    foreach (string childQuestionClientID in childQuestionsClientIDs)
                    {
                        js += "$get('" + childQuestionClientID + "').style.display = 'none';" + "Reset" +
                              childQuestionClientID + "();";
                    }

                js += "}";
            }

            lbValues.Attributes.Add("onclick", js);
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$get('" + lbValues.ClientID + "').selectedIndex = -1;}";
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