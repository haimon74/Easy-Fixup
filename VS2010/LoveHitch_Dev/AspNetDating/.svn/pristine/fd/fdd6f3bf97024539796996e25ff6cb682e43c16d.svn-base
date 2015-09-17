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

namespace AspNetDating.Components.Search
{
    /// <summary>
    ///		Summary description for SearchSingleChoice.
    /// </summary>
    public partial class SearchSingleChoice : UserControl, IProfileSearchComponent, ICascadeQuestionComponent
    {

        public string QuestionID
        {
            get { return hidQuestionId.Value; }
            set { hidQuestionId.Value = value; }
        }

        public ProfileQuestion Question
        {
            set
            {
                QuestionID = value.Id.ToString();

                if (Config.Misc.EnableProfileDataTranslation)
                    lblName.Text = Lang.Trans(value.AltName);
                else
                    lblName.Text = value.AltName;

                ProfileChoice[] profileChoices = value.MatchField.HasValue
                                              ? ProfileChoice.FetchByQuestionID(value.MatchField.Value)
                                              : value.FetchChoices();
                if (profileChoices != null)
                {
                    dropValues.Items.Add("");
                    foreach (ProfileChoice choice in profileChoices)
                    {
                        if (Config.Misc.EnableProfileDataTranslation)
                            dropValues.Items.Add(new ListItem(Lang.Trans(choice.Value), choice.Id.ToString()));
                        else
                            dropValues.Items.Add(
                                new ListItem(choice.Value, choice.Id.ToString()));
                    }
                }
            }
        }

        public ProfileAnswer[] Answers
        {
            get
            {
                ProfileAnswer answer = new ProfileAnswer();
                answer.Question = new ProfileQuestion(
                    Convert.ToInt32(QuestionID));

                if (dropValues.SelectedItem.Value.Trim().Length > 0)
                    answer.Value = ProfileChoice.Fetch(Int32.Parse(dropValues.SelectedItem.Value)).Value;// dropValues.SelectedItem.Text;}
                else
                    answer.Value = String.Empty;

                if (answer.Value == "")
                    return new ProfileAnswer[] {};
                else
                    return new ProfileAnswer[] {answer};
            }
        }

        public int[] ChoiceIds
        {
            get { return new int[] {Convert.ToInt32(dropValues.SelectedValue)}; }
            set
            {
                if (value.Length == 0) return;
                foreach (int choiceId in value)
                {
                    ListItem item = dropValues.Items.FindByValue(choiceId.ToString());
                    if (item != null)
                    {
                        dropValues.ClearSelection();
                        item.Selected = true;
                    }
                }
            }
        }

        public HtmlGenericControl UserControlPanel
        {
            get { return pnlID; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // Put user code to initialize the page here
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


            dropValues.Attributes.Add("onchange", js);
        }

        public void GenerateResetValuesJS()
        {
            string js = "<script>";
            js += "function Reset" + pnlID.ClientID + "()" +
                  "{" + "$get('" + dropValues.ClientID + "').selectedIndex = -1;}";
            js += "</script>";

            Page.ClientScript.RegisterStartupScript(GetType(), ID + "Reset", js);
        }

        public void ClearAnswers()
        {
            dropValues.SelectedIndex = -1;
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