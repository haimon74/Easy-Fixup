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
    public partial class EditSingleChoiceSelect : UserControl, IProfileQuestionComponent, ICascadeQuestionComponent
    {

        private bool adminMode;

        public bool AdminMode
        {
            set
            {
                adminMode = value;
                if (value)
                {
                    //lblName.CssClass = "font_css";
                    //dropValue.CssClass = "font_css";
                }
            }

            get { return adminMode; }
        }

        public string QuestionID
        {
            get { return hidQuestionId.Value; }
            set { hidQuestionId.Value = value; }
        }

        public string Name
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public string Value
        {
            get { return dropValue.SelectedValue; }
            set
            {
                try
                {
                    dropValue.SelectedValue = value;
                }
                catch
                {
                    // No suitable value was found; use default
                }
            }
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

        public ProfileQuestion Question
        {
            set
            {
                QuestionID = value.Id.ToString();
                required = value.Required;

                dropValue.Items.Add("");

                ProfileChoice[] choices = value.MatchField.HasValue
                                              ? ProfileChoice.FetchByQuestionID(value.MatchField.Value)
                                              : value.FetchChoices();
                if (choices != null)
                {
                    foreach (ProfileChoice choice in choices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                            dropValue.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Value));
                        else
                            dropValue.Items.Add(choice.Value);
                    }
                }

                if (Config.Misc.EnableProfileDataTranslation)
                    Name = Lang.Trans(value.Name);
                else
                    Name = value.Name;

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
                    var scriptManager = ScriptManager.GetCurrent(Page);
                    if (required && !adminMode && (scriptManager == null || !scriptManager.IsInAsyncPostBack))
                        throw new AnswerRequiredException(Name);
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
            string js = "javascript:";

            foreach (var pair in dChildClientIDsWithParentQuestionChoices)
            {
                string childClientID = pair.Key;
                string[] parentQuestionChoices = (string[]) pair.Value[0];
                List<string> childQuestionsClientIDs = (List<string>)pair.Value[1];

                js += "if(";
                for (int i = 0; i < parentQuestionChoices.Length; i++)
                {
                    string parentQuestionChoice = parentQuestionChoices[i];
                    if (translate)
                        parentQuestionChoice =
                            parentQuestionChoices[i].Translate().Replace("\\", "\\\\").Replace("'", "\\\\'").
                                Replace("\"", "\\\"");
                    js += "$('#'+this.id+' option:selected').text() == '" + parentQuestionChoice + "'" +
                          (i == parentQuestionChoices.Length - 1 ? String.Empty : " || ");
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


            dropValue.Attributes.Add("onChange", js);
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$get('" + dropValue.ClientID +"').selectedIndex = -1;}";
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