using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetDating.Classes;

namespace AspNetDating.Components.Profile
{
    public partial class ViewLink : UserControl, IProfileAnswerComponent
    {
        public void LoadAnswer(string Username, int QuestionID)
        {
            ProfileQuestion question = ProfileQuestion.Fetch(QuestionID);
            if (Config.Misc.EnableProfileDataTranslation)
                lblName.Text = Lang.Trans(question.AltName);
            else
                lblName.Text = question.AltName;

            try
            {
                ProfileAnswer answer = ProfileAnswer.Fetch(Username, QuestionID);
                if (answer.Value == String.Empty)
                    lnkValue.InnerText = Lang.Trans("-- no answer --");
                else
                {
                    if (answer.Approved)
                    {
                        string sAnswer = answer.Value;
                        if (!sAnswer.StartsWith("https://")) sAnswer = sAnswer.Insert(0, "https://");
                        lnkValue.HRef = sAnswer;
                        lnkValue.InnerText = sAnswer;
                    }
                    else
                        lnkValue.InnerText = Lang.Trans("-- pending approval --");
                }
            }
            catch (NotFoundException)
            {
                lnkValue.InnerText = Lang.Trans("-- no answer --");
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}