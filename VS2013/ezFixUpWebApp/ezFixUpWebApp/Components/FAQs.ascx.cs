using System;
using System.Data;
using System.Linq;
using System.Web;
using ezFixUp.Classes;

namespace ezFixUp.Components
{
    public partial class FAQs : System.Web.UI.UserControl
    {
        public int CategoryId { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            bindFAQs();
        }

        private void bindFAQs()
        {
            DataTable dtFAQs = new DataTable("FAQs");
            dtFAQs.Columns.Add("Question");
            dtFAQs.Columns.Add("Answer");

            FAQ[] faqs = FAQ.FetchAll(CategoryId).Where(f => f.Active).ToArray();

            foreach (FAQ faq in faqs)
            {
                string question = Server.HtmlEncode(faq.QuestionDisplay ?? String.Empty);
                string answer = Server.HtmlEncode(faq.AnswerDisplay ?? String.Empty);

                dtFAQs.Rows.Add(new object[] { question, answer });
            }

            //dtFAQs.DefaultView.Sort = "CategoryId DESC";

            dlFAQsPerCategory.DataSource = dtFAQs;
            dlFAQsPerCategory.DataBind();
        }
        protected override void OnUnload(EventArgs e)
        {
            //HttpContext.Current.Response.Flush();
            base.OnUnload(e);
        }
    }
}